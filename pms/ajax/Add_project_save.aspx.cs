using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonClass;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Services;
public partial class ajax_Add_project_save : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataAccess da = new DataAccess();
    CommonFunction cmf = new CommonFunction();
    SqlParameter[] prm, prm1;
    public function fns = new function();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    [WebMethod]
    public static void SaveProject(projectdata projectdetails)
    {
        DataAccess da1 = new DataAccess();
        string editid = projectdetails.id;
        string entrytype = "Insert";
        if (editid != "" && editid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[22];
        prm[0] = new SqlParameter("@id", editid);       
        prm[1] = new SqlParameter("@ProjectID", projectdetails.ProjectID);
        prm[2] = new SqlParameter("@ProjectName", projectdetails.ProjectName);
        prm[3] = new SqlParameter("@Owner", projectdetails.Owner);
        prm[4] = new SqlParameter("@ProjectStatus", projectdetails.ProjectStatus);
        prm[5] = new SqlParameter("@ProjectAddress1", projectdetails.ProjectAddress1);
        prm[6] = new SqlParameter("@ProjectAddress2", projectdetails.ProjectAddress2);
        prm[7] = new SqlParameter("@City", projectdetails.City);
        prm[8] = new SqlParameter("@PostalCode", projectdetails.PostalCode);
        prm[9] = new SqlParameter("@StateName", projectdetails.StateName);
        prm[10] = new SqlParameter("@Countryid", projectdetails.Countryid);
        prm[11] = new SqlParameter("@ClientName", projectdetails.ClientName);

        if(projectdetails.AwardDate== string.Empty)
        {
            prm[12] = new SqlParameter("@AwardDate", DBNull.Value);
        }
        else
        {
            prm[12] = new SqlParameter("@AwardDate", projectdetails.AwardDate);
        }
        
        if(projectdetails.StartDate == string.Empty)
        {
            prm[13] = new SqlParameter("@StartDate", DBNull.Value);
        }
        else
        {
            prm[13] = new SqlParameter("@StartDate", projectdetails.StartDate);
        }
        
        if(projectdetails.TargetDate==string.Empty)
        {
            prm[14] = new SqlParameter("@TargetDate", DBNull.Value);
        }
        else
        {
            prm[14] = new SqlParameter("@TargetDate", projectdetails.TargetDate);
        }
       
        prm[15] = new SqlParameter("@ApprovalDrawing", projectdetails.ApprovalDrawing);

        if (projectdetails.DrawingApprovalDate==string.Empty)
        {
            prm[16] = new SqlParameter("@DrawingApprovalDate", DBNull.Value);
        }
        else
        {
            prm[16] = new SqlParameter("@DrawingApprovalDate", projectdetails.DrawingApprovalDate);
        }
        
        prm[17] = new SqlParameter("@Uploaddrawing", projectdetails.Uploaddrawing);
        prm[18] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[19] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[20] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[21] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pmstbl_Addproject_usp", prm);
    }

    [WebMethod]
    public static string bindProjectdata()
    {
        string projectdata = "";
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        DataSet ds = da.GetDataSet("Pms_select_tblprojectall_dashbord", prm);
        projectdata += "<table cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm datatable'>";
        projectdata += "<thead>";
        projectdata += "<tr>";

        //projectdata += "<th>Select</th>";
        projectdata += "<th>S.No</th>";
        projectdata += "<th>Project ID</th>";
        projectdata += "<th>Project Name</th>";
        projectdata += "<th>Owner</th>";
        projectdata += "<th>Team</th>";
        projectdata += "<th>Status</th>";
        projectdata += "<th>Priority</th>";
        projectdata += "<th>Budget</th>";
        projectdata += "<th>Actual</th>";
        projectdata += "<th>Start Date</th>";
        projectdata += "<th>Target Date</th>";
        projectdata += "<th>Timeline</th>";
        projectdata += "<th>Completion Status</th>";
        projectdata += "<th>Milestone</th>";
        projectdata += "<th>Task</th>";
        projectdata += "<th>Issues</th>";
        projectdata += "<th>File</th>";
        projectdata += "<th>Project Address</th>";
        projectdata += "<th>Client Name</th>";
        projectdata += "<th>Award Date</th>";
        projectdata += "<th>Approval of Drawing</th>";
        projectdata += "<th>Drawing Approval Date</th>";
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
                projectdata += ds.Tables[0].Rows[i]["ProjectID"].ToString();
                projectdata += "</td>";

                projectdata += "<td valign='top'>";
                projectdata += ds.Tables[0].Rows[i]["ProjectName"].ToString();
                projectdata += "</td>";

                projectdata += "<td valign='top'>";
                projectdata += get_employee(ds.Tables[0].Rows[i]["Owner"].ToString());
                projectdata += "</td>";

                projectdata += "<td></td>"; // team 
                

                projectdata += "<td valign='top'>";
                projectdata += get_Projectstatus(ds.Tables[0].Rows[i]["ProjectStatus"].ToString());
                projectdata += "</td>";

                projectdata += "<td></td>";
                projectdata += "<td></td>";
                projectdata += "<td></td>";

                projectdata += "<td valign='top'>";
                projectdata += cmf.dateConvert(ds.Tables[0].Rows[i]["StartDate"].ToString());
                projectdata += "</td>";

                projectdata += "<td valign='top'>";
                projectdata += cmf.dateConvert(ds.Tables[0].Rows[i]["TargetDate"].ToString());
                projectdata += "</td>";

                projectdata += "<td></td>";
                projectdata += "<td></td>";
                projectdata += "<td></td>";
                projectdata += "<td></td>";
                projectdata += "<td></td>";

                projectdata += "<td valign='top'>";
                // projectdata += ds.Tables[0].Rows[i]["Uploaddrawing"].ToString();  // file upload image
                projectdata += "";  // file upload image
                projectdata += "</td>";

                projectdata += "<td valign='top'>";
                projectdata += ds.Tables[0].Rows[i]["ProjectAddress1"].ToString();
                projectdata += "</td>";

                projectdata += "<td valign='top'>";
                projectdata += ds.Tables[0].Rows[i]["ClientName"].ToString();
                projectdata += "</td>";

                projectdata += "<td valign='top'>";
                projectdata += cmf.dateConvert(ds.Tables[0].Rows[i]["AwardDate"].ToString());
                projectdata += "</td>";

                string strret = "";
                if (ds.Tables[0].Rows[i]["ApprovalDrawing"].ToString() == "1")
                {
                    strret = "Yes";

                }

                if (ds.Tables[0].Rows[i]["ApprovalDrawing"].ToString() == "2")
                {
                    strret = "No";

                }

                projectdata += "<td valign='top'>";
                projectdata += strret;
                projectdata += "</td>";

                projectdata += "<td valign='top'>";
                projectdata += cmf.dateConvert(ds.Tables[0].Rows[i]["DrawingApprovalDate"].ToString());
                projectdata += "</td>";


                //projectdata += "<td valign='top'>";
                //projectdata += "";
                //projectdata += "</td>";

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
    public static string Bindprojectdetail(int RecordId = 0)
    {

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "select * from Pms_tbl_AddProject where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
        ds = da.GetDataSetInline(strsql);


        return ds.GetXml();
    }



    [WebMethod]
    public static string Deleteprojectdata(int RecordId = 0)
    {
        string retval = "0";

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "update  Pms_tbl_AddProject set  status=0,archive=1  where  id='" + RecordId + "' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
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

        return strret;

    }

}
public class projectdata
{
    public string id { get; set; }
    public string ProjectID { get; set; }
    public string ProjectName { get; set; }
    public string Owner { get; set; }
    public string ProjectStatus { get; set; }
    public string ProjectAddress1 { get; set; }
    public string ProjectAddress2 { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
    public string StateName { get; set; }
    public string Countryid { get; set; }
    public string ClientName { get; set; }
    public string AwardDate { get; set; }
    public string StartDate { get; set; }
    public string TargetDate { get; set; }
    public string ApprovalDrawing { get; set; }
    public string DrawingApprovalDate { get; set; }
    public string Uploaddrawing { get; set; }
    

}