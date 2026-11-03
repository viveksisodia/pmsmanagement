using System;
using System.Data;
using System.Data.SqlClient;
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

/// <summary>
/// Summary description for coaddress
/// </summary>
public class coaddress
{
	public coaddress()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    DataAccess da;
    SqlParameter[] prm;

    #region Address Data member
    private string _address = string.Empty;
    private DataSet _ds;
    #endregion

    #region Address Properties
    public string address
    {
        get { return _address; }
        set { _address = value; }
    }
    public DataSet ds
    {
        get { return _ds; }
        set { _ds = value; }
    }
    #endregion

    #region Address Function
    public void invokeinfo()
    {
        da = new DataAccess();
        ds = da.GetDataSet("abc_addressinvokeinfo");

    }
    public void updateinfo()
    {
        da = new DataAccess();
        prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@address", _address);
        da.executeDMLQuery("abc_updateaddress",prm);

    }
    #endregion
}
