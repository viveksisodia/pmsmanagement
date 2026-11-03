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

public partial class Addproject : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataAccess da = new DataAccess();
    public CommonFunction cmf = new CommonFunction();
    SqlParameter[] prm, prm1;
    public function fns = new function();
    public string projectdata = "";
    public string Multiteam = "";
    public string MultiOwner = "";
    public string Multichangeowner = "";
    public string Multiaddteam = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            columnshowhide();
            bindcountry();
           //  bindemployee();
            bindMultiteam();
            bindMultiOwner();
            bindprojectstatus();
            bindPriorityType();
            bindMultichangeOwner();
            bindMultiAddteam();





        }
    }


    protected void bindcountry()
    {
        string strleadstatus = "select id,countryname  from tbl_countrymaster order by id";
        DataSet ds = da.GetDataSetInline(strleadstatus);
        if (ds.Tables[0].Rows.Count > 0)
        {
            cmf.BindDropDownList(Countryid, ds, "countryname", "id");
            ListItem li1 = new ListItem("", "0");
            Countryid.Items.Insert(0, li1);
        }


    }

    protected void bindState()
    {
        string strleadstatus = "select * from Imt_state  where conid='" + Countryid.SelectedValue + "' and status=1 and archive=0 order by stateid";
        DataSet ds = da.GetDataSetInline(strleadstatus);
        if (ds.Tables[0].Rows.Count > 0)
        {
            cmf.BindDropDownList(StateName, ds, "state", "stateid");
            ListItem li1 = new ListItem("", "0");
            StateName.Items.Insert(0, li1);
        }
        
    }

    protected void bindemployee()
    {
        string strleadstatus = "select id,DisplayName  from tbl_employee where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  order by id";
        DataSet ds = da.GetDataSetInline(strleadstatus);
        if (ds.Tables[0].Rows.Count > 0)
        {
            cmf.BindDropDownList(CmpOwner, ds, "DisplayName", "id");
            ListItem li1 = new ListItem("", "0");
            CmpOwner.Items.Insert(0, li1);
        }


    }
    //protected void bindprojectstatus()
    //{
    //    string strleadstatus = "select * from Pms_tbl_Projectstatus where status = 1 and archive = 0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
    //    DataSet ds = da.GetDataSetInline(strleadstatus);
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        cmf.BindDropDownList(ProjectStatus, ds, "ProjectStatus", "id");
    //        ListItem li1 = new ListItem("", "0");
    //        ProjectStatus.Items.Insert(0, li1);
    //    }


    //}

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

    //protected void btndelete_click(object sender, EventArgs e)
    //{
    //    string strsql = "update Pms_tbl_AddProject set status=0,archive=1 where id='" + hdnid.Value + "'";
    //    da.executeDMLQueryinline(strsql);
    //    Page.RegisterClientScriptBlock("js", "<script>alert('Data Deleted !');</script>");
    //    hdnid.Value = "0";
    //   // bindProjectdata();
    //}



    protected void bindprojectstatus()
    {
        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"].ToString());
        DataSet dataset = da.GetDataSet("Pms_SP_SelectProjectstatus_reg", prm);
        cmf.BindDropDownList(ProjectStatus, dataset, "ProjectStatus", "id");
        ListItem li = new ListItem("Select Status", "0");
        ProjectStatus.Items.Insert(0, li);
        ListItem li1 = new ListItem("+ Add New", "-1");
        ProjectStatus.Items.Add(li1);
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
            MultiOwner = hdnowner.Value;
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
            Countryid.SelectedValue = hdnCountryid.Value;
        }
        catch
        {

        }

        try
        {
            StateName.SelectedValue = hdnstatename.Value;
        }
        catch
        {

        }


        try
        {
            ProjectStatus.SelectedValue = hdnProjectStatus.Value;
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

        try
        {
            AwardDate.Value= Convert.ToDateTime(hdnawarddate.Value).ToString("yyyy-MM-dd");
        }
        catch
        {

        }

        try
        {
            StartDate.Value = Convert.ToDateTime(hdnstartdate.Value).ToString("yyyy-MM-dd");
        }
        catch
        {

        }

        try
        {
            TargetDate.Value = Convert.ToDateTime(hdntargetdate.Value).ToString("yyyy-MM-dd");
        }
        catch
        {

        }

        try
        {
            DrawingApprovalDate.Value = Convert.ToDateTime(hdndrawingaprvldate.Value).ToString("yyyy-MM-dd");
        }
        catch
        {

        }





    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        try
        {
            Extendatestrt.Value = Convert.ToDateTime(hdnextndstrt.Value).ToString("yyyy-MM-dd");
        }
        catch
        {

        }

        try
        {
            Extendateend.Value = Convert.ToDateTime(hdnextndend.Value).ToString("yyyy-MM-dd");
        }
        catch
        {

        }

    }

    // web method bind using ajax start //

    [WebMethod]
    public static string SaveProject(projectdata projectdetails)
    {
        string retval = "0";
        DataAccess da1 = new DataAccess();
        string editid = projectdetails.id;
        string entrytype = "Insert";
        if (editid != "" && editid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[30];
        prm[0] = new SqlParameter("@id", editid);
        prm[1] = new SqlParameter("@ProjectID", projectdetails.ProjectID);
        prm[2] = new SqlParameter("@ProjectName", projectdetails.ProjectName);
        prm[3] = new SqlParameter("@Owner", projectdetails.Owner);
        prm[4] = new SqlParameter("@team", projectdetails.team);
        prm[5] = new SqlParameter("@ProjectStatus", projectdetails.ProjectStatus);
        prm[6] = new SqlParameter("@ProjectAddress1", projectdetails.ProjectAddress1);
        prm[7] = new SqlParameter("@ProjectAddress2", projectdetails.ProjectAddress2);
        prm[8] = new SqlParameter("@City", projectdetails.City);
        prm[9] = new SqlParameter("@PostalCode", projectdetails.PostalCode);
        prm[10] = new SqlParameter("@StateName", projectdetails.StateName);
        prm[11] = new SqlParameter("@Countryid", projectdetails.Countryid);
        prm[12] = new SqlParameter("@ClientName", projectdetails.ClientName);

        prm[13] = new SqlParameter("@Priority", projectdetails.Priority);
        prm[14] = new SqlParameter("@Budget", projectdetails.Budget);
        prm[15] = new SqlParameter("@Actual", projectdetails.Actual);

        if (projectdetails.AwardDate == string.Empty)
        {
            prm[16] = new SqlParameter("@AwardDate", DBNull.Value);
        }
        else
        {
            prm[16] = new SqlParameter("@AwardDate", projectdetails.AwardDate);
        }

        if (projectdetails.StartDate == string.Empty)
        {
            prm[17] = new SqlParameter("@StartDate", DBNull.Value);
        }
        else
        {
            prm[17] = new SqlParameter("@StartDate", projectdetails.StartDate);
        }

        if (projectdetails.TargetDate == string.Empty)
        {
            prm[18] = new SqlParameter("@TargetDate", DBNull.Value);
        }
        else
        {
            prm[18] = new SqlParameter("@TargetDate", projectdetails.TargetDate);
        }

        if (projectdetails.ApprovalDrawing == string.Empty)
        {
            prm[19] = new SqlParameter("@ApprovalDrawing", DBNull.Value);
        }
        else
        {
            prm[19] = new SqlParameter("@ApprovalDrawing", projectdetails.ApprovalDrawing);
        }

        if (projectdetails.DrawingApprovalDate == string.Empty)
        {
            prm[20] = new SqlParameter("@DrawingApprovalDate", DBNull.Value);
        }
        else
        {
            prm[20] = new SqlParameter("@DrawingApprovalDate", projectdetails.DrawingApprovalDate);
        }

        if (projectdetails.Uploaddrawing == string.Empty)
        {
            prm[21] = new SqlParameter("@Uploaddrawing", DBNull.Value);
        }
        else
        {
            prm[21] = new SqlParameter("@Uploaddrawing", projectdetails.Uploaddrawing);
        }



        prm[22] = new SqlParameter("@Weightage", projectdetails.Weightage);
        prm[23] = new SqlParameter("@isdrawing", projectdetails.isdrawing);
        prm[24] = new SqlParameter("@checkprojectWeightage", projectdetails.checkprojectWeightage);
        prm[25] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[26] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[27] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[28] = new SqlParameter("@type", entrytype);
        //da1.executeDMLQuery("pmstbl_Addproject_usp_old", prm);
        prm[29] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[29].Direction = ParameterDirection.ReturnValue;
        int retval1 = da1.executeDMLQuery("pmstbl_Addproject_usp", prm);
        return retval = retval1.ToString();
    }

    [WebMethod]
    public static string SaveProject_Owner(projectdata projectdetails)
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string editid = projectdetails.id;
        string entrytype = "Update";
        if (editid != "" && editid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", editid);
        prm[1] = new SqlParameter("@Owner", projectdetails.Owner);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pmstbl_OwnerDeatilsUpdate_usp", prm);


        DataAccess da = new DataAccess();
        string strsql = "select * from Pms_tbl_AddProject where  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and  id='" + editid + "' ";
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
    public static string SaveProject_Team(projectdata projectdetails)
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string editid = projectdetails.id;
        string entrytype = "Update";
        if (editid != "" && editid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", editid);
        prm[1] = new SqlParameter("@team", projectdetails.team);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pmstbl_TeamDeatilsUpdate_usp", prm);

        DataAccess da = new DataAccess();
        string strsql = "select * from Pms_tbl_AddProject where  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and  id='" + editid + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            // retval = dscode.Tables[0].Rows[0]["DisplayName"].ToString();
            for (int i = 0; i < dscode.Tables[0].Rows.Count; i++)
            {
                string cnt = "0";
                string usersdetails = "";
                string teamtooltip = "";
                if (dscode.Tables[0].Rows[i]["team"].ToString()!= "")
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
        string strsql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus where  status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='"+ statusid + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            retval = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString()+"~~"+ dscode.Tables[0].Rows[0]["PrintColor"].ToString();
        }
        // return dscode.GetXml();
        return retval;
    }

    [WebMethod]
    public static string UpdatePriority_Status(string projectid = "", string Propertyid = "")
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
    public static string AddAddprojecttatus(string value = "", string statuscolor = "")
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
    public static string UpdateExtendeddatedtls(string prjctid = "", string extendStrtdate = "", string extendEnddate = "")
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
        if (prjctid != "" && prjctid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[7];
        prm[0] = new SqlParameter("@id", prjctid);
        prm[1] = new SqlParameter("@StartDate", extendStrtdate);
        prm[2] = new SqlParameter("@TargetDate", extendEnddate);
        prm[3] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[4] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[5] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[6] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pmstbl_Extnddate_DeatilsUpdate", prm);



        // extended timeleine 


        string strextend = "select id,extendStartdate as StartDate,extendEnddate as TargetDate from Pms_Tbl_Extended_Timeline where  status=1 and archive=0 and Type='Project' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and ProjectID = '" + prjctid + "'  ";
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


                //retval += "<div class='progress progress-xl bg-dark position-relative'  data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + estendleftDays.ToString().Replace("-", "") + " " + estendleftdlydys + "'>";
                //retval += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 70%;' aria-valuemax='100'></div>";
                //retval += "<div class='dayswrapper'>";
                //retval += "<span class='fromtodate'>" + estendststartdys + " " + estendstartmonthname + " - " + estendenddys + " " + estendendmonthname + "</span>";
                //retval += "<span class='totaldays'>" + estendDays + " days</span>";
                //retval += "</div>";
                //retval += "</div>";

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
    public static string SaveTags(string prjctid = "", string Tags = "", string TagsColor = "")
    {
        string retval = "";

        DataAccess da1 = new DataAccess();
        string editid = prjctid;
        string entrytype = "Insert";
        SqlParameter[] prm = new SqlParameter[8];
        prm[0] = new SqlParameter("@id", editid);
        prm[1] = new SqlParameter("@ProjectId", prjctid);
        prm[2] = new SqlParameter("@Tags", Tags);
        prm[3] = new SqlParameter("@TagsColor", "#"+TagsColor);
        prm[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[6] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[7] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pmstbl_AddTags_usp", prm);


        //string tagsname = "";
        //string tagscolor = "";
        //string strtags = "select Top 1 tags,TagsColor from   Pms_tbl_Addtags where ProjectId='" + prjctid + "'  order by id asc";
        //DataSet dstags = da1.GetDataSetInline(strtags);
        //if (dstags.Tables[0].Rows.Count > 0)
        //{
        //    tagsname = dstags.Tables[0].Rows[0]["tags"].ToString();
        //    tagscolor = dstags.Tables[0].Rows[0]["TagsColor"].ToString();
        //}


        //string tagdetails = "";
        //if (tagsname != "" && tagsname != "0")
        //{
        //    retval = "<span class='badge badge-label' style='background:" + tagscolor + ";'><i class='ti ti-point-filled'></i> " + tagsname + "</span>";

        //}


        //string counttags = "";
        //string strcountgs = "select COUNT(*) as counttags  from   Pms_tbl_Addtags where ProjectId='" + prjctid + "'";
        //DataSet dscounttags = da1.GetDataSetInline(strcountgs);
        //if (dscounttags.Tables[0].Rows.Count > 0)
        //{
        //    counttags = dscounttags.Tables[0].Rows[0]["counttags"].ToString();
        //}

        //string cnttgs = "";
        //if (counttags != "" && counttags != "0")
        //{
        //    cnttgs = "+" + counttags;

        //}



        string tagsname = "";
        string tagscolor = "";
        string strtags = "select Top 1 tags,TagsColor from   Pms_tbl_Addtags where ProjectId='" + prjctid + "'  order by id asc";
        DataSet dstags = da1.GetDataSetInline(strtags);
        if (dstags.Tables[0].Rows.Count > 0)
        {
            tagsname = dstags.Tables[0].Rows[0]["tags"].ToString();
            tagscolor = dstags.Tables[0].Rows[0]["TagsColor"].ToString();
        }


        string tagdetails = "";
        if (tagsname != "" && tagsname != "0")
        {
            tagdetails = "<span class='badge badge-label' style='background:" + tagscolor + ";'><i class='ti ti-point-filled'></i> " + tagsname + "</span>";

        }


        string counttags = "";
        string strcountgs = "select COUNT(*) as counttags  from   Pms_tbl_Addtags where ProjectId='" + prjctid + "'";
        DataSet dscounttags = da1.GetDataSetInline(strcountgs);
        if (dscounttags.Tables[0].Rows.Count > 0)
        {
            counttags = dscounttags.Tables[0].Rows[0]["counttags"].ToString();
        }

        string cnttgs = "";
        if (counttags != "" && counttags != "0")
        {
            cnttgs = "+" + counttags;

        }

        string tag_sname = "";
        string tags_details = "";
        string str_tags = "select * from   Pms_tbl_Addtags where status = 1 and archive = 0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  and ProjectId = '" + prjctid + "' ";
        DataSet dstag_dtls = da1.GetDataSetInline(str_tags);
        if (dstag_dtls.Tables[0].Rows.Count > 0)
        {
            for (int k = 0; k < dstag_dtls.Tables[0].Rows.Count; k++)
            {

                if (tag_sname == "")
                {
                    tag_sname = dstag_dtls.Tables[0].Rows[k]["Tags"].ToString();
                }
                else
                {
                    tag_sname += "," + dstag_dtls.Tables[0].Rows[k]["Tags"].ToString();

                }



            }
            if (tagsname != "")
            {
                tags_details = tag_sname;
            }
        }

        retval += "<div class='d-flex'>";
        retval += "<div class='flex-grow-1 align-self-center'>";
        retval += "<div class='flex-grow-1'>";
        retval += "<div class='flex-wrap gap-2'>";
        retval += "<a href = 'javascript:void(0);' type='button' data-bs-toggle='offcanvas' data-bs-target='#canvas_popup' onclick=loadsrc('inner_popup','/AddTagsPopup.aspx?id=" + prjctid + "&type=Project')>" + tagdetails + "</a>";
        retval += "<span class='cursor-pointer' data-bs-toggle='tooltip' data-bs-html='true' data-bs-placement='top' data-bs-trigger='hover' title='"+tags_details+"'>" + cnttgs + "</span>";
        retval += "</div>";
        retval += "</div>";
        retval += "</div>";
        retval += "</div>";
      



        return retval;



    }



    [WebMethod]
    public static string bindProjectdata()
    {
        string retval=  get_status_listbind();
        string strvalprority = get_Priority_listbind();
        string projectdata = "";
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



        projectdata += "<table cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm datatable'>";
        projectdata += "<thead>";
        projectdata += "<tr>";

        //projectdata += "<th>Select</th>";
        projectdata += "<th>S.No.</th>";
        projectdata += "<th>Action</th>";
        projectdata += "<th>Project ID</th>";
        projectdata += "<th style='min-width:240px;'>Project Name</th>";
        projectdata += "<th style=min-width:140px;>Completion Status</th>";
        projectdata += "<th style=min-width:140px;>Milestone</th>";
        if (istaskchk == 1)
        {
            projectdata += "<th style=min-width:140px;>Task</th>";
        }

        projectdata += "<th style='min-width:120px;'>Owner</th>";
        projectdata += "<th style='min-width:120px;'>Team</th>";
        projectdata += "<th style=min-width:140px;>Timeline</th>";
        //projectdata += "<th style=min-width:140px;>Milestone Timeline</th>";
        projectdata += "<th style=min-width:140px;>Extended Timeline</th>";
        projectdata += "<th>Start Date</th>";
        projectdata += "<th>End Date</th>";
        projectdata += "<th style='width:130px;'>Status</th>";
        projectdata += "<th>Priority</th>";
        projectdata += "<th style=min-width:140px;>Issues</th>";
        projectdata += "<th style='min-width:120px;'>Tags</th>";
        projectdata += "<th>Budget</th>";
        projectdata += "<th>Actual</th>";
        projectdata += "<th>Project Address</th>";
        projectdata += "<th>Client Name</th>";
        projectdata += "<th>Award Date</th>";
        if (isdrawingkchk == 1)
        {
            projectdata += "<th>Approval of Drawing</th>";
            projectdata += "<th>Drawing Approval Date</th>";
            projectdata += "<th>Upload Drawing</th>";
        }
        projectdata += "</tr>";
        projectdata += "</thead>";
        projectdata += "<tbody>";
        if (ds.Tables[0].Rows.Count > 0)
        {


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


                

                //   completion status start //

                //float Wattagecomplite = 0;
                //float Wattagepending = 100;
                //float projectTotal = 0;
                //float PhaseTotal = 0;
                //float TaskTotal = 0;

                //string str = "select isnull(weightage,0)as Weightage, * from Pms_tbl_AddProject where id='" + ds.Tables[0].Rows[i]["id"] + "' and status=1 and archive=0 ";
                //DataSet dss = da.GetDataSetInline(str);

                //for (int k = 0; k < dss.Tables[0].Rows.Count; k++)
                //{
                //    try
                //    {
                //        PhaseTotal = Convert.ToInt32(dss.Tables[0].Rows[k]["Weightage"]);
                //    }
                //    catch (Exception)
                //    {
                //    }
                //    string findtask = "select isnull(sum(weightage),0)Wattagetasksum from Pms_tbl_AddMilestone where ProjectId='" + dss.Tables[0].Rows[k]["id"] + "' and status=1 and status=1 and archive=0";
                //    DataSet dsssumtask = da.GetDataSetInline(findtask);

                //    try
                //    {
                //        TaskTotal = Convert.ToInt32(dsssumtask.Tables[0].Rows[0]["Wattagetasksum"]);
                //    }
                //    catch (Exception)
                //    {
                //    }
                //    projectTotal += (PhaseTotal * TaskTotal / 100);

                //}

                //if (projectTotal > 0)
                //{
                //    Wattagecomplite = projectTotal;
                //    Wattagepending = Math.Abs(Wattagepending - Wattagecomplite);
                //}

                // completion status end //


                //   completion status start //

                decimal Wattagecomplite = 0;
                decimal Wattagepending = 100;
                decimal projectTotal = 0;
                decimal PhaseTotal = 0;
                decimal TaskTotal = 0;
               

                string str = "select cast(ROUND(isnull(iscompletepercentage,0),2) AS DECIMAL(10,2)) as iscompletepercentage from Pms_tbl_AddProject where id='" + ds.Tables[0].Rows[i]["id"] + "' and status=1 and archive=0 ";
                DataSet dss = da.GetDataSetInline(str);
                

                    try
                    {
                        TaskTotal = Convert.ToDecimal(dss.Tables[0].Rows[0]["iscompletepercentage"].ToString());
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

                // completion status end //



                //   Milestone count start //

                float Milestonecomplite = 0;
                float Milestonepending = 0;
                float MilestoneprojectTotal = 0;
                float MilestonePhaseTotal = 0;
                float MilesstoneTaskTotal = 0;

                string str1 = "select COUNT(*) as countmilstone from Pms_tbl_AddMilestone where ProjectId='" + ds.Tables[0].Rows[i]["id"] + "' and status=1 and archive=0 ";
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

                string str2 = "select * from Pms_tbl_AddMilestone where ProjectId='" + ds.Tables[0].Rows[i]["id"] + "' and status=1 and archive=0 ";
                DataSet dss2 = da.GetDataSetInline(str2);
                if (dss2.Tables[0].Rows.Count > 0)
                {

                      string findtask1 = "select COUNT(*) as counttask  from Pms_tbl_AddMilestone where ProjectId='" + ds.Tables[0].Rows[i]["id"] + "'  and P_Status in ( select id from Pms_tbl_Projectstatus where  status=1 and archive=0 and isnull(iscomplete,0)=1)  and status=1 and archive=0";
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



                //   task count start //

                float tskcomplite = 0;
                float tskpending = 0;
                float tskprojectTotal = 0;
                float tskPhaseTotal = 0;
                float tsk_TaskTotal = 0;

                string str11 = "select COUNT(*) as counttsk from Pms_tbl_Addtask where ProjectId='" + ds.Tables[0].Rows[i]["id"] + "' and status=1 and archive=0 ";
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

                string str22 = "select * from Pms_tbl_AddMilestone where ProjectId='" + ds.Tables[0].Rows[i]["id"] + "' and status=1 and archive=0 ";
                DataSet dss22 = da.GetDataSetInline(str22);
                if (dss22.Tables[0].Rows.Count > 0)
                {

                    string findtask11 = "select COUNT(*) as counttask  from Pms_tbl_Addtask where ProjectId='" + ds.Tables[0].Rows[i]["id"] + "'  and P_Status in ( select id from Pms_tbl_Projectstatus where  status=1 and archive=0 and isnull(iscomplete,0)=1)  and status=1 and archive=0";
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

                string str111 = "select COUNT(*) as countIssues from Pms_tbl_Addissues where ProjectId='" + ds.Tables[0].Rows[i]["id"] + "' and Issueon='Project' and status=1 and archive=0 ";
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

              
                    string findtask111 = "select COUNT(*) as count_Issues from Pms_tbl_Addissues where ProjectId='" + ds.Tables[0].Rows[i]["id"] + "' and Issueon='Project' and IssueStatus in ( select id from Pms_tbl_Projectstatus where  status=1 and archive=0 and isnull(iscomplete,0)=1)  and status=1 and archive=0";
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



                projectdata += "<tr>";

                projectdata += "<td valign='top'>";
                projectdata += (i + 1);
                projectdata += "</td>";

                projectdata += " <td><a href='javascript:void(0);' Class='btn btn-info btn-sm' type='button' onclick='Binddetails(" + ds.Tables[0].Rows[i]["id"].ToString() + ")' data-bs-toggle='offcanvas' data-bs-target='#addproject'><i class='ri-edit-line'></i></a>";
                projectdata += "&nbsp;&nbsp;<a href='javascript:void(0);' Class='btn btn-danger btn-sm' type='button' onclick='deletedata(" + ds.Tables[0].Rows[i]["id"].ToString() + ")'><i class='ri-delete-bin-5-line'></i></a>";
                projectdata += "</td>";


                projectdata += "<td valign='top'>";
                projectdata += ds.Tables[0].Rows[i]["ProjectID"].ToString();
                projectdata += "</td>";
                

                //int checkissubtask = 0;
                //string strsmilstone = "select * from Pms_tbl_AddMilestone where ProjectId='" + ds.Tables[0].Rows[i]["id"].ToString() + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                //DataSet dsmile = da.GetDataSetInline(strsmilstone);
                //if (dsmile.Tables[0].Rows.Count > 0)
                //{
                //    for (int j = 0; j < dsmile.Tables[0].Rows.Count; j++)
                //    {
                //        string strstask = "select isnull(issubtask ,0) as issubtask  from Pms_tbl_Addtask where   ProjectId='" + ds.Tables[0].Rows[i]["id"].ToString() + "' and Milestoneid='" + dsmile.Tables[0].Rows[j]["id"].ToString() + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
                //        DataSet dstask = da.GetDataSetInline(strstask);
                //        if (dstask.Tables[0].Rows.Count > 0)
                //        {
                //            for (int k = 0; k < dstask.Tables[0].Rows.Count; k++)
                //            {

                //                checkissubtask = Convert.ToInt32(dstask.Tables[0].Rows[k]["issubtask"].ToString());

                //            }

                //        }
                //    }
                //}

                
                
                projectdata += "<td valign='top'>";
                //if(checkissubtask==1)
                //{
                //    projectdata += "<a href = 'javascript:void(0);' type='button' data-bs-toggle='offcanvas' data-bs-target='#canvas_popup_multi' onclick =loadsrc('inner_popup_multi','/MultiprojectSubtaskpopup.aspx?id=" + ds.Tables[0].Rows[i]["id"].ToString() + "&type=Project')>" + ds.Tables[0].Rows[i]["ProjectName"].ToString() + "</a>";
                //}
                //else
                //{
                    projectdata += "<a href = 'javascript:void(0);' type='button' data-bs-toggle='offcanvas' data-bs-target='#canvas_popup_multi' onclick =loadsrc('inner_popup_multi','/MultiProjectTaskPopup.aspx?id=" + ds.Tables[0].Rows[i]["id"].ToString() + "&type=Project')>" + ds.Tables[0].Rows[i]["ProjectName"].ToString() + "</a>";
               // }
              
                 projectdata += "</td>";



                //Completion Status
                projectdata += "<td>";
                if (Wattagecomplite == 0)
                {
                    projectdata += "<div class='progress progress-xl cursor-pointer'>";
                    projectdata += "<div class='progress progress-xl bg-light position-relative' data-bs-toggle='tooltip' data-bs-placement='top' title='Complete(0%)' role='progressbar' style='width:5%'><span style='text-align:center;'>0</span>";
                    projectdata += "</div>";
                    projectdata += "</div>";

                }

                else
                {
                    projectdata += "<div class='progress progress-xl cursor-pointer'>";
                    projectdata += "<div class='progress-bar bg-success' data-bs-toggle='tooltip' data-bs-placement='top' title='Complete (" + Wattagecomplite + "%)' role='progressbar' style='width:" + Wattagecomplite + "%'>" + Wattagecomplite + "%";
                    projectdata += "</div>";

                    projectdata += "<div class='progress-bar bg-warning' data-toggle='tooltip' data-bs-placemen='top' title='Pending (" + Wattagepending + "%)' role='progressbar' style='width:" + Wattagepending + "%'>" + Wattagepending + "%";
                    projectdata += "</div>";
                    projectdata += "</div>";

                }
                projectdata += "</td>";



                // milsetone

                projectdata += "<td valign='top'>";
                float completetepercent = 0;
                float Pendingepercent = 0;
                completetepercent = Milestonecomplite * 100 / MilestonePhaseTotal;
                Pendingepercent = Milestonepending * 100 / MilestonePhaseTotal;

                projectdata += "<div class='d-flex'>";
                projectdata += "<div class='flex-grow-1 align-self-center'>";

                projectdata += "<div class='progress progress-xl cursor-pointer'>";
                projectdata += "<div class='progress-bar bg-success' role='progressbar' style='width: " + fns.ToFormattedCurrencyString(completetepercent.ToString()) + "%' aria-valuemin='0' aria-valuemax='100' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + fns.ToFormattedCurrencyString(completetepercent.ToString()) + "% [" + Milestonecomplite + " of " + MilestonePhaseTotal + " Milestone Completed]'>" + Milestonecomplite + "</div>";

                projectdata += "<div class='progress-bar bg-warning' role='progressbar' style='width: " + fns.ToFormattedCurrencyString(Pendingepercent.ToString()) + "%' aria-valuemin='0' aria-valuemax='100' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + fns.ToFormattedCurrencyString(Pendingepercent.ToString()) + "% [" + Milestonepending + " of " + MilestonePhaseTotal + " Milestone Not Completed]'>" + Milestonepending + "</div>";
                projectdata += "</div>";
                projectdata += "</div>";

                projectdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                projectdata += "<a href ='javascript:void(0);' type ='button' data-bs-toggle ='offcanvas' data-bs-target =#canvas_popup onclick =loadsrc('inner_popup','/AddmilstonePopup.aspx?id=" + ds.Tables[0].Rows[i]["id"].ToString() + "&type=Project')><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Milestone'>+</span></a>";

                projectdata += "</div>";
                projectdata += "</td>";



                // task

                int btntaskchk = 0;
                string sqlbtntaskchk = "select isnull(istask,0) istask from Pms_tbl_AddMilestone where status=1 and archive=0  and ProjectId=" + ds.Tables[0].Rows[i]["id"].ToString() + "  and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and isnull(istask,0)=1  ";
                DataSet dschbtntaskchk = da.GetDataSetInline(sqlbtntaskchk);
                if (dschbtntaskchk.Tables[0].Rows.Count > 0)
                {
                    btntaskchk = 1;

                }


                if (istaskchk == 1)
                {
                    projectdata += "<td valign='top'>";
                    float completetepercenttsk = 0;
                    float Pendingepercenttsk = 0;
                    completetepercenttsk = tskcomplite * 100 / tskPhaseTotal;
                    Pendingepercenttsk = tskpending * 100 / tskPhaseTotal;

                    projectdata += "<div class='d-flex'>";
                    projectdata += "<div class='flex-grow-1 align-self-center'>";

                    projectdata += "<div class='progress progress-xl cursor-pointer'>";
                    projectdata += "<div class='progress-bar bg-success' role='progressbar' style='width: " + fns.ToFormattedCurrencyString(completetepercenttsk.ToString()) + "%' aria-valuemin='0' aria-valuemax='100' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + fns.ToFormattedCurrencyString(completetepercenttsk.ToString()) + "% [" + tskcomplite + " of " + tskPhaseTotal + " Task Completed]'>" + tskcomplite + "</div>";

                    projectdata += "<div class='progress-bar bg-warning' role='progressbar' style='width: " + fns.ToFormattedCurrencyString(Pendingepercenttsk.ToString()) + "%' aria-valuemin='0' aria-valuemax='100' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + fns.ToFormattedCurrencyString(Pendingepercenttsk.ToString()) + "% [" + tskpending + " of " + tskPhaseTotal + " Task Not Completed]'>" + tskpending + "</div>";
                    projectdata += "</div>";
                    projectdata += "</div>";

                    if (btntaskchk == 1)
                    {
                        projectdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                        projectdata += "<a href ='javascript:void(0);' type='button' data-bs-toggle='offcanvas' data-bs-target=#canvas_popup onclick=loadsrc('inner_popup','/AddTaskPopup.aspx?id=" + ds.Tables[0].Rows[i]["id"].ToString() + "&type=Project')><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Task'>+</span></a>";
                        projectdata += "</div>";
                    }


                    projectdata += "</td>";
                }


                //Owner

                projectdata += "<td>";
                projectdata += "<div class='d-flex'>";
                projectdata += "<div class='flex-grow-1'>";
                projectdata += "<div class='avatar-group' id='lblowner_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>";
                projectdata += usersdetailsowner;
                
                projectdata += "<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='" + ownertooltip + "' data-bs-original-title='more'>";

                if (cntownr != "" && cnt != "0")
                {
                    projectdata += "<div class='avatar-xxs'>";
                    projectdata += "<div class='avatar-title rounded-circle'>";

                    projectdata += (cntownr + "+");
                    projectdata += "</div>";
                    projectdata += "</div>";
                }

                projectdata += "</a>";
                projectdata += "</div>";
                projectdata += "</div>";
                projectdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                projectdata += "<a href ='javascript:void(0);' type='button' id='btnownr_" + ds.Tables[0].Rows[i]["id"].ToString() + "' onclick='Binddetails_Owner(" + ds.Tables[0].Rows[i]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#changeowner'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Owner'>+</span></a>";
                projectdata += "</div>";
                projectdata += "</div>";
                projectdata += "</td>";



                //team
                projectdata += "<td>";
                projectdata += "<div class='d-flex'>";
                projectdata += "<div class='flex-grow-1'>";
                projectdata += "<div class='avatar-group' id='lbluser_"+ ds.Tables[0].Rows[i]["id"].ToString() + "'>";
                projectdata += usersdetails;
                projectdata += "<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='" + teamtooltip + "' data-bs-original-title='more'>";
               
                if (cnt != "" && cnt != "0")
                {
                    projectdata += "<div class='avatar-xxs'>";
                    projectdata += "<div class='avatar-title rounded-circle'>";

                    projectdata += (cnt + "+");
                    projectdata += "</div>";
                    projectdata += "</div>";
                }               
               
                projectdata += "</a>";
                projectdata += "</div>";
                projectdata += "</div>";
                projectdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                projectdata += "<a href ='javascript:void(0);' type='button' id='btnteam_" + ds.Tables[0].Rows[i]["id"].ToString() + "' onclick='Binddetails_Team(" + ds.Tables[0].Rows[i]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#addteam'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Team'>+</span></a>";
                projectdata += "</div>";
                projectdata += "</div>";
                projectdata += "</td>";



                //Timeline

                DateTime StartDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"].ToString() == "" ? "0" : ds.Tables[0].Rows[i]["StartDate"].ToString());
                DateTime Targetdate = Convert.ToDateTime(ds.Tables[0].Rows[i]["TargetDate"].ToString() == "" ? "0" : ds.Tables[0].Rows[i]["TargetDate"].ToString());
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
                int projectstartyear = StartDate.Year;
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
                int projectEndyear = Targetdate.Year;
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



                projectdata += "<td>";
                projectdata += "<a href = 'javascript:void(0);' class='timesheet-wrap'>";
                projectdata += "<div class='progress progress-xl bg-dark position-relative' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + leftDays.ToString().Replace("-", "") + " " + leftdlydys + "'>";
                // projectdata += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 70%;' aria-valuemax='100'></div>";
                projectdata += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 100%;' aria-valuemax='100'></div>";
                projectdata += "<div class='dayswrapper'>";
                projectdata += "<span class='fromtodate'>" + ststartdys + " " + startmonthname+" " + projectstartyear + " - " + enddys + " " + endmonthname +" " + projectstartyear + "</span>";
                projectdata += "<span class='totaldays'>" + Days + " days</span>";
                projectdata += "</div>";
                projectdata += "</div>";
                projectdata += "</a>";
                projectdata += "</td>";


                //Project Timeline


                //string project_timeline = "";
                //string Project_Timeline = "select * from Pms_tbl_AddMilestone where status=1 and archive=0  and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and ProjectId = '" + ds.Tables[0].Rows[i]["id"].ToString() + "'  ";
                //DataSet ds_Project_Timeline = da.GetDataSetInline(Project_Timeline);
                //if (ds_Project_Timeline.Tables[0].Rows.Count > 0)
                //{
                //    for (int pt = 0; pt < ds_Project_Timeline.Tables[0].Rows.Count; pt++)
                //    {
                //        project_timeline += "<a href = 'javascript:void(0);' class='timesheet-wrap'>";
                //        project_timeline += "<div class='progress progress-xl bg-dark position-relative me-1' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + cmf.dateConvert(ds_Project_Timeline.Tables[0].Rows[pt]["StartDate"].ToString()) + " - " + cmf.dateConvert(ds_Project_Timeline.Tables[0].Rows[pt]["EndDate"].ToString()) + " '>";
                //        project_timeline += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 100%;' aria-valuemax='100'>" + ds_Project_Timeline.Tables[0].Rows[pt]["Milestone"].ToString() + "</div>";
                //        project_timeline += "</div>";
                //        project_timeline += "</a>";

                //    }

                //}

                //projectdata += "<td valign='top'>";
                //projectdata += "<div class='d-flex'>";
                //projectdata += "<div class='flex-grow-1 align-self-center me-2'>";
                //projectdata += "<div class='extended_time'>";
                //projectdata += "" + project_timeline + "";

                //projectdata += "</div>";
                //projectdata += "</div>";
                //projectdata += "</div>";
                //projectdata += "</td>";


                // extended timeleine 

                string estendleftdlydys = "";
                string estendstartmonthname = "";
                string estendendmonthname = "";
                double estendleftDays = 0;
                int estendststartdys = 0;
                int estendenddys = 0;
                double estendDays = 0;
                string extenddatebind = "";
                string strextend = "select id,extendStartdate as StartDate,extendEnddate as TargetDate from Pms_Tbl_Extended_Timeline where  status=1 and Type='Project' and archive=0  and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and ProjectID = '" + ds.Tables[0].Rows[i]["id"].ToString() + "'  ";
                DataSet dsstrextend = da.GetDataSetInline(strextend);
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

                        //extenddatebind += "<div class='progress progress-xl bg-dark position-relative'  data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + estendleftDays.ToString().Replace("-", "") + " " + estendleftdlydys + "'>";
                        //extenddatebind += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 70%;' aria-valuemax='100'></div>";
                        //extenddatebind += "<div class='dayswrapper'>";
                        //extenddatebind += "<span class='fromtodate'>" + estendststartdys + " " + estendstartmonthname + " - " + estendenddys + " " + estendendmonthname + "</span>";
                        //extenddatebind += "<span class='totaldays'>" + estendDays + " days</span>";
                        //extenddatebind += "</div>";
                        //extenddatebind += "</div>";



                        extenddatebind += "<a href = 'javascript:void(0);' class='timesheet-wrap'>";
                        extenddatebind += "<div class='progress progress-xl bg-dark position-relative me-1' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + estendststartdys + " " + estendstartmonthname + " - " + estendenddys + " " + estendendmonthname + " (" + estendleftDays.ToString().Replace("-", "") + " " + estendleftdlydys + ")'>";
                        extenddatebind += "<div class='progress-bar bg-secondary' role='progressbar' style='width: 100%;' aria-valuemax='100'>" + estendDays + " days</div>";
                        extenddatebind += "</div>";
                        extenddatebind += "</a>";



                    }

                }




                projectdata += "<td>";
                projectdata += "<div class='d-flex'>";
                projectdata += "<div class='flex-grow-1 align-self-center me-2'>";
                projectdata += "<div class='extended_time' id='lblExtnddt_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>";

                projectdata += "" + extenddatebind + "";

                projectdata += "</div>";
                projectdata += "</div>";

                if (leftDays < 0)
                {
                    projectdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                    projectdata += "<a href ='javascript:void(0);' type='button' id='btnextndt_" + ds.Tables[0].Rows[i]["id"].ToString() + "'  onclick='Bindextenddate_Details(" + ds.Tables[0].Rows[i]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#ExtendedTimline'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Extended Day'>+</span></a>";
                    projectdata += "</div>";
                }

                projectdata += "</div>";
                projectdata += "</div>";
                projectdata += "</td>";

                projectdata += "<td valign='top'>";
                projectdata += cmf.dateConvert(ds.Tables[0].Rows[i]["StartDate"].ToString());
                projectdata += "</td>";

                projectdata += "<td valign='top'>";
                projectdata += cmf.dateConvert(ds.Tables[0].Rows[i]["TargetDate"].ToString());
                projectdata += "</td>";



                // status

                string strvalcolor = get_Status_Color(ds.Tables[0].Rows[i]["ProjectStatus"].ToString());
                projectdata += "<td>";
                projectdata += "<div class='btn-group d-block'>";
                projectdata += "<a href ='javascript:void(0);' id='btnsts_"+ ds.Tables[0].Rows[i]["id"].ToString() + "' type='button' onclick=setprojectid('"+ ds.Tables[0].Rows[i]["id"].ToString() + "',this.id) data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + strvalcolor + "'>" + get_Projectstatus(ds.Tables[0].Rows[i]["ProjectStatus"].ToString()) + "</a>";
                projectdata +=  "<div class='dropdown-menu lead_form_body p-0'>";
                projectdata += retval;
                projectdata += "</div>";
                projectdata += "</div>";
                projectdata +=   "</td>";

                //priority
                string strprioritycolor = get_PriorityColor(ds.Tables[0].Rows[i]["Priority"].ToString());
                projectdata += "<td>";
                projectdata += "<div class='btn-group d-block'>";
                projectdata += "<a href ='javascript:void(0);'  id='btnpri_" + ds.Tables[0].Rows[i]["id"].ToString() + "' type='button' onclick=setpriorityid('" + ds.Tables[0].Rows[i]["id"].ToString() + "',this.id) data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + strprioritycolor + "'>" + get_PriorityName(ds.Tables[0].Rows[i]["Priority"].ToString()) + "</a>";
                projectdata += "<div class='dropdown-menu lead_form_body p-0'>";
                projectdata += strvalprority;
                projectdata += "</div>";
                projectdata += "</div>";
                projectdata += "</td>";


                //Issues
                

                projectdata += "<td valign='top'>";
                float completetepercentissues = 0;
                float Pendingepercentissues = 0;
                completetepercentissues = Issuescomplite * 100 / IssuesPhaseTotal;
                Pendingepercentissues = Issuespending * 100 / IssuesPhaseTotal;

                projectdata += "<div class='d-flex'>";
                projectdata += "<div class='flex-grow-1 align-self-center'>";

                projectdata += "<div class='progress progress-xl cursor-pointer'>";
                projectdata += "<div class='progress-bar bg-success' role='progressbar' style='width: " + fns.ToFormattedCurrencyString(completetepercentissues.ToString()) + "%' aria-valuemin='0' aria-valuemax='100' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + fns.ToFormattedCurrencyString(completetepercentissues.ToString()) + "% [" + Issuescomplite + " of " + IssuesPhaseTotal + " Issues Completed]'>" + Issuescomplite + "</div>";

                projectdata += "<div class='progress-bar bg-warning' role='progressbar' style='width: " + fns.ToFormattedCurrencyString(Pendingepercentissues.ToString()) + "%' aria-valuemin='0' aria-valuemax='100' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='" + fns.ToFormattedCurrencyString(Pendingepercentissues.ToString()) + "% [" + Issuespending + " of " + IssuesPhaseTotal + " Issues Not Completed]'>" + Issuespending + "</div>";
                projectdata += "</div>";
                projectdata += "</div>";

                projectdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                projectdata += "<a href ='javascript:void(0);' type='button' data-bs-toggle='offcanvas' data-bs-target=#canvas_popup onclick=loadsrc('inner_popup','/AddIssuesPopup.aspx?id=" + ds.Tables[0].Rows[i]["id"].ToString() + "&type=Project')><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Issue'>+</span></a>";

                projectdata += "</div>";
                projectdata += "</td>";


                //Tags //
                string tagsname = "";
                string tagscolor = "";
                string strtags = "select Top 1 tags,TagsColor from   Pms_tbl_Addtags where ProjectId='" + ds.Tables[0].Rows[i]["id"].ToString() + "'  order by id asc";
                DataSet dstags = da.GetDataSetInline(strtags);
                if (dstags.Tables[0].Rows.Count > 0)
                {
                    tagsname = dstags.Tables[0].Rows[0]["tags"].ToString();
                    tagscolor = dstags.Tables[0].Rows[0]["TagsColor"].ToString();
                }


                string tagdetails = "";
                if (tagsname != "" && tagsname != "0")
                {
                    tagdetails = "<span class='badge badge-label' style='background:" + tagscolor + ";'><i class='ti ti-point-filled'></i> " + tagsname + "</span>";

                }


                string counttags = "";
                string strcountgs = "select COUNT(*) as counttags  from   Pms_tbl_Addtags where ProjectId='" + ds.Tables[0].Rows[i]["id"].ToString() + "'";
                DataSet dscounttags = da.GetDataSetInline(strcountgs);
                if (dscounttags.Tables[0].Rows.Count > 0)
                {
                    counttags = dscounttags.Tables[0].Rows[0]["counttags"].ToString();
                }

                string cnttgs = "";
                if (counttags != "" && counttags != "0")
                {
                    cnttgs = "+" + counttags;

                }





                string tag_sname = "";
                string tags_details = "";
                string str_tags = "select * from   Pms_tbl_Addtags where status = 1 and archive = 0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  and ProjectId = '" + ds.Tables[0].Rows[i]["id"].ToString() + "' ";
                DataSet dstag_dtls = da.GetDataSetInline(str_tags);
                if (dstag_dtls.Tables[0].Rows.Count > 0)
                {
                    for (int k = 0; k < dstag_dtls.Tables[0].Rows.Count; k++)
                    {

                        if (tag_sname == "")
                        {
                            tag_sname = dstag_dtls.Tables[0].Rows[k]["Tags"].ToString();
                        }
                        else
                        {
                            tag_sname += "," + dstag_dtls.Tables[0].Rows[k]["Tags"].ToString();

                        }



                    }
                    if (tagsname != "")
                    {
                        tags_details = tag_sname;
                    }
                }



                //title = 'Mohit Tag <br/>  Sanjeev Tag'
                projectdata += "<td>";
                projectdata += "<div class='d-flex'>";
                projectdata += "<div class='flex-grow-1 align-self-center' id='lblAddtags_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>";
                projectdata += "<div class='flex-grow-1'>";
                projectdata += "<div class='flex-wrap gap-2'>";
                projectdata += "<a href = 'javascript:void(0);' type='button' data-bs-toggle='offcanvas' data-bs-target='#canvas_popup' onclick=loadsrc('inner_popup','/AddTagsPopup.aspx?id=" + ds.Tables[0].Rows[i]["id"].ToString() + "&type=Project')>" + tagdetails + "</a>";
                projectdata += "<span class='cursor-pointer' data-bs-toggle='tooltip' data-bs-html='true' data-bs-placement='top' data-bs-trigger='hover' title='" + tags_details + "'>" + cnttgs + "</span>";
                projectdata += "</div>";
                projectdata += "</div>";
                projectdata += "</div>";
                projectdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                projectdata += "<a href = 'javascript:void(0);' type='button' id='btntags_" + ds.Tables[0].Rows[i]["id"].ToString() + "' onclick='Binddetails_Addtags(" + ds.Tables[0].Rows[i]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#addtags'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Tags'>+</span></a>";
                projectdata += "</div>";
                projectdata += "</div>";
                projectdata += "</td>";

              

                projectdata += "<td valign='top'>";
                projectdata += ds.Tables[0].Rows[i]["Budget"].ToString(); // budget 
                projectdata += "</td>";

                projectdata += "<td valign='top'>";
                projectdata += ds.Tables[0].Rows[i]["Actual"].ToString(); // Actual 
                projectdata += "</td>";
                

                projectdata += "<td valign='top'>";
                projectdata += ds.Tables[0].Rows[i]["ProjectAddress1"].ToString() + " "+ ds.Tables[0].Rows[i]["ProjectAddress2"].ToString()+ " " + ds.Tables[0].Rows[i]["City"].ToString() + "</br> " + ds.Tables[0].Rows[i]["PostalCode"].ToString() + " " + get_statename(ds.Tables[0].Rows[i]["StateName"].ToString()) + " " + get_Countryname(ds.Tables[0].Rows[i]["Countryid"].ToString());
                projectdata += "</td>";

                projectdata += "<td valign='top'>";
                projectdata += ds.Tables[0].Rows[i]["ClientName"].ToString();
                projectdata += "</td>";

                projectdata += "<td valign='top'>";
                projectdata += cmf.dateConvert(ds.Tables[0].Rows[i]["AwardDate"].ToString());
                projectdata += "</td>";

                if (isdrawingkchk == 1)
                {

                    string strret = "";
                    if (ds.Tables[0].Rows[i]["ApprovalDrawing"].ToString() == "1")
                    {
                        strret = "Yes";

                    }

                    if (ds.Tables[0].Rows[i]["ApprovalDrawing"].ToString() == "2")
                    {
                        strret = "No";

                    }

                    projectdata += "<td valign='top'>";
                    projectdata += strret;
                    projectdata += "</td>";

                    projectdata += "<td valign='top'>";
                    projectdata += cmf.dateConvert(ds.Tables[0].Rows[i]["DrawingApprovalDate"].ToString());
                    projectdata += "</td>";

                    projectdata += "<td valign='top'>";
                    // projectdata += ds.Tables[0].Rows[i]["Uploaddrawing"].ToString();   //Upload Drawing file 
                    if (ds.Tables[0].Rows[i]["Uploaddrawing"].ToString() == "0" || ds.Tables[0].Rows[i]["Uploaddrawing"].ToString() == string.Empty)
                    {
                        projectdata += "";
                    }
                    else
                    {
                        projectdata += "<img src = '/assets/images/" + ds.Tables[0].Rows[i]["Uploaddrawing"].ToString() + "'";
                    }
                    

                    projectdata += "</td>";
                }




                projectdata += "</tr>";
            }
        }

        projectdata += "</tbody>";
        projectdata += "</table>";

        return projectdata;
    }

    [WebMethod]
    public static string bindProjectdata_searchstatus(string RecordId = "0")
    {
        string retval = get_status_listbind();
        string strvalprority = get_Priority_listbind();
        string projectdata = "";
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        SqlParameter[] prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[1] = new SqlParameter("@RecordId", RecordId);
        DataSet ds = da.GetDataSet("Pms_Search_status_dashbord", prm);
        projectdata += "<table cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm datatable'>";
        projectdata += "<thead>";
        projectdata += "<tr>";

        //projectdata += "<th>Select</th>";
        projectdata += "<th>S.No</th>";
        projectdata += "<th>Project ID</th>";
        projectdata += "<th>Project Name</th>";
        projectdata += "<th>Owner</th>";
        projectdata += "<th>Team</th>";
        projectdata += "<th>Status</th>";
        projectdata += "<th>Priority</th>";
        projectdata += "<th>Budget</th>";
        projectdata += "<th>Actual</th>";
        projectdata += "<th>Start Date</th>";
        projectdata += "<th>Target Date</th>";
        projectdata += "<th>Timeline</th>";
        projectdata += "<th>Completion Status</th>";
        projectdata += "<th>Milestone</th>";
        projectdata += "<th>Task</th>";
        projectdata += "<th>Issues</th>";
        projectdata += "<th>File</th>";
        projectdata += "<th>Project Address</th>";
        projectdata += "<th>Client Name</th>";
        projectdata += "<th>Award Date</th>";
        projectdata += "<th>Approval of Drawing</th>";
        projectdata += "<th>Drawing Approval Date</th>";
        projectdata += "<th>Action</th>";
        projectdata += "</tr>";
        projectdata += "</thead>";
        projectdata += "<tbody>";
        if (ds.Tables[0].Rows.Count > 0)
        {


            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {

                string cntownr = "0";
                string usersdetailsowner = "";
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



                projectdata += "<tr>";

                projectdata += "<td valign='top'>";
                projectdata += (i + 1);
                projectdata += "</td>";

                projectdata += "<td valign='top'>";
                projectdata += ds.Tables[0].Rows[i]["ProjectID"].ToString();
                projectdata += "</td>";

                projectdata += "<td valign='top'>";
                projectdata += ds.Tables[0].Rows[i]["ProjectName"].ToString();
                projectdata += "</td>";

                //Owner

                //projectdata += "<td>";
                //projectdata += "<a href ='javascript: void(0);' class='d-flex align-items-center' type='button' id='btnownr_" + ds.Tables[0].Rows[i]["id"].ToString() + "'   onclick='Binddetails_Owner(" + ds.Tables[0].Rows[i]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#changeowner'>";
                //projectdata += "<div class='avatar-xxs flex-shrink-0 me-1' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='" + get_team(ds.Tables[0].Rows[i]["Owner"].ToString()) + "'>";
                //projectdata += "<div class='rounded-circle orange text-white text-center'>";
                //projectdata += "<span>"+ get_employee_firsttwo(ds.Tables[0].Rows[i]["Owner"].ToString()) + "</span>";
                //projectdata += "</div>";
                //projectdata += "</div>";
                //projectdata += "<div class='flex-grow-1 text-black'>";
                //projectdata += get_team(ds.Tables[0].Rows[i]["Owner"].ToString());
                //projectdata += "</div>";
                //projectdata += "</a>";
                //projectdata += "</td>";


                projectdata += "<td>";
                projectdata += "<div class='d-flex'>";
                projectdata += "<div class='flex-grow-1'>";
                projectdata += "<div class='avatar-group' id='lblowner_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>";
                projectdata += usersdetailsowner;

                //projectdata += "</a>";
                projectdata += "<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='more' data-bs-original-title='more'>";

                if (cntownr != "" && cnt != "0")
                {
                    projectdata += "<div class='avatar-xxs'>";
                    projectdata += "<div class='avatar-title rounded-circle'>";

                    projectdata += (cntownr + "+");
                    projectdata += "</div>";
                    projectdata += "</div>";
                }

                projectdata += "</a>";
                projectdata += "</div>";
                projectdata += "</div>";
                projectdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                projectdata += "<a href ='javascript:void(0);' type='button' id='btnownr_" + ds.Tables[0].Rows[i]["id"].ToString() + "' onclick='Binddetails_Owner(" + ds.Tables[0].Rows[i]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#changeowner'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Owner'>+</span></a>";
                projectdata += "</div>";
                projectdata += "</div>";
                projectdata += "</td>";



                //team
                projectdata += "<td>";
                projectdata += "<div class='d-flex'>";
                projectdata += "<div class='flex-grow-1'>";
                projectdata += "<div class='avatar-group' id='lbluser_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>";
                projectdata += usersdetails;

                //projectdata += "</a>";
                projectdata += "<a href = 'javascript: void(0);' class='avatar-group-item' data-bs-toggle='tooltip' data-bs-trigger='hover' data-bs-placement='top' title='more' data-bs-original-title='more'>";

                if (cnt != "" && cnt != "0")
                {
                    projectdata += "<div class='avatar-xxs'>";
                    projectdata += "<div class='avatar-title rounded-circle'>";

                    projectdata += (cnt + "+");
                    projectdata += "</div>";
                    projectdata += "</div>";
                }

                projectdata += "</a>";
                projectdata += "</div>";
                projectdata += "</div>";
                projectdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                projectdata += "<a href ='javascript:void(0);' type='button' id='btnteam_" + ds.Tables[0].Rows[i]["id"].ToString() + "' onclick='Binddetails_Team(" + ds.Tables[0].Rows[i]["id"].ToString() + ",this.id)' data-bs-toggle='modal' data-bs-target='#addteam'><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Team'>+</span></a>";
                projectdata += "</div>";
                projectdata += "</div>";
                projectdata += "</td>";



                // status

                string strvalcolor = get_Status_Color(ds.Tables[0].Rows[i]["ProjectStatus"].ToString());
                projectdata += "<td>";
                projectdata += "<div class='btn-group d-block'>";
                projectdata += "<a href ='javascript:void(0);' id='btnsts_" + ds.Tables[0].Rows[i]["id"].ToString() + "' type='button' onclick=setprojectid('" + ds.Tables[0].Rows[i]["id"].ToString() + "',this.id) data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + strvalcolor + "'>" + get_Projectstatus(ds.Tables[0].Rows[i]["ProjectStatus"].ToString()) + "</a>";
                projectdata += "<div class='dropdown-menu lead_form_body p-0'>";
                projectdata += retval;
                projectdata += "</div>";
                projectdata += "</div>";
                projectdata += "</td>";

                //priority
                string strprioritycolor = get_PriorityColor(ds.Tables[0].Rows[i]["Priority"].ToString());
                projectdata += "<td>";
                projectdata += "<div class='btn-group d-block'>";
                projectdata += "<a href ='javascript:void(0);'  id='btnpri_" + ds.Tables[0].Rows[i]["id"].ToString() + "' type='button' onclick=setpriorityid('" + ds.Tables[0].Rows[i]["id"].ToString() + "',this.id) data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style='background-color:" + strprioritycolor + "'>" + get_PriorityName(ds.Tables[0].Rows[i]["Priority"].ToString()) + "</a>";
                projectdata += "<div class='dropdown-menu lead_form_body p-0'>";
                projectdata += strvalprority;
                projectdata += "</div>";
                projectdata += "</div>";
                projectdata += "</td>";



                projectdata += "<td valign='top'>";
                projectdata += ds.Tables[0].Rows[i]["Budget"].ToString(); // budget 
                projectdata += "</td>";

                projectdata += "<td valign='top'>";
                projectdata += ds.Tables[0].Rows[i]["Actual"].ToString(); // Actual 
                projectdata += "</td>";

                projectdata += "<td valign='top'>";
                projectdata += cmf.dateConvert(ds.Tables[0].Rows[i]["StartDate"].ToString());
                projectdata += "</td>";

                projectdata += "<td valign='top'>";
                projectdata += cmf.dateConvert(ds.Tables[0].Rows[i]["TargetDate"].ToString());
                projectdata += "</td>";

                projectdata += "<td></td>";
                projectdata += "<td></td>";


                //projectdata += "<td valign='top'>";
                //projectdata += ""; // milsetone
                //projectdata += "</td>";

                //projectdata += "<td valign='top'>";
                //projectdata += "<div class=gap - 1 flex - shrink - 0 align - self - center>";
                //projectdata += "<a href=javascript: void(0); type=button data-bs-toggle=offcanvas data-bs-target=#canvas_popup onclick=loadsrc('inner_popup','/milestone_popup.aspx')><span class=btn-plus2 data-bs-toggle=tooltip data-bs-placement=top data-bs-trigger=hover title=+ Add Milestone>+</span></a>";
                //projectdata += "</div>";
                //projectdata += "</td>";


                projectdata += "<td valign='top'>";
                projectdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                projectdata += "<a href ='javascript:void(0);' type ='button' data-bs-toggle ='offcanvas' data-bs-target =#canvas_popup onclick =loadsrc('inner_popup','/AddmilstonePopup.aspx?id=" + ds.Tables[0].Rows[i]["id"].ToString() + "&type=Project')><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Milestone'>+</span></a>";
                projectdata += "</div>";
                projectdata += "</td>";

                projectdata += "<td valign='top'>";
                projectdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                projectdata += "<a href ='javascript:void(0);' type='button' data-bs-toggle='offcanvas' data-bs-target=#canvas_popup onclick=loadsrc('inner_popup','/AddTaskPopup.aspx?id=" + ds.Tables[0].Rows[i]["id"].ToString() + "&type=Project')><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Task'>+</span></a>";
                projectdata += "</div>";
                projectdata += "</td>";

                projectdata += "<td valign='top'>";
                projectdata += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                projectdata += "<a href ='javascript:void(0);' type='button' data-bs-toggle='offcanvas' data-bs-target=#canvas_popup onclick=loadsrc('inner_popup','/AddIssuesPopup.aspx?id=" + ds.Tables[0].Rows[i]["id"].ToString() + "&type=Project')><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Issue'>+</span></a>";
                projectdata += "</div>";
                projectdata += "</td>";

                projectdata += "<td valign='top'>";
                // projectdata += ds.Tables[0].Rows[i]["Uploaddrawing"].ToString();  // file upload image
                projectdata += "";  // file upload image
                projectdata += "</td>";

                projectdata += "<td valign='top'>";
                projectdata += ds.Tables[0].Rows[i]["ProjectAddress1"].ToString() + " " + ds.Tables[0].Rows[i]["ProjectAddress2"].ToString() + " " + ds.Tables[0].Rows[i]["City"].ToString() + "</br> " + ds.Tables[0].Rows[i]["PostalCode"].ToString() + " " + get_statename(ds.Tables[0].Rows[i]["StateName"].ToString()) + " " + get_Countryname(ds.Tables[0].Rows[i]["Countryid"].ToString());
                projectdata += "</td>";

                projectdata += "<td valign='top'>";
                projectdata += ds.Tables[0].Rows[i]["ClientName"].ToString();
                projectdata += "</td>";

                projectdata += "<td valign='top'>";
                projectdata += cmf.dateConvert(ds.Tables[0].Rows[i]["AwardDate"].ToString());
                projectdata += "</td>";

                string strret = "";
                if (ds.Tables[0].Rows[i]["ApprovalDrawing"].ToString() == "1")
                {
                    strret = "Yes";

                }

                if (ds.Tables[0].Rows[i]["ApprovalDrawing"].ToString() == "2")
                {
                    strret = "No";

                }

                projectdata += "<td valign='top'>";
                projectdata += strret;
                projectdata += "</td>";

                projectdata += "<td valign='top'>";
                projectdata += cmf.dateConvert(ds.Tables[0].Rows[i]["DrawingApprovalDate"].ToString());
                projectdata += "</td>";


                //projectdata += "<td valign='top'>";
                //projectdata += "";
                //projectdata += "</td>";

                projectdata += " <td><a href='javascript:void(0);' Class='btn btn-info btn-sm' type='button' onclick='Binddetails(" + ds.Tables[0].Rows[i]["id"].ToString() + ")' data-bs-toggle='offcanvas' data-bs-target='#addproject'><i class='ri-edit-line'></i></a>";
                projectdata += "&nbsp;&nbsp;<a href='javascript:void(0);' Class='btn btn-danger btn-sm' type='button' onclick='deletedata(" + ds.Tables[0].Rows[i]["id"].ToString() + ")'><i class='ri-delete-bin-5-line'></i></a>";
                projectdata += "</td>";


                projectdata += "</tr>";
            }
        }

        projectdata += "</tbody>";
        projectdata += "</table>";

        return projectdata;
    }

    [WebMethod]
    public static string Bindprojectdetail(int RecordId = 0)
    {

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "select * from Pms_tbl_AddProject where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
        ds = da.GetDataSetInline(strsql);


        return ds.GetXml();
    }

    [WebMethod]
    public static string Bindprojectdetail_Owner(int RecordId = 0)
    {

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "select * from Pms_tbl_AddProject where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
        ds = da.GetDataSetInline(strsql);


        return ds.GetXml();
    }


    [WebMethod]
    public static string Bindprojectdetail_extendatedate(int RecordId = 0)
    {

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "select * from Pms_tbl_AddProject where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
        ds = da.GetDataSetInline(strsql);


        return ds.GetXml();
    }

    [WebMethod]
    public static string Bindprojectdetail_Team(int RecordId = 0)
    {

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "select * from Pms_tbl_AddProject where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
        ds = da.GetDataSetInline(strsql);


        return ds.GetXml();
    }

    [WebMethod]
    public static string Deleteprojectdata(int RecordId = 0)
    {
        string retval = "0";

        DataAccess da = new DataAccess();
        DataSet ds;
        string strsql = "";

        strsql = "update  Pms_tbl_AddProject set  status=0,archive=1  where  id='" + RecordId + "' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        ds = da.GetDataSetInline(strsql);

        strsql = "update  Pms_tbl_AddMilestone set  status=0,archive=1  where  ProjectId='" + RecordId + "' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        ds = da.GetDataSetInline(strsql);

        strsql = "update  Pms_tbl_Addtask set  status=0,archive=1  where  ProjectId='" + RecordId + "' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        ds = da.GetDataSetInline(strsql);

        strsql = "update  Pms_tbl_Addsubtask set  status=0,archive=1  where  ProjectId='" + RecordId + "' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        ds = da.GetDataSetInline(strsql);

        strsql = "update  Pms_tbl_Addissues set  status=0,archive=1  where  ProjectId='" + RecordId + "' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        ds = da.GetDataSetInline(strsql);

        return retval;
    }


    public static string get_employee(string ownerid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = "select * from tbl_employee where status=1 and archive=0 and id='" + ownerid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["DisplayName"].ToString();

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
                    "<a class='' href='javascript:void(0);' onclick = 'UpdateDataStatusdeatils(" + dscode.Tables[0].Rows[i]["id"].ToString() + "," + dscode.Tables[0].Rows[i]["iscomplete"].ToString() + ")' > " +
                    "<i class='bi bi-square-fill' style='color:"+ dscode.Tables[0].Rows[i]["PrintColor"].ToString() + "' ></i> "+ dscode.Tables[0].Rows[i]["ProjectStatus"].ToString()+ "" +
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

    public static string get_statename(string stateid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Imt_state where status=1 and archive=0 and stateid='" + stateid + "'  ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["state"].ToString();

        }

        return strret;
    }

    protected static string get_tagkname(string str1)
    {
        DataAccess da = new DataAccess();
        string str = str1;
        string[] splitstr = str.Split(',');
        string stname = "";
        for (int i = 0; i < splitstr.Length; i++)
        {
            string strsql = "select * from Pms_tbl_Addtags where id = '" + splitstr[i] + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0";
            DataSet ds2 = da.GetDataSetInline(strsql);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                if (ds2.Tables[0].Rows[0]["Tags"].ToString() != string.Empty)
                {
                    if (stname != string.Empty)
                    {
                        stname += "," + ds2.Tables[0].Rows[0]["Tags"].ToString();
                    }
                    else
                    {
                        stname += ds2.Tables[0].Rows[0]["Tags"].ToString();
                    }
                }

            }
        }
        return stname;
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



    protected void btnupload_Click(object sender, EventArgs e)
    {

        //if (FileUpload1.PostedFile != null)
        //{
        //    try
        //    {
        //        string path = string.Concat(Server.MapPath("/crm/Upload_Files/" + FileUpload1.FileName));
        //        FileInfo file = new FileInfo(path);
        //        if (file.Exists)//check file exsit or not  
        //        {
        //            file.Delete();
        //        }
        //        FileUpload1.SaveAs(path);
                
        //        string excelCS = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'", path);
        //          using (OleDbConnection con = new OleDbConnection(excelCS))
        //        {
        //            string Query = string.Format("Select * FROM [{0}]", "Sheet1$");

        //            DataSet ds = new DataSet();
        //            OleDbDataAdapter oda = new OleDbDataAdapter(Query, con);
        //            con.Close();
        //            oda.Fill(ds);
        //            if (ds.Tables[0].Rows.Count > 0)
        //            {
        //                SqlParameter[] prm;
        //                DataAccess da1 = new DataAccess();
        //                string editid = "0";
        //                string entrytype = "Insert";
        //                string Salutation = "0";
        //                string LeadStatus = "0";
        //                string OwnerName = "0";
        //                string Countryid = "0";
        //                string LeadSourceid = "0";
        //                string IndustryTypeid = "0";

        //                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //                {
        //                    try
        //                    {
        //                        if (ds.Tables[0].Rows[i]["Salutation"].ToString() != "" && ds.Tables[0].Rows[i]["Salutation"].ToString() != "0")
        //                        {
        //                            Salutation = (ds.Tables[0].Rows[i]["Salutation"].ToString() == "Mr." ? "1" : ds.Tables[0].Rows[i]["Salutation"].ToString() == "Ms." ? "2" : ds.Tables[0].Rows[i]["Salutation"].ToString() == "Mrs." ? "3" : ds.Tables[0].Rows[i]["Salutation"].ToString() == "Dr." ? "4" : ds.Tables[0].Rows[i]["Salutation"].ToString() == "Prof." ? "5" : ds.Tables[0].Rows[i]["Salutation"].ToString() == "Mx." ? "6" : "0");
        //                        }
        //                    }
        //                    catch
        //                    {
        //                        Salutation = "0";
        //                    }
        //                    try
        //                    {
        //                        if (ds.Tables[0].Rows[i]["LeadStatus"].ToString() != "" && ds.Tables[0].Rows[i]["LeadStatus"].ToString() != "0")
        //                        {
        //                            string strleadstatus = "select id,StatusName  from tbl_leadstatus where StatusName='" + ds.Tables[0].Rows[i]["LeadStatus"].ToString().Trim() + "'";
        //                            DataSet dschk1 = da.GetDataSetInline(strleadstatus);
        //                            if (dschk1.Tables[0].Rows.Count > 0)
        //                            {
        //                                LeadStatus = dschk1.Tables[0].Rows[0]["id"].ToString();
        //                            }
        //                            else
        //                            {
        //                                LeadStatus = "0";
        //                            }
        //                        }
        //                    }
        //                    catch
        //                    {
        //                        LeadStatus = "0";
        //                    }
        //                    try
        //                    {
        //                        if (ds.Tables[0].Rows[i]["OwnerName"].ToString() != "" && ds.Tables[0].Rows[i]["OwnerName"].ToString() != "0")
        //                        {
        //                            string strleadstatus = "select id,DisplayName  from tbl_employee where DisplayName='" + ds.Tables[0].Rows[i]["OwnerName"].ToString().Trim() + "'";
        //                            DataSet dschk2 = da.GetDataSetInline(strleadstatus);
        //                            if (dschk2.Tables[0].Rows.Count > 0)
        //                            {
        //                                OwnerName = dschk2.Tables[0].Rows[0]["id"].ToString();
        //                            }
        //                            else
        //                            {
        //                                OwnerName = Request.Cookies["user"]["user_id"];
        //                            }
        //                        }
        //                        else
        //                        {
        //                            OwnerName = Request.Cookies["user"]["user_id"];
        //                        }
        //                    }
        //                    catch
        //                    {
        //                        OwnerName = Request.Cookies["user"]["user_id"];
        //                    }
        //                    try
        //                    {
        //                        if (ds.Tables[0].Rows[i]["CountryName"].ToString() != "" && ds.Tables[0].Rows[i]["CountryName"].ToString() != "0")
        //                        {
        //                            string strleadstatus = "select id,countryname  from tbl_countrymaster where countryname='" + ds.Tables[0].Rows[i]["CountryName"].ToString().Trim() + "'";
        //                            DataSet dschk3 = da.GetDataSetInline(strleadstatus);
        //                            if (dschk3.Tables[0].Rows.Count > 0)
        //                            {
        //                                Countryid = dschk3.Tables[0].Rows[0]["id"].ToString();
        //                            }
        //                            else
        //                            {
        //                                Countryid = "0";
        //                            }
        //                        }
        //                    }
        //                    catch
        //                    {
        //                        Countryid = "0";
        //                    }
        //                    try
        //                    {
        //                        if (ds.Tables[0].Rows[i]["LeadSourceid"].ToString() != "" && ds.Tables[0].Rows[i]["LeadSourceid"].ToString() != "0")
        //                        {
        //                            string strleadstatus = "select id,Name  from tbl_crmmasterdata where Name='" + ds.Tables[0].Rows[i]["LeadSourceid"].ToString().Trim() + "'";
        //                            DataSet dschk3 = da.GetDataSetInline(strleadstatus);
        //                            if (dschk3.Tables[0].Rows.Count > 0)
        //                            {
        //                                LeadSourceid = dschk3.Tables[0].Rows[0]["id"].ToString();
        //                            }
        //                            else
        //                            {
        //                                LeadSourceid = "0";
        //                            }
        //                        }
        //                    }
        //                    catch
        //                    {
        //                        LeadSourceid = "0";
        //                    }
        //                    try
        //                    {
        //                        if (ds.Tables[0].Rows[i]["IndustryTypeid"].ToString() != "" && ds.Tables[0].Rows[i]["IndustryTypeid"].ToString() != "0")
        //                        {
        //                            string strleadstatus = "select id,Name  from tbl_crmmasterdata where Name='" + ds.Tables[0].Rows[i]["IndustryTypeid"].ToString().Trim() + "'";
        //                            DataSet dschk4 = da.GetDataSetInline(strleadstatus);
        //                            if (dschk4.Tables[0].Rows.Count > 0)
        //                            {
        //                                IndustryTypeid = dschk4.Tables[0].Rows[0]["id"].ToString();
        //                            }
        //                            else
        //                            {
        //                                IndustryTypeid = "0";
        //                            }
        //                        }
        //                    }
        //                    catch
        //                    {
        //                        IndustryTypeid = "0";
        //                    }

        //                    prm = new SqlParameter[29];
        //                    prm[0] = new SqlParameter("@id", editid);
        //                    prm[1] = new SqlParameter("@type", entrytype);
        //                    prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        //                    prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
        //                    prm[4] = new SqlParameter("@cdate", da1.getcdate());
        //                    prm[5] = new SqlParameter("@Salutation", Salutation);
        //                    prm[6] = new SqlParameter("@FirstName", ds.Tables[0].Rows[i]["FirstName"].ToString());
        //                    prm[7] = new SqlParameter("@LastName", ds.Tables[0].Rows[i]["LastName"].ToString());
        //                    prm[8] = new SqlParameter("@Company", ds.Tables[0].Rows[i]["Company"].ToString());
        //                    prm[9] = new SqlParameter("@Title", ds.Tables[0].Rows[i]["Designation"].ToString());
        //                    prm[10] = new SqlParameter("@Website", ds.Tables[0].Rows[i]["Website"].ToString());
        //                    prm[11] = new SqlParameter("@Descriptions", ds.Tables[0].Rows[i]["Descriptions"].ToString());
        //                    prm[12] = new SqlParameter("@LeadStatus", LeadStatus);
        //                    prm[13] = new SqlParameter("@OwnerName", OwnerName);
        //                    prm[14] = new SqlParameter("@PhoneNo", ds.Tables[0].Rows[i]["PhoneNo"].ToString());
        //                    prm[15] = new SqlParameter("@EmailId", ds.Tables[0].Rows[i]["EmailId"].ToString());
        //                    prm[16] = new SqlParameter("@LAddress", ds.Tables[0].Rows[i]["LAddress"].ToString());
        //                    prm[17] = new SqlParameter("@Street", ds.Tables[0].Rows[i]["Street"].ToString());
        //                    prm[18] = new SqlParameter("@CityId", "0");
        //                    prm[19] = new SqlParameter("@CityName", ds.Tables[0].Rows[i]["CityName"].ToString());
        //                    prm[20] = new SqlParameter("@PostalCode", ds.Tables[0].Rows[i]["PostalCode"].ToString());
        //                    prm[21] = new SqlParameter("@StateId", "0");
        //                    prm[22] = new SqlParameter("@StateName", ds.Tables[0].Rows[i]["StateName"].ToString());
        //                    prm[23] = new SqlParameter("@CountryId", Countryid);
        //                    prm[24] = new SqlParameter("@CountryName", ds.Tables[0].Rows[i]["CountryName"].ToString());
        //                    prm[25] = new SqlParameter("@NoOfEmployee", ds.Tables[0].Rows[i]["NoOfEmployee"].ToString());
        //                    prm[26] = new SqlParameter("@AnnualRevenue", ds.Tables[0].Rows[i]["AnnualRevenue"].ToString());
        //                    prm[27] = new SqlParameter("@LeadSourceid", LeadSourceid);
        //                    prm[28] = new SqlParameter("@IndustryTypeid", IndustryTypeid);
        //                    da1.executeDMLQuery("crm_tbl_mainleads_Insert", prm);

        //                    Salutation = "0";
        //                    LeadStatus = "0";
        //                    OwnerName = "0";
        //                    Countryid = "0";
        //                    LeadSourceid = "0";
        //                    IndustryTypeid = "0";

        //                }

        //                if (file.Exists)//check file exsit or not  
        //                {
        //                    file.Delete();

        //                }
        //                Page.RegisterClientScriptBlock("js", "<script>alert('" + fns.getltext("Data Saved Successfully!!", "2") + "');window.parent.location.href ='/crm/lead.aspx?leadtype=" + cmf.Encrypt(selectleadtype) + "&presearch=" + cmf.Encrypt(selecttype) + "'>';</script>");
                        
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
               
        //    }
        //}
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
                filename = "a" + filename;
                if (File.Exists(Server.MapPath("/Image/" + filename)))
                {
                    goto chk;
                }
                Uploaddrawing.SaveAs(Server.MapPath("/Image/" + filename));
                hdnUploaddrawing.Value = filename;

            }
            else
            {

                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('Please choose only .jpg, .png, .bmp and .gif image types!')", true);


            }
        }
    }

    protected void Countryid_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindState();
    }


    // column hide show //


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
        prm[4] = new SqlParameter("@type", "Project");
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

    public void columnshowhide()
    {

        HtmlAnchor[] linka = new[] { proj0, proj1, proj2, proj3, proj4, proj5, proj6, proj7, proj8, proj9, proj10, proj11, proj12, proj13, proj14, proj15, proj16, proj17, proj18, proj19, proj20,proj21,proj22, proj23, proj24 };

        HiddenField[] hdn = new[] { hdn0, hdn1, hdn2, hdn3, hdn4, hdn5, hdn6, hdn7, hdn8, hdn9, hdn10, hdn11, hdn12, hdn13, hdn14, hdn15, hdn16, hdn17, hdn18, hdn19, hdn20, hdn21, hdn22, hdn23, hdn24 };

        string str = " select * from Pms_tbl_showhide_column where status=1 and archive=0 and uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"] + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and type='Project'";
        DataSet ds = da.GetDataSetInline(str);
        // Response.Write(str);
        if (ds.Tables[0].Rows.Count > 0)
        {

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {


                if (linka[Convert.ToInt32(ds.Tables[0].Rows[i]["columnnameid"].ToString())] != null)
                {

                    if (ds.Tables[0].Rows[i]["isshow"].ToString() == "0")
                    {
                        linka[Convert.ToInt32(ds.Tables[0].Rows[i]["columnnameid"].ToString())].Attributes.Add("class", "dropdown-item toggle-vis text-mute");
                        hdn[Convert.ToInt32(ds.Tables[0].Rows[i]["columnnameid"].ToString())].Value = "0";
                    }

                }
            }


           
        }

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "txt", "hidecolumn(); hidecolumngrid();", true);



    }

}
public class projectdata
{
    public string id { get; set; }
    public string ProjectID { get; set; }
    public string ProjectName { get; set; }
    public string Owner { get; set; }
    public string team { get; set; }
    public string ProjectStatus { get; set; }
    public string ProjectAddress1 { get; set; }
    public string ProjectAddress2 { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
    public string StateName { get; set; }
    public string Countryid { get; set; }
    public string ClientName { get; set; }
    public string Priority { get; set; }
    public string Budget { get; set; }
    public string Actual { get; set; }
    public string AwardDate { get; set; }
    public string StartDate { get; set; }
    public string TargetDate { get; set; }
    public string ApprovalDrawing { get; set; }
    public string DrawingApprovalDate { get; set; }
    public string Uploaddrawing { get; set; }
    public string Weightage { get; set; }
    public string isdrawing { get; set; }
    public string checkprojectWeightage { get; set; }
}

   


