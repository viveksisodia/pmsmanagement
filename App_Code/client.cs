using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using CommonClass;
using System.Web.Script.Services;
/// <summary>
/// Summary description for client
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class client : System.Web.Services.WebService {

    public client () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string[] GetCompletionListofficeboys(string prefixText)
    {
        List<string> customers = new List<string>();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        ////ADO.Net
        //SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();


        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@txt", prefixText + "%");
        ds = da.GetDataSet("selectofficeboys", prm);

        dt = ds.Tables[0];



        foreach (DataRow row in dt.Rows)
        {

            customers.Add(string.Format("{0}###{1}", row["ccodename"].ToString(), row["id"].ToString()));

        }
        if (ds.Tables[0].Rows.Count == 0)
        {
            customers.Add(string.Format("{0}###{1}", "No Records Found !", "0"));
        }
        return customers.ToArray();
    }


}
