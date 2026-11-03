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

public partial class AddMDependency : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataAccess da = new DataAccess();
    public CommonFunction cmf = new CommonFunction();
    SqlParameter[] prm, prm1;
    public function fns = new function();
    public string milestonenamename = "";
    public string Projectname = "";
    public string MultiMilestone = "";
    public string MultiTask = "";
    public string MultiSubtask = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        bindmilestonenew("");
        if (!Page.IsPostBack)
        {

            if (HttpContext.Current.Request.QueryString["pid"] != null)
            {
                hdnpid.Value = HttpContext.Current.Request.QueryString["pid"].ToString();
            }
            if (HttpContext.Current.Request.QueryString["id"] != null)
            {
                hdnmid.Value = HttpContext.Current.Request.QueryString["id"].ToString();
            }
            if (hdnmid.Value != null)
            {
                bindmilestonename();
            }
            if (hdnpid.Value != null)
            {
                bindprojectname();
            }
            bindmilestone(hdnpid.Value, hdnmid.Value);
            bindProject();
            
            bindmilestonenetask(hdnmid.Value);
        }
    }
    protected void bindmilestone(string pid,string mid)
    {
        string strsql = "select Pms_tbl_AddMilestone.id,(Milestone+'-'+Pms_tbl_AddProject.ProjectName) name from Pms_tbl_AddMilestone " +
            " left join Pms_tbl_AddProject on Pms_tbl_AddMilestone.ProjectId=Pms_tbl_AddProject.id where Pms_tbl_AddMilestone.status=1 and Pms_tbl_AddMilestone.archive=0 and Pms_tbl_AddMilestone.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and Pms_tbl_AddMilestone.ProjectId='" + pid + "'";
        DataSet ds = da.GetDataSetInline(strsql);
        cmf.BindDropDownList(ddlMilestone, ds, "name", "id");
        ListItem li2 = new ListItem("", "0");
        ddlMilestone.Items.Insert(0, li2);
        try
        {
            ddlMilestone.SelectedValue = mid;
        }
        catch { }


    }
    protected void bindmilestonenew(string pid="")
    {
        string strsql = "select Pms_tbl_AddMilestone.id,Milestone name from Pms_tbl_AddMilestone " +
            " left join Pms_tbl_AddProject on Pms_tbl_AddMilestone.ProjectId=Pms_tbl_AddProject.id where Pms_tbl_AddMilestone.status=1 and Pms_tbl_AddMilestone.archive=0 and Pms_tbl_AddMilestone.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and Pms_tbl_AddMilestone.ProjectId='" + pid + "'";
        if(pid==""|| pid=="0")
        {
            strsql = "select Pms_tbl_AddMilestone.id,Milestone name from Pms_tbl_AddMilestone " +
                        " left join Pms_tbl_AddProject on Pms_tbl_AddMilestone.ProjectId=Pms_tbl_AddProject.id where Pms_tbl_AddMilestone.status=1 and Pms_tbl_AddMilestone.archive=0 and Pms_tbl_AddMilestone.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";

        }

        DataSet ds = da.GetDataSetInline(strsql);
        //cmf.BindDropDownList(ddldmid, ds, "name", "id");
        //ListItem li2 = new ListItem("", "0");
        //ddldmid.Items.Insert(0, li2);
        //try
        //{
        //    if (hdnmid.Value != "" && hdnmid.Value != "0")
        //    {
        //        ddldmid.SelectedValue = hdnmid.Value;
        //    }
        
            
        //}
        //catch { }
        string dataN = "";
        for (int i1 = 0; i1 < ds.Tables[0].Rows.Count; i1++)
        {
            if (dataN != string.Empty)
            {
                dataN += ",{";
                dataN += "id: '" + ds.Tables[0].Rows[i1]["id"].ToString() + "',";
                dataN += "text: '" + ds.Tables[0].Rows[i1]["name"].ToString().Replace("'", "`") + "'";
                dataN += "}";
            }
            else
            {
                dataN += "{";
                dataN += "id: '" + ds.Tables[0].Rows[i1]["id"].ToString() + "',";
                dataN += "text: '" + ds.Tables[0].Rows[i1]["name"].ToString().Replace("'", "`") + "'";
                dataN += "}";
            }
        }
        MultiMilestone = dataN;

    }
    protected void bindmilestonenetask(string mid="")
    {
        string strsql = "select Pms_tbl_Addtask.id,Pms_tbl_Addtask.taskname name from Pms_tbl_Addtask" +
            " left join Pms_tbl_AddMilestone on Pms_tbl_Addtask.Milestoneid=Pms_tbl_AddMilestone.id " +
            " left join Pms_tbl_AddProject on Pms_tbl_AddMilestone.ProjectId=Pms_tbl_AddProject.id where Pms_tbl_Addtask.status=1 and Pms_tbl_Addtask.archive=0 and Pms_tbl_Addtask.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";

        if (mid != "" && mid != "0")
        {
            strsql += " and Pms_tbl_Addtask.Milestoneid='" + mid + "' and Pms_tbl_Addtask.taskname IS NOT NULL and Pms_tbl_Addtask.taskname<> ' ' ";

           
        }
        DataSet ds = da.GetDataSetInline(strsql);
        //cmf.BindDropDownList(taskid, ds, "name", "id");
        //ListItem li2 = new ListItem("", "0");
        //taskid.Items.Insert(0, li2);
        string dataN = "";
        for (int i1 = 0; i1 < ds.Tables[0].Rows.Count; i1++)
        {
            if (dataN != string.Empty)
            {
                dataN += ",{";
                dataN += "id: '" + ds.Tables[0].Rows[i1]["id"].ToString() + "',";
                dataN += "text: '" + ds.Tables[0].Rows[i1]["name"].ToString().Replace("'", "`") + "'";
                dataN += "}";
            }
            else
            {
                dataN += "{";
                dataN += "id: '" + ds.Tables[0].Rows[i1]["id"].ToString() + "',";
                dataN += "text: '" + ds.Tables[0].Rows[i1]["name"].ToString().Replace("'", "`") + "'";
                dataN += "}";
            }
        }
        MultiTask = dataN;

    }
    [WebMethod]
    public static List<ListItem> bindtasks(string mid)
    {
        DataAccess da1 = new DataAccess();
        // Prepare some dummy data for second dropdown based on the first dropdown value
        List<ListItem> items = new List<ListItem>();
        if (mid != "")
        {
            items.Add(new ListItem("", ""));
        }
        if (mid=="")
        {
            mid = "0";
        }
        string strsql = "select Pms_tbl_Addtask.id,Pms_tbl_Addtask.taskname name from Pms_tbl_Addtask" +
            " left join Pms_tbl_AddMilestone on Pms_tbl_Addtask.Milestoneid=Pms_tbl_AddMilestone.id " +
            " left join Pms_tbl_AddProject on Pms_tbl_AddMilestone.ProjectId=Pms_tbl_AddProject.id where Pms_tbl_Addtask.status=1 and Pms_tbl_Addtask.archive=0 and Pms_tbl_Addtask.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";

       // if (mid != "" && mid != "0")
        //{
            strsql += " and Pms_tbl_Addtask.Milestoneid in(" + mid + ") and Pms_tbl_Addtask.taskname IS NOT NULL and Pms_tbl_Addtask.taskname<> ' ' ";


       // }
        DataSet dsN = da1.GetDataSetInline(strsql);

        string dataN = "";
    
        if (dsN.Tables[0].Rows.Count > 0)
        {

            for (int i1 = 0; i1 < dsN.Tables[0].Rows.Count; i1++)
            {
                items.Add(new ListItem(dsN.Tables[0].Rows[i1]["name"].ToString(), dsN.Tables[0].Rows[i1]["id"].ToString()));

            }

        }

        // Multisource = dataN.ToString();


        return items;
    }
    [WebMethod]
    public static List<ListItem> bindsubtasks(string tid)
    {
        DataAccess da1 = new DataAccess();
        // Prepare some dummy data for second dropdown based on the first dropdown value
        List<ListItem> items = new List<ListItem>();
        if (tid!= "")
        {
            items.Add(new ListItem("", ""));
        }
        if (tid == "")
        {
            tid = "0";
        }
        string strsql = "select id,Subtaskname name from Pms_tbl_Addsubtask where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";

        //if (tid != "" && tid != "0")
        //{
            strsql += " and Pms_tbl_Addsubtask.task in(" + tid + ")  ";


        //}
        DataSet dsN = da1.GetDataSetInline(strsql);

        string dataN = "";

        if (dsN.Tables[0].Rows.Count > 0)
        {

            for (int i1 = 0; i1 < dsN.Tables[0].Rows.Count; i1++)
            {
                items.Add(new ListItem(dsN.Tables[0].Rows[i1]["name"].ToString(), dsN.Tables[0].Rows[i1]["id"].ToString()));

            }

        }

        // Multisource = dataN.ToString();


        return items;
    }
    protected void bindmilestonenesubtask(string tid="")
    {
        string strsql = "select id,Subtaskname name from Pms_tbl_Addsubtask where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";

        if (tid != "" && tid != "0")
        {
            strsql += " and Pms_tbl_Addsubtask.task='" + tid + "'  ";

            
        }
        DataSet ds = da.GetDataSetInline(strsql);
        //cmf.BindDropDownList(subtaskid, ds, "name", "id");
        //ListItem li2 = new ListItem("", "0");
        //subtaskid.Items.Insert(0, li2);
        string dataN = "";
        for (int i1 = 0; i1 < ds.Tables[0].Rows.Count; i1++)
        {
            if (dataN != string.Empty)
            {
                dataN += ",{";
                dataN += "id: '" + ds.Tables[0].Rows[i1]["id"].ToString() + "',";
                dataN += "text: '" + ds.Tables[0].Rows[i1]["name"].ToString().Replace("'", "`") + "'";
                dataN += "}";
            }
            else
            {
                dataN += "{";
                dataN += "id: '" + ds.Tables[0].Rows[i1]["id"].ToString() + "',";
                dataN += "text: '" + ds.Tables[0].Rows[i1]["name"].ToString().Replace("'", "`") + "'";
                dataN += "}";
            }
        }
        MultiSubtask = dataN;

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
        else
        {
            ListItem li1 = new ListItem("", "0");
            ProjectId.Items.Insert(0, li1);
        }
        try
        {
if(hdnpid.Value!=""&& hdnpid.Value!="0")
            {
                ProjectId.SelectedValue = hdnpid.Value;
            }
        }
        catch { }


    }

    public static string getitemdetails(string ids,string strtypes)
    {
        DataAccess da = new DataAccess();
        string retval = "";
        try
        {

            string str_selecteddeatil = "";
            string strsqlprod = "select STUFF((SELECT ','+ itms.Milestone FROM Pms_tbl_AddMilestone itms WHERE itms.id  in (" + ids + ") FOR XML PATH('')) , 1, 1, '') Products";
            if (ids != "")
            {
                if(strtypes=="t")
                {
                    strsqlprod = "select STUFF((SELECT ','+ itms.taskname FROM Pms_tbl_Addtask itms WHERE itms.id  in (" + ids + ") FOR XML PATH('')) , 1, 1, '') Products";

                }
                if (strtypes == "st")
                {
                    strsqlprod = "select STUFF((SELECT ','+ itms.Subtaskname FROM Pms_tbl_Addsubtask itms WHERE itms.id  in (" + ids + ") FOR XML PATH('')) , 1, 1, '') Products";

                }
                DataSet dsprod = da.GetDataSetInline(strsqlprod);
                string strproduct = "";
                if (dsprod.Tables[0].Rows.Count > 0)
                {
                    retval = dsprod.Tables[0].Rows[0]["Products"].ToString();
                }
                else
                {
                    retval = "";
                }
            }



        }
        catch (Exception ex)
        {

        }
        return retval;
    }
    [WebMethod]
    public static string bindtaskdata(string RecordId = "0")
    {
        

        string taskdata = "";
        function fns = new function();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();

        
        //string strproject = "select * from Pms_tbl_AddProject where id='" + RecordId + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        string strsql = @"select DM.id, Prj.ProjectName,dmilestoneid mids,taskid tskids,subtaskid subtaskids,'' Milestone,'' taskname,'' Subtaskname  from tbl_dependencyM DM 
                        left join Pms_tbl_AddProject Prj on DM.projectid = Prj.id
                        
                        where DM.status = 1 and DM.archive = 0
                        and DM.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and DM.milestoneid='"+ RecordId + "'";
        DataSet dsdetails = da.GetDataSetInline(strsql);


       


        taskdata += "<table  id='myTable' style='width:100%;' cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm datatable'>";

        //taskdata += "<table id='tbDetails' cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm mb-0 datatable'>";

        taskdata += "<thead>";
        taskdata += "<tr>";


        taskdata += "<th style='display: none;'>S.No</th>";
        taskdata += "<th style='min-width:77px;'>Action</th>";
        taskdata += "<th style='min-width:280px;'>Project</th>";
        taskdata += "<th style='min-width:130px;'>Milestone</th>";
        taskdata += "<th style='min-width:180px;'>Task</th>";
        taskdata += "<th style='min-width:180px;'>Sub Task</th>";
       
        taskdata += "</tr>";
        taskdata += "</thead>";
        taskdata += "<tbody>";
        if (dsdetails.Tables[0].Rows.Count > 0)
        {

            for (int i = 0; i < dsdetails.Tables[0].Rows.Count; i++)
            {
                taskdata += "<tr>";
                taskdata += "<td style='display: none;'>"+(i+1)+"</td>";
                taskdata += " <td><a href='javascript:void(0);' Class='btn btn-info btn-sm' type='button' onclick='Binddetails(" + dsdetails.Tables[0].Rows[i]["id"].ToString() + ")' ><i class='ri-edit-line'></i></a>";
                taskdata += "&nbsp;&nbsp;<a href='javascript:void(0);' Class='btn btn-danger btn-sm' type='button' onclick='deletedatamile(\"" + dsdetails.Tables[0].Rows[i]["id"].ToString() + "\")'><i class='ri-delete-bin-5-line'></i></a>";
                taskdata += "</td>";
                taskdata += "<td>"+ dsdetails.Tables[0].Rows[i]["ProjectName"].ToString()+ "</td>";
                taskdata += "<td>" + getitemdetails(dsdetails.Tables[0].Rows[i]["mids"].ToString(),"m") + "</td>";
                taskdata += "<td>" + getitemdetails(dsdetails.Tables[0].Rows[i]["tskids"].ToString(), "t") + "</td>";
                taskdata += "<td>" + getitemdetails(dsdetails.Tables[0].Rows[i]["subtaskids"].ToString(), "st") + "</td>";
                taskdata += "</tr>";
            }
        }

        taskdata += "</tbody>";
        taskdata += "</table>";
        
        return taskdata;
    }
    protected void bindprojectname()
        {

            string strsql = "select id,ProjectName   from Pms_tbl_AddProject where id='" + hdnpid.Value + "' and status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by id";

            DataSet dsN = da.GetDataSetInline(strsql);

            if (dsN.Tables[0].Rows.Count > 0)
            {
                Projectname = dsN.Tables[0].Rows[0]["ProjectName"].ToString();


            }



        }
    protected void bindmilestonename()
        {

            string strsql = "select id, Milestone   from Pms_tbl_AddMilestone where id='" + hdnmid.Value + "' and status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by id";

            DataSet dsN = da.GetDataSetInline(strsql);

            if (dsN.Tables[0].Rows.Count > 0)
            {
            milestonenamename = dsN.Tables[0].Rows[0]["Milestone"].ToString();


            }



        }
    protected void btntask_Click(object sender, EventArgs e)
    {
        bindmilestonenetask(hdndmid.Value);
    }
    protected void btnsubtask_Click(object sender, EventArgs e)
    {
        bindmilestonenesubtask(hdntaskid.Value);
    }
    [WebMethod]
    public static string SaveeDependency(depentencydetails mdetails)
    {
        string retval = "1";
        DataAccess da1 = new DataAccess();
        CommonFunction cmf1 = new CommonFunction();
        string editid = mdetails.id;
        string entrytype = "Insert";
        if (editid != "" && editid != "0")
        {
            entrytype = "Update";
        }
       
            SqlParameter[] prm = new SqlParameter[11];
            prm[0] = new SqlParameter("@Action", entrytype);
            prm[1] = new SqlParameter("@id", editid);
            prm[2] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
            prm[3] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[4] = new SqlParameter("@cdate", da1.getcdatetime());            
            prm[5] = new SqlParameter("@projectid", mdetails.pid);
            prm[6] = new SqlParameter("@milestoneid", mdetails.mid);
            prm[7] = new SqlParameter("@dmilestoneid", mdetails.dmid);
            prm[8] = new SqlParameter("@taskid", mdetails.taskid);
            prm[9] = new SqlParameter("@subtaskid", mdetails.subtaskid);            
            prm[10] = new SqlParameter("ReturnValue", SqlDbType.Int);
            prm[10].Direction = ParameterDirection.ReturnValue;
            int retval1 = da1.executeDMLQuery("sp_tbl_dependencyM_DML", prm);
            retval = retval1.ToString();
           

           
        

        return retval;
    }
    [WebMethod]
    public static string DeleteDependency(int RecordId = 0)
    {
        string retval = "0";

        DataAccess da = new DataAccess();
        DataSet ds;
        string strsql = "";

        strsql = "update  tbl_dependencyM set  status=0,archive=1  where  id='" + RecordId + "' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        ds = da.GetDataSetInline(strsql);


        return retval = "1";
    }
    [WebMethod]
    public static string Binddetail(int RecordId = 0)
    {
        string retval = "0";

        DataAccess da = new DataAccess();
        DataSet ds;
        string strsql = "";

        strsql = "select * from  tbl_dependencyM where  id='" + RecordId + "' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        ds = da.GetDataSetInline(strsql);

        return ds.GetXml();

        
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
    protected void Button1_Click(object sender, EventArgs e)
    {
        //SaveLogText("editp");
        try
        {
            ddlMilestone.SelectedValue = hdnMilestoneid.Value;
            
        }
        catch
        {

        }
        try
        {

            ddldmid.SelectedValue = hdndmid.Value;
            bindmilestonenetask(hdndmid.Value); ;
        }
        catch(Exception ex)
        {
            SaveLogText("editp-"+ ex.Message);
        }
        try
        {
            taskid.SelectedValue = hdntaskid.Value;
            bindmilestonenesubtask(hdntaskid.Value); ;
        }
        catch
        {

        }
        try
        {
            subtaskid.SelectedValue = hdnsubtaskid.Value;
        }
        catch
        {

        }
       

    }
    protected void btncancel_Click(object sender, EventArgs e)
    {
        //SaveLogText("editp");
        
        try
        {
            ddlMilestone.SelectedValue = hdnMilestoneid.Value;
            
        }
        catch
        {

        }
        try
        {

            ddldmid.SelectedValue = hdnMilestoneid.Value;
            bindmilestonenetask(hdnMilestoneid.Value); ;

        }
        catch(Exception ex)
        {
            SaveLogText("editp-"+ ex.Message);
        }
        try
        {
            taskid.SelectedValue = "0";
            bindmilestonenesubtask(taskid.SelectedValue); ;
        }
        catch
        {

        }
        
       

    }
    public class depentencydetails
    {
        public string id { get; set; }
        public string pid { get; set; }
        public string mid { get; set; }
        public string dmid { get; set; }
        public string taskid { get; set; }
        public string subtaskid { get; set; }


    }
}

