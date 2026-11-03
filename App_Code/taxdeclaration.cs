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
public class taxdeclaration : System.Web.Services.WebService
{

    public taxdeclaration()
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
    public string inserttaxdeclarationandget(string sval2, string fval3, string eval4, string cval5, string acval6)
    {  function fns = new function();
    string commentsdetails = "";
        string defaultcur = fns.bindcurdefault();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        ////ADO.Net
        //SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();


        SqlParameter[] prm = new SqlParameter[11];
        prm[0] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[1] = new SqlParameter("@cdate", da.getcdatetime());
        prm[2] = new SqlParameter("@empid", cmf.Decrypt(eval4));
        prm[3] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
        prm[4] = new SqlParameter("@catid", cval5);
        prm[5] = new SqlParameter("@subcatid", cmf.Decrypt(sval2));
        prm[6] = new SqlParameter("@amount", acval6);
        prm[7] = new SqlParameter("@isapprove", "1");
        prm[8] = new SqlParameter("@isapproveuid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[9] = new SqlParameter("@fid", cmf.Decrypt(fval3));   
        if(cval5=="3")
        {
            prm[10] = new SqlParameter("@isallowance", "1");
        }
        else
        {
            prm[10] = new SqlParameter("@isallowance", "0");
        }

        ds = da.GetDataSet("tbl_taxdeductionheadempdetailsinsert", prm);
        dt = ds.Tables[0];
        foreach (DataRow row in dt.Rows)
        {
            commentsdetails = defaultcur+" "+fns.ToFormattedCurrencyString(acval6);
        }

        return commentsdetails;
    }




    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string inserttaxdeclarationdelete(string i, string sval2, string fval3, string eval4, string cval5, string acval6)
    {
        function fns = new function();
        string commentsdetails = "";
        string defaultcur = fns.bindcurdefault();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        ////ADO.Net
        //SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlParameter[] prm = new SqlParameter[4];

        prm[2] = new SqlParameter("@empid", cmf.Decrypt(eval4));
        prm[0] = new SqlParameter("@subcatid", cmf.Decrypt(sval2));
        prm[1] = new SqlParameter("@fid", cmf.Decrypt(fval3));
        if (cval5 == "3")
        {
            prm[3] = new SqlParameter("@isallowance", "1");
        }
        else
        {
            prm[3] = new SqlParameter("@isallowance", "0");
        }
        da.executeDMLQuery("tbl_taxdeductionheadempdetailsdelete", prm);

        commentsdetails = "<a role='button' data-toggle='modal' data-target='#myModal' href='javascript:\\'   onclick=\"showattfiles('" + i + "','" + sval2 + "','" + fval3 + "','" + eval4 + "','" + cval5 + "')\"><i class='fa fa-paperclip'></i> Upload </a>";
        string commentsdetails1 = "No Proof";
        return commentsdetails + "#####" + commentsdetails1;

       
    }




    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string taxdeclarationget(string sval2, string fval3, string eval4, string cval5)
    {
        function fns = new function();
        string commentsdetails = "";
        string defaultcur = fns.bindcurdefault();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        ////ADO.Net
        //SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();


        SqlParameter[] prm = new SqlParameter[4];

        prm[2] = new SqlParameter("@empid", cmf.Decrypt(eval4));


        prm[0] = new SqlParameter("@subcatid", cmf.Decrypt(sval2));

        prm[1] = new SqlParameter("@fid", cmf.Decrypt(fval3));
        if (cval5 == "3")
        {
            prm[3] = new SqlParameter("@isallowance", "1");
        }
        else
        {
            prm[3] = new SqlParameter("@isallowance", "0");
        }
        ds = da.GetDataSet("tbl_taxdeductionheadempdetailsselect", prm);
        if (ds.Tables[0].Rows.Count > 0)
        {
            commentsdetails = "Accepted <br/> Using amount " + fns.ToFormattedCurrencyString(ds.Tables[0].Rows[0]["amount"].ToString()) + " " + defaultcur + "#####" + ds.Tables[0].Rows[0]["amount"].ToString();

        }
        else
        {
            commentsdetails = "Not Submitted#####";
        }

        return commentsdetails;
    }


    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string taxdeclarationdocget(string i, string sval2, string fval3, string eval4, string cval5)
    {
        function fns = new function();
        string commentsdetails = "";
        string commentsdetails1 = "";
        string defaultcur = fns.bindcurdefault();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        ////ADO.Net
        //SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();


        SqlParameter[] prm = new SqlParameter[4];

        prm[2] = new SqlParameter("@empid", cmf.Decrypt(eval4));
        prm[0] = new SqlParameter("@subcatid", cmf.Decrypt(sval2));
        prm[1] = new SqlParameter("@fid", cmf.Decrypt(fval3));
        if (cval5 == "3")
        {
            prm[3] = new SqlParameter("@isallowance", "1");
        }
        else
        {
            prm[3] = new SqlParameter("@isallowance", "0");
        }
        ds = da.GetDataSet("tbl_taxdeductionheadempdetailsdocselect", prm);
        if (ds.Tables[0].Rows.Count > 0)
        {
            commentsdetails = "<a role='button' data-toggle='modal' data-target='#myModal' href='javascript:\\'   onclick=\"showattfiles('" + i + "','" + sval2 + "','" + fval3 + "','" + eval4 + "','"+ cval5+"')\"><i class='fa fa-paperclip'></i> " + ds.Tables[0].Rows.Count + " Files Attached </a>";
            commentsdetails1 = "<a role='button' data-toggle='modal' data-target='#myModal' href='javascript:\\'   onclick=\"showattfiles('" + i + "','" + sval2 + "','" + fval3 + "','" + eval4 + "','" + cval5 + "')\">View Receipts</a>";
        }
        else
        {
            commentsdetails = "<a role='button' data-toggle='modal' data-target='#myModal' href='javascript:\\'   onclick=\"showattfiles('" + i + "','" + sval2 + "','" + fval3 + "','" + eval4 + "','" + cval5 + "')\"><i class='fa fa-paperclip'></i> Upload </a>";
            commentsdetails1 = "No Proof";

        }

        return commentsdetails + "#####" + commentsdetails1;
    }



    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string getamountformate(string acval6)
    {
        function fns = new function();
        string commentsdetails = "";
        string defaultcur = fns.bindcurdefault();

        commentsdetails = defaultcur + " " + fns.ToFormattedCurrencyString(acval6);


        return commentsdetails;
    }
}
