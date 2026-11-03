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

public partial class Addtasklist : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataAccess da = new DataAccess();
    public CommonFunction cmf = new CommonFunction();
    SqlParameter[] prm, prm1;
    public function fns = new function();
    public string Tasklistdata = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {

           


        }
    }
    
    

    //// web method bind using ajax start //

    [WebMethod]
    public static void SaveTasklist(Tasklistdata Taskdetails)
    {
        DataAccess da1 = new DataAccess();
        string editid = Taskdetails.id;
        string entrytype = "Insert";
        if (editid != "" && editid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[17];
        prm[0] = new SqlParameter("@id", editid);
        prm[1] = new SqlParameter("@ProjectId", Taskdetails.ProjectId);
        prm[2] = new SqlParameter("@Milestone", Taskdetails.Milestone);
        prm[3] = new SqlParameter("@TaskList", Taskdetails.TaskList);
        prm[4] = new SqlParameter("@TagTo", Taskdetails.TagTo);
        prm[5] = new SqlParameter("@Access", Taskdetails.Access);
        prm[6] = new SqlParameter("@team", Taskdetails.team);
        prm[7] = new SqlParameter("@P_Status", Taskdetails.P_Status);
        prm[8] = new SqlParameter("@Priority", Taskdetails.Priority);
        if (Taskdetails.Budget == string.Empty)
        {
            prm[9] = new SqlParameter("@Budget", DBNull.Value);
        }
        else
        {
            prm[9] = new SqlParameter("@Budget", Taskdetails.Budget);
        }

        if (Taskdetails.Actual == string.Empty)
        {
            prm[10] = new SqlParameter("@Actual", DBNull.Value);
        }
        else
        {
            prm[10] = new SqlParameter("@Actual", Taskdetails.Actual);
        }
        if (Taskdetails.StartDate == string.Empty)
        {
            prm[11] = new SqlParameter("@StartDate", DBNull.Value);
        }
        else
        {
            prm[11] = new SqlParameter("@StartDate", Taskdetails.StartDate);
        }
        if (Taskdetails.TargateDate == string.Empty)
        {
            prm[12] = new SqlParameter("@TargateDate", DBNull.Value);
        }
        else
        {
            prm[12] = new SqlParameter("@TargateDate", Taskdetails.TargateDate);
        }

        prm[13] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[14] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[15] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[16] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pms_Addtasklist_usp", prm);
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
        prm[3] = new SqlParameter("@PrintColor", statuscolor);
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
        prm[3] = new SqlParameter("@PrintColor", prioritycolor);
        prm[4] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[4].Direction = ParameterDirection.ReturnValue;
        int id = da1.executeDMLQuery("Pms_SP_Priority_insert_reg", prm);
        retval = id.ToString();

        return retval;

    }

    [WebMethod]
    public static string Save_Team(Tasklistdata Taskdetails)
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string editid = Taskdetails.id;
        string entrytype = "Update";
        if (editid != "" && editid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", editid);
        prm[1] = new SqlParameter("@team", Taskdetails.team);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pms_tbl_tasklist_TeamDeatilsUpdate", prm);

        DataAccess da = new DataAccess();
        string strsql = "select * from Pms_tbl_Addtasklist where  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and  id='" + editid + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            
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
    public static string Update_Status(string tasklistid = "", string statusid = "")
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (tasklistid != "" && tasklistid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", tasklistid);
        prm[1] = new SqlParameter("@ProjectStatus", statusid);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pmstbl_tasklistStatus_DeatilsUpdate", prm);

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
    public static string UpdatePriority_Status(string tasklistid = "", string Propertyid = "")
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (tasklistid != "" && tasklistid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", tasklistid);
        prm[1] = new SqlParameter("@Priority", Propertyid);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pms_tbl_tasklistPriority_DeatilsUpdate", prm);

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
    public static string bindTasklistdata()
    {
        string retval = get_status_listbind();
        string strvalprority = get_Priority_listbind();
        string Tasklistdata = "";
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        DataSet ds = da.GetDataSet("Pms_BindTasklist_dashbord", prm);
        Tasklistdata += "<table cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm datatable'>";
        Tasklistdata += "<thead>";
        Tasklistdata += "<tr>";

        //Tasklistdata += "<th>Select</th>";
        Tasklistdata += "<th>S.No</th>";
        Tasklistdata += "<th style=min-width:240px;>Project</th>";
        Tasklistdata += "<th style=min-width:120px;>Milestone</th>";
        Tasklistdata += "<th style=min-width:120px;>Task List</th>";
        Tasklistdata += "<th style=min-width:120px;>Team</th>";
        Tasklistdata += "<th style=width:110px;>Status</th>";
        Tasklistdata += "<th>Priority</th>";
        Tasklistdata += "<th>Budget</th>";
        Tasklistdata += "<th>Actual</th>";
        Tasklistdata += "<th>Start Date</th>";
        Tasklistdata += "<th>Target Date</th>";
        Tasklistdata += "<th style=min-width:140px;>Timeline</th>";
        //Tasklistdata += "<th style=min-width:140px;>Completion Status</th>";
        Tasklistdata += "<th style=min-width:140px;>Issues</th>";
        Tasklistdata += "<th>File</th>";
        Tasklistdata += "<th>Action</th>";
        
        Tasklistdata += "</tr>";
        Tasklistdata += "</thead>";
        Tasklistdata += "<tbody>";
        if (ds.Tables[0].Rows.Count > 0)
        {


            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {

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


                Tasklistdata += "<tr>";
                Tasklistdata += "<td valign='top'>";
                Tasklistdata += (i + 1);
                Tasklistdata += "</td>";

                Tasklistdata += "<td valign='top'>";
                Tasklistdata += get_ProjectName(ds.Tables[0].Rows[i]["ProjectId"].ToString()); // project
                Tasklistdata += "</td>";

                Tasklistdata += "<td valign='top'>";
                Tasklistdata += get_Milestonename(ds.Tables[0].Rows[i]["Milestone"].ToString()); // milstone
                Tasklistdata += "</td>";

                Tasklistdata += "<td valign='top'>";
                Tasklistdata += ds.Tables[0].Rows[i]["TaskList"].ToString();
                Tasklistdata += "</td>";

                //team
                Tasklistdata += "<td>";
                Tasklistdata += "<div class='d-flex'>";
                Tasklistdata += "<div class='flex-grow-1'>";
                Tasklistdata += "<div class='avatar-group' id='lbluser_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>";
                Tasklistdata += usersdetails;

                //projectdata += "</a>";
                Tasklistdata += "<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='" + teamtooltip + "' data-bs-original-title='more'>";

                if (cnt != "" && cnt != "0")
                {
                    Tasklistdata += "<div class='avatar-xxs'>";
                    Tasklistdata += "<div class='avatar-title rounded-circle'>";

                    Tasklistdata += (cnt + "+");
                    Tasklistdata += "</div>";
                    Tasklistdata += "</div>";
                }

                Tasklistdata += "</a>";
                Tasklistdata += "</div>";
                Tasklistdata += "</div>";
                Tasklistdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                Tasklistdata += "<a href ='javascript:void(0);' type='button' id='btnteam_" + ds.Tables[0].Rows[i]["id"].ToString() + "' onclick='Binddetails_Team(" + ds.Tables[0].Rows[i]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#addteam'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Team'>+</span></a>";
                Tasklistdata += "</div>";
                Tasklistdata += "</div>";
                Tasklistdata += "</td>";


                // status

                string strvalcolor = get_Status_Color(ds.Tables[0].Rows[i]["P_Status"].ToString());
                Tasklistdata += "<td>";
                Tasklistdata += "<div class='btn-group d-block'>";
                Tasklistdata += "<a href ='javascript:void(0);' id='btnsts_" + ds.Tables[0].Rows[i]["id"].ToString() + "' type='button' onclick=setprojectid('" + ds.Tables[0].Rows[i]["id"].ToString() + "',this.id) data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + strvalcolor + "'>" + get_Projectstatus(ds.Tables[0].Rows[i]["P_Status"].ToString()) + "</a>";
                Tasklistdata += "<div class='dropdown-menu lead_form_body p-0'>";
                Tasklistdata += retval;
                Tasklistdata += "</div>";
                Tasklistdata += "</div>";
                Tasklistdata += "</td>";


                //priority
                string strprioritycolor = get_PriorityColor(ds.Tables[0].Rows[i]["Priority"].ToString());
                Tasklistdata += "<td>";
                Tasklistdata += "<div class='btn-group d-block'>";
                Tasklistdata += "<a href ='javascript:void(0);' id='btnpri_" + ds.Tables[0].Rows[i]["id"].ToString() + "' type='button' onclick=setpriorityid('" + ds.Tables[0].Rows[i]["id"].ToString() + "',this.id) data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + strprioritycolor + "'>" + get_PriorityName(ds.Tables[0].Rows[i]["Priority"].ToString()) + "</a>";
                Tasklistdata += "<div class='dropdown-menu lead_form_body p-0'>";
                Tasklistdata += strvalprority;
                Tasklistdata += "</div>";
                Tasklistdata += "</div>";
                Tasklistdata += "</td>";


                Tasklistdata += "<td valign='top'>";
                Tasklistdata += ds.Tables[0].Rows[i]["Budget"].ToString(); // budget 
                Tasklistdata += "</td>";

                Tasklistdata += "<td valign='top'>";
                Tasklistdata += ds.Tables[0].Rows[i]["Actual"].ToString(); // Actual 
                Tasklistdata += "</td>";

                Tasklistdata += "<td valign='top'>";
                Tasklistdata += cmf.dateConvert(ds.Tables[0].Rows[i]["StartDate"].ToString()); //start date
                Tasklistdata += "</td>";

                Tasklistdata += "<td valign='top'>";
                Tasklistdata += cmf.dateConvert(ds.Tables[0].Rows[i]["TargateDate"].ToString()); //target date
                Tasklistdata += "</td>";

                //Tasklistdata += "<td valign='top'>";
                //Tasklistdata += ""; //Timeline
                //Tasklistdata += "</td>";


                //Timeline

                DateTime StartDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"].ToString() == "" ? "0" : ds.Tables[0].Rows[i]["StartDate"].ToString());
                DateTime Targetdate = Convert.ToDateTime(ds.Tables[0].Rows[i]["TargateDate"].ToString() == "" ? "0" : ds.Tables[0].Rows[i]["TargateDate"].ToString());
                TimeSpan objTimeSpan = Targetdate - StartDate;
                double Days = Convert.ToDouble(objTimeSpan.TotalDays);
                Days = Days + 1;


                DateTime Enddate = Convert.ToDateTime(ds.Tables[0].Rows[i]["TargateDate"].ToString() == "" ? "0" : ds.Tables[0].Rows[i]["TargateDate"].ToString());
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



                Tasklistdata += "<td>";
                Tasklistdata += "<a href = 'javascript:void(0);' class='timesheet-wrap'>";
                Tasklistdata += "<div class='progress progress-xl bg-dark position-relative' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + leftDays.ToString().Replace("-", "") + " " + leftdlydys + "'>";
                Tasklistdata += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 70%;' aria-valuemax='100'></div>";
                Tasklistdata += "<div class='dayswrapper'>";
                Tasklistdata += "<span class='fromtodate'>" + ststartdys + " " + startmonthname + " - " + enddys + " " + endmonthname + "</span>";
                Tasklistdata += "<span class='totaldays'>" + Days + " days</span>";
                Tasklistdata += "</div>";
                Tasklistdata += "</div>";
                Tasklistdata += "</a>";
                Tasklistdata += "</td>";



                //Tasklistdata += "<td valign='top'>";
                //Tasklistdata += ""; // Completion Status
                //Tasklistdata += "</td>";

                //Tasklistdata += "<td valign='top'>";
                //Tasklistdata += ""; // isssues 
                //Tasklistdata += "</td>";

                Tasklistdata += "<td valign='top'>";
                Tasklistdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                Tasklistdata += "<a href ='javascript:void(0);' type='button' data-bs-toggle='offcanvas' data-bs-target=#canvas_popup onclick=loadsrc('inner_popup','/AddIssuesPopup.aspx?id=" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "&milstoneid=" + ds.Tables[0].Rows[i]["Milestone"].ToString() + "&tasklistid=" + ds.Tables[0].Rows[i]["id"].ToString() + "&type=tasklist')><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Issue'>+</span></a>";
                Tasklistdata += "</div>";  // isssues 
                Tasklistdata += "</td>";


                Tasklistdata += "<td valign='top'>";
                Tasklistdata += "";  // file 
                Tasklistdata += "</td>";

                Tasklistdata += " <td><a href='javascript:void(0);' Class='btn btn-info btn-sm' type='button' onclick='Binddetails(" + ds.Tables[0].Rows[i]["id"].ToString() + ")' data-bs-toggle='offcanvas' data-bs-target='#addtasklist'><i class='ri-edit-line'></i></a>";
                Tasklistdata += "&nbsp;&nbsp;<a href='javascript:void(0);' Class='btn btn-danger btn-sm' type='button' onclick='deletedata(" + ds.Tables[0].Rows[i]["id"].ToString() + ")'><i class='ri-delete-bin-5-line'></i></a>";
                Tasklistdata += "</td>";


                Tasklistdata += "</tr>";
            }
        }

        Tasklistdata += "</tbody>";
        Tasklistdata += "</table>";

        return Tasklistdata;
    }

    [WebMethod]
    public static string bindTasklistdata_statussearch(string RecordId = "0")
    {
        string Tasklistdata = "";
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        SqlParameter[] prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[1] = new SqlParameter("@RecordId", RecordId);
        DataSet ds = da.GetDataSet("Pms_BindTasklist_dashbord_searchdrop", prm);
        Tasklistdata += "<table cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm datatable'>";
        Tasklistdata += "<thead>";
        Tasklistdata += "<tr>";

        //Tasklistdata += "<th>Select</th>";
        Tasklistdata += "<th>S.No</th>";
        Tasklistdata += "<th style=min-width:300px;>Project</th>";
        Tasklistdata += "<th style=min-width:300px;>Milestone</th>";
        Tasklistdata += "<th style=min-width:300px;>Task List</th>";
        Tasklistdata += "<th style=min-width:220px;>Team</th>";
        Tasklistdata += "<th style=width:110px;>Status</th>";
        Tasklistdata += "<th>Priority</th>";
        Tasklistdata += "<th>Budget</th>";
        Tasklistdata += "<th>Actual</th>";
        Tasklistdata += "<th>Start Date</th>";
        Tasklistdata += "<th>Target Date</th>";
        Tasklistdata += "<th style=min-width:140px;>Timeline</th>";
        Tasklistdata += "<th style=min-width:140px;>Completion Status</th>";
        Tasklistdata += "<th style=min-width:140px;>Issues</th>";
        Tasklistdata += "<th>File</th>";
        Tasklistdata += "<th>Action</th>";

        Tasklistdata += "</tr>";
        Tasklistdata += "</thead>";
        Tasklistdata += "<tbody>";
        if (ds.Tables[0].Rows.Count > 0)
        {


            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Tasklistdata += "<tr>";

                Tasklistdata += "<td valign='top'>";
                Tasklistdata += (i + 1);
                Tasklistdata += "</td>";

                Tasklistdata += "<td valign='top'>";
                Tasklistdata += get_ProjectName(ds.Tables[0].Rows[i]["ProjectId"].ToString()); // project
                Tasklistdata += "</td>";

                Tasklistdata += "<td valign='top'>";
                Tasklistdata += get_ProjectName(ds.Tables[0].Rows[i]["Milestone"].ToString()); // milstone
                Tasklistdata += "</td>";

                Tasklistdata += "<td valign='top'>";
                Tasklistdata += ds.Tables[0].Rows[i]["TaskList"].ToString();
                Tasklistdata += "</td>";

                Tasklistdata += "<td valign='top'>";
                Tasklistdata += get_team(ds.Tables[0].Rows[i]["team"].ToString());//team
                Tasklistdata += "</td>";

                string stpstatus = "";
                if (ds.Tables[0].Rows[i]["P_Status"].ToString() == "1")
                {
                    stpstatus = "Status 1";

                }

                if (ds.Tables[0].Rows[i]["P_Status"].ToString() == "2")
                {
                    stpstatus = "Status 2";

                }

                if (ds.Tables[0].Rows[i]["P_Status"].ToString() == "3")
                {
                    stpstatus = "Status 3";

                }

                Tasklistdata += "<td valign='top'>";
                Tasklistdata += stpstatus;//status
                Tasklistdata += "</td>";


                string stpriority = "";
                if (ds.Tables[0].Rows[i]["Priority"].ToString() == "1")
                {
                    stpriority = "Low";

                }

                if (ds.Tables[0].Rows[i]["Priority"].ToString() == "2")
                {
                    stpriority = "High";

                }

                if (ds.Tables[0].Rows[i]["Priority"].ToString() == "3")
                {
                    stpriority = "Medium";

                }

                Tasklistdata += "<td valign='top'>";
                Tasklistdata += stpriority; //priority
                Tasklistdata += "</td>";

                Tasklistdata += "<td valign='top'>";
                Tasklistdata += ds.Tables[0].Rows[i]["Budget"].ToString(); // budget 
                Tasklistdata += "</td>";

                Tasklistdata += "<td valign='top'>";
                Tasklistdata += ds.Tables[0].Rows[i]["Actual"].ToString(); // Actual 
                Tasklistdata += "</td>";

                Tasklistdata += "<td valign='top'>";
                Tasklistdata += cmf.dateConvert(ds.Tables[0].Rows[i]["StartDate"].ToString()); //start date
                Tasklistdata += "</td>";

                Tasklistdata += "<td valign='top'>";
                Tasklistdata += cmf.dateConvert(ds.Tables[0].Rows[i]["TargateDate"].ToString()); //target date
                Tasklistdata += "</td>";

                Tasklistdata += "<td valign='top'>";
                Tasklistdata += ""; //Timeline
                Tasklistdata += "</td>";


                Tasklistdata += "<td valign='top'>";
                Tasklistdata += ""; // Completion Status
                Tasklistdata += "</td>";

                //Tasklistdata += "<td valign='top'>";
                //Tasklistdata += ""; // isssues 
                //Tasklistdata += "</td>";

                Tasklistdata += "<td valign='top'>";
                Tasklistdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                Tasklistdata += "<a href ='javascript:void(0);' type='button' data-bs-toggle='offcanvas' data-bs-target=#canvas_popup onclick=loadsrc('inner_popup','/AddIssuesPopup.aspx?id=" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "&milstoneid=" + ds.Tables[0].Rows[i]["Milestone"].ToString() + "&tasklistid=" + ds.Tables[0].Rows[i]["id"].ToString() + "&type=tasklist')><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Issue'>+</span></a>";
                Tasklistdata += "</div>";  // isssues 
                Tasklistdata += "</td>";


                Tasklistdata += "<td valign='top'>";
                Tasklistdata += "";  // file 
                Tasklistdata += "</td>";

                Tasklistdata += " <td><a href='javascript:void(0);' Class='btn btn-info btn-sm' type='button' onclick='Binddetails(" + ds.Tables[0].Rows[i]["id"].ToString() + ")' data-bs-toggle='offcanvas' data-bs-target='#addtasklist'><i class='ri-edit-line'></i></a>";
                Tasklistdata += "&nbsp;&nbsp;<a href='javascript:void(0);' Class='btn btn-danger btn-sm' type='button' onclick='deletedata(" + ds.Tables[0].Rows[i]["id"].ToString() + ")'><i class='ri-delete-bin-5-line'></i></a>";
                Tasklistdata += "</td>";


                Tasklistdata += "</tr>";
            }
        }

        Tasklistdata += "</tbody>";
        Tasklistdata += "</table>";

        return Tasklistdata;
    }

    [WebMethod]
    public static string BinTasklistdetail(int RecordId = 0)
    {
        DataAccess da = new DataAccess();
        DataSet ds;
        string strsql = "select * from Pms_tbl_Addtasklist where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
        ds = da.GetDataSetInline(strsql);

        return ds.GetXml();
    }



    [WebMethod]
    public static string DeleteTasklistdata(int RecordId = 0)
    {
        string retval = "0";

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "update  Pms_tbl_Addtasklist set  status=0,archive=1  where  id='" + RecordId + "' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        ds = da.GetDataSetInline(strsql);

        return retval;
    }
    
    [WebMethod]
    public static string Bindetail_Team(int RecordId = 0)
    {

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "select * from Pms_tbl_Addtasklist where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
        ds = da.GetDataSetInline(strsql);


        return ds.GetXml();
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
    public static string get_Milestonename(string milestoneid)
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
    protected static string get_Status_Color(string projectid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_Projectstatus where  status=1 and archive=0 and id='" + projectid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
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
        string strsql = " select * from Pms_tbl_Priority where  status=1 and archive=0 and Id='" + projectid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["PrintColor"].ToString();

        }

        return strret;

    }
}
public class Tasklistdata
{
    public string id { get; set; }
    public string ProjectId { get; set; }
    public string Milestone { get; set; }
    public string TaskList { get; set; }
    public string TagTo { get; set; }
    public string Access { get; set; }
    public string team { get; set; }
    public string P_Status { get; set; }
    public string Priority { get; set; }
    public string Budget { get; set; }
    public string Actual { get; set; }
    public string StartDate { get; set; }
    public string TargateDate { get; set; }


}




