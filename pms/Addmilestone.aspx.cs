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

public partial class Addmilestone : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataAccess da = new DataAccess();
    public CommonFunction cmf = new CommonFunction();
    SqlParameter[] prm, prm1;
    public function fns = new function();
    public string Milestonedata = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string strvalidationS = get_EditDelete_Validation("54");
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

    //// web method bind using ajax start //

    [WebMethod]
    public static string SaveMilestone(milestonetdata milestonedetails)
    {
        CommonFunction cmf = new CommonFunction();
        function fns = new function();
        string retval = "0";
        string strowner = "";
        string strteam = "";
        DataAccess da1 = new DataAccess();
        string strqlP = "select Owner,team from pms_tbl_addproject where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + milestonedetails.ProjectId + "'";
        DataSet dsDetailsP = da1.GetDataSetInline(strqlP);
        if (dsDetailsP.Tables[0].Rows.Count > 0)
        {
            if (dsDetailsP.Tables[0].Rows[0]["Owner"].ToString() != string.Empty)
            {
                strowner = dsDetailsP.Tables[0].Rows[0]["Owner"].ToString();
            }
            if (dsDetailsP.Tables[0].Rows[0]["team"].ToString() != string.Empty)
            {
                strteam = dsDetailsP.Tables[0].Rows[0]["team"].ToString();
            }
            
        }
        if (string.IsNullOrWhiteSpace(milestonedetails.Owner))
        {
            milestonedetails.Owner = strowner;
        }
        if (string.IsNullOrWhiteSpace(milestonedetails.team))
        {
            milestonedetails.team = strteam;
        }
        string editid = milestonedetails.id;
        string entrytype = "Insert";
        if (editid != "" && editid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[22];
        prm[0] = new SqlParameter("@id", editid);
        prm[1]= new SqlParameter("@ProjectId", milestonedetails.ProjectId);
        prm[2] = new SqlParameter("@Milestone", milestonedetails.Milestone);
        prm[3] = new SqlParameter("@Weightage", milestonedetails.Weightage);
        if (milestonedetails.StartDate == string.Empty)
        {
            prm[4] = new SqlParameter("@StartDate", DBNull.Value);
        }
        else
        {
            prm[4] = new SqlParameter("@StartDate", cmf.dateConvert(milestonedetails.StartDate));
        }
        if (milestonedetails.EndDate == string.Empty)
        {
            prm[5] = new SqlParameter("@EndDate", DBNull.Value);
        }
        else
        {
            prm[5] = new SqlParameter("@EndDate", cmf.dateConvert(milestonedetails.EndDate));
        }
        prm[6] = new SqlParameter("@Owner", milestonedetails.Owner);
        prm[7] = new SqlParameter("@Access", milestonedetails.Access);

        prm[8] = new SqlParameter("@team", milestonedetails.team);
        prm[9] = new SqlParameter("@P_Status", milestonedetails.P_Status);
        prm[10] = new SqlParameter("@Priority", milestonedetails.Priority);
        if(milestonedetails.Budget==string.Empty)
        {
            prm[11] = new SqlParameter("@Budget", DBNull.Value);
        }
        else
        {
            prm[11] = new SqlParameter("@Budget", milestonedetails.Budget);
        }

        if(milestonedetails.Actual==string.Empty)
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
    public static string SaveMilestone_Owner(milestonetdata milestonedetails)
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string editid = milestonedetails.id;
        string entrytype = "Update";
        if (editid != "" && editid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", editid);
        prm[1] = new SqlParameter("@Owner", milestonedetails.Owner);
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
    public static string SaveProject_Team(milestonetdata milestonedetails)
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string editid = milestonedetails.id;
        string entrytype = "Update";
        if (editid != "" && editid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", editid);
        prm[1] = new SqlParameter("@team", milestonedetails.team);
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
    public static string BindpMilestonedetail_Owner(int RecordId = 0)
    {

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "select * from Pms_tbl_AddMilestone where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
        ds = da.GetDataSetInline(strsql);


        return ds.GetXml();
    }


    [WebMethod]
    public static void SaveTimer(string ProjectId = "", string Milestoneid = "", string TimerNotes = "", string hour = "", string minute = "", string second = "", string millisecond = "", string Transactionid = "")
    {
        DataAccess da1 = new DataAccess();
        string entrytype = "update";
        SqlParameter[] prm = new SqlParameter[13];
        prm[0] = new SqlParameter("@ProjectId", ProjectId);
        prm[1] = new SqlParameter("@Milestoneid", Milestoneid);
        prm[2] = new SqlParameter("@hour", hour);
        prm[3] = new SqlParameter("@minute", minute);
        prm[4] = new SqlParameter("@second", second);
        prm[5] = new SqlParameter("@millisecond", millisecond);
        prm[6] = new SqlParameter("@id", Transactionid);
        prm[7] = new SqlParameter("@EndTime", da1.getcdatetime());
        prm[8] = new SqlParameter("@TimerNotes", TimerNotes);
        prm[9] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[10] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[11] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[12] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pmstbl_Milestone_TimerDetailsSaveUpdate", prm);


        string strstatusupdate = "update Pms_tbl_AddMilestone set statuscomplete=3 where id='" + Milestoneid + "' and status=1 and archive=0";
        DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

    }

    [WebMethod]
    public static string SaveTimerstartdetails(string ProjectId = "", string Milestoneid = "")
    {
        DataAccess da1 = new DataAccess();
        string entrytype = "Insert";
        SqlParameter[] prm = new SqlParameter[8];
        prm[0] = new SqlParameter("@ProjectId", ProjectId);
        prm[1] = new SqlParameter("@Milestoneid", Milestoneid);
        prm[2] = new SqlParameter("@StartTime", da1.getcdatetime());
        prm[3] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[4] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[5] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[6] = new SqlParameter("@type", entrytype);
        prm[7] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[7].Direction = ParameterDirection.ReturnValue;
        int retid = da1.executeDMLQuery("pmstbl_Milestone_StartTimerDetailsSave", prm);

        string strstatusupdate = "update Pms_tbl_AddMilestone set statuscomplete=1 where id='" + Milestoneid + "' and status=1 and archive=0";
        DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

        return retid.ToString();





    }


    [WebMethod]
    public static void SaveTimerpausedetails(string ProjectId = "", string Milestoneid = "", string transactionid = "", string Notes = "")
    {
        DataAccess da1 = new DataAccess();
        string entrytype = "Insert";
        SqlParameter[] prm = new SqlParameter[9];
        prm[0] = new SqlParameter("@ProjectId", ProjectId);
        prm[1] = new SqlParameter("@Milestoneid", Milestoneid);
        prm[2] = new SqlParameter("@Transactionid", transactionid);
        prm[3] = new SqlParameter("@Notes", Notes);
        prm[4] = new SqlParameter("@PauseDatetime", da1.getcdatetime());
        prm[5] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[6] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[7] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[8] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pmstbl_Milestone_PauseDetailsSave", prm);

        string strstatusupdate = "update Pms_tbl_AddMilestone set statuscomplete=2 where id='" + Milestoneid + "' and status=1 and archive=0";
        DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);
    }

    [WebMethod]
    public static void SaveTimerresumedetails(string ProjectId = "", string Milestoneid = "", string transactionid = "")
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
        string strhourminuts = "select *  from Pms_tbl_Timerstatus where Milestoneid='" + Milestoneid + "' and ProjectId='" + ProjectId + "'  and type='milestone' and status=1 and archive=0 order by id desc";
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
                string strpauseddetails = "select *  from Pms_Tbl_PauseStatus where Transactionid='" + transactionid + "' and type='milestone' and status=1 and archive=0 order by id desc";
                DataSet dspaused = da.GetDataSetInline(strpauseddetails);

                if (dspaused.Tables[0].Rows.Count > 0)
                {
                    TimeSpan toatlhr = (DateTime.Now - Convert.ToDateTime(dspaused.Tables[0].Rows[0]["PauseDatetime"].ToString()));
                    hour = toatlhr.Hours.ToString("00");
                    min = toatlhr.Minutes.ToString("00");
                    sec = toatlhr.Seconds.ToString("00");
                    mili = toatlhr.Milliseconds.ToString("000");
                    string totalmin = (toatlhr.Hours * 60 + toatlhr.Minutes + toatlhr.Seconds / 60).ToString();
                    string strresumedetails = "update Pms_Tbl_PauseStatus set StartDatetime='" + DateTime.Now.ToString() + "' ,PausedTime='" + totalmin + "',StartByid='" + HttpContext.Current.Request.Cookies["user"]["user_id"].ToString() + "' where Transactionid='" + transactionid + "' and id='" + dspaused.Tables[0].Rows[0]["id"].ToString() + "' and status=1 and archive=0;" +
                        "update  Pms_tbl_Timerstatus set IsPaused=0 where id='" + transactionid + "' and type='milestone'";
                    da.executeDMLQueryinline(strresumedetails);
                }


            }


        }



    }




    [WebMethod]
    public static void UpdateMilestone_Dependency(string milestoneid = "", string dependencyid = "")
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (milestoneid != "" && milestoneid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", milestoneid);
        prm[1] = new SqlParameter("@dependancy", dependencyid);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pms_tbl_Dependancy_Milestone_Update", prm);



        //dependancy start //

        string cntdepndcy = "0";
        string depdecydetails = "";
        string depcytooltip = "";

        DataAccess da = new DataAccess();
        string strdepcysql = "Select SUBSTRING((SELECT ',' + Cast(id as varchar) FROM Pms_tbl_Addtask where Pms_tbl_Addtask.Milestoneid = '" + milestoneid + "' and status=1 and archive=0  and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  FOR XML PATH('')),2,9999) As taskid  ";
        DataSet dsdepcy = da.GetDataSetInline(strdepcysql);
        if (dsdepcy.Tables[0].Rows[0]["taskid"].ToString() != "")
        {
            string[] arcntdep = dsdepcy.Tables[0].Rows[0]["taskid"].ToString().Trim().Split(',');
            cntdepndcy = arcntdep.Length.ToString();
            DataSet dsdtlsdepcy = new DataSet();
            if (dsdepcy.Tables[0].Rows[0]["taskid"].ToString().Trim() != "")
            {
                dsdtlsdepcy = da.GetDataSetInline("select id,taskname from Pms_tbl_Addtask where id in(" + dsdepcy.Tables[0].Rows[0]["taskid"].ToString().Trim() + ")");
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

            string cntdep = "";
            if (cntdepndcy != "" && cntdepndcy != "0")
            {
                cntdep = "+" + cntdepndcy;

            }
            retval += depdecydetails;
            retval += "<span class='cursor-pointer' data-bs-toggle='tooltip' data-bs-html='true' data-bs-placement='top' data-bs-trigger='hover' title='" + depcytooltip + "'>" + cntdep + "</span>";
            retval += "</div>";


        }


        // dependancy end//


        //  return retval;


    }

    [WebMethod]
    public static string Bindmilestone_Dependancy(int RecordId = 0)
    {

        DataAccess da = new DataAccess();
        DataSet ds;

       // string strsql = "select * from Pms_tbl_Addtask where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and Milestoneid='" + RecordId + "'";

        string strsql = "Select SUBSTRING((SELECT ',' + Cast(id as varchar) FROM Pms_tbl_Addtask where Pms_tbl_Addtask.Milestoneid = '" + RecordId + "' and status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  FOR XML PATH('')),2,9999) As taskid     ";

        ds = da.GetDataSetInline(strsql);
        return ds.GetXml();
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
    public static string Bindcheckstrtdateprojectdetails(int projec_id = 0)
    {

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "select * from Pms_tbl_AddProject where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + projec_id + "'";
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

    //public static string UpdateMilestone_Status(string milstoneid = "", string statusid = "", string iscomplete = "")
    //{
    //    string retval = "";
    //    DataAccess da1 = new DataAccess();
    //    string entrytype = "Update";
    //    if (milstoneid != "" && milstoneid != "0")
    //    {
    //        entrytype = "Update";
    //    }

    //    SqlParameter[] prm = new SqlParameter[8];
    //    prm[0] = new SqlParameter("@id", milstoneid);
    //    prm[1] = new SqlParameter("@milstoneStatus", statusid);
    //    prm[2] = new SqlParameter("@iscomplete", iscomplete);
    //    prm[3] = new SqlParameter("@iscompletedate", da1.getcdatetime());
    //    prm[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
    //    prm[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
    //    prm[6] = new SqlParameter("@cdate", da1.getcdatetime());
    //    prm[7] = new SqlParameter("@type", entrytype);
    //    da1.executeDMLQuery("pmstbl_Status_milstone_Update_usp", prm);



    //    // percentage update start  // 
    //    if (iscomplete == "1")
    //    {
    //        decimal milestonetotalpercent = 100;
    //        decimal totlweitagemile = 0;
    //        string finsql1 = "select * from Pms_tbl_AddMilestone where id=" + milstoneid + " and status=1  and archive=0";
    //        DataSet dsfindid1 = da1.GetDataSetInline(finsql1);

    //        // Milestone percentage //
    //        string strstatusupdate = "update Pms_tbl_AddMilestone set iscompletepercentage='" + milestonetotalpercent.ToString() + "' where id='" + milstoneid + "' and status=1 and archive=0 and isnull(iscomplete,0)=1";
    //        DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

    //        // project percentage //
    //        string finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1  and archive=0";
    //        DataSet dsfindid2 = da1.GetDataSetInline(finsql2);
    //        for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
    //        {
    //            decimal sumweitage = 0;
    //            decimal subpercentageemile = 0;
    //            sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
    //            subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
    //            totlweitagemile += (sumweitage * subpercentageemile) / 100;

    //        }

    //        string finsql4 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitagemile.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
    //        DataSet dsfindid4 = da1.GetDataSetInline(finsql4);






    //    }
    //    else
    //    {

    //        decimal milestonetotalpercent = 0;
    //        decimal totlweitagemile = 0;
    //        string finsql1 = "select * from Pms_tbl_AddMilestone where id=" + milstoneid + " and status=1  and archive=0";
    //        DataSet dsfindid1 = da1.GetDataSetInline(finsql1);

    //        // Milestone percentage //
    //        string strstatusupdate = "update Pms_tbl_AddMilestone set iscompletepercentage='" + milestonetotalpercent.ToString() + "' where id='" + milstoneid + "' and status=1 and archive=0 and isnull(iscomplete,0)=0";
    //        DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

    //        // project percentage //
    //        string finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1  and archive=0";
    //        DataSet dsfindid2 = da1.GetDataSetInline(finsql2);
    //        for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
    //        {
    //            decimal sumweitage = 0;
    //            decimal subpercentageemile = 0;
    //            sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
    //            subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
    //            totlweitagemile += (sumweitage * subpercentageemile) / 100;

    //        }

    //        string finsql4 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitagemile.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
    //        DataSet dsfindid4 = da1.GetDataSetInline(finsql4);



    //    }
    //    // percentage update end  // 




    //    //project  log details insert start
    //    SqlParameter[] prmlog = new SqlParameter[8];
    //    prmlog[0] = new SqlParameter("@Transid", milstoneid);
    //    prmlog[1] = new SqlParameter("@Logstatus", statusid);
    //    prmlog[2] = new SqlParameter("@LogDate", da1.getcdatetime());
    //    prmlog[3] = new SqlParameter("@Logtype", "milestone");
    //    prmlog[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
    //    prmlog[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
    //    prmlog[6] = new SqlParameter("@cdate", da1.getcdatetime());
    //    prmlog[7] = new SqlParameter("@type", "Insert");
    //    da1.executeDMLQuery("pmstbl_Loginsert_usp", prmlog);
    //    //project  log details insert end

    //    DataAccess da = new DataAccess();
    //    string strsql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus where  status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + statusid + "' ";
    //    DataSet dscode = da.GetDataSetInline(strsql);
    //    if (dscode.Tables[0].Rows.Count > 0)
    //    {
    //        retval = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
    //    }
    //    // return dscode.GetXml();
    //    return retval;
    //}

    [WebMethod]
    public static string UpdatePriority_Status(string milstoneid = "", string Propertyid = "")
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
    public static string AddAddMilestonetatus(string value = "", string statuscolor = "")
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
    public static string AddMilestonePrioritytype(string value = "", string prioritycolor = "")
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
    public static string UpdateExtendeddatedtls(string Milestoneid = "", string extendStrtdate = "", string extendEnddate = "")
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
        prm[1] = new SqlParameter("@StartDate", extendStrtdate);
        prm[2] = new SqlParameter("@TargetDate", extendEnddate);
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
    public static string bindMiletimelinedata(string projec_id = "0")
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
    public static string bindMilestonedata(string RecordId = "0")
    {
        string retval = get_status_listbind();
        string strvalprority = get_Priority_listbind();
        string Milestonedata = "";
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
            SqlParameter[] prm = new SqlParameter[2];
            prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[1] = new SqlParameter("@RecordId", RecordId);
            ds = da.GetDataSet("Pms_Bindmilestone_dashbord", prm);
        }

        else
        {
            SqlParameter[] prm = new SqlParameter[3];
            prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[1] = new SqlParameter("@RecordId", RecordId);
            prm[2] = new SqlParameter("@owner", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            ds = da.GetDataSet("Pms_Bindmilestone_dashbord_ownerwise", prm);
        }

        int istaskchk = 0;
        string sqlchkistsk = "select isnull(istask,0) istask from Pms_tbl_AddMilestone where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and isnull(istask,0)=1  ";

        if(RecordId!="0" && RecordId!="")
        {
              sqlchkistsk = "select isnull(istask,0) istask from Pms_tbl_AddMilestone where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and ProjectId='" + RecordId + "' and isnull(istask,0)=1  ";

        }

        DataSet dschkistsk = da.GetDataSetInline(sqlchkistsk);
        if (dschkistsk.Tables[0].Rows.Count > 0)
        {
            istaskchk = 1;

        }


        Milestonedata += "<table style='width:100%;' cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm datatable'>";
        Milestonedata += "<thead>";
        Milestonedata += "<tr>";
        
        Milestonedata += "<th>S.No</th>";
        Milestonedata += "<th>Action</th>";
      
        Milestonedata += "<th style=min-width:200px;>Project</th>";
        Milestonedata += "<th style=min-width:200px;>Milestone</th>";

        Milestonedata += "<th>Timer</th>";
        Milestonedata += "<th>Timer History</th>";

        Milestonedata += "<th style=min-width:150px;>Completion Status</th>";
        Milestonedata += "<th>Weightage</th>";
        if (istaskchk == 1)
        {
            Milestonedata += "<th style=min-width:140px;>Task</th>";
        }
        Milestonedata += "<th style=min-width:120px;>Owner</th>";
        Milestonedata += "<th style=min-width:120px;>Team</th>";
        Milestonedata += "<th style=min-width:140px;>Timeline</th>";
        //Milestonedata += "<th style=min-width:140px;>Task Timeline</th>";
        Milestonedata += "<th style=min-width:140px;>Extended timeline</th>";
        Milestonedata += "<th>Start Date</th>";
        Milestonedata += "<th>End Date</th>";
        Milestonedata += "<th style=width:130px;>Status</th>";
        Milestonedata += "<th>Priority</th>";
        Milestonedata += "<th style=min-width:140px;>Issue</th>";
        Milestonedata += "<th>File</th>";
        //Milestonedata += "<th>Budget</th>";
        //Milestonedata += "<th>Actual</th>";
      //Milestonedata += "<th style=min-width:140px;>Dependency</th>";
     
        Milestonedata += "</tr>";
        Milestonedata += "</thead>";
        Milestonedata += "<tbody>";

       
        string project_name = "";
      


        if (ds.Tables[0].Rows.Count > 0)
        {
            string strvalidationM = get_EditDelete_Validation("54");
            string strvalidationT = get_EditDelete_Validation("55");
            string strvalidationI = get_EditDelete_Validation("57");
            string strvalidationD = get_EditDelete_Validation("58");

            string streditM = "0";
            string strdeleteM = "0";
            string strownerM = "0";
            string strviewT = "0";
            string strviewI = "0";
            string strviewD = "0";
            try
            {
                string[] arrM = strvalidationM.Split(',');
                streditM = arrM[3];
                strdeleteM = arrM[4];
                strownerM = arrM[5];

            }
            catch { }           
            try
            {
                string[] arrT = strvalidationT.Split(',');
                strviewT = arrT[0];

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
                                    usersdetails += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title=" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + " data-bs-original-title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "'><img src = '/assets/images/avatar-1.jpg' alt='' class='rounded-circle avatar-xxs'></a>";
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
                                    usersdetails += @"<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='' data-bs-original-title='" + dsusers.Tables[0].Rows[k]["DisplayName"].ToString() + "'><img src = '/assets/images/avatar-1.jpg' alt='' class='rounded-circle avatar-xxs'></a>";
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
                
                string strdepcysql = "Select SUBSTRING((SELECT ',' + Cast(id as varchar) FROM Pms_tbl_Addtask where Pms_tbl_Addtask.Milestoneid = '" + ds.Tables[0].Rows[i]["id"] + "' and status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  FOR XML PATH('')),2,9999) As taskid  ";
                DataSet dsdepcy = da.GetDataSetInline(strdepcysql);
                if (dsdepcy.Tables[0].Rows[0]["taskid"].ToString() != "")
                {
                    string[] arcntdep = dsdepcy.Tables[0].Rows[0]["taskid"].ToString().Trim().Split(',');
                    cntdepndcy = arcntdep.Length.ToString();
                    DataSet dsdtlsdepcy = new DataSet();
                    if (dsdepcy.Tables[0].Rows[0]["taskid"].ToString().Trim() != "")
                    {
                        dsdtlsdepcy = da.GetDataSetInline("select id,taskname from Pms_tbl_Addtask where id in(" + dsdepcy.Tables[0].Rows[0]["taskid"].ToString().Trim() + ")");
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
                                  
                                    depdecydetails += "<span class='badge bg-primary'>"+ dsdtlsdepcy.Tables[0].Rows[k]["taskname"].ToString() + "</span>";
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





                //   completion status start //
                
                decimal Wattagecomplite = 0;
                decimal Wattagepending = 100;
                decimal projectTotal = 0;
                decimal PhaseTotal = 0;
                decimal TaskTotal = 0;
               
                
                   string findmile = "select cast(ROUND(isnull(iscompletepercentage,0),2) AS DECIMAL(10,2)) as iscompletepercentage from Pms_tbl_AddMilestone  where ProjectId='" + ds.Tables[0].Rows[i]["ProjectId"] + "' and id='" + ds.Tables[0].Rows[i]["id"] + "'  and status=1  and archive=0";
                   DataSet dsssummile2 = da.GetDataSetInline(findmile);

                    try
                    {

                        TaskTotal = Convert.ToDecimal(dsssummile2.Tables[0].Rows[0]["iscompletepercentage"].ToString());
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



                //// completion status end //



                //   task count start //

                float tskcomplite = 0;
                float tskpending = 0;
                float tskprojectTotal = 0;
                float tskPhaseTotal = 0;
                float tsk_TaskTotal = 0;

                string str11 = "select COUNT(*) as counttsk from Pms_tbl_Addtask where ProjectId='" + ds.Tables[0].Rows[i]["ProjectId"] + "' and Milestoneid='" + ds.Tables[0].Rows[i]["id"] + "' and status=1 and archive=0 ";
                DataSet dss11 = da.GetDataSetInline(str11);
                if (dss11.Tables[0].Rows.Count > 0)
                {

                    try
                    {
                        tskPhaseTotal = Convert.ToInt32(dss11.Tables[0].Rows[0]["counttsk"]);
                    }
                    catch (Exception)
                    {
                    }
                }

                string str22 = "select * from Pms_tbl_AddMilestone where ProjectId='" + ds.Tables[0].Rows[i]["ProjectId"] + "' and id='" + ds.Tables[0].Rows[i]["id"] + "' and status=1 and archive=0 ";
                DataSet dss22 = da.GetDataSetInline(str22);
                if (dss22.Tables[0].Rows.Count > 0)
                {

                    string findtask11 = "select COUNT(*) as counttask  from Pms_tbl_Addtask where ProjectId='" + ds.Tables[0].Rows[i]["ProjectId"] + "' and  Milestoneid='" + dss22.Tables[0].Rows[0]["id"] + "' and P_Status in ( select id from Pms_tbl_Projectstatus where  status=1 and archive=0 and isnull(iscomplete,0)=1)  and status=1 and archive=0";
                    DataSet dsssumtask11 = da.GetDataSetInline(findtask11);

                    try
                    {
                        tsk_TaskTotal = Convert.ToInt32(dsssumtask11.Tables[0].Rows[0]["counttask"]);
                    }
                    catch (Exception)
                    {

                    }


                    tskprojectTotal = (tskPhaseTotal - tsk_TaskTotal);


                }

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

                // task count end //




                //   Issues count start //

                float Issuescomplite = 0;
                float Issuespending = 0;
                float IssuesrojectTotal = 0;
                float IssuesPhaseTotal = 0;
                float Issues_Total = 0;

                string str111 = "select COUNT(*) as countIssues from Pms_tbl_Addissues where ProjectId='" + ds.Tables[0].Rows[i]["ProjectId"] + "' and Milestoneid='" + ds.Tables[0].Rows[i]["id"] + "' and Issueon='milstone' and status=1 and archive=0 ";
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


                string findtask111 = "select COUNT(*) as count_Issues from Pms_tbl_Addissues where ProjectId='" + ds.Tables[0].Rows[i]["ProjectId"] + "' and Milestoneid='" + ds.Tables[0].Rows[i]["id"] + "' and Issueon='milstone' and IssueStatus in ( select id from Pms_tbl_Projectstatus where  status=1 and archive=0 and isnull(iscomplete,0)=1)  and status=1 and archive=0";
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



                Milestonedata += "<tr>";
                Milestonedata += "<td valign='top'>";
                Milestonedata += (i + 1);
                Milestonedata += "</td>";


                Milestonedata += " <td><a href='javascript:void(0);' style='display:" + (streditM == "1" ? "" : "none") + "' Class='btn btn-info btn-sm' type='button' onclick='Binddetails(" + ds.Tables[0].Rows[i]["id"].ToString() + ")' data-bs-toggle='offcanvas' data-bs-target='#addmilestone'><i class='ri-edit-line'></i></a>";
                Milestonedata += "&nbsp;&nbsp;<a href='javascript:void(0);' style='display:" + (strdeleteM == "1" ? "" : "none") + "' Class='btn btn-danger btn-sm' type='button' onclick='deletedata(" + ds.Tables[0].Rows[i]["id"].ToString() + ")'><i class='ri-delete-bin-5-line'></i></a>";
                Milestonedata += "</td>";

                
                

                Milestonedata += "<td valign='top'>";
                if (ds.Tables[0].Rows[i]["ProjectId"].ToString() == project_name)
                {
                    Milestonedata += "";
                }
                else
                {
                    Milestonedata += get_ProjectName(ds.Tables[0].Rows[i]["ProjectId"].ToString());

                }
                
                Milestonedata += "</td>";

                Milestonedata += "<td valign='top'>";
                //Milestonedata += ds.Tables[0].Rows[i]["Milestone"].ToString();

                Milestonedata += "<div class='d-flex'><div class='flex-grow-1'>";
                Milestonedata += "<a href = 'javascript:void(0);' type='button' data-bs-toggle='offcanvas' data-bs-target='#canvas_popup_multi' >" + ds.Tables[0].Rows[i]["Milestone"].ToString() + "</a> ";
                Milestonedata += "</div>";

                //Milestonedata += "<div class='gap-1 flex-shrink-0 align-self-center'><a href='javascript:void(0);' type='button' data-bs-toggle='offcanvas' data-bs-target='#canvas_popup' onclick='loadsrc(\"inner_popup\",\"/pms/AddSTDependency.aspx?id=" + ds.Tables[0].Rows[i]["id"].ToString() + "&mprojectid=" + ds.Tables[0].Rows[i]["id"].ToString() + "&type=project\")'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Dependency' style='font-size:12px;height:22px;width:22px;margin-left:9px'>D</span></a></div>";
                Milestonedata += "<div class='gap-1 flex-shrink-0 align-self-center' style='display:" + (strviewD == "1" ? "" : "none") + "'><a href ='javascript:void(0);' type='button' id='btnD_" + ds.Tables[0].Rows[i]["id"].ToString() + "' onclick='Binddetails_D(" + ds.Tables[0].Rows[i]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#dependencyD'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='Dependency' style='font-size:12px;height:22px;width:22px;margin-left:9px'>D</span></a></div>";
                // mange issues
                int chkissues = CheckIssuesdata(ds.Tables[0].Rows[i]["id"].ToString(), "milestone");
                if (chkissues > 0)
                {
                    Milestonedata += "<div class='gap-1 flex-shrink-0 align-self-center' style='display:" + (strviewI== "1" ? "" : "none") + "'><a href ='javascript:void(0);' type='button' id='btnI_" + ds.Tables[0].Rows[i]["id"].ToString() + "' onclick='loadsrcdetails(\"issuesdetails\", \"/pms/userissuesdetails.aspx?selectid=" + ds.Tables[0].Rows[i]["id"].ToString() + "&selecttype=milestone\",\"View milestone " + ds.Tables[0].Rows[i]["Milestone"].ToString() + " issues details\")' data-bs-toggle='modal' data-bs-target='#viewissues'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='Issues' style='font-size:12px;height:22px;width:22px;margin-left:9px'>I</span></a></div>";
                }

                Milestonedata += "</div>";

                Milestonedata += "</td>";


                // timer 

                //check statuscomplet //
                int isstatuscomplete = 0;
                string strchsttscomplt = " select isnull(iscomplete,0) as iscomplete from Pms_tbl_Projectstatus where status = 1 and archive = 0  and id in (select P_Status from Pms_tbl_AddMilestone where ProjectId = '" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "' and id = '" + ds.Tables[0].Rows[i]["id"].ToString() + "' and status = 1 and archive = 0) and isnull(iscomplete,0)= 1  and status = 1 and archive = 0 ";
                DataSet dschksttscomplt = da.GetDataSetInline(strchsttscomplt);
                if (dschksttscomplt.Tables[0].Rows.Count > 0)
                {


                    isstatuscomplete = Convert.ToInt32(dschksttscomplt.Tables[0].Rows[0]["iscomplete"].ToString());
                }

                //check istask start //

                int checkistask = 0;

                string strchtask = " select isnull(istask,0) istask,isnull(statuscomplete,0)statuscomplete from  Pms_tbl_AddMilestone where id='" + ds.Tables[0].Rows[i]["id"].ToString() + "' ";
                DataSet dschktask = da.GetDataSetInline(strchtask);
                if (dschktask.Tables[0].Rows.Count > 0)
                {

                    checkistask = Convert.ToInt32(dschktask.Tables[0].Rows[0]["istask"].ToString());

                }

                // check istask end //
                
                if (isstatuscomplete != 1)
                {
                    //SaveLogText("isstatuscomplete-" + isstatuscomplete);
                    Milestonedata += "<td align ='center'>";


                    string hour = "";
                    string min = "";
                    string sec = "";
                    string mili = "";
                    string pausetime = "";
                    int ispause = 0;
                    int isrun = 0;
                    int isend = 0;
                    string transactionid = "0";
                    string strhourminuts = "select *  from Pms_tbl_Timerstatus where isnull(isend,0)=0 and Milestoneid='" + ds.Tables[0].Rows[i]["id"].ToString() + "' and ProjectId='" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "' and type='milestone' and status=1 and archive=0 order by id desc";
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
                            string strpauseddetails = "select *  from Pms_Tbl_PauseStatus where Transactionid='" + transactionid + "' and isnull(StartByid,0)=0 and status=1 and archive=0 and type='milestone' order by id desc";
                            DataSet dspaused = da.GetDataSetInline(strpauseddetails);

                            if (dspaused.Tables[0].Rows.Count > 0)
                            {
                                string chkpausemin = "0";

                                string strpauseddetails2 = "select Transactionid, sum(cast(isnull(PausedTime,0) as numeric)) tpause  from Pms_Tbl_PauseStatus where Transactionid='" + transactionid + "'  and status=1 and archive=0 and type='milestone' and isnumeric(isnull(PausedTime,0))=1 group by Transactionid";
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
                            string strpauseddetails2 = "select Transactionid, sum(cast(isnull(PausedTime,0) as numeric)) tpause  from Pms_Tbl_PauseStatus where Transactionid='" + transactionid + "'  and status=1 and archive=0 and type='task' and isnumeric(isnull(PausedTime,0))=1 group by Transactionid";
                            DataSet dspaused2 = da.GetDataSetInline(strpauseddetails2);
                            if (dspaused2.Tables[0].Rows.Count > 0)
                            {
                                chkpausemin = (dspaused2.Tables[0].Rows[0]["tpause"].ToString() == "" ? "0" : dspaused2.Tables[0].Rows[0]["tpause"].ToString());
                            }
                            TimeSpan toatlhr = (DateTime.Now - Convert.ToDateTime(dshourmin.Tables[0].Rows[0]["StartTime"].ToString()));
                            TimeSpan t1 = TimeSpan.FromMinutes(Convert.ToInt32(chkpausemin));
                            //TimeSpan t1 = new TimeSpan(0,Convert.ToInt32(chkpausemin) ,0,0);
                            int diffdays = (toatlhr.Days) * 24;

                            //int sumhour = diffdays + toatlhr.Hours+ toatlhr.Minutes+ toatlhr.Minutes;
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


                    Milestonedata += "<div  onclick='hideshow(divs_" + ds.Tables[0].Rows[i]["id"].ToString() + ",dinv_" + ds.Tables[0].Rows[i]["id"].ToString() + ")'>";

                    Milestonedata += "<button type='button' onclick='start_" + ds.Tables[0].Rows[i]["id"].ToString() + "(hour_" + ds.Tables[0].Rows[i]["id"].ToString() + ",minute_" + ds.Tables[0].Rows[i]["id"].ToString() + ",second_" + ds.Tables[0].Rows[i]["id"].ToString() + ",millisecond_" + ds.Tables[0].Rows[i]["id"].ToString() + "," + ds.Tables[0].Rows[i]["id"].ToString() + ");' id='btnauto_" + ds.Tables[0].Rows[i]["id"].ToString() + "' name='start' style='display:none;'><i class='bi bi-play-circle-fill text-secondary' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='Resume Timer'></i></button>";
                    //taskdata += "<a href = 'javascript: void(0);' name='start' id='btnauto_" + ds.Tables[0].Rows[i]["id"].ToString() + "' style='display:none' onclick='start_" + ds.Tables[0].Rows[i]["id"].ToString() + "(hour_" + ds.Tables[0].Rows[i]["id"].ToString() + ",minute_" + ds.Tables[0].Rows[i]["id"].ToString() + ",second_" + ds.Tables[0].Rows[i]["id"].ToString() + ",millisecond_" + ds.Tables[0].Rows[i]["id"].ToString() + "," + ds.Tables[0].Rows[i]["id"].ToString() + ")'>strt</a>";


                    if (checkistask != 1)
                    {


                        Milestonedata += "<button class='start_timer' id='divs_" + ds.Tables[0].Rows[i]["id"].ToString() + "'  type='button' name='start' style='background:none;border:0px;display:" + (showrun == 0 ? "block" : "none") + "'><i class='ri-timer-line fs-3xl' onclick='savestartdetails(" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "," + ds.Tables[0].Rows[i]["id"].ToString() +
                            "),start_" + ds.Tables[0].Rows[i]["id"].ToString() + "(hour_" + ds.Tables[0].Rows[i]["id"].ToString() + ",minute_" + ds.Tables[0].Rows[i]["id"].ToString() + ",second_" + ds.Tables[0].Rows[i]["id"].ToString() + ",millisecond_" + ds.Tables[0].Rows[i]["id"].ToString() + "," + ds.Tables[0].Rows[i]["id"].ToString() + ")' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='Start Timer'></i></button>";


                    }

                    Milestonedata += "<div class='timer_wrapper' style='display:" + (showrun == 1 ? "block" : "none") + ";' id='dinv_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>";
                    Milestonedata += "<button class='stop_timer' style='background:none;border:0px;'><i class='ri-timer-line fs-3xl' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='Stop Timer'></i></button>";
                    Milestonedata += "<div class='timer-content'>";
                    Milestonedata += "<button type ='button' name='stop'><i class='bi bi-stop-circle-fill text-danger' data-bs-toggle='modal' data-bs-target='#timer_stop' onclick='Binddetails_Timer(" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "," + ds.Tables[0].Rows[i]["id"].ToString() + ",hour_" + ds.Tables[0].Rows[i]["id"].ToString() + ",minute_" + ds.Tables[0].Rows[i]["id"].ToString() + ",second_" + ds.Tables[0].Rows[i]["id"].ToString() + ",millisecond_" + ds.Tables[0].Rows[i]["id"].ToString() + "," + transactionid + "," + varid + ")'  data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='Stop Timer'></i></button> <button type='button' onclick='saveResumeDetails(" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "," + ds.Tables[0].Rows[i]["id"].ToString() + "," + transactionid + "),start_" + ds.Tables[0].Rows[i]["id"].ToString() + "(hour_" + ds.Tables[0].Rows[i]["id"].ToString() + ",minute_" + ds.Tables[0].Rows[i]["id"].ToString() + ",second_" + ds.Tables[0].Rows[i]["id"].ToString() + ",millisecond_" + ds.Tables[0].Rows[i]["id"].ToString() + "," + ds.Tables[0].Rows[i]["id"].ToString() + "),hideshowResumebutton(btnpause_" + ds.Tables[0].Rows[i]["id"].ToString() + ",btnresum_" + ds.Tables[0].Rows[i]["id"].ToString() + ")' id='btnresum_" + ds.Tables[0].Rows[i]["id"].ToString() + "' name='start' style='display:" + (ispause == 1 ? "inline-block" : "none") + ";'><i class='bi bi-play-circle-fill text-secondary' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='Resume Timer'></i></button> <button type ='button' style='display:" + (ispause == 0 ? "inline-block" : "none") + ";' id='btnpause_" + ds.Tables[0].Rows[i]["id"].ToString() + "' name='pause' class='me-2'><i class='bi bi-pause-circle-fill text-warning' data-bs-toggle='modal' data-bs-target='#addnotes' onclick='BindPausedetails_Notes(\"" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "\",\"" + ds.Tables[0].Rows[i]["id"].ToString() + "\",\"" + transactionid + "\",\"" + varid + "\")'  data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='Pause Timer'></i></button>";
                    Milestonedata += "<input type='hidden' id='hdntrans_" + ds.Tables[0].Rows[i]["id"].ToString() + "'  value='" + transactionid + "' /><span id='hour_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>" + hour + "</span>:<span id='minute_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>" + min + "</span>:<span id='second_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>" + sec + "</span>:<span id='millisecond_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>000</span>";
                    Milestonedata += "</div>";
                    Milestonedata += "</div>";
                    Milestonedata += "</div>";

                    Milestonedata += "</td>";
                }

                if (isstatuscomplete == 1)
                {

                    Milestonedata += "<td valign='top' align ='center'>";
                    Milestonedata += ""; //timer status 
                    Milestonedata += "</td>";

                }


                // Timer History 

                string statushour = "";
                string statusmin = "";
                string statussec = "";
                string strchectsktskst = "select top (1) Pms_tbl_Timerstatus.hour,Pms_tbl_Timerstatus.minute,Pms_tbl_Timerstatus.second,Pms_tbl_Timerstatus.millisecond    from Pms_tbl_Timerstatus  where Milestoneid = " + ds.Tables[0].Rows[i]["id"].ToString() + " and Pms_tbl_Timerstatus.type = 'milestone' and Pms_tbl_Timerstatus.status = 1 and Pms_tbl_Timerstatus.archive = 0  order by id desc";
                DataSet dschkstts = da.GetDataSetInline(strchectsktskst);
                if (dschkstts.Tables[0].Rows.Count > 0)
                {

                    statushour = dschkstts.Tables[0].Rows[0]["hour"].ToString();
                    statusmin = dschkstts.Tables[0].Rows[0]["minute"].ToString();
                    statussec = dschkstts.Tables[0].Rows[0]["second"].ToString();
                    Milestonedata += "<td valign='top'>";
                    Milestonedata += "Hour : " + statushour + ", Min: " + statusmin + ", Sec: " + statussec;
                    Milestonedata += "</td>";
                }

                else
                {
                    Milestonedata += "<td valign='top'>";
                    Milestonedata += ""; // Timer History 
                    Milestonedata += "</td>";
                }



                //Completion Status

                Milestonedata += "<td>";
                if (Wattagecomplite == 0)
                {

                    Milestonedata += "<div class='progress progress-xl cursor-pointer'>";
                    Milestonedata += "<div class='progress progress-xl bg-light position-relative' data-bs-toggle='tooltip' data-bs-placement='top' title='Complete(0%)' role='progressbar' style='width:5%'><span style='text-align:center;'>0</span>";
                    Milestonedata += "</div>";
                    Milestonedata += "</div>";

                }

                else
                {

                    Milestonedata += "<div class='progress progress-xl cursor-pointer'>";
                    Milestonedata += "<div class='progress-bar bg-success' data-bs-toggle='tooltip' data-bs-placement='top' title='Complete (" + Wattagecomplite + "%)' role='progressbar' style='width:" + Wattagecomplite + "%'>" + Wattagecomplite + "%";
                    Milestonedata += "</div>";

                    Milestonedata += "<div class='progress-bar bg-warning' data-toggle='tooltip' data-bs-placemen='top' title='Pending (" + Wattagepending + "%)' role='progressbar' style='width:" + Wattagepending + "%'>" + Wattagepending + "%";
                    Milestonedata += "</div>";
                    Milestonedata += "</div>";



                }

                Milestonedata += "</td>";


                Milestonedata += "<td valign='top'>";
                Milestonedata += fns.ToFormattedCurrencyString(ds.Tables[0].Rows[i]["Weightage"].ToString()); // Weightage 
                Milestonedata += "</td>";


                // task
                if (istaskchk == 1)
                {
                    Milestonedata += "<td valign='top'>";
                    if (checkistask != 0)
                    {
                        float completetepercenttsk = 0;
                        float Pendingepercenttsk = 0;
                        completetepercenttsk = tskcomplite * 100 / tskPhaseTotal;
                        Pendingepercenttsk = tskpending * 100 / tskPhaseTotal;

                        Milestonedata += "<div class='d-flex'>";
                        Milestonedata += "<div class='flex-grow-1 align-self-center'>";

                        Milestonedata += "<div class='progress progress-xl cursor-pointer'>";
                        Milestonedata += "<div class='progress-bar bg-success' role='progressbar' style='width: " + fns.ToFormattedCurrencyString(completetepercenttsk.ToString()) + "%' aria-valuemin='0' aria-valuemax='100' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + fns.ToFormattedCurrencyString(completetepercenttsk.ToString()) + "% [" + tskcomplite + " of " + tskPhaseTotal + " Task Completed]'>" + tskcomplite + "</div>";

                        Milestonedata += "<div class='progress-bar bg-warning' role='progressbar' style='width: " + fns.ToFormattedCurrencyString(Pendingepercenttsk.ToString()) + "%' aria-valuemin='0' aria-valuemax='100' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + fns.ToFormattedCurrencyString(Pendingepercenttsk.ToString()) + "% [" + tskpending + " of " + tskPhaseTotal + " Task Not Completed]'>" + tskpending + "</div>";
                        Milestonedata += "</div>";
                        Milestonedata += "</div>";

                        Milestonedata += "<div class='gap-1 flex-shrink-0 align-self-center' style='display:" + (strviewT == "1" ? "" : "none") + "'>";
                        Milestonedata += "<a href ='javascript:void(0);' type='button' data-bs-toggle='offcanvas' data-bs-target=#canvas_popup onclick=loadsrc('inner_popup','/pms/AddTaskPopup.aspx?id=" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "&milstoneid=" + ds.Tables[0].Rows[i]["id"].ToString() + "&type=milstone')><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Task'>+</span></a>";
                        Milestonedata += "</div>";
                    }
                    Milestonedata += "</td>";
                }


                // owner //
                Milestonedata += "<td>";
                Milestonedata += "<div class='d-flex'>";
                Milestonedata += "<div class='flex-grow-1'>";
                Milestonedata += "<div class='avatar-group' id='lblowner_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>";
                Milestonedata += usersdetailsowner;

                //projectdata += "</a>";
                Milestonedata += "<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='" + ownertooltip + "' data-bs-original-title='more'>";

                if (cntownr != "" && cnt != "0")
                {
                    Milestonedata += "<div class='avatar-xxs'>";
                    Milestonedata += "<div class='avatar-title rounded-circle'>";

                    Milestonedata += (cntownr + "+");
                    Milestonedata += "</div>";
                    Milestonedata += "</div>";
                }

                Milestonedata += "</a>";
                Milestonedata += "</div>";
                Milestonedata += "</div>";
                Milestonedata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                Milestonedata += "<a href ='javascript:void(0);' style='display:" + (streditM == "1" ? "" : "none") + "' type='button' id='btnownr_" + ds.Tables[0].Rows[i]["id"].ToString() + "' onclick='Binddetails_Owner(" + ds.Tables[0].Rows[i]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#changeowner'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Owner'>+</span></a>";
                Milestonedata += "</div>";
                Milestonedata += "</div>";
                Milestonedata += "</td>";



                //team //

                Milestonedata += "<td>";
                Milestonedata += "<div class='d-flex'>";
                Milestonedata += "<div class='flex-grow-1'>";
                Milestonedata += "<div class='avatar-group' id='lbluser_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>";
                Milestonedata += usersdetails;

                //projectdata += "</a>";
                Milestonedata += "<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='" +teamtooltip + "' data-bs-original-title='more'>";

                if (cnt != "" && cnt != "0")
                {
                    Milestonedata += "<div class='avatar-xxs'>";
                    Milestonedata += "<div class='avatar-title rounded-circle'>";

                    Milestonedata += (cnt + "+");
                    Milestonedata += "</div>";
                    Milestonedata += "</div>";
                }

                Milestonedata += "</a>";
                Milestonedata += "</div>";
                Milestonedata += "</div>";
                Milestonedata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                Milestonedata += "<a href ='javascript:void(0);' style='display:" + (streditM == "1" ? "" : "none") + "' type='button' id='btnteam_" + ds.Tables[0].Rows[i]["id"].ToString() + "' onclick='Binddetails_Team(" + ds.Tables[0].Rows[i]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#addteam'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Team'>+</span></a>";
                Milestonedata += "</div>";
                Milestonedata += "</div>";
                Milestonedata += "</td>";



                //Timeline

                DateTime StartDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"].ToString() == "" ? DateTime.Now.ToString() : ds.Tables[0].Rows[i]["StartDate"].ToString());
                DateTime Targetdate = Convert.ToDateTime(ds.Tables[0].Rows[i]["EndDate"].ToString() == "" ? DateTime.Now.ToString() : ds.Tables[0].Rows[i]["EndDate"].ToString());
                TimeSpan objTimeSpan = Targetdate - StartDate;
                double Days = Convert.ToDouble(objTimeSpan.TotalDays);
                Days = Days + 1;


                DateTime Enddate = Convert.ToDateTime(ds.Tables[0].Rows[i]["EndDate"].ToString() == "" ? DateTime.Now.ToString() : ds.Tables[0].Rows[i]["EndDate"].ToString());
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
                int milestonestartyear = StartDate.Year;
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
                int milestoneEndyear = Targetdate.Year;
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



                Milestonedata += "<td>";
                Milestonedata += "<a href = 'javascript:void(0);' class='timesheet-wrap'>";
                Milestonedata += "<div class='progress progress-xl bg-dark position-relative' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + leftDays.ToString().Replace("-", "") + " " + leftdlydys + "'>";
                //Milestonedata += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 70%;' aria-valuemax='100'></div>";
                Milestonedata += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 100%;' aria-valuemax='100'></div>";
                Milestonedata += "<div class='dayswrapper'>";
                Milestonedata += "<span class='fromtodate'>" + ststartdys + " " + startmonthname +" " + milestonestartyear  + " - " + enddys + " " + endmonthname + " " + milestoneEndyear + "</span>";
                Milestonedata += "<span class='totaldays'>" + Days + " days</span>";
                Milestonedata += "</div>";
                Milestonedata += "</div>";
                Milestonedata += "</a>";
                Milestonedata += "</td>";


                //Milestone Timeline
                
                //string milestone_timeline = "";
                //string Milestone_Timeline = "select * from Pms_tbl_Addtask where status=1 and archive=0  and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and Milestoneid = '" + ds.Tables[0].Rows[i]["id"].ToString() + "'  ";
                //DataSet ds_milestone_Timeline = da.GetDataSetInline(Milestone_Timeline);
                //if (ds_milestone_Timeline.Tables[0].Rows.Count > 0)
                //{
                //    for (int mt = 0; mt < ds_milestone_Timeline.Tables[0].Rows.Count; mt++)
                //    {
                //        milestone_timeline += "<a href = 'javascript:void(0);' class='timesheet-wrap'>";
                //        milestone_timeline += "<div class='progress progress-xl bg-dark position-relative me-1' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + cmf.dateConvert(ds_milestone_Timeline.Tables[0].Rows[mt]["Startdate"].ToString()) + " - " + cmf.dateConvert(ds_milestone_Timeline.Tables[0].Rows[mt]["Targetdate"].ToString()) + " '>";
                //        milestone_timeline += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 100%;' aria-valuemax='100'>" + ds_milestone_Timeline.Tables[0].Rows[mt]["taskname"].ToString() + "</div>";
                //        milestone_timeline += "</div>";
                //        milestone_timeline += "</a>";

                //    }

                //}

                //Milestonedata += "<td valign='top'>";
                //Milestonedata += "<div class='d-flex'>";
                //Milestonedata += "<div class='flex-grow-1 align-self-center me-2'>";
                //Milestonedata += "<div class='extended_time'>";
                //Milestonedata += "" + milestone_timeline + "";
                
                //Milestonedata += "</div>";
                //Milestonedata += "</div>";
                //Milestonedata += "</div>";
                //Milestonedata += "</td>";



                // extended timeleine 

                string estendleftdlydys = "";
                string estendstartmonthname = "";
                string estendendmonthname = "";
                double estendleftDays = 0;
                int estendststartdys = 0;
                int estendenddys = 0;
                double estendDays = 0;
                string extenddatebind = "";
                string strextend = "select id,extendStartdate as StartDate,extendEnddate as TargetDate from Pms_Tbl_Extended_Timeline where  status=1 and Type='Milestone' and archive=0  and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and MilestoneID = '" + ds.Tables[0].Rows[i]["id"].ToString() + "'  ";
                DataSet dsstrextend = da.GetDataSetInline(strextend);
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



                        extenddatebind += "<a href = 'javascript:void(0);' class='timesheet-wrap'>";
                        extenddatebind += "<div class='progress progress-xl bg-dark position-relative me-1' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + estendststartdys + " " + estendstartmonthname + " - " + estendenddys + " " + estendendmonthname + " (" + estendleftDays.ToString().Replace("-", "") + " " + estendleftdlydys + ")'>";
                        extenddatebind += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 100%;' aria-valuemax='100'>" + estendDays + " days</div>";
                        extenddatebind += "</div>";
                        extenddatebind += "</a>";

                    }

                }





                Milestonedata += "<td>";
                Milestonedata += "<div class='d-flex'>";
                Milestonedata += "<div class='flex-grow-1 align-self-center me-2'>";
                Milestonedata += "<div class='extended_time' id='lblExtnddt_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>";

                Milestonedata += "" + extenddatebind + "";

                Milestonedata += "</div>";
                Milestonedata += "</div>";

                if (leftDays < 0)
                {
                    Milestonedata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                    Milestonedata += "<a href ='javascript:void(0);' style='display:" + (streditM == "1" ? "" : "none") + "' type='button' id='btnextndt_" + ds.Tables[0].Rows[i]["id"].ToString() + "'  onclick='Bindextenddate_Details(" + ds.Tables[0].Rows[i]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#ExtendedTimline'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Extended Day'>+</span></a>";
                    Milestonedata += "</div>";
                }

                Milestonedata += "</div>";
                Milestonedata += "</div>";
                Milestonedata += "</td>";



                Milestonedata += "<td valign='top'>";
                Milestonedata += cmf.dateConvert(ds.Tables[0].Rows[i]["StartDate"].ToString());
                Milestonedata += "</td>";

                Milestonedata += "<td valign='top'>";
                Milestonedata += cmf.dateConvert(ds.Tables[0].Rows[i]["EndDate"].ToString()); //target date
                Milestonedata += "</td>";



                //Status//

                string strvalcolor = get_Status_Color(ds.Tables[0].Rows[i]["P_Status"].ToString());
                Milestonedata += "<td>";
                Milestonedata += "<div class='btn-group d-block'>";
                if(streditM == "1")
                {
                    Milestonedata += "<a href ='javascript:void(0);' id='btnsts_" + ds.Tables[0].Rows[i]["id"].ToString() + "' type='button' onclick=setmilsestoneid('" + ds.Tables[0].Rows[i]["id"].ToString() + "',this.id) data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + strvalcolor + "'>" + get_Projectstatus(ds.Tables[0].Rows[i]["P_Status"].ToString()) + "</a>";
                }
                else
                {
                    Milestonedata += "<a href ='javascript:void(0);' id='btnsts_" + ds.Tables[0].Rows[i]["id"].ToString() + "' type='button'  data-bs-toggle='' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + strvalcolor + "'>" + get_Projectstatus(ds.Tables[0].Rows[i]["P_Status"].ToString()) + "</a>";
                }
                
                Milestonedata += "<div class='dropdown-menu lead_form_body p-0'>";
                Milestonedata += retval;
                Milestonedata += "</div>";
                Milestonedata += "</div>";
                Milestonedata += "</td>";


                //stpriority

                string strprioritycolor = get_PriorityColor(ds.Tables[0].Rows[i]["Priority"].ToString());
                Milestonedata += "<td>";
                Milestonedata += "<div class='btn-group d-block'>";
                if (streditM == "1")
                {
                    Milestonedata += "<a href ='javascript:void(0);' id='btnpri_" + ds.Tables[0].Rows[i]["id"].ToString() + "' type='button' onclick=setpriorityid('" + ds.Tables[0].Rows[i]["id"].ToString() + "',this.id) data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + strprioritycolor + "'>" + get_PriorityName(ds.Tables[0].Rows[i]["Priority"].ToString()) + "</a>";
                }
                else
                {
                    Milestonedata += "<a href ='javascript:void(0);' id='btnpri_" + ds.Tables[0].Rows[i]["id"].ToString() + "' type='button'  data-bs-toggle='' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + strprioritycolor + "'>" + get_PriorityName(ds.Tables[0].Rows[i]["Priority"].ToString()) + "</a>";
                }
                
                Milestonedata += "<div class='dropdown-menu lead_form_body p-0'>";
                Milestonedata += strvalprority;
                Milestonedata += "</div>";
                Milestonedata += "</div>";
                Milestonedata += "</td>";


                //Issues

                Milestonedata += "<td valign='top'>";

                float completetepercentissues = 0;
                float Pendingepercentissues = 0;
                completetepercentissues = Issuescomplite * 100 / IssuesPhaseTotal;
                Pendingepercentissues = Issuespending * 100 / IssuesPhaseTotal;

                Milestonedata += "<div class='d-flex'>";
                Milestonedata += "<div class='flex-grow-1 align-self-center'>";

                Milestonedata += "<div class='progress progress-xl cursor-pointer'>";
                Milestonedata += "<div class='progress-bar bg-success' role='progressbar' style='width: " + fns.ToFormattedCurrencyString(completetepercentissues.ToString()) + "%' aria-valuemin='0' aria-valuemax='100' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + fns.ToFormattedCurrencyString(completetepercentissues.ToString()) + "% [" + Issuescomplite + " of " + IssuesPhaseTotal + " Issues Completed]'>" + Issuescomplite + "</div>";

                Milestonedata += "<div class='progress-bar bg-warning' role='progressbar' style='width: " + fns.ToFormattedCurrencyString(Pendingepercentissues.ToString()) + "%' aria-valuemin='0' aria-valuemax='100' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + fns.ToFormattedCurrencyString(Pendingepercentissues.ToString()) + "% [" + Issuespending + " of " + IssuesPhaseTotal + " Issues Not Completed]'>" + Issuespending + "</div>";
                Milestonedata += "</div>";
                Milestonedata += "</div>";

                Milestonedata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                Milestonedata += "<a href ='javascript:void(0);' style='display:"+(strviewI=="1"?"":"none")+"' type='button' data-bs-toggle='offcanvas' data-bs-target=#canvas_popup onclick=loadsrc('inner_popup','/pms/AddIssuesPopup.aspx?id=" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "&milstoneid=" + ds.Tables[0].Rows[i]["id"].ToString() + "&type=milstone')><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Issue'>+</span></a>";

                Milestonedata += "</div>";
                Milestonedata += "</td>";

                
                Milestonedata += "<td valign='top'>";
                // file 
                string fileid = "filescount_"+ ds.Tables[0].Rows[i]["projectid"].ToString()+"_"+ ds.Tables[0].Rows[i]["id"].ToString();
                string strfilescount = countfiles(ds.Tables[0].Rows[i]["id"].ToString());
                Milestonedata += "<a href='javascript:void(0);' class='position-relative' type='button' data-bs-toggle='modal' data-bs-target='#addfiles'  onclick='loadsrcfiles(\"inner_popupfiles\",\"/pms/Addfiles.aspx?id=" + ds.Tables[0].Rows[i]["id"].ToString() + "&type=milestone&pid=" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "\",\"milestone\")'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='Upload Documents' style='font-size:12px;height:22px;width:22px;margin-left:9px'><i class='bi bi-file-earmark-arrow-up'></i></span><span class='files_count' id='"+ fileid + "'>"+ strfilescount + "</span></a>";
                //if (ds.Tables[0].Rows[i]["Uploaddrawing"].ToString() == "0" || ds.Tables[0].Rows[i]["Uploaddrawing"].ToString() == string.Empty)
                //{
                //    Milestonedata += "";
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
                //                strfiles += "<img src='/pms/MileImages/" + fileName + "' style='height:50px;' class='preview-item' />";
                //            }
                //            else if (extension == ".pdf")
                //            {
                //                strfiles += "<a href='/pms/MileImages/" + fileName + "' target='_blank' class='preview-item'><span style='font-size:25px; margin:5px;'>📄</span></a>";

                //            }
                //        }

                //    }
                //    else
                //    {
                //        string fileName = ds.Tables[0].Rows[i]["Uploaddrawing"].ToString();
                //        string extension = Path.GetExtension(fileName).ToLower();

                //        if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif")
                //        {
                //            strfiles += "<img src='/pms/MileImages/" + fileName + "' style='height:50px;' class='preview-item' />";
                //        }
                //        else if (extension == ".pdf")
                //        {
                //            strfiles += "<a href='/pms/MileImages/" + fileName + "' target='_blank' class='preview-item'><span style='font-size:25px; margin:5px;'>📄</span></a>";

                //        }
                //    }
                //    Milestonedata += strfiles;


                //}

                Milestonedata += "</td>";

                //Milestonedata += "<td valign='top'>";
                //Milestonedata += ds.Tables[0].Rows[i]["Budget"].ToString(); // budget 
                //Milestonedata += "</td>";

                //Milestonedata += "<td valign='top'>";
                //Milestonedata += ds.Tables[0].Rows[i]["Actual"].ToString(); // Actual 
                //Milestonedata += "</td>";


                project_name = ds.Tables[0].Rows[i]["ProjectId"].ToString();
             



                Milestonedata += "</tr>";
            }
        }

        Milestonedata += "</tbody>";
        Milestonedata += "</table>";

        return Milestonedata;
    }
    public static string countfiles(string mid)
    {
        DataAccess da = new DataAccess();
        string retval = "";
        string pid = "0";
        string strsql = "select id,isnull(ProjectId,0) as ProjectId from Pms_tbl_AddMilestone where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and id='"+ mid + "' ";
        DataSet dsM = da.GetDataSetInline(strsql);
        if (dsM.Tables[0].Rows.Count > 0)
        {
            pid = dsM.Tables[0].Rows[0]["ProjectId"].ToString();
            strsql = "select id from Pms_tbl_projectfiles where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and pid='"+pid+ "' and fentryid='" + mid + "' and ftype='milestone' ";
            DataSet dsF = da.GetDataSetInline(strsql);
            if(dsF.Tables[0].Rows.Count>0)
            {
                retval = dsF.Tables[0].Rows.Count.ToString();
            }
        }
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
    public static string Bindcheckweitagedetails(int RecordId = 0)
    {
        DataAccess da = new DataAccess();
        DataSet ds;
        string strsql = "select * from Pms_tbl_AddProject where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
        ds = da.GetDataSetInline(strsql);

        return ds.GetXml();
    }
    [WebMethod]
    public static string bindMultiOwner_new(string RecordId = "")
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

        return  dataN;

    }

    [WebMethod]
    public static string bindMultiChangeOwner_new(string projectid = "")
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
    public static string bindMultiTeam_new(string RecordId = "")
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
    public static string bindMultiChangeTeam_new(string projectid = "")
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
    public static string Bindprojectdetail_extendatedate(int RecordId = 0)
    {

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "select * from Pms_tbl_AddMilestone where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
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
        prm[4] = new SqlParameter("@type", "milestone");
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

    protected static string get_status_listbind()
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = "select * from Pms_tbl_Projectstatus where  status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret += "<ul class='sttus-list' data-simplebar style='max-height: 160px'>";
            for (int i = 0; i < dscode.Tables[0].Rows.Count; i++)
            {

                strret += "<li>" +
                  "<div class='dropdown-item p-0'>" +
                    "<a class='' href='javascript:void(0);' onclick = 'UpdateDataStatusdeatils(" + dscode.Tables[0].Rows[i]["id"].ToString() + "," + (dscode.Tables[0].Rows[i]["iscomplete"].ToString()==""?"0": dscode.Tables[0].Rows[i]["iscomplete"].ToString()) + ")' > " +
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

    protected static string get_Projectstatus(string projectid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_Projectstatus where  status=1 and archive=0 and id='" + projectid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
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

    [WebMethod]
    public static string Binddependencydata(string mid)
    {
        string retval = "";
        DataAccess da = new DataAccess();
        string strsql = "select * from tbl_dependencyST where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and isnull(milestoneid,0)='" + mid + "'";
       // SaveLogText("getdependencydetails-"+ strsql);
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
                        if (chkallcomplete !=1)
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
                string strprms = "strfromdate=" + strfromdate + ",strtodate=" + strtodate + ",selecttype=" + issuetype + ",entryid=" + RecordId + ",columname=" + strcolname;
                SaveLogText("CheckIssuesdata=" + strprms);
                string ownerid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();
                SqlParameter[] prmC = new SqlParameter[8];
                prmC[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prmC[1] = new SqlParameter("@isadmin", chkadmin);
                prmC[2] = new SqlParameter("@strfromdate", strfromdate);
                prmC[3] = new SqlParameter("@strtodate", strtodate);
                prmC[4] = new SqlParameter("@selecttype", "milstone");
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
public class milestonetdata
{
    public string id { get; set; }
    public string ProjectId { get; set;}
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




