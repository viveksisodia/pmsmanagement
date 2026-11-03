using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CommonClass;
using System.Collections.Generic;
using System.IO;
using CustomDropDown;
using System.Web.Services;

public partial class ucontrol_MultipleDataSubtask : System.Web.UI.UserControl
{
    DataSet ds = new DataSet();
    DataAccess da = new DataAccess();
    public CommonFunction cmf = new CommonFunction();
    SqlParameter[] prm, prm1;
    public function fns = new function();
    public string Subtaskdata = "";
    public string MultiOwner = "";
    public string Multiteam = "";
    public string Multiusers = "";
    public string Multichangeowner = "";
    public string Multiaddteam = "";
    public string settimers = "";
    public string setstartf = "";
    public string MultiDependancy = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            bindProject();
            bindmilestone();
            bindtasklist();
            bindtask();
            // bindemployee();
            bindMultiOwner();
            bindMultiteam();
            bindMultiusers();
            bindprojectstatus();
            bindPriorityType();
            bindStage();
            bindMultichangeOwner();
            bindMultiAddteam();
            bindMultiDependancy();

            try
            {
                if (Request.QueryString["type"] != null)
                {
                    hdntype.Value = "1";

                    if (Request.QueryString["type"].ToString() == "Project")
                    {
                        bindProject();
                        ProjectId.SelectedValue = Request.QueryString["id"].ToString();
                        bindSubtaskdatatimer("0", "0", "0", "0");
                    }

                    if (Request.QueryString["type"].ToString() == "milstone")
                    {
                        bindProject();
                        ProjectId.SelectedValue = Request.QueryString["id"].ToString();

                        bindmilestone();
                        Milestoneid.SelectedValue = Request.QueryString["milstoneid"].ToString();

                    }

                    if (Request.QueryString["type"].ToString() == "tasklist")
                    {
                        bindProject();
                        ProjectId.SelectedValue = Request.QueryString["id"].ToString();

                        bindmilestone();
                        Milestoneid.SelectedValue = Request.QueryString["milstoneid"].ToString();

                        bindtasklist();
                        tasklist.SelectedValue = Request.QueryString["tasklistid"].ToString();

                    }

                    if (Request.QueryString["type"].ToString() == "task")
                    {
                        bindProject();
                        ProjectId.SelectedValue = Request.QueryString["id"].ToString();

                        bindmilestone();
                        Milestoneid.SelectedValue = Request.QueryString["milstoneid"].ToString();

                        //bindtasklist();
                        //tasklist.SelectedValue = Request.QueryString["tasklistid"].ToString();

                        bindtask();
                        task.SelectedValue = Request.QueryString["taskid"].ToString();

                        hdnProjectId.Value = Request.QueryString["id"].ToString();
                        hdnmilstoneid.Value = Request.QueryString["milstoneid"].ToString();
                        hdntasklistid.Value = Request.QueryString["tasklistid"].ToString();
                        hdntaskid.Value = Request.QueryString["taskid"].ToString();

                        bindSubtaskdatatimer(hdnProjectId.Value, hdnmilstoneid.Value, hdntasklistid.Value, hdntaskid.Value);

                    }


                    //if (Request.QueryString["type"].ToString() == "subtask")
                    //{
                    //    bindProject();
                    //    ProjectId.SelectedValue = Request.QueryString["id"].ToString();

                    //    bindmilestone();
                    //    Milestoneid.SelectedValue = Request.QueryString["milstoneid"].ToString();

                    //    bindtasklist();
                    //    tasklistid.SelectedValue = Request.QueryString["tasklistid"].ToString();

                    //    bindtask();
                    //    taskid.SelectedValue = Request.QueryString["taskid"].ToString();

                    //    bindSubtask();
                    //    subtaskid.SelectedValue = Request.QueryString["subtaskid"].ToString();

                    //}

                }

                else
                {
                    bindSubtaskdatatimer("0", "0", "0", "0");
                }
            }
            catch
            {

            }



        }
    }



    protected void bindSubtaskdatatimer(string RecordId = "0", string milstoneid = "0", string tasklistid = "0", string taskid = "0")
    {
        hdnbtns.Value = "";
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        SqlParameter[] prm = new SqlParameter[5];
        prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[1] = new SqlParameter("@RecordId", RecordId);
        prm[2] = new SqlParameter("@milstoneid", milstoneid);
        prm[3] = new SqlParameter("@tasklistid", tasklistid);
        prm[4] = new SqlParameter("@taskid", taskid);
        DataSet ds = da.GetDataSet("Pms_BindSubtask_dashbord", prm);

        if (ds.Tables[0].Rows.Count > 0)
        {


            //Response.Write(ds.Tables[0].Rows.Count);
            // Response.End();
            settimers = "";
            setstartf = "";
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                settimers += @"let cron_" + ds.Tables[0].Rows[i]["id"].ToString() + ";  function timer_" + ds.Tables[0].Rows[i]["id"].ToString() + "(ctrlhours,ctrlminutes,ctrlseconds,ctrlmilliseconds,millisecond1,second1,minute1,hour1) {debugger;";
                settimers += @" if ((millisecond1 += 10) == 1000) {millisecond1 = 0;second1++;}
                  if (second1 == 60) {second1 = 0;minute1++;}
                  if (minute1 == 60) {minute1 = 0;hour1++;}
                  document.getElementById(ctrlhours).innerText = returnData(hour1);
                  document.getElementById(ctrlminutes).innerText = returnData(minute1);
                  document.getElementById(ctrlseconds).innerText = returnData(second1);
                  document.getElementById(ctrlmilliseconds).innerText = returnData(millisecond1);
                }";
                string strstartdetails = "select *  from Pms_tbl_Timerstatus where subtaskid='" + ds.Tables[0].Rows[i]["id"].ToString() + "' and isnull(isend,0)=0 and isnull(IsPaused,0)=0  and status=1 and archive=0 and type='subtask' order by id desc";
                DataSet dsstart = da.GetDataSetInline(strstartdetails);
                if (dsstart.Tables[0].Rows.Count > 0)
                {
                    //Response.Write("ok");
                    //Response.End();
                    if (hdnbtns.Value == "")
                    {
                        hdnbtns.Value = (i + 1).ToString();
                    }
                    else
                    { hdnbtns.Value = "," + (i + 1).ToString(); }
                    settimers += @" function autoplay_" + ds.Tables[0].Rows[i]["id"].ToString() + "(){ document.getElementById('btnauto_" + ds.Tables[0].Rows[i]["id"].ToString() + "').click();} ";
                }
                //Response.Write(settimers);
                // Response.End();
                setstartf += @" function start_" + ds.Tables[0].Rows[i]["id"].ToString() + "(hours, minutes, seconds, milliseconds,valid) {debugger;  ";
                setstartf += @" let hour_" + ds.Tables[0].Rows[i]["id"].ToString() + " = parseInt(document.getElementById(hours.id).innerHTML);";
                setstartf += @" let minute_" + ds.Tables[0].Rows[i]["id"].ToString() + " = parseInt(document.getElementById(minutes.id).innerHTML);";
                setstartf += @" let second_" + ds.Tables[0].Rows[i]["id"].ToString() + " = parseInt(document.getElementById(seconds.id).innerHTML);";
                setstartf += @" let millisecond_" + ds.Tables[0].Rows[i]["id"].ToString() + " = parseInt(document.getElementById(milliseconds.id).innerHTML);  pause();";

                setstartf += @" cron_" + ds.Tables[0].Rows[i]["id"].ToString() + " = setInterval(() => { timer_" + ds.Tables[0].Rows[i]["id"].ToString() + "(hours.id,minutes.id,seconds.id,milliseconds.id,parseInt(document.getElementById(milliseconds.id).innerHTML),parseInt(document.getElementById(seconds.id).innerHTML),parseInt(document.getElementById(minutes.id).innerHTML),parseInt(document.getElementById(hours.id).innerHTML)); }, 10);}";
                //.RegisterStartupScript(GetType(), "Javascript", "javascript:FUNCTIONNAME(); ", true);

            }
        }


    }


    protected void bindProject()
    {
        string strleadstatus = "select id,ProjectName  from Pms_tbl_AddProject where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  order by id";
        DataSet ds = da.GetDataSetInline(strleadstatus);
        if (ds.Tables[0].Rows.Count > 0)
        {
            cmf.BindDropDownList(ProjectId, ds, "ProjectName", "id");
            ListItem li1 = new ListItem("", "0");
            ProjectId.Items.Insert(0, li1);
        }


    }

    protected void bindmilestone()
    {
        string strleadstatus = "select id,Milestone  from Pms_tbl_AddMilestone where ProjectId='" + ProjectId.SelectedValue + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  order by id";
        DataSet ds = da.GetDataSetInline(strleadstatus);
        if (ds.Tables[0].Rows.Count > 0)
        {
            cmf.BindDropDownList(Milestoneid, ds, "Milestone", "id");
            ListItem li1 = new ListItem("", "0");
            Milestoneid.Items.Insert(0, li1);
        }

    }

    protected void bindtasklist()
    {
        string strleadstatus = "select id,TaskList  from Pms_tbl_Addtasklist where ProjectId='" + ProjectId.SelectedValue + "' and Milestone='" + Milestoneid.SelectedValue + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  order by id";
        DataSet ds = da.GetDataSetInline(strleadstatus);
        if (ds.Tables[0].Rows.Count > 0)
        {
            cmf.BindDropDownList(tasklist, ds, "TaskList", "id");
            ListItem li1 = new ListItem("", "0");
            tasklist.Items.Insert(0, li1);
        }

    }

    protected void bindtask()
    {
        string strleadstatus = "select id,taskname  from Pms_tbl_Addtask where ProjectId='" + ProjectId.SelectedValue + "' and Milestoneid='" + Milestoneid.SelectedValue + "'  and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  order by id";
        DataSet ds = da.GetDataSetInline(strleadstatus);
        if (ds.Tables[0].Rows.Count > 0)
        {
            cmf.BindDropDownList(task, ds, "taskname", "id");
            ListItem li1 = new ListItem("", "0");
            task.Items.Insert(0, li1);
        }

    }


    protected void WeightageCheck(string taskid)
    {
        string strweightage = "select isnull(checkTaskWeightage ,0) as checkTaskWeightage  from Pms_tbl_Addtask where id='" + taskid + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
        DataSet ds = da.GetDataSetInline(strweightage);
        if (ds.Tables[0].Rows.Count > 0)
        {

            hdnweightage.Value = ds.Tables[0].Rows[0]["checkTaskWeightage"].ToString();

        }

    }


    protected void bindprojectstatus()
    {
        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"].ToString());
        DataSet dataset = da.GetDataSet("Pms_SP_SelectProjectstatus_reg", prm);
        cmf.BindDropDownList(P_Status, dataset, "ProjectStatus", "id");
        ListItem li = new ListItem("Select Status", "0");
        P_Status.Items.Insert(0, li);
        ListItem li1 = new ListItem("+ Add New", "-1");
        P_Status.Items.Add(li1);
    }


    protected void bindPriorityType()
    {
        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"].ToString());
        DataSet dataset = da.GetDataSet("Pms_SP_Select_Priority_Type_reg", prm);
        cmf.BindDropDownList(Priority, dataset, "name", "Id");
        ListItem li = new ListItem("Select Priority", "0");
        Priority.Items.Insert(0, li);
        ListItem li1 = new ListItem("+ Add New", "-1");
        Priority.Items.Add(li1);
    }

    protected void bindStage()
    {
        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"].ToString());
        DataSet dataset = da.GetDataSet("Pms_SP_Select_Stage_reg", prm);
        cmf.BindDropDownList(Stage, dataset, "name", "Id");
        ListItem li = new ListItem("Select Stage", "0");
        Stage.Items.Insert(0, li);
        ListItem li1 = new ListItem("+ Add New", "-1");
        Stage.Items.Add(li1);
    }


    protected void bindMultiOwner()
    {
        string dataN = "";
        string strsql = "select id, DisplayName   from tbl_employee where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by id";

        DataSet dsN = da.GetDataSetInline(strsql);

        if (dsN.Tables[0].Rows.Count > 0)
        {

            for (int i1 = 0; i1 < dsN.Tables[0].Rows.Count; i1++)
            {
                if (dataN != string.Empty)
                {
                    dataN += ",{";
                    dataN += "id: '" + dsN.Tables[0].Rows[i1]["id"].ToString() + "',";
                    dataN += "text: '" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "'";
                    dataN += "}";
                }
                else
                {
                    dataN += "{";
                    dataN += "id: '" + dsN.Tables[0].Rows[i1]["id"].ToString() + "',";
                    dataN += "text: '" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "'";
                    dataN += "}";
                }
            }

        }

        MultiOwner = dataN.ToString();



    }

    protected void bindMultichangeOwner()
    {
        string dataN = "";
        string strsql = "select id, DisplayName   from tbl_employee where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by id";

        DataSet dsN = da.GetDataSetInline(strsql);

        if (dsN.Tables[0].Rows.Count > 0)
        {

            for (int i1 = 0; i1 < dsN.Tables[0].Rows.Count; i1++)
            {
                if (dataN != string.Empty)
                {
                    dataN += ",{";
                    dataN += "id: '" + dsN.Tables[0].Rows[i1]["id"].ToString() + "',";
                    dataN += "text: '" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "'";
                    dataN += "}";
                }
                else
                {
                    dataN += "{";
                    dataN += "id: '" + dsN.Tables[0].Rows[i1]["id"].ToString() + "',";
                    dataN += "text: '" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "'";
                    dataN += "}";
                }
            }

        }

        Multichangeowner = dataN.ToString();



    }
    protected void bindMultiAddteam()
    {
        string dataN = "";
        string strsql = "select id, DisplayName   from tbl_employee where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by id";

        DataSet dsN = da.GetDataSetInline(strsql);

        if (dsN.Tables[0].Rows.Count > 0)
        {

            for (int i1 = 0; i1 < dsN.Tables[0].Rows.Count; i1++)
            {
                if (dataN != string.Empty)
                {
                    dataN += ",{";
                    dataN += "id: '" + dsN.Tables[0].Rows[i1]["id"].ToString() + "',";
                    dataN += "text: '" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "'";
                    dataN += "}";
                }
                else
                {
                    dataN += "{";
                    dataN += "id: '" + dsN.Tables[0].Rows[i1]["id"].ToString() + "',";
                    dataN += "text: '" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "'";
                    dataN += "}";
                }
            }

        }

        Multiaddteam = dataN.ToString();



    }



    protected void bindMultiDependancy()
    {
        string dataN = "";
        string strsql = "select id, Subtaskname  from Pms_tbl_Addsubtask where status=1 and archive=0 and isnull(statuscomplete,0) !=3 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by id";

        DataSet dsN = da.GetDataSetInline(strsql);

        if (dsN.Tables[0].Rows.Count > 0)
        {

            for (int i1 = 0; i1 < dsN.Tables[0].Rows.Count; i1++)
            {
                if (dataN != string.Empty)
                {
                    dataN += ",{";
                    dataN += "id: '" + dsN.Tables[0].Rows[i1]["id"].ToString() + "',";
                    dataN += "text: '" + dsN.Tables[0].Rows[i1]["Subtaskname"].ToString().Replace("'", "`") + "'";
                    dataN += "}";
                }
                else
                {
                    dataN += "{";
                    dataN += "id: '" + dsN.Tables[0].Rows[i1]["id"].ToString() + "',";
                    dataN += "text: '" + dsN.Tables[0].Rows[i1]["Subtaskname"].ToString().Replace("'", "`") + "'";
                    dataN += "}";
                }
            }

        }

        MultiDependancy = dataN.ToString();



    }


    protected void bindemployee()
    {
        string strleadstatus = "select id,DisplayName  from tbl_employee where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  order by id";
        DataSet ds = da.GetDataSetInline(strleadstatus);
        if (ds.Tables[0].Rows.Count > 0)
        {
            cmf.BindDropDownList(owner, ds, "DisplayName", "id");
            ListItem li1 = new ListItem("", "0");
            owner.Items.Insert(0, li1);
        }

    }


    protected void bindMultiteam()
    {
        string dataN = "";
        string strsql = "select id, DisplayName   from tbl_employee where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by id";

        DataSet dsN = da.GetDataSetInline(strsql);

        if (dsN.Tables[0].Rows.Count > 0)
        {

            for (int i1 = 0; i1 < dsN.Tables[0].Rows.Count; i1++)
            {
                if (dataN != string.Empty)
                {
                    dataN += ",{";
                    dataN += "id: '" + dsN.Tables[0].Rows[i1]["id"].ToString() + "',";
                    dataN += "text: '" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "'";
                    dataN += "}";
                }
                else
                {
                    dataN += "{";
                    dataN += "id: '" + dsN.Tables[0].Rows[i1]["id"].ToString() + "',";
                    dataN += "text: '" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "'";
                    dataN += "}";
                }
            }

        }

        Multiteam = dataN.ToString();



    }

    protected void bindMultiusers()
    {
        string dataN = "";
        string strsql = "select id, DisplayName   from tbl_employee where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by id";

        DataSet dsN = da.GetDataSetInline(strsql);

        if (dsN.Tables[0].Rows.Count > 0)
        {

            for (int i1 = 0; i1 < dsN.Tables[0].Rows.Count; i1++)
            {
                if (dataN != string.Empty)
                {
                    dataN += ",{";
                    dataN += "id: '" + dsN.Tables[0].Rows[i1]["id"].ToString() + "',";
                    dataN += "text: '" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "'";
                    dataN += "}";
                }
                else
                {
                    dataN += "{";
                    dataN += "id: '" + dsN.Tables[0].Rows[i1]["id"].ToString() + "',";
                    dataN += "text: '" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "'";
                    dataN += "}";
                }
            }

        }

        Multiusers = dataN.ToString();



    }



    protected void Button1_Click(object sender, EventArgs e)
    {

        try
        {
            ProjectId.SelectedValue = hdnProjectId.Value;
        }
        catch
        {

        }



        try
        {
            Milestoneid.SelectedValue = hdnMilestoneid.Value;
            bindmilestone();
        }
        catch
        {

        }

        try
        {
            tasklist.SelectedValue = hdntasklist.Value;
            bindtasklist();
        }
        catch
        {

        }

        try
        {
            task.SelectedValue = hdntask.Value;
            bindtask();
        }
        catch
        {

        }

        try
        {
            WeightageCheck(task.SelectedValue);

        }
        catch
        {

        }



        try
        {
            owner.SelectedValue = hdnowner.Value;
        }
        catch
        {

        }

        try
        {
            Startdate.Value = Convert.ToDateTime(hdnStartdate.Value).ToString("yyyy-MM-dd");
        }
        catch
        {

        }

        try
        {
            Targetdate.Value = Convert.ToDateTime(hdnTargetdate.Value).ToString("yyyy-MM-dd");
        }
        catch
        {

        }


        try
        {

            string starttime = hdnstarttime.Value.Replace("AM", "").Replace("PM", "");
            StartTime.Value = Convert.ToDateTime(starttime).TimeOfDay.ToString();
        }
        catch
        {

        }

        try
        {

            string strtimeline = hdnTimeline.Value.Replace("AM", "").Replace("PM", "");
            Timeline.Value = Convert.ToDateTime(strtimeline).TimeOfDay.ToString();
        }
        catch
        {

        }

        try
        {
            P_Status.SelectedValue = hdnP_Status.Value;
        }
        catch
        {

        }

        try
        {
            Priority.SelectedValue = hdnPriority.Value;
        }
        catch
        {

        }

        try
        {
            Multiteam = hdnteam.Value;
        }
        catch
        {

        }

        try
        {
            Reminder.SelectedValue = hdnreminder.Value;
        }
        catch
        {

        }

        try
        {
            ReminderType.SelectedValue = hdnReminderType.Value;
        }
        catch
        {

        }




        try
        {
            // Time.Value = Convert.ToDateTime(hdnTime.Value).ToString("yyyy-MM-dd");
            string strtim = hdnTime.Value.Replace("AM", "").Replace("PM", "");
            Time.Value = Convert.ToDateTime(strtim).TimeOfDay.ToString();
        }
        catch
        {

        }

        try
        {
            Multiusers = hdnNotifyusers.Value;
        }
        catch
        {

        }

        try
        {
            ChooseDate.Value = Convert.ToDateTime(hdnChooseDate.Value).ToString("yyyy-MM-dd");
        }
        catch
        {

        }


        try
        {
            Stage.SelectedValue = hdnstage.Value;
        }
        catch
        {

        }




    }



    protected void ProjectId_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindmilestone();

    }

    protected void Milestoneid_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindtasklist();
        bindtask();

    }

    protected void tasklist_SelectedIndexChanged(object sender, EventArgs e)
    {
        //  bindtask();

    }





    protected void task_SelectedIndexChanged(object sender, EventArgs e)
    {
        WeightageCheck(task.SelectedValue);
    }

    protected void Button5_Click(object sender, EventArgs e)
    {
        try
        {
            Extendatestrt_subtask.Value = Convert.ToDateTime(hdnextndstrt_subtask.Value).ToString("yyyy-MM-dd");
        }
        catch
        {

        }

        try
        {
            Extendateend_subtask.Value = Convert.ToDateTime(hdnextndend_subtask.Value).ToString("yyyy-MM-dd");
        }
        catch
        {

        }
    }
}






