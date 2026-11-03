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

public partial class AddTags : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataAccess da = new DataAccess();
    public CommonFunction cmf = new CommonFunction();
    SqlParameter[] prm, prm1;
    public function fns = new function();
    public string Milestonedata = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {

        }
    }





    //// web method bind using ajax start //

    [WebMethod]
    public static void SaveTags(tagsdata Tagsdetails)
    {
        DataAccess da1 = new DataAccess();
        string editid = Tagsdetails.id;
        string entrytype = "Insert";
        if (editid != "" && editid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[9];
        prm[0] = new SqlParameter("@id", editid);
        prm[1] = new SqlParameter("@ProjectId", Tagsdetails.ProjectId);
        prm[2] = new SqlParameter("@Description", Tagsdetails.Description);
        prm[3] = new SqlParameter("@Tags", Tagsdetails.Tags);
        prm[4] = new SqlParameter("@TagsColor", Tagsdetails.TagsColor);
        prm[5] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[6] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[7] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[8] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pmstbl_AddTags_usp", prm);
    }

   

    [WebMethod]
    public static string UpdatePriority_Status(string milstoneid = "", string Propertyid = "")
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
    public static string bindTagsdata(string RecordId = "0")
    {
       
        string tags = "";
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        SqlParameter[] prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[1] = new SqlParameter("@RecordId", RecordId);
        DataSet ds = da.GetDataSet("Pms_BindTags_dashbord", prm);
        tags += "<table cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm datatable'>";
        tags += "<thead>";
        tags += "<tr>";

        //Milestonedata += "<th>Select</th>";
        tags += "<th>S.No</th>";
        //tags += "<th style=min-width:600px;>Description</th>";
        tags += "<th style=min-width:685px;>Project Name</th>";
        tags += "<th style=min-width:685px;>Tags</th>";
        //tags += "<th style=min-width:120px;>Action</th>";
        tags += "</tr>";
        tags += "</thead>";
        tags += "<tbody>";
        if (ds.Tables[0].Rows.Count > 0)
        {


            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                
                tags += "<tr>";
                tags += "<td valign='top'>";
                tags += (i + 1);
                tags += "</td>";

                // description 
                //tags += "<td valign='top'>";
                //tags += ds.Tables[0].Rows[i]["Description"].ToString();
                //tags += "</td>";

                // Project name 
                tags += "<td valign='top'>";
                tags += get_ProjectName(ds.Tables[0].Rows[i]["ProjectId"].ToString());
                tags += "</td>";

                // Tags name 
                //tags += "<td valign='top'>";
                //tags += ds.Tables[0].Rows[i]["Tags"].ToString();
                //tags += "</td>";


                tags += "<td ><span class='badge badge-label' style='background:"+ ds.Tables[0].Rows[i]["TagsColor"].ToString() + ";'><i class='ti ti-point-filled'></i> "+ ds.Tables[0].Rows[i]["Tags"].ToString() + "</span></td>";

                //Milestonedata += "<td valign='top'>";
                //Milestonedata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                //Milestonedata += "<a href ='javascript:void(0);' type='button' data-bs-toggle='offcanvas' data-bs-target=#canvas_popup onclick=loadsrc('inner_popup','/AddTaskPopup.aspx?id=" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "&milstoneid=" + ds.Tables[0].Rows[i]["id"].ToString() + "&type=milstone')><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Task'>+</span></a>";
                //Milestonedata += "</div>";  // tags
                //Milestonedata += "</td>";


                //tags += " <td><a href='javascript:void(0);' Class='btn btn-info btn-sm' type='button' onclick='Binddetails(" + ds.Tables[0].Rows[i]["id"].ToString() + ")' data-bs-toggle='offcanvas' data-bs-target='#addTags'><i class='ri-edit-line'></i></a>";
                //tags += "&nbsp;&nbsp;<a href='javascript:void(0);' Class='btn btn-danger btn-sm' type='button' onclick='deletedata(" + ds.Tables[0].Rows[i]["id"].ToString() + ")'><i class='ri-delete-bin-5-line'></i></a>";
                //tags += "</td>";


                tags += "</tr>";
            }
        }

        tags += "</tbody>";
        tags += "</table>";

        return tags;
    }


    [WebMethod]
    public static string bindTags_Dropdata(string RecordId = "0")
    {

        string tagdestilas = "";
        DataAccess da = new DataAccess();

        if (RecordId == "0")
        {

            string strleadstatus = "select * from Pms_tbl_Addtags where status = 1 and archive = 0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by id asc";
            DataSet ds = da.GetDataSetInline(strleadstatus);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int totalcount = 0;
                string strallcount = "select COUNT(*) allcount from Pms_tbl_Addtags where status = 1 and archive = 0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                DataSet ds1 = da.GetDataSetInline(strallcount);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    totalcount = Convert.ToInt32(ds1.Tables[0].Rows[0]["allcount"]);
                }

                tagdestilas += "<a href = '#' class='active'>All<span class='badge bg-secondary-subtle text-secondary ms-auto'>"+ totalcount + "</span></a>";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                   
                    tagdestilas += "<a href = '#'>" + ds.Tables[0].Rows[i]["Tags"].ToString() + "<span class='badge bg-secondary-subtle text-secondary ms-auto'>0</span></a>";
                  

                }
            }

        }

        else
        {

            string strleadstatus = "select * from Pms_tbl_Addtags where status = 1 and archive = 0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and ProjectId = '" + RecordId + "' order by id asc";
            DataSet ds = da.GetDataSetInline(strleadstatus);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    //tagdestilas += "<a href = '#' class='active'>All<span class='badge bg-secondary-subtle text-secondary ms-auto'>1</span></a>";
                    tagdestilas += "<a href = '#'>" + ds.Tables[0].Rows[i]["Tags"].ToString() + "<span class='badge bg-secondary-subtle text-secondary ms-auto'>0</span></a>";
                    //tagdestilas += "<a href = '#'>Phases<span class='badge bg-secondary-subtle text-secondary ms-auto'>0</span></a>";
                    //tagdestilas += "<a href = '#'>Tasks<span class='badge bg-secondary-subtle text-secondary ms-auto'>0</span></a>";
                    //tagdestilas += "<a href = '#'> Forums <span class='badge bg-secondary-subtle text-secondary ms-auto'>0</span></a>";
                    //tagdestilas += "<a href = '#' >Issues <span class='badge bg-secondary-subtle text-secondary ms-auto'>0</span></a>";



                }
            }
        }
        

        return tagdestilas;
    }



    [WebMethod]
    public static string BindTagsdetail(int RecordId = 0)
    {
        DataAccess da = new DataAccess();
        DataSet ds;
        string strsql = "select * from Pms_tbl_Addtags where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
        ds = da.GetDataSetInline(strsql);

        return ds.GetXml();
    }
    

    [WebMethod]
    public static string DeleteTagsdata(int RecordId = 0)
    {
        string retval = "0";

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "update  Pms_tbl_Addtags set  status=0,archive=1  where  id='" + RecordId + "' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        ds = da.GetDataSetInline(strsql);

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
        string strsql = "select * from Pms_tbl_Projectstatus where  status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret += "<ul class='sttus-list' data-simplebar style='max-height: 160px'>";
            for (int i = 0; i < dscode.Tables[0].Rows.Count; i++)
            {

                strret += "<li>" +
                  "<div class='dropdown-item p-0'>" +
                    "<a class='' href='javascript:void(0);' onclick = 'UpdateDataStatusdeatils(" + dscode.Tables[0].Rows[i]["id"].ToString() + ")' > " +
                    "<i class='bi bi-square-fill' style='color:" + dscode.Tables[0].Rows[i]["PrintColor"].ToString() + "' ></i> " + dscode.Tables[0].Rows[i]["ProjectStatus"].ToString() + "" +
                    "</a>" +
                    "</div>" +
                    "</li>";


            }
            strret += "</ul>";
        }

        return strret;

    }
    protected static string get_Priority_listbind()
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = "select * from Pms_tbl_Priority where  status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
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

    protected static string get_Projectstatus(string projectid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_Projectstatus where  status=1 and archive=0 and id='" + projectid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString();

        }

        return strret;

    }
    protected static string get_Status_Color(string projectid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_Projectstatus where  status=1 and archive=0 and id='" + projectid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
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
        string strsql = " select * from Pms_tbl_Priority where  status=1 and archive=0 and Id='" + projectid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["name"].ToString();

        }

        return strret;

    }
    protected static string get_PriorityColor(string projectid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_Priority where  status=1 and archive=0 and Id='" + projectid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["PrintColor"].ToString();

        }

        return strret;

    }

}
public class tagsdata
{
    public string id { get; set; }
    public string ProjectId { get; set; }
    public string Description { get; set; }
    public string Tags { get; set; }
    public string TagsColor { get; set; }
    
}




