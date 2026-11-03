using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using CommonClass;
using System.Data;

/// <summary>
/// Summary description for chkoffline
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]

public class chkoffline : System.Web.Services.WebService {

    public chkoffline () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }
    [WebMethod]
    public string chkkey(string keyvalue)
    {
        string strreturn = "1";
        DataAccess da=new DataAccess();

       // SendMail sm = new SendMail();
        try
        {

            string strsql = "select * from tbl_companyoffline where keyid='"+ keyvalue.Trim()+"' and status=1 and archive=0";
            DataSet ds = da.GetDataSetInline(strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                strreturn = "1#" + ds.Tables[0].Rows[0]["exdate"].ToString()+"#" + ds.Tables[0].Rows[0]["isprojectmanagement"].ToString(); ;
            }
            else
            {
                strreturn = "0#" + da.getcdate() ;
            }

            //sm.SendMailtoUser(to, from, sub, msg);
            //return "success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        return strreturn;


    }


    [WebMethod]
    public string chkkeyfirsttime(string keyvalue)
    {
        string strreturn = "0";
        DataAccess da = new DataAccess();

        // SendMail sm = new SendMail();
        try
        {

            string strsql = "select * from tbl_companyoffline where keyid='" + keyvalue.Trim() + "' and status=1 and archive=0 and (isused=0 or isused is null)";
            DataSet ds = da.GetDataSetInline(strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                strreturn = "1#" + ds.Tables[0].Rows[0]["exdate"].ToString(); ;
            }
            else
            {
                strreturn = "0#" + da.getcdate();
            }

            //sm.SendMailtoUser(to, from, sub, msg);
            //return "success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        return strreturn;


    }

    [WebMethod]
    public string chkkeyisused(string keyvalue)
    {
        string strreturn = "0";
        DataAccess da = new DataAccess();

        // SendMail sm = new SendMail();
        try
        {

            string strsql = "update tbl_companyoffline set isused=1 where keyid='" + keyvalue.Trim() + "'";
           da.executeDMLQueryinline(strsql);

            strreturn = "1";


        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        return strreturn;


    }
}
