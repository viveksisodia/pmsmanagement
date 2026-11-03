using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
using System.IO;
using CustomDropDown;
using System.Web.Services;

public partial class ucontrol_Milestone : System.Web.UI.UserControl
{
    DataSet ds = new DataSet();
    DataAccess da = new DataAccess();
    public CommonFunction cmf = new CommonFunction();
    SqlParameter[] prm, prm1;
    public function fns = new function();
    public string Milestonedata = "";
    public string Multiteam = "";
    public string MultiOwner = "";
    public string Multichangeowner = "";
    public string Multiaddteam = "";
    public string MultiDependancy = "";

    public string settimers = "";
    public string setstartf = "";
    public int ischeckAllaccess = 0;
    public int ischeckCreate = 0;
    public int ischeckEdit = 0;
    public int ischeckDelete = 0;
    public int ischeckView = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            StartDate.Attributes["type"] = "date";
            StartDate.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";

            EndDate.Attributes["type"] = "date";
            EndDate.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";
        }
        catch
        {

        }
        try
        {
            string strvalidationM = get_EditDelete_Validation("54");
            SaveLogText("strvalidationM-" + strvalidationM);
            string[] arM = strvalidationM.Split(',');
            ischeckAllaccess = Convert.ToInt32(arM[0]);
            ischeckView = Convert.ToInt32(arM[1]);
            ischeckCreate = Convert.ToInt32(arM[2]);
            ischeckEdit = Convert.ToInt32(arM[3]);
            ischeckDelete = Convert.ToInt32(arM[4]);

        }
        catch
        {

        }
        if (!Page.IsPostBack)
        {
            columnshowhide();
            bindProject();
           // bindemployee();
            bindMultiteam();
           //  bindMultiOwner();
            bindprojectstatus();
            bindPriorityType();
            bindMultichangeOwner();
            bindMultiAddteam();
            bindMultiDependancy();
           


            try
            {
                if (Request.QueryString["type"] != null)
                {
                    hdntype.Value = "1";

                    if (Request.QueryString["type"].ToString() == "Project")
                    {
                        bindProject();
                        ProjectId.SelectedValue = Request.QueryString["id"].ToString();
                        bindmilestonedatatimer(ProjectId.SelectedValue);
                    }

                    //if (Request.QueryString["type"].ToString() == "milstone")
                    //{
                    //    bindProject();
                    //    ProjectId.SelectedValue = Request.QueryString["id"].ToString();

                    //    bindmilestone();
                    //    Milestoneid.SelectedValue = Request.QueryString["milstoneid"].ToString();
                    //}

                    //if (Request.QueryString["type"].ToString() == "tasklist")
                    //{
                    //    bindProject();
                    //    ProjectId.SelectedValue = Request.QueryString["id"].ToString();

                    //    bindmilestone();
                    //    Milestoneid.SelectedValue = Request.QueryString["milstoneid"].ToString();

                    //    bindtasklist();
                    //    tasklistid.SelectedValue = Request.QueryString["tasklistid"].ToString();

                    //}

                    //if (Request.QueryString["type"].ToString() == "task")
                    //{
                    //    bindProject();
                    //    ProjectId.SelectedValue = Request.QueryString["id"].ToString();

                    //    bindmilestone();
                    //    Milestoneid.SelectedValue = Request.QueryString["milstoneid"].ToString();

                    //    bindtasklist();
                    //    tasklistid.SelectedValue = Request.QueryString["tasklistid"].ToString();

                    //    bindtask();
                    //    taskid.SelectedValue = Request.QueryString["taskid"].ToString();

                    //}


                    //if (Request.QueryString["type"].ToString() == "subtask")
                    //{
                    //    bindProject();
                    //    ProjectId.SelectedValue = Request.QueryString["id"].ToString();

                    //    bindmilestone();
                    //    Milestoneid.SelectedValue = Request.QueryString["milstoneid"].ToString();

                    //    bindtasklist();
                    //    tasklistid.SelectedValue = Request.QueryString["tasklistid"].ToString();

                    //    bindtask();
                    //    taskid.SelectedValue = Request.QueryString["taskid"].ToString();

                    //    bindSubtask();
                    //    subtaskid.SelectedValue = Request.QueryString["subtaskid"].ToString();

                    //}

                }

                else
                {
                    bindmilestonedatatimer("0");
                }

            }
            catch
            {

            }


        }
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
    public static string get_EditDelete_Validation(string moduleid)
    {
        string retval = "";
        string strFull = "0";
        string strView = "0";
        string strCreate = "0";
        string strEdit = "0";
        string strDelete = "0";
        string strChangeOwner = "0";
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
                }
                else
                {
                    string strsqlmodule = @"select crm_tbl_roletaskmaster.formname,crm_tbl_roletaskmaster.formurl, isnull(ismanage,0) ismanage,isnull(isschedule,0) isschedule,  isnull(ischangeowner,0) ischangeowner,
                            isnull(formaccess,0) formaccess, isnull(isinsert,0) isinsert, isnull(isupdate,0) isupdate, isnull(isdelete,0) isdelete, isnull(isview,0) isview, isnull(isimport,0) isimport, isnull(isexport,0) isexport, isnull(issendmail,0) issendmail, isnull(isviewmail,0) isviewmail, isnull(isdelmail,0) isdelmail, isnull(isfull,0) isfull, isnull(moduleid,0) moduleid  
                                    from crm_tbl_roleaccessdetails inner join crm_tbl_roletaskmaster on crm_tbl_roleaccessdetails.moduleid=crm_tbl_roletaskmaster.id 
                                    where crm_tbl_roleaccessdetails.status=1 and  crm_tbl_roleaccessdetails.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and crm_tbl_roleaccessdetails.roleid='" + ds.Tables[0].Rows[0]["id"].ToString() + "' and entrytype='form' and moduleid=" + moduleid + "";
                    //SaveLogText("strsqlmoduleA1-" + strsqlmodule);
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
        retval = strFull + "," + strView + "," + strCreate + "," + strEdit + "," + strDelete + "," + strChangeOwner;
        return retval;
    }
    public static string get_EditDelete_Validationold(string moduleid)
    {
        string retval = "";
        string strFull = "0";
        string strView = "0";
        string strCreate = "0";
        string strEdit = "0";
        string strDelete = "0";
        string strChangeOwner = "0";
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
                }
                else
                {
                    string strsqlmodule = "select * from tbl_rolepermisionsdetails where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and profileid='" + ds.Tables[0].Rows[0]["id"].ToString() + "' and moduleid=" + moduleid + "";

                    DataSet dsmodule = da.GetDataSetInline(strsqlmodule);
                    if (dsmodule.Tables[0].Rows.Count > 0)
                    {
                        strFull = dsmodule.Tables[0].Rows[0]["fullaccesspermision"].ToString();
                        if (strFull == "1")
                        {
                            strView = "1";
                            strCreate = "1";
                            strEdit = "1";
                            strDelete = "1";
                            strChangeOwner = "1";
                        }
                        else
                        {
                            strView = dsmodule.Tables[0].Rows[0]["viewpermision"].ToString();
                            strCreate = dsmodule.Tables[0].Rows[0]["Createpermision"].ToString();
                            strEdit = dsmodule.Tables[0].Rows[0]["Editpermision"].ToString();
                            strDelete = dsmodule.Tables[0].Rows[0]["Deletepermision"].ToString();
                            strChangeOwner = dsmodule.Tables[0].Rows[0]["Assignownerpermision"].ToString();
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
        retval = strFull+","+strView + "," + strCreate + "," + strEdit + "," + strDelete + "," + strChangeOwner;
        return retval;
    }
    protected void bindmilestonedatatimer(string RecordId = "0")
    {
        hdnbtns.Value = "";
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        SqlParameter[] prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[1] = new SqlParameter("@RecordId", RecordId);
       DataSet ds = da.GetDataSet("Pms_Bindmile_timer_dashbord", prm);

        if (ds.Tables[0].Rows.Count > 0)
        {
            //Response.Write(ds.Tables[0].Rows.Count);
            // Response.End();
            settimers = "";
            setstartf = "";
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                settimers += @"var cron_" + ds.Tables[0].Rows[i]["id"].ToString() + ";  function timer_" + ds.Tables[0].Rows[i]["id"].ToString() + "(ctrlhours,ctrlminutes,ctrlseconds,ctrlmilliseconds,millisecond1,second1,minute1,hour1) {";
                settimers += @" if ((millisecond1 += 10) == 1000) {millisecond1 = 0;second1++;}
                  if (second1 == 60) {second1 = 0;minute1++;}
                  if (minute1 == 60) {minute1 = 0;hour1++;}
                  document.getElementById(ctrlhours).innerText = returnData(hour1);
                  document.getElementById(ctrlminutes).innerText = returnData(minute1);
                  document.getElementById(ctrlseconds).innerText = returnData(second1);
                  document.getElementById(ctrlmilliseconds).innerText = returnData(millisecond1);
                }";
                string strstartdetails = "select *  from Pms_tbl_Timerstatus where Milestoneid='" + ds.Tables[0].Rows[i]["id"].ToString() + "' and isnull(isend,0)=0 and isnull(IsPaused,0)=0  and status=1 and archive=0 and type='milestone' order by id desc";
                DataSet dsstart = da.GetDataSetInline(strstartdetails);
                if (dsstart.Tables[0].Rows.Count > 0)
                {
                    //Response.Write("ok");
                    //Response.End();
                    if (hdnbtns.Value == "")
                    {
                        hdnbtns.Value = ds.Tables[0].Rows[i]["id"].ToString().ToString();
                    }
                    else
                    { hdnbtns.Value += "," + ds.Tables[0].Rows[i]["id"].ToString(); }
                    settimers += @" function autoplay_" + ds.Tables[0].Rows[i]["id"].ToString() + "(){ document.getElementById('btnauto_" + ds.Tables[0].Rows[i]["id"].ToString() + "').click();} ";
                }
                //Response.Write(settimers);
                // Response.End();
                setstartf += @" function start_" + ds.Tables[0].Rows[i]["id"].ToString() + "(hours, minutes, seconds, milliseconds,valid) {  ";
                setstartf += @" let hour_" + ds.Tables[0].Rows[i]["id"].ToString() + " = parseInt(document.getElementById(hours.id).innerHTML);";
                setstartf += @" let minute_" + ds.Tables[0].Rows[i]["id"].ToString() + " = parseInt(document.getElementById(minutes.id).innerHTML);";
                setstartf += @" let second_" + ds.Tables[0].Rows[i]["id"].ToString() + " = parseInt(document.getElementById(seconds.id).innerHTML);";
                setstartf += @" let millisecond_" + ds.Tables[0].Rows[i]["id"].ToString() + " = parseInt(document.getElementById(milliseconds.id).innerHTML);  pause(cron_" + ds.Tables[0].Rows[i]["id"].ToString() + ");";

                setstartf += @" cron_" + ds.Tables[0].Rows[i]["id"].ToString() + " = setInterval(() => { timer_" + ds.Tables[0].Rows[i]["id"].ToString() + "(hours.id,minutes.id,seconds.id,milliseconds.id,parseInt(document.getElementById(milliseconds.id).innerHTML),parseInt(document.getElementById(seconds.id).innerHTML),parseInt(document.getElementById(minutes.id).innerHTML),parseInt(document.getElementById(hours.id).innerHTML)); }, 10);}";
                //.RegisterStartupScript(GetType(), "Javascript", "javascript:FUNCTIONNAME(); ", true);

            }
        }




    }



    protected void bindProject()
    {
        string strleadstatus = "select id,ProjectName  from Pms_tbl_AddProject where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  order by id";
        DataSet ds = da.GetDataSetInline(strleadstatus);
        if (ds.Tables[0].Rows.Count > 0)
        {
            cmf.BindDropDownList(ProjectId, ds, "ProjectName", "id");
            ListItem li1 = new ListItem("", "0");
            ProjectId.Items.Insert(0, li1);
        }


    }

    protected void bindemployee()
    {
        string strleadstatus = "select id,DisplayName  from tbl_employee where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  order by id";
        DataSet ds = da.GetDataSetInline(strleadstatus);
        if (ds.Tables[0].Rows.Count > 0)
        {
            cmf.BindDropDownList(Owner, ds, "DisplayName", "id");
            ListItem li1 = new ListItem("", "0");
            Owner.Items.Insert(0, li1);
        }

    }


    protected void bindMultiteam()
    {
        string dataN = "";
        string strsql = "select id, DisplayName   from tbl_employee where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by id";

        DataSet dsN = da.GetDataSetInline(strsql);

        if (dsN.Tables[0].Rows.Count > 0)
        {

            for (int i1 = 0; i1 < dsN.Tables[0].Rows.Count; i1++)
            {
                if (dataN != string.Empty)
                {
                    dataN += ",{";
                    dataN += "id: '" + dsN.Tables[0].Rows[i1]["id"].ToString() + "',";
                    dataN += "text: '" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "'";
                    dataN += "}";
                }
                else
                {
                    dataN += "{";
                    dataN += "id: '" + dsN.Tables[0].Rows[i1]["id"].ToString() + "',";
                    dataN += "text: '" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "'";
                    dataN += "}";
                }
            }

        }

        Multiteam = dataN.ToString();



    }

    //protected void bindMultiOwner()
    //{
    //    string dataN = "";
    //    string strsql = "select id, DisplayName   from tbl_employee where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by id";

    //    DataSet dsN = da.GetDataSetInline(strsql);

    //    if (dsN.Tables[0].Rows.Count > 0)
    //    {

    //        for (int i1 = 0; i1 < dsN.Tables[0].Rows.Count; i1++)
    //        {
    //            if (dataN != string.Empty)
    //            {
    //                dataN += ",{";
    //                dataN += "id: \"" + dsN.Tables[0].Rows[i1]["id"].ToString() + "\",";
    //                dataN += "text: \"" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "\"";
    //                dataN += "}";
    //            }
    //            else
    //            {
    //                dataN += "{";
    //                dataN += "id: \"" + dsN.Tables[0].Rows[i1]["id"].ToString() + "',";
    //                dataN += "text: '" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "\"";
    //                dataN += "}";
    //            }
    //        }

    //    }

    //    MultiOwner = dataN.ToString();
        

    //}


 
  

    protected void bindMultichangeOwner()
    {
        string dataN = "";
        string strsql = "select id, DisplayName   from tbl_employee where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by id";

        DataSet dsN = da.GetDataSetInline(strsql);

        if (dsN.Tables[0].Rows.Count > 0)
        {

            for (int i1 = 0; i1 < dsN.Tables[0].Rows.Count; i1++)
            {
                if (dataN != string.Empty)
                {
                    dataN += ",{";
                    dataN += "id: '" + dsN.Tables[0].Rows[i1]["id"].ToString() + "',";
                    dataN += "text: '" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "'";
                    dataN += "}";
                }
                else
                {
                    dataN += "{";
                    dataN += "id: '" + dsN.Tables[0].Rows[i1]["id"].ToString() + "',";
                    dataN += "text: '" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "'";
                    dataN += "}";
                }
            }

        }

        Multichangeowner = dataN.ToString();



    }
    protected void bindMultiAddteam()
    {
        string dataN = "";
        string strsql = "select id, DisplayName   from tbl_employee where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by id";

        DataSet dsN = da.GetDataSetInline(strsql);

        if (dsN.Tables[0].Rows.Count > 0)
        {

            for (int i1 = 0; i1 < dsN.Tables[0].Rows.Count; i1++)
            {
                if (dataN != string.Empty)
                {
                    dataN += ",{";
                    dataN += "id: '" + dsN.Tables[0].Rows[i1]["id"].ToString() + "',";
                    dataN += "text: '" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "'";
                    dataN += "}";
                }
                else
                {
                    dataN += "{";
                    dataN += "id: '" + dsN.Tables[0].Rows[i1]["id"].ToString() + "',";
                    dataN += "text: '" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "'";
                    dataN += "}";
                }
            }

        }

        Multiaddteam = dataN.ToString();



    }


    protected void bindMultiDependancy()
    {
        string dataN = "";
        string strsql = "select id, taskname  from Pms_tbl_Addtask where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by id";

        DataSet dsN = da.GetDataSetInline(strsql);

        if (dsN.Tables[0].Rows.Count > 0)
        {

            for (int i1 = 0; i1 < dsN.Tables[0].Rows.Count; i1++)
            {
                if (dataN != string.Empty)
                {
                    dataN += ",{";
                    dataN += "id: '" + dsN.Tables[0].Rows[i1]["id"].ToString() + "',";
                    dataN += "text: '" + dsN.Tables[0].Rows[i1]["taskname"].ToString().Replace("'", "`") + "'";
                    dataN += "}";
                }
                else
                {
                    dataN += "{";
                    dataN += "id: '" + dsN.Tables[0].Rows[i1]["id"].ToString() + "',";
                    dataN += "text: '" + dsN.Tables[0].Rows[i1]["taskname"].ToString().Replace("'", "`") + "'";
                    dataN += "}";
                }
            }

        }

        MultiDependancy = dataN.ToString();



    }


    protected void bindprojectstatus()
    {
        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"].ToString());
        DataSet dataset = da.GetDataSet("Pms_SP_SelectProjectstatus_reg", prm);
        cmf.BindDropDownList(P_Status, dataset, "ProjectStatus", "id");
        ListItem li = new ListItem("Select Status", "0");
        P_Status.Items.Insert(0, li);
        ListItem li1 = new ListItem("+ Add New", "-1");
        P_Status.Items.Add(li1);
    }


    protected void bindPriorityType()
    {
        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"].ToString());
        DataSet dataset = da.GetDataSet("Pms_SP_Select_Priority_Type_reg", prm);
        cmf.BindDropDownList(Priority, dataset, "name", "Id");
        ListItem li = new ListItem("Select Priority", "0");
        Priority.Items.Insert(0, li);
        ListItem li1 = new ListItem("+ Add New", "-1");
        Priority.Items.Add(li1);
    }


    protected void btnload_Click(object sender, EventArgs e)
    {
        bindprojectstatus();
        bindPriorityType();
    }



  
    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            ProjectId.SelectedValue = hdnProjectId.Value;
        }
        catch
        {

        }

        try
        {
            StartDate.Value = Convert.ToDateTime(hdnStartDate.Value).ToString("yyyy-MM-dd");
        }
        catch
        {

        }

        try
        {
            EndDate.Value = Convert.ToDateTime(hdnEndDate.Value).ToString("yyyy-MM-dd");
        }
        catch
        {

        }

        //try
        //{
        //    Owner.SelectedValue = hdnowner.Value;
        //}
        //catch
        //{

        //}

        try
        {
            Access.SelectedValue = hdnAccess.Value;
        }
        catch
        {

        }


        try
        {
            ddldurationtype.SelectedValue = hdndurationtype.Value;
        }
        catch
        {

        }

        try
        {
            Multiteam = hdnteam.Value;
        }
        catch
        {

        }

        try
        {
            P_Status.SelectedValue = hdnP_Status.Value;
        }
        catch
        {

        }

        try
        {
            Priority.SelectedValue = hdnPriority.Value;
        }
        catch
        {

        }


        //bindMultiOwner(ProjectId.SelectedValue);


    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        try
        {
            Extendatestrt.Value = Convert.ToDateTime(hdnextndstrt.Value).ToString("dd/MM/yyyy");
        }
        catch
        {

        }

        try
        {
            Extendateend.Value = Convert.ToDateTime(hdnextndend.Value).ToString("dd/MM/yyyy");
        }
        catch
        {

        }

    }

    public void columnshowhide()
    {

        HtmlAnchor[] linka = new[] { mile0, mile1, mile2, mile3, mile4, mile5, mile6, mile7, mile8, mile9, mile10, mile11, mile12, mile13, mile14, mile15, mile16, mile17, mile18 };

        HiddenField[] hdn = new[] { hdn0, hdn1, hdn2, hdn3, hdn4, hdn5, hdn6, hdn7, hdn8, hdn9, hdn10, hdn11, hdn12, hdn13, hdn14, hdn15, hdn16, hdn17, hdn18 };

        string str = " select * from Pms_tbl_showhide_column where status=1 and archive=0 and uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"] + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and type='milestone'";
        DataSet ds = da.GetDataSetInline(str);
        // Response.Write(str);
        //Response.End();
        if (ds.Tables[0].Rows.Count > 0)
        {

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {


                if (linka[Convert.ToInt32((ds.Tables[0].Rows[i]["columnnameid"].ToString() == "" ? "0" : ds.Tables[0].Rows[i]["columnnameid"].ToString()))] != null)
                {

                    if (ds.Tables[0].Rows[i]["isshow"].ToString() == "0")
                    {
                        linka[Convert.ToInt32((ds.Tables[0].Rows[i]["columnnameid"].ToString() == "" ? "0" : ds.Tables[0].Rows[i]["columnnameid"].ToString()))].Attributes.Add("class", "dropdown-item toggle-vis text-mute");
                        hdn[Convert.ToInt32((ds.Tables[0].Rows[i]["columnnameid"].ToString() == "" ? "0" : ds.Tables[0].Rows[i]["columnnameid"].ToString()))].Value = "0";
                    }

                }
            }



        }

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "txt", "hidecolumn(); hidecolumngrid();", true);



    }
    protected void btnuploadimg_Click(object sender, EventArgs e)
    {

        if (Uploaddrawing.HasFile)
        {

            string ext = System.IO.Path.GetExtension(this.Uploaddrawing.PostedFile.FileName);
            //Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('"+ext+"')", true);
            if (ext == ".jpg" || ext == ".png" || ext == ".gif" || ext == ".jpeg" || ext == ".bmp")
            {
                string filename = Uploaddrawing.FileName;
                chk:
                filename = filename + "_t_" + DateTime.Now.ToString("yyyyMMdd_hhmmss");
                 if (File.Exists(Server.MapPath("/MileImages/" + filename)))
                {
                    goto chk;
                }
                Uploaddrawing.SaveAs(Server.MapPath("/MileImages/" + filename));
                hdnUploaddrawing.Value = filename;

            }
            else
            {

                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('Please choose only .jpg, .png, .bmp and .gif image types!')", true);


            }
        }


    }
    protected void bindmenudatasetting()
    {
        ViewState["isadmin"] = 0;
        string strsql = "select Manage_Profile.* from Manage_Profile inner join tbl_employee on Manage_Profile.id=cast(tbl_employee.roleid as int) where tbl_employee.id='" + Request.Cookies["user"]["user_id"] + "'  and Manage_Profile.status=1 and Manage_Profile.archive=0";
        DataSet ds = da.GetDataSetInline(strsql);

        if (ds.Tables[0].Rows.Count > 0)
        {

            if (ds.Tables[0].Rows[0]["id"].ToString() == "1")
            {
                ViewState["isadmin"] = 1;
                ischeckAllaccess = 1;
                ischeckCreate = 1;
                ischeckEdit = 1;
                ischeckDelete = 1;
                ischeckView = 1;


            }
            else
            {
                ViewState["isadmin"] = 0;
                string strsqlmodule = "select * from tbl_rolepermisionsdetails where regid='" + Request.Cookies["cadmin"]["id"].ToString() + "' and profileid='" + ds.Tables[0].Rows[0]["id"].ToString() + "' and moduleid=455";
                DataSet dsmodule = da.GetDataSetInline(strsqlmodule);
                if (dsmodule.Tables[0].Rows.Count > 0)
                {

                    string chkfullcontrolCustomersvalue = "0";
                    string CheckBox1Customersvalue = "0";
                    string CheckBox2Customersvalue = "0";
                    string CheckBox3Customersvalue = "0";
                    string CheckBox4Customersvalue = "0";
                    string CheckBox5Customersvalue = "0";
                    chkfullcontrolCustomersvalue = dsmodule.Tables[0].Rows[0]["fullaccesspermision"].ToString();
                    CheckBox1Customersvalue = dsmodule.Tables[0].Rows[0]["viewpermision"].ToString();
                    CheckBox2Customersvalue = dsmodule.Tables[0].Rows[0]["Createpermision"].ToString();
                    CheckBox3Customersvalue = dsmodule.Tables[0].Rows[0]["Editpermision"].ToString();
                    CheckBox4Customersvalue = dsmodule.Tables[0].Rows[0]["Deletepermision"].ToString();
                    CheckBox5Customersvalue = dsmodule.Tables[0].Rows[0]["Assignownerpermision"].ToString();
                    if (chkfullcontrolCustomersvalue == "1")
                    {
                        ischeckAllaccess = 1;
                        ischeckCreate = 1;
                        ischeckEdit = 1;
                        ischeckDelete = 1;
                        ischeckView = 1;
                    }
                    else
                    {
                        if (CheckBox1Customersvalue == "1")
                        {
                            ischeckView = 1;
                        }
                        if (CheckBox2Customersvalue == "1")
                        {
                            ischeckCreate = 1;
                        }
                        if (CheckBox3Customersvalue == "1")
                        {
                            ischeckEdit = 1;
                        }
                        if (CheckBox4Customersvalue == "1")
                        {
                            ischeckDelete = 1;
                        }
                    }


                }

            }

        }



    }

}

