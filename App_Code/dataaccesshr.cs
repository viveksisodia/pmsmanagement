using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
/// <summary>
/// Summary description for dataaccesshr
/// </summary>
public class dataaccesshr
{

    SqlConnection mycon;
    SqlCommand mycmd;
    SqlDataAdapter myda;
    DataSet myds;
    int retvalue;
    SqlDataReader mydr;
    SqlParameter[] param;

    public string getcdatetimenew()
    {
        OpenConnection();
        string retstring = "";
        string qstr = "select getdate()";
        mycmd = new SqlCommand(qstr, mycon);

        retstring = mycmd.ExecuteScalar().ToString();
        CloseConnection();


        return retstring;
    }

    public dataaccesshr()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    // This function is used for Open the Connection
    public void OpenConnection()
    {
        if (mycon == null)
        {
            mycon = new SqlConnection(ConfigurationManager.AppSettings["connectionStringhr"]);
        }
        if (mycon.State == ConnectionState.Closed)
        {
            mycon.Open();
        }

    }
    /************************************************************************************************/

    // This function is used for Close the connection

    public void CloseConnection()
    {
        if (mycon != null)
        {
            if (mycon.State == ConnectionState.Open)
            {
                mycon.Close();

            }
        }
    }
    /***************************************************************************************************/
    public bool getidforadmin(string pagetitle)
    {

        if (HttpContext.Current.Request.Cookies["user"] != null)
        {
            if (HttpContext.Current.Request.Cookies["user"]["user_id"].ToString() == "1")
            {
                return true;
            }
            else
            {

                string retstr = "";
                //OpenConnection();
                param = new SqlParameter[1];
                param[0] = new SqlParameter("@page", pagetitle);
                myds = GetDataSet("IFCI_sp_fetch_pageadminid", param);
                string userid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();


                if (myds.Tables[0].Rows.Count > 0)
                {
                    string[] useridarr = myds.Tables[0].Rows[0]["usr_id"].ToString().Split(',');
                    for (int i = 0; i < useridarr.Length; i++)
                    {
                        if (userid == useridarr[i].ToString())
                        {
                            return true;
                        }

                    }
                    return false;
                }
                else
                {
                    return false;
                }
                //CloseConnection();
            }

        }
        else
        {
            return false;
        }
    }

    //This function is used for executing any select query by Passing two parameter(procedure name and SqlParameter array)
    // and returning a dataset which is used  for binding the asp control..

    public DataSet GetDataSet(string procname, SqlParameter[] sqlparam)
    {
        myds = new DataSet();
        OpenConnection();
        mycmd = new SqlCommand(procname, mycon);
        mycmd.CommandType = CommandType.StoredProcedure;
        mycmd.Connection = mycon;
        mycmd.CommandText = procname;
        mycmd.Parameters.AddRange(sqlparam);
        myda = new SqlDataAdapter(mycmd);
        myda.Fill(myds);
        CloseConnection();
        return myds;
    }

    public DataSet GetDataSetInline(string qstring)
    {
        myds = new DataSet();
        OpenConnection();
        mycmd = new SqlCommand(qstring, mycon);
        myda = new SqlDataAdapter(mycmd);
        myda.Fill(myds);
        CloseConnection();
        return myds;
    }



    /**********************************************************************************/

    //This function is used for executing any select query by passing one parameter(procedure name) 
    //and returning a dataset which is used  for binding the asp control..

    public DataSet GetDataSet(string procname)
    {
        myds = new DataSet();
        OpenConnection();
        mycmd = new SqlCommand(procname, mycon);
        mycmd.CommandType = CommandType.StoredProcedure;
        mycmd.Connection = mycon;
        mycmd.CommandText = procname;
        myda = new SqlDataAdapter(mycmd);
        myda.Fill(myds);
        CloseConnection();
        return myds;
    }

    /**********************************************************************************/

    //In this function we will pass sqlParameters array from business logic and 
    //In this way it would be used reusable and any parameters and any value we can pass
    //in this function
    //This function is also used for any DMLQuery(Insert,Update,Delete)

    public int executeDMLQuery(string procname, SqlParameter[] sqlparam)
    {
        OpenConnection();
        mycmd = new SqlCommand(procname, mycon);
        mycmd.CommandType = CommandType.StoredProcedure;
        mycmd.Connection = mycon;
        mycmd.CommandText = procname;
        mycmd.Parameters.AddRange(sqlparam);
        //HttpContext.Current.Response.Write("Ya i m here ");    
        retvalue = mycmd.ExecuteNonQuery();
        // here we are checking that return parameter is passed or not 
        // if passed then return the returned value otherwise return affected rows 
        if (mycmd.Parameters.Contains("ReturnValue"))
        {
            retvalue = Convert.ToInt32(mycmd.Parameters["ReturnValue"].Value);
            //HttpContext.Current.Response.Write("ya value is find");    
        }
        mycmd.Cancel();
        CloseConnection();
        return retvalue;
    }

