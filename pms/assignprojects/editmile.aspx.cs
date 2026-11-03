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
public partial class editmile : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataAccess da = new DataAccess();
    public CommonFunction cmf = new CommonFunction();
    SqlParameter[] prm, prm1;
    public function fns = new function();
    public string Multiowner = "";
    public string Multiteam = "";
    public string strdisplay_close = "style='display:'";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (HttpContext.Current.Request.QueryString["ctype"] != null)
            {
                strdisplay_close = "style='display:none'";
            }
        }
        catch { }
        try
        {
            StartDatemile.Attributes["type"] = "date";
            StartDatemile.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";
            EndDatemile.Attributes["type"] = "date";
            EndDatemile.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";
            


        }
        catch
        {

        }
        if (Request.QueryString["pid"] != null)
        {
            bindmilestoneowner(Request.QueryString["pid"].ToString());
            bindmilestoneteam(Request.QueryString["pid"].ToString());
        }
       
        if (!IsPostBack)
        {
            bindprojectstatusmile();
            bindPriorityTypemile();
            bindProject();
            if (Request.QueryString["pid"] != null)
            {
                ProjectIdmile.SelectedValue = Request.QueryString["pid"].ToString();
            }
            //bindmilestone();
        }

    }
    protected void bindmilestoneowner(string pid = "")
    {
        string dataN = "";
        string strsql = "SELECT  Split.value as id,tbl_employee.DisplayName FROM Pms_tbl_AddProject AS project CROSS APPLY STRING_SPLIT(project.Owner, ',') as Split inner join tbl_employee on Split.value = tbl_employee.id where project.status = 1 and project.archive = 0 and project.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and project.id = '" + pid + "' ";

        DataSet dsN = da.GetDataSetInline(strsql);

        if (dsN.Tables[0].Rows.Count > 0)
        {

            for (int i1 = 0; i1 < dsN.Tables[0].Rows.Count; i1++)
            {
                if (dataN != string.Empty)
                {
                    dataN += ",{";
                    dataN += "\"id\": \"" + dsN.Tables[0].Rows[i1]["id"].ToString() + "\",\"";
                    dataN += "text\": \"" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "\"";
                    dataN += "}";
                }
                else
                {
                    dataN += "{";
                    dataN += "\"id\": \"" + dsN.Tables[0].Rows[i1]["id"].ToString() + "\",\"";
                    dataN += "text\": \"" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "\"";
                    dataN += "}";
                }
            }

        }
        Multiowner = dataN;

    }
    protected void bindmilestoneteam(string pid = "")
    {
        string dataN = "";
        string strsql = "SELECT  Split.value as id,tbl_employee.DisplayName FROM Pms_tbl_AddProject AS project CROSS APPLY STRING_SPLIT(project.team, ',') as Split inner join tbl_employee on Split.value = tbl_employee.id where project.status = 1 and project.archive = 0 and project.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and project.id = '" + pid + "' ";

        DataSet dsN = da.GetDataSetInline(strsql);

        if (dsN.Tables[0].Rows.Count > 0)
        {

            for (int i1 = 0; i1 < dsN.Tables[0].Rows.Count; i1++)
            {
                if (dataN != string.Empty)
                {
                    dataN += ",{";
                    dataN += "\"id\": \"" + dsN.Tables[0].Rows[i1]["id"].ToString() + "\",\"";
                    dataN += "text\": \"" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "\"";
                    dataN += "}";
                }
                else
                {
                    dataN += "{";
                    dataN += "\"id\": \"" + dsN.Tables[0].Rows[i1]["id"].ToString() + "\",\"";
                    dataN += "text\": \"" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "\"";
                    dataN += "}";
                }
            }

        }
        Multiteam = dataN;

    }
    protected void bindProject()
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

    protected void bindmilestone()
    {
        string strleadstatus = "select id,Milestone  from Pms_tbl_AddMilestone where ProjectId='" + ProjectIdmile.SelectedValue + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  order by id";
        if(ProjectIdmile.SelectedValue==""|| ProjectIdmile.SelectedValue=="0")
        {
            strleadstatus = "select id,Milestone  from Pms_tbl_AddMilestone where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  order by id";
        }
        DataSet ds = da.GetDataSetInline(strleadstatus);
        if (ds.Tables[0].Rows.Count > 0)
        {
            //cmf.BindDropDownList(Milestoneid, ds, "Milestone", "id");
            //ListItem li1 = new ListItem("", "0");
            //Milestoneid.Items.Insert(0, li1);
        }

    }

    protected void btnloadmile_Click(object sender, EventArgs e)
    {
        bindprojectstatusmile();
        bindPriorityTypemile();
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
    protected void Button1_Click(object sender, EventArgs e)
    {
        //try
        //{
        //   // ProjectId.SelectedValue = hdnProjectId.Value;
        //}
        //catch
        //{

        //}

        //try
        //{
        //    bindmilestone();
        //    //Milestoneid.SelectedValue = hdnMilestoneid.Value;

        //}
        //catch
        //{

        //}


        ////try
        ////{
        ////    Milestoneid.SelectedValue = hdnMilestoneid.Value;
        ////    bindmilestone();
        ////}
        ////catch
        ////{

        ////}




        //try
        //{
        //    //tasklist.SelectedValue = hdntasklist.Value;
        //    bindtasklist();
        //}
        //catch
        //{

        //}

        ////try
        ////{
        ////    owner.SelectedValue = hdnowner.Value;
        ////}
        ////catch
        ////{

        ////}

        //try
        //{
        //    //Startdate.Value = Convert.ToDateTime(hdnstartdate.Value).ToString("yyyy-MM-dd");
        //}
        //catch
        //{

        //}

        //try
        //{
        //    Targetdate.Value = Convert.ToDateTime(hdntargetdate.Value).ToString("yyyy-MM-dd");
        //}
        //catch
        //{

        //}


        //try
        //{

        //    string starttime = hdnstarttime.Value.Replace("AM", "").Replace("PM", "");
        //    StartTime.Value = Convert.ToDateTime(starttime).TimeOfDay.ToString();
        //}
        //catch
        //{

        //}

        //try
        //{

        //    string strtimeline = hdnTimeline.Value.Replace("AM", "").Replace("PM", "");
        //    Timeline.Value = Convert.ToDateTime(strtimeline).TimeOfDay.ToString();
        //}
        //catch
        //{

        //}


        //try
        //{
        //    P_Status.SelectedValue = hdnP_Status.Value;
        //}
        //catch
        //{

        //}

        //try
        //{
        //    Priority.SelectedValue = hdnPriority.Value;
        //}
        //catch
        //{

        //}

        ////try
        ////{
        ////    Multiteam = hdnteam.Value;
        ////}
        ////catch
        ////{

        ////}

        //try
        //{
        //    Reminder.SelectedValue = hdnreminder.Value;
        //}
        //catch
        //{

        //}

        //try
        //{
        //    ReminderType.SelectedValue = hdnReminderType.Value;
        //}
        //catch
        //{

        //}

        //try
        //{
        //    //Time.Value = Convert.ToDateTime(hdnTime.Value).ToString("yyyy-MM-dd");
        //    string strtim = hdnTime.Value.Replace("AM", "").Replace("PM", "");
        //    Time.Value = Convert.ToDateTime(strtim).TimeOfDay.ToString();
        //}
        //catch
        //{

        //}

        //try
        //{
        //    Multiusers = hdnNotifyusers.Value;
        //}
        //catch
        //{

        //}

        //try
        //{
        //    ChooseDate.Value = Convert.ToDateTime(hdnChooseDate.Value).ToString("yyyy-MM-dd");
        //}
        //catch
        //{

        //}


        //try
        //{
        //    Stage.SelectedValue = hdnstage.Value;
        //}
        //catch
        //{

        //}







    }
   




    protected void bindtasklist()
    {
        //string strleadstatus = "select id,TaskList  from Pms_tbl_Addtasklist where  ProjectId='" + ProjectId.SelectedValue + "' and Milestone='" + Milestoneid.SelectedValue + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  order by id";
        //DataSet ds = da.GetDataSetInline(strleadstatus);
        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    cmf.BindDropDownList(tasklist, ds, "TaskList", "id");
        //    ListItem li1 = new ListItem("", "0");
        //    tasklist.Items.Insert(0, li1);
        //}

    }
    protected void btnload_Click(object sender, EventArgs e)
    {
        //bindprojectstatus();
        //bindPriorityType();
        //bindStage();
    }

    protected void btnuploadimg1_Click(object sender, EventArgs e)
    {

        if (Uploaddrawing1.HasFile)
        {

            string ext = System.IO.Path.GetExtension(this.Uploaddrawing1.PostedFile.FileName);
            //Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('"+ext+"')", true);
            if (ext == ".jpg" || ext == ".png" || ext == ".gif" || ext == ".jpeg" || ext == ".bmp")
            {
                string filename = Uploaddrawing1.FileName;
                chk:
                filename = filename + "_m_" + DateTime.Now.ToString("yyyyMMdd_hhmmss");
                if (File.Exists(Server.MapPath("/MileImages/" + filename)))
                {
                    goto chk;
                }
                Uploaddrawing1.SaveAs(Server.MapPath("/MileImages/" + filename));
                hdnUploaddrawing1.Value = filename;

            }
            else
            {

                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('Please choose only .jpg, .png, .bmp and .gif image types!')", true);


            }
        }


    }
}