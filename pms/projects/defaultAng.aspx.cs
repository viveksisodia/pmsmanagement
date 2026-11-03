using System;
using System.Web;
using System.Collections.Generic;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Script.Services;
using CommonClass;
using System.Data;
using System.IO;
using System.Linq;

public partial class defaultAng : System.Web.UI.Page
{
    public function fns = new function();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            updateallstatus_subtask();
            updateallstatus_task();
            updateallstatus_mile();
        }
    }
    #region manage weightage

    public void updateallstatus_subtask()
    {
        DataAccess da = new DataAccess();
        string retval = "";
        string currentstatusid = "0";
        string chkstatusid = "0";
        try
        {
            string str2 = "select id, ProjectStatus as name, PrintColor as colorname,isnull(iscomplete,0) as iscomplete, isnull(isdefault,0) isdefault from Pms_tbl_Projectstatus where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and isnull(iscomplete,0)=1";
            DataSet dsS = da.GetDataSetInline(str2);
            chkstatusid = dsS.Tables[0].Rows[0]["id"].ToString();

        }
        catch { }

        string strsqlT = "select id,isnull(p_status,0) as p_status from pms_tbl_addtask where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  and status=1 and archive=0 ";
        DataSet dsT = da.GetDataSetInline(strsqlT);
        if (dsT.Tables[0].Rows.Count > 0)
        {
            for (int j = 0; j < dsT.Tables[0].Rows.Count; j++)
            {
                string strsql = "select id,isnull(p_status,0) as p_status from pms_tbl_addsubtask where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  and status=1 and archive=0 and Task='" + dsT.Tables[0].Rows[j]["id"].ToString() + "' ";//and projectid='" + HttpContext.Current.Request.QueryString["id"] + "'
                DataSet ds = da.GetDataSetInline(strsql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string updateids = "";
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        try
                        {


                            currentstatusid = ds.Tables[0].Rows[i]["p_status"].ToString();
                            if (currentstatusid == chkstatusid)
                            {

                                string sts = UpdateSubtask_wt(ds.Tables[0].Rows[i]["id"].ToString(), currentstatusid, "1");
                            }

                        }
                        catch (Exception ex)
                        {
                            SaveLogText("updateidsAEXE=" + ex.Message);
                        }
                        //updateids = updateids + ","+ ds.Tables[0].Rows[i]["id"].ToString();
                    }

                }
            }
        }
    }
    public void updateallstatus_task()
    {
        string retval = "";
        DataAccess da = new DataAccess();
        string currentstatusid = "0";
        string chkstatusid = "0";
        try
        {
            string str2 = "select id, ProjectStatus as name, PrintColor as colorname,isnull(iscomplete,0) as iscomplete, isnull(isdefault,0) isdefault from Pms_tbl_Projectstatus where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and isnull(iscomplete,0)=1";
            DataSet dsS = da.GetDataSetInline(str2);
            chkstatusid = dsS.Tables[0].Rows[0]["id"].ToString();

        }
        catch { }
        string strsqlM = "select id,isnull(p_status,0) as p_status from pms_tbl_addmilestone where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  and status=1 and archive=0 ";
        DataSet dsM = da.GetDataSetInline(strsqlM);
        if (dsM.Tables[0].Rows.Count > 0)
        {
            for (int j = 0; j < dsM.Tables[0].Rows.Count; j++)
            {
                string strsql = "select id,isnull(p_status,0) as p_status from pms_tbl_addtask where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and milestoneid='" + dsM.Tables[0].Rows[j]["id"].ToString() + "'  ";//and projectid='" + HttpContext.Current.Request.QueryString["id"] + "'
                DataSet ds = da.GetDataSetInline(strsql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string updateids = "";
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        try
                        {

                            currentstatusid = ds.Tables[0].Rows[i]["p_status"].ToString();
                            if (currentstatusid == chkstatusid)
                            {
                                string sts = UpdateTask_wt(ds.Tables[0].Rows[i]["id"].ToString(), currentstatusid, "1");
                                updateids = updateids + ",";
                            }

                        }
                        catch (Exception ex)
                        {
                            SaveLogText("updateidsAEXE=" + ex.Message);
                        }
                    }
                    SaveLogText("updateidsA=" + updateids);
                }
            }
        }
    }
    public void updateallstatus_mile()
    {
        string retval = "";
        DataAccess da = new DataAccess();
        string currentstatusid = "0";
        string chkstatusid = "0";
        try
        {
            string str2 = "select id, ProjectStatus as name, PrintColor as colorname,isnull(iscomplete,0) as iscomplete, isnull(isdefault,0) isdefault from Pms_tbl_Projectstatus where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and isnull(iscomplete,0)=1";
            DataSet dsS = da.GetDataSetInline(str2);
            chkstatusid = dsS.Tables[0].Rows[0]["id"].ToString();

        }
        catch { }
        string strsqlP = "select id from pms_tbl_addproject where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  and status=1 and archive=0 ";
        DataSet dsP = da.GetDataSetInline(strsqlP);
        if (dsP.Tables[0].Rows.Count > 0)
        {
            for (int j = 0; j < dsP.Tables[0].Rows.Count; j++)
            {
                string strsql = "select id,isnull(p_status,0) as p_status from pms_tbl_addmilestone where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and projectid='" + dsP.Tables[0].Rows[j]["id"].ToString() + "'  ";//and projectid='" + HttpContext.Current.Request.QueryString["id"] + "'
                DataSet ds = da.GetDataSetInline(strsql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        try
                        {


                            currentstatusid = ds.Tables[0].Rows[i]["p_status"].ToString();
                            if (currentstatusid == chkstatusid)
                            {
                                string sts = UpdateMilestone_wt(ds.Tables[0].Rows[i]["id"].ToString(), currentstatusid, "1");
                            }

                        }
                        catch { }
                    }
                }
            }
        }
    }
    protected string UpdateSubtask_wt(string subtaskid, string statusid, string iscomplete)
    {
        string retval = "";
        DataAccess da1 = new DataAccess();

        SqlParameter[] prm = new SqlParameter[8];
        prm[0] = new SqlParameter("@id", subtaskid);
        prm[1] = new SqlParameter("@ProjectStatus", statusid);
        prm[2] = new SqlParameter("@iscomplete", iscomplete);
        prm[3] = new SqlParameter("@iscompletedate", da1.getcdatetime());
        prm[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[6] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[7] = new SqlParameter("@type", "Update");
        da1.executeDMLQuery("pmstbl_subtask_Status_DeatilsUpdate", prm);
        decimal subtsktotalpercent = 100;
        decimal totlweitagemile = 0;
        decimal totlweitageproject = 0;

        string finsql1 = "select * from Pms_tbl_Addsubtask where id=" + subtaskid + " and status=1  and archive=0";
        DataSet dsfindid1 = da1.GetDataSetInline(finsql1);
        SaveLogText("finsql2-" + finsql1);
        string finsql2 = "select isnull(sum(weightage),0) as Wattagetasksum from Pms_tbl_Addsubtask where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "' and task='" + dsfindid1.Tables[0].Rows[0]["task"] + "'  and status=1 and  archive=0 and isnull(iscomplete,0)=1";

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


        retval = "1";






        return retval;
    }
    protected string UpdateTask_wt(string taskid, string statusid, string iscomplete)
    {
        string retval = "";
        DataAccess da1 = new DataAccess();

        SqlParameter[] prm = new SqlParameter[8];
        prm[0] = new SqlParameter("@id", taskid);
        prm[1] = new SqlParameter("@ProjectStatus", statusid);
        prm[2] = new SqlParameter("@iscomplete", iscomplete);
        prm[3] = new SqlParameter("@iscompletedate", da1.getcdatetime());
        prm[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[6] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[7] = new SqlParameter("@type", "Update");
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
        retval = "1";


        return retval;
    }
    protected string UpdateMilestone_wt(string milstoneid, string statusid, string iscomplete)
    {
        string retval = "";
        DataAccess da1 = new DataAccess();

        SqlParameter[] prm = new SqlParameter[8];
        prm[0] = new SqlParameter("@id", milstoneid);
        prm[1] = new SqlParameter("@milstoneStatus", statusid);
        prm[2] = new SqlParameter("@iscomplete", iscomplete);
        prm[3] = new SqlParameter("@iscompletedate", da1.getcdatetime());
        prm[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[6] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[7] = new SqlParameter("@type", "Update");
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
        retval = "1";

        return retval;
    }


    #endregion
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<projectlists> bindProjectdata(string currentid)
    {

        string projectdata = "";
        function fns = new function();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();

        DataSet ds = new DataSet();
        List<projectlists> projectlist = new List<projectlists>();
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
        SqlParameter[] prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[1] = new SqlParameter("@action", "GETBY_REGID");
        ds = da.GetDataSet("usp_pms_projecttemplates_all", prm);
        // if (isadmin == 1)
        //{
        //SqlParameter[] prm = new SqlParameter[1];
        //prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        //ds = da.GetDataSet("Pms_select_tblprojectall_dashbord", prm);

        //}
        //else
        //{
        //    SqlParameter[] prm = new SqlParameter[2];
        //    prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        //    prm[1] = new SqlParameter("@owner", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        //    ds = da.GetDataSet("Pms_select_tblprojectall_dashbord_ownerwise", prm);

        //}



        if (ds.Tables[0].Rows.Count > 0)
        {

            string streditP = "0";
            string strdeleteP = "0";
            string strownerP = "0";

            string strviewM = "0";
            string strviewT = "0";
            string strviewI = "0";
            string strviewD = "0";

            // SaveLogText("edtdel:" + strviewI+","+ strviewD);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (ds.Tables[0].Rows[i]["id"].ToString() != currentid)
                {
                    //ProjectName = ds.Tables[0].Rows[i]["ProjectName"].ToString(),
                    projectlist.Add(new projectlists
                    {
                        id = ds.Tables[0].Rows[i]["id"].ToString(),
                        ProjectName = ds.Tables[0].Rows[i]["templatename"].ToString(),

                    });
                }
            }
        }

        return projectlist;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<projectlists> bindProjectdata_old(string currentid)
    {

        string projectdata = "";
        function fns = new function();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();

        DataSet ds = new DataSet();
        List<projectlists> projectlist = new List<projectlists>();
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
            SqlParameter[] prm = new SqlParameter[1];
            prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            ds = da.GetDataSet("Pms_select_tblprojectall_dashbord", prm);

        }
        else
        {
            SqlParameter[] prm = new SqlParameter[2];
            prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[1] = new SqlParameter("@owner", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            ds = da.GetDataSet("Pms_select_tblprojectall_dashbord_ownerwise", prm);

        }


        int istaskchk = 0;
        string sqlchkistsk = "select isnull(istask,0) istask from Pms_tbl_AddMilestone where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and isnull(istask,0)=1  ";
        DataSet dschkistsk = da.GetDataSetInline(sqlchkistsk);
        if (dschkistsk.Tables[0].Rows.Count > 0)
        {
            istaskchk = 1;

        }


        int isdrawingkchk = 0;
        string sqlchkisdrawing = "select isnull(isdrawing,0) isdrawing from Pms_tbl_AddProject where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and isnull(isdrawing,0)=1  ";
        DataSet dschkisdrawing = da.GetDataSetInline(sqlchkisdrawing);
        if (dschkisdrawing.Tables[0].Rows.Count > 0)
        {
            isdrawingkchk = 1;

        }

        if (ds.Tables[0].Rows.Count > 0)
        {


            string streditP = "0";
            string strdeleteP = "0";
            string strownerP = "0";

            string strviewM = "0";
            string strviewT = "0";
            string strviewI = "0";
            string strviewD = "0";

            // SaveLogText("edtdel:" + strviewI+","+ strviewD);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (ds.Tables[0].Rows[i]["id"].ToString() != currentid)
                {
                    projectlist.Add(new projectlists
                    {
                        id = ds.Tables[0].Rows[i]["id"].ToString(),
                        ProjectName = ds.Tables[0].Rows[i]["ProjectName"].ToString(),

                    });
                }
            }
        }

        return projectlist;
    }
    public static int getcopyid(string templateid)
    {
        int retval = 0;
        DataAccess da = new DataAccess();
        string strsql = "select id,isnull(pid,0) projectid from pms_tbl_projecttemplates where status=1 and archive=0 and id=" + templateid + "";
        try
        {
            DataSet ds = da.GetDataSetInline(strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                retval = Convert.ToInt32(ds.Tables[0].Rows[0]["projectid"].ToString());
            }
        }
        catch { }

        return retval;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string copyproject(string currentid, string copyprojectid)
    {

        string projectdata = "0";
        function fns = new function();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();

        if (currentid != string.Empty && currentid != "0" && copyprojectid != string.Empty && copyprojectid != "0")
        {
            var cookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (cookie == null || string.IsNullOrEmpty(cookie["id"]))
            {
                throw new Exception("User not authenticated");
            }

            int currentId = Convert.ToInt32(currentid);
            //int copyId = Convert.ToInt32(copyprojectid);
            int copyId = getcopyid(copyprojectid);

            string regid = cookie["id"];

            SqlParameter[] prm = new SqlParameter[6];
            prm[0] = new SqlParameter("@CurrentProjectId", currentid);
            prm[1] = new SqlParameter("@CopyProjectId", copyId);
            prm[2] = new SqlParameter("@RegId", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[3] = new SqlParameter("@lastmodify_byid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
            prm[4] = new SqlParameter("@lastmodify_date", da.getcdatetime());

            prm[5] = new SqlParameter("ReturnValue", SqlDbType.Int);
            prm[5].Direction = ParameterDirection.ReturnValue;
            int retval = da.executeDMLQuery("usp_CopyProject_Flag", prm);
            if (retval == 1)
            {
                #region copy all details of copyprojectid in currentid milestone,task,subtasks

                SqlParameter[] prm2 = new SqlParameter[4];
                prm2[0] = new SqlParameter("@CurrentProjectId", currentid);
                prm2[1] = new SqlParameter("@CopyProjectId", copyId);
                prm2[2] = new SqlParameter("@RegId", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prm2[3] = new SqlParameter("ReturnValue", SqlDbType.Int);
                prm2[3].Direction = ParameterDirection.ReturnValue;
                int retval2 = da.executeDMLQuery("sp_CopyProjectData", prm2);
                if (retval2 == 1)
                {
                    projectdata = "1";
                }
                #endregion
                // end
            }



            // return details of current project as updated

        }
        return projectdata;
    }
    #region load milestone
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<userlist> binduserlist()
    {
        string str2 = "select id,DisplayName,fileupload1,SUBSTRING (DisplayName, 1, 2) as splinameone from tbl_employee where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
        DataAccess danew = new DataAccess();
        DataSet drmile = danew.GetDataSetInline(str2);
        List<userlist> list = new List<userlist>();
        foreach (DataRow row in drmile.Tables[0].Rows)
        {
            string profile = "";
            string prosname = "";
            if (row["fileupload1"].ToString() != "")
            {
                profile = row["fileupload1"].ToString();
            }

            list.Add(new userlist
            {
                id = row["id"].ToString(),
                name = row["DisplayName"].ToString(),
                profileimage = profile,
                shortname = row["splinameone"].ToString()

            });
        }
        return list;
    }
    public static List<userlist> binduserlistnew()
    {
        string str2 = "select id,DisplayName,fileupload1,SUBSTRING (DisplayName, 1, 2) as splinameone from tbl_employee where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
        DataAccess danew = new DataAccess();
        DataSet drmile = danew.GetDataSetInline(str2);
        List<userlist> list = new List<userlist>();
        foreach (DataRow row in drmile.Tables[0].Rows)
        {
            string profile = "";
            string prosname = "";
            if (row["fileupload1"].ToString() != "")
            {
                profile = row["fileupload1"].ToString();
            }

            list.Add(new userlist
            {
                id = row["id"].ToString(),
                name = row["DisplayName"].ToString(),
                profileimage = profile,
                shortname = row["splinameone"].ToString()

            });
        }
        return list;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<userlist> binduserlist_byrefid(string id, string utype, string stype)
    {
        DataAccess danew = new DataAccess();
        string struser = "";
        string refids = "";
        if (id != "" && id != "0")
        {
            if (utype == "project")
            {
                struser = "select team,owner from pms_tbl_addproject where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                struser += " and id=" + id;
            }
            else if (utype == "milestone")
            {
                struser = "select team,owner from pms_tbl_addmilestone where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                struser += " and id=" + id;
            }
            else if (utype == "task")
            {
                struser = "select team,owner from pms_tbl_addtask where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                struser += " and id=" + id;
            }
        }
        List<userlist> list = new List<userlist>();
        if (struser != "")
        {
            DataSet druser = danew.GetDataSetInline(struser);
            if (druser.Tables[0].Rows.Count > 0)
            {
                refids = druser.Tables[0].Rows[0][stype].ToString();
            }
            string str2 = "select id,DisplayName,fileupload1,SUBSTRING (DisplayName, 1, 2) as splinameone from tbl_employee where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";

            if (refids != "")
            {
                str2 += " and id in(" + refids + ")";
                DataSet drmile = danew.GetDataSetInline(str2);
                foreach (DataRow row in drmile.Tables[0].Rows)
                {
                    string profile = "";
                    string prosname = "";
                    if (row["fileupload1"].ToString() != "")
                    {
                        profile = row["fileupload1"].ToString();
                    }

                    list.Add(new userlist
                    {
                        id = row["id"].ToString(),
                        name = row["DisplayName"].ToString(),
                        profileimage = profile,
                        shortname = row["splinameone"].ToString()

                    });
                }
            }
        }
        else
        {
            list = binduserlistnew();
        }
        return list;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<statuslist> bindstatuslist()
    {
        string str2 = "select id, ProjectStatus as name, PrintColor as colorname,isnull(iscomplete,0) as iscomplete, isnull(isdefault,0) isdefault from Pms_tbl_Projectstatus where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";

        DataAccess danew = new DataAccess();
        DataSet drmile = danew.GetDataSetInline(str2);

        List<statuslist> list = new List<statuslist>();

        foreach (DataRow row in drmile.Tables[0].Rows)
        {
            statuslist obj = new statuslist();
            obj.id = row["id"].ToString();
            obj.name = row["name"].ToString();
            obj.colorname = row["colorname"].ToString();
            obj.iscomplete = row["iscomplete"].ToString();
            obj.isdefault = row["isdefault"].ToString();
            list.Add(obj);
        }

        return list;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<prioritylist> bindprioritylist()
    {
        string str2 = "select id, name, PrintColor as colorname, isnull(isdefault,0) isdefault from Pms_tbl_Priority where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";

        DataAccess danew = new DataAccess();
        DataSet drmile = danew.GetDataSetInline(str2);

        List<prioritylist> list = new List<prioritylist>();

        foreach (DataRow row in drmile.Tables[0].Rows)
        {
            prioritylist obj = new prioritylist();
            obj.id = row["id"].ToString();
            obj.name = row["name"].ToString();
            obj.colorname = row["colorname"].ToString();
            obj.isdefault = row["isdefault"].ToString();
            list.Add(obj);
        }

        return list;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static project_details bindtaskdata(string RecordId)
    {
        DataAccess da = new DataAccess();

        project_details obj = new project_details();
        try
        {
            if (string.IsNullOrEmpty(RecordId))
                RecordId = "0";

            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null)
                return obj;

            string regid = regCookie["id"];

            int chkadmin = 0;
            if (HttpContext.Current.Request.Cookies["user"]["user_id"] != null)
            {
                chkadmin = checkadmin(HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            }
            else
            {
                chkadmin = 0;
            }

            string strsql = "select * from Pms_tbl_AddProject where id=" + RecordId + " and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";


            SqlDataReader dr = da.getDataReaderInline(strsql);
            while (dr.Read())
            {
                List<ownerlist> newownerlist_p = getemployeelist<ownerlist>(dr["Owner"].ToString());
                List<teamlist> newteamlist_p = getemployeelist<teamlist>(dr["team"].ToString());

                string highlightcolor = geteditcolor();
                obj.id = dr["id"] != DBNull.Value ? Convert.ToInt32(dr["id"]) : (int?)null;
                obj.ProjectID = dr["ProjectID"].ToString();
                obj.ProjectName = dr["ProjectName"].ToString();
                obj.Owner = dr["Owner"].ToString();
                obj.team = dr["team"].ToString();
                obj.ProjectStatus = Convert.ToInt32(dr["ProjectStatus"].ToString() == "" ? "0" : dr["ProjectStatus"].ToString());
                obj.AwardDate = GetNullableDate(dr["AwardDate"]);
                obj.StartDate = GetNullableDate(dr["StartDate"]);
                obj.TargetDate = GetNullableDate(dr["TargetDate"]);
                obj.salesorderid = dr["salesorderid"] as int?;
                obj.customerid = dr["customerid"] as int?;
                obj.checkprojectWeightage = dr["checkprojectWeightage"] as int?;
                obj.hastask = dr["hastask"] as int?;
                obj.checktaskWeightage = dr["checktaskWeightage"] as int?;
                obj.hassubtask = dr["hassubtask"] as int?;
                obj.checksubtaskWeightage = dr["checksubtaskWeightage"] as int?;
                obj.editcolor = highlightcolor;
                obj.owners = newownerlist_p;
                obj.teams = newteamlist_p;
                obj.isadmin = chkadmin == 1 ? true : false;
                List<newmilestone> addmilestones = new List<newmilestone>();
                try
                {
                    string str2 = "select * from Pms_tbl_AddMilestone where ProjectId='" + obj.id + "' and status=1 and archive=0 order by isnull(orderno,0)";
                    DataAccess danew = new DataAccess();
                    DataSet drmile = danew.GetDataSetInline(str2);

                    foreach (DataRow row in drmile.Tables[0].Rows)
                    {


                        int totaltasks = 0;

                        List<ownerlist> newownerlist = getemployeelist<ownerlist>(row["Owner"].ToString());
                        List<teamlist> newteamlist = getemployeelist<teamlist>(row["team"].ToString());
                        List<projectfiles> newfileslist = bindfiledata(row["projectid"].ToString(), row["id"].ToString(), "milestone");
                        List<newtask> newtasklist = bindtasklist_bymileid(row["id"].ToString());
                        List<issuestdata> newissueslist = bindissueslist(row["id"].ToString(), "milestone");
                        int totaldependencys = 0;
                        totaldependencys = Binddependencydata(row["id"].ToString(), "milestone");
                        totaltasks = checktaskby_milestoneid(row["id"].ToString());

                        double mileTaskTotal = 0;
                        mileTaskTotal = get_completionstatus(row["id"].ToString(), "milestone");
                        //SaveLogText("chkadmintodayEX-" + mileTaskTotal);
                        double mileWattagecomplite = mileTaskTotal;
                        double mileWattagepending = 100;
                        if (mileTaskTotal > 0)
                        {
                            mileWattagepending = mileWattagepending - mileTaskTotal;
                        }
                        string tleftdays = getleftdays_load(row["id"].ToString(), "milestone");
                        #region find issues
                        int totalissues = 0;
                        int completeissues = 0;
                        int pendingissues = 0;
                        double completeissue_rates = 0;
                        double pendingissue_rates = 0;
                        try
                        {

                            totalissues = get_totalissue(row["id"].ToString(), "milestone");
                            completeissues = get_completeissue(row["id"].ToString(), "milestone");
                            if (completeissues > 0)
                            {
                                pendingissues = totalissues - completeissues;
                                completeissue_rates = Math.Round((Convert.ToDouble(completeissues) / totalissues) * 100, 2);
                                pendingissue_rates = 100 - completeissue_rates;
                            }
                            else
                            {
                                pendingissues = totalissues;
                                completeissue_rates = 0;
                                pendingissue_rates = 100;
                            }

                        }
                        catch
                        {

                        }
                        #endregion


                        addmilestones.Add(new newmilestone
                        {

                            id = row["id"] as int?,
                            ProjectId = row["ProjectId"] as int?,
                            Milestone = row["Milestone"].ToString(),
                            Weightage = row["Weightage"] as double?,
                            //StartDate =Convert.ToDateTime(row["StartDate"] as DateTime?),
                            StartDate = row["StartDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["StartDate"]) : null,
                            EndDate = row["EndDate"] as DateTime?,
                            Owner = row["Owner"].ToString(),
                            Access = row["Access"] as int?,
                            team = row["team"].ToString(),
                            P_Status = row["P_Status"] as int?,
                            status_name = get_Projectstatus(row["P_Status"].ToString()),
                            status_color = get_Status_Color(row["P_Status"].ToString()),
                            Priority = row["Priority"] as int?,
                            priority_name = get_PriorityName(row["Priority"].ToString()),
                            priority_color = get_PriorityColor(row["Priority"].ToString()),
                            Budget = row["Budget"] as double?,
                            Actual = row["Actual"] as double?,
                            uid = row["uid"] as int?,
                            cdate = row["cdate"] as DateTime?,
                            istask = row["istask"] as int?,
                            checkmileWeightage = row["checkmileWeightage"] as int?,
                            iscomplete = row["iscomplete"] as int?,
                            iscompletedate = row["iscompletedate"] as DateTime?,
                            iscompletepercentage = row["iscompletepercentage"] as int?,
                            Durationtype = row["Durationtype"] as int?,
                            TaskDuration = get_task_hrs(row["TaskDuration"].ToString(), "hrs"),
                            TaskDuration_min = get_task_hrs(row["TaskDuration"].ToString(), "min"),

                            Durationhourcal = row["Durationhourcal"] as double?,
                            orderno = row["orderno"] as int?,
                            miledescription = row["miledescription"].ToString(),
                            Uploaddrawing = row["Uploaddrawing"].ToString(),
                            txtdays = row["txtdays"].ToString(),
                            complete_value = Math.Round(mileTaskTotal, 2).ToString(),
                            pending_value = Math.Round(mileWattagepending, 2).ToString(),
                            taskcount = totaltasks,
                            isExpanded = false,
                            leftdays = tleftdays,
                            totalissue = totalissues,
                            completeissue = completeissues,
                            pendingissue = pendingissues,
                            completeissue_rate = completeissue_rates,
                            pendingissue_rate = pendingissue_rates,
                            totaldependency = totaldependencys,
                            owners = newownerlist,
                            teams = newteamlist,
                            viewfiles = newfileslist,
                            tasks = newtasklist



                        });
                    }

                    obj.miles = addmilestones;

                    // projectlist.Add(obj);
                }
                catch { }

            }


        }
        catch (Exception ex)
        {
            SaveLogText("chkadmintodayEX-" + ex.Message);
            return new project_details();
        }

        return obj;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static project_details bindProjectdetail_pre(string RecordId)
    {
        DataAccess da = new DataAccess();

        project_details obj = new project_details();
        try
        {
            if (string.IsNullOrEmpty(RecordId))
                RecordId = "0";

            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null)
                return obj;

            string regid = regCookie["id"];

            int chkadmin = 0;
            if (HttpContext.Current.Request.Cookies["user"]["user_id"] != null)
            {
                chkadmin = checkadmin(HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            }
            else
            {
                chkadmin = 0;
            }

            string strsql = "select * from Pms_tbl_AddProject where id=" + RecordId + " and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";

            //SaveLogText("mcount=" + strsql);
            DataSet dsP = da.GetDataSetInline(strsql);

            if (dsP.Tables[0].Rows.Count > 0)
            {
                List<ownerlist> newownerlist_p = getemployeelist<ownerlist>(dsP.Tables[0].Rows[0]["Owner"].ToString());
                List<teamlist> newteamlist_p = getemployeelist<teamlist>(dsP.Tables[0].Rows[0]["team"].ToString());

                string highlightcolor = geteditcolor();
                obj.id = dsP.Tables[0].Rows[0]["id"] != DBNull.Value ? Convert.ToInt32(dsP.Tables[0].Rows[0]["id"]) : (int?)null;
                obj.ProjectID = dsP.Tables[0].Rows[0]["ProjectID"].ToString();
                obj.ProjectName = dsP.Tables[0].Rows[0]["ProjectName"].ToString();
                obj.Owner = dsP.Tables[0].Rows[0]["Owner"].ToString();
                obj.team = dsP.Tables[0].Rows[0]["team"].ToString();
                obj.ProjectStatus = Convert.ToInt32(dsP.Tables[0].Rows[0]["ProjectStatus"].ToString() == "" ? "0" : dsP.Tables[0].Rows[0]["ProjectStatus"].ToString());
                obj.AwardDate = GetNullableDate(dsP.Tables[0].Rows[0]["AwardDate"]);
                obj.StartDate = GetNullableDate(dsP.Tables[0].Rows[0]["StartDate"]);
                obj.TargetDate = GetNullableDate(dsP.Tables[0].Rows[0]["TargetDate"]);
                obj.salesorderid = dsP.Tables[0].Rows[0]["salesorderid"] as int?;
                obj.customerid = dsP.Tables[0].Rows[0]["customerid"] as int?;
                obj.checkprojectWeightage = dsP.Tables[0].Rows[0]["checkprojectWeightage"] as int?;
                obj.hastask = dsP.Tables[0].Rows[0]["hastask"] as int?;
                obj.checktaskWeightage = dsP.Tables[0].Rows[0]["checktaskWeightage"] as int?;
                obj.hassubtask = dsP.Tables[0].Rows[0]["hassubtask"] as int?;
                obj.checksubtaskWeightage = dsP.Tables[0].Rows[0]["checksubtaskWeightage"] as int?;
                obj.editcolor = highlightcolor;
                obj.owners = newownerlist_p;
                obj.teams = newteamlist_p;
                obj.isadmin = chkadmin == 1 ? true : false;
                List<newmilestone> addmilestones = new List<newmilestone>();

                //string str2 = "select * from Pms_tbl_AddMilestone where ProjectId='" + obj.id + "' and status=1 and archive=0 order by isnull(orderno,0)";
                //DataAccess danew = new DataAccess();
                //DataSet drmile = danew.GetDataSetInline(str2);

                SqlParameter[] prm = new SqlParameter[2];
                prm[0] = new SqlParameter("@ProjectId", RecordId);
                prm[1] = new SqlParameter("@RegId", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                //SaveLogText("mcount=" + prm[0].Value+","+ prm[1].Value);

                DataSet drmile = da.GetDataSet("usp_GetMilestones_pre", prm);
                if (drmile.Tables[0].Rows.Count > 0)
                {

                    #region update copy data
                    string sqlupdt = "update pms_tbl_addmilestone set status=0 , archive=1 where projectid=" + RecordId + " and regid=" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + " and status=1 and archive=0";
                    sqlupdt += ";update pms_tbl_addtask set status=0 , archive=1 where projectid=" + RecordId + "  and regid=" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + " and status=1 and archive=0";
                    sqlupdt += ";update pms_tbl_addsubtask set status=0 , archive=1 where projectid=" + RecordId + "  and regid=" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + " and status=1 and archive=0";
                    da.executeDMLQueryinline(sqlupdt);
                    #endregion

                    foreach (DataRow row in drmile.Tables[0].Rows)
                    {
                        try
                        {

                            sqlupdt = "update pms_tbl_addmilestone set status=1 , archive=0 where projectid=" + RecordId + " and regid=" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + " and id=" + row["id"] + " and isnull(iscopy,0)=1";
                            da.executeDMLQueryinline(sqlupdt);

                            int totaltasks = 0;

                            List<ownerlist> newownerlist = getemployeelist<ownerlist>(row["Owner"].ToString());
                            List<teamlist> newteamlist = getemployeelist<teamlist>(row["team"].ToString());
                            List<projectfiles> newfileslist = bindfiledata(row["projectid"].ToString(), row["id"].ToString(), "milestone");
                            List<newtask> newtasklist = bindtasklist_pre_bymileid(row["id"].ToString());
                            List<issuestdata> newissueslist = bindissueslist(row["id"].ToString(), "milestone");


                            int totaldependencys = 0;
                            totaldependencys = Binddependencydata(row["id"].ToString(), "milestone");
                            totaltasks = checktaskby_milestoneid(row["id"].ToString());

                            double mileTaskTotal = 0;
                            mileTaskTotal = get_completionstatus(row["id"].ToString(), "milestone");

                            double mileWattagecomplite = mileTaskTotal;
                            double mileWattagepending = 100;
                            if (mileTaskTotal > 0)
                            {
                                mileWattagepending = mileWattagepending - mileTaskTotal;
                            }
                            string tleftdays = getleftdays_load(row["id"].ToString(), "milestone");
                            #region find issues
                            int totalissues = 0;
                            int completeissues = 0;
                            int pendingissues = 0;
                            double completeissue_rates = 0;
                            double pendingissue_rates = 0;
                            try
                            {

                                totalissues = get_totalissue(row["id"].ToString(), "milestone");
                                completeissues = get_completeissue(row["id"].ToString(), "milestone");
                                if (completeissues > 0)
                                {
                                    pendingissues = totalissues - completeissues;
                                    completeissue_rates = Math.Round((Convert.ToDouble(completeissues) / totalissues) * 100, 2);
                                    pendingissue_rates = 100 - completeissue_rates;
                                }
                                else
                                {
                                    pendingissues = totalissues;
                                    completeissue_rates = 0;
                                    pendingissue_rates = 100;
                                }

                            }
                            catch
                            {

                            }
                            #endregion

                            SaveLogText("mcount111=" + row["projectid"].ToString());
                            addmilestones.Add(new newmilestone
                            {

                                id = row["id"] as int?,
                                ProjectId = row["ProjectId"] as int?,
                                Milestone = row["Milestone"].ToString(),
                                Weightage = row["Weightage"] as double?,
                                //StartDate =Convert.ToDateTime(row["StartDate"] as DateTime?),
                                StartDate = row["StartDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["StartDate"]) : null,
                                EndDate = row["EndDate"] as DateTime?,
                                Owner = row["Owner"].ToString(),
                                Access = row["Access"] as int?,
                                team = row["team"].ToString(),
                                P_Status = row["P_Status"] as int?,
                                status_name = get_Projectstatus(row["P_Status"].ToString()),
                                status_color = get_Status_Color(row["P_Status"].ToString()),
                                Priority = row["Priority"] as int?,
                                priority_name = get_PriorityName(row["Priority"].ToString()),
                                priority_color = get_PriorityColor(row["Priority"].ToString()),
                                Budget = row["Budget"] as double?,
                                Actual = row["Actual"] as double?,
                                uid = row["uid"] as int?,
                                cdate = row["cdate"] as DateTime?,
                                istask = row["istask"] as int?,
                                checkmileWeightage = row["checkmileWeightage"] as int?,
                                iscomplete = row["iscomplete"] as int?,
                                iscompletedate = row["iscompletedate"] as DateTime?,
                                iscompletepercentage = row["iscompletepercentage"] as int?,
                                Durationtype = row["Durationtype"] as int?,
                                TaskDuration = get_task_hrs(row["TaskDuration"].ToString(), "hrs"),
                                TaskDuration_min = get_task_hrs(row["TaskDuration"].ToString(), "min"),

                                Durationhourcal = row["Durationhourcal"] as double?,
                                orderno = row["orderno"] as int?,
                                miledescription = row["miledescription"].ToString(),
                                Uploaddrawing = row["Uploaddrawing"].ToString(),
                                txtdays = row["txtdays"].ToString(),
                                complete_value = Math.Round(mileTaskTotal, 2).ToString(),
                                pending_value = Math.Round(mileWattagepending, 2).ToString(),
                                taskcount = totaltasks,
                                isExpanded = false,
                                leftdays = tleftdays,
                                totalissue = totalissues,
                                completeissue = completeissues,
                                pendingissue = pendingissues,
                                completeissue_rate = completeissue_rates,
                                pendingissue_rate = pendingissue_rates,
                                totaldependency = totaldependencys,
                                owners = newownerlist,
                                teams = newteamlist,
                                viewfiles = newfileslist,
                                tasks = newtasklist



                            });
                        }
                        catch (Exception ex)
                        {
                            SaveLogText("mcount=" + ex.Message);
                        }
                    }
                }
                obj.miles = addmilestones;

                // projectlist.Add(obj);


            }


        }
        catch (Exception ex)
        {
            SaveLogText("chkadmintodayEX-" + ex.Message);
            return new project_details();
        }

        return obj;
    }
    #endregion

    #region load task list
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static milestone_details bindmile_tasklist(string RecordId)
    {
        DataAccess da = new DataAccess();
        milestone_details obj = new milestone_details();
        try
        {
            if (string.IsNullOrEmpty(RecordId))
                RecordId = "0";

            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null)
                return obj;

            string regid = regCookie["id"];



            string strsql = "select * from Pms_tbl_Addmilestone where id=" + RecordId + " and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";


            SqlDataReader dr = da.getDataReaderInline(strsql);
            while (dr.Read())
            {

                obj.id = dr["id"] != DBNull.Value ? Convert.ToInt32(dr["id"]) : (int?)null;
                obj.ProjectId = dr["ProjectId"] != DBNull.Value ? Convert.ToInt32(dr["ProjectId"]) : (int?)null;
                obj.Milestone = dr["Milestone"].ToString();
                obj.Owner = dr["Owner"].ToString();
                obj.team = dr["team"].ToString();
                obj.P_Status = Convert.ToInt32(dr["P_Status"].ToString() == "" ? "0" : dr["P_Status"].ToString());
                obj.StartDate = GetNullableDate(dr["StartDate"]);
                obj.EndDate = GetNullableDate(dr["EndDate"]);
                obj.istask = dr["istask"] as int?;
                List<newtask> addtasks = new List<newtask>();
                try
                {
                    string str2 = "select * from Pms_tbl_Addtask where ProjectId='" + obj.ProjectId + "' and Milestoneid='" + RecordId + "' and status=1 and archive=0 order by isnull(orderno,0)";
                    DataAccess danew = new DataAccess();
                    DataSet drtask = danew.GetDataSetInline(str2);

                    foreach (DataRow row in drtask.Tables[0].Rows)
                    {
                        List<ownerlist> newownerlist = getemployeelist<ownerlist>(row["Owner"].ToString());
                        List<teamlist> newteamlist = getemployeelist<teamlist>(row["team"].ToString());
                        List<projectfiles> newfileslist = bindfiledata(row["ProjectId"].ToString(), row["id"].ToString(), "task");

                        double mileTaskTotal = 0;
                        mileTaskTotal = get_completionstatus(row["id"].ToString(), "milestone");
                        //SaveLogText("chkadmintodayEX-" + mileTaskTotal);
                        double mileWattagecomplite = mileTaskTotal;
                        double mileWattagepending = 100;
                        if (mileTaskTotal > 0)
                        {
                            mileWattagepending = mileWattagepending - mileTaskTotal;
                        }

                        addtasks.Add(new newtask
                        {

                            id = row["id"] as int?,
                            ProjectId = row["ProjectId"] as int?,
                            Milestoneid = row["Milestoneid"] as int?,
                            taskname = row["taskname"].ToString(),
                            Weightage = row["Weightage"] as double?,
                            Startdate = row["StartDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["StartDate"]) : null,
                            Targetdate = row["Targetdate"] as DateTime?,
                            owner = row["Owner"].ToString(),
                            team = row["team"].ToString(),
                            P_Status = row["P_Status"] as int?,
                            status_name = get_Projectstatus(row["P_Status"].ToString()),
                            status_color = get_Status_Color(row["P_Status"].ToString()),
                            Priority = row["Priority"] as int?,
                            priority_name = get_PriorityName(row["Priority"].ToString()),
                            priority_color = get_PriorityColor(row["Priority"].ToString()),
                            Budget = row["Budget"] as double?,
                            Actual = row["Actual"] as double?,
                            uid = row["uid"] as int?,
                            cdate = row["cdate"] as DateTime?,
                            issubtask = row["issubtask"] as int?,
                            iscompletepercentage = row["iscompletepercentage"] as double?,
                            TaskDuration = get_task_hrs(row["TaskDuration"].ToString(), "hrs"),
                            TaskDuration_min = get_task_hrs(row["TaskDuration"].ToString(), "min"),

                            orderno = row["orderno"] as int?,
                            taskdescripton = row["taskdescripton"].ToString(),
                            txtdays = row["txtdays"].ToString(),
                            complete_value = Math.Round(mileTaskTotal, 2).ToString(),
                            pending_value = Math.Round(mileWattagepending, 2).ToString(),
                            owners = newownerlist,
                            teams = newteamlist,
                            viewfiles = newfileslist



                        });
                    }

                    obj.tasks = addtasks;

                    // projectlist.Add(obj);
                }
                catch { }

            }


        }
        catch (Exception ex)
        {
            SaveLogText("chkadmintaskEX-" + ex.Message);
            return new milestone_details();
        }


        return obj;
    }

    public static List<newtask> bindtasklist_bymileid(string RecordId)
    {
        DataAccess da = new DataAccess();
        List<newtask> obj = new List<newtask>();

        try
        {
            if (string.IsNullOrEmpty(RecordId))
                RecordId = "0";

            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null)
                return obj;

            string regid = regCookie["id"];


            try
            {
                string str2 = "select * from Pms_tbl_Addtask where regid='" + regid + "' and Milestoneid='" + RecordId + "' and status=1 and archive=0 order by isnull(orderno,0)";

                SaveLogText("gettasklist-" + str2);
                DataAccess danew = new DataAccess();
                DataSet drtask = danew.GetDataSetInline(str2);

                foreach (DataRow row in drtask.Tables[0].Rows)
                {
                    List<ownerlist> newownerlist = getemployeelist<ownerlist>(row["Owner"].ToString());
                    List<teamlist> newteamlist = getemployeelist<teamlist>(row["team"].ToString());
                    List<projectfiles> newfileslist = bindfiledata(row["ProjectId"].ToString(), row["id"].ToString(), "task");
                    List<newsubtask> newsubtasklist = bindsubtasklist_bytaskid(row["id"].ToString());
                    SaveLogText("getsubtasklistcount-" + newsubtasklist.Count);
                    int totaldependencys = 0;
                    totaldependencys = Binddependencydata(row["id"].ToString(), "task");
                    double mileTaskTotal = 0;
                    mileTaskTotal = get_completionstatus(row["id"].ToString(), "task");
                    //SaveLogText("chkadmintodayEX-" + mileTaskTotal);
                    double mileWattagecomplite = mileTaskTotal;
                    double mileWattagepending = 100;
                    if (mileTaskTotal > 0)
                    {
                        mileWattagepending = mileWattagepending - mileTaskTotal;
                    }
                    string tleftdays = getleftdays_load(row["id"].ToString(), "task");
                    #region find issues
                    int totalissues = 0;
                    int completeissues = 0;
                    int pendingissues = 0;
                    double completeissue_rates = 0;
                    double pendingissue_rates = 0;
                    try
                    {

                        totalissues = get_totalissue(row["id"].ToString(), "task");
                        completeissues = get_completeissue(row["id"].ToString(), "task");
                        if (completeissues > 0)
                        {
                            pendingissues = totalissues - completeissues;
                            completeissue_rates = Math.Round((Convert.ToDouble(completeissues) / totalissues) * 100, 2);
                            pendingissue_rates = 100 - completeissue_rates;
                        }
                        else
                        {
                            pendingissues = totalissues;
                            completeissue_rates = 0;
                            pendingissue_rates = 100;
                        }

                    }
                    catch
                    {

                    }
                    #endregion
                    obj.Add(new newtask
                    {

                        id = row["id"] as int?,
                        ProjectId = row["ProjectId"] as int?,
                        Milestoneid = row["Milestoneid"] as int?,
                        taskname = row["taskname"].ToString(),
                        Weightage = row["Weightage"] as double?,
                        Startdate = row["StartDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["StartDate"]) : null,
                        Targetdate = row["Targetdate"] as DateTime?,
                        owner = row["Owner"].ToString(),
                        team = row["team"].ToString(),
                        P_Status = row["P_Status"] as int?,
                        status_name = get_Projectstatus(row["P_Status"].ToString()),
                        status_color = get_Status_Color(row["P_Status"].ToString()),
                        Priority = row["Priority"] as int?,
                        priority_name = get_PriorityName(row["Priority"].ToString()),
                        priority_color = get_PriorityColor(row["Priority"].ToString()),
                        Budget = row["Budget"] as double?,
                        Actual = row["Actual"] as double?,
                        uid = row["uid"] as int?,
                        cdate = row["cdate"] as DateTime?,
                        issubtask = row["issubtask"] as int?,
                        iscompletepercentage = row["iscompletepercentage"] as double?,
                        TaskDuration = get_task_hrs(row["TaskDuration"].ToString(), "hrs"),
                        TaskDuration_min = get_task_hrs(row["TaskDuration"].ToString(), "min"),
                        orderno = row["orderno"] as int?,
                        taskdescripton = row["taskdescripton"].ToString(),
                        txtdays = row["txtdays"].ToString(),
                        complete_value = Math.Round(mileTaskTotal, 2).ToString(),
                        pending_value = Math.Round(mileWattagepending, 2).ToString(),
                        isExpanded = false,
                        leftdays = tleftdays,
                        totalissue = totalissues,
                        completeissue = completeissues,
                        pendingissue = pendingissues,
                        completeissue_rate = completeissue_rates,
                        pendingissue_rate = pendingissue_rates,
                        totaldependency = totaldependencys,
                        owners = newownerlist,
                        teams = newteamlist,
                        viewfiles = newfileslist,
                        subtasks = newsubtasklist



                    });
                }



                // projectlist.Add(obj);
            }
            catch { }




        }
        catch (Exception ex)
        {
            SaveLogText("chkadmintaskEX-" + ex.Message);
            return new List<newtask>();
        }


        return obj;
    }
    public static List<newtask> bindtasklist_pre_bymileid(string RecordId)
    {
        DataAccess da = new DataAccess();
        List<newtask> obj = new List<newtask>();

        try
        {
            if (string.IsNullOrEmpty(RecordId))
                RecordId = "0";

            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null)
                return obj;

            string regid = regCookie["id"];


            try
            {
                string str2 = "select * from Pms_tbl_Addtask where regid='" + regid + "' and Milestoneid='" + RecordId + "' and status=0 and archive=1 and isnull(iscopy,0)=1 order by isnull(orderno,0)";

                SaveLogText("gettasklist-" + str2);
                DataAccess danew = new DataAccess();
                DataSet drtask = danew.GetDataSetInline(str2);

                foreach (DataRow row in drtask.Tables[0].Rows)
                {
                    List<ownerlist> newownerlist = getemployeelist<ownerlist>(row["Owner"].ToString());
                    List<teamlist> newteamlist = getemployeelist<teamlist>(row["team"].ToString());
                    List<projectfiles> newfileslist = bindfiledata(row["ProjectId"].ToString(), row["id"].ToString(), "task");
                    List<newsubtask> newsubtasklist = bindsubtasklist_pre_bytaskid(row["id"].ToString());
                    SaveLogText("getsubtasklistcount-" + newsubtasklist.Count);
                    int totaldependencys = 0;
                    totaldependencys = Binddependencydata(row["id"].ToString(), "task");
                    double mileTaskTotal = 0;
                    mileTaskTotal = get_completionstatus(row["id"].ToString(), "task");
                    //SaveLogText("chkadmintodayEX-" + mileTaskTotal);
                    double mileWattagecomplite = mileTaskTotal;
                    double mileWattagepending = 100;
                    if (mileTaskTotal > 0)
                    {
                        mileWattagepending = mileWattagepending - mileTaskTotal;
                    }
                    string tleftdays = getleftdays_load(row["id"].ToString(), "task");
                    #region find issues
                    int totalissues = 0;
                    int completeissues = 0;
                    int pendingissues = 0;
                    double completeissue_rates = 0;
                    double pendingissue_rates = 0;
                    try
                    {

                        totalissues = get_totalissue(row["id"].ToString(), "task");
                        completeissues = get_completeissue(row["id"].ToString(), "task");
                        if (completeissues > 0)
                        {
                            pendingissues = totalissues - completeissues;
                            completeissue_rates = Math.Round((Convert.ToDouble(completeissues) / totalissues) * 100, 2);
                            pendingissue_rates = 100 - completeissue_rates;
                        }
                        else
                        {
                            pendingissues = totalissues;
                            completeissue_rates = 0;
                            pendingissue_rates = 100;
                        }

                    }
                    catch
                    {

                    }
                    #endregion
                    obj.Add(new newtask
                    {

                        id = row["id"] as int?,
                        ProjectId = row["ProjectId"] as int?,
                        Milestoneid = row["Milestoneid"] as int?,
                        taskname = row["taskname"].ToString(),
                        Weightage = row["Weightage"] as double?,
                        Startdate = row["StartDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["StartDate"]) : null,
                        Targetdate = row["Targetdate"] as DateTime?,
                        owner = row["Owner"].ToString(),
                        team = row["team"].ToString(),
                        P_Status = row["P_Status"] as int?,
                        status_name = get_Projectstatus(row["P_Status"].ToString()),
                        status_color = get_Status_Color(row["P_Status"].ToString()),
                        Priority = row["Priority"] as int?,
                        priority_name = get_PriorityName(row["Priority"].ToString()),
                        priority_color = get_PriorityColor(row["Priority"].ToString()),
                        Budget = row["Budget"] as double?,
                        Actual = row["Actual"] as double?,
                        uid = row["uid"] as int?,
                        cdate = row["cdate"] as DateTime?,
                        issubtask = row["issubtask"] as int?,
                        iscompletepercentage = row["iscompletepercentage"] as double?,
                        TaskDuration = get_task_hrs(row["TaskDuration"].ToString(), "hrs"),
                        TaskDuration_min = get_task_hrs(row["TaskDuration"].ToString(), "min"),
                        orderno = row["orderno"] as int?,
                        taskdescripton = row["taskdescripton"].ToString(),
                        txtdays = row["txtdays"].ToString(),
                        complete_value = Math.Round(mileTaskTotal, 2).ToString(),
                        pending_value = Math.Round(mileWattagepending, 2).ToString(),
                        isExpanded = false,
                        leftdays = tleftdays,
                        totalissue = totalissues,
                        completeissue = completeissues,
                        pendingissue = pendingissues,
                        completeissue_rate = completeissue_rates,
                        pendingissue_rate = pendingissue_rates,
                        totaldependency = totaldependencys,
                        owners = newownerlist,
                        teams = newteamlist,
                        viewfiles = newfileslist,
                        subtasks = newsubtasklist



                    });
                }



                // projectlist.Add(obj);
            }
            catch { }




        }
        catch (Exception ex)
        {
            SaveLogText("chkadmintaskEX-" + ex.Message);
            return new List<newtask>();
        }


        return obj;
    }
    public static List<newtask_details> bindtaskdetails_bymileid(string RecordId)
    {
        DataAccess da = new DataAccess();
        List<newtask_details> obj = new List<newtask_details>();

        try
        {
            if (string.IsNullOrEmpty(RecordId))
                RecordId = "0";

            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null)
                return obj;

            string regid = regCookie["id"];


            try
            {
                string str2 = "select * from Pms_tbl_Addtask where regid='" + regid + "' and Milestoneid='" + RecordId + "' and status=1 and archive=0 order by isnull(orderno,0)";

                SaveLogText("gettasklist-" + str2);
                DataAccess danew = new DataAccess();
                DataSet drtask = danew.GetDataSetInline(str2);

                foreach (DataRow row in drtask.Tables[0].Rows)
                {

                    List<newsubtask_details> newsubtasklist = bindsubtaskdetails_bytaskid(row["id"].ToString());

                    double mileTaskTotal = 0;
                    mileTaskTotal = get_completionstatus(row["id"].ToString(), "task");
                    //SaveLogText("chkadmintodayEX-" + mileTaskTotal);
                    double mileWattagecomplite = mileTaskTotal;
                    double mileWattagepending = 100;
                    if (mileTaskTotal > 0)
                    {
                        mileWattagepending = mileWattagepending - mileTaskTotal;
                    }
                    obj.Add(new newtask_details
                    {
                        id = row["id"] as int?,
                        ProjectId = row["ProjectId"] as int?,
                        Milestoneid = row["Milestoneid"] as int?,
                        complete_value = Math.Round(mileTaskTotal, 2),
                        pending_value = Math.Round(mileWattagepending, 2),
                        subtasks_details = newsubtasklist

                    });
                }
                // projectlist.Add(obj);
            }
            catch { }
        }
        catch (Exception ex)
        {
            SaveLogText("chkadmintaskEX-" + ex.Message);
            return new List<newtask_details>();
        }


        return obj;
    }

    public static List<newsubtask> bindsubtasklist_bytaskid(string RecordId)
    {
        DataAccess da = new DataAccess();
        List<newsubtask> obj = new List<newsubtask>();
        try
        {
            if (string.IsNullOrEmpty(RecordId))
                RecordId = "0";

            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null)
                return obj;

            string regid = regCookie["id"];


            try
            {
                string str2 = "select * from Pms_tbl_Addsubtask where regid='" + regid + "' and task='" + RecordId + "' and status=1 and archive=0 order by isnull(orderno,0)";


                DataAccess danew = new DataAccess();
                DataSet drsubtask = danew.GetDataSetInline(str2);
                foreach (DataRow row in drsubtask.Tables[0].Rows)
                {
                    SaveLogText("getsubtaskobjlist-" + row["id"]);
                    List<ownerlist> newownerlist = getemployeelist<ownerlist>(row["Owner"].ToString());
                    List<teamlist> newteamlist = getemployeelist<teamlist>(row["team"].ToString());
                    List<projectfiles> newfileslist = bindfiledata(row["ProjectId"].ToString(), row["id"].ToString(), "subtask");
                    int totaldependencys = 0;
                    totaldependencys = Binddependencydata(row["id"].ToString(), "subtask");
                    double mileTaskTotal = 0;
                    mileTaskTotal = get_completionstatus(row["id"].ToString(), "subtask");
                    //SaveLogText("chkadmintodayEX-" + mileTaskTotal);
                    double mileWattagecomplite = mileTaskTotal;
                    double mileWattagepending = 100;
                    if (mileTaskTotal > 0)
                    {
                        mileWattagepending = mileWattagepending - mileTaskTotal;
                    }
                    string tleftdays = getleftdays_load(row["id"].ToString(), "subtask");
                    #region find issues
                    int totalissues = 0;
                    int completeissues = 0;
                    int pendingissues = 0;
                    double completeissue_rates = 0;
                    double pendingissue_rates = 0;
                    try
                    {

                        totalissues = get_totalissue(row["id"].ToString(), "subtask");
                        completeissues = get_completeissue(row["id"].ToString(), "subtask");
                        if (completeissues > 0)
                        {
                            pendingissues = totalissues - completeissues;
                            completeissue_rates = Math.Round((Convert.ToDouble(completeissues) / totalissues) * 100, 2);
                            pendingissue_rates = 100 - completeissue_rates;
                        }
                        else
                        {
                            pendingissues = totalissues;
                            completeissue_rates = 0;
                            pendingissue_rates = 100;
                        }

                    }
                    catch
                    {

                    }
                    #endregion
                    obj.Add(new newsubtask
                    {

                        id = row["id"] as int?,
                        ProjectId = row["ProjectId"] as int?,
                        Milestoneid = row["Milestoneid"] as int?,
                        Task = row["task"] as int?,
                        Subtaskname = row["Subtaskname"].ToString(),
                        Weightage = row["Weightage"] as double?,
                        Startdate = row["StartDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["StartDate"]) : null,
                        Targetdate = row["Targetdate"] as DateTime?,
                        owner = row["Owner"].ToString(),
                        team = row["team"].ToString(),
                        P_Status = row["P_Status"] as int?,
                        status_name = get_Projectstatus(row["P_Status"].ToString()),
                        status_color = get_Status_Color(row["P_Status"].ToString()),
                        Priority = row["Priority"] as int?,
                        priority_name = get_PriorityName(row["Priority"].ToString()),
                        priority_color = get_PriorityColor(row["Priority"].ToString()),
                        Budget = row["Budget"] as double?,
                        Actual = row["Actual"] as double?,
                        uid = row["uid"] as int?,
                        cdate = row["cdate"] as DateTime?,
                        iscompletepercentage = row["iscompletepercentage"] as double?,
                        TaskDuration = get_task_hrs(row["TaskDuration"].ToString(), "hrs"),
                        TaskDuration_min = get_task_hrs(row["TaskDuration"].ToString(), "min"),

                        orderno = row["orderno"] as int?,
                        SubTaskDescription = row["SubTaskDescription"].ToString(),
                        txtdays = row["txtdays"].ToString(),
                        complete_value = Math.Round(mileTaskTotal, 2).ToString(),
                        pending_value = Math.Round(mileWattagepending, 2).ToString(),
                        leftdays = tleftdays,
                        totalissue = totalissues,
                        completeissue = completeissues,
                        pendingissue = pendingissues,
                        completeissue_rate = completeissue_rates,
                        pendingissue_rate = pendingissue_rates,
                        totaldependency = totaldependencys,
                        owners = newownerlist,
                        teams = newteamlist,
                        viewfiles = newfileslist



                    });
                }




                // projectlist.Add(obj);
            }
            catch { }




        }
        catch (Exception ex)
        {
            SaveLogText("chkadminsubtaskEX-" + ex.Message);
            return new List<newsubtask>();
        }


        return obj;
    }
    public static List<newsubtask> bindsubtasklist_pre_bytaskid(string RecordId)
    {
        DataAccess da = new DataAccess();
        List<newsubtask> obj = new List<newsubtask>();
        try
        {
            if (string.IsNullOrEmpty(RecordId))
                RecordId = "0";

            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null)
                return obj;

            string regid = regCookie["id"];


            try
            {
                string str2 = "select * from Pms_tbl_Addsubtask where regid='" + regid + "' and task='" + RecordId + "' and status=0 and archive=1 and isnull(iscopy,0)=1 order by isnull(orderno,0)";


                DataAccess danew = new DataAccess();
                DataSet drsubtask = danew.GetDataSetInline(str2);
                foreach (DataRow row in drsubtask.Tables[0].Rows)
                {
                    SaveLogText("getsubtaskobjlist-" + row["id"]);
                    List<ownerlist> newownerlist = getemployeelist<ownerlist>(row["Owner"].ToString());
                    List<teamlist> newteamlist = getemployeelist<teamlist>(row["team"].ToString());
                    List<projectfiles> newfileslist = bindfiledata(row["ProjectId"].ToString(), row["id"].ToString(), "subtask");
                    int totaldependencys = 0;
                    totaldependencys = Binddependencydata(row["id"].ToString(), "subtask");
                    double mileTaskTotal = 0;
                    mileTaskTotal = get_completionstatus(row["id"].ToString(), "subtask");
                    //SaveLogText("chkadmintodayEX-" + mileTaskTotal);
                    double mileWattagecomplite = mileTaskTotal;
                    double mileWattagepending = 100;
                    if (mileTaskTotal > 0)
                    {
                        mileWattagepending = mileWattagepending - mileTaskTotal;
                    }
                    string tleftdays = getleftdays_load(row["id"].ToString(), "subtask");
                    #region find issues
                    int totalissues = 0;
                    int completeissues = 0;
                    int pendingissues = 0;
                    double completeissue_rates = 0;
                    double pendingissue_rates = 0;
                    try
                    {

                        totalissues = get_totalissue(row["id"].ToString(), "subtask");
                        completeissues = get_completeissue(row["id"].ToString(), "subtask");
                        if (completeissues > 0)
                        {
                            pendingissues = totalissues - completeissues;
                            completeissue_rates = Math.Round((Convert.ToDouble(completeissues) / totalissues) * 100, 2);
                            pendingissue_rates = 100 - completeissue_rates;
                        }
                        else
                        {
                            pendingissues = totalissues;
                            completeissue_rates = 0;
                            pendingissue_rates = 100;
                        }

                    }
                    catch
                    {

                    }
                    #endregion
                    obj.Add(new newsubtask
                    {

                        id = row["id"] as int?,
                        ProjectId = row["ProjectId"] as int?,
                        Milestoneid = row["Milestoneid"] as int?,
                        Task = row["task"] as int?,
                        Subtaskname = row["Subtaskname"].ToString(),
                        Weightage = row["Weightage"] as double?,
                        Startdate = row["StartDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["StartDate"]) : null,
                        Targetdate = row["Targetdate"] as DateTime?,
                        owner = row["Owner"].ToString(),
                        team = row["team"].ToString(),
                        P_Status = row["P_Status"] as int?,
                        status_name = get_Projectstatus(row["P_Status"].ToString()),
                        status_color = get_Status_Color(row["P_Status"].ToString()),
                        Priority = row["Priority"] as int?,
                        priority_name = get_PriorityName(row["Priority"].ToString()),
                        priority_color = get_PriorityColor(row["Priority"].ToString()),
                        Budget = row["Budget"] as double?,
                        Actual = row["Actual"] as double?,
                        uid = row["uid"] as int?,
                        cdate = row["cdate"] as DateTime?,
                        iscompletepercentage = row["iscompletepercentage"] as double?,
                        TaskDuration = get_task_hrs(row["TaskDuration"].ToString(), "hrs"),
                        TaskDuration_min = get_task_hrs(row["TaskDuration"].ToString(), "min"),

                        orderno = row["orderno"] as int?,
                        SubTaskDescription = row["SubTaskDescription"].ToString(),
                        txtdays = row["txtdays"].ToString(),
                        complete_value = Math.Round(mileTaskTotal, 2).ToString(),
                        pending_value = Math.Round(mileWattagepending, 2).ToString(),
                        leftdays = tleftdays,
                        totalissue = totalissues,
                        completeissue = completeissues,
                        pendingissue = pendingissues,
                        completeissue_rate = completeissue_rates,
                        pendingissue_rate = pendingissue_rates,
                        totaldependency = totaldependencys,
                        owners = newownerlist,
                        teams = newteamlist,
                        viewfiles = newfileslist



                    });
                }




                // projectlist.Add(obj);
            }
            catch { }




        }
        catch (Exception ex)
        {
            SaveLogText("chkadminsubtaskEX-" + ex.Message);
            return new List<newsubtask>();
        }


        return obj;
    }
    public static List<newsubtask_details> bindsubtaskdetails_bytaskid(string RecordId)
    {
        DataAccess da = new DataAccess();
        List<newsubtask_details> obj = new List<newsubtask_details>();
        try
        {
            if (string.IsNullOrEmpty(RecordId))
                RecordId = "0";

            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null)
                return obj;

            string regid = regCookie["id"];


            try
            {
                string str2 = "select * from Pms_tbl_Addsubtask where regid='" + regid + "' and task='" + RecordId + "' and status=1 and archive=0 order by isnull(orderno,0)";


                DataAccess danew = new DataAccess();
                DataSet drsubtask = danew.GetDataSetInline(str2);
                foreach (DataRow row in drsubtask.Tables[0].Rows)
                {
                    double mileTaskTotal = 0;
                    mileTaskTotal = get_completionstatus(row["id"].ToString(), "subtask");
                    double mileWattagecomplite = mileTaskTotal;
                    double mileWattagepending = 100;
                    if (mileTaskTotal > 0)
                    {
                        mileWattagepending = mileWattagepending - mileTaskTotal;
                    }
                    obj.Add(new newsubtask_details
                    {

                        id = row["id"] as int?,
                        ProjectId = row["ProjectId"] as int?,
                        Milestoneid = row["Milestoneid"] as int?,
                        Task = row["task"] as int?,
                        complete_value = Math.Round(mileTaskTotal, 2),
                        pending_value = Math.Round(mileWattagepending, 2),


                    });
                }




                // projectlist.Add(obj);
            }
            catch { }




        }
        catch (Exception ex)
        {
            SaveLogText("chkadminsubtaskEX-" + ex.Message);
            return new List<newsubtask_details>();
        }


        return obj;
    }


    #endregion

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
    protected static double get_completionstatus(string id, string type)
    {
        double strret = 0;
        string tblname = "Pms_tbl_AddMilestone";
        if (type == "task")
        {
            tblname = "Pms_tbl_Addtask";
        }
        else if (type == "subtask")
        {
            tblname = "Pms_tbl_Addsubtask";
        }
        if (id == "" || id == "0")
        {
            return 0;
        }
        DataAccess da = new DataAccess();

        double mileTaskTotal = 0;
        string findmile = "select cast(ROUND(isnull(iscompletepercentage,0),2) AS DECIMAL(10,2)) as iscompletepercentage from " + tblname + "  where id='" + id + "'  and status=1  and archive=0";
        SaveLogText("chkadmintodayEX-" + findmile);
        DataSet dsssummile = da.GetDataSetInline(findmile);

        try
        {
            mileTaskTotal = Convert.ToDouble(dsssummile.Tables[0].Rows[0]["iscompletepercentage"]);
        }
        catch (Exception)
        {
        }
        if (mileTaskTotal > 0)
        {
            strret = mileTaskTotal;

        }
        return strret;

    }
    protected static string get_Projectstatus(string statusid)
    {
        if (statusid == "" || statusid == "0")
        {
            return "-";
        }
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_Projectstatus where status=1 and archive=0 and id='" + statusid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString();

        }
        else
        {
            strret = "-";
        }

        return strret;

    }
    protected static string get_Status_Color(string statusid)
    {
        if (statusid == "" || statusid == "0")
        {
            return "-";
        }
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_Projectstatus where status=1 and archive=0 and id='" + statusid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["PrintColor"].ToString();

        }

        return strret;

    }
    protected static string get_task_hrs(string val, string gettype)
    {
        string retval = "";
        try
        {
            string[] ar_val = val.Split(':');
            if (gettype == "hrs")
            {
                retval = ar_val[0];
            }
            else if (gettype == "min")
            {
                if (ar_val.Length > 0)
                {
                    retval = ar_val[1];
                }

            }
        }
        catch { }
        return retval;

    }
    protected static string get_PriorityName(string priorityid)
    {
        if (priorityid == "" || priorityid == "0")
        {
            return "-";
        }
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_Priority where status=1 and archive=0 and Id='" + priorityid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["name"].ToString();

        }
        else
        {
            strret = "-";
        }

        return strret;

    }
    protected static string get_PriorityColor(string priorityid)
    {
        if (priorityid == "" || priorityid == "0")
        {
            return "-";
        }
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_Priority where status=1 and archive=0 and Id='" + priorityid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
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
    #region check admin login
    protected static int checkadmin(string struserid = "0")
    {
        int retval = 0;
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        function fns = new function();
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
    #endregion
    #region get highlight color
    public static string geteditcolor()
    {
        string retval = "";
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        function fns = new function();
        string strsql = "select id,PrintColor from Pms_tbl_editcolor  where Pms_tbl_editcolor.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["PrintColor"].ToString() == string.Empty)
            {
                retval = "#fde796";
            }
            else
            {
                retval = ds.Tables[0].Rows[0]["PrintColor"].ToString();
            }
        }
        return retval;
    }
    #endregion

    #region get user authentication
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
    #endregion


    #region savedata project
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string savedata_project(project_details model)
    {
        try
        {
            DataSet ds = new DataSet();
            DataAccess da = new DataAccess();
            CommonFunction cmf = new CommonFunction();
            function fns = new function();
            SqlParameter[] prm = new SqlParameter[68];
            //prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            //prm[1] = new SqlParameter("@cdate", da.getcdatetime());
            //prm[2] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
            //prm[3] = new SqlParameter("@ptype", (object)model.ptype ?? DBNull.Value);
            //prm[4] = new SqlParameter("@pname", (object)model.pname ?? DBNull.Value);
            //prm[5] = new SqlParameter("@Unitval", (object)model.Unitval ?? DBNull.Value);
            //prm[6] = new SqlParameter("@SKUval", (object)model.SKUval ?? DBNull.Value);
            //prm[7] = new SqlParameter("@exciseproduct", (object)model.exciseproduct ?? DBNull.Value);
            //prm[8] = new SqlParameter("@issale", (object)model.issale ?? DBNull.Value);
            //prm[9] = new SqlParameter("@salesRate", (object)model.salesRate ?? DBNull.Value);
            //prm[10] = new SqlParameter("@salesaccount", (object)model.salesaccount ?? DBNull.Value);
            //prm[11] = new SqlParameter("@salesDescription", (object)model.salesDescription ?? DBNull.Value);
            //prm[12] = new SqlParameter("@salesTaxtype", (object)model.salesTaxtype ?? DBNull.Value);
            //prm[13] = new SqlParameter("@salestaxvalue", (object)model.salestaxvalue ?? DBNull.Value);

            //prm[14] = new SqlParameter("@ispurchase", (object)model.ispurchase ?? "0");
            //prm[15] = new SqlParameter("@PurchaseRate", (object)model.PurchaseRate ?? DBNull.Value);
            //prm[16] = new SqlParameter("@Purchaseaccount", (object)model.Purchaseaccount ?? DBNull.Value);
            //prm[17] = new SqlParameter("@PurchaseDescription", (object)model.PurchaseDescription ?? DBNull.Value);
            //prm[18] = new SqlParameter("@PurchaseTaxtype", (object)model.PurchaseTaxtype ?? DBNull.Value);
            //prm[19] = new SqlParameter("@Purchasetaxvalue", (object)model.Purchasetaxvalue ?? DBNull.Value);

            //prm[20] = new SqlParameter("@isTrackInventory", ToDbInt(model.isTrackInventory));          
            //prm[21] = new SqlParameter("@trackAccount", (object)model.trackAccount ?? DBNull.Value);
            //prm[22] = new SqlParameter("@OpeningStock", (object)model.OpeningStock ?? DBNull.Value);
            //prm[23] = new SqlParameter("@OpeningStockrateperunit", (object)model.OpeningStockrateperunit ?? DBNull.Value);

            //prm[24] = new SqlParameter("ReturnValue", SqlDbType.Int);
            //prm[24].Direction = ParameterDirection.ReturnValue;

            //prm[25] = new SqlParameter("@pimg", (object)model.pimg ?? DBNull.Value);
            //prm[26] = new SqlParameter("@categoryid", ToDbInt(model.categoryid));
            //prm[27] = new SqlParameter("@subcategoryid", ToDbInt(model.subcategoryid));
            //prm[28] = new SqlParameter("@rsaleprice", (object)model.rsaleprice ?? DBNull.Value);
            //prm[29] = new SqlParameter("@rrentprice", (object)model.rrentprice ?? DBNull.Value);
            //prm[30] = new SqlParameter("@rtype", (object)model.rtype ?? DBNull.Value);
            //if ((object)model.issalepurchasesame == "1")
            //{
            //    prm[31] = new SqlParameter("@Unitval2", (object)model.Unitval ?? DBNull.Value);
            //    prm[32] = new SqlParameter("@unitrelationvalue", "1");
            //    prm[33] = new SqlParameter("@unitvalue", (object)model.unitvalue ?? "0");
            //    prm[34] = new SqlParameter("@unitvalue2", (object)model.unitvalue ?? "0");
            //}
            //else
            //{
            //    prm[31] = new SqlParameter("@Unitval2", (object)model.Unitval2 ?? DBNull.Value);
            //    prm[32] = new SqlParameter("@unitrelationvalue", (object)model.unitrelationvalue ?? "1");
            //    prm[33] = new SqlParameter("@unitvalue", (object)model.unitvalue ?? "0");
            //    prm[34] = new SqlParameter("@unitvalue2", (object)model.unitvalue2 ?? "0");
            //}
            //prm[35] = new SqlParameter("@barcode", (object)model.barcode ?? DBNull.Value);
            //prm[36] = new SqlParameter("@txtreorder", (object)model.txtreorder ?? DBNull.Value);
            //prm[37] = new SqlParameter("@istaxinclusive", (object)model.istaxinclusive ?? DBNull.Value);
            //prm[38] = new SqlParameter("@Wholesale_Price", (object)model.Wholesale_Price ?? DBNull.Value);
            //prm[39] = new SqlParameter("@Wholesale_istaxinclusive", (object)model.Wholesale_istaxinclusive ?? DBNull.Value);
            //prm[40] = new SqlParameter("@Franchise_Price", (object)model.Franchise_Price ?? DBNull.Value);
            //prm[41] = new SqlParameter("@Franchise_istaxinclusive", (object)model.Franchise_istaxinclusive ?? DBNull.Value);
            //prm[42] = new SqlParameter("@Discount", (object)model.Discount ?? DBNull.Value);
            //prm[43] = new SqlParameter("@txtfocprice", (object)model.txtfocprice ?? DBNull.Value);
            //prm[44] = new SqlParameter("@txtordercode", (object)model.txtordercode ?? DBNull.Value);
            //prm[45] = new SqlParameter("@CaseBag_Size", (object)model.CaseBag_Size ?? DBNull.Value);
            //prm[46] = new SqlParameter("@txtbrand", (object)model.txtbrand ?? DBNull.Value);
            //prm[47] = new SqlParameter("@PcsCase_Bag", (object)model.PcsCase_Bag ?? DBNull.Value);
            //prm[48] = new SqlParameter("@groupId", (object)model.groupId ?? DBNull.Value);
            //prm[49] = new SqlParameter("@txtspecificgravity", (object)model.txtspecificgravity ?? DBNull.Value);
            //prm[50] = new SqlParameter("@txtsize", (object)model.txtsize ?? DBNull.Value);
            //prm[51] = new SqlParameter("@txtopensize", (object)model.txtopensize ?? DBNull.Value);
            //prm[52] = new SqlParameter("@txtmicrion", (object)model.txtmicrion ?? DBNull.Value);
            //prm[53] = new SqlParameter("@txtcutlength", (object)model.txtcutlength ?? DBNull.Value);
            //prm[54] = new SqlParameter("@subcategory2id", ToDbInt(model.subcategory2id));
            //prm[55] = new SqlParameter("@subcategory3id", ToDbInt(model.subcategory3id));
            //prm[56] = new SqlParameter("@subcategory4id", ToDbInt(model.subcategory4id));
            //prm[57] = new SqlParameter("@MaxLimit", (object)model.MaxLimit ?? DBNull.Value);
            //prm[58] = new SqlParameter("@C_Cat", (object)model.C_Cat ?? DBNull.Value);
            //prm[59] = new SqlParameter("@C_Mod", (object)model.C_Mod ?? DBNull.Value);
            //prm[60] = new SqlParameter("@C_End", (object)model.C_End ?? DBNull.Value);
            //prm[61] = new SqlParameter("@C_Material", (object)model.C_Material ?? DBNull.Value);
            //prm[62] = new SqlParameter("@C_Pick", (object)model.C_Pick ?? DBNull.Value);
            //prm[63] = new SqlParameter("@C_Meter", (object)model.C_Meter ?? DBNull.Value);
            //prm[64] = new SqlParameter("@ddlinventorytype", (object)model.ddlinventorytype ?? "1");
            //prm[65] = new SqlParameter("@type", model.QType);
            //prm[66] = new SqlParameter("@issalepurchasesame", (object)model.issalepurchasesame ?? "0");
            //prm[67] = new SqlParameter("@id", (object)model.id ?? "0");

            //int cusid = 0;
            //cusid = da.executeDMLQuery("angular_usp_tbl_itemsdetails", prm);


            return "success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    #endregion
    #region savedata milestone
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static int addnewmilestonerrequest(newMilerequest request)
    {
        DataAccess da1 = new DataAccess();
        SaveLogText("newMilerequest-" + request);
        var obj = request.milestone;
        int newOrderNo = request.orderno;
        string strpid = obj.ProjectId.ToString();
        string strweight = "";
        string strdurationtype = "";
        string strduration = "";
        string stristask = "";
        string strstartdate = obj.StartDate != null ? obj.StartDate.ToString() : da1.getcdate();
        string strenddate = obj.EndDate != null ? obj.EndDate.ToString() : da1.getcdate();
        string strowner = obj.Owner != null ? obj.Owner.ToString() : "";
        string strteam = obj.team != null ? obj.team.ToString() : "";
        string strtimeline = "";
        string strstatus = obj.P_Status.ToString();
        string strpriority = obj.Priority.ToString();
        SqlParameter[] prm = new SqlParameter[23];
        prm[0] = new SqlParameter("@id", "0");
        prm[1] = new SqlParameter("@ProjectId", strpid);
        prm[2] = new SqlParameter("@Milestone", DBNull.Value);
        prm[3] = new SqlParameter("@Weightage", strweight);
        prm[4] = new SqlParameter("@StartDate", strstartdate);
        prm[5] = new SqlParameter("@EndDate", strenddate);
        if (strowner != "")
        {
            prm[6] = new SqlParameter("@Owner", strowner);
        }
        else
        {
            prm[6] = new SqlParameter("@Owner", DBNull.Value);
        }
        prm[7] = new SqlParameter("@Access", DBNull.Value);
        if (strteam != "")
        {
            prm[8] = new SqlParameter("@team", strteam);
        }
        else
        {
            prm[8] = new SqlParameter("@team", DBNull.Value);
        }
        if (strteam != "")
        {
            prm[9] = new SqlParameter("@P_Status", strstatus);
        }
        else
        {
            prm[9] = new SqlParameter("@P_Status", DBNull.Value);
        }



        if (strpriority != "")
        {
            prm[10] = new SqlParameter("@Priority", strpriority);
        }
        else
        {
            prm[10] = new SqlParameter("@Priority", DBNull.Value);
        }
        prm[11] = new SqlParameter("@Budget", DBNull.Value);
        prm[12] = new SqlParameter("@Actual", DBNull.Value);
        prm[13] = new SqlParameter("@checkmileWeightage", DBNull.Value);
        if (strdurationtype != "")
        {
            prm[14] = new SqlParameter("@Durationtype", strdurationtype);
        }
        else
        {
            prm[14] = new SqlParameter("@Durationtype", DBNull.Value);
        }

        if (strduration != "")
        {
            prm[15] = new SqlParameter("@TaskDuration", strduration);
        }
        else
        {
            prm[15] = new SqlParameter("@TaskDuration", DBNull.Value);
        }

        prm[16] = new SqlParameter("@istask", DBNull.Value);
        prm[17] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[18] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[19] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[20] = new SqlParameter("@type", "Insert");
        prm[21] = new SqlParameter("@orderno", newOrderNo);
        prm[22] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[22].Direction = ParameterDirection.ReturnValue;
        int retval1 = da1.executeDMLQuery("pmstbl_Addmilestone_usp_ang", prm);

        try
        {

        }
        catch { }
        return retval1;
        // Save to DB and return new id
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static int deletemilestone(string id)
    {
        int ret = 0;
        DataAccess da1 = new DataAccess();
        try
        {
            string sqlq = "update pms_tbl_addmilestone set status=0 , archive=1 where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + id + "'";
            SaveLogText("newMilerequest-" + sqlq);
            da1.executeDMLQueryinline(sqlq);
            string chkval = managecalculation(id, "milestone");
            ret = 1;



        }
        catch { }
        return ret;
        // Save to DB and return new id
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static int updatemilestone_order(string id, string orderno)
    {
        int ret = 0;
        DataAccess da1 = new DataAccess();
        try
        {
            string sqlq = "update pms_tbl_addmilestone set orderno='" + orderno + "' where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + id + "'";
            SaveLogText("newMilerequest-" + sqlq);
            da1.executeDMLQueryinline(sqlq);
            ret = 1;


        }
        catch { }
        return ret;
        // Save to DB and return new id
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string savedata_milestone(newmilestone model)
    {
        try
        {
            DataSet ds = new DataSet();
            DataAccess da = new DataAccess();
            CommonFunction cmf = new CommonFunction();
            function fns = new function();
            SqlParameter[] prm = new SqlParameter[68];
            //prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            //prm[1] = new SqlParameter("@cdate", da.getcdatetime());
            //prm[2] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
            //prm[3] = new SqlParameter("@ptype", (object)model.ptype ?? DBNull.Value);
            //prm[4] = new SqlParameter("@pname", (object)model.pname ?? DBNull.Value);
            //prm[5] = new SqlParameter("@Unitval", (object)model.Unitval ?? DBNull.Value);
            //prm[6] = new SqlParameter("@SKUval", (object)model.SKUval ?? DBNull.Value);
            //prm[7] = new SqlParameter("@exciseproduct", (object)model.exciseproduct ?? DBNull.Value);
            //prm[8] = new SqlParameter("@issale", (object)model.issale ?? DBNull.Value);
            //prm[9] = new SqlParameter("@salesRate", (object)model.salesRate ?? DBNull.Value);
            //prm[10] = new SqlParameter("@salesaccount", (object)model.salesaccount ?? DBNull.Value);
            //prm[11] = new SqlParameter("@salesDescription", (object)model.salesDescription ?? DBNull.Value);
            //prm[12] = new SqlParameter("@salesTaxtype", (object)model.salesTaxtype ?? DBNull.Value);
            //prm[13] = new SqlParameter("@salestaxvalue", (object)model.salestaxvalue ?? DBNull.Value);

            //prm[14] = new SqlParameter("@ispurchase", (object)model.ispurchase ?? "0");
            //prm[15] = new SqlParameter("@PurchaseRate", (object)model.PurchaseRate ?? DBNull.Value);
            //prm[16] = new SqlParameter("@Purchaseaccount", (object)model.Purchaseaccount ?? DBNull.Value);
            //prm[17] = new SqlParameter("@PurchaseDescription", (object)model.PurchaseDescription ?? DBNull.Value);
            //prm[18] = new SqlParameter("@PurchaseTaxtype", (object)model.PurchaseTaxtype ?? DBNull.Value);
            //prm[19] = new SqlParameter("@Purchasetaxvalue", (object)model.Purchasetaxvalue ?? DBNull.Value);

            //prm[20] = new SqlParameter("@isTrackInventory", ToDbInt(model.isTrackInventory));          
            //prm[21] = new SqlParameter("@trackAccount", (object)model.trackAccount ?? DBNull.Value);
            //prm[22] = new SqlParameter("@OpeningStock", (object)model.OpeningStock ?? DBNull.Value);
            //prm[23] = new SqlParameter("@OpeningStockrateperunit", (object)model.OpeningStockrateperunit ?? DBNull.Value);

            //prm[24] = new SqlParameter("ReturnValue", SqlDbType.Int);
            //prm[24].Direction = ParameterDirection.ReturnValue;

            //prm[25] = new SqlParameter("@pimg", (object)model.pimg ?? DBNull.Value);
            //prm[26] = new SqlParameter("@categoryid", ToDbInt(model.categoryid));
            //prm[27] = new SqlParameter("@subcategoryid", ToDbInt(model.subcategoryid));
            //prm[28] = new SqlParameter("@rsaleprice", (object)model.rsaleprice ?? DBNull.Value);
            //prm[29] = new SqlParameter("@rrentprice", (object)model.rrentprice ?? DBNull.Value);
            //prm[30] = new SqlParameter("@rtype", (object)model.rtype ?? DBNull.Value);
            //if ((object)model.issalepurchasesame == "1")
            //{
            //    prm[31] = new SqlParameter("@Unitval2", (object)model.Unitval ?? DBNull.Value);
            //    prm[32] = new SqlParameter("@unitrelationvalue", "1");
            //    prm[33] = new SqlParameter("@unitvalue", (object)model.unitvalue ?? "0");
            //    prm[34] = new SqlParameter("@unitvalue2", (object)model.unitvalue ?? "0");
            //}
            //else
            //{
            //    prm[31] = new SqlParameter("@Unitval2", (object)model.Unitval2 ?? DBNull.Value);
            //    prm[32] = new SqlParameter("@unitrelationvalue", (object)model.unitrelationvalue ?? "1");
            //    prm[33] = new SqlParameter("@unitvalue", (object)model.unitvalue ?? "0");
            //    prm[34] = new SqlParameter("@unitvalue2", (object)model.unitvalue2 ?? "0");
            //}
            //prm[35] = new SqlParameter("@barcode", (object)model.barcode ?? DBNull.Value);
            //prm[36] = new SqlParameter("@txtreorder", (object)model.txtreorder ?? DBNull.Value);
            //prm[37] = new SqlParameter("@istaxinclusive", (object)model.istaxinclusive ?? DBNull.Value);
            //prm[38] = new SqlParameter("@Wholesale_Price", (object)model.Wholesale_Price ?? DBNull.Value);
            //prm[39] = new SqlParameter("@Wholesale_istaxinclusive", (object)model.Wholesale_istaxinclusive ?? DBNull.Value);
            //prm[40] = new SqlParameter("@Franchise_Price", (object)model.Franchise_Price ?? DBNull.Value);
            //prm[41] = new SqlParameter("@Franchise_istaxinclusive", (object)model.Franchise_istaxinclusive ?? DBNull.Value);
            //prm[42] = new SqlParameter("@Discount", (object)model.Discount ?? DBNull.Value);
            //prm[43] = new SqlParameter("@txtfocprice", (object)model.txtfocprice ?? DBNull.Value);
            //prm[44] = new SqlParameter("@txtordercode", (object)model.txtordercode ?? DBNull.Value);
            //prm[45] = new SqlParameter("@CaseBag_Size", (object)model.CaseBag_Size ?? DBNull.Value);
            //prm[46] = new SqlParameter("@txtbrand", (object)model.txtbrand ?? DBNull.Value);
            //prm[47] = new SqlParameter("@PcsCase_Bag", (object)model.PcsCase_Bag ?? DBNull.Value);
            //prm[48] = new SqlParameter("@groupId", (object)model.groupId ?? DBNull.Value);
            //prm[49] = new SqlParameter("@txtspecificgravity", (object)model.txtspecificgravity ?? DBNull.Value);
            //prm[50] = new SqlParameter("@txtsize", (object)model.txtsize ?? DBNull.Value);
            //prm[51] = new SqlParameter("@txtopensize", (object)model.txtopensize ?? DBNull.Value);
            //prm[52] = new SqlParameter("@txtmicrion", (object)model.txtmicrion ?? DBNull.Value);
            //prm[53] = new SqlParameter("@txtcutlength", (object)model.txtcutlength ?? DBNull.Value);
            //prm[54] = new SqlParameter("@subcategory2id", ToDbInt(model.subcategory2id));
            //prm[55] = new SqlParameter("@subcategory3id", ToDbInt(model.subcategory3id));
            //prm[56] = new SqlParameter("@subcategory4id", ToDbInt(model.subcategory4id));
            //prm[57] = new SqlParameter("@MaxLimit", (object)model.MaxLimit ?? DBNull.Value);
            //prm[58] = new SqlParameter("@C_Cat", (object)model.C_Cat ?? DBNull.Value);
            //prm[59] = new SqlParameter("@C_Mod", (object)model.C_Mod ?? DBNull.Value);
            //prm[60] = new SqlParameter("@C_End", (object)model.C_End ?? DBNull.Value);
            //prm[61] = new SqlParameter("@C_Material", (object)model.C_Material ?? DBNull.Value);
            //prm[62] = new SqlParameter("@C_Pick", (object)model.C_Pick ?? DBNull.Value);
            //prm[63] = new SqlParameter("@C_Meter", (object)model.C_Meter ?? DBNull.Value);
            //prm[64] = new SqlParameter("@ddlinventorytype", (object)model.ddlinventorytype ?? "1");
            //prm[65] = new SqlParameter("@type", model.QType);
            //prm[66] = new SqlParameter("@issalepurchasesame", (object)model.issalepurchasesame ?? "0");
            //prm[67] = new SqlParameter("@id", (object)model.id ?? "0");

            //int cusid = 0;
            //cusid = da.executeDMLQuery("angular_usp_tbl_itemsdetails", prm);


            return "success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    #endregion
    #region savedata task
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static int addnewtaskrequest(newTaskrequest request)
    {
        DataAccess da1 = new DataAccess();
        var obj = request.task;
        int newOrderNo = request.orderno;
        string strpid = obj.ProjectId.ToString();
        string strstartdate = obj.Startdate != null ? obj.Startdate.ToString() : da1.getcdate();
        string strenddate = obj.Targetdate != null ? obj.Targetdate.ToString() : da1.getcdate();
        string strowner = obj.owner != null ? obj.owner.ToString() : "";
        string strteam = obj.team != null ? obj.team.ToString() : "";
        string strstatus = obj.P_Status.ToString();
        string strpriority = obj.Priority.ToString();
        SqlParameter[] prm = new SqlParameter[35];
        prm[0] = new SqlParameter("@id", "0");
        prm[1] = new SqlParameter("@ProjectId", obj.ProjectId);
        prm[2] = new SqlParameter("@Milestoneid", obj.Milestoneid);
        prm[3] = new SqlParameter("@tasklist", DBNull.Value);
        prm[4] = new SqlParameter("@taskname", DBNull.Value);
        prm[5] = new SqlParameter("@taskdescripton", DBNull.Value);
        prm[6] = new SqlParameter("@owner", obj.owner);
        prm[7] = new SqlParameter("@Startdate", strstartdate);
        prm[8] = new SqlParameter("@Targetdate", strenddate);
        prm[9] = new SqlParameter("@StartTime", DBNull.Value);
        prm[10] = new SqlParameter("@Timeline", DBNull.Value);
        prm[11] = new SqlParameter("@TaskDuration", obj.TaskDuration);
        prm[12] = new SqlParameter("@P_Status", obj.P_Status);
        prm[13] = new SqlParameter("@Priority", obj.Priority);
        prm[14] = new SqlParameter("@Budget", DBNull.Value);
        prm[15] = new SqlParameter("@Actual", DBNull.Value);
        prm[16] = new SqlParameter("@Reminder", DBNull.Value);
        prm[17] = new SqlParameter("@ReminderType", DBNull.Value);
        prm[18] = new SqlParameter("@Days", DBNull.Value);
        prm[19] = new SqlParameter("@Time", DBNull.Value);
        prm[20] = new SqlParameter("@Notifyusers", DBNull.Value);
        prm[21] = new SqlParameter("@ChooseDate", DBNull.Value);
        prm[22] = new SqlParameter("@Occurrence", DBNull.Value);
        prm[23] = new SqlParameter("@tagto", DBNull.Value);
        prm[24] = new SqlParameter("@team", obj.team);
        prm[25] = new SqlParameter("@Stage", DBNull.Value);
        prm[26] = new SqlParameter("@Weightage", obj.Weightage);
        prm[27] = new SqlParameter("@issubtask", DBNull.Value);
        prm[28] = new SqlParameter("@checkTaskWeightage", DBNull.Value);
        prm[29] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[30] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[31] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[32] = new SqlParameter("@type", "Insert");
        prm[33] = new SqlParameter("@Uploaddrawing", DBNull.Value);
        prm[34] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[34].Direction = ParameterDirection.ReturnValue;
        int retval1 = da1.executeDMLQuery("pms_Adtask_usp_ang", prm);
        return retval1;
        // Save to DB and return new id
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static int deletetask(string id)
    {
        int ret = 0;
        DataAccess da1 = new DataAccess();
        try
        {
            string sqlq = "update pms_tbl_addtask set status=0 , archive=1 where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + id + "'";
            da1.executeDMLQueryinline(sqlq);
            string chkval = managecalculation(id, "task");
            ret = 1;


        }
        catch { }
        return ret;
        // Save to DB and return new id
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static int updatetask_order(string id, string orderno)
    {
        int ret = 0;
        DataAccess da1 = new DataAccess();
        try
        {
            string sqlq = "update pms_tbl_addtask set orderno='" + orderno + "' where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + id + "'";
            da1.executeDMLQueryinline(sqlq);
            ret = 1;


        }
        catch { }
        return ret;
        // Save to DB and return new id
    }
    #endregion
    #region savedata sub task
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static int addnewsubtaskrequest(newSubTaskrequest request)
    {
        DataAccess da1 = new DataAccess();
        var obj = request.subtask;
        int newOrderNo = request.orderno;
        string strpid = obj.ProjectId.ToString();

        string strstartdate = obj.Startdate != null ? obj.Startdate.ToString() : da1.getcdate();
        string strenddate = obj.Targetdate != null ? obj.Targetdate.ToString() : da1.getcdate();
        string strowner = obj.owner != null ? obj.owner.ToString() : "";
        string strteam = obj.team != null ? obj.team.ToString() : "";
        string strtimeline = "";
        string strstatus = obj.P_Status.ToString();
        string strpriority = obj.Priority.ToString();
        SqlParameter[] prm = new SqlParameter[34];
        prm[0] = new SqlParameter("@id", "0");
        prm[1] = new SqlParameter("@ProjectId", obj.ProjectId);
        prm[2] = new SqlParameter("@Milestoneid", obj.Milestoneid);
        prm[3] = new SqlParameter("@tasklist", DBNull.Value);
        prm[4] = new SqlParameter("@task", obj.Task);
        prm[5] = new SqlParameter("@Subtaskname", DBNull.Value);
        prm[6] = new SqlParameter("@SubTaskDescription", DBNull.Value);
        prm[7] = new SqlParameter("@owner", obj.owner);
        prm[8] = new SqlParameter("@Startdate", strstartdate);
        prm[9] = new SqlParameter("@Targetdate", strenddate);
        prm[10] = new SqlParameter("@StartTime", DBNull.Value);
        prm[11] = new SqlParameter("@Timeline", DBNull.Value);
        prm[12] = new SqlParameter("@TaskDuration", obj.TaskDuration);
        prm[13] = new SqlParameter("@P_Status", obj.P_Status);
        prm[14] = new SqlParameter("@Priority", obj.Priority);
        prm[15] = new SqlParameter("@Budget", DBNull.Value);
        prm[16] = new SqlParameter("@Actual", DBNull.Value);
        prm[17] = new SqlParameter("@Reminder", DBNull.Value);
        prm[18] = new SqlParameter("@ReminderType", DBNull.Value);
        prm[19] = new SqlParameter("@Days", DBNull.Value);
        prm[20] = new SqlParameter("@Time", DBNull.Value);
        prm[21] = new SqlParameter("@Notifyusers", DBNull.Value);
        prm[22] = new SqlParameter("@ChooseDate", DBNull.Value);
        prm[23] = new SqlParameter("@Occurrence", DBNull.Value);
        prm[24] = new SqlParameter("@tagto", DBNull.Value);
        prm[25] = new SqlParameter("@team", obj.team);
        prm[26] = new SqlParameter("@Stage", DBNull.Value);
        prm[27] = new SqlParameter("@Weightage", obj.Weightage);
        prm[28] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[29] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[30] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[31] = new SqlParameter("@type", "Insert");
        prm[32] = new SqlParameter("@Uploaddrawing", DBNull.Value);
        prm[33] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[33].Direction = ParameterDirection.ReturnValue;
        int retval1 = da1.executeDMLQuery("pms_Add_Subtask_usp_ang", prm);
        return retval1;
        // Save to DB and return new id
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static int deletesubtask(string id)
    {
        int ret = 0;
        DataAccess da1 = new DataAccess();
        try
        {
            string sqlq = "update pms_tbl_addsubtask set status=0 , archive=1 where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + id + "'";
            da1.executeDMLQueryinline(sqlq);
            string chkval = managecalculation(id, "subtask");
            ret = 1;


        }
        catch { }
        return ret;
        // Save to DB and return new id
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static int updatesubtask_order(string id, string orderno)
    {
        int ret = 0;
        DataAccess da1 = new DataAccess();
        try
        {
            string sqlq = "update pms_tbl_addsubtask set orderno='" + orderno + "' where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + id + "'";
            da1.executeDMLQueryinline(sqlq);
            ret = 1;


        }
        catch { }
        return ret;
        // Save to DB and return new id
    }
    #endregion

    #region update details



    #region update status
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
                retval = "1";
                //DataAccess da = new DataAccess();
                //string strsql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus where  status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + statusid + "' ";
                //DataSet dscode = da.GetDataSetInline(strsql);
                //if (dscode.Tables[0].Rows.Count > 0)
                //{
                //    retval = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
                //}
            }


            //string chkval = managecalculation(milstoneid, "milestone");

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
            retval = "1";
            //DataAccess da = new DataAccess();
            //string strsql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus where  status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + statusid + "' ";
            //DataSet dscode = da.GetDataSetInline(strsql);
            //if (dscode.Tables[0].Rows.Count > 0)
            //{
            //    retval = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
            //}


        }
        // percentage update end  // 





        // return dscode.GetXml();
        return retval;
    }
    [WebMethod]
    public static item_summary UpdateMilestone_StatusN(string milstoneid = "", string statusid = "", string iscomplete = "")
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
                retval = "1";
                //DataAccess da = new DataAccess();
                //string strsql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus where  status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + statusid + "' ";
                //DataSet dscode = da.GetDataSetInline(strsql);
                //if (dscode.Tables[0].Rows.Count > 0)
                //{
                //    retval = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
                //}
            }


            //string chkval = managecalculation(milstoneid, "milestone");

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
            retval = "1";
            //DataAccess da = new DataAccess();
            //string strsql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus where  status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + statusid + "' ";
            //DataSet dscode = da.GetDataSetInline(strsql);
            //if (dscode.Tables[0].Rows.Count > 0)
            //{
            //    retval = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
            //}


        }
        // percentage update end  // 

        item_summary _summary = new item_summary();
        _summary.status = retval;
        double mileTaskTotal = 0;
        double mileWattagepending = 100;
        try
        {

            mileTaskTotal = get_completionstatus(milstoneid, "milestone");
            if (mileTaskTotal > 0)
            {
                mileWattagepending = mileWattagepending - mileTaskTotal;
            }
        }
        catch { }
        _summary.status = retval;
        _summary.copmelete_value = mileTaskTotal;
        _summary.pending_value = mileWattagepending;


        // return dscode.GetXml();
        return _summary;
    }
    [WebMethod]
    public static string UpdateTask_Status(string taskid = "", string statusid = "", string iscomplete = "")
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
                retval = "1";
                //string strsql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + statusid + "' ";
                //DataSet dscode = da1.GetDataSetInline(strsql);
                //if (dscode.Tables[0].Rows.Count > 0)
                //{
                //    retval = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
                //}
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
            retval = "1";
            //string strsql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + statusid + "' ";
            //DataSet dscode = da1.GetDataSetInline(strsql);
            //if (dscode.Tables[0].Rows.Count > 0)
            //{
            //    retval = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
            //}

        }

        // percentage update end  // 




        return retval;
    }

    [WebMethod]
    public static string UpdateSubtask_Status(string subtaskid = "", string statusid = "", string iscomplete = "")
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
                SaveLogText("finsql2-" + finsql1);
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


                retval = "1";

                //string strsql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + statusid + "' ";
                //DataSet dscode = da1.GetDataSetInline(strsql);
                //if (dscode.Tables[0].Rows.Count > 0)
                //{
                //    retval = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
                //}
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
            retval = "1";



            //string strsql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + statusid + "' ";
            //DataSet dscode = da1.GetDataSetInline(strsql);
            //if (dscode.Tables[0].Rows.Count > 0)
            //{
            //    retval = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
            //}


        }
        // percentage update end  // 




        return retval;
    }


    #endregion
    #region update priority
    [WebMethod]
    public static string MileUpdatePriority_Status(string milstoneid = "", string priorityid = "")
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
        prm[1] = new SqlParameter("@Priority", priorityid);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pmstbl_Priority_milstone_DtlsUpdate", prm);

        DataAccess da = new DataAccess();
        string strsql = "select Id,name,PrintColor from Pms_tbl_Priority where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + priorityid + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            retval = dscode.Tables[0].Rows[0]["name"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
        }
        // return dscode.GetXml();
        return retval;
    }
    [WebMethod]
    public static string TaskUpdatePriority_Status(string taskid = "", string priorityid = "")
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
        prm[1] = new SqlParameter("@Priority", priorityid);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pms_tbl_task_Priority_DeatilsUpdate", prm);

        DataAccess da = new DataAccess();
        string strsql = "select Id,name,PrintColor from Pms_tbl_Priority  where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + priorityid + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            retval = dscode.Tables[0].Rows[0]["name"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
        }

        return retval;
    }


    [WebMethod]
    public static string SubtaskUpdatePriority_Status(string subtaskid = "", string priorityid = "")
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
        prm[1] = new SqlParameter("@Priority", priorityid);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pms_tbl_Subtask_Priority_DeatilsUpdate", prm);

        DataAccess da = new DataAccess();
        string strsql = "select Id,name,PrintColor from Pms_tbl_Priority  where  status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + priorityid + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            retval = dscode.Tables[0].Rows[0]["name"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
        }

        return retval;

    }

    #endregion
    #region update owner

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string updateOwner(string id, string ownerId, string utype)
    {
        string retval = "0";
        DataAccess da1 = new DataAccess();
        if (id != string.Empty && id != "0")
        {
            string tblname = "";
            if (utype == "milestone")
            {
                tblname = "Pms_tbl_AddMilestone";
            }
            else if (utype == "task")
            {
                tblname = "Pms_tbl_Addtask";
            }
            else if (utype == "subtask")
            {
                tblname = "Pms_tbl_Addsubtask";
            }
            if (tblname != string.Empty)
            {
                try
                {
                    string strownerupdate = "update " + tblname + " set owner='" + ownerId + "' where id='" + id + "' and status=1 and archive=0 ";
                    SaveLogText("strownerupdate-" + strownerupdate);
                    da1.executeDMLQueryinline(strownerupdate);
                    retval = "1";
                }
                catch { }
            }

        }
        return retval;
    }

    #endregion
    #region update custom_col
    [WebMethod]
    public static string MileUpdateDetails(string id, string col_name, string col_value)
    {
        string retval = "0";
        DataAccess da1 = new DataAccess();
        try
        {
            string strsql = " update Pms_tbl_AddMilestone set " + col_name + "='" + col_value + "'  where status=1 and archive=0 and id='" + id + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
            da1.executeDMLQueryinline(strsql);
            retval = "1";
            if (col_name.ToLower() == "weightage")
            {
                string chkval = managecalculation(id, "milestone");
            }
        }
        catch (Exception ex)
        {
            retval = "0";
        }
        // return dscode.GetXml();

        return retval;
    }
    [WebMethod]
    public static string TaskUpdateDetails(string id, string col_name, string col_value)
    {
        string retval = "0";
        DataAccess da1 = new DataAccess();
        try
        {
            string strsql = " update Pms_tbl_AddTask set " + col_name + "='" + col_value + "'  where status=1 and archive=0 and id='" + id + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
            da1.executeDMLQueryinline(strsql);
            retval = "1";
            if (col_name.ToLower() == "weightage")
            {
                string chkval = managecalculation(id, "task");
            }
        }
        catch (Exception ex)
        {
            retval = "0";
        }
        // return dscode.GetXml();
        return retval;
    }
    [WebMethod]
    public static string SubTaskUpdateDetails(string id, string col_name, string col_value)
    {
        string retval = "0";
        DataAccess da1 = new DataAccess();
        try
        {
            string strsql = " update Pms_tbl_AddSubTask set " + col_name + "='" + col_value + "'  where status=1 and archive=0 and id='" + id + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
            da1.executeDMLQueryinline(strsql);
            retval = "1";
            if (col_name.ToLower() == "weightage")
            {
                string chkval = managecalculation(id, "subtask");
            }
        }
        catch (Exception ex)
        {
            retval = "0";
        }
        // return dscode.GetXml();
        return retval;
    }

    #endregion
    
    #region check dependency
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
                                SaveLogText("checkcomplete123-" + stsqlM);
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
    #endregion

    #region bindfiles
    
    public static List<projectfiles> bindfiledata(string pid, string id, string type)
    {

        DataAccess da = new DataAccess();
        string retval = "";
        
        
        string strsqlfils = "select * from Pms_tbl_projectfiles where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and status=1 and archive=0 and pid='" + pid + "' and fentryid='" + id + "' and ftype='" + type + "' order by id desc";
        SaveLogText("strsqlfils-" + strsqlfils);
        DataSet dsfils = da.GetDataSetInline(strsqlfils);
        string strdatafils = "";
        int countrow = 0;

        //"<thead>" +
        //"<tr><th></th><th>File</th><th style='width:30px'></th></tr>" +
        //"</thead>";
        List<projectfiles> newprojectfiles = new List<projectfiles>();
        if (dsfils.Tables[0].Rows.Count > 0)
        {            
            for (int i = 0; i < dsfils.Tables[0].Rows.Count; i++)
            {
                string filepath = "";
                string ftype = dsfils.Tables[0].Rows[i]["ftype"].ToString();
                string fentryid = dsfils.Tables[0].Rows[i]["fentryid"].ToString();
                if (type != string.Empty)
                {
                    if (type == "milestone")
                        filepath = "/pms/MileImages/" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "/" + fentryid + "/";
                    else if (type == "task")
                        filepath = "/pms/TaskImages/" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "/" + fentryid + "/";
                    else if (type == "subtask")
                        filepath = "/pms/SubtaskImages/" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "/" + fentryid + "/";

                }
                newprojectfiles.Add(new projectfiles
                {

                    id = dsfils.Tables[0].Rows[i]["id"].ToString(),
                    pid = dsfils.Tables[0].Rows[i]["pid"].ToString(),
                    fentryid = dsfils.Tables[0].Rows[i]["fentryid"].ToString(),
                    ftype = dsfils.Tables[0].Rows[i]["ftype"].ToString(),
                    fileurl = dsfils.Tables[0].Rows[i]["fileurl"].ToString(),
                    filedes = dsfils.Tables[0].Rows[i]["filedes"].ToString(),
                    fext = dsfils.Tables[0].Rows[i]["fext"].ToString(),
                    filepath = filepath,
                    

                });

            }
           
        }
        

        return newprojectfiles;
    }
    #endregion
    #region update timeline
    [WebMethod]
    public static DateTime? updatetimeline_mile(string id, string ndays, string startdate)
    {
        DateTime? retval = null;
        
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        startdate = cmf.dateConvert(startdate.ToString());
        //leave leaveweek = new leave();
        string empid = "0";
        string strsqltm = "";
        string sqldate = "select id,StartDate,EndDate as Targetdate,team,owner from Pms_tbl_AddMilestone where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  and id='" +id + "'";
        startdate = Convert.ToDateTime(startdate).ToString("yyyy-MM-dd");
        string edate = startdate;
        int newday = 0;
        
        int addon = 0;
        if (ndays != "")
        {
           // newday = Convert.ToInt32(ndays == "" ? "0" : ndays);
            if (!string.IsNullOrWhiteSpace(ndays))
            {
                newday = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(ndays == "" ? "0" : ndays)));
            }
            string checkdate = "";
            for (int i = 0; i < newday; i++)
            {
                addon = 0;
                if (checkdate == "")
                {
                    checkdate = Convert.ToDateTime(startdate).ToString();
                }
                else
                {
                    checkdate = Convert.ToDateTime(checkdate).AddDays(1).ToString();
                }

                if (getweeklyoff(Convert.ToDateTime(checkdate), empid))
                {
                    addon = 1;
                }
                else if (getholiday(Convert.ToDateTime(checkdate), empid))
                {
                    addon = 1;
                }
                if (addon == 1)
                {
                    newday += 1;
                }
            }
            edate = Convert.ToDateTime(startdate).AddDays(newday-1).ToString("yyyy-MM-dd");
            SaveLogText("edate-" + startdate + "/"+ edate);
            strsqltm = "update Pms_tbl_AddMilestone set startdate=cast('" + startdate + "' as date),EndDate=cast('" + edate + "' as date),txtdays='" + (ndays == "" ? "0" : ndays) + "'  where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  and  id='" + id + "' ";
            
            if (strsqltm != string.Empty)
            {
                try
                {
                    da.executeDMLQueryinline(strsqltm);                   
                   
                }
                catch(Exception ex)
                {
                    SaveLogText("strsqltmex-" + ex.Message);
                }
                try
                {
                    DataSet dsD = da.GetDataSetInline(sqldate);
                    if (dsD.Tables[0].Rows.Count > 0)
                    {
                        retval = Convert.ToDateTime(dsD.Tables[0].Rows[0]["Targetdate"]);
                    }
                }
                catch
                {

                }
            }
        }



        SaveLogText("retvalA="+ retval.ToString());
        return retval;
    }
    [WebMethod]
    public static DateTime? updatetimeline_task(string id, string ndays, string startdate)
    {
        DateTime? retval = null;

        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        startdate = cmf.dateConvert(startdate.ToString());
        //leave leaveweek = new leave();
        string empid = "0";
        string strsqltm = "";
        string sqldate = "select id,StartDate,Targetdate,team,owner from Pms_tbl_Addtask where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  and id='" + id + "'";
        startdate = Convert.ToDateTime(startdate).ToString("yyyy-MM-dd");
        string edate = startdate;
        int newday = 0;

        int addon = 0;
        if (ndays != "")
        {
            newday = Convert.ToInt32(ndays == "" ? "0" : ndays);
            string checkdate = "";
            for (int i = 0; i < newday; i++)
            {
                addon = 0;
                if (checkdate == "")
                {
                    checkdate = Convert.ToDateTime(startdate).ToString();
                }
                else
                {
                    checkdate = Convert.ToDateTime(checkdate).AddDays(1).ToString();
                }

                if (getweeklyoff(Convert.ToDateTime(checkdate), empid))
                {
                    addon = 1;
                }
                else if (getholiday(Convert.ToDateTime(checkdate), empid))
                {
                    addon = 1;
                }
                if (addon == 1)
                {
                    newday += 1;
                }
            }
            edate = Convert.ToDateTime(startdate).AddDays(newday - 1).ToString("yyyy-MM-dd");
            SaveLogText("edate-" + startdate + "/" + edate);
            strsqltm = "update Pms_tbl_Addtask set startdate=cast('" + startdate + "' as date),targetdate=cast('" + edate + "' as date),txtdays='" + (ndays == "" ? "0" : ndays) + "'  where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  and  id='" + id + "' ";
            SaveLogText("strsqltm-" + strsqltm);
            if (strsqltm != string.Empty)
            {
                try
                {
                    da.executeDMLQueryinline(strsqltm);

                }
                catch (Exception ex)
                {
                    SaveLogText("strsqltmex-" + ex.Message);
                }
                try
                {
                    DataSet dsD = da.GetDataSetInline(sqldate);
                    if (dsD.Tables[0].Rows.Count > 0)
                    {
                        retval = Convert.ToDateTime(dsD.Tables[0].Rows[0]["Targetdate"]);
                    }
                }
                catch
                {

                }
            }
        }



        SaveLogText("retvalA=" + retval.ToString());
        return retval;
    }
    [WebMethod]
    public static DateTime? updatetimeline_subtask(string id, string ndays, string startdate)
    {
        DateTime? retval = null;

        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        startdate = cmf.dateConvert(startdate.ToString());
        //leave leaveweek = new leave();
        string empid = "0";
        string strsqltm = "";
        string sqldate = "select id,StartDate,Targetdate,team,owner from Pms_tbl_Addsubtask where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  and id='" + id + "'";
        startdate = Convert.ToDateTime(startdate).ToString("yyyy-MM-dd");
        string edate = startdate;
        int newday = 0;

        int addon = 0;
        if (ndays != "")
        {
            newday = Convert.ToInt32(ndays == "" ? "0" : ndays);
            string checkdate = "";
            for (int i = 0; i < newday; i++)
            {
                addon = 0;
                if (checkdate == "")
                {
                    checkdate = Convert.ToDateTime(startdate).ToString();
                }
                else
                {
                    checkdate = Convert.ToDateTime(checkdate).AddDays(1).ToString();
                }

                if (getweeklyoff(Convert.ToDateTime(checkdate), empid))
                {
                    addon = 1;
                }
                else if (getholiday(Convert.ToDateTime(checkdate), empid))
                {
                    addon = 1;
                }
                if (addon == 1)
                {
                    newday += 1;
                }
            }
            edate = Convert.ToDateTime(startdate).AddDays(newday - 1).ToString("yyyy-MM-dd");
            SaveLogText("edate-" + startdate + "/" + edate);
            strsqltm = "update Pms_tbl_Addsubtask set startdate=cast('" + startdate + "' as date),targetdate=cast('" + edate + "' as date),txtdays='" + (ndays == "" ? "0" : ndays) + "'  where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  and  id='" + id + "' ";
            SaveLogText("strsqltm-" + strsqltm);
            if (strsqltm != string.Empty)
            {
                try
                {
                    da.executeDMLQueryinline(strsqltm);

                }
                catch (Exception ex)
                {
                    SaveLogText("strsqltmex-" + ex.Message);
                }
                try
                {
                    DataSet dsD = da.GetDataSetInline(sqldate);
                    if (dsD.Tables[0].Rows.Count > 0)
                    {
                        retval = Convert.ToDateTime(dsD.Tables[0].Rows[0]["Targetdate"]);
                    }
                }
                catch
                {

                }
            }
        }



        SaveLogText("retvalA=" + retval.ToString());
        return retval;
    }

    [WebMethod]
    public static string getleftdays(string id,string gtype)
    {
        string retval = "";

        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        string strsql = "";
        if (gtype == "subtask")
        {
            strsql = "select id,Startdate,Targetdate,team,owner from Pms_tbl_Addsubtask where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + id + "'";
        }
        else if (gtype == "task")
        {
            strsql = "select id,Startdate,Targetdate,team,owner from pms_tbl_addtask where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'   and id='" +id + "'";
        }
        else if (gtype=="milestone")
        {
            strsql = "select id,StartDate,EndDate as Targetdate,team,owner from Pms_tbl_AddMilestone where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  and id='" + id + "'";
        }
        string tleftdays = "";
        #region bind timeline
        //Timeline
        if (strsql != "")
        {
            DataSet dsD = da.GetDataSetInline(strsql);
            double miledayscomplite = 0;


            DateTime mileStartDate = Convert.ToDateTime(dsD.Tables[0].Rows[0]["StartDate"].ToString() == "" ? DateTime.Now.ToString() : dsD.Tables[0].Rows[0]["StartDate"].ToString());
            DateTime mileTargetdate = Convert.ToDateTime(dsD.Tables[0].Rows[0]["Targetdate"].ToString() == "" ? DateTime.Now.ToString() : dsD.Tables[0].Rows[0]["Targetdate"].ToString());
            TimeSpan mileobjTimeSpan = mileTargetdate - mileStartDate;
            double mileDays = Convert.ToDouble(mileobjTimeSpan.TotalDays);
            mileDays = mileDays + 1;


            DateTime mileEnddate = Convert.ToDateTime(dsD.Tables[0].Rows[0]["Targetdate"].ToString() == "" ? DateTime.Now.ToString() : dsD.Tables[0].Rows[0]["Targetdate"].ToString());
            DateTime mileCurrentdate = DateTime.Now;
            TimeSpan mileobjtimeleftspn = mileEnddate - mileCurrentdate;
            double mileleftDays = Convert.ToDouble(mileobjtimeleftspn.Days);
            

            string mileleftdlydys = "";
            if (mileleftDays < 0)
            {
                mileleftDays = mileleftDays + (-1);
                miledayscomplite = 100;
                mileleftdlydys = "days delayed";
            }
            else
            {
                mileleftDays = mileleftDays +1;
                miledayscomplite = Math.Round(((mileDays - mileleftDays) * 100) / mileDays, 2);
                mileleftdlydys = "days left";
            }
            //start days months  
            string milestartmonthname = "";
            int mileststartdys = mileStartDate.Day;
            int milestrtmonth = mileStartDate.Month;
            int milestonestartyear = mileStartDate.Year;
            //end days months 
            string mileendmonthname = "";
            int mileenddys = mileTargetdate.Day;
            int mileendmonth = mileTargetdate.Month;
            int milestoneEndyear = mileTargetdate.Year;
            if (mileleftdlydys != "")
            {
                tleftdays = mileleftDays.ToString().Replace("-", "") + " " + mileleftdlydys;
            }
            retval = tleftdays;
        }

        #endregion
        return retval;
    }
    public static string getleftdays_load(string id,string gtype)
    {
        string retval = "";

        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        string strsql = "";
        try
        {
            if (gtype == "subtask")
            {
                strsql = "select id,Startdate,Targetdate,team,owner from Pms_tbl_Addsubtask where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + id + "'";
            }
            else if (gtype == "task")
            {
                strsql = "select id,Startdate,Targetdate,team,owner from pms_tbl_addtask where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'   and id='" + id + "'";
            }
            else if (gtype == "milestone")
            {
                strsql = "select id,StartDate,EndDate as Targetdate,team,owner from Pms_tbl_AddMilestone where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  and id='" + id + "'";
            }
            string tleftdays = "";
            #region bind timeline
            //Timeline
            if (strsql != "")
            {
                DataSet dsD = da.GetDataSetInline(strsql);
                double miledayscomplite = 0;


                DateTime mileStartDate = Convert.ToDateTime(dsD.Tables[0].Rows[0]["StartDate"].ToString() == "" ? DateTime.Now.ToString() : dsD.Tables[0].Rows[0]["StartDate"].ToString());
                DateTime mileTargetdate = Convert.ToDateTime(dsD.Tables[0].Rows[0]["Targetdate"].ToString() == "" ? DateTime.Now.ToString() : dsD.Tables[0].Rows[0]["Targetdate"].ToString());
                TimeSpan mileobjTimeSpan = mileTargetdate - mileStartDate;
                double mileDays = Convert.ToDouble(mileobjTimeSpan.TotalDays);
                mileDays = mileDays + 1;


                DateTime mileEnddate = Convert.ToDateTime(dsD.Tables[0].Rows[0]["Targetdate"].ToString() == "" ? DateTime.Now.ToString() : dsD.Tables[0].Rows[0]["Targetdate"].ToString());
                DateTime mileCurrentdate = DateTime.Now;
                TimeSpan mileobjtimeleftspn = mileEnddate - mileCurrentdate;
                double mileleftDays = Convert.ToDouble(mileobjtimeleftspn.Days);


                string mileleftdlydys = "";
                if (mileleftDays < 0)
                {
                    mileleftDays = mileleftDays + (-1);
                    miledayscomplite = 100;
                    mileleftdlydys = "days delayed";
                }
                else
                {
                    mileleftDays = mileleftDays + 1;
                    miledayscomplite = Math.Round(((mileDays - mileleftDays) * 100) / mileDays, 2);
                    mileleftdlydys = "days left";
                }
                //start days months  
                string milestartmonthname = "";
                int mileststartdys = mileStartDate.Day;
                int milestrtmonth = mileStartDate.Month;
                int milestonestartyear = mileStartDate.Year;
                //end days months 
                string mileendmonthname = "";
                int mileenddys = mileTargetdate.Day;
                int mileendmonth = mileTargetdate.Month;
                int milestoneEndyear = mileTargetdate.Year;
                if (mileleftdlydys != "")
                {
                    tleftdays = mileleftDays.ToString().Replace("-", "") + " " + mileleftdlydys;
                }
                retval = tleftdays;
            }

            #endregion
        }
        catch { }
        return retval;
    }
    [WebMethod]
    public static DateTime? manage_enddate(string ndays, string startdate)
    {
        DateTime? retval = null;

        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        startdate = cmf.dateConvert(startdate.ToString());
        //leave leaveweek = new leave();
        string empid = "0";
        string strsqltm = "";
        startdate = Convert.ToDateTime(startdate).ToString("yyyy-MM-dd");
        string edate = startdate;
        int newday = 0;

        int addon = 0;
        if (ndays !=string.Empty&& ndays !="0")
        {
            //newday = Convert.ToInt32(ndays == "" ? "0" : ndays);
            if (!string.IsNullOrWhiteSpace(ndays))
            {
                newday = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(ndays == "" ? "0" : ndays)));
            }
            string checkdate = "";
            for (int i = 0; i < newday; i++)
            {
                addon = 0;
                if (checkdate == "")
                {
                    checkdate = Convert.ToDateTime(startdate).ToString();
                }
                else
                {
                    checkdate = Convert.ToDateTime(checkdate).AddDays(1).ToString();
                }

                if (getweeklyoff(Convert.ToDateTime(checkdate), empid))
                {
                    addon = 1;
                }
                else if (getholiday(Convert.ToDateTime(checkdate), empid))
                {
                    addon = 1;
                }
                if (addon == 1)
                {
                    newday += 1;
                }
            }
            retval = Convert.ToDateTime(startdate).AddDays(newday - 1);
           
        }
        else
        {
            retval = Convert.ToDateTime(startdate);
        }
        
        return retval;
    }

    #endregion
    #region update owner details

    
    [WebMethod]
    public static string UpdateMilestone_Owner(string id = "", string ownerids = "")
    {
        string retval = "0";
        try
        {
            DataAccess da1 = new DataAccess();
            string finsql4 = "update Pms_tbl_AddMilestone set Owner='" + ownerids + "' where id='" + id + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da1.GetDataSetInline(finsql4);
            retval = "1";
        }
        catch { }
        return retval;
    }
    [WebMethod]
    public static string UpdateTask_Owner(string id = "", string ownerids = "")
    {
        string retval = "0";
        try
        {
            DataAccess da1 = new DataAccess();
            string finsql4 = "update Pms_tbl_Addtask set Owner='" + ownerids + "' where id='" + id + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da1.GetDataSetInline(finsql4);
            retval = "1";
        }
        catch { }
        return retval;
    }
    [WebMethod]
    public static string UpdateSubtask_Owner(string id = "", string ownerids = "")
    {
        string retval = "0";
        try
        {
            DataAccess da1 = new DataAccess();
            string finsql4 = "update Pms_tbl_Addsubtask set Owner='" + ownerids + "' where id='" +id + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da1.GetDataSetInline(finsql4);
            retval = "1";
        }
        catch { }
        return retval;
    }



    [WebMethod]
    public static string removeownerbyid(projectowneres projectowneres)
    {
        string retval = "0";
        DataAccess da = new DataAccess();
        if (projectowneres.id != null)
        {

            try
            {
                string strsqlown = "select id,Owner from Pms_tbl_AddMilestone where status=1 and archive=0 and regid='"
                    + HttpContext.Current.Request.Cookies["cadmin"]["id"] +
                    "' and id='" + projectowneres.id + "' ";
                if (projectowneres.Ownertype == "task")
                {
                    strsqlown = "select id,Owner from Pms_tbl_AddTask where status=1 and archive=0 and regid='"
                                    + HttpContext.Current.Request.Cookies["cadmin"]["id"] +
                                    "' and id='" + projectowneres.id + "' ";
                }
                else if (projectowneres.Ownertype == "subtask")
                {
                    strsqlown = "select id,Owner from Pms_tbl_AddsubTask where status=1 and archive=0 and regid='"
                                    + HttpContext.Current.Request.Cookies["cadmin"]["id"] +
                                    "' and id='" + projectowneres.id + "' ";
                }
                DataSet dsown = da.GetDataSetInline(strsqlown);

                if (dsown.Tables[0].Rows.Count > 0)
                {
                    string ownerStr = dsown.Tables[0].Rows[0]["Owner"].ToString();
                    SaveLogText("updateQuery1-" + ownerStr);
                    if (!string.IsNullOrEmpty(ownerStr))
                    {
                        // Split
                        List<string> owners = ownerStr.Split(',').ToList();

                        // Remove matching ID
                        owners.Remove(projectowneres.Owner.ToString());

                        // Join back
                        string updatedOwners = string.Join(",", owners);

                        // Update DB
                        string updateQuery = "update Pms_tbl_AddMilestone set Owner='" + updatedOwners +
                                             "' where id='" + projectowneres.id + "'";
                        if (projectowneres.Ownertype == "task")
                        {
                            updateQuery = "update Pms_tbl_AddTask set Owner='" + updatedOwners +
                                             "' where id='" + projectowneres.id + "'";
                        }
                        else if (projectowneres.Ownertype == "subtask")
                        {
                            updateQuery = "update Pms_tbl_Addsubtask set Owner='" + updatedOwners +
                                              "' where id='" + projectowneres.id + "'";
                        }
                        SaveLogText("updateQuery2-" + updateQuery);
                        da.executeDMLQueryinline(updateQuery);
                        retval = "1";

                    }
                }
            }
            catch { }

        }

        
        return retval;
    }


    #endregion
    #region update team details
    [WebMethod]
    public static string UpdateMilestone_Team(string id = "", string ownerids = "")
    {
        string retval = "0";
        try
        {
            DataAccess da1 = new DataAccess();
            string finsql4 = "update Pms_tbl_AddMilestone set Team='" + ownerids + "' where id='" + id + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da1.GetDataSetInline(finsql4);
            retval = "1";
        }
        catch { }
        return retval;
    }
    [WebMethod]
    public static string UpdateTask_Team(string id = "", string ownerids = "")
    {
        string retval = "0";
        try
        {
            DataAccess da1 = new DataAccess();
            string finsql4 = "update Pms_tbl_Addtask set Team='" + ownerids + "' where id='" + id + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da1.GetDataSetInline(finsql4);
            retval = "1";
        }
        catch { }
        return retval;
    }
    [WebMethod]
    public static string UpdateSubtask_Team(string id = "", string ownerids = "")
    {
        string retval = "0";
        try
        {
            DataAccess da1 = new DataAccess();
            string finsql4 = "update Pms_tbl_Addsubtask set Team='" + ownerids + "' where id='" + id + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da1.GetDataSetInline(finsql4);
            retval = "1";
        }
        catch { }
        return retval;
    }

    [WebMethod]
    public static string removeteambyid(projectteams projectteams)
    {
        string retval = "";
        DataAccess da = new DataAccess();
        if (projectteams.id != null)
        {

            try
            {
                string strsqlown = "select id,team from Pms_tbl_AddMilestone where status=1 and archive=0 and regid='"
                + HttpContext.Current.Request.Cookies["cadmin"]["id"] +
                "' and id='" + projectteams.id + "' ";
                if (projectteams.Teamtype == "task")
                {
                    strsqlown = "select id,team from Pms_tbl_AddTask where status=1 and archive=0 and regid='"
                                    + HttpContext.Current.Request.Cookies["cadmin"]["id"] +
                                    "' and id='" + projectteams.id + "' ";
                }
                else if (projectteams.Teamtype == "subtask")
                {
                    strsqlown = "select id,team from Pms_tbl_AddsubTask where status=1 and archive=0 and regid='"
                                    + HttpContext.Current.Request.Cookies["cadmin"]["id"] +
                                    "' and id='" + projectteams.id + "' ";
                }
                DataSet dsown = da.GetDataSetInline(strsqlown);

                if (dsown.Tables[0].Rows.Count > 0)
                {
                    string teamStr = dsown.Tables[0].Rows[0]["team"].ToString();
                    SaveLogText("updateQuery1-" + teamStr);
                    if (!string.IsNullOrEmpty(teamStr))
                    {
                        // Split
                        List<string> teams = teamStr.Split(',').ToList();

                        // Remove matching ID
                        teams.Remove(projectteams.Team.ToString());

                        // Join back
                        string updatedteams = string.Join(",", teams);

                        // Update DB
                        string updateQuery = "update Pms_tbl_AddMilestone set team='" + updatedteams +
                                             "' where id='" + projectteams.id + "'";
                        if (projectteams.Teamtype == "task")
                        {
                            updateQuery = "update Pms_tbl_AddTask set team='" + updatedteams +
                                             "' where id='" + projectteams.id + "'";
                        }
                        else if (projectteams.Teamtype == "subtask")
                        {
                            updateQuery = "update Pms_tbl_Addsubtask set team='" + updatedteams +
                                              "' where id='" + projectteams.id + "'";
                        }
                        SaveLogText("updateQuery2-" + updateQuery);
                        da.executeDMLQueryinline(updateQuery);
                        retval = "1";

                    }
                }
            }
            catch { }
        }


        return retval;
    }
    #endregion
    [WebMethod]
    public static newmilestone binditemdetails_mile(string id, string type)
    {
        DataAccess da = new DataAccess();
        newmilestone obj = new newmilestone();
        if (string.IsNullOrEmpty(id))
            return obj;

        var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
        if (regCookie == null)
            return obj;
        string str2 = "select * from Pms_tbl_AddMilestone where id='" + id + "' and status=1 and archive=0";
        DataAccess danew = new DataAccess();
        DataSet drmile = danew.GetDataSetInline(str2);

        foreach (DataRow row in drmile.Tables[0].Rows)
        {


            int totaltasks = 0;
            List<ownerlist> newownerlist = getemployeelist<ownerlist>(row["Owner"].ToString());
            List<teamlist> newteamlist = getemployeelist<teamlist>(row["team"].ToString());
            List<projectfiles> newfileslist = bindfiledata(row["projectid"].ToString(), row["id"].ToString(), "milestone");
            List<newtask> newtasklist = bindtasklist_bymileid(row["id"].ToString());
            List<issuestdata> newissueslist = bindissueslist(row["id"].ToString(), "milestone");
            totaltasks = checktaskby_milestoneid(row["id"].ToString());
            int totaldependencys = 0;
            totaldependencys = Binddependencydata(row["id"].ToString(), "milestone");
            double mileTaskTotal = 0;
            mileTaskTotal = get_completionstatus(row["id"].ToString(), "milestone");
            //SaveLogText("chkadmintodayEX-" + mileTaskTotal);
            double mileWattagecomplite = mileTaskTotal;
            double mileWattagepending = 100;
            if (mileTaskTotal > 0)
            {
                mileWattagepending = mileWattagepending - mileTaskTotal;
            }
            string tleftdays = getleftdays_load(row["id"].ToString(), "milestone");
            #region find issues
            int totalissues = 0;
            int completeissues = 0;
            int pendingissues = 0;
            double completeissue_rates = 0;
            double pendingissue_rates = 0;
            try
            {

                totalissues = get_totalissue(row["id"].ToString(), "milestone");
                completeissues = get_completeissue(row["id"].ToString(), "milestone");
                if (completeissues > 0)
                {
                    pendingissues = totalissues - completeissues;
                    completeissue_rates = Math.Round((Convert.ToDouble(completeissues) / totalissues) * 100, 2);
                    pendingissue_rates = 100 - completeissue_rates;
                }
                else
                {
                    pendingissues = totalissues;
                    completeissue_rates = 0;
                    pendingissue_rates = 100;
                }

            }
            catch
            {

            }
            #endregion


            obj.id = row["id"] as int?;
            obj.ProjectId = row["ProjectId"] as int?;
            obj.Milestone = row["Milestone"].ToString();
            obj.Weightage = row["Weightage"] as double?;
            //StartDate =Convert.ToDateTime(row["StartDate"] as DateTime?),
            obj.StartDate = row["StartDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["StartDate"]) : null;
            obj.EndDate = row["EndDate"] as DateTime?;
            obj.Owner = row["Owner"].ToString();
            obj.Access = row["Access"] as int?;
            obj.team = row["team"].ToString();
            obj.P_Status = row["P_Status"] as int?;
            obj.status_name = get_Projectstatus(row["P_Status"].ToString());
            obj.status_color = get_Status_Color(row["P_Status"].ToString());
            obj.Priority = row["Priority"] as int?;
            obj.priority_name = get_PriorityName(row["Priority"].ToString());
            obj.priority_color = get_PriorityColor(row["Priority"].ToString());
            obj.Budget = row["Budget"] as double?;
            obj.Actual = row["Actual"] as double?;
            obj.uid = row["uid"] as int?;
            obj.cdate = row["cdate"] as DateTime?;
            obj.istask = row["istask"] as int?;
            obj.checkmileWeightage = row["checkmileWeightage"] as int?;
            obj.iscomplete = row["iscomplete"] as int?;
            obj.iscompletedate = row["iscompletedate"] as DateTime?;
            obj.iscompletepercentage = row["iscompletepercentage"] as int?;
            obj.Durationtype = row["Durationtype"] as int?;
            obj.TaskDuration = row["TaskDuration"].ToString();
            obj.Durationhourcal = row["Durationhourcal"] as double?;
            obj.orderno = row["orderno"] as int?;
            obj.miledescription = row["miledescription"].ToString();
            obj.Uploaddrawing = row["Uploaddrawing"].ToString();
            obj.txtdays = row["txtdays"].ToString();
            obj.complete_value = Math.Round(mileTaskTotal, 2).ToString();
            obj.pending_value = Math.Round(mileWattagepending, 2).ToString();
            obj.taskcount = totaltasks;
            obj.isExpanded = false;
            obj.leftdays = tleftdays;
            obj.totalissue = totalissues;
            obj.completeissue = completeissues;
            obj.pendingissue = pendingissues;
            obj.completeissue_rate = completeissue_rates;
            obj.pendingissue_rate = pendingissue_rates;
            obj.totaldependency = totaldependencys;
            obj.owners = newownerlist;
            obj.teams = newteamlist;
            obj.viewfiles = newfileslist;
            obj.tasks = newtasklist;

        }


        return obj;
    }
    [WebMethod]
    public static newtask binditemdetails_task(string id, string type)
    {
        DataAccess da = new DataAccess();
        newtask obj = new newtask();
        try
        {
            if (string.IsNullOrEmpty(id))
                return obj;

            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null)
                return obj;

            string regid = regCookie["id"];


            try
            {
                string str2 = "select * from Pms_tbl_Addtask where regid='" + regid + "' and id='" + id + "' and status=1 and archive=0 order by isnull(orderno,0)";

                SaveLogText("gettasklist-" + str2);
                DataAccess danew = new DataAccess();
                DataSet drtask = danew.GetDataSetInline(str2);

                foreach (DataRow row in drtask.Tables[0].Rows)
                {
                    List<ownerlist> newownerlist = getemployeelist<ownerlist>(row["Owner"].ToString());
                    List<teamlist> newteamlist = getemployeelist<teamlist>(row["team"].ToString());
                    List<projectfiles> newfileslist = bindfiledata(row["ProjectId"].ToString(), row["id"].ToString(), "task");
                    List<newsubtask> newsubtasklist = bindsubtasklist_bytaskid(row["id"].ToString());
                    SaveLogText("getsubtasklistcount-" + newsubtasklist.Count);
                    double mileTaskTotal = 0;
                    mileTaskTotal = get_completionstatus(row["id"].ToString(), "task");
                    int totaldependencys = 0;
                    totaldependencys = Binddependencydata(row["id"].ToString(), "task");
                    double mileWattagecomplite = mileTaskTotal;
                    double mileWattagepending = 100;
                    if (mileTaskTotal > 0)
                    {
                        mileWattagepending = mileWattagepending - mileTaskTotal;
                    }
                    string tleftdays = getleftdays_load(row["id"].ToString(), "task");
                    #region find issues
                    int totalissues = 0;
                    int completeissues = 0;
                    int pendingissues = 0;
                    double completeissue_rates = 0;
                    double pendingissue_rates = 0;
                    try
                    {

                        totalissues = get_totalissue(row["id"].ToString(), "task");
                        completeissues = get_completeissue(row["id"].ToString(), "task");
                        if (completeissues > 0)
                        {
                            pendingissues = totalissues - completeissues;
                            completeissue_rates = Math.Round((Convert.ToDouble(completeissues) / totalissues) * 100, 2);
                            pendingissue_rates = 100 - completeissue_rates;
                        }
                        else
                        {
                            pendingissues = totalissues;
                            completeissue_rates = 0;
                            pendingissue_rates = 100;
                        }

                    }
                    catch
                    {

                    }
                    #endregion


                    obj.id = row["id"] as int?;
                    obj.ProjectId = row["ProjectId"] as int?;
                    obj.Milestoneid = row["Milestoneid"] as int?;
                    obj.taskname = row["taskname"].ToString();
                    obj.Weightage = row["Weightage"] as double?;
                    obj.Startdate = row["StartDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["StartDate"]) : null;
                    obj.Targetdate = row["Targetdate"] as DateTime?;
                    obj.owner = row["Owner"].ToString();
                    obj.team = row["team"].ToString();
                    obj.P_Status = row["P_Status"] as int?;
                    obj.status_name = get_Projectstatus(row["P_Status"].ToString());
                    obj.status_color = get_Status_Color(row["P_Status"].ToString());
                    obj.Priority = row["Priority"] as int?;
                    obj.priority_name = get_PriorityName(row["Priority"].ToString());
                    obj.priority_color = get_PriorityColor(row["Priority"].ToString());
                    obj.Budget = row["Budget"] as double?;
                    obj.Actual = row["Actual"] as double?;
                    obj.uid = row["uid"] as int?;
                    obj.cdate = row["cdate"] as DateTime?;
                    obj.issubtask = row["issubtask"] as int?;
                    obj.iscompletepercentage = row["iscompletepercentage"] as double?;
                    obj.TaskDuration = row["TaskDuration"].ToString();
                    obj.orderno = row["orderno"] as int?;
                    obj.taskdescripton = row["taskdescripton"].ToString();
                    obj.txtdays = row["txtdays"].ToString();
                    obj.complete_value = Math.Round(mileTaskTotal, 2).ToString();
                    obj.pending_value = Math.Round(mileWattagepending, 2).ToString();
                    obj.isExpanded = false;
                    obj.leftdays = tleftdays;
                    obj.totalissue = totalissues;
                    obj.completeissue = completeissues;
                    obj.pendingissue = pendingissues;
                    obj.completeissue_rate = completeissue_rates;
                    obj.pendingissue_rate = pendingissue_rates;
                    obj.totaldependency = totaldependencys;
                    obj.owners = newownerlist;
                    obj.teams = newteamlist;
                    obj.viewfiles = newfileslist;
                    obj.subtasks = newsubtasklist;

                }


                // projectlist.Add(obj);
            }
            catch { }




        }
        catch (Exception ex)
        {
            SaveLogText("chkadmintaskEX-" + ex.Message);
            return new newtask();
        }


        return obj;
    }
    [WebMethod]
    public static newsubtask binditemdetails_subtask(string id, string type)
    {
        DataAccess da = new DataAccess();
        newsubtask obj = new newsubtask();
        try
        {
            if (string.IsNullOrEmpty(id))
                return obj;

            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null)
                return obj;
            string regid = regCookie["id"];
            string str2 = "select * from Pms_tbl_Addsubtask where regid='" + regid + "' and id='" + id + "' and status=1 and archive=0 order by isnull(orderno,0)";


            DataAccess danew = new DataAccess();
            DataSet drsubtask = danew.GetDataSetInline(str2);
            foreach (DataRow row in drsubtask.Tables[0].Rows)
            {
                SaveLogText("getsubtaskobjlist-" + row["id"]);
                List<ownerlist> newownerlist = getemployeelist<ownerlist>(row["Owner"].ToString());
                List<teamlist> newteamlist = getemployeelist<teamlist>(row["team"].ToString());
                List<projectfiles> newfileslist = bindfiledata(row["ProjectId"].ToString(), row["id"].ToString(), "subtask");
                double mileTaskTotal = 0;
                mileTaskTotal = get_completionstatus(row["id"].ToString(), "subtask");
                int totaldependencys = 0;
                totaldependencys = Binddependencydata(row["id"].ToString(), "subtask");
                //SaveLogText("chkadmintodayEX-" + mileTaskTotal);
                double mileWattagecomplite = mileTaskTotal;
                double mileWattagepending = 100;
                if (mileTaskTotal > 0)
                {
                    mileWattagepending = mileWattagepending - mileTaskTotal;
                }
                string tleftdays = getleftdays_load(row["id"].ToString(), "subtask");
                #region find issues
                int totalissues = 0;
                int completeissues = 0;
                int pendingissues = 0;
                double completeissue_rates = 0;
                double pendingissue_rates = 0;
                try
                {

                    totalissues = get_totalissue(row["id"].ToString(), "subtask");
                    completeissues = get_completeissue(row["id"].ToString(), "subtask");
                    if (completeissues > 0)
                    {
                        pendingissues = totalissues - completeissues;
                        completeissue_rates = Math.Round((Convert.ToDouble(completeissues) / totalissues) * 100, 2);
                        pendingissue_rates = 100 - completeissue_rates;
                    }
                    else
                    {
                        pendingissues = totalissues;
                        completeissue_rates = 0;
                        pendingissue_rates = 100;
                    }

                }
                catch
                {

                }
                #endregion


                obj.id = row["id"] as int?;
                obj.ProjectId = row["ProjectId"] as int?;
                obj.Milestoneid = row["Milestoneid"] as int?;
                obj.Task = row["task"] as int?;
                obj.Subtaskname = row["Subtaskname"].ToString();
                obj.Weightage = row["Weightage"] as double?;
                obj.Startdate = row["StartDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["StartDate"]) : null;
                obj.Targetdate = row["Targetdate"] as DateTime?;
                obj.owner = row["Owner"].ToString();
                obj.team = row["team"].ToString();
                obj.P_Status = row["P_Status"] as int?;
                obj.status_name = get_Projectstatus(row["P_Status"].ToString());
                obj.status_color = get_Status_Color(row["P_Status"].ToString());
                obj.Priority = row["Priority"] as int?;
                obj.priority_name = get_PriorityName(row["Priority"].ToString());
                obj.priority_color = get_PriorityColor(row["Priority"].ToString());
                obj.Budget = row["Budget"] as double?;
                obj.Actual = row["Actual"] as double?;
                obj.uid = row["uid"] as int?;
                obj.cdate = row["cdate"] as DateTime?;
                obj.iscompletepercentage = row["iscompletepercentage"] as double?;
                obj.TaskDuration = row["TaskDuration"].ToString();
                obj.orderno = row["orderno"] as int?;
                obj.SubTaskDescription = row["SubTaskDescription"].ToString();
                obj.txtdays = row["txtdays"].ToString();
                obj.complete_value = Math.Round(mileTaskTotal, 2).ToString();
                obj.pending_value = Math.Round(mileWattagepending, 2).ToString();
                obj.leftdays = tleftdays;
                obj.totalissue = totalissues;
                obj.completeissue = completeissues;
                obj.pendingissue = pendingissues;
                obj.completeissue_rate = completeissue_rates;
                obj.pendingissue_rate = pendingissue_rates;
                obj.totaldependency = totaldependencys;
                obj.owners = newownerlist;
                obj.teams = newteamlist;
                obj.viewfiles = newfileslist;




            }

        }
        catch (Exception ex)
        {
            return new newsubtask();
        }
        return obj;
    }

    #endregion
    #region mange issues
    [WebMethod]
    public static string saveIssue(issuestdata issuesdetails)
    {
        string retval = "0";
        DataAccess da1 = new DataAccess();
        string editid = issuesdetails.id.ToString();
        string entrytype = "Insert";
        if (editid != "" && editid != "0")
        {
            entrytype = "Update";
        }

        if (issuesdetails.subtaskid > 0)
        {
            issuesdetails.Issueon = "subtask";
        }
        else if (issuesdetails.taskid > 0)
        {
            issuesdetails.Issueon = "task";
        }
        else if (issuesdetails.Milestoneid > 0)
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

        if (issuesdetails.StartDate != null)
        {
            prm[7] = new SqlParameter("@StartDate", DBNull.Value);
        }
        else
        {
            prm[7] = new SqlParameter("@StartDate", issuesdetails.StartDate);
        }
        if (issuesdetails.EndDate != null)
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
        if (issuesdetails.ReminderType == string.Empty)
        {
            prm[14] = new SqlParameter("@ReminderType", DBNull.Value);
        }
        else
        {
            prm[14] = new SqlParameter("@ReminderType", issuesdetails.ReminderType);
        }
        if (issuesdetails.Days == string.Empty)
        {
            prm[15] = new SqlParameter("@Days", DBNull.Value);
        }
        else
        {
            prm[15] = new SqlParameter("@Days", issuesdetails.Days);
        }
        if (issuesdetails.Time == string.Empty)
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

        if (issuesdetails.ChooseDate == string.Empty)
        {
            prm[18] = new SqlParameter("@ChooseDate", DBNull.Value);
        }
        else
        {
            prm[18] = new SqlParameter("@ChooseDate", issuesdetails.ChooseDate);
        }
        if (issuesdetails.AssignTo == string.Empty)
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
    public static List<issuestdata> bindissueslist(string id, string issuetype)
    {
        DataAccess da = new DataAccess();
        List<issuestdata> obj = new List<issuestdata>();

        try
        {
            if (string.IsNullOrEmpty(id))
                id = "0";

            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null)
                return obj;

            string regid = regCookie["id"];


            try
            {
                string milestr111 = "";

                if (issuetype == "milestone")
                {
                    milestr111 = "select COUNT(*) as countIssues from Pms_tbl_Addissues where  Milestoneid='" + id + "' and Issueon='milstone' and status=1 and archive=0 ";
                }
                else if (issuetype == "task")
                {
                    milestr111 = "select COUNT(*) as countIssues from Pms_tbl_Addissues where  taskid='" + id + "' and Issueon='task' and status=1 and archive=0 ";
                }
                else if (issuetype == "subtask")
                {
                    milestr111 = "select COUNT(*) as countIssues from Pms_tbl_Addissues where  subtaskid='" + id + "' and Issueon='subtask' and status=1 and archive=0 ";
                }
                if (id == "" || id == "0")
                {
                    return obj;
                }


                DataSet dsI = da.GetDataSetInline(milestr111);

                foreach (DataRow row in dsI.Tables[0].Rows)
                {

                    obj.Add(new issuestdata
                    {

                        id = row["id"] as int?,
                        ProjectId = row["ProjectId"] as int?,
                        Milestoneid = row["Milestoneid"] as int?,
                        tasklistid = row["tasklistid"].ToString(),
                        taskid = row["taskid"] as int?,
                        subtaskid = row["subtaskid"] as int?,
                        Issues = row["Issues"].ToString(),
                        StartDate = row["StartDate"] as DateTime?,
                        EndDate = row["EndDate"] as DateTime?,
                        Owner = row["Owner"].ToString(),
                        team = row["team"].ToString(),
                        Issueon = row["Issueon"].ToString(),
                        project_id = row["project_id"].ToString(),
                        Description = row["Description"].ToString(),
                        Reminder = row["Reminder"].ToString(),
                        ReminderType = row["ReminderType"].ToString(),
                        Days = row["Days"].ToString(),
                        Time = row["Time"].ToString(),
                        Notifyusers = row["Notifyusers"].ToString(),
                        ChooseDate = row["ChooseDate"].ToString(),
                        AssignTo = row["AssignTo"].ToString(),
                        IssueStatus = row["IssueStatus"] as int?,
                        Priority = row["Priority"] as int?,
                        IssueType = row["IssueType"].ToString(),
                        Replicability = row["Replicability"].ToString(),

                    });
                }



                // projectlist.Add(obj);
            }
            catch { }




        }
        catch (Exception ex)
        {
            SaveLogText("chkadmintaskEX-" + ex.Message);
            return new List<issuestdata>();
        }


        return obj;
    }
    [WebMethod]
    public static issuesdetails bindissues_details(string id, string issuetype)
    {
        DataAccess da = new DataAccess();
        issuesdetails obj = new issuesdetails();

        try
        {
            if (string.IsNullOrEmpty(id))
                return obj;

            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null)
                return obj;


            string regid = regCookie["id"];
            int totalissues = 0;
            int completeissues = 0;
            int pendingissues = 0;
            double completeissue_rates = 0;
            double pendingissue_rates = 100;
            #region find issues
            try
            {

                totalissues = get_totalissue(id, issuetype);
                completeissues = get_completeissue(id, issuetype);
                SaveLogText("Issuetxt=" + totalissues.ToString() + "," + completeissues.ToString());
                if (completeissues > 0)
                {
                    pendingissues = totalissues - completeissues;
                    completeissue_rates = Math.Round((Convert.ToDouble(completeissues) / totalissues) * 100, 2);
                    pendingissue_rates = 100 - completeissue_rates;
                }
                else
                {
                    pendingissues = totalissues;
                    completeissue_rates = 0;
                    pendingissue_rates = 100;
                }

            }
            catch
            {

            }
            #endregion
            obj.totalissue = totalissues;
            obj.completeissue = completeissues;
            obj.pendingissue = pendingissues;
            obj.completeissues_rate = completeissue_rates;
            obj.pendingissues_rate = pendingissue_rates;

        }
        catch (Exception ex)
        {
            SaveLogText("chkadmintaskEX-" + ex.Message);
            return new issuesdetails();
        }


        return obj;
    }

    protected static int get_totalissue(string id, string type)
    {
        int strret = 0;
        string milestr111 = "";
       
        if (type == "milestone")
        {
            milestr111 = "select COUNT(*) as countIssues from Pms_tbl_Addissues where  Milestoneid='" + id + "' and Issueon='milstone' and status=1 and archive=0 ";
        }
        else if (type == "task")
        {
            milestr111 = "select COUNT(*) as countIssues from Pms_tbl_Addissues where  taskid='" + id + "' and Issueon='task' and status=1 and archive=0 ";
        }
        else if (type == "subtask")
        {
            milestr111 = "select COUNT(*) as countIssues from Pms_tbl_Addissues where  subtaskid='" + id + "' and Issueon='subtask' and status=1 and archive=0 ";
        }
        if (id == "" || id == "0")
        {
            return 0;
        }
        DataAccess da = new DataAccess();
        
        DataSet miledss111 = da.GetDataSetInline(milestr111);
        if (miledss111.Tables[0].Rows.Count > 0)
        {

            try
            {
                strret = Convert.ToInt32(miledss111.Tables[0].Rows[0]["countIssues"]);
            }
            catch (Exception)
            {
            }
        }
       
        return strret;

    }
    protected static int get_completeissue(string id, string type)
    {
        int strret = 0;
        string milestr111 = "";

        if (type == "milestone")
        {
            
            milestr111 = "select COUNT(*) as countIssues from Pms_tbl_Addissues where Milestoneid='" + id + "' and Issueon='milstone' and IssueStatus in ( select id from Pms_tbl_Projectstatus where  status=1 and archive=0 and isnull(iscomplete,0)=1)  and status=1 and archive=0";
        }
        else if (type == "task")
        {
            milestr111 = "select COUNT(*) as countIssues from Pms_tbl_Addissues where taskid='" + id + "' and Issueon='task' and IssueStatus in ( select id from Pms_tbl_Projectstatus where  status=1 and archive=0 and isnull(iscomplete,0)=1)  and status=1 and archive=0";
        }
        else if (type == "subtask")
        {
           milestr111 = "select COUNT(*) as countIssues from Pms_tbl_Addissues where subtaskid='" + id + "' and Issueon='subtask' and IssueStatus in ( select id from Pms_tbl_Projectstatus where  status=1 and archive=0 and isnull(iscomplete,0)=1)  and status=1 and archive=0";
        }
        if (id == "" || id == "0")
        {
            return 0;
        }
        
        DataAccess da = new DataAccess();

        DataSet miledss111 = da.GetDataSetInline(milestr111);
        if (miledss111.Tables[0].Rows.Count > 0)
        {

            try
            {
                strret = Convert.ToInt32(miledss111.Tables[0].Rows[0]["countIssues"]);
            }
            catch (Exception ex)
            {
                SaveLogText("Issuesql=" + milestr111+","+ ex.Message);
            }
        }

        return strret;

    }
    #endregion
    #region manage dependency
    public static int Binddependencydata(string id,string utype)
    {
        int retval = 0;
        DataAccess da = new DataAccess();
        string strD = "";
        if(utype=="milestone")
        {
         strD = " and isnull(milestoneid,0)='" + id + "'";
        }
        else if (utype == "task")
        {
            strD = " and isnull(taskid,0)='" + id + "'";
        }
        else if (utype == "subtask")
        {
            strD = " and isnull(subtaskid,0)='" + id + "'";
        }
        string strsql = "select * from tbl_dependencyST where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        // SaveLogText("getdependencydetails-"+ strsql);
        if (strD != "")
        {
            strsql += strD;
            DataSet dsD = da.GetDataSetInline(strsql);
            if (dsD.Tables[0].Rows.Count > 0)
            {
                retval = getdependencydetails_count(dsD.Tables[0].Rows[0]["id"].ToString());
            }
        }
        return retval;
    }
    public static int getdependencydetails_count(string ids)
    {
        int retval = 0;
        DataAccess da = new DataAccess();
        string strdependency = "";
        
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
                        
                        if (dsD.Tables[0].Rows[i]["mid"].ToString() != "" && dsD.Tables[0].Rows[i]["mid"].ToString() != "0")
                        {
                            try
                            {
                                string stsqlM = "select isnull(statuscomplete,0) statuscomplete from Pms_tbl_AddMilestone where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + dsD.Tables[0].Rows[i]["mid"].ToString() + "'";
                                DataSet dschkM = da.GetDataSetInline(stsqlM);
                                if (dschkM.Tables[0].Rows[0]["statuscomplete"].ToString()!="1")
                                {
                                    chkallcomplete =1;
                                }
                                
                            }
                            catch { }
                        }
                       
                        if (dsD.Tables[0].Rows[i]["tid"].ToString() != "" && dsD.Tables[0].Rows[i]["tid"].ToString() != "0")
                        {
                            try
                            {
                                string stsqlM = "select isnull(statuscomplete,0) statuscomplete from Pms_tbl_AddTask where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + dsD.Tables[0].Rows[i]["tid"].ToString() + "'";
                                DataSet dschkM = da.GetDataSetInline(stsqlM);
                                if (dschkM.Tables[0].Rows[0]["statuscomplete"].ToString()!= "1")
                                {
                                    chkallcomplete = 1;
                                }
                                
                            }
                            catch { }
                        }
                       

                        if (dsD.Tables[0].Rows[i]["stid"].ToString() != "" && dsD.Tables[0].Rows[i]["stid"].ToString() != "0")
                        {
                            try
                            {
                                string stsqlM = "select isnull(statuscomplete,0) statuscomplete from Pms_tbl_AddSubtask where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + dsD.Tables[0].Rows[i]["stid"].ToString() + "'";

                                DataSet dschkM = da.GetDataSetInline(stsqlM);
                                if (dschkM.Tables[0].Rows[0]["statuscomplete"].ToString() != "1")
                                {
                                    chkallcomplete = 1;
                                }
                                

                            }
                            catch (Exception ex)
                            {
                                //SaveLogText("strbgc_s2-" + ex.Message);
                            }
                        }
                        if(chkallcomplete==1)
                        {
                            retval++;
                        }
                    }
                }

            }
            if (ids == "7")
            {
                SaveLogText("ddetails:" + retval);

            }


        }
        catch (Exception ex)
        {

        }
        return retval;
    }
    [WebMethod]
    public static string binddependency_details(string id, string type)
    {
        string retval = "0";
        DataAccess da = new DataAccess();
      

        try
        {
            if (string.IsNullOrEmpty(id))
                return retval;

            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            

            string regid = regCookie["id"];
            
            #region find dependency
            try
            {

                retval = Binddependencydata(id, type).ToString();

            }
            catch
            {

            }
            #endregion
           

        }
        catch (Exception ex)
        {
            SaveLogText("chkadmintaskEX-" + ex.Message);
            
        }


        return retval;
    }

    #endregion

   

    

    #region manage complete weightage
    public static string managecalculation(string id,string type)
    {
        function fns = new function();
        DataAccess da1 = new DataAccess();
        string retval = "1";
        string rid = id;
        string pid = "0";
        string mid = "0";
        string tid = "0";
        string sid = "0";
        
        string sqlP = "select id as mid,isnull(ProjectId,0) as pid,0 as tid,0 as sid,isnull(checkmileWeightage ,0) as checkmileWeightage from pms_tbl_addmilestone where status=1 and archive=0 and id=" + rid + "";
        if (type=="task")
        {
            sqlP = "select id as tid, isnull(Milestoneid,0) as mid,isnull(ProjectId,0) as pid,0 as sid from pms_tbl_addtask where status=1 and archive=0 and id=" + rid + "";
        }
        else if (type == "subtask")
        {
            sqlP = "select id as sid,isnull(task,0) as tid, isnull(Milestoneid,0) as mid,isnull(ProjectId,0) as pid from pms_tbl_addsubtask where status=1 and archive=0 and id=" + rid + "";
        }
        
        try
        {
            DataSet dsD = da1.GetDataSetInline(sqlP);
            if (dsD.Tables[0].Rows.Count > 0)
            {
                pid = dsD.Tables[0].Rows[0]["pid"].ToString();
                mid = dsD.Tables[0].Rows[0]["mid"].ToString();
                tid = dsD.Tables[0].Rows[0]["tid"].ToString();
                sid = dsD.Tables[0].Rows[0]["sid"].ToString();
            }
         }
        catch {
        }
        int checkweitage = 0;
        decimal wetagecount = 0;
        decimal totalwetage = 0;
        
        if (type == "milestone")
        {
            try
            {
                string strweightage = "select isnull(checkprojectWeightage ,0) as checkprojectWeightage  from Pms_tbl_AddProject where id='" + pid + "' and  status=1 and  isnull(checkprojectWeightage ,0)=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                DataSet dsweitagecheck = da1.GetDataSetInline(strweightage);
                if (dsweitagecheck.Tables[0].Rows.Count > 0)
                {

                    checkweitage = Convert.ToInt32(dsweitagecheck.Tables[0].Rows[0]["checkprojectWeightage"]);

                }
                //   iscomplete

                if (checkweitage == 1)
                {
                    string strweightagecount = "select count (*) as WeitageCount from Pms_tbl_AddMilestone where ProjectId='" + pid + "' and  status=1   and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                    DataSet dsweitagecount = da1.GetDataSetInline(strweightagecount);
                    if (dsweitagecount.Tables[0].Rows.Count > 0)
                    {


                        wetagecount = Convert.ToInt32(dsweitagecount.Tables[0].Rows[0]["WeitageCount"]);

                    }

                    totalwetage = 100 / wetagecount;
                    string strstatusupdate = "update Pms_tbl_AddMilestone set Weightage='" + fns.ToFormattedCurrencyString(totalwetage.ToString()) + "' where ProjectId='" + pid + "' and status=1  and archive=0";
                    DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                }


                int iscomplete = 0;
                string findsqlismile = "select isnull(iscomplete,0) as iscomplete, * from Pms_tbl_AddMilestone where id=" + mid + " and status=1 and archive=0";
                DataSet dsfindidismile = da1.GetDataSetInline(findsqlismile);
                if (dsfindidismile.Tables[0].Rows.Count > 0)
                {

                    iscomplete = Convert.ToInt32(dsfindidismile.Tables[0].Rows[0]["iscomplete"]);

                }



                if (iscomplete == 1)
                {
                    decimal milestonetotalpercent = 100;
                    decimal totlweitagemile = 0;
                    //string finsql1 = "select * from Pms_tbl_AddMilestone where id=" + mid + " and status=1  and archive=0";
                    //DataSet dsfindid1 = da1.GetDataSetInline(finsql1);

                    // Milstone percentage //
                    string strstatusupdate = "update Pms_tbl_AddMilestone set iscompletepercentage='" + milestonetotalpercent.ToString() + "' where id='" + mid + "' and isnull(iscomplete,0)=1 and status=1 and archive=0";
                    DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                    // project percentage //
                    string finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + pid + "'  and status=1  and archive=0";
                    DataSet dsfindid2 = da1.GetDataSetInline(finsql2);
                    for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
                    {
                        decimal sumweitage = 0;
                        decimal subpercentageemile = 0;
                        sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
                        subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                        totlweitagemile += (sumweitage * subpercentageemile) / 100;

                    }

                    string finsql4 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitagemile.ToString() + "' where id='" + pid + "'  and status=1 and archive=0";
                    DataSet dsfindid4 = da1.GetDataSetInline(finsql4);

                }
                else
                {
                    string finsql2 = "";
                    DataSet dsfindid2;
                    decimal milestonetotalpercent = 0;
                    decimal totlweitagemile = 0;
                    decimal totlweitageproject = 0;
                    string finsql1 = "select * from Pms_tbl_AddMilestone where id=" + mid + " and status=1 and archive=0";
                    DataSet dsfindid1 = da1.GetDataSetInline(finsql1);
                    if (dsfindid1.Tables[0].Rows.Count > 0)
                    {
                        if (dsfindid1.Tables[0].Rows[0]["istask"].ToString() == "1")
                        {
                            finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where  Milestoneid='" + mid + "'  and status=1  and archive=0";
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
                    string strstatusupdate = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile.ToString() + "' where id='" + mid + "' and status=1 and archive=0 and isnull(iscomplete,0)=0";
                    DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                    // project percentage //
                    finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + pid + "'  and status=1  and archive=0";
                    dsfindid2 = da1.GetDataSetInline(finsql2);
                    for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
                    {
                        decimal sumweitage = 0;
                        decimal subpercentageemile = 0;
                        sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
                        subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                        totlweitageproject += (sumweitage * subpercentageemile) / 100;

                    }
                    string finsql4 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + pid + "'  and status=1 and archive=0";
                    DataSet dsfindid4 = da1.GetDataSetInline(finsql4);



                }
            }
            catch { retval = "0"; }
        }
        else if (type == "task")
        {
            try
            {
                string strweightage = "select isnull(checkprojectWeightage ,0) as checkprojectWeightage  from Pms_tbl_AddProject where id='" + pid + "' and  status=1 and  isnull(checkprojectWeightage ,0)=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                DataSet dsweitagecheck = da1.GetDataSetInline(strweightage);
                if (dsweitagecheck.Tables[0].Rows.Count > 0)
                {

                    checkweitage = Convert.ToInt32(dsweitagecheck.Tables[0].Rows[0]["checkprojectWeightage"]);

                }
                if (checkweitage == 1)
                {
                    string strweightagecount = "select count (*) as WeitageCount from Pms_tbl_Addtask where Milestoneid='" + mid + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                    DataSet dsweitagecount = da1.GetDataSetInline(strweightagecount);
                    if (dsweitagecount.Tables[0].Rows.Count > 0)
                    {

                        wetagecount = Convert.ToInt32(dsweitagecount.Tables[0].Rows[0]["WeitageCount"]);

                    }

                    totalwetage = 100 / wetagecount;
                    string strstatusupdate = "update Pms_tbl_Addtask set Weightage='" + fns.ToFormattedCurrencyString(totalwetage.ToString()) + "' where Milestoneid='" + mid + "' and status=1 and archive=0";
                    DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                }

                int iscomplete = 0;
                string finsql1 = "select isnull(iscomplete,0) as iscomplete, * from Pms_tbl_Addtask where id=" + tid + " and status=1 and archive=0";
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

                    string finsql01 = "select * from Pms_tbl_Addtask where id=" + tid + " and status=1  and archive=0";
                    DataSet dsfindid01 = da1.GetDataSetInline(finsql01);

                    // task percentage //
                    string strstatusupdate = "update Pms_tbl_Addtask set iscompletepercentage='" + tsktotalpercent.ToString() + "' where id='" + tid + "' and status=1 and archive=0 and isnull(iscomplete,0)=1";
                    DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                    // Milestone percentage //
                    string finsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where ProjectId='" + pid + "' and Milestoneid='" + mid + "'   and status=1  and archive=0";
                    DataSet dsfindmile = da1.GetDataSetInline(finsqlmile);
                    for (int j = 0; j < dsfindmile.Tables[0].Rows.Count; j++)
                    {
                        decimal sumweitage = 0;
                        decimal subpercentageemile = 0;
                        sumweitage = Convert.ToDecimal(dsfindmile.Tables[0].Rows[j]["Weightage"].ToString());
                        subpercentageemile = Convert.ToDecimal(dsfindmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                        totlweitagemile += (sumweitage * subpercentageemile) / 100;

                    }

                    string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile.ToString() + "' where ProjectId='" + pid + "' and id='" + mid + "'  and status=1 and archive=0";
                    DataSet dsfindid4 = da1.GetDataSetInline(finsql4);


                    // project percentage //
                    string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + pid + "'   and status=1  and archive=0";
                    DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
                    for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
                    {
                        decimal sumweitageprojt = 0;
                        decimal subpercentageeproject = 0;
                        sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                        subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                        totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

                    }

                    string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + pid + "'  and status=1 and archive=0";
                    DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);




                }
                else
                {
                    decimal tsktotalpercent = 0;
                    decimal totlweitagemile = 0;
                    decimal totlweitageproject = 0;

                    string finsql01 = "select * from Pms_tbl_Addtask where id=" + tid + " and status=1  and archive=0";
                    DataSet dsfindid01 = da1.GetDataSetInline(finsql01);


                    if (dsfindid01.Tables[0].Rows.Count > 0)
                    {
                        if (dsfindid01.Tables[0].Rows[0]["issubtask"].ToString() == "1")
                        {

                            string finsql211 = "select isnull(sum(weightage),0)Wattagetasksum from Pms_tbl_Addsubtask where   task='" + tid + "'  and status=1 and isnull(iscomplete,0)=1 and archive=0";
                            DataSet dsfindid211 = da1.GetDataSetInline(finsql211);
                            if (dsfindid211.Tables[0].Rows.Count > 0)
                            {
                                string strstatusupdate = "update Pms_tbl_Addtask set iscompletepercentage='" + dsfindid211.Tables[0].Rows[0]["Wattagetasksum"] + "' where id='" + tid + "' and status=1 and archive=0";
                                DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);
                            }
                        }
                        else
                        {
                            string strstatusupdate = "update Pms_tbl_Addtask set iscompletepercentage='0' where id='" + tid + "' and status=1 and archive=0";
                            DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                        }

                    }


                    // Milestone percentage //
                    string finsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where ProjectId='" + pid + "' and Milestoneid='" + mid + "'   and status=1  and archive=0";
                    DataSet dsfindmile = da1.GetDataSetInline(finsqlmile);
                    for (int j = 0; j < dsfindmile.Tables[0].Rows.Count; j++)
                    {
                        decimal sumweitage = 0;
                        decimal subpercentageemile = 0;
                        sumweitage = Convert.ToDecimal(dsfindmile.Tables[0].Rows[j]["Weightage"].ToString());
                        subpercentageemile = Convert.ToDecimal(dsfindmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                        totlweitagemile += (sumweitage * subpercentageemile) / 100;

                    }

                    string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile.ToString() + "' where ProjectId='" + pid + "' and id='" + mid + "'  and status=1 and archive=0";
                    DataSet dsfindid4 = da1.GetDataSetInline(finsql4);

                    // project percentage //
                    string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + pid + "'   and status=1  and archive=0";
                    DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
                    for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
                    {
                        decimal sumweitageprojt = 0;
                        decimal subpercentageeproject = 0;
                        sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                        subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                        totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

                    }

                    string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + pid + "'  and status=1 and archive=0";
                    DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);



                }
            }
            catch { retval = "0"; }
        }
        else if (type == "subtask")
        {
            try
            {
                string strweightage = "select isnull(checkTaskWeightage ,0) as checkTaskWeightage  from pms_tbl_addtask where id='" + tid + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                DataSet dsweitagecheck = da1.GetDataSetInline(strweightage);
                if (dsweitagecheck.Tables[0].Rows.Count > 0)
                {

                    checkweitage = Convert.ToInt32(dsweitagecheck.Tables[0].Rows[0]["checkTaskWeightage"]);

                }
                if (checkweitage == 1)
                {
                    string strweightagecount = "select count (*) as WeitageCount from Pms_tbl_Addsubtask where task='" + tid + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                    DataSet dsweitagecount = da1.GetDataSetInline(strweightagecount);
                    if (dsweitagecount.Tables[0].Rows.Count > 0)
                    {

                        wetagecount = Convert.ToInt32(dsweitagecount.Tables[0].Rows[0]["WeitageCount"]);

                    }

                    totalwetage = 100 / wetagecount;
                    string strstatusupdate = "update Pms_tbl_Addsubtask set Weightage='" + fns.ToFormattedCurrencyString(totalwetage.ToString()) + "' where task='" + tid + "' and status=1 and archive=0";
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

                           string finsql2 = "select isnull(sum(weightage),0)Wattagetasksum from Pms_tbl_Addsubtask where ProjectId='" +pid + "' and Milestoneid='" + mid + "' and task='" + tid + "'  and status=1 and isnull(iscomplete,0)=1 and archive=0";
                    DataSet dsfindid2 = da1.GetDataSetInline(finsql2);

                    //sub task percentage //
                    string strstatusupdate = "update Pms_tbl_Addsubtask set iscompletepercentage='" + subtsktotalpercent.ToString() + "' where id='" + sid + "' and status=1 and archive=0 and isnull(iscomplete,0)=1";
                    DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                    // task percentage //
                    string finsql3 = "update Pms_tbl_Addtask set iscompletepercentage='" + dsfindid2.Tables[0].Rows[0]["Wattagetasksum"] + "' where ProjectId='" + pid + "' and Milestoneid='" + mid + "' and id='" + tid + "' and status=1 and archive=0";
                    DataSet dsfindid3 = da1.GetDataSetInline(finsql3);


                    // Milestone percentage //
                    string findsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask  where ProjectId='" + pid + "' and Milestoneid='" + mid + "'  and status=1  and archive=0";
                    DataSet dsfindidmile = da1.GetDataSetInline(findsqlmile);
                    for (int j = 0; j < dsfindidmile.Tables[0].Rows.Count; j++)
                    {
                        decimal sumweitage = 0;
                        decimal subpercentageemile = 0;
                        sumweitage = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["Weightage"].ToString());
                        subpercentageemile = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                        totlweitagemile += (sumweitage * subpercentageemile) / 100;

                    }

                    string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile + "' where ProjectId='" + pid + "' and id='" + mid + "'  and status=1 and archive=0";
                    DataSet dsfindid4 = da1.GetDataSetInline(finsql4);



                    // project percentage //
                    string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + pid + "'    and status=1  and archive=0";
                    DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
                    for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
                    {
                        decimal sumweitageprojt = 0;
                        decimal subpercentageeproject = 0;
                        sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                        subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                        totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

                    }

                    string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + pid + "'  and status=1 and archive=0";
                    DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);

                }

                else
                {
                    decimal subtsktotalpercent = 0;
                    decimal totlweitagemile = 0;
                    decimal totlweitageproject = 0;
                    
                    string finsql2 = "select isnull(sum(weightage),0)Wattagetasksum from Pms_tbl_Addsubtask where ProjectId='" + pid+ "' and Milestoneid='" + mid + "' and task='" + tid + "'  and status=1 and isnull(iscomplete,0)=1 and archive=0";
                    DataSet dsfindid2 = da1.GetDataSetInline(finsql2);

                    //Sub task percentage //
                    string strstatusupdate = "update Pms_tbl_Addsubtask set iscompletepercentage='" + subtsktotalpercent.ToString() + "' where id='" + sid + "' and status=1 and archive=0 and isnull(iscomplete,0)=0";
                    DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                    // task percentage //
                    string finsql3 = "update Pms_tbl_Addtask set iscompletepercentage='" + dsfindid2.Tables[0].Rows[0]["Wattagetasksum"] + "' where ProjectId='" + pid + "' and Milestoneid='" + mid + "' and id='" + tid + "' and status=1 and archive=0";
                    DataSet dsfindid3 = da1.GetDataSetInline(finsql3);


                    // Milestone percentage //
                    string findsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask  where ProjectId='" + pid + "' and Milestoneid='" + mid + "'    and status=1  and archive=0";
                    DataSet dsfindidmile = da1.GetDataSetInline(findsqlmile);
                    for (int j = 0; j < dsfindidmile.Tables[0].Rows.Count; j++)
                    {
                        decimal sumweitage = 0;
                        decimal subpercentageemile = 0;
                        sumweitage = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["Weightage"].ToString());
                        subpercentageemile = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                        totlweitagemile += (sumweitage * subpercentageemile) / 100;

                    }

                    string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile + "' where ProjectId='" + pid + "' and id='" + mid + "'  and status=1 and archive=0";
                    DataSet dsfindid4 = da1.GetDataSetInline(finsql4);


                    // project percentage //
                    string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + pid + "'   and status=1  and archive=0";
                    DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
                    for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
                    {
                        decimal sumweitageprojt = 0;
                        decimal subpercentageeproject = 0;
                        sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                        subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                        totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

                    }

                    string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + pid + "'  and status=1 and archive=0";
                    DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);


                }

            }
            catch
            {
                retval = "0";
            }
        }
            return retval;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static completiondetails_mile bindcompletiondetails_mile(string RecordId)
    {
        DataAccess da = new DataAccess();
        completiondetails_mile obj = new completiondetails_mile();
        try
        {
            if (string.IsNullOrEmpty(RecordId))
                RecordId = "0";
            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null)
                return obj;
            string regid = regCookie["id"];
            string strsql = "select * from Pms_tbl_Addmilestone where id=" + RecordId + " and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
            SqlDataReader dr = da.getDataReaderInline(strsql);
            while (dr.Read())
            {
                obj.id = dr["id"] != DBNull.Value ? Convert.ToInt32(dr["id"]) : (int?)null;
                obj.ProjectId = dr["ProjectId"] != DBNull.Value ? Convert.ToInt32(dr["ProjectId"]) : (int?)null;
                double mileTaskTotal = 0;
                double mileWattagepending = 100;
                try
                {

                    mileTaskTotal = get_completionstatus(RecordId, "milestone");
                    if (mileTaskTotal > 0)
                    {
                        mileWattagepending = mileWattagepending - mileTaskTotal;
                    }
                }
                catch { }
                obj.complete_value = mileTaskTotal;
                obj.pending_value = mileWattagepending;
                List<newtask_details> newtasklist = bindtaskdetails_bymileid(RecordId);
                obj.tasks_details = newtasklist;
                

            }


        }
        catch (Exception ex)
        {
            SaveLogText("chkadmintaskEX-" + ex.Message);
            return new completiondetails_mile();
        }


        return obj;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static newtask_details bindcompletiondetails_task(string RecordId)
    {
        DataAccess da = new DataAccess();
        newtask_details obj = new newtask_details();
        try
        {
            if (string.IsNullOrEmpty(RecordId))
                RecordId = "0";
            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null)
                return obj;
            string regid = regCookie["id"];
            string strsql = "select * from Pms_tbl_Addtask where id=" + RecordId + " and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
            SqlDataReader dr = da.getDataReaderInline(strsql);
            while (dr.Read())
            {
                obj.id = dr["id"] != DBNull.Value ? Convert.ToInt32(dr["id"]) : (int?)null;
                obj.ProjectId = dr["ProjectId"] != DBNull.Value ? Convert.ToInt32(dr["ProjectId"]) : (int?)null;
                obj.Milestoneid = dr["Milestoneid"] != DBNull.Value ? Convert.ToInt32(dr["Milestoneid"]) : (int?)null;
                double mileTaskTotal = 0;
                double mileWattagepending = 100;
                try
                {

                    mileTaskTotal = get_completionstatus(RecordId, "task");
                    if (mileTaskTotal > 0)
                    {
                        mileWattagepending = mileWattagepending - mileTaskTotal;
                    }
                }
                catch { }
                obj.complete_value = mileTaskTotal;
                obj.pending_value = mileWattagepending;
                List<newsubtask_details> newsubtasklist = bindsubtaskdetails_bytaskid(RecordId);
                obj.subtasks_details = newsubtasklist;


            }


        }
        catch (Exception ex)
        {
            SaveLogText("chkadmintaskEX-" + ex.Message);
            return new newtask_details();
        }


        return obj;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static newsubtask_details bindcompletiondetails_subtask(string RecordId)
    {
        DataAccess da = new DataAccess();
        newsubtask_details obj = new newsubtask_details();
        try
        {
            if (string.IsNullOrEmpty(RecordId))
                RecordId = "0";
            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null)
                return obj;
            string regid = regCookie["id"];
            string strsql = "select * from Pms_tbl_Addsubtask where id=" + RecordId + " and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
            SqlDataReader dr = da.getDataReaderInline(strsql);
            while (dr.Read())
            {
                obj.id = dr["id"] != DBNull.Value ? Convert.ToInt32(dr["id"]) : (int?)null;
                obj.ProjectId = dr["ProjectId"] != DBNull.Value ? Convert.ToInt32(dr["ProjectId"]) : (int?)null;
                obj.Milestoneid = dr["Milestoneid"] != DBNull.Value ? Convert.ToInt32(dr["Milestoneid"]) : (int?)null;
                obj.Task = dr["Task"] != DBNull.Value ? Convert.ToInt32(dr["Task"]) : (int?)null;
                double mileTaskTotal = 0;
                double mileWattagepending = 100;
                try
                {

                    mileTaskTotal = get_completionstatus(RecordId, "subtask");
                    if (mileTaskTotal > 0)
                    {
                        mileWattagepending = mileWattagepending - mileTaskTotal;
                    }
                }
                catch { }
                obj.complete_value = mileTaskTotal;
                obj.pending_value = mileWattagepending;
               
            }


        }
        catch (Exception ex)
        {
            SaveLogText("chkadmintaskEX-" + ex.Message);
            return new newsubtask_details();
        }


        return obj;
    }




    public static List<newsubtask_details>bindsubtasklist_bytaskid_details(string RecordId)
    {
        DataAccess da = new DataAccess();
        List<newsubtask_details> obj = new List<newsubtask_details>();
        try
        {
            if (string.IsNullOrEmpty(RecordId))
                RecordId = "0";

            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null)
                return obj;

            string regid = regCookie["id"];
            try
            {
                string str2 = "select * from Pms_tbl_Addsubtask where regid='" + regid + "' and task='" + RecordId + "' and status=1 and archive=0 order by isnull(orderno,0)";


                DataAccess danew = new DataAccess();
                DataSet drsubtask = danew.GetDataSetInline(str2);
                foreach (DataRow row in drsubtask.Tables[0].Rows)
                {
                    double mileTaskTotal = 0;
                    mileTaskTotal = get_completionstatus(row["id"].ToString(), "subtask");
                    double mileWattagecomplite = mileTaskTotal;
                    double mileWattagepending = 100;
                    if (mileTaskTotal > 0)
                    {
                        mileWattagepending = mileWattagepending - mileTaskTotal;
                    }
                    obj.Add(new newsubtask_details
                    {

                        id = row["id"] as int?,
                        ProjectId = row["ProjectId"] as int?,
                        Milestoneid = row["Milestoneid"] as int?,
                        Task = row["task"] as int?,
                        complete_value = Math.Round(mileTaskTotal, 2),
                        pending_value = Math.Round(mileWattagepending, 2),

                    });
                }

            }
            catch { }




        }
        catch (Exception ex)
        {
            SaveLogText("chkadminsubtaskEX-" + ex.Message);
            return new List<newsubtask_details>();
        }


        return obj;
    }
    #endregion
    public static int GetWeekNumber(DateTime dtime)
    {
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        int weekno = 0;
        SqlParameter[] prmw = new SqlParameter[1];
        prmw[0] = new SqlParameter("@DATE", dtime);

        DataSet dsw = da.GetDataSet("selectweeknofromdate", prmw);
        if (dsw.Tables[0].Rows.Count > 0)
        {
            weekno = Convert.ToInt32(dsw.Tables[0].Rows[0]["WEEK_OF_MONTH"].ToString());
        }
        return weekno;
        //int count = 0;
        //if (dtime.Day <= 7)
        //{
        //    count = 1;
        //}
        //else if (dtime.Day > 7 && dtime.Day <= 14)
        //{
        //    count = 2;
        //}
        //else if (dtime.Day > 14 && dtime.Day <= 21)
        //{
        //    count = 3;
        //}
        //else if (dtime.Day > 21 && dtime.Day <= 28)
        //{
        //    count = 4;
        //}
        //else if (dtime.Day > 28)
        //{
        //    count = 5;
        //}

        //return count;
    }

    public static int GetdayNumber(DateTime dtime)
    {
        //    CultureInfo ciCurr = CultureInfo.CurrentCulture;
        //    int weekNum = ciCurr.Calendar.GetWeekOfYear(dtPassed, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        //    return weekNum;
        //}

        int count = 0;
        int day = (int)dtime.DayOfWeek;
        count = day + 1;
        return count;
    }
    public static bool getweeklyoff(DateTime edate, string empid = "")
    {
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();

        int wcount = 0;
        int dno = 0;
        wcount = GetWeekNumber(Convert.ToDateTime(edate));
        dno = GetdayNumber(Convert.ToDateTime(edate));
        SaveLogText("dayno-" + dno + "-" + edate);
        string w1, wt1, w2, wt2, w3, wt3, w4, wt4, w5, wt5, w6, wt6, w7, wt7;
        string strret = "";

        string strsql = "select * from tbl_weekoffmaster where  status = 1 and archive = 0 and isnull(isdefault,0)=1 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  ";
        SaveLogText("strsqlweekoff-" + strsql);
        DataSet dsh = da.GetDataSetInline(strsql);

        //SqlParameter[] prmh = new SqlParameter[1];
        //prmh[0] = new SqlParameter("@empid", empid);
        //DataSet dsh = da.GetDataSet("WeeklyOffPolicybyempid", prmh);

        if (dsh.Tables[0].Rows.Count > 0)
        {

            w1 = dsh.Tables[0].Rows[0]["isweekoff1"].ToString();
            wt1 = dsh.Tables[0].Rows[0]["isweekoff1type"].ToString();
            w2 = dsh.Tables[0].Rows[0]["isweekoff2"].ToString();
            wt2 = dsh.Tables[0].Rows[0]["isweekoff2type"].ToString();
            w3 = dsh.Tables[0].Rows[0]["isweekoff3"].ToString();
            wt3 = dsh.Tables[0].Rows[0]["isweekoff3type"].ToString();
            w4 = dsh.Tables[0].Rows[0]["isweekoff4"].ToString();
            wt4 = dsh.Tables[0].Rows[0]["isweekoff4type"].ToString();
            w5 = dsh.Tables[0].Rows[0]["isweekoff5"].ToString();
            wt5 = dsh.Tables[0].Rows[0]["isweekoff5type"].ToString();
            w6 = dsh.Tables[0].Rows[0]["isweekoff6"].ToString();
            wt6 = dsh.Tables[0].Rows[0]["isweekoff6type"].ToString();
            w7 = dsh.Tables[0].Rows[0]["isweekoff7"].ToString();
            wt7 = dsh.Tables[0].Rows[0]["isweekoff7type"].ToString();
            int returntype = 0;
            if (w1 == "1" && dno.ToString() == "1")
            {
                returntype = getreturnstatus(wt1, wcount);

            }
            if (w2 == "1" && dno.ToString() == "2")
            {
                returntype = getreturnstatus(wt2, wcount);

            }

            if (w3 == "1" && dno.ToString() == "3")
            {
                returntype = getreturnstatus(wt3, wcount);

            }

            if (w4 == "1" && dno.ToString() == "4")
            {
                returntype = getreturnstatus(wt4, wcount);

            }

            if (w5 == "1" && dno.ToString() == "5")
            {
                returntype = getreturnstatus(wt5, wcount);

            }

            if (w6 == "1" && dno.ToString() == "6")
            {
                returntype = getreturnstatus(wt6, wcount);

            }
            if (w7 == "1" && dno.ToString() == "7")
            {
                returntype = getreturnstatus(wt7, wcount);

            }
            SaveLogText("returntype-" + returntype);
            if (returntype == 1)
            {
                return true;
            }
        }


        return false;

        //}
        //catch (Exception ex)
        //{
        //    return false;
        //}
    }

    public static int getreturnstatus(string wt1chk, int wcountchk)
    {
        if (wt1chk == "1")
        {
            return 1;
        }
        if (wt1chk == "2" && wcountchk == 1)
        {
            return 1;
        }
        if (wt1chk == "3" && wcountchk == 2)
        {
            return 1;
        }
        if (wt1chk == "4" && wcountchk == 3)
        {
            return 1;
        }
        if (wt1chk == "5" && wcountchk == 4)
        {
            return 1;
        }
        if (wt1chk == "6" && wcountchk == 5)
        {
            return 1;
        }
        if (wt1chk == "7" && (wcountchk == 1 || wcountchk == 3))
        {
            return 1;
        }
        if (wt1chk == "8" && (wcountchk == 1 || wcountchk == 3 || wcountchk == 5))
        {
            return 1;
        }
        if (wt1chk == "9" && (wcountchk == 2 || wcountchk == 4))
        {
            return 1;
        }

        return 0;
    }



    public static bool getholiday(DateTime edate, string empid = "")
    {
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        try
        {

            // Response.Write(cmf.Decrypt(empid) + "," + edate.ToShortDateString());
            //SqlParameter[] prmh = new SqlParameter[2];
            //prmh[0] = new SqlParameter("@empid", empid);
            //prmh[1] = new SqlParameter("@cdate", edate);
            //DataSet dsh = da.GetDataSet("holidaysselectbydate", prmh);
            string strsql = "select * from holidays where cast(eventdate as date)=cast('" + edate + "' as date) and status = 1 and archive = 0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'    ";
            DataSet dsh = da.GetDataSetInline(strsql);
            if (dsh.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                //SqlParameter[] prmrh = new SqlParameter[2];
                //prmrh[0] = new SqlParameter("@empid", empid);
                //prmrh[1] = new SqlParameter("@cdate", edate);
                //DataSet dshr = da.GetDataSet("resholidaysselectbydate", prmrh);
                //if (dshr.Tables[0].Rows.Count > 0)
                //{
                //    return true;
                //}
                //else
                //{

                //    return false;
                //}

                return false;
            }
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    private static object ToDbInt(object value, bool defaultZero = false)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            return defaultZero ? (object)0 : DBNull.Value;

        int result;
        return int.TryParse(value.ToString(), out result)
            ? (object)result
            : (defaultZero ? (object)0 : DBNull.Value);
    }
    


    public static string SafeGetString(IDataReader dr, string col)
    {
        try
        {
            int ordinal = dr.GetOrdinal(col);
            return dr.IsDBNull(ordinal) ? "" : dr.GetValue(ordinal).ToString();
        }
        catch
        {
            return "";
        }
    }

    public static DateTime? SafeGetDate(IDataReader dr, string col)
    {
        int ordinal = dr.GetOrdinal(col);
        return dr.IsDBNull(ordinal) ? (DateTime?)null : Convert.ToDateTime(dr.GetValue(ordinal));
    }
    public static DateTime? GetNullableDate(object value)
    {
        return value == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(value);
    }
    public static bool? SafeGetBool(IDataReader dr, string col)
    {
        int ordinal = dr.GetOrdinal(col);
        return dr.IsDBNull(ordinal) ? (bool?)null : Convert.ToBoolean(dr.GetValue(ordinal));
    }

    public static decimal? SafeGetDecimal(IDataReader dr, string col)
    {
        int ordinal = dr.GetOrdinal(col);
        return dr.IsDBNull(ordinal) ? (decimal?)null : Convert.ToDecimal(dr.GetValue(ordinal));
    }


    public static List<T>getemployeelist<T>(string ids) where T : IPerson, new()
    {
        if (string.IsNullOrEmpty(ids) || ids == "0")
            return new List<T>();

        string str2 = "select id, DisplayName, fileupload1, SUBSTRING(DisplayName,1,2) as splinameone " +
                      "from tbl_employee " +
                      "where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' " +
                      "and id in(" + ids + ")";

        DataAccess danew = new DataAccess();
        DataSet drmile = danew.GetDataSetInline(str2);

        List<T> list = new List<T>();

        foreach (DataRow row in drmile.Tables[0].Rows)
        {
            T obj = new T();
            obj.id = row["id"].ToString();
            obj.name = row["DisplayName"].ToString();
            obj.profileimage = row["fileupload1"].ToString();
            obj.shortname = row["splinameone"].ToString();
            list.Add(obj);
        }

        return list;
    }
    public static int checktaskby_milestoneid(string mid)
     {
        int ret = 0;
        try
        {
            string str2 = "select id from Pms_tbl_Addtask where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and Milestoneid ='" + mid + "'";
            SaveLogText("gettasklist="+ str2);
            DataAccess danew = new DataAccess();
            DataSet drtsk = danew.GetDataSetInline(str2);
            if (drtsk.Tables[0].Rows.Count > 0)
            {
                ret = 1;
            }
        }
        catch { }
        return ret;
     }
  
}
public class item_summary
{
    public string status{ get; set; }
    public double? copmelete_value { get; set; }
    public double? pending_value { get; set; }
   
}
public class project_details
{
    public int? id { get; set; }
    public string ProjectID { get; set; }
    public string ProjectName { get; set; }
    public string Owner { get; set; }
    public string team { get; set; }
    public int? ProjectStatus { get; set; }
    public DateTime? AwardDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? TargetDate { get; set; }
    public int? ApprovalDrawing { get; set; }
    public int? salesorderid { get; set; }
    public int? customerid { get; set; }
    public int? checkprojectWeightage { get; set; }
    public int? hastask { get; set; }
    public int? checktaskWeightage { get; set; }
    public int? hassubtask { get; set; }
    public int? checksubtaskWeightage { get; set; }
    public string editcolor { get; set; }
    public List<ownerlist> owners { get; set; }
    public List<teamlist> teams { get; set; }
    public List<newmilestone> miles { get; set; }
    public bool isadmin { get; set; }




}
public class milestone_details
{
    public int? id { get; set; }
    public int?ProjectId { get; set; }
    public string Milestone { get; set; }
    public double? Weightage { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Owner { get; set; }
    public string team { get; set; }
    public int? P_Status { get; set; }
    public int? Priority { get; set; }
    public int? istask { get; set; }
    public List<newtask> tasks { get; set; }



}
public class newmilestone
{
    public int? id { get; set; }
    public int? ProjectId { get; set; }
    public string Milestone { get; set; }
    public double? Weightage { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Owner { get; set; }
    public int? Access { get; set; }
    public string team { get; set; }
    public int? P_Status { get; set; }
    public string status_name { get; set; }
    public string status_color { get; set; }
    public int? Priority {get; set;}
    public string priority_name { get; set; }
    public string priority_color { get; set; }
    public double? Budget { get; set; }
    public double? Actual { get; set; }
    public int? uid { get; set; }
    public DateTime? cdate { get; set; }
    public int? statuscomplete { get; set; }
    public int? istask { get; set; }
    public int? checkmileWeightage { get; set; }
    public int? iscomplete { get; set; }
    public DateTime? iscompletedate { get; set; }
    public double? iscompletepercentage { get; set; }
    public int? Durationtype { get; set; }
    public string TaskDuration { get; set; }
    public string TaskDuration_min { get; set; }
    public double? Durationhourcal { get; set; }
    public int? orderno { get; set; }
    public string miledescription { get; set; }
    public string Uploaddrawing { get; set; }
    public string txtdays { get; set; }
    public string complete_value { get; set; }
    public string pending_value { get; set; }
    public int? taskcount { get; set; }
    public bool isExpanded { get; set; }    
    public string leftdays { get; set; }
    public int? totalissue { get; set; }
    public int? pendingissue { get; set; }
    public int? completeissue { get; set; }
    public double? pendingissue_rate { get; set; }
    public double? completeissue_rate { get; set; }

    public int? totaldependency { get; set; }


    public List<ownerlist> owners { get; set; }
    public List<teamlist> teams { get; set; }
    public List<projectfiles>viewfiles { get; set; }
    public List<newtask> tasks { get; set; }
    //public List<issuestdata> mileissues { get; set; }



}
public class newtask
{
    public int? id { get; set; }
    public int? ProjectId { get; set; }
    public int? Milestoneid { get; set; }
    public string taskname { get; set; }
    public string taskdescripton { get; set; }
    public string owner { get; set; }
    public string team { get; set; }
    public DateTime? Startdate { get; set; }
    public DateTime? Targetdate { get; set; }
    public double? Weightage { get; set; }
    public string StartTime { get; set; }
    public string Timeline { get; set; }
    public string TaskDuration { get; set; }
    public string TaskDuration_min { get; set; }
    public int? P_Status { get; set; }
    public string status_name { get; set; }
    public string status_color { get; set; }
    public int? Priority { get; set; }
    public string priority_name { get; set; }
    public string priority_color { get; set; }
    public double? Budget { get; set; }
    public double? Actual { get; set; }
    public string Reminder { get; set; }
    public int? ReminderType { get; set; }
    public string Days { get; set; }
    public string Time { get; set; }
    public int? uid { get; set; }
    public DateTime? cdate { get; set; }
    public string tagto { get; set; }
    public string dependancy { get; set; }
    public int? issubtask { get; set; }
    public string checkTaskWeightage { get; set; }
    public string iscomplete { get; set; }
    public double? iscompletepercentage { get; set; }
    public int? orderno { get; set; }
    public string txtdays { get; set; }
    public string complete_value { get; set; }
    public string pending_value { get; set; }
    public bool isExpanded { get; set; }
    
