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

public partial class userdashboardreport : Page
{
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetReportData(string fromDate, string toDate, string reportType, string groupBy,
        string issueStatus, string issueCompletion, string issueEntityType, string employeeSearch, string issueSearch)
    {
        HttpCookie tenantCookie = HttpContext.Current.Request.Cookies["cadmin"];
        HttpCookie userCookie = HttpContext.Current.Request.Cookies["user"];
        if (tenantCookie == null || userCookie == null || String.IsNullOrWhiteSpace(tenantCookie["id"]) || String.IsNullOrWhiteSpace(userCookie["user_id"]))
            throw new HttpException(401, "Login information is missing.");

        DateTime today = GetGlobalToday();
        DateTime start = ParseDate(fromDate, today);
        DateTime end = ParseDate(toDate, start);
        if (end < start) { DateTime swap = start; start = end; end = swap; }

        if (String.Equals(reportType, "issues", StringComparison.OrdinalIgnoreCase))
            return BuildIssueReport(tenantCookie["id"], userCookie["user_id"], start, end,
                issueStatus, issueCompletion, issueEntityType, employeeSearch, issueSearch);

        SqlParameter[] parameters =
        {
            new SqlParameter("@RegId", tenantCookie["id"]),
            new SqlParameter("@UserId", userCookie["user_id"]),
            new SqlParameter("@FromDate", SqlDbType.Date) { Value = start.Date },
            new SqlParameter("@ToDate", SqlDbType.Date) { Value = end.Date }
        };
        DataSet data = new DataAccess().GetDataSet("dbo.usp_Pms_UserDashboard_GetAssignedSubtasks", parameters);
        DataTable table = data != null && data.Tables.Count > 0 ? data.Tables[0] : new DataTable();
        List<DataRow> records = table.Rows.Cast<DataRow>().ToList();
        Dictionary<int, TaskReportMetadata> taskMetadata = LoadTaskReportMetadata(tenantCookie["id"], userCookie["user_id"], records);
        if (String.Equals(reportType, "hours", StringComparison.OrdinalIgnoreCase))
            return BuildHoursReport(records, taskMetadata, tenantCookie["id"], userCookie["user_id"], start, end);
        Dictionary<int, double> spent = LoadSpentHours(tenantCookie["id"], userCookie["user_id"], records);
        Dictionary<int, string> tags = LoadTags(tenantCookie["id"], records);
        Dictionary<int, double> assignedBySubtask = records.Where(row => IsComplete(row) && GetSubtaskId(row) > 0)
            .GroupBy(GetSubtaskId)
            .ToDictionary(group => group.Key, group => group.Max(row => DurationHours(First(row, "AssignedDuration", "TaskDuration", "TotalTime"))));
        var rows = records.Select(row =>
        {
            int id = GetSubtaskId(row);
            double assignedHours = DurationHours(First(row, "AssignedDuration", "TaskDuration", "TotalTime"));
            double efficiencyAssignedHours = assignedBySubtask.ContainsKey(id) ? assignedBySubtask[id] : assignedHours;
            double spentHours = spent.ContainsKey(id) ? spent[id] : 0;
            bool complete = IsComplete(row);
            TaskReportMetadata metadata;
            taskMetadata.TryGetValue(id, out metadata);
            return new
            {
                subtaskId = id,
                projectName = FirstOrFallback(First(row, "ProjectName", "Project", "Project_Name"), metadata == null ? "" : metadata.ProjectName),
                taskName = First(row, "SubtaskName", "Subtaskname", "SubTaskName", "TaskName", "Task", "Subtask", "Name"),
                serialNo = First(row, "HierarchySrNo", "TaskSrNo", "SubtaskSrNo", "SerialNo"),
                description = FirstOrFallback(First(row, "Description", "TaskDescription", "SubtaskDescription", "TaskDesc", "SubtaskDesc"), metadata == null ? "" : metadata.Description),
                priority = First(row, "PriorityName", "Priority"),
                status = First(row, "StatusName", "Status"),
                situation = First(row, "SituationName", "Situation"),
                completion = Math.Max(0, Math.Min(100, Number(row, "CompletionPercent"))),
                assignedText = FormatDuration(assignedHours),
                spentText = Math.Round(spentHours * 60.0) > 0 ? FormatDuration(spentHours) : "",
                efficiency = complete ? (spentHours > 0 ? Math.Round(efficiencyAssignedHours / spentHours * 100.0, 1) : 0.0) : (double?)null,
                tags = TagNames(First(row, "TagIds", "Tags"), tags),
                startDate = FormatDate(First(row, "AssignmentStart", "AssignedAt", "StartDate")),
                endDate = FormatDate(FirstOrFallback(First(row, "AssignmentEnd", "AssignmentEndDate", "AssignedEndDate", "TargetDate", "TaskEndDate", "SubtaskEndDate", "EndDate", "DueDate"), metadata == null ? "" : metadata.AssignmentEnd)),
                isComplete = complete,
                assignedHours = Math.Round(assignedHours, 2),
                spentHours = Math.Round(spentHours, 2)
            };
        }).ToList();

        double averageEfficiency = rows.Where(row => row.isComplete && row.subtaskId > 0)
            .GroupBy(row => row.subtaskId).Select(group => group.First().efficiency.GetValueOrDefault()).DefaultIfEmpty(0).Average();
        return new
        {
            reportType = reportType ?? "status",
            fromDate = start.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            toDate = end.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            rows = rows,
            priorityOptions = LoadPriorityOptions(tenantCookie["id"]),
            statusOptions = LoadStatusOptions(tenantCookie["id"]),
            total = rows.Count,
            completed = rows.Count(row => row.isComplete),
            completion = rows.Count == 0 ? 0 : Math.Round(rows.Count(row => row.isComplete) * 100.0 / rows.Count),
            timeEfficiency = Math.Round(averageEfficiency)
        };
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetGroupedReportData(string fromDate, string toDate, string reportType,
        string groupBy, string groupName, string priority, string status, string search,
        int page, int pageSize, string mode)
    {
        HttpCookie tenant = HttpContext.Current.Request.Cookies["cadmin"];
        HttpCookie user = HttpContext.Current.Request.Cookies["user"];
        if (tenant == null || user == null || String.IsNullOrWhiteSpace(tenant["id"]) || String.IsNullOrWhiteSpace(user["user_id"]))
            throw new HttpException(401, "Login information is missing.");
        string report = (reportType ?? "status").ToLowerInvariant();
        if (report != "status" && report != "priority" && report != "situation" && report != "tag" && report != "taskefficiency" && report != "timeefficiency")
            throw new HttpException(400, "Invalid grouped report.");
        string grouping = (groupBy ?? "status").ToLowerInvariant();
        if (grouping != "status" && grouping != "priority" && grouping != "situation" && grouping != "tag")
            throw new HttpException(400, "Invalid grouping.");
        if (mode != "summary" && mode != "detail") throw new HttpException(400, "Invalid report mode.");
        DateTime start = ParseDate(fromDate, GetGlobalToday()), end = ParseDate(toDate, start);
        if (end < start) { DateTime swap = start; start = end; end = swap; }
        SqlParameter[] parameters = {
            new SqlParameter("@RegId", tenant["id"]), new SqlParameter("@UserId", user["user_id"]),
            new SqlParameter("@FromDate", SqlDbType.Date) { Value = start.Date },
            new SqlParameter("@ToDate", SqlDbType.Date) { Value = end.Date },
            new SqlParameter("@GroupBy", grouping),
            new SqlParameter("@GroupName", SqlDbType.NVarChar, 4000) { Value = (object)groupName ?? DBNull.Value },
            new SqlParameter("@Priority", priority ?? "ALL"), new SqlParameter("@Status", status ?? "ALL"),
            new SqlParameter("@Search", search ?? ""), new SqlParameter("@Page", Math.Max(1, page)),
            new SqlParameter("@PageSize", Math.Min(100, Math.Max(1, pageSize))), new SqlParameter("@Mode", mode)
        };
        DataSet data = new DataAccess().GetDataSet("dbo.usp_Pms_UserReport_Grouped", parameters);
        DataTable results = data != null && data.Tables.Count > 0 ? data.Tables[0] : new DataTable();
        DataRow totals = data != null && data.Tables.Count > 1 && data.Tables[1].Rows.Count > 0 ? data.Tables[1].Rows[0] : null;
        if (mode == "summary")
        {
            var groups = results.Rows.Cast<DataRow>().Select(row => new {
                name = Value(row, "Name"), count = (int)Number(row, "Count"),
                assignedText = FormatDuration(Number(row, "AssignedHours")),
                spentText = Number(row, "SpentHours") > 0 ? FormatDuration(Number(row, "SpentHours")) : "-",
                efficiencyText = report == "taskefficiency"
                    ? (Number(row, "Count") > 0 ? Number(row, "Completed") * 100.0 / Number(row, "Count") : 0).ToString("0.0", CultureInfo.InvariantCulture) + "%"
                    : (row["Efficiency"] == DBNull.Value ? "-" : Number(row, "Efficiency").ToString("0.0", CultureInfo.InvariantCulture) + "%")
            }).ToList();
            int total = totals == null ? 0 : (int)Number(totals, "Total");
            int completed = totals == null ? 0 : (int)Number(totals, "Completed");
            return new {
                groups = groups, total = total, completed = completed,
                completion = total == 0 ? 0 : Math.Round(completed * 100.0 / total),
                timeEfficiency = totals == null ? 0 : Math.Round(Number(totals, "TimeEfficiency")),
                priorityOptions = LoadPriorityOptions(tenant["id"]), statusOptions = LoadStatusOptions(tenant["id"]),
                fromDate = start.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), toDate = end.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
            };
        }
        var rows = results.Rows.Cast<DataRow>().Select(row => new {
            subtaskId = (int)Number(row, "SubtaskId"), projectName = Value(row, "ProjectName"),
            taskName = Value(row, "TaskName"), description = Value(row, "Description"), serialNo = "",
            priority = Value(row, "Priority"), status = Value(row, "Status"), situation = Value(row, "Situation"),
            completion = Number(row, "Completion"), assignedHours = Number(row, "AssignedHours"), spentHours = Number(row, "SpentHours"),
            assignedText = FormatDuration(Number(row, "AssignedHours")),
            spentText = Math.Round(Number(row, "SpentHours") * 60.0) > 0 ? FormatDuration(Number(row, "SpentHours")) : "",
            efficiency = row["Efficiency"] == DBNull.Value ? (double?)null : Number(row, "Efficiency"),
            isComplete = Convert.ToBoolean(row["IsComplete"], CultureInfo.InvariantCulture), tags = Value(row, "Tags"),
            startDate = FormatDate(Value(row, "StartDate")), endDate = FormatDate(Value(row, "EndDate"))
        }).ToList();
        return new {
            rows = rows, total = totals == null ? 0 : (int)Number(totals, "Total"),
            page = totals == null ? 1 : (int)Number(totals, "Page"),
            pageSize = totals == null ? pageSize : (int)Number(totals, "PageSize")
        };
    }

