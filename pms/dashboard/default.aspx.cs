using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.UI;
using CommonClass;

public partial class pms_manager_dashboard : Page
{
    DataSet ds = new DataSet();
    DataAccess da = new DataAccess();
    public CommonFunction cmf = new CommonFunction();
    protected void Page_Load(object sender, EventArgs e) {
        if (Request.Cookies["user"] == null || Request.Cookies["cadmin"] == null)
        {
            Response.Redirect("/login.aspx"); // Redirect immediately if cookies are missing
        }
        string uid = Request.Cookies["user"]["user_id"].ToString();
        if (Request.QueryString["empid"] != null)
        {
            uid = cmf.Decrypt(Request.QueryString["empid"]).ToString();
        }
        int chkisadmin = checkadmin(uid);
        int issingleuser = checksingleuser(uid);
        if (issingleuser == 1 && chkisadmin == 0)
        {
            string strabsoluteurl = Request.Url.ToString().Replace("/pms/dashboard/default.aspx", "/pms/userdashboard/default.aspx");
            Response.Redirect(strabsoluteurl);
        }
    }
    protected int checkadmin(string struserid = "0")
    {
        int retval = 0;
        string strsql = "select Manage_Profile.* from Manage_Profile inner join tbl_employee on Manage_Profile.id=cast(tbl_employee.roleid as int) where tbl_employee.id='" + struserid + "'  and Manage_Profile.status=1 and Manage_Profile.archive=0";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["id"].ToString() == "1")
            {
                retval = 1;
            }
        }
        return retval;
    }
    protected int checksingleuser(string struserid = "0")
    {
        int retval = 0;
        try
        {
            SqlParameter[] prmemp = new SqlParameter[2];
            prmemp[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"].ToString());
            prmemp[1] = new SqlParameter("@uid", struserid);
            DataSet ds = da.GetDataSet("pms_singleuser_sp", prmemp);
            if (ds.Tables[0].Rows.Count == 0)
            {

                retval = 1;

            }
        }
        catch (Exception ex)
        {
            retval = 0;
            SaveLogText("SingleExe:" + ex.Message);
        }
        return retval;
    }
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
    public static object GetProjects(int employeeId, int projectId, string startDate, string endDate)
    {
        HttpCookie tenant = HttpContext.Current.Request.Cookies["cadmin"];
        HttpCookie user = HttpContext.Current.Request.Cookies["user"];
        int regId, uid;
        if (tenant == null || user == null || !Int32.TryParse(tenant["id"], out regId) ||
            !Int32.TryParse(user["user_id"], out uid) || regId <= 0 || uid <= 0)
            throw new HttpException(401, "Login information is missing or invalid.");
        if (employeeId < 0 || projectId < 0) throw new ArgumentException("Invalid filter.");
        // Resolve self and active reporting employees within this tenant.
        int admin = 0; // Procedure determines the current role from the database.
        DataSet employees = new DataAccess().GetDataSet("pms_ManagerDashboardAllowedEmployees", new SqlParameter[] {
            new SqlParameter("@regid", regId), new SqlParameter("@isadmin", admin),
            new SqlParameter("@uid", SqlDbType.Int) { Value = uid }
        });
        if (employees == null || employees.Tables.Count < 2 || employees.Tables[1].Rows.Count != 1)
            throw new InvalidOperationException("Install the optimized employee procedure first.");
        admin = Convert.ToInt32(employees.Tables[1].Rows[0]["IsAdmin"], CultureInfo.InvariantCulture);

        var allowed = new HashSet<int>();
        if (employees != null && employees.Tables.Count > 0)
            foreach (DataRow row in employees.Tables[0].Rows) {
                int id; if (Int32.TryParse(Convert.ToString(row["id"]), out id) && id > 0) allowed.Add(id);
            }
        var ids = new List<string>();
        foreach (int id in allowed) ids.Add(id.ToString(CultureInfo.InvariantCulture));
        DateTime? from = ParseFilterDate(startDate), to = ParseFilterDate(endDate);
        if (from.HasValue && to.HasValue && from > to) throw new ArgumentException("Invalid date range.");
        SaveLogText(
      "pms_ManagerDashboardProjects" +
      " | uid=" + uid +
      " | regId=" + regId +
      " | isadmin=" + admin +
      " | AllowedEmployeeIds=" + String.Join(",", ids.ToArray()) +
      " | employeeId=" + employeeId +
      " | projectId=" + projectId +
      " | startDate=" + (from.HasValue
          ? from.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
          : "NULL") +
      " | endDate=" + (to.HasValue
          ? to.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
          : "NULL")
  );

        DataSet ds = new DataAccess().GetDataSet("pms_ManagerDashboardProjects", new SqlParameter[] {
            new SqlParameter("@regid", regId), new SqlParameter("@isadmin", admin),
            new SqlParameter("@AllowedEmployeeIds", SqlDbType.NVarChar, -1) { Value = String.Join(",", ids.ToArray()) },
            new SqlParameter("@employeeId", employeeId), new SqlParameter("@projectId", projectId),
            new SqlParameter("@startDate", SqlDbType.Date) { Value = from.HasValue ? (object)from.Value : DBNull.Value },
            new SqlParameter("@endDate", SqlDbType.Date) { Value = to.HasValue ? (object)to.Value : DBNull.Value }
        });
        
        if (ds == null || ds.Tables.Count < 8) throw new InvalidOperationException("Install the supplied dashboard procedure first.");
        return new { projects = Rows(ds.Tables[0]), masters = Rows(ds.Tables[1]), milestones = Rows(ds.Tables[2]), projectOptions = Rows(ds.Tables[3]), itemGroups = Rows(ds.Tables[4]), taskProjects = Rows(ds.Tables[5]), issueProjects = Rows(ds.Tables[6]), tags = Rows(ds.Tables[7]) };
    }
    public static string SaveLogText(string Message)
    {
        string retval = "";
        CommonFunction cmf1 = new CommonFunction();
        DataAccess da1 = new DataAccess();
        DataSet dschk = new DataSet();

        SqlParameter[] prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@message", Message);
        prm[1] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[1].Direction = ParameterDirection.ReturnValue;
        retval = Convert.ToString(da1.executeDMLQuery("crm_entryLogTable", prm));
        return retval;
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
