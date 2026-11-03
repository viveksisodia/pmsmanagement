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
using System.Web.Services;
using System.Globalization;
public partial class setdurationtime : System.Web.UI.UserControl
{
    DataSet ds = new DataSet();
    DataAccess da = new DataAccess();
    public CommonFunction cmf = new CommonFunction();
    SqlParameter[] prm, prm1;
    public function fns = new function();
    protected void Page_Load(object sender, EventArgs e)
    {
      
    }
   
[WebMethod(EnableSession = true)]
public static string UpdateAccess(bool isChecked)
{
    try
    {
        int isonworkingdays = isChecked ? 1 : 0;

        int updateid = Convert.ToInt32(checkupdate());
        var cadmin = HttpContext.Current.Request.Cookies["cadmin"];
        var user = HttpContext.Current.Request.Cookies["user"];

        string regid = cadmin["id"];
        string uid = user["user_id"];
        DataAccess da = new DataAccess();
        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", updateid);
        prm[1] = new SqlParameter("@regid", regid);
        prm[2] = new SqlParameter("@uid", uid);
        prm[3] = new SqlParameter("@cdate", da.getcdate());
        prm[4] = new SqlParameter("@isworkingdays", isonworkingdays);

        if (updateid > 0)
        {
            prm[5] = new SqlParameter("@type", "update");
        }
        else
        {
            prm[5] = new SqlParameter("@type", "insert");
        }

        da.executeDMLQuery("Pms_tbl_durationsetting_usp", prm);

        return "Success";
    }
    catch (Exception ex)
    {
        return "Error: " + ex.Message;
    }
}
[WebMethod]
    public static string binddata()
    {
        DataAccess da = new DataAccess();
        string strsql = "select * from Pms_tbl_durationsetting where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and status=1 and archive=0";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            return "1";
        }
        return "0";
    }
    public static string checkupdate()
    {
        string retval = "0";
        DataAccess da = new DataAccess();
        string strsql = "select * from Pms_tbl_durationsetting where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and status=1 and archive=0";
        DataSet ds= da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            retval = ds.Tables[0].Rows[0]["id"].ToString();
        }

        return retval;

    }
}