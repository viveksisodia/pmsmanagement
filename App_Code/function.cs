
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
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

/// <summary>
/// Summary description for function
/// </summary>
public class function
{
    DataAccess da = new DataAccess();
    CommonFunction cmf = new CommonFunction();
    SqlParameter[] prm;
    DataSet ds = new DataSet();
    public function()
    {
        //
        // TODO: Add constructor logic here
        //
    }


    public string getbranchidbyempid(string empid)
    {
        string strret = "0";

        string strsql = "select isnull(Location,0) as branchid from tbl_employee where id='" + empid + "'";
        DataSet dsl = da.GetDataSetInline(strsql);
        if (dsl.Tables[0].Rows.Count > 0)
        {
            strret = dsl.Tables[0].Rows[0]["branchid"].ToString();
        }
        return strret;

    }

    public void bindbranch(string empid, DropDownList ddlbranch, string regid, string typeid)
    {
        SqlParameter[] prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@empid", empid);
        prm[1] = new SqlParameter("@regid", regid);
        DataSet ds = da.GetDataSet("tbl_locationdetails_select", prm);
        cmf.BindDropDownList(ddlbranch, ds, "branchname", "id");
        if (typeid == "1")
        {
            ListItem li = new ListItem("--Select Location--", "0");
            ddlbranch.Items.Insert(0, li);
        }
        else
        {
            ListItem li = new ListItem("--All Location--", "%");
            ddlbranch.Items.Insert(0, li);

        }
    }
    public void bindbranchwithidforinventory(string empid, DropDownList ddlbranch, string regid, string typeid, string optionid, string branchid)
    {
        SqlParameter[] prm = new SqlParameter[4];
        prm[0] = new SqlParameter("@empid", empid);
        prm[1] = new SqlParameter("@regid", regid);
        prm[2] = new SqlParameter("@optionid", optionid);
        prm[3] = new SqlParameter("@branchid", branchid);
        DataSet ds = da.GetDataSet("tbl_locationdetails_option", prm);
        cmf.BindDropDownList(ddlbranch, ds, "branchname", "id");
        if (typeid == "1")
        {
            ListItem li = new ListItem("--Select Location--", "0");
            ddlbranch.Items.Insert(0, li);
        }
        else
        {
            ListItem li = new ListItem("--All Location--", "%");
            ddlbranch.Items.Insert(0, li);

        }

    }
    public void updatebranchid(string branchid, string id, string typeid)
    {
        SqlParameter[] prm = new SqlParameter[3];
        prm[0] = new SqlParameter("@branchid", branchid);
        prm[1] = new SqlParameter("@id", id);
        prm[2] = new SqlParameter("@typeid", typeid);
        da.executeDMLQuery("branch_updateinqpsib", prm);
    }

    public void bindbranch(string empid, DropDownList ddlbranch, string regid)
    {
        SqlParameter[] prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@empid", empid);
        prm[1] = new SqlParameter("@regid", regid);
        DataSet ds = da.GetDataSet("tbl_locationdetails_select", prm);
        cmf.BindDropDownList(ddlbranch, ds, "branchname", "id");
        ListItem li = new ListItem("--Select Branch--", "0");
        ddlbranch.Items.Insert(0, li);

    }
    public void bindSites(string empid, DropDownList ddlsites, string regid)
    {
        SqlParameter[] prm = new SqlParameter[3];
        prm[0] = new SqlParameter("@uid", empid);
        prm[1] = new SqlParameter("@regid", regid);
        prm[2] = new SqlParameter("@Mode", "SELECTBYID");
        DataSet ds = da.GetDataSet("tbl_locationdetails_select", prm);
        cmf.BindDropDownList(ddlsites, ds, "sitename", "id");
        ListItem li = new ListItem("--Select Site--", "0");
        ddlsites.Items.Insert(0, li);

    }
    public string getitemunitname(string id)
    {

        string strret = "";
        string strpvalue = "select Unitval from tbl_itemsdetails where id = '" + id + "' ";
        DataSet dataSet = da.GetDataSetInline(strpvalue);
        if (dataSet.Tables[0].Rows.Count > 0)
        {
            strret = dataSet.Tables[0].Rows[0]["Unitval"].ToString();
        }
        return strret;
    }


    public bool isaddnewitem()
    {

        string strsql = "select ddladdnew from tbl_Preferencesitems where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0";
        DataSet dataSet = da.GetDataSetInline(strsql);
        if (dataSet.Tables[0].Rows.Count > 0)
        {
            if (dataSet.Tables[0].Rows[0]["ddladdnew"].ToString() == "2")
            {
                return false;
            }
        }
        return true;
    }
    public bool isaddnewcontact()
    {

        string strsql = "select ddladdnew from tbl_Preferencescontact where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0";
        DataSet dataSet = da.GetDataSetInline(strsql);
        if (dataSet.Tables[0].Rows.Count > 0)
        {
            if (dataSet.Tables[0].Rows[0]["ddladdnew"].ToString() == "2")
            {
                return false;
            }
        }
        return true;
    }

    public string getitemunitid(string id)
    {

        string strret = "";
        string strpvalue = "select unitvalue from tbl_itemsdetails where id = '" + id + "' ";
        DataSet dataSet = da.GetDataSetInline(strpvalue);
        if (dataSet.Tables[0].Rows.Count > 0)
        {
            strret = dataSet.Tables[0].Rows[0]["unitvalue"].ToString();
        }
        return strret;
    }
    public string getparmnamevalue(string id)
    {

        string strret = "";
        string strpvalue = "select id,pcode,pvalue from tbl_parametervalues where id = '" + id + "' and status = 1 and archive = 0 order by id";
        DataSet dataSet = da.GetDataSetInline(strpvalue);
        if (dataSet.Tables[0].Rows.Count > 0)
        {
            strret = dataSet.Tables[0].Rows[0]["pvalue"].ToString();
        }
        return strret;
    }
    public string getcolorformo(string id)
    {

        string strret = "";
        switch (id)
        {
            case "1":
                strret = "#13c61c";
                break;
            case "2":
                strret = "#ffc107";
                break;
            case "3":
                strret = "#18b11f";
                break;
            default:
                strret = "#2196f3";
                break;
        }
        return strret;
    }


    public string getregid()
    {

        string strret = HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString();

        return strret;
    }
    public string getuid()
    {

        string strret = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();

        return strret;
    }
    public string showparm(string prm1, string prm2, string prm3, string prm4, string prm5, string prm6, string prm7, string prm8, string prm9)
    {
        string strret = "";
        try
        {

            List<string> prm = new List<string> { prm1, prm2, prm3, prm4, prm5, prm6, prm7, prm8, prm9 };

            string stpramval = "";
            for (int pr = 0; pr < 9; pr++)
            {
                string prcol = "prm" + pr;
                if (prm[pr] != "0" && prm[pr] != String.Empty)
                {
                    if (stpramval != string.Empty)
                    {
                        stpramval += "-" + getparmnamevalue(prm[pr]);
                    }
                    else
                    {
                        stpramval += getparmnamevalue(prm[pr]);
                    }
                }

            }
            strret = strret + " " + stpramval;
        }
        catch
        {

        }

        return strret;


    }


    public string getmonthname(int mid)
    {
        string strret = "";
        switch (mid)
        {
            case 1:
                strret = "JAN";
                break;
            case 2:
                strret = "FEB";
                break;
            case 3:
                strret = "MAR";
                break;
            case 4:
                strret = "APR";
                break;
            case 5:
                strret = "MAY";
                break;
            case 6:
                strret = "JUN";
                break;
            case 7:
                strret = "JUL";
                break;
            case 8:
                strret = "AUG";
                break;
            case 9:
                strret = "SEP";
                break;
            case 10:
                strret = "OCT";
                break;
            case 11:
                strret = "NOV";
                break;
            case 12:
                strret = "DEC";
                break;


        }

        return strret;
    }



