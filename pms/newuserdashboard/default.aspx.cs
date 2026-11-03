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

public partial class newuserdashboard : Page
{
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetUserDashboardData(string fromDate, string toDate, string projectId)
    {
        HttpCookie tenantCookie = HttpContext.Current.Request.Cookies["cadmin"];
        HttpCookie userCookie = HttpContext.Current.Request.Cookies["user"];
        if (tenantCookie == null || userCookie == null || String.IsNullOrWhiteSpace(tenantCookie["id"]) || String.IsNullOrWhiteSpace(userCookie["user_id"]))
            throw new HttpException(401, "Login information is missing.");

        DateTime globalToday = GetGlobalToday();
        DateTime rangeStart = ParseDashboardDate(fromDate, globalToday.AddDays(-(((int)globalToday.DayOfWeek + 6) % 7)));
        DateTime rangeEnd = ParseDashboardDate(toDate, rangeStart.AddDays(6));
        if (rangeEnd < rangeStart)
        {
            DateTime swap = rangeStart;
            rangeStart = rangeEnd;
            rangeEnd = swap;
        }
        rangeStart = rangeStart.Date;
        rangeEnd = rangeEnd.Date;

        string regId = tenantCookie["id"];
        string userId = userCookie["user_id"];
        SqlParameter[] parameters =
        {
            new SqlParameter("@RegId", regId),
            new SqlParameter("@UserId", userId),
            new SqlParameter("@FromDate", SqlDbType.Date) { Value = rangeStart },
            new SqlParameter("@ToDate", SqlDbType.Date) { Value = rangeEnd }
        };

        DataAccess dataAccess = new DataAccess();
        DataSet data = dataAccess.GetDataSet("dbo.usp_Pms_UserDashboard_GetAssignedSubtasks", parameters);
        DataTable rows = data != null && data.Tables.Count > 0 ? data.Tables[0] : new DataTable();
        EnrichTaskDetails(rows, tenantCookie["id"]);
        rows = FilterProject(rows, projectId);
        return BuildDashboard(rows, rangeStart, rangeEnd, regId, userId, projectId);
    }
    

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetTaskHoursData(string fromDate, string toDate, string projectId)
    {
        HttpCookie tenantCookie = HttpContext.Current.Request.Cookies["cadmin"];
        HttpCookie userCookie = HttpContext.Current.Request.Cookies["user"];
        if (tenantCookie == null || userCookie == null || String.IsNullOrWhiteSpace(tenantCookie["id"]) || String.IsNullOrWhiteSpace(userCookie["user_id"]))
            throw new HttpException(401, "Login information is missing.");

        DateTime globalToday = GetGlobalToday();
        DateTime rangeStart = ParseDashboardDate(fromDate, globalToday);
        DateTime rangeEnd = ParseDashboardDate(toDate, rangeStart);
        if (rangeEnd < rangeStart)
        {
            DateTime swap = rangeStart;
            rangeStart = rangeEnd;
            rangeEnd = swap;
        }
        rangeStart = rangeStart.Date;
        rangeEnd = rangeEnd.Date;

        SqlParameter[] parameters =
        {
            new SqlParameter("@RegId", tenantCookie["id"]),
            new SqlParameter("@UserId", userCookie["user_id"]),
            new SqlParameter("@FromDate", SqlDbType.Date) { Value = rangeStart },
            new SqlParameter("@ToDate", SqlDbType.Date) { Value = rangeEnd }
        };

        DataAccess dataAccess = new DataAccess();
        DataSet data = dataAccess.GetDataSet("dbo.usp_Pms_UserDashboard_GetAssignedSubtasks", parameters);
        DataTable rows = data != null && data.Tables.Count > 0 ? data.Tables[0] : new DataTable();
        rows = FilterProject(rows, projectId);
        Dictionary<string, double> spentHoursByDate = LoadSpentHoursByDate(tenantCookie["id"], userCookie["user_id"], rangeStart, rangeEnd, String.IsNullOrWhiteSpace(projectId) ? null : rows.Rows.Cast<DataRow>().Select(GetAssignedItemKey).ToList());
        var shiftCapacity = LoadTaskHoursShift(tenantCookie["id"], userCookie["user_id"]);
        var leaveDays = LoadApprovedLeaveDays(tenantCookie["id"], userCookie["user_id"], rangeStart, rangeEnd);
        return new { days = BuildHours(rows.Rows.Cast<DataRow>().ToList(), rangeStart, rangeEnd, spentHoursByDate, leaveDays, shiftCapacity.shiftHours), shiftHours = shiftCapacity.shiftHours, fullShiftHours = shiftCapacity.fullShiftHours, lunchHours = shiftCapacity.lunchHours, leaveDays = leaveDays.Values.ToList() };
    }
    public static DataRow checkonleave(string regId, string userId, string leavedate)
    {
        DataRow dr = null;

        try
        {
            SqlParameter[] parameters =
            {
            new SqlParameter("@regid", regId),
            new SqlParameter("@uid", userId),
            new SqlParameter("@CheckDate", SqlDbType.Date) { Value = ParseDashboardDate(leavedate, GetGlobalToday()).Date }
            };

            DataAccess dataAccess = new DataAccess();
            DataSet data = dataAccess.GetDataSet("dbo.pms_GetEmployeeLeaveDetails", parameters);
            if (data != null &&data.Tables.Count > 0 &&data.Tables[0].Rows.Count > 0)
            {
                dr = data.Tables[0].Rows[0];
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Trace.TraceError("Employee leave lookup failed: {0}", ex);
        }

        return dr;
    }
    private sealed class LeaveDayInfo
    {
        public string key { get; set; }
        public double fraction { get; set; }
        public string label { get; set; }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetTaskHoursLeaveData(string fromDate, string toDate)
    {
        HttpCookie tenant = HttpContext.Current.Request.Cookies["cadmin"];
        HttpCookie user = HttpContext.Current.Request.Cookies["user"];
        if (tenant == null || user == null) throw new HttpException(401, "Login information is missing.");
        DateTime start = ParseDashboardDate(fromDate, GetGlobalToday()).Date;
        DateTime end = ParseDashboardDate(toDate, start).Date;
        if (end < start) { DateTime swap = start; start = end; end = swap; }
        return new { days = LoadApprovedLeaveDays(tenant["id"], user["user_id"], start, end).Values.ToList() };
    }

    private static Dictionary<string, LeaveDayInfo> LoadApprovedLeaveDays(string regId, string userId, DateTime start, DateTime end)
    {
        int tenantId, employeeId;
        if (!Int32.TryParse(regId, out tenantId) || !Int32.TryParse(userId, out employeeId))
            throw new HttpException(401, "Login information is invalid.");
        // One range read; pending/rejected leaves never affect the chart.
        string sql = "SELECT fromdate,todate,ishalfday,firsthalfday,secondhalhday FROM dbo.tbl_empleavedetails " +
            "WHERE status=1 AND archive=0 AND isapprove=1 AND regid=" + tenantId + " AND uid=" + employeeId +
            " AND fromdate < '" + end.AddDays(1).ToString("yyyyMMdd", CultureInfo.InvariantCulture) +
            "' AND todate >= '" + start.ToString("yyyyMMdd", CultureInfo.InvariantCulture) + "'";
        DataSet data = new DataAccess().GetDataSetInline(sql);
        var flags = new Dictionary<string, int>();
        if (data != null && data.Tables.Count > 0)
        foreach (DataRow row in data.Tables[0].Rows)
        {
            DateTime? from = LeaveDate(row["fromdate"]), to = LeaveDate(row["todate"]);
            if (!from.HasValue || !to.HasValue) continue;
            bool half = LeaveFlag(row["ishalfday"]);
            bool first = LeaveFlag(row["firsthalfday"]), second = LeaveFlag(row["secondhalhday"]);
            int mask = !half ? 3 : first != second ? (first ? 1 : 2) : 4;
            DateTime dayStart = from.Value.Date > start ? from.Value.Date : start;
            DateTime dayEnd = to.Value.Date < end ? to.Value.Date : end;
            for (DateTime day = dayStart; day <= dayEnd; day = day.AddDays(1))
            {
                string key = day.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                int previous; flags.TryGetValue(key, out previous); flags[key] = previous | mask;
            }
        }
        var result = new Dictionary<string, LeaveDayInfo>();
        foreach (var entry in flags)
        {
            int halves = entry.Value & 3;
            result[entry.Key] = new LeaveDayInfo { key = entry.Key,
                fraction = halves == 3 ? 1 : 0.5,
                label = (halves == 3 ? "Full Day" : halves == 1 ? "First Half" : halves == 2 ? "Second Half" : "Half Day (half not specified)") + " (Approved)" };
        }
        return result;
    }

    private static bool LeaveFlag(object value)
    {
        string text = Convert.ToString(value, CultureInfo.InvariantCulture);
        return text == "1" || text.Equals("true", StringComparison.OrdinalIgnoreCase);
    }

    private static DateTime? LeaveDate(object value)
    {
        if (value == null || value == DBNull.Value) return null;
        DateTime date;
        return DateTime.TryParse(Convert.ToString(value, CultureInfo.InvariantCulture), CultureInfo.InvariantCulture, DateTimeStyles.None, out date) ? date.Date : (DateTime?)null;
    }

    private static TaskHoursShift LoadTaskHoursShift(string regId, string userId)
    {
        int tenantId, employeeId;
        if (!Int32.TryParse(regId, out tenantId) || !Int32.TryParse(userId, out employeeId))
            throw new HttpException(401, "Login information is invalid.");
        DataAccess da = new DataAccess();
        DataSet shift = da.GetDataSetInline(@"
select top(1) shft.starttime, shft.endtime, isnull(shft.lunchbreak,0) as lunchbreak
from tbl_employee emp with(nolock)
inner join tbl_shift shft with(nolock) on emp.ShiftType = shft.id
where emp.status=1 and emp.archive=0 and emp.regid=" + tenantId + " and emp.id=" + employeeId);
        // Same fallback as Weekly View when no shift is configured.
        TimeSpan start = new TimeSpan(9, 0, 0), end = new TimeSpan(18, 0, 0);
        double lunchMinutes = 0;
        if (shift != null && shift.Tables.Count > 0 && shift.Tables[0].Rows.Count > 0)
        {
            start = ParseTaskHoursShiftTime(shift.Tables[0].Rows[0]["starttime"].ToString(), start);
            end = ParseTaskHoursShiftTime(shift.Tables[0].Rows[0]["endtime"].ToString(), end);
            double parsedLunch;
            if (Double.TryParse(shift.Tables[0].Rows[0]["lunchbreak"].ToString(), NumberStyles.Number, CultureInfo.InvariantCulture, out parsedLunch))
                lunchMinutes = Math.Max(0, parsedLunch);
        }
        double minutes = (end - start).TotalMinutes;
        if (minutes <= 0) minutes += 24 * 60;
        return new TaskHoursShift {
            shiftHours = Math.Max(0, minutes - Math.Min(lunchMinutes, minutes)) / 60.0,
            fullShiftHours = minutes / 60.0,
            lunchHours = Math.Min(lunchMinutes, minutes) / 60.0
        };
    }

    private sealed class TaskHoursShift
    {
        public double shiftHours { get; set; }
        public double fullShiftHours { get; set; }
        public double lunchHours { get; set; }
    }

    private static TimeSpan ParseTaskHoursShiftTime(string value, TimeSpan fallback)
    {
        DateTime parsed;
        string[] formats = { "hh:mm tt", "h:mm tt", "HH:mm", "HH:mm:ss", "H:mm", "H:mm:ss" };
        if (!String.IsNullOrWhiteSpace(value) &&
            (DateTime.TryParseExact(value.Trim(), formats, CultureInfo.InvariantCulture,
                DateTimeStyles.AllowWhiteSpaces, out parsed) || DateTime.TryParse(value, out parsed)))
            return parsed.TimeOfDay;
        return fallback;
    }

    private static DateTime GetGlobalToday()
    {
        TimeZoneInfo globalTimeZone;
        try
        {
            globalTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time");
        }
        catch (TimeZoneNotFoundException)
        {
            globalTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Dubai");
        }
        return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, globalTimeZone).Date;
    }

    private static DateTime ParseDashboardDate(string value, DateTime fallback)
    {
        DateTime result;
        return DateTime.TryParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out result) ? result.Date : fallback.Date;
    }

    private static IssueSummary LoadIssueSummary(string regId, string userId, DateTime rangeStart, DateTime rangeEnd, string projectId)
    {
        var summary = new IssueSummary();
        int regIdValue;
        int userIdValue;
        if (!Int32.TryParse(regId, NumberStyles.Integer, CultureInfo.InvariantCulture, out regIdValue) ||
            !Int32.TryParse(userId, NumberStyles.Integer, CultureInfo.InvariantCulture, out userIdValue))
            return summary;

        string from = rangeStart.Date.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
        string to = rangeEnd.Date.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
        string employee = userIdValue.ToString(CultureInfo.InvariantCulture);
        string sql = @"
SELECT ps.id AS IssueStatusId, ps.name AS IssueStatusName,
       CONVERT(nvarchar(max), ps.PrintColor) AS IssueStatusColor, COUNT(DISTINCT i.id) AS IssueCount
FROM dbo.Pms_tbl_Addissues i WITH (NOLOCK)
LEFT JOIN dbo.Pms_tbl_addIssueStatus ps WITH (NOLOCK)
  ON ps.id = i.IssueStatus AND ps.regid = i.regid AND ps.status = 1 AND ps.archive = 0
WHERE i.regid = " + regIdValue.ToString(CultureInfo.InvariantCulture) + @"
  AND i.status = 1 AND i.archive = 0
  AND ISNULL(i.cdate, i.StartDate) >= CONVERT(date, '" + from + @"', 112)
  AND ISNULL(i.cdate, i.StartDate) < DATEADD(day, 1, CONVERT(date, '" + to + @"', 112))
  AND " + IssueEmployeeMatchClause(employee) + (String.IsNullOrWhiteSpace(projectId) ? "" : " AND i.projectid=" + ParseProjectId(projectId).ToString(CultureInfo.InvariantCulture)) + @"
GROUP BY ps.id, ps.name, CONVERT(nvarchar(max), ps.PrintColor)
ORDER BY ps.id";

        DataSet data = new DataAccess().GetDataSetInline(sql);
        if (data != null && data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow row in data.Tables[0].Rows)
            {
                int count = (int)Number(row, "IssueCount");
                string name = Value(row, "IssueStatusName");
                string color = Value(row, "IssueStatusColor");
                summary.Items.Add(new GroupItem {
                    Name = String.IsNullOrWhiteSpace(name) ? "Unknown status" : name,
                    Count = count,
                    Color = String.IsNullOrWhiteSpace(color) ? "#a5c7ff" : color
                });
                summary.Total += count;
            }
        }
        return summary;
    }

