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

public partial class dashboarduser : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<subtasklist_count> bindtaskdata_count(string fromdate = "", string todate = "")
    {
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
            if (userid != string.Empty && userid != "")
            {
                chkadmin = checkadmin(userid);
            }
            else
            {
                chkadmin = 0;
            }
            string procname = "pms_GetSubtaskStatusCount_user";
            SqlParameter[] prm1 = new SqlParameter[8];
            if (chkadmin == 1)
            {
                //prm1 = new SqlParameter[7];
                procname = "pms_GetSubtaskStatusCount_admin";
            }
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
            else
            {
                prm1[7] = new SqlParameter("@userid", "%");
            }
            string strprm = "";
            //SaveLogText("Astrprmuser1="+ prm1[0].Value+"," + prm1[1].Value + "," + prm1[2].Value + "," + prm1[3].Value + "," + prm1[4].Value + "," + prm1[5].Value + "," + prm1[6].Value);//+ "," + prm1[7].Value
            DataSet dsL = da.GetDataSet(procname, prm1);
            // SaveLogText("Astrprmusercount=" + dsL.Tables[0].Rows.Count);
            foreach (DataRow row in dsL.Tables[0].Rows)
            {

                _subtasklist.Add(new subtasklist_count
                {

                    statusid = row["statusid"].ToString(),
                    total = row["total"].ToString(),
                    statusname = row["statusname"].ToString(),
                    statuscolor = row["statuscolor"].ToString(),


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
    public static List<subtasklist> bindsubtasklist(string fromdate = "", string todate = "")
    {
        CommonFunction cmf = new CommonFunction();
        DataAccess da = new DataAccess();
        List<subtasklist> obj = new List<subtasklist>();
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
        //else if (ddlsearch == "9")
        //{

        //    DateTime[] dtReturn = DatesOfQuarter();


        //    DateTime strfromdateprvq = (dtReturn[0].AddDays(-1));
        //    DateTime[] dtReturnprv = DatesOfQuarterprv(strfromdateprvq);
        //    strfromdate = (dtReturnprv[0]).ToShortDateString(); ;
        //    strtodate = (dtReturnprv[1]).ToShortDateString();

        //}



        var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
        if (regCookie == null)
            return obj;
        string regid = regCookie["id"];
        int chkadmin = 0;
        string procname = "pms_GetSubtaskList_user";
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
        if (chkadmin == 1)
        {
            //prm1 = new SqlParameter[7];
            procname = "pms_GetSubtaskList_admin";
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
            else
            {
                prm1[7] = new SqlParameter("@userid", "%");
            }

            string strprm = "";
            SaveLogText("strprmuser=" + prm1[0].Value + "," + prm1[1].Value + "," + prm1[2].Value + "," + prm1[3].Value + "," + prm1[4].Value + "," + prm1[5].Value + "," + prm1[6].Value);
            DataSet dsL = da.GetDataSet(procname, prm1);

            foreach (DataRow row in dsL.Tables[0].Rows)
            {

                List<ownerlist> newownerlist = getemployeelist<ownerlist>(row["Owner"].ToString());
                List<teamlist> newteamlist = getemployeelist<teamlist>(row["team"].ToString());
                List<taglist> _tags = gettasktaglist(row["id"].ToString(), "subtask");
                List<projectfiles> newfileslist = bindfiledata(row["ProjectId"].ToString(), row["id"].ToString(), "subtask");

                double pending_rate = 100;
                double complete_rate = 0;

                string tleftdays = getleftdays_load(row["id"].ToString(), "subtask");
                #region find issues
                complete_rate = getcompletetime(row["id"].ToString());
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
                string execcedtime = compare_timespam(row["id"].ToString());
                try
                {
                    string[] ar_excced = execcedtime.Split('|');
                    isexceeds = Convert.ToInt32(ar_excced[0]);
                    spentexceedtimes = ar_excced[1];
                }
                catch { }
                #endregion
                obj.Add(new subtasklist
                {

                    id = row["id"] as int?,
                    selected = false,
                    isrun = checkactive(row["id"].ToString()),
                    ispause = checkpause(row["id"].ToString()),
                    isend = checkend(row["id"].ToString()),
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
                    teams = newteamlist,
                    timerText = gettimespam(row["id"].ToString()),
                    elapsedMilliseconds = ConvertTimerToMilliseconds(gettimespam(row["id"].ToString())),
                    complete_value = complete_rate.ToString(),
                    pending_value = pending_rate.ToString(),
                    timerText_spent = gettimespam_spent(row["id"].ToString()),
                    spentexceedtime = spentexceedtimes,
                    isexceed = isexceeds,
                    tags = _tags,


                });
            }




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
        string procname = "pms_GetSubtaskList_user";
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
            procname = "pms_GetSubtaskList_admin";
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
            DataSet dsL = da.GetDataSet(procname, prm1);

            foreach (DataRow row in dsL.Tables[0].Rows)
            {

                List<ownerlist> newownerlist = getemployeelist<ownerlist>(row["Owner"].ToString());
                List<teamlist> newteamlist = getemployeelist<teamlist>(row["team"].ToString());
                List<taglist> _tags = gettasktaglist(row["id"].ToString(), "subtask");
                List<projectfiles> newfileslist = bindfiledata(row["ProjectId"].ToString(), row["id"].ToString(), "subtask");

                double pending_rate = 100;
                double complete_rate = 0;

                string tleftdays = getleftdays_load(row["id"].ToString(), "subtask");
                #region find issues
                complete_rate = getcompletetime(row["id"].ToString());
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
                string execcedtime = compare_timespam(row["id"].ToString());
                try
                {
                    string[] ar_excced = execcedtime.Split('|');
                    isexceeds = Convert.ToInt32(ar_excced[0]);
                    spentexceedtimes = ar_excced[1];
                }
                catch { }
                #endregion
                obj.Add(new subtasklist
                {

                    id = row["id"] as int?,
                    isrun = checkactive(row["id"].ToString()),
                    ispause = checkpause(row["id"].ToString()),
                    isend = checkend(row["id"].ToString()),
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
                    teams = newteamlist,
                    timerText = gettimespam(row["id"].ToString()),
                    elapsedMilliseconds = ConvertTimerToMilliseconds(gettimespam(row["id"].ToString())),
                    complete_value = complete_rate.ToString(),
                    pending_value = pending_rate.ToString(),
                    timerText_spent = gettimespam_spent(row["id"].ToString()),
                    spentexceedtime = spentexceedtimes,
                    isexceed = isexceeds,
                    tags = _tags,


                });
            }




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
    public static viewhistory bindtaskhistory(string subtaskid)
    {
        CommonFunction cmf = new CommonFunction();
        DataAccess da = new DataAccess();
        viewhistory obj = new viewhistory();

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
            prm1[5] = new SqlParameter("@type", "subtask");
            prm1[6] = new SqlParameter("@userid", userid);
            DataSet dsL = da.GetDataSet("pms_GetSubtasktimer_ang", prm1);
            if (dsL.Tables[0].Rows.Count > 0)
            {

                string tasknames = get_taskname(subtaskid, "subtask");
                if (tasknames.Length > 30)
                    tasknames = tasknames.Substring(0, 30) + "...";
                List<subtaskhistory> _history = gettaskhistory(dsL.Tables[0].Rows[0]["id"].ToString());
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
    public static List<subtaskhistory> gettaskhistory(string id)
    {
        SaveLogText("historydetails3-" + id);
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
                prm1[2] = new SqlParameter("@type", "subtask");
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
                            spenttime = ConvertMinutesToTimer_history(Convert.ToDouble(row["PausedTime"].ToString() == "" ? "0" : row["PausedTime"].ToString())),
                            remark = row["notes"].ToString(),


                        });
                    }
                }
                // projectlist.Add(obj);
            }
            catch (Exception ex)
            {
                SaveLogText("historydetails3EX-" + ex.Message);
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
            string strsqlselect = "select * from pms_tbl_addsubtask where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + id + "'";
            DataSet dsD = da.GetDataSetInline(strsqlselect);
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
    public static string get_taskname(string id, string type)
    {
        DataAccess da = new DataAccess();
        string ret = "";
        string transactionid = "0";
        try
        {
            string strsqlselect = "";
            if (type == "subtask")
            {
                strsqlselect = "select * from pms_tbl_addsubtask where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + id + "'";
                DataSet dsD = da.GetDataSetInline(strsqlselect);
                if (dsD.Tables[0].Rows.Count > 0)
                {
                    ret = dsD.Tables[0].Rows[0]["Subtaskname"].ToString();
                }
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
    protected static int checkactive(string id)
    {
        int retval = 0;
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        string userid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();
        if (HttpContext.Current.Request.QueryString["empid"] != null)
        {
            userid = cmf.Decrypt(HttpContext.Current.Request.QueryString["empid"]).ToString();

        }
        string strsql = "select id  from Pms_tbl_Timerstatus with(nolock) where isnull(isend,0)=0 and type='subtask' and subtaskid='" + id + "' and uid='" + userid + "'   and status=1 and archive=0 order by id desc";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {

            retval = 1;

        }
        return retval;
    }
    protected static int checkpause(string id)
    {
        int retval = 0;
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        string userid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();
        if (HttpContext.Current.Request.QueryString["empid"] != null)
        {
            userid = cmf.Decrypt(HttpContext.Current.Request.QueryString["empid"]).ToString();

        }
        string strsql = "select id  from Pms_tbl_Timerstatus with(nolock) where isnull(IsPaused,0)=1 and isnull(isend,0)=0 and type='subtask' and subtaskid='" + id + "' and uid='" + userid + "' and status=1 and archive=0 order by id desc";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {

            retval = 1;

        }
        return retval;
    }
    protected static int checkend(string id)
    {
        int retval = 0;
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        string userid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();
        if (HttpContext.Current.Request.QueryString["empid"] != null)
        {
            userid = cmf.Decrypt(HttpContext.Current.Request.QueryString["empid"]).ToString();

        }
        string strsql = "select id  from Pms_tbl_Timerstatus with(nolock) where  isnull(isend,0)=1 and type='subtask' and subtaskid='" + id + "' and uid='" + userid + "' and status=1 and archive=0 order by id desc";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {

            retval = 1;

        }
        return retval;
    }
    protected static string gettimespam(string id)
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

            string strsqlselect = "select * from pms_tbl_addsubtask with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id=" + id + "";
            DataSet dsT = da.GetDataSetInline(strsqlselect);
            if (dsT.Tables[0].Rows.Count > 0)
            {
                transactionid = "0";
                string pausetime = "";
                int ispause = 0;
                int isrun = 0;
                int isend = 0;

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


                    }
                    else
                    {
                        string chkpausemin = "0";
                        string strpauseddetailsCHK = "select top(1) id,tenddate  from Pms_Tbl_PauseStatus with(nolock) where Transactionid='" + transactionid + "'  and status=1 and archive=0 and type='subtask' and isnumeric(isnull(PausedTime,0))=1 and tenddate is not null order by id desc";
                        if (transactionid == "1030")
                            SaveLogText("strpauseddetailsCHK1-" + strpauseddetailsCHK);
                        DataSet dspausedCHK = da.GetDataSetInline(strpauseddetailsCHK);
                        if (dspausedCHK.Tables[0].Rows.Count > 0)
                        {
                            string endatetime = dspausedCHK.Tables[0].Rows[0]["tenddate"].ToString();
                            if (transactionid == "1030")
                                SaveLogText("strpauseddetailsCHK4E-" + endatetime);
                            if (endatetime == string.Empty)
                            {
                                endatetime = dshourmin.Tables[0].Rows[0]["StartTime"].ToString();
                            }
                            string strpauseddetails2 = "select Transactionid, sum(cast(isnull(PausedTime,0) as numeric)) tpause  from Pms_Tbl_PauseStatus with(nolock) where Transactionid='" + transactionid + "'  and status=1 and archive=0 and type='subtask' and isnumeric(isnull(PausedTime,0))=1 group by Transactionid";
                            if (transactionid == "1030")
                                SaveLogText("strpauseddetailsCHK2-" + strpauseddetails2);
                            DataSet dspaused2 = da.GetDataSetInline(strpauseddetails2);
                            if (dspaused2.Tables[0].Rows.Count > 0)
                            {
                                chkpausemin = (dspaused2.Tables[0].Rows[0]["tpause"].ToString() == "" ? "0" : dspaused2.Tables[0].Rows[0]["tpause"].ToString());
                            }

                            TimeSpan toatlhr = (DateTime.Now - Convert.ToDateTime(endatetime));
                            TimeSpan t1 = TimeSpan.FromMinutes(Convert.ToInt32(chkpausemin));
                            if (transactionid == "1030")
                                SaveLogText("strpauseddetailsCHK4-" + toatlhr);
                            int diffdays = (toatlhr.Days) * 24;
                            toatlhr = toatlhr.Add(t1);
                            hour = (toatlhr.Hours + diffdays).ToString();
                            min = toatlhr.Minutes.ToString();
                            sec = toatlhr.Seconds.ToString();
                            // mili = toatlhr.Milliseconds.ToString();

                        }
                        else
                        {
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
        string retval = "";
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
        catch (Exception ex) { SaveLogText("strhourminutsA1EX-" + ex.Message); }
        retval = hour + ":" + min + ":" + sec + ":" + mili;
        //SaveLogText("strhourminutsA1-"+retval);
        return retval;
    }
    [WebMethod]
    public static string Getspenttime_byid(string id)
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
        string strhourminuts = "select *  from Pms_tbl_Timerstatus with(nolock) where subtaskid='" + id + "' and uid='" + userid + "'  and status=1 and archive=0 order by id desc";

        DataSet dshourmin = da.GetDataSetInline(strhourminuts);
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
    protected static string gettimespam_spent(string id)
    {
        string retval = "";
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
    protected static string compare_timespam(string id)
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
            string strsqlselect = "select * from pms_tbl_addsubtask with(nolock) " +
                                  "where status=1 and archive=0 " +
                                  "and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' " +
                                  "and id=" + id;

            DataSet dsT = da.GetDataSetInline(strsqlselect);

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
                                       "where type='subtask' " +
                                       "and subtaskid='" + id + "' " +
                                       "and taskid='" + dsT.Tables[0].Rows[0]["task"].ToString() + "' " +
                                       "and ProjectId='" + dsT.Tables[0].Rows[0]["ProjectId"].ToString() + "' " +
                                       "and Milestoneid='" + dsT.Tables[0].Rows[0]["Milestoneid"].ToString() + "' " +
                                       "and uid='" + userid + "' " +
                                       "and status=1 and archive=0 " +
                                       "order by id desc";

                DataSet dshourmin = da.GetDataSetInline(strhourminuts);

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

                string set_timeduration = "00:00:00:00";
                string cr_timeduration = "00:00:00:00";
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
    protected static double getcompletetime(string id)
    {
        double retval = 0;
        DataAccess da = new DataAccess();

        try
        {
            string strsqlselect = "select id,Task,Milestoneid,Projectid,isnull(TaskDuration,'00:00:00:00') as TaskDuration from pms_tbl_addsubtask with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id=" + id + "";
            DataSet dsT = da.GetDataSetInline(strsqlselect);
            if (dsT.Tables[0].Rows.Count > 0)
            {
                string strhr = "0";
                string strmin = "0";
                string stduration = "00:00";
                if (dsT.Tables[0].Rows[0]["TaskDuration"].ToString() != string.Empty)
                {
                    try
                    {
                        string[] arr = dsT.Tables[0].Rows[0]["TaskDuration"].ToString().Split(':');

                        strhr = arr[0] == "" ? "0" : arr[0];
                        strmin = arr[1] == "" ? "0" : arr[1];
                        stduration = strhr + ":" + strmin;


                    }
                    catch
                    {

                    }
                }
                double totalminoftask = ConvertTimerToMinutes(stduration);
                SaveLogText("totalminoftask-" + id + "=" + totalminoftask.ToString());
                double totalminoftask_consume = 0;
                string transactiontime = "00:00:00:00";
                string strhourminuts = "select id,isnull(hour,'00') as hour,isnull(minute,'00') as minute,isnull(second,'00') as second,isnull(millisecond,'00') as millisecond  from Pms_tbl_Timerstatus with(nolock) where  type='subtask' and subtaskid='" + id + "' and taskid='" + dsT.Tables[0].Rows[0]["task"].ToString() + "' and ProjectId='" + dsT.Tables[0].Rows[0]["ProjectId"].ToString() + "' and Milestoneid='" + dsT.Tables[0].Rows[0]["Milestoneid"].ToString() + "'  and status=1 and archive=0 order by id desc";

                DataSet dshourmin = da.GetDataSetInline(strhourminuts);
                if (dshourmin.Tables[0].Rows.Count > 0)
                {
                    transactiontime = dshourmin.Tables[0].Rows[0]["hour"].ToString() + ":" + dshourmin.Tables[0].Rows[0]["minute"].ToString() + ":" + dshourmin.Tables[0].Rows[0]["second"].ToString() + ":" + dshourmin.Tables[0].Rows[0]["millisecond"].ToString();
                    totalminoftask_consume = ConvertTimerToMinutes(transactiontime);
                }
                if (totalminoftask_consume > 0 && totalminoftask > 0)
                {
                    retval = Math.Round((totalminoftask_consume / totalminoftask) * 100, 2);
                }

            }
        }
        catch (Exception ex)
        {
            if (id == "2327")
                SaveLogText("getcompletetimeEX=" + id + ">" + ex.Message);
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
        string str2 = "select id, ProjectStatus as name, PrintColor as colorname,isnull(iscomplete,0) as iscomplete, isnull(isdefault,0) isdefault from Pms_tbl_Projectstatus with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";

        DataAccess danew = new DataAccess();
        DataSet drmile = danew.GetDataSetInline(str2);

        List<statuslist> list = new List<statuslist>();

        foreach (DataRow row in drmile.Tables[0].Rows)
        {
            statuslist obj = new statuslist();
            obj.id = row["id"].ToString();
            obj.name = row["name"].ToString();
            obj.colorname = (row["colorname"].ToString().Trim() == "" ? "#000" : row["colorname"].ToString());
            obj.iscomplete = row["iscomplete"].ToString();
            obj.isdefault = row["isdefault"].ToString();
            list.Add(obj);
        }

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
        string findmile = "select cast(ROUND(isnull(iscompletepercentage,0),2) AS DECIMAL(10,2)) as iscompletepercentage from " + tblname + " with(nolock)  where id='" + id + "'  and status=1  and archive=0";
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
    public static string SaveTags(string projectid = "", string milestoneid = "", string taskid = "", string subtaskid = "", string Tags = "", string TagsColor = "", string Tagstype = "")
    {
        string retval = "0";

        DataAccess da1 = new DataAccess();
        string editid = projectid;
        string entrytype = "Insert";
        SqlParameter[] prm;
        try
        {
            if (Tagstype == "subtask")
            {
                prm = new SqlParameter[13];
                prm[0] = new SqlParameter("@id", editid);
                prm[1] = new SqlParameter("@ProjectId", projectid);
                if (milestoneid != string.Empty)
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

                //retval = "1";
            }


        }
        catch
        {
            retval = "0";
        }
        return retval;



    }
    [WebMethod]
    public static string removetagbyid(string id)
    {
        string retval = "0";

        DataAccess da1 = new DataAccess();

        SqlParameter[] prm;
        try
        {
            string sql = "update Pms_tbl_Addtags set status=0 , archive=1  where id='" + id + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and tagtype='subtask'";
            da1.executeDMLQueryinline(sql);
            retval = "1";

        }
        catch
        {
            retval = "0";
        }
        return retval;



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
    public static List<taglist> gettasktaglist(string id, string tagtype)
    {
        string str2 = "select * from Pms_tbl_Addtags where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and Subtaskid='" + id + "' and tagtype='subtask'";
        if (tagtype == "task")
        {

        }
        else if (tagtype == "milestone")
        {

        }
        else if (tagtype == "project")
        {

        }
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
    #endregion

    #region save transaction details

    [WebMethod]
    public static string SaveTimerstartdetails(string ProjectId = "", string Milestoneid = "", string taskid = "", string subtaskid = "")
    {
        int retid = 0;
        DataAccess da1 = new DataAccess();
        string entrytype = "Insert";
        try
        {
            string strsqlselect = "select id,isnull(isend,0) as isend from Pms_tbl_Timerstatus with(nolock)  where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and projectid='" + ProjectId + "'  and milestoneid='" + Milestoneid + "' and taskid='" + taskid + "' and   subtaskid='" + subtaskid + "'  and uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"].ToString() + "'  and status=1 and archive=0";

            DataSet dscheck = da1.GetDataSetInline(strsqlselect);
            if (dscheck.Tables[0].Rows.Count > 0)
            {
                retid = Convert.ToInt32(dscheck.Tables[0].Rows[0]["id"].ToString());
                if (dscheck.Tables[0].Rows[0]["isend"].ToString() == "1")
                {
                    string strupdatestatus = "update Pms_tbl_Timerstatus set isend=0,IsPaused=0 where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and projectid='" + ProjectId + "'  and milestoneid='" + Milestoneid + "' and taskid='" + taskid + "' and   subtaskid='" + subtaskid + "' and status=1 and archive=0";
                    SaveLogText("strpauseddetails123456=" + strupdatestatus);
                    da1.executeDMLQueryinline(strupdatestatus);
                }
            }
            else
            {

                SqlParameter[] prm = new SqlParameter[10];
                prm[0] = new SqlParameter("@projectid", ProjectId);
                prm[1] = new SqlParameter("@milestoneid", Milestoneid);
                prm[2] = new SqlParameter("@taskid", taskid);
                prm[3] = new SqlParameter("@subtaskid", subtaskid);
                prm[4] = new SqlParameter("@StartTime", da1.getcdatetime());
                prm[5] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prm[6] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                prm[7] = new SqlParameter("@cdate", da1.getcdatetime());
                prm[8] = new SqlParameter("@type", entrytype);
                prm[9] = new SqlParameter("ReturnValue", SqlDbType.Int);
                prm[9].Direction = ParameterDirection.ReturnValue;
                retid = da1.executeDMLQuery("pmstbl_SubTask_StartTimerDetailsSave_ang", prm);
            }
            if (retid > 0)
            {
                SqlParameter[] prm1 = new SqlParameter[12];
                prm1[0] = new SqlParameter("@projectId", ProjectId);
                prm1[1] = new SqlParameter("@milestoneid", Milestoneid);
                prm1[2] = new SqlParameter("@taskid", taskid);
                prm1[3] = new SqlParameter("@subtaskid", subtaskid);
                prm1[4] = new SqlParameter("@Transactionid", retid);
                prm1[5] = new SqlParameter("@Notes", "");
                prm1[6] = new SqlParameter("@tstartdate", da1.getcdatetime());
                prm1[7] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prm1[8] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                prm1[9] = new SqlParameter("@cdate", da1.getcdatetime());
                prm1[10] = new SqlParameter("@type", entrytype);
                prm1[11] = new SqlParameter("@entrytype", "start");
                da1.executeDMLQuery("Pms_Tbl_PauseStatus_startDetailsSave_ang", prm1);

            }

            string strstatusupdate = "update Pms_tbl_Addsubtask set statuscomplete=1 where id='" + subtaskid + "' and status=1 and archive=0";
            DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);
        }
        catch (Exception ex)
        {
            SaveLogText("strsqlselect1-" + ex.Message);
        }
        return retid.ToString();


    }
    [WebMethod]
    public static string SaveTimerpausedetails(string ProjectId = "", string Milestoneid = "", string taskid = "", string subtaskid = "", string Notes = "")
    {
        string retval = "0";
        DataAccess da1 = new DataAccess();
        string entrytype = "Insert";
        string transactiondate = da1.getcdatetime();
        string transactionid = "0";

        string pausetime = "";
        string spam1 = "";
        string spam2 = "";
        try
        {
            string strhourminuts = "select *  from Pms_tbl_Timerstatus where type='subtask' and subtaskid='" + subtaskid + "' and taskid='" + taskid + "' and ProjectId='" + ProjectId + "' and Milestoneid='" + Milestoneid + "'  and uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"].ToString() + "'  and status=1 and archive=0 order by id desc";

            DataSet dshourmin = da1.GetDataSetInline(strhourminuts);
            if (dshourmin.Tables[0].Rows.Count > 0)
            {
                transactionid = dshourmin.Tables[0].Rows[0]["id"].ToString();
                transactiondate = dshourmin.Tables[0].Rows[0]["StartTime"].ToString();
            }
            SaveLogText("pausedeatils1-" + strhourminuts);
            if (transactionid != string.Empty && transactionid != "0")
            {
                string strsqllastpause = "select top(1) *  from Pms_Tbl_PauseStatus where type='subtask' and subtaskid='" + subtaskid + "' and taskid='" + taskid + "' and ProjectId='" + ProjectId + "' and Milestoneid='" + Milestoneid + "' and transactionid='" + transactionid + "' and entrytype='start' and tenddate is null and status=1 and archive=0 order by id desc";
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
                    string strresumedetails = "update Pms_Tbl_PauseStatus set notes='" + Notes + "', entrytype='pause', tenddate='" + da1.getcdatetime() + "', StartDatetime='" + da1.getcdatetime() + "' ,PausedTime='" + totalmin + "' where Transactionid='" + transactionid + "' and id='" + dsP.Tables[0].Rows[0]["id"].ToString() + "' and type='subtask' and status=1 and archive=0;update Pms_tbl_Timerstatus set IsPaused=1 where id='" + transactionid + "'";
                    SaveLogText("pausedeatils3-" + strresumedetails);
                    da1.executeDMLQueryinline(strresumedetails);
                    string sqlselectall = "select isnull(sum(isnull(cast(PausedTime as int),0)),0) as totalmin from Pms_Tbl_PauseStatus where status=1 and archive=0 and Transactionid='" + transactionid + "' and entrytype in ('pause','stop') and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
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

                    string strstatusupdate = "update Pms_tbl_Timerstatus set hour='" + cr_hour + "',minute='" + cr_min + "',second='" + cr_sec + "',millisecond='" + cr_mili + "',act_hour='" + ac_hour + "',act_minute='" + ac_min + "',act_second='" + ac_sec + "',act_millisecond='" + ac_mili + "',TimerNotes='" + Notes + "'  where id='" + transactionid + "'  and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0";
                    SaveLogText("pausedeatils5-" + strstatusupdate);
                    da1.executeDMLQueryinline(strstatusupdate);
                }
            }



            retval = "1";
        }
        catch
        {

        }
        return retval;

    }

    [WebMethod]
    public static void SaveTimerresumedetails(string ProjectId = "", string Milestoneid = "", string taskid = "", string subtaskid = "")
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
        string strhourminuts = "select *  from Pms_tbl_Timerstatus where subtaskid='" + subtaskid + "' and taskid='" + taskid + "' and ProjectId='" + ProjectId + "' and Milestoneid='" + Milestoneid + "'  and uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"].ToString() + "' and type='subtask' and status=1 and archive=0 order by id desc";
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
                string strresumedetails = "update  Pms_tbl_Timerstatus set IsPaused=0 where id='" + transactionid + "' and type='subtask'";
                SaveLogText("strpauseddetails1234=" + strresumedetails);
                try
                {
                    da.GetDataSetInline(strresumedetails);
                }
                catch (Exception ex)
                {
                    SaveLogText("strpauseddetails12345=" + ex.Message);
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
                    string strresumedetails = "update Pms_Tbl_PauseStatus set StartDatetime='" + da.getcdatetime() + "' ,PausedTime='" + totalmin + "',StartByid='" + HttpContext.Current.Request.Cookies["user"]["user_id"].ToString() + "',tenddate='" + da.getcdatetime() + "' where Transactionid='" + transactionid + "' and id='" + dspaused.Tables[0].Rows[0]["id"].ToString() + "' and type='subtask' and status=1 and archive=0;" +
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
    public static string SaveTimer(string ProjectId = "", string Milestoneid = "", string taskid = "", string subtaskid = "", string timernotes = "", string timerdetails = "")
    {
        DataAccess da1 = new DataAccess();
        string entrytype = "update";
        string transactiondate = da1.getcdatetime();
        string transactionid = "0";
        string retval = "0";
        string strtrans = "select  *  from Pms_tbl_Timerstatus where type='subtask' and subtaskid='" + subtaskid + "' and taskid='" + taskid + "' and ProjectId='" + ProjectId + "' and Milestoneid='" + Milestoneid + "' and uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"].ToString() + "' and status=1 and archive=0 order by id desc";
        DataSet dstrans = da1.GetDataSetInline(strtrans);
        if (dstrans.Tables[0].Rows.Count > 0)
        {
            transactionid = dstrans.Tables[0].Rows[0]["id"].ToString();
            transactiondate = dstrans.Tables[0].Rows[0]["StartTime"].ToString();
        }

        string strsqllastpause = "select top(1) *  from Pms_Tbl_PauseStatus where type='subtask' and subtaskid='" + subtaskid + "' and taskid='" + taskid + "' and ProjectId='" + ProjectId + "' and Milestoneid='" + Milestoneid + "' and transactionid='" + transactionid + "' and entrytype='start' and tenddate is null and status=1 and archive=0 order by id desc";
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
            string strresumedetails = "update Pms_Tbl_PauseStatus set entrytype='stop', tenddate='" + da1.getcdatetime() + "',notes='" + timernotes + "', StartDatetime='" + da1.getcdatetime() + "' ,PausedTime='" + totalmin + "' where Transactionid='" + transactionid + "' and id='" + dsP.Tables[0].Rows[0]["id"].ToString() + "' and type='subtask' and status=1 and archive=0;";

            SaveLogText("pausedeatils3-" + strresumedetails);
            da1.executeDMLQueryinline(strresumedetails);
            string sqlselectall = "select isnull(sum(isnull(cast(PausedTime as int),0)),0) as totalmin from Pms_Tbl_PauseStatus where status=1 and archive=0 and Transactionid='" + transactionid + "' and entrytype in ('pause','stop') and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
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

            string strstatusupdate = "update Pms_tbl_Timerstatus set hour='" + cr_hour + "',minute='" + cr_min + "',second='" + cr_sec + "',millisecond='" + cr_mili + "',act_hour='" + ac_hour + "',act_minute='" + ac_min + "',act_second='" + ac_sec + "',act_millisecond='" + ac_mili + "'  where id='" + transactionid + "'  and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0" +
                "update Pms_tbl_Timerstatus set IsPaused=0 and isend=1,endtime='" + da1.getcdatetime() + "',TimerNotes='" + timernotes + "' where id='" + transactionid + "'";
            SaveLogText("pausedeatils5-" + strstatusupdate);
            da1.executeDMLQueryinline(strstatusupdate);
            retval = "1";
        }
        else
        {
            string sqlselectall = "select isnull(sum(isnull(cast(PausedTime as int),0)),0) as totalmin from Pms_Tbl_PauseStatus where status=1 and archive=0 and Transactionid='" + transactionid + "' and entrytype in ('pause','stop') and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
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

            string strstatusupdate = "update Pms_tbl_Timerstatus set hour='" + cr_hour + "',minute='" + cr_min + "',second='" + cr_sec + "',millisecond='" + cr_mili + "',act_hour='" + ac_hour + "',act_minute='" + ac_min + "',act_second='" + ac_sec + "',act_millisecond='" + ac_mili + "'  where id='" + transactionid + "'  and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0;" +
                "update Pms_tbl_Timerstatus  set TimerNotes='" + timernotes + "',IsPaused=0 , isend=1,endtime='" + da1.getcdatetime() + "' where id='" + transactionid + "'";
            SaveLogText("pausedeatils5-" + strstatusupdate);
            da1.executeDMLQueryinline(strstatusupdate);
            retval = "1";
        }
        return retval;
    }
    [WebMethod]
    public static string SaveTimerold(string ProjectId = "", string Milestoneid = "", string taskid = "", string subtaskid = "", string timernotes = "", string timerdetails = "")
    {
        DataAccess da1 = new DataAccess();
        string entrytype = "update";
        string transactionid = "0";
        string retval = "0";
        string hour = "";
        string minute = "";
        string second = "";
        string millisecond = "";
        if (timerdetails != string.Empty)
        {
            string[] artim = timerdetails.Split(':');
            if (artim.Length > 0)
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
            SaveLogText("hourAV=" + hour);
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
            SaveLogText("hourAV2=" + prm[0].Value + "," + prm[1].Value + "," + prm[2].Value + "," + prm[3].Value + "," + prm[4].Value + "," + prm[5].Value + "," + prm[6].Value + "," + prm[7].Value + "," + prm[8].Value + "," + prm[9].Value + "," + prm[10].Value + "," + prm[11].Value + "," + prm[12].Value + "," + prm[13].Value + "," + prm[14].Value + "," + prm[15].Value);
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
                ts.Seconds.ToString("00") + ":" +
                ts.Milliseconds.ToString("000");
        }
        catch
        {
            return "00:00:00:000";
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

    public static DateTime[] DatesOfQuarterprv(DateTime dtprv)
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
    public class subtasklist_count
    {
        public string statusid { get; set; }
        public string statusname { get; set; }
        public string statuscolor { get; set; }
        public string total { get; set; }
    }
    public class viewhistory
    {
        public string id { get; set; }
        public string taskname { get; set; }
        public string duration { get; set; }
        public string spent { get; set; }
        public string remark { get; set; }
        public List<subtaskhistory> history { get; set; }
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
}
