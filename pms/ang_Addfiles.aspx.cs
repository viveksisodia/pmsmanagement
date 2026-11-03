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

public partial class ang_Addfiles : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataAccess da = new DataAccess();
    public CommonFunction cmf = new CommonFunction();
    SqlParameter[] prm, prm1;
    public function fns = new function();

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
        

        if (!Page.IsPostBack)
        {
            string strcountid = "filescount_";

            
            try
            {
                if (HttpContext.Current.Request.QueryString["callfrom"] != null)
                {
                    hdncallfrom.Value = HttpContext.Current.Request.QueryString["callfrom"];
                }
                else
                {
                    hdncallfrom.Value = "";
                }
               
            }
            catch { }
            try
            {
                if (HttpContext.Current.Request.QueryString["pid"] != null)
                {
                    hdnpid.Value = HttpContext.Current.Request.QueryString["pid"];
                   
                }
            }
            catch { }
            try
            {
                
                hdnfid.Value = HttpContext.Current.Request.QueryString["id"];
                
            }
            catch { }
            try
            {
                
                hdntype.Value = HttpContext.Current.Request.QueryString["type"];
            }
            catch { }
            try
            {
                if(HttpContext.Current.Request.QueryString["mid"]!=null)
                hdnmid.Value = HttpContext.Current.Request.QueryString["mid"];
            }
            catch { }
            try
            {
                if (HttpContext.Current.Request.QueryString["tid"] != null)
                    hdntid.Value = HttpContext.Current.Request.QueryString["tid"];
            }
            catch { }
            strcountid += hdnpid.Value;
            if(hdnmid.Value!="")
            {
                strcountid += "_" + hdnmid.Value;
            }
            if (hdntid.Value != "")
            {
                strcountid += "_" + hdntid.Value;
            }
            strcountid += "_" + hdnfid.Value;
            hdncountid.Value = strcountid;
        }
        
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
    public static string bindfiledata(string pid,string id, string type)
    {
        
        DataAccess da = new DataAccess();
        string retval = "";
        string filepath = "";
        if (type != string.Empty)
        {
            if (type == "milestone")
                filepath = "/pms/MileImages/" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "/" + id + "/";
            else if (type == "task")
                filepath = "/pms/TaskImages/" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "/" + id + "/";
            else if (type == "subtask")
                filepath = "/pms/SubtaskImages/" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "/" +id + "/";

        }
        string strsqlfils = "select * from Pms_tbl_projectfiles where regid='"+ HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and status=1 and archive=0 and pid='" + pid + "' and fentryid='" +id + "' and ftype='" + type + "' order by id desc";
        SaveLogText("strsqlfils-" + strsqlfils);
        DataSet dsfils = da.GetDataSetInline(strsqlfils);
        string strdatafils = "";
        int countrow = 0;
        
            //"<thead>" +
            //"<tr><th></th><th>File</th><th style='width:30px'></th></tr>" +
            //"</thead>";
        if (dsfils.Tables[0].Rows.Count > 0)
        {
            strdatafils += dsfils.Tables[0].Rows.Count.ToString()+"^";
            strdatafils += "<table class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm datatable'>";
            for (int i = 0; i < dsfils.Tables[0].Rows.Count; i++)
            {
                try
                {
                    strdatafils += "<tr id='tr_" + countrow.ToString() + "'>";
                    strdatafils += @"<td width='45px'><div class='attch_icon'>";
                    if (dsfils.Tables[0].Rows[i]["fext"].ToString().ToUpper() == ".PDF")
                    {
                        strdatafils += "<i class='bi bi-file-earmark-pdf-fill'></i></div></td>";
                    }
                    else if (dsfils.Tables[0].Rows[i]["fext"].ToString().ToUpper() == ".PNG" || dsfils.Tables[0].Rows[i]["fext"].ToString().ToUpper() == ".JPG" || dsfils.Tables[0].Rows[i]["fext"].ToString().ToUpper() == ".JPEG")
                    {
                        strdatafils += "<i class='bi bi-file-earmark-image'></i></div></td>";
                    }
                    else if (dsfils.Tables[0].Rows[i]["fext"].ToString().ToUpper() == ".DOCX" || dsfils.Tables[0].Rows[i]["fext"].ToString().ToUpper() == ".DOC")
                    {
                        strdatafils += "<i class='bi bi-file-earmark-word-fill'></i></div></td>";
                    }
                    else if (dsfils.Tables[0].Rows[i]["fext"].ToString().ToUpper() == ".XLS" || dsfils.Tables[0].Rows[i]["fext"].ToString().ToUpper() == ".XLSX")
                    {
                        strdatafils += "<i class='bi bi-file-earmark-excel-fill'></i></div></td>";
                    }
                    else
                    {
                        strdatafils += "<i class='bi bi-file-earmark-image-fill'></i></div></td>";
                    }
                    string strname =dsfils.Tables[0].Rows[i]["fileurl"].ToString();
                    strdatafils += "<td>"+ strname.Replace(filepath,"").Replace(dsfils.Tables[0].Rows[i]["fext"].ToString(),"") + "</td>";
                     strdatafils += @" <td class='text-right' style='width:30px;vertical-align:middle;'>";
                     string hrf = "";
                    if (dsfils.Tables[0].Rows[i]["fext"].ToString().ToUpper() == ".DOCX" || dsfils.Tables[0].Rows[i]["fext"].ToString().ToUpper() == ".DOC")
                    {
                        hrf = "/pms/ValidateDocument.aspx?isview=1&count=1&id=" + dsfils.Tables[0].Rows[i]["id"].ToString();
                        strdatafils += @"<a id='imgnew1_" + dsfils.Tables[0].Rows[i]["id"].ToString() + "' href='" + hrf + "' target='_blank' class='btn btn-success btn-icon btn-sm'><i class='ri-eye-line' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' data-bs-original-title='View'></i></a>";
                    }
                    else if (dsfils.Tables[0].Rows[i]["fext"].ToString().ToUpper() == ".XLS" || dsfils.Tables[0].Rows[i]["fext"].ToString().ToUpper() == ".XLSX")
                    {
                        hrf = "/pms/ValidateDocument.aspx?isview=1&count=1&id=" + dsfils.Tables[0].Rows[i]["id"].ToString();
                        strdatafils += @"<a id='imgnew1_" + dsfils.Tables[0].Rows[i]["id"].ToString() + "' href='" + hrf + "' target='_blank' class='btn btn-success btn-icon btn-sm'><i class='ri-eye-line' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' data-bs-original-title='View'></i></a>";
                    }
                    else if (dsfils.Tables[0].Rows[i]["fext"].ToString().ToUpper() == ".PDF")
                    {
                        hrf = "/pms/ValidateDocument.aspx?isview=1&count=1&id=" + dsfils.Tables[0].Rows[i]["id"].ToString();
                        strdatafils += @"<a id='imgnew1_" + dsfils.Tables[0].Rows[i]["id"].ToString() + "' href='" + hrf + "' target='_blank' class='btn btn-success btn-icon btn-sm'><i class='ri-eye-line' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' data-bs-original-title='View'></i></a>";

                    }
                    else
                    {
                        strdatafils += @"<a id='imgnew1_" + dsfils.Tables[0].Rows[i]["id"].ToString() + "' href='" + strname + "' target='_blank' class='btn btn-success btn-icon btn-sm'><i class='ri-eye-line' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' data-bs-original-title='View'></i></a>";
                    }
                    strdatafils +="<a href='javascript:void(0);' onclick=removedocumentnew(\"" + dsfils.Tables[0].Rows[i]["id"].ToString() + "\") class='btn btn-danger btn-icon btn-sm ms-1 me-1'><i class='ri-delete-bin-5-line' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' data-bs-original-title='Delete'></i></a>";
                   // strdatafils += @"<a href='javascript:void(0);' class='btn btn-primary btn-icon btn-sm' id='imgdwn" + dsfils.Tables[0].Rows[i]["id"].ToString() + "' onclick='downloaddocumentnew(\"" + dsfils.Tables[0].Rows[i]["id"].ToString() + "\",\"" + strname + "\")'><i class='bi bi-download' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' data-bs-original-title='Download'></i></a> ";

                    strdatafils += "</td></tr>";
                }
                catch
                { }

            }
            strdatafils += "</table>";
            //Response.Write(strdatafils);
        }
        else
        {
            strdatafils += "^";
        }
        
        return retval= strdatafils;
    }

}

