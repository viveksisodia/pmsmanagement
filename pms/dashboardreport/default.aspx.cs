using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.UI;
using CommonClass;

public partial class pms_manager_dashboardreport : Page
{
    protected void Page_Load(object sender, EventArgs e) { }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetEmployees()
    {
        HttpCookie tenant = HttpContext.Current.Request.Cookies["cadmin"];
        HttpCookie user = HttpContext.Current.Request.Cookies["user"];
        int regId, employeeId;
        if (tenant == null || user == null || !Int32.TryParse(tenant["id"], out regId) ||
            !Int32.TryParse(user["user_id"], out employeeId) || regId <= 0 || employeeId <= 0)
            throw new HttpException(401, "Login information is missing or invalid.");
        int isAdmin = 0; // Procedure determines the current role from the database.
        SqlParameter[] prm = {
            new SqlParameter("@regid", regId),
            new SqlParameter("@isadmin", isAdmin),
            new SqlParameter("@uid", SqlDbType.Int) { Value = employeeId }
        };
        DataSet ds = new DataAccess().GetDataSet("pms_ManagerDashboardAllowedEmployees", prm);
        var result = new List<object>();
        var seen = new HashSet<int>();
        if (ds != null && ds.Tables.Count > 0) {
            DataTable table = ds.Tables[0];
            string idColumn = "id";
            string nameColumn = "DisplayName";
            if (table.Rows.Count > 0 && (!table.Columns.Contains(idColumn) || !table.Columns.Contains(nameColumn)))
                throw new InvalidOperationException("Employee procedure must return employee ID and name columns.");
            foreach (DataRow row in table.Rows) {
                int id;
                if (!Int32.TryParse(Convert.ToString(row[idColumn]), out id) || id <= 0 || !seen.Add(id)) continue;
                result.Add(new { id = id, name = Convert.ToString(row[nameColumn]) });
            }
        }
        return result;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetReport(int employeeId, int projectId, string startDate, string endDate,
        string reportType,string groupBy,int statusId,int priorityId,int situationId,int tagId,string issueState,
        string action,string groupKey,string search,int page,int pageSize)
    {
        HttpCookie tenant = HttpContext.Current.Request.Cookies["cadmin"];
        HttpCookie user = HttpContext.Current.Request.Cookies["user"];
        int regId, uid;
        if (tenant == null || user == null || !Int32.TryParse(tenant["id"], out regId) ||
            !Int32.TryParse(user["user_id"], out uid) || regId <= 0 || uid <= 0)
            throw new HttpException(401, "Login information is missing or invalid.");
        if (employeeId < 0 || projectId < 0) throw new ArgumentException("Invalid filter.");
        // Resolve self and active reporting employees within this tenant.
        var timer = Stopwatch.StartNew();
        long elapsedStart = 0;
        int admin = 0; // Procedure determines the current role from the database.
        DataSet employees = new DataAccess().GetDataSet("pms_ManagerDashboardAllowedEmployees", new SqlParameter[] {
            new SqlParameter("@regid", regId), new SqlParameter("@isadmin", admin),
            new SqlParameter("@uid", SqlDbType.Int) { Value = uid }
        });
        if (employees == null || employees.Tables.Count < 2 || employees.Tables[1].Rows.Count != 1)
            throw new InvalidOperationException("Install the optimized employee procedure first.");
        admin = Convert.ToInt32(employees.Tables[1].Rows[0]["IsAdmin"], CultureInfo.InvariantCulture);
        long employeeMs = timer.ElapsedMilliseconds;
        var allowed = new HashSet<int>();
        if (employees != null && employees.Tables.Count > 0)
            foreach (DataRow row in employees.Tables[0].Rows) {
                int id; if (Int32.TryParse(Convert.ToString(row["id"]), out id) && id > 0) allowed.Add(id);
            }
        var ids = new List<string>();
        foreach (int id in allowed) ids.Add(id.ToString(CultureInfo.InvariantCulture));
        DateTime? from = ParseFilterDate(startDate), to = ParseFilterDate(endDate);
        if (from.HasValue && to.HasValue && from > to) throw new ArgumentException("Invalid date range.");
        elapsedStart = timer.ElapsedMilliseconds;
        DataSet ds = new DataAccess().GetDataSet("pms_ManagerDashboardReport", new SqlParameter[] {
            new SqlParameter("@reportType", reportType ?? "projects"),new SqlParameter("@groupBy", groupBy ?? "Status"),
            new SqlParameter("@statusId",statusId),new SqlParameter("@priorityId",priorityId),new SqlParameter("@situationId",situationId),new SqlParameter("@tagId",tagId),
            new SqlParameter("@issueState",issueState ?? "All"),new SqlParameter("@action",action ?? "groups"),
            new SqlParameter("@groupKey",SqlDbType.NVarChar,100){Value=(object)groupKey ?? DBNull.Value},
            new SqlParameter("@search",SqlDbType.NVarChar,200){Value=search ?? ""},new SqlParameter("@page",page),new SqlParameter("@pageSize",pageSize),
            new SqlParameter("@regid", regId), new SqlParameter("@isadmin", admin),
            new SqlParameter("@AllowedEmployeeIds", SqlDbType.NVarChar, -1) { Value = String.Join(",", ids.ToArray()) },
            new SqlParameter("@employeeId", employeeId), new SqlParameter("@projectId", projectId),
            new SqlParameter("@startDate", SqlDbType.Date) { Value = from.HasValue ? (object)from.Value : DBNull.Value },
            new SqlParameter("@endDate", SqlDbType.Date) { Value = to.HasValue ? (object)to.Value : DBNull.Value }
        });
        long sqlMs = timer.ElapsedMilliseconds - elapsedStart;
        elapsedStart = timer.ElapsedMilliseconds;
        if (ds == null || ds.Tables.Count < 2) throw new InvalidOperationException("Install the report procedure first.");
        bool grouping = action == "groups";
        var response = new { rows = Rows(ds.Tables[grouping ? 0 : 1]), total = Convert.ToInt32(ds.Tables[grouping ? 1 : 0].Rows[0]["Total"],CultureInfo.InvariantCulture) };
        long prepareMs = timer.ElapsedMilliseconds - elapsedStart;
        HttpContext.Current.Response.AppendHeader("Server-Timing",
            "employees;dur=" + employeeMs.ToString(CultureInfo.InvariantCulture) +
            ", dashboard_sql;dur=" + sqlMs.ToString(CultureInfo.InvariantCulture) +
            ", prepare;dur=" + prepareMs.ToString(CultureInfo.InvariantCulture) +
            ", handler;dur=" + timer.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture));
        return response;
    }
    [WebMethod]
    public static object GetOptions()
    {
        HttpCookie tenant=HttpContext.Current.Request.Cookies["cadmin"],user=HttpContext.Current.Request.Cookies["user"];
        int regId,uid;
        if(tenant==null || user==null || !Int32.TryParse(tenant["id"],out regId) || !Int32.TryParse(user["user_id"],out uid) || regId<=0 || uid<=0) throw new HttpException(401,"Login required.");
        var e=new DataAccess().GetDataSet("pms_ManagerDashboardAllowedEmployees",new SqlParameter[]{new SqlParameter("@regid",regId),new SqlParameter("@isadmin", SqlDbType.Int) { Value = 0 },new SqlParameter("@uid",uid)});
        if(e==null || e.Tables.Count<2) throw new InvalidOperationException("Install employee procedure first.");
        var ids=new List<string>(); foreach(DataRow row in e.Tables[0].Rows) ids.Add(Convert.ToInt32(row["id"]).ToString(CultureInfo.InvariantCulture));
        var d=new DataAccess().GetDataSet("pms_ManagerDashboardReportOptions",new SqlParameter[]{new SqlParameter("@regid",regId),new SqlParameter("@isadmin",Convert.ToInt32(e.Tables[1].Rows[0]["IsAdmin"])),new SqlParameter("@AllowedEmployeeIds",String.Join(",",ids.ToArray()))});
        if(d==null || d.Tables.Count<5) throw new InvalidOperationException("Install report options procedure first.");
        return new {employees=Rows(e.Tables[0]),projects=Rows(d.Tables[0]),statuses=Rows(d.Tables[1]),priorities=Rows(d.Tables[2]),situations=Rows(d.Tables[3]),tags=Rows(d.Tables[4])};
    }
    private static DateTime? ParseFilterDate(string value)
    {
        if (String.IsNullOrWhiteSpace(value)) return null;
        DateTime date;
        if (!DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            throw new ArgumentException("Date must use yyyy-MM-dd.");
        return date.Date;
    }
    private static List<Dictionary<string, object>> Rows(DataTable table)
    {
        var rows = new List<Dictionary<string, object>>();
        foreach (DataRow row in table.Rows) {
            var item = new Dictionary<string, object>();
            foreach (DataColumn col in table.Columns)
                item[col.ColumnName] = row[col] == DBNull.Value ? null : row[col] is DateTime ? ((DateTime)row[col]).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : row[col];
            rows.Add(item);
        }
        return rows;
    }
}
