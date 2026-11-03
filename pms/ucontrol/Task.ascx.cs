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

public partial class ucontrol_Task : System.Web.UI.UserControl
{
    DataSet ds = new DataSet();
    DataAccess da = new DataAccess();
    public CommonFunction cmf = new CommonFunction();
    SqlParameter[] prm, prm1;
    public function fns = new function();
    public string taskdata = "";
    public string MultiOwner = "";
    public string Multiteam = "";
    public string Multiusers = "";
    public string Multiaddteam = "";
    public string Multichangeowner = "";
    public string settimers = "";
    public string setstartf = "";
    public string MultiDependancy = "";
    public int ischeckAllaccess = 0;
    public int ischeckCreate = 0;
    public int ischeckEdit = 0;
    public int ischeckDelete = 0;
    public int ischeckView = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Startdate.Attributes["type"] = "date";
            Startdate.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";

            StartTime.Attributes["type"] = "time";
            StartTime.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";
            Timeline.Attributes["type"] = "time";
            Timeline.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";
            Time.Attributes["type"] = "time";
            Time.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";


            Targetdate.Attributes["type"] = "date";
            Targetdate.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";
            ChooseDate.Attributes["type"] = "date";
            ChooseDate.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";
            Extendatestrt_task.Attributes["type"] = "date";
            Extendatestrt_task.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";
            Extendateend_task.Attributes["type"] = "date";
            Extendateend_task.Attributes["onclick"] = "if(this.showPicker){this.showPicker();}";

        }
        catch
        {

        }
        try
        {
            string strvalidationT = get_EditDelete_Validation("55");
            string[] arT = strvalidationT.Split(',');
            ischeckAllaccess = Convert.ToInt32(arT[0]);
            ischeckView= Convert.ToInt32(arT[1]);
            ischeckCreate= Convert.ToInt32(arT[2]);
            ischeckEdit= Convert.ToInt32(arT[3]);
            ischeckDelete= Convert.ToInt32(arT[4]);

        }
        catch
        {

        }
        if (!Page.IsPostBack)
        {
            columnshowhide();
            bindProject();
           // bindmilestone();
           // bindtasklist();
           // bindemployee();
            bindMultiOwner();
            bindMultiteam();
            bindMultiusers();
            bindprojectstatus();
            bindPriorityType();
            bindStage();
            bindMultichangeOwner();
            bindMultiAddteam();
            bindMultiDependancy();


            try
            {
                if (Request.QueryString["type"] != null)
                {
                    hdntype.Value = "1";
                    hdntypeproject.Value = Request.QueryString["type"].ToString();

                    if (Request.QueryString["type"].ToString() == "Project")
                    {
                        bindProject();
                        ProjectId.SelectedValue = Request.QueryString["id"].ToString();

                        bindmilestone();
                        Milestoneid.SelectedValue = Request.QueryString["milstoneid"].ToString();
                        hdnmilstoneid.Value = Request.QueryString["milstoneid"].ToString();
                    

                        bindtaskdatatimer(ProjectId.SelectedValue, "0");
                    }

                    if (Request.QueryString["type"].ToString() == "milstone")
                    {
                        bindProject();
                        ProjectId.SelectedValue = Request.QueryString["id"].ToString();

                        bindmilestone();
                        Milestoneid.SelectedValue = Request.QueryString["milstoneid"].ToString();
                        hdnmilstoneid.Value = Request.QueryString["milstoneid"].ToString();

                        bindtaskdatatimer(ProjectId.SelectedValue, hdnmilstoneid.Value);

                       

                    }
                   
                 

                }
                else
                {
                    bindtaskdatatimer("0", "0");
                }
            }
            catch
            {

            }

            //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "txt", "start_1('00', '00', '00', '000','')", true);
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
        retval = strFull + "," + strView + "," + strCreate + "," + strEdit + "," + strDelete + "," + strChangeOwner;
        return retval;
    }
    protected void bindtaskdatatimer(string RecordId = "0", string milstoneid = "0")
    {
        hdnbtns.Value = "";
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        SqlParameter[] prm = new SqlParameter[3];
        prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[1] = new SqlParameter("@RecordId", RecordId);
        prm[2] = new SqlParameter("@milestoneid", milstoneid);
        DataSet ds = da.GetDataSet("Pms_Bindtask_dashbord", prm);
        
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
                string strstartdetails = "select *  from Pms_tbl_Timerstatus where taskid='" + ds.Tables[0].Rows[i]["id"].ToString() + "' and isnull(isend,0)=0 and isnull(IsPaused,0)=0  and status=1 and archive=0 and type='task' order by id desc";
                DataSet dsstart = da.GetDataSetInline(strstartdetails);
                if (dsstart.Tables[0].Rows.Count > 0)
                {
                    //Response.Write("ok");
                    //Response.End();
                    if(hdnbtns.Value=="")
                    {
                        hdnbtns.Value = ds.Tables[0].Rows[i]["id"].ToString();
                    }
                    else
                      { hdnbtns.Value +=","+ ds.Tables[0].Rows[i]["id"].ToString(); }
                    settimers += @" function autoplay_" + ds.Tables[0].Rows[i]["id"].ToString() + "(){ document.getElementById('btnauto_" + ds.Tables[0].Rows[i]["id"].ToString() + "').click();} ";
                }
                //Response.Write(settimers);
                // Response.End();
                setstartf += @" function start_"+ ds.Tables[0].Rows[i]["id"].ToString() + "(hours, minutes, seconds, milliseconds,valid) {  ";
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

    protected void bindmilestone()
    {
        string strleadstatus = "select id,Milestone  from Pms_tbl_AddMilestone where ProjectId='" + ProjectId.SelectedValue + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  order by id";
        DataSet ds = da.GetDataSetInline(strleadstatus);
        if (ds.Tables[0].Rows.Count > 0)
        {
            cmf.BindDropDownList(Milestoneid, ds, "Milestone", "id");
            ListItem li1 = new ListItem("", "0");
            Milestoneid.Items.Insert(0, li1);
        }

    }


  

    protected void bindtasklist()
    {
        string strleadstatus = "select id,TaskList  from Pms_tbl_Addtasklist where  ProjectId='" + ProjectId.SelectedValue + "' and Milestone='" + Milestoneid.SelectedValue + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  order by id";
        DataSet ds = da.GetDataSetInline(strleadstatus);
        if (ds.Tables[0].Rows.Count > 0)
        {
            cmf.BindDropDownList(tasklist, ds, "TaskList", "id");
            ListItem li1 = new ListItem("", "0");
            tasklist.Items.Insert(0, li1);
        }

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



    protected void bindStage()
    {
        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"].ToString());
        DataSet dataset = da.GetDataSet("Pms_SP_Select_Stage_reg", prm);
        cmf.BindDropDownList(Stage, dataset, "name", "Id");
        ListItem li = new ListItem("Select Stage", "0");
        Stage.Items.Insert(0, li);
        ListItem li1 = new ListItem("+ Add New", "-1");
        Stage.Items.Add(li1);
    }

    protected void bindMultiOwner()
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

        MultiOwner = dataN.ToString();



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

    protected void bindMultiusers()
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

        Multiusers = dataN.ToString();



    }


    protected void bindMultiDependancy()
    {
        string dataN = "";
        string strsql = "select id, taskname  from Pms_tbl_Addtask where status=1 and archive=0 and isnull(statuscomplete,0) !=3 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by id";

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

    protected void bindemployee()
    {
        string strleadstatus = "select id,DisplayName  from tbl_employee where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  order by id";
        DataSet ds = da.GetDataSetInline(strleadstatus);
        if (ds.Tables[0].Rows.Count > 0)
        {
            cmf.BindDropDownList(owner, ds, "DisplayName", "id");
            ListItem li1 = new ListItem("", "0");
            owner.Items.Insert(0, li1);
        }

    }


    protected void btnload_Click(object sender, EventArgs e)
    {
        bindprojectstatus();
        bindPriorityType();
        bindStage();
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
                    SaveLogText("selecfile=" + filename);
                    if (File.Exists(Server.MapPath("/TaskImages/" + filename)))
                    {
                        goto chk;
                    }
                    Uploaddrawing.SaveAs(Server.MapPath("/TaskImages/" + filename));
                    hdnUploaddrawing.Value = filename;

                }
                else
                {

                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('Please choose only .jpg, .png, .bmp and .gif image types!')", true);


                }
            }
        

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
            Milestoneid.SelectedValue = hdnMilestoneid.Value;
            bindmilestone();
        }
        catch
        {

        }

      


        try
        {
            tasklist.SelectedValue = hdntasklist.Value;
            bindtasklist();
        }
        catch
        {

        }

        //try
        //{
        //    owner.SelectedValue = hdnowner.Value;
        //}
        //catch
        //{

        //}

        try
        {
            Startdate.Value = Convert.ToDateTime(hdnstartdate.Value).ToString("yyyy-MM-dd");
        }
        catch
        {

        }

        try
        {
            Targetdate.Value = Convert.ToDateTime(hdntargetdate.Value).ToString("yyyy-MM-dd");
        }
        catch
        {

        }


        try
        {

            string starttime = hdnstarttime.Value.Replace("AM", "").Replace("PM", "");
            StartTime.Value = Convert.ToDateTime(starttime).TimeOfDay.ToString();
        }
        catch
        {

        }

        try
        {
            
            string strtimeline = hdnTimeline.Value.Replace("AM", "").Replace("PM", "");
            Timeline.Value = Convert.ToDateTime(strtimeline).TimeOfDay.ToString();
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

        //try
        //{
        //    Multiteam = hdnteam.Value;
        //}
        //catch
        //{

        //}

        try
        {
            Reminder.SelectedValue = hdnreminder.Value;
        }
        catch
        {

        }

        try
        {
            ReminderType.SelectedValue = hdnReminderType.Value;
        }
        catch
        {

        }

        try
        {
            //Time.Value = Convert.ToDateTime(hdnTime.Value).ToString("yyyy-MM-dd");
            string strtim = hdnTime.Value.Replace("AM", "").Replace("PM", "");
            Time.Value = Convert.ToDateTime(strtim).TimeOfDay.ToString();
        }
        catch
        {

        }

        try
        {
            Multiusers = hdnNotifyusers.Value;
        }
        catch
        {

        }

        try
        {
            ChooseDate.Value = Convert.ToDateTime(hdnChooseDate.Value).ToString("yyyy-MM-dd");
        }
        catch
        {

        }


        try
        {
            Stage.SelectedValue = hdnstage.Value;
        }
        catch
        {

        }







    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        try
        {
            Extendatestrt_task.Value = Convert.ToDateTime(hdnextndstrt_task.Value).ToString("yyyy-MM-dd");
        }
        catch
        {

        }

        try
        {
            Extendateend_task.Value = Convert.ToDateTime(hdnextndend_task.Value).ToString("yyyy-MM-dd");
        }
        catch
        {

        }

    }

    protected void ProjectId_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindmilestone();
        
    }

    protected void Milestoneid_SelectedIndexChanged(object sender, EventArgs e)
    {
      
    }

    public void columnshowhide()
    {

        HtmlAnchor[] linka = new[] { task0, task1, task2, task3, task4, task5, task6, task7, task8, task9, task10, task11, task12, task13, task14, task15, task16, task17, task18, task19, task20, task21, task22, task23, task24, task25, task26 };

        HiddenField[] hdn = new[] { hdn0, hdn1, hdn2, hdn3, hdn4, hdn5, hdn6, hdn7, hdn8, hdn9, hdn10, hdn11, hdn12, hdn13, hdn14, hdn15, hdn16, hdn17, hdn18, hdn19, hdn20, hdn21, hdn22, hdn23, hdn24, hdn25, hdn26};

        string str = " select * from Pms_tbl_showhide_column where status=1 and archive=0 and uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"] + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and type='task'";
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



    protected void Button2_Click(object sender, EventArgs e)
    {
        if (Uploaddrawing.HasFile)
        {

            string ext = System.IO.Path.GetExtension(this.Uploaddrawing.PostedFile.FileName);
            //Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('"+ext+"')", true);
            if (ext == ".jpg" || ext == ".png" || ext == ".gif" || ext == ".jpeg" || ext == ".bmp")
            {
                string filename = Uploaddrawing.FileName;
                chk:
                filename = filename + "_ta_" + DateTime.Now.ToString("yyyyMMdd_hhmmss");
                SaveLogText("selecfile=" + filename);
                if (File.Exists(Server.MapPath("/TaskImages/" + filename)))
                {
                    goto chk;
                }
                Uploaddrawing.SaveAs(Server.MapPath("/TaskImages/" + filename));
                hdnUploaddrawing.Value = filename;

            }
            else
            {

                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('Please choose only .jpg, .png, .bmp and .gif image types!')", true);


            }
        }
    }
    
}