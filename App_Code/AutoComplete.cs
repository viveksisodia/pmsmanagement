using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using CommonClass;
using System.Data;
/// <summary>
/// Summary description for AutoComplete
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class AutoComplete : System.Web.Services.WebService {


    DataAccess da = new DataAccess();
    CommonFunction cmf = new CommonFunction();

    public AutoComplete () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }


    [WebMethod]
    public string[] AutoCompleteAjaxRequest(string prefixText, int count)
    {


        

        List<string> ajaxDataCollection = new List<string>();
        DataTable _objdt = new DataTable();


        string strsql = "select groupname from tbl_ledgers where groupname like '" + prefixText + "%' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' order by groupname";
        DataSet ds = da.GetDataSetInline(strsql);


        _objdt = ds.Tables[0];
        if (_objdt.Rows.Count > 0)
        {
            for (int i = 0; i < _objdt.Rows.Count; i++)
            {
                ajaxDataCollection.Add(_objdt.Rows[i]["groupname"].ToString());
            }
        }
        return ajaxDataCollection.ToArray();
    }



    [WebMethod]
    public string[] AutoCompleteAjaxRequesttestname(string prefixText, int count)
    {




        List<string> ajaxDataCollection = new List<string>();
        DataTable _objdt = new DataTable();


        string strsql = "select testname from tbl_testmaster where testname like '" + prefixText + "%' and status=1 and archive=0 order by testname";
        DataSet ds = da.GetDataSetInline(strsql);


        _objdt = ds.Tables[0];
        if (_objdt.Rows.Count > 0)
        {
            for (int i = 0; i < _objdt.Rows.Count; i++)
            {
                ajaxDataCollection.Add(_objdt.Rows[i]["testname"].ToString());
            }
        }
        return ajaxDataCollection.ToArray();
    }


     [WebMethod]
    public string[] AutoCompleteAjaxRequestdoccode(string prefixText, int count)
    {




        List<string> ajaxDataCollection = new List<string>();
        DataTable _objdt = new DataTable();


        string strsql = "select code from tbl_doctorprofile where code like '" + prefixText + "%' and status=1 and archive=0 order by code";
        DataSet ds = da.GetDataSetInline(strsql);


        _objdt = ds.Tables[0];
        if (_objdt.Rows.Count > 0)
        {
            for (int i = 0; i < _objdt.Rows.Count; i++)
            {
                ajaxDataCollection.Add(_objdt.Rows[i]["code"].ToString());
            }
        }
        return ajaxDataCollection.ToArray();
    }
     [WebMethod]
    public string[] AutoCompleteAjaxRequestdocname(string prefixText, int count)
    {




        List<string> ajaxDataCollection = new List<string>();
        DataTable _objdt = new DataTable();


        string strsql = "select name from tbl_doctorprofile where name like '" + prefixText + "%' and status=1 and archive=0 order by name";
        DataSet ds = da.GetDataSetInline(strsql);


        _objdt = ds.Tables[0];
        if (_objdt.Rows.Count > 0)
        {
            for (int i = 0; i < _objdt.Rows.Count; i++)
            {
                ajaxDataCollection.Add(_objdt.Rows[i]["name"].ToString());
            }
        }
        return ajaxDataCollection.ToArray();
    }
}
