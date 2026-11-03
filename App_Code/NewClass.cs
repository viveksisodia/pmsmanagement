using System;
using System.Data.SqlClient;
using System.Collections;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CommonClass;
using System.Web.Configuration;


/// <summary>
/// Summary description for NewClass
/// </summary>
public class NewClass
{
    DataAccess da;
    CommonFunction cf;
    //public NewClass()
    //{
    //    //
    //    // TODO: Add constructor logic here
    //    //

    //}

    public void setdocread(string viewgroupid, string modifygroupid)
    {
        da = new DataAccess();
        cf = new CommonFunction();
        string viewgrpid = viewgroupid;
        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@groupid", viewgrpid);
        DataSet ds = da.GetDataSet("triveni_selectgroupusers", prm);
        foreach (DataRow theRow in ds.Tables[0].Rows)
        {
            string uid = theRow["uid"].ToString();
            setfinal(uid);
        }


        string modgrpid = modifygroupid;
        SqlParameter[] prm1 = new SqlParameter[1];
        prm1[0] = new SqlParameter("@groupid", modgrpid);
        DataSet ds1 = da.GetDataSet("triveni_selectgroupusers", prm1);
        foreach (DataRow theRow in ds1.Tables[0].Rows)
        {
            string uid = theRow["uid"].ToString();
            setfinal(uid);
        }
    }

    private void setfinal(string empid)
    {
        da = new DataAccess();
        cf = new CommonFunction();
        string uid = empid;
        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@empid", uid);
        da.executeDMLQuery("triveni_setfinalusers", prm);
    }

    public string Checkreadstatus(string empid)
    {
        da = new DataAccess();
        cf = new CommonFunction();
        string st = "0";
        string isread = "";
        string readdate = "";
        string uid = empid;
        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@empid", uid);
        DataSet ds = da.GetDataSet("triveni_checkreadstatus", prm);
        if (ds.Tables[0].Rows.Count > 0)
        {
            isread = ds.Tables[0].Rows[0]["kbdoc_read"].ToString();
            if (ds.Tables[0].Rows[0]["kbdoc_read_date"].ToString() != "")
            {
                readdate = cf.dateConvert(ds.Tables[0].Rows[0]["kbdoc_read_date"].ToString());
            }

            st = "1";
        }
        else
        {
            st = "0";
        }

        return st;
    }
    public string DateFormat(string c_date)
    {
        string retdate;
        if (c_date == "1/1/1900 12:00:00 AM")
        {
            return retdate = "";
        }

        else if (c_date != "")
        {
            string[] c_date1 = c_date.Split(' ');

            char[] ch = { '/' };
            string[] arrdate = c_date1[0].Split(ch);
            if (arrdate.Length >= 3)
            {
                if (arrdate[1].Length == 1)
                {
                    arrdate[1] = "0" + arrdate[1];
                }
                if (arrdate[0].Length == 1)
                {
                    arrdate[0] = "0" + arrdate[0];
                }

                retdate = arrdate[1] + "/" + arrdate[0] + "/" + arrdate[2] + " " + c_date1[1].ToString() + c_date1[2].ToString();

                return retdate;
            }
            else
            {
                return c_date;
            }
        }

        else
        {
            return c_date;
        }
    }
    public bool isauth()
    {
        da = new DataAccess();
        cf = new CommonFunction();
        bool retstr = false;
        int empid = Convert.ToInt32(HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());

        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@empid", empid);
        DataSet ds = da.GetDataSet("checkauth", prm);
        if (ds.Tables[0].Rows.Count > 0)
        {
            retstr = true;
        }
        return retstr;
    }

    public bool isadmin()
    {
        da = new DataAccess();
        cf = new CommonFunction();
        bool retstr = false;
        int empid = Convert.ToInt32(HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());

        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@empid", empid);
        DataSet ds = da.GetDataSet("checkadmin", prm);
        if (ds.Tables[0].Rows.Count > 0)
        {
            retstr = true;
        }
        return retstr;
    }

