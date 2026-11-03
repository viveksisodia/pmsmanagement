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
public class ajax : System.Web.Services.WebService {

    public ajax () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string getcursymboal(string prefixText)
    {
        string customers = "";
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        ////ADO.Net
        //SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        char [] ch={'-'};

        string[] arr = prefixText.Split(ch);
        string strdata = "select * from tbl_Currenciesmaster where status=1 and archive=0 and rtrim(ltrim(Currency))='" + arr[0].Trim() + "'";
        ds = da.GetDataSetInline(strdata);
        dt = ds.Tables[0];



        if (ds.Tables[0].Rows.Count > 0)
        {


            string displaempstyle = ds.Tables[0].Rows[0]["Symbol"].ToString();


            customers = (string.Format("{0}###{1}", displaempstyle, ds.Tables[0].Rows[0]["id"].ToString()));

        }
        if (ds.Tables[0].Rows.Count == 0)
        {
            customers = (string.Format("{0}###{1}", "No Records Found !", "0"));
        }
        return customers;
    }





    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string gettaxrate(string prefixText)
    {
        string customers = "";
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        ////ADO.Net
        //SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
     
        string strdata = "select * from tbl_taxmasterdetails where id='" + prefixText + "'";
        ds = da.GetDataSetInline(strdata);
        dt = ds.Tables[0];



        if (ds.Tables[0].Rows.Count > 0)
        {


            string displaempstyle = ds.Tables[0].Rows[0]["taxvalue"].ToString();


            customers = (string.Format("{0}", displaempstyle));

        }
        if (ds.Tables[0].Rows.Count == 0)
        {
            customers = (string.Format("{0}", ""));
        }
        return customers;
    }



    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string getcustomeraddress(string prefixText)
    {
        string customers = "";
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        ////ADO.Net
        //SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        char[] ch = { '-' };

        //string[] arr = prefixText.Split(ch);
        string strdata = "select * from tbl_contactsdetails where id='"+prefixText+"'";
        ds = da.GetDataSetInline(strdata);
        dt = ds.Tables[0];



        if (ds.Tables[0].Rows.Count > 0)
        {


           
            string msgstr = ds.Tables[0].Rows[0]["billAttention"].ToString();
            if (ds.Tables[0].Rows[0]["billAddress1"].ToString() != string.Empty)
            {
                if (msgstr == string.Empty)
                    msgstr += ds.Tables[0].Rows[0]["billAddress1"].ToString();
                else
                  msgstr += "<br/>" + ds.Tables[0].Rows[0]["billAddress1"].ToString();
            }
            if (ds.Tables[0].Rows[0]["billAddress2"].ToString() != string.Empty)
            {
                if (msgstr == string.Empty)
                    msgstr += ds.Tables[0].Rows[0]["billAddress2"].ToString();
                else
                msgstr += "<br/>" + ds.Tables[0].Rows[0]["billAddress2"].ToString();
            }
            if (ds.Tables[0].Rows[0]["billCity"].ToString() != string.Empty)
            {
                if (msgstr == string.Empty)
                    msgstr += ds.Tables[0].Rows[0]["billCity"].ToString();
                else
                msgstr += "<br/>" + ds.Tables[0].Rows[0]["billCity"].ToString();
            }
            if (ds.Tables[0].Rows[0]["billState"].ToString() != string.Empty)
            {
                if (msgstr == string.Empty)
                    msgstr += ds.Tables[0].Rows[0]["billState"].ToString();
                else
                msgstr += "<br/>" + ds.Tables[0].Rows[0]["billState"].ToString();
            }
            if (ds.Tables[0].Rows[0]["billZipCode"].ToString() != string.Empty)
            {
                if (msgstr == string.Empty)
                    msgstr += ds.Tables[0].Rows[0]["billZipCode"].ToString();
                else
                msgstr += "<br/>" + ds.Tables[0].Rows[0]["billZipCode"].ToString();
            }
            if (ds.Tables[0].Rows[0]["billCountry"].ToString() != string.Empty)
            {
                if (msgstr == string.Empty)
                    msgstr += ds.Tables[0].Rows[0]["billCountry"].ToString();
                else
                msgstr += "<br/>" + ds.Tables[0].Rows[0]["billCountry"].ToString();
            }
            if (ds.Tables[0].Rows[0]["billPhone"].ToString() != string.Empty)
            {
                if (msgstr == string.Empty)
                    msgstr += ds.Tables[0].Rows[0]["billPhone"].ToString();
                else
                msgstr += "<br/>" + ds.Tables[0].Rows[0]["billPhone"].ToString();
            }
            if (ds.Tables[0].Rows[0]["billFax"].ToString() != string.Empty)
            {
                if (msgstr == string.Empty)
                    msgstr += ds.Tables[0].Rows[0]["billFax"].ToString();
                else
                msgstr += "<br/>" + ds.Tables[0].Rows[0]["billFax"].ToString();
            }
            customers = (string.Format("{0}", msgstr));

        }
        if (ds.Tables[0].Rows.Count == 0)
        {
            customers = (string.Format("{0}", ""));
        }
        return customers;
    }



}
