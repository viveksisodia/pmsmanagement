
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
using System.Web.Services;

public partial class _Default : System.Web.UI.Page
{
    DataAccess da = new DataAccess();
    SqlParameter[] prm;
    DataSet ds = new DataSet();
    CommonFunction cmg = new CommonFunction();
    CommonFunction cmf = new CommonFunction();
    public string empdata = "";
    public function fns = new function();
    public int isAccess = 0;
    public int isAccessadmin = 0;
    public int isgardex = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Title = fns.getltext("All Employees", "2");
        if(!Page.IsPostBack)
        {
           // bindemployee();
            binddepartment();
            bindDesignation();
            bindreporting();
            ddlprofilebind();
        }
    }
    protected void ddlprofilebind()
    {
        string strsql = "select * from Manage_Profile where (regid is null or regid='" + Request.Cookies["cadmin"]["id"] + "') and status=1 and archive=0";
        DataSet dsddl = da.GetDataSetInline(strsql);
        cmf.BindDropDownList(ddlrolandresponsiblity, dsddl, "profile", "id");
        ListItem li = new ListItem("Select Roles & Responsibility", "0");
        ddlrolandresponsiblity.Items.Insert(0, li);

    }
    protected void bindreporting()
    {
        SqlParameter[] prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@empname", "%");
        prm[1] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
        DataSet ds = da.GetDataSet("selectemployeesetup", prm);
        cmf.BindDropDownList(ddlreportto, ds, "empname", "id");
        ListItem li = new ListItem("Select Report To", "0");
        ddlreportto.Items.Insert(0, li);

    }
    protected void binddepartment()
    {
        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"].ToString());
        DataSet dataset = da.GetDataSet("SP_SelectDepartment_reg", prm);
        cmf.BindDropDownList(ddldepartment, dataset, "Department_name", "Dept_Id");
        ListItem li = new ListItem("Select Department", "0");
        ddldepartment.Items.Insert(0, li);
        ListItem li1 = new ListItem("+ Add New", "-1");
        ddldepartment.Items.Add(li1);
    }
    protected void bindDesignation()
    {
        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"].ToString());
        DataSet dataset = da.GetDataSet("SP_Selectjobtitle_reg", prm);
        cmf.BindDropDownList(ddljobtitle, dataset, "name", "Id");
        ListItem li = new ListItem("Select Jobtitle", "0");
        ddljobtitle.Items.Insert(0, li);
        ListItem li1 = new ListItem("+ Add New", "-1");
        ddljobtitle.Items.Add(li1);
    }

    //protected void btndelete_click(object sender,EventArgs e)
    //{
    //    string strsql = "update tbl_employee set status=0,archive=1 where id='" + hdnid.Value + "'";
    //    da.executeDMLQueryinline(strsql);
    //    Page.RegisterClientScriptBlock("js", "<script>alert('Data Deleted !');</script>");
    //    hdnid.Value = "0";
    //    bindemployee();
    //}



    protected void Button1_Click(object sender,EventArgs e)
    {
        try
        {
            binddepartment();
            ddldepartment.SelectedValue = hdndepartment.Value;
        }
        catch
        {

        }
        try
        {
            bindDesignation();
            ddljobtitle.SelectedValue = hdndesignation.Value;
        }
        catch
        {

        }
        try
        {
            ddlreportto.SelectedValue = hdnreportto.Value;
        }
        catch
        {

        }
        try
        {
            ddlrolandresponsiblity.SelectedValue = hdnrolandresponsiblity.Value;
        }
        catch
        {

        }
        
        }

    // web method bind using ajax start //

    [WebMethod]
    public static void Saveemployee(empdata empdetails)
    {
        DataAccess da1 = new DataAccess();
        string editid = empdetails.id;
        string entrytype = "Insert";
        if (editid != "" && editid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[14];
        prm[0] = new SqlParameter("@id", editid);
        prm[1] = new SqlParameter("@type", entrytype);
        prm[2] = new SqlParameter("@Department", empdetails.Department);
        prm[3] = new SqlParameter("@Location", empdetails.Location);
        prm[4] = new SqlParameter("@JobTitle", empdetails.JobTitle);
        prm[5] = new SqlParameter("@DisplayName", empdetails.DisplayName);
        prm[6] = new SqlParameter("@emp_name", empdetails.emp_name);
        prm[7] = new SqlParameter("@Password", empdetails.Password);
        prm[8] = new SqlParameter("@Email", empdetails.Email);
        prm[9] = new SqlParameter("@MobileNumber", empdetails.MobileNumber);
        prm[10] = new SqlParameter("@roleid", empdetails.roleid);
        prm[11] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[12] = new SqlParameter("@currentdate", da1.getcdatetime());
        prm[13] = new SqlParameter("@ReportingManager", empdetails.ReportingManager);
        da1.executeDMLQuery("qhstbl_employee_usp", prm);
    }
    [WebMethod]
    public static string Adddesignation(string value = "")
    {

        string retval = "0";
        DataAccess da1 = new DataAccess();

        SqlParameter[] prm = new SqlParameter[4];
        prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[1] = new SqlParameter("@name", value);
        prm[2] = new SqlParameter("@type", "insert");
        prm[3] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[3].Direction = ParameterDirection.ReturnValue;
        int id = da1.executeDMLQuery("qhsc_SP_jobtitle_insert_reg", prm);
        retval = id.ToString();



        return retval;

    }
    [WebMethod]
    public static string Adddepartment(string value = "")
    {
        string retval = "0";
        DataAccess da1 = new DataAccess();

        SqlParameter[] prm = new SqlParameter[4];
        prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[1] = new SqlParameter("@name", value);
        prm[2] = new SqlParameter("@type", "insert");
        prm[3] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[3].Direction = ParameterDirection.ReturnValue;
        int id = da1.executeDMLQuery("qhsc_SP_Department_insert_reg", prm);
        retval = id.ToString();



        return retval;
    }
    
    [WebMethod]
    public static string Bindempdetailsdetail(int RecordId = 0)
    {

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "select * from tbl_employee where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
        ds = da.GetDataSetInline(strsql);


        return ds.GetXml();
    }
    
    [WebMethod]

    public static string bindemployee()
    {
        string empdata = "";
        DataAccess da = new DataAccess();
        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        DataSet ds = da.GetDataSet("select_tblemployeeall_dashbord", prm);
        empdata += "<table cellpadding='0' cellspacing='0' border='0' class='table table-hover table-striped table-sm mb-0 table-nowrap datatable table-bordered'>";
        empdata += "<thead>";
        empdata += "<tr>";
        empdata += "<th>S.NO</th>";
        empdata += "<th>Department</th>";
        empdata += "<th>Designation</th>";
        empdata += "<th>Name</th>";
        empdata += "<th>Report To</th>";
        empdata += "<th>Email id</th>";
        empdata += "<th>Mobile Number</th>";
        empdata += "<th>Roles & Responsibility</th>";
        empdata += "<th>Action</th>";

        empdata += "</tr>";
        empdata += "</thead>";
        empdata += "<tbody>";
        if (ds.Tables[0].Rows.Count > 0)
        {


            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                empdata += "<tr><td>" + (i + 1) + "</td>";
                empdata += " <td>" + ds.Tables[0].Rows[i]["Department_name"].ToString() + "</td>";
                empdata += " <td>" + ds.Tables[0].Rows[i]["Designation"].ToString() + "</td>";
                empdata += " <td>" + ds.Tables[0].Rows[i]["DisplayName"].ToString() + "</td>";
                empdata += " <td>" + ds.Tables[0].Rows[i]["tldisplayname"].ToString() + "</td>";
                empdata += " <td>" + ds.Tables[0].Rows[i]["Email"].ToString() + "</td>";
                empdata += " <td>" + ds.Tables[0].Rows[i]["MobileNumber"].ToString() + "</td>";
                empdata += " <td>" + ds.Tables[0].Rows[i]["profile"].ToString() + "</td>";
                empdata += " <td><a href='javascript:void(0);' Class='btn btn-info btn-sm' type='button' onclick='Binddetails(" + ds.Tables[0].Rows[i]["id"].ToString() + ")' data-bs-toggle='offcanvas' data-bs-target='#masterlist'><i class='ri-edit-line'></i></a>";
                empdata += "&nbsp;&nbsp;<a href='javascript:void(0);' Class='btn btn-danger btn-sm' type='button' onclick='deletedata(" + ds.Tables[0].Rows[i]["id"].ToString() + ")'><i class='ri-delete-bin-5-line'></i></a>";
                empdata += "</td>";


                empdata += " </tr>";
            }
        }

        empdata += "</tbody>";
        empdata += "</table>";

        return empdata;
    }

    [WebMethod]
    public static string Deleteempdata(int RecordId = 0)
    {
        string retval = "0";

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "update  tbl_employee set  status=0,archive=1  where  id='" + RecordId + "' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        ds = da.GetDataSetInline(strsql);

        return retval;
    }

}
public class empdata
{
    public string id { get; set; }
    public string Department { get; set; }
    public string Location { get; set; }
    public string JobTitle { get; set; }
    public string DisplayName { get; set; }
    public string ReportingManager { get; set; }
    public string Email { get; set; }
    public string MobileNumber { get; set; }
    public string roleid { get; set; }
    public string emp_name { get; set; }
    public string Password { get; set; }




}