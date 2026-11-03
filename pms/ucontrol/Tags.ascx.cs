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

public partial class ucontrol_Tags : System.Web.UI.UserControl
{
    DataSet ds = new DataSet();
    DataAccess da = new DataAccess();
    public CommonFunction cmf = new CommonFunction();
    SqlParameter[] prm, prm1;
    public function fns = new function();
    public string Milestonedata = "";
    public string Multiteam = "";
    public string MultiOwner = "";
    public string Multichangeowner = "";
    public string Multiaddteam = "";
    public string MultiDependancy = "";
    public string tagsname = "";
    public string tagscolorname = "";
  

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {

            bindProject();
            tagsprojectbind();



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

                    
                }
            }
            catch
            {

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


    protected void tagsprojectbind()
    {
        string strleadstatus = "select id,ProjectName  from Pms_tbl_AddProject where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  order by id";
        DataSet ds = da.GetDataSetInline(strleadstatus);
        if (ds.Tables[0].Rows.Count > 0)
        {
            cmf.BindDropDownList(ddltagsproject, ds, "ProjectName", "id");
            ListItem li1 = new ListItem("All Project", "0");
            ddltagsproject.Items.Insert(0, li1);
        }


    }


    protected void Projectnamecolor()
    {
        string strleadstatus = "select top(1) *   from Pms_tbl_Addtags where status=1 and archive=0 and ProjectId = '"+ Request.QueryString["id"].ToString() + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  order by id";
        DataSet ds = da.GetDataSetInline(strleadstatus);
        if (ds.Tables[0].Rows.Count > 0)
        {
            tagsname = ds.Tables[0].Rows[0]["Tags"].ToString();
          //  tagscolorname = ds.Tables[0].Rows[0]["TagsColor"].ToString();

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

       

    }


    protected void Button2_Click(object sender, EventArgs e)
    {
        try
        {
          
            ddltagsproject.SelectedValue = hdntagsproject.Value;

        }
        catch
        {

        }

    }

    protected void ddltagsproject_SelectedIndexChanged(object sender, EventArgs e)
    {

        //string strleadstatus = "select id,ProjectName from Pms_tbl_AddProject where id='" + ddltagsproject.SelectedValue + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by id";
        //DataSet ds = da.GetDataSetInline(strleadstatus);
        //if (ds.Tables[0].Rows.Count > 0)
        //{

        //    cmf.BindDropDownList(ddltagsproject, ds, "ProjectName", "id");
        //    ListItem li1 = new ListItem("All Project", "0");
        //    ddltagsproject.Items.Insert(0, li1);

        //}

    }




}