    private static List<string> LoadPriorityOptions(string regId)
    {
        int reg;
        if (!Int32.TryParse(regId, NumberStyles.Integer, CultureInfo.InvariantCulture, out reg)) return new List<string>();
        string sql = "SELECT DISTINCT ISNULL(name,'') AS FilterName FROM dbo.Pms_tbl_Priority WITH (NOLOCK) " +
                     "WHERE regid=" + reg.ToString(CultureInfo.InvariantCulture) + " AND status=1 AND archive=0 " +
                     "AND NULLIF(LTRIM(RTRIM(ISNULL(name,''))),'') IS NOT NULL ORDER BY FilterName";
        DataSet data = new DataAccess().GetDataSetInline(sql);
        if (data == null || data.Tables.Count == 0) return new List<string>();
        return data.Tables[0].Rows.Cast<DataRow>().Select(row => Value(row, "FilterName")).Where(value => !String.IsNullOrWhiteSpace(value)).ToList();
    }

    private static List<string> LoadStatusOptions(string regId)
    {
        int reg;
        if (!Int32.TryParse(regId, NumberStyles.Integer, CultureInfo.InvariantCulture, out reg)) return new List<string>();
        string sql = "SELECT DISTINCT ISNULL(ProjectStatus,'') AS FilterName FROM dbo.Pms_tbl_Projectstatus WITH (NOLOCK) " +
                     "WHERE regid=" + reg.ToString(CultureInfo.InvariantCulture) + " AND status=1 AND archive=0 " +
                     "AND NULLIF(LTRIM(RTRIM(ISNULL(ProjectStatus,''))),'') IS NOT NULL ORDER BY FilterName";
        DataSet data = new DataAccess().GetDataSetInline(sql);
        if (data == null || data.Tables.Count == 0) return new List<string>();
        return data.Tables[0].Rows.Cast<DataRow>().Select(row => Value(row, "FilterName")).Where(value => !String.IsNullOrWhiteSpace(value)).ToList();
    }