    private static string IssueEmployeeMatchClause(string employee)
    {
        string pattern = "(',' + REPLACE(REPLACE(REPLACE(ISNULL(CONVERT(nvarchar(2000),{0}),''),' ',''),';',','),'|',',') + ',') LIKE '%," + employee + ",%'";
        return "(TRY_CONVERT(int,i.Owner)=" + employee +
               " OR " + String.Format(CultureInfo.InvariantCulture, pattern, "i.Owner") +
               " OR TRY_CONVERT(int,i.AssignTo)=" + employee +
               " OR " + String.Format(CultureInfo.InvariantCulture, pattern, "i.AssignTo") +
               " OR " + String.Format(CultureInfo.InvariantCulture, pattern, "i.team") + ")";
    }

    private static object BuildDashboard(DataTable rows, DateTime rangeStart, DateTime rangeEnd, string regId, string userId, string projectId)
    {
        List<DataRow> records = rows.Rows.Cast<DataRow>().ToList();
        int total = records.Count;
        List<GroupItem> statuses = LoadConfiguredStatuses(records, regId);
        List<GroupItem> priorities = Group(records, "PriorityName", "PriorityColor");
        List<GroupItem> situations = Group(records, "SituationName", "SituationColor");

        var statusItems = statuses.Select((item, index) => new
        {
            name = item.Name,
            count = item.Count,
            percent = Percent(item.Count, total),
            color = item.Color,
            bubble = 24 + (int)Math.Round(Math.Sqrt(item.Count) * 7),
            bubbleColor = BubbleColor(index),
            textColor = BubbleTextColor(index),
            fontSize = 12 + Math.Min(20, item.Count),
            x = 8 + ((index * 23) % 74),
            y = 8 + ((index * 31) % 66)
        }).ToList();