    public string leftdays { get; set; }
    public int? totalissue { get; set; }
    public int? pendingissue { get; set; }
    public int? completeissue { get; set; }
    public double? pendingissue_rate { get; set; }
    public double? completeissue_rate { get; set; }
    public int? totaldependency { get; set; }
    public List<ownerlist> owners { get; set; }
    public List<teamlist> teams { get; set; }
    public List<projectfiles> viewfiles { get; set; }
    public List<newsubtask> subtasks { get; set; }
    //public List<issuestdata> taskissues { get; set; }


}
public class newsubtask
{
    public int? id { get; set; }
    public int? ProjectId { get; set; }
    public int? Milestoneid { get; set; }
    public int? Task { get; set; }
    public string Subtaskname { get; set; }
    public string SubTaskDescription { get; set; }
    public string owner { get; set; }
    public string team { get; set; }
    public DateTime? Startdate { get; set; }
    public DateTime? Targetdate { get; set; }
    public double? Weightage { get; set; }
    public string StartTime { get; set; }
    public string Timeline { get; set; }
    public string TaskDuration { get; set; }
    public string TaskDuration_min { get; set; }
    public int? P_Status { get; set; }
    public string status_name { get; set; }
    public string status_color { get; set; }
    public int? Priority { get; set; }
    public string priority_name { get; set; }
    public string priority_color { get; set; }
    public double? Budget { get; set; }
    public double? Actual { get; set; }
    public string Reminder { get; set; }
    public int? ReminderType { get; set; }
    public string Days { get; set; }
    public string Time { get; set; }
    public int? uid { get; set; }
    public DateTime? cdate { get; set; }
    public string tagto { get; set; }
    public string dependancy { get; set; }
    public string iscomplete { get; set; }
    public double? iscompletepercentage { get; set; }
    public int? orderno { get; set; }
    public string txtdays { get; set; }
    public string complete_value { get; set; }
    public string pending_value { get; set; }
    
