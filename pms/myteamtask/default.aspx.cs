using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using CommonClass;

public partial class products_default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Cookies["cadmin"] == null || Request.Cookies["user"] == null)
        {
            Response.Redirect("/login.aspx");
        }
    }

    #region check admin login
    protected static int checkadmin(string struserid = "0")
    {
        int retval = 0;
        DataAccess da = new DataAccess();
        string safeUserId = (struserid ?? "0").Replace("'", "''");
        string strsql = @"
select Manage_Profile.*
from Manage_Profile
inner join tbl_employee
    on Manage_Profile.id=cast(tbl_employee.roleid as int)
where tbl_employee.id='" + safeUserId + @"'
  and Manage_Profile.status=1
  and Manage_Profile.archive=0";

        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 &&
            ds.Tables[0].Rows[0]["id"].ToString() == "1")
        {
            retval = 1;
        }
        return retval;
    }
    #endregion

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<MyTeamEmployee> GetMyTeamEmployees()
    {
        List<MyTeamEmployee> employees = new List<MyTeamEmployee>();
        HttpCookie regCookie = HttpContext.Current.Request.Cookies["cadmin"];
        HttpCookie userCookie = HttpContext.Current.Request.Cookies["user"];
        if (regCookie == null || userCookie == null) return employees;

        string regid = regCookie["id"] ?? "0";
        string uid = userCookie["user_id"] ?? "0";
        int isadmin = String.IsNullOrWhiteSpace(uid) ? 0 : checkadmin(uid);

        DataAccess da = new DataAccess();
        SqlParameter[] prm = new SqlParameter[3];
        prm[0] = new SqlParameter("@regid", regid);
        prm[1] = new SqlParameter("@isadmin", isadmin);
        prm[2] = new SqlParameter("@uid", uid);
        DataSet ds = da.GetDataSet("bindemployee_forTransfer", prm);

        // Admin must always see every active employee. Keep the existing procedure
        // authoritative, but protect this page if that procedure returns no table/rows.
        if (isadmin == 1 && (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0))
        {
            string safeRegId = regid.Replace("'", "''");
            ds = da.GetDataSetInline(@"
select id, empcode, DisplayName, fileupload1
from tbl_employee with(nolock)
where regid='" + safeRegId + @"'
  and status=1
  and archive=0
order by DisplayName");
        }

        if (ds.Tables.Count == 0) return employees;
        foreach (DataRow row in ds.Tables[0].Rows)
        {
            string displayName = GetColumn(row, "DisplayName");
            string profileImage = GetColumn(row, "fileupload1");
            employees.Add(new MyTeamEmployee
            {
                id = GetColumn(row, "id"),
                empid = GetColumn(row, "empcode"),
                name = displayName,
                profileimage = profileImage,
                shortname = displayName.Length > 2 ? displayName.Substring(0, 2) : displayName,
                isadmin = isadmin
            });
        }

        // Keep the existing procedure result, but also include the logged-in user and
        // reporting employees (up to three levels). This prevents a valid direct report
        // from disappearing from My Team when bindemployee_forTransfer omits it.
        if (isadmin == 0)
        {
            int safeRegId = 0;
            int safeUid = 0;
            Int32.TryParse(regid, out safeRegId);
            Int32.TryParse(uid, out safeUid);
            if (safeRegId > 0 && safeUid > 0)
            {
                DataSet reportDs = da.GetDataSetInline(@"
select distinct id,empcode,DisplayName,fileupload1
from tbl_employee with(nolock)
where regid=" + safeRegId + @" and status=1 and archive=0
  and (
      id=" + safeUid + @"
      or councelor_id=" + safeUid + @"
      or councelor_id in (
          select id from tbl_employee with(nolock)
          where regid=" + safeRegId + @" and status=1 and archive=0 and councelor_id=" + safeUid + @"
      )
      or councelor_id in (
          select id from tbl_employee with(nolock)
          where regid=" + safeRegId + @" and status=1 and archive=0
            and councelor_id in (
                select id from tbl_employee with(nolock)
                where regid=" + safeRegId + @" and status=1 and archive=0 and councelor_id=" + safeUid + @"
            )
      )
  )
order by DisplayName");

                HashSet<string> existingIds = new HashSet<string>(employees.Select(x => x.id ?? String.Empty));
                if (reportDs.Tables.Count > 0)
                {
                    foreach (DataRow row in reportDs.Tables[0].Rows)
                    {
                        string reportId = GetColumn(row, "id");
                        if (String.IsNullOrWhiteSpace(reportId) || existingIds.Contains(reportId)) continue;
                        string displayName = GetColumn(row, "DisplayName");
                        employees.Add(new MyTeamEmployee
                        {
                            id = reportId,
                            empid = GetColumn(row, "empcode"),
                            name = displayName,
                            profileimage = GetColumn(row, "fileupload1"),
                            shortname = displayName.Length > 2 ? displayName.Substring(0, 2) : displayName,
                            isadmin = isadmin
                        });
                        existingIds.Add(reportId);
                    }
                }
            }
        }

        return employees.OrderBy(x => x.name).ToList();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<MyTeamProject> GetMyTeamProjects()
    {
        List<MyTeamProject> projects = new List<MyTeamProject>();

        HttpCookie regCookie = HttpContext.Current.Request.Cookies["cadmin"];
        HttpCookie userCookie = HttpContext.Current.Request.Cookies["user"];
        if (regCookie == null || userCookie == null) return projects;

        int regid = 0;
        int uid = 0;
        Int32.TryParse(regCookie["id"] ?? "0", out regid);
        Int32.TryParse(userCookie["user_id"] ?? "0", out uid);

        if (regid <= 0 || uid <= 0) return projects;

        int isadmin = checkadmin(uid.ToString());

        DataAccess da = new DataAccess();
        SqlParameter[] prm = new SqlParameter[3];
        prm[0] = new SqlParameter("@regid", SqlDbType.Int);
        prm[0].Value = regid;
        prm[1] = new SqlParameter("@owner", SqlDbType.Int);
        prm[1].Value = uid;
        prm[2] = new SqlParameter("@isadmin", SqlDbType.Int);
        prm[2].Value = isadmin;

        DataSet ds = da.GetDataSet("usp_Pms_MyTeam_GetProjects", prm);

        if (ds.Tables.Count == 0) return projects;

        foreach (DataRow row in ds.Tables[0].Rows)
        {
            projects.Add(new MyTeamProject
            {
                id = GetColumn(row, "id"),
                ProjectName = GetColumn(row, "ProjectName")
            });
        }

        return projects;
    }


    // ========================================================================
    // MY TEAM LOCAL SCHEDULER
    // All scheduler code used by /pms/myteamtask now lives in this page.
    // It does not call /pms/assignprojects/default.aspx.
    // The hierarchy / availability rules intentionally mirror Assign Project.
    // ========================================================================

    private static int CurrentRegId()
    {
        int value = 0;
        HttpCookie c = HttpContext.Current.Request.Cookies["cadmin"];
        if (c != null) Int32.TryParse(c["id"] ?? "0", out value);
        return value;
    }

    private static int CurrentUserId()
    {
        int value = 0;
        HttpCookie c = HttpContext.Current.Request.Cookies["user"];
        if (c != null) Int32.TryParse(c["user_id"] ?? "0", out value);
        return value;
    }

    private static DateTime ParseMyTeamDate(string value)
    {
        DateTime parsed;
        string text = (value ?? String.Empty).Trim();
        string[] formats = new string[]
        {
            "yyyy-MM-dd", "yyyyMMdd", "dd/MM/yyyy", "d/M/yyyy",
            "MM/dd/yyyy", "M/d/yyyy", "dd-MM-yyyy", "d-M-yyyy"
        };

        if (DateTime.TryParseExact(text, formats, CultureInfo.InvariantCulture,
            DateTimeStyles.AllowWhiteSpaces, out parsed))
            return parsed.Date;

        if (DateTime.TryParse(text, CultureInfo.InvariantCulture,
            DateTimeStyles.AllowWhiteSpaces, out parsed))
            return parsed.Date;

        return DateTime.Today;
    }

    private static TimeSpan ParseMyTeamTime(string value, string fallback)
    {
        DateTime parsed;
        string text = String.IsNullOrWhiteSpace(value) ? fallback : value.Trim();
        string[] formats = new string[]
        {
            "hh:mm tt", "h:mm tt", "HH:mm", "H:mm", "HH:mm:ss", "H:mm:ss"
        };

        if (DateTime.TryParseExact(text, formats, CultureInfo.InvariantCulture,
            DateTimeStyles.AllowWhiteSpaces, out parsed))
            return parsed.TimeOfDay;

        if (DateTime.TryParseExact(fallback, formats, CultureInfo.InvariantCulture,
            DateTimeStyles.AllowWhiteSpaces, out parsed))
            return parsed.TimeOfDay;

        return TimeSpan.Zero;
    }

    private static string ToMyTeamSqlDateTime(DateTime value)
    {
        return value.ToString("yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
    }

    private static int MyTeamDurationMinutes(string value, int fallbackMinutes)
    {
        if (String.IsNullOrWhiteSpace(value)) return fallbackMinutes;

        string text = value.Trim().Replace('/', ':');
        string[] parts = text.Split(':');
        int h = 0;
        int m = 0;
        if (parts.Length >= 1 && Int32.TryParse(parts[0], out h))
        {
            if (parts.Length > 1) Int32.TryParse(parts[1], out m);
            return Math.Max(0, h * 60 + m);
        }

        decimal hours;
        if (Decimal.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out hours))
            return Math.Max(0, Convert.ToInt32(Math.Round(hours * 60M, 0, MidpointRounding.AwayFromZero)));

        return fallbackMinutes;
    }

    private static string MyTeamDurationText(int minutes)
    {
        minutes = Math.Max(0, minutes);
        return (minutes / 60).ToString("00", CultureInfo.InvariantCulture) + ":" +
               (minutes % 60).ToString("00", CultureInfo.InvariantCulture);
    }

    private static string NormalizeMyTeamType(string value)
    {
        string type = (value ?? String.Empty).Trim().ToLowerInvariant();
        if (type == "sub task" || type == "sub-task") type = "subtask";
        return type;
    }

    private static int? MyTeamNullableInt(object value)
    {
        if (value == null || value == DBNull.Value) return null;
        int result;
        if (Int32.TryParse(Convert.ToString(value, CultureInfo.InvariantCulture), out result))
            return result;
        return null;
    }

    private static DateTime? MyTeamNullableDate(object value)
    {
        if (value == null || value == DBNull.Value) return null;
        try { return Convert.ToDateTime(value, CultureInfo.InvariantCulture); }
        catch { return null; }
    }

    private static bool CsvHasEmployee(string csv, int employeeId)
    {
        if (employeeId <= 0 || String.IsNullOrWhiteSpace(csv)) return false;
        string wanted = employeeId.ToString(CultureInfo.InvariantCulture);
        return csv.Split(',')
            .Select(x => (x ?? String.Empty).Trim())
            .Any(x => String.Equals(x, wanted, StringComparison.OrdinalIgnoreCase));
    }

    private static void AddCsvEmployees(HashSet<int> result, string csv)
    {
        if (result == null || String.IsNullOrWhiteSpace(csv)) return;
        foreach (string item in csv.Split(','))
        {
            int id;
            if (Int32.TryParse((item ?? String.Empty).Trim(), out id) && id > 0)
                result.Add(id);
        }
    }

    private static List<int> ParseIdList(string csv)
    {
        List<int> ids = new List<int>();
        if (String.IsNullOrWhiteSpace(csv)) return ids;
        foreach (string item in csv.Split(','))
        {
            int id;
            if (Int32.TryParse((item ?? String.Empty).Trim(), out id) && id > 0 && !ids.Contains(id))
                ids.Add(id);
        }
        return ids;
    }


    private static bool CanCurrentUserManageEmployee(DataAccess da, int regid, int employeeId)
    {
        int uid = CurrentUserId();
        if (regid <= 0 || uid <= 0 || employeeId <= 0) return false;
        if (checkadmin(uid.ToString(CultureInfo.InvariantCulture)) == 1) return true;

        try
        {
            return GetMyTeamEmployees().Any(x => String.Equals(
                x.id,
                employeeId.ToString(CultureInfo.InvariantCulture),
                StringComparison.OrdinalIgnoreCase));
        }
        catch
        {
            return false;
        }
    }

    private static string GetMyTeamEmployeeShift(int employeeId)
    {
        string shiftStart = "09:00 AM";
        string shiftEnd = "06:00 PM";
        DataAccess da = new DataAccess();
        int regid = CurrentRegId();

        try
        {
            //inner join tbl_shift shft with(nolock) on emp.ddlShift=shft.id
            string sql = @"
select top(1) shft.starttime,shft.endtime
from tbl_employee emp with(nolock)
inner join tbl_shift shft with(nolock) on emp.ShiftType=shft.id
where emp.regid=" + regid + @"
  and emp.status=1 and emp.archive=0
  and emp.id=" + employeeId;

            DataSet ds = da.GetDataSetInline(sql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!String.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["starttime"].ToString()))
                    shiftStart = ds.Tables[0].Rows[0]["starttime"].ToString();
                if (!String.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["endtime"].ToString()))
                    shiftEnd = ds.Tables[0].Rows[0]["endtime"].ToString();
            }
        }
        catch { }

        DateTime s = DateTime.Today.Add(ParseMyTeamTime(shiftStart, "09:00 AM"));
        DateTime e = DateTime.Today.Add(ParseMyTeamTime(shiftEnd, "06:00 PM"));
        if (e <= s) e = e.AddDays(1);
        int hours = Math.Max(1, Convert.ToInt32(Math.Round((e - s).TotalHours, 0, MidpointRounding.AwayFromZero)));
        return shiftStart + "-" + hours.ToString(CultureInfo.InvariantCulture) + "-" + shiftEnd;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<MyTeamEmployee> GetMyTeamProjectMembers(string projectId)
    {
        List<MyTeamEmployee> result = new List<MyTeamEmployee>();
        int pid;
        int regid = CurrentRegId();
        if (regid <= 0 || !Int32.TryParse(projectId, out pid) || pid <= 0) return result;

        DataAccess da = new DataAccess();
        HashSet<int> ids = new HashSet<int>();

        string sql = @"
select isnull(Owner,'') Owner,isnull(team,'') team
from Pms_tbl_AddProject with(nolock)
where regid=" + regid + " and id=" + pid + @" and status=1 and archive=0
union all
select isnull(Owner,''),isnull(team,'')
from Pms_tbl_AddMilestone with(nolock)
where regid=" + regid + " and ProjectId=" + pid + @" and status=1 and archive=0
union all
select isnull(Owner,''),isnull(team,'')
from Pms_tbl_Addtask with(nolock)
where regid=" + regid + " and ProjectId=" + pid + @" and status=1 and archive=0
union all
select isnull(Owner,''),isnull(team,'')
from Pms_tbl_Addsubtask with(nolock)
where regid=" + regid + " and ProjectId=" + pid + @" and status=1 and archive=0";

        DataSet ds = da.GetDataSetInline(sql);
        if (ds.Tables.Count == 0) return result;

        foreach (DataRow row in ds.Tables[0].Rows)
        {
            AddCsvEmployees(ids, GetColumn(row, "Owner"));
            AddCsvEmployees(ids, GetColumn(row, "team"));
        }

        if (ids.Count == 0) return result;
        string idList = String.Join(",", ids.OrderBy(x => x));
        DataSet empDs = da.GetDataSetInline(@"
select id,empcode,DisplayName,fileupload1
from tbl_employee with(nolock)
where regid=" + regid + @" and status=1 and archive=0
  and id in(" + idList + @")
order by DisplayName");

        if (empDs.Tables.Count == 0) return result;
        int isadmin = checkadmin(CurrentUserId().ToString(CultureInfo.InvariantCulture));
        foreach (DataRow row in empDs.Tables[0].Rows)
        {
            string name = GetColumn(row, "DisplayName");
            result.Add(new MyTeamEmployee
            {
                id = GetColumn(row, "id"),
                empid = GetColumn(row, "empcode"),
                name = name,
                profileimage = GetColumn(row, "fileupload1"),
                shortname = name.Length > 2 ? name.Substring(0, 2) : name,
                isadmin = isadmin
            });
        }
        return result;
    }

    private static List<MyTeamAssignmentRow> LoadMyTeamAssignmentRows(int employeeId, DateTime selectedDate)
    {
        List<MyTeamAssignmentRow> list = new List<MyTeamAssignmentRow>();
        int regid = CurrentRegId();
        if (regid <= 0 || employeeId <= 0) return list;

        DateTime rangeStart = selectedDate.Date.AddDays(-30);
        DateTime rangeEnd = selectedDate.Date.AddDays(30);
        DataAccess da = new DataAccess();

        string sql = @"
select
 a.id assignid,a.uid,a.cdate,a.assigndatetime,a.tasktype,a.projectid,a.milestoneid,a.taskid,a.subtaskid,a.ownerid,
 a.startdatetime,a.enddatetime,isnull(a.taskduration,'01:00') taskduration,
 p.ProjectName,m.Milestone,t.taskname,st.Subtaskname,st.SubTaskDescription,
 case when a.tasktype='milestone' then m.miledescription when a.tasktype='task' then t.taskdescripton else st.SubTaskDescription end Description
from Pms_tbl_assigntasks a with(nolock)
left join Pms_tbl_AddProject p with(nolock) on p.id=a.projectid
left join Pms_tbl_AddMilestone m with(nolock) on m.id=a.milestoneid
left join Pms_tbl_Addtask t with(nolock) on t.id=a.taskid
left join Pms_tbl_Addsubtask st with(nolock) on st.id=a.subtaskid
where a.status=1 and a.archive=0
 and a.regid=" + regid + @"
 and a.ownerid=" + employeeId + @"
 and a.startdatetime < dateadd(day,1,convert(datetime,'" + rangeEnd.ToString("yyyyMMdd", CultureInfo.InvariantCulture) + @"',112))
 and isnull(a.enddatetime,a.startdatetime) >= convert(datetime,'" + rangeStart.ToString("yyyyMMdd", CultureInfo.InvariantCulture) + @"',112)
order by a.startdatetime,a.assigndatetime";

        DataSet ds = da.GetDataSetInline(sql);
        if (ds.Tables.Count == 0) return list;

        foreach (DataRow row in ds.Tables[0].Rows)
        {
            string type = NormalizeMyTeamType(GetColumn(row, "tasktype"));
            int entityId = 0;
            if (type == "milestone") entityId = MyTeamNullableInt(row["milestoneid"]) ?? 0;
            else if (type == "task") entityId = MyTeamNullableInt(row["taskid"]) ?? 0;
            else entityId = MyTeamNullableInt(row["subtaskid"]) ?? 0;

            DateTime? start = MyTeamNullableDate(row["startdatetime"]);
            DateTime? end = MyTeamNullableDate(row["enddatetime"]);
            string startTime = start.HasValue ? start.Value.ToString("hh:mm tt", CultureInfo.InvariantCulture) : "09:00 AM";

            list.Add(new MyTeamAssignmentRow
            {
                id = entityId,
                assignid = MyTeamNullableInt(row["assignid"]),
                cdate = MyTeamNullableDate(row["cdate"]),
                assigndatetime = MyTeamNullableDate(row["assigndatetime"]),
                tasktype = type,
                ProjectId = MyTeamNullableInt(row["projectid"]),
                ProjectName = GetColumn(row, "ProjectName"),
                Milestoneid = MyTeamNullableInt(row["milestoneid"]),
                Milestone = GetColumn(row, "Milestone"),
                Task = MyTeamNullableInt(row["taskid"]),
                Taskname = GetColumn(row, "taskname"),
                Subtaskname = GetColumn(row, "Subtaskname"),
                Description = GetColumn(row, "Description"),
                SubTaskDescription = GetColumn(row, "SubTaskDescription"),
                owner = GetColumn(row, "ownerid"),
                Startdate = start,
                Targetdate = end,
                StartDateKey = start.HasValue ? start.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : "",
                TargetDateKey = end.HasValue ? end.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : "",
                TaskDuration = GetColumn(row, "taskduration"),
                starttime = startTime,
                StartTime = startTime
            });
        }
        return list;
    }

    private static string MyTeamRowTitle(MyTeamAssignmentRow row)
    {
        string type = NormalizeMyTeamType(row.tasktype);
        if (type == "subtask") return !String.IsNullOrWhiteSpace(row.Subtaskname) ? row.Subtaskname : "Subtask";
        if (type == "task") return !String.IsNullOrWhiteSpace(row.Taskname) ? row.Taskname : "Task";
        return !String.IsNullOrWhiteSpace(row.Milestone) ? row.Milestone : "Milestone";
    }

    private static List<MyTeamTaskSegment> BuildMyTeamSegments(List<MyTeamAssignmentRow> tasks, string shiftStart, string shiftEnd)
    {
        List<MyTeamTaskSegment> segments = new List<MyTeamTaskSegment>();
        if (tasks == null || tasks.Count == 0) return segments;

        DateTime shiftStartTime = DateTime.Today.Add(ParseMyTeamTime(shiftStart, "09:00 AM"));
        DateTime shiftEndTime = DateTime.Today.Add(ParseMyTeamTime(shiftEnd, "06:00 PM"));
        bool night = shiftEndTime <= shiftStartTime;
        if (night) shiftEndTime = shiftEndTime.AddDays(1);

        var ordered = tasks
            .OrderBy(x => x.assigndatetime ?? x.cdate ?? x.Startdate ?? DateTime.Now)
            .ThenBy(x => (x.Startdate ?? DateTime.Now).Date)
            .ThenBy(x => ParseMyTeamTime(x.starttime, shiftStart))
            .ToList();

        Func<MyTeamAssignmentRow, DateTime> getStart = delegate(MyTeamAssignmentRow x)
        {
            DateTime d = (x.Startdate ?? DateTime.Now).Date;
            DateTime s = d.Add(ParseMyTeamTime(x.starttime, shiftStart));
            if (night && s < d.Add(shiftStartTime.TimeOfDay)) s = s.AddDays(1);
            return s;
        };

        Func<MyTeamAssignmentRow, DateTime> getOrder = delegate(MyTeamAssignmentRow x)
        {
            return x.assigndatetime ?? x.cdate ?? getStart(x);
        };

        foreach (MyTeamAssignmentRow t in ordered)
        {
            int remaining = MyTeamDurationMinutes(t.TaskDuration, 60);
            if (remaining <= 0) continue;

            DateTime workDate = (t.Startdate ?? DateTime.Now).Date;
            DateTime cursor = getStart(t);
            DateTime currentOrder = getOrder(t);
            int safety = 0;

            while (remaining > 0 && safety < 120)
            {
                safety++;
                DateTime dayStart = workDate.Date.Add(shiftStartTime.TimeOfDay);
                DateTime dayEnd = workDate.Date.Add(shiftEndTime.TimeOfDay);
                if (dayEnd <= dayStart) dayEnd = dayEnd.AddDays(1);

                if (night && cursor < dayStart) cursor = cursor.AddDays(1);
                if (cursor < dayStart) cursor = dayStart;
                if (cursor >= dayEnd)
                {
                    workDate = workDate.AddDays(1);
                    cursor = workDate.Date.Add(shiftStartTime.TimeOfDay);
                    continue;
                }

                var obstacles = ordered
                    .Where(x => !Object.ReferenceEquals(x, t))
                    .Where(x =>
                    {
                        DateTime order = getOrder(x);
                        return order < currentOrder || (order == currentOrder && getStart(x) < getStart(t));
                    })
                    .Select(x => new
                    {
                        Start = getStart(x),
                        End = getStart(x).AddMinutes(MyTeamDurationMinutes(x.TaskDuration, 60))
                    })
                    .Where(x => x.End > dayStart && x.Start < dayEnd)
                    .OrderBy(x => x.Start)
                    .ToList();

                DateTime freeEnd = dayEnd;
                bool jumped = false;
                foreach (var o in obstacles)
                {
                    if (cursor >= o.End) continue;
                    if (cursor >= o.Start && cursor < o.End)
                    {
                        cursor = o.End;
                        jumped = true;
                        break;
                    }
                    if (o.Start > cursor)
                    {
                        freeEnd = o.Start < freeEnd ? o.Start : freeEnd;
                        break;
                    }
                }
                if (jumped) continue;
                if (freeEnd <= cursor)
                {
                    workDate = workDate.AddDays(1);
                    cursor = workDate.Date.Add(shiftStartTime.TimeOfDay);
                    continue;
                }

                int allocate = Math.Min(remaining, (int)(freeEnd - cursor).TotalMinutes);
                if (allocate <= 0) break;

                segments.Add(new MyTeamTaskSegment
                {
                    assignid = t.assignid,
                    taskid = t.id,
                    tasktype = t.tasktype,
                    taskname = MyTeamRowTitle(t),
                    workDateKey = cursor.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    DurationMinutes = allocate,
                    TaskDuration = t.TaskDuration,
                    StartTime = cursor.ToString("hh:mm tt", CultureInfo.InvariantCulture),
                    EndTime = cursor.AddMinutes(allocate).ToString("hh:mm tt", CultureInfo.InvariantCulture),
                    mid = Convert.ToString(t.Milestoneid ?? 0, CultureInfo.InvariantCulture),
                    tid = Convert.ToString(t.Task ?? 0, CultureInfo.InvariantCulture)
                });

                remaining -= allocate;
                cursor = cursor.AddMinutes(allocate);
                if (remaining > 0 && cursor >= dayEnd)
                {
                    workDate = workDate.AddDays(1);
                    cursor = workDate.Date.Add(shiftStartTime.TimeOfDay);
                }
            }
        }
        return segments;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<MyTeamScheduleUser> GetMyTeamScheduleUsers(string userIds, string checkdate)
    {
        List<MyTeamScheduleUser> result = new List<MyTeamScheduleUser>();
        List<int> ids = ParseIdList(userIds);
        int regid = CurrentRegId();
        if (regid <= 0 || ids.Count == 0) return result;

        DateTime selected = ParseMyTeamDate(checkdate);
        string idList = String.Join(",", ids);
        DataAccess da = new DataAccess();
        DataSet ds = da.GetDataSetInline(@"
select id,empcode,DisplayName,fileupload1
from tbl_employee with(nolock)
where regid=" + regid + @" and status=1 and archive=0
 and id in(" + idList + @")
order by DisplayName");

        if (ds.Tables.Count == 0) return result;
        foreach (DataRow row in ds.Tables[0].Rows)
        {
            int employeeId = Convert.ToInt32(row["id"], CultureInfo.InvariantCulture);
            string shift = GetMyTeamEmployeeShift(employeeId);
            string[] parts = shift.Split('-');
            string shiftStart = parts.Length > 0 ? parts[0] : "09:00 AM";
            string shiftHours = parts.Length > 1 ? parts[1] : "9";
            string shiftEnd = parts.Length > 2 ? parts[2] : "06:00 PM";
            List<MyTeamAssignmentRow> rows = LoadMyTeamAssignmentRows(employeeId, selected);

            result.Add(new MyTeamScheduleUser
            {
                id = employeeId.ToString(CultureInfo.InvariantCulture),
                empid = GetColumn(row, "empcode"),
                name = GetColumn(row, "DisplayName"),
                profileimage = GetColumn(row, "fileupload1"),
                shortname = GetColumn(row, "DisplayName").Length > 2 ? GetColumn(row, "DisplayName").Substring(0, 2) : GetColumn(row, "DisplayName"),
                ShiftStart = shiftStart,
                ShiftHours = shiftHours,
                ShiftEnd = shiftEnd,
                usertasks = rows,
                taskSegments = BuildMyTeamSegments(rows, shiftStart, shiftEnd)
            });
        }
        return result;
    }

    private class MyTeamHierarchyNode
    {
        public string Type;
        public int Id;
        public int ProjectId;
        public int MilestoneId;
        public int TaskId;
        public string Name;
        public string OrderNo;
        public string Owner;
        public string Team;
        public DateTime? StartDate;
        public DateTime? TargetDate;
        public string Duration;
        public bool IsComplete;
    }

    private static string AssignmentKey(string type, int id)
    {
        type = NormalizeMyTeamType(type);
        return type + ":" + id.ToString(CultureInfo.InvariantCulture);
    }

    private static HashSet<string> ActiveAssignmentKeys(DataAccess da, int regid, int projectId)
    {
        HashSet<string> result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        DataSet ds = da.GetDataSetInline(@"
select tasktype,milestoneid,taskid,subtaskid
from Pms_tbl_assigntasks with(nolock)
where regid=" + regid + " and projectid=" + projectId + @"
 and status=1 and archive=0");
        if (ds.Tables.Count == 0) return result;

        foreach (DataRow row in ds.Tables[0].Rows)
        {
            string type = NormalizeMyTeamType(GetColumn(row, "tasktype"));
            int id = 0;
            if (type == "milestone") id = MyTeamNullableInt(row["milestoneid"]) ?? 0;
            else if (type == "task") id = MyTeamNullableInt(row["taskid"]) ?? 0;
            else if (type == "subtask") id = MyTeamNullableInt(row["subtaskid"]) ?? 0;
            if (id > 0) result.Add(AssignmentKey(type, id));
        }
        return result;
    }

    private static HashSet<string> LoadAssignProjectAvailableKeys(DataAccess da, int regid, int projectId, int milestoneId, int recordId, string type)
    {
        HashSet<string> result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        try
        {
            int labelId = type == "milestone" ? 3 : (type == "task" ? 2 : 1);
            SqlParameter[] prm = new SqlParameter[5];
            prm[0] = new SqlParameter("@regid", regid);
            prm[1] = new SqlParameter("@projectid", projectId);
            prm[2] = new SqlParameter("@milestoneid", milestoneId);
            prm[3] = new SqlParameter("@recordid", recordId);
            prm[4] = new SqlParameter("@labelid", labelId);
            DataSet ds = da.GetDataSet("pms_GetSubtaskList_unassign_labelview", prm);
            if (ds.Tables.Count == 0) return result;

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string rowType = row.Table.Columns.Contains("tasktype")
                    ? NormalizeMyTeamType(GetColumn(row, "tasktype"))
                    : "subtask";
                if (String.IsNullOrWhiteSpace(rowType)) rowType = "subtask";
                int id = row.Table.Columns.Contains("id") ? (MyTeamNullableInt(row["id"]) ?? 0) : 0;
                if (id > 0) result.Add(AssignmentKey(rowType, id));
            }
        }
        catch { }
        return result;
    }

    // ------------------------------------------------------------------------
    // Local copy of Assign Project unassigned-item process.
    // This page calls pms_GetSubtaskList_unassign_labelview directly so
    // /pms/myteamtask is independent from /pms/assignprojects/default.aspx.
    // ------------------------------------------------------------------------
    private static List<unassignsubtasklist> LoadMyTeamUnassignedTaskList(
        DataAccess da,
        int regid,
        int projectId,
        int milestoneId,
        int recordId,
        string rtype,
        HashSet<string> activeAssignmentKeys)
    {
        List<unassignsubtasklist> result = new List<unassignsubtasklist>();
        string normalizedType = NormalizeMyTeamType(rtype);
        int labelId = normalizedType == "milestone" ? 3 : (normalizedType == "task" ? 2 : 1);

        try
        {
            SqlParameter[] prm = new SqlParameter[5];
            prm[0] = new SqlParameter("@regid", regid);
            prm[1] = new SqlParameter("@projectid", projectId);
            prm[2] = new SqlParameter("@milestoneid", milestoneId);
            prm[3] = new SqlParameter("@recordid", recordId);
            prm[4] = new SqlParameter("@labelid", labelId);

            DataSet ds = da.GetDataSet("pms_GetSubtaskList_unassign_labelview", prm);
            if (ds.Tables.Count == 0) return result;

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string rowType = row.Table.Columns.Contains("tasktype")
                    ? NormalizeMyTeamType(GetColumn(row, "tasktype"))
                    : normalizedType;

                if (String.IsNullOrWhiteSpace(rowType))
                    rowType = normalizedType == "task" || normalizedType == "subtask"
                        ? "subtask"
                        : normalizedType;

                int rowId = row.Table.Columns.Contains("id")
                    ? (MyTeamNullableInt(row["id"]) ?? 0)
                    : 0;

                string assignmentKey = rowId > 0
                    ? AssignmentKey(rowType, rowId)
                    : String.Empty;

                bool isAlreadyAssigned = !String.IsNullOrWhiteSpace(assignmentKey) &&
                    activeAssignmentKeys != null &&
                    activeAssignmentKeys.Contains(assignmentKey);

                result.Add(new unassignsubtasklist
                {
                    id = rowId > 0 ? (int?)rowId : null,
                    tasktype = rowType,
                    isassigned = isAlreadyAssigned,
                    ProjectId = row.Table.Columns.Contains("ProjectId") ? MyTeamNullableInt(row["ProjectId"]) : projectId,
                    ProjectName = GetColumn(row, "ProjectName"),
                    Powner = GetColumn(row, "powner"),
                    Milestoneid = row.Table.Columns.Contains("Milestoneid") ? MyTeamNullableInt(row["Milestoneid"]) : milestoneId,
                    Milestone = GetColumn(row, "Milestone"),
                    Task = row.Table.Columns.Contains("Task") ? MyTeamNullableInt(row["Task"]) : null,
                    Taskname = GetColumn(row, "taskname"),
                    Subtaskname = GetColumn(row, "Subtaskname"),
                    SubTaskDescription = GetColumn(row, "SubTaskDescription"),
                    owner = GetColumn(row, "owner"),
                    team = GetColumn(row, "team"),
                    Startdate = row.Table.Columns.Contains("Startdate") && row["Startdate"] != DBNull.Value
                        ? Convert.ToDateTime(row["Startdate"], CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                        : String.Empty,
                    Targetdate = row.Table.Columns.Contains("Targetdate") && row["Targetdate"] != DBNull.Value
                        ? Convert.ToDateTime(row["Targetdate"], CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                        : String.Empty,
                    TaskDuration = GetColumn(row, "TaskDuration"),
                    P_Status = row.Table.Columns.Contains("statusid") ? MyTeamNullableInt(row["statusid"]) : null,
                    status_name = GetColumn(row, "statusname"),
                    status_color = GetColumn(row, "statuscolor"),
                    Priority = row.Table.Columns.Contains("Priority") ? MyTeamNullableInt(row["Priority"]) : null,
                    priority_name = GetColumn(row, "priorityname"),
                    priority_color = GetColumn(row, "prioritycolor")
                });
            }
        }
        catch
        {
            return new List<unassignsubtasklist>();
        }

        return result;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<unassignsubtasklist> getunassigntasklist(
        string ProjectId,
        string MilestoneId,
        string RecordId,
        string Rtype)
    {
        int regid = CurrentRegId();
        int projectId = 0;
        int milestoneId = 0;
        int recordId = 0;
        if (regid <= 0 ||
            !Int32.TryParse(ProjectId, out projectId) || projectId <= 0 ||
            !Int32.TryParse(MilestoneId, out milestoneId) || milestoneId <= 0 ||
            !Int32.TryParse(RecordId, out recordId) || recordId <= 0)
            return new List<unassignsubtasklist>();

        DataAccess da = new DataAccess();
        HashSet<string> active = ActiveAssignmentKeys(da, regid, projectId);
        return LoadMyTeamUnassignedTaskList(
            da,
            regid,
            projectId,
            milestoneId,
            recordId,
            Rtype,
            active);
    }

    private static bool IsEmployeeInSubtaskTeam(DataAccess da, int regid, int employeeId, int subtaskId)
    {
        if (employeeId <= 0 || subtaskId <= 0) return false;
        DataSet ds = da.GetDataSetInline(@"
select top(1) isnull(team,'') team
from Pms_tbl_Addsubtask with(nolock)
where regid=" + regid + " and id=" + subtaskId + @"
  and status=1 and archive=0");
        if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return false;
        return CsvHasEmployee(GetColumn(ds.Tables[0].Rows[0], "team"), employeeId);
    }

    private static bool SubtaskAvailableOnSelectedDate(DataRow row, DateTime selected)
    {
        if (row == null) return false;
        if (row.Table.Columns.Contains("iscomplete") && row["iscomplete"] != DBNull.Value)
        {
            int completed = 0;
            Int32.TryParse(row["iscomplete"].ToString(), out completed);
            if (completed == 1) return false;
        }

        DateTime? startDate = row.Table.Columns.Contains("StartDate")
            ? MyTeamNullableDate(row["StartDate"])
            : null;
        if (startDate.HasValue && selected.Date < startDate.Value.Date) return false;
        return true;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static MyTeamAssignableTreeResult GetMyTeamAssignableTree(
        string employeeId,
        string workDate,
        string projectId)
    {
        MyTeamAssignableTreeResult result = new MyTeamAssignableTreeResult();
        int regid = CurrentRegId();
        int empid = 0;
        int selectedProjectId = 0;
        Int32.TryParse(employeeId ?? "0", out empid);
        Int32.TryParse(projectId ?? "0", out selectedProjectId);
        if (regid <= 0 || empid <= 0) return result;

        DataAccess da = new DataAccess();
        if (!CanCurrentUserManageEmployee(da, regid, empid)) return result;

        DateTime selectedDate = ParseMyTeamDate(workDate);
        List<MyTeamProject> loginProjects = GetMyTeamProjects();
        if (selectedProjectId > 0)
        {
            loginProjects = loginProjects
                .Where(x => String.Equals(x.id, selectedProjectId.ToString(CultureInfo.InvariantCulture), StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        foreach (MyTeamProject loginProject in loginProjects)
        {
            int pid = 0;
            if (!Int32.TryParse(loginProject.id, out pid) || pid <= 0) continue;

            HashSet<string> active = ActiveAssignmentKeys(da, regid, pid);

            string hierarchySql = @"
select
    p.id ProjectId,p.ProjectName,
    m.id MilestoneId,m.Milestone,isnull(m.orderno,0) MilestoneOrder,isnull(m.TaskDuration,'') MilestoneDuration,
    t.id TaskId,t.taskname,isnull(t.orderno,0) TaskOrder,isnull(t.TaskDuration,'') ParentTaskDuration,
    s.id SubtaskId,s.Subtaskname,isnull(s.orderno,0) SubtaskOrder,
    isnull(s.Owner,'') SubtaskOwner,isnull(s.team,'') SubtaskTeam,
    s.StartDate,s.TargetDate,s.TaskDuration,
    isnull(s.iscomplete,0) iscomplete
from Pms_tbl_AddProject p with(nolock)
inner join Pms_tbl_AddMilestone m with(nolock)
    on m.ProjectId=p.id and m.regid=p.regid and m.status=1 and m.archive=0
inner join Pms_tbl_Addtask t with(nolock)
    on t.ProjectId=p.id and t.Milestoneid=m.id and t.regid=p.regid and t.status=1 and t.archive=0
inner join Pms_tbl_Addsubtask s with(nolock)
    on s.ProjectId=p.id and s.Milestoneid=m.id and s.Task=t.id and s.regid=p.regid and s.status=1 and s.archive=0
where p.regid=" + regid + @"
  and p.id=" + pid + @"
  and p.status=1 and p.archive=0
order by isnull(m.orderno,0),m.id,isnull(t.orderno,0),t.id,isnull(s.orderno,0),s.id";

            DataSet hds = da.GetDataSetInline(hierarchySql);
            if (hds.Tables.Count == 0 || hds.Tables[0].Rows.Count == 0) continue;

            MyTeamProjectTree projectNode = new MyTeamProjectTree
            {
                Id = pid,
                Name = !String.IsNullOrWhiteSpace(loginProject.ProjectName)
                    ? loginProject.ProjectName
                    : GetColumn(hds.Tables[0].Rows[0], "ProjectName"),
                Expanded = true
            };

            Dictionary<int, MyTeamMilestoneTree> milestoneNodes = new Dictionary<int, MyTeamMilestoneTree>();
            Dictionary<int, MyTeamTaskTree> taskNodes = new Dictionary<int, MyTeamTaskTree>();

            var taskGroups = hds.Tables[0].AsEnumerable().GroupBy(r => Convert.ToInt32(r["TaskId"]));
            foreach (var taskGroup in taskGroups)
            {
                DataRow firstTaskRow = taskGroup.First();
                int milestoneId = Convert.ToInt32(firstTaskRow["MilestoneId"]);
                int taskId = Convert.ToInt32(firstTaskRow["TaskId"]);

                // Same process as Assign Project: ask the existing SP at task level.
                // If an older SP/view returns nothing, fall back to exact subtask-level calls.
                List<unassignsubtasklist> taskRows = LoadMyTeamUnassignedTaskList(
                    da, regid, pid, milestoneId, taskId, "task", active);

                HashSet<int> availableSubtaskIds = new HashSet<int>(
                    taskRows
                        .Where(x => !x.isassigned && NormalizeMyTeamType(x.tasktype) == "subtask" && x.id.HasValue)
                        .Select(x => x.id.Value));

                if (availableSubtaskIds.Count == 0)
                {
                    foreach (DataRow subRow in taskGroup)
                    {
                        int sid = Convert.ToInt32(subRow["SubtaskId"]);
                        List<unassignsubtasklist> exactRows = LoadMyTeamUnassignedTaskList(
                            da, regid, pid, milestoneId, sid, "subtask", active);
                        if (exactRows.Any(x => x.id.HasValue && x.id.Value == sid && !x.isassigned))
                            availableSubtaskIds.Add(sid);
                    }
                }

                foreach (DataRow subRow in taskGroup)
                {
                    int sid = Convert.ToInt32(subRow["SubtaskId"]);
                    if (!availableSubtaskIds.Contains(sid)) continue;
                    if (active.Contains(AssignmentKey("subtask", sid))) continue;
                    if (!SubtaskAvailableOnSelectedDate(subRow, selectedDate)) continue;

                    MyTeamMilestoneTree milestoneNode;
                    if (!milestoneNodes.TryGetValue(milestoneId, out milestoneNode))
                    {
                        milestoneNode = new MyTeamMilestoneTree
                        {
                            Id = milestoneId,
                            Name = GetColumn(subRow, "Milestone"),
                            OrderNo = GetColumn(subRow, "MilestoneOrder"),
                            Duration = GetColumn(subRow, "MilestoneDuration"),
                            Expanded = true
                        };
                        milestoneNodes[milestoneId] = milestoneNode;
                        projectNode.Milestones.Add(milestoneNode);
                    }

                    MyTeamTaskTree taskNode;
                    if (!taskNodes.TryGetValue(taskId, out taskNode))
                    {
                        taskNode = new MyTeamTaskTree
                        {
                            Id = taskId,
                            Name = GetColumn(subRow, "taskname"),
                            OrderNo = GetColumn(subRow, "TaskOrder"),
                            Duration = GetColumn(subRow, "ParentTaskDuration"),
                            Expanded = true
                        };
                        taskNodes[taskId] = taskNode;
                        milestoneNode.Tasks.Add(taskNode);
                    }

                    int durationMinutes = MyTeamDurationMinutes(GetColumn(subRow, "TaskDuration"), 0);
                    MyTeamAvailableItem item = new MyTeamAvailableItem
                    {
                        Key = "subtask-" + sid.ToString(CultureInfo.InvariantCulture),
                        Type = "subtask",
                        EntityId = sid,
                        ProjectId = pid,
                        ProjectName = projectNode.Name,
                        MilestoneId = milestoneId,
                        MilestoneName = milestoneNode.Name,
                        TaskId = taskId,
                        TaskName = taskNode.Name,
                        SubTaskId = sid,
                        Name = GetColumn(subRow, "Subtaskname"),
                        OrderNo = GetColumn(subRow, "SubtaskOrder"),
                        DurationMinutes = durationMinutes,
                        Duration = MyTeamDurationText(durationMinutes),
                        StartDate = MyTeamNullableDate(subRow["StartDate"]).HasValue
                            ? MyTeamNullableDate(subRow["StartDate"]).Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                            : String.Empty,
                        TargetDate = MyTeamNullableDate(subRow["TargetDate"]).HasValue
                            ? MyTeamNullableDate(subRow["TargetDate"]).Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                            : String.Empty,
                        Owner = GetColumn(subRow, "SubtaskOwner"),
                        Team = GetColumn(subRow, "SubtaskTeam")
                    };

                    taskNode.Subtasks.Add(item);
                    result.Subtasks.Add(item);
                }
            }

            // Keep only hierarchy branches that contain at least one available subtask.
            foreach (MyTeamMilestoneTree m in projectNode.Milestones)
                m.Tasks = m.Tasks.Where(t => t.Subtasks.Count > 0).ToList();
            projectNode.Milestones = projectNode.Milestones.Where(m => m.Tasks.Count > 0).ToList();

            if (projectNode.Milestones.Count > 0)
                result.Projects.Add(projectNode);
        }

        result.Projects = result.Projects.OrderBy(x => x.Name).ToList();
        result.Subtasks = result.Subtasks
            .OrderBy(x => x.ProjectName)
            .ThenBy(x => x.MilestoneId)
            .ThenBy(x => x.TaskId)
            .ThenBy(x => x.OrderNo)
            .ThenBy(x => x.EntityId)
            .ToList();

        return result;
    }

    private static bool NodeDateAllowed(MyTeamHierarchyNode node, DateTime selected)
    {
        if (node == null || node.IsComplete) return false;
        // My Team means "available today". Future items should not be offered early.
        // Overdue unfinished items remain assignable, so TargetDate is not used as a hard stop.
        if (node.StartDate.HasValue && selected.Date < node.StartDate.Value.Date) return false;
        return true;
    }

    private static bool NodeEmployeeAllowed(MyTeamHierarchyNode node, int employeeId,
        Dictionary<int, MyTeamHierarchyNode> milestoneMap,
        Dictionary<int, MyTeamHierarchyNode> taskMap,
        MyTeamHierarchyNode projectNode)
    {
        if (node == null) return false;
        if (CsvHasEmployee(node.Owner, employeeId) || CsvHasEmployee(node.Team, employeeId)) return true;

        // Follow parent only when this level has no explicit Owner/Team at all.
        if (!String.IsNullOrWhiteSpace(node.Owner) || !String.IsNullOrWhiteSpace(node.Team)) return false;

        if (node.Type == "subtask" && taskMap.ContainsKey(node.TaskId))
            return NodeEmployeeAllowed(taskMap[node.TaskId], employeeId, milestoneMap, taskMap, projectNode);
        if (node.Type == "task" && milestoneMap.ContainsKey(node.MilestoneId))
            return NodeEmployeeAllowed(milestoneMap[node.MilestoneId], employeeId, milestoneMap, taskMap, projectNode);
        if (node.Type == "milestone" && projectNode != null)
            return CsvHasEmployee(projectNode.Owner, employeeId) || CsvHasEmployee(projectNode.Team, employeeId);
        return false;
    }

    private static MyTeamAvailableItem ToAvailableItem(MyTeamHierarchyNode n)
    {
        return new MyTeamAvailableItem
        {
            Key = n.Type + "-" + n.Id.ToString(CultureInfo.InvariantCulture),
            Type = n.Type,
            EntityId = n.Id,
            ProjectId = n.ProjectId,
            MilestoneId = n.MilestoneId,
            TaskId = n.TaskId,
            SubTaskId = n.Type == "subtask" ? n.Id : 0,
            Name = n.Name,
            OrderNo = n.OrderNo,
            DurationMinutes = MyTeamDurationMinutes(n.Duration, 60),
            Duration = MyTeamDurationText(MyTeamDurationMinutes(n.Duration, 60)),
            TargetDate = n.TargetDate.HasValue ? n.TargetDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : ""
        };
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static MyTeamAssignableResult GetMyTeamAssignableItems(string employeeId, string workDate, string projectId)
    {
        MyTeamAssignableResult result = new MyTeamAssignableResult();
        int empid, pid;
        int regid = CurrentRegId();
        if (regid <= 0 || !Int32.TryParse(employeeId, out empid) || empid <= 0 ||
            !Int32.TryParse(projectId, out pid) || pid <= 0)
            return result;

        DateTime selected = ParseMyTeamDate(workDate);
        DataAccess da = new DataAccess();
        if (!CanCurrentUserManageEmployee(da, regid, empid)) return result;

        DataSet pds = da.GetDataSetInline(@"
select top(1) id,ProjectName,isnull(Owner,'') Owner,isnull(team,'') team
from Pms_tbl_AddProject with(nolock)
where regid=" + regid + " and id=" + pid + @" and status=1 and archive=0");
        if (pds.Tables.Count == 0 || pds.Tables[0].Rows.Count == 0) return result;

        MyTeamHierarchyNode projectNode = new MyTeamHierarchyNode
        {
            Type = "project",
            Id = pid,
            ProjectId = pid,
            Owner = GetColumn(pds.Tables[0].Rows[0], "Owner"),
            Team = GetColumn(pds.Tables[0].Rows[0], "team")
        };

        List<MyTeamHierarchyNode> milestones = new List<MyTeamHierarchyNode>();
        List<MyTeamHierarchyNode> tasks = new List<MyTeamHierarchyNode>();
        List<MyTeamHierarchyNode> subtasks = new List<MyTeamHierarchyNode>();

        DataSet mds = da.GetDataSetInline(@"
select id,ProjectId,Milestone,isnull(Owner,'') Owner,isnull(team,'') team,
 isnull(orderno,0) orderno,StartDate,EndDate,isnull(TaskDuration,'01:00') TaskDuration,isnull(iscomplete,0) iscomplete
from Pms_tbl_AddMilestone with(nolock)
where regid=" + regid + " and ProjectId=" + pid + @" and status=1 and archive=0
order by isnull(orderno,0),id");
        if (mds.Tables.Count > 0)
        {
            foreach (DataRow row in mds.Tables[0].Rows)
            {
                MyTeamHierarchyNode n = new MyTeamHierarchyNode
                {
                    Type = "milestone", Id = Convert.ToInt32(row["id"]), ProjectId = pid,
                    MilestoneId = Convert.ToInt32(row["id"]), TaskId = 0,
                    Name = GetColumn(row, "Milestone"), OrderNo = GetColumn(row, "orderno"),
                    Owner = GetColumn(row, "Owner"), Team = GetColumn(row, "team"),
                    StartDate = MyTeamNullableDate(row["StartDate"]), TargetDate = MyTeamNullableDate(row["EndDate"]),
                    Duration = GetColumn(row, "TaskDuration"), IsComplete = Convert.ToInt32(row["iscomplete"]) == 1
                };
                milestones.Add(n);
                result.Milestones.Add(new MyTeamMilestoneFilter { Id = n.Id, Name = n.Name, OrderNo = n.OrderNo });
            }
        }

        DataSet tds = da.GetDataSetInline(@"
select id,ProjectId,Milestoneid,taskname,isnull(Owner,'') Owner,isnull(team,'') team,
 isnull(orderno,0) orderno,StartDate,TargetDate,isnull(TaskDuration,'01:00') TaskDuration,isnull(iscomplete,0) iscomplete
from Pms_tbl_Addtask with(nolock)
where regid=" + regid + " and ProjectId=" + pid + @" and status=1 and archive=0
order by isnull(orderno,0),id");
        if (tds.Tables.Count > 0)
        {
            foreach (DataRow row in tds.Tables[0].Rows)
            {
                tasks.Add(new MyTeamHierarchyNode
                {
                    Type = "task", Id = Convert.ToInt32(row["id"]), ProjectId = pid,
                    MilestoneId = Convert.ToInt32(row["Milestoneid"]), TaskId = Convert.ToInt32(row["id"]),
                    Name = GetColumn(row, "taskname"), OrderNo = GetColumn(row, "orderno"),
                    Owner = GetColumn(row, "Owner"), Team = GetColumn(row, "team"),
                    StartDate = MyTeamNullableDate(row["StartDate"]), TargetDate = MyTeamNullableDate(row["TargetDate"]),
                    Duration = GetColumn(row, "TaskDuration"), IsComplete = Convert.ToInt32(row["iscomplete"]) == 1
                });
            }
        }

        DataSet sds = da.GetDataSetInline(@"
select id,ProjectId,Milestoneid,Task,Subtaskname,isnull(Owner,'') Owner,isnull(team,'') team,
 isnull(orderno,0) orderno,StartDate,TargetDate,isnull(TaskDuration,'01:00') TaskDuration,isnull(iscomplete,0) iscomplete
from Pms_tbl_Addsubtask with(nolock)
where regid=" + regid + " and ProjectId=" + pid + @" and status=1 and archive=0
order by isnull(orderno,0),id");
        if (sds.Tables.Count > 0)
        {
            foreach (DataRow row in sds.Tables[0].Rows)
            {
                subtasks.Add(new MyTeamHierarchyNode
                {
                    Type = "subtask", Id = Convert.ToInt32(row["id"]), ProjectId = pid,
                    MilestoneId = Convert.ToInt32(row["Milestoneid"]), TaskId = Convert.ToInt32(row["Task"]),
                    Name = GetColumn(row, "Subtaskname"), OrderNo = GetColumn(row, "orderno"),
                    Owner = GetColumn(row, "Owner"), Team = GetColumn(row, "team"),
                    StartDate = MyTeamNullableDate(row["StartDate"]), TargetDate = MyTeamNullableDate(row["TargetDate"]),
                    Duration = GetColumn(row, "TaskDuration"), IsComplete = Convert.ToInt32(row["iscomplete"]) == 1
                });
            }
        }

        Dictionary<int, MyTeamHierarchyNode> milestoneMap = milestones.ToDictionary(x => x.Id, x => x);
        Dictionary<int, MyTeamHierarchyNode> taskMap = tasks.ToDictionary(x => x.Id, x => x);
        HashSet<string> active = ActiveAssignmentKeys(da, regid, pid);
        HashSet<string> assignProjectAvailable = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Same hierarchy request pattern as Assign Project:
        // milestone without tasks => milestone call; otherwise one task call.
        foreach (MyTeamHierarchyNode milestone in milestones)
        {
            List<MyTeamHierarchyNode> childTasks = tasks.Where(x => x.MilestoneId == milestone.Id).ToList();
            if (childTasks.Count == 0)
            {
                assignProjectAvailable.UnionWith(LoadAssignProjectAvailableKeys(da, regid, pid, milestone.Id, milestone.Id, "milestone"));
                continue;
            }

            foreach (MyTeamHierarchyNode task in childTasks)
            {
                HashSet<string> taskKeys = LoadAssignProjectAvailableKeys(da, regid, pid, milestone.Id, task.Id, "task");
                assignProjectAvailable.UnionWith(taskKeys);

                // Some older SP versions return nothing at task level when the task has subtasks.
                // Fallback to the exact subtask call used by Assign Project when S3 is clicked.
                List<MyTeamHierarchyNode> childSubs = subtasks.Where(x => x.TaskId == task.Id).ToList();
                if (childSubs.Count > 0 && !childSubs.Any(x => taskKeys.Contains(AssignmentKey("subtask", x.Id))))
                {
                    foreach (MyTeamHierarchyNode sub in childSubs)
                        assignProjectAvailable.UnionWith(LoadAssignProjectAvailableKeys(da, regid, pid, milestone.Id, sub.Id, "subtask"));
                }
            }
        }

        List<MyTeamHierarchyNode> candidates = new List<MyTeamHierarchyNode>();
        foreach (MyTeamHierarchyNode milestone in milestones)
        {
            List<MyTeamHierarchyNode> childTasks = tasks.Where(x => x.MilestoneId == milestone.Id).ToList();
            if (childTasks.Count == 0)
            {
                candidates.Add(milestone);
                continue;
            }
            foreach (MyTeamHierarchyNode task in childTasks)
            {
                List<MyTeamHierarchyNode> childSubs = subtasks.Where(x => x.TaskId == task.Id).ToList();
                if (childSubs.Count > 0) candidates.AddRange(childSubs);
                else candidates.Add(task);
            }
        }

        foreach (MyTeamHierarchyNode node in candidates)
        {
            string key = AssignmentKey(node.Type, node.Id);
            if (!assignProjectAvailable.Contains(key)) continue;
            if (active.Contains(key)) continue;
            if (!NodeDateAllowed(node, selected)) continue;
            if (!NodeEmployeeAllowed(node, empid, milestoneMap, taskMap, projectNode)) continue;
            result.Tasks.Add(ToAvailableItem(node));
        }

        result.Tasks = result.Tasks
            .OrderBy(x => x.MilestoneId)
            .ThenBy(x => x.TaskId)
            .ThenBy(x => x.OrderNo)
            .ThenBy(x => x.EntityId)
            .ToList();

        return result;
    }

    private static bool IsEmployeeEligibleForEntity(DataAccess da, int regid, int employeeId, string type, int entityId)
    {
        type = NormalizeMyTeamType(type);
        string table = type == "milestone" ? "Pms_tbl_AddMilestone" : (type == "task" ? "Pms_tbl_Addtask" : "Pms_tbl_Addsubtask");
        DataSet ds = da.GetDataSetInline("select top(1) isnull(Owner,'') Owner,isnull(team,'') team from " + table +
            " with(nolock) where regid=" + regid + " and status=1 and archive=0 and id=" + entityId);
        if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return false;
        string owner = GetColumn(ds.Tables[0].Rows[0], "Owner");
        string team = GetColumn(ds.Tables[0].Rows[0], "team");
        return CsvHasEmployee(owner, employeeId) || CsvHasEmployee(team, employeeId) ||
               (String.IsNullOrWhiteSpace(owner) && String.IsNullOrWhiteSpace(team));
    }

    private static DateTime CalculateMyTeamAssignmentEnd(int employeeId, DateTime start, int durationMinutes)
    {
        string shift = GetMyTeamEmployeeShift(employeeId);
        string[] p = shift.Split('-');
        TimeSpan shiftStart = ParseMyTeamTime(p.Length > 0 ? p[0] : "09:00 AM", "09:00 AM");
        TimeSpan shiftEnd = ParseMyTeamTime(p.Length > 2 ? p[2] : "06:00 PM", "06:00 PM");
        DateTime cursor = start;
        int remaining = durationMinutes <= 0 ? 60 : durationMinutes;
        int safety = 0;

        while (remaining > 0 && safety < 120)
        {
            safety++;
            DateTime dayStart = cursor.Date.Add(shiftStart);
            DateTime dayEnd = cursor.Date.Add(shiftEnd);
            if (dayEnd <= dayStart) dayEnd = dayEnd.AddDays(1);
            if (cursor < dayStart) cursor = dayStart;
            if (cursor >= dayEnd)
            {
                cursor = cursor.Date.AddDays(1).Add(shiftStart);
                continue;
            }
            int take = Math.Min(remaining, (int)(dayEnd - cursor).TotalMinutes);
            cursor = cursor.AddMinutes(take);
            remaining -= take;
            if (remaining > 0) cursor = cursor.Date.AddDays(1).Add(shiftStart);
        }
        return cursor;
    }

    private static bool MyTeamHasOverlap(DataAccess da, int regid, int employeeId, DateTime start, int durationMinutes, int excludeAssignId)
    {
        string shift = GetMyTeamEmployeeShift(employeeId);
        string[] p = shift.Split('-');
        TimeSpan shiftStart = ParseMyTeamTime(p.Length > 0 ? p[0] : "09:00 AM", "09:00 AM");
        TimeSpan shiftEnd = ParseMyTeamTime(p.Length > 2 ? p[2] : "06:00 PM", "06:00 PM");
        DateTime cursor = start;
        int remaining = durationMinutes <= 0 ? 60 : durationMinutes;
        int safety = 0;

        while (remaining > 0 && safety < 120)
        {
            safety++;
            DateTime dayStart = cursor.Date.Add(shiftStart);
            DateTime dayEnd = cursor.Date.Add(shiftEnd);
            if (dayEnd <= dayStart) dayEnd = dayEnd.AddDays(1);
            if (cursor < dayStart) cursor = dayStart;
            if (cursor >= dayEnd)
            {
                cursor = cursor.Date.AddDays(1).Add(shiftStart);
                continue;
            }

            int take = Math.Min(remaining, (int)(dayEnd - cursor).TotalMinutes);
            DateTime segEnd = cursor.AddMinutes(take);
            string sql = @"
select top(1) id
from Pms_tbl_assigntasks with(nolock)
where regid=" + regid + " and status=1 and archive=0 and ownerid=" + employeeId + @"
 and startdatetime < '" + ToMyTeamSqlDateTime(segEnd) + @"'
 and isnull(enddatetime,startdatetime) > '" + ToMyTeamSqlDateTime(cursor) + @"'";
            if (excludeAssignId > 0) sql += " and id<>" + excludeAssignId;
            DataSet ds = da.GetDataSetInline(sql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) return true;

            cursor = segEnd;
            remaining -= take;
            if (remaining > 0) cursor = cursor.Date.AddDays(1).Add(shiftStart);
        }
        return false;
    }

    private static DataRow GetEntityRow(DataAccess da, int regid, string type, int entityId)
    {
        type = NormalizeMyTeamType(type);
        string sql = "";
        if (type == "milestone")
            sql = "select top(1) ProjectId,id MilestoneId,0 TaskId,0 SubtaskId,TaskDuration,StartDate,EndDate TargetDate,isnull(iscomplete,0) iscomplete,Milestone EntityName,isnull(team,'') team from Pms_tbl_AddMilestone with(nolock) where regid=" + regid + " and id=" + entityId + " and status=1 and archive=0";
        else if (type == "task")
            sql = "select top(1) ProjectId,Milestoneid MilestoneId,id TaskId,0 SubtaskId,TaskDuration,StartDate,TargetDate,isnull(iscomplete,0) iscomplete,taskname EntityName,isnull(team,'') team from Pms_tbl_Addtask with(nolock) where regid=" + regid + " and id=" + entityId + " and status=1 and archive=0";
        else if (type == "subtask")
            sql = @"select top(1)
                        s.ProjectId,s.Milestoneid MilestoneId,s.Task TaskId,s.id SubtaskId,
                        s.TaskDuration,s.StartDate,s.TargetDate,isnull(s.iscomplete,0) iscomplete,
                        s.Subtaskname EntityName,isnull(s.team,'') team
                    from Pms_tbl_Addsubtask s with(nolock)
                    inner join Pms_tbl_Addtask t with(nolock)
                        on t.id=s.Task and t.ProjectId=s.ProjectId and t.Milestoneid=s.Milestoneid
                       and t.regid=s.regid and t.status=1 and t.archive=0
                    inner join Pms_tbl_AddMilestone m with(nolock)
                        on m.id=s.Milestoneid and m.ProjectId=s.ProjectId
                       and m.regid=s.regid and m.status=1 and m.archive=0
                    inner join Pms_tbl_AddProject p with(nolock)
                        on p.id=s.ProjectId and p.regid=s.regid and p.status=1 and p.archive=0
                    where s.regid=" + regid + " and s.id=" + entityId + " and s.status=1 and s.archive=0";
        if (String.IsNullOrEmpty(sql)) return null;
        DataSet ds = da.GetDataSetInline(sql);
        return ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 ? ds.Tables[0].Rows[0] : null;
    }

    private static bool MyTeamProjectIsAvailableToLoginUser(int projectId)
    {
        if (projectId <= 0) return false;
        try
        {
            return GetMyTeamProjects().Any(x =>
                String.Equals(x.id, projectId.ToString(CultureInfo.InvariantCulture), StringComparison.OrdinalIgnoreCase));
        }
        catch
        {
            return false;
        }
    }

    private static bool MyTeamDropStartsInsideEmployeeShift(int employeeId, DateTime start)
    {
        string shift = GetMyTeamEmployeeShift(employeeId);
        string[] parts = shift.Split('-');
        TimeSpan shiftStart = ParseMyTeamTime(parts.Length > 0 ? parts[0] : "09:00 AM", "09:00 AM");
        TimeSpan shiftEnd = ParseMyTeamTime(parts.Length > 2 ? parts[2] : "06:00 PM", "06:00 PM");
        TimeSpan startTime = start.TimeOfDay;

        if (shiftEnd > shiftStart)
            return startTime >= shiftStart && startTime < shiftEnd;

        // Night shift, for example 09:00 PM - 06:00 AM.
        return startTime >= shiftStart || startTime < shiftEnd;
    }

    private static string FormatMyTeamValidationDate(DateTime value)
    {
        return value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
    }

    private static string ValidateMyTeamSubtaskResizeDuration(DataAccess da, int regid, int subtaskId, int newDurationMinutes)
    {
        if (subtaskId <= 0 || newDurationMinutes <= 0) return String.Empty;

        DataSet subDs = da.GetDataSetInline(@"
select top(1) s.Task,s.ProjectId,s.Milestoneid
from Pms_tbl_Addsubtask s with(nolock)
where s.regid=" + regid + " and s.id=" + subtaskId + @" and s.status=1 and s.archive=0");
        if (subDs.Tables.Count == 0 || subDs.Tables[0].Rows.Count == 0) return String.Empty;

        int taskId = MyTeamNullableInt(subDs.Tables[0].Rows[0]["Task"]) ?? 0;
        if (taskId <= 0) return String.Empty;

        DataSet taskDs = da.GetDataSetInline(@"
select top(1) TaskDuration
from Pms_tbl_Addtask with(nolock)
where regid=" + regid + " and id=" + taskId + @" and status=1 and archive=0");
        if (taskDs.Tables.Count == 0 || taskDs.Tables[0].Rows.Count == 0) return String.Empty;

        int taskMinutes = MyTeamDurationMinutes(GetColumn(taskDs.Tables[0].Rows[0], "TaskDuration"), 0);
        if (taskMinutes <= 0) return String.Empty;

        DataSet siblingDs = da.GetDataSetInline(@"
select id,TaskDuration
from Pms_tbl_Addsubtask with(nolock)
where regid=" + regid + " and Task=" + taskId + @" and status=1 and archive=0");

        int totalSubtaskMinutes = 0;
        if (siblingDs.Tables.Count > 0)
        {
            foreach (DataRow r in siblingDs.Tables[0].Rows)
            {
                int sid = MyTeamNullableInt(r["id"]) ?? 0;
                totalSubtaskMinutes += sid == subtaskId
                    ? newDurationMinutes
                    : MyTeamDurationMinutes(GetColumn(r, "TaskDuration"), 0);
            }
        }

        return totalSubtaskMinutes > taskMinutes
            ? "Total subtask duration cannot be greater than task duration."
            : String.Empty;
    }

    private static void InsertMyTeamAssignment(DataAccess da, int regid, int uid, int employeeId,
        int projectId, int milestoneId, int taskId, int subtaskId, string type,
        DateTime start, DateTime end, string duration, DateTime? assignedDate)
    {
        string sql = @"
insert into Pms_tbl_assigntasks
(uid,regid,cdate,status,archive,projectid,milestoneid,taskid,tasktype,subtaskid,ownerid,startdatetime,enddatetime,taskduration,assigndatetime)
values(" + uid + "," + regid + @",getdate(),1,0," + projectId + "," + milestoneId + "," + taskId + ",'" +
            type.Replace("'", "''") + "'," + subtaskId + "," + employeeId + ",'" + ToMyTeamSqlDateTime(start) + "','" +
            ToMyTeamSqlDateTime(end) + "','" + duration.Replace("'", "''") + "','" +
            ToMyTeamSqlDateTime(assignedDate ?? DateTime.Now) + "')";
        da.executeDMLQueryinline(sql);
    }

    private static void GetMyTeamShiftWindow(int employeeId, DateTime cursor, out DateTime shiftStartDateTime, out DateTime shiftEndDateTime)
    {
        string shift = GetMyTeamEmployeeShift(employeeId);
        string[] parts = shift.Split('-');
        TimeSpan shiftStart = ParseMyTeamTime(parts.Length > 0 ? parts[0] : "09:00 AM", "09:00 AM");
        TimeSpan shiftEnd = ParseMyTeamTime(parts.Length > 2 ? parts[2] : "06:00 PM", "06:00 PM");

        if (shiftEnd > shiftStart)
        {
            shiftStartDateTime = cursor.Date.Add(shiftStart);
            shiftEndDateTime = cursor.Date.Add(shiftEnd);
            return;
        }

        // Night shift, for example 09:00 PM - 06:00 AM.
        // A 02:00 AM cursor belongs to the shift that started on the previous date.
        if (cursor.TimeOfDay < shiftEnd)
        {
            shiftStartDateTime = cursor.Date.AddDays(-1).Add(shiftStart);
            shiftEndDateTime = cursor.Date.Add(shiftEnd);
        }
        else
        {
            shiftStartDateTime = cursor.Date.Add(shiftStart);
            shiftEndDateTime = cursor.Date.AddDays(1).Add(shiftEnd);
        }
    }

    private static DateTime NormalizeMyTeamToShiftCursor(int employeeId, DateTime cursor)
    {
        DateTime shiftStart;
        DateTime shiftEnd;
        GetMyTeamShiftWindow(employeeId, cursor, out shiftStart, out shiftEnd);

        if (cursor < shiftStart) return shiftStart;
        if (cursor < shiftEnd) return cursor;

        // Ask for the next calendar instant and normalize again. This also works for night shift.
        DateTime probe = cursor.Date.AddDays(1);
        GetMyTeamShiftWindow(employeeId, probe, out shiftStart, out shiftEnd);
        if (probe < shiftStart) return shiftStart;
        return probe;
    }

    private static bool TryBuildMyTeamNonOverlapSchedule(
        DataAccess da,
        int regid,
        int employeeId,
        DateTime requestedStart,
        int durationMinutes,
        out DateTime scheduleEnd,
        out DateTime conflictEnd)
    {
        scheduleEnd = requestedStart;
        conflictEnd = DateTime.MinValue;
        DateTime cursor = NormalizeMyTeamToShiftCursor(employeeId, requestedStart);
        int remaining = durationMinutes;
        int safety = 0;

        while (remaining > 0 && safety < 180)
        {
            safety++;

            DateTime shiftStart;
            DateTime shiftEnd;
            GetMyTeamShiftWindow(employeeId, cursor, out shiftStart, out shiftEnd);

            if (cursor < shiftStart) cursor = shiftStart;
            if (cursor >= shiftEnd)
            {
                cursor = NormalizeMyTeamToShiftCursor(employeeId, cursor.AddMinutes(1));
                continue;
            }

            int available = (int)(shiftEnd - cursor).TotalMinutes;
            int take = Math.Min(remaining, available);
            if (take <= 0)
            {
                cursor = NormalizeMyTeamToShiftCursor(employeeId, shiftEnd.AddMinutes(1));
                continue;
            }

            DateTime segStart = cursor;
            DateTime segEnd = cursor.AddMinutes(take);
            string sql = @"
select top(1) id,startdatetime,isnull(enddatetime,startdatetime) enddatetime
from Pms_tbl_assigntasks with(nolock)
where regid=" + regid + @" and status=1 and archive=0
  and ownerid=" + employeeId + @"
  and startdatetime < '" + ToMyTeamSqlDateTime(segEnd) + @"'
  and isnull(enddatetime,startdatetime) > '" + ToMyTeamSqlDateTime(segStart) + @"'
order by startdatetime";

            DataSet ds = da.GetDataSetInline(sql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DateTime busyEnd = Convert.ToDateTime(ds.Tables[0].Rows[0]["enddatetime"], CultureInfo.InvariantCulture);
                conflictEnd = busyEnd > segStart ? busyEnd : segStart.AddMinutes(15);
                return false;
            }

            cursor = segEnd;
            remaining -= take;
            if (remaining > 0)
                cursor = NormalizeMyTeamToShiftCursor(employeeId, shiftEnd.AddMinutes(1));
        }

        scheduleEnd = cursor;
        return remaining <= 0;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string MyTeamFindNextAvailableSubtaskStart(string id, string empid, string changedate, string starttime)
    {
        int subtaskId;
        int employeeId;
        int regid = CurrentRegId();
        int uid = CurrentUserId();

        if (regid <= 0 || uid <= 0 ||
            !Int32.TryParse(id, out subtaskId) || subtaskId <= 0 ||
            !Int32.TryParse(empid, out employeeId) || employeeId <= 0)
            return "Invalid assignment request.";

        DataAccess da = new DataAccess();
        if (!CanCurrentUserManageEmployee(da, regid, employeeId))
            return "Employee is not in your My Team list.";

        DataRow row = GetEntityRow(da, regid, "subtask", subtaskId);
        if (row == null)
            return "This subtask is no longer active or its hierarchy is not available.";

        string entityName = GetColumn(row, "EntityName");
        if (String.IsNullOrWhiteSpace(entityName)) entityName = "Selected subtask";

        int projectId = Convert.ToInt32(row["ProjectId"], CultureInfo.InvariantCulture);
        int milestoneId = Convert.ToInt32(row["MilestoneId"], CultureInfo.InvariantCulture);

        if (!MyTeamProjectIsAvailableToLoginUser(projectId))
            return "This project is not available for the logged-in user.";

        int isComplete = 0;
        Int32.TryParse(GetColumn(row, "iscomplete"), out isComplete);
        if (isComplete == 1)
            return entityName + " is completed and cannot be assigned.";

        if (!IsEmployeeInSubtaskTeam(da, regid, employeeId, subtaskId))
            return "Selected employee is not in the team of this subtask.";

        HashSet<string> active = ActiveAssignmentKeys(da, regid, projectId);
        List<unassignsubtasklist> availableRows = LoadMyTeamUnassignedTaskList(
            da, regid, projectId, milestoneId, subtaskId, "subtask", active);
        if (!availableRows.Any(x => x.id.HasValue && x.id.Value == subtaskId && !x.isassigned))
            return "This subtask is no longer available for assignment.";

        string duration = GetColumn(row, "TaskDuration");
        int durationMinutes = MyTeamDurationMinutes(duration, 0);
        if (durationMinutes <= 0)
            return entityName + " duration is required before assignment. Please enter duration first.";

        DateTime cursor = ParseMyTeamDate(changedate).Add(ParseMyTeamTime(starttime, "09:00 AM"));
        cursor = NormalizeMyTeamToShiftCursor(employeeId, cursor);

        DateTime? startDate = MyTeamNullableDate(row["StartDate"]);
        DateTime? targetDate = MyTeamNullableDate(row["TargetDate"]);
        int safety = 0;

        while (safety < 500)
        {
            safety++;

            if (startDate.HasValue && cursor.Date < startDate.Value.Date)
                cursor = NormalizeMyTeamToShiftCursor(employeeId, startDate.Value.Date);

            if (targetDate.HasValue && cursor.Date > targetDate.Value.Date)
                return entityName + " cannot be assigned on " + FormatMyTeamValidationDate(cursor) +
                       ". Selected date should not be greater than Targetdate (" + FormatMyTeamValidationDate(targetDate.Value) + ").";

            DateTime end;
            DateTime conflictEnd;
            if (TryBuildMyTeamNonOverlapSchedule(da, regid, employeeId, cursor, durationMinutes, out end, out conflictEnd))
            {
                return "1~~" +
                       cursor.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "~~" +
                       cursor.ToString("hh:mm tt", CultureInfo.InvariantCulture) + "~~" +
                       end.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "~~" +
                       end.ToString("hh:mm tt", CultureInfo.InvariantCulture);
            }

            cursor = conflictEnd == DateTime.MinValue
                ? cursor.AddMinutes(15)
                : conflictEnd;
            cursor = NormalizeMyTeamToShiftCursor(employeeId, cursor);
        }

        return "Drop only in available area";
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string MyTeamAssignItem(string id, string empid, string utype, string changedate, string starttime)
    {
        int entityId, employeeId;
        int regid = CurrentRegId();
        int uid = CurrentUserId();
        string type = NormalizeMyTeamType(utype);
        if (regid <= 0 || uid <= 0 || !Int32.TryParse(id, out entityId) || !Int32.TryParse(empid, out employeeId) || entityId <= 0 || employeeId <= 0)
            return "Invalid assignment request.";

        DataAccess da = new DataAccess();

        // 1. The target employee must be visible/manageable in the logged-in user's My Team scope.
        if (!CanCurrentUserManageEmployee(da, regid, employeeId))
            return "Employee is not in your My Team list.";

        // 2. My Team right panel allows only Subtask drag/drop. Project/Milestone/Task are hierarchy view only.
        if (type != "subtask")
            return "Only subtask can be assigned from My Team.";

        // 3. The complete Project -> Milestone -> Task -> Subtask hierarchy must still be active.
        DataRow row = GetEntityRow(da, regid, type, entityId);
        if (row == null)
            return "This subtask is no longer active or its hierarchy is not available.";

        int project = Convert.ToInt32(row["ProjectId"], CultureInfo.InvariantCulture);
        int milestone = Convert.ToInt32(row["MilestoneId"], CultureInfo.InvariantCulture);
        int task = Convert.ToInt32(row["TaskId"], CultureInfo.InvariantCulture);
        int subtask = Convert.ToInt32(row["SubtaskId"], CultureInfo.InvariantCulture);
        string entityName = GetColumn(row, "EntityName");
        if (String.IsNullOrWhiteSpace(entityName)) entityName = "Selected subtask";

        // 4. The project must be one of the projects available to the logged-in user.
        if (!MyTeamProjectIsAvailableToLoginUser(project))
            return "This project is not available for the logged-in user.";

        // 5. Completed Subtask cannot be assigned.
        int isComplete = 0;
        Int32.TryParse(GetColumn(row, "iscomplete"), out isComplete);
        if (isComplete == 1)
            return entityName + " is completed and cannot be assigned.";

        // 6. Required My Team rule: selected employee must exist in Subtask Team.
        // Owner alone is intentionally not sufficient for this popup.
        if (!IsEmployeeInSubtaskTeam(da, regid, employeeId, entityId))
            return "Selected employee is not in the team of this subtask.";

        // 7. Recheck the same Assign Project unassigned process at save time.
        // This prevents stale right-panel data or double assignment from another session.
        HashSet<string> activeForAvailability = ActiveAssignmentKeys(da, regid, project);
        List<unassignsubtasklist> availableRows = LoadMyTeamUnassignedTaskList(
            da,
            regid,
            project,
            milestone,
            entityId,
            "subtask",
            activeForAvailability);
        if (!availableRows.Any(x => x.id.HasValue && x.id.Value == entityId && !x.isassigned))
            return "This subtask is no longer available for assignment.";

        DateTime workDate = ParseMyTeamDate(changedate);
        DateTime? entityStart = MyTeamNullableDate(row["StartDate"]);
        DateTime? entityTarget = MyTeamNullableDate(row["TargetDate"]);

        // 8. Selected date cannot be before Subtask StartDate.
        if (entityStart.HasValue && workDate.Date < entityStart.Value.Date)
            return entityName + " cannot be assigned on " + FormatMyTeamValidationDate(workDate) +
                   ". Selected date should not be less than StartDate (" + FormatMyTeamValidationDate(entityStart.Value) + ").";

        // 9. Same assignment-date guard used by Assign Project: selected date cannot exceed TargetDate.
        if (entityTarget.HasValue && workDate.Date > entityTarget.Value.Date)
            return entityName + " cannot be assigned on " + FormatMyTeamValidationDate(workDate) +
                   ". Selected date should not be greater than Targetdate (" + FormatMyTeamValidationDate(entityTarget.Value) + ").";

        // 10. Duration is mandatory. Do not silently convert blank/zero duration to one hour.
        string duration = GetColumn(row, "TaskDuration");
        int minutes = MyTeamDurationMinutes(duration, 0);
        if (minutes <= 0)
            return entityName + " duration is required before assignment. Please enter duration first.";

        DateTime start = workDate.Add(ParseMyTeamTime(starttime, "09:00 AM"));

        // 11. Drop START point must be inside the employee shift. A long Subtask may continue to next day.
        if (!MyTeamDropStartsInsideEmployeeShift(employeeId, start))
            return "Drop only in available area";

        // 12. Full duration must remain conflict-free across employee shift segments, including multi-day continuation.
        if (MyTeamHasOverlap(da, regid, employeeId, start, minutes, 0))
            return "Drop only in available area";

        DateTime end = CalculateMyTeamAssignmentEnd(employeeId, start, minutes);

        // Keep the single-assignment-table rule. The availability check above normally means
        // there is no active row, but this cleanup protects against stale duplicate rows.
        da.executeDMLQueryinline(
            "update Pms_tbl_assigntasks set status=0,archive=1,removedatetime=getdate() where regid=" + regid +
            " and status=1 and archive=0 and tasktype='subtask' and subtaskid=" + entityId);

        InsertMyTeamAssignment(da, regid, uid, employeeId, project, milestone, task, subtask, type,
            start, end, MyTeamDurationText(minutes), DateTime.Now);
        return "1";
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string MyTeamRemoveAssignment(string id, string utype)
    {
        int assignId;
        int regid = CurrentRegId();
        if (regid <= 0 || !Int32.TryParse(id, out assignId) || assignId <= 0) return "0";

        DataAccess da = new DataAccess();
        DataSet ds = da.GetDataSetInline(@"
select top(1) ownerid
from Pms_tbl_assigntasks with(nolock)
where regid=" + regid + " and status=1 and archive=0 and id=" + assignId);
        if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return "0";

        int employeeId = MyTeamNullableInt(ds.Tables[0].Rows[0]["ownerid"]) ?? 0;
        if (!CanCurrentUserManageEmployee(da, regid, employeeId))
            return "Employee is not in your My Team list.";

        da.executeDMLQueryinline("update Pms_tbl_assigntasks set status=0,archive=1,removedatetime=getdate() where regid=" + regid +
            " and status=1 and archive=0 and id=" + assignId);
        return "1";
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string MyTeamUpdateAssignment(string id, string utype, string starttime, string taskduration,
        string changedate, string oldstarttime, string oldchangedate, string editmode)
    {
        int assignId;
        int regid = CurrentRegId();
        int uid = CurrentUserId();
        if (regid <= 0 || uid <= 0 || !Int32.TryParse(id, out assignId) || assignId <= 0) return "0";

        DataAccess da = new DataAccess();
        DataSet ds = da.GetDataSetInline(@"
select top(1) * from Pms_tbl_assigntasks with(nolock)
where regid=" + regid + " and status=1 and archive=0 and id=" + assignId);
        if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return "0";
        DataRow ar = ds.Tables[0].Rows[0];

        int employeeId = Convert.ToInt32(ar["ownerid"]);
        if (!CanCurrentUserManageEmployee(da, regid, employeeId))
            return "Employee is not in your My Team list.";

        string realType = NormalizeMyTeamType(GetColumn(ar, "tasktype"));
        int realEntityId = realType == "milestone"
            ? (MyTeamNullableInt(ar["milestoneid"]) ?? 0)
            : (realType == "task"
                ? (MyTeamNullableInt(ar["taskid"]) ?? 0)
                : (MyTeamNullableInt(ar["subtaskid"]) ?? 0));

        DateTime oldStart = Convert.ToDateTime(ar["startdatetime"], CultureInfo.InvariantCulture);
        int oldTotalMinutes = MyTeamDurationMinutes(GetColumn(ar, "taskduration"), 0);
        int requestedMinutes = MyTeamDurationMinutes(taskduration, 0);
        if (requestedMinutes <= 0)
            return "Duration is required before assignment. Please enter duration first.";

        bool resize = !String.IsNullOrWhiteSpace(editmode) && editmode.ToLowerInvariant().Contains("resize");
        DateTime selectedDate = ParseMyTeamDate(changedate);
        DateTime newStart = resize ? oldStart : selectedDate.Add(ParseMyTeamTime(starttime, "09:00 AM"));
        int newTotalMinutes = requestedMinutes;

        DataRow entityRow = realEntityId > 0 ? GetEntityRow(da, regid, realType, realEntityId) : null;

        // Same date/shift rules used by Assign Project when an existing block is moved.
        if (!resize)
        {
            if (entityRow != null)
            {
                DateTime? entityStart = MyTeamNullableDate(entityRow["StartDate"]);
                DateTime? entityTarget = MyTeamNullableDate(entityRow["TargetDate"]);
                string entityName = GetColumn(entityRow, "EntityName");
                if (String.IsNullOrWhiteSpace(entityName)) entityName = "Selected task";

                if (entityStart.HasValue && selectedDate.Date < entityStart.Value.Date)
                    return entityName + " cannot be assigned on " + FormatMyTeamValidationDate(selectedDate) +
                           ". Selected date should not be less than StartDate (" + FormatMyTeamValidationDate(entityStart.Value) + ").";

                if (entityTarget.HasValue && selectedDate.Date > entityTarget.Value.Date)
                    return entityName + " cannot be assigned on " + FormatMyTeamValidationDate(selectedDate) +
                           ". Selected date should not be greater than Targetdate (" + FormatMyTeamValidationDate(entityTarget.Value) + ").";
            }

            if (!MyTeamDropStartsInsideEmployeeShift(employeeId, newStart))
                return "Drop only in available area";
        }

        // Preserve untouched days when a multi-day visible segment is resized/moved.
        List<MyTeamAssignmentRow> allRows = LoadMyTeamAssignmentRows(employeeId, ParseMyTeamDate(oldchangedate));
        string shift = GetMyTeamEmployeeShift(employeeId);
        string[] sp = shift.Split('-');
        List<MyTeamTaskSegment> sameSegments = BuildMyTeamSegments(allRows,
            sp.Length > 0 ? sp[0] : "09:00 AM", sp.Length > 2 ? sp[2] : "06:00 PM")
            .Where(x => x.assignid.HasValue && x.assignid.Value == assignId)
            .OrderBy(x => x.workDateKey).ThenBy(x => x.StartTime).ToList();

        if (sameSegments.Count > 1)
        {
            string oldDateKey = ParseMyTeamDate(oldchangedate).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            MyTeamTaskSegment edited = sameSegments.FirstOrDefault(x => x.workDateKey == oldDateKey &&
                String.Equals((x.StartTime ?? "").Trim(), (oldstarttime ?? "").Trim(), StringComparison.OrdinalIgnoreCase));
            if (edited == null) edited = sameSegments.FirstOrDefault(x => x.workDateKey == oldDateKey);

            if (resize && edited != null)
            {
                // Same Assign Project rule: only the last visible day of a multi-day assignment can be resized.
                MyTeamTaskSegment lastVisible = sameSegments.LastOrDefault();
                if (lastVisible != null && !Object.ReferenceEquals(edited, lastVisible))
                    return "Resize only last day segment. Previous day time is fixed.";

                newTotalMinutes = Math.Max(15, oldTotalMinutes - edited.DurationMinutes + requestedMinutes);

                if (realType == "subtask")
                {
                    string resizeValidation = ValidateMyTeamSubtaskResizeDuration(da, regid, realEntityId, newTotalMinutes);
                    if (!String.IsNullOrWhiteSpace(resizeValidation)) return resizeValidation;
                }

                DateTime end = CalculateMyTeamAssignmentEnd(employeeId, oldStart, newTotalMinutes);
                if (MyTeamHasOverlap(da, regid, employeeId, oldStart, newTotalMinutes, assignId))
                    return "Drop only in available area";
                da.executeDMLQueryinline("update Pms_tbl_assigntasks set enddatetime='" + ToMyTeamSqlDateTime(end) +
                    "',taskduration='" + MyTeamDurationText(newTotalMinutes) + "' where regid=" + regid + " and id=" + assignId + " and status=1 and archive=0");
                return "1";
            }

            // Moving one visible day: preserve all other segments as independent rows.
            if (!resize && edited != null)
            {
                int editedMinutes = requestedMinutes;
                DateTime editedStart = selectedDate.Add(ParseMyTeamTime(starttime, "09:00 AM"));
                if (MyTeamHasOverlap(da, regid, employeeId, editedStart, editedMinutes, assignId))
                    return "Drop only in available area";

                da.executeDMLQueryinline("update Pms_tbl_assigntasks set status=0,archive=1,removedatetime=getdate() where regid=" + regid + " and id=" + assignId + " and status=1 and archive=0");
                foreach (MyTeamTaskSegment sg in sameSegments)
                {
                    bool isEdited = Object.ReferenceEquals(sg, edited);
                    DateTime segStart = isEdited
                        ? editedStart
                        : ParseMyTeamDate(sg.workDateKey).Add(ParseMyTeamTime(sg.StartTime, "09:00 AM"));
                    int segMinutes = isEdited ? editedMinutes : sg.DurationMinutes;
                    DateTime segEnd = CalculateMyTeamAssignmentEnd(employeeId, segStart, segMinutes);
                    InsertMyTeamAssignment(da, regid, uid, employeeId,
                        Convert.ToInt32(ar["projectid"]), Convert.ToInt32(ar["milestoneid"]),
                        Convert.ToInt32(ar["taskid"]), Convert.ToInt32(ar["subtaskid"]),
                        NormalizeMyTeamType(GetColumn(ar, "tasktype")), segStart, segEnd,
                        MyTeamDurationText(segMinutes), MyTeamNullableDate(ar["assigndatetime"]));
                }
                return "1";
            }
        }

        if (resize && realType == "subtask")
        {
            string resizeValidation = ValidateMyTeamSubtaskResizeDuration(da, regid, realEntityId, newTotalMinutes);
            if (!String.IsNullOrWhiteSpace(resizeValidation)) return resizeValidation;
        }

        if (!resize && !MyTeamDropStartsInsideEmployeeShift(employeeId, newStart))
            return "Drop only in available area";

        if (MyTeamHasOverlap(da, regid, employeeId, newStart, newTotalMinutes, assignId))
            return "Drop only in available area";
        DateTime newEnd = CalculateMyTeamAssignmentEnd(employeeId, newStart, newTotalMinutes);
        da.executeDMLQueryinline("update Pms_tbl_assigntasks set startdatetime='" + ToMyTeamSqlDateTime(newStart) +
            "',enddatetime='" + ToMyTeamSqlDateTime(newEnd) + "',taskduration='" + MyTeamDurationText(newTotalMinutes) +
            "' where regid=" + regid + " and id=" + assignId + " and status=1 and archive=0");
        return "1";
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string MyTeamMoveAssignment(string id, string empid, string changedate, string starttime)
    {
        int assignId, employeeId;
        int regid = CurrentRegId();
        if (regid <= 0 || CurrentUserId() <= 0 || !Int32.TryParse(id, out assignId) || assignId <= 0 ||
            !Int32.TryParse(empid, out employeeId) || employeeId <= 0) return "Invalid assignment request.";
        DataAccess da = new DataAccess();
        DataSet ds = da.GetDataSetInline("select top(1) * from Pms_tbl_assigntasks where regid=" + regid +
            " and id=" + assignId + " and status=1 and archive=0");
        if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return "Assignment no longer available.";
        DataRow assignment = ds.Tables[0].Rows[0];
        int oldEmployee = MyTeamNullableInt(assignment["ownerid"]) ?? 0;
        if (!CanCurrentUserManageEmployee(da, regid, oldEmployee) ||
            !CanCurrentUserManageEmployee(da, regid, employeeId)) return "Employee is not in your My Team list.";
        string type = NormalizeMyTeamType(GetColumn(assignment, "tasktype"));
        int entityId = MyTeamNullableInt(assignment[type == "milestone" ? "milestoneid" : type == "task" ? "taskid" : "subtaskid"]) ?? 0;
        DataRow entity = GetEntityRow(da, regid, type, entityId);
        if (entity == null) return "This item is no longer active.";
        if (!MyTeamProjectIsAvailableToLoginUser(MyTeamNullableInt(entity["ProjectId"]) ?? 0))
            return "Project is not available to you.";
        bool addToTeam = !CsvHasEmployee(GetColumn(entity, "team"), employeeId);
        DateTime date = ParseMyTeamDate(changedate);
        DateTime? from = MyTeamNullableDate(entity["StartDate"]);
        DateTime? to = MyTeamNullableDate(entity["TargetDate"]);
        if ((from.HasValue && date.Date < from.Value.Date) || (to.HasValue && date.Date > to.Value.Date))
            return "Selected date must be within the item's StartDate and TargetDate.";
        DateTime start = date.Add(ParseMyTeamTime(starttime, "09:00 AM"));
        int duration = MyTeamDurationMinutes(GetColumn(assignment, "taskduration"), 0);
        if (duration <= 0) return "Duration is required before assignment.";
        if (!MyTeamDropStartsInsideEmployeeShift(employeeId, start) ||
            MyTeamHasOverlap(da, regid, employeeId, start, duration, assignId)) return "Drop only in available area.";
        DateTime end = CalculateMyTeamAssignmentEnd(employeeId, start, duration);
        // Validate first; team cascade and transfer commit together.
        string updates = String.Empty;
        if (addToTeam)
        {
            int projectId = MyTeamNullableInt(entity["ProjectId"]) ?? 0;
            int milestoneId = MyTeamNullableInt(entity["MilestoneId"]) ?? 0;
            int taskId = MyTeamNullableInt(entity["TaskId"]) ?? 0;
            updates += MyTeamMergeEmployeeSql("Pms_tbl_AddProject", "id=" + projectId, regid, employeeId);
            updates += MyTeamMergeEmployeeSql("Pms_tbl_AddMilestone", "id=" + milestoneId + " and ProjectId=" + projectId, regid, employeeId);
            if (type == "milestone")
            {
                updates += MyTeamMergeEmployeeSql("Pms_tbl_AddTask", "Milestoneid=" + milestoneId + " and ProjectId=" + projectId, regid, employeeId);
                updates += MyTeamMergeEmployeeSql("Pms_tbl_AddSubtask", "Milestoneid=" + milestoneId + " and ProjectId=" + projectId, regid, employeeId);
            }
            else
            {
                updates += MyTeamMergeEmployeeSql("Pms_tbl_AddTask", "id=" + taskId + " and ProjectId=" + projectId, regid, employeeId);
                updates += MyTeamMergeEmployeeSql("Pms_tbl_AddSubtask",
                    (type == "task" ? "task=" + taskId : "id=" + entityId) + " and ProjectId=" + projectId, regid, employeeId);
            }
        }
        string transfer = "update Pms_tbl_assigntasks set ownerid=" + employeeId +
            ",startdatetime='" + ToMyTeamSqlDateTime(start) + "',enddatetime='" + ToMyTeamSqlDateTime(end) +
            "' where regid=" + regid + " and id=" + assignId + " and ownerid=" + oldEmployee + " and status=1 and archive=0;";
        da.executeDMLQueryinline("SET XACT_ABORT ON; BEGIN TRY BEGIN TRANSACTION; " + updates + transfer +
            " IF @@ROWCOUNT <> 1 THROW 50001, 'Assignment changed. Refresh and try again.', 1; " +
            "COMMIT TRANSACTION; END TRY BEGIN CATCH IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION; THROW; END CATCH;");
        return "1";
    }

    // Same add cascade as Assign Task; existing members are never removed.
    private static string MyTeamMergeEmployeeSql(string table, string predicate, int regid, int employeeId)
    {
        string normalized = "REPLACE(REPLACE(REPLACE(ISNULL(CONVERT(nvarchar(max),team),''),' ',''),';',','),'|',',')";
        return "update " + table + " set team=CASE WHEN " + normalized + "='' THEN '" + employeeId +
            "' ELSE " + normalized + "+'," + employeeId + "' END where regid=" + regid +
            " and status=1 and archive=0 and " + predicate +
            " and (','+" + normalized + "+',') NOT LIKE '%," + employeeId + ",%'; ";
    }

    private static string GetColumn(DataRow row, string columnName)
    {
        return row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value
            ? row[columnName].ToString()
            : String.Empty;
    }
}

public class MyTeamEmployee
{
    public string id { get; set; }
    public string empid { get; set; }
    public string name { get; set; }
    public string profileimage { get; set; }
    public string shortname { get; set; }
    public int isadmin { get; set; }
}


public class MyTeamProject
{
    public string id { get; set; }
    public string ProjectName { get; set; }
}

public class MyTeamScheduleUser
{
    public string id { get; set; }
    public string empid { get; set; }
    public string name { get; set; }
    public string profileimage { get; set; }
    public string shortname { get; set; }
    public string ShiftStart { get; set; }
    public string ShiftHours { get; set; }
    public string ShiftEnd { get; set; }
    public List<MyTeamAssignmentRow> usertasks { get; set; }
    public List<MyTeamTaskSegment> taskSegments { get; set; }
}

public class MyTeamAssignmentRow
{
    public int? id { get; set; }
    public int? assignid { get; set; }
    public DateTime? cdate { get; set; }
    public DateTime? assigndatetime { get; set; }
    public string tasktype { get; set; }
    public int? ProjectId { get; set; }
    public string ProjectName { get; set; }
    public int? Milestoneid { get; set; }
    public string Milestone { get; set; }
    public int? Task { get; set; }
    public string Taskname { get; set; }
    public string Subtaskname { get; set; }
    public string Description { get; set; }
    public string SubTaskDescription { get; set; }
    public string owner { get; set; }
    public DateTime? Startdate { get; set; }
    public DateTime? Targetdate { get; set; }
    public string StartDateKey { get; set; }
    public string TargetDateKey { get; set; }
    public string TaskDuration { get; set; }
    public string starttime { get; set; }
    public string StartTime { get; set; }
}

public class MyTeamTaskSegment
{
    public int? assignid { get; set; }
    public int? taskid { get; set; }
    public string tasktype { get; set; }
    public string taskname { get; set; }
    public string workDateKey { get; set; }
    public int DurationMinutes { get; set; }
    public string TaskDuration { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public string mid { get; set; }
    public string tid { get; set; }
}

public class MyTeamAssignableResult
{
    public List<MyTeamMilestoneFilter> Milestones { get; set; }
    public List<MyTeamAvailableItem> Tasks { get; set; }

    public MyTeamAssignableResult()
    {
        Milestones = new List<MyTeamMilestoneFilter>();
        Tasks = new List<MyTeamAvailableItem>();
    }
}

public class MyTeamMilestoneFilter
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string OrderNo { get; set; }
}

public class MyTeamAvailableItem
{
    public string Key { get; set; }
    public string Type { get; set; }
    public int EntityId { get; set; }
    public int ProjectId { get; set; }
    public string ProjectName { get; set; }
    public int MilestoneId { get; set; }
    public string MilestoneName { get; set; }
    public int TaskId { get; set; }
    public string TaskName { get; set; }
    public int SubTaskId { get; set; }
    public string Name { get; set; }
    public string OrderNo { get; set; }
    public int DurationMinutes { get; set; }
    public string Duration { get; set; }
    public string StartDate { get; set; }
    public string TargetDate { get; set; }
    public string Owner { get; set; }
    public string Team { get; set; }
}

public class MyTeamAssignableTreeResult
{
    public List<MyTeamProjectTree> Projects { get; set; }
    public List<MyTeamAvailableItem> Subtasks { get; set; }

    public MyTeamAssignableTreeResult()
    {
        Projects = new List<MyTeamProjectTree>();
        Subtasks = new List<MyTeamAvailableItem>();
    }
}

public class MyTeamProjectTree
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool Expanded { get; set; }
    public List<MyTeamMilestoneTree> Milestones { get; set; }

    public MyTeamProjectTree()
    {
        Milestones = new List<MyTeamMilestoneTree>();
    }
}

public class MyTeamMilestoneTree
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string OrderNo { get; set; }
    public string Duration { get; set; }
    public bool Expanded { get; set; }
    public List<MyTeamTaskTree> Tasks { get; set; }

    public MyTeamMilestoneTree()
    {
        Tasks = new List<MyTeamTaskTree>();
    }
}

public class MyTeamTaskTree
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string OrderNo { get; set; }
    public string Duration { get; set; }
    public bool Expanded { get; set; }
    public List<MyTeamAvailableItem> Subtasks { get; set; }

    public MyTeamTaskTree()
    {
        Subtasks = new List<MyTeamAvailableItem>();
    }
}

public class unassignsubtasklist
{
    public int? id { get; set; }
    public int? assignid { get; set; }
    public DateTime? assigndatetime { get; set; }
    public string tasktype { get; set; }
    public bool isassigned { get; set; }
    public int? ProjectId { get; set; }
    public string ProjectName { get; set; }
    public string Powner { get; set; }
    public int? Milestoneid { get; set; }
    public string Milestone { get; set; }
    public int? Task { get; set; }
    public string Taskname { get; set; }
    public string Subtaskname { get; set; }
    public string SubTaskDescription { get; set; }
    public string owner { get; set; }
    public string team { get; set; }
    public string Startdate { get; set; }
    public string Targetdate { get; set; }
    public string TaskDuration { get; set; }
    public int? P_Status { get; set; }
    public string status_name { get; set; }
    public string status_color { get; set; }
    public int? Priority { get; set; }
    public string priority_name { get; set; }
    public string priority_color { get; set; }
}
