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
using System.IO;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Web.Services;


public partial class angularmaster_angular : System.Web.UI.MasterPage
{
    DataAccess da = new DataAccess();
    DataSet Ds = new DataSet();
    public string currentpage = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Cookies["user"] == null || Request.Cookies["cadmin"] == null)
        {
            Response.Redirect("/login.aspx"); // Redirect immediately if cookies are missing
        }
        String activepage = Request.RawUrl;
        currentpage = activepage;
        if (currentpage.Contains("/dashboarduser.aspx"))
        {
            usertask.Attributes.Add("class", "");
            userdashboard.Attributes.Add("class", "active");
        }
        else if (currentpage.Contains("/mytaskuser.aspx"))
        {
            userdashboard.Attributes.Add("class", "");
            usertask.Attributes.Add("class", "active");
        }
        else if (currentpage.Contains("/tasklog.aspx"))
        {

        }
        if (!IsPostBack)
        {

            getname();
        }
        }
    protected void getname()
    {
        string str = "select emp_name,roleid,f_name,l_name,DisplayName from tbl_employee where id='" + Request.Cookies["user"]["user_id"].ToString() + "' and status=1 and archive=0 and isnull(isactivelogin,0)!='2' ";
        DataSet ds = da.GetDataSetInline(str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            lblusername.Text = "Welcome " + ds.Tables[0].Rows[0]["DisplayName"].ToString() + "!";
            lblusername2.Text = "Welcome " + ds.Tables[0].Rows[0]["DisplayName"].ToString() + "!";
        }
        else
        {
            Response.Redirect("/login.aspx");
        }

    }
}
