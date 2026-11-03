<%@ WebHandler Language="C#" Class="Deleteimagehandler" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonClass;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using CustomDropDown;
using System.Web.Script.Serialization;


public class Deleteimagehandler : IHttpHandler {

    public void ProcessRequest (HttpContext context)
    {
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
   try
   {
        string jsonData;
        using (var reader = new StreamReader(context.Request.InputStream))
        {
            jsonData = reader.ReadToEnd();
        }
        var serializer = new JavaScriptSerializer();
        var data = serializer.Deserialize<Dictionary<string, string>>(jsonData);
        string fileId = data["id"];
       string regId = HttpContext.Current.Request.Cookies["cadmin"]["id"];

    if (!string.IsNullOrEmpty(fileId) && !string.IsNullOrEmpty(regId))
    {
        // Use parameterized query to avoid SQL injection
        string sqlquery = "update Pms_tbl_projectfiles set  status=0 , archive=1 where regid = '" + regId + "' and id='" + fileId + "'";
        da.executeDMLQueryinline(sqlquery);
        context.Response.ContentType = "application/json";
        context.Response.Write("{\"success\": true}");
    }
    else
    {
        context.Response.ContentType = "application/json";
        context.Response.Write("{\"success\": false, \"message\": \"Missing file id or regid"+fileId+"\"}");
    }
}
catch (Exception ex)
{
    context.Response.ContentType = "application/json";
    context.Response.Write("{\"success\": false, \"message\": \"" + ex.Message.Replace("\"","\\\"") + "\"}");
}
        //try
        //{
        //    try
        //    {
        //        SaveLogText("delfiles-" + HttpContext.Current.Request.QueryString["id"]);
        //    }
        //    catch(Exception ex) {
        //        SaveLogText("delfiles-" + ex.Message);
        //    }
        //    if (HttpContext.Current.Request.QueryString["id"] != null)
        //    {
        //        string sqlquery = "update Pms_tbl_projectfiles set  status=0 , archive=1 where regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + HttpContext.Current.Request.QueryString["id"] + "'";
        //        da.executeDMLQueryinline(sqlquery);
        //        context.Response.ContentType = "text/plain";
        //        context.Response.Write("1");

        //    }
        //}
        //catch (Exception ex)
        //{
        //    context.Response.ContentType = "text/plain";
        //    context.Response.Write(ex.Message);
        //}


    }
    public static string SaveLogText(string Message)
    {
        string retval = "";
        CommonFunction cmf1 = new CommonFunction();
        DataAccess da1 = new DataAccess();
        DataSet dschk = new DataSet();

        SqlParameter[] prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@message", Message);
        prm[1] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[1].Direction = ParameterDirection.ReturnValue;
        retval = Convert.ToString(da1.executeDMLQuery("crm_entryLogTable", prm));
        return retval;
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}