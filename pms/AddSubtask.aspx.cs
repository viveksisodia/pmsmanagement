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

public partial class AddSubtask : System.Web.UI.Page
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
        try
        {
            string strvalidationS = get_EditDelete_Validation("56");
            string[] arS = strvalidationS.Split(',');

            if (arS[0] == "0" && arS[1] == "0" && arS[2] == "0" && arS[3] == "0" && arS[4] == "0")
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

        SqlParameter[] prm = new SqlParameter[34];
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
        prm[32] = new SqlParameter("@Uploaddrawing", subtaskdetails.Uploaddrawing);

        prm[33] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[33].Direction = ParameterDirection.ReturnValue;
        int retval1= da1.executeDMLQuery("pms_Add_Subtask_usp", prm);
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
    public static string statussave(string value = "", string statuscolor = "")
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
    public static string AddStage(string value = "", string Stagecolor = "")
    {

        string retval = "0";
        DataAccess da1 = new DataAccess();

        SqlParameter[] prm = new SqlParameter[5];
        prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[1] = new SqlParameter("@name", value);
        prm[2] = new SqlParameter("@type", "insert");
        prm[3] = new SqlParameter("@PrintColor", "#"+Stagecolor);
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
    public static string Bindcheckstrtdatetaskdetails(int taski_d = 0)
    {

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "select * from Pms_tbl_Addtask where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + taski_d + "'";
        ds = da.GetDataSetInline(strsql);


        return ds.GetXml();
    }
    public static string checkdependency(string strtype, string strid)
    {
        DataAccess da1 = new DataAccess();
        string ret = "0";
        string strsqlM = "select isnull(Isdone,0) Isdone from tbl_dependencyST where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and milestoneid>0 and  milestoneid='" + strid + "' and status=1 and archive=0 and isnull(Isdone,0)=0";
        if (strtype == "task")
        {
            strsqlM = "select  isnull(Isdone,0)  from tbl_dependencyST where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and taskid='" + strid + "'  and status=1 and archive=0  and isnull(Isdone,0)=0";
        }
        else if (strtype == "subtask")
        {
            strsqlM = "select  isnull(Isdone,0)  from tbl_dependencyST where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and subtaskid='" + strid + "'  and status=1 and archive=0  and isnull(Isdone,0)=0";
        }
        DataSet dschkD = da1.GetDataSetInline(strsqlM);
        if (dschkD.Tables[0].Rows.Count > 0)
        {
            ret = "1";

        }

        return ret;

    }
    [WebMethod]
    public static string Update_Status(string subtaskid = "", string statusid = "", string iscomplete = "")
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (subtaskid != "" && subtaskid != "0")
        {
            entrytype = "Update";
        }



        // percentage update start  // 
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

    //public static string Update_Status(string subtaskid = "", string statusid = "", string iscomplete = "")
    //{
    //    string retval = "";
    //    DataAccess da1 = new DataAccess();
    //    string entrytype = "Update";
    //    if (subtaskid != "" && subtaskid != "0")
    //    {
    //        entrytype = "Update";
    //    }

    //    SqlParameter[] prm = new SqlParameter[8];
    //    prm[0] = new SqlParameter("@id", subtaskid);
    //    prm[1] = new SqlParameter("@ProjectStatus", statusid);
    //    prm[2] = new SqlParameter("@iscomplete", iscomplete);
    //    prm[3] = new SqlParameter("@iscompletedate", da1.getcdatetime());
    //    prm[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
    //    prm[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
    //    prm[6] = new SqlParameter("@cdate", da1.getcdatetime());
    //    prm[7] = new SqlParameter("@type", entrytype);
    //    da1.executeDMLQuery("pmstbl_subtask_Status_DeatilsUpdate", prm);

    //    // percentage update start  // 
    //    if (iscomplete == "1")
    //    {
    //        decimal subtsktotalpercent = 100;
    //        decimal totlweitagemile = 0;
    //        decimal totlweitageproject = 0;

    //        string finsql1 = "select * from Pms_tbl_Addsubtask where id=" + subtaskid + " and status=1  and archive=0";
    //        DataSet dsfindid1 = da1.GetDataSetInline(finsql1);

    //        string finsql2 = "select isnull(sum(weightage),0)Wattagetasksum from Pms_tbl_Addsubtask where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "' and task='" + dsfindid1.Tables[0].Rows[0]["task"] + "'  and status=1 and  archive=0 and isnull(iscomplete,0)=1";
    //        DataSet dsfindid2 = da1.GetDataSetInline(finsql2);

    //        //sub task percentage //
    //        string strstatusupdate = "update Pms_tbl_Addsubtask set iscompletepercentage='" + subtsktotalpercent.ToString() + "' where id='" + subtaskid + "' and status=1 and archive=0 and isnull(iscomplete,0)=1";
    //        DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

    //        // task percentage //
    //        string finsql3 = "update Pms_tbl_Addtask set iscompletepercentage='" + dsfindid2.Tables[0].Rows[0]["Wattagetasksum"] + "' where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "' and id='" + dsfindid1.Tables[0].Rows[0]["task"] + "' and status=1 and archive=0";
    //        DataSet dsfindid3 = da1.GetDataSetInline(finsql3);


    //        // Milestone percentage //
    //        string findsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1  and archive=0";
    //        DataSet dsfindidmile = da1.GetDataSetInline(findsqlmile);
    //        for (int j = 0; j < dsfindidmile.Tables[0].Rows.Count; j++)
    //        {
    //            decimal sumweitage = 0;
    //            decimal subpercentageemile = 0;
    //            sumweitage = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["Weightage"].ToString());
    //            subpercentageemile = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
    //            totlweitagemile += (sumweitage * subpercentageemile) / 100;

    //        }

    //        string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile + "' where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and id='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1 and archive=0";
    //        DataSet dsfindid4 = da1.GetDataSetInline(finsql4);

    //        // project percentage //
    //        string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'   and status=1  and archive=0";
    //        DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
    //        for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
    //        {
    //            decimal sumweitageprojt = 0;
    //            decimal subpercentageeproject = 0;
    //            sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
    //            subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
    //            totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

    //        }

    //        string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'   and status=1 and archive=0";
    //        DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);




    //    }
    //    else
    //    {
    //        decimal subtsktotalpercent = 0;
    //        decimal totlweitagemile = 0;
    //        decimal totlweitageproject = 0;

    //        string finsql1 = "select * from Pms_tbl_Addsubtask where id=" + subtaskid + " and status=1  and archive=0";
    //        DataSet dsfindid1 = da1.GetDataSetInline(finsql1);

    //        string finsql2 = "select isnull(sum(weightage),0)Wattagetasksum from Pms_tbl_Addsubtask where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "' and task='" + dsfindid1.Tables[0].Rows[0]["task"] + "'  and status=1 and isnull(iscomplete,0)=1 and archive=0";
    //        DataSet dsfindid2 = da1.GetDataSetInline(finsql2);

    //        //sub task percentage //
    //        string strstatusupdate = "update Pms_tbl_Addsubtask set iscompletepercentage='" + subtsktotalpercent.ToString() + "' where id='" + subtaskid + "' and status=1 and archive=0";
    //        DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

    //        //task percentage //
    //        string finsql3 = "update Pms_tbl_Addtask set iscompletepercentage='" + dsfindid2.Tables[0].Rows[0]["Wattagetasksum"] + "' where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "' and id='" + dsfindid1.Tables[0].Rows[0]["task"] + "' and status=1 and archive=0";
    //        DataSet dsfindid3 = da1.GetDataSetInline(finsql3);

    //        //milestone percentage //
    //        string findsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'    and status=1  and archive=0";
    //        DataSet dsfindidmile = da1.GetDataSetInline(findsqlmile);
    //        for (int j = 0; j < dsfindidmile.Tables[0].Rows.Count; j++)
    //        {
    //            decimal sumweitage = 0;
    //            decimal subpercentageemile = 0;
    //            sumweitage = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["Weightage"].ToString());
    //            subpercentageemile = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
    //            totlweitagemile += (sumweitage * subpercentageemile) / 100;

    //        }

    //        string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile + "' where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and id='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1 and archive=0";
    //        DataSet dsfindid4 = da1.GetDataSetInline(finsql4);


    //        // project percentage //
    //        string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'   and status=1  and archive=0";
    //        DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
    //        for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
    //        {
    //            decimal sumweitageprojt = 0;
    //            decimal subpercentageeproject = 0;
    //            sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
    //            subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
    //            totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

    //        }

    //        string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'   and status=1 and archive=0";
    //        DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);



    //    }
    //    // percentage update end  // 


    //    //project  log details insert start
    //    SqlParameter[] prmlog = new SqlParameter[8];
    //    prmlog[0] = new SqlParameter("@Transid", subtaskid);
    //    prmlog[1] = new SqlParameter("@Logstatus", statusid);
    //    prmlog[2] = new SqlParameter("@LogDate", da1.getcdatetime());
    //    prmlog[3] = new SqlParameter("@Logtype", "subtask");
    //    prmlog[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
    //    prmlog[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
    //    prmlog[6] = new SqlParameter("@cdate", da1.getcdatetime());
    //    prmlog[7] = new SqlParameter("@type", "Insert");
    //    da1.executeDMLQuery("pmstbl_Loginsert_usp", prmlog);
    //    //project  log details insert end



    //    DataAccess da = new DataAccess();
    //    string strsql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + statusid + "' ";
    //    DataSet dscode = da.GetDataSetInline(strsql);
    //    if (dscode.Tables[0].Rows.Count > 0)
    //    {
    //        retval = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
    //    }

    //    return retval;
    //}

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
    public static string bindMultiOwner_new(string tas_kid = "")
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
    public static string bindMultiChangeOwner_new(string tsk_id = "")
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
    public static string bindMultiTeam_new(string tas_kid = "")
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
    public static string bindMultiChangeTeam_new(string tsk_id = "")
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
    public static string bindtasktimelinedata(string tski_d = "0")
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
    public static string bindSubtasktimelinedata(string tski_d = "0")
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
    public static string SaveTimerstartdetails(string ProjectId = "", string Milestoneid = "", string tasklistid = "", string taskid = "",string Subtaskid = "")
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
        SaveLogText("strhourminuts-"+ strhourminuts);
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
    public static string bindSubtaskdata(string RecordId = "0", string milstoneid = "0", string tasklistid = "0", string taskid = "0")
    {
        string retval = get_status_listbind();
        string retstageval = get_Stage_listbind();
        string strvalprority = get_Priority_listbind();
        string Subtaskdata = "";
        function fns = new function();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        DataSet ds = new DataSet();
        int isadmin = 0;
        string strsql = "select Manage_Profile.* from Manage_Profile inner join tbl_employee on Manage_Profile.id=cast(tbl_employee.roleid as int) where tbl_employee.id='" + HttpContext.Current.Request.Cookies["user"]["user_id"] + "'  and Manage_Profile.status=1 and Manage_Profile.archive=0";
        DataSet dsadminchk = da.GetDataSetInline(strsql);
        if (dsadminchk.Tables[0].Rows.Count > 0)
        {
            if (dsadminchk.Tables[0].Rows[0]["id"].ToString() == "1")
            {
                isadmin = 1;

            }

        }

        if (isadmin == 1)
        {

            SqlParameter[] prm = new SqlParameter[5];
            prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[1] = new SqlParameter("@RecordId", RecordId);
            prm[2] = new SqlParameter("@milstoneid", milstoneid);
            prm[3] = new SqlParameter("@tasklistid", tasklistid);
            prm[4] = new SqlParameter("@taskid", taskid);
            ds = da.GetDataSet("Pms_BindSubtask_dashbord", prm);
        }
        else
        {
            SqlParameter[] prm = new SqlParameter[7];
            prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[1] = new SqlParameter("@RecordId", RecordId);
            prm[2] = new SqlParameter("@milstoneid", milstoneid);
            prm[3] = new SqlParameter("@tasklistid", tasklistid);
            prm[4] = new SqlParameter("@taskid", taskid);
            prm[5] = new SqlParameter("@owner", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prm[6] = new SqlParameter("@team", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            ds = da.GetDataSet("Pms_BindSubtask_dashbord_ownerwise", prm);

        }




        Subtaskdata += "<table style='width:100%;' cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm datatable'>";
        Subtaskdata += "<thead>";
        Subtaskdata += "<tr>";

        //Subtaskdata += "<th>Select</th>";
        Subtaskdata += "<th>S.No</th>";
        Subtaskdata += "<th>Action</th>";
        Subtaskdata += "<th style=min-width:130px;>Project</th>";
        Subtaskdata += "<th style=min-width:130px;>Milestone</th>";
        Subtaskdata += "<th style=min-width:130px;>Task</th>";
        Subtaskdata += "<th style=min-width:130px;>Sub Task</th>";
        Subtaskdata += "<th style=min-width:130px;>Timer</th>";
        Subtaskdata += "<th>Timer History </th>";

        Subtaskdata += "<th style=min-width:130px;>Description</th>";
        Subtaskdata += "<th style=min-width:140px;>Dependency</th>";
        Subtaskdata += "<th style=min-width:140px;>Completion Status</th>";
        Subtaskdata += "<th>Weightage</th>";
        Subtaskdata += "<th style=min-width:130px;>Owner</th>";
        Subtaskdata += "<th style=min-width:130px;>Team</th>";
        Subtaskdata += "<th style=min-width:140px;>Timeline</th>";
        Subtaskdata += "<th style=min-width:140px;>Extended Timeline</th>";
        Subtaskdata += "<th>Start Date & Time</th>";
        Subtaskdata += "<th>End Date & Time</th>";
        Subtaskdata += "<th style=width:115px;>Status</th>";
        Subtaskdata += "<th>Priority</th>";
        Subtaskdata += "<th style=min-width:140px;>Issues</th>";
        
        //Subtaskdata += "<th>Budget</th>";
        //Subtaskdata += "<th>Actual</th>";
        //Subtaskdata += "<th>Sub Task Status</th>";
        Subtaskdata += "<th>Task Duration (Hour)</th>";
        Subtaskdata += "<th style=min-width:140px;>Stage</th>";
        Subtaskdata += "<th>Work Hours (W)</th>";
        Subtaskdata += "<th>Timelog Total (T)</th>";
        Subtaskdata += "<th>Difference (W-T)</th>";
        Subtaskdata += "<th>File</th>";
        Subtaskdata += "</tr>";
        Subtaskdata += "</thead>";
        Subtaskdata += "<tbody>";
        string project_name = "";
        string milestone_name = "";
        string task_name = "";
        if (ds.Tables[0].Rows.Count > 0)
        {

            string strvalidationS = get_EditDelete_Validation("56");
            string strvalidationI = get_EditDelete_Validation("57");
            string strvalidationD = get_EditDelete_Validation("58");

            string streditS = "0";
            string strdeleteS = "0";
            string strownerS = "0";
            string strviewI = "0";
            string strviewD = "0";
            try
            {
                string[] arrS = strvalidationS.Split(',');
                streditS = arrS[3];
                strdeleteS = arrS[4];
                strownerS = arrS[5];

            }
            catch { }
           
            try
            {
                string[] arrI = strvalidationI.Split(',');
                strviewI = arrI[1];

            }
            catch { }
            try
            {
                string[] arrD = strvalidationD.Split(',');
                strviewD = arrD[1];

            }
            catch { }
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


                // dependancy end//



                //   completion  status  start //

                float Wattagecomplite = 0;
                float Wattagepending = 100;
                float projectTotal = 0;
                float PhaseTotal = 0;
                float TaskTotal = 0;
                int checkiscomplete = 0;
                
                string str = "select isnull(iscomplete,0) as iscomplete, * from Pms_tbl_Addsubtask where ProjectId='" + ds.Tables[0].Rows[i]["ProjectId"] + "' and Milestoneid='" + ds.Tables[0].Rows[i]["Milestoneid"] + "' and task='" + ds.Tables[0].Rows[i]["task"] + "' and id='" + ds.Tables[0].Rows[i]["id"] + "' and status=1 and archive=0 ";
                DataSet dss = da.GetDataSetInline(str);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    checkiscomplete = Convert.ToInt32(dss.Tables[0].Rows[0]["iscomplete"].ToString());
                    if(checkiscomplete==1)
                    {
                        projectTotal = Convert.ToInt32(dss.Tables[0].Rows[0]["iscompletepercentage"].ToString());
                    }
                    
                    
                }

                if (projectTotal > 0)
                {
                    Wattagecomplite = projectTotal;
                    Wattagepending = Math.Abs(Wattagepending - Wattagecomplite);
                }


                //   completion  status  end //

                

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


                string findtask111 = "select COUNT(*) as count_Issues from Pms_tbl_Addissues where ProjectId='" + ds.Tables[0].Rows[i]["ProjectId"] + "' and Milestoneid='" + ds.Tables[0].Rows[i]["Milestoneid"] + "' and  taskid='" + ds.Tables[0].Rows[i]["task"] + "' and  subtaskid='" + ds.Tables[0].Rows[i]["id"] + "' and Issueon='subtask' and IssueStatus in ( select id from Pms_tbl_Projectstatus where  status=1 and archive=0 and isnull(iscomplete,0)=1)  and status=1 and archive=0";
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

                Subtaskdata += " <td><a href='javascript:void(0);' style='display:" + (streditS == "1" ? "" : "none") + "' Class='btn btn-info btn-sm' type='button' onclick='Binddetails(" + ds.Tables[0].Rows[i]["id"].ToString() + ")' data-bs-toggle='offcanvas' data-bs-target='#addtask'><i class='ri-edit-line'></i></a>";
                Subtaskdata += "&nbsp;&nbsp;<a href='javascript:void(0);' style='display:" + (strdeleteS == "1" ? "" : "none") + "' Class='btn btn-danger btn-sm' type='button' onclick='deletedata(" + ds.Tables[0].Rows[i]["id"].ToString() + ")'><i class='ri-delete-bin-5-line'></i></a>";
                Subtaskdata += "</td>";

               



                Subtaskdata += "<td valign='top'>";
                if (ds.Tables[0].Rows[i]["ProjectId"].ToString() == project_name && ds.Tables[0].Rows[i]["Milestoneid"].ToString() == milestone_name && ds.Tables[0].Rows[i]["task"].ToString() == task_name)
                {
                    Subtaskdata += "";
                }
                else
                {
                    Subtaskdata += get_ProjectName(ds.Tables[0].Rows[i]["ProjectId"].ToString()); // Project
                }

                Subtaskdata += "</td>";

                Subtaskdata += "<td valign='top'>";
                if (ds.Tables[0].Rows[i]["ProjectId"].ToString() == project_name && ds.Tables[0].Rows[i]["Milestoneid"].ToString() == milestone_name && ds.Tables[0].Rows[i]["task"].ToString() == task_name)
                {
                    Subtaskdata += "";
                }
                else
                {
                    Subtaskdata += get_Milstonename(ds.Tables[0].Rows[i]["Milestoneid"].ToString()); // Milestone
                }
                    
                Subtaskdata += "</td>";
                

                Subtaskdata += "<td valign='top'>";
                if (ds.Tables[0].Rows[i]["ProjectId"].ToString() == project_name && ds.Tables[0].Rows[i]["Milestoneid"].ToString() == milestone_name && ds.Tables[0].Rows[i]["task"].ToString() == task_name)
                {
                    Subtaskdata += "";
                }
                else
                {
                    Subtaskdata += get_Taskname(ds.Tables[0].Rows[i]["task"].ToString()); // Task 
                }
              
                Subtaskdata += "</td>";
                
                Subtaskdata += "<td valign='top'>";

                //Subtaskdata += ds.Tables[0].Rows[i]["Subtaskname"].ToString(); //Sub task
                Subtaskdata += "<div class='d-flex'><div class='flex-grow-1'>";
                Subtaskdata += ds.Tables[0].Rows[i]["Subtaskname"].ToString();
                Subtaskdata += "</div>";
                Subtaskdata += "<div class='gap-1 flex-shrink-0 align-self-center' style='display:"+(strviewD=="1"?"":"none")+"'><a href ='javascript:void(0);' type='button' id='btnD_" + ds.Tables[0].Rows[i]["id"].ToString() + "' onclick='Binddetails_D(" + ds.Tables[0].Rows[i]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#dependencyD'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='Dependency' style='font-size:12px;height:22px;width:22px;margin-left:9px'>D</span></a></div>";
                // mange issues
                int chkissues = CheckIssuesdata(ds.Tables[0].Rows[i]["id"].ToString(), "subtask");
                if (chkissues > 0)
                {
                    Subtaskdata += "<div class='gap-1 flex-shrink-0 align-self-center' style='display:" + (strviewI == "1" ? "" : "none") + "'><a href ='javascript:void(0);' type='button' id='btnI_" + ds.Tables[0].Rows[i]["id"].ToString() + "' onclick='loadsrcdetails(\"issuesdetails\", \"/pms/userissuesdetails.aspx?selectid=" + ds.Tables[0].Rows[i]["id"].ToString() + "&selecttype=subtask\",\"View sub task " + ds.Tables[0].Rows[i]["subtaskname"].ToString() + " issues details\")' data-bs-toggle='modal' data-bs-target='#viewissues'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='Issues' style='font-size:12px;height:22px;width:22px;margin-left:9px'>I</span></a></div>";
                }

                Subtaskdata += "</div>";

                Subtaskdata += "</td>";


                // timer 



                //check statuscomplet //
                int chkisstatuscomplete = 0;
                string strchsttscomplt = "select isnull(iscomplete,0) as iscomplete from Pms_tbl_Projectstatus where status = 1 and archive = 0  and id in (select P_Status from Pms_tbl_Addsubtask where ProjectId = '" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "' and Milestoneid = '" + ds.Tables[0].Rows[i]["Milestoneid"].ToString() + "' and task = '" + ds.Tables[0].Rows[i]["task"].ToString() + "' and id = '" + ds.Tables[0].Rows[i]["id"].ToString() + "' and status = 1 and archive = 0) and isnull(iscomplete,0)= 1  and status = 1 and archive = 0 ";
                DataSet dschksttscomplt = da.GetDataSetInline(strchsttscomplt);
                if (dschksttscomplt.Tables[0].Rows.Count > 0)
                {


                    chkisstatuscomplete = Convert.ToInt32(dschksttscomplt.Tables[0].Rows[0]["iscomplete"].ToString());
                }

                if (chkisstatuscomplete != 1)
                {

                    Subtaskdata += "<td align ='center'>";
                    // dependency check start //
                    int isstatuscomplete = 0;
                    string depcheck = "";
                    string deptaskname = "";
                    string strdepcy = "SELECT  Split.value as dependancy,isnull(statuscomplete,0) statuscomplete FROM Pms_tbl_Addsubtask AS subtask CROSS APPLY STRING_SPLIT(subtask.dependancy, ',') as Split where subtask.status = 1 and subtask.archive = 0 and subtask.regid = '1'  and subtask.id = '" + ds.Tables[0].Rows[i]["id"].ToString() + "' ";
                    DataSet dsdepcychk = da.GetDataSetInline(strdepcy);
                    if (dsdepcychk.Tables[0].Rows.Count > 0)
                    {

                        isstatuscomplete = Convert.ToInt32(dsdepcychk.Tables[0].Rows[0]["statuscomplete"].ToString());

                        for (int k = 0; k < dsdepcychk.Tables[0].Rows.Count; k++)
                        {
                            if (Convert.ToInt32(dsdepcychk.Tables[0].Rows[k]["statuscomplete"]) != 3)
                            {
                                if (depcheck == "")
                                {
                                    depcheck = dsdepcychk.Tables[0].Rows[k]["dependancy"].ToString();
                                }
                                else
                                {
                                    depcheck += "," + dsdepcychk.Tables[0].Rows[k]["dependancy"].ToString();

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

                        Subtaskdata += "<button class='start_timer' type='button' name='start' style='background:none;border:0px;display:" + (showrun == 0 ? "block" : "none") + "'><i class='ri-timer-line fs-3xl' onclick='savestartdetails(\"" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "\",\"" + ds.Tables[0].Rows[i]["Milestoneid"].ToString() + "\",\"" + ds.Tables[0].Rows[i]["tasklist"].ToString() + "\",\"" + ds.Tables[0].Rows[i]["task"].ToString() + "\",\"" + ds.Tables[0].Rows[i]["id"].ToString() + "\",\"" + deptaskname + "\")' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='Start Timer'></i></button>";
                    }

                    else
                    {
                        Subtaskdata += "<button class='start_timer' id='divs_" + ds.Tables[0].Rows[i]["id"].ToString() + "'  type='button' name='start' style='background:none;border:0px;display:" + (showrun == 0 ? "block" : "none") + "'><i class='ri-timer-line fs-3xl' onclick='savestartdetails(\"" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "\",\"" + ds.Tables[0].Rows[i]["Milestoneid"].ToString() + "\",\"" + ds.Tables[0].Rows[i]["tasklist"].ToString() + "\",\"" + ds.Tables[0].Rows[i]["task"].ToString() + "\",\"" + ds.Tables[0].Rows[i]["id"].ToString() + "\",\"" + deptaskname + "\"),start_" + ds.Tables[0].Rows[i]["id"].ToString() + "(hour_" + ds.Tables[0].Rows[i]["id"].ToString() + ",minute_" + ds.Tables[0].Rows[i]["id"].ToString() + ",second_" + ds.Tables[0].Rows[i]["id"].ToString() + ",millisecond_" + ds.Tables[0].Rows[i]["id"].ToString() + "," + ds.Tables[0].Rows[i]["id"].ToString() + ")' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='Start Timer'></i></button>";

                    }



                    Subtaskdata += "<div class='timer_wrapper' style='display:" + (showrun == 1 ? "block" : "none") + ";' id='dinv_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>";
                    Subtaskdata += "<button class='stop_timer' style='background:none;border:0px;'><i class='ri-timer-line fs-3xl' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='Stop Timer'></i></button>";
                    Subtaskdata += "<div class='timer-content'>";
                   // Subtaskdata += "<button type ='button' name='stop'><i class='bi bi-stop-circle-fill text-danger' data-bs-toggle='modal' data-bs-target='#timer_stop' onclick='Binddetails_Timer(" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "," + ds.Tables[0].Rows[i]["Milestoneid"].ToString() + "," + ds.Tables[0].Rows[i]["tasklist"].ToString() + "," + ds.Tables[0].Rows[i]["task"].ToString() + "," + ds.Tables[0].Rows[i]["id"].ToString() + ",hour_" + ds.Tables[0].Rows[i]["id"].ToString() + ",minute_" + ds.Tables[0].Rows[i]["id"].ToString() + ",second_" + ds.Tables[0].Rows[i]["id"].ToString() + ",millisecond_" + ds.Tables[0].Rows[i]["id"].ToString() + "," + transactionid + "," + varid + ")'  data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='Stop Timer'></i></button> <button type='button' onclick='saveResumeDetails(" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "," + ds.Tables[0].Rows[i]["Milestoneid"].ToString() + "," + ds.Tables[0].Rows[i]["tasklist"].ToString() + "," + ds.Tables[0].Rows[i]["task"].ToString() + "," + ds.Tables[0].Rows[i]["id"].ToString() + "," + transactionid + "),start_" + ds.Tables[0].Rows[i]["id"].ToString() + "(hour_" + ds.Tables[0].Rows[i]["id"].ToString() + ",minute_" + ds.Tables[0].Rows[i]["id"].ToString() + ",second_" + ds.Tables[0].Rows[i]["id"].ToString() + ",millisecond_" + ds.Tables[0].Rows[i]["id"].ToString() + "," + ds.Tables[0].Rows[i]["id"].ToString() + "),hideshowPausebutton(btnpause_" + ds.Tables[0].Rows[i]["id"].ToString() + ",btnresum_" + ds.Tables[0].Rows[i]["id"].ToString() + ")' id='btnresum_" + ds.Tables[0].Rows[i]["id"].ToString() + "' name='start' style='display:" + (ispause == 1 ? "inline-block" : "none") + ";'><i class='bi bi-play-circle-fill text-secondary' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='Resume Timer'></i></button> <button type ='button' style='display:" + (ispause == 0 ? "inline-block" : "none") + ";' id='btnpause_" + ds.Tables[0].Rows[i]["id"].ToString() + "' name='pause' class='me-2'><i class='bi bi-pause-circle-fill text-warning' data-bs-toggle='modal' data-bs-target='#addnotes' onclick='BindPausedetails_Notes(" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "," + ds.Tables[0].Rows[i]["Milestoneid"].ToString() + "," + ds.Tables[0].Rows[i]["tasklist"].ToString() + "," + ds.Tables[0].Rows[i]["task"].ToString() + "," + ds.Tables[0].Rows[i]["id"].ToString() + "," + transactionid + "," + varid + ")'  data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='Pause Timer'></i></button>";
                    Subtaskdata += "<button type ='button' name='stop'><i class='bi bi-stop-circle-fill text-danger' data-bs-toggle='modal' data-bs-target='#timer_stop' onclick='Binddetails_Timer(\"" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "\",\"" + ds.Tables[0].Rows[i]["Milestoneid"].ToString() + "\",\"" + ds.Tables[0].Rows[i]["tasklist"].ToString() + "\",\"" + ds.Tables[0].Rows[i]["task"].ToString() + "\",\"" + ds.Tables[0].Rows[i]["id"].ToString() + "\",hour_" + ds.Tables[0].Rows[i]["id"].ToString() + ",minute_" + ds.Tables[0].Rows[i]["id"].ToString() + ",second_" + ds.Tables[0].Rows[i]["id"].ToString() + ",millisecond_" + ds.Tables[0].Rows[i]["id"].ToString() + ",\"" + transactionid + "\",\"" + varid + "\")'  data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='Stop Timer'></i></button>" +
                        " <button type='button' onclick='saveResumeDetails(\"" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "\",\"" + ds.Tables[0].Rows[i]["Milestoneid"].ToString() + "\",\"" + ds.Tables[0].Rows[i]["tasklist"].ToString() + "\",\"" + ds.Tables[0].Rows[i]["task"].ToString() + "\",\"" + ds.Tables[0].Rows[i]["id"].ToString() + "\",\"" + transactionid + "\"),start_" + ds.Tables[0].Rows[i]["id"].ToString() + "(hour_" + ds.Tables[0].Rows[i]["id"].ToString() + ",minute_" + ds.Tables[0].Rows[i]["id"].ToString() + ",second_" + ds.Tables[0].Rows[i]["id"].ToString() + ",millisecond_" + ds.Tables[0].Rows[i]["id"].ToString() + "," + ds.Tables[0].Rows[i]["id"].ToString() + "),hideshowPausebutton(btnpause_" + ds.Tables[0].Rows[i]["id"].ToString() + ",btnresum_" + ds.Tables[0].Rows[i]["id"].ToString() + ")' id='btnresum_" + ds.Tables[0].Rows[i]["id"].ToString() + "' name='start' style='display:" + (ispause == 1 ? "inline-block" : "none") + ";'><i class='bi bi-play-circle-fill text-secondary' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='Resume Timer'></i></button> <button type ='button' style='display:" + (ispause == 0 ? "inline-block" : "none") + ";' id='btnpause_" + ds.Tables[0].Rows[i]["id"].ToString() + "' name='pause' class='me-2'><i class='bi bi-pause-circle-fill text-warning' data-bs-toggle='modal' data-bs-target='#addnotes' onclick='BindPausedetails_Notes(\"" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "\",\"" + ds.Tables[0].Rows[i]["Milestoneid"].ToString() + "\",\"" + ds.Tables[0].Rows[i]["tasklist"].ToString() + "\",\"" + ds.Tables[0].Rows[i]["task"].ToString() + "\",\"" + ds.Tables[0].Rows[i]["id"].ToString() + "\",\"" + transactionid + "\",\"" + varid + "\")'  data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='Pause Timer'></i></button>";
                    Subtaskdata += "<input type='hidden' id='hdntrans_" + ds.Tables[0].Rows[i]["id"].ToString() + "'  value='" + transactionid + "' /><span id='hour_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>" + hour + "</span>:<span id='minute_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>" + min + "</span>:<span id='second_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>" + sec + "</span>:<span id='millisecond_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>000</span>";
                    Subtaskdata += "</div>";
                    Subtaskdata += "</div>";
                    Subtaskdata += "</div>";
                    Subtaskdata += "</td>";
                }


                if (chkisstatuscomplete == 1)
                {

                    Subtaskdata += "<td valign='top'>";
                    Subtaskdata += ""; // timer status 
                    Subtaskdata += "</td>";

                }


                string statushour = "";
                string statusmin = "";
                string statussec = "";
                string strchectsktskst = " select top (1) Pms_tbl_Timerstatus.hour,Pms_tbl_Timerstatus.minute,Pms_tbl_Timerstatus.second,Pms_tbl_Timerstatus.millisecond    from Pms_tbl_Timerstatus  where subtaskid = " + ds.Tables[0].Rows[i]["id"].ToString() + " and Pms_tbl_Timerstatus.type = 'subtask' and Pms_tbl_Timerstatus.status = 1 and Pms_tbl_Timerstatus.archive = 0 order by id desc ";
                SaveLogText("strchectsktskst-"+ strchectsktskst);
                DataSet dschkstts = da.GetDataSetInline(strchectsktskst);
                if (dschkstts.Tables[0].Rows.Count > 0)
                {

                    statushour = dschkstts.Tables[0].Rows[0]["hour"].ToString();
                    statusmin = dschkstts.Tables[0].Rows[0]["minute"].ToString();
                    statussec = dschkstts.Tables[0].Rows[0]["second"].ToString();
                    Subtaskdata += "<td valign='top'>";
                    Subtaskdata += "Hour : " + statushour + ", Min: " + statusmin + ", Sec: " + statussec; // Task status 
                    Subtaskdata += "</td>";
                }

                else
                {
                    Subtaskdata += "<td valign='top'>";
                    Subtaskdata += ""; // Task status 
                    Subtaskdata += "</td>";
                }




                Subtaskdata += "<td valign='top'>";
                Subtaskdata += ds.Tables[0].Rows[i]["SubTaskDescription"].ToString(); //Sub task Description
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

                Subtaskdata += "<a href = 'javascript:void(0);' style='display:"+(strviewD=="1"?"":"none")+"' type='button' id='btndependcy_" + ds.Tables[0].Rows[i]["id"].ToString() + "' onclick='Binddetails_Dependency(" + ds.Tables[0].Rows[i]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#adddependency'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Dependency'>+</span></a>";

                Subtaskdata += "</div>";
                Subtaskdata += "</div>";
                Subtaskdata += "</td>";

                //sub task completion status start 

                Subtaskdata += "<td>";
                if (Wattagecomplite == 0)
                {

                    Subtaskdata += "<div class='progress progress-xl cursor-pointer'>";
                    Subtaskdata += "<div class='progress-bar bg-warning' data-bs-toggle='tooltip' data-bs-placement='top' title='Pending(100%)' role='progressbar' style='width:100%'><span style='text-align:center;'>100%</span>";
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

                Subtaskdata += "<td valign='top'>";
                Subtaskdata += fns.ToFormattedCurrencyString(ds.Tables[0].Rows[i]["Weightage"].ToString()); // Weightage 
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
                Subtaskdata += "<a href ='javascript:void(0);' style='display:" + (streditS == "1" ? "" : "none") + "' type='button' id='btnownr_" + ds.Tables[0].Rows[i]["id"].ToString() + "' onclick='Binddetails_Owner(" + ds.Tables[0].Rows[i]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#changeowner'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Owner'>+</span></a>";
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
                Subtaskdata += "<a href ='javascript:void(0);' style='display:" + (streditS == "1" ? "" : "none") + "' type='button' id='btnteam_" + ds.Tables[0].Rows[i]["id"].ToString() + "' onclick='Binddetails_Team(" + ds.Tables[0].Rows[i]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#addteam'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Team'>+</span></a>";
                Subtaskdata += "</div>";
                Subtaskdata += "</div>";
                Subtaskdata += "</td>";

                //Timeline

                //Storing input Dates  
                DateTime StartDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"].ToString() == "" ? DateTime.Now.ToString() : ds.Tables[0].Rows[i]["StartDate"].ToString());
                DateTime Targetdate = Convert.ToDateTime(ds.Tables[0].Rows[i]["Targetdate"].ToString() == "" ? DateTime.Now.ToString() : ds.Tables[0].Rows[i]["Targetdate"].ToString());
                TimeSpan objTimeSpan = Targetdate - StartDate;
                double Days = Convert.ToDouble(objTimeSpan.TotalDays);
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
                int subtaskstartyear = StartDate.Year;
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
                int subtaskEndyear = Targetdate.Year;
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
                Subtaskdata += "<a href = 'javascript:void(0);' class='timesheet-wrap'>";
                Subtaskdata += "<div class='progress progress-xl bg-dark position-relative' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + leftDays.ToString().Replace("-", "") + " " + leftdlydys + "'>";
                //Subtaskdata += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 70%;' aria-valuemax='100'></div>";
                Subtaskdata += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 100%;' aria-valuemax='100'></div>";
                Subtaskdata += "<div class='dayswrapper'>";
                Subtaskdata += "<span class='fromtodate'>" + ststartdys + " " + startmonthname +" " + subtaskstartyear + " - " + enddys + " " + endmonthname + " " + subtaskEndyear + "</span>";
                Subtaskdata += "<span class='totaldays'>" + Days + " days</span>";
                Subtaskdata += "</div>";
                Subtaskdata += "</div>";
                Subtaskdata += "</a>";
                Subtaskdata += "</td>";    




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
                    Subtaskdata += "<a href ='javascript:void(0);' style='display:" + (streditS == "1" ? "" : "none") + "' type='button' id='btnextndtsubtask_" + ds.Tables[0].Rows[i]["id"].ToString() + "'  onclick='Bindextenddatesubtask_Details(" + ds.Tables[0].Rows[i]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#ExtendedTimline_subtask'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Extended Day'>+</span></a>";
                    Subtaskdata += "</div>";
                }

                Subtaskdata += "</div>";
                Subtaskdata += "</div>";
                Subtaskdata += "</td>";

                Subtaskdata += "<td valign='top'>";
                Subtaskdata += cmf.dateConvert(ds.Tables[0].Rows[i]["StartDate"].ToString()) + " " + ds.Tables[0].Rows[i]["StartTime"].ToString(); // start date
                Subtaskdata += "</td>";

                Subtaskdata += "<td valign='top'>";
                Subtaskdata += cmf.dateConvert(ds.Tables[0].Rows[i]["Targetdate"].ToString()) + " " + ds.Tables[0].Rows[i]["Timeline"].ToString(); //target date
                Subtaskdata += "</td>";


                // status

                string strvalcolor = get_Status_Color(ds.Tables[0].Rows[i]["P_Status"].ToString());
                Subtaskdata += "<td>";
                Subtaskdata += "<div class='btn-group d-block'>";
                if(streditS == "1")
                {
                    Subtaskdata += "<a href ='javascript:void(0);' id='btnsts_" + ds.Tables[0].Rows[i]["id"].ToString() + "' type='button' onclick=setsubtaskstustid('" + ds.Tables[0].Rows[i]["id"].ToString() + "',this.id) data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + strvalcolor + "'>" + get_Projectstatus(ds.Tables[0].Rows[i]["P_Status"].ToString()) + "</a>";
                }
                else
                {
                    Subtaskdata += "<a href ='javascript:void(0);' id='btnsts_" + ds.Tables[0].Rows[i]["id"].ToString() + "' type='button' data-bs-toggle='' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + strvalcolor + "'>" + get_Projectstatus(ds.Tables[0].Rows[i]["P_Status"].ToString()) + "</a>";
                }
                
                Subtaskdata += "<div class='dropdown-menu lead_form_body p-0'>";
                Subtaskdata += retval;
                Subtaskdata += "</div>";
                Subtaskdata += "</div>";
                Subtaskdata += "</td>";


                //priority
                string strprioritycolor = get_PriorityColor(ds.Tables[0].Rows[i]["Priority"].ToString());
                Subtaskdata += "<td>";
                Subtaskdata += "<div class='btn-group d-block'>";
                if (streditS == "1")
                {
                    Subtaskdata += "<a href ='javascript:void(0);' id='btnpri_" + ds.Tables[0].Rows[i]["id"].ToString() + "' type='button' onclick=setpriorityid('" + ds.Tables[0].Rows[i]["id"].ToString() + "',this.id) data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + strprioritycolor + "'>" + get_PriorityName(ds.Tables[0].Rows[i]["Priority"].ToString()) + "</a>";
                }
                else
                {
                    Subtaskdata += "<a href ='javascript:void(0);' id='btnpri_" + ds.Tables[0].Rows[i]["id"].ToString() + "' type='button'  data-bs-toggle='' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + strprioritycolor + "'>" + get_PriorityName(ds.Tables[0].Rows[i]["Priority"].ToString()) + "</a>";
                }
                
                Subtaskdata += "<div class='dropdown-menu lead_form_body p-0'>";
                Subtaskdata += strvalprority;
                Subtaskdata += "</div>";
                Subtaskdata += "</div>";
                Subtaskdata += "</td>";


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

                Subtaskdata += "<div class='gap-1 flex-shrink-0 align-self-center' style='display:"+(strviewI == "1"?"":"none") +"'>";
                Subtaskdata += "<a href ='javascript:void(0);' type='button' data-bs-toggle='offcanvas' data-bs-target=#canvas_popup onclick=loadsrc('inner_popup','/pms/AddIssuesPopup.aspx?id=" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "&milstoneid=" + ds.Tables[0].Rows[i]["Milestoneid"].ToString() + "&tasklistid=" + ds.Tables[0].Rows[i]["tasklist"].ToString() + "&taskid=" + ds.Tables[0].Rows[i]["task"].ToString() + "&subtaskid=" + ds.Tables[0].Rows[i]["id"].ToString() + "&type=subtask')><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Issue'>+</span></a>";

                Subtaskdata += "</div>";
                Subtaskdata += "</td>";

               


                //Subtaskdata += "<td valign='top'>";
                //Subtaskdata += ds.Tables[0].Rows[i]["Budget"].ToString(); // budget 
                //Subtaskdata += "</td>";

                //Subtaskdata += "<td valign='top'>";
                //Subtaskdata += ds.Tables[0].Rows[i]["Actual"].ToString(); // Actual 
                //Subtaskdata += "</td>";
                

                //string taskstatus = "";
                //Subtaskdata += "<td valign='top'>";
                //if (ds.Tables[0].Rows[i]["statuscomplete"].ToString() == "3")
                //{
                //    taskstatus = "Sub Task Complete";
                //}
                //Subtaskdata += taskstatus; // Task status 
                //Subtaskdata += "</td>";
                

                Subtaskdata += "<td valign='top'>";
                Subtaskdata += cmf.dateConvert(ds.Tables[0].Rows[i]["TaskDuration"].ToString()); //Task Duration Hours
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
                Subtaskdata += "<td valign='top'>";
                // file
                string strfilescount = countfiles(ds.Tables[0].Rows[i]["id"].ToString());
                Subtaskdata += "<a href='javascript:void(0);' class='position-relative' type='button' data-bs-toggle='modal' data-bs-target='#addfiles'  onclick='loadsrcfiles(\"inner_popupfiles\",\"/pms/Addfiles.aspx?id=" + ds.Tables[0].Rows[i]["id"].ToString() + "&type=subtask&pid=" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "\",\"subtask\")'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='Upload Documents' style='font-size:12px;height:22px;width:22px;margin-left:9px'><i class='bi bi-file-earmark-arrow-up'></i></span><span class='files_count'>" + strfilescount + "</span></a>";

                //if (ds.Tables[0].Rows[i]["Uploaddrawing"].ToString() == "0" || ds.Tables[0].Rows[i]["Uploaddrawing"].ToString() == string.Empty)
                //{
                // Subtaskdata += "";
                //}
                //else
                //{
                //    string strfiles = "";
                //    string[] ar_img = ds.Tables[0].Rows[i]["Uploaddrawing"].ToString().Split('^');
                //    if (ar_img.Length > 1)
                //    {
                //        for (int j = 0; j < ar_img.Length; j++)
                //        {

                //            string fileName = ar_img[j];
                //            string extension = Path.GetExtension(fileName).ToLower();

                //            if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif")
                //            {
                //                strfiles += "<img src='/pms/SubtaskImages/" + fileName + "' style='height:50px;' class='preview-item' />";
                //            }
                //            else if (extension == ".pdf")
                //            {
                //                strfiles += "<a href='/pms/SubtaskImages/" + fileName + "' target='_blank' class='preview-item'><span style='font-size:25px; margin:5px;'>📄</span></a>";

                //            }
                //        }

                //    }
                //    else
                //    {
                //        string fileName = ds.Tables[0].Rows[i]["Uploaddrawing"].ToString();
                //        string extension = Path.GetExtension(fileName).ToLower();

                //        if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif")
                //        {
                //            strfiles += "<img src='/pms/SubtaskImages/" + fileName + "' style='height:50px;' class='preview-item' />";
                //        }
                //        else if (extension == ".pdf")
                //        {
                //            strfiles += "<a href='/pms/SubtaskImages/" + fileName + "' target='_blank' class='preview-item'><span style='font-size:25px; margin:5px;'>📄</span></a>";

                //        }
                //    }
                //    Subtaskdata += strfiles;


                //}
                Subtaskdata += "</td>";
                project_name = ds.Tables[0].Rows[i]["ProjectId"].ToString();
                milestone_name = ds.Tables[0].Rows[i]["Milestoneid"].ToString();
                task_name =   ds.Tables[0].Rows[i]["task"].ToString();

                Subtaskdata += "</tr>";
            }
        }

        Subtaskdata += "</tbody>";
        Subtaskdata += "</table>";

        return Subtaskdata;
    }
    public static string countfiles(string id)
    {
        DataAccess da = new DataAccess();
        string retval = "";
        string pid = "0";
        string strsql = "select id,isnull(ProjectId,0) as ProjectId from Pms_tbl_Addsubtask where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and id='" + id + "' ";
        DataSet dsM = da.GetDataSetInline(strsql);
        if (dsM.Tables[0].Rows.Count > 0)
        {
            pid = dsM.Tables[0].Rows[0]["ProjectId"].ToString();
            strsql = "select id from Pms_tbl_projectfiles where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and pid='" + pid + "' and fentryid='" + id + "' and ftype='subtask' ";
            DataSet dsF = da.GetDataSetInline(strsql);
            if (dsF.Tables[0].Rows.Count > 0)
            {
                retval = dsF.Tables[0].Rows.Count.ToString();
            }
        }
        return retval;
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
    public static string Bindcheckweitagedetails(int RecordId = 0)
    {
        DataAccess da = new DataAccess();
        DataSet ds;
        string strsql = "select * from Pms_tbl_Addtask where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
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
        prm[4] = new SqlParameter("@type", "subtask");
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
    public static string Deletesubtaskdata(int RecordId = 0)
    {
        string retval = "0";

        DataAccess da = new DataAccess();
        DataSet ds;
        string strsql = "";

  

        string findtaskid = "select * from Pms_tbl_Addsubtask where id=" + RecordId + " and status=1  and archive=0";
        DataSet dstaskid = da.GetDataSetInline(findtaskid);


        int checkweitage = 0;
        decimal wetagecount = 0;
        decimal totalwetage = 0;
        string strweightage = "select isnull(checkTaskWeightage ,0) as checkTaskWeightage  from pms_tbl_addtask where id='" + dstaskid.Tables[0].Rows[0]["task"].ToString() + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
        DataSet dsweitagecheck = da.GetDataSetInline(strweightage);
        if (dsweitagecheck.Tables[0].Rows.Count > 0)
        {

            checkweitage = Convert.ToInt32(dsweitagecheck.Tables[0].Rows[0]["checkTaskWeightage"]);

        }

        if (checkweitage == 1)
        {
            string strweightagecount = "select count (*) as WeitageCount from Pms_tbl_Addsubtask where task='" + dstaskid.Tables[0].Rows[0]["task"] + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
            DataSet dsweitagecount = da.GetDataSetInline(strweightagecount);
            if (dsweitagecount.Tables[0].Rows.Count > 0)
            {

                wetagecount = Convert.ToInt32(dsweitagecount.Tables[0].Rows[0]["WeitageCount"]);

            }

            totalwetage = 100 / wetagecount;
            string strstatusupdate = "update Pms_tbl_Addsubtask set Weightage='" + totalwetage.ToString() + "' where task='" + dstaskid.Tables[0].Rows[0]["task"] + "' and status=1 and archive=0";
            DataSet dshourmin = da.GetDataSetInline(strstatusupdate);

        }


        int iscomplete = 0;
        string finsql1 = "select  isnull(iscomplete,0) as iscomplete,* from Pms_tbl_Addsubtask where id=" + RecordId + " and status=1  and archive=0";
        DataSet dsfindid1 = da.GetDataSetInline(finsql1);
        if (dsfindid1.Tables[0].Rows.Count > 0)
        {

            iscomplete = Convert.ToInt32(dsfindid1.Tables[0].Rows[0]["iscomplete"]);

        }

        if (iscomplete == 1)
        {
            decimal subtsktotalpercent = 100;
            decimal totlweitagemile = 0;
            decimal totlweitageproject = 0;

            string finsql01 = "select * from Pms_tbl_Addsubtask where id=" + RecordId + " and status=1  and archive=0";
            DataSet dsfindid01 = da.GetDataSetInline(finsql01);

            string finsql2 = "select isnull(sum(weightage),0)Wattagetasksum from Pms_tbl_Addsubtask where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "' and task='" + dsfindid01.Tables[0].Rows[0]["task"] + "'  and status=1 and isnull(iscomplete,0)=1 and archive=0";
            DataSet dsfindid2 = da.GetDataSetInline(finsql2);

            //sub task percentage //
            string strstatusupdate = "update Pms_tbl_Addsubtask set iscompletepercentage='" + subtsktotalpercent.ToString() + "' where id='" + RecordId + "' and status=1 and archive=0 and isnull(iscomplete,0)=1";
            DataSet dshourmin = da.GetDataSetInline(strstatusupdate);

            // task percentage //
            string finsql3 = "update Pms_tbl_Addtask set iscompletepercentage='" + dsfindid2.Tables[0].Rows[0]["Wattagetasksum"] + "' where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "' and id='" + dsfindid01.Tables[0].Rows[0]["task"] + "' and status=1 and archive=0";
            DataSet dsfindid3 = da.GetDataSetInline(finsql3);


            // Milestone percentage //
            string findsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask  where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1  and archive=0";
            DataSet dsfindidmile = da.GetDataSetInline(findsqlmile);
            for (int j = 0; j < dsfindidmile.Tables[0].Rows.Count; j++)
            {
                decimal sumweitage = 0;
                decimal subpercentageemile = 0;
                sumweitage = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["Weightage"].ToString());
                subpercentageemile = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                totlweitagemile += (sumweitage * subpercentageemile) / 100;

            }

            string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile + "' where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and id='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da.GetDataSetInline(finsql4);



            // project percentage //
            string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and status=1  and archive=0";
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
            decimal subtsktotalpercent = 0;
            decimal totlweitagemile = 0;
            decimal totlweitageproject = 0;

            string finsql01 = "select * from Pms_tbl_Addsubtask where id=" + RecordId + " and status=1 and archive=0";
            DataSet dsfindid01 = da.GetDataSetInline(finsql1);

            string finsql2 = "select isnull(sum(weightage),0)Wattagetasksum from Pms_tbl_Addsubtask where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "' and task='" + dsfindid01.Tables[0].Rows[0]["task"] + "'  and status=1 and isnull(iscomplete,0)=1 and archive=0";
            DataSet dsfindid2 = da.GetDataSetInline(finsql2);

            //Sub task percentage //
            string strstatusupdate = "update Pms_tbl_Addsubtask set iscompletepercentage='" + subtsktotalpercent.ToString() + "' where id='" + RecordId + "' and status=1 and archive=0 and isnull(iscomplete,0)=0";
            DataSet dshourmin = da.GetDataSetInline(strstatusupdate);

            // task percentage //
            string finsql3 = "update Pms_tbl_Addtask set iscompletepercentage='" + dsfindid2.Tables[0].Rows[0]["Wattagetasksum"] + "' where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "' and id='" + dsfindid01.Tables[0].Rows[0]["task"] + "' and status=1 and archive=0";
            DataSet dsfindid3 = da.GetDataSetInline(finsql3);


            // Milestone percentage //
            string findsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask  where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "'    and status=1  and archive=0";
            DataSet dsfindidmile = da.GetDataSetInline(findsqlmile);
            for (int j = 0; j < dsfindidmile.Tables[0].Rows.Count; j++)
            {
                decimal sumweitage = 0;
                decimal subpercentageemile = 0;
                sumweitage = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["Weightage"].ToString());
                subpercentageemile = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                totlweitagemile += (sumweitage * subpercentageemile) / 100;

            }

            string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile + "' where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and id='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1 and archive=0";
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

        strsql = "update  Pms_tbl_Addsubtask set  status=0,archive=1  where  id='" + RecordId + "' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
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

    public static string get_Taskname(string taskid)
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
                    "<a class='' href='javascript:void(0);' onclick = 'UpdateDataStatusdeatils(" + dscode.Tables[0].Rows[i]["id"].ToString() + ","+ (dscode.Tables[0].Rows[i]["iscomplete"].ToString()==""?"0": dscode.Tables[0].Rows[i]["iscomplete"].ToString()) + ")' > " +
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
    public static string Binddependencydata(string stid)
    {
        string retval = "";
        DataAccess da = new DataAccess();
        string strsql = "select * from tbl_dependencyST where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and isnull(subtaskid,0)='" + stid + "'";
         SaveLogText("getdependencydetails-"+ strsql);
        DataSet dsD = da.GetDataSetInline(strsql);
        if (dsD.Tables[0].Rows.Count > 0)
        {
            retval = getdependencydetails(dsD.Tables[0].Rows[0]["id"].ToString());
        }
        return retval;
    }
    public static string getdependencydetails(string ids)
    {

        DataAccess da = new DataAccess();
        string strdependency = "";
        string retval = "";
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
                        int chkallcomplete = 0;
                        string strbgc_m = "label";
                        if (dsD.Tables[0].Rows[i]["mid"].ToString() != "" && dsD.Tables[0].Rows[i]["mid"].ToString() != "0")
                        {
                            try
                            {
                                string stsqlM = "select isnull(statuscomplete,0) statuscomplete from Pms_tbl_AddMilestone where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + dsD.Tables[0].Rows[i]["mid"].ToString() + "'";
                                DataSet dschkM = da.GetDataSetInline(stsqlM);
                                if (dschkM.Tables[0].Rows[0]["statuscomplete"].ToString() == "1")
                                {
                                    strbgc_m = "label complete";
                                }
                            }
                            catch { }
                        }
                        string strbgc_t = "label";
                        if (dsD.Tables[0].Rows[i]["tid"].ToString() != "" && dsD.Tables[0].Rows[i]["tid"].ToString() != "0")
                        {
                            try
                            {
                                string stsqlM = "select isnull(statuscomplete,0) statuscomplete from Pms_tbl_AddTask where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + dsD.Tables[0].Rows[i]["tid"].ToString() + "'";
                                DataSet dschkM = da.GetDataSetInline(stsqlM);
                                if (dschkM.Tables[0].Rows[0]["statuscomplete"].ToString() == "1")
                                {
                                    strbgc_t = "label complete";
                                }
                            }
                            catch { }
                        }
                        string strbgc_s = "label";

                        if (dsD.Tables[0].Rows[i]["stid"].ToString() != "" && dsD.Tables[0].Rows[i]["stid"].ToString() != "0")
                        {
                            try
                            {
                                string stsqlM = "select isnull(statuscomplete,0) statuscomplete from Pms_tbl_AddSubtask where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + dsD.Tables[0].Rows[i]["stid"].ToString() + "'";

                                DataSet dschkM = da.GetDataSetInline(stsqlM);
                                if (dschkM.Tables[0].Rows[0]["statuscomplete"].ToString() == "1")
                                {
                                    strbgc_s = "label complete";
                                }

                            }
                            catch (Exception ex)
                            {
                                //SaveLogText("strbgc_s2-" + ex.Message);
                            }
                        }
                        if (chkallcomplete != 1)
                        {
                            str_selecteddeatil += "<div class='hierarchy-line'><b>" + (i + 1).ToString() + "-</b>&nbsp;" +
                                 "<span class='" + strbgc_m + "'>" + dsD.Tables[0].Rows[i]["Milestone"].ToString() + "</span>" +
                                 "<span class='" + strbgc_t + "'>" + (dsD.Tables[0].Rows[i]["Taskname"].ToString() == "" ? "" : "- " + dsD.Tables[0].Rows[i]["Taskname"].ToString()) + "</span>" +
                                 "<span class='" + strbgc_s + "'>" + (dsD.Tables[0].Rows[i]["Subtaskname"].ToString() == "" ? "" : "- " + dsD.Tables[0].Rows[i]["Subtaskname"].ToString()) + "</span>" +

                                 "</div>";
                        }
                    }
                }

            }


            retval = str_selecteddeatil;
        }
        catch (Exception ex)
        {

        }
        return retval;
    }

    public static int CheckIssuesdata(string RecordId = "0", string issuetype = "")
    {

        DataAccess da = new DataAccess();

        string retval = get_status_listbind();
        string strvalprority = get_Priority_listbind();



        int chkadmin = checkadmin(HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        int issuesdata = 0;
        string ddlsearch = "5";
        int monthid = Convert.ToDateTime(da.getcdate()).Month;

        int yearid = Convert.ToDateTime(da.getcdate()).Year;
        int lastday = DateTime.DaysInMonth(yearid, monthid);

        string strfromdate = da.getcdate();
        string strtodate = da.getcdate();



        DateTime baseDate = Convert.ToDateTime(da.getcdate());
        DateTime thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
        DateTime thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
        if (ddlsearch == "1")
        {
            strfromdate = da.getcdate();
            strtodate = da.getcdate();
        }
        else if (ddlsearch == "2")
        {
            strfromdate = thisWeekStart.ToShortDateString();
            strtodate = thisWeekEnd.ToShortDateString();
        }
        else if (ddlsearch == "3")
        {
            strfromdate = monthid + "/01/" + yearid;
            strtodate = monthid + "/" + lastday + "/" + yearid;
        }

        else if (ddlsearch == "4")
        {
            DateTime[] dtReturn = DatesOfQuarter();


            strfromdate = (dtReturn[0]).ToShortDateString(); ;
            strtodate = (dtReturn[1]).ToShortDateString();
        }
        else if (ddlsearch == "5")
        {

            strfromdate = "01/01/" + yearid;
            strtodate = "12/31/" + yearid;
        }
        else if (ddlsearch == "10")
        {

            strfromdate = "01/01/" + (yearid - 1);
            strtodate = "12/31/" + (yearid - 1);
        }
        else if (ddlsearch == "6")
        {
            strfromdate = Convert.ToDateTime(da.getcdate()).AddDays(-1).ToShortDateString();
            strtodate = Convert.ToDateTime(da.getcdate()).AddDays(-1).ToShortDateString();
        }
        else if (ddlsearch == "7")
        {
            DateTime previousWeekStart = thisWeekStart.AddDays(-7);
            DateTime previousWeekEnd = thisWeekStart.AddDays(-1);

            strfromdate = previousWeekStart.ToShortDateString();
            strtodate = previousWeekEnd.ToShortDateString();


        }

        else if (ddlsearch == "8")
        {

            strfromdate = monthid + "/01/" + yearid;
            strfromdate = Convert.ToDateTime(strfromdate).AddMonths(-1).ToShortDateString();
            int prvyearid = Convert.ToDateTime(strfromdate).Year;
            int prvmonthid = Convert.ToDateTime(strfromdate).Month;
            int lastdayprv = DateTime.DaysInMonth(prvyearid, prvmonthid);

            strtodate = prvmonthid + "/" + lastdayprv + "/" + prvyearid;
            // strtodate = previousWeekEnd.ToShortDateString();


        }
        else if (ddlsearch == "9")
        {

            DateTime[] dtReturn = DatesOfQuarter();


            DateTime strfromdateprvq = (dtReturn[0].AddDays(-1));
            DateTime[] dtReturnprv = DatesOfQuarterprv(strfromdateprvq);
            strfromdate = (dtReturnprv[0]).ToShortDateString(); ;
            strtodate = (dtReturnprv[1]).ToShortDateString();

        }
        string strcolname = "";

        try
        {
            if (issuetype == "subtask")
            {
                strcolname = "subtaskid";
            }
            else if (issuetype == "task")
            {
                strcolname = "taskid";
            }
            else if (issuetype == "milestone")
            {
                strcolname = "Milestoneid";
            }
            else
            {
                strcolname = "ProjectId";
            }
        }
        catch { }

        try
        {

            string str_selecteddeatil = "";

            if (RecordId != "")
            {
                string ownerid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();
                SqlParameter[] prmC = new SqlParameter[8];
                prmC[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prmC[1] = new SqlParameter("@isadmin", chkadmin);
                prmC[2] = new SqlParameter("@strfromdate", strfromdate);
                prmC[3] = new SqlParameter("@strtodate", strtodate);
                prmC[4] = new SqlParameter("@selecttype", issuetype);
                prmC[5] = new SqlParameter("@entryid", RecordId);
                prmC[6] = new SqlParameter("@columname", strcolname);
                prmC[7] = new SqlParameter("@ownerids", ownerid);
                DataSet dsD = da.GetDataSet("pms_Getallissues_byid", prmC);
                string milestone_name = "";

                if (dsD.Tables[0].Rows.Count > 0)
                {
                    issuesdata = 1;
                }

            }


        }
        catch (Exception ex)
        {

        }


        return issuesdata;
    }
    public static DateTime[] DatesOfQuarter()
    {
        DataAccess da = new DataAccess();

        DateTime[] dtReturn = new DateTime[2];
        DateTime dtNow = Convert.ToDateTime(da.getcdate()); ;
        if (dtNow.Month >= 1 && dtNow.Month <= 3)
        {
            dtReturn[0] = DateTime.Parse("1/1/" + dtNow.Year.ToString());
            dtReturn[1] = DateTime.Parse("3/31/" + dtNow.Year.ToString());
        }
        else if (dtNow.Month >= 4 && dtNow.Month <= 6)
        {
            dtReturn[0] = DateTime.Parse("4/1/" + dtNow.Year.ToString());
            dtReturn[1] = DateTime.Parse("6/30/" + dtNow.Year.ToString());
        }
        else if (dtNow.Month >= 7 && dtNow.Month <= 9)
        {
            dtReturn[0] = DateTime.Parse("7/1/" + dtNow.Year.ToString());
            dtReturn[1] = DateTime.Parse("9/30/" + dtNow.Year.ToString());
        }
        else if (dtNow.Month >= 10 && dtNow.Month <= 12)
        {
            dtReturn[0] = DateTime.Parse("10/1/" + dtNow.Year.ToString());
            dtReturn[1] = DateTime.Parse("12/31/" + dtNow.Year.ToString());
        }
        return dtReturn;
    }

    public static DateTime[] DatesOfQuarterprv(DateTime dtprv)
    {
        DateTime[] dtReturn = new DateTime[2];
        DateTime dtNow = dtprv; ;
        if (dtNow.Month >= 1 && dtNow.Month <= 3)
        {
            dtReturn[0] = DateTime.Parse("1/1/" + dtNow.Year.ToString());
            dtReturn[1] = DateTime.Parse("3/31/" + dtNow.Year.ToString());
        }
        else if (dtNow.Month >= 4 && dtNow.Month <= 6)
        {
            dtReturn[0] = DateTime.Parse("4/1/" + dtNow.Year.ToString());
            dtReturn[1] = DateTime.Parse("6/30/" + dtNow.Year.ToString());
        }
        else if (dtNow.Month >= 7 && dtNow.Month <= 9)
        {
            dtReturn[0] = DateTime.Parse("7/1/" + dtNow.Year.ToString());
            dtReturn[1] = DateTime.Parse("9/30/" + dtNow.Year.ToString());
        }
        else if (dtNow.Month >= 10 && dtNow.Month <= 12)
        {
            dtReturn[0] = DateTime.Parse("10/1/" + dtNow.Year.ToString());
            dtReturn[1] = DateTime.Parse("12/31/" + dtNow.Year.ToString());
        }
        return dtReturn;
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
    public string Uploaddrawing {get; set;}

}




