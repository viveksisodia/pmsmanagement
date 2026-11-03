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
        
    }
    
}