    public void executeDMLQueryinline(string strq)
    {
        OpenConnection();
        mycmd = new SqlCommand(strq, mycon);
        mycmd.ExecuteNonQuery();
        CloseConnection();

    }

    public void createTable(string tableQuery)
    {
        OpenConnection();
        mycmd = new SqlCommand(tableQuery, mycon);
        mycmd.ExecuteNonQuery();
        CloseConnection();
    }

    /*********************************************************************************************/


    //In this function we will pass sqlParameters array from business logic and 
    //In this way it would be used reusable and any parameters and any value we can pass in this function
    //This function is also used for any Select Query and return the data table and
    //which can be used for bind the data control

    public DataTable getDataTable(string procname, SqlParameter[] sqlparam)
    {
        DataTable mydt = new DataTable();
        OpenConnection();
        mycmd = new SqlCommand(procname, mycon);
        mycmd.CommandType = CommandType.StoredProcedure;
        mycmd.Connection = mycon;
        mycmd.CommandText = procname;
        mycmd.Parameters.AddRange(sqlparam);
        myda = new SqlDataAdapter(mycmd);
        myda.Fill(mydt);
        CloseConnection();
        return mydt;
    }
    /*****************************************************************************************************/


    //This function is used for executing any select query and returning a data reader which can be used 
    //for binding the data control and closed the data reader on that page where it is used..

    public SqlDataReader GetDataReader(string procname, SqlParameter[] sqlparam)
    {
        OpenConnection();
        mycmd = new SqlCommand(procname, mycon);
        mycmd.CommandType = CommandType.StoredProcedure;
        mycmd.Connection = mycon;
        mycmd.CommandText = procname;
        mycmd.Parameters.AddRange(sqlparam);
        mydr = mycmd.ExecuteReader(CommandBehavior.CloseConnection);
        return mydr;
    }
    public SqlDataReader GetDataReader(string procname)
    {
        OpenConnection();
        mycmd = new SqlCommand(procname, mycon);
        mycmd.CommandType = CommandType.StoredProcedure;
        mycmd.Connection = mycon;
        mycmd.CommandText = procname;
        mydr = mycmd.ExecuteReader(CommandBehavior.CloseConnection);
        return mydr;
    }

    /*********************** this is used to execute inline query***********************/
    public SqlDataReader getDataReaderInline(string qstr)
    {
        OpenConnection();
        mycmd = new SqlCommand(qstr, mycon);
        mydr = mycmd.ExecuteReader(CommandBehavior.CloseConnection);
        return mydr;

    }
    /********************************************************************************************/