    public string getdbname()
    {
        string strret = "";
        dataaccesscomm dacom = new dataaccesscomm();
        SqlParameter[] prm1 = new SqlParameter[1];
        prm1[0] = new SqlParameter("@id", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
        DataSet ds = dacom.GetDataSet("tbl_registrationdetailsselect", prm1);
        if (ds.Tables[0].Rows.Count > 0)
        {
            strret = ds.Tables[0].Rows[0]["databasename"].ToString();
        }

        return strret;
    }

    public void updateitemopingclosingbalanceamount(string itemid)
    {
        SqlParameter[] prmcc = new SqlParameter[1];
        prmcc[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
        DataSet dscc = da.GetDataSet("tbl_companydetails_selectbyregidforbooksdate", prmcc);
        if (dscc.Tables[0].Rows.Count > 0)
        {
            string dateofbooks = dscc.Tables[0].Rows[0]["txtdateofbooks"].ToString();



            SqlParameter[] prmdate = new SqlParameter[2];
            prmdate[0] = new SqlParameter("@startdate", dateofbooks);
            prmdate[1] = new SqlParameter("@enddate", da.getcdate());
            DataSet dsdate = da.GetDataSet("getallfinancialdateindia", prmdate);
            //HttpContext.Current.Response.Write(dsdate.Tables[0].Rows.Count);
            //HttpContext.Current.Response.End();
            if (dsdate.Tables[0].Rows.Count > 0)
            {
                for (int dd = 0; dd < dsdate.Tables[0].Rows.Count; dd++)
                {
                    string fromdate = dsdate.Tables[0].Rows[dd]["StartDate"].ToString();
                    string todate = dsdate.Tables[0].Rows[dd]["EndOfYear"].ToString();

                    SqlParameter[] prm = new SqlParameter[3];
                    prm[0] = new SqlParameter("@itemid", itemid);
                    prm[1] = new SqlParameter("@cdate", fromdate);
                    prm[2] = new SqlParameter("@todate", todate);
                    DataSet ds = da.GetDataSet("selecttbl_itemsdetailsbalance_cat_openingclosingbyitemid", prm);

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        decimal opeingbalance = 0;
                        decimal closingbalance = 0;
                        string OpeningStock = ds.Tables[0].Rows[i]["OpeningStock"].ToString();
                        string OpeningStockrateperunit = ds.Tables[0].Rows[i]["OpeningStockrateperunit"].ToString();

                        if (OpeningStock == string.Empty)
                        {
                            OpeningStock = "0";
                        }
                        if (OpeningStockrateperunit == string.Empty)
                        {
                            OpeningStockrateperunit = "0";
                        }
                        opeingbalance = opeingstock(ds.Tables[0].Rows[i]["id"].ToString(), fromdate, todate, ds.Tables[0].Rows[i]["openingbalances"].ToString(), OpeningStock, OpeningStockrateperunit);
                        closingbalance = closingstock(ds.Tables[0].Rows[i]["id"].ToString(), fromdate, todate, ds.Tables[0].Rows[i]["closingbalances"].ToString(), OpeningStock, OpeningStockrateperunit);



                        SqlParameter[] prmup = new SqlParameter[6];
                        prmup[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                        prmup[1] = new SqlParameter("@itemid", ds.Tables[0].Rows[i]["id"].ToString());
                        prmup[2] = new SqlParameter("@fromdate", fromdate);
                        prmup[3] = new SqlParameter("@todate", todate);
                        prmup[4] = new SqlParameter("@openingamount", opeingbalance);
                        prmup[5] = new SqlParameter("@closingamount", closingbalance);
                        da.executeDMLQuery("tbl_stockopeningclosingamountinsertupdate", prmup);
                    }
                }
            }
        }

    }

    protected decimal opeingstock(string itemid, string fromdate, string todate, string openingbalances, string OpeningStock, string OpeningStockrateperunit)
    {
        decimal totalopeningbalance = 0;
        try
        {

            DataAccess Da = new DataAccess();

            //   string 



            decimal totalitemquantity = Convert.ToDecimal(openingbalances);

            if (totalitemquantity > 0)
            {






                SqlParameter[] prmitem = new SqlParameter[3];
                prmitem[0] = new SqlParameter("@itemid", itemid);
                prmitem[1] = new SqlParameter("@cdate", fromdate);
                prmitem[2] = new SqlParameter("@todate", todate);
                DataSet dsitemdetails = Da.GetDataSet("selectitemvalueforprofit", prmitem);
                if (dsitemdetails.Tables[0].Rows.Count > 0)
                {


                    for (int j = 0; j < dsitemdetails.Tables[0].Rows.Count; j++)
                    {



                        decimal totalquntitypur = Convert.ToDecimal(dsitemdetails.Tables[0].Rows[j]["quantity"].ToString());

                        if (totalitemquantity <= totalquntitypur)
                        {


                            //  Response.Write(totalitemquantity + ",");
                            try
                            {


                                if (dsitemdetails.Tables[0].Rows[j]["convertionrate"].ToString() != string.Empty)
                                {
                                    totalopeningbalance += totalitemquantity * (Convert.ToDecimal(dsitemdetails.Tables[0].Rows[j]["rate"].ToString()) * Convert.ToDecimal(dsitemdetails.Tables[0].Rows[j]["convertionrate"].ToString()));
                                    totalitemquantity = 0;
                                }
                                else
                                {
                                    totalopeningbalance += totalitemquantity * (Convert.ToDecimal(dsitemdetails.Tables[0].Rows[j]["rate"].ToString()));
                                    totalitemquantity = 0;
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                            break;
                        }
                        else
                        {

                            try
                            {

                                if (dsitemdetails.Tables[0].Rows[j]["convertionrate"].ToString() != string.Empty)
                                {
                                    totalopeningbalance += Convert.ToDecimal(dsitemdetails.Tables[0].Rows[j]["quantity"].ToString()) * (Convert.ToDecimal(dsitemdetails.Tables[0].Rows[j]["rate"].ToString()) * Convert.ToDecimal(dsitemdetails.Tables[0].Rows[j]["convertionrate"].ToString()));
                                    totalitemquantity = totalitemquantity - Convert.ToDecimal(dsitemdetails.Tables[0].Rows[j]["quantity"].ToString());
                                }
                                else
                                {
                                    totalopeningbalance += Convert.ToDecimal(dsitemdetails.Tables[0].Rows[j]["quantity"].ToString()) * (Convert.ToDecimal(dsitemdetails.Tables[0].Rows[j]["rate"].ToString()));
                                    totalitemquantity = totalitemquantity - Convert.ToDecimal(dsitemdetails.Tables[0].Rows[j]["quantity"].ToString());

                                }
                                // Response.Write(totalitemquantity + ",");
                            }
                            catch (Exception ex)
                            {
                            }


                        }


                    }



                }



                if (totalitemquantity > 0)
                {


                    decimal totalquantityOpeningStock = Convert.ToDecimal(OpeningStock);

                    if (totalitemquantity <= totalquantityOpeningStock)
                    {
                        try
                        {
                            totalopeningbalance += totalitemquantity * (Convert.ToDecimal(OpeningStockrateperunit));
                            totalitemquantity = 0;
                        }
                        catch (Exception ex)
                        {
                        }

                    }
                    else
                    {
                        try
                        {
                            totalopeningbalance += totalquantityOpeningStock * (Convert.ToDecimal(OpeningStockrateperunit));
                            totalitemquantity = totalitemquantity - totalquantityOpeningStock;
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }





        }
        catch (Exception ex)
        {

        }
        return totalopeningbalance;
    }

    protected decimal closingstock(string itemid, string fromdate, string todate, string openingbalances, string OpeningStock, string OpeningStockrateperunit)
    {
        decimal totalopeningbalance = 0;
        try
        {

            DataAccess Da = new DataAccess();

            //   string 



            decimal totalitemquantity = Convert.ToDecimal(openingbalances);

            if (totalitemquantity > 0)
            {






                SqlParameter[] prmitem = new SqlParameter[3];
                prmitem[0] = new SqlParameter("@itemid", itemid);
                prmitem[1] = new SqlParameter("@cdate", fromdate);
                prmitem[2] = new SqlParameter("@todate", todate);
                DataSet dsitemdetails = Da.GetDataSet("selectitemvalueforprofitclose", prmitem);
                if (dsitemdetails.Tables[0].Rows.Count > 0)
                {


                    for (int j = 0; j < dsitemdetails.Tables[0].Rows.Count; j++)
                    {



                        decimal totalquntitypur = Convert.ToDecimal(dsitemdetails.Tables[0].Rows[j]["quantity"].ToString());

                        if (totalitemquantity <= totalquntitypur)
                        {


                            //  Response.Write(totalitemquantity + ",");
                            try
                            {


                                if (dsitemdetails.Tables[0].Rows[j]["convertionrate"].ToString() != string.Empty)
                                {
                                    totalopeningbalance += totalitemquantity * (Convert.ToDecimal(dsitemdetails.Tables[0].Rows[j]["rate"].ToString()) * Convert.ToDecimal(dsitemdetails.Tables[0].Rows[j]["convertionrate"].ToString()));
                                    totalitemquantity = 0;
                                }
                                else
                                {
                                    totalopeningbalance += totalitemquantity * (Convert.ToDecimal(dsitemdetails.Tables[0].Rows[j]["rate"].ToString()));
                                    totalitemquantity = 0;
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                            break;
                        }
                        else
                        {

                            try
                            {

                                if (dsitemdetails.Tables[0].Rows[j]["convertionrate"].ToString() != string.Empty)
                                {
                                    totalopeningbalance += Convert.ToDecimal(dsitemdetails.Tables[0].Rows[j]["quantity"].ToString()) * (Convert.ToDecimal(dsitemdetails.Tables[0].Rows[j]["rate"].ToString()) * Convert.ToDecimal(dsitemdetails.Tables[0].Rows[j]["convertionrate"].ToString()));
                                    totalitemquantity = totalitemquantity - Convert.ToDecimal(dsitemdetails.Tables[0].Rows[j]["quantity"].ToString());
                                }
                                else
                                {
                                    totalopeningbalance += Convert.ToDecimal(dsitemdetails.Tables[0].Rows[j]["quantity"].ToString()) * (Convert.ToDecimal(dsitemdetails.Tables[0].Rows[j]["rate"].ToString()));
                                    totalitemquantity = totalitemquantity - Convert.ToDecimal(dsitemdetails.Tables[0].Rows[j]["quantity"].ToString());

                                }
                                // Response.Write(totalitemquantity + ",");
                            }
                            catch (Exception ex)
                            {
                            }


                        }


                    }



                }



                if (totalitemquantity > 0)
                {


                    decimal totalquantityOpeningStock = Convert.ToDecimal(OpeningStock);

                    if (totalitemquantity <= totalquantityOpeningStock)
                    {
                        try
                        {
                            totalopeningbalance += totalitemquantity * (Convert.ToDecimal(OpeningStockrateperunit));
                            totalitemquantity = 0;
                        }
                        catch (Exception ex)
                        {
                        }

                    }
                    else
                    {
                        try
                        {
                            totalopeningbalance += totalquantityOpeningStock * (Convert.ToDecimal(OpeningStockrateperunit));
                            totalitemquantity = totalitemquantity - totalquantityOpeningStock;
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }





        }
        catch (Exception ex)
        {

        }
        return totalopeningbalance;
    }


    public string getmobilenoofcustomer(string customerid)
    {
        string stret = "";

        string strsql = "select txtmobile from tbl_contactsdetails where id='" + customerid + "'";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            stret = ds.Tables[0].Rows[0]["txtmobile"].ToString();
        }

        return stret;
    }
    public string getnameofcustomer(string customerid)
    {
        string stret = "";

        string strsql = "select txtcompanyname from tbl_contactsdetails where id='" + customerid + "'";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            stret = ds.Tables[0].Rows[0]["txtcompanyname"].ToString();
        }

        return stret;
    }

    public string getWarehouseslable()
    {
        string strret = "";
        string strsql = "select rdlWarehouse,txtWarehouselabel from tbl_companydetails where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "'";

        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            strret = ds.Tables[0].Rows[0]["rdlWarehouse"].ToString() + "~" + ds.Tables[0].Rows[0]["txtWarehouselabel"].ToString();
        }
        return strret;
    }



    public string getempsalarypermonth(string empid)
    {


        string strret = "";


        string strsql = "select top 1 * from dbo.tbl_employeesalarydetails where empid='" + empid + "'  and status=1 and archive=0 order by efectivedate desc";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {

            try
            {
                decimal totalsalaryperanm = Convert.ToDecimal(ds.Tables[0].Rows[0]["totalsalaryamount"].ToString());
                decimal totalsalarypermonth = Math.Round((Convert.ToDecimal(totalsalaryperanm) / 12), 2);

                strret = totalsalarypermonth.ToString();
            }
            catch (Exception ex)
            {

            }
        }

        return strret;
    }


    public int DateDifference(DateTime d1, DateTime d2)
    {

        //Storing input Dates  
        DateTime FromYear = Convert.ToDateTime(d1);
        DateTime ToYear = Convert.ToDateTime(d2);

        //Creating object of TimeSpan Class  
        TimeSpan objTimeSpan = ToYear - FromYear;
        //years  
        int Years = ToYear.Year - FromYear.Year;
        //months  
        int month = ToYear.Month - FromYear.Month;
        //TotalDays  
        double tDays = Convert.ToDouble(objTimeSpan.TotalDays);
        //Total Months  
        int TotalMonths = (Years * 12) + month;
        //Total Hours  
        double TotalHours = objTimeSpan.TotalHours;
        //Total Minutes  
        double TotalMinutes = objTimeSpan.TotalMinutes;
        //Total Seconds  
        double TotalSeconds = objTimeSpan.TotalSeconds;
        //Total Mile Seconds  
        double TotalMileSeconds = objTimeSpan.TotalMilliseconds;
        //Assining values to td tags  

        int year = Years;

        TimeSpan t = d1 - d2;
        double NrOfDays = t.TotalDays;
        // this.day = Convert.ToInt32(NrOfDays);
        int day = Convert.ToInt32(tDays);

        return (day + 1);


    }






    public string showbonusstatus(string val)
    {

        string strret = "";
        if (val == "0")
        {
            strret = "" + getltext("Pending", "2") + "";
        }
        if (val == "1")
        {
            strret = "" + getltext("Paid Outside AbsoluteERP Payroll", "2") + "";
        }


        return strret;
    }
    public int getcountryid()
    {
        int countryid = 0;
       

        string strsql = "select isnull(countryid,2) as countryid from tbl_registrationdetails where id='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "'";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            countryid = Convert.ToInt32(ds.Tables[0].Rows[0]["countryid"].ToString());
        }
        try
        {
            if (HttpContext.Current.Request.Cookies["regcid"] != null)
            {
                if (HttpContext.Current.Request.Cookies["regcid"]["regcidname"] != null)
                {
                    countryid = Convert.ToInt32(HttpContext.Current.Request.Cookies["regcid"]["regcidname"].ToString());
                }
            }
        }
        catch
        {

        }
        return countryid;
    }
    public int getstateid()
    {
        int countryid = 0;
        string strsql = "select isnull(stateid,0) as stateid from tbl_companydetails where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "'";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            countryid = Convert.ToInt32(ds.Tables[0].Rows[0]["stateid"].ToString());
        }

        return countryid;
    }
    public int getstateidbybranchid(string branchid)
    {
        int countryid = 0;
        if (branchid != string.Empty && branchid != "0")
        {
            string strsql = "select isnull(stateid,0) as stateid from tbl_locationdetails where id='" + branchid + "'";
            DataSet ds = da.GetDataSetInline(strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                countryid = Convert.ToInt32(ds.Tables[0].Rows[0]["stateid"].ToString());
            }
        }
        else
        {

            string strsql = "select isnull(stateid,0) as stateid from tbl_companydetails where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "'";
            DataSet ds = da.GetDataSetInline(strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                countryid = Convert.ToInt32(ds.Tables[0].Rows[0]["stateid"].ToString());
            }
        }

        return countryid;
    }


    public string getgstnbybranchid(string branchid)
    {
        string countryid = "";
        if (branchid != string.Empty && branchid != "0")
        {
            string strsql = "select vatrnno from tbl_locationdetails where id='" + branchid + "'";
            DataSet ds = da.GetDataSetInline(strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                countryid = ds.Tables[0].Rows[0]["vatrnno"].ToString();
            }
        }


        return countryid;
    }

    public byte[] ResizeImageFile(byte[] imageFile, int width, int height)
    {
        try
        {
            using (System.Drawing.Image oldImage = System.Drawing.Image.FromStream(new MemoryStream(imageFile)))
            {
                if (oldImage.Width > width || oldImage.Height > height)
                {
                    var ratioX = (double)width / oldImage.Width;
                    var ratioY = (double)height / oldImage.Height;
                    var ratio = Math.Min(ratioX, ratioY);

                    width = (int)(oldImage.Width * ratio);
                    height = (int)(oldImage.Height * ratio);
                }
                else
                {
                    width = oldImage.Width;
                    height = oldImage.Height;
                }



                //if (oldImage.Height > oldImage.Width)
                //{
                //    width = (int)(oldImage.Width * ((float)height / (float)oldImage.Height));



                //}

                //else
                //{


                //    height = (int)(oldImage.Height * ((float)height / (float)oldImage.Width));

                //}



                // Size newSize = CalculateDimensions(oldImage.Size, targetSize);

                //using (Bitmap newImage = new Bitmap(newSize.Width, newSize.Height, PixelFormat.Format24bppRgb))
                using (Bitmap newImage = new Bitmap(width, height, PixelFormat.Format24bppRgb))
                {
                    using (Graphics canvas = Graphics.FromImage(newImage))
                    {

                        canvas.SmoothingMode = SmoothingMode.AntiAlias;

                        canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;

                        canvas.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        //canvas.DrawImage(oldImage, new Rectangle(new Point(0, 0), newSize)); MemoryStream m = new MemoryStream();
                        canvas.DrawImage(oldImage, new Rectangle(0, 0, width, height));
                        MemoryStream m = new MemoryStream();
                        newImage.Save(m, ImageFormat.Jpeg);
                        return m.GetBuffer();

                    }

                }

            }
        }
        catch (Exception)
        {
            MemoryStream m = new MemoryStream();
            return m.GetBuffer();
        }
    }

    public int ispayrolladminmenu(string empid)
    {
        int retint = 0;


        string strsqladminn = "select Manage_Profile.* from Manage_Profile inner join tbl_employee on Manage_Profile.id=cast(tbl_employee.roleid as int) where tbl_employee.id='" + empid + "'  and Manage_Profile.status=1 and Manage_Profile.archive=0";
        DataSet dsadmin = da.GetDataSetInline(strsqladminn);
        if (dsadmin.Tables[0].Rows.Count > 0)
        {

            if (dsadmin.Tables[0].Rows[0]["id"].ToString() == "1")
            {
                return 1;
            }
        }

        string strsql = "select empid from tbl_payrollempPermissions where empid='" + empid + "'  and (rollid=2 or rollid=1) and status=1 and archive=0";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            retint = 1;
        }


        return retint;
    }

    public int isHRManagermenu(string empid)
    {
        int retint = 0;


        string strsqladminn = "select Manage_Profile.* from Manage_Profile inner join tbl_employee on Manage_Profile.id=cast(tbl_employee.roleid as int) where tbl_employee.id='" + empid + "'  and Manage_Profile.status=1 and Manage_Profile.archive=0";
        DataSet dsadmin = da.GetDataSetInline(strsqladminn);
        if (dsadmin.Tables[0].Rows.Count > 0)
        {

            if (dsadmin.Tables[0].Rows[0]["id"].ToString() == "1")
            {
                return 1;
            }
        }

        string strsql = "select empid from tbl_payrollempPermissions where empid='" + empid + "'  and (rollid=3 or rollid=1)  and status=1 and archive=0";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            retint = 1;
        }


        return retint;
    }
    public int isHRExecutivemenu(string empid)
    {
        int retint = 0;

        string strsqladminn = "select Manage_Profile.* from Manage_Profile inner join tbl_employee on Manage_Profile.id=cast(tbl_employee.roleid as int) where tbl_employee.id='" + empid + "'  and Manage_Profile.status=1 and Manage_Profile.archive=0";
        DataSet dsadmin = da.GetDataSetInline(strsqladminn);
        if (dsadmin.Tables[0].Rows.Count > 0)
        {

            if (dsadmin.Tables[0].Rows[0]["id"].ToString() == "1")
            {
                return 1;
            }
        }


        string strsql = "select empid from tbl_payrollempPermissions where empid='" + empid + "'  and (rollid=4 or rollid=1)  and status=1 and archive=0";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            retint = 1;
        }


        return retint;
    }
    public int isAssetManagermenu(string empid)
    {
        int retint = 0;

        string strsqladminn = "select Manage_Profile.* from Manage_Profile inner join tbl_employee on Manage_Profile.id=cast(tbl_employee.roleid as int) where tbl_employee.id='" + empid + "'  and Manage_Profile.status=1 and Manage_Profile.archive=0";
        DataSet dsadmin = da.GetDataSetInline(strsqladminn);
        if (dsadmin.Tables[0].Rows.Count > 0)
        {

            if (dsadmin.Tables[0].Rows[0]["id"].ToString() == "1")
            {
                return 1;
            }
        }


        string strsql = "select empid from tbl_payrollempPermissions where empid='" + empid + "'  and (rollid=5 or rollid=1)  and status=1 and archive=0";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            retint = 1;
        }


        return retint;
    }
    public int isadminmenu(string empid)
    {
        int retint = 0;

        string strsqladminn = "select Manage_Profile.* from Manage_Profile inner join tbl_employee on Manage_Profile.id=cast(tbl_employee.roleid as int) where tbl_employee.id='" + empid + "'  and Manage_Profile.status=1 and Manage_Profile.archive=0";
        DataSet dsadmin = da.GetDataSetInline(strsqladminn);
        if (dsadmin.Tables[0].Rows.Count > 0)
        {

            if (dsadmin.Tables[0].Rows[0]["id"].ToString() == "1")
            {
                return 1;
            }
        }


        string strsql = "select empid from tbl_payrollempPermissions where empid='" + empid + "'  and rollid=1 and status=1 and archive=0";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            retint = 1;
        }


        return retint;
    }

    public int istl(string empid)
    {
        int retint = 0;




        string strsqladminn = "select Manage_Profile.* from Manage_Profile inner join tbl_employee on Manage_Profile.id=cast(tbl_employee.roleid as int) where tbl_employee.id='" + empid + "'  and Manage_Profile.status=1 and Manage_Profile.archive=0";
        DataSet dsadmin = da.GetDataSetInline(strsqladminn);
        if (dsadmin.Tables[0].Rows.Count > 0)
        {

            if (dsadmin.Tables[0].Rows[0]["id"].ToString() == "1")
            {
                return 1;
            }
        }


        string strsql = "select empid from tbl_payrollempPermissions where empid='" + empid + "'  and rollid=1 and status=1 and archive=0";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            retint = 1;
        }
        else
        {
            strsql = "select id from tbl_employee where  councelor_id='" + empid + "' and status=1 and archive=0";
            ds = da.GetDataSetInline(strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                retint = 1;
            }
        }


        return retint;
    }


    public int istl(string empid, string reportee)
    {
        int retint = 0;
        string strsql = "select empid from tbl_payrollempPermissions where empid='" + empid + "'  and rollid=1 and status=1 and archive=0";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            retint = 1;
        }

        else
        {
            strsql = "select id from tbl_employee where  councelor_id='" + empid + "' and id='" + reportee + "' and status=1 and archive=0";
            ds = da.GetDataSetInline(strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                retint = 1;
            }
        }


        return retint;
    }