        var statusKpis = statuses.Select((item, index) => new
        {
            name = item.Name,
            count = item.Count,
            icon = StatusIcon(item.Name),
            color = item.Color
        }).ToList();

        Dictionary<string, double> spentHoursByDate = LoadSpentHoursByDate(regId, HttpContext.Current.Request.Cookies["user"]["user_id"], rangeStart, rangeEnd, String.IsNullOrWhiteSpace(projectId) ? null : records.Select(GetAssignedItemKey).ToList());
        double timeEfficiency = CalculateAverageTimeEfficiency(records, regId, userId);
        int totalAssignedSubtasks = records.Count;
        int completedSubtasks = records.Count(IsCompletedSubtask);
        double completion = totalAssignedSubtasks == 0
            ? 0
            : (completedSubtasks * 100.0) / totalAssignedSubtasks;
        IssueSummary issueSummary = LoadIssueSummary(regId, userId, rangeStart, rangeEnd, projectId);
        int totalIssues = issueSummary.Total;

        var now = GetGlobalToday();
        var shiftCapacity = LoadTaskHoursShift(regId, userId);
        var leaveDays = LoadApprovedLeaveDays(regId, userId, rangeStart, rangeEnd);
        var hourDays = BuildHours(records, rangeStart, rangeEnd, spentHoursByDate, leaveDays, shiftCapacity.shiftHours);
        var priorityItems = BuildChartItems(priorities, total);
        var situationItems = BuildChartItems(situations, total);
        var issueItems = BuildChartItems(issueSummary.Items, totalIssues);

        return new
        {
            tasks = BuildTaskDetails(records, regId, userId),
            userName = CurrentUserName(),
            asOf = now,
            statuses = statusKpis,
            taskStatus = new { total = total, items = statusItems },
            priority = new { total = total, items = priorityItems, gradient = Gradient(priorities.Select(x => x.Count).ToList(), priorities.Select(x => x.Color).ToList()) },
            situation = new { total = total, items = situationItems, gradient = Gradient(situations.Select(x => x.Count).ToList(), situations.Select(x => x.Color).ToList()) },
            hours = new { days = hourDays, shiftHours = shiftCapacity.shiftHours, fullShiftHours = shiftCapacity.fullShiftHours, lunchHours = shiftCapacity.lunchHours, leaveDays = leaveDays.Values.ToList() },
            efficiencyTrend = BuildEfficiencyTrend(regId, userId, projectId, rangeEnd),
            taskEfficiency = Math.Round(completion),
            timeEfficiency = timeEfficiency,
            tags = BuildTagCounts(records, regId),
            issues = new { total = totalIssues, items = issueItems, gradient = Gradient(issueSummary.Items.Select(x => x.Count).ToList(), issueSummary.Items.Select(x => x.Color).ToList()) }
        };
    }


    private static List<object> BuildEfficiencyTrend(string regId, string userId, string projectId, DateTime selectedEnd)
    {
        DateTime today = GetGlobalToday();
        DateTime end = selectedEnd.Date > today ? today : selectedEnd.Date;
        DateTime currentWeek = end.AddDays(-(((int)end.DayOfWeek + 6) % 7));
        DateTime firstWeek = currentWeek.AddDays(-35);
        DataSet data = new DataAccess().GetDataSet("dbo.usp_Pms_UserDashboard_GetAssignedSubtasks", new[] {
            new SqlParameter("@RegId",regId), new SqlParameter("@UserId",userId),
            new SqlParameter("@FromDate",SqlDbType.Date) { Value=firstWeek },
            new SqlParameter("@ToDate",SqlDbType.Date) { Value=end }
        });
        DataTable rows = data != null && data.Tables.Count>0 ? data.Tables[0] : new DataTable();
        rows = FilterProject(rows,projectId);
        var records = rows.Rows.Cast<DataRow>().ToList();
        var results = new List<object>();
        for (int i=0;i<6;i++)
        {
            DateTime start=firstWeek.AddDays(i*7), finish=start.AddDays(6);
            if (finish>end) finish=end;
            var week=records.Where(row => { DateTime assigned=DateValue(row,"AssignmentStart","AssignedAt"); return assigned.Date>=start && assigned.Date<=finish; }).ToList();
            int complete=week.Count(IsCompletedSubtask);
            results.Add(new { week=start.ToString("yyyy-MM-dd",CultureInfo.InvariantCulture),
                task=week.Count==0 ? (double?)null : Math.Round(complete*100.0/week.Count),
                time=complete==0 ? (double?)null : CalculateAverageTimeEfficiency(week,regId,userId) });
        }
        return results;
    }