    //This function is used for check existence of record in the database 
    //and return value true if record exist otherwise return false..
    public bool isExist(string procname)
    {
        int intResult;
        OpenConnection();
        mycmd = new SqlCommand(procname, mycon);
        mycmd.CommandType = CommandType.StoredProcedure;
        mycmd.Connection = mycon;
        mycmd.CommandText = procname;
        intResult = (int)mycmd.ExecuteScalar();
        CloseConnection();
        if (intResult > 0)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    public string getcdate()
    {
        OpenConnection();
        string retstring = "";
        string qstr = "select getdate()";
        mycmd = new SqlCommand(qstr, mycon);

        retstring = mycmd.ExecuteScalar().ToString();
        CloseConnection();
        string[] arr = retstring.Split(' ');
        retstring = arr[0];
        return retstring;
    }
    public string getcdatetime()
    {
        OpenConnection();
        string retstring = "";
        string qstr = "select getdate()";
        mycmd = new SqlCommand(qstr, mycon);

        retstring = mycmd.ExecuteScalar().ToString();
        CloseConnection();


        return retstring;
    }

    public string getexecutescalar(string qstr)
    {
        OpenConnection();
        string retstring = "";

        SqlCommand mycmd = new SqlCommand(qstr, mycon);

        retstring = mycmd.ExecuteScalar().ToString();
        CloseConnection();
        return retstring;
    }

    ///********************************function For email id****************************************/
    public string getemailid(string pagetitle)
    {
        string retstr = "";
        //OpenConnection();
        param = new SqlParameter[1];
        param[0] = new SqlParameter("@pagetitle", pagetitle);
        myds = GetDataSet("IFCI_sp_fetch_pageemail", param);
        // mycmd = new SqlCommand("IFCI_sp_fetch_pageemail", mycon);
        //mycmd.CommandType = CommandType.StoredProcedure;
        //mycmd.Connection = mycon;
        // mycmd.CommandText = "IFCI_sp_fetch_pageemail";
        // mycmd.Parameters.AddRange(param);
        // myda = new SqlDataAdapter(mycmd);
        // myda.Fill(myds);
        if (myds.Tables[0].Rows.Count > 0)
        {
            retstr = myds.Tables[0].Rows[0]["email_id"].ToString();
        }
        else
        {
            retstr = "vivek@netcommlabs.com";
        }
        //CloseConnection();
        return retstr;


    }

    public string getuseremail()
    {
        string retstr = "";
        //OpenConnection();
        string str = "select dbo.getusermail(" + Convert.ToInt32(HttpContext.Current.Request.Cookies["user"]["user_id"].ToString()) + ") as email_id";
        myds = GetDataSetInline(str);

        // mycmd = new SqlCommand("IFCI_sp_fetch_pageemail", mycon);
        //mycmd.CommandType = CommandType.StoredProcedure;
        //mycmd.Connection = mycon;
        // mycmd.CommandText = "IFCI_sp_fetch_pageemail";
        // mycmd.Parameters.AddRange(param);
        // myda = new SqlDataAdapter(mycmd);
        // myda.Fill(myds);
        if (myds.Tables[0].Rows.Count > 0)
        {
            retstr = myds.Tables[0].Rows[0]["email_id"].ToString();
        }
        else
        {
            retstr = "vivek@netcommlabs.com";
        }
        //CloseConnection();
        return retstr;


    }

    public string getuseremailrofgrant(string uid)
    {
        string retstr = "";
        //OpenConnection();
        //HttpContext.Current.Response.Write(uid);
        string str = "select dbo.getusermail('" + uid + "') as email_id";
        myds = GetDataSetInline(str);

        // mycmd = new SqlCommand("IFCI_sp_fetch_pageemail", mycon);
        //mycmd.CommandType = CommandType.StoredProcedure;
        //mycmd.Connection = mycon;
        // mycmd.CommandText = "IFCI_sp_fetch_pageemail";
        // mycmd.Parameters.AddRange(param);
        // myda = new SqlDataAdapter(mycmd);
        // myda.Fill(myds);
        if (myds.Tables[0].Rows.Count > 0)
        {
            retstr = myds.Tables[0].Rows[0]["email_id"].ToString();
        }
        else
        {
            retstr = "vivek@netcommlabs.com";
        }
        //CloseConnection();
        return retstr;


    }


    public string getusermanageremail()
    {
        string retstr = "";
        //OpenConnection();
        string str = "select dbo.getusermanagermail(" + Convert.ToInt32(HttpContext.Current.Request.Cookies["user"]["user_id"].ToString()) + ") as email_id";
        myds = GetDataSetInline(str);

        // mycmd = new SqlCommand("IFCI_sp_fetch_pageemail", mycon);
        //mycmd.CommandType = CommandType.StoredProcedure;
        //mycmd.Connection = mycon;
        // mycmd.CommandText = "IFCI_sp_fetch_pageemail";
        // mycmd.Parameters.AddRange(param);
        // myda = new SqlDataAdapter(mycmd);
        // myda.Fill(myds);
        if (myds.Tables[0].Rows.Count > 0)
        {
            retstr = myds.Tables[0].Rows[0]["email_id"].ToString();
        }
        else
        {
            retstr = "vivek@netcommlabs.com";
        }
        //CloseConnection();
        return retstr;


    }

    public string getusername(int uid)
    {
        string retstr = "";
        //OpenConnection();
        string str = "select dbo.getusername(" + Convert.ToInt32(uid) + ") as name";
        myds = GetDataSetInline(str);

        // mycmd = new SqlCommand("IFCI_sp_fetch_pageemail", mycon);
        //mycmd.CommandType = CommandType.StoredProcedure;
        //mycmd.Connection = mycon;
        // mycmd.CommandText = "IFCI_sp_fetch_pageemail";
        // mycmd.Parameters.AddRange(param);
        // myda = new SqlDataAdapter(mycmd);
        // myda.Fill(myds);
        if (myds.Tables[0].Rows.Count > 0)
        {
            retstr = myds.Tables[0].Rows[0]["name"].ToString();
        }
        else
        {
            retstr = "";
        }
        //CloseConnection();
        return retstr;


    }


    public string getadmin_id()
    {
        string retstr = "";
        //OpenConnection();
        string str = "select dbo.get_adminid(" + Convert.ToInt32(HttpContext.Current.Request.Cookies["user"]["user_id"].ToString()) + ") as admin_id";
        myds = GetDataSetInline(str);
        if (myds.Tables[0].Rows.Count > 0)
        {
            retstr = myds.Tables[0].Rows[0]["admin_id"].ToString();
        }
        else
        {
            retstr = "";
        }
        //CloseConnection();
        return retstr;

    }

    public string getfun_id(string uide)
    {
        string retstr = "";
        //OpenConnection();
        SqlParameter[] prm1 = new SqlParameter[1];
        prm1[0] = new SqlParameter("@empid", uide);
        myds = GetDataSet("IFCI_sp_getfunhead", prm1);
        if (myds.Tables[0].Rows.Count > 0)
        {
            retstr = myds.Tables[0].Rows[0]["headid"].ToString();
        }
        else
        {
            retstr = "";
        }
        //CloseConnection();
        return retstr;

    }


    public string getadmin_id(string uide)
    {
        string retstr = "";
        //OpenConnection();
        string str = "select dbo.get_adminid(" + Convert.ToInt32(uide) + ") as admin_id";
        myds = GetDataSetInline(str);
        if (myds.Tables[0].Rows.Count > 0)
        {
            retstr = myds.Tables[0].Rows[0]["admin_id"].ToString();
        }
        else
        {
            retstr = "";
        }
        //CloseConnection();
        return retstr;

    }

    public string gethod()
    {
        string retstr = "";
        //OpenConnection();
        string str = "select dbo.get_councelor(" + Convert.ToInt32(HttpContext.Current.Request.Cookies["user"]["user_id"].ToString()) + ") as counc_id";
        myds = GetDataSetInline(str);
        if (myds.Tables[0].Rows.Count > 0)
        {
            retstr = myds.Tables[0].Rows[0]["counc_id"].ToString();
        }
        else
        {
            retstr = "";
        }
        //CloseConnection();
        return retstr;

    }


    public bool isadmin()
    {
        bool retstr = false;
        //OpenConnection();
        string str = "select uid from users where uid=" + Convert.ToInt32(HttpContext.Current.Request.Cookies["user"]["user_id"].ToString()) + " and is_admin=1";
        myds = GetDataSetInline(str);
        if (myds.Tables[0].Rows.Count > 0)
        {
            retstr = true;
        }
        else
        {
            retstr = false;
        }
        //CloseConnection();
        return retstr;

    }



    public string getuseremail_helpdesk(int did)
    {
        string retstr = "";
        //OpenConnection();
        string str = "select dbo.getusermail_helpdesk(" + did + ") as email_id";
        myds = GetDataSetInline(str);

        // mycmd = new SqlCommand("IFCI_sp_fetch_pageemail", mycon);
        //mycmd.CommandType = CommandType.StoredProcedure;
        //mycmd.Connection = mycon;
        // mycmd.CommandText = "IFCI_sp_fetch_pageemail";
        // mycmd.Parameters.AddRange(param);
        // myda = new SqlDataAdapter(mycmd);
        // myda.Fill(myds);
        if (myds.Tables[0].Rows.Count > 0)
        {
            retstr = myds.Tables[0].Rows[0]["email_id"].ToString();
        }
        else
        {
            retstr = "vivek@netcommlabs.com";
        }
        //CloseConnection();
        return retstr;


    }






    public string getuserid_helpdesk(string mail)
    {
        string retstr = "";
        //OpenConnection();
        string str = "select dbo.getuserid ('" + mail + "') as usrid";
        myds = GetDataSetInline(str);

        // mycmd = new SqlCommand("IFCI_sp_fetch_pageemail", mycon);
        //mycmd.CommandType = CommandType.StoredProcedure;
        //mycmd.Connection = mycon;
        // mycmd.CommandText = "IFCI_sp_fetch_pageemail";
        // mycmd.Parameters.AddRange(param);
        // myda = new SqlDataAdapter(mycmd);
        // myda.Fill(myds);
        if (myds.Tables[0].Rows.Count > 0)
        {
            retstr = myds.Tables[0].Rows[0]["usrid"].ToString();
        }
        else
        {
            retstr = "0";
        }
        //CloseConnection();
        return retstr;


    }


}
