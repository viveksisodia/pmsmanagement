
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
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
using System.Globalization;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
/// <summary>
/// Summary description for notifications
/// </summary>
public class notifications
{
    DataAccess da = new DataAccess();
    CommonFunction cmf = new CommonFunction();
    SqlParameter[] prm;
    DataSet ds = new DataSet();
    public function fns = new function();
    public notifications()
    {
        //
        // TODO: Add constructor logic here
        //
    }


    public void sendnotificationmo(int rid, int typeid, int ntfor)
    {

        try
        {
            string strsql = "select * from tbl_notificationtemplates where typeid='" + typeid + "' and ntfor='" + ntfor + "' and status=1 and archive=0";
            DataSet dsnot = da.GetDataSetInline(strsql);
            if (dsnot.Tables[0].Rows.Count > 0)
            {

                string notificationtext = dsnot.Tables[0].Rows[0]["temptext"].ToString();
                string isnotification = dsnot.Tables[0].Rows[0]["isnotification"].ToString();
                string isemail = dsnot.Tables[0].Rows[0]["isemail"].ToString();
                string issms = dsnot.Tables[0].Rows[0]["issms"].ToString();
                string uid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();

                string MOno = "";
                string MODate = "";
                string startDate = "";
                string Enddate = "";
                string SOno = "";
                string RequestNo = "";
                string ProductDetails = "";

                SqlParameter[] prmpurch = new SqlParameter[3];
                prmpurch[0] = new SqlParameter("@id", rid);
                prmpurch[1] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                prmpurch[2] = new SqlParameter("@type", "selectbyidnotification");
                DataSet dsstrsqlpurch = da.GetDataSet("tbl_manufacturingorderdetails_usp", prmpurch);
                if (dsstrsqlpurch.Tables[0].Rows.Count > 0)
                {




                    //  string companyname = dsstrsqlpurch.Tables[0].Rows[0]["txtcontactdisplayname"].ToString();
                    // lbltitle.Text = fns.getltext("Email Quotation To " + companyname, "2");
                    MOno = dsstrsqlpurch.Tables[0].Rows[0]["mocode"].ToString();
                    MODate = dsstrsqlpurch.Tables[0].Rows[0]["cdate"].ToString();
                    startDate = dsstrsqlpurch.Tables[0].Rows[0]["eststartdate"].ToString();
                    Enddate = dsstrsqlpurch.Tables[0].Rows[0]["estenddate"].ToString();
                    SOno = dsstrsqlpurch.Tables[0].Rows[0]["Estimateno"].ToString();
                    //  RequestNo = dsstrsqlpurch.Tables[0].Rows[0]["requesid"].ToString();
                    ProductDetails = dsstrsqlpurch.Tables[0].Rows[0]["pname"].ToString();
                    uid = dsstrsqlpurch.Tables[0].Rows[0]["uid"].ToString();

                    notificationtext = notificationtext.Replace("{####MOno####}", MOno.ToString()
                ).Replace("{####MODate####}", MODate.ToString()
                ).Replace("{####startDate####}", startDate.ToString()
                ).Replace("{####Enddate####}", Enddate.ToString()
                ).Replace("{####SOno####}", SOno.ToString()
                ).Replace("{####RequestNo####}", RequestNo.ToString()
                ).Replace("{####ProductDetails####}", ProductDetails.ToString());

                }
                string nuid = "0";
                string isview = dsnot.Tables[0].Rows[0]["isview"].ToString();


                char[] cha = { ',' };
                char[] cha1 = { '~' };

                string strsqlitem = "select * from tbl_notificationsetupemp where type='" + typeid + "'  and ntfor='" + ntfor + "' and status=1 and archive=0  order by aLevel ";
                DataSet dsadd = da.GetDataSetInline(strsqlitem);
                if (dsadd.Tables[0].Rows.Count > 0)
                {
                    functions fnsm = new functions();
                    string emailids = "";

                    for (int i = 0; i < dsadd.Tables[0].Rows.Count; i++)
                    {

                        string[] arr = dsadd.Tables[0].Rows[i]["hdnempids"].ToString().Split(cha, StringSplitOptions.None);

                        for (int j = 0; j < arr.Length; j++)
                        {
                            nuid = "0";
                            if (arr[j].ToString() != string.Empty)
                            {
                                string[] arrlist = arr[j].ToString().Split(cha1, StringSplitOptions.None);

                                if (arrlist.Length > 1)
                                {
                                    if (arrlist[1].ToString() == "1")
                                    {

                                        if (arrlist[0].ToString() == "1")
                                        {
                                            nuid = fns.teamleaderid(uid);

                                        }
                                        if (arrlist[0].ToString() == "2")
                                        {
                                            nuid = fns.l2managerid(uid);
                                        }
                                    }
                                    else
                                    {
                                        nuid = arrlist[0].ToString();

                                    }

                                    if (nuid != "0")
                                    {
                                        if (isnotification == "1")
                                        {

                                            SqlParameter[] prmn = new SqlParameter[12];
                                            prmn[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                                            prmn[1] = new SqlParameter("@uid", uid);
                                            prmn[2] = new SqlParameter("@nuid", nuid);
                                            prmn[3] = new SqlParameter("@ndatetime", da.getcdatetime());
                                            prmn[4] = new SqlParameter("@notificationtext", notificationtext);
                                            prmn[5] = new SqlParameter("@rid", rid);
                                            prmn[6] = new SqlParameter("@typeid", typeid);
                                            prmn[7] = new SqlParameter("@isnew", "1");
                                            prmn[8] = new SqlParameter("@isread", "0");
                                            prmn[9] = new SqlParameter("@isreaddate", DBNull.Value);
                                            prmn[10] = new SqlParameter("@isview", isview);
                                            prmn[11] = new SqlParameter("@ntfor", ntfor);
                                            da.executeDMLQuery("tbl_pnotificationdetailsinsert", prmn);
                                        }
                                        if (isemail == "1")
                                        {

                                            try
                                            {
                                                string strtoemail = fnsm.getEmpemailById(nuid);
                                                if (strtoemail != string.Empty)
                                                {

                                                    if (emailids != string.Empty)
                                                    {
                                                        emailids = emailids + "," + strtoemail;

                                                    }
                                                    else
                                                    {
                                                        emailids = strtoemail;

                                                    }
                                                }
                                            }
                                            catch
                                            {

                                            }


                                        }

                                    }


                                }

                            }
                        }
                    }


                    if (isemail == "1")
                    {
                        try
                        {

                            SendMail sm = new SendMail();

                            string to = "";
                            string from = "";
                            string cc = "";
                            string bcc = "";
                            string sub = "";
                            string body = "";
                            string files = "";
                            string[] chkdd = { "####" };
                            string[] filesdata = files.Split(chkdd, StringSplitOptions.None);

                            from = fnsm.getEmpemailById(uid);
                            to = emailids;

                            cc = "";
                            bcc = "";
                            sub = "MO Notification by " + fnsm.getEmpNameById(uid);
                            body = notificationtext;
                            if (to != string.Empty)
                            {
                                sm.SendMailtoUser(to, from, cc, bcc, sub, body, filesdata);
                            }
                        }
                        catch
                        {

                        }
                    }

                }


            }
        }
        catch
        {

        }

    }
    public void sendnotificationMaterialRequisition(int rid, int typeid, int ntfor)
    {

        try
        {
            string strsql = "select * from tbl_notificationtemplates where typeid='" + typeid + "' and ntfor='" + ntfor + "' and status=1 and archive=0";
            DataSet dsnot = da.GetDataSetInline(strsql);


            if (dsnot.Tables[0].Rows.Count > 0)
            {

                string notificationtext = dsnot.Tables[0].Rows[0]["temptext"].ToString();
                string isnotification = dsnot.Tables[0].Rows[0]["isnotification"].ToString();
                string isemail = dsnot.Tables[0].Rows[0]["isemail"].ToString();
                string issms = dsnot.Tables[0].Rows[0]["issms"].ToString();
                string uid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();

                string MOno = "";
                string MODate = "";
                string startDate = "";
                string Enddate = "";
                string SOno = "";
                string RequestNo = "";
                string ProductDetails = "";

                SqlParameter[] prmpurch = new SqlParameter[2];
                prmpurch[0] = new SqlParameter("@id", rid);
                prmpurch[1] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                DataSet dsstrsqlpurch = da.GetDataSet("tbl_rawmeterialrequestbyid", prmpurch);
                if (dsstrsqlpurch.Tables[0].Rows.Count > 0)
                {




                    //  string companyname = dsstrsqlpurch.Tables[0].Rows[0]["txtcontactdisplayname"].ToString();
                    // lbltitle.Text = fns.getltext("Email Quotation To " + companyname, "2");
                    MOno = dsstrsqlpurch.Tables[0].Rows[0]["mocode"].ToString();
                    MODate = dsstrsqlpurch.Tables[0].Rows[0]["modate"].ToString();
                    startDate = dsstrsqlpurch.Tables[0].Rows[0]["eststartdate"].ToString();
                    Enddate = dsstrsqlpurch.Tables[0].Rows[0]["estenddate"].ToString();
                    SOno = dsstrsqlpurch.Tables[0].Rows[0]["Estimateno"].ToString();
                    RequestNo = dsstrsqlpurch.Tables[0].Rows[0]["requesid"].ToString();
                    ProductDetails = dsstrsqlpurch.Tables[0].Rows[0]["pname"].ToString();
                    uid = dsstrsqlpurch.Tables[0].Rows[0]["uid"].ToString();

                    notificationtext = notificationtext.Replace("{####MOno####}", MOno.ToString()
                ).Replace("{####MODate####}", MODate.ToString()
                ).Replace("{####startDate####}", startDate.ToString()
                ).Replace("{####Enddate####}", Enddate.ToString()
                ).Replace("{####SOno####}", SOno.ToString()
                ).Replace("{####RequestNo####}", RequestNo.ToString()
                ).Replace("{####ProductDetails####}", ProductDetails.ToString());

                }
                string nuid = "0";
                string isview = dsnot.Tables[0].Rows[0]["isview"].ToString();


                char[] cha = { ',' };
                char[] cha1 = { '~' };

                string strsqlitem = "select * from tbl_notificationsetupemp where type='" + typeid + "'  and ntfor='" + ntfor + "' and status=1 and archive=0  order by aLevel ";
                DataSet dsadd = da.GetDataSetInline(strsqlitem);
                if (dsadd.Tables[0].Rows.Count > 0)
                {
                    functions fnsm = new functions();
                    string emailids = "";
                    for (int i = 0; i < dsadd.Tables[0].Rows.Count; i++)
                    {

                        string[] arr = dsadd.Tables[0].Rows[i]["hdnempids"].ToString().Split(cha, StringSplitOptions.None);

                        for (int j = 0; j < arr.Length; j++)
                        {
                            nuid = "0";
                            if (arr[j].ToString() != string.Empty)
                            {
                                string[] arrlist = arr[j].ToString().Split(cha1, StringSplitOptions.None);

                                if (arrlist.Length > 1)
                                {
                                    if (arrlist[1].ToString() == "1")
                                    {

                                        if (arrlist[0].ToString() == "1")
                                        {
                                            nuid = fns.teamleaderid(uid);

                                        }
                                        if (arrlist[0].ToString() == "2")
                                        {
                                            nuid = fns.l2managerid(uid);
                                        }
                                    }
                                    else
                                    {
                                        nuid = arrlist[0].ToString();

                                    }

                                    if (nuid != "0")
                                    {
                                        if (isnotification == "1")
                                        {

                                            SqlParameter[] prmn = new SqlParameter[12];
                                            prmn[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                                            prmn[1] = new SqlParameter("@uid", uid);
                                            prmn[2] = new SqlParameter("@nuid", nuid);
                                            prmn[3] = new SqlParameter("@ndatetime", da.getcdatetime());
                                            prmn[4] = new SqlParameter("@notificationtext", notificationtext);
                                            prmn[5] = new SqlParameter("@rid", rid);
                                            prmn[6] = new SqlParameter("@typeid", typeid);
                                            prmn[7] = new SqlParameter("@isnew", "1");
                                            prmn[8] = new SqlParameter("@isread", "0");
                                            prmn[9] = new SqlParameter("@isreaddate", DBNull.Value);
                                            prmn[10] = new SqlParameter("@isview", isview);
                                            prmn[11] = new SqlParameter("@ntfor", ntfor);
                                            da.executeDMLQuery("tbl_pnotificationdetailsinsert", prmn);
                                        }
                                        if (isemail == "1")
                                        {

                                            try
                                            {
                                                string strtoemail = fnsm.getEmpemailById(nuid);
                                                if (strtoemail != string.Empty)
                                                {

                                                    if (emailids != string.Empty)
                                                    {
                                                        emailids = emailids + "," + strtoemail;

                                                    }
                                                    else
                                                    {
                                                        emailids = strtoemail;

                                                    }
                                                }
                                            }
                                            catch
                                            {

                                            }


                                        }

                                       
                                    }


                                }

                            }
                        }
                    }

                    if (isemail == "1")
                    {
                        try
                        {

                            SendMail sm = new SendMail();

                            string to = "";
                            string from = "";
                            string cc = "";
                            string bcc = "";
                            string sub = "";
                            string body = "";
                            string files = "";
                            string[] chkdd = { "####" };
                            string[] filesdata = files.Split(chkdd, StringSplitOptions.None);

                            from = fnsm.getEmpemailById(uid);
                            to = emailids;

                            cc = "";
                            bcc = "";
                            sub = "Material Requisition Notification by " + fnsm.getEmpNameById(uid);
                            body = notificationtext;
                            if (to != string.Empty)
                            {
                                sm.SendMailtoUser(to, from, cc, bcc, sub, body, filesdata);
                            }
                        }
                        catch
                        {

                        }
                    }


                }


            }
        }
        catch
        {

        }

    }


    public void sendnotification(int rid, int typeid, int ntfor)
    {
          
        try
        {

            string strsql = "select * from tbl_notificationtemplates where typeid='" + typeid + "' and ntfor='" + ntfor + "' and status=1 and archive=0";
            DataSet dsnot = da.GetDataSetInline(strsql);
            if (dsnot.Tables[0].Rows.Count > 0)
            {

                string notificationtext = dsnot.Tables[0].Rows[0]["temptext"].ToString();

                string isnotification = dsnot.Tables[0].Rows[0]["isnotification"].ToString();
                string isemail = dsnot.Tables[0].Rows[0]["isemail"].ToString();
                string issms = dsnot.Tables[0].Rows[0]["issms"].ToString();
                string uid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();

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


                SqlParameter[] prmpurch = new SqlParameter[2];
                prmpurch[0] = new SqlParameter("@id", rid);
                prmpurch[1] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                DataSet dsstrsqlpurch = da.GetDataSet("tbl_Quotationorderdetailsselectdetails", prmpurch);
                if (dsstrsqlpurch.Tables[0].Rows.Count > 0)
                {
                    string companyname = dsstrsqlpurch.Tables[0].Rows[0]["txtcontactdisplayname"].ToString();
                    // lbltitle.Text = fns.getltext("Email Quotation To " + companyname, "2");
                    pono = dsstrsqlpurch.Tables[0].Rows[0]["Estimateno"].ToString();
                    uid= dsstrsqlpurch.Tables[0].Rows[0]["uid"].ToString();
                    // lblfdffilename.Text = dsstrsqlpurch.Tables[0].Rows[0]["Estimateno"].ToString().Replace(@"/", "_").Replace(@"\", "_");
                    // lblfdffilename.Text = dsstrsqlpurch.Tables[0].Rows[0]["Estimateno"].ToString();
                    IssuedOn = Convert.ToDateTime(dsstrsqlpurch.Tables[0].Rows[0]["EstimateDate"].ToString()).ToString(fns.getdateformat());
                    PurchaseOrderNumber = dsstrsqlpurch.Tables[0].Rows[0]["Estimateno"].ToString();
                    PurchaseOrderRef = dsstrsqlpurch.Tables[0].Rows[0]["Reference"].ToString();
                    FormattedTotal = dsstrsqlpurch.Tables[0].Rows[0]["totalamount"].ToString();
                    Total = dsstrsqlpurch.Tables[0].Rows[0]["totalamount"].ToString();
                    DeliveryDate = Convert.ToDateTime(dsstrsqlpurch.Tables[0].Rows[0]["ExpiryDate"].ToString()).ToString(fns.getdateformat());

                    OrganizationUser = fns.getusersname(dsstrsqlpurch.Tables[0].Rows[0]["uid"].ToString());



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
                string nuid = "0";
                string isview = dsnot.Tables[0].Rows[0]["isview"].ToString();


                char[] cha = { ',' };
                char[] cha1 = { '~' };

                string strsqlitem = "select * from tbl_notificationsetupemp where type='" + typeid + "'  and ntfor='" + ntfor + "' and status=1 and archive=0  order by aLevel ";
                DataSet dsadd = da.GetDataSetInline(strsqlitem);
                if (dsadd.Tables[0].Rows.Count > 0)
                {
                    functions fnsm = new functions();
                    string emailids = "";
                    for (int i = 0; i < dsadd.Tables[0].Rows.Count; i++)
                    {

                        string[] arr = dsadd.Tables[0].Rows[i]["hdnempids"].ToString().Split(cha, StringSplitOptions.None);

                        for (int j = 0; j < arr.Length; j++)
                        {
                            nuid = "0";
                            if (arr[j].ToString() != string.Empty)
                            {
                                string[] arrlist = arr[j].ToString().Split(cha1, StringSplitOptions.None);

                                if (arrlist.Length > 1)
                                {
                                    if (arrlist[1].ToString() == "1")
                                    {

                                        if (arrlist[0].ToString() == "1")
                                        {
                                            nuid = fns.teamleaderid(uid);

                                        }
                                        if (arrlist[0].ToString() == "2")
                                        {
                                            nuid = fns.l2managerid(uid);
                                        }
                                    }
                                    else
                                    {
                                        nuid = arrlist[0].ToString();

                                    }

                                    if (nuid != "0")
                                    {
                                        if (isnotification == "1")
                                        {

                                            SqlParameter[] prmn = new SqlParameter[12];
                                            prmn[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                                            prmn[1] = new SqlParameter("@uid", uid);
                                            prmn[2] = new SqlParameter("@nuid", nuid);
                                            prmn[3] = new SqlParameter("@ndatetime", da.getcdatetime());
                                            prmn[4] = new SqlParameter("@notificationtext", notificationtext);
                                            prmn[5] = new SqlParameter("@rid", rid);
                                            prmn[6] = new SqlParameter("@typeid", typeid);
                                            prmn[7] = new SqlParameter("@isnew", "1");
                                            prmn[8] = new SqlParameter("@isread", "0");
                                            prmn[9] = new SqlParameter("@isreaddate", DBNull.Value);
                                            prmn[10] = new SqlParameter("@isview", isview);
                                            prmn[11] = new SqlParameter("@ntfor", ntfor);
                                            da.executeDMLQuery("tbl_pnotificationdetailsinsert", prmn);
                                        }
                                        if (isemail == "1")
                                        {

                                            try
                                            {
                                                string strtoemail = fnsm.getEmpemailById(nuid);
                                                if (strtoemail != string.Empty)
                                                {

                                                    if (emailids != string.Empty)
                                                    {
                                                        emailids = emailids + "," + strtoemail;

                                                    }
                                                    else
                                                    {
                                                        emailids = strtoemail;

                                                    }
                                                }
                                            }
                                            catch
                                            {

                                            }


                                        }
                                       
                                    }


                                }

                            }
                        }
                    }

                    if (isemail == "1")
                    {
                        try
                        {

                            SendMail sm = new SendMail();
                           
                            string to = "";
                            string from = "";
                            string cc = "";
                            string bcc = "";
                            string sub = "";
                            string body = "";
                            string files = "";
                            string[] chkdd = { "####" };
                            string[] filesdata = files.Split(chkdd, StringSplitOptions.None);

                            from = fnsm.getEmpemailById(uid);
                            to = emailids;

                            cc = "";
                            bcc = "";
                            sub = "Quotation Notification by " + fnsm.getEmpNameById(uid);
                            body = notificationtext;
                            if (to != string.Empty)
                            {
                                sm.SendMailtoUser(to, from, cc, bcc, sub, body, filesdata);
                            }
                        }
                        catch
                        {

                        }
                    }


                }


            }
        }
        catch
        {

        }

    }

    public void sendnotificationsalesorder(int rid, int typeid, int ntfor)
    {

        try
        {
            string strsql = "select * from tbl_notificationtemplates where typeid='" + typeid + "' and ntfor='" + ntfor + "' and status=1 and archive=0";
            DataSet dsnot = da.GetDataSetInline(strsql);
            if (dsnot.Tables[0].Rows.Count > 0)
            {

                string notificationtext = dsnot.Tables[0].Rows[0]["temptext"].ToString();

                string isnotification = dsnot.Tables[0].Rows[0]["isnotification"].ToString();
                string isemail = dsnot.Tables[0].Rows[0]["isemail"].ToString();
                string issms = dsnot.Tables[0].Rows[0]["issms"].ToString();
                string uid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();

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


                SqlParameter[] prmpurch = new SqlParameter[2];
                prmpurch[0] = new SqlParameter("@id", rid);
                prmpurch[1] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                DataSet dsstrsqlpurch = da.GetDataSet("tbl_SalesOrderdetailsselectdetails", prmpurch);
                if (dsstrsqlpurch.Tables[0].Rows.Count > 0)
                {
                    string companyname = dsstrsqlpurch.Tables[0].Rows[0]["txtcontactdisplayname"].ToString();
                    uid = dsstrsqlpurch.Tables[0].Rows[0]["uid"].ToString();
                    // lbltitle.Text = fns.getltext("Email Quotation To " + companyname, "2");
                    pono = dsstrsqlpurch.Tables[0].Rows[0]["Estimateno"].ToString();
                    // lblfdffilename.Text = dsstrsqlpurch.Tables[0].Rows[0]["Estimateno"].ToString().Replace(@"/", "_").Replace(@"\", "_");
                    // lblfdffilename.Text = dsstrsqlpurch.Tables[0].Rows[0]["Estimateno"].ToString();
                    IssuedOn = Convert.ToDateTime(dsstrsqlpurch.Tables[0].Rows[0]["EstimateDate"].ToString()).ToString(fns.getdateformat());
                    PurchaseOrderNumber = dsstrsqlpurch.Tables[0].Rows[0]["Estimateno"].ToString();
                    PurchaseOrderRef = dsstrsqlpurch.Tables[0].Rows[0]["Reference"].ToString();
                    FormattedTotal = dsstrsqlpurch.Tables[0].Rows[0]["totalamount"].ToString();
                    Total = dsstrsqlpurch.Tables[0].Rows[0]["totalamount"].ToString();
                    DeliveryDate = Convert.ToDateTime(dsstrsqlpurch.Tables[0].Rows[0]["ExpiryDate"].ToString()).ToString(fns.getdateformat());

                    OrganizationUser = fns.getusersname(dsstrsqlpurch.Tables[0].Rows[0]["uid"].ToString());



                    strsql = "select * from tbl_contactsdetails where id='" + dsstrsqlpurch.Tables[0].Rows[0]["cusid"].ToString() + "' and status=1 and archive=0 and regid='" +HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "'";
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
                ).Replace("{####OrderDate####}", IssuedOn.ToString()
                ).Replace("{####SalesOrderNumber####}", PurchaseOrderNumber.ToString()
                ).Replace("{####SalesRefNumber####}", PurchaseOrderRef.ToString()
                ).Replace("{####FormattedTotal####}", FormattedTotal.ToString()
                ).Replace("{####SalesTotal####}", Total.ToString()
                ).Replace("{####DeliveryDate####}", DeliveryDate.ToString()
                ).Replace("{####CurrencyCode####}", CurrencyCode.ToString()
                ).Replace("{####OrganizationUser####}", OrganizationUser.ToString()
                ).Replace("{####OrganizationName####}", OrganizationName.ToString()
                ).Replace("{####OrgLogo####}", OrgLogo.ToString());
                }
                string nuid = "0";
                string isview = dsnot.Tables[0].Rows[0]["isview"].ToString();


                char[] cha = { ',' };
                char[] cha1 = { '~' };

                string strsqlitem = "select * from tbl_notificationsetupemp where type='" + typeid + "'  and ntfor='" + ntfor + "' and status=1 and archive=0  order by aLevel ";
                DataSet dsadd = da.GetDataSetInline(strsqlitem);
                if (dsadd.Tables[0].Rows.Count > 0)
                {
                    functions fnsm = new functions();
                    string emailids = "";

                    for (int i = 0; i < dsadd.Tables[0].Rows.Count; i++)
                    {

                        string[] arr = dsadd.Tables[0].Rows[i]["hdnempids"].ToString().Split(cha, StringSplitOptions.None);

                        for (int j = 0; j < arr.Length; j++)
                        {
                            nuid = "0";
                            if (arr[j].ToString() != string.Empty)
                            {
                                string[] arrlist = arr[j].ToString().Split(cha1, StringSplitOptions.None);

                                if (arrlist.Length > 1)
                                {
                                    if (arrlist[1].ToString() == "1")
                                    {

                                        if (arrlist[0].ToString() == "1")
                                        {
                                            nuid = fns.teamleaderid(uid);

                                        }
                                        if (arrlist[0].ToString() == "2")
                                        {
                                            nuid = fns.l2managerid(uid);
                                        }
                                    }
                                    else
                                    {
                                        nuid = arrlist[0].ToString();

                                    }

                                    if (nuid != "0")
                                    {
                                        if (isnotification == "1")
                                        {

                                            SqlParameter[] prmn = new SqlParameter[12];
                                            prmn[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                                            prmn[1] = new SqlParameter("@uid", uid);
                                            prmn[2] = new SqlParameter("@nuid", nuid);
                                            prmn[3] = new SqlParameter("@ndatetime", da.getcdatetime());
                                            prmn[4] = new SqlParameter("@notificationtext", notificationtext);
                                            prmn[5] = new SqlParameter("@rid", rid);
                                            prmn[6] = new SqlParameter("@typeid", typeid);
                                            prmn[7] = new SqlParameter("@isnew", "1");
                                            prmn[8] = new SqlParameter("@isread", "0");
                                            prmn[9] = new SqlParameter("@isreaddate", DBNull.Value);
                                            prmn[10] = new SqlParameter("@isview", isview);
                                            prmn[11] = new SqlParameter("@ntfor", ntfor);
                                            da.executeDMLQuery("tbl_pnotificationdetailsinsert", prmn);
                                        }
                                        if (isemail == "1")
                                        {

                                            try
                                            {
                                                string strtoemail = fnsm.getEmpemailById(nuid);
                                                if (strtoemail != string.Empty)
                                                {

                                                    if (emailids != string.Empty)
                                                    {
                                                        emailids = emailids + "," + strtoemail;

                                                    }
                                                    else
                                                    {
                                                        emailids = strtoemail;

                                                    }
                                                }
                                            }
                                            catch
                                            {

                                            }

                                           
                                        }
                                    }


                                }

                            }
                        }
                    }
                    if (isemail == "1")
                    {
                        try
                        {

                            SendMail sm = new SendMail();

                            string to = "";
                            string from = "";
                            string cc = "";
                            string bcc = "";
                            string sub = "";
                            string body = "";
                            string files = "";
                            string[] chkdd = { "####" };
                            string[] filesdata = files.Split(chkdd, StringSplitOptions.None);

                            from = fnsm.getEmpemailById(uid);
                            to = emailids;

                            cc = "";
                            bcc = "";
                            sub = "Sales Order Notification by " + fnsm.getEmpNameById(uid);
                            body = notificationtext;
                            if (to != string.Empty)
                            {
                                sm.SendMailtoUser(to, from, cc, bcc, sub, body, filesdata);
                            }
                        }
                        catch
                        {

                        }

                    }
                }


            }
        }
        catch
        {

        }

    }

    public void sendnotificationinvoice(int rid, int typeid, int ntfor)
    {

        try
        {
            string strsql = "select * from tbl_notificationtemplates where typeid='" + typeid + "' and ntfor='" + ntfor + "' and status=1 and archive=0";
        DataSet dsnot = da.GetDataSetInline(strsql);


        if (dsnot.Tables[0].Rows.Count > 0)
        {

            string notificationtext = dsnot.Tables[0].Rows[0]["temptext"].ToString();

            string isnotification = dsnot.Tables[0].Rows[0]["isnotification"].ToString();
            string isemail = dsnot.Tables[0].Rows[0]["isemail"].ToString();
            string issms = dsnot.Tables[0].Rows[0]["issms"].ToString();
            string uid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();

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


            SqlParameter[] prmpurch = new SqlParameter[2];
            prmpurch[0] = new SqlParameter("@id", rid);
            prmpurch[1] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
            DataSet dsstrsqlpurch = da.GetDataSet("tbl_invoiceorderdetailsselectdetails", prmpurch);
            if (dsstrsqlpurch.Tables[0].Rows.Count > 0)
            {


                string companyname = dsstrsqlpurch.Tables[0].Rows[0]["txtcontactdisplayname"].ToString();
                // lbltitle.Text = fns.getltext("Email Quotation To " + companyname, "2");
                pono = dsstrsqlpurch.Tables[0].Rows[0]["Estimateno"].ToString();
                uid = dsstrsqlpurch.Tables[0].Rows[0]["uid"].ToString();
                // lblfdffilename.Text = dsstrsqlpurch.Tables[0].Rows[0]["Estimateno"].ToString().Replace(@"/", "_").Replace(@"\", "_");
                // lblfdffilename.Text = dsstrsqlpurch.Tables[0].Rows[0]["Estimateno"].ToString();
                IssuedOn = Convert.ToDateTime(dsstrsqlpurch.Tables[0].Rows[0]["EstimateDate"].ToString()).ToString(fns.getdateformat());
                PurchaseOrderNumber = dsstrsqlpurch.Tables[0].Rows[0]["Estimateno"].ToString();
                PurchaseOrderRef = dsstrsqlpurch.Tables[0].Rows[0]["Reference"].ToString();
                FormattedTotal = dsstrsqlpurch.Tables[0].Rows[0]["totalamount"].ToString();
                Total = dsstrsqlpurch.Tables[0].Rows[0]["totalamount"].ToString();
                DeliveryDate = Convert.ToDateTime(dsstrsqlpurch.Tables[0].Rows[0]["ExpiryDate"].ToString()).ToString(fns.getdateformat());

                OrganizationUser = fns.getusersname(dsstrsqlpurch.Tables[0].Rows[0]["uid"].ToString());



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
            ).Replace("{####InvoiceDate####}", IssuedOn.ToString()
            ).Replace("{####InvoiceNumber####}", PurchaseOrderNumber.ToString()
            ).Replace("{####SalesOrderRef####}", PurchaseOrderRef.ToString()
            ).Replace("{####FormattedTotal####}", FormattedTotal.ToString()
            ).Replace("{####Total####}", Total.ToString()
            ).Replace("{####DueDate####}", DeliveryDate.ToString()
            ).Replace("{####CurrencyCode####}", CurrencyCode.ToString()
            ).Replace("{####OrganizationUser####}", OrganizationUser.ToString()
            ).Replace("{####OrganizationName####}", OrganizationName.ToString()
            ).Replace("{####OrgLogo####}", OrgLogo.ToString());
            }
            string nuid = "0";
            string isview = dsnot.Tables[0].Rows[0]["isview"].ToString();


            char[] cha = { ',' };
            char[] cha1 = { '~' };

            string strsqlitem = "select * from tbl_notificationsetupemp where type='" + typeid + "'  and ntfor='" + ntfor + "' and status=1 and archive=0  order by aLevel ";
            DataSet dsadd = da.GetDataSetInline(strsqlitem);
            if (dsadd.Tables[0].Rows.Count > 0)
            {
                    functions fnsm = new functions();
                    string emailids = "";

                    for (int i = 0; i < dsadd.Tables[0].Rows.Count; i++)
                {

                    string[] arr = dsadd.Tables[0].Rows[i]["hdnempids"].ToString().Split(cha, StringSplitOptions.None);

                    for (int j = 0; j < arr.Length; j++)
                    {
                        nuid = "0";
                        if (arr[j].ToString() != string.Empty)
                        {
                            string[] arrlist = arr[j].ToString().Split(cha1, StringSplitOptions.None);

                            if (arrlist.Length > 1)
                            {
                                if (arrlist[1].ToString() == "1")
                                {

                                    if (arrlist[0].ToString() == "1")
                                    {
                                        nuid = fns.teamleaderid(uid);

                                    }
                                    if (arrlist[0].ToString() == "2")
                                    {
                                        nuid = fns.l2managerid(uid);
                                    }
                                }
                                else
                                {
                                    nuid = arrlist[0].ToString();

                                }

                                if (nuid != "0")
                                {
                                    if (isnotification == "1")
                                    {

                                        SqlParameter[] prmn = new SqlParameter[12];
                                        prmn[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                                        prmn[1] = new SqlParameter("@uid", uid);
                                        prmn[2] = new SqlParameter("@nuid", nuid);
                                        prmn[3] = new SqlParameter("@ndatetime", da.getcdatetime());
                                        prmn[4] = new SqlParameter("@notificationtext", notificationtext);
                                        prmn[5] = new SqlParameter("@rid", rid);
                                        prmn[6] = new SqlParameter("@typeid", typeid);
                                        prmn[7] = new SqlParameter("@isnew", "1");
                                        prmn[8] = new SqlParameter("@isread", "0");
                                        prmn[9] = new SqlParameter("@isreaddate", DBNull.Value);
                                        prmn[10] = new SqlParameter("@isview", isview);
                                        prmn[11] = new SqlParameter("@ntfor", ntfor);
                                        da.executeDMLQuery("tbl_pnotificationdetailsinsert", prmn);
                                    }
                                        if (isemail == "1")
                                        {

                                            try
                                            {
                                                string strtoemail = fnsm.getEmpemailById(nuid);
                                                if (strtoemail != string.Empty)
                                                {

                                                    if (emailids != string.Empty)
                                                    {
                                                        emailids = emailids + "," + strtoemail;

                                                    }
                                                    else
                                                    {
                                                        emailids = strtoemail;

                                                    }
                                                }
                                            }
                                            catch
                                            {

                                            }


                                        }
                                       
                                }


                            }

                        }
                    }
                }

                    if (isemail == "1")
                    {
                        //try
                        //{

                        SendMail sm = new SendMail();
                         
                        string to = "";
                        string from = "";
                        string cc = "";
                        string bcc = "";
                        string sub = "";
                        string body = "";
                        string files = "";
                        string[] chkdd = { "####" };
                        string[] filesdata = files.Split(chkdd, StringSplitOptions.None);

                        from = fnsm.getEmpemailById(uid);
                        to = emailids;

                        cc = "";
                        bcc = "";
                        sub = "Invoice Notification by " + fnsm.getEmpNameById(uid);
                        body = notificationtext;
                        if (to != string.Empty)
                        {
                            sm.SendMailtoUser(to, from, cc, bcc, sub, body, filesdata);
                        }
                        //}
                        //catch
                        //{

                        //}
                    }


                }


        }
    }
        catch
        {

        }

    }

    public void sendnotificationcreditnotes(int rid, int typeid, int ntfor)
    {

        try
        {
            string strsql = "select * from tbl_notificationtemplates where typeid='" + typeid + "' and ntfor='" + ntfor + "' and status=1 and archive=0";
            DataSet dsnot = da.GetDataSetInline(strsql);


            if (dsnot.Tables[0].Rows.Count > 0)
            {

                string notificationtext = dsnot.Tables[0].Rows[0]["temptext"].ToString();

                string isnotification = dsnot.Tables[0].Rows[0]["isnotification"].ToString();
                string isemail = dsnot.Tables[0].Rows[0]["isemail"].ToString();
                string issms = dsnot.Tables[0].Rows[0]["issms"].ToString();
                string uid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();

                string CreditNoteDate = "&nbsp;";
                string CreditNoteNumber = "&nbsp;";
                string CustomerName = "&nbsp;";
                string CustomerFirstName = "&nbsp;";
                string CustomerLastName = "&nbsp;";
                string CustomerDepartment = "&nbsp;";
                string CustomerDesignation = "&nbsp;";



                SqlParameter[] prmpurch = new SqlParameter[2];
                prmpurch[0] = new SqlParameter("@id", rid);
                prmpurch[1] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                DataSet dsstrsqlpurch = da.GetDataSet("tbl_CreditNotesdetailsselectnotification", prmpurch);
                if (dsstrsqlpurch.Tables[0].Rows.Count > 0)
                {



                    // lbltitle.Text = fns.getltext("Email Quotation To " + companyname, "2");
                    CreditNoteNumber = dsstrsqlpurch.Tables[0].Rows[0]["Estimateno"].ToString();
                    uid = dsstrsqlpurch.Tables[0].Rows[0]["uid"].ToString();
                    // lblfdffilename.Text = dsstrsqlpurch.Tables[0].Rows[0]["Estimateno"].ToString().Replace(@"/", "_").Replace(@"\", "_");
                    // lblfdffilename.Text = dsstrsqlpurch.Tables[0].Rows[0]["Estimateno"].ToString();
                    CreditNoteDate = Convert.ToDateTime(dsstrsqlpurch.Tables[0].Rows[0]["EstimateDate"].ToString()).ToString(fns.getdateformat());


                    strsql = "select * from tbl_contactsdetails where id='" + dsstrsqlpurch.Tables[0].Rows[0]["cusid"].ToString() + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "'";
                    DataSet ds = da.GetDataSetInline(strsql);
                    if (ds.Tables[0].Rows.Count > 0)
                    {


                        CustomerName = ds.Tables[0].Rows[0]["txtcompanyname"].ToString();

                        CustomerFirstName = ds.Tables[0].Rows[0]["txtfirstname"].ToString();
                        CustomerLastName = ds.Tables[0].Rows[0]["txtlastname"].ToString();
                        CustomerDepartment = ds.Tables[0].Rows[0]["txtDepartment"].ToString();
                        CustomerDesignation = ds.Tables[0].Rows[0]["txtDesignation"].ToString();




                    }










                    notificationtext = notificationtext.Replace("{####CreditNoteDate####}", CreditNoteDate.ToString()
                      ).Replace("{####CreditNoteNumber####}", CreditNoteNumber.ToString()
                      ).Replace("{####CustomerName####}", CustomerName.ToString()
                      ).Replace("{####CustomerFirstName####}", CustomerFirstName.ToString()
                      ).Replace("{####CustomerLastName####}", CustomerLastName.ToString()
                      ).Replace("{####CustomerDepartment####}", CustomerDepartment.ToString()
                      ).Replace("{####CustomerDesignation####}", CustomerDesignation.ToString()
                      );
                }
                string nuid = "0";
                string isview = dsnot.Tables[0].Rows[0]["isview"].ToString();


                char[] cha = { ',' };
                char[] cha1 = { '~' };

                string strsqlitem = "select * from tbl_notificationsetupemp where type='" + typeid + "'  and ntfor='" + ntfor + "' and status=1 and archive=0  order by aLevel ";
                DataSet dsadd = da.GetDataSetInline(strsqlitem);
                if (dsadd.Tables[0].Rows.Count > 0)
                {
                    functions fnsm = new functions();
                    string emailids = "";

                    for (int i = 0; i < dsadd.Tables[0].Rows.Count; i++)
                    {

                        string[] arr = dsadd.Tables[0].Rows[i]["hdnempids"].ToString().Split(cha, StringSplitOptions.None);

                        for (int j = 0; j < arr.Length; j++)
                        {
                            nuid = "0";
                            if (arr[j].ToString() != string.Empty)
                            {
                                string[] arrlist = arr[j].ToString().Split(cha1, StringSplitOptions.None);

                                if (arrlist.Length > 1)
                                {
                                    if (arrlist[1].ToString() == "1")
                                    {

                                        if (arrlist[0].ToString() == "1")
                                        {
                                            nuid = fns.teamleaderid(uid);

                                        }
                                        if (arrlist[0].ToString() == "2")
                                        {
                                            nuid = fns.l2managerid(uid);
                                        }
                                    }
                                    else
                                    {
                                        nuid = arrlist[0].ToString();

                                    }

                                    if (nuid != "0")
                                    {
                                        if (isnotification == "1")
                                        {

                                            SqlParameter[] prmn = new SqlParameter[12];
                                            prmn[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                                            prmn[1] = new SqlParameter("@uid", uid);
                                            prmn[2] = new SqlParameter("@nuid", nuid);
                                            prmn[3] = new SqlParameter("@ndatetime", da.getcdatetime());
                                            prmn[4] = new SqlParameter("@notificationtext", notificationtext);
                                            prmn[5] = new SqlParameter("@rid", rid);
                                            prmn[6] = new SqlParameter("@typeid", typeid);
                                            prmn[7] = new SqlParameter("@isnew", "1");
                                            prmn[8] = new SqlParameter("@isread", "0");
                                            prmn[9] = new SqlParameter("@isreaddate", DBNull.Value);
                                            prmn[10] = new SqlParameter("@isview", isview);
                                            prmn[11] = new SqlParameter("@ntfor", ntfor);
                                            da.executeDMLQuery("tbl_pnotificationdetailsinsert", prmn);
                                        }
                                        if (isemail == "1")
                                        {

                                            try
                                            {
                                                string strtoemail = fnsm.getEmpemailById(nuid);
                                                if (strtoemail != string.Empty)
                                                {

                                                    if (emailids != string.Empty)
                                                    {
                                                        emailids = emailids + "," + strtoemail;

                                                    }
                                                    else
                                                    {
                                                        emailids = strtoemail;

                                                    }
                                                }
                                            }
                                            catch
                                            {

                                            }


                                        }
                                      
                                    }


                                }

                            }
                        }
                    }


                    if (isemail == "1")
                    {
                        //try
                        //{

                        SendMail sm = new SendMail();
                         
                        string to = "";
                        string from = "";
                        string cc = "";
                        string bcc = "";
                        string sub = "";
                        string body = "";
                        string files = "";
                        string[] chkdd = { "####" };
                        string[] filesdata = files.Split(chkdd, StringSplitOptions.None);

                        from = fnsm.getEmpemailById(uid);
                        to = emailids;

                        cc = "";
                        bcc = "";
                        sub = "Creadit Note Notification by " + fnsm.getEmpNameById(uid);
                        body = notificationtext;
                        if (to != string.Empty)
                        {
                            sm.SendMailtoUser(to, from, cc, bcc, sub, body, filesdata);
                        }
                        //}
                        //catch
                        //{

                        //}
                    }

                }


            }
        }
        catch
        {

        }

    }


    public void sendnotificationpurchase(int rid, int typeid, int ntfor)
    {

        try
        {
            string strsql = "select * from tbl_notificationtemplates where typeid='" + typeid + "' and ntfor='" + ntfor + "' and status=1 and archive=0";
            DataSet dsnot = da.GetDataSetInline(strsql);


            if (dsnot.Tables[0].Rows.Count > 0)
            {

                string notificationtext = dsnot.Tables[0].Rows[0]["temptext"].ToString();

                string isnotification = dsnot.Tables[0].Rows[0]["isnotification"].ToString();
                string isemail = dsnot.Tables[0].Rows[0]["isemail"].ToString();
                string issms = dsnot.Tables[0].Rows[0]["issms"].ToString();
                string uid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();

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


                SqlParameter[] prmpurch = new SqlParameter[2];
                prmpurch[0] = new SqlParameter("@id", rid);
                prmpurch[1] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                DataSet dsstrsqlpurch = da.GetDataSet("tbl_purchaseorderdetailsselectdetails", prmpurch);
                if (dsstrsqlpurch.Tables[0].Rows.Count > 0)
                {


                    string companyname = dsstrsqlpurch.Tables[0].Rows[0]["txtcontactdisplayname"].ToString();
                    // lbltitle.Text = fns.getltext("Email Quotation To " + companyname, "2");
                    pono = dsstrsqlpurch.Tables[0].Rows[0]["Estimateno"].ToString();
                    uid = dsstrsqlpurch.Tables[0].Rows[0]["uid"].ToString();
                    // lblfdffilename.Text = dsstrsqlpurch.Tables[0].Rows[0]["Estimateno"].ToString().Replace(@"/", "_").Replace(@"\", "_");
                    // lblfdffilename.Text = dsstrsqlpurch.Tables[0].Rows[0]["Estimateno"].ToString();
                    IssuedOn = Convert.ToDateTime(dsstrsqlpurch.Tables[0].Rows[0]["EstimateDate"].ToString()).ToString(fns.getdateformat());
                    PurchaseOrderNumber = dsstrsqlpurch.Tables[0].Rows[0]["Estimateno"].ToString();
                    PurchaseOrderRef = dsstrsqlpurch.Tables[0].Rows[0]["Reference"].ToString();
                    FormattedTotal = dsstrsqlpurch.Tables[0].Rows[0]["totalamount"].ToString();
                    Total = dsstrsqlpurch.Tables[0].Rows[0]["totalamount"].ToString();
                    DeliveryDate = Convert.ToDateTime(dsstrsqlpurch.Tables[0].Rows[0]["ExpiryDate"].ToString()).ToString(fns.getdateformat());

                    OrganizationUser = fns.getusersname(dsstrsqlpurch.Tables[0].Rows[0]["uid"].ToString());



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
                ).Replace("{####SupplierSalutation####}", customerSalutation.ToString()
                ).Replace("{####SupplierWebsite####}", CustomerWebsite.ToString()
                ).Replace("{####SupplierName####}", CustomerName.ToString()
                ).Replace("{####SupplierCompanyName####}", CustomerName.ToString()
                ).Replace("{####SupplierFirstName####}", CustomerFirstName.ToString()
                ).Replace("{####SupplierLastName####}", CustomerLastName.ToString()
                ).Replace("{####SupplierDepartment####}", CustomerDepartment.ToString()
                ).Replace("{####SupplierDesignation####}", CustomerDesignation.ToString()
                ).Replace("{####IssuedOn####}", IssuedOn.ToString()
                ).Replace("{####PurchaseOrderNumber####}", PurchaseOrderNumber.ToString()
                ).Replace("{####PurchaseOrderRef####}", PurchaseOrderRef.ToString()
                ).Replace("{####FormattedTotal####}", FormattedTotal.ToString()
                ).Replace("{####Total####}", Total.ToString()
                ).Replace("{####DeliveryDate####}", DeliveryDate.ToString()
                ).Replace("{####CurrencyCode####}", CurrencyCode.ToString()
                ).Replace("{####OrganizationUser####}", OrganizationUser.ToString()
                ).Replace("{####OrganizationName####}", OrganizationName.ToString()
                ).Replace("{####OrgLogo####}", OrgLogo.ToString());
                }
                string nuid = "0";
                string isview = dsnot.Tables[0].Rows[0]["isview"].ToString();


                char[] cha = { ',' };
                char[] cha1 = { '~' };

                string strsqlitem = "select * from tbl_notificationsetupemp where type='" + typeid + "'  and ntfor='" + ntfor + "' and status=1 and archive=0  order by aLevel ";
                DataSet dsadd = da.GetDataSetInline(strsqlitem);
                if (dsadd.Tables[0].Rows.Count > 0)
                {
                    functions fnsm = new functions();
                    string emailids = "";

                    for (int i = 0; i < dsadd.Tables[0].Rows.Count; i++)
                    {

                        string[] arr = dsadd.Tables[0].Rows[i]["hdnempids"].ToString().Split(cha, StringSplitOptions.None);

                        for (int j = 0; j < arr.Length; j++)
                        {
                            nuid = "0";
                            if (arr[j].ToString() != string.Empty)
                            {
                                string[] arrlist = arr[j].ToString().Split(cha1, StringSplitOptions.None);

                                if (arrlist.Length > 1)
                                {
                                    if (arrlist[1].ToString() == "1")
                                    {

                                        if (arrlist[0].ToString() == "1")
                                        {
                                            nuid = fns.teamleaderid(uid);

                                        }
                                        if (arrlist[0].ToString() == "2")
                                        {
                                            nuid = fns.l2managerid(uid);
                                        }
                                    }
                                    else
                                    {
                                        nuid = arrlist[0].ToString();

                                    }

                                    if (nuid != "0")
                                    {
                                        if (isnotification == "1")
                                        {

                                            SqlParameter[] prmn = new SqlParameter[12];
                                            prmn[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                                            prmn[1] = new SqlParameter("@uid", uid);
                                            prmn[2] = new SqlParameter("@nuid", nuid);
                                            prmn[3] = new SqlParameter("@ndatetime", da.getcdatetime());
                                            prmn[4] = new SqlParameter("@notificationtext", notificationtext);
                                            prmn[5] = new SqlParameter("@rid", rid);
                                            prmn[6] = new SqlParameter("@typeid", typeid);
                                            prmn[7] = new SqlParameter("@isnew", "1");
                                            prmn[8] = new SqlParameter("@isread", "0");
                                            prmn[9] = new SqlParameter("@isreaddate", DBNull.Value);
                                            prmn[10] = new SqlParameter("@isview", isview);
                                            prmn[11] = new SqlParameter("@ntfor", ntfor);
                                            da.executeDMLQuery("tbl_pnotificationdetailsinsert", prmn);
                                        }
                                        if (isemail == "1")
                                        {

                                            try
                                            {
                                                string strtoemail = fnsm.getEmpemailById(nuid);
                                                if (strtoemail != string.Empty)
                                                {

                                                    if (emailids != string.Empty)
                                                    {
                                                        emailids = emailids + "," + strtoemail;

                                                    }
                                                    else
                                                    {
                                                        emailids = strtoemail;

                                                    }
                                                }
                                            }
                                            catch
                                            {

                                            }


                                        }
                                        
                                    }


                                }

                            }
                        }
                    }

                    if (isemail == "1")
                    {
                        try
                        {

                            SendMail sm = new SendMail();
                             
                            string to = "";
                            string from = "";
                            string cc = "";
                            string bcc = "";
                            string sub = "";
                            string body = "";
                            string files = "";
                            string[] chkdd = { "####" };
                            string[] filesdata = files.Split(chkdd, StringSplitOptions.None);

                            from = fnsm.getEmpemailById(uid);
                            to = emailids;

                            cc = "";
                            bcc = "";
                            sub = "Purchase Order Notification by " + fnsm.getEmpNameById(uid);
                            body = notificationtext;
                            if (to != string.Empty)
                            {
                                sm.SendMailtoUser(to, from, cc, bcc, sub, body, filesdata);
                            }
                        }
                        catch
                        {

                        }
                    }


                }


            }
        }
        catch
        {

        }

    }
    public void sendnotificationgrn(int rid, int typeid, int ntfor)
    {

        try
        {
            string strsql = "select * from tbl_notificationtemplates where typeid='" + typeid + "' and ntfor='" + ntfor + "' and status=1 and archive=0";
            DataSet dsnot = da.GetDataSetInline(strsql);


            if (dsnot.Tables[0].Rows.Count > 0)
            {

                string notificationtext = dsnot.Tables[0].Rows[0]["temptext"].ToString();
                string notificationtextmain = notificationtext;

                string isnotification = dsnot.Tables[0].Rows[0]["isnotification"].ToString();
                string isemail = dsnot.Tables[0].Rows[0]["isemail"].ToString();
                string issms = dsnot.Tables[0].Rows[0]["issms"].ToString();
                string uid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();

                string PurchaseOrderNumber = "&nbsp;";
                string GRN = "&nbsp;";
                string GRNDATE = "&nbsp;";
                string ItemsDetails = "&nbsp;";
                string SupplierName = "&nbsp;";
                string SupplierFirstName = "&nbsp;";
                string SupplierLastName = "&nbsp;";
                string SupplierDepartment = "&nbsp;";
                string SupplierDesignation = "&nbsp;";

                SqlParameter[] prmpurch = new SqlParameter[2];
                prmpurch[0] = new SqlParameter("@id", rid);
                prmpurch[1] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                DataSet dsstrsqlpurch = da.GetDataSet("tbl_billorderdetailsGRNdetailsfornotification", prmpurch);
                if (dsstrsqlpurch.Tables[0].Rows.Count > 0)
                {
                    PurchaseOrderNumber = dsstrsqlpurch.Tables[0].Rows[0]["pono"].ToString();
                    GRN = dsstrsqlpurch.Tables[0].Rows[0]["Estimateno"].ToString();
                    GRNDATE = dsstrsqlpurch.Tables[0].Rows[0]["EstimateDate"].ToString();

                    for (int ki = 0; ki < dsstrsqlpurch.Tables[1].Rows.Count; ki++)
                    {
                        if (ItemsDetails == string.Empty)
                        {
                            ItemsDetails += dsstrsqlpurch.Tables[1].Rows[ki]["pname"].ToString() + "- Qty " + dsstrsqlpurch.Tables[1].Rows[ki]["quantity"].ToString();
                        }
                        else
                        {
                            ItemsDetails += "," + dsstrsqlpurch.Tables[1].Rows[ki]["pname"].ToString() + "- Qty " + dsstrsqlpurch.Tables[1].Rows[ki]["quantity"].ToString();

                        }


                    }




                    strsql = "select * from tbl_contactsdetails where id='" + dsstrsqlpurch.Tables[0].Rows[0]["cusid"].ToString() + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "'";
                    DataSet ds = da.GetDataSetInline(strsql);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        SupplierName = ds.Tables[0].Rows[0]["txtcompanyname"].ToString();
                        SupplierFirstName = ds.Tables[0].Rows[0]["txtfirstname"].ToString();
                        SupplierLastName = ds.Tables[0].Rows[0]["txtlastname"].ToString();
                        SupplierDepartment = ds.Tables[0].Rows[0]["txtDepartment"].ToString();
                        SupplierDesignation = ds.Tables[0].Rows[0]["txtDesignation"].ToString();


                    }



                }


                notificationtext = notificationtextmain.ToString();

                notificationtext = notificationtext.Replace("{####PurchaseOrderNumber####}", PurchaseOrderNumber.ToString()
               ).Replace("{####GRN####}", GRN.ToString()
               ).Replace("{####GRNDATE####}", GRNDATE.ToString()
                ).Replace("{####SupplierName####}", SupplierName.ToString()
                ).Replace("{####SupplierFirstName####}", SupplierFirstName.ToString()
                   ).Replace("{####SupplierLastName####}", SupplierLastName.ToString()
                   ).Replace("{####SupplierDepartment####}", SupplierDepartment.ToString()
                    ).Replace("{####SupplierDesignation####}", SupplierDesignation.ToString()
               ).Replace("{####ItemsDetails####}", ItemsDetails.ToString());

                string nuid = "0";
                string isview = dsnot.Tables[0].Rows[0]["isview"].ToString();


                char[] cha = { ',' };
                char[] cha1 = { '~' };

                string strsqlitem = "select * from tbl_notificationsetupemp where type='" + typeid + "'  and ntfor='" + ntfor + "' and status=1 and archive=0  order by aLevel ";
                DataSet dsadd = da.GetDataSetInline(strsqlitem);
                if (dsadd.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < dsadd.Tables[0].Rows.Count; i++)
                    {

                        string[] arr = dsadd.Tables[0].Rows[i]["hdnempids"].ToString().Split(cha, StringSplitOptions.None);

                        for (int j = 0; j < arr.Length; j++)
                        {
                            nuid = "0";
                            if (arr[j].ToString() != string.Empty)
                            {
                                string[] arrlist = arr[j].ToString().Split(cha1, StringSplitOptions.None);

                                if (arrlist.Length > 1)
                                {
                                    if (arrlist[1].ToString() == "1")
                                    {

                                        if (arrlist[0].ToString() == "1")
                                        {
                                            nuid = fns.teamleaderid(uid);

                                        }
                                        if (arrlist[0].ToString() == "2")
                                        {
                                            nuid = fns.l2managerid(uid);
                                        }
                                    }
                                    else
                                    {
                                        nuid = arrlist[0].ToString();

                                    }

                                    if (nuid != "0")
                                    {
                                        if (isnotification == "1")
                                        {

                                            SqlParameter[] prmn = new SqlParameter[12];
                                            prmn[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                                            prmn[1] = new SqlParameter("@uid", uid);
                                            prmn[2] = new SqlParameter("@nuid", nuid);
                                            prmn[3] = new SqlParameter("@ndatetime", da.getcdatetime());
                                            prmn[4] = new SqlParameter("@notificationtext", notificationtext);
                                            prmn[5] = new SqlParameter("@rid", rid);
                                            prmn[6] = new SqlParameter("@typeid", typeid);
                                            prmn[7] = new SqlParameter("@isnew", "1");
                                            prmn[8] = new SqlParameter("@isread", "0");
                                            prmn[9] = new SqlParameter("@isreaddate", DBNull.Value);
                                            prmn[10] = new SqlParameter("@isview", isview);
                                            prmn[11] = new SqlParameter("@ntfor", ntfor);
                                            da.executeDMLQuery("tbl_pnotificationdetailsinsert", prmn);
                                        }
                                        if (isemail == "1")
                                        {
                                            try
                                            {

                                                SendMail sm = new SendMail();
                                                functions fnsm = new functions();
                                                string to = "";
                                                string from = "";
                                                string cc = "";
                                                string bcc = "";
                                                string sub = "";
                                                string body = "";
                                                string files = "";
                                                string[] chkdd = { "####" };
                                                string[] filesdata = files.Split(chkdd, StringSplitOptions.None);

                                                from = fnsm.getEmpemailById(uid);
                                                to = fnsm.getEmpemailById(nuid);

                                                cc = "";
                                                bcc = "";
                                                sub = "Purchase Order Notification by " + fnsm.getEmpNameById(uid);
                                                body = notificationtext;
                                                sm.SendMailtoUser(to, from, cc, bcc, sub, body, filesdata);
                                            }
                                            catch
                                            {

                                            }
                                        }
                                    }


                                }

                            }
                        }
                    }




                }


            }
        }
        catch
        {

        }

    }

    public void sendnotificationpurchaseBooked(int rid, int typeid, int ntfor)
    {

        try
        {
            string strsql = "select * from tbl_notificationtemplates where typeid='" + typeid + "' and ntfor='" + ntfor + "' and status=1 and archive=0";
            DataSet dsnot = da.GetDataSetInline(strsql);


            if (dsnot.Tables[0].Rows.Count > 0)
            {

                string notificationtext = dsnot.Tables[0].Rows[0]["temptext"].ToString();
                string notificationtextmain  = notificationtext;

                string isnotification = dsnot.Tables[0].Rows[0]["isnotification"].ToString();
                string isemail = dsnot.Tables[0].Rows[0]["isemail"].ToString();
                string issms = dsnot.Tables[0].Rows[0]["issms"].ToString();
                string uid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();

                string IndentNo = "&nbsp;";
                string MOno = "&nbsp;";
                string RequestNo = "&nbsp;";
                string ProductDetails = "&nbsp;";



                SqlParameter[] prmpurch = new SqlParameter[2];
                prmpurch[0] = new SqlParameter("@poid", rid);
                prmpurch[1] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                DataSet dsstrsqlpurch = da.GetDataSet("selectitemsPurchasescountfunbookbyidnotification", prmpurch);
                if (dsstrsqlpurch.Tables[0].Rows.Count > 0)
                {


                    for (int ki = 0; ki < dsstrsqlpurch.Tables[0].Rows.Count; ki++)
                    {


                        IndentNo = dsstrsqlpurch.Tables[0].Rows[ki]["IndentId"].ToString();
                        MOno = dsstrsqlpurch.Tables[0].Rows[ki]["mocode"].ToString();
                        RequestNo = dsstrsqlpurch.Tables[0].Rows[ki]["requesid"].ToString();
                        ProductDetails = dsstrsqlpurch.Tables[0].Rows[ki]["pname"].ToString();
                        notificationtext = notificationtextmain.ToString();

                        notificationtext = notificationtext.Replace("{####IndentNo####}", IndentNo.ToString()
                       ).Replace("{####MOno####}", MOno.ToString()
                       ).Replace("{####RequestNo####}", RequestNo.ToString()
                       ).Replace("{####ProductDetails####}", ProductDetails.ToString());

                        string nuid = "0";
                        string isview = dsnot.Tables[0].Rows[0]["isview"].ToString();


                        char[] cha = { ',' };
                        char[] cha1 = { '~' };

                        string strsqlitem = "select * from tbl_notificationsetupemp where type='" + typeid + "'  and ntfor='" + ntfor + "' and status=1 and archive=0  order by aLevel ";
                        DataSet dsadd = da.GetDataSetInline(strsqlitem);
                        if (dsadd.Tables[0].Rows.Count > 0)
                        {

                            for (int i = 0; i < dsadd.Tables[0].Rows.Count; i++)
                            {

                                string[] arr = dsadd.Tables[0].Rows[i]["hdnempids"].ToString().Split(cha, StringSplitOptions.None);

                                for (int j = 0; j < arr.Length; j++)
                                {
                                    nuid = "0";
                                    if (arr[j].ToString() != string.Empty)
                                    {
                                        string[] arrlist = arr[j].ToString().Split(cha1, StringSplitOptions.None);

                                        if (arrlist.Length > 1)
                                        {
                                            if (arrlist[1].ToString() == "1")
                                            {

                                                if (arrlist[0].ToString() == "1")
                                                {
                                                    nuid = fns.teamleaderid(uid);

                                                }
                                                if (arrlist[0].ToString() == "2")
                                                {
                                                    nuid = fns.l2managerid(uid);
                                                }
                                            }
                                            else
                                            {
                                                nuid = arrlist[0].ToString();

                                            }

                                            if (nuid != "0")
                                            {
                                                if (isnotification == "1")
                                                {

                                                    SqlParameter[] prmn = new SqlParameter[12];
                                                    prmn[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                                                    prmn[1] = new SqlParameter("@uid", uid);
                                                    prmn[2] = new SqlParameter("@nuid", nuid);
                                                    prmn[3] = new SqlParameter("@ndatetime", da.getcdatetime());
                                                    prmn[4] = new SqlParameter("@notificationtext", notificationtext);
                                                    prmn[5] = new SqlParameter("@rid", rid);
                                                    prmn[6] = new SqlParameter("@typeid", typeid);
                                                    prmn[7] = new SqlParameter("@isnew", "1");
                                                    prmn[8] = new SqlParameter("@isread", "0");
                                                    prmn[9] = new SqlParameter("@isreaddate", DBNull.Value);
                                                    prmn[10] = new SqlParameter("@isview", isview);
                                                    prmn[11] = new SqlParameter("@ntfor", ntfor);
                                                    da.executeDMLQuery("tbl_pnotificationdetailsinsert", prmn);
                                                }
                                                if (isemail == "1")
                                                {
                                                    try
                                                    {

                                                        SendMail sm = new SendMail();
                                                        functions fnsm = new functions();
                                                        string to = "";
                                                        string from = "";
                                                        string cc = "";
                                                        string bcc = "";
                                                        string sub = "";
                                                        string body = "";
                                                        string files = "";
                                                        string[] chkdd = { "####" };
                                                        string[] filesdata = files.Split(chkdd, StringSplitOptions.None);

                                                        from = fnsm.getEmpemailById(uid);
                                                        to = fnsm.getEmpemailById(nuid);

                                                        cc = "";
                                                        bcc = "";
                                                        sub = "Purchase Order Notification by " + fnsm.getEmpNameById(uid);
                                                        body = notificationtext;
                                                        sm.SendMailtoUser(to, from, cc, bcc, sub, body, filesdata);
                                                    }
                                                    catch
                                                    {

                                                    }
                                                }
                                            }


                                        }

                                    }
                                }
                            }




                        }
                    }
                }

            }
        }
        catch
        {

        }

    }
    public void sendnotificationpurchasereserv(int rid, int typeid, int ntfor)
    {

        try
        {
            string strsql = "select * from tbl_notificationtemplates where typeid='" + typeid + "' and ntfor='" + ntfor + "' and status=1 and archive=0";
            DataSet dsnot = da.GetDataSetInline(strsql);


            if (dsnot.Tables[0].Rows.Count > 0)
            {

                string notificationtext = dsnot.Tables[0].Rows[0]["temptext"].ToString();
                string notificationtextmain = notificationtext;

                string isnotification = dsnot.Tables[0].Rows[0]["isnotification"].ToString();
                string isemail = dsnot.Tables[0].Rows[0]["isemail"].ToString();
                string issms = dsnot.Tables[0].Rows[0]["issms"].ToString();
                string uid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();

                string IndentNo = "&nbsp;";
                string MOno = "&nbsp;";
                string RequestNo = "&nbsp;";
                string ProductDetails = "&nbsp;";



                SqlParameter[] prmpurch = new SqlParameter[2];
                prmpurch[0] = new SqlParameter("@poid", rid);
                prmpurch[1] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                DataSet dsstrsqlpurch = da.GetDataSet("selectitemsbillcountfunbookbyidnotification", prmpurch);
                if (dsstrsqlpurch.Tables[0].Rows.Count > 0)
                {


                    for (int ki = 0; ki < dsstrsqlpurch.Tables[0].Rows.Count; ki++)
                    {


                        IndentNo = dsstrsqlpurch.Tables[0].Rows[ki]["IndentId"].ToString();
                        MOno = dsstrsqlpurch.Tables[0].Rows[ki]["mocode"].ToString();
                        RequestNo = dsstrsqlpurch.Tables[0].Rows[ki]["requesid"].ToString();
                        ProductDetails = dsstrsqlpurch.Tables[0].Rows[ki]["pname"].ToString();
                        notificationtext = notificationtextmain.ToString();

                        notificationtext = notificationtext.Replace("{####IndentNo####}", IndentNo.ToString()
                       ).Replace("{####MOno####}", MOno.ToString()
                       ).Replace("{####RequestNo####}", RequestNo.ToString()
                       ).Replace("{####ProductDetails####}", ProductDetails.ToString());

                        string nuid = "0";
                        string isview = dsnot.Tables[0].Rows[0]["isview"].ToString();


                        char[] cha = { ',' };
                        char[] cha1 = { '~' };

                        string strsqlitem = "select * from tbl_notificationsetupemp where type='" + typeid + "'  and ntfor='" + ntfor + "' and status=1 and archive=0  order by aLevel ";
                        DataSet dsadd = da.GetDataSetInline(strsqlitem);
                        if (dsadd.Tables[0].Rows.Count > 0)
                        {

                            for (int i = 0; i < dsadd.Tables[0].Rows.Count; i++)
                            {

                                string[] arr = dsadd.Tables[0].Rows[i]["hdnempids"].ToString().Split(cha, StringSplitOptions.None);

                                for (int j = 0; j < arr.Length; j++)
                                {
                                    nuid = "0";
                                    if (arr[j].ToString() != string.Empty)
                                    {
                                        string[] arrlist = arr[j].ToString().Split(cha1, StringSplitOptions.None);

                                        if (arrlist.Length > 1)
                                        {
                                            if (arrlist[1].ToString() == "1")
                                            {

                                                if (arrlist[0].ToString() == "1")
                                                {
                                                    nuid = fns.teamleaderid(uid);

                                                }
                                                if (arrlist[0].ToString() == "2")
                                                {
                                                    nuid = fns.l2managerid(uid);
                                                }
                                            }
                                            else
                                            {
                                                nuid = arrlist[0].ToString();

                                            }

                                            if (nuid != "0")
                                            {
                                                if (isnotification == "1")
                                                {

                                                    SqlParameter[] prmn = new SqlParameter[12];
                                                    prmn[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                                                    prmn[1] = new SqlParameter("@uid", uid);
                                                    prmn[2] = new SqlParameter("@nuid", nuid);
                                                    prmn[3] = new SqlParameter("@ndatetime", da.getcdatetime());
                                                    prmn[4] = new SqlParameter("@notificationtext", notificationtext);
                                                    prmn[5] = new SqlParameter("@rid", rid);
                                                    prmn[6] = new SqlParameter("@typeid", typeid);
                                                    prmn[7] = new SqlParameter("@isnew", "1");
                                                    prmn[8] = new SqlParameter("@isread", "0");
                                                    prmn[9] = new SqlParameter("@isreaddate", DBNull.Value);
                                                    prmn[10] = new SqlParameter("@isview", isview);
                                                    prmn[11] = new SqlParameter("@ntfor", ntfor);
                                                    da.executeDMLQuery("tbl_pnotificationdetailsinsert", prmn);
                                                }
                                                if (isemail == "1")
                                                {
                                                    try
                                                    {

                                                        SendMail sm = new SendMail();
                                                        functions fnsm = new functions();
                                                        string to = "";
                                                        string from = "";
                                                        string cc = "";
                                                        string bcc = "";
                                                        string sub = "";
                                                        string body = "";
                                                        string files = "";
                                                        string[] chkdd = { "####" };
                                                        string[] filesdata = files.Split(chkdd, StringSplitOptions.None);

                                                        from = fnsm.getEmpemailById(uid);
                                                        to = fnsm.getEmpemailById(nuid);

                                                        cc = "";
                                                        bcc = "";
                                                        sub = "Purchase Order Notification by " + fnsm.getEmpNameById(uid);
                                                        body = notificationtext;
                                                        sm.SendMailtoUser(to, from, cc, bcc, sub, body, filesdata);
                                                    }
                                                    catch
                                                    {

                                                    }
                                                }
                                            }


                                        }

                                    }
                                }
                            }




                        }
                    }
                }

            }
        }
        catch
        {

        }

    }




}