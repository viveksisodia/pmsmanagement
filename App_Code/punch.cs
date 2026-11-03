using System;
using System.Data;
using System.Configuration;
using System.Linq;
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
/// Summary description for punch
/// </summary>
public class punch
{
    DataAccess da = new DataAccess();
	public punch()
	{
      

	}

    public string getpunchcode(string uid, string type)
    {
        string retstr="0";
        SqlParameter[] prm=new SqlParameter[2];
        prm[0]=new SqlParameter("@id",uid);
        prm[1]=new SqlParameter("@type",type);
        DataSet ds = da.GetDataSet("getpunchcode", prm);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["punchcode"].ToString() != string.Empty)
            {
                retstr = ds.Tables[0].Rows[0]["punchcode"].ToString();
            }
        }
        return retstr;
    }


}
