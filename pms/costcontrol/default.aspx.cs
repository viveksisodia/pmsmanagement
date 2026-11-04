using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using CommonClass;

/// <summary>
/// PMS Cost Control (Phase 1): cost codes, versioned project budgets,
/// cost entries and budget-vs-actual.
/// All data access goes through the parameterised usp_pms_cc_* procedures
/// (see pms/costcontrol/sql/costcontrol_phase1.sql). Existing PMS tables are
/// only read, never written.
/// Access: company administrators only (same admin rule as the Issues module).
/// </summary>
public partial class pms_costcontrol_default : Page
{
    public int pageRegId;
    public int pageEmployeeId;
    public int isadmin;

    protected void Page_Load(object sender, EventArgs e)
    {
        pageRegId = GetCookieInt("cadmin", "id");
        pageEmployeeId = GetCookieInt("user", "user_id");
        isadmin = IsAdmin(pageRegId, pageEmployeeId) ? 1 : 0;
        if (isadmin != 1) Response.Redirect("/pms/accessdenied.aspx");
    }

    #region Login and access helpers

    private static int GetCookieInt(string cookieName, string key)
    {
        HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
        int value;
        return cookie != null && Int32.TryParse(cookie[key], out value) ? value : 0;
    }

    private static bool IsAdmin(int regId, int employeeId)
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

    private sealed class LoginContext { public int RegId; public int EmployeeId; }

    private static LoginContext RequireAdminLogin()
    {
        int regId = GetCookieInt("cadmin", "id");
        int employeeId = GetCookieInt("user", "user_id");
        if (regId <= 0 || employeeId <= 0) throw new HttpException(401, "Login information was not found.");
        if (!IsAdmin(regId, employeeId)) throw new HttpException(403, "Only administrators can use Cost Control.");
        return new LoginContext { RegId = regId, EmployeeId = employeeId };
    }

    #endregion

    #region Data helpers

    private static DataSet RunProcedure(string procedureName, params SqlParameter[] parameters)
    {
        foreach (SqlParameter parameter in parameters)
            if (parameter.Value == null) parameter.Value = DBNull.Value;
        return new DataAccess().GetDataSet(procedureName, parameters);
    }

    private static object DbValue(int? value) { return value.HasValue && value.Value > 0 ? (object)value.Value : DBNull.Value; }
    private static object DbValue(decimal? value) { return value.HasValue ? (object)value.Value : DBNull.Value; }
    private static object DbText(string value) { return String.IsNullOrWhiteSpace(value) ? (object)DBNull.Value : value.Trim(); }

    private static List<Dictionary<string, object>> ToRows(DataSet ds, int tableIndex)
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

    /// <summary>Reads the Result / Message / id row returned by every save procedure.</summary>
    private static object SaveResult(DataSet ds)
    {
        if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            return new { Success = false, Message = "No response from the database." };
        DataRow row = ds.Tables[0].Rows[0];
        bool success = Convert.ToInt32(row["Result"], CultureInfo.InvariantCulture) == 1;
        int id = ds.Tables[0].Columns.Contains("id") && row["id"] != DBNull.Value ? Convert.ToInt32(row["id"], CultureInfo.InvariantCulture) : 0;
        return new { Success = success, Message = Convert.ToString(row["Message"], CultureInfo.InvariantCulture), Id = id };
    }

    private static object Failure(Exception ex) { return new { Success = false, Message = ex.Message }; }