    private static List<string> LoadIssueStatusOptions(string regId)
    {
        int reg;
        if (!Int32.TryParse(regId, out reg)) return new List<string>();
        string sql = "SELECT DISTINCT ISNULL(name,'') AS FilterName FROM dbo.Pms_tbl_addIssueStatus WITH (NOLOCK) " +
            "WHERE regid=" + reg.ToString(CultureInfo.InvariantCulture) + " AND Status=1 AND Archive=0 " +
            "AND NULLIF(LTRIM(RTRIM(ISNULL(name,''))),'') IS NOT NULL ORDER BY FilterName";
        DataSet data = new DataAccess().GetDataSetInline(sql);
        if (data == null || data.Tables.Count == 0) return new List<string>();
        return data.Tables[0].Rows.Cast<DataRow>().Select(row => Value(row, "FilterName")).ToList();
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
            int weekNo = WeeklyGetWeekNumber(date);
            string enabledColumn = "isweekoff" + dayNo.ToString(CultureInfo.InvariantCulture);
            string typeColumn = enabledColumn + "type";
            if (!row.Table.Columns.Contains(enabledColumn) || !row.Table.Columns.Contains(typeColumn)) return false;
            if (row[enabledColumn].ToString() != "1") return false;
            return WeeklyReturnStatus(row[typeColumn].ToString(), weekNo) == 1;
        }
        catch { return false; }
    }

    private sealed class LeaveDayInfo
    {
        public string key { get; set; }
        public double fraction { get; set; }
        public string label { get; set; }
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

    public sealed class ReportHoursDay
    {
        public string key { get; set; }
        public string weekday { get; set; }
        public string dateLabel { get; set; }
        public bool isWeekOff { get; set; }
        public bool isHoliday { get; set; }
        public double assignedHours { get; set; }
        public string assignedText { get; set; }
        public string lunchText { get; set; }
        public string leaveLabel { get; set; }
    }

    private static object BuildHoursReport(List<DataRow> records, Dictionary<int, TaskReportMetadata> taskMetadata, string regId, string userId, DateTime start, DateTime end)
    {
        int reg, user;
        if (!Int32.TryParse(regId, out reg) || !Int32.TryParse(userId, out user))
            throw new HttpException(401, "Login information is invalid.");
        var shift = LoadTaskHoursShift(regId, userId);
        var da = new DataAccess();
        DataRow weekOff = null;
        DataSet policy = da.GetDataSetInline("select top(1) * from tbl_weekoffmaster with(nolock) where status=1 and archive=0 and isnull(isdefault,0)=1 and regid=" + reg);
        if (policy != null && policy.Tables.Count > 0 && policy.Tables[0].Rows.Count > 0)
            weekOff = policy.Tables[0].Rows[0];
        var holidays = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        DataSet holidayData = da.GetDataSetInline("select cast(eventdate as date) as EventDate from holidays with(nolock) where status=1 and archive=0 and regid=" + reg +
            " and eventdate>=convert(date,'" + start.ToString("yyyyMMdd", CultureInfo.InvariantCulture) + "',112) and eventdate<convert(date,'" + end.AddDays(1).ToString("yyyyMMdd", CultureInfo.InvariantCulture) + "',112)");
        if (holidayData != null && holidayData.Tables.Count > 0)
            foreach (DataRow row in holidayData.Tables[0].Rows)
                holidays.Add(Convert.ToDateTime(row["EventDate"], CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
        var approvedLeaves = LoadApprovedLeaveDays(regId, userId, start.Date, end.Date);
        var days = new List<ReportHoursDay>();
        var dayKeys = new List<string>();
        for (DateTime day = start.Date; day <= end.Date; day = day.AddDays(1))
        {
            string key = day.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            bool off = WeeklyIsWeekOff(day, weekOff);
            bool holiday = holidays.Contains(key);
            bool working = !off && !holiday;
            LeaveDayInfo leave;
            approvedLeaves.TryGetValue(key, out leave);
            // Half day uses 50% of the net shift, matching the dashboard rule.
            double availableHours = working ? shift.shiftHours * (leave == null ? 1 : 1 - leave.fraction) : 0;
            dayKeys.Add(key);
            days.Add(new ReportHoursDay {
                key = key,
                weekday = day.ToString("dddd", CultureInfo.InvariantCulture),
                dateLabel = day.ToString("dd MMM", CultureInfo.InvariantCulture),
                isWeekOff = off, isHoliday = holiday,
                assignedHours = availableHours,
                assignedText = FormatDuration(availableHours),
                lunchText = FormatDuration(working && (leave == null || leave.fraction < 1) ? shift.lunchHours : 0),
                leaveLabel = leave == null ? "" : leave.label
            });
        }
        double capacityHours = days.Sum(day => day.assignedHours);

        var uniqueRows = records.Where(row => GetSubtaskId(row) > 0)
            .GroupBy(GetSubtaskId).Select(group => group.First()).ToList();
        var dailySpent = LoadSpentHoursBySubtaskAndDate(regId, userId, uniqueRows, start, end);
        var taskRows = uniqueRows.Select(row =>
        {
            int subtaskId = GetSubtaskId(row);
            TaskReportMetadata metadata;
            taskMetadata.TryGetValue(subtaskId, out metadata);
            double assignedHours = capacityHours;
            var daily = dayKeys.Select(key => dailySpent.ContainsKey(subtaskId + "|" + key) ? dailySpent[subtaskId + "|" + key] : 0).ToList();
            double actualHours = daily.Sum();
            return new HoursReportRow
            {
                subtaskId = subtaskId,
                projectName = FirstOrFallback(First(row, "ProjectName", "Project", "Project_Name"), metadata == null ? "" : metadata.ProjectName),
                taskName = First(row, "SubtaskName", "Subtaskname", "SubTaskName", "TaskName", "Task", "Subtask", "Name"),
                priority = First(row, "PriorityName", "Priority"),
                status = First(row, "StatusName", "Status"),
                dailyActual = daily.Select(FormatDuration).ToList(),
                assignedHours = assignedHours,
                actualHours = actualHours,
                assignedText = FormatDuration(assignedHours),
                actualText = FormatDuration(actualHours)
            };
        }).ToList();

        var totals = dayKeys.Select((key, index) => taskRows.Sum(row => DurationHours(row.dailyActual[index]))).ToList();
        double assignedTotal = taskRows.Sum(row => row.assignedHours);
        double actualTotal = totals.Sum();
        return new
        {
            reportType = "hours",
            fromDate = start.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            toDate = end.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            days = days,
            rows = taskRows,
            dailyTotals = totals.Select(FormatDuration).ToList(),
            assignedTotalText = FormatDuration(assignedTotal),
            actualTotalText = FormatDuration(actualTotal),
            priorityOptions = LoadPriorityOptions(regId),
            statusOptions = LoadStatusOptions(regId),
            total = taskRows.Count,
            completed = 0,
            completion = 0,
            timeEfficiency = 0
        };
    }

    private static Dictionary<int, TaskReportMetadata> LoadTaskReportMetadata(string regId, string userId, List<DataRow> records)
    {
        var result = new Dictionary<int, TaskReportMetadata>();
        int reg, user;
        var ids = records.Select(GetSubtaskId).Where(id => id > 0).Distinct().ToList();
        if (ids.Count == 0 || !Int32.TryParse(regId, out reg) || !Int32.TryParse(userId, out user)) return result;

        // The base subtask table stores ProjectId, Task, Milestoneid, and Targetdate directly.
        // Load these known fields independently so an optional description/assignment column
        // cannot prevent Project Name and End Date from being returned.
        Dictionary<string, HashSet<string>> columns = LoadReportTableColumns();
        HashSet<string> subtaskColumns = TableColumns(columns, "Pms_tbl_AddSubTask");
        HashSet<string> taskColumns = TableColumns(columns, "Pms_tbl_AddTask");
        HashSet<string> projectColumns = TableColumns(columns, "Pms_tbl_AddProject");
        HashSet<string> assignmentColumns = TableColumns(columns, "Pms_tbl_assigntasks");
        AddKnownColumns(subtaskColumns, "id", "regid", "task", "projectid", "milestoneid", "targetdate", "status", "archive");
        AddKnownColumns(taskColumns, "id", "regid", "milestoneid");
        AddKnownColumns(projectColumns, "id", "regid", "projectname");
        AddKnownColumns(assignmentColumns, "regid", "uid", "subtaskid", "projectid", "enddatetime", "status", "archive");

        string idList = String.Join(",", ids.Select(id => id.ToString(CultureInfo.InvariantCulture)).ToArray());
        string coreSql = "SELECT s.id AS SubTaskId, ISNULL(p.ProjectName,'') AS ProjectName, " +
            "CONVERT(nvarchar(50),s.Targetdate,126) AS AssignmentEnd " +
            "FROM dbo.Pms_tbl_AddSubTask s WITH (NOLOCK) " +
            "LEFT JOIN dbo.Pms_tbl_AddProject p WITH (NOLOCK) ON p.id=s.ProjectId AND p.regid=s.regid " +
            "WHERE s.regid=" + reg.ToString(CultureInfo.InvariantCulture) + " AND s.id IN (" + idList + ")";
        try
        {
            DataSet coreData = new DataAccess().GetDataSetInline(coreSql);
            if (coreData != null && coreData.Tables.Count > 0)
            {
                foreach (DataRow row in coreData.Tables[0].Rows)
                {
                    int id;
                    if (!Int32.TryParse(Value(row, "SubTaskId"), NumberStyles.Integer, CultureInfo.InvariantCulture, out id) || id <= 0) continue;
                    result[id] = new TaskReportMetadata
                    {
                        ProjectName = Value(row, "ProjectName"),
                        AssignmentEnd = Value(row, "AssignmentEnd")
                    };
                }
            }
        }
        catch { return result; }

        // Prefer the assigned end date when present; retain the subtask Targetdate as fallback.
        string assignmentEndColumn = FirstExistingColumn(assignmentColumns, "enddatetime", "enddate", "end_date");
        if (assignmentColumns.Contains("subtaskid") && assignmentColumns.Contains("regid") && assignmentColumns.Contains("uid") && assignmentEndColumn != null)
        {
            string assignmentFilters = "a.regid=" + reg.ToString(CultureInfo.InvariantCulture) + " AND a.uid=" + user.ToString(CultureInfo.InvariantCulture) +
                " AND a.subtaskid IN (" + idList + ")";
            if (assignmentColumns.Contains("status")) assignmentFilters += " AND a.status=1";
            if (assignmentColumns.Contains("archive")) assignmentFilters += " AND a.archive=0";
            string assignmentSql = "SELECT a.subtaskid AS SubTaskId, MAX(TRY_CONVERT(datetime,a.[" + assignmentEndColumn + "])) AS AssignmentEnd " +
                "FROM dbo.Pms_tbl_assigntasks a WITH (NOLOCK) WHERE " + assignmentFilters + " GROUP BY a.subtaskid";
            try
            {
                DataSet assignmentData = new DataAccess().GetDataSetInline(assignmentSql);
                if (assignmentData != null && assignmentData.Tables.Count > 0)
                    foreach (DataRow row in assignmentData.Tables[0].Rows)
                    {
                        int id;
                        if (Int32.TryParse(Value(row, "SubTaskId"), NumberStyles.Integer, CultureInfo.InvariantCulture, out id) &&
                            result.ContainsKey(id) && !String.IsNullOrWhiteSpace(Value(row, "AssignmentEnd")))
                            result[id].AssignmentEnd = Value(row, "AssignmentEnd");
                    }
            }
            catch { }
        }

        // Descriptions are optional across installations. Query only columns found in this DB,
        // and isolate this lookup so it cannot blank project/date data.
        string taskJoin = taskColumns.Contains("id") && taskColumns.Contains("regid") && subtaskColumns.Contains("task")
            ? "LEFT JOIN dbo.Pms_tbl_AddTask t WITH (NOLOCK) ON t.id=s.[Task] AND t.regid=s.regid " : "";
        var descriptionParts = new List<string>();
        string subtaskDescription = FirstExistingColumn(subtaskColumns, "description", "taskdescription", "subtaskdescription", "taskdesc", "subtaskdesc", "desc");
        string taskDescription = FirstExistingColumn(taskColumns, "description", "taskdescription", "taskdesc", "desc");
        if (subtaskDescription != null) descriptionParts.Add("NULLIF(LTRIM(RTRIM(ISNULL(CONVERT(nvarchar(max),s.[" + subtaskDescription + "]),''))),'')");
        if (taskDescription != null && taskJoin.Length > 0) descriptionParts.Add("NULLIF(LTRIM(RTRIM(ISNULL(CONVERT(nvarchar(max),t.[" + taskDescription + "]),''))),'')");
        if (descriptionParts.Count > 0)
        {
            string descriptionSql = "SELECT s.id AS SubTaskId, COALESCE(" + String.Join(",", descriptionParts.ToArray()) + ", '') AS Description " +
                "FROM dbo.Pms_tbl_AddSubTask s WITH (NOLOCK) " + taskJoin +
                "WHERE s.regid=" + reg.ToString(CultureInfo.InvariantCulture) + " AND s.id IN (" + idList + ")";
            try
            {
                DataSet descriptionData = new DataAccess().GetDataSetInline(descriptionSql);
                if (descriptionData != null && descriptionData.Tables.Count > 0)
                    foreach (DataRow row in descriptionData.Tables[0].Rows)
                    {
                        int id;
                        if (Int32.TryParse(Value(row, "SubTaskId"), NumberStyles.Integer, CultureInfo.InvariantCulture, out id) && result.ContainsKey(id))
                            result[id].Description = Value(row, "Description");
                    }
            }
            catch { }
        }
        return result;
    }

    private static Dictionary<string, HashSet<string>> LoadReportTableColumns()
    {
        var result = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
        try
        {
            DataSet data = new DataAccess().GetDataSetInline("SELECT TABLE_NAME, COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='dbo' AND TABLE_NAME IN ('Pms_tbl_AddSubTask','Pms_tbl_AddTask','Pms_tbl_AddMilestone','Pms_tbl_AddProject','Pms_tbl_assigntasks')");
            if (data == null || data.Tables.Count == 0) return result;
            foreach (DataRow row in data.Tables[0].Rows)
            {
                string table = Value(row, "TABLE_NAME");
                string column = Value(row, "COLUMN_NAME");
                if (String.IsNullOrWhiteSpace(table) || String.IsNullOrWhiteSpace(column)) continue;
                if (!result.ContainsKey(table)) result[table] = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                result[table].Add(column);
            }
        }
        catch { }
        return result;
    }

    private static HashSet<string> TableColumns(Dictionary<string, HashSet<string>> columns, string table)
    {
        HashSet<string> found;
        return columns.TryGetValue(table, out found) ? found : new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    }

    private static void AddKnownColumns(HashSet<string> columns, params string[] known)
    {
        foreach (string column in known) columns.Add(column);
    }

    private static string FirstExistingColumn(HashSet<string> columns, params string[] candidates)
    {
        foreach (string candidate in candidates) if (columns.Contains(candidate)) return candidate;
        return null;
    }

    private static Dictionary<string, double> LoadSpentHoursBySubtaskAndDate(string regId, string userId, List<DataRow> rows, DateTime start, DateTime end)
    {
        var result = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);
        var ids = rows.Select(GetSubtaskId).Where(id => id > 0).Distinct().ToList();
        int reg, user;
        if (ids.Count == 0 || !Int32.TryParse(regId, out reg) || !Int32.TryParse(userId, out user)) return result;
        string idList = String.Join(",", ids.Select(id => id.ToString(CultureInfo.InvariantCulture)).ToArray());
        string from = start.Date.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
        string to = end.Date.AddDays(1).ToString("yyyyMMdd", CultureInfo.InvariantCulture);
        string sql = @"
SELECT subtaskid AS SubTaskId, CONVERT(varchar(10), CAST(StartDatetime AS date), 23) AS WorkDate,
       ISNULL(SUM(CAST(ISNULL(PausedTime,0) AS numeric(18,2))),0) AS TotalMinutes
FROM Pms_Tbl_PauseStatus WITH (NOLOCK)
WHERE status=1 AND archive=0 AND regid=" + reg.ToString(CultureInfo.InvariantCulture) + @"
  AND uid=" + user.ToString(CultureInfo.InvariantCulture) + @" AND LOWER(ISNULL(type,'subtask'))='subtask'
  AND ISNULL(subtaskid,0) IN (" + idList + @") AND ISNULL(entrytype,'') IN ('pause','stop')
  AND StartDatetime >= CONVERT(datetime,'" + from + @"',112) AND StartDatetime < CONVERT(datetime,'" + to + @"',112)
  AND ISNUMERIC(ISNULL(PausedTime,0))=1
GROUP BY subtaskid, CAST(StartDatetime AS date)";
        DataSet data = new DataAccess().GetDataSetInline(sql);
        if (data == null || data.Tables.Count == 0) return result;
        foreach (DataRow row in data.Tables[0].Rows)
        {
            int id;
            DateTime workDate;
            if (Int32.TryParse(Value(row, "SubTaskId"), out id) &&
                DateTime.TryParse(Value(row, "WorkDate"), CultureInfo.InvariantCulture, DateTimeStyles.None, out workDate))
                result[id + "|" + workDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)] = Math.Max(0, Number(row, "TotalMinutes") / 60.0);
        }
        return result;
    }

    private static object BuildIssueReport(string regId, string userId, DateTime start, DateTime end,
        string status, string completion, string entityType, string employeeSearch, string issueSearch)
    {
        int reg, user;
        if (!Int32.TryParse(regId, out reg) || !Int32.TryParse(userId, out user))
            return new { reportType = "issues", rows = new object[0], total = 0, completed = 0, completion = 0, timeEfficiency = 0 };

        string employee = user.ToString(CultureInfo.InvariantCulture);
        string pattern = "(',' + REPLACE(REPLACE(REPLACE(ISNULL(CONVERT(nvarchar(2000),{0}),''),' ',''),';',','),'|',',') + ',') LIKE '%,' + CONVERT(nvarchar(20)," + employee + ") + ',%'";
        string sql = @"
SELECT i.id AS Id, ISNULL(i.Issues,'') AS IssueName, ISNULL(i.Description,'') AS Description,
       ISNULL(CONVERT(nvarchar(2000),i.Owner),'') AS Owner, ISNULL(CONVERT(nvarchar(2000),i.team),'') AS Team,
       ISNULL(CONVERT(nvarchar(2000),i.AssignTo),'') AS AssignTo, ISNULL(ps.name,'Unknown') AS StatusName, ISNULL(ps.PrintColor,'') AS StatusColor,
       ISNULL(pcs.iscomplete,0) AS IsComplete, ISNULL(pr.name,'') AS PriorityName,
       ISNULL(CONVERT(nvarchar(100),i.IssueType),'') AS IssueType,
       ISNULL(ist.name,'') AS IssueTypeName, ISNULL(CONVERT(nvarchar(30),i.Issueon),'') AS EntityType,
       ISNULL(t.taskname,'') AS TaskName,
       i.cdate AS CreateDate, i.EndDate AS TargetDate, ISNULL(p.ProjectName,'') AS ProjectName,
       ISNULL(m.milestone,'') AS MilestoneName,
       ISNULL(CONVERT(varchar(20),m.orderno),'') +
       CASE WHEN t.orderno IS NULL THEN '' ELSE '.' + CONVERT(varchar(20),t.orderno) END +
       CASE WHEN s.orderno IS NULL THEN '' ELSE '.' + CONVERT(varchar(20),s.orderno) END AS SerialNo
FROM dbo.Pms_tbl_Addissues i WITH (NOLOCK)
LEFT JOIN dbo.Pms_tbl_IssueType ist WITH (NOLOCK) ON ist.id=i.IssueType AND ist.regid=i.regid AND ist.status=1 AND ist.archive=0
LEFT JOIN dbo.Pms_tbl_addIssueStatus ps WITH (NOLOCK) ON ps.Id=i.IssueStatus AND ps.regid=i.regid AND ps.Status=1 AND ps.Archive=0
LEFT JOIN dbo.Pms_tbl_Projectstatus pcs WITH (NOLOCK) ON pcs.id=i.IssueStatus AND pcs.regid=i.regid AND pcs.status=1 AND pcs.archive=0
LEFT JOIN dbo.Pms_tbl_Priority pr WITH (NOLOCK) ON pr.id=i.Priority AND pr.regid=i.regid AND pr.status=1 AND pr.archive=0
LEFT JOIN dbo.Pms_tbl_AddProject p WITH (NOLOCK) ON p.id=i.ProjectId AND p.regid=i.regid AND p.status=1 AND p.archive=0
LEFT JOIN dbo.Pms_tbl_AddMilestone m WITH (NOLOCK) ON m.id=i.Milestoneid AND m.regid=i.regid AND m.status=1 AND m.archive=0
LEFT JOIN dbo.Pms_tbl_AddTask t WITH (NOLOCK) ON t.id=COALESCE(NULLIF(i.taskid,0),NULLIF(i.tasklistid,0)) AND t.regid=i.regid AND t.status=1 AND t.archive=0
LEFT JOIN dbo.Pms_tbl_AddSubTask s WITH (NOLOCK) ON s.id=i.subtaskid AND s.regid=i.regid AND s.status=1 AND s.archive=0
WHERE i.regid=" + reg.ToString(CultureInfo.InvariantCulture) + @" AND i.status=1 AND i.archive=0
  AND ISNULL(i.cdate,i.StartDate) >= CONVERT(date,'" + start.ToString("yyyyMMdd", CultureInfo.InvariantCulture) + @"',112)
  AND ISNULL(i.cdate,i.StartDate) < DATEADD(day,1,CONVERT(date,'" + end.ToString("yyyyMMdd", CultureInfo.InvariantCulture) + @"',112))
  AND (TRY_CONVERT(int,i.Owner)=" + employee + " OR " + String.Format(CultureInfo.InvariantCulture, pattern, "i.Owner") +
  " OR TRY_CONVERT(int,i.AssignTo)=" + employee + " OR " + String.Format(CultureInfo.InvariantCulture, pattern, "i.AssignTo") +
  " OR " + String.Format(CultureInfo.InvariantCulture, pattern, "i.team") + ") " +
  "AND (UPPER(LTRIM(RTRIM(CONVERT(nvarchar(30),i.Issueon)))) <> 'SUBTASK' OR s.id IS NOT NULL) ";

        string safeStatus = SafeSql(status).ToUpperInvariant();
        if (!String.IsNullOrWhiteSpace(safeStatus) && safeStatus != "ALL")
            sql += "AND UPPER(LTRIM(RTRIM(ISNULL(ps.name,'Unknown'))))=N'" + safeStatus + "' ";
        // Issue Completion is no longer a report filter.
        string safeEntity = SafeSql(entityType);
        if (!String.IsNullOrWhiteSpace(safeEntity) && safeEntity != "ALL") sql += "AND UPPER(LTRIM(RTRIM(CONVERT(nvarchar(30),i.Issueon))))='" + safeEntity.ToUpperInvariant() + "' ";
        string safeSearch = SafeSql(issueSearch);
        if (!String.IsNullOrWhiteSpace(safeSearch)) sql += "AND (i.Issues LIKE '%" + safeSearch + "%' OR i.Description LIKE '%" + safeSearch + "%') ";
        sql += "ORDER BY i.cdate DESC,i.id DESC";

        DataSet data = new DataAccess().GetDataSetInline(sql);
        DataTable table = data != null && data.Tables.Count > 0 ? data.Tables[0] : new DataTable();
        Dictionary<int, string> employeeNames = LoadEmployeeNames(reg, table);
        var rows = table.Rows.Cast<DataRow>().Select(row => new IssueReportRow
        {
            issueName = Value(row, "IssueName"),
            serialNo = Value(row, "SerialNo"),
            taskName = Value(row, "TaskName"),
            description = Value(row, "Description"),
            entityType = Value(row, "EntityType"),
            priority = Value(row, "PriorityName"),
            status = Value(row, "StatusName"),
            statusColor = Value(row, "StatusColor"),
            owner = ResolveEmployeeList(Value(row, "Owner"), employeeNames),
            team = ResolveEmployeeList(Value(row, "Team"), employeeNames),
            assignTo = ResolveEmployeeList(Value(row, "AssignTo"), employeeNames),
            createDate = FormatDate(Value(row, "CreateDate")),
            targetDate = FormatDate(Value(row, "TargetDate")),
            issueType = Value(row, "IssueTypeName"),
            projectName = Value(row, "ProjectName"),
            milestoneName = Value(row, "MilestoneName"),
            isComplete = Number(row, "IsComplete") != 0
        }).ToList();
        var employeeOptions = rows.SelectMany(item => new[] { item.owner, item.assignTo }
                .Concat((item.team ?? "").Split(new[] { ',', ';', '|' }, StringSplitOptions.RemoveEmptyEntries)))
            .Select(item => (item ?? "").Trim()).Where(item => item.Length > 0)
            .Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(item => item, StringComparer.OrdinalIgnoreCase).ToList();
        string employeeFilter = (employeeSearch ?? "").Trim();
        if (employeeFilter.Length > 0)
            rows = rows.Where(item => ContainsText(item.owner, employeeFilter) || ContainsText(item.assignTo, employeeFilter) || ContainsText(item.team, employeeFilter)).ToList();
        int completed = rows.Count(item => item.isComplete);
        return new
        {
            reportType = "issues",
            fromDate = start.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            toDate = end.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            rows = rows,
            employeeOptions = employeeOptions,
            priorityOptions = LoadPriorityOptions(regId),
            statusOptions = LoadIssueStatusOptions(regId),
            total = rows.Count,
            completed = completed,
            completion = rows.Count == 0 ? 0 : Math.Round(completed * 100.0 / rows.Count),
            timeEfficiency = 0
        };
    }

    private static Dictionary<int, double> LoadSpentHours(string regId, string userId, List<DataRow> rows)
    {
        var result = new Dictionary<int, double>();
        var ids = rows.Select(GetSubtaskId).Where(id => id > 0).Distinct().ToList();
        int reg, user;
        if (ids.Count == 0 || !Int32.TryParse(regId, out reg) || !Int32.TryParse(userId, out user)) return result;
        string idList = String.Join(",", ids.Select(id => id.ToString(CultureInfo.InvariantCulture)).ToArray());
        string sql = @"
SELECT subtaskid AS SubTaskId, ISNULL(SUM(CAST(ISNULL(PausedTime,0) AS numeric(18,2))),0) AS TotalMinutes
FROM Pms_Tbl_PauseStatus WITH (NOLOCK)
WHERE status=1 AND archive=0 AND regid=" + reg.ToString(CultureInfo.InvariantCulture) + @"
  AND uid=" + user.ToString(CultureInfo.InvariantCulture) + @" AND LOWER(ISNULL(type,'subtask'))='subtask'
  AND ISNULL(subtaskid,0) IN (" + idList + @") AND ISNULL(entrytype,'') IN ('pause','stop')
  AND ISNUMERIC(ISNULL(PausedTime,0))=1 GROUP BY subtaskid";
        DataSet data = new DataAccess().GetDataSetInline(sql);
        if (data == null || data.Tables.Count == 0) return result;
        foreach (DataRow row in data.Tables[0].Rows)
        {
            int id;
            if (Int32.TryParse(Value(row, "SubTaskId"), out id) && id > 0) result[id] = Math.Max(0, Number(row, "TotalMinutes") / 60.0);
        }
        return result;
    }

    private static Dictionary<int, string> LoadTags(string regId, List<DataRow> rows)
    {
        var ids = new HashSet<int>();
        foreach (DataRow row in rows)
            foreach (string token in First(row, "TagIds", "Tags").Split(',', ';', '|'))
            { int id; if (Int32.TryParse(token.Trim(), out id) && id > 0) ids.Add(id); }
        var map = new Dictionary<int, string>();
        int reg;
        if (ids.Count == 0 || !Int32.TryParse(regId, out reg)) return map;
        string idList = String.Join(",", ids.Select(id => id.ToString(CultureInfo.InvariantCulture)).ToArray());
        string sql = "SELECT id,Tags FROM dbo.Pms_tbl_Addtags WITH (NOLOCK) WHERE regid=" + reg.ToString(CultureInfo.InvariantCulture) + " AND status=1 AND archive=0 AND id IN (" + idList + ")";
        DataSet data = new DataAccess().GetDataSetInline(sql);
        if (data == null || data.Tables.Count == 0) return map;
        foreach (DataRow row in data.Tables[0].Rows) { int id; if (Int32.TryParse(Value(row, "id"), out id)) map[id] = Value(row, "Tags"); }
        return map;
    }

    private static Dictionary<int, string> LoadEmployeeNames(int regId, DataTable table)
    {
        var ids = new HashSet<int>();
        foreach (DataRow row in table.Rows)
            foreach (string column in new[] { "Owner", "AssignTo", "Team" })
                foreach (int id in ParseIds(Value(row, column))) ids.Add(id);
        var result = new Dictionary<int, string>();
        if (ids.Count == 0) return result;
        string idList = String.Join(",", ids.Select(id => id.ToString(CultureInfo.InvariantCulture)).ToArray());
        string sql = "SELECT id,ISNULL(DisplayName,'') AS DisplayName FROM dbo.tbl_employee WITH (NOLOCK) WHERE regid=" + regId.ToString(CultureInfo.InvariantCulture) + " AND status=1 AND archive=0 AND id IN (" + idList + ")";
        DataSet data = new DataAccess().GetDataSetInline(sql);
        if (data == null || data.Tables.Count == 0) return result;
        foreach (DataRow row in data.Tables[0].Rows)
        {
            int id;
            if (Int32.TryParse(Value(row, "id"), NumberStyles.Integer, CultureInfo.InvariantCulture, out id))
                result[id] = Value(row, "DisplayName");
        }
        return result;
    }

    private static IEnumerable<int> ParseIds(string value)
    {
        foreach (string token in (value ?? "").Split(new[] { ',', ';', '|', ' ' }, StringSplitOptions.RemoveEmptyEntries))
        { int id; if (Int32.TryParse(token.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out id) && id > 0) yield return id; }
    }

    private static string ResolveEmployeeList(string source, IDictionary<int, string> names)
    {
        var result = new List<string>();
        foreach (string token in (source ?? "").Split(new[] { ',', ';', '|' }, StringSplitOptions.RemoveEmptyEntries))
        {
            int id;
            string name;
            string value = token.Trim();
            if (Int32.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out id) && names.TryGetValue(id, out name) && !String.IsNullOrWhiteSpace(name)) value = name;
            if (value.Length > 0 && !result.Contains(value, StringComparer.OrdinalIgnoreCase)) result.Add(value);
        }
        return String.Join(", ", result.ToArray());
    }

    private static bool ContainsText(string source, string value)
    { return !String.IsNullOrEmpty(source) && source.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0; }

    private static string TagNames(string source, Dictionary<int, string> map)
    {
        var names = new List<string>();
        foreach (string token in (source ?? "").Split(',', ';', '|'))
        { int id; string name; if (Int32.TryParse(token.Trim(), out id) && map.TryGetValue(id, out name)) names.Add(name); else if (!Int32.TryParse(token.Trim(), out id) && !String.IsNullOrWhiteSpace(token)) names.Add(token.Trim()); }
        return String.Join(", ", names.Distinct(StringComparer.OrdinalIgnoreCase).ToArray());
    }

    private static int GetSubtaskId(DataRow row)
    {
        foreach (string column in new[] { "SubTaskId", "subtaskid", "SubtaskId", "Id", "id" })
        { int id; if (row.Table.Columns.Contains(column) && Int32.TryParse(Value(row, column), out id) && id > 0) return id; }
        return 0;
    }
    private static bool IsComplete(DataRow row)
    {
        if (row.Table.Columns.Contains("IsComplete") && row["IsComplete"] != DBNull.Value)
        {
            bool complete;
            if (Boolean.TryParse(Convert.ToString(row["IsComplete"], CultureInfo.InvariantCulture), out complete)) return complete;
            return Number(row, "IsComplete") != 0;
        }
        string status = First(row, "StatusName", "Status");
        return status.Equals("Complete", StringComparison.OrdinalIgnoreCase) || status.Equals("Completed", StringComparison.OrdinalIgnoreCase) || status.Equals("Done", StringComparison.OrdinalIgnoreCase) || Number(row, "CompletionPercent") >= 100;
    }
    private static DateTime GetGlobalToday()
    {
        // Browser sends its current UTC offset for fallback dates only.
        // Explicit from/to dates remain date-only in every report query.
        int offsetMinutes;
        string offset = HttpContext.Current.Request.Headers["X-Pms-UtcOffsetMinutes"];
        if (Int32.TryParse(offset, NumberStyles.Integer, CultureInfo.InvariantCulture, out offsetMinutes) &&
            offsetMinutes >= -840 && offsetMinutes <= 840)
            return DateTime.UtcNow.AddMinutes(-offsetMinutes).Date;
        return DateTime.UtcNow.Date;
    }
    private static DateTime ParseDate(string value, DateTime fallback)
    { DateTime d; return DateTime.TryParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out d) || DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out d) ? d.Date : fallback.Date; }
    private static string FormatDate(string value) { DateTime d; return DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out d) ? d.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : ""; }
    private static string FirstOrFallback(string value, string fallback) { return String.IsNullOrWhiteSpace(value) ? (fallback ?? "") : value; }
    private static string FormatDuration(double hours) { int mins = (int)Math.Round(Math.Max(0, hours) * 60); return (mins / 60).ToString("00", CultureInfo.InvariantCulture) + ":" + (mins % 60).ToString("00", CultureInfo.InvariantCulture); }
    private static double DurationHours(string value) { TimeSpan d; return TimeSpan.TryParse(value, CultureInfo.InvariantCulture, out d) ? d.TotalHours : 0; }
    private static double Number(DataRow row, string column) { double n; return Double.TryParse(Value(row, column), NumberStyles.Any, CultureInfo.InvariantCulture, out n) ? n : 0; }
    private static string Value(DataRow row, string column) { return row.Table.Columns.Contains(column) && row[column] != DBNull.Value ? Convert.ToString(row[column], CultureInfo.InvariantCulture) : ""; }
    private static string First(DataRow row, params string[] columns) { foreach (string column in columns) if (row.Table.Columns.Contains(column) && row[column] != DBNull.Value && !String.IsNullOrWhiteSpace(Convert.ToString(row[column], CultureInfo.InvariantCulture))) return Convert.ToString(row[column], CultureInfo.InvariantCulture); return ""; }
    private static string SafeSql(string value) { return (value ?? "").Replace("'", "''").Trim(); }

    public sealed class IssueReportRow
    {
        public string issueName { get; set; }
        public string serialNo { get; set; }
        public string taskName { get; set; }
        public string description { get; set; }
        public string entityType { get; set; }
        public string priority { get; set; }
        public string status { get; set; }
        public string statusColor { get; set; }
        public string owner { get; set; }
        public string team { get; set; }
        public string assignTo { get; set; }
        public string createDate { get; set; }
        public string targetDate { get; set; }
        public string issueType { get; set; }
        public string projectName { get; set; }
        public string milestoneName { get; set; }
        public bool isComplete { get; set; }
    }

    public sealed class HoursReportRow
    {
        public int subtaskId { get; set; }
        public string projectName { get; set; }
        public string taskName { get; set; }
        public string priority { get; set; }
        public string status { get; set; }
        public List<string> dailyActual { get; set; }
        public double assignedHours { get; set; }
        public double actualHours { get; set; }
        public string assignedText { get; set; }
        public string actualText { get; set; }
    }

    private sealed class TaskReportMetadata
    {
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string AssignmentEnd { get; set; }
    }
}
