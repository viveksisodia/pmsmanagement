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

public partial class Add_Task : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataAccess da = new DataAccess();
    public CommonFunction cmf = new CommonFunction();
    SqlParameter[] prm, prm1;
    public function fns = new function();
    public string taskdata = "";
    public string Multiteam = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {

            bindtasklist();
            bindemployee();
            bindMultiteam();





        }
    }


    protected void bindtasklist()
    {
        string strleadstatus = "select id,TaskList  from Pms_tbl_Addtasklist where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  order by id";
        DataSet ds = da.GetDataSetInline(strleadstatus);
        if (ds.Tables[0].Rows.Count > 0)
        {
            cmf.BindDropDownList(tasklist, ds, "TaskList", "id");
            ListItem li1 = new ListItem("", "0");
            tasklist.Items.Insert(0, li1);
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
  




    protected void Button1_Click(object sender, EventArgs e)
    {

        try
        {
            tasklist.SelectedValue = hdntasklist.Value;
        }
        catch
        {

        }

        try
        {
            owner.SelectedValue = hdnowner.Value;
        }
        catch
        {

        }

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

        try
        {
            Multiteam = hdnteam.Value;
        }
        catch
        {

        }




    }


    //// web method bind using ajax start //

    [WebMethod]
    public static void SaveTask(Taskdata taskdetails)
    {
        DataAccess da1 = new DataAccess();
        string editid = taskdetails.id;
        string entrytype = "Insert";
        if (editid != "" && editid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[17];
        prm[0] = new SqlParameter("@id", editid);
        prm[1] = new SqlParameter("@tasklist", taskdetails.tasklist);
        prm[2] = new SqlParameter("@taskname", taskdetails.taskname);
        prm[3] = new SqlParameter("@taskdescripton", taskdetails.taskdescripton);
        prm[4] = new SqlParameter("@owner", taskdetails.owner);
        if (taskdetails.Startdate == string.Empty)
        {
            prm[5] = new SqlParameter("@Startdate", DBNull.Value);
        }
        else
        {
            prm[5] = new SqlParameter("@Startdate", taskdetails.Startdate);
        }
        if (taskdetails.Targetdate == string.Empty)
        {
            prm[6] = new SqlParameter("@Targetdate", DBNull.Value);
        }
        else
        {
            prm[6] = new SqlParameter("@Targetdate", taskdetails.Targetdate);
        }
        prm[7] = new SqlParameter("@P_Status", taskdetails.P_Status);
        prm[8] = new SqlParameter("@Priority", taskdetails.Priority);
        prm[9] = new SqlParameter("@Reminder", taskdetails.Reminder);
        prm[10] = new SqlParameter("@Occurrence", taskdetails.Occurrence);
        prm[11] = new SqlParameter("@tagto", taskdetails.tagto);
        prm[12] = new SqlParameter("@team", taskdetails.team);
        prm[13] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[14] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[15] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[16] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pms_Adtask_usp", prm);
    }

    [WebMethod]
    public static string bindtaskdata()
    {
        string taskdata = "";
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        DataSet ds = da.GetDataSet("Pms_Bindtask_dashbord", prm);
        taskdata += "<table cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm datatable'>";
        taskdata += "<thead>";
        taskdata += "<tr>";

        //taskdata += "<th>Select</th>";
        taskdata += "<th>S.No</th>";
        taskdata += "<th style=min-width:210px;>Task</th>";
        taskdata += "<th style=min-width:130px;>Team</th>";
        taskdata += "<th style=width:115px;>Status</th>";
        taskdata += "<th>Priority</th>";
        taskdata += "<th>Budget</th>";
        taskdata += "<th>Actual</th>";
        taskdata += "<th>Start Date</th>";
        taskdata += "<th>Target Date</th>";
        taskdata += "<th style=min-width:140px;>Timeline</th>";
        taskdata += "<th style=min-width:140px;>Completion Status</th>";
        taskdata += "<th style=min-width:140px;>Sub Task</th>";
        taskdata += "<th style=min-width:140px;>Issues</th>";
        taskdata += "<th>File</th>";
        taskdata += "<th>Action</th>";
        taskdata += "</tr>";
        taskdata += "</thead>";
        taskdata += "<tbody>";
        if (ds.Tables[0].Rows.Count > 0)
        {


            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                taskdata += "<tr>";

                taskdata += "<td valign='top'>";
                taskdata += (i + 1);
                taskdata += "</td>";

                taskdata += "<td valign='top'>";
                taskdata += ds.Tables[0].Rows[i]["taskname"].ToString(); // task
                taskdata += "</td>";
                
                taskdata += "<td valign='top'>";
                taskdata += get_team(ds.Tables[0].Rows[i]["team"].ToString());//team
                taskdata += "</td>";

                taskdata += "<td valign='top'>";
                taskdata += "";//status
                taskdata += "</td>";

                taskdata += "<td valign='top'>";
                taskdata += ""; //priority
                taskdata += "</td>";

                taskdata += "<td valign='top'>";
                taskdata += ""; //budget  
                taskdata += "</td>";

                taskdata += "<td valign='top'>";
                taskdata += ""; //Actual
                taskdata += "</td>";

                taskdata += "<td valign='top'>";
                taskdata += cmf.dateConvert(ds.Tables[0].Rows[i]["StartDate"].ToString()); // start date
                taskdata += "</td>";

                taskdata += "<td valign='top'>";
                taskdata += cmf.dateConvert(ds.Tables[0].Rows[i]["Targetdate"].ToString()); //target date
                taskdata += "</td>";

                taskdata += "<td valign='top'>";
                taskdata += ""; //Timeline
                taskdata += "</td>";
                
                taskdata += "<td valign='top'>";
                taskdata += ""; // Completion Status
                taskdata += "</td>";

                taskdata += "<td valign='top'>";
                taskdata += ""; // sub Task
                taskdata += "</td>";

                taskdata += "<td valign='top'>";
                taskdata += ""; // isssues 
                taskdata += "</td>";

                taskdata += "<td valign='top'>";
                taskdata += "";  // file 
                taskdata += "</td>";

                taskdata += " <td><a href='javascript:void(0);' Class='btn btn-info btn-sm' type='button' onclick='Binddetails(" + ds.Tables[0].Rows[i]["id"].ToString() + ")' data-bs-toggle='offcanvas' data-bs-target='#addtask'><i class='ri-edit-line'></i></a>";
                taskdata += "&nbsp;&nbsp;<a href='javascript:void(0);' Class='btn btn-danger btn-sm' type='button' onclick='deletedata(" + ds.Tables[0].Rows[i]["id"].ToString() + ")'><i class='ri-delete-bin-5-line'></i></a>";
                taskdata += "</td>";


                taskdata += "</tr>";
            }
        }

        taskdata += "</tbody>";
        taskdata += "</table>";

        return taskdata;
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
    public static string Deletetaskdata(int RecordId = 0)
    {
        string retval = "0";

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "update  Pms_tbl_Addtask set  status=0,archive=1  where  id='" + RecordId + "' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
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


}
public class Taskdata
{
    public string id { get; set; }
    public string tasklist { get; set; }
    public string taskname { get; set; }
    public string taskdescripton { get; set; }
    public string owner { get; set; }
    public string Startdate { get; set; }
    public string Targetdate { get; set; }
    public string P_Status { get; set; }
    public string Priority { get; set; }
    public string Reminder { get; set; }
    public string Occurrence { get; set; }
    public string tagto { get; set; }
    public string team { get; set; }
 

}




