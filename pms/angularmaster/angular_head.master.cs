using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using CommonClass;
using System.Data.SqlClient;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Web.Services;


public partial class angular_head : System.Web.UI.MasterPage
{
    DataAccess da = new DataAccess();
    DataSet Ds = new DataSet();
    public int chkisadmin = 0;
    public int issingleuser = 0;
    // menus
    public int isdashboard = 0;
    public int isproject = 0;
    public int ismytask = 0;
    public int isdelayedtask = 0;
    public int isteamwork = 0;
    public int isqc = 0;
    public int isissue = 0;
    public int iscostcontrol = 0;

    public string strmenudata = "";
    public int ispmsdashboard_admin= 0;
    public int ispmsdashboard_user = 0;
    public int ispmsdashboard_team = 0;

    public string currentpage = "";
    
    public int ischeckAllaccess = 0;
    public int ischeckCreate = 0;
    public int ischeckEdit = 0;
    public int ischeckDelete = 0;
    public int ischeckView = 0;
    public int ischeckAssign = 0;

    public int ischeckAllaccess_mile = 0;
    public int ischeckCreate_mile = 0;
    public int ischeckEdit_mile = 0;
    public int ischeckDelete_mile = 0;
    public int ischeckView_mile = 0;
    public int ischeckAssign_mile = 0;

    public int ischeckAllaccess_task = 0;
    public int ischeckCreate_task = 0;
    public int ischeckEdit_task = 0;
    public int ischeckDelete_task = 0;
    public int ischeckView_task = 0;
    public int ischeckAssign_task = 0;

