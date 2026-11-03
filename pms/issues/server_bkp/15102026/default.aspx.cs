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

    // Uses the module ID supplied in the existing permission example.
    private const int IssuePermissionModuleId = 57;
    //private const int IssuePermissionModuleId = 1060;
    public IssuePermissions pagePermissions = new IssuePermissions();
    public class IssuePermissions {
        public bool Full { get; set; } public bool Create { get; set; }
        public bool View { get; set; } public bool Edit { get; set; }
        public bool Delete { get; set; } public bool Assign { get; set; }
        public bool ChangeOwner { get; set; }
    }
    private static bool Flag(DataRow row,string name) {
        string value=ReadString(row,name); return value=="1" || value.Equals("True",StringComparison.OrdinalIgnoreCase);
    }
    private static IssuePermissions Permissions(int reg,int uid) {
        var result=new IssuePermissions();
        if(reg<=0 || uid<=0) return result;
        if(IsAdmin(reg,uid)) return new IssuePermissions {Full=true,View=true,Create=true,Edit=true,Delete=true,Assign=true,ChangeOwner=true};
        DataSet ds=new DataAccess().GetDataSetInline("SELECT ra.* FROM dbo.crm_tbl_roleaccessdetails ra INNER JOIN dbo.crm_tbl_roletaskmaster rm ON ra.moduleid=rm.id INNER JOIN dbo.tbl_employee e ON TRY_CONVERT(int,e.roleid)=ra.roleid INNER JOIN dbo.Manage_Profile mp ON mp.id=ra.roleid WHERE e.id="+uid+" AND e.regid="+reg+" AND e.status=1 AND e.archive=0 AND mp.status=1 AND mp.archive=0 AND ra.status=1 AND ra.regid="+reg+" AND rm.entrytype='form' AND ra.moduleid="+IssuePermissionModuleId);
        if(ds==null || ds.Tables.Count==0 || ds.Tables[0].Rows.Count==0) return result;
        DataRow row=ds.Tables[0].Rows[0]; bool full=Flag(row,"isfull");
        result.Full=full; result.Create=full || Flag(row,"isinsert");
        result.Edit=full || Flag(row,"isupdate"); result.Delete=full || Flag(row,"isdelete");
        result.Assign=full || Flag(row,"isassign"); result.ChangeOwner=full || Flag(row,"ischangeowner");
        result.View=full || Flag(row,"isview") || Flag(row,"isinsert") || result.Edit || result.Delete;
        return result;
    }
    private static void RequireView(int reg,int uid) { if(!Permissions(reg,uid).View) throw new HttpException(403,"You do not have permission to view issues."); }
    public int ischeckView, ischeckAllaccess, ischeckCreate, ischeckEdit, ischeckDelete, ischeckAssign, ischeckChangeOwner;
    // Corrected CSV contract: Full,View,Create,Edit,Delete,Assign,ChangeOwner.
    public static string get_EditDelete_Validation(string moduleid, string userid)
    {
        int module, uid;
        if (!Int32.TryParse(moduleid,out module) || module != IssuePermissionModuleId || !Int32.TryParse(userid,out uid))
            return "0,0,0,0,0,0,0";
        IssuePermissions permission=Permissions(GetCookieInt("cadmin","id"),uid);
        return String.Join(",",new[] {permission.Full,permission.View,permission.Create,permission.Edit,permission.Delete,permission.Assign,permission.ChangeOwner}.Select(value=>value ? "1" : "0").ToArray());
    }
    protected void bindpermision()
    {
        string uid=GetCookieInt("user","user_id").ToString(CultureInfo.InvariantCulture);
        string[] arS=get_EditDelete_Validation("57",uid).Split(',');
        //string[] arS=get_EditDelete_Validation("1060",uid).Split(',');
        ischeckAllaccess=Convert.ToInt32(arS[0],CultureInfo.InvariantCulture);
        ischeckView=Convert.ToInt32(arS[1],CultureInfo.InvariantCulture);
        ischeckCreate=Convert.ToInt32(arS[2],CultureInfo.InvariantCulture);
        ischeckEdit=Convert.ToInt32(arS[3],CultureInfo.InvariantCulture);
        ischeckDelete=Convert.ToInt32(arS[4],CultureInfo.InvariantCulture);
        ischeckAssign=Convert.ToInt32(arS[5],CultureInfo.InvariantCulture);
        ischeckChangeOwner=Convert.ToInt32(arS[6],CultureInfo.InvariantCulture);
        pagePermissions=new IssuePermissions {Full=ischeckAllaccess==1,View=ischeckView==1,Create=ischeckCreate==1,Edit=ischeckEdit==1,Delete=ischeckDelete==1,Assign=ischeckAssign==1,ChangeOwner=ischeckChangeOwner==1};
        if (ischeckView==0) Response.Redirect("/pms/accessdenied.aspx");
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        pageRegId = GetCookieInt("cadmin", "id");
        pageEmployeeId = GetCookieInt("user", "user_id");
        isadmin = IsAdmin(pageRegId, pageEmployeeId) ? 1 : 0;
        bindpermision();
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
            RequireView(regId,employeeId);
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
                StatusId = ReadInt(row, "StatusId"), PriorityId = ReadInt(row, "PriorityId"),
                StatusColor = ReadString(row, "StatusColor"),
                OwnerId = ReadInt(row, "OwnerId"), AssignToId = ReadInt(row, "AssignToId"),
                TeamIds = SplitIds(ReadString(row, "IssueTeam")).Select(v => { int id; return Int32.TryParse(v, out id) ? id : 0; }).Where(id => id > 0).Distinct().ToList(),
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
                IssueTypeName = ReadString(row, "issuetypename"),
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
            RequireView(regId,employeeId);
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


    public class IssueOption { public int Id { get; set; } public string Name { get; set; } public string Color { get; set; } }
    public class IssueEditRequest
    {
        public int Id { get; set; } public string Field { get; set; }
        public string IssueName { get; set; } public string Description { get; set; }
        public int StatusId { get; set; } public int PriorityId { get; set; }
        public int OwnerId { get; set; } public int AssignToId { get; set; }
        public List<int> TeamIds { get; set; }
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetIssueEditOptions()
    {
        try {
            int reg = GetCookieInt("cadmin", "id"), uid = GetCookieInt("user", "user_id");
            if (reg <= 0 || uid <= 0) throw new HttpException(401, "Login information was not found.");
            RequireView(reg,uid);
            DataSet ds = new DataAccess().GetDataSetInline("SELECT id,name,PrintColor FROM dbo.Pms_tbl_addIssueStatus WHERE status=1 AND archive=0 AND regid=" + reg + "; SELECT * FROM dbo.Pms_tbl_Priority WHERE status=1 AND archive=0 AND regid=" + reg);
            if (ds == null || ds.Tables.Count < 2) throw new Exception("Unable to load issue options.");
            return new { Success = true, Statuses = ds.Tables[0].Rows.Cast<DataRow>().Select(r => new IssueOption { Id=ReadInt(r,"id"), Name=ReadString(r,"name"), Color=ReadString(r,"PrintColor") }).ToList(),
                Priorities = ds.Tables[1].Rows.Cast<DataRow>().Select(r => new IssueOption { Id=ReadInt(r,"id"), Name=ReadString(r,"name"), Color=ReadFirstString(r,"PrintColor","color","ColorCode") }).ToList() };
        } catch(Exception ex) { return new { Success=false, Message=ex.Message }; }
    }
    private static string SqlText(string value) { return "N'" + (value ?? "").Replace("'", "''") + "'"; }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object SaveIssue(IssueEditRequest request)
    {
        try {
            int reg = GetCookieInt("cadmin", "id"), uid = GetCookieInt("user", "user_id");
            if (reg <= 0 || uid <= 0) throw new HttpException(401, "Login information was not found.");
            if (request == null || request.Id <= 0) throw new ArgumentException("Select an issue.");
            // Apply the same tenant and hierarchy visibility rules as the grid.
            string query = BuildIssueSql(reg.ToString(CultureInfo.InvariantCulture), uid.ToString(CultureInfo.InvariantCulture), IsAdmin(reg,uid), "ALL", "ALL", null, null, new int[0]);
            query = query.Replace("WHERE i.regid=", "WHERE i.id=" + request.Id + " AND i.regid=");
            DataSet access = new DataAccess().GetDataSetInline(query);
            if (access == null || access.Tables.Count == 0 || access.Tables[0].Rows.Count != 1) throw new HttpException(403, "You cannot modify this issue.");
            string field = request.Field ?? "", change;
            IssuePermissions permission=Permissions(reg,uid);
            if (!permission.View || (field=="Delete" ? !permission.Delete : field=="AssignTo" ? !permission.Assign : field=="Owner" ? !permission.ChangeOwner : !permission.Edit))
                throw new HttpException(403,"You do not have permission for this issue action.");
            if(field=="Edit" && request.OwnerId != ReadInt(access.Tables[0].Rows[0],"OwnerId") && !permission.ChangeOwner)
                throw new HttpException(403,"You do not have permission to change the owner.");
            if(field=="Edit" && request.AssignToId != ReadInt(access.Tables[0].Rows[0],"AssignToId") && !permission.Assign)
                throw new HttpException(403,"You do not have permission to change Assign To.");
            if (field == "Delete") change = "status=0,archive=1";
            else {
                if (field != "Edit" && field != "Status" && field != "Priority" && field != "AssignTo" && field != "Owner" && field != "Team") throw new ArgumentException("Invalid issue action.");
                List<string> changes = new List<string>();
                if (field == "Owner") {
                    IssueEmployeeResponse employees=GetIssueEmployees();
                    if(!employees.Success) throw new Exception(employees.Message);
                    int oldOwner=ReadInt(access.Tables[0].Rows[0],"OwnerId");
                    if(request.OwnerId<0 || (request.OwnerId>0 && request.OwnerId!=oldOwner && !employees.Employees.Any(e=>e.Id==request.OwnerId))) throw new ArgumentException("The employee is not available for this issue.");
                    changes.Add("Owner="+(request.OwnerId>0 ? request.OwnerId.ToString(CultureInfo.InvariantCulture) : "NULL"));
                }
                if (field == "Edit" || field == "Status") {
                    ValidateIssueOption("Pms_tbl_addIssueStatus",request.StatusId,reg);
                    changes.Add("IssueStatus=" + request.StatusId);
                }
                if (field == "Edit" || field == "Priority") {
                    ValidateIssueOption("Pms_tbl_Priority",request.PriorityId,reg);
                    changes.Add("Priority=" + request.PriorityId);
                }
                if (field == "Edit" || field == "Team" || field == "AssignTo") {
                    DataRow original=access.Tables[0].Rows[0];
                    var oldTeam=new HashSet<int>(); AddIds(oldTeam,ReadString(original,"IssueTeam"));
                    var team = field == "AssignTo" ? oldTeam.ToList() : (request.TeamIds ?? new List<int>()).Where(id=>id>0).Distinct().ToList();
                    int assignee = field == "Team" ? ReadInt(original,"AssignToId") : request.AssignToId;
                    int owner = field == "Edit" ? request.OwnerId : ReadInt(original,"OwnerId");
                    // New members must be among the login's permitted employee choices.
                    IssueEmployeeResponse employees=GetIssueEmployees();
                    if (!employees.Success) throw new Exception(employees.Message);
                    var allowed=new HashSet<int>(employees.Employees.Select(e=>e.Id));
                    var retained=new HashSet<int>(oldTeam); AddIds(retained,ReadString(original,"OwnerId")); AddIds(retained,ReadString(original,"AssignToId"));
                    foreach(int id in team.Concat(new[]{owner,assignee}).Where(id=>id>0))
                        if (!allowed.Contains(id) && !retained.Contains(id)) throw new ArgumentException("The employee is not available for this issue.");
                    if (field != "AssignTo") changes.Add("team="+SqlText(String.Join(",",team)));
                    if (field != "Team") changes.Add("AssignTo="+(assignee>0 ? assignee.ToString(CultureInfo.InvariantCulture) : "NULL"));
                    if (field == "Edit") changes.Add("Owner="+(owner>0 ? owner.ToString(CultureInfo.InvariantCulture) : "NULL"));
                }
                if (field == "Edit") {
                    if (String.IsNullOrWhiteSpace(request.IssueName)) throw new ArgumentException("Issue name is required.");
                    changes.Add("Issues="+SqlText(request.IssueName.Trim())); changes.Add("Description="+SqlText(request.Description));
                }
                change=String.Join(",",changes);
            }
            DataRow snapshot = access.Tables[0].Rows[0];
            string unchanged = " AND ISNULL(CONVERT(nvarchar(max),Issues),'')=" + SqlText(ReadString(snapshot,"IssueName")) +
                " AND ISNULL(CONVERT(nvarchar(max),Description),'')=" + SqlText(ReadString(snapshot,"Description")) +
                " AND ISNULL(IssueStatus,0)=" + ReadInt(snapshot,"StatusId") + " AND ISNULL(Priority,0)=" + ReadInt(snapshot,"PriorityId") +
                " AND ISNULL(CONVERT(nvarchar(2000),team),'')=" + SqlText(ReadString(snapshot,"IssueTeam")) +
                " AND ISNULL(CONVERT(nvarchar(50),Owner),'')=" + SqlText(ReadString(snapshot,"OwnerId")) +
                " AND ISNULL(CONVERT(nvarchar(50),AssignTo),'')=" + SqlText(ReadString(snapshot,"AssignToId"));
            DataSet saved = new DataAccess().GetDataSetInline("UPDATE dbo.Pms_tbl_Addissues SET " + change + " WHERE id=" + request.Id + " AND regid=" + reg + " AND status=1 AND archive=0" + unchanged + "; SELECT @@ROWCOUNT AS Changed;");
            if (saved==null || saved.Tables.Count==0 || ReadInt(saved.Tables[0].Rows[0],"Changed")!=1) throw new Exception("The issue changed or was removed. Refresh and try again.");
            return new { Success=true };
        } catch(Exception ex) { return new { Success=false, Message=ex.Message }; }
    }
    private static void ValidateIssueOption(string table,int id,int reg)
    {
        DataSet ds=new DataAccess().GetDataSetInline("SELECT id FROM dbo."+table+" WHERE id="+id+" AND regid="+reg+" AND status=1 AND archive=0");
        if(ds==null || ds.Tables.Count==0 || ds.Tables[0].Rows.Count!=1) throw new ArgumentException("Select an active status or priority.");
    }

    private static string BuildIssueSql(string reg, string employee, bool admin, string type,
        string status, DateTime? fromDate, DateTime? toDate, int[] selectedEmployees)
    {
        string sql =
    "SELECT i.id AS Id, ISNULL(i.Issues,'') AS IssueName, ISNULL(i.Description,'') AS Description, " +
    "ISNULL(CONVERT(nvarchar(50),i.Owner),'') AS OwnerId, ISNULL(CONVERT(nvarchar(2000),i.team),'') AS IssueTeam, " +
    "i.IssueStatus AS StatusId, i.Priority AS PriorityId, ps.PrintColor AS StatusColor, ISNULL(CONVERT(nvarchar(50),i.AssignTo),'') AS AssignToId, ISNULL(ps.name,'Unknown') AS StatusName, " +
    "CASE WHEN LOWER(LTRIM(RTRIM(ISNULL(ps.name,'')))) IN ('complete','completed') THEN 1 ELSE 0 END AS IsComplete, ISNULL(pr.name,'') AS PriorityName, " +
    "ISNULL(CONVERT(nvarchar(100),i.IssueType),'') AS IssueType, ISNULL(ist.name,'') AS IssueTypeName, " +
    "ISNULL(CONVERT(nvarchar(30),i.Issueon),'') AS EntityType, i.cdate AS CreateDate, i.EndDate AS TargetDate, " +
    "ISNULL(p.ProjectName,'') AS ProjectName, ISNULL(m.milestone,'') AS MilestoneName, " +
    "ISNULL(CONVERT(varchar(20),m.orderno),'') + " +
    "CASE WHEN t.orderno IS NULL THEN '' ELSE '.' + CONVERT(varchar(20),t.orderno) END + " +
    "CASE WHEN s.orderno IS NULL THEN '' ELSE '.' + CONVERT(varchar(20),s.orderno) END AS HierarchySrNo " +
    "FROM dbo.Pms_tbl_Addissues i WITH (NOLOCK) " +
    "LEFT JOIN dbo.Pms_tbl_IssueType ist WITH (NOLOCK) " +
    "ON ist.id=i.IssueType AND ist.regid=i.regid AND ist.status=1 AND ist.archive=0 " +
    "LEFT JOIN dbo.Pms_tbl_addIssueStatus ps WITH (NOLOCK) " +
    "ON ps.id=i.IssueStatus AND ps.regid=i.regid AND ps.status=1 AND ps.archive=0 " +
    "LEFT JOIN dbo.Pms_tbl_Priority pr WITH (NOLOCK) " +
    "ON pr.id=i.Priority AND pr.regid=i.regid AND pr.status=1 AND pr.archive=0 " +
    "LEFT JOIN dbo.Pms_tbl_AddProject p WITH (NOLOCK) " +
    "ON p.id=i.ProjectId AND p.regid=i.regid AND p.status=1 AND p.archive=0 " +
    "LEFT JOIN dbo.Pms_tbl_AddMilestone m WITH (NOLOCK) " +
    "ON m.id=i.Milestoneid AND m.regid=i.regid AND m.status=1 AND m.archive=0 " +
    "LEFT JOIN dbo.Pms_tbl_AddTask t WITH (NOLOCK) " +
    "ON t.id=COALESCE(NULLIF(i.taskid,0),NULLIF(i.tasklistid,0)) " +
    "AND t.regid=i.regid AND t.status=1 AND t.archive=0 " +
    "LEFT JOIN dbo.Pms_tbl_AddSubTask s WITH (NOLOCK) " +
    "ON s.id=i.subtaskid AND s.regid=i.regid AND s.status=1 AND s.archive=0 " +
    "WHERE i.regid=" + reg + " AND i.status=1 AND i.archive=0 "; 

        int statusId;
        if (Int32.TryParse(status,NumberStyles.None,CultureInfo.InvariantCulture,out statusId) && statusId>0)
            sql += "AND i.IssueStatus=" + statusId.ToString(CultureInfo.InvariantCulture) + " ";
        if (type != "ALL") sql += "AND UPPER(LTRIM(RTRIM(CONVERT(nvarchar(30),i.Issueon))))='" + type + "' ";
        if (status == "PENDING") sql += "AND CASE WHEN LOWER(LTRIM(RTRIM(ISNULL(ps.name,'')))) IN ('complete','completed') THEN 1 ELSE 0 END=0 ";
        else if (status == "COMPLETE") sql += "AND CASE WHEN LOWER(LTRIM(RTRIM(ISNULL(ps.name,'')))) IN ('complete','completed') THEN 1 ELSE 0 END=1 ";
        if (fromDate.HasValue && toDate.HasValue)
        {
            string from = fromDate.Value.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
            string to = toDate.Value.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
            sql += "AND ISNULL(i.cdate, i.StartDate) >= CONVERT(date,'" + from + "',112) " +
                   "AND ISNULL(i.cdate, i.StartDate) < DATEADD(day,1,CONVERT(date,'" + to + "',112)) ";
        }

        if (!admin)
        {
            sql += "AND " + UserDashboardIssueMatchClause(employee) + " ";
        }
        if (selectedEmployees != null && selectedEmployees.Length > 0)
        {
            string selectedMatch = String.Join(" OR ", selectedEmployees
                .Select(id => UserDashboardIssueMatchClause(id.ToString(CultureInfo.InvariantCulture))).ToArray());
            sql += "AND (" + selectedMatch + ") ";
        }

        return sql + "ORDER BY i.cdate DESC, i.id DESC";
    }

    // Match the issue binding used by userdashboard: a non-admin sees issues
    // attached to active subtasks assigned to that employee.
    private static string UserDashboardIssueMatchClause(string employee)
    {
        return "(UPPER(LTRIM(RTRIM(ISNULL(CONVERT(nvarchar(30),i.Issueon),''))))='SUBTASK' " +
               "AND EXISTS (SELECT 1 FROM dbo.Pms_tbl_assigntasks a WITH (NOLOCK) " +
               "INNER JOIN dbo.Pms_tbl_AddSubTask assignedSubtask WITH (NOLOCK) " +
               "ON assignedSubtask.id=a.subtaskid AND assignedSubtask.regid=a.regid " +
               "AND assignedSubtask.status=1 AND assignedSubtask.archive=0 " +
               "WHERE a.regid=i.regid AND a.subtaskid=i.subtaskid " +
               "AND a.ownerid=" + employee +
               " AND LOWER(LTRIM(RTRIM(ISNULL(a.tasktype,''))))='subtask' " +
               "AND a.status=1 AND a.archive=0))";
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
        int id;
        if (Int32.TryParse(value,NumberStyles.None,CultureInfo.InvariantCulture,out id) && id>0) return id.ToString(CultureInfo.InvariantCulture);
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
        public int StatusId { get; set; }
        public int PriorityId { get; set; }
        public int OwnerId { get; set; }
        public int AssignToId { get; set; }
        public List<int> TeamIds { get; set; }
        public string StatusColor { get; set; }
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
        public string IssueTypeName { get; set; }
        public string ProjectName { get; set; }
        public string MilestoneName { get; set; }
    }
}
