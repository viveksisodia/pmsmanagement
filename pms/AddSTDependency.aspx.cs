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

public partial class AddSTDependency : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataAccess da = new DataAccess();
    public CommonFunction cmf = new CommonFunction();
    SqlParameter[] prm, prm1;
    public function fns = new function();
    public string strdisplay_close = "style='display:'";
    public string strmilestone = "";
    public string strtask = "";
    public string strsubtask = "";
    public string strdvfilter = "style='display:none'";
    public string strbtnnew = "";

    public string milestonenamename = "";
    public string Projectname = "";


    public string MultiMilestone = "";
    public string MultiTask = "";
    public string MultiSubtask = "";
    public string strdivtsk = "style='display:none'";
    public int ischeckAllaccess = 0;
    public int ischeckCreate = 0;
    public int ischeckEdit = 0;
    public int ischeckDelete = 0;
    public int ischeckView = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (HttpContext.Current.Request.QueryString["ctype"] != null)
            {
                strdisplay_close = "style='display:none'";
            }
        }
        catch { }
        try
        {
            string strvalidationS = get_EditDelete_Validation("459");
            string[] arS = strvalidationS.Split(',');
            ischeckAllaccess = Convert.ToInt32(arS[0]);
            ischeckView = Convert.ToInt32(arS[1]);
            ischeckCreate = Convert.ToInt32(arS[2]);
            ischeckEdit = Convert.ToInt32(arS[3]);
            ischeckDelete = Convert.ToInt32(arS[4]);
            if(ischeckAllaccess==0&&ischeckView==0&&ischeckCreate==0&&ischeckEdit==0&&ischeckDelete == 0)
            {
                Response.Redirect("/pms/accessdenied.aspx");
            }

        }
        catch
        {

        }

        try
        {
            bindmilestonenew(HttpContext.Current.Request.QueryString["mprojectid"].ToString(),"");
        }
        catch { }
        if (!Page.IsPostBack)
        {
            bindfilters();
            binddstatus();
            if (HttpContext.Current.Request.QueryString["pid"] != null)
            {
                hdnpid.Value = HttpContext.Current.Request.QueryString["pid"].ToString();
            }
            if (HttpContext.Current.Request.QueryString["mid"] != null)
            {
                hdnmid.Value = HttpContext.Current.Request.QueryString["mid"].ToString();
            }
            if (HttpContext.Current.Request.QueryString["tid"] != null)
            {
                hdntid.Value = HttpContext.Current.Request.QueryString["tid"].ToString();
            }
            if (HttpContext.Current.Request.QueryString["id"] != null)
            {
                hdndependid.Value = HttpContext.Current.Request.QueryString["id"].ToString();
            }

            if (HttpContext.Current.Request.QueryString["type"] != null)
            {
                hdndependtype.Value = HttpContext.Current.Request.QueryString["type"].ToString();
                binddetails_for(HttpContext.Current.Request.QueryString["type"].ToString(), HttpContext.Current.Request.QueryString["id"].ToString());
               
            }

            bindProject();
            //bindmilestone(hdnpid.Value, hdnmid.Value);
            bindmilestonenetask(hdnmid.Value);
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


    protected void bindfilters()
    {
        try
        {

            ListItem li1 = new ListItem("", "0");
            ddlfilter.Items.Insert(0, li1);


        }
        catch
        { }
    }
    protected void binddstatus()
    {
        try
        {

            ListItem li1 = new ListItem("", "0");
            ddldstatus.Items.Insert(0, li1);


        }
        catch
        { }
    }
    protected void bindmilestone(string pid)
    {
        string strsql = "select Pms_tbl_AddMilestone.id,Milestone name from Pms_tbl_AddMilestone " +
            " left join Pms_tbl_AddProject on Pms_tbl_AddMilestone.ProjectId=Pms_tbl_AddProject.id where Pms_tbl_AddMilestone.status=1 and Pms_tbl_AddMilestone.archive=0 and Pms_tbl_AddMilestone.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and Pms_tbl_AddMilestone.ProjectId='" + pid + "'";
       
        DataSet ds = da.GetDataSetInline(strsql);
        cmf.BindDropDownList(ddlMilestone, ds, "name", "id");
        ListItem li2 = new ListItem("", "0");
        ddlMilestone.Items.Insert(0, li2);
        


    }

    protected void bindmilestonenew(string pid="",string strmid="")
    {
        string strsql = "select Pms_tbl_AddMilestone.id,Milestone name from Pms_tbl_AddMilestone " +
            " left join Pms_tbl_AddProject on Pms_tbl_AddMilestone.ProjectId=Pms_tbl_AddProject.id where Pms_tbl_AddMilestone.status=1 and Pms_tbl_AddMilestone.archive=0 and Pms_tbl_AddMilestone.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and Pms_tbl_AddMilestone.ProjectId='" + pid + "'";
        if(pid==""|| pid=="0")
        {
            strsql = "select Pms_tbl_AddMilestone.id,Milestone name from Pms_tbl_AddMilestone " +
                        " left join Pms_tbl_AddProject on Pms_tbl_AddMilestone.ProjectId=Pms_tbl_AddProject.id where Pms_tbl_AddMilestone.status=1 and Pms_tbl_AddMilestone.archive=0 and Pms_tbl_AddMilestone.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";

        }
        if (strmid != "" && strmid != "0")
        {
            strsql += " and Pms_tbl_AddMilestone.id not in(" + strmid + ")";
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
    public static List<ListItem> bindmilestonedata(string mprojectid,string mid)
    {
        DataAccess da1 = new DataAccess();
        // Prepare some dummy data for second dropdown based on the first dropdown value
        List<ListItem> items = new List<ListItem>();
        if (mid != "")
        {
            items.Add(new ListItem("", ""));
        }
        if (mid == "")
        {
            mid = "0";
        }
        string strsql ="select Pms_tbl_AddMilestone.id,Milestone name from Pms_tbl_AddMilestone  where Pms_tbl_AddMilestone.status=1 and Pms_tbl_AddMilestone.archive=0 and Pms_tbl_AddMilestone.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and projectid='"+ mprojectid + "' ";        
        if (mid != "" && mid != "0")
        {
            strsql += " and Pms_tbl_AddMilestone.id not in(" + mid + ")";
        }
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
    public static List<ListItem> bindtasks(string mprojectid,string mid,string tid)
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

        if (mid != "" && mid != "0")
        {
            strsql += " and Pms_tbl_Addtask.Milestoneid in(" + mid + ") ";
        }
        strsql += " and Pms_tbl_Addtask.ProjectId ='" + mprojectid + "'  and Pms_tbl_Addtask.taskname IS NOT NULL and Pms_tbl_Addtask.taskname<> ' ' ";
        if (tid != "" && tid != "0")
        {
            strsql += " and Pms_tbl_Addtask.id not in(" + tid + ") ";
        }
        SaveLogText("strsqlttt"+ strsql);
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
    public static List<ListItem> bindsubtasks(string mprojectid,string tid,string stid)
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

        
            strsql += " and Pms_tbl_Addsubtask.Projectid='"+ mprojectid + "' and Pms_tbl_Addsubtask.task in(" + tid + ") and Pms_tbl_Addsubtask.subtaskname IS NOT NULL and Pms_tbl_Addsubtask.subtaskname<> ' ' ";
      if (stid != "" && stid != "0")
        {
            strsql += "  and Pms_tbl_Addsubtask.id not in(" + stid + ")  ";
        }
        SaveLogText("strsqltt1"+ strsql);
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
            string strsqlprod = "select STUFF((SELECT ','+ itms.Milestone FROM Pms_tbl_AddMilestone itms WHERE itms.id  in (" + ids + ") and isnull(itms.statuscomplete,0)=0 FOR XML PATH('')) , 1,1, '') Products";
            SaveLogText("strsqlprod-"+ strsqlprod);

            if (ids != "")
            {
                if(strtypes=="t")
                {
                    strsqlprod = "select STUFF((SELECT ','+ itms.taskname FROM Pms_tbl_Addtask itms WHERE itms.id  in (" + ids + ") and isnull(itms.statuscomplete,0)=0 FOR XML PATH('')) , 1, 1,'') Products";

                }
                if (strtypes == "st")
                {
                    strsqlprod = "select STUFF((SELECT ','+ itms.Subtaskname FROM Pms_tbl_Addsubtask itms WHERE itms.id  in (" + ids + ") and isnull(itms.statuscomplete,0)=0 FOR XML PATH('')) , 1, 1,'') Products";

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
    public static string getdependencydetails(string ids)
    {
        DataAccess da = new DataAccess();
        string strdependency = "";
        string retval = "";
        try
        {

            string str_selecteddeatil = "";
            //string strsqlprod = "select * from tbl_dependencyST where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
            if (ids != "")
            {
               
                 SqlParameter[] prm = new SqlParameter[2];
                 prm[0] = new SqlParameter("@id", ids);
                 prm[1] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                 DataSet dsD=da.GetDataSet("sp_dependencydetails", prm);
                if(dsD.Tables[0].Rows.Count>0)
                {
                    for(int i=0;i< dsD.Tables[0].Rows.Count;i++)
                    {
                        string strbgc_m = "label";
                        if(dsD.Tables[0].Rows[i]["mid"].ToString()!=""&& dsD.Tables[0].Rows[i]["mid"].ToString() != "0")
                        {
                            try
                            {
                                string stsqlM = "select isnull(statuscomplete,0) statuscomplete from Pms_tbl_AddMilestone where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + dsD.Tables[0].Rows[i]["mid"].ToString() + "'  and status=1 and archive=0";
                                DataSet dschkM = da.GetDataSetInline(stsqlM);
                                if (dschkM.Tables[0].Rows[0]["statuscomplete"].ToString()=="1")
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
                                string stsqlM = "select isnull(statuscomplete,0) statuscomplete from Pms_tbl_AddTask where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + dsD.Tables[0].Rows[i]["tid"].ToString() + "'  and status=1 and archive=0";
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
                                string stsqlM = "select isnull(statuscomplete,0) statuscomplete from Pms_tbl_AddSubtask where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + dsD.Tables[0].Rows[i]["stid"].ToString() + "' and status=1 and archive=0";
                                
                                DataSet dschkM = da.GetDataSetInline(stsqlM);
                                if (dschkM.Tables[0].Rows[0]["statuscomplete"].ToString() == "1")
                                {
                                    strbgc_s = "label complete";
                                }
                                
                            }
                            catch(Exception ex) {
                                //SaveLogText("strbgc_s2-" + ex.Message);
                            }
                        }
                        //str_selecteddeatil += (i+1).ToString()+" "+ dsD.Tables[0].Rows[i]["Milestone"].ToString() + " - " + dsD.Tables[0].Rows[i]["Taskname"].ToString() + " - " + dsD.Tables[0].Rows[i]["Subtaskname"].ToString();
                        str_selecteddeatil += "<div class='hierarchy-line'><b>"+ (i + 1).ToString() +"</b>&nbsp;"+
                             "<span class='"+ strbgc_m + "'>" + dsD.Tables[0].Rows[i]["Milestone"].ToString() + "</span>" +
                             "<span class='" + strbgc_t + "'>" + (dsD.Tables[0].Rows[i]["Taskname"].ToString()==""?"":"- "+ dsD.Tables[0].Rows[i]["Taskname"].ToString()) + "</span>" +
                             "<span class='" + strbgc_s + "'>" + (dsD.Tables[0].Rows[i]["Subtaskname"].ToString()==""?"":"- "+ dsD.Tables[0].Rows[i]["Subtaskname"].ToString()) + "</span>" +
                             
                             "</div>";
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
    public static string checkdependencynotcomplete(string ids)
    {
        DataAccess da = new DataAccess();
        string strdependency = "";
        string retval = "1";
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
                        
                        if (dsD.Tables[0].Rows[i]["mid"].ToString() != "" && dsD.Tables[0].Rows[i]["mid"].ToString() != "0")
                        {
                            try
                            {
                                string stsqlM = "select isnull(statuscomplete,0) statuscomplete from Pms_tbl_AddMilestone where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + dsD.Tables[0].Rows[i]["mid"].ToString() + "'";
                                DataSet dschkM = da.GetDataSetInline(stsqlM);
                                if (dschkM.Tables[0].Rows[0]["statuscomplete"].ToString()!= "1")
                                {
                                    retval = "0";
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
                                if (dschkM.Tables[0].Rows[0]["statuscomplete"].ToString() != "1")
                                {
                                    retval = "0";
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
                                    retval = "0";
                                }

                            }
                            catch (Exception ex)
                            {
                                //SaveLogText("strbgc_s2-" + ex.Message);
                            }
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
    [WebMethod]
    public static string bindtaskdata(string RecordId = "0", string RecordType="")
    {
        string taskdata = "";
        function fns = new function();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        string strprojectdisplay = "style='min-width:180px;'"; 
        string strmilestonedisplay = "style='min-width:180px;'"; 
        string strtaskdisplay = "style='min-width:180px;'"; 
        string strsubtaskdisplay = "style='min-width:180px;'"; 

        
        
        
        //string strproject = "select * from Pms_tbl_AddProject where id='" + RecordId + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        string strsql = @"select DM.id,isnull(DM.milestoneid,0) mid,isnull(DM.taskid,0) tid,isnull(DM.subtaskid,0) stid, Prj.ProjectName,dmilestoneid mids,dtaskid tskids,dsubtaskid subtaskids,mil.Milestone,tsk.taskname,stsk.Subtaskname  from tbl_dependencyST DM 
                        left join Pms_tbl_Addsubtask stsk on DM.subtaskid = stsk.id                        
                        left join Pms_tbl_Addtask tsk on DM.taskid = tsk.id                        
                        left join Pms_tbl_AddMilestone mil on DM.milestoneid = mil.id                        
                        left join Pms_tbl_AddProject Prj on DM.projectid = Prj.id                        
                        where DM.status = 1 and DM.archive = 0
                        and DM.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";

        if (RecordType == "project")
        {

            strsql += " and (DM.MProjectId='" + RecordId + "')";
        }
        else if (RecordType == "milestone")
        {
            strprojectdisplay = "style='min-width:180px;display:none'";
            strtaskdisplay = "style='min-width:180px;display:none'";
            strsubtaskdisplay = "style='min-width:180px;display:none'";
            strsql += " and DM.milestoneid='" + RecordId + "'";
        }
        else if (RecordType == "task")
        {
            strprojectdisplay = "style='min-width:180px;display:none'";
            strmilestonedisplay = "style='min-width:180px;display:none'";
            strsubtaskdisplay = "style='min-width:180px;display:none'";

            strsql += " and DM.taskid='" + RecordId + "'";
        }
        else if (RecordType == "subtask")
        {
            strprojectdisplay = "style='min-width:180px;display:none'";
            strmilestonedisplay = "style='min-width:180px;display:none'";
            strtaskdisplay = "style='min-width:180px;display:none'";
           
            strsql += " and DM.subtaskid='" + RecordId + "'";
        }

        DataSet dsdetails = da.GetDataSetInline(strsql);


       


        taskdata += "<table  id='myTable' style='width:100%;' cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm datatable'>";

        //taskdata += "<table id='tbDetails' cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm mb-0 datatable'>";

        taskdata += "<thead>";
        taskdata += "<tr>";


        taskdata += "<th style='display: none;'>S.No</th>";
        taskdata += "<th style='min-width:77px;'>Action</th>";
        taskdata += "<th "+ strprojectdisplay+ ">Project</th>";
        taskdata += "<th " + strmilestonedisplay+ ">Milestone</th>";
        taskdata += "<th " + strtaskdisplay+ ">Task</th>";
        taskdata += "<th " + strsubtaskdisplay+ ">Sub Task</th>";
        taskdata += "<th style='min-width:250px;'>Dependency On</th>";
        //taskdata += "<th style='min-width:250px;'>Dependency On</th>";
       
        taskdata += "</tr>";
        taskdata += "</thead>";
        taskdata += "<tbody>";
        string strvalidationD = get_EditDelete_Validation("459");

        string streditD = "0";
        string strdeleteD = "0";
        string strownerD = "0";

        try
        {
            string[] arrD = strvalidationD.Split(',');
            streditD = arrD[3];
            strdeleteD = arrD[4];
           

        }
        catch { }
        if (dsdetails.Tables[0].Rows.Count > 0)
        {
            string strprojectname = "";
            string strmilestonename = "";
            string strtaskname = "";
            string strsubtaskname = "";
            DataSet dsDnew = new DataSet();

            for (int i = 0; i < dsdetails.Tables[0].Rows.Count; i++)
            {
                string strM = dsdetails.Tables[0].Rows[i]["mid"].ToString();
                string strT = dsdetails.Tables[0].Rows[i]["tid"].ToString();
                string strST =dsdetails.Tables[0].Rows[i]["stid"].ToString();
                strprojectname = "";
                strmilestonename = "";
                strtaskname = "";
                strsubtaskname = "";
                if (strST!="0")
                {
                  string  strsqlQ = "select stsk.id,stsk.task tskid,Subtaskname,taskname,mil.Milestone,proj.ProjectName,tsk.Milestoneid   from Pms_tbl_Addsubtask stsk " +
                                           "left join Pms_tbl_Addtask tsk on stsk.task=tsk.id " +
                                           "left join  Pms_tbl_AddMilestone mil on tsk.Milestoneid=mil.id " +
                                           "left join Pms_tbl_AddProject proj on mil.ProjectId=proj.id" +
                                           " where stsk.id='" + strST + "' and stsk.status=1 and stsk.archive=0 and stsk.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by stsk.id";

                    dsDnew = da.GetDataSetInline(strsqlQ);
                    if(dsDnew.Tables[0].Rows.Count>0)
                    {
                        strprojectname = dsDnew.Tables[0].Rows[0]["ProjectName"].ToString();
                        strmilestonename = dsDnew.Tables[0].Rows[0]["Milestone"].ToString();
                        strtaskname = dsDnew.Tables[0].Rows[0]["taskname"].ToString();
                        strsubtaskname = dsDnew.Tables[0].Rows[0]["Subtaskname"].ToString();
                    }
                }
                else if (strT != "0")
                {
                    string strsqlQ = "select tsk.id,taskname,mil.Milestone,proj.ProjectName,tsk.Milestoneid   from Pms_tbl_Addtask tsk " +
                            "left join  Pms_tbl_AddMilestone mil on tsk.Milestoneid=mil.id " +
                            "left join Pms_tbl_AddProject proj on mil.ProjectId=proj.id" +
                            " where tsk.id='" + strT + "' and tsk.status=1 and tsk.archive=0 and tsk.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by tsk.id";

                    dsDnew = da.GetDataSetInline(strsqlQ);
                    if (dsDnew.Tables[0].Rows.Count > 0)
                    {
                        strprojectname = dsDnew.Tables[0].Rows[0]["ProjectName"].ToString();
                        strmilestonename = dsDnew.Tables[0].Rows[0]["Milestone"].ToString();
                        strtaskname = dsDnew.Tables[0].Rows[0]["taskname"].ToString();
                        
                    }
                }
                else if (strM != "0")
                {
                    string strsqlQ = "select mil.id,mil.projectid,Milestone,proj.ProjectName   from Pms_tbl_AddMilestone mil " +
                "left join Pms_tbl_AddProject proj on mil.ProjectId=proj.id" +
                " where mil.id='" + strM + "' and mil.status=1 and mil.archive=0 and mil.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by mil.id";

                    dsDnew = da.GetDataSetInline(strsqlQ);
                    if (dsDnew.Tables[0].Rows.Count > 0)
                    {
                        strprojectname = dsDnew.Tables[0].Rows[0]["ProjectName"].ToString();
                        strmilestonename = dsDnew.Tables[0].Rows[0]["Milestone"].ToString();
                        

                    }
                }
                else
                {
                    strprojectname = dsdetails.Tables[0].Rows[i]["ProjectName"].ToString();

                }

                taskdata += "<tr>";
                taskdata += "<td style='display: none;'>"+(i+1)+"</td>";
                taskdata += " <td><a href='javascript:void(0);' style='display:"+(streditD=="1"?"":"none") +"' Class='btn btn-info btn-sm' type='button' onclick='Binddetails(" + dsdetails.Tables[0].Rows[i]["id"].ToString() + ")' ><i class='ri-edit-line'></i></a>";
                taskdata += "&nbsp;&nbsp;<a href='javascript:void(0);' style='display:" + (strdeleteD == "1" ? "" : "none") + "' Class='btn btn-danger btn-sm' type='button' onclick='deletedatamile(\"" + dsdetails.Tables[0].Rows[i]["id"].ToString() + "\")'><i class='ri-delete-bin-5-line'></i></a>";
                taskdata += "</td>";
                taskdata += "<td  " + strprojectdisplay + ">" + strprojectname + "</td>";
                taskdata += "<td  " + strmilestonedisplay + ">" + strmilestonename + "</td>";
                taskdata += "<td  " + strtaskdisplay + ">" + strtaskname + "</td>";
                taskdata += "<td  " + strsubtaskdisplay + ">" + strsubtaskname + "</td>";
                taskdata += "<td>" + getdependencydetails(dsdetails.Tables[0].Rows[i]["id"].ToString()) + "</td>";
                taskdata += "</tr>";
            }
        }

        taskdata += "</tbody>";
        taskdata += "</table>";
        
        return taskdata;
    }
    [WebMethod]
    public static string bindtaskdatacustom(string RecordType = "", string status = "0",string strid="")
    {
        string taskdata = "";
        function fns = new function();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        string strprojectdisplay = "style='min-width:180px;'";
        string strmilestonedisplay = "style='min-width:180px;'";
        string strtaskdisplay = "style='min-width:180px;'";
        string strsubtaskdisplay = "style='min-width:180px;'";




        //string strproject = "select * from Pms_tbl_AddProject where id='" + RecordId + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        string strsql = @"select DM.id,isnull(DM.milestoneid,0) mid,isnull(DM.taskid,0) tid,isnull(DM.subtaskid,0) stid, Prj.ProjectName,dmilestoneid mids,dtaskid tskids,dsubtaskid subtaskids,mil.Milestone,tsk.taskname,stsk.Subtaskname  from tbl_dependencyST DM 
                        left join Pms_tbl_Addsubtask stsk on DM.subtaskid = stsk.id                        
                        left join Pms_tbl_Addtask tsk on DM.taskid = tsk.id                        
                        left join Pms_tbl_AddMilestone mil on DM.milestoneid = mil.id                        
                        left join Pms_tbl_AddProject Prj on DM.projectid = Prj.id                        
                        where DM.status = 1 and DM.archive = 0
                        and DM.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";

        if (RecordType == "milestone")
        {
            strsql += " and isnull(DM.milestoneid,0)='" + strid + "'";
        }
        else if (RecordType == "task")
        {
            strsql += " and isnull(DM.taskid,0)='" + strid + "'";
        }
        else if (RecordType == "subtask")
        {
            strsql += " and isnull(DM.subtaskid,0)='" + strid + "'";
        }
        else
        {

            strsql += " and (DM.MProjectId='" + strid + "')";
        }
        
      
        DataSet dsdetails = da.GetDataSetInline(strsql);





        taskdata += "<table  id='myTable' style='width:100%;' cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm datatable'>";

        //taskdata += "<table id='tbDetails' cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm mb-0 datatable'>";

        taskdata += "<thead>";
        taskdata += "<tr>";


        taskdata += "<th style='display: none;'>S.No</th>";
        taskdata += "<th style='min-width:77px;'>Action</th>";
        taskdata += "<th " + strprojectdisplay + ">Project</th>";
        taskdata += "<th " + strmilestonedisplay + ">Milestone</th>";
        taskdata += "<th " + strtaskdisplay + ">Task</th>";
        taskdata += "<th " + strsubtaskdisplay + ">Sub Task</th>";
        taskdata += "<th style='min-width:250px;'>Dependency On</th>";
        //taskdata += "<th style='min-width:250px;'>Dependency On</th>";

        taskdata += "</tr>";
        taskdata += "</thead>";
        taskdata += "<tbody>";
        if (dsdetails.Tables[0].Rows.Count > 0)
        {
            string strprojectname = "";
            string strmilestonename = "";
            string strtaskname = "";
            string strsubtaskname = "";
            DataSet dsDnew = new DataSet();
            int indx = 0;
            for (int i = 0; i < dsdetails.Tables[0].Rows.Count; i++)
            {
                string strM = dsdetails.Tables[0].Rows[i]["mid"].ToString();
                string strT = dsdetails.Tables[0].Rows[i]["tid"].ToString();
                string strST = dsdetails.Tables[0].Rows[i]["stid"].ToString();
                strprojectname = "";
                strmilestonename = "";
                strtaskname = "";
                strsubtaskname = "";
                if (strST != "0")
                {
                    string strsqlQ = "select stsk.id,stsk.task tskid,Subtaskname,taskname,mil.Milestone,proj.ProjectName,tsk.Milestoneid   from Pms_tbl_Addsubtask stsk " +
                                             "left join Pms_tbl_Addtask tsk on stsk.task=tsk.id " +
                                             "left join  Pms_tbl_AddMilestone mil on tsk.Milestoneid=mil.id " +
                                             "left join Pms_tbl_AddProject proj on mil.ProjectId=proj.id" +
                                             " where stsk.id='" + strST + "' and stsk.status=1 and stsk.archive=0 and stsk.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by stsk.id";

                    dsDnew = da.GetDataSetInline(strsqlQ);
                    if (dsDnew.Tables[0].Rows.Count > 0)
                    {
                        strprojectname = dsDnew.Tables[0].Rows[0]["ProjectName"].ToString();
                        strmilestonename = dsDnew.Tables[0].Rows[0]["Milestone"].ToString();
                        strtaskname = dsDnew.Tables[0].Rows[0]["taskname"].ToString();
                        strsubtaskname = dsDnew.Tables[0].Rows[0]["Subtaskname"].ToString();
                    }
                }
                else if (strT != "0")
                {
                    string strsqlQ = "select tsk.id,taskname,mil.Milestone,proj.ProjectName,tsk.Milestoneid   from Pms_tbl_Addtask tsk " +
                            "left join  Pms_tbl_AddMilestone mil on tsk.Milestoneid=mil.id " +
                            "left join Pms_tbl_AddProject proj on mil.ProjectId=proj.id" +
                            " where tsk.id='" + strT + "' and tsk.status=1 and tsk.archive=0 and tsk.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by tsk.id";

                    dsDnew = da.GetDataSetInline(strsqlQ);
                    if (dsDnew.Tables[0].Rows.Count > 0)
                    {
                        strprojectname = dsDnew.Tables[0].Rows[0]["ProjectName"].ToString();
                        strmilestonename = dsDnew.Tables[0].Rows[0]["Milestone"].ToString();
                        strtaskname = dsDnew.Tables[0].Rows[0]["taskname"].ToString();

                    }
                }
                else if (strM != "0")
                {
                    string strsqlQ = "select mil.id,mil.projectid,Milestone,proj.ProjectName   from Pms_tbl_AddMilestone mil " +
                "left join Pms_tbl_AddProject proj on mil.ProjectId=proj.id" +
                " where mil.id='" + strM + "' and mil.status=1 and mil.archive=0 and mil.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by mil.id";

                    dsDnew = da.GetDataSetInline(strsqlQ);
                    if (dsDnew.Tables[0].Rows.Count > 0)
                    {
                        strprojectname = dsDnew.Tables[0].Rows[0]["ProjectName"].ToString();
                        strmilestonename = dsDnew.Tables[0].Rows[0]["Milestone"].ToString();


                    }
                }
                else
                {
                    strprojectname = dsdetails.Tables[0].Rows[i]["ProjectName"].ToString();

                }
                if (status == "1")
                {
                    string chkcomplete = checkdependencynotcomplete(dsdetails.Tables[0].Rows[i]["id"].ToString());
                    if (chkcomplete == "1")
                    {
                        indx++;
                        taskdata += "<tr>";
                        taskdata += "<td style='display: none;'>" + indx + "</td>";
                        taskdata += " <td><a href='javascript:void(0);' Class='btn btn-info btn-sm' type='button' onclick='Binddetails(" + dsdetails.Tables[0].Rows[i]["id"].ToString() + ")' ><i class='ri-edit-line'></i></a>";
                        taskdata += "&nbsp;&nbsp;<a href='javascript:void(0);' Class='btn btn-danger btn-sm' type='button' onclick='deletedatamile(\"" + dsdetails.Tables[0].Rows[i]["id"].ToString() + "\")'><i class='ri-delete-bin-5-line'></i></a>";
                        taskdata += "</td>";
                        taskdata += "<td  " + strprojectdisplay + ">" + strprojectname + "</td>";
                        taskdata += "<td  " + strmilestonedisplay + ">" + strmilestonename + "</td>";
                        taskdata += "<td  " + strtaskdisplay + ">" + strtaskname + "</td>";
                        taskdata += "<td  " + strsubtaskdisplay + ">" + strsubtaskname + "</td>";
                        taskdata += "<td>" + getdependencydetails(dsdetails.Tables[0].Rows[i]["id"].ToString()) + "</td>";
                        taskdata += "</tr>";
                    }
                }
                else if (status == "2")
                {
                    string chkcomplete = checkdependencynotcomplete(dsdetails.Tables[0].Rows[i]["id"].ToString());
                    if (chkcomplete == "0")
                    {
                        indx++;
                        taskdata += "<tr>";
                        taskdata += "<td style='display: none;'>" + indx + "</td>";
                        taskdata += " <td><a href='javascript:void(0);' Class='btn btn-info btn-sm' type='button' onclick='Binddetails(" + dsdetails.Tables[0].Rows[i]["id"].ToString() + ")' ><i class='ri-edit-line'></i></a>";
                        taskdata += "&nbsp;&nbsp;<a href='javascript:void(0);' Class='btn btn-danger btn-sm' type='button' onclick='deletedatamile(\"" + dsdetails.Tables[0].Rows[i]["id"].ToString() + "\")'><i class='ri-delete-bin-5-line'></i></a>";
                        taskdata += "</td>";
                        taskdata += "<td  " + strprojectdisplay + ">" + strprojectname + "</td>";
                        taskdata += "<td  " + strmilestonedisplay + ">" + strmilestonename + "</td>";
                        taskdata += "<td  " + strtaskdisplay + ">" + strtaskname + "</td>";
                        taskdata += "<td  " + strsubtaskdisplay + ">" + strsubtaskname + "</td>";
                        taskdata += "<td>" + getdependencydetails(dsdetails.Tables[0].Rows[i]["id"].ToString()) + "</td>";
                        taskdata += "</tr>";
                    }
                }
                else
                {
                    
                    taskdata += "<tr>";
                    taskdata += "<td style='display: none;'>" + (i+1) + "</td>";
                    taskdata += " <td><a href='javascript:void(0);' Class='btn btn-info btn-sm' type='button' onclick='Binddetails(" + dsdetails.Tables[0].Rows[i]["id"].ToString() + ")' ><i class='ri-edit-line'></i></a>";
                    taskdata += "&nbsp;&nbsp;<a href='javascript:void(0);' Class='btn btn-danger btn-sm' type='button' onclick='deletedatamile(\"" + dsdetails.Tables[0].Rows[i]["id"].ToString() + "\")'><i class='ri-delete-bin-5-line'></i></a>";
                    taskdata += "</td>";
                    taskdata += "<td  " + strprojectdisplay + ">" + strprojectname + "</td>";
                    taskdata += "<td  " + strmilestonedisplay + ">" + strmilestonename + "</td>";
                    taskdata += "<td  " + strtaskdisplay + ">" + strtaskname + "</td>";
                    taskdata += "<td  " + strsubtaskdisplay + ">" + strsubtaskname + "</td>";
                    taskdata += "<td>" + getdependencydetails(dsdetails.Tables[0].Rows[i]["id"].ToString()) + "</td>";
                    taskdata += "</tr>";
                }
                
            }
        }

        taskdata += "</tbody>";
        taskdata += "</table>";

        return taskdata;
    }
    protected void bindtasknamename()
        {

         string strsql = "select id, taskname   from Pms_tbl_Addtask where id='" + hdntid.Value + "' and status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by id";

        DataSet dsN = da.GetDataSetInline(strsql);

            if (dsN.Tables[0].Rows.Count > 0)
            {
                Projectname = dsN.Tables[0].Rows[0]["taskname"].ToString();


            }



        }   
    protected void bindsubtaskname()
    {

        string strsql = "select id,Subtaskname   from Pms_tbl_Addsubtask where id='" + hdnstid.Value + "' and status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by id";

        DataSet dsN = da.GetDataSetInline(strsql);

        if (dsN.Tables[0].Rows.Count > 0)
        {
            milestonenamename = dsN.Tables[0].Rows[0]["Subtaskname"].ToString();


        }



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

    protected void binddetails_for(string strtypes, string strid)
    {
        string strsql = "";
        string pid = "";
        string mid = "";
        string tid = "";
        DataSet dsdetails = new DataSet();
        if(strtypes=="project")
        {
            strdvfilter = "";
            strsql = "select id, ProjectName   from Pms_tbl_AddProject where id='" + strid + "' and status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by id";

            dsdetails = da.GetDataSetInline(strsql);

            if (dsdetails.Tables[0].Rows.Count > 0)
            {
                milestonenamename = dsdetails.Tables[0].Rows[0]["ProjectName"].ToString();


            }

            bindmilestone(strid);

        }
        else if (strtypes == "milestone")
        {
            strdvfilter = "style='display:none'";
            hdnmid.Value = strid;

            strmilestone = "style='display:none'";
            strtask = "style='display:none'";
            strsubtask = "style='display:none'";
            strsql = "select mil.id,mil.projectid,Milestone,proj.ProjectName   from Pms_tbl_AddMilestone mil " +
                "left join Pms_tbl_AddProject proj on mil.ProjectId=proj.id" +
                " where mil.id='" + strid + "' and mil.status=1 and mil.archive=0 and mil.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by mil.id";

            dsdetails = da.GetDataSetInline(strsql);

            if (dsdetails.Tables[0].Rows.Count > 0)
            {
                milestonenamename = dsdetails.Tables[0].Rows[0]["Milestone"].ToString();
                Projectname = dsdetails.Tables[0].Rows[0]["ProjectName"].ToString();
                bindmilestone(dsdetails.Tables[0].Rows[0]["projectid"].ToString());

            }
            try
            {
                bindmilestonenew(HttpContext.Current.Request.QueryString["mprojectid"].ToString(), strid);
            }
            catch { }
            try
            {

                  ddlMilestone.SelectedValue = strid;
                  bindtaskids(ddlMilestone.SelectedValue);
            }
            catch { }
        }
        else if (strtypes == "task")
        {
            strdvfilter = "style='display:none'";
            strmilestone = "style='display:none'";
            strtask = "style='display:none'";
            strsubtask = "style='display:none'";
            strsql = "select tsk.id,taskname,mil.Milestone,proj.ProjectName,tsk.Milestoneid   from Pms_tbl_Addtask tsk " +
                            "left join  Pms_tbl_AddMilestone mil on tsk.Milestoneid=mil.id " +
                            "left join Pms_tbl_AddProject proj on mil.ProjectId=proj.id" +
                            " where tsk.id='" + strid + "' and tsk.status=1 and tsk.archive=0 and tsk.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by tsk.id";

            dsdetails = da.GetDataSetInline(strsql);

            if (dsdetails.Tables[0].Rows.Count > 0)
            {
                hdnmid.Value = dsdetails.Tables[0].Rows[0]["Milestoneid"].ToString();
                milestonenamename = dsdetails.Tables[0].Rows[0]["taskname"].ToString();
                Projectname = dsdetails.Tables[0].Rows[0]["Milestone"].ToString();


            }
            ddlMilestone.SelectedValue = hdnmid.Value;
            bindtaskids(hdnmid.Value);
            ddltask1.SelectedValue = strid;
            bindsubtaskids(ddltask1.SelectedValue);
        }
        else if (strtypes == "subtask")
        {
            strdvfilter = "style='display:none'";
            strmilestone = "style='display:none'";
            strtask = "style='display:none'";
            strsubtask = "style='display:none'";

            strsql = "select stsk.id,stsk.task tskid,Subtaskname,taskname,mil.Milestone,proj.ProjectName,tsk.Milestoneid   from Pms_tbl_Addsubtask stsk " +
                                        "left join Pms_tbl_Addtask tsk on stsk.task=tsk.id " +
                                        "left join  Pms_tbl_AddMilestone mil on tsk.Milestoneid=mil.id " +
                                        "left join Pms_tbl_AddProject proj on mil.ProjectId=proj.id" +
                                        " where stsk.id='" + strid + "' and stsk.status=1 and stsk.archive=0 and stsk.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by stsk.id";

            dsdetails = da.GetDataSetInline(strsql);

            if (dsdetails.Tables[0].Rows.Count > 0)
            {
                hdnmid.Value = dsdetails.Tables[0].Rows[0]["Milestoneid"].ToString();
                hdntask1.Value = dsdetails.Tables[0].Rows[0]["tskid"].ToString();
                milestonenamename = dsdetails.Tables[0].Rows[0]["Subtaskname"].ToString();
                Projectname = dsdetails.Tables[0].Rows[0]["taskname"].ToString();


            }
            ddlMilestone.SelectedValue = hdnmid.Value;
            bindtaskids(hdnmid.Value);
            ddltask1.SelectedValue = hdntask1.Value;
            bindsubtaskids(hdntask1.Value);
            ddlsubtask1.SelectedValue = strid;

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
    public static string getmprojectid(string strid,string strtypes)
    {
        string retid = "0";
        string strsql = "";
        DataAccess da = new DataAccess();
        DataSet dsdetails = new DataSet();
        if (strtypes == "project")
        {
            strsql = "select id, ProjectName   from Pms_tbl_AddProject where id='" + strid + "' and status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by id";

            dsdetails = da.GetDataSetInline(strsql);

            if (dsdetails.Tables[0].Rows.Count > 0)
            {
                retid = dsdetails.Tables[0].Rows[0]["id"].ToString();


            }

           

        }
        else if (strtypes == "milestone")
        {
            retid = "0";
            strsql = "";
            strsql = "select mil.id,mil.projectid,Milestone,proj.ProjectName   from Pms_tbl_AddMilestone mil " +
                "left join Pms_tbl_AddProject proj on mil.ProjectId=proj.id" +
                " where mil.id='" + strid + "' and mil.status=1 and mil.archive=0 and mil.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by mil.id";

            dsdetails = da.GetDataSetInline(strsql);

            if (dsdetails.Tables[0].Rows.Count > 0)
            {
                retid = dsdetails.Tables[0].Rows[0]["projectid"].ToString();
               

            }
           
        }
        else if (strtypes == "task")
        {

            retid = "0";
            strsql = "";
            strsql = "select tsk.id,tsk.projectid,taskname,mil.Milestone,proj.ProjectName,tsk.Milestoneid   from Pms_tbl_Addtask tsk " +
                            "left join  Pms_tbl_AddMilestone mil on tsk.Milestoneid=mil.id " +
                            "left join Pms_tbl_AddProject proj on mil.ProjectId=proj.id" +
                            " where tsk.id='" + strid + "' and tsk.status=1 and tsk.archive=0 and tsk.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by tsk.id";

            dsdetails = da.GetDataSetInline(strsql);

            if (dsdetails.Tables[0].Rows.Count > 0)
            {
                retid = dsdetails.Tables[0].Rows[0]["projectid"].ToString();


            }
            
        }
        else if (strtypes == "subtask")
        {
            retid = "0";
            strsql = "";

            strsql = "select stsk.id,tsk.projectid,stsk.task tskid,Subtaskname,taskname,mil.Milestone,proj.ProjectName,tsk.Milestoneid   from Pms_tbl_Addsubtask stsk " +
                                        "left join Pms_tbl_Addtask tsk on stsk.task=tsk.id " +
                                        "left join  Pms_tbl_AddMilestone mil on tsk.Milestoneid=mil.id " +
                                        "left join Pms_tbl_AddProject proj on mil.ProjectId=proj.id" +
                                        " where tsk.id='" + strid + "' and tsk.status=1 and tsk.archive=0 and tsk.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by tsk.id";

            dsdetails = da.GetDataSetInline(strsql);

            if (dsdetails.Tables[0].Rows.Count > 0)
            {
                retid = dsdetails.Tables[0].Rows[0]["projectid"].ToString();


            }
            

        }
        return retid;
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
        string mprojectid = "0"; 
       
        if (mdetails.stid != "" && mdetails.stid != "0")
        {
            mprojectid =  getmprojectid(mdetails.stid,"subtask");
            mdetails.pid = "0";
            mdetails.mid = "0";
            mdetails.tid = "0";
        }
        else if (mdetails.tid != "" && mdetails.tid != "0")
        {
            mprojectid = mdetails.mprojectid; //getmprojectid(mdetails.tid, "task");
            mdetails.pid = "0";
            mdetails.mid = "0";
            mdetails.stid = "0";
        }
        else if (mdetails.mid != "" && mdetails.mid != "0")
        {
            mprojectid = mdetails.mprojectid; //getmprojectid(mdetails.mid, "milestone");
            mdetails.pid = "0";
            mdetails.tid = "0";
            mdetails.stid = "0";
        }
        else
        {
            mprojectid = mdetails.mprojectid; //mdetails.did;
            mdetails.pid = mdetails.did;
            mdetails.mid = "0";
            mdetails.tid = "0";
            mdetails.stid = "0";
        }
        SqlParameter[] prm = new SqlParameter[14];
            prm[0] = new SqlParameter("@Action", entrytype);
            prm[1] = new SqlParameter("@id", editid);
            prm[2] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
            prm[3] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
            prm[5] = new SqlParameter("@projectid", mdetails.pid);
            prm[6] = new SqlParameter("@milestoneid", mdetails.mid);
            prm[7] = new SqlParameter("@taskid", mdetails.tid);
            prm[8] = new SqlParameter("@subtaskid", mdetails.stid);
           
        if (mdetails.dmid == "" || mdetails.dmid == "0")
        {
            prm[9] = new SqlParameter("@dmilestoneid", DBNull.Value);
        }
        else
        {
            prm[9] = new SqlParameter("@dmilestoneid", mdetails.dmid);
        }

        if (mdetails.dtaskid == "" || mdetails.dtaskid == "0")
        {
            
            prm[10] = new SqlParameter("@dtaskid", DBNull.Value);
        }
        else
        {
            prm[10] = new SqlParameter("@dtaskid", mdetails.dtaskid);
        }
        if (mdetails.dsubtaskid == "" || mdetails.dsubtaskid == "0")
            {
                prm[11] = new SqlParameter("@dsubtaskid", DBNull.Value);
            }
            else
            {
                prm[11] = new SqlParameter("@dsubtaskid", mdetails.dsubtaskid);
            }
            prm[12] = new SqlParameter("@MProjectId", mprojectid);
            prm[13] = new SqlParameter("ReturnValue", SqlDbType.Int);
            prm[13].Direction = ParameterDirection.ReturnValue;
            int retval1 = da1.executeDMLQuery("sp_tbl_dependencyST_DML", prm);
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

        strsql = "update  tbl_dependencyST set  status=0,archive=1  where  id='" + RecordId + "' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
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

        strsql = "select * from  tbl_dependencyST where  id='" + RecordId + "' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
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
   
    protected void btnedit_Click(object sender, EventArgs e)
    {
        //SaveLogText("editp");
        string strpid = "0";
        string strmid = "0";
        string strtid = "0";
        string strstid = "0";
       string strsql = "select id pid,'0' mid,'0' tid,'0' stid    from Pms_tbl_AddProject where id='" + hdndependid.Value + "' and status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by id";
       
        if (hdnsubtask1.Value!=""&& hdnsubtask1.Value!="0")
        {
            strsql = "select stsk.id stid, tsk.id tid,tsk.Milestoneid mid,mil.ProjectId pid   from Pms_tbl_Addsubtask stsk " +
                                                    "left join Pms_tbl_Addtask tsk on stsk.task=tsk.id " +
                                                    "left join  Pms_tbl_AddMilestone mil on tsk.Milestoneid=mil.id " +
                                                    "left join Pms_tbl_AddProject proj on mil.ProjectId=proj.id" +
                                                    " where stsk.id='" + hdnsubtask1.Value + "' and stsk.status=1 and stsk.archive=0 and stsk.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by stsk.id";

        }
        else if (hdntask1.Value != "" && hdntask1.Value != "0")
        {
            strsql = "select '0' stid, tsk.id tid,tsk.Milestoneid mid,mil.ProjectId pid   from Pms_tbl_Addtask tsk " +
                                                   "left join  Pms_tbl_AddMilestone mil on tsk.Milestoneid=mil.id " +
                                                   "left join Pms_tbl_AddProject proj on mil.ProjectId=proj.id" +
                                                   " where tsk.id='" + hdntask1.Value + "' and tsk.status=1 and tsk.archive=0 and tsk.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by tsk.id";
           
        }
        else if (hdnMilestoneid.Value != "" && hdnMilestoneid.Value != "0")
        {

            strsql = "select '0' stid, tsk.'0' tid ,mil.id mid,ProjectId pid   from Pms_tbl_AddMilestone mil " +
                                   "left join Pms_tbl_AddProject proj on mil.ProjectId=proj.id" +
                                   " where mil.id='" + hdnMilestoneid.Value + "' and mil.status=1 and mil.archive=0 and mil.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by mil.id";


        }

        DataSet dsdt= da.GetDataSetInline(strsql);
        if(dsdt.Tables[0].Rows.Count>0)
        {
            strpid = dsdt.Tables[0].Rows[0]["pid"].ToString();
            strmid = dsdt.Tables[0].Rows[0]["mid"].ToString();
            strtid = dsdt.Tables[0].Rows[0]["tid"].ToString();
            strstid = dsdt.Tables[0].Rows[0]["stid"].ToString();
        }
        else
        {
            strpid = "0";
            strmid = "0";
            strtid = "0";
            strstid = "0";
        }

        try
        {
           
            bindmilestone(strpid);
            ddlMilestone.SelectedValue = strmid;
        }
        catch
        {

        }
        try
        {


            bindtaskids(ddlMilestone.SelectedValue);
            ddltask1.SelectedValue = strtid;
        }
        catch (Exception ex)
        {
            SaveLogText("editp-" + ex.Message);
        }
        try
        {

            bindsubtaskids(ddltask1.SelectedValue);
            ddlsubtask1.SelectedValue = strstid;
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
    public static string getcheckid(string ids,string strtype)
    {
        string retid = "0";
        DataAccess da = new DataAccess();
        string strsqlM = "select * from tbl_dependencyST where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and milestoneid>0 and  milestoneid='" + ids + "' and status=1 and archive=0";
        if(strtype=="task")
        {
            strsqlM = "select * from tbl_dependencyST where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and taskid='" + ids + "'  and status=1 and archive=0";
        }
        else if (strtype == "subtask")
        {
            strsqlM = "select * from tbl_dependencyST where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and subtaskid='" + ids + "'  and status=1 and archive=0";
        }
        try
        {
            DataSet dsM = da.GetDataSetInline(strsqlM);
            retid = dsM.Tables[0].Rows[0]["id"].ToString();
        }
        catch { }
        return retid;
    }
    protected void ddlmilestone_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strid = "0";
        try
        {
            bindtaskids(ddlMilestone.SelectedValue);
            strid = getcheckid(ddlMilestone.SelectedValue, "milestone");
        }
        catch { }
       
        if (strid != "" && strid != "0")
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "txt", "bindmilestone('na'),BinddetailsExist(" + strid + ")", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "txt", "bindmilestone('na')", true);
        }
    }
    protected void ddltask1_SelectedIndexChanged(object sender, EventArgs e)
    {

        
        bindsubtaskids(ddltask1.SelectedValue);
        string strid = getcheckid(ddltask1.SelectedValue, "task");
        string strtyp = "all";
        if(ddltask1.SelectedValue==""||ddltask1.SelectedValue == "0")
        {
            strtyp = "na";
        }
        if (strid != "" && strid != "0")
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "txt", "bindmilestone('"+ strtyp + "'),BinddetailsExist(" + strid + ")", true);
        }
        else
        {
           
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "txt", "bindmilestone('" + strtyp + "')", true);
            
        }
    }
    protected void ddlsubtask1_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strid = getcheckid(ddlsubtask1.SelectedValue, "subtask");
        string strtyp = "all";
        //if (ddlsubtask1.SelectedValue == "" || ddlsubtask1.SelectedValue == "0")
        //{
        //    strtyp = "na";
        //}
        SaveLogText("strddltask1-"+ ddlsubtask1.SelectedValue+","+ strid);
        if (ddltask1.SelectedValue == "" || ddltask1.SelectedValue == "0")
        {
            strtyp = "na";
        }
        if (strid != "" && strid != "0")
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "txt", "bindmilestone('"+ strtyp + "'),BinddetailsExist(" + strid + ")", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "txt", "bindmilestone('" + strtyp + "')", true);
        }

    }
    protected void bindtaskids(string mid)
    {
        DataSet dsN = new DataSet();
        try
        {
            string strsql = "select id, taskname name   from Pms_tbl_Addtask where  status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and milestoneid='" + mid + "' and (taskname is not null and taskname<>'') order by id";
            //SaveLogText("strsqltest1-"+ strsql);
             dsN = da.GetDataSetInline(strsql);
            
        }
        catch
        {
            strdivtsk = "style='display:none'";
        }
        if (dsN.Tables[0].Rows.Count > 0)
        {
            strdivtsk = "style='display:'";
            //Visible = true;
        }
        
        cmf.BindDropDownList(ddltask1, dsN, "name", "id");
        ListItem li2 = new ListItem("", "0");
        ddltask1.Items.Insert(0, li2);



    }
    protected void bindsubtaskids(string tid)
    {
        try
        {
            string strsql = "select id, Subtaskname name  from Pms_tbl_Addsubtask where  status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and task='" + tid + "' order by id";

            DataSet dsN = da.GetDataSetInline(strsql);
            cmf.BindDropDownList(ddlsubtask1, dsN, "name", "id");
            ListItem li2 = new ListItem("", "0");
            ddlsubtask1.Items.Insert(0, li2);
            //if(dsN.Tables[0].Rows.Count==0)
            //{
            //    strsubtask = "style='display:none'";
            //}
        }
        catch
        {
            ListItem li2 = new ListItem("", "0");
            ddlsubtask1.Items.Insert(0, li2);
        }




    }
    public class depentencydetails
    {
        public string id { get; set; }
        public string mprojectid { get; set; }
        public string did { get; set; }
        public string dtype { get; set; }
        public string pid { get; set; }
        public string mid { get; set; }
        public string tid { get; set; }
        public string stid { get; set; }
        public string dmid { get; set; }
        public string dtaskid { get; set; }
        public string dsubtaskid { get; set; }


    }
}