    public int ischeckAllaccess_stask = 0;
    public int ischeckCreate_stask = 0;
    public int ischeckEdit_stask = 0;
    public int ischeckDelete_stask = 0;
    public int ischeckView_stask = 0;
    public int ischeckAssign_stask = 0;
    public int ischeckAllaccess_qc = 0;
    public int ischeckCreate_qc = 0;
    public int ischeckEdit_qc = 0;
    public int ischeckDelete_qc = 0;
    public int ischeckView_qc = 0;
    public int ischeckAssign_qc = 0;
    public CommonFunction cmf = new CommonFunction();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Cookies["user"] == null || Request.Cookies["cadmin"] == null)
        {
            Response.Redirect("/login.aspx"); // Redirect immediately if cookies are missing
        }
        String activepage = Request.RawUrl;
        currentpage = activepage;
        //if (currentpage.Contains("/dashboarduser.aspx"))
        //{
        //    myproject.Attributes.Add("class", "");
        //    dashboard.Attributes.Add("class", "active");
        //}
        //else if (currentpage.Contains("/mytaskuser.aspx"))
        //{
        //    dashboard.Attributes.Add("class", "");
        //    myproject.Attributes.Add("class", "active");
        //}
        //else if (currentpage.Contains("/myteam.aspx"))
        //{

        //}
        string uid = Request.Cookies["user"]["user_id"].ToString();
        try
        {
            if (Request.QueryString["empid"] != null)
            {
                uid = cmf.Decrypt(Request.QueryString["empid"]).ToString();
            }
        }
        catch
        { }
        chkisadmin = checkadmin(uid);
        issingleuser = checksingleuser(uid);
        // Cost Control link: administrators and employees granted access (never throws).
        iscostcontrol = PmsCostControl.ShowMenuLink() ? 1 : 0;
        if (Request.QueryString["empid"] != null)
        {
            
            checkformvalidation(chkisadmin,Request.QueryString["empid"].ToString(),uid);
        }
        else
        {
            checkformvalidation(chkisadmin,"", uid);
        }
        //try
        //{
        //    string strvalidationS = get_EditDelete_Validation("53");
        //    //string strvalidationS = get_EditDelete_Validation("1056");

        //    string[] arS = strvalidationS.Split(',');
        //    ischeckAllaccess = Convert.ToInt32(arS[0]);
        //    ischeckView = Convert.ToInt32(arS[1]);
        //    ischeckCreate = Convert.ToInt32(arS[2]);
        //    ischeckEdit = Convert.ToInt32(arS[3]);
        //    ischeckDelete = Convert.ToInt32(arS[4]);
        //    ischeckAssign = Convert.ToInt32(arS[5]);
        //    #region milestone
        //    string strvalidationM = get_EditDelete_Validation("54");
        //    //string strvalidationS = get_EditDelete_Validation("1056");
        //    string[] arM = strvalidationM.Split(',');
        //    ischeckView_mile = Convert.ToInt32(arM[0]);
        //    ischeckAllaccess_mile = Convert.ToInt32(arM[1]);
        //    ischeckCreate_mile = Convert.ToInt32(arM[2]);
        //    ischeckEdit_mile = Convert.ToInt32(arM[3]);
        //    ischeckDelete_mile = Convert.ToInt32(arM[4]);
        //    ischeckAssign_mile = Convert.ToInt32(arM[5]);
        //    #endregion
        //    #region task
        //    string strvalidationS_task = get_EditDelete_Validation("55");
        //    // string strvalidationS_task = get_EditDelete_Validation("1057");
        //    string[] arS_task = strvalidationS_task.Split(',');

        //    ischeckView_task = Convert.ToInt32(arS_task[0]);
        //    ischeckAllaccess_task = Convert.ToInt32(arS_task[1]);
        //    ischeckCreate_task = Convert.ToInt32(arS_task[2]);
        //    ischeckEdit_task = Convert.ToInt32(arS_task[3]);
        //    ischeckDelete_task = Convert.ToInt32(arS_task[4]);
        //    ischeckAssign_task = Convert.ToInt32(arS_task[5]);
        //    #endregion
        //    #region sub task
        //    string strvalidationS_stask = get_EditDelete_Validation("56");
        //    string[] arS_stask = strvalidationS_stask.Split(',');
        //    ischeckView_stask = Convert.ToInt32(arS_stask[0]);
        //    ischeckAllaccess_stask = Convert.ToInt32(arS_stask[1]);
        //    ischeckCreate_stask = Convert.ToInt32(arS_stask[2]);
        //    ischeckEdit_stask = Convert.ToInt32(arS_stask[3]);
        //    ischeckDelete_stask = Convert.ToInt32(arS_stask[4]);
        //    ischeckAssign_stask = Convert.ToInt32(arS_stask[5]);
        //    #endregion
        //    #region QC
        //    //string strvalidationS_qc = get_EditDelete_Validation("56");
        //    string strvalidationS_qc = get_EditDelete_Validation("1063");
        //    string[] arS_qc = strvalidationS_stask.Split(',');
        //    ischeckView_qc = Convert.ToInt32(arS_stask[0]);
        //    ischeckAllaccess_qc = Convert.ToInt32(arS_stask[1]);
        //    ischeckCreate_qc = Convert.ToInt32(arS_stask[2]);
        //    ischeckEdit_qc = Convert.ToInt32(arS_stask[3]);
        //    ischeckDelete_qc = Convert.ToInt32(arS_stask[4]);
        //    ischeckAssign_qc = Convert.ToInt32(arS_stask[5]);
        //    #endregion
        //}
        //catch
        //{

        //}
       
        //if (ischeckView == 1 || ischeckCreate == 1 || ischeckEdit == 1 || ischeckDelete == 1 || ischeckAssign == 1 || ischeckView_mile == 1 || ischeckCreate_mile == 1 || ischeckEdit_mile == 1 || ischeckDelete_mile == 1 || ischeckAssign_mile == 1 || ischeckView_task == 1 || ischeckCreate_task == 1 || ischeckEdit_task == 1 || ischeckDelete_task == 1 || ischeckAssign_task == 1 || ischeckView_stask == 1 || ischeckCreate_stask == 1 || ischeckEdit_stask == 1 || ischeckDelete_stask == 1 || ischeckAssign_stask == 1)
        //{
        //    isproject = 1;
        //    usertask.Attributes.Add("class", "");
        //    myproject.Attributes.Add("class", "active");
        //    //if (currentpage.Contains("/users/"))
        //    //{
        //    //    myproject.Attributes.Add("class", "");
        //    //    usertask.Attributes.Add("class", "active");
        //    //}
        //    if (currentpage.Contains("/dashboard/"))
        //    {
        //        myproject.Attributes.Add("class", "");
        //        myqc.Attributes.Add("class", "");
        //        usertask.Attributes.Add("class", "");
        //        myissues.Attributes.Add("class", "");
        //        userdashboard.Attributes.Add("class", "");
        //        dashboard.Attributes.Add("class", "active");
        //    }
        //    else if(currentpage.Contains("/userdashboard/"))
        //    {
        //        myproject.Attributes.Add("class", "");
        //        myqc.Attributes.Add("class", "");
        //        usertask.Attributes.Add("class", "");
        //        myissues.Attributes.Add("class", "");
        //        dashboard.Attributes.Add("class", "");
        //        userdashboard.Attributes.Add("class", "active");
        //    }
        //    else if(currentpage.Contains("/newuserdashboard/"))
        //    {
        //        myproject.Attributes.Add("class", "");
        //        myqc.Attributes.Add("class", "");
        //        usertask.Attributes.Add("class", "");
        //        myissues.Attributes.Add("class", "");
        //        dashboard.Attributes.Add("class", "");
        //        userdashboard.Attributes.Add("class", "");
        //        newuserdashboard.Attributes.Add("class", "active");
        //    }
        //    else if (currentpage.Contains("/projects/"))
        //    {
               
        //        dashboard.Attributes.Add("class", "");
        //        myqc.Attributes["class"] = "";
        //        userdashboard.Attributes.Add("class", "");
        //        myissues.Attributes.Add("class", "");
        //        usertask.Attributes["class"] = "";
        //        userdelayedtask.Attributes["class"] = "";
        //        myproject.Attributes["class"] = "active";
              
        //    }
        //    else if (currentpage.Contains("/users/"))
        //    {
        //        string taskType = (Request.QueryString["type"] ?? "mytask")
        //            .Trim()
        //            .ToLowerInvariant();
        //        dashboard.Attributes.Add("class", "");
        //        myproject.Attributes["class"] = "";
        //        myqc.Attributes["class"] = "";
        //        userdashboard.Attributes.Add("class", "");
        //        myissues.Attributes.Add("class", "");
        //        if (taskType == "delayedtask")
        //        {
        //            usertask.Attributes["class"] = "";
        //            userdelayedtask.Attributes["class"] = "active";
        //        }
        //        else
        //        {
        //            // Default is My Task
        //            usertask.Attributes["class"] = "active";
        //            userdelayedtask.Attributes["class"] = "";
        //        }
        //    }
        //    else if (currentpage.Contains("/pendingqc/"))
        //    {
        //        dashboard.Attributes.Add("class", "");
        //        myproject.Attributes.Add("class", "");
        //        usertask.Attributes.Add("class", "");
        //        myissues.Attributes.Add("class", "");
        //        myqc.Attributes.Add("class", "active");
        //    }
        //    else if (currentpage.Contains("/myteamtask/"))
        //    {
        //        dashboard.Attributes.Add("class", "");
        //        myproject.Attributes.Add("class", "");
        //        usertask.Attributes.Add("class", "");
        //        userdashboard.Attributes.Add("class", "");
        //        myqc.Attributes.Add("class", "");
        //        myissues.Attributes.Add("class", "");
        //        myteam.Attributes.Add("class", "active");
        //    }
        //    else if (currentpage.Contains("/issues/"))
        //    {
        //        dashboard.Attributes.Add("class", "");
        //        myproject.Attributes.Add("class", "");
        //        usertask.Attributes.Add("class", "");
        //        userdashboard.Attributes.Add("class", "");
        //        myqc.Attributes.Add("class", "");
        //        myteam.Attributes.Add("class", "");
        //        myissues.Attributes.Add("class", "active");
        //    }
        //    else
        //    {
        //       // Response.Write("ok");
        //        //Response.End();
        //        myproject.Attributes.Add("class", "");
        //        usertask.Attributes.Add("class", "");
        //        myissues.Attributes.Add("class", "");
        //        userdashboard.Attributes.Add("class", "");
        //        dashboard.Attributes.Add("class", "active");
        //    }
        //}
        //else
        //{            
            if (currentpage.Contains("/pendingqc/"))
            {
                //if (ischeckView_qc == 1 || ischeckCreate_qc == 1 || ischeckEdit_qc == 1 || ischeckDelete_qc == 1 || ischeckAssign_qc == 1)
                //{
                userdashboard.Attributes.Add("class", "");
                dashboard.Attributes.Add("class", "");
                myproject.Attributes.Add("class", "");
                    usertask.Attributes.Add("class", "");
                    myissues.Attributes.Add("class", "");
                    myqc.Attributes.Add("class", "active");
               // }
            }
            else if (currentpage.Contains("/userdashboard/"))
            {
                myproject.Attributes.Add("class", "");
                myqc.Attributes.Add("class", "");
                usertask.Attributes.Add("class", "");
                myissues.Attributes.Add("class", "");
                dashboard.Attributes.Add("class", "");
                userdashboard.Attributes.Add("class", "active");
            }
            else if (currentpage.Contains("/newuserdashboard/"))
            {
                myproject.Attributes.Add("class", "");
                myqc.Attributes.Add("class", "");
                usertask.Attributes.Add("class", "");
                myissues.Attributes.Add("class", "");
                dashboard.Attributes.Add("class", "");
                userdashboard.Attributes.Add("class", "");
                newuserdashboard.Attributes.Add("class", "active");
            }
            else if (currentpage.Contains("/users/"))
            {
                string taskType = (Request.QueryString["type"] ?? "mytask")
                    .Trim()
                    .ToLowerInvariant();
                dashboard.Attributes.Add("class", "");
                myproject.Attributes["class"] = "";
                myqc.Attributes["class"] = "";
                userdashboard.Attributes.Add("class", "");
                myissues.Attributes.Add("class", "");
                if (taskType == "delayedtask")
                {
                    usertask.Attributes["class"] = "";
                    userdelayedtask.Attributes["class"] = "active";
                }
                else
                {
                    // Default is My Task
                    usertask.Attributes["class"] = "active";
                    userdelayedtask.Attributes["class"] = "";
                }
            }
            else if (currentpage.Contains("/myteamtask/"))
            {
                dashboard.Attributes.Add("class", "");
                myproject.Attributes.Add("class", "");
                usertask.Attributes.Add("class", "");
                myqc.Attributes.Add("class", "");
                myissues.Attributes.Add("class", "");
                myteam.Attributes.Add("class", "active");
            }
            else if (currentpage.Contains("/issues/"))
            {
                dashboard.Attributes.Add("class", "");
                myproject.Attributes.Add("class", "");
                usertask.Attributes.Add("class", "");
                userdashboard.Attributes.Add("class", "");
                myqc.Attributes.Add("class", "");
                myteam.Attributes.Add("class", "");
                myissues.Attributes.Add("class", "active");
                
            }
            else if (currentpage.Contains("/projects/"))
        {

            dashboard.Attributes.Add("class", "");
            userdashboard.Attributes.Add("class", "");
            myqc.Attributes["class"] = "";
            userdashboard.Attributes.Add("class", "");
            myissues.Attributes.Add("class", "");
            usertask.Attributes["class"] = "";
            userdelayedtask.Attributes["class"] = "";
            myproject.Attributes["class"] = "active";

        }
            else
            {
            if (currentpage.Contains("userdashboard"))
            {
                myproject.Attributes.Add("class", "");
                myqc.Attributes.Add("class", "");
                usertask.Attributes.Add("class", "");
                myissues.Attributes.Add("class", "");
                dashboard.Attributes.Add("class", "");
                userdashboard.Attributes.Add("class", "active");
            }
            else if (currentpage.Contains("newuserdashboard"))
            {
                myproject.Attributes.Add("class", "");
                myqc.Attributes.Add("class", "");
                usertask.Attributes.Add("class", "");
                myissues.Attributes.Add("class", "");
                dashboard.Attributes.Add("class", "");
                userdashboard.Attributes.Add("class", "");
                newuserdashboard.Attributes.Add("class", "active");
            }
            else if (currentpage.Contains("users"))
            {
                string taskType = (Request.QueryString["type"] ?? "mytask")
                    .Trim()
                    .ToLowerInvariant();
                dashboard.Attributes.Add("class", "");
                myproject.Attributes["class"] = "";
                myqc.Attributes["class"] = "";
                userdashboard.Attributes.Add("class", "");
                myissues.Attributes.Add("class", "");
                if (taskType == "delayedtask")
                {
                    usertask.Attributes["class"] = "";
                    userdelayedtask.Attributes["class"] = "active";
                }
                else
                {
                    // Default is My Task
                    usertask.Attributes["class"] = "active";
                    userdelayedtask.Attributes["class"] = "";
                }
            }
            else if (currentpage.Contains("myteamtask"))
            {
                dashboard.Attributes.Add("class", "");
                myproject.Attributes.Add("class", "");
                usertask.Attributes.Add("class", "");
                myqc.Attributes.Add("class", "");
                myissues.Attributes.Add("class", "");
                myteam.Attributes.Add("class", "active");
            }
            else if (currentpage.Contains("issues"))
            {
                dashboard.Attributes.Add("class", "");
                myproject.Attributes.Add("class", "");
                usertask.Attributes.Add("class", "");
                userdashboard.Attributes.Add("class", "");
                myqc.Attributes.Add("class", "");
                myteam.Attributes.Add("class", "");
                myissues.Attributes.Add("class", "active");

            }
            else if (currentpage.Contains("projects"))
            {

                dashboard.Attributes.Add("class", "");
                userdashboard.Attributes.Add("class", "");
                myqc.Attributes["class"] = "";
                userdashboard.Attributes.Add("class", "");
                myissues.Attributes.Add("class", "");
                usertask.Attributes["class"] = "";
                userdelayedtask.Attributes["class"] = "";
                myproject.Attributes["class"] = "active";

            }
        }
       // }


        if (!IsPostBack)
        {

            getname();
        }
    }
    protected void checkformvalidation(int chkisadmin=0 ,string empid = "",string uid="")
    {
        string strempid = "";
        if (empid != "")
        {
            //+"&mylead="+Cmf.Encrypt("1")
            strempid = "?empid=" + empid;
            //if(Request.QueryString["presearch"]!=null)
            //{
            //    strempid += "&presearch=" + Request.QueryString["presearch"].ToString();
            //}
        }
        else
        {
            empid = uid;
        }
     
        
        string strsqlprofile = @"select emp.id,emp.roleid, Manage_Profile. profile  as profilename from tbl_employee emp  
                                left join Manage_Profile on  emp.roleid= Manage_Profile.id 
                                where emp.status=1 and emp.archive=0  and emp.id='" + empid + "'";
        DataSet dsprofile = da.GetDataSetInline(strsqlprofile);
        strmenudata = "";
        string strdashboard = "";
        if (dsprofile.Tables[0].Rows.Count > 0)
        {
            if (dsprofile.Tables[0].Rows[0]["roleid"].ToString() == "1")
            {
                
                string strsqlchkrole = @"select crm_tbl_roletaskmaster.formname,crm_tbl_roletaskmaster.formdetails,crm_tbl_roletaskmaster.formurl,isnull(crm_tbl_roletaskmaster.ispms,0) as ispms from crm_tbl_roletaskmaster  
                                     left join crm_tbl_moduleshow on crm_tbl_moduleshow.moduleid=crm_tbl_roletaskmaster.id   
                                    where crm_tbl_roletaskmaster.status=1  and isshow=1  and entrytype='form' and crm_tbl_moduleshow.regid='" + Request.Cookies["cadmin"]["id"] + "'  and crm_tbl_roletaskmaster.ispms=1  order by orderno";
                DataSet dsmodulchkrole = da.GetDataSetInline(strsqlchkrole);
                if (dsmodulchkrole.Tables[0].Rows.Count > 0)
                {
                    isdashboard = 1;
                    isproject = 1;
                    ismytask = 1;
                    isdelayedtask = 1;
                    isteamwork = 1;
                    isqc = 1;
                    isissue = 1;
                }
            }
            else
            {

                string strsqlchkrole = @"select crm_tbl_roletaskmaster.formname,crm_tbl_roletaskmaster.formdetails,crm_tbl_roletaskmaster.formurl,isnull(crm_tbl_roletaskmaster.ispms,0) as ispms, isnull(ismanage,0) ismanage,isnull(isschedule,0) isschedule,  isnull(ischangeowner,0) ischangeowner,isnull(formaccess,0) formaccess, isnull(isinsert,0) isinsert, isnull(isupdate,0) isupdate, isnull(isdelete,0) isdelete, isnull(isview,0) isview, isnull(isimport,0) isimport, isnull(isexport,0) isexport, isnull(issendmail,0) issendmail, isnull(isviewmail,0) isviewmail, isnull(isdelmail,0) isdelmail, isnull(isfull,0) isfull, isnull(crm_tbl_roleaccessdetails.moduleid,0) moduleid  
                                    from crm_tbl_roleaccessdetails inner join crm_tbl_roletaskmaster on crm_tbl_roleaccessdetails.moduleid=crm_tbl_roletaskmaster.id 
                                   left join crm_tbl_moduleshow on crm_tbl_moduleshow.moduleid=crm_tbl_roleaccessdetails.moduleid   
                                    where crm_tbl_roleaccessdetails.status=1 and crm_tbl_roletaskmaster.status=1 and crm_tbl_roletaskmaster.ispms=1 and    crm_tbl_roleaccessdetails.roleid='" + dsprofile.Tables[0].Rows[0]["roleid"].ToString() + "' and crm_tbl_moduleshow.regid='" + Request.Cookies["cadmin"]["id"] + "' and isshow=1 and entrytype='form' order by orderno";

                //Response.Write(strsqlchkrole);
                //Response.End();
                DataSet dsmodulchkrole = da.GetDataSetInline(strsqlchkrole);
                if (dsmodulchkrole.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsmodulchkrole.Tables[0].Rows.Count; i++)
                    {
                        if (dsmodulchkrole.Tables[0].Rows[i]["formaccess"].ToString() == "1" || dsmodulchkrole.Tables[0].Rows[i]["formaccess"].ToString() == "True")
                        {
                            if (dsmodulchkrole.Tables[0].Rows[i]["formdetails"].ToString() == "Dashboard")
                            {
                                
                                    isdashboard = 1;
                               
                            }
                            else
                            {
                                if (dsmodulchkrole.Tables[0].Rows[i]["formdetails"].ToString() == "Project")
                                {
                                    isproject = 1;
                                }
                                else if (dsmodulchkrole.Tables[0].Rows[i]["formdetails"].ToString() == "My Task")
                                {
                                    ismytask = 1;
                                }
                                else if (dsmodulchkrole.Tables[0].Rows[i]["formdetails"].ToString() == "Delayed Task")
                                {
                                    isdelayedtask = 1;
                                }
                                else if (dsmodulchkrole.Tables[0].Rows[i]["formdetails"].ToString() == "Team Work")
                                {
                                    isteamwork = 1;
                                }
                                else if (dsmodulchkrole.Tables[0].Rows[i]["formdetails"].ToString() == "QC")
                                {
                                    isqc = 1;
                                }
                                else if (dsmodulchkrole.Tables[0].Rows[i]["formdetails"].ToString() == "Issues")
                                {
                                    isissue = 1;
                                }
                            }
                        }
                    }
                }
                //Response.Write(isdashboard);
                //Response.End();
            }
        }
    }
    protected int checkadmin(string struserid = "0")
    {
        int retval = 0;
        string strsql = "select Manage_Profile.* from Manage_Profile inner join tbl_employee on Manage_Profile.id=cast(tbl_employee.roleid as int) where tbl_employee.id='" + struserid + "'  and Manage_Profile.status=1 and Manage_Profile.archive=0 ";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["id"].ToString() == "1")
            {
                retval = 1;
            }
        }
        return retval;
    }
    protected int checksingleuser(string struserid = "0")
    {
        int retval = 0;
        try
        {
            SqlParameter[] prmemp = new SqlParameter[2];
            prmemp[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"].ToString());
            prmemp[1] = new SqlParameter("@uid", struserid);
            DataSet ds = da.GetDataSet("pms_singleuser_sp", prmemp);
            if (ds.Tables[0].Rows.Count==0)
            {
              
             retval = 1;
               
            }
        }
        catch { retval = 0; }
        return retval;
    }
    public static string get_EditDelete_Validation(string moduleid)
    {
        string retval = "";
        string strFull = "0";
        string strView = "0";
        string strCreate = "0";
        string strEdit = "0";
        string strDelete = "0";
        string strChangeOwner = "0";
        string strAssign = "0";
        DataAccess da = new DataAccess();
        try
        {
            string strsql = "select Manage_Profile.* from Manage_Profile inner join tbl_employee on Manage_Profile.id=cast(tbl_employee.roleid as int) where tbl_employee.id='" + HttpContext.Current.Request.Cookies["user"]["user_id"] + "'  and Manage_Profile.status=1 and Manage_Profile.archive=0";
            DataSet ds = da.GetDataSetInline(strsql);

            if (ds.Tables[0].Rows.Count > 0)
            {

                if (ds.Tables[0].Rows[0]["id"].ToString() == "1")
                {

                    strFull = "1";
                    strView = "1";
                    strCreate = "1";
                    strEdit = "1";
                    strDelete = "1";
                    strChangeOwner = "1";
                    strAssign = "1";
                }
                else
                {
                    string strsqlmodule = @"select crm_tbl_roletaskmaster.formname,crm_tbl_roletaskmaster.formurl, isnull(ismanage,0) ismanage,isnull(isschedule,0) isschedule,  isnull(ischangeowner,0) ischangeowner,
                            isnull(formaccess,0) formaccess, isnull(isinsert,0) isinsert, isnull(isupdate,0) isupdate, isnull(isdelete,0) isdelete, isnull(isview,0) isview, isnull(isimport,0) isimport, isnull(isexport,0) isexport, isnull(issendmail,0) issendmail, isnull(isviewmail,0) isviewmail, isnull(isdelmail,0) isdelmail, isnull(isfull,0) isfull, isnull(moduleid,0) moduleid, isnull(isassign,0) as isassign  
                                    from crm_tbl_roleaccessdetails inner join crm_tbl_roletaskmaster on crm_tbl_roleaccessdetails.moduleid=crm_tbl_roletaskmaster.id 
                                    where crm_tbl_roleaccessdetails.status=1 and  crm_tbl_roleaccessdetails.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and crm_tbl_roleaccessdetails.roleid='" + ds.Tables[0].Rows[0]["id"].ToString() + "' and entrytype='form' and moduleid=" + moduleid + "";

                   
                    DataSet dsmodule = da.GetDataSetInline(strsqlmodule);
                    if (dsmodule.Tables[0].Rows.Count > 0)
                    {
                        if (dsmodule.Tables[0].Rows[0]["isfull"].ToString() == "1" || dsmodule.Tables[0].Rows[0]["isfull"].ToString() == "True")
                        {
                            strFull = "1";
                        }
                        if (strFull == "1")
                        {
                            strView = "1";
                            strCreate = "1";
                            strEdit = "1";
                            strDelete = "1";
                            strChangeOwner = "1";
                            strAssign = "1";
                        }
                        else
                        {
                            if (dsmodule.Tables[0].Rows[0]["isview"].ToString() == "1" || dsmodule.Tables[0].Rows[0]["isview"].ToString() == "True")
                            {
                                strView = "1";
                            }
                            if (dsmodule.Tables[0].Rows[0]["isinsert"].ToString() == "1" || dsmodule.Tables[0].Rows[0]["isinsert"].ToString() == "True")
                            {
                                strCreate = "1";
                            }
                            if (dsmodule.Tables[0].Rows[0]["isupdate"].ToString() == "1" || dsmodule.Tables[0].Rows[0]["isupdate"].ToString() == "True")
                            {
                                strEdit = "1";
                            }
                            if (dsmodule.Tables[0].Rows[0]["isdelete"].ToString() == "1" || dsmodule.Tables[0].Rows[0]["isdelete"].ToString() == "True")
                            {
                                strDelete = "1";
                            }
                            if (dsmodule.Tables[0].Rows[0]["ischangeowner"].ToString() == "1" || dsmodule.Tables[0].Rows[0]["ischangeowner"].ToString() == "True")
                            {
                                strChangeOwner = "1";
                            }
                            if (dsmodule.Tables[0].Rows[0]["isassign"].ToString() == "1" || dsmodule.Tables[0].Rows[0]["isassign"].ToString() == "True")
                            {
                                strAssign = "1";
                            }
                            if (strView == "0" && (strCreate == "1" || strEdit == "1" || strDelete == "1"))
                            {
                                strView = "1";
                            }

                        }

                    }

                }

            }
        }
        catch { }
        retval = strFull + "," + strView + "," + strCreate + "," + strEdit + "," + strDelete + "," + strAssign + "," + strChangeOwner;
        return retval;
    }
    protected void getname()
    {
        string str = "select emp_name,roleid,f_name,l_name,DisplayName from tbl_employee where id='" + Request.Cookies["user"]["user_id"].ToString() + "' and status=1 and archive=0 and isnull(isactivelogin,0)!='2' ";
        DataSet ds = da.GetDataSetInline(str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            lblusername.Text = "Welcome " + ds.Tables[0].Rows[0]["DisplayName"].ToString() + "!";
            lblusername2.Text = "Welcome " + ds.Tables[0].Rows[0]["DisplayName"].ToString() + "!";
        }
        else
        {
            Response.Redirect("/login.aspx");
        }

    }
}
