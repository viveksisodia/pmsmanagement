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
using System.Data.SqlClient;
using CommonClass;
/// <summary>
/// Summary description for opprice
/// </summary>
public class opprice
{
    DataAccess da = new DataAccess();
    DataSet ds = new DataSet();

    CommonFunction cmf = new CommonFunction();
    SqlParameter[] prm;
	public opprice()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    private DropDownList _ddl;
    private DataSet _ds1;
    public DataSet ds1
    {
        get { return _ds1; }
        set { _ds1 = value; }
    }
    private int _cashid;
    public int Cashid
    {
        get { return _cashid; }
        set { _cashid=value;}
    }
    private string _vehtype=string.Empty;
    private string _vehno=string.Empty;
    private string _amt=string.Empty;
    private string _trip=string.Empty;
    private string _veh=string.Empty;
    private DateTime? _currdate = null;
    private DateTime? _recdate = null;
    private string _name = string.Empty;
    private string _ph = string.Empty;
    private string _bill = string.Empty;
    private string _recdamt = string.Empty;
    private string _remainamt = string.Empty;
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }
    public string Phone
    {
        get { return _ph; }
        set { _ph = value; }
    }
    public string Bill
    {
        get { return _bill; }
        set { _bill = value; }
    }

    private string _amount = string.Empty;
    public string Amount
    {
        get{return _amount;}
        set{_amount=value;}
    }
    public string Recdamt
    {
        get { return _recdamt; }
        set { _recdamt = value; }
    }
    public string RemainAmount
    {
        get { return _remainamt; }
        set { _remainamt = value; }
    }

    private string _paymode = string.Empty;
    public string Paymode
    {
        get { return _paymode; }
        set { _paymode = value; }
    }

    private string _chqbnkname = string.Empty;
        public string Chqbnkname
        {
            get{return _chqbnkname;}
            set{_chqbnkname=value;}
        }


        private string _chqno = string.Empty;
        public string Chqno
        {
            get { return _chqno; }
            set { _chqno = value; }
        }

        private string _remarks = string.Empty;
        public string Remarks
        {
            get { return _remarks; }
            set { _remarks = value; }
        }

        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private int _paystatus;
        public int Paystatus
        {
            get { return _paystatus; }
            set { _paystatus = value; }
        }

        private string _paystatuschr;
        public string Paystatuschr
        {
            get { return _paystatuschr; }
            set { _paystatuschr = value; }
        }


    public DropDownList Ddl
    {
        get { return _ddl; }
        set { _ddl = value; }
    }
    public string Vehtype
    {
        get { return _vehtype; }
        set { _vehtype = value; }
    }
    public string Vehno
    {
        get { return _vehno; }
        set { _vehno = value; }
    }
    public string Amt
    {
        get { return _amt; }
        set { _amt = value; }
    }
    public string Trip
    {
        get { return _trip; }
        set { _trip = value; }
    }
    private DataSet _ds;
    public DataSet Ds
    {
        get { return _ds; }
        set { _ds = value; }
    }
    private int _vid;
    public int Vid
    {
        get { return _vid; }
        set { _vid = value; }
    }
    public string Veh
    {
        get { return _veh; }
        set { _veh = value; }
    }
    public DateTime? Currdate
    {
        get { return _currdate; }
        set { _currdate = value; }
    }
    public DateTime? Recdate
    {
        get { return _recdate; }
        set { _recdate = value; }
    }

    public void bindveh()
    {
        ds = da.GetDataSet("Adarsh_vehicleinfo");
        cmf.BindDropDownList(_ddl, ds, "vichlename", "vichlename");
        ListItem li = new ListItem("--SelectVehicle--", "0");
        _ddl.Items.Insert(0, li);
    }
    public void bindvehserh()
    {
        ds = da.GetDataSet("Adarsh_vehicleinfo");
        cmf.BindDropDownList(_ddl, ds, "vichlename", "vichlename");
        ListItem li = new ListItem("--SelectVehicle--", "0");
        _ddl.Items.Insert(0, li);
    }
    public void bindtrip()
    {
        ds = da.GetDataSet("Adarsh_tripinfo");
        cmf.BindDropDownList(_ddl, ds, "trip_name", "trip_name");
        ListItem li = new ListItem("--SelectTrip--", "0");
        _ddl.Items.Insert(0, li);
    }
    public void Binddata()
    {

        _ds = da.GetDataSet("Adarsh_selectoppricelist");
    }

    public void binddebitcash()
    {
        ds1 = da.GetDataSet("Adarsh_dailycashdebit_select");
    }

    public void bindcreditcash()
    {
        ds1 = da.GetDataSet("Adarsh_dailycashcredit_select");
    }

    public void debitcashmemoinsert()
    {
        prm=new SqlParameter[7];
        if (_currdate != null)
            prm[0] = new SqlParameter("@date", _currdate);
        else
            prm[0] = new SqlParameter("@date", DBNull.Value);
        prm[1] = new SqlParameter("@name",_name);
        prm[2] = new SqlParameter("@amount",_amount);
        prm[3] = new SqlParameter("@paymode",_paymode);
       
        prm[4] = new SqlParameter("@chqbnkname",_chqbnkname);
        prm[5] = new SqlParameter("@chqno",_chqno);
        prm[6] = new SqlParameter("@remarks",_remarks);
        da.executeDMLQuery("Adarsh_dailycash_insert", prm);

    }
    public void creditcashmemoinsert()
    {
        prm = new SqlParameter[7];
        if (_currdate != null)
            prm[0] = new SqlParameter("@date", _currdate);
        else
            prm[0] = new SqlParameter("@date", DBNull.Value);
        prm[1] = new SqlParameter("@name", _name);
        prm[2] = new SqlParameter("@amount", _amount);
        prm[3] = new SqlParameter("@paymode", _paymode);

        prm[4] = new SqlParameter("@chqbnkname", _chqbnkname);
        prm[5] = new SqlParameter("@chqno", _chqno);
        prm[6] = new SqlParameter("@remarks", _remarks);
        da.executeDMLQuery("Adarsh_dailycreditcash_insert", prm);

    }

    public void debitcashmemoupdate()
    {
        prm = new SqlParameter[8];
        if (_currdate != null)
            prm[0] = new SqlParameter("@date", _currdate);
        else
            prm[0] = new SqlParameter("@date", DBNull.Value);
        prm[1] = new SqlParameter("@name", _name);
        prm[2] = new SqlParameter("@amount", _amount);
        prm[3] = new SqlParameter("@paymode", _paymode);
        prm[4] = new SqlParameter("@chqbnkname", _chqbnkname);
        prm[5] = new SqlParameter("@chqno", _chqno);
        prm[6] = new SqlParameter("@remarks", _remarks);
        prm[7] = new SqlParameter("@id",_cashid);
        da.executeDMLQuery("Adarsh_dailycash_update", prm);

    }
    public void creditcashmemoupdate()
    {
        prm = new SqlParameter[8];
        if (_currdate != null)
            prm[0] = new SqlParameter("@date", _currdate);
        else
            prm[0] = new SqlParameter("@date", DBNull.Value);
        prm[1] = new SqlParameter("@name", _name);
        prm[2] = new SqlParameter("@amount", _amount);
        prm[3] = new SqlParameter("@paymode", _paymode);
        prm[4] = new SqlParameter("@chqbnkname", _chqbnkname);
        prm[5] = new SqlParameter("@chqno", _chqno);
        prm[6] = new SqlParameter("@remarks", _remarks);
        prm[7] = new SqlParameter("@id", _cashid);
        da.executeDMLQuery("Adarsh_dailycreditcash_update", prm);

    }
    public void deletedebitcash()
    {

        prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@id", _cashid);
        da.executeDMLQuery("Adarsh_dailycashmemo_debit_delete", prm);

    }
    public void deletecreditcash()
    {

        prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@id", _cashid);
        da.executeDMLQuery("Adarsh_dailycashmemo_credit_delete", prm);

    }

    public void insertbusser()
    {
        prm = new SqlParameter[5];
        if (_currdate != null)
            prm[0] = new SqlParameter("@date", _currdate);
        else
            prm[0] = new SqlParameter("@date", DBNull.Value);
        prm[1] = new SqlParameter("@vehtype", _vehtype);
        prm[2] = new SqlParameter("@vehno", _vehno);
        prm[3] = new SqlParameter("@trip", _trip);
        prm[4] = new SqlParameter("@amt", _amt);

        da.executeDMLQuery("Adarsh_oppricelist_inserts", prm);

    }

    public void Bindname()
    {
        ds = da.GetDataSet("Adarsh_dailycashdebit_select");
        cmf.BindDropDownList(_ddl, ds, "vichlename", "vichlename");
        ListItem li = new ListItem("--SelectVehicle--", "0");
        _ddl.Items.Insert(0, li);
    }

    public void deletepass()
    {

        prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@id", _vid);
        da.executeDMLQuery("Adarsh_opprice_delete", prm);

    }

    public void busupdate()
    {

        prm = new SqlParameter[6];
        if (_currdate != null)
        {
            prm[0] = new SqlParameter("@date", _currdate);
        }
        else
        {
            prm[0] = new SqlParameter("@date", DBNull.Value);

        }
        prm[1] = new SqlParameter("@vehtype", _vehtype);
        prm[2] = new SqlParameter("@vehno", _vehno);
        prm[3] = new SqlParameter("@trip", _trip);
        prm[4] = new SqlParameter("@amt", _amt);
        prm[5] = new SqlParameter("@id", _vid);

        ds = da.GetDataSet("Adarsh_oppriceupdate", prm);

    }

    public void billdetailinsert()
    {
        prm = new SqlParameter[14];
        if (_currdate != null)
            prm[0] = new SqlParameter("@date", _currdate);
        else
            prm[0] = new SqlParameter("@date", DBNull.Value);
        prm[1] = new SqlParameter("@name", _name);
        prm[2] = new SqlParameter("@totlamnt", _amount);
        prm[3] = new SqlParameter("@phone", _ph);
        prm[4] = new SqlParameter("@billno", _bill);
        if (_recdate != null)
            prm[5] = new SqlParameter("@recdate", _recdate);
        else
            prm[5] = new SqlParameter("@recdate", DBNull.Value);
        prm[6] = new SqlParameter("@paymntmode", _paymode);

        prm[7] = new SqlParameter("@chqbnkname", _chqbnkname);
        prm[8] = new SqlParameter("@chqno", _chqno);
        prm[9] = new SqlParameter("@recvdamnt", _recdamt);
        prm[10] = new SqlParameter("@balance", _remainamt);
        prm[11] = new SqlParameter("@remarks", _remarks);
        prm[12] = new SqlParameter("@paystatus", _paystatus);
        prm[13] = new SqlParameter("@paystatuschr", _paystatuschr);
        da.executeDMLQuery("Adarsh_bill_insert", prm);

    }
    public void billdetaildelete()
    {

        prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@id", _cashid);
        da.executeDMLQuery("Adarsh_bill_delete", prm);

    }

    public void billdetailupdate()
    {
        prm = new SqlParameter[15];
        if (_currdate != null)
            prm[0] = new SqlParameter("@date", _currdate);
        else
            prm[0] = new SqlParameter("@date", DBNull.Value);
        prm[1] = new SqlParameter("@name", _name);
        prm[2] = new SqlParameter("@totlamnt", _amount);
        prm[3] = new SqlParameter("@phone", _ph);
        prm[4] = new SqlParameter("@billno", _bill);
        if (_recdate != null)
            prm[5] = new SqlParameter("@recdate", _recdate);
        else
            prm[5] = new SqlParameter("@recdate", DBNull.Value);
        prm[6] = new SqlParameter("@paymntmode", _paymode);

        prm[7] = new SqlParameter("@chqbnkname", _chqbnkname);
        prm[8] = new SqlParameter("@chqno", _chqno);
        prm[9] = new SqlParameter("@recvdamnt", _recdamt);
        prm[10] = new SqlParameter("@balance", _remainamt);
        prm[11] = new SqlParameter("@remarks", _remarks);
        prm[12] = new SqlParameter("@id", _cashid);
        prm[13] = new SqlParameter("@paystatus",_paystatus);
        prm[14] = new SqlParameter("@paystatuschr",_paystatuschr);
        da.executeDMLQuery("Adarsh_bill_update", prm);

    }

    protected void bindgriddata()
    {
        //da = new DataAccess();

        //SqlParameter[] prm = new SqlParameter[2];
        //if ((fdate.CalendarDate) != string.Empty)
        //    prm[0] = new SqlParameter("@fromdate", Convert.ToDateTime(cmf.dateConvert(fdate.CalendarDate.ToString())));
        //  Response.Write(Convert.ToDateTime(cmf.dateConvert(fromdate.CalendarDate.ToString())));
        //else
        //    prm[0] = new SqlParameter("@fromdate", Convert.ToDateTime(cmf.dateConvert("01/01/1950")));
        //if ((tdate.CalendarDate) != string.Empty)
        //    prm[1] = new SqlParameter("@todate", Convert.ToDateTime(cmf.dateConvert(tdate.CalendarDate.ToString())));
        // Response.Write(Convert.ToDateTime(cmf.dateConvert(todate.CalendarDate.ToString())));
        //else
        //    prm[1] = new SqlParameter("@todate", Convert.ToDateTime(cmf.dateConvert("01/01/2025")));
        //ds = da.GetDataSet("Adarsh_bill_selectbydate", prm);
        //grdvendor.DataSource = ds;
        //grdvendor.DataBind();

    }
}
