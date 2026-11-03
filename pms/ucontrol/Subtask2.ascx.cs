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

public partial class ucontrol_Subtask : System.Web.UI.UserControl
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
            columnshowhide();
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
                        
                    }

                    if (Request.QueryString["type"].ToString() == "milstone")
                    {
                        bindProject();
                        ProjectId.SelectedValue = Request.QueryString["id"].ToString();

                        bindmilestone();
                        Milestoneid1.SelectedValue = Request.QueryString["milstoneid"].ToString();
                        
                    }

                    if (Request.QueryString["type"].ToString() == "tasklist")
                    {
                        bindProject();
                        ProjectId.SelectedValue = Request.QueryString["id"].ToString();

                        bindmilestone();
                        Milestoneid1.SelectedValue = Request.QueryString["milstoneid"].ToString();

                        bindtasklist();
                        tasklist.SelectedValue = Request.QueryString["tasklistid"].ToString();
                        
                    }

                    if (Request.QueryString["type"].ToString() == "task")
                    {
                        bindProject();
                        ProjectId.SelectedValue = Request.QueryString["id"].ToString();

                        bindmilestone();
                        Milestoneid1.SelectedValue = Request.QueryString["milstoneid"].ToString();

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


                    

                }

                else
                {
                    hdntype.Value = "0";
                    if (Request.QueryString["type"].ToString() == "task")
                    {
                        bindProject();
                        ProjectId.SelectedValue = Request.QueryString["id"].ToString();

                        bindmilestone();
                        Milestoneid1.SelectedValue = Request.QueryString["milstoneid"].ToString();

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
                    //bindSubtaskdatatimer("0", "0", "0", "0");
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
        SaveLogText("strleadstatus-"+ strleadstatus);
        DataSet ds = da.GetDataSetInline(strleadstatus);
        if (ds.Tables[0].Rows.Count > 0)
        {
            cmf.BindDropDownList(Milestoneid1, ds, "Milestone", "id");
            ListItem li1 = new ListItem("", "0");
            Milestoneid1.Items.Insert(0, li1);
        }

    }

    protected void bindtasklist()
    {
        string strleadstatus = "select id,TaskList  from Pms_tbl_Addtasklist where ProjectId='" + ProjectId.SelectedValue + "' and Milestone='" + Milestoneid1.SelectedValue + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  order by id";
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
        string strleadstatus = "select id,taskname  from Pms_tbl_Addtask where ProjectId='" + ProjectId.SelectedValue + "' and Milestoneid='" + Milestoneid1.SelectedValue + "'  and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  order by id";
        DataSet ds = da.GetDataSetInline(strleadstatus);
        if (ds.Tables[0].Rows.Count > 0)
        {
            cmf.BindDropDownList(task, ds, "taskname", "id");
            ListItem li1 = new ListItem("", "0");
            task.Items.Insert(0, li1);
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


    protected void btnload1_Click(object sender, EventArgs e)
    {
        bindprojectstatus();
        bindPriorityType();
        bindStage();
    }

    protected void Button12_Click(object sender, EventArgs e)
    {
        SaveLogText("callid");
        //try
        //{
            ProjectId.SelectedValue = hdnProjectId.Value;
        //}
        //catch
        // {

        // }



        //try
        //{
        bindmilestone();
        Milestoneid1.SelectedValue = hdnMilestoneid1.Value;



        //}
        //catch
        //{

        // }

        //try
        //{
        bindtasklist();
        tasklist.SelectedValue = hdntasklist.Value;
            
        //}
        //catch
        //{

        //}

        try
        {
            task.SelectedValue = hdntask.Value;
            bindtask();
        }
        catch
        {

        }

       
        

        //try
        //{
        //    owner.SelectedValue = hdnowner.Value;
        //}
        //catch
        //{

        //}

        try
        {
            Startdate1.Value = Convert.ToDateTime(hdnStartdate1.Value).ToString("yyyy-MM-dd");
        }
        catch
        {

        }

        try
        {
            Targetdate1.Value = Convert.ToDateTime(hdnTargetdate1.Value).ToString("yyyy-MM-dd");
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

        //try
        //{
        //    Multiteam = hdnteam.Value;
        //}
        //catch
        //{

        //}

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

    public void columnshowhide()
    {

        HtmlAnchor[] linka = new[] { subtask0, subtask1, subtask2, subtask3, subtask4, subtask5, subtask6, subtask7, subtask8, subtask9, subtask10, subtask11, subtask12, subtask13, subtask14, subtask15, subtask16, subtask17, subtask18, subtask19, subtask20, subtask21, subtask22, subtask23, subtask24, subtask25, subtask26 };

        HiddenField[] hdn = new[] { hdn0, hdn1, hdn2, hdn3, hdn4, hdn5, hdn6, hdn7, hdn8, hdn9, hdn10, hdn11, hdn12, hdn13, hdn14, hdn15, hdn16, hdn17, hdn18, hdn19, hdn20, hdn21, hdn22, hdn23, hdn24, hdn25, hdn26 };

        string str = " select * from Pms_tbl_showhide_column where status=1 and archive=0 and uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"] + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and type='subtask'";
        DataSet ds = da.GetDataSetInline(str);
        // Response.Write(str);
        //Response.End();
        if (ds.Tables[0].Rows.Count > 0)
        {

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {


                if (linka[Convert.ToInt32((ds.Tables[0].Rows[i]["columnnameid"].ToString() == "" ? "0" : ds.Tables[0].Rows[i]["columnnameid"].ToString()))] != null)
                {

                    if (ds.Tables[0].Rows[i]["isshow"].ToString() == "0")
                    {
                        linka[Convert.ToInt32((ds.Tables[0].Rows[i]["columnnameid"].ToString() == "" ? "0" : ds.Tables[0].Rows[i]["columnnameid"].ToString()))].Attributes.Add("class", "dropdown-item toggle-vis text-mute");
                        hdn[Convert.ToInt32((ds.Tables[0].Rows[i]["columnnameid"].ToString() == "" ? "0" : ds.Tables[0].Rows[i]["columnnameid"].ToString()))].Value = "0";
                    }

                }
            }



        }

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "txt", "hidecolumn(); hidecolumngrid();", true);



    }

}