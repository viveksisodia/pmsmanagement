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
public partial class settings_CountryMaster : System.Web.UI.Page
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
    public static void SaveCountryMaster(CountryMaster Countrydetails)
    {
        DataAccess da1 = new DataAccess();
        string editid = Countrydetails.id;
        string entrytype = "Insert";
        if (editid != "" && editid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", editid);
        prm[1] = new SqlParameter("@name", Countrydetails.Name);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pmstbl_CountryMater_usp", prm);
    }




    [WebMethod]
    public static string bindCountryData()
    {
        string projectdata = "";
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        DataSet ds = da.GetDataSet("pmstbl_CountryMaster_dashbord", prm);
        projectdata += "<table cellpadding='0' cellspacing='0' border='0'width=100% class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm datatable'>";
        projectdata += "<thead>";
        projectdata += "<tr>";

        projectdata += "<th>S.No</th>";
        projectdata += "<th>Country Name</th>";
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
                projectdata += ds.Tables[0].Rows[i]["countryname"].ToString();
                projectdata += "</td>";

                

                projectdata += " <td><a href='javascript:void(0);' Class='btn btn-info btn-sm' type='button' onclick='Binddetails113(" + ds.Tables[0].Rows[i]["id"].ToString() + ")' data-bs-toggle='offcanvas' data-bs-target='#addCountry'><i class='ri-edit-line'></i></a>";
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
    public static string BindCountrydetail(int RecordId = 0)
    {

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "select * from tbl_countrymaster where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
        ds = da.GetDataSetInline(strsql);


        return ds.GetXml();
    }

    

    [WebMethod]
    public static string DeleteCountryMaster(int RecordId = 0)
    {
        string retval = "0";

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "update  tbl_countrymaster set  status=0,archive=1  where  id='" + RecordId + "' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        ds = da.GetDataSetInline(strsql);

        return retval;
    }
}
public class CountryMaster
{
    public string id { get; set; }
    public string Name { get; set; }
   

}




