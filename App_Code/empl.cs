
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
/// Summary description for subject
/// </summary>
public class empl
{
	public empl()
	{
		//
		// TODO: Add constructor logic here
		//
    }
    SqlParameter[] prm;
    DataAccess da;
    CommonFunction cmf;
    #region Data Member
    private int _id; private int _nid; private int _qid;
    private int _bid;
    private int _cid; private int _sid; private int _eid;
    private int _kid; private int _jid; private int _did;
    public int Sid
    {
        get { return _sid; }
        set { _sid = value; }
    }
    public int Qid
    {
        get { return _qid; }
        set { _qid = value; }
    }
    public int Bid
    {
        get { return _bid; }
        set { _bid = value; }
    }
    private int _pid; private int _rid;
    private string _name = string.Empty;
    private string _log = string.Empty;
    private string _pwd = string.Empty;
    private string _useremail = string.Empty;
    private DateTime? _ArrivalTime;
    private DateTime? _DepartureTime;
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }
    public string Useremail
    {
        get { return _useremail; }
        set { _useremail = value; }
    }
    public string Log
    {
        get { return _log; }
        set { _log = value; }
    }
    public string Pwd
    {
        get { return _pwd; }
        set { _pwd = value; }
    }
    private string _fname = string.Empty;

    public string Fname
    {
        get { return _fname; }
        set { _fname = value; }
    }
    private string _date = string.Empty;

    public string Date
    {
        get { return _date; }
        set { _date = value; }
    } private string _roll = string.Empty;

    public string Roll
    {
        get { return _roll; }
        set { _roll = value; }
    }
    public DateTime? ArrivalTime
    {
        get { return _ArrivalTime; }
        set { _ArrivalTime = value; }
    }
    public DateTime? DepartureTime
    {
        get { return _DepartureTime; }
        set { _DepartureTime = value; }
    }
    private string _gender = string.Empty;

    public string Gender
    {
        get { return _gender; }
        set { _gender = value; }
    }
    private string _mob = string.Empty;

    public string Mob
    {
        get { return _mob; }
        set { _mob = value; }
    }
    private string _marital = string.Empty;

    public string Marital
    {
        get { return _marital; }
        set { _marital = value; }
    }
    private string _tadd = string.Empty;

    public string Tadd
    {
        get { return _tadd; }
        set { _tadd = value; }
    }
    private string _padd = string.Empty;

    public string Padd
    {
        get { return _padd; }
        set { _padd = value; }
    }
    private string _quali = string.Empty;

    public string Quali
    {
        get { return _quali; }
        set { _quali = value; }
    }
    private string _year = string.Empty;

    public string Year
    {
        get { return _year; }
        set { _year = value; }
    }
    private string _uni = string.Empty;

    public string Uni
    {
        get { return _uni; }
        set { _uni = value; }
    }
    private string _other = string.Empty;

    public string Other
    {
        get { return _other; }
        set { _other = value; }
    }
    private string _dept = string.Empty;

    public string Dept
    {
        get { return _dept; }
        set { _dept = value; }
    }
    private string _des = string.Empty;

    public string Des
    {
        get { return _des; }
        set { _des = value; }
    }
    private string _sal = string.Empty;

    public string Sal
    {
        get { return _sal; }
        set { _sal = value; }
    }
    private string _date11 = string.Empty;

    public string Date11
    {
        get { return _date11; }
        set { _date11 = value; }
    }
    private string _poor = string.Empty;

    public string Poor
    {
        get { return _poor; }
        set { _poor= value; }
    }
    private string _poorrem = string.Empty;

    public string Poorrem
    {
        get { return _poorrem; }
        set { _poorrem = value; }
    }
    private string _tu = string.Empty;

    public string Tu
    {
        get { return _tu; }
        set { _tu = value; }
    }
    private string _turem = string.Empty;

    public string Turem
    {
        get { return _turem; }
        set { _turem = value; }

    }
    private string _conv = string.Empty;

    public string Conv
    {
        get { return _conv; }
        set { _conv = value; }
    }
    private string _convrem = string.Empty;

    public string Convrem
    {
        get { return _convrem; }
        set { _convrem = value; }
    } 
    private string _hos= string.Empty;

    public string hos
    {
        get { return _hos; }
        set { _hos = value; }
    }
    private string _hosvrem = string.Empty;

    public string Hosvrem
    {
        get { return _hosvrem; }
        set { _hosvrem = value; }
    } 
    private string _fine = string.Empty;

    public string Fine
    {
        get { return _fine; }
        set { _fine = value; }
    } 
    private string _finerem = string.Empty;

    public string Finerem
    {
        get { return _finerem; }
        set { _finerem = value; }
    } 
    private string _other1 = string.Empty;

    public string Other1
    {
        get { return _other1; }
        set { _other1 = value; }
    } 
    private string _otherem = string.Empty;

    public string Otherem
    {
        get { return _otherem; }
        set { _otherem = value; }
    }
    private string _total = string.Empty;

    public string Total
    {
        get { return _total; }
        set { _total = value; }
    }
    private string _totalrem = string.Empty;

    public string Totalrem
    {
        get { return _totalrem; }
        set { _totalrem = value; }
    } 
    private string _amtinwords= string.Empty;

    public string Amtinwords
    {
        get { return _amtinwords; }
        set { _amtinwords = value; }
    }
    private DataSet _ds;
    private DropDownList _ddl;
    #endregion
    #region Property
    private CheckBoxList _chk;
    public CheckBoxList chk
    {
        get { return _chk; }
        set { _chk = value; }
    }
    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }
    public int Nid
    {
        get { return _nid; }
        set { _nid = value; }
    }
    public int Eid
    {
        get { return _eid; }
        set { _eid = value; }
    }
    public int Cid
    {
        get { return _cid; }
        set { _cid = value; }
    }
    public int Rid
    {
        get { return _rid; }
        set { _rid = value; }
    }
    public int Kid
    {
        get { return _kid; }
        set { _kid = value; }
    }
    public int Jid
    {
        get { return _jid; }
        set { _jid = value; }
    }
    public int Did
    {
        get { return _did; }
        set { _did = value; }
    }
    public int Pid
    {
        get { return _pid; }
        set { _pid = value; }
    }
   
    public DropDownList Ddl
    {
        get { return _ddl; }
        set { _ddl = value; }
    }
  
   
    public DataSet ds
    {
        get { return _ds; }
        set { _ds = value; }
    }


    private int _secid;
    public int Secid
    {
        get { return _secid; }
        set { _secid = value; }
    }


    #endregion
    private string _passwd = string.Empty;
    private int _chid;
    private string _module = string.Empty;
    private string _url = string.Empty;
    public string Passwd
    {
        get { return _passwd; }
        set { _passwd = value; }
    }
    public int Chid
    {
        get { return _chid; }
        set { _chid = value; }
    }
    
    public string Module
    {
        get { return _module; }
        set { _module = value; }
    }
   
    
    public string Url
    {
        get { return _url; }
        set { _url = value; }
    }
    public void Binduser()
    {
        da = new DataAccess();
        _ds = da.GetDataSet("Adarsh_userlogin_select");

    }
    public void Bindddl()
    {
        ListItem li = new ListItem("--Select--", "0");
        _ddl.Items.Insert(0, li);

    }
    public void BindData()
    {
        da = new DataAccess();
        _ds = da.GetDataSet("sms_emp_select");

    }
    public void BindDep()
    {
        da = new DataAccess();
        _ds = da.GetDataSet("sms_dept_select");

    }
    public void Bindtime()
    {
        da = new DataAccess();
        _ds = da.GetDataSet("SMS_time_select");

    }
    public void Bindfeess()
    {
        da = new DataAccess();
        _ds = da.GetDataSet("sms_fees_grid");

    }
    public void selectfeesdata()
    {
        da = new DataAccess();
        prm = new SqlParameter[3];
       
        prm[0] = new SqlParameter("@id", _id);
       
           prm[1] = new SqlParameter("@month", _cid);
       
       
        prm[2] = new SqlParameter("@idn",_did);
        
        //prm[3] = new SqlParameter("@batch", _eid);
    

       _ds= da.GetDataSet("sms_select_feesshow", prm);

    }
    public void selectsubject()
    {
        da = new DataAccess();
       DataSet Ds = new DataSet();
        cmf = new CommonFunction();
        prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@cid",_id);
        _ds = da.GetDataSet("sms_classwisesubassigned_select", prm);
        cmf.BindcheckboxList(_chk, _ds, "s_name", "id");
      }
    public void Bindfreeteacher()
    {

        da = new DataAccess();
        cmf = new CommonFunction();
        prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@timeid",_id);
        prm[1] = new SqlParameter("@day",_pid);
        ds = da.GetDataSet("sms_freeteacher_select",prm);
        cmf.BindDropDownList(_ddl, _ds, "firstname", "id");
        ListItem li = new ListItem("--Select--", "0");
        _ddl.Items.Insert(0, li);
    }
    //public void Bindsubjectteacher()
    //{
    //   da = new DataAccess();
    //    cmf = new CommonFunction();
    //    prm = new SqlParameter[5];
    //    prm[0] = new SqlParameter("@timeid", _id);
    //    prm[1] = new SqlParameter("@id", _rid);
    //    prm[2] = new SqlParameter("@day",_pid);
    //    prm[3] = new SqlParameter("bid", _bid);
    //    prm[4] = new SqlParameter("@secid", _cid);
    //    prm[3] = new SqlParameter("bid", _bid);
    //    prm[4] = new SqlParameter("@secid", _cid);
    //    ds = da.GetDataSet("sms_freesubject_select", prm);
    //    cmf.BindDropDownList(_ddl, _ds, "subname", "id");
    //    ListItem li = new ListItem("--Select--", "0");
    //    _ddl.Items.Insert(0, li);
    //}



    public void Bindclasstfree()
    {

        da = new DataAccess();
        cmf = new CommonFunction();
        prm = new SqlParameter[3];
        prm[0] = new SqlParameter("@timeid", _id);
      
        prm[1] = new SqlParameter("@day",_rid);
       prm[2] = new SqlParameter("@bid", _cid);
        ds = da.GetDataSet("sms_freeclass_select", prm);
        cmf.BindDropDownList(_ddl, _ds, "class", "id");
        ListItem li = new ListItem("--Select--", "0");
        _ddl.Items.Insert(0, li);
    }
    public void inserttime()
    {
        da = new DataAccess();
        prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@start",_name);
        prm[1] = new SqlParameter("@over",_fname);
        da.executeDMLQuery("SMS_time_insert", prm);
    }
    public int insertfeessub()
    {
        da = new DataAccess();
        prm = new SqlParameter[10];
        prm[0] = new SqlParameter("@classid",_sid);
        prm[1] = new SqlParameter("@sid ",_jid);
        prm[2] = new SqlParameter("@totalfees",_kid);
        prm[3] = new SqlParameter("@month",_nid);
        prm[4] = new SqlParameter("@remain",_pid);
        prm[5] = new SqlParameter("@advance",_rid);
        prm[6] = new SqlParameter("@cid", _qid);
        prm[7] = new SqlParameter("@batch", _cid);
        prm[8] = new SqlParameter("@secid",_secid);
        prm[9] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[9].Direction = ParameterDirection.ReturnValue;
        int id= da.executeDMLQuery("sms_Fees_insert", prm);
        return id;
    }
    public void updatetime()
    {
        da = new DataAccess();
        prm = new SqlParameter[3];
        prm[0] = new SqlParameter("@id", _id);
        prm[1] = new SqlParameter("@start", _ArrivalTime);
        prm[2] = new SqlParameter("@over", _DepartureTime);
        da.executeDMLQuery("SMS_time_update", prm);
    }
    public void dateselect()
    {
        da = new DataAccess();
        prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@id", _id);
       
        da.executeDMLQuery("dps_daydata", prm);
    }
    public void deletetime()
    {
        da = new DataAccess();
        prm = new SqlParameter[1];

        prm[0] = new SqlParameter("@id", _id);
        da.executeDMLQuery("SMS_time_delete", prm);
    }
    public void dselectnanme()
    {
        da = new DataAccess();
        prm = new SqlParameter[1];

        prm[0] = new SqlParameter("@id",_pid);
        ds = da.GetDataSet("sms_name_select ", prm);
    }
    public void selectrollno()
    {
         da = new DataAccess();
        cmf = new CommonFunction();
       
        prm = new SqlParameter[1];

        prm[0] = new SqlParameter("@id", _id);
        ds = da.GetDataSet("sms_rollno_select", prm);
     
        cmf.BindDropDownList(_ddl, _ds, "rno", "id");
        ListItem li = new ListItem("--Select--", "0");
        _ddl.Items.Insert(0, li);
      
    }
    public void insertdep()
    {
        da = new DataAccess();
        prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@dep", Name);
        da.executeDMLQuery("sms_dept_insert", prm);
    }
    public void updatedep()
    {
        da = new DataAccess();
        prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@dep", _name);
        prm[1] = new SqlParameter("@id", _id);
        da.executeDMLQuery("sms_dept_update", prm);
    }
    public void deletedep()
    {
        da = new DataAccess();
        prm = new SqlParameter[1];
      
        prm[0] = new SqlParameter("@id", _id);
        da.executeDMLQuery("sms_dept_delete", prm);
    }
    public void insertdetails()
    {
        da = new DataAccess();
        prm = new SqlParameter[34];
        prm[0] = new SqlParameter("@form", _id);
        prm[1] = new SqlParameter("@rgno", _pid);
        prm[2] = new SqlParameter("@img",_name);
        prm[3] = new SqlParameter("@fname", _gender);
        prm[4] = new SqlParameter("@mname", _mob);
        prm[5] = new SqlParameter("@name", _marital);
        prm[6] = new SqlParameter("@dob", _ArrivalTime);
        prm[7] = new SqlParameter("@sex", _padd);
        prm[8] = new SqlParameter("@byear", _quali);
        prm[9] = new SqlParameter("@bmonth", _year);
        prm[10] = new SqlParameter("@bday", _date);
        prm[11] = new SqlParameter("@class", _other);
        prm[12] = new SqlParameter("@psch", _dept);
        prm[13] = new SqlParameter("@pclass", _des);
        prm[14] = new SqlParameter("@nationality", _sal);
        prm[15] = new SqlParameter("@tongue", _date11);
        prm[16] = new SqlParameter("@fqauli", _poor);
        prm[17] = new SqlParameter("@mquali", _poorrem);
        prm[18] = new SqlParameter("@fpro", _tu);
        prm[19] = new SqlParameter("@mpro", _turem);
        prm[20] = new SqlParameter("@fadd", _conv);
        prm[21] = new SqlParameter("@madd", _convrem);
        prm[22] = new SqlParameter("@fph", _hos);
        prm[23] = new SqlParameter("@mph", _hosvrem);
        prm[24] = new SqlParameter("@foff", _fine);
        prm[25] = new SqlParameter("@moff", _finerem);
        prm[26] = new SqlParameter("@nochild", _kid);
        prm[27] = new SqlParameter("@date", _DepartureTime);
        prm[28] = new SqlParameter("@roll",_roll);
        prm[29] = new SqlParameter("@cid", _cid);
        prm[30] = new SqlParameter("@sid", _did);
        prm[31] = new SqlParameter("@batch", _eid);
        prm[32] = new SqlParameter("@log ", _log);
        prm[33] = new SqlParameter("@pwd", _pwd);
       
        da.executeDMLQuery("sms_detaiuls", prm);
      

    }
    public void Bindrollno()
    {
        da = new DataAccess();
        cmf = new CommonFunction();
        _ds = da.GetDataSet("Sms_roll_pr");
        cmf.BindDropDownList(_ddl, _ds, "rollno", "id");
        ListItem li = new ListItem("--Select--", "0");
        _ddl.Items.Insert(0, li);

    }
    public void Bindyear()
    {
        da = new DataAccess();
        cmf = new CommonFunction();
        _ds = da.GetDataSet("sms_batch_select");
        cmf.BindDropDownList(_ddl, _ds, "batchname", "id");
        ListItem li = new ListItem("--Select--", "0");
        _ddl.Items.Insert(0, li);

    }
    public void insertemployee()
    {
        da = new DataAccess();
        prm = new SqlParameter[16];
        prm[0] = new SqlParameter("@name", _name);
        prm[1] = new SqlParameter("@fname", _fname);
        prm[2] = new SqlParameter("@dateofbirth", _date);
        prm[3] = new SqlParameter("@gender", _gender);
        prm[4] = new SqlParameter("@mob", _mob);
        prm[5] = new SqlParameter("@marital", _marital);
        prm[6] = new SqlParameter("@tadd", _tadd);
        prm[7] = new SqlParameter("@padd", _padd);
        prm[8] = new SqlParameter("@quali", _quali);
        prm[9] = new SqlParameter("@year", _year);
        prm[10] = new SqlParameter("@universe", _uni);
        prm[11] = new SqlParameter("@other", _other);
        prm[12] = new SqlParameter("@depart", _dept);
        prm[13] = new SqlParameter("@desig", _des);
        prm[14] = new SqlParameter("@sal ", _sal);
        prm[15] = new SqlParameter("@hiring", _date11);
       

        da.executeDMLQuery("sms_emp_insert", prm);

    }
   
    public void inserthistory()
    {
        da = new DataAccess();
        prm = new SqlParameter[3];
        prm[0] = new SqlParameter("@sid", _id);
        prm[1] = new SqlParameter("@cid", _cid);
        prm[2] = new SqlParameter("@bid", _eid);

        da.executeDMLQuery("sms_insert_history", prm);

    }
    public void insertcalendar()
    {
        da = new DataAccess();
        prm = new SqlParameter[9];
        prm[0] = new SqlParameter("@ses", _sid);
        prm[1] = new SqlParameter("@month", _id);
        prm[2] = new SqlParameter("@days", _rid);
        prm[3] = new SqlParameter("@sun", _pid);
        prm[4] = new SqlParameter("@vac", _cid);
        prm[5] = new SqlParameter("@cel", _kid);
        prm[6] = new SqlParameter("@total", _jid);
        prm[7] = new SqlParameter("@leaves",_did);
        prm[8] = new SqlParameter("@working", _nid);

        da.executeDMLQuery("sms_calendar_insert", prm);

    }
    public void updateemployee()
    {
        da = new DataAccess();
        prm = new SqlParameter[11];
        prm[0] = new SqlParameter("@id",_id);
       
        prm[1] = new SqlParameter("@mob", _mob);
        prm[2] = new SqlParameter("@marital", _marital);
        prm[3] = new SqlParameter("@tadd", _tadd);
        prm[4] = new SqlParameter("@padd", _padd);
        prm[5] = new SqlParameter("@quali", _quali);
        prm[6] = new SqlParameter("@universe", _uni);
        prm[7] = new SqlParameter("@other", _other);
        prm[8] = new SqlParameter("@depart", _dept);
        prm[9] = new SqlParameter("@desig", _des);
        prm[10] = new SqlParameter("@sal ", _sal);
       da.executeDMLQuery("sms_emp_update", prm);

    }
    public void selectnamewise()
    {
        da = new DataAccess();
        prm = new SqlParameter[1];
      
        prm[0] = new SqlParameter("@id", _id);
       _ds= da.GetDataSet("sms_emp_select_namewise", prm);

    }
    public void addtotal()
    {
        da = new DataAccess();
        prm = new SqlParameter[1];

        prm[0] = new SqlParameter("@cid", _id);
        _ds = da.GetDataSet("sms_classwisetotalfees_select", prm);

    }
   
    public void deleteemp()
    {
        da = new DataAccess();
        prm = new SqlParameter[1];
    
        prm[0] = new SqlParameter("@id", _id);
        da.executeDMLQuery("sms_emp_delete", prm);

    }
    public void deletrollno()
    {
        da = new DataAccess();
        prm = new SqlParameter[1];

        prm[0] = new SqlParameter("@id", _id);
        da.executeDMLQuery("Sms_roll_pr_del", prm);

    }
    public void selctrolln()
    {
        da = new DataAccess();
        prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@id", _id);
        _ds=da.GetDataSet("sms_selectrolno_byid", prm);

    }
    public void Bindteacheremop()
    {
        da = new DataAccess();
        cmf = new CommonFunction();
        ds = da.GetDataSet("Sms_teacherselect");
        cmf.BindDropDownList(_ddl, _ds, "firstname", "id");
        ListItem li = new ListItem("--Select--", "0");
        _ddl.Items.Insert(0, li);

    }

    public void Bindtime11()
    {
        da = new DataAccess();
        cmf = new CommonFunction();
        ds = da.GetDataSet("DPS_class_time");
        cmf.BindDropDownList(_ddl, _ds, "startime", "id");
        ListItem li = new ListItem("--Select--", "0");
        _ddl.Items.Insert(0, li);

    }

    public void Bindmonth()
    {
        da = new DataAccess();
        cmf = new CommonFunction();
        ds = da.GetDataSet("sms_month_select");
        cmf.BindDropDownList(_ddl, _ds, "monthday", "id");
        ListItem li = new ListItem("--Select--", "0");
        _ddl.Items.Insert(0, li);

    }

   
    public void Bindstart()
    {
        da = new DataAccess();
        cmf = new CommonFunction();
        ds = da.GetDataSet("sms_startime_select");
        cmf.BindDropDownList(_ddl, _ds, "startime", "id");
        ListItem li = new ListItem("--Select--", "0");
        _ddl.Items.Insert(0, li);

    }
    public void Bindend()
    {
        da = new DataAccess();
        cmf = new CommonFunction();
        prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@start", _id);
        ds = da.GetDataSet("sms_endtime_select",prm);
        cmf.BindDropDownList(_ddl, _ds, "endtime", "id");
        ListItem li = new ListItem("--Select--", "0");
        _ddl.Items.Insert(0, li);

    }
    public void Bindday()
    {
        da = new DataAccess();
        cmf = new CommonFunction();
       
        ds = da.GetDataSet("sms_day_select");
        cmf.BindDropDownList(_ddl, _ds, "dayname", "id");
        ListItem li = new ListItem("--Select--", "0");
        _ddl.Items.Insert(0, li);

    }
    public void Bindemp()
    {
        da = new DataAccess();
        cmf = new CommonFunction();
        ds = da.GetDataSet("sms_emp_select");
        cmf.BindDropDownList(_ddl, _ds, "name", "id");
        ListItem li = new ListItem("--Select--", "0");
        _ddl.Items.Insert(0, li);

    }
    public void Binddepartmrny()
    {
        da = new DataAccess();
        cmf = new CommonFunction();
        ds = da.GetDataSet("sms_dept_select");
        cmf.BindDropDownList(_ddl, _ds, "dep", "id");
        ListItem li = new ListItem("--Select--", "0");
        _ddl.Items.Insert(0, li);

    }
    public void Binddesignation()
    {
        da = new DataAccess();
        cmf = new CommonFunction();
        prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@ID",_id);
        ds = da.GetDataSet("DPS_emp_deptwise", prm);
        cmf.BindDropDownList(_ddl, _ds, "designation", "id");
        ListItem li = new ListItem("--Select--", "0");
        _ddl.Items.Insert(0, li);

    }
   
    public void desiinsert()
    {
        da = new DataAccess();
        prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@cid",_id);
        prm[1] = new SqlParameter("@sname",_name);
        da.executeDMLQuery("Sms_desiginsert", prm);
    }
    public void desiupdate()
    {
        da = new DataAccess();
        prm = new SqlParameter[3];
        prm[0] = new SqlParameter("@cid", _cid);
        prm[1] = new SqlParameter("@sname", _name);
        prm[2] = new SqlParameter("@id", _id);
        da.executeDMLQuery("Sms_desig_update", prm);
    }
    public void sec_class_insert()
    {
        da = new DataAccess();
        prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@cid", _id);
        prm[1] = new SqlParameter("@sname", _name);
      //  prm[2] = new SqlParameter("@bid", _did);
        da.executeDMLQuery("Sms_sec_class_insert", prm);
    }
    public void sec_class_update()
    {
        da = new DataAccess();
        prm = new SqlParameter[3];
        prm[0] = new SqlParameter("@cid", _cid);
        prm[1] = new SqlParameter("@sname",_name);
        prm[2] = new SqlParameter("@id", _id);
      //  prm[3] = new SqlParameter("@bid", _did);
        da.executeDMLQuery("Sms_sec_class_update", prm);

    }
    public void desicustomer()
    {
        da = new DataAccess();
        prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@id",_id);
        da.executeDMLQuery("Sms_desig_delete", prm);

    }
    public void sec_class_delete()
    {
        da = new DataAccess();
        prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@id", _id);
        da.executeDMLQuery("Sms_sec_class_delete", prm);

    }
    public void bindgrid()
    {
        da = new DataAccess();
        _ds = da.GetDataSet("Sms_sec_class_select");
    }
    public void bindgrdata()
    {
        da = new DataAccess();
        _ds = da.GetDataSet("Sms_data_select_empl");
    }
    public void partinsert()
    {
        
        da = new DataAccess();
        prm = new SqlParameter[4];
        prm[0] = new SqlParameter("@from ",_ArrivalTime);
        if (_DepartureTime == null)
        {
            prm[1] = new SqlParameter("@to ", DBNull.Value);
        }
        else
        {
            prm[1] = new SqlParameter("@to ", _DepartureTime);
        }
        prm[2] = new SqlParameter("@name", _name);
        prm[3] = new SqlParameter("@session",_sal);
        da.executeDMLQuery("sms_part_insert", prm);
    }
    public void part1insert()
    {
        da = new DataAccess();
        prm = new SqlParameter[4];
        prm[0] = new SqlParameter("@from ", _ArrivalTime);
        prm[1] = new SqlParameter("@to ", _DepartureTime);
        prm[2] = new SqlParameter("@name", _name);
        prm[3] = new SqlParameter("@session", _id);
        da.executeDMLQuery("sms_part1_insert", prm);
    }
    public void part1update()
    {
        da = new DataAccess();
        prm = new SqlParameter[4];
        prm[0] = new SqlParameter("@from ", _ArrivalTime);
        prm[1] = new SqlParameter("@to ", _DepartureTime);
        prm[2] = new SqlParameter("@name", _name);
    
        prm[3] = new SqlParameter("@id", _id);
        da.executeDMLQuery("sms_part1_update", prm);
    }
    public void partupdate()
    {
        da = new DataAccess();
        prm = new SqlParameter[3];
        prm[0] = new SqlParameter("@from ", _ArrivalTime);
        //prm[1] = new SqlParameter("@to ", _DepartureTime);
        prm[1] = new SqlParameter("@name", _name);
       
        prm[2] = new SqlParameter("@id", _id);
        da.executeDMLQuery("sms_part_update", prm);
    }
    public void partdelete()
    {
        da = new DataAccess();
        prm = new SqlParameter[1];
        
        prm[0] = new SqlParameter("@id", _id);
        da.executeDMLQuery("sms_part_delete", prm);
    }
    public void part1delete()
    {
        da = new DataAccess();
        prm = new SqlParameter[1];

        prm[0] = new SqlParameter("@id", _id);
        da.executeDMLQuery("sms_part1_delete", prm);
    }
    public void part1select()
    {
        da = new DataAccess();
        _ds = da.GetDataSet("sms_part1_select");
    }
    public void partselect()
    {
        da = new DataAccess();
        _ds = da.GetDataSet("sms_part_select");
    }
    public int updateuserrole()
    {
        da = new DataAccess();
        prm = new SqlParameter[5];
        prm[0] = new SqlParameter("@name", _name);
        prm[1] = new SqlParameter("@useremail", _useremail);
        prm[2] = new SqlParameter("@pwd", _passwd);
        prm[3] = new SqlParameter("@id", _id);
        prm[4] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[4].Direction = ParameterDirection.ReturnValue;
       int x= da.executeDMLQuery("Adarsh_userlogin_update", prm);
       return x;

    }
    public int insertuser()
    {
        da = new DataAccess();
        prm = new SqlParameter[4];
        prm[0] = new SqlParameter("@name", _name);
        prm[1] = new SqlParameter("@useremail", _useremail);
        prm[2] = new SqlParameter("@pwd", _passwd);
        prm[3] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[3].Direction = ParameterDirection.ReturnValue;
      int x=  da.executeDMLQuery("Adarsh_userlogin_insert", prm);
      return x;
    }
    public void deleteuser()
    {
        da = new DataAccess();
        prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@id", _chid);
        da.executeDMLQuery("Adarsh_delete_userlogin", prm);

    }
    public void updaterole()
    {
        da = new DataAccess();
        prm = new SqlParameter[3];
        prm[0] = new SqlParameter("@pagename", _module);
        prm[1] = new SqlParameter("@pageurl", _url);
        prm[2] = new SqlParameter("@id", _id);

        da.executeDMLQuery("Adarsh_module_update", prm);

    }
    public void insertmodules()
    {
        da = new DataAccess();
        prm = new SqlParameter[3];
        prm[0] = new SqlParameter("@pagename", _module);
        prm[1] = new SqlParameter("@pageurl", _url);
        prm[2] = new SqlParameter("@pid", _pid);
        da.executeDMLQuery("Adarsh_roles_insertmodules", prm);
    }
    public void bindsubmodule()
    {
        prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@pid", _pid);
        da = new DataAccess();
        ds = da.GetDataSet("Adarsh_submodule_select", prm);
    }
    public void deleterole()
    {
        da = new DataAccess();
        prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@id", _chid);
        da.executeDMLQuery("Adarsh_role_delete", prm);

    }

    public void bindmodule()
    {
        da = new DataAccess();
        ds = da.GetDataSet("Adarsh_module_selectall");
    }
    public void bindmenu()
    {
        da = new DataAccess();
        prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@pid", _pid);
        prm[1] = new SqlParameter("@uid", _id);
        ds = da.GetDataSet("tbl_user_select", prm);
    }
   
}
