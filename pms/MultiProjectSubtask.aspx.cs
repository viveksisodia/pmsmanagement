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

public partial class MultiProjectSubtask : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataAccess da = new DataAccess();
    public CommonFunction cmf = new CommonFunction();
    SqlParameter[] prm, prm1;
    public function fns = new function();
    public string Subtaskdata = "";
    public string Multiteam = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {


        }
    }





    //// web method bind using ajax start //

    [WebMethod]
    public static void SaveSubTask(SubSubtaskdatamulti subtaskdetails)
    {
        DataAccess da1 = new DataAccess();
        string editid = subtaskdetails.id;
        string entrytype = "Insert";
        if (editid != "" && editid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[32];
        prm[0] = new SqlParameter("@id", editid);
        prm[1] = new SqlParameter("@ProjectId", subtaskdetails.ProjectId);
        prm[2] = new SqlParameter("@Milestoneid", subtaskdetails.Milestoneid);
        prm[3] = new SqlParameter("@tasklist", subtaskdetails.tasklist);
        prm[4] = new SqlParameter("@task", subtaskdetails.task);
        prm[5] = new SqlParameter("@Subtaskname", subtaskdetails.Subtaskname);
        prm[6] = new SqlParameter("@SubTaskDescription", subtaskdetails.SubTaskDescription);
        prm[7] = new SqlParameter("@owner", subtaskdetails.owner);
        if (subtaskdetails.Startdate == string.Empty)
        {
            prm[8] = new SqlParameter("@Startdate", DBNull.Value);
        }
        else
        {
            prm[8] = new SqlParameter("@Startdate", subtaskdetails.Startdate);
        }
        if (subtaskdetails.Targetdate == string.Empty)
        {
            prm[9] = new SqlParameter("@Targetdate", DBNull.Value);
        }
        else
        {
            prm[9] = new SqlParameter("@Targetdate", subtaskdetails.Targetdate);
        }


        if (subtaskdetails.StartTime == string.Empty)
        {
            prm[10] = new SqlParameter("@StartTime", DBNull.Value);
        }
        else
        {
            prm[10] = new SqlParameter("@StartTime", subtaskdetails.StartTime);
        }


        if (subtaskdetails.Timeline == string.Empty)
        {
            prm[11] = new SqlParameter("@Timeline", DBNull.Value);
        }
        else
        {
            prm[11] = new SqlParameter("@Timeline", subtaskdetails.Timeline);
        }

        prm[12] = new SqlParameter("@TaskDuration", subtaskdetails.TaskDuration);

        prm[13] = new SqlParameter("@P_Status", subtaskdetails.P_Status);
        prm[14] = new SqlParameter("@Priority", subtaskdetails.Priority);

        if (subtaskdetails.Budget == string.Empty)
        {
            prm[15] = new SqlParameter("@Budget", DBNull.Value);
        }
        else
        {
            prm[15] = new SqlParameter("@Budget", subtaskdetails.Budget);
        }

        if (subtaskdetails.Actual == string.Empty)
        {
            prm[16] = new SqlParameter("@Actual", DBNull.Value);
        }
        else
        {
            prm[16] = new SqlParameter("@Actual", subtaskdetails.Actual);
        }


        prm[17] = new SqlParameter("@Reminder", subtaskdetails.Reminder);

        if (subtaskdetails.ReminderType == string.Empty)
        {
            prm[18] = new SqlParameter("@ReminderType", DBNull.Value);
        }
        else
        {
            prm[18] = new SqlParameter("@ReminderType", subtaskdetails.ReminderType);
        }

        if (subtaskdetails.Days == string.Empty)
        {
            prm[19] = new SqlParameter("@Days", DBNull.Value);
        }
        else
        {
            prm[19] = new SqlParameter("@Days", subtaskdetails.Days);

        }
        if (subtaskdetails.Time == string.Empty)
        {
            prm[20] = new SqlParameter("@Time", DBNull.Value);
        }
        else
        {
            prm[20] = new SqlParameter("@Time", subtaskdetails.Time);
        }
        if (subtaskdetails.Notifyusers == string.Empty)
        {
            prm[21] = new SqlParameter("@Notifyusers", DBNull.Value);
        }
        else
        {
            prm[21] = new SqlParameter("@Notifyusers", subtaskdetails.Notifyusers);
        }

        if (subtaskdetails.ChooseDate == string.Empty)
        {
            prm[22] = new SqlParameter("@ChooseDate", DBNull.Value);
        }
        else
        {
            prm[22] = new SqlParameter("@ChooseDate", subtaskdetails.ChooseDate);
        }

        prm[23] = new SqlParameter("@Occurrence", subtaskdetails.Occurrence);
        prm[24] = new SqlParameter("@tagto", subtaskdetails.tagto);
        prm[25] = new SqlParameter("@team", subtaskdetails.team);
        prm[26] = new SqlParameter("@Stage", subtaskdetails.Stage);
        prm[27] = new SqlParameter("@Weightage", subtaskdetails.Weightage);
        prm[28] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[29] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[30] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[31] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pms_Add_Subtask_usp", prm);


        int checkweitage = 0;
        decimal wetagecount = 0;
        decimal totalwetage = 0;
        string strweightage = "select isnull(checkTaskWeightage ,0) as checkTaskWeightage  from pms_tbl_addtask where id='" + subtaskdetails.task + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
        DataSet dsweitagecheck = da1.GetDataSetInline(strweightage);
        if (dsweitagecheck.Tables[0].Rows.Count > 0)
        {

            checkweitage = Convert.ToInt32(dsweitagecheck.Tables[0].Rows[0]["checkTaskWeightage"]);

        }

        if (checkweitage == 1)
        {
            string strweightagecount = "select count (*) as WeitageCount from Pms_tbl_Addsubtask where task='" + subtaskdetails.task + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
            DataSet dsweitagecount = da1.GetDataSetInline(strweightagecount);
            if (dsweitagecount.Tables[0].Rows.Count > 0)
            {

                wetagecount = Convert.ToInt32(dsweitagecount.Tables[0].Rows[0]["WeitageCount"]);

            }

            totalwetage = 100 / wetagecount;
            string strstatusupdate = "update Pms_tbl_Addsubtask set Weightage='" + totalwetage.ToString() + "' where task='" + subtaskdetails.task + "' and status=1 and archive=0";
            DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

        }

        if (checkweitage == 0)
        {
            string strstatusupdate = "update Pms_tbl_Addsubtask set Weightage='0' where task='" + subtaskdetails.task + "' and status=1 and archive=0";
            DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

        }

    }

    [WebMethod]
    public static string statussave(string value = "", string statuscolor = "")
    {

        string retval = "0";
        DataAccess da1 = new DataAccess();

        SqlParameter[] prm = new SqlParameter[5];
        prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[1] = new SqlParameter("@name", value);
        prm[2] = new SqlParameter("@type", "insert");
        prm[3] = new SqlParameter("@PrintColor", "#" + statuscolor);
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
        prm[3] = new SqlParameter("@PrintColor", "#" + prioritycolor);
        prm[4] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[4].Direction = ParameterDirection.ReturnValue;
        int id = da1.executeDMLQuery("Pms_SP_Priority_insert_reg", prm);
        retval = id.ToString();

        return retval;

    }

    [WebMethod]
    public static string AddStage(string value = "", string Stagecolor = "")
    {

        string retval = "0";
        DataAccess da1 = new DataAccess();

        SqlParameter[] prm = new SqlParameter[5];
        prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[1] = new SqlParameter("@name", value);
        prm[2] = new SqlParameter("@type", "insert");
        prm[3] = new SqlParameter("@PrintColor", "#" + Stagecolor);
        prm[4] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[4].Direction = ParameterDirection.ReturnValue;
        int id = da1.executeDMLQuery("Pms_SP_Stage_insert_reg", prm);
        retval = id.ToString();

        return retval;

    }


    [WebMethod]
    public static string Save_Owner(string id = "", string ownerdropdownid = "")
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string editid = id;
        string entrytype = "Update";
        if (editid != "" && editid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", id);
        prm[1] = new SqlParameter("@owner", ownerdropdownid);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pmstbl_subtaskOwnerDeatilsUpdate_usp", prm);

        DataAccess da = new DataAccess();
        string strsql = "select * from Pms_tbl_Addsubtask where  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and  id='" + editid + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {

            for (int i = 0; i < dscode.Tables[0].Rows.Count; i++)
            {
                string cntownr = "0";
                string usersdetailsowner = "";
                string ownertooltip = "";
                if (dscode.Tables[0].Rows[i]["owner"].ToString() != "")
                {
                    string[] arcnt = dscode.Tables[0].Rows[i]["owner"].ToString().Trim().Split(',');
                    cntownr = arcnt.Length.ToString();
                    DataSet dsusers = new DataSet();
                    if (dscode.Tables[0].Rows[i]["owner"].ToString().Trim() != "")
                    {
                        dsusers = da.GetDataSetInline("select id,DisplayName,fileupload1,SUBSTRING (DisplayName, 1, 2) as splinameone from tbl_employee where id in(" + dscode.Tables[0].Rows[i]["owner"].ToString().Trim() + ")");
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

        string strsql = "select * from Pms_tbl_Addsubtask where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
        ds = da.GetDataSetInline(strsql);


        return ds.GetXml();
    }

    [WebMethod]
    public static string Save_Team(string id = "", string teamdropdownid = "")
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string editid = id;
        string entrytype = "Update";
        if (editid != "" && editid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", editid);
        prm[1] = new SqlParameter("@team", teamdropdownid);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pms_tbl_subtask_TeamDeatilsUpdate", prm);

        DataAccess da = new DataAccess();
        string strsql = "select * from Pms_tbl_Addsubtask where  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and  id='" + editid + "' ";
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

        string strsql = "select * from Pms_tbl_Addsubtask where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
        ds = da.GetDataSetInline(strsql);


        return ds.GetXml();
    }


    [WebMethod]
    public static string Update_Status(string subtaskid = "", string statusid = "")
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (subtaskid != "" && subtaskid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", subtaskid);
        prm[1] = new SqlParameter("@ProjectStatus", statusid);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pmstbl_subtask_Status_DeatilsUpdate", prm);

        DataAccess da = new DataAccess();
        string strsql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + statusid + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            retval = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
        }

        return retval;
    }

    [WebMethod]
    public static string UpdatePriority_Status(string taskid = "", string Propertyid = "")
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (taskid != "" && taskid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", taskid);
        prm[1] = new SqlParameter("@Priority", Propertyid);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pms_tbl_Subtask_Priority_DeatilsUpdate", prm);

        DataAccess da = new DataAccess();
        string strsql = "select Id,name,PrintColor from Pms_tbl_Priority  where  status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + Propertyid + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            retval = dscode.Tables[0].Rows[0]["name"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
        }

        return retval;

    }

    [WebMethod]
    public static string Update_Stagge(string taskid = "", string Stageid = "")
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (taskid != "" && taskid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", taskid);
        prm[1] = new SqlParameter("@Stage", Stageid);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pmstbl_Subtask_Stage_DeatilsUpdate", prm);

        DataAccess da = new DataAccess();
        string strsql = "select id,name,PrintColor from Pms_tbl_Stage where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + Stageid + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            retval = dscode.Tables[0].Rows[0]["name"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
        }

        return retval;
    }


    [WebMethod]
    public static string UpdateSubTask_Dependency(string Subtaskid = "", string dependencyid = "")
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (Subtaskid != "" && Subtaskid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", Subtaskid);
        prm[1] = new SqlParameter("@dependancy", dependencyid);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pms_tbl_DependancySubtaskdtls_Update", prm);



        //dependancy start //


        DataAccess da = new DataAccess();
        string strsql = "select * from Pms_tbl_Addsubtask where  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and  id='" + Subtaskid + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dscode.Tables[0].Rows.Count; i++)
            {

                string cntdepndcy = "0";
                string depdecydetails = "";
                string depcytooltip = "";

                string strdepcysql = "Select * from Pms_tbl_Addsubtask where id = '" + Subtaskid + "' and status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                DataSet dsdepcy = da.GetDataSetInline(strdepcysql);
                if (dsdepcy.Tables[0].Rows[0]["dependancy"].ToString() != "")
                {
                    string[] arcntdep = dsdepcy.Tables[0].Rows[0]["dependancy"].ToString().Trim().Split(',');
                    cntdepndcy = arcntdep.Length.ToString();
                    DataSet dsdtlsdepcy = new DataSet();
                    if (dsdepcy.Tables[0].Rows[0]["dependancy"].ToString().Trim() != "")
                    {
                        dsdtlsdepcy = da.GetDataSetInline("select id,Subtaskname from Pms_tbl_Addsubtask where id in(" + dsdepcy.Tables[0].Rows[0]["dependancy"].ToString().Trim() + ")");
                        if (dsdtlsdepcy.Tables[0].Rows.Count > 1)
                        {
                            cntdepndcy = (Convert.ToInt32(cntdepndcy) - 1).ToString();

                            // dependancy tool tip //
                            for (int l = 1; l < dsdtlsdepcy.Tables[0].Rows.Count; l++)
                            {
                                if (dsdtlsdepcy.Tables[0].Rows[l]["Subtaskname"].ToString() != string.Empty)
                                {
                                    if (depcytooltip != string.Empty)
                                    {
                                        depcytooltip += "," + dsdtlsdepcy.Tables[0].Rows[l]["Subtaskname"].ToString();
                                    }
                                    else
                                    {
                                        depcytooltip += dsdtlsdepcy.Tables[0].Rows[l]["Subtaskname"].ToString();
                                    }
                                }

                            }

                            for (int k = 0; k < 1; k++)
                            {
                                if (dsdtlsdepcy.Tables[0].Rows[k]["Subtaskname"].ToString() != "")
                                {

                                    depdecydetails += "<span class='badge bg-primary'>" + dsdtlsdepcy.Tables[0].Rows[k]["Subtaskname"].ToString() + "</span>";
                                }
                                else
                                {
                                    depdecydetails += "<span class='badge bg-primary'>" + dsdtlsdepcy.Tables[0].Rows[k]["Subtaskname"].ToString() + "</span>";
                                }
                            }

                        }
                        else
                        {
                            cntdepndcy = "";
                            for (int k = 0; k < dsdtlsdepcy.Tables[0].Rows.Count; k++)
                            {
                                if (dsdtlsdepcy.Tables[0].Rows[k]["Subtaskname"].ToString() != "")
                                {
                                    depdecydetails += "<span class='badge bg-primary'>" + dsdtlsdepcy.Tables[0].Rows[k]["Subtaskname"].ToString() + "</span>";
                                }
                                else
                                {
                                    depdecydetails += "<span class='badge bg-primary'>" + dsdtlsdepcy.Tables[0].Rows[k]["Subtaskname"].ToString() + "</span>";
                                }
                            }
                        }

                    }


                }




                string cntdep = "";
                if (cntdepndcy != "" && cntdepndcy != "0")
                {
                    cntdep = "+" + cntdepndcy;

                }

                retval += "<div class='d-flex'>";
                retval += "<div class='flex-grow-1'>";
                retval += "<div class='flex-wrap gap-2'>";
                retval += depdecydetails;
                retval += "<span class='cursor-pointer' data-bs-toggle='tooltip' data-bs-html='true' data-bs-placement='top' data-bs-trigger='hover' title='" + depcytooltip + "'>" + cntdep + "</span>";
                retval += "</div>";
                retval += "</div>";
                retval += "</div>";





            }

        }
        return retval;

        // dependancy end//

    }

    [WebMethod]
    public static string Bindsubtask_Dependancy(int RecordId = 0)
    {

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "select * from Pms_tbl_Addsubtask where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";

        ds = da.GetDataSetInline(strsql);
        return ds.GetXml();
    }


    [WebMethod]
    public static string Bindsubtaskdetail_extendatedate(int RecordId = 0)
    {

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "select * from Pms_tbl_Addsubtask where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
        ds = da.GetDataSetInline(strsql);


        return ds.GetXml();
    }


    [WebMethod]
    public static string UpdateExtendeddatedtls_subtask(string subtask_id = "", string extendStrtdate_subtask = "", string extendEnddate_subtask = "")
    {
        string retval = "";

        string estendleftdlydys = "";
        string estendstartmonthname = "";
        string estendendmonthname = "";
        double estendleftDays = 0;
        int estendststartdys = 0;
        int estendenddys = 0;
        double estendDays = 0;
        string extenddatebind = "";


        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (subtask_id != "" && subtask_id != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[7];
        prm[0] = new SqlParameter("@id", subtask_id);
        prm[1] = new SqlParameter("@StartDate", extendStrtdate_subtask);
        prm[2] = new SqlParameter("@TargetDate", extendEnddate_subtask);
        prm[3] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[4] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[5] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[6] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pmstbl_ExtnddatedateUpdate_subtask", prm);



        // extended timeleine 


        string strextend = "select id,extendStartdate as StartDate,extendEnddate as TargetDate from Pms_Tbl_Extended_Timeline where  status=1 and archive=0 and Type='subtask' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and subtaskid = '" + subtask_id + "'  ";
        DataSet dsstrextend = da1.GetDataSetInline(strextend);
        if (dsstrextend.Tables[0].Rows.Count > 0)
        {
            for (int k = 0; k < dsstrextend.Tables[0].Rows.Count; k++)
            {
                DateTime estendStartDate = Convert.ToDateTime(dsstrextend.Tables[0].Rows[k]["StartDate"].ToString() == "" ? "0" : dsstrextend.Tables[0].Rows[k]["StartDate"].ToString());
                DateTime estendTargetdate = Convert.ToDateTime(dsstrextend.Tables[0].Rows[k]["TargetDate"].ToString() == "" ? "0" : dsstrextend.Tables[0].Rows[k]["TargetDate"].ToString());
                TimeSpan estendobjTimeSpan = estendTargetdate - estendStartDate;
                estendDays = Convert.ToDouble(estendobjTimeSpan.TotalDays);
                estendDays = estendDays + 1;


                DateTime estendEnddate = Convert.ToDateTime(dsstrextend.Tables[0].Rows[k]["Targetdate"].ToString() == "" ? "0" : dsstrextend.Tables[0].Rows[k]["Targetdate"].ToString());
                DateTime estendCurrentdate = DateTime.Now;
                TimeSpan estendobjtimeleftspn = estendEnddate - estendCurrentdate;
                estendleftDays = Convert.ToDouble(estendobjtimeleftspn.Days);
                estendleftDays = estendleftDays + 1;


                if (estendleftDays < 0)
                {
                    estendleftdlydys = "days delayed";
                }
                else
                {
                    estendleftdlydys = "days left";
                }

                //start days months  

                estendststartdys = estendStartDate.Day;
                int estendstrtmonth = estendStartDate.Month;
                if (estendstrtmonth == 1)
                {
                    estendstartmonthname = "Jan";
                }
                if (estendstrtmonth == 2)
                {
                    estendstartmonthname = "Feb";
                }
                if (estendstrtmonth == 3)
                {
                    estendstartmonthname = "Mar";
                }

                if (estendstrtmonth == 4)
                {
                    estendstartmonthname = "Apr";
                }

                if (estendstrtmonth == 5)
                {
                    estendstartmonthname = "May";
                }

                if (estendstrtmonth == 6)
                {
                    estendstartmonthname = "Jun";
                }

                if (estendstrtmonth == 7)
                {
                    estendstartmonthname = "Jul";
                }

                if (estendstrtmonth == 8)
                {
                    estendstartmonthname = "Aug";
                }

                if (estendstrtmonth == 9)
                {
                    estendstartmonthname = "Sep";
                }

                if (estendstrtmonth == 10)
                {
                    estendstartmonthname = "Oct";
                }

                if (estendstrtmonth == 11)
                {
                    estendstartmonthname = "Nov";
                }

                if (estendstrtmonth == 12)
                {
                    estendstartmonthname = "Dec";
                }

                //end days months 

                estendenddys = estendTargetdate.Day;
                int estendendmonth = estendTargetdate.Month;
                if (estendendmonth == 1)
                {
                    estendendmonthname = "Jan";
                }
                if (estendendmonth == 2)
                {
                    estendendmonthname = "Feb";
                }
                if (estendendmonth == 3)
                {
                    estendendmonthname = "Mar";
                }

                if (estendendmonth == 4)
                {
                    estendendmonthname = "Apr";
                }

                if (estendendmonth == 5)
                {
                    estendendmonthname = "May";
                }

                if (estendendmonth == 6)
                {
                    estendendmonthname = "Jun";
                }

                if (estendendmonth == 7)
                {
                    estendendmonthname = "Jul";
                }

                if (estendendmonth == 8)
                {
                    estendendmonthname = "Aug";
                }

                if (estendendmonth == 9)
                {
                    estendendmonthname = "Sep";
                }

                if (estendendmonth == 10)
                {
                    estendendmonthname = "Oct";
                }

                if (estendendmonth == 11)
                {
                    estendendmonthname = "Nov";
                }

                if (estendendmonth == 12)
                {
                    estendendmonthname = "Dec";
                }



                retval += "<a href = 'javascript:void(0);' class='timesheet-wrap'>";
                retval += "<div class='progress progress-xl bg-dark position-relative me-1' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + estendststartdys + " " + estendstartmonthname + " - " + estendenddys + " " + estendendmonthname + " (" + estendleftDays.ToString().Replace("-", "") + " " + estendleftdlydys + ")'>";
                retval += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 100%;' aria-valuemax='100'>" + estendDays + " days</div>";
                retval += "</div>";
                retval += "</a>";

            }


        }




        return retval;


    }


    [WebMethod]
    public static void SaveTimer(string ProjectId = "", string Milestoneid = "", string tasklistid = "", string taskid = "", string subtaskid = "", string TimerNotes = "", string hour = "", string minute = "", string second = "", string millisecond = "", string Transactionid = "")
    {
        DataAccess da1 = new DataAccess();
        string entrytype = "update";
        SqlParameter[] prm = new SqlParameter[16];
        prm[0] = new SqlParameter("@ProjectId", ProjectId);
        prm[1] = new SqlParameter("@Milestoneid", Milestoneid);
        prm[2] = new SqlParameter("@tasklist", tasklistid);
        prm[3] = new SqlParameter("@taskid", taskid);
        prm[4] = new SqlParameter("@subtaskid", subtaskid);
        prm[5] = new SqlParameter("@hour", hour);
        prm[6] = new SqlParameter("@minute", minute);
        prm[7] = new SqlParameter("@second", second);
        prm[8] = new SqlParameter("@millisecond", millisecond);
        prm[9] = new SqlParameter("@id", Transactionid);
        prm[10] = new SqlParameter("@EndTime", da1.getcdatetime());
        prm[11] = new SqlParameter("@TimerNotes", TimerNotes);
        prm[12] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[13] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[14] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[15] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pmstbl_SubTask_TimerDetailsSaveUpdate", prm);

        string strstatusupdate = "update Pms_tbl_Addsubtask set statuscomplete=3 where id='" + subtaskid + "' and status=1 and archive=0";
        DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

    }

    [WebMethod]
    public static string SaveTimerstartdetails(string ProjectId = "", string Milestoneid = "", string tasklistid = "", string taskid = "", string Subtaskid = "")
    {
        DataAccess da1 = new DataAccess();
        string entrytype = "Insert";
        SqlParameter[] prm = new SqlParameter[11];
        prm[0] = new SqlParameter("@ProjectId", ProjectId);
        prm[1] = new SqlParameter("@Milestoneid", Milestoneid);
        prm[2] = new SqlParameter("@tasklist", tasklistid);
        prm[3] = new SqlParameter("@taskid", taskid);
        prm[4] = new SqlParameter("@subtaskid", Subtaskid);
        prm[5] = new SqlParameter("@StartTime", da1.getcdatetime());
        prm[6] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[7] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[8] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[9] = new SqlParameter("@type", entrytype);
        prm[10] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[10].Direction = ParameterDirection.ReturnValue;
        int retid = da1.executeDMLQuery("pmstbl_SubTask_StartTimerDetailsSave", prm);

        string strstatusupdate = "update Pms_tbl_Addsubtask set statuscomplete=1 where id='" + Subtaskid + "' and status=1 and archive=0";
        DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

        return retid.ToString();


    }


    [WebMethod]
    public static void SaveTimerpausedetails(string ProjectId = "", string Milestoneid = "", string tasklistid = "", string taskid = "", string subtaskid = "", string transactionid = "", string Notes = "")
    {
        DataAccess da1 = new DataAccess();
        string entrytype = "Insert";
        SqlParameter[] prm = new SqlParameter[12];
        prm[0] = new SqlParameter("@ProjectId", ProjectId);
        prm[1] = new SqlParameter("@Milestoneid", Milestoneid);
        prm[2] = new SqlParameter("@tasklist", tasklistid);
        prm[3] = new SqlParameter("@taskid", taskid);
        prm[4] = new SqlParameter("@subtaskid", subtaskid);
        prm[5] = new SqlParameter("@Transactionid", transactionid);
        prm[6] = new SqlParameter("@Notes", Notes);
        prm[7] = new SqlParameter("@PauseDatetime", da1.getcdatetime());
        prm[8] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[9] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[10] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[11] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pmstbl_SubTask_PauseDetailsSave", prm);


        string strstatusupdate = "update Pms_tbl_Addsubtask set statuscomplete=2 where id='" + subtaskid + "' and status=1 and archive=0";
        DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);


    }

    [WebMethod]
    public static void SaveTimerresumedetails(string ProjectId = "", string Milestoneid = "", string tasklistid = "", string taskid = "", string subtaskid = "", string transactionid = "")
    {
        CommonFunction cmf = new CommonFunction();
        DataAccess da = new DataAccess();
        string hour = "";
        string min = "";
        string sec = "";
        string mili = "";
        string pausetime = "";
        int ispause = 0;
        int isend = 0;
        //string transactionid = "0";
        string strhourminuts = "select *  from Pms_tbl_Timerstatus where subtaskid='" + subtaskid + "' and taskid='" + taskid + "' and ProjectId='" + ProjectId + "' and Milestoneid='" + Milestoneid + "' and tasklist='" + tasklistid + "' and type='subtask' and status=1 and archive=0 order by id desc";
        DataSet dshourmin = da.GetDataSetInline(strhourminuts);
        if (dshourmin.Tables[0].Rows.Count > 0)
        {
            transactionid = dshourmin.Tables[0].Rows[0]["id"].ToString();
            if (dshourmin.Tables[0].Rows[0]["IsEnd"].ToString() == "1")
            {
                isend = 1;
            }
            else
            {
                isend = 0;
            }

            if (dshourmin.Tables[0].Rows[0]["IsPaused"].ToString() == "1")
            {
                ispause = 1;
                string strpauseddetails = "select *  from Pms_Tbl_PauseStatus where Transactionid='" + transactionid + "' and type='subtask' and status=1 and archive=0 order by id desc";
                DataSet dspaused = da.GetDataSetInline(strpauseddetails);

                if (dspaused.Tables[0].Rows.Count > 0)
                {
                    TimeSpan toatlhr = (DateTime.Now - Convert.ToDateTime(dspaused.Tables[0].Rows[0]["PauseDatetime"].ToString()));
                    hour = toatlhr.Hours.ToString("00");
                    min = toatlhr.Minutes.ToString("00");
                    sec = toatlhr.Seconds.ToString("00");
                    mili = toatlhr.Milliseconds.ToString("000");
                    string totalmin = (toatlhr.Hours * 60 + toatlhr.Minutes + toatlhr.Seconds / 60).ToString();
                    string strresumedetails = "update Pms_Tbl_PauseStatus set StartDatetime='" + DateTime.Now.ToString() + "' ,PausedTime='" + totalmin + "',StartByid='" + HttpContext.Current.Request.Cookies["user"]["user_id"].ToString() + "' where Transactionid='" + transactionid + "' and id='" + dspaused.Tables[0].Rows[0]["id"].ToString() + "' and type='subtask' and status=1 and archive=0;" +
                        "update  Pms_tbl_Timerstatus set IsPaused=0 where id='" + transactionid + "' and type='subtask'";
                    da.executeDMLQueryinline(strresumedetails);
                }


            }


        }



    }



    [WebMethod]
    public static string bindSubtaskdata(string RecordId = "0")
    {
        string retval = get_status_listbind();
        string retstageval = get_Stage_listbind();
        string strvalprority = get_Priority_listbind();
        string Subtaskdata = "";
        function fns = new function();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();

        //SqlParameter[] prm = new SqlParameter[5];
        //prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        //prm[1] = new SqlParameter("@RecordId", RecordId);
        //prm[2] = new SqlParameter("@milstoneid", milstoneid);
        //prm[3] = new SqlParameter("@tasklistid", tasklistid);
        //prm[4] = new SqlParameter("@taskid", taskid);
        //DataSet ds = da.GetDataSet("Pms_BindSubtask_dashbord", prm);
        

        string strproject = "select * from Pms_tbl_AddProject where id='" + RecordId + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dsproject = da.GetDataSetInline(strproject);

       // Subtaskdata += "<table cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm datatable'>";
        Subtaskdata += "<table cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm'>";
        Subtaskdata += "<thead>";
        Subtaskdata += "<tr>";

        //Subtaskdata += "<th>Select</th>";
        Subtaskdata += "<th>S.No</th>";
        Subtaskdata += "<th style='min-width:180px;'>Sub Task</th>";
        Subtaskdata += "<th style='min-width:130px;'></th>";
        Subtaskdata += "<th style='min-width:180px;'>Dependency</th>";
        Subtaskdata += "<th style='min-width:180px;'>Owner</th>";
        Subtaskdata += "<th style='min-width:180px;'>Team</th>";
        Subtaskdata += "<th style='min-width:180px;'>Status</th>";
        Subtaskdata += "<th style='min-width:180px;'>Priority</th>";
        Subtaskdata += "<th>Budget</th>";
        Subtaskdata += "<th>Actual</th>";
        Subtaskdata += "<th>Start Date</th>";
        Subtaskdata += "<th>Due Date</th>";
        Subtaskdata += "<th>Task Duration (Hour)</th>";
        Subtaskdata += "<th style='min-width:180px;'>Timeline</th>";
        Subtaskdata += "<th style='min-width:180px;'>Extended Timeline</th>";
        Subtaskdata += "<th style='min-width:180px;'>Stage</th>";
        Subtaskdata += "<th style='min-width:180px;'>Completion Status</th>";
        Subtaskdata += "<th style='min-width:180px;'>Issues</th>";

        Subtaskdata += "<th>Work Hours (W)</th>";
        Subtaskdata += "<th>Timelog Total (T)</th>";
        Subtaskdata += "<th>Difference (W-T)</th>";
        
        Subtaskdata += "<th>Action</th>";
        Subtaskdata += "</tr>";
        Subtaskdata += "</thead>";
        Subtaskdata += "<tbody>";
        if (dsproject.Tables[0].Rows.Count > 0)
        {

            for (int P = 0; P < dsproject.Tables[0].Rows.Count; P++)
            {
                //Subtaskdata += "<tr>";
                //Subtaskdata += "<td colspan='24'>";
                //Subtaskdata += "<span ><b>" + dsproject.Tables[0].Rows[P]["ProjectName"].ToString() + "</b></span><br>";
                //Subtaskdata += "</tr>";


                string strsmilstone = "select * from Pms_tbl_AddMilestone where ProjectId='" + dsproject.Tables[0].Rows[P]["id"].ToString() + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                DataSet ds1 = da.GetDataSetInline(strsmilstone);
                if (ds1.Tables[0].Rows.Count > 0)
                {

                    for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                    {
                        Subtaskdata += "<tr>";
                        
                        Subtaskdata += "<td colspan='22'>"; // milestone
                        Subtaskdata += "<span style='font-size:20px;'><b>" + get_Milstonename(ds1.Tables[0].Rows[j]["id"].ToString()) + "</b></span>"; // milestone
                        Subtaskdata += "</tr>";


                        string strstask = "select * from Pms_tbl_Addtask where   ProjectId='" + dsproject.Tables[0].Rows[P]["id"].ToString() + "' and Milestoneid='" + ds1.Tables[0].Rows[j]["id"].ToString() + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                        DataSet ds2 = da.GetDataSetInline(strstask);
                        if (ds2.Tables[0].Rows.Count > 0)
                        {

                            for (int k = 0; k < ds2.Tables[0].Rows.Count; k++)
                            {


                                Subtaskdata += "<tr>";
                                
                                Subtaskdata += "<td colspan='22'>"; 
                                Subtaskdata += "<span style='font-size:20px;'><b>" + get_Task(ds2.Tables[0].Rows[k]["id"].ToString()) + "</b></span>"; // task
                                Subtaskdata += "</tr>";
                                
                                Subtaskdata += "</tr>";

                                string strsubstask = "select * from Pms_tbl_Addsubtask where   ProjectId='" + dsproject.Tables[0].Rows[P]["id"].ToString() + "' and Milestoneid='" + ds1.Tables[0].Rows[j]["id"].ToString() + "' and task='" + ds2.Tables[0].Rows[k]["id"].ToString() + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                                DataSet ds = da.GetDataSetInline(strsubstask);
                                if (ds.Tables[0].Rows.Count > 0)
                                {

                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {
                                        string cntownr = "0";
                                        string usersdetailsowner = "";
                                        string ownertooltip = "";
                                        if (ds.Tables[0].Rows[i]["owner"].ToString() != "")
                                        {
                                            string[] arcnt = ds.Tables[0].Rows[i]["owner"].ToString().Trim().Split(',');
                                            cntownr = arcnt.Length.ToString();
                                            DataSet dsusers = new DataSet();
                                            if (ds.Tables[0].Rows[i]["owner"].ToString().Trim() != "")
                                            {
                                                dsusers = da.GetDataSetInline("select id,DisplayName,fileupload1,SUBSTRING (DisplayName, 1, 2) as splinameone from tbl_employee where id in(" + ds.Tables[0].Rows[i]["owner"].ToString().Trim() + ")");
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

                                                    for (int U = 0; U < 3; U++)
                                                    {
                                                        if (dsusers.Tables[0].Rows[U]["fileupload1"].ToString() != "")
                                                        {
                                                            usersdetailsowner += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title=" + dsusers.Tables[0].Rows[U]["DisplayName"].ToString() + " data-bs-original-title='" + dsusers.Tables[0].Rows[U]["DisplayName"].ToString() + "'><img src = '" + dsusers.Tables[0].Rows[U]["fileupload1"] + "' alt='' class='rounded-circle avatar-xxs'></a>";
                                                        }
                                                        else
                                                        {
                                                            usersdetailsowner += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover'" +
                                         " data-bs-placement='top' title='" + dsusers.Tables[0].Rows[U]["DisplayName"].ToString() + "' data-bs-original-title='" + dsusers.Tables[0].Rows[U]["DisplayName"].ToString() + "'>";
                                                            usersdetailsowner += @"<div class='avatar-xxs'><div class='avatar-title rounded-circle bg-light text-primary'>" + dsusers.Tables[0].Rows[k]["splinameone"].ToString() + "</div></div>";
                                                            usersdetailsowner += @"</a>";
                                                        }
                                                    }

                                                }
                                                else
                                                {
                                                    cntownr = "";
                                                    for (int U = 0; U < dsusers.Tables[0].Rows.Count; U++)
                                                    {
                                                        if (dsusers.Tables[0].Rows[U]["fileupload1"].ToString() != "")
                                                        {
                                                            //img src='/assets/images/avatar-1.jpg'
                                                            usersdetailsowner += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='' data-bs-original-title='" + dsusers.Tables[0].Rows[U]["DisplayName"].ToString() + "'><img src = '" + dsusers.Tables[0].Rows[U]["fileupload1"] + "' alt='' class='rounded-circle avatar-xxs'></a>";
                                                        }
                                                        else
                                                        {
                                                            usersdetailsowner += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover'  data-bs-placement='top' title='" + dsusers.Tables[0].Rows[U]["DisplayName"].ToString() + "' data-bs-original-title='" + dsusers.Tables[0].Rows[U]["DisplayName"].ToString() + "'>";
                                                            usersdetailsowner += @"<div class='avatar-xxs'>
                            <div class='avatar-title rounded-circle bg-light text-primary'>" + dsusers.Tables[0].Rows[U]["splinameone"].ToString() + "</div></div>";
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

                                                    for (int V = 0; V < 3; V++)
                                                    {
                                                        if (dsusers.Tables[0].Rows[V]["fileupload1"].ToString() != "")
                                                        {
                                                            usersdetails += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title=" + dsusers.Tables[0].Rows[V]["DisplayName"].ToString() + " data-bs-original-title='" + dsusers.Tables[0].Rows[V]["DisplayName"].ToString() + "'><img src = '" + dsusers.Tables[0].Rows[V]["fileupload1"] + "' alt='' class='rounded-circle avatar-xxs'></a>";
                                                        }
                                                        else
                                                        {
                                                            usersdetails += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover'" +
                                         " data-bs-placement='top' title='" + dsusers.Tables[0].Rows[V]["DisplayName"].ToString() + "' data-bs-original-title='" + dsusers.Tables[0].Rows[V]["DisplayName"].ToString() + "'>";
                                                            usersdetails += @"<div class='avatar-xxs'><div class='avatar-title rounded-circle bg-light text-primary'>" + dsusers.Tables[0].Rows[V]["splinameone"].ToString() + "</div></div>";
                                                            usersdetails += @"</a>";
                                                        }
                                                    }

                                                }
                                                else
                                                {
                                                    cnt = "";
                                                    for (int V = 0; V < dsusers.Tables[0].Rows.Count; V++)
                                                    {
                                                        if (dsusers.Tables[0].Rows[k]["fileupload1"].ToString() != "")
                                                        {
                                                            //img src='/assets/images/avatar-1.jpg'
                                                            usersdetails += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='' data-bs-original-title='" + dsusers.Tables[0].Rows[V]["DisplayName"].ToString() + "'><img src = '" + dsusers.Tables[0].Rows[V]["fileupload1"] + "' alt='' class='rounded-circle avatar-xxs'></a>";
                                                        }
                                                        else
                                                        {
                                                            usersdetails += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover'  data-bs-placement='top' title='" + dsusers.Tables[0].Rows[V]["DisplayName"].ToString() + "' data-bs-original-title='" + dsusers.Tables[0].Rows[V]["DisplayName"].ToString() + "'>";
                                                            usersdetails += @"<div class='avatar-xxs'>
                            <div class='avatar-title rounded-circle bg-light text-primary'>" + dsusers.Tables[0].Rows[V]["splinameone"].ToString() + "</div></div>";
                                                            usersdetails += @"</a>";

                                                        }
                                                    }
                                                }

                                            }
                                        }



                                        //dependancy start //

                                        string cntdepndcy = "0";
                                        string depdecydetails = "";
                                        string depcytooltip = "";

                                        string strdepcysql = "Select * from Pms_tbl_Addsubtask where id = '" + ds.Tables[0].Rows[i]["id"] + "' and status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                                        DataSet dsdepcy = da.GetDataSetInline(strdepcysql);
                                        if (dsdepcy.Tables[0].Rows[0]["dependancy"].ToString() != "")
                                        {
                                            string[] arcntdep = dsdepcy.Tables[0].Rows[0]["dependancy"].ToString().Trim().Split(',');
                                            cntdepndcy = arcntdep.Length.ToString();
                                            DataSet dsdtlsdepcy = new DataSet();
                                            if (dsdepcy.Tables[0].Rows[0]["dependancy"].ToString().Trim() != "")
                                            {
                                                dsdtlsdepcy = da.GetDataSetInline("select id,Subtaskname from Pms_tbl_Addsubtask where id in(" + dsdepcy.Tables[0].Rows[0]["dependancy"].ToString().Trim() + ")");
                                                if (dsdtlsdepcy.Tables[0].Rows.Count > 1)
                                                {
                                                    cntdepndcy = (Convert.ToInt32(cntdepndcy) - 1).ToString();

                                                    // dependancy tool tip //
                                                    for (int l = 1; l < dsdtlsdepcy.Tables[0].Rows.Count; l++)
                                                    {
                                                        if (dsdtlsdepcy.Tables[0].Rows[l]["Subtaskname"].ToString() != string.Empty)
                                                        {
                                                            if (depcytooltip != string.Empty)
                                                            {
                                                                depcytooltip += "," + dsdtlsdepcy.Tables[0].Rows[l]["Subtaskname"].ToString();
                                                            }
                                                            else
                                                            {
                                                                depcytooltip += dsdtlsdepcy.Tables[0].Rows[l]["Subtaskname"].ToString();
                                                            }
                                                        }

                                                    }

                                                    for (int W = 0; k < 1; W++)
                                                    {
                                                        if (dsdtlsdepcy.Tables[0].Rows[W]["Subtaskname"].ToString() != "")
                                                        {

                                                            depdecydetails += "<span class='badge bg-primary'>" + dsdtlsdepcy.Tables[0].Rows[W]["Subtaskname"].ToString() + "</span>";
                                                        }
                                                        else
                                                        {
                                                            depdecydetails += "<span class='badge bg-primary'>" + dsdtlsdepcy.Tables[0].Rows[W]["Subtaskname"].ToString() + "</span>";
                                                        }
                                                    }

                                                }
                                                else
                                                {
                                                    cntdepndcy = "";
                                                    for (int W = 0; W < dsdtlsdepcy.Tables[0].Rows.Count; W++)
                                                    {
                                                        if (dsdtlsdepcy.Tables[0].Rows[k]["Subtaskname"].ToString() != "")
                                                        {
                                                            depdecydetails += "<span class='badge bg-primary'>" + dsdtlsdepcy.Tables[0].Rows[W]["Subtaskname"].ToString() + "</span>";
                                                        }
                                                        else
                                                        {
                                                            depdecydetails += "<span class='badge bg-primary'>" + dsdtlsdepcy.Tables[0].Rows[W]["Subtaskname"].ToString() + "</span>";
                                                        }
                                                    }
                                                }

                                            }
                                        }


                                        // dependancy end//


                                        


                                        //   completion  status  start //

                                        float Wattagecomplite = 0;
                                        float Wattagepending = 100;
                                        float TaskTotal = 0;
                                        string findtask = "select isnull(sum(ROUND(weightage, 2)),0)Wattagetasksum from Pms_tbl_Addsubtask where id='" + ds.Tables[0].Rows[i]["id"] + "' and status=1 and archive=0";
                                        DataSet dsssumtask = da.GetDataSetInline(findtask);

                                        try
                                        {
                                            TaskTotal = Convert.ToInt32(dsssumtask.Tables[0].Rows[0]["Wattagetasksum"]);
                                        }
                                        catch (Exception)
                                        {
                                        }

                                        if (TaskTotal > 0)
                                        {
                                            Wattagecomplite = TaskTotal;
                                            Wattagepending = Math.Abs(Wattagepending - Wattagecomplite);
                                        }

                                        //// completion task end //



                                        //   Issues count start //

                                        float Issuescomplite = 0;
                                        float Issuespending = 0;
                                        float IssuesrojectTotal = 0;
                                        float IssuesPhaseTotal = 0;
                                        float Issues_Total = 0;

                                        string str111 = "select COUNT(*) as countIssues from Pms_tbl_Addissues where ProjectId='" + ds.Tables[0].Rows[i]["ProjectId"] + "' and Milestoneid='" + ds.Tables[0].Rows[i]["Milestoneid"] + "' and  taskid='" + ds.Tables[0].Rows[i]["task"] + "' and  subtaskid='" + ds.Tables[0].Rows[i]["id"] + "' and Issueon='subtask' and status=1 and archive=0 ";
                                        DataSet dss111 = da.GetDataSetInline(str111);
                                        if (dss111.Tables[0].Rows.Count > 0)
                                        {

                                            try
                                            {
                                                IssuesPhaseTotal = Convert.ToInt32(dss111.Tables[0].Rows[0]["countIssues"]);
                                            }
                                            catch (Exception)
                                            {
                                            }
                                        }


                                        string findtask111 = "select COUNT(*) as count_Issues from Pms_tbl_Addissues where ProjectId='" + ds.Tables[0].Rows[i]["ProjectId"] + "' and Milestoneid='" + ds.Tables[0].Rows[i]["Milestoneid"] + "' and  taskid='" + ds.Tables[0].Rows[i]["task"] + "' and  subtaskid='" + ds.Tables[0].Rows[i]["id"] + "' and Issueon='subtask' and IssueStatus=6  and status=1 and archive=0";
                                        DataSet dsssumtask111 = da.GetDataSetInline(findtask111);
                                        if (dsssumtask111.Tables[0].Rows.Count > 0)
                                        {
                                            try
                                            {
                                                Issues_Total = Convert.ToInt32(dsssumtask111.Tables[0].Rows[0]["count_Issues"]);
                                            }
                                            catch (Exception)
                                            {

                                            }
                                        }


                                        IssuesrojectTotal = (IssuesPhaseTotal - Issues_Total);


                                        if (IssuesrojectTotal > 0)
                                        {
                                            Issuescomplite = Issues_Total;
                                            Issuespending = Math.Abs(IssuesPhaseTotal - Issues_Total);
                                        }

                                        if (IssuesrojectTotal == 0)
                                        {
                                            Issuescomplite = Issues_Total;
                                            Issuespending = Math.Abs(IssuesPhaseTotal - Issues_Total);
                                        }

                                        // Issues count end //


                                        Subtaskdata += "<tr>";
                                        Subtaskdata += "<td valign='top'>";
                                        Subtaskdata += (i + 1);
                                        Subtaskdata += "</td>";
                                        

                                        Subtaskdata += "<td valign='top'>";
                                        Subtaskdata += ds.Tables[0].Rows[i]["Subtaskname"].ToString(); //Sub task
                                        Subtaskdata += "</td>";



                                        // timer 

                                        Subtaskdata += "<td align ='center'>";


                                        // dependency check start //
                                        string depcheck = "";
                                        string deptaskname = "";
                                        string strdepcy = "SELECT  Split.value as dependancy,isnull(statuscomplete,0) statuscomplete FROM Pms_tbl_Addsubtask AS subtask CROSS APPLY STRING_SPLIT(subtask.dependancy, ',') as Split where subtask.status = 1 and subtask.archive = 0 and subtask.regid = '1'  and subtask.id = '" + ds.Tables[0].Rows[i]["id"].ToString() + "' ";
                                        DataSet dsdepcychk = da.GetDataSetInline(strdepcy);
                                        if (dsdepcychk.Tables[0].Rows.Count > 0)
                                        {
                                            for (int W = 0; W < dsdepcychk.Tables[0].Rows.Count; W++)
                                            {
                                                if (Convert.ToInt32(dsdepcychk.Tables[0].Rows[W]["statuscomplete"]) != 3)
                                                {
                                                    if (depcheck == "")
                                                    {
                                                        depcheck = dsdepcychk.Tables[0].Rows[W]["dependancy"].ToString();
                                                    }
                                                    else
                                                    {
                                                        depcheck += "," + dsdepcychk.Tables[0].Rows[W]["dependancy"].ToString();

                                                    }

                                                }
                                                else
                                                {
                                                    depcheck = "";
                                                }
                                            }
                                            if (depcheck != "")
                                            {
                                                deptaskname = get_depdencytaskname(depcheck);
                                            }
                                        }


                                        // dependency check end //



                                        string hour = "";
                                        string min = "";
                                        string sec = "";
                                        string mili = "";
                                        string pausetime = "";
                                        int ispause = 0;
                                        int isrun = 0;
                                        int isend = 0;
                                        string transactionid = "0";
                                        string strhourminuts = "select *  from Pms_tbl_Timerstatus where isnull(isend,0)=0 and type='subtask' and subtaskid='" + ds.Tables[0].Rows[i]["id"].ToString() + "' and taskid='" + ds.Tables[0].Rows[i]["task"].ToString() + "' and ProjectId='" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "' and Milestoneid='" + ds.Tables[0].Rows[i]["Milestoneid"].ToString() + "' and tasklist='" + ds.Tables[0].Rows[i]["tasklist"].ToString() + "'  and status=1 and archive=0 order by id desc";
                                        DataSet dshourmin = da.GetDataSetInline(strhourminuts);
                                        if (dshourmin.Tables[0].Rows.Count > 0)
                                        {
                                            //taskdata += "<script>autoplay_" + ds.Tables[0].Rows[i]["id"].ToString() + "();</script>";

                                            transactionid = dshourmin.Tables[0].Rows[0]["id"].ToString();
                                            if (dshourmin.Tables[0].Rows[0]["IsEnd"].ToString() == "1")
                                            {
                                                isend = 1;
                                            }
                                            else
                                            {
                                                isrun = 1;
                                                isend = 0;
                                            }

                                            if (dshourmin.Tables[0].Rows[0]["IsPaused"].ToString() == "1")
                                            {
                                                ispause = 1;
                                                string strpauseddetails = "select *  from Pms_Tbl_PauseStatus where Transactionid='" + transactionid + "' and isnull(StartByid,0)=0 and status=1 and archive=0 and type='subtask' order by id desc";
                                                DataSet dspaused = da.GetDataSetInline(strpauseddetails);

                                                if (dspaused.Tables[0].Rows.Count > 0)
                                                {
                                                    string chkpausemin = "0";

                                                    string strpauseddetails2 = "select Transactionid, sum(cast(isnull(PausedTime,0) as numeric)) tpause  from Pms_Tbl_PauseStatus where Transactionid='" + transactionid + "'  and status=1 and archive=0 and type='subtask' and isnumeric(isnull(PausedTime,0))=1 group by Transactionid";
                                                    DataSet dspaused2 = da.GetDataSetInline(strpauseddetails2);
                                                    if (dspaused2.Tables[0].Rows.Count > 0)
                                                    {
                                                        chkpausemin = dspaused2.Tables[0].Rows[0]["tpause"].ToString();
                                                    }
                                                    TimeSpan toatlhr = (Convert.ToDateTime(dspaused.Tables[0].Rows[0]["PauseDatetime"].ToString()) - Convert.ToDateTime(dshourmin.Tables[0].Rows[0]["StartTime"].ToString()));
                                                    TimeSpan t1 = TimeSpan.FromMinutes(Convert.ToInt32(chkpausemin));
                                                    //TimeSpan t1 = new TimeSpan(0,Convert.ToInt32(chkpausemin) ,0,0);
                                                    int diffdays = (toatlhr.Days) * 24;
                                                    toatlhr = toatlhr.Subtract(t1);

                                                    hour = (toatlhr.Hours + diffdays).ToString("00");
                                                    min = toatlhr.Minutes.ToString("00");
                                                    sec = toatlhr.Seconds.ToString("00");
                                                    mili = toatlhr.Milliseconds.ToString("000");

                                                }

                                            }
                                            else
                                            {
                                                //string starttime = dshourmin.Tables[0].Rows[0]["hour"].ToString()+":"+ dshourmin.Tables[0].Rows[0]["minute"].ToString()+":"+ dshourmin.Tables[0].Rows[0]["StartTime"].ToString();
                                                string chkpausemin = "0";
                                                string strpauseddetails2 = "select Transactionid, sum(cast(isnull(PausedTime,0) as numeric)) tpause  from Pms_Tbl_PauseStatus where Transactionid='" + transactionid + "'  and status=1 and archive=0 and type='subtask' and isnumeric(isnull(PausedTime,0))=1 group by Transactionid";
                                                DataSet dspaused2 = da.GetDataSetInline(strpauseddetails2);
                                                if (dspaused2.Tables[0].Rows.Count > 0)
                                                {
                                                    chkpausemin = (dspaused2.Tables[0].Rows[0]["tpause"].ToString() == "" ? "0" : dspaused2.Tables[0].Rows[0]["tpause"].ToString());
                                                }
                                                TimeSpan toatlhr = (DateTime.Now - Convert.ToDateTime(dshourmin.Tables[0].Rows[0]["StartTime"].ToString()));
                                                TimeSpan t1 = TimeSpan.FromMinutes(Convert.ToInt32(chkpausemin));
                                                //TimeSpan t1 = new TimeSpan(0,Convert.ToInt32(chkpausemin) ,0,0);
                                                int diffdays = (toatlhr.Days) * 24;
                                                toatlhr = toatlhr.Subtract(t1);
                                                hour = (toatlhr.Hours + diffdays).ToString();
                                                min = toatlhr.Minutes.ToString();
                                                sec = toatlhr.Seconds.ToString();
                                                mili = toatlhr.Milliseconds.ToString();
                                            }


                                        }
                                        else
                                        {
                                            hour = "00";
                                            min = "00";
                                            sec = "00";
                                            mili = "00";

                                        }


                                        string varid = "cron_" + ds.Tables[0].Rows[i]["id"].ToString();


                                        int showrun = 0;
                                        if (ispause == 1 || isrun == 1)
                                        {
                                            showrun = 1;
                                        }
                                        else
                                        {
                                            showrun = 0;
                                        }


                                        Subtaskdata += "<div  onclick='hideshow(divs_" + ds.Tables[0].Rows[i]["id"].ToString() + ",dinv_" + ds.Tables[0].Rows[i]["id"].ToString() + ")'>";

                                        Subtaskdata += "<button type='button' onclick='start_" + ds.Tables[0].Rows[i]["id"].ToString() + "(hour_" + ds.Tables[0].Rows[i]["id"].ToString() + ",minute_" + ds.Tables[0].Rows[i]["id"].ToString() + ",second_" + ds.Tables[0].Rows[i]["id"].ToString() + ",millisecond_" + ds.Tables[0].Rows[i]["id"].ToString() + "," + ds.Tables[0].Rows[i]["id"].ToString() + ");' id='btnauto_" + ds.Tables[0].Rows[i]["id"].ToString() + "' name='start' style='display:none;'><i class='bi bi-play-circle-fill text-secondary' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='Resume Timer'></i></button>";
                                        //taskdata += "<a href = 'javascript: void(0);' name='start' id='btnauto_" + ds.Tables[0].Rows[i]["id"].ToString() + "' style='display:none' onclick='start_" + ds.Tables[0].Rows[i]["id"].ToString() + "(hour_" + ds.Tables[0].Rows[i]["id"].ToString() + ",minute_" + ds.Tables[0].Rows[i]["id"].ToString() + ",second_" + ds.Tables[0].Rows[i]["id"].ToString() + ",millisecond_" + ds.Tables[0].Rows[i]["id"].ToString() + "," + ds.Tables[0].Rows[i]["id"].ToString() + ")'>strt</a>";

                                        if (depcheck != "")
                                        {

                                            Subtaskdata += "<button class='start_timer' type='button' name='start' style='background:none;border:0px;display:" + (showrun == 0 ? "block" : "none") + "'><i class='ri-timer-line fs-3xl' onclick='savestartdetails(" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "," + ds.Tables[0].Rows[i]["Milestoneid"].ToString() + "," + ds.Tables[0].Rows[i]["tasklist"].ToString() + "," + ds.Tables[0].Rows[i]["task"].ToString() + "," + ds.Tables[0].Rows[i]["id"].ToString() + ",\"" + deptaskname + "\")' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='Start Timer'></i></button>";
                                        }

                                        else
                                        {
                                            Subtaskdata += "<button class='start_timer' id='divs_" + ds.Tables[0].Rows[i]["id"].ToString() + "'  type='button' name='start' style='background:none;border:0px;display:" + (showrun == 0 ? "block" : "none") + "'><i class='ri-timer-line fs-3xl' onclick='savestartdetails(" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "," + ds.Tables[0].Rows[i]["Milestoneid"].ToString() + "," + ds.Tables[0].Rows[i]["tasklist"].ToString() + "," + ds.Tables[0].Rows[i]["task"].ToString() + "," + ds.Tables[0].Rows[i]["id"].ToString() + ",\"" + deptaskname + "\"),start_" + ds.Tables[0].Rows[i]["id"].ToString() + "(hour_" + ds.Tables[0].Rows[i]["id"].ToString() + ",minute_" + ds.Tables[0].Rows[i]["id"].ToString() + ",second_" + ds.Tables[0].Rows[i]["id"].ToString() + ",millisecond_" + ds.Tables[0].Rows[i]["id"].ToString() + "," + ds.Tables[0].Rows[i]["id"].ToString() + ")' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='Start Timer'></i></button>";

                                        }



                                        Subtaskdata += "<div class='timer_wrapper' style='display:" + (showrun == 1 ? "block" : "none") + ";' id='dinv_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>";
                                        Subtaskdata += "<button class='stop_timer' style='background:none;border:0px;'><i class='ri-timer-line fs-3xl' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='Stop Timer'></i></button>";
                                        Subtaskdata += "<div class='timer-content'>";
                                        Subtaskdata += "<button type ='button' name='stop'><i class='bi bi-stop-circle-fill text-danger' data-bs-toggle='modal' data-bs-target='#timer_stop' onclick='Binddetails_Timer(" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "," + ds.Tables[0].Rows[i]["Milestoneid"].ToString() + "," + ds.Tables[0].Rows[i]["tasklist"].ToString() + "," + ds.Tables[0].Rows[i]["task"].ToString() + "," + ds.Tables[0].Rows[i]["id"].ToString() + ",hour_" + ds.Tables[0].Rows[i]["id"].ToString() + ",minute_" + ds.Tables[0].Rows[i]["id"].ToString() + ",second_" + ds.Tables[0].Rows[i]["id"].ToString() + ",millisecond_" + ds.Tables[0].Rows[i]["id"].ToString() + "," + transactionid + "," + varid + ")'  data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='Stop Timer'></i></button> <button type='button' onclick='saveResumeDetails(" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "," + ds.Tables[0].Rows[i]["Milestoneid"].ToString() + "," + ds.Tables[0].Rows[i]["tasklist"].ToString() + "," + ds.Tables[0].Rows[i]["task"].ToString() + "," + ds.Tables[0].Rows[i]["id"].ToString() + "," + transactionid + "),start_" + ds.Tables[0].Rows[i]["id"].ToString() + "(hour_" + ds.Tables[0].Rows[i]["id"].ToString() + ",minute_" + ds.Tables[0].Rows[i]["id"].ToString() + ",second_" + ds.Tables[0].Rows[i]["id"].ToString() + ",millisecond_" + ds.Tables[0].Rows[i]["id"].ToString() + "," + ds.Tables[0].Rows[i]["id"].ToString() + "),hideshowPausebutton(btnpause_" + ds.Tables[0].Rows[i]["id"].ToString() + ",btnresum_" + ds.Tables[0].Rows[i]["id"].ToString() + ")' id='btnresum_" + ds.Tables[0].Rows[i]["id"].ToString() + "' name='start' style='display:" + (ispause == 1 ? "inline-block" : "none") + ";'><i class='bi bi-play-circle-fill text-secondary' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='Resume Timer'></i></button> <button type ='button' style='display:" + (ispause == 0 ? "inline-block" : "none") + ";' id='btnpause_" + ds.Tables[0].Rows[i]["id"].ToString() + "' name='pause' class='me-2'><i class='bi bi-pause-circle-fill text-warning' data-bs-toggle='modal' data-bs-target='#addnotes' onclick='BindPausedetails_Notes(" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "," + ds.Tables[0].Rows[i]["Milestoneid"].ToString() + "," + ds.Tables[0].Rows[i]["tasklist"].ToString() + "," + ds.Tables[0].Rows[i]["task"].ToString() + "," + ds.Tables[0].Rows[i]["id"].ToString() + "," + transactionid + "," + varid + ")'  data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='Pause Timer'></i></button>";
                                        Subtaskdata += "<input type='hidden' id='hdntrans_" + ds.Tables[0].Rows[i]["id"].ToString() + "'  value='" + transactionid + "' /><span id='hour_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>" + hour + "</span>:<span id='minute_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>" + min + "</span>:<span id='second_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>" + sec + "</span>:<span id='millisecond_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>000</span>";
                                        Subtaskdata += "</div>";
                                        Subtaskdata += "</div>";
                                        Subtaskdata += "</div>";
                                        Subtaskdata += "</td>";



                                        //Dependency 
                                        string cntdep = "";
                                        if (cntdepndcy != "" && cntdepndcy != "0")
                                        {
                                            cntdep = "+" + cntdepndcy;

                                        }
                                        Subtaskdata += "<td style = 'width:100px;'>";
                                        Subtaskdata += "<div class='d-flex'>";
                                        Subtaskdata += "<div class='flex-grow-1'>";
                                        Subtaskdata += "<div class='flex-wrap gap-2' id='lblDependancy_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>";
                                        Subtaskdata += depdecydetails;
                                        Subtaskdata += "<span class='cursor-pointer' data-bs-toggle='tooltip' data-bs-html='true' data-bs-placement='top' data-bs-trigger='hover' title='" + depcytooltip + "'>" + cntdep + "</span>";
                                        Subtaskdata += "</div>";
                                        Subtaskdata += "</div>";
                                        Subtaskdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                                        Subtaskdata += "<a href = 'javascript:void(0);' type='button' id='btndependcy_" + ds.Tables[0].Rows[i]["id"].ToString() + "' onclick='Binddetails_Dependency(" + ds.Tables[0].Rows[i]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#adddependency'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Dependency'>+</span></a>";
                                        Subtaskdata += "</div>";
                                        Subtaskdata += "</div>";
                                        Subtaskdata += "</td>";


                                        //Owner

                                        Subtaskdata += "<td>";
                                        Subtaskdata += "<div class='d-flex'>";
                                        Subtaskdata += "<div class='flex-grow-1'>";
                                        Subtaskdata += "<div class='avatar-group' id='lblowner_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>";
                                        Subtaskdata += usersdetailsowner;

                                        //projectdata += "</a>";
                                        Subtaskdata += "<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='" + ownertooltip + "' data-bs-original-title='more'>";

                                        if (cntownr != "" && cnt != "0")
                                        {
                                            Subtaskdata += "<div class='avatar-xxs'>";
                                            Subtaskdata += "<div class='avatar-title rounded-circle'>";

                                            Subtaskdata += (cntownr + "+");
                                            Subtaskdata += "</div>";
                                            Subtaskdata += "</div>";
                                        }

                                        Subtaskdata += "</a>";
                                        Subtaskdata += "</div>";
                                        Subtaskdata += "</div>";
                                        Subtaskdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                                        Subtaskdata += "<a href ='javascript:void(0);' type='button' id='btnownr_" + ds.Tables[0].Rows[i]["id"].ToString() + "' onclick='Binddetails_Owner(" + ds.Tables[0].Rows[i]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#changeowner'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Owner'>+</span></a>";
                                        Subtaskdata += "</div>";
                                        Subtaskdata += "</div>";
                                        Subtaskdata += "</td>";



                                        //team
                                        Subtaskdata += "<td>";
                                        Subtaskdata += "<div class='d-flex'>";
                                        Subtaskdata += "<div class='flex-grow-1'>";
                                        Subtaskdata += "<div class='avatar-group' id='lbluser_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>";
                                        Subtaskdata += usersdetails;

                                        //projectdata += "</a>";
                                        Subtaskdata += "<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='" + teamtooltip + "' data-bs-original-title='more'>";

                                        if (cnt != "" && cnt != "0")
                                        {
                                            Subtaskdata += "<div class='avatar-xxs'>";
                                            Subtaskdata += "<div class='avatar-title rounded-circle'>";

                                            Subtaskdata += (cnt + "+");
                                            Subtaskdata += "</div>";
                                            Subtaskdata += "</div>";
                                        }

                                        Subtaskdata += "</a>";
                                        Subtaskdata += "</div>";
                                        Subtaskdata += "</div>";
                                        Subtaskdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                                        Subtaskdata += "<a href ='javascript:void(0);' type='button' id='btnteam_" + ds.Tables[0].Rows[i]["id"].ToString() + "' onclick='Binddetails_Team(" + ds.Tables[0].Rows[i]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#addteam'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Team'>+</span></a>";
                                        Subtaskdata += "</div>";
                                        Subtaskdata += "</div>";
                                        Subtaskdata += "</td>";


                                        // status

                                        string strvalcolor = get_Status_Color(ds.Tables[0].Rows[i]["P_Status"].ToString());
                                        Subtaskdata += "<td>";
                                        Subtaskdata += "<div class='btn-group d-block'>";
                                        Subtaskdata += "<a href ='javascript:void(0);' id='btnsts_" + ds.Tables[0].Rows[i]["id"].ToString() + "' type='button' onclick=setsubtaskstustid('" + ds.Tables[0].Rows[i]["id"].ToString() + "',this.id) data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + strvalcolor + "'>" + get_Projectstatus(ds.Tables[0].Rows[i]["P_Status"].ToString()) + "</a>";
                                        Subtaskdata += "<div class='dropdown-menu lead_form_body p-0'>";
                                        Subtaskdata += retval;
                                        Subtaskdata += "</div>";
                                        Subtaskdata += "</div>";
                                        Subtaskdata += "</td>";


                                        //priority
                                        string strprioritycolor = get_PriorityColor(ds.Tables[0].Rows[i]["Priority"].ToString());
                                        Subtaskdata += "<td>";
                                        Subtaskdata += "<div class='btn-group d-block'>";
                                        Subtaskdata += "<a href ='javascript:void(0);' id='btnpri_" + ds.Tables[0].Rows[i]["id"].ToString() + "' type='button' onclick=setpriorityid('" + ds.Tables[0].Rows[i]["id"].ToString() + "',this.id) data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + strprioritycolor + "'>" + get_PriorityName(ds.Tables[0].Rows[i]["Priority"].ToString()) + "</a>";
                                        Subtaskdata += "<div class='dropdown-menu lead_form_body p-0'>";
                                        Subtaskdata += strvalprority;
                                        Subtaskdata += "</div>";
                                        Subtaskdata += "</div>";
                                        Subtaskdata += "</td>";

                                        Subtaskdata += "<td valign='top'>";
                                        Subtaskdata += ds.Tables[0].Rows[i]["Budget"].ToString(); // budget 
                                        Subtaskdata += "</td>";

                                        Subtaskdata += "<td valign='top'>";
                                        Subtaskdata += ds.Tables[0].Rows[i]["Actual"].ToString(); // Actual 
                                        Subtaskdata += "</td>";
                                        
                                        Subtaskdata += "<td valign='top'>";
                                        Subtaskdata += cmf.dateConvert(ds.Tables[0].Rows[i]["StartDate"].ToString()); // start date
                                        Subtaskdata += "</td>";

                                        Subtaskdata += "<td valign='top'>";
                                        Subtaskdata += cmf.dateConvert(ds.Tables[0].Rows[i]["Targetdate"].ToString()); //target date
                                        Subtaskdata += "</td>";


                                        Subtaskdata += "<td valign='top'>";
                                        Subtaskdata += cmf.dateConvert(ds.Tables[0].Rows[i]["TaskDuration"].ToString()); //Task Duration Hours
                                        Subtaskdata += "</td>";

                                        //Storing input Dates  
                                        DateTime StartDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"].ToString() == "" ? "0" : ds.Tables[0].Rows[i]["StartDate"].ToString());
                                        DateTime Targetdate = Convert.ToDateTime(ds.Tables[0].Rows[i]["Targetdate"].ToString() == "" ? "0" : ds.Tables[0].Rows[i]["Targetdate"].ToString());
                                        TimeSpan objTimeSpan = Targetdate - StartDate;
                                        double Days = Convert.ToDouble(objTimeSpan.TotalDays);
                                        Days = Days + 1;

                                        DateTime Enddate = Convert.ToDateTime(ds.Tables[0].Rows[i]["Targetdate"].ToString() == "" ? "0" : ds.Tables[0].Rows[i]["Targetdate"].ToString());
                                        DateTime Currentdate = DateTime.Now;
                                        TimeSpan objtimeleftspn = Enddate - Currentdate;
                                        double leftDays = Convert.ToDouble(objtimeleftspn.Days);
                                        leftDays = leftDays + 1;

                                        string leftdlydys = "";
                                        if (leftDays < 0)
                                        {
                                            leftdlydys = "days delayed";
                                        }
                                        else
                                        {
                                            leftdlydys = "days left";
                                        }


                                        //start days months  
                                        string startmonthname = "";
                                        int ststartdys = StartDate.Day;
                                        int strtmonth = StartDate.Month;
                                        if (strtmonth == 1)
                                        {
                                            startmonthname = "Jan";
                                        }
                                        if (strtmonth == 2)
                                        {
                                            startmonthname = "Feb";
                                        }
                                        if (strtmonth == 3)
                                        {
                                            startmonthname = "Mar";
                                        }

                                        if (strtmonth == 4)
                                        {
                                            startmonthname = "Apr";
                                        }

                                        if (strtmonth == 5)
                                        {
                                            startmonthname = "May";
                                        }

                                        if (strtmonth == 6)
                                        {
                                            startmonthname = "Jun";
                                        }

                                        if (strtmonth == 7)
                                        {
                                            startmonthname = "Jul";
                                        }

                                        if (strtmonth == 8)
                                        {
                                            startmonthname = "Aug";
                                        }

                                        if (strtmonth == 9)
                                        {
                                            startmonthname = "Sep";
                                        }

                                        if (strtmonth == 10)
                                        {
                                            startmonthname = "Oct";
                                        }

                                        if (strtmonth == 11)
                                        {
                                            startmonthname = "Nov";
                                        }

                                        if (strtmonth == 12)
                                        {
                                            startmonthname = "Dec";
                                        }



                                        //end days months 
                                        string endmonthname = "";
                                        int enddys = Targetdate.Day;
                                        int endmonth = Targetdate.Month;
                                        if (endmonth == 1)
                                        {
                                            endmonthname = "Jan";
                                        }
                                        if (endmonth == 2)
                                        {
                                            endmonthname = "Feb";
                                        }
                                        if (endmonth == 3)
                                        {
                                            endmonthname = "Mar";
                                        }

                                        if (endmonth == 4)
                                        {
                                            endmonthname = "Apr";
                                        }

                                        if (endmonth == 5)
                                        {
                                            endmonthname = "May";
                                        }

                                        if (endmonth == 6)
                                        {
                                            endmonthname = "Jun";
                                        }

                                        if (endmonth == 7)
                                        {
                                            endmonthname = "Jul";
                                        }

                                        if (endmonth == 8)
                                        {
                                            endmonthname = "Aug";
                                        }

                                        if (endmonth == 9)
                                        {
                                            endmonthname = "Sep";
                                        }

                                        if (endmonth == 10)
                                        {
                                            endmonthname = "Oct";
                                        }

                                        if (endmonth == 11)
                                        {
                                            endmonthname = "Nov";
                                        }

                                        if (endmonth == 12)
                                        {
                                            endmonthname = "Dec";
                                        }





                                        Subtaskdata += "<td>";
                                        Subtaskdata += "<a href = 'javascript:void(0);'>";
                                        Subtaskdata += "<div class='progress progress-xl bg-dark position-relative' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + leftDays.ToString().Replace("-", "") + " " + leftdlydys + "'>";
                                        Subtaskdata += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 70%;' aria-valuemax='100'></div>";
                                        Subtaskdata += "<div class='dayswrapper'>";
                                        Subtaskdata += "<span class='fromtodate'>" + ststartdys + " " + startmonthname + " - " + enddys + " " + endmonthname + "</span>";
                                        Subtaskdata += "<span class='totaldays'>" + Days + " days</span>";
                                        Subtaskdata += "</div>";
                                        Subtaskdata += "</div>";
                                        Subtaskdata += "</a>";
                                        Subtaskdata += "</td>";     //Timeline




                                        //Sub Task extended timeleine 

                                        string subtask_estendleftdlydys = "";
                                        string subTask_estendstartmonthname = "";
                                        string subtask_estendendmonthname = "";
                                        double subtask_estendleftDays = 0;
                                        int subtask_estendststartdys = 0;
                                        int subtask_estendenddys = 0;
                                        double subtask_estendDays = 0;
                                        string subtask_extenddatebind = "";
                                        string subtask_strextend = "select id,extendStartdate as StartDate,extendEnddate as TargetDate from Pms_Tbl_Extended_Timeline where  status=1 and Type='subtask' and archive=0  and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and taskid = '" + ds.Tables[0].Rows[i]["id"].ToString() + "'  ";
                                        DataSet subtask_dsstrextend = da.GetDataSetInline(subtask_strextend);
                                        if (subtask_dsstrextend.Tables[0].Rows.Count > 0)
                                        {
                                            for (int ST = 0; ST < subtask_dsstrextend.Tables[0].Rows.Count; ST++)
                                            {
                                                DateTime subtask_estendStartDate = Convert.ToDateTime(subtask_dsstrextend.Tables[0].Rows[ST]["StartDate"].ToString() == "" ? "0" : subtask_dsstrextend.Tables[0].Rows[ST]["StartDate"].ToString());
                                                DateTime subtask_estendTargetdate = Convert.ToDateTime(subtask_dsstrextend.Tables[0].Rows[ST]["TargetDate"].ToString() == "" ? "0" : subtask_dsstrextend.Tables[0].Rows[ST]["TargetDate"].ToString());
                                                TimeSpan subtask_estendobjTimeSpan = subtask_estendTargetdate - subtask_estendStartDate;
                                                subtask_estendDays = Convert.ToDouble(subtask_estendobjTimeSpan.TotalDays);
                                                subtask_estendDays = subtask_estendDays + 1;


                                                DateTime subtask_estendEnddate = Convert.ToDateTime(subtask_dsstrextend.Tables[0].Rows[ST]["Targetdate"].ToString() == "" ? "0" : subtask_dsstrextend.Tables[0].Rows[ST]["Targetdate"].ToString());
                                                DateTime subtask_estendCurrentdate = DateTime.Now;
                                                TimeSpan subtask_estendobjtimeleftspn = subtask_estendEnddate - subtask_estendCurrentdate;
                                                subtask_estendleftDays = Convert.ToDouble(subtask_estendobjtimeleftspn.Days);
                                                subtask_estendleftDays = subtask_estendleftDays + 1;


                                                if (subtask_estendleftDays < 0)
                                                {
                                                    subtask_estendleftdlydys = "days delayed";
                                                }
                                                else
                                                {
                                                    subtask_estendleftdlydys = "days left";
                                                }

                                                //start days months  

                                                subtask_estendststartdys = subtask_estendCurrentdate.Day;
                                                int subtask_estendstrtmonth = subtask_estendCurrentdate.Month;
                                                if (subtask_estendstrtmonth == 1)
                                                {
                                                    subTask_estendstartmonthname = "Jan";
                                                }
                                                if (subtask_estendstrtmonth == 2)
                                                {
                                                    subTask_estendstartmonthname = "Feb";
                                                }
                                                if (subtask_estendstrtmonth == 3)
                                                {
                                                    subTask_estendstartmonthname = "Mar";
                                                }

                                                if (subtask_estendstrtmonth == 4)
                                                {
                                                    subTask_estendstartmonthname = "Apr";
                                                }

                                                if (subtask_estendstrtmonth == 5)
                                                {
                                                    subTask_estendstartmonthname = "May";
                                                }

                                                if (subtask_estendstrtmonth == 6)
                                                {
                                                    subTask_estendstartmonthname = "Jun";
                                                }

                                                if (subtask_estendstrtmonth == 7)
                                                {
                                                    subTask_estendstartmonthname = "Jul";
                                                }

                                                if (subtask_estendstrtmonth == 8)
                                                {
                                                    subTask_estendstartmonthname = "Aug";
                                                }

                                                if (subtask_estendstrtmonth == 9)
                                                {
                                                    subTask_estendstartmonthname = "Sep";
                                                }

                                                if (subtask_estendstrtmonth == 10)
                                                {
                                                    subTask_estendstartmonthname = "Oct";
                                                }

                                                if (subtask_estendstrtmonth == 11)
                                                {
                                                    subTask_estendstartmonthname = "Nov";
                                                }

                                                if (subtask_estendstrtmonth == 12)
                                                {
                                                    subTask_estendstartmonthname = "Dec";
                                                }

                                                //end days months 

                                                subtask_estendenddys = subtask_estendTargetdate.Day;
                                                int subtask_estendendmonth = subtask_estendTargetdate.Month;
                                                if (subtask_estendendmonth == 1)
                                                {
                                                    subtask_estendendmonthname = "Jan";
                                                }
                                                if (subtask_estendendmonth == 2)
                                                {
                                                    subtask_estendendmonthname = "Feb";
                                                }
                                                if (subtask_estendendmonth == 3)
                                                {
                                                    subtask_estendendmonthname = "Mar";
                                                }

                                                if (subtask_estendendmonth == 4)
                                                {
                                                    subtask_estendendmonthname = "Apr";
                                                }

                                                if (subtask_estendendmonth == 5)
                                                {
                                                    subtask_estendendmonthname = "May";
                                                }

                                                if (subtask_estendendmonth == 6)
                                                {
                                                    subtask_estendendmonthname = "Jun";
                                                }

                                                if (subtask_estendendmonth == 7)
                                                {
                                                    subtask_estendendmonthname = "Jul";
                                                }

                                                if (subtask_estendendmonth == 8)
                                                {
                                                    subtask_estendendmonthname = "Aug";
                                                }

                                                if (subtask_estendendmonth == 9)
                                                {
                                                    subtask_estendendmonthname = "Sep";
                                                }

                                                if (subtask_estendendmonth == 10)
                                                {
                                                    subtask_estendendmonthname = "Oct";
                                                }

                                                if (subtask_estendendmonth == 11)
                                                {
                                                    subtask_estendendmonthname = "Nov";
                                                }

                                                if (subtask_estendendmonth == 12)
                                                {
                                                    subtask_estendendmonthname = "Dec";
                                                }



                                                subtask_extenddatebind += "<a href = 'javascript:void(0);' class='timesheet-wrap'>";
                                                subtask_extenddatebind += "<div class='progress progress-xl bg-dark position-relative me-1' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + subtask_estendststartdys + " " + subTask_estendstartmonthname + " - " + subtask_estendenddys + " " + subtask_estendendmonthname + " (" + subtask_estendleftDays.ToString().Replace("-", "") + " " + subtask_estendleftdlydys + ")'>";
                                                subtask_extenddatebind += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 100%;' aria-valuemax='100'>" + subtask_estendDays + " days</div>";
                                                subtask_extenddatebind += "</div>";
                                                subtask_extenddatebind += "</a>";



                                            }

                                        }




                                        Subtaskdata += "<td>";
                                        Subtaskdata += "<div class='d-flex'>";
                                        Subtaskdata += "<div class='flex-grow-1 align-self-center me-2'>";
                                        Subtaskdata += "<div class='extended_time' id='lblExtnddtsubtask_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>";

                                        Subtaskdata += "" + subtask_extenddatebind + "";

                                        Subtaskdata += "</div>";
                                        Subtaskdata += "</div>";

                                        if (leftDays < 0)
                                        {
                                            Subtaskdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                                            Subtaskdata += "<a href ='javascript:void(0);' type='button' id='btnextndtsubtask_" + ds.Tables[0].Rows[i]["id"].ToString() + "'  onclick='Bindextenddatesubtask_Details(" + ds.Tables[0].Rows[i]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#ExtendedTimline_subtask'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Extended Day'>+</span></a>";
                                            Subtaskdata += "</div>";
                                        }

                                        Subtaskdata += "</div>";
                                        Subtaskdata += "</div>";
                                        Subtaskdata += "</td>";




                                        // Stage
                                        string strvalstagecolor = get_Stage_Color(ds.Tables[0].Rows[i]["stage"].ToString());
                                        Subtaskdata += "<td>";
                                        Subtaskdata += "<div class='btn-group d-block'>";
                                        Subtaskdata += "<a href ='javascript:void(0);' id='btnStage_" + ds.Tables[0].Rows[i]["id"].ToString() + "' type='button' onclick=setStageid('" + ds.Tables[0].Rows[i]["id"].ToString() + "',this.id) data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + strvalstagecolor + "'>" + get_Stagename(ds.Tables[0].Rows[i]["stage"].ToString()) + "</a>";
                                        Subtaskdata += "<div class='dropdown-menu lead_form_body p-0'>";
                                        Subtaskdata += retstageval;
                                        Subtaskdata += "</div>";
                                        Subtaskdata += "</div>";
                                        Subtaskdata += "</td>";


                                        //task completion status start 

                                        Subtaskdata += "<td>";
                                        if (Wattagecomplite == 0)
                                        {

                                            Subtaskdata += "<div class='progress progress-xl cursor-pointer'>";
                                            Subtaskdata += "<div class='progress progress-xl bg-light position-relative' data-bs-toggle='tooltip' data-bs-placement='top' title='Complete(0%)' role='progressbar' style='width:5%'><span style='text-align:center;'>0</span>";
                                            Subtaskdata += "</div>";
                                            Subtaskdata += "</div>";

                                        }

                                        else
                                        {

                                            Subtaskdata += "<div class='progress progress-xl cursor-pointer'>";
                                            Subtaskdata += "<div class='progress-bar bg-success' data-bs-toggle='tooltip' data-bs-placement='top' title='Complete (" + Wattagecomplite + "%)' role='progressbar' style='width:" + Wattagecomplite + "%'>" + Wattagecomplite + "%";
                                            Subtaskdata += "</div>";
                                            Subtaskdata += "<div class='progress-bar bg-warning' data-toggle='tooltip' data-bs-placemen='top' title='Pending (" + Wattagepending + "%)' role='progressbar' style='width:" + Wattagepending + "%'>" + Wattagepending + "%";
                                            Subtaskdata += "</div>";
                                            Subtaskdata += "</div>";

                                        }

                                        Subtaskdata += "</td>";


                                        // completion status end //


                                        //Issues

                                        Subtaskdata += "<td valign='top'>";

                                        float completetepercentissues = 0;
                                        float Pendingepercentissues = 0;
                                        completetepercentissues = Issuescomplite * 100 / IssuesPhaseTotal;
                                        Pendingepercentissues = Issuespending * 100 / IssuesPhaseTotal;

                                        Subtaskdata += "<div class='d-flex'>";
                                        Subtaskdata += "<div class='flex-grow-1 align-self-center'>";

                                        Subtaskdata += "<div class='progress progress-xl cursor-pointer'>";
                                        Subtaskdata += "<div class='progress-bar bg-success' role='progressbar' style='width: " + fns.ToFormattedCurrencyString(completetepercentissues.ToString()) + "%' aria-valuemin='0' aria-valuemax='100' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + fns.ToFormattedCurrencyString(completetepercentissues.ToString()) + "% [" + Issuescomplite + " of " + IssuesPhaseTotal + " Issues Completed]'>" + Issuescomplite + "</div>";

                                        Subtaskdata += "<div class='progress-bar bg-warning' role='progressbar' style='width: " + fns.ToFormattedCurrencyString(Pendingepercentissues.ToString()) + "%' aria-valuemin='0' aria-valuemax='100' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + fns.ToFormattedCurrencyString(Pendingepercentissues.ToString()) + "% [" + Issuespending + " of " + IssuesPhaseTotal + " Issues Not Completed]'>" + Issuespending + "</div>";
                                        Subtaskdata += "</div>";
                                        Subtaskdata += "</div>";

                                        Subtaskdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                                        Subtaskdata += "<a href ='javascript:void(0);' type='button' data-bs-toggle='offcanvas' data-bs-target=#canvas_popup onclick=loadsrc('inner_popup','/AddIssuesPopup.aspx?id=" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "&milstoneid=" + ds.Tables[0].Rows[i]["Milestoneid"].ToString() + "&tasklistid=" + ds.Tables[0].Rows[i]["tasklist"].ToString() + "&taskid=" + ds.Tables[0].Rows[i]["task"].ToString() + "&subtaskid=" + ds.Tables[0].Rows[i]["id"].ToString() + "&type=subtask')><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Issue'>+</span></a>";

                                        Subtaskdata += "</div>";
                                        Subtaskdata += "</td>";

                                        //Subtaskdata += "<td valign='top'>";
                                        //Subtaskdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                                        //Subtaskdata += "<a href ='javascript:void(0);' type='button' data-bs-toggle='offcanvas' data-bs-target=#canvas_popup onclick=loadsrc('inner_popup','/AddIssuesPopup.aspx?id=" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "&milstoneid=" + ds.Tables[0].Rows[i]["Milestoneid"].ToString() + "&tasklistid=" + ds.Tables[0].Rows[i]["tasklist"].ToString() + "&taskid=" + ds.Tables[0].Rows[i]["task"].ToString() + "&subtaskid=" + ds.Tables[0].Rows[i]["id"].ToString() + "&type=subtask')><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Issue'>+</span></a>";
                                        //Subtaskdata += "</div>";  // isssues 
                                        //Subtaskdata += "</td>";


                                        string Workinghors = "0";
                                        string strworkinghrs = "select  sum(cast(isnull(hour,0) as numeric)) Workhors  from Pms_tbl_Timerstatus where subtaskid='" + ds.Tables[0].Rows[i]["id"].ToString() + "' and status=1 and isnull(IsEnd,0)=1 and archive=0 and type='subtask'";
                                        DataSet dsworkghrs = da.GetDataSetInline(strworkinghrs);
                                        if (dsworkghrs.Tables[0].Rows.Count > 0)
                                        {

                                            Workinghors = dsworkghrs.Tables[0].Rows[0]["Workhors"].ToString();

                                        }

                                        Subtaskdata += "<td valign='top'>";
                                        Subtaskdata += Workinghors;  //Work Hours (W)
                                        Subtaskdata += "</td>";

                                        Subtaskdata += "<td valign='top'>";
                                        Subtaskdata += "";   //Timelog Total (T)
                                        Subtaskdata += "</td>";


                                        Subtaskdata += "<td valign='top'>";
                                        Subtaskdata += "";    //Difference (W-T)
                                        Subtaskdata += "</td>";
                                        

                                        Subtaskdata += " <td><a href='javascript:void(0);' Class='btn btn-info btn-sm' type='button' onclick='Binddetails(" + ds.Tables[0].Rows[i]["id"].ToString() + ")' data-bs-toggle='offcanvas' data-bs-target='#addtask'><i class='ri-edit-line'></i></a>";
                                        Subtaskdata += "&nbsp;&nbsp;<a href='javascript:void(0);' Class='btn btn-danger btn-sm' type='button' onclick='deletedata(" + ds.Tables[0].Rows[i]["id"].ToString() + ")'><i class='ri-delete-bin-5-line'></i></a>";
                                        Subtaskdata += "</td>";


                                        Subtaskdata += "</tr>";
                                    }



                                    //subtask task add //

                                    Subtaskdata += "<td valign='top'>";
                                    Subtaskdata += "";
                                    Subtaskdata += "</td>";


                                    Subtaskdata += "<td>";
                                    Subtaskdata += "<div class='d-flex'>";
                                    Subtaskdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                                    Subtaskdata += "<a href = 'javascript:void(0);' type='button' onclick='AddMultisubtask(" + dsproject.Tables[0].Rows[P]["id"].ToString() + "," + ds1.Tables[0].Rows[j]["id"].ToString() + ","+ ds2.Tables[0].Rows[k]["id"].ToString() + ")' data-bs-toggle='modal' data-bs-target='#addmultitaskadd'><span><b>Add SubTask</b></span></a>";
                                    Subtaskdata += "</div>";
                                    Subtaskdata += "</div>";
                                    Subtaskdata += "</td>";

                                    Subtaskdata += "<td valign='top' style='text-align:center'>";
                                    Subtaskdata += "";
                                    Subtaskdata += "</td>";



                                    Subtaskdata += "<td valign='top'>";
                                    Subtaskdata += "";
                                    Subtaskdata += "</td>";

                                    Subtaskdata += "<td valign='top'>";
                                    Subtaskdata += "";
                                    Subtaskdata += "</td>";

                                    Subtaskdata += "<td valign='top'>";
                                    Subtaskdata += "";
                                    Subtaskdata += "</td>";

                                    Subtaskdata += "<td valign='top'>";
                                    Subtaskdata += "";
                                    Subtaskdata += "</td>";

                                    Subtaskdata += "<td valign='top'>";
                                    Subtaskdata += "";
                                    Subtaskdata += "</td>";

                                    Subtaskdata += "<td valign='top'>";
                                    Subtaskdata += "";
                                    Subtaskdata += "</td>";

                                    Subtaskdata += "<td valign='top'>";
                                    Subtaskdata += "";
                                    Subtaskdata += "</td>";

                                    Subtaskdata += "<td valign='top'>";
                                    Subtaskdata += "";
                                    Subtaskdata += "</td>";

                                    Subtaskdata += "<td valign='top'>";
                                    Subtaskdata += "";
                                    Subtaskdata += "</td>";

                                    Subtaskdata += "<td valign='top'>";
                                    Subtaskdata += "";
                                    Subtaskdata += "</td>";

                                    Subtaskdata += "<td valign='top'>";
                                    Subtaskdata += "";
                                    Subtaskdata += "</td>";

                                    Subtaskdata += "<td valign='top'>";
                                    Subtaskdata += "";
                                    Subtaskdata += "</td>";

                                    Subtaskdata += "<td valign='top'>";
                                    Subtaskdata += "";
                                    Subtaskdata += "</td>";

                                    Subtaskdata += "<td valign='top'>";
                                    Subtaskdata += "";
                                    Subtaskdata += "</td>";

                                    Subtaskdata += "<td valign='top'>";
                                    Subtaskdata += "";
                                    Subtaskdata += "</td>";

                                    Subtaskdata += "<td valign='top'>";
                                    Subtaskdata += "";
                                    Subtaskdata += "</td>";

                                    Subtaskdata += "<td valign='top'>";
                                    Subtaskdata += "";
                                    Subtaskdata += "</td>";

                                    Subtaskdata += "<td valign='top'>";
                                    Subtaskdata += "";
                                    Subtaskdata += "</td>";

                                    Subtaskdata += "<td valign='top'>";
                                    Subtaskdata += "";
                                    Subtaskdata += "</td>";

                                    //Subtaskdata += "<td valign='top'>";
                                    //Subtaskdata += "";
                                    //Subtaskdata += "</td>";

                                    //Subtaskdata += "<td valign='top'>";
                                    //Subtaskdata += "";
                                    //Subtaskdata += "</td>";


                                    Subtaskdata += "</tr>";

                                }


                            }
                        }



                    }


                }

            }



           
        }

        Subtaskdata += "</tbody>";
        Subtaskdata += "</table>";

        return Subtaskdata;
    }

   


    [WebMethod]
    public static string Bindsubtaskdetail(int RecordId = 0)
    {
        DataAccess da = new DataAccess();
        DataSet ds;
        string strsql = "select * from Pms_tbl_Addsubtask where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
        ds = da.GetDataSetInline(strsql);

        return ds.GetXml();
    }



    [WebMethod]
    public static string Deletesubtaskdata(int RecordId = 0)
    {
        string retval = "0";

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "update  Pms_tbl_Addsubtask set  status=0,archive=1  where  id='" + RecordId + "' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        ds = da.GetDataSetInline(strsql);


        string taskid = "";
        string strfindtaskid = "select *  from Pms_tbl_Addsubtask where id='" + RecordId + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
        DataSet dstskidfnd = da.GetDataSetInline(strfindtaskid);
        if (dstskidfnd.Tables[0].Rows.Count > 0)
        {

            taskid = dstskidfnd.Tables[0].Rows[0]["task"].ToString();

        }



        int checkweitage = 0;
        decimal wetagecount = 0;
        decimal totalwetage = 0;
        string strweightage = "select isnull(checkTaskWeightage ,0) as checkTaskWeightage  from pms_tbl_addtask where id='" + taskid + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
        DataSet dsweitagecheck = da.GetDataSetInline(strweightage);
        if (dsweitagecheck.Tables[0].Rows.Count > 0)
        {

            checkweitage = Convert.ToInt32(dsweitagecheck.Tables[0].Rows[0]["checkmileWeightage"]);

        }

        if (checkweitage == 1)
        {
            string strweightagecount = "select count (*) as WeitageCount from Pms_tbl_Addsubtask where task='" + taskid + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
            DataSet dsweitagecount = da.GetDataSetInline(strweightagecount);
            if (dsweitagecount.Tables[0].Rows.Count > 0)
            {

                wetagecount = Convert.ToInt32(dsweitagecount.Tables[0].Rows[0]["WeitageCount"]);

            }

            totalwetage = 100 / wetagecount;
            string strstatusupdate = "update Pms_tbl_Addsubtask set Weightage='" + totalwetage.ToString() + "' where task='" + taskid + "' and status=1 and archive=0";
            DataSet dshourmin = da.GetDataSetInline(strstatusupdate);

        }

        if (checkweitage == 0)
        {
            string strstatusupdate = "update Pms_tbl_Addsubtask set Weightage='0' where task='" + taskid + "' and status=1 and archive=0";
            DataSet dshourmin = da.GetDataSetInline(strstatusupdate);
        }



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

    public static string get_ProjectName(string Projectid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_AddProject where status=1 and archive=0 and id='" + Projectid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["ProjectName"].ToString();

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

    public static string get_task(string taskid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_Addtask where status=1 and archive=0 and id='" + taskid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["Milestone"].ToString();

        }

        return strret;

    }


    public static string get_Tasklistname(string tasklistid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_Addtasklist where status=1 and archive=0 and id='" + tasklistid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["TaskList"].ToString();

        }

        return strret;

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

    public static string get_Task(string taskid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_Addtask where status=1 and archive=0 and id='" + taskid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["taskname"].ToString();

        }

        return strret;

    }

    protected static string get_status_listbind()
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = "select * from Pms_tbl_Projectstatus where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret += "<ul class='sttus-list' data-simplebar style='max-height: 160px'>";
            for (int i = 0; i < dscode.Tables[0].Rows.Count; i++)
            {

                strret += "<li>" +
                  "<div class='dropdown-item p-0'>" +
                    "<a class='' href='javascript:void(0);' onclick = 'UpdateDataStatusdeatils(" + dscode.Tables[0].Rows[i]["id"].ToString() + ")' > " +
                    "<i class='bi bi-square-fill' style='color:" + dscode.Tables[0].Rows[i]["PrintColor"].ToString() + "' ></i> " + dscode.Tables[0].Rows[i]["ProjectStatus"].ToString() + "" +
                    "</a>" +
                    "</div>" +
                    "</li>";


            }
            strret += "</ul>";
        }

        return strret;

    }

    protected static string get_Stage_listbind()
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = "select * from Pms_tbl_Stage where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret += "<ul class='sttus-list' data-simplebar style='max-height: 160px'>";
            for (int i = 0; i < dscode.Tables[0].Rows.Count; i++)
            {

                strret += "<li>" +
                  "<div class='dropdown-item p-0'>" +
                    "<a class='' href='javascript:void(0);' onclick = 'UpdateDataStagedeatils(" + dscode.Tables[0].Rows[i]["id"].ToString() + ")' > " +
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
        string strsql = "select * from Pms_tbl_Priority where  status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
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

    protected static string get_depdencytaskname(string str1)
    {
        DataAccess da = new DataAccess();
        string str = str1;
        string[] splitstr = str.Split(',');
        string stname = "";
        for (int i = 0; i < splitstr.Length; i++)
        {
            string strsql = "select * from Pms_tbl_Addsubtask where id = '" + splitstr[i] + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0";
            DataSet ds2 = da.GetDataSetInline(strsql);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                if (ds2.Tables[0].Rows[0]["Subtaskname"].ToString() != string.Empty)
                {
                    if (stname != string.Empty)
                    {
                        stname += "," + ds2.Tables[0].Rows[0]["Subtaskname"].ToString();
                    }
                    else
                    {
                        stname += ds2.Tables[0].Rows[0]["Subtaskname"].ToString();
                    }
                }

            }
        }
        return stname;
    }


    protected static string get_Projectstatus(string projectid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_Projectstatus where status=1 and archive=0 and id='" + projectid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString();

        }

        return strret;

    }

    protected static string get_Stagename(string stageid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_Stage where status=1 and archive=0 and id='" + stageid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["name"].ToString();

        }

        return strret;

    }

    protected static string get_Status_Color(string projectid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_Projectstatus where status=1 and archive=0 and id='" + projectid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["PrintColor"].ToString();

        }

        return strret;

    }

    protected static string get_Stage_Color(string stageid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_Stage where status=1 and archive=0 and id='" + stageid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
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
        string strsql = " select * from Pms_tbl_Priority where  status=1 and archive=0 and Id='" + projectid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
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
        string strsql = " select * from Pms_tbl_Priority where status=1 and archive=0 and Id='" + projectid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["PrintColor"].ToString();

        }

        return strret;

    }

}
public class SubSubtaskdatamulti
{
    public string id { get; set; }
    public string ProjectId { get; set; }
    public string Milestoneid { get; set; }
    public string tasklist { get; set; }
    public string task { get; set; }
    public string Subtaskname { get; set; }
    public string SubTaskDescription { get; set; }
    public string owner { get; set; }
    public string Startdate { get; set; }
    public string Targetdate { get; set; }
    public string StartTime { get; set; }
    public string Timeline { get; set; }
    public string TaskDuration { get; set; }
    public string P_Status { get; set; }
    public string Priority { get; set; }
    public string Budget { get; set; }
    public string Actual { get; set; }
    public string Reminder { get; set; }
    public string Occurrence { get; set; }
    public string ReminderType { get; set; }
    public string Days { get; set; }
    public string Time { get; set; }
    public string Notifyusers { get; set; }
    public string ChooseDate { get; set; }
    public string tagto { get; set; }
    public string team { get; set; }
    public string Stage { get; set; }
    public string Weightage { get; set; }


}









