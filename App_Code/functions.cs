using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CommonClass;
using System.Data.SqlClient;
/// <summary>
/// Summary description for functions
/// </summary>
public class functions
{
    ListItem li;
    DataAccess da=new DataAccess();
    CommonFunction cmf=new CommonFunction();
	public functions()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public string getltext(string stext, string lid)
    {
        string returnstr = stext;
        dataaccesscomm dacom = new dataaccesscomm();
        try
        {
            if (HttpContext.Current.Request.Cookies["language"] != null)
            {

                if (HttpContext.Current.Request.Cookies["language"]["id"].ToString() != "1")
                {
                    SqlParameter[] prm = new SqlParameter[2];
                    prm[0] = new SqlParameter("@sname", stext.Trim());

                    prm[1] = new SqlParameter("@lid", HttpContext.Current.Request.Cookies["language"]["id"].ToString());

                    DataSet ds = dacom.GetDataSet("selectstringmastertranslater", prm);
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        if (HttpContext.Current.Request.Cookies["witheng"] != null)
                        {
                            if (HttpContext.Current.Request.Cookies["witheng"]["id"] != null)
                            {
                                try
                                {
                                    if (HttpContext.Current.Request.Cookies["witheng"]["id"] == "1")
                                    {
                                        returnstr += " (" + ds.Tables[0].Rows[0]["tsname"].ToString() + ")";
                                    }
                                    else
                                    {
                                        returnstr = ds.Tables[0].Rows[0]["tsname"].ToString();
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                            else
                            {
                                returnstr = ds.Tables[0].Rows[0]["tsname"].ToString();
                            }
                        }
                        else
                        {
                            returnstr = ds.Tables[0].Rows[0]["tsname"].ToString();
                        }



                    }
                }
            }
        }
        catch (Exception ex)
        {

        }

        return returnstr;
    }
    public string getApplicationId(int id)
    {
        int appid = 1000 + id;
        string apptext = "AQCAP" + appid.ToString();
        string strsql = "select * from consultant_registration where id='" + id + "'";
        DataSet dsappid = da.GetDataSetInline(strsql);
        if (dsappid.Tables[0].Rows.Count > 0)
        {
            if (dsappid.Tables[0].Rows[0]["applicationfor"].ToString() == "2")
            {
                apptext = "ROHSAP" + appid.ToString();
            }
        }

    
        return apptext;
    }

    public void bindddlhr(DropDownList ddl)
    {
        int i;
        for (i = 1; i <= 12; i++)
        {
            if (i < 10)
            {
                string j = "0" + i;
                li = new ListItem(Convert.ToString(j), Convert.ToString(j));
            }
            else
            {
                li = new ListItem(Convert.ToString(i), Convert.ToString(i));
            }
            ddl.Items.Add(li);
        }
    }

    public void bindddlmm(DropDownList ddl)
    {
        int i;
        for (i = 0; i <= 59; i++)
        {
            if (i < 10)
            {
                string j = "0" + i;
                li = new ListItem(Convert.ToString(j), Convert.ToString(j));
            }
            else
            {
                li = new ListItem(Convert.ToString(i), Convert.ToString(i));
            }
            ddl.Items.Add(li);
            //i = i + 4;
        }
    }



    public string getaftype(string afid)
    {
        string retstr = "AQC";
        if (afid == "2")
        {
            retstr = "ROHS";
        }

        return retstr;
    }


    public string getpftype(string afid)
    {
        string retstr = "AQC";
        if (afid == "2")
        {
            retstr = "ROHS";
        }

        return retstr;
    }


    public string getcontractreviewIdbywid(string id)
    {
        string strret = string.Empty;
        string sqlstr = "select review from contractreview where id in (select cr_id from workorder2 where id='" + id + "')";

        DataSet ds = da.GetDataSetInline(sqlstr);
        if (ds.Tables[0].Rows.Count > 0)
        {
            strret = getEmpemailById(ds.Tables[0].Rows[0]["review"].ToString());
        }
        else
        {
            strret = " ";
        }
        return strret;
    }


    public string getdocno(string id)
    {
        string strret = string.Empty;
        string sqlstr = "select docfrom from Assign_DocketNo where id='" + id + "' ";

        DataSet ds = da.GetDataSetInline(sqlstr);
        if (ds.Tables[0].Rows.Count > 0)
        {
            strret = ds.Tables[0].Rows[0]["docfrom"].ToString();
        }
        else
        {
            strret = " ";
        }
        return strret;
    }




    public string getConsultantremainingamount(string id)
    {
        string strret = string.Empty;
        string sqlstr = "select remainingamount from babalance_new where baid='" + id + "' order by id desc";
      
        DataSet ds = da.GetDataSetInline(sqlstr);
        if (ds.Tables[0].Rows.Count > 0)
        {
            strret = ds.Tables[0].Rows[0]["remainingamount"].ToString();
        }
        else
        {
            strret = "0";
        }
        return strret;
    }


    public string getConsultantNameById(string id)
    {
        string strret = string.Empty;
        string sqlstr = "select name from consultants where id='" + id + "'";

        DataSet ds = da.GetDataSetInline(sqlstr);
        if (ds.Tables[0].Rows.Count > 0)
        {
            strret = ds.Tables[0].Rows[0]["name"].ToString();
        }
        return strret;
    }
    public string getCompanynameBywId(string wid)
    {
        string strret = string.Empty;
        SqlParameter[] prm=new SqlParameter[1];
        prm[0] = new SqlParameter("@wid", wid);

        DataSet ds = da.GetDataSet("getcompanynamebyworkorderid", prm);
        if (ds.Tables[0].Rows.Count > 0)
        {
            strret = ds.Tables[0].Rows[0]["companyname"].ToString();
        }
        return strret;
    }
    public string getClientnameBywId(string wid)
    {
        string strret = string.Empty;
        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@wid", wid);

        DataSet ds = da.GetDataSet("getcompanynamebyworkorderid", prm);
        if (ds.Tables[0].Rows.Count > 0)
        {
            strret = ds.Tables[0].Rows[0]["person"].ToString();
        }
        return strret;
    }
    public string getaudidBywId(string wid)
    {
        string strret = string.Empty;
        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@wid", wid);

        DataSet ds = da.GetDataSet("getcompanynamebyworkorderid", prm);
        if (ds.Tables[0].Rows.Count > 0)
        {
            strret = ds.Tables[0].Rows[0]["aud_id"].ToString();
        }
        return strret;
    }

    public string getCouriornamebyid(string id)
    {
        string strret = string.Empty;
      string strsql="select cname from couriermaster where id="+id;

      DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            strret = ds.Tables[0].Rows[0]["cname"].ToString();
        }
        return strret;
    }
    public string getExpertOrAuditorFromCr(string cid)
    {
        string sqlstr = "select laud_id,tech_id  from contractreview where id='" + cid + "'";
        string retstr = "";
        DataSet ds = da.GetDataSetInline(sqlstr);

        int numrows = ds.Tables[0].Rows.Count;
        if (numrows > 0)
        {
            if (ds.Tables[0].Rows[0]["laud_id"].ToString() != "")
            {
                retstr =getAuditorsNameById(ds.Tables[0].Rows[0]["laud_id"].ToString()) + " ( LA ) ";
            }
            if (ds.Tables[0].Rows[0]["tech_id"].ToString() != "")
            {
                if (getTechExpsNameById(ds.Tables[0].Rows[0]["tech_id"].ToString()) != string.Empty)
                {
                    if (retstr != string.Empty)
                    {

                        retstr += "/" + getTechExpsNameById(ds.Tables[0].Rows[0]["tech_id"].ToString()) + " ( TE ) ";
                    }
                    else
                    {
                        retstr += getTechExpsNameById(ds.Tables[0].Rows[0]["tech_id"].ToString()) + " ( TE ) ";
                    }
                }
            }

        }
        return retstr;
    }



    public string getExpertOrAuditorFromCrservillence(string cid)
    {
        string sqlstr = "select laud_id,tech_id  from servellienceauditerselection where id='" + cid + "'";
        string retstr = "";
        DataSet ds = da.GetDataSetInline(sqlstr);

        int numrows = ds.Tables[0].Rows.Count;
        if (numrows > 0)
        {
            if (ds.Tables[0].Rows[0]["laud_id"].ToString() != "")
            {
                retstr = getAuditorsNameById(ds.Tables[0].Rows[0]["laud_id"].ToString()) + " ( LA ) ";
            }
            if (ds.Tables[0].Rows[0]["tech_id"].ToString() != "")
            {
                if (getTechExpsNameById(ds.Tables[0].Rows[0]["tech_id"].ToString()) != string.Empty)
                {
                    if (retstr != string.Empty)
                    {

                        retstr += "/" + getTechExpsNameById(ds.Tables[0].Rows[0]["tech_id"].ToString()) + " ( TE ) ";
                    }
                    else
                    {
                        retstr += getTechExpsNameById(ds.Tables[0].Rows[0]["tech_id"].ToString()) + " ( TE ) ";
                    }
                }
            }

        }
        return retstr;
    }



    public string getExpertOrAuditorFromCrservillence2(string cid)
    {
        string sqlstr = "select laud_id,tech_id  from servellience2auditerselection where id='" + cid + "'";
        string retstr = "";
        DataSet ds = da.GetDataSetInline(sqlstr);

        int numrows = ds.Tables[0].Rows.Count;
        if (numrows > 0)
        {
            if (ds.Tables[0].Rows[0]["laud_id"].ToString() != "")
            {
                retstr = getAuditorsNameById(ds.Tables[0].Rows[0]["laud_id"].ToString()) + " ( LA ) ";
            }
            if (ds.Tables[0].Rows[0]["tech_id"].ToString() != "")
            {
                if (getTechExpsNameById(ds.Tables[0].Rows[0]["tech_id"].ToString()) != string.Empty)
                {
                    if (retstr != string.Empty)
                    {

                        retstr += "/" + getTechExpsNameById(ds.Tables[0].Rows[0]["tech_id"].ToString()) + " ( TE ) ";
                    }
                    else
                    {
                        retstr += getTechExpsNameById(ds.Tables[0].Rows[0]["tech_id"].ToString()) + " ( TE ) ";
                    }
                }
            }

        }
        return retstr;
    }


    public string getSubCategoryCodeName(string id)
    {
        string sqlstr = "select code from subcategory where id in( select * from [dbo].[UF_CSVToTable]('" + id + "'))";
        string retstr = "";
        DataSet ds = da.GetDataSetInline(sqlstr);
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            if (retstr != string.Empty)
            {
                retstr += "," + ds.Tables[0].Rows[i]["code"].ToString();
            }
            else
            {
                retstr += ds.Tables[0].Rows[i]["code"].ToString();
            }
        }
        return retstr;

    }


