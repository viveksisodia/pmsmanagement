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

public partial class Addissues : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataAccess da = new DataAccess();
    public CommonFunction cmf = new CommonFunction();
    SqlParameter[] prm, prm1;
    public function fns = new function();
    public string issuesdata = "";
  
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string strvalidationS = get_EditDelete_Validation("57");
            string[] arS = strvalidationS.Split(',');
           
            if (arS[0] =="0" && arS[1] == "0" && arS[2] == "0" && arS[3] == "0" && arS[4] == "0")
            {
                Response.Redirect("/pms/accessdenied.aspx");
            }

        }
        catch
        {

        }
        if (!Page.IsPostBack)
        {

           
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


    //// web method bind using ajax start //

    // Options for the new project issue editor. Uses the same tenant and
    // stored procedures as the existing Issues.ascx dropdowns.
    [WebMethod]
    public static Dictionary<string, List<IssueFormOption>> GetIssueFormOptions()
    {
        int regid;
        HttpCookie cookie = HttpContext.Current.Request.Cookies["cadmin"];
        if (cookie == null || !int.TryParse(cookie["id"], out regid) || regid <= 0)
            throw new HttpException(401, "Session expired");

        DataAccess data = new DataAccess();
        Dictionary<string, List<IssueFormOption>> result = new Dictionary<string, List<IssueFormOption>>();
        result["employees"] = IssueOptionsFromTable(data.GetDataSetInline(
            "select id, DisplayName from tbl_employee where status=1 and archive=0 and regid=" + regid + " order by DisplayName"), "id", "DisplayName");
        result["statuses"] = IssueOptionsFromTable(data.GetDataSet("Pms_SP_SelectIssuestatus_reg",
            new SqlParameter[] { new SqlParameter("@regid", regid) }), "id", "issuename");
        result["priorities"] = IssueOptionsFromTable(data.GetDataSet("Pms_SP_Select_Priority_Type_reg",
            new SqlParameter[] { new SqlParameter("@regid", regid) }), "Id", "name");
        result["types"] = IssueOptionsFromTable(data.GetDataSet("Pms_SP_SelectIssuType_reg",
            new SqlParameter[] { new SqlParameter("@regid", regid) }), "Id", "name");
        result["replicabilities"] = IssueOptionsFromTable(data.GetDataSet("Pms_SP_SelectReplicability_reg",
            new SqlParameter[] { new SqlParameter("@regid", regid) }), "Id", "name");

        return result;
    }

    private static List<IssueFormOption> IssueOptionsFromTable(DataSet data, string idColumn, string nameColumn)
    {
        List<IssueFormOption> options = new List<IssueFormOption>();
        if (data == null || data.Tables.Count == 0) return options;
        foreach (DataRow row in data.Tables[0].Rows)
            options.Add(new IssueFormOption { id = Convert.ToString(row[idColumn]), name = Convert.ToString(row[nameColumn]) });
        return options;
    }

    [WebMethod]
    public static string SaveIsues(issuestdata issuesdetails)
    {
        string retval = "0";
        DataAccess da1 = new DataAccess();
        string editid = issuesdetails.id;
        string entrytype = "Insert";
        if (editid != "" && editid != "0")
        {
            entrytype = "Update";
        }
        
        if(issuesdetails.subtaskid!=""&& issuesdetails.subtaskid != "0")
        {
            issuesdetails.Issueon = "subtask";
        }
        else if (issuesdetails.taskid != "" && issuesdetails.taskid != "0")
        {
            issuesdetails.Issueon = "task";
        }
        else if (issuesdetails.Milestoneid != "" && issuesdetails.Milestoneid != "0")
        {
            issuesdetails.Issueon = "milstone";
        }
        else
        {
            issuesdetails.Issueon = "project";
        }
        SqlParameter[] prm = new SqlParameter[30];
        prm[0] = new SqlParameter("@id", editid);
        prm[1] = new SqlParameter("@ProjectId", issuesdetails.ProjectId);
        prm[2] = new SqlParameter("@Milestoneid", issuesdetails.Milestoneid);
        prm[3] = new SqlParameter("@tasklistid", issuesdetails.tasklistid);
        prm[4] = new SqlParameter("@taskid", issuesdetails.taskid);
        prm[5] = new SqlParameter("@subtaskid", issuesdetails.subtaskid);
        prm[6] = new SqlParameter("@Issues", issuesdetails.Issues);
       
        if (issuesdetails.StartDate == string.Empty)
        {
            prm[7] = new SqlParameter("@StartDate", DBNull.Value);
        }
        else
        {
            prm[7] = new SqlParameter("@StartDate", issuesdetails.StartDate);
        }
        if (issuesdetails.EndDate == string.Empty)
        {
            prm[8] = new SqlParameter("@EndDate", DBNull.Value);
        }
        else
        {
            prm[8] = new SqlParameter("@EndDate", issuesdetails.EndDate);
        }

        prm[9] = new SqlParameter("@Owner", issuesdetails.Owner);
        prm[10] = new SqlParameter("@team", issuesdetails.team);
        prm[11] = new SqlParameter("@Issueon", issuesdetails.Issueon);
        prm[12] = new SqlParameter("@Description", issuesdetails.Description);
        prm[13] = new SqlParameter("@Reminder", issuesdetails.Reminder);
        if(issuesdetails.ReminderType==string.Empty)
        {
            prm[14] = new SqlParameter("@ReminderType", DBNull.Value);
        }
        else
        {
            prm[14] = new SqlParameter("@ReminderType", issuesdetails.ReminderType);
        }
        if(issuesdetails.Days==string.Empty)
        {
            prm[15] = new SqlParameter("@Days", DBNull.Value);
        }
        else
        {
            prm[15] = new SqlParameter("@Days", issuesdetails.Days);
        }
        if(issuesdetails.Time== string.Empty)
        {
            prm[16] = new SqlParameter("@Time", DBNull.Value);

        }
        else
        {
            prm[16] = new SqlParameter("@Time", issuesdetails.Time);
        }

        if (issuesdetails.Notifyusers == string.Empty)
        {
            prm[17] = new SqlParameter("@Notifyusers", DBNull.Value);
        }
        else
        {
            prm[17] = new SqlParameter("@Notifyusers", issuesdetails.Notifyusers);
        }

        if (issuesdetails.ChooseDate==string.Empty)
        {
            prm[18] = new SqlParameter("@ChooseDate", DBNull.Value);
        }
        else
        {
            prm[18] = new SqlParameter("@ChooseDate", issuesdetails.ChooseDate);
        }
        if(issuesdetails.AssignTo== string.Empty)
        {
            prm[19] = new SqlParameter("@AssignTo", DBNull.Value);
        }
        else
        {
            prm[19] = new SqlParameter("@AssignTo", issuesdetails.AssignTo);

        }
        prm[20] = new SqlParameter("@IssueStatus", issuesdetails.IssueStatus);
        prm[21] = new SqlParameter("@Priority", issuesdetails.Priority);
        prm[22] = new SqlParameter("@IssueType", issuesdetails.IssueType);
        prm[23] = new SqlParameter("@Replicability", issuesdetails.Replicability);
        prm[24] = new SqlParameter("@UploadImage", issuesdetails.UploadImage);
        prm[25] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[26] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[27] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[28] = new SqlParameter("@type", entrytype);
       // da1.executeDMLQuery("pms_AddiSSUES_usp_bak", prm);
        prm[29] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[29].Direction = ParameterDirection.ReturnValue;
        int retval1 = da1.executeDMLQuery("pms_AddiSSUES_usp", prm);
        return retval = retval1.ToString();
    }


    [WebMethod]
    public static string AddIssustatus(string value = "", string statuscolor = "")
    {

        string retval = "0";
        DataAccess da1 = new DataAccess();

        SqlParameter[] prm = new SqlParameter[5];
        prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[1] = new SqlParameter("@name", value);
        prm[2] = new SqlParameter("@type", "insert");
        prm[3] = new SqlParameter("@PrintColor", "#"+statuscolor);
        prm[4] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[4].Direction = ParameterDirection.ReturnValue;
        int id = da1.executeDMLQuery("Pms_SP_Project_status_insert_reg", prm);
        retval = id.ToString();
        return retval;

    }

    [WebMethod]
    public static string AddPrioritytype(string value = "", string prioritycolor = "")
    {

        string retval = "0";
        DataAccess da1 = new DataAccess();

        SqlParameter[] prm = new SqlParameter[5];
        prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[1] = new SqlParameter("@name", value);
        prm[2] = new SqlParameter("@type", "insert");
        prm[3] = new SqlParameter("@PrintColor", "#"+prioritycolor);
        prm[4] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[4].Direction = ParameterDirection.ReturnValue;
        int id = da1.executeDMLQuery("Pms_SP_Priority_insert_reg", prm);
        retval = id.ToString();

        return retval;

    }

    [WebMethod]
    public static string AddIssustype(string value = "", string Issuescolor = "")
    {

        string retval = "0";
        DataAccess da1 = new DataAccess();

        SqlParameter[] prm = new SqlParameter[5];
        prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[1] = new SqlParameter("@name", value);
        prm[2] = new SqlParameter("@type", "insert");
        prm[3] = new SqlParameter("@PrintColor", "#"+Issuescolor);
        prm[4] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[4].Direction = ParameterDirection.ReturnValue;
        int id = da1.executeDMLQuery("Pms_SP_IssuType_insert_reg", prm);
        retval = id.ToString();
        
        return retval;

    }

    [WebMethod]
    public static string AddReplicability(string value = "",string Replicabilitycolor="")
    {

        string retval = "0";
        DataAccess da1 = new DataAccess();

        SqlParameter[] prm = new SqlParameter[5];
        prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[1] = new SqlParameter("@name", value);
        prm[2] = new SqlParameter("@type", "insert");
        prm[3] = new SqlParameter("@PrintColor", Replicabilitycolor);
        prm[4] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[4].Direction = ParameterDirection.ReturnValue;
        int id = da1.executeDMLQuery("Pms_SP_Replicability_insert_reg", prm);
        retval = id.ToString();
        
        return retval;

    }
    
    [WebMethod]
    public static string Save_Owner(issuestdata issuesdetails)
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string editid = issuesdetails.id;
        string entrytype = "Update";
        if (editid != "" && editid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", editid);
        prm[1] = new SqlParameter("@Owner", issuesdetails.Owner);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pmstbl_IssuesOwnerDeatilsUpdate", prm);

        DataAccess da = new DataAccess();
        string strsql = "select * from Pms_tbl_Addissues where  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and  id='" + editid + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {

            for (int i = 0; i < dscode.Tables[0].Rows.Count; i++)
            {
                string cntownr = "0";
                string usersdetailsowner = "";
                string ownertooltip = "";
                if (dscode.Tables[0].Rows[i]["Owner"].ToString() != "")
                {
                    string[] arcnt = dscode.Tables[0].Rows[i]["Owner"].ToString().Trim().Split(',');
                    cntownr = arcnt.Length.ToString();
                    DataSet dsusers = new DataSet();
                    if (dscode.Tables[0].Rows[i]["Owner"].ToString().Trim() != "")
                    {
                        dsusers = da.GetDataSetInline("select id,DisplayName,fileupload1,SUBSTRING (DisplayName, 1, 2) as splinameone from tbl_employee where id in(" + dscode.Tables[0].Rows[i]["Owner"].ToString().Trim() + ")");
                        if (dsusers.Tables[0].Rows.Count > 3)
                        {
                            cntownr = (Convert.ToInt32(cntownr) - 3).ToString();

                            // owner tool tip //
                            for (int l = 3; l < dsusers.Tables[0].Rows.Count; l++)
                            {
                                if (dsusers.Tables[0].Rows[l]["DisplayName"].ToString() != string.Empty)
                                {
                                    if (ownertooltip != string.Empty)
                                    {
                                        ownertooltip += "," + dsusers.Tables[0].Rows[l]["DisplayName"].ToString();
                                    }
                                    else
                                    {
                                        ownertooltip += dsusers.Tables[0].Rows[l]["DisplayName"].ToString();
                                    }
                                }

                            }

                            for (int k = 0; k < 3; k++)
                            {
                                if (dsusers.Tables[0].Rows[k]["fileupload1"].ToString() != "")
                                {
                                    usersdetailsowner += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title=" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + " data-bs-original-title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "'><img src = '" + dsusers.Tables[0].Rows[k]["fileupload1"] + "' alt='' class='rounded-circle avatar-xxs'></a>";
                                }
                                else
                                {
                                    usersdetailsowner += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover'" +
                 " data-bs-placement='top' title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "' data-bs-original-title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "'>";
                                    usersdetailsowner += @"<div class='avatar-xxs'><div class='avatar-title rounded-circle bg-light text-primary'>" + dsusers.Tables[0].Rows[k]["splinameone"].ToString() + "</div></div>";
                                    usersdetailsowner += @"</a>";
                                }
                            }

                        }
                        else
                        {
                            cntownr = "";
                            for (int k = 0; k < dsusers.Tables[0].Rows.Count; k++)
                            {
                                if (dsusers.Tables[0].Rows[k]["fileupload1"].ToString() != "")
                                {
                                    //img src='/assets/images/avatar-1.jpg'
                                    usersdetailsowner += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='' data-bs-original-title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "'><img src = '" + dsusers.Tables[0].Rows[k]["fileupload1"] + "' alt='' class='rounded-circle avatar-xxs'></a>";
                                }
                                else
                                {
                                    usersdetailsowner += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover'  data-bs-placement='top' title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "' data-bs-original-title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "'>";
                                    usersdetailsowner += @"<div class='avatar-xxs'>
                            <div class='avatar-title rounded-circle bg-light text-primary'>" + dsusers.Tables[0].Rows[k]["splinameone"].ToString() + "</div></div>";
                                    usersdetailsowner += @"</a>";

                                }
                            }
                        }

                    }
                }

                retval += usersdetailsowner;
                retval += "<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='" + ownertooltip + "' data-bs-original-title='more'>";

                if (cntownr != "" && cntownr != "0")
                {
                    retval += "<div class='avatar-xxs'>";
                    retval += "<div class='avatar-title rounded-circle'>";

                    retval += (cntownr + "+");
                    retval += "</div>";
                    retval += "</div>";
                }

                retval += "</a>";




            }


        }
        return retval;


    }

    [WebMethod]
    public static string Binddetail_Owner(int RecordId = 0)
    {

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "select * from Pms_tbl_Addissues where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
        ds = da.GetDataSetInline(strsql);


        return ds.GetXml();
    }


    [WebMethod]
    public static void Savecolumndata(showhide_column columndetails)
    {
        DataAccess da1 = new DataAccess();
        string editid = columndetails.id;
        string entrytype = "Insert";


        SqlParameter[] prm = new SqlParameter[7];
        prm[0] = new SqlParameter("@id", editid);
        prm[1] = new SqlParameter("@mode", entrytype);
        prm[2] = new SqlParameter("@columnnameid", columndetails.columnnameid);
        if (columndetails.isshow.ToString().Trim() == "0")
        {
            prm[3] = new SqlParameter("@isshow", "0");
        }
        else
        {
            prm[3] = new SqlParameter("@isshow", "1");
        }
        prm[4] = new SqlParameter("@type", "issues");
        prm[5] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[6] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
        da1.executeDMLQuery("Pms_tbl_showhide_columnDML", prm);
    }
    public class showhide_column
    {
        public string id { get; set; }
        public string columnnameid { get; set; }
        public string isshow { get; set; }
    }



    [WebMethod]
    public static string Save_Team(issuestdata issuesdetails)
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string editid = issuesdetails.id;
        string entrytype = "Update";
        if (editid != "" && editid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", editid);
        prm[1] = new SqlParameter("@team", issuesdetails.team);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pms_tbl_Issues_TeamDeatilsUpdate", prm);

        DataAccess da = new DataAccess();
        string strsql = "select * from Pms_tbl_Addissues where  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and  id='" + editid + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            // retval = dscode.Tables[0].Rows[0]["DisplayName"].ToString();
            for (int i = 0; i < dscode.Tables[0].Rows.Count; i++)
            {
                string cnt = "0";
                string usersdetails = "";
                string teamtooltip = "";
                if (dscode.Tables[0].Rows[i]["team"].ToString() != "")
                {
                    string[] arcnt = dscode.Tables[0].Rows[i]["team"].ToString().Trim().Split(',');
                    cnt = arcnt.Length.ToString();
                    DataSet dsusers = new DataSet();
                    if (dscode.Tables[0].Rows[i]["team"].ToString().Trim() != "")
                    {
                        dsusers = da.GetDataSetInline("select id,DisplayName,fileupload1,SUBSTRING (DisplayName, 1, 2) as splinameone from tbl_employee where id in(" + dscode.Tables[0].Rows[i]["team"].ToString().Trim() + ")");
                        if (dsusers.Tables[0].Rows.Count > 3)
                        {
                            cnt = (Convert.ToInt32(cnt) - 3).ToString();

                            // team tool tip //
                            for (int l = 3; l < dsusers.Tables[0].Rows.Count; l++)
                            {
                                if (dsusers.Tables[0].Rows[l]["DisplayName"].ToString() != string.Empty)
                                {
                                    if (teamtooltip != string.Empty)
                                    {
                                        teamtooltip += "," + dsusers.Tables[0].Rows[l]["DisplayName"].ToString();
                                    }
                                    else
                                    {
                                        teamtooltip += dsusers.Tables[0].Rows[l]["DisplayName"].ToString();
                                    }
                                }

                            }

                            for (int k = 0; k < 3; k++)
                            {
                                if (dsusers.Tables[0].Rows[k]["fileupload1"].ToString() != "")
                                {
                                    usersdetails += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title=" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + " data-bs-original-title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "'><img src = '" + dsusers.Tables[0].Rows[k]["fileupload1"] + "' alt='' class='rounded-circle avatar-xxs'></a>";
                                }
                                else
                                {
                                    usersdetails += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover'" +
                 " data-bs-placement='top' title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "' data-bs-original-title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "'>";
                                    usersdetails += @"<div class='avatar-xxs'><div class='avatar-title rounded-circle bg-light text-primary'>" + dsusers.Tables[0].Rows[k]["splinameone"].ToString() + "</div></div>";
                                    usersdetails += @"</a>";
                                }
                            }

                        }
                        else
                        {
                            cnt = "";
                            for (int k = 0; k < dsusers.Tables[0].Rows.Count; k++)
                            {
                                if (dsusers.Tables[0].Rows[k]["fileupload1"].ToString() != "")
                                {
                                    //img src='/assets/images/avatar-1.jpg'
                                    usersdetails += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='' data-bs-original-title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "'><img src = '" + dsusers.Tables[0].Rows[k]["fileupload1"] + "' alt='' class='rounded-circle avatar-xxs'></a>";
                                }
                                else
                                {
                                    usersdetails += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover'  data-bs-placement='top' title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "' data-bs-original-title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "'>";
                                    usersdetails += @"<div class='avatar-xxs'>
                            <div class='avatar-title rounded-circle bg-light text-primary'>" + dsusers.Tables[0].Rows[k]["splinameone"].ToString() + "</div></div>";
                                    usersdetails += @"</a>";

                                }
                            }
                        }

                    }
                }

                retval += usersdetails;
                retval += "<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='" + teamtooltip + "' data-bs-original-title='more'>";

                if (cnt != "" && cnt != "0")
                {
                    retval += "<div class='avatar-xxs'>";
                    retval += "<div class='avatar-title rounded-circle'>";

                    retval += (cnt + "+");
                    retval += "</div>";
                    retval += "</div>";
                }

                retval += "</a>";




            }


        }
        return retval;

    }


    [WebMethod]
    public static string Bindetail_Team(int RecordId = 0)
    {

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "select * from Pms_tbl_Addissues where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
        ds = da.GetDataSetInline(strsql);


        return ds.GetXml();
    }



    [WebMethod]
    public static string Update_Status(string issuesid = "", string statusid = "")
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (issuesid != "" && issuesid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", issuesid);
        prm[1] = new SqlParameter("@ProjectStatus", statusid);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pmstbl_Issues_Status_DeatilsUpdate", prm);


        DataAccess da = new DataAccess();
        string strsql = "select id,name,PrintColor from Pms_tbl_addIssueStatus where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + statusid + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            retval = dscode.Tables[0].Rows[0]["name"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
        }

        return retval;
    }

    [WebMethod]
    public static string UpdatePriority_Status(string issuesid = "", string Propertyid = "")
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (issuesid != "" && issuesid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", issuesid);
        prm[1] = new SqlParameter("@Priority", Propertyid);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pms_tbl_issues_Priority_DeatilsUpdate", prm);

        DataAccess da = new DataAccess();
        string strsql = "select Id,name,PrintColor from Pms_tbl_Priority where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + Propertyid + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            retval = dscode.Tables[0].Rows[0]["name"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
        }

        return retval;
    }



    [WebMethod]
    public static string Update_Issutype(string issuesid = "", string IssueTypeid = "")
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (issuesid != "" && issuesid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", issuesid);
        prm[1] = new SqlParameter("@Issutype", IssueTypeid);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pmstbl_Issue_Type_DeatilsUpdate", prm);


        DataAccess da = new DataAccess();
        string strsql = "select id,name,PrintColor from Pms_tbl_IssueType where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + IssueTypeid + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            retval = dscode.Tables[0].Rows[0]["name"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
        }

        return retval;
    }


    [WebMethod]
    public static string UpdateReplicable(string issuesid = "", string Replicableidid = "")
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (issuesid != "" && issuesid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", issuesid);
        prm[1] = new SqlParameter("@Replicable", Replicableidid);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pms_tbl_Replicable_DeatilsUpdate", prm);

        DataAccess da = new DataAccess();
        string strsql = "select Id,name,PrintColor from Pms_tbl_Replicability where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + Replicableidid + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            retval = dscode.Tables[0].Rows[0]["name"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
        }

        return retval;
    }

    [WebMethod]
    public static string bindIssuesdata(string RecordId = "0", string milstoneid = "0", string taskid = "0", string subtaskid = "0", string issuetype="")
    {
        string retval = get_status_listbind();
        string strvalprority = get_Priority_listbind();

        string retvalissutype = get_issutype_listbind();
        string strvalReplicability = get_Replicability_listbind();

        int chkadmin = checkadmin(HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());

        //chkadmin = 1;
        string issuesdata = "";
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        SqlParameter[] prm = new SqlParameter[7];
        prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[1] = new SqlParameter("@RecordId", RecordId);
        prm[2] = new SqlParameter("@milstoneid", milstoneid);
        prm[3] = new SqlParameter("@taskid", taskid);
        prm[4] = new SqlParameter("@Subtaskid", subtaskid);
        prm[5] = new SqlParameter("@issuetype", issuetype);
        if (chkadmin == 0)
        {
            prm[6] = new SqlParameter("@ownerids", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        }
        else
        {
            prm[6] = new SqlParameter("@ownerids", DBNull.Value);
        }
        issuesdata += "<table style='width:100%;' cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm datatable'>";
        issuesdata += "<thead>";
        issuesdata += "<tr>";

        //issuesdata += "<th>Select</th>";
        issuesdata += "<th>S.No</th>";
        issuesdata += "<th style=min-width:200px;>Issues</th>";
        issuesdata += "<th style=min-width:120px;>Owner</th>";
        issuesdata += "<th style=min-width:120px;>Team</th>";
        issuesdata += "<th style=width:130px;>Status</th>";
        issuesdata += "<th>Priority</th>";
        issuesdata += "<th>Tags</th>";
        issuesdata += "<th>Assign To</th>";
        issuesdata += "<th>Create Date</th>";
        issuesdata += "<th>Target Date</th>";
        issuesdata += "<th style=min-width:140px;>Issue Type</th>";
        //issuesdata += "<th style=min-width:140px;>Dependency</th>";
        issuesdata += "<th style=min-width:140px;>Completion Date</th>";
        issuesdata += "<th style=min-width:140px;>Milestone</th>";
        issuesdata += "<th style=min-width:140px;>Replicability</th>";
        issuesdata += "<th>File</th>";
        issuesdata += "<th>Action</th>";
        issuesdata += "</tr>";
        issuesdata += "</thead>";
        issuesdata += "<tbody>";
        try
        {
            DataSet ds = da.GetDataSet("Pms_Bindissues_dashbord", prm);
            
            string milestone_name = "";
            if (ds.Tables[0].Rows.Count > 0)
            {

                string strvalidationI = get_EditDelete_Validation("57");

                string streditI = "0";
                string strdeleteI = "0";
                string strownerI = "0";

                try
                {
                    string[] arrI = strvalidationI.Split(',');
                    streditI = arrI[3];
                    strdeleteI = arrI[4];
                    strownerI = arrI[5];

                }
                catch { }



                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    string cntownr = "0";
                    string usersdetailsowner = "";
                    string ownertooltip = "";
                    if (ds.Tables[0].Rows[i]["Owner"].ToString() != "")
                    {
                        string[] arcnt = ds.Tables[0].Rows[i]["Owner"].ToString().Trim().Split(',');
                        cntownr = arcnt.Length.ToString();
                        DataSet dsusers = new DataSet();
                        if (ds.Tables[0].Rows[i]["Owner"].ToString().Trim() != "")
                        {
                            dsusers = da.GetDataSetInline("select id,DisplayName,fileupload1,SUBSTRING (DisplayName, 1, 2) as splinameone from tbl_employee where id in(" + ds.Tables[0].Rows[i]["Owner"].ToString().Trim() + ")");
                            if (dsusers.Tables[0].Rows.Count > 3)
                            {
                                cntownr = (Convert.ToInt32(cntownr) - 3).ToString();

                                // owner tool tip //
                                for (int l = 3; l < dsusers.Tables[0].Rows.Count; l++)
                                {
                                    if (dsusers.Tables[0].Rows[l]["DisplayName"].ToString() != string.Empty)
                                    {
                                        if (ownertooltip != string.Empty)
                                        {
                                            ownertooltip += "," + dsusers.Tables[0].Rows[l]["DisplayName"].ToString();
                                        }
                                        else
                                        {
                                            ownertooltip += dsusers.Tables[0].Rows[l]["DisplayName"].ToString();
                                        }
                                    }

                                }


                                for (int k = 0; k < 3; k++)
                                {
                                    if (dsusers.Tables[0].Rows[k]["fileupload1"].ToString() != "")
                                    {
                                        usersdetailsowner += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title=" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + " data-bs-original-title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "'><img src = '" + dsusers.Tables[0].Rows[k]["fileupload1"] + "' alt='' class='rounded-circle avatar-xxs'></a>";
                                    }
                                    else
                                    {
                                        usersdetailsowner += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover'" +
                     " data-bs-placement='top' title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "' data-bs-original-title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "'>";
                                        usersdetailsowner += @"<div class='avatar-xxs'><div class='avatar-title rounded-circle bg-light text-primary'>" + dsusers.Tables[0].Rows[k]["splinameone"].ToString() + "</div></div>";
                                        usersdetailsowner += @"</a>";
                                    }
                                }

                            }
                            else
                            {
                                cntownr = "";
                                for (int k = 0; k < dsusers.Tables[0].Rows.Count; k++)
                                {
                                    if (dsusers.Tables[0].Rows[k]["fileupload1"].ToString() != "")
                                    {
                                        //img src='/assets/images/avatar-1.jpg'
                                        usersdetailsowner += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='' data-bs-original-title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "'><img src = '" + dsusers.Tables[0].Rows[k]["fileupload1"] + "' alt='' class='rounded-circle avatar-xxs'></a>";
                                    }
                                    else
                                    {
                                        usersdetailsowner += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover'  data-bs-placement='top' title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "' data-bs-original-title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "'>";
                                        usersdetailsowner += @"<div class='avatar-xxs'>
                            <div class='avatar-title rounded-circle bg-light text-primary'>" + dsusers.Tables[0].Rows[k]["splinameone"].ToString() + "</div></div>";
                                        usersdetailsowner += @"</a>";

                                    }
                                }
                            }

                        }
                    }


                    string cnt = "0";
                    string usersdetails = "";
                    string teamtooltip = "";
                    if (ds.Tables[0].Rows[i]["team"].ToString() != "")
                    {
                        string[] arcnt = ds.Tables[0].Rows[i]["team"].ToString().Trim().Split(',');
                        cnt = arcnt.Length.ToString();
                        DataSet dsusers = new DataSet();
                        if (ds.Tables[0].Rows[i]["team"].ToString().Trim() != "")
                        {
                            dsusers = da.GetDataSetInline("select id,DisplayName,fileupload1,SUBSTRING (DisplayName, 1, 2) as splinameone from tbl_employee where id in(" + ds.Tables[0].Rows[i]["team"].ToString().Trim() + ")");
                            if (dsusers.Tables[0].Rows.Count > 3)
                            {
                                cnt = (Convert.ToInt32(cnt) - 3).ToString();

                                // team tool tip //
                                for (int l = 3; l < dsusers.Tables[0].Rows.Count; l++)
                                {
                                    if (dsusers.Tables[0].Rows[l]["DisplayName"].ToString() != string.Empty)
                                    {
                                        if (teamtooltip != string.Empty)
                                        {
                                            teamtooltip += "," + dsusers.Tables[0].Rows[l]["DisplayName"].ToString();
                                        }
                                        else
                                        {
                                            teamtooltip += dsusers.Tables[0].Rows[l]["DisplayName"].ToString();
                                        }
                                    }

                                }

                                for (int k = 0; k < 3; k++)
                                {
                                    if (dsusers.Tables[0].Rows[k]["fileupload1"].ToString() != "")
                                    {
                                        usersdetails += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title=" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + " data-bs-original-title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "'><img src = '" + dsusers.Tables[0].Rows[k]["fileupload1"] + "' alt='' class='rounded-circle avatar-xxs'></a>";
                                    }
                                    else
                                    {
                                        usersdetails += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover'" +
                     " data-bs-placement='top' title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "' data-bs-original-title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "'>";
                                        usersdetails += @"<div class='avatar-xxs'><div class='avatar-title rounded-circle bg-light text-primary'>" + dsusers.Tables[0].Rows[k]["splinameone"].ToString() + "</div></div>";
                                        usersdetails += @"</a>";
                                    }
                                }

                            }
                            else
                            {
                                cnt = "";
                                for (int k = 0; k < dsusers.Tables[0].Rows.Count; k++)
                                {
                                    if (dsusers.Tables[0].Rows[k]["fileupload1"].ToString() != "")
                                    {
                                        //img src='/assets/images/avatar-1.jpg'
                                        usersdetails += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='' data-bs-original-title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "'><img src = '" + dsusers.Tables[0].Rows[k]["fileupload1"] + "' alt='' class='rounded-circle avatar-xxs'></a>";
                                    }
                                    else
                                    {
                                        usersdetails += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover'  data-bs-placement='top' title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "' data-bs-original-title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "'>";
                                        usersdetails += @"<div class='avatar-xxs'>
                            <div class='avatar-title rounded-circle bg-light text-primary'>" + dsusers.Tables[0].Rows[k]["splinameone"].ToString() + "</div></div>";
                                        usersdetails += @"</a>";

                                    }
                                }
                            }

                        }
                    }



                    issuesdata += "<tr>";
                    issuesdata += "<td valign='top'>";
                    issuesdata += (i + 1);
                    issuesdata += "</td>";

                    issuesdata += "<td valign='top'>";
                    issuesdata += ds.Tables[0].Rows[i]["Issues"].ToString();
                    issuesdata += "</td>";

                    //Owner

                    issuesdata += "<td>";
                    issuesdata += "<div class='d-flex'>";
                    issuesdata += "<div class='flex-grow-1'>";
                    issuesdata += "<div class='avatar-group' id='lblowner_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>";
                    issuesdata += usersdetailsowner;
                    issuesdata += "<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='" + ownertooltip + "' data-bs-original-title='more'>";

                    if (cntownr != "" && cnt != "0")
                    {
                        issuesdata += "<div class='avatar-xxs'>";
                        issuesdata += "<div class='avatar-title rounded-circle'>";

                        issuesdata += (cntownr + "+");
                        issuesdata += "</div>";
                        issuesdata += "</div>";
                    }

                    issuesdata += "</a>";
                    issuesdata += "</div>";
                    issuesdata += "</div>";
                    issuesdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                    issuesdata += "<a href ='javascript:void(0);' style='display:" + (streditI == "1" ? "" : "none") + "' type='button' id='btnownr_" + ds.Tables[0].Rows[i]["id"].ToString() + "' onclick='Binddetails_Owner(" + ds.Tables[0].Rows[i]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#changeowner'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Owner'>+</span></a>";
                    issuesdata += "</div>";
                    issuesdata += "</div>";
                    issuesdata += "</td>";


                    //team


                    issuesdata += "<td>";
                    issuesdata += "<div class='d-flex'>";
                    issuesdata += "<div class='flex-grow-1'>";
                    issuesdata += "<div class='avatar-group' id='lbluser_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>";
                    issuesdata += usersdetails;


                    issuesdata += "<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='" + teamtooltip + "' data-bs-original-title='more'>";

                    if (cnt != "" && cnt != "0")
                    {
                        issuesdata += "<div class='avatar-xxs'>";
                        issuesdata += "<div class='avatar-title rounded-circle'>";

                        issuesdata += (cnt + "+");
                        issuesdata += "</div>";
                        issuesdata += "</div>";
                    }

                    issuesdata += "</a>";
                    issuesdata += "</div>";
                    issuesdata += "</div>";
                    issuesdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                    issuesdata += "<a href ='javascript:void(0);' style='display:" + (streditI == "1" ? "" : "none") + "' type='button' id='btnteam_" + ds.Tables[0].Rows[i]["id"].ToString() + "' onclick='Binddetails_Team(" + ds.Tables[0].Rows[i]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#addteam'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Team'>+</span></a>";
                    issuesdata += "</div>";
                    issuesdata += "</div>";
                    issuesdata += "</td>";

                    // status


                    string strvalcolor = get_Status_Color(ds.Tables[0].Rows[i]["IssueStatus"].ToString());
                    issuesdata += "<td>";
                    issuesdata += "<div class='btn-group d-block'>";
                    if (streditI == "1")
                    {
                        issuesdata += "<a href ='javascript:void(0);' id='btnsts_" + ds.Tables[0].Rows[i]["id"].ToString() + "' type='button' onclick=setissustatusid('" + ds.Tables[0].Rows[i]["id"].ToString() + "',this.id) data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + strvalcolor + "'>" + get_Projectstatus(ds.Tables[0].Rows[i]["IssueStatus"].ToString()) + "</a>";

                    }
                    else
                    {
                        issuesdata += "<a href ='javascript:void(0);' id='btnsts_" + ds.Tables[0].Rows[i]["id"].ToString() + "' type='button' data-bs-toggle='' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + strvalcolor + "'>" + get_Projectstatus(ds.Tables[0].Rows[i]["IssueStatus"].ToString()) + "</a>";

                    }
                    //issuesdata += "<a href ='javascript:void(0);' id='btnsts_" + ds.Tables[0].Rows[i]["id"].ToString() + "' type='button' onclick=setissustatusid('" + ds.Tables[0].Rows[i]["id"].ToString() + "',this.id) data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + strvalcolor + "'>" + get_Projectstatus(ds.Tables[0].Rows[i]["IssueStatus"].ToString()) + "</a>";
                    issuesdata += "<div class='dropdown-menu lead_form_body p-0'>";
                    issuesdata += retval;
                    issuesdata += "</div>";
                    issuesdata += "</div>";
                    issuesdata += "</td>";

                    //priority


                    string strprioritycolor = get_PriorityColor(ds.Tables[0].Rows[i]["Priority"].ToString());
                    issuesdata += "<td>";
                    issuesdata += "<div class='btn-group d-block'>";
                    if (streditI == "1")
                    {
                        issuesdata += "<a href ='javascript:void(0);' id='btnpri_" + ds.Tables[0].Rows[i]["id"].ToString() + "' type='button' onclick=setpriorityid('" + ds.Tables[0].Rows[i]["id"].ToString() + "',this.id) data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + strprioritycolor + "'>" + get_PriorityName(ds.Tables[0].Rows[i]["Priority"].ToString()) + "</a>";

                    }
                    else
                    {
                        issuesdata += "<a href ='javascript:void(0);' id='btnpri_" + ds.Tables[0].Rows[i]["id"].ToString() + "' type='button'  data-bs-toggle='' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + strprioritycolor + "'>" + get_PriorityName(ds.Tables[0].Rows[i]["Priority"].ToString()) + "</a>";

                    }
                    //issuesdata += "<a href ='javascript:void(0);' id='btnpri_" + ds.Tables[0].Rows[i]["id"].ToString() + "' type='button' onclick=setpriorityid('" + ds.Tables[0].Rows[i]["id"].ToString() + "',this.id) data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + strprioritycolor + "'>" + get_PriorityName(ds.Tables[0].Rows[i]["Priority"].ToString()) + "</a>";
                    issuesdata += "<div class='dropdown-menu lead_form_body p-0'>";
                    issuesdata += strvalprority;
                    issuesdata += "</div>";
                    issuesdata += "</div>";
                    issuesdata += "</td>";

                    issuesdata += "<td valign='top'>";
                    issuesdata += ""; //Tags  
                    issuesdata += "</td>";

                    issuesdata += "<td valign='top'>";
                    issuesdata += get_employee(ds.Tables[0].Rows[i]["AssignTo"].ToString()); //Assign to
                    issuesdata += "</td>";

                    issuesdata += "<td valign='top'>";
                    issuesdata += cmf.dateConvert(ds.Tables[0].Rows[i]["StartDate"].ToString()); //Create Date
                    issuesdata += "</td>";

                    issuesdata += "<td valign='top'>";
                    issuesdata += cmf.dateConvert(ds.Tables[0].Rows[i]["EndDate"].ToString()); //target date
                    issuesdata += "</td>";


                    //Issue Type

                    string strissucolor = get_Issutype_Color(ds.Tables[0].Rows[i]["IssueType"].ToString());
                    issuesdata += "<td>";
                    issuesdata += "<div class='btn-group d-block'>";
                    if (streditI == "1")
                    {
                        issuesdata += "<a href ='javascript:void(0);' id='btnissutype_" + ds.Tables[0].Rows[i]["id"].ToString() + "' type='button' onclick=setissutypestatusid('" + ds.Tables[0].Rows[i]["id"].ToString() + "',this.id) data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + strissucolor + "'>" + get_IssueName(ds.Tables[0].Rows[i]["IssueType"].ToString()) + "</a>";

                    }
                    else
                    {
                        issuesdata += "<a href ='javascript:void(0);' id='btnissutype_" + ds.Tables[0].Rows[i]["id"].ToString() + "' type='button'  data-bs-toggle='' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + strissucolor + "'>" + get_IssueName(ds.Tables[0].Rows[i]["IssueType"].ToString()) + "</a>";

                    }
                    //issuesdata += "<a href ='javascript:void(0);' id='btnissutype_" + ds.Tables[0].Rows[i]["id"].ToString() + "' type='button' onclick=setissutypestatusid('" + ds.Tables[0].Rows[i]["id"].ToString() + "',this.id) data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + strissucolor + "'>" + get_IssueName(ds.Tables[0].Rows[i]["IssueType"].ToString()) + "</a>";
                    issuesdata += "<div class='dropdown-menu lead_form_body p-0'>";
                    issuesdata += retvalissutype;
                    issuesdata += "</div>";
                    issuesdata += "</div>";
                    issuesdata += "</td>";



                    //issuesdata += "<td valign='top'>";
                    //issuesdata += ""; //Dependency  
                    //issuesdata += "</td>";

                    issuesdata += "<td valign='top'>";
                    issuesdata += ""; // Completion Date
                    issuesdata += "</td>";

                    issuesdata += "<td valign='top'>";
                    if (ds.Tables[0].Rows[i]["Milestoneid"].ToString() == milestone_name)
                    {
                        issuesdata += "";
                    }
                    else
                    {
                        issuesdata += get_Milstonename(ds.Tables[0].Rows[i]["Milestoneid"].ToString());// Milestone
                    }

                    issuesdata += "</td>";


                    // Replicability 
                    string strReplicabilitycolor = get_ReplicableColor(ds.Tables[0].Rows[i]["Replicability"].ToString());
                    issuesdata += "<td>";
                    issuesdata += "<div class='btn-group d-block'>";
                    if (streditI == "1")
                    {
                        issuesdata += "<a href ='javascript:void(0);' id='btnreplicable_" + ds.Tables[0].Rows[i]["id"].ToString() + "' type='button' onclick=setreplicableid('" + ds.Tables[0].Rows[i]["id"].ToString() + "',this.id) data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + strReplicabilitycolor + "'>" + get_ReplicableName(ds.Tables[0].Rows[i]["Replicability"].ToString()) + "</a>";
                    }
                    else
                    {
                        issuesdata += "<a href ='javascript:void(0);' id='btnreplicable_" + ds.Tables[0].Rows[i]["id"].ToString() + "' type='button'  data-bs-toggle='' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + strReplicabilitycolor + "'>" + get_ReplicableName(ds.Tables[0].Rows[i]["Replicability"].ToString()) + "</a>";
                    }
                    issuesdata += "<div class='dropdown-menu lead_form_body p-0'>";
                    issuesdata += strvalReplicability;
                    issuesdata += "</div>";
                    issuesdata += "</div>";
                    issuesdata += "</td>";


                    issuesdata += "<td valign='top'>";
                    // file 
                    if (ds.Tables[0].Rows[i]["UploadImage"].ToString() == "0" || ds.Tables[0].Rows[i]["UploadImage"].ToString() == string.Empty)
                    {
                        issuesdata += "";
                    }
                    else
                    {
                        string strfiles = "";
                        string[] ar_img = ds.Tables[0].Rows[i]["UploadImage"].ToString().Split('^');
                        if (ar_img.Length > 1)
                        {
                            for (int j = 0; j < ar_img.Length; j++)
                            {

                                string fileName = ar_img[j];
                                string extension = Path.GetExtension(fileName).ToLower();

                                if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif")
                                {
                                    strfiles += "<img src='/pms/IssueImages/" + fileName + "' style='height:50px;' class='preview-item' />";
                                }
                                else if (extension == ".pdf")
                                {
                                    strfiles += "<a href='/pms/IssueImages/" + fileName + "' target='_blank' class='preview-item'><span style='font-size:25px; margin:5px;'>📄</span></a>";

                                }
                            }

                        }
                        else
                        {
                            string fileName = ds.Tables[0].Rows[i]["UploadImage"].ToString();
                            string extension = Path.GetExtension(fileName).ToLower();

                            if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif")
                            {
                                strfiles += "<img src='/pms/IssueImages/" + fileName + "' style='height:50px;' class='preview-item' />";
                            }
                            else if (extension == ".pdf")
                            {
                                strfiles += "<a href='/pms/IssueImages/" + fileName + "' target='_blank' class='preview-item'><span style='font-size:25px; margin:5px;'>📄</span></a>";

                            }
                        }
                        issuesdata += strfiles;


                    }
                    issuesdata += "</td>";

                    issuesdata += " <td><a href='javascript:void(0);' style='display:" + (streditI == "1" ? "" : "none") + "' Class='btn btn-info btn-sm' type='button' onclick='Binddetails(" + ds.Tables[0].Rows[i]["id"].ToString() + ")' data-bs-toggle='offcanvas' data-bs-target='#addissues'><i class='ri-edit-line'></i></a>";
                    issuesdata += "&nbsp;&nbsp;<a href='javascript:void(0);' style='display:" + (strdeleteI == "1" ? "" : "none") + "' Class='btn btn-danger btn-sm' type='button' onclick='deletedata(" + ds.Tables[0].Rows[i]["id"].ToString() + ")'><i class='ri-delete-bin-5-line'></i></a>";
                    issuesdata += "</td>";

                    milestone_name = ds.Tables[0].Rows[i]["Milestoneid"].ToString();


                    issuesdata += "</tr>";
                }
            }
        }
        catch(Exception ex) {
            SaveLogText("ExceptionI3-"+ex.Message);
        }
        issuesdata += "</tbody>";
        issuesdata += "</table>";

        return issuesdata;
    }
    public static int checkadmin(string struserid = "0")
    {
        int retval = 0;
        DataAccess da = new DataAccess();
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

    [WebMethod]
    public static string Bindissuesdetail(int RecordId = 0)
    {
        DataAccess da = new DataAccess();
        DataSet ds;
        string strsql = "select * from Pms_tbl_Addissues where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
        ds = da.GetDataSetInline(strsql);

        return ds.GetXml();
    }



    [WebMethod]
    public static string Deleteissuesdata(int RecordId = 0)
    {
        string retval = "0";

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "update  Pms_tbl_Addissues set  status=0,archive=1  where  id='" + RecordId + "' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        ds = da.GetDataSetInline(strsql);

        return retval;
    }


    public static string get_employee(string ownerid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from tbl_employee where status=1 and archive=0 and id='" + ownerid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["DisplayName"].ToString();

        }

        return strret;

    }

    protected static string get_team(string str1)
    {
        DataAccess da = new DataAccess();
        string str = str1;
        string[] splitstr = str.Split(',');
        string stname = "";
        for (int i = 0; i < splitstr.Length; i++)
        {
            string strsql = "select * from tbl_employee where id = '" + splitstr[i] + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0";
            DataSet ds2 = da.GetDataSetInline(strsql);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                if (ds2.Tables[0].Rows[0]["DisplayName"].ToString() != string.Empty)
                {
                    if (stname != string.Empty)
                    {
                        stname += "," + ds2.Tables[0].Rows[0]["DisplayName"].ToString();
                    }
                    else
                    {
                        stname += ds2.Tables[0].Rows[0]["DisplayName"].ToString();
                    }
                }

            }
        }
        return stname;
    }

    protected static string get_employee_firsttwo(string str1)
    {
        DataAccess da = new DataAccess();
        string str = str1;
        string[] splitstr = str.Split(',');
        string stname = "";
        for (int i = 0; i < splitstr.Length; i++)
        {
            string strsql = "select id,SUBSTRING (DisplayName, 1, 2) as DisplayName from tbl_employee where id = '" + splitstr[i] + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0";
            DataSet ds2 = da.GetDataSetInline(strsql);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                if (ds2.Tables[0].Rows[0]["DisplayName"].ToString() != string.Empty)
                {
                    if (stname != string.Empty)
                    {
                        stname += "," + ds2.Tables[0].Rows[0]["DisplayName"].ToString();
                    }
                    else
                    {
                        stname += ds2.Tables[0].Rows[0]["DisplayName"].ToString();
                    }
                }

            }
        }
        return stname;
    }
    protected static string get_employee_first_onesplit(string str1)
    {
        DataAccess da = new DataAccess();
        string str = str1;
        string[] splitstr = str.Split(',');
        string stname = "";
        for (int i = 0; i < splitstr.Length; i++)
        {
            string strsql = "select id,SUBSTRING (DisplayName, 1, 1) as DisplayName from tbl_employee where id = '" + splitstr[i] + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0";
            DataSet ds2 = da.GetDataSetInline(strsql);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                if (ds2.Tables[0].Rows[0]["DisplayName"].ToString() != string.Empty)
                {
                    if (stname != string.Empty)
                    {
                        stname += "," + ds2.Tables[0].Rows[0]["DisplayName"].ToString();
                    }
                    else
                    {
                        stname += ds2.Tables[0].Rows[0]["DisplayName"].ToString();
                    }
                }

            }
        }
        return stname;
    }

    protected static string get_Status_Color(string projectid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_addIssueStatus where status=1 and archive=0 and id='" + projectid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["PrintColor"].ToString();

        }

        return strret;

    }
    protected static string get_Issutype_Color(string projectid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_IssueType where status=1 and archive=0 and id='" + projectid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["PrintColor"].ToString();

        }

        return strret;

    }
    protected static string get_PriorityName(string projectid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_Priority where status=1 and archive=0 and Id='" + projectid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["name"].ToString();

        }

        return strret;

    }

    protected static string get_ReplicableName(string projectid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_Replicability where status=1 and archive=0 and Id='" + projectid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["name"].ToString();

        }

        return strret;

    }

    protected static string get_Projectstatus(string projectid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_addIssueStatus where status=1 and archive=0 and id='" + projectid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["name"].ToString();

        }

        return strret;

    }

    protected static string get_IssueName(string projectid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_IssueType where status=1 and archive=0 and id='" + projectid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["name"].ToString();

        }

        return strret;

    }

    protected static string get_PriorityColor(string projectid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_Priority where  status=1 and archive=0 and Id='" + projectid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["PrintColor"].ToString();

        }

        return strret;

    }


    protected static string get_ReplicableColor(string projectid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_Replicability where  status=1 and archive=0 and Id='" + projectid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["PrintColor"].ToString();

        }

        return strret;

    }

    protected static string get_status_listbind()
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = "select * from Pms_tbl_addIssueStatus where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret += "<ul class='sttus-list' data-simplebar style='max-height: 160px'>";
            for (int i = 0; i < dscode.Tables[0].Rows.Count; i++)
            {

                strret += "<li>" +
                  "<div class='dropdown-item p-0'>" +
                    "<a class='' href='javascript:void(0);' onclick = 'UpdateDataStatusdeatils(" + dscode.Tables[0].Rows[i]["id"].ToString() + ")' > " +
                    "<i class='bi bi-square-fill' style='color:" + dscode.Tables[0].Rows[i]["PrintColor"].ToString() + "' ></i> " + dscode.Tables[0].Rows[i]["name"].ToString() + "" +
                    "</a>" +
                    "</div>" +
                    "</li>";


            }
            strret += "</ul>";
        }

        return strret;

    }

    protected static string get_issutype_listbind()
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = "select * from Pms_tbl_IssueType where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret += "<ul class='sttus-list' data-simplebar style='max-height: 160px'>";
            for (int i = 0; i < dscode.Tables[0].Rows.Count; i++)
            {

                strret += "<li>" +
                  "<div class='dropdown-item p-0'>" +
                    "<a class='' href='javascript:void(0);' onclick = 'UpdateDataIssuetypedeatils(" + dscode.Tables[0].Rows[i]["id"].ToString() + ")' > " +
                    "<i class='bi bi-square-fill' style='color:" + dscode.Tables[0].Rows[i]["PrintColor"].ToString() + "' ></i> " + dscode.Tables[0].Rows[i]["name"].ToString() + "" +
                    "</a>" +
                    "</div>" +
                    "</li>";


            }
            strret += "</ul>";
        }

        return strret;

    }


    protected static string get_Priority_listbind()
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = "select * from Pms_tbl_Priority where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret += "<ul class='sttus-list' data-simplebar style='max-height: 160px'>";
            for (int i = 0; i < dscode.Tables[0].Rows.Count; i++)
            {

                strret += "<li>" +
                  "<div class='dropdown-item p-0'>" +
                    "<a class='' href='javascript:void(0);' onclick = 'UpdateDatapropertydeatils(" + dscode.Tables[0].Rows[i]["id"].ToString() + ")' > " +
                    "<i class='bi bi-square-fill' style='color:" + dscode.Tables[0].Rows[i]["PrintColor"].ToString() + "' ></i> " + dscode.Tables[0].Rows[i]["name"].ToString() + "" +
                    "</a>" +
                    "</div>" +
                    "</li>";


            }
            strret += "</ul>";
        }

        return strret;


    }

    protected static string get_Replicability_listbind()
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = "select * from Pms_tbl_Replicability where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret += "<ul class='sttus-list' data-simplebar style='max-height: 160px'>";
            for (int i = 0; i < dscode.Tables[0].Rows.Count; i++)
            {

                strret += "<li>" +
                  "<div class='dropdown-item p-0'>" +
                    "<a class='' href='javascript:void(0);' onclick = 'UpdateDataReplicabledeatils(" + dscode.Tables[0].Rows[i]["id"].ToString() + ")' > " +
                    "<i class='bi bi-square-fill' style='color:" + dscode.Tables[0].Rows[i]["PrintColor"].ToString() + "' ></i> " + dscode.Tables[0].Rows[i]["name"].ToString() + "" +
                    "</a>" +
                    "</div>" +
                    "</li>";


            }
            strret += "</ul>";
        }

        return strret;


    }

    public static string get_issutype(string issutypeid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_IssueType where status=1 and archive=0 and id='" + issutypeid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["name"].ToString();

        }

        return strret;

    }

    public static string get_Replicability(string replicableid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_Replicability where status=1 and archive=0 and id='" + replicableid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["name"].ToString();

        }

        return strret;

    }


    public static string get_Milstonename(string milestoneid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_AddMilestone where status=1 and archive=0 and id='" + milestoneid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["Milestone"].ToString();

        }

        return strret;

    }

    public static string get_Countryname(string countryid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from tbl_countrymaster where status=1 and archive=0 and id='" + countryid + "'  ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["countryname"].ToString();

        }

        return strret;
    }


}
public class IssueFormOption
{
    public string id { get; set; }
    public string name { get; set; }
}

public class issuestdata
{
    public string id { get; set; }
    public string ProjectId { get; set; }
    public string Milestoneid { get; set; }
    public string tasklistid { get; set; }
    public string taskid { get; set; }
    public string subtaskid { get; set; }
    public string Issues { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string Owner { get; set; }
    public string team { get; set; }
    public string Issueon { get; set;}
    public string project_id { get; set;}
    public string Description { get; set; }
    public string Reminder { get; set; }
    public string ReminderType { get; set; }
    public string Days { get; set; }
    public string Time { get; set; }
    public string Notifyusers { get; set; }
    public string ChooseDate { get; set; }
    public string AssignTo { get; set; }
    public string IssueStatus { get; set; }
    public string Priority { get; set; }
    public string IssueType { get; set; }
    public string Replicability { get; set; }
    public string UploadImage { get; set; }

}