    private static DateTime ParseBusinessDate(string value)
    {
        DateTime date;
        if (!DateTime.TryParseExact(value ?? "", "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            throw new ArgumentException("Enter the cost date as a valid date.");
        return date.Date;
    }

    #endregion

    #region Lookups

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetLookups()
    {
        try
        {
            LoginContext login = RequireAdminLogin();
            DataSet projects = RunProcedure("usp_pms_cc_Project_List", new SqlParameter("@regid", login.RegId));
            DataSet costCodes = RunProcedure("usp_pms_cc_CostCode_List", new SqlParameter("@regid", login.RegId));
            DataSet units = RunProcedure("usp_pms_cc_Unit_List", new SqlParameter("@regid", login.RegId));
            return new { Success = true, Projects = ToRows(projects, 0), CostCodes = ToRows(costCodes, 0), Units = ToRows(units, 0) };
        }
        catch (Exception ex) { return Failure(ex); }
    }

    /// <summary>Adds a unit to the existing unit master through the existing ERP procedure.</summary>
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object AddUnit(string unitName)
    {
        try
        {
            LoginContext login = RequireAdminLogin();
            return SaveResult(RunProcedure("usp_pms_cc_Unit_Add",
                new SqlParameter("@regid", login.RegId), new SqlParameter("@UnitName", unitName ?? "")));
        }
        catch (Exception ex) { return Failure(ex); }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetProjectStructure(int projectId)
    {
        try
        {
            LoginContext login = RequireAdminLogin();
            DataSet ds = RunProcedure("usp_pms_cc_ProjectStructure",
                new SqlParameter("@regid", login.RegId), new SqlParameter("@ProjectId", projectId));
            return new { Success = true, Milestones = ToRows(ds, 0), Tasks = ToRows(ds, 1) };
        }
        catch (Exception ex) { return Failure(ex); }
    }

    #endregion

    #region Cost codes

    public class CostCodeRequest
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string CostType { get; set; }
        public string Description { get; set; }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object SaveCostCode(CostCodeRequest request)
    {
        try
        {
            LoginContext login = RequireAdminLogin();
            if (request == null) throw new ArgumentException("Nothing to save.");
            return SaveResult(RunProcedure("usp_pms_cc_CostCode_Save",
                new SqlParameter("@regid", login.RegId),
                new SqlParameter("@id", request.Id),
                new SqlParameter("@Code", request.Code ?? ""),
                new SqlParameter("@Name", request.Name ?? ""),
                new SqlParameter("@CostType", request.CostType ?? ""),
                new SqlParameter("@Description", DbText(request.Description)),
                new SqlParameter("@uid", login.EmployeeId)));
        }
        catch (Exception ex) { return Failure(ex); }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object DeleteCostCode(int costCodeId)
    {
        try
        {
            LoginContext login = RequireAdminLogin();
            return SaveResult(RunProcedure("usp_pms_cc_CostCode_Delete",
                new SqlParameter("@regid", login.RegId), new SqlParameter("@id", costCodeId), new SqlParameter("@uid", login.EmployeeId)));
        }
        catch (Exception ex) { return Failure(ex); }
    }

    #endregion

    #region Budget

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetBudget(int projectId, int versionId)
    {
        try
        {
            LoginContext login = RequireAdminLogin();
            DataSet ds = RunProcedure("usp_pms_cc_Budget_Get",
                new SqlParameter("@regid", login.RegId),
                new SqlParameter("@ProjectId", projectId),
                new SqlParameter("@VersionId", DbValue((int?)versionId)));
            return new { Success = true, Versions = ToRows(ds, 0), Lines = ToRows(ds, 1) };
        }
        catch (Exception ex) { return Failure(ex); }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object CreateBudgetVersion(int projectId, string reason)
    {
        try
        {
            LoginContext login = RequireAdminLogin();
            return SaveResult(RunProcedure("usp_pms_cc_BudgetVersion_Create",
                new SqlParameter("@regid", login.RegId), new SqlParameter("@ProjectId", projectId),
                new SqlParameter("@Reason", DbText(reason)), new SqlParameter("@uid", login.EmployeeId)));
        }
        catch (Exception ex) { return Failure(ex); }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object LockBudgetVersion(int versionId)
    {
        try
        {
            LoginContext login = RequireAdminLogin();
            return SaveResult(RunProcedure("usp_pms_cc_BudgetVersion_Lock",
                new SqlParameter("@regid", login.RegId), new SqlParameter("@VersionId", versionId), new SqlParameter("@uid", login.EmployeeId)));
        }
        catch (Exception ex) { return Failure(ex); }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object DiscardDraftVersion(int versionId)
    {
        try
        {
            LoginContext login = RequireAdminLogin();
            return SaveResult(RunProcedure("usp_pms_cc_BudgetVersion_DeleteDraft",
                new SqlParameter("@regid", login.RegId), new SqlParameter("@VersionId", versionId), new SqlParameter("@uid", login.EmployeeId)));
        }
        catch (Exception ex) { return Failure(ex); }
    }

    public class BudgetLineRequest
    {
        public int Id { get; set; }
        public int VersionId { get; set; }
        public int? MilestoneId { get; set; }
        public int? TaskId { get; set; }
        public int CostCodeId { get; set; }
        public decimal? Quantity { get; set; }
        public int? UnitId { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Amount { get; set; }
        public string Remarks { get; set; }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object SaveBudgetLine(BudgetLineRequest request)
    {
        try
        {
            LoginContext login = RequireAdminLogin();
            if (request == null) throw new ArgumentException("Nothing to save.");
            return SaveResult(RunProcedure("usp_pms_cc_BudgetLine_Save",
                new SqlParameter("@regid", login.RegId),
                new SqlParameter("@id", request.Id),
                new SqlParameter("@VersionId", request.VersionId),
                new SqlParameter("@MilestoneId", DbValue(request.MilestoneId)),
                new SqlParameter("@TaskId", DbValue(request.TaskId)),
                new SqlParameter("@CostCodeId", request.CostCodeId),
                new SqlParameter("@Quantity", DbValue(request.Quantity)),
                new SqlParameter("@UnitId", DbValue(request.UnitId)),
                new SqlParameter("@Rate", DbValue(request.Rate)),
                new SqlParameter("@Amount", DbValue(request.Amount)),
                new SqlParameter("@Remarks", DbText(request.Remarks)),
                new SqlParameter("@uid", login.EmployeeId)));
        }
        catch (Exception ex) { return Failure(ex); }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object DeleteBudgetLine(int budgetLineId)
    {
        try
        {
            LoginContext login = RequireAdminLogin();
            return SaveResult(RunProcedure("usp_pms_cc_BudgetLine_Delete",
                new SqlParameter("@regid", login.RegId), new SqlParameter("@id", budgetLineId), new SqlParameter("@uid", login.EmployeeId)));
        }
        catch (Exception ex) { return Failure(ex); }
    }

    #endregion

    #region Cost entries (actuals)

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetCostEntries(int projectId)
    {
        try
        {
            LoginContext login = RequireAdminLogin();
            DataSet ds = RunProcedure("usp_pms_cc_CostEntry_List",
                new SqlParameter("@regid", login.RegId), new SqlParameter("@ProjectId", projectId));
            return new { Success = true, Entries = ToRows(ds, 0) };
        }
        catch (Exception ex) { return Failure(ex); }
    }

    public class CostEntryRequest
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int? MilestoneId { get; set; }
        public int? TaskId { get; set; }
        public int CostCodeId { get; set; }
        public string EntryDate { get; set; }   // yyyy-MM-dd, business date only
        public decimal? Amount { get; set; }
        public string ReferenceNo { get; set; }
        public string Remarks { get; set; }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object SaveCostEntry(CostEntryRequest request)
    {
        try
        {
            LoginContext login = RequireAdminLogin();
            if (request == null) throw new ArgumentException("Nothing to save.");
            DateTime entryDate = ParseBusinessDate(request.EntryDate);
            SqlParameter dateParameter = new SqlParameter("@EntryDate", SqlDbType.Date) { Value = entryDate };
            return SaveResult(RunProcedure("usp_pms_cc_CostEntry_Save",
                new SqlParameter("@regid", login.RegId),
                new SqlParameter("@id", request.Id),
                new SqlParameter("@ProjectId", request.ProjectId),
                new SqlParameter("@MilestoneId", DbValue(request.MilestoneId)),
                new SqlParameter("@TaskId", DbValue(request.TaskId)),
                new SqlParameter("@CostCodeId", request.CostCodeId),
                dateParameter,
                new SqlParameter("@Amount", DbValue(request.Amount)),
                new SqlParameter("@ReferenceNo", DbText(request.ReferenceNo)),
                new SqlParameter("@Remarks", DbText(request.Remarks)),
                new SqlParameter("@uid", login.EmployeeId)));
        }
        catch (Exception ex) { return Failure(ex); }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object DeleteCostEntry(int costEntryId)
    {
        try
        {
            LoginContext login = RequireAdminLogin();
            return SaveResult(RunProcedure("usp_pms_cc_CostEntry_Delete",
                new SqlParameter("@regid", login.RegId), new SqlParameter("@id", costEntryId), new SqlParameter("@uid", login.EmployeeId)));
        }
        catch (Exception ex) { return Failure(ex); }
    }

    #endregion

    #region Budget vs actual

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetBudgetVsActual(int projectId, string groupBy)
    {
        try
        {
            LoginContext login = RequireAdminLogin();
            string grouping = String.Equals(groupBy, "MILESTONE", StringComparison.OrdinalIgnoreCase) ? "MILESTONE" : "COSTCODE";
            DataSet ds = RunProcedure("usp_pms_cc_BudgetVsActual",
                new SqlParameter("@regid", login.RegId), new SqlParameter("@ProjectId", projectId), new SqlParameter("@GroupBy", grouping));
            List<Dictionary<string, object>> header = ToRows(ds, 1);
            return new { Success = true, Rows = ToRows(ds, 0), Header = header.FirstOrDefault() };
        }
        catch (Exception ex) { return Failure(ex); }
    }

    #endregion
}
