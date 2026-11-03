using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using CommonClass;
using System.Data.SqlClient;

public partial class Logout : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataAccess da = new DataAccess();
    CommonFunction cmf = new CommonFunction();
    protected void Page_Load(object sender, EventArgs e)
    {

        string id = "0";

        if (Request.Cookies["user"] != null)
        {
            if (Request.Cookies["user"]["user_id"] != null)
            {

                string strsql = "select top 1 id from Login_logdetails where uid='" + Request.Cookies["user"]["user_id"].ToString() + "' order by id desc";

                DataSet dsnew = da.GetDataSetInline(strsql);
                if (dsnew.Tables[0].Rows.Count > 0)
                {
                    id = dsnew.Tables[0].Rows[0]["id"].ToString();
                    string str = "update Login_logdetails set outtime='" + da.getcdatetime() + "' where id='" + id + "'";

                    da.executeDMLQueryinline(str);
                }
            }
        }

        Session.Remove("leftmenudata");
        Response.Cookies["v_user"].Expires = DateTime.Now.AddDays(-1);
        Response.Cookies["cadmin"].Expires = DateTime.Now.AddDays(-1);
        Response.Cookies["user"].Expires = DateTime.Now.AddDays(-1);
        Response.Cookies["fromdate"].Expires = DateTime.Now.AddDays(-1);
        Response.Cookies["todate"].Expires = DateTime.Now.AddDays(-1);
        Response.Cookies["fyear"].Expires = DateTime.Now.AddDays(-1);
        Response.Cookies["database"].Expires = DateTime.Now.AddDays(-1);
        try
        {
            Response.Cookies["regcid"].Expires = DateTime.Now.AddDays(-1);
        }
        catch
        {

        }
        try
        {
            Response.Cookies["myself"].Expires = DateTime.Now.AddDays(-1);
        }
        catch
        {

        }
        Session.Remove("adminid");
        Session.Remove("rollid");
        Response.Redirect("/login.aspx");
        
    }

    
}