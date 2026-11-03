using System;
using System.Web;
using System.Collections.Generic;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Script.Services;
using CommonClass;
using System.Data;
using System.IO;
using System.Linq;
using System.Globalization;
public partial class mytaskuser : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    // Date-only request contract. No timezone conversion is performed here.
    private static SqlParameter CreateTaskDateParameter(string name, string value)
    {
        if (String.IsNullOrWhiteSpace(value))
            return new SqlParameter(name, SqlDbType.Date) { Value = DBNull.Value };

        DateTime parsed;
        string[] formats = { "MM/dd/yyyy", "M/d/yyyy", "yyyy-MM-dd" };
        if (!DateTime.TryParseExact(value.Trim(), formats,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed))
            throw new ArgumentException("Invalid " + name +
                ". Use MM/dd/yyyy or yyyy-MM-dd.");

        return new SqlParameter(name, SqlDbType.Date) { Value = parsed.Date };
    }

    private static Dictionary<string, int[]> LoadTaskTimerFlags(DataTable tasks)
    {
        try
        {
            string userId = HttpContext.Current.Request.Cookies["user"]["user_id"];
            if (HttpContext.Current.Request.QueryString["empid"] != null)
                userId = new CommonFunction().Decrypt(HttpContext.Current.Request.QueryString["empid"]).ToString();
            int numericUserId;
            if (!Int32.TryParse(userId, out numericUserId)) return null;
            Dictionary<string, HashSet<int>> ids = new Dictionary<string, HashSet<int>>();
            ids["subtask"] = new HashSet<int>();
            ids["task"] = new HashSet<int>();
            ids["milestone"] = new HashSet<int>();
            Dictionary<string, int[]> result = new Dictionary<string, int[]>();
            foreach (DataRow row in tasks.Rows)
            {
                string type = Convert.ToString(row["tasktype"]).ToLowerInvariant();
                int id;
                if (!ids.ContainsKey(type) || !Int32.TryParse(Convert.ToString(row["id"]), out id))
                    continue;
                ids[type].Add(id);
                result[type + "|" + id.ToString(CultureInfo.InvariantCulture)] = new int[3];
            }
            DataAccess da = new DataAccess();
            foreach (KeyValuePair<string, HashSet<int>> group in ids)
            {
                string column = group.Key == "task" ? "taskid" :
                    group.Key == "milestone" ? "milestoneid" : "subtaskid";
                List<int> values = group.Value.ToList();
                // Bound SQL size while replacing three queries per task.
                for (int offset = 0; offset < values.Count; offset += 500)
                {
                    string idList = String.Join(",", values.Skip(offset).Take(500)
                        .Select(x => x.ToString(CultureInfo.InvariantCulture)));
                    string sql = "select " + column + @" as EntityId,
                        max(case when isnull(isend,0)=0 then 1 else 0 end) as IsRunning,
                        max(case when isnull(isend,0)=0 and isnull(IsPaused,0)=1 then 1 else 0 end) as IsPaused,
                        max(case when isnull(isend,0)=1 then 1 else 0 end) as IsEnded
                        from Pms_tbl_Timerstatus with(nolock)
                        where status=1 and archive=0 and uid=" +
                        numericUserId.ToString(CultureInfo.InvariantCulture) +
                        " and type='" + group.Key + "' and " + column +
                        " in (" + idList + ") group by " + column;
                    DataSet ds = da.GetDataSetInline(sql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        string key = group.Key + "|" + Convert.ToInt32(row["EntityId"])
                            .ToString(CultureInfo.InvariantCulture);
                        result[key] = new int[] { Convert.ToInt32(row["IsRunning"]),
                            Convert.ToInt32(row["IsPaused"]), Convert.ToInt32(row["IsEnded"]) };
                    }
                }
            }
            return result;
        }
        catch (Exception ex)
        {
            SaveLogText("LoadTaskTimerFlagsEX-" + ex.Message);
            return null; // Preserve existing per-task queries if bulk read fails.
        }
    }

    private static readonly object TaskLoadingReadsKey = new object();

    // Enabled only by read-only task-list loading methods.
    // Bounded and request-local: no cached values cross users or refreshes.
    private static DataSet ReadTaskLoadingData(DataAccess da, string sql)
    {
        HttpContext context = HttpContext.Current;
        Dictionary<string, DataSet> reads = context == null ? null :
            context.Items[TaskLoadingReadsKey] as Dictionary<string, DataSet>;
        if (reads == null) return da.GetDataSetInline(sql);
        DataSet data;
        if (reads.TryGetValue(sql, out data)) return data;
        data = da.GetDataSetInline(sql);
        if (data != null && data.Tables.Count > 0)
        {
            // Keep memory bounded when large lists are requested.
            if (reads.Count >= 64) reads.Clear();
            reads[sql] = data;
        }
        return data;
    }

    private static void ReportTaskLoadingTiming(long databaseMilliseconds, long processingMilliseconds)
    {
        try
        {
            HttpContext context = HttpContext.Current;
            if (context == null) return;
            context.Response.AppendHeader("Server-Timing",
                "task_sp;dur=" + databaseMilliseconds.ToString(CultureInfo.InvariantCulture) +
                ";desc=\"Task stored procedure\", task_processing;dur=" +
                processingMilliseconds.ToString(CultureInfo.InvariantCulture) +
                ";desc=\"Task row processing\"");
        }
        catch
        {
            // Timing is diagnostic only; it must never interrupt task loading.
        }
    }

    private static object ParseDateOnly(object value)
    {
        if (value == null) return DBNull.Value;

        if (value is DateTime)
            return (DateTime)value;

        string s = value.ToString().Trim();
        if (string.IsNullOrWhiteSpace(s)) return DBNull.Value;

        DateTime dt;
        if (DateTime.TryParse(s, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out dt))
            return dt;
        if (DateTime.TryParse(s, out dt))
            return dt;

        return DBNull.Value;
    }
    private static string GetDateOnlyString(object value)
    {
        if (value == null || value == DBNull.Value)
            return null;

        DateTime dateValue;

        if (value is DateTime)
        {
            dateValue = ((DateTime)value).Date;
        }
        else if (!DateTime.TryParse(
                     Convert.ToString(value, CultureInfo.InvariantCulture),
                     CultureInfo.InvariantCulture,
                     DateTimeStyles.AllowWhiteSpaces,
                     out dateValue))
        {
            return null;
        }

        // Universal date-only response contract:
        // return yyyy-MM-dd text, never an ASP.NET /Date(milliseconds)/
        // timestamp. Browsers in India, Dubai, USA, etc. receive the same
        // calendar date and perform no timezone conversion.
        return dateValue.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
    }
    private static bool IncludeTaskForListType(
        object targetDateValue,
        string listType,
        DateTime today,
        bool isCompleted)
    {
        string normalizedType = (listType ?? "").Trim().ToLowerInvariant();
        if (normalizedType == "" || normalizedType == "all")
            return true;
        DateTime targetDate = DateTime.MinValue;
        bool hasTargetDate = targetDateValue != null
            && targetDateValue != DBNull.Value
            && DateTime.TryParse(Convert.ToString(targetDateValue), out targetDate);
        
        if (normalizedType == "delayedtask")
        {
            // The existing SP continues to apply the selected assignment-date
            // range. From those rows, only incomplete tasks whose TargetDate
            // has passed belong to Delayed Task.
            return !isCompleted
                && hasTargetDate
                && targetDate.Date < today.Date;
        }
        else if (normalizedType == "mytask")
        {
            // Keep My Task separate from Delayed Task. A missing TargetDate is
            // retained here because it cannot be classified as delayed.
            if(isCompleted)
            return true;
            else
             return !hasTargetDate || targetDate.Date >= today.Date;
        }


        // Do not expose every task when an unexpected list type is received.
        return false;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<subtasklist_count> bindtaskdata_count(string fromdate = "", string todate = "")
    {
        // Validate before the existing catch blocks so invalid input is reported,
        // rather than silently returning an empty task list.
        SqlParameter startDateParameter = CreateTaskDateParameter("@startdate", fromdate);
        SqlParameter endDateParameter = CreateTaskDateParameter("@enddate", todate);

        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        int monthid = Convert.ToDateTime(da.getcdate()).Month;

        int yearid = Convert.ToDateTime(da.getcdate()).Year;
        int lastday = DateTime.DaysInMonth(yearid, monthid);
        string ddlsearch = "0";
        string strfromdate = fromdate;
        string strtodate = todate;
        //string strfromdate = da.getcdate();
        //string strtodate = da.getcdate();



        //DateTime baseDate = Convert.ToDateTime(da.getcdate());
        //DateTime thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
        //DateTime thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
        //if (ddlsearch == "0")
        //{
        //    //strfromdate = Request.Cookies["fromdate"]["id"].ToString();
        //    strfromdate = "01/01/2025";
        //    strtodate = da.getcdate();
        //}
        //else if (ddlsearch == "1")
        //{
        //    strfromdate = da.getcdate();
        //    strtodate = da.getcdate();
        //}
        //else if (ddlsearch == "2")
        //{
        //    strfromdate = thisWeekStart.ToShortDateString();
        //    strtodate = thisWeekEnd.ToShortDateString();
        //}
        //else if (ddlsearch == "3")
        //{
        //    strfromdate = monthid + "/01/" + yearid;
        //    strtodate = monthid + "/" + lastday + "/" + yearid;
        //}

        //else if (ddlsearch == "4")
        //{
        //    DateTime[] dtReturn = DatesOfQuarter();


        //    strfromdate = (dtReturn[0]).ToShortDateString(); ;
        //    strtodate = (dtReturn[1]).ToShortDateString();
        //}
        //else if (ddlsearch == "5")
        //{

        //    strfromdate = "01/01/" + yearid;
        //    strtodate = "12/31/" + yearid;
        //}
        //else if (ddlsearch == "10")
        //{

        //    strfromdate = "01/01/" + (yearid - 1);
        //    strtodate = "12/31/" + (yearid - 1);
        //}
        //else if (ddlsearch == "6")
        //{
        //    strfromdate = Convert.ToDateTime(da.getcdate()).AddDays(-1).ToShortDateString();
        //    strtodate = Convert.ToDateTime(da.getcdate()).AddDays(-1).ToShortDateString();
        //}
        //else if (ddlsearch == "7")
        //{
        //    DateTime previousWeekStart = thisWeekStart.AddDays(-7);
        //    DateTime previousWeekEnd = thisWeekStart.AddDays(-1);

        //    strfromdate = previousWeekStart.ToShortDateString();
        //    strtodate = previousWeekEnd.ToShortDateString();


        //}

        //else if (ddlsearch == "8")
        //{

        //    strfromdate = monthid + "/01/" + yearid;
        //    strfromdate = Convert.ToDateTime(strfromdate).AddMonths(-1).ToShortDateString();
        //    int prvyearid = Convert.ToDateTime(strfromdate).Year;
        //    int prvmonthid = Convert.ToDateTime(strfromdate).Month;
        //    int lastdayprv = DateTime.DaysInMonth(prvyearid, prvmonthid);

        //    strtodate = prvmonthid + "/" + lastdayprv + "/" + prvyearid;
        //    // strtodate = previousWeekEnd.ToShortDateString();


        //}
        //else if (ddlsearch== "9")
        //{

        //    DateTime[] dtReturn = DatesOfQuarter();


        //    DateTime strfromdateprvq = (dtReturn[0].AddDays(-1));
        //    DateTime[] dtReturnprv = DatesOfQuarterprv(strfromdateprvq);
        //    strfromdate = (dtReturnprv[0]).ToShortDateString(); ;
        //    strtodate = (dtReturnprv[1]).ToShortDateString();

        //}
        List<subtasklist_count> _subtasklist = new List<subtasklist_count>();
        try
        {
           // if (string.IsNullOrEmpty(RecordId))
               // RecordId = "0";

            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null)
                return _subtasklist;

            string regid = regCookie["id"];
            string userid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();
            if (HttpContext.Current.Request.QueryString["empid"] != null)
            {
                userid = cmf.Decrypt(HttpContext.Current.Request.QueryString["empid"]).ToString();
               
            }
            int chkadmin = 0;
            if (userid!=string.Empty&&userid!="")
            {
                chkadmin = checkadmin(userid);
            }
            else
            {
                chkadmin = 0;
            }
            //string procname = "pms_GetSubtaskStatusCount_user";
            string procname = "pms_GetSubtaskStatusCount_user_view";
            SqlParameter[] prm1 = new SqlParameter[8];
            if (chkadmin == 1)
            {
                //prm1 = new SqlParameter[7];
                //procname = "pms_GetSubtaskStatusCount_admin";
                procname = "pms_GetSubtaskStatusCount_admin_view";
            }
            prm1[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm1[1] = new SqlParameter("@projectid", '%');
            prm1[2] = new SqlParameter("@milestoneid", '%');
            prm1[3] = new SqlParameter("@taskid", '%');
            prm1[4] = new SqlParameter("@ondate", DBNull.Value);
            prm1[5] = startDateParameter;
            prm1[6] = endDateParameter;
            if (chkadmin == 0)
            {
                prm1[7] = new SqlParameter("@userid", userid);

            }
            else
            {
            prm1[7] = new SqlParameter("@userid", "%");
            }
            string strprm = "";
            //SaveLogText("Astrprmuser1="+ prm1[0].Value+"," + prm1[1].Value + "," + prm1[2].Value + "," + prm1[3].Value + "," + prm1[4].Value + "," + prm1[5].Value + "," + prm1[6].Value);//+ "," + prm1[7].Value
            DataSet dsL= da.GetDataSet(procname, prm1);
           // SaveLogText("Astrprmusercount=" + dsL.Tables[0].Rows.Count);
            foreach (DataRow row in dsL.Tables[0].Rows)
            {

                _subtasklist.Add(new subtasklist_count
                {

                    statusid = row["statusid"].ToString(),
                    total = row["total"].ToString(),
                    statusname = row["statusname"].ToString(),
                    statuscolor = row["statuscolor"].ToString(),
                    statusicon = row["statusicon"].ToString(),


                });
            }


        }
        catch (Exception ex)
        {
            SaveLogText("Astrprmuser2=" + ex.Message);
            return new List<subtasklist_count>();
        }

        return _subtasklist;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<subtasklist> bindsubtasklist(string fromdate = "", string todate = "", string listtype = "")
    {
        listtype = (listtype ?? "").Trim().ToLowerInvariant();
        // Validate before the existing catch blocks so invalid input is reported,
        // rather than silently returning an empty task list.
        SqlParameter startDateParameter = CreateTaskDateParameter("@startdate", fromdate);
        SqlParameter endDateParameter = CreateTaskDateParameter("@enddate", todate);

        CommonFunction cmf = new CommonFunction();
        DataAccess da = new DataAccess();
        List<subtasklist> obj = new List<subtasklist>();
        int monthid = Convert.ToDateTime(da.getcdate()).Month;

        int yearid = Convert.ToDateTime(da.getcdate()).Year;
        int lastday = DateTime.DaysInMonth(yearid, monthid);
        string ddlsearch = "0";
        string strfromdate = fromdate;
        string strtodate = todate;
        DateTime today = Convert.ToDateTime(da.getcdate()).Date;


        var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
        if (regCookie == null)
            return obj;
        string regid = regCookie["id"];
        int chkadmin = 0;
        string procname = "pms_GetSubtaskList_user_view2";
        if (listtype.ToLower() == "delayedtask")
        {
            procname = "pms_GetSubtaskList_user_view2_delayed"; 
        }
        //string procname = "pms_GetSubtaskList_user";
        SqlParameter[] prm1 = new SqlParameter[8];
        string userid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();
        string empid = "";
        if (HttpContext.Current.Request.QueryString["empid"] != null)
        {
            userid = cmf.Decrypt(HttpContext.Current.Request.QueryString["empid"]).ToString();
            empid = userid;
        }
        if (userid != string.Empty && userid != "")
        {
            chkadmin = checkadmin(userid);
        }
        else
        {
            chkadmin = 0;
        }
        if (chkadmin==1)
        {
            if(listtype.ToLower() == "mytask")
            {
                chkadmin = 0;
            }
        }
        //if (chkadmin==1)
        //{
        //    //prm1 = new SqlParameter[7];
        //    //procname = "pms_GetSubtaskList_admin";
        //    procname = "pms_GetSubtaskList_admin_view";
        //}
        try
        {
                //string str2 = "select * from Pms_tbl_Addsubtask where regid='" + regid + "' and task='" + RecordId + "' and status=1 and archive=0 order by isnull(orderno,0)";
                
                prm1[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);                
                prm1[1] = new SqlParameter("@projectid", '%');
                prm1[2] = new SqlParameter("@milestoneid", '%');
                prm1[3] = new SqlParameter("@taskid", '%');
                prm1[4] = new SqlParameter("@ondate",DBNull.Value);
                prm1[5] = startDateParameter;
                prm1[6] = endDateParameter;
                if (chkadmin == 0)
                {
                    prm1[7] = new SqlParameter("@userid", userid);

                }
                else
                {
                    prm1[7] = new SqlParameter("@userid", "%");
                }
               
                string strprm = "";
                SaveLogText("strprmuser=" + procname+">"+ prm1[0].Value + "," + prm1[1].Value + "," + prm1[2].Value + "," + prm1[3].Value + "," + prm1[4].Value + "," + prm1[5].Value + "," + prm1[6].Value + "," + prm1[7].Value);
                var databaseWatch = System.Diagnostics.Stopwatch.StartNew();
                DataSet dsL = da.GetDataSet(procname, prm1);
                databaseWatch.Stop();
                var processingWatch = System.Diagnostics.Stopwatch.StartNew();
            HttpContext.Current.Items[TaskLoadingReadsKey] = new Dictionary<string, DataSet>(StringComparer.Ordinal);
            Dictionary<string, int[]> timerFlags = LoadTaskTimerFlags(dsL.Tables[0]);
            Dictionary<string, int[]> hierarchyOrders = LoadTaskHierarchyOrders(dsL.Tables[0], regid);
            Dictionary<string, List<taglist>> loadedTags = LoadTaskTags(dsL.Tables[0], regid, false);

                //SaveLogText("strprmuserC=" + dsL.Tables[0].Rows.Count);
                 Dictionary<string, List<teamlist>> teamCache = LoadTaskTeams(dsL.Tables[0], regid)
                ?? new Dictionary<string, List<teamlist>>();
            Dictionary<string, int> completedStatusCache = new Dictionary<string, int>();
            foreach (DataRow row in dsL.Tables[0].Rows)
                {
                    string statusKey = row["statusid"].ToString();
                    int iscompletestatus;
                    if (!completedStatusCache.TryGetValue(statusKey, out iscompletestatus))
                    {
                        iscompletestatus = checkcompletestatus(statusKey);
                        completedStatusCache[statusKey] = iscompletestatus;
                    }
                bool includeForListType = IncludeTaskForListType(
                        row["TargetDate"],
                        listtype,
                        today,
                        iscompletestatus == 1);
                SaveLogText("isCompleted-"+ procname + "-" + iscompletestatus.ToString() + "/" + row["id"].ToString() + "");

                if (!includeForListType)
                        continue;
                    
                    string teamKey = row["team"].ToString();
                    List<teamlist> newteamlist;
                    if (!teamCache.TryGetValue(teamKey, out newteamlist))
                    {
                        newteamlist = getemployeelist<teamlist>(teamKey);
                        teamCache[teamKey] = newteamlist;
                    }
                    //List<taglist> _tags = GetLoadedTaskTags(loadedTags, row["id"].ToString(),"subtask");
                    List<taglist> _tags = GetLoadedTaskTags(loadedTags, row["id"].ToString(), row["tasktype"].ToString());
                    
                    double pending_rate = 100;
                    double complete_rate = 0;
                   
                #region find issues
                //complete_rate = getcompletetime(row["id"].ToString(), row["tasktype"].ToString());
                complete_rate = get_completionstatus(row["id"].ToString(), row["tasktype"].ToString());
                if(complete_rate>0&& complete_rate<=100)
                {
                    pending_rate = 100 - complete_rate;
                }
                else if (complete_rate > 0 && complete_rate>100)
                {
                    complete_rate = 100;
                    pending_rate =0;
                }
                if(iscompletestatus==1)
                {
                    complete_rate = 100;
                    pending_rate = 0;
                }
                string spentexceedtimes = "00:00:00";
                int isexceeds = 0;
                string execcedtime = compare_timespam(row["id"].ToString(),row["tasktype"].ToString());
                try
                {
                    string[] ar_excced = execcedtime.Split('|');
                    isexceeds = Convert.ToInt32(ar_excced[0]);
                    spentexceedtimes = ar_excced[1];
                }
                catch { }
                string tasktypes = "subtask";
                try
                {
                    tasktypes = row["tasktype"].ToString();
                }
                catch { }
                #endregion
                int[] hierarchyOrder = GetLoadedTaskHierarchyOrders(hierarchyOrders, da, regid, row["id"].ToString(), tasktypes);
                //tasktype= row["tasktype"].ToString(),
                int[] rowTimerFlags = null;
                string timerFlagKey = Convert.ToString(row["tasktype"]).ToLowerInvariant() +
                    "|" + Convert.ToString(row["id"], CultureInfo.InvariantCulture);
                if (timerFlags != null) timerFlags.TryGetValue(timerFlagKey, out rowTimerFlags);
                string currentTimerText = gettimespam(row["id"].ToString(), row["tasktype"].ToString());
                obj.Add(new subtasklist
                {

                    id = row["id"] as int?,
                    tasktype = tasktypes,
                    selected =false,
                    isrun = rowTimerFlags != null ? rowTimerFlags[0] : checkactive(row["id"].ToString(),row["tasktype"].ToString()),
                    ispause = rowTimerFlags != null ? rowTimerFlags[1] : checkpause(row["id"].ToString(), row["tasktype"].ToString()),
                    isend = rowTimerFlags != null ? rowTimerFlags[2] : checkend(row["id"].ToString(), row["tasktype"].ToString()),
                    ProjectId = row["ProjectId"] as int?,
                    Powner = row["powner"].ToString(),
                    ProjectName = row["ProjectName"].ToString(),
                    Milestoneid = row["Milestoneid"] as int?,
                    Milestone = row["Milestone"].ToString(),
                    Task = row["task"] as int?,
                    Taskname = row["taskname"].ToString(),
                    Subtaskname = row["Subtaskname"].ToString(),
                    SubTaskDescription = row["SubTaskDescription"].ToString(),
                    P_Status = row["statusid"] as int?,
                    status_name = row["statusname"].ToString(),
                    status_color = row["statuscolor"].ToString(),
                    Priority = row["Priority"] as int?,
                    priority_name = row["Priorityname"].ToString(),
                    priority_color = row["Prioritycolor"].ToString(),
                    Startdate = GetNullableDate(row["Startdate"]),
                    Targetdate = GetNullableDate(row["TargetDate"]),
                    TaskDuration = row["TaskDuration"].ToString(),
                    MilestoneOrderNo = hierarchyOrder[0],
                    TaskOrderNo = hierarchyOrder[1],
                    SubTaskOrderNo = hierarchyOrder[2],
                    teams = newteamlist,
                    timerText = currentTimerText,
                    elapsedMilliseconds = ConvertTimerToMilliseconds(currentTimerText),
                    complete_value = complete_rate.ToString(),
                    pending_value = pending_rate.ToString(),
                    timerText_spent = gettimespam_spent(row["id"].ToString(), row["tasktype"].ToString()),
                    spentexceedtime = spentexceedtimes,
                    isexceed = isexceeds,
                    tags = _tags,


                });
                }




            processingWatch.Stop();
            ReportTaskLoadingTiming(databaseWatch.ElapsedMilliseconds, processingWatch.ElapsedMilliseconds);

        }
        catch (Exception ex)
        {
            SaveLogText("chkadminsubtaskEX-" + ex.Message);
            return new List<subtasklist>();
        }


        return obj;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<subtasklist> bindsubtasklist_byfilter()
    {
        CommonFunction cmf = new CommonFunction();
        DataAccess da = new DataAccess();
        List<subtasklist> obj = new List<subtasklist>();
        int monthid = Convert.ToDateTime(da.getcdate()).Month;

        int yearid = Convert.ToDateTime(da.getcdate()).Year;
        int lastday = DateTime.DaysInMonth(yearid, monthid);
        string ddlsearch = "0";
        string strfromdate = da.getcdate();
        string strtodate = da.getcdate();



        DateTime baseDate = Convert.ToDateTime(da.getcdate());
        DateTime thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
        DateTime thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
        if (ddlsearch == "0")
        {
            //strfromdate = Request.Cookies["fromdate"]["id"].ToString();
            strfromdate = "01/01/2025";
            strtodate = da.getcdate();
        }
        else if (ddlsearch == "1")
        {
            strfromdate = da.getcdate();
            strtodate = da.getcdate();
        }
        else if (ddlsearch == "2")
        {
            strfromdate = thisWeekStart.ToShortDateString();
            strtodate = thisWeekEnd.ToShortDateString();
        }
        else if (ddlsearch == "3")
        {
            strfromdate = monthid + "/01/" + yearid;
            strtodate = monthid + "/" + lastday + "/" + yearid;
        }

        else if (ddlsearch == "4")
        {
            DateTime[] dtReturn = DatesOfQuarter();


            strfromdate = (dtReturn[0]).ToShortDateString(); ;
            strtodate = (dtReturn[1]).ToShortDateString();
        }
        else if (ddlsearch == "5")
        {

            strfromdate = "01/01/" + yearid;
            strtodate = "12/31/" + yearid;
        }
        else if (ddlsearch == "10")
        {

            strfromdate = "01/01/" + (yearid - 1);
            strtodate = "12/31/" + (yearid - 1);
        }
        else if (ddlsearch == "6")
        {
            strfromdate = Convert.ToDateTime(da.getcdate()).AddDays(-1).ToShortDateString();
            strtodate = Convert.ToDateTime(da.getcdate()).AddDays(-1).ToShortDateString();
        }
        else if (ddlsearch == "7")
        {
            DateTime previousWeekStart = thisWeekStart.AddDays(-7);
            DateTime previousWeekEnd = thisWeekStart.AddDays(-1);

            strfromdate = previousWeekStart.ToShortDateString();
            strtodate = previousWeekEnd.ToShortDateString();


        }

        else if (ddlsearch == "8")
        {

            strfromdate = monthid + "/01/" + yearid;
            strfromdate = Convert.ToDateTime(strfromdate).AddMonths(-1).ToShortDateString();
            int prvyearid = Convert.ToDateTime(strfromdate).Year;
            int prvmonthid = Convert.ToDateTime(strfromdate).Month;
            int lastdayprv = DateTime.DaysInMonth(prvyearid, prvmonthid);

            strtodate = prvmonthid + "/" + lastdayprv + "/" + prvyearid;
            // strtodate = previousWeekEnd.ToShortDateString();


        }
        else if (ddlsearch == "9")
        {

            DateTime[] dtReturn = DatesOfQuarter();


            DateTime strfromdateprvq = (dtReturn[0].AddDays(-1));
            DateTime[] dtReturnprv = DatesOfQuarterprv(strfromdateprvq);
            strfromdate = (dtReturnprv[0]).ToShortDateString(); ;
            strtodate = (dtReturnprv[1]).ToShortDateString();

        }



        var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
        if (regCookie == null)
            return obj;
        string regid = regCookie["id"];
        int chkadmin = 0;
        string procname = "pms_GetSubtaskList_user_view";
        SqlParameter[] prm1 = new SqlParameter[8];
        string userid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();
        if (HttpContext.Current.Request.QueryString["empid"] != null)
        {
            userid = cmf.Decrypt(HttpContext.Current.Request.QueryString["empid"]).ToString();

        }
        if (userid != string.Empty && userid != "")
        {
            chkadmin = checkadmin(userid);
        }
        else
        {
            chkadmin = 0;
        }
        if (chkadmin == 1)
        {
            prm1 = new SqlParameter[7];
            procname = "pms_GetSubtaskList_admin_view";
        }
        try
        {
            //string str2 = "select * from Pms_tbl_Addsubtask where regid='" + regid + "' and task='" + RecordId + "' and status=1 and archive=0 order by isnull(orderno,0)";

            prm1[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm1[1] = new SqlParameter("@projectid", '%');
            prm1[2] = new SqlParameter("@milestoneid", '%');
            prm1[3] = new SqlParameter("@taskid", '%');
            prm1[4] = new SqlParameter("@ondate", DBNull.Value);
            prm1[5] = new SqlParameter("@startdate", strfromdate);
            prm1[6] = new SqlParameter("@enddate", strtodate);
            if (chkadmin == 0)
            {
                prm1[7] = new SqlParameter("@userid", userid);

            }
            string strprm = "";
            SaveLogText("strprmuser=" + prm1[0].Value + "," + prm1[1].Value + "," + prm1[2].Value + "," + prm1[3].Value + "," + prm1[4].Value + "," + prm1[5].Value + "," + prm1[6].Value);
            var databaseWatch = System.Diagnostics.Stopwatch.StartNew();
                DataSet dsL = da.GetDataSet(procname, prm1);
                databaseWatch.Stop();
                var processingWatch = System.Diagnostics.Stopwatch.StartNew();
            HttpContext.Current.Items[TaskLoadingReadsKey] = new Dictionary<string, DataSet>(StringComparer.Ordinal);
            Dictionary<string, int[]> timerFlags = LoadTaskTimerFlags(dsL.Tables[0]);
            Dictionary<string, int[]> hierarchyOrders = LoadTaskHierarchyOrders(dsL.Tables[0], regid);
            Dictionary<string, List<taglist>> loadedTags = LoadTaskTags(dsL.Tables[0], regid, true);

            Dictionary<string, List<teamlist>> teamCache = LoadTaskTeams(dsL.Tables[0], regid)
                ?? new Dictionary<string, List<teamlist>>();
            foreach (DataRow row in dsL.Tables[0].Rows)
            {

                string teamKey = row["team"].ToString();
                    List<teamlist> newteamlist;
                    if (!teamCache.TryGetValue(teamKey, out newteamlist))
                    {
                        newteamlist = getemployeelist<teamlist>(teamKey);
                        teamCache[teamKey] = newteamlist;
                    }
                List<taglist> _tags = GetLoadedTaskTags(loadedTags, row["id"].ToString(), "subtask");

                double pending_rate = 100;
                double complete_rate = 0;

                #region find issues
                //complete_rate = getcompletetime(row["id"].ToString(), "subtask");
                complete_rate = get_completionstatus(row["id"].ToString(), "subtask");
                if (complete_rate > 0 && complete_rate <= 100)
                {
                    pending_rate = 100 - complete_rate;
                }
                else if (complete_rate > 0 && complete_rate > 100)
                {
                    complete_rate = 100;
                    pending_rate = 0;
                }
                string spentexceedtimes = "00:00:00";
                int isexceeds = 0;
                string execcedtime = compare_timespam(row["id"].ToString(), "subtask");
                try
                {
                    string[] ar_excced = execcedtime.Split('|');
                    isexceeds = Convert.ToInt32(ar_excced[0]);
                    spentexceedtimes = ar_excced[1];
                }
                catch { }
                #endregion
                string hierarchyTaskType = "subtask";
                try
                {
                    hierarchyTaskType = row["tasktype"].ToString();
                }
                catch { }
                int[] hierarchyOrder = GetLoadedTaskHierarchyOrders(hierarchyOrders, da, regid, row["id"].ToString(), hierarchyTaskType);
                int[] rowTimerFlags = null;
                string timerFlagKey = Convert.ToString(row["tasktype"]).ToLowerInvariant() +
                    "|" + Convert.ToString(row["id"], CultureInfo.InvariantCulture);
                if (timerFlags != null) timerFlags.TryGetValue(timerFlagKey, out rowTimerFlags);
                string currentTimerText = gettimespam(row["id"].ToString(), row["tasktype"].ToString());
                obj.Add(new subtasklist
                {

                    id = row["id"] as int?,
                    isrun = rowTimerFlags != null ? rowTimerFlags[0] : checkactive(row["id"].ToString(), row["tasktype"].ToString()),
                    ispause = rowTimerFlags != null ? rowTimerFlags[1] : checkpause(row["id"].ToString(), row["tasktype"].ToString()),
                    isend = rowTimerFlags != null ? rowTimerFlags[2] : checkend(row["id"].ToString(), row["tasktype"].ToString()),
                    ProjectId = row["ProjectId"] as int?,
                    Powner = row["Powner"].ToString(),
                    ProjectName = row["ProjectName"].ToString(),
                    Milestoneid = row["Milestoneid"] as int?,
                    Milestone = row["Milestone"].ToString(),
                    Task = row["task"] as int?,
                    Taskname = row["taskname"].ToString(),
                    Subtaskname = row["Subtaskname"].ToString(),
                    SubTaskDescription = row["SubTaskDescription"].ToString(),
                    P_Status = row["statusid"] as int?,
                    status_name = row["statusname"].ToString(),
                    status_color = row["statuscolor"].ToString(),
                    Priority = row["Priority"] as int?,
                    priority_name = row["Priorityname"].ToString(),
                    priority_color = row["Prioritycolor"].ToString(),
                    Startdate = GetNullableDate(row["Startdate"]),
                    Targetdate = GetNullableDate(row["TargetDate"]),
                    TaskDuration = row["TaskDuration"].ToString(),
                    MilestoneOrderNo = hierarchyOrder[0],
                    TaskOrderNo = hierarchyOrder[1],
                    SubTaskOrderNo = hierarchyOrder[2],
                    teams = newteamlist,
                    timerText = currentTimerText,
                    elapsedMilliseconds = ConvertTimerToMilliseconds(currentTimerText),
                    complete_value = complete_rate.ToString(),
                    pending_value = pending_rate.ToString(),
                    timerText_spent = gettimespam_spent(row["id"].ToString(),row["tasktype"].ToString()),
                    spentexceedtime = spentexceedtimes,
                    isexceed = isexceeds,
                    tags = _tags,


                });
            }




            processingWatch.Stop();
            ReportTaskLoadingTiming(databaseWatch.ElapsedMilliseconds, processingWatch.ElapsedMilliseconds);

        }
        catch (Exception ex)
        {
            SaveLogText("chkadminsubtaskEX-" + ex.Message);
            return new List<subtasklist>();
        }


        return obj;
    }

    // =========================================================
    // TASK DISPLAY SERIAL NUMBER (display only)
    // Reads existing orderno values from Milestone -> Task -> Subtask.
    // No save/update/status/timer behavior is changed.
    // =========================================================
    private static Dictionary<string, List<teamlist>> LoadTaskTeams(DataTable tasks, string regidValue)
    {
        try
        {
            int regid;
            if (!Int32.TryParse(regidValue, out regid)) return null;
            Dictionary<string, HashSet<int>> groups = new Dictionary<string, HashSet<int>>();
            HashSet<int> allIds = new HashSet<int>();
            foreach (DataRow row in tasks.Rows)
            {
                string raw = Convert.ToString(row["team"]);
                if (groups.ContainsKey(raw)) continue;
                HashSet<int> ids = new HashSet<int>();
                bool valid = true;
                if (!String.IsNullOrEmpty(raw) && raw != "0")
                    foreach (string token in raw.Split(','))
                    {
                        int id;
                        if (!Int32.TryParse(token.Trim(), out id)) { valid = false; break; }
                        ids.Add(id);
                    }
                if (!valid) continue;
                groups[raw] = ids;
                allIds.UnionWith(ids);
            }
            Dictionary<int, List<teamlist>> people = new Dictionary<int, List<teamlist>>();
            List<int> wanted = allIds.ToList();
            DataAccess da = new DataAccess();
            for (int offset = 0; offset < wanted.Count; offset += 500)
            {
                string idList = String.Join(",", wanted.Skip(offset).Take(500)
                    .Select(x => x.ToString(CultureInfo.InvariantCulture)));
                string sql = @"select id,DisplayName,fileupload1,
                    UPPER(SUBSTRING(DisplayName,1,1)) as splinameone
                    from tbl_employee where status=1 and archive=0 and regid=" +
                    regid.ToString(CultureInfo.InvariantCulture) + " and id in (" + idList + ")";
                DataSet ds = da.GetDataSetInline(sql);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    int id = Convert.ToInt32(row["id"]);
                    List<teamlist> list;
                    if (!people.TryGetValue(id, out list)) { list = new List<teamlist>(); people[id] = list; }
                    list.Add(new teamlist { id = Convert.ToString(row["id"]),
                        name = Convert.ToString(row["DisplayName"]),
                        profileimage = Convert.ToString(row["fileupload1"]),
                        shortname = Convert.ToString(row["splinameone"]) });
                }
            }
            Dictionary<string, List<teamlist>> result = new Dictionary<string, List<teamlist>>();
            foreach (KeyValuePair<string, HashSet<int>> group in groups)
            {
                List<teamlist> list = new List<teamlist>();
                foreach (int id in group.Value)
                {
                    List<teamlist> items;
                    if (people.TryGetValue(id, out items)) list.AddRange(items);
                }
                result[group.Key] = list;
            }
            return result;
        }
        catch (Exception ex)
        {
            SaveLogText("LoadTaskTeamsEX-" + ex.Message);
            return null;
        }
    }

    private static Dictionary<string, List<taglist>> LoadTaskTags(DataTable tasks, string regidValue, bool useSubtaskType)
    {
        try
        {
            int regid;
            if (!Int32.TryParse(regidValue, out regid)) return null;
            Dictionary<string, HashSet<int>> ids = new Dictionary<string, HashSet<int>>();
            ids["subtask"] = new HashSet<int>();
            ids["task"] = new HashSet<int>();
            ids["milestone"] = new HashSet<int>();
            Dictionary<string, List<taglist>> result = new Dictionary<string, List<taglist>>();
            Dictionary<string, HashSet<int>> selectedTags = new Dictionary<string, HashSet<int>>();
            HashSet<int> allTags = new HashSet<int>();
            foreach (DataRow row in tasks.Rows)
            {
                string type = useSubtaskType ? "subtask" : Convert.ToString(row["tasktype"]);
                int id;
                if (!ids.ContainsKey(type) || !Int32.TryParse(Convert.ToString(row["id"]), out id))
                    continue;
                ids[type].Add(id);
                result[type + "|" + id.ToString(CultureInfo.InvariantCulture)] = new List<taglist>();
            }
            DataAccess da = new DataAccess();
            foreach (KeyValuePair<string, HashSet<int>> group in ids)
            {
                string table = group.Key == "task" ? "pms_tbl_addtask" :
                    group.Key == "milestone" ? "pms_tbl_addmilestone" : "pms_tbl_addsubtask";
                List<int> values = group.Value.ToList();
                for (int offset = 0; offset < values.Count; offset += 500)
                {
                    string idList = String.Join(",", values.Skip(offset).Take(500)
                        .Select(x => x.ToString(CultureInfo.InvariantCulture)));
                    DataSet ds = da.GetDataSetInline("select id,tagids from " + table +
                        " with(nolock) where status=1 and archive=0 and regid=" +
                        regid.ToString(CultureInfo.InvariantCulture) + " and id in (" + idList + ")");
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        string key = group.Key + "|" + Convert.ToInt32(row["id"]).ToString(CultureInfo.InvariantCulture);
                        if (selectedTags.ContainsKey(key)) continue;
                        HashSet<int> tagIds = new HashSet<int>();
                        string raw = Convert.ToString(row["tagids"]);
                        bool valid = true;
                        if (!String.IsNullOrWhiteSpace(raw))
                            foreach (string token in raw.Split(','))
                            {
                                int tagId;
                                if (!Int32.TryParse(token.Trim(), out tagId)) { valid = false; break; }
                                tagIds.Add(tagId);
                            }
                        if (!valid)
                        {
                            // Preserve legacy handling for nonstandard tag expressions.
                            result.Remove(key);
                            continue;
                        }
                        selectedTags[key] = tagIds;
                        allTags.UnionWith(tagIds);
                    }
                }
            }
            Dictionary<int, List<taglist>> tagLookup = new Dictionary<int, List<taglist>>();
            List<int> wanted = allTags.ToList();
            for (int offset = 0; offset < wanted.Count; offset += 500)
            {
                string idList = String.Join(",", wanted.Skip(offset).Take(500)
                    .Select(x => x.ToString(CultureInfo.InvariantCulture)));
                DataSet ds = da.GetDataSetInline("select id,Tags,TagsColor from Pms_tbl_Addtags with(nolock)" +
                    " where status=1 and archive=0 and regid=" + regid.ToString(CultureInfo.InvariantCulture) +
                    " and id in (" + idList + ")");
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    int id = Convert.ToInt32(row["id"]);
                    List<taglist> items;
                    if (!tagLookup.TryGetValue(id, out items)) { items = new List<taglist>(); tagLookup[id] = items; }
                    items.Add(new taglist { id = Convert.ToString(row["id"]),
                        tagname = Convert.ToString(row["Tags"]), tagcolor = Convert.ToString(row["TagsColor"]) });
                }
            }
            foreach (KeyValuePair<string, HashSet<int>> task in selectedTags)
                foreach (int tagId in task.Value)
                {
                    List<taglist> items;
                    if (tagLookup.TryGetValue(tagId, out items)) result[task.Key].AddRange(items);
                }
            return result;
        }
        catch (Exception ex)
        {
            SaveLogText("LoadTaskTagsEX-" + ex.Message);
            return null;
        }
    }

    private static List<taglist> GetLoadedTaskTags(Dictionary<string, List<taglist>> tags, string id, string type)
    {
        List<taglist> list;
        if (tags != null && tags.TryGetValue(type + "|" + id, out list)) return list;
        return gettasktaglist(id, type);
    }

    private static Dictionary<string, int[]> LoadTaskHierarchyOrders(DataTable tasks, string regidValue)
    {
        try
        {
            int regid;
            if (!Int32.TryParse(regidValue, out regid) || regid <= 0) return null;
            Dictionary<string, HashSet<int>> ids = new Dictionary<string, HashSet<int>>();
            ids["milestone"] = new HashSet<int>();
            ids["task"] = new HashSet<int>();
            ids["subtask"] = new HashSet<int>();
            Dictionary<string, int[]> result = new Dictionary<string, int[]>();
            foreach (DataRow row in tasks.Rows)
            {
                string type = Convert.ToString(row["tasktype"]).Trim().ToLowerInvariant();
                int id;
                if (!ids.ContainsKey(type) || !Int32.TryParse(Convert.ToString(row["id"]), out id) || id <= 0)
                    continue;
                ids[type].Add(id);
                result[type + "|" + id.ToString(CultureInfo.InvariantCulture)] = new int[3];
            }
            DataAccess da = new DataAccess();
            foreach (KeyValuePair<string, HashSet<int>> group in ids)
            {
                List<int> values = group.Value.ToList();
                for (int offset = 0; offset < values.Count; offset += 500)
                {
                    string idList = String.Join(",", values.Skip(offset).Take(500)
                        .Select(x => x.ToString(CultureInfo.InvariantCulture)));
                    string query;
                    if (group.Key == "milestone")
                        query = @"select m.id as EntityId, isnull(m.orderno,0) as MilestoneOrderNo,
                            0 as TaskOrderNo, 0 as SubTaskOrderNo
                            from Pms_tbl_Addmilestone m with(nolock)
                            where m.status=1 and m.archive=0 and m.regid=" +
                            regid.ToString(CultureInfo.InvariantCulture) + " and m.id in (" + idList + ")";
                    else if (group.Key == "task")
                        query = @"select t.id as EntityId, isnull(m.orderno,0) as MilestoneOrderNo,
                            isnull(t.orderno,0) as TaskOrderNo, 0 as SubTaskOrderNo
                            from Pms_tbl_Addtask t with(nolock)
                            left join Pms_tbl_Addmilestone m with(nolock)
                              on m.id=t.Milestoneid and m.regid=t.regid and m.status=1 and m.archive=0
                            where t.status=1 and t.archive=0 and t.regid=" +
                            regid.ToString(CultureInfo.InvariantCulture) + " and t.id in (" + idList + ")";
                    else
                        query = @"select st.id as EntityId, isnull(m.orderno,0) as MilestoneOrderNo,
                            isnull(t.orderno,0) as TaskOrderNo, isnull(st.orderno,0) as SubTaskOrderNo
                            from Pms_tbl_Addsubtask st with(nolock)
                            left join Pms_tbl_Addtask t with(nolock)
                              on t.id=st.Task and t.regid=st.regid and t.status=1 and t.archive=0
                            left join Pms_tbl_Addmilestone m with(nolock)
                              on m.id=st.Milestoneid and m.regid=st.regid and m.status=1 and m.archive=0
                            where st.status=1 and st.archive=0 and st.regid=" +
                            regid.ToString(CultureInfo.InvariantCulture) + " and st.id in (" + idList + ")";
                    DataSet ds = da.GetDataSetInline(query);
                    HashSet<string> seen = new HashSet<string>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        string key = group.Key + "|" + Convert.ToInt32(row["EntityId"])
                            .ToString(CultureInfo.InvariantCulture);
                        if (!seen.Add(key)) continue;
                        int[] order = new int[3];
                        Int32.TryParse(Convert.ToString(row["MilestoneOrderNo"]), out order[0]);
                        Int32.TryParse(Convert.ToString(row["TaskOrderNo"]), out order[1]);
                        Int32.TryParse(Convert.ToString(row["SubTaskOrderNo"]), out order[2]);
                        result[key] = order;
                    }
                }
            }
            return result;
        }
        catch (Exception ex)
        {
            SaveLogText("LoadTaskHierarchyOrdersEX-" + ex.Message);
            return null;
        }
    }

    private static int[] GetLoadedTaskHierarchyOrders(Dictionary<string, int[]> orders,
        DataAccess da, string regid, string id, string taskType)
    {
        int[] order;
        string key = (taskType ?? "subtask").Trim().ToLowerInvariant() + "|" + id;
        if (orders != null && orders.TryGetValue(key, out order)) return order;
        return GetTaskHierarchyOrderNumbers(da, regid, id, taskType);
    }

    private static int[] GetTaskHierarchyOrderNumbers(DataAccess da, string regidValue, string entityIdValue, string taskTypeValue)
    {
        int[] result = new int[] { 0, 0, 0 }; // Milestone, Task, Subtask

        try
        {
            int regid;
            int entityId;
            if (!Int32.TryParse(regidValue, out regid) || regid <= 0) return result;
            if (!Int32.TryParse(entityIdValue, out entityId) || entityId <= 0) return result;

            string taskType = (taskTypeValue ?? "subtask").Trim().ToLowerInvariant();
            string sql = "";

            if (taskType == "milestone")
            {
                sql = @"
select top(1)
       isnull(m.orderno,0) as MilestoneOrderNo,
       0 as TaskOrderNo,
       0 as SubTaskOrderNo
from Pms_tbl_Addmilestone m with(nolock)
where m.status=1 and m.archive=0
  and m.regid=" + regid + @"
  and m.id=" + entityId;
            }
            else if (taskType == "task")
            {
                sql = @"
select top(1)
       isnull(m.orderno,0) as MilestoneOrderNo,
       isnull(t.orderno,0) as TaskOrderNo,
       0 as SubTaskOrderNo
from Pms_tbl_Addtask t with(nolock)
left join Pms_tbl_Addmilestone m with(nolock)
       on m.id=t.Milestoneid
      and m.regid=t.regid
      and m.status=1 and m.archive=0
where t.status=1 and t.archive=0
  and t.regid=" + regid + @"
  and t.id=" + entityId;
            }
            else
            {
                sql = @"
select top(1)
       isnull(m.orderno,0) as MilestoneOrderNo,
       isnull(t.orderno,0) as TaskOrderNo,
       isnull(st.orderno,0) as SubTaskOrderNo
from Pms_tbl_Addsubtask st with(nolock)
left join Pms_tbl_Addtask t with(nolock)
       on t.id=st.Task
      and t.regid=st.regid
      and t.status=1 and t.archive=0
left join Pms_tbl_Addmilestone m with(nolock)
       on m.id=st.Milestoneid
      and m.regid=st.regid
      and m.status=1 and m.archive=0
where st.status=1 and st.archive=0
  and st.regid=" + regid + @"
  and st.id=" + entityId;
            }

            DataSet dsOrder = da.GetDataSetInline(sql);
            if (dsOrder != null && dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count > 0)
            {
                Int32.TryParse(Convert.ToString(dsOrder.Tables[0].Rows[0]["MilestoneOrderNo"]), out result[0]);
                Int32.TryParse(Convert.ToString(dsOrder.Tables[0].Rows[0]["TaskOrderNo"]), out result[1]);
                Int32.TryParse(Convert.ToString(dsOrder.Tables[0].Rows[0]["SubTaskOrderNo"]), out result[2]);
            }
        }
        catch (Exception ex)
        {
            SaveLogText("GetTaskHierarchyOrderNumbersEX-" + ex.Message);
        }

        return result;
    }

    // =========================================================
    // WEEKLY REPORT (display/report only)
    // Existing My Task save/status/timer working is not changed.
    // =========================================================
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static WeeklyReportData GetWeeklyReportData(string weekStart = "", string weekEnd = "")
    {
        WeeklyReportData result = new WeeklyReportData();
        result.days = new List<WeeklyReportDay>();
        result.work = new List<WeeklyReportWork>();
        result.shiftStart = "09:00 AM";
        result.shiftEnd = "06:00 PM";
        result.shiftTime = "09:00";
        result.weekCapacity = "00:00";
        result.weekActualTime = "00:00";
        result.workingDays = 0;

        try
        {
            DateTime startDate;
            if (!WeeklyTryParseDate(weekStart, out startDate))
            {
                startDate = DateTime.Today;
                int backToMonday = ((int)startDate.DayOfWeek + 6) % 7;
                startDate = startDate.AddDays(-backToMonday);
            }
            startDate = startDate.Date;

            // Weekly View always represents exactly Monday-Sunday / 7 calendar days.
            DateTime endExclusive = startDate.AddDays(7);

            HttpCookie regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            HttpCookie userCookie = HttpContext.Current.Request.Cookies["user"];
            if (regCookie == null || userCookie == null ||
                regCookie["id"] == null || userCookie["user_id"] == null)
                return result;

            int regid;
            int userid;
            if (!Int32.TryParse(regCookie["id"].ToString(), out regid)) return result;
            if (!Int32.TryParse(userCookie["user_id"].ToString(), out userid)) return result;

            // Follow the page's existing empid behavior when an administrator opens another user's My Task.
            try
            {
                if (HttpContext.Current.Request.QueryString["empid"] != null)
                {
                    CommonFunction cmf = new CommonFunction();
                    userid = Convert.ToInt32(cmf.Decrypt(HttpContext.Current.Request.QueryString["empid"]));
                }
            }
            catch { }

            DataAccess da = new DataAccess();

            // Same shift source already used by My Team Task:
            // tbl_employee.ShiftType -> tbl_shift.starttime/endtime.
            string shiftSql = @"
select top(1) shft.starttime, shft.endtime, isnull(shft.lunchbreak,0) as lunchbreak
from tbl_employee emp with(nolock)
inner join tbl_shift shft with(nolock) on emp.ShiftType = shft.id
where emp.regid=" + regid + @"
  and emp.status=1 and emp.archive=0
  and emp.id=" + userid;

            int lunchBreakMinutes = 0;
            DataSet dsShift = da.GetDataSetInline(shiftSql);
            if (dsShift.Tables.Count > 0 && dsShift.Tables[0].Rows.Count > 0)
            {
                string s = dsShift.Tables[0].Rows[0]["starttime"].ToString();
                string e = dsShift.Tables[0].Rows[0]["endtime"].ToString();
                if (!String.IsNullOrWhiteSpace(s)) result.shiftStart = s;
                if (!String.IsNullOrWhiteSpace(e)) result.shiftEnd = e;

                // Weekly Assigned Time / Week Capacity only:
                // tbl_shift.lunchbreak is stored in minutes (example: 30 = 30 minutes).
                // Shift display, Timesheet scale, timer and spent-time working stay unchanged.
                int parsedLunchBreak;
                if (Int32.TryParse(dsShift.Tables[0].Rows[0]["lunchbreak"].ToString(), out parsedLunchBreak))
                    lunchBreakMinutes = Math.Max(0, parsedLunchBreak);
            }

            TimeSpan shiftStart = WeeklyParseTime(result.shiftStart, new TimeSpan(9, 0, 0));
            TimeSpan shiftEnd = WeeklyParseTime(result.shiftEnd, new TimeSpan(18, 0, 0));
            DateTime shiftStartDate = DateTime.Today.Add(shiftStart);
            DateTime shiftEndDate = DateTime.Today.Add(shiftEnd);
            if (shiftEndDate <= shiftStartDate) shiftEndDate = shiftEndDate.AddDays(1); // night shift
            int shiftMinutes = Math.Max(0, Convert.ToInt32(Math.Round((shiftEndDate - shiftStartDate).TotalMinutes)));
            result.shiftTime = WeeklyFormatSeconds((long)shiftMinutes * 60L);

            // Load the default Week Off policy once, using the same fields/rules as Assign Project.
            DataRow weekOffRow = null;
            try
            {
                string weekOffSql = "select top(1) * from tbl_weekoffmaster with(nolock) " +
                                    "where status=1 and archive=0 and isnull(isdefault,0)=1 and regid='" + regid + "'";
                DataSet dsWeekOff = da.GetDataSetInline(weekOffSql);
                if (dsWeekOff.Tables.Count > 0 && dsWeekOff.Tables[0].Rows.Count > 0)
                    weekOffRow = dsWeekOff.Tables[0].Rows[0];
            }
            catch { }

            // Load holidays for this week once.
            HashSet<string> holidayKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            try
            {
                string startSql = startDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
                string endSql = endExclusive.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
                string holidaySql =
                    "select cast(eventdate as date) as EventDate from holidays with(nolock) " +
                    "where status=1 and archive=0 and regid='" + regid + "' " +
                    "and cast(eventdate as date)>=convert(date,'" + startSql + "',112) " +
                    "and cast(eventdate as date)<convert(date,'" + endSql + "',112)";
                DataSet dsHoliday = da.GetDataSetInline(holidaySql);
                if (dsHoliday.Tables.Count > 0)
                {
                    foreach (DataRow r in dsHoliday.Tables[0].Rows)
                    {
                        DateTime hd;
                        if (DateTime.TryParse(Convert.ToString(r["EventDate"], CultureInfo.InvariantCulture), out hd))
                            holidayKeys.Add(hd.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
                    }
                }
            }
            catch { }

            Dictionary<string, WeeklyReportDay> dayMap = new Dictionary<string, WeeklyReportDay>(StringComparer.OrdinalIgnoreCase);
            long weekCapacitySeconds = 0;

            for (int i = 0; i < 7; i++)
            {
                DateTime day = startDate.AddDays(i);
                string key = day.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                bool isWeekOff = WeeklyIsWeekOff(day, weekOffRow);
                bool isHoliday = holidayKeys.Contains(key);
                bool working = !isWeekOff && !isHoliday;

                // Daily Total Time tooltip remains the full shift duration.
                long capacitySeconds = working ? (long)shiftMinutes * 60L : 0L;

                // Weekly Assigned Time / Week Capacity excludes lunch once per working day.
                // Week Off and Holiday already contribute zero capacity.
                int assignedMinutesForDay = working
                    ? Math.Max(0, shiftMinutes - Math.Min(lunchBreakMinutes, shiftMinutes))
                    : 0;

                WeeklyReportDay item = new WeeklyReportDay();
                item.key = key;
                item.isWeekOff = isWeekOff;
                item.isHoliday = isHoliday;
                item.totalTime = WeeklyFormatSeconds(capacitySeconds);
                item.actualTime = "00:00";
                item.actualSeconds = 0;
                result.days.Add(item);
                dayMap[key] = item;

                if (working)
                {
                    result.workingDays++;
                    weekCapacitySeconds += (long)assignedMinutesForDay * 60L;
                }
            }

            result.weekCapacity = WeeklyFormatSeconds(weekCapacitySeconds);

            // Day-wise Spent Time comes directly from Pms_Tbl_PauseStatus.PausedTime,
            // exactly as the existing timer save flow stores it on pause/stop.
            // StartDatetime decides the report date. No timer-save behavior is changed.
            string reportStartSql = startDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
            string reportEndSql = endExclusive.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
            string workSql = @"
select lower(isnull(type,'subtask')) as tasktype,
       isnull(subtaskid,0) as subtaskid,
       isnull(taskid,0) as taskid,
       isnull(milestoneid,0) as milestoneid,
       cast(StartDatetime as date) as WorkDate,
       isnull(sum(cast(isnull(PausedTime,0) as numeric(18,2))),0) as TotalMinutes
from Pms_Tbl_PauseStatus with(nolock)
where status=1 and archive=0
  and regid=" + regid + @"
  and uid=" + userid + @"
  and StartDatetime is not null
  and StartDatetime >= convert(datetime,'" + reportStartSql + @"',112)
  and StartDatetime < convert(datetime,'" + reportEndSql + @"',112)
  and isnull(entrytype,'') in ('pause','stop')
  and isnumeric(isnull(PausedTime,0))=1
group by lower(isnull(type,'subtask')),
         isnull(subtaskid,0),
         isnull(taskid,0),
         isnull(milestoneid,0),
         cast(StartDatetime as date)";

            DataSet dsWork = da.GetDataSetInline(workSql);
            Dictionary<string, long> workSeconds = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
            Dictionary<string, long> daySeconds = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);

            if (dsWork.Tables.Count > 0)
            {
                foreach (DataRow row in dsWork.Tables[0].Rows)
                {
                    string taskType = Convert.ToString(row["tasktype"], CultureInfo.InvariantCulture).Trim().ToLowerInvariant();
                    string entityId = "0";

                    if (taskType == "task")
                        entityId = Convert.ToString(row["taskid"], CultureInfo.InvariantCulture);
                    else if (taskType == "milestone")
                        entityId = Convert.ToString(row["milestoneid"], CultureInfo.InvariantCulture);
                    else
                    {
                        taskType = "subtask";
                        entityId = Convert.ToString(row["subtaskid"], CultureInfo.InvariantCulture);
                    }

                    if (String.IsNullOrWhiteSpace(entityId) || entityId == "0")
                        continue;

                    DateTime workDate;
                    if (!DateTime.TryParse(Convert.ToString(row["WorkDate"], CultureInfo.InvariantCulture), out workDate))
                        continue;

                    decimal totalMinutes = 0;
                    Decimal.TryParse(
                        Convert.ToString(row["TotalMinutes"], CultureInfo.InvariantCulture),
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture,
                        out totalMinutes
                    );

                    long seconds = Math.Max(0L, Convert.ToInt64(Math.Round(totalMinutes * 60M)));
                    string dayKey = workDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    string mapKey = taskType + "|" + entityId + "|" + dayKey;

                    if (!workSeconds.ContainsKey(mapKey)) workSeconds[mapKey] = 0;
                    workSeconds[mapKey] += seconds;

                    if (!daySeconds.ContainsKey(dayKey)) daySeconds[dayKey] = 0;
                    daySeconds[dayKey] += seconds;
                }
            }
            foreach (KeyValuePair<string, long> kv in workSeconds)
            {
                int lastPipe = kv.Key.LastIndexOf('|');
                if (lastPipe <= 0) continue;
                WeeklyReportWork w = new WeeklyReportWork();
                w.entityKey = kv.Key.Substring(0, lastPipe);
                w.date = kv.Key.Substring(lastPipe + 1);
                w.seconds = kv.Value;
                result.work.Add(w);
            }

            long weekActualSeconds = 0;
            foreach (WeeklyReportDay day in result.days)
            {
                long actual = 0;
                if (daySeconds.ContainsKey(day.key)) actual = daySeconds[day.key];
                day.actualSeconds = actual;
                day.actualTime = WeeklyFormatSeconds(actual);
                weekActualSeconds += actual;
            }
            result.weekActualTime = WeeklyFormatSeconds(weekActualSeconds);
        }
        catch (Exception ex)
        {
            SaveLogText("WeeklyReport EX-" + ex.Message);
        }

        return result;
    }

    private static bool WeeklyTryParseDate(string value, out DateTime date)
    {
        date = DateTime.MinValue;
        if (String.IsNullOrWhiteSpace(value)) return false;
        string[] formats = new string[] { "yyyy-MM-dd", "MM/dd/yyyy", "M/d/yyyy", "dd/MM/yyyy", "d/M/yyyy" };
        return DateTime.TryParseExact(value.Trim(), formats, CultureInfo.InvariantCulture,
                   DateTimeStyles.AllowWhiteSpaces, out date) || DateTime.TryParse(value, out date);
    }

    private static TimeSpan WeeklyParseTime(string value, TimeSpan fallback)
    {
        if (String.IsNullOrWhiteSpace(value)) return fallback;
        DateTime dt;
        string[] formats = new string[] { "hh:mm tt", "h:mm tt", "HH:mm", "HH:mm:ss", "H:mm", "H:mm:ss" };
        if (DateTime.TryParseExact(value.Trim(), formats, CultureInfo.InvariantCulture,
            DateTimeStyles.AllowWhiteSpaces, out dt)) return dt.TimeOfDay;
        if (DateTime.TryParse(value, out dt)) return dt.TimeOfDay;
        return fallback;
    }

    private static string WeeklyFormatSeconds(long totalSeconds)
    {
        // Weekly/Timesheet report display is HH:mm only.
        // Keep raw seconds separately in WeeklyReportWork for calculations.
        if (totalSeconds < 0) totalSeconds = 0;
        long totalMinutes = totalSeconds / 60;
        long hours = totalMinutes / 60;
        long minutes = totalMinutes % 60;
        return hours.ToString("00", CultureInfo.InvariantCulture) + ":" +
               minutes.ToString("00", CultureInfo.InvariantCulture);
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

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static viewhistory bindtaskhistory(string subtaskid,string tasktype)
    {
        CommonFunction cmf = new CommonFunction();
        DataAccess da = new DataAccess();
        viewhistory obj = new viewhistory();
        if (tasktype == "")
            tasktype = "subtask";
        string projectid = "0";
        string milestoneid = "0";
        string taskid = "0";
        var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
        if (regCookie == null)
            return obj;
        string regid = regCookie["id"];
        
        try
        {
            string userid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();
            if (HttpContext.Current.Request.QueryString["empid"] != null)
            {
                userid = cmf.Decrypt(HttpContext.Current.Request.QueryString["empid"]).ToString();

            }
            obj.taskname = "ok111";
            string strselect = "select id,projectid,milestoneid,isnull(Task,0) as Task from pms_tbl_addsubtask where status=1 and archive=0 and id=" + subtaskid + "";
          if(tasktype=="task")
                strselect = "select id,projectid,milestoneid,id as Task from pms_tbl_addtask where status=1 and archive=0 and id=" + subtaskid + "";
            else if (tasktype == "milestone")
                strselect = "select id,projectid,id as milestoneid,'' as Task from pms_tbl_addmilestone where status=1 and archive=0 and id=" + subtaskid + "";
            //obj.taskname = strselect;
            DataSet dsST = da.GetDataSetInline(strselect);
            projectid = dsST.Tables[0].Rows[0]["projectid"].ToString();
            milestoneid = dsST.Tables[0].Rows[0]["milestoneid"].ToString();
            taskid = dsST.Tables[0].Rows[0]["Task"].ToString();
            SqlParameter[] prm1 = new SqlParameter[7];
            prm1[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm1[1] = new SqlParameter("@projectid", projectid);
            prm1[2] = new SqlParameter("@milestoneid", milestoneid);
            prm1[3] = new SqlParameter("@taskid", taskid);
            prm1[4] = new SqlParameter("@subtaskid", subtaskid);
            if (tasktype == "task")
            {
                prm1[2] = new SqlParameter("@milestoneid", milestoneid);
                prm1[3] = new SqlParameter("@taskid", subtaskid);
                prm1[4] = new SqlParameter("@subtaskid", "%");
            }
            else if (tasktype == "milestone")
            {
                prm1[2] = new SqlParameter("@milestoneid", subtaskid);
                prm1[3] = new SqlParameter("@taskid", "%");
                prm1[4] = new SqlParameter("@subtaskid", "%");
            }
            prm1[5] = new SqlParameter("@type", tasktype);
            prm1[6] = new SqlParameter("@userid", userid);
            SaveLogText("historydetailsA1-" + prm1[0].Value+","+ prm1[1].Value + "," + prm1[2].Value + "," + prm1[3].Value + "," + prm1[4].Value + "," + prm1[5].Value + "," + prm1[6].Value);
            DataSet dsL = da.GetDataSet("pms_GetSubtasktimer_ang", prm1);

            if (dsL.Tables[0].Rows.Count > 0)
            {
                
                string tasknames = get_taskname(subtaskid, tasktype);
                if (tasknames.Length > 30)
                    tasknames = tasknames.Substring(0, 30) + "...";
               
                List<subtaskhistory> _history=gettaskhistory(dsL.Tables[0].Rows[0]["id"].ToString(), tasktype);
                obj.taskname = tasknames;
                obj.spent = dsL.Tables[0].Rows[0]["hour"].ToString() + ":" + dsL.Tables[0].Rows[0]["minute"].ToString() + ":" + dsL.Tables[0].Rows[0]["second"].ToString();//+ ":" + dsL.Tables[0].Rows[0]["millisecond"].ToString()
                obj.history = _history;
            }

        }
        catch (Exception ex)
        {
            SaveLogText("chkadminsubtaskEX-" + ex.Message);
            return new viewhistory();
        }


        return obj;
    }
    public static List<subtaskhistory> gettaskhistory(string id,string tasktype)
    {
        
        DataAccess da = new DataAccess();
        List<subtaskhistory> obj = new List<subtaskhistory>();
        SaveLogText("historydetails3-1");
        try
        {
            if (string.IsNullOrEmpty(id))
                id = "0";

            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null)
                return obj;

            string regid = regCookie["id"];


            try
            {
                SqlParameter[] prm1 = new SqlParameter[3];
                prm1[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prm1[1] = new SqlParameter("@transationid", id);
                prm1[2] = new SqlParameter("@type", tasktype);
                SaveLogText("historydetails3-" + prm1[0].Value + "," + prm1[1].Value + "," + "," + prm1[2].Value);
                DataSet dsL = da.GetDataSet("pms_GetSubtasktimerhistory_ang", prm1);
                if (dsL.Tables[0].Rows.Count > 0)
                {
                    SaveLogText("historydetails4-" + dsL.Tables[0].Rows.Count);
                    foreach (DataRow row in dsL.Tables[0].Rows)
                    {

                        obj.Add(new subtaskhistory
                        {

                            startdate = GetNullableDate(row["tstartdate"]),
                            enddate = GetNullableDate(row["tenddate"]),
                            spenttime = ConvertMinutesToTimer_history(Convert.ToDouble(row["PausedTime"].ToString()==""?"0": row["PausedTime"].ToString())),
                            remark = row["notes"].ToString(),


                        });
                    }
                }
                // projectlist.Add(obj);
            }
            catch(Exception ex) {
                SaveLogText("historydetails3EX-"+ ex.Message);
            }




        }
        catch (Exception ex)
        {
            
            return new List<subtaskhistory>();
        }


        return obj;
    }
    public static string get_totalspent_subtask(string id)
    {
        DataAccess da = new DataAccess();
        string ret = "00:00:00";
        string transactionid = "0";
        try
        {
            string strsqlselect = "select * from pms_tbl_addsubtask where status=1 and archive=0 and regid='"+ HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='"+id+"'";
           DataSet dsD= da.GetDataSetInline(strsqlselect);
            if (dsD.Tables[0].Rows.Count > 0)
            {

                string strtrans = "select  *  from Pms_tbl_Timerstatus where type='subtask' and subtaskid='" + id + "' and taskid='" + dsD.Tables[0].Rows[0]["Task"].ToString() + "' and ProjectId='" + dsD.Tables[0].Rows[0]["projectid"].ToString() + "' and Milestoneid='" + dsD.Tables[0].Rows[0]["Milestoneid"].ToString() + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"].ToString() + "' and status=1 and archive=0 order by id desc";
                DataSet dstrans = da.GetDataSetInline(strtrans);
                if (dstrans.Tables[0].Rows.Count > 0)
                {
                    transactionid = dstrans.Tables[0].Rows[0]["id"].ToString();
                }
                string sqlselectall = "select isnull(sum(isnull(cast(PausedTime as int),0)),0) as totalmin from Pms_Tbl_PauseStatus where status=1 and archive=0 and Transactionid='" + transactionid + "' and entrytype in ('pause','stop') and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                DataSet dsTM = da.GetDataSetInline(sqlselectall);
                ret = ConvertMinutesToTimer(Convert.ToDouble(dsTM.Tables[0].Rows[0]["totalmin"].ToString()));
            }
        }
        catch { }
        return ret;
    }
    public static string get_taskname(string id,string type)
    {
        DataAccess da = new DataAccess();
        string ret = "";
        string transactionid = "0";
        try
        {
            string strsqlselect = "";
            
                 strsqlselect = "select * from pms_tbl_addsubtask where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + id + "'";
            if (type == "task")
                strsqlselect = "select id,taskname as Subtaskname  from pms_tbl_addtask where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + id + "'";
            if (type == "milestone")
                strsqlselect = "select id,milestone as Subtaskname from pms_tbl_addmilestone where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + id + "'";
            DataSet dsD = da.GetDataSetInline(strsqlselect);
                if (dsD.Tables[0].Rows.Count > 0)
                {
                    ret = dsD.Tables[0].Rows[0]["Subtaskname"].ToString();
                }
            
        }
        catch { }
        return ret;
    }
    public static double ConvertTimerToMilliseconds(string time)
    {
        try
        {
            if (string.IsNullOrEmpty(time))
                return 0;

            string[] arr = time.Split(':');

            int h = Convert.ToInt32(arr[0]);

            int m = Convert.ToInt32(arr[1]);

            int s = Convert.ToInt32(arr[2]);

            int ms = Convert.ToInt32(arr[3]);

            return (
                (h * 60 * 60 * 1000) +
                (m * 60 * 1000) +
                (s * 1000) +
                ms
            );
        }
        catch
        {
            return 0;
        }
    }
    public static double ConvertTimerToMinutes(string time)
    {
        try
        {
            /*
                Formats Supported:

                2:15
                2:15:30
                2:15:30:500
            */

            if (string.IsNullOrWhiteSpace(time))
                return 0;

            string[] arr = time.Split(':');

            int h = 0;
            int m = 0;
            int s = 0;
            int ms = 0;

            // HH:MM
            if (arr.Length >= 2)
            {
                h = Convert.ToInt32(arr[0]);

                m = Convert.ToInt32(arr[1]);
            }

            // HH:MM:SS
            if (arr.Length >= 3)
            {
                s = Convert.ToInt32(arr[2]);
            }

            // HH:MM:SS:MS
            if (arr.Length >= 4)
            {
                ms = Convert.ToInt32(arr[3]);
            }

            double totalMinutes =
                (h * 60) +
                m +
                (s / 60.0) +
                (ms / 60000.0);

            return Math.Round(totalMinutes, 2);
        }
        catch
        {
            return 0;
        }
    }
    #region bind master details
    public static DateTime? SafeGetDate(IDataReader dr, string col)
    {
        int ordinal = dr.GetOrdinal(col);
        return dr.IsDBNull(ordinal) ? (DateTime?)null : Convert.ToDateTime(dr.GetValue(ordinal));
    }
    public static DateTime? GetNullableDate(object value)
    {
        return value == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(value);
    }
    protected static int checkadmin(string struserid = "0")
    {
        int retval = 0;
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        function fns = new function();
        string strsql = "select Manage_Profile.* from Manage_Profile inner join tbl_employee on Manage_Profile.id=cast(tbl_employee.roleid as int) where tbl_employee.id='" + struserid + "'  and Manage_Profile.status=1 and Manage_Profile.archive=0 ";
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
    protected static int checkactive(string id,string tasktype)
    {
        int retval = 0;
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        string userid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();
        if (HttpContext.Current.Request.QueryString["empid"] != null)
        {
            userid = cmf.Decrypt(HttpContext.Current.Request.QueryString["empid"]).ToString();

        }
        string strsql = "select id  from Pms_tbl_Timerstatus with(nolock) where isnull(isend,0)=0 and type='"+ tasktype + "' and subtaskid='" + id + "' and uid='"+userid+"'   and status=1 and archive=0 order by id desc";
        if(tasktype=="task")
            strsql = "select id  from Pms_tbl_Timerstatus with(nolock) where isnull(isend,0)=0 and type='" + tasktype + "' and taskid='" + id + "' and uid='" + userid + "'   and status=1 and archive=0 order by id desc";
        if (tasktype == "milestone")
            strsql = "select id  from Pms_tbl_Timerstatus with(nolock) where isnull(isend,0)=0 and type='" + tasktype + "' and milestoneid='" + id + "' and uid='" + userid + "'   and status=1 and archive=0 order by id desc";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
           
                retval = 1;
          
        }
        return retval;
    }
    protected static int checkpause(string id,string tasktype)
    {
        int retval = 0;
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        string userid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();
        if (HttpContext.Current.Request.QueryString["empid"] != null)
        {
            userid = cmf.Decrypt(HttpContext.Current.Request.QueryString["empid"]).ToString();

        }
        string strsql = "select id  from Pms_tbl_Timerstatus with(nolock) where isnull(IsPaused,0)=1 and isnull(isend,0)=0 and type='"+ tasktype + "' and subtaskid='" + id + "' and uid='"+ userid + "' and status=1 and archive=0 order by id desc";
        if (tasktype == "task")
            strsql = "select id  from Pms_tbl_Timerstatus with(nolock) where isnull(IsPaused,0)=1 and isnull(isend,0)=0 and type='" + tasktype + "' and taskid='" + id + "' and uid='" + userid + "' and status=1 and archive=0 order by id desc";
        if (tasktype == "milestone")
            strsql = "select id  from Pms_tbl_Timerstatus with(nolock) where isnull(IsPaused,0)=1 and isnull(isend,0)=0 and type='" + tasktype + "' and milestoneid='" + id + "' and uid='" + userid + "' and status=1 and archive=0 order by id desc";

        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {

                retval = 1;
            
        }
        return retval;
    }
    protected static int checkend(string id,string tasktype)
    {
        int retval = 0;
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        string userid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();
        if (HttpContext.Current.Request.QueryString["empid"] != null)
        {
            userid = cmf.Decrypt(HttpContext.Current.Request.QueryString["empid"]).ToString();

        }
        string strsql = "select id  from Pms_tbl_Timerstatus with(nolock) where  isnull(isend,0)=1 and type='" + tasktype + "' and subtaskid='" + id + "' and uid='" + userid + "' and status=1 and archive=0 order by id desc";
        if (tasktype == "task")
            strsql = "select id  from Pms_tbl_Timerstatus with(nolock) where  isnull(isend,0)=1 and type='" + tasktype + "' and taskid='" + id + "' and uid='" + userid + "' and status=1 and archive=0 order by id desc";
        if (tasktype == "milestone")
            strsql = "select id  from Pms_tbl_Timerstatus with(nolock) where  isnull(isend,0)=1 and type='" + tasktype + "' and milestoneid='" + id + "' and uid='" + userid + "' and status=1 and archive=0 order by id desc";

        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {

            retval = 1;

        }
        return retval;
    }
    protected static string gettimespam(string id,string tasktype)
    {
        CommonFunction cmf = new CommonFunction();
        string retval = "";
        DataAccess da = new DataAccess();
        
        string hour = "00";
        string min = "00";
        string sec = "00";
        string mili = "00";
        string transactionid = "0";
        try
        {
            
            //string strsqlselect = "select * from pms_tbl_addsubtask with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id=" + id + "";
            //if(tasktype=="task")
               // strsqlselect = "select * from pms_tbl_addtask with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id=" + id + "";
            //else if (tasktype == "milestone")
                //strsqlselect = "select * from pms_tbl_addmilestone with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id=" + id + "";

            string strsqlselect = "select id,task,Milestoneid,Projectid from pms_tbl_addsubtask with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id=" + id + "";
            if (tasktype == "task")
                strsqlselect = "select id,id as Task,Milestoneid,Projectid from pms_tbl_addtask with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id=" + id + "";
            if (tasktype == "milestone")
                strsqlselect = "select id,'' Task,id Milestoneid,Projectid from pms_tbl_addmilestone with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id=" + id + "";




            DataSet dsT = ReadTaskLoadingData(da, strsqlselect);
            if (dsT.Tables[0].Rows.Count > 0)
            {
                
                transactionid = "0";
                string pausetime = "";
                int ispause = 0;
                int isrun = 0;
                int isend = 0;
                
                //isnull(isend,0)=0 and
                string strhourminuts = "select *  from Pms_tbl_Timerstatus with(nolock) where  type='"+tasktype+"' and subtaskid='" + id + "' and taskid='" + dsT.Tables[0].Rows[0]["task"].ToString() + "' and ProjectId='" + dsT.Tables[0].Rows[0]["ProjectId"].ToString() + "' and Milestoneid='" + dsT.Tables[0].Rows[0]["Milestoneid"].ToString() + "' and uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"].ToString() + "'  and status=1 and archive=0 order by id desc";
                if (tasktype == "task")
                    strhourminuts = "select *  from Pms_tbl_Timerstatus with(nolock) where  type='" + tasktype + "' and taskid='" + id + "' and ProjectId='" + dsT.Tables[0].Rows[0]["ProjectId"].ToString() + "' and Milestoneid='" + dsT.Tables[0].Rows[0]["Milestoneid"].ToString() + "' and uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"].ToString() + "'  and status=1 and archive=0 order by id desc";
                else if (tasktype == "milestone")
                    strhourminuts = "select *  from Pms_tbl_Timerstatus with(nolock) where  type='" + tasktype + "' and Milestoneid='" + id + "'  and ProjectId='" + dsT.Tables[0].Rows[0]["ProjectId"].ToString() + "'  and uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"].ToString() + "'  and status=1 and archive=0 order by id desc";


                if (id == "2388")
                    SaveLogText("strpauseddetailsCHKA-" + strsqlselect);
                DataSet dshourmin = ReadTaskLoadingData(da, strhourminuts);
                if (dshourmin.Tables[0].Rows.Count > 0)
                {
                    transactionid = dshourmin.Tables[0].Rows[0]["id"].ToString();
                    if (dshourmin.Tables[0].Rows[0]["Isend"].ToString() == "1")
                    {
                        hour = "00";
                        min = "00";
                        sec = "00";
                        mili = "00";
                    }
                    else if (dshourmin.Tables[0].Rows[0]["IsPaused"].ToString() == "1")
                    {
                        hour = dshourmin.Tables[0].Rows[0]["hour"].ToString();
                        min = dshourmin.Tables[0].Rows[0]["minute"].ToString();
                        sec = dshourmin.Tables[0].Rows[0]["second"].ToString();
                        mili = dshourmin.Tables[0].Rows[0]["millisecond"].ToString();

                    }
                    else
                    {
                        // Running timer: calculate only from the CURRENT open start segment.
                        // This avoids counting idle time between an old stop and a new start.
                        // Existing pause/stop history remains untouched and is added as saved spent minutes.
                        string chkpausemin = "0";

                        string strpauseddetails2 = "select Transactionid, sum(cast(isnull(PausedTime,0) as numeric)) tpause " +
                            "from Pms_Tbl_PauseStatus with(nolock) where Transactionid='" + transactionid + "' " +
                            "and status=1 and archive=0 and type='" + tasktype + "' " +
                            "and isnull(entrytype,'') in ('pause','stop') " +
                            "and isnumeric(isnull(PausedTime,0))=1 group by Transactionid";

                        DataSet dspaused2 = ReadTaskLoadingData(da, strpauseddetails2);
                        if (dspaused2.Tables[0].Rows.Count > 0)
                        {
                            chkpausemin = (dspaused2.Tables[0].Rows[0]["tpause"].ToString() == "" ? "0" : dspaused2.Tables[0].Rows[0]["tpause"].ToString());
                        }

                        string stropenstart = "select top(1) tstartdate from Pms_Tbl_PauseStatus with(nolock) " +
                            "where Transactionid='" + transactionid + "' and status=1 and archive=0 " +
                            "and type='" + tasktype + "' and isnull(entrytype,'')='start' " +
                            "and tenddate is null order by id desc";

                        DataSet dsopenstart = ReadTaskLoadingData(da, stropenstart);
                        DateTime currentStart;
                        if (dsopenstart.Tables[0].Rows.Count > 0 &&
                            DateTime.TryParse(dsopenstart.Tables[0].Rows[0]["tstartdate"].ToString(), out currentStart))
                        {
                            TimeSpan currentRun = DateTime.Now - currentStart;
                            double previousMinutes = 0;
                            Double.TryParse(chkpausemin, out previousMinutes);
                            TimeSpan totalTime = currentRun.Add(TimeSpan.FromMinutes(previousMinutes));

                            int diffdays = totalTime.Days * 24;
                            hour = (totalTime.Hours + diffdays).ToString();
                            min = totalTime.Minutes.ToString();
                            sec = totalTime.Seconds.ToString();
                            mili = totalTime.Milliseconds.ToString();
                        }
                        else
                        {
                            // Fallback for legacy/incomplete rows: preserve previous behavior.
                            TimeSpan toatlhr = (DateTime.Now - Convert.ToDateTime(dshourmin.Tables[0].Rows[0]["StartTime"].ToString()));
                            double previousMinutes = 0;
                            Double.TryParse(chkpausemin, out previousMinutes);
                            toatlhr = toatlhr.Add(TimeSpan.FromMinutes(previousMinutes));
                            int diffdays = (toatlhr.Days) * 24;
                            hour = (toatlhr.Hours + diffdays).ToString();
                            min = toatlhr.Minutes.ToString();
                            sec = toatlhr.Seconds.ToString();
                            mili = toatlhr.Milliseconds.ToString();
                        }
                    }


                }

            }
        }
        catch (Exception ex) { SaveLogText("strhourminutsA1EX-" + ex.Message); }
        retval = hour + ":" + min + ":" + sec + ":" + mili;
        if (transactionid == "1030")
            SaveLogText("strpauseddetailsCHK3-" + retval);
        //SaveLogText("strhourminutsA1-"+retval);
        return retval;
    }
    protected static string gettimespam_old(string id)
    {
        string retval ="";
        DataAccess da = new DataAccess();

        string hour = "00";
        string min = "00";
        string sec = "00";
        string mili = "00";
        try
        {
            string strsqlselect = "select * from pms_tbl_addsubtask with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id=" + id + "";
            DataSet dsT = da.GetDataSetInline(strsqlselect);
            if (dsT.Tables[0].Rows.Count > 0)
            {
                string pausetime = "";
                int ispause = 0;
                int isrun = 0;
                int isend = 0;
                string transactionid = "0";
                //isnull(isend,0)=0 and
                string strhourminuts = "select *  from Pms_tbl_Timerstatus with(nolock) where  type='subtask' and subtaskid='" + id + "' and taskid='" + dsT.Tables[0].Rows[0]["task"].ToString() + "' and ProjectId='" + dsT.Tables[0].Rows[0]["ProjectId"].ToString() + "' and Milestoneid='" + dsT.Tables[0].Rows[0]["Milestoneid"].ToString() + "' and uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"].ToString() + "'  and status=1 and archive=0 order by id desc";
                
                DataSet dshourmin = da.GetDataSetInline(strhourminuts);
                if (dshourmin.Tables[0].Rows.Count > 0)
                {
                    transactionid = dshourmin.Tables[0].Rows[0]["id"].ToString();
                    if (dshourmin.Tables[0].Rows[0]["Isend"].ToString() == "1")
                    {
                        hour = "00";
                        min = "00";
                        sec = "00";
                        mili = "00";
                    }
                    else if (dshourmin.Tables[0].Rows[0]["IsPaused"].ToString() == "1")
                    {
                        hour = dshourmin.Tables[0].Rows[0]["hour"].ToString();
                        min = dshourmin.Tables[0].Rows[0]["minute"].ToString();
                        sec = dshourmin.Tables[0].Rows[0]["second"].ToString();
                        mili = dshourmin.Tables[0].Rows[0]["millisecond"].ToString();

                        //string strpauseddetails = "select *  from Pms_Tbl_PauseStatus with(nolock) where Transactionid='" + transactionid + "' and status=1 and archive=0 and type='subtask' order by id desc";

                        //DataSet dspaused = da.GetDataSetInline(strpauseddetails);

                        //if (dspaused.Tables[0].Rows.Count > 0)
                        //{
                        //    string chkpausemin = "0";

                        //    string strpauseddetails2 = "select Transactionid, sum(cast(isnull(PausedTime,0) as numeric)) tpause  from Pms_Tbl_PauseStatus with(nolock) where Transactionid='" + transactionid + "'  and status=1 and archive=0 and type='subtask' and isnumeric(isnull(PausedTime,0))=1 group by Transactionid";

                        //    DataSet dspaused2 = da.GetDataSetInline(strpauseddetails2);
                        //    if (dspaused2.Tables[0].Rows.Count > 0)
                        //    {
                        //        chkpausemin = dspaused2.Tables[0].Rows[0]["tpause"].ToString();
                        //    }

                        //    TimeSpan toatlhr = (Convert.ToDateTime(dspaused.Tables[0].Rows[0]["tstartdate"].ToString()) - Convert.ToDateTime(dshourmin.Tables[0].Rows[0]["StartTime"].ToString()));
                        //    TimeSpan t1 = TimeSpan.FromMinutes(Convert.ToInt32(chkpausemin));
                        //    //TimeSpan t1 = new TimeSpan(0,Convert.ToInt32(chkpausemin) ,0,0);
                        //    int diffdays = (toatlhr.Days) * 24;
                        //    toatlhr = toatlhr.Subtract(t1);
                        //    SaveLogText("strhourminutsA1-" + transactionid + "=" + toatlhr.ToString());
                        //    hour = (toatlhr.Hours + diffdays).ToString("00");
                        //    min = toatlhr.Minutes.ToString("00");
                        //    sec = toatlhr.Seconds.ToString("00");
                        //    mili = toatlhr.Milliseconds.ToString("000");
                        //    SaveLogText("strhourminutsA1-" + transactionid+"="+ hour + "," + min + "," + sec);
                        //}

                    }
                    else
                    {
                        //string starttime = dshourmin.Tables[0].Rows[0]["hour"].ToString()+":"+ dshourmin.Tables[0].Rows[0]["minute"].ToString()+":"+ dshourmin.Tables[0].Rows[0]["StartTime"].ToString();
                        string chkpausemin = "0";
                        string strpauseddetails2 = "select Transactionid, sum(cast(isnull(PausedTime,0) as numeric)) tpause  from Pms_Tbl_PauseStatus with(nolock) where Transactionid='" + transactionid + "'  and status=1 and archive=0 and type='subtask' and isnumeric(isnull(PausedTime,0))=1 group by Transactionid";
                        DataSet dspaused2 = da.GetDataSetInline(strpauseddetails2);
                        if (dspaused2.Tables[0].Rows.Count > 0)
                        {
                            chkpausemin = (dspaused2.Tables[0].Rows[0]["tpause"].ToString() == "" ? "0" : dspaused2.Tables[0].Rows[0]["tpause"].ToString());
                        }

                        TimeSpan toatlhr = (DateTime.Now - Convert.ToDateTime(dshourmin.Tables[0].Rows[0]["StartTime"].ToString()));
                        TimeSpan t1 = TimeSpan.FromMinutes(Convert.ToInt32(chkpausemin));
                        //TimeSpan t1 = new TimeSpan(0,Convert.ToInt32(chkpausemin) ,0,0);
                        int diffdays = (toatlhr.Days) * 24;
                        toatlhr = toatlhr.Subtract(t1);
                        hour = (toatlhr.Hours + diffdays).ToString();
                        min = toatlhr.Minutes.ToString();
                        sec = toatlhr.Seconds.ToString();
                        mili = toatlhr.Milliseconds.ToString();
                    }


                }
                
            }
        }
        catch(Exception ex) { SaveLogText("strhourminutsA1EX-" + ex.Message); }
        retval = hour + ":" + min + ":" + sec + ":" + mili;
       //SaveLogText("strhourminutsA1-"+retval);
        return retval;
    }
    [WebMethod]
    public static string Getspenttime_byid(string id,string tasktype)
    {
        string retval = "00:00:00";
        string hour = "00";
        string min = "00";
        string sec = "00";
        string mili = "00";
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        string strsql = "";
       string userid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();
        if (HttpContext.Current.Request.QueryString["empid"] != null)
        {
            userid = cmf.Decrypt(HttpContext.Current.Request.QueryString["empid"]).ToString();

        }
        string strhourminuts = "select *  from Pms_tbl_Timerstatus with(nolock) where type='"+tasktype+ "' and  subtaskid='" + id + "' and uid='"+ userid + "'  and status=1 and archive=0 order by id desc";
        if(tasktype=="task")
            strhourminuts = "select *  from Pms_tbl_Timerstatus with(nolock) where  type='" + tasktype + "' and   taskid='" + id + "' and uid='" + userid + "'  and status=1 and archive=0 order by id desc";
        else if (tasktype == "milestone")
            strhourminuts = "select *  from Pms_tbl_Timerstatus with(nolock) where  type='" + tasktype + "' and   milestoneid='" + id + "' and uid='" + userid + "'  and status=1 and archive=0 order by id desc";
        DataSet dshourmin = da.GetDataSetInline(strhourminuts);
        SaveLogText("strhourminutsA-"+ strhourminuts);
        if (dshourmin.Tables[0].Rows.Count > 0)
        {

            hour = dshourmin.Tables[0].Rows[0]["hour"].ToString();
            min = dshourmin.Tables[0].Rows[0]["minute"].ToString();
            sec = dshourmin.Tables[0].Rows[0]["second"].ToString();
            mili = dshourmin.Tables[0].Rows[0]["millisecond"].ToString();



        }
        retval = hour + ":" + min + ":" + sec;
        return retval;
    }
    protected static string gettimespam_spent(string id,string tasktype)
    {
        string retval = "";
        DataAccess da = new DataAccess();

        string hour = "00";
        string min = "00";
        string sec = "00";
        string mili = "00";
        try
        {
            
            string strsqlselect = "select id,task,Milestoneid,Projectid from pms_tbl_addsubtask with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id=" + id + "";
            if (tasktype == "task")
                strsqlselect = "select id,id as Task,Milestoneid,Projectid from pms_tbl_addtask with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id=" + id + "";
            if (tasktype == "milestone")
                strsqlselect = "select id,'' Task,id Milestoneid,Projectid from pms_tbl_addmilestone with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id=" + id + "";



            SaveLogText("strsqlselectA1-"+ strsqlselect);
            DataSet dsT = ReadTaskLoadingData(da, strsqlselect);
            if (dsT.Tables[0].Rows.Count > 0)
            {
                string pausetime = "";
                int ispause = 0;
                int isrun = 0;
                int isend = 0;
                string transactionid = "0";
                //isnull(isend,0)=0 and
                string strhourminuts = "select *  from Pms_tbl_Timerstatus with(nolock) where  type='"+tasktype+"' and subtaskid='" + id + "' and taskid='" + dsT.Tables[0].Rows[0]["task"].ToString() + "' and ProjectId='" + dsT.Tables[0].Rows[0]["ProjectId"].ToString() + "' and Milestoneid='" + dsT.Tables[0].Rows[0]["Milestoneid"].ToString() + "' and uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"].ToString() + "'  and status=1 and archive=0 order by id desc";
                if (tasktype == "task")
                    strhourminuts = "select *  from Pms_tbl_Timerstatus with(nolock) where  type='" + tasktype + "'  and taskid='" + id + "' and ProjectId='" + dsT.Tables[0].Rows[0]["ProjectId"].ToString() + "' and Milestoneid='" + dsT.Tables[0].Rows[0]["Milestoneid"].ToString() + "' and uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"].ToString() + "'  and status=1 and archive=0 order by id desc";
                else if (tasktype == "milestone")
                    strhourminuts = "select *  from Pms_tbl_Timerstatus with(nolock) where  type='" + tasktype + "' and ProjectId='" + dsT.Tables[0].Rows[0]["ProjectId"].ToString() + "' and Milestoneid='" + id + "' and uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"].ToString() + "'  and status=1 and archive=0 order by id desc";

                SaveLogText("strsqlselectA2-" + strhourminuts);
                DataSet dshourmin = ReadTaskLoadingData(da, strhourminuts);
                if (dshourmin.Tables[0].Rows.Count > 0)
                {
                    transactionid = dshourmin.Tables[0].Rows[0]["id"].ToString();
                    
                        hour = dshourmin.Tables[0].Rows[0]["hour"].ToString();
                        min = dshourmin.Tables[0].Rows[0]["minute"].ToString();
                        sec = dshourmin.Tables[0].Rows[0]["second"].ToString();
                        mili = dshourmin.Tables[0].Rows[0]["millisecond"].ToString();



                }

            }
        }
        catch (Exception ex) { SaveLogText("strhourminutsA1EX-" + ex.Message); }
        retval = hour + ":" + min + ":" + sec;
        //SaveLogText("strhourminutsA1-"+retval);
        return retval;
    }
    protected static string compare_timespam(string id,string tasktype)
    {
        string retval = "0|00:00:00";
        CommonFunction cmf = new CommonFunction();
        string userid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();
        if (HttpContext.Current.Request.QueryString["empid"] != null)
        {
            userid = cmf.Decrypt(HttpContext.Current.Request.QueryString["empid"]).ToString();

        }
        DataAccess da = new DataAccess();

        // set duration
        string set_hour = "00";
        string set_min = "00";
        // current duration
        string hour = "00";
        string min = "00";
        string sec = "00";
        string mili = "00";

        try
        {
            // get subtask details
            string strsqlselect = "select id,task,Milestoneid,Projectid,isnull(TaskDuration,'00:00:00') as TaskDuration from pms_tbl_addsubtask with(nolock) " +
                                  "where status=1 and archive=0 " +
                                  "and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' " +
                                  "and id=" + id;
            if(tasktype=="task")
                strsqlselect = "select id,id as Task,Milestoneid,Projectid,isnull(TaskDuration,'00:00:00') as TaskDuration from pms_tbl_addtask with(nolock) " +
                                  "where status=1 and archive=0 " +
                                  "and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' " +
                                  "and id=" + id;
            if (tasktype == "milestone")
                strsqlselect = "select id,'' Task,id Milestoneid,Projectid,isnull(TaskDuration,'00:00:00') as TaskDuration from pms_tbl_addmilestone with(nolock) " +
                                  "where status=1 and archive=0 " +
                                  "and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' " +
                                  "and id=" + id;
            DataSet dsT = ReadTaskLoadingData(da, strsqlselect);

            if (dsT.Tables[0].Rows.Count > 0)
            {
                // =========================
                // GET SET TASK DURATION
                // =========================
                if (dsT.Tables[0].Rows[0]["TaskDuration"].ToString() != string.Empty)
                {
                    try
                    {
                        // expected format: HH:mm
                        string[] arr = dsT.Tables[0].Rows[0]["TaskDuration"].ToString().Split(':');

                        set_hour = arr.Length > 0 ? (arr[0] == "" ? "00" : arr[0]) : "00";
                        set_min = arr.Length > 1 ? (arr[1] == "" ? "00" : arr[1]) : "00";
                    }
                    catch
                    {
                        set_hour = "00";
                        set_min = "00";
                    }
                }

                // =========================
                // GET CURRENT TIMER
                // =========================
                string strhourminuts = "select top 1 * from Pms_tbl_Timerstatus with(nolock) " +
                                       "where type='"+tasktype+"' " +
                                       "and subtaskid='" + id + "' " +
                                       "and taskid='" + dsT.Tables[0].Rows[0]["task"].ToString() + "' " +
                                       "and ProjectId='" + dsT.Tables[0].Rows[0]["ProjectId"].ToString() + "' " +
                                       "and Milestoneid='" + dsT.Tables[0].Rows[0]["Milestoneid"].ToString() + "' " +
                                       "and uid='" + userid + "' " +
                                       "and status=1 and archive=0 " +
                                       "order by id desc";
                if (tasktype == "task")
                    strhourminuts = "select top 1 * from Pms_tbl_Timerstatus with(nolock) " +
                                       "where type='" + tasktype + "' " +
                                       "and taskid='" + id + "' " +
                                       "and ProjectId='" + dsT.Tables[0].Rows[0]["ProjectId"].ToString() + "' " +
                                       "and Milestoneid='" + dsT.Tables[0].Rows[0]["Milestoneid"].ToString() + "' " +
                                       "and uid='" + userid + "' " +
                                       "and status=1 and archive=0 " +
                                       "order by id desc";
                if (tasktype == "milestone")
                    strhourminuts = "select top 1 * from Pms_tbl_Timerstatus with(nolock) " +
                                       "where type='" + tasktype + "' " +
                                       "and ProjectId='" + dsT.Tables[0].Rows[0]["ProjectId"].ToString() + "' " +
                                       "and Milestoneid='" + id + "' " +
                                       "and uid='" + userid + "' " +
                                       "and status=1 and archive=0 " +
                                       "order by id desc";
                DataSet dshourmin = ReadTaskLoadingData(da, strhourminuts);

                if (dshourmin.Tables[0].Rows.Count > 0)
                {
                    hour = dshourmin.Tables[0].Rows[0]["hour"].ToString() == ""
                            ? "00"
                            : dshourmin.Tables[0].Rows[0]["hour"].ToString();

                    min = dshourmin.Tables[0].Rows[0]["minute"].ToString() == ""
                            ? "00"
                            : dshourmin.Tables[0].Rows[0]["minute"].ToString();

                    sec = dshourmin.Tables[0].Rows[0]["second"].ToString() == ""
                            ? "00"
                            : dshourmin.Tables[0].Rows[0]["second"].ToString();

                    mili = dshourmin.Tables[0].Rows[0]["millisecond"].ToString() == ""
                            ? "00"
                            : dshourmin.Tables[0].Rows[0]["millisecond"].ToString();
                }

                // =========================
                // COMPARE TIMES
                // =========================

                TimeSpan setTime = new TimeSpan(
                    Convert.ToInt32(set_hour),
                    Convert.ToInt32(set_min),
                    0
                );

                TimeSpan currentTime = new TimeSpan(
                    Convert.ToInt32(hour),
                    Convert.ToInt32(min),
                    Convert.ToInt32(sec)
                );

                // current > set
                if (currentTime > setTime)
                {
                    TimeSpan extra = currentTime - setTime;

                    retval = "1|" +
                             extra.Hours.ToString("00") + ":" +
                             extra.Minutes.ToString("00") + ":" +
                             extra.Seconds.ToString("00");

                    // example:
                    // 1|00:35:20
                }
                else
                {
                    retval = "0|" +
                             currentTime.Hours.ToString("00") + ":" +
                             currentTime.Minutes.ToString("00") + ":" +
                             currentTime.Seconds.ToString("00");

                    // example:
                    // 0|01:10:15
                }
            }
        }
        catch (Exception ex)
        {
            SaveLogText("compare_timespam EX-" + ex.Message);
            retval = "0|00:00:00";
        }

        return retval;
    }
    protected static string compare_timespam_old(string id)
    {
        string retval = "";
        CommonFunction cmf = new CommonFunction();
        DataAccess da = new DataAccess();
        // to get task duration
        string set_hour = "00";
        string set_min = "00";
        string set_sec = "00";
        string set_mili = "00";

        string hour = "00";
        string min = "00";
        string sec = "00";
        string mili = "00";
        try
        {
            string strsqlselect = "select * from pms_tbl_addsubtask with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id=" + id + "";
            DataSet dsT = da.GetDataSetInline(strsqlselect);
            if (dsT.Tables[0].Rows.Count > 0)
            {
                string pausetime = "";
                int ispause = 0;
                int isrun = 0;
                int isend = 0;

                string set_timeduration = "00:00:00";
                string cr_timeduration = "00:00:00";
                if (dsT.Tables[0].Rows[0]["TaskDuration"].ToString() != string.Empty)
                {
                    try
                    {
                        string[] arr = dsT.Tables[0].Rows[0]["TaskDuration"].ToString().Split(':');

                        set_hour = arr[0] == "" ? "00" : arr[0];
                        set_min = arr[1] == "" ? "00" : arr[1];
                        set_timeduration = set_hour + ":" + set_min + ":00:00";
                    }
                    catch
                    {

                    }
                }

                string transactionid = "0";
                //isnull(isend,0)=0 and
                string userid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();
                if (HttpContext.Current.Request.QueryString["empid"] != null)
                {
                    userid = cmf.Decrypt(HttpContext.Current.Request.QueryString["empid"]).ToString();

                }
                string strhourminuts = "select *  from Pms_tbl_Timerstatus with(nolock) where  type='subtask' and subtaskid='" + id + "' and taskid='" + dsT.Tables[0].Rows[0]["task"].ToString() + "' and ProjectId='" + dsT.Tables[0].Rows[0]["ProjectId"].ToString() + "' and Milestoneid='" + dsT.Tables[0].Rows[0]["Milestoneid"].ToString() + "' and uid='" + userid + "'  and status=1 and archive=0 order by id desc";

                DataSet dshourmin = da.GetDataSetInline(strhourminuts);
                if (dshourmin.Tables[0].Rows.Count > 0)
                {
                    transactionid = dshourmin.Tables[0].Rows[0]["id"].ToString();
                    hour = dshourmin.Tables[0].Rows[0]["hour"].ToString();
                    min = dshourmin.Tables[0].Rows[0]["minute"].ToString();
                    sec = dshourmin.Tables[0].Rows[0]["second"].ToString();
                    mili = dshourmin.Tables[0].Rows[0]["millisecond"].ToString();
                    cr_timeduration = hour + ":" + min + ":" + sec + ":" + mili;
                }
            }

        }
        catch (Exception ex) { SaveLogText("strhourminutsA1EX-" + ex.Message); }

        // compare set set_timeduration with cr_timeduration if cr_timeduration>set_timeduration return a message or 1 with how much time consume extra
        retval = hour + ":" + min + ":" + sec + ":" + mili;
        
        return retval;
    }
    protected static double get_completionstatus(string id, string type)
    {
        double strret = 0;
        string tblname = "Pms_tbl_AddMilestone";
        if (type == "task")
        {
            tblname = "Pms_tbl_Addtask";
        }
        else if (type == "subtask")
        {
            tblname = "Pms_tbl_Addsubtask";
        }
        if (id == "" || id == "0")
        {
            return 0;
        }
        DataAccess da = new DataAccess();

        double mileTaskTotal = 0;
        string findmile = "select cast(ROUND(isnull(iscompletepercentage,0),2) AS DECIMAL(10,2)) as iscompletepercentage from " + tblname + "  where id='" + id + "'  and status=1  and archive=0";
        SaveLogText("chkadmintodayEX-" + findmile);
        DataSet dsssummile = da.GetDataSetInline(findmile);

        try
        {
            mileTaskTotal = Convert.ToDouble(dsssummile.Tables[0].Rows[0]["iscompletepercentage"]);
            if (mileTaskTotal == 99.99)
            {
                mileTaskTotal = 100;
            }
        }
        catch (Exception)
        {
        }
        if (mileTaskTotal > 0)
        {
            strret = mileTaskTotal;

        }
        return strret;

    }
    protected static double getcompletetime(string id,string tasktype)
    {
        double retval =0;
        DataAccess da = new DataAccess();       
        
        try
        {
            string strsqlselect = "select id,Task,Milestoneid,Projectid,isnull(TaskDuration,'00:00:00') as TaskDuration from pms_tbl_addsubtask with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id=" + id + "";
            if(tasktype=="task")
                strsqlselect = "select id,id as Task,Milestoneid,Projectid,isnull(TaskDuration,'00:00:00') as TaskDuration from pms_tbl_addtask with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id=" + id + "";
            if (tasktype == "milestone")
                strsqlselect = "select id,'' Task,id Milestoneid,Projectid,isnull(TaskDuration,'00:00:00') as TaskDuration from pms_tbl_addmilestone with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id=" + id + "";
            DataSet dsT = ReadTaskLoadingData(da, strsqlselect);
            if (dsT.Tables[0].Rows.Count > 0)
            {
                string strhr = "0";
                string strmin = "0";
                string stduration = "00:00";
                if(dsT.Tables[0].Rows[0]["TaskDuration"].ToString()!=string.Empty)
                {
                    try
                    {
                        string[] arr = dsT.Tables[0].Rows[0]["TaskDuration"].ToString().Split(':');
                        
                            strhr =  arr[0] ==""?"0":arr[0];
                            strmin = arr[1] ==""?"0":arr[1];
                            stduration = strhr + ":" + strmin;


                    }
                    catch
                    {

                    }
                }
                double totalminoftask = ConvertTimerToMinutes(stduration);
                SaveLogText("totalminoftask-"+ id+"=" + totalminoftask.ToString());
                double totalminoftask_consume = 0;
                string transactiontime = "00:00:00";
                string strhourminuts = "select id,isnull(hour,'00') as hour,isnull(minute,'00') as minute,isnull(second,'00') as second,isnull(millisecond,'00') as millisecond  from Pms_tbl_Timerstatus with(nolock) where  type='"+ tasktype + "' and subtaskid='" + id + "' and taskid='" + dsT.Tables[0].Rows[0]["task"].ToString() + "' and ProjectId='" + dsT.Tables[0].Rows[0]["ProjectId"].ToString() + "' and Milestoneid='" + dsT.Tables[0].Rows[0]["Milestoneid"].ToString() + "'  and status=1 and archive=0 order by id desc";
                if (tasktype == "task")
                    strhourminuts = "select id,isnull(hour,'00') as hour,isnull(minute,'00') as minute,isnull(second,'00') as second,isnull(millisecond,'00') as millisecond  from Pms_tbl_Timerstatus with(nolock) where  type='" + tasktype + "' and taskid='" + id + "' and ProjectId='" + dsT.Tables[0].Rows[0]["ProjectId"].ToString() + "' and Milestoneid='" + dsT.Tables[0].Rows[0]["Milestoneid"].ToString() + "'  and status=1 and archive=0 order by id desc";
                if (tasktype == "milestone")
                    strhourminuts = "select id,isnull(hour,'00') as hour,isnull(minute,'00') as minute,isnull(second,'00') as second,isnull(millisecond,'00') as millisecond  from Pms_tbl_Timerstatus with(nolock) where  type='" + tasktype + "' and Milestoneid='" + dsT.Tables[0].Rows[0]["Milestoneid"].ToString() + "' and ProjectId='" + dsT.Tables[0].Rows[0]["ProjectId"].ToString() + "'   and status=1 and archive=0 order by id desc";
                DataSet dshourmin = ReadTaskLoadingData(da, strhourminuts);
                if (dshourmin.Tables[0].Rows.Count > 0)
                {
                    transactiontime = dshourmin.Tables[0].Rows[0]["hour"].ToString()+":"+ dshourmin.Tables[0].Rows[0]["minute"].ToString() + ":" + dshourmin.Tables[0].Rows[0]["second"].ToString() + ":" + dshourmin.Tables[0].Rows[0]["millisecond"].ToString();
                    totalminoftask_consume = ConvertTimerToMinutes(transactiontime);
                   }
                if(totalminoftask_consume>0&&totalminoftask>0)
                {
                    retval = Math.Round((totalminoftask_consume / totalminoftask) * 100, 2);
                }
                
            }
        }
        catch(Exception ex) {
            if(id=="2327")
            SaveLogText("getcompletetimeEX="+id+">"+ex.Message);
        }
        //retval = hour + ":" + min + ":" + sec + ":" + mili;
        return retval;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<userlist> binduserlist_byrefid(string id, string utype, string stype)
    {
        DataAccess danew = new DataAccess();
        string struser = "";
        string refids = "";
        if (id != "" && id != "0")
        {
            if (utype == "project")
            {
                struser = "select team,owner from pms_tbl_addproject with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                struser += " and id=" + id;
            }
            else if (utype == "milestone")
            {
                struser = "select team,owner from pms_tbl_addmilestone with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                struser += " and id=" + id;
            }
            else if (utype == "task")
            {
                struser = "select team,owner from pms_tbl_addtask with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                struser += " and id=" + id;
            }
        }
        List<userlist> list = new List<userlist>();
        if (struser != "")
        {
            DataSet druser = danew.GetDataSetInline(struser);
            if (druser.Tables[0].Rows.Count > 0)
            {
                refids = druser.Tables[0].Rows[0][stype].ToString();
            }
            string str2 = "select id,DisplayName,fileupload1,SUBSTRING (DisplayName, 1, 2) as splinameone from tbl_employee with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";

            if (refids != "")
            {
                str2 += " and id in(" + refids + ")";
                DataSet drmile = danew.GetDataSetInline(str2);
                foreach (DataRow row in drmile.Tables[0].Rows)
                {
                    string profile = "";
                    string prosname = "";
                    if (row["fileupload1"].ToString() != "")
                    {
                        profile = row["fileupload1"].ToString();
                    }

                    list.Add(new userlist
                    {
                        id = row["id"].ToString(),
                        name = row["DisplayName"].ToString(),
                        profileimage = profile,
                        shortname = row["splinameone"].ToString()

                    });
                }
            }
        }
        else
        {
            list = binduserlistnew();
        }
        return list;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<statuslist> bindstatuslist()
    {
        string str2 = "select id, ProjectStatus as name, PrintColor as colorname,isnull(iscomplete,0) as iscomplete, isnull(isdefault,0) as isdefault,PrintIcon as statusicon  from Pms_tbl_Projectstatus with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";

        DataAccess danew = new DataAccess();
        DataSet drmile = danew.GetDataSetInline(str2);

        List<statuslist> list = new List<statuslist>();

        foreach (DataRow row in drmile.Tables[0].Rows)
        {
            statuslist obj = new statuslist();
            obj.id = row["id"].ToString();
            obj.name = row["name"].ToString();
            obj.colorname = (row["colorname"].ToString().Trim()==""?"#000": row["colorname"].ToString());
            obj.statusicon = row["statusicon"].ToString();
            obj.iscomplete = row["iscomplete"].ToString();
            obj.isdefault = row["isdefault"].ToString();
            list.Add(obj);
        }

        return list;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<taglist> bindtaglist()
    {
        string str2 = "select id, Tags as name, TagsColor as colorname  from Pms_tbl_Addtags with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";

        DataAccess danew = new DataAccess();
        DataSet drmile = danew.GetDataSetInline(str2);

        List<taglist> list = new List<taglist>();

        foreach (DataRow row in drmile.Tables[0].Rows)
        {
            taglist obj = new taglist();
            obj.id = row["id"].ToString();
            obj.tagname = row["name"].ToString();
            obj.tagcolor = (row["colorname"].ToString().Trim()==""?"#000": row["colorname"].ToString());
            
            list.Add(obj);
        }
        taglist obj1 = new taglist();
        obj1.id ="-1";
        obj1.tagname ="Add New";
        list.Add(obj1);
        return list;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<prioritylist> bindprioritylist()
    {
        string str2 = "select id, name, PrintColor as colorname, isnull(isdefault,0) isdefault from Pms_tbl_Priority with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";

        DataAccess danew = new DataAccess();
        DataSet drmile = danew.GetDataSetInline(str2);

        List<prioritylist> list = new List<prioritylist>();

        foreach (DataRow row in drmile.Tables[0].Rows)
        {
            prioritylist obj = new prioritylist();
            obj.id = row["id"].ToString();
            obj.name = row["name"].ToString();
            obj.colorname = row["colorname"].ToString();
            obj.isdefault = row["isdefault"].ToString();
            list.Add(obj);
        }

        return list;
    }
    protected static string get_task_hrs(string val, string gettype)
    {
        string retval = "";
        try
        {
            string[] ar_val = val.Split(':');
            if (gettype == "hrs")
            {
                retval = ar_val[0];
            }
            else if (gettype == "min")
            {
                if (ar_val.Length > 0)
                {
                    retval = ar_val[1];
                }

            }
        }
        catch { }
        return retval;

    }
    
    public static List<projectfiles> bindfiledata(string pid, string id, string type)
    {

        DataAccess da = new DataAccess();
        string retval = "";


        string strsqlfils = "select * from Pms_tbl_projectfiles with(nolock) where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and status=1 and archive=0 and pid='" + pid + "' and fentryid='" + id + "' and ftype='" + type + "' order by id desc";
        SaveLogText("strsqlfils-" + strsqlfils);
        DataSet dsfils = da.GetDataSetInline(strsqlfils);
        string strdatafils = "";
        int countrow = 0;

        //"<thead>" +
        //"<tr><th></th><th>File</th><th style='width:30px'></th></tr>" +
        //"</thead>";
        List<projectfiles> newprojectfiles = new List<projectfiles>();
        if (dsfils.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dsfils.Tables[0].Rows.Count; i++)
            {
                string filepath = "";
                string ftype = dsfils.Tables[0].Rows[i]["ftype"].ToString();
                string fentryid = dsfils.Tables[0].Rows[i]["fentryid"].ToString();
                if (type != string.Empty)
                {
                    if (type == "milestone")
                        filepath = "/pms/MileImages/" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "/" + fentryid + "/";
                    else if (type == "task")
                        filepath = "/pms/TaskImages/" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "/" + fentryid + "/";
                    else if (type == "subtask")
                        filepath = "/pms/SubtaskImages/" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "/" + fentryid + "/";

                }
                newprojectfiles.Add(new projectfiles
                {

                    id = dsfils.Tables[0].Rows[i]["id"].ToString(),
                    pid = dsfils.Tables[0].Rows[i]["pid"].ToString(),
                    fentryid = dsfils.Tables[0].Rows[i]["fentryid"].ToString(),
                    ftype = dsfils.Tables[0].Rows[i]["ftype"].ToString(),
                    fileurl = dsfils.Tables[0].Rows[i]["fileurl"].ToString(),
                    filedes = dsfils.Tables[0].Rows[i]["filedes"].ToString(),
                    fext = dsfils.Tables[0].Rows[i]["fext"].ToString(),
                    filepath = filepath,


                });

            }

        }


        return newprojectfiles;
    }
    public static List<T> getemployeelist<T>(string ids) where T : IPerson, new()
    {
        if (string.IsNullOrEmpty(ids) || ids == "0")
            return new List<T>();

        string str2 = "select id, DisplayName, fileupload1,UPPER(SUBSTRING(DisplayName,1,1)) as splinameone " +
                      "from tbl_employee " +
                      "where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' " +
                      "and id in(" + ids + ")";

        DataAccess danew = new DataAccess();
        DataSet drmile = danew.GetDataSetInline(str2);

        List<T> list = new List<T>();

        foreach (DataRow row in drmile.Tables[0].Rows)
        {
            T obj = new T();
            obj.id = row["id"].ToString();
            obj.name = row["DisplayName"].ToString();
            obj.profileimage = row["fileupload1"].ToString();
            obj.shortname = row["splinameone"].ToString();
            list.Add(obj);
        }

        return list;
    }
    [WebMethod]
    public static string getleftdays(string id, string gtype)
    {
        string retval = "";

        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        string strsql = "";
        if (gtype == "subtask")
        {
            strsql = "select id,Startdate,Targetdate,team,owner from Pms_tbl_Addsubtask with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + id + "'";
        }
        else if (gtype == "task")
        {
            strsql = "select id,Startdate,Targetdate,team,owner from pms_tbl_addtask with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'   and id='" + id + "'";
        }
        else if (gtype == "milestone")
        {
            strsql = "select id,StartDate,EndDate as Targetdate,team,owner from Pms_tbl_AddMilestone with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  and id='" + id + "'";
        }
        string tleftdays = "";
        #region bind timeline
        //Timeline
        if (strsql != "")
        {
            DataSet dsD = da.GetDataSetInline(strsql);
            double miledayscomplite = 0;


            DateTime mileStartDate = Convert.ToDateTime(dsD.Tables[0].Rows[0]["StartDate"].ToString() == "" ? DateTime.Now.ToString() : dsD.Tables[0].Rows[0]["StartDate"].ToString());
            DateTime mileTargetdate = Convert.ToDateTime(dsD.Tables[0].Rows[0]["Targetdate"].ToString() == "" ? DateTime.Now.ToString() : dsD.Tables[0].Rows[0]["Targetdate"].ToString());
            TimeSpan mileobjTimeSpan = mileTargetdate - mileStartDate;
            double mileDays = Convert.ToDouble(mileobjTimeSpan.TotalDays);
            mileDays = mileDays + 1;


            DateTime mileEnddate = Convert.ToDateTime(dsD.Tables[0].Rows[0]["Targetdate"].ToString() == "" ? DateTime.Now.ToString() : dsD.Tables[0].Rows[0]["Targetdate"].ToString());
            DateTime mileCurrentdate = DateTime.Now;
            TimeSpan mileobjtimeleftspn = mileEnddate - mileCurrentdate;
            double mileleftDays = Convert.ToDouble(mileobjtimeleftspn.Days);


            string mileleftdlydys = "";
            if (mileleftDays < 0)
            {
                mileleftDays = mileleftDays + (-1);
                miledayscomplite = 100;
                mileleftdlydys = "days delayed";
            }
            else
            {
                mileleftDays = mileleftDays + 1;
                miledayscomplite = Math.Round(((mileDays - mileleftDays) * 100) / mileDays, 2);
                mileleftdlydys = "days left";
            }
            //start days months  
            string milestartmonthname = "";
            int mileststartdys = mileStartDate.Day;
            int milestrtmonth = mileStartDate.Month;
            int milestonestartyear = mileStartDate.Year;
            //end days months 
            string mileendmonthname = "";
            int mileenddys = mileTargetdate.Day;
            int mileendmonth = mileTargetdate.Month;
            int milestoneEndyear = mileTargetdate.Year;
            if (mileleftdlydys != "")
            {
                tleftdays = mileleftDays.ToString().Replace("-", "") + " " + mileleftdlydys;
            }
            retval = tleftdays;
        }

        #endregion
        return retval;
    }
    [WebMethod]    
    public static string SaveTags(string projectid = "", string milestoneid = "",string taskid = "", string subtaskid = "", string Tags = "", string TagsColor = "",string Tagstype="")
    {
        string retval = "0";

        DataAccess da1 = new DataAccess();
        string editid = projectid;
        string entrytype = "Insert";
        SqlParameter[] prm;
        try
        {
            
                prm = new SqlParameter[13];
                prm[0] = new SqlParameter("@id", editid);
                prm[1] = new SqlParameter("@ProjectId", projectid);
                if(milestoneid!=string.Empty)
                prm[2] = new SqlParameter("@Milestoneid", milestoneid);
                else
                prm[2] = new SqlParameter("@Milestoneid", DBNull.Value);
                if (taskid != string.Empty)
                    prm[3] = new SqlParameter("@Taskid", taskid);
                else
                    prm[3] = new SqlParameter("@Taskid", DBNull.Value);
                if (subtaskid != string.Empty)
                    prm[4] = new SqlParameter("@Subtaskid", subtaskid);
                else
                    prm[4] = new SqlParameter("@Subtaskid", DBNull.Value);
                
                prm[5] = new SqlParameter("@tagtype", Tagstype);
                prm[6] = new SqlParameter("@Tags", Tags);
                prm[7] = new SqlParameter("@TagsColor", "#" + TagsColor);
                prm[8] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prm[9] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                prm[10] = new SqlParameter("@cdate", da1.getcdatetime());
                prm[11] = new SqlParameter("@type", entrytype);
                prm[12] = new SqlParameter("ReturnValue", SqlDbType.Int);
                prm[12].Direction = ParameterDirection.ReturnValue;
                retval = Convert.ToString(da1.executeDMLQuery("pmstbl_AddTags_usp", prm));
                if(retval!=string .Empty&& retval !="0")
                {
                 string sqltag = "update pms_tbl_addsubtask set tagids=CASE WHEN ISNULL(tagids,'') ='' THEN '" + retval + "' ELSE tagids + '," + retval +"' END where id='" + subtaskid + "' and regid='"+ HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                if(Tagstype=="task")
                    sqltag = "update pms_tbl_addtask set tagids=CASE WHEN ISNULL(tagids,'') ='' THEN '" + retval + "' ELSE tagids + '," + retval + "' END where id='" + subtaskid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                if (Tagstype == "milestone")
                    sqltag = "update pms_tbl_addmilestone set tagids=CASE WHEN ISNULL(tagids,'') ='' THEN '" + retval + "' ELSE tagids + '," + retval + "' END where id='" + subtaskid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                da1.executeDMLQueryinline(sqltag);
                }
                //retval = "1";
            


        }
        catch
        {
            retval = "0";
        }
        return retval;



    }
    [WebMethod]
    public static string SaveTags_new(string Tags = "", string TagsColor = "")
    {
        string retval = "0";

        DataAccess da1 = new DataAccess();
        string entrytype = "Insert";
        SqlParameter[] prm;
        try
        {

            prm = new SqlParameter[8];
            prm[0] = new SqlParameter("@id","0");           
            prm[1] = new SqlParameter("@Tags", Tags);
            prm[2] = new SqlParameter("@TagsColor", "#" + TagsColor);
            prm[3] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[4] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prm[5] = new SqlParameter("@cdate", da1.getcdatetime());
            prm[6] = new SqlParameter("@type", entrytype);
            prm[7] = new SqlParameter("ReturnValue", SqlDbType.Int);
            prm[7].Direction = ParameterDirection.ReturnValue;
            retval = Convert.ToString(da1.executeDMLQuery("pmstbl_AddTags_usp", prm));
            
            //retval = "1";



        }
        catch
        {
            retval = "0";
        }
        return retval;



    }


    [WebMethod]
    public static string SaveTags_update_Multi(string taskids,string tagids = "")
    {
        string retval = "0";

        DataAccess da1 = new DataAccess();
        string entrytype = "Insert";
        SqlParameter[] prm;
        try
        {
            if (taskids == string.Empty)
                return "";
            string[] ar_tsks = taskids.Split(',');
            for (int S = 0; S < ar_tsks.Length; S++)
            {

                string strtaskdetail = ar_tsks[S];
                string[] ar_dt = strtaskdetail.Split(':');
                string taskid = ar_dt[0];
                string tasktype = ar_dt[1];
                string sqlupdate = "update pms_tbl_addsubtask set tagids='"+ tagids + "' where regid='"+ HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='"+ taskid + "'";
                if (tasktype == "task")
                    sqlupdate = "update pms_tbl_addtask set tagids='" + tagids + "' where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + taskid + "'";
                else if (tasktype == "milestone")
                    sqlupdate = "update pms_tbl_addsubtask set tagids='" + tagids + "' where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + taskid + "'";
                da1.executeDMLQueryinline(sqlupdate);
                  
            }
            //project  log details insert end
            retval = "1";

        }
        catch { retval = "0"; }
        return retval;



    }


    [WebMethod]
    public static string UpdateP_tags(string id = "",string tasktype="", string ids = "")
    {
        string retval = "0";
        try
        {
            DataAccess da1 = new DataAccess();
            string finsql4 = "update pms_tbl_addsubtask set tagids='" + ids + "' where id='" + id + "'  and status=1 and archive=0";
            if(tasktype=="task")
            {
                finsql4 = "update pms_tbl_addtask set tagids='" + ids + "' where id='" + id + "'  and status=1 and archive=0";
            }
            else if (tasktype == "milestone")
            {
                finsql4 = "update pms_tbl_addmilestone set tagids='" + ids + "' where id='" + id + "'  and status=1 and archive=0";
            }
            DataSet dsfindid4 = da1.GetDataSetInline(finsql4);
            retval = "1";
        }
        catch { }
        return retval;
    }
    [WebMethod]
    public static string removetagbyid(removetags removetags)
    {
        string retval = "";
        DataAccess da = new DataAccess();
        if (removetags.id != null)
        {

            try
            {
                string strsqlown = "select id,tagids from pms_tbl_addsubtask where status=1 and archive=0 and regid='"
                + HttpContext.Current.Request.Cookies["cadmin"]["id"] +
                "' and id='" + removetags.id + "' ";
                if(removetags.removetype =="task")
                    strsqlown = "select id,tagids from pms_tbl_addask where status=1 and archive=0 and regid='"
                + HttpContext.Current.Request.Cookies["cadmin"]["id"] +
                "' and id='" + removetags.id + "' ";
                if (removetags.removetype == "milestone")
                    strsqlown = "select id,tagids from pms_tbl_addmilestone where status=1 and archive=0 and regid='"
                + HttpContext.Current.Request.Cookies["cadmin"]["id"] +
                "' and id='" + removetags.id + "' ";
                DataSet dsown = da.GetDataSetInline(strsqlown);

                if (dsown.Tables[0].Rows.Count > 0)
                {
                    string teamStr = dsown.Tables[0].Rows[0]["tagids"].ToString();

                    if (!string.IsNullOrEmpty(teamStr))
                    {
                        // Split
                        List<string> tags = teamStr.Split(',').ToList();

                        // Remove matching ID
                        tags.Remove(removetags.tagname.ToString());

                        // Join back
                        string updatedteams = string.Join(",", tags);

                        // Update DB
                        string updateQuery = "update pms_tbl_addsubtask set tagids='" + updatedteams +
                                             "' where id='" + removetags.id + "'";
                        if (removetags.removetype == "task")
                            updateQuery = "update pms_tbl_addtask set tagids='" + updatedteams +
                                             "' where id='" + removetags.id + "'";
                        if (removetags.removetype == "milestone")
                            updateQuery = "update pms_tbl_addmilestone set tagids='" + updatedteams +
                                             "' where id='" + removetags.id + "'";
                        da.executeDMLQueryinline(updateQuery);
                        retval = "1";

                    }
                }
            }
            catch { }
        }


        return retval;
    }
    [WebMethod]
    public static string removetagbyid(string id,string removetype, string tagids)
    {
        string retval = "0";

        DataAccess da1 = new DataAccess();
        
        SqlParameter[] prm;
        try
        {
            string sql = "update pms_tbl_addsubtask set tagids='"+ tagids + "'  where id='" + id+"' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
            if (removetype == "task")
                sql = "update pms_tbl_addtask set tagids='" + tagids + "'  where id='" + id + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
            if (removetype == "milestone")
                sql = "update pms_tbl_addmilestone set tagids='" + tagids + "'  where id='" + id + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
            SaveLogText("removetagbyid-"+ sql);
            da1.executeDMLQueryinline(sql);
            retval = "1";

        }
        catch
        {
            retval = "0";
        }
        return retval;



    }
    #region update status
    [WebMethod]
    public static string UpdateMilestone_Status(string milstoneid = "", string statusid = "", string iscomplete = "")
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (milstoneid != "" && milstoneid != "0")
        {
            entrytype = "Update";
        }

        // percentage update start  // 
        if (iscomplete == "1")
        {
            string independency = checkdependency("milestone", milstoneid);
            if (independency == "1")
            {
                retval = "Please complete first dependency of this? ";
            }
            else
            {
                SqlParameter[] prm = new SqlParameter[8];
                prm[0] = new SqlParameter("@id", milstoneid);
                prm[1] = new SqlParameter("@milstoneStatus", statusid);
                prm[2] = new SqlParameter("@iscomplete", iscomplete);
                prm[3] = new SqlParameter("@iscompletedate", da1.getcdatetime());
                prm[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prm[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                prm[6] = new SqlParameter("@cdate", da1.getcdatetime());
                prm[7] = new SqlParameter("@type", entrytype);
                da1.executeDMLQuery("pmstbl_Status_milstone_Update_usp", prm);
                decimal milestonetotalpercent = 100;
                decimal totlweitagemile = 0;
                string finsql1 = "select * from Pms_tbl_AddMilestone where id=" + milstoneid + " and status=1  and archive=0";
                DataSet dsfindid1 = da1.GetDataSetInline(finsql1);

                // Milestone percentage //
                string strstatusupdate = "update Pms_tbl_AddMilestone set iscompletepercentage='" + milestonetotalpercent.ToString() + "' where id='" + milstoneid + "' and status=1 and archive=0 and isnull(iscomplete,0)=1";
                DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                // project percentage //
                string finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1  and archive=0";
                DataSet dsfindid2 = da1.GetDataSetInline(finsql2);
                for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
                {
                    decimal sumweitage = 0;
                    decimal subpercentageemile = 0;
                    sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
                    subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                    totlweitagemile += (sumweitage * subpercentageemile) / 100;

                }

                string finsql4 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitagemile.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
                DataSet dsfindid4 = da1.GetDataSetInline(finsql4);

                //project  log details insert start
                SqlParameter[] prmlog = new SqlParameter[8];
                prmlog[0] = new SqlParameter("@Transid", milstoneid);
                prmlog[1] = new SqlParameter("@Logstatus", statusid);
                prmlog[2] = new SqlParameter("@LogDate", da1.getcdatetime());
                prmlog[3] = new SqlParameter("@Logtype", "milestone");
                prmlog[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prmlog[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                prmlog[6] = new SqlParameter("@cdate", da1.getcdatetime());
                prmlog[7] = new SqlParameter("@type", "Insert");
                da1.executeDMLQuery("pmstbl_Loginsert_usp", prmlog);
                //project  log details insert end
                retval = "1";
                //DataAccess da = new DataAccess();
                //string strsql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus where  status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + statusid + "' ";
                //DataSet dscode = da.GetDataSetInline(strsql);
                //if (dscode.Tables[0].Rows.Count > 0)
                //{
                //    retval = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
                //}
            }


            //string chkval = managecalculation(milstoneid, "milestone");

        }
        else
        {
            SqlParameter[] prm = new SqlParameter[8];
            prm[0] = new SqlParameter("@id", milstoneid);
            prm[1] = new SqlParameter("@milstoneStatus", statusid);
            prm[2] = new SqlParameter("@iscomplete", iscomplete);
            prm[3] = new SqlParameter("@iscompletedate", da1.getcdatetime());
            prm[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prm[6] = new SqlParameter("@cdate", da1.getcdatetime());
            prm[7] = new SqlParameter("@type", entrytype);
            da1.executeDMLQuery("pmstbl_Status_milstone_Update_usp", prm);
            decimal milestonetotalpercent = 0;
            decimal totlweitagemile = 0;
            string finsql1 = "select * from Pms_tbl_AddMilestone where id=" + milstoneid + " and status=1  and archive=0";
            DataSet dsfindid1 = da1.GetDataSetInline(finsql1);

            // Milestone percentage //
            string strstatusupdate = "update Pms_tbl_AddMilestone set iscompletepercentage='" + milestonetotalpercent.ToString() + "' where id='" + milstoneid + "' and status=1 and archive=0 and isnull(iscomplete,0)=0";
            DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

            if (dsfindid1.Tables[0].Rows[0]["istask"].ToString() == "1")
            {

                string sqlCW = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where  Milestoneid='" + milstoneid + "'  and status=1  and archive=0 ";
                DataSet dsCW = da1.GetDataSetInline(sqlCW);
                for (int k = 0; k < dsCW.Tables[0].Rows.Count; k++)
                {
                    decimal sumweitage = 0;
                    decimal subpercentageemile = 0;
                    sumweitage = Convert.ToDecimal(dsCW.Tables[0].Rows[k]["Weightage"].ToString());
                    subpercentageemile = Convert.ToDecimal(dsCW.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                    totlweitagemile += (sumweitage * subpercentageemile) / 100;

                }
                string strupdate = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile.ToString() + "' where id='" + milstoneid + "' and status=1 and archive=0 ";
                SaveLogText("updatestatusA=" + strupdate);
                da1.executeDMLQueryinline(strupdate);

            }
            // project percentage //
            string finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1  and archive=0";
            DataSet dsfindid2 = da1.GetDataSetInline(finsql2);
            for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
            {
                decimal sumweitage = 0;
                decimal subpercentageemile = 0;
                sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
                subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                totlweitagemile += (sumweitage * subpercentageemile) / 100;

            }

            string finsql4 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitagemile.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da1.GetDataSetInline(finsql4);
            //project  log details insert start
            SqlParameter[] prmlog = new SqlParameter[8];
            prmlog[0] = new SqlParameter("@Transid", milstoneid);
            prmlog[1] = new SqlParameter("@Logstatus", statusid);
            prmlog[2] = new SqlParameter("@LogDate", da1.getcdatetime());
            prmlog[3] = new SqlParameter("@Logtype", "milestone");
            prmlog[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prmlog[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prmlog[6] = new SqlParameter("@cdate", da1.getcdatetime());
            prmlog[7] = new SqlParameter("@type", "Insert");
            da1.executeDMLQuery("pmstbl_Loginsert_usp", prmlog);
            //project  log details insert end
            retval = "1";



        }
        // percentage update end  // 





        // return dscode.GetXml();
        return retval;
    }
    [WebMethod]
    public static string UpdateTask_Status(string taskid = "", string statusid = "", string iscomplete = "")
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (taskid != "" && taskid != "0")
        {
            entrytype = "Update";
        }




        // percentage update start  // 
        if (iscomplete == "1")
        {
            string independency = checkdependency("task", taskid);
            if (independency == "1")
            {
                retval = "Please complete first dependency of this? ";
            }
            else
            {
                SqlParameter[] prm = new SqlParameter[8];
                prm[0] = new SqlParameter("@id", taskid);
                prm[1] = new SqlParameter("@ProjectStatus", statusid);
                prm[2] = new SqlParameter("@iscomplete", iscomplete);
                prm[3] = new SqlParameter("@iscompletedate", da1.getcdatetime());
                prm[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prm[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                prm[6] = new SqlParameter("@cdate", da1.getcdatetime());
                prm[7] = new SqlParameter("@type", entrytype);
                da1.executeDMLQuery("pmstbl_task_Status_DeatilsUpdate", prm);
                decimal tsktotalpercent = 100;
                decimal totlweitagemile = 0;
                decimal totlweitageproject = 0;
                string finsql1 = "select * from Pms_tbl_Addtask where id=" + taskid + " and status=1 and archive=0";
                DataSet dsfindid1 = da1.GetDataSetInline(finsql1);

                // task percentage //
                string strstatusupdate = "update Pms_tbl_Addtask set iscompletepercentage='" + tsktotalpercent.ToString() + "' where id='" + taskid + "' and status=1 and archive=0 and isnull(iscomplete,0)=1";
                DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                // milestone percentage //
                string finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'   and status=1  and archive=0";
                DataSet dsfindid2 = da1.GetDataSetInline(finsql2);
                for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
                {
                    decimal sumweitage = 0;
                    decimal subpercentageemile = 0;
                    sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
                    subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                    totlweitagemile += (sumweitage * subpercentageemile) / 100;

                }

                string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile.ToString() + "' where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and id='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1 and archive=0";
                DataSet dsfindid4 = da1.GetDataSetInline(finsql4);

                // project percentage //

                string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1  and archive=0";
                DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
                for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
                {
                    decimal sumweitageprojt = 0;
                    decimal subpercentageeproject = 0;
                    sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                    subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                    totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

                }

                string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
                DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);



                //project  log details insert start
                SqlParameter[] prmlog = new SqlParameter[8];
                prmlog[0] = new SqlParameter("@Transid", taskid);
                prmlog[1] = new SqlParameter("@Logstatus", statusid);
                prmlog[2] = new SqlParameter("@LogDate", da1.getcdatetime());
                prmlog[3] = new SqlParameter("@Logtype", "task");
                prmlog[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prmlog[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                prmlog[6] = new SqlParameter("@cdate", da1.getcdatetime());
                prmlog[7] = new SqlParameter("@type", "Insert");
                da1.executeDMLQuery("pmstbl_Loginsert_usp", prmlog);
                //project  log details insert end
                retval = "1";
                //string strsql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + statusid + "' ";
                //DataSet dscode = da1.GetDataSetInline(strsql);
                //if (dscode.Tables[0].Rows.Count > 0)
                //{
                //    retval = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
                //}
            }


        }
        else
        {
            SqlParameter[] prm = new SqlParameter[8];
            prm[0] = new SqlParameter("@id", taskid);
            prm[1] = new SqlParameter("@ProjectStatus", statusid);
            prm[2] = new SqlParameter("@iscomplete", iscomplete);
            prm[3] = new SqlParameter("@iscompletedate", da1.getcdatetime());
            prm[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prm[6] = new SqlParameter("@cdate", da1.getcdatetime());
            prm[7] = new SqlParameter("@type", entrytype);
            da1.executeDMLQuery("pmstbl_task_Status_DeatilsUpdate", prm);
            decimal tsktotalpercent = 0;
            decimal totlweitagemile = 0;
            decimal totlweitageproject = 0;

            string finsql1 = "select * from Pms_tbl_Addtask where id=" + taskid + " and status=1  and archive=0";
            DataSet dsfindid1 = da1.GetDataSetInline(finsql1);

            // task percentage //
            if (dsfindid1.Tables[0].Rows.Count > 0)
            {
                if (dsfindid1.Tables[0].Rows[0]["issubtask"].ToString() == "1")
                {

                    string finsql211 = "select isnull(sum(weightage),0)Wattagetasksum from Pms_tbl_Addsubtask where   task='" + taskid + "'  and status=1 and isnull(iscomplete,0)=1 and archive=0";
                    DataSet dsfindid211 = da1.GetDataSetInline(finsql211);
                    if (dsfindid211.Tables[0].Rows.Count > 0)
                    {
                        string strstatusupdate = "update Pms_tbl_Addtask set iscompletepercentage='" + dsfindid211.Tables[0].Rows[0]["Wattagetasksum"] + "' where id='" + taskid + "' and status=1 and archive=0";
                        DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);
                    }
                }
                else
                {
                    string strstatusupdate = "update Pms_tbl_Addtask set iscompletepercentage='0' where id='" + taskid + "' and status=1 and archive=0";
                    DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                }

            }


            // milestone percentage //

            string finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1  and archive=0";
            DataSet dsfindid2 = da1.GetDataSetInline(finsql2);
            for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
            {
                decimal sumweitage = 0;
                decimal subpercentageemile = 0;
                sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
                subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                totlweitagemile += (sumweitage * subpercentageemile) / 100;

            }

            string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile + "' where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and id='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da1.GetDataSetInline(finsql4);

            // project percentage //

            string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'   and status=1  and archive=0";
            DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
            for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
            {
                decimal sumweitageprojt = 0;
                decimal subpercentageeproject = 0;
                sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

            }

            string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
            DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);



            //project  log details insert start
            SqlParameter[] prmlog = new SqlParameter[8];
            prmlog[0] = new SqlParameter("@Transid", taskid);
            prmlog[1] = new SqlParameter("@Logstatus", statusid);
            prmlog[2] = new SqlParameter("@LogDate", da1.getcdatetime());
            prmlog[3] = new SqlParameter("@Logtype", "task");
            prmlog[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prmlog[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prmlog[6] = new SqlParameter("@cdate", da1.getcdatetime());
            prmlog[7] = new SqlParameter("@type", "Insert");
            da1.executeDMLQuery("pmstbl_Loginsert_usp", prmlog);
            //project  log details insert end
            retval = "1";
            //string strsql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + statusid + "' ";
            //DataSet dscode = da1.GetDataSetInline(strsql);
            //if (dscode.Tables[0].Rows.Count > 0)
            //{
            //    retval = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
            //}

        }

        // percentage update end  // 




        return retval;
    }

    [WebMethod]
    public static string UpdateSubtask_Status(string subtaskid = "", string statusid = "", string iscomplete = "")
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";

        if (iscomplete == "1")
        {
            string independency = checkdependency("subtask", subtaskid);
            if (independency == "1")
            {
                retval = "Please complete first dependency of this? ";
            }
            else
            {

                SqlParameter[] prm = new SqlParameter[8];
                prm[0] = new SqlParameter("@id", subtaskid);
                prm[1] = new SqlParameter("@ProjectStatus", statusid);
                prm[2] = new SqlParameter("@iscomplete", iscomplete);
                prm[3] = new SqlParameter("@iscompletedate", da1.getcdatetime());
                prm[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prm[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                prm[6] = new SqlParameter("@cdate", da1.getcdatetime());
                prm[7] = new SqlParameter("@type", entrytype);
                da1.executeDMLQuery("pmstbl_subtask_Status_DeatilsUpdate", prm);


                decimal subtsktotalpercent = 100;
                decimal totlweitagemile = 0;
                decimal totlweitageproject = 0;

                string finsql1 = "select * from Pms_tbl_Addsubtask where id=" + subtaskid + " and status=1  and archive=0";
                DataSet dsfindid1 = da1.GetDataSetInline(finsql1);
                SaveLogText("finsql2-" + finsql1);
                string finsql2 = "select isnull(sum(weightage),0)Wattagetasksum from Pms_tbl_Addsubtask where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "' and task='" + dsfindid1.Tables[0].Rows[0]["task"] + "'  and status=1 and  archive=0 and isnull(iscomplete,0)=1";

                DataSet dsfindid2 = da1.GetDataSetInline(finsql2);

                //sub task percentage //
                string strstatusupdate = "update Pms_tbl_Addsubtask set iscompletepercentage='" + subtsktotalpercent.ToString() + "' where id='" + subtaskid + "' and status=1 and archive=0 and isnull(iscomplete,0)=1";
                DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                // task percentage //
                string finsql3 = "update Pms_tbl_Addtask set iscompletepercentage='" + dsfindid2.Tables[0].Rows[0]["Wattagetasksum"] + "' where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "' and id='" + dsfindid1.Tables[0].Rows[0]["task"] + "' and status=1 and archive=0";
                DataSet dsfindid3 = da1.GetDataSetInline(finsql3);


                // Milestone percentage //
                string findsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1  and archive=0";
                DataSet dsfindidmile = da1.GetDataSetInline(findsqlmile);
                for (int j = 0; j < dsfindidmile.Tables[0].Rows.Count; j++)
                {
                    decimal sumweitage = 0;
                    decimal subpercentageemile = 0;
                    sumweitage = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["Weightage"].ToString());
                    subpercentageemile = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                    totlweitagemile += (sumweitage * subpercentageemile) / 100;

                }

                string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile + "' where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and id='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1 and archive=0";
                DataSet dsfindid4 = da1.GetDataSetInline(finsql4);

                // project percentage //
                string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'   and status=1  and archive=0";
                DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
                for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
                {
                    decimal sumweitageprojt = 0;
                    decimal subpercentageeproject = 0;
                    sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                    subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                    totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

                }

                string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'   and status=1 and archive=0";
                DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);
                //project  log details insert start
                SqlParameter[] prmlog = new SqlParameter[8];
                prmlog[0] = new SqlParameter("@Transid", subtaskid);
                prmlog[1] = new SqlParameter("@Logstatus", statusid);
                prmlog[2] = new SqlParameter("@LogDate", da1.getcdatetime());
                prmlog[3] = new SqlParameter("@Logtype", "subtask");
                prmlog[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prmlog[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                prmlog[6] = new SqlParameter("@cdate", da1.getcdatetime());
                prmlog[7] = new SqlParameter("@type", "Insert");
                da1.executeDMLQuery("pmstbl_Loginsert_usp", prmlog);
                //project  log details insert end


                retval = "1";

                //string strsql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + statusid + "' ";
                //DataSet dscode = da1.GetDataSetInline(strsql);
                //if (dscode.Tables[0].Rows.Count > 0)
                //{
                //    retval = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
                //}
            }


        }
        else
        {
            decimal subtsktotalpercent = 0;
            decimal totlweitagemile = 0;
            decimal totlweitageproject = 0;
            SqlParameter[] prm = new SqlParameter[8];
            prm[0] = new SqlParameter("@id", subtaskid);
            prm[1] = new SqlParameter("@ProjectStatus", statusid);
            prm[2] = new SqlParameter("@iscomplete", iscomplete);
            prm[3] = new SqlParameter("@iscompletedate", da1.getcdatetime());
            prm[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prm[6] = new SqlParameter("@cdate", da1.getcdatetime());
            prm[7] = new SqlParameter("@type", entrytype);
            da1.executeDMLQuery("pmstbl_subtask_Status_DeatilsUpdate", prm);
            string finsql1 = "select * from Pms_tbl_Addsubtask where id=" + subtaskid + " and status=1  and archive=0";
            DataSet dsfindid1 = da1.GetDataSetInline(finsql1);

            string finsql2 = "select isnull(sum(weightage),0)Wattagetasksum from Pms_tbl_Addsubtask where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "' and task='" + dsfindid1.Tables[0].Rows[0]["task"] + "'  and status=1 and isnull(iscomplete,0)=1 and archive=0";
            DataSet dsfindid2 = da1.GetDataSetInline(finsql2);

            //sub task percentage //
            string strstatusupdate = "update Pms_tbl_Addsubtask set iscompletepercentage='" + subtsktotalpercent.ToString() + "' where id='" + subtaskid + "' and status=1 and archive=0";
            DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

            //task percentage //
            string finsql3 = "update Pms_tbl_Addtask set iscompletepercentage='" + dsfindid2.Tables[0].Rows[0]["Wattagetasksum"] + "' where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "' and id='" + dsfindid1.Tables[0].Rows[0]["task"] + "' and status=1 and archive=0";
            DataSet dsfindid3 = da1.GetDataSetInline(finsql3);

            //milestone percentage //
            string findsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'    and status=1  and archive=0";
            DataSet dsfindidmile = da1.GetDataSetInline(findsqlmile);
            for (int j = 0; j < dsfindidmile.Tables[0].Rows.Count; j++)
            {
                decimal sumweitage = 0;
                decimal subpercentageemile = 0;
                sumweitage = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["Weightage"].ToString());
                subpercentageemile = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                totlweitagemile += (sumweitage * subpercentageemile) / 100;

            }

            string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile + "' where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and id='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da1.GetDataSetInline(finsql4);


            // project percentage //
            string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'   and status=1  and archive=0";
            DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
            for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
            {
                decimal sumweitageprojt = 0;
                decimal subpercentageeproject = 0;
                sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

            }

            string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'   and status=1 and archive=0";
            DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);
            //project  log details insert start
            SqlParameter[] prmlog = new SqlParameter[8];
            prmlog[0] = new SqlParameter("@Transid", subtaskid);
            prmlog[1] = new SqlParameter("@Logstatus", statusid);
            prmlog[2] = new SqlParameter("@LogDate", da1.getcdatetime());
            prmlog[3] = new SqlParameter("@Logtype", "subtask");
            prmlog[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prmlog[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prmlog[6] = new SqlParameter("@cdate", da1.getcdatetime());
            prmlog[7] = new SqlParameter("@type", "Insert");
            da1.executeDMLQuery("pmstbl_Loginsert_usp", prmlog);
            //project  log details insert end
            retval = "1";


        }
        // percentage update end  // 




        // Critical Workflow Step 2: status successfully changed.
        if (retval == "1")
        {
            //CheckCriticalWorkflow("subtask", subtaskid);
        }

        return retval;
    }


    #endregion
    [WebMethod]
    public static string UpdateSubtask_StatusMulti(string taskids = "", string statusid = "", string iscomplete = "")
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (taskids == string.Empty)
        {
            return "";
        }
        try
        {
            if (taskids == string.Empty)
                return "";
            string[] ar_tsks = taskids.Split(',');
            for (int S = 0; S < ar_tsks.Length; S++)
            {
                string strtaskdetail = ar_tsks[S];
                string[] ar_dt = strtaskdetail.Split(':');
                string taskid = ar_dt[0];
                string tasktype = ar_dt[1];
                if (tasktype == "subtask")
                    UpdateSubtask_Status(taskid, statusid, iscomplete);
                else if (tasktype == "task")
                    UpdateTask_Status(taskid, statusid, iscomplete);
                else if (tasktype == "milestone")
                    UpdateMilestone_Status(taskid, statusid, iscomplete);
            }
            //project  log details insert end
            retval = "1";

        }
        catch { }


        return retval;
    }

    // =========================================================
    // KANBAN PRIORITY DROP
    // Isolated from the existing status/timer save flow.
    // =========================================================
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string UpdateKanbanPriority(string id = "", string tasktype = "", string priorityid = "")
    {
        try
        {
            int entityId;
            int priorityId;
            int regid;

            if (!Int32.TryParse(id, out entityId) || entityId <= 0)
                return "Invalid task.";

            if (!Int32.TryParse(priorityid, out priorityId) || priorityId <= 0)
                return "Invalid priority.";

            HttpCookie regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null || regCookie["id"] == null ||
                !Int32.TryParse(regCookie["id"].ToString(), out regid) || regid <= 0)
                return "Invalid company.";

            string type = (tasktype ?? "").Trim().ToLowerInvariant();
            string tableName = "";

            if (type == "milestone")
                tableName = "Pms_tbl_AddMilestone";
            else if (type == "task")
                tableName = "Pms_tbl_Addtask";
            else if (type == "subtask")
                tableName = "Pms_tbl_Addsubtask";
            else
                return "Invalid task type.";

            DataAccess da = new DataAccess();

            DataSet dsPriority = da.GetDataSetInline(
                "select top(1) id from Pms_tbl_Priority with(nolock) " +
                "where id=" + priorityId + " and regid=" + regid + " and status=1 and archive=0"
            );

            if (dsPriority.Tables.Count == 0 || dsPriority.Tables[0].Rows.Count == 0)
                return "Priority not found.";

            DataSet dsEntity = da.GetDataSetInline(
                "select top(1) id from " + tableName + " with(nolock) " +
                "where id=" + entityId + " and regid=" + regid + " and status=1 and archive=0"
            );

            if (dsEntity.Tables.Count == 0 || dsEntity.Tables[0].Rows.Count == 0)
                return "Task not found.";

            da.executeDMLQueryinline(
                "update " + tableName + " set Priority=" + priorityId +
                " where id=" + entityId + " and regid=" + regid + " and status=1 and archive=0"
            );

            return "1";
        }
        catch (Exception ex)
        {
            SaveLogText("UpdateKanbanPriority EX-" + ex.Message);
            return "Priority update failed.";
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<userlist> binduserlist()
    {
        string str2 = "select id,DisplayName,fileupload1,SUBSTRING (DisplayName, 1, 2) as splinameone from tbl_employee where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
        DataAccess danew = new DataAccess();
        DataSet drmile = danew.GetDataSetInline(str2);
        List<userlist> list = new List<userlist>();
        foreach (DataRow row in drmile.Tables[0].Rows)
        {
            string profile = "";
            string prosname = "";
            if (row["fileupload1"].ToString() != "")
            {
                profile = row["fileupload1"].ToString();
            }

            list.Add(new userlist
            {
                id = row["id"].ToString(),
                name = row["DisplayName"].ToString(),
                profileimage = profile,
                shortname = row["splinameone"].ToString()

            });
        }
        return list;
    }
    public static List<userlist> binduserlistnew()
    {
        string str2 = "select id,DisplayName,fileupload1,SUBSTRING (DisplayName, 1, 2) as splinameone from tbl_employee where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
        DataAccess danew = new DataAccess();
        DataSet drmile = danew.GetDataSetInline(str2);
        List<userlist> list = new List<userlist>();
        foreach (DataRow row in drmile.Tables[0].Rows)
        {
            string profile = "";
            string prosname = "";
            if (row["fileupload1"].ToString() != "")
            {
                profile = row["fileupload1"].ToString();
            }

            list.Add(new userlist
            {
                id = row["id"].ToString(),
                name = row["DisplayName"].ToString(),
                profileimage = profile,
                shortname = row["splinameone"].ToString()

            });
        }
        return list;
    }
    public static List<taglist> gettasktaglist(string id,string selecttype="")
    {
        string tagid = "";
        DataAccess danew = new DataAccess();
        string sqlselectid = "";
        DataSet dsSelect = new DataSet();
        
            try
            {


                sqlselectid = "select tagids from pms_tbl_addsubtask  with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + id + "' ";
            if (selecttype == "task")
              sqlselectid = "select tagids from pms_tbl_addtask  with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + id + "' ";
            if (selecttype == "milestone")
                sqlselectid = "select tagids from pms_tbl_addmilestone  with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + id + "' ";

            dsSelect = danew.GetDataSetInline(sqlselectid);
                tagid = dsSelect.Tables[0].Rows[0]["tagids"].ToString();
            }
            catch { }
        
        List<taglist> list = new List<taglist>();
        string str2 = "select * from Pms_tbl_Addtags  with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id in(" + tagid + ") ";
        
        try
        {
            DataSet drmile = danew.GetDataSetInline(str2);

            foreach (DataRow row in drmile.Tables[0].Rows)
            {

                list.Add(new taglist
                {
                    id = row["id"].ToString(),
                    tagname = row["Tags"].ToString(),
                    tagcolor = row["TagsColor"].ToString(),


                });
            }

        }
        catch { }
        return list;
    }
    public static List<taglist> gettasktaglist_old(string id,string tagtype)
    {
        string str2 = "select * from Pms_tbl_Addtags where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and Subtaskid='"+id+"' and tagtype='subtask'";
        
        DataAccess danew = new DataAccess();
        DataSet drmile = danew.GetDataSetInline(str2);
        List<taglist> list = new List<taglist>();
        foreach (DataRow row in drmile.Tables[0].Rows)
        {            

            list.Add(new taglist
            {
                id = row["id"].ToString(),
                tagname = row["Tags"].ToString(),
                tagcolor = row["TagsColor"].ToString(),


            });
        }
        return list;
    }
    public static string getleftdays_load(string id, string gtype)
    {
        string retval = "";

        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        string strsql = "";
        try
        {
            if (gtype == "subtask")
            {
                strsql = "select id,Startdate,Targetdate,team,owner from Pms_tbl_Addsubtask where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + id + "'";
            }
            else if (gtype == "task")
            {
                strsql = "select id,Startdate,Targetdate,team,owner from pms_tbl_addtask where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'   and id='" + id + "'";
            }
            else if (gtype == "milestone")
            {
                strsql = "select id,StartDate,EndDate as Targetdate,team,owner from Pms_tbl_AddMilestone where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  and id='" + id + "'";
            }
            string tleftdays = "";
            #region bind timeline
            //Timeline
            if (strsql != "")
            {
                DataSet dsD = da.GetDataSetInline(strsql);
                double miledayscomplite = 0;


                DateTime mileStartDate = Convert.ToDateTime(dsD.Tables[0].Rows[0]["StartDate"].ToString() == "" ? DateTime.Now.ToString() : dsD.Tables[0].Rows[0]["StartDate"].ToString());
                DateTime mileTargetdate = Convert.ToDateTime(dsD.Tables[0].Rows[0]["Targetdate"].ToString() == "" ? DateTime.Now.ToString() : dsD.Tables[0].Rows[0]["Targetdate"].ToString());
                TimeSpan mileobjTimeSpan = mileTargetdate - mileStartDate;
                double mileDays = Convert.ToDouble(mileobjTimeSpan.TotalDays);
                mileDays = mileDays + 1;


                DateTime mileEnddate = Convert.ToDateTime(dsD.Tables[0].Rows[0]["Targetdate"].ToString() == "" ? DateTime.Now.ToString() : dsD.Tables[0].Rows[0]["Targetdate"].ToString());
                DateTime mileCurrentdate = DateTime.Now;
                TimeSpan mileobjtimeleftspn = mileEnddate - mileCurrentdate;
                double mileleftDays = Convert.ToDouble(mileobjtimeleftspn.Days);


                string mileleftdlydys = "";
                if (mileleftDays < 0)
                {
                    mileleftDays = mileleftDays + (-1);
                    miledayscomplite = 100;
                    mileleftdlydys = "days delayed";
                }
                else
                {
                    mileleftDays = mileleftDays + 1;
                    miledayscomplite = Math.Round(((mileDays - mileleftDays) * 100) / mileDays, 2);
                    mileleftdlydys = "days left";
                }
                //start days months  
                string milestartmonthname = "";
                int mileststartdys = mileStartDate.Day;
                int milestrtmonth = mileStartDate.Month;
                int milestonestartyear = mileStartDate.Year;
                //end days months 
                string mileendmonthname = "";
                int mileenddys = mileTargetdate.Day;
                int mileendmonth = mileTargetdate.Month;
                int milestoneEndyear = mileTargetdate.Year;
                if (mileleftdlydys != "")
                {
                    tleftdays = mileleftDays.ToString().Replace("-", "") + " " + mileleftdlydys;
                }
                retval = tleftdays;
            }

            #endregion
        }
        catch { }
        return retval;
    }
    public static int checkcompletestatus(string id)
    {
        int retval = 0;

        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        string strsql = "";
        try
        {
            
                strsql = "select id, ProjectStatus as name, PrintColor as colorname,isnull(iscomplete,0) as iscomplete, isnull(isdefault,0) isdefault from Pms_tbl_Projectstatus with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='"+id+"'";


            #region bind timeline
            //Timeline
            if (strsql != "")
            {
                DataSet dsD = da.GetDataSetInline(strsql);
                if(dsD.Tables[0].Rows[0]["iscomplete"].ToString()=="1")
                {
                    retval = 1;
                }                
                
            }

            #endregion
        }
        catch { }
        return retval;
    }
    #endregion
    #region check dependency
    public static string checkdependency(string strtype, string strid)
    {
        DataAccess da1 = new DataAccess();
        string ret = "0";
        string strsqlM = "select id, isnull(Isdone,0) Isdone from tbl_dependencyST where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and milestoneid>0 and  milestoneid='" + strid + "' and status=1 and archive=0 and isnull(Isdone,0)=0";
        if (strtype == "task")
        {
            strsqlM = "select id, isnull(Isdone,0)  from tbl_dependencyST where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and taskid>0  and taskid='" + strid + "'  and status=1 and archive=0  and isnull(Isdone,0)=0";
        }
        else if (strtype == "subtask")
        {
            strsqlM = "select id,  isnull(Isdone,0)  from tbl_dependencyST where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and subtaskid>0  and subtaskid='" + strid + "'  and status=1 and archive=0  and isnull(Isdone,0)=0";
        }

        DataSet dschkD = da1.GetDataSetInline(strsqlM);
        if (dschkD.Tables[0].Rows.Count > 0)
        {
            string chkD = checkdependencynotcomplete(dschkD.Tables[0].Rows[0]["id"].ToString());
            ret = chkD;

        }

        return ret;

    }
    public static string checkdependencynotcomplete(string ids)
    {
        DataAccess da = new DataAccess();
        string strdependency = "";
        string retval = "0";
        try
        {

            string str_selecteddeatil = "";
            if (ids != "")
            {

                SqlParameter[] prm = new SqlParameter[2];
                prm[0] = new SqlParameter("@id", ids);
                prm[1] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                DataSet dsD = da.GetDataSet("sp_dependencydetails", prm);
                if (dsD.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsD.Tables[0].Rows.Count; i++)
                    {
                        if (dsD.Tables[0].Rows[i]["stid"].ToString() != "" && dsD.Tables[0].Rows[i]["stid"].ToString() != "0")
                        {
                            try
                            {
                                string stsqlM = "select isnull(statuscomplete,0) statuscomplete from Pms_tbl_AddSubtask where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + dsD.Tables[0].Rows[i]["stid"].ToString() + "' and isnull(statuscomplete,0)=0";
                                SaveLogText("checkcomplete123-" + stsqlM);
                                DataSet dschkM = da.GetDataSetInline(stsqlM);
                                //if (dschkM.Tables[0].Rows[0]["statuscomplete"].ToString() != "1")
                                if (dschkM.Tables[0].Rows.Count > 0)
                                {
                                    retval = "1";
                                }

                            }
                            catch (Exception ex)
                            {
                                //SaveLogText("strbgc_s2-" + ex.Message);
                            }
                        }
                        else if (dsD.Tables[0].Rows[i]["tid"].ToString() != "" && dsD.Tables[0].Rows[i]["tid"].ToString() != "0")
                        {
                            try
                            {
                                string stsqlM = "select isnull(statuscomplete,0) statuscomplete from Pms_tbl_AddTask where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + dsD.Tables[0].Rows[i]["tid"].ToString() + "' and isnull(statuscomplete,0)=0";


                                DataSet dschkM = da.GetDataSetInline(stsqlM);
                                //if (dschkM.Tables[0].Rows[0]["statuscomplete"].ToString() != "1")
                                if (dschkM.Tables[0].Rows.Count > 0)
                                {
                                    retval = "1";
                                }
                            }
                            catch { }
                        }

                        else if (dsD.Tables[0].Rows[i]["mid"].ToString() != "" && dsD.Tables[0].Rows[i]["mid"].ToString() != "0")
                        {
                            try
                            {
                                string stsqlM = "select isnull(statuscomplete,0) statuscomplete from Pms_tbl_AddMilestone where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + dsD.Tables[0].Rows[i]["mid"].ToString() + "' and isnull(statuscomplete,0)=0";
                                DataSet dschkM = da.GetDataSetInline(stsqlM);
                                //if (dschkM.Tables[0].Rows[0]["statuscomplete"].ToString() != "1")
                                if (dschkM.Tables[0].Rows.Count > 0)
                                {
                                    retval = "1";
                                }
                            }
                            catch { }
                        }

                    }
                }

            }



        }
        catch (Exception ex)
        {

        }
        return retval;
    }
    public static string checkdependencyUpdate(string strtype, string strid)
    {
        DataAccess da1 = new DataAccess();
        string ret = "0";
        string strsqlM = "select isnull(Isdone,0) Isdone from tbl_dependencyST where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and milestoneid>0 and  milestoneid='" + strid + "' and status=1 and archive=0 and isnull(Isdone,0)=0";
        if (strtype == "task")
        {
            strsqlM = "select  isnull(Isdone,0)  from tbl_dependencyST where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and taskid>0 and taskid='" + strid + "'  and status=1 and archive=0  and isnull(Isdone,0)=0";
        }
        else if (strtype == "subtask")
        {
            strsqlM = "select  isnull(Isdone,0)  from tbl_dependencyST where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and subtaskid>0 and subtaskid='" + strid + "'  and status=1 and archive=0  and isnull(Isdone,0)=0";
        }
        //SaveLogText("strp123=" + strsqlM);

        DataSet dschkD = da1.GetDataSetInline(strsqlM);
        if (dschkD.Tables[0].Rows.Count > 0)
        {
            string strupdt = "update  tbl_dependencyST set Isdone=1    where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and milestoneid>0 and  milestoneid='" + strid + "'  and status=1 and archive=0  and isnull(Isdone,0)=0";
            if (strtype == "task")
            {
                strupdt = "update  tbl_dependencyST set Isdone=1    where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and taskid>0 and taskid='" + strid + "'  and status=1 and archive=0  and isnull(Isdone,0)=0";
            }
            if (strtype == "subtask")
            {
                strupdt = "update  tbl_dependencyST set Isdone=1    where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and subtaskid>0 and subtaskid='" + strid + "'  and status=1 and archive=0  and isnull(Isdone,0)=0";
            }
            ret = "1";

        }

        return ret;

    }
    #endregion
    #region manage critical workflow
    // ===== Critical Workflow Step 3/4/5 : Active Workflows + Rule Details + Current Context =====
    // Existing Status / Timer working is not changed.
    // Step 6 evaluates conditions/branches for logging only.
    // No CriticalLabel/action update is done here yet.
    private static void CheckCriticalWorkflow(string tasktype, string entityId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(entityId))
                return;

            if (string.IsNullOrWhiteSpace(tasktype))
                tasktype = "subtask";

            HttpCookie regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            HttpCookie userCookie = HttpContext.Current.Request.Cookies["user"];

            if (regCookie == null || regCookie["id"] == null ||
                userCookie == null || userCookie["user_id"] == null)
                return;

            string regid = regCookie["id"].ToString();
            string uid = userCookie["user_id"].ToString();

            SaveLogText(
                "CriticalWorkflow Trigger => Type=" + tasktype +
                ", EntityId=" + entityId +
                ", RegId=" + regid +
                ", Uid=" + uid
            );

            DataAccess daCritical = new DataAccess();

            // 1. Resolve LevelId from the same Critical Workflow master used by CriticalSetting.
            //    No hard-coded LevelId is used.
            SqlParameter[] prmMaster = new SqlParameter[2];
            prmMaster[0] = new SqlParameter("@regid", regid);
            prmMaster[1] = new SqlParameter("@uid", uid);

            DataSet dsMaster = daCritical.GetDataSet("usp_Pms_Critical_GetMasters", prmMaster);

            if (dsMaster == null || dsMaster.Tables.Count < 2)
            {
                SaveLogText("CriticalWorkflow Level Master Not Found => Type=" + tasktype + ", RegId=" + regid);
                return;
            }

            string requiredLevelCode = GetCriticalLevelCode(tasktype);
            string levelId = "";

            foreach (DataRow levelRow in dsMaster.Tables[1].Rows)
            {
                string levelCode = GetCriticalRowValue(levelRow, "LevelCode");

                if (string.Equals(levelCode, requiredLevelCode, StringComparison.OrdinalIgnoreCase))
                {
                    levelId = GetCriticalRowValue(levelRow, "id", "LevelId");
                    break;
                }
            }

            if (string.IsNullOrWhiteSpace(levelId))
            {
                SaveLogText(
                    "CriticalWorkflow Level Not Found => Type=" + tasktype +
                    ", LevelCode=" + requiredLevelCode +
                    ", RegId=" + regid
                );
                return;
            }

            // ===== Critical Workflow Step 5 : Load current entity context once =====
            // Status / Priority / Completion come from the current PMS entity row.
            // Time Consumed % reuses the existing timer calculation already used by this page.
            CriticalWorkflowContext criticalContext = LoadCriticalWorkflowContext(tasktype, entityId, regid);
            if (criticalContext == null)
            {
                SaveLogText(
                    "CriticalWorkflow Context Not Found => Type=" + tasktype +
                    ", EntityId=" + entityId +
                    ", RegId=" + regid
                );
                return;
            }

            SaveLogText(
                "CriticalWorkflow Context => Type=" + criticalContext.TaskType +
                ", EntityId=" + criticalContext.EntityId +
                ", StatusId=" + criticalContext.StatusId +
                ", PriorityId=" + criticalContext.PriorityId +
                ", Completion=" + criticalContext.CompletionPercent.ToString(CultureInfo.InvariantCulture) +
                ", TimeConsumed=" + criticalContext.TimeConsumedPercent.ToString(CultureInfo.InvariantCulture) +
                ", TaskDuration=" + criticalContext.TaskDuration +
                ", RegId=" + regid
            );

            // 2. Load all workflows of the current company + current level
            //    using the same SP already used by CriticalSetting.
            SqlParameter[] prmRules = new SqlParameter[2];
            prmRules[0] = new SqlParameter("@regid", regid);
            prmRules[1] = new SqlParameter("@LevelId", levelId);

            DataSet dsRules = daCritical.GetDataSet("usp_Pms_CriticalRule_List", prmRules);

            if (dsRules == null || dsRules.Tables.Count == 0)
            {
                SaveLogText(
                    "CriticalWorkflow Active Rules => Type=" + tasktype +
                    ", LevelId=" + levelId +
                    ", Count=0"
                );
                return;
            }

            DataTable dtRules = dsRules.Tables[0];
            int activeCount = 0;

            // Track the FINAL label result across ALL active workflows.
            // Do not clear after one rule fails to match, because another active rule may still set a label.
            bool anyCriticalLabelApplied = false;
            bool criticalEvaluationHadError = false;

            // 3. Only status=1 and archive=0 rules are considered active.
            foreach (DataRow ruleRow in dtRules.Rows)
            {
                if (!IsCriticalRuleActive(ruleRow))
                    continue;

                activeCount++;

                string ruleId = GetCriticalRowValue(ruleRow, "id", "RuleId");
                string ruleName = GetCriticalRowValue(ruleRow, "RuleName");
                string ruleLevelId = GetCriticalRowValue(ruleRow, "LevelId");

                SaveLogText(
                    "CriticalWorkflow Active Rule => RuleId=" + ruleId +
                    ", RuleName=" + ruleName +
                    ", LevelId=" + (string.IsNullOrWhiteSpace(ruleLevelId) ? levelId : ruleLevelId) +
                    ", RegId=" + regid
                );

                // ===== Critical Workflow Step 4 : Load Rule Conditions + Actions =====
                // Rule detail is loaded here. Step 6 evaluates it; Step 7 executes supported matched actions.
                int currentRuleId = 0;
                if (!int.TryParse(ruleId, out currentRuleId) || currentRuleId <= 0)
                {
                    SaveLogText(
                        "CriticalWorkflow Invalid RuleId => RuleId=" + ruleId +
                        ", RegId=" + regid
                    );
                    criticalEvaluationHadError = true;
                    continue;
                }

                try
                {
                    SqlParameter[] prmRuleDetail = new SqlParameter[2];
                    prmRuleDetail[0] = new SqlParameter("@RuleId", currentRuleId);
                    prmRuleDetail[1] = new SqlParameter("@regid", regid);

                    DataSet dsRuleDetail = daCritical.GetDataSet(
                        "usp_Pms_CriticalRule_GetById",
                        prmRuleDetail
                    );

                    if (dsRuleDetail == null)
                    {
                        SaveLogText(
                            "CriticalWorkflow Rule Detail Not Found => RuleId=" + currentRuleId +
                            ", RegId=" + regid
                        );
                        criticalEvaluationHadError = true;
                        continue;
                    }

                    DataTable dtConditions =
                        dsRuleDetail.Tables.Count > 1 ? dsRuleDetail.Tables[1] : null;

                    DataTable dtActions =
                        dsRuleDetail.Tables.Count > 2 ? dsRuleDetail.Tables[2] : null;

                    int conditionCount = dtConditions == null ? 0 : dtConditions.Rows.Count;
                    int actionCount = dtActions == null ? 0 : dtActions.Rows.Count;

                    if (dtConditions != null)
                    {
                        foreach (DataRow conditionRow in dtConditions.Rows)
                        {
                            string conditionFieldId = GetCriticalRowValue(conditionRow, "FieldId");
                            string conditionOperatorId = GetCriticalRowValue(conditionRow, "OperatorId");

                            SaveLogText(
                                "CriticalWorkflow Condition => " +
                                "RuleId=" + currentRuleId +
                                ", BranchNo=" + GetCriticalRowValue(conditionRow, "BranchNo") +
                                ", BranchType=" + GetCriticalRowValue(conditionRow, "BranchType") +
                                ", SequenceNo=" + GetCriticalRowValue(conditionRow, "SequenceNo") +
                                ", ConditionSource=" + GetCriticalRowValue(conditionRow, "ConditionSource") +
                                ", FieldId=" + conditionFieldId +
                                ", OperatorId=" + conditionOperatorId +
                                ", CompareValue=" + GetCriticalRowValue(conditionRow, "CompareValue") +
                                ", CompareValue2=" + GetCriticalRowValue(conditionRow, "CompareValue2") +
                                ", LogicalOperatorId=" + GetCriticalRowValue(conditionRow, "LogicalOperatorId") +
                                ", OpenBracket=" + GetCriticalRowValue(conditionRow, "OpenBracket") +
                                ", CloseBracket=" + GetCriticalRowValue(conditionRow, "CloseBracket")
                            );

                            // Step 5: resolve FieldId/OperatorId from the same masters used by CriticalSetting.
                            // No numeric FieldId or OperatorId is hard-coded here.
                            DataRow fieldMasterRow = null;
                            if (dsMaster.Tables.Count > 2)
                                fieldMasterRow = FindCriticalMasterRow(dsMaster.Tables[2], conditionFieldId, "FieldId");

                            DataRow operatorMasterRow = null;
                            if (dsMaster.Tables.Count > 3)
                                operatorMasterRow = FindCriticalMasterRow(dsMaster.Tables[3], conditionOperatorId, "OperatorId");

                            string fieldName = GetCriticalRowValue(fieldMasterRow, "FieldName");
                            string fieldCode = GetCriticalRowValue(fieldMasterRow, "FieldCode", "FieldKey");
                            string dataType = GetCriticalRowValue(fieldMasterRow, "DataType");
                            string sourceType = GetCriticalRowValue(fieldMasterRow, "SourceType");
                            string operatorName = GetCriticalRowValue(operatorMasterRow, "OperatorName");
                            string operatorSymbol = GetCriticalRowValue(operatorMasterRow, "OperatorSymbol");
                            string actualValue = GetCriticalActualValue(fieldMasterRow, criticalContext);

                            SaveLogText(
                                "CriticalWorkflow Field => " +
                                "RuleId=" + currentRuleId +
                                ", BranchNo=" + GetCriticalRowValue(conditionRow, "BranchNo") +
                                ", SequenceNo=" + GetCriticalRowValue(conditionRow, "SequenceNo") +
                                ", FieldId=" + conditionFieldId +
                                ", FieldName=" + fieldName +
                                ", FieldCode=" + fieldCode +
                                ", SourceType=" + sourceType +
                                ", DataType=" + dataType +
                                ", ActualValue=" + actualValue +
                                ", OperatorId=" + conditionOperatorId +
                                ", OperatorName=" + operatorName +
                                ", OperatorSymbol=" + operatorSymbol +
                                ", CompareValue=" + GetCriticalRowValue(conditionRow, "CompareValue") +
                                ", CompareValue2=" + GetCriticalRowValue(conditionRow, "CompareValue2")
                            );
                        }
                    }

                    if (dtActions != null)
                    {
                        foreach (DataRow actionRow in dtActions.Rows)
                        {
                            SaveLogText(
                                "CriticalWorkflow Action => " +
                                "RuleId=" + currentRuleId +
                                ", BranchNo=" + GetCriticalRowValue(actionRow, "BranchNo") +
                                ", BranchType=" + GetCriticalRowValue(actionRow, "BranchType") +
                                ", SequenceNo=" + GetCriticalRowValue(actionRow, "SequenceNo") +
                                ", ActionId=" + GetCriticalRowValue(actionRow, "ActionId") +
                                ", ActionValue=" + GetCriticalRowValue(actionRow, "ActionValue") +
                                ", RecipientType=" + GetCriticalRowValue(actionRow, "RecipientType") +
                                ", RecipientValue=" + GetCriticalRowValue(actionRow, "RecipientValue") +
                                ", IsEmail=" + GetCriticalRowValue(actionRow, "IsEmail") +
                                ", IsWhatsApp=" + GetCriticalRowValue(actionRow, "IsWhatsApp") +
                                ", IsBell=" + GetCriticalRowValue(actionRow, "IsBell")
                            );
                        }
                    }

                    SaveLogText(
                        "CriticalWorkflow Rule Detail => RuleId=" + currentRuleId +
                        ", RuleName=" + ruleName +
                        ", Conditions=" + conditionCount +
                        ", Actions=" + actionCount +
                        ", RegId=" + regid
                    );

                    // ===== Critical Workflow Step 6/7 : Evaluate branch + execute supported matched actions =====
                    // First matching branch wins inside this workflow.
                    // Step 7 executes actions of that matched branch in SequenceNo order.
                    CriticalWorkflowEvaluationResult evaluationResult = EvaluateCriticalRuleAndExecuteActions(
                        currentRuleId,
                        ruleName,
                        dtConditions,
                        dtActions,
                        dsMaster,
                        criticalContext,
                        regid
                    );

                    if (evaluationResult != null)
                    {
                        if (evaluationResult.LabelApplied)
                            anyCriticalLabelApplied = true;

                        if (evaluationResult.HadError)
                            criticalEvaluationHadError = true;
                    }
                }
                catch (Exception ruleDetailEx)
                {
                    // Rule-detail logging must never disturb existing Status / Timer working.
                    criticalEvaluationHadError = true;
                    SaveLogText(
                        "CriticalWorkflow Rule Detail Error => RuleId=" + currentRuleId +
                        ", Error=" + ruleDetailEx.Message
                    );
                }
            }

            SaveLogText(
                "CriticalWorkflow Active Rules => Type=" + tasktype +
                ", EntityId=" + entityId +
                ", LevelId=" + levelId +
                ", Count=" + activeCount +
                ", RegId=" + regid
            );

            // IMPORTANT: CriticalLabelId is derived from the CURRENT workflow result.
            // Clear the old/stale label only after ALL active workflows were evaluated successfully
            // and none of their matched branches applied SET_CRITICAL_LABEL.
            if (!criticalEvaluationHadError && !anyCriticalLabelApplied)
            {
                ClearCriticalEntityLabel(criticalContext, regid);
            }
            else if (criticalEvaluationHadError)
            {
                SaveLogText(
                    "CriticalWorkflow Label Clear Skipped => Type=" + criticalContext.TaskType +
                    ", EntityId=" + criticalContext.EntityId +
                    ", Reason=Evaluation error detected"
                );
            }

            // Step 7 remains unchanged: matched SET_CRITICAL_LABEL actions are executed.
            // SEND_NOTIFICATION now resolves configured recipients/channels for verification only.
            // Actual Email / WhatsApp / Bell sending and parent roll-up are still intentionally not enabled.
        }
        catch (Exception ex)
        {
            // Critical workflow must never disturb existing Status / Timer working.
            SaveLogText("CriticalWorkflow Trigger Error => " + ex.Message);
        }
    }

    private sealed class CriticalWorkflowContext
    {
        public string TaskType { get; set; }
        public string EntityId { get; set; }
        public string StatusId { get; set; }
        public string PriorityId { get; set; }
        public decimal CompletionPercent { get; set; }
        public decimal TimeConsumedPercent { get; set; }
        public string TaskDuration { get; set; }
    }

    private sealed class CriticalWorkflowEvaluationResult
    {
        public bool BranchMatched { get; set; }
        public bool LabelApplied { get; set; }
        public bool HadError { get; set; }
    }

    private sealed class CriticalActionExecutionResult
    {
        public bool LabelApplied { get; set; }
        public bool HadError { get; set; }
    }

    private static CriticalWorkflowContext LoadCriticalWorkflowContext(string tasktype, string entityId, string regid)
    {
        try
        {
            int entityIdValue = 0;
            if (!int.TryParse(entityId, out entityIdValue) || entityIdValue <= 0)
                return null;

            string type = (tasktype ?? "").Trim().Replace(" ", "").ToUpperInvariant();
            string safeRegId = (regid ?? "").Replace("'", "''");
            string sql = "";

            if (type == "SUBTASK")
            {
                sql = "select top 1 id,isnull(P_Status,0) as StatusId,isnull(Priority,0) as PriorityId," +
                      "isnull(iscompletepercentage,0) as CompletionPercent,isnull(TaskDuration,'') as TaskDuration " +
                      "from Pms_tbl_AddSubTask with(nolock) where id=" + entityIdValue +
                      " and regid='" + safeRegId + "' and status=1 and archive=0";
            }
            else if (type == "TASK")
            {
                sql = "select top 1 id,isnull(P_Status,0) as StatusId,isnull(Priority,0) as PriorityId," +
                      "isnull(iscompletepercentage,0) as CompletionPercent,isnull(TaskDuration,'') as TaskDuration " +
                      "from Pms_tbl_AddTask with(nolock) where id=" + entityIdValue +
                      " and regid='" + safeRegId + "' and status=1 and archive=0";
            }
            else if (type == "MILESTONE")
            {
                sql = "select top 1 id,isnull(P_Status,0) as StatusId,isnull(Priority,0) as PriorityId," +
                      "isnull(iscompletepercentage,0) as CompletionPercent,isnull(TaskDuration,'') as TaskDuration " +
                      "from Pms_tbl_AddMilestone with(nolock) where id=" + entityIdValue +
                      " and regid='" + safeRegId + "' and status=1 and archive=0";
            }
            else if (type == "PROJECT")
            {
                sql = "select top 1 id,isnull(ProjectStatus,0) as StatusId,isnull(Priority,0) as PriorityId," +
                      "isnull(iscompletepercentage,0) as CompletionPercent,'' as TaskDuration " +
                      "from Pms_tbl_AddProject with(nolock) where id=" + entityIdValue +
                      " and regid='" + safeRegId + "' and status=1 and archive=0";
            }
            else
            {
                return null;
            }

            DataAccess daContext = new DataAccess();
            DataSet dsContext = daContext.GetDataSetInline(sql);
            if (dsContext == null || dsContext.Tables.Count == 0 || dsContext.Tables[0].Rows.Count == 0)
                return null;

            DataRow row = dsContext.Tables[0].Rows[0];
            CriticalWorkflowContext context = new CriticalWorkflowContext();
            context.TaskType = type.ToLowerInvariant();
            context.EntityId = entityIdValue.ToString(CultureInfo.InvariantCulture);
            context.StatusId = GetCriticalRowValue(row, "StatusId");
            context.PriorityId = GetCriticalRowValue(row, "PriorityId");
            context.CompletionPercent = GetCriticalDecimal(row, "CompletionPercent");
            context.TaskDuration = GetCriticalRowValue(row, "TaskDuration");

            // Reuse the page's existing assigned-duration vs spent-time calculation.
            // This avoids introducing a second timer formula in the workflow code.
            if (type == "SUBTASK" || type == "TASK" || type == "MILESTONE")
            {
                try
                {
                    //context.TimeConsumedPercent = Convert.ToDecimal(getcompletetime(context.EntityId, context.TaskType),CultureInfo.InvariantCulture);
                    context.TimeConsumedPercent = Convert.ToDecimal(get_completionstatus(context.EntityId, context.TaskType),CultureInfo.InvariantCulture);
                }
                catch
                {
                    context.TimeConsumedPercent = 0;
                }
            }
            else
            {
                context.TimeConsumedPercent = 0;
            }

            return context;
        }
        catch (Exception ex)
        {
            SaveLogText("CriticalWorkflow Context Error => Type=" + tasktype + ", EntityId=" + entityId + ", Error=" + ex.Message);
            return null;
        }
    }

    private static decimal GetCriticalDecimal(DataRow row, params string[] columnNames)
    {
        string value = GetCriticalRowValue(row, columnNames);
        decimal result = 0;

        if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            return result;

        if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out result))
            return result;

        return 0;
    }

    private static DataRow FindCriticalMasterRow(DataTable table, string id, string alternateIdColumn)
    {
        if (table == null || string.IsNullOrWhiteSpace(id))
            return null;

        foreach (DataRow row in table.Rows)
        {
            string rowId = GetCriticalRowValue(row, "id", alternateIdColumn);
            if (string.Equals(rowId, id, StringComparison.OrdinalIgnoreCase))
                return row;
        }

        return null;
    }

    private static string GetCriticalActualValue(DataRow fieldMasterRow, CriticalWorkflowContext context)
    {
        if (fieldMasterRow == null || context == null)
            return "";

        // Resolve by field metadata, not by numeric FieldId.
        // SourceType is preferred; FieldCode/FieldKey/FieldName are safe fallbacks.
        string sourceType = NormalizeCriticalKey(GetCriticalRowValue(fieldMasterRow, "SourceType"));
        string fieldCode = NormalizeCriticalKey(GetCriticalRowValue(fieldMasterRow, "FieldCode", "FieldKey"));
        string fieldName = NormalizeCriticalKey(GetCriticalRowValue(fieldMasterRow, "FieldName"));
        string key = sourceType + "|" + fieldCode + "|" + fieldName;

        if (key.Contains("TIMECONSUMED") ||
            key.Contains("CONSUMEDTIME") ||
            key.Contains("TIMESPENT") ||
            key.Contains("SPENTTIME") ||
            key.Contains("TIMEUSED"))
        {
            return context.TimeConsumedPercent.ToString(CultureInfo.InvariantCulture);
        }

        if (key.Contains("STATUS"))
            return context.StatusId ?? "";

        if (key.Contains("PRIORITY"))
            return context.PriorityId ?? "";

        if (key.Contains("COMPLETION") ||
            key.Contains("COMPLETEPERCENT") ||
            key.Contains("PROGRESS"))
        {
            return context.CompletionPercent.ToString(CultureInfo.InvariantCulture);
        }

        return "";
    }


    // ===== Critical Workflow Step 6/7 helpers =====
    // Evaluates every condition in a branch, honours AND/OR + brackets,
    // and stops at the first matching branch inside ONE workflow.
    // Step 7 then executes supported actions of that matched branch.
    private static CriticalWorkflowEvaluationResult EvaluateCriticalRuleAndExecuteActions(
        int ruleId,
        string ruleName,
        DataTable dtConditions,
        DataTable dtActions,
        DataSet dsMaster,
        CriticalWorkflowContext context,
        string regid)
    {
        CriticalWorkflowEvaluationResult evaluationResult = new CriticalWorkflowEvaluationResult();

        if (dtConditions == null || dtConditions.Rows.Count == 0)
        {
            SaveLogText(
                "CriticalWorkflow No Conditions => RuleId=" + ruleId +
                ", RuleName=" + (ruleName ?? "")
            );
            // Do not clear an existing label when the rule itself could not be evaluated.
            evaluationResult.HadError = true;
            return evaluationResult;
        }

        List<int> branchNos = new List<int>();

        foreach (DataRow row in dtConditions.Rows)
        {
            int branchNo;
            if (Int32.TryParse(GetCriticalRowValue(row, "BranchNo"), out branchNo) &&
                branchNo > 0 && !branchNos.Contains(branchNo))
            {
                branchNos.Add(branchNo);
            }
        }

        // Include an actions-only branch too, so a classic empty ELSE remains possible later.
        if (dtActions != null)
        {
            foreach (DataRow row in dtActions.Rows)
            {
                int branchNo;
                if (Int32.TryParse(GetCriticalRowValue(row, "BranchNo"), out branchNo) &&
                    branchNo > 0 && !branchNos.Contains(branchNo))
                {
                    branchNos.Add(branchNo);
                }
            }
        }

        branchNos.Sort();
        bool matchedAnyBranch = false;

        foreach (int branchNo in branchNos)
        {
            List<DataRow> branchConditions = dtConditions.AsEnumerable()
                .Where(delegate(DataRow r)
                {
                    int n;
                    return Int32.TryParse(GetCriticalRowValue(r, "BranchNo"), out n) && n == branchNo;
                })
                .OrderBy(delegate(DataRow r)
                {
                    int n;
                    return Int32.TryParse(GetCriticalRowValue(r, "SequenceNo"), out n) ? n : Int32.MaxValue;
                })
                .ToList();

            string branchType = "";
            if (branchConditions.Count > 0)
                branchType = GetCriticalRowValue(branchConditions[0], "BranchType");

            if (String.IsNullOrWhiteSpace(branchType) && dtActions != null)
            {
                DataRow actionBranchRow = dtActions.AsEnumerable().FirstOrDefault(delegate(DataRow r)
                {
                    int n;
                    return Int32.TryParse(GetCriticalRowValue(r, "BranchNo"), out n) && n == branchNo;
                });
                branchType = GetCriticalRowValue(actionBranchRow, "BranchType");
            }

            branchType = (branchType ?? "").Trim().ToUpperInvariant();

            string expressionText;
            string branchError;
            bool branchResult;

            // Current UI saves conditions for ELSE too. This fallback also supports
            // a classic empty ELSE safely if one exists in old data.
            if (branchConditions.Count == 0)
            {
                branchResult = branchType == "ELSE";
                expressionText = branchResult ? "TRUE (unconditional ELSE)" : "";
                branchError = branchResult ? "" : "No conditions found.";
            }
            else
            {
                branchResult = EvaluateCriticalBranch(
                    ruleId,
                    branchNo,
                    branchType,
                    branchConditions,
                    dsMaster,
                    context,
                    out expressionText,
                    out branchError
                );
            }

            SaveLogText(
                "CriticalWorkflow Branch Result => " +
                "RuleId=" + ruleId +
                ", BranchNo=" + branchNo +
                ", BranchType=" + branchType +
                ", Expression=" + expressionText +
                ", Result=" + branchResult.ToString() +
                (String.IsNullOrWhiteSpace(branchError) ? "" : ", Error=" + branchError)
            );

            if (!String.IsNullOrWhiteSpace(branchError))
                evaluationResult.HadError = true;

            if (!branchResult)
                continue;

            matchedAnyBranch = true;
            evaluationResult.BranchMatched = true;

            SaveLogText(
                "CriticalWorkflow Matched Branch => " +
                "RuleId=" + ruleId +
                ", RuleName=" + (ruleName ?? "") +
                ", BranchNo=" + branchNo +
                ", BranchType=" + branchType
            );

            // Execute every action of the matched branch in SequenceNo order.
            // Each action is isolated so a new/unsupported action cannot break existing status/timer working.
            if (dtActions != null)
            {
                List<DataRow> matchedActions = dtActions.AsEnumerable()
                    .Where(delegate(DataRow r)
                    {
                        int n;
                        return Int32.TryParse(GetCriticalRowValue(r, "BranchNo"), out n) && n == branchNo;
                    })
                    .OrderBy(delegate(DataRow r)
                    {
                        int n;
                        return Int32.TryParse(GetCriticalRowValue(r, "SequenceNo"), out n) ? n : Int32.MaxValue;
                    })
                    .ToList();

                foreach (DataRow actionRow in matchedActions)
                {
                    SaveLogText(
                        "CriticalWorkflow Matched Action => " +
                        "RuleId=" + ruleId +
                        ", BranchNo=" + branchNo +
                        ", SequenceNo=" + GetCriticalRowValue(actionRow, "SequenceNo") +
                        ", ActionId=" + GetCriticalRowValue(actionRow, "ActionId") +
                        ", ActionValue=" + GetCriticalRowValue(actionRow, "ActionValue") +
                        ", RecipientType=" + GetCriticalRowValue(actionRow, "RecipientType") +
                        ", RecipientValue=" + GetCriticalRowValue(actionRow, "RecipientValue") +
                        ", IsEmail=" + GetCriticalRowValue(actionRow, "IsEmail") +
                        ", IsWhatsApp=" + GetCriticalRowValue(actionRow, "IsWhatsApp") +
                        ", IsBell=" + GetCriticalRowValue(actionRow, "IsBell")
                    );

                    try
                    {
                        CriticalActionExecutionResult actionResult = ExecuteCriticalMatchedAction(
                            ruleId,
                            branchNo,
                            actionRow,
                            dsMaster,
                            context,
                            regid
                        );

                        if (actionResult != null)
                        {
                            if (actionResult.LabelApplied)
                                evaluationResult.LabelApplied = true;

                            if (actionResult.HadError)
                                evaluationResult.HadError = true;
                        }
                    }
                    catch (Exception actionEx)
                    {
                        evaluationResult.HadError = true;
                        SaveLogText(
                            "CriticalWorkflow Action Execute Error => " +
                            "RuleId=" + ruleId +
                            ", BranchNo=" + branchNo +
                            ", ActionId=" + GetCriticalRowValue(actionRow, "ActionId") +
                            ", Error=" + actionEx.Message
                        );
                    }
                }
            }

            // FIRST matching IF / ELSE IF / ELSE branch wins inside this workflow.
            // The outer workflow loop continues to the next active workflow.
            break;
        }

        if (!matchedAnyBranch)
        {
            SaveLogText(
                "CriticalWorkflow No Matched Branch => RuleId=" + ruleId +
                ", RuleName=" + (ruleName ?? "")
            );
        }

        return evaluationResult;
    }

    // ===== Critical Workflow Step 7 : Execute matched action =====
    // ActionId itself is NOT hard-coded. ActionCode is resolved from the action master
    // returned by usp_Pms_Critical_GetMasters (table index 5).
    private static CriticalActionExecutionResult ExecuteCriticalMatchedAction(
        int ruleId,
        int branchNo,
        DataRow actionRow,
        DataSet dsMaster,
        CriticalWorkflowContext context,
        string regid)
    {
        CriticalActionExecutionResult executionResult = new CriticalActionExecutionResult();

        if (actionRow == null || context == null)
        {
            executionResult.HadError = true;
            return executionResult;
        }

        string actionId = GetCriticalRowValue(actionRow, "ActionId");
        string actionValue = GetCriticalRowValue(actionRow, "ActionValue");
        string sequenceNo = GetCriticalRowValue(actionRow, "SequenceNo");

        DataRow actionMasterRow = null;
        if (dsMaster != null && dsMaster.Tables.Count > 5)
            actionMasterRow = FindCriticalMasterRow(dsMaster.Tables[5], actionId, "ActionId");

        if (actionMasterRow == null)
        {
            SaveLogText(
                "CriticalWorkflow Action Master Not Found => " +
                "RuleId=" + ruleId +
                ", BranchNo=" + branchNo +
                ", ActionId=" + actionId
            );
            executionResult.HadError = true;
            return executionResult;
        }

        string actionCodeRaw = GetCriticalRowValue(actionMasterRow, "ActionCode", "ActionKey", "Code");
        string actionName = GetCriticalRowValue(actionMasterRow, "ActionName", "Name");
        string actionCode = NormalizeCriticalKey(actionCodeRaw);

        SaveLogText(
            "CriticalWorkflow Execute Action => " +
            "RuleId=" + ruleId +
            ", BranchNo=" + branchNo +
            ", SequenceNo=" + sequenceNo +
            ", ActionId=" + actionId +
            ", ActionCode=" + actionCodeRaw +
            ", ActionName=" + actionName +
            ", ActionValue=" + actionValue
        );

        if (actionCode == "SETCRITICALLABEL")
        {
            executionResult.LabelApplied = UpdateCriticalEntityLabel(
                ruleId,
                branchNo,
                context,
                actionValue,
                dsMaster,
                regid
            );

            if (!executionResult.LabelApplied)
                executionResult.HadError = true;

            return executionResult;
        }

        // ===== Critical Workflow Notification Verify Step =====
        // Safe extension after Step 7: only resolve recipients + configured channels and LOG them.
        // No Email / WhatsApp / Bell is actually sent in this step, so existing working is untouched.
        if (actionCode == "SENDNOTIFICATION")
        {
            VerifyCriticalNotificationAction(
                ruleId,
                branchNo,
                actionRow,
                context,
                regid
            );
            return executionResult;
        }

        SaveLogText(
            "CriticalWorkflow Unsupported Action => " +
            "RuleId=" + ruleId +
            ", BranchNo=" + branchNo +
            ", ActionId=" + actionId +
            ", ActionCode=" + actionCodeRaw
        );

        return executionResult;
    }

    private static void VerifyCriticalNotificationAction(
        int ruleId,
        int branchNo,
        DataRow actionRow,
        CriticalWorkflowContext context,
        string regid)
    {
        if (actionRow == null || context == null)
            return;

        string recipientTypeRaw = GetCriticalRowValue(actionRow, "RecipientType");
        string recipientType = NormalizeCriticalKey(recipientTypeRaw);
        string recipientValue = GetCriticalRowValue(actionRow, "RecipientValue");
        string notificationText = GetCriticalRowValue(actionRow, "ActionValue");

        bool isEmail = GetCriticalBoolean(actionRow, "IsEmail");
        bool isWhatsApp = GetCriticalBoolean(actionRow, "IsWhatsApp");
        bool isBell = GetCriticalBoolean(actionRow, "IsBell");

        SaveLogText(
            "CriticalWorkflow Notification Config => " +
            "RuleId=" + ruleId +
            ", BranchNo=" + branchNo +
            ", RecipientType=" + recipientTypeRaw +
            ", IsEmail=" + isEmail.ToString() +
            ", IsWhatsApp=" + isWhatsApp.ToString() +
            ", IsBell=" + isBell.ToString() +
            ", Message=" + (notificationText ?? "")
        );

        // Setting validation already requires at least one channel. Runtime still protects old rows.
        if (!isEmail && !isWhatsApp && !isBell)
        {
            SaveLogText(
                "CriticalWorkflow Notification Skip => " +
                "RuleId=" + ruleId +
                ", BranchNo=" + branchNo +
                ", Reason=No notification channel selected"
            );
            return;
        }

        List<int> configuredRecipients = ResolveCriticalNotificationRecipients(
            context,
            recipientType,
            recipientValue,
            regid
        );

        List<CriticalNotificationRecipient> activeRecipients =
            LoadCriticalActiveNotificationRecipients(configuredRecipients, regid);

        string recipientIds = String.Join(",", activeRecipients.Select(x => x.EmployeeId.ToString()).ToArray());
        string recipientNames = String.Join("|", activeRecipients.Select(x => x.DisplayName ?? "").ToArray());
        string recipientEmails = String.Join("|", activeRecipients
            .Where(x => !String.IsNullOrWhiteSpace(x.EmailId))
            .Select(x => x.EmailId)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray());
        string recipientMobiles = String.Join("|", activeRecipients
            .Where(x => !String.IsNullOrWhiteSpace(x.MobileNo))
            .Select(x => x.MobileNo)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray());

        int emailRecipientCount = activeRecipients.Count(x => !String.IsNullOrWhiteSpace(x.EmailId));
        int mobileRecipientCount = activeRecipients.Count(x => !String.IsNullOrWhiteSpace(x.MobileNo));

        SaveLogText(
            "CriticalWorkflow Notification Recipients => " +
            "RuleId=" + ruleId +
            ", BranchNo=" + branchNo +
            ", Type=" + context.TaskType +
            ", EntityId=" + context.EntityId +
            ", RecipientType=" + recipientTypeRaw +
            ", Count=" + activeRecipients.Count +
            ", RecipientIds=" + recipientIds +
            ", RecipientNames=" + recipientNames +
            ", RecipientEmails=" + recipientEmails +
            ", RecipientMobiles=" + recipientMobiles
        );

        SaveLogText(
            "CriticalWorkflow Notification Ready => " +
            "RuleId=" + ruleId +
            ", BranchNo=" + branchNo +
            ", RecipientCount=" + activeRecipients.Count +
            ", EmailRecipientCount=" + emailRecipientCount +
            ", MobileRecipientCount=" + mobileRecipientCount +
            ", Email=" + isEmail.ToString() +
            ", WhatsApp=" + isWhatsApp.ToString() +
            ", Bell=" + isBell.ToString() +
            ", ActualSend=False"
        );
    }

    private sealed class CriticalNotificationRecipient
    {
        public int EmployeeId { get; set; }
        public string DisplayName { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
    }

    // Returns first existing NON-BLANK value.
    // This is different from GetCriticalRowValue because notification contact details
    // may exist in alternate legacy columns (email_Id/Email and mobile_no/MobileNumber).
    private static string GetCriticalFirstNonEmptyRowValue(DataRow row, params string[] columnNames)
    {
        if (row == null || row.Table == null || columnNames == null)
            return "";

        foreach (string columnName in columnNames)
        {
            if (String.IsNullOrWhiteSpace(columnName))
                continue;

            foreach (DataColumn col in row.Table.Columns)
            {
                if (!String.Equals(col.ColumnName, columnName, StringComparison.OrdinalIgnoreCase))
                    continue;

                if (row[col] == null || row[col] == DBNull.Value)
                    break;

                string value = row[col].ToString();
                if (!String.IsNullOrWhiteSpace(value))
                    return value.Trim();

                break;
            }
        }

        return "";
    }

    private static bool GetCriticalBoolean(DataRow row, params string[] columnNames)
    {
        string value = GetCriticalRowValue(row, columnNames);
        if (String.IsNullOrWhiteSpace(value))
            return false;

        value = value.Trim();

        bool boolValue;
        if (Boolean.TryParse(value, out boolValue))
            return boolValue;

        int intValue;
        if (Int32.TryParse(value, out intValue))
            return intValue == 1;

        return String.Equals(value, "yes", StringComparison.OrdinalIgnoreCase) ||
               String.Equals(value, "y", StringComparison.OrdinalIgnoreCase);
    }

    private static List<int> ResolveCriticalNotificationRecipients(
        CriticalWorkflowContext context,
        string recipientType,
        string recipientValue,
        string regid)
    {
        List<int> recipients = new List<int>();

        if (context == null)
            return recipients;

        // Custom employees are already saved as comma-separated employee IDs by Critical Setting.
        if (recipientType == "CUSTOM")
            return ParseCriticalEmployeeIds(recipientValue);

        int entityId;
        if (!Int32.TryParse(context.EntityId, out entityId) || entityId <= 0)
            return recipients;

        string type = NormalizeCriticalKey(context.TaskType);
        string tableName = "";
        string projectIdExpression = "ProjectId";

        // Only physical PMS table mapping is fixed. Recipient values always come from current entity data.
        if (type == "SUBTASK") tableName = "Pms_tbl_AddSubTask";
        else if (type == "TASK") tableName = "Pms_tbl_AddTask";
        else if (type == "MILESTONE") tableName = "Pms_tbl_AddMilestone";
        else if (type == "PROJECT")
        {
            tableName = "Pms_tbl_AddProject";
            projectIdExpression = "id";
        }
        else
            return recipients;

        string safeRegId = (regid ?? "").Replace("'", "''");
        DataAccess da = new DataAccess();

        string entitySql =
            "select top 1 " +
            "isnull(cast(Owner as nvarchar(max)),'') as Owner," +
            "isnull(cast(Team as nvarchar(max)),'') as Team," +
            "isnull(cast(" + projectIdExpression + " as nvarchar(50)),'') as ProjectId " +
            "from " + tableName + " with(nolock) " +
            "where id=" + entityId +
            " and regid='" + safeRegId + "' and status=1 and archive=0";

        DataSet dsEntity = da.GetDataSetInline(entitySql);
        if (dsEntity == null || dsEntity.Tables.Count == 0 || dsEntity.Tables[0].Rows.Count == 0)
            return recipients;

        DataRow entityRow = dsEntity.Tables[0].Rows[0];

        if (recipientType == "OWNER")
            return ParseCriticalEmployeeIds(GetCriticalRowValue(entityRow, "Owner"));

        if (recipientType == "TEAM")
            return ParseCriticalEmployeeIds(GetCriticalRowValue(entityRow, "Team"));

        if (recipientType == "PROJECTMANAGER")
        {
            int projectId;
            if (!Int32.TryParse(GetCriticalRowValue(entityRow, "ProjectId"), out projectId) || projectId <= 0)
                return recipients;

            string projectSql =
                "select top 1 isnull(cast(Owner as nvarchar(max)),'') as Owner " +
                "from Pms_tbl_AddProject with(nolock) " +
                "where id=" + projectId +
                " and regid='" + safeRegId + "' and status=1 and archive=0";

            DataSet dsProject = da.GetDataSetInline(projectSql);
            if (dsProject != null && dsProject.Tables.Count > 0 && dsProject.Tables[0].Rows.Count > 0)
                return ParseCriticalEmployeeIds(GetCriticalRowValue(dsProject.Tables[0].Rows[0], "Owner"));
        }

        return recipients;
    }

    private static List<int> ParseCriticalEmployeeIds(string ids)
    {
        List<int> result = new List<int>();
        if (String.IsNullOrWhiteSpace(ids))
            return result;

        string[] parts = ids.Split(new char[] { ',', ';', '|' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string part in parts)
        {
            int employeeId;
            if (Int32.TryParse((part ?? "").Trim(), out employeeId) && employeeId > 0 && !result.Contains(employeeId))
                result.Add(employeeId);
        }

        return result;
    }

    private static List<CriticalNotificationRecipient> LoadCriticalActiveNotificationRecipients(
        List<int> employeeIds,
        string regid)
    {
        List<CriticalNotificationRecipient> result = new List<CriticalNotificationRecipient>();
        if (employeeIds == null || employeeIds.Count == 0)
            return result;

        List<int> safeIds = employeeIds.Where(x => x > 0).Distinct().ToList();
        if (safeIds.Count == 0)
            return result;

        string safeRegId = (regid ?? "").Replace("'", "''");
        string idList = String.Join(",", safeIds.Select(x => x.ToString(CultureInfo.InvariantCulture)).ToArray());

        // Select employee row once, then use a safe fallback in C# for legacy contact columns:
        // Email: email_Id -> Email -> EmailId / EmailID
        // Mobile: mobile_no -> MobileNumber -> MobileNo / Mobile
        // Using SELECT * here avoids SQL failure when one legacy alternate column does not exist.
        string sql =
            "select * " +
            "from tbl_employee with(nolock) " +
            "where id in(" + idList + ") " +
            "and regid='" + safeRegId + "' and status=1 and archive=0 " +
            "order by DisplayName";

        DataAccess da = new DataAccess();
        DataSet ds = da.GetDataSetInline(sql);

        if (ds == null || ds.Tables.Count == 0)
            return result;

        foreach (DataRow row in ds.Tables[0].Rows)
        {
            int employeeId;
            if (!Int32.TryParse(GetCriticalRowValue(row, "id"), out employeeId))
                continue;

            result.Add(new CriticalNotificationRecipient
            {
                EmployeeId = employeeId,
                DisplayName = GetCriticalRowValue(row, "DisplayName"),
                EmailId = GetCriticalFirstNonEmptyRowValue(row, "email_Id", "Email", "EmailId", "EmailID"),
                MobileNo = GetCriticalFirstNonEmptyRowValue(row, "mobile_no", "MobileNumber", "MobileNo", "Mobile")
            });
        }

        return result;
    }

    private static bool UpdateCriticalEntityLabel(
        int ruleId,
        int branchNo,
        CriticalWorkflowContext context,
        string actionValue,
        DataSet dsMaster,
        string regid)
    {
        int entityId;
        int criticalLabelId;

        if (!Int32.TryParse(context.EntityId, out entityId) || entityId <= 0)
        {
            SaveLogText(
                "CriticalWorkflow Set Label Error => RuleId=" + ruleId +
                ", BranchNo=" + branchNo +
                ", Error=Invalid EntityId " + (context.EntityId ?? "")
            );
            return false;
        }

        if (!Int32.TryParse(actionValue, out criticalLabelId) || criticalLabelId <= 0)
        {
            SaveLogText(
                "CriticalWorkflow Set Label Error => RuleId=" + ruleId +
                ", BranchNo=" + branchNo +
                ", Error=Invalid CriticalLabelId " + (actionValue ?? "")
            );
            return false;
        }

        // Validate the configured label against the same label master used by CriticalSetting.
        DataRow labelMasterRow = null;
        if (dsMaster != null && dsMaster.Tables.Count > 0)
            labelMasterRow = FindCriticalMasterRow(dsMaster.Tables[0], criticalLabelId.ToString(CultureInfo.InvariantCulture), "CriticalLabelId");

        if (labelMasterRow == null)
        {
            SaveLogText(
                "CriticalWorkflow Set Label Error => RuleId=" + ruleId +
                ", BranchNo=" + branchNo +
                ", CriticalLabelId=" + criticalLabelId +
                ", Error=Critical label not found in active master"
            );
            return false;
        }

        string tableName;
        string type = NormalizeCriticalKey(context.TaskType);

        // Physical table mapping only. Business rule values/label IDs are not hard-coded.
        if (type == "SUBTASK") tableName = "Pms_tbl_AddSubTask";
        else if (type == "TASK") tableName = "Pms_tbl_AddTask";
        else if (type == "MILESTONE") tableName = "Pms_tbl_AddMilestone";
        else if (type == "PROJECT") tableName = "Pms_tbl_AddProject";
        else
        {
            SaveLogText(
                "CriticalWorkflow Set Label Error => RuleId=" + ruleId +
                ", BranchNo=" + branchNo +
                ", Error=Unsupported entity type " + (context.TaskType ?? "")
            );
            return false;
        }

        string safeRegId = (regid ?? "").Replace("'", "''");
        string labelName = GetCriticalRowValue(
            labelMasterRow,
            "CriticalLabelName",
            "LabelName",
            "CriticalLabel",
            "Name"
        );

        DataAccess daAction = new DataAccess();

        // IDs are already parsed as integers above. regid is escaped before inline SQL.
        string updateSql =
            "update " + tableName +
            " set CriticalLabelId=" + criticalLabelId +
            " where id=" + entityId +
            " and regid='" + safeRegId + "' and status=1 and archive=0";

        daAction.executeDMLQueryinline(updateSql);

        // Read back the value so the log confirms that the action actually reached the entity row.
        string verifySql =
            "select top 1 isnull(CriticalLabelId,0) as CriticalLabelId from " + tableName +
            " with(nolock) where id=" + entityId +
            " and regid='" + safeRegId + "' and status=1 and archive=0";

        DataSet dsVerify = daAction.GetDataSetInline(verifySql);
        string savedLabelId = "";
        if (dsVerify != null && dsVerify.Tables.Count > 0 && dsVerify.Tables[0].Rows.Count > 0)
            savedLabelId = GetCriticalRowValue(dsVerify.Tables[0].Rows[0], "CriticalLabelId");

        bool saved = String.Equals(
            savedLabelId,
            criticalLabelId.ToString(CultureInfo.InvariantCulture),
            StringComparison.OrdinalIgnoreCase
        );

        SaveLogText(
            "CriticalWorkflow Set Label => " +
            "RuleId=" + ruleId +
            ", BranchNo=" + branchNo +
            ", Type=" + context.TaskType +
            ", EntityId=" + entityId +
            ", CriticalLabelId=" + criticalLabelId +
            (String.IsNullOrWhiteSpace(labelName) ? "" : ", LabelName=" + labelName) +
            ", Saved=" + saved.ToString()
        );

        return saved;
    }

    // Clears a stale CriticalLabelId when the CURRENT evaluation produces no SET_CRITICAL_LABEL action.
    // This is intentionally called only after ALL active workflows for the entity have been evaluated.
    private static bool ClearCriticalEntityLabel(CriticalWorkflowContext context, string regid)
    {
        try
        {
            if (context == null)
                return false;

            int entityId;
            if (!Int32.TryParse(context.EntityId, out entityId) || entityId <= 0)
                return false;

            string type = NormalizeCriticalKey(context.TaskType);
            string tableName = "";

            if (type == "SUBTASK") tableName = "Pms_tbl_AddSubTask";
            else if (type == "TASK") tableName = "Pms_tbl_AddTask";
            else if (type == "MILESTONE") tableName = "Pms_tbl_AddMilestone";
            else if (type == "PROJECT") tableName = "Pms_tbl_AddProject";
            else
                return false;

            string safeRegId = (regid ?? "").Replace("'", "''");
            DataAccess da = new DataAccess();

            // Update only when a stale value exists. Existing NULL remains untouched.
            string clearSql =
                "update " + tableName +
                " set CriticalLabelId=NULL " +
                "where id=" + entityId +
                " and regid='" + safeRegId + "' and status=1 and archive=0 " +
                "and CriticalLabelId is not null";

            da.executeDMLQueryinline(clearSql);

            // Verify the current value after the clear attempt.
            string verifySql =
                "select top 1 CriticalLabelId from " + tableName +
                " with(nolock) where id=" + entityId +
                " and regid='" + safeRegId + "' and status=1 and archive=0";

            DataSet dsVerify = da.GetDataSetInline(verifySql);
            bool cleared = false;

            if (dsVerify != null && dsVerify.Tables.Count > 0 && dsVerify.Tables[0].Rows.Count > 0)
            {
                object currentValue = dsVerify.Tables[0].Rows[0]["CriticalLabelId"];
                cleared = currentValue == null || currentValue == DBNull.Value ||
                          String.IsNullOrWhiteSpace(Convert.ToString(currentValue));
            }

            SaveLogText(
                "CriticalWorkflow Label Cleared => " +
                "Type=" + context.TaskType +
                ", EntityId=" + context.EntityId +
                ", Cleared=" + cleared.ToString()
            );

            return cleared;
        }
        catch (Exception ex)
        {
            // Clearing must never disturb existing Status / Timer working.
            SaveLogText(
                "CriticalWorkflow Label Clear Error => Type=" +
                (context == null ? "" : context.TaskType) +
                ", EntityId=" + (context == null ? "" : context.EntityId) +
                ", Error=" + ex.Message
            );
            return false;
        }
    }


    private static bool EvaluateCriticalBranch(
        int ruleId,
        int branchNo,
        string branchType,
        List<DataRow> conditions,
        DataSet dsMaster,
        CriticalWorkflowContext context,
        out string expressionText,
        out string error)
    {
        expressionText = "";
        error = "";
        List<string> tokens = new List<string>();

        if (conditions == null || conditions.Count == 0)
        {
            error = "No conditions found.";
            return false;
        }

        for (int i = 0; i < conditions.Count; i++)
        {
            DataRow conditionRow = conditions[i];
            string source = (GetCriticalRowValue(conditionRow, "ConditionSource") ?? "SELF").Trim().ToUpperInvariant();
            string fieldId = GetCriticalRowValue(conditionRow, "FieldId");
            string operatorId = GetCriticalRowValue(conditionRow, "OperatorId");
            string compareValue = GetCriticalRowValue(conditionRow, "CompareValue");
            string compareValue2 = GetCriticalRowValue(conditionRow, "CompareValue2");
            string sequenceNo = GetCriticalRowValue(conditionRow, "SequenceNo");

            DataRow fieldMasterRow = null;
            if (dsMaster != null && dsMaster.Tables.Count > 2)
                fieldMasterRow = FindCriticalMasterRow(dsMaster.Tables[2], fieldId, "FieldId");

            DataRow operatorMasterRow = null;
            if (dsMaster != null && dsMaster.Tables.Count > 3)
                operatorMasterRow = FindCriticalMasterRow(dsMaster.Tables[3], operatorId, "OperatorId");

            string fieldName = GetCriticalRowValue(fieldMasterRow, "FieldName");
            string fieldCode = GetCriticalRowValue(fieldMasterRow, "FieldCode", "FieldKey");
            string dataType = GetCriticalRowValue(fieldMasterRow, "DataType");
            string operatorName = GetCriticalRowValue(operatorMasterRow, "OperatorName");
            string operatorSymbol = ResolveCriticalOperatorSymbol(operatorMasterRow);
            string actualValue = "";
            bool conditionResult = false;
            string conditionError = "";

            if (source != "SELF")
            {
                conditionError = "ConditionSource " + source + " is not evaluated in Step 6.";
                conditionResult = false;
            }
            else if (fieldMasterRow == null)
            {
                conditionError = "Field master not found for FieldId=" + fieldId;
                conditionResult = false;
            }
            else if (operatorMasterRow == null)
            {
                conditionError = "Operator master not found for OperatorId=" + operatorId;
                conditionResult = false;
            }
            else
            {
                actualValue = GetCriticalActualValue(fieldMasterRow, context);
                conditionResult = EvaluateCriticalComparison(
                    actualValue,
                    compareValue,
                    compareValue2,
                    dataType,
                    operatorSymbol,
                    out conditionError
                );
            }

            SaveLogText(
                "CriticalWorkflow Evaluation => " +
                "RuleId=" + ruleId +
                ", BranchNo=" + branchNo +
                ", BranchType=" + branchType +
                ", SequenceNo=" + sequenceNo +
                ", FieldId=" + fieldId +
                ", FieldName=" + fieldName +
                ", FieldCode=" + fieldCode +
                ", Actual=" + actualValue +
                ", Operator=" + operatorSymbol +
                ", OperatorName=" + operatorName +
                ", Expected=" + compareValue +
                (String.IsNullOrWhiteSpace(compareValue2) ? "" : ", Expected2=" + compareValue2) +
                ", Result=" + conditionResult.ToString() +
                (String.IsNullOrWhiteSpace(conditionError) ? "" : ", Error=" + conditionError)
            );

            AddCriticalBracketTokens(tokens, GetCriticalRowValue(conditionRow, "OpenBracket"), true);
            tokens.Add(conditionResult ? "TRUE" : "FALSE");
            AddCriticalBracketTokens(tokens, GetCriticalRowValue(conditionRow, "CloseBracket"), false);

            if (i < conditions.Count - 1)
            {
                string logicalOperatorId = GetCriticalRowValue(conditionRow, "LogicalOperatorId");
                string logicalOperator = ResolveCriticalLogicalOperator(dsMaster, logicalOperatorId);

                if (String.IsNullOrWhiteSpace(logicalOperator))
                {
                    error = "Logical operator not found after SequenceNo=" + sequenceNo +
                            " (LogicalOperatorId=" + logicalOperatorId + ").";
                    expressionText = String.Join(" ", tokens.ToArray());
                    return false;
                }

                tokens.Add(logicalOperator);
            }
        }

        expressionText = String.Join(" ", tokens.ToArray());

        bool result;
        string expressionError;
        if (!EvaluateCriticalBooleanExpression(tokens, out result, out expressionError))
        {
            error = expressionError;
            return false;
        }

        return result;
    }

    private static bool EvaluateCriticalComparison(
        string actualValue,
        string compareValue,
        string compareValue2,
        string dataType,
        string operatorSymbol,
        out string error)
    {
        error = "";
        string op = (operatorSymbol ?? "").Trim().ToUpperInvariant();
        if (op == "==") op = "=";
        if (op == "<>") op = "!=";

        if (String.IsNullOrWhiteSpace(op))
        {
            error = "Operator symbol is blank.";
            return false;
        }

        if (op == "IN" || op == "NOT IN")
        {
            string[] values = (compareValue ?? "").Split(',');
            bool found = values.Any(delegate(string x)
            {
                return String.Equals(
                    (x ?? "").Trim(),
                    (actualValue ?? "").Trim(),
                    StringComparison.OrdinalIgnoreCase
                );
            });
            return op == "IN" ? found : !found;
        }

        string normalizedDataType = NormalizeCriticalKey(dataType);
        bool numericOperator = op == ">" || op == "<" || op == ">=" || op == "<=" || op == "BETWEEN";
        bool shouldCompareAsNumber = normalizedDataType == "NUMBER" || numericOperator;

        if (shouldCompareAsNumber)
        {
            decimal actualNumber;
            decimal expectedNumber;

            if (!TryParseCriticalDecimal(actualValue, out actualNumber))
            {
                error = "Actual value is not numeric: " + (actualValue ?? "");
                return false;
            }

            if (!TryParseCriticalDecimal(compareValue, out expectedNumber))
            {
                error = "Compare value is not numeric: " + (compareValue ?? "");
                return false;
            }

            if (op == "=") return actualNumber == expectedNumber;
            if (op == "!=") return actualNumber != expectedNumber;
            if (op == ">") return actualNumber > expectedNumber;
            if (op == "<") return actualNumber < expectedNumber;
            if (op == ">=") return actualNumber >= expectedNumber;
            if (op == "<=") return actualNumber <= expectedNumber;

            if (op == "BETWEEN")
            {
                decimal secondNumber;
                if (!TryParseCriticalDecimal(compareValue2, out secondNumber))
                {
                    error = "Second compare value is not numeric: " + (compareValue2 ?? "");
                    return false;
                }

                // BETWEEN is inclusive: >= first value AND <= second value.
                return actualNumber >= expectedNumber && actualNumber <= secondNumber;
            }

            error = "Unsupported numeric operator: " + op;
            return false;
        }

        int stringCompare = String.Compare(
            (actualValue ?? "").Trim(),
            (compareValue ?? "").Trim(),
            StringComparison.OrdinalIgnoreCase
        );

        if (op == "=") return stringCompare == 0;
        if (op == "!=") return stringCompare != 0;

        error = "Unsupported operator for DataType=" + dataType + ": " + op;
        return false;
    }

    private static bool TryParseCriticalDecimal(string value, out decimal result)
    {
        if (Decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            return true;

        if (Decimal.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out result))
            return true;

        result = 0;
        return false;
    }

    private static string ResolveCriticalOperatorSymbol(DataRow operatorMasterRow)
    {
        string symbol = GetCriticalRowValue(operatorMasterRow, "OperatorSymbol", "OperatorCode");
        if (!String.IsNullOrWhiteSpace(symbol))
            return symbol.Trim().ToUpperInvariant();

        string name = NormalizeCriticalKey(GetCriticalRowValue(operatorMasterRow, "OperatorName"));
        if (name == "EQUALTO" || name == "EQUAL") return "=";
        if (name == "NOTEQUALTO" || name == "NOTEQUAL") return "!=";
        if (name == "GREATERTHAN") return ">";
        if (name == "LESSTHAN") return "<";
        if (name == "GREATERTHANOREQUAL" || name == "GREATERTHANOREQUALTO") return ">=";
        if (name == "LESSTHANOREQUAL" || name == "LESSTHANOREQUALTO") return "<=";
        if (name == "BETWEEN") return "BETWEEN";
        if (name == "IN") return "IN";
        if (name == "NOTIN") return "NOT IN";
        return "";
    }

    private static string ResolveCriticalLogicalOperator(DataSet dsMaster, string logicalOperatorId)
    {
        if (dsMaster == null || dsMaster.Tables.Count <= 4 || String.IsNullOrWhiteSpace(logicalOperatorId))
            return "";

        DataRow logicalRow = FindCriticalMasterRow(dsMaster.Tables[4], logicalOperatorId, "LogicalOperatorId");
        if (logicalRow == null)
            return "";

        string value = GetCriticalRowValue(
            logicalRow,
            "OperatorName",
            "LogicalOperator",
            "LogicalOperatorName",
            "OperatorCode",
            "OperatorSymbol"
        );

        string key = NormalizeCriticalKey(value);
        if (key.Contains("AND")) return "AND";
        if (key.Contains("OR")) return "OR";
        return "";
    }

    private static void AddCriticalBracketTokens(List<string> tokens, string bracketValue, bool opening)
    {
        if (tokens == null || String.IsNullOrWhiteSpace(bracketValue))
            return;

        char wanted = opening ? '(' : ')';
        foreach (char ch in bracketValue)
        {
            if (ch == wanted)
                tokens.Add(wanted.ToString());
        }
    }

    private static bool EvaluateCriticalBooleanExpression(
        List<string> tokens,
        out bool result,
        out string error)
    {
        result = false;
        error = "";

        if (tokens == null || tokens.Count == 0)
        {
            error = "Boolean expression is empty.";
            return false;
        }

        List<string> postfix = new List<string>();
        Stack<string> operators = new Stack<string>();

        foreach (string rawToken in tokens)
        {
            string token = (rawToken ?? "").Trim().ToUpperInvariant();

            if (token == "TRUE" || token == "FALSE")
            {
                postfix.Add(token);
            }
            else if (token == "AND" || token == "OR")
            {
                while (operators.Count > 0 &&
                       operators.Peek() != "(" &&
                       GetCriticalBooleanPrecedence(operators.Peek()) >= GetCriticalBooleanPrecedence(token))
                {
                    postfix.Add(operators.Pop());
                }
                operators.Push(token);
            }
            else if (token == "(")
            {
                operators.Push(token);
            }
            else if (token == ")")
            {
                bool foundOpen = false;
                while (operators.Count > 0)
                {
                    string op = operators.Pop();
                    if (op == "(")
                    {
                        foundOpen = true;
                        break;
                    }
                    postfix.Add(op);
                }

                if (!foundOpen)
                {
                    error = "Closing bracket has no matching opening bracket.";
                    return false;
                }
            }
            else
            {
                error = "Invalid boolean token: " + token;
                return false;
            }
        }

        while (operators.Count > 0)
        {
            string op = operators.Pop();
            if (op == "(" || op == ")")
            {
                error = "Opening bracket has no matching closing bracket.";
                return false;
            }
            postfix.Add(op);
        }

        Stack<bool> values = new Stack<bool>();
        foreach (string token in postfix)
        {
            if (token == "TRUE")
            {
                values.Push(true);
            }
            else if (token == "FALSE")
            {
                values.Push(false);
            }
            else if (token == "AND" || token == "OR")
            {
                if (values.Count < 2)
                {
                    error = "Invalid AND/OR expression.";
                    return false;
                }

                bool right = values.Pop();
                bool left = values.Pop();
                values.Push(token == "AND" ? (left && right) : (left || right));
            }
        }

        if (values.Count != 1)
        {
            error = "Boolean expression did not reduce to one result.";
            return false;
        }

        result = values.Pop();
        return true;
    }

    private static int GetCriticalBooleanPrecedence(string logicalOperator)
    {
        if (String.Equals(logicalOperator, "AND", StringComparison.OrdinalIgnoreCase)) return 2;
        if (String.Equals(logicalOperator, "OR", StringComparison.OrdinalIgnoreCase)) return 1;
        return 0;
    }

    private static string NormalizeCriticalKey(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return "";

        char[] chars = value
            .ToUpperInvariant()
            .Where(delegate(char ch) { return Char.IsLetterOrDigit(ch); })
            .ToArray();

        return new string(chars);
    }

    private static string GetCriticalLevelCode(string tasktype)
    {
        string type = (tasktype ?? "").Trim().Replace(" ", "").ToUpperInvariant();

        if (type == "PROJECT") return "PROJECT";
        if (type == "MILESTONE") return "MILESTONE";
        if (type == "TASK") return "TASK";

        // Current end-user screen normally sends subtask.
        return "SUBTASK";
    }

    private static string GetCriticalRowValue(DataRow row, params string[] columnNames)
    {
        if (row == null || row.Table == null || columnNames == null)
            return "";

        foreach (string columnName in columnNames)
        {
            if (string.IsNullOrWhiteSpace(columnName))
                continue;

            foreach (DataColumn col in row.Table.Columns)
            {
                if (string.Equals(col.ColumnName, columnName, StringComparison.OrdinalIgnoreCase))
                {
                    if (row[col] == null || row[col] == DBNull.Value)
                        return "";

                    return row[col].ToString();
                }
            }
        }

        return "";
    }

    private static bool IsCriticalRuleActive(DataRow row)
    {
        // status must be 1 / true when the column is returned by the list SP.
        string statusValue = GetCriticalRowValue(row, "status", "Status");
        if (!string.IsNullOrWhiteSpace(statusValue))
        {
            bool statusBool;
            int statusInt;

            if (bool.TryParse(statusValue, out statusBool))
            {
                if (!statusBool) return false;
            }
            else if (int.TryParse(statusValue, out statusInt))
            {
                if (statusInt != 1) return false;
            }
            else
            {
                return false;
            }
        }

        // archive must be 0 / false when the column is returned by the list SP.
        // If archive is not returned, the existing list SP remains the source of the non-archived list.
        string archiveValue = GetCriticalRowValue(row, "archive", "Archive");
        if (!string.IsNullOrWhiteSpace(archiveValue))
        {
            bool archiveBool;
            int archiveInt;

            if (bool.TryParse(archiveValue, out archiveBool))
            {
                if (archiveBool) return false;
            }
            else if (int.TryParse(archiveValue, out archiveInt))
            {
                if (archiveInt != 0) return false;
            }
            else
            {
                return false;
            }
        }

        return true;
    }
    #endregion
    #region save transaction details

    // =========================================================
    // SINGLE RUNNING TIMER CHECK - read only
    // Existing start/pause/stop methods are intentionally unchanged.
    // Running means:
    // TimerStatus: active + not ended + not paused
    // PauseStatus: matching transaction/type with an open START row.
    // =========================================================
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static RunningTimerInfo GetRunningTimerBeforeStart(string selectedType = "", string selectedId = "")
    {
        RunningTimerInfo result = new RunningTimerInfo();
        result.hasRunning = false;

        try
        {
            HttpCookie regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            HttpCookie userCookie = HttpContext.Current.Request.Cookies["user"];
            if (regCookie == null || userCookie == null || regCookie["id"] == null || userCookie["user_id"] == null)
                throw new InvalidOperationException("User session is unavailable.");

            int regid;
            int userid;
            if (!Int32.TryParse(regCookie["id"].ToString(), out regid)) throw new InvalidOperationException("Invalid company session.");
            if (!Int32.TryParse(userCookie["user_id"].ToString(), out userid)) throw new InvalidOperationException("Invalid user session.");

            int clickedEntityId;
            Int32.TryParse(selectedId, out clickedEntityId);
            string safeType = (selectedType ?? "").Trim().ToLowerInvariant();
            if (safeType != "project" && safeType != "milestone" && safeType != "task" && safeType != "subtask")
                safeType = "";
            DataAccess da = new DataAccess();
            string sql = @"
select top(1)
       ts.id as TransactionId,
       lower(isnull(ts.type,'subtask')) as TaskType,
       isnull(ts.ProjectId,0) as ProjectId,
       isnull(ts.Milestoneid,0) as MilestoneId,
       isnull(ts.taskid,0) as TaskId,
       isnull(ts.subtaskid,0) as SubTaskId
from Pms_tbl_Timerstatus ts with(nolock)
where ts.regid=" + regid + @"
  and ts.uid=" + userid + @"
  and ts.status=1 and ts.archive=0
  and isnull(ts.isend,0)=0
  and isnull(ts.IsPaused,0)=0
  and lower(isnull(ts.type,'')) in ('subtask','task','milestone','project')
  and exists
  (
      select 1
      from Pms_Tbl_PauseStatus ps with(nolock)
      where ps.Transactionid=ts.id
        and ps.regid=ts.regid
        and ps.uid=ts.uid
        and ps.status=1 and ps.archive=0
        and lower(isnull(ps.type,''))=lower(isnull(ts.type,''))
        and lower(isnull(ps.entrytype,''))='start'
        and ps.tenddate is null
  )
  and not (lower(isnull(ts.type,''))='" + safeType + @"'
      and case lower(ts.type) when 'project' then ts.ProjectId
          when 'milestone' then ts.Milestoneid when 'task' then ts.taskid
          else ts.subtaskid end=" + clickedEntityId + @")
order by ts.id desc";

            DataSet ds = da.GetDataSetInline(sql);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return result;

            DataRow row = ds.Tables[0].Rows[0];
            string taskType = Convert.ToString(row["TaskType"]).Trim().ToLowerInvariant();
            int projectId = Convert.ToInt32(row["ProjectId"]);
            int milestoneId = Convert.ToInt32(row["MilestoneId"]);
            int taskId = Convert.ToInt32(row["TaskId"]);
            int subtaskId = Convert.ToInt32(row["SubTaskId"]);
            int entityId = taskType == "project" ? projectId :
                           taskType == "milestone" ? milestoneId :
                           taskType == "task" ? taskId : subtaskId;

            // If the row being clicked is already the same running entity, no switch popup is required.
            int clickedId;
            if (Int32.TryParse(selectedId, out clickedId) && clickedId > 0 &&
                String.Equals((selectedType ?? "").Trim(), taskType, StringComparison.OrdinalIgnoreCase) &&
                clickedId == entityId)
                return result;

            string taskName = "";
            string serialNo = "";

            try
            {
            if (taskType == "project")
            {
                string q = "select top(1) ProjectName from Pms_tbl_AddProject with(nolock) where id=" + projectId +
                           " and regid=" + regid + " and status=1 and archive=0";
                DataSet dn = da.GetDataSetInline(q);
                if (dn.Tables.Count > 0 && dn.Tables[0].Rows.Count > 0)
                    taskName = Convert.ToString(dn.Tables[0].Rows[0]["ProjectName"]);
                serialNo = projectId.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                taskName = get_taskname(entityId.ToString(CultureInfo.InvariantCulture), taskType);
                int[] order = GetTaskHierarchyOrderNumbers(da, regid.ToString(CultureInfo.InvariantCulture), entityId.ToString(CultureInfo.InvariantCulture), taskType);
                if (taskType == "milestone")
                    serialNo = order[0] > 0 ? order[0].ToString(CultureInfo.InvariantCulture) : "";
                else if (taskType == "task")
                    serialNo = (order[0] > 0 ? order[0].ToString(CultureInfo.InvariantCulture) : "") +
                               (order[1] > 0 ? "." + order[1].ToString(CultureInfo.InvariantCulture) : "");
                else
                    serialNo = (order[0] > 0 ? order[0].ToString(CultureInfo.InvariantCulture) : "") +
                               (order[1] > 0 ? "." + order[1].ToString(CultureInfo.InvariantCulture) : "") +
                               (order[2] > 0 ? "." + order[2].ToString(CultureInfo.InvariantCulture) : "");
            }

            }
            catch (Exception displayEx) { SaveLogText("RunningTimerDisplay-" + displayEx.Message); }

            result.hasRunning = true;
            result.transactionId = Convert.ToInt32(row["TransactionId"]);
            result.taskType = taskType;
            result.typeName = taskType == "subtask" ? "Sub Task" :
                              taskType == "task" ? "Task" :
                              taskType == "milestone" ? "Milestone" : "Project";
            result.entityId = entityId;
            result.projectId = projectId;
            result.milestoneId = milestoneId;
            result.taskId = taskId;
            result.subtaskId = subtaskId;
            result.taskName = taskName;
            result.serialNo = serialNo;
        }
        catch (Exception ex)
        {
            SaveLogText("GetRunningTimerBeforeStartEX-" + ex.Message);
            throw new InvalidOperationException("Unable to check running timers.", ex);
        }

        return result;
    }

    [WebMethod]    
    public static string SaveTimerstartdetails(string tasktype, string ProjectId = "", string Milestoneid = "",string taskid = "", string id = "")
    {
        int retid = 0;
        DataAccess da1 = new DataAccess();
        string entrytype = "Insert";
        if (tasktype == string.Empty)
            tasktype = "subtask";
        try
        {
            string strsqlselect = "select id,isnull(isend,0) as isend from Pms_tbl_Timerstatus with(nolock)  where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and projectid='" + ProjectId + "'  and milestoneid='" + Milestoneid + "' and taskid='" + taskid + "' and   subtaskid='" + id + "'  and uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"].ToString() + "'  and status=1 and archive=0 and type='" + tasktype + "'";
            if(tasktype=="task")
                strsqlselect = "select id,isnull(isend,0) as isend from Pms_tbl_Timerstatus with(nolock)  where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and projectid='" + ProjectId + "'  and milestoneid='" + Milestoneid + "' and taskid='" + id + "'  and uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"].ToString() + "'  and status=1 and archive=0 and type='" + tasktype + "'";
            if (tasktype=="milestone")
                strsqlselect = "select id,isnull(isend,0) as isend from Pms_tbl_Timerstatus with(nolock)  where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and projectid='" + ProjectId + "'  and milestoneid='" + id + "'  and uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"].ToString() + "'  and status=1 and archive=0 and type='" + tasktype + "'";
            DataSet dscheck = da1.GetDataSetInline(strsqlselect);
            if (dscheck.Tables[0].Rows.Count > 0)
            {
                retid = Convert.ToInt32(dscheck.Tables[0].Rows[0]["id"].ToString());
                if(dscheck.Tables[0].Rows[0]["isend"].ToString()=="1")
                {
                    string strupdatestatus = "update Pms_tbl_Timerstatus set isend=0,IsPaused=0 where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and projectid='" + ProjectId + "'  and milestoneid='" + Milestoneid + "' and taskid='" + taskid + "' and   subtaskid='" + id + "' and status=1 and archive=0 and type='subtask'";
                    if (tasktype == "task")
                        strupdatestatus = "update Pms_tbl_Timerstatus set isend=0,IsPaused=0 where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and projectid='" + ProjectId + "'  and milestoneid='" + Milestoneid + "' and taskid='" + id + "' and status=1 and archive=0 and type='task'";
                    if (tasktype == "milestone")
                        strupdatestatus = "update Pms_tbl_Timerstatus set isend=0,IsPaused=0 where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and projectid='" + ProjectId + "'  and milestoneid='" + id + "' and status=1 and archive=0 and type='milestone'";
                    //SaveLogText("strpauseddetails123456=" + strupdatestatus);
                    da1.executeDMLQueryinline(strupdatestatus);
                }
            }
            else
            {

                SqlParameter[] prm = new SqlParameter[11];
                prm[0] = new SqlParameter("@projectid", ProjectId);
                prm[1] = new SqlParameter("@milestoneid", Milestoneid);
                prm[2] = new SqlParameter("@taskid", taskid);
                prm[3] = new SqlParameter("@subtaskid", id);
                if (tasktype == "task")
                {                    
                    prm[1] = new SqlParameter("@milestoneid", Milestoneid);
                    prm[2] = new SqlParameter("@taskid", id);
                    prm[3] = new SqlParameter("@subtaskid",DBNull.Value);
                }
                else if (tasktype == "milestone")
                {                    
                    prm[1] = new SqlParameter("@milestoneid", id);
                    prm[2] = new SqlParameter("@taskid", DBNull.Value);
                    prm[3] = new SqlParameter("@subtaskid", DBNull.Value);
                }
                prm[4] = new SqlParameter("@StartTime", da1.getcdatetime());
                prm[5] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prm[6] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                prm[7] = new SqlParameter("@cdate", da1.getcdatetime());
                prm[8] = new SqlParameter("@type", entrytype);
                prm[9] = new SqlParameter("@tasktype", tasktype);
                prm[10] = new SqlParameter("ReturnValue", SqlDbType.Int);
                prm[10].Direction = ParameterDirection.ReturnValue;
                retid = da1.executeDMLQuery("pmstbl_SubTask_StartTimerDetailsSave_ang", prm);
                SaveLogText("savetimer-"+ retid);
            }
            if(retid>0)
            {
                SqlParameter[] prm1 = new SqlParameter[13];
                prm1[0] = new SqlParameter("@projectId", ProjectId);
                prm1[1] = new SqlParameter("@milestoneid", Milestoneid);
                prm1[2] = new SqlParameter("@taskid", taskid);
                prm1[3] = new SqlParameter("@subtaskid", id);
                if (tasktype == "task")
                {
                    prm1[1] = new SqlParameter("@milestoneid", Milestoneid);
                    prm1[2] = new SqlParameter("@taskid", id);
                    prm1[3] = new SqlParameter("@subtaskid", DBNull.Value);                    
                }
                else if (tasktype == "milestone")
                {
                    prm1[1] = new SqlParameter("@milestoneid", id);
                    prm1[2] = new SqlParameter("@taskid", DBNull.Value);
                    prm1[3] = new SqlParameter("@subtaskid", DBNull.Value);                   
                }
                prm1[4] = new SqlParameter("@Transactionid", retid);
                prm1[5] = new SqlParameter("@Notes", "");
                prm1[6] = new SqlParameter("@tstartdate", da1.getcdatetime());
                prm1[7] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prm1[8] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                prm1[9] = new SqlParameter("@cdate", da1.getcdatetime());
                prm1[10] = new SqlParameter("@type", entrytype);
                prm1[11] = new SqlParameter("@entrytype", "start");
                prm1[12] = new SqlParameter("@tasktype", tasktype);
                da1.executeDMLQuery("Pms_Tbl_PauseStatus_startDetailsSave_ang", prm1);
                
            }

            string strstatusupdate = "update Pms_tbl_Addsubtask set statuscomplete=1 where id='" + id + "' and status=1 and archive=0";
            if (tasktype == "task")
                strstatusupdate = "update Pms_tbl_Addtask set statuscomplete=1 where id='" + id + "' and status=1 and archive=0";
            if (tasktype == "milestone")
                strstatusupdate = "update Pms_tbl_Addmilestone set statuscomplete=1 where id='" + id + "' and status=1 and archive=0";
            DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);
        }
        catch(Exception ex)
        {
            SaveLogText("strsqlselect1-" + ex.Message);
        }
        // Critical Workflow Step 2: timer started successfully.
        if (retid > 0)
        {
            //CheckCriticalWorkflow(tasktype, id);
        }

        return retid.ToString();

    }
    [WebMethod]
    public static string SaveTimerpausedetails(string tasktype, string ProjectId = "", string Milestoneid = "", string taskid = "", string id = "", string Notes = "")
    {
        SaveLogText("CallIt-1");
        string retval = "0";
        DataAccess da1 = new DataAccess();
        string entrytype = "Insert";
        if (tasktype == string.Empty)
            tasktype = "subtask";
        string transactiondate = da1.getcdatetime();
        string transactionid = "0";
        
        string pausetime = "";
        string spam1 = "";
        string spam2 = "";
        try
        {
            string strhourminuts = "select *  from Pms_tbl_Timerstatus where type='" + tasktype + "' and subtaskid='" + id + "' and taskid='" + taskid + "' and ProjectId='" + ProjectId + "' and Milestoneid='" + Milestoneid + "'  and uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"].ToString() + "'  and status=1 and archive=0 order by id desc";
            if (tasktype == "task")
                strhourminuts = "select *  from Pms_tbl_Timerstatus where type='" + tasktype + "' and  taskid='" + id + "' and ProjectId='" + ProjectId + "' and Milestoneid='" + Milestoneid + "'  and uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"].ToString() + "'  and status=1 and archive=0   order by id desc";
            if (tasktype == "milestone")
                strhourminuts = "select *  from Pms_tbl_Timerstatus where type='" + tasktype + "' and Milestoneid='" + id + "'  and ProjectId='" + ProjectId + "'   and uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"].ToString() + "'  and status=1 and archive=0   order by id desc";
            SaveLogText("strhourminuts-" + strhourminuts);
            DataSet dshourmin = da1.GetDataSetInline(strhourminuts);
            if (dshourmin.Tables[0].Rows.Count > 0)
            {
                transactionid = dshourmin.Tables[0].Rows[0]["id"].ToString();
                transactiondate = dshourmin.Tables[0].Rows[0]["StartTime"].ToString();
            }
            SaveLogText("pausedeatils1-"+ strhourminuts);
            if (transactionid != string.Empty && transactionid != "0")
            {
                string strsqllastpause = "select top(1) *  from Pms_Tbl_PauseStatus where type='"+ tasktype + "' and subtaskid='" + id + "' and taskid='" + taskid + "' and ProjectId='" + ProjectId + "' and Milestoneid='" + Milestoneid + "' and transactionid='" + transactionid + "' and entrytype='start' and tenddate is null and status=1 and archive=0 order by id desc";
                if (tasktype == "task")
                    strsqllastpause = "select top(1) *  from Pms_Tbl_PauseStatus where type='" + tasktype + "' and taskid='" + id + "' and ProjectId='" + ProjectId + "' and Milestoneid='" + Milestoneid + "' and transactionid='" + transactionid + "' and entrytype='start' and tenddate is null and status=1 and archive=0 order by id desc";
                if (tasktype == "milestone")
                    strsqllastpause = "select top(1) *  from Pms_Tbl_PauseStatus where type='" + tasktype + "' and Milestoneid='" + id + "' and ProjectId='" + ProjectId + "'  and transactionid='" + transactionid + "' and entrytype='start' and tenddate is null and status=1 and archive=0 order by id desc";
                SaveLogText("pausedeatils2-" + strsqllastpause);
                DataSet dsP = da1.GetDataSetInline(strsqllastpause);
                if (dsP.Tables[0].Rows.Count > 0)
                {
                    string hour = "00";
                    string min = "00";
                    string sec = "00";
                    string mili = "00";
                    TimeSpan toatlhr = (DateTime.Now - Convert.ToDateTime(dsP.Tables[0].Rows[0]["tstartdate"].ToString()));
                    hour = toatlhr.Hours.ToString("00");
                    min = toatlhr.Minutes.ToString("00");
                    sec = toatlhr.Seconds.ToString("00");
                    mili = toatlhr.Milliseconds.ToString("000");
                    string totalmin = (toatlhr.Hours * 60 + toatlhr.Minutes + toatlhr.Seconds / 60).ToString();
                    string strresumedetails = "update Pms_Tbl_PauseStatus set notes='"+Notes+"', entrytype='pause', tenddate='" + da1.getcdatetime() + "', StartDatetime='" + da1.getcdatetime() + "' ,PausedTime='" + totalmin + "',hour='" + hour + "',minute='" + min + "',second='" + sec + "',millisecond='" + mili + "' where Transactionid='" + transactionid + "' and id='" + dsP.Tables[0].Rows[0]["id"].ToString() + "' and type='"+ tasktype + "' and status=1 and archive=0;update Pms_tbl_Timerstatus set IsPaused=1 where id='"+transactionid+ "' and type='" + tasktype + "'";

                    SaveLogText("pausedeatils3-" + strresumedetails);
                    da1.executeDMLQueryinline(strresumedetails);
                    string sqlselectall = "select isnull(sum(isnull(cast(PausedTime as int),0)),0) as totalmin from Pms_Tbl_PauseStatus where status=1 and archive=0 and Transactionid='" + transactionid + "' and entrytype in ('pause','stop') and regid='"+ HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and type='" + tasktype + "'";
                    SaveLogText("pausedeatils4-" + sqlselectall);
                    DataSet dsTM= da1.GetDataSetInline(sqlselectall);
                    string cr_hour = "00";
                    string cr_min = "00";
                    string cr_sec = "00";
                    string cr_mili = "00";
                    try
                    {
                        
                        string formattime = ConvertMinutesToTimer(Convert.ToDouble(dsTM.Tables[0].Rows[0]["totalmin"].ToString()));
                        string[] tim = formattime.Split(':');
                        if(tim.Length>0)
                        {
                            cr_hour = tim[0];
                            cr_min = tim[1];
                            cr_sec = tim[2];
                            cr_mili = tim[3];
                        }
                    }
                    catch { }
                    // string strstatusupdate = "update Pms_tbl_Timerstatus set hour='"+ cr_hour + "',minute='" + cr_min + "',second='" + cr_sec + "',millisecond='" + cr_mili + "'  where id='" + transactionid+ "'  and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0";
                    //da1.executeDMLQueryinline(strstatusupdate);
                    string ac_hour = "00";
                    string ac_min = "00";
                    string ac_sec = "00";
                    string ac_mili = "00";
                    TimeSpan ac_toatlhr = (DateTime.Now - Convert.ToDateTime(transactiondate));
                    ac_hour = ac_toatlhr.Hours.ToString("00");
                    ac_min = ac_toatlhr.Minutes.ToString("00");
                    ac_sec = ac_toatlhr.Seconds.ToString("00");
                    ac_mili = ac_toatlhr.Milliseconds.ToString("000");
                    string ac_totalmin = (ac_toatlhr.Hours * 60 + ac_toatlhr.Minutes + ac_toatlhr.Seconds / 60).ToString();

                    string strstatusupdate = "update Pms_tbl_Timerstatus set hour='" + cr_hour + "',minute='" + cr_min + "',second='" + cr_sec + "',millisecond='" + cr_mili + "',act_hour='" + ac_hour + "',act_minute='" + ac_min + "',act_second='" + ac_sec + "',act_millisecond='" + ac_mili + "',TimerNotes='"+Notes+"'  where id='" + transactionid + "'  and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0 and type='" + tasktype + "'";
                    SaveLogText("pausedeatils5-" + strstatusupdate);
                    da1.executeDMLQueryinline(strstatusupdate);
                }
            }
          
            
            
            retval = "1";
        }
        catch
        {

        }
        // Critical Workflow Step 2: latest spent time is already saved above.
        if (retval == "1")
        {
            //CheckCriticalWorkflow(tasktype, id);
        }

        return retval;

    }
   
    [WebMethod]
    public static void SaveTimerresumedetails(string tasktype, string ProjectId = "", string Milestoneid = "", string taskid = "", string id = "")
    {
        CommonFunction cmf = new CommonFunction();
        DataAccess da = new DataAccess();
        string hour = "";
        string min = "";
        string sec = "";
        string mili = "";
        string pausetime = "";
        int ispause = 0;
        int isend = 0;
        string transactionid = "0";
        if (tasktype == string.Empty)
            tasktype = "subtask";
        string strhourminuts = "select *  from Pms_tbl_Timerstatus where subtaskid='" + id + "' and taskid='" + taskid + "' and Milestoneid='" + Milestoneid + "' and ProjectId='" + ProjectId + "'   and uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"].ToString() + "' and type='"+tasktype+"' and status=1 and archive=0 order by id desc";
        if(tasktype=="task")
            strhourminuts = "select *  from Pms_tbl_Timerstatus where taskid='" + id + "' and Milestoneid='" + Milestoneid + "' and ProjectId='" + ProjectId + "'   and uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"].ToString() + "' and type='" + tasktype + "' and status=1 and archive=0 order by id desc";
        if (tasktype == "milestone")
            strhourminuts = "select *  from Pms_tbl_Timerstatus where  Milestoneid='" + id + "' and ProjectId='" + ProjectId + "'   and uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"].ToString() + "' and type='" + tasktype + "' and status=1 and archive=0 order by id desc";
        SaveLogText("strhourminuts11-" + strhourminuts);
        DataSet dshourmin = da.GetDataSetInline(strhourminuts);
        if (dshourmin.Tables[0].Rows.Count > 0)
        {
            transactionid = dshourmin.Tables[0].Rows[0]["id"].ToString();
            if (dshourmin.Tables[0].Rows[0]["IsEnd"].ToString() == "1")
            {
                isend = 1;
            }
            else
            {
                isend = 0;
            }
            SaveLogText("strpauseddetails12=" + dshourmin.Tables[0].Rows[0]["IsPaused"].ToString());
            if (dshourmin.Tables[0].Rows[0]["IsPaused"].ToString() == "1")
            {
                ispause = 1;
                string strresumedetails = "update  Pms_tbl_Timerstatus set IsPaused=0 where id='" + transactionid + "' and type='" + tasktype + "'";

                SaveLogText("strpauseddetails1234=" + strresumedetails);
                try
                {
                    da.GetDataSetInline(strresumedetails);
                }
                catch (Exception ex)
                {
                    SaveLogText("strpauseddetails12345=" + ex.Message);
                }
                SqlParameter[] prm1 = new SqlParameter[13];
                prm1[0] = new SqlParameter("@projectId", ProjectId);                
                prm1[1] = new SqlParameter("@milestoneid", Milestoneid);
                prm1[2] = new SqlParameter("@taskid", taskid);
                prm1[3] = new SqlParameter("@subtaskid", id);
                
                if(tasktype=="task")
                {
                    prm1[1] = new SqlParameter("@milestoneid", Milestoneid);
                    prm1[2] = new SqlParameter("@taskid", id);
                    prm1[3] = new SqlParameter("@subtaskid",DBNull.Value);                    
                }
                else if (tasktype == "milestone")
                {
                    prm1[1] = new SqlParameter("@milestoneid", id);
                    prm1[2] = new SqlParameter("@taskid",DBNull.Value);
                    prm1[3] = new SqlParameter("@subtaskid",DBNull.Value);                    
                }
                prm1[4] = new SqlParameter("@Transactionid", transactionid);
                prm1[5] = new SqlParameter("@Notes", "");
                prm1[6] = new SqlParameter("@tstartdate", da.getcdatetime());
                prm1[7] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prm1[8] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                prm1[9] = new SqlParameter("@cdate", da.getcdatetime());
                prm1[10] = new SqlParameter("@type", "Insert");
                prm1[11] = new SqlParameter("@entrytype", "start");
                prm1[12] = new SqlParameter("@tasktype", tasktype);
                da.executeDMLQuery("Pms_Tbl_PauseStatus_startDetailsSave_ang", prm1);

            }


        }



    }


    [WebMethod]
    public static void SaveTimerresumedetailsold(string ProjectId = "", string Milestoneid = "", string taskid = "", string subtaskid = "")
    {
        CommonFunction cmf = new CommonFunction();
        DataAccess da = new DataAccess();
        string hour = "";
        string min = "";
        string sec = "";
        string mili = "";
        string pausetime = "";
        int ispause = 0;
        int isend = 0;
        string transactionid = "0";
        string strhourminuts = "select *  from Pms_tbl_Timerstatus where subtaskid='" + subtaskid + "' and taskid='" + taskid + "' and ProjectId='" + ProjectId + "' and Milestoneid='" + Milestoneid + "' and uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"].ToString() + "'  and type='subtask' and status=1 and archive=0 order by id desc";
        SaveLogText("strhourminuts11-" + strhourminuts);
        DataSet dshourmin = da.GetDataSetInline(strhourminuts);
        if (dshourmin.Tables[0].Rows.Count > 0)
        {
            transactionid = dshourmin.Tables[0].Rows[0]["id"].ToString();
            if (dshourmin.Tables[0].Rows[0]["IsEnd"].ToString() == "1")
            {
                isend = 1;
            }
            else
            {
                isend = 0;
            }
            SaveLogText("strpauseddetails12=" + dshourmin.Tables[0].Rows[0]["IsPaused"].ToString());
            if (dshourmin.Tables[0].Rows[0]["IsPaused"].ToString() == "1")
            {
                ispause = 1;
                string strpauseddetails = "select top(1) *  from Pms_Tbl_PauseStatus where Transactionid='" + transactionid + "' and type='subtask' and status=1 and archive=0 order by id desc";
                SaveLogText("strpauseddetails123=" + strpauseddetails);
                DataSet dspaused = da.GetDataSetInline(strpauseddetails);

                if (dspaused.Tables[0].Rows.Count > 0)
                {
                    TimeSpan toatlhr = (DateTime.Now - Convert.ToDateTime(dspaused.Tables[0].Rows[0]["PauseDatetime"].ToString()));
                    hour = toatlhr.Hours.ToString("00");
                    min = toatlhr.Minutes.ToString("00");
                    sec = toatlhr.Seconds.ToString("00");
                    mili = toatlhr.Milliseconds.ToString("000");
                    string totalmin = (toatlhr.Hours * 60 + toatlhr.Minutes + toatlhr.Seconds / 60).ToString();
                    string strresumedetails = "update Pms_Tbl_PauseStatus set StartDatetime='" + da.getcdatetime() + "' ,PausedTime='" + totalmin + "' ,hour='" + hour + "',minute='" + min + "',second='" + sec + "',millisecond='" + mili + "',StartByid='" + HttpContext.Current.Request.Cookies["user"]["user_id"].ToString() + "',tenddate='" + da.getcdatetime() + "' where Transactionid='" + transactionid + "' and id='" + dspaused.Tables[0].Rows[0]["id"].ToString() + "' and type='subtask' and status=1 and archive=0;" +
                        "update  Pms_tbl_Timerstatus set IsPaused=0 where id='" + transactionid + "' and type='subtask'";
                    SaveLogText("strpauseddetails1234=" + strresumedetails);
                    try
                    {
                        da.GetDataSetInline(strresumedetails);
                    }
                    catch (Exception ex)
                    {
                        SaveLogText("strpauseddetails12345=" + ex.Message);
                    }
                }
                SqlParameter[] prm1 = new SqlParameter[12];
                prm1[0] = new SqlParameter("@projectId", ProjectId);
                prm1[1] = new SqlParameter("@milestoneid", Milestoneid);
                prm1[2] = new SqlParameter("@taskid", taskid);
                prm1[3] = new SqlParameter("@subtaskid", subtaskid);
                prm1[4] = new SqlParameter("@Transactionid", transactionid);
                prm1[5] = new SqlParameter("@Notes", "");
                prm1[6] = new SqlParameter("@tstartdate", da.getcdatetime());
                prm1[7] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prm1[8] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                prm1[9] = new SqlParameter("@cdate", da.getcdatetime());
                prm1[10] = new SqlParameter("@type", "Insert");
                prm1[11] = new SqlParameter("@entrytype", "start");
                da.executeDMLQuery("Pms_Tbl_PauseStatus_startDetailsSave_ang", prm1);

            }


        }



    }

    [WebMethod]
    public static string SaveTimer(string tasktype,string ProjectId = "", string Milestoneid = "", string taskid = "", string subtaskid = "", string timernotes = "",string timerdetails="")
    {
        DataAccess da1 = new DataAccess();
        string entrytype = "update";
        string transactiondate = da1.getcdatetime();
        string transactionid = "0";
        string retval = "0";
           string strtrans = "select  *  from Pms_tbl_Timerstatus where type='"+ tasktype + "' and subtaskid='" + subtaskid + "' and taskid='" + taskid + "' and ProjectId='" + ProjectId + "' and Milestoneid='" + Milestoneid + "' and uid='"+ HttpContext.Current.Request.Cookies["user"]["user_id"].ToString() + "' and status=1 and archive=0 order by id desc";
        if(tasktype=="task")
            strtrans = "select  *  from Pms_tbl_Timerstatus where type='" + tasktype + "'  and taskid='" + subtaskid + "' and ProjectId='" + ProjectId + "' and Milestoneid='" + Milestoneid + "' and uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"].ToString() + "' and status=1 and archive=0 order by id desc";
        if (tasktype == "milestone")
            strtrans = "select  *  from Pms_tbl_Timerstatus where type='" + tasktype + "'  and ProjectId='" + ProjectId + "' and Milestoneid='" + subtaskid + "' and uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"].ToString() + "' and status=1 and archive=0 order by id desc";

        SaveLogText("pausedeatils2A-" + strtrans);
        DataSet dstrans = da1.GetDataSetInline(strtrans);
            if (dstrans.Tables[0].Rows.Count > 0)
            {
                transactionid = dstrans.Tables[0].Rows[0]["id"].ToString();
                transactiondate= dstrans.Tables[0].Rows[0]["StartTime"].ToString();
            }

        string strsqllastpause = "select top(1) *  from Pms_Tbl_PauseStatus where type='"+ tasktype + "'   and transactionid='" + transactionid + "' and entrytype='start' and tenddate is null and status=1 and archive=0 order by id desc";
       
                SaveLogText("pausedeatils2-" + strsqllastpause);
        DataSet dsP = da1.GetDataSetInline(strsqllastpause);
        if (dsP.Tables[0].Rows.Count > 0)
        {
            string hour = "00";
            string min = "00";
            string sec = "00";
            string mili = "00";
            TimeSpan toatlhr = (DateTime.Now - Convert.ToDateTime(dsP.Tables[0].Rows[0]["tstartdate"].ToString()));
            hour = toatlhr.Hours.ToString("00");
            min = toatlhr.Minutes.ToString("00");
            sec = toatlhr.Seconds.ToString("00");
            mili = toatlhr.Milliseconds.ToString("000");
            string totalmin = (toatlhr.Hours * 60 + toatlhr.Minutes + toatlhr.Seconds / 60).ToString();
            string strresumedetails = "update Pms_Tbl_PauseStatus set entrytype='stop', tenddate='" + da1.getcdatetime() + "',notes='" + timernotes + "', StartDatetime='" + da1.getcdatetime() + "' ,PausedTime='" + totalmin + "',hour='" + hour + "',minute='" + min + "',second='" + sec + "',millisecond='" + mili + "' where Transactionid='" + transactionid + "' and id='" + dsP.Tables[0].Rows[0]["id"].ToString() + "' and type='"+tasktype+"' and status=1 and archive=0;"; 
                
            SaveLogText("pausedeatils3-" + strresumedetails);
            da1.executeDMLQueryinline(strresumedetails);
            string sqlselectall = "select isnull(sum(isnull(cast(PausedTime as int),0)),0) as totalmin from Pms_Tbl_PauseStatus where status=1 and archive=0 and type='" + tasktype + "' and Transactionid='" + transactionid + "' and entrytype in ('pause','stop') and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
            SaveLogText("pausedeatils4-" + sqlselectall);
            DataSet dsTM = da1.GetDataSetInline(sqlselectall);
            string cr_hour = "00";
            string cr_min = "00";
            string cr_sec = "00";
            string cr_mili = "00";
            try
            {

                string formattime = ConvertMinutesToTimer(Convert.ToDouble(dsTM.Tables[0].Rows[0]["totalmin"].ToString()));
                string[] tim = formattime.Split(':');
                if (tim.Length > 0)
                {
                    cr_hour = tim[0];
                    cr_min = tim[1];
                    cr_sec = tim[2];
                    cr_mili = tim[3];
                }
            }
            catch { }
            string ac_hour = "00";
            string ac_min = "00";
            string ac_sec = "00";
            string ac_mili = "00";
            TimeSpan ac_toatlhr = (DateTime.Now - Convert.ToDateTime(transactiondate));
            ac_hour = ac_toatlhr.Hours.ToString("00");
            ac_min = ac_toatlhr.Minutes.ToString("00");
            ac_sec = ac_toatlhr.Seconds.ToString("00");
            ac_mili = ac_toatlhr.Milliseconds.ToString("000");
            string ac_totalmin = (ac_toatlhr.Hours * 60 + ac_toatlhr.Minutes + ac_toatlhr.Seconds / 60).ToString();

            string strstatusupdate = "update Pms_tbl_Timerstatus set hour='" + cr_hour + "',minute='" + cr_min + "',second='" + cr_sec + "',millisecond='" + cr_mili + "',act_hour='" + ac_hour + "',act_minute='" + ac_min + "',act_second='" + ac_sec + "',act_millisecond='" + ac_mili + "'  where type='"+tasktype+"' and id='" + transactionid + "'  and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0;"+
                "update Pms_tbl_Timerstatus set IsPaused=0 , isend=1,endtime='" + da1.getcdatetime() + "',TimerNotes='" + timernotes + "' where id='" + transactionid + "' and type='" + tasktype + "'  ";
            SaveLogText("pausedeatils5-" + strstatusupdate);
            da1.executeDMLQueryinline(strstatusupdate);
            retval = "1";
        }
        else
        {
            string sqlselectall = "select isnull(sum(isnull(cast(PausedTime as int),0)),0) as totalmin from Pms_Tbl_PauseStatus where status=1 and archive=0 and  type='" + tasktype + "' and Transactionid='" + transactionid + "' and entrytype in ('pause','stop') and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
            SaveLogText("pausedeatils4-" + sqlselectall);
            DataSet dsTM = da1.GetDataSetInline(sqlselectall);
            string cr_hour = "00";
            string cr_min = "00";
            string cr_sec = "00";
            string cr_mili = "00";
            try
            {

                string formattime = ConvertMinutesToTimer(Convert.ToDouble(dsTM.Tables[0].Rows[0]["totalmin"].ToString()));
                string[] tim = formattime.Split(':');
                if (tim.Length > 0)
                {
                    cr_hour = tim[0];
                    cr_min = tim[1];
                    cr_sec = tim[2];
                    cr_mili = tim[3];
                }
            }
            catch { }
            string ac_hour = "00";
            string ac_min = "00";
            string ac_sec = "00";
            string ac_mili = "00";
            TimeSpan ac_toatlhr = (DateTime.Now - Convert.ToDateTime(transactiondate));
            ac_hour = ac_toatlhr.Hours.ToString("00");
            ac_min = ac_toatlhr.Minutes.ToString("00");
            ac_sec = ac_toatlhr.Seconds.ToString("00");
            ac_mili = ac_toatlhr.Milliseconds.ToString("000");
            string ac_totalmin = (ac_toatlhr.Hours * 60 + ac_toatlhr.Minutes + ac_toatlhr.Seconds / 60).ToString();

            string strstatusupdate = "update Pms_tbl_Timerstatus set hour='" + cr_hour + "',minute='" + cr_min + "',second='" + cr_sec + "',millisecond='" + cr_mili + "',act_hour='" + ac_hour + "',act_minute='" + ac_min + "',act_second='" + ac_sec + "',act_millisecond='" + ac_mili + "'  where id='" + transactionid + "' and  type='" + tasktype + "'  and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0;" +
                "update Pms_tbl_Timerstatus  set TimerNotes='" + timernotes + "',IsPaused=0 , isend=1,endtime='" + da1.getcdatetime() + "' where id='" + transactionid + "' and  type='" + tasktype + "'";
            SaveLogText("pausedeatils5-" + strstatusupdate);
            da1.executeDMLQueryinline(strstatusupdate);
            retval = "1";
        }        
        // Critical Workflow Step 2: final spent time is already saved above.
        if (retval == "1")
        {
            //CheckCriticalWorkflow(tasktype, subtaskid);
        }

        return retval;
    }