    private static int ParseProjectId(string value)
    {
        int id;
        if (!Int32.TryParse(value, out id) || id <= 0) throw new HttpException(400, "Invalid project.");
        return id;
    }
    private static DataTable FilterProject(DataTable rows, string projectId)
    {
        if (String.IsNullOrWhiteSpace(projectId)) return rows;
        int id = ParseProjectId(projectId);
        DataTable filtered = rows.Clone();
        foreach (DataRow row in rows.Rows) if (Number(row, "ProjectId") == id) filtered.ImportRow(row);
        return filtered;
    }
    // Display enrichment is keyed by actual assigned item type/id, never by subtaskid alone.
    private static void EnrichTaskDetails(DataTable rows, string regId)
    {
        if (rows.Rows.Count == 0) return;
        int tenant;
        if (!Int32.TryParse(regId, out tenant)) throw new HttpException(401, "Invalid login.");
        DataAccess da = new DataAccess();
        DataSet schema = da.GetDataSetInline(@"SELECT t.name AS TableName,c.name AS ColumnName FROM sys.tables t
            INNER JOIN sys.columns c ON c.object_id=t.object_id
            WHERE SCHEMA_NAME(t.schema_id)='dbo' AND t.name IN
            ('Pms_tbl_AddMilestone','Pms_tbl_AddTask','Pms_tbl_AddSubTask','Pms_tbl_AddProject','Pms_tbl_AddProjects')");
        var columns = schema.Tables[0].Rows.Cast<DataRow>().ToList();
        Func<string, string[], string> choose = (table, candidates) => candidates.FirstOrDefault(candidate =>
            columns.Any(row => Value(row,"TableName").Equals(table,StringComparison.OrdinalIgnoreCase) && Value(row,"ColumnName").Equals(candidate,StringComparison.OrdinalIgnoreCase)));
        if (!rows.Columns.Contains("DueDate")) rows.Columns.Add("DueDate", typeof(DateTime));
        if (!rows.Columns.Contains("CompletedDate")) rows.Columns.Add("CompletedDate", typeof(DateTime));
        if (!rows.Columns.Contains("ProjectName")) rows.Columns.Add("ProjectName", typeof(string));
        if (!rows.Columns.Contains("SerialNo")) rows.Columns.Add("SerialNo", typeof(string));
        var detailsByKey = new Dictionary<string,DataRow>(StringComparer.OrdinalIgnoreCase);
        foreach (string type in new[] { "milestone", "task", "subtask" })
        {
            string table = type == "milestone" ? "Pms_tbl_AddMilestone" : type == "task" ? "Pms_tbl_AddTask" : "Pms_tbl_AddSubTask";
            string ids = String.Join(",", rows.Rows.Cast<DataRow>().Where(row => AssignedItemType(row)==type)
                .Select(AssignedItemId).Where(id=>id>0).Distinct().Select(id=>id.ToString(CultureInfo.InvariantCulture)).ToArray());
            if (String.IsNullOrEmpty(ids)) continue;
            string due = choose(table, type == "milestone" ? new[] { "EndDate", "TargetDate", "DueDate", "EndDatetime", "DueDatetime" } : new[] { "TargetDate", "DueDate", "EndDate", "EndDatetime", "DueDatetime" });
            string finished = choose(table, new[] { "CompletedDate", "CompletionDate", "CompleteDate", "CompletedOn", "CompletedDatetime", "CompleteDatetime", "ActualEndDate", "FinishDate" });
            string sql = "SELECT id AS ItemId," + (due == null ? "CAST(NULL AS datetime)" : "["+due+"]") + " AS DueDate," +
                (finished == null ? "CAST(NULL AS datetime)" : "["+finished+"]") + " AS CompletedDate FROM dbo.["+table+"] WHERE regid="+tenant+" AND status=1 AND archive=0 AND id IN ("+ids+")";
            DataSet details=da.GetDataSetInline(sql);
            if (details!=null && details.Tables.Count>0)
                foreach (DataRow detail in details.Tables[0].Rows) detailsByKey[type+":"+Value(detail,"ItemId")]=detail;
        }
        string projectTable = columns.Any(row => Value(row,"TableName").Equals("Pms_tbl_AddProject",StringComparison.OrdinalIgnoreCase)) ? "Pms_tbl_AddProject" : "Pms_tbl_AddProjects";
        string projectName = choose(projectTable,new[] { "Projectname", "ProjectName", "Name", "Title" });
        string projectIds = String.Join(",",rows.Rows.Cast<DataRow>().Select(row=>(int)Number(row,"ProjectId")).Where(id=>id>0).Distinct().Select(id=>id.ToString(CultureInfo.InvariantCulture)).ToArray());
        var names = new Dictionary<int,string>();
        if (projectName != null && !String.IsNullOrEmpty(projectIds))
        {
            DataSet projects=da.GetDataSetInline("SELECT id,["+projectName+"] AS ProjectName FROM dbo.["+projectTable+"] WHERE regid="+tenant+" AND id IN ("+projectIds+")");
            if (projects!=null && projects.Tables.Count>0)
                foreach (DataRow project in projects.Tables[0].Rows) names[(int)Number(project,"id")]=Value(project,"ProjectName");
        }
        var serials=LoadHierarchySerials(da,tenant,projectIds,choose);
        foreach (DataRow row in rows.Rows)
        {
            DataRow detail; DateTime date;
            if (detailsByKey.TryGetValue(GetAssignedItemKey(row),out detail))
            {
                if (DateTime.TryParse(Value(detail,"DueDate"),out date)) row["DueDate"]=date.Date;
                if (DateTime.TryParse(Value(detail,"CompletedDate"),out date)) row["CompletedDate"]=date.Date;
            }
            string name,serial;
            row["ProjectName"]=names.TryGetValue((int)Number(row,"ProjectId"),out name) ? name : "Project "+Value(row,"ProjectId");
            row["SerialNo"]=serials.TryGetValue(GetAssignedItemKey(row),out serial) ? serial : String.Empty;
        }
    }

    private static Dictionary<string,string> LoadHierarchySerials(DataAccess da,int tenant,string projectIds,Func<string,string[],string> choose)
    {
        var serials=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
        if (String.IsNullOrEmpty(projectIds)) return serials;
        string milestoneProject=choose("Pms_tbl_AddMilestone",new[] { "projectid", "pid" });
        string taskMilestone=choose("Pms_tbl_AddTask",new[] { "milestoneid", "mid" });
        string subtaskTask=choose("Pms_tbl_AddSubTask",new[] { "Task", "taskid", "tid" });
        if (milestoneProject==null || taskMilestone==null || subtaskTask==null)
            throw new InvalidOperationException("Hierarchy parent columns could not be resolved for the dashboard serial number.");
        string milestoneOrder=choose("Pms_tbl_AddMilestone",new[] { "orderno" });
        string taskOrder=choose("Pms_tbl_AddTask",new[] { "orderno" });
        string subtaskOrder=choose("Pms_tbl_AddSubTask",new[] { "orderno" });
        Func<string,string,string> order=(alias,column)=>column==null ? "CAST(NULL AS nvarchar(100))" : "CONVERT(nvarchar(100),"+alias+".["+column+"])";
        // Read all active siblings in each relevant project, before assignment/date/status filters.
        string sql="SELECT 'milestone' AS ItemType,m.id AS ItemId,m.["+milestoneProject+"] AS ProjectId,0 AS ParentId,"+order("m",milestoneOrder)+" AS LocalOrder FROM dbo.Pms_tbl_AddMilestone m WHERE m.regid="+tenant+" AND m.status=1 AND m.archive=0 AND m.["+milestoneProject+"] IN ("+projectIds+")"+
            " UNION ALL SELECT 'task',t.id,m.["+milestoneProject+"],t.["+taskMilestone+"],"+order("t",taskOrder)+" FROM dbo.Pms_tbl_AddTask t INNER JOIN dbo.Pms_tbl_AddMilestone m ON m.id=t.["+taskMilestone+"] AND m.regid=t.regid WHERE t.regid="+tenant+" AND t.status=1 AND t.archive=0 AND m.status=1 AND m.archive=0 AND m.["+milestoneProject+"] IN ("+projectIds+")"+
            " UNION ALL SELECT 'subtask',s.id,m.["+milestoneProject+"],s.["+subtaskTask+"],"+order("s",subtaskOrder)+" FROM dbo.Pms_tbl_AddSubTask s INNER JOIN dbo.Pms_tbl_AddTask t ON t.id=s.["+subtaskTask+"] AND t.regid=s.regid INNER JOIN dbo.Pms_tbl_AddMilestone m ON m.id=t.["+taskMilestone+"] AND m.regid=t.regid WHERE s.regid="+tenant+" AND s.status=1 AND s.archive=0 AND t.status=1 AND t.archive=0 AND m.status=1 AND m.archive=0 AND m.["+milestoneProject+"] IN ("+projectIds+")";
        DataSet hierarchy=da.GetDataSetInline(sql);
        if (hierarchy==null || hierarchy.Tables.Count==0) return serials;
        var nodes=hierarchy.Tables[0].Rows.Cast<DataRow>().ToList();
        foreach (string type in new[] { "milestone", "task", "subtask" })
        {
            var groups=nodes.Where(row=>Value(row,"ItemType")==type).GroupBy(row=>type=="milestone" ? Value(row,"ProjectId") : Value(row,"ParentId"));
            foreach (var group in groups)
            {
                int index=0;
                foreach (DataRow node in group.OrderBy(row=>Number(row,"LocalOrder")).ThenBy(row=>Number(row,"ItemId")))
                {
                    index++;
                    string local=Value(node,"LocalOrder").Trim();
                    decimal localNumber;
                    if (Decimal.TryParse(local,NumberStyles.Any,CultureInfo.InvariantCulture,out localNumber))
                        local=localNumber>0 ? localNumber.ToString("0.############################",CultureInfo.InvariantCulture) : String.Empty;
                    if (String.IsNullOrEmpty(local)) local=index.ToString(CultureInfo.InvariantCulture);
                    string serial=local,parent;
                    if (type!="milestone")
                    {
                        string parentKey=(type=="task" ? "milestone:" : "task:")+Value(node,"ParentId");
                        if (!serials.TryGetValue(parentKey,out parent)) continue;
                        serial=parent+"."+local;
                    }
                    serials[type+":"+Value(node,"ItemId")]=serial;
                }
            }
        }
        return serials;
    }

    private static List<object> BuildTaskDetails(List<DataRow> records,string regId,string userId)
    {
        var spent=LoadCompleteHoursByItem(regId,userId,records.Select(GetAssignedItemKey).Distinct().ToList());
        return records.Select(row=> {
            double hours; spent.TryGetValue(GetAssignedItemKey(row),out hours);
            DateTime due; string dueKey=DateTime.TryParse(Value(row,"DueDate"),out due) ? due.ToString("yyyy-MM-dd",CultureInfo.InvariantCulture) : null;
            DateTime finished; string completedKey=DateTime.TryParse(Value(row,"CompletedDate"),out finished) ? finished.ToString("yyyy-MM-dd",CultureInfo.InvariantCulture) : null;
            string type=AssignedItemType(row);
            return (object)new { completedDate=completedKey,id=AssignedItemId(row),itemKey=GetAssignedItemKey(row),serialNo=Value(row,"SerialNo"),
                type=type=="milestone" ? "Milestone" : type=="task" ? "Task" : "Subtask",
                title=String.IsNullOrWhiteSpace(Value(row,"ItemName")) ? Value(row,"Subtaskname") : Value(row,"ItemName"),projectId=Value(row,"ProjectId"),project=Value(row,"ProjectName"),
                status=Value(row,"StatusName"),statusColor=Value(row,"StatusColor"),completed=IsCompletedSubtask(row),
                priority=Value(row,"PriorityName"),priorityColor=Value(row,"PriorityColor"),situation=Value(row,"SituationName"),situationColor=Value(row,"SituationColor"),
                due=dueKey,planned=DurationHours(Value(row,"AssignedDuration")),spent=hours,issues=Number(row,"TotalIssues"),resolved=Number(row,"ResolvedIssues") };
        }).ToList();
    }

    private static List<object> BuildTagCounts(List<DataRow> records, string regId)
    {
        var tagIds = new HashSet<int>();
        foreach (DataRow row in records)
        {
            foreach (string part in Value(row, "TagIds").Split(','))
            {
                int id;
                if (Int32.TryParse(part.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out id) && id > 0)
                    tagIds.Add(id);
            }
        }

        if (tagIds.Count == 0) return new List<object>();

        var counts = new Dictionary<int, int>();
        foreach (DataRow row in records)
        {
            var rowTagIds = new HashSet<int>();
            foreach (string part in Value(row, "TagIds").Split(','))
            {
                int id;
                if (Int32.TryParse(part.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out id) && id > 0)
                    rowTagIds.Add(id);
            }
            foreach (int id in rowTagIds)
                counts[id] = counts.ContainsKey(id) ? counts[id] + 1 : 1;
        }

        string idList = String.Join(",", tagIds.Select(id => id.ToString(CultureInfo.InvariantCulture)).ToArray());
        string safeRegId = (regId ?? String.Empty).Replace("'", "''");
        string sql = "SELECT id, Tags, TagsColor FROM dbo.Pms_tbl_Addtags WITH (NOLOCK) " +
                     "WHERE status=1 AND archive=0 AND regid='" + safeRegId + "' AND id IN (" + idList + ")";
        DataSet tagData;
        try { tagData = new DataAccess().GetDataSetInline(sql); }
        catch { return new List<object>(); }
        if (tagData == null || tagData.Tables.Count == 0) return new List<object>();

        var result = new List<TagCountItem>();
        foreach (DataRow tag in tagData.Tables[0].Rows)
        {
            int id;
            if (!Int32.TryParse(Convert.ToString(tag["id"], CultureInfo.InvariantCulture), out id) || !counts.ContainsKey(id))
                continue;
            string name = Convert.ToString(tag["Tags"], CultureInfo.InvariantCulture);
            string color = Convert.ToString(tag["TagsColor"], CultureInfo.InvariantCulture);
            result.Add(new TagCountItem { Name = name, Color = String.IsNullOrWhiteSpace(color) ? "#000" : color, Count = counts[id] });
        }
        return result.OrderByDescending(item => item.Count)
            .ThenBy(item => item.Name, StringComparer.OrdinalIgnoreCase)
            .Select(item => (object)new { name = item.Name, color = item.Color, count = item.Count }).ToList();
    }

    private static List<GroupItem> LoadConfiguredStatuses(List<DataRow> records, string regId)
    {
        int tenant; if (!Int32.TryParse(regId, out tenant)) throw new HttpException(401, "Invalid login.");
        var groups = Group(records, "StatusName", "StatusColor");
        DataSet configured = new DataAccess().GetDataSetInline("SELECT ProjectStatus,PrintColor FROM dbo.Pms_tbl_Projectstatus WHERE regid=" + tenant + " AND status=1 AND archive=0 ORDER BY id");
        var result = new List<GroupItem>();
        if (configured != null && configured.Tables.Count > 0)
        foreach (DataRow row in configured.Tables[0].Rows)
        {
            string name = Value(row,"ProjectStatus");
            if (String.IsNullOrWhiteSpace(name) || result.Any(item => item.Name.Equals(name,StringComparison.OrdinalIgnoreCase))) continue;
            GroupItem group = groups.FirstOrDefault(item => item.Name.Equals(name,StringComparison.OrdinalIgnoreCase));
            result.Add(new GroupItem { Name=name, Count=group == null ? 0 : group.Count, Color=String.IsNullOrWhiteSpace(Value(row,"PrintColor")) ? "#8a94a6" : Value(row,"PrintColor") });
        }
        result.AddRange(groups.Where(group => !result.Any(item => item.Name.Equals(group.Name,StringComparison.OrdinalIgnoreCase))));
        return result;
    }

    private static List<GroupItem> Group(List<DataRow> rows, string nameColumn, string colorColumn)
    {
        return rows.GroupBy(row => String.IsNullOrWhiteSpace(Value(row, nameColumn)) ? "Unspecified" : Value(row, nameColumn), StringComparer.OrdinalIgnoreCase)
            .Select(group => new GroupItem
            {
                Name = group.Key,
                Count = group.Count(),
                Color = String.IsNullOrWhiteSpace(Value(group.First(), colorColumn)) ? "#a5c7ff" : Value(group.First(), colorColumn)
            }).OrderByDescending(item => item.Count).ToList();
    }

    private static List<object> BuildChartItems(List<GroupItem> groups, int total)
    {
        return groups.Select(item => (object)new { name = item.Name, count = item.Count, percent = Percent(item.Count, total), color = item.Color }).ToList();
    }

    private static double CalculateAverageTimeEfficiency(List<DataRow> records, string regId, string userId)
    {
        var completed = records.Where(IsCompletedSubtask)
            .Select(row => new { Key = GetAssignedItemKey(row), AssignedHours = DurationHours(Value(row, "AssignedDuration")) })
            .Where(item => !String.IsNullOrEmpty(item.Key) && item.AssignedHours > 0)
            .GroupBy(item => item.Key)
            .Select(group => new { Key = group.Key, AssignedHours = group.Max(item => item.AssignedHours) }).ToList();
        if (completed.Count == 0) return 0;
        var spent = LoadCompleteHoursByItem(regId, userId, completed.Select(item => item.Key).ToList());
        return Math.Round(completed.Select(item =>
        {
            double hours;
            return spent.TryGetValue(item.Key, out hours) && hours > 0 ? item.AssignedHours / hours * 100.0 : 0.0;
        }).Average());
    }

    private static string GetAssignedItemKey(DataRow row)
    {
        string type = Value(row, "ItemType").Trim().ToLowerInvariant();
        if (String.IsNullOrEmpty(type)) type = Value(row, "TaskType").Trim().ToLowerInvariant();
        if (String.IsNullOrEmpty(type)) type = "subtask"; // Compatibility with the previous procedure.
        if (type != "milestone" && type != "task" && type != "subtask") return String.Empty;
        int id;
        if (!Int32.TryParse(Value(row, "ItemId"), out id) || id <= 0)
        {
            string column = type == "milestone" ? "MilestoneId" : type == "task" ? "TaskId" : "SubtaskId";
            if (!Int32.TryParse(Value(row, column), out id) || id <= 0) return String.Empty;
        }
        return type + ":" + id.ToString(CultureInfo.InvariantCulture);
    }

    private static Dictionary<string, double> LoadCompleteHoursByItem(string regId, string userId, List<string> keys)
    {
        var result = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);
        int tenant, user;
        if (keys == null || keys.Count == 0 || !Int32.TryParse(regId, out tenant) || !Int32.TryParse(userId, out user)) return result;
        var clauses = new List<string>();
        foreach (string type in new[] { "milestone", "task", "subtask" })
        {
            var ids = new List<int>();
            foreach (string key in keys.Distinct())
            {
                string[] parts = key.Split(':');
                int id;
                if (parts.Length == 2 && parts[0] == type && Int32.TryParse(parts[1], out id) && id > 0) ids.Add(id);
            }
            if (ids.Count > 0)
                clauses.Add("(ItemType='" + type + "' AND ItemId IN (" + String.Join(",", ids.Select(id => id.ToString(CultureInfo.InvariantCulture)).ToArray()) + "))");
        }
        if (clauses.Count == 0) return result;
        string sql = @"
;WITH TimerItems AS
(
    SELECT LOWER(LTRIM(RTRIM(ISNULL(type,'subtask')))) AS ItemType,
           CASE LOWER(LTRIM(RTRIM(ISNULL(type,'subtask'))))
               WHEN 'milestone' THEN milestoneid WHEN 'task' THEN taskid
               WHEN 'subtask' THEN subtaskid END AS ItemId, PausedTime
    FROM dbo.Pms_Tbl_PauseStatus WITH (NOLOCK)
    WHERE status=1 AND archive=0 AND regid=" + tenant.ToString(CultureInfo.InvariantCulture) + @"
      AND uid=" + user.ToString(CultureInfo.InvariantCulture) + @"
      AND ISNULL(entrytype,'') IN ('pause','stop')
      AND ISNUMERIC(ISNULL(PausedTime,0))=1
)
SELECT ItemType,ItemId,ISNULL(SUM(CAST(ISNULL(PausedTime,0) AS numeric(18,2))),0) AS TotalMinutes
FROM TimerItems WHERE " + String.Join(" OR ", clauses.ToArray()) + @"
GROUP BY ItemType,ItemId";
        DataSet data = new DataAccess().GetDataSetInline(sql);
        if (data == null || data.Tables.Count == 0) return result;
        foreach (DataRow row in data.Tables[0].Rows)
        {
            int id;
            if (!Int32.TryParse(Value(row, "ItemId"), out id) || id <= 0) continue;
            result[Value(row, "ItemType") + ":" + id.ToString(CultureInfo.InvariantCulture)] = Math.Max(0, Number(row, "TotalMinutes") / 60.0);
        }
        return result;
    }

