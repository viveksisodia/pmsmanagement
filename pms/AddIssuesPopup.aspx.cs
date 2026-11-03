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

public partial class AddIssuesPopup : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataAccess da = new DataAccess();
    public CommonFunction cmf = new CommonFunction();
    SqlParameter[] prm, prm1;
    public function fns = new function();
    public string Projectname = "";
    public string strdisplay_close = "style='padding:14px 24px;display:'";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if(HttpContext.Current.Request.QueryString["ctype"]!=null)
            {
                strdisplay_close = "style='padding:14px 24px;display:none'";
            }
        }
        catch { }
        if (!Page.IsPostBack)
        {
            hdiprojid.Value = HttpContext.Current.Request.QueryString["id"].ToString();
            hdnprojecttype.Value = HttpContext.Current.Request.QueryString["type"].ToString();
            if (hdiprojid.Value != null)
            {
                bindprojectname();
            }
        }
    }

        protected void bindprojectname()
        {

            string strsql = "select id, ProjectName   from Pms_tbl_AddProject where id='" + hdiprojid.Value + "' and status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by id";

            DataSet dsN = da.GetDataSetInline(strsql);

            if (dsN.Tables[0].Rows.Count > 0)
            {
                Projectname = dsN.Tables[0].Rows[0]["ProjectName"].ToString();


            }



        }

  
}