    public string getrohsSubCategoryCodeName(string id)
    {
        string sqlstr = "select code from rohssubcategory where id in( select * from [dbo].[UF_CSVToTable]('" + id + "'))";
        string retstr = "";
        DataSet ds = da.GetDataSetInline(sqlstr);
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            if (retstr != string.Empty)
            {
                retstr += "," + ds.Tables[0].Rows[i]["code"].ToString();
            }
            else
            {
                retstr += ds.Tables[0].Rows[i]["code"].ToString();
            }
        }
        return retstr;

    }


    public string getfoodCodeName(string id)
    {
        string sqlstr = "select * from Food_codemaster where id ='" + id + "'";
        string retstr = "";
        DataSet ds = da.GetDataSetInline(sqlstr);
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            if (retstr != string.Empty)
            {
                retstr += "," + ds.Tables[0].Rows[i]["fcode"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[i]["fdesc"].ToString() + ")"; 
            }
            else
            {
                retstr += ds.Tables[0].Rows[i]["fcode"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[i]["fdesc"].ToString() + ")";
            }
        }
        return retstr;

    }


    public string getfoodCodedescName(string id)
    {
        string sqlstr = "select fdesc from Food_codemaster where id ='" + id + "'";
        string retstr = "";
        DataSet ds = da.GetDataSetInline(sqlstr);
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            if (retstr != string.Empty)
            {
                retstr += "," + ds.Tables[0].Rows[i]["fdesc"].ToString();
            }
            else
            {
                retstr += ds.Tables[0].Rows[i]["fdesc"].ToString();
            }
        }
        return retstr;

    }

    public string getWorkOrderReferenceNumber(string year)
    {
        string date = year;
        string yy = GetLast(year,2);  
        string wid = string.Empty;
        string clientref = string.Empty;
        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@year", year);
        DataSet ds = da.GetDataSet("getnewworkorderseriesno",prm);
        if (ds.Tables[0].Rows.Count > 0)
        {
            wid = "10" + yy + ds.Tables[0].Rows[0]["seriesno"].ToString()+"/" + date;
            clientref = "10" + yy + ds.Tables[0].Rows[0]["seriesno"].ToString();
        }
        return wid + "##" + clientref;
    }
  

    public string getrohsWorkOrderReferenceNumber(string year)
    {
        string date = year;
        string yy = GetLast(year, 2);
        string wid = string.Empty;
        string clientref = string.Empty;
        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@year", year);
        DataSet ds = da.GetDataSet("rohsgetnewworkorderseriesno", prm);
        if (ds.Tables[0].Rows.Count > 0)
        {
            wid = "13" + yy + ds.Tables[0].Rows[0]["seriesno"].ToString() + "/" + date;
            clientref = "13" + yy + ds.Tables[0].Rows[0]["seriesno"].ToString();
        }
        return wid + "##" + clientref;
    }
    public string getWorkOrderReferenceNumberSurveillance(string cid,string type)
    {
       
        string wid = string.Empty;
        string clientref = string.Empty;
        SqlParameter[] prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@cid", cid);
        prm[1] = new SqlParameter("@type", type);
        DataSet ds = da.GetDataSet("Surveillancegetnewworkorderseriesno",prm);
        if (ds.Tables[0].Rows.Count > 0)
        {
            wid = ds.Tables[0].Rows[0]["workrefnumber"].ToString() + "/S1";
            clientref =  ds.Tables[0].Rows[0]["clientref"].ToString()+"/S1";
        }
        return wid + "##" + clientref;
    }
    public string getWorkOrderReferenceNumberSurveillance2(string cid, string type)
    {

        string wid = string.Empty;
        string clientref = string.Empty;
        SqlParameter[] prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@cid", cid);
        prm[1] = new SqlParameter("@type", type);
        DataSet ds = da.GetDataSet("Surveillancegetnewworkorderseriesno", prm);
        if (ds.Tables[0].Rows.Count > 0)
        {
            wid = ds.Tables[0].Rows[0]["workrefnumber"].ToString() + "/S2";
            clientref = ds.Tables[0].Rows[0]["clientref"].ToString() + "/S2";
        }
        return wid + "##" + clientref;
    }
    public static string GetLast(string s, int tail_length)
    {
        if (tail_length >= s.Length)
            return s;
        return s.Substring(s.Length - tail_length);
    }
    public string getWorkOrderReferenceNumberByAppId(int type, string year)
    {
        string date = year;
        string wid = string.Empty;
        string clientref = string.Empty;
        string table = string.Empty;
        if (type == 1) { table = "workorder"; }
        if (type == 2) { table = "workorder2"; }


        string excQry = "select top 1 clientref from " + table + " where YEAR( cast(issuedate as Datetime)) ='" + year + "' and issuedate!='' order by id  desc ";
       // HttpContext.Current.Response.Write(excQry);

      //  HttpContext.Current.Response.End();
        DataSet ds = da.GetDataSetInline(excQry);

        int numrows = ds.Tables[0].Rows.Count;


        if (numrows == 0)
        {
            if (type == 2)
            {
                wid = "10001/" + date;
                clientref = "10001";
            }
            else if (type == 1)
            {
                wid = "10001/" + date;
                clientref = "10001";
            }

        }
        else
        {
            int excid = Convert.ToInt32(ds.Tables[0].Rows[0]["clientref"].ToString().Replace("J",""));

            int finalid = 1 + excid;

            if (type == 2)
            {

                wid = "" + finalid.ToString() + "/" + date;
                clientref = "" + finalid.ToString();
            }
            else if (type == 1)
            {
                wid = finalid.ToString() + "/" + date;
                clientref = finalid.ToString();
            }

        }
        return wid + "##" + clientref;
    }

    public DataSet getempdetailbybaid(string id)
    {
        DataSet ds = new DataSet();

        string str = "select employee.*,consultants.name as baname from consultants inner join employee on  consultants.reportto=employee.id where consultants.id='" + id + "'";
        ds = da.GetDataSetInline(str);

        return ds;
    }


        public bool  matchPublicHoliday(string date)
        {
            //string sqlstr = "select *,cast(eventdate as Datetime) from holidays where cast(eventdate as Datetime)>=cast(" + date + " as Datetime) and status=1";

            string sqlstr = "select * from holidays where  convert(char(10),cast(eventdate as Datetime),101)=convert(char(10),cast('" + date + "' as Datetime),101) and status=1";


           // HttpContext.Current.Response.Write(sqlstr);

            DataSet ds = da.GetDataSetInline(sqlstr);

            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                sqlstr = "select datename (dw, '" + date + "') as NameOfDay ";
                string dayname = da.getexecutescalar(sqlstr);
               
                if (dayname.Trim() == "Sunday")
                {
                  //  HttpContext.Current.Response.Write(date+" " +dayname);

                    //HttpContext.Current.Response.End();
                    return true;
                }
            }

           return false;

        }


        public string getWorkOrderDate(string cid)
        {
            string selQry = "select doc  from contractreview where id=" + cid;

            string cdate = da.getexecutescalar(selQry);
            if (cdate == string.Empty)
            {
                return da.getcdate();
            }
            else
            {
                return getCertificationDate(cdate);
            }
            //$workorderArr=getCertificationDate($date,getPublicHoliday());
            //$workOrderDate=$workorderArr[5];
            //return $workOrderDate;
        }

        public string getWorkOrderDateservellience(string cid)
        {
            string selQry = "select doc  from servellienceauditerselection where id=" + cid;

            string cdate = da.getexecutescalar(selQry);
            if (cdate == string.Empty)
            {
                return da.getcdate();
            }
            else
            {
                return getCertificationDateSurveillance(cdate);
            }
            //$workorderArr=getCertificationDate($date,getPublicHoliday());
            //$workOrderDate=$workorderArr[5];
            //return $workOrderDate;
        }

        public string getWorkOrderDateservellience2(string cid)
        {
            string selQry = "select doc  from servellience2auditerselection where id=" + cid;

            string cdate = da.getexecutescalar(selQry);
            if (cdate == string.Empty)
            {
                return da.getcdate();
            }
            else
            {
                return getCertificationDateSurveillance(cdate);
            }
            //$workorderArr=getCertificationDate($date,getPublicHoliday());
            //$workOrderDate=$workorderArr[5];
            //return $workOrderDate;
        }


        public string getCertificationDateSurveillance(string cdate)
        {
            DateTime wdate = Convert.ToDateTime(cdate).AddDays(-9);

            string selsun = "select [dbo].[getatotalnoofsunday]('" + Convert.ToDateTime(cdate) + "','" + wdate + "')";
            string sunno = da.getexecutescalar(selsun);
            if (sunno != "0")
            {
                int sn = 0 - Convert.ToInt32(sunno);
                wdate = Convert.ToDateTime(wdate).AddDays(sn);
            }

            string selholi = "select [dbo].[getatotalnoofholiday] ('" + Convert.ToDateTime(cdate) + "','" + wdate + "')";
            string selholino = da.getexecutescalar(selsun);

            try
            {
                if (selholi != "0")
                {
                    int sn = 0 - Convert.ToInt32(selholi);
                    wdate = Convert.ToDateTime(wdate).AddDays(sn);
                }
            }
            catch (Exception ex)
            {

            }
        vvk:
            if (matchPublicHoliday(wdate.ToShortDateString()))
            {
                wdate = Convert.ToDateTime(wdate).AddDays(-1);
                goto vvk;
            }

            return wdate.ToShortDateString();
        }

    public string getCertificationDate(string cdate)
    {
        DateTime wdate =Convert.ToDateTime(cdate).AddDays(-15);

        string selsun = "select [dbo].[getatotalnoofsunday]('" + Convert.ToDateTime(cdate) + "','" + wdate + "')";
       string sunno= da.getexecutescalar(selsun);
       if (sunno != "0")
       {
           int sn=0-Convert.ToInt32(sunno);
           wdate =Convert.ToDateTime(wdate).AddDays(sn);
       }

       string selholi= "select [dbo].[getatotalnoofholiday] ('" + Convert.ToDateTime(cdate) + "','" + wdate + "')";
       string selholino = da.getexecutescalar(selsun);

       try
       {
           if (selholi != "0")
           {
               int sn = 0 - Convert.ToInt32(selholi);
               wdate = Convert.ToDateTime(wdate).AddDays(sn);
           }
       }
       catch (Exception ex)
       {

       }
   vvk:
       if (matchPublicHoliday(wdate.ToShortDateString()))
       {
           wdate = Convert.ToDateTime(wdate).AddDays(-1);
           goto vvk;
       }

       return wdate.ToShortDateString();
    }



    public string getCertificationstagedate(string cdate)
    {


        DateTime wdate = Convert.ToDateTime(cdate).AddDays(-15);

        string selsun = "select [dbo].[getatotalnoofsunday]('" + Convert.ToDateTime(cdate) + "','" + wdate + "')";
        string sunno = da.getexecutescalar(selsun);
        if (sunno != "0")
        {
            int sn = 0 - Convert.ToInt32(sunno);
            wdate = Convert.ToDateTime(wdate).AddDays(sn);
        }
    vvk:
        if (matchPublicHoliday(wdate.ToShortDateString()))
        {
            wdate = Convert.ToDateTime(wdate).AddDays(-1);
            goto vvk;
        }

        string retstr = string.Empty;
        string cerTificationApprovalDate = Convert.ToDateTime(cdate).AddDays(-1).ToShortDateString();

    vvk1:
        if (matchPublicHoliday(cerTificationApprovalDate))
        {
            cerTificationApprovalDate = Convert.ToDateTime(cerTificationApprovalDate).AddDays(-1).ToShortDateString();
            goto vvk1;
        }


        string cerTificationIssueCheckListDate = Convert.ToDateTime(cerTificationApprovalDate).AddDays(-1).ToShortDateString();

    vvk2:
        if (matchPublicHoliday(cerTificationIssueCheckListDate))
        {
            cerTificationIssueCheckListDate = Convert.ToDateTime(cerTificationIssueCheckListDate).AddDays(-1).ToShortDateString();
            goto vvk2;
        }
        string clientFeedbackDate = Convert.ToDateTime(cerTificationIssueCheckListDate).AddDays(-1).ToShortDateString();

    vvk3:
        if (matchPublicHoliday(clientFeedbackDate))
        {
            clientFeedbackDate = Convert.ToDateTime(clientFeedbackDate).AddDays(-1).ToShortDateString();
            goto vvk3;
        }

        string stageIIauditdate = Convert.ToDateTime(clientFeedbackDate).AddDays(-1).ToShortDateString();

    vvk4:
        if (matchPublicHoliday(stageIIauditdate))
        {
            stageIIauditdate = Convert.ToDateTime(stageIIauditdate).AddDays(-1).ToShortDateString();
            //HttpContext.Current.Response.Write(stageIIauditdate);
            
            goto vvk4;
        }

    
        string stag1auditdate = wdate.AddDays(3).ToShortDateString();
    vvk5:
        if (matchPublicHoliday(stag1auditdate))
        {
            stag1auditdate = Convert.ToDateTime(stag1auditdate).AddDays(-1).ToShortDateString();
            goto vvk5;
        }

        retstr = wdate + "###" + cerTificationApprovalDate + "###" + cerTificationIssueCheckListDate + "###" + clientFeedbackDate + "###" + stag1auditdate + "###" + stageIIauditdate;
        return retstr;
    }

    public string getCertificationstagedateSurveillance(string cdate)
    {


        DateTime wdate = Convert.ToDateTime(cdate).AddDays(-9);

        string selsun = "select [dbo].[getatotalnoofsunday]('" + Convert.ToDateTime(cdate) + "','" + wdate + "')";
        string sunno = da.getexecutescalar(selsun);
        if (sunno != "0")
        {
            int sn = 0 - Convert.ToInt32(sunno);
            wdate = Convert.ToDateTime(wdate).AddDays(sn);
        }
    vvk:
        if (matchPublicHoliday(wdate.ToShortDateString()))
        {
            wdate = Convert.ToDateTime(wdate).AddDays(-1);
            goto vvk;
        }

        string retstr = string.Empty;
        string cerTificationApprovalDate = Convert.ToDateTime(cdate).AddDays(-2).ToShortDateString();

    vvk1:
        if (matchPublicHoliday(cerTificationApprovalDate))
        {
            cerTificationApprovalDate = Convert.ToDateTime(cerTificationApprovalDate).AddDays(-1).ToShortDateString();
            goto vvk1;
        }


        string cerTificationIssueCheckListDate = Convert.ToDateTime(cerTificationApprovalDate).AddDays(-1).ToShortDateString();

    vvk2:
        if (matchPublicHoliday(cerTificationIssueCheckListDate))
        {
            cerTificationIssueCheckListDate = Convert.ToDateTime(cerTificationIssueCheckListDate).AddDays(-1).ToShortDateString();
            goto vvk2;
        }
    //    string reportreviewdate = Convert.ToDateTime(cerTificationIssueCheckListDate).AddDays(-1).ToShortDateString();

    //vvk3:
    //    if (matchPublicHoliday(reportreviewdate))
    //    {
    //        reportreviewdate = Convert.ToDateTime(reportreviewdate).AddDays(-1).ToShortDateString();
    //        goto vvk3;
    //    }

    string stageIIauditdate = Convert.ToDateTime(cerTificationIssueCheckListDate).AddDays(-2).ToShortDateString();

    vvk4:
        if (matchPublicHoliday(stageIIauditdate))
        {
            stageIIauditdate = Convert.ToDateTime(stageIIauditdate).AddDays(-1).ToShortDateString();
            //HttpContext.Current.Response.Write(stageIIauditdate);

            goto vvk4;
        }


    //    string stag1auditdate = wdate.AddDays(3).ToShortDateString();
    //vvk5:
    //    if (matchPublicHoliday(stag1auditdate))
    //    {
    //        stag1auditdate = Convert.ToDateTime(stag1auditdate).AddDays(-1).ToShortDateString();
    //        goto vvk5;
    //    }

        retstr = wdate + "###" + cerTificationApprovalDate + "###" + cerTificationIssueCheckListDate + "###" + stageIIauditdate;
        return retstr;
    }
    public string getPrimarySiteNameById(string rid)
    {
        string retstr=string.Empty;

        string str = "select top 1  site_addr  from consultant_site where r_id='" + rid + "' and archive='0' order by id desc";
        retstr = da.getexecutescalar(str);

        return retstr;
    }

    public string getPrimarycertificateId(string rid,string type)
    {
            string retstr = string.Empty;
            string Query = "";
            if (type == "2")
            {
                 Query = "select finalcertificate.id as fcid  from finalcertificate inner join AuditReviewchecklist on ";
                Query += "finalcertificate.chk_id=AuditReviewchecklist.id   inner join  workorder2 on AuditReviewchecklist.wid=workorder2.id inner join ";
                Query += "contractreview on workorder2.cr_id=contractreview.id inner join consultant_registration on contractreview.app_id=consultant_registration.id ";
                Query += "where finalcertificate.status=0 and finalcertificate.type='" + type + "' and consultant_registration.id='" + rid + "'";
            }
            else
            {
                Query = "select finalcertificate.id as fcid  from finalcertificate inner join AuditReviewchecklist on ";
                Query += "finalcertificate.chk_id=AuditReviewchecklist.id   inner join  workorder on AuditReviewchecklist.wid=workorder.id inner join ";
                Query += "contractreview on workorder.cr_id=contractreview.id inner join consultant_registration on contractreview.app_id=consultant_registration.id ";
                Query += "where finalcertificate.status=0 and finalcertificate.type='" + type + "' and consultant_registration.id='" + rid + "'";
            }
            retstr = da.getexecutescalar(Query);

        return retstr;
    }


    public string getCertificationNumbertype1(string refnum, string type)
    {
        string retst = "";
        // string[] arr = refnum.Split('/');
        string show1 = refnum.Substring(0, 4);
        string show2 = refnum.Substring(4, refnum.Length - 4);
        //if (type == "1")
        //{
        //    retst = refnum;
        //}
        //else if (type == "2")
        //{
            retst = show1 + "Q" + show2;
        //}

        return retst;
    }
    public string getCertificationNumbertype1isms(string refnum, string type)
    {
        string retst = "";
        // string[] arr = refnum.Split('/');
        string show1 = refnum.Substring(0, 4);
        string show2 = refnum.Substring(4, refnum.Length - 4);
        //if (type == "1")
        //{
        //    retst = refnum;
        //}
        //else if (type == "2")
        //{
        retst = show1 + "I" + show2;
        //}

        return retst;
    }


    public string getCertificationNumbertype1food(string refnum, string type)
    {
        string retst = "";
        // string[] arr = refnum.Split('/');
        string show1 = refnum.Substring(0, 4);
        string show2 = refnum.Substring(4, refnum.Length - 4);
        //if (type == "1")
        //{
        //    retst = refnum;
        //}
        //else if (type == "2")
        //{
        retst = show1 + "F" + show2;
        //}

        return retst;
    }
    public string getCertificationNumbertype1CE(string refnum, string type)
    {
        string retst = "";
        // string[] arr = refnum.Split('/');
        string show1 = refnum.Substring(0, 4);
        string show2 = refnum.Substring(4, refnum.Length - 4);
        //if (type == "1")
        //{
        //    retst = refnum;
        //}
        //else if (type == "2")
        //{
        retst = show1 + "C" + show2;
        //}

        return retst;
    }
    public string getCertificationNumbertype1it(string refnum, string type)
    {
        string retst = "";
        // string[] arr = refnum.Split('/');
        string show1 = refnum.Substring(0, 4);
        string show2 = refnum.Substring(4, refnum.Length - 4);
        //if (type == "1")
        //{
        //    retst = refnum;
        //}
        //else if (type == "2")
        //{
        retst = show1 + "IT" + show2;
        //}

        return retst;
    }
    public string getCertificationNumbertype1HACCP(string refnum, string type)
    {
        string retst = "";
        // string[] arr = refnum.Split('/');
        string show1 = refnum.Substring(0, 4);
        string show2 = refnum.Substring(4, refnum.Length - 4);
        //if (type == "1")
        //{
        //    retst = refnum;
        //}
        //else if (type == "2")
        //{
        retst = show1 + "H" + show2;
        //}

        return retst;
    }
    public string getCertificationNumbertype1GMP(string refnum, string type)
    {
        string retst = "";
        // string[] arr = refnum.Split('/');
        string show1 = refnum.Substring(0, 4);
        string show2 = refnum.Substring(4, refnum.Length - 4);
        //if (type == "1")
        //{
        //    retst = refnum;
        //}
        //else if (type == "2")
        //{
        retst = show1 + "G" + show2;
        //}

        return retst;
    }

    public string getCertificationNumbertype1ohsas(string refnum, string type)
    {
        string retst = "";
        // string[] arr = refnum.Split('/');
        string show1 = refnum.Substring(0, 4);
        string show2 = refnum.Substring(4, refnum.Length - 4);
        //if (type == "1")
        //{
        //    retst = refnum;
        //}
        //else if (type == "2")
        //{
        retst = show1 + "O" + show2;
        //}

        return retst;
    }

    public string getCertificationNumbertype1rohs(string refnum, string type)
    {
        string retst = "";
        // string[] arr = refnum.Split('/');
        string show1 = refnum.Substring(0, 4);
        string show2 = refnum.Substring(4, refnum.Length - 4);
        //if (type == "1")
        //{
        //    retst = refnum;
        //}
        //else if (type == "2")
        //{
        retst = show1 + "R" + show2;
        //}

        return retst;
    }

    public string getCertificationNumbertype1ems(string refnum, string type)
    {
        string retst = "";
        // string[] arr = refnum.Split('/');
        string show1 = refnum.Substring(0, 4);
        string show2 = refnum.Substring(4, refnum.Length - 4);
        //if (type == "1")
        //{
        //    retst = refnum;
        //}
        //else if (type == "2")
        //{
        retst = show1 + "E" + show2;
        //}

        return retst;
    }

    public string getCertificationNumbertype1MEDITECH(string refnum, string type)
    {
        string retst = "";
        // string[] arr = refnum.Split('/');
        string show1 = refnum.Substring(0, 4);
        string show2 = refnum.Substring(4, refnum.Length - 4);
        //if (type == "1")
        //{
        //    retst = refnum;
        //}
        //else if (type == "2")
        //{
        retst = show1 + "M" + show2;
        //}

        return retst;
    }

    public string getCertificationNumbertype1old(string refnum, string type)
    {
        string retst = string.Empty;
        int countlength=refnum.Length;
        int spoint = countlength - 2;
        int lpoint = countlength - 1;
        string last2 = refnum.Substring(spoint);
        string[] arr=refnum.Split('/');
        if (type == "1")
        {
            retst = "1012" + arr[0];
        }
        else if (type == "2")
        {
           
            retst = "10" +last2+"/Q/"+arr[0].Substring(1, arr[0].Length);
        }

        return retst;
    }


    public string getCertificationNumber(string refnum, string type)
    {
        string retst = string.Empty;
        int countlength = refnum.Length;
        int spoint = countlength - 2;
        int lpoint = countlength - 1;
        string last2 = refnum.Substring(spoint);
        string[] arr = refnum.Split('/');
        if (type == "1")
        {
            retst = "1012" + arr[0];
        }
        else if (type == "2")
        {
            string sqlstr="select cert_no  from finalcertificate where cert_no like '10"+last2+"%' and type='"+type+"'  order by id desc";
            //HttpContext.Current.Response.Write(sqlstr);
            //HttpContext.Current.Response.End();


          DataSet ds=da.GetDataSetInline(sqlstr);
          if (ds.Tables[0].Rows.Count > 0)
          {
              int finalid = 1 + Convert.ToInt32(ds.Tables[0].Rows[0][0]);
              retst = finalid.ToString();
              //HttpContext.Current.Response.Write(sqlstr);
              //HttpContext.Current.Response.End();
		
		  }
          else
          {

              retst = "10" + last2 + "10001";	 
          }
            
           
        }

        return retst;
    }

    public string getFrequency(string num)
    {
        string frq = string.Empty;
        if (num == "1") { frq = "Annually"; }
        if (num == "2") { frq = "Nine Monthly"; }
        if (num == "3") { frq = "Six Monthly"; }
        return frq;

    }
    public string getAuditorsNameById(string id)
    {
        string sqlstr = "select name  from leadauditors where id='" + id + "'";
        string retstr = "";
        DataSet ds = da.GetDataSetInline(sqlstr);

        int numrows = ds.Tables[0].Rows.Count;
        if (numrows > 0)
        {
            retstr = ds.Tables[0].Rows[0]["name"].ToString();
        }
        return retstr;
    }
    public string getclientcompNameById(string id)
    {
        string sqlstr = "select companyname  from consultant_registration where id='" + id + "'";
        string retstr = "";
        DataSet ds = da.GetDataSetInline(sqlstr);

        int numrows = ds.Tables[0].Rows.Count;
        if (numrows > 0)
        {
            retstr = ds.Tables[0].Rows[0]["companyname"].ToString();
        }
        return retstr;
    }

    public string getclientNameById(string id)
    {
        string sqlstr = "select person  from consultant_registration where id='" + id + "'";
        string retstr = "";
        DataSet ds = da.GetDataSetInline(sqlstr);

        int numrows = ds.Tables[0].Rows.Count;
        if (numrows > 0)
        {
            retstr = ds.Tables[0].Rows[0]["person"].ToString();
        }
        return retstr;
    }
    public string getclientemailById(string id)
    {
        string sqlstr = "select email  from consultant_registration where id='" + id + "'";
        string retstr = "";
        DataSet ds = da.GetDataSetInline(sqlstr);

        int numrows = ds.Tables[0].Rows.Count;
        if (numrows > 0)
        {
            retstr = ds.Tables[0].Rows[0]["email"].ToString();
        }
        return retstr;
    }
    public string getAuditorsemailById(string id)
    {
        string sqlstr = "select auditoremail  from leadauditors where id='" + id + "'";
        string retstr = "";
        DataSet ds = da.GetDataSetInline(sqlstr);

        int numrows = ds.Tables[0].Rows.Count;
        if (numrows > 0)
        {
            retstr = ds.Tables[0].Rows[0]["auditoremail"].ToString();
        }
        return retstr;
    }
    public string getEmpNameById(string id)
    {

        if (id != "0")
        {

            string sqlstr = "select (isnull(tbl_employee.f_name,'') +' '+isnull(tbl_employee.l_name,''))  as EmployeeName  from tbl_employee where id='" + id + "'";
            string retstr = "";
            DataSet ds = da.GetDataSetInline(sqlstr);

            int numrows = ds.Tables[0].Rows.Count;
            if (numrows > 0)
            {
                retstr = ds.Tables[0].Rows[0]["EmployeeName"].ToString();
            }
            return retstr;
        }
        else
        {
            return "";
        }
    }

    public string getteamleadnamebyteamid(string id)
    {

        string sqlstr = "select teamlead from empteammaster where id='" + id + "'";
        string retstr = "";
        DataSet ds = da.GetDataSetInline(sqlstr);

        int numrows = ds.Tables[0].Rows.Count;
        if (numrows > 0)
        {
            retstr = getEmpNameById(ds.Tables[0].Rows[0]["teamlead"].ToString());
        }
        return retstr;
    }

    public string getempbarefund(string EventYear, string EventMonth, string eid)
    {
        string retstr = "";
        SqlParameter[] prma = new SqlParameter[3];
        prma[0] = new SqlParameter("@EventYear", EventYear);
        prma[1] = new SqlParameter("@EventMonth", EventMonth);
        prma[2] = new SqlParameter("@eid", eid);
        DataSet dscam = da.GetDataSet("selectbarefundtds", prma);



        int numrows = dscam.Tables[0].Rows.Count;
        if (numrows > 0)
        {
            retstr = dscam.Tables[0].Rows[0]["netdebit"].ToString() + "#" + dscam.Tables[0].Rows[0]["tds"].ToString() + "#" + dscam.Tables[0].Rows[0]["refund"].ToString();
      
        }
        return retstr;
    }
    public string getempbarefundteamemp(string EventYear, string EventMonth, string eid)
    {
        string retstr = "";
        SqlParameter[] prma = new SqlParameter[3];
        prma[0] = new SqlParameter("@EventYear", EventYear);
        prma[1] = new SqlParameter("@EventMonth", EventMonth);
        prma[2] = new SqlParameter("@empid", eid);
        DataSet dscam = da.GetDataSet("selectbarefundtds_team_emp", prma);



        int numrows = dscam.Tables[0].Rows.Count;
        if (numrows > 0)
        {
            retstr = dscam.Tables[0].Rows[0]["netdebit"].ToString() + "#" + dscam.Tables[0].Rows[0]["tds"].ToString() + "#" + dscam.Tables[0].Rows[0]["refund"].ToString();
        }
        return retstr;
    }


    public string showempname(string empcode)
    {
        string strreturn = "";
        string strsql = "select ISNULL([dbo].[getusername](dbo.getuidbyempcodebyregid('" + empcode + "','"+HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString()+"')),'')+' (" + empcode + ")' as name";

        //Response.Write(strsql);
        //Response.End();
        DataSet dsempname = da.GetDataSetInline(strsql);
        if (dsempname.Tables[0].Rows.Count > 0)
        {
            strreturn = dsempname.Tables[0].Rows[0]["name"].ToString();
        }

        return strreturn;
    }
    public string showempid(string empcode)
    {
        string strreturn = "";
        string strsql = "select dbo.getuidbyempcodebyregid('" + empcode + "','" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "') as uid";

        //Response.Write(strsql);
        //Response.End();
        DataSet dsempname = da.GetDataSetInline(strsql);
        if (dsempname.Tables[0].Rows.Count > 0)
        {
            strreturn = dsempname.Tables[0].Rows[0]["uid"].ToString();
        }

        return strreturn;
    }


    public string getempbarefundteam(string EventYear, string EventMonth, string eid)
    {
        string retstr = "";
        SqlParameter[] prma = new SqlParameter[3];
        prma[0] = new SqlParameter("@EventYear", EventYear);
        prma[1] = new SqlParameter("@EventMonth", EventMonth);
        prma[2] = new SqlParameter("@empid", eid);
        DataSet dscam = da.GetDataSet("selectbarefundtds_team", prma);



        int numrows = dscam.Tables[0].Rows.Count;
        if (numrows > 0)
        {
            retstr = dscam.Tables[0].Rows[0]["netdebit"].ToString() + "#" + dscam.Tables[0].Rows[0]["tds"].ToString() + "#" + dscam.Tables[0].Rows[0]["refund"].ToString();
        }
        return retstr;
    }

    public string getempbarefundmonth(string EventYear, string EventMonth, string eid)
    {
        string retstr = "";
        SqlParameter[] prma = new SqlParameter[3];
        prma[0] = new SqlParameter("@EventYear", EventYear);
        prma[1] = new SqlParameter("@EventMonth", EventMonth);
        prma[2] = new SqlParameter("@empid", eid);
        DataSet dscam = da.GetDataSet("selectbarefundtds_month", prma);



        int numrows = dscam.Tables[0].Rows.Count;
        if (numrows > 0)
        {
            retstr = dscam.Tables[0].Rows[0]["netdebit"].ToString() + "#" + dscam.Tables[0].Rows[0]["tds"].ToString() + "#" + dscam.Tables[0].Rows[0]["refund"].ToString();
        }
        return retstr;
    }
    public string getempamount(string EventYear, string EventMonth, string eid)
    {
        string retstr = "";
        SqlParameter[] prma = new SqlParameter[3];
        prma[0] = new SqlParameter("@EventYear", EventYear);
        prma[1] = new SqlParameter("@EventMonth", EventMonth);
          prma[2] = new SqlParameter("@eid", eid);
          DataSet dscam = da.GetDataSet("selectbapaymentallclearunclear", prma);



          int numrows = dscam.Tables[0].Rows.Count;
        if (numrows > 0)
        {
            retstr = dscam.Tables[0].Rows[0]["amount"].ToString() + "#" + dscam.Tables[0].Rows[0]["servicetax"].ToString() + "#" + dscam.Tables[0].Rows[0]["tds"].ToString() + "#" + dscam.Tables[0].Rows[0]["refund"].ToString() + "#" + dscam.Tables[0].Rows[0]["targetamount"].ToString() + "#" + dscam.Tables[0].Rows[0]["expences"].ToString();
        }
        return retstr;
    }

    public string getempclearamount(string EventYear, string EventMonth, string eid)
    {
        string retstr = "";
        SqlParameter[] prma = new SqlParameter[3];
        prma[0] = new SqlParameter("@EventYear", EventYear);
        prma[1] = new SqlParameter("@EventMonth", EventMonth);
        prma[2] = new SqlParameter("@eid", eid);
        DataSet dscam = da.GetDataSet("selectbapaymentallclearunclear", prma);



        int numrows = dscam.Tables[1].Rows.Count;
        if (numrows > 0)
        {
            retstr = dscam.Tables[1].Rows[0]["amount"].ToString() + "#" + dscam.Tables[1].Rows[0]["servicetax"].ToString() + "#" + dscam.Tables[1].Rows[0]["tds"].ToString() + "#" + dscam.Tables[1].Rows[0]["refund"].ToString() + "#" + dscam.Tables[1].Rows[0]["targetamount"].ToString() + "#" + dscam.Tables[0].Rows[0]["expences"].ToString();
        }
        return retstr;
    }
    public string getempunclearamount(string EventYear, string EventMonth, string eid)
    {
        string retstr = "";
        SqlParameter[] prma = new SqlParameter[3];
        prma[0] = new SqlParameter("@EventYear", EventYear);
        prma[1] = new SqlParameter("@EventMonth", EventMonth);
        prma[2] = new SqlParameter("@eid", eid);
        DataSet dscam = da.GetDataSet("selectbapaymentallclearunclear", prma);



        int numrows = dscam.Tables[2].Rows.Count;
        if (numrows > 0)
        {
            retstr = dscam.Tables[2].Rows[0]["amount"].ToString() + "#" + dscam.Tables[2].Rows[0]["servicetax"].ToString() + "#" + dscam.Tables[2].Rows[0]["tds"].ToString() + "#" + dscam.Tables[2].Rows[0]["refund"].ToString() + "#" + dscam.Tables[2].Rows[0]["targetamount"].ToString() + "#" + dscam.Tables[0].Rows[0]["expences"].ToString();
        }
        return retstr;
    }




    public string getempamountteam(string EventYear, string EventMonth, string eid)
    {
        string retstr = "";
        SqlParameter[] prma = new SqlParameter[3];
        prma[0] = new SqlParameter("@EventYear", EventYear);
        prma[1] = new SqlParameter("@EventMonth", EventMonth);
        prma[2] = new SqlParameter("@empid", eid);
        DataSet dscam = da.GetDataSet("getachiveamountteam_service", prma);



        int numrows = dscam.Tables[0].Rows.Count;
        if (numrows > 0)
        {
            retstr = dscam.Tables[0].Rows[0]["amount"].ToString() + "#" + dscam.Tables[0].Rows[0]["servicetax"].ToString() + "#" + dscam.Tables[0].Rows[0]["tds"].ToString() + "#" + dscam.Tables[0].Rows[0]["refund"].ToString() + "#" + dscam.Tables[0].Rows[0]["targetamount"].ToString() + "#" + dscam.Tables[0].Rows[0]["expences"].ToString();
        }
        return retstr;
    }




    public string getempamountteamemp(string EventYear, string EventMonth, string eid)
    {
        string retstr = "";
        SqlParameter[] prma = new SqlParameter[3];
        prma[0] = new SqlParameter("@EventYear", EventYear);
        prma[1] = new SqlParameter("@EventMonth", EventMonth);
        prma[2] = new SqlParameter("@empid", eid);
        DataSet dscam = da.GetDataSet("getachiveamountteam_service_emp", prma);



        int numrows = dscam.Tables[0].Rows.Count;
        if (numrows > 0)
        {
            retstr = dscam.Tables[0].Rows[0]["amount"].ToString() + "#" + dscam.Tables[0].Rows[0]["servicetax"].ToString() + "#" + dscam.Tables[0].Rows[0]["tds"].ToString() + "#" + dscam.Tables[0].Rows[0]["refund"].ToString() + "#" + dscam.Tables[0].Rows[0]["targetamount"].ToString() + "#" + dscam.Tables[0].Rows[0]["expences"].ToString();
        }
        return retstr;
    }

    public string getempclearamountteamemp(string EventYear, string EventMonth, string eid)
    {
        string retstr = "";
        SqlParameter[] prma = new SqlParameter[3];
        prma[0] = new SqlParameter("@EventYear", EventYear);
        prma[1] = new SqlParameter("@EventMonth", EventMonth);
        prma[2] = new SqlParameter("@empid", eid);
        DataSet dscam = da.GetDataSet("getachiveamountteam_service_emp", prma);



        int numrows = dscam.Tables[1].Rows.Count;
        if (numrows > 0)
        {
            retstr = dscam.Tables[1].Rows[0]["amount"].ToString() + "#" + dscam.Tables[1].Rows[0]["servicetax"].ToString() + "#" + dscam.Tables[1].Rows[0]["tds"].ToString() + "#" + dscam.Tables[1].Rows[0]["refund"].ToString() + "#" + dscam.Tables[1].Rows[0]["targetamount"].ToString() + "#" + dscam.Tables[0].Rows[0]["expences"].ToString();
        }
        return retstr;
    }
    public string getempunclearamountteamemp(string EventYear, string EventMonth, string eid)
    {
        string retstr = "";
        SqlParameter[] prma = new SqlParameter[3];
        prma[0] = new SqlParameter("@EventYear", EventYear);
        prma[1] = new SqlParameter("@EventMonth", EventMonth);
        prma[2] = new SqlParameter("@empid", eid);
        DataSet dscam = da.GetDataSet("getachiveamountteam_service_emp", prma);



        int numrows = dscam.Tables[2].Rows.Count;
        if (numrows > 0)
        {
            retstr = dscam.Tables[2].Rows[0]["amount"].ToString() + "#" + dscam.Tables[2].Rows[0]["servicetax"].ToString() + "#" + dscam.Tables[2].Rows[0]["tds"].ToString() + "#" + dscam.Tables[2].Rows[0]["refund"].ToString() + "#" + dscam.Tables[2].Rows[0]["targetamount"].ToString() + "#" + dscam.Tables[0].Rows[0]["expences"].ToString();
        }
        return retstr;
    }
    public string getempclearamountteam(string EventYear, string EventMonth, string eid)
    {
        string retstr = "";
        SqlParameter[] prma = new SqlParameter[3];
        prma[0] = new SqlParameter("@EventYear", EventYear);
        prma[1] = new SqlParameter("@EventMonth", EventMonth);
        prma[2] = new SqlParameter("@empid", eid);
        DataSet dscam = da.GetDataSet("getachiveamountteam_service", prma);



        int numrows = dscam.Tables[1].Rows.Count;
        if (numrows > 0)
        {
            retstr = dscam.Tables[1].Rows[0]["amount"].ToString() + "#" + dscam.Tables[1].Rows[0]["servicetax"].ToString() + "#" + dscam.Tables[1].Rows[0]["tds"].ToString() + "#" + dscam.Tables[1].Rows[0]["refund"].ToString() + "#" + dscam.Tables[1].Rows[0]["targetamount"].ToString() + "#" + dscam.Tables[0].Rows[0]["expences"].ToString();
        }
        return retstr;
    }
    public string getempunclearamountteam(string EventYear, string EventMonth, string eid)
    {
        string retstr = "";
        SqlParameter[] prma = new SqlParameter[3];
        prma[0] = new SqlParameter("@EventYear", EventYear);
        prma[1] = new SqlParameter("@EventMonth", EventMonth);
        prma[2] = new SqlParameter("@empid", eid);
        DataSet dscam = da.GetDataSet("getachiveamountteam_service", prma);



        int numrows = dscam.Tables[2].Rows.Count;
        if (numrows > 0)
        {
            retstr = dscam.Tables[2].Rows[0]["amount"].ToString() + "#" + dscam.Tables[2].Rows[0]["servicetax"].ToString() + "#" + dscam.Tables[2].Rows[0]["tds"].ToString() + "#" + dscam.Tables[2].Rows[0]["refund"].ToString() + "#" + dscam.Tables[2].Rows[0]["targetamount"].ToString() + "#" + dscam.Tables[0].Rows[0]["expences"].ToString();
        }
        return retstr;
    }




    public string getempamountteammonth(string EventYear, string EventMonth)
    {
        string retstr = "";
        SqlParameter[] prma = new SqlParameter[3];
        prma[0] = new SqlParameter("@EventYear", EventYear);
        prma[1] = new SqlParameter("@EventMonth", EventMonth);
        prma[2] = new SqlParameter("@empid", HttpContext.Current.Session["teamid"]);
        DataSet dscam = da.GetDataSet("getachiveamountteam_service_month", prma);



        int numrows = dscam.Tables[0].Rows.Count;
        if (numrows > 0)
        {
            retstr = dscam.Tables[0].Rows[0]["amount"].ToString() + "#" + dscam.Tables[0].Rows[0]["servicetax"].ToString() + "#" + dscam.Tables[0].Rows[0]["tds"].ToString() + "#" + dscam.Tables[0].Rows[0]["refund"].ToString() + "#" + dscam.Tables[0].Rows[0]["targetamount"].ToString() + "#" + dscam.Tables[0].Rows[0]["expences"].ToString();
        }
        return retstr;
    }

    public string getempregionname(string id)
    {

        string strretrn = "";
        string strsql = "select region from employee where id='" + id + "'";

        DataSet dsemp = da.GetDataSetInline(strsql);
        if (dsemp.Tables[0].Rows.Count > 0)
        {
            strretrn = dsemp.Tables[0].Rows[0]["region"].ToString();
        }

        return strretrn;
    }


    public string getempclearamountteammonth(string EventYear, string EventMonth)
    {
        string retstr = "";
        SqlParameter[] prma = new SqlParameter[3];
        prma[0] = new SqlParameter("@EventYear", EventYear);
        prma[1] = new SqlParameter("@EventMonth", EventMonth);
        prma[2] = new SqlParameter("@empid", "0");
        DataSet dscam = da.GetDataSet("getachiveamountteam_service_month", prma);



        int numrows = dscam.Tables[1].Rows.Count;
        if (numrows > 0)
        {
            retstr = dscam.Tables[1].Rows[0]["amount"].ToString() + "#" + dscam.Tables[1].Rows[0]["servicetax"].ToString() + "#" + dscam.Tables[1].Rows[0]["tds"].ToString() + "#" + dscam.Tables[1].Rows[0]["refund"].ToString() + "#" + dscam.Tables[1].Rows[0]["targetamount"].ToString() + "#" + dscam.Tables[0].Rows[0]["expences"].ToString();
        }
        return retstr;
    }
    public string getempunclearamountteammonth(string EventYear, string EventMonth)
    {
        string retstr = "";
        SqlParameter[] prma = new SqlParameter[3];
        prma[0] = new SqlParameter("@EventYear", EventYear);
        prma[1] = new SqlParameter("@EventMonth", EventMonth);
        prma[2] = new SqlParameter("@empid", "0");
        DataSet dscam = da.GetDataSet("getachiveamountteam_service_month", prma);



        int numrows = dscam.Tables[2].Rows.Count;
        if (numrows > 0)
        {
            retstr = dscam.Tables[2].Rows[0]["amount"].ToString() + "#" + dscam.Tables[2].Rows[0]["servicetax"].ToString() + "#" + dscam.Tables[2].Rows[0]["tds"].ToString() + "#" + dscam.Tables[2].Rows[0]["refund"].ToString() + "#" + dscam.Tables[2].Rows[0]["targetamount"].ToString() + "#" + dscam.Tables[0].Rows[0]["expences"].ToString();
        }
        return retstr;
    }




    public string getEmpNameBybaId(string id)
    {

        string sqlstr = "select reportto from consultants where id='" + id + "'";
        string retstr = "";
        DataSet ds = da.GetDataSetInline(sqlstr);

        int numrows = ds.Tables[0].Rows.Count;
        if (numrows > 0)
        {
            retstr = getEmpNameById(ds.Tables[0].Rows[0]["reportto"].ToString());
        }
        return retstr;
    }
    public string getEmpemailById(string id)
    {

        string sqlstr = "select email_Id  from tbl_employee where id='" + id + "'";
        string retstr = "";
        DataSet ds = da.GetDataSetInline(sqlstr);

        int numrows = ds.Tables[0].Rows.Count;
        if (numrows > 0)
        {
            retstr = ds.Tables[0].Rows[0]["email_Id"].ToString();
        }
        return retstr;
    }
    public DataSet getEmpecoundetails(string id)
    {

        string sqlstr = "select *  from tbl_employee where id in (select councelor_id from tbl_employee where id='" + id + "')";
        //string retstr = "";
        DataSet ds = da.GetDataSetInline(sqlstr);

        //int numrows = ds.Tables[0].Rows.Count;
        //if (numrows > 0)
        //{
        //    retstr = ds.Tables[0].Rows[0]["email"].ToString();
        //}
        return ds;
    }
    public string getbamailById(string id)
    {

        string sqlstr = "select email  from consultants where id='" + id + "'";
        string retstr = "";
        DataSet ds = da.GetDataSetInline(sqlstr);

        int numrows = ds.Tables[0].Rows.Count;
        if (numrows > 0)
        {
            retstr = ds.Tables[0].Rows[0]["email"].ToString();
        }
        return retstr;
    }

    public string getbanameById(string id)
    {

        string sqlstr = "select name  from consultants where id='" + id + "'";
        string retstr = "";
        DataSet ds = da.GetDataSetInline(sqlstr);

        int numrows = ds.Tables[0].Rows.Count;
        if (numrows > 0)
        {
            retstr = ds.Tables[0].Rows[0]["name"].ToString();
        }
        return retstr;
    }
    public string getbacntidById(string id)
    {

        string sqlstr = "select cnt_id  from consultants where id='" + id + "'";
        string retstr = "";
        DataSet ds = da.GetDataSetInline(sqlstr);

        int numrows = ds.Tables[0].Rows.Count;
        if (numrows > 0)
        {
            retstr = ds.Tables[0].Rows[0]["cnt_id"].ToString();
        }
        return retstr;
    }
    public string getTechExpsNameById(string id)
    {
        string sqlstr = "select name  from techexperts where id='" + id + "'";
        string retstr = "";
        DataSet ds = da.GetDataSetInline(sqlstr);

        int numrows = ds.Tables[0].Rows.Count;
        if (numrows > 0)
        {
            retstr = ds.Tables[0].Rows[0]["name"].ToString();
        }
        return retstr;
    }

    public string getprimarySiteLocation(string cid, string rid)
    {
        string sqlstr = "select top 1 *  from consultant_site where cns_id='" + cid + "' and r_id='" + rid + "' and isselect=1 and archive=0 order by id";
        string retstr = "";
        DataSet ds = da.GetDataSetInline(sqlstr);

        int numrows = ds.Tables[0].Rows.Count;
        if (numrows > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (retstr != string.Empty)
                {
                    retstr += "," + ds.Tables[0].Rows[i]["site_addr"].ToString();
                }
                else
                {
                    retstr += ds.Tables[0].Rows[i]["site_addr"].ToString();
                }
            }


        }
        if (retstr != string.Empty)
        {
            return retstr;
        }
        else
        {
            return "No Company Yet";
        }

    }

    public string getSiteLocation(string cid, string rid)
    {
        string sqlstr = "select *  from consultant_site where r_id='" + rid + "' and isselect=1 and archive=0";
        string retstr = "";
        DataSet ds = da.GetDataSetInline(sqlstr);

        int numrows = ds.Tables[0].Rows.Count;
        if (numrows > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (retstr != string.Empty)
                {
                    retstr +="###"+ ds.Tables[0].Rows[i]["site_addr"].ToString();
                }
                else
                {
                    retstr += ds.Tables[0].Rows[i]["site_addr"].ToString();
                }
            }


        }
        if (retstr != string.Empty)
        {
            return retstr;
        }
        else
        {
            return "No Company Yet";
        }

    }


    public string getTypeTextByType(string type)
    {

        switch (type)
        {
            case "1":
                return "BA";

            case "2":
                return "AD";

            case "3":
                return "TE";


        }
        return "";
    }
    public string getLegalTypeById(string id)
    {
        string execQry = "select name from legal where id='" + id + "'";
        string retstr = "";
        DataSet ds = da.GetDataSetInline(execQry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            retstr = ds.Tables[0].Rows[0]["name"].ToString();
        }
        return retstr;
    }
    public string getRegistrationTypeById(string id)
    {
        string execQry = "select name from registration where id='" + id + "'";
        string retstr = "";
        DataSet ds = da.GetDataSetInline(execQry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            retstr = ds.Tables[0].Rows[0]["name"].ToString();
        }
        return retstr;
    }
    public string getAccdById(string id)
    {
        string sqlstr = "select name from accrediation where id='" + id + "'";
        string retstr = "";
        DataSet ds = da.GetDataSetInline(sqlstr);
        if (ds.Tables[0].Rows.Count > 0)
        {
            retstr = ds.Tables[0].Rows[0]["name"].ToString();
        }
        return retstr;
    }

    public string getStandardsNameById(string id)
    {
        string sqlstr = "select name from standards where id in( select * from [dbo].[UF_CSVToTable]('" + id + "'))";
        string retstr = "";
        DataSet ds = da.GetDataSetInline(sqlstr);
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            if (retstr != string.Empty)
            {
                retstr += "," + ds.Tables[0].Rows[i]["name"].ToString();
            }
            else
            {
                retstr += ds.Tables[0].Rows[i]["name"].ToString();
            }
        }
        return retstr;

    }

    public string getTeamtypeById(string id)
    {
        string sqlstr = "select tyname from empteammaster where id ='" + id + "'";
        string retstr = "";
        DataSet ds = da.GetDataSetInline(sqlstr);

        if (ds.Tables[0].Rows.Count > 0)
        {
            retstr = ds.Tables[0].Rows[0]["tyname"].ToString();
        }

        return retstr;
    }

    public string getTeamNameById(string id)
    {
        string sqlstr = "select tname from empteammaster where id in( select * from [dbo].[UF_CSVToTable]('" + id + "'))";
        string retstr = "";
        DataSet ds = da.GetDataSetInline(sqlstr);
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            if (retstr != string.Empty)
            {
                retstr += "," + ds.Tables[0].Rows[i]["tname"].ToString();
            }
            else
            {
                retstr += ds.Tables[0].Rows[i]["tname"].ToString();
            }
        }
        return retstr;

    }
    public string getMultiSiteStatus(int val)
    {
        if (val > 1) { return "Yes"; } else { return "No"; }

    }


    public string getTotalNumberofEmployee(string rid)
    {
        string companyemp = "0";
        string selQry = "select sum(isnull( cast(site_wrk as int),0))+(sum(isnull(cast(site_staff as int),0)))/2 as totalnoofemp from consultant_site where r_id='" + rid + "' and isselect=1 and archive=0";

       // HttpContext.Current.Response.Write(selQry);
        DataSet ds = da.GetDataSetInline(selQry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            companyemp = ds.Tables[0].Rows[0][0].ToString();
        }
        if (companyemp == string.Empty)
        {
            companyemp = "0";
        }
        return companyemp;
    }


    public string getMandaysStructure(int emp){
        string stage1 = string.Empty, stage2 = string.Empty, saudit = string.Empty, reassess = string.Empty, mandays = string.Empty;
	if(emp>=1 && emp<=5){
		stage1=".5";
		stage2="1";
		saudit="1";
		reassess="1";
		mandays="1.5";
	
	}
	if(emp>=6 && emp<=10){
		stage1="1";
		stage2="1";
		saudit="1";
		reassess="1";
		mandays="2";
	
	}
	if(emp>=11 && emp<=15){
		stage1="1";
		stage2="1.5";
		saudit="1";
		reassess="2";
		mandays="2.5";
	
	}
	if(emp>=16 && emp<=25){
		stage1="1";
		stage2="2";
		saudit="1";
		reassess="2";
		mandays="3";
	
	}
	if(emp>=26 && emp<=45){
		stage1="1";
		stage2="3";
		saudit="1.5";
		reassess="3";
		mandays="4";
	
	}if(emp>=46 && emp<=65){
		stage1="1";
		stage2="4";
		saudit="1.5";
		reassess="3";
		mandays="5";
	
	}
	if(emp>=66 && emp<=85){
		stage1="1";
		stage2="5";
		saudit="2";
		reassess="4";
		mandays="6";
	
	}
	if(emp>=86 && emp<=125){
		stage1="2";
		stage2="5";
		saudit="2";
		reassess="5";
		mandays="7";
	
	}
	if(emp>=126 && emp<=175){
		stage1="2";
		stage2="6";
		saudit="3";
		reassess="5";
		mandays="8";
	
	}
	if(emp>=176 && emp<=275){
		stage1="2";
		stage2="7";
		saudit="3";
		reassess="6";
		mandays="9";
	
	}
	if(emp>=276 && emp<=425){
		stage1="2";
		stage2="8";
		saudit="3";
		reassess="7";
		mandays="10";
	
	}
	if(emp>=426 && emp<=625){
		stage1="2";
		stage2="9";
		saudit="4";
		reassess="7";
		mandays="11";
	
	}
	if(emp>=626 && emp<=875){
		stage1="2";
		stage2="10";
		saudit="4";
		reassess="8";
		mandays="12";
	
	}
	if(emp>=876 && emp<=1175){
		stage1="2";
		stage2="11";
		saudit="4";
		reassess="9";
		mandays="13";
	
	}
	if(emp>=1176 && emp<=1550){
		stage1="2";
		stage2="12";
		saudit="5";
		reassess="9";
		mandays="14";
	
	}
	if(emp>=1551 && emp<=2025){
		stage1="2";
		stage2="13";
		saudit="5";
		reassess="10";
		mandays="15";
	
	}
	if(emp>=2026 && emp<=2675){
		stage1="2";
		stage2="14";
		saudit="5";
		reassess="11";
		mandays="16";
	
	}
	if(emp>=2676 && emp<=3450){
		stage1="2";
		stage2="15";
		saudit="6";
		reassess="11";
		mandays="17";
	
	}
	if(emp>=3451 && emp<=4350){
		stage1="2";
		stage2="16";
		saudit="6";
		reassess="12";
		mandays="18";
	
	}
	if(emp>=4351 && emp<=5450){
		stage1="2";
		stage2="17";
		saudit="6";
		reassess="13";
		mandays="19";
	
	}
	if(emp>=5451 && emp<=6800){
		stage1="2";
		stage2="18";
		saudit="7";
		reassess="13";
		mandays="20";
	
	}
	if(emp>=6801 && emp<=8500){
		stage1="2";
		stage2="19";
		saudit="7";
		reassess="14";
		mandays="21";
	
	}
	if(emp>=8501 && emp<=10700){
		stage1="2";
		stage2="20";
		saudit="7";
		reassess="15";
		mandays="22";
	
	}
	if(emp>10700){
		/*stage1=1;
		stage2=1;
		saudit=1;
		reassess=1;*/
	
	}
	
	return (stage1+"#"+stage2+"#"+saudit+"#"+reassess+"#"+mandays);

}



    public string getmdayforfoodcode(string id)
    {
        string mday = "0";
        string faddmday = "0";
        string sql = "select * from Food_codemaster where id='" + id + "'";
        DataSet dsf = da.GetDataSetInline(sql);
        if (dsf.Tables[0].Rows.Count > 0)
        {
            mday = dsf.Tables[0].Rows[0]["ftime"].ToString();
            faddmday = dsf.Tables[0].Rows[0]["faddtime"].ToString();
        }

        return mday+"#"+faddmday;
    }




    public string getohsasscore_old(string id)
    {
        string mday = "0";
        string faddmday = "0";
        int oscore = 0;

          SqlParameter[] prm = new SqlParameter[1];
          prm[0] = new SqlParameter("@r_id", id);
          DataSet   ds = da.GetDataSet("bindohsas18001_2007", prm);
            if (ds.Tables[0].Rows.Count > 0)
            {

                if (ds.Tables[0].Rows[0]["Dangerous_Goods1"].ToString() == "1")
                {
                    oscore += 5;
                }
                if (ds.Tables[0].Rows[0]["Dangerous_Goods2"].ToString() == "1")
                {
                    oscore += 10;
                }
                if (ds.Tables[0].Rows[0]["Vehicle_pedestrian1"].ToString() == "1")
                {
                    oscore += 5;
                }
                if (ds.Tables[0].Rows[0]["Vehicle_pedestrian2"].ToString() == "1")
                {
                    oscore += 10;
                }
                if (ds.Tables[0].Rows[0]["Powered_plant1"].ToString() == "1")
                {
                    oscore += 5;
                }
                if (ds.Tables[0].Rows[0]["Powered_plant2"].ToString() == "1")
                {
                    oscore += 10;
                }
                if (ds.Tables[0].Rows[0]["Other_plant1"].ToString() == "1")
                {
                    oscore += 5;
                }
                if (ds.Tables[0].Rows[0]["Other_plant2"].ToString() == "1")
                {
                    oscore += 10;
                }
                if (ds.Tables[0].Rows[0]["Manual_handling1"].ToString() == "1")
                {
                    oscore += 5;
                }
                if (ds.Tables[0].Rows[0]["Manual_handling2"].ToString() == "1")
                {
                    oscore += 10;
                }
                if (ds.Tables[0].Rows[0]["Atmospheric1"].ToString() == "1")
                {
                    oscore += 2;
                }
                if (ds.Tables[0].Rows[0]["Atmospheric2"].ToString() == "1")
                {
                    oscore += 5;
                }
                if (ds.Tables[0].Rows[0]["Use_of_ionising1"].ToString() == "1")
                {
                    oscore += 5;
                }
                if (ds.Tables[0].Rows[0]["Use_of_ionising2"].ToString() == "1")
                {
                    oscore += 10;
                }
                if (ds.Tables[0].Rows[0]["Confined_Space1"].ToString() == "1")
                {
                    oscore += 10;
                }
                if (ds.Tables[0].Rows[0]["Confined_Space2"].ToString() == "1")
                {
                    oscore += 20;
                }
                if (ds.Tables[0].Rows[0]["Slips_trips1"].ToString() == "1")
                {
                    oscore += 5;
                }

                if (ds.Tables[0].Rows[0]["Slips_trips2"].ToString() == "1")
                {
                    oscore += 20;
                }
                if (ds.Tables[0].Rows[0]["Noise1"].ToString() == "1")
                {
                    oscore += 5;
                }
                if (ds.Tables[0].Rows[0]["Noise2"].ToString() == "1")
                {
                    oscore += 15;
                }
                if (ds.Tables[0].Rows[0]["Thermal_environment1"].ToString() != string.Empty )
                {
                    try
                    {
                        oscore += Convert.ToInt32(ds.Tables[0].Rows[0]["Thermal_environment1"].ToString());
                    }
                    catch (Exception ex)
                    {

                    }
                }
                if (ds.Tables[0].Rows[0]["Below_ground1"].ToString() == "1")
                {
                    oscore += 10;
                }
                if (ds.Tables[0].Rows[0]["Below_ground2"].ToString() == "1")
                {
                    oscore += 30;
                }
                if (ds.Tables[0].Rows[0]["Storage1"].ToString() == "1")
                {
                    oscore += 5;
                }
                if (ds.Tables[0].Rows[0]["Storage2"].ToString() == "1")
                {
                    oscore += 10;
                }
                if (ds.Tables[0].Rows[0]["Electrical_hazards1"].ToString() == "1")
                {
                    oscore += 2;
                }
                if (ds.Tables[0].Rows[0]["Electrical_hazards2"].ToString() == "1")
                {
                    oscore += 5;
                }
                if (ds.Tables[0].Rows[0]["Electrical_hazards3"].ToString() == "1")
                {
                    oscore += 10;
                }
                if (ds.Tables[0].Rows[0]["Pressurised1"].ToString() == "1")
                {
                    oscore += 5;
                }
                if (ds.Tables[0].Rows[0]["Threats1"].ToString() == "1")
                {
                    oscore += 2;
                }
                if (ds.Tables[0].Rows[0]["Threats2"].ToString() == "1")
                {
                    oscore += 10;
                }
                

            }

            return oscore.ToString();
    }


    public string getohsasscore(string id)
    {
        string mday = "0";
        string faddmday = "0";
        int oscore = 0;

        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@r_id", id);
        DataSet ds = da.GetDataSet("bindohsas18001_2007", prm);
        if (ds.Tables[0].Rows.Count > 0)
        {

            if (ds.Tables[0].Rows[0]["Dangerous_Goods1"].ToString() == "1")
            {
                try
                {
                    oscore += Convert.ToInt32(ds.Tables[0].Rows[0]["txtchk1"].ToString());
                }
                catch (Exception)
                {
                }
            }
            if (ds.Tables[0].Rows[0]["Dangerous_Goods2"].ToString() == "1")
            {
                try
                {
                    oscore += Convert.ToInt32(ds.Tables[0].Rows[0]["txtchk2"].ToString());
                }
                catch (Exception)
                {
                }
            }
            if (ds.Tables[0].Rows[0]["Vehicle_pedestrian1"].ToString() == "1")
            {
                try
                {
                    oscore += Convert.ToInt32(ds.Tables[0].Rows[0]["txtchk3"].ToString());
                }
                catch (Exception)
                {
                }
            }
            if (ds.Tables[0].Rows[0]["Vehicle_pedestrian2"].ToString() == "1")
            {
                try
                {
                    oscore += Convert.ToInt32(ds.Tables[0].Rows[0]["txtchk4"].ToString());
                }
                catch (Exception)
                {
                }
            }
            if (ds.Tables[0].Rows[0]["Powered_plant1"].ToString() == "1")
            {
                try
                {
                    oscore += Convert.ToInt32(ds.Tables[0].Rows[0]["txtchk5"].ToString());
                }
                catch (Exception)
                {
                }
            }
            if (ds.Tables[0].Rows[0]["Powered_plant2"].ToString() == "1")
            {
                try
                {
                    oscore += Convert.ToInt32(ds.Tables[0].Rows[0]["txtchk6"].ToString());
                }
                catch (Exception)
                {
                }
            }
            if (ds.Tables[0].Rows[0]["Other_plant1"].ToString() == "1")
            {
                try
                {
                    oscore += Convert.ToInt32(ds.Tables[0].Rows[0]["txtchk7"].ToString());
                }
                catch (Exception)
                {
                }
            }
            if (ds.Tables[0].Rows[0]["Other_plant2"].ToString() == "1")
            {
                try
                {
                    oscore += Convert.ToInt32(ds.Tables[0].Rows[0]["txtchk8"].ToString());
                }
                catch (Exception)
                {
                }
            }
            if (ds.Tables[0].Rows[0]["Manual_handling1"].ToString() == "1")
            {
                try
                {
                    oscore += Convert.ToInt32(ds.Tables[0].Rows[0]["txtchk9"].ToString());
                }
                catch (Exception)
                {
                }
            }
            if (ds.Tables[0].Rows[0]["Manual_handling2"].ToString() == "1")
            {
                try
                {
                    oscore += Convert.ToInt32(ds.Tables[0].Rows[0]["txtchk10"].ToString());
                }
                catch (Exception)
                {
                }
            }
            if (ds.Tables[0].Rows[0]["Atmospheric1"].ToString() == "1")
            {
                try
                {
                    oscore += Convert.ToInt32(ds.Tables[0].Rows[0]["txtchk11"].ToString());
                }
                catch (Exception)
                {
                }
            }
            if (ds.Tables[0].Rows[0]["Atmospheric2"].ToString() == "1")
            {
                try
                {
                    oscore += Convert.ToInt32(ds.Tables[0].Rows[0]["txtchk12"].ToString());

                }
                catch (Exception)
                {
                }
            }
            if (ds.Tables[0].Rows[0]["Use_of_ionising1"].ToString() == "1")
            {
                try
                {
                    oscore += Convert.ToInt32(ds.Tables[0].Rows[0]["txtchk13"].ToString());
                }
                catch (Exception)
                {
                }
            }
            if (ds.Tables[0].Rows[0]["Use_of_ionising2"].ToString() == "1")
            {
                try
                {
                    oscore += Convert.ToInt32(ds.Tables[0].Rows[0]["txtchk14"].ToString());
                }
                catch (Exception)
                {
                }
            }
            if (ds.Tables[0].Rows[0]["Confined_Space1"].ToString() == "1")
            {
                try
                {
                    oscore += Convert.ToInt32(ds.Tables[0].Rows[0]["txtchk15"].ToString());
                }
                catch (Exception)
                {
                }
            }
            if (ds.Tables[0].Rows[0]["Confined_Space2"].ToString() == "1")
            {
                try
                {
                    oscore += Convert.ToInt32(ds.Tables[0].Rows[0]["txtchk16"].ToString());
                }
                catch (Exception)
                {
                }
            }
            if (ds.Tables[0].Rows[0]["Slips_trips1"].ToString() == "1")
            {
                try
                {
                    oscore += Convert.ToInt32(ds.Tables[0].Rows[0]["txtchk17"].ToString());
                }
                catch (Exception)
                {
                }
            }

            if (ds.Tables[0].Rows[0]["Slips_trips2"].ToString() == "1")
            {
                try
                {
                    oscore += Convert.ToInt32(ds.Tables[0].Rows[0]["txtchk18"].ToString());
                }
                catch (Exception)
                {
                }
            }
            if (ds.Tables[0].Rows[0]["Noise1"].ToString() == "1")
            {
                try
                {
                    oscore += Convert.ToInt32(ds.Tables[0].Rows[0]["txtchk19"].ToString());
                }
                catch (Exception)
                {
                }
            }
            if (ds.Tables[0].Rows[0]["Noise2"].ToString() == "1")
            {
                try
                {
                    oscore += Convert.ToInt32(ds.Tables[0].Rows[0]["txtchk20"].ToString());
                }
                catch (Exception)
                {
                }
            }
            if (ds.Tables[0].Rows[0]["Thermal_environment1"].ToString() != string.Empty)
            {
                try
                {
                    oscore += Convert.ToInt32(ds.Tables[0].Rows[0]["Thermal_environment1"].ToString());
                }
                catch (Exception ex)
                {

                }
            }
            if (ds.Tables[0].Rows[0]["Below_ground1"].ToString() == "1")
            {
                try
                {
                    oscore += Convert.ToInt32(ds.Tables[0].Rows[0]["txtchk21"].ToString());
                }
                catch (Exception)
                {
                }
            }
            if (ds.Tables[0].Rows[0]["Below_ground2"].ToString() == "1")
            {
                try
                {
                    oscore += Convert.ToInt32(ds.Tables[0].Rows[0]["txtchk22"].ToString());
                }
                catch (Exception)
                {
                }
            }
            if (ds.Tables[0].Rows[0]["Storage1"].ToString() == "1")
            {
                try
                {
                    oscore += Convert.ToInt32(ds.Tables[0].Rows[0]["txtchk23"].ToString());
                }
                catch (Exception)
                {
                }
            }
            if (ds.Tables[0].Rows[0]["Storage2"].ToString() == "1")
            {
                try
                {
                    oscore += Convert.ToInt32(ds.Tables[0].Rows[0]["txtchk24"].ToString());
                }
                catch (Exception)
                {
                }
            }
            if (ds.Tables[0].Rows[0]["Electrical_hazards1"].ToString() == "1")
            {
                try
                {
                    oscore += Convert.ToInt32(ds.Tables[0].Rows[0]["txtchk25"].ToString());
                }
                catch (Exception)
                {
                }
            }
            if (ds.Tables[0].Rows[0]["Electrical_hazards2"].ToString() == "1")
            {
                try
                {
                    oscore += Convert.ToInt32(ds.Tables[0].Rows[0]["txtchk26"].ToString());
                }
                catch (Exception)
                {
                }
            }
            if (ds.Tables[0].Rows[0]["Electrical_hazards3"].ToString() == "1")
            {
                try
                {
                    oscore += Convert.ToInt32(ds.Tables[0].Rows[0]["txtchk27"].ToString());
                }
                catch (Exception)
                {
                }
            }
            if (ds.Tables[0].Rows[0]["Pressurised1"].ToString() == "1")
            {
                try
                {
                    oscore += Convert.ToInt32(ds.Tables[0].Rows[0]["txtchk28"].ToString());
                }
                catch (Exception)
                {
                }
            }
            if (ds.Tables[0].Rows[0]["Threats1"].ToString() == "1")
            {
                try
                {
                    oscore += Convert.ToInt32(ds.Tables[0].Rows[0]["txtchk29"].ToString());
                }
                catch (Exception)
                {
                }
            }
            if (ds.Tables[0].Rows[0]["Threats2"].ToString() == "1")
            {
                try
                {
                    oscore += Convert.ToInt32(ds.Tables[0].Rows[0]["txtchk30"].ToString());
                }
                catch (Exception)
                {
                }
            }


        }

        return oscore.ToString();
    }

    public string getMandaysStructureforohsas(int emp,string scode)
    {
        string stage1 = string.Empty, stage2 = string.Empty, saudit = string.Empty, reassess = string.Empty, mandays = string.Empty;
        switch (scode)
        {
            case "1":


                if (emp >= 1 && emp <= 5)
                {


                    //stage1 = "1";
                    //stage2 = "2";
                    //saudit = "1";
                    //reassess = "2";
                    mandays = "2.5";

                }
                if (emp >= 6 && emp <= 10)
                {
                    //stage1 = "1";
                    //stage2 = "2.5";
                    //saudit = "1";
                    //reassess = "2";
                    mandays = "3";

                }
                if (emp >= 11 && emp <= 15)
                {
                    //stage1 = "1";
                    //stage2 = "3.5";
                    //saudit = "1.5";
                    //reassess = "3";
                    mandays = "3";

                }
                if (emp >= 16 && emp <= 25)
                {
                    //stage1 = "1";
                    //stage2 = "4.5";
                    //saudit = "2";
                    //reassess = "4";
                    mandays = "3.5";

                }
                if (emp >= 26 && emp <= 45)
                {
                    //stage1 = "2";
                    //stage2 = "5";
                    //saudit = "2";
                    //reassess = "5";
                    mandays = "4";

                } if (emp >= 46 && emp <= 65)
                {
                    //stage1 = "2";
                    //stage2 = "6";
                    //saudit = "3";
                    //reassess = "5";
                    mandays = "4.5";

                }
                if (emp >= 66 && emp <= 85)
                {
                    //stage1 = "2";
                    //stage2 = "7";
                    //saudit = "3";
                    //reassess = "6";
                    mandays = "5";

                }
                if (emp >= 86 && emp <= 125)
                {
                    //stage1 = "2";
                    //stage2 = "9";
                    //saudit = "4";
                    //reassess = "7";
                    mandays = "5.5";

                }
                if (emp >= 126 && emp <= 175)
                {
                    //stage1 = "2";
                    //stage2 = "10";
                    //saudit = "4";
                    //reassess = "8";
                    mandays = "6";

                }
                if (emp >= 176 && emp <= 275)
                {
                    //stage1 = "3";
                    //stage2 = "10";
                    //saudit = "4";
                    //reassess = "9";
                    mandays = "7";

                }
                if (emp >= 276 && emp <= 425)
                {
                    //stage1 = "3";
                    //stage2 = "12";
                    //saudit = "5";
                    //reassess = "10";
                    mandays = "8";

                }
                if (emp >= 426 && emp <= 625)
                {
                    //stage1 = "4";
                    //stage2 = "12";
                    //saudit = "5";
                    //reassess = "11";
                    mandays = "9";

                }
                if (emp >= 626 && emp <= 875)
                {
                    //stage1 = "4";
                    //stage2 = "13";
                    //saudit = "6";
                    //reassess = "11";
                    mandays = "10";

                }
                if (emp >= 876 && emp <= 1175)
                {
                    //stage1 = "5";
                    //stage2 = "14";
                    //saudit = "6";
                    //reassess = "13";
                    mandays = "11";

                }
                if (emp >= 1176 && emp <= 1550)
                {
                    //stage1 = "5";
                    //stage2 = "15";
                    //saudit = "7";
                    //reassess = "13";
                    mandays = "12";

                }
                if (emp >= 1551 && emp <= 2025)
                {
                    //stage1 = "6";
                    //stage2 = "15";
                    //saudit = "7";
                    //reassess = "14";
                    mandays = "12";

                }
                if (emp >= 2026 && emp <= 2675)
                {
                    //stage1 = "6";
                    //stage2 = "17";
                    //saudit = "8";
                    //reassess = "15";
                    mandays = "13";

                }
                if (emp >= 2676 && emp <= 3450)
                {
                    //stage1 = "7";
                    //stage2 = "18";
                    //saudit = "8";
                    //reassess = "17";
                    mandays = "14";

                }
                if (emp >= 3451 && emp <= 4350)
                {
                    //stage1 = "7";
                    //stage2 = "20";
                    //saudit = "9";
                    //reassess = "18";
                    mandays = "15";

                }
                if (emp >= 4351 && emp <= 5450)
                {
                    //stage1 = "8";
                    //stage2 = "20";
                    //saudit = "9";
                    //reassess = "19";
                    mandays = "16";

                }
                if (emp >= 5451 && emp <= 6800)
                {
                    //stage1 = "8";
                    //stage2 = "22";
                    //saudit = "10";
                    //reassess = "20";
                    mandays = "17";

                }
                if (emp >= 6801 && emp <= 8500)
                {
                    //stage1 = "9";
                    //stage2 = "23";
                    //saudit = "11";
                    //reassess = "21";
                    mandays = "19";

                }
                if (emp >= 8501 && emp <= 10700)
                {
                    //stage1 = "9";
                    //stage2 = "25";
                    //saudit = "11";
                    //reassess = "23";
                    mandays = "20";

                }
                if (emp > 10700)
                {
                    /*stage1=1;
                    stage2=1;
                    saudit=1;
                    reassess=1;*/

                }
                break;
            case "2":


                if (emp >= 1 && emp <= 5)
                {




                    //stage1 = "1";
                    //stage2 = "1.5";
                    //saudit = "1";
                    //reassess = "2";
                    mandays = "2.5";

                }
                if (emp >= 6 && emp <= 10)
                {
                    //stage1 = "1";
                    //stage2 = "2";
                    //saudit = "1";
                    //reassess = "2";
                    mandays = "3";

                }
                if (emp >= 11 && emp <= 15)
                {
                    //stage1 = "1";
                    //stage2 = "2.5";
                    //saudit = "1";
                    //reassess = "2";
                    mandays = "3.5";

                }
                if (emp >= 16 && emp <= 25)
                {
                    //stage1 = "1";
                    //stage2 = "3.5";
                    //saudit = "1.5";
                    //reassess = "3";
                    mandays = "4.5";

                }
                if (emp >= 26 && emp <= 45)
                {
                    //stage1 = "1";
                    //stage2 = "4.5";
                    //saudit = "2";
                    //reassess = "4";
                    mandays = "5.5";

                } if (emp >= 46 && emp <= 65)
                {
                    //stage1 = "1";
                    //stage2 = "5";
                    //saudit = "2";
                    //reassess = "4";
                    mandays = "6";

                }
                if (emp >= 66 && emp <= 85)
                {
                    //stage1 = "2";
                    //stage2 = "5";
                    //saudit = "2";
                    //reassess = "5";
                    mandays = "7";

                }
                if (emp >= 86 && emp <= 125)
                {
                    //stage1 = "2";
                    //stage2 = "6";
                    //saudit = "3";
                    //reassess = "5";
                    mandays = "8";

                }
                if (emp >= 126 && emp <= 175)
                {
                    //stage1 = "2";
                    //stage2 = "7";
                    //saudit = "3";
                    //reassess = "6";
                    mandays = "9";

                }
                if (emp >= 176 && emp <= 275)
                {
                    //stage1 = "2";
                    //stage2 = "8";
                    //saudit = "3";
                    //reassess = "7";
                    mandays = "10";

                }
                if (emp >= 276 && emp <= 425)
                {
                    //stage1 = "2";
                    //stage2 = "9";
                    //saudit = "4";
                    //reassess = "7";
                    mandays = "11";

                }
                if (emp >= 426 && emp <= 625)
                {
                    //stage1 = "3";
                    //stage2 = "9";
                    //saudit = "4";
                    //reassess = "8";
                    mandays = "12";

                }
                if (emp >= 626 && emp <= 875)
                {
                    //stage1 = "4";
                    //stage2 = "9";
                    //saudit = "4";
                    //reassess = "9";
                    mandays = "13";

                }
                if (emp >= 876 && emp <= 1175)
                {
                    //stage1 = "4";
                    //stage2 = "11";
                    //saudit = "5";
                    //reassess = "10";
                    mandays = "15";

                }
                if (emp >= 1176 && emp <= 1550)
                {
                    //stage1 = "5";
                    //stage2 = "11";
                    //saudit = "5";
                    //reassess = "11";
                    mandays = "16";

                }
                if (emp >= 1551 && emp <= 2025)
                {
                    //stage1 = "5";
                    //stage2 = "12";
                    //saudit = "6";
                    //reassess = "11";
                    mandays = "17";

                }
                if (emp >= 2026 && emp <= 2675)
                {
                    //stage1 = "5";
                    //stage2 = "13";
                    //saudit = "6";
                    //reassess = "12";
                    mandays = "18";

                }
                if (emp >= 2676 && emp <= 3450)
                {
                    //stage1 = "6";
                    //stage2 = "13";
                    //saudit = "6";
                    //reassess = "13";
                    mandays = "19";

                }
                if (emp >= 3451 && emp <= 4350)
                {
                    //stage1 = "6";
                    //stage2 = "14";
                    //saudit = "7";
                    //reassess = "13";
                    mandays = "20";

                }
                if (emp >= 4351 && emp <= 5450)
                {
                    //stage1 = "7";
                    //stage2 = "14";
                    //saudit = "7";
                    //reassess = "14";
                    mandays = "21";

                }
                if (emp >= 5451 && emp <= 6800)
                {
                    //stage1 = "7";
                    //stage2 = "14";
                    //saudit = "8";
                    //reassess = "15";
                    mandays = "23";

                }
                if (emp >= 6801 && emp <= 8500)
                {
                    //stage1 = "8";
                    //stage2 = "17";
                    //saudit = "8";
                    //reassess = "17";
                    mandays = "25";

                }
                if (emp >= 8501 && emp <= 10700)
                {
                    //stage1 = "9";
                    //stage2 = "18";
                    //saudit = "9";
                    //reassess = "18";
                    mandays = "27";

                }
                if (emp > 10700)
                {
                    /*stage1=1;
                    stage2=1;
                    saudit=1;
                    reassess=1;*/

                }
                break;
            case "3":


                if (emp >= 1 && emp <= 5)
                {




                    //stage1 = "1";
                    //stage2 = "1.5";
                    //saudit = "1";
                    //reassess = "2";
                    mandays = "3";

                }
                if (emp >= 6 && emp <= 10)
                {
                    //stage1 = "1";
                    //stage2 = "2";
                    //saudit = "1";
                    //reassess = "2";
                    mandays = "3.5";

                }
                if (emp >= 11 && emp <= 15)
                {
                    //stage1 = "1";
                    //stage2 = "2";
                    //saudit = "1";
                    //reassess = "2";
                    mandays = "4.5";

                }
                if (emp >= 16 && emp <= 25)
                {
                    //stage1 = "1";
                    //stage2 = "2.5";
                    //saudit = "1";
                    //reassess = "2";
                    mandays = "5.5";

                }
                if (emp >= 26 && emp <= 45)
                {
                    //stage1 = "1";
                    //stage2 = "3";
                    //saudit = "1.5";
                    //reassess = "3";
                    mandays = "7";

                } if (emp >= 46 && emp <= 65)
                {
                    //stage1 = "1";
                    //stage2 = "3.5";
                    //saudit = "1.5";
                    //reassess = "3";
                    mandays = "8";

                }
                if (emp >= 66 && emp <= 85)
                {
                    //stage1 = "1";
                    //stage2 = "4";
                    //saudit = "1.5";
                    //reassess = "3";
                    mandays = "9";

                }
                if (emp >= 86 && emp <= 125)
                {
                    //stage1 = "1";
                    //stage2 = "4.5";
                    //saudit = "2";
                    //reassess = "4";
                    mandays = "11";

                }
                if (emp >= 126 && emp <= 175)
                {
                    //stage1 = "1";
                    //stage2 = "5";
                    //saudit = "2";
                    //reassess = "4";
                    mandays = "12";

                }
                if (emp >= 176 && emp <= 275)
                {
                    //stage1 = "2";
                    //stage2 = "5";
                    //saudit = "2";
                    //reassess = "5";
                    mandays = "13";

                }
                if (emp >= 276 && emp <= 425)
                {
                    //stage1 = "2";
                    //stage2 = "6";
                    //saudit = "3";
                    //reassess = "5";
                    mandays = "15";

                }
                if (emp >= 426 && emp <= 625)
                {
                    //stage1 = "3";
                    //stage2 = "6";
                    //saudit = "3";
                    //reassess = "6";
                    mandays = "16";

                }
                if (emp >= 626 && emp <= 875)
                {
                    //stage1 = "3";
                    //stage2 = "7";
                    //saudit = "3";
                    //reassess = "7";
                    mandays = "17";

                }
                if (emp >= 876 && emp <= 1175)
                {
                    //stage1 = "3";
                    //stage2 = "8";
                    //saudit = "4";
                    //reassess = "7";
                    mandays = "19";

                }
                if (emp >= 1176 && emp <= 1550)
                {
                    //stage1 = "3";
                    //stage2 = "9";
                    //saudit = "4";
                    //reassess = "8";
                    mandays = "20";

                }
                if (emp >= 1551 && emp <= 2025)
                {
                    //stage1 = "3";
                    //stage2 = "9";
                    //saudit = "4";
                    //reassess = "8";
                    mandays = "21";

                }
                if (emp >= 2026 && emp <= 2675)
                {
                    //stage1 = "4";
                    //stage2 = "9";
                    //saudit = "4";
                    //reassess = "9";
                    mandays = "23";

                }
                if (emp >= 2676 && emp <= 3450)
                {
                    //stage1 = "4";
                    //stage2 = "10";
                    //saudit = "5";
                    //reassess = "9";
                    mandays = "25";

                }
                if (emp >= 3451 && emp <= 4350)
                {
                    //stage1 = "5";
                    //stage2 = "10";
                    //saudit = "5";
                    //reassess = "10";
                    mandays = "27";

                }
                if (emp >= 4351 && emp <= 5450)
                {
                    //stage1 = "5";
                    //stage2 = "11";
                    //saudit = "5";
                    //reassess = "11";
                    mandays = "28";

                }
                if (emp >= 5451 && emp <= 6800)
                {
                    //stage1 = "5";
                    //stage2 = "12";
                    //saudit = "6";
                    //reassess = "11";
                    mandays = "30";

                }
                if (emp >= 6801 && emp <= 8500)
                {
                    //stage1 = "5";
                    //stage2 = "14";
                    //saudit = "6";
                    //reassess = "13";
                    mandays = "32";

                }
                if (emp >= 8501 && emp <= 10700)
                {
                    //stage1 = "6";
                    //stage2 = "14";
                    //saudit = "7";
                    //reassess = "13";
                    mandays = "34";

                }
                if (emp > 10700)
                {
                    /*stage1=1;
                    stage2=1;
                    saudit=1;
                    reassess=1;*/

                }
                break;
            
        }
        return (stage1 + "#" + stage2 + "#" + saudit + "#" + reassess + "#" + mandays);

    }




     public string getMandaysFORisms(int emp)
    {
        string stage1 = string.Empty, stage2 = string.Empty, saudit = string.Empty, reassess = string.Empty, mandays = string.Empty;
        string mpland = string.Empty;

      
                

                    if (emp >= 1 && emp <= 10)
                    {

                        mandays = "5";
                        mpland = "4";
                        stage1 = "1";
                        stage2 = "3";
                        saudit = "1";
                        reassess = "3";
                       
                       

                    }
                    if (emp >= 11 && emp <= 25)
                    {
                        mandays = "7";
                        mpland = "6";
                        stage1 = "2";
                        stage2 = "4";
                        saudit = "2";
                        reassess = "4";

                    }
                    if (emp >= 26 && emp <= 45)
                    {
                        mandays = "8.5";
                        mpland = "7";
                        stage1 = "2";
                        stage2 = "5";
                        saudit = "2.5";
                        reassess = "4";

                    }
                    if (emp >= 46 && emp <= 65)
                    {
                        mandays = "10";
                        mpland = "8";
                        stage1 = "2";
                        stage2 = "6";
                        saudit = "3";
                        reassess = "5";

                    }
                    if (emp >= 66 && emp <= 85)
                    {
                        mandays = "11";
                        mpland = "9";
                        stage1 = "2.5";
                        stage2 = "6.5";
                        saudit = "3";
                        reassess = "6";

                    } if (emp >= 86 && emp <= 125)
                    {
                        mandays = "12";
                        mpland = "10";
                        stage1 = "2.5";
                        stage2 = "7.5";
                        saudit = "3.5";
                        reassess = "6.5";

                    }
                    if (emp >= 126 && emp <= 175)
                    {
                        mandays = "13";
                        mpland = "10.5";
                        stage1 = "2.5";
                        stage2 = "8";
                        saudit = "3.5";
                        reassess = "7";

                    }
                    if (emp >= 176 && emp <= 275)
                    {
                        mandays = "14";
                        mpland = "11.5";
                        stage1 = "3";
                        stage2 = "8.5";
                        saudit = "4";
                        reassess = "7.5";

                    }
                    if (emp >= 276 && emp <= 425)
                    {
                        mandays = "15";
                        mpland = "12";
                        stage1 = "3";
                        stage2 = "9";
                        saudit = "4";
                        reassess = "8";

                    }
                    if (emp >= 426 && emp <= 625)
                    {
                        mandays = "16.5";
                        mpland = "13.5";
                        stage1 = "3";
                        stage2 = "10.5";
                        saudit = "4.5";
                        reassess = "9";

                    }
                    if (emp >= 626 && emp <= 875)
                    {
                        mandays = "17.5";
                        mpland = "14";
                        stage1 = "3";
                        stage2 = "11";
                        saudit = "5";
                        reassess = "9";

                    }
                    if (emp >= 876 && emp <= 1175)
                    {
                        mandays = "18.5";
                        mpland = "15";
                        stage1 = "3";
                        stage2 = "12";
                        saudit = "5";
                        reassess = "10";

                    }
                    if (emp >= 1176 && emp <= 1550)
                    {
                        mandays = "19.5";
                        mpland = "16";
                        stage1 = "3";
                        stage2 = "13";
                        saudit = "5.5";
                        reassess = "10.5";

                    }
                    if (emp >= 1551 && emp <= 2025)
                    {
                        mandays = "21";
                        mpland = "17";
                        stage1 = "4";
                        stage2 = "13";
                        saudit = "6";
                        reassess = "11";

                    }
                    if (emp >= 2026 && emp <= 2675)
                    {
                        mandays = "22";
                        mpland = "18";
                        stage1 = "4";
                        stage2 = "14";
                        saudit = "6";
                        reassess = "12";

                    }
                    if (emp >= 2676 && emp <= 3450)
                    {
                        mandays = "23";
                        mpland = "18.5";
                        stage1 = "4";
                        stage2 = "14.5";
                        saudit = "6.5";
                        reassess = "11.5";

                    }
                    if (emp >= 3451 && emp <= 4350)
                    {
                        mandays = "24";
                        mpland = "19.5";
                        stage1 = "4";
                        stage2 = "15.4";
                        saudit = "6.5";
                        reassess = "13";

                    }
                    if (emp >= 4351 && emp <= 5450)
                    {
                        mandays = "25";
                        mpland = "20";
                        stage1 = "4";
                        stage2 = "16";
                        saudit = "7";
                        reassess = "13";

                    }
                    if (emp >= 5451 && emp <= 6800)
                    {
                        mandays = "26";
                        mpland = "21";
                        stage1 = "4";
                        stage2 = "17";
                        saudit = "7";
                        reassess = "14";

                    }
                    if (emp >= 6801 && emp <= 8500)
                    {
                        mandays = "27";
                        mpland = "22";
                        stage1 = "5";
                        stage2 = "17";
                        saudit = "7.5";
                        reassess = "14.5";

                    }
                    if (emp >= 8501 && emp <= 10700)
                    {
                        mandays = "28";
                        mpland = "22.5";
                        stage1 = "5";
                        stage2 = "17.5";
                        saudit = "7.5";
                        reassess = "15";

                    }
                   
                    if (emp > 10700)
                    {
                        /*stage1=1;
                        stage2=1;
                        saudit=1;
                        reassess=1;*/

                    }

                    return (stage1 + "#" + stage2 + "#" + saudit + "#" + reassess + "#" + mandays + "#" + mpland);


                    
        }

     public string getMandaysFORmdq(int emp)
     {
         string stage1 = string.Empty, stage2 = string.Empty, saudit = string.Empty, reassess = string.Empty, mandays = string.Empty;


         

                 if (emp >= 1 && emp <= 5)
                 {




                     stage1 = "1";
                     stage2 = "1.5";
                     saudit = "1";
                     reassess = "1.5";
                     mandays = "3.0";

                 }
                 if (emp >= 6 && emp <= 10)
                 {
                     stage1 = "1";
                     stage2 = "2.5";
                     saudit = "1";
                     reassess = "2.5";
                     mandays = "4";

                 }
                 if (emp >= 11 && emp <= 15)
                 {
                     stage1 = "1.5";
                     stage2 = "2.5";
                     saudit = "1.5";
                     reassess = "2.5";
                     mandays = "4.5";

                 }
                 if (emp >= 16 && emp <= 25)
                 {
                     stage1 = "1.5";
                     stage2 = "2.5";
                     saudit = "1.5";
                     reassess = "2.5";
                     mandays = "5";

                 }
                 if (emp >= 26 && emp <= 45)
                 {
                     stage1 = "1.5";
                     stage2 = "3.8";
                     saudit = "1.5";
                     reassess = "3.8";
                     mandays = "6";

                 } if (emp >= 46 && emp <= 65)
                 {
                     stage1 = "2";
                     stage2 = "3.6";
                     saudit = "2";
                     reassess = "3.6";
                     mandays = "7";

                 }
                 if (emp >= 66 && emp <= 85)
                 {
                     stage1 = "2";
                     stage2 = "4.4";
                     saudit = "2";
                     reassess = "4.4";
                     mandays = "8";

                 }
                 if (emp >= 86 && emp <= 125)
                 {
                     stage1 = "2.5";
                     stage2 = "5.5";
                     saudit = "2.5";
                     reassess = "5.5";
                     mandays = "10";

                 }
                 if (emp >= 126 && emp <= 175)
                 {
                     stage1 = "2.5";
                     stage2 = "6.5";
                     saudit = "2.5";
                     reassess = "6.5";
                     mandays = "11";

                 }
                 if (emp >= 176 && emp <= 275)
                 {
                     stage1 = "3";
                     stage2 = "6.6";
                     saudit = "3";
                     reassess = "6.6";
                     mandays = "12";

                 }
                 if (emp >= 276 && emp <= 425)
                 {
                     stage1 = "3";
                     stage2 = "7.4";
                     saudit = "3";
                     reassess = "7.4";
                     mandays = "13";

                 }
                 if (emp >= 426 && emp <= 625)
                 {
                     stage1 = "3.5";
                     stage2 = "7.7";
                     saudit = "3.5";
                     reassess = "7.7";
                     mandays = "14";

                 }
                 if (emp >= 626 && emp <= 875)
                 {
                     stage1 = "3.5";
                     stage2 = "8.5";
                     saudit = "3.5";
                     reassess = "8.5";
                     mandays = "15";

                 }
                 if (emp >= 876 && emp <= 1175)
                 {
                     stage1 = "4";
                     stage2 = "8.8";
                     saudit = "4";
                     reassess = "8.8";
                     mandays = "16";

                 }
                 if (emp >= 1176 && emp <= 1550)
                 {
                     stage1 = "4";
                     stage2 = "9.6";
                     saudit = "4";
                     reassess = "9.6";
                     mandays = "17";

                 }
                 if (emp >= 1551 && emp <= 2025)
                 {
                     stage1 = "4.4";
                     stage2 = "9";
                     saudit = "4";
                     reassess = "9";
                     mandays = "18";

                 }
                 if (emp >= 2026 && emp <= 2675)
                 {
                     stage1 = "5";
                     stage2 = "10.2";
                     saudit = "5";
                     reassess = "10.2";
                     mandays = "19";

                 }
                 if (emp >= 2676 && emp <= 3450)
                 {
                     stage1 = "5";
                     stage2 = "11.0";
                     saudit = "5";
                     reassess = "11";
                     mandays = "20";

                 }
                 if (emp >= 3451 && emp <= 4350)
                 {
                     stage1 = "5";
                     stage2 = "11.8";
                     saudit = "5";
                     reassess = "11.8";
                     mandays = "21";

                 }
                 if (emp >= 4351 && emp <= 5450)
                 {
                     stage1 = "5";
                     stage2 = "12.6";
                     saudit = "5";
                     reassess = "12.6";
                     mandays = "22";

                 }
                 if (emp >= 5451 && emp <= 6800)
                 {
                     stage1 = "5.5";
                     stage2 = "13";
                     saudit = "5.5";
                     reassess = "13";
                     mandays = "23";

                 }
                 if (emp >= 6801 && emp <= 8500)
                 {
                     stage1 = "5.5";
                     stage2 = "13.8";
                     saudit = "5.5";
                     reassess = "13.8";
                     mandays = "24";

                 }
                 if (emp >= 8501 && emp <= 10700)
                 {
                     stage1 = "6";
                     stage2 = "14";
                     saudit = "6";
                     reassess = "14";
                     mandays = "25";

                 }
                 if (emp > 10700)
                 {
                     /*stage1=1;
                     stage2=1;
                     saudit=1;
                     reassess=1;*/

                 }
            

        
         return (stage1 + "#" + stage2 + "#" + saudit + "#" + reassess + "#" + mandays);

     }


    public string getMandaysFOREMS(int emp,string hcode)
    {
        string stage1 = string.Empty, stage2 = string.Empty, saudit = string.Empty, reassess = string.Empty, mandays = string.Empty;


        switch (hcode)
        {
            case "3":
                

                    if (emp >= 1 && emp <= 5)
                    {


                        stage1 = "1";
                        stage2 = "2";
                        saudit = "1";
                        reassess = "2";
                        mandays = "3.0";

                    }
                    if (emp >= 6 && emp <= 10)
                    {
                        stage1 = "1";
                        stage2 = "2.5";
                        saudit = "1";
                        reassess = "2";
                        mandays = "3.5";

                    }
                    if (emp >= 11 && emp <= 15)
                    {
                        stage1 = "1";
                        stage2 = "3.5";
                        saudit = "1.5";
                        reassess = "3";
                        mandays = "4.5";

                    }
                    if (emp >= 16 && emp <= 25)
                    {
                        stage1 = "1";
                        stage2 = "4.5";
                        saudit = "2";
                        reassess = "4";
                        mandays = "5";

                    }
                    if (emp >= 26 && emp <= 45)
                    {
                        stage1 = "2";
                        stage2 = "5";
                        saudit = "2";
                        reassess = "5";
                        mandays = "7";

                    } if (emp >= 46 && emp <= 65)
                    {
                        stage1 = "2";
                        stage2 = "6";
                        saudit = "3";
                        reassess = "5";
                        mandays = "8";

                    }
                    if (emp >= 66 && emp <= 85)
                    {
                        stage1 = "2";
                        stage2 = "7";
                        saudit = "3";
                        reassess = "6";
                        mandays = "9";

                    }
                    if (emp >= 86 && emp <= 125)
                    {
                        stage1 = "2";
                        stage2 = "9";
                        saudit = "4";
                        reassess = "7";
                        mandays = "11";

                    }
                    if (emp >= 126 && emp <= 175)
                    {
                        stage1 = "2";
                        stage2 = "10";
                        saudit = "4";
                        reassess = "8";
                        mandays = "12";

                    }
                    if (emp >= 176 && emp <= 275)
                    {
                        stage1 = "3";
                        stage2 = "10";
                        saudit = "4";
                        reassess = "9";
                        mandays = "13";

                    }
                    if (emp >= 276 && emp <= 425)
                    {
                        stage1 = "3";
                        stage2 = "12";
                        saudit = "5";
                        reassess = "10";
                        mandays = "15";

                    }
                    if (emp >= 426 && emp <= 625)
                    {
                        stage1 = "4";
                        stage2 = "12";
                        saudit = "5";
                        reassess = "11";
                        mandays = "16";

                    }
                    if (emp >= 626 && emp <= 875)
                    {
                        stage1 = "4";
                        stage2 = "13";
                        saudit = "6";
                        reassess = "11";
                        mandays = "17";

                    }
                    if (emp >= 876 && emp <= 1175)
                    {
                        stage1 = "5";
                        stage2 = "14";
                        saudit = "6";
                        reassess = "13";
                        mandays = "19";

                    }
                    if (emp >= 1176 && emp <= 1550)
                    {
                        stage1 = "5";
                        stage2 = "15";
                        saudit = "7";
                        reassess = "13";
                        mandays = "20";

                    }
                    if (emp >= 1551 && emp <= 2025)
                    {
                        stage1 = "6";
                        stage2 = "15";
                        saudit = "7";
                        reassess = "14";
                        mandays = "21";

                    }
                    if (emp >= 2026 && emp <= 2675)
                    {
                        stage1 = "6";
                        stage2 = "17";
                        saudit = "8";
                        reassess = "15";
                        mandays = "23";

                    }
                    if (emp >= 2676 && emp <= 3450)
                    {
                        stage1 = "7";
                        stage2 = "18";
                        saudit = "8";
                        reassess = "17";
                        mandays = "25";

                    }
                    if (emp >= 3451 && emp <= 4350)
                    {
                        stage1 = "7";
                        stage2 = "20";
                        saudit = "9";
                        reassess = "18";
                        mandays = "27";

                    }
                    if (emp >= 4351 && emp <= 5450)
                    {
                        stage1 = "8";
                        stage2 = "20";
                        saudit = "9";
                        reassess = "19";
                        mandays = "28";

                    }
                    if (emp >= 5451 && emp <= 6800)
                    {
                        stage1 = "8";
                        stage2 = "22";
                        saudit = "10";
                        reassess = "20";
                        mandays = "30";

                    }
                    if (emp >= 6801 && emp <= 8500)
                    {
                        stage1 = "9";
                        stage2 = "23";
                        saudit = "11";
                        reassess = "21";
                        mandays = "32";

                    }
                    if (emp >= 8501 && emp <= 10700)
                    {
                        stage1 = "9";
                        stage2 = "25";
                        saudit = "11";
                        reassess = "23";
                        mandays = "34";

                    }
                    if (emp > 10700)
                    {
                        /*stage1=1;
                        stage2=1;
                        saudit=1;
                        reassess=1;*/

                    }
                    break;
            case "2":


                    if (emp >= 1 && emp <= 5)
                    {




                        stage1 = "1";
                        stage2 = "1.5";
                        saudit = "1";
                        reassess = "2";
                        mandays = "2.5";

                    }
                    if (emp >= 6 && emp <= 10)
                    {
                        stage1 = "1";
                        stage2 = "2";
                        saudit = "1";
                        reassess = "2";
                        mandays = "3";

                    }
                    if (emp >= 11 && emp <= 15)
                    {
                        stage1 = "1";
                        stage2 = "2.5";
                        saudit = "1";
                        reassess = "2";
                        mandays = "3.5";

                    }
                    if (emp >= 16 && emp <= 25)
                    {
                        stage1 = "1";
                        stage2 = "3.5";
                        saudit = "1.5";
                        reassess = "3";
                        mandays = "4.5";

                    }
                    if (emp >= 26 && emp <= 45)
                    {
                        stage1 = "1";
                        stage2 = "4.5";
                        saudit = "2";
                        reassess = "4";
                        mandays = "5.5";

                    } if (emp >= 46 && emp <= 65)
                    {
                        stage1 = "1";
                        stage2 = "5";
                        saudit = "2";
                        reassess = "4";
                        mandays = "6";

                    }
                    if (emp >= 66 && emp <= 85)
                    {
                        stage1 = "2";
                        stage2 = "5";
                        saudit = "2";
                        reassess = "5";
                        mandays = "7";

                    }
                    if (emp >= 86 && emp <= 125)
                    {
                        stage1 = "2";
                        stage2 = "6";
                        saudit = "3";
                        reassess = "5";
                        mandays = "8";

                    }
                    if (emp >= 126 && emp <= 175)
                    {
                        stage1 = "2";
                        stage2 = "7";
                        saudit = "3";
                        reassess = "6";
                        mandays = "9";

                    }
                    if (emp >= 176 && emp <= 275)
                    {
                        stage1 = "2";
                        stage2 = "8";
                        saudit = "3";
                        reassess = "7";
                        mandays = "10";

                    }
                    if (emp >= 276 && emp <= 425)
                    {
                        stage1 = "2";
                        stage2 = "9";
                        saudit = "4";
                        reassess = "7";
                        mandays = "11";

                    }
                    if (emp >= 426 && emp <= 625)
                    {
                        stage1 = "3";
                        stage2 = "9";
                        saudit = "4";
                        reassess = "8";
                        mandays = "12";

                    }
                    if (emp >= 626 && emp <= 875)
                    {
                        stage1 = "4";
                        stage2 = "9";
                        saudit = "4";
                        reassess = "9";
                        mandays = "13";

                    }
                    if (emp >= 876 && emp <= 1175)
                    {
                        stage1 = "4";
                        stage2 = "11";
                        saudit = "5";
                        reassess = "10";
                        mandays = "15";

                    }
                    if (emp >= 1176 && emp <= 1550)
                    {
                        stage1 = "5";
                        stage2 = "11";
                        saudit = "5";
                        reassess = "11";
                        mandays = "16";

                    }
                    if (emp >= 1551 && emp <= 2025)
                    {
                        stage1 = "5";
                        stage2 = "12";
                        saudit = "6";
                        reassess = "11";
                        mandays = "17";

                    }
                    if (emp >= 2026 && emp <= 2675)
                    {
                        stage1 = "5";
                        stage2 = "13";
                        saudit = "6";
                        reassess = "12";
                        mandays = "18";

                    }
                    if (emp >= 2676 && emp <= 3450)
                    {
                        stage1 = "6";
                        stage2 = "13";
                        saudit = "6";
                        reassess = "13";
                        mandays = "19";

                    }
                    if (emp >= 3451 && emp <= 4350)
                    {
                        stage1 = "6";
                        stage2 = "14";
                        saudit = "7";
                        reassess = "13";
                        mandays = "20";

                    }
                    if (emp >= 4351 && emp <= 5450)
                    {
                        stage1 = "7";
                        stage2 = "14";
                        saudit = "7";
                        reassess = "14";
                        mandays = "21";

                    }
                    if (emp >= 5451 && emp <= 6800)
                    {
                        stage1 = "7";
                        stage2 = "14";
                        saudit = "8";
                        reassess = "15";
                        mandays = "23";

                    }
                    if (emp >= 6801 && emp <= 8500)
                    {
                        stage1 = "8";
                        stage2 = "17";
                        saudit = "8";
                        reassess = "17";
                        mandays = "25";

                    }
                    if (emp >= 8501 && emp <= 10700)
                    {
                        stage1 = "9";
                        stage2 = "18";
                        saudit = "9";
                        reassess = "18";
                        mandays = "27";

                    }
                    if (emp > 10700)
                    {
                        /*stage1=1;
                        stage2=1;
                        saudit=1;
                        reassess=1;*/

                    }
                    break;
            case "1":


                    if (emp >= 1 && emp <= 5)
                    {




                        stage1 = "1";
                        stage2 = "1.5";
                        saudit = "1";
                        reassess = "2";
                        mandays = "2.5";

                    }
                    if (emp >= 6 && emp <= 10)
                    {
                        stage1 = "1";
                        stage2 = "2";
                        saudit = "1";
                        reassess = "2";
                        mandays = "3";

                    }
                    if (emp >= 11 && emp <= 15)
                    {
                        stage1 = "1";
                        stage2 = "2";
                        saudit = "1";
                        reassess = "2";
                        mandays = "3";

                    }
                    if (emp >= 16 && emp <= 25)
                    {
                        stage1 = "1";
                        stage2 = "2.5";
                        saudit = "1";
                        reassess = "2";
                        mandays = "3.5";

                    }
                    if (emp >= 26 && emp <= 45)
                    {
                        stage1 = "1";
                        stage2 = "3";
                        saudit = "1.5";
                        reassess = "3";
                        mandays = "4";

                    } if (emp >= 46 && emp <= 65)
                    {
                        stage1 = "1";
                        stage2 = "3.5";
                        saudit = "1.5";
                        reassess = "3";
                        mandays = "4.5";

                    }
                    if (emp >= 66 && emp <= 85)
                    {
                        stage1 = "1";
                        stage2 = "4";
                        saudit = "1.5";
                        reassess = "3";
                        mandays = "5";

                    }
                    if (emp >= 86 && emp <= 125)
                    {
                        stage1 = "1";
                        stage2 = "4.5";
                        saudit = "2";
                        reassess = "4";
                        mandays = "5.5";

                    }
                    if (emp >= 126 && emp <= 175)
                    {
                        stage1 = "1";
                        stage2 = "5";
                        saudit = "2";
                        reassess = "4";
                        mandays = "6";

                    }
                    if (emp >= 176 && emp <= 275)
                    {
                        stage1 = "2";
                        stage2 = "5";
                        saudit = "2";
                        reassess = "5";
                        mandays = "7";

                    }
                    if (emp >= 276 && emp <= 425)
                    {
                        stage1 = "2";
                        stage2 = "6";
                        saudit = "3";
                        reassess = "5";
                        mandays = "8";

                    }
                    if (emp >= 426 && emp <= 625)
                    {
                        stage1 = "3";
                        stage2 = "6";
                        saudit = "3";
                        reassess = "6";
                        mandays = "9";

                    }
                    if (emp >= 626 && emp <= 875)
                    {
                        stage1 = "3";
                        stage2 = "7";
                        saudit = "3";
                        reassess = "7";
                        mandays = "10";

                    }
                    if (emp >= 876 && emp <= 1175)
                    {
                        stage1 = "3";
                        stage2 = "8";
                        saudit = "4";
                        reassess = "7";
                        mandays = "11";

                    }
                    if (emp >= 1176 && emp <= 1550)
                    {
                        stage1 = "3";
                        stage2 = "9";
                        saudit = "4";
                        reassess = "8";
                        mandays = "12";

                    }
                    if (emp >= 1551 && emp <= 2025)
                    {
                        stage1 = "3";
                        stage2 = "9";
                        saudit = "4";
                        reassess = "8";
                        mandays = "12";

                    }
                    if (emp >= 2026 && emp <= 2675)
                    {
                        stage1 = "4";
                        stage2 = "9";
                        saudit = "4";
                        reassess = "9";
                        mandays = "13";

                    }
                    if (emp >= 2676 && emp <= 3450)
                    {
                        stage1 = "4";
                        stage2 = "10";
                        saudit = "5";
                        reassess = "9";
                        mandays = "14";

                    }
                    if (emp >= 3451 && emp <= 4350)
                    {
                        stage1 = "5";
                        stage2 = "10";
                        saudit = "5";
                        reassess = "10";
                        mandays = "15";

                    }
                    if (emp >= 4351 && emp <= 5450)
                    {
                        stage1 = "5";
                        stage2 = "11";
                        saudit = "5";
                        reassess = "11";
                        mandays = "16";

                    }
                    if (emp >= 5451 && emp <= 6800)
                    {
                        stage1 = "5";
                        stage2 = "12";
                        saudit = "6";
                        reassess = "11";
                        mandays = "17";

                    }
                    if (emp >= 6801 && emp <= 8500)
                    {
                        stage1 = "5";
                        stage2 = "14";
                        saudit = "6";
                        reassess = "13";
                        mandays = "19";

                    }
                    if (emp >= 8501 && emp <= 10700)
                    {
                        stage1 = "6";
                        stage2 = "14";
                        saudit = "7";
                        reassess = "13";
                        mandays = "20";

                    }
                    if (emp > 10700)
                    {
                        /*stage1=1;
                        stage2=1;
                        saudit=1;
                        reassess=1;*/

                    }
                    break;
            case "5":


                    if (emp >= 1 && emp <= 5)
                    {




                        stage1 = ".5";
                        stage2 = "1";
                        saudit = "1";
                        reassess = "2";
                        mandays = "2.5";

                    }
                    if (emp >= 6 && emp <= 10)
                    {
                        stage1 = "1";
                        stage2 = "2";
                        saudit = "1";
                        reassess = "2";
                        mandays = "3";

                    }
                    if (emp >= 11 && emp <= 15)
                    {
                        stage1 = "1";
                        stage2 = "2";
                        saudit = "1";
                        reassess = "2";
                        mandays = "3";

                    }
                    if (emp >= 16 && emp <= 25)
                    {
                        stage1 = "1";
                        stage2 = "2";
                        saudit = "1";
                        reassess = "2";
                        mandays = "3";

                    }
                    if (emp >= 26 && emp <= 45)
                    {
                        stage1 = "1";
                        stage2 = "2";
                        saudit = "1";
                        reassess = "2";
                        mandays = "3";

                    } if (emp >= 46 && emp <= 65)
                    {
                        stage1 = "1";
                        stage2 = "2.5";
                        saudit = "1";
                        reassess = "2";
                        mandays = "3.5";

                    }
                    if (emp >= 66 && emp <= 85)
                    {
                        stage1 = "1";
                        stage2 = "2.5";
                        saudit = "1";
                        reassess = "2";
                        mandays = "3.5";

                    }
                    if (emp >= 86 && emp <= 125)
                    {
                        stage1 = "1";
                        stage2 = "3";
                        saudit = "1";
                        reassess = "3";
                        mandays = "4";

                    }
                    if (emp >= 126 && emp <= 175)
                    {
                        stage1 = "1";
                        stage2 = "3.5";
                        saudit = "1.5";
                        reassess = "3";
                        mandays = "4.5";

                    }
                    if (emp >= 176 && emp <= 275)
                    {
                        stage1 = "1";
                        stage2 = "4";
                        saudit = "2";
                        reassess = "3";
                        mandays = "5";

                    }
                    if (emp >= 276 && emp <= 425)
                    {
                        stage1 = "2";
                        stage2 = "3.5";
                        saudit = "2";
                        reassess = "4";
                        mandays = "5.5";

                    }
                    if (emp >= 426 && emp <= 625)
                    {
                        stage1 = "2";
                        stage2 = "4";
                        saudit = "2";
                        reassess = "4";
                        mandays = "6";

                    }
                    if (emp >= 626 && emp <= 875)
                    {
                        stage1 = "2";
                        stage2 = "4.5";
                        saudit = "2";
                        reassess = "4";
                        mandays = "6.5";

                    }
                    if (emp >= 876 && emp <= 1175)
                    {
                        stage1 = "2";
                        stage2 = "5";
                        saudit = "2";
                        reassess = "5";
                        mandays = "7";

                    }
                    if (emp >= 1176 && emp <= 1550)
                    {
                        stage1 = "2";
                        stage2 = "5.5";
                        saudit = "2.5";
                        reassess = "5";
                        mandays = "7.5";

                    }
                    if (emp >= 1551 && emp <= 2025)
                    {
                        stage1 = "3";
                        stage2 = "5";
                        saudit = "3";
                        reassess = "5";
                        mandays = "8";

                    }
                    if (emp >= 2026 && emp <= 2675)
                    {
                        stage1 = "3";
                        stage2 = "5.5";
                        saudit = "3";
                        reassess = "6";
                        mandays = "8.5";

                    }
                    if (emp >= 2676 && emp <= 3450)
                    {
                        stage1 = "3";
                        stage2 = "6";
                        saudit = "3";
                        reassess = "6";
                        mandays = "9";

                    }
                    if (emp >= 3451 && emp <= 4350)
                    {
                        stage1 = "3";
                        stage2 = "7";
                        saudit = "3";
                        reassess = "7";
                        mandays = "10";

                    }
                    if (emp >= 4351 && emp <= 5450)
                    {
                        stage1 = "4";
                        stage2 = "7";
                        saudit = "3";
                        reassess = "7";
                        mandays = "11";

                    }
                    if (emp >= 5451 && emp <= 6800)
                    {
                        stage1 = "4";
                        stage2 = "8";
                        saudit = "3";
                        reassess = "8";
                        mandays = "12";

                    }
                    if (emp >= 6801 && emp <= 8500)
                    {
                        stage1 = "4";
                        stage2 = "9";
                        saudit = "3";
                        reassess = "9";
                        mandays = "13";

                    }
                    if (emp >= 8501 && emp <= 10700)
                    {
                        stage1 = "4";
                        stage2 = "10";
                        saudit = "4";
                        reassess = "9";
                        mandays = "14";

                    }
                    if (emp > 10700)
                    {
                        /*stage1=1;
                        stage2=1;
                        saudit=1;
                        reassess=1;*/

                    }
                    break;
            case "4":


                    if (emp >= 1 && emp <= 5)
                    {




                        stage1 = ".5";
                        stage2 = "1";
                        saudit = "1";
                        reassess = "1";
                        mandays = "3.0";

                    }
                    if (emp >= 6 && emp <= 10)
                    {
                        stage1 = "1";
                        stage2 = "1";
                        saudit = "1";
                        reassess = "1";
                        mandays = "2";

                    }
                    if (emp >= 11 && emp <= 15)
                    {
                        stage1 = "1";
                        stage2 = "1.5";
                        saudit = "1";
                        reassess = "2";
                        mandays = "2.5";

                    }
                    if (emp >= 16 && emp <= 25)
                    {
                        stage1 = "1";
                        stage2 = "2";
                        saudit = "1";
                        reassess = "2";
                        mandays = "3";

                    }
                    if (emp >= 26 && emp <= 45)
                    {
                        stage1 = "1";
                        stage2 = "3";
                        saudit = "1.5";
                        reassess = "3";
                        mandays = "4";

                    } if (emp >= 46 && emp <= 65)
                    {
                        stage1 = "1";
                        stage2 = "4";
                        saudit = "1.5";
                        reassess = "3";
                        mandays = "5";

                    }
                    if (emp >= 66 && emp <= 85)
                    {
                        stage1 = "1";
                        stage2 = "5";
                        saudit = "2";
                        reassess = "4";
                        mandays = "6";

                    }
                    if (emp >= 86 && emp <= 125)
                    {
                        stage1 = "2";
                        stage2 = "5";
                        saudit = "2";
                        reassess = "5";
                        mandays = "7";

                    }
                    if (emp >= 126 && emp <= 175)
                    {
                        stage1 = "2";
                        stage2 = "6";
                        saudit = "3";
                        reassess = "5";
                        mandays = "8";

                    }
                    if (emp >= 176 && emp <= 275)
                    {
                        stage1 = "2";
                        stage2 = "7";
                        saudit = "3";
                        reassess = "6";
                        mandays = "9";

                    }
                    if (emp >= 276 && emp <= 425)
                    {
                        stage1 = "2";
                        stage2 = "8";
                        saudit = "3";
                        reassess = "7";
                        mandays = "10";

                    }
                    if (emp >= 426 && emp <= 625)
                    {
                        stage1 = "2";
                        stage2 = "9";
                        saudit = "4";
                        reassess = "7";
                        mandays = "11";

                    }
                    if (emp >= 626 && emp <= 875)
                    {
                        stage1 = "2";
                        stage2 = "10";
                        saudit = "4";
                        reassess = "8";
                        mandays = "12";

                    }
                    if (emp >= 876 && emp <= 1175)
                    {
                        stage1 = "2";
                        stage2 = "11";
                        saudit = "4";
                        reassess = "9";
                        mandays = "13";

                    }
                    if (emp >= 1176 && emp <= 1550)
                    {
                        stage1 = "2";
                        stage2 = "12";
                        saudit = "5";
                        reassess = "9";
                        mandays = "14";

                    }
                    if (emp >= 1551 && emp <= 2025)
                    {
                        stage1 = "2";
                        stage2 = "13";
                        saudit = "5";
                        reassess = "10";
                        mandays = "15";

                    }
                    if (emp >= 2026 && emp <= 2675)
                    {
                        stage1 = "2";
                        stage2 = "14";
                        saudit = "5";
                        reassess = "11";
                        mandays = "16";

                    }
                    if (emp >= 2676 && emp <= 3450)
                    {
                        stage1 = "2";
                        stage2 = "15";
                        saudit = "6";
                        reassess = "11";
                        mandays = "17";

                    }
                    if (emp >= 3451 && emp <= 4350)
                    {
                        stage1 = "2";
                        stage2 = "16";
                        saudit = "6";
                        reassess = "12";
                        mandays = "18";

                    }
                    if (emp >= 4351 && emp <= 5450)
                    {
                        stage1 = "2";
                        stage2 = "17";
                        saudit = "6";
                        reassess = "13";
                        mandays = "19";

                    }
                    if (emp >= 5451 && emp <= 6800)
                    {
                        stage1 = "2";
                        stage2 = "18";
                        saudit = "7";
                        reassess = "13";
                        mandays = "20";

                    }
                    if (emp >= 6801 && emp <= 8500)
                    {
                        stage1 = "2";
                        stage2 = "19";
                        saudit = "7";
                        reassess = "14";
                        mandays = "21";

                    }
                    if (emp >= 8501 && emp <= 10700)
                    {
                        stage1 = "2";
                        stage2 = "20";
                        saudit = "7";
                        reassess = "15";
                        mandays = "22";

                    }
                    if (emp > 10700)
                    {
                        /*stage1=1;
                        stage2=1;
                        saudit=1;
                        reassess=1;*/

                    }
                    break;
               
        }
        return (stage1 + "#" + stage2 + "#" + saudit + "#" + reassess + "#" + mandays);

        }

    public string getMandaysStructureforfood(int emp)
    {
        string stage1 = string.Empty, stage2 = string.Empty, saudit = string.Empty, reassess = string.Empty, mandays = string.Empty;
        if (emp >= 1 && emp <= 19)
        {
            //stage1 = ".5";
            //stage2 = "1";
            //saudit = "1";
            //reassess = "1";
            mandays = "0";

        }
        if (emp >= 20 && emp <= 49)
        {
            //stage1 = "1";
            //stage2 = "1";
            //saudit = "1";
            //reassess = "1";
            mandays = "0.5";

        }
        if (emp >= 50 && emp <= 79)
        {
            //stage1 = "1";
            //stage2 = "1.5";
            //saudit = "1";
            //reassess = "2";
            mandays = "1";

        }
        if (emp >= 80 && emp <=199)
        {
            //stage1 = "1";
            //stage2 = "2";
            //saudit = "1";
            //reassess = "2";
            mandays = "1.5";

        }
        if (emp >= 200 && emp <= 499)
        {
            //stage1 = "1";
            //stage2 = "3";
            //saudit = "1.5";
            //reassess = "3";
            mandays = "2.0";

        } if (emp >= 500 && emp <= 899)
        {
            //stage1 = "1";
            //stage2 = "4";
            //saudit = "1.5";
            //reassess = "3";
            mandays = "2.5";

        }
        if (emp >= 900 && emp <= 1299)
        {
            //stage1 = "1";
            //stage2 = "5";
            //saudit = "2";
            //reassess = "4";
            mandays = "3.0";

        }
        if (emp >= 1300 && emp <= 1699)
        {
            //stage1 = "2";
            //stage2 = "5";
            //saudit = "2";
            //reassess = "5";
            mandays = "3.5";

        }
        if (emp >= 1700 && emp <= 2999)
        {
            //stage1 = "2";
            //stage2 = "6";
            //saudit = "3";
            //reassess = "5";
            mandays = "4.0";

        }
        if (emp >= 3000 && emp <= 5000)
        {
            //stage1 = "2";
            //stage2 = "7";
            //saudit = "3";
            //reassess = "6";
            mandays = "4.5";

        }
        
        if (emp > 5000)
        {
            /*stage1=1;
            stage2=1;
            saudit=1;
            reassess=1;*/
            mandays = "5.0";

        }

        return (stage1 + "#" + stage2 + "#" + saudit + "#" + reassess + "#" + mandays);

    }

    public DataSet getleadauditorbyanzsic(string anzid, string stdid, string conid)
    {

        DataSet ds = new DataSet();


        return ds;

    }
    public bool gerCrStatus(string id)
    {
        string selQry = "select count(*)  from contractreview where app_id='" + id + "'";
        DataSet ds = da.GetDataSetInline(selQry);
        int numrows = ds.Tables[0].Rows.Count;
        if (ds.Tables[0].Rows[0][0].ToString() == "0")
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool gerauselStatus(string id)
    {
        string selQry = "select count(*)  from servellienceauditerselection where app_id='" + id + "'";
        DataSet ds = da.GetDataSetInline(selQry);
        int numrows = ds.Tables[0].Rows.Count;
        if (ds.Tables[0].Rows[0][0].ToString() == "0")
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public bool gerausel2Status(string id)
    {
        string selQry = "select count(*)  from servellience2auditerselection where app_id='" + id + "'";
        DataSet ds = da.GetDataSetInline(selQry);
        int numrows = ds.Tables[0].Rows.Count;
        if (ds.Tables[0].Rows[0][0].ToString() == "0")
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public string getNumberofPendingApplication()
    {
        string str = string.Empty;
        string sqlstr = "select count(*)  from consultant_registration where status='0' and archive='0'";
       
        str = da.getexecutescalar(sqlstr);
        return str;
    }
    public string getNumberofPendingcertificatesignature()
    {
        string str = string.Empty;
        string sqlstr = "select count(*) from accountview where (isadmin='0' or isadmin is null)";
        str = da.getexecutescalar(sqlstr);
        return str;
    }


    public string getNumberofPendingaccountcertificate()
    {
        string str = string.Empty;
        string sqlstr = "select  count(*) from accountview where isadmin ='1' and (isacount <>'1' or isacount is null)";
        str = da.getexecutescalar(sqlstr);
        return str;
    }

    public string getNumberofApprovedApplication()
    {
        string str = string.Empty;
        string sqlstr = "select count(*)  from consultant_registration where status='1' and archive='0' ";
        str = da.getexecutescalar(sqlstr);
        return str;
    }

    public string getNoOfWodTy1()
    {
        string str = string.Empty;
        string sqlstr = "select count(*)  from contractreview inner join consultant_registration on consultant_registration.id=contractreview.app_id where consultant_registration.status=1 and consultant_registration.archive=0 and contractreview.status='0' and contractreview.type ='1' and consultant_registration.applicationfor='1' and contractreview.id not in (select cr_id from workorder) ";
        str = da.getexecutescalar(sqlstr);
        return str;

    }

    public string getrohsNoOfWodTy1()
    {
        string str = string.Empty;
        string sqlstr = "select count(*)  from contractreview inner join consultant_registration on consultant_registration.id=contractreview.app_id where consultant_registration.status=1 and consultant_registration.archive=0 and contractreview.status='0' and contractreview.type ='1' and consultant_registration.applicationfor='2'  and contractreview.id not in (select cr_id from workorder) ";
        str = da.getexecutescalar(sqlstr);
        return str;

    }
    public string getNoOfCustFdbkTy1()
    {
        string str = string.Empty;
        string sqlstr = "select count(*)  from workorder inner join contractreview on workorder.cr_id=contractreview.id    inner join consultant_registration on consultant_registration.id=contractreview.app_id where consultant_registration.status=1 and consultant_registration.archive=0 and applicationfor='1' and workorder.status='0' ";
        str = da.getexecutescalar(sqlstr);
        return str;

    }

    public string getrohsNoOfCustFdbkTy1()
    {
        string str = string.Empty;
        string sqlstr = "select count(*)  from workorder inner join contractreview on workorder.cr_id=contractreview.id    inner join consultant_registration on consultant_registration.id=contractreview.app_id where consultant_registration.status=1 and consultant_registration.archive=0 and applicationfor='2' and workorder.status='0' ";
        str = da.getexecutescalar(sqlstr);
        return str;

    }
    public string getNoOfAudReportTy1()
    {
        string str = string.Empty;
        string sqlstr = "select count(*)  from customerfeedback where status='0' ";
        str = da.getexecutescalar(sqlstr);
        return str;

    }


    public string getNoOfCertifiChkLstTy1()
    {
        string str = string.Empty;
        string sqlstr = "select count(*) from AuditReviewchecklist inner join workorder on AuditReviewchecklist.wid=workorder.id inner join  customerfeedback on customerfeedback.w_id=workorder.id inner join contractreview on workorder.cr_id=contractreview.id inner join consultant_registration on contractreview.app_id=consultant_registration.id  where consultant_registration.status=1 and consultant_registration.archive=0 and AuditReviewchecklist.status=0  and AuditReviewchecklist.type=1  and applicationfor='1' and AuditReviewchecklist.isadmin=1";
        str = da.getexecutescalar(sqlstr);
        return str;

    }
    public string getrohsNoOfCertifiChkLstTy1()
    {
        string str = string.Empty;
        string sqlstr = "select count(*) from AuditReviewchecklist inner join workorder on AuditReviewchecklist.wid=workorder.id inner join  customerfeedback on customerfeedback.w_id=workorder.id inner join contractreview on workorder.cr_id=contractreview.id inner join consultant_registration on contractreview.app_id=consultant_registration.id  where consultant_registration.status=1 and consultant_registration.archive=0 and AuditReviewchecklist.status=0  and AuditReviewchecklist.type=1 and applicationfor='2' and AuditReviewchecklist.isadmin=1";
        str = da.getexecutescalar(sqlstr);
        return str;

    }
    public string getNoOfFinalCertifiChkLstTy1()
    {
        string str = string.Empty;
        string sqlstr = "select count(*) from finalcertificate inner join AuditReviewchecklist on finalcertificate.chk_id=AuditReviewchecklist.id   inner join  workorder on AuditReviewchecklist.wid=workorder.id inner join contractreview on workorder.cr_id=contractreview.id inner join consultant_registration on contractreview.app_id=consultant_registration.id  where  consultant_registration.status=1  and  consultant_registration.archive=0 and finalcertificate.status=0 and applicationfor='1' and finalcertificate.type='1'  ";
        str = da.getexecutescalar(sqlstr);
        return str;

    }

    public string getrohsNoOfFinalCertifiChkLstTy1()
    {
        string str = string.Empty;
        string sqlstr = "select count(*) from finalcertificate inner join AuditReviewchecklist on finalcertificate.chk_id=AuditReviewchecklist.id   inner join  workorder on AuditReviewchecklist.wid=workorder.id inner join contractreview on workorder.cr_id=contractreview.id inner join consultant_registration on contractreview.app_id=consultant_registration.id  where  consultant_registration.status=1  and  consultant_registration.archive=0 and finalcertificate.status=0 and applicationfor='2' and finalcertificate.type='1'  ";
        str = da.getexecutescalar(sqlstr);
        return str;

    }
    public string getNoOfWodTy2()
    {
        string str = string.Empty;
        string sqlstr = "select count(*)  from contractreview inner join consultant_registration on contractreview.app_id=consultant_registration.id";
        sqlstr += "  where consultant_registration.status=1  and  consultant_registration.archive=0 and  contractreview.status='0' and contractreview.type='2'  and applicationfor='1' and contractreview.id not in (select cr_id from workorder2) ";
        str = da.getexecutescalar(sqlstr);
        return str;

    }


    public string getrohsNoOfWodTy2()
    {
        string str = string.Empty;
        string sqlstr = "select count(*)  from contractreview inner join consultant_registration on contractreview.app_id=consultant_registration.id";
        sqlstr += "  where consultant_registration.status=1  and  consultant_registration.archive=0 and  contractreview.status='0' and contractreview.type='2' and applicationfor='2' and contractreview.id not in (select cr_id from workorder2) ";
        str = da.getexecutescalar(sqlstr);
        return str;

    }

    public string getNoOfSTG1WodTy2()
    {
        string str = string.Empty;
        string sqlstr = "select count(*)  from workorder2 inner join contractreview on workorder2.cr_id=contractreview.id    inner join consultant_registration on consultant_registration.id=contractreview.app_id where consultant_registration.status=1 and consultant_registration.archive=0 and applicationfor='1' and workorder2.id not in (select wid from stage1audit where adminstatus=1)";
        str = da.getexecutescalar(sqlstr);
        return str;

    }
    public string getrohsNoOfSTG1WodTy2()
    {
        string str = string.Empty;
        string sqlstr = "select count(*)  from workorder2 inner join contractreview on workorder2.cr_id=contractreview.id    inner join consultant_registration on consultant_registration.id=contractreview.app_id where consultant_registration.status=1 and consultant_registration.archive=0 and applicationfor='2' and workorder2.id not in (select wid from stage1audit where adminstatus=1)";
        str = da.getexecutescalar(sqlstr);
        return str;

    }
    public string getNoOfSTG2WodTy2()
    {
        string str = string.Empty;
        string sqlstr = "select count(*)  from stage1audit inner join workorder2 on workorder2.id=stage1audit.wid  inner join contractreview on workorder2.cr_id=contractreview.id inner join consultant_registration on contractreview.app_id=consultant_registration.id  where  consultant_registration.status=1  and  consultant_registration.archive=0  and applicationfor='1' and stage1audit.wid not in (select wid from  stage2audit where adminstatus=1) and stage1audit.adminstatus=1  ";
        str = da.getexecutescalar(sqlstr);
        return str;

    }
    public string getrohsNoOfSTG2WodTy2()
    {
        string str = string.Empty;
        string sqlstr = "select count(*)  from stage1audit inner join workorder2 on workorder2.id=stage1audit.wid  inner join contractreview on workorder2.cr_id=contractreview.id inner join consultant_registration on contractreview.app_id=consultant_registration.id  where  consultant_registration.status=1  and  consultant_registration.archive=0 and applicationfor='2' and stage1audit.wid not in (select wid from  stage2audit where adminstatus=1) and stage1audit.adminstatus=1  ";
        str = da.getexecutescalar(sqlstr);
        return str;

    }
    public string getNoOfCustFdbkTy2()
    {
        string str = string.Empty;
        string sqlstr = "select count(*)  from stage2audit inner join workorder2 on workorder2.id=stage2audit.wid  inner join contractreview on workorder2.cr_id=contractreview.id inner join consultant_registration on contractreview.app_id=consultant_registration.id  where  consultant_registration.status=1 and applicationfor='2'  and  consultant_registration.archive=0 and stage2audit.status='0'  ";
        str = da.getexecutescalar(sqlstr);
        return str;

    }

    public string getrohsNoOfCustFdbkTy2()
    {
        string str = string.Empty;
        string sqlstr = "select count(*)  from stage2audit inner join workorder2 on workorder2.id=stage2audit.wid  inner join contractreview on workorder2.cr_id=contractreview.id inner join consultant_registration on contractreview.app_id=consultant_registration.id  where  consultant_registration.status=1 and applicationfor='2'  and  consultant_registration.archive=0 and stage2audit.status='0'  ";
        str = da.getexecutescalar(sqlstr);
        return str;

    }
    public string getNoOfAudReportTy2()
    {
        string str = string.Empty;
        string sqlstr = "select count(*) from stage2audit inner join  workorder2 on stage2audit.wid=workorder2.id inner join contractreview on workorder2.cr_id=contractreview.id inner join consultant_registration on contractreview.app_id=consultant_registration.id  where  consultant_registration.status=1  and  consultant_registration.archive=0 and stage2audit.adminstatus=1 and applicationfor='1' and  workorder2.id not in (select wid from AuditReviewchecklist where isadmin=1 and type=2)  ";
        str = da.getexecutescalar(sqlstr);
        return str;

    }

    public string getrohsNoOfAudReportTy2()
    {
        string str = string.Empty;
        string sqlstr = "select count(*) from stage2audit inner join  workorder2 on stage2audit.wid=workorder2.id inner join contractreview on workorder2.cr_id=contractreview.id inner join consultant_registration on contractreview.app_id=consultant_registration.id  where  consultant_registration.status=1  and  consultant_registration.archive=0 and stage2audit.adminstatus=1 and applicationfor='2' and  workorder2.id not in (select wid from AuditReviewchecklist where isadmin=1 and type=2)  ";
        str = da.getexecutescalar(sqlstr);
        return str;

    }


    public string getNoOfCertifiChkLstTy2()
    {
        string str = string.Empty;
        string sqlstr = "select count(*)  from AuditReviewchecklist inner join workorder2 on AuditReviewchecklist.wid=workorder2.id  inner join contractreview on workorder2.cr_id=contractreview.id inner join consultant_registration on contractreview.app_id=consultant_registration.id  where consultant_registration.status=1  and  consultant_registration.archive=0  and applicationfor='1' and AuditReviewchecklist.status=0 and AuditReviewchecklist.type=2 and AuditReviewchecklist.isadmin=1  ";
        str = da.getexecutescalar(sqlstr);
        return str;

    }

    public string getrohsNoOfCertifiChkLstTy2()
    {
        string str = string.Empty;
        string sqlstr = "select count(*)  from AuditReviewchecklist inner join workorder2 on AuditReviewchecklist.wid=workorder2.id  inner join contractreview on workorder2.cr_id=contractreview.id inner join consultant_registration on contractreview.app_id=consultant_registration.id  where consultant_registration.status=1  and  consultant_registration.archive=0 and applicationfor='2' and AuditReviewchecklist.status=0 and AuditReviewchecklist.type=2 and AuditReviewchecklist.isadmin=1  ";
        str = da.getexecutescalar(sqlstr);
        return str;

    }

    public string getNoOfFinalCertifiChkLstTy2()
    {
        string str = string.Empty;
        string sqlstr = "select count(*)  from finalcertificate inner join AuditReviewchecklist on finalcertificate.chk_id=AuditReviewchecklist.id   inner join  workorder2 on AuditReviewchecklist.wid=workorder2.id inner join contractreview on workorder2.cr_id=contractreview.id inner join consultant_registration on contractreview.app_id=consultant_registration.id  where  consultant_registration.status=1  and  consultant_registration.archive=0 and finalcertificate.status=0  and applicationfor='1' and finalcertificate.type='2'  ";
        str = da.getexecutescalar(sqlstr);
        return str;

    }

    public string getrohsNoOfFinalCertifiChkLstTy2()
    {
        string str = string.Empty;
        string sqlstr = "select count(*)  from finalcertificate inner join AuditReviewchecklist on finalcertificate.chk_id=AuditReviewchecklist.id   inner join  workorder2 on AuditReviewchecklist.wid=workorder2.id inner join contractreview on workorder2.cr_id=contractreview.id inner join consultant_registration on contractreview.app_id=consultant_registration.id  where  consultant_registration.status=1  and  consultant_registration.archive=0 and finalcertificate.status=0 and applicationfor='2' and finalcertificate.type='2'  ";
        str = da.getexecutescalar(sqlstr);
        return str;

    }
    public string getAppIdByCrId(string crid){
         string str = string.Empty;
        string sqlstr = "select count(*)  from contractreview where id='"+crid+"'";
        str = da.getexecutescalar(sqlstr);
        return str;


}
}
