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

public partial class Addmilestone : System.Web.UI.Page
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
                        
            bindemployee();
         
        }
    }


    //protected void bindcountry()
    //{
    //    string strleadstatus = "select id,countryname  from tbl_countrymaster order by id";
    //    DataSet ds = da.GetDataSetInline(strleadstatus);
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        cmf.BindDropDownList(Countryid, ds, "countryname", "id");
    //        ListItem li1 = new ListItem("", "0");
    //        Countryid.Items.Insert(0, li1);
    //    }


    //}

    protected void bindemployee()
    {
        string strleadstatus = "select id,DisplayName  from tbl_employee where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  order by id";
        DataSet ds = da.GetDataSetInline(strleadstatus);
        if (ds.Tables[0].Rows.Count > 0)
        {
            cmf.BindDropDownList(Owner, ds, "DisplayName", "id");
            ListItem li1 = new ListItem("", "0");
            Owner.Items.Insert(0, li1);
        }

    }
    //protected void bindprojectstatus()
    //{
    //    string strleadstatus = "select * from Pms_tbl_Projectstatus where status = 1 and archive = 0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
    //    DataSet ds = da.GetDataSetInline(strleadstatus);
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        cmf.BindDropDownList(ProjectStatus, ds, "ProjectStatus", "id");
    //        ListItem li1 = new ListItem("", "0");
    //        ProjectStatus.Items.Insert(0, li1);
    //    }


    //}




    protected void Button1_Click(object sender, EventArgs e)
    {
        

        try
        {
            StartDate.Value = Convert.ToDateTime(hdnStartDate.Value).ToString("yyyy-MM-dd");
        }
        catch
        {

        }

        try
        {
            EndDate.Value = Convert.ToDateTime(hdnEndDate.Value).ToString("yyyy-MM-dd");
        }
        catch
        {

        }

        try
        {
            Owner.SelectedValue = hdnowner.Value;
        }
        catch
        {

        }

        try
        {
            Access.SelectedValue = hdnAccess.Value;
        }
        catch
        {

        }






    }


    //// web method bind using ajax start //

    [WebMethod]
    public static void SaveMilestone(milestonetdata milestonedetails)
    {
        DataAccess da1 = new DataAccess();
        string editid = milestonedetails.id;
        string entrytype = "Insert";
        if (editid != "" && editid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[11];
        prm[0] = new SqlParameter("@id", editid);
        prm[1] = new SqlParameter("@Milestone", milestonedetails.Milestone);
        prm[2] = new SqlParameter("@Weightage", milestonedetails.Weightage);
        if (milestonedetails.StartDate == string.Empty)
        {
            prm[3] = new SqlParameter("@StartDate", DBNull.Value);
        }
        else
        {
            prm[3] = new SqlParameter("@StartDate", milestonedetails.StartDate);
        }
        if (milestonedetails.EndDate == string.Empty)
        {
            prm[4] = new SqlParameter("@EndDate", DBNull.Value);
        }
        else
        {
            prm[4] = new SqlParameter("@EndDate", milestonedetails.EndDate);
        }
        prm[5] = new SqlParameter("@Owner", milestonedetails.Owner);
        prm[6] = new SqlParameter("@Access", milestonedetails.Access);
        prm[7] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[8] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[9] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[10] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pmstbl_Addmilestone_usp", prm);
    }

    [WebMethod]
    public static string bindMilestonedata()
    {
        string Milestonedata = "";
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        DataSet ds = da.GetDataSet("Pms_Bindmilestone_dashbord", prm);
        Milestonedata += "<table cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm datatable'>";
        Milestonedata += "<thead>";
        Milestonedata += "<tr>";

        //Milestonedata += "<th>Select</th>";
        Milestonedata += "<th>S.No</th>";
        Milestonedata += "<th style=min-width:200px;>Milestone</th>";
        Milestonedata += "<th style=min-width:120px;>Owner</th>";
        Milestonedata += "<th style=min-width:120px;>Team</th>";
        Milestonedata += "<th style=width:130px;>Status</th>";
        Milestonedata += "<th>Priority</th>";
        Milestonedata += "<th>Budget</th>";
        Milestonedata += "<th>Actual</th>";
        Milestonedata += "<th>Start Date</th>";
        Milestonedata += "<th>Target Date</th>";
        Milestonedata += "<th style=min-width:140px;>Timeline</th>";
        Milestonedata += "<th style=min-width:140px;>Dependency</th>";
        Milestonedata += "<th style=min-width:140px;>Completion Status</th>";
        Milestonedata += "<th style=min-width:140px;>Task</th>";
        Milestonedata += "<th style=min-width:140px;>Issues</th>";
        Milestonedata += "<th>File</th>";
        Milestonedata += "<th>Action</th>";
        Milestonedata += "</tr>";
        Milestonedata += "</thead>";
        Milestonedata += "<tbody>";
        if (ds.Tables[0].Rows.Count > 0)
        {


            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Milestonedata += "<tr>";

                Milestonedata += "<td valign='top'>";
                Milestonedata += (i + 1);
                Milestonedata += "</td>";

                Milestonedata += "<td valign='top'>";
                Milestonedata += ds.Tables[0].Rows[i]["Milestone"].ToString();
                Milestonedata += "</td>";

                Milestonedata += "<td valign='top'>";
                Milestonedata += get_employee(ds.Tables[0].Rows[i]["Owner"].ToString());
                Milestonedata += "</td>";

                Milestonedata += "<td valign='top'>";
                Milestonedata += "";//team
                Milestonedata += "</td>";

                Milestonedata += "<td valign='top'>";
                Milestonedata += "";//status
                Milestonedata += "</td>";
                
                Milestonedata += "<td valign='top'>";
                Milestonedata += ""; //priority
                Milestonedata += "</td>";

                Milestonedata += "<td valign='top'>";
                Milestonedata += ""; //budget  
                Milestonedata += "</td>";

                Milestonedata += "<td valign='top'>";
                Milestonedata += ""; //Actual
                Milestonedata += "</td>";

                Milestonedata += "<td valign='top'>";
                Milestonedata += cmf.dateConvert(ds.Tables[0].Rows[i]["StartDate"].ToString());
                Milestonedata += "</td>";

                Milestonedata += "<td valign='top'>";
                Milestonedata += ""; //target date
                Milestonedata += "</td>";

                Milestonedata += "<td valign='top'>";
                Milestonedata += ""; //Timeline
                Milestonedata += "</td>";

                Milestonedata += "<td valign='top'>";
                Milestonedata += ""; //Dependency  
                Milestonedata += "</td>";

                Milestonedata += "<td valign='top'>";
                Milestonedata += ""; // Completion Status
                Milestonedata += "</td>";

                Milestonedata += "<td valign='top'>";
                Milestonedata += ""; // Task
                Milestonedata += "</td>";

                Milestonedata += "<td valign='top'>";
                Milestonedata += ""; // isssues 
                Milestonedata += "</td>";
                
                Milestonedata += "<td valign='top'>";
                 Milestonedata += "";  // file 
                Milestonedata += "</td>";
                
                Milestonedata += " <td><a href='javascript:void(0);' Class='btn btn-info btn-sm' type='button' onclick='Binddetails(" + ds.Tables[0].Rows[i]["id"].ToString() + ")' data-bs-toggle='offcanvas' data-bs-target='#addmilestone'><i class='ri-edit-line'></i></a>";
                Milestonedata += "&nbsp;&nbsp;<a href='javascript:void(0);' Class='btn btn-danger btn-sm' type='button' onclick='deletedata(" + ds.Tables[0].Rows[i]["id"].ToString() + ")'><i class='ri-delete-bin-5-line'></i></a>";
                Milestonedata += "</td>";


                Milestonedata += "</tr>";
            }
        }

        Milestonedata += "</tbody>";
        Milestonedata += "</table>";

        return Milestonedata;
    }
    [WebMethod]
    public static string Bindmilestonedetail(int RecordId = 0)
    {
        DataAccess da = new DataAccess();
        DataSet ds;
         string strsql = "select * from Pms_tbl_AddMilestone where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
        ds = da.GetDataSetInline(strsql);
        
        return ds.GetXml();
    }



    [WebMethod]
    public static string DeleteMilestonedata(int RecordId = 0)
    {
        string retval = "0";

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "update  Pms_tbl_AddMilestone set  status=0,archive=1  where  id='" + RecordId + "' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
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


}
public class milestonetdata
{
    public string id { get; set; }
    public string Milestone { get; set; }
    public string Weightage { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string Owner { get; set; }
    public string Access { get; set; }
   
}