    private static string AssignedItemType(DataRow row)
    {
        string key = GetAssignedItemKey(row);
        return String.IsNullOrEmpty(key) ? "subtask" : key.Split(':')[0];
    }
    private static int AssignedItemId(DataRow row)
    {
        string key = GetAssignedItemKey(row);
        int id;
        return !String.IsNullOrEmpty(key) && Int32.TryParse(key.Split(':')[1], out id) ? id : 0;
    }
    private static string TimerItemFilter(List<string> keys)
    {
        if (keys == null) return "1=1";
        var clauses = new List<string>();
        foreach (string type in new[] { "milestone", "task", "subtask" })
        {
            var ids = new List<int>();
            foreach (string key in keys.Distinct())
            {
                string[] parts = key.Split(':'); int id;
                if (parts.Length == 2 && parts[0] == type && Int32.TryParse(parts[1], out id) && id > 0) ids.Add(id);
            }
            if (ids.Count > 0) clauses.Add("(LOWER(LTRIM(RTRIM(ISNULL(type,'subtask'))))='" + type + "' AND " + type + "id IN (" + String.Join(",", ids.Select(id => id.ToString(CultureInfo.InvariantCulture)).ToArray()) + "))");
        }
        return clauses.Count == 0 ? "1=0" : "(" + String.Join(" OR ", clauses.ToArray()) + ")";
    }

