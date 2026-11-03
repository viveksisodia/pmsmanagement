using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using CommonClass;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using Ionic.Zip;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
public partial class viewsalesorder : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataAccess da = new DataAccess();
   public CommonFunction cmf = new CommonFunction();
    SqlParameter[] prm, prm1;
    public function fns = new function(); 

    public string prvdata;
    public string prvdataforpdf;
    public string strurldata;
    public string iscancel = "0";
    public string islock1 = "0";
    public string islock = "0";
    public string attfilesdata = "";
  
    public string prvdatawithoutheader;
    public string showrental = "0";
    public string ratetext = "Rate";
    public string showisindia = "0";
    public string showgstsummery = "0";
    public int isdutyslip;
    public int isdutysliprajshree;
    public string customermobile = "";
    public int ddlproformainvoice = 1;
    public int isinvoiced = 0;
    public int proformainvoicecreate = 0;
    public int mainvoicecreate = 0;
    public int isapproveshow;
    public int iscatshow = 0;
    public int isimgshow = 0;
    public string showbranch = "display:none;";
    public int isAAA = 0;
    public int isanmol = 0;
    public int iscoslight = 0;

    public int module1;
    public int module1view;
    //public int module1Create;
   // public int module1Edit;
   // public int module1Delete;
    public int module2Create;

    public int module1create = 0;
    public int module1update = 0;
    public int module1delete = 0;
    public int module1import = 0;
    public int module1export = 0;
    public string displaycreate = "none";
    public string displayupdate = "none";
    public string displaydelete = "none";
    public string displayimport = "none";
    public string displayexport = "none";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Cookies["user"] == null || Request.Cookies["cadmin"] == null)
        {
            Response.Redirect("login.aspx");
        }
        bindpermision();
        if (fns.getcountryid() == 6)
        {
            string strabsoluteurl = Request.Url.ToString().Replace("viewsalesorder.aspx", "Egypt/viewsalesorder.aspx");
            Response.Redirect(strabsoluteurl);
        }
        lbladdorder.Text = fns.getltext("+ New Sale Order", "2");
        lblallorder.Text = fns.getltext("All Sale Order", "2");
        isdutyslip = 0;
        isdutysliprajshree = 0;


        if (Request.Cookies["cadmin"]["id"].ToString() == "2224" || Request.Cookies["cadmin"]["id"].ToString() == "2219" || Request.Cookies["cadmin"]["id"].ToString() == "2214")//HiraFilter
        {
            string strabsoluteurl = Request.Url.ToString().Replace("viewsalesorder.aspx", "viewsalesorder_ExpertAdvice.aspx");
            Response.Redirect(strabsoluteurl);
        }

        if (Request.QueryString["type"] != null)
        {
            if (cmf.Decrypt(Request.QueryString["type"].ToString()) == "approval")
            {
                pnlapprove1.Visible = false;
                pnlapprove2.Visible = true;
            }
            else
            {
                pnlapprove1.Visible = true;
                pnlapprove2.Visible = false;
            }
        }
        else
        {
            pnlapprove1.Visible = true;
            pnlapprove2.Visible = false;
        }

        if (fns.getcountryid() == 1)
        {
            showisindia = "1";
        }
        //if (Request.Cookies["cadmin"]["id"].ToString() == "147")
        //{
        //    isdutyslip = 1;
        //}
        //if (Request.Cookies["cadmin"]["id"].ToString() == "157")
        //{
        //    isdutysliprajshree = 1; 
        //}
        //if (Request.Cookies["cadmin"]["id"].ToString() == "98")
        //{
        //    lbltitle.Text = fns.getltext("View Bill of Supply", "2");
        //    lblallorder.Text = "All Bill of Supply";
        //    lbladdorder.Text = "+ New Bill of Supply";
        //}


        string strsqlPref = "select * from tbl_Preferencesitems where regid=" + Request.Cookies["cadmin"]["id"].ToString() + " and status=1 and archive=0";
        DataSet dsPref = da.GetDataSetInline(strsqlPref);
        if (dsPref.Tables[0].Rows.Count > 0)
        {
            if (dsPref.Tables[0].Rows[0]["imginvoicechk"].ToString() == "1")
            {
                isimgshow = 1;
            }
            else
            {
                isimgshow = 0;
            }
            if (dsPref.Tables[0].Rows[0]["chkcatshow"].ToString() == "1")
            {
                iscatshow = 1;
            }
            else
            {
                iscatshow = 0;
            }
        }

        DataSet dsfreference = fns.getgpreference();

        if (dsfreference.Tables[0].Rows.Count > 0)
        {
            //if (dsfreference.Tables[0].Rows[0]["comname"].ToString() == "Sukh")
            //{
            //    islock1 = "1";
            //}

            //if (dsfreference.Tables[0].Rows[0]["comname"].ToString() == "anmol")
            //{
            //    isanmol = 1;
            //}
        

            if (dsfreference.Tables[0].Rows[0]["ddltypeofactivity"].ToString() == "5")
            {
                showrental = "1";
                ratetext = "Rental Value";
                lbltitle.Text = fns.getltext("View Delivery Order", "2");
                lblallorder.Text = "All Delivery Order";
                lbladdorder.Text = "+ New Delivery Order";
                // Response.Write(showrental);

            }
            else if (dsfreference.Tables[0].Rows[0]["ddltypeofactivity"].ToString() == "7")
            {
                showrental = "0";
                ratetext = "Rental Value";
                lbltitle.Text = fns.getltext("View Delivery Order", "2");
                lblallorder.Text = "All Delivery Order";
                lbladdorder.Text = "+ New Delivery Order";
                // Response.Write(showrental);

            }
            else
            {
                showrental = "0";
                lbltitle.Text = fns.getltext("View Sale Order", "2");
                ratetext = "Rate";
            }

            if (dsfreference.Tables[0].Rows[0]["rdlrtaxsummary"].ToString() == "1")
            {
                showgstsummery = "1";
            }
            else
            {
                showgstsummery = "0";
            }
            if (dsfreference.Tables[0].Rows[0]["rdlbranch"].ToString() == "2")
            {
                showbranch = "";

            }
        }


        string strsqlsss = "select * from tbl_tbl_Preferencessalesorder where regid='" + Request.Cookies["cadmin"]["id"].ToString() + "' and status=1 and archive=0";
        //Response.Write(strsqlsss);
        //Response.End();
        DataSet dspreferensedd = da.GetDataSetInline(strsqlsss);
        if (dspreferensedd.Tables[0].Rows.Count > 0)
        {
            ViewState["iscustomeparm"] = "0";
            if (dsfreference.Tables[0].Rows[0]["isfgparameter"].ToString() != "2")
            {
                ViewState["iscustomeparm"] = "1";
            }
            ViewState["isapprovalrequired"] = dspreferensedd.Tables[0].Rows[0]["isapprovalrequired"].ToString();
            //Response.Write(ViewState["isapprovalrequired"]);
            //Response.End();
            if (dspreferensedd.Tables[0].Rows[0]["rdlpschedule"].ToString() == "1")
                pnlpaymentschedule.Visible = true;
            if (dspreferensedd.Tables[0].Rows[0]["isapprovalrequired"].ToString() == "1")
            {
                ViewState["approvalLevel"] = dspreferensedd.Tables[0].Rows[0]["approvalLevel"].ToString();

                //code here
                if (ViewState["isapproved"] != null)
                {
                    if (ViewState["isapproved"].ToString() == "1")
                    {
                        lnkreccived.Visible = true;
                        lblviewhistory.Visible = true;

                        try
                        {
                            lnkreccivedDelivery.Visible = true;
                            LinkButton1.Visible = true;
                        }
                        catch
                        {
                        }

                        if (iscancel == "1")
                        {
                            lnkreccived.Visible = false;
                            lblviewhistory.Visible = false;
                            lnkreccivedDelivery.Visible = false;
                            LinkButton1.Visible = false;
                        }
                    }
                    else
                    {
                        lnkreccived.Visible = false;
                        lblviewhistory.Visible = false;
                        try
                        {
                            lnkreccivedDelivery.Visible = false;
                            LinkButton1.Visible = false;
                        }
                        catch
                        {
                        }
                    }
                }

            }
            else
            {
                ViewState["approvalLevel"] = "0";
            }


            if (dspreferensedd.Tables[0].Rows[0]["isapproval"].ToString() == "1")
            {
                ViewState["isappravelreqired"] = "1";



            }
            else
            {
                ViewState["isappravelreqired"] = "0";

            }


            ddlproformainvoice = 0;

            try
            {
                ddlproformainvoice = Convert.ToInt32(dspreferensedd.Tables[0].Rows[0]["ddlproformainvoice"].ToString());
            }
            catch (Exception ex)
            {

            }

        }
        else
        {
            ViewState["isappravelreqired"] = "0";

        }
      
        //Response.Write("proformainvoicecreate:" + proformainvoicecreate.ToString()+ "/mainvoicecreate:" + mainvoicecreate.ToString()+ "/ddlproformainvoice:"+ ddlproformainvoice.ToString()+"/iscancel:"+ iscancel.ToString());
        //Response.End();
        isapproveshow = 1;
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["id"] != null)
            {
                // && Request.QueryString["type"] == null
                if (Request.QueryString["isview"] != null)
                {
                    updatenotification(cmf.Decrypt(Request.QueryString["isview"]).ToString());
                    if (CheckDelete(cmf.Decrypt(Request.QueryString["id"]).ToString()))
                    {
                        Response.Redirect("/crm/DelRecord.aspx?viewtype=1&id=" + Request.QueryString["id"].ToString());
                    }
                }
            }
            bindresonecancel();
            ViewState["issamestate"] = "0";
            ViewState["backgroundimage"] = "";
            ViewState["headerbackgroundimage"] = "";
            ViewState["footersettingBackgroundImage"] = ""; ViewState["Signaturimage"] = "";
            ViewState["backgroundimagesignature"] = "";
            ViewState["customermobile"] = "";
            //module1 = 0;
            //module1view = 0;
            //module1create = 0;
            //module1update = 0;
            //module1delete = 0;
            //module2Create = 0;
            //bindpwemision();
            binddata();
            bindprv();

            bindhistory();
            try
            {
                bindprvforpdf();
                bindpdf();
                bindpdfonletterhead();
            }
            catch
            {

            }

        }
        if (ViewState["isapprovalrequired"] != null)
        {
            if (ViewState["isapprovalrequired"].ToString() == "1")
            {
                if (ViewState["isapproved"] != null)
                {
                    if (ViewState["isapproved"].ToString() != "1")
                    {
                        isapproveshow = 0;
                    }
                }
            }
        }

        customermobile = ViewState["customermobile"].ToString();
        if (Request.QueryString["email"] != null)
        {
            Response.Redirect("sendmailforapproval.aspx?id=" + Request.QueryString["id"] + "&email=" + Request.QueryString["email"] + "&latype=" + Request.QueryString["latype"] + "&type=2");
        }


        SqlParameter[] prmm = new SqlParameter[2];
        prmm[0] = new SqlParameter("@sid", cmf.Decrypt(Request.QueryString["id"].ToString()));
        prmm[1] = new SqlParameter("@type", "selectforsidrep");

        DataSet dsmo = da.GetDataSet("tbl_manufacturingorderdetails_usp", prmm);
        //  MOno = ds.Tables[0].Rows[i]["prm9"].ToString();
        if (dsmo.Tables[0].Rows.Count > 0)
        {
            // module1Edit = 0;
            module1delete = 0;
        }
        //lblchk.Text = ddlproformainvoice + "," + iscancel + "," + proformainvoicecreate;
       // Response.Write(ddlproformainvoice + "," + iscancel + "," + proformainvoicecreate);

    }
    protected void updatenotification(string recordid = "")
    {


        string strcusmofieldupdate = "update tbl_pnotificationdetails set isview='1',isread='1' where id='" + recordid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and typeid=2 ";

        da.executeDMLQueryinline(strcusmofieldupdate);

        // return strsqltask;
    }
    protected bool CheckDelete(string RecordId = "0")
    {
        string strsql = "select * from tbl_SalesOrderdetails  where id='" + RecordId + "' and status=0 and archive=1";
        DataSet dsdel = da.GetDataSetInline(strsql);
        if (dsdel.Tables[0].Rows.Count > 0)
        {
            return true;
        }

        return false;

    }
    protected void bindpermision()
    {
        String requestpage = Request.RawUrl;

        string strsql = "select Manage_Profile.* from Manage_Profile inner join tbl_employee on Manage_Profile.id=cast(tbl_employee.roleid as int) where tbl_employee.id='" + Request.Cookies["user"]["user_id"] + "'  and Manage_Profile.status=1 and Manage_Profile.archive=0";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {

            if (ds.Tables[0].Rows[0]["id"].ToString() == "1")
            {
                module1 = 1;
                module1view = 1;
                module1create = 1;
                module1update = 1;
                module1delete = 1;
                module1import = 1;
                module1export = 1;
                displaycreate = "";
                displayupdate = "";
                displaydelete = "";
                displayimport = "";
                displayexport = "";

                module2Create = 1;
                proformainvoicecreate = 1;
                mainvoicecreate = 1;
            }
            else
            {
                string strsqlmodule = @"select crm_tbl_roletaskmaster.formname,crm_tbl_roletaskmaster.formurl, isnull(ismanage,0) ismanage,isnull(isschedule,0) isschedule,  isnull(ischangeowner,0) ischangeowner,
                            isnull(formaccess,0) formaccess, isnull(isinsert,0) isinsert, isnull(isupdate,0) isupdate, isnull(isdelete,0) isdelete, isnull(isview,0) isview, isnull(isimport,0) isimport, isnull(isexport,0) isexport, isnull(issendmail,0) issendmail, isnull(isviewmail,0) isviewmail, isnull(isdelmail,0) isdelmail, isnull(isfull,0) isfull, isnull(moduleid,0) moduleid  
                                    from crm_tbl_roleaccessdetails inner join crm_tbl_roletaskmaster on crm_tbl_roleaccessdetails.moduleid=crm_tbl_roletaskmaster.id 
                                    where crm_tbl_roleaccessdetails.status=1 and  crm_tbl_roleaccessdetails.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and crm_tbl_roleaccessdetails.roleid='" + ds.Tables[0].Rows[0]["id"].ToString() + "' and entrytype='form' and moduleid in(8,9,10)";
            DataSet dsmodule = da.GetDataSetInline(strsqlmodule);
               
                //string strsqlmodule = "select * from tbl_rolepermisionsdetails where regid='" + Request.Cookies["cadmin"]["id"].ToString() + "' and profileid='" + ds.Tables[0].Rows[0]["id"].ToString() + "' and moduleid in('6','4')";
                //DataSet dsmodule = da.GetDataSetInline(strsqlmodule);
                if (dsmodule.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsmodule.Tables[0].Rows.Count; i++)
                {
                    if(dsmodule.Tables[0].Rows[i]["moduleid"].ToString() == "8")
                    {
                        if (dsmodule.Tables[0].Rows[i]["formaccess"].ToString() == "1" || dsmodule.Tables[0].Rows[i]["formaccess"].ToString() == "True")
                        {
                            module1 = 1;
                            if (dsmodule.Tables[0].Rows[i]["isview"].ToString() == "1" || dsmodule.Tables[0].Rows[i]["isview"].ToString() == "True")
                            {
                                module1 = 1;
                                module1view = 1;
                            }
                            if (dsmodule.Tables[0].Rows[i]["isinsert"].ToString() == "1" || dsmodule.Tables[0].Rows[i]["isinsert"].ToString() == "True")
                            {
                                module1create = 1;
                                displaycreate = "";
                            }
                            if (dsmodule.Tables[0].Rows[i]["isupdate"].ToString() == "1" || dsmodule.Tables[0].Rows[i]["isupdate"].ToString() == "True")
                            {
                                module1update = 1;
                                displayupdate = "";
                            }
                            if (dsmodule.Tables[0].Rows[i]["isdelete"].ToString() == "1" || dsmodule.Tables[0].Rows[i]["isdelete"].ToString() == "True")
                            {
                                module1delete = 1;
                                displaydelete = "";
                            }
                            if (dsmodule.Tables[0].Rows[i]["isimport"].ToString() == "1" || dsmodule.Tables[0].Rows[i]["isimport"].ToString() == "True")
                            {
                                module1import = 1;
                                displayimport = "";
                            }
                            if (dsmodule.Tables[0].Rows[i]["isexport"].ToString() == "1" || dsmodule.Tables[0].Rows[i]["isexport"].ToString() == "True")
                            {
                                module1export = 1;
                                displayexport = "";
                            }

                            if (module1view == 0)
                            {
                                Response.Redirect("/crm/accessdenied.aspx");
                            }
                        }
                        else
                        {
                            module1 = 0;
                            module1view = 0;
                            module1create = 0;
                            module1update = 0;
                            module1delete = 0;
                            module1import = 0;
                            module1export = 0;
                            module2Create = 1;

                            displaycreate = "none";
                            displayupdate = "none";
                            displaydelete = "none";
                            displayimport = "none";
                            displayexport = "none";
                            Response.Redirect("/crm/accessdenied.aspx");
                        }
                    }
                   else if (dsmodule.Tables[0].Rows[i]["moduleid"].ToString() == "9")
                    {
                        if (dsmodule.Tables[0].Rows[i]["isinsert"].ToString() == "1" || dsmodule.Tables[0].Rows[i]["isinsert"].ToString() == "True")
                        {
                            proformainvoicecreate = 1;
                        }
                       
                    }
                    else if (dsmodule.Tables[0].Rows[i]["moduleid"].ToString() == "10")
                    {
                        if (dsmodule.Tables[0].Rows[i]["isinsert"].ToString() == "1" || dsmodule.Tables[0].Rows[i]["isinsert"].ToString() == "True")
                        {
                            mainvoicecreate = 1;
                        }
                       
                    }
                   
                }

            }
             }


        }



        //Response.Write("ddlproformainvoice:"+proformainvoicecreate.ToString()+ "/mainvoicecreate:" + mainvoicecreate.ToString()+ "/ddlproformainvoice:"+ ddlproformainvoice.ToString()+"/iscancel:"+ iscancel.ToString());
        //Response.End();


    }
    protected string showparm(string prm1, string prm2, string prm3, string prm4, string prm5, string prm6, string prm7, string prm8, string prm9)
    {
        string strret = "";
        try
        {

            List<string> prm = new List<string> { prm1, prm2, prm3, prm4, prm5, prm6, prm7, prm8, prm9 };

            string stpramval = "";
            for (int pr = 0; pr < 9; pr++)
            {
                string prcol = "prm" + pr;
                if (prm[pr] != "0" && prm[pr] != String.Empty)
                {
                    if (stpramval != string.Empty)
                    {
                        stpramval += "-" + getparmnamevalue(prm[pr]);
                    }
                    else
                    {
                        stpramval += getparmnamevalue(prm[pr]);
                    }
                }

            }
            strret = strret + " " + stpramval;
        }
        catch
        {

        }

        return strret;


    }
    protected string showreceiveqty(string poid, string itemid, string rate, string prm1, string prm2, string prm3, string prm4, string prm5, string prm6, string prm7, string prm8, string prm9)
    {
        string lblreceivedvalue = "0";
        try
        {
            List<string> prm = new List<string> { prm1, prm2, prm3, prm4, prm5, prm6, prm7, prm8, prm9 };

            string strsql = "select isnull(sum(isnull(quantity,0)),0) as totalreceivedquanty from tbl_invoiceorderdetailsdeliveryorderitemsdetails where itemid='" + itemid + "' ";
            for (int k = 0; k < prm.Count; k++)
            {
                if (prm[k] != null)
                {

                    string prmname = "prm" + (k + 1);

                    strsql += " and isnull(" + prmname + ",0)='" + prm[k] + "' ";

                }
            }
            strsql += " and rate='" + rate + "' ";
            strsql += " and estmid in (select id from tbl_invoiceorderdetailsdeliveryorder where poid='" + poid + "' and status=1 and archive=0) and status=1 and archive=0";


            DataSet dsrecievedquantity = da.GetDataSetInline(strsql);
            if (dsrecievedquantity.Tables[0].Rows.Count > 0)
            {
                lblreceivedvalue = dsrecievedquantity.Tables[0].Rows[0]["totalreceivedquanty"].ToString();
            }
            else
            {
                lblreceivedvalue = "0";
            }



        }
        catch (Exception ex)
        {
            lblreceivedvalue = "0";
        }
        return lblreceivedvalue;

    }

    protected string showcanecelqty(string poid, string itemid, string rate, string prm1, string prm2, string prm3, string prm4, string prm5, string prm6, string prm7, string prm8, string prm9)
    {
        string lblreceivedvalue = "0";
        try
        {
            List<string> prm = new List<string> { prm1, prm2, prm3, prm4, prm5, prm6, prm7, prm8, prm9 };

            string strsql = "select isnull(sum(isnull(quantity,0)),0) as totalreceivedquanty from tbl_poitemsdetailscancel where itemid='" + itemid + "' ";
            for (int k = 0; k < prm.Count; k++)
            {
                if (prm[k] != null)
                {

                    string prmname = "prm" + (k + 1);

                    strsql += " and isnull(" + prmname + ",0)='" + prm[k] + "' ";

                }
            }
            strsql += " and rate='" + rate + "' ";
            strsql += " and estmid='" + poid + "' and status=1 and archive=0 and type='SO'";


            DataSet dsrecievedquantity = da.GetDataSetInline(strsql);
            if (dsrecievedquantity.Tables[0].Rows.Count > 0)
            {
                lblreceivedvalue = dsrecievedquantity.Tables[0].Rows[0]["totalreceivedquanty"].ToString();
            }
            else
            {
                lblreceivedvalue = "0";
            }



        }
        catch (Exception ex)
        {
            lblreceivedvalue = "0";
        }
        return lblreceivedvalue;

    }

    protected void btncancelitemdetails_click(object sender, EventArgs e)
    {

        for (int i = 0; i < Gridview1.Rows.Count; i++)
        {
            HiddenField hdnestmid = (HiddenField)Gridview1.Rows[i].FindControl("hdnestmid");
            HiddenField hdnitemid = (HiddenField)Gridview1.Rows[i].FindControl("hdnitemid");
            HiddenField hdnprm1 = (HiddenField)Gridview1.Rows[i].FindControl("hdnprm1");
            HiddenField hdnprm2 = (HiddenField)Gridview1.Rows[i].FindControl("hdnprm2");
            HiddenField hdnprm3 = (HiddenField)Gridview1.Rows[i].FindControl("hdnprm3");
            HiddenField hdnprm4 = (HiddenField)Gridview1.Rows[i].FindControl("hdnprm4");
            HiddenField hdnprm5 = (HiddenField)Gridview1.Rows[i].FindControl("hdnprm5");
            HiddenField hdnprm6 = (HiddenField)Gridview1.Rows[i].FindControl("hdnprm6");
            HiddenField hdnprm7 = (HiddenField)Gridview1.Rows[i].FindControl("hdnprm7");
            HiddenField hdnprm8 = (HiddenField)Gridview1.Rows[i].FindControl("hdnprm8");
            HiddenField hdnprm9 = (HiddenField)Gridview1.Rows[i].FindControl("hdnprm9");
            HiddenField hdnrate = (HiddenField)Gridview1.Rows[i].FindControl("hdnrate");
            TextBox txtcancelqty = (TextBox)Gridview1.Rows[i].FindControl("txtcancelqty");
            TextBox txtcancelresone = (TextBox)Gridview1.Rows[i].FindControl("txtcancelresone");
            if (txtcancelqty != null)
            {
                try
                {
                    if (Convert.ToDecimal(txtcancelqty.Text) > 0)
                    {

                        //    Response.Write(txtcancelqty.Text);

                        SqlParameter[] prmitem = new SqlParameter[20];
                        prmitem[0] = new SqlParameter("@estmid", hdnestmid.Value);
                        prmitem[1] = new SqlParameter("@itemid", hdnitemid.Value);
                        prmitem[2] = new SqlParameter("@quantity", txtcancelqty.Text);
                        prmitem[3] = new SqlParameter("@rate", hdnrate.Value);
                        prmitem[4] = new SqlParameter("@prm1", hdnprm1.Value);
                        prmitem[5] = new SqlParameter("@prm2", hdnprm2.Value);
                        prmitem[6] = new SqlParameter("@prm3", hdnprm3.Value);
                        prmitem[7] = new SqlParameter("@prm4", hdnprm4.Value);
                        prmitem[8] = new SqlParameter("@prm5", hdnprm5.Value);
                        prmitem[9] = new SqlParameter("@prm6", hdnprm6.Value);
                        prmitem[10] = new SqlParameter("@prm7", hdnprm7.Value);
                        prmitem[11] = new SqlParameter("@prm8", hdnprm8.Value);
                        prmitem[12] = new SqlParameter("@prm9", hdnprm9.Value);
                        prmitem[13] = new SqlParameter("@prm10", "0");
                        prmitem[14] = new SqlParameter("@prm11", "0");
                        prmitem[15] = new SqlParameter("@prm12", "0");
                        prmitem[15] = new SqlParameter("@prm12", "0");
                        prmitem[16] = new SqlParameter("@resone", txtcancelresone.Text);
                        prmitem[17] = new SqlParameter("@uid", Request.Cookies["user"]["user_id"]);
                        prmitem[18] = new SqlParameter("@cdate", da.getcdatetime());
                        prmitem[19] = new SqlParameter("@type", "SO");
                        da.executeDMLQuery("tbl_poitemsdetailscancelinsert", prmitem);




                    }
                }
                catch
                {

                }
            }
        }
        Page.RegisterClientScriptBlock("jsdec", "<script>alert('Sale Order Item Canceled Successfully!!');document.location.href = '/crm/viewsalesorder.aspx?id=" + Request.QueryString["id"] + "';</script>");

    }

    protected void bindresonecancel()
    {
        SqlParameter[] prm = new SqlParameter[3];
        prm[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"]);
        prm[1] = new SqlParameter("@searchtxt", "%");
        prm[2] = new SqlParameter("@type", "Search");
        DataSet ds = da.GetDataSet("USP_tbl_Reasonsmaster", prm);
        cmf.BindDropDownList(ddlreasonscancel, ds, "catname", "id");
        System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("", "0");
        ddlreasonscancel.Items.Insert(0, li);
        System.Web.UI.WebControls.ListItem li1 = new System.Web.UI.WebControls.ListItem("+ Add New", "-1");
        ddlreasonscancel.Items.Add(li1);
    }

    protected void ddlreasonscancel_change(object sender, EventArgs e)
    {
        if (ddlreasonscancel.SelectedValue == "-1")
        {
            txtotherresonecancel.Visible = true;
            txtotherresonecancel.Focus();
            RequiredFieldValidator7.Enabled = true;
        }
        else
        {
            txtotherresonecancel.Visible = false;
            RequiredFieldValidator7.Enabled = false;
        }
    }
    protected string getparmnamevalue(string id)
    {

        string strret = "";
        string strpvalue = "select id,pcode,pvalue from tbl_parametervalues where id = '" + id + "' and status = 1 and archive = 0 order by id";
        DataSet dataSet = da.GetDataSetInline(strpvalue);
        if (dataSet.Tables[0].Rows.Count > 0)
        {
            strret = dataSet.Tables[0].Rows[0]["pvalue"].ToString();
        }
        return strret;
    }

    protected void ddlastatus_change(object sender, EventArgs e)
    {
        if (ddlastatus.SelectedValue == "2")
        {
            pnlreasone.Visible = true;
            bindresone();
        }
        else
        {
            pnlreasone.Visible = false;

        }
    }


    protected void bindresone()
    {
        SqlParameter[] prm = new SqlParameter[3];
        prm[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"]);
        prm[1] = new SqlParameter("@searchtxt", "%");
        prm[2] = new SqlParameter("@type", "Search");
        DataSet ds = da.GetDataSet("USP_tbl_Reasonsmaster", prm);
        cmf.BindDropDownList(ddlreasone, ds, "catname", "id");
        System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("", "0");
        ddlreasone.Items.Insert(0, li);
        System.Web.UI.WebControls.ListItem li1 = new System.Web.UI.WebControls.ListItem("+ Add New", "-1");
        ddlreasone.Items.Add(li1);


    }

    protected void ddlreasone_change(object sender, EventArgs e)
    {
        if (ddlreasone.SelectedValue == "-1")
        {
            txtotherresone.Visible = true;
            txtotherresone.Focus();
            RequiredFieldValidator5.Enabled = true;
        }
        else
        {
            txtotherresone.Visible = false;
            RequiredFieldValidator5.Enabled = false;
        }
    }

    protected void btnsaveapp_click(object sender, EventArgs e)
    {
        if (ddlastatus.SelectedValue == "1")
        {
            approvedata();
        }
        else if (ddlastatus.SelectedValue == "2")
        {
            rejectdata();
        }
    }

    protected void updateentrydataforappoved(string cusid)
    {
        string strsqldd = "select * from tbl_entrydata where  refid='" + cusid + "' and type='2'";
        DataSet dssss = da.GetDataSetInline(strsqldd);
        if (dssss.Tables[0].Rows.Count > 0)
        {
            int entryid = Convert.ToInt32(dssss.Tables[0].Rows[0]["id"].ToString());
            SqlParameter[] prm = new SqlParameter[4];
            prm[0] = new SqlParameter("@entryid", entryid);
            prm[1] = new SqlParameter("@status", "1");
            prm[2] = new SqlParameter("@archive", "0");
            prm[3] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"].ToString());
            da.executeDMLQuery("updateentrydataforapprval", prm);


        }
    }
    protected void approvedata()
    {
        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", cmf.Decrypt(Request.QueryString["id"].ToString()));
        prm[1] = new SqlParameter("@uid", Request.Cookies["user"]["user_id"].ToString());
        prm[2] = new SqlParameter("@cdate", da.getcdatetime());
        prm[3] = new SqlParameter("@isapproveremarks", txtremarks.Text);
        prm[4] = new SqlParameter("@approvestatus", "1");
        prm[5] = new SqlParameter("@type", "2");
        da.executeDMLQuery("tbl_approvaldetailsapprove_reject", prm);
       
        string remarksstr = "Sale Order Approved. by " + fns.getusersname(Request.Cookies["user"]["user_id"].ToString());

        if (txtremarks.Text != string.Empty)
        {
            remarksstr += "<br />" + txtremarks.Text;
        }
        string refidstr = cmf.Decrypt(Request.QueryString["id"].ToString()).ToString();
        string typestr = "2";
        fns.historyinsert(remarksstr, refidstr, typestr);
        string strsql = "select isapprove,uid,approvallevel,capprovallevel from tbl_SalesOrderdetails where id='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "' and status=1 and archive=0";
        DataSet dsa = da.GetDataSetInline(strsql);
        if (dsa.Tables[0].Rows.Count > 0)
        {
            if (dsa.Tables[0].Rows[0]["isapprove"].ToString() == "1")
            {
                try
                {
                    if (Request.QueryString["isview"] != null)
                    {
                        string strcusmofieldupdate = "update tbl_pnotificationdetails set isview='1',isread='1' where rid='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and typeid=2 and isnull(notif_type,0)=5 and isnull(ntfor,0)=9";
                        da.executeDMLQueryinline(strcusmofieldupdate);
                    }
                }
                catch
                {

                }

                try
                {
                    //notifications sendnoyi = new notifications();
                    //sendnoyi.sendnotification(Convert.ToInt32(cmf.Decrypt(Request.QueryString["id"].ToString())), 1, 1);
                    setnotification(cmf.Decrypt(Request.QueryString["id"].ToString()), "2", "1");
                }
                catch
                {

                }
                updateentrydataforappoved(cmf.Decrypt(Request.QueryString["id"].ToString()));
            }
            else
            {
                try
                {
                    if (Request.QueryString["isview"] != null)
                    {
                        string lid = cmf.Decrypt(Request.QueryString["lavelid"].ToString());
                        string strcusmofieldupdate = "update tbl_pnotificationdetails set isview='1',isread='1' where rid='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and isnull(notif_type,0)=5 and isnull(ntfor,0)=9  and typeid=2 and lavelid='" + lid + "' ";
                        da.executeDMLQueryinline(strcusmofieldupdate);

                        string strsqlitem = "select * from tbl_masterapprovalsetup where type='2' and status=1 and archive=0   and aLevel='" + dsa.Tables[0].Rows[0]["capprovallevel"].ToString() + "' ";
                        DataSet dsadd = da.GetDataSetInline(strsqlitem);
                        if (dsadd.Tables[0].Rows.Count > 0 && dsa.Tables[0].Rows[0]["capprovallevel"].ToString() != lid)
                        {
                            string notificationtext = "&nbsp;";
                            string strsqltemp = "select * from tbl_pnotificationdetails where rid='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and isnull(notif_type,0)=5 and isnull(ntfor,0)=9  and typeid=2 and lavelid='" + lid + "' ";
                            DataSet dstemp = da.GetDataSetInline(strsqltemp);
                            if (dstemp.Tables[0].Rows.Count > 0)
                            {
                                notificationtext = dstemp.Tables[0].Rows[0]["notificationtext"].ToString();
                            }
                            char[] cha = { ',' };
                            char[] cha1 = { '~' };
                            for (int i = 0; i < dsadd.Tables[0].Rows.Count; i++)
                            {
                                //SaveLogText("hdnempids:" + dsadd.Tables[0].Rows[i]["hdnempids"].ToString());
                                string[] arr = dsadd.Tables[0].Rows[i]["hdnempids"].ToString().Split(cha, StringSplitOptions.None);

                                for (int j = 0; j < arr.Length; j++)
                                {
                                    if (arr[j].ToString() != string.Empty)
                                    {
                                        string[] arrlist = arr[j].ToString().Split(cha1, StringSplitOptions.None);

                                        if (arrlist.Length > 1)
                                        {
                                            if (arrlist[1].ToString() == "1")
                                            {
                                                string empid = "";
                                                if (arrlist[0].ToString() == "1")
                                                {

                                                    empid = fns.teamleaderid(Request.Cookies["user"]["user_id"].ToString());

                                                    if (i == 0)
                                                    {
                                                        // empmailid = Convert.ToInt32(empid);

                                                        SqlParameter[] prmn = new SqlParameter[14];
                                                        prmn[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                                                        prmn[1] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                                                        prmn[2] = new SqlParameter("@nuid", empid);
                                                        prmn[3] = new SqlParameter("@ndatetime", da.getcdatetime());
                                                        prmn[4] = new SqlParameter("@notificationtext", notificationtext);
                                                        prmn[5] = new SqlParameter("@rid", cmf.Decrypt(Request.QueryString["id"].ToString()));
                                                        prmn[6] = new SqlParameter("@typeid", "2");
                                                        prmn[7] = new SqlParameter("@isnew", "1");
                                                        prmn[8] = new SqlParameter("@isread", "0");
                                                        prmn[9] = new SqlParameter("@isreaddate", DBNull.Value);
                                                        prmn[10] = new SqlParameter("@isview", "0");
                                                        prmn[11] = new SqlParameter("@ntfor", "9"); //9- for approval
                                                        prmn[12] = new SqlParameter("@notif_type", "5");
                                                        prmn[13] = new SqlParameter("@lavelid", dsa.Tables[0].Rows[0]["capprovallevel"].ToString());
                                                        da.executeDMLQuery("tbl_pnotificationdetailsinsert_crm", prmn);
                                                        //SaveLogText("Stp-4=>entry:Done");

                                                    }



                                                }
                                                if (arrlist[0].ToString() == "2")
                                                {

                                                    if (i == 0)
                                                    {
                                                        //empmailid = Convert.ToInt32(empid);
                                                        empid = fns.l2managerid(Request.Cookies["user"]["user_id"].ToString());
                                                        SqlParameter[] prmn = new SqlParameter[14];
                                                        prmn[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                                                        prmn[1] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                                                        prmn[2] = new SqlParameter("@nuid", empid);
                                                        prmn[3] = new SqlParameter("@ndatetime", da.getcdatetime());
                                                        prmn[4] = new SqlParameter("@notificationtext", notificationtext);
                                                        prmn[5] = new SqlParameter("@rid", cmf.Decrypt(Request.QueryString["id"].ToString()));
                                                        prmn[6] = new SqlParameter("@typeid", "2");
                                                        prmn[7] = new SqlParameter("@isnew", "1");
                                                        prmn[8] = new SqlParameter("@isread", "0");
                                                        prmn[9] = new SqlParameter("@isreaddate", DBNull.Value);
                                                        prmn[10] = new SqlParameter("@isview", "0");
                                                        prmn[11] = new SqlParameter("@ntfor", "9"); //9- for approval
                                                        prmn[12] = new SqlParameter("@notif_type", "5");
                                                        prmn[13] = new SqlParameter("@lavelid", dsa.Tables[0].Rows[0]["capprovallevel"].ToString());
                                                        da.executeDMLQuery("tbl_pnotificationdetailsinsert_crm", prmn);
                                                        //SaveLogText("Stp-4=>entry:Done");

                                                    }

                                                }



                                            }
                                            else
                                            {
                                                if (i == 0)
                                                {
                                                    // empmailid = Convert.ToInt32(arrlist[0].ToString());

                                                    SqlParameter[] prmn = new SqlParameter[14];
                                                    prmn[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                                                    prmn[1] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                                                    prmn[2] = new SqlParameter("@nuid", arrlist[0].ToString());
                                                    prmn[3] = new SqlParameter("@ndatetime", da.getcdatetime());
                                                    prmn[4] = new SqlParameter("@notificationtext", notificationtext);
                                                    prmn[5] = new SqlParameter("@rid", cmf.Decrypt(Request.QueryString["id"].ToString()));
                                                    prmn[6] = new SqlParameter("@typeid", "2");
                                                    prmn[7] = new SqlParameter("@isnew", "1");
                                                    prmn[8] = new SqlParameter("@isread", "0");
                                                    prmn[9] = new SqlParameter("@isreaddate", DBNull.Value);
                                                    prmn[10] = new SqlParameter("@isview", "0");
                                                    prmn[11] = new SqlParameter("@ntfor", "9"); //9- for approval
                                                    prmn[12] = new SqlParameter("@notif_type", "5");
                                                    prmn[13] = new SqlParameter("@lavelid", dsa.Tables[0].Rows[0]["capprovallevel"].ToString());
                                                    da.executeDMLQuery("tbl_pnotificationdetailsinsert_crm", prmn);
                                                    //SaveLogText("Stp-5=>entry:Done");

                                                }

                                            }
                                        }

                                    }
                                }
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    //SaveLogText("stp-5 Exception:" + ex.Message);
                }
            }
        }

        // SendRequisitionMail("", hdnleaveid.Value, Request.Cookies["user"]["user_id"].ToString());
        if (Request.QueryString["isview"] != null)
        {
            Page.RegisterClientScriptBlock("js", "<script>alert('Sale Order Approved Successfully!');</script>");
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "txt", "close_bycode()", true);
           // Page.RegisterClientScriptBlock("js", "<script>alert('Sale Order Approved Successfully!');document.location.href='/crm/managesalesorder.aspx';</script>");
        }
        else
        {

            Page.RegisterClientScriptBlock("js", "<script>alert('Sale Order Approved Successfully!');window.parent.document.location.href='approvalsalesorder.aspx';</script>");
            // ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", "alert('Leave Reques Approved Successfully!');", true);
        }

    }
    protected void rejectdata()
    {
        string resoneid = "0";
        resoneid = ddlreasone.SelectedValue;
        if (ddlreasone.SelectedValue == "-1")
        {
            SqlParameter[] prmaa = new SqlParameter[4];
            prmaa[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"]);
            prmaa[1] = new SqlParameter("@catname", txtotherresone.Text);
            prmaa[2] = new SqlParameter("@type", "Insert");
            prmaa[3] = new SqlParameter("ReturnValue", SqlDbType.Int);
            prmaa[3].Direction = ParameterDirection.ReturnValue;
            resoneid = da.executeDMLQuery("USP_tbl_Reasonsmaster", prmaa).ToString();

        }

        SqlParameter[] prm = new SqlParameter[7];
        prm[0] = new SqlParameter("@id", cmf.Decrypt(Request.QueryString["id"].ToString()));
        prm[1] = new SqlParameter("@uid", Request.Cookies["user"]["user_id"].ToString());
        prm[2] = new SqlParameter("@cdate", da.getcdatetime());
        prm[3] = new SqlParameter("@isapproveremarks", txtremarks.Text);
        prm[4] = new SqlParameter("@approvestatus", "2");
        prm[5] = new SqlParameter("@type", "2");
        prm[6] = new SqlParameter("@resoneid", resoneid);
        da.executeDMLQuery("tbl_approvaldetailsapprove_reject", prm);
        try
        {
            if (Request.QueryString["isview"] != null)
            {
                string strcusmofieldupdate = "update tbl_pnotificationdetails set isview='1',isread='1' where rid='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and typeid=2 ";
                da.executeDMLQueryinline(strcusmofieldupdate);
            }
        }
        catch
        {

        }

        string remarksstr = "<b>Sale Order Rejected.</b> by <b>" + fns.getusersname(Request.Cookies["user"]["user_id"].ToString()) + "</b>";

        if (ddlreasone.SelectedValue != "-1")
        {
            remarksstr += "<br /><b> Reasons </b>" + ddlreasone.SelectedItem.Text;

        }
        else
        {
            remarksstr += "<br /> <b>Reasons</b> " + txtotherresone.Text;
        }

        if (txtremarks.Text != string.Empty)
        {
            remarksstr += "<br />" + txtremarks.Text;
        }
        string refidstr = cmf.Decrypt(Request.QueryString["id"].ToString()).ToString();
        string typestr = "2";
        fns.historyinsert(remarksstr, refidstr, typestr);

        if (Request.QueryString["isview"] != null)
        {
            Page.RegisterClientScriptBlock("js", "<script>alert('Sale Order Rejected Successfully!');</script>");
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "txt", "close_bycode()", true);
            //Page.RegisterClientScriptBlock("js", "<script>alert('Sale Order Rejected Successfully!');document.location.href='/crm/managesalesorder.aspx';</script>");
        }
        else
        {
            
            Page.RegisterClientScriptBlock("js", "<script>alert('Sale Order Rejected Successfully!');window.parent.document.location.href='approvalsalesorder.aspx';</script>");
        }
        //ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", "alert('Leave Reques Rejected Successfully!');", true);


    }


    protected void btnapprove_click(object sender, EventArgs e)
    {
        approvedata();
    }
    protected void btnreject_click(object sender, EventArgs e)
    {
        rejectdata();
    }


    protected bool checkvalidation()
    {
        SqlParameter[] prm1 = new SqlParameter[2];
        prm1[0] = new SqlParameter("@saleid", cmf.Decrypt(Request.QueryString["id"]));
        prm1[1] = new SqlParameter("@cdate", da.getcdate());
        DataSet cds = new DataSet();
        cds = da.GetDataSet("selectcomponentforbom", prm1);
        string strsqlitemmain = "select *,tbl_itemsdetails.pname,tbl_itemsdetails.Unitval from tbl_SalesOrderdetailsitemsdetails left outer join tbl_itemsdetails on tbl_SalesOrderdetailsitemsdetails.itemid=tbl_itemsdetails.id where tbl_SalesOrderdetailsitemsdetails.estmid='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "' and tbl_SalesOrderdetailsitemsdetails.status=1 and tbl_SalesOrderdetailsitemsdetails.archive=0  order by tbl_SalesOrderdetailsitemsdetails.id ";
        DataSet dsaddmain = da.GetDataSetInline(strsqlitemmain);





        if (cds.Tables[0].Rows.Count > 0)
        {

            decimal qty = 0;
            decimal Quantity = 0;
            decimal Quantityitemall = 0;
            decimal req_Quantity = 0;
            decimal avil_Quantity = 0;
            decimal stock = 0;
            decimal totalAmt = 0;
            decimal SumtotalAmt = 0;
            for (int i = 0; i < cds.Tables[0].Rows.Count; i++)
            {
                totalAmt = 0;
                req_Quantity = 0;
                Quantityitemall = 0;


                if (dsaddmain.Tables[0].Rows.Count > 0)
                {

                    for (int l = 0; l < dsaddmain.Tables[0].Rows.Count; l++)
                    {
                        if (dsaddmain.Tables[0].Rows[l]["itemid"].ToString() != string.Empty)
                        {
                            try
                            {
                                if (Convert.ToDecimal(dsaddmain.Tables[0].Rows[l]["quantity"].ToString()) > 0)
                                {

                                    string strprocess = "select  tbl_itemscomponets.quantity from tbl_itemscomponets inner join tbl_itemsdetails on tbl_itemscomponets.itemid=tbl_itemsdetails.id  where  estmid='" + (dsaddmain.Tables[0].Rows[l]["itemid"].ToString()) + "' and tbl_itemscomponets.itemid='" + cds.Tables[0].Rows[i]["id"].ToString() + "' and tbl_itemscomponets.status=1 and tbl_itemscomponets.archive=0";
                                    DataSet dsadd = da.GetDataSetInline(strprocess);
                                    if (dsadd.Tables[0].Rows.Count > 0)
                                    {
                                        try
                                        {
                                            req_Quantity = Convert.ToDecimal(dsadd.Tables[0].Rows[0]["quantity"].ToString());
                                            qty = Convert.ToDecimal(dsaddmain.Tables[0].Rows[l]["quantity"].ToString());
                                            Quantity = req_Quantity * qty;
                                            Quantityitemall += Quantity;
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                            }



                        }
                    }
                }


                try
                {
                    stock = Quantityitemall - (Convert.ToDecimal(cds.Tables[0].Rows[i]["closingbalances"]));
                }
                catch (Exception ex)
                {

                }


                try
                {
                    totalAmt = Quantityitemall * (Convert.ToDecimal(cds.Tables[0].Rows[i]["PurchaseRate"].ToString()));
                    SumtotalAmt += totalAmt;
                }
                catch (Exception ex)
                {

                }

                if (stock > 0)
                {
                    return false;


                }



            }

        }
        return true;

    }
    protected void btnConvertedtoProduction_click(object sender, EventArgs e)
    {


        if (checkvalidation())
        {

            string strsqlitemmain = "select *,tbl_itemsdetails.pname,tbl_itemsdetails.Unitval from tbl_SalesOrderdetailsitemsdetails left outer join tbl_itemsdetails on tbl_SalesOrderdetailsitemsdetails.itemid=tbl_itemsdetails.id where tbl_SalesOrderdetailsitemsdetails.estmid='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "' and tbl_SalesOrderdetailsitemsdetails.status=1 and tbl_SalesOrderdetailsitemsdetails.archive=0  order by tbl_SalesOrderdetailsitemsdetails.id ";
            DataSet dsaddmain = da.GetDataSetInline(strsqlitemmain);




            if (dsaddmain.Tables[0].Rows.Count > 0)
            {

                for (int l = 0; l < dsaddmain.Tables[0].Rows.Count; l++)
                {



                    if (dsaddmain.Tables[0].Rows[l]["itemid"].ToString() != string.Empty)
                    {


                        try
                        {
                            if (Convert.ToDecimal(dsaddmain.Tables[0].Rows[l]["quantity"].ToString()) > 0)
                            {
                                SqlParameter[] prmitem = new SqlParameter[17];
                                prmitem[0] = new SqlParameter("@estmid", "0");
                                prmitem[1] = new SqlParameter("@itemid", dsaddmain.Tables[0].Rows[l]["itemid"].ToString());
                                prmitem[2] = new SqlParameter("@quantity", dsaddmain.Tables[0].Rows[l]["quantity"].ToString());
                                prmitem[3] = new SqlParameter("@rate", "0");
                                prmitem[4] = new SqlParameter("@amount", dsaddmain.Tables[0].Rows[l]["quantity"].ToString());
                                prmitem[5] = new SqlParameter("@taxtype", "0");
                                prmitem[6] = new SqlParameter("@taxtvalue", "0");
                                prmitem[7] = new SqlParameter("@taxamount", "0");
                                prmitem[8] = new SqlParameter("@itemdesc", dsaddmain.Tables[0].Rows[l]["itemdesc"].ToString());
                                prmitem[9] = new SqlParameter("@lblunit", dsaddmain.Tables[0].Rows[l]["lblunit"].ToString());
                                prmitem[10] = new SqlParameter("@taxtypeid", dsaddmain.Tables[0].Rows[l]["taxtypeid"].ToString());
                                prmitem[11] = new SqlParameter("@Milestoneid", "0");
                                prmitem[12] = new SqlParameter("ReturnValue", SqlDbType.Int);
                                prmitem[12].Direction = ParameterDirection.ReturnValue;
                                prmitem[13] = new SqlParameter("@cdate", da.getcdatetime());
                                prmitem[14] = new SqlParameter("@productiondate", da.getcdatetime());
                                prmitem[15] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"]);
                                prmitem[16] = new SqlParameter("@uid", Request.Cookies["user"]["user_id"]);
                                int rowid = da.executeDMLQuery("tbl_itemsDailyProductioninsert", prmitem);



                                string strsqlitem = "select * from tbl_itemscomponets where estmid='" + dsaddmain.Tables[0].Rows[l]["itemid"].ToString() + "' and status=1 and archive=0  order by id ";
                                DataSet dsadd = da.GetDataSetInline(strsqlitem);
                                if (dsadd.Tables[0].Rows.Count > 0)
                                {

                                    for (int j = 0; j < dsadd.Tables[0].Rows.Count; j++)
                                    {

                                        decimal totalquantity = 0;
                                        try
                                        {
                                            totalquantity = Math.Round(Convert.ToDecimal(dsaddmain.Tables[0].Rows[l]["quantity"].ToString()) * Convert.ToDecimal(dsadd.Tables[0].Rows[j]["quantity"].ToString()), 2, MidpointRounding.AwayFromZero);
                                        }
                                        catch (Exception ex)
                                        {

                                        }


                                        SqlParameter[] prmaddcomp = new SqlParameter[4];
                                        prmaddcomp[0] = new SqlParameter("@transid", rowid);

                                        prmaddcomp[1] = new SqlParameter("@cdate", da.getcdatetime());
                                        prmaddcomp[2] = new SqlParameter("@itemid", dsadd.Tables[0].Rows[j]["itemid"].ToString());
                                        prmaddcomp[3] = new SqlParameter("@quantity", totalquantity);
                                        da.executeDMLQuery("tbl_DailyProductionitemsrawmatirialdetailsinsert", prmaddcomp);


                                    }

                                }


                            }

                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }


            string strupdate = "update tbl_SalesOrderdetails set  isproduction=1,isproductiondate='" + da.getcdatetime() + "',isproductionuid='" + Request.Cookies["user"]["user_id"].ToString() + "' where id='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "'";
            da.executeDMLQueryinline(strupdate);
            string remarksstr = "Sale Order Converted to Production.by " + fns.getusersname(Request.Cookies["user"]["user_id"].ToString());
            string refidstr = cmf.Decrypt(Request.QueryString["id"]);
            string typestr = "2";
            fns.historyinsert(remarksstr, refidstr, typestr);
            ViewState["count"] = Convert.ToInt32(ViewState["count"]) + 1;
            string vv = "js" + ViewState["count"].ToString();
            Page.RegisterClientScriptBlock(vv, "<script>alert('" + fns.getltext("Sale Order Converted to Production Successfully!!", "2") + "'); document.location.href = '/crm/managesalesorder.aspx';</script>");
        }
        else
        {
            ViewState["count"] = Convert.ToInt32(ViewState["count"]) + 1;
            string vv = "js" + ViewState["count"].ToString();
            Page.RegisterClientScriptBlock(vv, "<script>alert('" + fns.getltext("You Can Not  Converted to Production Due to shortage of raw material.Please Click on View BOM to Request the raw material !!", "2") + "'); document.location.href =  document.location.href;</script>");

        }
    }

    protected void bindpdfonletterhead()
    {







        //   sfdsdf="margin:" + convertinctopx(MarginsTop.Value) + " " + convertinctopx(MarginsRight.Value) + " " + convertinctopx(MarginsBottom.Value) + " " + convertinctopx(MarginsLeft.Value) + " 

        float margintop = float.Parse(convertinctopxfloat(MarginsTop.Value));
        float marginRight = float.Parse(convertinctopxfloat(MarginsRight.Value));
        float marginBottom = float.Parse(convertinctopxfloat(MarginsBottom.Value));
        float marginLeft = float.Parse(convertinctopxfloat(MarginsLeft.Value));


        Document pdfDoc;
        StringWriter sw;
        HtmlTextWriter hw;
        StringReader sr;
        HTMLWorker htmlparser;

        pdfDoc = new Document();






        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        sw = new StringWriter();
        hw = new HtmlTextWriter(sw);
        sr = new StringReader(Convert.ToString(prvdatawithoutheader));
        pdfDoc = new Document(PageSize.A4, marginLeft, marginRight, margintop, marginBottom);

        htmlparser = new HTMLWorker(pdfDoc);

        if (!Directory.Exists(Server.MapPath("/salesPDFonletterhead/" + Request.Cookies["cadmin"]["id"].ToString())))
        {
            Directory.CreateDirectory(Server.MapPath("/salesPDFonletterhead/" + Request.Cookies["cadmin"]["id"].ToString()));
        }

        PdfWriter.GetInstance(pdfDoc, new FileStream(Server.MapPath("/salesPDFonletterhead/" + Request.Cookies["cadmin"]["id"].ToString() + "/" + ViewState["pono"].ToString().Replace(@"/", "_").Replace(@"\", "_").Replace(@"#", "_") + ".pdf"), FileMode.Create));




        pdfDoc.Open();

        htmlparser.Parse(sr);




        pdfDoc.Close();





        strurldata = ViewState["pono"].ToString().Replace(@"#", "_");

        string embed = "<object data=\"/salesPDFonletterhead/" + Request.Cookies["cadmin"]["id"].ToString() + "/" + ViewState["pono"].ToString().Replace(@"/", "_").Replace(@"\", "_").Replace(@"#", "_") + ".pdf\" type=\"application/pdf\" width=\"100%\" height=\"600px\">";
        embed += "If you are unable to view file, you can download from <a href = \"/salesPDFonletterhead/" + Request.Cookies["cadmin"]["id"].ToString() + "/" + ViewState["pono"].ToString().Replace(@"/", "_").Replace(@"\", "_").Replace(@"#", "_") + ".pdf\">here</a>";
        embed += " or download <a target = \"_blank\" href = \"/salesPDFonletterhead/" + Request.Cookies["cadmin"]["id"].ToString() + "/" + ViewState["pono"].ToString().Replace(@"/", "_").Replace(@"\", "_").Replace(@"#", "_") + ".pdf\">Adobe PDF Reader</a> to view the file.";
        embed += "</object>";
        viewpdfonletterhead.Text = embed;

    }
    protected string convertinctopxfloat(string val)
    {
        string retstr = "0px";

        try
        {
            double pic = Convert.ToDouble(val) * 96;
            retstr = Math.Round(pic).ToString();
        }
        catch (Exception ex)
        {
        }

        return retstr;
    }

    protected void btnupload_click(object sender, EventArgs e)
    {
        if (fileuploaderfile.HasFile)
        {

            string filenameurl = fileuploaderfile.FileName;
        vvkfile:
            filenameurl = Request.Cookies["cadmin"]["id"] + filenameurl;
            if (File.Exists(Server.MapPath("/pofileupload/" + filenameurl)))
            {
                goto vvkfile;
            }
            fileuploaderfile.SaveAs(Server.MapPath("/pofileupload/" + filenameurl));

            SqlParameter[] prmitem = new SqlParameter[3];
            prmitem[0] = new SqlParameter("@estmid", cmf.Decrypt(Request.QueryString["id"].ToString()));
            prmitem[1] = new SqlParameter("@fileurl", filenameurl);
            prmitem[2] = new SqlParameter("@cdate", da.getcdatetime());
            da.executeDMLQuery("tbl_SalesOrderdetailsattachedfilesinsert", prmitem);

            //bindpwemision();
            binddata();
            bindprv();

            bindhistory();
            bindpdf();
            bindpdfonletterhead();
        }


    }


    public string viewhistorystring = "";
    private string RemoveAnchorTags(string input)
    {
        // Regular expression to match <a> tags and their contents
        string pattern = @"<a[^>]*>(.*?)<\/a>";

        // Replace the anchor tags with their inner content
        string result = Regex.Replace(input, pattern, "$1");

        return result;
    }
    static string ExtractQueryString(string input)
    {
        // Regular expression to extract the "id" parameter value from the href attribute
        string pattern = @"href='[^']*id=([^&]*)'";

        // Match the pattern in the input string
        Match match = Regex.Match(input, pattern);

        // If a match is found, return the captured group (the "id" value), otherwise return an empty string
        return match.Success ? match.Groups[1].Value : string.Empty;
    }
    protected void bindhistory()
    {

        string strsql = "select * from tbl_historydetails where type='2' and regid='" + Request.Cookies["cadmin"]["id"] + "' and refid='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "' and status=1 and archive=0 order by id desc";

        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {


            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {


                viewhistorystring += "<li>";
                viewhistorystring += " <div class='clearfix'>";
                viewhistorystring += " <div class='date-section pull-left'>";
                viewhistorystring += "  <div class='font-xxs text-draft font-light'> " + cmf.dateConvertdatetime(ds.Tables[0].Rows[i]["cdate"].ToString()) + "&nbsp;&nbsp;</div>";
                viewhistorystring += " </div>";
                viewhistorystring += " <div class='comment-section pull-left'>";
                viewhistorystring += " <div class='media'>";
                viewhistorystring += "<div class='pull-left'>";
                viewhistorystring += " <div class='txn-comment-icon circle-box'></div>";
                viewhistorystring += " </div>";
                viewhistorystring += " <div class='media-body'>";

                //viewhistorystring += "<div class='comment'> <span class=''>" + ds.Tables[0].Rows[i]["remarks"].ToString() + "</span>";
                if (ds.Tables[0].Rows[i]["remarks"].ToString().Contains("Converted to Proforma") || ds.Tables[0].Rows[i]["remarks"].ToString().Contains("View Proforma Invoice"))
                {
                    try
                    {
                        string strid = ExtractQueryString(ds.Tables[0].Rows[i]["remarks"].ToString());
                        DataSet dschkD = da.GetDataSetInline("select * from tbl_invoiceorderdetailsProforma where id='" + cmf.Decrypt(strid) + "' and status=0 and archive=1 ");
                        if (dschkD.Tables[0].Rows.Count > 0)
                        {
                            viewhistorystring += "<div class='comment'> <span class=''>" + RemoveAnchorTags(ds.Tables[0].Rows[i]["remarks"].ToString()).Replace("View Proforma Invoice", "") + "</span>";
                        }
                        else
                        {
                            viewhistorystring += "<div class='comment'> <span class=''>" + ds.Tables[0].Rows[i]["remarks"].ToString() + "</span>";
                        }
                    }
                    catch
                    {
                        viewhistorystring += "<div class='comment'> <span class=''>" + ds.Tables[0].Rows[i]["remarks"].ToString() + "</span>";
                    }
                }
                else if (ds.Tables[0].Rows[i]["remarks"].ToString().Contains("Converted to Invoice") || ds.Tables[0].Rows[i]["remarks"].ToString().Contains("View the Invoice"))
                {
                    try
                    {
                        string strid = ExtractQueryString(ds.Tables[0].Rows[i]["remarks"].ToString());
                        DataSet dschkD = da.GetDataSetInline("select * from tbl_invoiceorderdetails where id='" + cmf.Decrypt(strid) + "' and status=0 and archive=1 ");
                        if (dschkD.Tables[0].Rows.Count > 0)
                        {
                            viewhistorystring += "<div class='comment'> <span class=''>" + RemoveAnchorTags(ds.Tables[0].Rows[i]["remarks"].ToString()).Replace("View the Invoice", "") + "</span>";
                        }
                        else
                        {
                            viewhistorystring += "<div class='comment'> <span class=''>" + ds.Tables[0].Rows[i]["remarks"].ToString() + "</span>";
                        }
                    }
                    catch
                    {
                        viewhistorystring += "<div class='comment'> <span class=''>" + ds.Tables[0].Rows[i]["remarks"].ToString() + "</span>";
                    }
                }
                else
                {
                    viewhistorystring += "<div class='comment'> <span class=''>" + ds.Tables[0].Rows[i]["remarks"].ToString() + "</span>";
                }
                viewhistorystring += "  </div>";
                viewhistorystring += "  </div>";
                viewhistorystring += "  </div>";
                viewhistorystring += " </div>";
                viewhistorystring += " </div>";
                viewhistorystring += " </li>";

            }

        }


    }

    protected void bindpwemision()
    {
        string strsql = "select Manage_Profile.* from Manage_Profile inner join tbl_employee on Manage_Profile.id=cast(tbl_employee.roleid as int) where tbl_employee.id='" + Request.Cookies["user"]["user_id"] + "'  and Manage_Profile.status=1 and Manage_Profile.archive=0";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {

            if (ds.Tables[0].Rows[0]["id"].ToString() == "1")
            {

                module1 = 1;
                module1view = 1;
                module1create = 1;
                module1update = 1;
                module1delete = 1;
                module2Create = 1;
                proformainvoicecreate = 1;
                mainvoicecreate = 1;
            }
            else
            {
                string strsqlmodule = "select * from tbl_rolepermisionsdetails where regid='" + Request.Cookies["cadmin"]["id"].ToString() + "' and profileid='" + ds.Tables[0].Rows[0]["id"].ToString() + "' and moduleid in('7','4','26')";
                DataSet dsmodule = da.GetDataSetInline(strsqlmodule);
                if (dsmodule.Tables[0].Rows.Count > 0)
                {

                    //Response.Write(dsmodule.Tables[0].Rows.Count);

                    for (int i = 0; i < dsmodule.Tables[0].Rows.Count; i++)
                    {
                        string chkfullcontrolCustomersvalue = "0";
                        string CheckBox1Customersvalue = "0";
                        string CheckBox2Customersvalue = "0";
                        string CheckBox3Customersvalue = "0";
                        string CheckBox4Customersvalue = "0";
                        string CheckBox5Customersvalue = "0";


                        chkfullcontrolCustomersvalue = dsmodule.Tables[0].Rows[i]["fullaccesspermision"].ToString();
                        CheckBox1Customersvalue = dsmodule.Tables[0].Rows[i]["viewpermision"].ToString();
                        CheckBox2Customersvalue = dsmodule.Tables[0].Rows[i]["Createpermision"].ToString();
                        CheckBox3Customersvalue = dsmodule.Tables[0].Rows[i]["Editpermision"].ToString();
                        CheckBox4Customersvalue = dsmodule.Tables[0].Rows[i]["Deletepermision"].ToString();
                        CheckBox5Customersvalue = dsmodule.Tables[0].Rows[i]["Assignownerpermision"].ToString();

                        if (dsmodule.Tables[0].Rows[i]["moduleid"].ToString() == "7")
                        {

                            if (chkfullcontrolCustomersvalue == "1")
                            {
                                module1 = 1;
                            }
                            if (CheckBox1Customersvalue == "1")
                            {
                                module1 = 1;
                                module1view = 1;
                            }
                            if (CheckBox2Customersvalue == "1")
                            {
                                module1 = 1;
                                module1create = 1;
                            }
                            if (CheckBox3Customersvalue == "1")
                            {
                                module1 = 1;
                                module1update = 1;
                            }
                            if (CheckBox4Customersvalue == "1")
                            {
                                module1 = 1;
                                module1delete = 1;
                            }

                        }

                        if (dsmodule.Tables[0].Rows[i]["moduleid"].ToString() == "4")
                        {
                            if (CheckBox2Customersvalue == "1")
                            {

                                module2Create = 1;
                                mainvoicecreate = 1;
                            }
                        }
                        if (dsmodule.Tables[0].Rows[i]["moduleid"].ToString() == "26")
                        {
                            if (CheckBox2Customersvalue == "1")
                            {

                                module2Create = 1;
                                proformainvoicecreate = 1;
                            }
                        }



                    }
                }
            }


        }






    }
    protected void btndeletepurchase_click(object sender, EventArgs e)
    {
        string updatesql = "update tbl_SalesOrderdetails set status='0',archive='1',isdeletedate='" + da.getcdatetime() + "',isdeleteuid='" + Request.Cookies["user"]["user_id"] + "',isdeletetxt='" + txtdeleteremarks.Text + "' where id='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "'";
        da.executeDMLQueryinline(updatesql);

        string strsqlS = "select id from tbl_mainleads where isLSaleOrderId='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "' and status=1 and archive=0 and regid='" + Request.Cookies["cadmin"]["id"].ToString() + "'";
        DataSet dsS = da.GetDataSetInline(strsqlS);
        if (dsS.Tables[0].Rows.Count > 0)
        {
            updatesql = "update tbl_mainleads set isLSaleOrderId='0' where id='" + dsS.Tables[0].Rows[0]["id"].ToString() + "'";
            da.executeDMLQueryinline(updatesql);

        }
        string remarksstr = "Sale Order Deleted. by " + fns.getusersname(Request.Cookies["user"]["user_id"].ToString());
        string refidstr = cmf.Decrypt(Request.QueryString["id"]);
        string typestr = "2";
        
        try
        {
            fns.historyinsert(remarksstr, refidstr, typestr);
            //notifications sendnoyi = new notifications();
            //sendnoyi.sendnotificationsalesorder(Convert.ToInt32(cmf.Decrypt(Request.QueryString["id"].ToString())), 2, 3);
            setnotification(cmf.Decrypt(Request.QueryString["id"].ToString()), "2", "3");
        }
        catch
        {

        }
        Page.RegisterClientScriptBlock("jsdec", "<script>alert('Sale Order Deleted Successfully!!');window.parent.document.location.href = '/crm/managesalesorder.aspx';</script>");

    }

    protected void btncancelyes_click(object sender, EventArgs e)
    {
        string resoneid = "0";
        resoneid = ddlreasonscancel.SelectedValue;
        if (ddlreasonscancel.SelectedValue == "-1")
        {
            SqlParameter[] prmaa = new SqlParameter[4];
            prmaa[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"]);
            prmaa[1] = new SqlParameter("@catname", txtotherresonecancel.Text);
            prmaa[2] = new SqlParameter("@type", "Insert");
            prmaa[3] = new SqlParameter("ReturnValue", SqlDbType.Int);
            prmaa[3].Direction = ParameterDirection.ReturnValue;
            resoneid = da.executeDMLQuery("USP_tbl_Reasonsmaster", prmaa).ToString();

        }

        string updatesql = "update tbl_SalesOrderdetails set iscancel='1',iscanceldate='" + da.getcdatetime() + "',iscanceluid='" + Request.Cookies["user"]["user_id"] + "',iscancelremarks='" + txtcancelremarks.Text + "',resoneidcancel='" + resoneid + "'  where id='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "'";
        da.executeDMLQueryinline(updatesql);

        string remarksstr = "Sale Order Canceled. by " + fns.getusersname(Request.Cookies["user"]["user_id"].ToString());
        string refidstr = cmf.Decrypt(Request.QueryString["id"]);
        string typestr = "2";
        if (ddlreasonscancel.SelectedValue != "-1")
        {
            remarksstr += "<br /><b>Reasons </b>" + ddlreasonscancel.SelectedItem.Text;

        }
        else
        {
            remarksstr += "<br /><b>Reasons </b>" + txtotherresonecancel.Text;
        }

        if (txtcancelremarks.Text != string.Empty)
        {
            remarksstr += "<br />" + txtcancelremarks.Text;
        }
        fns.historyinsert(remarksstr, refidstr, typestr);

        try
        {
           // notifications sendnoyi = new notifications();
            //sendnoyi.sendnotificationsalesorder(Convert.ToInt32(cmf.Decrypt(Request.QueryString["id"].ToString())), 2, 4);
            setnotification(cmf.Decrypt(Request.QueryString["id"].ToString()), "2", "4");
        }
        catch
        {

        }

        Page.RegisterClientScriptBlock("jsdec", "<script>alert('Sale Order Cancel Successfully!!');window.parent.document.location.href = '/crm/managesalesorder.aspx';</script>");

    }
    protected void btndeletepurchasecancel_click(object sender, EventArgs e)
    {
        binddata();
        bindprv();
        bindpdf();
        bindpdfonletterhead();
    }
    protected void btncancelNo_click(object sender, EventArgs e)
    {
        //bindpwemision();

        bindhistory();
        binddata();
        bindprv();
        bindpdf();
        bindpdfonletterhead();
    }

    protected void btncancelyes1_click(object sender, EventArgs e)
    {
        string strsql = "update tbl_SalesOrderdetails set islock=1,islockdate='" + da.getcdatetime() + "',islockuid='" + Request.Cookies["user"]["user_id"].ToString() + "' where id='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "'";
        da.executeDMLQueryinline(strsql);
        Page.RegisterClientScriptBlock("jsdec", "<script>alert('Sale Order Lock Successfully!!');window.parent.document.location.href = '/crm/managesalesorder.aspx';</script>");

    }
    protected void btncancelNo1_click(object sender, EventArgs e)
    {
        //bindpwemision();

        bindhistory();
        binddata();
        bindprv();
        bindpdf();
        bindpdfonletterhead();
    }

    protected void btndeliverydatesave_click(object sender, EventArgs e)
    {
        SqlParameter[] prm = new SqlParameter[5];
        prm[0] = new SqlParameter("@poid", cmf.Decrypt(Request.QueryString["id"].ToString()));
        prm[1] = new SqlParameter("@deleverydate", EstimateDate.Text);
        prm[2] = new SqlParameter("@uid", Request.Cookies["user"]["user_id"]);
        prm[3] = new SqlParameter("@cdate", da.getcdatetime());
        prm[4] = new SqlParameter("@notes", txtnotes.Text);
        da.executeDMLQuery("tbl_podelevrydatesinsert", prm);
        binddata();
        bindprv();
        bindpdf();
        bindpdfonletterhead();
        Page.RegisterClientScriptBlock("jsde", "<script>alert('Data Saved Successfully!!');</script>");
    }

    //protected void bindpdf()
    //{


    //    Document pdfDoc;
    //    StringWriter sw;
    //    HtmlTextWriter hw;
    //    StringReader sr;
    //    HTMLWorker htmlparser;

    //    pdfDoc = new Document();


    //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
    //    sw = new StringWriter();
    //    hw = new HtmlTextWriter(sw);
    //    sr = new StringReader(Convert.ToString(prvdata));
    //    pdfDoc = new Document(PageSize.A2, 50f, 50f, 20f, 20f);
    //    htmlparser = new HTMLWorker(pdfDoc);

    //    if (!Directory.Exists(Server.MapPath("/salesPDF/" + Request.Cookies["cadmin"]["id"].ToString())))
    //    {
    //        Directory.CreateDirectory(Server.MapPath("/salesPDF/" + Request.Cookies["cadmin"]["id"].ToString()));
    //    }

    //    PdfWriter.GetInstance(pdfDoc, new FileStream(Server.MapPath("/salesPDF/" + Request.Cookies["cadmin"]["id"].ToString() + "/" + ViewState["pono"].ToString().Replace(@"/", "_").Replace(@"\", "_")  + ".pdf"), FileMode.Create));
    //    pdfDoc.Open();
    //    htmlparser.Parse(sr);
    //    pdfDoc.Close();

    //    strurldata = ViewState["pono"].ToString();

    //    string embed = "<object data=\"/salesPDF/" + Request.Cookies["cadmin"]["id"].ToString() + "/" + ViewState["pono"].ToString().Replace(@"/", "_").Replace(@"\", "_")  + ".pdf\" type=\"application/pdf\" width=\"100%\" height=\"600px\">";
    //    embed += "If you are unable to view file, you can download from <a href = \"/salesPDF/" + Request.Cookies["cadmin"]["id"].ToString() + "/" + ViewState["pono"].ToString().Replace(@"/", "_").Replace(@"\", "_")  + ".pdf\">here</a>";
    //    embed += " or download <a target = \"_blank\" href = \"/salesPDF/" + Request.Cookies["cadmin"]["id"].ToString() + "/" + ViewState["pono"].ToString().Replace(@"/", "_").Replace(@"\", "_")  + ".pdf\">Adobe PDF Reader</a> to view the file.";
    //    embed += "</object>";
    //    viewpdf.Text = embed;

    //}


    protected void bindpdf()
    {




        try
        {





            if (ViewState["headerbackgroundimage"] != string.Empty && ViewState["footersettingBackgroundImage"] != string.Empty)
            {


                Document pdfDoc;
                StringWriter sw;
                HtmlTextWriter hw;
                StringReader sr;
                HTMLWorker htmlparser;

                pdfDoc = new Document();




                var header = iTextSharp.text.Image.GetInstance(Server.MapPath("~/uploaddatafile/" + ViewState["headerbackgroundimage"] + ""));
                float imgheight = header.Height;
                float imgwidth = header.Width;


                header.ScaleAbsolute(595, imgheight);


                //  Response.Write(header.Height);

                float yno = 850 - imgheight;

                header.SetAbsolutePosition(0, yno); // X and Y Accroding to need
                var footer = iTextSharp.text.Image.GetInstance(Server.MapPath("~/uploaddatafile/" + ViewState["footersettingBackgroundImage"] + ""));

                float imgheightfooter = footer.Height;
                float imgwidthfooter = footer.Width;
                footer.SetAbsolutePosition(0, 0); // X and Y Accroding to need
                footer.ScaleAbsolute(595, imgheightfooter);

                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                sw = new StringWriter();
                hw = new HtmlTextWriter(sw);
                sr = new StringReader(Convert.ToString(prvdataforpdf));

                float margintop = float.Parse(convertinctopxfloat(MarginsTop.Value));
                float marginRight = float.Parse(convertinctopxfloat(MarginsRight.Value));
                float marginBottom = float.Parse(convertinctopxfloat(MarginsBottom.Value));
                float marginLeft = float.Parse(convertinctopxfloat(MarginsLeft.Value));
                pdfDoc = new Document(PageSize.A4, marginLeft, marginRight, imgheight, imgheightfooter);


                // pdfDoc = new Document(PageSize.A4, 10f, 10f, imgheight, imgheightfooter);
                //  pdfDoc.AddHeader("vuvek", "hi vivek how r u");
                htmlparser = new HTMLWorker(pdfDoc);

                if (!Directory.Exists(Server.MapPath("/salesPDF/" + Request.Cookies["cadmin"]["id"].ToString())))
                {
                    Directory.CreateDirectory(Server.MapPath("/salesPDF/" + Request.Cookies["cadmin"]["id"].ToString()));
                }

                PdfWriter.GetInstance(pdfDoc, new FileStream(Server.MapPath("/salesPDF/" + Request.Cookies["cadmin"]["id"].ToString() + "/" + ViewState["pono"].ToString().Replace(@"/", "_").Replace(@"\", "_").Replace(@"#", "_") + ".pdf"), FileMode.Create));




                pdfDoc.Open();
                pdfDoc.Add(header); //Adding Header
                pdfDoc.Add(footer);
                htmlparser.Parse(sr);


                int psize = pdfDoc.PageNumber;

                for (int i = 0; i <= pdfDoc.PageNumber; i++)
                {
                    pdfDoc.Add(header); //Adding Header
                    pdfDoc.Add(footer);

                }

                pdfDoc.Close();


            }

            else if (ViewState["headerbackgroundimage"] != string.Empty && ViewState["footersettingBackgroundImage"] == "")
            {


                Document pdfDoc;
                StringWriter sw;
                HtmlTextWriter hw;
                StringReader sr;
                HTMLWorker htmlparser;

                pdfDoc = new Document();
                string strheaderemg = "";
                strheaderemg = "<img src='" + ConfigurationManager.AppSettings["domainname"].ToString() + "/uploaddatafile/" + ViewState["headerbackgroundimage"] + "'></img>";


                strheaderemg += prvdataforpdf;


                var header = iTextSharp.text.Image.GetInstance(Server.MapPath("~/uploaddatafile/" + ViewState["headerbackgroundimage"] + ""));
                float imgheight = header.Height;
                float imgwidth = header.Width;


                header.ScaleAbsolute(595, imgheight);


                //  Response.Write(header.Height);

                float yno = 850 - imgheight;

                header.SetAbsolutePosition(0, yno); // X and Y Accroding to need
                //var footer = iTextSharp.text.Image.GetInstance(Server.MapPath("~/uploaddatafile/" + ViewState["footersettingBackgroundImage"] + ""));

                //float imgheightfooter = footer.Height;
                //float imgwidthfooter = footer.Width;
                //footer.SetAbsolutePosition(0, 0); // X and Y Accroding to need
                //footer.ScaleAbsolute(595, imgheightfooter);

                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                sw = new StringWriter();
                hw = new HtmlTextWriter(sw);
                sr = new StringReader(Convert.ToString(strheaderemg));

                float margintop = float.Parse(convertinctopxfloat(MarginsTop.Value));
                float marginRight = float.Parse(convertinctopxfloat(MarginsRight.Value));
                float marginBottom = float.Parse(convertinctopxfloat(MarginsBottom.Value));
                float marginLeft = float.Parse(convertinctopxfloat(MarginsLeft.Value));

                if (showpageno.Checked)
                {
                    pdfDoc = new Document(PageSize.A4, marginLeft, marginRight, 10f, 35f);
                }
                else
                {
                    pdfDoc = new Document(PageSize.A4, marginLeft, marginRight, 10f, 10f);

                }
                // pdfDoc = new Document(PageSize.A4, marginLeft, marginRight, 10f, 35f);

                // pdfDoc = new Document(PageSize.A4, 10f, 10f, imgheight, 10f);
                //  pdfDoc.AddHeader("vuvek", "hi vivek how r u");
                htmlparser = new HTMLWorker(pdfDoc);

                if (!Directory.Exists(Server.MapPath("/salesPDF/" + Request.Cookies["cadmin"]["id"].ToString())))
                {
                    Directory.CreateDirectory(Server.MapPath("/salesPDF/" + Request.Cookies["cadmin"]["id"].ToString()));
                }

                PdfWriter.GetInstance(pdfDoc, new FileStream(Server.MapPath("/salesPDF/" + Request.Cookies["cadmin"]["id"].ToString() + "/" + ViewState["pono"].ToString().Trim().Replace(@"/", "_").Replace(@"\", "_").Replace(@"#", "_") + ".pdf"), FileMode.Create));




                pdfDoc.Open();
                FontFactory.Register(Server.MapPath("/fonts/ARIALUNI.TTF"));
                StyleSheet style = new StyleSheet();
                style.LoadTagStyle("body", "face", "Arial Unicode MS");
                style.LoadTagStyle("body", "encoding", BaseFont.IDENTITY_H);
                BaseFont bf = BaseFont.CreateFont(Server.MapPath("/fonts/ARIALUNI.TTF"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                foreach (IElement element in HTMLWorker.ParseToList(
                new StringReader(strheaderemg.ToString()), style))
                {
                    pdfDoc.Add(element);
                }
                //htmlparser.Parse(sr);


                int psize = pdfDoc.PageNumber;

                for (int i = 0; i <= pdfDoc.PageNumber; i++)
                {
                    //if (i >0)
                    //{
                    //    pdfDoc.SetMargins(marginLeft, marginRight, 10f, 10f);
                    //pdfDoc.SetMarginMirroring(true);
                    // pdfDoc.TopMargin=10;
                    //pdfDoc = new Document(PageSize.A4, marginLeft, marginRight, imgheight, 10f);
                    //  pdfDoc.Add(header); //Adding Header
                    // pdfDoc.Add(footer);
                    //}

                }

                pdfDoc.Close();


            }

            else if (ViewState["headerbackgroundimage"] == "" && ViewState["footersettingBackgroundImage"] != string.Empty)
            {


                Document pdfDoc;
                StringWriter sw;
                HtmlTextWriter hw;
                StringReader sr;
                HTMLWorker htmlparser;

                pdfDoc = new Document();




                //var header = iTextSharp.text.Image.GetInstance(Server.MapPath("~/uploaddatafile/" + ViewState["headerbackgroundimage"] + ""));
                //float imgheight = header.Height;
                //float imgwidth = header.Width;


                //header.ScaleAbsolute(595, imgheight);


                //  Response.Write(header.Height);

                //float yno = 850 - imgheight;

                //header.SetAbsolutePosition(0, yno); // X and Y Accroding to need
                var footer = iTextSharp.text.Image.GetInstance(Server.MapPath("~/uploaddatafile/" + ViewState["footersettingBackgroundImage"] + ""));

                float imgheightfooter = footer.Height;
                float imgwidthfooter = footer.Width;
                footer.SetAbsolutePosition(0, 0); // X and Y Accroding to need
                footer.ScaleAbsolute(595, imgheightfooter);

                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                sw = new StringWriter();
                hw = new HtmlTextWriter(sw);
                sr = new StringReader(Convert.ToString(prvdataforpdf));


                float margintop = float.Parse(convertinctopxfloat(MarginsTop.Value));
                float marginRight = float.Parse(convertinctopxfloat(MarginsRight.Value));
                float marginBottom = float.Parse(convertinctopxfloat(MarginsBottom.Value));
                float marginLeft = float.Parse(convertinctopxfloat(MarginsLeft.Value));
                pdfDoc = new Document(PageSize.A4, marginLeft, marginRight, 10f, imgheightfooter);

                // pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, imgheightfooter);
                //  pdfDoc.AddHeader("vuvek", "hi vivek how r u");
                htmlparser = new HTMLWorker(pdfDoc);

                if (!Directory.Exists(Server.MapPath("/salesPDF/" + Request.Cookies["cadmin"]["id"].ToString())))
                {
                    Directory.CreateDirectory(Server.MapPath("/salesPDF/" + Request.Cookies["cadmin"]["id"].ToString()));
                }

                PdfWriter.GetInstance(pdfDoc, new FileStream(Server.MapPath("/salesPDF/" + Request.Cookies["cadmin"]["id"].ToString() + "/" + ViewState["pono"].ToString().Replace(@"/", "_").Replace(@"\", "_").Replace(@"#", "_") + ".pdf"), FileMode.Create));




                pdfDoc.Open();
                // pdfDoc.Add(header); //Adding Header
                pdfDoc.Add(footer);
                htmlparser.Parse(sr);


                int psize = pdfDoc.PageNumber;

                for (int i = 0; i <= pdfDoc.PageNumber; i++)
                {
                    // pdfDoc.Add(header); //Adding Header
                    pdfDoc.Add(footer);

                }

                pdfDoc.Close();


            }

            else
            {


                Document pdfDoc;
                StringWriter sw;
                HtmlTextWriter hw;
                StringReader sr;
                HTMLWorker htmlparser;

                pdfDoc = new Document();





                //var header = iTextSharp.text.Image.GetInstance(Server.MapPath("~/uploaddatafile/" + ViewState["headerbackgroundimage"] + ""));
                //float imgheight = header.Height;
                //float imgwidth = header.Width;



                //header.ScaleAbsolute(595, imgheight);


                ////  Response.Write(header.Height);

                //float yno = 850 - imgheight;

                //header.SetAbsolutePosition(0, yno); // X and Y Accroding to need
                //var footer = iTextSharp.text.Image.GetInstance(Server.MapPath("~/uploaddatafile/" + ViewState["headerbackgroundimage"] + ""));
                //footer.SetAbsolutePosition(0, 0); // X and Y Accroding to need
                //footer.ScaleAbsolute(595, 40);

                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                sw = new StringWriter();
                hw = new HtmlTextWriter(sw);
                sr = new StringReader(Convert.ToString(prvdataforpdf));

                float margintop = float.Parse(convertinctopxfloat(MarginsTop.Value));
                float marginRight = float.Parse(convertinctopxfloat(MarginsRight.Value));
                float marginBottom = float.Parse(convertinctopxfloat(MarginsBottom.Value));
                float marginLeft = float.Parse(convertinctopxfloat(MarginsLeft.Value));
                pdfDoc = new Document(PageSize.A4, marginLeft, marginRight, 20f, 40f);

                //   pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 40f);
                //  pdfDoc.AddHeader("vuvek", "hi vivek how r u");
                htmlparser = new HTMLWorker(pdfDoc);

                if (!Directory.Exists(Server.MapPath("/salesPDF/" + Request.Cookies["cadmin"]["id"].ToString())))
                {
                    Directory.CreateDirectory(Server.MapPath("/salesPDF/" + Request.Cookies["cadmin"]["id"].ToString()));
                }

                PdfWriter.GetInstance(pdfDoc, new FileStream(Server.MapPath("/salesPDF/" + Request.Cookies["cadmin"]["id"].ToString() + "/" + ViewState["pono"].ToString().Replace(@"/", "_").Replace(@"\", "_").Replace(@"#", "_") + ".pdf"), FileMode.Create));




                pdfDoc.Open();

                FontFactory.Register(Server.MapPath("/fonts/ARIALUNI.TTF"));
                StyleSheet style = new StyleSheet();
                style.LoadTagStyle("body", "face", "Arial");
                style.LoadTagStyle("body", "encoding", BaseFont.IDENTITY_H);
                BaseFont bf = BaseFont.CreateFont(Server.MapPath("/fonts/ARIALUNI.TTF"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                foreach (IElement element in HTMLWorker.ParseToList(
                new StringReader(prvdataforpdf.ToString()), style))
                {
                    pdfDoc.Add(element);
                }

                // htmlparser.Parse(sr);




                pdfDoc.Close();


            }


            strurldata = ViewState["pono"].ToString().Replace(@"#", "_");

            string embed = "<object data=\"/salesPDF/" + Request.Cookies["cadmin"]["id"].ToString() + "/" + ViewState["pono"].ToString().Replace(@"/", "_").Replace(@"\", "_").Replace(@"#", "_") + ".pdf\" type=\"application/pdf\" width=\"100%\" height=\"600px\">";
            embed += "If you are unable to view file, you can download from <a href = \"/salesPDF/" + Request.Cookies["cadmin"]["id"].ToString() + "/" + ViewState["pono"].ToString().Replace(@"/", "_").Replace(@"\", "_").Replace(@"#", "_") + ".pdf\">here</a>";
            embed += " or download <a target = \"_blank\" href = \"/salesPDF/" + Request.Cookies["cadmin"]["id"].ToString() + "/" + ViewState["pono"].ToString().Replace(@"/", "_").Replace(@"\", "_").Replace(@"#", "_") + ".pdf\">Adobe PDF Reader</a> to view the file.";
            embed += "</object>";
            viewpdf.Text = embed;
        }
        catch (Exception ex)
        {

        }

    }


    //protected string convertinctopxfloat(string val)
    //{
    //    string retstr = "0px";

    //    try
    //    {
    //        double pic = Convert.ToDouble(val) * 96;
    //        retstr = Math.Round(pic).ToString();
    //    }
    //    catch (Exception ex)
    //    {
    //    }

    //    return retstr;
    //}


    protected string convertinctopx(string val)
    {
        string retstr = "0px";

        try
        {
            double pic = Convert.ToDouble(val) * 96;
            retstr = Math.Round(pic).ToString() + "px";
        }
        catch (Exception ex)
        {
        }

        return retstr;
    }
    protected string showcolor(string val)
    {
        string retstr = "#";

        try
        {

            retstr += val.Replace("#", "");
        }
        catch (Exception ex)
        {
        }

        return retstr;
    }

    protected string getlabelcolor()
    {
        string retstr = "#";

        try
        {

            retstr += LabelColor.Value.Replace("#", "");
        }
        catch (Exception ex)
        {
        }

        return retstr;
    }

    protected string getfontcolor()
    {
        string retstr = "#";

        try
        {

            retstr += FontColor.Value.Replace("#", "");
        }
        catch (Exception ex)
        {
        }

        return retstr;
    }

    protected string getcomapnatnamefontcolor()
    {
        string retstr = "#";

        try
        {

            retstr += OrganizationNameColor.Value.Replace("#", "");
        }
        catch (Exception ex)
        {
        }

        return retstr;
    }
    protected void binddata()
    {
        //Response.Write(cmf.Decrypt(Request.QueryString["id"].ToString()));
        //Response.End();
        string strsql = "select * from tbl_SalesOrderdetails where id='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "' and status=1 and archive=0 and regid='" + Request.Cookies["cadmin"]["id"].ToString() + "'";
        //Response.Write(strsql);
        //Response.End();
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            isinvoiced = checkinvoiced(cmf.Decrypt(Request.QueryString["id"].ToString()), Convert.ToDecimal(ds.Tables[0].Rows[0]["totalamount"].ToString()==""?"0":ds.Tables[0].Rows[0]["totalamount"].ToString()));
           
            pnldelete.Visible = false;
            pnlapprove1.Visible = true;
            if (Request.Cookies["cadmin"]["id"].ToString() == "2362")
            {
                string showlog = "";
                string strsqllog = "select id from tbl_Logsdetails where ModuleName='SalesOrder' and Appid='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "' and status=1 and archive=0";
                DataSet dslog = da.GetDataSetInline(strsqllog);
                if (dslog.Tables[0].Rows.Count > 0)
                {
                    showlog = "<div style='float:right;margin:2px;'><a class='btn btn-primary' href='viewloghistory.aspx?id=" + Request.QueryString["id"].ToString() + "&ModuleName=SalesOrder' target='_blank'>view Log History</a></div>";

                }
                lblviewlogs.Text = showlog;
            }

            ViewState["tempid"] = ds.Tables[0].Rows[0]["teamplateid"].ToString();
            ViewState["branchid"] = ds.Tables[0].Rows[0]["branchid"].ToString();
            if (ds.Tables[0].Rows[0]["iscancel"].ToString() == "1")
            {
                iscancel = "1";
            }

            if (ds.Tables[0].Rows[0]["islock"].ToString() == "1")
            {
                islock = "0";
            }
            else
            {
                islock = "1";
            }
        }
        else
        {
            pnlapprove1.Visible = false;
            pnldelete.Visible = true;
        }
        if (Request.QueryString["calltype"] != null)
        {
           
        }
        

        string strssql = "select * from tbl_TemplatePropertiesregistration where id=" + ViewState["tempid"] + " and regid='" + Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0";
        try
        {
            //if(ViewState["tempid"]==null)
            //{
            //    strssql = "select * from tbl_TemplatePropertiesregistration where id=1 and regid='" + Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0";
            //}
            //else if (ViewState["tempid"].ToString() == "")
            //{
            //    strssql = "select * from tbl_TemplatePropertiesregistration where id=1 and regid='" + Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0";
            //}

            ds = da.GetDataSetInline(strssql);


            if (ds.Tables[0].Rows.Count > 0)
            {
                Teamplatename.Value = ds.Tables[0].Rows[0]["Teamplatename"].ToString();
                PaperSize.SelectedValue = ds.Tables[0].Rows[0]["PaperSize"].ToString();
                Orientation.SelectedValue = ds.Tables[0].Rows[0]["Orientation"].ToString();
                MarginsTop.Value = ds.Tables[0].Rows[0]["MarginsTop"].ToString();
                MarginsBottom.Value = ds.Tables[0].Rows[0]["MarginsBottom"].ToString();
                MarginsLeft.Value = ds.Tables[0].Rows[0]["MarginsLeft"].ToString();
                MarginsRight.Value = ds.Tables[0].Rows[0]["MarginsRight"].ToString();
                PDFFont.SelectedValue = ds.Tables[0].Rows[0]["PDFFont"].ToString();
                ViewState["backgroundimage"] = ds.Tables[0].Rows[0]["backgroundimage"].ToString();
                ViewState["backgroundimagesignature"] = ds.Tables[0].Rows[0]["Signaturimage"].ToString();
                if (ViewState["backgroundimage"] != string.Empty)
                {
                    lblBackgroundImage.Text = "<img src='" + ConfigurationManager.AppSettings["domainname"].ToString() + "/uploaddatafile/" + ViewState["backgroundimage"] + "' width='100px' height='50px'></img>";
                    pnlbackground.Visible = true;
                    pnlbackgroundupload.Visible = false;
                }

                imagepotion.SelectedValue = ds.Tables[0].Rows[0]["imagepotion"].ToString();
                BackgroundColor.Value = ds.Tables[0].Rows[0]["BackgroundColor"].ToString();
                if (ds.Tables[0].Rows[0]["isbackgroundcolor"].ToString() == "1")
                {
                    isbackgroundcolor.Checked = true;
                }
                else
                {
                    isbackgroundcolor.Checked = false;
                }

                LabelColor.Value = ds.Tables[0].Rows[0]["LabelColor"].ToString();
                FontColor.Value = ds.Tables[0].Rows[0]["FontColor"].ToString();
                txtFontSize.Value = ds.Tables[0].Rows[0]["txtFontSize"].ToString();
                ViewState["headerbackgroundimage"] = ds.Tables[0].Rows[0]["headerbackgroundimage"].ToString();
                ViewState["footersettingBackgroundImage"] = ds.Tables[0].Rows[0]["footersettingBackgroundImage"].ToString();
                if (ViewState["headerbackgroundimage"] != string.Empty)
                {
                    lblBackgroundImage1.Text = "<img src='" + ConfigurationManager.AppSettings["domainname"].ToString() + "/uploaddatafile/" + ViewState["headerbackgroundimage"] + "' width='100px' height='50px'></img>";
                    pnlbackground1.Visible = true;
                    pnlbackgroundupload1.Visible = false;
                }

                headerbackgroundimagepotion.SelectedValue = ds.Tables[0].Rows[0]["headerbackgroundimagepotion"].ToString();
                if (ds.Tables[0].Rows[0]["isheaderBackgroundColor"].ToString() == "1")
                {
                    isheaderBackgroundColor.Checked = true;
                }
                else
                {
                    isheaderBackgroundColor.Checked = false;
                }


                headerBackgroundColor.Value = ds.Tables[0].Rows[0]["headerBackgroundColor"].ToString();

                if (ds.Tables[0].Rows[0]["isfirstpage"].ToString() == "1")
                {
                    isfirstpage.Checked = true;
                }
                else
                {
                    isfirstpage.Checked = false;
                }

                if (ds.Tables[0].Rows[0]["isdocumenttitle"].ToString() == "1")
                {
                    isdocumenttitle.Checked = true;
                }
                else
                {
                    isdocumenttitle.Checked = false;
                }

                DocumentTitle.Value = ds.Tables[0].Rows[0]["DocumentTitle"].ToString();
                DocumentTitleFontSize.Value = ds.Tables[0].Rows[0]["DocumentTitleFontSize"].ToString();
                DocumentTitleFontColor.Value = ds.Tables[0].Rows[0]["DocumentTitleFontColor"].ToString();

                if (ds.Tables[0].Rows[0]["ShowOrganizationLogo"].ToString() == "1")
                {
                    ShowOrganizationLogo.Checked = true;
                }
                else
                {
                    ShowOrganizationLogo.Checked = false;
                }

                if (ds.Tables[0].Rows[0]["ShowOrganizationName"].ToString() == "1")
                {
                    ShowOrganizationName.Checked = true;
                }
                else
                {
                    ShowOrganizationName.Checked = false;
                }
                if (ds.Tables[0].Rows[0]["ShowOrganizationAddress"].ToString() == "1")
                {
                    ShowOrganizationAddress.Checked = true;
                }
                else
                {
                    ShowOrganizationAddress.Checked = false;
                }

                OrganizationNameColor.Value = ds.Tables[0].Rows[0]["OrganizationNameColor"].ToString();
                OrganizationNamefont.Value = ds.Tables[0].Rows[0]["OrganizationNamefont"].ToString();
                customerfontcolor.Value = ds.Tables[0].Rows[0]["customerfontcolor"].ToString();
                customerfontsize.Value = ds.Tables[0].Rows[0]["customerfontsize"].ToString();

                NumberField.Value = ds.Tables[0].Rows[0]["NumberField"].ToString();


                if (ds.Tables[0].Rows[0]["isDateField"].ToString() == "1")
                {
                    isDateField.Checked = true;
                }
                else
                {
                    isDateField.Checked = false;
                }
                DateField.Value = ds.Tables[0].Rows[0]["DateField"].ToString();




                if (ds.Tables[0].Rows[0]["isExpiryDate"].ToString() == "1")
                {
                    isExpiryDate.Checked = true;
                }
                else
                {
                    isExpiryDate.Checked = false;
                }
                ExpiryDate.Value = ds.Tables[0].Rows[0]["ExpiryDate"].ToString();


                if (ds.Tables[0].Rows[0]["isReferenceField"].ToString() == "1")
                {
                    isReferenceField.Checked = true;
                }
                else
                {
                    isReferenceField.Checked = false;
                }
                ReferenceField.Value = ds.Tables[0].Rows[0]["ReferenceField"].ToString();



                if (ds.Tables[0].Rows[0]["isSalesperson"].ToString() == "1")
                {
                    isSalesperson.Checked = true;
                }
                else
                {
                    isSalesperson.Checked = false;
                }
                Salesperson.Value = ds.Tables[0].Rows[0]["Salesperson"].ToString();

                if (ds.Tables[0].Rows[0]["isBillTo"].ToString() == "1")
                {
                    isBillTo.Checked = true;
                }
                else
                {
                    isBillTo.Checked = false;
                }
                BillTo.Value = ds.Tables[0].Rows[0]["BillTo"].ToString();

                if (ds.Tables[0].Rows[0]["isShipTo"].ToString() == "1")
                {
                    isShipTo.Checked = true;
                }
                else
                {
                    isShipTo.Checked = false;
                }
                ShipTo.Value = ds.Tables[0].Rows[0]["ShipTo"].ToString();


                if (ds.Tables[0].Rows[0]["SerialNumber1"].ToString() == "1")
                {
                    SerialNumber1.Checked = true;
                }
                else
                {
                    SerialNumber1.Checked = false;
                }
                SerialNumber.Value = ds.Tables[0].Rows[0]["SerialNumber"].ToString();



                if (ds.Tables[0].Rows[0]["Item1"].ToString() == "1")
                {
                    Item1.Checked = true;
                }
                else
                {
                    Item1.Checked = false;
                }
                Item.Value = ds.Tables[0].Rows[0]["Item"].ToString();



                if (ds.Tables[0].Rows[0]["Description1"].ToString() == "1")
                {
                    Description1.Checked = true;
                }
                else
                {
                    Description1.Checked = false;
                }
                Description.Value = ds.Tables[0].Rows[0]["Description"].ToString();


                if (ds.Tables[0].Rows[0]["Quantity1"].ToString() == "1")
                {
                    Quantity1.Checked = true;
                }
                else
                {
                    Quantity1.Checked = false;
                }
                Quantity.Value = ds.Tables[0].Rows[0]["Quantity"].ToString();


                if (ds.Tables[0].Rows[0]["Rate1"].ToString() == "1")
                {
                    Rate1.Checked = true;
                }
                else
                {
                    Rate1.Checked = false;
                }
                Rate.Value = ds.Tables[0].Rows[0]["Rate"].ToString();


                if (ds.Tables[0].Rows[0]["Taxpercent1"].ToString() == "1")
                {
                    Taxpercent1.Checked = true;
                }
                else
                {
                    Taxpercent1.Checked = false;
                }
                Taxpercent.Value = ds.Tables[0].Rows[0]["Taxpercent"].ToString();

                if (ds.Tables[0].Rows[0]["TaxAmount1"].ToString() == "1")
                {
                    TaxAmount1.Checked = true;
                }
                else
                {
                    TaxAmount1.Checked = false;
                }
                TaxAmount.Value = ds.Tables[0].Rows[0]["TaxAmount"].ToString();

                if (ds.Tables[0].Rows[0]["Discount1"].ToString() == "1")
                {
                    Discount1.Checked = true;
                }
                else
                {
                    Discount1.Checked = false;
                }
                Discount.Value = ds.Tables[0].Rows[0]["Discount"].ToString();


                if (ds.Tables[0].Rows[0]["Amount1"].ToString() == "1")
                {
                    Amount1.Checked = true;
                }
                else
                {
                    Amount1.Checked = false;
                }
                Amount.Value = ds.Tables[0].Rows[0]["Amount"].ToString();

                if (ds.Tables[0].Rows[0]["Amount1"].ToString() == "1")
                {
                    Amount1.Checked = true;
                }
                else
                {
                    Amount1.Checked = false;
                }
                Amount.Value = ds.Tables[0].Rows[0]["Amount"].ToString();
                if (ds.Tables[0].Rows[0]["TableBorder1"].ToString() == "1")
                {
                    TableBorder1.Checked = true;
                }
                else
                {
                    TableBorder1.Checked = false;
                }
                TableBorder.Value = ds.Tables[0].Rows[0]["TableBorder"].ToString();
                headerFontSize.Value = ds.Tables[0].Rows[0]["headerFontSize"].ToString();

                if (ds.Tables[0].Rows[0]["tableheaderBackgroundColor1"].ToString() == "1")
                {
                    tableheaderBackgroundColor1.Checked = true;
                }
                else
                {
                    tableheaderBackgroundColor1.Checked = false;
                }
                tableheaderBackgroundColor.Value = ds.Tables[0].Rows[0]["tableheaderBackgroundColor"].ToString();
                tableheaderFontColor.Value = ds.Tables[0].Rows[0]["tableheaderFontColor"].ToString();
                tableItemRowfontsize.Value = ds.Tables[0].Rows[0]["tableItemRowfontsize"].ToString();


                if (ds.Tables[0].Rows[0]["tableItemRowBackgroundColor1"].ToString() == "1")
                {
                    tableItemRowBackgroundColor1.Checked = true;
                }
                else
                {
                    tableItemRowBackgroundColor1.Checked = false;
                }
                tableItemRowBackgroundColor.Value = ds.Tables[0].Rows[0]["tableItemRowBackgroundColor"].ToString();
                tableItemRowFontColor.Value = ds.Tables[0].Rows[0]["tableItemRowFontColor"].ToString();
                tableitemdescFontSize.Value = ds.Tables[0].Rows[0]["tableitemdescFontSize"].ToString();
                tableitemdescFontColor.Value = ds.Tables[0].Rows[0]["tableitemdescFontColor"].ToString();

                if (ds.Tables[0].Rows[0]["ShowTotalSection"].ToString() == "1")
                {
                    ShowTotalSection.Checked = true;
                }
                else
                {
                    ShowTotalSection.Checked = false;
                }
                if (ds.Tables[0].Rows[0]["SubTotal1"].ToString() == "1")
                {
                    SubTotal1.Checked = true;
                }
                else
                {
                    SubTotal1.Checked = false;
                }

                SubTotal.Value = ds.Tables[0].Rows[0]["SubTotal"].ToString();


                if (ds.Tables[0].Rows[0]["SubTotal1"].ToString() == "1")
                {
                    SubTotal1.Checked = true;
                }
                else
                {
                    SubTotal1.Checked = false;
                }

                SubTotal.Value = ds.Tables[0].Rows[0]["SubTotal"].ToString();
                ShippingCharges.Value = ds.Tables[0].Rows[0]["ShippingCharges"].ToString();
                tableTotal.Value = ds.Tables[0].Rows[0]["tableTotal"].ToString();
                try
                {
                    CurrencySymbol.SelectedValue = ds.Tables[0].Rows[0]["CurrencySymbol"].ToString();
                }
                catch (Exception EX)
                {

                }


                if (ds.Tables[0].Rows[0]["showitemQuantity"].ToString() == "1")
                {
                    showitemQuantity.Checked = true;
                }
                else
                {
                    showitemQuantity.Checked = false;
                }

                ItemsinTotal.Value = ds.Tables[0].Rows[0]["ItemsinTotal"].ToString();

                if (ds.Tables[0].Rows[0]["Showamountinwords"].ToString() == "1")
                {
                    Showamountinwords.Checked = true;
                }
                else
                {
                    Showamountinwords.Checked = false;
                }



                TotalSubtotaltaxFontSize.Value = ds.Tables[0].Rows[0]["TotalSubtotaltaxFontSize"].ToString();
                TotalSubtotaltaxFontColor.Value = ds.Tables[0].Rows[0]["TotalSubtotaltaxFontColor"].ToString();

                if (ds.Tables[0].Rows[0]["TotalSubtotaltaxBackgroundColor1"].ToString() == "1")
                {
                    TotalSubtotaltaxBackgroundColor1.Checked = true;
                }
                else
                {
                    TotalSubtotaltaxBackgroundColor1.Checked = false;
                }

                TotalSubtotaltaxBackgroundColor.Value = ds.Tables[0].Rows[0]["TotalSubtotaltaxBackgroundColor"].ToString();
                BalanceDueFontSize.Value = ds.Tables[0].Rows[0]["BalanceDueFontSize"].ToString();
                BalanceDueFontColor.Value = ds.Tables[0].Rows[0]["BalanceDueFontColor"].ToString();

                if (ds.Tables[0].Rows[0]["BalanceDueBackgroundColor1"].ToString() == "1")
                {
                    BalanceDueBackgroundColor1.Checked = true;
                }
                else
                {
                    BalanceDueBackgroundColor1.Checked = false;
                }


                BalanceDueBackgroundColor.Value = ds.Tables[0].Rows[0]["BalanceDueBackgroundColor"].ToString();
                FooterNotesLabel.Value = ds.Tables[0].Rows[0]["FooterNotesLabel"].ToString();
                FooterFontSize.Value = ds.Tables[0].Rows[0]["FooterFontSize"].ToString();
                TermsConditionsLabel.Value = ds.Tables[0].Rows[0]["TermsConditionsLabel"].ToString();


                if (ds.Tables[0].Rows[0]["chkistermsandcondition"].ToString() == string.Empty)
                {
                    chkistermsandcondition.Checked = true;
                }
                else
                {
                    chkistermsandcondition.Checked = false;

                }
                if (ds.Tables[0].Rows[0]["chkisnotes"].ToString() == string.Empty)
                {
                    chkisnotes.Checked = true;
                }
                else
                {
                    chkisnotes.Checked = false;

                }


                TermsConditionsFontSize.Value = ds.Tables[0].Rows[0]["TermsConditionsFontSize"].ToString();
                if (ds.Tables[0].Rows[0]["isSignatur"].ToString() == "1")
                {
                    isSignatur.Checked = true;
                }
                else
                {
                    isSignatur.Checked = false;
                }


                isSignatureLabel.Value = ds.Tables[0].Rows[0]["isSignatureLabel"].ToString();
                SignatureName.Value = ds.Tables[0].Rows[0]["SignatureName"].ToString();
                footersettingFontSize.Value = ds.Tables[0].Rows[0]["footersettingFontSize"].ToString();
                footersettingFontColor.Value = ds.Tables[0].Rows[0]["footersettingFontColor"].ToString();

                if (ds.Tables[0].Rows[0]["isfootersettingBackgroundColor"].ToString() == "1")
                {
                    isfootersettingBackgroundColor.Checked = true;
                }
                else
                {
                    isfootersettingBackgroundColor.Checked = false;
                }
                footersettingBackgroundColor.Value = ds.Tables[0].Rows[0]["footersettingFontColor"].ToString();

                footersettingBackgroundImageposition.SelectedValue = ds.Tables[0].Rows[0]["footersettingBackgroundImageposition"].ToString();
                if (ds.Tables[0].Rows[0]["showpageno"].ToString() == "1")
                {
                    showpageno.Checked = true;
                }
                else
                {
                    showpageno.Checked = false;
                }

                if (showbranch == "")
                {
                    try
                    {
                        SqlParameter[] prmb = new SqlParameter[3];
                        prmb[0] = new SqlParameter("@tid", ViewState["tempid"]);
                        prmb[1] = new SqlParameter("@branchid", ViewState["branchid"]);
                        prmb[2] = new SqlParameter("@type", "select");
                        DataSet dsbt = da.GetDataSet("tbl_Tbimagebranchwise_usp", prmb);
                        if (dsbt.Tables[0].Rows.Count > 0)
                        {
                            ViewState["headerbackgroundimage"] = dsbt.Tables[0].Rows[0]["headerbackgroundimage"].ToString();
                            // headerbackgroundimageh2 = dsbt.Tables[0].Rows[0]["headerbackgroundimageh2"].ToString();
                            ViewState["backgroundimagesignature"] = dsbt.Tables[0].Rows[0]["Signaturimage"].ToString();
                            ViewState["footersettingBackgroundImage"] = dsbt.Tables[0].Rows[0]["footersettingBackgroundImage"].ToString();

                        }
                    }
                    catch
                    {

                    }

                }
            }
        }
        catch { }
    }

    protected int checkinvoiced(string invcid,decimal soamt)
    {
     int retval = 0;
        try
        {
            SqlParameter[] prm = new SqlParameter[1];
            prm[0] = new SqlParameter("@soid", invcid);
            ds = da.GetDataSet("sp_SoInvoicedAmount", prm);
            if(ds.Tables[0].Rows.Count>0)
            {
                if(Convert.ToDecimal(ds.Tables[0].Rows[0]["InvoicedAmount"])>=soamt)
                {
                    retval = 1;
                }
            }
         }
        catch
        {
            retval = 0;
        }
     return retval;
    }

    protected void btndeleteattament_click(object sender, EventArgs e)
    {


        string strsqldelete = "update tbl_SalesOrderdetailsattachedfiles set status=0,archive=1 where id='" + hdnattid.Value + "'";
        da.executeDMLQueryinline(strsqldelete);





        string strsqlattached = "select * from tbl_SalesOrderdetailsattachedfiles where estmid='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "' and status=1 and archive=0  order by id ";
        DataSet dsaddattached = da.GetDataSetInline(strsqlattached);
        if (dsaddattached.Tables[0].Rows.Count > 0)
        {

            filecount.Text = dsaddattached.Tables[0].Rows.Count.ToString();
            attfilesdata += "";
            for (int kk = 0; kk < dsaddattached.Tables[0].Rows.Count; kk++)
            {
                attfilesdata += "<div class='list-item'> <div class='row'>" +
                " <div class='col-md-3'> " +
                "<img src='/image/img-icon.png' />" +
              "  </div>" +
                 "<div class='font-small col-md-9'>" +
                 "  <div class='file'> " + dsaddattached.Tables[0].Rows[kk]["fileurl"].ToString() + " <span class='font-xs'> <br>" +
                "  File Size: 11.8 KB </span>" +
                "  </div>    " +
               "<div style='margin-top: 3px;' class='line-seperator font-xs'>" +
                 "  <a class='col' href='/pofileupload/" + dsaddattached.Tables[0].Rows[kk]["fileurl"].ToString() + "' target='_blanck'>Download</a>" +

                 "  <a class='col' href='javascript://' onclick='deleteatt(\"" + dsaddattached.Tables[0].Rows[kk]["id"].ToString() + "\",\"" + (dsaddattached.Tables[0].Rows.Count - 1).ToString() + "\")'>Delete</a> " +
                 " </div> " +
                 "</div> " +
                  " </div> " +
                " </div>";
            }

        }

    }

    //protected void bindprv()
    //{



    //    SqlParameter[] prmpurch = new SqlParameter[2];
    //    prmpurch[0]=new SqlParameter("@id",cmf.Decrypt(Request.QueryString["id"].ToString()));
    //    prmpurch[1] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"].ToString());
    //    DataSet dsstrsqlpurch = da.GetDataSet("tbl_SalesOrderdetailsselectdetails", prmpurch);


    //     //string strsqlpurch = "select * from tbl_purchaseorderdetails where id='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "' and status=1 and archive=0 and regid='" + Request.Cookies["cadmin"]["id"].ToString() + "'";
    //     //DataSet dsstrsqlpurch = da.GetDataSetInline(strsqlpurch);
    //     if (dsstrsqlpurch.Tables[0].Rows.Count > 0)
    //     {


    //         ViewState["pono"] = dsstrsqlpurch.Tables[0].Rows[0]["Estimateno"].ToString();
    //         attfilesdata += "";

    //         string strsqlattached = "select * from tbl_SalesOrderdetailsattachedfiles where estmid='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "' and status=1 and archive=0  order by id ";
    //         DataSet dsaddattached = da.GetDataSetInline(strsqlattached);
    //         if (dsaddattached.Tables[0].Rows.Count > 0)
    //         {
    //             filecount.Text = dsaddattached.Tables[0].Rows.Count.ToString();
    //             for (int kk = 0; kk < dsaddattached.Tables[0].Rows.Count; kk++)
    //             {
    //                 attfilesdata += "<div class='list-item'> <div class='row'>" +
    //                 " <div class='col-md-3'> " +
    //                 "<img src='/image/img-icon.png' />" +
    //               "  </div>" +
    //                  "<div class='font-small col-md-9'>" +
    //                  "  <div class='file'> " + dsaddattached.Tables[0].Rows[kk]["fileurl"].ToString() + " <span class='font-xs'> <br>" +
    //                 "  File Size: 11.8 KB </span>" +
    //                 "  </div>    " +
    //                "<div style='margin-top: 3px;' class='line-seperator font-xs'>" +
    //                  "  <a class='col' href='/pofileupload/" + dsaddattached.Tables[0].Rows[kk]["fileurl"].ToString() + "' target='_blanck'>Download</a>" +

    //                  "  <a class='col' href='javascript://' onclick='deleteatt(\"" + dsaddattached.Tables[0].Rows[kk]["id"].ToString() + "\",\"" + (dsaddattached.Tables[0].Rows.Count - 1).ToString() + "\")'>Delete</a> " +
    //                  " </div> " +
    //                  "</div> " +
    //                   " </div> " +
    //                 " </div>";
    //             }
    //         }




    //         prvdata = "";

    //         string clogo = "";
    //         SqlParameter[] prm = new SqlParameter[1];
    //         prm[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"].ToString());
    //         DataSet ds = da.GetDataSet("tbl_companydetails_selectbyregid", prm);
    //         if (ds.Tables[0].Rows.Count > 0)
    //         {
    //             ViewState["id"] = ds.Tables[0].Rows[0]["id"].ToString();
    //             ViewState["clogo"] = ds.Tables[0].Rows[0]["clogo"].ToString();
    //             ViewState["txtnameofcompany"] = ds.Tables[0].Rows[0]["txtnameofcompany"].ToString();


    //             ViewState["txtaddress1"] = ds.Tables[0].Rows[0]["txtaddress1"].ToString()+" &nbsp;";
    //             ViewState["txtaddress2"] = ds.Tables[0].Rows[0]["txtaddress2"].ToString() + " &nbsp;";
    //             ViewState["txtCountry"] = ds.Tables[0].Rows[0]["txtCountry"].ToString() + " &nbsp;";
    //             ViewState["txtcity"] = ds.Tables[0].Rows[0]["txtcity"].ToString() + " &nbsp;";
    //             ViewState["txtzipcode"] = ds.Tables[0].Rows[0]["txtzipcode"].ToString() + " &nbsp;";
    //             ViewState["txtphoneno"] = ds.Tables[0].Rows[0]["txtphoneno"].ToString() + " &nbsp;";
    //             ViewState["txtfax"] = ds.Tables[0].Rows[0]["txtfax"].ToString() + " &nbsp;";
    //             ViewState["txtwebsite"] = ds.Tables[0].Rows[0]["txtwebsite"].ToString() + " &nbsp;";
    //             ViewState["txtemailcontact"] = ds.Tables[0].Rows[0]["txtemailcontact"].ToString() + " &nbsp;"; 
    //             if (ViewState["clogo"] != string.Empty)
    //             {
    //                 clogo = "<img src='"+ConfigurationManager.AppSettings["domainname"].ToString()+"/clogo/" + ViewState["clogo"] + "' width='150px' />";
    //             }


    //             ViewState["orgaddress"] = fns.getorganitaionaddress(Request.Cookies["cadmin"]["id"].ToString());





    //             //  txtnameofcompany.Text = ds.Tables[0].Rows[0]["txtnameofcompany"].ToString();

    //         }



    //         string DocumentTitlevalue = "";
    //         if (isdocumenttitle.Checked)
    //         {
    //             DocumentTitlevalue = DocumentTitle.Value;
    //         }
    //         string BillTovalue = "";
    //         BillTovalue = BillTo.Value;
    //         string backgroundimg = "";
    //         string backgroundimgpotion = imagepotion.SelectedValue;
    //         if (ViewState["backgroundimage"] != string.Empty)
    //         {
    //             backgroundimg = "/uploaddatafile/" + ViewState["backgroundimage"];
    //         }


    //         string backgroundimg1 = "";
    //         string backgroundimgpotion1 = headerbackgroundimagepotion.SelectedValue;
    //         if (ViewState["headerbackgroundimage"] != string.Empty)
    //         {
    //            backgroundimg1 = "/uploaddatafile/" + ViewState["headerbackgroundimage"];

    //            // prvdata = "<div  style='height:50px'>";
    //            prvdata = "<div  style='background: url(\"" + backgroundimg1 + "\") " + backgroundimgpotion1 + " no-repeat;height:50px'>";
    //             prvdata += "</div>";
    //         }
    //        // prvdata += "<div style='margin:" + convertinctopx(MarginsTop.Value) + " " + convertinctopx(MarginsRight.Value) + " " + convertinctopx(MarginsBottom.Value) + " " + convertinctopx(MarginsLeft.Value) + " ;background-color:" + showcolor(BackgroundColor.Value) + ";color:" + getfontcolor() + ";font-size:" + txtFontSize.Value + "px;border:1px solid #000;'>";
    //        prvdata += "<table cellpadding='0' border='1' valign='top' style='margin:" + convertinctopx(MarginsTop.Value) + " " + convertinctopx(MarginsRight.Value) + " " + convertinctopx(MarginsBottom.Value) + " " + convertinctopx(MarginsLeft.Value) + " ;background-color:" + showcolor(BackgroundColor.Value) + ";color:" + getfontcolor() + ";font-size:" + txtFontSize.Value + "px;border:1px solid #000; background: url(\"" + backgroundimg + "\") " + backgroundimgpotion + " no-repeat;'>";
    //        prvdata += "<tr><td style='padding:0px;'>";
    //         prvdata += "<table cellpadding='5' style='width:100%;table-layout: fixed; margin:0px;' border='0'>";
    //         prvdata += "<tbody>";
    //         prvdata += "<tr>";


    //         if (ShowOrganizationLogo.Checked)
    //         {
    //             prvdata += "<td width='17%' style='vertical-align: middle; border-bottom:1px solid; border-right:0px; width:160px; text-align:left;' border='0'>";

    //             prvdata += "<span>" + clogo + "</span>";
    //             prvdata += "</td>";
    //         }
    //         prvdata += "<td style='vertical-align: top; width:50%; border-bottom:1px solid; border-left:0px; text-align:left; line-height:130%;'>";
    //         if (ShowOrganizationName.Checked)
    //         {
    //             prvdata += "<span style='color:" + getcomapnatnamefontcolor() + ";font-size:" + OrganizationNamefont.Value + "px;'><b>"+ ViewState["txtnameofcompany"]+"</b></span>";
    //         }
    //         if (ShowOrganizationAddress.Checked)
    //         {
    //             prvdata += "<br>"+ ViewState["orgaddress"].ToString()+" </td>";
    //         }
    //         prvdata += "<td style='text-align:right; vertical-align: middle; border-bottom:1px solid;' ><span style='color:" + showcolor(DocumentTitleFontColor.Value) + ";font-size:" + DocumentTitleFontSize.Value + "px;'>" + DocumentTitlevalue + "</span><br>";

    //         prvdata += " </td>";
    //         prvdata += " </tr>";
    //         prvdata += " </tbody>";
    //         prvdata += " </table>";
    //         prvdata += " <table cellpadding='5' style='clear:both;width:100%;margin:0px;'>";
    //         prvdata += " <tbody>";

    //         prvdata += "<tr>";

    //         prvdata += " <td style='vertical-align:bottom;width: 50%; padding-top:0px; border-right:1px solid;' align='left'>";
    //         prvdata += " <table cellpadding='5' style='float:right;width: 100%;line-height: 112%;' cellspacing='0' cellpadding='0' border='0'>";
    //         prvdata += " <tbody>";
    //         prvdata += " <tr>";


    //         prvdata += "  <td style='padding:0px; width:35%;' ><span style='color:" + getlabelcolor() + "'>" + NumberField.Value + "</span></td>";
    //         prvdata += "   <td style='padding:0px;'> : <span>" + dsstrsqlpurch.Tables[0].Rows[0]["Estimateno"].ToString() + "</span></td>";
    //         prvdata += "  </tr>";



    //         if (isDateField.Checked)
    //         {
    //             prvdata += " <tr>";
    //             prvdata += "  <td style='padding:0px;' ><span style='color:" + getlabelcolor() + "'> " + DateField.Value + "</span></td>";
    //             prvdata += "   <td style='padding:0px;' > : <span> " + Convert.ToDateTime(dsstrsqlpurch.Tables[0].Rows[0]["EstimateDate"].ToString()).ToString(fns.getdateformat()) + "</span></td>";
    //             prvdata += "  </tr>";
    //         }
    //         if (isExpiryDate.Checked)
    //         {
    //             prvdata += " <tr>";
    //             prvdata += " <td style='padding:0px;' ><span style='color:" + getlabelcolor() + "'> " + ExpiryDate.Value + "</span></td>";
    //             prvdata += " <td style='padding:0px;' > : <span>" + Convert.ToDateTime(dsstrsqlpurch.Tables[0].Rows[0]["ExpiryDate"].ToString()).ToString(fns.getdateformat()) + "</span></td>";
    //             prvdata += " </tr>";
    //         }

    //         if (isReferenceField.Checked)
    //         {
    //             prvdata += "  <tr>";
    //             prvdata += " <td style='padding:0px;' ><span style='color:" + getlabelcolor() + "'> " + ReferenceField.Value + "</span></td>";
    //             prvdata += "  <td style='padding:0px;'> : <span>" + dsstrsqlpurch.Tables[0].Rows[0]["Reference"].ToString() + "</span></td>";
    //             prvdata += "  </tr>";

    //         }
    //         prvdata += "  </tbody>";
    //         prvdata += "   </table>";
    //         prvdata += "   </td>";
    //         prvdata += "<td style='width:50%;vertical-align:top; padding-top:0px;'>";
    //         prvdata += " <table cellpadding='5' style='float:right;width: 100%; line-height: 112%;' cellspacing='0' cellpadding='0' border='0'>";
    //         prvdata += " <tbody>";
    //         if (isSalesperson.Checked)
    //         {


    //             prvdata += " <tr >";
    //             prvdata += "  <td style='padding:0px;' ><span style='color:" + getlabelcolor() + "'>" + Salesperson.Value + "</span></td>";
    //             prvdata += "   <td style='padding:0px;'> : <span>" + dsstrsqlpurch.Tables[0].Rows[0]["salespersonename"].ToString() + "</span></td>";
    //             prvdata += "  </tr>";
    //         }

    //         prvdata += "  </tbody>";
    //         prvdata += "   </table>";

    //         prvdata += "</td>";


    //         prvdata += " </tr>";
    //         prvdata += "<tr style='background-color: #f2f3f4;' bgcolor='" + showcolor(tableheaderBackgroundColor.Value) + "'>";
    //         prvdata += "<td style='width:50%;vertical-align:top; border-top:1px solid; border-right:1px solid;line-height: 112%;'>";
    //         if (isBillTo.Checked)
    //         {

    //             prvdata += "     <span style='color:" + getlabelcolor() + "'>" + BillTovalue + "</span>";

    //         }


    //         prvdata += "</td>";
    //         prvdata += "<td style='width:50%;vertical-align:top;border-top:1px solid;line-height: 112%;'>";

    //         if (isShipTo.Checked)
    //         {
    //             prvdata += "     <span style='color:" + getlabelcolor() + "'>" + ShipTo.Value + "</span>";


    //         }
    //         prvdata += "</td></tr>";







    //         prvdata += "<tr>";
    //         prvdata += "<td style='width:50%;vertical-align:top; border-top:1px solid; border-right:1px solid;line-height: 112%;'>";

    //         if (isBillTo.Checked)
    //         {



    //             prvdata += " <span style='color:" + showcolor(customerfontcolor.Value) + ";font-size:" + customerfontsize.Value + "px;'> <b>" + dsstrsqlpurch.Tables[0].Rows[0]["txtcontactdisplayname"].ToString() + "</b></span> <br>";

    //             string msgstr = dsstrsqlpurch.Tables[0].Rows[0]["billAttention"].ToString();
    //             if (dsstrsqlpurch.Tables[0].Rows[0]["billAddress1"].ToString() != string.Empty)
    //             {
    //                 if (msgstr == string.Empty)
    //                     msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billAddress1"].ToString();
    //                 else
    //                     msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billAddress1"].ToString();
    //             }
    //             if (dsstrsqlpurch.Tables[0].Rows[0]["billAddress2"].ToString() != string.Empty)
    //             {
    //                 if (msgstr == string.Empty)
    //                     msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billAddress2"].ToString();
    //                 else
    //                     msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billAddress2"].ToString();
    //             }
    //             if (dsstrsqlpurch.Tables[0].Rows[0]["billCity"].ToString() != string.Empty)
    //             {
    //                 if (msgstr == string.Empty)
    //                     msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billCity"].ToString();
    //                 else
    //                     msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billCity"].ToString();
    //             }
    //             if (dsstrsqlpurch.Tables[0].Rows[0]["billState"].ToString() != string.Empty)
    //             {
    //                 if (msgstr == string.Empty)
    //                     msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billState"].ToString();
    //                 else
    //                     msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billState"].ToString();
    //             }
    //             if (dsstrsqlpurch.Tables[0].Rows[0]["billZipCode"].ToString() != string.Empty)
    //             {
    //                 if (msgstr == string.Empty)
    //                     msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billZipCode"].ToString();
    //                 else
    //                     msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billZipCode"].ToString();
    //             }
    //             if (dsstrsqlpurch.Tables[0].Rows[0]["billCountry"].ToString() != string.Empty)
    //             {
    //                 if (msgstr == string.Empty)
    //                     msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billCountry"].ToString();
    //                 else
    //                     msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billCountry"].ToString();
    //             }

    //             if (dsstrsqlpurch.Tables[0].Rows[0]["billPhone"].ToString() != string.Empty)
    //             {
    //                 if (msgstr == string.Empty)
    //                     msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billPhone"].ToString();
    //                 else
    //                     msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billPhone"].ToString();
    //             }



    //             prvdata += msgstr;

    //             prvdata += " </span>";

    //         }
    //         prvdata += "</td>";
    //         prvdata += "<td style='width:50%;vertical-align:top;border-top:1px solid;line-height: 112%;'>";

    //         if (isShipTo.Checked)
    //         {
    //             string msgstr ="";
    //             if (dsstrsqlpurch.Tables[0].Rows[0]["billAddress11"].ToString() != string.Empty)
    //             {
    //                 if (msgstr == string.Empty)
    //                     msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billAddress11"].ToString();
    //                 else
    //                     msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billAddress11"].ToString();
    //             }
    //             if (dsstrsqlpurch.Tables[0].Rows[0]["billAddress21"].ToString() != string.Empty)
    //             {
    //                 if (msgstr == string.Empty)
    //                     msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billAddress21"].ToString();
    //                 else
    //                     msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billAddress21"].ToString();
    //             }
    //             if (dsstrsqlpurch.Tables[0].Rows[0]["billCity1"].ToString() != string.Empty)
    //             {
    //                 if (msgstr == string.Empty)
    //                     msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billCity1"].ToString();
    //                 else
    //                     msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billCity1"].ToString();
    //             }
    //             if (dsstrsqlpurch.Tables[0].Rows[0]["billState1"].ToString() != string.Empty)
    //             {
    //                 if (msgstr == string.Empty)
    //                     msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billState1"].ToString();
    //                 else
    //                     msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billState1"].ToString();
    //             }
    //             if (dsstrsqlpurch.Tables[0].Rows[0]["billZipCode1"].ToString() != string.Empty)
    //             {
    //                 if (msgstr == string.Empty)
    //                     msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billZipCode1"].ToString();
    //                 else
    //                     msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billZipCode1"].ToString();
    //             }
    //             if (dsstrsqlpurch.Tables[0].Rows[0]["billCountry1"].ToString() != string.Empty)
    //             {
    //                 if (msgstr == string.Empty)
    //                     msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billCountry1"].ToString();
    //                 else
    //                     msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billCountry1"].ToString();
    //             }

    //             if (dsstrsqlpurch.Tables[0].Rows[0]["billPhone1"].ToString() != string.Empty)
    //             {
    //                 if (msgstr == string.Empty)
    //                     msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billPhone1"].ToString();
    //                 else
    //                     msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billPhone1"].ToString();
    //             }

    //             prvdata += msgstr;
    //             //prvdata += "   4141 Hacienda Drive <br>";
    //             //prvdata += "  Pleasanton <br>";
    //             //prvdata += "   94588 CA <br>";
    //             prvdata += "</span>";

    //         }
    //         prvdata += "</td>";
    //         prvdata += " </tr>";
    //         prvdata += "  </tbody>";
    //         prvdata += "  </table>";
    //         decimal totalquantity = 0;



    //         string strsqlitem = "select *,tbl_itemsdetails.pname from tbl_SalesOrderdetailsitemsdetails left outer join tbl_itemsdetails on tbl_SalesOrderdetailsitemsdetails.itemid=tbl_itemsdetails.id where tbl_SalesOrderdetailsitemsdetails.estmid='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "' and tbl_SalesOrderdetailsitemsdetails.status=1 and tbl_SalesOrderdetailsitemsdetails.archive=0  order by tbl_SalesOrderdetailsitemsdetails.id ";
    //         DataSet dsadd = da.GetDataSetInline(strsqlitem);
    //         if (dsadd.Tables[0].Rows.Count > 0)
    //         {
    //             ViewState["CurrentTable"] = dsadd.Tables[0];








    //             string tblborderval = "0";
    //             if (TableBorder1.Checked)
    //             {
    //                 tblborderval = "1";
    //             }

    //             prvdata += "   <table cellpadding='5' style='width:100%;margin: 0;border-left:0px;border-right:0px; border-color:" + showcolor(TableBorder.Value) + " !important;' cellspacing='0' border='" + tblborderval + "'>";
    //             prvdata += " <thead>";
    //             if (tableheaderBackgroundColor1.Checked)
    //             {
    //                 prvdata += " <tr bgcolor='" + showcolor(tableheaderBackgroundColor.Value) + "' style='height:32px;border-left:0px;border-right:0px; background:" + showcolor(tableheaderBackgroundColor.Value) + ";font-size:" + headerFontSize.Value + "px;color:" + showcolor(tableheaderFontColor.Value) + ";'>";
    //             }
    //             else
    //             {
    //                 prvdata += " <tr style='height:32px;border-left:0px;border-right:0px; font-size:" + headerFontSize.Value + "px;color:" + showcolor(tableheaderFontColor.Value) + ";'>";
    //             }
    //             if (SerialNumber1.Checked)
    //             {

    //                 prvdata += " <td style='padding: 5px 0px 5px 5px;border-left:0px;width: 5%;text-align: center;'>" + SerialNumber.Value + "</td>";
    //             }

    //             prvdata += "  <td style='padding: 5px 10px 5px 20px;width: 30%;text-align: left;'>";
    //             if (Item1.Checked)
    //             {
    //                 prvdata += Item.Value + " &amp; ";
    //             }
    //             if (Description1.Checked)
    //             {
    //                 prvdata += Description.Value;

    //             }
    //             prvdata += "</td>";
    //             if (Quantity1.Checked)
    //             {
    //                 prvdata += "  <td style='padding: 5px 10px 5px 5px;width: 11%;text-align: right; '>" + Quantity.Value + " </td>";
    //             }
    //             if (Rate1.Checked)
    //             {
    //                 prvdata += "  <td style='padding: 5px 10px 5px 5px;width: 11%;text-align: right; '> " + Rate.Value + " </td>";
    //             }
    //             if (Discount1.Checked)
    //             {
    //                 prvdata += "  <td style='padding: 5px 10px 5px 5px;width: 11%;text-align: right; '> " + Discount.Value + " </td>";
    //             }
    //             if (Taxpercent1.Checked)
    //             {
    //                 prvdata += "   <td style='padding: 5px 10px 5px 5px;width: 11%;text-align: right; '> " + Taxpercent.Value + "</td>";
    //             }

    //             if (TaxAmount1.Checked)
    //             {
    //                 prvdata += "  <td style='padding: 5px 10px 5px 5px;border-right:0px;width: 11%;text-align: right;border-right:0px; '> " + TaxAmount.Value + " </td>";
    //             }
    //             if (Amount1.Checked)
    //             {
    //                 prvdata += "    <td style='padding: 5px 10px 5px 5px;border-right:0px;width: 15%;text-align: right; '> " + Amount.Value + " </td>";
    //             }
    //             prvdata += "   </tr>";
    //             prvdata += " </thead>";
    //             prvdata += "  <tbody>";







    //             for (int jj = 0; jj < dsadd.Tables[0].Rows.Count; jj++)
    //             {


    //                 if (tableItemRowBackgroundColor1.Checked)
    //                 {
    //                     prvdata += " <tr style='border-bottom:1px solid " + showcolor(TableBorder.Value) + "; background:" + showcolor(tableItemRowBackgroundColor.Value) + ";font-size:" + tableItemRowfontsize.Value + "px;color:" + showcolor(tableItemRowFontColor.Value) + ";'>";
    //                 }
    //                 else
    //                 {
    //                     prvdata += " <tr style='border-bottom:1px solid " + showcolor(TableBorder.Value) + ";font-size:" + tableItemRowfontsize.Value + "px;color:" + showcolor(tableItemRowFontColor.Value) + ";'>";
    //                 }



    //                 if (SerialNumber1.Checked)
    //                 {
    //                     prvdata += " <td style='padding: 10px 0 10px 5px;text-align: center;text-align: center;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'> "+(jj+1)+" </td>";
    //                 }
    //                 prvdata += " <td style='padding: 10px 0px 10px 20px;text-align: left;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'>";
    //                 if (Item1.Checked)
    //                 {
    //                     prvdata += "  <span style='font-size:" + tableitemdescFontSize.Value + "px;color:" + showcolor(tableitemdescFontColor.Value) + ";'>"+dsadd.Tables[0].Rows[jj]["pname"].ToString()+"</span>";
    //                 }
    //                 if (Description1.Checked)
    //                 {
    //                     prvdata += "<br><span style='font-size:" + tableitemdescFontSize.Value + "px;color:" + showcolor(tableitemdescFontColor.Value) + ";'>" + dsadd.Tables[0].Rows[jj]["itemdesc"].ToString() + "</span>";
    //                 }
    //                 prvdata += " </td>";
    //                 try{
    //                     totalquantity += Convert.ToDecimal(dsadd.Tables[0].Rows[jj]["quantity"].ToString());
    //                 }
    //                 catch(Exception ex)
    //                 {

    //                 }
    //                 if (Quantity1.Checked)
    //                 {
    //                     prvdata += "<td style='padding: 10px 10px 10px 0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'><span>" + dsadd.Tables[0].Rows[jj]["quantity"].ToString() + "</span></td>";
    //                 }
    //                 if (Rate1.Checked)
    //                 {
    //                     prvdata += "<td style='padding: 10px 10px 10px 0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'><span>" + dsadd.Tables[0].Rows[jj]["rate"].ToString() + "</span></td>";
    //                 }
    //                 if (Discount1.Checked)
    //                 {
    //                     prvdata += "<td style='padding: 10px 10px 10px 0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'>" + dsadd.Tables[0].Rows[jj]["taxtype"].ToString() + " </td>";
    //                 }
    //                 if (Taxpercent1.Checked)
    //                 {
    //                     prvdata += "<td style='padding: 10px 10px 10px 0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'><span>" + dsadd.Tables[0].Rows[jj]["taxtvalue"].ToString() + "%</span></td>";
    //                 }
    //                 if (TaxAmount1.Checked)
    //                 {
    //                     prvdata += "<td style='padding: 10px 10px 10px 0px;border-right:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'><span>" + dsadd.Tables[0].Rows[jj]["taxamount"].ToString() + "</span></td>";
    //                 }
    //                 if (Amount1.Checked)
    //                 {
    //                     prvdata += "<td style='padding: 10px 10px 10px 0px;border-right:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'><span>" + dsadd.Tables[0].Rows[jj]["amount"].ToString() + "</span></td>";
    //                 }
    //                 prvdata += "</tr>";

    //             }
    //         }

    //         prvdata += "</tbody></table>";
    //         prvdata += "   <table style='width:100%; margin:0px;' cellspacing='0' cellpadding='0' border='1'>";

    //         prvdata += "<tr><td aling='top' colspane='2' style='border-color:" + showcolor(TableBorder.Value) + " !important;' >";
    //        prvdata += "   <table style='width:100%; margin:0px;' cellspacing='0' cellpadding='5' border='0'><tr><td>";


    //         if (showitemQuantity.Checked)
    //         {
    //             prvdata += ItemsinTotal.Value + " " + totalquantity.ToString();

    //         }
    //         if (Showamountinwords.Checked)
    //         {

    //             prvdata += "<br/>Total In Words<br/>";
    //             prvdata += "<b>" + fns.getnumbertoworld(dsstrsqlpurch.Tables[0].Rows[0]["totalamount"].ToString()) + "</b>";
    //         }

    //          prvdata += "</td></tr></table>";

    //         prvdata += "<table style='width:100%;margin:15px 0 0 0;' cellspacing='0' cellpadding='5' border='0'>";
    //         prvdata += "  <tbody>";
    //         prvdata += "  <tr>";
    //         prvdata += "    <td style='padding:5px 10px 5px 5px;font-size:11px;' align='left'><span style='color:" + getlabelcolor() + ";font-size:" + TermsConditionsFontSize.Value + "px;'>" + FooterNotesLabel.Value + "</span><br>";
    //         prvdata += "   <span style='font-size:" + FooterFontSize.Value + "px;'>"+dsstrsqlpurch.Tables[0].Rows[0]["CustomerNotes"].ToString()+"</span></td>";
    //         prvdata += " </tr>";

    //         prvdata += "  <tr>";
    //         prvdata += "   <td style='padding:5px 10px 5px 5px;font-size: 10px;' align='left'><span style='color:" + getlabelcolor() + ";font-size:" + TermsConditionsFontSize.Value + "px;'> " + TermsConditionsLabel.Value + "</span><br>";
    //         prvdata += "   <span style='font-size:" + FooterFontSize.Value + "px;'>" + dsstrsqlpurch.Tables[0].Rows[0]["TermsandConditions"].ToString() + "</span></td>";
    //         prvdata += " </tr>";
    //         prvdata += " </tbody>";
    //         prvdata += " </table>";

    //         prvdata += "</td>";

    //         prvdata += "<td style='width:40%;padding:0px 0px 0px 0px; border-left:1px solid; border-color:" + showcolor(TableBorder.Value) + " !important;'>";
    //         if (ShowTotalSection.Checked)
    //         {
    //             prvdata += "<table border='0' width='100%' cellpadding='5' cellspacing='0'  >";
    //             if (SubTotal1.Checked)
    //             {
    //                 prvdata += " <tr>";
    //                 string TotalSubtotaltaxBackgroundColorvalue = "";

    //                 if (TotalSubtotaltaxBackgroundColor1.Checked)
    //                 {
    //                     TotalSubtotaltaxBackgroundColorvalue = showcolor(TotalSubtotaltaxBackgroundColor.Value);

    //                 }

    //                 prvdata += " <td style='padding: 0px 5px 0px 0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;font-size:" + TotalSubtotaltaxFontSize.Value + "px;color:" + showcolor(TotalSubtotaltaxFontColor.Value) + ";background:" + TotalSubtotaltaxBackgroundColorvalue + ";' valign='top' ><span>" + SubTotal.Value + "</span></td>";
    //                 prvdata += " <td style='padding: 0px 5px 0px 0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'><span>" + dsstrsqlpurch.Tables[0].Rows[0]["SubTotal"].ToString() + "</span></td>";
    //                 prvdata += " </tr>";
    //             }
    //             prvdata += " <tr>";
    //             prvdata += " <td style='padding: 0px 5px 0px 0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' ><span>Total Tax </span></td>";
    //             prvdata += " <td style='padding: 0px 5px 0px 0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'><span>" + dsstrsqlpurch.Tables[0].Rows[0]["totalvat"].ToString() + "</span></td>";
    //             prvdata += " </tr>";
    //             prvdata += " <tr>";
    //             prvdata += "  <td style='padding: 0px 5px 0px 0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' ><span>Discount </span></td>";
    //             prvdata += "  <td style='padding: 0px 5px 0px 0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' ><span>" + dsstrsqlpurch.Tables[0].Rows[0]["discountamount"].ToString() + "</span></td>";
    //             prvdata += "  </tr>";
    //             prvdata += " <tr>";
    //             prvdata += "  <td style='padding: 0px 5px 0px 0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' ><span>" + dsstrsqlpurch.Tables[0].Rows[0]["extratfields"].ToString() + " </span></td>";
    //             prvdata += "  <td style='padding: 0px 5px 0px 0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' ><span>" + dsstrsqlpurch.Tables[0].Rows[0]["extrafiledvalue"].ToString() + "</span></td>";
    //             prvdata += "  </tr>";
    //             prvdata += "  <tr>";
    //             prvdata += " <td style='padding: 0px 5px 0px 0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' ><span>" + tableTotal.Value + "</span></td>";

    //             if (CurrencySymbol.SelectedValue == "Before amount")
    //             {
    //                 prvdata += " <td style='padding: 0px 5px 0px 0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' ><span>"+fns.bindcurdefault()+" " + dsstrsqlpurch.Tables[0].Rows[0]["totalamount"].ToString() + "</span></td>";
    //             }
    //             else
    //             {
    //                 prvdata += " <td style='padding: 5px 5px 5px 0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' ><span>" + dsstrsqlpurch.Tables[0].Rows[0]["totalamount"].ToString() + " "+fns.bindcurdefault()+"</span></td>";

    //             }

    //             prvdata += " </tr>";

    //             if (isSignatur.Checked)
    //             {
    //                 prvdata += "<tr  style='border-top:1px solid " + showcolor(TableBorder.Value) + "; text-align:center;'> <td colspan='2' width='100%' border='1' style='vertical-align:bottom; text-align:center; padding:0px;border-bottom:0px solid #000000;' height='100px'  >";
    //                 prvdata += "&nbsp;<br/><br/>" + SignatureName.Value + "&nbsp;<br/>";

    //                 prvdata += isSignatureLabel.Value;
    //                 prvdata += "</td></tr>";
    //             }


    //             prvdata += "</table>";


    //         }


    //         prvdata += "</td>";
    //         prvdata += "</tr></table>";
    //         //prvdata += " </table>";

    //         prvdata += "</td></tr>";

    //         prvdata += " </table>";
    //     }

    //}






    protected void bindprv()
    {






        string istrn = "1";
        string isvat = "1";
        if (Request.Cookies["cadmin"]["id"].ToString() == "2264")
        {
            istrn = "0";
            isvat = "0";
        }

        string strdataheader = "";
        string strdataheader1 = "";

        SqlParameter[] prmpurch = new SqlParameter[2];
        prmpurch[0] = new SqlParameter("@id", cmf.Decrypt(Request.QueryString["id"].ToString()));
        prmpurch[1] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"].ToString());
        DataSet dsstrsqlpurch = da.GetDataSet("tbl_SalesOrderdetailsselectdetails", prmpurch);


        //string strsqlpurch = "select * from tbl_purchaseorderdetails where id='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "' and status=1 and archive=0 and regid='" + Request.Cookies["cadmin"]["id"].ToString() + "'";
        //DataSet dsstrsqlpurch = da.GetDataSetInline(strsqlpurch);
        if (dsstrsqlpurch.Tables[0].Rows.Count > 0)
        {
            if (dsstrsqlpurch.Tables[0].Rows[0]["status"].ToString() == "0" || dsstrsqlpurch.Tables[0].Rows[0]["status"].ToString() == "False")
            {
                pnlhistory.Visible = false;
                string msg = "Sales Order Deleted";
                //Response.Write(msg);
                //Response.End();
                HtmlGenericControl div = new HtmlGenericControl("div");
                div.InnerHtml = "<strong>" + msg + "!</strong>";
                div.Style["color"] = "green";
                div.Style["text-align"] = "center";
                pnldelete.Controls.Add(div);

            }
            else
            {
                pnlhistory.Visible = true;
                #region preview sale
                ViewState["isapproved"] = dsstrsqlpurch.Tables[0].Rows[0]["isapprove"].ToString();
            ViewState["branchid"] = dsstrsqlpurch.Tables[0].Rows[0]["branchid"].ToString();
            ViewState["customermobile"] = fns.getmobilenoofcustomer(dsstrsqlpurch.Tables[0].Rows[0]["cusid"].ToString());
            ViewState["invoiceid"] = cmf.Decrypt(Request.QueryString["id"].ToString());
            //try
            //    {
            //        if (dsstrsqlpurch.Tables[0].Rows[0]["isproduction"].ToString() == "1")
            //        {

            //            pnlconvertproduction.Visible = false;
            //            pnlinvoice.Visible = true;

            //            //Response.Write(dsstrsqlpurch.Tables[0].Rows[0]["isproduction"].ToString());
            //        }
            //        else
            //        {
            //            pnlconvertproduction.Visible = true;
            //            pnlinvoice.Visible = false;
            //        }
            //    }
            //    catch (Exception ex)
            //    {

            //    }
            // Response.Write(dsstrsqlpurch.Tables[0].Rows[0]["isapprove"].ToString());
            //Response.End();
            //Response.Write(ViewState["isappravelreqired"].ToString());
            //Response.End();

            try
            {
                if (ViewState["isappravelreqired"].ToString() == "1")
                {


                    if (dsstrsqlpurch.Tables[0].Rows[0]["isapprove"].ToString() == "1")
                    {
                        pnlinvoice.Visible = true;

                    }
                    else
                    {
                        pnlinvoice.Visible = false;
                    }

                }


            }
            catch (Exception ex)
            {

            }


            ViewState["pono"] = dsstrsqlpurch.Tables[0].Rows[0]["Estimateno"].ToString();
            attfilesdata += "";

            string strsqlattached = "select * from tbl_SalesOrderdetailsattachedfiles where estmid='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "' and status=1 and archive=0  order by id ";
            DataSet dsaddattached = da.GetDataSetInline(strsqlattached);
            if (dsaddattached.Tables[0].Rows.Count > 0)
            {
                filecount.Text = dsaddattached.Tables[0].Rows.Count.ToString();
                for (int kk = 0; kk < dsaddattached.Tables[0].Rows.Count; kk++)
                {
                    attfilesdata += "<div class='list-item'> <div class='row'>" +
                    " <div class='col-md-3'> " +
                    "<img src='/image/img-icon.png' />" +
                  "  </div>" +
                     "<div class='font-small col-md-9'>" +
                     "  <div class='file'> " + dsaddattached.Tables[0].Rows[kk]["fileurl"].ToString() + " <span class='font-xs'> <br>" +
                    "  File Size: 11.8 KB </span>" +
                    "  </div>    " +
                   "<div style='margin-top: 3pt;' class='line-seperator font-xs'>" +
                     "  <a class='col' href='/pofileupload/" + dsaddattached.Tables[0].Rows[kk]["fileurl"].ToString() + "' target='_blanck'>Download</a>" +

                     "  <a class='col' href='javascript://' onclick='deleteatt(\"" + dsaddattached.Tables[0].Rows[kk]["id"].ToString() + "\",\"" + (dsaddattached.Tables[0].Rows.Count - 1).ToString() + "\")'>Delete</a> " +
                     " </div> " +
                     "</div> " +
                      " </div> " +
                    " </div>";
                }
            }



            prvdatawithoutheader = "";
            prvdata = "";

            string clogo = "";
            SqlParameter[] prm = new SqlParameter[1];
            prm[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"].ToString());
            DataSet ds = da.GetDataSet("tbl_companydetails_selectbyregid", prm);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["id"] = ds.Tables[0].Rows[0]["id"].ToString();
                ViewState["clogo"] = ds.Tables[0].Rows[0]["clogo"].ToString();
                ViewState["txtnameofcompany"] = ds.Tables[0].Rows[0]["txtnameofcompany"].ToString();
                ViewState["vatrnno"] = ds.Tables[0].Rows[0]["vatrnno"].ToString();
                try
                {
                    if (showbranch == "")
                    {
                        ViewState["vatrnno"] = fns.getgstnbybranchid(ViewState["branchid"].ToString());

                    }

                }
                catch
                {

                }

                ViewState["txtaddress1"] = ds.Tables[0].Rows[0]["txtaddress1"].ToString() + " &nbsp;";
                ViewState["txtaddress2"] = ds.Tables[0].Rows[0]["txtaddress2"].ToString() + " &nbsp;";
                ViewState["txtCountry"] = ds.Tables[0].Rows[0]["txtCountry"].ToString() + " &nbsp;";
                ViewState["txtcity"] = ds.Tables[0].Rows[0]["txtcity"].ToString() + " &nbsp;";
                ViewState["txtzipcode"] = ds.Tables[0].Rows[0]["txtzipcode"].ToString() + " &nbsp;";
                ViewState["txtphoneno"] = ds.Tables[0].Rows[0]["txtphoneno"].ToString() + " &nbsp;";
                ViewState["txtfax"] = ds.Tables[0].Rows[0]["txtfax"].ToString() + " &nbsp;";
                ViewState["txtwebsite"] = ds.Tables[0].Rows[0]["txtwebsite"].ToString() + " &nbsp;";
                ViewState["txtemailcontact"] = ds.Tables[0].Rows[0]["txtemailcontact"].ToString() + " &nbsp;";
                if (ViewState["clogo"] != string.Empty)
                {
                    if (Request.Cookies["cadmin"]["id"] == "21")
                    {

                        clogo = "<img src='" + ConfigurationManager.AppSettings["domainname"].ToString() + "/clogo/" + ViewState["clogo"] + "' height='100px' />";
                    }
                    else if (Request.Cookies["cadmin"]["id"] == "23")
                    {

                        clogo = "<img src='" + ConfigurationManager.AppSettings["domainname"].ToString() + "/clogo/" + ViewState["clogo"] + "'  />";
                    }
                    else
                    {
                        clogo = "<img src='" + ConfigurationManager.AppSettings["domainname"].ToString() + "/clogo/" + ViewState["clogo"] + "' class='clogo' />";

                    }
                }


                ViewState["orgaddress"] = fns.getorganitaionaddress(Request.Cookies["cadmin"]["id"].ToString());


                try
                {
                    if (showbranch == "")
                    {
                        ViewState["orgaddress"] = fns.getaddressbybranchid(ViewState["branchid"].ToString());

                    }

                }
                catch
                {

                }



                //  txtnameofcompany.Text = ds.Tables[0].Rows[0]["txtnameofcompany"].ToString();

            }



            string DocumentTitlevalue = "";
            if (isdocumenttitle.Checked)
            {
                DocumentTitlevalue = DocumentTitle.Value;
            }
            string BillTovalue = "";
            BillTovalue = BillTo.Value;
            string backgroundimg = "";
            string backgroundimgsignature = "";
            string backgroundimgpotion = imagepotion.SelectedValue;
            if (ViewState["backgroundimage"] != string.Empty)
            {
                backgroundimg = "/uploaddatafile/" + ViewState["backgroundimage"];
            }
            if (ViewState["backgroundimagesignature"] != string.Empty)
            {

                backgroundimgsignature = "/uploaddatafile/" + ViewState["backgroundimagesignature"];
            }

            string backgroundimg1 = "";
            string backgroundimgpotion1 = headerbackgroundimagepotion.SelectedValue;
            //if (ViewState["headerbackgroundimage"] != string.Empty)
            //{
            //   backgroundimg1 = "/uploaddatafile/" + ViewState["headerbackgroundimage"];

            //   // prvdata = "<div  style='height:50px'>";
            //   prvdata = "<div  style='background: url(\"" + backgroundimg1 + "\") " + backgroundimgpotion1 + " no-repeat;height:50px'>";
            //    prvdata += "</div>";
            //}
            // prvdata += "<div style='margin:" + convertinctopx(MarginsTop.Value) + " " + convertinctopx(MarginsRight.Value) + " " + convertinctopx(MarginsBottom.Value) + " " + convertinctopx(MarginsLeft.Value) + " ;background-color:" + showcolor(BackgroundColor.Value) + ";color:" + getfontcolor() + ";font-size:" + txtFontSize.Value + "pt;border:1px solid #000;'>";




            if (ViewState["headerbackgroundimage"] != string.Empty)
            {

                //string headerimage = "<img src='" + ConfigurationManager.AppSettings["domainname"].ToString() + "/uploaddatafile/" + ViewState["headerbackgroundimage"] + "' style='width:100%;'   />";
                //prvdata += "<table style='width:100%;table-layout: fixed; margin:0px;'>";
                //prvdata += "<tbody>";
                //prvdata += "<tr>";
                //prvdata += "<td style='vertical-align: top;  border-bottom:1px solid; border-left:0px; ; line-height:108%;' align='" + backgroundimgpotion1 + "'>";
                ////backgroundimg1 = "/uploaddatafile/" + ViewState["headerbackgroundimage"];
                //prvdata += "<span>" + headerimage + "</span>";
                ////prvdata = "<div  style='background: url(\"" + backgroundimg1 + "\") " + backgroundimgpotion1 + " no-repeat;height:50px'>";
                ////prvdata += "</div>";

                //prvdata += " </td>";
                //prvdata += " </tr>";
                //prvdata += " </tbody>";
                //prvdata += " </table>";
            }
            else
            {
                strdataheader += "<table cellpadding='5' style='width:100%;table-layout: fixed; margin:0pt; border:0;' border='0'>";
                strdataheader += "<tbody>";
                strdataheader += "<tr>";
                if (ShowOrganizationLogo.Checked)
                {
                    strdataheader += "<td   style='vertical-align: middle; border-bottom:1px solid; border-right:0pt;   text-align:left;' border='0'>";

                    strdataheader += "<span>" + clogo + "</span>";
                    strdataheader += "</td>";
                }
                strdataheader += "<td style='vertical-align: top;  border-bottom:1px solid; border-left:0pt; text-align:left; line-height:130%;' colspan='2'>";
                if (ShowOrganizationName.Checked)
                {
                    strdataheader += "<span style='color:" + getcomapnatnamefontcolor() + ";font-size:" + OrganizationNamefont.Value + "pt;'><b>" + ViewState["txtnameofcompany"] + "</b></span><br>";
                }
                if (ShowOrganizationAddress.Checked)
                {
                    strdataheader += "" + ViewState["orgaddress"].ToString() + " </td>";
                }

                strdataheader += "</tr>";
                strdataheader += "<tr>";
                strdataheader += "<td style='vertical-align: middle; border-bottom:0px solid; border-right:0pt; text-align:left;padding:0;'   colspan='3' border='1'></td></tr>";

                strdataheader += "</tr>";
                strdataheader += "</tbody>";
                strdataheader += "</table>";
            }

            strdataheader1 += "<table cellpadding='0' border='1' valign='top' style='background-color:" + showcolor(BackgroundColor.Value) + ";color:" + getfontcolor() + ";font-size:" + txtFontSize.Value + "pt;border:0px solid #000; background: url(\"" + backgroundimg + "\") " + backgroundimgpotion + " no-repeat;'>";
            strdataheader1 += "<tr><td style='padding:0pt;' >";







            prvdata += "<table cellpadding='5' style='clear:both;width:100%;margin:0pt;'>";
            prvdata += "<tbody>";

            prvdata += "<tr>";

            prvdata += "<td style='vertical-align:bottom;width: 50%; padding-top:0pt;' align='left' border='0'>";
            prvdata += "<table cellpadding='5' style='float:right;width: 100%;line-height: 112%;' cellspacing='0' cellpadding='0' border='0'>";
            prvdata += "<tbody>";
            prvdata += "<tr>";


            prvdata += "<td style='padding:0pt; width:35%;' ><b><span style='color:" + getlabelcolor() + "'>" + NumberField.Value + "</span></b></td>";
            prvdata += "<td style='padding:0pt;'> : <span>" + dsstrsqlpurch.Tables[0].Rows[0]["Estimateno"].ToString() + "</span></td>";
            prvdata += "</tr>";



            if (isDateField.Checked)
            {
                prvdata += "<tr>";
                prvdata += "<td style='padding:0pt;' ><b><span style='color:" + getlabelcolor() + "'>" + DateField.Value + "</span></b></td>";
                prvdata += "<td style='padding:0pt;' > : <span> " + Convert.ToDateTime(dsstrsqlpurch.Tables[0].Rows[0]["EstimateDate"].ToString()).ToString(fns.getdateformat()) + "</span></td>";
                prvdata += "</tr>";
            }
            if (isExpiryDate.Checked)
            {
                prvdata += "<tr>";
                prvdata += "<td style='padding:0pt;' ><b><span style='color:" + getlabelcolor() + "'>" + ExpiryDate.Value + "</span></b></td>";
                if (dsstrsqlpurch.Tables[0].Rows[0]["ExpiryDate"].ToString() != string.Empty)
                {
                    prvdata += "<td style='padding:0pt;' > : <span>" + Convert.ToDateTime(dsstrsqlpurch.Tables[0].Rows[0]["ExpiryDate"].ToString()).ToString(fns.getdateformat()) + "</span></td>";
                }
                else
                {
                    prvdata += "<td style='padding:0pt;' > : <span></span></td>";

                }
                prvdata += "</tr>";
            }

            if (isReferenceField.Checked)
            {
                prvdata += "<tr>";
                prvdata += "<td style='padding:0pt;' ><b><span style='color:" + getlabelcolor() + "'>" + ReferenceField.Value + "</span></b></td>";
                prvdata += "<td style='padding:0pt;'> : <span>" + dsstrsqlpurch.Tables[0].Rows[0]["Reference"].ToString() + "</span></td>";
                prvdata += "</tr>";

            }
            if (Request.Cookies["cadmin"]["id"].ToString() == "2322")
            {
                prvdata += "<tr>";
                prvdata += "<td style='padding:0pt;' ><b><span style='color:" + getlabelcolor() + "'>Customer PO No</span></b></td>";
                prvdata += "<td style='padding:0pt;'> : <span>" + dsstrsqlpurch.Tables[0].Rows[0]["apono"].ToString() + "</span></td>";
                prvdata += "</tr>";
                prvdata += "<tr>";
                prvdata += "<td style='padding:0pt;' ><b><span style='color:" + getlabelcolor() + "'>Customer PO date</span></b></td>";
                if (dsstrsqlpurch.Tables[0].Rows[0]["apodate"].ToString() != String.Empty)
                {
                    prvdata += "<td style='padding:0pt;'> : <span>" + Convert.ToDateTime(dsstrsqlpurch.Tables[0].Rows[0]["apodate"].ToString()).ToString(fns.getdateformat()) + "</span></td>";
                }
                else
                {
                    prvdata += "<td style='padding:0pt;'> : <span></span></td>";
                }
                prvdata += "</tr>";


                prvdata += "<tr>";
                prvdata += "<td style='padding:0pt;' ><b><span style='color:" + getlabelcolor() + "'>Payment Terms</span></b></td>";
                prvdata += "<td style='padding:0pt;'> : <span>" + dsstrsqlpurch.Tables[0].Rows[0]["apterms"].ToString() + "</span></td>";
                prvdata += "</tr>";


                prvdata += "<tr>";
                prvdata += "<td style='padding:0pt;' ><b><span style='color:" + getlabelcolor() + "'>Dispatch Through</span></b></td>";
                prvdata += "<td style='padding:0pt;'> : <span>" + dsstrsqlpurch.Tables[0].Rows[0]["adispatchtrough"].ToString() + "</span></td>";
                prvdata += "</tr>";


            }

            prvdata += bindcusdataheader();
            prvdata += "</tbody>";
            prvdata += "</table>";
            prvdata += "</td>";
            prvdata += "<td style='width:50%;vertical-align:top; padding-top:0pt;' border='0'>";
            prvdata += "<table cellpadding='5' style='float:right;width: 100%; line-height: 112%;' cellspacing='0' cellpadding='0' border='0'>";
            prvdata += "<tbody>";
            prvdata += "<tr>";
            prvdata += "<td style='text-align:right; vertical-align: middle; border-bottom:0px solid;' ><b><span style='color:" + showcolor(DocumentTitleFontColor.Value) + ";font-size:" + DocumentTitleFontSize.Value + "pt;'>" + DocumentTitlevalue + "</span></b><br>";

            prvdata += "</td>";
            prvdata += "</tr>";

            if (isSalesperson.Checked)
            {


                prvdata += " <tr>";
                prvdata += "  <td style='padding:0pt;' ><b><span style='color:" + getlabelcolor() + "'>" + Salesperson.Value + "</span></b></td>";
                prvdata += "   <td style='padding:0pt;'> : <span>" + dsstrsqlpurch.Tables[0].Rows[0]["salespersonename"].ToString() + "</span></td>";
                prvdata += "  </tr>";
            }
            prvdata += "  </tbody>";
            prvdata += "   </table>";

            prvdata += "</td>";


            prvdata += " </tr>";
            prvdata += "<tr>";
            prvdata += "<td  colspan='2'  border='1' style='padding: 0;'></td></tr>";



            prvdata += "<tr style='background-color: #f2f3f4;' bgcolor='" + showcolor(tableheaderBackgroundColor.Value) + "'>";
            prvdata += "<td style='width:50%;vertical-align:top; border-top:1px solid; border-right:0px solid;line-height: 112%;'  border='0'>";
            if (isBillTo.Checked)
            {

                prvdata += "<b><span style='color:" + getlabelcolor() + "'>" + BillTovalue + "</span></b>";

            }


            prvdata += "</td>";
            prvdata += "<td style='width:50%;vertical-align:top;border-top:1px solid;line-height: 112%;'  border='0'>";

            if (isShipTo.Checked)
            {
                prvdata += "<b><span style='color:" + getlabelcolor() + "'>" + ShipTo.Value + "</span></b>";


            }
            prvdata += "</td></tr>";







            prvdata += "<tr>";
            prvdata += "<td style='width:50%;vertical-align:top; border-top:1px solid; border-right:0px solid;line-height: 112%;' border='0'>";

            if (isBillTo.Checked)
            {



                prvdata += "<span style='color:" + showcolor(customerfontcolor.Value) + ";font-size:" + customerfontsize.Value + "pt;'><b>" + dsstrsqlpurch.Tables[0].Rows[0]["txtcontactdisplayname"].ToString() + "</b></span><br>";

                string msgstr = dsstrsqlpurch.Tables[0].Rows[0]["billAttention"].ToString();
                if (dsstrsqlpurch.Tables[0].Rows[0]["billAddress1"].ToString() != string.Empty)
                {
                    if (msgstr == string.Empty)
                        msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billAddress1"].ToString();
                    else
                        msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billAddress1"].ToString();
                }
                if (dsstrsqlpurch.Tables[0].Rows[0]["billAddress2"].ToString() != string.Empty)
                {
                    if (msgstr == string.Empty)
                        msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billAddress2"].ToString();
                    else
                        msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billAddress2"].ToString();
                }
                if (dsstrsqlpurch.Tables[0].Rows[0]["billCity"].ToString() != string.Empty)
                {
                    if (msgstr == string.Empty)
                        msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billCity"].ToString();
                    else
                        msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billCity"].ToString();
                }
                if (dsstrsqlpurch.Tables[0].Rows[0]["billState"].ToString() != string.Empty)
                {
                    if (msgstr == string.Empty)
                        msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billState"].ToString();
                    else
                        msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billState"].ToString();
                }
                if (dsstrsqlpurch.Tables[0].Rows[0]["billZipCode"].ToString() != string.Empty)
                {
                    if (msgstr == string.Empty)
                        msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billZipCode"].ToString();
                    else
                        msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billZipCode"].ToString();
                }
                if (dsstrsqlpurch.Tables[0].Rows[0]["billCountry"].ToString() != string.Empty)
                {
                    if (msgstr == string.Empty)
                        msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billCountry"].ToString();
                    else
                        msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billCountry"].ToString();
                }

                if (dsstrsqlpurch.Tables[0].Rows[0]["billPhone"].ToString() != string.Empty)
                {
                    if (msgstr == string.Empty)
                        msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billPhone"].ToString();
                    else
                        msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billPhone"].ToString();
                }

                if (showisindia == "1")
                {
                    if (dsstrsqlpurch.Tables[0].Rows[0]["TaxRegistrationNumber"].ToString() != string.Empty)
                        msgstr += " <br/><b>GSTIN : " + dsstrsqlpurch.Tables[0].Rows[0]["TaxRegistrationNumber"].ToString() + "</b>";

                }
                else
                {
                    if (dsstrsqlpurch.Tables[0].Rows[0]["TaxRegistrationNumber"].ToString() != string.Empty)
                        msgstr += " <br/><b>TRN : " + dsstrsqlpurch.Tables[0].Rows[0]["TaxRegistrationNumber"].ToString() + "</b>";
                }
                if (Request.Cookies["cadmin"]["id"].ToString() == "282")
                {
                    string strsqldata = "select * from tbl_customercustomefieldvalue where uid='" + dsstrsqlpurch.Tables[0].Rows[0]["cusid"].ToString() + "' and controlid='515' and status=1 and archive=0";
                    DataSet dsc = da.GetDataSetInline(strsqldata);
                    if (dsc.Tables[0].Rows.Count > 0)
                    {

                        msgstr += " <br/><b>PAN CARD NO. : " + dsc.Tables[0].Rows[0]["controlvalue"].ToString() + "</b>";


                    }
                }


                prvdata += msgstr;

                prvdata += " </span>";

            }
            prvdata += "</td>";
            prvdata += "<td style='width:50%;vertical-align:top;border-top:1px solid;line-height: 112%;' border='0'>";

            if (isShipTo.Checked)
            {
                string msgstr = "";
                if (dsstrsqlpurch.Tables[0].Rows[0]["billAddress11"].ToString() != string.Empty)
                {
                    if (msgstr == string.Empty)
                        msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billAddress11"].ToString();
                    else
                        msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billAddress11"].ToString();
                }
                if (dsstrsqlpurch.Tables[0].Rows[0]["billAddress21"].ToString() != string.Empty)
                {
                    if (msgstr == string.Empty)
                        msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billAddress21"].ToString();
                    else
                        msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billAddress21"].ToString();
                }
                if (dsstrsqlpurch.Tables[0].Rows[0]["billCity1"].ToString() != string.Empty)
                {
                    if (msgstr == string.Empty)
                        msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billCity1"].ToString();
                    else
                        msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billCity1"].ToString();
                }
                if (dsstrsqlpurch.Tables[0].Rows[0]["billState1"].ToString() != string.Empty)
                {
                    if (msgstr == string.Empty)
                        msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billState1"].ToString();
                    else
                        msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billState1"].ToString();
                }
                if (dsstrsqlpurch.Tables[0].Rows[0]["billZipCode1"].ToString() != string.Empty)
                {
                    if (msgstr == string.Empty)
                        msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billZipCode1"].ToString();
                    else
                        msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billZipCode1"].ToString();
                }
                if (dsstrsqlpurch.Tables[0].Rows[0]["billCountry1"].ToString() != string.Empty)
                {
                    if (msgstr == string.Empty)
                        msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billCountry1"].ToString();
                    else
                        msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billCountry1"].ToString();
                }

                if (dsstrsqlpurch.Tables[0].Rows[0]["billPhone1"].ToString() != string.Empty)
                {
                    if (msgstr == string.Empty)
                        msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billPhone1"].ToString();
                    else
                        msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billPhone1"].ToString();
                }
                if (Request.Cookies["cadmin"]["id"].ToString() == "2182")
                {

                    prvdata += "<span style='color:" + showcolor(customerfontcolor.Value) + ";font-size:" + customerfontsize.Value + "pt;'><b>" + dsstrsqlpurch.Tables[0].Rows[0]["txtcontactdisplayname"].ToString() + "</b></span><br/>";
                }
                prvdata += msgstr;
                //prvdata += "   4141 Hacienda Drive <br>";
                //prvdata += "  Pleasanton <br>";
                //prvdata += "   94588 CA <br>";
                prvdata += "</span>";

            }
            prvdata += "</td>";
            prvdata += " </tr>";
            prvdata += "  </tbody>";
            prvdata += "  </table>";
            decimal totalquantity = 0;


            prvdata += bindcusdatainvoice();
            //string strsqlitem = "select *,tbl_itemsdetails.pname,tbl_itemsdetails.Unitval from tbl_Quotationorderdetailsitemsdetails left outer join tbl_itemsdetails on tbl_Quotationorderdetailsitemsdetails.itemid=tbl_itemsdetails.id where tbl_Quotationorderdetailsitemsdetails.estmid='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "' and tbl_Quotationorderdetailsitemsdetails.status=1 and tbl_Quotationorderdetailsitemsdetails.archive=0  order by tbl_Quotationorderdetailsitemsdetails.id ";
            //DataSet dsadd = da.GetDataSetInline(strsqlitem);

            ViewState["withdiscount"] = dsstrsqlpurch.Tables[0].Rows[0]["withdiscount"].ToString();
            ViewState["enableGrossWeight"] = dsstrsqlpurch.Tables[0].Rows[0]["enableGrossWeight"].ToString();

            int isextracolumn = 0;
            string strsqlPreferences = "select * from tbl_tbl_Preferencessalesinvoices where regid='" + Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0";

            DataSet dsPreferences = da.GetDataSetInline(strsqlPreferences);
            if (dsPreferences.Tables[0].Rows.Count > 0)
            {

                if (Request.Cookies["cadmin"]["id"].ToString() == "157")
                {
                    try
                    {
                        if (dsPreferences.Tables[0].Rows[0]["noofextrafiled4"].ToString() != string.Empty)
                        {
                            isextracolumn = Convert.ToInt32(dsPreferences.Tables[0].Rows[0]["noofextrafiled4"].ToString());

                            // Response.Write(ViewState["extrafield"].ToString());

                        }
                        else
                        {
                            isextracolumn = 0;
                        }


                        string extfield1 = dsPreferences.Tables[0].Rows[0]["extfield25"].ToString();
                        ViewState["extfield1"] = extfield1;
                        string extfield2 = dsPreferences.Tables[0].Rows[0]["extfield26"].ToString();
                        ViewState["extfield2"] = extfield2;
                        string extfield3 = dsPreferences.Tables[0].Rows[0]["extfield27"].ToString();
                        ViewState["extfield3"] = extfield3;
                        string extfield4 = dsPreferences.Tables[0].Rows[0]["extfield28"].ToString();
                        ViewState["extfield4"] = extfield4;
                        string extfield5 = dsPreferences.Tables[0].Rows[0]["extfield29"].ToString();
                        ViewState["extfield5"] = extfield5;
                        string extfield6 = dsPreferences.Tables[0].Rows[0]["extfield29"].ToString();
                        ViewState["extfield6"] = extfield6;

                    }

                    catch (Exception ex)
                    {

                    }
                }
                else
                {



                    if (dsPreferences.Tables[0].Rows[0]["noofextrafiled"].ToString() != string.Empty)
                    {
                        try
                        {
                            isextracolumn = Convert.ToInt32(dsPreferences.Tables[0].Rows[0]["noofextrafiled"].ToString());

                            string extfield1 = dsPreferences.Tables[0].Rows[0]["extfield1"].ToString();
                            ViewState["extfield1"] = extfield1;
                            string extfield2 = dsPreferences.Tables[0].Rows[0]["extfield2"].ToString();
                            ViewState["extfield2"] = extfield2;
                            string extfield3 = dsPreferences.Tables[0].Rows[0]["extfield3"].ToString();
                            ViewState["extfield3"] = extfield3;
                            string extfield4 = dsPreferences.Tables[0].Rows[0]["extfield4"].ToString();
                            ViewState["extfield4"] = extfield4;
                            string extfield5 = dsPreferences.Tables[0].Rows[0]["extfield5"].ToString();
                            ViewState["extfield5"] = extfield5;
                            string extfield6 = dsPreferences.Tables[0].Rows[0]["extfield6"].ToString();
                            ViewState["extfield6"] = extfield6;


                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else
                    {
                        isextracolumn = 0;
                    }
                }

            }




            string strsqlitem = "select *,tbl_itemsdetails.pname,tbl_itemsdetails.txtordercode, tbl_itemsdetails.Unitval from tbl_SalesOrderdetailsitemsdetails left outer join tbl_itemsdetails on tbl_SalesOrderdetailsitemsdetails.itemid=tbl_itemsdetails.id where tbl_SalesOrderdetailsitemsdetails.estmid='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "' and tbl_SalesOrderdetailsitemsdetails.status=1 and tbl_SalesOrderdetailsitemsdetails.archive=0  order by tbl_SalesOrderdetailsitemsdetails.id ";
            DataSet dsadd = da.GetDataSetInline(strsqlitem);

            if (dsadd.Tables[0].Rows.Count > 0)
            {
                ViewState["CurrentTable"] = dsadd.Tables[0];

                Gridview1.DataSource = dsadd;
                Gridview1.DataBind();
                hdncount.Value = dsadd.Tables[0].Rows.Count.ToString();







                string tblborderval = "0";
                if (TableBorder1.Checked)
                {
                    tblborderval = "1";
                }

                prvdata += "   <table cellpadding='5' style='width:100%;margin: 0;border-left:0pt;border-right:0pt; border-color:" + showcolor(TableBorder.Value) + " !important;' cellspacing='0' border='" + tblborderval + "'>";
                prvdata += " <thead>";
                if (tableheaderBackgroundColor1.Checked)
                {
                    prvdata += " <tr bgcolor='" + showcolor(tableheaderBackgroundColor.Value) + "' style='height:32pt;border-left:0pt;border-right:0pt; background:" + showcolor(tableheaderBackgroundColor.Value) + ";font-size:" + headerFontSize.Value + "pt;color:" + showcolor(tableheaderFontColor.Value) + ";'>";
                }
                else
                {
                    prvdata += " <tr style='height:32pt;border-left:0pt;border-right:0pt; font-size:" + headerFontSize.Value + "pt;color:" + showcolor(tableheaderFontColor.Value) + ";'>";
                }
                if (SerialNumber1.Checked)
                {

                    prvdata += " <td style='padding: 5px 0px 5px 5pt;border-left:0pt;width: 5%;text-align: center;'><b>" + SerialNumber.Value + "</b></td>";
                }

                prvdata += "  <td style='padding: 5px 10px 5px 10px;width: 30%;text-align: left;' colspan='6'><b>";



                if (Item1.Checked)
                {
                    prvdata += Item.Value + "";
                }
                if (Description1.Checked)
                {
                    if (Item1.Checked)
                    {
                        if (Item.Value != string.Empty)
                        {
                            if (Description.Value != string.Empty)
                            {
                                prvdata += " &amp; " + Description.Value;
                            }

                        }
                        else
                        {
                            prvdata += Description.Value;
                        }
                    }
                    else
                    {
                        prvdata += Description.Value;
                    }


                }


                prvdata += "</b></td>";

                if (iscoslight == 1)
                {
                    prvdata += "<td style='padding: 5px 10px 5px 5pt;width: 6%;text-align: right;' colspan='2'><b>Part No.</b></td>";
                }

                if (isextracolumn > 0)
                {
                    prvdata += "<td style='padding: 5px 10px 5px 5pt;width: 6%;text-align: right;' colspan='2'><b>" + ViewState["extfield1"] + "</b></td>";


                }
                if (isextracolumn > 1)
                {
                    prvdata += "<td style='padding: 5px 10px 5px 5pt;width: 6%;text-align: right;' colspan='2'><b>" + ViewState["extfield2"] + "</b></td>";


                }
                if (isextracolumn > 2)
                {
                    prvdata += "<td style='padding: 5px 10px 5px 5pt;width: 6%;text-align: right;' colspan='2'><b>" + ViewState["extfield3"] + "</b></td>";


                }
                if (isextracolumn > 3)
                {
                    prvdata += "<td style='padding: 5px 10px 5px 5pt;width: 6%;text-align: right;' colspan='2'><b>" + ViewState["extfield4"] + "</b></td>";


                }
                if (isextracolumn > 4)
                {
                    prvdata += "<td style='padding: 5px 10px 5px 5pt;width: 6%;text-align: right;' colspan='2'><b>" + ViewState["extfield5"] + "</b></td>";


                }
                if (isextracolumn > 5)
                {
                    prvdata += "<td style='padding: 5px 10px 5px 5pt;width: 6%;text-align: right;' colspan='2'><b>" + ViewState["extfield6"] + "</b></td>";


                }


                if (ViewState["enableGrossWeight"].ToString() == "1")
                {
                    prvdata += "<td style='padding: 5px 10px 5px 5pt;width: 6%;text-align: right;' colspan='2'><b>Quantity(Gross)</b></td>";
                    prvdata += "<td style='padding: 5px 10px 5px 5pt;width: 6%;text-align: right;' colspan='2'><b>Quantity(Net)</b></td>";
                    //if (Quantity1.Checked)
                    //{
                    //    prvdata += "<td style='padding: 5px 10px 5px 5pt;width: 6%;text-align: right;' colspan='2'><b>" + Quantity.Value + "</b></td>";
                    //}
                }
                else
                {
                    if (Quantity1.Checked)
                    {
                        prvdata += "<td style='padding: 5px 10px 5px 5pt;width: 6%;text-align: right;' colspan='2'><b>" + Quantity.Value + "</b></td>";
                    }
                }



                if (Rate1.Checked)
                {
                    if (showrental == "1")
                    {
                        prvdata += "<td style='padding: 5px 10px 5px 5pt;width: 6%;text-align: right;'  colspan='2'><b>Duration</b></td>";
                        prvdata += "<td style='padding: 5px 10px 5px 5pt;width: 6%;text-align: right;'  colspan='2'><b>" + ratetext + "</b></td>";
                    }
                    else
                    {
                        prvdata += "<td style='padding: 5px 10px 5px 5pt;width: 6%;text-align: right;'  colspan='2'><b>" + Rate.Value + "</b></td>";
                    }
                }

                if (ViewState["withdiscount"].ToString() == "2")
                {

                    prvdata += "<td style='padding: 5px 10px 5px 5pt;width: 6%;text-align: right;' colspan='2'><b>Discount</b></td>";

                }



                if (Discount1.Checked)
                {
                    prvdata += "<td style='padding: 5px 10px 5px 5pt;width: 6%;text-align: right;'><b>" + Discount.Value + "</b></td>";
                }
                if (showisindia == "0")
                {
                    if (showrental == "0")
                    {
                        if (Taxpercent1.Checked)
                        {
                            prvdata += "<td style='padding: 5px 10px 5px 5pt;width: 6%;text-align: right;' ><b>" + Taxpercent.Value + "</b></td>";
                        }

                        if (TaxAmount1.Checked)
                        {
                            prvdata += "<td style='padding: 5px 10px 5px 5pt;border-right:0pt;width: 8%;text-align: right;border-right:0pt; ' colspan='2'><b>" + TaxAmount.Value + "</b></td>";
                        }
                    }
                }
                if (Amount1.Checked)
                {
                    prvdata += "    <td style='padding: 5px 10px 5px 5pt;border-right:0pt;width: 8%;text-align: right; ' colspan='3'><b>" + Amount.Value + " </b></td>";

                    //prvdata += "    <td style='padding: 5px 10px 5px 5pt;border-right:0pt;width: 8%;text-align: right; ' colspan='3'>sd<b>" + fns.ToFormattedCurrencyString(Convert.ToDecimal(Amount.Value.Trim())) + " </b></td>";


                }
                prvdata += "   </tr>";
                prvdata += " </thead>";
                prvdata += "  <tbody>";







                for (int jj = 0; jj < dsadd.Tables[0].Rows.Count; jj++)
                {


                    if (tableItemRowBackgroundColor1.Checked)
                    {
                        prvdata += " <tr style='border-bottom:1px solid " + showcolor(TableBorder.Value) + "; background:" + showcolor(tableItemRowBackgroundColor.Value) + ";font-size:" + tableItemRowfontsize.Value + "pt;color:" + showcolor(tableItemRowFontColor.Value) + ";'>";
                    }
                    else
                    {
                        prvdata += " <tr style='border-bottom:1px solid " + showcolor(TableBorder.Value) + ";font-size:" + tableItemRowfontsize.Value + "pt;color:" + showcolor(tableItemRowFontColor.Value) + ";'>";
                    }



                    if (SerialNumber1.Checked)
                    {
                        prvdata += " <td style='padding: 10px 0 10px 5px; border-bottom:1px;border-top:0px;text-align: center;text-align: center;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'> " + (jj + 1) + " </td>";
                    }
                    prvdata += " <td style='padding: 10px 0px 10px 10px;border-bottom:1px;border-top:0px;text-align: left;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='6'>";
                    if (Item1.Checked)
                    {

                        string catsubcatname = "";

                        if (iscatshow == 1)
                        {

                            if (dsadd.Tables[0].Rows[jj]["categoryid"].ToString() != string.Empty)
                            {

                                string strsqlcat = "select * from tbl_itemcategory where id='" + dsadd.Tables[0].Rows[jj]["categoryid"].ToString() + "' and status=1 and archive=0";
                                DataSet dscat = da.GetDataSetInline(strsqlcat);
                                if (dscat.Tables[0].Rows.Count > 0)
                                {
                                    catsubcatname += dscat.Tables[0].Rows[0]["catname"].ToString() + " ";
                                }
                                strsqlcat = "select * from tbl_itemsubcatgory where id='" + dsadd.Tables[0].Rows[jj]["subcategoryid"].ToString() + "' and status=1 and archive=0";
                                dscat = da.GetDataSetInline(strsqlcat);
                                if (dscat.Tables[0].Rows.Count > 0)
                                {
                                    catsubcatname += dscat.Tables[0].Rows[0]["subcatname"].ToString() + " ";
                                }

                            }

                        }



                        prvdata += "  <span style='font-size:" + tableitemdescFontSize.Value + "pt;color:" + showcolor(tableitemdescFontColor.Value) + ";' >" + catsubcatname + dsadd.Tables[0].Rows[jj]["pname"].ToString() + " ";
                        string strparmraw = "";
                        if (ViewState["iscustomeparm"] != null)
                        {
                            if (ViewState["iscustomeparm"].ToString() == "1")
                            {
                                try
                                {

                                    if (ViewState["iscustomeparm"] != null)
                                    {
                                        if (ViewState["iscustomeparm"].ToString() == "1")
                                        {
                                            // Label lblitemspecification = (Label)gvr.FindControl("lblitemspecification");
                                            SqlParameter[] prmdata = new SqlParameter[3];
                                            prmdata[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"]);
                                            prmdata[1] = new SqlParameter("@searchtxt", "%");
                                            prmdata[2] = new SqlParameter("@pid", "0");
                                            DataSet dsspecification = da.GetDataSet("tbl_masteritemsdetailsselectmanage", prmdata);
                                            int counterspecification = 1;
                                            string controlvalue = "";
                                            for (int x = 0; x < dsspecification.Tables[0].Rows.Count; x++)
                                            {
                                                string controlid = dsspecification.Tables[0].Rows[x]["id"].ToString();


                                                string strsqldata = "select tbl_itemsdetailscustomefieldvaluespecification.*,tbl_masteritemsdetails.groupname from tbl_itemsdetailscustomefieldvaluespecification inner join tbl_masteritemsdetails on tbl_itemsdetailscustomefieldvaluespecification.controlvalue=tbl_masteritemsdetails.id where uid='" + dsadd.Tables[0].Rows[jj]["itemid"].ToString() + "' and tbl_itemsdetailscustomefieldvaluespecification.controlid='" + controlid + "' and tbl_itemsdetailscustomefieldvaluespecification.status=1 and tbl_itemsdetailscustomefieldvaluespecification.archive=0 and tbl_masteritemsdetails.status=1 and tbl_masteritemsdetails.archive=0";
                                                DataSet dsc = da.GetDataSetInline(strsqldata);
                                                if (dsc.Tables[0].Rows.Count > 0)
                                                {
                                                    if (controlvalue != string.Empty)
                                                    {
                                                        controlvalue += "-" + dsc.Tables[0].Rows[0]["groupname"].ToString();
                                                    }
                                                    else
                                                    {
                                                        controlvalue = dsc.Tables[0].Rows[0]["groupname"].ToString();
                                                    }
                                                }
                                            }

                                            strparmraw = controlvalue;
                                        }
                                    }
                                }
                                catch
                                {

                                }

                            }
                        }
                        string stpramval = "";

                        for (int pr = 1; pr < 13; pr++)
                        {
                            string prcol = "prm" + pr;
                            if (dsadd.Tables[0].Rows[jj][prcol].ToString() != "0" && dsadd.Tables[0].Rows[jj][prcol].ToString() != String.Empty)
                            {
                                if (stpramval != string.Empty)
                                {
                                    stpramval += "-" + getparmnamevalue(dsadd.Tables[0].Rows[jj][prcol].ToString());
                                }
                                else
                                {
                                    stpramval += getparmnamevalue(dsadd.Tables[0].Rows[jj][prcol].ToString());
                                }
                            }

                        }

                        prvdata += strparmraw + " " + stpramval + "</span>";

                    }
                    if (isanmol == 1)
                    {
                        prvdata += "<br><b>Item code : </b>" + dsadd.Tables[0].Rows[jj]["txtordercode"].ToString() + "";
                    }

                    if (Description1.Checked)
                    {
                        if (dsadd.Tables[0].Rows[jj]["itemdesc"].ToString().Replace("\n", "") != string.Empty)
                        {
                            prvdata += "<br><span style='font-size:" + tableitemdescFontSize.Value + "pt;color:" + showcolor(tableitemdescFontColor.Value) + ";'>" + dsadd.Tables[0].Rows[jj]["itemdesc"].ToString().Replace("\n", "<br/>") + "</span>";
                        }
                    }



                    prvdata += " </td>";
                    //try
                    //{
                    //    totalquantity += Convert.ToDecimal(dsadd.Tables[0].Rows[jj]["quantity"].ToString());
                    //}
                    //catch (Exception ex)
                    //{

                    //}


                    if (iscoslight == 1)
                    {
                        prvdata += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='2'><span>" + dsadd.Tables[0].Rows[jj]["ERPCode"].ToString() + "<br/></span></td>";
                    }


                    if (isextracolumn > 0)
                    {
                        prvdata += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='2'><span>" + dsadd.Tables[0].Rows[jj]["extfield1"].ToString().Replace("\n", "<br/>") + "</span></td>";

                    }
                    if (isextracolumn > 1)
                    {
                        prvdata += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='2'><span>" + dsadd.Tables[0].Rows[jj]["extfield2"].ToString().Replace("\n", "<br/>") + "</span></td>";

                    }
                    if (isextracolumn > 2)
                    {
                        prvdata += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='2'><span>" + dsadd.Tables[0].Rows[jj]["extfield3"].ToString().Replace("\n", "<br/>") + "</span></td>";

                    }
                    if (isextracolumn > 3)
                    {
                        prvdata += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='2'><span>" + dsadd.Tables[0].Rows[jj]["extfield4"].ToString().Replace("\n", "<br/>") + "</span></td>";

                    }
                    if (isextracolumn > 4)
                    {
                        prvdata += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='2'><span>" + dsadd.Tables[0].Rows[jj]["extfield5"].ToString().Replace("\n", "<br/>") + "</span></td>";

                    }
                    if (isextracolumn > 5)
                    {
                        prvdata += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='2'><span>" + dsadd.Tables[0].Rows[jj]["extfield6"].ToString().Replace("\n", "<br/>") + "</span></td>";

                    }
                    try
                    {
                        totalquantity += Convert.ToDecimal(dsadd.Tables[0].Rows[jj]["quantity"].ToString());
                    }
                    catch (Exception ex)
                    {

                    }
                    if (ViewState["enableGrossWeight"].ToString() == "1")
                    {


                        //if (Quantity1.Checked)
                        //{


                        if (dsadd.Tables[0].Rows[jj]["Unitval"].ToString() != string.Empty)
                        {
                            prvdata += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='2'><span>" + dsadd.Tables[0].Rows[jj]["quantityGross"].ToString() + " &nbsp;(" + dsadd.Tables[0].Rows[jj]["Unitval"].ToString() + ")</span></td>";
                        }
                        else
                        {
                            prvdata += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='2'><span>" + dsadd.Tables[0].Rows[jj]["quantityGross"].ToString() + "&nbsp;</span></td>";
                        }


                        if (dsadd.Tables[0].Rows[jj]["Unitval"].ToString() != string.Empty)
                        {
                            prvdata += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='2'><span>" + dsadd.Tables[0].Rows[jj]["quantity"].ToString() + "&nbsp;(" + dsadd.Tables[0].Rows[jj]["Unitval"].ToString() + ")</span></td>";
                        }
                        else
                        {
                            prvdata += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='2'><span>" + dsadd.Tables[0].Rows[jj]["quantity"].ToString() + "&nbsp;</span></td>";
                        }

                        //}
                    }
                    else
                    {
                        if (Quantity1.Checked)
                        {

                            if (dsadd.Tables[0].Rows[jj]["Unitval"].ToString() != string.Empty)
                            {
                                prvdata += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='2'><span>" + dsadd.Tables[0].Rows[jj]["quantity"].ToString() + "&nbsp;(" + dsadd.Tables[0].Rows[jj]["Unitval"].ToString() + ")</span></td>";
                            }
                            else
                            {
                                prvdata += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='2'><span>" + dsadd.Tables[0].Rows[jj]["quantity"].ToString() + "&nbsp;</span></td>";
                            }

                        }
                    }



                    if (showrental == "1")
                    {
                        if (Rate1.Checked)
                        {
                            prvdata += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='2'><span>" + dsadd.Tables[0].Rows[jj]["rtypoeno"].ToString() + "&nbsp;" + dsadd.Tables[0].Rows[jj]["lblnumbertype"].ToString() + " </span></td>";


                            prvdata += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='2'><span>" + fns.ToFormattedCurrencyString(dsadd.Tables[0].Rows[jj]["rate"].ToString()) + " &nbsp; " + dsadd.Tables[0].Rows[jj]["lblrtype"].ToString() + "</span></td>";
                        }
                    }
                    else
                    {
                        if (Rate1.Checked)
                        {
                            prvdata += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='2'><span>" + fns.ToFormattedCurrencyString(dsadd.Tables[0].Rows[jj]["rate"].ToString()) + "</span></td>";
                        }
                    }

                    if (ViewState["withdiscount"].ToString() == "2")
                    {

                        prvdata += "<td colspan='2' style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'>" + fns.ToFormattedCurrencyString(dsadd.Tables[0].Rows[jj]["totaldiscount"].ToString()) + " </td>";

                    }

                    if (Discount1.Checked)
                    {
                        prvdata += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'>" + dsadd.Tables[0].Rows[jj]["taxtype"].ToString() + " </td>";
                    }
                    if (showisindia == "0")
                    {
                        if (showrental == "0")
                        {
                            if (Taxpercent1.Checked)
                            {
                                prvdata += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' ><span>" + dsadd.Tables[0].Rows[jj]["taxtvalue"].ToString() + "%</span></td>";
                            }
                            if (TaxAmount1.Checked)
                            {
                                prvdata += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;border-right:0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='2'><span>" + fns.ToFormattedCurrencyString(dsadd.Tables[0].Rows[jj]["taxamount"].ToString()) + "</span></td>";
                            }
                        }
                    }
                    if (Amount1.Checked)
                    {
                        if (ViewState["withdiscount"].ToString() == "3")
                        {

                            decimal totalamountwithdiscount = 0;

                            totalamountwithdiscount = Convert.ToDecimal(dsadd.Tables[0].Rows[jj]["amount"].ToString());
                            if (dsadd.Tables[0].Rows[jj]["totaldiscount"].ToString() != string.Empty)
                            {
                                totalamountwithdiscount += Convert.ToDecimal(dsadd.Tables[0].Rows[jj]["totaldiscount"].ToString());
                            }



                            prvdata += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;border-right:0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='3'><span>" + fns.ToFormattedCurrencyString(totalamountwithdiscount.ToString()) + "</span></td>";

                        }
                        else
                        {


                            prvdata += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;border-right:0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='3'><span>" + fns.ToFormattedCurrencyString(dsadd.Tables[0].Rows[jj]["amount"].ToString()) + "</span></td>";


                        }


                    }
                    prvdata += "</tr>";

                }
            }

            prvdata += "</tbody></table>";
            prvdata += "<table style='width:100%; margin:0pt;border: 0px;' cellspacing='0' cellpadding='0' border='1'>";

            prvdata += "<tr><td aling='top' colspan='9' style='border:0px; padding:0px; border-color:" + showcolor(TableBorder.Value) + " !important;' >";


            if (showitemQuantity.Checked || Showamountinwords.Checked)
            {
                prvdata += "<table style='width:100%; margin:0pt;' cellspacing='0' cellpadding='5' border='0'><tr><td >";


                if (showitemQuantity.Checked)
                {
                    prvdata += ItemsinTotal.Value + " " + totalquantity.ToString() + "<br/>";

                }
                if (Showamountinwords.Checked)
                {

                    prvdata += "<b>Amount in Words :</b><br/>";
                    // prvdata += "" + fns.getnumbertoworld(dsstrsqlpurch.Tables[0].Rows[0]["totalamount"].ToString()) + "";

                    if (fns.bindcurdefaultid(dsstrsqlpurch.Tables[0].Rows[0]["currencyid"].ToString()) == "138")
                    {

                        prvdata += "" + fns.getnumbertoworldusd(dsstrsqlpurch.Tables[0].Rows[0]["totalamount"].ToString()) + "";

                        //try
                        //{
                        //    string strsql111 = "select tbl_Currenciesdetails.*,tbl_Currenciesmaster.Currency,tbl_Currenciesmaster.curName from tbl_Currenciesdetails inner join tbl_Currenciesmaster on tbl_Currenciesdetails.curid=tbl_Currenciesmaster.id where tbl_Currenciesdetails.regid='" + Request.Cookies["cadmin"]["id"].ToString() + "' and tbl_Currenciesdetails.status=1 and tbl_Currenciesdetails.archive=0 and  isdefault=1";
                        //    DataSet dssss = da.GetDataSetInline(strsql111);
                        //    if (dssss.Tables[0].Rows.Count > 0)
                        //    {
                        //        ViewState["mathround"] = dssss.Tables[0].Rows[0]["decimalpoint"].ToString();

                        //    }
                        //    int rountno = Convert.ToInt32(ViewState["mathround"]);
                        //    decimal totalamountundefault = Math.Round((Convert.ToDecimal(dsstrsqlpurch.Tables[0].Rows[0]["totalamount"].ToString()) * Convert.ToDecimal(dsstrsqlpurch.Tables[0].Rows[0]["convertionrate"].ToString())), rountno);

                        //    ViewState["totalamountundefaul"] = totalamountundefault.ToString();

                        //    string strsql = "select tbl_Currenciesmaster.* from tbl_Currenciesdetails inner join tbl_Currenciesmaster on tbl_Currenciesdetails.curid=tbl_Currenciesmaster.id where tbl_Currenciesdetails.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and tbl_Currenciesdetails.status=1 and tbl_Currenciesdetails.archive=0 and isdefault=1";

                        //    // string strsql = "select * from tbl_Currenciesmaster where id='" + fns.bindcurdefaultid(dsstrsqlpurch.Tables[0].Rows[0]["currencyid"].ToString()) + "'";
                        //    DataSet dscurname = da.GetDataSetInline(strsql);
                        //    if (dscurname.Tables[0].Rows.Count > 0)
                        //    {
                        //        prvdata += "<br/>" + fns.getnumbertoworldcur(totalamountundefault.ToString(), dscurname.Tables[0].Rows[0]["curworld"].ToString(), dscurname.Tables[0].Rows[0]["curworldpaisa"].ToString()) + "";
                        //    }


                        //    //prvdata += "<br/>" + fns.getnumbertoworldcur(totalamountundefault.ToString()) + "";
                        //}
                        //catch (Exception ex)
                        //{
                        //}



                    }
                    else
                    {

                        string strsql = "select * from tbl_Currenciesmaster where id='" + fns.bindcurdefaultid(dsstrsqlpurch.Tables[0].Rows[0]["currencyid"].ToString()) + "'";
                        DataSet dscurname = da.GetDataSetInline(strsql);
                        if (dscurname.Tables[0].Rows.Count > 0)
                        {
                            prvdata += "" + fns.getnumbertoworldcur(dsstrsqlpurch.Tables[0].Rows[0]["totalamount"].ToString(), dscurname.Tables[0].Rows[0]["curworld"].ToString(), dscurname.Tables[0].Rows[0]["curworldpaisa"].ToString()) + "";
                        }

                        // prvdata += "" + fns.getnumbertoworld(dsstrsqlpurch.Tables[0].Rows[0]["totalamount"].ToString()) + "";
                    }

                }

                prvdata += "</td></tr></table>";
            }
            if (chkistermsandcondition.Checked || chkisnotes.Checked)
            {
                prvdata += "<table style='width:100%;margin:15px 0 0 0;' cellspacing='0' cellpadding='5' border='0'>";
                prvdata += "<tbody>";
                // if (chkisnotes.Checked)
                //  {
                prvdata += "<tr>";
                prvdata += "<td style='padding:5px 10px 5px 5pt;font-size:11pt;' align='left'><span style='color:" + getlabelcolor() + ";font-size:" + TermsConditionsFontSize.Value + "pt;'><b>" + FooterNotesLabel.Value + "</b></span><br>";
                prvdata += "<span style='font-size:" + FooterFontSize.Value + "pt;'>" + dsstrsqlpurch.Tables[0].Rows[0]["CustomerNotes"].ToString().Replace("\n", "<br/>") + "</span></td>";
                prvdata += "</tr>";
                // }
                if (Request.Cookies["cadmin"]["id"].ToString() != "50")
                {
                    if (chkistermsandcondition.Checked)
                    {
                        prvdata += "<tr>";
                        prvdata += "<td style='padding:5px 10px 5px 5pt;' align='left'><span style='color:" + getlabelcolor() + ";font-size:" + TermsConditionsFontSize.Value + "pt;'><b>" + TermsConditionsLabel.Value + "</b></span><br>";
                        prvdata += "<span style='font-size:" + FooterFontSize.Value + "pt;'>" + dsstrsqlpurch.Tables[0].Rows[0]["TermsandConditions"].ToString().Replace("\n", "<br/>") + "</span></td>";
                        prvdata += "</tr>";
                    }
                }
                prvdata += "</tbody>";
                prvdata += "</table>";
            }

            prvdata += bindcusdatafooter();
            prvdata += "</td>";
            if (ShowTotalSection.Checked)
            {
                if (ViewState["withdiscount"].ToString() == "2")
                {
                    if (ViewState["enableGrossWeight"].ToString() == "1")
                    {
                        prvdata += "<td colspan='11' style='width:40.5%;padding:0px 0px 0px 0pt; border:0px; border-left:1px solid; border-color:" + showcolor(TableBorder.Value) + " !important;'>";
                    }
                    else
                    {
                        prvdata += "<td colspan='10' style='width:40.5%;padding:0px 0px 0px 0pt; border:0px; border-left:1px solid; border-color:" + showcolor(TableBorder.Value) + " !important;'>";

                    }
                }
                else
                {
                    if (ViewState["enableGrossWeight"].ToString() == "1")
                    {
                        prvdata += "<td colspan='9' style='width:40.5%;padding:0px 0px 0px 0pt; border:0px; border-left:1px solid; border-color:" + showcolor(TableBorder.Value) + " !important;'>";
                    }
                    else
                    {
                        prvdata += "<td colspan='8' style='width:40.5%;padding:0px 0px 0px 0pt; border:0px; border-left:1px solid; border-color:" + showcolor(TableBorder.Value) + " !important;'>";

                    }
                }

                string TotalSubtotaltaxBackgroundColorvalue = "";
                prvdata += "<table border='0' width='100%' cellpadding='0' cellspacing='0'  >";

                //if (ViewState["withdiscount"].ToString() == "2")
                //{
                //    prvdata += "<tr>";
                //    prvdata += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;font-size:" + TotalSubtotaltaxFontSize.Value + "pt;color:" + showcolor(TotalSubtotaltaxFontColor.Value) + ";background:" + TotalSubtotaltaxBackgroundColorvalue + ";' valign='top' ><b><span>Total Discount&nbsp;</span></b></td>";
                //    prvdata += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' ><span>" + fns.ToFormattedCurrencyString(dsstrsqlpurch.Tables[0].Rows[0]["discountamount"].ToString()) + "&nbsp;</span></td>";
                //    prvdata += "</tr>";

                //}
                if (ViewState["withdiscount"].ToString() == "3")
                {

                    decimal totalsubtotalmain = Convert.ToDecimal(dsstrsqlpurch.Tables[0].Rows[0]["SubTotal"].ToString()) + Convert.ToDecimal(dsstrsqlpurch.Tables[0].Rows[0]["discountamount"].ToString());

                    prvdata += "<tr>";
                    prvdata += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;font-size:" + TotalSubtotaltaxFontSize.Value + "pt;color:" + showcolor(TotalSubtotaltaxFontColor.Value) + ";background:" + TotalSubtotaltaxBackgroundColorvalue + ";' valign='top' ><b><span>Sub Total&nbsp;</span></b></td>";
                    prvdata += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' ><span>" + fns.ToFormattedCurrencyString(totalsubtotalmain.ToString()) + "&nbsp;</span></td>";
                    prvdata += "</tr>";

                    prvdata += "<tr>";
                    prvdata += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;font-size:" + TotalSubtotaltaxFontSize.Value + "pt;color:" + showcolor(TotalSubtotaltaxFontColor.Value) + ";background:" + TotalSubtotaltaxBackgroundColorvalue + ";' valign='top' ><b><span>Total Discount&nbsp;</span></b></td>";
                    prvdata += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' ><span>" + fns.ToFormattedCurrencyString(dsstrsqlpurch.Tables[0].Rows[0]["discountamount"].ToString()) + "&nbsp;</span></td>";
                    prvdata += "</tr>";

                }

                if (ViewState["withdiscount"].ToString() == "2")
                {
                    prvdata += "<tr>";
                    prvdata += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;font-size:" + TotalSubtotaltaxFontSize.Value + "pt;color:" + showcolor(TotalSubtotaltaxFontColor.Value) + ";background:" + TotalSubtotaltaxBackgroundColorvalue + ";' valign='top' ><b><span>Total Discount&nbsp;</span></b></td>";
                    prvdata += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' ><span>" + fns.ToFormattedCurrencyString(dsstrsqlpurch.Tables[0].Rows[0]["discountamount"].ToString()) + "&nbsp;</span></td>";
                    prvdata += "</tr>";

                }
                if (SubTotal1.Checked)
                {
                    prvdata += " <tr>";


                    if (TotalSubtotaltaxBackgroundColor1.Checked)
                    {
                        TotalSubtotaltaxBackgroundColorvalue = showcolor(TotalSubtotaltaxBackgroundColor.Value);

                    }

                    prvdata += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;font-size:" + TotalSubtotaltaxFontSize.Value + "pt;color:" + showcolor(TotalSubtotaltaxFontColor.Value) + ";background:" + TotalSubtotaltaxBackgroundColorvalue + ";' valign='top' ><b><span>" + SubTotal.Value + "&nbsp;</span></b></td>";
                    prvdata += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'><span>" + fns.ToFormattedCurrencyString(dsstrsqlpurch.Tables[0].Rows[0]["SubTotal"].ToString()) + "&nbsp;</span></td>";
                    prvdata += "</tr>";
                }
                string strsqlextracharge = "select tbl_salesorderextrachargesdetails.*,tbl_ledgers.groupname as groupname from tbl_salesorderextrachargesdetails inner join tbl_ledgers on tbl_salesorderextrachargesdetails.extrachargeid=tbl_ledgers.refid where tbl_ledgers.entrytype=10 and tbl_salesorderextrachargesdetails.invoiceid='" + ViewState["invoiceid"].ToString() + "' and tbl_salesorderextrachargesdetails.status=1 and tbl_salesorderextrachargesdetails.archive=0 and istax=1 order by id";
                DataSet dsextrac = da.GetDataSetInline(strsqlextracharge);

                // Response.Write(strsqlextracharge);
                if (dsextrac.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < dsextrac.Tables[0].Rows.Count; i++)
                    {
                        prvdata += " <tr>";
                        prvdata += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;font-size:" + TotalSubtotaltaxFontSize.Value + "pt;color:" + showcolor(TotalSubtotaltaxFontColor.Value) + ";background:" + TotalSubtotaltaxBackgroundColorvalue + ";' valign='top' ><b><span>" + dsextrac.Tables[0].Rows[i]["groupname"].ToString() + "&nbsp;</span></b></td>";
                        prvdata += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'><span>" + fns.ToFormattedCurrencyString(dsextrac.Tables[0].Rows[i]["extrachargeamount"].ToString()) + "&nbsp;</span></td>";
                        prvdata += "</tr>";

                    }
                }
                else
                {
                    if (Request.Cookies["cadmin"]["id"].ToString() == "263")
                    {
                        prvdata += " <tr>";
                        prvdata += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;font-size:" + TotalSubtotaltaxFontSize.Value + "pt;color:" + showcolor(TotalSubtotaltaxFontColor.Value) + ";background:" + TotalSubtotaltaxBackgroundColorvalue + ";' valign='top' ><b><span>Freight &nbsp;</span></b></td>";
                        prvdata += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'><span>" + fns.ToFormattedCurrencyString("0") + "&nbsp;</span></td>";
                        prvdata += "</tr>";
                    }
                }
                if (showisindia == "0")
                {


                    if (isvat == "1")
                    {
                        prvdata += "<tr>";
                        prvdata += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;font-size:" + TotalSubtotaltaxFontSize.Value + "pt;color:" + showcolor(TotalSubtotaltaxFontColor.Value) + ";background:" + TotalSubtotaltaxBackgroundColorvalue + ";' valign='top' ><b><span>Total VAT&nbsp;</span></b></td>";
                        prvdata += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'><span>" + fns.ToFormattedCurrencyString(dsstrsqlpurch.Tables[0].Rows[0]["totalvat"].ToString()) + "&nbsp;</span></td>";
                        prvdata += "</tr>";
                    }
                }
                else
                {

                    string strsql = "select * from tbl_contactsdetails where id='" + dsstrsqlpurch.Tables[0].Rows[0]["cusid"].ToString() + "' and status=1 and archive=0 and regid='" + Request.Cookies["cadmin"]["id"].ToString() + "'";
                    DataSet dsissame = da.GetDataSetInline(strsql);
                    if (dsissame.Tables[0].Rows.Count > 0)
                    {
                        if (showbranch == "")
                        {
                            if (fns.getstateidbybranchid(ViewState["branchid"].ToString()).ToString() == dsissame.Tables[0].Rows[0]["PlaceOfSupply"].ToString())
                            {
                                ViewState["issamestate"] = "1";
                                prvdata += "<tr>";
                                prvdata += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;font-size:" + TotalSubtotaltaxFontSize.Value + "pt;color:" + showcolor(TotalSubtotaltaxFontColor.Value) + ";background:" + TotalSubtotaltaxBackgroundColorvalue + ";' valign='top' ><b><span>SGST&nbsp;</span></b></td>";
                                prvdata += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'><span>" + fns.ToFormattedCurrencyString((Convert.ToDecimal(dsstrsqlpurch.Tables[0].Rows[0]["totalvat"].ToString()) / 2).ToString()) + "&nbsp;</span></td>";
                                prvdata += "</tr>";

                                prvdata += "<tr>";
                                prvdata += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;font-size:" + TotalSubtotaltaxFontSize.Value + "pt;color:" + showcolor(TotalSubtotaltaxFontColor.Value) + ";background:" + TotalSubtotaltaxBackgroundColorvalue + ";' valign='top' ><b><span>CGST&nbsp;</span></b></td>";
                                prvdata += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'><span>" + fns.ToFormattedCurrencyString((Convert.ToDecimal(dsstrsqlpurch.Tables[0].Rows[0]["totalvat"].ToString()) / 2).ToString()) + "&nbsp;</span></td>";
                                prvdata += "</tr>";
                            }
                            else
                            {
                                prvdata += "<tr>";
                                prvdata += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;font-size:" + TotalSubtotaltaxFontSize.Value + "pt;color:" + showcolor(TotalSubtotaltaxFontColor.Value) + ";background:" + TotalSubtotaltaxBackgroundColorvalue + ";' valign='top' ><b><span>IGST&nbsp;</span></b></td>";
                                prvdata += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'><span>" + fns.ToFormattedCurrencyString(dsstrsqlpurch.Tables[0].Rows[0]["totalvat"].ToString()) + "&nbsp;</span></td>";
                                prvdata += "</tr>";

                                ViewState["issamestate"] = "0";
                            }
                        }
                        else
                        {


                            if (fns.getstateid().ToString() == dsissame.Tables[0].Rows[0]["PlaceOfSupply"].ToString())
                            {
                                ViewState["issamestate"] = "1";
                                prvdata += "<tr>";
                                prvdata += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;font-size:" + TotalSubtotaltaxFontSize.Value + "pt;color:" + showcolor(TotalSubtotaltaxFontColor.Value) + ";background:" + TotalSubtotaltaxBackgroundColorvalue + ";' valign='top' ><b><span>SGST&nbsp;</span></b></td>";
                                prvdata += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'><span>" + fns.ToFormattedCurrencyString((Convert.ToDecimal(dsstrsqlpurch.Tables[0].Rows[0]["totalvat"].ToString()) / 2).ToString()) + "&nbsp;</span></td>";
                                prvdata += "</tr>";

                                prvdata += "<tr>";
                                prvdata += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;font-size:" + TotalSubtotaltaxFontSize.Value + "pt;color:" + showcolor(TotalSubtotaltaxFontColor.Value) + ";background:" + TotalSubtotaltaxBackgroundColorvalue + ";' valign='top' ><b><span>CGST&nbsp;</span></b></td>";
                                prvdata += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'><span>" + fns.ToFormattedCurrencyString((Convert.ToDecimal(dsstrsqlpurch.Tables[0].Rows[0]["totalvat"].ToString()) / 2).ToString()) + "&nbsp;</span></td>";
                                prvdata += "</tr>";
                                //trsgst.Visible = true;
                                //trcgst.Visible = true;
                                //lblvat.Text = "Total Tax";

                            }
                            else
                            {

                                prvdata += "<tr>";
                                prvdata += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;font-size:" + TotalSubtotaltaxFontSize.Value + "pt;color:" + showcolor(TotalSubtotaltaxFontColor.Value) + ";background:" + TotalSubtotaltaxBackgroundColorvalue + ";' valign='top' ><b><span>IGST&nbsp;</span></b></td>";
                                prvdata += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'><span>" + fns.ToFormattedCurrencyString(dsstrsqlpurch.Tables[0].Rows[0]["totalvat"].ToString()) + "&nbsp;</span></td>";
                                prvdata += "</tr>";

                                ViewState["issamestate"] = "0";
                                //trsgst.Visible = false;
                                //trcgst.Visible = false;
                                //lblvat.Text = "IGST";
                            }

                        }
                    }
                }

                if (isvat == "1")
                {

                    strsqlextracharge = "select tbl_salesorderextrachargesdetails.*,tbl_ledgers.groupname as groupname from tbl_salesorderextrachargesdetails inner join tbl_ledgers on tbl_salesorderextrachargesdetails.extrachargeid=tbl_ledgers.refid where tbl_ledgers.entrytype=10 and tbl_salesorderextrachargesdetails.invoiceid='" + ViewState["invoiceid"].ToString() + "' and tbl_salesorderextrachargesdetails.status=1 and tbl_salesorderextrachargesdetails.archive=0 and istax=0 order by id";
                    dsextrac = da.GetDataSetInline(strsqlextracharge);

                    // Response.Write(strsqlextracharge);
                    if (dsextrac.Tables[0].Rows.Count > 0)
                    {

                        for (int i = 0; i < dsextrac.Tables[0].Rows.Count; i++)
                        {
                            prvdata += " <tr>";
                            prvdata += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;font-size:" + TotalSubtotaltaxFontSize.Value + "pt;color:" + showcolor(TotalSubtotaltaxFontColor.Value) + ";background:" + TotalSubtotaltaxBackgroundColorvalue + ";' valign='top' ><b><span>" + dsextrac.Tables[0].Rows[i]["groupname"].ToString() + "&nbsp;</span></b></td>";
                            prvdata += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'><span>" + fns.ToFormattedCurrencyString(dsextrac.Tables[0].Rows[i]["extrachargeamount"].ToString()) + "&nbsp;</span></td>";
                            prvdata += "</tr>";

                        }
                    }
                    prvdata += "<tr>";
                    prvdata += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;font-size:" + TotalSubtotaltaxFontSize.Value + "pt;color:" + showcolor(TotalSubtotaltaxFontColor.Value) + ";background:" + TotalSubtotaltaxBackgroundColorvalue + ";' valign='top' ><b><span>" + dsstrsqlpurch.Tables[0].Rows[0]["extratfields"].ToString() + "&nbsp;</span></b></td>";
                    prvdata += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' ><span>" + fns.ToFormattedCurrencyString(dsstrsqlpurch.Tables[0].Rows[0]["extrafiledvalue"].ToString()) + "&nbsp;</span></td>";
                    prvdata += "</tr>";
                    prvdata += "<tr>";
                    prvdata += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;font-size:" + TotalSubtotaltaxFontSize.Value + "pt;color:" + showcolor(TotalSubtotaltaxFontColor.Value) + ";background:" + TotalSubtotaltaxBackgroundColorvalue + ";' valign='top' ><b><span>" + fns.ToFormattedCurrencyString(tableTotal.Value) + "&nbsp;</span></b></td>";

                    if (CurrencySymbol.SelectedValue == "Before amount")
                    {
                        prvdata += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' ><span>" + fns.bindcurdefault(dsstrsqlpurch.Tables[0].Rows[0]["currencyid"].ToString()) + " " + fns.ToFormattedCurrencyString(dsstrsqlpurch.Tables[0].Rows[0]["totalamount"].ToString()) + "&nbsp;</span><br/><br/></td>";
                    }
                    else
                    {
                        prvdata += "<td style='padding: 5px 5px 5px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' ><span>" + fns.ToFormattedCurrencyString(dsstrsqlpurch.Tables[0].Rows[0]["totalamount"].ToString()) + " " + fns.bindcurdefault(dsstrsqlpurch.Tables[0].Rows[0]["currencyid"].ToString()) + "&nbsp;</span><br/><br/></td>";

                    }

                    prvdata += " </tr>";
                }
                if (isSignatur.Checked)
                {
                    prvdata += "<tr  text-align:center;'> <td colspan='2' width='100%' border='1' style='vertical-align:bottom; text-align:center; padding:0px;border-bottom:0px solid #000000;' align='center' ></td></tr>";


                    if (backgroundimgsignature != string.Empty)
                    {
                        prvdata += "<tr  style='border-top:1px solid " + showcolor(TableBorder.Value) + "; text-align:center;'> <td colspan='2' width='100%' border='0' style='vertical-align:bottom; text-align:center; padding:0px;border-bottom:0px solid #000000;' height='150px' align='center' >";

                        prvdata += "<br/><br/><h1><center><img  style='width:auto;' src='" + ConfigurationManager.AppSettings["domainname"].ToString() + "/" + backgroundimgsignature + "' width='100%'  align='center' /></center></h1>";
                        prvdata += "<b>" + isSignatureLabel.Value + "</b><br/><br/>&nbsp;";
                        prvdata += "</td></tr>";
                    }
                    else
                    {
                        prvdata += "<tr  style='border-top:1px solid " + showcolor(TableBorder.Value) + "; text-align:center;'> <td colspan='2' width='100%' border='0' style='vertical-align:bottom; padding:0px;border-bottom:0px solid #000000; text-align:center;' height='150px' align='center' >";

                        prvdata += "&nbsp;<br/><br/><br/>" + SignatureName.Value + "&nbsp;<br/><b>";
                        prvdata += isSignatureLabel.Value;
                        prvdata += "&nbsp;</b></center><br/><br/></td></tr>";
                    }




                    //prvdata += "<tr  style='border-top:1px solid " + showcolor(TableBorder.Value) + ";border-bottom:0px; text-align:center;'> <td colspan='2' width='100%' border='1' style='vertical-align:bottom; text-align:center; padding:0pt;border-bottom:0px solid #000000;' height='100px'  >";
                    //prvdata += "&nbsp;<br/><br/>" + SignatureName.Value + "&nbsp;<br/>";

                    //prvdata += isSignatureLabel.Value;
                    //prvdata += "</td></tr>";
                }


                prvdata += "</table>";


            }


            prvdata += "</td>";
            prvdata += "</tr></table>";
            //prvdata += " </table>";



            if (Request.Cookies["cadmin"]["id"].ToString() == "50")
            {
                if (chkistermsandcondition.Checked)
                {
                    prvdata += "<table style='width:100%;margin:0px 0 0 0;' cellspacing='0' cellpadding='5' border='1'>";
                    prvdata += "<tbody>";
                    prvdata += "<tr>";
                    prvdata += "<td style='padding:5px 10px 5px 5pt;' align='left'><span style='color:" + getlabelcolor() + ";font-size:" + TermsConditionsFontSize.Value + "pt;'><b>" + TermsConditionsLabel.Value + "</b></span><br>";
                    prvdata += "<span style='font-size:" + FooterFontSize.Value + "pt;'>" + dsstrsqlpurch.Tables[0].Rows[0]["TermsandConditions"].ToString().Replace("\n", "<br/>") + "</span></td>";
                    prvdata += "</tr>";
                    prvdata += "</tbody>";
                    prvdata += "</table>";
                }
            }
            prvdata += "</td></tr>";
            prvdata += "</table>";


            if (showisindia == "1" && showgstsummery == "1")
            {
                prvdata += "<table cellpadding='5' style='width:100%;margin: 0;border-left:0pt;border-right:0pt; border-color:" + showcolor(TableBorder.Value) + " !important;' cellspacing='0' border='1'>";
                prvdata += "<tr  bgcolor='" + showcolor(tableheaderBackgroundColor.Value) + "' style='height:32pt;border-left:0pt;border-right:0pt; background:" + showcolor(tableheaderBackgroundColor.Value) + ";font-size:" + headerFontSize.Value + "pt;color:" + showcolor(tableheaderFontColor.Value) + ";'><th rowspan='2' style='text-align: center;' valign='top'><b>HSN/SAC</b></th><th rowspan='2' style='text-align: right;' valign='top'><b>Taxable</b></th><th colspan='2' style='text-align: center;'><b>IGST</b></th><th colspan='2' style='text-align: center;'><b>SGST</b></th><th colspan='2' style='text-align: center;'><b>CGST</b></th></tr>";
                prvdata += "<tr  bgcolor='" + showcolor(tableheaderBackgroundColor.Value) + "' style='height:32pt;border-left:0pt;border-right:0pt; background:" + showcolor(tableheaderBackgroundColor.Value) + ";font-size:" + headerFontSize.Value + "pt;color:" + showcolor(tableheaderFontColor.Value) + ";'><th style='text-align: right;'><b>%</b></th><th style='text-align: right;'><b>Amount</b></th><th style='text-align: right;'><b>%</b></th><th style='text-align: right;'><b>Amount</b></th><th style='text-align: right;'><b>%</b></th><th style='text-align: right;'><b>Amount</b></th></tr>";


                decimal totaligst = 0;
                decimal totalsgst = 0;
                // decimal totalcgst = 0;
                // string strsqlitemhsn = "select *,tbl_itemsdetails.pname,tbl_itemsdetails.Unitval,tbl_itemsdetails.SKUval from tbl_SalesOrderdetailsitemsdetails left outer join tbl_itemsdetails on tbl_SalesOrderdetailsitemsdetails.itemid=tbl_itemsdetails.id where tbl_SalesOrderdetailsitemsdetails.estmid='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "' and tbl_SalesOrderdetailsitemsdetails.status=1 and tbl_SalesOrderdetailsitemsdetails.archive=0  order by tbl_SalesOrderdetailsitemsdetails.id ";
                string strsqlitemhsn = "select tbl_itemsdetails.SKUval ,sum(amount) as amount,sum(taxamount) as taxamount,taxtvalue from tbl_SalesOrderdetailsitemsdetails left outer join tbl_itemsdetails on tbl_SalesOrderdetailsitemsdetails.itemid=tbl_itemsdetails.id where tbl_SalesOrderdetailsitemsdetails.estmid='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "' and tbl_SalesOrderdetailsitemsdetails.status=1 and tbl_SalesOrderdetailsitemsdetails.archive=0  group by SKUval,taxtvalue  order by SKUval";


                DataSet dsaddhsn = da.GetDataSetInline(strsqlitemhsn);


                if (dsaddhsn.Tables[0].Rows.Count > 0)
                {



                    for (int j = 0; j < dsaddhsn.Tables[0].Rows.Count; j++)
                    {
                        if (tableItemRowBackgroundColor1.Checked)
                        {
                            prvdata += " <tr style='border-bottom:1px solid " + showcolor(TableBorder.Value) + "; background:" + showcolor(tableItemRowBackgroundColor.Value) + ";font-size:" + tableItemRowfontsize.Value + "pt;color:" + showcolor(tableItemRowFontColor.Value) + ";'>";
                        }
                        else
                        {
                            prvdata += " <tr style='border-bottom:1px solid " + showcolor(TableBorder.Value) + ";font-size:" + tableItemRowfontsize.Value + "pt;color:" + showcolor(tableItemRowFontColor.Value) + ";'>";
                        }


                        prvdata += "<td style='text-align: center;' valign='top' >" + dsaddhsn.Tables[0].Rows[j]["SKUval"].ToString() + "</td>";
                        prvdata += "<td style='text-align: right;' valign='top'>" + fns.ToFormattedCurrencyString(dsaddhsn.Tables[0].Rows[j]["amount"].ToString()) + "</td>";
                        if (ViewState["issamestate"].ToString() == "1")
                        {
                            prvdata += "<td style='text-align: right;'>0</td>";
                            prvdata += "<td style='text-align: right;'>0</td>";

                            prvdata += "<td style='text-align: right;'>" + (Convert.ToDecimal(dsaddhsn.Tables[0].Rows[j]["taxtvalue"].ToString()) / 2) + "</td>";
                            prvdata += "<td style='text-align: right;'>" + fns.ToFormattedCurrencyString((Convert.ToDecimal(dsaddhsn.Tables[0].Rows[j]["taxamount"].ToString()) / 2).ToString()) + "</td>";
                            prvdata += "<td style='text-align: right;'>" + (Convert.ToDecimal(dsaddhsn.Tables[0].Rows[j]["taxtvalue"].ToString()) / 2) + "</td>";
                            prvdata += "<td style='text-align: right;'>" + fns.ToFormattedCurrencyString((Convert.ToDecimal(dsaddhsn.Tables[0].Rows[j]["taxamount"].ToString()) / 2).ToString()) + "</td>";
                            totalsgst += (Convert.ToDecimal(dsaddhsn.Tables[0].Rows[j]["taxamount"].ToString()) / 2);


                        }
                        else
                        {



                            //prvdata += "<td style='text-align: right;'>0</td>";
                            //prvdata += "<td style='text-align: right;'>0</td>";
                            prvdata += "<td style='text-align: right;'>" + dsaddhsn.Tables[0].Rows[j]["taxtvalue"].ToString() + "</td>";
                            prvdata += "<td style='text-align: right;'>" + fns.ToFormattedCurrencyString(dsaddhsn.Tables[0].Rows[j]["taxamount"].ToString()) + "</td>";
                            prvdata += "<td style='text-align: right;'>0</td>";
                            prvdata += "<td style='text-align: right;'>0</td>";
                            prvdata += "<td style='text-align: right;'>0</td>";
                            prvdata += "<td style='text-align: right;'>0</td>";
                            totaligst += Convert.ToDecimal(dsaddhsn.Tables[0].Rows[j]["taxamount"].ToString());
                        }
                        prvdata += "</tr>";

                    }
                }


                string strsqlextracharge = "select tbl_salesorderextrachargesdetails.*,tbl_extrachargesdetails.vatclassificationcode as SKUval from tbl_salesorderextrachargesdetails inner join tbl_extrachargesdetails on tbl_salesorderextrachargesdetails.extrachargeid=tbl_extrachargesdetails.id where tbl_salesorderextrachargesdetails.invoiceid='" + ViewState["invoiceid"].ToString() + "' and tbl_salesorderextrachargesdetails.status=1 and tbl_salesorderextrachargesdetails.archive=0 and istax=1 order by id";
                DataSet dsextrac = da.GetDataSetInline(strsqlextracharge);

                // Response.Write(strsqlextracharge);
                if (dsextrac.Tables[0].Rows.Count > 0)
                {

                    for (int j = 0; j < dsextrac.Tables[0].Rows.Count; j++)
                    {



                        if (tableItemRowBackgroundColor1.Checked)
                        {
                            prvdata += " <tr style='border-bottom:1px solid " + showcolor(TableBorder.Value) + "; background:" + showcolor(tableItemRowBackgroundColor.Value) + ";font-size:" + tableItemRowfontsize.Value + "pt;color:" + showcolor(tableItemRowFontColor.Value) + ";'>";
                        }
                        else
                        {
                            prvdata += " <tr style='border-bottom:1px solid " + showcolor(TableBorder.Value) + ";font-size:" + tableItemRowfontsize.Value + "pt;color:" + showcolor(tableItemRowFontColor.Value) + ";'>";
                        }


                        prvdata += "<td style='text-align: center;' valign='top' >" + dsextrac.Tables[0].Rows[j]["SKUval"].ToString() + "</td>";
                        prvdata += "<td style='text-align: right;' valign='top'>" + fns.ToFormattedCurrencyString(dsextrac.Tables[0].Rows[j]["extrachargeamount"].ToString()) + "</td>";
                        if (ViewState["issamestate"].ToString() == "1")
                        {
                            prvdata += "<td style='text-align: right;'>0</td>";
                            prvdata += "<td style='text-align: right;'>0</td>";

                            prvdata += "<td style='text-align: right;'>" + (Convert.ToDecimal(dsextrac.Tables[0].Rows[j]["taxpersentage"].ToString()) / 2) + "</td>";
                            prvdata += "<td style='text-align: right;'>" + fns.ToFormattedCurrencyString((Convert.ToDecimal(dsextrac.Tables[0].Rows[j]["istaxamount"].ToString()) / 2).ToString()) + "</td>";
                            prvdata += "<td style='text-align: right;'>" + (Convert.ToDecimal(dsextrac.Tables[0].Rows[j]["taxpersentage"].ToString()) / 2) + "</td>";
                            prvdata += "<td style='text-align: right;'>" + fns.ToFormattedCurrencyString((Convert.ToDecimal(dsextrac.Tables[0].Rows[j]["istaxamount"].ToString()) / 2).ToString()) + "</td>";
                            totalsgst += (Convert.ToDecimal(dsextrac.Tables[0].Rows[j]["istaxamount"].ToString()) / 2);


                        }
                        else
                        {




                            prvdata += "<td style='text-align: right;'>" + dsextrac.Tables[0].Rows[j]["taxpersentage"].ToString() + "</td>";
                            prvdata += "<td style='text-align: right;'>" + fns.ToFormattedCurrencyString(dsextrac.Tables[0].Rows[j]["istaxamount"].ToString()) + "</td>";
                            prvdata += "<td style='text-align: right;'>0</td>";
                            prvdata += "<td style='text-align: right;'>0</td>";
                            prvdata += "<td style='text-align: right;'>0</td>";
                            prvdata += "<td style='text-align: right;'>0</td>";
                            totaligst += Convert.ToDecimal(dsextrac.Tables[0].Rows[j]["istaxamount"].ToString());
                        }
                        prvdata += "</tr>";


                        //prvdata += " <tr>";
                        //prvdata += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;font-size:" + TotalSubtotaltaxFontSize.Value + "pt;color:" + showcolor(TotalSubtotaltaxFontColor.Value) + ";background:" + TotalSubtotaltaxBackgroundColorvalue + ";' valign='top' ><b><span>" + dsextrac.Tables[0].Rows[i]["groupname"].ToString() + "&nbsp;</span></b></td>";
                        //prvdata += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'><span>" + fns.ToFormattedCurrencyString(dsextrac.Tables[0].Rows[i]["extrachargeamount"].ToString()) + "&nbsp;</span></td>";
                        //prvdata += "</tr>";

                    }
                }

                if (tableItemRowBackgroundColor1.Checked)
                {
                    prvdata += " <tr style='border-bottom:1px solid " + showcolor(TableBorder.Value) + "; background:" + showcolor(tableItemRowBackgroundColor.Value) + ";font-size:" + tableItemRowfontsize.Value + "pt;color:" + showcolor(tableItemRowFontColor.Value) + ";'>";
                }
                else
                {
                    prvdata += " <tr style='border-bottom:1px solid " + showcolor(TableBorder.Value) + ";font-size:" + tableItemRowfontsize.Value + "pt;color:" + showcolor(tableItemRowFontColor.Value) + ";'>";
                }
                prvdata += "<td style='text-align: right;' colspan='2'><b>Total</b></td>";

                prvdata += "<td style='text-align: right;' colspan='2'>" + fns.ToFormattedCurrencyString(totaligst.ToString()) + "</td>";

                prvdata += "<td style='text-align: right;' colspan='2'>" + fns.ToFormattedCurrencyString(totalsgst.ToString()) + "</td>";

                prvdata += "<td style='text-align: right;' colspan='2'>" + fns.ToFormattedCurrencyString(totalsgst.ToString()) + "</td>";
                prvdata += "</tr>";
                prvdata += "</table>";
            }


            if (Request.Cookies["cadmin"]["id"].ToString() == "50")
            {

                prvdata += "<br/><br/><br/><center><table cellpadding='0' style='width:100%;margin: 0;border-left:0pt;border-right:0pt; border-color:" + showcolor(TableBorder.Value) + " !important;' cellspacing='0' border='0'>";
                prvdata += "<tr><td>&nbsp;&nbsp;</td><td>&nbsp;&nbsp;</td></tr>";

                prvdata += "<tr style='font-size:10pt;'><td><b>_______________________________________</b></td><td><b>________________________________</b></td></tr>";

                prvdata += "<tr style='font-size:10pt;'><td><b>FOR PINK PEPPER CATERING SERVICES LLC</b></td><td><b>RECEIVER’S NAME AND SIGNATURE</b></td></tr>";
                prvdata += "</table></center>";
            }





            prvdatawithoutheader = strdataheader1 + prvdata;

            prvdata = strdataheader1 + strdataheader + prvdata;
            #endregion
            }

        }

    }


    protected void bindprvforpdf()
    {






        string istrn = "1";
        string isvat = "1";
        if (Request.Cookies["cadmin"]["id"].ToString() == "2264")
        {
            istrn = "0";
            isvat = "0";
        }

        string strdataheader = "";
        string strdataheader1 = "";

        SqlParameter[] prmpurch = new SqlParameter[2];
        prmpurch[0] = new SqlParameter("@id", cmf.Decrypt(Request.QueryString["id"].ToString()));
        prmpurch[1] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"].ToString());
        DataSet dsstrsqlpurch = da.GetDataSet("tbl_SalesOrderdetailsselectdetails", prmpurch);


        //string strsqlpurch = "select * from tbl_purchaseorderdetails where id='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "' and status=1 and archive=0 and regid='" + Request.Cookies["cadmin"]["id"].ToString() + "'";
        //DataSet dsstrsqlpurch = da.GetDataSetInline(strsqlpurch);
        if (dsstrsqlpurch.Tables[0].Rows.Count > 0)
        {
            ViewState["isapproved"] = dsstrsqlpurch.Tables[0].Rows[0]["isapprove"].ToString();
            ViewState["branchid"] = dsstrsqlpurch.Tables[0].Rows[0]["branchid"].ToString();
            ViewState["customermobile"] = fns.getmobilenoofcustomer(dsstrsqlpurch.Tables[0].Rows[0]["cusid"].ToString());
            ViewState["invoiceid"] = cmf.Decrypt(Request.QueryString["id"].ToString());
            //try
            //    {
            //        if (dsstrsqlpurch.Tables[0].Rows[0]["isproduction"].ToString() == "1")
            //        {

            //            pnlconvertproduction.Visible = false;
            //            pnlinvoice.Visible = true;

            //            //Response.Write(dsstrsqlpurch.Tables[0].Rows[0]["isproduction"].ToString());
            //        }
            //        else
            //        {
            //            pnlconvertproduction.Visible = true;
            //            pnlinvoice.Visible = false;
            //        }
            //    }
            //    catch (Exception ex)
            //    {

            //    }

            try
            {
                if (ViewState["isappravelreqired"].ToString() == "1")
                {
                    //Response.Write("ok2");
                    if (dsstrsqlpurch.Tables[0].Rows[0]["isapprove"].ToString() == "1")
                    {
                       
                        pnlinvoice.Visible = true;
                       
                    }
                    else
                    {
                        pnlinvoice.Visible = false;
                    }

                }


            }
            catch (Exception ex)
            {

            }


            ViewState["pono"] = dsstrsqlpurch.Tables[0].Rows[0]["Estimateno"].ToString();
            attfilesdata += "";



            prvdatawithoutheader = "";
            prvdataforpdf = "";

            string clogo = "";
            SqlParameter[] prm = new SqlParameter[1];
            prm[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"].ToString());
            DataSet ds = da.GetDataSet("tbl_companydetails_selectbyregid", prm);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["id"] = ds.Tables[0].Rows[0]["id"].ToString();
                ViewState["clogo"] = ds.Tables[0].Rows[0]["clogo"].ToString();
                ViewState["txtnameofcompany"] = ds.Tables[0].Rows[0]["txtnameofcompany"].ToString();
                ViewState["vatrnno"] = ds.Tables[0].Rows[0]["vatrnno"].ToString();
                try
                {
                    if (showbranch == "")
                    {
                        ViewState["vatrnno"] = fns.getgstnbybranchid(ViewState["branchid"].ToString());

                    }

                }
                catch
                {

                }

                ViewState["txtaddress1"] = ds.Tables[0].Rows[0]["txtaddress1"].ToString() + " &nbsp;";
                ViewState["txtaddress2"] = ds.Tables[0].Rows[0]["txtaddress2"].ToString() + " &nbsp;";
                ViewState["txtCountry"] = ds.Tables[0].Rows[0]["txtCountry"].ToString() + " &nbsp;";
                ViewState["txtcity"] = ds.Tables[0].Rows[0]["txtcity"].ToString() + " &nbsp;";
                ViewState["txtzipcode"] = ds.Tables[0].Rows[0]["txtzipcode"].ToString() + " &nbsp;";
                ViewState["txtphoneno"] = ds.Tables[0].Rows[0]["txtphoneno"].ToString() + " &nbsp;";
                ViewState["txtfax"] = ds.Tables[0].Rows[0]["txtfax"].ToString() + " &nbsp;";
                ViewState["txtwebsite"] = ds.Tables[0].Rows[0]["txtwebsite"].ToString() + " &nbsp;";
                ViewState["txtemailcontact"] = ds.Tables[0].Rows[0]["txtemailcontact"].ToString() + " &nbsp;";
                if (ViewState["clogo"] != string.Empty)
                {
                    if (Request.Cookies["cadmin"]["id"] == "21")
                    {

                        clogo = "<img src='" + ConfigurationManager.AppSettings["domainname"].ToString() + "/clogo/" + ViewState["clogo"] + "' height='100px' />";
                    }
                    else if (Request.Cookies["cadmin"]["id"] == "23")
                    {

                        clogo = "<img src='" + ConfigurationManager.AppSettings["domainname"].ToString() + "/clogo/" + ViewState["clogo"] + "'  />";
                    }
                    else
                    {
                        clogo = "<img src='" + ConfigurationManager.AppSettings["domainname"].ToString() + "/clogo/" + ViewState["clogo"] + "'  height='50px'  class='clogo' />";

                    }
                }


                ViewState["orgaddress"] = fns.getorganitaionaddress(Request.Cookies["cadmin"]["id"].ToString());


                try
                {
                    if (showbranch == "")
                    {
                        ViewState["orgaddress"] = fns.getaddressbybranchid(ViewState["branchid"].ToString());

                    }

                }
                catch
                {

                }



                //  txtnameofcompany.Text = ds.Tables[0].Rows[0]["txtnameofcompany"].ToString();

            }



            string DocumentTitlevalue = "";
            if (isdocumenttitle.Checked)
            {
                DocumentTitlevalue = DocumentTitle.Value;
            }
            string BillTovalue = "";
            BillTovalue = BillTo.Value;
            string backgroundimg = "";
            string backgroundimgsignature = "";
            string backgroundimgpotion = imagepotion.SelectedValue;
            if (ViewState["backgroundimage"] != string.Empty)
            {
                backgroundimg = "/uploaddatafile/" + ViewState["backgroundimage"];
            }
            if (ViewState["backgroundimagesignature"] != string.Empty)
            {

                backgroundimgsignature = "/uploaddatafile/" + ViewState["backgroundimagesignature"];
            }

            string backgroundimg1 = "";
            string backgroundimgpotion1 = headerbackgroundimagepotion.SelectedValue;
            //if (ViewState["headerbackgroundimage"] != string.Empty)
            //{
            //   backgroundimg1 = "/uploaddatafile/" + ViewState["headerbackgroundimage"];

            //   // prvdataforpdf = "<div  style='height:50px'>";
            //   prvdataforpdf = "<div  style='background: url(\"" + backgroundimg1 + "\") " + backgroundimgpotion1 + " no-repeat;height:50px'>";
            //    prvdataforpdf += "</div>";
            //}
            // prvdataforpdf += "<div style='margin:" + convertinctopx(MarginsTop.Value) + " " + convertinctopx(MarginsRight.Value) + " " + convertinctopx(MarginsBottom.Value) + " " + convertinctopx(MarginsLeft.Value) + " ;background-color:" + showcolor(BackgroundColor.Value) + ";color:" + getfontcolor() + ";font-size:" + txtFontSize.Value + "pt;border:1px solid #000;'>";




            if (ViewState["headerbackgroundimage"] != string.Empty)
            {

                //string headerimage = "<img src='" + ConfigurationManager.AppSettings["domainname"].ToString() + "/uploaddatafile/" + ViewState["headerbackgroundimage"] + "' style='width:100%;'   />";
                //prvdataforpdf += "<table style='width:100%;table-layout: fixed; margin:0px;'>";
                //prvdataforpdf += "<tbody>";
                //prvdataforpdf += "<tr>";
                //prvdataforpdf += "<td style='vertical-align: top;  border-bottom:1px solid; border-left:0px; ; line-height:108%;' align='" + backgroundimgpotion1 + "'>";
                ////backgroundimg1 = "/uploaddatafile/" + ViewState["headerbackgroundimage"];
                //prvdataforpdf += "<span>" + headerimage + "</span>";
                ////prvdataforpdf = "<div  style='background: url(\"" + backgroundimg1 + "\") " + backgroundimgpotion1 + " no-repeat;height:50px'>";
                ////prvdataforpdf += "</div>";

                //prvdataforpdf += " </td>";
                //prvdataforpdf += " </tr>";
                //prvdataforpdf += " </tbody>";
                //prvdataforpdf += " </table>";
            }
            else
            {
                strdataheader += "<table cellpadding='5' style='width:100%;table-layout: fixed; margin:0pt; border:0;' border='0'>";
                strdataheader += "<tbody>";
                strdataheader += "<tr>";
                if (ShowOrganizationLogo.Checked)
                {
                    strdataheader += "<td   style='vertical-align: middle; border-bottom:1px solid; border-right:0pt;   text-align:left;' border='0'>";

                    strdataheader += "<span>" + clogo + "</span>";
                    strdataheader += "</td>";
                }
                strdataheader += "<td style='vertical-align: top;  border-bottom:1px solid; border-left:0pt; text-align:left; line-height:130%;' colspan='2'>";
                if (ShowOrganizationName.Checked)
                {
                    strdataheader += "<span style='color:" + getcomapnatnamefontcolor() + ";font-size:" + OrganizationNamefont.Value + "pt;'><b>" + ViewState["txtnameofcompany"] + "</b></span><br>";
                }
                if (ShowOrganizationAddress.Checked)
                {
                    strdataheader += "" + ViewState["orgaddress"].ToString() + " </td>";
                }

                strdataheader += "</tr>";
                strdataheader += "<tr>";
                strdataheader += "<td style='vertical-align: middle; border-bottom:0px solid; border-right:0pt; text-align:left;padding:0;'   colspan='3' border='1'></td></tr>";

                strdataheader += "</tr>";
                strdataheader += "</tbody>";
                strdataheader += "</table>";
            }

            strdataheader1 += "<table cellpadding='0' border='1' valign='top' style='background-color:" + showcolor(BackgroundColor.Value) + ";color:" + getfontcolor() + ";font-size:" + txtFontSize.Value + "pt;border:0px solid #000; background: url(\"" + backgroundimg + "\") " + backgroundimgpotion + " no-repeat;'>";
            strdataheader1 += "<tr><td style='padding:0pt;' >";







            prvdataforpdf += "<table cellpadding='5' style='clear:both;width:100%;margin:0pt;'>";
            prvdataforpdf += "<tbody>";

            prvdataforpdf += "<tr>";

            prvdataforpdf += "<td style='vertical-align:bottom;width: 50%; padding-top:0pt;' align='left' border='0'>";
            prvdataforpdf += "<table cellpadding='5' style='float:right;width: 100%;line-height: 112%;' cellspacing='0' cellpadding='0' border='0'>";
            prvdataforpdf += "<tbody>";
            prvdataforpdf += "<tr>";


            prvdataforpdf += "<td style='padding:0pt; width:35%;' ><b><span style='color:" + getlabelcolor() + "'>" + NumberField.Value + "</span></b></td>";
            prvdataforpdf += "<td style='padding:0pt;'> : <span>" + dsstrsqlpurch.Tables[0].Rows[0]["Estimateno"].ToString() + "</span></td>";
            prvdataforpdf += "</tr>";



            if (isDateField.Checked)
            {
                prvdataforpdf += "<tr>";
                prvdataforpdf += "<td style='padding:0pt;' ><b><span style='color:" + getlabelcolor() + "'>" + DateField.Value + "</span></b></td>";
                prvdataforpdf += "<td style='padding:0pt;' > : <span> " + Convert.ToDateTime(dsstrsqlpurch.Tables[0].Rows[0]["EstimateDate"].ToString()).ToString(fns.getdateformat()) + "</span></td>";
                prvdataforpdf += "</tr>";
            }
            if (isExpiryDate.Checked)
            {
                prvdataforpdf += "<tr>";
                prvdataforpdf += "<td style='padding:0pt;' ><b><span style='color:" + getlabelcolor() + "'>" + ExpiryDate.Value + "</span></b></td>";
                if (dsstrsqlpurch.Tables[0].Rows[0]["ExpiryDate"].ToString() != string.Empty)
                {
                    prvdataforpdf += "<td style='padding:0pt;' > : <span>" + Convert.ToDateTime(dsstrsqlpurch.Tables[0].Rows[0]["ExpiryDate"].ToString()).ToString(fns.getdateformat()) + "</span></td>";
                }
                else
                {
                    prvdataforpdf += "<td style='padding:0pt;' > : <span></span></td>";

                }
                prvdataforpdf += "</tr>";
            }

            if (isReferenceField.Checked)
            {
                prvdataforpdf += "<tr>";
                prvdataforpdf += "<td style='padding:0pt;' ><b><span style='color:" + getlabelcolor() + "'>" + ReferenceField.Value + "</span></b></td>";
                prvdataforpdf += "<td style='padding:0pt;'> : <span>" + dsstrsqlpurch.Tables[0].Rows[0]["Reference"].ToString() + "</span></td>";
                prvdataforpdf += "</tr>";

            }
            if (Request.Cookies["cadmin"]["id"].ToString() == "2322")
            {
                prvdataforpdf += "<tr>";
                prvdataforpdf += "<td style='padding:0pt;' ><b><span style='color:" + getlabelcolor() + "'>Customer PO No</span></b></td>";
                prvdataforpdf += "<td style='padding:0pt;'> : <span>" + dsstrsqlpurch.Tables[0].Rows[0]["apono"].ToString() + "</span></td>";
                prvdataforpdf += "</tr>";
                prvdataforpdf += "<tr>";
                prvdataforpdf += "<td style='padding:0pt;' ><b><span style='color:" + getlabelcolor() + "'>Customer PO date</span></b></td>";
                if (dsstrsqlpurch.Tables[0].Rows[0]["apodate"].ToString() != String.Empty)
                {
                    prvdataforpdf += "<td style='padding:0pt;'> : <span>" + Convert.ToDateTime(dsstrsqlpurch.Tables[0].Rows[0]["apodate"].ToString()).ToString(fns.getdateformat()) + "</span></td>";
                }
                else
                {
                    prvdataforpdf += "<td style='padding:0pt;'> : <span></span></td>";
                }
                prvdataforpdf += "</tr>";


                prvdataforpdf += "<tr>";
                prvdataforpdf += "<td style='padding:0pt;' ><b><span style='color:" + getlabelcolor() + "'>Payment Terms</span></b></td>";
                prvdataforpdf += "<td style='padding:0pt;'> : <span>" + dsstrsqlpurch.Tables[0].Rows[0]["apterms"].ToString() + "</span></td>";
                prvdataforpdf += "</tr>";


                prvdataforpdf += "<tr>";
                prvdataforpdf += "<td style='padding:0pt;' ><b><span style='color:" + getlabelcolor() + "'>Dispatch Through</span></b></td>";
                prvdataforpdf += "<td style='padding:0pt;'> : <span>" + dsstrsqlpurch.Tables[0].Rows[0]["adispatchtrough"].ToString() + "</span></td>";
                prvdataforpdf += "</tr>";


            }
            prvdataforpdf += bindcusdataheaderforpdf();
            prvdataforpdf += "</tbody>";
            prvdataforpdf += "</table>";
            prvdataforpdf += "</td>";
            prvdataforpdf += "<td style='width:50%;vertical-align:top; padding-top:0pt;' border='0'>";
            prvdataforpdf += "<table cellpadding='5' style='float:right;width: 100%; line-height: 112%;' cellspacing='0' cellpadding='0' border='0'>";
            prvdataforpdf += "<tbody>";
            prvdataforpdf += "<tr>";
            prvdataforpdf += "<td style='text-align:right; vertical-align: middle; border-bottom:0px solid;' ><b><span style='color:" + showcolor(DocumentTitleFontColor.Value) + ";font-size:" + DocumentTitleFontSize.Value + "pt;'>" + DocumentTitlevalue + "</span></b><br>";

            prvdataforpdf += "</td>";
            prvdataforpdf += "</tr>";

            if (isSalesperson.Checked)
            {


                prvdataforpdf += " <tr>";
                prvdataforpdf += "  <td style='padding:0pt;' ><b><span style='color:" + getlabelcolor() + "'>" + Salesperson.Value + "</span></b></td>";
                prvdataforpdf += "   <td style='padding:0pt;'> : <span>" + dsstrsqlpurch.Tables[0].Rows[0]["salespersonename"].ToString() + "</span></td>";
                prvdataforpdf += "  </tr>";
            }
            prvdataforpdf += "  </tbody>";
            prvdataforpdf += "   </table>";

            prvdataforpdf += "</td>";


            prvdataforpdf += " </tr>";
            prvdataforpdf += "<tr>";
            prvdataforpdf += "<td  colspan='2'  border='1' style='padding: 0;'></td></tr>";



            prvdataforpdf += "<tr style='background-color: #f2f3f4;' bgcolor='" + showcolor(tableheaderBackgroundColor.Value) + "'>";
            prvdataforpdf += "<td style='width:50%;vertical-align:top; border-top:1px solid; border-right:0px solid;line-height: 112%;'  border='0'>";
            if (isBillTo.Checked)
            {

                prvdataforpdf += "<b><span style='color:" + getlabelcolor() + "'>" + BillTovalue + "</span></b>";

            }


            prvdataforpdf += "</td>";
            prvdataforpdf += "<td style='width:50%;vertical-align:top;border-top:1px solid;line-height: 112%;'  border='0'>";

            if (isShipTo.Checked)
            {
                prvdataforpdf += "<b><span style='color:" + getlabelcolor() + "'>" + ShipTo.Value + "</span></b>";


            }
            prvdataforpdf += "</td></tr>";







            prvdataforpdf += "<tr>";
            prvdataforpdf += "<td style='width:50%;vertical-align:top; border-top:1px solid; border-right:0px solid;line-height: 112%;' border='0'>";

            if (isBillTo.Checked)
            {



                prvdataforpdf += "<span style='color:" + showcolor(customerfontcolor.Value) + ";font-size:" + customerfontsize.Value + "pt;'><b>" + dsstrsqlpurch.Tables[0].Rows[0]["txtcontactdisplayname"].ToString() + "</b></span><br>";

                string msgstr = dsstrsqlpurch.Tables[0].Rows[0]["billAttention"].ToString();
                if (dsstrsqlpurch.Tables[0].Rows[0]["billAddress1"].ToString() != string.Empty)
                {
                    if (msgstr == string.Empty)
                        msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billAddress1"].ToString();
                    else
                        msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billAddress1"].ToString();
                }
                if (dsstrsqlpurch.Tables[0].Rows[0]["billAddress2"].ToString() != string.Empty)
                {
                    if (msgstr == string.Empty)
                        msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billAddress2"].ToString();
                    else
                        msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billAddress2"].ToString();
                }
                if (dsstrsqlpurch.Tables[0].Rows[0]["billCity"].ToString() != string.Empty)
                {
                    if (msgstr == string.Empty)
                        msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billCity"].ToString();
                    else
                        msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billCity"].ToString();
                }
                if (dsstrsqlpurch.Tables[0].Rows[0]["billState"].ToString() != string.Empty)
                {
                    if (msgstr == string.Empty)
                        msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billState"].ToString();
                    else
                        msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billState"].ToString();
                }
                if (dsstrsqlpurch.Tables[0].Rows[0]["billZipCode"].ToString() != string.Empty)
                {
                    if (msgstr == string.Empty)
                        msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billZipCode"].ToString();
                    else
                        msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billZipCode"].ToString();
                }
                if (dsstrsqlpurch.Tables[0].Rows[0]["billCountry"].ToString() != string.Empty)
                {
                    if (msgstr == string.Empty)
                        msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billCountry"].ToString();
                    else
                        msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billCountry"].ToString();
                }

                if (dsstrsqlpurch.Tables[0].Rows[0]["billPhone"].ToString() != string.Empty)
                {
                    if (msgstr == string.Empty)
                        msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billPhone"].ToString();
                    else
                        msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billPhone"].ToString();
                }

                if (showisindia == "1")
                {
                    if (dsstrsqlpurch.Tables[0].Rows[0]["TaxRegistrationNumber"].ToString() != string.Empty)
                        msgstr += " <br/><b>GSTIN : " + dsstrsqlpurch.Tables[0].Rows[0]["TaxRegistrationNumber"].ToString() + "</b>";

                }
                else
                {
                    if (dsstrsqlpurch.Tables[0].Rows[0]["TaxRegistrationNumber"].ToString() != string.Empty)
                        msgstr += " <br/><b>TRN : " + dsstrsqlpurch.Tables[0].Rows[0]["TaxRegistrationNumber"].ToString() + "</b>";
                }
                if (Request.Cookies["cadmin"]["id"].ToString() == "282")
                {
                    string strsqldata = "select * from tbl_customercustomefieldvalue where uid='" + dsstrsqlpurch.Tables[0].Rows[0]["cusid"].ToString() + "' and controlid='515' and status=1 and archive=0";
                    DataSet dsc = da.GetDataSetInline(strsqldata);
                    if (dsc.Tables[0].Rows.Count > 0)
                    {

                        msgstr += " <br/><b>PAN CARD NO. : " + dsc.Tables[0].Rows[0]["controlvalue"].ToString() + "</b>";


                    }
                }


                prvdataforpdf += msgstr;

                prvdataforpdf += " </span>";

            }
            prvdataforpdf += "</td>";
            prvdataforpdf += "<td style='width:50%;vertical-align:top;border-top:1px solid;line-height: 112%;' border='0'>";

            if (isShipTo.Checked)
            {
                string msgstr = "";
                if (dsstrsqlpurch.Tables[0].Rows[0]["billAddress11"].ToString() != string.Empty)
                {
                    if (msgstr == string.Empty)
                        msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billAddress11"].ToString();
                    else
                        msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billAddress11"].ToString();
                }
                if (dsstrsqlpurch.Tables[0].Rows[0]["billAddress21"].ToString() != string.Empty)
                {
                    if (msgstr == string.Empty)
                        msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billAddress21"].ToString();
                    else
                        msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billAddress21"].ToString();
                }
                if (dsstrsqlpurch.Tables[0].Rows[0]["billCity1"].ToString() != string.Empty)
                {
                    if (msgstr == string.Empty)
                        msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billCity1"].ToString();
                    else
                        msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billCity1"].ToString();
                }
                if (dsstrsqlpurch.Tables[0].Rows[0]["billState1"].ToString() != string.Empty)
                {
                    if (msgstr == string.Empty)
                        msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billState1"].ToString();
                    else
                        msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billState1"].ToString();
                }
                if (dsstrsqlpurch.Tables[0].Rows[0]["billZipCode1"].ToString() != string.Empty)
                {
                    if (msgstr == string.Empty)
                        msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billZipCode1"].ToString();
                    else
                        msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billZipCode1"].ToString();
                }
                if (dsstrsqlpurch.Tables[0].Rows[0]["billCountry1"].ToString() != string.Empty)
                {
                    if (msgstr == string.Empty)
                        msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billCountry1"].ToString();
                    else
                        msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billCountry1"].ToString();
                }

                if (dsstrsqlpurch.Tables[0].Rows[0]["billPhone1"].ToString() != string.Empty)
                {
                    if (msgstr == string.Empty)
                        msgstr += dsstrsqlpurch.Tables[0].Rows[0]["billPhone1"].ToString();
                    else
                        msgstr += "<br/>" + dsstrsqlpurch.Tables[0].Rows[0]["billPhone1"].ToString();
                }

                prvdataforpdf += msgstr;
                //prvdataforpdf += "   4141 Hacienda Drive <br>";
                //prvdataforpdf += "  Pleasanton <br>";
                //prvdataforpdf += "   94588 CA <br>";
                prvdataforpdf += "</span>";

            }
            prvdataforpdf += "</td>";
            prvdataforpdf += " </tr>";
            prvdataforpdf += "  </tbody>";
            prvdataforpdf += "  </table>";
            decimal totalquantity = 0;


            prvdataforpdf += bindcusdatainvoice();
            //string strsqlitem = "select *,tbl_itemsdetails.pname,tbl_itemsdetails.Unitval from tbl_Quotationorderdetailsitemsdetails left outer join tbl_itemsdetails on tbl_Quotationorderdetailsitemsdetails.itemid=tbl_itemsdetails.id where tbl_Quotationorderdetailsitemsdetails.estmid='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "' and tbl_Quotationorderdetailsitemsdetails.status=1 and tbl_Quotationorderdetailsitemsdetails.archive=0  order by tbl_Quotationorderdetailsitemsdetails.id ";
            //DataSet dsadd = da.GetDataSetInline(strsqlitem);

            ViewState["withdiscount"] = dsstrsqlpurch.Tables[0].Rows[0]["withdiscount"].ToString();
            ViewState["enableGrossWeight"] = dsstrsqlpurch.Tables[0].Rows[0]["enableGrossWeight"].ToString();

            int isextracolumn = 0;
            string strsqlPreferences = "select * from tbl_tbl_Preferencessalesinvoices where regid='" + Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0";

            DataSet dsPreferences = da.GetDataSetInline(strsqlPreferences);
            if (dsPreferences.Tables[0].Rows.Count > 0)
            {

                if (Request.Cookies["cadmin"]["id"].ToString() == "157")
                {
                    try
                    {
                        if (dsPreferences.Tables[0].Rows[0]["noofextrafiled4"].ToString() != string.Empty)
                        {
                            isextracolumn = Convert.ToInt32(dsPreferences.Tables[0].Rows[0]["noofextrafiled4"].ToString());

                            // Response.Write(ViewState["extrafield"].ToString());

                        }
                        else
                        {
                            isextracolumn = 0;
                        }


                        string extfield1 = dsPreferences.Tables[0].Rows[0]["extfield25"].ToString();
                        ViewState["extfield1"] = extfield1;
                        string extfield2 = dsPreferences.Tables[0].Rows[0]["extfield26"].ToString();
                        ViewState["extfield2"] = extfield2;
                        string extfield3 = dsPreferences.Tables[0].Rows[0]["extfield27"].ToString();
                        ViewState["extfield3"] = extfield3;
                        string extfield4 = dsPreferences.Tables[0].Rows[0]["extfield28"].ToString();
                        ViewState["extfield4"] = extfield4;
                        string extfield5 = dsPreferences.Tables[0].Rows[0]["extfield29"].ToString();
                        ViewState["extfield5"] = extfield5;
                        string extfield6 = dsPreferences.Tables[0].Rows[0]["extfield29"].ToString();
                        ViewState["extfield6"] = extfield6;

                    }

                    catch (Exception ex)
                    {

                    }
                }
                else
                {



                    if (dsPreferences.Tables[0].Rows[0]["noofextrafiled"].ToString() != string.Empty)
                    {
                        try
                        {
                            isextracolumn = Convert.ToInt32(dsPreferences.Tables[0].Rows[0]["noofextrafiled"].ToString());

                            string extfield1 = dsPreferences.Tables[0].Rows[0]["extfield1"].ToString();
                            ViewState["extfield1"] = extfield1;
                            string extfield2 = dsPreferences.Tables[0].Rows[0]["extfield2"].ToString();
                            ViewState["extfield2"] = extfield2;
                            string extfield3 = dsPreferences.Tables[0].Rows[0]["extfield3"].ToString();
                            ViewState["extfield3"] = extfield3;
                            string extfield4 = dsPreferences.Tables[0].Rows[0]["extfield4"].ToString();
                            ViewState["extfield4"] = extfield4;
                            string extfield5 = dsPreferences.Tables[0].Rows[0]["extfield5"].ToString();
                            ViewState["extfield5"] = extfield5;
                            string extfield6 = dsPreferences.Tables[0].Rows[0]["extfield6"].ToString();
                            ViewState["extfield6"] = extfield6;


                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else
                    {
                        isextracolumn = 0;
                    }
                }

            }




            string strsqlitem = "select *,tbl_itemsdetails.pname,tbl_itemsdetails.Unitval from tbl_SalesOrderdetailsitemsdetails left outer join tbl_itemsdetails on tbl_SalesOrderdetailsitemsdetails.itemid=tbl_itemsdetails.id where tbl_SalesOrderdetailsitemsdetails.estmid='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "' and tbl_SalesOrderdetailsitemsdetails.status=1 and tbl_SalesOrderdetailsitemsdetails.archive=0  order by tbl_SalesOrderdetailsitemsdetails.id ";
            DataSet dsadd = da.GetDataSetInline(strsqlitem);

            if (dsadd.Tables[0].Rows.Count > 0)
            {
                ViewState["CurrentTable"] = dsadd.Tables[0];








                string tblborderval = "0";
                if (TableBorder1.Checked)
                {
                    tblborderval = "1";
                }

                prvdataforpdf += "   <table cellpadding='5' style='width:100%;margin: 0;border-left:0pt;border-right:0pt; border-color:" + showcolor(TableBorder.Value) + " !important;' cellspacing='0' border='" + tblborderval + "'>";
                prvdataforpdf += " <thead>";
                if (tableheaderBackgroundColor1.Checked)
                {
                    prvdataforpdf += " <tr bgcolor='" + showcolor(tableheaderBackgroundColor.Value) + "' style='height:32pt;border-left:0pt;border-right:0pt; background:" + showcolor(tableheaderBackgroundColor.Value) + ";font-size:" + headerFontSize.Value + "pt;color:" + showcolor(tableheaderFontColor.Value) + ";'>";
                }
                else
                {
                    prvdataforpdf += " <tr style='height:32pt;border-left:0pt;border-right:0pt; font-size:" + headerFontSize.Value + "pt;color:" + showcolor(tableheaderFontColor.Value) + ";'>";
                }
                if (SerialNumber1.Checked)
                {

                    prvdataforpdf += " <td style='padding: 5px 0px 5px 5pt;border-left:0pt;width: 5%;text-align: center;'><b>" + SerialNumber.Value + "</b></td>";
                }

                prvdataforpdf += "  <td style='padding: 5px 10px 5px 10px;width: 30%;text-align: left;' colspan='6'><b>";



                if (Item1.Checked)
                {
                    prvdataforpdf += Item.Value + "";
                }
                if (Description1.Checked)
                {
                    if (Item1.Checked)
                    {
                        if (Item.Value != string.Empty)
                        {
                            if (Description.Value != string.Empty)
                            {
                                prvdataforpdf += " &amp; " + Description.Value;
                            }

                        }
                        else
                        {
                            prvdataforpdf += Description.Value;
                        }
                    }
                    else
                    {
                        prvdataforpdf += Description.Value;
                    }


                }
                prvdataforpdf += "</b></td>";

                if (iscoslight == 1)
                {
                    prvdata += "<td style='padding: 5px 10px 5px 5pt;width: 6%;text-align: right;' colspan='2'><b>Part No.</b></td>";
                }
                if (isextracolumn > 0)
                {
                    prvdataforpdf += "<td style='padding: 5px 10px 5px 5pt;width: 6%;text-align: right;' colspan='2'><b>" + ViewState["extfield1"] + "</b></td>";


                }
                if (isextracolumn > 1)
                {
                    prvdataforpdf += "<td style='padding: 5px 10px 5px 5pt;width: 6%;text-align: right;' colspan='2'><b>" + ViewState["extfield2"] + "</b></td>";


                }
                if (isextracolumn > 2)
                {
                    prvdataforpdf += "<td style='padding: 5px 10px 5px 5pt;width: 6%;text-align: right;' colspan='2'><b>" + ViewState["extfield3"] + "</b></td>";


                }
                if (isextracolumn > 3)
                {
                    prvdataforpdf += "<td style='padding: 5px 10px 5px 5pt;width: 6%;text-align: right;' colspan='2'><b>" + ViewState["extfield4"] + "</b></td>";


                }
                if (isextracolumn > 4)
                {
                    prvdataforpdf += "<td style='padding: 5px 10px 5px 5pt;width: 6%;text-align: right;' colspan='2'><b>" + ViewState["extfield5"] + "</b></td>";


                }
                if (isextracolumn > 5)
                {
                    prvdataforpdf += "<td style='padding: 5px 10px 5px 5pt;width: 6%;text-align: right;' colspan='2'><b>" + ViewState["extfield6"] + "</b></td>";


                }


                if (ViewState["enableGrossWeight"].ToString() == "1")
                {
                    prvdataforpdf += "<td style='padding: 5px 10px 5px 5pt;width: 6%;text-align: right;' colspan='2'><b>Quantity(Gross)</b></td>";
                    prvdataforpdf += "<td style='padding: 5px 10px 5px 5pt;width: 6%;text-align: right;' colspan='2'><b>Quantity(Net)</b></td>";
                    //if (Quantity1.Checked)
                    //{
                    //    prvdataforpdf += "<td style='padding: 5px 10px 5px 5pt;width: 6%;text-align: right;' colspan='2'><b>" + Quantity.Value + "</b></td>";
                    //}
                }
                else
                {
                    if (Quantity1.Checked)
                    {
                        prvdataforpdf += "<td style='padding: 5px 10px 5px 5pt;width: 6%;text-align: right;' colspan='2'><b>" + Quantity.Value + "</b></td>";
                    }
                }



                if (Rate1.Checked)
                {
                    if (showrental == "1")
                    {
                        prvdataforpdf += "<td style='padding: 5px 10px 5px 5pt;width: 6%;text-align: right;'  colspan='2'><b>Duration</b></td>";
                        prvdataforpdf += "<td style='padding: 5px 10px 5px 5pt;width: 6%;text-align: right;'  colspan='2'><b>" + ratetext + "</b></td>";
                    }
                    else
                    {
                        prvdataforpdf += "<td style='padding: 5px 10px 5px 5pt;width: 6%;text-align: right;'  colspan='2'><b>" + Rate.Value + "</b></td>";
                    }
                }

                if (ViewState["withdiscount"].ToString() == "2")
                {

                    prvdataforpdf += "<td style='padding: 5px 10px 5px 5pt;width: 6%;text-align: right;' colspan='2'><b>Discount</b></td>";

                }



                if (Discount1.Checked)
                {
                    prvdataforpdf += "<td style='padding: 5px 10px 5px 5pt;width: 6%;text-align: right;'><b>" + Discount.Value + "</b></td>";
                }
                if (showisindia == "0")
                {
                    if (showrental == "0")
                    {
                        if (Taxpercent1.Checked)
                        {
                            prvdataforpdf += "<td style='padding: 5px 10px 5px 5pt;width: 6%;text-align: right;' ><b>" + Taxpercent.Value + "</b></td>";
                        }

                        if (TaxAmount1.Checked)
                        {
                            prvdataforpdf += "<td style='padding: 5px 10px 5px 5pt;border-right:0pt;width: 8%;text-align: right;border-right:0pt; ' colspan='2'><b>" + TaxAmount.Value + "</b></td>";
                        }
                    }
                }
                if (Amount1.Checked)
                {
                    prvdataforpdf += "    <td style='padding: 5px 10px 5px 5pt;border-right:0pt;width: 8%;text-align: right; ' colspan='3'><b>" + Amount.Value + " </b></td>";

                    //prvdataforpdf += "    <td style='padding: 5px 10px 5px 5pt;border-right:0pt;width: 8%;text-align: right; ' colspan='3'>sd<b>" + fns.ToFormattedCurrencyString(Convert.ToDecimal(Amount.Value.Trim())) + " </b></td>";


                }
                prvdataforpdf += "   </tr>";
                prvdataforpdf += " </thead>";
                prvdataforpdf += "  <tbody>";







                for (int jj = 0; jj < dsadd.Tables[0].Rows.Count; jj++)
                {


                    if (tableItemRowBackgroundColor1.Checked)
                    {
                        prvdataforpdf += " <tr style='border-bottom:1px solid " + showcolor(TableBorder.Value) + "; background:" + showcolor(tableItemRowBackgroundColor.Value) + ";font-size:" + tableItemRowfontsize.Value + "pt;color:" + showcolor(tableItemRowFontColor.Value) + ";'>";
                    }
                    else
                    {
                        prvdataforpdf += " <tr style='border-bottom:1px solid " + showcolor(TableBorder.Value) + ";font-size:" + tableItemRowfontsize.Value + "pt;color:" + showcolor(tableItemRowFontColor.Value) + ";'>";
                    }



                    if (SerialNumber1.Checked)
                    {
                        prvdataforpdf += " <td style='padding: 10px 0 10px 5px; border-bottom:1px;border-top:0px;text-align: center;text-align: center;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'> " + (jj + 1) + " </td>";
                    }
                    prvdataforpdf += " <td style='padding: 10px 0px 10px 10px;border-bottom:1px;border-top:0px;text-align: left;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='6'>";
                    if (Item1.Checked)
                    {

                        string catsubcatname = "";

                        if (iscatshow == 1)
                        {

                            if (dsadd.Tables[0].Rows[jj]["categoryid"].ToString() != string.Empty)
                            {

                                string strsqlcat = "select * from tbl_itemcategory where id='" + dsadd.Tables[0].Rows[jj]["categoryid"].ToString() + "' and status=1 and archive=0";
                                DataSet dscat = da.GetDataSetInline(strsqlcat);
                                if (dscat.Tables[0].Rows.Count > 0)
                                {
                                    catsubcatname += dscat.Tables[0].Rows[0]["catname"].ToString() + " ";
                                }
                                strsqlcat = "select * from tbl_itemsubcatgory where id='" + dsadd.Tables[0].Rows[jj]["subcategoryid"].ToString() + "' and status=1 and archive=0";
                                dscat = da.GetDataSetInline(strsqlcat);
                                if (dscat.Tables[0].Rows.Count > 0)
                                {
                                    catsubcatname += dscat.Tables[0].Rows[0]["subcatname"].ToString() + " ";
                                }

                            }

                        }



                        prvdataforpdf += "  <span style='font-size:" + tableitemdescFontSize.Value + "pt;color:" + showcolor(tableitemdescFontColor.Value) + ";' >" + catsubcatname + dsadd.Tables[0].Rows[jj]["pname"].ToString() + " ";
                        string strparmraw = "";
                        if (ViewState["iscustomeparm"] != null)
                        {
                            if (ViewState["iscustomeparm"].ToString() == "1")
                            {
                                try
                                {

                                    if (ViewState["iscustomeparm"] != null)
                                    {
                                        if (ViewState["iscustomeparm"].ToString() == "1")
                                        {
                                            // Label lblitemspecification = (Label)gvr.FindControl("lblitemspecification");
                                            SqlParameter[] prmdata = new SqlParameter[3];
                                            prmdata[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"]);
                                            prmdata[1] = new SqlParameter("@searchtxt", "%");
                                            prmdata[2] = new SqlParameter("@pid", "0");
                                            DataSet dsspecification = da.GetDataSet("tbl_masteritemsdetailsselectmanage", prmdata);
                                            int counterspecification = 1;
                                            string controlvalue = "";
                                            for (int x = 0; x < dsspecification.Tables[0].Rows.Count; x++)
                                            {
                                                string controlid = dsspecification.Tables[0].Rows[x]["id"].ToString();


                                                string strsqldata = "select tbl_itemsdetailscustomefieldvaluespecification.*,tbl_masteritemsdetails.groupname from tbl_itemsdetailscustomefieldvaluespecification inner join tbl_masteritemsdetails on tbl_itemsdetailscustomefieldvaluespecification.controlvalue=tbl_masteritemsdetails.id where uid='" + dsadd.Tables[0].Rows[jj]["itemid"].ToString() + "' and tbl_itemsdetailscustomefieldvaluespecification.controlid='" + controlid + "' and tbl_itemsdetailscustomefieldvaluespecification.status=1 and tbl_itemsdetailscustomefieldvaluespecification.archive=0 and tbl_masteritemsdetails.status=1 and tbl_masteritemsdetails.archive=0";
                                                DataSet dsc = da.GetDataSetInline(strsqldata);
                                                if (dsc.Tables[0].Rows.Count > 0)
                                                {
                                                    if (controlvalue != string.Empty)
                                                    {
                                                        controlvalue += "-" + dsc.Tables[0].Rows[0]["groupname"].ToString();
                                                    }
                                                    else
                                                    {
                                                        controlvalue = dsc.Tables[0].Rows[0]["groupname"].ToString();
                                                    }
                                                }
                                            }

                                            strparmraw = controlvalue;
                                        }
                                    }
                                }
                                catch
                                {

                                }

                            }
                        }
                        string stpramval = "";

                        for (int pr = 1; pr < 13; pr++)
                        {
                            string prcol = "prm" + pr;
                            if (dsadd.Tables[0].Rows[jj][prcol].ToString() != "0" && dsadd.Tables[0].Rows[jj][prcol].ToString() != String.Empty)
                            {
                                if (stpramval != string.Empty)
                                {
                                    stpramval += "-" + getparmnamevalue(dsadd.Tables[0].Rows[jj][prcol].ToString());
                                }
                                else
                                {
                                    stpramval += getparmnamevalue(dsadd.Tables[0].Rows[jj][prcol].ToString());
                                }
                            }

                        }

                        prvdataforpdf += strparmraw + " " + stpramval + "</span>";

                    }
                    if (Description1.Checked)
                    {
                        if (dsadd.Tables[0].Rows[jj]["itemdesc"].ToString().Replace("\n", "") != string.Empty)
                        {
                            prvdataforpdf += "<br><span style='font-size:" + tableitemdescFontSize.Value + "pt;color:" + showcolor(tableitemdescFontColor.Value) + ";'>" + dsadd.Tables[0].Rows[jj]["itemdesc"].ToString().Replace("\n", "<br/>") + "</span>";
                        }
                    }
                    prvdataforpdf += " </td>";
                    //try
                    //{
                    //    totalquantity += Convert.ToDecimal(dsadd.Tables[0].Rows[jj]["quantity"].ToString());
                    //}
                    //catch (Exception ex)
                    //{

                    //}



                    if (iscoslight == 1)
                    {
                        prvdata += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='2'><span>" + dsadd.Tables[0].Rows[jj]["ERPCode"].ToString() + "<br/></span></td>";
                    }

                    if (isextracolumn > 0)
                    {
                        prvdataforpdf += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='2'><span>" + dsadd.Tables[0].Rows[jj]["extfield1"].ToString().Replace("\n", "<br/>") + "</span></td>";

                    }
                    if (isextracolumn > 1)
                    {
                        prvdataforpdf += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='2'><span>" + dsadd.Tables[0].Rows[jj]["extfield2"].ToString().Replace("\n", "<br/>") + "</span></td>";

                    }
                    if (isextracolumn > 2)
                    {
                        prvdataforpdf += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='2'><span>" + dsadd.Tables[0].Rows[jj]["extfield3"].ToString().Replace("\n", "<br/>") + "</span></td>";

                    }
                    if (isextracolumn > 3)
                    {
                        prvdataforpdf += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='2'><span>" + dsadd.Tables[0].Rows[jj]["extfield4"].ToString().Replace("\n", "<br/>") + "</span></td>";

                    }
                    if (isextracolumn > 4)
                    {
                        prvdataforpdf += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='2'><span>" + dsadd.Tables[0].Rows[jj]["extfield5"].ToString().Replace("\n", "<br/>") + "</span></td>";

                    }
                    if (isextracolumn > 5)
                    {
                        prvdataforpdf += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='2'><span>" + dsadd.Tables[0].Rows[jj]["extfield6"].ToString().Replace("\n", "<br/>") + "</span></td>";

                    }
                    try
                    {
                        totalquantity += Convert.ToDecimal(dsadd.Tables[0].Rows[jj]["quantity"].ToString());
                    }
                    catch (Exception ex)
                    {

                    }
                    if (ViewState["enableGrossWeight"].ToString() == "1")
                    {


                        //if (Quantity1.Checked)
                        //{


                        if (dsadd.Tables[0].Rows[jj]["Unitval"].ToString() != string.Empty)
                        {
                            prvdataforpdf += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='2'><span>" + dsadd.Tables[0].Rows[jj]["quantityGross"].ToString() + " &nbsp;(" + dsadd.Tables[0].Rows[jj]["Unitval"].ToString() + ")</span></td>";
                        }
                        else
                        {
                            prvdataforpdf += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='2'><span>" + dsadd.Tables[0].Rows[jj]["quantityGross"].ToString() + "&nbsp;</span></td>";
                        }


                        if (dsadd.Tables[0].Rows[jj]["Unitval"].ToString() != string.Empty)
                        {
                            prvdataforpdf += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='2'><span>" + dsadd.Tables[0].Rows[jj]["quantity"].ToString() + "&nbsp;(" + dsadd.Tables[0].Rows[jj]["Unitval"].ToString() + ")</span></td>";
                        }
                        else
                        {
                            prvdataforpdf += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='2'><span>" + dsadd.Tables[0].Rows[jj]["quantity"].ToString() + "&nbsp;</span></td>";
                        }

                        //}
                    }
                    else
                    {
                        if (Quantity1.Checked)
                        {

                            if (dsadd.Tables[0].Rows[jj]["Unitval"].ToString() != string.Empty)
                            {
                                prvdataforpdf += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='2'><span>" + dsadd.Tables[0].Rows[jj]["quantity"].ToString() + "&nbsp;(" + dsadd.Tables[0].Rows[jj]["Unitval"].ToString() + ")</span></td>";
                            }
                            else
                            {
                                prvdataforpdf += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='2'><span>" + dsadd.Tables[0].Rows[jj]["quantity"].ToString() + "&nbsp;</span></td>";
                            }

                        }
                    }



                    if (showrental == "1")
                    {
                        if (Rate1.Checked)
                        {
                            prvdataforpdf += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='2'><span>" + dsadd.Tables[0].Rows[jj]["rtypoeno"].ToString() + "&nbsp;" + dsadd.Tables[0].Rows[jj]["lblnumbertype"].ToString() + " </span></td>";


                            prvdataforpdf += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='2'><span>" + fns.ToFormattedCurrencyString(dsadd.Tables[0].Rows[jj]["rate"].ToString()) + " &nbsp; " + dsadd.Tables[0].Rows[jj]["lblrtype"].ToString() + "</span></td>";
                        }
                    }
                    else
                    {
                        if (Rate1.Checked)
                        {
                            prvdataforpdf += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='2'><span>" + fns.ToFormattedCurrencyString(dsadd.Tables[0].Rows[jj]["rate"].ToString()) + "</span></td>";
                        }
                    }

                    if (ViewState["withdiscount"].ToString() == "2")
                    {

                        prvdataforpdf += "<td colspan='2' style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'>" + fns.ToFormattedCurrencyString(dsadd.Tables[0].Rows[jj]["totaldiscount"].ToString()) + " </td>";

                    }

                    if (Discount1.Checked)
                    {
                        prvdataforpdf += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'>" + dsadd.Tables[0].Rows[jj]["taxtype"].ToString() + " </td>";
                    }
                    if (showisindia == "0")
                    {
                        if (showrental == "0")
                        {
                            if (Taxpercent1.Checked)
                            {
                                prvdataforpdf += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' ><span>" + dsadd.Tables[0].Rows[jj]["taxtvalue"].ToString() + "%</span></td>";
                            }
                            if (TaxAmount1.Checked)
                            {
                                prvdataforpdf += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;border-right:0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='2'><span>" + fns.ToFormattedCurrencyString(dsadd.Tables[0].Rows[jj]["taxamount"].ToString()) + "</span></td>";
                            }
                        }
                    }
                    if (Amount1.Checked)
                    {
                        if (ViewState["withdiscount"].ToString() == "3")
                        {

                            decimal totalamountwithdiscount = 0;

                            totalamountwithdiscount = Convert.ToDecimal(dsadd.Tables[0].Rows[jj]["amount"].ToString());
                            if (dsadd.Tables[0].Rows[jj]["totaldiscount"].ToString() != string.Empty)
                            {
                                totalamountwithdiscount += Convert.ToDecimal(dsadd.Tables[0].Rows[jj]["totaldiscount"].ToString());
                            }



                            prvdataforpdf += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;border-right:0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='3'><span>" + fns.ToFormattedCurrencyString(totalamountwithdiscount.ToString()) + "</span></td>";

                        }
                        else
                        {


                            prvdataforpdf += "<td style='padding: 10px 10px 10px 0pt;border-bottom:1px;border-top:0px;border-right:0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' colspan='3'><span>" + fns.ToFormattedCurrencyString(dsadd.Tables[0].Rows[jj]["amount"].ToString()) + "</span></td>";


                        }


                    }
                    prvdataforpdf += "</tr>";

                }
            }

            prvdataforpdf += "</tbody></table>";
            prvdataforpdf += "<table style='width:100%; margin:0pt;border: 0px;' cellspacing='0' cellpadding='0' border='1'>";

            prvdataforpdf += "<tr><td aling='top' colspan='9' style='border:0px; padding:0px; border-color:" + showcolor(TableBorder.Value) + " !important;' >";


            if (showitemQuantity.Checked || Showamountinwords.Checked)
            {
                prvdataforpdf += "<table style='width:100%; margin:0pt;' cellspacing='0' cellpadding='5' border='0'><tr><td >";


                if (showitemQuantity.Checked)
                {
                    prvdataforpdf += ItemsinTotal.Value + " " + totalquantity.ToString() + "<br/>";

                }
                if (Showamountinwords.Checked)
                {

                    prvdataforpdf += "<b>Amount in Words :</b><br/>";
                    // prvdataforpdf += "" + fns.getnumbertoworld(dsstrsqlpurch.Tables[0].Rows[0]["totalamount"].ToString()) + "";

                    if (fns.bindcurdefaultid(dsstrsqlpurch.Tables[0].Rows[0]["currencyid"].ToString()) == "138")
                    {

                        prvdataforpdf += "" + fns.getnumbertoworldusd(dsstrsqlpurch.Tables[0].Rows[0]["totalamount"].ToString()) + "";

                        //try
                        //{
                        //    string strsql111 = "select tbl_Currenciesdetails.*,tbl_Currenciesmaster.Currency,tbl_Currenciesmaster.curName from tbl_Currenciesdetails inner join tbl_Currenciesmaster on tbl_Currenciesdetails.curid=tbl_Currenciesmaster.id where tbl_Currenciesdetails.regid='" + Request.Cookies["cadmin"]["id"].ToString() + "' and tbl_Currenciesdetails.status=1 and tbl_Currenciesdetails.archive=0 and  isdefault=1";
                        //    DataSet dssss = da.GetDataSetInline(strsql111);
                        //    if (dssss.Tables[0].Rows.Count > 0)
                        //    {
                        //        ViewState["mathround"] = dssss.Tables[0].Rows[0]["decimalpoint"].ToString();

                        //    }
                        //    int rountno = Convert.ToInt32(ViewState["mathround"]);
                        //    decimal totalamountundefault = Math.Round((Convert.ToDecimal(dsstrsqlpurch.Tables[0].Rows[0]["totalamount"].ToString()) * Convert.ToDecimal(dsstrsqlpurch.Tables[0].Rows[0]["convertionrate"].ToString())), rountno);

                        //    ViewState["totalamountundefaul"] = totalamountundefault.ToString();

                        //    string strsql = "select tbl_Currenciesmaster.* from tbl_Currenciesdetails inner join tbl_Currenciesmaster on tbl_Currenciesdetails.curid=tbl_Currenciesmaster.id where tbl_Currenciesdetails.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and tbl_Currenciesdetails.status=1 and tbl_Currenciesdetails.archive=0 and isdefault=1";

                        //    // string strsql = "select * from tbl_Currenciesmaster where id='" + fns.bindcurdefaultid(dsstrsqlpurch.Tables[0].Rows[0]["currencyid"].ToString()) + "'";
                        //    DataSet dscurname = da.GetDataSetInline(strsql);
                        //    if (dscurname.Tables[0].Rows.Count > 0)
                        //    {
                        //        prvdataforpdf += "<br/>" + fns.getnumbertoworldcur(totalamountundefault.ToString(), dscurname.Tables[0].Rows[0]["curworld"].ToString(), dscurname.Tables[0].Rows[0]["curworldpaisa"].ToString()) + "";
                        //    }


                        //    //prvdataforpdf += "<br/>" + fns.getnumbertoworldcur(totalamountundefault.ToString()) + "";
                        //}
                        //catch (Exception ex)
                        //{
                        //}



                    }
                    else
                    {

                        string strsql = "select * from tbl_Currenciesmaster where id='" + fns.bindcurdefaultid(dsstrsqlpurch.Tables[0].Rows[0]["currencyid"].ToString()) + "'";
                        DataSet dscurname = da.GetDataSetInline(strsql);
                        if (dscurname.Tables[0].Rows.Count > 0)
                        {
                            prvdataforpdf += "" + fns.getnumbertoworldcur(dsstrsqlpurch.Tables[0].Rows[0]["totalamount"].ToString(), dscurname.Tables[0].Rows[0]["curworld"].ToString(), dscurname.Tables[0].Rows[0]["curworldpaisa"].ToString()) + "";
                        }

                        // prvdataforpdf += "" + fns.getnumbertoworld(dsstrsqlpurch.Tables[0].Rows[0]["totalamount"].ToString()) + "";
                    }

                }

                prvdataforpdf += "</td></tr></table>";
            }
            if (chkistermsandcondition.Checked || chkisnotes.Checked)
            {
                prvdataforpdf += "<table style='width:100%;margin:15px 0 0 0;' cellspacing='0' cellpadding='5' border='0'>";
                prvdataforpdf += "<tbody>";
                if (chkisnotes.Checked)
                {
                    prvdataforpdf += "<tr>";
                    prvdataforpdf += "<td style='padding:5px 10px 5px 5pt;font-size:11pt;' align='left'><span style='color:" + getlabelcolor() + ";font-size:" + TermsConditionsFontSize.Value + "pt;'><b>" + FooterNotesLabel.Value + "</b></span><br>";
                    prvdataforpdf += "<span style='font-size:" + FooterFontSize.Value + "pt;'>" + dsstrsqlpurch.Tables[0].Rows[0]["CustomerNotes"].ToString().Replace("\n", "<br/>") + "</span></td>";
                    prvdataforpdf += "</tr>";
                }
                if (Request.Cookies["cadmin"]["id"].ToString() != "50")
                {
                    if (chkistermsandcondition.Checked)
                    {
                        prvdataforpdf += "<tr>";
                        prvdataforpdf += "<td style='padding:5px 10px 5px 5pt;' align='left'><span style='color:" + getlabelcolor() + ";font-size:" + TermsConditionsFontSize.Value + "pt;'><b>" + TermsConditionsLabel.Value + "</b></span><br>";
                        prvdataforpdf += "<span style='font-size:" + FooterFontSize.Value + "pt;'>" + dsstrsqlpurch.Tables[0].Rows[0]["TermsandConditions"].ToString().Replace("\n", "<br/>") + "</span></td>";
                        prvdataforpdf += "</tr>";
                    }
                }
                prvdataforpdf += "</tbody>";
                prvdataforpdf += "</table>";
            }

            prvdataforpdf += bindcusdatafooter();
            prvdataforpdf += "</td>";
            if (ShowTotalSection.Checked)
            {
                if (ViewState["withdiscount"].ToString() == "2")
                {
                    if (ViewState["enableGrossWeight"].ToString() == "1")
                    {
                        prvdataforpdf += "<td colspan='11' style='width:40.5%;padding:0px 0px 0px 0pt; border:0px; border-left:1px solid; border-color:" + showcolor(TableBorder.Value) + " !important;'>";
                    }
                    else
                    {
                        prvdataforpdf += "<td colspan='10' style='width:40.5%;padding:0px 0px 0px 0pt; border:0px; border-left:1px solid; border-color:" + showcolor(TableBorder.Value) + " !important;'>";

                    }
                }
                else
                {
                    if (ViewState["enableGrossWeight"].ToString() == "1")
                    {
                        prvdataforpdf += "<td colspan='9' style='width:40.5%;padding:0px 0px 0px 0pt; border:0px; border-left:1px solid; border-color:" + showcolor(TableBorder.Value) + " !important;'>";
                    }
                    else
                    {
                        prvdataforpdf += "<td colspan='8' style='width:40.5%;padding:0px 0px 0px 0pt; border:0px; border-left:1px solid; border-color:" + showcolor(TableBorder.Value) + " !important;'>";

                    }
                }

                string TotalSubtotaltaxBackgroundColorvalue = "";
                prvdataforpdf += "<table border='0' width='100%' cellpadding='0' cellspacing='0'  >";

                //if (ViewState["withdiscount"].ToString() == "2")
                //{
                //    prvdataforpdf += "<tr>";
                //    prvdataforpdf += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;font-size:" + TotalSubtotaltaxFontSize.Value + "pt;color:" + showcolor(TotalSubtotaltaxFontColor.Value) + ";background:" + TotalSubtotaltaxBackgroundColorvalue + ";' valign='top' ><b><span>Total Discount&nbsp;</span></b></td>";
                //    prvdataforpdf += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' ><span>" + fns.ToFormattedCurrencyString(dsstrsqlpurch.Tables[0].Rows[0]["discountamount"].ToString()) + "&nbsp;</span></td>";
                //    prvdataforpdf += "</tr>";

                //}
                if (ViewState["withdiscount"].ToString() == "3")
                {

                    decimal totalsubtotalmain = Convert.ToDecimal(dsstrsqlpurch.Tables[0].Rows[0]["SubTotal"].ToString()) + Convert.ToDecimal(dsstrsqlpurch.Tables[0].Rows[0]["discountamount"].ToString());

                    prvdataforpdf += "<tr>";
                    prvdataforpdf += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;font-size:" + TotalSubtotaltaxFontSize.Value + "pt;color:" + showcolor(TotalSubtotaltaxFontColor.Value) + ";background:" + TotalSubtotaltaxBackgroundColorvalue + ";' valign='top' ><b><span>Sub Total&nbsp;</span></b></td>";
                    prvdataforpdf += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' ><span>" + fns.ToFormattedCurrencyString(totalsubtotalmain.ToString()) + "&nbsp;</span></td>";
                    prvdataforpdf += "</tr>";

                    prvdataforpdf += "<tr>";
                    prvdataforpdf += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;font-size:" + TotalSubtotaltaxFontSize.Value + "pt;color:" + showcolor(TotalSubtotaltaxFontColor.Value) + ";background:" + TotalSubtotaltaxBackgroundColorvalue + ";' valign='top' ><b><span>Total Discount&nbsp;</span></b></td>";
                    prvdataforpdf += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' ><span>" + fns.ToFormattedCurrencyString(dsstrsqlpurch.Tables[0].Rows[0]["discountamount"].ToString()) + "&nbsp;</span></td>";
                    prvdataforpdf += "</tr>";

                }

                if (ViewState["withdiscount"].ToString() == "2")
                {
                    prvdataforpdf += "<tr>";
                    prvdataforpdf += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;font-size:" + TotalSubtotaltaxFontSize.Value + "pt;color:" + showcolor(TotalSubtotaltaxFontColor.Value) + ";background:" + TotalSubtotaltaxBackgroundColorvalue + ";' valign='top' ><b><span>Total Discount&nbsp;</span></b></td>";
                    prvdataforpdf += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' ><span>" + fns.ToFormattedCurrencyString(dsstrsqlpurch.Tables[0].Rows[0]["discountamount"].ToString()) + "&nbsp;</span></td>";
                    prvdataforpdf += "</tr>";

                }
                if (SubTotal1.Checked)
                {
                    prvdataforpdf += " <tr>";


                    if (TotalSubtotaltaxBackgroundColor1.Checked)
                    {
                        TotalSubtotaltaxBackgroundColorvalue = showcolor(TotalSubtotaltaxBackgroundColor.Value);

                    }

                    prvdataforpdf += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;font-size:" + TotalSubtotaltaxFontSize.Value + "pt;color:" + showcolor(TotalSubtotaltaxFontColor.Value) + ";background:" + TotalSubtotaltaxBackgroundColorvalue + ";' valign='top' ><b><span>" + SubTotal.Value + "&nbsp;</span></b></td>";
                    prvdataforpdf += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'><span>" + fns.ToFormattedCurrencyString(dsstrsqlpurch.Tables[0].Rows[0]["SubTotal"].ToString()) + "&nbsp;</span></td>";
                    prvdataforpdf += "</tr>";
                }
                string strsqlextracharge = "select tbl_salesorderextrachargesdetails.*,tbl_ledgers.groupname as groupname from tbl_salesorderextrachargesdetails inner join tbl_ledgers on tbl_salesorderextrachargesdetails.extrachargeid=tbl_ledgers.refid where tbl_ledgers.entrytype=10 and tbl_salesorderextrachargesdetails.invoiceid='" + ViewState["invoiceid"].ToString() + "' and tbl_salesorderextrachargesdetails.status=1 and tbl_salesorderextrachargesdetails.archive=0 and istax=1 order by id";
                DataSet dsextrac = da.GetDataSetInline(strsqlextracharge);

                // Response.Write(strsqlextracharge);
                if (dsextrac.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < dsextrac.Tables[0].Rows.Count; i++)
                    {
                        prvdataforpdf += " <tr>";
                        prvdataforpdf += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;font-size:" + TotalSubtotaltaxFontSize.Value + "pt;color:" + showcolor(TotalSubtotaltaxFontColor.Value) + ";background:" + TotalSubtotaltaxBackgroundColorvalue + ";' valign='top' ><b><span>" + dsextrac.Tables[0].Rows[i]["groupname"].ToString() + "&nbsp;</span></b></td>";
                        prvdataforpdf += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'><span>" + fns.ToFormattedCurrencyString(dsextrac.Tables[0].Rows[i]["extrachargeamount"].ToString()) + "&nbsp;</span></td>";
                        prvdataforpdf += "</tr>";

                    }
                }
                else
                {
                    if (Request.Cookies["cadmin"]["id"].ToString() == "263")
                    {
                        prvdataforpdf += " <tr>";
                        prvdataforpdf += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;font-size:" + TotalSubtotaltaxFontSize.Value + "pt;color:" + showcolor(TotalSubtotaltaxFontColor.Value) + ";background:" + TotalSubtotaltaxBackgroundColorvalue + ";' valign='top' ><b><span>Freight &nbsp;</span></b></td>";
                        prvdataforpdf += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'><span>" + fns.ToFormattedCurrencyString("0") + "&nbsp;</span></td>";
                        prvdataforpdf += "</tr>";
                    }
                }
                if (showisindia == "0")
                {


                    if (isvat == "1")
                    {
                        prvdataforpdf += "<tr>";
                        prvdataforpdf += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;font-size:" + TotalSubtotaltaxFontSize.Value + "pt;color:" + showcolor(TotalSubtotaltaxFontColor.Value) + ";background:" + TotalSubtotaltaxBackgroundColorvalue + ";' valign='top' ><b><span>Total VAT&nbsp;</span></b></td>";
                        prvdataforpdf += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'><span>" + fns.ToFormattedCurrencyString(dsstrsqlpurch.Tables[0].Rows[0]["totalvat"].ToString()) + "&nbsp;</span></td>";
                        prvdataforpdf += "</tr>";
                    }
                }
                else
                {

                    string strsql = "select * from tbl_contactsdetails where id='" + dsstrsqlpurch.Tables[0].Rows[0]["cusid"].ToString() + "' and status=1 and archive=0 and regid='" + Request.Cookies["cadmin"]["id"].ToString() + "'";
                    DataSet dsissame = da.GetDataSetInline(strsql);
                    if (dsissame.Tables[0].Rows.Count > 0)
                    {
                        if (showbranch == "")
                        {
                            if (fns.getstateidbybranchid(ViewState["branchid"].ToString()).ToString() == dsissame.Tables[0].Rows[0]["PlaceOfSupply"].ToString())
                            {
                                ViewState["issamestate"] = "1";
                                prvdataforpdf += "<tr>";
                                prvdataforpdf += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;font-size:" + TotalSubtotaltaxFontSize.Value + "pt;color:" + showcolor(TotalSubtotaltaxFontColor.Value) + ";background:" + TotalSubtotaltaxBackgroundColorvalue + ";' valign='top' ><b><span>SGST&nbsp;</span></b></td>";
                                prvdataforpdf += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'><span>" + fns.ToFormattedCurrencyString((Convert.ToDecimal(dsstrsqlpurch.Tables[0].Rows[0]["totalvat"].ToString()) / 2).ToString()) + "&nbsp;</span></td>";
                                prvdataforpdf += "</tr>";

                                prvdataforpdf += "<tr>";
                                prvdataforpdf += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;font-size:" + TotalSubtotaltaxFontSize.Value + "pt;color:" + showcolor(TotalSubtotaltaxFontColor.Value) + ";background:" + TotalSubtotaltaxBackgroundColorvalue + ";' valign='top' ><b><span>CGST&nbsp;</span></b></td>";
                                prvdataforpdf += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'><span>" + fns.ToFormattedCurrencyString((Convert.ToDecimal(dsstrsqlpurch.Tables[0].Rows[0]["totalvat"].ToString()) / 2).ToString()) + "&nbsp;</span></td>";
                                prvdataforpdf += "</tr>";
                            }
                            else
                            {
                                prvdataforpdf += "<tr>";
                                prvdataforpdf += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;font-size:" + TotalSubtotaltaxFontSize.Value + "pt;color:" + showcolor(TotalSubtotaltaxFontColor.Value) + ";background:" + TotalSubtotaltaxBackgroundColorvalue + ";' valign='top' ><b><span>IGST&nbsp;</span></b></td>";
                                prvdataforpdf += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'><span>" + fns.ToFormattedCurrencyString(dsstrsqlpurch.Tables[0].Rows[0]["totalvat"].ToString()) + "&nbsp;</span></td>";
                                prvdataforpdf += "</tr>";

                                ViewState["issamestate"] = "0";
                            }
                        }
                        else
                        {


                            if (fns.getstateid().ToString() == dsissame.Tables[0].Rows[0]["PlaceOfSupply"].ToString())
                            {
                                ViewState["issamestate"] = "1";
                                prvdataforpdf += "<tr>";
                                prvdataforpdf += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;font-size:" + TotalSubtotaltaxFontSize.Value + "pt;color:" + showcolor(TotalSubtotaltaxFontColor.Value) + ";background:" + TotalSubtotaltaxBackgroundColorvalue + ";' valign='top' ><b><span>SGST&nbsp;</span></b></td>";
                                prvdataforpdf += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'><span>" + fns.ToFormattedCurrencyString((Convert.ToDecimal(dsstrsqlpurch.Tables[0].Rows[0]["totalvat"].ToString()) / 2).ToString()) + "&nbsp;</span></td>";
                                prvdataforpdf += "</tr>";

                                prvdataforpdf += "<tr>";
                                prvdataforpdf += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;font-size:" + TotalSubtotaltaxFontSize.Value + "pt;color:" + showcolor(TotalSubtotaltaxFontColor.Value) + ";background:" + TotalSubtotaltaxBackgroundColorvalue + ";' valign='top' ><b><span>CGST&nbsp;</span></b></td>";
                                prvdataforpdf += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'><span>" + fns.ToFormattedCurrencyString((Convert.ToDecimal(dsstrsqlpurch.Tables[0].Rows[0]["totalvat"].ToString()) / 2).ToString()) + "&nbsp;</span></td>";
                                prvdataforpdf += "</tr>";
                                //trsgst.Visible = true;
                                //trcgst.Visible = true;
                                //lblvat.Text = "Total Tax";

                            }
                            else
                            {

                                prvdataforpdf += "<tr>";
                                prvdataforpdf += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;font-size:" + TotalSubtotaltaxFontSize.Value + "pt;color:" + showcolor(TotalSubtotaltaxFontColor.Value) + ";background:" + TotalSubtotaltaxBackgroundColorvalue + ";' valign='top' ><b><span>IGST&nbsp;</span></b></td>";
                                prvdataforpdf += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'><span>" + fns.ToFormattedCurrencyString(dsstrsqlpurch.Tables[0].Rows[0]["totalvat"].ToString()) + "&nbsp;</span></td>";
                                prvdataforpdf += "</tr>";

                                ViewState["issamestate"] = "0";
                                //trsgst.Visible = false;
                                //trcgst.Visible = false;
                                //lblvat.Text = "IGST";
                            }

                        }
                    }
                }

                if (isvat == "1")
                {

                    strsqlextracharge = "select tbl_salesorderextrachargesdetails.*,tbl_ledgers.groupname as groupname from tbl_salesorderextrachargesdetails inner join tbl_ledgers on tbl_salesorderextrachargesdetails.extrachargeid=tbl_ledgers.refid where tbl_ledgers.entrytype=10 and tbl_salesorderextrachargesdetails.invoiceid='" + ViewState["invoiceid"].ToString() + "' and tbl_salesorderextrachargesdetails.status=1 and tbl_salesorderextrachargesdetails.archive=0 and istax=0 order by id";
                    dsextrac = da.GetDataSetInline(strsqlextracharge);

                    // Response.Write(strsqlextracharge);
                    if (dsextrac.Tables[0].Rows.Count > 0)
                    {

                        for (int i = 0; i < dsextrac.Tables[0].Rows.Count; i++)
                        {
                            prvdataforpdf += " <tr>";
                            prvdataforpdf += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;font-size:" + TotalSubtotaltaxFontSize.Value + "pt;color:" + showcolor(TotalSubtotaltaxFontColor.Value) + ";background:" + TotalSubtotaltaxBackgroundColorvalue + ";' valign='top' ><b><span>" + dsextrac.Tables[0].Rows[i]["groupname"].ToString() + "&nbsp;</span></b></td>";
                            prvdataforpdf += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'><span>" + fns.ToFormattedCurrencyString(dsextrac.Tables[0].Rows[i]["extrachargeamount"].ToString()) + "&nbsp;</span></td>";
                            prvdataforpdf += "</tr>";

                        }
                    }
                    prvdataforpdf += "<tr>";
                    prvdataforpdf += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;font-size:" + TotalSubtotaltaxFontSize.Value + "pt;color:" + showcolor(TotalSubtotaltaxFontColor.Value) + ";background:" + TotalSubtotaltaxBackgroundColorvalue + ";' valign='top' ><b><span>" + dsstrsqlpurch.Tables[0].Rows[0]["extratfields"].ToString() + "&nbsp;</span></b></td>";
                    prvdataforpdf += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' ><span>" + fns.ToFormattedCurrencyString(dsstrsqlpurch.Tables[0].Rows[0]["extrafiledvalue"].ToString()) + "&nbsp;</span></td>";
                    prvdataforpdf += "</tr>";
                    prvdataforpdf += "<tr>";
                    prvdataforpdf += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;font-size:" + TotalSubtotaltaxFontSize.Value + "pt;color:" + showcolor(TotalSubtotaltaxFontColor.Value) + ";background:" + TotalSubtotaltaxBackgroundColorvalue + ";' valign='top' ><b><span>" + fns.ToFormattedCurrencyString(tableTotal.Value) + "&nbsp;</span></b></td>";

                    if (CurrencySymbol.SelectedValue == "Before amount")
                    {
                        prvdataforpdf += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' ><span>" + fns.bindcurdefault(dsstrsqlpurch.Tables[0].Rows[0]["currencyid"].ToString()) + " " + fns.ToFormattedCurrencyString(dsstrsqlpurch.Tables[0].Rows[0]["totalamount"].ToString()) + "&nbsp;</span><br/><br/></td>";
                    }
                    else
                    {
                        prvdataforpdf += "<td style='padding: 5px 5px 5px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top' ><span>" + fns.ToFormattedCurrencyString(dsstrsqlpurch.Tables[0].Rows[0]["totalamount"].ToString()) + " " + fns.bindcurdefault(dsstrsqlpurch.Tables[0].Rows[0]["currencyid"].ToString()) + "&nbsp;</span><br/><br/></td>";

                    }

                    prvdataforpdf += " </tr>";
                }
                if (isSignatur.Checked)
                {
                    prvdataforpdf += "<tr  text-align:center;'> <td colspan='2' width='100%' border='1' style='vertical-align:bottom; text-align:center; padding:0px;border-bottom:0px solid #000000;' align='center' ></td></tr>";


                    if (backgroundimgsignature != string.Empty)
                    {
                        prvdataforpdf += "<tr  style='border-top:1px solid " + showcolor(TableBorder.Value) + "; text-align:center;'> <td colspan='2' width='100%' border='0' style='vertical-align:bottom; text-align:center; padding:0px;border-bottom:0px solid #000000;' height='150px' align='center' >";

                        prvdataforpdf += "<br/><br/><h1><center><img  style='width:auto;' src='" + ConfigurationManager.AppSettings["domainname"].ToString() + "/" + backgroundimgsignature + "' width='100%'  align='center' /></center></h1>";
                        prvdataforpdf += "<b>" + isSignatureLabel.Value + "</b><br/><br/>&nbsp;";
                        prvdataforpdf += "</td></tr>";
                    }
                    else
                    {
                        prvdataforpdf += "<tr  style='border-top:1px solid " + showcolor(TableBorder.Value) + "; text-align:center;'> <td colspan='2' width='100%' border='0' style='vertical-align:bottom; padding:0px;border-bottom:0px solid #000000; text-align:center;' height='150px' align='center' >";

                        prvdataforpdf += "&nbsp;<br/><br/><br/>" + SignatureName.Value + "&nbsp;<br/><b>";
                        prvdataforpdf += isSignatureLabel.Value;
                        prvdataforpdf += "&nbsp;</b></center><br/><br/></td></tr>";
                    }




                    //prvdataforpdf += "<tr  style='border-top:1px solid " + showcolor(TableBorder.Value) + ";border-bottom:0px; text-align:center;'> <td colspan='2' width='100%' border='1' style='vertical-align:bottom; text-align:center; padding:0pt;border-bottom:0px solid #000000;' height='100px'  >";
                    //prvdataforpdf += "&nbsp;<br/><br/>" + SignatureName.Value + "&nbsp;<br/>";

                    //prvdataforpdf += isSignatureLabel.Value;
                    //prvdataforpdf += "</td></tr>";
                }


                prvdataforpdf += "</table>";


            }


            prvdataforpdf += "</td>";
            prvdataforpdf += "</tr></table>";
            //prvdataforpdf += " </table>";



            if (Request.Cookies["cadmin"]["id"].ToString() == "50")
            {
                if (chkistermsandcondition.Checked)
                {
                    prvdataforpdf += "<table style='width:100%;margin:0px 0 0 0;' cellspacing='0' cellpadding='5' border='1'>";
                    prvdataforpdf += "<tbody>";
                    prvdataforpdf += "<tr>";
                    prvdataforpdf += "<td style='padding:5px 10px 5px 5pt;' align='left'><span style='color:" + getlabelcolor() + ";font-size:" + TermsConditionsFontSize.Value + "pt;'><b>" + TermsConditionsLabel.Value + "</b></span><br>";
                    prvdataforpdf += "<span style='font-size:" + FooterFontSize.Value + "pt;'>" + dsstrsqlpurch.Tables[0].Rows[0]["TermsandConditions"].ToString().Replace("\n", "<br/>") + "</span></td>";
                    prvdataforpdf += "</tr>";
                    prvdataforpdf += "</tbody>";
                    prvdataforpdf += "</table>";
                }
            }
            prvdataforpdf += "</td></tr>";
            prvdataforpdf += "</table>";


            if (showisindia == "1" && showgstsummery == "1")
            {
                prvdataforpdf += "<table cellpadding='5' style='width:100%;margin: 0;border-left:0pt;border-right:0pt; border-color:" + showcolor(TableBorder.Value) + " !important;' cellspacing='0' border='1'>";
                prvdataforpdf += "<tr  bgcolor='" + showcolor(tableheaderBackgroundColor.Value) + "' style='height:32pt;border-left:0pt;border-right:0pt; background:" + showcolor(tableheaderBackgroundColor.Value) + ";font-size:" + headerFontSize.Value + "pt;color:" + showcolor(tableheaderFontColor.Value) + ";'><th rowspan='2' style='text-align: center;' valign='top'><b>HSN/SAC</b></th><th rowspan='2' style='text-align: right;' valign='top'><b>Taxable</b></th><th colspan='2' style='text-align: center;'><b>IGST</b></th><th colspan='2' style='text-align: center;'><b>SGST</b></th><th colspan='2' style='text-align: center;'><b>CGST</b></th></tr>";
                prvdataforpdf += "<tr  bgcolor='" + showcolor(tableheaderBackgroundColor.Value) + "' style='height:32pt;border-left:0pt;border-right:0pt; background:" + showcolor(tableheaderBackgroundColor.Value) + ";font-size:" + headerFontSize.Value + "pt;color:" + showcolor(tableheaderFontColor.Value) + ";'><th style='text-align: right;'><b>%</b></th><th style='text-align: right;'><b>Amount</b></th><th style='text-align: right;'><b>%</b></th><th style='text-align: right;'><b>Amount</b></th><th style='text-align: right;'><b>%</b></th><th style='text-align: right;'><b>Amount</b></th></tr>";


                decimal totaligst = 0;
                decimal totalsgst = 0;
                // decimal totalcgst = 0;
                // string strsqlitemhsn = "select *,tbl_itemsdetails.pname,tbl_itemsdetails.Unitval,tbl_itemsdetails.SKUval from tbl_SalesOrderdetailsitemsdetails left outer join tbl_itemsdetails on tbl_SalesOrderdetailsitemsdetails.itemid=tbl_itemsdetails.id where tbl_SalesOrderdetailsitemsdetails.estmid='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "' and tbl_SalesOrderdetailsitemsdetails.status=1 and tbl_SalesOrderdetailsitemsdetails.archive=0  order by tbl_SalesOrderdetailsitemsdetails.id ";
                string strsqlitemhsn = "select tbl_itemsdetails.SKUval ,sum(amount) as amount,sum(taxamount) as taxamount,taxtvalue from tbl_SalesOrderdetailsitemsdetails left outer join tbl_itemsdetails on tbl_SalesOrderdetailsitemsdetails.itemid=tbl_itemsdetails.id where tbl_SalesOrderdetailsitemsdetails.estmid='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "' and tbl_SalesOrderdetailsitemsdetails.status=1 and tbl_SalesOrderdetailsitemsdetails.archive=0  group by SKUval,taxtvalue  order by SKUval";


                DataSet dsaddhsn = da.GetDataSetInline(strsqlitemhsn);


                if (dsaddhsn.Tables[0].Rows.Count > 0)
                {



                    for (int j = 0; j < dsaddhsn.Tables[0].Rows.Count; j++)
                    {
                        if (tableItemRowBackgroundColor1.Checked)
                        {
                            prvdataforpdf += " <tr style='border-bottom:1px solid " + showcolor(TableBorder.Value) + "; background:" + showcolor(tableItemRowBackgroundColor.Value) + ";font-size:" + tableItemRowfontsize.Value + "pt;color:" + showcolor(tableItemRowFontColor.Value) + ";'>";
                        }
                        else
                        {
                            prvdataforpdf += " <tr style='border-bottom:1px solid " + showcolor(TableBorder.Value) + ";font-size:" + tableItemRowfontsize.Value + "pt;color:" + showcolor(tableItemRowFontColor.Value) + ";'>";
                        }


                        prvdataforpdf += "<td style='text-align: center;' valign='top' >" + dsaddhsn.Tables[0].Rows[j]["SKUval"].ToString() + "</td>";
                        prvdataforpdf += "<td style='text-align: right;' valign='top'>" + fns.ToFormattedCurrencyString(dsaddhsn.Tables[0].Rows[j]["amount"].ToString()) + "</td>";
                        if (ViewState["issamestate"].ToString() == "1")
                        {
                            prvdataforpdf += "<td style='text-align: right;'>0</td>";
                            prvdataforpdf += "<td style='text-align: right;'>0</td>";

                            prvdataforpdf += "<td style='text-align: right;'>" + (Convert.ToDecimal(dsaddhsn.Tables[0].Rows[j]["taxtvalue"].ToString()) / 2) + "</td>";
                            prvdataforpdf += "<td style='text-align: right;'>" + fns.ToFormattedCurrencyString((Convert.ToDecimal(dsaddhsn.Tables[0].Rows[j]["taxamount"].ToString()) / 2).ToString()) + "</td>";
                            prvdataforpdf += "<td style='text-align: right;'>" + (Convert.ToDecimal(dsaddhsn.Tables[0].Rows[j]["taxtvalue"].ToString()) / 2) + "</td>";
                            prvdataforpdf += "<td style='text-align: right;'>" + fns.ToFormattedCurrencyString((Convert.ToDecimal(dsaddhsn.Tables[0].Rows[j]["taxamount"].ToString()) / 2).ToString()) + "</td>";
                            totalsgst += (Convert.ToDecimal(dsaddhsn.Tables[0].Rows[j]["taxamount"].ToString()) / 2);


                        }
                        else
                        {



                            //prvdataforpdf += "<td style='text-align: right;'>0</td>";
                            //prvdataforpdf += "<td style='text-align: right;'>0</td>";
                            prvdataforpdf += "<td style='text-align: right;'>" + dsaddhsn.Tables[0].Rows[j]["taxtvalue"].ToString() + "</td>";
                            prvdataforpdf += "<td style='text-align: right;'>" + fns.ToFormattedCurrencyString(dsaddhsn.Tables[0].Rows[j]["taxamount"].ToString()) + "</td>";
                            prvdataforpdf += "<td style='text-align: right;'>0</td>";
                            prvdataforpdf += "<td style='text-align: right;'>0</td>";
                            prvdataforpdf += "<td style='text-align: right;'>0</td>";
                            prvdataforpdf += "<td style='text-align: right;'>0</td>";
                            totaligst += Convert.ToDecimal(dsaddhsn.Tables[0].Rows[j]["taxamount"].ToString());
                        }
                        prvdataforpdf += "</tr>";

                    }
                }


                string strsqlextracharge = "select tbl_salesorderextrachargesdetails.*,tbl_extrachargesdetails.vatclassificationcode as SKUval from tbl_salesorderextrachargesdetails inner join tbl_extrachargesdetails on tbl_salesorderextrachargesdetails.extrachargeid=tbl_extrachargesdetails.id where tbl_salesorderextrachargesdetails.invoiceid='" + ViewState["invoiceid"].ToString() + "' and tbl_salesorderextrachargesdetails.status=1 and tbl_salesorderextrachargesdetails.archive=0 and istax=1 order by id";
                DataSet dsextrac = da.GetDataSetInline(strsqlextracharge);

                // Response.Write(strsqlextracharge);
                if (dsextrac.Tables[0].Rows.Count > 0)
                {

                    for (int j = 0; j < dsextrac.Tables[0].Rows.Count; j++)
                    {



                        if (tableItemRowBackgroundColor1.Checked)
                        {
                            prvdataforpdf += " <tr style='border-bottom:1px solid " + showcolor(TableBorder.Value) + "; background:" + showcolor(tableItemRowBackgroundColor.Value) + ";font-size:" + tableItemRowfontsize.Value + "pt;color:" + showcolor(tableItemRowFontColor.Value) + ";'>";
                        }
                        else
                        {
                            prvdataforpdf += " <tr style='border-bottom:1px solid " + showcolor(TableBorder.Value) + ";font-size:" + tableItemRowfontsize.Value + "pt;color:" + showcolor(tableItemRowFontColor.Value) + ";'>";
                        }


                        prvdataforpdf += "<td style='text-align: center;' valign='top' >" + dsextrac.Tables[0].Rows[j]["SKUval"].ToString() + "</td>";
                        prvdataforpdf += "<td style='text-align: right;' valign='top'>" + fns.ToFormattedCurrencyString(dsextrac.Tables[0].Rows[j]["extrachargeamount"].ToString()) + "</td>";
                        if (ViewState["issamestate"].ToString() == "1")
                        {
                            prvdataforpdf += "<td style='text-align: right;'>0</td>";
                            prvdataforpdf += "<td style='text-align: right;'>0</td>";

                            prvdataforpdf += "<td style='text-align: right;'>" + (Convert.ToDecimal(dsextrac.Tables[0].Rows[j]["taxpersentage"].ToString()) / 2) + "</td>";
                            prvdataforpdf += "<td style='text-align: right;'>" + fns.ToFormattedCurrencyString((Convert.ToDecimal(dsextrac.Tables[0].Rows[j]["istaxamount"].ToString()) / 2).ToString()) + "</td>";
                            prvdataforpdf += "<td style='text-align: right;'>" + (Convert.ToDecimal(dsextrac.Tables[0].Rows[j]["taxpersentage"].ToString()) / 2) + "</td>";
                            prvdataforpdf += "<td style='text-align: right;'>" + fns.ToFormattedCurrencyString((Convert.ToDecimal(dsextrac.Tables[0].Rows[j]["istaxamount"].ToString()) / 2).ToString()) + "</td>";
                            totalsgst += (Convert.ToDecimal(dsextrac.Tables[0].Rows[j]["istaxamount"].ToString()) / 2);


                        }
                        else
                        {




                            prvdataforpdf += "<td style='text-align: right;'>" + dsextrac.Tables[0].Rows[j]["taxpersentage"].ToString() + "</td>";
                            prvdataforpdf += "<td style='text-align: right;'>" + fns.ToFormattedCurrencyString(dsextrac.Tables[0].Rows[j]["istaxamount"].ToString()) + "</td>";
                            prvdataforpdf += "<td style='text-align: right;'>0</td>";
                            prvdataforpdf += "<td style='text-align: right;'>0</td>";
                            prvdataforpdf += "<td style='text-align: right;'>0</td>";
                            prvdataforpdf += "<td style='text-align: right;'>0</td>";
                            totaligst += Convert.ToDecimal(dsextrac.Tables[0].Rows[j]["istaxamount"].ToString());
                        }
                        prvdataforpdf += "</tr>";


                        //prvdataforpdf += " <tr>";
                        //prvdataforpdf += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;font-size:" + TotalSubtotaltaxFontSize.Value + "pt;color:" + showcolor(TotalSubtotaltaxFontColor.Value) + ";background:" + TotalSubtotaltaxBackgroundColorvalue + ";' valign='top' ><b><span>" + dsextrac.Tables[0].Rows[i]["groupname"].ToString() + "&nbsp;</span></b></td>";
                        //prvdataforpdf += "<td style='padding: 0px 5px 0px 0pt;text-align: right;border-color:" + showcolor(TableBorder.Value) + " !important;' valign='top'><span>" + fns.ToFormattedCurrencyString(dsextrac.Tables[0].Rows[i]["extrachargeamount"].ToString()) + "&nbsp;</span></td>";
                        //prvdataforpdf += "</tr>";

                    }
                }

                if (tableItemRowBackgroundColor1.Checked)
                {
                    prvdataforpdf += " <tr style='border-bottom:1px solid " + showcolor(TableBorder.Value) + "; background:" + showcolor(tableItemRowBackgroundColor.Value) + ";font-size:" + tableItemRowfontsize.Value + "pt;color:" + showcolor(tableItemRowFontColor.Value) + ";'>";
                }
                else
                {
                    prvdataforpdf += " <tr style='border-bottom:1px solid " + showcolor(TableBorder.Value) + ";font-size:" + tableItemRowfontsize.Value + "pt;color:" + showcolor(tableItemRowFontColor.Value) + ";'>";
                }
                prvdataforpdf += "<td style='text-align: right;' colspan='2'><b>Total</b></td>";

                prvdataforpdf += "<td style='text-align: right;' colspan='2'>" + fns.ToFormattedCurrencyString(totaligst.ToString()) + "</td>";

                prvdataforpdf += "<td style='text-align: right;' colspan='2'>" + fns.ToFormattedCurrencyString(totalsgst.ToString()) + "</td>";

                prvdataforpdf += "<td style='text-align: right;' colspan='2'>" + fns.ToFormattedCurrencyString(totalsgst.ToString()) + "</td>";
                prvdataforpdf += "</tr>";
                prvdataforpdf += "</table>";
            }


            if (Request.Cookies["cadmin"]["id"].ToString() == "50")
            {

                prvdataforpdf += "<br/><br/><br/><center><table cellpadding='0' style='width:100%;margin: 0;border-left:0pt;border-right:0pt; border-color:" + showcolor(TableBorder.Value) + " !important;' cellspacing='0' border='0'>";
                prvdataforpdf += "<tr><td>&nbsp;&nbsp;</td><td>&nbsp;&nbsp;</td></tr>";

                prvdataforpdf += "<tr style='font-size:10pt;'><td><b>_______________________________________</b></td><td><b>________________________________</b></td></tr>";

                prvdataforpdf += "<tr style='font-size:10pt;'><td><b>FOR PINK PEPPER CATERING SERVICES LLC</b></td><td><b>RECEIVER’S NAME AND SIGNATURE</b></td></tr>";
                prvdataforpdf += "</table></center>";
            }





            prvdatawithoutheader = strdataheader1 + prvdataforpdf;

            prvdataforpdf = strdataheader1 + strdataheader + prvdataforpdf;

        }

    }



    protected string bindcusdatainvoice()
    {



        string strcusdata = "";

        string strsql = "select tbl_customefielddetails.*,tbl_cusdatatypemaster.cusdatatype from tbl_customefielddetails inner join tbl_cusdatatypemaster  on  tbl_customefielddetails.labeldatatype=tbl_cusdatatypemaster.id  where tbl_customefielddetails.regid='" + Request.Cookies["cadmin"]["id"] + "' and tbl_customefielddetails.status=1 and tbl_customefielddetails.archive=0 and tbl_customefielddetails.typeid='4' and (showonprint=1 or showonprint is null) and printpotion=2 order by orderno,id";
        DataSet ds = da.GetDataSetInline(strsql);

        int counter = 1; // this will be used to control naming  
        // create a table to store 

        if (ds.Tables[0].Rows.Count > 0)
        {
            strcusdata = "<table border='0' cellpadding='5'>";
        }

        for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
        {
            int y = Convert.ToInt32(ds.Tables[0].Rows[x]["labeldatatype"].ToString());
            string controlid = ds.Tables[0].Rows[x]["id"].ToString();
            string controlvalue = "";
            strcusdata += "<tr><td colspan='3'>";
            if (ds.Tables[0].Rows[x]["showlavel"].ToString() == "1")
            {

                strcusdata += "<b>" + ds.Tables[0].Rows[x]["labelname"].ToString() + " : </span></b> ";

            }

            string strsqldata = "select * from tbl_SalesOrderdetailscustomefieldvalue where uid='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "' and controlid='" + controlid + "' and status=1 and archive=0 order by id ";
            DataSet dsc = da.GetDataSetInline(strsqldata);
            if (dsc.Tables[0].Rows.Count > 0)
            {
                // controlvalue = dsc.Tables[0].Rows[0]["controlvalue"].ToString();
                strcusdata += dsc.Tables[0].Rows[0]["controlvalue"].ToString().Replace("\n", "<br/>");
                switch (y)
                {
                    case 1:

                        break;
                    case 2:

                        break;
                    case 3:


                        break;
                    default:
                        break;
                }
            }

            strcusdata += "</td></tr>";
            counter++;


        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            strcusdata += "</table>";
        }

        return strcusdata;
    }
    protected string bindcusdataheaderforpdf()
    {



        string strcusdata = "";

        string strsql = "select tbl_customefielddetails.*,tbl_cusdatatypemaster.cusdatatype from tbl_customefielddetails inner join tbl_cusdatatypemaster  on  tbl_customefielddetails.labeldatatype=tbl_cusdatatypemaster.id  where tbl_customefielddetails.regid='" + Request.Cookies["cadmin"]["id"] + "' and tbl_customefielddetails.status=1 and tbl_customefielddetails.archive=0 and tbl_customefielddetails.typeid='4' and (showonprint=1 or showonprint is null) and printpotion=1 order by orderno,id";
        DataSet ds = da.GetDataSetInline(strsql);

        int counter = 1; // this will be used to control naming  
        // create a table to store 

        if (ds.Tables[0].Rows.Count > 0)
        {
            //  strcusdata = "<table border='0' cellpadding='5'>";
        }

        for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
        {
            int y = Convert.ToInt32(ds.Tables[0].Rows[x]["labeldatatype"].ToString());
            string controlid = ds.Tables[0].Rows[x]["id"].ToString();
            string controlvalue = "";




            strcusdata += "<tr><td style='padding:0pt;' ><b><span style='color:" + getlabelcolor() + "'>";
            if (ds.Tables[0].Rows[x]["showlavel"].ToString() == "1")
            {

                strcusdata += "" + ds.Tables[0].Rows[x]["labelname"].ToString() + "</span>";

            }
            strcusdata += "</span></b></td>";
            strcusdata += "  <td style='padding:0pt;'> : <span>";
            string strsqldata = "select * from tbl_SalesOrderdetailscustomefieldvalue where uid='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "' and controlid='" + controlid + "' and status=1 and archive=0 order by id ";
            DataSet dsc = da.GetDataSetInline(strsqldata);
            if (dsc.Tables[0].Rows.Count > 0)
            {


                strcusdata += dsc.Tables[0].Rows[0]["controlvalue"].ToString().Replace("\n", "<br/>");





                switch (y)
                {
                    case 1:

                        break;
                    case 2:

                        break;
                    case 3:


                        break;
                    default:
                        break;
                }
            }

            strcusdata += "</span></td></tr>";
            counter++;


        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            // strcusdata += "</table>";
        }

        return strcusdata;
    }
    protected string bindcusdataheader()
    {



        string strcusdata = "";

        string strsql = "select tbl_customefielddetails.*,tbl_cusdatatypemaster.cusdatatype from tbl_customefielddetails inner join tbl_cusdatatypemaster  on  tbl_customefielddetails.labeldatatype=tbl_cusdatatypemaster.id  where tbl_customefielddetails.regid='" + Request.Cookies["cadmin"]["id"] + "' and tbl_customefielddetails.status=1 and tbl_customefielddetails.archive=0 and tbl_customefielddetails.typeid='4'  and printpotion=1 order by orderno,id";
        DataSet ds = da.GetDataSetInline(strsql);

        int counter = 1; // this will be used to control naming  
        // create a table to store 

        if (ds.Tables[0].Rows.Count > 0)
        {
            //  strcusdata = "<table border='0' cellpadding='5'>";
        }

        for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
        {
            int y = Convert.ToInt32(ds.Tables[0].Rows[x]["labeldatatype"].ToString());
            string controlid = ds.Tables[0].Rows[x]["id"].ToString();
            string controlvalue = "";




            strcusdata += "<tr><td style='padding:0pt;' ><b><span style='color:" + getlabelcolor() + "'>";


            strcusdata += "" + ds.Tables[0].Rows[x]["labelname"].ToString() + "</span>";


            strcusdata += "</span></b></td>";
            strcusdata += "  <td style='padding:0pt;'> : <span>";
            string strsqldata = "select * from tbl_SalesOrderdetailscustomefieldvalue where uid='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "' and controlid='" + controlid + "' and status=1 and archive=0 order by id ";
            DataSet dsc = da.GetDataSetInline(strsqldata);
            if (dsc.Tables[0].Rows.Count > 0)
            {


                strcusdata += dsc.Tables[0].Rows[0]["controlvalue"].ToString().Replace("\n", "<br/>");





                switch (y)
                {
                    case 1:

                        break;
                    case 2:

                        break;
                    case 3:


                        break;
                    default:
                        break;
                }
            }

            strcusdata += "</span></td></tr>";
            counter++;


        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            // strcusdata += "</table>";
        }

        return strcusdata;
    }
    protected string bindcusdatafooter()
    {

        string strcusdata = "";

        string strsql = "select tbl_customefielddetails.*,tbl_cusdatatypemaster.cusdatatype from tbl_customefielddetails inner join tbl_cusdatatypemaster  on  tbl_customefielddetails.labeldatatype=tbl_cusdatatypemaster.id  where tbl_customefielddetails.regid='" + Request.Cookies["cadmin"]["id"] + "' and tbl_customefielddetails.status=1 and tbl_customefielddetails.archive=0 and tbl_customefielddetails.typeid='4' and (showonprint=1 or showonprint is null) and printpotion=3 order by orderno,id";
        DataSet ds = da.GetDataSetInline(strsql);

        int counter = 1; // this will be used to control naming  
                         // create a table to store 
        if (ds.Tables[0].Rows.Count > 0)
        {
            strcusdata += "<table style='width:100%;margin:15px 0 0 0;' cellspacing='0' cellpadding='5' border='0'>";
            strcusdata += "<tbody>";
        }

        for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
        {
            int y = Convert.ToInt32(ds.Tables[0].Rows[x]["labeldatatype"].ToString());
            string controlid = ds.Tables[0].Rows[x]["id"].ToString();
            string controlvalue = "";

            if (ds.Tables[0].Rows[x]["showlavel"].ToString() == "1")
            {
                strcusdata += "<tr><td style='padding:5px 10px 5px 5pt;' align='left'><span style='color:" + getlabelcolor() + ";font-size:" + TermsConditionsFontSize.Value + "pt;' ><b>";
                strcusdata += ds.Tables[0].Rows[x]["labelname"].ToString() + "</b></span><br>";
            }
            else
            {
                strcusdata += "<tr><td style='padding:5px 10px 5px 5pt;' align='left' ><span style='font-size:" + FooterFontSize.Value + "pt;'>";

            }
            string strsqldata = "select * from tbl_SalesOrderdetailscustomefieldvalue where uid='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "' and controlid='" + controlid + "' and status=1 and archive=0 order by id ";
            DataSet dsc = da.GetDataSetInline(strsqldata);
            if (dsc.Tables[0].Rows.Count > 0)
            {
                // controlvalue = dsc.Tables[0].Rows[0]["controlvalue"].ToString();
                strcusdata += dsc.Tables[0].Rows[0]["controlvalue"].ToString().Replace("\n", "<br/>");
                switch (y)
                {
                    case 1:

                        break;
                    case 2:

                        break;
                    case 3:


                        break;
                    default:
                        break;
                }
            }

            strcusdata += "</span></td></tr>";
            counter++;


        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            strcusdata += "</tbody>";
            strcusdata += "</table>";
        }

        return strcusdata;
    }



    protected void txtcancelqty_TextChanged(object sender, EventArgs e)
    {
        //  TextBox link = (TextBox)Gridview1.FindControl("quantity");
        //link = (TextBox)sender;
        //GridViewRow gvr = (GridViewRow)link.NamingContainer;
        //HiddenField hdnrcvqty = (HiddenField)gvr.FindControl("hdnrcvqty");
        //HiddenField hdncancelqty = (HiddenField)gvr.FindControl("hdncancelqty");
        //TextBox txtcancelqty = (TextBox)gvr.FindControl("txtcancelqty");
        //Label lblmsg = (Label)gvr.FindControl("lblmsg");
        //decimal remqty = 0;
        //if (hdnrcvqty.Value!="0" && hdnrcvqty.Value != "0")
        //{
        //    if (Convert.ToDecimal(hdnrcvqty.Value) > Convert.ToDecimal(hdncancelqty.Value))
        //    {
        //        remqty = Convert.ToDecimal(hdnrcvqty.Value) - Convert.ToDecimal(hdncancelqty.Value);
        //        if(Convert.ToDecimal(txtcancelqty.Text) > remqty)
        //        {
        //            lblmsg.Visible = true;
        //            lblmsg.Text = "Cancel Quantiy is not be greater than Remaining Qty";
        //            txtcancelqty.Text = "0";
        //        }
        //        else
        //        {
        //            lblmsg.Visible = false;
        //            lblmsg.Text = "";

        //        }

        //    }
        //  }
    }
    protected void lnkreccived_Click(object sender, EventArgs e)
    {
        Response.Redirect("addnewinvoiceordersalesorder.aspx?id=" + Request.QueryString["id"].ToString() + "&type=sales");
    }

    protected void lnkreccivedDelivery_Click(object sender, EventArgs e)
    {
        Response.Redirect("addnewinvoiceorderDeliveryorder.aspx?id=" + Request.QueryString["id"].ToString() + "&type=sales");
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Response.Redirect("manageInvoicesdeliveryorderforinvoice.aspx?id=" + Request.QueryString["id"].ToString());
    }

    protected void setnotification(string strid, string typeid, string ntfor)
    {
        string r1 = "";
        //setoff==> type off notification like lead,opp etc
        string notif_type = "";
        DataAccess da = new DataAccess();

        string strsql = "select * from tbl_notificationtemplates where typeid='" + typeid + "' and ntfor='" + ntfor + "' and status=1 and archive=0";
        //r1 = strsql;
        DataSet dsnot = da.GetDataSetInline(strsql);
        if (dsnot.Tables[0].Rows.Count > 0)
        {

            DataRow[] dr_arr = dsnot.Tables[0].Select("notif_type=1");

            #region set for bell icon
            if (dr_arr.Length > 0)
            {
                notif_type = "1";
                //SaveLogText("notif_type "+ notif_type);
                setnotification_quotation(strid, typeid, ntfor, notif_type);
                dr_arr = null;
            }
            #endregion
            #region set for email
            dr_arr = dsnot.Tables[0].Select("notif_type=2");
            if (dr_arr.Length > 0)
            {
                notif_type = "2";
                setnotification_quotation(strid, typeid, ntfor, notif_type);
                dr_arr = null;
            }
            #endregion
            #region set for whatsapp
            dr_arr = dsnot.Tables[0].Select("notif_type=3");
            if (dr_arr.Length > 0)
            {
                notif_type = "3";
                setnotification_quotation(strid, typeid, ntfor, notif_type);
                dr_arr = null;
            }
            #endregion
            #region set for sms
            dr_arr = dsnot.Tables[0].Select("notif_type=4");
            if (dr_arr.Length > 0)
            {
                notif_type = "4";
                setnotification_quotation(strid, typeid, ntfor, notif_type);
                dr_arr = null;
            }
            #endregion
            #region set for screen
            dr_arr = dsnot.Tables[0].Select("notif_type=5");
            if (dr_arr.Length > 0)
            {
                notif_type = "5";
                setnotification_quotation(strid, typeid, ntfor, notif_type);
                dr_arr = null;
            }

            #endregion
        }
    }

    protected void setnotification_quotation(string strid, string typeid, string ntfor, string notif_type)
    {

        DataAccess da = new DataAccess();
        CommonFunction fns = new CommonFunction();
        //public function fns = new function();
        function fns2 = new function();
        try
        {
            string userid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();
            string str_ntfor = "";
            str_ntfor = "Quotation Notification by " + fns2.getempnamebyid(userid);
            //if (ntfor == "1")
            //{
            //    str_ntfor = "New Quotation";
            //}
            //else if (ntfor == "2")
            //{
            //    str_ntfor = "Modified Quotation";
            //}
            //else if (ntfor == "3")
            //{
            //    str_ntfor = "Deleted Quotation";
            //}
            //else if (ntfor == "4")
            //{
            //    str_ntfor = "Canceled Quotation";
            //}
            #region set for bell icon
            string strsql = "select * from tbl_notificationtemplates where typeid='" + typeid + "' and ntfor='" + ntfor + "' and notif_type='" + notif_type + "' and status=1 and archive=0";
            DataSet dsnot = da.GetDataSetInline(strsql);
            if (dsnot.Tables[0].Rows.Count > 0)
            {
                string isview = dsnot.Tables[0].Rows[0]["isview"].ToString();
                string notificationtext = dsnot.Tables[0].Rows[0]["temptext"].ToString();

                string clogo = "&nbsp;";
                string txtnameofcompany = "&nbsp;";
                string txtaddress1 = "&nbsp;";
                string txtaddress2 = "&nbsp;";
                string txtCountry = "&nbsp;";
                string txtcity = "&nbsp;";
                string txtzipcode = "&nbsp;";
                string txtphoneno = "&nbsp;";
                string txtfax = "&nbsp;";
                string txtwebsite = "&nbsp;";
                string txtemailcontact = "&nbsp;";
                string IssuedOn = "&nbsp;";
                string PurchaseOrderNumber = "&nbsp;";
                string PurchaseOrderRef = "&nbsp;";
                string FormattedTotal = "&nbsp;";
                string Total = "&nbsp;";
                string CurrencyCode = "";
                string DeliveryDate = "&nbsp;";
                string customerCompanyName = "&nbsp;";
                string customerSalutation = "&nbsp;";
                string CustomerBalance = "&nbsp;";
                string CustomerWebsite = "&nbsp;";
                string CustomerName = "&nbsp;";
                string CustomerFirstName = "&nbsp;";
                string CustomerLastName = "&nbsp;";
                string CustomerDepartment = "&nbsp;";
                string CustomerDesignation = "&nbsp;";
                string OrganizationName = "&nbsp;";
                string OrganizationUser = "&nbsp;";
                string OrganizationPhone = "&nbsp;";
                string OrganizationFax = "&nbsp;";
                string OrganizationWebsite = "&nbsp;";
                string OrgLogo = "&nbsp;";
                string pono = "";
                string KpiName = "&nbsp;";
                string KpiType = "&nbsp;";
                string KpiDescription = "&nbsp;";
                string KpiDataType = "&nbsp;";



                SqlParameter[] prmpurch = new SqlParameter[2];
                prmpurch[0] = new SqlParameter("@id", strid);
                prmpurch[1] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                DataSet dsstrsqlpurch = da.GetDataSet("tbl_Quotationorderdetailsselectdetails", prmpurch);
                if (dsstrsqlpurch.Tables[0].Rows.Count > 0)
                {
                    string companyname = dsstrsqlpurch.Tables[0].Rows[0]["txtcontactdisplayname"].ToString();
                    // lbltitle.Text = fns.getltext("Email Quotation To " + companyname, "2");
                    pono = dsstrsqlpurch.Tables[0].Rows[0]["Estimateno"].ToString();
                    userid = dsstrsqlpurch.Tables[0].Rows[0]["uid"].ToString();
                    // lblfdffilename.Text = dsstrsqlpurch.Tables[0].Rows[0]["Estimateno"].ToString().Replace(@"/", "_").Replace(@"\", "_");
                    // lblfdffilename.Text = dsstrsqlpurch.Tables[0].Rows[0]["Estimateno"].ToString();
                    IssuedOn = Convert.ToDateTime(dsstrsqlpurch.Tables[0].Rows[0]["EstimateDate"].ToString()).ToString(fns2.getdateformat());
                    PurchaseOrderNumber = dsstrsqlpurch.Tables[0].Rows[0]["Estimateno"].ToString();
                    PurchaseOrderRef = dsstrsqlpurch.Tables[0].Rows[0]["Reference"].ToString();
                    FormattedTotal = dsstrsqlpurch.Tables[0].Rows[0]["totalamount"].ToString();
                    Total = dsstrsqlpurch.Tables[0].Rows[0]["totalamount"].ToString();
                    DeliveryDate = Convert.ToDateTime(dsstrsqlpurch.Tables[0].Rows[0]["ExpiryDate"].ToString()).ToString(fns2.getdateformat());

                    OrganizationUser = fns2.getusersname(dsstrsqlpurch.Tables[0].Rows[0]["uid"].ToString());



                    strsql = "select * from tbl_contactsdetails where id='" + dsstrsqlpurch.Tables[0].Rows[0]["cusid"].ToString() + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "'";
                    DataSet ds = da.GetDataSetInline(strsql);
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        customerSalutation = ds.Tables[0].Rows[0]["txtsalutation"].ToString();
                        //  string CustomerBalance"] =ds.Tables[0].Rows[0]["Estimateno"].ToString();
                        CustomerWebsite = ds.Tables[0].Rows[0]["Website"].ToString();
                        CustomerName = ds.Tables[0].Rows[0]["txtcompanyname"].ToString();
                        customerCompanyName = ds.Tables[0].Rows[0]["txtcompanyname"].ToString();
                        CustomerFirstName = ds.Tables[0].Rows[0]["txtfirstname"].ToString();
                        CustomerLastName = ds.Tables[0].Rows[0]["txtlastname"].ToString();
                        CustomerDepartment = ds.Tables[0].Rows[0]["txtDepartment"].ToString();
                        CustomerDesignation = ds.Tables[0].Rows[0]["txtDesignation"].ToString();

                        string[] ch = { " -" };
                        try
                        {
                            CurrencyCode = ds.Tables[0].Rows[0]["txtCurrency"].ToString();
                        }
                        catch
                        {

                        }


                    }







                    SqlParameter[] prm = new SqlParameter[1];
                    prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                    ds = da.GetDataSet("tbl_companydetails_selectbyregid", prm);
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        clogo = ds.Tables[0].Rows[0]["clogo"].ToString();

                        if (clogo != string.Empty)
                        {
                            OrgLogo = "<img src='" + ConfigurationManager.AppSettings["domainname"].ToString() + "/clogo/" + clogo + "' width='150px' />";
                        }
                        txtnameofcompany = ds.Tables[0].Rows[0]["txtnameofcompany"].ToString();
                        txtaddress1 = ds.Tables[0].Rows[0]["txtaddress1"].ToString() + " &nbsp;";
                        txtaddress2 = ds.Tables[0].Rows[0]["txtaddress2"].ToString() + " &nbsp;";
                        txtCountry = ds.Tables[0].Rows[0]["txtCountry"].ToString() + " &nbsp;";
                        txtcity = ds.Tables[0].Rows[0]["txtcity"].ToString() + " &nbsp;";
                        txtzipcode = ds.Tables[0].Rows[0]["txtzipcode"].ToString() + " &nbsp;";
                        txtphoneno = ds.Tables[0].Rows[0]["txtphoneno"].ToString() + " &nbsp;";
                        txtfax = ds.Tables[0].Rows[0]["txtfax"].ToString() + " &nbsp;";
                        txtwebsite = ds.Tables[0].Rows[0]["txtwebsite"].ToString() + " &nbsp;";
                        txtemailcontact = ds.Tables[0].Rows[0]["txtemailcontact"].ToString() + " &nbsp;";
                        OrganizationName = txtnameofcompany;
                        OrganizationPhone = txtphoneno;
                        OrganizationFax = txtfax;
                        OrganizationWebsite = txtwebsite;

                        try
                        {

                            if (CurrencyCode == string.Empty)
                            {

                                strsql = "select tbl_Currenciesdetails.*,tbl_Currenciesmaster.Currency,tbl_Currenciesmaster.curName from tbl_Currenciesdetails inner join tbl_Currenciesmaster on tbl_Currenciesdetails.curid=tbl_Currenciesmaster.id where tbl_Currenciesdetails.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and tbl_Currenciesdetails.status=1 and tbl_Currenciesdetails.archive=0 order by tbl_Currenciesdetails.id";
                                ds = da.GetDataSetInline(strsql);
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    CurrencyCode = ds.Tables[0].Rows[0]["cursym"].ToString();
                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {

                                        if (ds.Tables[0].Rows[i]["isdefault"].ToString() == "1")
                                        {
                                            CurrencyCode = ds.Tables[0].Rows[i]["cursym"].ToString();
                                        }
                                    }
                                }

                            }
                            else
                            {
                                strsql = "select tbl_Currenciesdetails.*,tbl_Currenciesmaster.Currency,tbl_Currenciesmaster.curName from tbl_Currenciesdetails inner join tbl_Currenciesmaster on tbl_Currenciesdetails.curid=tbl_Currenciesmaster.id where tbl_Currenciesdetails.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and tbl_Currenciesdetails.status=1 and tbl_Currenciesdetails.archive=0 and tbl_Currenciesdetails.id='" + CurrencyCode + "' order by tbl_Currenciesdetails.id";
                                ds = da.GetDataSetInline(strsql);
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    CurrencyCode = ds.Tables[0].Rows[0]["cursym"].ToString();
                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {

                                        if (ds.Tables[0].Rows[i]["isdefault"].ToString() == "1")
                                        {
                                            CurrencyCode = ds.Tables[0].Rows[i]["cursym"].ToString();
                                        }
                                    }
                                }
                            }
                        }
                        catch
                        {

                        }



                    }


                    notificationtext = notificationtext.Replace("{####OrganizationName####}", OrganizationName.ToString()
                        ).Replace("{####OrganizationPhone####}", OrganizationPhone.ToString()
                        ).Replace("{####OrganizationFax####}", OrganizationFax.ToString()
                        ).Replace("{####OrganizationWebsite####}", OrganizationWebsite.ToString()
                        ).Replace("{####CustomerSalutation####}", customerSalutation.ToString()
                        ).Replace("{####CustomerWebsite####}", CustomerWebsite.ToString()
                        ).Replace("{####CustomerName####}", CustomerName.ToString()
                        ).Replace("{####CustomerFirstName####}", CustomerFirstName.ToString()
                        ).Replace("{####CustomerLastName####}", CustomerLastName.ToString()
                        ).Replace("{####CustomerDepartment####}", CustomerDepartment.ToString()
                        ).Replace("{####CustomerDesignation####}", CustomerDesignation.ToString()
                        ).Replace("{####QuotationDate####}", IssuedOn.ToString()
                        ).Replace("{####QuotationNumber####}", PurchaseOrderNumber.ToString()
                        ).Replace("{####QuotationRef####}", PurchaseOrderRef.ToString()
                        ).Replace("{####FormattedTotal####}", FormattedTotal.ToString()
                        ).Replace("{####QuotationTotal####}", Total.ToString()
                        ).Replace("{####DeliveryDate####}", DeliveryDate.ToString()
                        ).Replace("{####CurrencyCode####}", CurrencyCode.ToString()
                        ).Replace("{####OrganizationUser####}", OrganizationUser.ToString()
                        ).Replace("{####OrganizationName####}", OrganizationName.ToString()
                        ).Replace("{####customerCompanyName####}", customerCompanyName.ToString()
                        ).Replace("{####OrgLogo####}", OrgLogo.ToString());
                }


                char[] cha = { ',' };
                //SaveLogText("notificationtext " + notificationtext);
                string strsqlitem = "select * from tbl_notificationsetupemp where type='" + typeid + "'  and ntfor='" + ntfor + "'  and notif_type='" + notif_type + "'  and status=1 and archive=0  order by aLevel ";
                DataSet dsadd = da.GetDataSetInline(strsqlitem);
                if (dsadd.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < dsadd.Tables[0].Rows.Count; i++)
                    {

                        string[] arr = dsadd.Tables[0].Rows[i]["hdnempids"].ToString().Split(cha, StringSplitOptions.None);
                        string nuid = "0";
                        for (int j = 0; j < arr.Length; j++)
                        {
                            if (notif_type == "1" || notif_type == "5")
                            {
                                // 1- Bell Icon,5-Screen popup
                                //SaveLogText(notif_type);
                                SqlParameter[] prmn = new SqlParameter[13];
                                prmn[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                                prmn[1] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                                prmn[2] = new SqlParameter("@nuid", arr[j]);
                                prmn[3] = new SqlParameter("@ndatetime", da.getcdatetime());
                                prmn[4] = new SqlParameter("@notificationtext", notificationtext);
                                prmn[5] = new SqlParameter("@rid", strid);
                                prmn[6] = new SqlParameter("@typeid", typeid);
                                prmn[7] = new SqlParameter("@isnew", "1");
                                prmn[8] = new SqlParameter("@isread", "0");
                                prmn[9] = new SqlParameter("@isreaddate", DBNull.Value);
                                prmn[10] = new SqlParameter("@isview", isview);
                                prmn[11] = new SqlParameter("@ntfor", ntfor);
                                prmn[12] = new SqlParameter("@notif_type", notif_type);
                                da.executeDMLQuery("tbl_pnotificationdetailsinsert_crm", prmn);
                                //SaveLogText("ok");
                            }
                            if (notif_type == "2")
                            {
                                // Email Notification
                                SendMail sm = new SendMail();

                                string to = "";
                                string from = "";
                                string cc = "";
                                string bcc = "";
                                string sub = str_ntfor;
                                string body = notificationtext;
                                string files = "";
                                string[] chkdd = { "####" };

                                string[] filesdata = files.Split(chkdd, StringSplitOptions.None);

                                string strsqlempto = "select id,DisplayName,Email,email_Id from tbl_employee where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id ='" + arr[j] + "' order by id";
                                DataSet dsempto = da.GetDataSetInline(strsqlempto);
                                if (dsempto.Tables[0].Rows.Count > 0)
                                {
                                    //SaveLogText(strsqlempto);
                                    to = (dsempto.Tables[0].Rows[0]["email_Id"].ToString() == "" ? dsempto.Tables[0].Rows[0]["Email"].ToString() : dsempto.Tables[0].Rows[0]["email_Id"].ToString());
                                }

                                string strsqlchk = "select tbl_gmailconfiguration_crm.*,tbl_employee.Displayname from tbl_gmailconfiguration_crm inner join tbl_employee on tbl_gmailconfiguration_crm.uid=tbl_employee.id where tbl_gmailconfiguration_crm.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and tbl_gmailconfiguration_crm.uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"] + "' and  tbl_gmailconfiguration_crm.status=1 and tbl_gmailconfiguration_crm.archive=0 and configtype='1'";
                                DataSet Dschk = da.GetDataSetInline(strsqlchk);
                                if (Dschk.Tables[0].Rows.Count > 0)
                                {
                                    //SaveLogText("send from :" + Dschk.Tables[0].Rows[0]["cfgmailid"].ToString());
                                    from = Dschk.Tables[0].Rows[0]["cfgmailid"].ToString();
                                    sendmailnew_common(strid, Dschk.Tables[0].Rows[0]["tokenid"].ToString(), Dschk.Tables[0].Rows[0]["refreshtokenid"].ToString(), to, from, cc, bcc, sub, body, filesdata, "1", "6", 1);
                                }
                                else
                                {
                                    strsqlchk = "select tbl_gmailconfiguration_crm.*,tbl_employee.Displayname from tbl_gmailconfiguration_crm inner join tbl_employee on tbl_gmailconfiguration_crm.uid=tbl_employee.id where tbl_gmailconfiguration_crm.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and tbl_gmailconfiguration_crm.uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"] + "' and  tbl_gmailconfiguration_crm.status=1 and tbl_gmailconfiguration_crm.archive=0 and configtype='2'";
                                    Dschk = da.GetDataSetInline(strsqlchk);
                                    if (Dschk.Tables[0].Rows.Count > 0)
                                    {
                                        //SaveLogText("send from :" + Dschk.Tables[0].Rows[0]["cfgmailid"].ToString());
                                        from = Dschk.Tables[0].Rows[0]["cfgmailid"].ToString();
                                        sendmailnew_common(strid, Dschk.Tables[0].Rows[0]["tokenid"].ToString(), Dschk.Tables[0].Rows[0]["refreshtokenid"].ToString(), to, from, cc, bcc, sub, body, filesdata, "1", "6", 2);
                                    }
                                    else
                                    {
                                        string strsqlemp = "select id,DisplayName,Email,email_Id from tbl_employee where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id ='" + HttpContext.Current.Request.Cookies["user"]["user_id"].ToString() + "' order by id";
                                        DataSet dsemp = da.GetDataSetInline(strsqlemp);
                                        if (dsemp.Tables[0].Rows.Count > 0)
                                        {
                                            from = (dsemp.Tables[0].Rows[0]["email_Id"].ToString() == "" ? dsemp.Tables[0].Rows[0]["Email"].ToString() : dsemp.Tables[0].Rows[0]["email_Id"].ToString());
                                            sm.SendMailtoUser(to, from, cc, bcc, sub, body, filesdata);
                                        }

                                       
                                    }
                                }



                            }
                            if (notif_type == "3")
                            {
                                // Whatsap Notificatiomn
                            }
                            if (notif_type == "4")
                            {
                                // SMS Notificatiomn
                            }


                        }
                    }
                }
                else
                {

                }


            }
            #endregion
        }
        catch (Exception ex)
        {
            //SaveLogText("Step 2 User Already Configured " + ex.Message + "");
        }

    }
    protected void sendmailnew_common(string entryid, string Tokenid, string RefTokenid, string To, string from, string CC, string BCC, string Subject, string Message, string[] files, string status, string entrytypeid = "0", int emailtype = 0)
    {
        //entrytypeid==>1 -lead,2-opp ,3-company,4-contact
        //status==>1-send,2-replay
        DataAccess da = new DataAccess();
        CommonFunction fns = new CommonFunction();
        string attachfiles = "";
        List<string> filesurl = new List<string>();
        for (int i = 0; i < files.Length; i++)
        {

            string imagePath = files[i]; // Path to your image file
            if (attachfiles != "")
            {
                //attachfiles += "####" + ConvertImageToBase64(imagePath);
                attachfiles += imagePath;
                filesurl.Add(imagePath);
            }
            else
            {
                //attachfiles= ConvertImageToBase64(imagePath);
                attachfiles += imagePath;
                filesurl.Add(imagePath);
            }
        }


        string apiUrl = ConfigurationManager.AppSettings["gmailapi"].ToString() + "/api/values/Post";
        if (emailtype == 2)
        {
            apiUrl = ConfigurationManager.AppSettings["gmailapi"].ToString() + "/api/values/SendOutlookMail";
        }
        var httpRequest1 = (HttpWebRequest)WebRequest.Create(apiUrl);

        httpRequest1.Method = "POST";

        httpRequest1.Accept = "application/json";
        httpRequest1.ContentType = "application/json";

        var data1 = @"{";
        data1 += " \"TokenID\":\"" + Tokenid + "\",";
        data1 += " \"RefreshTokenID\":\"" + RefTokenid + "\",";
        data1 += " \"From\":\"" + from + "\",";
        data1 += " \"To\":\"" + To + "\",";
        data1 += " \"Subject\":\"" + Subject.Trim() + "\",";
        data1 += " \"Message\":\"" + EscapeQuotes(Message.Trim()) + "\",";
        data1 += " \"CC\":\"" + CC + "\",";
        data1 += " \"BCC\":\"" + BCC + "\", ";
        data1 += " \"Attachments\":" + JsonConvert.SerializeObject(filesurl) + " ";
        data1 += "}";


        using (var streamWriter = new StreamWriter(httpRequest1.GetRequestStream()))
        {
            streamWriter.Write(data1);
        }
        var httpResponse1 = (HttpWebResponse)httpRequest1.GetResponse();
        using (var streamReader = new StreamReader(httpResponse1.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd().Trim();
            dynamic data11 = JObject.Parse(result);
            //Response.Write(data11.messageid);
            if (data11.messageid != "")
            {
                string msgid = data11.messageid;
                string msgstatus = status;
                SqlParameter[] prm = new SqlParameter[17];

                prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prm[1] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
                prm[2] = new SqlParameter("@cdate", da.getcdatetime());
                prm[3] = new SqlParameter("@entryid", entryid);
                prm[4] = new SqlParameter("@entrytypeid", entrytypeid);
                prm[5] = new SqlParameter("@msgid", msgid);
                prm[6] = new SqlParameter("@msgstatus", msgstatus);

                prm[7] = new SqlParameter("@tokenid", Tokenid);
                prm[8] = new SqlParameter("@MFrom", from);
                prm[9] = new SqlParameter("@MTo", To);
                prm[10] = new SqlParameter("@MSubject", Subject);
                prm[11] = new SqlParameter("@MMessage", Message);
                if (CC == "")
                {
                    prm[12] = new SqlParameter("@MCc", DBNull.Value);
                }
                else
                {
                    prm[12] = new SqlParameter("@MCc", CC);
                }
                if (BCC == "")
                {
                    prm[13] = new SqlParameter("@MBcc", DBNull.Value);
                }
                else
                {
                    prm[13] = new SqlParameter("@MBcc", BCC);
                }
                if (attachfiles == "")
                {
                    prm[14] = new SqlParameter("@MFiles", DBNull.Value);
                }
                else
                {
                    prm[14] = new SqlParameter("@MFiles", attachfiles);
                }
                prm[15] = new SqlParameter("@emailtype", emailtype);
                prm[16] = new SqlParameter("ReturnValue", SqlDbType.Int);
                prm[16].Direction = ParameterDirection.ReturnValue;
                da.executeDMLQuery("tbl_gmailhistory_crm_DML", prm);
            }

        }


    }
    protected string EscapeQuotes(string input)
    {
        return input.Replace("\"", "\\\"");
    }
}