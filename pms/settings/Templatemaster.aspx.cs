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
public partial class Templatemaster : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataAccess da = new DataAccess();
    public CommonFunction cmf = new CommonFunction();
    SqlParameter[] prm, prm1;
    public function fns = new function();
    public string projectdata = "";
    public string Multiteam = "";
    public string MultiOwner = "";
    public string Multichangeowner = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            bindproject();

        }
    }
    protected void bindproject()
    {
        DataSet ds = new DataSet();
        int isadmin = 0;
        string strsql = "select Manage_Profile.* from Manage_Profile inner join tbl_employee on Manage_Profile.id=cast(tbl_employee.roleid as int) where tbl_employee.id='" + HttpContext.Current.Request.Cookies["user"]["user_id"] + "'  and Manage_Profile.status=1 and Manage_Profile.archive=0";
        DataSet dsadminchk = da.GetDataSetInline(strsql);
        if (dsadminchk.Tables[0].Rows.Count > 0)
        {
            if (dsadminchk.Tables[0].Rows[0]["id"].ToString() == "1")
            {
                isadmin = 1;

            }

        }

        if (isadmin == 1)
        {
            SqlParameter[] prm = new SqlParameter[1];
            prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            ds = da.GetDataSet("Pms_select_tblprojectall_dashbord", prm);

        }
        else
        {
            SqlParameter[] prm = new SqlParameter[2];
            prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[1] = new SqlParameter("@owner", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            ds = da.GetDataSet("Pms_select_tblprojectall_dashbord_ownerwise", prm);

        }
        if (ds.Tables[0].Rows.Count > 0)
        {
            cmf.BindDropDownList(ddlproject, ds, "ProjectName", "id");
            ListItem li1 = new ListItem("", "0");
            ddlproject.Items.Insert(0, li1);
        }


    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    Type.SelectedValue = hdntype.Value;
        //}
        //catch
        //{

        //}
    }
    [WebMethod]
    public static void Savetemplates(templates templatedetails)
    {
        DataAccess da1 = new DataAccess();
        string editid = templatedetails.id;
        int chkdefault =0;       
        string entrytype = "INSERT";
        if (editid != "" && editid != "0")
        {
            entrytype = "UPDATE";
        }
        SqlParameter[] prm = new SqlParameter[7];
        prm[0] = new SqlParameter("@id", editid);
        prm[1] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@templatename", templatedetails.templatename);
        prm[4] = new SqlParameter("@pid", templatedetails.pid);       
        prm[5] = new SqlParameter("@isdefault", (templatedetails.isdefault==""?"0": templatedetails.isdefault));
        prm[6] = new SqlParameter("@action", entrytype);
        da1.executeDMLQuery("usp_pms_projecttemplates_all", prm);
    }




    [WebMethod]
    public static string bindTemplateData()
    {
        string projectdata = "";
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        SqlParameter[] prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[1] = new SqlParameter("@action", "GETBY_REGID");
        DataSet ds = da.GetDataSet("usp_pms_projecttemplates_all", prm);

        projectdata += "<table cellpadding='0' cellspacing='0' border='0'width=100% class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm datatable'>";
        projectdata += "<thead>";
        projectdata += "<tr>";

        projectdata += "<th>S.No</th>";
        projectdata += "<th>Template Name</th>";
        projectdata += "<th>Copy From</th>";
        projectdata += "<th>Action</th>";
        projectdata += "</tr>";
        projectdata += "</thead>";
        projectdata += "<tbody>";
        if (ds.Tables[0].Rows.Count > 0)
        {
            
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                projectdata += "<tr>";


                projectdata += "<td valign='top'>";
                projectdata += (i + 1);
                projectdata += "</td>";

                projectdata += "<td valign='top'>";
                projectdata += ds.Tables[0].Rows[i]["templatename"].ToString();
                projectdata += "</td>";
                projectdata += "<td valign='top'>";
                projectdata += ds.Tables[0].Rows[i]["ProjectName"].ToString();
                projectdata += "</td>";


                projectdata += " <td><a href='javascript:void(0);' Class='btn btn-info btn-sm' type='button' onclick='Binddetails(" + ds.Tables[0].Rows[i]["id"].ToString() + ")' data-bs-toggle='offcanvas' data-bs-target='#addproject'><i class='ri-edit-line'></i></a>";
                projectdata += "&nbsp;&nbsp;<a href='javascript:void(0);' Class='btn btn-danger btn-sm' type='button' onclick='deletedata(" + ds.Tables[0].Rows[i]["id"].ToString() + ")'><i class='ri-delete-bin-5-line'></i></a>";
                projectdata += "</td>";



                projectdata += "</tr>";
            }
        }

        projectdata += "</tbody>";
        projectdata += "</table>";

        return projectdata;
    }

    


    [WebMethod]
    public static string Bindtemplatedetail(int RecordId = 0)
    {
        DataAccess da = new DataAccess();
        DataSet ds;
        string strsql = "select id,isnull(isdefault,0) as isdefault,templatename,pid from pms_tbl_projecttemplates where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
        ds = da.GetDataSetInline(strsql);

        return ds.GetXml();
    }

    [WebMethod]
    public static string Deleteprojectdata(int RecordId = 0)
    {
        string retval = "0";
        DataAccess da = new DataAccess();
        DataSet ds;
        string strsql = "update  pms_tbl_projecttemplates set  status=0,archive=1  where  id='" + RecordId + "' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        ds = da.GetDataSetInline(strsql);

        return retval;
    }
}
public class templates
{
    public string id { get; set; }
    public string templatename { get; set; }
    public string pid { get; set; }
    public string isdefault { get; set; }
}