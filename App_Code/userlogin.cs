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
/// Summary description for userlogin
/// </summary>
public class userlogin
{
	public userlogin()
	{
		//
		// TODO: Add constructor logic here
		//
    }
    SqlParameter[] prm;
    DataAccess da;
    CommonFunction cmf;

    #region Data Member
    private int _id; 
   
    private string _passwd = string.Empty;
    public string Passwd
    {
        get { return _passwd; }
        set { _passwd = value; }
    }
    private string _roleid = string.Empty;
    public string Roleid
    {
        get { return _roleid; }
        set { _roleid = value; }
    }
    private string dob = string.Empty;
    public string Dob
    {
        get { return dob; }
        set { dob = value; }
    }
    private string doj = string.Empty;

    public string Doj
    {
        get { return doj; }
        set { doj = value; }
    }
  
  
    private int _pid; 

  
   

    private string _from = string.Empty;
    private string _to = string.Empty;
    
    private string _firstname = string.Empty;
    private string _lastname = string.Empty;
    private string _group = string.Empty;

    public string Group
    {
        get { return _group; }
        set { _group = value; }
    }
    private string _designation = string.Empty;

    public string Designation
    {
        get { return _designation; }
        set { _designation = value; }
    }
    private string _username = string.Empty;

    public string Username
    {
        get { return _username; }
        set { _username = value; }
    }
  
    public string Firstname
    {
        get { return _firstname; }
        set { _firstname = value; }
    }
    public string Lastname
    {
        get { return _lastname; }
        set { _lastname = value; }
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




 
 
   
 




    private DataSet _ds;
    private DropDownList _ddl;
  
 

  
    private string _module = string.Empty;
    public string Module
    {
        get { return _module; }
        set { _module = value; }
    }

    private string _url = string.Empty;
    private int _chid;
    public string Url
    {
        get { return _url; }
        set { _url = value; }
    }
    public int Chid
    {
        get { return _chid; }
        set { _chid = value; }
    }

  


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


    private string _emailid = string.Empty;
    public string Emailid
    {
        get { return _emailid; }
        set { _emailid = value; }
    }
    private string _target = string.Empty;
    public string target
    {
        get { return _target; }
        set { _target = value; }
    }

    private string _sallery = string.Empty;
    public string sallery
    {
        get { return _sallery; }
        set { _sallery = value; }
    }


    private string _phone = string.Empty;
    public string phone
    {
        get { return _phone; }
        set { _phone = value; }
    }

    private string _extention = string.Empty;
    public string extention
    {
        get { return _extention; }
        set { _extention = value; }
    }


    #endregion
    public void Bindddl()
    {
        ListItem li = new ListItem("--Select--", "0");
        _ddl.Items.Insert(0, li);

    }

    public void Binduser()
    {
        da = new DataAccess();
        _ds = da.GetDataSet("la_userlogin_select");

    }

    public void Bindroles()
    {
        da = new DataAccess();
        _ds = da.GetDataSet("la_roles_selectall");

    }


    public void insertuser()
    {
        da = new DataAccess();
        prm = new SqlParameter[15];
        prm[0] = new SqlParameter("@firstname", _firstname);
        prm[1] = new SqlParameter("@lastname", _lastname);
        prm[2] = new SqlParameter("@emailid", _emailid);
        prm[3] = new SqlParameter("@passwd", _passwd);
        prm[4] = new SqlParameter("@createddate", _date);
        prm[5] = new SqlParameter("@groupid", _group);
        prm[6] = new SqlParameter("@roleid", _designation);
        prm[7] = new SqlParameter("@username", _username);
        prm[8] = new SqlParameter("@dob", dob);
        prm[9] = new SqlParameter("@doj", doj);
        prm[10] = new SqlParameter("@imgurl", _url);
        prm[11] = new SqlParameter("@target", _target);
        prm[12] = new SqlParameter("@sallery", _sallery);
        prm[13] = new SqlParameter("@phone", _phone);
        prm[14] = new SqlParameter("@extention", _extention);
        //HttpContext.Current.Response.Write(prm[6].Value);
        da.executeDMLQuery("la_userlogin_insert_new", prm);
    }


    public void insertrole()
    {
        da = new DataAccess();
        prm = new SqlParameter[4];
        prm[0] = new SqlParameter("@profile", _module);
        prm[1] = new SqlParameter("@priority", _roll);
        prm[2] = new SqlParameter("@regid",HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@cdate", da.getcdatetime());
        da.executeDMLQuery("Manage_Profile_Insertcdate", prm);
    }


    public void insertmodules()
    {
        da = new DataAccess();
        prm = new SqlParameter[3];
        prm[0] = new SqlParameter("@pagename", _module);
        prm[1] = new SqlParameter("@pageurl", _url);
        prm[2] = new SqlParameter("@pid", _pid);
        da.executeDMLQuery("la_roles_insertmodules", prm);
    }


    

    public void updaterole()
    {
        da = new DataAccess();
        prm = new SqlParameter[3];
        prm[0] = new SqlParameter("@profile", _module);
        prm[1] = new SqlParameter("@priority", _roll);
        prm[2] = new SqlParameter("@id", _id);

        da.executeDMLQuery("Manage_Profile_Update", prm);

    }
    public void updateuserrole()
    {
        da = new DataAccess();
         prm = new SqlParameter[15];
        prm[0] = new SqlParameter("@firstname", _firstname);
        prm[1] = new SqlParameter("@lastname", _lastname);
        prm[2] = new SqlParameter("@emailid", _emailid);
        prm[3] = new SqlParameter("@passwd", _passwd);        
        prm[4] = new SqlParameter("@id", _id);
        prm[5] = new SqlParameter("@groupid", _group);
        prm[6] = new SqlParameter("@roleid", _designation);
        prm[7] = new SqlParameter("@username", _username);
        prm[8] = new SqlParameter("@dob", dob);
        prm[9] = new SqlParameter("@doj", doj);
        prm[10] = new SqlParameter("@imgurl", _url);
        prm[11] = new SqlParameter("@target", _target);
        prm[12] = new SqlParameter("@sallery", _sallery);
        prm[13] = new SqlParameter("@phone", _phone);
        prm[14] = new SqlParameter("@extention", _extention);
        da.executeDMLQuery("la_userlogin_update_new", prm);

    }

    public void deleteuser()
    {
        da = new DataAccess();
        prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@id", _chid);
        da.executeDMLQuery("la_delete_userlogin", prm);

    }

    public void deleterole()
    {
        da = new DataAccess();
        prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@id", _chid);
        da.executeDMLQuery("Manage_Profile_Delete", prm);

    }
    public void bindmodule()
    {
        da = new DataAccess();
        ds = da.GetDataSet("la_module_selectall");
    }


    public void bindsubmodule()
    {
        prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@pid", _pid);
        da = new DataAccess();
        ds = da.GetDataSet("la_submodule_select", prm);
    }


    public void bindmenu()
    {
        da = new DataAccess();
        prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@pid", _pid);
        prm[1] = new SqlParameter("@uid", _id);
        ds = da.GetDataSet("la_menu_select", prm);
    }

}