    public string l2managerid(string empid)
    {
        string tlempid = "0";
        string strsql = "select councelor_id as ReportingManager from tbl_employee where id in(select  councelor_id from tbl_employee where id='" + empid + "') and status=1 and archive=0";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["ReportingManager"].ToString() != string.Empty)
            {
                tlempid = ds.Tables[0].Rows[0]["ReportingManager"].ToString();
            }
        }
        return tlempid;
    }


    public string teamleaderid(string empid)
    {
        string tlempid = "0";

        string strsql = "select councelor_id as ReportingManager from tbl_employee where id='" + empid + "'";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["ReportingManager"].ToString() != string.Empty)
            {
                tlempid = ds.Tables[0].Rows[0]["ReportingManager"].ToString();
            }
        }
        return tlempid;
    }


    public string showcallstatus(string val)
    {

        string strret = "";


        if (val == "1")
        {

            strret = "<span style='font-size:15px;' >" + getltext("Follow up", "2") + "</span>";
        }
        if (val == "2")
        {

            strret = "<span style='font-size:15px;'>" + getltext("Meeting", "2") + "</span>";
        }
        if (val == "3")
        {

            strret = "<span style='font-size:15px;' class='text-danger'>" + getltext("Not Interested", "2") + "</span>";
        }
        if (val == "4")
        {
            strret = "<span style='font-size:15px;' class='text-primary'>" + getltext("Quotation", "2") + "</span>";

        }
        if (val == "5")
        {
            strret = "<span style='font-size:15px;' class='text-success'>" + getltext("Sales Order", "2") + "</span>";

        }
        if (val == "6")
        {
            strret = "<span style='font-size:15px;' class='text-success'>" + getltext("Invoice", "2") + "</span>";

        }
        if (val == "7")
        {

            strret = "<span style='font-size:15px;'>" + getltext("Demo", "2") + "</span>";
        }
        return strret;


    }
    public string getcity(string id)
    {
        string strret = "";
        string strsql = "select city_name from cities where city_id='" + id + "'";
        try
        {
            strret = da.getexecutescalar(strsql);
        }
        catch
        {

        }
        return strret;

    }
    public string getstate(string id)
    {
        string strret = "";
        string strsql = "select statename from tbl_statemaster where id='" + id + "'";
        try
        {
            strret = da.getexecutescalar(strsql);
        }
        catch
        {

        }
        return strret;
    }


    public string getempnamebyid(string empid)
    {
        string tlempid = "";


        char[] ch = { ',' };

        string[] arremp = empid.Split(ch, StringSplitOptions.None);

        for (int jj = 0; jj < arremp.Length; jj++)
        {

            string strsql = "select displayname,f_name,l_name from tbl_employee where id='" + arremp[jj] + "'";
            DataSet ds = da.GetDataSetInline(strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (tlempid == string.Empty)
                {

                    if (ds.Tables[0].Rows[0]["displayname"].ToString() != string.Empty)
                    {
                        tlempid = ds.Tables[0].Rows[0]["displayname"].ToString();
                    }
                    else
                    {
                        tlempid = ds.Tables[0].Rows[0]["f_name"].ToString() + " " + ds.Tables[0].Rows[0]["l_name"].ToString();
                    }
                }
                else
                {
                    if (ds.Tables[0].Rows[0]["displayname"].ToString() != string.Empty)
                    {
                        tlempid += "," + ds.Tables[0].Rows[0]["displayname"].ToString();
                    }
                    else
                    {
                        tlempid += "," + ds.Tables[0].Rows[0]["f_name"].ToString() + " " + ds.Tables[0].Rows[0]["l_name"].ToString();
                    }
                }

            }
        }
        return tlempid;
    }
    public string getShiftsnamebyid(string empid)
    {
        string tlempid = "";


        char[] ch = { ',' };

        string[] arremp = empid.Split(ch, StringSplitOptions.None);

        for (int jj = 0; jj < arremp.Length; jj++)
        {

            string strsql = "select shiftname from tbl_shift where id='" + arremp[jj] + "'";
            DataSet ds = da.GetDataSetInline(strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (tlempid == string.Empty)
                {

                    if (ds.Tables[0].Rows[0]["shiftname"].ToString() != string.Empty)
                    {
                        tlempid = ds.Tables[0].Rows[0]["shiftname"].ToString();
                    }

                }
                else
                {
                    if (ds.Tables[0].Rows[0]["shiftname"].ToString() != string.Empty)
                    {
                        tlempid += "," + ds.Tables[0].Rows[0]["shiftname"].ToString();
                    }

                }

            }
        }
        return tlempid;
    }

    public string getempcodebyid(string empid)
    {
        string tlempid = "0";

        string strsql = "select EmpNo from tbl_employee where id='" + empid + "'";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {

            tlempid = ds.Tables[0].Rows[0]["EmpNo"].ToString();


        }
        return tlempid;
    }

    public string getempmobilenobyid(string empid)
    {
        string tlempid = "";

        string strsql = "select mobile_no,f_name,l_name from tbl_employee where id='" + empid + "'";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["mobile_no"].ToString() != string.Empty)
            {
                tlempid = ds.Tables[0].Rows[0]["mobile_no"].ToString();
            }


        }
        return tlempid;
    }
    public string getempemailbyid(string empid)
    {
        string tlempid = "";

        string strsql = "select email_Id,f_name,l_name from tbl_employee where id='" + empid + "'";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["email_Id"].ToString() != string.Empty)
            {
                tlempid = ds.Tables[0].Rows[0]["email_Id"].ToString();
            }


        }
        return tlempid;
    }
    public string binddataempteam(string empid)
    {

        string empdetails = "";
        empdetails = "<div class='row'>";
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        ////ADO.Net
        //SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();


        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@empid", empid);
        ds = da.GetDataSet("selectemployeedetailsforteamleader", prm);

        dt = ds.Tables[0];



        foreach (DataRow row in dt.Rows)
        {
            char ch1;

            if (row["LastName"] != string.Empty)
            {
                try
                {
                    ch1 = row["LastName"].ToString().ToCharArray().ElementAt(0);
                }
                catch
                {
                    ch1 = ' ';
                }

            }
            else
            {
                ch1 = ' ';
            }

            char ch = ' ';
            try
            {
                if (row["FirstName"] != string.Empty)
                {
                    ch = row["FirstName"].ToString().ToCharArray().ElementAt(0);
                }
            }
            catch
            {

            }
            // char ch1 = row["LastName"].ToString().ToCharArray().ElementAt(0);
            empdetails += "<div class='col-sm-6'><div class='member-detail clear-side-padding ng-scope'>";
            empdetails += "<div class='member-profile-picture-container profile-picture-container'>";

            empdetails += "<div class='profile-picture teamPictures ng-scope ng-isolate-scope'>";
            if (row["empimg"].ToString() == string.Empty)
            {

                empdetails += "<div class='img-initials ng-binding' style='background-color: rgb(21, 160, 203);'>" + ch + ch1 + "</div>";
            }
            else
            {
                //if (File.Exists(HttpServerUtility Server.MapPath("/employee/emppic/" + row["empimg"].ToString())))
                //{
                empdetails += "   <img src='/employee/emppic/" + row["empimg"].ToString() + "' />";
                //}
                //else
                //{
                //    empdetails += "<div class='img-initials ng-binding' style='background-color: rgb(21, 160, 203);'>" + ch + ch1 + "</div>";
                //}

            }
            empdetails += "</div>";
            empdetails += "</div>";
            empdetails += "<div>";
            empdetails += "<a href='/employee/employee-summary.aspx?empid=" + cmf.Encrypt(row["id"].ToString()) + "'>";
            empdetails += "<h4 class='ng-binding'>" + row["empname"].ToString() + "</h4>";
            empdetails += " </a>";
            empdetails += "<h5><span class=''>" + row["Designation"].ToString() + "</span></h5>";
            empdetails += "<h5><span class=''>" + row["MobileNumber"].ToString() + "</span></h5>";
            empdetails += "</div>";
            empdetails += "</div>";
            empdetails += "</div>";





        }

        empdetails += "</div>";

        if (ds.Tables[0].Rows.Count == 0)
        {
            empdetails = "<div class='alert alert-info'>" + getltext("No Data Available", "2") + "</div>";

        }




        return empdetails;
    }


    public string binddataempteamsumery(string empid)
    {

        string empdetails = "";
        empdetails = "<div class='row'>";
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        ////ADO.Net
        //SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();


        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@empid", empid);
        ds = da.GetDataSet("selectemployeedetailsforteamleader", prm);

        dt = ds.Tables[0];



        foreach (DataRow row in dt.Rows)
        {
            char ch1 = ' ';

            if (row["LastName"] != string.Empty)
            {
                try
                {
                    ch1 = row["LastName"].ToString().ToCharArray().ElementAt(0);
                }
                catch
                {

                }

            }
            else
            {
                ch1 = ' ';
            }


            char ch = row["FirstName"].ToString().ToCharArray().ElementAt(0);
            // char ch1 = row["LastName"].ToString().ToCharArray().ElementAt(0);
            empdetails += "<div class='col-sm-12'><div class='member-detail clear-side-padding ng-scope'>";
            empdetails += "<div class='member-profile-picture-container profile-picture-container'>";

            empdetails += "<div class='profile-picture teamPictures ng-scope ng-isolate-scope'>";
            if (row["empimg"].ToString() == string.Empty)
            {

                empdetails += "<div class='img-initials ng-binding' style='background-color: rgb(21, 160, 203);'>" + ch + ch1 + "</div>";
            }
            else
            {
                //if (File.Exists(HttpServerUtility Server.MapPath("/employee/emppic/" + row["empimg"].ToString())))
                //{
                empdetails += "   <img src='/employee/emppic/" + row["empimg"].ToString() + "' />";
                //}
                //else
                //{
                //    empdetails += "<div class='img-initials ng-binding' style='background-color: rgb(21, 160, 203);'>" + ch + ch1 + "</div>";
                //}

            }
            empdetails += "</div>";
            empdetails += "</div>";
            empdetails += "<div>";
            empdetails += "<a href='/employee/employee-summary.aspx?empid=" + cmf.Encrypt(row["id"].ToString()) + "'>";
            empdetails += "<h4 class='ng-binding'>" + row["empname"].ToString() + "</h4>";
            empdetails += " </a>";
            empdetails += "<h5><span class='ng-binding ng-scope'>" + row["Designation"].ToString() + "</span></h5>";
            empdetails += "<h5><span class='ng-binding'>" + row["MobileNumber"].ToString() + "</span></h5>";
            empdetails += "</div>";
            empdetails += "</div>";
            empdetails += "</div>";





        }
        if (ds.Tables[0].Rows.Count == 0)
        {
            empdetails = "<div class='alert alert-info'>" + getltext("No Data Available", "2") + "</div>";

        }


        empdetails += "</div>";

        return empdetails;
    }






    public DataSet getgpreference()
    {




        string strsql = "select * from tbl_PreferencesGeneral where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and status=1 and archive=0";
        DataSet ds = da.GetDataSetInline(strsql);

        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    strret = ds.Tables[0].Rows[0]["isitemsTax"].ToString();
        //}

        return ds;
    }

    public string getnetwaight()
    {
        string strret = "0";
        string strsql = "select * from tbl_PreferencesGeneral where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and status=1 and archive=0";
        DataSet ds = da.GetDataSetInline(strsql);

        if (ds.Tables[0].Rows.Count > 0)
        {
            strret = ds.Tables[0].Rows[0]["isitemsTax"].ToString();
        }

        return strret;
    }


    public string getrtypevalue(string id)
    {
        string strret = "";

        string strsql = "select * from tbl_rentaltypemaster where id='" + id + "'";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            strret = ds.Tables[0].Rows[0]["rtypename"].ToString();
        }

        return strret;
    }

    public string getrsorttypevalue(string id)
    {
        string strret = "";

        string strsql = "select * from tbl_rentaltypemaster where id='" + id + "'";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            strret = ds.Tables[0].Rows[0]["shortname"].ToString();
        }

        return strret;
    }


    public string getcostcenter()
    {
        string strret = "0";




        string strsql = "select * from tbl_PreferencesGeneral where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and status=1 and archive=0";
        DataSet ds = da.GetDataSetInline(strsql);

        if (ds.Tables[0].Rows.Count > 0)
        {
            strret = ds.Tables[0].Rows[0]["rdldiscount"].ToString();
        }

        return strret;
    }
    public string getcostcenter1()
    {
        string strret = "0";




        string strsql = "select * from tbl_PreferencesGeneral where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and status=1 and archive=0";
        DataSet ds = da.GetDataSetInline(strsql);

        if (ds.Tables[0].Rows.Count > 0)
        {
            strret = ds.Tables[0].Rows[0]["rdlcostcenter1"].ToString();
        }

        return strret;
    }

    public string getdiscountenabel()
    {
        string strret = "0";

        //string strsql = "select * from tbl_netwaightregister where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and status=1 and archive=0";
        //DataSet ds = da.GetDataSetInline(strsql);
        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    strret = "1";
        //}


        string strsql = "select * from tbl_PreferencesGeneral where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and status=1 and archive=0";
        DataSet ds = da.GetDataSetInline(strsql);

        if (ds.Tables[0].Rows.Count > 0)
        {
            strret = ds.Tables[0].Rows[0]["isdiscount"].ToString();
        }

        return strret;
    }
    public string turkeynoformateinsert(string currencyAmount)
    {
        string returnstr = currencyAmount;
        //if (getcountryid() == 3)
        //{
        returnstr = returnstr.Replace(",", ".");
        // }
        return returnstr;
    }
    public string turkeynoformateview(string currencyAmount)
    {
        string returnstr = currencyAmount;
        //if (getcountryid() == 3)
        //{
        returnstr = returnstr.Replace(".", ",");
        // }
        return returnstr;
    }
    public string ToFormattedCurrencyString(string currencyAmount)
    {

        string returnstr = currencyAmount;

        //try
        //{
        //    char[] ch={'-'};
        //    string[] arr = currencyAmount.Split(ch, StringSplitOptions.None);

        //    if (arr.Length > 1)
        //    {
        //        currencyAmount = arr[1];
        //    }
        //    var userCurrencyCode = new RegionInfo(CultureInfo.GetCultureInfo("AR-AE").Name).ISOCurrencySymbol;

        //    //if (userCurrencyCode == isoCurrencyCode)
        //    //{
        //    //    return currencyAmount.ToString("C", userCulture);
        //    //}

        //    if (arr.Length > 1)
        //    {
        //        returnstr= string.Format(
        //         "{0}",

        //        "-"+Convert.ToDecimal(currencyAmount).ToString("N2", CultureInfo.GetCultureInfo("AR-AE")));
        //    }
        //    else
        //    {

        //        returnstr= string.Format(
        //        "{0}",

        //       Convert.ToDecimal(currencyAmount).ToString("N2", CultureInfo.GetCultureInfo("AR-AE")));
        //    }
        //}
        //catch (Exception ex)
        //{
        //    returnstr= currencyAmount;
        //}
        if (getcountryid() == 1)
        {
            try
            {
                char[] ch = { '-' };
                string[] arr = currencyAmount.Split(ch, StringSplitOptions.None);

                if (arr.Length > 1)
                {
                    currencyAmount = arr[1];
                }
                var userCurrencyCode = new RegionInfo(CultureInfo.GetCultureInfo("en-IN").Name).ISOCurrencySymbol;

                //if (userCurrencyCode == isoCurrencyCode)
                //{
                //    return currencyAmount.ToString("C", userCulture);
                //}

                if (arr.Length > 1)
                {
                    returnstr = string.Format(
                     "{0}",

                    "-" + Convert.ToDecimal(currencyAmount).ToString("N", CultureInfo.GetCultureInfo("en-IN")));
                }
                else
                {

                    returnstr = string.Format(
                    "{0}",

                   Convert.ToDecimal(currencyAmount).ToString("N", CultureInfo.GetCultureInfo("en-IN")));
                }
            }
            catch (Exception ex)
            {
                returnstr = currencyAmount;
            }
        }
        else
        {
            try
            {
                char[] ch = { '-' };
                string[] arr = currencyAmount.Split(ch, StringSplitOptions.None);

                if (arr.Length > 1)
                {
                    currencyAmount = arr[1];
                }
                var userCurrencyCode = new RegionInfo(CultureInfo.GetCultureInfo("en-us").Name).ISOCurrencySymbol;

                //if (userCurrencyCode == isoCurrencyCode)
                //{
                //    return currencyAmount.ToString("C", userCulture);
                //}

                if (arr.Length > 1)
                {
                    returnstr = string.Format(
                     "{0}",

                    "-" + Convert.ToDecimal(currencyAmount).ToString("N", CultureInfo.GetCultureInfo("en-us")));
                }
                else
                {

                    returnstr = string.Format(
                    "{0}",

                   Convert.ToDecimal(currencyAmount).ToString("N", CultureInfo.GetCultureInfo("en-us")));
                }
            }
            catch (Exception ex)
            {
                returnstr = currencyAmount;
            }
        }

        if (getcountryid() == 3)
        {
            returnstr = returnstr.Replace(".", "-").Replace(",", ".").Replace("-", ",");
        }
        return returnstr;
    }

    public string ToFormattedCurrencyStringwithoutzero(string currencyAmount)
    {

        try
        {
            char[] ch = { '-' };
            string[] arr = currencyAmount.Split(ch, StringSplitOptions.None);

            if (arr.Length > 1)
            {
                currencyAmount = arr[1];
            }
            var userCurrencyCode = new RegionInfo(CultureInfo.GetCultureInfo("AR-AE").Name).ISOCurrencySymbol;

            //if (userCurrencyCode == isoCurrencyCode)
            //{
            //    return currencyAmount.ToString("C", userCulture);
            //}

            if (arr.Length > 1)
            {
                return string.Format(
                 "{0}",

                "-" + Math.Round(Convert.ToDecimal(currencyAmount)).ToString("N2", CultureInfo.GetCultureInfo("AR-AE")).Replace(".00", ""));
            }
            else
            {

                return string.Format(
                    "{0}",

                    Math.Round(Convert.ToDecimal(currencyAmount)).ToString("N2", CultureInfo.GetCultureInfo("AR-AE")).Replace(".00", ""));
            }
        }
        catch (Exception ex)
        {
            return currencyAmount;
        }
    }

    public string getdateformat()
    {
        string returnstr = "";

        returnstr = "dd/MM/yyyy";

        return returnstr;
    }

    public string bindcurdefault()
    {

        string curname = "";
        string strsql111 = "select tbl_Currenciesdetails.*,tbl_Currenciesmaster.Currency,tbl_Currenciesmaster.curName from tbl_Currenciesdetails inner join tbl_Currenciesmaster on tbl_Currenciesdetails.curid=tbl_Currenciesmaster.id where tbl_Currenciesdetails.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and tbl_Currenciesdetails.status=1 and tbl_Currenciesdetails.archive=0 and isdefault=1";
        DataSet ds = da.GetDataSetInline(strsql111);


        if (ds.Tables[0].Rows.Count > 0)
        {
            curname = ds.Tables[0].Rows[0]["cursym"].ToString();
        }

        return curname;
    }

    public string bindcurdefault(string curid)
    {

        string curname = "";
        string strsql111 = "select tbl_Currenciesdetails.*,tbl_Currenciesmaster.Currency,tbl_Currenciesmaster.curName from tbl_Currenciesdetails inner join tbl_Currenciesmaster on tbl_Currenciesdetails.curid=tbl_Currenciesmaster.id where tbl_Currenciesdetails.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and tbl_Currenciesdetails.status=1 and tbl_Currenciesdetails.archive=0 and isdefault=1";

        if (curid != string.Empty)
        {
            strsql111 = "select tbl_Currenciesdetails.*,tbl_Currenciesmaster.Currency,tbl_Currenciesmaster.curName from tbl_Currenciesdetails inner join tbl_Currenciesmaster on tbl_Currenciesdetails.curid=tbl_Currenciesmaster.id where tbl_Currenciesdetails.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and tbl_Currenciesdetails.id='" + curid + "' and tbl_Currenciesdetails.status=1 and tbl_Currenciesdetails.archive=0";

        }
        DataSet ds = da.GetDataSetInline(strsql111);
        if (ds.Tables[0].Rows.Count > 0)
        {
            curname = ds.Tables[0].Rows[0]["cursym"].ToString();
        }

        return curname;
    }


    public string bindcurnameforcsv(string curid)
    {

        string curname = "";
        try
        {
            string strsql111 = "select tbl_Currenciesdetails.*,tbl_Currenciesmaster.Currency,tbl_Currenciesmaster.curName from tbl_Currenciesdetails inner join tbl_Currenciesmaster on tbl_Currenciesdetails.curid=tbl_Currenciesmaster.id where tbl_Currenciesdetails.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and tbl_Currenciesdetails.id='" + curid + "' and tbl_Currenciesdetails.status=1 and tbl_Currenciesdetails.archive=0 and  isdefault=0";


            DataSet ds = da.GetDataSetInline(strsql111);


            if (ds.Tables[0].Rows.Count > 0)
            {
                curname = ds.Tables[0].Rows[0]["Currency"].ToString();
            }
        }
        catch (Exception ex)
        {
        }
        return curname;
    }



    public string bindcurdefaultid(string curid)
    {
        string curname = "";
        string strsql111 = "select tbl_Currenciesmaster.id from tbl_Currenciesdetails inner join tbl_Currenciesmaster on tbl_Currenciesdetails.curid=tbl_Currenciesmaster.id where tbl_Currenciesdetails.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and tbl_Currenciesdetails.status=1 and tbl_Currenciesdetails.archive=0 and isdefault=1";

        if (curid != string.Empty)
        {
            strsql111 = "select tbl_Currenciesmaster.id from tbl_Currenciesdetails inner join tbl_Currenciesmaster on tbl_Currenciesdetails.curid=tbl_Currenciesmaster.id where tbl_Currenciesdetails.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and tbl_Currenciesdetails.id='" + curid + "' and tbl_Currenciesdetails.status=1 and tbl_Currenciesdetails.archive=0";

        }
        DataSet ds = da.GetDataSetInline(strsql111);


        if (ds.Tables[0].Rows.Count > 0)
        {
            curname = ds.Tables[0].Rows[0]["id"].ToString();
        }

        return curname;
    }

    public string getnumbertoworld(string value)
    {
        string strret = "";

        string strsql = "select dbo.fnMoneyToEnglish('" + value + "')";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            strret = ds.Tables[0].Rows[0][0].ToString();
        }


        return strret;
    }

    public string getnumbertoworldusd(string value)
    {
        string strret = "";

        string strsql = "select dbo.fnMoneyToEnglishusd('" + value + "')";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            strret = ds.Tables[0].Rows[0][0].ToString();
        }


        return strret;
    }


    public string getnumbertoworldcur(string value, string curname, string curpaisa)
    {
        string strret = "";


        if (curname == "Rupees")
        {
            string strsql = "select dbo.NUMBERTOWORDS_india('" + value + "')";
            DataSet ds = da.GetDataSetInline(strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                strret = ds.Tables[0].Rows[0][0].ToString();
            }
        }
        else
        {

            string strsql = "select dbo.fnMoneyToEnglishusd_curname('" + value + "','" + curname + "','" + curpaisa + "')";
            DataSet ds = da.GetDataSetInline(strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                strret = ds.Tables[0].Rows[0][0].ToString();
            }

        }
        return strret;
    }

    public void historyinsert(string remarks, string refidstr, string type)
    {

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@cdate", da.getcdatetime());
        prm[1] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[2] = new SqlParameter("@remarks", remarks);
        prm[3] = new SqlParameter("@refid", refidstr);
        prm[4] = new SqlParameter("@type", type);
        prm[5] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
        da.executeDMLQuery("tbl_historydetailsinsert", prm);


    }



    public string getorganitaionaddress(string id)
    {
        string returnstr = "";

        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@regid", id);
        DataSet ds = da.GetDataSet("tbl_companydetails_selectbyregid", prm);
        if (ds.Tables[0].Rows.Count > 0)
        {

            string txtnameofcompany = ds.Tables[0].Rows[0]["txtnameofcompany"].ToString();


            string txtaddress1 = ds.Tables[0].Rows[0]["txtaddress1"].ToString() + " &nbsp;";
            string txtaddress2 = ds.Tables[0].Rows[0]["txtaddress2"].ToString() + " &nbsp;";
            string txtCountry = ds.Tables[0].Rows[0]["txtCountry"].ToString() + " &nbsp;";
            string txtcity = ds.Tables[0].Rows[0]["txtcity"].ToString() + " &nbsp;";
            string txtzipcode = ds.Tables[0].Rows[0]["txtzipcode"].ToString() + " &nbsp;";
            string txtphoneno = ds.Tables[0].Rows[0]["txtphoneno"].ToString() + " &nbsp;";
            string txtfax = ds.Tables[0].Rows[0]["txtfax"].ToString() + " &nbsp;";
            string txtwebsite = ds.Tables[0].Rows[0]["txtwebsite"].ToString() + " &nbsp;";
            string txtemailcontact = ds.Tables[0].Rows[0]["txtemailcontact"].ToString() + " &nbsp;";


            string orgaddressmain = "";
            if (ds.Tables[0].Rows[0]["txtaddress1"].ToString() != string.Empty)
            {
                orgaddressmain = ds.Tables[0].Rows[0]["txtaddress1"].ToString();

            }
            else
            {
                txtaddress1 = "";
            }
            if (ds.Tables[0].Rows[0]["txtaddress2"].ToString() != string.Empty)
            {

                if (orgaddressmain != string.Empty)
                {
                    orgaddressmain += "<br/>" + ds.Tables[0].Rows[0]["txtaddress2"].ToString();
                }
                else
                {
                    orgaddressmain += ds.Tables[0].Rows[0]["txtaddress2"].ToString();
                }
            }
            else
            {
                txtaddress2 = "";
            }
            if (ds.Tables[0].Rows[0]["txtcity"].ToString() != string.Empty)
            {


                if (orgaddressmain != string.Empty)
                {
                    orgaddressmain += "<br/>" + ds.Tables[0].Rows[0]["txtcity"].ToString();
                }
                else
                {
                    orgaddressmain += ds.Tables[0].Rows[0]["txtcity"].ToString();
                }
            }
            else
            {
                txtcity = "";
            }
            if (ds.Tables[0].Rows[0]["txtCountry"].ToString() != string.Empty)
            {

                if (orgaddressmain != string.Empty)
                {
                    orgaddressmain += "<br/>" + ds.Tables[0].Rows[0]["txtCountry"].ToString();
                }
                else
                {
                    orgaddressmain += ds.Tables[0].Rows[0]["txtCountry"].ToString();
                }
            }
            else
            {
                txtCountry = "";
            }
            if (ds.Tables[0].Rows[0]["txtzipcode"].ToString() != string.Empty)
            {

                if (orgaddressmain != string.Empty)
                {
                    orgaddressmain += "<br/>" + ds.Tables[0].Rows[0]["txtzipcode"].ToString();
                }
                else
                {
                    orgaddressmain += ds.Tables[0].Rows[0]["txtzipcode"].ToString();
                }
            }
            else
            {
                txtzipcode = "";
            }
            if (ds.Tables[0].Rows[0]["txtphoneno"].ToString() != string.Empty)
            {


                if (orgaddressmain != string.Empty)
                {
                    orgaddressmain += "<br/>" + ds.Tables[0].Rows[0]["txtphoneno"].ToString();
                }
                else
                {
                    orgaddressmain += ds.Tables[0].Rows[0]["txtphoneno"].ToString();
                }
            }
            else
            {
                txtphoneno = "";
            }


            if (ds.Tables[0].Rows[0]["txtwebsite"].ToString() != string.Empty)
            {

                if (orgaddressmain != string.Empty)
                {
                    orgaddressmain += "<br/>" + ds.Tables[0].Rows[0]["txtwebsite"].ToString();
                }
                else
                {
                    orgaddressmain += ds.Tables[0].Rows[0]["txtwebsite"].ToString();
                }
            }
            else
            {
                txtwebsite = "";
            }
            string orgaddress = orgaddressmain;




            string strsqlpre = "select * from tbl_PreferencesGeneral where regid='" + id + "'";
            DataSet dsprc = da.GetDataSetInline(strsqlpre);
            if (dsprc.Tables[0].Rows.Count > 0)
            {
                if (dsprc.Tables[0].Rows[0]["orgaddressformat"].ToString() != string.Empty)
                {

                    string orgaddresscius = dsprc.Tables[0].Rows[0]["orgaddressformat"].ToString().Replace("{####txtaddress1####}", txtaddress1).Replace("{####txtaddress2####}", txtaddress2).Replace("{####txtcity####}", txtcity).Replace("{####txtCountry####}", txtCountry).Replace("{####txtzipcode####}", txtzipcode).Replace("{####Fax####}", txtfax).Replace("{####txtphoneno####}", txtphoneno).Replace("{####txtemailcontact####}", txtemailcontact).Replace("{####txtwebsite####}", txtwebsite).Replace("\n", "<br/>"); ;
                    orgaddress = orgaddresscius;

                }

            }
            returnstr = orgaddress;
            //  txtnameofcompany.Text = ds.Tables[0].Rows[0]["txtnameofcompany"].ToString();

        }
        return returnstr;

    }

    public string getaddressbybranchid(string id)
    {
        string returnstr = "";

        string strsql = "select * from tbl_locationdetails where id='" + id + "'";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {




            string orgaddressmain = "";
            if (ds.Tables[0].Rows[0]["OfficeAddres"].ToString() != string.Empty)
            {
                orgaddressmain = ds.Tables[0].Rows[0]["OfficeAddres"].ToString().Replace("\n", "<br/>");

            }

            string orgaddress = orgaddressmain;





            returnstr = orgaddress;


        }
        return returnstr;

    }



    public void changeddltext(DropDownList ddl)
    {
        for (int i = 0; i < ddl.Items.Count; i++)
        {
            ddl.Items[i].Text = getltext(ddl.Items[i].Text, "2");
        }
    }
    public void changecdltext(CheckBoxList ddl)
    {
        for (int i = 0; i < ddl.Items.Count; i++)
        {
            ddl.Items[i].Text = getltext(ddl.Items[i].Text, "2");
        }
    }
    public void changerdltext(RadioButtonList ddl)
    {
        for (int i = 0; i < ddl.Items.Count; i++)
        {
            ddl.Items[i].Text = getltext(ddl.Items[i].Text, "2");
        }
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

    public string getltextButton(string stext, string lid)
    {
        string returnstr = stext;
        dataaccesscomm dacom = new dataaccesscomm();
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

                    try
                    {

                        returnstr += " (" + ds.Tables[0].Rows[0]["tsname"].ToString() + ")";


                    }
                    catch
                    {

                    }


                }
            }
        }

        return returnstr;
    }


    public string getltextbyid(string stext, string lid)
    {
        string returnstr = stext;
        dataaccesscomm dacom = new dataaccesscomm();
        SqlParameter[] prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@sname", stext.Trim());

        prm[1] = new SqlParameter("@lid", lid);

        DataSet ds = dacom.GetDataSet("selectstringmastertranslater", prm);
        if (ds.Tables[0].Rows.Count > 0)
        {
            returnstr += " (" + ds.Tables[0].Rows[0]["tsname"].ToString() + ")";

        }


        return returnstr;
    }
    public DataSet getempdetails(string uid)
    {




        string strql = "select * from tbl_employee where id='" + uid + "'";
        DataSet dsemp = da.GetDataSetInline(strql);
        return dsemp;
    }

    public string getempcode()
    {
        int appid = 140000;
        string str = "select serno from srno";
        // DataSet dsss = da.GetDataSetInline(str);
        DataSet dsss = da.GetDataSetInline(str);
        if (dsss.Tables[0].Rows.Count > 0)
        {
            appid = Convert.ToInt32(dsss.Tables[0].Rows[0]["serno"].ToString()) + 1;
        }

        string apptext = appid.ToString();
        //string strupdate = "update srno set serno='" + appid.ToString() + "'";
        //da.executeDMLQueryinline(strupdate);
        return apptext;
    }

    public string getstatename(string id)
    {
        string strreturn = "";
        string strsql = "select * from Imt_state where stateid='" + id + "'";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            strreturn = ds.Tables[0].Rows[0]["state"].ToString();
        }

        return strreturn;

    }

    public string getCountryname(string id)
    {
        string strreturn = "";
        string strsql = "select * from Imt_Country where conid='" + id + "'";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            strreturn = ds.Tables[0].Rows[0]["country"].ToString();
        }

        return strreturn;

    }

    public string getcityname(string id)
    {
        string strreturn = "";
        string strsql = "select * from Imt_city where cityid='" + id + "'";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            strreturn = ds.Tables[0].Rows[0]["city"].ToString();
        }

        return strreturn;

    }

    public DataSet profileinfo(string uid)
    {

        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@userid", uid);
        ds = da.GetDataSet("tbl_user_profile_info", prm);
        return ds;
    }

    public DataSet gettemplate(string uid)
    {
        SqlParameter[] prm = new SqlParameter[1];

        prm[0] = new SqlParameter("@userid", Convert.ToInt32(uid));

        ds = da.GetDataSet("tbl_template_show", prm);
        return ds;
    }

    public DataSet getsignature(string uid)
    {
        SqlParameter[] prm = new SqlParameter[1];

        prm[0] = new SqlParameter("@userid", Convert.ToInt32(uid));

        ds = da.GetDataSet("tbl_signature_show", prm);
        return ds;
    }


    public DataSet getsenderid(string uid)
    {
        SqlParameter[] prm = new SqlParameter[1];

        prm[0] = new SqlParameter("@userid", Convert.ToInt32(uid));

        ds = da.GetDataSet("tbl_sender_show_approved", prm);
        return ds;
    }


    public DataSet getgroup(string uid)
    {
        SqlParameter[] prm = new SqlParameter[1];

        prm[0] = new SqlParameter("@userid", Convert.ToInt32(uid));

        ds = da.GetDataSet("tbl_group_show", prm);
        return ds;
    }


    public DataSet getusers(string uid)
    {
        SqlParameter[] prm = new SqlParameter[1];

        prm[0] = new SqlParameter("@userid", Convert.ToInt32(uid));

        ds = da.GetDataSet("tbl_user_showactive", prm);
        return ds;
    }
    public string getlabourname(string lid)
    {
        string strreturn = "";
        string strsql = "select f_name from tbl_Labours where id='" + lid + "'";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            strreturn = ds.Tables[0].Rows[0]["f_name"].ToString();
        }


        return strreturn;
    }

    public string getContractorname(string Cid)
    {
        string strreturn = "";
        string strsql = "select (First_Name+' '+Last_Name)as Name from tbl_Contactor  where id='" + Cid + "'";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            strreturn = ds.Tables[0].Rows[0]["Name"].ToString();
        }


        return strreturn;
    }


    public string getusersname(string uid)
    {
        string strreturn = "";
        string strsql = "select f_name,l_name from tbl_employee where id='" + uid + "'";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            strreturn = ds.Tables[0].Rows[0]["f_name"].ToString() + " " + ds.Tables[0].Rows[0]["l_name"].ToString();
        }


        return strreturn;
    }


    public string getusersemail(string uid)
    {
        string strreturn = "";
        string strsql = "select email_Id from tbl_employee where id='" + uid + "'";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            strreturn = ds.Tables[0].Rows[0]["email_Id"].ToString();
        }


        return strreturn;
    }
    public string getusersopenamount(string uid)
    {
        string strreturn = "0";

        SqlParameter[] prm = new SqlParameter[1];

        prm[0] = new SqlParameter("@userid", Convert.ToInt32(uid));

        ds = da.GetDataSet("tbl_user_show_byid", prm);

        if (ds.Tables[0].Rows.Count > 0)
        {
            strreturn = ds.Tables[0].Rows[0]["sms_credit"].ToString();
        }
        return strreturn;
    }

    public string gettemptext(string tid)
    {
        string strreturn = "0";

        SqlParameter[] prm = new SqlParameter[1];

        prm[0] = new SqlParameter("@id", tid);

        ds = da.GetDataSet("tbl_templateselect", prm);

        if (ds.Tables[0].Rows.Count > 0)
        {
            strreturn = ds.Tables[0].Rows[0]["template_text"].ToString();
        }
        return strreturn;
    }

    public bool checkBalance(int sms)
    {
        string uid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();

        DataSet dsp = profileinfo(uid);

        if (dsp.Tables[0].Rows.Count > 0)
        {
            if (Convert.ToInt32(dsp.Tables[0].Rows[0]["sms_credit"].ToString()) >= sms)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;

    }

    public bool checkBalance(int sms, string uid)
    {


        DataSet dsp = profileinfo(uid);

        if (dsp.Tables[0].Rows.Count > 0)
        {
            if (Convert.ToInt32(dsp.Tables[0].Rows[0]["sms_credit"].ToString()) >= sms)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;

    }

    public string getsignaturetext(string tid)
    {
        string strreturn = "0";

        SqlParameter[] prm = new SqlParameter[1];

        prm[0] = new SqlParameter("@id", tid);

        ds = da.GetDataSet("tbl_signatureselect", prm);

        if (ds.Tables[0].Rows.Count > 0)
        {
            strreturn = ds.Tables[0].Rows[0]["signature_text"].ToString();
        }
        return strreturn;
    }

    public string[] GetDistinctValues(string[] arr)
    {
        List<string> lst = new List<string>();
        for (int i = 0; i < arr.Length; i++)
        {
            //if (arr[i] != string.Empty)
            //{
            if (lst.Contains(arr[i]))
                continue;
            lst.Add(arr[i]);
            // }
        }
        return lst.ToArray();
    }

    public string[] GetremoveblankValues(string[] arr)
    {
        List<string> lst = new List<string>();
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] != string.Empty)
            {
                //if (lst.Contains(arr[i]))
                //    continue;
                lst.Add(arr[i]);
            }
        }
        return lst.ToArray();
    }


    public string mno(string mn)
    {

        string mobilenamber = "";
        if (mn.Length > 0)
        {

            string mobno = cmf.removeInValidCharinmobileno(mn);
            //mobilenamber = "";
            if (mobno.Length == 10)
            {
                mobilenamber = mobno;


            }
            else if (mobno.Length == 12)
            {
                //Response.Write(mobno + "<br/>");
                string mon = mobno.Substring(2, 10);

                mobilenamber = mon;

                //mobilenamber = mobno;
            }
            else if (mobno.Length == 11)
            {
                string mon = mobno.Substring(1, 10);




                mobilenamber = mobno;

            }
            else if (mobno.Length > 12)
            {
                string mon = mobno.Substring(0, 10);

                mobilenamber = mon;

            }

        }
        return mobilenamber;
    }


    public int gettotalnofromgroup(string gid)
    {
        int strreturn = 0;

        SqlParameter[] prm = new SqlParameter[1];

        prm[0] = new SqlParameter("@id", gid);

        ds = da.GetDataSet("tbl_contactselectnoofcontact", prm);

        if (ds.Tables[0].Rows.Count > 0)
        {
            strreturn = Convert.ToInt32(ds.Tables[0].Rows[0]["tno"].ToString());
        }
        return strreturn;
    }

    public int gettotalnofromcsv(string gid)
    {
        int strreturn = 0;

        SqlParameter[] prm = new SqlParameter[1];

        prm[0] = new SqlParameter("@id", gid);

        ds = da.GetDataSet("tbl_contactselectnoofcontactcsv", prm);

        if (ds.Tables[0].Rows.Count > 0)
        {
            strreturn = Convert.ToInt32(ds.Tables[0].Rows[0]["tno"].ToString());
        }
        return strreturn;
    }
    public string[] getmobilenofromgroup(string gid)
    {
        List<string> lst = new List<string>();

        return lst.ToArray();
    }
}