    public string leftdays { get; set; }
    public int? totalissue { get; set; }
    public int? pendingissue { get; set; }
    public int? completeissue { get; set; }
    public double? pendingissue_rate { get; set; }
    public double? completeissue_rate { get; set; }

    public int? totaldependency { get; set; }

    public List<ownerlist> owners { get; set; }
    public List<teamlist> teams { get; set; }
    public List<projectfiles> viewfiles { get; set; }
    //public List<issuestdata> subtaskissues { get; set; }


}
public class projectfiles
{
    public string id { get; set; }
    public string pid { get; set; }
    public string fentryid { get; set; }
    public string ftype { get; set; }
    public string fileurl { get; set; }
    public string filedes { get; set; }
    public string fext { get; set; }
    public string filepath { get; set; }


}

public class ownerlist : IPerson { public string id { get; set; } public string name { get; set; } public string profileimage { get; set; } public string shortname { get; set; } }
public class teamlist : IPerson { public string id { get; set; } public string name { get; set; } public string profileimage { get; set; } public string shortname { get; set; } }
public class userlist : IPerson { public string id { get; set; } public string name { get; set; } public string profileimage { get; set; } public string shortname { get; set; } }
public class statuslist
{  public string id { get; set; }
    public string name { get; set; }
    public string colorname { get; set; }
    public string iscomplete { get; set; }
    public string isdefault { get; set; }
}
public class prioritylist
{
    public string id { get; set; }
    public string name { get; set; }
    public string colorname { get; set; }
    public string isdefault { get; set; }
}
public interface IPerson
{
    string id { get; set; }
    string name { get; set; }
    string profileimage { get; set; }
    string shortname { get; set; }
}
public class newMilerequest
{
    public newmilestone milestone { get; set; }
    public int orderno { get; set; }
}
public class newTaskrequest
{
    public newtask task { get; set; }
    public int orderno { get; set; }
}
public class newSubTaskrequest
{
    public newsubtask subtask { get; set; }
    public int orderno { get; set; }
}
public class projectowneres
{
    public string id { get; set; }
    public string Owner { get; set; }
    public string Ownertype { get; set; }
}
public class projectteams
{
    public string id { get; set; }
    public string Team { get; set; }
    public string Teamtype { get; set; }
}
public class newuserlist {
    public string id { get; set; }
    public string name { get; set; }
    
}
public class issuesdetails
{
    public int? totalissue { get; set; }
    public int? completeissue { get; set; }
    public int? pendingissue { get; set; }
    public double? completeissues_rate { get; set; }
    public double? pendingissues_rate { get; set; }

}
public class issuestdata
{
    public int? id { get; set; }
    public int? ProjectId { get; set; }
    public int? Milestoneid { get; set; }
    public string tasklistid { get; set; }
    public int? taskid { get; set; }
    public int? subtaskid { get; set; }
    public string Issues { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Owner { get; set; }
    public string team { get; set; }
    public string Issueon { get; set; }
    public string project_id { get; set; }
    public string Description { get; set; }
    public string Reminder { get; set; }
    public string ReminderType { get; set; }
    public string Days { get; set; }
    public string Time { get; set; }
    public string Notifyusers { get; set; }
    public string ChooseDate { get; set; }
    public string AssignTo { get; set; }
    public int? IssueStatus { get; set; }
    public int? Priority { get; set; }
    public string IssueType { get; set; }
    public string Replicability { get; set; }
    public string UploadImage { get; set; }

}
public class completiondetails_mile
{
    public int? id { get; set; }
    public int? ProjectId { get; set; }
    public double? complete_value { get; set; }
    public double? pending_value { get; set; }
    public List<newtask_details> tasks_details { get; set; }

}
public class newtask_details
{
    public int? id { get; set; }
    public int? ProjectId { get; set; }
    public int? Milestoneid { get; set; }
    public double? complete_value { get; set; }
    public double? pending_value { get; set; }
    public List<newsubtask_details> subtasks_details { get; set; }  


}
public class newsubtask_details
{
    public int? id { get; set; }
    public int? ProjectId { get; set; }
    public int? Milestoneid { get; set; }
    public int? Task { get; set; }
    public double? complete_value { get; set; }
    public double? pending_value { get; set; }
}
public class projectlists
{
    public string id { get; set; }
    public string ProjectName { get; set; }
}


