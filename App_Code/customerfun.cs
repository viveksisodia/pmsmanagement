using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;
using CommonClass;
/// <summary>
/// Summary description for customer
/// </summary>
public class customerfun
{
    DataAccess da = new DataAccess();
    CommonFunction cmf = new CommonFunction();
   SqlParameter[]prm;
    public customerfun()
	{
		//
		// TODO: Add constructor logic here
		//
	}


    public string getplanedetails(string pid)
    {
        string strreturn = "";
        string strsql = "select * from plandetails where id='" + pid + "'";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            strreturn = ds.Tables[0].Rows[0]["pname"].ToString();
        }

        return strreturn;
             
    }

    public string gettotalnoofsearcheng()
    {
        string retstr = "0";

        string strsql = "select count(*) from searchengmaster where status=1 and archive=0";
        DataSet ds = da.GetDataSetInline(strsql);

        if (ds.Tables[0].Rows.Count > 0)
        {
            retstr = ds.Tables[0].Rows[0][0].ToString();
        }
        return retstr;

    }


    public DataSet gettotalsearcheng()
    {
       

        string strsql = "select * from searchengmaster where status=1 and archive=0";
        DataSet ds = da.GetDataSetInline(strsql);


        return ds;

    }



    public string gettotalsearchengamount(string sid,DateTime cdate)
    {

        string str = "0";
          prm = new SqlParameter[2];
          prm[0] = new SqlParameter("@sid", sid);
          prm[1] = new SqlParameter("@cdate", cdate);
          DataSet ds=da.GetDataSet("selectsearchcampainamount",prm);

          if (ds.Tables[0].Rows.Count > 0)
          {
              str = ds.Tables[0].Rows[0]["camount"].ToString();
          }


          return str;

    }


    public string gettotalnoofemp(DateTime cdate)
    {

        string str = "0";
        prm = new SqlParameter[1];       
        prm[0] = new SqlParameter("@cdate", cdate);
        DataSet ds = da.GetDataSet("selecttotalnoofemplogin", prm);

        if (ds.Tables[0].Rows.Count > 0)
        {
            str = ds.Tables[0].Rows[0]["totalagent"].ToString();
        }


        return str;

    }
    public string gettotalempsalary(DateTime cdate)
    {

        string str = "0";
        prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@cdate", cdate);
        DataSet ds = da.GetDataSet("selecttotalnoofempSalary", prm);

        if (ds.Tables[0].Rows.Count > 0)
        {
            str = ds.Tables[0].Rows[0]["totalsalary"].ToString();
        }

        decimal totalperday = Math.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["totalsalary"].ToString())/22,2);

        return totalperday.ToString();

    }

    public string gettotalexpansec(DateTime cdate,string eid,string etype)
    {

        string str = "0";
         decimal totalperday=0;
        prm = new SqlParameter[3];
         prm[0] = new SqlParameter("@eid", eid);
        prm[1] = new SqlParameter("@cdate", cdate);
        prm[2] = new SqlParameter("@etype", etype);
        DataSet ds = da.GetDataSet("selecttotalexpansec", prm);

        if (ds.Tables[0].Rows.Count > 0)
        {
            str = ds.Tables[0].Rows[0]["totalexp"].ToString();
        }

        if (etype == "1")
        {

             totalperday = Math.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["totalexp"].ToString()) / 30,2);
        }
        else
        {
            totalperday = Math.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["totalexp"].ToString()),2);
        }
        

        return totalperday.ToString();

    } 

    public string gettotalsall(DateTime cdate)
    {
        string str = "0";
        prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@fdate", cdate);
        DataSet ds = da.GetDataSet("selecttotalsale", prm);

        if (ds.Tables[0].Rows.Count > 0)
        {
            str = ds.Tables[0].Rows[0]["sales"].ToString();
        }

        return str;

    }

    public string gettotalsallrefund(DateTime cdate)
    {
        string str = "0";
        prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@fdate", cdate);
        DataSet ds = da.GetDataSet("selecttotalfund", prm);

        if (ds.Tables[0].Rows.Count > 0)
        {
            str = ds.Tables[0].Rows[0]["refund"].ToString();
        }

        return str;

    }

    public string gettotalsallchargeback(DateTime cdate)
    {
        string str = "0";
        prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@fdate", cdate);
        DataSet ds = da.GetDataSet("selecttotalcharegback", prm);

        if (ds.Tables[0].Rows.Count > 0)
        {
            str = ds.Tables[0].Rows[0]["chargeback"].ToString();
        }

        return str;

    }

    public DataSet gettotalExpenseMaster()
    {
        string str = "select * from ExpenseMaster where status=1 and archive=0";
        DataSet ds = da.GetDataSetInline(str);
        return ds;
    }

    public string getcustomerId(int id)
    {
        int appid = 1000 + id;

        string strarr = da.getcdate();
        char[] ch={'-'};
        string[] arr = strarr.Split(ch);

        string apptext = arr[2].ToString()+"/"+arr[0]+"/" + appid.ToString();
        return apptext;
    }

    public string getusername(string id)
    {
        string sqlstr = "select (firstname +' '+ lastname) as ename from la_userlogin where id=" + id;
        string retstr = "";
        DataSet ds = da.GetDataSetInline(sqlstr);
        if (ds.Tables[0].Rows.Count > 0)
        {
            retstr = ds.Tables[0].Rows[0]["ename"].ToString();
        }
        return retstr;
    }
}