    private static Dictionary<string, double> LoadSpentHoursByDate(string regId, string userId, DateTime rangeStart, DateTime rangeEnd, List<string> itemKeys = null)
    {
        var result = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);
        int regIdValue;
        int userIdValue;
        if (!Int32.TryParse(regId, NumberStyles.Integer, CultureInfo.InvariantCulture, out regIdValue) ||
            !Int32.TryParse(userId, NumberStyles.Integer, CultureInfo.InvariantCulture, out userIdValue))
            return result;

        DateTime endExclusive = rangeEnd.Date.AddDays(1);
        string startSql = rangeStart.Date.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
        string endSql = endExclusive.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
        string sql = @"
SELECT CONVERT(varchar(10), CAST(StartDatetime AS date), 23) AS WorkDate,
       ISNULL(SUM(CAST(ISNULL(PausedTime, 0) AS numeric(18,2))), 0) AS TotalMinutes
FROM Pms_Tbl_PauseStatus WITH (NOLOCK)
WHERE status = 1
  AND archive = 0
  AND regid = " + regIdValue.ToString(CultureInfo.InvariantCulture) + @"
  AND uid = " + userIdValue.ToString(CultureInfo.InvariantCulture) + @"
  AND " + TimerItemFilter(itemKeys) + @"
  AND StartDatetime >= CONVERT(datetime, '" + startSql + @"', 112)
  AND StartDatetime < CONVERT(datetime, '" + endSql + @"', 112)
  AND ISNULL(entrytype, '') IN ('pause', 'stop')
  AND ISNUMERIC(ISNULL(PausedTime, 0)) = 1
GROUP BY CAST(StartDatetime AS date)";

