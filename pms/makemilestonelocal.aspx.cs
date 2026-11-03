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
using System.Web.Script.Services;
public partial class makemilestone : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataAccess da = new DataAccess();
    public CommonFunction cmf = new CommonFunction();
    SqlParameter[] prm, prm1;
    public function fns = new function();
    public string Projectname = "";
    public string setmilstone = "";
    public string taskdata = "";
    public string MultiOwner = "";
    public string Multiteam = "";
    public string Multiusers = "";
    public string Multiuserssubtask = "";

    public string Multiaddteam = "";
    public string Multichangeowner = "";
    public string mileMultichangeowner = "";
    public string mileMultiaddteam = "";
    public string settimers = "";
    public string setstartf = "";
    public string strquery = "";


    public string changeownermltitsk = "";
    public string teamrmltitsk = "";
    public string Notifymultitsk = "";
    public string strshow1 = "none";
    public string strshow2 = "none";
    public int chkmilstone = 1;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            
            if (Request.QueryString["id"] != null)
            {
                hdiprojid.Value = HttpContext.Current.Request.QueryString["id"].ToString();
                if (hdiprojid.Value != null)
                {
                    bindprojectname();
                    bindtaskdataP(hdiprojid.Value);
                    strquery = "?id=" + hdiprojid.Value;
                }
                

            }
            if (Request.QueryString["type"] != null)
            {
                if(strquery!="")
                {
                    strquery += "&type=" + Request.QueryString["type"].ToString();
                }
                else
                {
                    strquery += "?type=" + Request.QueryString["type"].ToString();
                }
            }
            //id = 1 & type = Project & count = 2
            columnshowhide();


            // bindProject();
            //  bindProjectsubtask();



            bindmilestone();
            // bindtasklist();
            // bindemployee();
            bindMultiOwner();
            bindMultiteam();
            bindMultiusers();
            bindMultiuserssubtask();
            bindprojectstatus();
            bindPriorityType();
            bindStage();

            bindprojectstatussubtask();
            bindPriorityTypesubtask();
            bindStagesubtask();

            bindMultichangeOwner();
            bindMultiAddteam();

            mile_bindMultichangeOwner();
            Mile_bindMultiAddteam();

            bindProjectmile();
            bindprojectstatusmile();
            bindPriorityTypemile();

            bindmilestonesubtask();



            try
            {
                if (Request.QueryString["type"] != null)
                {
                    hdntype.Value = "1";

                    if (Request.QueryString["type"].ToString() == "Project")
                    {
                        bindProject();
                        ProjectId.SelectedValue = Request.QueryString["id"].ToString();


                        bindProjectmile();
                        ProjectIdmile.SelectedValue = Request.QueryString["id"].ToString();

                        bindProjectsubtask();
                        ProjectIdsubtask.SelectedValue = Request.QueryString["id"].ToString();

                        ProjectId.SelectedValue = Request.QueryString["id"].ToString();
                        bindmilestone();

                        ProjectIdsubtask.SelectedValue = Request.QueryString["id"].ToString();
                        bindmilestonesubtask();



                    }

                    if (Request.QueryString["type"].ToString() == "milstone")
                    {
                        bindProject();
                        ProjectId.SelectedValue = Request.QueryString["id"].ToString();

                        bindmilestone();
                        Milestoneid.SelectedValue = Request.QueryString["milstoneid"].ToString();
                        hdnmilstoneid.Value = Request.QueryString["milstoneid"].ToString();





                    }



                }
                else
                {

                }
            }
            catch
            {

            }
        }

    }

    protected void bindprojectname()
    {

        string strsql = "select id, ProjectName   from Pms_tbl_AddProject where id='" + hdiprojid.Value + "' and status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by id";

        DataSet dsN = da.GetDataSetInline(strsql);

        if (dsN.Tables[0].Rows.Count > 0)
        {
            Projectname = dsN.Tables[0].Rows[0]["ProjectName"].ToString();


        }



    }
    public void bindtaskdataP(string RecordId = "0")
    {


        function fns = new function();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();


        string strproject = "select * from Pms_tbl_AddProject where id='" + RecordId + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dsproject = da.GetDataSetInline(strproject);

        if (dsproject.Tables[0].Rows.Count > 0)
        {


            for (int P = 0; P < dsproject.Tables[0].Rows.Count; P++)
            {




                string strsmilstone = "select * from Pms_tbl_AddMilestone where ProjectId='" + dsproject.Tables[0].Rows[P]["id"].ToString() + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                DataSet ds1 = da.GetDataSetInline(strsmilstone);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    chkmilstone = 1;
                    strshow1 = "";
                    strshow2 = "none";
                    for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                    {

                        setmilstone += @" $(function() {$('#showtrM_" + ds1.Tables[0].Rows[j]["id"].ToString() + "').on('click', function() { $('.taskshow_" + ds1.Tables[0].Rows[j]["id"].ToString() + "').toggle();  showtrnew(\"" + ds1.Tables[0].Rows[j]["id"].ToString() + "\");});});";

                        setmilstone += @" $('#showtrM_" + ds1.Tables[0].Rows[j]["id"].ToString() + "').click(function() { $(this).toggleClass('shown');});";

                        #region task add
                        string strstask = "select * from Pms_tbl_Addtask where   ProjectId='" + dsproject.Tables[0].Rows[P]["id"].ToString() + "' and Milestoneid='" + ds1.Tables[0].Rows[j]["id"].ToString() + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                        DataSet ds = da.GetDataSetInline(strstask);
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {


                                setmilstone += @" $(function() {$('#showtasktr_" + ds.Tables[0].Rows[i]["id"].ToString() + "').on('click', function() {$('.subtaskshow_" + ds.Tables[0].Rows[i]["id"].ToString() + "').toggle(); showtrnewsubtask(\"" + dsproject.Tables[0].Rows[P]["id"].ToString() + "_"+ds1.Tables[0].Rows[j]["id"].ToString() + "_"+ ds.Tables[0].Rows[i]["id"].ToString() + "\"); });});";
                                setmilstone += @" $('#showtasktr_" + ds.Tables[0].Rows[i]["id"].ToString() + "').click(function() { $(this).toggleClass('shown');});";



                                //string strsubstask = "select * from Pms_tbl_Addsubtask where ProjectId='" + dsproject.Tables[0].Rows[P]["id"].ToString() + "' and Milestoneid='" + ds1.Tables[0].Rows[j]["id"].ToString() + "' and task='" + ds.Tables[0].Rows[i]["id"].ToString() + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                                //DataSet dssubtsk = da.GetDataSetInline(strsubstask);
                                //if (dssubtsk.Tables[0].Rows.Count > 0)
                                //{

                                //    for (int sb = 0; sb < dssubtsk.Tables[0].Rows.Count; sb++)
                                //    {



                                //        taskdata += "<tr class='subtaskshow_" + ds.Tables[0].Rows[i]["id"].ToString() + "' style='display:none;'>";


                                //    }

                                //}
                            }




                        }

                        #endregion

                    }


                }
                else
                {
                    chkmilstone = 0;
                    strshow2 = "";
                    strshow1 = "none";

                }

            }



        }
        //Response.Write(setmilstone);
        //Response.End();


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
        string strleadstatus = "select id,Milestone  from Pms_tbl_AddMilestone where ProjectId='" + ProjectId.SelectedValue + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  order by id";
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
        string strleadstatus = "select id,TaskList  from Pms_tbl_Addtasklist where  ProjectId='" + ProjectId.SelectedValue + "' and Milestone='" + Milestoneid.SelectedValue + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  order by id";
        DataSet ds = da.GetDataSetInline(strleadstatus);
        if (ds.Tables[0].Rows.Count > 0)
        {
            cmf.BindDropDownList(tasklist, ds, "TaskList", "id");
            ListItem li1 = new ListItem("", "0");
            tasklist.Items.Insert(0, li1);
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

    protected void mile_bindMultichangeOwner()
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

        mileMultichangeowner = dataN.ToString();



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

    protected void Mile_bindMultiAddteam()
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

        mileMultiaddteam = dataN.ToString();



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

    protected void bindMultiuserssubtask()
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

        Multiuserssubtask = dataN.ToString();



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



    protected void btnload_Click(object sender, EventArgs e)
    {
        bindprojectstatus();
        bindPriorityType();
        bindStage();
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
            bindmilestone();
            Milestoneid.SelectedValue = hdnMilestoneid.Value;

        }
        catch
        {

        }


        //try
        //{
        //    Milestoneid.SelectedValue = hdnMilestoneid.Value;
        //    bindmilestone();
        //}
        //catch
        //{

        //}




        try
        {
            tasklist.SelectedValue = hdntasklist.Value;
            bindtasklist();
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
            Startdate.Value = Convert.ToDateTime(hdnstartdate.Value).ToString("yyyy-MM-dd");
        }
        catch
        {

        }

        try
        {
            Targetdate.Value = Convert.ToDateTime(hdntargetdate.Value).ToString("yyyy-MM-dd");
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
            //Time.Value = Convert.ToDateTime(hdnTime.Value).ToString("yyyy-MM-dd");
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
    protected void Button4_Click(object sender, EventArgs e)
    {
        try
        {
            Extendatestrt_task.Value = Convert.ToDateTime(hdnextndstrt_task.Value).ToString("yyyy-MM-dd");
        }
        catch
        {

        }

        try
        {
            Extendateend_task.Value = Convert.ToDateTime(hdnextndend_task.Value).ToString("yyyy-MM-dd");
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

    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        try
        {
            mileExtendatestrt.Value = Convert.ToDateTime(milehdnextndstrt.Value).ToString("yyyy-MM-dd");
        }
        catch
        {

        }

        try
        {
            mileExtendateend.Value = Convert.ToDateTime(milehdnextndend.Value).ToString("yyyy-MM-dd");
        }
        catch
        {

        }

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


    //subtask deatils add //

    protected void bindProjectsubtask()
    {
        string strleadstatus = "select id,ProjectName  from Pms_tbl_AddProject where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  order by id";
        DataSet ds = da.GetDataSetInline(strleadstatus);
        if (ds.Tables[0].Rows.Count > 0)
        {
            cmf.BindDropDownList(ProjectIdsubtask, ds, "ProjectName", "id");
            ListItem li1 = new ListItem("", "0");
            ProjectIdsubtask.Items.Insert(0, li1);
        }


    }

    protected void bindmilestonesubtask()
    {
        string strleadstatus = "select id,Milestone  from Pms_tbl_AddMilestone where ProjectId='" + ProjectIdsubtask.SelectedValue + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  order by id";
        DataSet ds = da.GetDataSetInline(strleadstatus);
        if (ds.Tables[0].Rows.Count > 0)
        {
            cmf.BindDropDownList(Milestoneidsubtask, ds, "Milestone", "id");
            ListItem li1 = new ListItem("", "0");
            Milestoneidsubtask.Items.Insert(0, li1);
        }

    }



    protected void bindtasksubtask()
    {
        string strleadstatus = "select id,taskname  from Pms_tbl_Addtask where ProjectId='" + ProjectIdsubtask.SelectedValue + "' and Milestoneid='" + Milestoneidsubtask.SelectedValue + "'  and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  order by id";
        DataSet ds = da.GetDataSetInline(strleadstatus);
        if (ds.Tables[0].Rows.Count > 0)
        {
            cmf.BindDropDownList(task, ds, "taskname", "id");
            ListItem li1 = new ListItem("", "0");
            task.Items.Insert(0, li1);
        }

    }


    protected void bindprojectstatussubtask()
    {
        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"].ToString());
        DataSet dataset = da.GetDataSet("Pms_SP_SelectProjectstatus_reg", prm);
        cmf.BindDropDownList(P_Statussubtask, dataset, "ProjectStatus", "id");
        ListItem li = new ListItem("Select Status", "0");
        P_Statussubtask.Items.Insert(0, li);
        ListItem li1 = new ListItem("+ Add New", "-1");
        P_Statussubtask.Items.Add(li1);
    }


    protected void bindPriorityTypesubtask()
    {
        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"].ToString());
        DataSet dataset = da.GetDataSet("Pms_SP_Select_Priority_Type_reg", prm);
        cmf.BindDropDownList(Prioritysubtask, dataset, "name", "Id");
        ListItem li = new ListItem("Select Priority", "0");
        Prioritysubtask.Items.Insert(0, li);
        ListItem li1 = new ListItem("+ Add New", "-1");
        Prioritysubtask.Items.Add(li1);
    }



    protected void bindStagesubtask()
    {
        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"].ToString());
        DataSet dataset = da.GetDataSet("Pms_SP_Select_Stage_reg", prm);
        cmf.BindDropDownList(Stagesubtask, dataset, "name", "Id");
        ListItem li = new ListItem("Select Stage", "0");
        Stagesubtask.Items.Insert(0, li);
        ListItem li1 = new ListItem("+ Add New", "-1");
        Stagesubtask.Items.Add(li1);
    }


    protected void btnloadsubtask_Click(object sender, EventArgs e)
    {
        bindprojectstatussubtask();
        bindPriorityTypesubtask();
        bindStagesubtask();
    }



    protected void ProjectIdsubtask_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindmilestonesubtask();
    }

    protected void Milestoneidsubtask_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindtasksubtask();
    }

    protected void task_SelectedIndexChanged(object sender, EventArgs e)
    {

    }




    protected void Button1subtask_Click(object sender, EventArgs e)
    {
        try
        {
            ProjectIdsubtask.SelectedValue = hdnProjectIdsubtask.Value;
        }
        catch
        {

        }



        try
        {
            Milestoneidsubtask.SelectedValue = hdnMilestoneidsubtask.Value;
            bindmilestonesubtask();
        }
        catch
        {

        }



        try
        {
            task.SelectedValue = hdntask.Value;
            bindtasksubtask();
        }
        catch
        {

        }



        try
        {
            Startdatesubtask.Value = Convert.ToDateTime(hdnStartdatesubtask.Value).ToString("yyyy-MM-dd");
        }
        catch
        {

        }

        try
        {
            Targetdatesubtask.Value = Convert.ToDateTime(hdnTargetdatesubtask.Value).ToString("yyyy-MM-dd");
        }
        catch
        {

        }


        try
        {

            string starttimesubtask = hdnstarttimesubtask.Value.Replace("AM", "").Replace("PM", "");
            StartTimesubtask.Value = Convert.ToDateTime(starttimesubtask).TimeOfDay.ToString();
        }
        catch
        {

        }

        try
        {

            string strtimelinesubtask = hdnTimeline.Value.Replace("AM", "").Replace("PM", "");
            Timelinesubtask.Value = Convert.ToDateTime(strtimelinesubtask).TimeOfDay.ToString();
        }
        catch
        {

        }

        try
        {
            P_Statussubtask.SelectedValue = hdnP_Statussubtask.Value;
        }
        catch
        {

        }

        try
        {
            Prioritysubtask.SelectedValue = hdnPrioritysubtask.Value;
        }
        catch
        {

        }



        try
        {
            Remindersubtask.SelectedValue = hdnremindersubtask.Value;
        }
        catch
        {

        }

        try
        {
            ReminderTypesubtask.SelectedValue = hdnReminderTypesubtask.Value;
        }
        catch
        {

        }




        try
        {
            // Time.Value = Convert.ToDateTime(hdnTime.Value).ToString("yyyy-MM-dd");
            string strtimsubtask = hdnTime.Value.Replace("AM", "").Replace("PM", "");
            Timesubtask.Value = Convert.ToDateTime(strtimsubtask).TimeOfDay.ToString();
        }
        catch
        {

        }

        try
        {
            Multiuserssubtask = hdnNotifyuserssubtask.Value;
        }
        catch
        {

        }

        try
        {
            ChooseDatesubtask.Value = Convert.ToDateTime(hdnChooseDatesubtask.Value).ToString("yyyy-MM-dd");
        }
        catch
        {

        }


        try
        {
            Stagesubtask.SelectedValue = hdnstagesubtask.Value;
        }
        catch
        {

        }
    }



    // milsetone details add

    //subtask deatils add //

    protected void bindProjectmile()
    {
        string strleadstatus = "select id,ProjectName  from Pms_tbl_AddProject where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  order by id";
        DataSet ds = da.GetDataSetInline(strleadstatus);
        if (ds.Tables[0].Rows.Count > 0)
        {
            cmf.BindDropDownList(ProjectIdmile, ds, "ProjectName", "id");
            ListItem li1 = new ListItem("", "0");
            ProjectIdmile.Items.Insert(0, li1);
        }


    }



    protected void bindprojectstatusmile()
    {
        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"].ToString());
        DataSet dataset = da.GetDataSet("Pms_SP_SelectProjectstatus_reg", prm);
        cmf.BindDropDownList(P_Statusmile, dataset, "ProjectStatus", "id");
        ListItem li = new ListItem("Select Status", "0");
        P_Statusmile.Items.Insert(0, li);
        ListItem li1 = new ListItem("+ Add New", "-1");
        P_Statusmile.Items.Add(li1);
    }


    protected void bindPriorityTypemile()
    {
        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"].ToString());
        DataSet dataset = da.GetDataSet("Pms_SP_Select_Priority_Type_reg", prm);
        cmf.BindDropDownList(Prioritymile, dataset, "name", "Id");
        ListItem li = new ListItem("Select Priority", "0");
        Prioritymile.Items.Insert(0, li);
        ListItem li1 = new ListItem("+ Add New", "-1");
        Prioritymile.Items.Add(li1);
    }






    protected void btnloadmile_Click(object sender, EventArgs e)
    {
        bindprojectstatusmile();
        bindPriorityTypemile();
    }






    protected void Button1mile_Click(object sender, EventArgs e)
    {
        try
        {
            ProjectIdmile.SelectedValue = hdnProjectIdmile.Value;
        }
        catch
        {

        }

        try
        {
            StartDatemile.Value = Convert.ToDateTime(hdnStartDatemile.Value).ToString("yyyy-MM-dd");
        }
        catch
        {

        }

        try
        {
            EndDatemile.Value = Convert.ToDateTime(hdnEndDatemile.Value).ToString("yyyy-MM-dd");
        }
        catch
        {

        }



        try
        {
            Accessmile.SelectedValue = hdnAccessmile.Value;
        }
        catch
        {

        }


        try
        {
            ddldurationtype.SelectedValue = hdndurationtype.Value;
        }
        catch
        {

        }



        try
        {
            P_Statusmile.SelectedValue = hdnP_Statusmile.Value;
        }
        catch
        {

        }

        try
        {
            Prioritymile.SelectedValue = hdnPrioritymile.Value;
        }
        catch
        {

        }



    }


    public void columnshowhide()
    {

        HtmlAnchor[] linka = new[] { multi0, multi1, multi2, multi3, multi4, multi5, multi6, multi7, multi8, multi9, multi10, multi11, multi12, multi13 };//, multi14, multi15

        HiddenField[] hdn = new[] { hdn0, hdn1, hdn2, hdn3, hdn4, hdn5, hdn6, hdn7, hdn8, hdn9, hdn10, hdn11, hdn12, hdn13 };//, hdn14, hdn15

        string str = " select * from Pms_tbl_showhide_column where status=1 and archive=0 and uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"] + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and type='multiproject'";
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
    [WebMethod]
    public static string bindtaskdata(string RecordId = "0")
    {
        int chkadmin = 0;
        function fns = new function();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();

        string userid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();
        try
        {
            if (HttpContext.Current.Request.QueryString["empid"] != null)
            {
                userid = cmf.Decrypt(HttpContext.Current.Request.QueryString["empid"]).ToString();
            }
        }
        catch
        { }
        chkadmin = checkadmin(userid);

        string projretval = Projget_status_listbind();
        string retval = get_status_listbind();
        string mileretval = Mileget_status_listbind();
        string retstageval = get_Stage_listbind();
        string strvalprority = get_Priority_listbind();
        string milestrvalprority = Mileget_Priority_listbind();
        string projstrvalprority = Projget_Priority_listbind();

        string sbtskretval = sbtskget_status_listbind();
        string sbtskretstageval = sbtskget_Stage_listbind();
        string sbtskstrvalprority = sbtskget_Priority_listbind();


        string taskdata = "";
        

        //SqlParameter[] prm = new SqlParameter[3];
        //prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        //prm[1] = new SqlParameter("@RecordId", RecordId);
        //prm[2] = new SqlParameter("@milestoneid", milstoneid);
        //DataSet ds = da.GetDataSet("Pms_Bindtask_dashbord", prm);

        string strproject = "select * from Pms_tbl_AddProject where id='" + RecordId + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dsproject = da.GetDataSetInline(strproject);


        int projistaskchk = 0;
        string sqlchkistskproj = "select isnull(istask,0) istask from Pms_tbl_AddMilestone where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and isnull(istask,0)=1  ";
        DataSet dschkistskproj = da.GetDataSetInline(sqlchkistskproj);
        if (dschkistskproj.Tables[0].Rows.Count > 0)
        {
            projistaskchk = 1;

        }


        taskdata += "<table  id='myTable' style='width:100%;' cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm datatable'>";

        //taskdata += "<table id='tbDetails' cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm mb-0 datatable'>";

        taskdata += "<thead>";
        taskdata += "<tr>";


        taskdata += "<th style='display: none;'>S.No</th>";
        taskdata += "<th style='min-width:77px;'>Action</th>";
         taskdata += "<th style='min-width:280px;'>Milestone</th>";
        taskdata += "<th style='min-width:130px;'>Completion Status</th>";
          taskdata += "<th>Weightage</th>";
        taskdata += "<th style='min-width:180px;'>Owner</th>";
        taskdata += "<th style='min-width:100px;'>Team</th>";
        taskdata += "<th style='min-width:100px;'>Issues</th>";
        taskdata += "<th style='min-width:130px;'>Timeline</th>";
        taskdata += "<th style='min-width:130px;display:none'>Extended Timeline</th>";
        //taskdata += "<th>Start Date</th>";
        //taskdata += "<th>End Date</th>";
        taskdata += "<th style='min-width:100px;'>Status</th>";
        taskdata += "<th style='min-width:70px;'>Priority</th>";
       
        taskdata += "<th>Task (Hour)</th>";
        taskdata += "<th style='min-width:100px;'>Stage</th>";
        taskdata += "<th style='min-width:100px;'>Description</th>";
        taskdata += "</tr>";
        taskdata += "</thead>";
        taskdata += "<tbody>";
        if (dsproject.Tables[0].Rows.Count > 0)
        {


            for (int P = 0; P < dsproject.Tables[0].Rows.Count; P++)
            {

                //   Milestone count start //
                #region bind project
                float Milestonecomplite = 0;
                float Milestonepending = 0;
                float MilestoneprojectTotal = 0;
                float MilestonePhaseTotal = 0;
                float MilesstoneTaskTotal = 0;

                string str1 = "select COUNT(*) as countmilstone from Pms_tbl_AddMilestone where ProjectId='" + dsproject.Tables[0].Rows[P]["id"] + "' and status=1 and archive=0 ";
                DataSet dss1 = da.GetDataSetInline(str1);
                if (dss1.Tables[0].Rows.Count > 0)
                {

                    try
                    {
                        MilestonePhaseTotal = Convert.ToInt32(dss1.Tables[0].Rows[0]["countmilstone"]);
                    }
                    catch (Exception)
                    {
                    }
                }

                string str2 = "select * from Pms_tbl_AddMilestone where ProjectId='" + dsproject.Tables[0].Rows[P]["id"] + "' and status=1 and archive=0 ";
                DataSet dss2 = da.GetDataSetInline(str2);
                if (dss2.Tables[0].Rows.Count > 0)
                {

                    string findtask1 = "select COUNT(*) as counttask  from Pms_tbl_AddMilestone where ProjectId='" + dsproject.Tables[0].Rows[P]["id"] + "'  and P_Status in ( select id from Pms_tbl_Projectstatus where  status=1 and archive=0 and isnull(iscomplete,0)=1)  and status=1 and archive=0";
                    DataSet dsssumtask1 = da.GetDataSetInline(findtask1);

                    try
                    {
                        MilesstoneTaskTotal = Convert.ToInt32(dsssumtask1.Tables[0].Rows[0]["counttask"]);
                    }
                    catch (Exception)
                    {

                    }


                    MilestoneprojectTotal = (MilestonePhaseTotal - MilesstoneTaskTotal);


                }

                if (MilestoneprojectTotal > 0)
                {
                    Milestonecomplite = MilesstoneTaskTotal;
                    Milestonepending = Math.Abs(MilestonePhaseTotal - MilesstoneTaskTotal);
                }

                if (MilestoneprojectTotal == 0)
                {
                    Milestonecomplite = MilesstoneTaskTotal;
                    Milestonepending = Math.Abs(MilestonePhaseTotal - MilesstoneTaskTotal);
                }

                // Milestone count end //


                //   completion status start //

                decimal projectWattagecomplite = 0;
                decimal projectWattagepending = 100;
                decimal projectprojectTotal = 0;
                decimal projectPhaseTotal = 0;
                decimal projectTaskTotal = 0;


                string projectstr = "select cast(ROUND(isnull(iscompletepercentage,0),2) AS DECIMAL(10,2)) as iscompletepercentage from Pms_tbl_AddProject where id='" + dsproject.Tables[0].Rows[P]["id"] + "' and status=1 and archive=0 ";
                DataSet projectdss = da.GetDataSetInline(projectstr);


                try
                {
                    projectTaskTotal = Convert.ToDecimal(projectdss.Tables[0].Rows[0]["iscompletepercentage"].ToString());
                }
                catch (Exception)
                {
                }




                projectprojectTotal = projectTaskTotal;



                if (projectprojectTotal > 0)
                {
                    projectWattagecomplite = projectprojectTotal;
                    projectWattagepending = Math.Abs(projectWattagepending - projectWattagecomplite);
                }

                // completion status end //



                //   task count start //

                float projtskcomplite = 0;
                float projtskpending = 0;
                float projtskprojectTotal = 0;
                float projtskPhaseTotal = 0;
                float projtsk_TaskTotal = 0;

                string projstr11 = "select COUNT(*) as counttsk from Pms_tbl_Addtask where ProjectId='" + dsproject.Tables[0].Rows[P]["id"] + "' and status=1 and archive=0 ";
                DataSet projdss11 = da.GetDataSetInline(projstr11);
                if (projdss11.Tables[0].Rows.Count > 0)
                {

                    try
                    {
                        projtskPhaseTotal = Convert.ToInt32(projdss11.Tables[0].Rows[0]["counttsk"]);
                    }
                    catch (Exception)
                    {
                    }
                }

                string projstr22 = "select * from Pms_tbl_AddMilestone where ProjectId='" + dsproject.Tables[0].Rows[P]["id"] + "' and status=1 and archive=0 ";
                DataSet projdss22 = da.GetDataSetInline(projstr22);
                if (projdss22.Tables[0].Rows.Count > 0)
                {

                    string findtask11 = "select COUNT(*) as counttask  from Pms_tbl_Addtask where ProjectId='" + dsproject.Tables[0].Rows[P]["id"] + "'  and P_Status in ( select id from Pms_tbl_Projectstatus where  status=1 and archive=0 and isnull(iscomplete,0)=1)  and status=1 and archive=0";
                    DataSet dsssumtask11 = da.GetDataSetInline(findtask11);

                    try
                    {
                        projtsk_TaskTotal = Convert.ToInt32(dsssumtask11.Tables[0].Rows[0]["counttask"]);
                    }
                    catch (Exception)
                    {

                    }


                    projtskprojectTotal = (projtskPhaseTotal - projtsk_TaskTotal);


                }

                if (projtskprojectTotal > 0)
                {
                    projtskcomplite = projtsk_TaskTotal;
                    projtskpending = Math.Abs(projtskPhaseTotal - projtsk_TaskTotal);
                }

                if (projtskprojectTotal == 0)
                {
                    projtskcomplite = projtsk_TaskTotal;
                    projtskpending = Math.Abs(projtskPhaseTotal - projtsk_TaskTotal);
                }

                // task count end //




                //   Issues count start //

                float projIssuescomplite = 0;
                float projIssuespending = 0;
                float projIssuesrojectTotal = 0;
                float projIssuesPhaseTotal = 0;
                float projIssues_Total = 0;

                string projstr111 = "select COUNT(*) as countIssues from Pms_tbl_Addissues where ProjectId='" + dsproject.Tables[0].Rows[P]["id"] + "' and Issueon='Project' and status=1 and archive=0 ";
                DataSet projdss111 = da.GetDataSetInline(projstr111);
                if (projdss111.Tables[0].Rows.Count > 0)
                {

                    try
                    {
                        projIssuesPhaseTotal = Convert.ToInt32(projdss111.Tables[0].Rows[0]["countIssues"]);
                    }
                    catch (Exception)
                    {
                    }
                }


                string projfindtask111 = "select COUNT(*) as count_Issues from Pms_tbl_Addissues where ProjectId='" + dsproject.Tables[0].Rows[P]["id"] + "' and Issueon='Project' and IssueStatus in ( select id from Pms_tbl_Projectstatus where  status=1 and archive=0 and isnull(iscomplete,0)=1)  and status=1 and archive=0";
                DataSet projdsssumtask111 = da.GetDataSetInline(projfindtask111);
                if (projdsssumtask111.Tables[0].Rows.Count > 0)
                {
                    try
                    {
                        projIssues_Total = Convert.ToInt32(projdsssumtask111.Tables[0].Rows[0]["count_Issues"]);
                    }
                    catch (Exception)
                    {

                    }
                }


                projIssuesrojectTotal = (projIssuesPhaseTotal - projIssues_Total);


                if (projIssuesrojectTotal > 0)
                {
                    projIssuescomplite = projIssues_Total;
                    projIssuespending = Math.Abs(projIssuesPhaseTotal - projIssues_Total);
                }

                if (projIssuesrojectTotal == 0)
                {
                    projIssuescomplite = projIssues_Total;
                    projIssuespending = Math.Abs(projIssuesPhaseTotal - projIssues_Total);
                }

                // Issues count end //


                #endregion


                string strsmilstone = "select * from Pms_tbl_AddMilestone where ProjectId='" + dsproject.Tables[0].Rows[P]["id"].ToString() + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by isnull(orderno,0) ";
                SaveLogText("strsmilstone-" + strsmilstone);
                DataSet ds1 = da.GetDataSetInline(strsmilstone);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    //SaveLogText();
                    for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                    {
                        #region milestone
                        string milecntownr = "0";
                        string mileusersdetailsowner = "";
                        string mileownertooltip = "";
                        int isownerM = 0;
                        if (ds1.Tables[0].Rows[j]["Owner"].ToString() != "")
                        {
                            string[] milearcnt = ds1.Tables[0].Rows[j]["Owner"].ToString().Trim().Split(',');
                            try
                            {
                                if (milearcnt.Contains(HttpContext.Current.Request.Cookies["user"]["user_id"].ToString()))
                                {
                                    isownerM = 1;
                                }
                            }
                            catch
                            {
                                isownerM = 0;
                            }
                            milecntownr = milearcnt.Length.ToString();
                            DataSet miledsusers = new DataSet();
                            if (ds1.Tables[0].Rows[j]["Owner"].ToString().Trim() != "")
                            {
                                miledsusers = da.GetDataSetInline("select id,DisplayName,fileupload1,SUBSTRING (DisplayName, 1, 2) as splinameone from tbl_employee where id in(" + ds1.Tables[0].Rows[j]["Owner"].ToString().Trim() + ")");
                                if (miledsusers.Tables[0].Rows.Count > 3)
                                {
                                    milecntownr = (Convert.ToInt32(milecntownr) - 3).ToString();

                                    // owner tool tip //
                                    for (int Ml = 3; Ml < miledsusers.Tables[0].Rows.Count; Ml++)
                                    {
                                        if (miledsusers.Tables[0].Rows[Ml]["DisplayName"].ToString() != string.Empty)
                                        {
                                            if (mileownertooltip != string.Empty)
                                            {
                                                mileownertooltip += "," + miledsusers.Tables[0].Rows[Ml]["DisplayName"].ToString();
                                            }
                                            else
                                            {
                                                mileownertooltip += miledsusers.Tables[0].Rows[Ml]["DisplayName"].ToString();
                                            }
                                        }

                                    }

                                    for (int mk = 0; mk < 3; mk++)
                                    {
                                        if (miledsusers.Tables[0].Rows[mk]["fileupload1"].ToString() != "")
                                        {
                                            mileusersdetailsowner += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title=" + miledsusers.Tables[0].Rows[mk]["DisplayName"].ToString() + " data-bs-original-title='" + miledsusers.Tables[0].Rows[mk]["DisplayName"].ToString() + "'><img src = '" + miledsusers.Tables[0].Rows[mk]["fileupload1"] + "' alt='' class='rounded-circle avatar-xxs'></a>";
                                        }
                                        else
                                        {
                                            mileusersdetailsowner += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover'" +
                         " data-bs-placement='top' title='" + miledsusers.Tables[0].Rows[mk]["DisplayName"].ToString() + "' data-bs-original-title='" + miledsusers.Tables[0].Rows[mk]["DisplayName"].ToString() + "'>";
                                            mileusersdetailsowner += @"<div class='avatar-xxs'><div class='avatar-title rounded-circle bg-light text-primary'>" + miledsusers.Tables[0].Rows[mk]["splinameone"].ToString() + "</div></div>";
                                            mileusersdetailsowner += @"</a>";
                                        }
                                    }

                                }
                                else
                                {
                                    milecntownr = "";
                                    for (int mk1 = 0; mk1 < miledsusers.Tables[0].Rows.Count; mk1++)
                                    {
                                        if (miledsusers.Tables[0].Rows[mk1]["fileupload1"].ToString() != "")
                                        {
                                            //img src='/assets/images/avatar-1.jpg'
                                            mileusersdetailsowner += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='' data-bs-original-title='" + miledsusers.Tables[0].Rows[mk1]["DisplayName"].ToString() + "'><img src = '" + miledsusers.Tables[0].Rows[mk1]["fileupload1"] + "' alt='' class='rounded-circle avatar-xxs'></a>";
                                        }
                                        else
                                        {
                                            mileusersdetailsowner += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover'  data-bs-placement='top' title='" + miledsusers.Tables[0].Rows[mk1]["DisplayName"].ToString() + "' data-bs-original-title='" + miledsusers.Tables[0].Rows[mk1]["DisplayName"].ToString() + "'>";
                                            mileusersdetailsowner += @"<div class='avatar-xxs'>
                            <div class='avatar-title rounded-circle bg-light text-primary'>" + miledsusers.Tables[0].Rows[mk1]["splinameone"].ToString() + "</div></div>";
                                            mileusersdetailsowner += @"</a>";

                                        }
                                    }
                                }

                            }
                        }


                        string milecnt = "0";
                        string mileusersdetails = "";
                        string mileteamtooltip = "";
                        if (ds1.Tables[0].Rows[j]["team"].ToString() != "")
                        {
                            string[] milearcnt = ds1.Tables[0].Rows[j]["team"].ToString().Trim().Split(',');
                            milecnt = milearcnt.Length.ToString();
                            DataSet mile1dsusers = new DataSet();
                            if (ds1.Tables[0].Rows[j]["team"].ToString().Trim() != "")
                            {
                                mile1dsusers = da.GetDataSetInline("select id,DisplayName,fileupload1,SUBSTRING (DisplayName, 1, 2) as splinameone from tbl_employee where id in(" + ds1.Tables[0].Rows[j]["team"].ToString().Trim() + ")");
                                if (mile1dsusers.Tables[0].Rows.Count > 3)
                                {
                                    milecnt = (Convert.ToInt32(milecnt) - 3).ToString();

                                    // team tool tip //
                                    for (int lt = 3; lt < mile1dsusers.Tables[0].Rows.Count; lt++)
                                    {
                                        if (mile1dsusers.Tables[0].Rows[lt]["DisplayName"].ToString() != string.Empty)
                                        {
                                            if (mileteamtooltip != string.Empty)
                                            {
                                                mileteamtooltip += "," + mile1dsusers.Tables[0].Rows[lt]["DisplayName"].ToString();
                                            }
                                            else
                                            {
                                                mileteamtooltip += mile1dsusers.Tables[0].Rows[lt]["DisplayName"].ToString();
                                            }
                                        }

                                    }

                                    for (int kt = 0; kt < 3; kt++)
                                    {
                                        if (mile1dsusers.Tables[0].Rows[kt]["fileupload1"].ToString() != "")
                                        {
                                            mileusersdetails += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title=" + mile1dsusers.Tables[0].Rows[kt]["DisplayName"].ToString() + " data-bs-original-title='" + mile1dsusers.Tables[0].Rows[kt]["DisplayName"].ToString() + "'><img src = '/pms/assets/images/avatar-1.jpg' alt='' class='rounded-circle avatar-xxs'></a>";
                                        }
                                        else
                                        {
                                            mileusersdetails += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover'" +
                         " data-bs-placement='top' title='" + mile1dsusers.Tables[0].Rows[kt]["DisplayName"].ToString() + "' data-bs-original-title='" + mile1dsusers.Tables[0].Rows[kt]["DisplayName"].ToString() + "'>";
                                            mileusersdetails += @"<div class='avatar-xxs'><div class='avatar-title rounded-circle bg-light text-primary'>" + mile1dsusers.Tables[0].Rows[kt]["splinameone"].ToString() + "</div></div>";
                                            mileusersdetails += @"</a>";
                                        }
                                    }

                                }
                                else
                                {
                                    milecnt = "";
                                    for (int kt1 = 0; kt1 < mile1dsusers.Tables[0].Rows.Count; kt1++)
                                    {
                                        if (mile1dsusers.Tables[0].Rows[kt1]["fileupload1"].ToString() != "")
                                        {
                                            mileusersdetails += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='' data-bs-original-title='" + mile1dsusers.Tables[0].Rows[kt1]["DisplayName"].ToString() + "'><img src = '/pms/assets/images/avatar-1.jpg' alt='' class='rounded-circle avatar-xxs'></a>";
                                        }
                                        else
                                        {
                                            mileusersdetails += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover'  data-bs-placement='top' title='" + mile1dsusers.Tables[0].Rows[kt1]["DisplayName"].ToString() + "' data-bs-original-title='" + mile1dsusers.Tables[0].Rows[kt1]["DisplayName"].ToString() + "'>";
                                            mileusersdetails += @"<div class='avatar-xxs'>
                            <div class='avatar-title rounded-circle bg-light text-primary'>" + mile1dsusers.Tables[0].Rows[kt1]["splinameone"].ToString() + "</div></div>";
                                            mileusersdetails += @"</a>";

                                        }
                                    }
                                }

                            }
                        }



                        // Milestone  completion status start //

                        decimal mileWattagecomplite = 0;
                        decimal mileWattagepending = 100;
                        decimal mileTaskTotal = 0;
                        string findmile = "select cast(ROUND(isnull(iscompletepercentage,0),2) AS DECIMAL(10,2)) as iscompletepercentage from Pms_tbl_AddMilestone  where ProjectId='" + ds1.Tables[0].Rows[j]["ProjectId"] + "' and id='" + ds1.Tables[0].Rows[j]["id"] + "'  and status=1  and archive=0";
                        DataSet dsssummile = da.GetDataSetInline(findmile);

                        try
                        {
                            mileTaskTotal = Convert.ToDecimal(dsssummile.Tables[0].Rows[0]["iscompletepercentage"]);
                        }
                        catch (Exception)
                        {
                        }

                        if (mileTaskTotal > 0)
                        {
                            mileWattagecomplite = mileTaskTotal;
                            mileWattagepending = Math.Abs(mileWattagepending - mileWattagecomplite);
                        }



                        //// Milestone  completion status  end //


                        //   task count start //

                        float miletskcomplite = 0;
                        float miletskpending = 0;
                        float miletskprojectTotal = 0;
                        float miletskPhaseTotal = 0;
                        float miletsk_TaskTotal = 0;

                        string milestr11 = "select COUNT(*) as counttsk from Pms_tbl_Addtask where ProjectId='" + ds1.Tables[0].Rows[j]["ProjectId"] + "' and Milestoneid='" + ds1.Tables[0].Rows[j]["id"] + "' and status=1 and archive=0 ";
                        DataSet miledss11 = da.GetDataSetInline(milestr11);
                        if (miledss11.Tables[0].Rows.Count > 0)
                        {

                            try
                            {
                                miletskPhaseTotal = Convert.ToInt32(miledss11.Tables[0].Rows[0]["counttsk"]);
                            }
                            catch (Exception)
                            {
                            }
                        }

                        string str22 = "select * from Pms_tbl_AddMilestone where ProjectId='" + ds1.Tables[0].Rows[j]["ProjectId"] + "' and id='" + ds1.Tables[0].Rows[j]["id"] + "' and status=1 and archive=0 ";
                        DataSet dss22 = da.GetDataSetInline(str22);
                        if (dss22.Tables[0].Rows.Count > 0)
                        {

                            string findtask11 = "select COUNT(*) as counttask  from Pms_tbl_Addtask where ProjectId='" + ds1.Tables[0].Rows[j]["ProjectId"] + "' and  Milestoneid='" + ds1.Tables[0].Rows[j]["id"] + "' and P_Status in ( select id from Pms_tbl_Projectstatus where  status=1 and archive=0 and isnull(iscomplete,0)=1)  and status=1 and archive=0";
                            DataSet dsssumtask11 = da.GetDataSetInline(findtask11);

                            try
                            {
                                miletsk_TaskTotal = Convert.ToInt32(dsssumtask11.Tables[0].Rows[0]["counttask"]);
                            }
                            catch (Exception)
                            {

                            }


                            miletskprojectTotal = (miletskPhaseTotal - miletsk_TaskTotal);


                        }

                        if (miletskprojectTotal > 0)
                        {
                            miletskcomplite = miletsk_TaskTotal;
                            miletskpending = Math.Abs(miletskPhaseTotal - miletsk_TaskTotal);
                        }

                        if (miletskprojectTotal == 0)
                        {
                            miletskcomplite = miletsk_TaskTotal;
                            miletskpending = Math.Abs(miletskPhaseTotal - miletsk_TaskTotal);
                        }

                        // task count end //


                        //   Issues count start //

                        float mileIssuescomplite = 0;
                        float mileIssuespending = 0;
                        float mileIssuesrojectTotal = 0;
                        float mileIssuesPhaseTotal = 0;
                        float mileIssues_Total = 0;

                        string milestr111 = "select COUNT(*) as countIssues from Pms_tbl_Addissues where ProjectId='" + ds1.Tables[0].Rows[j]["ProjectId"] + "' and Milestoneid='" + ds1.Tables[0].Rows[j]["id"] + "' and Issueon='milstone' and status=1 and archive=0 ";
                        DataSet miledss111 = da.GetDataSetInline(milestr111);
                        if (miledss111.Tables[0].Rows.Count > 0)
                        {

                            try
                            {
                                mileIssuesPhaseTotal = Convert.ToInt32(miledss111.Tables[0].Rows[0]["countIssues"]);
                            }
                            catch (Exception)
                            {
                            }
                        }


                        string milefindtask111 = "select COUNT(*) as count_Issues from Pms_tbl_Addissues where ProjectId='" + ds1.Tables[0].Rows[j]["ProjectId"] + "' and Milestoneid='" + ds1.Tables[0].Rows[j]["id"] + "' and Issueon='milstone' and IssueStatus in ( select id from Pms_tbl_Projectstatus where  status=1 and archive=0 and isnull(iscomplete,0)=1)  and status=1 and archive=0";
                        DataSet miledsssumtask111 = da.GetDataSetInline(milefindtask111);
                        if (miledsssumtask111.Tables[0].Rows.Count > 0)
                        {
                            try
                            {
                                mileIssues_Total = Convert.ToInt32(miledsssumtask111.Tables[0].Rows[0]["count_Issues"]);
                            }
                            catch (Exception)
                            {

                            }
                        }


                        mileIssuesrojectTotal = (mileIssuesPhaseTotal - mileIssues_Total);


                        if (mileIssuesrojectTotal > 0)
                        {
                            mileIssuescomplite = mileIssues_Total;
                            mileIssuespending = Math.Abs(mileIssuesPhaseTotal - mileIssues_Total);
                        }

                        if (mileIssuesrojectTotal == 0)
                        {
                            mileIssuescomplite = mileIssues_Total;
                            mileIssuespending = Math.Abs(mileIssuesPhaseTotal - mileIssues_Total);
                        }

                        // Issues count end //



                        taskdata += "<tr>";

                        //sno //
                        taskdata += "<td valign='top'>";
                        taskdata += "";
                        taskdata += "</td>";

                        //Action //
                        //taskdata += " <td><a href='javascript:void(0);' Class='btn btn-info btn-sm' type='button' onclick='Binddetails_mile(" + ds1.Tables[0].Rows[j]["id"].ToString() + ")'  data-bs-toggle='offcanvas' data-bs-target='#addmile'><i class='ri-edit-line'></i></a>";
                        taskdata += " <td><a href='javascript:void(0);' Class='btn btn-info btn-sm' type='button' onclick='loadsrcedit(\"editdetails\", \"/pms/editmile.aspx?id=" + ds1.Tables[0].Rows[j]["id"].ToString()+"&pid="+ ds1.Tables[0].Rows[j]["ProjectId"] + "\",\"Edit Milestone\")'  data-bs-toggle='offcanvas' data-bs-target='#addmile'><i class='ri-edit-line'></i></a>";
                        taskdata += "&nbsp;&nbsp;<a href='javascript:void(0);' Class='btn btn-danger btn-sm' type='button' onclick='deletedatamile(\""+ dsproject.Tables[0].Rows[P]["id"].ToString() + "\",\"" + ds1.Tables[0].Rows[j]["id"].ToString() + "\",this)'><i class='ri-delete-bin-5-line'></i></a>";
                        taskdata += "</td>";


                        int checkistask = 0;

                        string milestrchtask = " select isnull(istask,0) istask,isnull(statuscomplete,0)statuscomplete from  Pms_tbl_AddMilestone where id='" + ds1.Tables[0].Rows[j]["id"].ToString() + "' ";
                        DataSet miledschktask = da.GetDataSetInline(milestrchtask);
                        if (miledschktask.Tables[0].Rows.Count > 0)
                        {

                            checkistask = Convert.ToInt32(miledschktask.Tables[0].Rows[0]["istask"].ToString());

                        }


                        string checkshowhidecolmn_mile = "";
                        string showhidecolmn_mile = "";
                        string strchkhidecolmn_mile = " select * from Pms_tbl_Addtask where Milestoneid='" + ds1.Tables[0].Rows[j]["id"].ToString() + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                        DataSet dshideshow_mile = da.GetDataSetInline(strchkhidecolmn_mile);
                        if (dshideshow_mile.Tables[0].Rows.Count > 0)
                        {

                            checkshowhidecolmn_mile = "class='details-control'";
                            showhidecolmn_mile = "id = 'showtrM_" + ds1.Tables[0].Rows[j]["id"].ToString() + "'";
                        }



                        //Miestone Name
                        //taskdata += "<td "+ showhidecolmn_mile + " "+ checkshowhidecolmn_mile + ">";
                        taskdata += "<td>";
                        string txtdt1 = "<div class='d-flex'><div class='flex-grow-1'>";
                        string txtval = get_Milstonename(ds1.Tables[0].Rows[j]["id"].ToString());
                        if (txtval.Trim() == "")
                        {
                            txtval = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                        }
                        txtdt1 += "<span  style='font-size:16px;'><b>" + (j + 1) + "&nbsp;<span id='lblm_"+ dsproject.Tables[0].Rows[P]["id"].ToString()+ '_'+ds1.Tables[0].Rows[j]["id"].ToString() + "_1' onclick='edittext(this.id)' style='background:#fde796;'> " + txtval + "</span></b></span>" +
                            "<input type='text' id='txtm_" + dsproject.Tables[0].Rows[P]["id"].ToString() + '_' + ds1.Tables[0].Rows[j]["id"].ToString() + "_1' onblur='updatedetails(this.id,\"" + ds1.Tables[0].Rows[j]["id"].ToString() + "\",\"Milestone\");' style='display:none;border:1px solid #ddd;outline:none;' value='" + get_Milstonename(ds1.Tables[0].Rows[j]["id"].ToString()) + "'>";
                         txtdt1 += "&nbsp;<a href ='javascript:void(0);' role='button' data-bs-toggle='dropdown' aria-expanded='false'><span id='btn_"+ dsproject.Tables[0].Rows[P]["id"].ToString() +"_"+ ds1.Tables[0].Rows[j]["id"].ToString() + "' class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover'>+</span>" +
                                "<ul class='dropdown-menu  dropdownmenu-secondary'>" +

                                "<li onclick='addrowUpDown(\"1\",\"" + j + "\",\"" + dsproject.Tables[0].Rows[P]["id"].ToString() + "\",\"" + ds1.Tables[0].Rows[j]["id"].ToString() + "\",\"" + ds1.Tables[0].Rows[j]["orderno"].ToString() + "\")' style='display:inline-block;'><i class='bi bi-arrow-up-square' data-bs-toggle='' data-bs-placement='top' data-bs-trigger='hover' title='Add Above'></i></li>" +
                                "<li onclick='addrowUpDown(\"2\",\"" + j + "\",\"" + dsproject.Tables[0].Rows[P]["id"].ToString() + "\",\"" + ds1.Tables[0].Rows[j]["id"].ToString() + "\",\"" + ds1.Tables[0].Rows[j]["orderno"].ToString() + "\")' style='display:inline-block;margin:0 15px;'><i class='bi-arrow-down-square' data-bs-toggle='' data-bs-placement='top' data-bs-trigger='hover' title='Add Below'></i> </li>";
                        int chktask = 0;
                        if (checkistask!= 0)
                        {
                            string strsqltsk = "select * from pms_tbl_addtask where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0 and projectid='" + dsproject.Tables[0].Rows[P]["id"].ToString() + "' and milestoneid='" + ds1.Tables[0].Rows[j]["id"].ToString() + "'";
                            DataSet dstaskchk = da.GetDataSetInline(strsqltsk);
                            if (dstaskchk.Tables[0].Rows.Count > 0)
                            {
                                chktask = 1;
                                //txtdt1 += "<li " + showhidecolmn_mile + " " + checkshowhidecolmn_mile + ">➕ add task</li>";
                            }
                            //else
                            //{

                            //    txtdt1 += "<li " + showhidecolmn_mile + " " + checkshowhidecolmn_mile + " onclick='addrowUpDowntask(\"2\",\"0\",\"" + dsproject.Tables[0].Rows[P]["id"].ToString() + "\",\"" + ds1.Tables[0].Rows[j]["id"].ToString() + "\",\"\",\"0\")'>➕ Add Task</li>";

                            //}
                            txtdt1 += "<li onclick='addrowUpDowntask(\"2\",\"0\",\"" + dsproject.Tables[0].Rows[P]["id"].ToString() + "\",\"" + ds1.Tables[0].Rows[j]["id"].ToString() + "\",\"\",\"0\")' style='display:inline-block;'><i class='ri-list-unordered label-icon align-middle me-1' data-bs-toggle='' data-bs-placement='top' data-bs-trigger='hover' title='Add Task'></i></li>";

                        }
                        txtdt1 += "</ul></a></div>";


                        SaveLogText("txtdt1-" + txtdt1);

                        //}
                        taskdata += txtdt1;
                        if (chktask == 1)
                        {
                            if (isownerM == 1 || chkadmin== 1)
                            {
                                taskdata += "<div class='gap-1 flex-shrink-0 align-self-center'><a href='javascript:void(0);' " + showhidecolmn_mile + " onclick='updatesrc(\"" + dsproject.Tables[0].Rows[P]["id"].ToString() + "\",\"" + ds1.Tables[0].Rows[j]["id"].ToString() + "\",\"\",\"m\",this.id)'><img id='imgidm_" + ds1.Tables[0].Rows[j]["id"].ToString() + "' src='/pms/assets/images/open.png' alt='open'></a><a href='javascript:void(0);' type='button' data-bs-toggle='offcanvas' data-bs-target='#canvas_popup' onclick='loadsrc(\"inner_popup\",\"/pms/AddSTDependency.aspx?id=" + ds1.Tables[0].Rows[j]["id"].ToString() + "&mprojectid=" + dsproject.Tables[0].Rows[P]["id"].ToString() + "&type=milestone&pid=" + dsproject.Tables[0].Rows[P]["id"].ToString() + "\")'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Dependency' style='font-size:12px;height:22px;width:22px;margin-left:9px'>D</span></a>&nbsp;&nbsp;&nbsp; <a href='javascript:void(0);' type='button' data-bs-toggle='offcanvas' data-bs-target='#canvas_popup' onclick='loadsrc(\"inner_popup\",\"/pms/AddIssuesPopup.aspx?id=" + dsproject.Tables[0].Rows[P]["id"].ToString() + "&milestoneid=" + ds1.Tables[0].Rows[j]["id"].ToString() + "&mprojectid=" + dsproject.Tables[0].Rows[P]["id"].ToString() + "&type=milstone&pid=" + dsproject.Tables[0].Rows[P]["id"].ToString() + "\")'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Issue' style='font-size:12px;height:22px;width:22px;'>I</span></a></div>";
                            }
                            else
                            {
                                taskdata += "<div class='gap-1 flex-shrink-0 align-self-center'><a href='javascript:void(0);' " + showhidecolmn_mile + " onclick='updatesrc(\"" + dsproject.Tables[0].Rows[P]["id"].ToString() + "\",\"" + ds1.Tables[0].Rows[j]["id"].ToString() + "\",\"\",\"m\",this.id)'><img id='imgidm_" + ds1.Tables[0].Rows[j]["id"].ToString() + "' src='/pms/assets/images/open.png' alt='open'></a></div>";

                            }

                        }
                        else
                        {
                            if (isownerM == 1 || chkadmin == 1)
                            {
                                taskdata += "<div class='gap-1 flex-shrink-0 align-self-center'><a href='javascript:void(0);' type='button' data-bs-toggle='offcanvas' data-bs-target='#canvas_popup' onclick='loadsrc(\"inner_popup\",\"/pms/AddSTDependency.aspx?id=" + ds1.Tables[0].Rows[j]["id"].ToString() + "&mprojectid=" + dsproject.Tables[0].Rows[P]["id"].ToString() + "&type=milestone&pid=" + dsproject.Tables[0].Rows[P]["id"].ToString() + "\")'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Dependency' style='font-size:12px;height:22px;width:22px;margin-left:9px'>D</span></a>&nbsp;&nbsp;&nbsp; <a href='javascript:void(0);' type='button' data-bs-toggle='offcanvas' data-bs-target='#canvas_popup' onclick='loadsrc(\"inner_popup\",\"/pms/AddIssuesPopup.aspx?id=" + dsproject.Tables[0].Rows[P]["id"].ToString() + "&milestoneid=" + ds1.Tables[0].Rows[j]["id"].ToString() + "&mprojectid=" + dsproject.Tables[0].Rows[P]["id"].ToString() + "&type=milstone&pid=" + dsproject.Tables[0].Rows[P]["id"].ToString() + "\")'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Issue' style='font-size:12px;height:22px;width:22px;'>I</span></a></div>";
                            }
                           

                        }
                        taskdata += "</div> </td> ";


                        //milestone Completion Status start 
                        taskdata += "<td>";
                        if (mileWattagecomplite == 0)
                        {

                            taskdata += "<div class='progress progress-xl cursor-pointer'>";
                            taskdata += "<div class='progress progress-xl bg-light position-relative' data-bs-toggle='tooltip' data-bs-placement='top' title='Complete(0%)' role='progressbar' style='width:5%'><span style='text-align:center;'>0</span>";
                            taskdata += "</div>";
                            taskdata += "</div>";

                        }

                        else
                        {
                            taskdata += "<div class='progress progress-xl cursor-pointer'>";
                            taskdata += "<div class='progress-bar bg-success' data-bs-toggle='tooltip' data-bs-placement='top' title='Complete (" + mileWattagecomplite + "%)' role='progressbar' style='width:" + mileWattagecomplite + "%'>" + mileWattagecomplite + "%";
                            taskdata += "</div>";
                            taskdata += "<div class='progress-bar bg-warning' data-toggle='tooltip' data-bs-placemen='top' title='Pending (" + mileWattagepending + "%)' role='progressbar' style='width:" + mileWattagepending + "%'>" + mileWattagepending + "%";
                            taskdata += "</div>";
                            taskdata += "</div>";

                        }

                        taskdata += "</td>";
                        //Completion Status end 







                        //weitage //
                        taskdata += "<td valign='top'>";
                        // taskdata += fns.ToFormattedCurrencyString(ds1.Tables[0].Rows[j]["Weightage"].ToString());
                        taskdata += "<span id='lblm_" + dsproject.Tables[0].Rows[P]["id"].ToString() + '_' + ds1.Tables[0].Rows[j]["id"].ToString() + "_3' onclick='edittext(this.id)' style='background:#fde796;'> " + fns.ToFormattedCurrencyString(ds1.Tables[0].Rows[j]["Weightage"].ToString()==""?"0": ds1.Tables[0].Rows[j]["Weightage"].ToString()) + "</span> ";
                        taskdata += "<input type='text' id='txtm_" + dsproject.Tables[0].Rows[P]["id"].ToString() + '_' + ds1.Tables[0].Rows[j]["id"].ToString() + "_3' onblur='updatedetails(this.id,\"" + ds1.Tables[0].Rows[j]["id"].ToString() + "\",\"Weightage\");' style='display:none;border:1px solid #ddd;outline:none;' value='" + fns.ToFormattedCurrencyString(ds1.Tables[0].Rows[j]["Weightage"].ToString()) + "' onkeypress='return isnumeric(event);'>";


                        taskdata += "</td>";


                        // owner //

                        taskdata += "<td>";
                        taskdata += "<div class='d-flex'>";
                        taskdata += "<div class='flex-grow-1'>";
                        taskdata += "<div class='avatar-group' id='milelblowner_" + ds1.Tables[0].Rows[j]["id"].ToString() + "'>";
                        taskdata += mileusersdetailsowner;

                        taskdata += "<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='" + mileownertooltip + "' data-bs-original-title='more'>";

                        if (milecntownr != "" && milecnt != "0")
                        {
                            taskdata += "<div class='avatar-xxs'>";
                            taskdata += "<div class='avatar-title rounded-circle'>";

                            taskdata += (milecntownr + "+");
                            taskdata += "</div>";
                            taskdata += "</div>";
                        }

                        taskdata += "</a>";
                        taskdata += "</div>";
                        taskdata += "</div>";
                        taskdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                        taskdata += "<a href ='javascript:void(0);' type='button' id='milebtnownr_" + ds1.Tables[0].Rows[j]["id"].ToString() + "' onclick='mile_Binddetails_Owner(" + ds1.Tables[0].Rows[j]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#milechangeowner'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Owner'>+</span></a>";
                        taskdata += "</div>";
                        taskdata += "</div>";
                        taskdata += "</td>";



                        //team //

                        //taskdata += "<td valign='top'>";
                        //taskdata += ""; //team
                        //taskdata += "</td>";

                        taskdata += "<td>";
                        taskdata += "<div class='d-flex'>";
                        taskdata += "<div class='flex-grow-1'>";
                        taskdata += "<div class='avatar-group' id='milelbluser_" + ds1.Tables[0].Rows[j]["id"].ToString() + "'>";
                        taskdata += mileusersdetails;

                        taskdata += "<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='" + mileteamtooltip + "' data-bs-original-title='more'>";

                        if (milecnt != "" && milecnt != "0")
                        {
                            taskdata += "<div class='avatar-xxs'>";
                            taskdata += "<div class='avatar-title rounded-circle'>";

                            taskdata += (milecnt + "+");
                            taskdata += "</div>";
                            taskdata += "</div>";
                        }

                        taskdata += "</a>";
                        taskdata += "</div>";
                        taskdata += "</div>";
                        taskdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                        taskdata += "<a href ='javascript:void(0);' type='button' id='milebtnteam_" + ds1.Tables[0].Rows[j]["id"].ToString() + "' onclick='mileBinddetails_Team(" + ds1.Tables[0].Rows[j]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#mileaddteam'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Team'>+</span></a>";
                        taskdata += "</div>";
                        taskdata += "</div>";
                        taskdata += "</td>";

                        //Issues

                        taskdata += "<td valign='top'>";

                        float milecompletetepercentissues = 0;
                        float milePendingepercentissues = 0;
                        milecompletetepercentissues = mileIssuescomplite * 100 / mileIssuesPhaseTotal;
                        milePendingepercentissues = mileIssuespending * 100 / mileIssuesPhaseTotal;

                        taskdata += "<div class='d-flex'>";
                        taskdata += "<div class='flex-grow-1 align-self-center'>";

                        taskdata += "<div class='progress progress-xl cursor-pointer'>";
                        taskdata += "<div class='progress-bar bg-success' role='progressbar' style='width: " + fns.ToFormattedCurrencyString(milecompletetepercentissues.ToString()) + "%' aria-valuemin='0' aria-valuemax='100' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + fns.ToFormattedCurrencyString(milecompletetepercentissues.ToString()) + "% [" + mileIssuescomplite + " of " + mileIssuesPhaseTotal + " Issues Completed]'>" + mileIssuescomplite + "</div>";

                        taskdata += "<div class='progress-bar bg-warning' role='progressbar' style='width: " + fns.ToFormattedCurrencyString(milePendingepercentissues.ToString()) + "%' aria-valuemin='0' aria-valuemax='100' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + fns.ToFormattedCurrencyString(milePendingepercentissues.ToString()) + "% [" + mileIssuespending + " of " + mileIssuesPhaseTotal + " Issues Not Completed]'>" + mileIssuespending + "</div>";
                        taskdata += "</div>";
                        taskdata += "</div>";

                        taskdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                        taskdata += "<a href ='javascript:void(0);' type='button' data-bs-toggle='offcanvas' data-bs-target=#canvas_popup onclick=loadsrc('inner_popup','/pms/AddIssuesPopup.aspx?id=" + ds1.Tables[0].Rows[j]["ProjectId"].ToString() + "&milstoneid=" + ds1.Tables[0].Rows[j]["id"].ToString() + "&type=milstone')><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Issue'>+</span></a>";

                        taskdata += "</div>";
                        taskdata += "</td>";
                        //Timeline

                        DateTime mileStartDate = Convert.ToDateTime(ds1.Tables[0].Rows[j]["StartDate"].ToString() == "" ? DateTime.Now.ToString() : ds1.Tables[0].Rows[j]["StartDate"].ToString());
                        DateTime mileTargetdate = Convert.ToDateTime(ds1.Tables[0].Rows[j]["EndDate"].ToString() == "" ? DateTime.Now.ToString() : ds1.Tables[0].Rows[j]["EndDate"].ToString());
                        TimeSpan mileobjTimeSpan = mileTargetdate - mileStartDate;
                        double mileDays = Convert.ToDouble(mileobjTimeSpan.TotalDays);
                        mileDays = mileDays + 1;


                        DateTime mileEnddate = Convert.ToDateTime(ds1.Tables[0].Rows[j]["EndDate"].ToString() == "" ? DateTime.Now.ToString() : ds1.Tables[0].Rows[j]["EndDate"].ToString());
                        DateTime mileCurrentdate = DateTime.Now;
                        TimeSpan mileobjtimeleftspn = mileEnddate - mileCurrentdate;
                        double mileleftDays = Convert.ToDouble(mileobjtimeleftspn.Days);
                        mileleftDays = mileleftDays + 1;

                        string mileleftdlydys = "";
                        if (mileleftDays < 0)
                        {
                            mileleftdlydys = "days delayed";
                        }
                        else
                        {
                            mileleftdlydys = "days left";
                        }


                        //start days months  
                        string milestartmonthname = "";
                        int mileststartdys = mileStartDate.Day;
                        int milestrtmonth = mileStartDate.Month;
                        int milestonestartyear = mileStartDate.Year;
                        
                        milestartmonthname = getmonthname(milestrtmonth);


                        //end days months 
                        string mileendmonthname = "";
                        int mileenddys = mileTargetdate.Day;
                        int mileendmonth = mileTargetdate.Month;
                        int milestoneEndyear = mileTargetdate.Year;
                       
                        mileendmonthname = getmonthname(mileendmonth);


                        taskdata += "<td>";
                        taskdata += "<a href = 'javascript:void(0);' class='timesheet-wrap' onclick='addeventdateselect(\""+ dsproject.Tables[0].Rows[P]["id"].ToString() + "\",\"" + ds1.Tables[0].Rows[j]["id"].ToString() + "\",\"\",\"\",\"m\",this);'>";
                        taskdata += "<div id='title1_" + dsproject.Tables[0].Rows[P]["id"].ToString() + "_" + ds1.Tables[0].Rows[j]["id"].ToString() + "' class='progress progress-xl bg-dark position-relative' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + mileleftDays.ToString().Replace("-", "") + " " + mileleftdlydys + "'>";
                        //Milestonedata += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 70%;' aria-valuemax='100'></div>";
                        taskdata += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 100%;' aria-valuemax='100'></div>";
                        taskdata += "<div class='dayswrapper'>";
                        taskdata += "<span class='fromtodate' id='spn1_" + dsproject.Tables[0].Rows[P]["id"].ToString() + "_" + ds1.Tables[0].Rows[j]["id"].ToString() + "'>" + mileststartdys + " " + milestartmonthname + " " + milestonestartyear + " - " + mileenddys + " " + mileendmonthname + " " + milestoneEndyear + "</span>";
                        taskdata += "<span class='totaldays'  id='spn2_" + dsproject.Tables[0].Rows[P]["id"].ToString() + "_" + ds1.Tables[0].Rows[j]["id"].ToString() + "'>" + mileDays + " days</span>";
                        taskdata += "</div>";
                        taskdata += "</div>";
                        taskdata += "</a>";
                        taskdata += "</td>";


                        // extended timeleine 

                        string mileestendleftdlydys = "";
                        string mileestendstartmonthname = "";
                        string mileestendendmonthname = "";
                        double mileestendleftDays = 0;
                        int mileestendststartdys = 0;
                        int mileestendenddys = 0;
                        double mileestendDays = 0;
                        string mileextenddatebind = "";
                        string milestrextend = "select id,extendStartdate as StartDate,extendEnddate as TargetDate from Pms_Tbl_Extended_Timeline where  status=1 and Type='Milestone' and archive=0  and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and MilestoneID = '" + ds1.Tables[0].Rows[j]["id"].ToString() + "'  ";
                        DataSet miledsstrextend = da.GetDataSetInline(milestrextend);
                        if (miledsstrextend.Tables[0].Rows.Count > 0)
                        {
                            for (int k = 0; k < miledsstrextend.Tables[0].Rows.Count; k++)
                            {
                                DateTime mileestendStartDate = Convert.ToDateTime(miledsstrextend.Tables[0].Rows[k]["StartDate"].ToString() == "" ? DateTime.Now.ToString() : miledsstrextend.Tables[0].Rows[k]["StartDate"].ToString());
                                DateTime mileestendTargetdate = Convert.ToDateTime(miledsstrextend.Tables[0].Rows[k]["TargetDate"].ToString() == "" ? DateTime.Now.ToString() : miledsstrextend.Tables[0].Rows[k]["TargetDate"].ToString());
                                TimeSpan mileestendobjTimeSpan = mileestendTargetdate - mileestendStartDate;
                                mileestendDays = Convert.ToDouble(mileestendobjTimeSpan.TotalDays);
                                mileestendDays = mileestendDays + 1;


                                DateTime mileestendEnddate = Convert.ToDateTime(miledsstrextend.Tables[0].Rows[k]["Targetdate"].ToString() == "" ? DateTime.Now.ToString() : miledsstrextend.Tables[0].Rows[k]["Targetdate"].ToString());
                                DateTime mileestendCurrentdate = DateTime.Now;
                                TimeSpan mileestendobjtimeleftspn = mileestendEnddate - mileestendCurrentdate;
                                mileestendleftDays = Convert.ToDouble(mileestendobjtimeleftspn.Days);
                                mileestendleftDays = mileestendleftDays + 1;


                                if (mileestendleftDays < 0)
                                {
                                    mileestendleftdlydys = "days delayed";
                                }
                                else
                                {
                                    mileestendleftdlydys = "days left";
                                }

                                //start days months  

                                mileestendststartdys = mileestendStartDate.Day;
                                int mileestendstrtmonth = mileestendStartDate.Month;
                                
                                mileestendstartmonthname = getmonthname(mileestendstrtmonth);
                                //end days months 

                                mileestendenddys = mileestendTargetdate.Day;
                                int mileestendendmonth = mileestendTargetdate.Month;
                               
                                mileestendendmonthname = getmonthname(mileestendendmonth);


                                mileextenddatebind += "<a href ='javascript:void(0);' class='timesheet-wrap'>";
                                mileextenddatebind += "<div class='progress progress-xl bg-dark position-relative me-1' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + mileestendststartdys + " " + mileestendstartmonthname + " - " + mileestendenddys + " " + mileestendendmonthname + " (" + mileestendleftDays.ToString().Replace("-", "") + " " + mileestendleftdlydys + ")'>";
                                mileextenddatebind += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 100%;' aria-valuemax='100'>" + mileestendDays + " days</div>";
                                mileextenddatebind += "</div>";
                                mileextenddatebind += "</a>";

                            }

                        }





                        taskdata += "<td style='display:none;'>";
                        taskdata += "<div class='d-flex'>";
                        taskdata += "<div class='flex-grow-1 align-self-center me-2'>";
                        taskdata += "<div class='extended_time' id='milelblExtnddt_" + ds1.Tables[0].Rows[j]["id"].ToString() + "'>";

                        taskdata += "" + mileextenddatebind + "";

                        taskdata += "</div>";
                        taskdata += "</div>";

                        if (mileleftDays < 0)
                        {
                            taskdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                            taskdata += "<a href ='javascript:void(0);' type='button' id='btnextndt_" + ds1.Tables[0].Rows[j]["id"].ToString() + "'  onclick='MileBindextenddate_Details(" + ds1.Tables[0].Rows[j]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#MileExtendedTimline'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Extended Day'>+</span></a>";
                            taskdata += "</div>";
                        }

                        taskdata += "</div>";
                        taskdata += "</div>";
                        taskdata += "</td>";


                        // start date
                        //taskdata += "<td valign='top'>";
                        //taskdata += cmf.dateConvert(ds1.Tables[0].Rows[j]["StartDate"].ToString());
                        //taskdata += "</td>";

                        //End date
                        //taskdata += "<td valign='top'>";
                        //taskdata += cmf.dateConvert(ds1.Tables[0].Rows[j]["EndDate"].ToString());
                        //taskdata += "</td>";

                        //Status//

                        string milestrvalcolor = get_Status_Color(ds1.Tables[0].Rows[j]["P_Status"].ToString());
                        taskdata += "<td>";
                        taskdata += "<div class='btn-group d-block'>";
                        taskdata += "<a href ='javascript:void(0);' id='milebtnsts_" + ds1.Tables[0].Rows[j]["id"].ToString() + "' type='button' onclick=setmilsestoneid('" + ds1.Tables[0].Rows[j]["id"].ToString() + "',this.id) data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + milestrvalcolor + "'>" + get_Projectstatus(ds1.Tables[0].Rows[j]["P_Status"].ToString()) + "</a>";
                        taskdata += "<div class='dropdown-menu lead_form_body p-0'>";
                        taskdata += mileretval;
                        taskdata += "</div>";
                        taskdata += "</div>";
                        taskdata += "</td>";

                        //stpriority

                        string milestrprioritycolor = get_PriorityColor(ds1.Tables[0].Rows[j]["Priority"].ToString());
                        taskdata += "<td>";
                        taskdata += "<div class='btn-group d-block'>";
                        taskdata += "<a href ='javascript:void(0);' id='milebtnpri_" + ds1.Tables[0].Rows[j]["id"].ToString() + "' type='button' onclick=milesetpriorityid('" + ds1.Tables[0].Rows[j]["id"].ToString() + "',this.id) data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + milestrprioritycolor + "'>" + get_PriorityName(ds1.Tables[0].Rows[j]["Priority"].ToString()) + "</a>";
                        taskdata += "<div class='dropdown-menu lead_form_body p-0'>";
                        taskdata += milestrvalprority;
                        taskdata += "</div>";
                        taskdata += "</div>";
                        taskdata += "</td>";


                       

                        //Task Duration //
                        taskdata += "<td valign='top'>";
                        //taskdata += "";

                         string tskduration = ds1.Tables[0].Rows[j]["TaskDuration"].ToString();
                        if(tskduration=="")
                        {
                            tskduration = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                        }
                        //string tskduration2 = (ds1.Tables[0].Rows[j]["TaskDuration"].ToString() == "" ? "0" : ds1.Tables[0].Rows[j]["TaskDuration"].ToString());
                        taskdata += "<span id='lblm_" + dsproject.Tables[0].Rows[P]["id"].ToString() + '_' + ds1.Tables[0].Rows[j]["id"].ToString() + "_13' onclick='edittext(this.id)' style='background:#fde796;'> " + tskduration + "</span>";
                       
                        taskdata += "<input type='text' id='txtm_" + dsproject.Tables[0].Rows[P]["id"].ToString() + '_' + ds1.Tables[0].Rows[j]["id"].ToString() + "_13' onblur='updatedetails(this.id,\"" + ds1.Tables[0].Rows[j]["id"].ToString() + "\",\"TaskDuration\");' style='display:none;border:1px solid #ddd;outline:none;' value='" + ds1.Tables[0].Rows[j]["TaskDuration"].ToString() + "' onkeypress='return isnumeric(event);'>";

                        taskdata += "</td>";
                        
                        //Stage //
                        taskdata += "<td valign='top'>";
                        taskdata += "";
                        taskdata += "</td>";
                        //Mile Description //
                        taskdata += "<td valign='top'>";
                        string strdescriptionM = ds1.Tables[0].Rows[j]["miledescription"].ToString();
                        if (strdescriptionM == "")
                        {
                            strdescriptionM = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                        }
                        taskdata += "<span id='lblm_" + dsproject.Tables[0].Rows[P]["id"].ToString() + '_' + ds1.Tables[0].Rows[j]["id"].ToString() + "_15' onclick='edittext(this.id)' style='background:#fde796;'> " + strdescriptionM + "</span>";

                        taskdata += "<textarea" +
                            " type='text' id='txtm_" + dsproject.Tables[0].Rows[P]["id"].ToString() + '_' + ds1.Tables[0].Rows[j]["id"].ToString() + "_15' onblur='updatedetails(this.id,\"" + ds1.Tables[0].Rows[j]["id"].ToString() + "\",\"miledescription\");' style='display:none;border:1px solid #ddd;outline:none;' value='" + ds1.Tables[0].Rows[j]["miledescription"].ToString() + "' >";

                        taskdata += "</td>";

                        //Subtask //



                        taskdata += "</tr>";
                        #endregion
                        #region task add
                        string strstask = "select * from Pms_tbl_Addtask where   ProjectId='" + dsproject.Tables[0].Rows[P]["id"].ToString() + "' and Milestoneid='" + ds1.Tables[0].Rows[j]["id"].ToString() + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by isnull(orderno,0)";
                        DataSet ds = da.GetDataSetInline(strstask);
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {

                                string cntownr = "0";
                                string usersdetailsowner = "";
                                string ownertooltip = "";
                                int isownerT = 0;
                                if (ds.Tables[0].Rows[i]["owner"].ToString() != "")
                                {
                                    string[] arcnt = ds.Tables[0].Rows[i]["owner"].ToString().Trim().Split(',');
                                    cntownr = arcnt.Length.ToString();
                                    try
                                    {
                                        if (arcnt.Contains(HttpContext.Current.Request.Cookies["user"]["user_id"].ToString()))
                                        {
                                            isownerT = 1;
                                        }
                                    }
                                    catch
                                    {
                                        isownerT = 0;
                                    }
                                    DataSet dsusers = new DataSet();
                                    if (ds.Tables[0].Rows[i]["owner"].ToString().Trim() != "")
                                    {
                                        dsusers = da.GetDataSetInline("select id,DisplayName,fileupload1,SUBSTRING (DisplayName, 1, 2) as splinameone from tbl_employee where id in(" + ds.Tables[0].Rows[i]["owner"].ToString().Trim() + ")");
                                        if (dsusers.Tables[0].Rows.Count > 3)
                                        {
                                            cntownr = (Convert.ToInt32(cntownr) - 3).ToString();


                                            // owner tool tip //
                                            for (int l = 3; l < dsusers.Tables[0].Rows.Count; l++)
                                            {
                                                if (dsusers.Tables[0].Rows[l]["DisplayName"].ToString() != string.Empty)
                                                {
                                                    if (ownertooltip != string.Empty)
                                                    {
                                                        ownertooltip += "," + dsusers.Tables[0].Rows[l]["DisplayName"].ToString();
                                                    }
                                                    else
                                                    {
                                                        ownertooltip += dsusers.Tables[0].Rows[l]["DisplayName"].ToString();
                                                    }
                                                }

                                            }


                                            for (int k = 0; k < 3; k++)
                                            {

                                                if (dsusers.Tables[0].Rows[k]["fileupload1"].ToString() != "")
                                                {
                                                    usersdetailsowner += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title=" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + " data-bs-original-title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "'><img src = '" + dsusers.Tables[0].Rows[k]["fileupload1"] + "' alt='' class='rounded-circle avatar-xxs'></a>";
                                                }
                                                else
                                                {
                                                    usersdetailsowner += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover'" +
                                 " data-bs-placement='top' title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "' data-bs-original-title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "'>";
                                                    usersdetailsowner += @"<div class='avatar-xxs'><div class='avatar-title rounded-circle bg-light text-primary'>" + dsusers.Tables[0].Rows[k]["splinameone"].ToString() + "</div></div>";
                                                    usersdetailsowner += @"</a>";
                                                }
                                            }

                                        }
                                        else
                                        {
                                            cntownr = "";
                                            for (int k = 0; k < dsusers.Tables[0].Rows.Count; k++)
                                            {
                                                if (dsusers.Tables[0].Rows[k]["fileupload1"].ToString() != "")
                                                {
                                                    //img src='/assets/images/avatar-1.jpg'
                                                    usersdetailsowner += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='' data-bs-original-title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "'><img src = '" + dsusers.Tables[0].Rows[k]["fileupload1"] + "' alt='' class='rounded-circle avatar-xxs'></a>";
                                                }
                                                else
                                                {
                                                    usersdetailsowner += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover'  data-bs-placement='top' title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "' data-bs-original-title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "'>";
                                                    usersdetailsowner += @"<div class='avatar-xxs'>
                            <div class='avatar-title rounded-circle bg-light text-primary'>" + dsusers.Tables[0].Rows[k]["splinameone"].ToString() + "</div></div>";
                                                    usersdetailsowner += @"</a>";

                                                }
                                            }
                                        }

                                    }
                                }

                                string cnt = "0";
                                string usersdetails = "";
                                string teamtooltip = "";
                                if (ds.Tables[0].Rows[i]["team"].ToString() != "")
                                {
                                    string[] arcnt = ds.Tables[0].Rows[i]["team"].ToString().Trim().Split(',');
                                    cnt = arcnt.Length.ToString();
                                    DataSet dsusers = new DataSet();
                                    if (ds.Tables[0].Rows[i]["team"].ToString().Trim() != "")
                                    {
                                        dsusers = da.GetDataSetInline("select id,DisplayName,fileupload1,SUBSTRING (DisplayName, 1, 2) as splinameone from tbl_employee where id in(" + ds.Tables[0].Rows[i]["team"].ToString().Trim() + ")");
                                        if (dsusers.Tables[0].Rows.Count > 3)
                                        {
                                            cnt = (Convert.ToInt32(cnt) - 3).ToString();

                                            // team tool tip //
                                            for (int l = 3; l < dsusers.Tables[0].Rows.Count; l++)
                                            {
                                                if (dsusers.Tables[0].Rows[l]["DisplayName"].ToString() != string.Empty)
                                                {
                                                    if (teamtooltip != string.Empty)
                                                    {
                                                        teamtooltip += "," + dsusers.Tables[0].Rows[l]["DisplayName"].ToString();
                                                    }
                                                    else
                                                    {
                                                        teamtooltip += dsusers.Tables[0].Rows[l]["DisplayName"].ToString();
                                                    }
                                                }

                                            }

                                            for (int k = 0; k < 3; k++)
                                            {
                                                if (dsusers.Tables[0].Rows[k]["fileupload1"].ToString() != "")
                                                {
                                                    usersdetails += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title=" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + " data-bs-original-title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "'><img src = '" + dsusers.Tables[0].Rows[k]["fileupload1"] + "' alt='' class='rounded-circle avatar-xxs'></a>";
                                                }
                                                else
                                                {
                                                    usersdetails += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover'" +
                                 " data-bs-placement='top' title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "' data-bs-original-title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "'>";
                                                    usersdetails += @"<div class='avatar-xxs'><div class='avatar-title rounded-circle bg-light text-primary'>" + dsusers.Tables[0].Rows[k]["splinameone"].ToString() + "</div></div>";
                                                    usersdetails += @"</a>";
                                                }
                                            }

                                        }
                                        else
                                        {
                                            cnt = "";
                                            for (int k = 0; k < dsusers.Tables[0].Rows.Count; k++)
                                            {
                                                if (dsusers.Tables[0].Rows[k]["fileupload1"].ToString() != "")
                                                {
                                                    //img src='/assets/images/avatar-1.jpg'
                                                    usersdetails += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='' data-bs-original-title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "'><img src = '" + dsusers.Tables[0].Rows[k]["fileupload1"] + "' alt='' class='rounded-circle avatar-xxs'></a>";
                                                }
                                                else
                                                {
                                                    usersdetails += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover'  data-bs-placement='top' title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "' data-bs-original-title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "'>";
                                                    usersdetails += @"<div class='avatar-xxs'>
                            <div class='avatar-title rounded-circle bg-light text-primary'>" + dsusers.Tables[0].Rows[k]["splinameone"].ToString() + "</div></div>";
                                                    usersdetails += @"</a>";

                                                }
                                            }
                                        }

                                    }
                                }


                                //dependancy start //

                                string cntdepndcy = "0";
                                string depdecydetails = "";
                                string depcytooltip = "";

                                string strdepcysql = "Select * from Pms_tbl_Addtask where id = '" + ds.Tables[0].Rows[i]["id"] + "' and status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                                DataSet dsdepcy = da.GetDataSetInline(strdepcysql);
                                if (dsdepcy.Tables[0].Rows[0]["dependancy"].ToString() != "")
                                {
                                    string[] arcntdep = dsdepcy.Tables[0].Rows[0]["dependancy"].ToString().Trim().Split(',');
                                    cntdepndcy = arcntdep.Length.ToString();
                                    DataSet dsdtlsdepcy = new DataSet();
                                    if (dsdepcy.Tables[0].Rows[0]["dependancy"].ToString().Trim() != "")
                                    {
                                        dsdtlsdepcy = da.GetDataSetInline("select id,taskname from Pms_tbl_Addtask where id in(" + dsdepcy.Tables[0].Rows[0]["dependancy"].ToString().Trim() + ")");
                                        if (dsdtlsdepcy.Tables[0].Rows.Count > 1)
                                        {
                                            cntdepndcy = (Convert.ToInt32(cntdepndcy) - 1).ToString();

                                            // dependancy tool tip //
                                            for (int l = 1; l < dsdtlsdepcy.Tables[0].Rows.Count; l++)
                                            {
                                                if (dsdtlsdepcy.Tables[0].Rows[l]["taskname"].ToString() != string.Empty)
                                                {
                                                    if (depcytooltip != string.Empty)
                                                    {
                                                        depcytooltip += "," + dsdtlsdepcy.Tables[0].Rows[l]["taskname"].ToString();
                                                    }
                                                    else
                                                    {
                                                        depcytooltip += dsdtlsdepcy.Tables[0].Rows[l]["taskname"].ToString();
                                                    }
                                                }

                                            }

                                            for (int k = 0; k < 1; k++)
                                            {
                                                if (dsdtlsdepcy.Tables[0].Rows[k]["taskname"].ToString() != "")
                                                {

                                                    depdecydetails += "<span class='badge bg-primary'>" + dsdtlsdepcy.Tables[0].Rows[k]["taskname"].ToString() + "</span>";
                                                }
                                                else
                                                {
                                                    depdecydetails += "<span class='badge bg-primary'>" + dsdtlsdepcy.Tables[0].Rows[k]["taskname"].ToString() + "</span>";
                                                }
                                            }

                                        }
                                        else
                                        {
                                            cntdepndcy = "";
                                            for (int k = 0; k < dsdtlsdepcy.Tables[0].Rows.Count; k++)
                                            {
                                                if (dsdtlsdepcy.Tables[0].Rows[k]["taskname"].ToString() != "")
                                                {
                                                    depdecydetails += "<span class='badge bg-primary'>" + dsdtlsdepcy.Tables[0].Rows[k]["taskname"].ToString() + "</span>";
                                                }
                                                else
                                                {
                                                    depdecydetails += "<span class='badge bg-primary'>" + dsdtlsdepcy.Tables[0].Rows[k]["taskname"].ToString() + "</span>";
                                                }
                                            }
                                        }

                                    }
                                }


                                // dependancy end//




                                //   completion  status  start //

                                decimal Wattagecomplite = 0;
                                decimal Wattagepending = 100;
                                decimal projectTotal = 0;
                                decimal PhaseTotal = 0;
                                decimal TaskTotal = 0;

                                string findtask = "select cast(ROUND(isnull(iscompletepercentage,0),2) AS DECIMAL(10,2)) as iscompletepercentage from Pms_tbl_Addtask  where ProjectId='" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "' and Milestoneid='" + ds.Tables[0].Rows[i]["Milestoneid"] + "' and id='" + ds.Tables[0].Rows[i]["id"] + "'  and status=1  and archive=0";
                                DataSet dsssumtask2 = da.GetDataSetInline(findtask);

                                try
                                {

                                    TaskTotal = Convert.ToDecimal(dsssumtask2.Tables[0].Rows[0]["iscompletepercentage"].ToString());
                                }
                                catch (Exception)
                                {

                                }

                                projectTotal = TaskTotal;




                                if (projectTotal > 0)
                                {
                                    Wattagecomplite = projectTotal;
                                    Wattagepending = Math.Abs(Wattagepending - Wattagecomplite);
                                }

                                //// completion task end //



                                //  Sub task count start //

                                float tskcomplite = 0;
                                float tskpending = 0;
                                float tskprojectTotal = 0;
                                float tskPhaseTotal = 0;
                                float tsk_TaskTotal = 0;

                                string str11 = "select COUNT(*) as countsbtsk from Pms_tbl_Addsubtask where ProjectId='" + ds.Tables[0].Rows[i]["ProjectId"] + "' and Milestoneid='" + ds.Tables[0].Rows[i]["Milestoneid"] + "' and task='" + ds.Tables[0].Rows[i]["id"] + "' and status=1 and archive=0 ";
                                DataSet dss11 = da.GetDataSetInline(str11);
                                if (dss11.Tables[0].Rows.Count > 0)
                                {

                                    try
                                    {
                                        tskPhaseTotal = Convert.ToInt32(dss11.Tables[0].Rows[0]["countsbtsk"]);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }

                                string findtask11 = "select COUNT(*) as counttask  from Pms_tbl_Addsubtask where ProjectId='" + ds.Tables[0].Rows[i]["ProjectId"] + "' and  Milestoneid='" + ds.Tables[0].Rows[i]["Milestoneid"] + "' and task='" + ds.Tables[0].Rows[i]["id"] + "' and P_Status in ( select id from Pms_tbl_Projectstatus where  status=1 and archive=0 and isnull(iscomplete,0)=1)  and status=1 and archive=0";
                                DataSet dsssumtask11 = da.GetDataSetInline(findtask11);

                                try
                                {
                                    tsk_TaskTotal = Convert.ToInt32(dsssumtask11.Tables[0].Rows[0]["counttask"]);
                                }
                                catch (Exception)
                                {

                                }


                                tskprojectTotal = (tskPhaseTotal - tsk_TaskTotal);




                                if (tskprojectTotal > 0)
                                {
                                    tskcomplite = tsk_TaskTotal;
                                    tskpending = Math.Abs(tskPhaseTotal - tsk_TaskTotal);
                                }

                                if (tskprojectTotal == 0)
                                {
                                    tskcomplite = tsk_TaskTotal;
                                    tskpending = Math.Abs(tskPhaseTotal - tsk_TaskTotal);
                                }

                                //Sub task count end //




                                //   Issues count start //

                                float Issuescomplite = 0;
                                float Issuespending = 0;
                                float IssuesrojectTotal = 0;
                                float IssuesPhaseTotal = 0;
                                float Issues_Total = 0;

                                string str111 = "select COUNT(*) as countIssues from Pms_tbl_Addissues where ProjectId='" + ds.Tables[0].Rows[i]["ProjectId"] + "' and Milestoneid='" + ds.Tables[0].Rows[i]["Milestoneid"] + "' and taskid='" + ds.Tables[0].Rows[i]["id"] + "' and Issueon='task' and status=1 and archive=0 ";
                                DataSet dss111 = da.GetDataSetInline(str111);
                                if (dss111.Tables[0].Rows.Count > 0)
                                {

                                    try
                                    {
                                        IssuesPhaseTotal = Convert.ToInt32(dss111.Tables[0].Rows[0]["countIssues"]);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }


                                string findtask111 = "select COUNT(*) as count_Issues from Pms_tbl_Addissues where ProjectId='" + ds.Tables[0].Rows[i]["ProjectId"] + "' and Milestoneid='" + ds.Tables[0].Rows[i]["Milestoneid"] + "' and taskid='" + ds.Tables[0].Rows[i]["id"] + "' and Issueon='task' and IssueStatus in ( select id from Pms_tbl_Projectstatus where  status=1 and archive=0 and isnull(iscomplete,0)=1)  and status=1 and archive=0";
                                DataSet dsssumtask111 = da.GetDataSetInline(findtask111);
                                if (dsssumtask111.Tables[0].Rows.Count > 0)
                                {
                                    try
                                    {
                                        Issues_Total = Convert.ToInt32(dsssumtask111.Tables[0].Rows[0]["count_Issues"]);
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }


                                IssuesrojectTotal = (IssuesPhaseTotal - Issues_Total);


                                if (IssuesrojectTotal > 0)
                                {
                                    Issuescomplite = Issues_Total;
                                    Issuespending = Math.Abs(IssuesPhaseTotal - Issues_Total);
                                }

                                if (IssuesrojectTotal == 0)
                                {
                                    Issuescomplite = Issues_Total;
                                    Issuespending = Math.Abs(IssuesPhaseTotal - Issues_Total);
                                }

                                // Issues count end //



                                taskdata += "<tr id='tr_" + ds1.Tables[0].Rows[j]["id"].ToString() + "' class='taskshow_" + ds1.Tables[0].Rows[j]["id"].ToString() + "' style='display:none;'>";

                                //sno //

                                taskdata += "<td valign='top'>";
                                taskdata += "";
                                taskdata += "</td>";

                                //taskdata += " <td><a href='javascript:void(0);' Class='btn btn-info btn-sm' type='button' onclick='Binddetails(" + ds.Tables[0].Rows[i]["id"].ToString() + ")' data-bs-toggle='offcanvas' data-bs-target='#addtask'><i class='ri-edit-line'></i></a>";
                                 taskdata += " <td><a href='javascript:void(0);' Class='btn btn-info btn-sm' type='button' onclick='loadsrcedit(\"editdetails\", \"/pms/edittask.aspx?id=" + ds.Tables[0].Rows[i]["id"].ToString() + "&mid=" + ds1.Tables[0].Rows[j]["id"].ToString() + "\",\"Edit Task\")'  data-bs-toggle='offcanvas' data-bs-target='#addmile'><i class='ri-edit-line'></i></a>";

                                taskdata += "&nbsp;&nbsp;<a href='javascript:void(0);' Class='btn btn-danger btn-sm' type='button' onclick='deletedata(\"" + dsproject.Tables[0].Rows[P]["id"].ToString() + "\",\"" + ds1.Tables[0].Rows[j]["id"].ToString() + "\",\"" + ds.Tables[0].Rows[i]["id"].ToString() + "\",this)'><i class='ri-delete-bin-5-line'></i></a>";
                                taskdata += "</td>";

                                //// timer 

                                ////check statuscomplet //
                                //int isstatuscomplete = 0;
                                //string strchsttscomplt = "select isnull(iscomplete,0) as iscomplete from Pms_tbl_Projectstatus where status = 1 and archive = 0  and id in (select P_Status from Pms_tbl_Addtask where ProjectId = '" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "' and Milestoneid = '" + ds.Tables[0].Rows[i]["Milestoneid"].ToString() + "' and id = '" + ds.Tables[0].Rows[i]["id"].ToString() + "' and status = 1 and archive = 0) and isnull(iscomplete,0)= 1  and status = 1 and archive = 0 ";
                                //DataSet dschksttscomplt = da.GetDataSetInline(strchsttscomplt);
                                //if (dschksttscomplt.Tables[0].Rows.Count > 0)
                                //{


                                //    isstatuscomplete = Convert.ToInt32(dschksttscomplt.Tables[0].Rows[0]["iscomplete"].ToString());
                                //}


                                ////check issubtask start //

                                int checkissubtask = 0;
                                string strchksubtask = " select isnull(issubtask,0) issubtask from  Pms_tbl_Addtask where id='" + ds.Tables[0].Rows[i]["id"].ToString() + "' ";
                                DataSet dschksubtask = da.GetDataSetInline(strchksubtask);
                                if (dschksubtask.Tables[0].Rows.Count > 0)
                                {

                                    checkissubtask = Convert.ToInt32(dschksubtask.Tables[0].Rows[0]["issubtask"].ToString());
                                }



                                string checkshowhidecolmn = "";
                                string showhidecolmn_task = "";
                                string strchkhidecolmn = " select * from Pms_tbl_Addsubtask where task='" + ds.Tables[0].Rows[i]["id"].ToString() + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by isnull(orderno,0) ";
                                DataSet dshideshow = da.GetDataSetInline(strchkhidecolmn);
                                if (dshideshow.Tables[0].Rows.Count > 0)
                                {

                                    checkshowhidecolmn = "class='details-control'";
                                    showhidecolmn_task = "id='showtasktr_" + ds.Tables[0].Rows[i]["id"].ToString() + "'";
                                }



                                //taskdata += "<td " + showhidecolmn_task + " " + checkshowhidecolmn + ">";
                                //taskdata += "&nbsp;&nbsp;&nbsp;&nbsp; " + (j + 1) + "." + (i + 1) + " " + ds.Tables[0].Rows[i]["taskname"].ToString();

                                //if (checkissubtask != 0)
                                //{

                                //    taskdata += "<a href = 'javascript:void(0);' type='button' onclick='AddMultisubtask(" + dsproject.Tables[0].Rows[P]["id"].ToString() + "," + ds1.Tables[0].Rows[j]["id"].ToString() + "," + ds.Tables[0].Rows[i]["id"].ToString() + ")' data-bs-toggle='offcanvas' data-bs-target='#addsubtask'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Sub Task'>+</span></a>";

                                //}
                                //pi_mi_ti_col
                                taskdata += "<td> <div class='d-flex'><div class='flex-grow-1'>";
                                string txtval2 = ds.Tables[0].Rows[i]["taskname"].ToString();
                                if (txtval2.Trim() == "")
                                {
                                    txtval2 = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                                }
                                taskdata += "<span  style='font-size:14px;'><b>&nbsp;&nbsp;&nbsp;&nbsp; " + (j + 1) + "." + (i + 1) + " &nbsp;<span id='lblm_" + dsproject.Tables[0].Rows[P]["id"].ToString()+"_"+ ds1.Tables[0].Rows[j]["id"].ToString()+ "_"+ ds.Tables[0].Rows[i]["id"].ToString() + "_1' onclick='edittexttask(this.id)' style='background:#fde796;'> " + ds.Tables[0].Rows[i]["taskname"].ToString() + "</span></b></span>";
                               

                                taskdata += "<input type='text' id='txtm_" + dsproject.Tables[0].Rows[P]["id"].ToString() + "_" + ds1.Tables[0].Rows[j]["id"].ToString() + "_" + ds.Tables[0].Rows[i]["id"].ToString() + "_1' onblur='updatedetailstask(this.id,\"" + ds.Tables[0].Rows[i]["id"].ToString() + "\",\"taskname\");' style='display:none;border:1px solid #ddd;outline:none;' value='" + txtval2 + "'>";

                                taskdata += "&nbsp;<a href ='javascript:void(0);' role='button' data-bs-toggle='dropdown' aria-expanded='false'><span id='btn_" + dsproject.Tables[0].Rows[P]["id"].ToString() + "_" + ds1.Tables[0].Rows[j]["id"].ToString() + "_" + ds.Tables[0].Rows[i]["id"].ToString() + "' class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover'>+</span>" +
                                "<ul class='dropdown-menu  dropdownmenu-secondary'>" +

                                "<li onclick='addrowUpDowntask(\"1\",\"" + i + "\",\"" + dsproject.Tables[0].Rows[P]["id"].ToString() + "\",\"" + ds1.Tables[0].Rows[j]["id"].ToString() + "\",\"" + ds.Tables[0].Rows[i]["id"].ToString() + "\",\"" + ds.Tables[0].Rows[i]["orderno"].ToString() + "\")' style='display:inline-block;'><i class='bi bi-arrow-up-square' data-bs-toggle='' data-bs-placement='top' title='Add Above'></i></li>" +
                                "<li onclick='addrowUpDowntask(\"2\",\"" + i + "\",\"" + dsproject.Tables[0].Rows[P]["id"].ToString() + "\",\"" + ds1.Tables[0].Rows[j]["id"].ToString() + "\",\"" + ds.Tables[0].Rows[i]["id"].ToString() + "\",\"" + ds.Tables[0].Rows[i]["orderno"].ToString() + "\")' style='display:inline-block;margin:0 15px;'><i class='bi-arrow-down-square' data-bs-toggle='' data-bs-placement='top' title='Add Below'></i></li>";
                                int chksubtask = 0;
                                if (checkissubtask != 0)
                                {
                                    string strsqltsk = "select * from Pms_tbl_Addsubtask where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0 and ProjectId='" + dsproject.Tables[0].Rows[P]["id"].ToString() + "' and Milestoneid='" + ds1.Tables[0].Rows[j]["id"].ToString() + "' and task='" + ds.Tables[0].Rows[i]["id"].ToString() + "' ";
                                    DataSet dstaskchk = da.GetDataSetInline(strsqltsk);
                                    if (dstaskchk.Tables[0].Rows.Count > 0)
                                    {
                                        chksubtask = 1;
                                        //taskdata += "<li " + showhidecolmn_task + " " + checkshowhidecolmn + ">➕ Add Sub Task</li>";
                                    }
                                    //else
                                    //{

                                    //    taskdata += "<li " + showhidecolmn_task + " " + checkshowhidecolmn + " onclick='addrowUpDownsubtask(\"2\",\"0\",\"" + dsproject.Tables[0].Rows[P]["id"].ToString() + "\",\"" + ds1.Tables[0].Rows[j]["id"].ToString() + "\",\"" + ds.Tables[0].Rows[i]["id"].ToString() + "\",\"\",\"0\")'>➕ Add Sub Task</li>";
                                    //}
                                    taskdata += "<li onclick='addrowUpDownsubtask(\"2\",\"0\",\"" + dsproject.Tables[0].Rows[P]["id"].ToString() + "\",\"" + ds1.Tables[0].Rows[j]["id"].ToString() + "\",\"" + ds.Tables[0].Rows[i]["id"].ToString() + "\",\"\",\"0\")' style='display:inline-block;'><i class='bi-card-checklist label-icon align-middle me-1' data-bs-toggle='' data-bs-placement='top' title='Add Sub Task'></i></li>";
                                }
                                taskdata += "</ul></a></div>";
                                if (chksubtask == 1)
                                {
                                    if(isownerT==1||chkadmin==1)
                                    {
                                        taskdata += "<div class='gap-1 flex-shrink-0 align-self-center'><a href='javascript:void(0);' " + showhidecolmn_task + " onclick='updatesrc(\"" + dsproject.Tables[0].Rows[P]["id"].ToString() + "\",\"" + ds1.Tables[0].Rows[j]["id"].ToString() + "\",\"" + ds.Tables[0].Rows[i]["id"].ToString() + "\",\"t\",this.id)'><img id='imgidt_" + ds.Tables[0].Rows[i]["id"].ToString() + "' src='/pms/assets/images/open.png' alt='open'></a></div></a><a href='javascript:void(0);' type='button' data-bs-toggle='offcanvas' data-bs-target='#canvas_popup' onclick='loadsrc(\"inner_popup\",\"/pms/AddSTDependency.aspx?id=" + ds.Tables[0].Rows[i]["id"].ToString() + "&mid=" + ds1.Tables[0].Rows[j]["id"].ToString() + "&pid=" + dsproject.Tables[0].Rows[P]["id"].ToString() + "&mprojectid=" + dsproject.Tables[0].Rows[P]["id"].ToString() + "&type=task\")'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Dependency' style='font-size:12px;height:22px;width:22px;margin-left:9px'>D</span></a>&nbsp;&nbsp;&nbsp; <a href='javascript:void(0);' type='button' data-bs-toggle='offcanvas' data-bs-target='#canvas_popup' onclick='loadsrc(\"inner_popup\",\"/pms/AddIssuesPopup.aspx?id=" + dsproject.Tables[0].Rows[P]["id"].ToString() + "&milstoneid=" + ds1.Tables[0].Rows[j]["id"].ToString() + "&taskid=" + ds.Tables[0].Rows[i]["id"].ToString() + "&tasklistid=0&mprojectid=" + dsproject.Tables[0].Rows[P]["id"].ToString() + "&type=task\")'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Issue' style='font-size:12px;height:22px;width:22px;'>I</span></a></div>";
                                    }
                                    else
                                    {
                                        taskdata += "<div class='gap-1 flex-shrink-0 align-self-center'><a href='javascript:void(0);' " + showhidecolmn_task + " onclick='updatesrc(\"" + dsproject.Tables[0].Rows[P]["id"].ToString() + "\",\"" + ds1.Tables[0].Rows[j]["id"].ToString() + "\",\"" + ds.Tables[0].Rows[i]["id"].ToString() + "\",\"t\",this.id)'><img id='imgidt_" + ds.Tables[0].Rows[i]["id"].ToString() + "' src='/pms/assets/images/open.png' alt='open'></a></div></a></div>";
                                    }
                                    
                                }
                                else
                                {
                                    if (isownerT == 1 || chkadmin == 1)
                                    {
                                        taskdata += "<div class='gap-1 flex-shrink-0 align-self-center'><a href='javascript:void(0);' type='button' data-bs-toggle='offcanvas' data-bs-target='#canvas_popup' onclick='loadsrc(\"inner_popup\",\"/pms/AddSTDependency.aspx?id=" + ds.Tables[0].Rows[i]["id"].ToString() + "&mid=" + ds1.Tables[0].Rows[j]["id"].ToString() + "&pid=" + dsproject.Tables[0].Rows[P]["id"].ToString() + "&mprojectid=" + dsproject.Tables[0].Rows[P]["id"].ToString() + "&type=task\")'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Dependency' style='font-size:12px;height:22px;width:22px;margin-left:9px'>D</span></a>&nbsp;&nbsp;&nbsp; <a href='javascript:void(0);' type='button' data-bs-toggle='offcanvas' data-bs-target='#canvas_popup' onclick='loadsrc(\"inner_popup\",\"/pms/AddIssuesPopup.aspx?id=" + dsproject.Tables[0].Rows[P]["id"].ToString() + "&milstoneid=" + ds1.Tables[0].Rows[j]["id"].ToString() + "&tasklistid=0&mprojectid=" + dsproject.Tables[0].Rows[P]["id"].ToString() + "&taskid=" + ds.Tables[0].Rows[i]["id"].ToString() + "&type=task\")'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Issue' style='font-size:12px;height:22px;width:22px;'>I</span></a></div>";
                                    }
                                    
                                }
                                taskdata += "</td>";
                                // task name




                                //completion status start 

                                taskdata += "<td>";

                                if (Wattagecomplite == 0)
                                {

                                    taskdata += "<div class='progress progress-xl cursor-pointer'>";
                                    taskdata += "<div class='progress progress-xl bg-light position-relative' data-bs-toggle='tooltip' data-bs-placement='top' title='Complete(0%)' role='progressbar' style='width:5%'><span style='text-align:center;'>0</span>";
                                    taskdata += "</div>";

                                    taskdata += "</div>";

                                }

                                else
                                {

                                    taskdata += "<div class='progress progress-xl cursor-pointer'>";
                                    taskdata += "<div class='progress-bar bg-success' data-bs-toggle='tooltip' data-bs-placement='top' title='Complete (" + Wattagecomplite + "%)' role='progressbar' style='width:" + Wattagecomplite + "%'>" + Wattagecomplite + "%";
                                    taskdata += "</div>";

                                    taskdata += "<div class='progress-bar bg-warning' data-toggle='tooltip' data-bs-placemen='top' title='Pending (" + Wattagepending + "%)' role='progressbar' style='width:" + Wattagepending + "%'>" + Wattagepending + "%";
                                    taskdata += "</div>";
                                    taskdata += "</div>";


                                }

                                taskdata += "</td>";


                                // completion status end //


                                //// Task //
                                //taskdata += "<td valign='top'>";
                                //taskdata += "";
                                //taskdata += "</td>";

                                taskdata += "<td valign='top'>";
                                //taskdata += fns.ToFormattedCurrencyString(ds.Tables[0].Rows[i]["Weightage"].ToString()); // Weightage 
                                string wt_task = fns.ToFormattedCurrencyString(ds.Tables[0].Rows[i]["Weightage"].ToString());
                                if (wt_task.Trim() == "")
                                {
                                    wt_task = "0";
                                }
                                taskdata += "<span id='lblm_" + dsproject.Tables[0].Rows[P]["id"].ToString() + "_" + ds1.Tables[0].Rows[j]["id"].ToString() + "_" + ds.Tables[0].Rows[i]["id"].ToString() + "_3' onclick='edittexttask(this.id)' style='background:#fde796;'> " + wt_task + "</span>";


                                taskdata += "<input type='text' id='txtm_" + dsproject.Tables[0].Rows[P]["id"].ToString() + "_" + ds1.Tables[0].Rows[j]["id"].ToString() + "_" + ds.Tables[0].Rows[i]["id"].ToString() + "_3' onblur='updatedetailstask(this.id,\"" + ds.Tables[0].Rows[i]["id"].ToString() + "\",\"Weightage\");' style='display:none;border:1px solid #ddd;outline:none;' value='" + fns.ToFormattedCurrencyString(ds.Tables[0].Rows[i]["Weightage"].ToString()) + "' onkeypress='return isnumeric(event);'>";

                                taskdata += "</td>";


                                //Owner
                                taskdata += "<td>";
                                taskdata += "<div class='d-flex'>";
                                taskdata += "<div class='flex-grow-1'>";
                                taskdata += "<div class='avatar-group' id='lblowner_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>";
                                taskdata += usersdetailsowner;
                                taskdata += "<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='" + ownertooltip + "' data-bs-original-title='more'>";

                                if (cntownr != "" && cnt != "0")
                                {
                                    taskdata += "<div class='avatar-xxs'>";
                                    taskdata += "<div class='avatar-title rounded-circle'>";

                                    taskdata += (cntownr + "+");
                                    taskdata += "</div>";
                                    taskdata += "</div>";
                                }

                                taskdata += "</a>";
                                taskdata += "</div>";
                                taskdata += "</div>";
                                taskdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                                taskdata += "<a href ='javascript:void(0);' type='button' id='btnownr_" + ds.Tables[0].Rows[i]["id"].ToString() + "' onclick='Binddetails_Owner(" + ds.Tables[0].Rows[i]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#changeowner'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Owner'>+</span></a>";
                                taskdata += "</div>";
                                taskdata += "</div>";
                                taskdata += "</td>";


                                //team
                                taskdata += "<td>";
                                taskdata += "<div class='d-flex'>";
                                taskdata += "<div class='flex-grow-1'>";
                                taskdata += "<div class='avatar-group' id='lbluser_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>";
                                taskdata += usersdetails;

                                //projectdata += "</a>";
                                taskdata += "<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='" + teamtooltip + "' data-bs-original-title='more'>";

                                if (cnt != "" && cnt != "0")
                                {
                                    taskdata += "<div class='avatar-xxs'>";
                                    taskdata += "<div class='avatar-title rounded-circle'>";

                                    taskdata += (cnt + "+");
                                    taskdata += "</div>";
                                    taskdata += "</div>";
                                }

                                taskdata += "</a>";
                                taskdata += "</div>";
                                taskdata += "</div>";
                                taskdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                                taskdata += "<a href ='javascript:void(0);' type='button' id='btnteam_" + ds.Tables[0].Rows[i]["id"].ToString() + "' onclick='Binddetails_Team(" + ds.Tables[0].Rows[i]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#addteam'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Team'>+</span></a>";
                                taskdata += "</div>";
                                taskdata += "</div>";
                                taskdata += "</td>";

                                //Issues

                                taskdata += "<td valign='top'>";

                                float completetepercentissues = 0;
                                float Pendingepercentissues = 0;
                                completetepercentissues = Issuescomplite * 100 / IssuesPhaseTotal;
                                Pendingepercentissues = Issuespending * 100 / IssuesPhaseTotal;

                                taskdata += "<div class='d-flex'>";
                                taskdata += "<div class='flex-grow-1 align-self-center'>";

                                taskdata += "<div class='progress progress-xl cursor-pointer'>";
                                taskdata += "<div class='progress-bar bg-success' role='progressbar' style='width: " + fns.ToFormattedCurrencyString(completetepercentissues.ToString()) + "%' aria-valuemin='0' aria-valuemax='100' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + fns.ToFormattedCurrencyString(completetepercentissues.ToString()) + "% [" + Issuescomplite + " of " + IssuesPhaseTotal + " Issues Completed]'>" + Issuescomplite + "</div>";

                                taskdata += "<div class='progress-bar bg-warning' role='progressbar' style='width: " + fns.ToFormattedCurrencyString(Pendingepercentissues.ToString()) + "%' aria-valuemin='0' aria-valuemax='100' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + fns.ToFormattedCurrencyString(Pendingepercentissues.ToString()) + "% [" + Issuespending + " of " + IssuesPhaseTotal + " Issues Not Completed]'>" + Issuespending + "</div>";
                                taskdata += "</div>";
                                taskdata += "</div>";

                                taskdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                                taskdata += "<a href ='javascript:void(0);' type='button' data-bs-toggle='offcanvas' data-bs-target=#canvas_popup onclick=loadsrc('inner_popup','/pms/AddIssuesPopup.aspx?id=" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "&milstoneid=" + ds.Tables[0].Rows[i]["Milestoneid"].ToString() + "&tasklistid=" + ds.Tables[0].Rows[i]["tasklist"].ToString() + "&taskid=" + ds.Tables[0].Rows[i]["id"].ToString() + "&type=task')><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Issue'>+</span></a>";

                                taskdata += "</div>";
                                taskdata += "</td>";
                                //Timeline

                                //Storing input Dates  
                                DateTime StartDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"].ToString() == "" ? DateTime.Now.ToString() : ds.Tables[0].Rows[i]["StartDate"].ToString());
                                DateTime Targetdate = Convert.ToDateTime(ds.Tables[0].Rows[i]["Targetdate"].ToString() == "" ? DateTime.Now.ToString() : ds.Tables[0].Rows[i]["Targetdate"].ToString());
                                TimeSpan objTimeSpan = Targetdate - StartDate;
                                double Days = Convert.ToDouble(objTimeSpan.Days);
                                Days = Days + 1;


                                DateTime Enddate = Convert.ToDateTime(ds.Tables[0].Rows[i]["Targetdate"].ToString() == "" ? DateTime.Now.ToString() : ds.Tables[0].Rows[i]["Targetdate"].ToString());
                                DateTime Currentdate = DateTime.Now;
                                TimeSpan objtimeleftspn = Enddate - Currentdate;
                                double leftDays = Convert.ToDouble(objtimeleftspn.Days);
                                leftDays = leftDays + 1;

                                string leftdlydys = "";
                                if (leftDays < 0)
                                {
                                    leftdlydys = "days delayed";
                                }
                                else
                                {
                                    leftdlydys = "days left";
                                }


                                //start days months  
                                string startmonthname = "";
                                int ststartdys = StartDate.Day;
                                int strtmonth = StartDate.Month;
                                int taskstartyear = StartDate.Year;
                                SaveLogText("strtmonthT-"+ strtmonth+"-"+ StartDate);
                                startmonthname = getmonthname(strtmonth);


                                //end days months 
                                string endmonthname = "";
                                int enddys = Targetdate.Day;
                                int endmonth = Targetdate.Month;
                                int taskEndyear = Targetdate.Year;
                               
                                endmonthname = getmonthname(endmonth);

                                //title = '"+leftDays+" days left'

                                taskdata += "<td>";
                                taskdata += "<a href = 'javascript:void(0);' class='timesheet-wrap' onclick='addeventdateselect(\"" + dsproject.Tables[0].Rows[P]["id"].ToString() + "\",\"" + ds1.Tables[0].Rows[j]["id"].ToString() + "\",\"" + ds.Tables[0].Rows[i]["id"].ToString() + "\",\"\",\"t\",this);'>";
                                taskdata += "<div id='title1_" + dsproject.Tables[0].Rows[P]["id"].ToString() + "_" + ds1.Tables[0].Rows[j]["id"].ToString() + "_"+ ds.Tables[0].Rows[i]["id"].ToString()+"' class='progress progress-xl bg-dark position-relative' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + leftDays.ToString().Replace("-", "") + " " + leftdlydys + "'>";
                                // taskdata += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 70%;' aria-valuemax='100'></div>";
                                taskdata += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 100%;' aria-valuemax='100'></div>";
                                taskdata += "<div class='dayswrapper'>";
                                taskdata += "<span class='fromtodate' id='spn1_" + dsproject.Tables[0].Rows[P]["id"].ToString() + "_" + ds1.Tables[0].Rows[j]["id"].ToString() + "_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>" + ststartdys + " " + startmonthname + " " + taskstartyear + " - " + enddys + " " + endmonthname + " " + taskEndyear + "</span>";
                                taskdata += "<span class='totaldays' id='spn2_" + dsproject.Tables[0].Rows[P]["id"].ToString() + "_" + ds1.Tables[0].Rows[j]["id"].ToString() + "_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>" + Days + " days</span>";
                                taskdata += "</div>";
                                taskdata += "</div>";
                                taskdata += "</a>";
                                taskdata += "</td>";




                                //Task extended timeleine 

                                string task_estendleftdlydys = "";
                                string Task_estendstartmonthname = "";
                                string task_estendendmonthname = "";
                                double task_estendleftDays = 0;
                                int task_estendststartdys = 0;
                                int task_estendenddys = 0;
                                double task_estendDays = 0;
                                string task_extenddatebind = "";
                                string task_strextend = "select id,extendStartdate as StartDate,extendEnddate as TargetDate from Pms_Tbl_Extended_Timeline where  status=1 and Type='task' and archive=0  and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and taskid = '" + ds.Tables[0].Rows[i]["id"].ToString() + "'  ";
                                DataSet task_dsstrextend = da.GetDataSetInline(task_strextend);
                                if (task_dsstrextend.Tables[0].Rows.Count > 0)
                                {
                                    for (int Tk = 0; Tk < task_dsstrextend.Tables[0].Rows.Count; Tk++)
                                    {
                                        DateTime task_estendStartDate = Convert.ToDateTime(task_dsstrextend.Tables[0].Rows[Tk]["StartDate"].ToString() == "" ? DateTime.Now.ToString() : task_dsstrextend.Tables[0].Rows[Tk]["StartDate"].ToString());
                                        DateTime task_estendTargetdate = Convert.ToDateTime(task_dsstrextend.Tables[0].Rows[Tk]["TargetDate"].ToString() == "" ? DateTime.Now.ToString() : task_dsstrextend.Tables[0].Rows[Tk]["TargetDate"].ToString());
                                        TimeSpan task_estendobjTimeSpan = task_estendTargetdate - task_estendStartDate;
                                        task_estendDays = Convert.ToDouble(task_estendobjTimeSpan.TotalDays);
                                        task_estendDays = task_estendDays + 1;


                                        DateTime task_estendEnddate = Convert.ToDateTime(task_dsstrextend.Tables[0].Rows[Tk]["Targetdate"].ToString() == "" ? DateTime.Now.ToString() : task_dsstrextend.Tables[0].Rows[Tk]["Targetdate"].ToString());
                                        DateTime task_estendCurrentdate = DateTime.Now;
                                        TimeSpan task_estendobjtimeleftspn = task_estendEnddate - task_estendCurrentdate;
                                        task_estendleftDays = Convert.ToDouble(task_estendobjtimeleftspn.Days);
                                        task_estendleftDays = task_estendleftDays + 1;


                                        if (task_estendleftDays < 0)
                                        {
                                            task_estendleftdlydys = "days delayed";
                                        }
                                        else
                                        {
                                            task_estendleftdlydys = "days left";
                                        }

                                        //start days months  

                                        task_estendststartdys = task_estendCurrentdate.Day;
                                        int task_estendstrtmonth = task_estendCurrentdate.Month;
                                       
                                        Task_estendstartmonthname = getmonthname(task_estendstrtmonth);
                                        //end days months 

                                        task_estendenddys = task_estendTargetdate.Day;
                                        int task_estendendmonth = task_estendTargetdate.Month;
                                       
                                        task_estendendmonthname = getmonthname(task_estendendmonth);


                                        task_extenddatebind += "<a href = 'javascript:void(0);' class='timesheet-wrap' >";
                                        task_extenddatebind += "<div class='progress progress-xl bg-dark position-relative me-1' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + task_estendststartdys + " " + Task_estendstartmonthname + " - " + task_estendenddys + " " + task_estendendmonthname + " (" + task_estendleftDays.ToString().Replace("-", "") + " " + task_estendleftdlydys + ")'>";
                                        task_extenddatebind += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 100%;' aria-valuemax='100'>" + task_estendDays + " days</div>";
                                        task_extenddatebind += "</div>";
                                        task_extenddatebind += "</a>";



                                    }

                                }




                                taskdata += "<td style='display:none;'>";
                                taskdata += "<div class='d-flex'>";
                                taskdata += "<div class='flex-grow-1 align-self-center me-2'>";
                                taskdata += "<div class='extended_time' id='lblExtnddttask_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>";

                                taskdata += "" + task_extenddatebind + "";

                                taskdata += "</div>";
                                taskdata += "</div>";

                                if (leftDays < 0)
                                {
                                    taskdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                                    taskdata += "<a href ='javascript:void(0);' type='button' id='btnextndttask_" + ds.Tables[0].Rows[i]["id"].ToString() + "'  onclick='Bindextenddatetask_Details(" + ds.Tables[0].Rows[i]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#ExtendedTimline_task'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Extended Day'>+</span></a>";
                                    taskdata += "</div>";
                                }

                                taskdata += "</div>";
                                taskdata += "</div>";
                                taskdata += "</td>";


                                //taskdata += "<td valign='top'>";
                                //taskdata += cmf.dateConvert(ds.Tables[0].Rows[i]["StartDate"].ToString()); // start date
                                //taskdata += "</td>";

                                //taskdata += "<td valign='top'>";
                                //taskdata += cmf.dateConvert(ds.Tables[0].Rows[i]["Targetdate"].ToString()); //target date
                                //taskdata += "</td>";


                                // status

                                string strvalcolor = get_Status_Color(ds.Tables[0].Rows[i]["P_Status"].ToString());
                                taskdata += "<td>";
                                taskdata += "<div class='btn-group d-block'>";
                                taskdata += "<a href ='javascript:void(0);' id='btnsts_" + ds.Tables[0].Rows[i]["id"].ToString() + "' type='button' onclick=setprojectid('" + ds.Tables[0].Rows[i]["id"].ToString() + "',this.id) data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + strvalcolor + "'>" + get_Projectstatus(ds.Tables[0].Rows[i]["P_Status"].ToString()) + "</a>";
                                taskdata += "<div class='dropdown-menu lead_form_body p-0'>";
                                taskdata += retval;
                                taskdata += "</div>";
                                taskdata += "</div>";
                                taskdata += "</td>";




                                //priority
                                string strprioritycolor = get_PriorityColor(ds.Tables[0].Rows[i]["Priority"].ToString());
                                taskdata += "<td>";
                                taskdata += "<div class='btn-group d-block'>";
                                taskdata += "<a href ='javascript:void(0);' id='btnpri_" + ds.Tables[0].Rows[i]["id"].ToString() + "' type='button' onclick=setpriorityid('" + ds.Tables[0].Rows[i]["id"].ToString() + "',this.id) data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + strprioritycolor + "'>" + get_PriorityName(ds.Tables[0].Rows[i]["Priority"].ToString()) + "</a>";
                                taskdata += "<div class='dropdown-menu lead_form_body p-0'>";
                                taskdata += strvalprority;
                                taskdata += "</div>";
                                taskdata += "</div>";
                                taskdata += "</td>";

                                

                                //taskdata += "<td valign='top'>";
                                //taskdata += ds.Tables[0].Rows[i]["Budget"].ToString(); // budget 
                                //taskdata += "</td>";

                                //taskdata += "<td valign='top'>";
                                //taskdata += ds.Tables[0].Rows[i]["Actual"].ToString(); // Actual 
                                //taskdata += "</td>";

                                //Task Duration Hours
                                taskdata += "<td valign='top'>";
                                
                                //taskdata += ds.Tables[0].Rows[i]["TaskDuration"].ToString();
                                string wt_taskduration = ds.Tables[0].Rows[i]["TaskDuration"].ToString();
                                if (wt_taskduration.Trim() == "")
                                {
                                    wt_taskduration = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                                }
                                taskdata += "<span id='lblm_" + dsproject.Tables[0].Rows[P]["id"].ToString() + "_" + ds1.Tables[0].Rows[j]["id"].ToString() + "_" + ds.Tables[0].Rows[i]["id"].ToString() + "_13' onclick='edittexttask(this.id)' style='background:#fde796;'> " + wt_taskduration + "</span>";


                                taskdata += "<input type='text' id='txtm_" + dsproject.Tables[0].Rows[P]["id"].ToString() + "_" + ds1.Tables[0].Rows[j]["id"].ToString() + "_" + ds.Tables[0].Rows[i]["id"].ToString() + "_13' onblur='updatedetailstask(this.id,\"" + ds.Tables[0].Rows[i]["id"].ToString() + "\",\"TaskDuration\");' style='display:none;border:1px solid #ddd;outline:none;' value='" + ds.Tables[0].Rows[i]["TaskDuration"].ToString() + "'>";

                                taskdata += "</td>";



                                // Stage
                                string strvalstagecolor = get_Stage_Color(ds.Tables[0].Rows[i]["stage"].ToString());
                                taskdata += "<td>";
                                taskdata += "<div class='btn-group d-block'>";
                                taskdata += "<a href ='javascript:void(0);' id='btnStage_" + ds.Tables[0].Rows[i]["id"].ToString() + "' type='button' onclick=setStageid('" + ds.Tables[0].Rows[i]["id"].ToString() + "',this.id) data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + strvalstagecolor + "'>" + (get_Stagename(ds.Tables[0].Rows[i]["stage"].ToString())==""?"-": get_Stagename(ds.Tables[0].Rows[i]["stage"].ToString())) + "</a>";
                                taskdata += "<div class='dropdown-menu lead_form_body p-0'>";
                                taskdata += retstageval;
                                taskdata += "</div>";
                                taskdata += "</div>";
                                taskdata += "</td>";
                                //task Description //
                                taskdata += "<td valign='top'>";
                                string strdescriptionT = ds.Tables[0].Rows[i]["taskdescripton"].ToString();
                                if (strdescriptionT == "")
                                {
                                    strdescriptionT = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                                }
                                taskdata += "<span id='lblm_" + dsproject.Tables[0].Rows[P]["id"].ToString() + '_' + ds1.Tables[0].Rows[j]["id"].ToString() + "_" + ds.Tables[0].Rows[i]["id"].ToString()+ "_15' onclick='edittexttask(this.id)' style='background:#fde796;'> " + strdescriptionT + "</span>";

                                taskdata += "<input type='text' id='txtm_" + dsproject.Tables[0].Rows[P]["id"].ToString() + '_' + ds1.Tables[0].Rows[j]["id"].ToString() + "_" + ds.Tables[0].Rows[i]["id"].ToString() + "_15' onblur='updatedetailstask(this.id,\"" + ds.Tables[0].Rows[i]["id"].ToString() + "\",\"taskdescripton\");' style='display:none;border:1px solid #ddd;outline:none;' value='" + ds.Tables[0].Rows[i]["taskdescripton"].ToString() + "' >";

                                taskdata += "</td>";











                                string Workinghors = "0";
                                string strworkinghrs = "select  sum(cast(isnull(hour,0) as numeric)) Workhors  from Pms_tbl_Timerstatus where taskid='" + ds.Tables[0].Rows[i]["id"].ToString() + "' and status=1 and isnull(IsEnd,0)=1 and archive=0 and type='task'";
                                DataSet dsworkghrs = da.GetDataSetInline(strworkinghrs);
                                if (dsworkghrs.Tables[0].Rows.Count > 0)
                                {

                                    Workinghors = dsworkghrs.Tables[0].Rows[0]["Workhors"].ToString();

                                }



                                taskdata += "</tr>";



                                string strsubstask = "select * from Pms_tbl_Addsubtask where ProjectId='" + dsproject.Tables[0].Rows[P]["id"].ToString() + "' and Milestoneid='" + ds1.Tables[0].Rows[j]["id"].ToString() + "' and task='" + ds.Tables[0].Rows[i]["id"].ToString() + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by isnull(orderno,0)  ";
                                DataSet dssubtsk = da.GetDataSetInline(strsubstask);
                                if (dssubtsk.Tables[0].Rows.Count > 0)
                                {

                                    for (int sb = 0; sb < dssubtsk.Tables[0].Rows.Count; sb++)
                                    {

                                        string sbtskcntownr = "0";
                                        string sbtskusersdetailsowner = "";
                                        string sbtskownertooltip = "";
                                        int isownerS = 0;
                                        if (dssubtsk.Tables[0].Rows[sb]["owner"].ToString() != "")
                                        {
                                            string[] arcnt = dssubtsk.Tables[0].Rows[sb]["owner"].ToString().Trim().Split(',');
                                            sbtskcntownr = arcnt.Length.ToString();
                                            try
                                            {
                                                if (arcnt.Contains(HttpContext.Current.Request.Cookies["user"]["user_id"].ToString()))
                                                {
                                                    isownerS = 1;
                                                }
                                            }
                                            catch { }
                                            DataSet dsusers = new DataSet();
                                            if (dssubtsk.Tables[0].Rows[sb]["owner"].ToString().Trim() != "")
                                            {
                                                dsusers = da.GetDataSetInline("select id,DisplayName,fileupload1,SUBSTRING (DisplayName, 1, 2) as splinameone from tbl_employee where id in(" + dssubtsk.Tables[0].Rows[sb]["owner"].ToString().Trim() + ")");
                                                if (dsusers.Tables[0].Rows.Count > 3)
                                                {
                                                    sbtskcntownr = (Convert.ToInt32(sbtskcntownr) - 3).ToString();

                                                    // owner tool tip //
                                                    for (int lT = 3; lT < dsusers.Tables[0].Rows.Count; lT++)
                                                    {
                                                        if (dsusers.Tables[0].Rows[lT]["DisplayName"].ToString() != string.Empty)
                                                        {
                                                            if (sbtskownertooltip != string.Empty)
                                                            {
                                                                sbtskownertooltip += "," + dsusers.Tables[0].Rows[lT]["DisplayName"].ToString();
                                                            }
                                                            else
                                                            {
                                                                sbtskownertooltip += dsusers.Tables[0].Rows[lT]["DisplayName"].ToString();
                                                            }
                                                        }

                                                    }

                                                    for (int U = 0; U < 3; U++)
                                                    {
                                                        if (dsusers.Tables[0].Rows[U]["fileupload1"].ToString() != "")
                                                        {
                                                            sbtskusersdetailsowner += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title=" + dsusers.Tables[0].Rows[U]["DisplayName"].ToString() + " data-bs-original-title='" + dsusers.Tables[0].Rows[U]["DisplayName"].ToString() + "'><img src = '" + dsusers.Tables[0].Rows[U]["fileupload1"] + "' alt='' class='rounded-circle avatar-xxs'></a>";
                                                        }
                                                        else
                                                        {
                                                            sbtskusersdetailsowner += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover'" +
                                         " data-bs-placement='top' title='" + dsusers.Tables[0].Rows[U]["DisplayName"].ToString() + "' data-bs-original-title='" + dsusers.Tables[0].Rows[U]["DisplayName"].ToString() + "'>";
                                                            sbtskusersdetailsowner += @"<div class='avatar-xxs'><div class='avatar-title rounded-circle bg-light text-primary'>" + dsusers.Tables[0].Rows[U]["splinameone"].ToString() + "</div></div>";
                                                            sbtskusersdetailsowner += @"</a>";
                                                        }
                                                    }

                                                }
                                                else
                                                {
                                                    sbtskcntownr = "";
                                                    for (int U = 0; U < dsusers.Tables[0].Rows.Count; U++)
                                                    {
                                                        if (dsusers.Tables[0].Rows[U]["fileupload1"].ToString() != "")
                                                        {
                                                            //img src='/assets/images/avatar-1.jpg'
                                                            sbtskusersdetailsowner += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='' data-bs-original-title='" + dsusers.Tables[0].Rows[U]["DisplayName"].ToString() + "'><img src = '" + dsusers.Tables[0].Rows[U]["fileupload1"] + "' alt='' class='rounded-circle avatar-xxs'></a>";
                                                        }
                                                        else
                                                        {
                                                            sbtskusersdetailsowner += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover'  data-bs-placement='top' title='" + dsusers.Tables[0].Rows[U]["DisplayName"].ToString() + "' data-bs-original-title='" + dsusers.Tables[0].Rows[U]["DisplayName"].ToString() + "'>";
                                                            sbtskusersdetailsowner += @"<div class='avatar-xxs'>
                            <div class='avatar-title rounded-circle bg-light text-primary'>" + dsusers.Tables[0].Rows[U]["splinameone"].ToString() + "</div></div>";
                                                            sbtskusersdetailsowner += @"</a>";

                                                        }
                                                    }
                                                }

                                            }
                                        }


                                        string sbtskcnt = "0";
                                        string sbtskusersdetails = "";
                                        string sbtskteamtooltip = "";
                                        if (dssubtsk.Tables[0].Rows[sb]["team"].ToString() != "")
                                        {
                                            string[] arcnt = dssubtsk.Tables[0].Rows[sb]["team"].ToString().Trim().Split(',');
                                            sbtskcnt = arcnt.Length.ToString();
                                            DataSet dsusers = new DataSet();
                                            if (dssubtsk.Tables[0].Rows[sb]["team"].ToString().Trim() != "")
                                            {
                                                dsusers = da.GetDataSetInline("select id,DisplayName,fileupload1,SUBSTRING (DisplayName, 1, 2) as splinameone from tbl_employee where id in(" + dssubtsk.Tables[0].Rows[sb]["team"].ToString().Trim() + ")");
                                                if (dsusers.Tables[0].Rows.Count > 3)
                                                {
                                                    sbtskcnt = (Convert.ToInt32(sbtskcnt) - 3).ToString();

                                                    // team tool tip //
                                                    for (int sl = 3; sl < dsusers.Tables[0].Rows.Count; sl++)
                                                    {
                                                        if (dsusers.Tables[0].Rows[sl]["DisplayName"].ToString() != string.Empty)
                                                        {
                                                            if (sbtskteamtooltip != string.Empty)
                                                            {
                                                                sbtskteamtooltip += "," + dsusers.Tables[0].Rows[sl]["DisplayName"].ToString();
                                                            }
                                                            else
                                                            {
                                                                sbtskteamtooltip += dsusers.Tables[0].Rows[sl]["DisplayName"].ToString();
                                                            }
                                                        }

                                                    }

                                                    for (int V = 0; V < 3; V++)
                                                    {
                                                        if (dsusers.Tables[0].Rows[V]["fileupload1"].ToString() != "")
                                                        {
                                                            sbtskusersdetails += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title=" + dsusers.Tables[0].Rows[V]["DisplayName"].ToString() + " data-bs-original-title='" + dsusers.Tables[0].Rows[V]["DisplayName"].ToString() + "'><img src = '" + dsusers.Tables[0].Rows[V]["fileupload1"] + "' alt='' class='rounded-circle avatar-xxs'></a>";
                                                        }
                                                        else
                                                        {
                                                            sbtskusersdetails += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover'" +
                                         " data-bs-placement='top' title='" + dsusers.Tables[0].Rows[V]["DisplayName"].ToString() + "' data-bs-original-title='" + dsusers.Tables[0].Rows[V]["DisplayName"].ToString() + "'>";
                                                            sbtskusersdetails += @"<div class='avatar-xxs'><div class='avatar-title rounded-circle bg-light text-primary'>" + dsusers.Tables[0].Rows[V]["splinameone"].ToString() + "</div></div>";
                                                            sbtskusersdetails += @"</a>";
                                                        }
                                                    }

                                                }
                                                else
                                                {
                                                    sbtskcnt = "";
                                                    for (int sV = 0; sV < dsusers.Tables[0].Rows.Count; sV++)
                                                    {
                                                        if (dsusers.Tables[0].Rows[sV]["fileupload1"].ToString() != "")
                                                        {
                                                            //img src='/assets/images/avatar-1.jpg'
                                                            sbtskusersdetails += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='' data-bs-original-title='" + dsusers.Tables[0].Rows[sV]["DisplayName"].ToString() + "'><img src = '" + dsusers.Tables[0].Rows[sV]["fileupload1"] + "' alt='' class='rounded-circle avatar-xxs'></a>";
                                                        }
                                                        else
                                                        {
                                                            sbtskusersdetails += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover'  data-bs-placement='top' title='" + dsusers.Tables[0].Rows[sV]["DisplayName"].ToString() + "' data-bs-original-title='" + dsusers.Tables[0].Rows[sV]["DisplayName"].ToString() + "'>";
                                                            sbtskusersdetails += @"<div class='avatar-xxs'>
                            <div class='avatar-title rounded-circle bg-light text-primary'>" + dsusers.Tables[0].Rows[sV]["splinameone"].ToString() + "</div></div>";
                                                            sbtskusersdetails += @"</a>";

                                                        }
                                                    }
                                                }

                                            }
                                        }



                                        //   completion  status  start //

                                        float subtaskWattagecomplite = 0;
                                        float subtaskWattagepending = 100;
                                        float subtaskprojectTotal = 0;
                                        float subtaskPhaseTotal = 0;
                                        float subtaskTaskTotal = 0;
                                        int subtaskcheckiscomplete = 0;

                                        string subtaskstr = "select isnull(iscomplete,0) as iscomplete, * from Pms_tbl_Addsubtask where ProjectId='" + dsproject.Tables[0].Rows[P]["id"] + "' and Milestoneid='" + ds1.Tables[0].Rows[j]["id"] + "' and task='" + ds.Tables[0].Rows[i]["id"] + "' and id='" + dssubtsk.Tables[0].Rows[sb]["id"] + "' and status=1 and archive=0 ";
                                        DataSet subtaskdss = da.GetDataSetInline(subtaskstr);
                                        if (subtaskdss.Tables[0].Rows.Count > 0)
                                        {
                                            subtaskcheckiscomplete = Convert.ToInt32((subtaskdss.Tables[0].Rows[0]["iscomplete"].ToString()==""?"0": subtaskdss.Tables[0].Rows[0]["iscomplete"].ToString()));
                                            if (subtaskcheckiscomplete == 1)
                                            {
                                                subtaskprojectTotal = Convert.ToInt32(subtaskdss.Tables[0].Rows[0]["iscompletepercentage"].ToString());
                                            }


                                        }

                                        if (subtaskprojectTotal > 0)
                                        {
                                            subtaskWattagecomplite = subtaskprojectTotal;
                                            subtaskWattagepending = Math.Abs(subtaskWattagepending - subtaskWattagecomplite);
                                        }


                                        //   completion  status  end //



                                        //   Issues count start //

                                        float sbtskIssuescomplite = 0;
                                        float sbtskIssuespending = 0;
                                        float sbtskIssuesrojectTotal = 0;
                                        float sbtskIssuesPhaseTotal = 0;
                                        float sbttskIssues_Total = 0;

                                        string sbtskstr111 = "select COUNT(*) as countIssues from Pms_tbl_Addissues where ProjectId='" + dsproject.Tables[0].Rows[P]["id"].ToString() + "' and Milestoneid='" + ds1.Tables[0].Rows[j]["id"].ToString() + "' and  taskid='" + ds.Tables[0].Rows[i]["id"].ToString() + "' and  subtaskid='" + dssubtsk.Tables[0].Rows[sb]["id"] + "' and Issueon='subtask' and status=1 and archive=0 ";
                                        DataSet sbtskdss111 = da.GetDataSetInline(sbtskstr111);
                                        if (sbtskdss111.Tables[0].Rows.Count > 0)
                                        {

                                            try
                                            {
                                                sbtskIssuesPhaseTotal = Convert.ToInt32(sbtskdss111.Tables[0].Rows[0]["countIssues"]);
                                            }
                                            catch (Exception)
                                            {
                                            }
                                        }


                                        string sbtskfindtask111 = "select COUNT(*) as count_Issues from Pms_tbl_Addissues where ProjectId='" + dsproject.Tables[0].Rows[P]["id"].ToString() + "' and Milestoneid='" + ds1.Tables[0].Rows[j]["id"].ToString() + "' and  taskid='" + ds.Tables[0].Rows[i]["id"].ToString() + "' and  subtaskid='" + dssubtsk.Tables[0].Rows[sb]["id"] + "' and Issueon='subtask' and IssueStatus in ( select id from Pms_tbl_Projectstatus where  status=1 and archive=0 and isnull(iscomplete,0)=1)  and status=1 and archive=0";
                                        DataSet sbtskdsssumtask111 = da.GetDataSetInline(sbtskfindtask111);
                                        if (sbtskdsssumtask111.Tables[0].Rows.Count > 0)
                                        {
                                            try
                                            {
                                                sbttskIssues_Total = Convert.ToInt32(sbtskdsssumtask111.Tables[0].Rows[0]["count_Issues"]);
                                            }
                                            catch (Exception)
                                            {

                                            }
                                        }


                                        sbtskIssuesrojectTotal = (sbtskIssuesPhaseTotal - sbttskIssues_Total);


                                        if (sbtskIssuesrojectTotal > 0)
                                        {
                                            sbtskIssuescomplite = sbttskIssues_Total;
                                            sbtskIssuespending = Math.Abs(sbtskIssuesPhaseTotal - sbttskIssues_Total);
                                        }

                                        if (sbtskIssuesrojectTotal == 0)
                                        {
                                            sbtskIssuescomplite = sbttskIssues_Total;
                                            sbtskIssuespending = Math.Abs(sbtskIssuesPhaseTotal - sbttskIssues_Total);
                                        }

                                        // Issues count end //



                                        taskdata += "<tr id='tr_" + ds.Tables[0].Rows[i]["id"].ToString() + "' class='subtaskshow_" + ds.Tables[0].Rows[i]["id"].ToString() + "' style='display:none;'>";

                                        //sr no

                                        taskdata += "<td valign='top'>";
                                        taskdata += "";
                                        taskdata += "</td>";

                                        taskdata += " <td><a href='javascript:void(0);' Class='btn btn-info btn-sm' type='button' onclick='Binddetailssubtask(" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + ")' data-bs-toggle='offcanvas' data-bs-target='#addsubtask'><i class='ri-edit-line'></i></a>";
                                        taskdata += "&nbsp;&nbsp;<a href='javascript:void(0);' Class='btn btn-danger btn-sm' type='button' onclick='deletedatasubtask(\"" + dsproject.Tables[0].Rows[P]["id"].ToString() + "\",\"" + ds1.Tables[0].Rows[j]["id"].ToString() + "\",\"" + ds.Tables[0].Rows[i]["id"].ToString() + "\",\"" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "\",this)'><i class='ri-delete-bin-5-line'></i></a>";
                                        taskdata += "</td>";


                                        //subtask name
                                        taskdata += "<td valign='top'><div class='d-flex'><div class='flex-grow-1'>";

                                        //taskdata += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + (j + 1) + "." + (i + 1) + "." + (sb + 1) + "  " + dssubtsk.Tables[0].Rows[sb]["Subtaskname"].ToString();
                                        string txtval3 = dssubtsk.Tables[0].Rows[sb]["Subtaskname"].ToString();
                                        if (txtval3.Trim() == "")
                                        {
                                            txtval3 = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                                        }
                                        taskdata += "<span  style='font-size:12px;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; " + (j + 1) + "." + (i + 1) + "." + (sb + 1) + " &nbsp;<span id='lblm_" + dsproject.Tables[0].Rows[P]["id"].ToString() + "_" + ds1.Tables[0].Rows[j]["id"].ToString() + "_" + ds.Tables[0].Rows[i]["id"].ToString() + "_" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "_1' onclick='edittextsubtask(this.id)' style='background:#fde796;'> " + txtval3 + "</span></span>";


                                        taskdata += "<input type='text' id='txtm_" + dsproject.Tables[0].Rows[P]["id"].ToString() + "_" + ds1.Tables[0].Rows[j]["id"].ToString() + "_" + ds.Tables[0].Rows[i]["id"].ToString() + "_" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "_1' onblur='updatedetailssubtask(this.id,\"" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "\",\"Subtaskname\");' style='display:none;border:1px solid #ddd;outline:none;' value='" + txtval3 + "'>";

                                        taskdata += "&nbsp;<a href ='javascript:void(0);' role='button' data-bs-toggle='dropdown' aria-expanded='false'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover'>+</span>" +
                                        "<ul class='dropdown-menu  dropdownmenu-secondary'>" +

                                        "<li onclick='addrowUpDownsubtask(\"1\",\"" + i + "\",\"" + dsproject.Tables[0].Rows[P]["id"].ToString() + "\",\"" + ds1.Tables[0].Rows[j]["id"].ToString() + "\",\"" + ds.Tables[0].Rows[i]["id"].ToString() + "\",\"" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "\",\"" + dssubtsk.Tables[0].Rows[sb]["orderno"].ToString() + "\")' style='display:inline-block;'><i class='bi bi-arrow-up-square' data-bs-toggle='' data-bs-placement='top' title='Add Above'></i></li>" +
                                        "<li onclick='addrowUpDownsubtask(\"2\",\"" + i + "\",\"" + dsproject.Tables[0].Rows[P]["id"].ToString() + "\",\"" + ds1.Tables[0].Rows[j]["id"].ToString() + "\",\"" + ds.Tables[0].Rows[i]["id"].ToString() + "\",\"" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "\",\"" + dssubtsk.Tables[0].Rows[sb]["orderno"].ToString() + "\")' style='display:inline-block;margin:0 15px;'><i class='bi-arrow-down-square' data-bs-toggle='' data-bs-placement='top' title='Add Below'></i></li>";

                                        taskdata += "</ul></div>";
                                        if (isownerS == 1||chkadmin==1)
                                        {
                                            taskdata += "<div class='gap-1 flex-shrink-0 align-self-center'><a href='javascript:void(0);' type='button' data-bs-toggle='offcanvas' data-bs-target='#canvas_popup' onclick='loadsrc(\"inner_popup\",\"/pms/AddSTDependency.aspx?id=" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "&tid=" + ds.Tables[0].Rows[i]["id"].ToString() + "&mid=" + ds1.Tables[0].Rows[j]["id"].ToString() + "&pid=" + dsproject.Tables[0].Rows[P]["id"].ToString() + "&mprojectid=" + dsproject.Tables[0].Rows[P]["id"].ToString() + "&type=subtask\")'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Dependency' style='font-size:12px;height:22px;width:22px;margin-left:9px'>D</span></a>&nbsp;&nbsp;&nbsp; <a href='javascript:void(0);' type='button' data-bs-toggle='offcanvas' data-bs-target='#canvas_popup' onclick='loadsrc(\"inner_popup\",\"/pms/AddIssuesPopup.aspx?id=" + dsproject.Tables[0].Rows[P]["id"].ToString() + "&taskid=" + ds.Tables[0].Rows[i]["id"].ToString() + "&milstoneid=" + ds1.Tables[0].Rows[j]["id"].ToString() + "&subtaskid=" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "&tasklistid=0&mprojectid=" + dsproject.Tables[0].Rows[P]["id"].ToString() + "&type=subtask\")'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Issue' style='font-size:12px;height:22px;width:22px;'>I</span></a></div>";
                                        }
                                        
                                        taskdata += "</td>";

                                        //sub task completion status start 
                                        taskdata += "<td>";
                                        if (subtaskWattagecomplite == 0)
                                        {

                                            taskdata += "<div class='progress progress-xl cursor-pointer'>";
                                            taskdata += "<div class='progress-bar bg-warning' data-bs-toggle='tooltip' data-bs-placement='top' title='Pending(100%)' role='progressbar' style='width:100%'><span style='text-align:center;'>100%</span>";
                                            taskdata += "</div>";
                                            taskdata += "</div>";

                                        }

                                        else
                                        {

                                            taskdata += "<div class='progress progress-xl cursor-pointer'>";
                                            taskdata += "<div class='progress-bar bg-success' data-bs-toggle='tooltip' data-bs-placement='top' title='Complete (" + subtaskWattagecomplite + "%)' role='progressbar' style='width:" + subtaskWattagecomplite + "%'>" + subtaskWattagecomplite + "%";
                                            taskdata += "</div>";
                                            taskdata += "<div class='progress-bar bg-warning' data-toggle='tooltip' data-bs-placemen='top' title='Pending (" + subtaskWattagepending + "%)' role='progressbar' style='width:" + subtaskWattagepending + "%'>" + subtaskWattagepending + "%";
                                            taskdata += "</div>";
                                            taskdata += "</div>";

                                        }

                                        taskdata += "</td>";

                                        // Weightage 
                                        taskdata += "<td valign='top'>";

                                        //taskdata += fns.ToFormattedCurrencyString(dssubtsk.Tables[0].Rows[sb]["Weightage"].ToString());
                                        string wt_subtask= fns.ToFormattedCurrencyString(dssubtsk.Tables[0].Rows[sb]["Weightage"].ToString());
                                        if(wt_subtask.Trim()=="")
                                        {
                                            wt_subtask = "0";
                                        }
                                        taskdata += "<span id='lblm_" + dsproject.Tables[0].Rows[P]["id"].ToString() + "_" + ds1.Tables[0].Rows[j]["id"].ToString() + "_" + ds.Tables[0].Rows[i]["id"].ToString() + "_" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "_3' onclick='edittextsubtask(this.id)' style='background:#fde796;'> " + wt_subtask + "</span>";


                                        taskdata += "<input type='text' id='txtm_" + dsproject.Tables[0].Rows[P]["id"].ToString() + "_" + ds1.Tables[0].Rows[j]["id"].ToString() + "_" + ds.Tables[0].Rows[i]["id"].ToString() + "_" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "_3' onblur='updatedetailssubtask(this.id,\"" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "\",\"Weightage\");' style='display:none;border:1px solid #ddd;outline:none;' value='" + fns.ToFormattedCurrencyString(dssubtsk.Tables[0].Rows[sb]["Weightage"].ToString()) + "' onkeypress='return isnumeric(event);'>";


                                        taskdata += "</td>";


                                        //Owner

                                        taskdata += "<td>";
                                        taskdata += "<div class='d-flex'>";
                                        taskdata += "<div class='flex-grow-1'>";
                                        taskdata += "<div class='avatar-group' id='sbtsklblowner_" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "'>";
                                        taskdata += sbtskusersdetailsowner;

                                        taskdata += "<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='" + sbtskownertooltip + "' data-bs-original-title='more'>";

                                        if (sbtskcntownr != "" && sbtskcntownr != "0")
                                        {
                                            taskdata += "<div class='avatar-xxs'>";
                                            taskdata += "<div class='avatar-title rounded-circle'>";

                                            taskdata += (sbtskcntownr + "+");
                                            taskdata += "</div>";
                                            taskdata += "</div>";
                                        }

                                        taskdata += "</a>";
                                        taskdata += "</div>";
                                        taskdata += "</div>";
                                        taskdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                                        taskdata += "<a href ='javascript:void(0);' type='button' id='sbtskbtnownr_" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "' onclick='sbtskBinddetails_Owner(" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#sbtskchangeowner'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Owner'>+</span></a>";
                                        taskdata += "</div>";
                                        taskdata += "</div>";
                                        taskdata += "</td>";


                                        //team
                                        taskdata += "<td>";
                                        taskdata += "<div class='d-flex'>";
                                        taskdata += "<div class='flex-grow-1'>";
                                        taskdata += "<div class='avatar-group' id='sbtsklbluser_" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "'>";
                                        taskdata += sbtskusersdetails;

                                        taskdata += "<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='" + sbtskteamtooltip + "' data-bs-original-title='more'>";

                                        if (sbtskcnt != "" && sbtskcnt != "0")
                                        {
                                            taskdata += "<div class='avatar-xxs'>";
                                            taskdata += "<div class='avatar-title rounded-circle'>";

                                            taskdata += (sbtskcnt + "+");
                                            taskdata += "</div>";
                                            taskdata += "</div>";
                                        }

                                        taskdata += "</a>";
                                        taskdata += "</div>";
                                        taskdata += "</div>";
                                        taskdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                                        taskdata += "<a href ='javascript:void(0);' type='button' id='sbtskbtnteam_" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "' onclick='sbtskBinddetails_Team(" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#sbtskaddteam'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Team'>+</span></a>";
                                        taskdata += "</div>";
                                        taskdata += "</div>";
                                        taskdata += "</td>";
                                        //Issues
                                        taskdata += "<td valign='top'>";
                                        float sbtskcompletetepercentissues = 0;
                                        float sbtskPendingepercentissues = 0;
                                        sbtskcompletetepercentissues = sbtskIssuescomplite * 100 / sbtskIssuesPhaseTotal;
                                        Pendingepercentissues = sbtskIssuespending * 100 / sbtskIssuesPhaseTotal;

                                        taskdata += "<div class='d-flex'>";
                                        taskdata += "<div class='flex-grow-1 align-self-center'>";
                                        taskdata += "<div class='progress progress-xl cursor-pointer'>";
                                        taskdata += "<div class='progress-bar bg-success' role='progressbar' style='width: " + fns.ToFormattedCurrencyString(sbtskcompletetepercentissues.ToString()) + "%' aria-valuemin='0' aria-valuemax='100' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + fns.ToFormattedCurrencyString(sbtskcompletetepercentissues.ToString()) + "% [" + sbtskIssuescomplite + " of " + sbtskIssuesPhaseTotal + " Issues Completed]'>" + sbtskIssuescomplite + "</div>";
                                        taskdata += "<div class='progress-bar bg-warning' role='progressbar' style='width: " + fns.ToFormattedCurrencyString(sbtskPendingepercentissues.ToString()) + "%' aria-valuemin='0' aria-valuemax='100' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + fns.ToFormattedCurrencyString(sbtskPendingepercentissues.ToString()) + "% [" + sbtskIssuespending + " of " + sbtskIssuesPhaseTotal + " Issues Not Completed]'>" + sbtskIssuespending + "</div>";
                                        taskdata += "</div>";
                                        taskdata += "</div>";
                                        taskdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                                        taskdata += "<a href ='javascript:void(0);' type='button' data-bs-toggle='offcanvas' data-bs-target=#canvas_popup onclick=loadsrc('inner_popup','/pms/AddIssuesPopup.aspx?id=" + dsproject.Tables[0].Rows[P]["id"].ToString() + "&milstoneid=" + ds1.Tables[0].Rows[j]["id"].ToString() + "&tasklistid=" + dssubtsk.Tables[0].Rows[sb]["tasklist"].ToString() + "&taskid=" + ds.Tables[0].Rows[i]["id"].ToString() + "&subtaskid=" + dssubtsk.Tables[0].Rows[sb]["id"] + "&type=subtask\")><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Issue'>+</span></a>";
                                        taskdata += "</div>";
                                        taskdata += "</td>";
                                        //Timeline

                                        //Storing input Dates  
                                        DateTime sbtskStartDate = Convert.ToDateTime(dssubtsk.Tables[0].Rows[sb]["StartDate"].ToString() == "" ? DateTime.Now.ToString() : dssubtsk.Tables[0].Rows[sb]["StartDate"].ToString());
                                        DateTime sbtskTargetdate = Convert.ToDateTime(dssubtsk.Tables[0].Rows[sb]["Targetdate"].ToString() == "" ? DateTime.Now.ToString() : dssubtsk.Tables[0].Rows[sb]["Targetdate"].ToString());
                                        TimeSpan sbtskobjTimeSpan = sbtskTargetdate - sbtskStartDate;
                                        double sbtskDays = Convert.ToDouble(sbtskobjTimeSpan.TotalDays);
                                        sbtskDays = sbtskDays + 1;

                                        DateTime sbtskEnddate = Convert.ToDateTime(dssubtsk.Tables[0].Rows[sb]["Targetdate"].ToString() == "" ? DateTime.Now.ToString() : dssubtsk.Tables[0].Rows[sb]["Targetdate"].ToString());
                                        DateTime sbtskCurrentdate = DateTime.Now;
                                        TimeSpan sbtskobjtimeleftspn = sbtskEnddate - sbtskCurrentdate;
                                        double sbtskleftDays = Convert.ToDouble(sbtskobjtimeleftspn.Days);
                                        sbtskleftDays = sbtskleftDays + 1;

                                        string sbtskleftdlydys = "";
                                        if (sbtskleftDays < 0)
                                        {
                                            sbtskleftdlydys = "days delayed";
                                        }
                                        else
                                        {
                                            sbtskleftdlydys = "days left";
                                        }


                                        //start days months  
                                        string sbtskstartmonthname = "";
                                        int sbtskststartdys = sbtskStartDate.Day;
                                        int sbtskstrtmonth = sbtskStartDate.Month;
                                        int subtaskstartyear = sbtskStartDate.Year;
                                       
                                        sbtskstartmonthname = getmonthname(sbtskstrtmonth);


                                        //end days months 
                                        string sbtskendmonthname = "";
                                        int sbtskenddys = sbtskTargetdate.Day;
                                        int sbtskendmonth = sbtskTargetdate.Month;
                                        int subtaskEndyear = sbtskTargetdate.Year;
                                       
                                        sbtskendmonthname=getmonthname(sbtskendmonth);




                                        taskdata += "<td>";
                                        taskdata += "<a href = 'javascript:void(0);' class='timesheet-wrap' onclick='addeventdateselect(\"" + dsproject.Tables[0].Rows[P]["id"].ToString() + "\",\"" + ds1.Tables[0].Rows[j]["id"].ToString() + "\",\"" + ds.Tables[0].Rows[i]["id"].ToString() + "\",\"" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "\",\"st\",this);'>";
                                        taskdata += "<div id='title1_" + dsproject.Tables[0].Rows[P]["id"].ToString() + "_" + ds1.Tables[0].Rows[j]["id"].ToString() + "_" + ds.Tables[0].Rows[i]["id"].ToString() + "_" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "' class='progress progress-xl bg-dark position-relative' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + sbtskleftDays.ToString().Replace("-", "") + " " + sbtskleftdlydys + "'>";
                                        taskdata += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 100%;' aria-valuemax='100'></div>";
                                        taskdata += "<div class='dayswrapper'>";
                                        taskdata += "<span class='fromtodate' id='spn1_" + dsproject.Tables[0].Rows[P]["id"].ToString() + "_" + ds1.Tables[0].Rows[j]["id"].ToString() + "_" + ds.Tables[0].Rows[i]["id"].ToString() + "_" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "'>" + sbtskststartdys + " " + sbtskstartmonthname + " " + subtaskstartyear + " - " + sbtskenddys + " " + sbtskendmonthname + " " + subtaskEndyear + "</span>";
                                        taskdata += "<span class='totaldays' id='spn2_" + dsproject.Tables[0].Rows[P]["id"].ToString() + "_" + ds1.Tables[0].Rows[j]["id"].ToString() + "_" + ds.Tables[0].Rows[i]["id"].ToString() + "_" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "'>" + sbtskDays + " days</span>";
                                        taskdata += "</div>";
                                        taskdata += "</div>";
                                        taskdata += "</a>";
                                        taskdata += "</td>";





                                        //Sub Task extended timeleine 

                                        string subtask_estendleftdlydys = "";
                                        string subTask_estendstartmonthname = "";
                                        string subtask_estendendmonthname = "";
                                        double subtask_estendleftDays = 0;
                                        int subtask_estendststartdys = 0;
                                        int subtask_estendenddys = 0;
                                        double subtask_estendDays = 0;
                                        string subtask_extenddatebind = "";
                                        string subtask_strextend = "select id,extendStartdate as StartDate,extendEnddate as TargetDate from Pms_Tbl_Extended_Timeline where  status=1 and Type='subtask' and archive=0  and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and subtaskid = '" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "'  ";
                                        DataSet subtask_dsstrextend = da.GetDataSetInline(subtask_strextend);
                                        if (subtask_dsstrextend.Tables[0].Rows.Count > 0)
                                        {
                                            for (int ST = 0; ST < subtask_dsstrextend.Tables[0].Rows.Count; ST++)
                                            {
                                                DateTime subtask_estendStartDate = Convert.ToDateTime(subtask_dsstrextend.Tables[0].Rows[ST]["StartDate"].ToString() == "" ? DateTime.Now.ToString() : subtask_dsstrextend.Tables[0].Rows[ST]["StartDate"].ToString());
                                                DateTime subtask_estendTargetdate = Convert.ToDateTime(subtask_dsstrextend.Tables[0].Rows[ST]["TargetDate"].ToString() == "" ? DateTime.Now.ToString() : subtask_dsstrextend.Tables[0].Rows[ST]["TargetDate"].ToString());
                                                TimeSpan subtask_estendobjTimeSpan = subtask_estendTargetdate - subtask_estendStartDate;
                                                subtask_estendDays = Convert.ToDouble(subtask_estendobjTimeSpan.TotalDays);
                                                subtask_estendDays = subtask_estendDays + 1;


                                                DateTime subtask_estendEnddate = Convert.ToDateTime(subtask_dsstrextend.Tables[0].Rows[ST]["Targetdate"].ToString() == "" ? DateTime.Now.ToString() : subtask_dsstrextend.Tables[0].Rows[ST]["Targetdate"].ToString());
                                                DateTime subtask_estendCurrentdate = DateTime.Now;
                                                TimeSpan subtask_estendobjtimeleftspn = subtask_estendEnddate - subtask_estendCurrentdate;
                                                subtask_estendleftDays = Convert.ToDouble(subtask_estendobjtimeleftspn.Days);
                                                subtask_estendleftDays = subtask_estendleftDays + 1;


                                                if (subtask_estendleftDays < 0)
                                                {
                                                    subtask_estendleftdlydys = "days delayed";
                                                }
                                                else
                                                {
                                                    subtask_estendleftdlydys = "days left";
                                                }

                                                //start days months  

                                                subtask_estendststartdys = subtask_estendCurrentdate.Day;
                                                int subtask_estendstrtmonth = subtask_estendCurrentdate.Month;
                                               
                                                    subTask_estendstartmonthname = getmonthname(subtask_estendstrtmonth);


                                                    //end days months 

                                                    subtask_estendenddys = subtask_estendTargetdate.Day;
                                                int subtask_estendendmonth = subtask_estendTargetdate.Month;
                                                
                                                 subtask_estendendmonthname = getmonthname(subtask_estendendmonth);
                                               



                                                subtask_extenddatebind += "<a href = 'javascript:void(0);' class='timesheet-wrap' >";
                                                subtask_extenddatebind += "<div class='progress progress-xl bg-dark position-relative me-1' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + subtask_estendststartdys + " " + subTask_estendstartmonthname + " - " + subtask_estendenddys + " " + subtask_estendendmonthname + " (" + subtask_estendleftDays.ToString().Replace("-", "") + " " + subtask_estendleftdlydys + ")'>";
                                                subtask_extenddatebind += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 100%;' aria-valuemax='100'>" + subtask_estendDays + " days</div>";
                                                subtask_extenddatebind += "</div>";
                                                subtask_extenddatebind += "</a>";



                                            }

                                        }




                                        taskdata += "<td style='display:none'>";
                                        taskdata += "<div class='d-flex'>";
                                        taskdata += "<div class='flex-grow-1 align-self-center me-2'>";
                                        taskdata += "<div class='extended_time' id='lblExtnddtsubtask_" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "'>";

                                        taskdata += "" + subtask_extenddatebind + "";

                                        taskdata += "</div>";
                                        taskdata += "</div>";

                                        if (leftDays < 0)
                                        {
                                            taskdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                                            taskdata += "<a href ='javascript:void(0);' type='button' id='btnextndtsubtask_" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "'  onclick='Bindextenddatesubtask_Details(" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#ExtendedTimline_subtask'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Extended Day'>+</span></a>";
                                            taskdata += "</div>";
                                        }

                                        taskdata += "</div>";
                                        taskdata += "</div>";
                                        taskdata += "</td>";


                                        // start date
                                        //taskdata += "<td valign='top'>";
                                        //taskdata += cmf.dateConvert(dssubtsk.Tables[0].Rows[sb]["Startdate"].ToString());
                                        //taskdata += "</td>";

                                        //End date
                                        //taskdata += "<td valign='top'>";
                                        //taskdata += cmf.dateConvert(dssubtsk.Tables[0].Rows[sb]["Targetdate"].ToString());
                                        //taskdata += "</td>";


                                        //status
                                        string sbtskstrvalcolor = get_Status_Color(dssubtsk.Tables[0].Rows[sb]["P_Status"].ToString());
                                        taskdata += "<td>";
                                        taskdata += "<div class='btn-group d-block'>";
                                        taskdata += "<a href ='javascript:void(0);' id='btnsts_" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "' type='button' onclick=setsubtaskstustid('" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "',this.id) data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + sbtskstrvalcolor + "'>" + get_Projectstatus(dssubtsk.Tables[0].Rows[sb]["P_Status"].ToString()) + "</a>";
                                        taskdata += "<div class='dropdown-menu lead_form_body p-0'>";
                                        taskdata += sbtskretval;
                                        taskdata += "</div>";
                                        taskdata += "</div>";
                                        taskdata += "</td>";


                                        //priority
                                        string sbtskstrprioritycolor = get_PriorityColor(dssubtsk.Tables[0].Rows[sb]["Priority"].ToString());
                                        taskdata += "<td>";
                                        taskdata += "<div class='btn-group d-block'>";
                                        taskdata += "<a href ='javascript:void(0);' id='btnpri_" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "' type='button' onclick=sbtsksetpriorityid('" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "',this.id) data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + sbtskstrprioritycolor + "'>" + get_PriorityName(dssubtsk.Tables[0].Rows[sb]["Priority"].ToString()) + "</a>";
                                        taskdata += "<div class='dropdown-menu lead_form_body p-0'>";
                                        taskdata += sbtskstrvalprority;
                                        taskdata += "</div>";
                                        taskdata += "</div>";
                                        taskdata += "</td>";

                                       


                                        // sub task duration hour
                                        taskdata += "<td valign='top'>";
                                        //taskdata += dssubtsk.Tables[0].Rows[sb]["TaskDuration"].ToString();
                                        string wt_subtaskdusration = dssubtsk.Tables[0].Rows[sb]["TaskDuration"].ToString();
                                        if (wt_subtaskdusration.Trim() == "")
                                        {
                                            wt_subtaskdusration = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                                        }
                                        taskdata += "<span id='lblm_" + dsproject.Tables[0].Rows[P]["id"].ToString() + "_" + ds1.Tables[0].Rows[j]["id"].ToString() + "_" + ds.Tables[0].Rows[i]["id"].ToString() + "_" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "_13' onclick='edittextsubtask(this.id)' style='background:#fde796;'> " + wt_subtaskdusration + "</span>";


                                        taskdata += "<input type='text' id='txtm_" + dsproject.Tables[0].Rows[P]["id"].ToString() + "_" + ds1.Tables[0].Rows[j]["id"].ToString() + "_" + ds.Tables[0].Rows[i]["id"].ToString() + "_" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "_13' onblur='updatedetailssubtask(this.id,\"" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "\",\"TaskDuration\");' style='display:none;border:1px solid #ddd;outline:none;' value='" + dssubtsk.Tables[0].Rows[sb]["TaskDuration"].ToString() + "'>";

                                        taskdata += "</td>";


                                        // Stage
                                        string sbtskstrvalstagecolor = get_Stage_Color(dssubtsk.Tables[0].Rows[sb]["stage"].ToString());
                                        taskdata += "<td>";
                                        taskdata += "<div class='btn-group d-block'>";
                                        taskdata += "<a href ='javascript:void(0);' id='btnStage_" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "' type='button' onclick=sbtsksetStageid('" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "',this.id) data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + sbtskstrvalstagecolor + "'>" + get_Stagename(dssubtsk.Tables[0].Rows[sb]["stage"].ToString()) + "</a>";
                                        taskdata += "<div class='dropdown-menu lead_form_body p-0'>";
                                        taskdata += sbtskretstageval;
                                        taskdata += "</div>";
                                        taskdata += "</div>";
                                        taskdata += "</td>";


                                        //sub task Description //
                                        taskdata += "<td valign='top'>";
                                        string strdescriptionS = dssubtsk.Tables[0].Rows[sb]["SubTaskDescription"].ToString();
                                        if (strdescriptionS == "")
                                        {
                                            strdescriptionS = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                                        }
                                        taskdata += "<span id='lblm_" + dsproject.Tables[0].Rows[P]["id"].ToString() + '_' + ds1.Tables[0].Rows[j]["id"].ToString() + "_" + ds.Tables[0].Rows[i]["id"].ToString() + "_" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "_15' onclick='edittextsubtask(this.id)' style='background:#fde796;'> " + strdescriptionS + "</span>";

                                        taskdata += "<input type='text' id='txtm_" + dsproject.Tables[0].Rows[P]["id"].ToString() + '_' + ds1.Tables[0].Rows[j]["id"].ToString() + "_" + ds.Tables[0].Rows[i]["id"].ToString() + "_" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "_15' onblur='updatedetailssubtask(this.id,\"" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "\",\"SubTaskDescription\");' style='display:none;border:1px solid #ddd;outline:none;' value='" + dssubtsk.Tables[0].Rows[sb]["SubTaskDescription"].ToString() + "' >";

                                        taskdata += "</td>";






                                        taskdata += "</tr>";
                                    }





                                }





                            }

                            


                        }

                        #endregion

                    }


                }

            }



        }

        taskdata += "</tbody>";
        taskdata += "</table>";
        SaveLogText("taskdata-"+ taskdata);
        return taskdata;
    }
    public static string getmonthname(int mid)
    {
        string retval = "";
        if (mid == 1)
        {
            retval = "Jan";
        }
        if (mid == 2)
        {
            retval = "Feb";
        }
        if (mid == 3)
        {
            retval = "Mar";
        }

        if (mid == 4)
        {
            retval = "Apr";
        }

        if (mid == 5)
        {
            retval = "May";
        }

        if (mid == 6)
        {
            retval = "Jun";
        }

        if (mid == 7)
        {
            retval = "Jul";
        }

        if (mid == 8)
        {
            retval = "Aug";
        }

        if (mid == 9)
        {
            retval = "Sep";
        }

        if (mid == 10)
        {
            retval = "Oct";
        }

        if (mid == 11)
        {
            retval = "Nov";
        }

        if (mid == 12)
        {
            retval = "Dec";
        }
        return retval;
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

    public static string get_employee(string ownerid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from tbl_employee where status=1 and archive=0 and id='" + ownerid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["DisplayName"].ToString();

        }

        return strret;

    }


    public static string get_ProjectName(string Projectid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_AddProject where status=1 and archive=0 and id='" + Projectid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["ProjectName"].ToString();

        }

        return strret;

    }

    public static string get_Milstonename(string milestoneid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_AddMilestone where status=1 and archive=0 and id='" + milestoneid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["Milestone"].ToString();

        }

        return strret;

    }
    public static string get_Tasklistname(string tasklistid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_Addtasklist where status=1 and archive=0 and id='" + tasklistid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["TaskList"].ToString();

        }

        return strret;

    }


    public static string get_Countryname(string countryid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from tbl_countrymaster where status=1 and archive=0 and id='" + countryid + "'  ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["countryname"].ToString();

        }

        return strret;
    }

    protected static string get_team(string str1)
    {
        DataAccess da = new DataAccess();
        string str = str1;
        string[] splitstr = str.Split(',');
        string stname = "";
        for (int i = 0; i < splitstr.Length; i++)
        {
            string strsql = "select * from tbl_employee where id = '" + splitstr[i] + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0";
            DataSet ds2 = da.GetDataSetInline(strsql);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                if (ds2.Tables[0].Rows[0]["DisplayName"].ToString() != string.Empty)
                {
                    if (stname != string.Empty)
                    {
                        stname += "," + ds2.Tables[0].Rows[0]["DisplayName"].ToString();
                    }
                    else
                    {
                        stname += ds2.Tables[0].Rows[0]["DisplayName"].ToString();
                    }
                }

            }
        }
        return stname;
    }





    protected static string get_status_listbind()
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = "select * from Pms_tbl_Projectstatus where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret += "<ul class='sttus-list' data-simplebar style='max-height: 160px'>";
            for (int i = 0; i < dscode.Tables[0].Rows.Count; i++)
            {

                strret += "<li>" +
                  "<div class='dropdown-item p-0'>" +
                    "<a class='' href='javascript:void(0);' onclick = 'UpdateDataStatusdeatils(" + dscode.Tables[0].Rows[i]["id"].ToString() + "," + (dscode.Tables[0].Rows[i]["iscomplete"].ToString()==""?"0": dscode.Tables[0].Rows[i]["iscomplete"].ToString()) + ")' > " +
                    "<i class='bi bi-square-fill' style='color:" + dscode.Tables[0].Rows[i]["PrintColor"].ToString() + "' ></i> " + dscode.Tables[0].Rows[i]["ProjectStatus"].ToString() + "" +
                    "</a>" +
                    "</div>" +
                    "</li>";


            }
            strret += "</ul>";
        }

        return strret;

    }
    protected static string Projget_status_listbind()
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = "select * from Pms_tbl_Projectstatus where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret += "<ul class='sttus-list' data-simplebar style='max-height: 160px'>";
            for (int i = 0; i < dscode.Tables[0].Rows.Count; i++)
            {

                strret += "<li>" +
                  "<div class='dropdown-item p-0'>" +
                    "<a class='' href='javascript:void(0);' onclick = 'ProjUpdateDataStatusdeatils(" + dscode.Tables[0].Rows[i]["id"].ToString() + "," + (dscode.Tables[0].Rows[i]["iscomplete"].ToString()==""?"0": dscode.Tables[0].Rows[i]["iscomplete"].ToString()) + ")' > " +
                    "<i class='bi bi-square-fill' style='color:" + dscode.Tables[0].Rows[i]["PrintColor"].ToString() + "' ></i> " + dscode.Tables[0].Rows[i]["ProjectStatus"].ToString() + "" +
                    "</a>" +
                    "</div>" +
                    "</li>";


            }
            strret += "</ul>";
        }

        return strret;

    }

    protected static string Mileget_status_listbind()
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = "select * from Pms_tbl_Projectstatus where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret += "<ul class='sttus-list' data-simplebar style='max-height: 160px'>";
            for (int i = 0; i < dscode.Tables[0].Rows.Count; i++)
            {

                strret += "<li>" +
                  "<div class='dropdown-item p-0'>" +
                    "<a class='' href='javascript:void(0);' onclick = 'MileUpdateDataStatusdeatils(" + dscode.Tables[0].Rows[i]["id"].ToString() + "," + (dscode.Tables[0].Rows[i]["iscomplete"].ToString()==""?"0": dscode.Tables[0].Rows[i]["iscomplete"].ToString()) + ")' > " +
                    "<i class='bi bi-square-fill' style='color:" + dscode.Tables[0].Rows[i]["PrintColor"].ToString() + "' ></i> " + dscode.Tables[0].Rows[i]["ProjectStatus"].ToString() + "" +
                    "</a>" +
                    "</div>" +
                    "</li>";


            }
            strret += "</ul>";
        }

        return strret;

    }

    protected static string get_Stage_listbind()
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = "select * from Pms_tbl_Stage where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret += "<ul class='sttus-list' data-simplebar style='max-height: 160px'>";
            for (int i = 0; i < dscode.Tables[0].Rows.Count; i++)
            {

                strret += "<li>" +
                  "<div class='dropdown-item p-0'>" +
                    "<a class='' href='javascript:void(0);' onclick = 'UpdateDataStagedeatils(" + dscode.Tables[0].Rows[i]["id"].ToString() + ")' > " +
                    "<i class='bi bi-square-fill' style='color:" + dscode.Tables[0].Rows[i]["PrintColor"].ToString() + "' ></i> " + dscode.Tables[0].Rows[i]["name"].ToString() + "" +
                    "</a>" +
                    "</div>" +
                    "</li>";


            }
            strret += "</ul>";
        }

        return strret;

    }

    protected static string sbtskget_status_listbind()
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = "select * from Pms_tbl_Projectstatus where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret += "<ul class='sttus-list' data-simplebar style='max-height: 160px'>";
            for (int i = 0; i < dscode.Tables[0].Rows.Count; i++)
            {

                strret += "<li>" +
                  "<div class='dropdown-item p-0'>" +
                    "<a class='' href='javascript:void(0);' onclick = 'sbtskUpdateDataStatusdeatils(" + dscode.Tables[0].Rows[i]["id"].ToString() + " ," + (dscode.Tables[0].Rows[i]["iscomplete"].ToString()==""?"0": dscode.Tables[0].Rows[i]["iscomplete"].ToString()) + ")' > " +
                    "<i class='bi bi-square-fill' style='color:" + dscode.Tables[0].Rows[i]["PrintColor"].ToString() + "' ></i> " + dscode.Tables[0].Rows[i]["ProjectStatus"].ToString() + "" +
                    "</a>" +
                    "</div>" +
                    "</li>";


            }
            strret += "</ul>";
        }

        return strret;

    }

    protected static string sbtskget_Stage_listbind()
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = "select * from Pms_tbl_Stage where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret += "<ul class='sttus-list' data-simplebar style='max-height: 160px'>";
            for (int i = 0; i < dscode.Tables[0].Rows.Count; i++)
            {

                strret += "<li>" +
                  "<div class='dropdown-item p-0'>" +
                    "<a class='' href='javascript:void(0);' onclick = 'sbtskUpdateDataStagedeatils(" + dscode.Tables[0].Rows[i]["id"].ToString() + ")' > " +
                    "<i class='bi bi-square-fill' style='color:" + dscode.Tables[0].Rows[i]["PrintColor"].ToString() + "' ></i> " + dscode.Tables[0].Rows[i]["name"].ToString() + "" +
                    "</a>" +
                    "</div>" +
                    "</li>";


            }
            strret += "</ul>";
        }

        return strret;

    }
    protected static string sbtskget_Priority_listbind()
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = "select * from Pms_tbl_Priority where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret += "<ul class='sttus-list' data-simplebar style='max-height: 160px'>";
            for (int i = 0; i < dscode.Tables[0].Rows.Count; i++)
            {

                strret += "<li>" +
                  "<div class='dropdown-item p-0'>" +
                    "<a class='' href='javascript:void(0);' onclick = 'sbtskUpdateDatapropertydeatils(" + dscode.Tables[0].Rows[i]["id"].ToString() + ")' > " +
                    "<i class='bi bi-square-fill' style='color:" + dscode.Tables[0].Rows[i]["PrintColor"].ToString() + "' ></i> " + dscode.Tables[0].Rows[i]["name"].ToString() + "" +
                    "</a>" +
                    "</div>" +
                    "</li>";


            }
            strret += "</ul>";
        }

        return strret;


    }



    protected static string get_depdencytaskname(string str1)
    {
        DataAccess da = new DataAccess();
        string str = str1;
        string[] splitstr = str.Split(',');
        string stname = "";
        for (int i = 0; i < splitstr.Length; i++)
        {
            string strsql = "select * from Pms_tbl_Addtask where id = '" + splitstr[i] + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0";
            DataSet ds2 = da.GetDataSetInline(strsql);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                if (ds2.Tables[0].Rows[0]["taskname"].ToString() != string.Empty)
                {
                    if (stname != string.Empty)
                    {
                        stname += "," + ds2.Tables[0].Rows[0]["taskname"].ToString();
                    }
                    else
                    {
                        stname += ds2.Tables[0].Rows[0]["taskname"].ToString();
                    }
                }

            }
        }
        return stname;
    }

    protected static string get_Priority_listbind()
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = "select * from Pms_tbl_Priority where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret += "<ul class='sttus-list' data-simplebar style='max-height: 160px'>";
            for (int i = 0; i < dscode.Tables[0].Rows.Count; i++)
            {

                strret += "<li>" +
                  "<div class='dropdown-item p-0'>" +
                    "<a class='' href='javascript:void(0);' onclick = 'UpdateDatapropertydeatils(" + dscode.Tables[0].Rows[i]["id"].ToString() + ")' > " +
                    "<i class='bi bi-square-fill' style='color:" + dscode.Tables[0].Rows[i]["PrintColor"].ToString() + "' ></i> " + dscode.Tables[0].Rows[i]["name"].ToString() + "" +
                    "</a>" +
                    "</div>" +
                    "</li>";


            }
            strret += "</ul>";
        }

        return strret;


    }

    protected static string Mileget_Priority_listbind()
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = "select * from Pms_tbl_Priority where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret += "<ul class='sttus-list' data-simplebar style='max-height: 160px'>";
            for (int i = 0; i < dscode.Tables[0].Rows.Count; i++)
            {

                strret += "<li>" +
                  "<div class='dropdown-item p-0'>" +
                    "<a class='' href='javascript:void(0);' onclick = 'MileUpdateDatapropertydeatils(" + dscode.Tables[0].Rows[i]["id"].ToString() + ")' > " +
                    "<i class='bi bi-square-fill' style='color:" + dscode.Tables[0].Rows[i]["PrintColor"].ToString() + "' ></i> " + dscode.Tables[0].Rows[i]["name"].ToString() + "" +
                    "</a>" +
                    "</div>" +
                    "</li>";


            }
            strret += "</ul>";
        }

        return strret;


    }
    protected static string Projget_Priority_listbind()
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = "select * from Pms_tbl_Priority where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret += "<ul class='sttus-list' data-simplebar style='max-height: 160px'>";
            for (int i = 0; i < dscode.Tables[0].Rows.Count; i++)
            {

                strret += "<li>" +
                  "<div class='dropdown-item p-0'>" +
                    "<a class='' href='javascript:void(0);' onclick = 'ProjUpdateDatapropertydeatils(" + dscode.Tables[0].Rows[i]["id"].ToString() + ")' > " +
                    "<i class='bi bi-square-fill' style='color:" + dscode.Tables[0].Rows[i]["PrintColor"].ToString() + "' ></i> " + dscode.Tables[0].Rows[i]["name"].ToString() + "" +
                    "</a>" +
                    "</div>" +
                    "</li>";


            }
            strret += "</ul>";
        }

        return strret;


    }

    protected static string get_Projectstatus(string projectid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_Projectstatus where status=1 and archive=0 and id='" + projectid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString();

        }
        else
        {
            strret = "-";
        }

        return strret;

    }

    protected static string get_Stagename(string stageid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_Stage where status=1 and archive=0 and id='" + stageid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["name"].ToString();

        }

        return strret;

    }

    protected static string get_Status_Color(string projectid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_Projectstatus where status=1 and archive=0 and id='" + projectid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["PrintColor"].ToString();

        }

        return strret;

    }


    protected static string get_Stage_Color(string stageid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_Stage where status=1 and archive=0 and id='" + stageid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["PrintColor"].ToString();

        }

        return strret;

    }

    protected static string get_PriorityName(string projectid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_Priority where status=1 and archive=0 and Id='" + projectid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["name"].ToString();

        }
        else
        {
            strret = "-";
        }

        return strret;

    }
    protected static string get_PriorityColor(string projectid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_Priority where status=1 and archive=0 and Id='" + projectid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["PrintColor"].ToString();

        }

        return strret;

    }
      public static void updateorder(string newrowid,string projectid, string crmid, string crmorder,string addrowtype,string maxcount)
        {
        DataAccess da = new DataAccess();
        try
        {
            if (newrowid!= "")
            {
                
                string strret = "";
                string strsql = " select ProjectId,id mid, isnull(orderno,0) orderno from Pms_tbl_AddMilestone where status=1 and archive=0 and ProjectId='" + projectid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by isnull(orderno,0) ";
               // SaveLogText("strsql222-" + newrowid + "," + projectid + "," + addrowtype);
                SaveLogText("strsql222A-" + strsql);
                DataSet dsM = da.GetDataSetInline(strsql);
                
               
                
                if (addrowtype == "1")
                {
                    //add row above
                    DataRow[] dr = dsM.Tables[0].Select("orderno>=" +Convert.ToInt32(crmorder) + "");
                    if(dr.Length>0)
                    {
                        SaveLogText("strsql222B-" + dr.Length);

                        for (int i=0;i< dr.Length;i++)
                        {
                            strsql = " update Pms_tbl_AddMilestone set orderno=(orderno+1)  where status=1 and archive=0 and ProjectId='" + projectid + "' and id='" + dr[i]["mid"].ToString() + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                            da.executeDMLQueryinline(strsql);
                        }
                        strsql = " update Pms_tbl_AddMilestone set orderno='" + crmorder + "'  where status=1 and archive=0 and ProjectId='" + projectid + "' and id='" + newrowid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                        da.executeDMLQueryinline(strsql);

                    }
                    else
                    {
                        strsql = " update Pms_tbl_AddMilestone set orderno='" + crmorder + "'  where status=1 and archive=0 and ProjectId='" + projectid + "' and id='" + newrowid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                        da.executeDMLQueryinline(strsql);
                    }

                }
                else if (addrowtype == "2")
                {
                    //add row down
                    if (Convert.ToInt32(crmorder) == 0)
                    {
                        int maxno = (Convert.ToInt32(maxcount == "" ? "0" : maxcount) + 1);
                        strsql = " update Pms_tbl_AddMilestone set orderno='" + maxno + "'  where status=1 and archive=0 and ProjectId='" + projectid + "' and id='" + newrowid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                        da.executeDMLQueryinline(strsql);
                    }
                    else
                    {
                        DataRow[] dr = dsM.Tables[0].Select("orderno>" + Convert.ToInt32(crmorder) + "");
                        if (dr.Length > 0)
                        {
                            for (int i = 0; i < dr.Length; i++)
                            {
                                strsql = " update Pms_tbl_AddMilestone set orderno=(orderno+1)  where status=1 and archive=0 and ProjectId='" + projectid + "' and id='" + dr[i]["mid"].ToString() + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                                da.executeDMLQueryinline(strsql);
                            }
                            strsql = " update Pms_tbl_AddMilestone set orderno='" + (Convert.ToInt32(crmorder) + 1) + "'  where status=1 and archive=0 and ProjectId='" + projectid + "' and id='" + newrowid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                            da.executeDMLQueryinline(strsql);

                        }
                        else
                        {
                            strsql = " update Pms_tbl_AddMilestone set orderno='" + (Convert.ToInt32(crmorder) + 1) + "'  where status=1 and archive=0 and ProjectId='" + projectid + "' and id='" + newrowid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                            da.executeDMLQueryinline(strsql);
                        }
                    }
                }
                else
                {
                    
                    
                    strsql = " update Pms_tbl_AddMilestone set orderno='"+(Convert.ToInt32(maxcount)+1).ToString()+ "'  where status=1 and archive=0 and ProjectId='" + projectid + "' and id='"+ newrowid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                    da.executeDMLQueryinline(strsql);
                    //add row default

                }
            }
        }
        catch
        {

        }

        }
