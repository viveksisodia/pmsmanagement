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
public partial class _Default : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataAccess da = new DataAccess();
    public CommonFunction cmf = new CommonFunction();
    SqlParameter[] prm, prm1;
    public function fns = new function();
    public string Projectname = "";
    public string setmilstone = "";
    public string strquery = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            if (Request.QueryString["id"] != null)
            {
               string hdiprojid = HttpContext.Current.Request.QueryString["id"].ToString();
                if (hdiprojid!= null)
                {                   
                    strquery = "?id=" + hdiprojid;
                    bindprojectname(hdiprojid);
                }


            }
            if (Request.QueryString["type"] != null)
            {
                if (strquery != "")
                {
                    strquery += "&type=" + Request.QueryString["type"].ToString();
                }
                else
                {
                    strquery += "?type=" + Request.QueryString["type"].ToString();
                }
            }
        }
        }

    protected void bindprojectname(string pid)
    {

        string strsql = "select id, ProjectName   from Pms_tbl_AddProject where id='" + pid + "' and status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by id";

        DataSet dsN = da.GetDataSetInline(strsql);

        if (dsN.Tables[0].Rows.Count > 0)
        {
            Projectname = dsN.Tables[0].Rows[0]["ProjectName"].ToString();


        }



    }
}