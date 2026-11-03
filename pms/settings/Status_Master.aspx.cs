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
public partial class Status_Master : System.Web.UI.Page
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
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            //ddlicons.Items.Clear();

            //ddlicons.Items.Add(new ListItem("None", "bi-dash-circle"));
            //ddlicons.Items.Add(new ListItem("Hour glass", "bx bx-hourglass"));
            //ddlicons.Items.Add(new ListItem("Forward", "ri-share-forward-line"));
            //ddlicons.Items.Add(new ListItem("Achive", "bx bx-archive"));
            //ddlicons.Items.Add(new ListItem("Check", "bx bx-check-circle"));
            //ddlicons.Items.Add(new ListItem("Clock", "bi-clock"));
            //ddlicons.Items.Add(new ListItem("Bell", "bi bi-bell"));
            //ddlicons.Items.Add(new ListItem("Tag", "bi bi-tag"));
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
       
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
    public static void SavePrintingMaster(printingColor printingdetails)
    {
        DataAccess da1 = new DataAccess();
        string editid = (printingdetails.id==""?"0": printingdetails.id);
        string entrytype = "Insert";
        if (editid != "" && editid != "0")
        {
            entrytype = "Update";
        }
        SaveLogText("statusdefault:"+ printingdetails.isdefault);
        SqlParameter[] prm = new SqlParameter[10];
        prm[0] = new SqlParameter("@id", editid);
        prm[1] = new SqlParameter("@ProjectStatus", printingdetails.StatusName);
        prm[2] = new SqlParameter("@PrintColor", "#"+printingdetails.PrintColor);
        prm[3] = new SqlParameter("@PrintIcon",printingdetails.PrintIcon);
        prm[4] = new SqlParameter("@iscomplete", (printingdetails.iscomplete==""?"0": printingdetails.iscomplete));
        prm[5] = new SqlParameter("@isdefault", (printingdetails.isdefault == ""?"0": printingdetails.isdefault));
        prm[6] = new SqlParameter("@isrework", (printingdetails.isrework == ""?"0": printingdetails.isrework));
        prm[7] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
        prm[8] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[9] = new SqlParameter("@type", entrytype);

        da1.executeDMLQuery("Insert_projectstatus_usp", prm);
    }




    [WebMethod]
    public static string bindPrintingMasterData()
    {
        string projectdata = "";
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        DataSet ds = da.GetDataSet("[pmstbl_projectstatus_dashbord]", prm);
        projectdata += "<table cellpadding='0' cellspacing='0' border='0'width='100%'  class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm datatable'>";
        projectdata += "<thead>";
        projectdata += "<tr>";

        projectdata += "<th>S.No</th>";
        projectdata += "<th>Status Name </th>";
        //projectdata += "<th>Type</th>";
      
        projectdata += "<th>Printing Color</th>";
        projectdata += "<th>Printing Icon</th>";
        projectdata += "<th>Action</th>";
        projectdata += "</tr>";
        projectdata += "</thead>";
        projectdata += "<tbody>";
        if (ds.Tables[0].Rows.Count > 0)
        {


            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                projectdata += "<tr>";



                projectdata += "<td valign='top'>";
                projectdata += (i + 1);
                projectdata += "</td>";

                projectdata += "<td valign='top'>";
                projectdata += ds.Tables[0].Rows[i]["ProjectStatus"].ToString();
                projectdata += "</td>";


                //projectdata += "<td valign='top'>";
                //projectdata += "<span class='badge badge-label' style='background:"+ ds.Tables[0].Rows[i]["PrintColor"].ToString() + ";'><i class='ti ti-point-filled'></i></span></a>";
                //projectdata += "<span class='cursor-pointer'>"+ ds.Tables[0].Rows[i]["PrintColor"].ToString() + "</span>";
                //projectdata += "</td>";
                
                projectdata += "<td>";
                projectdata += "<div class='gap-1 flex-shrink-0 align-self-center' id='lblAddcolor_" + ds.Tables[0].Rows[i]["id"].ToString() + "'>";
                projectdata += "<a href = 'javascript:void(0);' type='button' id='btncolor_" + ds.Tables[0].Rows[i]["id"].ToString() + "' onclick='Binddetails_Addcolor(" + ds.Tables[0].Rows[i]["id"].ToString() + ",\"" + ds.Tables[0].Rows[i]["PrintColor"].ToString() + "\",this.id)' data-bs-toggle='modal' data-bs-target='#addcolor'><span style='width:20px;height:20px;display:inline-block;background:" + ds.Tables[0].Rows[i]["PrintColor"].ToString() + ";margin-left:5px;'></span></a>";
                projectdata += "</div>";
              projectdata += "</td>";
              projectdata += "<td>";
              projectdata += "<label id='lblicon'><i class='"+ ds.Tables[0].Rows[i]["PrintIcon"].ToString() + "'></i></label>";
              projectdata += "</td>";




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
    public static string UpdateColor(string update_id = "", string Color = "")
    {
        string retval = "";

        DataAccess da1 = new DataAccess();
        string editid = update_id;
        string entrytype = "Update";
        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", editid);
        prm[1] = new SqlParameter("@PrintColor", "#"+Color);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pmstbl_status_colorupdate", prm);

        string strcolor = "select * from Pms_tbl_Projectstatus where  status=1 and archive=0  and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id = '" + editid + "'  ";
        DataSet dsscolor = da1.GetDataSetInline(strcolor);
        if (dsscolor.Tables[0].Rows.Count > 0)
        {
            for (int k = 0; k < dsscolor.Tables[0].Rows.Count; k++)
            {


                retval += "<div class='gap-1 flex-shrink-0 align-self-center'>";
                retval += "<a href = 'javascript:void(0);' type='button' id='btncolor_" + dsscolor.Tables[0].Rows[k]["id"].ToString() + "' onclick='Binddetails_Addcolor(" + dsscolor.Tables[0].Rows[k]["id"].ToString() + ",\"" + dsscolor.Tables[0].Rows[k]["PrintColor"].ToString() + "\",this.id)' data-bs-toggle='modal' data-bs-target='#addcolor'><span style='width:20px;height:20px;display:inline-block;background:" + dsscolor.Tables[0].Rows[k]["PrintColor"].ToString() + ";margin-left:5px;'></span></a>";
                retval += "</div>";
                retval += "</td>";

            }

        }




        return retval;



    }

    [WebMethod]
    public static string BindPrintingdetail(int RecordId = 0)
    {

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "select * from Pms_tbl_Projectstatus where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + RecordId + "'";
        ds = da.GetDataSetInline(strsql);


        return ds.GetXml();
    }

    [WebMethod]
    public static string DeletePrintingMaster(int RecordId = 0)
    {
        string retval = "0";

        DataAccess da = new DataAccess();
        DataSet ds;

        string strsql = "update  Pms_tbl_Projectstatus set  status=0,archive=1  where  id='" + RecordId + "' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        ds = da.GetDataSetInline(strsql);

        return retval;
    }
}
public class printingColor
{
    public string id { get; set; }
    public string StatusName { get; set; }
    public string PrintColor { get; set; }
    public string PrintIcon { get; set; }
    public string iscomplete { get; set; }
    public string isdefault { get; set; }
    public string isrework { get; set; }

}