
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
public class empl_new
{
    private SqlConnection mycon;
    private SqlCommand mycmd;
    private SqlDataAdapter myda;
    private DataSet myds;
    private int retvalue;
    private SqlDataReader mydr;
    private SqlParameter[] param;
    public empl_new()
	{
		//
		// TODO: Add constructor logic here
		//
    }
    SqlParameter[] prm;
    DataAccess da=new DataAccess();
    CommonFunction cmf;
    #region Data Member
    private int _id; private int _nid; private int _qid;
    private int _bid;
    private int _cid; private int _sid; private int _eid;
    private int _kid; private int _jid; private int _did;
    private string _starttime;
    private string _endtime;
    private string _day;
    private string _prefixname;
    private string _middlename;
    private string _lname;
    private string _email2;
    private string _mobno2;
    private string _fathername;
    private string _mothername;
    private string _bloodgroup;
    private string _country;
    private string _country2;
    private string _state;
    private string _state2;
    private string _city;
    private string _city2;
    private string _zipcode;
    private string _zipcode2;
    private string _companyname;
    private string _contactno;

    public string Contactno
    {
        get { return _contactno; }
        set { _contactno = value; }
    }
    public string Companyname
    {
        get { return _companyname; }
        set{_companyname= value;}
    }
    public string Zipcode2
    {
        get { return _zipcode2; }
        set { _zipcode2 = value; }
    }
    public string Zipcode
    {
        get { return _zipcode; }
        set { _zipcode = value; }
    }
    public string City2
    {
        get { return _city2; }
        set { _city2 = value; }
    }
    public string City
    {
        get { return _city; }
        set { _city = value; }
    }
    public string State2
    {
        get { return _state2; }
        set { _state2 = value; }
    }
    public string State
    {
        get { return _state; }
        set { _state = value; }
    }
    public string Country2
    {
        get { return _country2; }
        set { _country2 = value; }
    }
    public string Country
    {
        get { return _country; }
        set { _country = value; }
    }
    public string Bloodgroup
    {
        get { return _bloodgroup; }
        set { _bloodgroup = value; }
    }
    public string Mothername
    {
        get { return _mothername; }
        set { _mothername = value; }
    }
    public string Fathername
    {
        get { return _fathername; }
        set { _fathername = value; }
    }
    public string Mobno2
    {
        get { return _mobno2; }
        set { _mobno2 = value; }
    }
    public string Email2
    {
        get { return _email2; }
        set { _email2 = value; }
    }
    public string Lname
    {
        get { return _lname; }
        set { _lname = value; }
    }
    public string Prefixname
    {
        get { return _prefixname; }
        set { _prefixname = value; }
    }
    public string Middlename
    {
        get { return _middlename; }
        set { _middlename = value; }
    }
    public string Day
    {
        get { return _day; }
        set { _day = value; }
    }
    public string Endtime
    {
        get { return _endtime; }
        set { _endtime = value; }
    }
   
    public string Starttime
    {
        get { return _starttime; }
        set { _starttime = value; }
    }
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
    private int _yyyy;    
    private int _MM;
    private int _isfixweekoff;   
    private string _weekday = string.Empty;
    public int Isfixweekoff
    {
        get { return _isfixweekoff; }
        set { _isfixweekoff = value; }
    }
  
    public int Yyyy
    {
        get { return _yyyy; }
        set { _yyyy = value; }
    }
    public int MM
    {
        get { return _MM; }
        set { _MM = value; }
    }

    public string Weekday
    {
        get { return _weekday; }
        set { _weekday = value; }
    }
    private string _module = string.Empty;
    private string _url = string.Empty;
    private string _cattype = string.Empty;
    private string _category = string.Empty;

    public string Category
    {
        get { return _category; }
        set { _category = value; }
    }
    public string Cattype
    {
        get { return _cattype; }
        set { _cattype = value; }
    }
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
    private string _subcategory = string.Empty;


    #region Employee Attendance


    public void OpenConnection()
    {
        if (this.mycon == null)
            this.mycon = new SqlConnection(ConfigurationManager.AppSettings["connectionString"]);
        if (this.mycon.State != ConnectionState.Closed)
            return;
        this.mycon.Open();
    }

    public void CloseConnection()
    {
        if (this.mycon == null || this.mycon.State != ConnectionState.Open)
            return;
        this.mycon.Close();
    }

    public string getcdate()
    {
        string str = da.getcdate();

        return str;


      //  this.OpenConnection();
      //  this.mycmd = new SqlCommand("select  DATEADD(mi,30,DATEADD(hh,13,GETDATE()))", this.mycon);
      //  string str = this.mycmd.ExecuteScalar().ToString();
      //  this.CloseConnection();
      //  return str.Split(new char[1]
      //{
      //  ' '
      //})[0];
    }

