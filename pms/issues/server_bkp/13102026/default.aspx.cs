using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.UI;
using CommonClass;

public partial class pms_issues_default : Page
{
    public int pageRegId;
    public int pageEmployeeId;
    public int isadmin;

    protected void Page_Load(object sender, EventArgs e)
    {
        pageRegId = GetCookieInt("cadmin", "id");
        pageEmployeeId = GetCookieInt("user", "user_id");
        isadmin = IsAdmin(pageRegId, pageEmployeeId) ? 1 : 0;
    }

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

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static IssueGridResponse GetIssuesGrid(IssueGridRequest request)
    {
        IssueGridResponse response = new IssueGridResponse();
        try
        {
            int regId = GetCookieInt("cadmin", "id");
            int employeeId = GetCookieInt("user", "user_id");
            bool admin = IsAdmin(regId, employeeId);
            if (regId <= 0) throw new HttpException(401, "Company login was not found.");
            if (employeeId <= 0) throw new HttpException(401, "Employee login was not found.");
            if (request == null) request = new IssueGridRequest();

            string type = NormalizeType(request.EntityType);
            string status = NormalizeStatus(request.IssueStatus);
            DateTime? fromDate = ParseDate(request.FromDate);
            DateTime? toDate = ParseDate(request.ToDate);
            int[] selectedEmployees = request.EmployeeIds == null
                ? new int[0]
                : request.EmployeeIds.Where(id => id > 0).Distinct().Take(100).ToArray();
            if (fromDate.HasValue != toDate.HasValue)
                throw new ArgumentException("Select both dates for the selected date range.");
            if (fromDate.HasValue && toDate.Value < fromDate.Value)
                throw new ArgumentException("From date must be on or before To date.");

            string reg = regId.ToString(CultureInfo.InvariantCulture);
            string employee = employeeId.ToString(CultureInfo.InvariantCulture);
            string sql = BuildIssueSql(reg, employee, admin, type, status, fromDate, toDate, selectedEmployees);
            DataSet ds = new DataAccess().GetDataSetInline(sql);
            DataTable table = ds != null && ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
            Dictionary<int, string> employeeNames = LoadEmployeeNames(regId, table);

            List<IssueGridRow> rows = table.Rows.Cast<DataRow>().Select(row => new IssueGridRow
            {
                Id = ReadInt(row, "Id"),
                HierarchySrNo = ReadString(row, "HierarchySrNo"),
                EntityType = NormalizeType(ReadString(row, "EntityType")),
                IssueName = ReadString(row, "IssueName"),
                Description = ReadString(row, "Description"),
                OwnerName = MapPeople(ReadString(row, "OwnerId"), employeeNames),
                TeamNames = MapPeople(ReadString(row, "IssueTeam"), employeeNames),
                StatusName = ReadString(row, "StatusName"),
                IsComplete = ReadInt(row, "IsComplete"),
                PriorityName = ReadString(row, "PriorityName"),
                AssigneeName = MapPeople(ReadString(row, "AssignToId"), employeeNames),
                CreateDate = ReadDate(row, "CreateDate"),
                TargetDate = ReadDate(row, "TargetDate"),
                IssueType = ReadString(row, "IssueType"),
                ProjectName = ReadString(row, "ProjectName"),
                MilestoneName = ReadString(row, "MilestoneName")
            }).ToList();

            int pageSize = request.PageSize > 0 ? Math.Min(request.PageSize, 100) : 25;
            int pageNumber = request.PageNumber > 0 ? request.PageNumber : 1;
            int totalRows = rows.Count;
            int totalPages = Math.Max(1, (int)Math.Ceiling(totalRows / (double)pageSize));
            if (pageNumber > totalPages) pageNumber = totalPages;

            response.Rows = rows.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            response.Success = true;
            response.IsAdmin = admin ? 1 : 0;
            response.TotalRows = totalRows;
            response.TotalPages = totalPages;
            response.PageNumber = pageNumber;
            return response;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
            return response;
        }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static IssueEmployeeResponse GetIssueEmployees()
    {
        IssueEmployeeResponse response = new IssueEmployeeResponse();
        try
        {
            int regId = GetCookieInt("cadmin", "id");
            int employeeId = GetCookieInt("user", "user_id");
            if (regId <= 0 || employeeId <= 0) throw new HttpException(401, "Login information was not found.");
            int isAdmin = IsAdmin(regId, employeeId) ? 1 : 0;
            SqlParameter[] prm = new SqlParameter[3];
            prm[0] = new SqlParameter("@regid", regId);
            prm[1] = new SqlParameter("@isadmin", isAdmin);
            prm[2] = new SqlParameter("@uid", employeeId.ToString(CultureInfo.InvariantCulture));
            DataSet ds = new DataAccess().GetDataSet("bindemployee_forTransfer", prm);
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    int id = ReadFirstInt(row, "id", "uid", "employeeid", "EmployeeId", "value");
                    if (id <= 0) continue;
                    string name = ReadFirstString(row, "DisplayName", "name", "EmployeeName", "employeename", "empname", "text");
                    response.Employees.Add(new IssueEmployeeOption { Id = id, Name = String.IsNullOrWhiteSpace(name) ? "Employee " + id : name });
                }
            }
            response.Employees = response.Employees.GroupBy(item => item.Id).Select(group => group.First())
                .OrderBy(item => item.Name, StringComparer.OrdinalIgnoreCase).ToList();
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }
        return response;
    }

    private static string BuildIssueSql(string reg, string employee, bool admin, string type,
        string status, DateTime? fromDate, DateTime? toDate, int[] selectedEmployees)
    {
        string sql =
            (admin ? String.Empty : BuildEmployeeHierarchyCte(reg, employee)) +
            "SELECT i.id AS Id, ISNULL(i.Issues,'') AS IssueName, ISNULL(i.Description,'') AS Description, " +
            "ISNULL(CONVERT(nvarchar(50),i.Owner),'') AS OwnerId, ISNULL(CONVERT(nvarchar(2000),i.team),'') AS IssueTeam, " +
            "ISNULL(CONVERT(nvarchar(50),i.AssignTo),'') AS AssignToId, ISNULL(ps.ProjectStatus,'Unknown') AS StatusName, " +
            "ISNULL(ps.iscomplete,0) AS IsComplete, ISNULL(pr.name,'') AS PriorityName, " +
            "ISNULL(CONVERT(nvarchar(100),i.IssueType),'') AS IssueType, " +
            "ISNULL(CONVERT(nvarchar(30),i.Issueon),'') AS EntityType, i.cdate AS CreateDate, i.EndDate AS TargetDate, " +
            "ISNULL(p.ProjectName,'') AS ProjectName, ISNULL(m.milestone,'') AS MilestoneName, " +
            "ISNULL(CONVERT(varchar(20),m.orderno),'') + " +
            "CASE WHEN t.orderno IS NULL THEN '' ELSE '.' + CONVERT(varchar(20),t.orderno) END + " +
            "CASE WHEN s.orderno IS NULL THEN '' ELSE '.' + CONVERT(varchar(20),s.orderno) END AS HierarchySrNo " +
            "FROM dbo.Pms_tbl_Addissues i WITH (NOLOCK) " +
            "LEFT JOIN dbo.Pms_tbl_Projectstatus ps WITH (NOLOCK) ON ps.id=i.IssueStatus AND ps.regid=i.regid AND ps.status=1 AND ps.archive=0 " +
            "LEFT JOIN dbo.Pms_tbl_Priority pr WITH (NOLOCK) ON pr.id=i.Priority AND pr.regid=i.regid AND pr.status=1 AND pr.archive=0 " +
            "LEFT JOIN dbo.Pms_tbl_AddProject p WITH (NOLOCK) ON p.id=i.ProjectId AND p.regid=i.regid AND p.status=1 AND p.archive=0 " +
            "LEFT JOIN dbo.Pms_tbl_AddMilestone m WITH (NOLOCK) ON m.id=i.Milestoneid AND m.regid=i.regid AND m.status=1 AND m.archive=0 " +
            "LEFT JOIN dbo.Pms_tbl_AddTask t WITH (NOLOCK) ON t.id=COALESCE(NULLIF(i.taskid,0),NULLIF(i.tasklistid,0)) AND t.regid=i.regid AND t.status=1 AND t.archive=0 " +
            "LEFT JOIN dbo.Pms_tbl_AddSubTask s WITH (NOLOCK) ON s.id=i.subtaskid AND s.regid=i.regid AND s.status=1 AND s.archive=0 " +
            "WHERE i.regid=" + reg + " AND i.status=1 AND i.archive=0 ";

        if (type != "ALL") sql += "AND UPPER(LTRIM(RTRIM(CONVERT(nvarchar(30),i.Issueon))))='" + type + "' ";
        if (status == "PENDING") sql += "AND ISNULL(ps.iscomplete,0)=0 ";
        else if (status == "COMPLETE") sql += "AND ISNULL(ps.iscomplete,0)=1 ";
        if (fromDate.HasValue && toDate.HasValue)
        {
            string from = fromDate.Value.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
            string to = toDate.Value.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
            sql += "AND ISNULL(i.cdate, i.StartDate) >= CONVERT(date,'" + from + "',112) " +
                   "AND ISNULL(i.cdate, i.StartDate) < DATEADD(day,1,CONVERT(date,'" + to + "',112)) ";
        }

        if (!admin)
        {
            sql += "AND " + EmployeeHierarchyMatchClause() + " ";
        }
        if (selectedEmployees != null && selectedEmployees.Length > 0)
        {
            string selectedMatch = String.Join(" OR ", selectedEmployees
                .Select(id => EmployeeMatchClause(id.ToString(CultureInfo.InvariantCulture))).ToArray());
            sql += "AND (" + selectedMatch + ") ";
        }

        return sql + "ORDER BY i.cdate DESC, i.id DESC" + (admin ? String.Empty : " OPTION (MAXRECURSION 8)");
    }

    private static string BuildEmployeeHierarchyCte(string reg, string employee)
    {
        return "WITH EmployeeHierarchy AS (" +
               "SELECT root.id, 0 AS Depth FROM dbo.tbl_employee root WITH (NOLOCK) " +
               "WHERE root.regid=" + reg + " AND root.id=" + employee + " AND root.status=1 AND root.archive=0 " +
               "UNION ALL " +
               "SELECT child.id, parent.Depth+1 FROM dbo.tbl_employee child WITH (NOLOCK) " +
               "INNER JOIN EmployeeHierarchy parent ON child.councelor_id=parent.id " +
               "WHERE child.regid=" + reg + " AND child.status=1 AND child.archive=0 AND parent.Depth<7) ";
    }

    private static string EmployeeHierarchyMatchClause()
    {
        string pattern = "(',' + REPLACE(REPLACE(REPLACE(ISNULL(CONVERT(nvarchar(2000),{0}),''),' ',''),';',','),'|',',') + ',') " +
                         "LIKE '%,' + CONVERT(nvarchar(20),eh.id) + ',%'";
        return "(TRY_CONVERT(int,i.Owner) IN (SELECT id FROM EmployeeHierarchy) " +
               "OR EXISTS (SELECT 1 FROM EmployeeHierarchy eh WHERE " + String.Format(CultureInfo.InvariantCulture, pattern, "i.Owner") + ") " +
               "OR TRY_CONVERT(int,i.AssignTo) IN (SELECT id FROM EmployeeHierarchy) " +
               "OR EXISTS (SELECT 1 FROM EmployeeHierarchy eh WHERE " + String.Format(CultureInfo.InvariantCulture, pattern, "i.AssignTo") + ") " +
               "OR EXISTS (SELECT 1 FROM EmployeeHierarchy eh WHERE " + String.Format(CultureInfo.InvariantCulture, pattern, "i.team") + ") " +
               "OR EXISTS (SELECT 1 FROM EmployeeHierarchy eh WHERE " + String.Format(CultureInfo.InvariantCulture, pattern, "p.team") + ") " +
               "OR EXISTS (SELECT 1 FROM EmployeeHierarchy eh WHERE " + String.Format(CultureInfo.InvariantCulture, pattern, "m.team") + ") " +
               "OR EXISTS (SELECT 1 FROM EmployeeHierarchy eh WHERE " + String.Format(CultureInfo.InvariantCulture, pattern, "t.team") + ") " +
               "OR EXISTS (SELECT 1 FROM EmployeeHierarchy eh WHERE " + String.Format(CultureInfo.InvariantCulture, pattern, "s.team") + "))";
    }

    private static string EmployeeMatchClause(string employee)
    {
        string pattern = "(',' + REPLACE(REPLACE(REPLACE(ISNULL(CONVERT(nvarchar(2000),{0}),''),' ',''),';',','),'|',',') + ',') LIKE '%," + employee + ",%'";
        return "(TRY_CONVERT(int,i.Owner)=" + employee +
               " OR " + String.Format(CultureInfo.InvariantCulture, pattern, "i.Owner") +
               " OR TRY_CONVERT(int,i.AssignTo)=" + employee +
               " OR " + String.Format(CultureInfo.InvariantCulture, pattern, "i.AssignTo") +
               " OR " + String.Format(CultureInfo.InvariantCulture, pattern, "i.team") +
               " OR " + String.Format(CultureInfo.InvariantCulture, pattern, "p.team") +
               " OR " + String.Format(CultureInfo.InvariantCulture, pattern, "m.team") +
               " OR " + String.Format(CultureInfo.InvariantCulture, pattern, "t.team") +
               " OR " + String.Format(CultureInfo.InvariantCulture, pattern, "s.team") + ")";
    }

    private static Dictionary<int, string> LoadEmployeeNames(int regId, DataTable issues)
    {
        var ids = new HashSet<int>();
        foreach (DataRow row in issues.Rows)
        {
            AddIds(ids, ReadString(row, "OwnerId"));
            AddIds(ids, ReadString(row, "AssignToId"));
            AddIds(ids, ReadString(row, "IssueTeam"));
        }
        var names = new Dictionary<int, string>();
        if (ids.Count == 0) return names;
        string list = String.Join(",", ids.Select(id => id.ToString(CultureInfo.InvariantCulture)).ToArray());
        string sql = "SELECT id, DisplayName FROM dbo.tbl_employee WITH (NOLOCK) WHERE regid=" +
                     regId.ToString(CultureInfo.InvariantCulture) + " AND status=1 AND archive=0 AND id IN (" + list + ")";
        try
        {
            DataSet ds = new DataAccess().GetDataSetInline(sql);
            if (ds == null || ds.Tables.Count == 0) return names;
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int id = ReadInt(row, "id");
                if (id > 0) names[id] = ReadString(row, "DisplayName");
            }
        }
        catch { }
        return names;
    }

    private static string MapPeople(string value, Dictionary<int, string> names)
    {
        var result = new List<string>();
        foreach (string part in SplitIds(value))
        {
            int id;
            if (!Int32.TryParse(part, NumberStyles.Integer, CultureInfo.InvariantCulture, out id) || id <= 0) continue;
            string name;
            if (names.TryGetValue(id, out name) && !String.IsNullOrWhiteSpace(name)) result.Add(name);
        }
        return String.Join(", ", result.Distinct(StringComparer.OrdinalIgnoreCase).ToArray());
    }

    private static void AddIds(HashSet<int> target, string value)
    {
        foreach (string part in SplitIds(value))
        {
            int id;
            if (Int32.TryParse(part, NumberStyles.Integer, CultureInfo.InvariantCulture, out id) && id > 0) target.Add(id);
        }
    }

    private static IEnumerable<string> SplitIds(string value)
    {
        return (value ?? String.Empty).Split(new[] { ',', ';', '|', ' ' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(part => part.Trim());
    }

    private static DateTime? ParseDate(string value)
    {
        if (String.IsNullOrWhiteSpace(value)) return null;
        DateTime date;
        if (DateTime.TryParseExact(value.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date)) return date.Date;
        return null;
    }

    private static string NormalizeType(string type)
    {
        string value = (type ?? String.Empty).Trim().ToUpperInvariant().Replace(" ", String.Empty);
        if (value == "PROJECT" || value == "MILESTONE" || value == "TASK" || value == "SUBTASK") return value;
        return "ALL";
    }

    private static string NormalizeStatus(string status)
    {
        string value = (status ?? String.Empty).Trim().ToUpperInvariant();
        return value == "PENDING" || value == "COMPLETE" ? value : "ALL";
    }

    private static int ReadInt(DataRow row, string column)
    {
        if (!row.Table.Columns.Contains(column) || row[column] == DBNull.Value) return 0;
        int value;
        return Int32.TryParse(Convert.ToString(row[column], CultureInfo.InvariantCulture), out value) ? value : 0;
    }

    private static int ReadFirstInt(DataRow row, params string[] columns)
    {
        foreach (string column in columns)
        {
            if (!row.Table.Columns.Contains(column) || row[column] == DBNull.Value) continue;
            int value;
            if (Int32.TryParse(Convert.ToString(row[column], CultureInfo.InvariantCulture), out value)) return value;
        }
        return 0;
    }

    private static string ReadFirstString(DataRow row, params string[] columns)
    {
        foreach (string column in columns)
        {
            if (row.Table.Columns.Contains(column) && row[column] != DBNull.Value)
            {
                string value = Convert.ToString(row[column], CultureInfo.InvariantCulture);
                if (!String.IsNullOrWhiteSpace(value)) return value.Trim();
            }
        }
        return String.Empty;
    }

    private static string ReadString(DataRow row, string column)
    {
        if (!row.Table.Columns.Contains(column) || row[column] == DBNull.Value) return String.Empty;
        return Convert.ToString(row[column], CultureInfo.InvariantCulture) ?? String.Empty;
    }

    private static string ReadDate(DataRow row, string column)
    {
        if (!row.Table.Columns.Contains(column) || row[column] == DBNull.Value) return String.Empty;
        DateTime value;
        if (DateTime.TryParse(Convert.ToString(row[column], CultureInfo.InvariantCulture), CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out value))
            return value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        return String.Empty;
    }

    public class IssueGridRequest
    {
        public string EntityType { get; set; }
        public string IssueStatus { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public List<int> EmployeeIds { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class IssueEmployeeResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<IssueEmployeeOption> Employees { get; set; }
        public IssueEmployeeResponse() { Success = false; Message = String.Empty; Employees = new List<IssueEmployeeOption>(); }
    }

    public class IssueEmployeeOption
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class IssueGridResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int IsAdmin { get; set; }
        public int TotalRows { get; set; }
        public int TotalPages { get; set; }
        public int PageNumber { get; set; }
        public List<IssueGridRow> Rows { get; set; }
        public IssueGridResponse() { Rows = new List<IssueGridRow>(); Message = String.Empty; }
    }

    public class IssueGridRow
    {
        public int Id { get; set; }
        public string HierarchySrNo { get; set; }
        public string EntityType { get; set; }
        public string IssueName { get; set; }
        public string Description { get; set; }
        public string OwnerName { get; set; }
        public string TeamNames { get; set; }
        public string StatusName { get; set; }
        public int IsComplete { get; set; }
        public string PriorityName { get; set; }
        public string AssigneeName { get; set; }
        public string CreateDate { get; set; }
        public string TargetDate { get; set; }
        public string IssueType { get; set; }
        public string ProjectName { get; set; }
        public string MilestoneName { get; set; }
    }
}