[WebMethod]
    public static string SaveTimerold(string ProjectId = "", string Milestoneid = "", string taskid = "", string subtaskid = "", string timernotes = "",string timerdetails="")
    {
        DataAccess da1 = new DataAccess();
        string entrytype = "update";
        string transactionid = "0";
        string retval = "0";
        string hour = "";
        string minute = "";
        string second = "";
        string millisecond = "";
        if(timerdetails!=string.Empty)
        {
            string[] artim = timerdetails.Split(':');
            if(artim.Length>0)
            {
                try
                {
                    hour = artim[0];
                    minute = artim[1];
                    second = artim[2];
                    millisecond = artim[3];
                }
                catch { }
            }
        }
        try
        {
            string strhourminuts = "select *  from Pms_tbl_Timerstatus where isnull(isend,0)=0 and type='subtask' and subtaskid='" + subtaskid + "' and taskid='" + taskid + "' and ProjectId='" + ProjectId + "' and Milestoneid='" + Milestoneid + "' and status=1 and archive=0 order by id desc";
            DataSet dshourmin = da1.GetDataSetInline(strhourminuts);
            if (dshourmin.Tables[0].Rows.Count > 0)
            {
                transactionid = dshourmin.Tables[0].Rows[0]["id"].ToString();
            }
            SaveLogText("hourAV="+ hour);
            SqlParameter[] prm = new SqlParameter[16];
            prm[0] = new SqlParameter("@ProjectId", ProjectId);
            prm[1] = new SqlParameter("@Milestoneid", Milestoneid);
            prm[2] = new SqlParameter("@tasklist", DBNull.Value);
            prm[3] = new SqlParameter("@taskid", taskid);
            prm[4] = new SqlParameter("@subtaskid", subtaskid);
            prm[5] = new SqlParameter("@hour", hour);
            prm[6] = new SqlParameter("@minute", minute);
            prm[7] = new SqlParameter("@second", second);
            prm[8] = new SqlParameter("@millisecond", millisecond);
            prm[9] = new SqlParameter("@id", transactionid);
            prm[10] = new SqlParameter("@EndTime", da1.getcdatetime());
            prm[11] = new SqlParameter("@TimerNotes", timernotes);
            prm[12] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[13] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prm[14] = new SqlParameter("@cdate", da1.getcdatetime());
            prm[15] = new SqlParameter("@type", entrytype);
            SaveLogText("hourAV2=" + prm[0].Value+","+ prm[1].Value + "," + prm[2].Value + "," + prm[3].Value + "," + prm[4].Value + "," + prm[5].Value + "," + prm[6].Value + "," + prm[7].Value + "," + prm[8].Value + "," + prm[9].Value + "," + prm[10].Value + "," + prm[11].Value + "," + prm[12].Value + "," + prm[13].Value + "," + prm[14].Value + "," + prm[15].Value);
            da1.executeDMLQuery("pmstbl_SubTask_TimerDetailsSaveUpdate", prm);
            string strstatusupdate = "update Pms_tbl_Addsubtask set statuscomplete=3 where id='" + subtaskid + "' and status=1 and archive=0";
            
            da1.executeDMLQueryinline(strstatusupdate);
            retval = "1";
        }
        catch { }
        return retval;
    }
    

    public static string ConvertMinutesToTimer(double totalMinutes)
    {
        try
        {
            TimeSpan ts = TimeSpan.FromMinutes(totalMinutes);

            return
                ((int)ts.TotalHours).ToString("00") + ":" +
                ts.Minutes.ToString("00") + ":" +
                ts.Seconds.ToString("00") + ":"+
                ts.Milliseconds.ToString("000");
        }
        catch
        {
            return "00:00:000";
            //return "00:00:00";
        }
    }
    public static string ConvertMinutesToTimer_history(double totalMinutes)
    {
        try
        {
            TimeSpan ts = TimeSpan.FromMinutes(totalMinutes);

            return
                ((int)ts.TotalHours).ToString("00") + ":" +
                ts.Minutes.ToString("00") + ":" +
                ts.Seconds.ToString("00");
        }
        catch
        {
            return "00:00:00";
           
        }
    }
    #endregion

    public static DateTime[] DatesOfQuarter()
    {
        DataAccess da = new DataAccess();
        DateTime[] dtReturn = new DateTime[2];
        DateTime dtNow = Convert.ToDateTime(da.getcdate()); ;
        if (dtNow.Month >= 1 && dtNow.Month <= 3)
        {
            dtReturn[0] = DateTime.Parse("1/1/" + dtNow.Year.ToString());
            dtReturn[1] = DateTime.Parse("3/31/" + dtNow.Year.ToString());
        }
        else if (dtNow.Month >= 4 && dtNow.Month <= 6)
        {
            dtReturn[0] = DateTime.Parse("4/1/" + dtNow.Year.ToString());
            dtReturn[1] = DateTime.Parse("6/30/" + dtNow.Year.ToString());
        }
        else if (dtNow.Month >= 7 && dtNow.Month <= 9)
        {
            dtReturn[0] = DateTime.Parse("7/1/" + dtNow.Year.ToString());
            dtReturn[1] = DateTime.Parse("9/30/" + dtNow.Year.ToString());
        }
        else if (dtNow.Month >= 10 && dtNow.Month <= 12)
        {
            dtReturn[0] = DateTime.Parse("10/1/" + dtNow.Year.ToString());
            dtReturn[1] = DateTime.Parse("12/31/" + dtNow.Year.ToString());
        }
        return dtReturn;
    }

    public static DateTime[] DatesOfQuarterprv(
        DateTime dtprv)
    {
        DataAccess da = new DataAccess();
        DateTime[] dtReturn = new DateTime[2];
        DateTime dtNow = dtprv; ;
        if (dtNow.Month >= 1 && dtNow.Month <= 3)
        {
            dtReturn[0] = DateTime.Parse("1/1/" + dtNow.Year.ToString());
            dtReturn[1] = DateTime.Parse("3/31/" + dtNow.Year.ToString());
        }
        else if (dtNow.Month >= 4 && dtNow.Month <= 6)
        {
            dtReturn[0] = DateTime.Parse("4/1/" + dtNow.Year.ToString());
            dtReturn[1] = DateTime.Parse("6/30/" + dtNow.Year.ToString());
        }
        else if (dtNow.Month >= 7 && dtNow.Month <= 9)
        {
            dtReturn[0] = DateTime.Parse("7/1/" + dtNow.Year.ToString());
            dtReturn[1] = DateTime.Parse("9/30/" + dtNow.Year.ToString());
        }
        else if (dtNow.Month >= 10 && dtNow.Month <= 12)
        {
            dtReturn[0] = DateTime.Parse("10/1/" + dtNow.Year.ToString());
            dtReturn[1] = DateTime.Parse("12/31/" + dtNow.Year.ToString());
        }
        return dtReturn;
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

    public class WeeklyReportData
    {
        public string shiftStart { get; set; }
        public string shiftEnd { get; set; }
        public string shiftTime { get; set; }
        public string weekCapacity { get; set; }
        public string weekActualTime { get; set; }
        public int workingDays { get; set; }
        public List<WeeklyReportDay> days { get; set; }
        public List<WeeklyReportWork> work { get; set; }
    }

    public class WeeklyReportDay
    {
        public string key { get; set; }
        public bool isWeekOff { get; set; }
        public bool isHoliday { get; set; }
        public string totalTime { get; set; }
        public string actualTime { get; set; }
        public long actualSeconds { get; set; }
    }

    public class WeeklyReportWork
    {
        public string entityKey { get; set; }
        public string date { get; set; }
        public long seconds { get; set; }
    }

    public class RunningTimerInfo
    {
        public bool hasRunning { get; set; }
        public int transactionId { get; set; }
        public string taskType { get; set; }
        public string typeName { get; set; }
        public int entityId { get; set; }
        public int projectId { get; set; }
        public int milestoneId { get; set; }
        public int taskId { get; set; }
        public int subtaskId { get; set; }
        public string taskName { get; set; }
        public string serialNo { get; set; }
    }

    public class subtasklist_count
    {
        public string statusid { get; set; }
        public string statusname { get; set; }
        public string statuscolor { get; set; }
        public string statusicon { get; set; }
        public string total { get; set; }
    }
    public class viewhistory
    {
        public string id { get; set; }
        public string taskname { get; set; }
        public string duration { get; set; }
        public string spent { get; set; }
        public string remark { get; set; }
        public List<subtaskhistory>history { get; set; }
    }
    public class subtaskhistory
    {
        public DateTime? startdate { get; set; }
        public DateTime? enddate { get; set; }
        public string spenttime { get; set; }
        public string remark { get; set; }
    }
    public class subtasklist
    {
        public int? id { get; set; }
        public string tasktype { get; set; }
        public bool selected { get; set; }
        public int? ispause { get; set; }
        public int? isrun { get; set; }
        public int? isend { get; set; }
        public int? ProjectId { get; set; }        
        public string timerText { get; set; }
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
        public DateTime? Startdate { get; set; }
        public DateTime? Targetdate { get; set; }
        public double? Weightage { get; set; }
        public string StartTime { get; set; }
        public string Timeline { get; set; }
        public string TaskDuration { get; set; }
        public string TaskDuration_min { get; set; }
        public int? P_Status { get; set; }
        public string status_name { get; set; }
        public string status_color { get; set; }
        public int? Priority { get; set; }
        public string priority_name { get; set; }
        public string priority_color { get; set; }
        public double? Budget { get; set; }
        public double? Actual { get; set; }
        public string Reminder { get; set; }
        public int? ReminderType { get; set; }
        public string Days { get; set; }
        public string Time { get; set; }
        public int? uid { get; set; }
        public DateTime? cdate { get; set; }
        public string tagto { get; set; }
        public string dependancy { get; set; }
        public string iscomplete { get; set; }
        public double? iscompletepercentage { get; set; }
        public int? orderno { get; set; }
        public int MilestoneOrderNo { get; set; }
        public int TaskOrderNo { get; set; }
        public int SubTaskOrderNo { get; set; }
        public string txtdays { get; set; }

        public string complete_value { get; set; }
         public string pending_value { get; set; }

        public string leftdays { get; set; }
        public int? totalissue { get; set; }
        public int? pendingissue { get; set; }
        public int? completeissue { get; set; }
        public double? pendingissue_rate { get; set; }
        public double? completeissue_rate { get; set; }
        public int? totaldependency { get; set; }
        public List<ownerlist> owners { get; set; }
        public List<teamlist> teams { get; set; }
        public List<taglist> tags { get; set; }
        public List<projectfiles> viewfiles { get; set; }
        //public List<issuestdata> subtaskissues { get; set; }
        public double? elapsedMilliseconds { get; set; }
        public string currentTimer { get; set; }
        public string timerText_spent { get; set; }
        public int? isexceed { get; set; }
        public string spentexceedtime { get; set; }

    }
    public class projectfiles
    {
        public string id { get; set; }
        public string pid { get; set; }
        public string fentryid { get; set; }
        public string ftype { get; set; }
        public string fileurl { get; set; }
        public string filedes { get; set; }
        public string fext { get; set; }
        public string filepath { get; set; }

    }

    public class ownerlist : IPerson { public string id { get; set; } public string name { get; set; } public string profileimage { get; set; } public string shortname { get; set; } }
    public class teamlist : IPerson { public string id { get; set; } public string name { get; set; } public string profileimage { get; set; } public string shortname { get; set; } }
    public class userlist : IPerson { public string id { get; set; } public string name { get; set; } public string profileimage { get; set; } public string shortname { get; set; } }
    public interface IPerson
    {
        string id { get; set; }
        string name { get; set; }
        string profileimage { get; set; }
        string shortname { get; set; }
    }
   
    public class statuslist
    {
        public string id { get; set; }
        public string name { get; set; }
        public string colorname { get; set; }
        public string statusicon { get; set; }
        public string iscomplete { get; set; }
        public string isdefault { get; set; }
    }
    public class prioritylist
    {
        public string id { get; set; }
        public string name { get; set; }
        public string colorname { get; set; }
        public string isdefault { get; set; }
    }
    public class taglist
    {
        public string id { get; set; }
        public string tagname { get; set; }
        public string tagcolor { get; set; }
        
    }
    public class removetags
    {
        public string id { get; set; }
        public string removetype { get; set; }
        public string tagname { get; set; }

    }
}