        DataSet data = new DataAccess().GetDataSetInline(sql);
        if (data == null || data.Tables.Count == 0) return result;

        foreach (DataRow row in data.Tables[0].Rows)
        {
            DateTime workDate;
            if (!DateTime.TryParse(Value(row, "WorkDate"), CultureInfo.InvariantCulture, DateTimeStyles.None, out workDate))
                continue;
            double totalMinutes = Number(row, "TotalMinutes");
            result[workDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)] = Math.Max(0, totalMinutes / 60.0);
        }
        return result;
    }

    // Same calendar policy as Weekly View; load once per requested range.
    private static Dictionary<string, double> LoadDailyCapacity(DateTime start, DateTime end,
        Dictionary<string, LeaveDayInfo> leaves, double shiftHours)
    {
        int regId = Convert.ToInt32(HttpContext.Current.Request.Cookies["cadmin"]["id"], CultureInfo.InvariantCulture);
        DataAccess da = new DataAccess();
        DataRow weekOff = null;
        DataSet policy = da.GetDataSetInline("select top(1) * from tbl_weekoffmaster with(nolock) where status=1 and archive=0 and isnull(isdefault,0)=1 and regid=" + regId);
        if (policy != null && policy.Tables.Count > 0 && policy.Tables[0].Rows.Count > 0) weekOff = policy.Tables[0].Rows[0];
        var holidays = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        DataSet holidayData = da.GetDataSetInline("select cast(eventdate as date) as EventDate from holidays with(nolock) where status=1 and archive=0 and regid=" + regId +
            " and eventdate>=convert(datetime,'" + start.ToString("yyyyMMdd", CultureInfo.InvariantCulture) + "',112) and eventdate<convert(datetime,'" + end.AddDays(1).ToString("yyyyMMdd", CultureInfo.InvariantCulture) + "',112)");
        if (holidayData != null && holidayData.Tables.Count > 0)
            foreach (DataRow row in holidayData.Tables[0].Rows)
                holidays.Add(Convert.ToDateTime(row["EventDate"], CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
        var result = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);
        for (DateTime day = start.Date; day <= end.Date; day = day.AddDays(1))
        {
            string key = day.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            LeaveDayInfo leave;
            double fraction = leaves.TryGetValue(key, out leave) ? Math.Max(0, Math.Min(1, leave.fraction)) : 0;
            result[key] = holidays.Contains(key) || WeeklyIsWeekOff(day, weekOff) ? 0 : Math.Max(0, shiftHours * (1 - fraction));
        }
        return result;
    }

    private static double CapacityBetween(Dictionary<string, double> capacity, DateTime start, DateTime end)
    {
        string first = start.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        string last = end.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        return capacity.Where(item => String.CompareOrdinal(item.Key, first) >= 0 && String.CompareOrdinal(item.Key, last) <= 0).Sum(item => item.Value);
    }

    private static int WeeklyGetWeekNumber(DateTime date)
    {
        try
        {
            DataAccess da = new DataAccess();
            SqlParameter[] prm = new SqlParameter[1];
            prm[0] = new SqlParameter("@DATE", date);
            DataSet ds = da.GetDataSet("selectweeknofromdate", prm);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int n;
                if (Int32.TryParse(ds.Tables[0].Rows[0]["WEEK_OF_MONTH"].ToString(), out n)) return n;
            }
        }
        catch { }
        return ((date.Day - 1) / 7) + 1;
    }

    private static int WeeklyReturnStatus(string type, int weekNo)
    {
        if (type == "1") return 1;
        if (type == "2" && weekNo == 1) return 1;
        if (type == "3" && weekNo == 2) return 1;
        if (type == "4" && weekNo == 3) return 1;
        if (type == "5" && weekNo == 4) return 1;
        if (type == "6" && weekNo == 5) return 1;
        if (type == "7" && (weekNo == 1 || weekNo == 3)) return 1;
        if (type == "8" && (weekNo == 1 || weekNo == 3 || weekNo == 5)) return 1;
        if (type == "9" && (weekNo == 2 || weekNo == 4)) return 1;
        return 0;
    }

    private static bool WeeklyIsWeekOff(DateTime date, DataRow row)
    {
        if (row == null) return false;
        try
        {
            int dayNo = ((int)date.DayOfWeek) + 1; // Sunday=1 ... Saturday=7, same as Assign Project.
            string enabledColumn = "isweekoff" + dayNo.ToString(CultureInfo.InvariantCulture);
            string typeColumn = enabledColumn + "type";
            if (!row.Table.Columns.Contains(enabledColumn) || !row.Table.Columns.Contains(typeColumn)) return false;
            if (row[enabledColumn].ToString() != "1") return false;
            string type = row[typeColumn].ToString();
            if (type == "1") return true;
            return WeeklyReturnStatus(type, WeeklyGetWeekNumber(date)) == 1;
        }
        catch { return false; }
    }

    private static List<object> BuildHours(List<DataRow> rows, DateTime rangeStart, DateTime rangeEnd, IDictionary<string, double> spentHoursByDate, Dictionary<string, LeaveDayInfo> leaveDays, double shiftHours)
    {
        spentHoursByDate = spentHoursByDate ?? new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);
        var capacity = LoadDailyCapacity(rangeStart, rangeEnd, leaveDays, shiftHours);
        int spanDays = Math.Max(1, (rangeEnd.Date - rangeStart.Date).Days + 1);
        var results = new List<object>();
        if (spanDays <= 14)
        {
            for (DateTime date = rangeStart.Date; date <= rangeEnd.Date; date = date.AddDays(1))
            {
                string dayLabel = spanDays == 1 ? "Today" : date.ToString("ddd", CultureInfo.InvariantCulture);
                string key = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                double spent = spentHoursByDate.ContainsKey(key) ? spentHoursByDate[key] : 0;
                results.Add(new { day = dayLabel, key = key, assigned = Math.Round(capacity[key], 2), spent = spent });
            }
            return results;
        }

        if (spanDays <= 62)
        {
            for (DateTime start = rangeStart.Date; start <= rangeEnd.Date; start = start.AddDays(7))
            {
                DateTime end = start.AddDays(6) < rangeEnd.Date ? start.AddDays(6) : rangeEnd.Date;
                double spent = spentHoursByDate.Where(item => { DateTime d; return DateTime.TryParseExact(item.Key, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out d) && d.Date >= start && d.Date <= end; }).Sum(item => item.Value);
                results.Add(new { day = start.ToString("dd MMM", CultureInfo.InvariantCulture), fromKey = start.ToString("yyyy-MM-dd"), toKey = end.ToString("yyyy-MM-dd"), assigned = Math.Round(CapacityBetween(capacity, start, end), 2), spent = spent });
            }
            return results;
        }

        DateTime monthStart = new DateTime(rangeStart.Year, rangeStart.Month, 1);
        while (monthStart <= rangeEnd.Date)
        {
            DateTime monthEnd = monthStart.AddMonths(1).AddDays(-1);
            double spent = spentHoursByDate.Where(item => { DateTime d; return DateTime.TryParseExact(item.Key, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out d) && d.Date >= monthStart && d.Date <= monthEnd; }).Sum(item => item.Value);
            results.Add(new { day = monthStart.ToString("MMM yy", CultureInfo.InvariantCulture), fromKey = (monthStart < rangeStart ? rangeStart : monthStart).ToString("yyyy-MM-dd"), toKey = (monthEnd > rangeEnd ? rangeEnd : monthEnd).ToString("yyyy-MM-dd"), assigned = Math.Round(CapacityBetween(capacity, monthStart < rangeStart ? rangeStart : monthStart, monthEnd > rangeEnd ? rangeEnd : monthEnd), 2), spent = spent });
            monthStart = monthStart.AddMonths(1);
        }
        return results;
    }

    private static string Gradient(List<int> values, List<string> colors)
    {
        int total = values.Sum();
        if (total <= 0 || colors.Count == 0) return "conic-gradient(#e5e9ef 0 100%)";
        var parts = new List<string>();
        double current = 0;
        for (int i = 0; i < values.Count; i++)
        {
            double next = current + values[i] * 100.0 / total;
            parts.Add(colors[i] + " " + current.ToString("0.##", CultureInfo.InvariantCulture) + "% " + next.ToString("0.##", CultureInfo.InvariantCulture) + "%");
            current = next;
        }
        return "conic-gradient(" + String.Join(",", parts) + ")";
    }

    private static int Percent(int count, int total) { return total == 0 ? 0 : (int)Math.Round(count * 100.0 / total); }
    private static double DurationHours(string value) { TimeSpan duration; return TimeSpan.TryParse(value, CultureInfo.InvariantCulture, out duration) ? duration.TotalHours : 0; }
    private static bool IsCompletedSubtask(DataRow row)
    {
        if (row.Table.Columns.Contains("IsComplete") && row["IsComplete"] != DBNull.Value)
        {
            bool completeFlag;
            if (Boolean.TryParse(Convert.ToString(row["IsComplete"], CultureInfo.InvariantCulture), out completeFlag))
                return completeFlag;
            return Number(row, "IsComplete") != 0;
        }

        string status = Value(row, "StatusName").Trim();
        if (status.Equals("Complete", StringComparison.OrdinalIgnoreCase) ||
            status.Equals("Completed", StringComparison.OrdinalIgnoreCase) ||
            status.Equals("Done", StringComparison.OrdinalIgnoreCase) ||
            status.Equals("Closed", StringComparison.OrdinalIgnoreCase))
            return true;

        return Number(row, "CompletionPercent") >= 100;
    }

    private static double Number(DataRow row, string column) { double value; return Double.TryParse(Value(row, column), NumberStyles.Any, CultureInfo.InvariantCulture, out value) ? value : 0; }
    private static string Value(DataRow row, string column) { return row.Table.Columns.Contains(column) && row[column] != DBNull.Value ? Convert.ToString(row[column], CultureInfo.InvariantCulture) : ""; }
    private static DateTime DateValue(DataRow row, string primary, string fallback) { DateTime value; string text = Value(row, primary); if (!DateTime.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out value)) DateTime.TryParse(Value(row, fallback), CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out value); return value; }
    private static string CurrentUserName() { HttpCookie cookie = HttpContext.Current.Request.Cookies["user"]; return cookie == null ? "User" : (cookie["user_name"] ?? "User"); }
    private static string BubbleColor(int index) { string[] colors = { "#ffe7d8", "#d5ffd7", "#fff4d1", "#ebdbff", "#d9edff" }; return colors[index % colors.Length]; }
    private static string BubbleTextColor(int index) { string[] colors = { "#f96e18", "#00b80e", "#e5ad00", "#8a38f5", "#1682d4" }; return colors[index % colors.Length]; }
    private static string StatusIcon(string name) { string value = (name ?? "").ToLowerInvariant(); if (value.Contains("complete")) return "ri-checkbox-circle-line"; if (value.Contains("progress")) return "ri-hourglass-line"; if (value.Contains("reopen")) return "ri-history-line"; return "ri-folder-open-line"; }

    private sealed class IssueSummary { public int Total; public List<GroupItem> Items = new List<GroupItem>(); }
    private sealed class GroupItem { public string Name; public int Count; public string Color; }
    private sealed class TagCountItem { public string Name; public int Count; public string Color; }
   
}
