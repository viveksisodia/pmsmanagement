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
public class data : System.Web.Services.WebService {

    public data () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string[] GetCompletionListgroup(string prefixText)
    {
        List<string> customers = new List<string>();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
       
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();


        SqlParameter[] prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@txtsearch", "%"+prefixText + "%");
        prm[1] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
        ds = da.GetDataSet("tbl_groupnameselect", prm);

        dt = ds.Tables[0];



        foreach (DataRow row in dt.Rows)
        {


            string displaempstyle = row["groupname"].ToString();

            //if (row["Designation"].ToString() != string.Empty)
            //{
            //    displaempstyle += " (" + row["Designation"].ToString() + ")";
            //}
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
    public string[] GetCompletionListgroupsetup(string prefixText)
    {
        List<string> customers = new List<string>();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();


        SqlParameter[] prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@txtsearch", "%" + prefixText + "%");
        prm[1] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
        ds = da.GetDataSet("tbl_groupnameselectsetup", prm);

        dt = ds.Tables[0];



        foreach (DataRow row in dt.Rows)
        {


            string displaempstyle = row["groupname"].ToString();

            //if (row["Designation"].ToString() != string.Empty)
            //{
            //    displaempstyle += " (" + row["Designation"].ToString() + ")";
            //}
            customers.Add(string.Format("{0}###{1}", displaempstyle, row["id"].ToString()));

        }
        if (ds.Tables[0].Rows.Count == 0)
        {
            customers.Add(string.Format("{0}###{1}", "No Records Found !", "0"));
        }
        return customers.ToArray();
    }
}
