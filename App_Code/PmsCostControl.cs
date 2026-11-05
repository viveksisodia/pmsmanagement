using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using CommonClass;

/// <summary>
/// Shared helpers for the PMS Cost Control module (/pms/costcontrol/):
/// login context, access rights and parameterised stored-procedure calls.
///
/// Access rules (requirements 5.1, AP-03):
///   - Company administrators: view, enter, cost codes and access management.
///     They do NOT get automatic approval rights; approval needs "Can approve".
///   - Other employees: only what is granted in Pms_tbl_CostControlAccess.
/// </summary>
public static class PmsCostControl
{
    public enum Need { View, Enter, Approve, Admin }

    public sealed class Context
    {
        public int RegId;
        public int EmployeeId;
        public bool IsAdmin;
        public bool CanView;
        public bool CanEnter;
        public bool CanApprove;

        public bool Allows(Need need)
        {
            switch (need)
            {
                case Need.Admin: return IsAdmin;
                case Need.Approve: return CanApprove;
                case Need.Enter: return IsAdmin || CanEnter;
                default: return IsAdmin || CanView || CanEnter || CanApprove;
            }
        }
    }

    public static int GetCookieInt(string cookieName, string key)
    {
        HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
        int value;
        return cookie != null && Int32.TryParse(cookie[key], out value) ? value : 0;
    }

    /// <summary>Same admin rule as the Issues module (Manage_Profile id 1).</summary>
    public static bool IsAdmin(int regId, int employeeId)
    {
        if (regId <= 0 || employeeId <= 0) return false;
        string sql = "SELECT TOP 1 mp.id FROM dbo.Manage_Profile mp WITH (NOLOCK) " +
                     "INNER JOIN dbo.tbl_employee e WITH (NOLOCK) ON mp.id=TRY_CONVERT(int,e.roleid) " +
                     "WHERE e.id=" + employeeId.ToString(CultureInfo.InvariantCulture) +
                     " AND e.regid=" + regId.ToString(CultureInfo.InvariantCulture) +
                     " AND mp.status=1 AND mp.archive=0";
        try
        {
            DataSet ds = new DataAccess().GetDataSetInline(sql);
            return ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 &&
                   Convert.ToInt32(ds.Tables[0].Rows[0]["id"], CultureInfo.InvariantCulture) == 1;
        }
        catch { return false; }
    }

    /// <summary>Reads the current login and its Cost Control rights. Never throws.</summary>
    public static Context Current()
    {
        var context = new Context
        {
            RegId = GetCookieInt("cadmin", "id"),
            EmployeeId = GetCookieInt("user", "user_id")
        };
        if (context.RegId <= 0 || context.EmployeeId <= 0) return context;
        context.IsAdmin = IsAdmin(context.RegId, context.EmployeeId);
        try
        {
            DataSet ds = RunProcedure("usp_pms_cc_Access_Get",
                new SqlParameter("@regid", context.RegId), new SqlParameter("@EmployeeId", context.EmployeeId));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                context.CanView = Convert.ToInt32(row["CanView"], CultureInfo.InvariantCulture) == 1;
                context.CanEnter = Convert.ToInt32(row["CanEnter"], CultureInfo.InvariantCulture) == 1;
                context.CanApprove = Convert.ToInt32(row["CanApprove"], CultureInfo.InvariantCulture) == 1;
            }
        }
        catch
        {
            // Phase 2 script not installed yet: only administrators (view/enter) can use the module.
        }
        return context;
    }

    /// <summary>Returns the login when it has the required right, otherwise throws 401/403.</summary>
    public static Context Require(Need need)
    {
        Context context = Current();
        if (context.RegId <= 0 || context.EmployeeId <= 0) throw new HttpException(401, "Login information was not found.");
        if (!context.Allows(need))
        {
            string message;
            switch (need)
            {
                case Need.Admin: message = "Only administrators can do this."; break;
                case Need.Approve: message = "You do not have Cost Control approval rights."; break;
                case Need.Enter: message = "You do not have rights to enter Cost Control data."; break;
                default: message = "You do not have access to Cost Control."; break;
            }
            throw new HttpException(403, message);
        }
        return context;
    }

    /// <summary>True when the header menu should show the Cost Control link. Never throws.</summary>
    public static bool ShowMenuLink()
    {
        try { return Current().Allows(Need.View); } catch { return false; }
    }

    public static DataSet RunProcedure(string procedureName, params SqlParameter[] parameters)
    {
        foreach (SqlParameter parameter in parameters)
            if (parameter.Value == null) parameter.Value = DBNull.Value;
        return new DataAccess().GetDataSet(procedureName, parameters);
    }

    public static object DbValue(int? value) { return value.HasValue && value.Value > 0 ? (object)value.Value : DBNull.Value; }
    public static object DbValue(decimal? value) { return value.HasValue ? (object)value.Value : DBNull.Value; }
    public static object DbText(string value) { return String.IsNullOrWhiteSpace(value) ? (object)DBNull.Value : value.Trim(); }

    public static List<Dictionary<string, object>> ToRows(DataSet ds, int tableIndex)
    {
        var rows = new List<Dictionary<string, object>>();
        if (ds == null || ds.Tables.Count <= tableIndex) return rows;
        DataTable table = ds.Tables[tableIndex];
        foreach (DataRow row in table.Rows)
        {
            var item = new Dictionary<string, object>();
            foreach (DataColumn column in table.Columns)
            {
                object value = row[column];
                if (value == DBNull.Value) value = null;
                else if (value is DateTime) value = ((DateTime)value).ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                item[column.ColumnName] = value;
            }
            rows.Add(item);
        }
        return rows;
    }

    public static DateTime ParseBusinessDate(string value, string fieldName)
    {
        DateTime date;
        if (!DateTime.TryParseExact(value ?? "", "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            throw new ArgumentException("Enter " + fieldName + " as a valid date.");
        return date.Date;
    }

    /// <summary>Budget-vs-actual grouping: COSTCODE (default), MILESTONE or COSTTYPE.</summary>
    public static string NormalizeGroupBy(string groupBy)
    {
        string value = (groupBy ?? "").Trim().ToUpperInvariant();
        return value == "MILESTONE" || value == "COSTTYPE" ? value : "COSTCODE";
    }

    public static SqlParameter DateParameter(string name, DateTime value)
    {
        return new SqlParameter(name, SqlDbType.Date) { Value = value };
    }
}