[WebMethod]
    public static string addnewmilestoneblank(string addon, string currentindex, string pid, string mid, string orderno)
    {
        string retval = "0";
        function fns = new function();
    DataAccess da1 = new DataAccess();
        DataSet ds;
        string strsql = "";
        try
        {
            string strsqlcnt = " select max(isnull(orderno,0)) orderno from Pms_tbl_AddMilestone where status=1 and archive=0 and ProjectId='" + pid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  ";
            // SaveLogText("strsql222-" + newrowid + "," + projectid + "," + addrowtype);
            //SaveLogText("strsql222-" + strsqlcnt);
            int maxcount = 0;
            try
            {
                DataSet dsM = da1.GetDataSetInline(strsqlcnt);
                if (dsM.Tables[0].Rows.Count > 0)
                {
                    maxcount = Convert.ToInt32(dsM.Tables[0].Rows[0]["orderno"].ToString());
                }
            }
            catch (Exception ex)
            {
               // SaveLogText("maxcntex-" + strsqlcnt);
            }
            string strweight = "";
            string strdurationtype = "";
            string strduration = "";
            string stristask = "";
            string strstartdate = "";
            string strenddate = "";
            string strowner = "";
            string strteam = "";
            string strtimeline = "";
            string strstatus = "";
            string strpriority = "";
            string strqldetails= "select Weightage,StartDate,EndDate,Owner,team,'1' pstatus,Priority,isnull(istask,0) istask ,isnull(Durationtype,0) Durationtype,TaskDuration from Pms_tbl_AddMilestone where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + mid + "'";
            DataSet dsDetails = da1.GetDataSetInline(strqldetails);
            if(dsDetails.Tables[0].Rows.Count>0)
            {
                strweight = dsDetails.Tables[0].Rows[0]["Weightage"].ToString();
                strdurationtype = dsDetails.Tables[0].Rows[0]["Durationtype"].ToString();
                strduration = dsDetails.Tables[0].Rows[0]["TaskDuration"].ToString();
                stristask = dsDetails.Tables[0].Rows[0]["istask"].ToString();
                strstartdate = dsDetails.Tables[0].Rows[0]["StartDate"].ToString();
                strenddate = dsDetails.Tables[0].Rows[0]["EndDate"].ToString();
                strowner = dsDetails.Tables[0].Rows[0]["Owner"].ToString();
                strteam = dsDetails.Tables[0].Rows[0]["team"].ToString();
                strstatus = dsDetails.Tables[0].Rows[0]["pstatus"].ToString();
                strpriority = dsDetails.Tables[0].Rows[0]["Priority"].ToString();
            }


            SqlParameter[] prm = new SqlParameter[23];
            prm[0] = new SqlParameter("@id", "0");
            prm[1] = new SqlParameter("@ProjectId", pid);
            prm[2] = new SqlParameter("@Milestone", DBNull.Value);
            prm[3] = new SqlParameter("@Weightage", strweight);
            prm[4] = new SqlParameter("@StartDate", strstartdate);
            prm[5] = new SqlParameter("@EndDate", strenddate);
            if (strowner != "")
            {
                prm[6] = new SqlParameter("@Owner", strowner);
            }
            else
            {
                prm[6] = new SqlParameter("@Owner", DBNull.Value);
            }
            prm[7] = new SqlParameter("@Access", DBNull.Value);
            if (strteam != "")
            {
                prm[8] = new SqlParameter("@team", strteam);
            }
            else
            {
                prm[8] = new SqlParameter("@team", DBNull.Value);
            }
            if (strteam != "")
            {
                prm[9] = new SqlParameter("@P_Status", strstatus);
            }
            else
            {
                prm[9] = new SqlParameter("@P_Status", DBNull.Value);
            }
           

           
            if (strpriority != "")
            {
                prm[10] = new SqlParameter("@Priority", strpriority);
            }
            else
            {
                prm[10] = new SqlParameter("@Priority", DBNull.Value);
            }
            prm[11] = new SqlParameter("@Budget", DBNull.Value);
            prm[12] = new SqlParameter("@Actual", DBNull.Value);
            prm[13] = new SqlParameter("@checkmileWeightage", DBNull.Value);
            if (strdurationtype != "")
            {
                prm[14] = new SqlParameter("@Durationtype", strdurationtype);
            }
            else
            {
                prm[14] = new SqlParameter("@Durationtype", DBNull.Value);
            }
            
            if (strduration != "")
            {
                prm[15] = new SqlParameter("@TaskDuration", strduration);
            }
            else
            {
                prm[15] = new SqlParameter("@TaskDuration", DBNull.Value);
            }
            
            prm[16] = new SqlParameter("@istask", DBNull.Value);

            prm[17] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[18] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prm[19] = new SqlParameter("@cdate", da1.getcdatetime());
            prm[20] = new SqlParameter("@type", "Insert");
            prm[21] = new SqlParameter("@orderno", DBNull.Value);            
            prm[22] = new SqlParameter("ReturnValue", SqlDbType.Int);
            prm[22].Direction = ParameterDirection.ReturnValue;
            int retval1 = da1.executeDMLQuery("pmstbl_Addmilestone_usp", prm);
            retval = retval1.ToString();
            try
            {
                
                int checkweitage = 0;
                int checktask = 0;
                int checktaskWeightage = 0;
                decimal wetagecount = 0;
                decimal totalwetage = 0;
                string strweightage = "select isnull(checkprojectWeightage ,0) as checkprojectWeightage,isnull(hastask ,0) hastask,isnull(checktaskWeightage,0) checktaskWeightage  from Pms_tbl_AddProject where id='" + pid + "' and  status=1  and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                SaveLogText("strweightage-"+ strweightage);
                DataSet dsweitagecheck = da1.GetDataSetInline(strweightage);
                if (dsweitagecheck.Tables[0].Rows.Count > 0)
                {

                    checkweitage = Convert.ToInt32(dsweitagecheck.Tables[0].Rows[0]["checkprojectWeightage"]);
                    checktask=Convert.ToInt32(dsweitagecheck.Tables[0].Rows[0]["hastask"]);
                    checktaskWeightage = Convert.ToInt32(dsweitagecheck.Tables[0].Rows[0]["checktaskWeightage"]);

                }

                if (checkweitage == 1)
                {
                    string strweightagecount = "select count (*) as WeitageCount from Pms_tbl_AddMilestone where ProjectId='" + pid + "' and  status=1   and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                    DataSet dsweitagecount = da1.GetDataSetInline(strweightagecount);
                    if (dsweitagecount.Tables[0].Rows.Count > 0)
                    {


                        wetagecount = Convert.ToInt32(dsweitagecount.Tables[0].Rows[0]["WeitageCount"]);

                    }

                    totalwetage = 100 / wetagecount;
                    string strstatusupdate = "update Pms_tbl_AddMilestone set Weightage='" + fns.ToFormattedCurrencyString(totalwetage.ToString()) + "' where ProjectId='" +pid + "' and status=1  and archive=0";
                    DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                }
               
                string strsubtaskupdate = "";
                if (checktask == 1)
                {
                    strsubtaskupdate = "update Pms_tbl_AddMilestone set istask=1 ";
                    
                }
                if (checktaskWeightage == 1)
                {
                    if (strsubtaskupdate != "")
                    {
                        strsubtaskupdate += " , checkmileWeightage=1 ";
                    }
                    else
                    {
                        strsubtaskupdate += "update Pms_tbl_AddMilestone set checkmileWeightage=1 ";
                    }



                }
                if (strsubtaskupdate != "")
                {
                    strsubtaskupdate += " where id='" + retval + "' and status=1 and archive=0";
                    da1.executeDMLQueryinline(strsubtaskupdate);
                }
            }
            catch
            { }
            updateorder(retval,pid,mid,orderno,addon, maxcount.ToString());

        }
        catch
        { }

        return retval;
    }
    [WebMethod]
    public static string addnewmilestoneblanktask(string addon, string currentindex, string pid, string mid,string taskid, string orderno)
    {
        string retval = "0";
        function fns = new function();
        DataAccess da1 = new DataAccess();
        DataSet ds;
        string strsql = "";
        try
        {
            string strsqlcnt = " select max(isnull(orderno,0)) orderno from Pms_tbl_Addtask where status=1 and archive=0 and ProjectId='" + pid + "' and Milestoneid='"+ mid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
            int maxcount = 0;
            SaveLogText("maxcnt-" + strsqlcnt);
            try
            {
                DataSet dsM = da1.GetDataSetInline(strsqlcnt);
                if (dsM.Tables[0].Rows.Count > 0)
                {
                    maxcount =Convert.ToInt32(dsM.Tables[0].Rows[0]["orderno"].ToString());
                }
            }
            catch(Exception ex) {
                //SaveLogText("maxcntex-" + strsqlcnt);
            }
            string strweight = "";
            string strdurationtype = "";
            string strduration = "";
            string stristask = "";
            string strstartdate = "";
            string strenddate = "";
            string strowner = "";
            string strteam = "";
            string strtimeline = "";
            string strstatus = "";
            string strpriority = "";
            string strqldetails = "select Weightage,StartDate,Targetdate EndDate,Owner,team,'1' pstatus,Priority,isnull(issubtask,0) issubtask ,TaskDuration from Pms_tbl_Addtask where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + taskid + "'";
            DataSet dsDetails = da1.GetDataSetInline(strqldetails);
            if (dsDetails.Tables[0].Rows.Count > 0)
            {
                strweight = dsDetails.Tables[0].Rows[0]["Weightage"].ToString();
                //strdurationtype = dsDetails.Tables[0].Rows[0]["Durationtype"].ToString();
                strduration = dsDetails.Tables[0].Rows[0]["TaskDuration"].ToString();
                stristask = dsDetails.Tables[0].Rows[0]["issubtask"].ToString();
                strstartdate = dsDetails.Tables[0].Rows[0]["StartDate"].ToString();
                strenddate = dsDetails.Tables[0].Rows[0]["EndDate"].ToString();
                strowner = dsDetails.Tables[0].Rows[0]["Owner"].ToString();
                strteam = dsDetails.Tables[0].Rows[0]["team"].ToString();
                strstatus = dsDetails.Tables[0].Rows[0]["pstatus"].ToString();
                strpriority = dsDetails.Tables[0].Rows[0]["Priority"].ToString();
            }

            SqlParameter[] prm = new SqlParameter[34];
            prm[0] = new SqlParameter("@id", "0");
            prm[1] = new SqlParameter("@ProjectId", pid);
            prm[2] = new SqlParameter("@Milestoneid", mid);
            prm[3] = new SqlParameter("@tasklist", DBNull.Value);
            prm[4] = new SqlParameter("@taskname", DBNull.Value);
            prm[5] = new SqlParameter("@taskdescripton", DBNull.Value);
            if (strowner != "")
            {
                prm[6] = new SqlParameter("@owner", strowner);
            }
            else
            {
                prm[6] = new SqlParameter("@owner", DBNull.Value);
            }
            if (strstartdate != "")
            {
                prm[7] = new SqlParameter("@Startdate", strstartdate);
            }
            else
            {
                prm[7] = new SqlParameter("@Startdate", DBNull.Value);
            }
            if (strenddate != "")
            {
                prm[8] = new SqlParameter("@Targetdate", strenddate);
            }
            else
            {
                prm[8] = new SqlParameter("@Targetdate", DBNull.Value);
            }
            
           prm[9] = new SqlParameter("@StartTime", DBNull.Value);
           prm[10] = new SqlParameter("@Timeline", DBNull.Value);
            if (strduration != "")
            {
                prm[11] = new SqlParameter("@TaskDuration", strduration);
            }
            else
            {
                prm[11] = new SqlParameter("@TaskDuration", DBNull.Value);
            }
            if (strstatus != "")
            {
                prm[12] = new SqlParameter("@P_Status", strstatus);
            }
            else
            {
                prm[12] = new SqlParameter("@P_Status", DBNull.Value);
            }
            if (strpriority != "")
            {
                prm[13] = new SqlParameter("@Priority", strpriority);
            }
            else
            {
                prm[13] = new SqlParameter("@Priority", DBNull.Value);
            }
           
            prm[14] = new SqlParameter("@Budget", DBNull.Value);
            prm[15] = new SqlParameter("@Actual", DBNull.Value);
            prm[16] = new SqlParameter("@Reminder", DBNull.Value);
            prm[17] = new SqlParameter("@ReminderType", DBNull.Value);
            prm[18] = new SqlParameter("@Days", DBNull.Value);
            prm[19] = new SqlParameter("@Time", DBNull.Value);
            prm[20] = new SqlParameter("@Notifyusers", DBNull.Value);
            prm[21] = new SqlParameter("@ChooseDate", DBNull.Value);
            prm[22] = new SqlParameter("@Occurrence", DBNull.Value);
            prm[23] = new SqlParameter("@tagto", DBNull.Value);
            
            if (strteam != "")
            {
                prm[24] = new SqlParameter("@team", strteam);
            }
            else
            {
                prm[24] = new SqlParameter("@team", DBNull.Value);
            }
            prm[25] = new SqlParameter("@Stage", DBNull.Value);
            if (strweight != "")
            {
                prm[26] = new SqlParameter("@Weightage", strweight);
            }
            else
            {
                prm[26] = new SqlParameter("@Weightage", DBNull.Value);
            }
           
            prm[27] = new SqlParameter("@issubtask", DBNull.Value);
            prm[28] = new SqlParameter("@checkTaskWeightage", DBNull.Value);
            prm[29] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[30] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prm[31] = new SqlParameter("@cdate", da1.getcdatetime());
            prm[32] = new SqlParameter("@type", "insert");
            prm[33] = new SqlParameter("ReturnValue", SqlDbType.Int);
            prm[33].Direction = ParameterDirection.ReturnValue;
            int retval1 = da1.executeDMLQuery("pms_Adtask_usp", prm);
            retval = retval1.ToString();
            try
            {
                int checksub = 0;
                int checksubweitage = 0;
                int checkweitage = 0;
                decimal wetagecount = 0;
                decimal totalwetage = 0;
                // get project
                string strproject = "select isnull(hastask ,0) hastask,isnull(checktaskWeightage,0) checktaskWeightage,isnull(hassubtask,0) hassubtask,isnull(checksubtaskWeightage,0) checksubtaskWeightage  from Pms_tbl_AddProject where id='" + pid + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                DataSet dsprojectcheck = da1.GetDataSetInline(strproject);
               
// end project
                string strweightage = "select isnull(checkmileWeightage ,0) as checkmileWeightage  from Pms_tbl_AddMilestone where id='" + mid + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                DataSet dsweitagecheck = da1.GetDataSetInline(strweightage);
                if (dsweitagecheck.Tables[0].Rows.Count > 0)
                {

                    checkweitage = Convert.ToInt32(dsweitagecheck.Tables[0].Rows[0]["checkmileWeightage"]);

                }
                // check on project
                if(checkweitage==0)
                {
                    if (dsprojectcheck.Tables[0].Rows.Count > 0)
                    {

                        checkweitage = Convert.ToInt32(dsprojectcheck.Tables[0].Rows[0]["checktaskWeightage"]);

                    }
                }

                if (checkweitage == 1)
                {
                    string strweightagecount = "select count (*) as WeitageCount from Pms_tbl_Addtask where Milestoneid='" + mid + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                    DataSet dsweitagecount = da1.GetDataSetInline(strweightagecount);
                    if (dsweitagecount.Tables[0].Rows.Count > 0)
                    {

                        wetagecount = Convert.ToInt32(dsweitagecount.Tables[0].Rows[0]["WeitageCount"]);

                    }

                    totalwetage = 100 / wetagecount;
                    string strstatusupdate = "update Pms_tbl_Addtask set Weightage='" + fns.ToFormattedCurrencyString(totalwetage.ToString()) + "' where Milestoneid='" + mid + "' and status=1 and archive=0";
                    DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                }
                if (dsprojectcheck.Tables[0].Rows.Count > 0)
                {

                    checksub = Convert.ToInt32(dsprojectcheck.Tables[0].Rows[0]["hassubtask"]);
                    checksubweitage= Convert.ToInt32(dsprojectcheck.Tables[0].Rows[0]["checksubtaskWeightage"]);
                }
                string strsubtaskupdate = "";
                if (checksub==1)
                {
                     strsubtaskupdate = "update Pms_tbl_Addtask set issubtask=1 ";
                    
                }
                if (checksubweitage == 1)
                {
                    if (strsubtaskupdate != "")
                    {
                        strsubtaskupdate += " , checkTaskWeightage=1 ";
                    }
                    else
                    {
                        strsubtaskupdate += "update Pms_tbl_Addtask set checkTaskWeightage=1 ";
                    }
     


                }
                if(strsubtaskupdate!="")
                {
                    strsubtaskupdate += " where id='" + retval + "' and status=1 and archive=0";
                    da1.executeDMLQueryinline(strsubtaskupdate);
                }

            }
            catch
            { }
            updateordertask(retval, pid, mid, taskid, orderno, addon, maxcount.ToString());

        }
        catch
        { }

        return retval;
    }
    public static void updateordertask(string newrowid, string projectid, string crmid,string crtid, string crmorder, string addrowtype, string maxcount)
    {
        DataAccess da = new DataAccess();
        try
        {
            if (newrowid != "")
            {

                string strret = "";
                string strsql = " select ProjectId,Milestoneid mid,id tid, isnull(orderno,0) orderno from Pms_tbl_Addtask where status=1 and archive=0 and ProjectId='" + projectid + "' and Milestoneid='" + crmid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by isnull(orderno,0) ";
                // SaveLogText("strsql222-" + newrowid + "," + projectid + "," + addrowtype);
                //SaveLogText("strsql222-" + strsql);
                DataSet dsM = da.GetDataSetInline(strsql);



                if (addrowtype == "1")
                {
                    //add row above
                    DataRow[] dr = dsM.Tables[0].Select("orderno>=" + Convert.ToInt32(crmorder) + "");
                    if (dr.Length > 0)
                    {
                        for (int i = 0; i < dr.Length; i++)
                        {
                            strsql = " update Pms_tbl_Addtask set orderno=(orderno+1)  where status=1 and archive=0 and ProjectId='" + projectid + "' and Milestoneid ='"+ dr[i]["mid"].ToString() + "' and  id='" + dr[i]["tid"].ToString() + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                            da.executeDMLQueryinline(strsql);
                        }
                        strsql = " update Pms_tbl_Addtask set orderno='" + crmorder + "'  where status=1 and archive=0 and ProjectId='" + projectid + "'  and Milestoneid ='" + crmid + "' and id='" + newrowid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                        da.executeDMLQueryinline(strsql);

                    }
                    else
                    {
                        strsql = " update Pms_tbl_Addtask set orderno='" + crmorder + "'  where status=1 and archive=0 and ProjectId='" + projectid + "'  and Milestoneid ='" + crmid + "' and id='" + newrowid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                        da.executeDMLQueryinline(strsql);
                    }

                }
                else if (addrowtype == "2")
                {
                    //add row down
                    if (Convert.ToInt32(crmorder) == 0)
                    {
                        int maxno=(Convert.ToInt32(maxcount==""?"0": maxcount)+1);
                        strsql = " update Pms_tbl_Addtask set orderno='" + maxno + "'  where status=1 and archive=0  and id='" + newrowid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                        da.executeDMLQueryinline(strsql);
                    }
                    else
                    {
                        DataRow[] dr = dsM.Tables[0].Select("orderno>" + Convert.ToInt32(crmorder) + "");
                        if (dr.Length > 0)
                        {

                            for (int i = 0; i < dr.Length; i++)
                            {
                                strsql = " update Pms_tbl_Addtask set orderno=(orderno+1)  where status=1 and archive=0 and  id='" + dr[i]["tid"].ToString() + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                                da.executeDMLQueryinline(strsql);
                            }
                            strsql = " update Pms_tbl_Addtask set orderno='" + (Convert.ToInt32(crmorder) + 1) + "'  where status=1 and archive=0  and id='" + newrowid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                            da.executeDMLQueryinline(strsql);

                        }
                        else
                        {
                            strsql = " update Pms_tbl_Addtask set orderno='" + (Convert.ToInt32(crmorder) + 1) + "'  where status=1 and archive=0  and id='" + newrowid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                            da.executeDMLQueryinline(strsql);
                        }
                    }
                }
                else
                {


                    strsql = " update Pms_tbl_Addtask set orderno='" + (Convert.ToInt32(maxcount) + 1).ToString() + "'  where status=1 and archive=0 and ProjectId='" + projectid + "' and Milestoneid ='" + crmid + "' and id='" + newrowid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                    da.executeDMLQueryinline(strsql);

                    //add row default

                }
            }
        }
        catch
        {

        }

    }

    [WebMethod]
    public static string addnewmilestoneblanksubtask(string addon, string currentindex, string pid, string mid, string taskid, string subtaskid, string orderno)
    {
        string retval = "0";
        function fns = new function();
        DataAccess da1 = new DataAccess();
        DataSet ds;
        string strsql = "";
        try
        {
            string strsqlcnt = " select max(isnull(orderno,0)) orderno from Pms_tbl_Addsubtask where status=1 and archive=0 and ProjectId='" + pid + "' and Milestoneid='" + mid + "' and task='"+ taskid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
            // SaveLogText("strsql222-" + newrowid + "," + projectid + "," + addrowtype);
           // SaveLogText("strsql222-" + strsqlcnt);
            
            int maxcount = 0;
            try
            {
                DataSet dsM = da1.GetDataSetInline(strsqlcnt);
                if (dsM.Tables[0].Rows.Count > 0)
                {
                    maxcount = Convert.ToInt32(dsM.Tables[0].Rows[0]["orderno"].ToString());
                }
            }
            catch (Exception ex)
            {
               // SaveLogText("maxcntex-" + strsqlcnt);
            }

            string strweight = "";
            string strdurationtype = "";
            string strduration = "";
            string stristask = "";
            string strstartdate = "";
            string strenddate = "";
            string strowner = "";
            string strteam = "";
            string strtimeline = "";
            string strstatus = "";
            string strpriority = "";
            string strqldetails = "select StartTime,Timeline,Weightage,StartDate,Targetdate EndDate,Owner,team,'1' pstatus,Priority ,TaskDuration from Pms_tbl_Addsubtask where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + subtaskid + "'";
            DataSet dsDetails = da1.GetDataSetInline(strqldetails);
            if (dsDetails.Tables[0].Rows.Count > 0)
            {
                strweight = dsDetails.Tables[0].Rows[0]["Weightage"].ToString();
                //strdurationtype = dsDetails.Tables[0].Rows[0]["Durationtype"].ToString();
                strduration = dsDetails.Tables[0].Rows[0]["TaskDuration"].ToString();
               // stristask = dsDetails.Tables[0].Rows[0]["issubtask"].ToString();
                strstartdate = dsDetails.Tables[0].Rows[0]["StartDate"].ToString();
                strenddate = dsDetails.Tables[0].Rows[0]["EndDate"].ToString();
                strowner = dsDetails.Tables[0].Rows[0]["Owner"].ToString();
                strteam = dsDetails.Tables[0].Rows[0]["team"].ToString();
                strstatus = dsDetails.Tables[0].Rows[0]["pstatus"].ToString();
                strpriority = dsDetails.Tables[0].Rows[0]["Priority"].ToString();
            }
            SqlParameter[] prm = new SqlParameter[33];
            prm[0] = new SqlParameter("@id", "0");
            prm[1] = new SqlParameter("@ProjectId", pid);
            prm[2] = new SqlParameter("@Milestoneid", mid);
            prm[3] = new SqlParameter("@tasklist", DBNull.Value);
            prm[4] = new SqlParameter("@task", taskid);
            prm[5] = new SqlParameter("@Subtaskname", DBNull.Value);
            prm[6] = new SqlParameter("@SubTaskDescription", DBNull.Value);
            
            if (strowner != "")
            {
                prm[7] = new SqlParameter("@owner", strowner);
            }
            else
            {
                prm[7] = new SqlParameter("@owner", DBNull.Value);
            }

            if (strstartdate != "")
            {
                prm[8] = new SqlParameter("@Startdate", strstartdate);
            }
            else
            {
                prm[8] = new SqlParameter("@Startdate", DBNull.Value);
            }
            
            if (strenddate != "")
            {
                prm[9] = new SqlParameter("@Targetdate", strenddate);
            }
            else
            {
                prm[9] = new SqlParameter("@Targetdate", DBNull.Value);
            }



            prm[10] = new SqlParameter("@StartTime", DBNull.Value);
           

            
                prm[11] = new SqlParameter("@Timeline", DBNull.Value);
           

           

            if (strduration != "")
            {
                prm[12] = new SqlParameter("@TaskDuration", strduration);
            }
            else
            {
                prm[12] = new SqlParameter("@TaskDuration", DBNull.Value);
            }

            if (strstatus != "")
            {
                prm[13] = new SqlParameter("@P_Status", strstatus);
            }
            else
            {
                prm[13] = new SqlParameter("@P_Status", DBNull.Value);
            }
            if (strpriority != "")
            {
                prm[14] = new SqlParameter("@Priority", strpriority);
            }
            else
            {
                prm[14] = new SqlParameter("@Priority", DBNull.Value);
            }

            prm[15] = new SqlParameter("@Budget", DBNull.Value);
            

            
                prm[16] = new SqlParameter("@Actual", DBNull.Value);
            


            prm[17] = new SqlParameter("@Reminder", DBNull.Value);

            
                prm[18] = new SqlParameter("@ReminderType", DBNull.Value);
            

           
                prm[19] = new SqlParameter("@Days", DBNull.Value);
           
            
                prm[20] = new SqlParameter("@Time", DBNull.Value);
            
            
                prm[21] = new SqlParameter("@Notifyusers", DBNull.Value);
            

            
                prm[22] = new SqlParameter("@ChooseDate", DBNull.Value);
            

            prm[23] = new SqlParameter("@Occurrence", DBNull.Value);
            prm[24] = new SqlParameter("@tagto", DBNull.Value);
           
            if (strteam != "")
            {
                prm[25] = new SqlParameter("@team", strteam);
            }
            else
            {
                prm[25] = new SqlParameter("@team", DBNull.Value);
            }
            prm[26] = new SqlParameter("@Stage", DBNull.Value);
            if (strweight != "")
            {
                prm[27] = new SqlParameter("@Weightage", strweight);
            }
            else
            {
                prm[27] = new SqlParameter("@Weightage", DBNull.Value);
            }
            
            prm[28] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[29] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prm[30] = new SqlParameter("@cdate", da1.getcdatetime());
            prm[31] = new SqlParameter("@type", "insert");
            prm[32] = new SqlParameter("ReturnValue", SqlDbType.Int);
            prm[32].Direction = ParameterDirection.ReturnValue;
            int retval1 = da1.executeDMLQuery("pms_Add_Subtask_usp", prm);
            retval = retval1.ToString();
            try
            {
                int checksub = 0;
                int checksubweitage = 0;
                int checkweitage = 0;
                decimal wetagecount = 0;
                decimal totalwetage = 0;
                // get project
                string strproject = "select isnull(hastask ,0) hastask,isnull(checktaskWeightage,0) checktaskWeightage,isnull(hassubtask,0) hassubtask,isnull(checksubtaskWeightage,0) checksubtaskWeightage  from Pms_tbl_AddProject where id='" + pid + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                DataSet dsprojectcheck = da1.GetDataSetInline(strproject);

                // end project
                string strweightage = "select isnull(checkTaskWeightage ,0) as checkTaskWeightage  from Pms_tbl_Addtask where id='" + taskid + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                DataSet dsweitagecheck = da1.GetDataSetInline(strweightage);
                if (dsweitagecheck.Tables[0].Rows.Count > 0)
                {

                    checkweitage = Convert.ToInt32(dsweitagecheck.Tables[0].Rows[0]["checkTaskWeightage"]);

                }
                // check on project
                if (checkweitage == 0)
                {
                    if (dsprojectcheck.Tables[0].Rows.Count > 0)
                    {

                        checkweitage = Convert.ToInt32(dsprojectcheck.Tables[0].Rows[0]["checksubtaskWeightage"]);

                    }
                }

                if (checkweitage == 1)
                {
                    string strweightagecount = "select count (*) as WeitageCount from Pms_tbl_Addsubtask where task='" + taskid + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                    DataSet dsweitagecount = da1.GetDataSetInline(strweightagecount);
                    if (dsweitagecount.Tables[0].Rows.Count > 0)
                    {

                        wetagecount = Convert.ToInt32(dsweitagecount.Tables[0].Rows[0]["WeitageCount"]);

                    }

                    totalwetage = 100 / wetagecount;
                    string strstatusupdate = "update Pms_tbl_Addsubtask set Weightage='" + fns.ToFormattedCurrencyString(totalwetage.ToString()) + "' where task='" + taskid + "' and status=1 and archive=0";
                    DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                }
               

            }
            catch
            { }
            updateordersubtask(retval, pid, mid, taskid, subtaskid, orderno, addon, maxcount.ToString());

        }
        catch
        { }

        return retval;
    }
    public static void updateordersubtask(string newrowid, string projectid, string crmid, string crtid, string crsubtid, string crmorder, string addrowtype, string maxcount)
    {
        DataAccess da = new DataAccess();
        try
        {
            if (newrowid != "")
            {

                string strret = "";
                string strsql = " select ProjectId,Milestoneid mid,task tid,id subtid, isnull(orderno,0) orderno from Pms_tbl_Addsubtask where status=1 and archive=0 and ProjectId='" + projectid + "' and Milestoneid='" + crmid + "' and task='" + crtid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by isnull(orderno,0) ";
                // SaveLogText("strsql222-" + newrowid + "," + projectid + "," + addrowtype);
                //SaveLogText("strsql222-" + strsql);
                DataSet dsM = da.GetDataSetInline(strsql);



                if (addrowtype == "1")
                {
                    //add row above
                    DataRow[] dr = dsM.Tables[0].Select("orderno>=" + Convert.ToInt32(crmorder) + "");
                    if (dr.Length > 0)
                    {
                        for (int i = 0; i < dr.Length; i++)
                        {
                            strsql = " update Pms_tbl_Addsubtask set orderno=(orderno+1)  where status=1 and archive=0 and ProjectId='" + projectid + "' and Milestoneid ='" + dr[i]["mid"].ToString() + "'  and task ='" + dr[i]["tid"].ToString() + "' and  id='" + dr[i]["subtid"].ToString() + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                            da.executeDMLQueryinline(strsql);
                        }
                        strsql = " update Pms_tbl_Addsubtask set orderno='" + crmorder + "'  where status=1 and archive=0 and ProjectId='" + projectid + "'  and Milestoneid ='" + crmid + "' and id='" + newrowid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                        da.executeDMLQueryinline(strsql);

                    }
                    else
                    {
                        strsql = " update Pms_tbl_Addsubtask set orderno='" + crmorder + "'  where status=1 and archive=0 and ProjectId='" + projectid + "'  and Milestoneid ='" + crmid + "' and id='" + newrowid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                        da.executeDMLQueryinline(strsql);
                    }

                }
                else if (addrowtype == "2")
                {
                    //add row down
                    if (Convert.ToInt32(crmorder) == 0)
                    {
                        int maxno = (Convert.ToInt32(maxcount == "" ? "0" : maxcount) + 1);
                        strsql = " update Pms_tbl_Addsubtask set orderno='" + maxno + "'  where status=1 and archive=0  and id='" + newrowid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                        da.executeDMLQueryinline(strsql);
                    }
                    else
                    {
                        DataRow[] dr = dsM.Tables[0].Select("orderno>" + Convert.ToInt32(crmorder) + "");
                        if (dr.Length > 0)
                        {

                            for (int i = 0; i < dr.Length; i++)
                            {
                                strsql = " update Pms_tbl_Addsubtask set orderno=(orderno+1)  where status=1 and archive=0 and  id='" + dr[i]["subtid"].ToString() + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                                da.executeDMLQueryinline(strsql);
                            }
                            strsql = " update Pms_tbl_Addsubtask set orderno='" + (Convert.ToInt32(crmorder) + 1) + "'  where status=1 and archive=0  and id='" + newrowid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                            da.executeDMLQueryinline(strsql);

                        }
                        else
                        {
                            strsql = " update Pms_tbl_Addsubtask set orderno='" + (Convert.ToInt32(crmorder) + 1) + "'  where status=1 and archive=0  and id='" + newrowid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                            da.executeDMLQueryinline(strsql);
                        }
                    }
                }
                else
                {


                    strsql = " update Pms_tbl_Addsubtask set orderno='" + (Convert.ToInt32(maxcount) + 1).ToString() + "'  where status=1 and archive=0 and ProjectId='" + projectid + "' and Milestoneid ='" + crmid + "' and id='" + newrowid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                    da.executeDMLQueryinline(strsql);

                    //add row default

                }
            }
        }
        catch
        {

        }

    }

    [WebMethod]
    public static string SaveTask(MultiTaskData taskdetails)
    {
        function fns = new function();
        string retval = "0";
        DataAccess da1 = new DataAccess();
        string editid = taskdetails.id;
        string entrytype = "Insert";
        if (editid != "" && editid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[34];
        prm[0] = new SqlParameter("@id", editid);
        prm[1] = new SqlParameter("@ProjectId", taskdetails.ProjectId);
        prm[2] = new SqlParameter("@Milestoneid", taskdetails.Milestoneid);
        prm[3] = new SqlParameter("@tasklist", taskdetails.tasklist);
        prm[4] = new SqlParameter("@taskname", taskdetails.taskname);
        prm[5] = new SqlParameter("@taskdescripton", taskdetails.taskdescripton);
        prm[6] = new SqlParameter("@owner", taskdetails.owner);
        if (taskdetails.Startdate == string.Empty)
        {
            prm[7] = new SqlParameter("@Startdate", DBNull.Value);
        }
        else
        {
            prm[7] = new SqlParameter("@Startdate", taskdetails.Startdate);
        }
        if (taskdetails.Targetdate == string.Empty)
        {
            prm[8] = new SqlParameter("@Targetdate", DBNull.Value);
        }
        else
        {
            prm[8] = new SqlParameter("@Targetdate", taskdetails.Targetdate);
        }


        if (taskdetails.StartTime == string.Empty)
        {
            prm[9] = new SqlParameter("@StartTime", DBNull.Value);
        }
        else
        {
            prm[9] = new SqlParameter("@StartTime", taskdetails.StartTime);
        }


        if (taskdetails.Timeline == string.Empty)
        {
            prm[10] = new SqlParameter("@Timeline", DBNull.Value);
        }
        else
        {
            prm[10] = new SqlParameter("@Timeline", taskdetails.Timeline);
        }

        prm[11] = new SqlParameter("@TaskDuration", taskdetails.TaskDuration);


        prm[12] = new SqlParameter("@P_Status", taskdetails.P_Status);
        prm[13] = new SqlParameter("@Priority", taskdetails.Priority);


        if (taskdetails.Budget == string.Empty)
        {
            prm[14] = new SqlParameter("@Budget", DBNull.Value);
        }
        else
        {
            prm[14] = new SqlParameter("@Budget", taskdetails.Budget);
        }

        if (taskdetails.Actual == string.Empty)
        {
            prm[15] = new SqlParameter("@Actual", DBNull.Value);
        }
        else
        {
            prm[15] = new SqlParameter("@Actual", taskdetails.Actual);
        }


        prm[16] = new SqlParameter("@Reminder", taskdetails.Reminder);

        if (taskdetails.ReminderType == string.Empty)
        {
            prm[17] = new SqlParameter("@ReminderType", DBNull.Value);
        }
        else
        {
            prm[17] = new SqlParameter("@ReminderType", taskdetails.ReminderType);
        }

        if (taskdetails.Days == string.Empty)
        {
            prm[18] = new SqlParameter("@Days", DBNull.Value);
        }
        else
        {
            prm[18] = new SqlParameter("@Days", taskdetails.Days);

        }
        if (taskdetails.Time == string.Empty)
        {
            prm[19] = new SqlParameter("@Time", DBNull.Value);
        }
        else
        {
            prm[19] = new SqlParameter("@Time", taskdetails.Time);
        }
        if (taskdetails.Notifyusers == string.Empty)
        {
            prm[20] = new SqlParameter("@Notifyusers", DBNull.Value);
        }
        else
        {
            prm[20] = new SqlParameter("@Notifyusers", taskdetails.Notifyusers);
        }

        if (taskdetails.ChooseDate == string.Empty)
        {
            prm[21] = new SqlParameter("@ChooseDate", DBNull.Value);
        }
        else
        {
            prm[21] = new SqlParameter("@ChooseDate", taskdetails.ChooseDate);
        }

        prm[22] = new SqlParameter("@Occurrence", taskdetails.Occurrence);
        prm[23] = new SqlParameter("@tagto", taskdetails.tagto);
        prm[24] = new SqlParameter("@team", taskdetails.team);
        prm[25] = new SqlParameter("@Stage", taskdetails.Stage);
        prm[26] = new SqlParameter("@Weightage", taskdetails.Weightage);
        prm[27] = new SqlParameter("@issubtask", taskdetails.issubtask);
        prm[28] = new SqlParameter("@checkTaskWeightage", taskdetails.checkTaskWeightage);
        prm[29] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[30] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[31] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[32] = new SqlParameter("@type", entrytype);
        prm[33] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[33].Direction = ParameterDirection.ReturnValue;
        int retval1 = da1.executeDMLQuery("pms_Adtask_usp", prm);
        retval = retval1.ToString();



        // iscomplete update //

        string striscomplt = "select  isnull(iscomplete,0) as iscomplete from Pms_tbl_Projectstatus where status = 1 and archive = 0  and id='" + taskdetails.P_Status + "'";
        DataSet dsiscomplt = da1.GetDataSetInline(striscomplt);
        if (dsiscomplt.Tables[0].Rows.Count > 0)
        {
            int iscompletestatus = 0;
            iscompletestatus = Convert.ToInt32(dsiscomplt.Tables[0].Rows[0]["iscomplete"]);

            string strstatusupdateiscom = "update Pms_tbl_Addtask set iscomplete='" + iscompletestatus.ToString() + "' where id = " + retval + " and status=1 and archive=0";
            DataSet dshourminiscom = da1.GetDataSetInline(strstatusupdateiscom);

        }

        //   iscomplete update  



        int checkweitage = 0;
        decimal wetagecount = 0;
        decimal totalwetage = 0;
        string strweightage = "select isnull(checkmileWeightage ,0) as checkmileWeightage  from Pms_tbl_AddMilestone where id='" + taskdetails.Milestoneid + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
        DataSet dsweitagecheck = da1.GetDataSetInline(strweightage);
        if (dsweitagecheck.Tables[0].Rows.Count > 0)
        {

            checkweitage = Convert.ToInt32(dsweitagecheck.Tables[0].Rows[0]["checkmileWeightage"]);

        }

        if (checkweitage == 1)
        {
            string strweightagecount = "select count (*) as WeitageCount from Pms_tbl_Addtask where Milestoneid='" + taskdetails.Milestoneid + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
            DataSet dsweitagecount = da1.GetDataSetInline(strweightagecount);
            if (dsweitagecount.Tables[0].Rows.Count > 0)
            {

                wetagecount = Convert.ToInt32(dsweitagecount.Tables[0].Rows[0]["WeitageCount"]);

            }

            totalwetage = 100 / wetagecount;
            string strstatusupdate = "update Pms_tbl_Addtask set Weightage='" + fns.ToFormattedCurrencyString(totalwetage.ToString()) + "' where Milestoneid='" + taskdetails.Milestoneid + "' and status=1 and archive=0";
            DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

        }


        int iscomplete = 0;
        string finsql1 = "select isnull(iscomplete,0) as iscomplete, * from Pms_tbl_Addtask where id=" + retval + " and status=1 and archive=0";
        DataSet dsfindid1 = da1.GetDataSetInline(finsql1);
        if (dsfindid1.Tables[0].Rows.Count > 0)
        {

            iscomplete = Convert.ToInt32(dsfindid1.Tables[0].Rows[0]["iscomplete"]);

        }

        if (iscomplete == 1)
        {
            decimal tsktotalpercent = 100;
            decimal totlweitagemile = 0;
            decimal totlweitageproject = 0;

            string finsql01 = "select * from Pms_tbl_Addtask where id=" + retval + " and status=1  and archive=0";
            DataSet dsfindid01 = da1.GetDataSetInline(finsql01);

            // task percentage //
            string strstatusupdate = "update Pms_tbl_Addtask set iscompletepercentage='" + tsktotalpercent.ToString() + "' where id='" + retval + "' and status=1 and archive=0 and isnull(iscomplete,0)=1";
            DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

            // Milestone percentage //
            string finsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "'   and status=1  and archive=0";
            DataSet dsfindmile = da1.GetDataSetInline(finsqlmile);
            for (int j = 0; j < dsfindmile.Tables[0].Rows.Count; j++)
            {
                decimal sumweitage = 0;
                decimal subpercentageemile = 0;
                sumweitage = Convert.ToDecimal(dsfindmile.Tables[0].Rows[j]["Weightage"].ToString());
                subpercentageemile = Convert.ToDecimal(dsfindmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                totlweitagemile += (sumweitage * subpercentageemile) / 100;

            }

            string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile.ToString() + "' where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and id='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da1.GetDataSetInline(finsql4);


            // project percentage //
            string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "'   and status=1  and archive=0";
            DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
            for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
            {
                decimal sumweitageprojt = 0;
                decimal subpercentageeproject = 0;
                sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

            }

            string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
            DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);




        }
        else
        {
            decimal tsktotalpercent = 0;
            decimal totlweitagemile = 0;
            decimal totlweitageproject = 0;

            string finsql01 = "select * from Pms_tbl_Addtask where id=" + retval + " and status=1  and archive=0";
            DataSet dsfindid01 = da1.GetDataSetInline(finsql01);


            if (dsfindid01.Tables[0].Rows.Count > 0)
            {
                if (dsfindid01.Tables[0].Rows[0]["issubtask"].ToString() == "1")
                {

                    string finsql211 = "select isnull(sum(weightage),0)Wattagetasksum from Pms_tbl_Addsubtask where   task='" + retval + "'  and status=1 and isnull(iscomplete,0)=1 and archive=0";
                    DataSet dsfindid211 = da1.GetDataSetInline(finsql211);
                    if (dsfindid211.Tables[0].Rows.Count > 0)
                    {
                        string strstatusupdate = "update Pms_tbl_Addtask set iscompletepercentage='" + dsfindid211.Tables[0].Rows[0]["Wattagetasksum"] + "' where id='" + retval + "' and status=1 and archive=0";
                        DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);
                    }
                }
                else
                {
                    string strstatusupdate = "update Pms_tbl_Addtask set iscompletepercentage='0' where id='" + retval + "' and status=1 and archive=0";
                    DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                }

            }

            // task percentage //
            //string strstatusupdate = "update Pms_tbl_Addtask set iscompletepercentage='" + tsktotalpercent.ToString() + "' where id='" + retval + "' and status=1 and archive=0 and isnull(iscomplete,0)=0";
            //DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

            // Milestone percentage //
            string finsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "'   and status=1  and archive=0";
            DataSet dsfindmile = da1.GetDataSetInline(finsqlmile);
            for (int j = 0; j < dsfindmile.Tables[0].Rows.Count; j++)
            {
                decimal sumweitage = 0;
                decimal subpercentageemile = 0;
                sumweitage = Convert.ToDecimal(dsfindmile.Tables[0].Rows[j]["Weightage"].ToString());
                subpercentageemile = Convert.ToDecimal(dsfindmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                totlweitagemile += (sumweitage * subpercentageemile) / 100;

            }

            string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile.ToString() + "' where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and id='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da1.GetDataSetInline(finsql4);

            // project percentage //
            string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "'   and status=1  and archive=0";
            DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
            for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
            {
                decimal sumweitageprojt = 0;
                decimal subpercentageeproject = 0;
                sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

            }

            string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
            DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);



        }




        return retval;

    }
    [WebMethod]
    public static string SaveSubTask(SubSubtaskdata subtaskdetails)
    {
        function fns = new function();
        string retval = "0";
        DataAccess da1 = new DataAccess();
        string editid = subtaskdetails.id;
        string entrytype = "Insert";
        if (editid != "" && editid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[33];
        prm[0] = new SqlParameter("@id", editid);
        prm[1] = new SqlParameter("@ProjectId", subtaskdetails.ProjectId);
        prm[2] = new SqlParameter("@Milestoneid", subtaskdetails.Milestoneid);
        prm[3] = new SqlParameter("@tasklist", subtaskdetails.tasklist);
        prm[4] = new SqlParameter("@task", subtaskdetails.task);
        prm[5] = new SqlParameter("@Subtaskname", subtaskdetails.Subtaskname);
        prm[6] = new SqlParameter("@SubTaskDescription", subtaskdetails.SubTaskDescription);
        prm[7] = new SqlParameter("@owner", subtaskdetails.owner);
        if (subtaskdetails.Startdate == string.Empty)
        {
            prm[8] = new SqlParameter("@Startdate", DBNull.Value);
        }
        else
        {
            prm[8] = new SqlParameter("@Startdate", subtaskdetails.Startdate);
        }
        if (subtaskdetails.Targetdate == string.Empty)
        {
            prm[9] = new SqlParameter("@Targetdate", DBNull.Value);
        }
        else
        {
            prm[9] = new SqlParameter("@Targetdate", subtaskdetails.Targetdate);
        }


        if (subtaskdetails.StartTime == string.Empty)
        {
            prm[10] = new SqlParameter("@StartTime", DBNull.Value);
        }
        else
        {
            prm[10] = new SqlParameter("@StartTime", subtaskdetails.StartTime);
        }


        if (subtaskdetails.Timeline == string.Empty)
        {
            prm[11] = new SqlParameter("@Timeline", DBNull.Value);
        }
        else
        {
            prm[11] = new SqlParameter("@Timeline", subtaskdetails.Timeline);
        }

        prm[12] = new SqlParameter("@TaskDuration", subtaskdetails.TaskDuration);

        prm[13] = new SqlParameter("@P_Status", subtaskdetails.P_Status);
        prm[14] = new SqlParameter("@Priority", subtaskdetails.Priority);

        if (subtaskdetails.Budget == string.Empty)
        {
            prm[15] = new SqlParameter("@Budget", DBNull.Value);
        }
        else
        {
            prm[15] = new SqlParameter("@Budget", subtaskdetails.Budget);
        }

        if (subtaskdetails.Actual == string.Empty)
        {
            prm[16] = new SqlParameter("@Actual", DBNull.Value);
        }
        else
        {
            prm[16] = new SqlParameter("@Actual", subtaskdetails.Actual);
        }


        prm[17] = new SqlParameter("@Reminder", subtaskdetails.Reminder);

        if (subtaskdetails.ReminderType == string.Empty)
        {
            prm[18] = new SqlParameter("@ReminderType", DBNull.Value);
        }
        else
        {
            prm[18] = new SqlParameter("@ReminderType", subtaskdetails.ReminderType);
        }

        if (subtaskdetails.Days == string.Empty)
        {
            prm[19] = new SqlParameter("@Days", DBNull.Value);
        }
        else
        {
            prm[19] = new SqlParameter("@Days", subtaskdetails.Days);

        }
        if (subtaskdetails.Time == string.Empty)
        {
            prm[20] = new SqlParameter("@Time", DBNull.Value);
        }
        else
        {
            prm[20] = new SqlParameter("@Time", subtaskdetails.Time);
        }
        if (subtaskdetails.Notifyusers == string.Empty)
        {
            prm[21] = new SqlParameter("@Notifyusers", DBNull.Value);
        }
        else
        {
            prm[21] = new SqlParameter("@Notifyusers", subtaskdetails.Notifyusers);
        }

        if (subtaskdetails.ChooseDate == string.Empty)
        {
            prm[22] = new SqlParameter("@ChooseDate", DBNull.Value);
        }
        else
        {
            prm[22] = new SqlParameter("@ChooseDate", subtaskdetails.ChooseDate);
        }

        prm[23] = new SqlParameter("@Occurrence", subtaskdetails.Occurrence);
        prm[24] = new SqlParameter("@tagto", subtaskdetails.tagto);
        prm[25] = new SqlParameter("@team", subtaskdetails.team);
        prm[26] = new SqlParameter("@Stage", subtaskdetails.Stage);
        prm[27] = new SqlParameter("@Weightage", subtaskdetails.Weightage);
        prm[28] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[29] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[30] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[31] = new SqlParameter("@type", entrytype);
        prm[32] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[32].Direction = ParameterDirection.ReturnValue;
        int retval1 = da1.executeDMLQuery("pms_Add_Subtask_usp", prm);
        retval = retval1.ToString();



        // iscomplete update //

        string striscomplt = "select  isnull(iscomplete,0) as iscomplete from Pms_tbl_Projectstatus where status = 1 and archive = 0  and id='" + subtaskdetails.P_Status + "'";
        DataSet dsiscomplt = da1.GetDataSetInline(striscomplt);
        if (dsiscomplt.Tables[0].Rows.Count > 0)
        {
            int iscompletestatus = 0;
            iscompletestatus = Convert.ToInt32(dsiscomplt.Tables[0].Rows[0]["iscomplete"]);

            string strstatusupdateiscom = "update Pms_tbl_Addsubtask set iscomplete='" + iscompletestatus.ToString() + "' where id = " + retval + " and status=1 and archive=0";
            DataSet dshourminiscom = da1.GetDataSetInline(strstatusupdateiscom);

        }

        //   iscomplete update  



        int checkweitage = 0;
        decimal wetagecount = 0;
        decimal totalwetage = 0;
        string strweightage = "select isnull(checkTaskWeightage ,0) as checkTaskWeightage  from pms_tbl_addtask where id='" + subtaskdetails.task + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
        DataSet dsweitagecheck = da1.GetDataSetInline(strweightage);
        if (dsweitagecheck.Tables[0].Rows.Count > 0)
        {

            checkweitage = Convert.ToInt32(dsweitagecheck.Tables[0].Rows[0]["checkTaskWeightage"]);

        }

        if (checkweitage == 1)
        {
            string strweightagecount = "select count (*) as WeitageCount from Pms_tbl_Addsubtask where task='" + subtaskdetails.task + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
            DataSet dsweitagecount = da1.GetDataSetInline(strweightagecount);
            if (dsweitagecount.Tables[0].Rows.Count > 0)
            {

                wetagecount = Convert.ToInt32(dsweitagecount.Tables[0].Rows[0]["WeitageCount"]);

            }

            totalwetage = 100 / wetagecount;
            string strstatusupdate = "update Pms_tbl_Addsubtask set Weightage='" + fns.ToFormattedCurrencyString(totalwetage.ToString()) + "' where task='" + subtaskdetails.task + "' and status=1 and archive=0";
            DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

        }


        int iscomplete = 0;
        string finsql1 = "select  isnull(iscomplete,0) as iscomplete,* from Pms_tbl_Addsubtask where id=" + retval + " and status=1  and archive=0";
        DataSet dsfindid1 = da1.GetDataSetInline(finsql1);
        if (dsfindid1.Tables[0].Rows.Count > 0)
        {

            iscomplete = Convert.ToInt32(dsfindid1.Tables[0].Rows[0]["iscomplete"]);

        }

        if (iscomplete == 1)
        {
            decimal subtsktotalpercent = 100;
            decimal totlweitagemile = 0;
            decimal totlweitageproject = 0;

            string finsql01 = "select * from Pms_tbl_Addsubtask where id=" + retval + " and status=1  and archive=0";
            DataSet dsfindid01 = da1.GetDataSetInline(finsql01);

            string finsql2 = "select isnull(sum(weightage),0)Wattagetasksum from Pms_tbl_Addsubtask where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "' and task='" + dsfindid01.Tables[0].Rows[0]["task"] + "'  and status=1 and isnull(iscomplete,0)=1 and archive=0";
            DataSet dsfindid2 = da1.GetDataSetInline(finsql2);

            //sub task percentage //
            string strstatusupdate = "update Pms_tbl_Addsubtask set iscompletepercentage='" + subtsktotalpercent.ToString() + "' where id='" + retval + "' and status=1 and archive=0 and isnull(iscomplete,0)=1";
            DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

            // task percentage //
            string finsql3 = "update Pms_tbl_Addtask set iscompletepercentage='" + dsfindid2.Tables[0].Rows[0]["Wattagetasksum"] + "' where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "' and id='" + dsfindid01.Tables[0].Rows[0]["task"] + "' and status=1 and archive=0";
            DataSet dsfindid3 = da1.GetDataSetInline(finsql3);


            // Milestone percentage //
            string findsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask  where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1  and archive=0";
            DataSet dsfindidmile = da1.GetDataSetInline(findsqlmile);
            for (int j = 0; j < dsfindidmile.Tables[0].Rows.Count; j++)
            {
                decimal sumweitage = 0;
                decimal subpercentageemile = 0;
                sumweitage = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["Weightage"].ToString());
                subpercentageemile = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                totlweitagemile += (sumweitage * subpercentageemile) / 100;

            }

            string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile + "' where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and id='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da1.GetDataSetInline(finsql4);



            // project percentage //
            string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "'    and status=1  and archive=0";
            DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
            for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
            {
                decimal sumweitageprojt = 0;
                decimal subpercentageeproject = 0;
                sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

            }

            string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
            DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);






        }

        else
        {
            decimal subtsktotalpercent = 0;
            decimal totlweitagemile = 0;
            decimal totlweitageproject = 0;

            string finsql01 = "select * from Pms_tbl_Addsubtask where id=" + retval + " and status=1 and archive=0";
            DataSet dsfindid01 = da1.GetDataSetInline(finsql1);

            string finsql2 = "select isnull(sum(weightage),0)Wattagetasksum from Pms_tbl_Addsubtask where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "' and task='" + dsfindid01.Tables[0].Rows[0]["task"] + "'  and status=1 and isnull(iscomplete,0)=1 and archive=0";
            DataSet dsfindid2 = da1.GetDataSetInline(finsql2);

            //Sub task percentage //
            string strstatusupdate = "update Pms_tbl_Addsubtask set iscompletepercentage='" + subtsktotalpercent.ToString() + "' where id='" + retval + "' and status=1 and archive=0 and isnull(iscomplete,0)=0";
            DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

            // task percentage //
            string finsql3 = "update Pms_tbl_Addtask set iscompletepercentage='" + dsfindid2.Tables[0].Rows[0]["Wattagetasksum"] + "' where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "' and id='" + dsfindid01.Tables[0].Rows[0]["task"] + "' and status=1 and archive=0";
            DataSet dsfindid3 = da1.GetDataSetInline(finsql3);


            // Milestone percentage //
            string findsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask  where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "'    and status=1  and archive=0";
            DataSet dsfindidmile = da1.GetDataSetInline(findsqlmile);
            for (int j = 0; j < dsfindidmile.Tables[0].Rows.Count; j++)
            {
                decimal sumweitage = 0;
                decimal subpercentageemile = 0;
                sumweitage = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["Weightage"].ToString());
                subpercentageemile = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                totlweitagemile += (sumweitage * subpercentageemile) / 100;

            }

            string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile + "' where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and id='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da1.GetDataSetInline(finsql4);


            // project percentage //
            string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "'   and status=1  and archive=0";
            DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
            for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
            {
                decimal sumweitageprojt = 0;
                decimal subpercentageeproject = 0;
                sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

            }

            string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
            DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);


        }

        return retval;

    }

    public class MultiTaskData
    {
        public string id { get; set; }
        public string ProjectId { get; set; }
        public string Milestoneid { get; set; }
        public string tasklist { get; set; }
        public string taskname { get; set; }
        public string taskdescripton { get; set; }
        public string owner { get; set; }
        public string Startdate { get; set; }
        public string Targetdate { get; set; }
        public string StartTime { get; set; }
        public string Timeline { get; set; }
        public string TaskDuration { get; set; }
        public string P_Status { get; set; }
        public string Priority { get; set; }
        public string Budget { get; set; }
        public string Actual { get; set; }
        public string Reminder { get; set; }
        public string ReminderType { get; set; }
        public string Days { get; set; }
        public string Time { get; set; }
        public string Notifyusers { get; set; }
        public string ChooseDate { get; set; }
        public string Occurrence { get; set; }
        public string tagto { get; set; }
        public string team { get; set; }
        public string Stage { get; set; }
        public string Weightage { get; set; }
        public string issubtask { get; set; }
        public string checkTaskWeightage { get; set; }
    }

    public class SubSubtaskdata
    {
        public string id { get; set; }
        public string ProjectId { get; set; }
        public string Milestoneid { get; set; }
        public string tasklist { get; set; }
        public string task { get; set; }
        public string Subtaskname { get; set; }
        public string SubTaskDescription { get; set; }
        public string owner { get; set; }
        public string Startdate { get; set; }
        public string Targetdate { get; set; }
        public string StartTime { get; set; }
        public string Timeline { get; set; }
        public string TaskDuration { get; set; }
        public string P_Status { get; set; }
        public string Priority { get; set; }
        public string Budget { get; set; }
        public string Actual { get; set; }
        public string Reminder { get; set; }
        public string Occurrence { get; set; }
        public string ReminderType { get; set; }
        public string Days { get; set; }
        public string Time { get; set; }
        public string Notifyusers { get; set; }
        public string ChooseDate { get; set; }
        public string tagto { get; set; }
        public string team { get; set; }
        public string Stage { get; set; }
        public string Weightage { get; set; }


    }
    public static string checkdependency(string strtype,string strid)
    {
        DataAccess da1 = new DataAccess();
        string ret = "0";
        string strsqlM = "select isnull(Isdone,0) Isdone from tbl_dependencyST where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and milestoneid>0 and  milestoneid='" + strid + "' and status=1 and archive=0 and isnull(Isdone,0)=0";
        if (strtype == "task")
        {
            strsqlM = "select  isnull(Isdone,0)  from tbl_dependencyST where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and taskid='" + strid + "'  and status=1 and archive=0  and isnull(Isdone,0)=0";
        }
        else if (strtype == "subtask")
        {
            strsqlM = "select  isnull(Isdone,0)  from tbl_dependencyST where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and subtaskid='" + strid + "'  and status=1 and archive=0  and isnull(Isdone,0)=0";
        }
        DataSet dschkD= da1.GetDataSetInline(strsqlM);
        if(dschkD.Tables[0].Rows.Count>0)
        {
            ret = "1";

        }

        return ret;

    }
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

                DataAccess da = new DataAccess();
                string strsql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus where  status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + statusid + "' ";
                DataSet dscode = da.GetDataSetInline(strsql);
                if (dscode.Tables[0].Rows.Count > 0)
                {
                    retval = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
                }
            }




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

            DataAccess da = new DataAccess();
            string strsql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus where  status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + statusid + "' ";
            DataSet dscode = da.GetDataSetInline(strsql);
            if (dscode.Tables[0].Rows.Count > 0)
            {
                retval = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
            }


        }
        // percentage update end  // 




        
        // return dscode.GetXml();
        return retval;
    }
     [WebMethod]
    public static string updatetimeline(string pid,string mid,string tid,string stid,string fromdate,string todate)
    {
        string retval = "0";
        DataAccess da = new DataAccess();
        string strupdate = "";
            if (stid!=""&&stid!="0")
            {
            strupdate = "update Pms_tbl_Addsubtask set StartDate='" + fromdate + "',Targetdate='" + todate + "' where id='" + stid + "' and status=1 and archive=0";

           }
            else if (tid != "" && tid != "0")
            {
            strupdate = "update Pms_tbl_Addtask set StartDate='" + fromdate + "',Targetdate='" + todate + "' where id='" + tid + "' and status=1 and archive=0";

           }
            else if (mid != "" && mid != "0")
            {
                 strupdate = "update Pms_tbl_AddMilestone set StartDate='" + fromdate + "',EndDate='" + todate + "' where id='" + mid + "' and status=1 and archive=0";
            
            }
        SaveLogText("strupdateA-" + strupdate);
        if (strupdate != "")
            {
              da.executeDMLQueryinline(strupdate);
            retval = "1";
            }
        return retval;
    }

    [WebMethod]
    public static string MileUpdatePriority_Status(string milstoneid = "", string Propertyid = "")
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (milstoneid != "" && milstoneid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", milstoneid);
        prm[1] = new SqlParameter("@Priority", Propertyid);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pmstbl_Priority_milstone_DtlsUpdate", prm);

        DataAccess da = new DataAccess();
        string strsql = "select Id,name,PrintColor from Pms_tbl_Priority where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + Propertyid + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            retval = dscode.Tables[0].Rows[0]["name"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
        }
        // return dscode.GetXml();
        return retval;
    }


    [WebMethod]
    public static string projectUpdatePriority_Status(string projectid = "", string Propertyid = "")
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (projectid != "" && projectid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", projectid);
        prm[1] = new SqlParameter("@Priority", Propertyid);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pmstbl_Priority_DeatilsUpdate_usp", prm);

        DataAccess da = new DataAccess();
        string strsql = "select Id,name,PrintColor from Pms_tbl_Priority  where  status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + Propertyid + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            retval = dscode.Tables[0].Rows[0]["name"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
        }
        // return dscode.GetXml();
        return retval;

    }

    [WebMethod]
    public static string UpdatePriority_Status(string taskid = "", string Propertyid = "")
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (taskid != "" && taskid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", taskid);
        prm[1] = new SqlParameter("@Priority", Propertyid);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pms_tbl_task_Priority_DeatilsUpdate", prm);

        DataAccess da = new DataAccess();
        string strsql = "select Id,name,PrintColor from Pms_tbl_Priority  where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + Propertyid + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            retval = dscode.Tables[0].Rows[0]["name"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
        }

        return retval;
    }

    [WebMethod]
    public static string sbtskUpdate_Status(string subtaskid = "", string statusid = "", string iscomplete = "")
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (subtaskid != "" && subtaskid != "0")
        {
            entrytype = "Update";
        }

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

        // percentage update start  // 
        if (iscomplete == "1")
        {
            decimal subtsktotalpercent = 100;
            decimal totlweitagemile = 0;
            decimal totlweitageproject = 0;

            string finsql1 = "select * from Pms_tbl_Addsubtask where id=" + subtaskid + " and status=1  and archive=0";
            DataSet dsfindid1 = da1.GetDataSetInline(finsql1);

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




        }
        else
        {
            decimal subtsktotalpercent = 0;
            decimal totlweitagemile = 0;
            decimal totlweitageproject = 0;

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



        }
        // percentage update end  // 


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



        DataAccess da = new DataAccess();
        string strsql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + statusid + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            retval = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
        }

        return retval;
    }

    [WebMethod]
    public static string sbtskUpdatePriority_Status(string taskid = "", string Propertyid = "")
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (taskid != "" && taskid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", taskid);
        prm[1] = new SqlParameter("@Priority", Propertyid);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pms_tbl_Subtask_Priority_DeatilsUpdate", prm);

        DataAccess da = new DataAccess();
        string strsql = "select Id,name,PrintColor from Pms_tbl_Priority  where  status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + Propertyid + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            retval = dscode.Tables[0].Rows[0]["name"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
        }

        return retval;

    }


    [WebMethod]
    public static string Update_Stagge(string taskid = "", string Stageid = "")
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (taskid != "" && taskid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", taskid);
        prm[1] = new SqlParameter("@Stage", Stageid);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pmstbl_Stage_DeatilsUpdate", prm);

        DataAccess da = new DataAccess();
        string strsql = "select id,name,PrintColor from Pms_tbl_Stage where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + Stageid + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            retval = dscode.Tables[0].Rows[0]["name"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
        }

        return retval;
    }


    [WebMethod]
    public static string sbtskUpdate_Stagge(string taskid = "", string Stageid = "")
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (taskid != "" && taskid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", taskid);
        prm[1] = new SqlParameter("@Stage", Stageid);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pmstbl_Subtask_Stage_DeatilsUpdate", prm);

        DataAccess da = new DataAccess();
        string strsql = "select id,name,PrintColor from Pms_tbl_Stage where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + Stageid + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            retval = dscode.Tables[0].Rows[0]["name"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
        }

        return retval;
    }





    [WebMethod]
    public static string UpdateExtendeddatedtls_subtask(string subtask_id = "", string extendStrtdate_subtask = "", string extendEnddate_subtask = "")
    {
        string retval = "";

        string estendleftdlydys = "";
        string estendstartmonthname = "";
        string estendendmonthname = "";
        double estendleftDays = 0;
        int estendststartdys = 0;
        int estendenddys = 0;
        double estendDays = 0;
        string extenddatebind = "";


        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (subtask_id != "" && subtask_id != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[7];
        prm[0] = new SqlParameter("@id", subtask_id);
        prm[1] = new SqlParameter("@StartDate", extendStrtdate_subtask);
        prm[2] = new SqlParameter("@TargetDate", extendEnddate_subtask);
        prm[3] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[4] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[5] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[6] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pmstbl_ExtnddatedateUpdate_subtask", prm);



        // extended timeleine 


        string strextend = "select id,extendStartdate as StartDate,extendEnddate as TargetDate from Pms_Tbl_Extended_Timeline where  status=1 and archive=0 and Type='subtask' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and subtaskid = '" + subtask_id + "'  ";
        DataSet dsstrextend = da1.GetDataSetInline(strextend);
        if (dsstrextend.Tables[0].Rows.Count > 0)
        {
            for (int k = 0; k < dsstrextend.Tables[0].Rows.Count; k++)
            {
                DateTime estendStartDate = Convert.ToDateTime(dsstrextend.Tables[0].Rows[k]["StartDate"].ToString() == "" ? DateTime.Now.ToString() : dsstrextend.Tables[0].Rows[k]["StartDate"].ToString());
                DateTime estendTargetdate = Convert.ToDateTime(dsstrextend.Tables[0].Rows[k]["TargetDate"].ToString() == "" ? DateTime.Now.ToString() : dsstrextend.Tables[0].Rows[k]["TargetDate"].ToString());
                TimeSpan estendobjTimeSpan = estendTargetdate - estendStartDate;
                estendDays = Convert.ToDouble(estendobjTimeSpan.TotalDays);
                estendDays = estendDays + 1;


                DateTime estendEnddate = Convert.ToDateTime(dsstrextend.Tables[0].Rows[k]["Targetdate"].ToString() == "" ? DateTime.Now.ToString() : dsstrextend.Tables[0].Rows[k]["Targetdate"].ToString());
                DateTime estendCurrentdate = DateTime.Now;
                TimeSpan estendobjtimeleftspn = estendEnddate - estendCurrentdate;
                estendleftDays = Convert.ToDouble(estendobjtimeleftspn.Days);
                estendleftDays = estendleftDays + 1;


                if (estendleftDays < 0)
                {
                    estendleftdlydys = "days delayed";
                }
                else
                {
                    estendleftdlydys = "days left";
                }

                //start days months  

                estendststartdys = estendStartDate.Day;
                int estendstrtmonth = estendStartDate.Month;
                if (estendstrtmonth == 1)
                {
                    estendstartmonthname = "Jan";
                }
                if (estendstrtmonth == 2)
                {
                    estendstartmonthname = "Feb";
                }
                if (estendstrtmonth == 3)
                {
                    estendstartmonthname = "Mar";
                }

                if (estendstrtmonth == 4)
                {
                    estendstartmonthname = "Apr";
                }

                if (estendstrtmonth == 5)
                {
                    estendstartmonthname = "May";
                }

                if (estendstrtmonth == 6)
                {
                    estendstartmonthname = "Jun";
                }

                if (estendstrtmonth == 7)
                {
                    estendstartmonthname = "Jul";
                }

                if (estendstrtmonth == 8)
                {
                    estendstartmonthname = "Aug";
                }

                if (estendstrtmonth == 9)
                {
                    estendstartmonthname = "Sep";
                }

                if (estendstrtmonth == 10)
                {
                    estendstartmonthname = "Oct";
                }

                if (estendstrtmonth == 11)
                {
                    estendstartmonthname = "Nov";
                }

                if (estendstrtmonth == 12)
                {
                    estendstartmonthname = "Dec";
                }

                //end days months 

                estendenddys = estendTargetdate.Day;
                int estendendmonth = estendTargetdate.Month;
                if (estendendmonth == 1)
                {
                    estendendmonthname = "Jan";
                }
                if (estendendmonth == 2)
                {
                    estendendmonthname = "Feb";
                }
                if (estendendmonth == 3)
                {
                    estendendmonthname = "Mar";
                }

                if (estendendmonth == 4)
                {
                    estendendmonthname = "Apr";
                }

                if (estendendmonth == 5)
                {
                    estendendmonthname = "May";
                }

                if (estendendmonth == 6)
                {
                    estendendmonthname = "Jun";
                }

                if (estendendmonth == 7)
                {
                    estendendmonthname = "Jul";
                }

                if (estendendmonth == 8)
                {
                    estendendmonthname = "Aug";
                }

                if (estendendmonth == 9)
                {
                    estendendmonthname = "Sep";
                }

                if (estendendmonth == 10)
                {
                    estendendmonthname = "Oct";
                }

                if (estendendmonth == 11)
                {
                    estendendmonthname = "Nov";
                }

                if (estendendmonth == 12)
                {
                    estendendmonthname = "Dec";
                }



                retval += "<a href = 'javascript:void(0);' class='timesheet-wrap'>";
                retval += "<div class='progress progress-xl bg-dark position-relative me-1' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + estendststartdys + " " + estendstartmonthname + " - " + estendenddys + " " + estendendmonthname + " (" + estendleftDays.ToString().Replace("-", "") + " " + estendleftdlydys + ")'>";
                retval += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 100%;' aria-valuemax='100'>" + estendDays + " days</div>";
                retval += "</div>";
                retval += "</a>";

            }


        }




        return retval;


    }

    [WebMethod]
    public static string sbtskSave_Owner(string id = "", string ownerdropdownid = "")
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string editid = id;
        string entrytype = "Update";
        if (editid != "" && editid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", id);
        prm[1] = new SqlParameter("@owner", ownerdropdownid);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pmstbl_subtaskOwnerDeatilsUpdate_usp", prm);

        DataAccess da = new DataAccess();
        string strsql = "select * from Pms_tbl_Addsubtask where  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and  id='" + editid + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {

            for (int i = 0; i < dscode.Tables[0].Rows.Count; i++)
            {
                string cntownr = "0";
                string usersdetailsowner = "";
                string ownertooltip = "";
                if (dscode.Tables[0].Rows[i]["owner"].ToString() != "")
                {
                    string[] arcnt = dscode.Tables[0].Rows[i]["owner"].ToString().Trim().Split(',');
                    cntownr = arcnt.Length.ToString();
                    DataSet dsusers = new DataSet();
                    if (dscode.Tables[0].Rows[i]["owner"].ToString().Trim() != "")
                    {
                        dsusers = da.GetDataSetInline("select id,DisplayName,fileupload1,SUBSTRING (DisplayName, 1, 2) as splinameone from tbl_employee where id in(" + dscode.Tables[0].Rows[i]["owner"].ToString().Trim() + ")");
                        if (dsusers.Tables[0].Rows.Count > 3)
                        {
                            cntownr = (Convert.ToInt32(cntownr) - 3).ToString();

                            // owner tool tip //
                            for (int l = 3; l < dsusers.Tables[0].Rows.Count; l++)
                            {
                                if (dsusers.Tables[0].Rows[l]["DisplayName"].ToString() != string.Empty)
                                {
                                    if (ownertooltip != string.Empty)
                                    {
                                        ownertooltip += "," + dsusers.Tables[0].Rows[l]["DisplayName"].ToString();
                                    }
                                    else
                                    {
                                        ownertooltip += dsusers.Tables[0].Rows[l]["DisplayName"].ToString();
                                    }
                                }

                            }

                            for (int k = 0; k < 3; k++)
                            {
                                if (dsusers.Tables[0].Rows[k]["fileupload1"].ToString() != "")
                                {
                                    usersdetailsowner += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title=" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + " data-bs-original-title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "'><img src = '" + dsusers.Tables[0].Rows[k]["fileupload1"] + "' alt='' class='rounded-circle avatar-xxs'></a>";
                                }
                                else
                                {
                                    usersdetailsowner += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover'" +
                 " data-bs-placement='top' title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "' data-bs-original-title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "'>";
                                    usersdetailsowner += @"<div class='avatar-xxs'><div class='avatar-title rounded-circle bg-light text-primary'>" + dsusers.Tables[0].Rows[k]["splinameone"].ToString() + "</div></div>";
                                    usersdetailsowner += @"</a>";
                                }
                            }

                        }
                        else
                        {
                            cntownr = "";
                            for (int k = 0; k < dsusers.Tables[0].Rows.Count; k++)
                            {
                                if (dsusers.Tables[0].Rows[k]["fileupload1"].ToString() != "")
                                {
                                    //img src='/assets/images/avatar-1.jpg'
                                    usersdetailsowner += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='' data-bs-original-title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "'><img src = '" + dsusers.Tables[0].Rows[k]["fileupload1"] + "' alt='' class='rounded-circle avatar-xxs'></a>";
                                }
                                else
                                {
                                    usersdetailsowner += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover'  data-bs-placement='top' title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "' data-bs-original-title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "'>";
                                    usersdetailsowner += @"<div class='avatar-xxs'>
                            <div class='avatar-title rounded-circle bg-light text-primary'>" + dsusers.Tables[0].Rows[k]["splinameone"].ToString() + "</div></div>";
                                    usersdetailsowner += @"</a>";

                                }
                            }
                        }

                    }
                }

                retval += usersdetailsowner;
                retval += "<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='" + ownertooltip + "' data-bs-original-title='more'>";

                if (cntownr != "" && cntownr != "0")
                {
                    retval += "<div class='avatar-xxs'>";
                    retval += "<div class='avatar-title rounded-circle'>";

                    retval += (cntownr + "+");
                    retval += "</div>";
                    retval += "</div>";
                }

                retval += "</a>";




            }


        }
        return retval;


    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetGanttData(string pid)
    {
        int chkadmin = 0;
        function fns = new function();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        string userid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();
        try
        {
            if (HttpContext.Current.Request.QueryString["empid"] != null)
            {
                userid = cmf.Decrypt(HttpContext.Current.Request.QueryString["empid"]).ToString();
            }
        }
        catch
        { }
        chkadmin = checkadmin(userid);
        var tasks = new List<object>();
        var links = new List<object>();
        string strproject = "select * from Pms_tbl_AddProject where id='" + pid + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        
        DataSet dsP = da.GetDataSetInline(strproject);
        if (dsP.Tables[0].Rows.Count > 0)
        {
            string strsqlM = "select ('M'+cast(Pms_tbl_AddMilestone.id as varchar)) id,id mid,Milestone text,cast(isnull(StartDate,GETDATE()) as date) start_date,ISNULL(TaskDuration, 0) duration,'0' progress from Pms_tbl_AddMilestone where ProjectId='" + pid + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by isnull(orderno,0)";
           
            DataSet dsM = da.GetDataSetInline(strsqlM);
            if (dsM.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsM.Tables[0].Rows.Count; i++)
                {
                    tasks.Add(new
                    {
                        id = dsM.Tables[0].Rows[i]["id"],
                        text = dsM.Tables[0].Rows[i]["text"],
                        start_date = Convert.ToDateTime(dsM.Tables[0].Rows[i]["start_date"]).ToString("dd-MM-yyyy"),
                        duration = dsM.Tables[0].Rows[i]["duration"],
                        progress = dsM.Tables[0].Rows[i]["progress"],
                        open = true
                    });
                    string strsqlT = "select ('T'+cast(Pms_tbl_Addtask.id as varchar)) id,id tid,taskname text,cast(isnull(StartDate,GETDATE()) as date) start_date,ISNULL(TaskDuration, 0) duration,'0' progress from Pms_tbl_Addtask where ProjectId='" + pid + "' and Milestoneid='" + dsM.Tables[0].Rows[i]["mid"] + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by isnull(orderno,0) ";
                    //SaveLogText("strproject-" + strsqlT);
                    DataSet dsT = da.GetDataSetInline(strsqlT);
                    if (dsT.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j < dsT.Tables[0].Rows.Count; j++)
                        {
                            tasks.Add(new
                            {
                                id = dsT.Tables[0].Rows[j]["id"],
                                text = dsT.Tables[0].Rows[j]["text"],
                                start_date = Convert.ToDateTime(dsT.Tables[0].Rows[j]["start_date"]).ToString("dd-MM-yyyy"),
                                duration = dsT.Tables[0].Rows[j]["duration"],
                                progress = dsT.Tables[0].Rows[j]["progress"],
                                parent = dsM.Tables[0].Rows[i]["id"],
                                open = true
                            });
                            string strsqlS = "select ('S'+cast(Pms_tbl_Addsubtask.id as varchar)) id ,id sid,Subtaskname text,cast(isnull(StartDate,GETDATE()) as date) start_date,ISNULL(TaskDuration, 0) duration,'0' progress from Pms_tbl_Addsubtask where task='" + dsT.Tables[0].Rows[j]["tid"].ToString() + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by isnull(orderno,0) ";
                            DataSet dsS = da.GetDataSetInline(strsqlS);
                            if (dsS.Tables[0].Rows.Count > 0)
                            {
                                for (int k = 0; k < dsS.Tables[0].Rows.Count; k++)
                                {
                                    tasks.Add(new
                                    {
                                        id = dsS.Tables[0].Rows[k]["id"],
                                        text = dsS.Tables[0].Rows[k]["text"],
                                        start_date = Convert.ToDateTime(dsS.Tables[0].Rows[k]["start_date"]).ToString("dd-MM-yyyy"),
                                        duration = dsS.Tables[0].Rows[k]["duration"],
                                        progress = dsS.Tables[0].Rows[k]["progress"],
                                        parent = dsT.Tables[0].Rows[j]["id"],
                                        open = true
                                    });
                                }

                            }


                        }

                    }
                }
            }
        }



        return new { data = tasks, links = links };
    }
    
}