    public bool ishod()
    {
        da = new DataAccess();
        cf = new CommonFunction();
        bool retstr = false;
        int empid = Convert.ToInt32(HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());

        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@empid", empid);
        DataSet ds = da.GetDataSet("checkhod", prm);
        if (ds.Tables[0].Rows.Count > 0)
        {
            retstr = true;
        }
        return retstr;
    }

    public int gethod()
    {
        int hod = 0;
        da = new DataAccess();
        cf = new CommonFunction();
        int empid = Convert.ToInt32(HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@empid", empid);
        DataSet ds = da.GetDataSet("gethod", prm);
        if (ds.Tables[0].Rows.Count > 0)
        {
            hod = Convert.ToInt32(ds.Tables[0].Rows[0]["councelor_id"]);
        }
        return hod;
    }

    public int gethod(string e)
    {
        int hod = 0;
        da = new DataAccess();
        cf = new CommonFunction();
        int empid = Convert.ToInt32(e);
        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@empid", empid);
        DataSet ds = da.GetDataSet("gethod", prm);
        if (ds.Tables[0].Rows.Count > 0)
        {
            hod = Convert.ToInt32(ds.Tables[0].Rows[0]["councelor_id"]);
        }
        return hod;
    }

    public bool ishod2()
    {
        da = new DataAccess();
        cf = new CommonFunction();
        bool retstr = false;
        int empid = Convert.ToInt32(HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());

        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@empid", empid);
        DataSet ds = da.GetDataSet("checkhod2", prm);
        if (ds.Tables[0].Rows.Count > 0)
        {
            retstr = true;
        }
        return retstr;
    }

    public int gethod2()
    {
        int hod2 = 0;
        da = new DataAccess();
        cf = new CommonFunction();
        int empid = Convert.ToInt32(HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@empid", empid);
        DataSet ds = da.GetDataSet("gethod2", prm);
        if (ds.Tables[0].Rows.Count > 0)
        {
            hod2 = Convert.ToInt32(ds.Tables[0].Rows[0]["functionalhead_id"]); ;
        }
        return hod2;
    }

    public int gethod2(string e)
    {
        int hod2 = 0;
        da = new DataAccess();
        cf = new CommonFunction();
        int empid = Convert.ToInt32(e);
        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@empid", empid);
        DataSet ds = da.GetDataSet("gethod2", prm);
        if (ds.Tables[0].Rows.Count > 0)
        {
            hod2 = Convert.ToInt32(ds.Tables[0].Rows[0]["functionalhead_id"]); ;
        }
        return hod2;
    }

    public int ismoderator(string e)
    {
        da = new DataAccess();
        int moderator = 0;
        string sql = "select admin_id from admin_login where uid='" + e + "' and status='1' and archive='0'";
        DataSet ds = da.GetDataSetInline(sql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            moderator = 1;
        }
        return moderator;
    }

    public DataSet UserInfo()
    {
        da = new DataAccess();
        cf = new CommonFunction();
        int empid = Convert.ToInt32(HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        SqlParameter[] nprm = new SqlParameter[1];
        nprm[0] = new SqlParameter("@uid", empid);
        DataSet nds = da.GetDataSet("SetSession", nprm);
        return nds;
    }

    public DataSet UserInfo(int uid)
    {
        da = new DataAccess();
        cf = new CommonFunction();
        int empid = uid;
        SqlParameter[] nprm = new SqlParameter[1];
        nprm[0] = new SqlParameter("@uid", empid);
        DataSet nds = da.GetDataSet("SetSession", nprm);
        return nds;
    }

    public string ExportToExcel(string proc)
    {
        da = new DataAccess();
        cf = new CommonFunction();
        string str = "";
        return str;
    }

    public string GetMailURL()
    {
        da = new DataAccess();
        cf = new CommonFunction();
        string mailurl = "";

        mailurl = WebConfigurationManager.AppSettings["MailUrl"].ToString();
        //HttpContext.Current.Response.Write(mailurl);
        //HttpContext.Current.Response.End();
        return mailurl;
    }
}