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
using System.Web.Script.Serialization;

public partial class UpdatetaskHandler : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataAccess da = new DataAccess();
    public CommonFunction cmf = new CommonFunction();
    SqlParameter[] prm, prm1;
    public function fns = new function();
    protected void Page_Load(object sender, EventArgs e)
    {
        // Only process POST requests
        if (Request.HttpMethod == "POST")
        {
            // Read JSON from request input stream
            string jsonData;
            using (var reader = new System.IO.StreamReader(Request.InputStream))
            {
                jsonData = reader.ReadToEnd();
            }

            var js = new JavaScriptSerializer();
            var data = js.Deserialize<UpdateRequest>(jsonData);

            bool success = UpdateDatabase(data.id, data.columnName, data.updateText);

            Response.ContentType = "application/json";
            var result = new { success = success, error = "something wrong" };
            Response.Write(js.Serialize(result));
            Response.End();
        }
    }

    private bool UpdateDatabase(string id, string columnName, string updateText)
    {
       
        try
        {
            string strsql = " update Pms_tbl_Addtask set " + columnName + "='" + updateText + "'  where status=1 and archive=0 and id='" + id + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
            da.executeDMLQueryinline(strsql);
            return true;
        }
        catch (Exception ex)
        {            
            return false;
        }
    }
}

public class UpdateRequest
{
    public string id { get; set; }
    public string columnName { get; set; }
    public string updateText { get; set; }
}