    public string getcdatetime()
    {
        //this.OpenConnection();
        //this.mycmd = new SqlCommand("select  DATEADD(mi,0,DATEADD(hh,13,GETDATE()))", this.mycon);
       // string str = this.mycmd.ExecuteScalar().ToString();
       // this.CloseConnection();
        string str = da.getcdatetime();
        
        return str;
    }

    public string NoOfSpecificDaysThisMonth(int tdays)
    {

        int iDayCnt = 4;
        if (tdays != 31 || tdays != 30)
        {
            iDayCnt = tdays / 7;
        }
        // Defaults to four days 
        if (_isfixweekoff == 1)
        {
            DayOfWeek dw = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), _weekday); ;
            DateTime firstDay = new DateTime(_yyyy, _MM, 1);
            int iRemainder = DateTime.DaysInMonth(_yyyy, _MM) % 7;

            if ((dw >= firstDay.DayOfWeek) & ((dw - firstDay.DayOfWeek) < iRemainder))
            {
                iDayCnt = iDayCnt + 1;
            }
            else if ((dw < firstDay.DayOfWeek) & ((7 + dw - firstDay.DayOfWeek) < iRemainder))
            {
                iDayCnt = iDayCnt + 1;
            }
        }
        return iDayCnt.ToString();
    }
    public string NoOFPublicHolidays(string mm, string yy)
    {
        string str = string.Empty;
        SqlParameter[] prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@mm", mm);
        prm[1] = new SqlParameter("@yy", yy);
        DataSet ds = da.GetDataSet("GetNoOFPublicHolidays", prm);
        if (ds.Tables[0].Rows.Count > 0)
        {
            str = ds.Tables[0].Rows[0]["noofDays"].ToString();
        }
        else
        {
            str = "0";
        }
        return str;
    }
    public string GetPublicHoliday(string cdate)
    {
        string str = string.Empty;
        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@cdate", cdate);
        DataSet ds = da.GetDataSet("GetPublicHolidaysInMonth", prm);
        if (ds.Tables[0].Rows.Count > 0)
        {
            str = "1";
        }
        else
        {
            str = "0";
        }
        return str;
    }
    public string convertminstoHours(int minutes)
    {

        int hours = (minutes - minutes % 60) / 60;

        return hours + ":" + (minutes - hours * 60);

    }
    public string getinoutremark(string empcode, string cdate, string inout)
    {
        string retstr = string.Empty;
        SqlParameter[] prm = new SqlParameter[3];
        prm[0] = new SqlParameter("@cdate", cdate);
        prm[1] = new SqlParameter("@empid", empcode);
        prm[2] = new SqlParameter("@inout", inout);
        DataSet dsinout = da.GetDataSet("la_getremark", prm);
        if (dsinout.Tables[0].Rows.Count > 0)
        {
            retstr = dsinout.Tables[0].Rows[0]["la_remark"].ToString();
        }
        return retstr;
    }
    public string getoffremarks(string empcode, string cdate)
    {
        string retstr = string.Empty;
        //SqlParameter[] prm = new SqlParameter[2];
        //prm[0] = new SqlParameter("@cdate", cdate);
        //prm[1] = new SqlParameter("@empid", empcode);
        //DataSet dsinout = da.GetDataSet("getholidaydetailsbyuid", prm);
        //if (dsinout.Tables[0].Rows.Count > 0)
        //{
        //    retstr = dsinout.Tables[0].Rows[0]["remarks"].ToString();
        //}
        return retstr;
    }
    public string getoffreason(string empcode, string cdate)
    {
        string retstr = string.Empty;
        //SqlParameter[] prm = new SqlParameter[2];
        //prm[0] = new SqlParameter("@cdate", cdate);
        //prm[1] = new SqlParameter("@empid", empcode);
        //DataSet dsinout = da.GetDataSet("getholidaydetailsbyuid", prm);
        //if (dsinout.Tables[0].Rows.Count > 0)
        //{
        //    retstr = dsinout.Tables[0].Rows[0]["reason"].ToString();
        //}
        return retstr;
    }
    public int NoofweeklyoffOnPH(string empcode, string cdate)
    {
        string retstr = string.Empty;
        SqlParameter[] prm = new SqlParameter[3];
        prm[0] = new SqlParameter("@cdate", cdate);
        prm[1] = new SqlParameter("@empid", empcode);
        prm[2] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[2].Direction = ParameterDirection.ReturnValue;
        int x = da.executeDMLQuery("getnoofweeklyoffonpublicholiday", prm);
        return x;
    }
    public double mygetinouttimediff(string cdate, string edate, DataSet ds)
    {


        double totalmins = 0;
        double retstr = 0;
        double tminutes = 0;
        double mm = 0;
        if (cdate != string.Empty && edate != string.Empty)
        {
            DateTime startTime = Convert.ToDateTime(cdate);


            DateTime endTime = Convert.ToDateTime(edate);

            TimeSpan span = endTime.Subtract(startTime);
            tminutes = span.TotalMinutes;
        }
        DataSet dsinout = ds;
        int tdcount = 0;
        int flag = 0;
        int flagnew = 0;
        if (dsinout.Tables[0].Rows.Count > 0)
        {
            string lcdate = string.Empty;
            string ledate = string.Empty;
            int tddd = 0;
            int tdddd = 0;
            for (int i = 0; i < dsinout.Tables[0].Rows.Count; i++)
            {
                tdddd++;
                if (tdcount < 2)
                {
                    flag = 1;
                    tdcount++;
                }
                else
                {

                    if (lcdate != string.Empty && ledate != string.Empty)
                    {
                        DateTime lstartTime = Convert.ToDateTime(lcdate);

                        DateTime lendTime = Convert.ToDateTime(ledate);

                        TimeSpan lspan = lendTime.Subtract(lstartTime);
                        totalmins += lspan.TotalMinutes;
                        if (dsinout.Tables[0].Rows[i]["outtype"].ToString() == "4")
                        {
                            mm += lspan.TotalMinutes;
                        }
                        else if (dsinout.Tables[0].Rows[i]["outtype"].ToString() == "2")
                        {
                            if (lspan.TotalMinutes > 30)
                            {
                                double tm = lspan.TotalMinutes - 30;
                                mm += tm;
                            }
                        }
                    }

                    flag = 0;
                    tdcount = 1;
                }


                tddd++;

                if (tddd == 1)
                {
                    lcdate = dsinout.Tables[0].Rows[i]["actualtime"].ToString();
                    ledate = string.Empty;
                }
                if (tddd == 2)
                {

                    ledate = dsinout.Tables[0].Rows[i]["actualtime"].ToString();
                    tddd = 0;
                }



            }


            if (lcdate != string.Empty && ledate != string.Empty)
            {
                DateTime lstartTime = Convert.ToDateTime(lcdate);

                DateTime lendTime = Convert.ToDateTime(ledate);

                TimeSpan lspan = lendTime.Subtract(lstartTime);

                totalmins += lspan.TotalMinutes;
                if (dsinout.Tables[0].Rows[0]["outtype"].ToString() == "4")
                {
                    mm += lspan.TotalMinutes;
                }
                else if (dsinout.Tables[0].Rows[0]["outtype"].ToString() == "2")
                {
                    if (lspan.TotalMinutes > 30)
                    {
                        double tm = lspan.TotalMinutes - 30;
                        mm += tm;
                    }
                }
            }


            if (flagnew == 0)
            {
                flagnew = 1;
            }
            retstr = tminutes - mm;
            //   retstr = tminutes;
        }
        else
        {

            retstr = tminutes;
        }

        return retstr;


    }
    public string gettdcolor(string date)
    {
        string retstr = string.Empty;
        if (date != string.Empty)
        {
            retstr = "1";
        }
        else
        {
            retstr = "0";
        }
        return retstr;
    }
    public string getap(string date)
    {
        string retstr = string.Empty;
        if (date != string.Empty)
        {
            retstr = "Present";
        }
        else
        {
            retstr = "Absent";
        }
        return retstr;
    }

    public string gettime(string date)
    {
        //10/1/2013 9:33:00 AM
        string rest = string.Empty;
        if (date.ToString() != "")
        {
            string[] tm1 = date.ToString().Split(' ');
            string[] tmnew = tm1[1].Split(':');
            rest = tmnew[0].ToString() + ":" + tmnew[1].ToString() + " " + tm1[2].ToString();
        }
        else
        {
            rest = "--:--:--";

        }
        return rest;
    }


    public double ConvertMinutesToDays(double minutes)
    {
        return TimeSpan.FromMinutes(minutes).TotalDays;
    }
    public double ConvertMinutesToHours(double minutes)
    {
        return TimeSpan.FromMinutes(minutes).TotalHours;
    }
    public decimal getdaysfromred(int red)
    {
        decimal days = 0;
        if (red >= 3)
        {
            days = red / 3;
        }
        return days;
    }
    public decimal getdaysfromhalf(int hlf)
    {
        decimal days = 0;
        if (hlf >= 1)
        {
            days = hlf * Convert.ToDecimal(0.5);
        }
        return days;
    }
    public string getpointsperday(string empcode, string cdate)
    {
        string retstr = string.Empty;
         SqlParameter[] prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@ddate", cdate);
        prm[1] = new SqlParameter("@empid", empcode);
        DataSet dsinout = da.GetDataSet("getpointsperday", prm);
        if (dsinout.Tables[0].Rows.Count > 0)
        {
            retstr = dsinout.Tables[0].Rows[0]["DayPoints"].ToString();
        }
        return retstr;

    }
    public string getremarks(string empcode, string cdate, string edate)
    {
        string retstr = string.Empty;
        SqlParameter[] prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@cdate", cdate);
        prm[1] = new SqlParameter("@empid", empcode);
        DataSet dsinout = da.GetDataSet("getempinoutedit", prm);
        if (dsinout.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dsinout.Tables[0].Rows.Count; i++)
            {
                retstr += "<span style='color:#008000; font-weight:bold; font-size:12; text-decoration:blink' >" + dsinout.Tables[0].Rows[i]["remarks"].ToString() + "</span><br/>";
            }
        }
        return retstr;
    }
    #endregion


    public string Subcategory
    {
        get { return _subcategory; }
        set { _subcategory = value; }
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
        prm = new SqlParameter[5];
        prm[0] = new SqlParameter("@starttime", _starttime);
        prm[1] = new SqlParameter("@endtime", _endtime);
        prm[2] = new SqlParameter("@day", _day);
        prm[3] = new SqlParameter("@date", _date);
        prm[4] = new SqlParameter("@cid", _cid);
        ds = da.GetDataSet("Select_FreeTeacher_for_TimeTable", prm);
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
    public string inserttime()
    {
        string msg="";
        da = new DataAccess();
        prm = new SqlParameter[3];
        prm[0] = new SqlParameter("@start",_name);
        prm[1] = new SqlParameter("@over",_fname);
        prm[2] = new SqlParameter("@return", SqlDbType.NVarChar,50);
        prm[2].Direction = ParameterDirection.Output;
        da.executeDMLQuery("SMS_time_insert", prm);
        msg = prm[2].Value.ToString();
        return msg;
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
        int id=  da.executeDMLQuery("sms_Fees_insert", prm);
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
    public void ATMDirectoryInsert()
    {
        da = new DataAccess();
        prm = new SqlParameter[30];
        prm[0] = new SqlParameter("@gender", _gender);
        prm[1] = new SqlParameter("@prefix", _prefixname);
        prm[2] = new SqlParameter("@fname", _fname);
        prm[3] = new SqlParameter("@mname", _middlename);
        prm[4] = new SqlParameter("@lname", _lname);
        prm[5] = new SqlParameter("@email", _useremail);
        prm[6] = new SqlParameter("@email2", _email2);
        prm[7] = new SqlParameter("@mobno", _mob);
        prm[8] = new SqlParameter("@mobno2", _mobno2);
        prm[9] = new SqlParameter("@fathername", _fathername);
        prm[10] = new SqlParameter("@mothername", _mothername);
        prm[11] = new SqlParameter("@marital", _marital);
        prm[12] = new SqlParameter("@bloodgroup", _bloodgroup);
        prm[13] = new SqlParameter("@country", _country);
        prm[14] = new SqlParameter("@country2", _country2);
        prm[15] = new SqlParameter("@state", _state);
        prm[16] = new SqlParameter("@state2", _state2);
        prm[17] = new SqlParameter("@city", _city);
        prm[18] = new SqlParameter("@city2", _city2);
        prm[19] = new SqlParameter("@currentaddress", _tadd);
        prm[20] = new SqlParameter("@zipcode", _zipcode);
        prm[21] = new SqlParameter("@parmanentaddress", _padd);
        prm[22] = new SqlParameter("@zipcode2", _zipcode2);
        prm[23] = new SqlParameter("@companyname", _companyname);
        prm[24] = new SqlParameter("@departmentid", _dept);
        prm[25] = new SqlParameter("@managername", _name);
        prm[26] = new SqlParameter("@mangerphonno", _contactno);
        prm[27] = new SqlParameter("@cattype", _cattype);
        prm[28] = new SqlParameter("@categoryid", _category);
        prm[29] = new SqlParameter("@subcat", _subcategory);
        da.executeDMLQuery("ATM_M_Directory_Insert", prm);

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
        prm[0] = new SqlParameter("@start", _starttime);
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
        ds = da.GetDataSet("Adarsh_menu_select", prm);
    }

    public void bindmenuforEmployee()
    {
        da = new DataAccess();
        prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@pid", _pid);
        prm[1] = new SqlParameter("@uid", _id);
        ds = da.GetDataSet("Adarsh_menu_selectforemployee", prm);
    }


   
}
