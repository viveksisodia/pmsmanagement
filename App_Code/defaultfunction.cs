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
/// Summary description for defaultfunction
/// </summary>
public class defaultfunction
{
    ListItem li;
    DataAccess da = new DataAccess();
    CommonFunction cmf = new CommonFunction();
	public defaultfunction()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public string getcountryname(string id)
    {
        
        string apptext = "";
        string strsql = "select * from imt_country where conid='" + id + "'";
        DataSet dsappid = da.GetDataSetInline(strsql);
        if (dsappid.Tables[0].Rows.Count > 0)
        {

            apptext = dsappid.Tables[0].Rows[0]["country"].ToString();
            
        }


        return apptext;
    }


    public string getempname(string id)
    {

        string apptext = "";
        string strsql = "select * from tbl_employee where id='" + id + "'";
        DataSet dsappid = da.GetDataSetInline(strsql);
        if (dsappid.Tables[0].Rows.Count > 0)
        {

            apptext = dsappid.Tables[0].Rows[0]["DisplayName"].ToString();

        }


        return apptext;
    }

    public string getgendername(string id)
    {

        string apptext = "Not Available";
        string strsql = "select * from tbl_gender where id='" + id + "'";
        DataSet dsappid = da.GetDataSetInline(strsql);
        if (dsappid.Tables[0].Rows.Count > 0)
        {

            apptext = dsappid.Tables[0].Rows[0]["gname"].ToString();

        }


        return apptext;
    }
    public string getmaritalname(string id)
    {

        string apptext = "Not Available";
        string strsql = "select * from tbl_maritalstatus where id='" + id + "'";
        DataSet dsappid = da.GetDataSetInline(strsql);
        if (dsappid.Tables[0].Rows.Count > 0)
        {

            apptext = dsappid.Tables[0].Rows[0]["mstatusname"].ToString();

        }


        return apptext;
    }
    public string getbloodname(string id)
    {

        string apptext = "Not Available";
        string strsql = "select * from tbl_bloodmaster where id='" + id + "'";
        DataSet dsappid = da.GetDataSetInline(strsql);
        if (dsappid.Tables[0].Rows.Count > 0)
        {

            apptext = dsappid.Tables[0].Rows[0]["bloodtype"].ToString();

        }


        return apptext;
    }
    public string getworktypename(string id)
    {

        string apptext = "Not Available";
        string strsql = "select * from tbl_workertype where id='" + id + "'";
        DataSet dsappid = da.GetDataSetInline(strsql);
        if (dsappid.Tables[0].Rows.Count > 0)
        {

            apptext = dsappid.Tables[0].Rows[0]["name"].ToString();

        }


        return apptext;
    }
    public string getTimeTypeename(string id)
    {

        string apptext = "Not Available";
        string strsql = "select * from tbl_timetype where id='" + id + "'";
        DataSet dsappid = da.GetDataSetInline(strsql);
        if (dsappid.Tables[0].Rows.Count > 0)
        {

            apptext = dsappid.Tables[0].Rows[0]["name"].ToString();

        }


        return apptext;
    }
    public string getshifttypename(string id)
    {

        string apptext = "Not Available";
        string strsql = "select *,dbo.hrms_GetHours(starttime,endtime)as difftime from tbl_shift where id='" + id + "'";

        DataSet dsappid = da.GetDataSetInline(strsql);
        if (dsappid.Tables[0].Rows.Count > 0)
        {

            apptext = dsappid.Tables[0].Rows[0]["shiftname"].ToString();

        }


        return apptext;
    }
    public string getrelationname(string id, string rtype)
    {

        string apptext = "Not Available";
        string strsql = "select * from tbl_employeerelation where empid='" + id + "' and relationid='"+ rtype + "'";
        DataSet dsappid = da.GetDataSetInline(strsql);
        if (dsappid.Tables[0].Rows.Count > 0)
        {

            apptext = dsappid.Tables[0].Rows[0]["firstname"].ToString()+" "+ dsappid.Tables[0].Rows[0]["lastname"].ToString();

        }


        return apptext;
    }
    public string getNotSpecified(string value)
    {

        string apptext = "Not Specified";
        if (value.Trim() != string.Empty)
        {
            apptext = value.Trim();
        }


        return apptext;
    }


    public string getgBloodGroupname(string id)
    {

        string apptext = "Not Available";
        string strsql = "select * from tbl_bloodmaster where id='" + id + "'";
        DataSet dsappid = da.GetDataSetInline(strsql);
        if (dsappid.Tables[0].Rows.Count > 0)
        {

            apptext = dsappid.Tables[0].Rows[0]["bloodtype"].ToString();

        }


        return apptext;
    }
    public string getNotAvailable(string value)
    {

        string apptext = "Not Available";
        if (value.Trim() != string.Empty)
        {
            apptext = value.Trim();
        }


        return apptext;
    }


    public string showappliesshift(string val1)
    {
        string strret = "select count(id) as totalemp from tbl_employee where ShiftType='" + val1 + "' and tbl_employee.status=1 and tbl_employee.archive=0    and (isnull(tbl_employee.isexit,0)=0 or (isnull(tbl_employee.isexit,0)=1 and tbl_employee.isexitdate>GETDATE()))";

        DataSet dscount = da.GetDataSetInline(strret);
        if (dscount.Tables[0].Rows.Count > 0)
        {
            strret = dscount.Tables[0].Rows[0]["totalemp"].ToString();
        }
        return strret;
    }
}