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

public partial class MultiProjectTask : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataAccess da = new DataAccess();
    public CommonFunction cmf = new CommonFunction();
    SqlParameter[] prm, prm1;
    public function fns = new function();
    public string taskdata = "";
    public string Multiteam = "";
   
    public string settask = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
           
        }
    }

    //// web method bind using ajax start //

    [WebMethod]
    public static string SaveTask(MultiTaskData taskdetails)
    {
        function fns = new function();
        string retval = "0";
        DataAccess da1 = new DataAccess();
        string editid = taskdetails.id;
        string entrytype = "Insert";
        if (editid != "" && editid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[34];
        prm[0] = new SqlParameter("@id", editid);
        prm[1] = new SqlParameter("@ProjectId", taskdetails.ProjectId);
        prm[2] = new SqlParameter("@Milestoneid", taskdetails.Milestoneid);
        prm[3] = new SqlParameter("@tasklist", taskdetails.tasklist);
        prm[4] = new SqlParameter("@taskname", taskdetails.taskname);
        prm[5] = new SqlParameter("@taskdescripton", taskdetails.taskdescripton);
        prm[6] = new SqlParameter("@owner", taskdetails.owner);
        if (taskdetails.Startdate == string.Empty)
        {
            prm[7] = new SqlParameter("@Startdate", DBNull.Value);
        }
        else
        {
            prm[7] = new SqlParameter("@Startdate", taskdetails.Startdate);
        }
        if (taskdetails.Targetdate == string.Empty)
        {
            prm[8] = new SqlParameter("@Targetdate", DBNull.Value);
        }
        else
        {
            prm[8] = new SqlParameter("@Targetdate", taskdetails.Targetdate);
        }


        if (taskdetails.StartTime == string.Empty)
        {
            prm[9] = new SqlParameter("@StartTime", DBNull.Value);
        }
        else
        {
            prm[9] = new SqlParameter("@StartTime", taskdetails.StartTime);
        }


        if (taskdetails.Timeline == string.Empty)
        {
            prm[10] = new SqlParameter("@Timeline", DBNull.Value);
        }
        else
        {
            prm[10] = new SqlParameter("@Timeline", taskdetails.Timeline);
        }

        prm[11] = new SqlParameter("@TaskDuration", taskdetails.TaskDuration);


        prm[12] = new SqlParameter("@P_Status", taskdetails.P_Status);
        prm[13] = new SqlParameter("@Priority", taskdetails.Priority);


        if (taskdetails.Budget == string.Empty)
        {
            prm[14] = new SqlParameter("@Budget", DBNull.Value);
        }
        else
        {
            prm[14] = new SqlParameter("@Budget", taskdetails.Budget);
        }

        if (taskdetails.Actual == string.Empty)
        {
            prm[15] = new SqlParameter("@Actual", DBNull.Value);
        }
        else
        {
            prm[15] = new SqlParameter("@Actual", taskdetails.Actual);
        }


        prm[16] = new SqlParameter("@Reminder", taskdetails.Reminder);

        if (taskdetails.ReminderType == string.Empty)
        {
            prm[17] = new SqlParameter("@ReminderType", DBNull.Value);
        }
        else
        {
            prm[17] = new SqlParameter("@ReminderType", taskdetails.ReminderType);
        }

        if (taskdetails.Days == string.Empty)
        {
            prm[18] = new SqlParameter("@Days", DBNull.Value);
        }
        else
        {
            prm[18] = new SqlParameter("@Days", taskdetails.Days);

        }
        if (taskdetails.Time == string.Empty)
        {
            prm[19] = new SqlParameter("@Time", DBNull.Value);
        }
        else
        {
            prm[19] = new SqlParameter("@Time", taskdetails.Time);
        }
        if (taskdetails.Notifyusers == string.Empty)
        {
            prm[20] = new SqlParameter("@Notifyusers", DBNull.Value);
        }
        else
        {
            prm[20] = new SqlParameter("@Notifyusers", taskdetails.Notifyusers);
        }

        if (taskdetails.ChooseDate == string.Empty)
        {
            prm[21] = new SqlParameter("@ChooseDate", DBNull.Value);
        }
        else
        {
            prm[21] = new SqlParameter("@ChooseDate", taskdetails.ChooseDate);
        }

        prm[22] = new SqlParameter("@Occurrence", taskdetails.Occurrence);
        prm[23] = new SqlParameter("@tagto", taskdetails.tagto);
        prm[24] = new SqlParameter("@team", taskdetails.team);
        prm[25] = new SqlParameter("@Stage", taskdetails.Stage);
        prm[26] = new SqlParameter("@Weightage", taskdetails.Weightage);
        prm[27] = new SqlParameter("@issubtask", taskdetails.issubtask);
        prm[28] = new SqlParameter("@checkTaskWeightage", taskdetails.checkTaskWeightage);
        prm[29] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[30] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[31] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[32] = new SqlParameter("@type", entrytype);
        prm[33] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[33].Direction = ParameterDirection.ReturnValue;
        int retval1 = da1.executeDMLQuery("pms_Adtask_usp", prm);
        retval = retval1.ToString();



        // iscomplete update //

        string striscomplt = "select  isnull(iscomplete,0) as iscomplete from Pms_tbl_Projectstatus where status = 1 and archive = 0  and id='" + taskdetails.P_Status + "'";
        DataSet dsiscomplt = da1.GetDataSetInline(striscomplt);
        if (dsiscomplt.Tables[0].Rows.Count > 0)
        {
            int iscompletestatus = 0;
            iscompletestatus = Convert.ToInt32(dsiscomplt.Tables[0].Rows[0]["iscomplete"]);

            string strstatusupdateiscom = "update Pms_tbl_Addtask set iscomplete='" + iscompletestatus.ToString() + "' where id = " + retval + " and status=1 and archive=0";
            DataSet dshourminiscom = da1.GetDataSetInline(strstatusupdateiscom);

        }

        //   iscomplete update  



        int checkweitage = 0;
        decimal wetagecount = 0;
        decimal totalwetage = 0;
        string strweightage = "select isnull(checkmileWeightage ,0) as checkmileWeightage  from Pms_tbl_AddMilestone where id='" + taskdetails.Milestoneid + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
        DataSet dsweitagecheck = da1.GetDataSetInline(strweightage);
        if (dsweitagecheck.Tables[0].Rows.Count > 0)
        {

            checkweitage = Convert.ToInt32(dsweitagecheck.Tables[0].Rows[0]["checkmileWeightage"]);

        }

        if (checkweitage == 1)
        {
            string strweightagecount = "select count (*) as WeitageCount from Pms_tbl_Addtask where Milestoneid='" + taskdetails.Milestoneid + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
            DataSet dsweitagecount = da1.GetDataSetInline(strweightagecount);
            if (dsweitagecount.Tables[0].Rows.Count > 0)
            {

                wetagecount = Convert.ToInt32(dsweitagecount.Tables[0].Rows[0]["WeitageCount"]);

            }

            totalwetage = 100 / wetagecount;
            string strstatusupdate = "update Pms_tbl_Addtask set Weightage='" + fns.ToFormattedCurrencyString(totalwetage.ToString()) + "' where Milestoneid='" + taskdetails.Milestoneid + "' and status=1 and archive=0";
            DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

        }


        int iscomplete = 0;
        string finsql1 = "select isnull(iscomplete,0) as iscomplete, * from Pms_tbl_Addtask where id=" + retval + " and status=1 and archive=0";
        DataSet dsfindid1 = da1.GetDataSetInline(finsql1);
        if (dsfindid1.Tables[0].Rows.Count > 0)
        {

            iscomplete = Convert.ToInt32(dsfindid1.Tables[0].Rows[0]["iscomplete"]);

        }

        if (iscomplete == 1)
        {
            decimal tsktotalpercent = 100;
            decimal totlweitagemile = 0;
            decimal totlweitageproject = 0;

            string finsql01 = "select * from Pms_tbl_Addtask where id=" + retval + " and status=1  and archive=0";
            DataSet dsfindid01 = da1.GetDataSetInline(finsql01);

            // task percentage //
            string strstatusupdate = "update Pms_tbl_Addtask set iscompletepercentage='" + tsktotalpercent.ToString() + "' where id='" + retval + "' and status=1 and archive=0 and isnull(iscomplete,0)=1";
            DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

            // Milestone percentage //
            string finsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "'   and status=1  and archive=0";
            DataSet dsfindmile = da1.GetDataSetInline(finsqlmile);
            for (int j = 0; j < dsfindmile.Tables[0].Rows.Count; j++)
            {
                decimal sumweitage = 0;
                decimal subpercentageemile = 0;
                sumweitage = Convert.ToDecimal(dsfindmile.Tables[0].Rows[j]["Weightage"].ToString());
                subpercentageemile = Convert.ToDecimal(dsfindmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                totlweitagemile += (sumweitage * subpercentageemile) / 100;

            }

            string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile.ToString() + "' where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and id='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da1.GetDataSetInline(finsql4);


            // project percentage //
            string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "'   and status=1  and archive=0";
            DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
            for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
            {
                decimal sumweitageprojt = 0;
                decimal subpercentageeproject = 0;
                sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

            }

            string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
            DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);




        }
        else
        {
            decimal tsktotalpercent = 0;
            decimal totlweitagemile = 0;
            decimal totlweitageproject = 0;

            string finsql01 = "select * from Pms_tbl_Addtask where id=" + retval + " and status=1  and archive=0";
            DataSet dsfindid01 = da1.GetDataSetInline(finsql01);


            if (dsfindid01.Tables[0].Rows.Count > 0)
            {
                if (dsfindid01.Tables[0].Rows[0]["issubtask"].ToString() == "1")
                {

                    string finsql211 = "select isnull(sum(weightage),0)Wattagetasksum from Pms_tbl_Addsubtask where   task='" + retval + "'  and status=1 and isnull(iscomplete,0)=1 and archive=0";
                    DataSet dsfindid211 = da1.GetDataSetInline(finsql211);
                    if (dsfindid211.Tables[0].Rows.Count > 0)
                    {
                        string strstatusupdate = "update Pms_tbl_Addtask set iscompletepercentage='" + dsfindid211.Tables[0].Rows[0]["Wattagetasksum"] + "' where id='" + retval + "' and status=1 and archive=0";
                        DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);
                    }
                }
                else
                {
                    string strstatusupdate = "update Pms_tbl_Addtask set iscompletepercentage='0' where id='" + retval + "' and status=1 and archive=0";
                    DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                }

            }

            // task percentage //
            //string strstatusupdate = "update Pms_tbl_Addtask set iscompletepercentage='" + tsktotalpercent.ToString() + "' where id='" + retval + "' and status=1 and archive=0 and isnull(iscomplete,0)=0";
            //DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

            // Milestone percentage //
            string finsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "'   and status=1  and archive=0";
            DataSet dsfindmile = da1.GetDataSetInline(finsqlmile);
            for (int j = 0; j < dsfindmile.Tables[0].Rows.Count; j++)
            {
                decimal sumweitage = 0;
                decimal subpercentageemile = 0;
                sumweitage = Convert.ToDecimal(dsfindmile.Tables[0].Rows[j]["Weightage"].ToString());
                subpercentageemile = Convert.ToDecimal(dsfindmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                totlweitagemile += (sumweitage * subpercentageemile) / 100;

            }

            string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile.ToString() + "' where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and id='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da1.GetDataSetInline(finsql4);

            // project percentage //
            string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "'   and status=1  and archive=0";
            DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
            for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
            {
                decimal sumweitageprojt = 0;
                decimal subpercentageeproject = 0;
                sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

            }

            string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
            DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);



        }




        return retval;

    }

    [WebMethod]
    public static string Savetask_Owner(string id = "", string ownerdropdownid = "")
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
        prm[1] = new SqlParameter("@Owner", ownerdropdownid);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pmstbl_Task_OwnerDeatilsUpdate", prm);


        DataAccess da = new DataAccess();
        string strsql = "select * from Pms_tbl_Addtask where  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and  id='" + editid + "' ";
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
    public static string SaveMilestone_Owner(string mileid = "", string mileownerdropdownid = "")
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string editid = mileid;
        string entrytype = "Update";
        if (editid != "" && editid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", editid);
        prm[1] = new SqlParameter("@Owner", mileownerdropdownid);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pms_tbl_OwnerDeatils_Milestone_Update", prm);

        DataAccess da = new DataAccess();
        string strsql = "select * from Pms_tbl_AddMilestone where  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and  id='" + editid + "' ";
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
                retval += "<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='" + ownertooltip + "' data -bs-original-title='more'>";

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
    public static string BindpMilestonedetail_Owner(int RecordId = 0)
    {

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "select * from Pms_tbl_AddMilestone where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
        ds = da.GetDataSetInline(strsql);


        return ds.GetXml();
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
    public static string Save_Team(MultiTaskData taskdetails)
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string editid = taskdetails.id;
        string entrytype = "Update";
        if (editid != "" && editid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", editid);
        prm[1] = new SqlParameter("@team", taskdetails.team);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pms_tbl_task_TeamDeatilsUpdate", prm);

        DataAccess da = new DataAccess();
        string strsql = "select * from Pms_tbl_Addtask where  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and  id='" + editid + "' ";
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
                retval += "<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='" + teamtooltip + "'  data-bs-original-title='more'>";

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

        string strsql = "select * from Pms_tbl_Addtask where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
        ds = da.GetDataSetInline(strsql);


        return ds.GetXml();
    }

    [WebMethod]
    public static string Bindcheckstrtdatemiledetails(int milesto_nid = 0)
    {

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "select * from Pms_tbl_AddMilestone where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + milesto_nid + "'";
        ds = da.GetDataSetInline(strsql);


        return ds.GetXml();
    }


     public static string checkdependency(string strtype, string strid)
    {
        DataAccess da1 = new DataAccess();
        string ret = "0";
        string strsqlM = "select id, isnull(Isdone,0) Isdone from tbl_dependencyST where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and milestoneid>0 and  milestoneid='" + strid + "' and status=1 and archive=0 and isnull(Isdone,0)=0";
        if (strtype == "task")
        {
            strsqlM = "select id, isnull(Isdone,0)  from tbl_dependencyST where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and taskid>0  and taskid='" + strid + "'  and status=1 and archive=0  and isnull(Isdone,0)=0";
        }
        else if (strtype == "subtask")
        {
            strsqlM = "select id,  isnull(Isdone,0)  from tbl_dependencyST where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and subtaskid>0  and subtaskid='" + strid + "'  and status=1 and archive=0  and isnull(Isdone,0)=0";
        }
       
        DataSet dschkD = da1.GetDataSetInline(strsqlM);
        if (dschkD.Tables[0].Rows.Count > 0)
        {
            string chkD = checkdependencynotcomplete(dschkD.Tables[0].Rows[0]["id"].ToString());
            ret = chkD;

        }

        return ret;

    }
    public static string checkdependencynotcomplete(string ids)
    {
        DataAccess da = new DataAccess();
        string strdependency = "";
        string retval = "0";
        try
        {

            string str_selecteddeatil = "";
            if (ids != "")
            {

                SqlParameter[] prm = new SqlParameter[2];
                prm[0] = new SqlParameter("@id", ids);
                prm[1] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                DataSet dsD = da.GetDataSet("sp_dependencydetails", prm);
                if (dsD.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsD.Tables[0].Rows.Count; i++)
                    {
                        if (dsD.Tables[0].Rows[i]["stid"].ToString() != "" && dsD.Tables[0].Rows[i]["stid"].ToString() != "0")
                        {
                            try
                            {
                                string stsqlM = "select isnull(statuscomplete,0) statuscomplete from Pms_tbl_AddSubtask where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + dsD.Tables[0].Rows[i]["stid"].ToString() + "' and isnull(statuscomplete,0)=0";
                                SaveLogText("checkcomplete123-"+ stsqlM);
                                DataSet dschkM = da.GetDataSetInline(stsqlM);
                                //if (dschkM.Tables[0].Rows[0]["statuscomplete"].ToString() != "1")
                                if (dschkM.Tables[0].Rows.Count > 0)
                                {
                                    retval = "1";
                                }

                            }
                            catch (Exception ex)
                            {
                                //SaveLogText("strbgc_s2-" + ex.Message);
                            }
                        }
                       else if (dsD.Tables[0].Rows[i]["tid"].ToString() != "" && dsD.Tables[0].Rows[i]["tid"].ToString() != "0")
                        {
                            try
                            {
                                string stsqlM = "select isnull(statuscomplete,0) statuscomplete from Pms_tbl_AddTask where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + dsD.Tables[0].Rows[i]["tid"].ToString() + "' and isnull(statuscomplete,0)=0";
                                

                                DataSet dschkM = da.GetDataSetInline(stsqlM);
                                //if (dschkM.Tables[0].Rows[0]["statuscomplete"].ToString() != "1")
                                if (dschkM.Tables[0].Rows.Count > 0)
                                {
                                    retval = "1";
                                }
                            }
                            catch { }
                        }

                       else if (dsD.Tables[0].Rows[i]["mid"].ToString() != "" && dsD.Tables[0].Rows[i]["mid"].ToString() != "0")
                        {
                            try
                            {
                                string stsqlM = "select isnull(statuscomplete,0) statuscomplete from Pms_tbl_AddMilestone where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + dsD.Tables[0].Rows[i]["mid"].ToString() + "' and isnull(statuscomplete,0)=0";
                                DataSet dschkM = da.GetDataSetInline(stsqlM);
                                //if (dschkM.Tables[0].Rows[0]["statuscomplete"].ToString() != "1")
                                if (dschkM.Tables[0].Rows.Count > 0)
                                {
                                    retval = "1";
                                }
                            }
                            catch { }
                        }
                        
                    }
                }

            }



        }
        catch (Exception ex)
        {

        }
        return retval;
    }
    public static string checkdependencyUpdate(string strtype, string strid)
    {
        DataAccess da1 = new DataAccess();
        string ret = "0";
        string strsqlM = "select isnull(Isdone,0) Isdone from tbl_dependencyST where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and milestoneid>0 and  milestoneid='" + strid + "' and status=1 and archive=0 and isnull(Isdone,0)=0";
        if (strtype == "task")
        {
            strsqlM = "select  isnull(Isdone,0)  from tbl_dependencyST where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and taskid>0 and taskid='" + strid + "'  and status=1 and archive=0  and isnull(Isdone,0)=0";
        }
        else if (strtype == "subtask")
        {
            strsqlM = "select  isnull(Isdone,0)  from tbl_dependencyST where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and subtaskid>0 and subtaskid='" + strid + "'  and status=1 and archive=0  and isnull(Isdone,0)=0";
        }
        //SaveLogText("strp123=" + strsqlM);

        DataSet dschkD = da1.GetDataSetInline(strsqlM);
        if (dschkD.Tables[0].Rows.Count > 0)
        {
            string strupdt = "update  tbl_dependencyST set Isdone=1    where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and milestoneid>0 and  milestoneid='" + strid + "'  and status=1 and archive=0  and isnull(Isdone,0)=0";
            if (strtype == "task")
            {
                strupdt = "update  tbl_dependencyST set Isdone=1    where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and taskid>0 and taskid='" + strid + "'  and status=1 and archive=0  and isnull(Isdone,0)=0";
            }
            if (strtype == "subtask")
            {
                strupdt = "update  tbl_dependencyST set Isdone=1    where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and subtaskid>0 and subtaskid='" + strid + "'  and status=1 and archive=0  and isnull(Isdone,0)=0";
            }
            ret = "1";

        }

        return ret;

    }
    [WebMethod]
    public static string UpdateMilestone_Status(string milstoneid = "", string statusid = "", string iscomplete = "")
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (milstoneid != "" && milstoneid != "0")
        {
            entrytype = "Update";
        }





        // percentage update start  // 
        if (iscomplete == "1")
        {
            string independency = checkdependency("milestone", milstoneid);
            if (independency == "1")
            {
                retval = "Please complete first dependency of this? ";
            }
            else
            {
                SqlParameter[] prm = new SqlParameter[8];
                prm[0] = new SqlParameter("@id", milstoneid);
                prm[1] = new SqlParameter("@milstoneStatus", statusid);
                prm[2] = new SqlParameter("@iscomplete", iscomplete);
                prm[3] = new SqlParameter("@iscompletedate", da1.getcdatetime());
                prm[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prm[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                prm[6] = new SqlParameter("@cdate", da1.getcdatetime());
                prm[7] = new SqlParameter("@type", entrytype);
                da1.executeDMLQuery("pmstbl_Status_milstone_Update_usp", prm);
                decimal milestonetotalpercent = 100;
                decimal totlweitagemile = 0;
                string finsql1 = "select * from Pms_tbl_AddMilestone where id=" + milstoneid + " and status=1  and archive=0";
                DataSet dsfindid1 = da1.GetDataSetInline(finsql1);

                // Milestone percentage //
                string strstatusupdate = "update Pms_tbl_AddMilestone set iscompletepercentage='" + milestonetotalpercent.ToString() + "' where id='" + milstoneid + "' and status=1 and archive=0 and isnull(iscomplete,0)=1";
                DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                // project percentage //
                string finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1  and archive=0";
                DataSet dsfindid2 = da1.GetDataSetInline(finsql2);
                for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
                {
                    decimal sumweitage = 0;
                    decimal subpercentageemile = 0;
                    sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
                    subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                    totlweitagemile += (sumweitage * subpercentageemile) / 100;

                }

                string finsql4 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitagemile.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
                DataSet dsfindid4 = da1.GetDataSetInline(finsql4);

                //project  log details insert start
                SqlParameter[] prmlog = new SqlParameter[8];
                prmlog[0] = new SqlParameter("@Transid", milstoneid);
                prmlog[1] = new SqlParameter("@Logstatus", statusid);
                prmlog[2] = new SqlParameter("@LogDate", da1.getcdatetime());
                prmlog[3] = new SqlParameter("@Logtype", "milestone");
                prmlog[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prmlog[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                prmlog[6] = new SqlParameter("@cdate", da1.getcdatetime());
                prmlog[7] = new SqlParameter("@type", "Insert");
                da1.executeDMLQuery("pmstbl_Loginsert_usp", prmlog);
                //project  log details insert end

                DataAccess da = new DataAccess();
                string strsql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus where  status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + statusid + "' ";
                DataSet dscode = da.GetDataSetInline(strsql);
                if (dscode.Tables[0].Rows.Count > 0)
                {
                    retval = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
                }
            }




        }
        else
        {
            SqlParameter[] prm = new SqlParameter[8];
            prm[0] = new SqlParameter("@id", milstoneid);
            prm[1] = new SqlParameter("@milstoneStatus", statusid);
            prm[2] = new SqlParameter("@iscomplete", iscomplete);
            prm[3] = new SqlParameter("@iscompletedate", da1.getcdatetime());
            prm[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prm[6] = new SqlParameter("@cdate", da1.getcdatetime());
            prm[7] = new SqlParameter("@type", entrytype);
            da1.executeDMLQuery("pmstbl_Status_milstone_Update_usp", prm);
            decimal milestonetotalpercent = 0;
            decimal totlweitagemile = 0;
            string finsql1 = "select * from Pms_tbl_AddMilestone where id=" + milstoneid + " and status=1  and archive=0";
            DataSet dsfindid1 = da1.GetDataSetInline(finsql1);

            // Milestone percentage //
            string strstatusupdate = "update Pms_tbl_AddMilestone set iscompletepercentage='" + milestonetotalpercent.ToString() + "' where id='" + milstoneid + "' and status=1 and archive=0 and isnull(iscomplete,0)=0";
            DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

            // project percentage //
            string finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1  and archive=0";
            DataSet dsfindid2 = da1.GetDataSetInline(finsql2);
            for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
            {
                decimal sumweitage = 0;
                decimal subpercentageemile = 0;
                sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
                subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                totlweitagemile += (sumweitage * subpercentageemile) / 100;

            }

            string finsql4 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitagemile.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da1.GetDataSetInline(finsql4);
            //project  log details insert start
            SqlParameter[] prmlog = new SqlParameter[8];
            prmlog[0] = new SqlParameter("@Transid", milstoneid);
            prmlog[1] = new SqlParameter("@Logstatus", statusid);
            prmlog[2] = new SqlParameter("@LogDate", da1.getcdatetime());
            prmlog[3] = new SqlParameter("@Logtype", "milestone");
            prmlog[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prmlog[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prmlog[6] = new SqlParameter("@cdate", da1.getcdatetime());
            prmlog[7] = new SqlParameter("@type", "Insert");
            da1.executeDMLQuery("pmstbl_Loginsert_usp", prmlog);
            //project  log details insert end

            DataAccess da = new DataAccess();
            string strsql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus where  status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + statusid + "' ";
            DataSet dscode = da.GetDataSetInline(strsql);
            if (dscode.Tables[0].Rows.Count > 0)
            {
                retval = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
            }


        }
        // percentage update end  // 





        // return dscode.GetXml();
        return retval;
    }
    [WebMethod]
    public static string Update_Status(string taskid = "", string statusid = "", string iscomplete = "")
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (taskid != "" && taskid != "0")
        {
            entrytype = "Update";
        }




        // percentage update start  // 
        if (iscomplete == "1")
        {
            string independency = checkdependency("task", taskid);
            if (independency == "1")
            {
                retval = "Please complete first dependency of this? ";
            }
            else
            {
                SqlParameter[] prm = new SqlParameter[8];
                prm[0] = new SqlParameter("@id", taskid);
                prm[1] = new SqlParameter("@ProjectStatus", statusid);
                prm[2] = new SqlParameter("@iscomplete", iscomplete);
                prm[3] = new SqlParameter("@iscompletedate", da1.getcdatetime());
                prm[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prm[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                prm[6] = new SqlParameter("@cdate", da1.getcdatetime());
                prm[7] = new SqlParameter("@type", entrytype);
                da1.executeDMLQuery("pmstbl_task_Status_DeatilsUpdate", prm);
                decimal tsktotalpercent = 100;
                decimal totlweitagemile = 0;
                decimal totlweitageproject = 0;
                string finsql1 = "select * from Pms_tbl_Addtask where id=" + taskid + " and status=1 and archive=0";
                DataSet dsfindid1 = da1.GetDataSetInline(finsql1);

                // task percentage //
                string strstatusupdate = "update Pms_tbl_Addtask set iscompletepercentage='" + tsktotalpercent.ToString() + "' where id='" + taskid + "' and status=1 and archive=0 and isnull(iscomplete,0)=1";
                DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                // milestone percentage //
                string finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'   and status=1  and archive=0";
                DataSet dsfindid2 = da1.GetDataSetInline(finsql2);
                for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
                {
                    decimal sumweitage = 0;
                    decimal subpercentageemile = 0;
                    sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
                    subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                    totlweitagemile += (sumweitage * subpercentageemile) / 100;

                }

                string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile.ToString() + "' where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and id='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1 and archive=0";
                DataSet dsfindid4 = da1.GetDataSetInline(finsql4);

                // project percentage //

                string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1  and archive=0";
                DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
                for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
                {
                    decimal sumweitageprojt = 0;
                    decimal subpercentageeproject = 0;
                    sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                    subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                    totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

                }

                string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
                DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);



                //project  log details insert start
                SqlParameter[] prmlog = new SqlParameter[8];
                prmlog[0] = new SqlParameter("@Transid", taskid);
                prmlog[1] = new SqlParameter("@Logstatus", statusid);
                prmlog[2] = new SqlParameter("@LogDate", da1.getcdatetime());
                prmlog[3] = new SqlParameter("@Logtype", "task");
                prmlog[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prmlog[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                prmlog[6] = new SqlParameter("@cdate", da1.getcdatetime());
                prmlog[7] = new SqlParameter("@type", "Insert");
                da1.executeDMLQuery("pmstbl_Loginsert_usp", prmlog);
                //project  log details insert end

                string strsql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + statusid + "' ";
                DataSet dscode = da1.GetDataSetInline(strsql);
                if (dscode.Tables[0].Rows.Count > 0)
                {
                    retval = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
                }
            }


        }
        else
        {
            SqlParameter[] prm = new SqlParameter[8];
            prm[0] = new SqlParameter("@id", taskid);
            prm[1] = new SqlParameter("@ProjectStatus", statusid);
            prm[2] = new SqlParameter("@iscomplete", iscomplete);
            prm[3] = new SqlParameter("@iscompletedate", da1.getcdatetime());
            prm[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prm[6] = new SqlParameter("@cdate", da1.getcdatetime());
            prm[7] = new SqlParameter("@type", entrytype);
            da1.executeDMLQuery("pmstbl_task_Status_DeatilsUpdate", prm);
            decimal tsktotalpercent = 0;
            decimal totlweitagemile = 0;
            decimal totlweitageproject = 0;

            string finsql1 = "select * from Pms_tbl_Addtask where id=" + taskid + " and status=1  and archive=0";
            DataSet dsfindid1 = da1.GetDataSetInline(finsql1);

            // task percentage //
            if (dsfindid1.Tables[0].Rows.Count > 0)
            {
                if (dsfindid1.Tables[0].Rows[0]["issubtask"].ToString() == "1")
                {

                    string finsql211 = "select isnull(sum(weightage),0)Wattagetasksum from Pms_tbl_Addsubtask where   task='" + taskid + "'  and status=1 and isnull(iscomplete,0)=1 and archive=0";
                    DataSet dsfindid211 = da1.GetDataSetInline(finsql211);
                    if (dsfindid211.Tables[0].Rows.Count > 0)
                    {
                        string strstatusupdate = "update Pms_tbl_Addtask set iscompletepercentage='" + dsfindid211.Tables[0].Rows[0]["Wattagetasksum"] + "' where id='" + taskid + "' and status=1 and archive=0";
                        DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);
                    }
                }
                else
                {
                    string strstatusupdate = "update Pms_tbl_Addtask set iscompletepercentage='0' where id='" + taskid + "' and status=1 and archive=0";
                    DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                }

            }


            // milestone percentage //

            string finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1  and archive=0";
            DataSet dsfindid2 = da1.GetDataSetInline(finsql2);
            for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
            {
                decimal sumweitage = 0;
                decimal subpercentageemile = 0;
                sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
                subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                totlweitagemile += (sumweitage * subpercentageemile) / 100;

            }

            string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile + "' where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and id='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da1.GetDataSetInline(finsql4);

            // project percentage //

            string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'   and status=1  and archive=0";
            DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
            for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
            {
                decimal sumweitageprojt = 0;
                decimal subpercentageeproject = 0;
                sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

            }

            string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
            DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);



            //project  log details insert start
            SqlParameter[] prmlog = new SqlParameter[8];
            prmlog[0] = new SqlParameter("@Transid", taskid);
            prmlog[1] = new SqlParameter("@Logstatus", statusid);
            prmlog[2] = new SqlParameter("@LogDate", da1.getcdatetime());
            prmlog[3] = new SqlParameter("@Logtype", "task");
            prmlog[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prmlog[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prmlog[6] = new SqlParameter("@cdate", da1.getcdatetime());
            prmlog[7] = new SqlParameter("@type", "Insert");
            da1.executeDMLQuery("pmstbl_Loginsert_usp", prmlog);
            //project  log details insert end

            string strsql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + statusid + "' ";
            DataSet dscode = da1.GetDataSetInline(strsql);
            if (dscode.Tables[0].Rows.Count > 0)
            {
                retval = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
            }

        }

        // percentage update end  // 




        return retval;
    }

    [WebMethod]
    public static string sbtskUpdate_Status(string subtaskid = "", string statusid = "", string iscomplete = "")
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
       
        if (iscomplete == "1")
        {
            string independency = checkdependency("subtask", subtaskid);
            if (independency == "1")
            {
                retval = "Please complete first dependency of this? ";
            }
            else
            {
                
                SqlParameter[] prm = new SqlParameter[8];
                prm[0] = new SqlParameter("@id", subtaskid);
                prm[1] = new SqlParameter("@ProjectStatus", statusid);
                prm[2] = new SqlParameter("@iscomplete", iscomplete);
                prm[3] = new SqlParameter("@iscompletedate", da1.getcdatetime());
                prm[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prm[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                prm[6] = new SqlParameter("@cdate", da1.getcdatetime());
                prm[7] = new SqlParameter("@type", entrytype);
                da1.executeDMLQuery("pmstbl_subtask_Status_DeatilsUpdate", prm);


                decimal subtsktotalpercent = 100;
                decimal totlweitagemile = 0;
                decimal totlweitageproject = 0;

                string finsql1 = "select * from Pms_tbl_Addsubtask where id=" + subtaskid + " and status=1  and archive=0";
                DataSet dsfindid1 = da1.GetDataSetInline(finsql1);
                SaveLogText("finsql2-"+ finsql1);
                string finsql2 = "select isnull(sum(weightage),0)Wattagetasksum from Pms_tbl_Addsubtask where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "' and task='" + dsfindid1.Tables[0].Rows[0]["task"] + "'  and status=1 and  archive=0 and isnull(iscomplete,0)=1";
                
                DataSet dsfindid2 = da1.GetDataSetInline(finsql2);

                //sub task percentage //
                string strstatusupdate = "update Pms_tbl_Addsubtask set iscompletepercentage='" + subtsktotalpercent.ToString() + "' where id='" + subtaskid + "' and status=1 and archive=0 and isnull(iscomplete,0)=1";
                DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                // task percentage //
                string finsql3 = "update Pms_tbl_Addtask set iscompletepercentage='" + dsfindid2.Tables[0].Rows[0]["Wattagetasksum"] + "' where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "' and id='" + dsfindid1.Tables[0].Rows[0]["task"] + "' and status=1 and archive=0";
                DataSet dsfindid3 = da1.GetDataSetInline(finsql3);


                // Milestone percentage //
                string findsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1  and archive=0";
                DataSet dsfindidmile = da1.GetDataSetInline(findsqlmile);
                for (int j = 0; j < dsfindidmile.Tables[0].Rows.Count; j++)
                {
                    decimal sumweitage = 0;
                    decimal subpercentageemile = 0;
                    sumweitage = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["Weightage"].ToString());
                    subpercentageemile = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                    totlweitagemile += (sumweitage * subpercentageemile) / 100;

                }

                string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile + "' where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and id='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1 and archive=0";
                DataSet dsfindid4 = da1.GetDataSetInline(finsql4);

                // project percentage //
                string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'   and status=1  and archive=0";
                DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
                for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
                {
                    decimal sumweitageprojt = 0;
                    decimal subpercentageeproject = 0;
                    sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                    subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                    totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

                }

                string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'   and status=1 and archive=0";
                DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);
                //project  log details insert start
                SqlParameter[] prmlog = new SqlParameter[8];
                prmlog[0] = new SqlParameter("@Transid", subtaskid);
                prmlog[1] = new SqlParameter("@Logstatus", statusid);
                prmlog[2] = new SqlParameter("@LogDate", da1.getcdatetime());
                prmlog[3] = new SqlParameter("@Logtype", "subtask");
                prmlog[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prmlog[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                prmlog[6] = new SqlParameter("@cdate", da1.getcdatetime());
                prmlog[7] = new SqlParameter("@type", "Insert");
                da1.executeDMLQuery("pmstbl_Loginsert_usp", prmlog);
                //project  log details insert end



                
                string strsql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + statusid + "' ";
                DataSet dscode = da1.GetDataSetInline(strsql);
                if (dscode.Tables[0].Rows.Count > 0)
                {
                    retval = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
                }
            }


        }
        else
        {
            decimal subtsktotalpercent = 0;
            decimal totlweitagemile = 0;
            decimal totlweitageproject = 0;
            SqlParameter[] prm = new SqlParameter[8];
            prm[0] = new SqlParameter("@id", subtaskid);
            prm[1] = new SqlParameter("@ProjectStatus", statusid);
            prm[2] = new SqlParameter("@iscomplete", iscomplete);
            prm[3] = new SqlParameter("@iscompletedate", da1.getcdatetime());
            prm[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prm[6] = new SqlParameter("@cdate", da1.getcdatetime());
            prm[7] = new SqlParameter("@type", entrytype);
            da1.executeDMLQuery("pmstbl_subtask_Status_DeatilsUpdate", prm);
            string finsql1 = "select * from Pms_tbl_Addsubtask where id=" + subtaskid + " and status=1  and archive=0";
            DataSet dsfindid1 = da1.GetDataSetInline(finsql1);

            string finsql2 = "select isnull(sum(weightage),0)Wattagetasksum from Pms_tbl_Addsubtask where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "' and task='" + dsfindid1.Tables[0].Rows[0]["task"] + "'  and status=1 and isnull(iscomplete,0)=1 and archive=0";
            DataSet dsfindid2 = da1.GetDataSetInline(finsql2);

            //sub task percentage //
            string strstatusupdate = "update Pms_tbl_Addsubtask set iscompletepercentage='" + subtsktotalpercent.ToString() + "' where id='" + subtaskid + "' and status=1 and archive=0";
            DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

            //task percentage //
            string finsql3 = "update Pms_tbl_Addtask set iscompletepercentage='" + dsfindid2.Tables[0].Rows[0]["Wattagetasksum"] + "' where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "' and id='" + dsfindid1.Tables[0].Rows[0]["task"] + "' and status=1 and archive=0";
            DataSet dsfindid3 = da1.GetDataSetInline(finsql3);

            //milestone percentage //
            string findsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'    and status=1  and archive=0";
            DataSet dsfindidmile = da1.GetDataSetInline(findsqlmile);
            for (int j = 0; j < dsfindidmile.Tables[0].Rows.Count; j++)
            {
                decimal sumweitage = 0;
                decimal subpercentageemile = 0;
                sumweitage = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["Weightage"].ToString());
                subpercentageemile = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                totlweitagemile += (sumweitage * subpercentageemile) / 100;

            }

            string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile + "' where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and id='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da1.GetDataSetInline(finsql4);


            // project percentage //
            string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'   and status=1  and archive=0";
            DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
            for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
            {
                decimal sumweitageprojt = 0;
                decimal subpercentageeproject = 0;
                sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

            }

            string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'   and status=1 and archive=0";
            DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);
            //project  log details insert start
            SqlParameter[] prmlog = new SqlParameter[8];
            prmlog[0] = new SqlParameter("@Transid", subtaskid);
            prmlog[1] = new SqlParameter("@Logstatus", statusid);
            prmlog[2] = new SqlParameter("@LogDate", da1.getcdatetime());
            prmlog[3] = new SqlParameter("@Logtype", "subtask");
            prmlog[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prmlog[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prmlog[6] = new SqlParameter("@cdate", da1.getcdatetime());
            prmlog[7] = new SqlParameter("@type", "Insert");
            da1.executeDMLQuery("pmstbl_Loginsert_usp", prmlog);
            //project  log details insert end



           
            string strsql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + statusid + "' ";
            DataSet dscode = da1.GetDataSetInline(strsql);
            if (dscode.Tables[0].Rows.Count > 0)
            {
                retval = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
            }


        }
        // percentage update end  // 


       

        return retval;
    }


    [WebMethod]
    public static string MileUpdatePriority_Status(string milstoneid = "", string Propertyid = "")
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (milstoneid != "" && milstoneid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", milstoneid);
        prm[1] = new SqlParameter("@Priority", Propertyid);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pmstbl_Priority_milstone_DtlsUpdate", prm);

        DataAccess da = new DataAccess();
        string strsql = "select Id,name,PrintColor from Pms_tbl_Priority where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + Propertyid + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            retval = dscode.Tables[0].Rows[0]["name"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
        }
        // return dscode.GetXml();
        return retval;
    }


    [WebMethod]
    public static string projectUpdatePriority_Status(string projectid = "", string Propertyid = "")
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (projectid != "" && projectid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", projectid);
        prm[1] = new SqlParameter("@Priority", Propertyid);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pmstbl_Priority_DeatilsUpdate_usp", prm);

        DataAccess da = new DataAccess();
        string strsql = "select Id,name,PrintColor from Pms_tbl_Priority  where  status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + Propertyid + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            retval = dscode.Tables[0].Rows[0]["name"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
        }
        // return dscode.GetXml();
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
        da1.executeDMLQuery("pms_tbl_task_Priority_DeatilsUpdate", prm);

        DataAccess da = new DataAccess();
        string strsql = "select Id,name,PrintColor from Pms_tbl_Priority  where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + Propertyid + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            retval = dscode.Tables[0].Rows[0]["name"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
        }

        return retval;
    }

   
    [WebMethod]
    public static string sbtskUpdatePriority_Status(string taskid = "", string Propertyid = "")
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
        da1.executeDMLQuery("pmstbl_Stage_DeatilsUpdate", prm);

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
    public static string sbtskUpdate_Stagge(string taskid = "", string Stageid = "")
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
                DateTime estendStartDate = Convert.ToDateTime(dsstrextend.Tables[0].Rows[k]["StartDate"].ToString() == "" ? DateTime.Now.ToString() : dsstrextend.Tables[0].Rows[k]["StartDate"].ToString());
                DateTime estendTargetdate = Convert.ToDateTime(dsstrextend.Tables[0].Rows[k]["TargetDate"].ToString() == "" ? DateTime.Now.ToString() : dsstrextend.Tables[0].Rows[k]["TargetDate"].ToString());
                TimeSpan estendobjTimeSpan = estendTargetdate - estendStartDate;
                estendDays = Convert.ToDouble(estendobjTimeSpan.TotalDays);
                estendDays = estendDays + 1;


                DateTime estendEnddate = Convert.ToDateTime(dsstrextend.Tables[0].Rows[k]["Targetdate"].ToString() == "" ? DateTime.Now.ToString() : dsstrextend.Tables[0].Rows[k]["Targetdate"].ToString());
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
    public static string sbtskSave_Owner(string id = "", string ownerdropdownid = "")
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
    public static string statussavesubtask(string value = "", string statuscolor = "")
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
    public static string SaveMilestone(milestonetdata milestonedetails)
    {
        function fns = new function();
        string retval = "0";
        DataAccess da1 = new DataAccess();
        string editid = milestonedetails.id;
        string entrytype = "Insert";
        if (editid != "" && editid != "0")
        {
            entrytype = "Update";
        }
        string istasks = "0";
        string checkmileWeightages = "0";
        try
        {
            string sqlM = "select id,isnull(hastask,0) as istask,isnull(checkprojectWeightage,0) as checkmileWeightage from pms_tbl_addproject where status=1 and archive=0 and id=" + milestonedetails.ProjectId + " and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
            SaveLogText("checkprms1=" + sqlM);
            DataSet dsM = da1.GetDataSetInline(sqlM);
            if (dsM.Tables[0].Rows.Count > 0)
            {
                istasks = dsM.Tables[0].Rows[0]["istask"].ToString();
                checkmileWeightages = dsM.Tables[0].Rows[0]["checkmileWeightage"].ToString();
            }
        }
        catch { }
        if(milestonedetails.istask==string.Empty)
        {
            milestonedetails.istask = istasks;
        }
        if (milestonedetails.checkmileWeightage == string.Empty)
        {
            milestonedetails.checkmileWeightage = checkmileWeightages;
        }
        SaveLogText("checkprms2="+ milestonedetails.istask+","+ milestonedetails.checkmileWeightage);
        SqlParameter[] prm = new SqlParameter[22];
        prm[0] = new SqlParameter("@id", editid);
        prm[1] = new SqlParameter("@ProjectId", milestonedetails.ProjectId);
        prm[2] = new SqlParameter("@Milestone", milestonedetails.Milestone);
        prm[3] = new SqlParameter("@Weightage", milestonedetails.Weightage);
        if (milestonedetails.StartDate == string.Empty)
        {
            prm[4] = new SqlParameter("@StartDate", DBNull.Value);
        }
        else
        {
            prm[4] = new SqlParameter("@StartDate", milestonedetails.StartDate);
        }
        if (milestonedetails.EndDate == string.Empty)
        {
            prm[5] = new SqlParameter("@EndDate", DBNull.Value);
        }
        else
        {
            prm[5] = new SqlParameter("@EndDate", milestonedetails.EndDate);
        }
        prm[6] = new SqlParameter("@Owner", milestonedetails.Owner);
        prm[7] = new SqlParameter("@Access", milestonedetails.Access);

        prm[8] = new SqlParameter("@team", milestonedetails.team);
        prm[9] = new SqlParameter("@P_Status", milestonedetails.P_Status);
        prm[10] = new SqlParameter("@Priority", milestonedetails.Priority);
        if (milestonedetails.Budget == string.Empty)
        {
            prm[11] = new SqlParameter("@Budget", DBNull.Value);
        }
        else
        {
            prm[11] = new SqlParameter("@Budget", milestonedetails.Budget);
        }

        if (milestonedetails.Actual == string.Empty)
        {
            prm[12] = new SqlParameter("@Actual", DBNull.Value);
        }
        else
        {
            prm[12] = new SqlParameter("@Actual", milestonedetails.Actual);
        }
        prm[13] = new SqlParameter("@checkmileWeightage", milestonedetails.checkmileWeightage);

        prm[14] = new SqlParameter("@Durationtype", milestonedetails.Durationtype);
        prm[15] = new SqlParameter("@TaskDuration", milestonedetails.TaskDuration);

        prm[16] = new SqlParameter("@istask", milestonedetails.istask);
        prm[17] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[18] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[19] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[20] = new SqlParameter("@type", entrytype);
        prm[21] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[21].Direction = ParameterDirection.ReturnValue;
        int retval1 = da1.executeDMLQuery("pmstbl_Addmilestone_usp", prm);
        retval = retval1.ToString();


        //Hours days month calculated start //

        //Hours
        if (milestonedetails.Durationtype == "1")
        {
            string strhours = "update Pms_tbl_AddMilestone set Durationhourcal='" + milestonedetails.TaskDuration + "' where id = " + retval + " and status=1  and archive=0";
            DataSet dshours = da1.GetDataSetInline(strhours);

        }

        //Days
        decimal totaldayshours = 0;
        if (milestonedetails.Durationtype == "2")
        {
            decimal dayscount = Convert.ToDecimal(milestonedetails.TaskDuration);
            totaldayshours = dayscount * 24;
            string strDays = "update Pms_tbl_AddMilestone set Durationhourcal='" + totaldayshours + "' where id = " + retval + " and status=1  and archive=0";
            DataSet dsDays = da1.GetDataSetInline(strDays);

        }

        //Months
        decimal totalMonthhours = 0;
        if (milestonedetails.Durationtype == "3")
        {
            decimal Monthdyscount = Convert.ToDecimal(milestonedetails.TaskDuration);
            decimal Monthdyshourcount = Monthdyscount * 30;
            totalMonthhours = Monthdyshourcount * 24;
            string strMonths = "update Pms_tbl_AddMilestone set Durationhourcal='" + totalMonthhours + "' where id = " + retval + " and status=1  and archive=0";
            DataSet dsMonths = da1.GetDataSetInline(strMonths);

        }

        //Hours days month calculated start //



        // iscomplete update //

        string striscomplt = "select  isnull(iscomplete,0) as iscomplete from Pms_tbl_Projectstatus where status = 1 and archive = 0  and id='" + milestonedetails.P_Status + "'";
        DataSet dsiscomplt = da1.GetDataSetInline(striscomplt);
        if (dsiscomplt.Tables[0].Rows.Count > 0)
        {
            int iscompletestatus = 0;
            iscompletestatus = Convert.ToInt32(dsiscomplt.Tables[0].Rows[0]["iscomplete"]);

            string strstatusupdateiscom = "update Pms_tbl_AddMilestone set iscomplete='" + iscompletestatus.ToString() + "' where id = " + retval + " and status=1 and archive=0";
            DataSet dshourminiscom = da1.GetDataSetInline(strstatusupdateiscom);

        }

        //   iscomplete update  



        int checkweitage = 0;
        decimal wetagecount = 0;
        decimal totalwetage = 0;
        string strweightage = "select isnull(checkprojectWeightage ,0) as checkprojectWeightage  from Pms_tbl_AddProject where id='" + milestonedetails.ProjectId + "' and  status=1 and  isnull(checkprojectWeightage ,0)=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
        DataSet dsweitagecheck = da1.GetDataSetInline(strweightage);
        if (dsweitagecheck.Tables[0].Rows.Count > 0)
        {

            checkweitage = Convert.ToInt32(dsweitagecheck.Tables[0].Rows[0]["checkprojectWeightage"]);

        }

        if (checkweitage == 1)
        {
            string strweightagecount = "select count (*) as WeitageCount from Pms_tbl_AddMilestone where ProjectId='" + milestonedetails.ProjectId + "' and  status=1   and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
            DataSet dsweitagecount = da1.GetDataSetInline(strweightagecount);
            if (dsweitagecount.Tables[0].Rows.Count > 0)
            {


                wetagecount = Convert.ToInt32(dsweitagecount.Tables[0].Rows[0]["WeitageCount"]);

            }

            totalwetage = 100 / wetagecount;
            string strstatusupdate = "update Pms_tbl_AddMilestone set Weightage='" + fns.ToFormattedCurrencyString(totalwetage.ToString()) + "' where ProjectId='" + milestonedetails.ProjectId + "' and status=1  and archive=0";
            DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

        }


        int iscomplete = 0;
        string findsqlismile = "select isnull(iscomplete,0) as iscomplete, * from Pms_tbl_AddMilestone where id=" + retval + " and status=1 and archive=0";
        DataSet dsfindidismile = da1.GetDataSetInline(findsqlismile);
        if (dsfindidismile.Tables[0].Rows.Count > 0)
        {

            iscomplete = Convert.ToInt32(dsfindidismile.Tables[0].Rows[0]["iscomplete"]);

        }



        if (iscomplete == 1)
        {
            decimal milestonetotalpercent = 100;
            decimal totlweitagemile = 0;
            string finsql1 = "select * from Pms_tbl_AddMilestone where id=" + retval + " and status=1  and archive=0";
            DataSet dsfindid1 = da1.GetDataSetInline(finsql1);

            // Milstone percentage //
            string strstatusupdate = "update Pms_tbl_AddMilestone set iscompletepercentage='" + milestonetotalpercent.ToString() + "' where id='" + retval + "' and isnull(iscomplete,0)=1 and status=1 and archive=0";
            DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

            // project percentage //
            string finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1  and archive=0";
            DataSet dsfindid2 = da1.GetDataSetInline(finsql2);
            for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
            {
                decimal sumweitage = 0;
                decimal subpercentageemile = 0;
                sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
                subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                totlweitagemile += (sumweitage * subpercentageemile) / 100;

            }

            string finsql4 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitagemile.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da1.GetDataSetInline(finsql4);






        }
        else
        {
            string finsql2 = "";
            DataSet dsfindid2;
            decimal milestonetotalpercent = 0;
            decimal totlweitagemile = 0;
            decimal totlweitageproject = 0;
            string finsql1 = "select * from Pms_tbl_AddMilestone where id=" + retval + " and status=1 and archive=0";
            DataSet dsfindid1 = da1.GetDataSetInline(finsql1);
            if (dsfindid1.Tables[0].Rows.Count > 0)
            {
                if (dsfindid1.Tables[0].Rows[0]["istask"].ToString() == "1")
                {



                    finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where  Milestoneid='" + retval + "'  and status=1  and archive=0";
                    dsfindid2 = da1.GetDataSetInline(finsql2);
                    for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
                    {
                        decimal sumweitage = 0;
                        decimal subpercentageemile = 0;
                        sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
                        subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                        totlweitagemile += (sumweitage * subpercentageemile) / 100;

                    }
                }

            }
            // Milestone percentage //
            string strstatusupdate = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile.ToString() + "' where id='" + retval + "' and status=1 and archive=0 and isnull(iscomplete,0)=0";
            DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

            // project percentage //
            finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1  and archive=0";
            dsfindid2 = da1.GetDataSetInline(finsql2);
            for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
            {
                decimal sumweitage = 0;
                decimal subpercentageemile = 0;
                sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
                subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                totlweitageproject += (sumweitage * subpercentageemile) / 100;

            }

            string finsql4 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da1.GetDataSetInline(finsql4);



        }



        return retval;


    }



    [WebMethod]
    public static string AddPrioritytypesubtask(string value = "", string prioritycolor = "")
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
    public static string AddStagesubtask(string value = "", string Stagecolor = "")
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
    public static string SaveSubTask(SubSubtaskdata subtaskdetails)
    {
        function fns = new function();
        string retval = "0";
        DataAccess da1 = new DataAccess();
        string editid = subtaskdetails.id;
        string entrytype = "Insert";
        if (editid != "" && editid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[33];
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
        prm[32] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[32].Direction = ParameterDirection.ReturnValue;
        int retval1 = da1.executeDMLQuery("pms_Add_Subtask_usp", prm);
        retval = retval1.ToString();



        // iscomplete update //

        string striscomplt = "select  isnull(iscomplete,0) as iscomplete from Pms_tbl_Projectstatus where status = 1 and archive = 0  and id='" + subtaskdetails.P_Status + "'";
        DataSet dsiscomplt = da1.GetDataSetInline(striscomplt);
        if (dsiscomplt.Tables[0].Rows.Count > 0)
        {
            int iscompletestatus = 0;
            iscompletestatus = Convert.ToInt32(dsiscomplt.Tables[0].Rows[0]["iscomplete"]);

            string strstatusupdateiscom = "update Pms_tbl_Addsubtask set iscomplete='" + iscompletestatus.ToString() + "' where id = " + retval + " and status=1 and archive=0";
            DataSet dshourminiscom = da1.GetDataSetInline(strstatusupdateiscom);

        }

        //   iscomplete update  



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
            string strstatusupdate = "update Pms_tbl_Addsubtask set Weightage='" + fns.ToFormattedCurrencyString(totalwetage.ToString()) + "' where task='" + subtaskdetails.task + "' and status=1 and archive=0";
            DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

        }


        int iscomplete = 0;
        string finsql1 = "select  isnull(iscomplete,0) as iscomplete,* from Pms_tbl_Addsubtask where id=" + retval + " and status=1  and archive=0";
        DataSet dsfindid1 = da1.GetDataSetInline(finsql1);
        if (dsfindid1.Tables[0].Rows.Count > 0)
        {

            iscomplete = Convert.ToInt32(dsfindid1.Tables[0].Rows[0]["iscomplete"]);

        }

        if (iscomplete == 1)
        {
            decimal subtsktotalpercent = 100;
            decimal totlweitagemile = 0;
            decimal totlweitageproject = 0;

            string finsql01 = "select * from Pms_tbl_Addsubtask where id=" + retval + " and status=1  and archive=0";
            DataSet dsfindid01 = da1.GetDataSetInline(finsql01);

            string finsql2 = "select isnull(sum(weightage),0)Wattagetasksum from Pms_tbl_Addsubtask where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "' and task='" + dsfindid01.Tables[0].Rows[0]["task"] + "'  and status=1 and isnull(iscomplete,0)=1 and archive=0";
            DataSet dsfindid2 = da1.GetDataSetInline(finsql2);

            //sub task percentage //
            string strstatusupdate = "update Pms_tbl_Addsubtask set iscompletepercentage='" + subtsktotalpercent.ToString() + "' where id='" + retval + "' and status=1 and archive=0 and isnull(iscomplete,0)=1";
            DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

            // task percentage //
            string finsql3 = "update Pms_tbl_Addtask set iscompletepercentage='" + dsfindid2.Tables[0].Rows[0]["Wattagetasksum"] + "' where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "' and id='" + dsfindid01.Tables[0].Rows[0]["task"] + "' and status=1 and archive=0";
            DataSet dsfindid3 = da1.GetDataSetInline(finsql3);


            // Milestone percentage //
            string findsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask  where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1  and archive=0";
            DataSet dsfindidmile = da1.GetDataSetInline(findsqlmile);
            for (int j = 0; j < dsfindidmile.Tables[0].Rows.Count; j++)
            {
                decimal sumweitage = 0;
                decimal subpercentageemile = 0;
                sumweitage = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["Weightage"].ToString());
                subpercentageemile = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                totlweitagemile += (sumweitage * subpercentageemile) / 100;

            }

            string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile + "' where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and id='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da1.GetDataSetInline(finsql4);



            // project percentage //
            string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "'    and status=1  and archive=0";
            DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
            for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
            {
                decimal sumweitageprojt = 0;
                decimal subpercentageeproject = 0;
                sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

            }

            string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
            DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);






        }

        else
        {
            decimal subtsktotalpercent = 0;
            decimal totlweitagemile = 0;
            decimal totlweitageproject = 0;

            string finsql01 = "select * from Pms_tbl_Addsubtask where id=" + retval + " and status=1 and archive=0";
            DataSet dsfindid01 = da1.GetDataSetInline(finsql1);

            string finsql2 = "select isnull(sum(weightage),0)Wattagetasksum from Pms_tbl_Addsubtask where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "' and task='" + dsfindid01.Tables[0].Rows[0]["task"] + "'  and status=1 and isnull(iscomplete,0)=1 and archive=0";
            DataSet dsfindid2 = da1.GetDataSetInline(finsql2);

            //Sub task percentage //
            string strstatusupdate = "update Pms_tbl_Addsubtask set iscompletepercentage='" + subtsktotalpercent.ToString() + "' where id='" + retval + "' and status=1 and archive=0 and isnull(iscomplete,0)=0";
            DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

            // task percentage //
            string finsql3 = "update Pms_tbl_Addtask set iscompletepercentage='" + dsfindid2.Tables[0].Rows[0]["Wattagetasksum"] + "' where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "' and id='" + dsfindid01.Tables[0].Rows[0]["task"] + "' and status=1 and archive=0";
            DataSet dsfindid3 = da1.GetDataSetInline(finsql3);


            // Milestone percentage //
            string findsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask  where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "'    and status=1  and archive=0";
            DataSet dsfindidmile = da1.GetDataSetInline(findsqlmile);
            for (int j = 0; j < dsfindidmile.Tables[0].Rows.Count; j++)
            {
                decimal sumweitage = 0;
                decimal subpercentageemile = 0;
                sumweitage = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["Weightage"].ToString());
                subpercentageemile = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                totlweitagemile += (sumweitage * subpercentageemile) / 100;

            }

            string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile + "' where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and id='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da1.GetDataSetInline(finsql4);


            // project percentage //
            string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "'   and status=1  and archive=0";
            DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
            for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
            {
                decimal sumweitageprojt = 0;
                decimal subpercentageeproject = 0;
                sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

            }

            string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
            DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);


        }

        return retval;

    }





    [WebMethod]
    public static string sbtskBinddetail_Owner(int RecordId = 0)
    {

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "select * from Pms_tbl_Addsubtask where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
        ds = da.GetDataSetInline(strsql);


        return ds.GetXml();
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
    public static string bindtasktimelinedatasubtask(string tski_d = "0")
    {

        string projectdata = "";
        function fns = new function();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();

        //projectdata += "<table cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm datatable'>";
        projectdata += "<table cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm mb-0'>";
        projectdata += "<thead>";
        projectdata += "<tr>";
        projectdata += "<th style=min-width:140px;>Task Timeline</th>";
        projectdata += "</tr>";
        projectdata += "</thead>";
        projectdata += "<tbody>";

        string project_timeline = "";
        string Project_Timeline = "select * from Pms_tbl_Addtask where status=1 and archive=0  and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id = '" + tski_d + "'  ";
        DataSet ds_Project_Timeline = da.GetDataSetInline(Project_Timeline);
        if (ds_Project_Timeline.Tables[0].Rows.Count > 0)
        {
            for (int pt = 0; pt < ds_Project_Timeline.Tables[0].Rows.Count; pt++)
            {

                project_timeline += "<a href = 'javascript:void(0);' class='timesheet-wrap'>";
                // project_timeline += "<div class='progress progress-xl bg-dark position-relative me-1' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + cmf.dateConvert(ds_Project_Timeline.Tables[0].Rows[pt]["Startdate"].ToString()) + " - " + cmf.dateConvert(ds_Project_Timeline.Tables[0].Rows[pt]["Targetdate"].ToString()) + " '>";
                // project_timeline += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 100%;' aria-valuemax='100'>" + ds_Project_Timeline.Tables[0].Rows[pt]["taskname"].ToString() + "</div>";

                project_timeline += "<div class='progress progress-xl bg-dark position-relative me-1' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + ds_Project_Timeline.Tables[0].Rows[pt]["taskname"].ToString() + " '>";
                project_timeline += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 100%;' aria-valuemax='100'>" + cmf.dateConvert(ds_Project_Timeline.Tables[0].Rows[pt]["Startdate"].ToString()) + " - " + cmf.dateConvert(ds_Project_Timeline.Tables[0].Rows[pt]["Targetdate"].ToString()) + "</div>";

                project_timeline += "</div>";
                project_timeline += "</a>";

            }

        }


        projectdata += "<tr>";
        //Task Timeline
        projectdata += "<td valign='top'>";
        projectdata += "<div class='d-flex'>";
        projectdata += "<div class='flex-grow-1 align-self-center me-2'>";
        projectdata += "<div class='extended_time'>";
        projectdata += "" + project_timeline + "";
        projectdata += "</div>";
        projectdata += "</div>";
        projectdata += "</div>";
        projectdata += "</td>";

        projectdata += "</tr>";


        projectdata += "</tbody>";
        projectdata += "</table>";

        return projectdata;
    }

    [WebMethod]
    public static string bindSubtasktimelinedatasubtask(string tski_d = "0")
    {

        string projectdata = "";
        function fns = new function();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();



        string project_timeline = "";
        string strret = "";
        string Project_Timeline = "select  * from Pms_tbl_Addsubtask where status=1 and archive=0  and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and task = '" + tski_d + "'  ";
        DataSet ds_Project_Timeline = da.GetDataSetInline(Project_Timeline);
        if (ds_Project_Timeline.Tables[0].Rows.Count > 0)
        {
            //projectdata += "<table cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm datatable'>";
            projectdata += "<table cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm mb-0'>";
            projectdata += "<thead>";
            projectdata += "<tr>";
            projectdata += "<th style=min-width:140px;>SubTask Timeline</th>";
            projectdata += "</tr>";
            projectdata += "</thead>";
            projectdata += "<tbody>";

            for (int pt = 0; pt < ds_Project_Timeline.Tables[0].Rows.Count; pt++)
            {
                strret = "S" + " " + (pt + 1);
                project_timeline += "<a href = 'javascript:void(0);' class='timesheet-wrap'>";
                project_timeline += "<div class='progress progress-xl bg-dark position-relative me-1' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title=' " + ds_Project_Timeline.Tables[0].Rows[pt]["Subtaskname"].ToString() + "  " + cmf.dateConvert(ds_Project_Timeline.Tables[0].Rows[pt]["Startdate"].ToString()) + " - " + cmf.dateConvert(ds_Project_Timeline.Tables[0].Rows[pt]["Targetdate"].ToString()) + " '>";
                project_timeline += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 100%;' aria-valuemax='100'>" + strret + "</div>";
                project_timeline += "</div>";
                project_timeline += "</a>";

            }

        }


        projectdata += "<tr>";
        //Project Timeline
        projectdata += "<td valign='top'>";
        projectdata += "<div class='d-flex'>";
        projectdata += "<div class='flex-grow-1 align-self-center me-2'>";
        projectdata += "<div class='extended_time'>";
        projectdata += "" + project_timeline + "";
        projectdata += "</div>";
        projectdata += "</div>";
        projectdata += "</div>";
        projectdata += "</td>";

        projectdata += "</tr>";


        projectdata += "</tbody>";
        projectdata += "</table>";

        return projectdata;
    }


    [WebMethod]
    public static string Bindcheckstrtdatetaskdetails(int taski_d = 0)
    {

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "select * from Pms_tbl_Addtask where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + taski_d + "'";
        ds = da.GetDataSetInline(strsql);


        return ds.GetXml();
    }


    [WebMethod]
    public static string bindtasktimelinedata_subtask(string tski_d = "0")
    {

        string projectdata = "";
        function fns = new function();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();

        //projectdata += "<table cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm datatable'>";
        projectdata += "<table cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm mb-0'>";
        projectdata += "<thead>";
        projectdata += "<tr>";
        projectdata += "<th style=min-width:140px;>Task Timeline</th>";
        projectdata += "</tr>";
        projectdata += "</thead>";
        projectdata += "<tbody>";

        string project_timeline = "";
        string Project_Timeline = "select * from Pms_tbl_Addtask where status=1 and archive=0  and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id = '" + tski_d + "'  ";
        DataSet ds_Project_Timeline = da.GetDataSetInline(Project_Timeline);
        if (ds_Project_Timeline.Tables[0].Rows.Count > 0)
        {
            for (int pt = 0; pt < ds_Project_Timeline.Tables[0].Rows.Count; pt++)
            {

                project_timeline += "<a href = 'javascript:void(0);' class='timesheet-wrap'>";
                // project_timeline += "<div class='progress progress-xl bg-dark position-relative me-1' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + cmf.dateConvert(ds_Project_Timeline.Tables[0].Rows[pt]["Startdate"].ToString()) + " - " + cmf.dateConvert(ds_Project_Timeline.Tables[0].Rows[pt]["Targetdate"].ToString()) + " '>";
                // project_timeline += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 100%;' aria-valuemax='100'>" + ds_Project_Timeline.Tables[0].Rows[pt]["taskname"].ToString() + "</div>";

                project_timeline += "<div class='progress progress-xl bg-dark position-relative me-1' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + ds_Project_Timeline.Tables[0].Rows[pt]["taskname"].ToString() + " '>";
                project_timeline += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 100%;' aria-valuemax='100'>" + cmf.dateConvert(ds_Project_Timeline.Tables[0].Rows[pt]["Startdate"].ToString()) + " - " + cmf.dateConvert(ds_Project_Timeline.Tables[0].Rows[pt]["Targetdate"].ToString()) + "</div>";

                project_timeline += "</div>";
                project_timeline += "</a>";

            }

        }


        projectdata += "<tr>";
        //Task Timeline
        projectdata += "<td valign='top'>";
        projectdata += "<div class='d-flex'>";
        projectdata += "<div class='flex-grow-1 align-self-center me-2'>";
        projectdata += "<div class='extended_time'>";
        projectdata += "" + project_timeline + "";
        projectdata += "</div>";
        projectdata += "</div>";
        projectdata += "</div>";
        projectdata += "</td>";

        projectdata += "</tr>";


        projectdata += "</tbody>";
        projectdata += "</table>";

        return projectdata;
    }


    [WebMethod]
    public static string BindcheckSubTaskdurationdetails(int ts_kid = 0)
    {
        DataAccess da = new DataAccess();
        DataSet ds;
        //string strsql = "select sum(cast(isnull(Pms_tbl_Addtask.TaskDuration,0) as decimal)) as TaskDuration,isnull(Pms_tbl_AddMilestone.Durationhourcal, 0) as Durationhourcal  from Pms_tbl_Addtask inner join Pms_tbl_AddMilestone on Pms_tbl_Addtask.Milestoneid = Pms_tbl_AddMilestone.id where  Pms_tbl_Addtask.status = 1 and Pms_tbl_Addtask.archive = 0 and Pms_tbl_Addtask.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and Pms_tbl_Addtask.Milestoneid = '" + Milestoneid + "'  group by Pms_tbl_AddMilestone.Durationhourcal";
        string strsql = "select sum(cast(isnull(Pms_tbl_Addsubtask.TaskDuration,0) as decimal)) as TaskDuration,isnull(Pms_tbl_Addtask.TaskDuration, 0) as Durationhourcal from Pms_tbl_Addtask left join Pms_tbl_Addsubtask on Pms_tbl_Addtask.id = Pms_tbl_Addsubtask.task where Pms_tbl_Addtask.status = 1 and Pms_tbl_Addtask.archive = 0 and Pms_tbl_Addtask.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and Pms_tbl_Addtask.id = '" + ts_kid + "' group by Pms_tbl_Addtask.TaskDuration";
        ds = da.GetDataSetInline(strsql);
        return ds.GetXml();
    }

    [WebMethod]
    public static string sbtskBindetail_Team(int RecordId = 0)
    {

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "select * from Pms_tbl_Addsubtask where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
        ds = da.GetDataSetInline(strsql);


        return ds.GetXml();
    }

    [WebMethod]
    public static string bindMultiChangesubtaskTeam_new(string tsk_id = "")
    {
        DataAccess da1 = new DataAccess();
        string dataN = "";
        string strsql = "SELECT  Split.value as id,tbl_employee.DisplayName FROM Pms_tbl_Addtask AS task CROSS APPLY STRING_SPLIT(task.team, ',') as Split inner join tbl_employee on Split.value = tbl_employee.id where task.status = 1 and task.archive = 0 and task.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and task.id = '" + tsk_id + "' ";
        DataSet dsN = da1.GetDataSetInline(strsql);

        if (dsN.Tables[0].Rows.Count > 0)
        {

            for (int i1 = 0; i1 < dsN.Tables[0].Rows.Count; i1++)
            {
                if (dataN != string.Empty)
                {
                    dataN += ",{";
                    dataN += "\"id\": \"" + dsN.Tables[0].Rows[i1]["id"].ToString() + "\",\"";
                    dataN += "text\": \"" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "\"";
                    dataN += "}";
                }
                else
                {
                    dataN += "{";
                    dataN += "\"id\": \"" + dsN.Tables[0].Rows[i1]["id"].ToString() + "\",\"";
                    dataN += "text\": \"" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "\"";
                    dataN += "}";
                }
            }

        }

        return dataN;

    }


    [WebMethod]
    public static string sbtskSave_Team(string id = "", string teamdropdownid = "")
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
    public static string Bindsubtaskdetail_extendatedate(int RecordId = 0)
    {

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "select * from Pms_tbl_Addsubtask where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
        ds = da.GetDataSetInline(strsql);


        return ds.GetXml();
    }

    [WebMethod]
    public static string bindMultiOwner_new(string mile_stoneid = "")
    {
        DataAccess da1 = new DataAccess();
        string dataN = "";
        string strsql = "SELECT  Split.value as id,tbl_employee.DisplayName FROM Pms_tbl_AddMilestone AS milestone CROSS APPLY STRING_SPLIT(milestone.Owner, ',') as Split inner join tbl_employee on Split.value = tbl_employee.id where milestone.status = 1 and milestone.archive = 0 and milestone.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and milestone.id = '" + mile_stoneid + "' ";
        DataSet dsN = da1.GetDataSetInline(strsql);

        if (dsN.Tables[0].Rows.Count > 0)
        {

            for (int i1 = 0; i1 < dsN.Tables[0].Rows.Count; i1++)
            {
                if (dataN != string.Empty)
                {
                    dataN += ",{";
                    dataN += "\"id\": \"" + dsN.Tables[0].Rows[i1]["id"].ToString() + "\",\"";
                    dataN += "text\": \"" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "\"";
                    dataN += "}";
                }
                else
                {
                    dataN += "{";
                    dataN += "\"id\": \"" + dsN.Tables[0].Rows[i1]["id"].ToString() + "\",\"";
                    dataN += "text\": \"" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "\"";
                    dataN += "}";
                }
            }

        }

        return dataN;

    }

    [WebMethod]
    public static string bindMultiOwner_newsubtask(string tas_kid = "")
    {
        DataAccess da1 = new DataAccess();
        string dataN = "";
        string strsql = "SELECT  Split.value as id,tbl_employee.DisplayName FROM Pms_tbl_Addtask AS task CROSS APPLY STRING_SPLIT(task.Owner, ',') as Split inner join tbl_employee on Split.value = tbl_employee.id where task.status = 1 and task.archive = 0 and task.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and task.id = '" + tas_kid + "' ";
        DataSet dsN = da1.GetDataSetInline(strsql);

        if (dsN.Tables[0].Rows.Count > 0)
        {

            for (int i1 = 0; i1 < dsN.Tables[0].Rows.Count; i1++)
            {
                if (dataN != string.Empty)
                {
                    dataN += ",{";
                    dataN += "\"id\": \"" + dsN.Tables[0].Rows[i1]["id"].ToString() + "\",\"";
                    dataN += "text\": \"" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "\"";
                    dataN += "}";
                }
                else
                {
                    dataN += "{";
                    dataN += "\"id\": \"" + dsN.Tables[0].Rows[i1]["id"].ToString() + "\",\"";
                    dataN += "text\": \"" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "\"";
                    dataN += "}";
                }
            }

        }

        return dataN;

    }

    [WebMethod]
    public static string bindMultiTeam_newsubtask(string tas_kid = "")
    {
        DataAccess da1 = new DataAccess();
        string dataN = "";
        string strsql = "SELECT  Split.value as id,tbl_employee.DisplayName FROM Pms_tbl_Addtask AS task CROSS APPLY STRING_SPLIT(task.team, ',') as Split inner join tbl_employee on Split.value = tbl_employee.id where task.status = 1 and task.archive = 0 and task.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and task.id = '" + tas_kid + "' ";
        DataSet dsN = da1.GetDataSetInline(strsql);

        if (dsN.Tables[0].Rows.Count > 0)
        {

            for (int i1 = 0; i1 < dsN.Tables[0].Rows.Count; i1++)
            {
                if (dataN != string.Empty)
                {
                    dataN += ",{";
                    dataN += "\"id\": \"" + dsN.Tables[0].Rows[i1]["id"].ToString() + "\",\"";
                    dataN += "text\": \"" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "\"";
                    dataN += "}";
                }
                else
                {
                    dataN += "{";
                    dataN += "\"id\": \"" + dsN.Tables[0].Rows[i1]["id"].ToString() + "\",\"";
                    dataN += "text\": \"" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "\"";
                    dataN += "}";
                }
            }

        }

        return dataN;

    }

    [WebMethod]
    public static string bindMultiChangeOwner_new(string mile_stoneid = "")
    {
        DataAccess da1 = new DataAccess();
        string dataN = "";
        string strsql = "SELECT  Split.value as id,tbl_employee.DisplayName FROM Pms_tbl_AddMilestone AS milestone CROSS APPLY STRING_SPLIT(milestone.Owner, ',') as Split inner join tbl_employee on Split.value = tbl_employee.id where milestone.status = 1 and milestone.archive = 0 and milestone.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and milestone.id = '" + mile_stoneid + "' ";
        DataSet dsN = da1.GetDataSetInline(strsql);

        if (dsN.Tables[0].Rows.Count > 0)
        {

            for (int i1 = 0; i1 < dsN.Tables[0].Rows.Count; i1++)
            {
                if (dataN != string.Empty)
                {
                    dataN += ",{";
                    dataN += "\"id\": \"" + dsN.Tables[0].Rows[i1]["id"].ToString() + "\",\"";
                    dataN += "text\": \"" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "\"";
                    dataN += "}";
                }
                else
                {
                    dataN += "{";
                    dataN += "\"id\": \"" + dsN.Tables[0].Rows[i1]["id"].ToString() + "\",\"";
                    dataN += "text\": \"" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "\"";
                    dataN += "}";
                }
            }

        }

        return dataN;

    }

    [WebMethod]
    public static string bindMultiChangeOwnersubtask_new(string tsk_id = "")
    {
        DataAccess da1 = new DataAccess();
        string dataN = "";
        string strsql = "SELECT  Split.value as id,tbl_employee.DisplayName FROM Pms_tbl_Addtask AS task CROSS APPLY STRING_SPLIT(task.Owner, ',') as Split inner join tbl_employee on Split.value = tbl_employee.id where task.status = 1 and task.archive = 0 and task.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and task.id = '" + tsk_id + "' ";
        DataSet dsN = da1.GetDataSetInline(strsql);

        if (dsN.Tables[0].Rows.Count > 0)
        {

            for (int i1 = 0; i1 < dsN.Tables[0].Rows.Count; i1++)
            {
                if (dataN != string.Empty)
                {
                    dataN += ",{";
                    dataN += "\"id\": \"" + dsN.Tables[0].Rows[i1]["id"].ToString() + "\",\"";
                    dataN += "text\": \"" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "\"";
                    dataN += "}";
                }
                else
                {
                    dataN += "{";
                    dataN += "\"id\": \"" + dsN.Tables[0].Rows[i1]["id"].ToString() + "\",\"";
                    dataN += "text\": \"" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "\"";
                    dataN += "}";
                }
            }

        }

        return dataN;

    }

    [WebMethod]
    public static string bindMultiTeam_new(string mile_stoneid = "")
    {
        DataAccess da1 = new DataAccess();
        string dataN = "";
        string strsql = "SELECT  Split.value as id,tbl_employee.DisplayName FROM Pms_tbl_AddMilestone AS milestone CROSS APPLY STRING_SPLIT(milestone.team, ',') as Split inner join tbl_employee on Split.value = tbl_employee.id where milestone.status = 1 and milestone.archive = 0 and milestone.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and milestone.id = '" + mile_stoneid + "' ";
        DataSet dsN = da1.GetDataSetInline(strsql);

        if (dsN.Tables[0].Rows.Count > 0)
        {

            for (int i1 = 0; i1 < dsN.Tables[0].Rows.Count; i1++)
            {
                if (dataN != string.Empty)
                {
                    dataN += ",{";
                    dataN += "\"id\": \"" + dsN.Tables[0].Rows[i1]["id"].ToString() + "\",\"";
                    dataN += "text\": \"" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "\"";
                    dataN += "}";
                }
                else
                {
                    dataN += "{";
                    dataN += "\"id\": \"" + dsN.Tables[0].Rows[i1]["id"].ToString() + "\",\"";
                    dataN += "text\": \"" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "\"";
                    dataN += "}";
                }
            }

        }

        return dataN;

    }

    [WebMethod]
    public static string bindMultiChangeTeam_new(string mile_stoneid = "")
    {
        DataAccess da1 = new DataAccess();
        string dataN = "";
        string strsql = "SELECT  Split.value as id,tbl_employee.DisplayName FROM Pms_tbl_AddMilestone AS milestone CROSS APPLY STRING_SPLIT(milestone.team, ',') as Split inner join tbl_employee on Split.value = tbl_employee.id where milestone.status = 1 and milestone.archive = 0 and milestone.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and milestone.id = '" + mile_stoneid + "' ";
        DataSet dsN = da1.GetDataSetInline(strsql);

        if (dsN.Tables[0].Rows.Count > 0)
        {

            for (int i1 = 0; i1 < dsN.Tables[0].Rows.Count; i1++)
            {
                if (dataN != string.Empty)
                {
                    dataN += ",{";
                    dataN += "\"id\": \"" + dsN.Tables[0].Rows[i1]["id"].ToString() + "\",\"";
                    dataN += "text\": \"" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "\"";
                    dataN += "}";
                }
                else
                {
                    dataN += "{";
                    dataN += "\"id\": \"" + dsN.Tables[0].Rows[i1]["id"].ToString() + "\",\"";
                    dataN += "text\": \"" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "\"";
                    dataN += "}";
                }
            }

        }

        return dataN;

    }

    [WebMethod]
    public static string bindMultiChangeOwnerMile_new(string projectid = "")
    {
        DataAccess da1 = new DataAccess();
        string dataN = "";
        string strsql = "SELECT  Split.value as id,tbl_employee.DisplayName FROM Pms_tbl_AddProject AS project CROSS APPLY STRING_SPLIT(project.Owner, ',') as Split inner join tbl_employee on Split.value = tbl_employee.id where project.status = 1 and project.archive = 0 and project.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and project.id = '" + projectid + "' ";
        DataSet dsN = da1.GetDataSetInline(strsql);

        if (dsN.Tables[0].Rows.Count > 0)
        {

            for (int i1 = 0; i1 < dsN.Tables[0].Rows.Count; i1++)
            {
                if (dataN != string.Empty)
                {
                    dataN += ",{";
                    dataN += "\"id\": \"" + dsN.Tables[0].Rows[i1]["id"].ToString() + "\",\"";
                    dataN += "text\": \"" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "\"";
                    dataN += "}";
                }
                else
                {
                    dataN += "{";
                    dataN += "\"id\": \"" + dsN.Tables[0].Rows[i1]["id"].ToString() + "\",\"";
                    dataN += "text\": \"" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "\"";
                    dataN += "}";
                }
            }

        }

        return dataN;

    }

    [WebMethod]
    public static string bindMultiChangeTeamMile_new(string projectid = "")
    {
        DataAccess da1 = new DataAccess();
        string dataN = "";
        string strsql = "SELECT  Split.value as id,tbl_employee.DisplayName FROM Pms_tbl_AddProject AS project CROSS APPLY STRING_SPLIT(project.team, ',') as Split inner join tbl_employee on Split.value = tbl_employee.id where project.status = 1 and project.archive = 0 and project.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and project.id = '" + projectid + "' ";
        DataSet dsN = da1.GetDataSetInline(strsql);

        if (dsN.Tables[0].Rows.Count > 0)
        {

            for (int i1 = 0; i1 < dsN.Tables[0].Rows.Count; i1++)
            {
                if (dataN != string.Empty)
                {
                    dataN += ",{";
                    dataN += "\"id\": \"" + dsN.Tables[0].Rows[i1]["id"].ToString() + "\",\"";
                    dataN += "text\": \"" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "\"";
                    dataN += "}";
                }
                else
                {
                    dataN += "{";
                    dataN += "\"id\": \"" + dsN.Tables[0].Rows[i1]["id"].ToString() + "\",\"";
                    dataN += "text\": \"" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "\"";
                    dataN += "}";
                }
            }

        }

        return dataN;

    }


    [WebMethod]
    public static string bindMultiOwner_new_mile(string RecordId = "")
    {
        DataAccess da1 = new DataAccess();
        string dataN = "";
        string strsql = "SELECT  Split.value as id,tbl_employee.DisplayName FROM Pms_tbl_AddProject AS project CROSS APPLY STRING_SPLIT(project.Owner, ',') as Split inner join tbl_employee on Split.value = tbl_employee.id where project.status = 1 and project.archive = 0 and project.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and project.id = '" + RecordId + "' ";
        DataSet dsN = da1.GetDataSetInline(strsql);

        if (dsN.Tables[0].Rows.Count > 0)
        {

            for (int i1 = 0; i1 < dsN.Tables[0].Rows.Count; i1++)
            {
                if (dataN != string.Empty)
                {
                    dataN += ",{";
                    dataN += "\"id\": \"" + dsN.Tables[0].Rows[i1]["id"].ToString() + "\",\"";
                    dataN += "text\": \"" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "\"";
                    dataN += "}";
                }
                else
                {
                    dataN += "{";
                    dataN += "\"id\": \"" + dsN.Tables[0].Rows[i1]["id"].ToString() + "\",\"";
                    dataN += "text\": \"" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "\"";
                    dataN += "}";
                }
            }

        }

        return dataN;

    }

    //[WebMethod]
    //public static string bindtaskdataP(string RecordId = "0")
    //{

    //    function fns = new function();
    //    DataAccess da = new DataAccess();
    //    CommonFunction cmf = new CommonFunction();
    //    string setmilstone = "";

    //    string strproject = "select * from Pms_tbl_AddProject where id='" + RecordId + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
    //    DataSet dsproject = da.GetDataSetInline(strproject);

    //    if (dsproject.Tables[0].Rows.Count > 0)
    //    {


    //        for (int P = 0; P < dsproject.Tables[0].Rows.Count; P++)
    //        {
    //            string strsmilstone = "select * from Pms_tbl_AddMilestone where ProjectId='" + dsproject.Tables[0].Rows[P]["id"].ToString() + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
    //            DataSet ds1 = da.GetDataSetInline(strsmilstone);
    //            if (ds1.Tables[0].Rows.Count > 0)
    //            {

    //                for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
    //                {

    //                    setmilstone += @" $(function() {$('#showtrM_" + ds1.Tables[0].Rows[j]["id"].ToString() + "').on('click', function() { $('.taskshow_" + ds1.Tables[0].Rows[j]["id"].ToString() + "').toggle();  });});";

    //                    setmilstone += @" $('#showtrM_" + ds1.Tables[0].Rows[j]["id"].ToString() + "').click(function() { $(this).toggleClass('shown');});";

    //                    #region task add
    //                    string strstask = "select * from Pms_tbl_Addtask where   ProjectId='" + dsproject.Tables[0].Rows[P]["id"].ToString() + "' and Milestoneid='" + ds1.Tables[0].Rows[j]["id"].ToString() + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
    //                    DataSet ds = da.GetDataSetInline(strstask);
    //                    if (ds.Tables[0].Rows.Count > 0)
    //                    {

    //                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //                        {


    //                            setmilstone += @" $(function() {$('#showtasktr_" + ds.Tables[0].Rows[i]["id"].ToString() + "').on('click', function() {$('.subtaskshow_" + ds.Tables[0].Rows[i]["id"].ToString() + "').toggle();  });});";
    //                            setmilstone += @" $('#showtasktr_" + ds.Tables[0].Rows[i]["id"].ToString() + "').click(function() { $(this).toggleClass('shown');});";


    //                        }




    //                    }

    //                    #endregion

    //                }


    //            }

    //        }



    //    }



    //    return setmilstone;

    //}


    [WebMethod]
    public static string bindMultiTeam_new_mile(string RecordId = "")
    {
        DataAccess da1 = new DataAccess();
        string dataN = "";
        string strsql = "SELECT  Split.value as id,tbl_employee.DisplayName FROM Pms_tbl_AddProject AS project CROSS APPLY STRING_SPLIT(project.team, ',') as Split inner join tbl_employee on Split.value = tbl_employee.id where project.status = 1 and project.archive = 0 and project.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and project.id = '" + RecordId + "' ";
        DataSet dsN = da1.GetDataSetInline(strsql);

        if (dsN.Tables[0].Rows.Count > 0)
        {

            for (int i1 = 0; i1 < dsN.Tables[0].Rows.Count; i1++)
            {
                if (dataN != string.Empty)
                {
                    dataN += ",{";
                    dataN += "\"id\": \"" + dsN.Tables[0].Rows[i1]["id"].ToString() + "\",\"";
                    dataN += "text\": \"" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "\"";
                    dataN += "}";
                }
                else
                {
                    dataN += "{";
                    dataN += "\"id\": \"" + dsN.Tables[0].Rows[i1]["id"].ToString() + "\",\"";
                    dataN += "text\": \"" + dsN.Tables[0].Rows[i1]["DisplayName"].ToString().Replace("'", "`") + "\"";
                    dataN += "}";
                }
            }

        }

        return dataN;

    }

    [WebMethod]
    public static string Bindcheckweitagedetails_mile(int RecordId = 0)
    {
        DataAccess da = new DataAccess();
        DataSet ds;
        string strsql = "select * from Pms_tbl_AddProject where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
        ds = da.GetDataSetInline(strsql);

        return ds.GetXml();
    }


    [WebMethod]
    public static string UpdateProject_Status(string projectid = "", string statusid = "", string iscomplete = "")
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (projectid != "" && projectid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[8];
        prm[0] = new SqlParameter("@id", projectid);
        prm[1] = new SqlParameter("@ProjectStatus", statusid);
        prm[2] = new SqlParameter("@iscomplete", iscomplete);
        prm[3] = new SqlParameter("@iscompletedate", da1.getcdatetime());
        prm[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[6] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[7] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pmstbl_Status_DeatilsUpdate_usp", prm);


        // percentage update start  // 
        if (iscomplete == "1")
        {
            decimal projecttotalpercent = 100;
            string strstatusupdate = "update Pms_tbl_AddProject set iscompletepercentage='" + projecttotalpercent.ToString() + "' where id='" + projectid + "' and status=1 and archive=0 and isnull(iscomplete,0)=1";
            DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

        }
        else
        {

            decimal projecttotalpercent = 0;
            string strstatusupdate = "update Pms_tbl_AddProject set iscompletepercentage='" + projecttotalpercent.ToString() + "' where id='" + projectid + "' and status=1 and archive=0 and isnull(iscomplete,0)=0";
            DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

        }
        // percentage update end  // 



        //project  log details insert start
        SqlParameter[] prmlog = new SqlParameter[8];
        prmlog[0] = new SqlParameter("@Transid", projectid);
        prmlog[1] = new SqlParameter("@Logstatus", statusid);
        prmlog[2] = new SqlParameter("@LogDate", da1.getcdatetime());
        prmlog[3] = new SqlParameter("@Logtype", "project");
        prmlog[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prmlog[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prmlog[6] = new SqlParameter("@cdate", da1.getcdatetime());
        prmlog[7] = new SqlParameter("@type", "Insert");
        da1.executeDMLQuery("pmstbl_Loginsert_usp", prmlog);
        //project  log details insert end

        DataAccess da = new DataAccess();
        string strsql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus where  status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + statusid + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            retval = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
        }
        // return dscode.GetXml();
        return retval;
    }

    [WebMethod]
    public static string Bindmilestonedetail_Team(int RecordId = 0)
    {

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "select * from Pms_tbl_AddMilestone where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
        ds = da.GetDataSetInline(strsql);


        return ds.GetXml();
    }

    [WebMethod]
    public static string Bindmilestonedetail(int RecordId = 0)
    {
        DataAccess da = new DataAccess();
        DataSet ds;
        string strsql = "select * from Pms_tbl_AddMilestone where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
        ds = da.GetDataSetInline(strsql);

        return ds.GetXml();
    }



    [WebMethod]
    public static string bindProjecttimelinedata(string projec_id = "0")
    {

        string projectdata = "";
        function fns = new function();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();

        //projectdata += "<table cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm datatable'>";
        projectdata += "<table cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm mb-0'>";
        projectdata += "<thead>";
        projectdata += "<tr>";
        projectdata += "<th style=min-width:140px;>Project Timeline</th>";
        projectdata += "</tr>";
        projectdata += "</thead>";
        projectdata += "<tbody>";

        string project_timeline = "";
        string Project_Timeline = "select * from Pms_tbl_AddProject where status=1 and archive=0  and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id = '" + projec_id + "'  ";
        DataSet ds_Project_Timeline = da.GetDataSetInline(Project_Timeline);
        if (ds_Project_Timeline.Tables[0].Rows.Count > 0)
        {
            for (int pt = 0; pt < ds_Project_Timeline.Tables[0].Rows.Count; pt++)
            {

                project_timeline += "<a href = 'javascript:void(0);' class='timesheet-wrap'>";
                project_timeline += "<div class='progress progress-xl bg-dark position-relative me-1' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + ds_Project_Timeline.Tables[0].Rows[pt]["ProjectName"].ToString() + "'>";
                project_timeline += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 100%;' aria-valuemax='100'>" + cmf.dateConvert(ds_Project_Timeline.Tables[0].Rows[pt]["StartDate"].ToString()) + " - " + cmf.dateConvert(ds_Project_Timeline.Tables[0].Rows[pt]["TargetDate"].ToString()) + "</div>";
                project_timeline += "</div>";
                project_timeline += "</a>";

            }

        }


        projectdata += "<tr>";
        //Project Timeline
        projectdata += "<td valign='top'>";
        projectdata += "<div class='d-flex'>";
        projectdata += "<div class='flex-grow-1 align-self-center me-2'>";
        projectdata += "<div class='extended_time'>";
        projectdata += "" + project_timeline + "";
        projectdata += "</div>";
        projectdata += "</div>";
        projectdata += "</div>";
        projectdata += "</td>";

        projectdata += "</tr>";


        projectdata += "</tbody>";
        projectdata += "</table>";

        return projectdata;
    }


    [WebMethod]
    public static string DeleteMilestonedata(int RecordId = 0)
    {
        string retval = "0";

        DataAccess da = new DataAccess();
        DataSet ds;
        string strsql = "";




        string findprojctid = "select * from Pms_tbl_AddMilestone where id=" + RecordId + " and status=1  and archive=0";
        DataSet dsprojctid = da.GetDataSetInline(findprojctid);

        int checkweitage = 0;
        decimal wetagecount = 0;
        decimal totalwetage = 0;
        string strweightage = "select isnull(checkprojectWeightage ,0) as checkprojectWeightage  from Pms_tbl_AddProject where id='" + dsprojctid.Tables[0].Rows[0]["ProjectId"] + "' and  status=1 and  isnull(checkprojectWeightage ,0)=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
        DataSet dsweitagecheck = da.GetDataSetInline(strweightage);
        if (dsweitagecheck.Tables[0].Rows.Count > 0)
        {

            checkweitage = Convert.ToInt32(dsweitagecheck.Tables[0].Rows[0]["checkprojectWeightage"]);

        }

        if (checkweitage == 1)
        {
            string strweightagecount = "select count (*) as WeitageCount from Pms_tbl_AddMilestone where ProjectId='" + dsprojctid.Tables[0].Rows[0]["ProjectId"] + "' and  status=1   and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
            DataSet dsweitagecount = da.GetDataSetInline(strweightagecount);
            if (dsweitagecount.Tables[0].Rows.Count > 0)
            {

                wetagecount = Convert.ToInt32(dsweitagecount.Tables[0].Rows[0]["WeitageCount"]);

            }

            totalwetage = 100 / wetagecount;
            string strstatusupdate = "update Pms_tbl_AddMilestone set Weightage='" + totalwetage.ToString() + "' where ProjectId='" + dsprojctid.Tables[0].Rows[0]["ProjectId"] + "' and status=1  and archive=0";
            DataSet dshourmin = da.GetDataSetInline(strstatusupdate);

        }


        int iscomplete = 0;
        string findsqlismile = "select isnull(iscomplete,0) as iscomplete, * from Pms_tbl_AddMilestone where id=" + RecordId + " and status=1 and archive=0";
        DataSet dsfindidismile = da.GetDataSetInline(findsqlismile);
        if (dsfindidismile.Tables[0].Rows.Count > 0)
        {

            iscomplete = Convert.ToInt32(dsfindidismile.Tables[0].Rows[0]["iscomplete"]);

        }



        if (iscomplete == 1)
        {
            decimal milestonetotalpercent = 100;
            decimal totlweitagemile = 0;
            string finsql1 = "select * from Pms_tbl_AddMilestone where id=" + RecordId + " and status=1  and archive=0";
            DataSet dsfindid1 = da.GetDataSetInline(finsql1);

            // Milstone percentage //
            string strstatusupdate = "update Pms_tbl_AddMilestone set iscompletepercentage='" + milestonetotalpercent.ToString() + "' where id='" + RecordId + "' and isnull(iscomplete,0)=1 and status=1 and archive=0";
            DataSet dshourmin = da.GetDataSetInline(strstatusupdate);

            // project percentage //
            string finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1  and archive=0";
            DataSet dsfindid2 = da.GetDataSetInline(finsql2);
            for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
            {
                decimal sumweitage = 0;
                decimal subpercentageemile = 0;
                sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
                subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                totlweitagemile += (sumweitage * subpercentageemile) / 100;

            }

            string finsql4 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitagemile.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da.GetDataSetInline(finsql4);






        }
        else
        {
            string finsql2 = "";
            DataSet dsfindid2;
            decimal milestonetotalpercent = 0;
            decimal totlweitagemile = 0;
            decimal totlweitageproject = 0;
            string finsql1 = "select * from Pms_tbl_AddMilestone where id=" + RecordId + " and status=1 and archive=0";
            DataSet dsfindid1 = da.GetDataSetInline(finsql1);
            if (dsfindid1.Tables[0].Rows.Count > 0)
            {
                if (dsfindid1.Tables[0].Rows[0]["istask"].ToString() == "1")
                {



                    finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where  Milestoneid='" + RecordId + "'  and status=1  and archive=0";
                    dsfindid2 = da.GetDataSetInline(finsql2);
                    for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
                    {
                        decimal sumweitage = 0;
                        decimal subpercentageemile = 0;
                        sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
                        subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                        totlweitagemile += (sumweitage * subpercentageemile) / 100;

                    }
                }

            }






            // Milestone percentage //
            string strstatusupdate = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile.ToString() + "' where id='" + RecordId + "' and status=1 and archive=0 and isnull(iscomplete,0)=0";
            DataSet dshourmin = da.GetDataSetInline(strstatusupdate);

            // project percentage //
            finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1  and archive=0";
            dsfindid2 = da.GetDataSetInline(finsql2);
            for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
            {
                decimal sumweitage = 0;
                decimal subpercentageemile = 0;
                sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
                subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                totlweitageproject += (sumweitage * subpercentageemile) / 100;

            }

            string finsql4 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da.GetDataSetInline(finsql4);



        }


        strsql = "update  Pms_tbl_AddMilestone set  status=0,archive=1  where  id='" + RecordId + "' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        ds = da.GetDataSetInline(strsql);


        return retval;
    }

    [WebMethod]
    public static string bindMiletimelinedata_mile(string projec_id = "0")
    {

        string projectdata = "";
        function fns = new function();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();




        string project_timeline = "";
        string strret = "";
        string Project_Timeline = "select   * from Pms_tbl_AddMilestone where status=1 and archive=0  and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and ProjectId = '" + projec_id + "'  ";
        DataSet ds_Project_Timeline = da.GetDataSetInline(Project_Timeline);
        if (ds_Project_Timeline.Tables[0].Rows.Count > 0)
        {
            //projectdata += "<table cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm datatable'>";
            projectdata += "<table cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm mb-0'>";
            projectdata += "<thead>";
            projectdata += "<tr>";
            projectdata += "<th style=min-width:140px;>Milestone Timeline</th>";
            projectdata += "</tr>";
            projectdata += "</thead>";
            projectdata += "<tbody>";

            for (int pt = 0; pt < ds_Project_Timeline.Tables[0].Rows.Count; pt++)
            {


                strret = "M" + " " + (pt + 1);
                project_timeline += "<a href = 'javascript:void(0);' class='timesheet-wrap'>";
                project_timeline += "<div class='progress progress-xl bg-dark position-relative me-1' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title=' " + ds_Project_Timeline.Tables[0].Rows[pt]["Milestone"].ToString() + " " + cmf.dateConvert(ds_Project_Timeline.Tables[0].Rows[pt]["StartDate"].ToString()) + " - " + cmf.dateConvert(ds_Project_Timeline.Tables[0].Rows[pt]["EndDate"].ToString()) + " '>";
                // project_timeline += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 100%;' aria-valuemax='100'>" + ds_Project_Timeline.Tables[0].Rows[pt]["Milestone"].ToString() + "</div>";
                project_timeline += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 100%;' aria-valuemax='100'>" + strret + "</div>";
                project_timeline += "</div>";
                project_timeline += "</a>";

            }

        }


        projectdata += "<tr>";
        //Milestone Timeline
        projectdata += "<td valign='top'>";
        projectdata += "<div class='d-flex'>";
        projectdata += "<div class='flex-grow-1 align-self-center me-2'>";
        projectdata += "<div class='extended_time'>";
        projectdata += "" + project_timeline + "";
        projectdata += "</div>";
        projectdata += "</div>";
        projectdata += "</div>";
        projectdata += "</td>";

        projectdata += "</tr>";


        projectdata += "</tbody>";
        projectdata += "</table>";

        return projectdata;
    }

    [WebMethod]
    public static string Bindcheckstrtdateprojectdetails(int projec_id = 0)
    {

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "select * from Pms_tbl_AddProject where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + projec_id + "'";
        ds = da.GetDataSetInline(strsql);


        return ds.GetXml();
    }

    [WebMethod]
    public static string AddAddMilestonetatus(string value = "", string statuscolor = "")
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
    public static string AddMilestonePrioritytype(string value = "", string prioritycolor = "")
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
    public static string SaveMileProject_Team(string mileid = "", string mileTeamdropdownid = "")
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string editid = mileid;
        string entrytype = "Update";
        if (editid != "" && editid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", editid);
        prm[1] = new SqlParameter("@team", mileTeamdropdownid);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pms_tbl_TeamDeatils_MilestoneUpdate", prm);

        DataAccess da = new DataAccess();
        string strsql = "select * from Pms_tbl_AddMilestone where  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and  id='" + editid + "' ";
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
    public static string Bindtaskdetail_extendatedate(int RecordId = 0)
    {

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "select * from Pms_tbl_Addtask where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
        ds = da.GetDataSetInline(strsql);


        return ds.GetXml();
    }


    [WebMethod]
    public static string Bindprojectdetail_Mileextendatedate(int RecordId = 0)
    {

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "select * from Pms_tbl_AddMilestone where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
        ds = da.GetDataSetInline(strsql);


        return ds.GetXml();
    }

    [WebMethod]
    public static string MileUpdateExtendeddatedtls(string Milestoneid = "", string mileextendStrtdate = "", string mileextendEnddate = "")
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
        if (Milestoneid != "" && Milestoneid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[7];
        prm[0] = new SqlParameter("@id", Milestoneid);
        prm[1] = new SqlParameter("@StartDate", mileextendStrtdate);
        prm[2] = new SqlParameter("@TargetDate", mileextendEnddate);
        prm[3] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[4] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[5] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[6] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pmstbl_Milestone_Extnddate_DeatilsUpdate", prm);


        // extended timeleine 


        string strextend = "select id,extendStartdate as StartDate,extendEnddate as TargetDate from Pms_Tbl_Extended_Timeline where  status=1 and archive=0 and Type='Milestone' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and MilestoneID = '" + Milestoneid + "'  ";
        DataSet dsstrextend = da1.GetDataSetInline(strextend);
        if (dsstrextend.Tables[0].Rows.Count > 0)
        {
            for (int k = 0; k < dsstrextend.Tables[0].Rows.Count; k++)
            {
                DateTime estendStartDate = Convert.ToDateTime(dsstrextend.Tables[0].Rows[k]["StartDate"].ToString() == "" ? DateTime.Now.ToString() : dsstrextend.Tables[0].Rows[k]["StartDate"].ToString());
                DateTime estendTargetdate = Convert.ToDateTime(dsstrextend.Tables[0].Rows[k]["TargetDate"].ToString() == "" ? DateTime.Now.ToString() : dsstrextend.Tables[0].Rows[k]["TargetDate"].ToString());
                TimeSpan estendobjTimeSpan = estendTargetdate - estendStartDate;
                estendDays = Convert.ToDouble(estendobjTimeSpan.TotalDays);
                estendDays = estendDays + 1;


                DateTime estendEnddate = Convert.ToDateTime(dsstrextend.Tables[0].Rows[k]["Targetdate"].ToString() == "" ? DateTime.Now.ToString() : dsstrextend.Tables[0].Rows[k]["Targetdate"].ToString());
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
    public static string UpdateExtendeddatedtls_task(string task_id = "", string extendStrtdate_task = "", string extendEnddate_task = "")
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
        if (task_id != "" && task_id != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[7];
        prm[0] = new SqlParameter("@id", task_id);
        prm[1] = new SqlParameter("@StartDate", extendStrtdate_task);
        prm[2] = new SqlParameter("@TargetDate", extendEnddate_task);
        prm[3] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[4] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[5] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[6] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pmstbl_ExtnddatedateUpdate_task", prm);



        // extended timeleine 


        string strextend = "select id,extendStartdate as StartDate,extendEnddate as TargetDate from Pms_Tbl_Extended_Timeline where  status=1 and archive=0 and Type='task' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and taskid = '" + task_id + "'  ";
        DataSet dsstrextend = da1.GetDataSetInline(strextend);
        if (dsstrextend.Tables[0].Rows.Count > 0)
        {
            for (int k = 0; k < dsstrextend.Tables[0].Rows.Count; k++)
            {
                DateTime estendStartDate = Convert.ToDateTime(dsstrextend.Tables[0].Rows[k]["StartDate"].ToString() == "" ? DateTime.Now.ToString() : dsstrextend.Tables[0].Rows[k]["StartDate"].ToString());
                DateTime estendTargetdate = Convert.ToDateTime(dsstrextend.Tables[0].Rows[k]["TargetDate"].ToString() == "" ? DateTime.Now.ToString() : dsstrextend.Tables[0].Rows[k]["TargetDate"].ToString());
                TimeSpan estendobjTimeSpan = estendTargetdate - estendStartDate;
                estendDays = Convert.ToDouble(estendobjTimeSpan.TotalDays);
                estendDays = estendDays + 1;


                DateTime estendEnddate = Convert.ToDateTime(dsstrextend.Tables[0].Rows[k]["Targetdate"].ToString() == "" ? DateTime.Now.ToString() : dsstrextend.Tables[0].Rows[k]["Targetdate"].ToString());
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
    public static string bindMiletimelinedata(string mile_id = "0")
    {

        string projectdata = "";
        function fns = new function();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();

        //projectdata += "<table cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm datatable'>";
        projectdata += "<table cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm mb-0'>";
        projectdata += "<thead>";
        projectdata += "<tr>";
        projectdata += "<th style=min-width:140px;>Milestone Timeline</th>";
        projectdata += "</tr>";
        projectdata += "</thead>";
        projectdata += "<tbody>";

        string project_timeline = "";
        string Project_Timeline = "select * from Pms_tbl_AddMilestone where status=1 and archive=0  and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id = '" + mile_id + "'  ";
        DataSet ds_Project_Timeline = da.GetDataSetInline(Project_Timeline);
        if (ds_Project_Timeline.Tables[0].Rows.Count > 0)
        {
            for (int pt = 0; pt < ds_Project_Timeline.Tables[0].Rows.Count; pt++)
            {

                project_timeline += "<a href = 'javascript:void(0);' class='timesheet-wrap'>";
                // project_timeline += "<div class='progress progress-xl bg-dark position-relative me-1' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + cmf.dateConvert(ds_Project_Timeline.Tables[0].Rows[pt]["StartDate"].ToString()) + " - " + cmf.dateConvert(ds_Project_Timeline.Tables[0].Rows[pt]["EndDate"].ToString()) + " '>";
                project_timeline += "<div class='progress progress-xl bg-dark position-relative me-1' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + ds_Project_Timeline.Tables[0].Rows[pt]["Milestone"].ToString() + " '>";
                // project_timeline += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 100%;' aria-valuemax='100'>" + ds_Project_Timeline.Tables[0].Rows[pt]["Milestone"].ToString() + "</div>";
                project_timeline += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 100%;' aria-valuemax='100'>" + cmf.dateConvert(ds_Project_Timeline.Tables[0].Rows[pt]["StartDate"].ToString()) + " - " + cmf.dateConvert(ds_Project_Timeline.Tables[0].Rows[pt]["EndDate"].ToString()) + "</div>";
                project_timeline += "</div>";
                project_timeline += "</a>";

            }

        }


        projectdata += "<tr>";
        //Milestone Timeline
        projectdata += "<td valign='top'>";
        projectdata += "<div class='d-flex'>";
        projectdata += "<div class='flex-grow-1 align-self-center me-2'>";
        projectdata += "<div class='extended_time'>";
        projectdata += "" + project_timeline + "";
        projectdata += "</div>";
        projectdata += "</div>";
        projectdata += "</div>";
        projectdata += "</td>";

        projectdata += "</tr>";


        projectdata += "</tbody>";
        projectdata += "</table>";

        return projectdata;
    }


    [WebMethod]
    public static string bindtasktimelinedata(string mile_id = "0")
    {

        string projectdata = "";
        function fns = new function();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();

      

        string project_timeline = "";
        string strret = "";
        string Project_Timeline = "select  * from Pms_tbl_Addtask where status=1 and archive=0  and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and Milestoneid = '" + mile_id + "'  ";
        DataSet ds_Project_Timeline = da.GetDataSetInline(Project_Timeline);
        if (ds_Project_Timeline.Tables[0].Rows.Count > 0)
        {
            //projectdata += "<table cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm datatable'>";
            projectdata += "<table cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm mb-0'>";
            projectdata += "<thead>";
            projectdata += "<tr>";
            projectdata += "<th style=min-width:140px;>Task Timeline</th>";
            projectdata += "</tr>";
            projectdata += "</thead>";
            projectdata += "<tbody>";

            for (int pt = 0; pt < ds_Project_Timeline.Tables[0].Rows.Count; pt++)
            {
                strret = "T" + " " + (pt + 1);
                project_timeline += "<a href = 'javascript:void(0);' class='timesheet-wrap'>";
                project_timeline += "<div class='progress progress-xl bg-dark position-relative me-1' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title=' " + ds_Project_Timeline.Tables[0].Rows[pt]["taskname"].ToString() + "  " + cmf.dateConvert(ds_Project_Timeline.Tables[0].Rows[pt]["Startdate"].ToString()) + " - " + cmf.dateConvert(ds_Project_Timeline.Tables[0].Rows[pt]["Targetdate"].ToString()) + " '>";
                project_timeline += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 100%;' aria-valuemax='100'>" + strret + "</div>";
                project_timeline += "</div>";
                project_timeline += "</a>";

            }

        }


        projectdata += "<tr>";
        //Project Timeline
        projectdata += "<td valign='top'>";
        projectdata += "<div class='d-flex'>";
        projectdata += "<div class='flex-grow-1 align-self-center me-2'>";
        projectdata += "<div class='extended_time'>";
        projectdata += "" + project_timeline + "";
        projectdata += "</div>";
        projectdata += "</div>";
        projectdata += "</div>";
        projectdata += "</td>";

        projectdata += "</tr>";


        projectdata += "</tbody>";
        projectdata += "</table>";

        return projectdata;
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
        prm[4] = new SqlParameter("@type", "multiproject");
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
    public static string bindtaskdata(string RecordId = "0")
    {
        string projretval = Projget_status_listbind();
        string retval = get_status_listbind();
        string mileretval = Mileget_status_listbind();
        string retstageval = get_Stage_listbind();
        string strvalprority = get_Priority_listbind();
        string milestrvalprority = Mileget_Priority_listbind();
        string projstrvalprority = Projget_Priority_listbind();

        string sbtskretval = sbtskget_status_listbind();
        string sbtskretstageval = sbtskget_Stage_listbind();
        string sbtskstrvalprority = sbtskget_Priority_listbind();


        string taskdata = "";
        function fns = new function();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();

        //SqlParameter[] prm = new SqlParameter[3];
        //prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        //prm[1] = new SqlParameter("@RecordId", RecordId);
        //prm[2] = new SqlParameter("@milestoneid", milstoneid);
        //DataSet ds = da.GetDataSet("Pms_Bindtask_dashbord", prm);

        string strproject = "select * from Pms_tbl_AddProject where id='" + RecordId + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dsproject = da.GetDataSetInline(strproject);


        int projistaskchk = 0;
        string sqlchkistskproj = "select isnull(istask,0) istask from Pms_tbl_AddMilestone where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and isnull(istask,0)=1  ";
        DataSet dschkistskproj = da.GetDataSetInline(sqlchkistskproj);
        if (dschkistskproj.Tables[0].Rows.Count > 0)
        {
            projistaskchk = 1;

        }


        taskdata += "<table style='width:100%;' cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm datatable'>";

        //taskdata += "<table id='tbDetails' cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm mb-0 datatable'>";

        taskdata += "<thead>";
        taskdata += "<tr>";

       
        taskdata += "<th style='display: none;'>S.No</th>";
        taskdata += "<th style='min-width:77px;'>Action</th>";
        //taskdata += "<th style='min-width:130px;'>Timer</th>";
        //taskdata += "<th style='min-width:130px;'>Timer History</th>";
        //taskdata += "<th style='min-width:180px;'>Project Name</th>";
        taskdata += "<th style='min-width:280px;'>Milestone</th>";
        //taskdata += "<th style='min-width:180px;'>Task Name</th>";
        //taskdata += "<th style='min-width:180px;'>Description</th>";
        //taskdata += "<th style='min-width:180px;'>Dependency</th>";
        taskdata += "<th style='min-width:180px;'>Completion Status</th>";
        //if (projistaskchk == 1)
        //{
        //    taskdata += "<th style='min-width:180px;'>Task</th>";
        //}
        taskdata += "<th>Weightage</th>";
        taskdata += "<th style='min-width:180px;'>Owner</th>";
        taskdata += "<th style='min-width:180px;'>Team</th>";
        taskdata += "<th style='min-width:180px;'>Timeline</th>";
        taskdata += "<th style='min-width:180px;'>Extended Timeline</th>";
        taskdata += "<th>Start Date</th>";
        taskdata += "<th>End Date</th>";
        taskdata += "<th style='min-width:180px;'>Status</th>";
        taskdata += "<th style='min-width:180px;'>Priority</th>";
        taskdata += "<th style='min-width:180px;'>Issues</th>";
        //taskdata += "<th>Budget</th>";
        //taskdata += "<th>Actual</th>";
        taskdata += "<th>Task Duration (Hour)</th>";
        taskdata += "<th style='min-width:180px;'>Stage</th>";
        //taskdata += "<th style='min-width:180px;'>Sub Task</th>";
        //taskdata += "<th style='text-align:right'>Work Hours (W)</th>";
        //taskdata += "<th style='text-align:right'>Timelog Total (T)</th>";
        //taskdata += "<th style='text-align:right'>Difference (W-T)</th>";
        taskdata += "</tr>";
        taskdata += "</thead>";
        taskdata += "<tbody>";
        if (dsproject.Tables[0].Rows.Count > 0)
        {


            for (int P = 0; P < dsproject.Tables[0].Rows.Count; P++)
            {

                //   Milestone count start //
                #region bind project
                float Milestonecomplite = 0;
                float Milestonepending = 0;
                float MilestoneprojectTotal = 0;
                float MilestonePhaseTotal = 0;
                float MilesstoneTaskTotal = 0;

                string str1 = "select COUNT(*) as countmilstone from Pms_tbl_AddMilestone where ProjectId='" + dsproject.Tables[0].Rows[P]["id"] + "' and status=1 and archive=0 ";
                DataSet dss1 = da.GetDataSetInline(str1);
                if (dss1.Tables[0].Rows.Count > 0)
                {

                    try
                    {
                        MilestonePhaseTotal = Convert.ToInt32(dss1.Tables[0].Rows[0]["countmilstone"]);
                    }
                    catch (Exception)
                    {
                    }
                }

                string str2 = "select * from Pms_tbl_AddMilestone where ProjectId='" + dsproject.Tables[0].Rows[P]["id"] + "' and status=1 and archive=0 ";
                DataSet dss2 = da.GetDataSetInline(str2);
                if (dss2.Tables[0].Rows.Count > 0)
                {

                    string findtask1 = "select COUNT(*) as counttask  from Pms_tbl_AddMilestone where ProjectId='" + dsproject.Tables[0].Rows[P]["id"] + "'  and P_Status in ( select id from Pms_tbl_Projectstatus where  status=1 and archive=0 and isnull(iscomplete,0)=1)  and status=1 and archive=0";
                    DataSet dsssumtask1 = da.GetDataSetInline(findtask1);

                    try
                    {
                        MilesstoneTaskTotal = Convert.ToInt32(dsssumtask1.Tables[0].Rows[0]["counttask"]);
                    }
                    catch (Exception)
                    {

                    }


                    MilestoneprojectTotal = (MilestonePhaseTotal - MilesstoneTaskTotal);


                }

                if (MilestoneprojectTotal > 0)
                {
                    Milestonecomplite = MilesstoneTaskTotal;
                    Milestonepending = Math.Abs(MilestonePhaseTotal - MilesstoneTaskTotal);
                }

                if (MilestoneprojectTotal == 0)
                {
                    Milestonecomplite = MilesstoneTaskTotal;
                    Milestonepending = Math.Abs(MilestonePhaseTotal - MilesstoneTaskTotal);
                }

                // Milestone count end //


                //   completion status start //

                decimal projectWattagecomplite = 0;
                decimal projectWattagepending = 100;
                decimal projectprojectTotal = 0;
                decimal projectPhaseTotal = 0;
                decimal projectTaskTotal = 0;


                string projectstr = "select cast(ROUND(isnull(iscompletepercentage,0),2) AS DECIMAL(10,2)) as iscompletepercentage from Pms_tbl_AddProject where id='" + dsproject.Tables[0].Rows[P]["id"] + "' and status=1 and archive=0 ";
                DataSet projectdss = da.GetDataSetInline(projectstr);


                try
                {
                    projectTaskTotal = Convert.ToDecimal(projectdss.Tables[0].Rows[0]["iscompletepercentage"].ToString());
                }
                catch (Exception)
                {
                }




                projectprojectTotal = projectTaskTotal;



                if (projectprojectTotal > 0)
                {
                    projectWattagecomplite = projectprojectTotal;
                    projectWattagepending = Math.Abs(projectWattagepending - projectWattagecomplite);
                }

                // completion status end //



                //   task count start //

                float projtskcomplite = 0;
                float projtskpending = 0;
                float projtskprojectTotal = 0;
                float projtskPhaseTotal = 0;
                float projtsk_TaskTotal = 0;

                string projstr11 = "select COUNT(*) as counttsk from Pms_tbl_Addtask where ProjectId='" + dsproject.Tables[0].Rows[P]["id"] + "' and status=1 and archive=0 ";
                DataSet projdss11 = da.GetDataSetInline(projstr11);
                if (projdss11.Tables[0].Rows.Count > 0)
                {

                    try
                    {
                        projtskPhaseTotal = Convert.ToInt32(projdss11.Tables[0].Rows[0]["counttsk"]);
                    }
                    catch (Exception)
                    {
                    }
                }

                string projstr22 = "select * from Pms_tbl_AddMilestone where ProjectId='" + dsproject.Tables[0].Rows[P]["id"] + "' and status=1 and archive=0 ";
                DataSet projdss22 = da.GetDataSetInline(projstr22);
                if (projdss22.Tables[0].Rows.Count > 0)
                {

                    string findtask11 = "select COUNT(*) as counttask  from Pms_tbl_Addtask where ProjectId='" + dsproject.Tables[0].Rows[P]["id"] + "'  and P_Status in ( select id from Pms_tbl_Projectstatus where  status=1 and archive=0 and isnull(iscomplete,0)=1)  and status=1 and archive=0";
                    DataSet dsssumtask11 = da.GetDataSetInline(findtask11);

                    try
                    {
                        projtsk_TaskTotal = Convert.ToInt32(dsssumtask11.Tables[0].Rows[0]["counttask"]);
                    }
                    catch (Exception)
                    {

                    }


                    projtskprojectTotal = (projtskPhaseTotal - projtsk_TaskTotal);


                }

                if (projtskprojectTotal > 0)
                {
                    projtskcomplite = projtsk_TaskTotal;
                    projtskpending = Math.Abs(projtskPhaseTotal - projtsk_TaskTotal);
                }

                if (projtskprojectTotal == 0)
                {
                    projtskcomplite = projtsk_TaskTotal;
                    projtskpending = Math.Abs(projtskPhaseTotal - projtsk_TaskTotal);
                }

                // task count end //




                //   Issues count start //

                float projIssuescomplite = 0;
                float projIssuespending = 0;
                float projIssuesrojectTotal = 0;
                float projIssuesPhaseTotal = 0;
                float projIssues_Total = 0;

                string projstr111 = "select COUNT(*) as countIssues from Pms_tbl_Addissues where ProjectId='" + dsproject.Tables[0].Rows[P]["id"] + "' and Issueon='Project' and status=1 and archive=0 ";
                DataSet projdss111 = da.GetDataSetInline(projstr111);
                if (projdss111.Tables[0].Rows.Count > 0)
                {

                    try
                    {
                        projIssuesPhaseTotal = Convert.ToInt32(projdss111.Tables[0].Rows[0]["countIssues"]);
                    }
                    catch (Exception)
                    {
                    }
                }


                string projfindtask111 = "select COUNT(*) as count_Issues from Pms_tbl_Addissues where ProjectId='" + dsproject.Tables[0].Rows[P]["id"] + "' and Issueon='Project' and IssueStatus in ( select id from Pms_tbl_Projectstatus where  status=1 and archive=0 and isnull(iscomplete,0)=1)  and status=1 and archive=0";
                DataSet projdsssumtask111 = da.GetDataSetInline(projfindtask111);
                if (projdsssumtask111.Tables[0].Rows.Count > 0)
                {
                    try
                    {
                        projIssues_Total = Convert.ToInt32(projdsssumtask111.Tables[0].Rows[0]["count_Issues"]);
                    }
                    catch (Exception)
                    {

                    }
                }


                projIssuesrojectTotal = (projIssuesPhaseTotal - projIssues_Total);


                if (projIssuesrojectTotal > 0)
                {
                    projIssuescomplite = projIssues_Total;
                    projIssuespending = Math.Abs(projIssuesPhaseTotal - projIssues_Total);
                }

                if (projIssuesrojectTotal == 0)
                {
                    projIssuescomplite = projIssues_Total;
                    projIssuespending = Math.Abs(projIssuesPhaseTotal - projIssues_Total);
                }

                // Issues count end //


                #endregion


                string strsmilstone = "select * from Pms_tbl_AddMilestone where ProjectId='" + dsproject.Tables[0].Rows[P]["id"].ToString() + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by isnull(orderno,0) ";
                SaveLogText("strsmilstone-" + strsmilstone);
                DataSet ds1 = da.GetDataSetInline(strsmilstone);
                if (ds1.Tables[0].Rows.Count > 0)
                {

                    for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                    {
                        #region milestone
                        string milecntownr = "0";
                        string mileusersdetailsowner = "";
                        string mileownertooltip = "";
                        if (ds1.Tables[0].Rows[j]["Owner"].ToString() != "")
                        {
                            string[] milearcnt = ds1.Tables[0].Rows[j]["Owner"].ToString().Trim().Split(',');
                            milecntownr = milearcnt.Length.ToString();
                            DataSet miledsusers = new DataSet();
                            if (ds1.Tables[0].Rows[j]["Owner"].ToString().Trim() != "")
                            {
                                miledsusers = da.GetDataSetInline("select id,DisplayName,fileupload1,SUBSTRING (DisplayName, 1, 2) as splinameone from tbl_employee where id in(" + ds1.Tables[0].Rows[j]["Owner"].ToString().Trim() + ")");
                                if (miledsusers.Tables[0].Rows.Count > 3)
                                {
                                    milecntownr = (Convert.ToInt32(milecntownr) - 3).ToString();

                                    // owner tool tip //
                                    for (int Ml = 3; Ml < miledsusers.Tables[0].Rows.Count; Ml++)
                                    {
                                        if (miledsusers.Tables[0].Rows[Ml]["DisplayName"].ToString() != string.Empty)
                                        {
                                            if (mileownertooltip != string.Empty)
                                            {
                                                mileownertooltip += "," + miledsusers.Tables[0].Rows[Ml]["DisplayName"].ToString();
                                            }
                                            else
                                            {
                                                mileownertooltip += miledsusers.Tables[0].Rows[Ml]["DisplayName"].ToString();
                                            }
                                        }

                                    }

                                    for (int mk = 0; mk < 3; mk++)
                                    {
                                        if (miledsusers.Tables[0].Rows[mk]["fileupload1"].ToString() != "")
                                        {
                                            mileusersdetailsowner += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title=" + miledsusers.Tables[0].Rows[mk]["DisplayName"].ToString() + " data-bs-original-title='" + miledsusers.Tables[0].Rows[mk]["DisplayName"].ToString() + "'><img src = '" + miledsusers.Tables[0].Rows[mk]["fileupload1"] + "' alt='' class='rounded-circle avatar-xxs'></a>";
                                        }
                                        else
                                        {
                                            mileusersdetailsowner += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover'" +
                         " data-bs-placement='top' title='" + miledsusers.Tables[0].Rows[mk]["DisplayName"].ToString() + "' data-bs-original-title='" + miledsusers.Tables[0].Rows[mk]["DisplayName"].ToString() + "'>";
                                            mileusersdetailsowner += @"<div class='avatar-xxs'><div class='avatar-title rounded-circle bg-light text-primary'>" + miledsusers.Tables[0].Rows[mk]["splinameone"].ToString() + "</div></div>";
                                            mileusersdetailsowner += @"</a>";
                                        }
                                    }

                                }
                                else
                                {
                                    milecntownr = "";
                                    for (int mk1 = 0; mk1 < miledsusers.Tables[0].Rows.Count; mk1++)
                                    {
                                        if (miledsusers.Tables[0].Rows[mk1]["fileupload1"].ToString() != "")
                                        {
                                            //img src='/assets/images/avatar-1.jpg'
                                            mileusersdetailsowner += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='' data-bs-original-title='" + miledsusers.Tables[0].Rows[mk1]["DisplayName"].ToString() + "'><img src = '" + miledsusers.Tables[0].Rows[mk1]["fileupload1"] + "' alt='' class='rounded-circle avatar-xxs'></a>";
                                        }
                                        else
                                        {
                                            mileusersdetailsowner += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover'  data-bs-placement='top' title='" + miledsusers.Tables[0].Rows[mk1]["DisplayName"].ToString() + "' data-bs-original-title='" + miledsusers.Tables[0].Rows[mk1]["DisplayName"].ToString() + "'>";
                                            mileusersdetailsowner += @"<div class='avatar-xxs'>
                            <div class='avatar-title rounded-circle bg-light text-primary'>" + miledsusers.Tables[0].Rows[mk1]["splinameone"].ToString() + "</div></div>";
                                            mileusersdetailsowner += @"</a>";

                                        }
                                    }
                                }

                            }
                        }


                        string milecnt = "0";
                        string mileusersdetails = "";
                        string mileteamtooltip = "";
                        if (ds1.Tables[0].Rows[j]["team"].ToString() != "")
                        {
                            string[] milearcnt = ds1.Tables[0].Rows[j]["team"].ToString().Trim().Split(',');
                            milecnt = milearcnt.Length.ToString();
                            DataSet mile1dsusers = new DataSet();
                            if (ds1.Tables[0].Rows[j]["team"].ToString().Trim() != "")
                            {
                                mile1dsusers = da.GetDataSetInline("select id,DisplayName,fileupload1,SUBSTRING (DisplayName, 1, 2) as splinameone from tbl_employee where id in(" + ds1.Tables[0].Rows[j]["team"].ToString().Trim() + ")");
                                if (mile1dsusers.Tables[0].Rows.Count > 3)
                                {
                                    milecnt = (Convert.ToInt32(milecnt) - 3).ToString();

                                    // team tool tip //
                                    for (int lt = 3; lt < mile1dsusers.Tables[0].Rows.Count; lt++)
                                    {
                                        if (mile1dsusers.Tables[0].Rows[lt]["DisplayName"].ToString() != string.Empty)
                                        {
                                            if (mileteamtooltip != string.Empty)
                                            {
                                                mileteamtooltip += "," + mile1dsusers.Tables[0].Rows[lt]["DisplayName"].ToString();
                                            }
                                            else
                                            {
                                                mileteamtooltip += mile1dsusers.Tables[0].Rows[lt]["DisplayName"].ToString();
                                            }
                                        }

                                    }

                                    for (int kt = 0; kt < 3; kt++)
                                    {
                                        if (mile1dsusers.Tables[0].Rows[kt]["fileupload1"].ToString() != "")
                                        {
                                            mileusersdetails += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title=" + mile1dsusers.Tables[0].Rows[kt]["DisplayName"].ToString() + " data-bs-original-title='" + mile1dsusers.Tables[0].Rows[kt]["DisplayName"].ToString() + "'><img src = '/assets/images/avatar-1.jpg' alt='' class='rounded-circle avatar-xxs'></a>";
                                        }
                                        else
                                        {
                                            mileusersdetails += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover'" +
                         " data-bs-placement='top' title='" + mile1dsusers.Tables[0].Rows[kt]["DisplayName"].ToString() + "' data-bs-original-title='" + mile1dsusers.Tables[0].Rows[kt]["DisplayName"].ToString() + "'>";
                                            mileusersdetails += @"<div class='avatar-xxs'><div class='avatar-title rounded-circle bg-light text-primary'>" + mile1dsusers.Tables[0].Rows[kt]["splinameone"].ToString() + "</div></div>";
                                            mileusersdetails += @"</a>";
                                        }
                                    }

                                }
                                else
                                {
                                    milecnt = "";
                                    for (int kt1 = 0; kt1 < mile1dsusers.Tables[0].Rows.Count; kt1++)
                                    {
                                        if (mile1dsusers.Tables[0].Rows[kt1]["fileupload1"].ToString() != "")
                                        {
                                            mileusersdetails += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='' data-bs-original-title='" + mile1dsusers.Tables[0].Rows[kt1]["DisplayName"].ToString() + "'><img src = '/assets/images/avatar-1.jpg' alt='' class='rounded-circle avatar-xxs'></a>";
                                        }
                                        else
                                        {
                                            mileusersdetails += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover'  data-bs-placement='top' title='" + mile1dsusers.Tables[0].Rows[kt1]["DisplayName"].ToString() + "' data-bs-original-title='" + mile1dsusers.Tables[0].Rows[kt1]["DisplayName"].ToString() + "'>";
                                            mileusersdetails += @"<div class='avatar-xxs'>
                            <div class='avatar-title rounded-circle bg-light text-primary'>" + mile1dsusers.Tables[0].Rows[kt1]["splinameone"].ToString() + "</div></div>";
                                            mileusersdetails += @"</a>";

                                        }
                                    }
                                }

                            }
                        }



                        // Milestone  completion status start //

                        decimal mileWattagecomplite = 0;
                        decimal mileWattagepending = 100;
                        decimal mileTaskTotal = 0;
                        string findmile = "select cast(ROUND(isnull(iscompletepercentage,0),2) AS DECIMAL(10,2)) as iscompletepercentage from Pms_tbl_AddMilestone  where ProjectId='" + ds1.Tables[0].Rows[j]["ProjectId"] + "' and id='" + ds1.Tables[0].Rows[j]["id"] + "'  and status=1  and archive=0";
                        DataSet dsssummile = da.GetDataSetInline(findmile);

                        try
                        {
                            mileTaskTotal = Convert.ToDecimal(dsssummile.Tables[0].Rows[0]["iscompletepercentage"]);
                        }
                        catch (Exception)
                        {
                        }

                        if (mileTaskTotal > 0)
                        {
                            mileWattagecomplite = mileTaskTotal;
                            mileWattagepending = Math.Abs(mileWattagepending - mileWattagecomplite);
                        }



                        //// Milestone  completion status  end //


                        //   task count start //

                        float miletskcomplite = 0;
                        float miletskpending = 0;
                        float miletskprojectTotal = 0;
                        float miletskPhaseTotal = 0;
                        float miletsk_TaskTotal = 0;

                        string milestr11 = "select COUNT(*) as counttsk from Pms_tbl_Addtask where ProjectId='" + ds1.Tables[0].Rows[j]["ProjectId"] + "' and Milestoneid='" + ds1.Tables[0].Rows[j]["id"] + "' and status=1 and archive=0 ";
                        DataSet miledss11 = da.GetDataSetInline(milestr11);
                        if (miledss11.Tables[0].Rows.Count > 0)
                        {

                            try
                            {
                                miletskPhaseTotal = Convert.ToInt32(miledss11.Tables[0].Rows[0]["counttsk"]);
                            }
                            catch (Exception)
                            {
                            }
                        }

                        string str22 = "select * from Pms_tbl_AddMilestone where ProjectId='" + ds1.Tables[0].Rows[j]["ProjectId"] + "' and id='" + ds1.Tables[0].Rows[j]["id"] + "' and status=1 and archive=0 ";
                        DataSet dss22 = da.GetDataSetInline(str22);
                        if (dss22.Tables[0].Rows.Count > 0)
                        {

                            string findtask11 = "select COUNT(*) as counttask  from Pms_tbl_Addtask where ProjectId='" + ds1.Tables[0].Rows[j]["ProjectId"] + "' and  Milestoneid='" + ds1.Tables[0].Rows[j]["id"] + "' and P_Status in ( select id from Pms_tbl_Projectstatus where  status=1 and archive=0 and isnull(iscomplete,0)=1)  and status=1 and archive=0";
                            DataSet dsssumtask11 = da.GetDataSetInline(findtask11);

                            try
                            {
                                miletsk_TaskTotal = Convert.ToInt32(dsssumtask11.Tables[0].Rows[0]["counttask"]);
                            }
                            catch (Exception)
                            {

                            }


                            miletskprojectTotal = (miletskPhaseTotal - miletsk_TaskTotal);


                        }

                        if (miletskprojectTotal > 0)
                        {
                            miletskcomplite = miletsk_TaskTotal;
                            miletskpending = Math.Abs(miletskPhaseTotal - miletsk_TaskTotal);
                        }

                        if (miletskprojectTotal == 0)
                        {
                            miletskcomplite = miletsk_TaskTotal;
                            miletskpending = Math.Abs(miletskPhaseTotal - miletsk_TaskTotal);
                        }

                        // task count end //


                        //   Issues count start //

                        float mileIssuescomplite = 0;
                        float mileIssuespending = 0;
                        float mileIssuesrojectTotal = 0;
                        float mileIssuesPhaseTotal = 0;
                        float mileIssues_Total = 0;

                        string milestr111 = "select COUNT(*) as countIssues from Pms_tbl_Addissues where ProjectId='" + ds1.Tables[0].Rows[j]["ProjectId"] + "' and Milestoneid='" + ds1.Tables[0].Rows[j]["id"] + "' and Issueon='milstone' and status=1 and archive=0 ";
                        DataSet miledss111 = da.GetDataSetInline(milestr111);
                        if (miledss111.Tables[0].Rows.Count > 0)
                        {

                            try
                            {
                                mileIssuesPhaseTotal = Convert.ToInt32(miledss111.Tables[0].Rows[0]["countIssues"]);
                            }
                            catch (Exception)
                            {
                            }
                        }


                        string milefindtask111 = "select COUNT(*) as count_Issues from Pms_tbl_Addissues where ProjectId='" + ds1.Tables[0].Rows[j]["ProjectId"] + "' and Milestoneid='" + ds1.Tables[0].Rows[j]["id"] + "' and Issueon='milstone' and IssueStatus in ( select id from Pms_tbl_Projectstatus where  status=1 and archive=0 and isnull(iscomplete,0)=1)  and status=1 and archive=0";
                        DataSet miledsssumtask111 = da.GetDataSetInline(milefindtask111);
                        if (miledsssumtask111.Tables[0].Rows.Count > 0)
                        {
                            try
                            {
                                mileIssues_Total = Convert.ToInt32(miledsssumtask111.Tables[0].Rows[0]["count_Issues"]);
                            }
                            catch (Exception)
                            {

                            }
                        }


                        mileIssuesrojectTotal = (mileIssuesPhaseTotal - mileIssues_Total);


                        if (mileIssuesrojectTotal > 0)
                        {
                            mileIssuescomplite = mileIssues_Total;
                            mileIssuespending = Math.Abs(mileIssuesPhaseTotal - mileIssues_Total);
                        }

                        if (mileIssuesrojectTotal == 0)
                        {
                            mileIssuescomplite = mileIssues_Total;
                            mileIssuespending = Math.Abs(mileIssuesPhaseTotal - mileIssues_Total);
                        }

                        // Issues count end //



                        taskdata += "<tr>";

                        //sno //
                        taskdata += "<td valign='top'>";
                        taskdata += "";
                        taskdata += "</td>";

                        //Action //
                        taskdata += " <td><a href='javascript:void(0);' Class='btn btn-info btn-sm' type='button' onclick='Binddetails_mile(" + ds1.Tables[0].Rows[j]["id"].ToString() + ")'  data-bs-toggle='offcanvas' data-bs-target='#addmile'><i class='ri-edit-line'></i></a>";
                        taskdata += "&nbsp;&nbsp;<a href='javascript:void(0);' Class='btn btn-danger btn-sm' type='button' onclick='deletedatamile(" + ds1.Tables[0].Rows[j]["id"].ToString() + ")'><i class='ri-delete-bin-5-line'></i></a>";
                        taskdata += "</td>";


                        int checkistask = 0;

                        string milestrchtask = " select isnull(istask,0) istask,isnull(statuscomplete,0)statuscomplete from  Pms_tbl_AddMilestone where id='" + ds1.Tables[0].Rows[j]["id"].ToString() + "' ";
                        DataSet miledschktask = da.GetDataSetInline(milestrchtask);
                        if (miledschktask.Tables[0].Rows.Count > 0)
                        {

                            checkistask = Convert.ToInt32(miledschktask.Tables[0].Rows[0]["istask"].ToString());

                        }


                        string checkshowhidecolmn_mile = "";
                        string showhidecolmn_mile = "";
                        string strchkhidecolmn_mile = " select * from Pms_tbl_Addtask where Milestoneid='" + ds1.Tables[0].Rows[j]["id"].ToString() + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                        DataSet dshideshow_mile = da.GetDataSetInline(strchkhidecolmn_mile);
                        if (dshideshow_mile.Tables[0].Rows.Count > 0)
                        {

                            checkshowhidecolmn_mile = "class='details-control'";
                            showhidecolmn_mile = "id = 'showtrM_" + ds1.Tables[0].Rows[j]["id"].ToString() + "'";
                        }



                        //Miestone Name
                        //taskdata += "<td "+ showhidecolmn_mile + " "+ checkshowhidecolmn_mile + ">";
                        taskdata += "<td>";
                       
                        taskdata += "<span  id='lblm_" + j.ToString() + "_1' onclick='edittext(this.id)' style='font-size:16px;'><b>" + (j + 1)+" " + get_Milstonename(ds1.Tables[0].Rows[j]["id"].ToString()) + "</b></span>";
                        taskdata += "<input type='text' id='txtm_" + j.ToString() + "_1' onleave='updatedetails(this.id);' style=display:none;>";
                        //if (checkistask != 0)
                        //{

                        //taskdata += "<a href = 'javascript:void(0);' type='button' onclick='AddMultitask(" + dsproject.Tables[0].Rows[P]["id"].ToString() + "," + ds1.Tables[0].Rows[j]["id"].ToString() + ")' data-bs-toggle='offcanvas' data-bs-target='#addtask' ><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Task'>+</span></a>";
                        taskdata += "<a href ='javascript:void(0);' role='button' data-bs-toggle='dropdown' aria-expanded='false'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover'>+</span>" +
                                "<ul class='dropdown-menu  dropdownmenu-secondary'>" +
                                "<li onclick='addrowUpDown(\"1\",\"" + j+"\",\""+ dsproject.Tables[0].Rows[P]["id"].ToString() + "\",\"" + (j+1).ToString() + "\")'>⬆️ Add Above</li>" +
                                "<li onclick='addrowUpDown(\"2\",\"" + j + "\",\"" + dsproject.Tables[0].Rows[P]["id"].ToString() + "\",\"" + ds1.Tables[0].Rows[j]["orderno"].ToString() + "\")'>⬇️ Add Bellow</li>" +
                                "<li " + showhidecolmn_mile + " " + checkshowhidecolmn_mile + ">➕ Add Task</li></ul></a>";
  

                           

                        //}
                        taskdata += " </td> ";


                        //milestone Completion Status start 
                        taskdata += "<td>";
                        if (mileWattagecomplite == 0)
                        {
                            
                            taskdata += "<div class='progress progress-xl cursor-pointer'>";
                            taskdata += "<div class='progress progress-xl bg-light position-relative' data-bs-toggle='tooltip' data-bs-placement='top' title='Complete(0%)' role='progressbar' style='width:5%'><span style='text-align:center;'>0</span>";
                            taskdata += "</div>";
                            taskdata += "</div>";

                        }

                        else
                        {
                            taskdata += "<div class='progress progress-xl cursor-pointer'>";
                            taskdata += "<div class='progress-bar bg-success' data-bs-toggle='tooltip' data-bs-placement='top' title='Complete (" + mileWattagecomplite + "%)' role='progressbar' style='width:" + mileWattagecomplite + "%'>" + mileWattagecomplite + "%";
                            taskdata += "</div>";
                            taskdata += "<div class='progress-bar bg-warning' data-toggle='tooltip' data-bs-placemen='top' title='Pending (" + mileWattagepending + "%)' role='progressbar' style='width:" + mileWattagepending + "%'>" + mileWattagepending + "%";
                            taskdata += "</div>";
                            taskdata += "</div>";
                            
                        }

                        taskdata += "</td>";
                        //Completion Status end 




                      


                        //weitage //
                        taskdata += "<td valign='top'>";
                        taskdata += fns.ToFormattedCurrencyString(ds1.Tables[0].Rows[j]["Weightage"].ToString()); 
                        taskdata += "</td>";


                        // owner //
                        
                        taskdata += "<td>";
                        taskdata += "<div class='d-flex'>";
                        taskdata += "<div class='flex-grow-1'>";
                        taskdata += "<div class='avatar-group' id='milelblowner_" + ds1.Tables[0].Rows[j]["id"].ToString() + "'>";
                        taskdata += mileusersdetailsowner;

                        taskdata += "<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='" + mileownertooltip + "' data-bs-original-title='more'>";

                        if (milecntownr != "" && milecnt != "0")
                        {
                            taskdata += "<div class='avatar-xxs'>";
                            taskdata += "<div class='avatar-title rounded-circle'>";

                            taskdata += (milecntownr + "+");
                            taskdata += "</div>";
                            taskdata += "</div>";
                        }

                        taskdata += "</a>";
                        taskdata += "</div>";
                        taskdata += "</div>";
                        taskdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                        taskdata += "<a href ='javascript:void(0);' type='button' id='milebtnownr_" + ds1.Tables[0].Rows[j]["id"].ToString() + "' onclick='mile_Binddetails_Owner(" + ds1.Tables[0].Rows[j]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#milechangeowner'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Owner'>+</span></a>";
                        taskdata += "</div>";
                        taskdata += "</div>";
                        taskdata += "</td>";



                        //team //

                        //taskdata += "<td valign='top'>";
                        //taskdata += ""; //team
                        //taskdata += "</td>";

                        taskdata += "<td>";
                        taskdata += "<div class='d-flex'>";
                        taskdata += "<div class='flex-grow-1'>";
                        taskdata += "<div class='avatar-group' id='milelbluser_" + ds1.Tables[0].Rows[j]["id"].ToString() + "'>";
                        taskdata += mileusersdetails;

                        taskdata += "<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='" + mileteamtooltip + "' data-bs-original-title='more'>";

                        if (milecnt != "" && milecnt != "0")
                        {
                            taskdata += "<div class='avatar-xxs'>";
                            taskdata += "<div class='avatar-title rounded-circle'>";

                            taskdata += (milecnt + "+");
                            taskdata += "</div>";
                            taskdata += "</div>";
                        }

                        taskdata += "</a>";
                        taskdata += "</div>";
                        taskdata += "</div>";
                        taskdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                        taskdata += "<a href ='javascript:void(0);' type='button' id='milebtnteam_" + ds1.Tables[0].Rows[j]["id"].ToString() + "' onclick='mileBinddetails_Team(" + ds1.Tables[0].Rows[j]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#mileaddteam'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Team'>+</span></a>";
                        taskdata += "</div>";
                        taskdata += "</div>";
                        taskdata += "</td>";


                        //Timeline

                        DateTime mileStartDate = Convert.ToDateTime(ds1.Tables[0].Rows[j]["StartDate"].ToString() == "" ? DateTime.Now.ToString() : ds1.Tables[0].Rows[j]["StartDate"].ToString());
                        DateTime mileTargetdate = Convert.ToDateTime(ds1.Tables[0].Rows[j]["EndDate"].ToString() == "" ? DateTime.Now.ToString() : ds1.Tables[0].Rows[j]["EndDate"].ToString());
                        TimeSpan mileobjTimeSpan = mileTargetdate - mileStartDate;
                        double mileDays = Convert.ToDouble(mileobjTimeSpan.TotalDays);
                        mileDays = mileDays + 1;


                        DateTime mileEnddate = Convert.ToDateTime(ds1.Tables[0].Rows[j]["EndDate"].ToString() == "" ? DateTime.Now.ToString() : ds1.Tables[0].Rows[j]["EndDate"].ToString());
                        DateTime mileCurrentdate = DateTime.Now;
                        TimeSpan mileobjtimeleftspn = mileEnddate - mileCurrentdate;
                        double mileleftDays = Convert.ToDouble(mileobjtimeleftspn.Days);
                        mileleftDays = mileleftDays + 1;

                        string mileleftdlydys = "";
                        if (mileleftDays < 0)
                        {
                            mileleftdlydys = "days delayed";
                        }
                        else
                        {
                            mileleftdlydys = "days left";
                        }


                        //start days months  
                        string milestartmonthname = "";
                        int mileststartdys = mileStartDate.Day;
                        int milestrtmonth = mileStartDate.Month;
                        int milestonestartyear = mileStartDate.Year;
                        if (milestrtmonth == 1)
                        {
                            milestartmonthname = "Jan";
                        }
                        if (milestrtmonth == 2)
                        {
                            milestartmonthname = "Feb";
                        }
                        if (milestrtmonth == 3)
                        {
                            milestartmonthname = "Mar";
                        }

                        if (milestrtmonth == 4)
                        {
                            milestartmonthname = "Apr";
                        }

                        if (milestrtmonth == 5)
                        {
                            milestartmonthname = "May";
                        }

                        if (milestrtmonth == 6)
                        {
                            milestartmonthname = "Jun";
                        }

                        if (milestrtmonth == 7)
                        {
                            milestartmonthname = "Jul";
                        }

                        if (milestrtmonth == 8)
                        {
                            milestartmonthname = "Aug";
                        }

                        if (milestrtmonth == 9)
                        {
                            milestartmonthname = "Sep";
                        }

                        if (milestrtmonth == 10)
                        {
                            milestartmonthname = "Oct";
                        }

                        if (milestrtmonth == 11)
                        {
                            milestartmonthname = "Nov";
                        }

                        if (milestrtmonth == 12)
                        {
                            milestartmonthname = "Dec";
                        }



                        //end days months 
                        string mileendmonthname = "";
                        int mileenddys = mileTargetdate.Day;
                        int mileendmonth = mileTargetdate.Month;
                        int milestoneEndyear = mileTargetdate.Year;
                        if (mileendmonth == 1)
                        {
                            mileendmonthname = "Jan";
                        }
                        if (mileendmonth == 2)
                        {
                            mileendmonthname = "Feb";
                        }
                        if (mileendmonth == 3)
                        {
                            mileendmonthname = "Mar";
                        }

                        if (mileendmonth == 4)
                        {
                            mileendmonthname = "Apr";
                        }

                        if (mileendmonth == 5)
                        {
                            mileendmonthname = "May";
                        }

                        if (mileendmonth == 6)
                        {
                            mileendmonthname = "Jun";
                        }

                        if (mileendmonth == 7)
                        {
                            mileendmonthname = "Jul";
                        }

                        if (mileendmonth == 8)
                        {
                            mileendmonthname = "Aug";
                        }

                        if (mileendmonth == 9)
                        {
                            mileendmonthname = "Sep";
                        }

                        if (mileendmonth == 10)
                        {
                            mileendmonthname = "Oct";
                        }

                        if (mileendmonth == 11)
                        {
                            mileendmonthname = "Nov";
                        }

                        if (mileendmonth == 12)
                        {
                            mileendmonthname = "Dec";
                        }



                        taskdata += "<td>";
                        taskdata += "<a href = 'javascript:void(0);' class='timesheet-wrap'>";
                        taskdata += "<div class='progress progress-xl bg-dark position-relative' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + mileleftDays.ToString().Replace("-", "") + " " + mileleftdlydys + "'>";
                        //Milestonedata += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 70%;' aria-valuemax='100'></div>";
                        taskdata += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 100%;' aria-valuemax='100'></div>";
                        taskdata += "<div class='dayswrapper'>";
                        taskdata += "<span class='fromtodate'>" + mileststartdys + " " + milestartmonthname +" " + milestonestartyear + " - " + mileenddys + " " + mileendmonthname +" " + milestoneEndyear + "</span>";
                        taskdata += "<span class='totaldays'>" + mileDays + " days</span>";
                        taskdata += "</div>";
                        taskdata += "</div>";
                        taskdata += "</a>";
                        taskdata += "</td>";


                        // extended timeleine 

                        string mileestendleftdlydys = "";
                        string mileestendstartmonthname = "";
                        string mileestendendmonthname = "";
                        double mileestendleftDays = 0;
                        int mileestendststartdys = 0;
                        int mileestendenddys = 0;
                        double mileestendDays = 0;
                        string mileextenddatebind = "";
                        string milestrextend = "select id,extendStartdate as StartDate,extendEnddate as TargetDate from Pms_Tbl_Extended_Timeline where  status=1 and Type='Milestone' and archive=0  and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and MilestoneID = '" + ds1.Tables[0].Rows[j]["id"].ToString() + "'  ";
                        DataSet miledsstrextend = da.GetDataSetInline(milestrextend);
                        if (miledsstrextend.Tables[0].Rows.Count > 0)
                        {
                            for (int k = 0; k < miledsstrextend.Tables[0].Rows.Count; k++)
                            {
                                DateTime mileestendStartDate = Convert.ToDateTime(miledsstrextend.Tables[0].Rows[k]["StartDate"].ToString() == "" ? DateTime.Now.ToString() : miledsstrextend.Tables[0].Rows[k]["StartDate"].ToString());
                                DateTime mileestendTargetdate = Convert.ToDateTime(miledsstrextend.Tables[0].Rows[k]["TargetDate"].ToString() == "" ? DateTime.Now.ToString() : miledsstrextend.Tables[0].Rows[k]["TargetDate"].ToString());
                                TimeSpan mileestendobjTimeSpan = mileestendTargetdate - mileestendStartDate;
                                mileestendDays = Convert.ToDouble(mileestendobjTimeSpan.TotalDays);
                                mileestendDays = mileestendDays + 1;


                                DateTime mileestendEnddate = Convert.ToDateTime(miledsstrextend.Tables[0].Rows[k]["Targetdate"].ToString() == "" ? DateTime.Now.ToString() : miledsstrextend.Tables[0].Rows[k]["Targetdate"].ToString());
                                DateTime mileestendCurrentdate = DateTime.Now;
                                TimeSpan mileestendobjtimeleftspn = mileestendEnddate - mileestendCurrentdate;
                                mileestendleftDays = Convert.ToDouble(mileestendobjtimeleftspn.Days);
                                mileestendleftDays = mileestendleftDays + 1;


                                if (mileestendleftDays < 0)
                                {
                                    mileestendleftdlydys = "days delayed";
                                }
                                else
                                {
                                    mileestendleftdlydys = "days left";
                                }

                                //start days months  

                                mileestendststartdys = mileestendStartDate.Day;
                                int mileestendstrtmonth = mileestendStartDate.Month;
                                if (mileestendstrtmonth == 1)
                                {
                                    mileestendstartmonthname = "Jan";
                                }
                                if (mileestendstrtmonth == 2)
                                {
                                    mileestendstartmonthname = "Feb";
                                }
                                if (mileestendstrtmonth == 3)
                                {
                                    mileestendstartmonthname = "Mar";
                                }

                                if (mileestendstrtmonth == 4)
                                {
                                    mileestendstartmonthname = "Apr";
                                }

                                if (mileestendstrtmonth == 5)
                                {
                                    mileestendstartmonthname = "May";
                                }

                                if (mileestendstrtmonth == 6)
                                {
                                    mileestendstartmonthname = "Jun";
                                }

                                if (mileestendstrtmonth == 7)
                                {
                                    mileestendstartmonthname = "Jul";
                                }

                                if (mileestendstrtmonth == 8)
                                {
                                    mileestendstartmonthname = "Aug";
                                }

                                if (mileestendstrtmonth == 9)
                                {
                                    mileestendstartmonthname = "Sep";
                                }

                                if (mileestendstrtmonth == 10)
                                {
                                    mileestendstartmonthname = "Oct";
                                }

                                if (mileestendstrtmonth == 11)
                                {
                                    mileestendstartmonthname = "Nov";
                                }

                                if (mileestendstrtmonth == 12)
                                {
                                    mileestendstartmonthname = "Dec";
                                }

                                //end days months 

                                mileestendenddys = mileestendTargetdate.Day;
                                int mileestendendmonth = mileestendTargetdate.Month;
                                if (mileestendendmonth == 1)
                                {
                                    mileestendendmonthname = "Jan";
                                }
                                if (mileestendendmonth == 2)
                                {
                                    mileestendendmonthname = "Feb";
                                }
                                if (mileestendendmonth == 3)
                                {
                                    mileestendendmonthname = "Mar";
                                }

                                if (mileestendendmonth == 4)
                                {
                                    mileestendendmonthname = "Apr";
                                }

                                if (mileestendendmonth == 5)
                                {
                                    mileestendendmonthname = "May";
                                }

                                if (mileestendendmonth == 6)
                                {
                                    mileestendendmonthname = "Jun";
                                }

                                if (mileestendendmonth == 7)
                                {
                                    mileestendendmonthname = "Jul";
                                }

                                if (mileestendendmonth == 8)
                                {
                                    mileestendendmonthname = "Aug";
                                }

                                if (mileestendendmonth == 9)
                                {
                                    mileestendendmonthname = "Sep";
                                }

                                if (mileestendendmonth == 10)
                                {
                                    mileestendendmonthname = "Oct";
                                }

                                if (mileestendendmonth == 11)
                                {
                                    mileestendendmonthname = "Nov";
                                }

                                if (mileestendendmonth == 12)
                                {
                                    mileestendendmonthname = "Dec";
                                }



                                mileextenddatebind += "<a href ='javascript:void(0);' class='timesheet-wrap'>";
                                mileextenddatebind += "<div class='progress progress-xl bg-dark position-relative me-1' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + mileestendststartdys + " " + mileestendstartmonthname + " - " + mileestendenddys + " " + mileestendendmonthname + " (" + mileestendleftDays.ToString().Replace("-", "") + " " + mileestendleftdlydys + ")'>";
                                mileextenddatebind += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 100%;' aria-valuemax='100'>" + mileestendDays + " days</div>";
                                mileextenddatebind += "</div>";
                                mileextenddatebind += "</a>";

                            }

                        }





                        taskdata += "<td>";
                        taskdata += "<div class='d-flex'>";
                        taskdata += "<div class='flex-grow-1 align-self-center me-2'>";
                        taskdata += "<div class='extended_time' id='milelblExtnddt_" + ds1.Tables[0].Rows[j]["id"].ToString() + "'>";

                        taskdata += "" + mileextenddatebind + "";

                        taskdata += "</div>";
                        taskdata += "</div>";

                        if (mileleftDays < 0)
                        {
                            taskdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                            taskdata += "<a href ='javascript:void(0);' type='button' id='btnextndt_" + ds1.Tables[0].Rows[j]["id"].ToString() + "'  onclick='MileBindextenddate_Details(" + ds1.Tables[0].Rows[j]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#MileExtendedTimline'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Extended Day'>+</span></a>";
                            taskdata += "</div>";
                        }

                        taskdata += "</div>";
                        taskdata += "</div>";
                        taskdata += "</td>";


                        // start date
                        taskdata += "<td valign='top'>";
                        taskdata += cmf.dateConvert(ds1.Tables[0].Rows[j]["StartDate"].ToString()); 
                        taskdata += "</td>";

                        //End date
                        taskdata += "<td valign='top'>";
                        taskdata += cmf.dateConvert(ds1.Tables[0].Rows[j]["EndDate"].ToString()); 
                        taskdata += "</td>";
                        
                        //Status//

                        string milestrvalcolor = get_Status_Color(ds1.Tables[0].Rows[j]["P_Status"].ToString());
                        taskdata += "<td>";
                        taskdata += "<div class='btn-group d-block'>";
                        taskdata += "<a href ='javascript:void(0);' id='milebtnsts_" + ds1.Tables[0].Rows[j]["id"].ToString() + "' type='button' onclick=setmilsestoneid('" + ds1.Tables[0].Rows[j]["id"].ToString() + "',this.id) data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + milestrvalcolor + "'>" + get_Projectstatus(ds1.Tables[0].Rows[j]["P_Status"].ToString()) + "</a>";
                        taskdata += "<div class='dropdown-menu lead_form_body p-0'>";
                        taskdata += mileretval;
                        taskdata += "</div>";
                        taskdata += "</div>";
                        taskdata += "</td>";
                        
                        //stpriority

                        string milestrprioritycolor = get_PriorityColor(ds1.Tables[0].Rows[j]["Priority"].ToString());
                        taskdata += "<td>";
                        taskdata += "<div class='btn-group d-block'>";
                        taskdata += "<a href ='javascript:void(0);' id='milebtnpri_" + ds1.Tables[0].Rows[j]["id"].ToString() + "' type='button' onclick=milesetpriorityid('" + ds1.Tables[0].Rows[j]["id"].ToString() + "',this.id) data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + milestrprioritycolor + "'>" + get_PriorityName(ds1.Tables[0].Rows[j]["Priority"].ToString()) + "</a>";
                        taskdata += "<div class='dropdown-menu lead_form_body p-0'>";
                        taskdata += milestrvalprority;
                        taskdata += "</div>";
                        taskdata += "</div>";
                        taskdata += "</td>";


                        //Issues

                        taskdata += "<td valign='top'>";

                        float milecompletetepercentissues = 0;
                        float milePendingepercentissues = 0;
                        milecompletetepercentissues = mileIssuescomplite * 100 / mileIssuesPhaseTotal;
                        milePendingepercentissues = mileIssuespending * 100 / mileIssuesPhaseTotal;

                        taskdata += "<div class='d-flex'>";
                        taskdata += "<div class='flex-grow-1 align-self-center'>";

                        taskdata += "<div class='progress progress-xl cursor-pointer'>";
                        taskdata += "<div class='progress-bar bg-success' role='progressbar' style='width: " + fns.ToFormattedCurrencyString(milecompletetepercentissues.ToString()) + "%' aria-valuemin='0' aria-valuemax='100' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + fns.ToFormattedCurrencyString(milecompletetepercentissues.ToString()) + "% [" + mileIssuescomplite + " of " + mileIssuesPhaseTotal + " Issues Completed]'>" + mileIssuescomplite + "</div>";

                        taskdata += "<div class='progress-bar bg-warning' role='progressbar' style='width: " + fns.ToFormattedCurrencyString(milePendingepercentissues.ToString()) + "%' aria-valuemin='0' aria-valuemax='100' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + fns.ToFormattedCurrencyString(milePendingepercentissues.ToString()) + "% [" + mileIssuespending + " of " + mileIssuesPhaseTotal + " Issues Not Completed]'>" + mileIssuespending + "</div>";
                        taskdata += "</div>";
                        taskdata += "</div>";

                        taskdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                        taskdata += "<a href ='javascript:void(0);' type='button' data-bs-toggle='offcanvas' data-bs-target=#canvas_popup onclick=loadsrc('inner_popup','/pms/AddIssuesPopup.aspx?id=" + ds1.Tables[0].Rows[j]["ProjectId"].ToString() + "&milstoneid=" + ds1.Tables[0].Rows[j]["id"].ToString() + "&type=milstone')><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Issue'>+</span></a>";

                        taskdata += "</div>";
                        taskdata += "</td>";

                        //Task Duration //
                        taskdata += "<td valign='top'>";
                        taskdata += "";
                        taskdata += "</td>";

                       //Stage //
                        taskdata += "<td valign='top'>";
                        taskdata += "";
                        taskdata += "</td>";

                        //Subtask //

                        
                        
                        taskdata += "</tr>";
                        #endregion
                        #region task add
                        string strstask = "select * from Pms_tbl_Addtask where   ProjectId='" + dsproject.Tables[0].Rows[P]["id"].ToString() + "' and Milestoneid='" + ds1.Tables[0].Rows[j]["id"].ToString() + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                        DataSet ds = da.GetDataSetInline(strstask);
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


                                //dependancy start //

                                string cntdepndcy = "0";
                                string depdecydetails = "";
                                string depcytooltip = "";

                                string strdepcysql = "Select * from Pms_tbl_Addtask where id = '" + ds.Tables[0].Rows[i]["id"] + "' and status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                                DataSet dsdepcy = da.GetDataSetInline(strdepcysql);
                                if (dsdepcy.Tables[0].Rows[0]["dependancy"].ToString() != "")
                                {
                                    string[] arcntdep = dsdepcy.Tables[0].Rows[0]["dependancy"].ToString().Trim().Split(',');
                                    cntdepndcy = arcntdep.Length.ToString();
                                    DataSet dsdtlsdepcy = new DataSet();
                                    if (dsdepcy.Tables[0].Rows[0]["dependancy"].ToString().Trim() != "")
                                    {
                                        dsdtlsdepcy = da.GetDataSetInline("select id,taskname from Pms_tbl_Addtask where id in(" + dsdepcy.Tables[0].Rows[0]["dependancy"].ToString().Trim() + ")");
                                        if (dsdtlsdepcy.Tables[0].Rows.Count > 1)
                                        {
                                            cntdepndcy = (Convert.ToInt32(cntdepndcy) - 1).ToString();

                                            // dependancy tool tip //
                                            for (int l = 1; l < dsdtlsdepcy.Tables[0].Rows.Count; l++)
                                            {
                                                if (dsdtlsdepcy.Tables[0].Rows[l]["taskname"].ToString() != string.Empty)
                                                {
                                                    if (depcytooltip != string.Empty)
                                                    {
                                                        depcytooltip += "," + dsdtlsdepcy.Tables[0].Rows[l]["taskname"].ToString();
                                                    }
                                                    else
                                                    {
                                                        depcytooltip += dsdtlsdepcy.Tables[0].Rows[l]["taskname"].ToString();
                                                    }
                                                }

                                            }

                                            for (int k = 0; k < 1; k++)
                                            {
                                                if (dsdtlsdepcy.Tables[0].Rows[k]["taskname"].ToString() != "")
                                                {

                                                    depdecydetails += "<span class='badge bg-primary'>" + dsdtlsdepcy.Tables[0].Rows[k]["taskname"].ToString() + "</span>";
                                                }
                                                else
                                                {
                                                    depdecydetails += "<span class='badge bg-primary'>" + dsdtlsdepcy.Tables[0].Rows[k]["taskname"].ToString() + "</span>";
                                                }
                                            }

                                        }
                                        else
                                        {
                                            cntdepndcy = "";
                                            for (int k = 0; k < dsdtlsdepcy.Tables[0].Rows.Count; k++)
                                            {
                                                if (dsdtlsdepcy.Tables[0].Rows[k]["taskname"].ToString() != "")
                                                {
                                                    depdecydetails += "<span class='badge bg-primary'>" + dsdtlsdepcy.Tables[0].Rows[k]["taskname"].ToString() + "</span>";
                                                }
                                                else
                                                {
                                                    depdecydetails += "<span class='badge bg-primary'>" + dsdtlsdepcy.Tables[0].Rows[k]["taskname"].ToString() + "</span>";
                                                }
                                            }
                                        }

                                    }
                                }


                                // dependancy end//




                                //   completion  status  start //

                                decimal Wattagecomplite = 0;
                                decimal Wattagepending = 100;
                                decimal projectTotal = 0;
                                decimal PhaseTotal = 0;
                                decimal TaskTotal = 0;

                                string findtask = "select cast(ROUND(isnull(iscompletepercentage,0),2) AS DECIMAL(10,2)) as iscompletepercentage from Pms_tbl_Addtask  where ProjectId='" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "' and Milestoneid='" + ds.Tables[0].Rows[i]["Milestoneid"] + "' and id='" + ds.Tables[0].Rows[i]["id"] + "'  and status=1  and archive=0";
                                DataSet dsssumtask2 = da.GetDataSetInline(findtask);

                                try
                                {

                                    TaskTotal = Convert.ToDecimal(dsssumtask2.Tables[0].Rows[0]["iscompletepercentage"].ToString());
                                }
                                catch (Exception)
                                {

                                }

                                projectTotal = TaskTotal;




                                if (projectTotal > 0)
                                {
                                    Wattagecomplite = projectTotal;
                                    Wattagepending = Math.Abs(Wattagepending - Wattagecomplite);
                                }

                                //// completion task end //



                                //  Sub task count start //

                                float tskcomplite = 0;
                                float tskpending = 0;
                                float tskprojectTotal = 0;
                                float tskPhaseTotal = 0;
                                float tsk_TaskTotal = 0;

                                string str11 = "select COUNT(*) as countsbtsk from Pms_tbl_Addsubtask where ProjectId='" + ds.Tables[0].Rows[i]["ProjectId"] + "' and Milestoneid='" + ds.Tables[0].Rows[i]["Milestoneid"] + "' and task='" + ds.Tables[0].Rows[i]["id"] + "' and status=1 and archive=0 ";
                                DataSet dss11 = da.GetDataSetInline(str11);
                                if (dss11.Tables[0].Rows.Count > 0)
                                {

                                    try
                                    {
                                        tskPhaseTotal = Convert.ToInt32(dss11.Tables[0].Rows[0]["countsbtsk"]);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }

                                string findtask11 = "select COUNT(*) as counttask  from Pms_tbl_Addsubtask where ProjectId='" + ds.Tables[0].Rows[i]["ProjectId"] + "' and  Milestoneid='" + ds.Tables[0].Rows[i]["Milestoneid"] + "' and task='" + ds.Tables[0].Rows[i]["id"] + "' and P_Status in ( select id from Pms_tbl_Projectstatus where  status=1 and archive=0 and isnull(iscomplete,0)=1)  and status=1 and archive=0";
                                DataSet dsssumtask11 = da.GetDataSetInline(findtask11);

                                try
                                {
                                    tsk_TaskTotal = Convert.ToInt32(dsssumtask11.Tables[0].Rows[0]["counttask"]);
                                }
                                catch (Exception)
                                {

                                }


                                tskprojectTotal = (tskPhaseTotal - tsk_TaskTotal);




                                if (tskprojectTotal > 0)
                                {
                                    tskcomplite = tsk_TaskTotal;
                                    tskpending = Math.Abs(tskPhaseTotal - tsk_TaskTotal);
                                }

                                if (tskprojectTotal == 0)
                                {
                                    tskcomplite = tsk_TaskTotal;
                                    tskpending = Math.Abs(tskPhaseTotal - tsk_TaskTotal);
                                }

                                //Sub task count end //




                                //   Issues count start //

                                float Issuescomplite = 0;
                                float Issuespending = 0;
                                float IssuesrojectTotal = 0;
                                float IssuesPhaseTotal = 0;
                                float Issues_Total = 0;

                                string str111 = "select COUNT(*) as countIssues from Pms_tbl_Addissues where ProjectId='" + ds.Tables[0].Rows[i]["ProjectId"] + "' and Milestoneid='" + ds.Tables[0].Rows[i]["Milestoneid"] + "' and taskid='" + ds.Tables[0].Rows[i]["id"] + "' and Issueon='task' and status=1 and archive=0 ";
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


                                string findtask111 = "select COUNT(*) as count_Issues from Pms_tbl_Addissues where ProjectId='" + ds.Tables[0].Rows[i]["ProjectId"] + "' and Milestoneid='" + ds.Tables[0].Rows[i]["Milestoneid"] + "' and taskid='" + ds.Tables[0].Rows[i]["id"] + "' and Issueon='task' and IssueStatus in ( select id from Pms_tbl_Projectstatus where  status=1 and archive=0 and isnull(iscomplete,0)=1)  and status=1 and archive=0";
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



                                taskdata += "<tr class='taskshow_"+ ds1.Tables[0].Rows[j]["id"].ToString() + "' style='display:none;'>";

                                //sno //

                                taskdata += "<td valign='top'>";
                                taskdata += "";
                                taskdata += "</td>";

                                taskdata += " <td><a href='javascript:void(0);' Class='btn btn-info btn-sm' type='button' onclick='Binddetails(" + ds.Tables[0].Rows[i]["id"].ToString() + ")' data-bs-toggle='offcanvas' data-bs-target='#addtask'><i class='ri-edit-line'></i></a>";
                                taskdata += "&nbsp;&nbsp;<a href='javascript:void(0);' Class='btn btn-danger btn-sm' type='button' onclick='deletedata(" + ds.Tables[0].Rows[i]["id"].ToString() + ")'><i class='ri-delete-bin-5-line'></i></a>";
                                taskdata += "</td>";

                                //// timer 

                                ////check statuscomplet //
                                //int isstatuscomplete = 0;
                                //string strchsttscomplt = "select isnull(iscomplete,0) as iscomplete from Pms_tbl_Projectstatus where status = 1 and archive = 0  and id in (select P_Status from Pms_tbl_Addtask where ProjectId = '" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "' and Milestoneid = '" + ds.Tables[0].Rows[i]["Milestoneid"].ToString() + "' and id = '" + ds.Tables[0].Rows[i]["id"].ToString() + "' and status = 1 and archive = 0) and isnull(iscomplete,0)= 1  and status = 1 and archive = 0 ";
                                //DataSet dschksttscomplt = da.GetDataSetInline(strchsttscomplt);
                                //if (dschksttscomplt.Tables[0].Rows.Count > 0)
                                //{


                                //    isstatuscomplete = Convert.ToInt32(dschksttscomplt.Tables[0].Rows[0]["iscomplete"].ToString());
                                //}


                                ////check issubtask start //

                                int checkissubtask = 0;
                                string strchksubtask = " select isnull(issubtask,0) issubtask from  Pms_tbl_Addtask where id='" + ds.Tables[0].Rows[i]["id"].ToString() + "' ";
                                DataSet dschksubtask = da.GetDataSetInline(strchksubtask);
                                if (dschksubtask.Tables[0].Rows.Count > 0)
                                {

                                    checkissubtask = Convert.ToInt32(dschksubtask.Tables[0].Rows[0]["issubtask"].ToString());
                                }



                                string checkshowhidecolmn = "";
                                string showhidecolmn_task = "";
                                string strchkhidecolmn = " select * from Pms_tbl_Addsubtask where task='" + ds.Tables[0].Rows[i]["id"].ToString() + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                                DataSet dshideshow = da.GetDataSetInline(strchkhidecolmn);
                                if (dshideshow.Tables[0].Rows.Count > 0)
                                {

                                    checkshowhidecolmn = "class='details-control'";
                                    showhidecolmn_task = "id='showtasktr_" + ds.Tables[0].Rows[i]["id"].ToString() + "'";
                                }



                                taskdata += "<td "+ showhidecolmn_task + " "+ checkshowhidecolmn + ">";
                                taskdata += "&nbsp;&nbsp;&nbsp;&nbsp; " + (j + 1) + "." + (i + 1) +" " + ds.Tables[0].Rows[i]["taskname"].ToString();

                                if (checkissubtask != 0)
                                {

                                    taskdata += "<a href = 'javascript:void(0);' type='button' onclick='AddMultisubtask(" + dsproject.Tables[0].Rows[P]["id"].ToString() + "," + ds1.Tables[0].Rows[j]["id"].ToString() + "," + ds.Tables[0].Rows[i]["id"].ToString() + ")' data-bs-toggle='offcanvas' data-bs-target='#addsubtask'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Sub Task'>+</span></a>";

                                }

                                taskdata += "</td>";

                                // task name

                              


                                //completion status start 

                                taskdata += "<td>";

                                if (Wattagecomplite == 0)
                                {

                                    taskdata += "<div class='progress progress-xl cursor-pointer'>";
                                    taskdata += "<div class='progress progress-xl bg-light position-relative' data-bs-toggle='tooltip' data-bs-placement='top' title='Complete(0%)' role='progressbar' style='width:5%'><span style='text-align:center;'>0</span>";
                                    taskdata += "</div>";

                                    taskdata += "</div>";

                                }

                                else
                                {

                                    taskdata += "<div class='progress progress-xl cursor-pointer'>";
                                    taskdata += "<div class='progress-bar bg-success' data-bs-toggle='tooltip' data-bs-placement='top' title='Complete (" + Wattagecomplite + "%)' role='progressbar' style='width:" + Wattagecomplite + "%'>" + Wattagecomplite + "%";
                                    taskdata += "</div>";

                                    taskdata += "<div class='progress-bar bg-warning' data-toggle='tooltip' data-bs-placemen='top' title='Pending (" + Wattagepending + "%)' role='progressbar' style='width:" + Wattagepending + "%'>" + Wattagepending + "%";
                                    taskdata += "</div>";
                                    taskdata += "</div>";


                                }

                                taskdata += "</td>";


                                // completion status end //


                                //// Task //
                                //taskdata += "<td valign='top'>";
                                //taskdata += "";
                                //taskdata += "</td>";

                                taskdata += "<td valign='top'>";
                                taskdata += fns.ToFormattedCurrencyString(ds.Tables[0].Rows[i]["Weightage"].ToString()); // Weightage 
                                taskdata += "</td>";
                                

                                //Owner
                                taskdata += "<td>";
                                taskdata += "<div class='d-flex'>";
                                taskdata += "<div class='flex-grow-1'>";
                                taskdata += "<div class='avatar-group' id='lblowner_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>";
                                taskdata += usersdetailsowner;
                                taskdata += "<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='" + ownertooltip + "' data-bs-original-title='more'>";

                                if (cntownr != "" && cnt != "0")
                                {
                                    taskdata += "<div class='avatar-xxs'>";
                                    taskdata += "<div class='avatar-title rounded-circle'>";

                                    taskdata += (cntownr + "+");
                                    taskdata += "</div>";
                                    taskdata += "</div>";
                                }

                                taskdata += "</a>";
                                taskdata += "</div>";
                                taskdata += "</div>";
                                taskdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                                taskdata += "<a href ='javascript:void(0);' type='button' id='btnownr_" + ds.Tables[0].Rows[i]["id"].ToString() + "' onclick='Binddetails_Owner(" + ds.Tables[0].Rows[i]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#changeowner'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Owner'>+</span></a>";
                                taskdata += "</div>";
                                taskdata += "</div>";
                                taskdata += "</td>";


                                //team
                                taskdata += "<td>";
                                taskdata += "<div class='d-flex'>";
                                taskdata += "<div class='flex-grow-1'>";
                                taskdata += "<div class='avatar-group' id='lbluser_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>";
                                taskdata += usersdetails;

                                //projectdata += "</a>";
                                taskdata += "<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='" + teamtooltip + "' data-bs-original-title='more'>";

                                if (cnt != "" && cnt != "0")
                                {
                                    taskdata += "<div class='avatar-xxs'>";
                                    taskdata += "<div class='avatar-title rounded-circle'>";

                                    taskdata += (cnt + "+");
                                    taskdata += "</div>";
                                    taskdata += "</div>";
                                }

                                taskdata += "</a>";
                                taskdata += "</div>";
                                taskdata += "</div>";
                                taskdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                                taskdata += "<a href ='javascript:void(0);' type='button' id='btnteam_" + ds.Tables[0].Rows[i]["id"].ToString() + "' onclick='Binddetails_Team(" + ds.Tables[0].Rows[i]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#addteam'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Team'>+</span></a>";
                                taskdata += "</div>";
                                taskdata += "</div>";
                                taskdata += "</td>";


                                //Timeline

                                //Storing input Dates  
                                DateTime StartDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"].ToString() == "" ? DateTime.Now.ToString() : ds.Tables[0].Rows[i]["StartDate"].ToString());
                                DateTime Targetdate = Convert.ToDateTime(ds.Tables[0].Rows[i]["Targetdate"].ToString() == "" ? DateTime.Now.ToString() : ds.Tables[0].Rows[i]["Targetdate"].ToString());
                                TimeSpan objTimeSpan = Targetdate - StartDate;
                                double Days = Convert.ToDouble(objTimeSpan.Days);
                                Days = Days + 1;


                                DateTime Enddate = Convert.ToDateTime(ds.Tables[0].Rows[i]["Targetdate"].ToString() == "" ? DateTime.Now.ToString() : ds.Tables[0].Rows[i]["Targetdate"].ToString());
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
                                int taskstartyear = StartDate.Year;
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
                                int taskEndyear = Targetdate.Year;
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


                                //title = '"+leftDays+" days left'

                                taskdata += "<td>";
                                taskdata += "<a href = 'javascript:void(0);'>";
                                taskdata += "<div class='progress progress-xl bg-dark position-relative' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + leftDays.ToString().Replace("-", "") + " " + leftdlydys + "'>";
                                // taskdata += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 70%;' aria-valuemax='100'></div>";
                                taskdata += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 100%;' aria-valuemax='100'></div>";
                                taskdata += "<div class='dayswrapper'>";
                                taskdata += "<span class='fromtodate'>" + ststartdys + " " + startmonthname+ " " + taskstartyear + " - " + enddys + " " + endmonthname + " " + taskEndyear + "</span>";
                                taskdata += "<span class='totaldays'>" + Days + " days</span>";
                                taskdata += "</div>";
                                taskdata += "</div>";
                                taskdata += "</a>";
                                taskdata += "</td>";




                                //Task extended timeleine 

                                string task_estendleftdlydys = "";
                                string Task_estendstartmonthname = "";
                                string task_estendendmonthname = "";
                                double task_estendleftDays = 0;
                                int task_estendststartdys = 0;
                                int task_estendenddys = 0;
                                double task_estendDays = 0;
                                string task_extenddatebind = "";
                                string task_strextend = "select id,extendStartdate as StartDate,extendEnddate as TargetDate from Pms_Tbl_Extended_Timeline where  status=1 and Type='task' and archive=0  and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and taskid = '" + ds.Tables[0].Rows[i]["id"].ToString() + "'  ";
                                DataSet task_dsstrextend = da.GetDataSetInline(task_strextend);
                                if (task_dsstrextend.Tables[0].Rows.Count > 0)
                                {
                                    for (int Tk = 0; Tk < task_dsstrextend.Tables[0].Rows.Count; Tk++)
                                    {
                                        DateTime task_estendStartDate = Convert.ToDateTime(task_dsstrextend.Tables[0].Rows[Tk]["StartDate"].ToString() == "" ? DateTime.Now.ToString() : task_dsstrextend.Tables[0].Rows[Tk]["StartDate"].ToString());
                                        DateTime task_estendTargetdate = Convert.ToDateTime(task_dsstrextend.Tables[0].Rows[Tk]["TargetDate"].ToString() == "" ? DateTime.Now.ToString() : task_dsstrextend.Tables[0].Rows[Tk]["TargetDate"].ToString());
                                        TimeSpan task_estendobjTimeSpan = task_estendTargetdate - task_estendStartDate;
                                        task_estendDays = Convert.ToDouble(task_estendobjTimeSpan.TotalDays);
                                        task_estendDays = task_estendDays + 1;


                                        DateTime task_estendEnddate = Convert.ToDateTime(task_dsstrextend.Tables[0].Rows[Tk]["Targetdate"].ToString() == "" ? DateTime.Now.ToString() : task_dsstrextend.Tables[0].Rows[Tk]["Targetdate"].ToString());
                                        DateTime task_estendCurrentdate = DateTime.Now;
                                        TimeSpan task_estendobjtimeleftspn = task_estendEnddate - task_estendCurrentdate;
                                        task_estendleftDays = Convert.ToDouble(task_estendobjtimeleftspn.Days);
                                        task_estendleftDays = task_estendleftDays + 1;


                                        if (task_estendleftDays < 0)
                                        {
                                            task_estendleftdlydys = "days delayed";
                                        }
                                        else
                                        {
                                            task_estendleftdlydys = "days left";
                                        }

                                        //start days months  

                                        task_estendststartdys = task_estendCurrentdate.Day;
                                        int task_estendstrtmonth = task_estendCurrentdate.Month;
                                        if (task_estendstrtmonth == 1)
                                        {
                                            Task_estendstartmonthname = "Jan";
                                        }
                                        if (task_estendstrtmonth == 2)
                                        {
                                            Task_estendstartmonthname = "Feb";
                                        }
                                        if (task_estendstrtmonth == 3)
                                        {
                                            Task_estendstartmonthname = "Mar";
                                        }

                                        if (task_estendstrtmonth == 4)
                                        {
                                            Task_estendstartmonthname = "Apr";
                                        }

                                        if (task_estendstrtmonth == 5)
                                        {
                                            Task_estendstartmonthname = "May";
                                        }

                                        if (task_estendstrtmonth == 6)
                                        {
                                            Task_estendstartmonthname = "Jun";
                                        }

                                        if (task_estendstrtmonth == 7)
                                        {
                                            Task_estendstartmonthname = "Jul";
                                        }

                                        if (task_estendstrtmonth == 8)
                                        {
                                            Task_estendstartmonthname = "Aug";
                                        }

                                        if (task_estendstrtmonth == 9)
                                        {
                                            Task_estendstartmonthname = "Sep";
                                        }

                                        if (task_estendstrtmonth == 10)
                                        {
                                            Task_estendstartmonthname = "Oct";
                                        }

                                        if (task_estendstrtmonth == 11)
                                        {
                                            Task_estendstartmonthname = "Nov";
                                        }

                                        if (task_estendstrtmonth == 12)
                                        {
                                            Task_estendstartmonthname = "Dec";
                                        }

                                        //end days months 

                                        task_estendenddys = task_estendTargetdate.Day;
                                        int task_estendendmonth = task_estendTargetdate.Month;
                                        if (task_estendendmonth == 1)
                                        {
                                            task_estendendmonthname = "Jan";
                                        }
                                        if (task_estendendmonth == 2)
                                        {
                                            task_estendendmonthname = "Feb";
                                        }
                                        if (task_estendendmonth == 3)
                                        {
                                            task_estendendmonthname = "Mar";
                                        }

                                        if (task_estendendmonth == 4)
                                        {
                                            task_estendendmonthname = "Apr";
                                        }

                                        if (task_estendendmonth == 5)
                                        {
                                            task_estendendmonthname = "May";
                                        }

                                        if (task_estendendmonth == 6)
                                        {
                                            task_estendendmonthname = "Jun";
                                        }

                                        if (task_estendendmonth == 7)
                                        {
                                            task_estendendmonthname = "Jul";
                                        }

                                        if (task_estendendmonth == 8)
                                        {
                                            task_estendendmonthname = "Aug";
                                        }

                                        if (task_estendendmonth == 9)
                                        {
                                            task_estendendmonthname = "Sep";
                                        }

                                        if (task_estendendmonth == 10)
                                        {
                                            task_estendendmonthname = "Oct";
                                        }

                                        if (task_estendendmonth == 11)
                                        {
                                            task_estendendmonthname = "Nov";
                                        }

                                        if (task_estendendmonth == 12)
                                        {
                                            task_estendendmonthname = "Dec";
                                        }



                                        task_extenddatebind += "<a href = 'javascript:void(0);' class='timesheet-wrap'>";
                                        task_extenddatebind += "<div class='progress progress-xl bg-dark position-relative me-1' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + task_estendststartdys + " " + Task_estendstartmonthname + " - " + task_estendenddys + " " + task_estendendmonthname + " (" + task_estendleftDays.ToString().Replace("-", "") + " " + task_estendleftdlydys + ")'>";
                                        task_extenddatebind += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 100%;' aria-valuemax='100'>" + task_estendDays + " days</div>";
                                        task_extenddatebind += "</div>";
                                        task_extenddatebind += "</a>";



                                    }

                                }




                                taskdata += "<td>";
                                taskdata += "<div class='d-flex'>";
                                taskdata += "<div class='flex-grow-1 align-self-center me-2'>";
                                taskdata += "<div class='extended_time' id='lblExtnddttask_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>";

                                taskdata += "" + task_extenddatebind + "";

                                taskdata += "</div>";
                                taskdata += "</div>";

                                if (leftDays < 0)
                                {
                                    taskdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                                    taskdata += "<a href ='javascript:void(0);' type='button' id='btnextndttask_" + ds.Tables[0].Rows[i]["id"].ToString() + "'  onclick='Bindextenddatetask_Details(" + ds.Tables[0].Rows[i]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#ExtendedTimline_task'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Extended Day'>+</span></a>";
                                    taskdata += "</div>";
                                }

                                taskdata += "</div>";
                                taskdata += "</div>";
                                taskdata += "</td>";


                                taskdata += "<td valign='top'>";
                                taskdata += cmf.dateConvert(ds.Tables[0].Rows[i]["StartDate"].ToString()); // start date
                                taskdata += "</td>";

                                taskdata += "<td valign='top'>";
                                taskdata += cmf.dateConvert(ds.Tables[0].Rows[i]["Targetdate"].ToString()); //target date
                                taskdata += "</td>";


                                // status

                                string strvalcolor = get_Status_Color(ds.Tables[0].Rows[i]["P_Status"].ToString());
                                taskdata += "<td>";
                                taskdata += "<div class='btn-group d-block'>";
                                taskdata += "<a href ='javascript:void(0);' id='btnsts_" + ds.Tables[0].Rows[i]["id"].ToString() + "' type='button' onclick=setprojectid('" + ds.Tables[0].Rows[i]["id"].ToString() + "',this.id) data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + strvalcolor + "'>" + get_Projectstatus(ds.Tables[0].Rows[i]["P_Status"].ToString()) + "</a>";
                                taskdata += "<div class='dropdown-menu lead_form_body p-0'>";
                                taskdata += retval;
                                taskdata += "</div>";
                                taskdata += "</div>";
                                taskdata += "</td>";




                                //priority
                                string strprioritycolor = get_PriorityColor(ds.Tables[0].Rows[i]["Priority"].ToString());
                                taskdata += "<td>";
                                taskdata += "<div class='btn-group d-block'>";
                                taskdata += "<a href ='javascript:void(0);' id='btnpri_" + ds.Tables[0].Rows[i]["id"].ToString() + "' type='button' onclick=setpriorityid('" + ds.Tables[0].Rows[i]["id"].ToString() + "',this.id) data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + strprioritycolor + "'>" + get_PriorityName(ds.Tables[0].Rows[i]["Priority"].ToString()) + "</a>";
                                taskdata += "<div class='dropdown-menu lead_form_body p-0'>";
                                taskdata += strvalprority;
                                taskdata += "</div>";
                                taskdata += "</div>";
                                taskdata += "</td>";

                                //Issues

                                taskdata += "<td valign='top'>";

                                float completetepercentissues = 0;
                                float Pendingepercentissues = 0;
                                completetepercentissues = Issuescomplite * 100 / IssuesPhaseTotal;
                                Pendingepercentissues = Issuespending * 100 / IssuesPhaseTotal;

                                taskdata += "<div class='d-flex'>";
                                taskdata += "<div class='flex-grow-1 align-self-center'>";

                                taskdata += "<div class='progress progress-xl cursor-pointer'>";
                                taskdata += "<div class='progress-bar bg-success' role='progressbar' style='width: " + fns.ToFormattedCurrencyString(completetepercentissues.ToString()) + "%' aria-valuemin='0' aria-valuemax='100' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + fns.ToFormattedCurrencyString(completetepercentissues.ToString()) + "% [" + Issuescomplite + " of " + IssuesPhaseTotal + " Issues Completed]'>" + Issuescomplite + "</div>";

                                taskdata += "<div class='progress-bar bg-warning' role='progressbar' style='width: " + fns.ToFormattedCurrencyString(Pendingepercentissues.ToString()) + "%' aria-valuemin='0' aria-valuemax='100' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + fns.ToFormattedCurrencyString(Pendingepercentissues.ToString()) + "% [" + Issuespending + " of " + IssuesPhaseTotal + " Issues Not Completed]'>" + Issuespending + "</div>";
                                taskdata += "</div>";
                                taskdata += "</div>";

                                taskdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                                taskdata += "<a href ='javascript:void(0);' type='button' data-bs-toggle='offcanvas' data-bs-target=#canvas_popup onclick=loadsrc('inner_popup','/pms/AddIssuesPopup.aspx?id=" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "&milstoneid=" + ds.Tables[0].Rows[i]["Milestoneid"].ToString() + "&tasklistid=" + ds.Tables[0].Rows[i]["tasklist"].ToString() + "&taskid=" + ds.Tables[0].Rows[i]["id"].ToString() + "&type=task')><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Issue'>+</span></a>";

                                taskdata += "</div>";
                                taskdata += "</td>";

                                //taskdata += "<td valign='top'>";
                                //taskdata += ds.Tables[0].Rows[i]["Budget"].ToString(); // budget 
                                //taskdata += "</td>";

                                //taskdata += "<td valign='top'>";
                                //taskdata += ds.Tables[0].Rows[i]["Actual"].ToString(); // Actual 
                                //taskdata += "</td>";
                                

                                taskdata += "<td valign='top'>";
                                taskdata += ds.Tables[0].Rows[i]["TaskDuration"].ToString(); //Task Duration Hours
                                taskdata += "</td>";

                                

                                // Stage
                                string strvalstagecolor = get_Stage_Color(ds.Tables[0].Rows[i]["stage"].ToString());
                                taskdata += "<td>";
                                taskdata += "<div class='btn-group d-block'>";
                                taskdata += "<a href ='javascript:void(0);' id='btnStage_" + ds.Tables[0].Rows[i]["id"].ToString() + "' type='button' onclick=setStageid('" + ds.Tables[0].Rows[i]["id"].ToString() + "',this.id) data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + strvalstagecolor + "'>" + get_Stagename(ds.Tables[0].Rows[i]["stage"].ToString()) + "</a>";
                                taskdata += "<div class='dropdown-menu lead_form_body p-0'>";
                                taskdata += retstageval;
                                taskdata += "</div>";
                                taskdata += "</div>";
                                taskdata += "</td>";



                                


                               
                              

                                

                                string Workinghors = "0";
                                string strworkinghrs = "select  sum(cast(isnull(hour,0) as numeric)) Workhors  from Pms_tbl_Timerstatus where taskid='" + ds.Tables[0].Rows[i]["id"].ToString() + "' and status=1 and isnull(IsEnd,0)=1 and archive=0 and type='task'";
                                DataSet dsworkghrs = da.GetDataSetInline(strworkinghrs);
                                if (dsworkghrs.Tables[0].Rows.Count > 0)
                                {

                                    Workinghors = dsworkghrs.Tables[0].Rows[0]["Workhors"].ToString();

                                }

                             

                                taskdata += "</tr>";



                                string strsubstask = "select * from Pms_tbl_Addsubtask where ProjectId='" + dsproject.Tables[0].Rows[P]["id"].ToString() + "' and Milestoneid='" + ds1.Tables[0].Rows[j]["id"].ToString() + "' and task='" + ds.Tables[0].Rows[i]["id"].ToString() + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                                DataSet dssubtsk = da.GetDataSetInline(strsubstask);
                                if (dssubtsk.Tables[0].Rows.Count > 0)
                                {

                                    for (int sb = 0; sb < dssubtsk.Tables[0].Rows.Count; sb++)
                                    {

                                        string sbtskcntownr = "0";
                                        string sbtskusersdetailsowner = "";
                                        string sbtskownertooltip = "";
                                        if (dssubtsk.Tables[0].Rows[sb]["owner"].ToString() != "")
                                        {
                                            string[] arcnt = dssubtsk.Tables[0].Rows[sb]["owner"].ToString().Trim().Split(',');
                                            sbtskcntownr = arcnt.Length.ToString();
                                            DataSet dsusers = new DataSet();
                                            if (dssubtsk.Tables[0].Rows[sb]["owner"].ToString().Trim() != "")
                                            {
                                                dsusers = da.GetDataSetInline("select id,DisplayName,fileupload1,SUBSTRING (DisplayName, 1, 2) as splinameone from tbl_employee where id in(" + dssubtsk.Tables[0].Rows[sb]["owner"].ToString().Trim() + ")");
                                                if (dsusers.Tables[0].Rows.Count > 3)
                                                {
                                                    sbtskcntownr = (Convert.ToInt32(sbtskcntownr) - 3).ToString();

                                                    // owner tool tip //
                                                    for (int lT = 3; lT < dsusers.Tables[0].Rows.Count; lT++)
                                                    {
                                                        if (dsusers.Tables[0].Rows[lT]["DisplayName"].ToString() != string.Empty)
                                                        {
                                                            if (sbtskownertooltip != string.Empty)
                                                            {
                                                                sbtskownertooltip += "," + dsusers.Tables[0].Rows[lT]["DisplayName"].ToString();
                                                            }
                                                            else
                                                            {
                                                                sbtskownertooltip += dsusers.Tables[0].Rows[lT]["DisplayName"].ToString();
                                                            }
                                                        }

                                                    }

                                                    for (int U = 0; U < 3; U++)
                                                    {
                                                        if (dsusers.Tables[0].Rows[U]["fileupload1"].ToString() != "")
                                                        {
                                                            sbtskusersdetailsowner += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title=" + dsusers.Tables[0].Rows[U]["DisplayName"].ToString() + " data-bs-original-title='" + dsusers.Tables[0].Rows[U]["DisplayName"].ToString() + "'><img src = '" + dsusers.Tables[0].Rows[U]["fileupload1"] + "' alt='' class='rounded-circle avatar-xxs'></a>";
                                                        }
                                                        else
                                                        {
                                                            sbtskusersdetailsowner += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover'" +
                                         " data-bs-placement='top' title='" + dsusers.Tables[0].Rows[U]["DisplayName"].ToString() + "' data-bs-original-title='" + dsusers.Tables[0].Rows[U]["DisplayName"].ToString() + "'>";
                                                            sbtskusersdetailsowner += @"<div class='avatar-xxs'><div class='avatar-title rounded-circle bg-light text-primary'>" + dsusers.Tables[0].Rows[U]["splinameone"].ToString() + "</div></div>";
                                                            sbtskusersdetailsowner += @"</a>";
                                                        }
                                                    }

                                                }
                                                else
                                                {
                                                    sbtskcntownr = "";
                                                    for (int U = 0; U < dsusers.Tables[0].Rows.Count; U++)
                                                    {
                                                        if (dsusers.Tables[0].Rows[U]["fileupload1"].ToString() != "")
                                                        {
                                                            //img src='/assets/images/avatar-1.jpg'
                                                            sbtskusersdetailsowner += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='' data-bs-original-title='" + dsusers.Tables[0].Rows[U]["DisplayName"].ToString() + "'><img src = '" + dsusers.Tables[0].Rows[U]["fileupload1"] + "' alt='' class='rounded-circle avatar-xxs'></a>";
                                                        }
                                                        else
                                                        {
                                                            sbtskusersdetailsowner += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover'  data-bs-placement='top' title='" + dsusers.Tables[0].Rows[U]["DisplayName"].ToString() + "' data-bs-original-title='" + dsusers.Tables[0].Rows[U]["DisplayName"].ToString() + "'>";
                                                            sbtskusersdetailsowner += @"<div class='avatar-xxs'>
                            <div class='avatar-title rounded-circle bg-light text-primary'>" + dsusers.Tables[0].Rows[U]["splinameone"].ToString() + "</div></div>";
                                                            sbtskusersdetailsowner += @"</a>";

                                                        }
                                                    }
                                                }

                                            }
                                        }


                                        string sbtskcnt = "0";
                                        string sbtskusersdetails = "";
                                        string sbtskteamtooltip = "";
                                        if (dssubtsk.Tables[0].Rows[sb]["team"].ToString() != "")
                                        {
                                            string[] arcnt = dssubtsk.Tables[0].Rows[sb]["team"].ToString().Trim().Split(',');
                                            sbtskcnt = arcnt.Length.ToString();
                                            DataSet dsusers = new DataSet();
                                            if (dssubtsk.Tables[0].Rows[sb]["team"].ToString().Trim() != "")
                                            {
                                                dsusers = da.GetDataSetInline("select id,DisplayName,fileupload1,SUBSTRING (DisplayName, 1, 2) as splinameone from tbl_employee where id in(" + dssubtsk.Tables[0].Rows[sb]["team"].ToString().Trim() + ")");
                                                if (dsusers.Tables[0].Rows.Count > 3)
                                                {
                                                    sbtskcnt = (Convert.ToInt32(sbtskcnt) - 3).ToString();

                                                    // team tool tip //
                                                    for (int sl = 3; sl < dsusers.Tables[0].Rows.Count; sl++)
                                                    {
                                                        if (dsusers.Tables[0].Rows[sl]["DisplayName"].ToString() != string.Empty)
                                                        {
                                                            if (sbtskteamtooltip != string.Empty)
                                                            {
                                                                sbtskteamtooltip += "," + dsusers.Tables[0].Rows[sl]["DisplayName"].ToString();
                                                            }
                                                            else
                                                            {
                                                                sbtskteamtooltip += dsusers.Tables[0].Rows[sl]["DisplayName"].ToString();
                                                            }
                                                        }

                                                    }

                                                    for (int V = 0; V < 3; V++)
                                                    {
                                                        if (dsusers.Tables[0].Rows[V]["fileupload1"].ToString() != "")
                                                        {
                                                            sbtskusersdetails += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title=" + dsusers.Tables[0].Rows[V]["DisplayName"].ToString() + " data-bs-original-title='" + dsusers.Tables[0].Rows[V]["DisplayName"].ToString() + "'><img src = '" + dsusers.Tables[0].Rows[V]["fileupload1"] + "' alt='' class='rounded-circle avatar-xxs'></a>";
                                                        }
                                                        else
                                                        {
                                                            sbtskusersdetails += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover'" +
                                         " data-bs-placement='top' title='" + dsusers.Tables[0].Rows[V]["DisplayName"].ToString() + "' data-bs-original-title='" + dsusers.Tables[0].Rows[V]["DisplayName"].ToString() + "'>";
                                                            sbtskusersdetails += @"<div class='avatar-xxs'><div class='avatar-title rounded-circle bg-light text-primary'>" + dsusers.Tables[0].Rows[V]["splinameone"].ToString() + "</div></div>";
                                                            sbtskusersdetails += @"</a>";
                                                        }
                                                    }

                                                }
                                                else
                                                {
                                                    sbtskcnt = "";
                                                    for (int sV = 0; sV < dsusers.Tables[0].Rows.Count; sV++)
                                                    {
                                                        if (dsusers.Tables[0].Rows[sV]["fileupload1"].ToString() != "")
                                                        {
                                                            //img src='/assets/images/avatar-1.jpg'
                                                            sbtskusersdetails += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='' data-bs-original-title='" + dsusers.Tables[0].Rows[sV]["DisplayName"].ToString() + "'><img src = '" + dsusers.Tables[0].Rows[sV]["fileupload1"] + "' alt='' class='rounded-circle avatar-xxs'></a>";
                                                        }
                                                        else
                                                        {
                                                            sbtskusersdetails += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover'  data-bs-placement='top' title='" + dsusers.Tables[0].Rows[sV]["DisplayName"].ToString() + "' data-bs-original-title='" + dsusers.Tables[0].Rows[sV]["DisplayName"].ToString() + "'>";
                                                            sbtskusersdetails += @"<div class='avatar-xxs'>
                            <div class='avatar-title rounded-circle bg-light text-primary'>" + dsusers.Tables[0].Rows[sV]["splinameone"].ToString() + "</div></div>";
                                                            sbtskusersdetails += @"</a>";

                                                        }
                                                    }
                                                }

                                            }
                                        }



                                        //   completion  status  start //

                                        float subtaskWattagecomplite = 0;
                                        float subtaskWattagepending = 100;
                                        float subtaskprojectTotal = 0;
                                        float subtaskPhaseTotal = 0;
                                        float subtaskTaskTotal = 0;
                                        int subtaskcheckiscomplete = 0;

                                        string subtaskstr = "select isnull(iscomplete,0) as iscomplete, * from Pms_tbl_Addsubtask where ProjectId='" + dsproject.Tables[0].Rows[P]["id"] + "' and Milestoneid='" + ds1.Tables[0].Rows[j]["id"] + "' and task='" + ds.Tables[0].Rows[i]["id"] + "' and id='" + dssubtsk.Tables[0].Rows[sb]["id"] + "' and status=1 and archive=0 ";
                                        DataSet subtaskdss = da.GetDataSetInline(subtaskstr);
                                        if (subtaskdss.Tables[0].Rows.Count > 0)
                                        {
                                            subtaskcheckiscomplete = Convert.ToInt32(subtaskdss.Tables[0].Rows[0]["iscomplete"].ToString());
                                            if (subtaskcheckiscomplete == 1)
                                            {
                                                subtaskprojectTotal = Convert.ToInt32(subtaskdss.Tables[0].Rows[0]["iscompletepercentage"].ToString());
                                            }


                                        }

                                        if (subtaskprojectTotal > 0)
                                        {
                                            subtaskWattagecomplite = subtaskprojectTotal;
                                            subtaskWattagepending = Math.Abs(subtaskWattagepending - subtaskWattagecomplite);
                                        }


                                        //   completion  status  end //



                                        //   Issues count start //

                                        float sbtskIssuescomplite = 0;
                                        float sbtskIssuespending = 0;
                                        float sbtskIssuesrojectTotal = 0;
                                        float sbtskIssuesPhaseTotal = 0;
                                        float sbttskIssues_Total = 0;

                                        string sbtskstr111 = "select COUNT(*) as countIssues from Pms_tbl_Addissues where ProjectId='" + dsproject.Tables[0].Rows[P]["id"].ToString() + "' and Milestoneid='" + ds1.Tables[0].Rows[j]["id"].ToString() + "' and  taskid='" + ds.Tables[0].Rows[i]["id"].ToString() + "' and  subtaskid='" + dssubtsk.Tables[0].Rows[sb]["id"] + "' and Issueon='subtask' and status=1 and archive=0 ";
                                        DataSet sbtskdss111 = da.GetDataSetInline(sbtskstr111);
                                        if (sbtskdss111.Tables[0].Rows.Count > 0)
                                        {

                                            try
                                            {
                                                sbtskIssuesPhaseTotal = Convert.ToInt32(sbtskdss111.Tables[0].Rows[0]["countIssues"]);
                                            }
                                            catch (Exception)
                                            {
                                            }
                                        }


                                        string sbtskfindtask111 = "select COUNT(*) as count_Issues from Pms_tbl_Addissues where ProjectId='" + dsproject.Tables[0].Rows[P]["id"].ToString() + "' and Milestoneid='" + ds1.Tables[0].Rows[j]["id"].ToString() + "' and  taskid='" + ds.Tables[0].Rows[i]["id"].ToString() + "' and  subtaskid='" + dssubtsk.Tables[0].Rows[sb]["id"] + "' and Issueon='subtask' and IssueStatus in ( select id from Pms_tbl_Projectstatus where  status=1 and archive=0 and isnull(iscomplete,0)=1)  and status=1 and archive=0";
                                        DataSet sbtskdsssumtask111 = da.GetDataSetInline(sbtskfindtask111);
                                        if (sbtskdsssumtask111.Tables[0].Rows.Count > 0)
                                        {
                                            try
                                            {
                                                sbttskIssues_Total = Convert.ToInt32(sbtskdsssumtask111.Tables[0].Rows[0]["count_Issues"]);
                                            }
                                            catch (Exception)
                                            {

                                            }
                                        }


                                        sbtskIssuesrojectTotal = (sbtskIssuesPhaseTotal - sbttskIssues_Total);


                                        if (sbtskIssuesrojectTotal > 0)
                                        {
                                            sbtskIssuescomplite = sbttskIssues_Total;
                                            sbtskIssuespending = Math.Abs(sbtskIssuesPhaseTotal - sbttskIssues_Total);
                                        }

                                        if (sbtskIssuesrojectTotal == 0)
                                        {
                                            sbtskIssuescomplite = sbttskIssues_Total;
                                            sbtskIssuespending = Math.Abs(sbtskIssuesPhaseTotal - sbttskIssues_Total);
                                        }

                                        // Issues count end //



                                        taskdata += "<tr class='subtaskshow_" + ds.Tables[0].Rows[i]["id"].ToString() + "' style='display:none;'>";

                                        //sr no

                                        taskdata += "<td valign='top'>";
                                        taskdata += "";
                                        taskdata += "</td>";

                                        taskdata += " <td><a href='javascript:void(0);' Class='btn btn-info btn-sm' type='button' onclick='Binddetailssubtask(" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + ")' data-bs-toggle='offcanvas' data-bs-target='#addsubtask'><i class='ri-edit-line'></i></a>";
                                        taskdata += "&nbsp;&nbsp;<a href='javascript:void(0);' Class='btn btn-danger btn-sm' type='button' onclick='deletedatasubtask(" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + ")'><i class='ri-delete-bin-5-line'></i></a>";
                                        taskdata += "</td>";


                                        //subtask name
                                        taskdata += "<td valign='top'>";
                                        taskdata += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + (j + 1) + "." + (i + 1) + "." + (sb + 1) + "  " + dssubtsk.Tables[0].Rows[sb]["Subtaskname"].ToString();
                                        //taskdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                                       // taskdata += "<a href = 'javascript:void(0);' type='button' onclick='AddMultitask(" + ds1.Tables[0].Rows[j]["id"].ToString() + "," + dsproject.Tables[0].Rows[P]["id"].ToString() + ")' data-bs-toggle='offcanvas' data-bs-target='#addtask'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add sub Task'>+</span></a>";
                                        //taskdata += "</div>";
                                     
                                        taskdata += "</td>";

                                        //sub task completion status start 
                                        taskdata += "<td>";
                                        if (subtaskWattagecomplite == 0)
                                        {

                                            taskdata += "<div class='progress progress-xl cursor-pointer'>";
                                            taskdata += "<div class='progress-bar bg-warning' data-bs-toggle='tooltip' data-bs-placement='top' title='Pending(100%)' role='progressbar' style='width:100%'><span style='text-align:center;'>100%</span>";
                                            taskdata += "</div>";
                                            taskdata += "</div>";

                                        }

                                        else
                                        {

                                            taskdata += "<div class='progress progress-xl cursor-pointer'>";
                                            taskdata += "<div class='progress-bar bg-success' data-bs-toggle='tooltip' data-bs-placement='top' title='Complete (" + subtaskWattagecomplite + "%)' role='progressbar' style='width:" + subtaskWattagecomplite + "%'>" + subtaskWattagecomplite + "%";
                                            taskdata += "</div>";
                                            taskdata += "<div class='progress-bar bg-warning' data-toggle='tooltip' data-bs-placemen='top' title='Pending (" + subtaskWattagepending + "%)' role='progressbar' style='width:" + subtaskWattagepending + "%'>" + subtaskWattagepending + "%";
                                            taskdata += "</div>";
                                            taskdata += "</div>";

                                        }

                                        taskdata += "</td>";

                                        // Weightage 
                                        taskdata += "<td valign='top'>";
                                        taskdata += fns.ToFormattedCurrencyString(dssubtsk.Tables[0].Rows[sb]["Weightage"].ToString()); 
                                        taskdata += "</td>";

                                        
                                        //Owner

                                        taskdata += "<td>";
                                        taskdata += "<div class='d-flex'>";
                                        taskdata += "<div class='flex-grow-1'>";
                                        taskdata += "<div class='avatar-group' id='sbtsklblowner_" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "'>";
                                        taskdata += sbtskusersdetailsowner;

                                        taskdata += "<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='" + sbtskownertooltip + "' data-bs-original-title='more'>";

                                        if (sbtskcntownr != "" && sbtskcntownr != "0")
                                        {
                                            taskdata += "<div class='avatar-xxs'>";
                                            taskdata += "<div class='avatar-title rounded-circle'>";

                                            taskdata += (sbtskcntownr + "+");
                                            taskdata += "</div>";
                                            taskdata += "</div>";
                                        }

                                        taskdata += "</a>";
                                        taskdata += "</div>";
                                        taskdata += "</div>";
                                        taskdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                                        taskdata += "<a href ='javascript:void(0);' type='button' id='sbtskbtnownr_" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "' onclick='sbtskBinddetails_Owner(" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#sbtskchangeowner'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Owner'>+</span></a>";
                                        taskdata += "</div>";
                                        taskdata += "</div>";
                                        taskdata += "</td>";


                                        //team
                                        taskdata += "<td>";
                                        taskdata += "<div class='d-flex'>";
                                        taskdata += "<div class='flex-grow-1'>";
                                        taskdata += "<div class='avatar-group' id='sbtsklbluser_" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "'>";
                                        taskdata += sbtskusersdetails;

                                        taskdata += "<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='" + sbtskteamtooltip + "' data-bs-original-title='more'>";

                                        if (sbtskcnt != "" && sbtskcnt != "0")
                                        {
                                            taskdata += "<div class='avatar-xxs'>";
                                            taskdata += "<div class='avatar-title rounded-circle'>";

                                            taskdata += (sbtskcnt + "+");
                                            taskdata += "</div>";
                                            taskdata += "</div>";
                                        }

                                        taskdata += "</a>";
                                        taskdata += "</div>";
                                        taskdata += "</div>";
                                        taskdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                                        taskdata += "<a href ='javascript:void(0);' type='button' id='sbtskbtnteam_" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "' onclick='sbtskBinddetails_Team(" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#sbtskaddteam'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Team'>+</span></a>";
                                        taskdata += "</div>";
                                        taskdata += "</div>";
                                        taskdata += "</td>";

                                        //Timeline

                                        //Storing input Dates  
                                        DateTime sbtskStartDate = Convert.ToDateTime(dssubtsk.Tables[0].Rows[sb]["StartDate"].ToString() == "" ? DateTime.Now.ToString() : dssubtsk.Tables[0].Rows[sb]["StartDate"].ToString());
                                        DateTime sbtskTargetdate = Convert.ToDateTime(dssubtsk.Tables[0].Rows[sb]["Targetdate"].ToString() == "" ? DateTime.Now.ToString() : dssubtsk.Tables[0].Rows[sb]["Targetdate"].ToString());
                                        TimeSpan sbtskobjTimeSpan = sbtskTargetdate - sbtskStartDate;
                                        double sbtskDays = Convert.ToDouble(sbtskobjTimeSpan.TotalDays);
                                        sbtskDays = sbtskDays + 1;

                                        DateTime sbtskEnddate = Convert.ToDateTime(dssubtsk.Tables[0].Rows[sb]["Targetdate"].ToString() == "" ? DateTime.Now.ToString() : dssubtsk.Tables[0].Rows[sb]["Targetdate"].ToString());
                                        DateTime sbtskCurrentdate = DateTime.Now;
                                        TimeSpan sbtskobjtimeleftspn = sbtskEnddate - sbtskCurrentdate;
                                        double sbtskleftDays = Convert.ToDouble(sbtskobjtimeleftspn.Days);
                                        sbtskleftDays = sbtskleftDays + 1;

                                        string sbtskleftdlydys = "";
                                        if (sbtskleftDays < 0)
                                        {
                                            sbtskleftdlydys = "days delayed";
                                        }
                                        else
                                        {
                                            sbtskleftdlydys = "days left";
                                        }


                                        //start days months  
                                        string sbtskstartmonthname = "";
                                        int sbtskststartdys = sbtskStartDate.Day;
                                        int sbtskstrtmonth = sbtskStartDate.Month;
                                        int subtaskstartyear = sbtskStartDate.Year;
                                        if (sbtskstrtmonth == 1)
                                        {
                                            sbtskstartmonthname = "Jan";
                                        }
                                        if (sbtskstrtmonth == 2)
                                        {
                                            sbtskstartmonthname = "Feb";
                                        }
                                        if (sbtskstrtmonth == 3)
                                        {
                                            sbtskstartmonthname = "Mar";
                                        }

                                        if (sbtskstrtmonth == 4)
                                        {
                                            sbtskstartmonthname = "Apr";
                                        }

                                        if (sbtskstrtmonth == 5)
                                        {
                                            sbtskstartmonthname = "May";
                                        }

                                        if (sbtskstrtmonth == 6)
                                        {
                                            sbtskstartmonthname = "Jun";
                                        }

                                        if (sbtskstrtmonth == 7)
                                        {
                                            sbtskstartmonthname = "Jul";
                                        }

                                        if (sbtskstrtmonth == 8)
                                        {
                                            sbtskstartmonthname = "Aug";
                                        }

                                        if (sbtskstrtmonth == 9)
                                        {
                                            sbtskstartmonthname = "Sep";
                                        }

                                        if (sbtskstrtmonth == 10)
                                        {
                                            sbtskstartmonthname = "Oct";
                                        }

                                        if (sbtskstrtmonth == 11)
                                        {
                                            sbtskstartmonthname = "Nov";
                                        }

                                        if (sbtskstrtmonth == 12)
                                        {
                                            sbtskstartmonthname = "Dec";
                                        }



                                        //end days months 
                                        string sbtskendmonthname = "";
                                        int sbtskenddys = sbtskTargetdate.Day;
                                        int sbtskendmonth = sbtskTargetdate.Month;
                                        int subtaskEndyear = sbtskTargetdate.Year;
                                        if (sbtskendmonth == 1)
                                        {
                                            sbtskendmonthname = "Jan";
                                        }
                                        if (sbtskendmonth == 2)
                                        {
                                            sbtskendmonthname = "Feb";
                                        }
                                        if (sbtskendmonth == 3)
                                        {
                                            sbtskendmonthname = "Mar";
                                        }

                                        if (sbtskendmonth == 4)
                                        {
                                            sbtskendmonthname = "Apr";
                                        }

                                        if (sbtskendmonth == 5)
                                        {
                                            sbtskendmonthname = "May";
                                        }

                                        if (sbtskendmonth == 6)
                                        {
                                            sbtskendmonthname = "Jun";
                                        }

                                        if (endmonth == 7)
                                        {
                                            sbtskendmonthname = "Jul";
                                        }

                                        if (sbtskendmonth == 8)
                                        {
                                            sbtskendmonthname = "Aug";
                                        }

                                        if (sbtskendmonth == 9)
                                        {
                                            sbtskendmonthname = "Sep";
                                        }

                                        if (sbtskendmonth == 10)
                                        {
                                            sbtskendmonthname = "Oct";
                                        }

                                        if (sbtskendmonth == 11)
                                        {
                                            sbtskendmonthname = "Nov";
                                        }

                                        if (sbtskendmonth == 12)
                                        {
                                            sbtskendmonthname = "Dec";
                                        }





                                        taskdata += "<td>";
                                        taskdata += "<a href = 'javascript:void(0);'>";
                                        taskdata += "<div class='progress progress-xl bg-dark position-relative' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + sbtskleftDays.ToString().Replace("-", "") + " " + sbtskleftdlydys + "'>";
                                        taskdata += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 100%;' aria-valuemax='100'></div>";
                                        taskdata += "<div class='dayswrapper'>";
                                        taskdata += "<span class='fromtodate'>" + sbtskststartdys + " " + sbtskstartmonthname + " " + subtaskstartyear +  " - " + sbtskenddys + " " + sbtskendmonthname  + " "+ subtaskEndyear + "</span>";
                                        taskdata += "<span class='totaldays'>" + sbtskDays + " days</span>";
                                        taskdata += "</div>";
                                        taskdata += "</div>";
                                        taskdata += "</a>";
                                        taskdata += "</td>";





                                        //Sub Task extended timeleine 

                                        string subtask_estendleftdlydys = "";
                                        string subTask_estendstartmonthname = "";
                                        string subtask_estendendmonthname = "";
                                        double subtask_estendleftDays = 0;
                                        int subtask_estendststartdys = 0;
                                        int subtask_estendenddys = 0;
                                        double subtask_estendDays = 0;
                                        string subtask_extenddatebind = "";
                                        string subtask_strextend = "select id,extendStartdate as StartDate,extendEnddate as TargetDate from Pms_Tbl_Extended_Timeline where  status=1 and Type='subtask' and archive=0  and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and subtaskid = '" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "'  ";
                                        DataSet subtask_dsstrextend = da.GetDataSetInline(subtask_strextend);
                                        if (subtask_dsstrextend.Tables[0].Rows.Count > 0)
                                        {
                                            for (int ST = 0; ST < subtask_dsstrextend.Tables[0].Rows.Count; ST++)
                                            {
                                                DateTime subtask_estendStartDate = Convert.ToDateTime(subtask_dsstrextend.Tables[0].Rows[ST]["StartDate"].ToString() == "" ? DateTime.Now.ToString() : subtask_dsstrextend.Tables[0].Rows[ST]["StartDate"].ToString());
                                                DateTime subtask_estendTargetdate = Convert.ToDateTime(subtask_dsstrextend.Tables[0].Rows[ST]["TargetDate"].ToString() == "" ? DateTime.Now.ToString() : subtask_dsstrextend.Tables[0].Rows[ST]["TargetDate"].ToString());
                                                TimeSpan subtask_estendobjTimeSpan = subtask_estendTargetdate - subtask_estendStartDate;
                                                subtask_estendDays = Convert.ToDouble(subtask_estendobjTimeSpan.TotalDays);
                                                subtask_estendDays = subtask_estendDays + 1;


                                                DateTime subtask_estendEnddate = Convert.ToDateTime(subtask_dsstrextend.Tables[0].Rows[ST]["Targetdate"].ToString() == "" ? DateTime.Now.ToString() : subtask_dsstrextend.Tables[0].Rows[ST]["Targetdate"].ToString());
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




                                        taskdata += "<td>";
                                        taskdata += "<div class='d-flex'>";
                                        taskdata += "<div class='flex-grow-1 align-self-center me-2'>";
                                        taskdata += "<div class='extended_time' id='lblExtnddtsubtask_" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "'>";

                                        taskdata += "" + subtask_extenddatebind + "";

                                        taskdata += "</div>";
                                        taskdata += "</div>";

                                        if (leftDays < 0)
                                        {
                                            taskdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                                            taskdata += "<a href ='javascript:void(0);' type='button' id='btnextndtsubtask_" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "'  onclick='Bindextenddatesubtask_Details(" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#ExtendedTimline_subtask'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Extended Day'>+</span></a>";
                                            taskdata += "</div>";
                                        }

                                        taskdata += "</div>";
                                        taskdata += "</div>";
                                        taskdata += "</td>";


                                        // start date
                                        taskdata += "<td valign='top'>";
                                        taskdata += cmf.dateConvert(dssubtsk.Tables[0].Rows[sb]["Startdate"].ToString()); 
                                        taskdata += "</td>";

                                        //End date
                                        taskdata += "<td valign='top'>";
                                        taskdata += cmf.dateConvert(dssubtsk.Tables[0].Rows[sb]["Targetdate"].ToString()); 
                                        taskdata += "</td>";


                                        //status
                                        string sbtskstrvalcolor = get_Status_Color(dssubtsk.Tables[0].Rows[sb]["P_Status"].ToString());
                                        taskdata += "<td>";
                                        taskdata += "<div class='btn-group d-block'>";
                                        taskdata += "<a href ='javascript:void(0);' id='btnsts_" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "' type='button' onclick=setsubtaskstustid('" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "',this.id) data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + sbtskstrvalcolor + "'>" + get_Projectstatus(dssubtsk.Tables[0].Rows[sb]["P_Status"].ToString()) + "</a>";
                                        taskdata += "<div class='dropdown-menu lead_form_body p-0'>";
                                        taskdata += sbtskretval;
                                        taskdata += "</div>";
                                        taskdata += "</div>";
                                        taskdata += "</td>";


                                        //priority
                                        string sbtskstrprioritycolor = get_PriorityColor(dssubtsk.Tables[0].Rows[sb]["Priority"].ToString());
                                        taskdata += "<td>";
                                        taskdata += "<div class='btn-group d-block'>";
                                        taskdata += "<a href ='javascript:void(0);' id='btnpri_" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "' type='button' onclick=sbtsksetpriorityid('" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "',this.id) data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + sbtskstrprioritycolor + "'>" + get_PriorityName(dssubtsk.Tables[0].Rows[sb]["Priority"].ToString()) + "</a>";
                                        taskdata += "<div class='dropdown-menu lead_form_body p-0'>";
                                        taskdata += sbtskstrvalprority;
                                        taskdata += "</div>";
                                        taskdata += "</div>";
                                        taskdata += "</td>";

                                        //Issues
                                        taskdata += "<td valign='top'>";
                                        float sbtskcompletetepercentissues = 0;
                                        float sbtskPendingepercentissues = 0;
                                        sbtskcompletetepercentissues = sbtskIssuescomplite * 100 / sbtskIssuesPhaseTotal;
                                        Pendingepercentissues = sbtskIssuespending * 100 / sbtskIssuesPhaseTotal;

                                        taskdata += "<div class='d-flex'>";
                                        taskdata += "<div class='flex-grow-1 align-self-center'>";
                                        taskdata += "<div class='progress progress-xl cursor-pointer'>";
                                        taskdata += "<div class='progress-bar bg-success' role='progressbar' style='width: " + fns.ToFormattedCurrencyString(sbtskcompletetepercentissues.ToString()) + "%' aria-valuemin='0' aria-valuemax='100' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + fns.ToFormattedCurrencyString(sbtskcompletetepercentissues.ToString()) + "% [" + sbtskIssuescomplite + " of " + sbtskIssuesPhaseTotal + " Issues Completed]'>" + sbtskIssuescomplite + "</div>";
                                        taskdata += "<div class='progress-bar bg-warning' role='progressbar' style='width: " + fns.ToFormattedCurrencyString(sbtskPendingepercentissues.ToString()) + "%' aria-valuemin='0' aria-valuemax='100' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + fns.ToFormattedCurrencyString(sbtskPendingepercentissues.ToString()) + "% [" + sbtskIssuespending + " of " + sbtskIssuesPhaseTotal + " Issues Not Completed]'>" + sbtskIssuespending + "</div>";
                                        taskdata += "</div>";
                                        taskdata += "</div>";
                                        taskdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                                        taskdata += "<a href ='javascript:void(0);' type='button' data-bs-toggle='offcanvas' data-bs-target=#canvas_popup onclick=loadsrc('inner_popup','/pms/AddIssuesPopup.aspx?id=" + dsproject.Tables[0].Rows[P]["id"].ToString() + "&milstoneid=" + ds1.Tables[0].Rows[j]["id"].ToString() + "&tasklistid=" + dssubtsk.Tables[0].Rows[sb]["tasklist"].ToString() + "&taskid=" + ds.Tables[0].Rows[i]["id"].ToString() + "&subtaskid=" + dssubtsk.Tables[0].Rows[sb]["id"] + "&type=subtask')><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Issue'>+</span></a>";
                                        taskdata += "</div>";
                                        taskdata += "</td>";


                                        // sub task duration hour
                                        taskdata += "<td valign='top'>";
                                        taskdata += dssubtsk.Tables[0].Rows[sb]["TaskDuration"].ToString();
                                        taskdata += "</td>";


                                        // Stage
                                        string sbtskstrvalstagecolor = get_Stage_Color(dssubtsk.Tables[0].Rows[sb]["stage"].ToString());
                                        taskdata += "<td>";
                                        taskdata += "<div class='btn-group d-block'>";
                                        taskdata += "<a href ='javascript:void(0);' id='btnStage_" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "' type='button' onclick=sbtsksetStageid('" + dssubtsk.Tables[0].Rows[sb]["id"].ToString() + "',this.id) data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + sbtskstrvalstagecolor + "'>" + get_Stagename(dssubtsk.Tables[0].Rows[sb]["stage"].ToString()) + "</a>";
                                        taskdata += "<div class='dropdown-menu lead_form_body p-0'>";
                                        taskdata += sbtskretstageval;
                                        taskdata += "</div>";
                                        taskdata += "</div>";
                                        taskdata += "</td>";


                                        //taskdata += "<td valign='top'>";
                                        //taskdata += "";
                                        //taskdata += "</td>";

                                       




                                        taskdata += "</tr>";
                                    }





                                }





                            }

                            


                        }

                        #endregion

                    }


                }

            }


            
        }

        taskdata += "</tbody>";
        taskdata += "</table>";

        return taskdata;
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


    [WebMethod]
    public static string Bindtaskdetail(int RecordId = 0)
    {
        DataAccess da = new DataAccess();
        DataSet ds;
        string strsql = "select * from Pms_tbl_Addtask where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
        ds = da.GetDataSetInline(strsql);

        return ds.GetXml();
    }


    [WebMethod]
    public static string Bindcheckweitagedetails(int RecordId = 0)
    {
        DataAccess da = new DataAccess();
        DataSet ds;
        string strsql = "select * from Pms_tbl_AddMilestone where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
        ds = da.GetDataSetInline(strsql);

        return ds.GetXml();
    }

    [WebMethod]
    public static string Bindcheckweitagedetails_subtask(int RecordId = 0)
    {
        DataAccess da = new DataAccess();
        DataSet ds;
        string strsql = "select * from Pms_tbl_Addtask where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
        ds = da.GetDataSetInline(strsql);

        return ds.GetXml();
    }

    [WebMethod]
    public static string BindcheckTaskdurationdetails(int Milestoneid = 0)
    {
        DataAccess da = new DataAccess();
        DataSet ds;
        //string strsql = "select sum(cast(isnull(Pms_tbl_Addtask.TaskDuration,0) as decimal)) as TaskDuration,isnull(Pms_tbl_AddMilestone.Durationhourcal, 0) as Durationhourcal  from Pms_tbl_Addtask inner join Pms_tbl_AddMilestone on Pms_tbl_Addtask.Milestoneid = Pms_tbl_AddMilestone.id where  Pms_tbl_Addtask.status = 1 and Pms_tbl_Addtask.archive = 0 and Pms_tbl_Addtask.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and Pms_tbl_Addtask.Milestoneid = '" + Milestoneid + "'  group by Pms_tbl_AddMilestone.Durationhourcal";
        string strsql = "select sum(cast(isnull(Pms_tbl_Addtask.TaskDuration,0) as decimal)) as TaskDuration,isnull(Pms_tbl_AddMilestone.Durationhourcal, 0) as Durationhourcal from Pms_tbl_AddMilestone left join Pms_tbl_Addtask on Pms_tbl_AddMilestone.id = Pms_tbl_Addtask.Milestoneid where Pms_tbl_AddMilestone.status = 1 and Pms_tbl_AddMilestone.archive = 0 and Pms_tbl_AddMilestone.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and Pms_tbl_AddMilestone.id = '" + Milestoneid + "' group by Pms_tbl_AddMilestone.Durationhourcal";
        ds = da.GetDataSetInline(strsql);
        return ds.GetXml();
    }


    [WebMethod]
    public static string Bindtaskdetail_Owner(int RecordId = 0)
    {

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "select * from Pms_tbl_Addtask where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
        ds = da.GetDataSetInline(strsql);


        return ds.GetXml();
    }


    [WebMethod]
    public static string BindNotesDetails(int ProjectId = 0, int Milestoneid = 0, int tasklistid = 0, int taskid = 0)
    {

        DataAccess da = new DataAccess();
        DataSet ds;

        //string strsql = "select * from Pms_tbl_Addtask where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  and ProjectId='" + ProjectId + "' and Milestoneid='" + Milestoneid + "' and tasklist='" + tasklistid + "' and id='" + taskid + "'";

        string strsql = "select timer.id,task.ProjectId,task.Milestoneid,task.tasklist,task.id as taskid,timer.Notes from Pms_tbl_Addtask task left join Pms_tbl_Timerstatus as timer on timer.ProjectId = task.ProjectId and timer.Milestoneid = task.Milestoneid and task.tasklist = timer.tasklist and task.id = timer.taskid where task.status = 1 and task.archive = 0 and task.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and task.ProjectId = '" + ProjectId + "' and task.Milestoneid = '" + Milestoneid + "' and task.tasklist = '" + tasklistid + "' and task.id ='" + taskid + "'";

        ds = da.GetDataSetInline(strsql);


        return ds.GetXml();
    }

    [WebMethod]
    public static string BindTimerDetails(int ProjectId = 0, int Milestoneid = 0, int tasklistid = 0, int taskid = 0)
    {

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "select timer.id,task.ProjectId,task.Milestoneid,task.tasklist,task.id as taskid,timer.hour,timer.minute,timer.second,timer.millisecond,timer.TimerNotes,timer.StartTime,timer.EndTime from Pms_tbl_Addtask task left join Pms_tbl_Timerstatus as timer on timer.ProjectId = task.ProjectId and timer.Milestoneid = task.Milestoneid and task.tasklist = timer.tasklist and task.id = timer.taskid where task.status = 1 and task.archive = 0 and task.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  and task.id ='" + taskid + "'";

        ds = da.GetDataSetInline(strsql);


        return ds.GetXml();
    }

    [WebMethod]
    public static string Deletetaskdata(int RecordId = 0)
    {
        string retval = "0";

        DataAccess da = new DataAccess();
        DataSet ds;

     


        string findmilstoneid = "select * from Pms_tbl_Addtask where id=" + RecordId + " and status=1  and archive=0";
        DataSet dsmilstoneid = da.GetDataSetInline(findmilstoneid);


        int checkweitage = 0;
        decimal wetagecount = 0;
        decimal totalwetage = 0;
        string strweightage = "select isnull(checkmileWeightage ,0) as checkmileWeightage  from Pms_tbl_AddMilestone where id='" + dsmilstoneid.Tables[0].Rows[0]["Milestoneid"] + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
        DataSet dsweitagecheck = da.GetDataSetInline(strweightage);
        if (dsweitagecheck.Tables[0].Rows.Count > 0)
        {

            checkweitage = Convert.ToInt32(dsweitagecheck.Tables[0].Rows[0]["checkmileWeightage"]);

        }

        if (checkweitage == 1)
        {
            string strweightagecount = "select count (*) as WeitageCount from Pms_tbl_Addtask where Milestoneid='" + dsmilstoneid.Tables[0].Rows[0]["Milestoneid"] + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
            DataSet dsweitagecount = da.GetDataSetInline(strweightagecount);
            if (dsweitagecount.Tables[0].Rows.Count > 0)
            {

                wetagecount = Convert.ToInt32(dsweitagecount.Tables[0].Rows[0]["WeitageCount"]);

            }

            totalwetage = 100 / wetagecount;
            string strstatusupdate = "update Pms_tbl_Addtask set Weightage='" + totalwetage.ToString() + "' where Milestoneid='" + dsmilstoneid.Tables[0].Rows[0]["Milestoneid"] + "' and status=1 and archive=0";
            DataSet dshourmin = da.GetDataSetInline(strstatusupdate);

        }

        int iscomplete = 0;
        string finsql1 = "select isnull(iscomplete,0) as iscomplete, * from Pms_tbl_Addtask where id=" + RecordId + " and status=1 and archive=0";
        DataSet dsfindid1 = da.GetDataSetInline(finsql1);
        if (dsfindid1.Tables[0].Rows.Count > 0)
        {

            iscomplete = Convert.ToInt32(dsfindid1.Tables[0].Rows[0]["iscomplete"]);

        }

        if (iscomplete == 1)
        {
            decimal tsktotalpercent = 100;
            decimal totlweitagemile = 0;
            decimal totlweitageproject = 0;

            string finsql01 = "select * from Pms_tbl_Addtask where id=" + RecordId + " and status=1  and archive=0";
            DataSet dsfindid01 = da.GetDataSetInline(finsql01);

            // task percentage //
            string strstatusupdate = "update Pms_tbl_Addtask set iscompletepercentage='" + tsktotalpercent.ToString() + "' where id='" + RecordId + "' and status=1 and archive=0 and isnull(iscomplete,0)=1";
            DataSet dshourmin = da.GetDataSetInline(strstatusupdate);

            // Milestone percentage //
            string finsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "'   and status=1  and archive=0";
            DataSet dsfindmile = da.GetDataSetInline(finsqlmile);
            for (int j = 0; j < dsfindmile.Tables[0].Rows.Count; j++)
            {
                decimal sumweitage = 0;
                decimal subpercentageemile = 0;
                sumweitage = Convert.ToDecimal(dsfindmile.Tables[0].Rows[j]["Weightage"].ToString());
                subpercentageemile = Convert.ToDecimal(dsfindmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                totlweitagemile += (sumweitage * subpercentageemile) / 100;

            }

            string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile.ToString() + "' where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and id='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da.GetDataSetInline(finsql4);


            // project percentage //
            string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "'   and status=1  and archive=0";
            DataSet dsfindidproject = da.GetDataSetInline(finsqlproject);
            for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
            {
                decimal sumweitageprojt = 0;
                decimal subpercentageeproject = 0;
                sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

            }

            string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
            DataSet dsfindidproject1 = da.GetDataSetInline(finsqlproject1);




        }
        else
        {
            decimal tsktotalpercent = 0;
            decimal totlweitagemile = 0;
            decimal totlweitageproject = 0;

            string finsql01 = "select * from Pms_tbl_Addtask where id=" + RecordId + " and status=1  and archive=0";
            DataSet dsfindid01 = da.GetDataSetInline(finsql01);


            if (dsfindid01.Tables[0].Rows.Count > 0)
            {
                if (dsfindid01.Tables[0].Rows[0]["issubtask"].ToString() == "1")
                {

                    string finsql211 = "select isnull(sum(weightage),0)Wattagetasksum from Pms_tbl_Addsubtask where   task='" + RecordId + "'  and status=1 and isnull(iscomplete,0)=1 and archive=0";
                    DataSet dsfindid211 = da.GetDataSetInline(finsql211);
                    if (dsfindid211.Tables[0].Rows.Count > 0)
                    {
                        string strstatusupdate = "update Pms_tbl_Addtask set iscompletepercentage='" + dsfindid211.Tables[0].Rows[0]["Wattagetasksum"] + "' where id='" + retval + "' and status=1 and archive=0";
                        DataSet dshourmin = da.GetDataSetInline(strstatusupdate);
                    }
                }
                else
                {
                    string strstatusupdate = "update Pms_tbl_Addtask set iscompletepercentage='0' where id='" + RecordId + "' and status=1 and archive=0";
                    DataSet dshourmin = da.GetDataSetInline(strstatusupdate);

                }

            }

            // task percentage //
            //string strstatusupdate = "update Pms_tbl_Addtask set iscompletepercentage='" + tsktotalpercent.ToString() + "' where id='" + retval + "' and status=1 and archive=0 and isnull(iscomplete,0)=0";
            //DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

            // Milestone percentage //
            string finsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "'   and status=1  and archive=0";
            DataSet dsfindmile = da.GetDataSetInline(finsqlmile);
            for (int j = 0; j < dsfindmile.Tables[0].Rows.Count; j++)
            {
                decimal sumweitage = 0;
                decimal subpercentageemile = 0;
                sumweitage = Convert.ToDecimal(dsfindmile.Tables[0].Rows[j]["Weightage"].ToString());
                subpercentageemile = Convert.ToDecimal(dsfindmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                totlweitagemile += (sumweitage * subpercentageemile) / 100;

            }

            string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile.ToString() + "' where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and id='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da.GetDataSetInline(finsql4);

            // project percentage //
            string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "'   and status=1  and archive=0";
            DataSet dsfindidproject = da.GetDataSetInline(finsqlproject);
            for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
            {
                decimal sumweitageprojt = 0;
                decimal subpercentageeproject = 0;
                sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

            }

            string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
            DataSet dsfindidproject1 = da.GetDataSetInline(finsqlproject1);



        }

        string strsql = "update  Pms_tbl_Addtask set  status=0,archive=1  where  id='" + RecordId + "' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        ds = da.GetDataSetInline(strsql);


        return retval;
    }

    [WebMethod]
    public static string Deletesubtaskdata(int RecordId = 0)
    {
        string retval = "0";

        DataAccess da = new DataAccess();
        DataSet ds;
        string strsql = "";
        string chkpid = "0";
        string chkmid = "0";
        string chktid = "0";
        int iscomplete = 0;
        int checkweitage = 0;
        decimal wetagecount = 0;
        decimal totalwetage = 0;

        string findtaskid = "select  isnull(iscomplete,0) as iscomplete,* from Pms_tbl_Addsubtask where id=" + RecordId + " and status=1  and archive=0";
        DataSet dstaskid = da.GetDataSetInline(findtaskid);
        if(dstaskid.Tables[0].Rows.Count > 0)
        {
            chkpid = dstaskid.Tables[0].Rows[0]["ProjectId"].ToString();
            chkmid = dstaskid.Tables[0].Rows[0]["Milestoneid"].ToString();
            chktid = dstaskid.Tables[0].Rows[0]["task"].ToString();
            iscomplete = Convert.ToInt32(dstaskid.Tables[0].Rows[0]["iscomplete"]);
        }
        strsql = "update  Pms_tbl_Addsubtask set  status=0,archive=1  where  id='" + RecordId + "' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        ds = da.GetDataSetInline(strsql);

        
        string strweightage = "select isnull(checkTaskWeightage ,0) as checkTaskWeightage  from pms_tbl_addtask where id='" + dstaskid.Tables[0].Rows[0]["task"].ToString() + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
        DataSet dsweitagecheck = da.GetDataSetInline(strweightage);
        if (dsweitagecheck.Tables[0].Rows.Count > 0)
        {

            checkweitage = Convert.ToInt32(dsweitagecheck.Tables[0].Rows[0]["checkTaskWeightage"]);

        }
        //SaveLogText("testA1="+ checkweitage);
       
        if (checkweitage == 1)
        {
            string strweightagecount = "select count (*) as WeitageCount from Pms_tbl_Addsubtask where task='" + dstaskid.Tables[0].Rows[0]["task"] + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
           // SaveLogText("testA2=" + strweightagecount);
            DataSet dsweitagecount = da.GetDataSetInline(strweightagecount);
            if (dsweitagecount.Tables[0].Rows.Count > 0)
            {

                wetagecount = Convert.ToInt32(dsweitagecount.Tables[0].Rows[0]["WeitageCount"]);

            }
            //SaveLogText("testA3=" + wetagecount.ToString());
            totalwetage = 100 / wetagecount;
            string strstatusupdate = "update Pms_tbl_Addsubtask set Weightage='" + totalwetage.ToString() + "' where task='" + dstaskid.Tables[0].Rows[0]["task"] + "' and status=1 and archive=0";
            //SaveLogText("testA4=" + strstatusupdate);
            DataSet dshourmin = da.GetDataSetInline(strstatusupdate);

        }


        
        //string finsql1 = "select  isnull(iscomplete,0) as iscomplete,* from Pms_tbl_Addsubtask where id=" + RecordId + " and status=1  and archive=0";
        //DataSet dsfindid1 = da.GetDataSetInline(finsql1);
        //if (dsfindid1.Tables[0].Rows.Count > 0)
        //{

        //    iscomplete = Convert.ToInt32(dsfindid1.Tables[0].Rows[0]["iscomplete"]);

        //}

        if (iscomplete == 1)
        {
            decimal subtsktotalpercent = 100;
            decimal totlweitagemile = 0;
            decimal totlweitageproject = 0;

            //string finsql01 = "select * from Pms_tbl_Addsubtask where id=" + RecordId + " and status=1  and archive=0";
            //DataSet dsfindid01 = da.GetDataSetInline(finsql01);

            string finsql2 = "select isnull(sum(weightage),0)Wattagetasksum from Pms_tbl_Addsubtask where ProjectId='" + chkpid + "' and Milestoneid='" + chkmid + "' and task='" + chktid + "'  and status=1 and isnull(iscomplete,0)=1 and archive=0";
            DataSet dsfindid2 = da.GetDataSetInline(finsql2);

            //sub task percentage //
            string strstatusupdate = "update Pms_tbl_Addsubtask set iscompletepercentage='" + subtsktotalpercent.ToString() + "' where id='" + RecordId + "' and status=1 and archive=0 and isnull(iscomplete,0)=1";
            DataSet dshourmin = da.GetDataSetInline(strstatusupdate);

            // task percentage //
            string finsql3 = "update Pms_tbl_Addtask set iscompletepercentage='" + dsfindid2.Tables[0].Rows[0]["Wattagetasksum"] + "' where ProjectId='" + chkpid + "' and Milestoneid='" + chkmid + "' and id='" + chktid + "' and status=1 and archive=0";
            DataSet dsfindid3 = da.GetDataSetInline(finsql3);


            // Milestone percentage //
            string findsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask  where ProjectId='" + chkpid + "' and Milestoneid='" + chkmid + "'  and status=1  and archive=0";
            DataSet dsfindidmile = da.GetDataSetInline(findsqlmile);
            for (int j = 0; j < dsfindidmile.Tables[0].Rows.Count; j++)
            {
                decimal sumweitage = 0;
                decimal subpercentageemile = 0;
                sumweitage = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["Weightage"].ToString());
                subpercentageemile = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                totlweitagemile += (sumweitage * subpercentageemile) / 100;

            }

            string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile + "' where ProjectId='" + chkpid + "' and id='" + chkmid + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da.GetDataSetInline(finsql4);



            // project percentage //
            string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + chkpid + "' and status=1  and archive=0";
            DataSet dsfindidproject = da.GetDataSetInline(finsqlproject);
            for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
            {
                decimal sumweitageprojt = 0;
                decimal subpercentageeproject = 0;
                sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

            }

            string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + chkpid + "'  and status=1 and archive=0";
            DataSet dsfindidproject1 = da.GetDataSetInline(finsqlproject1);






        }

        else
        {
            decimal subtsktotalpercent = 0;
            decimal totlweitagemile = 0;
            decimal totlweitageproject = 0;

           // string finsql01 = "select * from Pms_tbl_Addsubtask where id=" + RecordId + " and status=1 and archive=0";
            //DataSet dsfindid01 = da.GetDataSetInline(finsql1);

            string finsql2 = "select isnull(sum(weightage),0)Wattagetasksum from Pms_tbl_Addsubtask where ProjectId='" + chkpid + "' and Milestoneid='" + chkmid + "' and task='" + chktid + "'  and status=1 and isnull(iscomplete,0)=1 and archive=0";
            DataSet dsfindid2 = da.GetDataSetInline(finsql2);

            //Sub task percentage //
            string strstatusupdate = "update Pms_tbl_Addsubtask set iscompletepercentage='" + subtsktotalpercent.ToString() + "' where id='" + RecordId + "' and status=1 and archive=0 and isnull(iscomplete,0)=0";
            DataSet dshourmin = da.GetDataSetInline(strstatusupdate);

            // task percentage //
            string finsql3 = "update Pms_tbl_Addtask set iscompletepercentage='" + dsfindid2.Tables[0].Rows[0]["Wattagetasksum"] + "' where ProjectId='" + chkpid + "' and Milestoneid='" + chkmid + "' and id='" + chktid + "' and status=1 and archive=0";
            DataSet dsfindid3 = da.GetDataSetInline(finsql3);


            // Milestone percentage //
            string findsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask  where ProjectId='" + chkpid + "' and Milestoneid='" + chkmid + "'    and status=1  and archive=0";
            DataSet dsfindidmile = da.GetDataSetInline(findsqlmile);
            for (int j = 0; j < dsfindidmile.Tables[0].Rows.Count; j++)
            {
                decimal sumweitage = 0;
                decimal subpercentageemile = 0;
                sumweitage = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["Weightage"].ToString());
                subpercentageemile = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                totlweitagemile += (sumweitage * subpercentageemile) / 100;

            }

            string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile + "' where ProjectId='" + chkpid + "' and id='" + chkmid + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da.GetDataSetInline(finsql4);


            // project percentage //
            string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + chkpid + "'   and status=1  and archive=0";
            DataSet dsfindidproject = da.GetDataSetInline(finsqlproject);
            for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
            {
                decimal sumweitageprojt = 0;
                decimal subpercentageeproject = 0;
                sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

            }

            string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + chkpid + "'  and status=1 and archive=0";
            DataSet dsfindidproject1 = da.GetDataSetInline(finsqlproject1);


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
                    "<a class='' href='javascript:void(0);' onclick = 'UpdateDataStatusdeatils(" + dscode.Tables[0].Rows[i]["id"].ToString() + "," + dscode.Tables[0].Rows[i]["iscomplete"].ToString() + ")' > " +
                    "<i class='bi bi-square-fill' style='color:" + dscode.Tables[0].Rows[i]["PrintColor"].ToString() + "' ></i> " + dscode.Tables[0].Rows[i]["ProjectStatus"].ToString() + "" +
                    "</a>" +
                    "</div>" +
                    "</li>";


            }
            strret += "</ul>";
        }

        return strret;

    }
    protected static string Projget_status_listbind()
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
                    "<a class='' href='javascript:void(0);' onclick = 'ProjUpdateDataStatusdeatils(" + dscode.Tables[0].Rows[i]["id"].ToString() + "," + dscode.Tables[0].Rows[i]["iscomplete"].ToString() + ")' > " +
                    "<i class='bi bi-square-fill' style='color:" + dscode.Tables[0].Rows[i]["PrintColor"].ToString() + "' ></i> " + dscode.Tables[0].Rows[i]["ProjectStatus"].ToString() + "" +
                    "</a>" +
                    "</div>" +
                    "</li>";


            }
            strret += "</ul>";
        }

        return strret;

    }

    protected static string Mileget_status_listbind()
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
                    "<a class='' href='javascript:void(0);' onclick = 'MileUpdateDataStatusdeatils(" + dscode.Tables[0].Rows[i]["id"].ToString() + "," + dscode.Tables[0].Rows[i]["iscomplete"].ToString() + ")' > " +
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

    protected static string sbtskget_status_listbind()
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
                    "<a class='' href='javascript:void(0);' onclick = 'sbtskUpdateDataStatusdeatils(" + dscode.Tables[0].Rows[i]["id"].ToString() + " ," + dscode.Tables[0].Rows[i]["iscomplete"].ToString() + ")' > " +
                    "<i class='bi bi-square-fill' style='color:" + dscode.Tables[0].Rows[i]["PrintColor"].ToString() + "' ></i> " + dscode.Tables[0].Rows[i]["ProjectStatus"].ToString() + "" +
                    "</a>" +
                    "</div>" +
                    "</li>";


            }
            strret += "</ul>";
        }

        return strret;

    }

    protected static string sbtskget_Stage_listbind()
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
                    "<a class='' href='javascript:void(0);' onclick = 'sbtskUpdateDataStagedeatils(" + dscode.Tables[0].Rows[i]["id"].ToString() + ")' > " +
                    "<i class='bi bi-square-fill' style='color:" + dscode.Tables[0].Rows[i]["PrintColor"].ToString() + "' ></i> " + dscode.Tables[0].Rows[i]["name"].ToString() + "" +
                    "</a>" +
                    "</div>" +
                    "</li>";


            }
            strret += "</ul>";
        }

        return strret;

    }
    protected static string sbtskget_Priority_listbind()
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
                    "<a class='' href='javascript:void(0);' onclick = 'sbtskUpdateDatapropertydeatils(" + dscode.Tables[0].Rows[i]["id"].ToString() + ")' > " +
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
            string strsql = "select * from Pms_tbl_Addtask where id = '" + splitstr[i] + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0";
            DataSet ds2 = da.GetDataSetInline(strsql);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                if (ds2.Tables[0].Rows[0]["taskname"].ToString() != string.Empty)
                {
                    if (stname != string.Empty)
                    {
                        stname += "," + ds2.Tables[0].Rows[0]["taskname"].ToString();
                    }
                    else
                    {
                        stname += ds2.Tables[0].Rows[0]["taskname"].ToString();
                    }
                }

            }
        }
        return stname;
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

    protected static string Mileget_Priority_listbind()
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
                    "<a class='' href='javascript:void(0);' onclick = 'MileUpdateDatapropertydeatils(" + dscode.Tables[0].Rows[i]["id"].ToString() + ")' > " +
                    "<i class='bi bi-square-fill' style='color:" + dscode.Tables[0].Rows[i]["PrintColor"].ToString() + "' ></i> " + dscode.Tables[0].Rows[i]["name"].ToString() + "" +
                    "</a>" +
                    "</div>" +
                    "</li>";


            }
            strret += "</ul>";
        }

        return strret;


    }
    protected static string Projget_Priority_listbind()
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
                    "<a class='' href='javascript:void(0);' onclick = 'ProjUpdateDatapropertydeatils(" + dscode.Tables[0].Rows[i]["id"].ToString() + ")' > " +
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
        string strsql = " select * from Pms_tbl_Priority where status=1 and archive=0 and Id='" + projectid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
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
    [WebMethod]
    public static string addnewmilestoneblank(string addon, string currentindex,string pid,string orderno)
    {
        string retval = "0";

        DataAccess da1 = new DataAccess();
        DataSet ds;
        string strsql = "";
        try
        {
            SqlParameter[] prm = new SqlParameter[23];
            prm[0] = new SqlParameter("@id", "0");
            prm[1] = new SqlParameter("@ProjectId", pid);
            prm[2] = new SqlParameter("@Milestone", DBNull.Value);
            prm[3] = new SqlParameter("@Weightage", DBNull.Value);
            prm[4] = new SqlParameter("@StartDate", DBNull.Value);
            prm[5] = new SqlParameter("@EndDate", DBNull.Value);
            prm[6] = new SqlParameter("@Owner", DBNull.Value);
            prm[7] = new SqlParameter("@Access", DBNull.Value);
            prm[8] = new SqlParameter("@team", DBNull.Value);
            prm[9] = new SqlParameter("@P_Status", DBNull.Value);
            prm[10] = new SqlParameter("@Priority", DBNull.Value);
            prm[11] = new SqlParameter("@Budget", DBNull.Value);
            prm[12] = new SqlParameter("@Actual", DBNull.Value);
            prm[13] = new SqlParameter("@checkmileWeightage", DBNull.Value);
            prm[14] = new SqlParameter("@Durationtype", DBNull.Value);
            prm[15] = new SqlParameter("@TaskDuration", DBNull.Value);
            prm[16] = new SqlParameter("@istask", DBNull.Value);
            prm[17] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[18] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prm[19] = new SqlParameter("@cdate", da1.getcdatetime());
            prm[20] = new SqlParameter("@type", "Insert");
            if (orderno != "")
            {
                if(orderno=="0")
                {
                    orderno = "1";
                }
                prm[21] = new SqlParameter("@orderno", orderno);
            }
            else
            {
                prm[21] = new SqlParameter("@orderno", DBNull.Value);

            }
            prm[22] = new SqlParameter("ReturnValue", SqlDbType.Int);
            prm[22].Direction = ParameterDirection.ReturnValue;
            int retval1 = da1.executeDMLQuery("pmstbl_Addmilestone_usp", prm);
            retval = retval1.ToString();
        }
        catch
        { }

        return retval;
    }
    [WebMethod]
    public static string UpdateCommondata(commondataG commondataG)
    {
        function fns = new function();
        string retval = "0";
        DataAccess da1 = new DataAccess();
        string entrytype = "Insert";
        string editid = "0";
        string strsqlids = "";
        string statusid = "0";
        string priorityid = "0";

        
        if (commondataG.id!=""&& commondataG.id!="0")
        {
            editid = commondataG.id;
            entrytype = "Update";
        }       
        
        if(commondataG.updatetype=="milestone")
        {            
            if (commondataG.hdnnewid != "")
            {
                entrytype = "Insert";
                try
                {
                    strsqlids = "select * from Pms_tbl_Projectstatus where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                    DataSet dsId = da1.GetDataSetInline(strsqlids);
                    if (dsId.Tables[0].Rows.Count > 0)
                    {
                        statusid = dsId.Tables[0].Rows[0]["id"].ToString();
                    }

                }
                catch { }
                try
                {
                    strsqlids = "select * from Pms_tbl_Priority where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                    DataSet dsId = da1.GetDataSetInline(strsqlids);
                    if (dsId.Tables[0].Rows.Count > 0)
                    {
                        priorityid = dsId.Tables[0].Rows[0]["id"].ToString();
                    }

                }
                catch { }
            }
            SqlParameter[] prm = new SqlParameter[13];
            prm[0] = new SqlParameter("@id", editid);
            prm[1] = new SqlParameter("@ProjectId", commondataG.projectid);
            prm[2] = new SqlParameter("@Milestone", commondataG.txtname);
            if (commondataG.start_date == string.Empty)
            {
                prm[3] = new SqlParameter("@StartDate", DBNull.Value);
            }
            else
            {
                prm[3] = new SqlParameter("@StartDate", commondataG.start_date);
            }
            if (commondataG.end_date == string.Empty)
            {
                prm[4] = new SqlParameter("@EndDate", DBNull.Value);
            }
            else
            {
                prm[4] = new SqlParameter("@EndDate", commondataG.end_date);
            }
            prm[5] = new SqlParameter("@P_Status", statusid);
            prm[6] = new SqlParameter("@Priority",priorityid);
            prm[7] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[8] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prm[9] = new SqlParameter("@cdate", da1.getcdatetime());
            prm[10] = new SqlParameter("@type", entrytype);
            prm[11] = new SqlParameter("@miledescription", commondataG.description);
            prm[12] = new SqlParameter("ReturnValue", SqlDbType.Int);
            prm[12].Direction = ParameterDirection.ReturnValue;
            int retval1 = da1.executeDMLQuery("pmstbl_Addmilestone_uspG", prm);
            retval = retval1.ToString();
        }
        else if (commondataG.updatetype == "task")
        {
            string mileid = "0";
            if (commondataG.hdnnewid != "")
            {
                entrytype = "Insert";
                try
                {
                    strsqlids = "select * from Pms_tbl_Projectstatus where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                    DataSet dsId = da1.GetDataSetInline(strsqlids);
                    if (dsId.Tables[0].Rows.Count > 0)
                    {
                        statusid = dsId.Tables[0].Rows[0]["id"].ToString();
                    }

                }
                catch { }
                try
                {
                    strsqlids = "select * from Pms_tbl_Priority where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                    DataSet dsId = da1.GetDataSetInline(strsqlids);
                    if (dsId.Tables[0].Rows.Count > 0)
                    {
                        priorityid = dsId.Tables[0].Rows[0]["id"].ToString();
                    }

                }
                catch { }
                try
                {
                    strsqlids = "select * from Pms_tbl_AddMilestone where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='"+ commondataG.id+ "'";
                    DataSet dsId = da1.GetDataSetInline(strsqlids);
                    if (dsId.Tables[0].Rows.Count > 0)
                    {
                        mileid = dsId.Tables[0].Rows[0]["id"].ToString();
                    }

                }
                catch { }
            }
            SqlParameter[] prm = new SqlParameter[18];
            prm[0] = new SqlParameter("@id", editid);
            prm[1] = new SqlParameter("@ProjectId", commondataG.projectid);
            prm[2] = new SqlParameter("@Milestoneid", mileid);
            prm[3] = new SqlParameter("@tasklist", DBNull.Value);
            prm[4] = new SqlParameter("@taskname", commondataG.txtname);
            prm[5] = new SqlParameter("@taskdescripton", commondataG.description);
            if (commondataG.start_date == string.Empty)
            {
                prm[6] = new SqlParameter("@StartDate", DBNull.Value);
            }
            else
            {
                prm[6] = new SqlParameter("@StartDate", commondataG.start_date);
            }
            if (commondataG.end_date == string.Empty)
            {
                prm[7] = new SqlParameter("@Targetdate", DBNull.Value);
            }
            else
            {
                prm[7] = new SqlParameter("@Targetdate", commondataG.end_date);
            }

            if (commondataG.start_time == string.Empty)
            {
                prm[8] = new SqlParameter("@StartTime", DBNull.Value);
            }
            else
            {
                prm[8] = new SqlParameter("@StartTime", commondataG.start_time);
            }
            if (commondataG.end_time == string.Empty)
            {
                prm[9] = new SqlParameter("@Timeline", DBNull.Value);
            }
            else
            {
                prm[9] = new SqlParameter("@Timeline", commondataG.end_time);
            }

            prm[10] = new SqlParameter("@TaskDuration", DBNull.Value);
            prm[11] = new SqlParameter("@P_Status", statusid);
            prm[12] = new SqlParameter("@Priority", priorityid);
            prm[13] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[14] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prm[15] = new SqlParameter("@cdate", da1.getcdatetime());
            prm[16] = new SqlParameter("@type", entrytype);
            prm[17] = new SqlParameter("ReturnValue", SqlDbType.Int);
            prm[17].Direction = ParameterDirection.ReturnValue;
            int retval1 = da1.executeDMLQuery("pms_Adtask_uspG", prm);
            retval = retval1.ToString();
        }
        else if (commondataG.updatetype == "subtask")
        {
            string mileid = "0";
            string taskid = "0";
            if (commondataG.hdnnewid != "")
            {
                entrytype = "Insert";
                try
                {
                    strsqlids = "select * from Pms_tbl_Projectstatus where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                    DataSet dsId = da1.GetDataSetInline(strsqlids);
                    if (dsId.Tables[0].Rows.Count > 0)
                    {
                        statusid = dsId.Tables[0].Rows[0]["id"].ToString();
                    }

                }
                catch { }
                try
                {
                    strsqlids = "select * from Pms_tbl_Priority where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                    DataSet dsId = da1.GetDataSetInline(strsqlids);
                    if (dsId.Tables[0].Rows.Count > 0)
                    {
                        priorityid = dsId.Tables[0].Rows[0]["id"].ToString();
                    }

                }
                catch { }
                try
                {
                    strsqlids = "select * from Pms_tbl_Addtask where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  and id='" + commondataG.id + "'";
                    DataSet dsId = da1.GetDataSetInline(strsqlids);
                    if (dsId.Tables[0].Rows.Count > 0)
                    {
                        mileid = dsId.Tables[0].Rows[0]["Milestoneid"].ToString();
                        taskid = dsId.Tables[0].Rows[0]["id"].ToString();
                    }

                }
                catch { }
            }
            SqlParameter[] prm = new SqlParameter[19];
            prm[0] = new SqlParameter("@id", editid);
            prm[1] = new SqlParameter("@ProjectId", commondataG.projectid);
            prm[2] = new SqlParameter("@Milestoneid", mileid);
            prm[3] = new SqlParameter("@tasklist", DBNull.Value);
            prm[4] = new SqlParameter("@task", taskid);
            prm[5] = new SqlParameter("@SubTaskDescription", commondataG.description);
            if (commondataG.start_date == string.Empty)
            {
                prm[6] = new SqlParameter("@StartDate", DBNull.Value);
            }
            else
            {
                prm[6] = new SqlParameter("@StartDate", commondataG.start_date);
            }
            if (commondataG.end_date == string.Empty)
            {
                prm[7] = new SqlParameter("@Targetdate", DBNull.Value);
            }
            else
            {
                prm[7] = new SqlParameter("@Targetdate", commondataG.end_date);
            }

            if (commondataG.start_time == string.Empty)
            {
                prm[8] = new SqlParameter("@StartTime", DBNull.Value);
            }
            else
            {
                prm[8] = new SqlParameter("@StartTime", commondataG.start_time);
            }
            if (commondataG.end_time == string.Empty)
            {
                prm[9] = new SqlParameter("@Timeline", DBNull.Value);
            }
            else
            {
                prm[9] = new SqlParameter("@Timeline", commondataG.end_time);
            }

            prm[10] = new SqlParameter("@TaskDuration", DBNull.Value);
            prm[11] = new SqlParameter("@P_Status", statusid);
            prm[12] = new SqlParameter("@Priority", priorityid);
            prm[13] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[14] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prm[15] = new SqlParameter("@cdate", da1.getcdatetime());
            prm[16] = new SqlParameter("@type", entrytype);
            prm[17] = new SqlParameter("@Subtaskname", commondataG.txtname);
            prm[18] = new SqlParameter("ReturnValue", SqlDbType.Int);
            prm[18].Direction = ParameterDirection.ReturnValue;
            int retval1 = da1.executeDMLQuery("pms_Add_Subtask_uspG", prm);
            retval = retval1.ToString();
        }
        return retval;


    }
    [WebMethod]
    public static string UpdateCommondata_dates(commondataG commondataG)
    {
        function fns = new function();
        string retval = "0";
        DataAccess da1 = new DataAccess();
        string entrytype = "Insert";
        string editid = "0";
        if (commondataG.id != "" && commondataG.id != "0")
        {
            editid = commondataG.id;
            entrytype = "Update";
        }
        SqlParameter[] prmD;
        if (commondataG.updatetype == "milestone")
        {
            string statusid = "0";
            string priorityid = "0";
            prmD = new SqlParameter[5];
            prmD[0] = new SqlParameter("@id", editid);
            if (commondataG.start_date == string.Empty)
            {
                prmD[1] = new SqlParameter("@StartDate", DBNull.Value);
            }
            else
            {
                prmD[1] = new SqlParameter("@StartDate", commondataG.start_date);
            }
            if (commondataG.end_date == string.Empty)
            {
                prmD[2] = new SqlParameter("@EndDate", DBNull.Value);
            }
            else
            {
                prmD[2] = new SqlParameter("@EndDate", commondataG.end_date);
            }
            prmD[3] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prmD[4] = new SqlParameter("ReturnValue", SqlDbType.Int);
            prmD[4].Direction = ParameterDirection.ReturnValue;
            int retval1 = da1.executeDMLQuery("pmstbl_Addmilestone_uspGD", prmD);
            retval = retval1.ToString();
        }
        else if (commondataG.updatetype == "task")
        {
            string statusid = "0";
            string priorityid = "0";
            prmD = new SqlParameter[5];
            prmD[0] = new SqlParameter("@id", editid);
            if (commondataG.start_date == string.Empty)
            {
                prmD[1] = new SqlParameter("@StartDate", DBNull.Value);
            }
            else
            {
                prmD[1] = new SqlParameter("@StartDate", commondataG.start_date);
            }
            if (commondataG.end_date == string.Empty)
            {
                prmD[2] = new SqlParameter("@Targetdate", DBNull.Value);
            }
            else
            {
                prmD[2] = new SqlParameter("@Targetdate", commondataG.end_date);
            }
            prmD[3] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prmD[4] = new SqlParameter("ReturnValue", SqlDbType.Int);
            prmD[4].Direction = ParameterDirection.ReturnValue;
            int retval1 = da1.executeDMLQuery("pms_Adtask_uspGD", prmD);
            retval = retval1.ToString();
        }
        else if (commondataG.updatetype == "subtask")
        {
            SaveLogText("call subtask:-"+ commondataG.start_date+"," + commondataG.end_date);
            string statusid = "0";
            string priorityid = "0";
            prmD = new SqlParameter[5];
            prmD[0] = new SqlParameter("@id", editid);
            if (commondataG.start_date == string.Empty)
            {
                prmD[1] = new SqlParameter("@StartDate", DBNull.Value);
            }
            else
            {
                prmD[1] = new SqlParameter("@StartDate", commondataG.start_date);
            }
            if (commondataG.end_date == string.Empty)
            {
                prmD[2] = new SqlParameter("@Targetdate", DBNull.Value);
            }
            else
            {
                prmD[2] = new SqlParameter("@Targetdate", commondataG.end_date);
            }
            prmD[3] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prmD[4] = new SqlParameter("ReturnValue", SqlDbType.Int);
            prmD[4].Direction = ParameterDirection.ReturnValue;
            int retval1 = da1.executeDMLQuery("pms_Add_Subtask_uspGD", prmD);
            retval = retval1.ToString();
        }
        return retval;


    }
    [WebMethod]
    public static string DeleteCommondata(commondataG commondataG)
    {
        function fns = new function();
        string retval = "0";
        DataAccess da1 = new DataAccess();
        string entrytype = "Delete";
        string editid = commondataG.id;
        string strsql = "";
        if (commondataG.updatetype == "milestone")
        {
            SqlParameter[] prm = new SqlParameter[6];
            prm[0] = new SqlParameter("@id", editid);
            prm[1] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[2] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prm[3] = new SqlParameter("@cdate", da1.getcdatetime());
            prm[4] = new SqlParameter("@type", entrytype);
            prm[5] = new SqlParameter("ReturnValue", SqlDbType.Int);
            prm[5].Direction = ParameterDirection.ReturnValue;
            int retval1 = da1.executeDMLQuery("pmstbl_Addmilestone_uspG", prm);
            retval = retval1.ToString();
           
        }
        else if (commondataG.updatetype == "task")
        {
           SqlParameter[] prm = new SqlParameter[6];
            prm[0] = new SqlParameter("@id", editid);
            prm[1] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
            prm[2] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prm[3] = new SqlParameter("@cdate", da1.getcdatetime());
            prm[4] = new SqlParameter("@type", entrytype);            
            prm[5] = new SqlParameter("ReturnValue", SqlDbType.Int);
            prm[5].Direction = ParameterDirection.ReturnValue;
            int retval1 = da1.executeDMLQuery("pms_Adtask_uspG", prm);
            retval = retval1.ToString();
        }
        else if (commondataG.updatetype == "subtask")
        {
            SqlParameter[] prm = new SqlParameter[6];
            prm[0] = new SqlParameter("@id", editid);
            prm[1] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[2] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prm[3] = new SqlParameter("@cdate", da1.getcdatetime());
            prm[4] = new SqlParameter("@type", entrytype);
            prm[5] = new SqlParameter("ReturnValue", SqlDbType.Int);
            prm[5].Direction = ParameterDirection.ReturnValue;
            int retval1 = da1.executeDMLQuery("pms_Add_Subtask_uspG", prm);
            retval = retval1.ToString();
            
        }
        return retval;


    }
}
public class MultiTaskData
{
    public string id { get; set; }
    public string ProjectId { get; set; }
    public string Milestoneid { get; set; }
    public string tasklist { get; set; }
    public string taskname { get; set; }
    public string taskdescripton { get; set; }
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
    public string ReminderType { get; set; }
    public string Days { get; set; }
    public string Time { get; set; }
    public string Notifyusers { get; set; }
    public string ChooseDate { get; set; }
    public string Occurrence { get; set; }
    public string tagto { get; set; }
    public string team { get; set; }
    public string Stage { get; set; }
    public string Weightage { get; set; }
    public string issubtask { get; set; }
    public string checkTaskWeightage { get; set; }
}

public class SubSubtaskdata
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


public class milestonetdata
{
    public string id { get; set; }
    public string ProjectId { get; set; }
    public string Milestone { get; set; }
    public string Weightage { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string Owner { get; set; }
    public string Access { get; set; }
    public string team { get; set; }
    public string P_Status { get; set; }
    public string Priority { get; set; }
    public string Budget { get; set; }
    public string Actual { get; set; }
    public string istask { get; set; }
    public string checkmileWeightage { get; set; }
    public string Durationtype { get; set; }
    public string TaskDuration { get; set; }
}
public class commondataG
{
    public string id { get; set; }
    public string projectid { get; set; }
    public string updatetype { get; set; }
    public string txtname { get; set; }
    public string description { get; set; }
    public string start_date { get; set; }
    public string start_time { get; set; }
    public string end_date { get; set; }
    public string end_time { get; set; }
    public string hdnnewid { get; set; }
    public string hdnparentid { get; set; }
    
}








