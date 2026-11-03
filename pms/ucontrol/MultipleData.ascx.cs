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

public partial class ucontrol_MultipleData : System.Web.UI.UserControl
{
    DataSet ds = new DataSet();
    DataAccess da = new DataAccess();
    public CommonFunction cmf = new CommonFunction();
    SqlParameter[] prm, prm1;
    public function fns = new function();
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


    public string changeownermltitsk = "";
    public string teamrmltitsk = "";
    public string Notifymultitsk = "";
    public string setmilstone = "";
    public string strshow1 = "none";
    public string strshow2 = "none";
    public int chkmilstone = 1;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Startdate.Attributes["type"] = "date";
            Startdate.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";
            Targetdate.Attributes["type"] = "date";
            Targetdate.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";
            ChooseDate.Attributes["type"] = "date";
            ChooseDate.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";
            StartDatemile.Attributes["type"] = "date";
            StartDatemile.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";
            EndDatemile.Attributes["type"] = "date";
            EndDatemile.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";
            Extendatestrt_task.Attributes["type"] = "date";
            Extendatestrt_task.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";
            Extendateend_task.Attributes["type"] = "date";
            Extendateend_task.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";
            mileExtendatestrt.Attributes["type"] = "date";
            mileExtendatestrt.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";
            mileExtendateend.Attributes["type"] = "date";
            mileExtendateend.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";
            Extendatestrt_subtask.Attributes["type"] = "date";
            Extendatestrt_subtask.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";
            Extendateend_subtask.Attributes["type"] = "date";
            Extendateend_subtask.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";


            Startdatesubtask.Attributes["type"] = "date";
            Startdatesubtask.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";
            Targetdatesubtask.Attributes["type"] = "date";
            Targetdatesubtask.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";
            ChooseDatesubtask.Attributes["type"] = "date";
            ChooseDatesubtask.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";
            StartTimesubtask.Attributes["type"] = "date";
            StartTimesubtask.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";
            Timelinesubtask.Attributes["type"] = "date";
            Timelinesubtask.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";



            StartTime.Attributes["type"] = "time";
            StartTime.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";
            Timeline.Attributes["type"] = "time";
            Timeline.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";
            Time.Attributes["type"] = "time";
            Time.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";
            Timesubtask.Attributes["type"] = "time";
            Timesubtask.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";


        }
        catch
        {

        }
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["id"] != null)
            {
                bindtaskdataP(Request.QueryString["id"].ToString());
            }

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

                        setmilstone += @" $(function() {$('#showtrM_" + ds1.Tables[0].Rows[j]["id"].ToString() + "').on('click', function() { $('.taskshow_" + ds1.Tables[0].Rows[j]["id"].ToString() + "').toggle();  });});";

                        setmilstone += @" $('#showtrM_" + ds1.Tables[0].Rows[j]["id"].ToString() + "').click(function() { $(this).toggleClass('shown');});";

                        #region task add
                        string strstask = "select * from Pms_tbl_Addtask where   ProjectId='" + dsproject.Tables[0].Rows[P]["id"].ToString() + "' and Milestoneid='" + ds1.Tables[0].Rows[j]["id"].ToString() + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                        DataSet ds = da.GetDataSetInline(strstask);
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {


                                setmilstone += @" $(function() {$('#showtasktr_" + ds.Tables[0].Rows[i]["id"].ToString() + "').on('click', function() {$('.subtaskshow_" + ds.Tables[0].Rows[i]["id"].ToString() + "').toggle();  });});";
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

        HtmlAnchor[] linka = new[] { multi0, multi1, multi2, multi3, multi4, multi5, multi6, multi7, multi8, multi9, multi10, multi11, multi12, multi13, multi14,multi15 };

        HiddenField[] hdn = new[] { hdn0, hdn1, hdn2, hdn3, hdn4, hdn5, hdn6, hdn7, hdn8, hdn9, hdn10, hdn11, hdn12, hdn13, hdn14 ,hdn15 };

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

}

