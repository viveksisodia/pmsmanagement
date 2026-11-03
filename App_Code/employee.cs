using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using CommonClass;
using System.Web.Script.Services;
using System.IO;
using System.Web.UI.WebControls;
/// <summary>
/// Summary description for employee
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class employee : System.Web.Services.WebService
{

    public employee()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string[] GetCompletionListemployee(string prefixText)
    {
        List<string> customers = new List<string>();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        ////ADO.Net
        //SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();


        SqlParameter[] prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@empname", prefixText + "%");
        prm[1] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
        ds = da.GetDataSet("selectemployeesetup", prm);

        dt = ds.Tables[0];



        foreach (DataRow row in dt.Rows)
        {

            //string displaempstyle="<div class='member-detail clear-side-padding ng-scope'><div class='member-profile-picture-container profile-picture-container'> ";

            //                                  displaempstyle+="<div class='profile-picture teamPictures ng-scope ng-isolate-scope'>";
            //                                                 displaempstyle+="<div class='img-initials ng-binding' style='background-color: rgb(21, 160, 203);'>AA</div>";
            //                                            displaempstyle+="</div></div><div>";

            //                                                displaempstyle+="<h4 class='ng-binding'>"+row["empname"].ToString()+"</h4>";

            //                                             displaempstyle+="<h5><span class='ng-binding ng-scope'>Executive Director</span></h5>";
            //                                             displaempstyle+="<h5><span class='ng-binding'>981-882-5303</span></h5>";
            //                                        displaempstyle+=" </div>";
            //                                     displaempstyle+="</div>";

            //string displaempstyle = "<b>" + row["empname"].ToString() + "" + row["Designation"].ToString() + ")" + row["EmpNo"].ToString(); ;
            string displaempstyle = row["empname"].ToString();

            if (row["Designation"].ToString() != string.Empty)
            {
                displaempstyle += " (" + row["Designation"].ToString() + ")";
            }
            customers.Add(string.Format("{0}###{1}", displaempstyle, row["id"].ToString()));

        }
        if (ds.Tables[0].Rows.Count == 0)
        {
            customers.Add(string.Format("{0}###{1}", "No Records Found !", "0"));
        }
        return customers.ToArray();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetCompletionListemployeedirectory(string prefixText)
    {


        string empdetails = "<div class='row'>";
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        ////ADO.Net
        //SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();


        SqlParameter[] prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@empname", prefixText + "%");
        prm[1] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
        ds = da.GetDataSet("selectemployeesetupdirectory", prm);

        dt = ds.Tables[0];



        foreach (DataRow row in dt.Rows)
        {
            char ch = ' ';
            char ch1 = ' ';
            try
            {
                ch = row["FirstName"].ToString().ToCharArray().ElementAt(0);
            }
            catch(Exception ex)
            { }
            try
            {
                ch1 = row["LastName"].ToString().ToCharArray().ElementAt(0);
            }
            catch (Exception ex)
            { }
            empdetails += "<div class='col-sm-4'><div class='member-detail clear-side-padding ng-scope'>";
            empdetails += "<div class='member-profile-picture-container profile-picture-container'>";

            empdetails += "<div class='profile-picture teamPictures ng-scope ng-isolate-scope'>";
            if (row["empimg"].ToString() == string.Empty)
            {

                empdetails += "<div class='img-initials ng-binding' style='background-color: rgb(21, 160, 203);'>" + ch + ch1 + "</div>";
            }
            else
            {
                if (File.Exists(Server.MapPath("/employee/emppic/" + row["empimg"].ToString())))
                {
                    empdetails += "   <img src='/employee/emppic/" + row["empimg"].ToString() + "' />";
                }
                else
                {
                    empdetails += "<div class='img-initials ng-binding' style='background-color: rgb(21, 160, 203);'>" + ch + ch1 + "</div>";
                }

            }
            empdetails += "</div>";
            empdetails += "</div>";
            empdetails += "<div>";
            empdetails += "<a href='/employee/employee-summary.aspx?empid=" + cmf.Encrypt(row["id"].ToString()) + "'>";
            empdetails += "<h4 class='ng-binding'>" + row["empname"].ToString() + "</h4>";
            empdetails += " </a>";
            empdetails += "<h5><span class='ng-binding ng-scope'>" + row["Designation"].ToString() + "</span></h5>";
            empdetails += "<h5><span class='ng-binding'>" + row["MobileNumber"].ToString() + "</span></h5>";
            empdetails += "</div>";
            empdetails += "</div>";
            empdetails += "</div>";





        }
        if (ds.Tables[0].Rows.Count == 0)
        {
            empdetails = "<div class='alert alert-info'>No Data Available</div>";

        }


        empdetails += "</div>";

        return empdetails;
    }



    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetCompletionListemployeedirectorytxt(string prefixText)
    {


        string empdetails = "<div class='row'>";
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        ////ADO.Net
        //SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();


        SqlParameter[] prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@empname", prefixText + "%");
        prm[1] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
        ds = da.GetDataSet("selectemployeesetup", prm);

        dt = ds.Tables[0];



        foreach (DataRow row in dt.Rows)
        {

            char ch = ' ';
            char ch1 = ' ';
            try
            {
                ch = row["FirstName"].ToString().ToCharArray().ElementAt(0);
            }
            catch (Exception ex)
            { }
            try
            {
                ch1 = row["LastName"].ToString().ToCharArray().ElementAt(0);
            }
            catch (Exception ex)
            { }
            empdetails += "<div class='col-sm-4'><div class='member-detail clear-side-padding ng-scope'>";
            empdetails += "<div class='member-profile-picture-container profile-picture-container'>";

            empdetails += "<div class='profile-picture teamPictures ng-scope ng-isolate-scope'>";
            empdetails += "<div class='img-initials ng-binding' style='background-color: rgb(21, 160, 203);'>" + ch + ch1 + "</div>";
            empdetails += "</div>";
            empdetails += "</div>";
            empdetails += "<div>";
            empdetails += "<a href='/employee/employee-summary.aspx?empid=" + cmf.Encrypt(row["id"].ToString()) + "'>";
            empdetails += "<h4 class='ng-binding'>" + row["empname"].ToString() + "</h4>";
            empdetails += " </a>";
            empdetails += "<h5><span class='ng-binding ng-scope'>" + row["Designation"].ToString() + "</span></h5>";
            empdetails += "<h5><span class='ng-binding'>" + row["MobileNumber"].ToString() + "</span></h5>";
            empdetails += "</div>";
            empdetails += "</div>";
            empdetails += "</div>";





        }
        if (ds.Tables[0].Rows.Count == 0)
        {
            empdetails = "<div class='alert alert-info'>No Data Available</div>";

        }


        empdetails += "</div>";

        return empdetails;
    }





    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<ListItem> Getcomponentsalary(string sbreakid)
    {
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        ////ADO.Net
        //SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();

        List<ListItem> customers = new List<ListItem>();


        string strsqlchk = "select * from tbl_salarycomponent where id not in (select compid from tbl_salarycomponentdetails where sbreakid='" + sbreakid + "' and status=1 and archive=0) and status=1 and archive=0 and isenabled=1 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";

        DataSet dsadditem = da.GetDataSetInline(strsqlchk);
        for (int i = 0; i < dsadditem.Tables[0].Rows.Count; i++)
        {
            customers.Add(new ListItem
            {
                Value = dsadditem.Tables[0].Rows[i]["id"].ToString(),
                Text = dsadditem.Tables[0].Rows[i]["headname"].ToString()
            });
        }

        return customers;
    }





}
