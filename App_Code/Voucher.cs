using CommonClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Voucher
/// </summary>
public class Voucher
{
    DataAccess da = new DataAccess();
    public Voucher()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public string getentrycode(string branchid,string regid,string type)
    {
        string id = string.Empty;
        try
        {
            SqlParameter[] prm = new SqlParameter[3];
            prm[0] = new SqlParameter("@branchid", branchid);
            prm[1] = new SqlParameter("@regid", regid);
            prm[2] = new SqlParameter("@type", type);
            DataSet ds = da.GetDataSet("Set_VoucherCode", prm);

            if (ds.Tables[0].Rows.Count > 0)
            {
                id = ds.Tables[0].Rows[0][0].ToString();
            }
            else
            {
                id = "";
            }
        }
        catch 
        {

            
        }
        
        return id;
    }
}