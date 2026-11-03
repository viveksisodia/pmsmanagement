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
public partial class edittask : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataAccess da = new DataAccess();
    public CommonFunction cmf = new CommonFunction();
    SqlParameter[] prm, prm1;
    public function fns = new function();
    public string Multiowner = "";
    public string Multiteam = "";
    public string Multiusers = "";
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
            Startdate.Attributes["type"] = "date";
            Startdate.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";
            Targetdate.Attributes["type"] = "date";
            Targetdate.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";
            ChooseDate.Attributes["type"] = "date";
            ChooseDate.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";          

            
            StartTime.Attributes["type"] = "time";
            StartTime.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";
            Timeline.Attributes["type"] = "time";
            Timeline.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";
            Time.Attributes["type"] = "time";
            Time.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";
           


        }
        catch
        {

        }
        if (Request.QueryString["mid"] != null)
        {
            bindmilestoneowner(Request.QueryString["mid"].ToString());
            bindmilestoneteam(Request.QueryString["mid"].ToString());
        }
        bindMultiusers(); ;
        if (!IsPostBack)
        {
            bindprojectstatus();
            bindPriorityType();
            bindStage();
            bindProject();

            if (Request.QueryString["pid"] != null)
            {
                ProjectId.SelectedValue = Request.QueryString["pid"].ToString();
            }
            bindmilestone();

        }

    }
    protected void bindmilestoneowner(string mid = "")
    {
        string dataN = "";
        string strsql = "SELECT  Split.value as id,tbl_employee.DisplayName FROM Pms_tbl_AddMilestone AS milestone CROSS APPLY STRING_SPLIT(milestone.Owner, ',') as Split inner join tbl_employee on Split.value = tbl_employee.id where milestone.status = 1 and milestone.archive = 0 and milestone.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and milestone.id = '" + mid + "' ";
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
        string strsql = "SELECT  Split.value as id,tbl_employee.DisplayName FROM Pms_tbl_AddMilestone AS project CROSS APPLY STRING_SPLIT(project.team, ',') as Split inner join tbl_employee on Split.value = tbl_employee.id where project.status = 1 and project.archive = 0 and project.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and project.id = '" + pid + "' ";

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
            cmf.BindDropDownList(ProjectId, ds, "ProjectName", "id");
            ListItem li1 = new ListItem("", "0");
            ProjectId.Items.Insert(0, li1);
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
    protected void bindmilestone()
    {
        string strleadstatus = "select id,Milestone  from Pms_tbl_AddMilestone where ProjectId='" + ProjectId.SelectedValue + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  order by id";
        if(ProjectId.SelectedValue==""|| ProjectId.SelectedValue=="0")
        {
            strleadstatus = "select id,Milestone  from Pms_tbl_AddMilestone where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  order by id";
        }
        DataSet ds = da.GetDataSetInline(strleadstatus);
        if (ds.Tables[0].Rows.Count > 0)
        {
            cmf.BindDropDownList(Milestoneid, ds, "Milestone", "id");
            ListItem li1 = new ListItem("", "0");
            Milestoneid.Items.Insert(0, li1);
        }

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
            owner.SelectedValue = hdnowner.Value;
        }
        catch
        {

        }

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
            Time.Value = Convert.ToDateTime(hdnTime.Value).ToString("yyyy-MM-dd");
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
    protected void btnload_Click(object sender, EventArgs e)
    {
        bindprojectstatus();
        bindPriorityType();
        bindStage();
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
    protected void ProjectId_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindmilestone();

    }

    protected void Milestoneid_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnuploadimg_Click(object sender, EventArgs e)
    {

        if (Uploaddrawing.HasFile)
        {

            string ext = System.IO.Path.GetExtension(this.Uploaddrawing.PostedFile.FileName);
            //Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('"+ext+"')", true);
            if (ext == ".jpg" || ext == ".png" || ext == ".gif" || ext == ".jpeg" || ext == ".bmp")
            {
                string filename = Uploaddrawing.FileName;
                chk:
                filename = filename + "_t_" + DateTime.Now.ToString("yyyyMMdd_hhmmss");
                SaveLogText("selecfile=" + filename);
                if (File.Exists(Server.MapPath("/TaskImages/" + filename)))
                {
                    goto chk;
                }
                Uploaddrawing.SaveAs(Server.MapPath("/TaskImages/" + filename));
                hdnUploaddrawing.Value = filename;

            }
            else
            {

                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('Please choose only .jpg, .png, .bmp and .gif image types!')", true);


            }
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
}