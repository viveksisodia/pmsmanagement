using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonClass;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using CustomDropDown;
using System.Configuration;
using System.Web.Services;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

public partial class _Default : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataAccess da = new DataAccess();
    public CommonFunction cmf = new CommonFunction();
    SqlParameter[] prm, prm1;
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
    protected void Page_Load(object sender, EventArgs e)
    {
            if (Request.Cookies["user"] == null || Request.Cookies["cadmin"] == null)
            {
                Response.Redirect("/login.aspx"); // Redirect immediately if cookies are missing
            }
      
        //Response.Redirect("Addproject.aspx");
         string uid = Request.Cookies["user"]["user_id"].ToString();
            if (Request.QueryString["empid"] != null)
            {
                uid = cmf.Decrypt(Request.QueryString["empid"]).ToString();
            }

            int isadmin = checkadmin(uid);       
            int issingleuser = checksingleuser(uid);
       // Response.Write(issingleuser);
        //Response.End();
        if (isadmin == 1)
            {
            //Response.Redirect("myprojects.aspx");
             Response.Redirect("/pms/dashboard/");
            }
        else if (isadmin == 0&&issingleuser==0)
        {
            //Response.Redirect("myprojects.aspx");
            Response.Redirect("/pms/userdashboard/");
        }
        else if (isadmin == 0&& issingleuser==1)
            {
            Response.Redirect("/pms/userdashboard/?type=mytask");
            }
            else
            {
            try
            {
                string strvalidationS = get_EditDelete_Validation("53");
                //string strvalidationS = get_EditDelete_Validation("1056");

                string[] arS = strvalidationS.Split(',');
                ischeckAllaccess = Convert.ToInt32(arS[0]);
                ischeckView = Convert.ToInt32(arS[1]);
                ischeckCreate = Convert.ToInt32(arS[2]);
                ischeckEdit = Convert.ToInt32(arS[3]);
                ischeckDelete = Convert.ToInt32(arS[4]);
                ischeckAssign = Convert.ToInt32(arS[5]);
                #region milestone
                string strvalidationM = get_EditDelete_Validation("54");
                //string strvalidationS = get_EditDelete_Validation("1056");
                string[] arM = strvalidationM.Split(',');
                ischeckView_mile = Convert.ToInt32(arM[0]);
                ischeckAllaccess_mile = Convert.ToInt32(arM[1]);
                ischeckCreate_mile = Convert.ToInt32(arM[2]);
                ischeckEdit_mile = Convert.ToInt32(arM[3]);
                ischeckDelete_mile = Convert.ToInt32(arM[4]);
                ischeckAssign_mile = Convert.ToInt32(arM[5]);
                #endregion
                #region task
                string strvalidationS_task = get_EditDelete_Validation("55");
                // string strvalidationS_task = get_EditDelete_Validation("1057");
                string[] arS_task = strvalidationS_task.Split(',');

                ischeckView_task = Convert.ToInt32(arS_task[0]);
                ischeckAllaccess_task = Convert.ToInt32(arS_task[1]);
                ischeckCreate_task = Convert.ToInt32(arS_task[2]);
                ischeckEdit_task = Convert.ToInt32(arS_task[3]);
                ischeckDelete_task = Convert.ToInt32(arS_task[4]);
                ischeckAssign_task = Convert.ToInt32(arS_task[5]);
                #endregion
                #region sub task
                string strvalidationS_stask = get_EditDelete_Validation("56");
                string[] arS_stask = strvalidationS_stask.Split(',');
                ischeckView_stask = Convert.ToInt32(arS_stask[0]);
                ischeckAllaccess_stask = Convert.ToInt32(arS_stask[1]);
                ischeckCreate_stask = Convert.ToInt32(arS_stask[2]);
                ischeckEdit_stask = Convert.ToInt32(arS_stask[3]);
                ischeckDelete_stask = Convert.ToInt32(arS_stask[4]);
                ischeckAssign_stask = Convert.ToInt32(arS_stask[5]);
                #endregion

            }
            catch
            {

            }
             if (ischeckView == 1 || ischeckCreate == 1 || ischeckEdit == 1 || ischeckDelete == 1 || ischeckAssign == 1|| ischeckView_mile == 1 || ischeckCreate_mile == 1 || ischeckEdit_mile == 1 || ischeckDelete_mile == 1 || ischeckAssign_mile == 1|| ischeckView_task == 1 || ischeckCreate_task == 1 || ischeckEdit_task == 1 || ischeckDelete_task == 1 || ischeckAssign_task == 1 || ischeckView_stask == 1 || ischeckCreate_stask == 1 || ischeckEdit_stask == 1 || ischeckDelete_stask == 1 || ischeckAssign_stask == 1)
                {
                Response.Redirect("/pms/projects/");                 
                }
                else
                {
                    //Response.Redirect("mytaskuser.aspx");
                    Response.Redirect("/pms/users/");
                    
                }


                //if (HttpContext.Current.Request.Cookies["cadmin"].ToString() == "2176")
                //{
                //    Response.Redirect("mytaskuser.aspx");
                //}
                //else
                //{
                //    Response.Redirect("AddprojectAng.aspx");
                //}
            }

        
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
                   
                    SaveLogText("strsqlmoduleA1-" + strsqlmodule);
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
        retval = strFull + "," + strView + "," + strCreate + "," + strEdit + "," + strDelete +","+ strAssign + "," + strChangeOwner;
        return retval;
    }
    public static string SaveLogText(string Message)
    {
        string retval = "";
        CommonFunction cmf1 = new CommonFunction();
        DataAccess da1 = new DataAccess();
        DataSet dschk = new DataSet();

        SqlParameter[] prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@message", Message);
        prm[1] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[1].Direction = ParameterDirection.ReturnValue;
        retval = Convert.ToString(da1.executeDMLQuery("crm_entryLogTable", prm));
        return retval;
    }
    protected int checkadmin(string struserid = "0")
    {
        int retval = 0;
        string strsql = "select Manage_Profile.* from Manage_Profile inner join tbl_employee on Manage_Profile.id=cast(tbl_employee.roleid as int) where tbl_employee.id='" + struserid + "'  and Manage_Profile.status=1 and Manage_Profile.archive=0";
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
            if (ds.Tables[0].Rows.Count == 0)
            {

                retval = 1;

            }
        }
        catch(Exception ex) {
            retval = 0;
            SaveLogText("SingleExe:" + ex.Message);
        }
        return retval;
    }
}