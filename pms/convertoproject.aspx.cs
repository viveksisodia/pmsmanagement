using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonClass;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using CustomDropDown;
using System.Configuration;
using System.Web.Services;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
public partial class convertoproject : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataAccess da = new DataAccess();
    public CommonFunction Cmf = new CommonFunction();
    SqlParameter[] prm, prm1;
    public function fns = new function();
    public string Multiproduct = "";
    public string mcatReason = "";
    public string str_selected = "style='display:none'";
    public string strviewpropbablity = "style='display:none'";
    public string strdvcity = "style='display:none'";
    public string str_selecteddeatil = "";
    public int isthemeid = 1;
    public int isleadnotes = 0;
    public string isshowstateddl = "style='display:none'";
    protected void Page_Load(object sender, EventArgs e)
    {
        leadProbability.Disabled = true;
        string strsqladminc = "select *  from tbl_registrationdetails where id='" + Request.Cookies["cadmin"]["id"].ToString() + "' and status=1 and archive=0";
        DataSet dsisthe = da.GetDataSetInline(strsqladminc);
        // Response.Write(strsqladmin);
        if (dsisthe.Tables[0].Rows.Count > 0)
        {
            try
            {
                isthemeid = Convert.ToInt32(dsisthe.Tables[0].Rows[0]["themeid"].ToString());
            }
            catch
            {

            }
        }
        if (Request.Cookies["cadmin"]["id"].ToString() == "2214" || Request.Cookies["cadmin"]["id"].ToString() == "2219" || Request.Cookies["cadmin"]["id"].ToString() == "2224" || Request.Cookies["cadmin"]["id"].ToString() == "2176")
        {
            isleadnotes = 1;
        }
        if (!Page.IsPostBack)
        {

            bindSalutation();
            bindleadstatus();
            bindleadsource();
            bindleadcountry();
            bindleadcountrystates("0");
            bindleadindustry();
            bindleadowner();
            bindAddProduct();
            bind_minmaxbudget();
            bind_minmaxsize();
            hdisResidential.Value = "1";
            hdisrenbuy.Value = "2";
            lblrent.Text = bindListingdata("2", "rent");
            lblbuy.Text = bindListingdata("1", "buy");
            listingbeds.Text = listingstatusdatabind("13", "1", "beds", hdnlistingbeds);

            listingBaths.Text = listingstatusdatabind("14", "1", "Baths", hdnlistingBaths);
            listingstatus.Text = listingstatusdatabind("15", "1", "Furnishing", hdnlistingstatus);
            lblresidentialval.Text = bindPropertySubtypedata("1", "0", "residential");
            lblcommercialval.Text = bindPropertySubtypedata("2", "0", "commercial");
            bindReason();
            if (Request.QueryString["id"] != null)
            {
                if(Request.QueryString["type"] != null)
                {

                }
                
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "txt", "bindprobality();", true);
            }
        }
    }
    protected void binddata()
    {
        string chkcountryid = getcontry_map();
        string strsql = "select * from tbl_mainleads where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + Request.QueryString["id"].ToString() + "' ";

        DataSet dsN = da.GetDataSetInline(strsql);
        if (dsN.Tables[0].Rows.Count > 0)
        {
            btnsave.Text = "Update";
            btnsavemore.Visible = false;
            hdnleadeditid.Value = dsN.Tables[0].Rows[0]["id"].ToString();
            Salutation.SelectedValue = dsN.Tables[0].Rows[0]["Salutation"].ToString();
            fName.Text = dsN.Tables[0].Rows[0]["FirstName"].ToString();
            lName.Text = dsN.Tables[0].Rows[0]["LastName"].ToString();
            cName.Text = dsN.Tables[0].Rows[0]["Company"].ToString();
            PhoneNo.Text = dsN.Tables[0].Rows[0]["PhoneNo"].ToString();
            EmailId.Text = dsN.Tables[0].Rows[0]["EmailId"].ToString();
            title.Text = dsN.Tables[0].Rows[0]["Title"].ToString();
            website.Text = dsN.Tables[0].Rows[0]["Website"].ToString();
            description.Value = dsN.Tables[0].Rows[0]["Descriptions"].ToString();
            leadStatus.SelectedValue = dsN.Tables[0].Rows[0]["LeadStatus"].ToString();
            try
            {
                Industry.SelectedValue = dsN.Tables[0].Rows[0]["IndustryTypeid"].ToString();
            }
            catch
            {

            }
            try
            {
                boatid.Text = dsN.Tables[0].Rows[0]["TeleGramBoatId"].ToString();
                hdnleadProbability.Value = dsN.Tables[0].Rows[0]["LProbability"].ToString();
                leadProbability.Value = dsN.Tables[0].Rows[0]["LProbability"].ToString();
            }
            catch
            { }
            LeadSource.SelectedValue = dsN.Tables[0].Rows[0]["LeadSourceid"].ToString();
            Owner.SelectedValue = dsN.Tables[0].Rows[0]["OwnerName"].ToString();
            AddressDetails.Value = dsN.Tables[0].Rows[0]["LAddress"].ToString();
            StreetName.Value = dsN.Tables[0].Rows[0]["Street"].ToString();
            CityName.Value = dsN.Tables[0].Rows[0]["CityName"].ToString();
            PostalCode.Value = dsN.Tables[0].Rows[0]["PostalCode"].ToString();
            StateName.Value = dsN.Tables[0].Rows[0]["StateName"].ToString();
            CountryId.SelectedValue = dsN.Tables[0].Rows[0]["CountryId"].ToString();
            hdnddLeadProduct.Value = dsN.Tables[0].Rows[0]["Leadproduct"].ToString();
            try
            {
                txtlnotes.Value = dsN.Tables[0].Rows[0]["leadnotes"].ToString();
            }
            catch
            {

            }
            //try
            //{
            //Response.Write(chkcountryid);
            //Response.End();
            //if(chkcountryid=="77")
            //{
            if (CountryId.SelectedValue == "77")
            {
                StateId.Visible = true;
                CityId.Visible = true;
                pnlstate.Visible = false;
                pnlcity.Visible = false;
                bindleadcountrystates(CountryId.SelectedValue);
                StateId.SelectedValue = dsN.Tables[0].Rows[0]["StateId"].ToString();
                bindleadstatecity(StateId.SelectedValue);
                CityId.SelectedValue = dsN.Tables[0].Rows[0]["CityId"].ToString();
            }
            else
            {
                StateId.Visible = false;
                CityId.Visible = false;
                pnlstate.Visible = true;
                pnlcity.Visible = true;
            }
            //}
            //catch
            //{

            //}
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "txt", "binddata()", true);

        }
    }
    protected void bindAddProduct()
    {

        string dataN = "";
        string strsql = "select id, pname as DisplayName   from tbl_itemsdetails where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by pname";

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

        Multiproduct = dataN.ToString();
        // Response.Write(mattendee);
        //Response.End();

    }
    protected void bindSalutation()
    {
        try
        {

            ListItem li1 = new ListItem("", "0");
            Salutation.Items.Insert(0, li1);


        }
        catch
        { }

    }
    protected void bindleadcountry()
    {
        string strleadstatus = "select conid id,country countryname  from Imt_Country order by conid";
        //string strleadstatus = "select id,countryname  from tbl_countrymaster order by id";
        DataSet ds = da.GetDataSetInline(strleadstatus);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Cmf.BindDropDownList(CountryId, ds, "countryname", "id");
            ListItem li1 = new ListItem("", "0");
            CountryId.Items.Insert(0, li1);
        }
        ListItem li2 = new ListItem(fns.getltext("+Add New Country", "2"), "-1");
        CountryId.Items.Add(li2);
        string countryid = getcontry_map();

        if (countryid != "" && countryid != "0")
        {
            CountryId.SelectedValue = countryid;
        }
        else
        {
            CountryId.SelectedValue = "182";
        }
        if (countryid == "77")
        {
            isshowstateddl = "style='display:none'";
            StateId.Visible = true;
            CityId.Visible = true;
            pnlstate.Visible = false;
            pnlcity.Visible = false;
        }
        else
        {
            isshowstateddl = "style='display:'";
            StateId.Visible = false;
            CityId.Visible = false;
            pnlstate.Visible = true;
            pnlcity.Visible = true;

        }
        //CountryId.SelectedValue = "182";

    }
    protected void bindleadcountrystates(string countryid)
    {

        // string strleadstatus = "select id,statename  from tbl_statemaster order by id";
        try
        {

            //   countryid = getcontry_map();

            prm = new SqlParameter[2];
            prm[0] = new SqlParameter("@conid", CountryId.SelectedValue);
            prm[1] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"].ToString());
            DataSet ds = new DataSet();
            ds = da.GetDataSet("Add_Client_State_select", prm);
            Cmf.BindDropDownList(StateId, ds, "statename", "id");
            ListItem li3 = new ListItem(fns.getltext("", "2"), "0");
            StateId.Items.Insert(0, li3);

        }
        catch
        {
            StateId.DataSource = null;
            StateId.DataBind();
            ListItem li1 = new ListItem("", "0");
            StateId.Items.Insert(0, li1);
        }


    }
    protected void bindleadstatecity(string stateid)
    {

        string strleadstatus = "";
        //try
        //{

        //strleadstatus = "select id,cityname  from tbl_CityMasterCrm where stateid='" + stateid + "' order by id";

        //Response.Write(StateId.SelectedValue);
        SaveLogText("stateid-" + StateId.SelectedValue);
        prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@stateid", StateId.SelectedValue);
        prm[1] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"].ToString());
        DataSet ds = new DataSet();
        ds = da.GetDataSet("Add_Client_City_select_by_State", prm);
        Cmf.BindDropDownList(CityId, ds, "city_name", "city_id");
        ListItem li2 = new ListItem(fns.getltext("+Add New City", "2"), "-1");
        CityId.Items.Add(li2);
        ListItem li3 = new ListItem(fns.getltext("", "2"), "0");
        CityId.Items.Insert(0, li3);

        //}
        //catch
        //{
        //    CityId.DataSource = null;
        //    CityId.DataBind();
        //    ListItem li1 = new ListItem("", "0");
        //    CityId.Items.Insert(0, li1);
        //}
        try
        {
            pnlcity.Visible = true;
            if (ds.Tables[0].Rows.Count == 0)
            {
                CityId.SelectedValue = "-1";
                strdvcity = "";
            }
            else
            {

                strdvcity = "style='display:none'";
            }
        }
        catch
        {

        }


    }




    protected string getcontry_map()
    {
        string retval = "0";
        string strsql = "select isnull(countryid,2) countryid from tbl_registrationdetails where id='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["countryid"].ToString() != "")
            {
                string strsqlM = "select id ,isnull(country_mapid,0) mapid from tbl_countrymaster where id='" + ds.Tables[0].Rows[0]["countryid"].ToString() + "'";
                DataSet dsM = da.GetDataSetInline(strsqlM);
                if (dsM.Tables[0].Rows.Count > 0)
                {
                    retval = dsM.Tables[0].Rows[0]["mapid"].ToString();
                }

            }

        }
        return retval;
    }
    protected void bindleadstatus()
    {
        try
        {
            string strleadstatus = "select id,StatusName,orderno,isnull(IsDefault,0) IsDefault  from tbl_leadstatus where status=1 and archive=0 and regid='" + Request.Cookies["cadmin"]["id"].ToString() + "' order by orderno";
            DataSet ds = da.GetDataSetInline(strleadstatus);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow[] dr = ds.Tables[0].Select("orderno=1");
                Cmf.BindDropDownList(leadStatus, ds, "StatusName", "id");
                ListItem li1 = new ListItem("", "0");
                leadStatus.Items.Insert(0, li1);
                DataRow[] drdf = ds.Tables[0].Select("IsDefault=1");
                if (drdf.Length > 0)
                {

                    leadStatus.SelectedValue = drdf[0]["id"].ToString();
                    //Response.Write(leadStatus.SelectedValue);
                    //Response.End();
                }
                else
                {
                    try
                    {
                        leadStatus.SelectedValue = dr[0]["id"].ToString();
                    }
                    catch
                    { }
                }

            }
        }
        catch
        { }


    }
    protected void bindleadsource()
    {

        SqlParameter[] prm = new SqlParameter[5];
        prm[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"]);
        prm[1] = new SqlParameter("@name", DBNull.Value);
        prm[2] = new SqlParameter("@id", DBNull.Value);
        prm[3] = new SqlParameter("@type", "1");
        prm[4] = new SqlParameter("@typevalue", "select");
        DataSet ds = da.GetDataSet("crm_ups_tbl_crmmasterdata", prm);

        Cmf.BindDropDownList(LeadSource, ds, "name", "id");
        ListItem li2 = new ListItem("", "0");
        LeadSource.Items.Insert(0, li2);

        ListItem li3 = new ListItem(fns.getltext("+Add New Source", "2"), "-1");
        LeadSource.Items.Add(li3);
    }
    protected void bindleadindustry()
    {
        SqlParameter[] prm = new SqlParameter[5];
        prm[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"]);
        prm[1] = new SqlParameter("@name", DBNull.Value);
        prm[2] = new SqlParameter("@id", DBNull.Value);
        prm[3] = new SqlParameter("@type", "5");
        prm[4] = new SqlParameter("@typevalue", "select");

        DataSet ds = da.GetDataSet("crm_ups_tbl_crmmasterdata", prm);
        Cmf.BindDropDownList(Industry, ds, "name", "id");
        ListItem li2 = new ListItem("", "0");
        Industry.Items.Insert(0, li2);
        //Industry.DataValueField = "id";
        //Industry.DataTextField = "name";
        //Industry.DataSource = ds.Tables[0];
        //Industry.DataBind();
        //Industry.Items.Insert(0, "Select Industry");
        //Industry.SelectedIndex = 0;
        ListItem li1 = new ListItem(fns.getltext("+Add New Industry", "2"), "-1");
        Industry.Items.Add(li1);

    }
    protected void bindleadowner()
    {
        int isadmin = 0;

        string uid = Request.Cookies["user"]["user_id"].ToString();
        //try


        string strsqladm = "select Manage_Profile.* from Manage_Profile inner join tbl_employee on Manage_Profile.id=cast(tbl_employee.roleid as int) where tbl_employee.id='" + uid + "'  and Manage_Profile.status=1 and Manage_Profile.archive=0";
        DataSet dsadm = da.GetDataSetInline(strsqladm);
        if (dsadm.Tables[0].Rows.Count > 0)
        {
            if (dsadm.Tables[0].Rows[0]["id"].ToString() == "1")
            {
                isadmin = 1;

            }
            else
            {
                isadmin = 0;
            }
        }
        SqlParameter[] prm = new SqlParameter[3];
        prm[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"]);
        prm[1] = new SqlParameter("@isadmin", isadmin);
        prm[2] = new SqlParameter("@uid", Request.Cookies["user"]["user_id"].ToString());
        DataSet ds = da.GetDataSet("bindemployee_forTransfer", prm);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Cmf.BindDropDownList(Owner, ds, "displayname", "id");
            ListItem li1 = new ListItem("", "0");
            Owner.Items.Insert(0, li1);
        }
        try
        {
            Owner.SelectedValue = Request.Cookies["user"]["user_id"].ToString();
        }
        catch
        { }
    }
    protected void bind_minmaxbudget(string typefor = "")
    {
        string dataN = "";
        string strsql = "";
        if (typefor == "")
        {
            strsql = "select id,isnull(name,0) minimum,isnull(maximum,0) maximum ,isnull(interval,0) interval  from tbl_crmmasterdata where type =27 and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and isnull(purpose,0)=2  order by name";
        }
        else
        {
            strsql = "select id,isnull(name,0) minimum,isnull(maximum,0) maximum ,isnull(interval,0) interval  from tbl_crmmasterdata where type =27 and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and isnull(purpose,0)='" + typefor + "'  order by name";
        }

        DataSet dsN = da.GetDataSetInline(strsql);
        if (dsN.Tables[0].Rows.Count > 0)
        {
            ddlminBudget.Items.Clear();
            ddlmaxBudget.Items.Clear();
            ListItem li1 = new ListItem("", "0");
            ddlminBudget.Items.Insert(0, li1);
            ddlmaxBudget.Items.Insert(0, li1);
            int min = Convert.ToInt32(dsN.Tables[0].Rows[0]["minimum"].ToString());

            int max = Convert.ToInt32(dsN.Tables[0].Rows[0]["maximum"].ToString());
            int inerval = Convert.ToInt32(dsN.Tables[0].Rows[0]["interval"].ToString());

            int value = 0;
            int cnt = 0;
            while (max >= min)
            {

                if (cnt == 0)
                {
                    value += min;
                }
                else
                {
                    value += inerval;
                }
                max -= inerval;
                cnt++;

                ListItem li = new ListItem(value.ToString(), value.ToString());
                ddlminBudget.Items.Add(li);
                ddlmaxBudget.Items.Add(li);
            }
            //Cmf.BindDropDownList(ddlminBudget, dsN, "name", "id");
            //ListItem li1 = new ListItem("", "0");
            //ddlminBudget.Items.Insert(0, li1);
            //Cmf.BindDropDownList(ddlmaxBudget, dsN, "name", "id");
            //ddlmaxBudget.Items.Insert(0, li1);
        }
        else
        {
            ListItem li1 = new ListItem("", "0");
            ddlminBudget.Items.Insert(0, li1);
            ddlmaxBudget.Items.Insert(0, li1);
        }


    }
    protected void bind_minmaxsize(string typefor = "")
    {
        ddlminsize.Items.Clear();
        ddlmaxsize.Items.Clear();
        string dataN = "";
        string strsql = "";
        strsql = "select id,isnull(name,0) minimum,isnull(maximum,0) maximum ,isnull(interval,0) interval  from tbl_crmmasterdata where type =28 and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  and isnull(purpose,0)='2'   order by name";
        if (typefor != "")
        {
            strsql = "select id,isnull(name,0) minimum,isnull(maximum,0) maximum ,isnull(interval,0) interval  from tbl_crmmasterdata where type =28 and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and isnull(purpose,0)='" + typefor + "'  order by name";
        }

        DataSet dsN = da.GetDataSetInline(strsql);
        if (dsN.Tables[0].Rows.Count > 0)
        {
            ListItem li1 = new ListItem("", "0");
            ddlminsize.Items.Insert(0, li1);
            ddlmaxsize.Items.Insert(0, li1);
            int min = Convert.ToInt32(dsN.Tables[0].Rows[0]["minimum"].ToString());

            int max = Convert.ToInt32(dsN.Tables[0].Rows[0]["maximum"].ToString());
            int inerval = Convert.ToInt32(dsN.Tables[0].Rows[0]["interval"].ToString());

            int value = 0;
            int cnt = 0;
            while (max >= min)
            {

                if (cnt == 0)
                {
                    value += min;
                }
                else
                {
                    value += inerval;
                }
                max -= inerval;
                cnt++;

                ListItem li = new ListItem(value.ToString(), value.ToString());
                ddlminsize.Items.Add(li);
                ddlmaxsize.Items.Add(li);
            }
            //Cmf.BindDropDownList(ddlminsize, dsN, "name", "id");
            //ListItem li1 = new ListItem("", "0");
            //ddlminsize.Items.Insert(0, li1);
            //Cmf.BindDropDownList(ddlmaxsize, dsN, "name", "id");
            //ddlmaxsize.Items.Insert(0, li1);
        }
        else
        {
            ListItem li1 = new ListItem("", "0");
            ddlminsize.Items.Insert(0, li1);
            ddlmaxsize.Items.Insert(0, li1);
        }
    }
    protected static string getmasterdatname(string id)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strleadstatus = "select id,name  from tbl_crmmasterdata where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + id + "'  order by id";
        DataSet ds = da.GetDataSetInline(strleadstatus);
        if (ds.Tables[0].Rows.Count > 0)
        {
            strret = ds.Tables[0].Rows[0]["name"].ToString();
        }
        if (strret == string.Empty)
        {
            strret = "&nbsp;";
        }
        return strret;
    }

    public static string bindpropertytypedata(string id)
    {

        string strdata = "";
        function fns = new function();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();


        string sqltype = "select * from crm_tbl_propertytype where id='" + id + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  ";
        DataSet ds_proprtytype = da.GetDataSetInline(sqltype);
        if (ds_proprtytype.Tables[0].Rows.Count > 0)
        {


            strdata = ds_proprtytype.Tables[0].Rows[0]["Properttype"].ToString();


        }


        return strdata;
    }

    public string bindPropertySubtypedata(string type_id, string id, string Feature)
    {

        string properttypetdata = "";
        function fns = new function();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();

        string sqltype = "select * from crm_tbl_propertytype where Subproperttype='" + type_id + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  ";
        DataSet ds_proprtytype = da.GetDataSetInline(sqltype);
        if (ds_proprtytype.Tables[0].Rows.Count > 0)
        {
            properttypetdata += "<div class='d-flex align-items-center mb-1'>";
            properttypetdata += "<div class='hstack gap-2 flex-wrap property_typrc'>";
            for (int i = 0; i < ds_proprtytype.Tables[0].Rows.Count; i++)
            {
                if (ds_proprtytype.Tables[0].Rows[i]["id"].ToString() == id)
                {
                    //properttypetdata += "<input type = 'radio' class='btn-check' onclick='Binddetails_listingstatus(" + ds_proprtytype.Tables[0].Rows[i]["id"].ToString() + ")' name='residential' id='residential" + (i) + "' checked>";

                    properttypetdata += "<input type = 'checkbox' class='btn-check' checked='checked' onclick=\"Binddetails_Proptysubtype('" + ds_proprtytype.Tables[0].Rows[i]["id"].ToString() + "','" + type_id + "','" + Feature + (ds_proprtytype.Tables[0].Rows[i]["id"].ToString()) + "')\"    id='" + Feature + (ds_proprtytype.Tables[0].Rows[i]["id"].ToString()) + "'>";
                }
                else
                {
                    properttypetdata += "<input type = 'checkbox' class='btn-check' onclick=\"Binddetails_Proptysubtype('" + ds_proprtytype.Tables[0].Rows[i]["id"].ToString() + "','" + type_id + "','" + Feature + (ds_proprtytype.Tables[0].Rows[i]["id"].ToString()) + "')\"    id='" + Feature + (ds_proprtytype.Tables[0].Rows[i]["id"].ToString()) + "'>";
                }
                properttypetdata += "<label class='btn  btn-sm rounded-pill btn-outline-primary' for='" + Feature + (ds_proprtytype.Tables[0].Rows[i]["id"].ToString()) + "'>" + ds_proprtytype.Tables[0].Rows[i]["Properttype"].ToString() + "</label>";

            }
            properttypetdata += "</div>";
            properttypetdata += "</div>";

        }


        return properttypetdata;
    }

    public static string bindListingdata(string type_id, string typefor)
    {

        string strdata = "";
        function fns = new function();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();

        string sqltype = "select * from crm_tbl_Purposetype where PurposeStatus='" + type_id + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  ";
        DataSet dstype = da.GetDataSetInline(sqltype);
        if (dstype.Tables[0].Rows.Count > 0)
        {
            //strdata += "<div class='tab-content text-muted'>";
            //strdata += "<div class='tab-pane active' id='pro_rent' role='tabpanel'>";
            strdata += "<div class='hstack gap-2 flex - wrap status_btn mb-1'>";
            for (int i = 0; i < dstype.Tables[0].Rows.Count; i++)
            {

                strdata += "<input type = 'checkbox' onclick='Binddetails_listingdata(" + dstype.Tables[0].Rows[i]["id"].ToString() + ", \"" + typefor + "" + dstype.Tables[0].Rows[i]["id"].ToString() + "\", \"" + type_id + "\")' class='btn-check' id='" + typefor + "" + (dstype.Tables[0].Rows[i]["id"].ToString()) + "'>";
                strdata += "<label class='btn btn-sm rounded-pill btn-outline-primary' for='" + typefor + "" + (dstype.Tables[0].Rows[i]["id"].ToString()) + "'>" + dstype.Tables[0].Rows[i]["Purposetype"].ToString() + "</label>";

            }
            strdata += "</div>";
            //strdata += "</div>";
            //strdata += "</div>";

        }


        return strdata;
    }
    public static string getpurposedata(string type_id, string searchid)
    {

        string strdata = "";
        function fns = new function();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();

        string sqltype = "select * from crm_tbl_Purposetype where PurposeStatus='" + type_id + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id in(" + searchid + ") ";

        //Response.Write(sqltype);
        //Response.End();
        DataSet dstype = da.GetDataSetInline(sqltype);
        if (dstype.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dstype.Tables[0].Rows.Count; i++)
            {
                if (strdata == "")
                {
                    strdata = dstype.Tables[0].Rows[i]["Purposetype"].ToString();
                }
                else
                {
                    strdata += "," + dstype.Tables[0].Rows[i]["Purposetype"].ToString();
                }

            }

        }


        return strdata;
    }
    public static string getlistingdata(string type_id, string searchid)
    {

        string strdata = "";
        function fns = new function();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();

        string sqltype = "select * from tbl_crmmasterdata where type='" + type_id + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id in(" + searchid + ")  ";
        DataSet dstype = da.GetDataSetInline(sqltype);
        if (dstype.Tables[0].Rows.Count > 0)
        {
            //strdata = dstype.Tables[0].Rows[0]["name"].ToString();
            for (int i = 0; i < dstype.Tables[0].Rows.Count; i++)
            {
                if (strdata == "")
                {
                    strdata = dstype.Tables[0].Rows[i]["name"].ToString();
                }
                else
                {
                    strdata += "," + dstype.Tables[0].Rows[i]["name"].ToString();
                }

            }

        }


        return strdata;
    }
    public static string getresidentialdata(string type_id, string searchid)
    {

        string strdata = "";
        function fns = new function();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();

        string sqltype = "select * from crm_tbl_propertytype where Subproperttype='" + type_id + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  and id in(" + searchid + ")   ";
        DataSet ds_proprtytype = da.GetDataSetInline(sqltype);
        if (ds_proprtytype.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds_proprtytype.Tables[0].Rows.Count; i++)
            {
                if (strdata == "")
                {
                    strdata = ds_proprtytype.Tables[0].Rows[i]["Properttype"].ToString();
                }
                else
                {
                    strdata += "," + ds_proprtytype.Tables[0].Rows[i]["Properttype"].ToString();
                }

            }

        }


        return strdata;
    }
    public static string getlocationdata(string type_id, string searchid)
    {

        string strdata = "";
        function fns = new function();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();

        string sqltype = "select * from tbl_crmmasterdata where type in(" + type_id + ") and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id in(" + searchid + ")  ";
        DataSet dstype = da.GetDataSetInline(sqltype);
        if (dstype.Tables[0].Rows.Count > 0)
        {
            //strdata = dstype.Tables[0].Rows[0]["name"].ToString();
            for (int i = 0; i < dstype.Tables[0].Rows.Count; i++)
            {
                if (strdata == "")
                {
                    strdata = dstype.Tables[0].Rows[i]["name"].ToString();
                }
                else
                {
                    strdata += "," + dstype.Tables[0].Rows[i]["name"].ToString();
                }

            }

        }


        return strdata;
    }
    public string listingstatusdatabind(string type_id, string typefor, string Feature, HiddenField hidnfieldidforval)
    {

        string strdata = "";
        function fns = new function();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();

        string sqltype = "select * from tbl_crmmasterdata where type='" + type_id + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  ";
        DataSet ds_proprtytype = da.GetDataSetInline(sqltype);
        if (ds_proprtytype.Tables[0].Rows.Count > 0)
        {
            strdata += "<div class='hstack gap-2 flex-wrap status_btn mb-1'>";
            for (int i = 0; i < ds_proprtytype.Tables[0].Rows.Count; i++)
            {

                char[] ch = { ',' };
                string[] arr = hidnfieldidforval.Value.Split(ch, StringSplitOptions.None);

                string strchk = "";
                for (int k = 0; k < arr.Length; k++)
                {
                    if (arr[k] == ds_proprtytype.Tables[0].Rows[i]["id"].ToString())
                    {
                        strchk = "checked='checked'";
                    }
                }



                if (typefor == "0")
                {

                    strdata += "<input type = 'radio' " + strchk + " onclick=\"Binddetails_listingstatus('" + ds_proprtytype.Tables[0].Rows[i]["id"].ToString() + "','" + type_id + "','" + typefor + "','" + Feature + (ds_proprtytype.Tables[0].Rows[i]["id"].ToString()) + "','" + Feature + "')\"  class='btn-check' name='status" + type_id + "' id='" + Feature + (ds_proprtytype.Tables[0].Rows[i]["id"].ToString()) + "'>";
                }
                else
                {
                    strdata += "<input type = 'checkbox' " + strchk + " onclick=\"Binddetails_listingstatus('" + ds_proprtytype.Tables[0].Rows[i]["id"].ToString() + "','" + type_id + "','" + typefor + "','" + Feature + (ds_proprtytype.Tables[0].Rows[i]["id"].ToString()) + "','" + Feature + "')\"  class='btn-check'  id='" + Feature + (ds_proprtytype.Tables[0].Rows[i]["id"].ToString()) + "'>";


                }


                strdata += "<label class='btn btn-sm rounded-pill btn-outline-primary' for='" + Feature + (ds_proprtytype.Tables[0].Rows[i]["id"].ToString()) + "'>" + ds_proprtytype.Tables[0].Rows[i]["name"].ToString() + "</label>";

            }

            strdata += "</div>";
        }


        return strdata;
    }
    protected void bindReason()
    {
        string dataN = "";
        string strsql = "select id,name  from tbl_crmmasterdata where type in(16,17,18) and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  order by type";
        DataSet dsN = da.GetDataSetInline(strsql);
        if (dsN.Tables[0].Rows.Count > 0)
        {
            for (int i1 = 0; i1 < dsN.Tables[0].Rows.Count; i1++)
            {
                if (dataN != string.Empty)
                {
                    dataN += ",{";
                    dataN += "id: '" + dsN.Tables[0].Rows[i1]["id"].ToString() + "',";
                    dataN += "text: '" + dsN.Tables[0].Rows[i1]["name"].ToString() + "'";
                    dataN += "}";
                }
                else
                {
                    dataN += "{";
                    dataN += "id: '" + dsN.Tables[0].Rows[i1]["id"].ToString() + "',";
                    dataN += "text: '" + dsN.Tables[0].Rows[i1]["name"].ToString() + "'";
                    dataN += "}";
                }
            }
        }
        mcatReason = dataN.ToString();

        //Response.Write(mcatReason);
        //ListItem li = new ListItem("", "0");
        //ddlemployee.Items.Insert(0, li);
    }
    protected int checkadmin(string uids)
    {
        int retval = 0;
        string strsqladm = "select Manage_Profile.* from Manage_Profile inner join tbl_employee on Manage_Profile.id=cast(tbl_employee.roleid as int) where tbl_employee.id='" + uids + "'  and Manage_Profile.status=1 and Manage_Profile.archive=0";
        DataSet dsadm = da.GetDataSetInline(strsqladm);
        if (dsadm.Tables[0].Rows.Count > 0)
        {
            if (dsadm.Tables[0].Rows[0]["id"].ToString() == "1")
            {
                retval = 1;

            }
        }

        return retval;

    }
    protected void ButtonleadB_Click(object sender, EventArgs e)
    {
        string typefor = hdnbindtype.Value;
        string strsql = "";
        if (typefor == "")
        {
            strsql = "select id,isnull(name,0) minimum,isnull(maximum,0) maximum ,isnull(interval,0) interval  from tbl_crmmasterdata where type =27 and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and isnull(purpose,0)=2  order by name";
        }
        else
        {
            strsql = "select id,isnull(name,0) minimum,isnull(maximum,0) maximum ,isnull(interval,0) interval  from tbl_crmmasterdata where type =27 and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and isnull(purpose,0)='" + typefor + "'  order by name";
        }

        DataSet dsN = da.GetDataSetInline(strsql);
        if (dsN.Tables[0].Rows.Count > 0)
        {
            ddlmaxBudget.Items.Clear();
            ListItem li1 = new ListItem("", "0");
            ddlmaxBudget.Items.Insert(0, li1);
            int min = Convert.ToInt32(ddlminBudget.SelectedValue == "" ? "0" : ddlminBudget.SelectedValue);

            int max = Convert.ToInt32(dsN.Tables[0].Rows[0]["maximum"].ToString());
            int inerval = Convert.ToInt32(dsN.Tables[0].Rows[0]["interval"].ToString());

            int value = 0;
            int cnt = 0;
            while (max >= min)
            {

                if (cnt == 0)
                {
                    value += min;
                }
                else
                {
                    value += inerval;
                }
                max -= inerval;
                cnt++;

                ListItem li = new ListItem(value.ToString(), value.ToString());
                ddlmaxBudget.Items.Add(li);
            }

        }
        else
        {
            ListItem li1 = new ListItem("", "0");
            ddlmaxBudget.Items.Insert(0, li1);
        }
    }
    protected void ButtonleadS_Click(object sender, EventArgs e)
    {
        ddlmaxsize.Items.Clear();
        string typefor = hdnbindtype.Value;
        string strsql = "";
        strsql = "select id,isnull(name,0) minimum,isnull(maximum,0) maximum ,isnull(interval,0) interval  from tbl_crmmasterdata where type =28 and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  and isnull(purpose,0)='2'   order by name";
        if (typefor != "")
        {
            strsql = "select id,isnull(name,0) minimum,isnull(maximum,0) maximum ,isnull(interval,0) interval  from tbl_crmmasterdata where type =28 and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and isnull(purpose,0)='" + typefor + "'  order by name";
        }

        DataSet dsN = da.GetDataSetInline(strsql);
        if (dsN.Tables[0].Rows.Count > 0)
        {
            ListItem li1 = new ListItem("", "0");
            ddlmaxsize.Items.Insert(0, li1);
            int min = Convert.ToInt32(ddlminsize.SelectedValue == "" ? "0" : ddlminsize.SelectedValue);

            int max = Convert.ToInt32(dsN.Tables[0].Rows[0]["maximum"].ToString());
            int inerval = Convert.ToInt32(dsN.Tables[0].Rows[0]["interval"].ToString());

            int value = 0;
            int cnt = 0;
            while (max >= min)
            {

                if (cnt == 0)
                {
                    value += min;
                }
                else
                {
                    value += inerval;
                }
                max -= inerval;
                cnt++;

                ListItem li = new ListItem(value.ToString(), value.ToString());

                ddlmaxsize.Items.Add(li);
            }

        }
        else
        {
            ListItem li1 = new ListItem("", "0");
            ddlmaxsize.Items.Insert(0, li1);
        }
    }
    [WebMethod]
    public static List<ListItem> Getminsize(string typefor)
    {
        DataAccess da1 = new DataAccess();
        // Prepare some dummy data for second dropdown based on the first dropdown value
        List<ListItem> items = new List<ListItem>();
        items.Add(new ListItem("", ""));
        string strsql = "";
        strsql = "select id,isnull(name,0) minimum,isnull(maximum,0) maximum ,isnull(interval,0) interval  from tbl_crmmasterdata where type =28 and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  and isnull(purpose,0)='2'   order by name";
        if (typefor != "")
        {
            strsql = "select id,isnull(name,0) minimum,isnull(maximum,0) maximum ,isnull(interval,0) interval  from tbl_crmmasterdata where type =28 and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and isnull(purpose,0)='" + typefor + "'  order by name";
        }

        DataSet dsN = da1.GetDataSetInline(strsql);
        if (dsN.Tables[0].Rows.Count > 0)
        {
            int min = Convert.ToInt32(dsN.Tables[0].Rows[0]["minimum"].ToString());

            int max = Convert.ToInt32(dsN.Tables[0].Rows[0]["maximum"].ToString());
            int inerval = Convert.ToInt32(dsN.Tables[0].Rows[0]["interval"].ToString());

            int value = 0;
            int cnt = 0;
            while (max >= min)
            {

                if (cnt == 0)
                {
                    value += min;
                }
                else
                {
                    value += inerval;
                }
                max -= inerval;
                cnt++;
                items.Add(new ListItem(value.ToString(), value.ToString()));



            }
        }


        return items;
    }
    [WebMethod]
    public static List<ListItem> Getmaxsize(string minsize, string typefor)
    {
        DataAccess da1 = new DataAccess();
        // Prepare some dummy data for second dropdown based on the first dropdown value
        List<ListItem> items = new List<ListItem>();
        items.Add(new ListItem("", ""));
        string strsql = "";
        strsql = "select id,isnull(name,0) minimum,isnull(maximum,0) maximum ,isnull(interval,0) interval  from tbl_crmmasterdata where type =28 and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  and isnull(purpose,0)='2'   order by name";
        if (typefor != "")
        {
            strsql = "select id,isnull(name,0) minimum,isnull(maximum,0) maximum ,isnull(interval,0) interval  from tbl_crmmasterdata where type =28 and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and isnull(purpose,0)='" + typefor + "'  order by name";
        }

        DataSet dsN = da1.GetDataSetInline(strsql);
        if (dsN.Tables[0].Rows.Count > 0)
        {
            int min = Convert.ToInt32(minsize == "" ? "0" : minsize);

            int max = Convert.ToInt32(dsN.Tables[0].Rows[0]["maximum"].ToString());
            int inerval = Convert.ToInt32(dsN.Tables[0].Rows[0]["interval"].ToString());

            int value = 0;
            int cnt = 0;
            while (max >= min)
            {

                if (cnt == 0)
                {
                    value += min;
                }
                else
                {
                    value += inerval;
                }
                max -= inerval;
                cnt++;
                items.Add(new ListItem(value.ToString(), value.ToString()));



            }
        }


        return items;
    }
    [WebMethod]
    public static List<ListItem> Getminbudget(string typefor)
    {
        DataAccess da1 = new DataAccess();
        // Prepare some dummy data for second dropdown based on the first dropdown value
        List<ListItem> items = new List<ListItem>();
        items.Add(new ListItem("", ""));
        string strsql = "";
        strsql = "select id,isnull(name,0) minimum,isnull(maximum,0) maximum ,isnull(interval,0) interval  from tbl_crmmasterdata where type =27 and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and isnull(purpose,0)=2  order by name";
        if (typefor == "")
        {
            strsql = "select id,isnull(name,0) minimum,isnull(maximum,0) maximum ,isnull(interval,0) interval  from tbl_crmmasterdata where type =27 and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and isnull(purpose,0)=2  order by name";
        }
        else
        {
            strsql = "select id,isnull(name,0) minimum,isnull(maximum,0) maximum ,isnull(interval,0) interval  from tbl_crmmasterdata where type =27 and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and isnull(purpose,0)='" + typefor + "'  order by name";
        }

        DataSet dsN = da1.GetDataSetInline(strsql);
        if (dsN.Tables[0].Rows.Count > 0)
        {
            int min = Convert.ToInt32(dsN.Tables[0].Rows[0]["minimum"].ToString());

            int max = Convert.ToInt32(dsN.Tables[0].Rows[0]["maximum"].ToString());
            int inerval = Convert.ToInt32(dsN.Tables[0].Rows[0]["interval"].ToString());

            int value = 0;
            int cnt = 0;
            while (max >= min)
            {

                if (cnt == 0)
                {
                    value += min;
                }
                else
                {
                    value += inerval;
                }
                max -= inerval;
                cnt++;
                items.Add(new ListItem(value.ToString(), value.ToString()));



            }
        }

        return items;
    }
    [WebMethod]
    public static List<ListItem> Getmaxbudget(string minsize, string typefor)
    {
        DataAccess da1 = new DataAccess();
        // Prepare some dummy data for second dropdown based on the first dropdown value
        List<ListItem> items = new List<ListItem>();
        items.Add(new ListItem("", ""));
        string strsql = "";
        strsql = "select id,isnull(name,0) minimum,isnull(maximum,0) maximum ,isnull(interval,0) interval  from tbl_crmmasterdata where type =27 and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and isnull(purpose,0)=2  order by name";
        if (typefor == "")
        {
            strsql = "select id,isnull(name,0) minimum,isnull(maximum,0) maximum ,isnull(interval,0) interval  from tbl_crmmasterdata where type =27 and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and isnull(purpose,0)=2  order by name";
        }
        else
        {
            strsql = "select id,isnull(name,0) minimum,isnull(maximum,0) maximum ,isnull(interval,0) interval  from tbl_crmmasterdata where type =27 and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and isnull(purpose,0)='" + typefor + "'  order by name";
        }

        DataSet dsN = da1.GetDataSetInline(strsql);
        if (dsN.Tables[0].Rows.Count > 0)
        {
            int min = Convert.ToInt32(minsize == "" ? "0" : minsize);

            int max = Convert.ToInt32(dsN.Tables[0].Rows[0]["maximum"].ToString());
            int inerval = Convert.ToInt32(dsN.Tables[0].Rows[0]["interval"].ToString());

            int value = 0;
            int cnt = 0;
            while (max >= min)
            {

                if (cnt == 0)
                {
                    value += min;
                }
                else
                {
                    value += inerval;
                }
                max -= inerval;
                cnt++;
                items.Add(new ListItem(value.ToString(), value.ToString()));



            }
        }

        return items;
    }
    [WebMethod]
    public static string getsearchvalue(string hdisrenbuy, string hdisResidential, string isrentids, string hdnbuyid, string hdnlistingbeds, string hdnlistingBaths, string hdnresidentialval, string hdncommercialval, string hdnlistingstatus, string hdnlocation, string minbudget, string maxbudget, string minsize, string maxsize)
    {

        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();

        string str_selecteddeatil = "";
        string strrentbuy1 = "";

        if (hdisrenbuy != "" && hdisrenbuy != "0")
        {
            if (hdisrenbuy == "1")
            {
                strrentbuy1 = "<b>(Buy)</b>";
            }
            else if (hdisrenbuy == "2")
            {
                strrentbuy1 = "<b>(Rent)</b>";
            }
            else
            {
                strrentbuy1 = "";
            }
            if (str_selecteddeatil == "")
            {
                //str_selecteddeatil += (hdisrenbuy=="2"?" Rent: ":" Buy: ");
                str_selecteddeatil += strrentbuy1 + " ";
            }
            else
            {
                //str_selecteddeatil += " &"+(hdisrenbuy=="2"?"Rent: ":"Buy: ");
                str_selecteddeatil += strrentbuy1 + " ";
            }

        }
        if (isrentids != "" && isrentids != "0" && hdisrenbuy == "2")
        {
            if (str_selecteddeatil == "")
            {
                str_selecteddeatil += "(" + getpurposedata("2", isrentids) + ")";
            }
            else
            {
                str_selecteddeatil += "(" + getpurposedata("2", isrentids) + ")";
            }

        }
        if (hdnbuyid != "" && hdnbuyid != "0" && hdisrenbuy == "1")
        {
            //Response.Write(getpurposedata("2", hdnbuyid.Value));
            //Response.End();
            if (str_selecteddeatil == "")
            {
                str_selecteddeatil += "(" + getpurposedata("1", hdnbuyid) + ")";
            }
            else
            {
                str_selecteddeatil += "(" + getpurposedata("1", hdnbuyid) + ")";
            }

        }
        if (hdnlocation != "")
        {
            if (str_selecteddeatil == "")
            {
                //str_selecteddeatil += " Location: " + getlocationdata("16,17,18", hdnlocation);
                str_selecteddeatil += " (" + getlocationdata("16,17,18", hdnlocation) + " -<i class='ri-map-pin-2-fill search-icon'></i>)";
            }
            else
            {
                //str_selecteddeatil += " &Location: " + getlocationdata("16,17,18", hdnlocation);
                str_selecteddeatil += " (" + getlocationdata("16,17,18", hdnlocation) + " -<i class='ri-map-pin-2-fill search-icon'></i>)";
            }
        }
        if (hdnlistingbeds != "" && hdnlistingbeds != "0")
        {
            //if (str_selecteddeatil == "")
            //{
            //    str_selecteddeatil += " Beds: " + getlistingdata("13", hdnlistingbeds);
            //}
            //else
            //{
            //    str_selecteddeatil += " &Beds: " + getlistingdata("13", hdnlistingbeds);
            //}
            str_selecteddeatil += " (" + getlistingdata("13", hdnlistingbeds) + " -<i class='ti ti-bed'></i>)";

        }
        string strr1 = "";
        if (hdisResidential != "" && hdisResidential != "0")
        {
            //if (str_selecteddeatil == "")
            //{
            //    str_selecteddeatil += (hdisResidential == "2" ? " Commercial: " : " Residential: ");
            //}
            //else
            //{
            //    str_selecteddeatil += " &" + (hdisResidential == "2" ? "Commercial: " : "Residential: ");
            //}
            strr1 = " <b>-" + (hdisResidential == "2" ? "Commercial" : "Residential") + "</b>";
        }
        if (hdnresidentialval != "" && hdnresidentialval != "0" && hdisResidential == "1")
        {
            //if (str_selecteddeatil == "")
            //{
            //    str_selecteddeatil += getresidentialdata("1", hdnresidentialval);
            //}
            //else
            //{
            //    str_selecteddeatil += getresidentialdata("1", hdnresidentialval);
            //}
            str_selecteddeatil += " (" + getresidentialdata("1", hdnresidentialval) + "" + strr1 + ")";

        }
        else if (hdncommercialval != "" && hdncommercialval != "0" && hdisResidential == "2")
        {
            //if (str_selecteddeatil == "")
            //{
            //    str_selecteddeatil +=getresidentialdata("2", hdncommercialval);
            //}
            //else
            //{
            //    str_selecteddeatil +=getresidentialdata("2", hdncommercialval);
            //}
            str_selecteddeatil += " (" + getresidentialdata("2", hdncommercialval) + "" + strr1 + ")";

        }
        else
        {
            strr1 = " <b>(" + (hdisResidential == "2" ? "Commercial" : "Residential") + ")</b>";
            str_selecteddeatil += strr1;
        }
        if (hdnlistingstatus != "" && hdnlistingstatus != "0")
        {
            //if (str_selecteddeatil == "")
            //{
            //    str_selecteddeatil += " Status: " + getlistingdata("15", hdnlistingstatus);
            //}
            //else
            //{
            //    str_selecteddeatil += " &Status: " + getlistingdata("15", hdnlistingstatus);
            //}
            str_selecteddeatil += "  (" + getlistingdata("15", hdnlistingstatus) + " <b>-Status</b>)";
        }

        string budg = "";
        if (minbudget != "0" && minbudget != "")
        {
            //Response.Write(dsrent.Tables[0].Rows[0]["budget_min"].ToString());
            //Response.End();
            //budg = "Budget: ";

            budg += " (" + minbudget;


        }
        if (maxbudget != "0" && maxbudget != "")
        {
            if (budg == "")
            {
                // budg += "Budget: " + maxbudget;
                budg += " (" + maxbudget + " <b> AED</b>)";
                // getlistingdata("27", dsrent.Tables[0].Rows[0]["budget_max"].ToString());
            }
            else
            {
                budg += " - " + maxbudget + " <b> AED</b>)";// getlistingdata("27", dsrent.Tables[0].Rows[0]["budget_max"].ToString());
            }

        }
        else
        {
            if (budg != "")
            {
                budg += "  <b>-AED</b>)";
            }

        }
        if (budg != "")
        {
            //str_selecteddeatil += " &" + budg;
            str_selecteddeatil += budg;
        }

        string siz = "";
        if (minsize != "0" && minsize != "")
        {
            //siz = "Size: ";
            siz = " (";

            siz += minsize;// getlistingdata("28", dsrent.Tables[0].Rows[0]["size_min"].ToString());

        }
        if (maxsize != "0" && maxsize != "")
        {

            if (siz == "")
            {
                //siz += "Size: " + maxsize;
                siz += " (" + maxsize + " <b> Sqft</b>)";
                // getlistingdata("28", dsrent.Tables[0].Rows[0]["size_max"].ToString());
            }
            else
            {
                //siz += " - " + maxsize;
                siz += " - " + maxsize + " <b> Sqft</b>)";
                // getlistingdata("28", dsrent.Tables[0].Rows[0]["size_max"].ToString());
            }
        }
        else
        {
            if (siz != "")
            {
                siz += "  <b> Sqft</b>)";
            }
        }
        if (siz != "")
        {
            //str_selecteddeatil += " &" + siz;
            str_selecteddeatil += siz;
        }





        return str_selecteddeatil;
    }
    protected void btnbind_Click(object sender, EventArgs e)
    {
        string typval = hdnbindtype.Value;
        if (typval == "2")
        {
            bind_minmaxbudget(typval);
            bind_minmaxsize(typval);
        }
        else if (typval == "1")
        {
            bind_minmaxbudget(typval);
            bind_minmaxsize(typval);
        }

    }
    protected void btncountry_Click(object sender, EventArgs e)
    {


        bindleadcountrystates(CountryId.SelectedValue);
        string chkcountryid = getcontry_map();
        //if (chkcountryid == "77")
        //{
        if (CountryId.SelectedValue == "77")
        {
            isshowstateddl = "style='display:none'";
            StateId.Visible = true;
            CityId.Visible = true;
            pnlstate.Visible = false;
            pnlcity.Visible = false;
            strdvcity = "style='display:none'";
        }
        else
        {
            isshowstateddl = "style='display:'";
            StateId.Visible = false;
            CityId.Visible = false;
            pnlstate.Visible = true;
            pnlcity.Visible = true;
            strdvcity = "";
        }


    }
    protected void btnstate_Click(object sender, EventArgs e)
    {

        bindleadstatecity(StateId.SelectedValue);
        //string chkcountryid = getcontry_map();
        //if (chkcountryid == "77")
        //{
        //}

    }
    protected void btncity_Click(object sender, EventArgs e)
    {
        if (CityId.SelectedValue == "-1")
        {
            pnlcity.Visible = true;
            strdvcity = "";
        }
        else
        {
            pnlcity.Visible = false;
            strdvcity = "style='display:none'";
        }

    }
    protected bool checkitemname()
    {
        SqlParameter[] prm = new SqlParameter[5];
        prm[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"].ToString());
        prm[1] = new SqlParameter("@name", txtnewindustry.Text.Trim());
        prm[2] = new SqlParameter("@code", "");
        prm[3] = new SqlParameter("@type", "5");
        prm[4] = new SqlParameter("@typevalue", "Check");
        DataSet dsc = da.GetDataSet("crm_ups_tbl_crmmasterdata_DML", prm);
        if (dsc.Tables[0].Rows.Count > 0)
        {
            return false;
        }

        return true;
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {

        //if (HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString()=="2201"|| HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() == "2203")
        //{
        if (CountryId.SelectedValue == "77")
        {
            if (Industry.SelectedValue == "" || Industry.SelectedValue == "0" || Industry.SelectedValue == "-1")
            {

                if (Industry.SelectedValue == "-1")
                {
                    if (txtnewindustry.Text.Trim() == "")
                    {
                        if (CityId.SelectedValue == "-1")
                        {
                            pnlcity.Visible = true;
                            strdvcity = "";
                        }
                        if (CityId.SelectedValue == "-1")
                        {
                            pnlcity.Visible = true;
                            strdvcity = "";
                        }
                        Page.RegisterClientScriptBlock("", "<script>alert('Please select industry!'); </script>");
                        return;
                    }
                }
                else
                {
                    if (CityId.SelectedValue == "-1")
                    {
                        pnlcity.Visible = true;
                        strdvcity = "";
                    }
                    if (CityId.SelectedValue == "-1")
                    {
                        pnlcity.Visible = true;
                        strdvcity = "";
                    }
                    Page.RegisterClientScriptBlock("", "<script>alert('Please select industry!'); </script>");
                    return;
                }
            }
            if (StateId.SelectedValue == "" || StateId.SelectedValue == "0" || StateId.SelectedValue == "-1")
            {
                Page.RegisterClientScriptBlock("", "<script>alert('Please select state!'); </script>");
                return;
            }
            if (CityId.SelectedValue == "" || CityId.SelectedValue == "0")
            {
                Page.RegisterClientScriptBlock("", "<script>alert('Please select city!'); </script>");
                return;
            }
        }
        //}
        //try
        //{
        DataAccess da1 = new DataAccess();
        string editid = hdnleadeditid.Value;
        string entrytype = "Insert";
        string LeadSourceid = "0";
        string IndustryTypeid = "0";
        string ownerid = "0";
        string leadStatusid = "0";
        string Salutationid = "0";
        string isdouplicateid = "";

        if (editid != "" && editid != "0")
        {
            entrytype = "Update";
            if (LeadSource.SelectedValue == "0")
            {
                LeadSourceid = "0";
            }
            else
            {
                if (hdnnewsource.Value == "1" && txtnewsource.Value != "")
                {
                    try
                    {
                        SqlParameter[] prm1 = new SqlParameter[4];
                        prm1[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                        prm1[1] = new SqlParameter("@name", txtnewsource.Value);
                        prm1[2] = new SqlParameter("@typevalue", "check");
                        prm1[3] = new SqlParameter("@type", "1");
                        DataSet dsc = da1.GetDataSet("crm_ups_tbl_crmmasterdata", prm1);
                        if (dsc.Tables[0].Rows.Count > 0)
                        {
                            LeadSourceid = dsc.Tables[0].Rows[0]["id"].ToString();
                        }
                        else
                        {
                            SqlParameter[] prm2 = new SqlParameter[5];
                            prm2[0] = new SqlParameter("@type", "1");
                            prm2[3] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                            prm2[1] = new SqlParameter("@name", txtnewsource.Value);
                            prm2[2] = new SqlParameter("@typevalue", "insert");
                            prm2[4] = new SqlParameter("ReturnValue", SqlDbType.Int);
                            prm2[4].Direction = ParameterDirection.ReturnValue;
                            int id = da1.executeDMLQuery("crm_ups_tbl_crmmasterdata_New", prm2);
                            LeadSourceid = id.ToString();

                        }
                    }
                    catch
                    { }
                }
                else
                {
                    LeadSourceid = LeadSource.SelectedValue;
                }
            }
            if (Salutation.SelectedValue == "" || Salutation.SelectedValue == "-1")
            {
                Salutationid = "0";
            }
            else
            {
                Salutationid = Salutation.SelectedValue;
            }
            if (Industry.SelectedValue == "0" || Industry.SelectedValue == "-1")
            {
                IndustryTypeid = "0";
                if (checkitemname())
                {

                    SqlParameter[] prmnew = new SqlParameter[7];
                    prmnew[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"]);
                    prmnew[1] = new SqlParameter("@name", txtnewindustry.Text.Trim());
                    prmnew[2] = new SqlParameter("@code", "");
                    prmnew[3] = new SqlParameter("@id", DBNull.Value);
                    prmnew[4] = new SqlParameter("@type", "5");
                    prmnew[5] = new SqlParameter("@typevalue", "Insert");
                    prmnew[6] = new SqlParameter("ReturnValue", SqlDbType.Int);
                    prmnew[6].Direction = ParameterDirection.ReturnValue;
                    int retid = da.executeDMLQuery("crm_ups_tbl_crmmasterdata_DML", prmnew);
                    IndustryTypeid = retid.ToString();

                }
                else
                {
                    string sqlchk = "select * from tbl_crmmasterdata where status=1 and archive=0 and regid='" + Request.Cookies["cadmin"]["id"] + "' and name='" + txtnewindustry.Text.Trim() + "' ";
                    DataSet dsc = da.GetDataSetInline(sqlchk);
                    if (dsc.Tables[0].Rows.Count > 0)
                    {
                        IndustryTypeid = dsc.Tables[0].Rows[0]["id"].ToString();
                    }
                }


            }
            else
            {
                IndustryTypeid = Industry.SelectedValue;
            }
            if (Owner.SelectedValue == "" || Owner.SelectedValue == "-1")
            {
                ownerid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();
            }
            else
            {
                ownerid = Owner.SelectedValue;
            }

            if (leadStatus.SelectedValue == "" || leadStatus.SelectedValue == "-1")
            {
                string strleadstatus = "select id  from tbl_leadstatus where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and isnull(IsDefault,0)=1";
                DataSet dschk = da1.GetDataSetInline(strleadstatus);
                if (dschk.Tables[0].Rows.Count > 0)
                {
                    leadStatusid = dschk.Tables[0].Rows[0]["id"].ToString();
                }

            }
            else
            {
                leadStatusid = leadStatus.SelectedValue;
            }
            string cmpname = cName.Text;
            if (cmpname == "")
            {
                cmpname = fName.Text;
                if (lName.Text.Trim() != "")
                {
                    cmpname += " " + lName.Text;
                }
            }
            string chkcountryid = getcontry_map();
            string Pcityid = "0";
            string Pstateid = "0";
            string Pcityname = CityName.Value;
            string Pstatename = StateName.Value;
            if (chkcountryid == "77")
            {
                if (StateId.SelectedValue != "" && StateId.SelectedValue != "-1")
                {
                    Pstateid = StateId.SelectedValue;
                    Pstatename = StateId.SelectedItem.Text;
                }
                if (CityId.SelectedValue != "" && CityId.SelectedValue != "-1")
                {
                    Pcityid = CityId.SelectedValue;
                    Pcityname = CityId.SelectedItem.Text;
                }
                else if (CityId.SelectedValue == "-1")
                {
                    if (CityName.Value != string.Empty)
                    {
                        SqlParameter[] prmcity = new SqlParameter[4];
                        prmcity[0] = new SqlParameter("@Stateid", Pstateid);
                        prmcity[1] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"]);
                        prmcity[2] = new SqlParameter("ReturnValue", SqlDbType.Int);
                        prmcity[2].Direction = ParameterDirection.ReturnValue;
                        prmcity[3] = new SqlParameter("@cityName", CityName.Value);
                        Pcityid = da.executeDMLQuery("insertcity", prmcity).ToString();
                        Pcityname = CityName.Value;

                    }
                }
            }

            SqlParameter[] prm = new SqlParameter[35];
            prm[0] = new SqlParameter("@id", editid);
            prm[1] = new SqlParameter("@type", entrytype);
            prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
            prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
            prm[5] = new SqlParameter("@Salutation", (Salutation.SelectedValue == "" ? "0" : Salutation.SelectedValue));
            prm[6] = new SqlParameter("@FirstName", fName.Text);
            prm[7] = new SqlParameter("@LastName", lName.Text);
            prm[8] = new SqlParameter("@Company", cmpname);// cName.Text);
            prm[9] = new SqlParameter("@Title", title.Text);
            prm[10] = new SqlParameter("@Website", website.Text);
            prm[11] = new SqlParameter("@Descriptions", description.Value);
            prm[12] = new SqlParameter("@LeadStatus", leadStatusid);
            prm[13] = new SqlParameter("@OwnerName", ownerid);
            prm[14] = new SqlParameter("@PhoneNo", PhoneNo.Text);
            prm[15] = new SqlParameter("@EmailId", EmailId.Text);
            prm[16] = new SqlParameter("@LAddress", AddressDetails.Value);
            prm[17] = new SqlParameter("@Street", StreetName.Value);
            prm[18] = new SqlParameter("@CityId", Pcityid);
            prm[19] = new SqlParameter("@CityName", Pcityname);
            prm[20] = new SqlParameter("@PostalCode", PostalCode.Value);
            prm[21] = new SqlParameter("@StateId", Pstateid);
            prm[22] = new SqlParameter("@StateName", Pstatename);
            prm[23] = new SqlParameter("@CountryId", CountryId.SelectedValue);
            prm[24] = new SqlParameter("@CountryName", "");
            prm[25] = new SqlParameter("@NoOfEmployee", NoOfemp.Value);
            prm[26] = new SqlParameter("@AnnualRevenue", AnnualRevenue.Value);
            prm[27] = new SqlParameter("@LeadSourceid", LeadSourceid);
            prm[28] = new SqlParameter("@IndustryTypeid", IndustryTypeid);
            prm[29] = new SqlParameter("@Leadproduct", hdnddLeadProduct.Value);
            prm[30] = new SqlParameter("@Satageupdatetime", da1.getcdatetime());
            prm[31] = new SqlParameter("@LProbability", hdnleadProbability.Value);
            prm[32] = new SqlParameter("@TeleGramBoatId", boatid.Text);
            prm[33] = new SqlParameter("@leadnotes", txtlnotes.Value);
            prm[34] = new SqlParameter("ReturnValue", SqlDbType.Int);
            prm[34].Direction = ParameterDirection.ReturnValue;
            int rid = da1.executeDMLQuery("crm_tbl_mainleads_Insert", prm);
            string companyid = "0";
            string contactid = "0";
            if (rid > 0)
            {
                string strsqlchk = "";

                strsqlchk = "select id,Company from tbl_LeadsCompanies where status='1' and archive='0' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and leadid='" + rid + "'";
                //SaveLogText("strsqlchk1:"+ strsqlchk);
                DataSet dschk = da1.GetDataSetInline(strsqlchk);
                if (dschk.Tables[0].Rows.Count > 0)
                {
                    //SaveLogText("strsqlchk2:ok");
                    companyid = dschk.Tables[0].Rows[0]["id"].ToString();
                    prm = new SqlParameter[20];
                    prm[0] = new SqlParameter("@id", companyid);
                    prm[1] = new SqlParameter("@entrytype", "Update");
                    prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                    prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
                    prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
                    prm[5] = new SqlParameter("@Company", cmpname);
                    prm[6] = new SqlParameter("@CompanyOwner", ownerid);
                    prm[7] = new SqlParameter("@Website", website.Text);
                    prm[8] = new SqlParameter("@PhoneNo", PhoneNo.Text);
                    prm[9] = new SqlParameter("@BillingAddress", AddressDetails.Value);
                    prm[10] = new SqlParameter("@Street", StreetName.Value);
                    prm[11] = new SqlParameter("@CityId", Pcityid);
                    prm[12] = new SqlParameter("@CityName", Pcityname);
                    prm[13] = new SqlParameter("@PostalCode", PostalCode.Value);
                    prm[14] = new SqlParameter("@StateId", Pstateid);
                    prm[15] = new SqlParameter("@StateName", Pstatename);
                    prm[16] = new SqlParameter("@CountryId", (CountryId.SelectedValue == "" ? "0" : CountryId.SelectedValue));
                    prm[17] = new SqlParameter("@CountryName", CountryId.SelectedItem.Text);
                    prm[18] = new SqlParameter("@leadid", rid);
                    prm[19] = new SqlParameter("@IndustryTypeid", IndustryTypeid);
                    da.executeDMLQuery("crm_tbl_LeadsCompanies_Insert", prm);
                }
                else
                {
                    //strsqlchk = "select id,Company from tbl_LeadsCompanies where status='1' and archive='0' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and Company='" + cmpname + "' and PhoneNo='" + PhoneNo.Text + "'";
                    //dschk = da1.GetDataSetInline(strsqlchk);
                    SqlParameter[] prmc = new SqlParameter[3];
                    prmc[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                    prmc[1] = new SqlParameter("@company", cmpname);
                    prmc[2] = new SqlParameter("@phonno", PhoneNo.Text);
                    dschk = da.GetDataSet("getcompanyid", prmc);
                    if (dschk.Tables[0].Rows.Count > 0)
                    {
                        companyid = dschk.Tables[0].Rows[0]["id"].ToString();
                        prm = new SqlParameter[20];
                        prm[0] = new SqlParameter("@id", companyid);
                        prm[1] = new SqlParameter("@entrytype", "Update");
                        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
                        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
                        prm[5] = new SqlParameter("@Company", cmpname);
                        prm[6] = new SqlParameter("@CompanyOwner", ownerid);
                        prm[7] = new SqlParameter("@Website", website.Text);
                        prm[8] = new SqlParameter("@PhoneNo", PhoneNo.Text);
                        prm[9] = new SqlParameter("@BillingAddress", AddressDetails.Value);
                        prm[10] = new SqlParameter("@Street", StreetName.Value);
                        prm[11] = new SqlParameter("@CityId", Pcityid);
                        prm[12] = new SqlParameter("@CityName", Pcityname);
                        prm[13] = new SqlParameter("@PostalCode", PostalCode.Value);
                        prm[14] = new SqlParameter("@StateId", Pstateid);
                        prm[15] = new SqlParameter("@StateName", Pstatename);
                        prm[16] = new SqlParameter("@CountryId", (CountryId.SelectedValue == "" ? "0" : CountryId.SelectedValue));
                        prm[17] = new SqlParameter("@CountryName", CountryId.SelectedItem.Text);
                        prm[18] = new SqlParameter("@leadid", rid);
                        prm[19] = new SqlParameter("@IndustryTypeid", IndustryTypeid);
                        da.executeDMLQuery("crm_tbl_LeadsCompanies_Insert", prm);

                    }
                    else
                    {
                        try
                        {
                            //SaveLogText("strsqlchk4:4");
                            prm = new SqlParameter[31];
                            prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                            prm[1] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
                            prm[2] = new SqlParameter("@cdate", da1.getcdatetime());
                            prm[3] = new SqlParameter("@Company", cmpname);
                            prm[4] = new SqlParameter("@CompanyOwner", ownerid);
                            prm[5] = new SqlParameter("@Website", website.Text);
                            prm[6] = new SqlParameter("@Descriptions", DBNull.Value);
                            prm[7] = new SqlParameter("@PhoneNo", PhoneNo.Text);
                            prm[8] = new SqlParameter("@BillingAddress", AddressDetails.Value);
                            prm[9] = new SqlParameter("@Street", StreetName.Value);
                            prm[10] = new SqlParameter("@CityId", Pcityid);
                            prm[11] = new SqlParameter("@CityName", Pcityname);
                            prm[12] = new SqlParameter("@PostalCode", PostalCode.Value);
                            prm[13] = new SqlParameter("@StateId", Pstateid);
                            prm[14] = new SqlParameter("@StateName", Pstatename);
                            prm[15] = new SqlParameter("@CountryId", (CountryId.SelectedValue == "" ? "0" : CountryId.SelectedValue));
                            prm[16] = new SqlParameter("@CountryName", CountryId.SelectedItem.Text);
                            prm[17] = new SqlParameter("@ShippingAddress", AddressDetails.Value);
                            prm[18] = new SqlParameter("@ShippingStreet", StreetName.Value);
                            prm[19] = new SqlParameter("@ShippingCityId", Pcityid);
                            prm[20] = new SqlParameter("@ShippingCityName", CityName.Value);
                            prm[21] = new SqlParameter("@ShippingPostalCode", PostalCode.Value);
                            prm[22] = new SqlParameter("@ShippingStateId", Pstateid);
                            prm[23] = new SqlParameter("@ShippingStateName", StateName.Value);
                            prm[24] = new SqlParameter("@ShippingCountryId", (CountryId.SelectedValue == "" ? "0" : CountryId.SelectedValue));

                            prm[25] = new SqlParameter("@ShippingCountryName", CountryId.SelectedItem.Text);
                            prm[26] = new SqlParameter("@leadid", rid);
                            prm[27] = new SqlParameter("@IndustryTypeid", IndustryTypeid);
                            prm[28] = new SqlParameter("@PhoneNoOther", PhoneNo.Text);

                            prm[29] = new SqlParameter("@NoOfEmployee", DBNull.Value);

                            prm[30] = new SqlParameter("ReturnValue", SqlDbType.Int);
                            prm[30].Direction = ParameterDirection.ReturnValue;

                            companyid = Convert.ToString(da.executeDMLQuery("crm_tbl_LeadsCompanies_ConvertedInsert", prm));
                            SaveLogText("companyid: " + companyid);
                        }
                        catch
                        { }
                    }

                }
                strsqlchk = "select id from tbl_LeadsCompanyContact where status='1' and archive='0' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and leadid='" + rid + "'";
                dschk = da1.GetDataSetInline(strsqlchk);
                if (dschk.Tables[0].Rows.Count > 0)
                {
                    contactid = dschk.Tables[0].Rows[0]["id"].ToString();
                    prm = new SqlParameter[29];
                    prm[0] = new SqlParameter("@id", contactid);
                    prm[1] = new SqlParameter("@entrytype", "Update");
                    prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                    prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
                    prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
                    prm[5] = new SqlParameter("@Salutation", Salutation.SelectedValue);
                    prm[6] = new SqlParameter("@FirstName", fName.Text);
                    prm[7] = new SqlParameter("@LastName", lName.Text);
                    prm[8] = new SqlParameter("@Companyid", companyid);
                    prm[9] = new SqlParameter("@Suffix", "");
                    prm[10] = new SqlParameter("@ContactOwner", ownerid);
                    prm[11] = new SqlParameter("@Designation", title.Text);
                    prm[12] = new SqlParameter("@PhoneNo", PhoneNo.Text);
                    prm[13] = new SqlParameter("@Emailid", EmailId.Text);
                    prm[14] = new SqlParameter("@MobNo", PhoneNo.Text);
                    prm[15] = new SqlParameter("@FaxbNo", "");
                    prm[16] = new SqlParameter("@ReportTo", "");
                    prm[17] = new SqlParameter("@DepartmentName", "");
                    prm[18] = new SqlParameter("@MailingAddress", AddressDetails.Value);
                    prm[19] = new SqlParameter("@MailingStreet", StreetName.Value);
                    prm[20] = new SqlParameter("@MailingCityId", Pcityid);
                    prm[21] = new SqlParameter("@MailingCityName", Pcityname);
                    prm[22] = new SqlParameter("@MailingPostalCode", PostalCode.Value);
                    prm[23] = new SqlParameter("@MailingStateId", Pstateid);
                    prm[24] = new SqlParameter("@MailingStateName", Pstatename);
                    prm[25] = new SqlParameter("@MailingCountryId", (CountryId.SelectedValue == "" ? "0" : CountryId.SelectedValue));
                    prm[26] = new SqlParameter("@MailingCountryName", CountryId.SelectedItem.Text);
                    prm[27] = new SqlParameter("@leadid", rid);
                    prm[28] = new SqlParameter("@Descriptions", DBNull.Value);
                    da.executeDMLQuery("crm_tbl_LeadsCompanyContact_Insert", prm);
                }
                else
                {
                    strsqlchk = "select id from tbl_LeadsCompanyContact where status='1' and archive='0' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and Salutation='" + Salutation.SelectedValue + "' and FirstName='" + fName.Text + "' and LastName='" + lName.Text + "'";
                    dschk = da1.GetDataSetInline(strsqlchk);
                    if (dschk.Tables[0].Rows.Count > 0)
                    {
                        contactid = dschk.Tables[0].Rows[0]["id"].ToString();
                        prm = new SqlParameter[29];
                        prm[0] = new SqlParameter("@id", contactid);
                        prm[1] = new SqlParameter("@entrytype", "Update");
                        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
                        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
                        prm[5] = new SqlParameter("@Salutation", Salutation.SelectedValue);
                        prm[6] = new SqlParameter("@FirstName", fName.Text);
                        prm[7] = new SqlParameter("@LastName", lName.Text);
                        prm[8] = new SqlParameter("@Companyid", companyid);
                        prm[9] = new SqlParameter("@Suffix", "");
                        prm[10] = new SqlParameter("@ContactOwner", ownerid);
                        prm[11] = new SqlParameter("@Designation", title.Text);
                        prm[12] = new SqlParameter("@PhoneNo", PhoneNo.Text);
                        prm[13] = new SqlParameter("@Emailid", EmailId.Text);
                        prm[14] = new SqlParameter("@MobNo", PhoneNo.Text);
                        prm[15] = new SqlParameter("@FaxbNo", "");
                        prm[16] = new SqlParameter("@ReportTo", "");
                        prm[17] = new SqlParameter("@DepartmentName", "");
                        prm[18] = new SqlParameter("@MailingAddress", AddressDetails.Value);
                        prm[19] = new SqlParameter("@MailingStreet", StreetName.Value);
                        prm[20] = new SqlParameter("@MailingCityId", Pcityid);
                        prm[21] = new SqlParameter("@MailingCityName", Pcityname);
                        prm[22] = new SqlParameter("@MailingPostalCode", PostalCode.Value);
                        prm[23] = new SqlParameter("@MailingStateId", Pstateid);
                        prm[24] = new SqlParameter("@MailingStateName", Pstatename);
                        prm[25] = new SqlParameter("@MailingCountryId", (CountryId.SelectedValue == "" ? "0" : CountryId.SelectedValue));
                        prm[26] = new SqlParameter("@MailingCountryName", CountryId.SelectedItem.Text);
                        prm[27] = new SqlParameter("@leadid", rid);
                        prm[28] = new SqlParameter("@Descriptions", DBNull.Value);
                        da.executeDMLQuery("crm_tbl_LeadsCompanyContact_Insert", prm);
                    }
                    else
                    {
                        try
                        {
                            prm = new SqlParameter[29];
                            prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                            prm[1] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
                            prm[2] = new SqlParameter("@cdate", da1.getcdatetime());
                            prm[3] = new SqlParameter("@Salutation", (Salutation.SelectedValue == "" ? "0" : Salutation.SelectedValue));
                            prm[4] = new SqlParameter("@FirstName", fName.Text);
                            prm[5] = new SqlParameter("@MiddleName", "");
                            prm[6] = new SqlParameter("@LastName", lName.Text);
                            prm[7] = new SqlParameter("@Companyid", DBNull.Value);
                            prm[8] = new SqlParameter("@Suffix", "");
                            prm[9] = new SqlParameter("@ContactOwner", ownerid);
                            prm[10] = new SqlParameter("@Designation", title.Text);
                            prm[11] = new SqlParameter("@PhoneNo", PhoneNo.Text);
                            prm[12] = new SqlParameter("@Emailid", EmailId.Text);
                            prm[13] = new SqlParameter("@MobNo", PhoneNo.Text);
                            prm[14] = new SqlParameter("@FaxbNo", "");
                            prm[15] = new SqlParameter("@ReportTo", "");
                            prm[16] = new SqlParameter("@DepartmentName", "");
                            prm[17] = new SqlParameter("@MailingAddress", AddressDetails.Value);
                            prm[18] = new SqlParameter("@MailingStreet", StreetName.Value);
                            prm[19] = new SqlParameter("@MailingCityId", Pcityid);
                            prm[20] = new SqlParameter("@MailingCityName", Pcityname);
                            prm[21] = new SqlParameter("@MailingPostalCode", PostalCode.Value);
                            prm[22] = new SqlParameter("@MailingStateId", Pstateid);
                            prm[23] = new SqlParameter("@MailingStateName", Pstatename);
                            prm[24] = new SqlParameter("@MailingCountryId", (CountryId.SelectedValue == "" ? "0" : CountryId.SelectedValue));
                            prm[25] = new SqlParameter("@MailingCountryName", CountryId.SelectedItem.Text);
                            prm[26] = new SqlParameter("@leadid", rid);
                            prm[27] = new SqlParameter("@Descriptions", DBNull.Value);
                            prm[28] = new SqlParameter("ReturnValue", SqlDbType.Int);
                            prm[28].Direction = ParameterDirection.ReturnValue;
                            contactid = Convert.ToString(da.executeDMLQuery("crm_tbl_LeadsCompanyContact_ConvertedInsert", prm));
                        }
                        catch
                        { }
                    }
                }


                try
                {
                    if (entrytype == "Insert")
                    {
                        try
                        {
                            SaveLogText("called method");
                            //setnotification(rid.ToString(), "20", "1");
                            sendnotificationlead(rid, 20, 1);

                            //method for client communication
                           
                        }
                        catch
                        { }


                        try
                        {
                            sendnotificationleadtoclient(rid, 27, 27);
                        }

                        catch
                        { }




                    }
                    else if (entrytype == "Update")
                    {
                        try
                        {
                            //setnotification(rid.ToString(), "20", "1");
                            sendnotificationlead(rid, 20, 2);
                        }
                        catch
                        { }
                    }


                }
                catch
                { }

                string strquery = "";
                string leadtype = "";

                if (Request.QueryString["leadtype"] != null)
                {
                    leadtype = Request.QueryString["leadtype"];
                    if (strquery == "")
                    {
                        strquery += "?leadtype=" + leadtype;
                    }
                    else
                    {
                        strquery += "&leadtype=" + leadtype;
                    }
                }
                string presearch = "";
                if (Request.QueryString["presearch"] != null)
                {
                    presearch = Request.QueryString["presearch"];
                    if (strquery == "")
                    {
                        strquery += "?presearch=" + presearch;
                    }
                    else
                    {
                        strquery += "&presearch=" + presearch;
                    }
                }

                if (Request.QueryString["callfrom"] != null)
                {
                    if (Request.QueryString["callfrom"].ToString() == "leaddetails")
                    {
                        string strid = "";
                        if (Request.QueryString["id"] != null)
                        {

                            if (strquery == "")
                            {
                                strquery += "?id=" + Cmf.Encrypt(Request.QueryString["id"].ToString());
                            }
                            else
                            {
                                strquery += "&id=" + Cmf.Encrypt(Request.QueryString["id"].ToString());
                            }
                            // strquery = strid + strquery;
                        }
                        Page.RegisterClientScriptBlock("", "<script>alert('Data Saved Successfully!!');window.parent.window.parent.document.location.href ='/crm/lead-details.aspx" + strquery + "'; </script>");
                    }
                    else if (Request.QueryString["callfrom"].ToString() == "lead")
                    {
                        Page.RegisterClientScriptBlock("", "<script>alert('Data Saved Successfully!!');window.parent.window.parent.document.location.href ='/crm/lead.aspx" + strquery + "'; </script>");
                    }
                    //else if (Request.QueryString["callfrom"].ToString() == "leaddetailsp")
                    //{
                    //    Page.RegisterClientScriptBlock("", "<script>alert('Data Saved Successfully!!');window.parent.document.location.href ='/crm/lead-detailsp.aspx" + strquery + "'; </script>");
                    //}
                }
                else
                {
                    Page.RegisterClientScriptBlock("", "<script>alert('Data Saved Successfully!!');window.parent.window.parent.document.location.href ='/crm/lead.aspx" + strquery + "'; </script>");
                    //Page.RegisterClientScriptBlock("", "<script>alert('Data Saved Successfully!!');window.parent.window.parent.document.location.href ='/crm/kanban_lead.aspx" + strquery + "'; </script>");
                }


                if (entrytype == "Insert")
                {
                    entrytype = "1";
                }
                else if (entrytype == "Update")
                {
                    entrytype = "2";
                }

            }


        }
        else
        {
            isdouplicateid = checkIsDouplicatelead(PhoneNo.Text.Trim()).ToString();
            if (isdouplicateid != "" && isdouplicateid != "0" && (HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() == "2201" || HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() == "2203" || HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() == "2176"))
            {
                da.executeDMLQueryinline("update tbl_mainleads set Islost=0 where id='" + isdouplicateid + "'");
                if (LeadSource.SelectedValue == "0")
                {
                    LeadSourceid = "0";
                }
                else
                {
                    if (hdnnewsource.Value == "1" && txtnewsource.Value != "")
                    {
                        try
                        {
                            SqlParameter[] prm1 = new SqlParameter[4];
                            prm1[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                            prm1[1] = new SqlParameter("@name", txtnewsource.Value);
                            prm1[2] = new SqlParameter("@typevalue", "check");
                            prm1[3] = new SqlParameter("@type", "1");
                            DataSet dsc = da1.GetDataSet("crm_ups_tbl_crmmasterdata", prm1);
                            if (dsc.Tables[0].Rows.Count > 0)
                            {
                                LeadSourceid = dsc.Tables[0].Rows[0]["id"].ToString();
                            }
                            else
                            {
                                SqlParameter[] prm2 = new SqlParameter[5];
                                prm2[0] = new SqlParameter("@type", "1");
                                prm2[3] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                                prm2[1] = new SqlParameter("@name", txtnewsource.Value);
                                prm2[2] = new SqlParameter("@typevalue", "insert");
                                prm2[4] = new SqlParameter("ReturnValue", SqlDbType.Int);
                                prm2[4].Direction = ParameterDirection.ReturnValue;
                                int id = da1.executeDMLQuery("crm_ups_tbl_crmmasterdata_New", prm2);
                                LeadSourceid = id.ToString();

                            }
                        }
                        catch
                        { }
                    }
                    else
                    {
                        LeadSourceid = LeadSource.SelectedValue;
                    }
                }
                if (Salutation.SelectedValue == "" || Salutation.SelectedValue == "-1")
                {
                    Salutationid = "0";
                }
                else
                {
                    Salutationid = Salutation.SelectedValue;
                }
                if (Industry.SelectedValue == "0" || Industry.SelectedValue == "-1")
                {
                    IndustryTypeid = "0";
                    if (checkitemname())
                    {

                        SqlParameter[] prmnew = new SqlParameter[7];
                        prmnew[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"]);
                        prmnew[1] = new SqlParameter("@name", txtnewindustry.Text.Trim());
                        prmnew[2] = new SqlParameter("@code", "");
                        prmnew[3] = new SqlParameter("@id", DBNull.Value);
                        prmnew[4] = new SqlParameter("@type", "5");
                        prmnew[5] = new SqlParameter("@typevalue", "Insert");
                        prmnew[6] = new SqlParameter("ReturnValue", SqlDbType.Int);
                        prmnew[6].Direction = ParameterDirection.ReturnValue;
                        int retid = da.executeDMLQuery("crm_ups_tbl_crmmasterdata_DML", prmnew);
                        IndustryTypeid = retid.ToString();

                    }
                    else
                    {
                        string sqlchk = "select * from tbl_crmmasterdata where status=1 and archive=0 and regid='" + Request.Cookies["cadmin"]["id"] + "' and name='" + txtnewindustry.Text.Trim() + "' ";
                        DataSet dsc = da.GetDataSetInline(sqlchk);
                        if (dsc.Tables[0].Rows.Count > 0)
                        {
                            IndustryTypeid = dsc.Tables[0].Rows[0]["id"].ToString();
                        }
                    }
                }
                else
                {
                    IndustryTypeid = Industry.SelectedValue;
                }
                ownerid = Getownerid(isdouplicateid);
                if (ownerid == "" || ownerid == "0")
                {
                    if (Owner.SelectedValue == "" || Owner.SelectedValue == "-1")
                    {
                        ownerid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();
                    }
                    else
                    {
                        ownerid = Owner.SelectedValue;
                    }
                }

                if (leadStatus.SelectedValue == "" || leadStatus.SelectedValue == "-1")
                {
                    string strleadstatus = "select id  from tbl_leadstatus where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and isnull(IsDefault,0)=1";
                    DataSet dschk = da1.GetDataSetInline(strleadstatus);
                    if (dschk.Tables[0].Rows.Count > 0)
                    {
                        leadStatusid = dschk.Tables[0].Rows[0]["id"].ToString();
                    }

                }
                else
                {
                    leadStatusid = leadStatus.SelectedValue;
                }
                string cmpname = cName.Text;
                if (cmpname == "")
                {
                    cmpname = fName.Text;
                    if (lName.Text.Trim() != "")
                    {
                        cmpname += " " + lName.Text;
                    }
                }
                string chkcountryid = getcontry_map();
                string Pcityid = "0";
                string Pstateid = "0";
                string Pcityname = CityName.Value;
                string Pstatename = StateName.Value;
                if (chkcountryid == "77")
                {
                    if (StateId.SelectedValue != "" && StateId.SelectedValue != "-1")
                    {
                        Pstateid = StateId.SelectedValue;
                        Pstatename = StateId.SelectedItem.Text;
                    }
                    if (CityId.SelectedValue != "" && CityId.SelectedValue != "-1")
                    {
                        Pcityid = CityId.SelectedValue;
                        Pcityname = CityId.SelectedItem.Text;
                    }
                    else if (CityId.SelectedValue == "-1")
                    {
                        if (CityName.Value != string.Empty)
                        {
                            SqlParameter[] prmcity = new SqlParameter[4];
                            prmcity[0] = new SqlParameter("@Stateid", StateId.SelectedValue);
                            prmcity[1] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"]);
                            prmcity[2] = new SqlParameter("ReturnValue", SqlDbType.Int);
                            prmcity[2].Direction = ParameterDirection.ReturnValue;
                            prmcity[3] = new SqlParameter("@cityName", CityName.Value);
                            Pcityid = da.executeDMLQuery("insertcity", prmcity).ToString();
                            Pcityname = CityName.Value;

                        }
                    }
                }

                //SqlParameter[] prm = new SqlParameter[36];
                //prm[0] = new SqlParameter("@id", editid);
                //prm[1] = new SqlParameter("@type", entrytype);
                //prm[2] = new SqlParameter("@leadid", isdouplicateid);
                //prm[3] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                //prm[4] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
                //prm[5] = new SqlParameter("@cdate", da1.getcdatetime());
                //prm[6] = new SqlParameter("@Salutation", (Salutation.SelectedValue == "" ? "0" : Salutation.SelectedValue));
                //prm[7] = new SqlParameter("@FirstName", fName.Text);
                //prm[8] = new SqlParameter("@LastName", lName.Text);
                //prm[9] = new SqlParameter("@Company", cmpname);// cName.Text);
                //prm[10] = new SqlParameter("@Title", title.Text);
                //prm[11] = new SqlParameter("@Website", website.Text);
                //prm[12] = new SqlParameter("@Descriptions", description.Value);
                //prm[13] = new SqlParameter("@LeadStatus", leadStatusid);
                //prm[14] = new SqlParameter("@OwnerName", ownerid);
                //prm[15] = new SqlParameter("@PhoneNo", PhoneNo.Text);
                //prm[16] = new SqlParameter("@EmailId", EmailId.Text);
                //prm[17] = new SqlParameter("@LAddress", AddressDetails.Value);
                //prm[18] = new SqlParameter("@Street", StreetName.Value);
                //prm[19] = new SqlParameter("@CityId", Pcityid);
                //prm[20] = new SqlParameter("@CityName", CityName.Value);
                //prm[21] = new SqlParameter("@PostalCode", PostalCode.Value);
                //prm[22] = new SqlParameter("@StateId", Pstateid);
                //prm[23] = new SqlParameter("@StateName", StateName.Value);
                //prm[24] = new SqlParameter("@CountryId", CountryId.SelectedValue);
                //prm[25] = new SqlParameter("@CountryName", "");
                //prm[26] = new SqlParameter("@NoOfEmployee", NoOfemp.Value);
                //prm[27] = new SqlParameter("@AnnualRevenue", AnnualRevenue.Value);
                //prm[28] = new SqlParameter("@LeadSourceid", LeadSourceid);
                //prm[29] = new SqlParameter("@IndustryTypeid", IndustryTypeid);
                //prm[30] = new SqlParameter("@Leadproduct", hdnddLeadProduct.Value);
                //prm[31] = new SqlParameter("@Satageupdatetime", da1.getcdatetime());
                //prm[32] = new SqlParameter("@LProbability", hdnleadProbability.Value);
                //prm[33] = new SqlParameter("@TeleGramBoatId", boatid.Text);
                //prm[34] = new SqlParameter("@leadnotes", txtlnotes.Value);
                //prm[35] = new SqlParameter("ReturnValue", SqlDbType.Int);
                //prm[35].Direction = ParameterDirection.ReturnValue;
                //int rid = da1.executeDMLQuery("crm_tbl_mainleadsD_Insert", prm);
                SqlParameter[] prm = new SqlParameter[35];
                prm[0] = new SqlParameter("@id", editid);
                prm[1] = new SqlParameter("@type", entrytype);
                prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
                prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
                prm[5] = new SqlParameter("@Salutation", (Salutation.SelectedValue == "" ? "0" : Salutation.SelectedValue));
                prm[6] = new SqlParameter("@FirstName", fName.Text);
                prm[7] = new SqlParameter("@LastName", lName.Text);
                prm[8] = new SqlParameter("@Company", cmpname);// cName.Text);
                prm[9] = new SqlParameter("@Title", title.Text);
                prm[10] = new SqlParameter("@Website", website.Text);
                prm[11] = new SqlParameter("@Descriptions", description.Value);
                prm[12] = new SqlParameter("@LeadStatus", leadStatusid);
                prm[13] = new SqlParameter("@OwnerName", ownerid);
                prm[14] = new SqlParameter("@PhoneNo", PhoneNo.Text);
                prm[15] = new SqlParameter("@EmailId", EmailId.Text);
                prm[16] = new SqlParameter("@LAddress", AddressDetails.Value);
                prm[17] = new SqlParameter("@Street", StreetName.Value);
                prm[18] = new SqlParameter("@CityId", Pcityid);
                prm[19] = new SqlParameter("@CityName", Pcityname);
                prm[20] = new SqlParameter("@PostalCode", PostalCode.Value);
                prm[21] = new SqlParameter("@StateId", Pstateid);
                prm[22] = new SqlParameter("@StateName", Pstatename);
                prm[23] = new SqlParameter("@CountryId", CountryId.SelectedValue);
                prm[24] = new SqlParameter("@CountryName", "");
                prm[25] = new SqlParameter("@NoOfEmployee", NoOfemp.Value);
                prm[26] = new SqlParameter("@AnnualRevenue", AnnualRevenue.Value);
                prm[27] = new SqlParameter("@LeadSourceid", LeadSourceid);
                prm[28] = new SqlParameter("@IndustryTypeid", IndustryTypeid);
                prm[29] = new SqlParameter("@Leadproduct", hdnddLeadProduct.Value);
                prm[30] = new SqlParameter("@Satageupdatetime", da1.getcdatetime());
                prm[31] = new SqlParameter("@LProbability", hdnleadProbability.Value);
                prm[32] = new SqlParameter("@TeleGramBoatId", boatid.Text);
                prm[33] = new SqlParameter("@leadnotes", txtlnotes.Value);
                prm[34] = new SqlParameter("ReturnValue", SqlDbType.Int);
                prm[34].Direction = ParameterDirection.ReturnValue;
                int rid = da1.executeDMLQuery("crm_tbl_mainleads_Insert_website", prm);
                Page.RegisterClientScriptBlock("", "<script>alert('Data Saved Successfully!!');window.parent.document.location.href =window.parent.document.location.href; </script>");
                //try
                //{
                //    resendnotificationlead(Convert.ToInt32(isdouplicateid), 20, 1, rid.ToString());
                //}
                //catch
                //{ }

            }
            else
            {
                if (LeadSource.SelectedValue == "0")
                {
                    LeadSourceid = "0";
                }
                else
                {
                    if (hdnnewsource.Value == "1" && txtnewsource.Value != "")
                    {
                        try
                        {
                            SqlParameter[] prm1 = new SqlParameter[4];
                            prm1[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                            prm1[1] = new SqlParameter("@name", txtnewsource.Value);
                            prm1[2] = new SqlParameter("@typevalue", "check");
                            prm1[3] = new SqlParameter("@type", "1");
                            DataSet dsc = da1.GetDataSet("crm_ups_tbl_crmmasterdata", prm1);
                            if (dsc.Tables[0].Rows.Count > 0)
                            {
                                LeadSourceid = dsc.Tables[0].Rows[0]["id"].ToString();
                            }
                            else
                            {
                                SqlParameter[] prm2 = new SqlParameter[5];
                                prm2[0] = new SqlParameter("@type", "1");
                                prm2[3] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                                prm2[1] = new SqlParameter("@name", txtnewsource.Value);
                                prm2[2] = new SqlParameter("@typevalue", "insert");
                                prm2[4] = new SqlParameter("ReturnValue", SqlDbType.Int);
                                prm2[4].Direction = ParameterDirection.ReturnValue;
                                int id = da1.executeDMLQuery("crm_ups_tbl_crmmasterdata_New", prm2);
                                LeadSourceid = id.ToString();

                            }
                        }
                        catch
                        { }
                    }
                    else
                    {
                        LeadSourceid = LeadSource.SelectedValue;
                    }
                }
                if (Salutation.SelectedValue == "" || Salutation.SelectedValue == "-1")
                {
                    Salutationid = "0";
                }
                else
                {
                    Salutationid = Salutation.SelectedValue;
                }
                if (Industry.SelectedValue == "0" || Industry.SelectedValue == "-1")
                {
                    IndustryTypeid = "0";
                    if (checkitemname())
                    {

                        SqlParameter[] prmnew = new SqlParameter[7];
                        prmnew[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"]);
                        prmnew[1] = new SqlParameter("@name", txtnewindustry.Text.Trim());
                        prmnew[2] = new SqlParameter("@code", "");
                        prmnew[3] = new SqlParameter("@id", DBNull.Value);
                        prmnew[4] = new SqlParameter("@type", "5");
                        prmnew[5] = new SqlParameter("@typevalue", "Insert");
                        prmnew[6] = new SqlParameter("ReturnValue", SqlDbType.Int);
                        prmnew[6].Direction = ParameterDirection.ReturnValue;
                        int retid = da.executeDMLQuery("crm_ups_tbl_crmmasterdata_DML", prmnew);
                        IndustryTypeid = retid.ToString();

                    }
                    else
                    {
                        string sqlchk = "select * from tbl_crmmasterdata where status=1 and archive=0 and regid='" + Request.Cookies["cadmin"]["id"] + "' and name='" + txtnewindustry.Text.Trim() + "' ";
                        DataSet dsc = da.GetDataSetInline(sqlchk);
                        if (dsc.Tables[0].Rows.Count > 0)
                        {
                            IndustryTypeid = dsc.Tables[0].Rows[0]["id"].ToString();
                        }
                    }
                }
                else
                {
                    IndustryTypeid = Industry.SelectedValue;
                }
                if (Owner.SelectedValue == "" || Owner.SelectedValue == "-1")
                {
                    ownerid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();
                }
                else
                {
                    ownerid = Owner.SelectedValue;
                }

                if (leadStatus.SelectedValue == "" || leadStatus.SelectedValue == "-1")
                {
                    string strleadstatus = "select id  from tbl_leadstatus where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and isnull(IsDefault,0)=1";
                    DataSet dschk = da1.GetDataSetInline(strleadstatus);
                    if (dschk.Tables[0].Rows.Count > 0)
                    {
                        leadStatusid = dschk.Tables[0].Rows[0]["id"].ToString();
                    }

                }
                else
                {
                    leadStatusid = leadStatus.SelectedValue;
                }
                string cmpname = cName.Text;
                if (cmpname == "")
                {
                    cmpname = fName.Text;
                    if (lName.Text.Trim() != "")
                    {
                        cmpname += " " + lName.Text;
                    }
                }
                string chkcountryid = getcontry_map();
                string Pcityid = "0";
                string Pstateid = "0";
                string Pcityname = CityName.Value;
                string Pstatename = StateName.Value;
                if (chkcountryid == "77")
                {
                    if (StateId.SelectedValue != "" && StateId.SelectedValue != "-1")
                    {
                        Pstateid = StateId.SelectedValue;
                        Pstatename = StateId.SelectedItem.Text;
                    }
                    if (CityId.SelectedValue != "" && CityId.SelectedValue != "-1")
                    {
                        Pcityid = CityId.SelectedValue;
                        Pcityname = CityId.SelectedItem.Text;
                    }
                    else if (CityId.SelectedValue == "-1")
                    {
                        if (CityName.Value != string.Empty)
                        {
                            SqlParameter[] prmcity = new SqlParameter[4];
                            prmcity[0] = new SqlParameter("@Stateid", StateId.SelectedValue);
                            prmcity[1] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"]);
                            prmcity[2] = new SqlParameter("ReturnValue", SqlDbType.Int);
                            prmcity[2].Direction = ParameterDirection.ReturnValue;
                            prmcity[3] = new SqlParameter("@cityName", CityName.Value);
                            Pcityid = da.executeDMLQuery("insertcity", prmcity).ToString();
                            Pcityname = CityName.Value;

                        }
                    }
                }

                SqlParameter[] prm = new SqlParameter[35];
                prm[0] = new SqlParameter("@id", editid);
                prm[1] = new SqlParameter("@type", entrytype);
                prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
                prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
                prm[5] = new SqlParameter("@Salutation", (Salutation.SelectedValue == "" ? "0" : Salutation.SelectedValue));
                prm[6] = new SqlParameter("@FirstName", fName.Text);
                prm[7] = new SqlParameter("@LastName", lName.Text);
                prm[8] = new SqlParameter("@Company", cmpname);// cName.Text);
                prm[9] = new SqlParameter("@Title", title.Text);
                prm[10] = new SqlParameter("@Website", website.Text);
                prm[11] = new SqlParameter("@Descriptions", description.Value);
                prm[12] = new SqlParameter("@LeadStatus", leadStatusid);
                prm[13] = new SqlParameter("@OwnerName", ownerid);
                prm[14] = new SqlParameter("@PhoneNo", PhoneNo.Text);
                prm[15] = new SqlParameter("@EmailId", EmailId.Text);
                prm[16] = new SqlParameter("@LAddress", AddressDetails.Value);
                prm[17] = new SqlParameter("@Street", StreetName.Value);
                prm[18] = new SqlParameter("@CityId", Pcityid);
                prm[19] = new SqlParameter("@CityName", Pcityname);
                prm[20] = new SqlParameter("@PostalCode", PostalCode.Value);
                prm[21] = new SqlParameter("@StateId", Pstateid);
                prm[22] = new SqlParameter("@StateName", Pstatename);
                prm[23] = new SqlParameter("@CountryId", CountryId.SelectedValue);
                prm[24] = new SqlParameter("@CountryName", "");
                prm[25] = new SqlParameter("@NoOfEmployee", NoOfemp.Value);
                prm[26] = new SqlParameter("@AnnualRevenue", AnnualRevenue.Value);
                prm[27] = new SqlParameter("@LeadSourceid", LeadSourceid);
                prm[28] = new SqlParameter("@IndustryTypeid", IndustryTypeid);
                prm[29] = new SqlParameter("@Leadproduct", hdnddLeadProduct.Value);
                prm[30] = new SqlParameter("@Satageupdatetime", da1.getcdatetime());
                prm[31] = new SqlParameter("@LProbability", hdnleadProbability.Value);
                prm[32] = new SqlParameter("@TeleGramBoatId", boatid.Text);
                prm[33] = new SqlParameter("@leadnotes", txtlnotes.Value);
                prm[34] = new SqlParameter("ReturnValue", SqlDbType.Int);
                prm[34].Direction = ParameterDirection.ReturnValue;
                int rid = da1.executeDMLQuery("crm_tbl_mainleads_Insert", prm);
                string companyid = "0";
                string contactid = "0";
                if (rid > 0)
                {
                    string strsqlchk = "";

                    strsqlchk = "select id,Company from tbl_LeadsCompanies where status='1' and archive='0' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and leadid='" + rid + "'";
                    //SaveLogText("strsqlchk1:"+ strsqlchk);
                    DataSet dschk = da1.GetDataSetInline(strsqlchk);
                    if (dschk.Tables[0].Rows.Count > 0)
                    {
                        //SaveLogText("strsqlchk2:ok");
                        companyid = dschk.Tables[0].Rows[0]["id"].ToString();
                        prm = new SqlParameter[20];
                        prm[0] = new SqlParameter("@id", companyid);
                        prm[1] = new SqlParameter("@entrytype", "Update");
                        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
                        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
                        prm[5] = new SqlParameter("@Company", cmpname);
                        prm[6] = new SqlParameter("@CompanyOwner", ownerid);
                        prm[7] = new SqlParameter("@Website", website.Text);
                        prm[8] = new SqlParameter("@PhoneNo", PhoneNo.Text);
                        prm[9] = new SqlParameter("@BillingAddress", AddressDetails.Value);
                        prm[10] = new SqlParameter("@Street", StreetName.Value);
                        prm[11] = new SqlParameter("@CityId", Pcityid);
                        prm[12] = new SqlParameter("@CityName", Pcityname);
                        prm[13] = new SqlParameter("@PostalCode", PostalCode.Value);
                        prm[14] = new SqlParameter("@StateId", Pstateid);
                        prm[15] = new SqlParameter("@StateName", Pstatename);
                        prm[16] = new SqlParameter("@CountryId", (CountryId.SelectedValue == "" ? "0" : CountryId.SelectedValue));
                        prm[17] = new SqlParameter("@CountryName", CountryId.SelectedItem.Text);
                        prm[18] = new SqlParameter("@leadid", rid);
                        prm[19] = new SqlParameter("@IndustryTypeid", IndustryTypeid);
                        da.executeDMLQuery("crm_tbl_LeadsCompanies_Insert", prm);
                    }
                    else
                    {
                        //strsqlchk = "select id,Company from tbl_LeadsCompanies where status='1' and archive='0' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and Company='" + cmpname + "' and PhoneNo='" + PhoneNo.Text + "'";
                        //dschk = da1.GetDataSetInline(strsqlchk);
                        SqlParameter[] prmc = new SqlParameter[3];
                        prmc[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                        prmc[1] = new SqlParameter("@company", cmpname);
                        prmc[2] = new SqlParameter("@phonno", PhoneNo.Text);
                        dschk = da.GetDataSet("getcompanyid", prmc);
                        if (dschk.Tables[0].Rows.Count > 0)
                        {
                            companyid = dschk.Tables[0].Rows[0]["id"].ToString();
                            prm = new SqlParameter[20];
                            prm[0] = new SqlParameter("@id", companyid);
                            prm[1] = new SqlParameter("@entrytype", "Update");
                            prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                            prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
                            prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
                            prm[5] = new SqlParameter("@Company", cmpname);
                            prm[6] = new SqlParameter("@CompanyOwner", ownerid);
                            prm[7] = new SqlParameter("@Website", website.Text);
                            prm[8] = new SqlParameter("@PhoneNo", PhoneNo.Text);
                            prm[9] = new SqlParameter("@BillingAddress", AddressDetails.Value);
                            prm[10] = new SqlParameter("@Street", StreetName.Value);
                            prm[11] = new SqlParameter("@CityId", Pcityid);
                            prm[12] = new SqlParameter("@CityName", Pcityname);
                            prm[13] = new SqlParameter("@PostalCode", PostalCode.Value);
                            prm[14] = new SqlParameter("@StateId", Pstateid);
                            prm[15] = new SqlParameter("@StateName", Pstatename);
                            prm[16] = new SqlParameter("@CountryId", (CountryId.SelectedValue == "" ? "0" : CountryId.SelectedValue));
                            prm[17] = new SqlParameter("@CountryName", CountryId.SelectedItem.Text);
                            prm[18] = new SqlParameter("@leadid", rid);
                            prm[19] = new SqlParameter("@IndustryTypeid", IndustryTypeid);
                            da.executeDMLQuery("crm_tbl_LeadsCompanies_Insert", prm);

                        }
                        else
                        {
                            try
                            {
                                //SaveLogText("strsqlchk4:4");
                                prm = new SqlParameter[31];
                                prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                                prm[1] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
                                prm[2] = new SqlParameter("@cdate", da1.getcdatetime());
                                prm[3] = new SqlParameter("@Company", cmpname);
                                prm[4] = new SqlParameter("@CompanyOwner", ownerid);
                                prm[5] = new SqlParameter("@Website", website.Text);
                                prm[6] = new SqlParameter("@Descriptions", DBNull.Value);
                                prm[7] = new SqlParameter("@PhoneNo", PhoneNo.Text);
                                prm[8] = new SqlParameter("@BillingAddress", AddressDetails.Value);
                                prm[9] = new SqlParameter("@Street", StreetName.Value);
                                prm[10] = new SqlParameter("@CityId", Pcityid);
                                prm[11] = new SqlParameter("@CityName", Pcityname);
                                prm[12] = new SqlParameter("@PostalCode", PostalCode.Value);
                                prm[13] = new SqlParameter("@StateId", Pstateid);
                                prm[14] = new SqlParameter("@StateName", Pstatename);
                                prm[15] = new SqlParameter("@CountryId", (CountryId.SelectedValue == "" ? "0" : CountryId.SelectedValue));
                                prm[16] = new SqlParameter("@CountryName", CountryId.SelectedItem.Text);
                                prm[17] = new SqlParameter("@ShippingAddress", AddressDetails.Value);
                                prm[18] = new SqlParameter("@ShippingStreet", StreetName.Value);
                                prm[19] = new SqlParameter("@ShippingCityId", DBNull.Value);
                                prm[20] = new SqlParameter("@ShippingCityName", Pcityname);
                                prm[21] = new SqlParameter("@ShippingPostalCode", PostalCode.Value);
                                prm[22] = new SqlParameter("@ShippingStateId", DBNull.Value);
                                prm[23] = new SqlParameter("@ShippingStateName", Pstatename);
                                prm[24] = new SqlParameter("@ShippingCountryId", (CountryId.SelectedValue == "" ? "0" : CountryId.SelectedValue));

                                prm[25] = new SqlParameter("@ShippingCountryName", CountryId.SelectedItem.Text);
                                prm[26] = new SqlParameter("@leadid", rid);
                                prm[27] = new SqlParameter("@IndustryTypeid", IndustryTypeid);
                                prm[28] = new SqlParameter("@PhoneNoOther", PhoneNo.Text);

                                prm[29] = new SqlParameter("@NoOfEmployee", DBNull.Value);

                                prm[30] = new SqlParameter("ReturnValue", SqlDbType.Int);
                                prm[30].Direction = ParameterDirection.ReturnValue;

                                companyid = Convert.ToString(da.executeDMLQuery("crm_tbl_LeadsCompanies_ConvertedInsert", prm));
                                SaveLogText("companyid: " + companyid);
                            }
                            catch
                            { }
                        }

                    }
                    strsqlchk = "select id from tbl_LeadsCompanyContact where status='1' and archive='0' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and leadid='" + rid + "'";
                    dschk = da1.GetDataSetInline(strsqlchk);
                    if (dschk.Tables[0].Rows.Count > 0)
                    {
                        contactid = dschk.Tables[0].Rows[0]["id"].ToString();
                        prm = new SqlParameter[29];
                        prm[0] = new SqlParameter("@id", contactid);
                        prm[1] = new SqlParameter("@entrytype", "Update");
                        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
                        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
                        prm[5] = new SqlParameter("@Salutation", Salutation.SelectedValue);
                        prm[6] = new SqlParameter("@FirstName", fName.Text);
                        prm[7] = new SqlParameter("@LastName", lName.Text);
                        prm[8] = new SqlParameter("@Companyid", companyid);
                        prm[9] = new SqlParameter("@Suffix", "");
                        prm[10] = new SqlParameter("@ContactOwner", ownerid);
                        prm[11] = new SqlParameter("@Designation", title.Text);
                        prm[12] = new SqlParameter("@PhoneNo", PhoneNo.Text);
                        prm[13] = new SqlParameter("@Emailid", EmailId.Text);
                        prm[14] = new SqlParameter("@MobNo", PhoneNo.Text);
                        prm[15] = new SqlParameter("@FaxbNo", "");
                        prm[16] = new SqlParameter("@ReportTo", "");
                        prm[17] = new SqlParameter("@DepartmentName", "");
                        prm[18] = new SqlParameter("@MailingAddress", AddressDetails.Value);
                        prm[19] = new SqlParameter("@MailingStreet", StreetName.Value);
                        prm[20] = new SqlParameter("@MailingCityId", DBNull.Value);
                        prm[21] = new SqlParameter("@MailingCityName", Pcityname);
                        prm[22] = new SqlParameter("@MailingPostalCode", PostalCode.Value);
                        prm[23] = new SqlParameter("@MailingStateId", DBNull.Value);
                        prm[24] = new SqlParameter("@MailingStateName", Pstatename);
                        prm[25] = new SqlParameter("@MailingCountryId", (CountryId.SelectedValue == "" ? "0" : CountryId.SelectedValue));
                        prm[26] = new SqlParameter("@MailingCountryName", CountryId.SelectedItem.Text);
                        prm[27] = new SqlParameter("@leadid", rid);
                        prm[28] = new SqlParameter("@Descriptions", DBNull.Value);
                        da.executeDMLQuery("crm_tbl_LeadsCompanyContact_Insert", prm);
                    }
                    else
                    {
                        strsqlchk = "select id from tbl_LeadsCompanyContact where status='1' and archive='0' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and Salutation='" + Salutation.SelectedValue + "' and FirstName='" + fName.Text + "' and LastName='" + lName.Text + "'";
                        dschk = da1.GetDataSetInline(strsqlchk);
                        if (dschk.Tables[0].Rows.Count > 0)
                        {
                            contactid = dschk.Tables[0].Rows[0]["id"].ToString();
                            prm = new SqlParameter[29];
                            prm[0] = new SqlParameter("@id", contactid);
                            prm[1] = new SqlParameter("@entrytype", "Update");
                            prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                            prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
                            prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
                            prm[5] = new SqlParameter("@Salutation", Salutation.SelectedValue);
                            prm[6] = new SqlParameter("@FirstName", fName.Text);
                            prm[7] = new SqlParameter("@LastName", lName.Text);
                            prm[8] = new SqlParameter("@Companyid", companyid);
                            prm[9] = new SqlParameter("@Suffix", "");
                            prm[10] = new SqlParameter("@ContactOwner", ownerid);
                            prm[11] = new SqlParameter("@Designation", title.Text);
                            prm[12] = new SqlParameter("@PhoneNo", PhoneNo.Text);
                            prm[13] = new SqlParameter("@Emailid", EmailId.Text);
                            prm[14] = new SqlParameter("@MobNo", PhoneNo.Text);
                            prm[15] = new SqlParameter("@FaxbNo", "");
                            prm[16] = new SqlParameter("@ReportTo", "");
                            prm[17] = new SqlParameter("@DepartmentName", "");
                            prm[18] = new SqlParameter("@MailingAddress", AddressDetails.Value);
                            prm[19] = new SqlParameter("@MailingStreet", StreetName.Value);
                            prm[20] = new SqlParameter("@MailingCityId", DBNull.Value);
                            prm[21] = new SqlParameter("@MailingCityName", Pcityname);
                            prm[22] = new SqlParameter("@MailingPostalCode", PostalCode.Value);
                            prm[23] = new SqlParameter("@MailingStateId", DBNull.Value);
                            prm[24] = new SqlParameter("@MailingStateName", Pstatename);
                            prm[25] = new SqlParameter("@MailingCountryId", (CountryId.SelectedValue == "" ? "0" : CountryId.SelectedValue));
                            prm[26] = new SqlParameter("@MailingCountryName", CountryId.SelectedItem.Text);
                            prm[27] = new SqlParameter("@leadid", rid);
                            prm[28] = new SqlParameter("@Descriptions", DBNull.Value);
                            da.executeDMLQuery("crm_tbl_LeadsCompanyContact_Insert", prm);
                        }
                        else
                        {
                            try
                            {
                                prm = new SqlParameter[29];
                                prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                                prm[1] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
                                prm[2] = new SqlParameter("@cdate", da1.getcdatetime());
                                prm[3] = new SqlParameter("@Salutation", (Salutation.SelectedValue == "" ? "0" : Salutation.SelectedValue));
                                prm[4] = new SqlParameter("@FirstName", fName.Text);
                                prm[5] = new SqlParameter("@MiddleName", "");
                                prm[6] = new SqlParameter("@LastName", lName.Text);
                                prm[7] = new SqlParameter("@Companyid", companyid);
                                prm[8] = new SqlParameter("@Suffix", "");
                                prm[9] = new SqlParameter("@ContactOwner", ownerid);
                                prm[10] = new SqlParameter("@Designation", title.Text);
                                prm[11] = new SqlParameter("@PhoneNo", PhoneNo.Text);
                                prm[12] = new SqlParameter("@Emailid", EmailId.Text);
                                prm[13] = new SqlParameter("@MobNo", PhoneNo.Text);
                                prm[14] = new SqlParameter("@FaxbNo", "");
                                prm[15] = new SqlParameter("@ReportTo", "");
                                prm[16] = new SqlParameter("@DepartmentName", "");
                                prm[17] = new SqlParameter("@MailingAddress", AddressDetails.Value);
                                prm[18] = new SqlParameter("@MailingStreet", StreetName.Value);
                                prm[19] = new SqlParameter("@MailingCityId", DBNull.Value);
                                prm[20] = new SqlParameter("@MailingCityName", Pcityname);
                                prm[21] = new SqlParameter("@MailingPostalCode", PostalCode.Value);
                                prm[22] = new SqlParameter("@MailingStateId", DBNull.Value);
                                prm[23] = new SqlParameter("@MailingStateName", Pstatename);
                                prm[24] = new SqlParameter("@MailingCountryId", (CountryId.SelectedValue == "" ? "0" : CountryId.SelectedValue));
                                prm[25] = new SqlParameter("@MailingCountryName", CountryId.SelectedItem.Text);
                                prm[26] = new SqlParameter("@leadid", rid);
                                prm[27] = new SqlParameter("@Descriptions", DBNull.Value);
                                prm[28] = new SqlParameter("ReturnValue", SqlDbType.Int);
                                prm[28].Direction = ParameterDirection.ReturnValue;
                                contactid = Convert.ToString(da.executeDMLQuery("crm_tbl_LeadsCompanyContact_ConvertedInsert", prm));
                            }
                            catch
                            { }
                        }
                    }


                    try
                    {
                        if (entrytype == "Insert")
                        {
                            try
                            {
                                //setnotification(rid.ToString(), "20", "1");
                                sendnotificationlead(rid, 20, 1);
                                //send welcome message to client
                                sendnotificationleadtoclient(rid, 27, 27);
                            }
                            catch
                            { }
                        }
                        else if (entrytype == "Update")
                        {
                            try
                            {
                                //setnotification(rid.ToString(), "20", "1");
                                sendnotificationlead(rid, 20, 2);
                            }
                            catch
                            { }
                        }


                    }
                    catch
                    { }

                    string strquery = "";
                    string leadtype = "";

                    if (Request.QueryString["leadtype"] != null)
                    {
                        leadtype = Request.QueryString["leadtype"];
                        if (strquery == "")
                        {
                            strquery += "?leadtype=" + leadtype;
                        }
                        else
                        {
                            strquery += "&leadtype=" + leadtype;
                        }
                    }
                    string presearch = "";
                    if (Request.QueryString["presearch"] != null)
                    {
                        presearch = Request.QueryString["presearch"];
                        if (strquery == "")
                        {
                            strquery += "?presearch=" + presearch;
                        }
                        else
                        {
                            strquery += "&presearch=" + presearch;
                        }
                    }

                    if (Request.QueryString["callfrom"] != null)
                    {
                        if (Request.QueryString["callfrom"].ToString() == "leaddetails")
                        {
                            string strid = "";
                            if (Request.QueryString["id"] != null)
                            {

                                if (strquery == "")
                                {
                                    strquery += "?id=" + Cmf.Encrypt(Request.QueryString["id"].ToString());
                                }
                                else
                                {
                                    strquery += "&id=" + Cmf.Encrypt(Request.QueryString["id"].ToString());
                                }
                                // strquery = strid + strquery;
                            }
                            Page.RegisterClientScriptBlock("", "<script>alert('Data Saved Successfully!!');window.parent.window.parent.document.location.href ='/crm/lead-details.aspx" + strquery + "'; </script>");
                        }
                        else if (Request.QueryString["callfrom"].ToString() == "lead")
                        {
                            Page.RegisterClientScriptBlock("", "<script>alert('Data Saved Successfully!!');window.parent.window.parent.document.location.href ='/crm/lead.aspx" + strquery + "'; </script>");
                        }
                        //else if (Request.QueryString["callfrom"].ToString() == "leaddetailsp")
                        //{
                        //    Page.RegisterClientScriptBlock("", "<script>alert('Data Saved Successfully!!');window.parent.document.location.href ='/crm/lead-detailsp.aspx" + strquery + "'; </script>");
                        //}
                    }
                    else
                    {
                        Page.RegisterClientScriptBlock("", "<script>alert('Data Saved Successfully!!');window.parent.window.parent.document.location.href ='/crm/lead.aspx" + strquery + "'; </script>");
                        //Page.RegisterClientScriptBlock("", "<script>alert('Data Saved Successfully!!');window.parent.window.parent.document.location.href ='/crm/kanban_lead.aspx" + strquery + "'; </script>");
                    }


                    if (entrytype == "Insert")
                    {
                        entrytype = "1";
                    }
                    else if (entrytype == "Update")
                    {
                        entrytype = "2";
                    }

                }
            }


        }

        //}
        //    catch(Exception ex)
        //        {
        //       // Response.Write(ex.Message);
        //        //Response.End();
        //        }


    }
    protected void btnsavemore_Click(object sender, EventArgs e)
    {
        //try
        //{
        DataAccess da1 = new DataAccess();
        string editid = hdnleadeditid.Value;
        string entrytype = "Insert";
        string LeadSourceid = "0";
        string IndustryTypeid = "0";
        string ownerid = "0";
        string leadStatusid = "0";
        string Salutationid = "0";

        if (CountryId.SelectedValue == "77")
        {
            if (Industry.SelectedValue == "" || Industry.SelectedValue == "0" || Industry.SelectedValue == "-1")
            {

                if (Industry.SelectedValue == "-1")
                {
                    if (txtnewindustry.Text.Trim() == "")
                    {
                        if (CityId.SelectedValue == "-1")
                        {
                            pnlcity.Visible = true;
                            strdvcity = "";
                        }
                        if (CityId.SelectedValue == "-1")
                        {
                            pnlcity.Visible = true;
                            strdvcity = "";
                        }
                        Page.RegisterClientScriptBlock("", "<script>alert('Please select industry!'); </script>");
                        return;
                    }
                }
                else
                {
                    if (CityId.SelectedValue == "-1")
                    {
                        pnlcity.Visible = true;
                        strdvcity = "";
                    }
                    if (CityId.SelectedValue == "-1")
                    {
                        pnlcity.Visible = true;
                        strdvcity = "";
                    }
                    Page.RegisterClientScriptBlock("", "<script>alert('Please select industry!'); </script>");
                    return;
                }
            }
            if (StateId.SelectedValue == "" || StateId.SelectedValue == "0" || StateId.SelectedValue == "-1")
            {
                Page.RegisterClientScriptBlock("", "<script>alert('Please select state!'); </script>");
                return;
            }
            if (CityId.SelectedValue == "" || CityId.SelectedValue == "0")
            {
                Page.RegisterClientScriptBlock("", "<script>alert('Please select city!'); </script>");
                return;
            }
        }

        if (editid != "" && editid != "0")
        {
            entrytype = "Update";
            if (LeadSource.SelectedValue == "0")
            {
                LeadSourceid = "0";
            }
            else
            {
                if (hdnnewsource.Value == "1" && txtnewsource.Value != "")
                {
                    try
                    {
                        SqlParameter[] prm1 = new SqlParameter[4];
                        prm1[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                        prm1[1] = new SqlParameter("@name", txtnewsource.Value);
                        prm1[2] = new SqlParameter("@typevalue", "check");
                        prm1[3] = new SqlParameter("@type", "1");
                        DataSet dsc = da1.GetDataSet("crm_ups_tbl_crmmasterdata", prm1);
                        if (dsc.Tables[0].Rows.Count > 0)
                        {
                            LeadSourceid = dsc.Tables[0].Rows[0]["id"].ToString();
                        }
                        else
                        {
                            SqlParameter[] prm2 = new SqlParameter[5];
                            prm2[0] = new SqlParameter("@type", "1");
                            prm2[3] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                            prm2[1] = new SqlParameter("@name", txtnewsource.Value);
                            prm2[2] = new SqlParameter("@typevalue", "insert");
                            prm2[4] = new SqlParameter("ReturnValue", SqlDbType.Int);
                            prm2[4].Direction = ParameterDirection.ReturnValue;
                            int id = da1.executeDMLQuery("crm_ups_tbl_crmmasterdata_New", prm2);
                            LeadSourceid = id.ToString();

                        }
                    }
                    catch
                    { }
                }
                else
                {
                    LeadSourceid = LeadSource.SelectedValue;
                }
            }
            if (Salutation.SelectedValue == "" || Salutation.SelectedValue == "-1")
            {
                Salutationid = "0";
            }
            else
            {
                Salutationid = Salutation.SelectedValue;
            }
            if (Industry.SelectedValue == "0" || Industry.SelectedValue == "-1")
            {
                IndustryTypeid = "0";
            }
            else
            {
                IndustryTypeid = Industry.SelectedValue;
            }
            if (Owner.SelectedValue == "" || Owner.SelectedValue == "-1")
            {
                ownerid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();
            }
            else
            {
                ownerid = Owner.SelectedValue;
            }
            if (leadStatus.SelectedValue == "" || leadStatus.SelectedValue == "-1")
            {
                string strleadstatus = "select id  from tbl_leadstatus where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and isnull(IsDefault,0)=1";
                DataSet dschk = da1.GetDataSetInline(strleadstatus);
                if (dschk.Tables[0].Rows.Count > 0)
                {
                    leadStatusid = dschk.Tables[0].Rows[0]["id"].ToString();
                }

            }
            else
            {
                leadStatusid = leadStatus.SelectedValue;
            }
            string chkcountryid = getcontry_map();
            string Pcityid = "0";
            string Pstateid = "0";
            string Pcityname = CityName.Value;
            string Pstatename = StateName.Value;
            if (chkcountryid == "77")
            {
                if (StateId.SelectedValue != "" && StateId.SelectedValue != "-1")
                {
                    Pstateid = StateId.SelectedValue;
                    Pstatename = StateId.SelectedItem.Text;
                }
                if (CityId.SelectedValue != "" && CityId.SelectedValue != "-1")
                {
                    Pcityid = CityId.SelectedValue;
                    Pcityname = CityId.SelectedItem.Text;
                }
                else if (CityId.SelectedValue == "-1")
                {
                    if (CityName.Value != string.Empty)
                    {
                        SqlParameter[] prmcity = new SqlParameter[4];
                        prmcity[0] = new SqlParameter("@Stateid", StateId.SelectedValue);
                        prmcity[1] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"]);
                        prmcity[2] = new SqlParameter("ReturnValue", SqlDbType.Int);
                        prmcity[2].Direction = ParameterDirection.ReturnValue;
                        prmcity[3] = new SqlParameter("@cityName", CityName.Value);
                        Pcityid = da.executeDMLQuery("insertcity", prmcity).ToString();
                        Pcityname = CityName.Value;

                    }
                }
            }
            SqlParameter[] prm = new SqlParameter[35];
            prm[0] = new SqlParameter("@id", editid);
            prm[1] = new SqlParameter("@type", entrytype);
            prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
            prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
            prm[5] = new SqlParameter("@Salutation", (Salutation.SelectedValue == "" ? "0" : Salutation.SelectedValue));
            prm[6] = new SqlParameter("@FirstName", fName.Text);
            prm[7] = new SqlParameter("@LastName", lName.Text);
            prm[8] = new SqlParameter("@Company", cName.Text);
            prm[9] = new SqlParameter("@Title", title.Text);
            prm[10] = new SqlParameter("@Website", website.Text);
            prm[11] = new SqlParameter("@Descriptions", description.Value);
            prm[12] = new SqlParameter("@LeadStatus", leadStatusid);
            prm[13] = new SqlParameter("@OwnerName", ownerid);
            prm[14] = new SqlParameter("@PhoneNo", PhoneNo.Text);
            prm[15] = new SqlParameter("@EmailId", EmailId.Text);
            prm[16] = new SqlParameter("@LAddress", AddressDetails.Value);
            prm[17] = new SqlParameter("@Street", StreetName.Value);
            prm[18] = new SqlParameter("@CityId", Pcityid);
            prm[19] = new SqlParameter("@CityName", Pcityname);
            prm[20] = new SqlParameter("@PostalCode", PostalCode.Value);
            prm[21] = new SqlParameter("@StateId", Pstateid);
            prm[22] = new SqlParameter("@StateName", Pstatename);
            prm[23] = new SqlParameter("@CountryId", CountryId.SelectedValue);
            prm[24] = new SqlParameter("@CountryName", "");
            prm[25] = new SqlParameter("@NoOfEmployee", NoOfemp.Value);
            prm[26] = new SqlParameter("@AnnualRevenue", AnnualRevenue.Value);
            prm[27] = new SqlParameter("@LeadSourceid", LeadSourceid);
            prm[28] = new SqlParameter("@IndustryTypeid", IndustryTypeid);
            prm[29] = new SqlParameter("@Leadproduct", DBNull.Value);
            prm[30] = new SqlParameter("@Satageupdatetime", da1.getcdatetime());
            prm[31] = new SqlParameter("@LProbability", hdnleadProbability.Value);
            prm[32] = new SqlParameter("@TeleGramBoatId", boatid.Text);
            prm[33] = new SqlParameter("@leadnotes", txtlnotes.Value);
            prm[34] = new SqlParameter("ReturnValue", SqlDbType.Int);
            prm[34].Direction = ParameterDirection.ReturnValue;
            int rid = da1.executeDMLQuery("crm_tbl_mainleads_Insert", prm);
            string companyid = "0";
            string contactid = "0";
            if (rid > 0)
            {
                string cmpname = cName.Text;
                if (cmpname == "")
                {
                    cmpname = fName.Text;
                    if (lName.Text.Trim() != "")
                    {
                        cmpname += " " + lName.Text;
                    }
                }
                string strsqlchk = "";
                strsqlchk = "select id,Company from tbl_LeadsCompanies where status='1' and archive='0' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and leadid='" + rid + "'";
                DataSet dschk = da1.GetDataSetInline(strsqlchk);
                if (dschk.Tables[0].Rows.Count > 0)
                {
                    companyid = dschk.Tables[0].Rows[0]["id"].ToString();
                    prm = new SqlParameter[20];
                    prm[0] = new SqlParameter("@id", companyid);
                    prm[1] = new SqlParameter("@entrytype", "Update");
                    prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                    prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
                    prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
                    prm[5] = new SqlParameter("@Company", cName.Text);
                    prm[6] = new SqlParameter("@CompanyOwner", ownerid);
                    prm[7] = new SqlParameter("@Website", website.Text);
                    prm[8] = new SqlParameter("@PhoneNo", PhoneNo.Text);
                    prm[9] = new SqlParameter("@BillingAddress", AddressDetails.Value);
                    prm[10] = new SqlParameter("@Street", StreetName.Value);
                    prm[11] = new SqlParameter("@CityId", Pcityid);
                    prm[12] = new SqlParameter("@CityName", Pcityname);
                    prm[13] = new SqlParameter("@PostalCode", PostalCode.Value);
                    prm[14] = new SqlParameter("@StateId", Pstateid);
                    prm[15] = new SqlParameter("@StateName", Pstatename);
                    prm[16] = new SqlParameter("@CountryId", (CountryId.SelectedValue == "" ? "0" : CountryId.SelectedValue));
                    prm[17] = new SqlParameter("@CountryName", CountryId.SelectedItem.Text);
                    prm[18] = new SqlParameter("@leadid", rid);
                    prm[19] = new SqlParameter("@IndustryTypeid", IndustryTypeid);
                    da.executeDMLQuery("crm_tbl_LeadsCompanies_Insert", prm);
                }
                else
                {
                    SqlParameter[] prmc = new SqlParameter[3];
                    prmc[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                    prmc[1] = new SqlParameter("@company", cmpname);
                    prmc[2] = new SqlParameter("@phonno", PhoneNo.Text);
                    dschk = da.GetDataSet("getcompanyid", prmc);
                    if (dschk.Tables[0].Rows.Count > 0)
                    {
                        companyid = dschk.Tables[0].Rows[0]["id"].ToString();
                        prm = new SqlParameter[20];
                        prm[0] = new SqlParameter("@id", companyid);
                        prm[1] = new SqlParameter("@entrytype", "Update");
                        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
                        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
                        prm[5] = new SqlParameter("@Company", cmpname);
                        prm[6] = new SqlParameter("@CompanyOwner", ownerid);
                        prm[7] = new SqlParameter("@Website", website.Text);
                        prm[8] = new SqlParameter("@PhoneNo", PhoneNo.Text);
                        prm[9] = new SqlParameter("@BillingAddress", AddressDetails.Value);
                        prm[10] = new SqlParameter("@Street", StreetName.Value);
                        prm[11] = new SqlParameter("@CityId", Pcityid);
                        prm[12] = new SqlParameter("@CityName", Pcityname);
                        prm[13] = new SqlParameter("@PostalCode", PostalCode.Value);
                        prm[14] = new SqlParameter("@StateId", Pstateid);
                        prm[15] = new SqlParameter("@StateName", Pstatename);
                        prm[16] = new SqlParameter("@CountryId", (CountryId.SelectedValue == "" ? "0" : CountryId.SelectedValue));
                        prm[17] = new SqlParameter("@CountryName", CountryId.SelectedItem.Text);
                        prm[18] = new SqlParameter("@leadid", rid);
                        prm[19] = new SqlParameter("@IndustryTypeid", IndustryTypeid);
                        da.executeDMLQuery("crm_tbl_LeadsCompanies_Insert", prm);

                    }
                    else
                    {
                        try
                        {
                            prm = new SqlParameter[31];
                            prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                            prm[1] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
                            prm[2] = new SqlParameter("@cdate", da1.getcdatetime());
                            prm[3] = new SqlParameter("@Company", cmpname);
                            prm[4] = new SqlParameter("@CompanyOwner", ownerid);
                            prm[5] = new SqlParameter("@Website", website.Text);
                            prm[6] = new SqlParameter("@Descriptions", DBNull.Value);
                            prm[7] = new SqlParameter("@PhoneNo", PhoneNo.Text);
                            prm[8] = new SqlParameter("@BillingAddress", AddressDetails.Value);
                            prm[9] = new SqlParameter("@Street", StreetName.Value);
                            prm[10] = new SqlParameter("@CityId", Pcityid);
                            prm[11] = new SqlParameter("@CityName", Pcityname);
                            prm[12] = new SqlParameter("@PostalCode", PostalCode.Value);
                            prm[13] = new SqlParameter("@StateId", Pstateid);
                            prm[14] = new SqlParameter("@StateName", Pstatename);
                            prm[15] = new SqlParameter("@CountryId", (CountryId.SelectedValue == "" ? "0" : CountryId.SelectedValue));
                            prm[16] = new SqlParameter("@CountryName", CountryId.SelectedItem.Text);
                            prm[17] = new SqlParameter("@ShippingAddress", AddressDetails.Value);
                            prm[18] = new SqlParameter("@ShippingStreet", StreetName.Value);
                            prm[19] = new SqlParameter("@ShippingCityId", Pcityid);
                            prm[20] = new SqlParameter("@ShippingCityName", Pcityname);
                            prm[21] = new SqlParameter("@ShippingPostalCode", PostalCode.Value);
                            prm[22] = new SqlParameter("@ShippingStateId", Pstateid);
                            prm[23] = new SqlParameter("@ShippingStateName", Pstatename);
                            prm[24] = new SqlParameter("@ShippingCountryId", (CountryId.SelectedValue == "" ? "0" : CountryId.SelectedValue));

                            prm[25] = new SqlParameter("@ShippingCountryName", CountryId.SelectedItem.Text);
                            prm[26] = new SqlParameter("@leadid", rid);
                            prm[27] = new SqlParameter("@IndustryTypeid", IndustryTypeid);
                            prm[28] = new SqlParameter("@PhoneNoOther", PhoneNo.Text);

                            prm[29] = new SqlParameter("@NoOfEmployee", DBNull.Value);

                            prm[30] = new SqlParameter("ReturnValue", SqlDbType.Int);
                            prm[30].Direction = ParameterDirection.ReturnValue;

                            companyid = Convert.ToString(da.executeDMLQuery("crm_tbl_LeadsCompanies_ConvertedInsert", prm));
                            //SaveLogText("companyid: "+ companyid);
                        }
                        catch
                        { }
                    }

                }
                strsqlchk = "select id from tbl_LeadsCompanyContact where status='1' and archive='0' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and leadid='" + rid + "'";
                dschk = da1.GetDataSetInline(strsqlchk);
                if (dschk.Tables[0].Rows.Count > 0)
                {
                    contactid = dschk.Tables[0].Rows[0]["id"].ToString();
                    prm = new SqlParameter[29];
                    prm[0] = new SqlParameter("@id", contactid);
                    prm[1] = new SqlParameter("@entrytype", "Update");
                    prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                    prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
                    prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
                    prm[5] = new SqlParameter("@Salutation", Salutation.SelectedValue);
                    prm[6] = new SqlParameter("@FirstName", fName.Text);
                    prm[7] = new SqlParameter("@LastName", lName.Text);
                    prm[8] = new SqlParameter("@Companyid", companyid);
                    prm[9] = new SqlParameter("@Suffix", "");
                    prm[10] = new SqlParameter("@ContactOwner", ownerid);
                    prm[11] = new SqlParameter("@Designation", title.Text);
                    prm[12] = new SqlParameter("@PhoneNo", PhoneNo.Text);
                    prm[13] = new SqlParameter("@Emailid", EmailId.Text);
                    prm[14] = new SqlParameter("@MobNo", PhoneNo.Text);
                    prm[15] = new SqlParameter("@FaxbNo", "");
                    prm[16] = new SqlParameter("@ReportTo", "");
                    prm[17] = new SqlParameter("@DepartmentName", "");
                    prm[18] = new SqlParameter("@MailingAddress", AddressDetails.Value);
                    prm[19] = new SqlParameter("@MailingStreet", StreetName.Value);
                    prm[20] = new SqlParameter("@MailingCityId", Pcityid);
                    prm[21] = new SqlParameter("@MailingCityName", Pcityname);
                    prm[22] = new SqlParameter("@MailingPostalCode", PostalCode.Value);
                    prm[23] = new SqlParameter("@MailingStateId", Pstateid);
                    prm[24] = new SqlParameter("@MailingStateName", Pstatename);
                    prm[25] = new SqlParameter("@MailingCountryId", (CountryId.SelectedValue == "" ? "0" : CountryId.SelectedValue));
                    prm[26] = new SqlParameter("@MailingCountryName", CountryId.SelectedItem.Text);
                    prm[27] = new SqlParameter("@leadid", rid);
                    prm[28] = new SqlParameter("@Descriptions", DBNull.Value);
                    da.executeDMLQuery("crm_tbl_LeadsCompanyContact_Insert", prm);
                }
                else
                {
                    strsqlchk = "select id from tbl_LeadsCompanyContact where status='1' and archive='0' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and Salutation='" + Salutation.SelectedValue + "' and FirstName='" + fName.Text + "' and LastName='" + lName.Text + "'";
                    dschk = da1.GetDataSetInline(strsqlchk);
                    if (dschk.Tables[0].Rows.Count > 0)
                    {
                        contactid = dschk.Tables[0].Rows[0]["id"].ToString();
                        prm = new SqlParameter[29];
                        prm[0] = new SqlParameter("@id", contactid);
                        prm[1] = new SqlParameter("@entrytype", "Update");
                        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
                        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
                        prm[5] = new SqlParameter("@Salutation", Salutation.SelectedValue);
                        prm[6] = new SqlParameter("@FirstName", fName.Text);
                        prm[7] = new SqlParameter("@LastName", lName.Text);
                        prm[8] = new SqlParameter("@Companyid", companyid);
                        prm[9] = new SqlParameter("@Suffix", "");
                        prm[10] = new SqlParameter("@ContactOwner", ownerid);
                        prm[11] = new SqlParameter("@Designation", title.Text);
                        prm[12] = new SqlParameter("@PhoneNo", PhoneNo.Text);
                        prm[13] = new SqlParameter("@Emailid", EmailId.Text);
                        prm[14] = new SqlParameter("@MobNo", PhoneNo.Text);
                        prm[15] = new SqlParameter("@FaxbNo", "");
                        prm[16] = new SqlParameter("@ReportTo", "");
                        prm[17] = new SqlParameter("@DepartmentName", "");
                        prm[18] = new SqlParameter("@MailingAddress", AddressDetails.Value);
                        prm[19] = new SqlParameter("@MailingStreet", StreetName.Value);
                        prm[20] = new SqlParameter("@MailingCityId", Pcityid);
                        prm[21] = new SqlParameter("@MailingCityName", Pcityname);
                        prm[22] = new SqlParameter("@MailingPostalCode", PostalCode.Value);
                        prm[23] = new SqlParameter("@MailingStateId", Pstateid);
                        prm[24] = new SqlParameter("@MailingStateName", Pstatename);
                        prm[25] = new SqlParameter("@MailingCountryId", (CountryId.SelectedValue == "" ? "0" : CountryId.SelectedValue));
                        prm[26] = new SqlParameter("@MailingCountryName", CountryId.SelectedItem.Text);
                        prm[27] = new SqlParameter("@leadid", rid);
                        prm[28] = new SqlParameter("@Descriptions", DBNull.Value);
                        da.executeDMLQuery("crm_tbl_LeadsCompanyContact_Insert", prm);
                    }
                    else
                    {
                        try
                        {
                            prm = new SqlParameter[29];
                            prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                            prm[1] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
                            prm[2] = new SqlParameter("@cdate", da1.getcdatetime());
                            prm[3] = new SqlParameter("@Salutation", (Salutation.SelectedValue == "" ? "0" : Salutation.SelectedValue));
                            prm[4] = new SqlParameter("@FirstName", fName.Text);
                            prm[5] = new SqlParameter("@MiddleName", "");
                            prm[6] = new SqlParameter("@LastName", lName.Text);
                            prm[7] = new SqlParameter("@Companyid", companyid);
                            prm[8] = new SqlParameter("@Suffix", "");
                            prm[9] = new SqlParameter("@ContactOwner", ownerid);
                            prm[10] = new SqlParameter("@Designation", title.Text);
                            prm[11] = new SqlParameter("@PhoneNo", PhoneNo.Text);
                            prm[12] = new SqlParameter("@Emailid", EmailId.Text);
                            prm[13] = new SqlParameter("@MobNo", PhoneNo.Text);
                            prm[14] = new SqlParameter("@FaxbNo", "");
                            prm[15] = new SqlParameter("@ReportTo", "");
                            prm[16] = new SqlParameter("@DepartmentName", "");
                            prm[17] = new SqlParameter("@MailingAddress", AddressDetails.Value);
                            prm[18] = new SqlParameter("@MailingStreet", StreetName.Value);
                            prm[19] = new SqlParameter("@MailingCityId", Pcityid);
                            prm[20] = new SqlParameter("@MailingCityName", Pcityname);
                            prm[21] = new SqlParameter("@MailingPostalCode", PostalCode.Value);
                            prm[22] = new SqlParameter("@MailingStateId", Pstateid);
                            prm[23] = new SqlParameter("@MailingStateName", Pstatename);
                            prm[24] = new SqlParameter("@MailingCountryId", (CountryId.SelectedValue == "" ? "0" : CountryId.SelectedValue));
                            prm[25] = new SqlParameter("@MailingCountryName", CountryId.SelectedItem.Text);
                            prm[26] = new SqlParameter("@leadid", rid);
                            prm[27] = new SqlParameter("@Descriptions", DBNull.Value);
                            prm[28] = new SqlParameter("ReturnValue", SqlDbType.Int);
                            prm[28].Direction = ParameterDirection.ReturnValue;
                            contactid = Convert.ToString(da.executeDMLQuery("crm_tbl_LeadsCompanyContact_ConvertedInsert", prm));
                        }
                        catch
                        { }
                    }
                }

                try
                {
                    //setnotification(rid.ToString(), "20", "1");
                    sendnotificationlead(rid, 20, 1);
                }
                catch
                { }

                //if (Request.QueryString["id"] != null)
                //{
                //Page.RegisterClientScriptBlock("", "<script>alert('Data Saved Successfully!!');document.location.href ='/crm/AddPropertyLeads.aspx?id=" + Request.QueryString["id"].ToString() + "'; </script>");
                Page.RegisterClientScriptBlock("", "<script>alert('Data Saved Successfully!!');document.location.href ='/crm/AddNewLeads.aspx'; </script>");
                //}
                //else
                //{
                //    Page.RegisterClientScriptBlock("", "<script>alert('Data Saved Successfully!!');document.location.href ='/crm/AddPropertyLeads.aspx'; </script>");
                //}


                if (entrytype == "Insert")
                {
                    entrytype = "1";
                }
                else if (entrytype == "Update")
                {
                    entrytype = "2";
                }

            }
        }
        else
        {
            string isdouplicateid = checkIsDouplicatelead(PhoneNo.Text.Trim()).ToString();
            if (isdouplicateid != "" && isdouplicateid != "0" && (HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() == "2201" || HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() == "2203" || HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() == "2176"))
            {
                da.executeDMLQueryinline("update tbl_mainleads set Islost=0 where id='" + isdouplicateid + "'");
                if (LeadSource.SelectedValue == "0")
                {
                    LeadSourceid = "0";
                }
                else
                {
                    if (hdnnewsource.Value == "1" && txtnewsource.Value != "")
                    {
                        try
                        {
                            SqlParameter[] prm1 = new SqlParameter[4];
                            prm1[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                            prm1[1] = new SqlParameter("@name", txtnewsource.Value);
                            prm1[2] = new SqlParameter("@typevalue", "check");
                            prm1[3] = new SqlParameter("@type", "1");
                            DataSet dsc = da1.GetDataSet("crm_ups_tbl_crmmasterdata", prm1);
                            if (dsc.Tables[0].Rows.Count > 0)
                            {
                                LeadSourceid = dsc.Tables[0].Rows[0]["id"].ToString();
                            }
                            else
                            {
                                SqlParameter[] prm2 = new SqlParameter[5];
                                prm2[0] = new SqlParameter("@type", "1");
                                prm2[3] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                                prm2[1] = new SqlParameter("@name", txtnewsource.Value);
                                prm2[2] = new SqlParameter("@typevalue", "insert");
                                prm2[4] = new SqlParameter("ReturnValue", SqlDbType.Int);
                                prm2[4].Direction = ParameterDirection.ReturnValue;
                                int id = da1.executeDMLQuery("crm_ups_tbl_crmmasterdata_New", prm2);
                                LeadSourceid = id.ToString();

                            }
                        }
                        catch
                        { }
                    }
                    else
                    {
                        LeadSourceid = LeadSource.SelectedValue;
                    }
                }
                if (Salutation.SelectedValue == "" || Salutation.SelectedValue == "-1")
                {
                    Salutationid = "0";
                }
                else
                {
                    Salutationid = Salutation.SelectedValue;
                }
                if (Industry.SelectedValue == "0" || Industry.SelectedValue == "-1")
                {
                    IndustryTypeid = "0";
                }
                else
                {
                    IndustryTypeid = Industry.SelectedValue;
                }

                ownerid = Getownerid(isdouplicateid);
                if (ownerid == "" || ownerid == "0")
                {
                    if (Owner.SelectedValue == "" || Owner.SelectedValue == "-1")
                    {
                        ownerid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();
                    }
                    else
                    {
                        ownerid = Owner.SelectedValue;
                    }
                }

                if (leadStatus.SelectedValue == "" || leadStatus.SelectedValue == "-1")
                {
                    string strleadstatus = "select id  from tbl_leadstatus where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and isnull(IsDefault,0)=1";
                    DataSet dschk = da1.GetDataSetInline(strleadstatus);
                    if (dschk.Tables[0].Rows.Count > 0)
                    {
                        leadStatusid = dschk.Tables[0].Rows[0]["id"].ToString();
                    }

                }
                else
                {
                    leadStatusid = leadStatus.SelectedValue;
                }
                string cmpname = cName.Text;
                if (cmpname == "")
                {
                    cmpname = fName.Text;
                    if (lName.Text.Trim() != "")
                    {
                        cmpname += " " + lName.Text;
                    }
                }
                string chkcountryid = getcontry_map();
                string Pcityid = "0";
                string Pstateid = "0";
                string Pcityname = CityName.Value;
                string Pstatename = StateName.Value;
                if (chkcountryid == "77")
                {
                    if (StateId.SelectedValue != "" && StateId.SelectedValue != "-1")
                    {
                        Pstateid = StateId.SelectedValue;
                        Pstatename = StateId.SelectedItem.Text;
                    }
                    if (CityId.SelectedValue != "" && CityId.SelectedValue != "-1")
                    {
                        Pcityid = CityId.SelectedValue;
                        Pcityname = CityId.SelectedItem.Text;
                    }
                    else if (CityId.SelectedValue == "-1")
                    {
                        if (CityName.Value != string.Empty)
                        {
                            SqlParameter[] prmcity = new SqlParameter[4];
                            prmcity[0] = new SqlParameter("@Stateid", StateId.SelectedValue);
                            prmcity[1] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"]);
                            prmcity[2] = new SqlParameter("ReturnValue", SqlDbType.Int);
                            prmcity[2].Direction = ParameterDirection.ReturnValue;
                            prmcity[3] = new SqlParameter("@cityName", CityName.Value);
                            Pcityid = da.executeDMLQuery("insertcity", prmcity).ToString();
                            Pcityname = CityName.Value;

                        }
                    }
                }

                //SqlParameter[] prm = new SqlParameter[36];
                //prm[0] = new SqlParameter("@id", editid);
                //prm[1] = new SqlParameter("@type", entrytype);
                //prm[2] = new SqlParameter("@leadid", isdouplicateid);
                //prm[3] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                //prm[4] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
                //prm[5] = new SqlParameter("@cdate", da1.getcdatetime());
                //prm[6] = new SqlParameter("@Salutation", (Salutation.SelectedValue == "" ? "0" : Salutation.SelectedValue));
                //prm[7] = new SqlParameter("@FirstName", fName.Text);
                //prm[8] = new SqlParameter("@LastName", lName.Text);
                //prm[9] = new SqlParameter("@Company", cmpname);// cName.Text);
                //prm[10] = new SqlParameter("@Title", title.Text);
                //prm[11] = new SqlParameter("@Website", website.Text);
                //prm[12] = new SqlParameter("@Descriptions", description.Value);
                //prm[13] = new SqlParameter("@LeadStatus", leadStatusid);
                //prm[14] = new SqlParameter("@OwnerName", ownerid);
                //prm[15] = new SqlParameter("@PhoneNo", PhoneNo.Text);
                //prm[16] = new SqlParameter("@EmailId", EmailId.Text);
                //prm[17] = new SqlParameter("@LAddress", AddressDetails.Value);
                //prm[18] = new SqlParameter("@Street", StreetName.Value);
                //prm[19] = new SqlParameter("@CityId", Pcityid);
                //prm[20] = new SqlParameter("@CityName", CityName.Value);
                //prm[21] = new SqlParameter("@PostalCode", PostalCode.Value);
                //prm[22] = new SqlParameter("@StateId", Pstateid);
                //prm[23] = new SqlParameter("@StateName", StateName.Value);
                //prm[24] = new SqlParameter("@CountryId", CountryId.SelectedValue);
                //prm[25] = new SqlParameter("@CountryName", "");
                //prm[26] = new SqlParameter("@NoOfEmployee", NoOfemp.Value);
                //prm[27] = new SqlParameter("@AnnualRevenue", AnnualRevenue.Value);
                //prm[28] = new SqlParameter("@LeadSourceid", LeadSourceid);
                //prm[29] = new SqlParameter("@IndustryTypeid", IndustryTypeid);
                //prm[30] = new SqlParameter("@Leadproduct", hdnddLeadProduct.Value);
                //prm[31] = new SqlParameter("@Satageupdatetime", da1.getcdatetime());
                //prm[32] = new SqlParameter("@LProbability", hdnleadProbability.Value);
                //prm[33] = new SqlParameter("@TeleGramBoatId", boatid.Text);
                //prm[34] = new SqlParameter("@leadnotes", txtlnotes.Value);
                //prm[35] = new SqlParameter("ReturnValue", SqlDbType.Int);
                //prm[35].Direction = ParameterDirection.ReturnValue;
                //int rid = da1.executeDMLQuery("crm_tbl_mainleadsD_Insert", prm);
                SqlParameter[] prm = new SqlParameter[35];
                prm[0] = new SqlParameter("@id", editid);
                prm[1] = new SqlParameter("@type", entrytype);
                prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
                prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
                prm[5] = new SqlParameter("@Salutation", (Salutation.SelectedValue == "" ? "0" : Salutation.SelectedValue));
                prm[6] = new SqlParameter("@FirstName", fName.Text);
                prm[7] = new SqlParameter("@LastName", lName.Text);
                prm[8] = new SqlParameter("@Company", cmpname);// cName.Text);
                prm[9] = new SqlParameter("@Title", title.Text);
                prm[10] = new SqlParameter("@Website", website.Text);
                prm[11] = new SqlParameter("@Descriptions", description.Value);
                prm[12] = new SqlParameter("@LeadStatus", leadStatusid);
                prm[13] = new SqlParameter("@OwnerName", ownerid);
                prm[14] = new SqlParameter("@PhoneNo", PhoneNo.Text);
                prm[15] = new SqlParameter("@EmailId", EmailId.Text);
                prm[16] = new SqlParameter("@LAddress", AddressDetails.Value);
                prm[17] = new SqlParameter("@Street", StreetName.Value);
                prm[18] = new SqlParameter("@CityId", Pcityid);
                prm[19] = new SqlParameter("@CityName", Pcityname);
                prm[20] = new SqlParameter("@PostalCode", PostalCode.Value);
                prm[21] = new SqlParameter("@StateId", Pstateid);
                prm[22] = new SqlParameter("@StateName", Pstatename);
                prm[23] = new SqlParameter("@CountryId", CountryId.SelectedValue);
                prm[24] = new SqlParameter("@CountryName", "");
                prm[25] = new SqlParameter("@NoOfEmployee", NoOfemp.Value);
                prm[26] = new SqlParameter("@AnnualRevenue", AnnualRevenue.Value);
                prm[27] = new SqlParameter("@LeadSourceid", LeadSourceid);
                prm[28] = new SqlParameter("@IndustryTypeid", IndustryTypeid);
                prm[29] = new SqlParameter("@Leadproduct", hdnddLeadProduct.Value);
                prm[30] = new SqlParameter("@Satageupdatetime", da1.getcdatetime());
                prm[31] = new SqlParameter("@LProbability", hdnleadProbability.Value);
                prm[32] = new SqlParameter("@TeleGramBoatId", boatid.Text);
                prm[33] = new SqlParameter("@leadnotes", txtlnotes.Value);
                prm[34] = new SqlParameter("ReturnValue", SqlDbType.Int);
                prm[34].Direction = ParameterDirection.ReturnValue;
                int rid = da1.executeDMLQuery("crm_tbl_mainleads_Insert_website", prm);
                Page.RegisterClientScriptBlock("", "<script>alert('Data Saved Successfully!!');document.location.href =document.location.href; </script>");
                //try
                //{
                //    //setnotification(rid.ToString(), "20", "1");
                //    resendnotificationlead(Convert.ToInt32(isdouplicateid), 20, 1, rid.ToString());
                //}
                //catch
                //{ }

            }
            else
            {
                if (LeadSource.SelectedValue == "0")
                {
                    LeadSourceid = "0";
                }
                else
                {
                    if (hdnnewsource.Value == "1" && txtnewsource.Value != "")
                    {
                        try
                        {
                            SqlParameter[] prm1 = new SqlParameter[4];
                            prm1[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                            prm1[1] = new SqlParameter("@name", txtnewsource.Value);
                            prm1[2] = new SqlParameter("@typevalue", "check");
                            prm1[3] = new SqlParameter("@type", "1");
                            DataSet dsc = da1.GetDataSet("crm_ups_tbl_crmmasterdata", prm1);
                            if (dsc.Tables[0].Rows.Count > 0)
                            {
                                LeadSourceid = dsc.Tables[0].Rows[0]["id"].ToString();
                            }
                            else
                            {
                                SqlParameter[] prm2 = new SqlParameter[5];
                                prm2[0] = new SqlParameter("@type", "1");
                                prm2[3] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                                prm2[1] = new SqlParameter("@name", txtnewsource.Value);
                                prm2[2] = new SqlParameter("@typevalue", "insert");
                                prm2[4] = new SqlParameter("ReturnValue", SqlDbType.Int);
                                prm2[4].Direction = ParameterDirection.ReturnValue;
                                int id = da1.executeDMLQuery("crm_ups_tbl_crmmasterdata_New", prm2);
                                LeadSourceid = id.ToString();

                            }
                        }
                        catch
                        { }
                    }
                    else
                    {
                        LeadSourceid = LeadSource.SelectedValue;
                    }
                }
                if (Salutation.SelectedValue == "" || Salutation.SelectedValue == "-1")
                {
                    Salutationid = "0";
                }
                else
                {
                    Salutationid = Salutation.SelectedValue;
                }
                if (Industry.SelectedValue == "0" || Industry.SelectedValue == "-1")
                {
                    IndustryTypeid = "0";
                }
                else
                {
                    IndustryTypeid = Industry.SelectedValue;
                }
                if (Owner.SelectedValue == "" || Owner.SelectedValue == "-1")
                {
                    ownerid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();
                }
                else
                {
                    ownerid = Owner.SelectedValue;
                }
                if (leadStatus.SelectedValue == "" || leadStatus.SelectedValue == "-1")
                {
                    string strleadstatus = "select id  from tbl_leadstatus where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and isnull(IsDefault,0)=1";
                    DataSet dschk = da1.GetDataSetInline(strleadstatus);
                    if (dschk.Tables[0].Rows.Count > 0)
                    {
                        leadStatusid = dschk.Tables[0].Rows[0]["id"].ToString();
                    }

                }
                else
                {
                    leadStatusid = leadStatus.SelectedValue;
                }
                string chkcountryid = getcontry_map();
                string Pcityid = "0";
                string Pstateid = "0";
                string Pcityname = CityName.Value;
                string Pstatename = StateName.Value;
                if (chkcountryid == "77")
                {
                    if (StateId.SelectedValue != "" && StateId.SelectedValue != "-1")
                    {
                        Pstateid = StateId.SelectedValue;
                        Pstatename = StateId.SelectedItem.Text;
                    }
                    if (CityId.SelectedValue != "" && CityId.SelectedValue != "-1")
                    {
                        Pcityid = CityId.SelectedValue;
                        Pcityname = CityId.SelectedItem.Text;
                    }
                    else if (CityId.SelectedValue == "-1")
                    {
                        if (CityName.Value != string.Empty)
                        {
                            SqlParameter[] prmcity = new SqlParameter[4];
                            prmcity[0] = new SqlParameter("@Stateid", StateId.SelectedValue);
                            prmcity[1] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"]);
                            prmcity[2] = new SqlParameter("ReturnValue", SqlDbType.Int);
                            prmcity[2].Direction = ParameterDirection.ReturnValue;
                            prmcity[3] = new SqlParameter("@cityName", CityName.Value);
                            Pcityid = da.executeDMLQuery("insertcity", prmcity).ToString();
                            Pcityname = CityName.Value;

                        }
                    }
                }
                SqlParameter[] prm = new SqlParameter[35];
                prm[0] = new SqlParameter("@id", editid);
                prm[1] = new SqlParameter("@type", entrytype);
                prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
                prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
                prm[5] = new SqlParameter("@Salutation", (Salutation.SelectedValue == "" ? "0" : Salutation.SelectedValue));
                prm[6] = new SqlParameter("@FirstName", fName.Text);
                prm[7] = new SqlParameter("@LastName", lName.Text);
                prm[8] = new SqlParameter("@Company", cName.Text);
                prm[9] = new SqlParameter("@Title", title.Text);
                prm[10] = new SqlParameter("@Website", website.Text);
                prm[11] = new SqlParameter("@Descriptions", description.Value);
                prm[12] = new SqlParameter("@LeadStatus", leadStatusid);
                prm[13] = new SqlParameter("@OwnerName", ownerid);
                prm[14] = new SqlParameter("@PhoneNo", PhoneNo.Text);
                prm[15] = new SqlParameter("@EmailId", EmailId.Text);
                prm[16] = new SqlParameter("@LAddress", AddressDetails.Value);
                prm[17] = new SqlParameter("@Street", StreetName.Value);
                prm[18] = new SqlParameter("@CityId", Pcityid);
                prm[19] = new SqlParameter("@CityName", Pcityname);
                prm[20] = new SqlParameter("@PostalCode", PostalCode.Value);
                prm[21] = new SqlParameter("@StateId", Pstateid);
                prm[22] = new SqlParameter("@StateName", Pstatename);
                prm[23] = new SqlParameter("@CountryId", CountryId.SelectedValue);
                prm[24] = new SqlParameter("@CountryName", "");
                prm[25] = new SqlParameter("@NoOfEmployee", NoOfemp.Value);
                prm[26] = new SqlParameter("@AnnualRevenue", AnnualRevenue.Value);
                prm[27] = new SqlParameter("@LeadSourceid", LeadSourceid);
                prm[28] = new SqlParameter("@IndustryTypeid", IndustryTypeid);
                prm[29] = new SqlParameter("@Leadproduct", DBNull.Value);
                prm[30] = new SqlParameter("@Satageupdatetime", da1.getcdatetime());
                prm[31] = new SqlParameter("@LProbability", hdnleadProbability.Value);
                prm[32] = new SqlParameter("@TeleGramBoatId", boatid.Text);
                prm[33] = new SqlParameter("@leadnotes", txtlnotes.Value);
                prm[34] = new SqlParameter("ReturnValue", SqlDbType.Int);
                prm[34].Direction = ParameterDirection.ReturnValue;
                int rid = da1.executeDMLQuery("crm_tbl_mainleads_Insert", prm);
                string companyid = "0";
                string contactid = "0";
                if (rid > 0)
                {
                    string cmpname = cName.Text;
                    if (cmpname == "")
                    {
                        cmpname = fName.Text;
                        if (lName.Text.Trim() != "")
                        {
                            cmpname += " " + lName.Text;
                        }
                    }
                    string strsqlchk = "";
                    strsqlchk = "select id,Company from tbl_LeadsCompanies where status='1' and archive='0' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and leadid='" + rid + "'";
                    DataSet dschk = da1.GetDataSetInline(strsqlchk);
                    if (dschk.Tables[0].Rows.Count > 0)
                    {
                        companyid = dschk.Tables[0].Rows[0]["id"].ToString();
                        prm = new SqlParameter[20];
                        prm[0] = new SqlParameter("@id", companyid);
                        prm[1] = new SqlParameter("@entrytype", "Update");
                        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
                        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
                        prm[5] = new SqlParameter("@Company", cName.Text);
                        prm[6] = new SqlParameter("@CompanyOwner", ownerid);
                        prm[7] = new SqlParameter("@Website", website.Text);
                        prm[8] = new SqlParameter("@PhoneNo", PhoneNo.Text);
                        prm[9] = new SqlParameter("@BillingAddress", AddressDetails.Value);
                        prm[10] = new SqlParameter("@Street", StreetName.Value);
                        prm[11] = new SqlParameter("@CityId", Pcityid);
                        prm[12] = new SqlParameter("@CityName", Pcityname);
                        prm[13] = new SqlParameter("@PostalCode", PostalCode.Value);
                        prm[14] = new SqlParameter("@StateId", Pstateid);
                        prm[15] = new SqlParameter("@StateName", Pstatename);
                        prm[16] = new SqlParameter("@CountryId", (CountryId.SelectedValue == "" ? "0" : CountryId.SelectedValue));
                        prm[17] = new SqlParameter("@CountryName", CountryId.SelectedItem.Text);
                        prm[18] = new SqlParameter("@leadid", rid);
                        prm[19] = new SqlParameter("@IndustryTypeid", IndustryTypeid);
                        da.executeDMLQuery("crm_tbl_LeadsCompanies_Insert", prm);
                    }
                    else
                    {
                        SqlParameter[] prmc = new SqlParameter[3];
                        prmc[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                        prmc[1] = new SqlParameter("@company", cmpname);
                        prmc[2] = new SqlParameter("@phonno", PhoneNo.Text);
                        dschk = da.GetDataSet("getcompanyid", prmc);
                        if (dschk.Tables[0].Rows.Count > 0)
                        {
                            companyid = dschk.Tables[0].Rows[0]["id"].ToString();
                            prm = new SqlParameter[20];
                            prm[0] = new SqlParameter("@id", companyid);
                            prm[1] = new SqlParameter("@entrytype", "Update");
                            prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                            prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
                            prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
                            prm[5] = new SqlParameter("@Company", cmpname);
                            prm[6] = new SqlParameter("@CompanyOwner", ownerid);
                            prm[7] = new SqlParameter("@Website", website.Text);
                            prm[8] = new SqlParameter("@PhoneNo", PhoneNo.Text);
                            prm[9] = new SqlParameter("@BillingAddress", AddressDetails.Value);
                            prm[10] = new SqlParameter("@Street", StreetName.Value);
                            prm[11] = new SqlParameter("@CityId", Pcityid);
                            prm[12] = new SqlParameter("@CityName", Pcityname);
                            prm[13] = new SqlParameter("@PostalCode", PostalCode.Value);
                            prm[14] = new SqlParameter("@StateId", Pstateid);
                            prm[15] = new SqlParameter("@StateName", Pstatename);
                            prm[16] = new SqlParameter("@CountryId", (CountryId.SelectedValue == "" ? "0" : CountryId.SelectedValue));
                            prm[17] = new SqlParameter("@CountryName", CountryId.SelectedItem.Text);
                            prm[18] = new SqlParameter("@leadid", rid);
                            prm[19] = new SqlParameter("@IndustryTypeid", IndustryTypeid);
                            da.executeDMLQuery("crm_tbl_LeadsCompanies_Insert", prm);

                        }
                        else
                        {
                            try
                            {
                                prm = new SqlParameter[31];
                                prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                                prm[1] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
                                prm[2] = new SqlParameter("@cdate", da1.getcdatetime());
                                prm[3] = new SqlParameter("@Company", cmpname);
                                prm[4] = new SqlParameter("@CompanyOwner", ownerid);
                                prm[5] = new SqlParameter("@Website", website.Text);
                                prm[6] = new SqlParameter("@Descriptions", DBNull.Value);
                                prm[7] = new SqlParameter("@PhoneNo", PhoneNo.Text);
                                prm[8] = new SqlParameter("@BillingAddress", AddressDetails.Value);
                                prm[9] = new SqlParameter("@Street", StreetName.Value);
                                prm[10] = new SqlParameter("@CityId", Pcityid);
                                prm[11] = new SqlParameter("@CityName", Pcityname);
                                prm[12] = new SqlParameter("@PostalCode", PostalCode.Value);
                                prm[13] = new SqlParameter("@StateId", Pstateid);
                                prm[14] = new SqlParameter("@StateName", Pstatename);
                                prm[15] = new SqlParameter("@CountryId", (CountryId.SelectedValue == "" ? "0" : CountryId.SelectedValue));
                                prm[16] = new SqlParameter("@CountryName", CountryId.SelectedItem.Text);
                                prm[17] = new SqlParameter("@ShippingAddress", AddressDetails.Value);
                                prm[18] = new SqlParameter("@ShippingStreet", StreetName.Value);
                                prm[19] = new SqlParameter("@ShippingCityId", Pcityid);
                                prm[20] = new SqlParameter("@ShippingCityName", Pcityname);
                                prm[21] = new SqlParameter("@ShippingPostalCode", PostalCode.Value);
                                prm[22] = new SqlParameter("@ShippingStateId", Pstateid);
                                prm[23] = new SqlParameter("@ShippingStateName", Pstatename);
                                prm[24] = new SqlParameter("@ShippingCountryId", (CountryId.SelectedValue == "" ? "0" : CountryId.SelectedValue));

                                prm[25] = new SqlParameter("@ShippingCountryName", CountryId.SelectedItem.Text);
                                prm[26] = new SqlParameter("@leadid", rid);
                                prm[27] = new SqlParameter("@IndustryTypeid", IndustryTypeid);
                                prm[28] = new SqlParameter("@PhoneNoOther", PhoneNo.Text);

                                prm[29] = new SqlParameter("@NoOfEmployee", DBNull.Value);

                                prm[30] = new SqlParameter("ReturnValue", SqlDbType.Int);
                                prm[30].Direction = ParameterDirection.ReturnValue;

                                companyid = Convert.ToString(da.executeDMLQuery("crm_tbl_LeadsCompanies_ConvertedInsert", prm));
                                //SaveLogText("companyid: "+ companyid);
                            }
                            catch
                            { }
                        }

                    }
                    strsqlchk = "select id from tbl_LeadsCompanyContact where status='1' and archive='0' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and leadid='" + rid + "'";
                    dschk = da1.GetDataSetInline(strsqlchk);
                    if (dschk.Tables[0].Rows.Count > 0)
                    {
                        contactid = dschk.Tables[0].Rows[0]["id"].ToString();
                        prm = new SqlParameter[29];
                        prm[0] = new SqlParameter("@id", contactid);
                        prm[1] = new SqlParameter("@entrytype", "Update");
                        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
                        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
                        prm[5] = new SqlParameter("@Salutation", Salutation.SelectedValue);
                        prm[6] = new SqlParameter("@FirstName", fName.Text);
                        prm[7] = new SqlParameter("@LastName", lName.Text);
                        prm[8] = new SqlParameter("@Companyid", companyid);
                        prm[9] = new SqlParameter("@Suffix", "");
                        prm[10] = new SqlParameter("@ContactOwner", ownerid);
                        prm[11] = new SqlParameter("@Designation", title.Text);
                        prm[12] = new SqlParameter("@PhoneNo", PhoneNo.Text);
                        prm[13] = new SqlParameter("@Emailid", EmailId.Text);
                        prm[14] = new SqlParameter("@MobNo", PhoneNo.Text);
                        prm[15] = new SqlParameter("@FaxbNo", "");
                        prm[16] = new SqlParameter("@ReportTo", "");
                        prm[17] = new SqlParameter("@DepartmentName", "");
                        prm[18] = new SqlParameter("@MailingAddress", AddressDetails.Value);
                        prm[19] = new SqlParameter("@MailingStreet", StreetName.Value);
                        prm[20] = new SqlParameter("@MailingCityId", Pcityid);
                        prm[21] = new SqlParameter("@MailingCityName", Pcityname);
                        prm[22] = new SqlParameter("@MailingPostalCode", PostalCode.Value);
                        prm[23] = new SqlParameter("@MailingStateId", Pstateid);
                        prm[24] = new SqlParameter("@MailingStateName", Pstatename);
                        prm[25] = new SqlParameter("@MailingCountryId", (CountryId.SelectedValue == "" ? "0" : CountryId.SelectedValue));
                        prm[26] = new SqlParameter("@MailingCountryName", CountryId.SelectedItem.Text);
                        prm[27] = new SqlParameter("@leadid", rid);
                        prm[28] = new SqlParameter("@Descriptions", DBNull.Value);
                        da.executeDMLQuery("crm_tbl_LeadsCompanyContact_Insert", prm);
                    }
                    else
                    {
                        strsqlchk = "select id from tbl_LeadsCompanyContact where status='1' and archive='0' and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and Salutation='" + Salutation.SelectedValue + "' and FirstName='" + fName.Text + "' and LastName='" + lName.Text + "'";
                        dschk = da1.GetDataSetInline(strsqlchk);
                        if (dschk.Tables[0].Rows.Count > 0)
                        {
                            contactid = dschk.Tables[0].Rows[0]["id"].ToString();
                            prm = new SqlParameter[29];
                            prm[0] = new SqlParameter("@id", contactid);
                            prm[1] = new SqlParameter("@entrytype", "Update");
                            prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                            prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
                            prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
                            prm[5] = new SqlParameter("@Salutation", Salutation.SelectedValue);
                            prm[6] = new SqlParameter("@FirstName", fName.Text);
                            prm[7] = new SqlParameter("@LastName", lName.Text);
                            prm[8] = new SqlParameter("@Companyid", companyid);
                            prm[9] = new SqlParameter("@Suffix", "");
                            prm[10] = new SqlParameter("@ContactOwner", ownerid);
                            prm[11] = new SqlParameter("@Designation", title.Text);
                            prm[12] = new SqlParameter("@PhoneNo", PhoneNo.Text);
                            prm[13] = new SqlParameter("@Emailid", EmailId.Text);
                            prm[14] = new SqlParameter("@MobNo", PhoneNo.Text);
                            prm[15] = new SqlParameter("@FaxbNo", "");
                            prm[16] = new SqlParameter("@ReportTo", "");
                            prm[17] = new SqlParameter("@DepartmentName", "");
                            prm[18] = new SqlParameter("@MailingAddress", AddressDetails.Value);
                            prm[19] = new SqlParameter("@MailingStreet", StreetName.Value);
                            prm[20] = new SqlParameter("@MailingCityId", Pcityid);
                            prm[21] = new SqlParameter("@MailingCityName", Pcityname);
                            prm[22] = new SqlParameter("@MailingPostalCode", PostalCode.Value);
                            prm[23] = new SqlParameter("@MailingStateId", Pstateid);
                            prm[24] = new SqlParameter("@MailingStateName", Pstatename);
                            prm[25] = new SqlParameter("@MailingCountryId", (CountryId.SelectedValue == "" ? "0" : CountryId.SelectedValue));
                            prm[26] = new SqlParameter("@MailingCountryName", CountryId.SelectedItem.Text);
                            prm[27] = new SqlParameter("@leadid", rid);
                            prm[28] = new SqlParameter("@Descriptions", DBNull.Value);
                            da.executeDMLQuery("crm_tbl_LeadsCompanyContact_Insert", prm);
                        }
                        else
                        {
                            try
                            {
                                prm = new SqlParameter[29];
                                prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                                prm[1] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
                                prm[2] = new SqlParameter("@cdate", da1.getcdatetime());
                                prm[3] = new SqlParameter("@Salutation", (Salutation.SelectedValue == "" ? "0" : Salutation.SelectedValue));
                                prm[4] = new SqlParameter("@FirstName", fName.Text);
                                prm[5] = new SqlParameter("@MiddleName", "");
                                prm[6] = new SqlParameter("@LastName", lName.Text);
                                prm[7] = new SqlParameter("@Companyid", companyid);
                                prm[8] = new SqlParameter("@Suffix", "");
                                prm[9] = new SqlParameter("@ContactOwner", ownerid);
                                prm[10] = new SqlParameter("@Designation", title.Text);
                                prm[11] = new SqlParameter("@PhoneNo", PhoneNo.Text);
                                prm[12] = new SqlParameter("@Emailid", EmailId.Text);
                                prm[13] = new SqlParameter("@MobNo", PhoneNo.Text);
                                prm[14] = new SqlParameter("@FaxbNo", "");
                                prm[15] = new SqlParameter("@ReportTo", "");
                                prm[16] = new SqlParameter("@DepartmentName", "");
                                prm[17] = new SqlParameter("@MailingAddress", AddressDetails.Value);
                                prm[18] = new SqlParameter("@MailingStreet", StreetName.Value);
                                prm[19] = new SqlParameter("@MailingCityId", Pcityid);
                                prm[20] = new SqlParameter("@MailingCityName", Pcityname);
                                prm[21] = new SqlParameter("@MailingPostalCode", PostalCode.Value);
                                prm[22] = new SqlParameter("@MailingStateId", Pstateid);
                                prm[23] = new SqlParameter("@MailingStateName", Pstatename);
                                prm[24] = new SqlParameter("@MailingCountryId", (CountryId.SelectedValue == "" ? "0" : CountryId.SelectedValue));
                                prm[25] = new SqlParameter("@MailingCountryName", CountryId.SelectedItem.Text);
                                prm[26] = new SqlParameter("@leadid", rid);
                                prm[27] = new SqlParameter("@Descriptions", DBNull.Value);
                                prm[28] = new SqlParameter("ReturnValue", SqlDbType.Int);
                                prm[28].Direction = ParameterDirection.ReturnValue;
                                contactid = Convert.ToString(da.executeDMLQuery("crm_tbl_LeadsCompanyContact_ConvertedInsert", prm));
                            }
                            catch
                            { }
                        }
                    }

                    try
                    {
                        //setnotification(rid.ToString(), "20", "1");
                        sendnotificationlead(rid, 20, 1);
                    }
                    catch
                    { }

                    Page.RegisterClientScriptBlock("", "<script>alert('Data Saved Successfully!!');document.location.href =document.location.href; </script>");


                }
            }
        }



        //}
        //    catch(Exception ex)
        //        {
        //       // Response.Write(ex.Message);
        //        //Response.End();
        //        }


    }
    protected string checkIsDouplicatelead(string mobileno)
    {
        string ret = "";
        if (mobileno.Length > 10)
        {
            mobileno = mobileno.Trim().Substring(mobileno.Length - 10);
        }
        //tbl_mainleads.PhoneNo='" + mobileno + "' 
        string strsql = "select id leadid   from tbl_mainleads " +
            "where  tbl_mainleads.status=1 " +
            "and tbl_mainleads.archive=0 and tbl_mainleads.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' AND RIGHT(REPLACE(PhoneNo, '[^0-9]', ''), 10) = RIGHT(REPLACE('" + mobileno + "', '[^0-9]', ''), 10)";
        DataSet dsnew = da.GetDataSetInline(strsql);
        if (dsnew.Tables[0].Rows.Count > 0)
        {

            ret = dsnew.Tables[0].Rows[0]["leadid"].ToString();

        }
        return ret;
    }
    protected string Getownerid(string leadid)
    {
        string ret = "";

        //tbl_mainleads.PhoneNo='" + mobileno + "' 
        string strsql = "select id leadid,isnull(OwnerName,0) ownerid   from tbl_mainleads " +
            "where  tbl_mainleads.status=1 " +
            "and tbl_mainleads.archive=0 and tbl_mainleads.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  and id='" + leadid + "'";
        DataSet dsnew = da.GetDataSetInline(strsql);
        if (dsnew.Tables[0].Rows.Count > 0)
        {

            ret = dsnew.Tables[0].Rows[0]["ownerid"].ToString();

        }
        return ret;
    }
    public string AssignLeadToNewSalesExecutive(string RegID, string empids, int noofleads)
    {


        int FinalStatus = 0;
        int OrderID = 0;
        int OrderIDLeads = 0;
        int SalesUserOrderID = 0;
        ///string PreviousLeadID = "";
        int TotalNumberOfSalesExecutive = 0;



        string salesExecutiveID = "0";
        string CurrentID = "";
        string NextID = "";
        int userindex = 0;
        string[] ar_user = empids.Split(',');
        DataSet dt_strrr = da.GetDataSetInline("select isnull(uid,0) as UserID,UserAssignNo,UserAssignLeads from LeadAssignTrackingForUser where RegID='" + RegID + "' ");

        if (ar_user.Length > 0)
        {
            if (dt_strrr.Tables[0].Rows.Count > 0)
            {
                SaveLogText("AgainCurrentID:" + CurrentID);
                CurrentID = dt_strrr.Tables[0].Rows[0]["UserID"].ToString();
                //OrderID = Convert.ToInt32(dt_str.Tables[0].Rows[0]["UserAssignNo"].ToString());
                OrderIDLeads = Convert.ToInt32(dt_strrr.Tables[0].Rows[0]["UserAssignLeads"].ToString() == "" ? "0" : dt_strrr.Tables[0].Rows[0]["UserAssignLeads"].ToString());
                SaveLogText("CurrentID:" + CurrentID);
                if (CurrentID != "")
                {
                    userindex = Array.IndexOf(ar_user, CurrentID);
                    //CurrentID=
                }
                SaveLogText("userindex:" + userindex);
                if (userindex != -1)
                {
                    if ((userindex + 1) == ar_user.Length)
                    {
                        if (OrderIDLeads < noofleads)
                        {
                            NextID = CurrentID;
                            OrderIDLeads = OrderIDLeads + 1;
                        }
                        else
                        {

                            NextID = ar_user[userindex + 1];
                            OrderIDLeads = 1;
                        }
                    }
                    else
                    {
                        if (OrderIDLeads < noofleads)
                        {
                            OrderIDLeads = OrderIDLeads + 1;
                            NextID = CurrentID;

                        }
                        else
                        {
                            OrderIDLeads = 1;
                            NextID = ar_user[userindex + 1];

                        }
                    }
                }
                else
                {
                    NextID = ar_user[0];
                    OrderIDLeads = 1;
                }

                salesExecutiveID = NextID;
                SaveLogText("salesExecutiveID:" + salesExecutiveID);
                prm = new SqlParameter[5];
                prm[0] = new SqlParameter("@uid", salesExecutiveID);
                prm[1] = new SqlParameter("@RegID", RegID);
                prm[2] = new SqlParameter("@ORDERID", (userindex + 1));
                prm[3] = new SqlParameter("@UserAssignLeads", OrderIDLeads);
                prm[4] = new SqlParameter("ReturnValue", SqlDbType.Int);
                prm[4].Direction = ParameterDirection.ReturnValue;
                FinalStatus = da.executeDMLQuery("tbl_update_AssignUserID_crm", prm);
            }
            else
            {
                OrderIDLeads = 1;
                SaveLogText("UserAssignLeads11555:ok" + OrderIDLeads);
                salesExecutiveID = ar_user[userindex];
                prm = new SqlParameter[5];
                prm[0] = new SqlParameter("@uid", salesExecutiveID);
                prm[1] = new SqlParameter("@RegID", RegID);
                prm[2] = new SqlParameter("@ORDERID", (userindex + 1));
                prm[3] = new SqlParameter("@UserAssignLeads", OrderIDLeads);
                prm[4] = new SqlParameter("ReturnValue", SqlDbType.Int);
                prm[4].Direction = ParameterDirection.ReturnValue;
                FinalStatus = da.executeDMLQuery("tbl_update_AssignUserID_New", prm);
                SaveLogText("FinalStatus:" + FinalStatus);
            }

        }

        return salesExecutiveID;
    }
    public string GetAssignUserId()
    {
        string ret_val = "";
        string sql_users = "select deptids as salesUserID,deptnoofleads as noofleads from tbl_leadsassignrules_crm  where regid ='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0 and isnull(isautoassign,0)=1 ";
        DataSet dsU = da.GetDataSetInline(sql_users);
        if (dsU.Tables[0].Rows.Count > 0)
        {
            string userid = dsU.Tables[0].Rows[0]["salesUserID"].ToString();
            int noofleads = Convert.ToInt32(dsU.Tables[0].Rows[0]["noofleads"].ToString() == "" ? "0" : dsU.Tables[0].Rows[0]["noofleads"].ToString());

            ret_val = AssignLeadToNewSalesExecutive(HttpContext.Current.Request.Cookies["cadmin"]["id"], userid, noofleads);

        }
        else
        {
            ret_val = "0";
        }
        return ret_val;
    }
    public int checkautoassign()
    {
        int ret_val = 0;
        string sql_users = "select deptids as salesUserID,deptnoofleads as noofleads from tbl_leadsassignrules_crm  where regid ='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0 and isnull(isautoassign,0)=1 ";
        DataSet dsU = da.GetDataSetInline(sql_users);
        if (dsU.Tables[0].Rows.Count > 0)
        {
            ret_val = 1;
        }
        else
        {
            ret_val = 0;
        }
        return ret_val;
    }
    public string getsearchvaluenew(string hdisrenbuy, string hdisResidential, string isrentids, string hdnbuyid, string hdnlistingbeds, string hdnlistingBaths, string hdnresidentialval, string hdncommercialval, string hdnlistingstatus, string hdnlocation, string minbudget, string maxbudget, string minsize, string maxsize)
    {

        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();

        string str_selecteddeatil = "Requirement:-";
        string strrentbuy1 = "";

        if (hdisrenbuy != "" && hdisrenbuy != "0")
        {
            if (hdisrenbuy == "1")
            {
                strrentbuy1 = "(Buy)";
            }
            else if (hdisrenbuy == "2")
            {
                strrentbuy1 = "(Rent)";
            }
            else
            {
                strrentbuy1 = "";
            }
            if (str_selecteddeatil == "")
            {
                //str_selecteddeatil += (hdisrenbuy=="2"?" Rent: ":" Buy: ");
                str_selecteddeatil += strrentbuy1 + " ";
            }
            else
            {
                //str_selecteddeatil += " &"+(hdisrenbuy=="2"?"Rent: ":"Buy: ");
                str_selecteddeatil += strrentbuy1 + " ";
            }

        }
        if (isrentids != "" && isrentids != "0" && hdisrenbuy == "2")
        {
            if (str_selecteddeatil == "")
            {
                str_selecteddeatil += "(" + getpurposedata("2", isrentids) + ")";
            }
            else
            {
                str_selecteddeatil += "(" + getpurposedata("2", isrentids) + ")";
            }

        }
        if (hdnbuyid != "" && hdnbuyid != "0" && hdisrenbuy == "1")
        {
            //Response.Write(getpurposedata("2", hdnbuyid.Value));
            //Response.End();
            if (str_selecteddeatil == "")
            {
                str_selecteddeatil += "(" + getpurposedata("1", hdnbuyid) + ")";
            }
            else
            {
                str_selecteddeatil += "(" + getpurposedata("1", hdnbuyid) + ")";
            }

        }
        if (hdnlocation != "")
        {
            if (str_selecteddeatil == "")
            {
                //str_selecteddeatil += " Location: " + getlocationdata("16,17,18", hdnlocation);
                str_selecteddeatil += " (" + getlocationdata("16,17,18", hdnlocation) + " -Location)";
            }
            else
            {
                //str_selecteddeatil += " &Location: " + getlocationdata("16,17,18", hdnlocation);
                str_selecteddeatil += " (" + getlocationdata("16,17,18", hdnlocation) + " -Location)";
            }
        }
        if (hdnlistingbeds != "" && hdnlistingbeds != "0")
        {

            str_selecteddeatil += " (" + getlistingdata("13", hdnlistingbeds) + " -Beds)";

        }
        string strr1 = "";
        if (hdisResidential != "" && hdisResidential != "0")
        {

            strr1 = (hdisResidential == "2" ? "Commercial" : "Residential");
        }
        if (hdnresidentialval != "" && hdnresidentialval != "0" && hdisResidential == "1")
        {

            str_selecteddeatil += " (" + getresidentialdata("1", hdnresidentialval) + "" + strr1 + ")";

        }
        else if (hdncommercialval != "" && hdncommercialval != "0" && hdisResidential == "2")
        {

            str_selecteddeatil += " (" + getresidentialdata("2", hdncommercialval) + "" + strr1 + ")";

        }
        else
        {
            strr1 = " (" + (hdisResidential == "2" ? "Commercial" : "Residential") + ")";
            str_selecteddeatil += strr1;
        }
        if (hdnlistingstatus != "" && hdnlistingstatus != "0")
        {

            str_selecteddeatil += "  (" + getlistingdata("15", hdnlistingstatus) + " -Status)";
        }

        string budg = "";
        if (minbudget != "0" && minbudget != "")
        {
            //Response.Write(dsrent.Tables[0].Rows[0]["budget_min"].ToString());
            //Response.End();
            //budg = "Budget: ";

            budg += " (" + minbudget;


        }
        if (maxbudget != "0" && maxbudget != "")
        {
            if (budg == "")
            {
                // budg += "Budget: " + maxbudget;
                budg += " (" + maxbudget + " AED)";
                // getlistingdata("27", dsrent.Tables[0].Rows[0]["budget_max"].ToString());
            }
            else
            {
                budg += " - " + maxbudget + " AED)";// getlistingdata("27", dsrent.Tables[0].Rows[0]["budget_max"].ToString());
            }

        }
        else
        {
            if (budg != "")
            {
                budg += " AED)";
            }

        }
        if (budg != "")
        {
            //str_selecteddeatil += " &" + budg;
            str_selecteddeatil += budg;
        }

        string siz = "";
        if (minsize != "0" && minsize != "")
        {
            //siz = "Size: ";
            siz = " (";

            siz += minsize;// getlistingdata("28", dsrent.Tables[0].Rows[0]["size_min"].ToString());

        }
        if (maxsize != "0" && maxsize != "")
        {

            if (siz == "")
            {
                //siz += "Size: " + maxsize;
                siz += " (" + maxsize + " Sqft)";
                // getlistingdata("28", dsrent.Tables[0].Rows[0]["size_max"].ToString());
            }
            else
            {
                //siz += " - " + maxsize;
                siz += " - " + maxsize + " Sqft)";
                // getlistingdata("28", dsrent.Tables[0].Rows[0]["size_max"].ToString());
            }
        }
        else
        {
            if (siz != "")
            {
                siz = " Sqft)";
            }
        }
        if (siz != "")
        {
            //str_selecteddeatil += " &" + siz;
            str_selecteddeatil += siz;
        }





        return str_selecteddeatil;
    }

    protected static void sendTeleGramMessageTelegram(string OwnerID, int LeadID)
    {
        //SaveLogText("telegram api parametesssssr from lead");
        string apiUrl = "";
        DataSet ds = new DataSet();
        DataSet ds_chatid = new DataSet();
        DataAccess da = new DataAccess();
        string TeleGramChatID = "";

        string strsql22 = "select * from  tbl_employee  where id='" + OwnerID + "' ";
        ds = da.GetDataSetInline(strsql22);
        if (ds.Tables[0].Rows.Count > 0)
        {
            TeleGramChatID = ds.Tables[0].Rows[0]["TeleGramUserID"].ToString();
        }

        if (TeleGramChatID != "")

        {
            string ChatID = "";
            string strCHATID = "select * from  tbl_TeleGramBotAddedUserList  where ChatId='" + TeleGramChatID + "' and status=1 and archive=0";
            ds_chatid = da.GetDataSetInline(strCHATID);
            if (ds_chatid.Tables[0].Rows.Count > 0)
            {
                ChatID = ds_chatid.Tables[0].Rows[0]["ChatId"].ToString();
            }
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            apiUrl = ConfigurationManager.AppSettings["gmailapi"].ToString() + "/api/values/sendTelegramMessage";
            var httpRequest1 = (HttpWebRequest)WebRequest.Create(apiUrl);
            httpRequest1.Method = "POST";
            httpRequest1.Accept = "application/json";
            httpRequest1.ContentType = "application/json";
            var data1 = @"{";
            data1 += " \"ChatId\":\"" + ChatID + "\",";
            data1 += " \"LeadID\":\"" + LeadID + "\",";
            data1 += " \"RegID\":\"" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "\"";
            data1 += "}";

            //SaveLogText("telegram api parameter from lead " + data1);

            using (var streamWriter = new StreamWriter(httpRequest1.GetRequestStream()))
            {
                streamWriter.Write(data1);
            }
            var httpResponse1 = (HttpWebResponse)httpRequest1.GetResponse();
            using (var streamReader = new StreamReader(httpResponse1.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd().Trim();
                //SaveLogText("Step 9:User Email Sent and Responce" + result + "");
            }


        }

    }




    protected void setnotification(string strid, string typeid, string ntfor)
    {
        string notif_type = "";
        DataAccess da = new DataAccess();

        string strsql = "select * from tbl_notificationtemplates where typeid='" + typeid + "' and ntfor='" + ntfor + "'  and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  and status=1 and archive=0";
        //SaveLogText("strsql " + strsql + "");
        DataSet dsnot = da.GetDataSetInline(strsql);
        if (dsnot.Tables[0].Rows.Count > 0)
        {
            DataRow[] dr_arr = dsnot.Tables[0].Select("notif_type=1");

            #region set for bell icon
            if (dr_arr.Length > 0)
            {
                notif_type = "1";
                setnotification_lead(strid, typeid, ntfor, notif_type);
                dr_arr = null;
            }
            #endregion
            #region set for email
            dr_arr = dsnot.Tables[0].Select("notif_type=2");
            if (dr_arr.Length > 0)
            {
                notif_type = "2";
                setnotification_lead(strid, typeid, ntfor, notif_type);
                dr_arr = null;
            }
            #endregion
            #region set for whatsapp
            dr_arr = dsnot.Tables[0].Select("notif_type=3");
            if (dr_arr.Length > 0)
            {
                notif_type = "3";
                SaveLogText("notif_type " + notif_type + "");
                setnotification_lead(strid, typeid, ntfor, notif_type);
                dr_arr = null;
            }
            #endregion
            #region set for sms
            dr_arr = dsnot.Tables[0].Select("notif_type=4");
            if (dr_arr.Length > 0)
            {
                notif_type = "4";
                setnotification_lead(strid, typeid, ntfor, notif_type);
                dr_arr = null;
            }
            #endregion
            #region set for screen
            dr_arr = dsnot.Tables[0].Select("notif_type=5");
            if (dr_arr.Length > 0)
            {
                notif_type = "5";
                setnotification_lead(strid, typeid, ntfor, notif_type);
                dr_arr = null;
            }
            #endregion
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
    protected void setnotification_lead(string strid, string typeid, string ntfor, string notif_type)
    {

        DataAccess da = new DataAccess();
        CommonFunction fns = new CommonFunction();
        //function fns = new function();
        try
        {
            string str_ntfor = "";
            if (ntfor == "1")
            {
                str_ntfor = "New Lead";
            }
            else if (ntfor == "2")
            {
                str_ntfor = "Modified Lead";
            }
            else if (ntfor == "2")
            {
                str_ntfor = "Deleted Lead";
            }
            #region set for bell icon
            string strsql = "select * from tbl_notificationtemplates where typeid='" + typeid + "' and ntfor='" + ntfor + "' and notif_type='" + notif_type + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0";
            //SaveLogText("strsql " + strsql);
            DataSet dsnot = da.GetDataSetInline(strsql);
            if (dsnot.Tables[0].Rows.Count > 0)
            {
                //SaveLogText("count " + dsnot.Tables[0].Rows.Count + "");
                string isview = dsnot.Tables[0].Rows[0]["isview"].ToString();
                string notificationtext = dsnot.Tables[0].Rows[0]["temptext"].ToString();
                string wtsapp_no = "";
                string wtsapp_cno = "";
                string wtsapp_email = "";
                string wtsapp_company = "";
                string wtsapp_leadno = "";
                string wtsapp_from = "";
                string wtsapp_req = "";
                string countryid = "0";

                string LeadDate = "&nbsp;";
                string LeadNo = "&nbsp;";
                string LeadName = "&nbsp;";
                string FirstName = "&nbsp;";
                string LastName = "&nbsp;";
                string Orgname = "&nbsp;";
                string OrgUser = "&nbsp;";
                string OrgPhone = "&nbsp;";
                string OrgWebsite = "&nbsp;";
                string refno = "&nbsp;";


                string sqldetails = @"select (tbl_mainleads.FirstName+' '+tbl_mainleads.LastName)LeadName ,tbl_mainleads.FirstName,tbl_mainleads.lastName,Title,company,PhoneNo,EmailId,Website,DisplayName Employee,tbl_leadstatus.StatusName,tbl_mainleads.cdate,tbl_mainleads.id,tbl_mainleads.LeadStatus,tbl_mainleads.LeadNo,tbl_mainleads.Descriptions,ISNULL(tbl_employee.country_Id,0) countryid
,(case when tbl_employee.mobile_no is null then tbl_employee.MobileNumber when tbl_employee.mobile_no ='' then tbl_employee.MobileNumber else tbl_employee.mobile_no end) mobilenumber from tbl_mainleads  
                            inner join tbl_employee on tbl_mainleads.OwnerName = tbl_employee.id
                            left join tbl_leadstatus on tbl_mainleads.LeadStatus = tbl_leadstatus.id
                            where tbl_mainleads.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and tbl_mainleads.id='" + strid + "' and tbl_mainleads.status=1 and tbl_mainleads.archive=0";
                //SaveLogText("sqldetails: " + sqldetails);
                DataSet dsdetails = da.GetDataSetInline(sqldetails);
                if (dsdetails.Tables[0].Rows.Count > 0)
                {
                    try
                    {
                        countryid = getcontry();
                    }
                    catch
                    { }
                    LeadDate = fns.dateConvert1(dsdetails.Tables[0].Rows[0]["cdate"].ToString()).ToString();
                    LeadNo = dsdetails.Tables[0].Rows[0]["LeadNo"].ToString();
                    LeadName = dsdetails.Tables[0].Rows[0]["LeadName"].ToString();
                    FirstName = dsdetails.Tables[0].Rows[0]["FirstName"].ToString();
                    LastName = dsdetails.Tables[0].Rows[0]["lastName"].ToString();
                    Orgname = dsdetails.Tables[0].Rows[0]["company"].ToString();
                    OrgUser = dsdetails.Tables[0].Rows[0]["company"].ToString();
                    OrgPhone = dsdetails.Tables[0].Rows[0]["PhoneNo"].ToString();
                    OrgWebsite = dsdetails.Tables[0].Rows[0]["LeadNo"].ToString();

                    wtsapp_cno = dsdetails.Tables[0].Rows[0]["PhoneNo"].ToString();
                    wtsapp_email = dsdetails.Tables[0].Rows[0]["EmailId"].ToString();
                    wtsapp_company = dsdetails.Tables[0].Rows[0]["company"].ToString();
                    wtsapp_no = dsdetails.Tables[0].Rows[0]["mobilenumber"].ToString();
                    wtsapp_leadno = LeadNo;
                    wtsapp_from = LeadName;
                    wtsapp_req = dsdetails.Tables[0].Rows[0]["Descriptions"].ToString(); ;
                    try
                    {

                        string str_selecteddeatil = "";
                        string strsqlrent = "select * from tbl_leadrent_crm where leadid='" + strid + "'";

                        DataSet dsrent = da.GetDataSetInline(strsqlrent);
                        if (dsrent.Tables[0].Rows.Count > 0)
                        {
                            string strrenbuy = "";
                            string strrentid = "";
                            string strbuyid = "";
                            string strlistingbeds = "";
                            string strResidential = "";
                            string strresidentialval = "";
                            string strcommercialval = "";
                            string strlistingstatus = "";
                            string strminB = "";
                            string strmaxB = "";
                            string strminS = "";
                            string strmaxS = "";
                            string strlocation = "";
                            str_selecteddeatil = "";
                            strlocation = dsrent.Tables[0].Rows[0]["dlocation"].ToString();
                            strrenbuy = dsrent.Tables[0].Rows[0]["renttype"].ToString();
                            string hdisrenbuy = dsrent.Tables[0].Rows[0]["renttype"].ToString();

                            string hdnrentid = dsrent.Tables[0].Rows[0]["rentdetails"].ToString();
                            strrentid = dsrent.Tables[0].Rows[0]["rentdetails"].ToString();

                            string hdnbuyid = dsrent.Tables[0].Rows[0]["buydetails"].ToString();
                            strbuyid = dsrent.Tables[0].Rows[0]["buydetails"].ToString();

                            string hdnlistingbeds = dsrent.Tables[0].Rows[0]["bedroomdetails"].ToString();
                            strlistingbeds = dsrent.Tables[0].Rows[0]["bedroomdetails"].ToString();

                            string hdnlistingBaths = dsrent.Tables[0].Rows[0]["bathroomdetails"].ToString();
                            string hdisResidential = dsrent.Tables[0].Rows[0]["residentialtype"].ToString();
                            strResidential = dsrent.Tables[0].Rows[0]["residentialtype"].ToString();
                            string hdnresidentialval = dsrent.Tables[0].Rows[0]["residentialdetails"].ToString();
                            strresidentialval = dsrent.Tables[0].Rows[0]["residentialdetails"].ToString();

                            string hdncommercialval = dsrent.Tables[0].Rows[0]["commertialialdetails"].ToString();
                            strcommercialval = dsrent.Tables[0].Rows[0]["commertialialdetails"].ToString();

                            string hdnlistingstatus = dsrent.Tables[0].Rows[0]["furnishing_status"].ToString();
                            strlistingstatus = dsrent.Tables[0].Rows[0]["furnishing_status"].ToString();

                            string budg = "";
                            strminB = dsrent.Tables[0].Rows[0]["budget_min"].ToString();

                            strmaxB = dsrent.Tables[0].Rows[0]["budget_max"].ToString();

                            string siz = "";
                            strminS = dsrent.Tables[0].Rows[0]["size_min"].ToString();

                            strmaxS = dsrent.Tables[0].Rows[0]["size_max"].ToString();

                            //lbldescription.Text = getsearchvalue(strrenbuy,strResidential,strrentid,strbuyid,strlistingbeds,"",strresidentialval,strcommercialval,strlistingstatus,strlocation,strminB,strmaxB,strminS,strmaxS);
                            wtsapp_req = getsearchvaluenew(strrenbuy, strResidential, strrentid, strbuyid, strlistingbeds, "", strresidentialval, strcommercialval, strlistingstatus, strlocation, strminB, strmaxB, strminS, strmaxS);
                            //lblrequirements.Text = lbldescription.Text;

                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
                try
                {
                    if (notif_type == "3")
                    {
                        //SaveLogText("wtsapp_no " + wtsapp_no + "");
                        if (wtsapp_no != "")
                        {
                            setnotification_leadwhatsapp(wtsapp_leadno, wtsapp_from, wtsapp_no, wtsapp_req, ntfor, countryid, wtsapp_cno, wtsapp_email, wtsapp_company);
                        }
                        //if (HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() == "2176")
                        //{
                        //    setnotification_leadwhatsapp(wtsapp_leadno, wtsapp_from, wtsapp_no, wtsapp_req, ntfor, countryid);
                        //}
                    }
                }
                catch
                {

                }
                //SaveLogText("notif_type " + notif_type + "");
                notificationtext = notificationtext.Replace("{####LeadDate####}", LeadDate
                    ).Replace("{####LeadNumber####}", LeadNo
                    ).Replace("{####RefNumber####}", refno
                    ).Replace("{####Customername####}", LeadName
                    ).Replace("{####CustomerFirstName####}", FirstName
                    ).Replace("{####CustomerLastName####}", LastName
                    ).Replace("{####OrganizationName####}", Orgname
                    ).Replace("{####OrganizationUser####}", OrgUser
                    ).Replace("{####OrganizationPhone####}", OrgPhone
                   ).Replace("{####OrganizationWebsite####}", OrgWebsite);
                char[] cha = { ',' };

                string strsqlitem = "select * from tbl_notificationsetupemp where type='" + typeid + "'  and ntfor='" + ntfor + "'  and notif_type='" + notif_type + "'  and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  and status=1 and archive=0  order by aLevel ";
                DataSet dsadd = da.GetDataSetInline(strsqlitem);
                if (dsadd.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < dsadd.Tables[0].Rows.Count; i++)
                    {

                        string[] arr = dsadd.Tables[0].Rows[i]["hdnempids"].ToString().Split(cha, StringSplitOptions.None);
                        string nuid = "0";
                        for (int j = 0; j < arr.Length; j++)
                        {
                            //wtsapp_no = "";
                            //wtsapp_no = getmobileno(arr[j].ToString());
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

                            }
                            if (notif_type == "2")
                            {
                                // Email Notification
                                try
                                {
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
                                    string stsignature = "";
                                    string strsqlchk = "select tbl_gmailconfiguration_crm.*,tbl_employee.Displayname from tbl_gmailconfiguration_crm inner join tbl_employee on tbl_gmailconfiguration_crm.uid=tbl_employee.id where tbl_gmailconfiguration_crm.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and tbl_gmailconfiguration_crm.uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"] + "' and  tbl_gmailconfiguration_crm.status=1 and tbl_gmailconfiguration_crm.archive=0 and configtype='1'";
                                    DataSet Dschk = da.GetDataSetInline(strsqlchk);
                                    if (Dschk.Tables[0].Rows.Count > 0)
                                    {
                                        stsignature = Dschk.Tables[0].Rows[0]["txtSignature"].ToString();
                                        if (stsignature != "")
                                        {
                                            body += "<br>" + stsignature;
                                        }
                                        //SaveLogText("send from :" + Dschk.Tables[0].Rows[0]["cfgmailid"].ToString());
                                        from = Dschk.Tables[0].Rows[0]["cfgmailid"].ToString();
                                        sendmailnew_common(strid, Dschk.Tables[0].Rows[0]["tokenid"].ToString(), Dschk.Tables[0].Rows[0]["refreshtokenid"].ToString(), to, from, cc, bcc, sub, body, filesdata, "1", "1", 1);
                                    }
                                    else
                                    {
                                        strsqlchk = "select tbl_gmailconfiguration_crm.*,tbl_employee.Displayname from tbl_gmailconfiguration_crm inner join tbl_employee on tbl_gmailconfiguration_crm.uid=tbl_employee.id where tbl_gmailconfiguration_crm.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and tbl_gmailconfiguration_crm.uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"] + "' and  tbl_gmailconfiguration_crm.status=1 and tbl_gmailconfiguration_crm.archive=0 and configtype='2'";
                                        Dschk = da.GetDataSetInline(strsqlchk);
                                        if (Dschk.Tables[0].Rows.Count > 0)
                                        {
                                            stsignature = Dschk.Tables[0].Rows[0]["txtSignature"].ToString();
                                            if (stsignature != "")
                                            {
                                                body += "<br>" + stsignature;
                                            }
                                            //SaveLogText("send from :" + Dschk.Tables[0].Rows[0]["cfgmailid"].ToString());
                                            from = Dschk.Tables[0].Rows[0]["cfgmailid"].ToString();
                                            sendmailnew_common(strid, Dschk.Tables[0].Rows[0]["tokenid"].ToString(), Dschk.Tables[0].Rows[0]["refreshtokenid"].ToString(), to, from, cc, bcc, sub, body, filesdata, "1", "1", 2);
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
                                catch
                                { }

                            }


                            if (notif_type == "3")
                            {
                                try
                                {
                                    string strsqlempto = "select id,DisplayName,Email,email_Id,(case when tbl_employee.mobile_no is null then tbl_employee.MobileNumber when tbl_employee.mobile_no ='' then tbl_employee.MobileNumber else tbl_employee.mobile_no end) mobilenumber from tbl_employee where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id ='" + arr[j] + "' order by id";
                                    DataSet dsempto = da.GetDataSetInline(strsqlempto);
                                    if (dsempto.Tables[0].Rows.Count > 0)
                                    {
                                        //SaveLogText(strsqlempto);
                                        wtsapp_no = dsempto.Tables[0].Rows[0]["MobileNumber"].ToString();
                                    }

                                    if (wtsapp_no != "")
                                    {
                                        setnotification_leadwhatsapp(wtsapp_leadno, wtsapp_from, wtsapp_no, wtsapp_req, ntfor, countryid, wtsapp_cno, wtsapp_email, wtsapp_company);
                                    }

                                    //SaveLogText("wtsapp_no " + wtsapp_no + "");
                                    //if (wtsapp_no != "")
                                    //{
                                    //    setnotification_leadwhatsapp(wtsapp_leadno, wtsapp_from, wtsapp_no, wtsapp_req, ntfor, countryid, wtsapp_cno, wtsapp_email, wtsapp_company);
                                    //}
                                }
                                catch
                                {

                                }
                            }


                        }
                    }
                }

                if (notif_type == "4")
                {
                    // SMS Notificatiomn
                }


            }
            #endregion
        }
        catch (Exception ex)
        {
            //SaveLogText("ex-1 " + ex.Message + "");
        }

    }

    protected static void sendmailnew_common(string entryid, string Tokenid, string RefTokenid, string To, string from, string CC, string BCC, string Subject, string Message, string[] files, string status, string entrytypeid = "0", int emailtype = 0)
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

        SaveLogText("Parameter " + data1);
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
    protected static void setnotification_leadwhatsapp(string leadno, string leadfrom, string mobileno, string message, string ntfor, string countryid, string contactno, string emailid, string company)
    {
        if (leadno == "")
        {
            leadno = "No Lead No";
        }
        if (leadfrom == "")
        {
            leadfrom = "No From Name";
        }
        if (contactno == "")
        {
            contactno = "No Contact No";
        }
        if (emailid == "")
        {
            emailid = "No Email Id";
        }
        if (company == "")
        {
            company = leadfrom;
            // company = "No Comnpany Name";
        }
        //if (message == "")
        //{
        //    message = "No Comnpany Name";
        //}
        DataAccess da = new DataAccess();
        CommonFunction fns = new CommonFunction();
        //function fns = new function();
        try
        {

            string numbers = "1234567890";

            string numbercode = "91";

            if (countryid == "8")
            {
                numbercode = "966";
            }
            if (countryid == "2")
            {
                numbercode = "971";
            }
            if (countryid == "7")
            {
                numbercode = "212";
            }
            mobileno = numbercode + mobileno;

            var data1 = @"{";
            data1 += "\"message\":[{";

            data1 += "\"recipient_whatsapp\":\"" + mobileno + "\",";
            data1 += "\"message_type\":\"template\",";
            data1 += "\"recipient_type\":\"individual\",";
            data1 += "\"type_template\":[{";

            data1 += "\"name\":\"add_new_lead\",";


            data1 += "\"attributes\":[\"" + leadno + "\",\"" + leadfrom + "\",\"" + contactno + "\",\"" + emailid + "\",\"" + company + "\",\"" + message + "\"],";



            data1 += "\"language\":{";
            data1 += "\"locale\":\"en\",";
            data1 += "\"policy\":\"deterministic\"";

            data1 += "}";
            data1 += "}";
            data1 += "]}";

            data1 += "]}";
            SaveLogText("data1:" + data1);
            string apiUrl = ConfigurationManager.AppSettings["Whatsupurl"].ToString();
            string apiKey = ConfigurationManager.AppSettings["WhatsupApiKey"].ToString();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var httpRequest1 = (HttpWebRequest)WebRequest.Create(apiUrl);

            httpRequest1.Method = "POST";
            httpRequest1.Headers.Add("Authorization", "Bearer " + apiKey);
            httpRequest1.ContentType = "application/json";
            httpRequest1.Accept = "application/json";


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
                    var msgdataid = data11.data;
                    string msgid = msgdataid.id;
                    string msgstr = data11.message;
                    string msgsts = data11.status;
                    //SaveLogText("msgid " + msgid + " Status:" + msgsts + " Message" + msgstr);
                }
            }

        }
        catch (Exception ex)
        {
            // SaveLogText("ex:" + ex.Message);
        }

    }
    protected static void setnotification_whatsapp(string rid = "", string regid = "", string mobileno = "", string templateid = "", string prms = "")
    {

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        string apiUrl = ConfigurationManager.AppSettings["gmailapi"].ToString() + "/api/values/sendWhatsAppAddLead";
        var httpRequest1 = (HttpWebRequest)WebRequest.Create(apiUrl);

        httpRequest1.Method = "POST";
        httpRequest1.Accept = "application/json";
        httpRequest1.ContentType = "application/json";
        var data1 = @"{";
        data1 += " \"LeadID\":\"" + rid + "\",";
        data1 += " \"RegID\":\"" + regid + "\",";
        data1 += " \"mobileno\":\"" + mobileno + "\",";
        data1 += " \"TemplateID\":\"" + templateid + "\",";
        data1 += " \"Parameters\":\"" + prms + "\"";

        data1 += "}";

        SaveLogText("Customer: " + data1);
        using (var streamWriter = new StreamWriter(httpRequest1.GetRequestStream()))
        {
            streamWriter.Write(data1);
        }
        try
        {
            var httpResponse1 = (HttpWebResponse)httpRequest1.GetResponse();
            using (var streamReader = new StreamReader(httpResponse1.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd().Trim();
                SaveLogText("New WhatApp API Result " + result);

            }
        }
        catch (Exception ex)
        {
            SaveLogText("error when read the data" + ex.Message);
        }



    }
    //protected static void setnotification_whatsapp(string mobileno, string countryid, string templateid = "", string template = "")
    //{

    //    //if (message == "")
    //    //{
    //    //    message = "No Comnpany Name";
    //    //}
    //    DataAccess da = new DataAccess();
    //    CommonFunction fns = new CommonFunction();
    //    //function fns = new function();
    //    try
    //    {

    //        string numbers = "1234567890";

    //        string numbercode = "91";

    //        if (countryid == "8")
    //        {
    //            numbercode = "966";
    //        }
    //        if (countryid == "2")
    //        {
    //            numbercode = "971";
    //        }
    //        if (countryid == "7")
    //        {
    //            numbercode = "212";
    //        }
    //        mobileno = numbercode + mobileno;

    //        var data1 = @"{";
    //        data1 += "\"message\":[{";

    //        data1 += "\"recipient_whatsapp\":\"" + mobileno + "\",";
    //        data1 += "\"message_type\":\"template\",";
    //        data1 += "\"recipient_type\":\"individual\",";
    //        data1 += "\"type_template\":[{";

    //        //data1 += "\"name\":\"add_new_lead\",";
    //        data1 += "\"name\":\"" + templateid + "\",";

    //        // data1 += "\"attributes\":[\"" + leadno + "\",\"" + leadfrom + "\",\"" + contactno + "\",\"" + emailid + "\",\"" + company + "\",\"" + message + "\"],";
    //        data1 += template;

    //        data1 += "\"language\":{";
    //        data1 += "\"locale\":\"en\",";
    //        data1 += "\"policy\":\"deterministic\"";

    //        data1 += "}";
    //        data1 += "}";
    //        data1 += "]}";

    //        data1 += "]}";
    //        SaveLogText("data1:" + data1);
    //        string apiUrl = ConfigurationManager.AppSettings["Whatsupurl"].ToString();
    //        string apiKey = ConfigurationManager.AppSettings["WhatsupApiKey"].ToString();
    //        ServicePointManager.Expect100Continue = true;
    //        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

    //        var httpRequest1 = (HttpWebRequest)WebRequest.Create(apiUrl);

    //        httpRequest1.Method = "POST";
    //        httpRequest1.Headers.Add("Authorization", "Bearer " + apiKey);
    //        httpRequest1.ContentType = "application/json";
    //        httpRequest1.Accept = "application/json";


    //        using (var streamWriter = new StreamWriter(httpRequest1.GetRequestStream()))
    //        {
    //            streamWriter.Write(data1);
    //        }
    //        var httpResponse1 = (HttpWebResponse)httpRequest1.GetResponse();
    //        using (var streamReader = new StreamReader(httpResponse1.GetResponseStream()))
    //        {
    //            var result = streamReader.ReadToEnd().Trim();
    //            dynamic data11 = JObject.Parse(result);
    //            //Response.Write(data11.messageid);
    //            if (data11.messageid != "")
    //            {
    //                var msgdataid = data11.data;
    //                string msgid = msgdataid.id;
    //                string msgstr = data11.message;
    //                string msgsts = data11.status;
    //                //SaveLogText("msgid " + msgid + " Status:" + msgsts + " Message" + msgstr);
    //            }
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        // SaveLogText("ex:" + ex.Message);
    //    }



    //}

    public void sendnotificationlead(int rid, int typeid, int ntfor)
    {

        try
        {
            string strsql = "select * from tbl_notificationtemplates where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and typeid='" + typeid + "' and ntfor='" + ntfor + "' and status=1 and archive=0";
            DataSet dsnot = da.GetDataSetInline(strsql);
            if (dsnot.Tables[0].Rows.Count > 0)
            {
                string isscreenpopup = dsnot.Tables[0].Rows[0]["isscreenpoppup"].ToString();
                string isscreenpopup_frz = dsnot.Tables[0].Rows[0]["isscreenfreez"].ToString();
                string notificationtext = dsnot.Tables[0].Rows[0]["temptext"].ToString();

                string isnotification = dsnot.Tables[0].Rows[0]["isnotification"].ToString();
                string isemail = dsnot.Tables[0].Rows[0]["isemail"].ToString();
                string issms = dsnot.Tables[0].Rows[0]["issms"].ToString();
                string isscreenpoppup = dsnot.Tables[0].Rows[0]["isscreenpoppup"].ToString();
                string isscreenfreez = dsnot.Tables[0].Rows[0]["isscreenfreez"].ToString();


                string whatsapptemplateid = dsnot.Tables[0].Rows[0]["Whatsapptemplateid"].ToString();
                string Whatsapptemplateprm = dsnot.Tables[0].Rows[0]["Whatsapptemplateprm"].ToString();
                string Whatsapptemplate = dsnot.Tables[0].Rows[0]["Whatsapptemplate"].ToString();
                string strtemplate_wt = "";
                string iswatsapp = dsnot.Tables[0].Rows[0]["iswhatsapp"].ToString();
                string istelegram = dsnot.Tables[0].Rows[0]["istelegram"].ToString();
                string uid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();

                string ownerid = "0";

                string telegramchatid = "";
                string wtsapp_no = "";
                string wtsapp_cno = "";
                string wtsapp_email = "";
                string wtsapp_company = "";
                string wtsapp_leadno = "";
                string wtsapp_from = "";
                string wtsapp_req = "";
                string countryid = "0";

                string LeadDate = "&nbsp;";
                string LeadNo = "&nbsp;";
                string LeadName = "&nbsp;";
                string FirstName = "&nbsp;";
                string LastName = "&nbsp;";
                string Orgname = "&nbsp;";
                string OrgUser = "&nbsp;";
                string OrgPhone = "&nbsp;";
                string OrgWebsite = "&nbsp;";
                string refno = "&nbsp;";






                string sqldetails = @"select (tbl_mainleads.FirstName+' '+tbl_mainleads.LastName)LeadName ,tbl_mainleads.FirstName,tbl_mainleads.lastName,Title,company,PhoneNo,EmailId,Website,DisplayName Employee,tbl_leadstatus.StatusName,tbl_mainleads.cdate,tbl_mainleads.id,tbl_mainleads.LeadStatus,tbl_mainleads.LeadNo,tbl_mainleads.Descriptions,ISNULL(tbl_employee.country_Id,0) countryid
,(case when tbl_employee.mobile_no is null then tbl_employee.MobileNumber when tbl_employee.mobile_no ='' then tbl_employee.MobileNumber else tbl_employee.mobile_no end) mobilenumber,isnull(tbl_mainleads.OwnerName,0) ownerid,TeleGramBoatId from tbl_mainleads  
                            inner join tbl_employee on tbl_mainleads.OwnerName = tbl_employee.id
                            left join tbl_leadstatus on tbl_mainleads.LeadStatus = tbl_leadstatus.id
                            where tbl_mainleads.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and tbl_mainleads.id='" + rid + "' and tbl_mainleads.status=1 and tbl_mainleads.archive=0";
                //SaveLogText("sqldetails: " + sqldetails);
                DataSet dsdetails = da.GetDataSetInline(sqldetails);
                if (dsdetails.Tables[0].Rows.Count > 0)
                {
                    ownerid = dsdetails.Tables[0].Rows[0]["ownerid"].ToString();
                    try
                    {
                        countryid = getcontry();
                    }
                    catch
                    { }
                    LeadDate = Cmf.dateConvert1(dsdetails.Tables[0].Rows[0]["cdate"].ToString()).ToString();
                    LeadNo = dsdetails.Tables[0].Rows[0]["LeadNo"].ToString();
                    LeadName = dsdetails.Tables[0].Rows[0]["LeadName"].ToString();
                    FirstName = dsdetails.Tables[0].Rows[0]["FirstName"].ToString();
                    LastName = dsdetails.Tables[0].Rows[0]["lastName"].ToString();
                    Orgname = dsdetails.Tables[0].Rows[0]["company"].ToString();
                    OrgUser = dsdetails.Tables[0].Rows[0]["company"].ToString();
                    OrgPhone = dsdetails.Tables[0].Rows[0]["PhoneNo"].ToString();
                    OrgWebsite = dsdetails.Tables[0].Rows[0]["LeadNo"].ToString();

                    wtsapp_cno = dsdetails.Tables[0].Rows[0]["PhoneNo"].ToString();
                    wtsapp_email = dsdetails.Tables[0].Rows[0]["EmailId"].ToString();
                    wtsapp_company = dsdetails.Tables[0].Rows[0]["company"].ToString();
                    wtsapp_no = dsdetails.Tables[0].Rows[0]["mobilenumber"].ToString();
                    wtsapp_leadno = LeadNo;
                    wtsapp_from = LeadName;
                    wtsapp_req = dsdetails.Tables[0].Rows[0]["Descriptions"].ToString();
                    telegramchatid = dsdetails.Tables[0].Rows[0]["TeleGramBoatId"].ToString();


                }

                string nuid = "0";
                string iscustomer = "0";
                string isview = "0";

                notificationtext = notificationtext.Replace("{####LeadDate####}", LeadDate
                   ).Replace("{####LeadNumber####}", LeadNo
                   ).Replace("{####RefNumber####}", refno
                   ).Replace("{####CustomerName####}", LeadName
                   ).Replace("{####CustomerFirstName####}", FirstName
                   ).Replace("{####CustomerLastName####}", LastName
                   ).Replace("{####OrganizationName####}", Orgname
                   ).Replace("{####OrganizationUser####}", OrgUser
                   ).Replace("{####OrganizationPhone####}", OrgPhone
                  ).Replace("{####OrganizationWebsite####}", OrgWebsite
                  ).Replace("{####OrganizationEmail####}", wtsapp_email
                  ).Replace("{####Descriptions####}", wtsapp_req

                  );
                if (Whatsapptemplateprm != "")
                {
                    string[] arprm = Whatsapptemplateprm.Split(',');
                    for (int i = 0; i < arprm.Length; i++)
                    {
                        //if (notificationtext.Contains(arprm[i]))
                        //{
                        if (arprm[i] == "LeadDate")
                        {
                            Whatsapptemplateprm = Whatsapptemplateprm.Replace("LeadDate", Cmf.dateConvert1(dsdetails.Tables[0].Rows[0]["cdate"].ToString()));


                        }
                        else if (arprm[i] == "LeadNumber")
                        {
                            Whatsapptemplateprm = Whatsapptemplateprm.Replace("LeadNumber", wtsapp_leadno);


                        }
                        else if (arprm[i] == "CustomerName")
                        {
                            Whatsapptemplateprm = Whatsapptemplateprm.Replace("CustomerName", LeadName);


                        }
                        else if (arprm[i] == "CustomerFirstName")
                        {
                            Whatsapptemplateprm = Whatsapptemplateprm.Replace("CustomerFirstName", FirstName);


                        }
                        else if (arprm[i] == "CustomerLastName")
                        {
                            Whatsapptemplateprm = Whatsapptemplateprm.Replace("CustomerLastName", LastName);


                        }
                        else if (arprm[i] == "OrganizationName")
                        {

                            if (wtsapp_company == "")
                            {
                                wtsapp_company = "No Company";
                            }
                            Whatsapptemplateprm = Whatsapptemplateprm.Replace("OrganizationName", wtsapp_company);

                        }
                        else if (arprm[i] == "OrganizationUser")
                        {
                            if (wtsapp_company == "")
                            {
                                wtsapp_company = "No Company";
                            }
                            Whatsapptemplateprm = Whatsapptemplateprm.Replace("OrganizationUser", wtsapp_company);

                        }
                        else if (arprm[i] == "OrganizationPhone")
                        {
                            if (OrgPhone == "")
                            {
                                OrgPhone = "No Contact No";
                            }
                            Whatsapptemplateprm = Whatsapptemplateprm.Replace("OrganizationPhone", OrgPhone);

                        }
                        else if (arprm[i] == "OrganizationEmail")
                        {
                            if (wtsapp_email == "")
                            {
                                wtsapp_email = "No Mail Id";
                            }
                            Whatsapptemplateprm = Whatsapptemplateprm.Replace("OrganizationEmail", wtsapp_email);

                        }
                        else if (arprm[i] == "OrganizationWebsite")
                        {
                            if (OrgWebsite == "")
                            {
                                OrgWebsite = "No";
                            }
                            Whatsapptemplateprm = Whatsapptemplateprm.Replace("OrganizationWebsite", OrgWebsite);

                        }
                        else if (arprm[i] == "Descriptions")
                        {
                            if (wtsapp_req == "")
                            {
                                wtsapp_req = "No Descriptions";
                            }
                            Whatsapptemplateprm = Whatsapptemplateprm.Replace("Descriptions", wtsapp_req);

                        }
                        //}
                    }

                }
                char[] cha = { ',' };
                char[] cha1 = { '~' };

                string strsqlitem = "select * from tbl_notificationsetupemp where type='" + typeid + "'  and ntfor='" + ntfor + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  order by aLevel ";
                DataSet dsadd = da.GetDataSetInline(strsqlitem);
                if (dsadd.Tables[0].Rows.Count > 0)
                {
                    functions fnsm = new functions();
                    string emailids = "";
                    string ewatsappno = "";
                    string etelegram = "";

                    for (int i = 0; i < dsadd.Tables[0].Rows.Count; i++)
                    {

                        string[] arr = dsadd.Tables[0].Rows[i]["hdnempids"].ToString().Split(cha, StringSplitOptions.None);

                        for (int j = 0; j < arr.Length; j++)
                        {
                            nuid = "0";
                            iscustomer = "0";
                            ewatsappno = "";
                            etelegram = "";
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
                                        if (arrlist[0].ToString() == "3")
                                        {
                                            iscustomer = "1";
                                            nuid = rid.ToString();
                                        }
                                        if (arrlist[0].ToString() == "4")
                                        {
                                            if (ownerid != HttpContext.Current.Request.Cookies["user"]["user_id"].ToString())
                                            {
                                                nuid = ownerid;
                                            }

                                        }
                                    }
                                    else
                                    {
                                        nuid = arrlist[0].ToString();

                                    }

                                    if (nuid != "0")
                                    {
                                        string strtoemail = "";
                                        if (isnotification == "1")
                                        {

                                            //SqlParameter[] prmn = new SqlParameter[12];
                                            //prmn[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                                            //prmn[1] = new SqlParameter("@uid", uid);
                                            //prmn[2] = new SqlParameter("@nuid", nuid);
                                            //prmn[3] = new SqlParameter("@ndatetime", da.getcdatetime());
                                            //prmn[4] = new SqlParameter("@notificationtext", notificationtext);
                                            //prmn[5] = new SqlParameter("@rid", rid);
                                            //prmn[6] = new SqlParameter("@typeid", typeid);
                                            //prmn[7] = new SqlParameter("@isnew", "1");
                                            //prmn[8] = new SqlParameter("@isread", "0");
                                            //prmn[9] = new SqlParameter("@isreaddate", DBNull.Value);
                                            //prmn[10] = new SqlParameter("@isview", isview);
                                            //prmn[11] = new SqlParameter("@ntfor", ntfor);
                                            SqlParameter[] prmn = new SqlParameter[14];
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
                                            if (isscreenpopup == "1")
                                            {
                                                prmn[12] = new SqlParameter("@isscreenpoppup", isscreenpopup);
                                            }
                                            else
                                            {
                                                prmn[12] = new SqlParameter("@isscreenpoppup", DBNull.Value);
                                            }
                                            if (isscreenpopup_frz == "1")
                                            {
                                                prmn[13] = new SqlParameter("@isscreenfreez", isscreenpopup_frz);
                                            }
                                            else
                                            {
                                                prmn[13] = new SqlParameter("@isscreenfreez", DBNull.Value);
                                            }
                                            da.executeDMLQuery("tbl_pnotificationdetailsinsert", prmn);
                                        }
                                        if (issms == "1")
                                        {

                                        }
                                        if (iswatsapp == "1")
                                        {
                                            if (iscustomer == "1")
                                            {
                                                ewatsappno = wtsapp_cno;
                                            }
                                            else
                                            {
                                                ewatsappno = getwatsappId(nuid);
                                            }
                                            if (ewatsappno != "")
                                            {

                                                string WhatsAppConfiguredWithCompany = "";
                                                string APIKey = "";
                                                string APIURL = "";
                                                string CampaignName = "";
                                                string strsqlchk = "select * from tbl_WhatsAppconfiguration_crm where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and status=1 and archive=0";
                                                DataSet Dschk = da.GetDataSetInline(strsqlchk);
                                                if (Dschk.Tables[0].Rows.Count > 0)
                                                {
                                                    string RegistrationID = HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString();
                                                    setnotification_whatsapp(rid.ToString(), RegistrationID, ewatsappno, whatsapptemplateid, Whatsapptemplateprm);
                                                    //setnotification_whatsapp(ewatsappno, countryid, whatsapptemplateid, strtemplate_wt);

                                                }




                                            }
                                        }
                                        if (istelegram == "1")
                                        {
                                            if (iscustomer == "1")
                                            {
                                                etelegram = telegramchatid;
                                            }
                                            else
                                            {
                                                etelegram = gettelegramId(nuid);
                                            }
                                            if (etelegram != "")
                                            {
                                                sendTeleGramMessage(etelegram, notificationtext);
                                            }

                                        }
                                        if (isemail == "1")
                                        {

                                            try
                                            {
                                                if (iscustomer == "1")
                                                {
                                                    strtoemail = wtsapp_email;
                                                }
                                                else
                                                {
                                                    strtoemail = fnsm.getEmpemailById(nuid);
                                                }

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
                            sub = "Lead Notification by " + fnsm.getEmpNameById(uid);
                            body = notificationtext;
                            string stsignature = "";
                            //and tbl_gmailconfiguration_crm.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'
                            string strsqlchk = "select tbl_gmailconfiguration_crm.*,tbl_employee.Displayname from tbl_gmailconfiguration_crm inner join tbl_employee on tbl_gmailconfiguration_crm.uid=tbl_employee.id where tbl_gmailconfiguration_crm.uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"] + "'   and  tbl_gmailconfiguration_crm.status=1 and tbl_gmailconfiguration_crm.archive=0 and configtype='1'";
                            DataSet Dschk = da.GetDataSetInline(strsqlchk);
                            if (Dschk.Tables[0].Rows.Count > 0)
                            {
                                stsignature = Dschk.Tables[0].Rows[0]["txtSignature"].ToString();
                                if (stsignature != "")
                                {
                                    body += "<br>" + stsignature;
                                }
                                //SaveLogText("send from :" + Dschk.Tables[0].Rows[0]["cfgmailid"].ToString());
                                from = Dschk.Tables[0].Rows[0]["cfgmailid"].ToString();
                                if (to != string.Empty)
                                {
                                    sendmailnew_common(rid.ToString(), Dschk.Tables[0].Rows[0]["tokenid"].ToString(), Dschk.Tables[0].Rows[0]["refreshtokenid"].ToString(), to, from, cc, bcc, sub, body, filesdata, "1", "1", 1);
                                }
                            }
                            else
                            {
                                //tbl_gmailconfiguration_crm.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'
                                strsqlchk = "select tbl_gmailconfiguration_crm.*,tbl_employee.Displayname from tbl_gmailconfiguration_crm inner join tbl_employee on tbl_gmailconfiguration_crm.uid=tbl_employee.id where   tbl_gmailconfiguration_crm.uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"] + "' and  tbl_gmailconfiguration_crm.status=1 and tbl_gmailconfiguration_crm.archive=0 and configtype='2'";
                                Dschk = da.GetDataSetInline(strsqlchk);
                                if (Dschk.Tables[0].Rows.Count > 0)
                                {
                                    stsignature = Dschk.Tables[0].Rows[0]["txtSignature"].ToString();
                                    if (stsignature != "")
                                    {
                                        body += "<br>" + stsignature;
                                    }
                                    //SaveLogText("send from :" + Dschk.Tables[0].Rows[0]["cfgmailid"].ToString());
                                    from = Dschk.Tables[0].Rows[0]["cfgmailid"].ToString();
                                    if (to != string.Empty)
                                    {
                                        sendmailnew_common(rid.ToString(), Dschk.Tables[0].Rows[0]["tokenid"].ToString(), Dschk.Tables[0].Rows[0]["refreshtokenid"].ToString(), to, from, cc, bcc, sub, body, filesdata, "1", "1", 2);
                                    }
                                }
                                else
                                {
                                    if (to != string.Empty)
                                    {
                                        try
                                        {
                                            //sm.SendMailtoUser(to, from, cc, bcc, sub, body, filesdata);
                                        }
                                        catch
                                        { }
                                    }

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
        catch
        {

        }








    }

    public void sendnotificationleadtoclient(int rid, int typeid, int ntfor)
    {

        try
        {
            string strsql = "select * from tbl_notificationtemplates where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and typeid='" + typeid + "' and ntfor='" + ntfor + "' and status=1 and archive=0";
            DataSet dsnot = da.GetDataSetInline(strsql);
            if (dsnot.Tables[0].Rows.Count > 0)
            {
                string isscreenpopup = dsnot.Tables[0].Rows[0]["isscreenpoppup"].ToString();
                string isscreenpopup_frz = dsnot.Tables[0].Rows[0]["isscreenfreez"].ToString();
                string notificationtext = dsnot.Tables[0].Rows[0]["temptext"].ToString();

                string isnotification = dsnot.Tables[0].Rows[0]["isnotification"].ToString();
                string isemail = dsnot.Tables[0].Rows[0]["isemail"].ToString();
                string issms = dsnot.Tables[0].Rows[0]["issms"].ToString();
                string isscreenpoppup = dsnot.Tables[0].Rows[0]["isscreenpoppup"].ToString();
                string isscreenfreez = dsnot.Tables[0].Rows[0]["isscreenfreez"].ToString();
                string whatsapptemplateid = dsnot.Tables[0].Rows[0]["Whatsapptemplateid"].ToString();
                string Whatsapptemplateprm = dsnot.Tables[0].Rows[0]["Whatsapptemplateprm"].ToString();
                string Whatsapptemplate = dsnot.Tables[0].Rows[0]["Whatsapptemplate"].ToString();
                ///string strtemplate_wt = "";
                string iswatsapp = dsnot.Tables[0].Rows[0]["iswhatsapp"].ToString();
                string istelegram = dsnot.Tables[0].Rows[0]["istelegram"].ToString();
                string uid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();

                string TemplateType = dsnot.Tables[0].Rows[0]["TemplateType"].ToString();
                string TemplateMediaType = dsnot.Tables[0].Rows[0]["TemplateMediaType"].ToString();
                string uplaodfilename = dsnot.Tables[0].Rows[0]["uplaodfilename"].ToString();


                string ownerid = "0";

                string telegramchatid = "";
                string wtsapp_no = "";
                string wtsapp_cno = "";
                string wtsapp_email = "";
                string wtsapp_company = "";
                string wtsapp_leadno = "";
                string wtsapp_from = "";
                string wtsapp_req = "";
                string countryid = "0";
                string LeadDate = "&nbsp;";
                string LeadNo = "&nbsp;";
                string LeadName = "&nbsp;";
                string FirstName = "&nbsp;";
                string LastName = "&nbsp;";
                string Orgname = "&nbsp;";
                string OrgUser = "&nbsp;";
                string OrgPhone = "&nbsp;";
                string OrgWebsite = "&nbsp;";
                string refno = "&nbsp;";
                string EmployeeName = "&nbsp;";
                string EmployeeContactNumber = "&nbsp;";



                string sqldetails = @"select (tbl_mainleads.FirstName+' '+tbl_mainleads.LastName)LeadName ,tbl_mainleads.FirstName,tbl_mainleads.lastName,Title,company,PhoneNo,EmailId,Website,DisplayName Employee,tbl_leadstatus.StatusName,tbl_mainleads.cdate,tbl_mainleads.id,tbl_mainleads.LeadStatus,tbl_mainleads.LeadNo,tbl_mainleads.Descriptions,ISNULL(tbl_employee.country_Id,0) countryid
,(case when tbl_employee.mobile_no is null then tbl_employee.MobileNumber when tbl_employee.mobile_no ='' then tbl_employee.MobileNumber else tbl_employee.mobile_no end) mobilenumber,isnull(tbl_mainleads.OwnerName,0) ownerid,TeleGramBoatId from tbl_mainleads  
                            inner join tbl_employee on tbl_mainleads.OwnerName = tbl_employee.id
                            left join tbl_leadstatus on tbl_mainleads.LeadStatus = tbl_leadstatus.id
                            where tbl_mainleads.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and tbl_mainleads.id='" + rid + "' and tbl_mainleads.status=1 and tbl_mainleads.archive=0";
                //SaveLogText("sqldetails: " + sqldetails);
                DataSet dsdetails = da.GetDataSetInline(sqldetails);
                if (dsdetails.Tables[0].Rows.Count > 0)
                {
                    ownerid = dsdetails.Tables[0].Rows[0]["ownerid"].ToString();
                    try
                    {
                        countryid = getcontry();
                    }
                    catch
                    { }
                    LeadDate = Cmf.dateConvert1(dsdetails.Tables[0].Rows[0]["cdate"].ToString()).ToString();
                    LeadNo = dsdetails.Tables[0].Rows[0]["LeadNo"].ToString();
                    LeadName = dsdetails.Tables[0].Rows[0]["LeadName"].ToString();
                    FirstName = dsdetails.Tables[0].Rows[0]["FirstName"].ToString();
                    LastName = dsdetails.Tables[0].Rows[0]["lastName"].ToString();
                    Orgname = dsdetails.Tables[0].Rows[0]["company"].ToString();
                    OrgUser = dsdetails.Tables[0].Rows[0]["company"].ToString();
                    OrgPhone = dsdetails.Tables[0].Rows[0]["PhoneNo"].ToString();
                    OrgWebsite = dsdetails.Tables[0].Rows[0]["LeadNo"].ToString();
                    wtsapp_cno = dsdetails.Tables[0].Rows[0]["PhoneNo"].ToString();
                    wtsapp_email = dsdetails.Tables[0].Rows[0]["EmailId"].ToString();
                    wtsapp_company = dsdetails.Tables[0].Rows[0]["company"].ToString();
                    EmployeeName = dsdetails.Tables[0].Rows[0]["Employee"].ToString();
                    EmployeeContactNumber = dsdetails.Tables[0].Rows[0]["mobilenumber"].ToString();
                    
                    wtsapp_leadno = LeadNo;
                    wtsapp_from = LeadName;
                    wtsapp_req = dsdetails.Tables[0].Rows[0]["Descriptions"].ToString();
                    telegramchatid = dsdetails.Tables[0].Rows[0]["TeleGramBoatId"].ToString();


                }
                
                notificationtext = notificationtext.Replace("{####CustomerName####}", FirstName
                       ).Replace("{####EmployeeName####}", EmployeeName
                       ).Replace("{####EmployeeContactNumber####}", EmployeeContactNumber
                       ).Replace("{####MeetingDate####}", ""
                       ).Replace("{####MeetingTime####}", ""
                       ).Replace("{####MeetingLink####}", ""
                       ).Replace("{####EmployeeRole####}", ""///Main Till here
                       ).Replace("{####OrganizationPhone####}", OrgPhone
                      ).Replace("{####OrganizationWebsite####}", OrgWebsite
                      ).Replace("{####OrganizationEmail####}", wtsapp_email
                      ).Replace("{####Descriptions####}", wtsapp_req

                      );

                if (Whatsapptemplateprm != "")
                {
                    string[] arprm = Whatsapptemplateprm.Split(',');
                    for (int i = 0; i < arprm.Length; i++)
                    {
                        //if (notificationtext.Contains(arprm[i]))
                        //{
                        if (arprm[i] == "CustomerName")
                        {
                            /// Whatsapptemplateprm = Whatsapptemplateprm.Replace("LeadDate", Cmf.dateConvert1(dsdetails.Tables[0].Rows[0]["cdate"].ToString()));

                            Whatsapptemplateprm = Whatsapptemplateprm.Replace("CustomerName", dsdetails.Tables[0].Rows[0]["FirstName"].ToString());


                        }
                        else if (arprm[i] == "EmployeeName")
                        {
                            Whatsapptemplateprm = Whatsapptemplateprm.Replace("EmployeeName", EmployeeName);

                        }
                        else if (arprm[i] == "EmployeeContactNumber")
                        {
                            Whatsapptemplateprm = Whatsapptemplateprm.Replace("EmployeeContactNumber", EmployeeContactNumber);

                        }


                        else if (arprm[i] == "OrganizationName")
                        {

                            if (wtsapp_company == "")
                            {
                                wtsapp_company = "No Company";
                            }
                            Whatsapptemplateprm = Whatsapptemplateprm.Replace("OrganizationName", wtsapp_company);

                        }
                        else if (arprm[i] == "OrganizationUser")
                        {
                            if (wtsapp_company == "")
                            {
                                wtsapp_company = "No Company";
                            }
                            Whatsapptemplateprm = Whatsapptemplateprm.Replace("OrganizationUser", wtsapp_company);

                        }
                        else if (arprm[i] == "OrganizationPhone")
                        {
                            if (OrgPhone == "")
                            {
                                OrgPhone = "No Contact No";
                            }
                            Whatsapptemplateprm = Whatsapptemplateprm.Replace("OrganizationPhone", OrgPhone);

                        }
                        else if (arprm[i] == "OrganizationEmail")
                        {
                            if (wtsapp_email == "")
                            {
                                wtsapp_email = "No Mail Id";
                            }
                            Whatsapptemplateprm = Whatsapptemplateprm.Replace("OrganizationEmail", wtsapp_email);

                        }
                        else if (arprm[i] == "OrganizationWebsite")
                        {
                            if (OrgWebsite == "")
                            {
                                OrgWebsite = "No";
                            }
                            Whatsapptemplateprm = Whatsapptemplateprm.Replace("OrganizationWebsite", OrgWebsite);

                        }
                        else if (arprm[i] == "Descriptions")
                        {
                            if (wtsapp_req == "")
                            {
                                wtsapp_req = "No Descriptions";
                            }
                            Whatsapptemplateprm = Whatsapptemplateprm.Replace("Descriptions", wtsapp_req);

                        }
                        //}
                    }

                }
                char[] cha = { ',' };
                char[] cha1 = { '~' };

                string strtoemail = "";
                functions fnsm = new functions();
                string emailids = "";
                if (iswatsapp == "1")
                {

                    if (OrgPhone != "")
                    {
                        ////SaveLogText("method called" + OrgPhone);

                        string strsqlchk = "select * from tbl_WhatsAppconfiguration_crm where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0";
                        DataSet Dschk = da.GetDataSetInline(strsqlchk);
                        if (Dschk.Tables[0].Rows.Count > 0)
                        {
                           
                            string RegistrationID = HttpContext.Current.Request.Cookies["cadmin"]["id"];
                            ///SaveLogText("whatsapp installed" + RegistrationID);
                            SetWelcomeNotification_toclient(rid.ToString(), RegistrationID, whatsapptemplateid, TemplateType, TemplateMediaType, uplaodfilename, FirstName, EmployeeName, EmployeeContactNumber, OrgPhone);


                        }

                    }
                }

                if (isemail == "1")
                {
                    ///SaveLogText("CALLED IS EMAIL");
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

                        from = fnsm.getEmpemailById(ownerid);
                        to = wtsapp_email;
                        cc = "";
                        bcc = "";

                        sub = "We’ve Received Your Request – Welcome Aboard!";
                        body = notificationtext;
                        string stsignature = "";

                        string leadDetail = "select tbl_gmailconfiguration_crm.*,tbl_employee.Displayname from tbl_gmailconfiguration_crm inner join tbl_employee on tbl_gmailconfiguration_crm.uid=tbl_employee.id where tbl_gmailconfiguration_crm.uid='" + uid + "' and tbl_gmailconfiguration_crm.status=1 and tbl_gmailconfiguration_crm.archive=0";
                        DataSet Dschk = da.GetDataSetInline(leadDetail);
                        if (Dschk.Tables[0].Rows.Count > 0)
                        {


                            string configtype = Dschk.Tables[0].Rows[0]["configtype"].ToString();
                            if (configtype == "1" || configtype == "2")
                            {
                                stsignature = Dschk.Tables[0].Rows[0]["txtSignature"].ToString();
                                if (stsignature != "")
                                {
                                    body += "<br>" + stsignature;
                                }
                                //SaveLogText("send from :" + Dschk.Tables[0].Rows[0]["cfgmailid"].ToString());
                                from = Dschk.Tables[0].Rows[0]["cfgmailid"].ToString();
                                if (to != string.Empty)
                                {
                                    sendmailnew_common(rid.ToString(), Dschk.Tables[0].Rows[0]["tokenid"].ToString(), Dschk.Tables[0].Rows[0]["refreshtokenid"].ToString(), to, from, cc, bcc, sub, body, filesdata, "1", "1", 1);
                                }


                            }


                            else
                            {

                                stsignature = Dschk.Tables[0].Rows[0]["txtSignature"].ToString();
                                if (stsignature != "")
                                {
                                    body += "<br>" + stsignature;
                                }

                                string smtpServer = "";
                                string port = "";
                                string Password = "";

                               //// SaveLogText("Other email config for UID" + uid);
                                string othermailconf = "select * from tbl_EmailConfigurationSettings_Crm where uid ='" + uid + "' and status =1 and archive=0";
                                DataSet dt_othermailconf = da.GetDataSetInline(othermailconf);
                                if (dt_othermailconf.Tables[0].Rows.Count > 0)
                                {

                                    //SaveLogText("User detail got");
                                    from = dt_othermailconf.Tables[0].Rows[0]["txtEmail"].ToString();
                                    smtpServer = dt_othermailconf.Tables[0].Rows[0]["txtOutgoingServer"].ToString();
                                    port = dt_othermailconf.Tables[0].Rows[0]["outgoingPort"].ToString();
                                    Password = dt_othermailconf.Tables[0].Rows[0]["txtPassword"].ToString();
                                }
                                body = EscapeQuotes(body.Trim());
                                ////content = EscapeQuotes(content.Trim());
                                if (to != string.Empty)
                                {
                                    sendmailnew_otherconfig(to, from, sub, body, filesdata, "1", smtpServer, port, Password);
                                }

                            }
                        }
                    }

                    catch (Exception ex)
                    {
                        ///Logger.FaceBookLogError($"error in when isemail=1 while we are sending the welcome mail to client : {ex.Message}");
                    }
                }

            }
        }
        catch
        {

        }


    }
    protected void sendmailnew_otherconfig(string To, string from, string Subject, string Message, string[] files, string status, string smtpserver, string port, string password)
    {

        string CC = "";
        string BCC = "";
        string successemail = "";
        string attachfiles = "";
        List<string> filesurl = new List<string>();
        int NumberofFiles = files.Length;
        //SaveLogText("NumberofFiles" + NumberofFiles);
        for (int i = 0; i < files.Length; i++)
        {

            string imagePath = files[i]; // Path to your image file
            if (attachfiles != "")
            {
                //attachfiles += "####" + ConvertImageToBase64(imagePath);
                attachfiles += imagePath + ",";
                filesurl.Add(imagePath);
            }
            else
            {
                //attachfiles= ConvertImageToBase64(imagePath);
                if (NumberofFiles > 1)
                {
                    attachfiles += imagePath + ",";
                    filesurl.Add(imagePath);
                }
                else
                {
                    attachfiles += imagePath;
                    filesurl.Add(imagePath);
                }
            }
        }

        // Remove the last comma if it exists
        if (attachfiles.EndsWith(","))
        {
            attachfiles = attachfiles.Substring(0, attachfiles.Length - 1);
        }

        string apiUrl = ConfigurationManager.AppSettings["gmailapi"].ToString() + "/api/values/SendOtherEmail";
        var httpRequest1 = (HttpWebRequest)WebRequest.Create(apiUrl);

        httpRequest1.Method = "POST";

        httpRequest1.Accept = "application/json";
        httpRequest1.ContentType = "application/json";

        var data1 = @"{";
        data1 += " \"smtpServer\":\"" + smtpserver + "\",";
        data1 += " \"port\":\"" + port + "\",";
        data1 += " \"Password\":\"" + password + "\",";
        data1 += " \"From\":\"" + from + "\",";
        data1 += " \"To\":\"" + To + "\",";
        data1 += " \"Subject\":\"" + Subject.Trim() + "\",";
        data1 += " \"Body\":\"" + Message.Trim() + "\",";
        data1 += " \"CC\":\"" + CC + "\",";
        data1 += " \"BCC\":\"" + BCC + "\", ";
        data1 += " \"Attachments\":" + JsonConvert.SerializeObject(filesurl) + " ";
        data1 += "}";

        // SaveLogText(data1);

        ////SaveLogText("Other mail body param" + data1);
        using (var streamWriter = new StreamWriter(httpRequest1.GetRequestStream()))
        {
            streamWriter.Write(data1);
        }

        try
        {
            var httpResponse1 = (HttpWebResponse)httpRequest1.GetResponse();
            using (var streamReader = new StreamReader(httpResponse1.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd().Trim();
                ///SaveLogText(result);
                //response res = new response();
                dynamic data11 = JObject.Parse(result);

                //Response.Write(data11.messageid);
                if (data11.messageid != "" && data11.messageid != "0" && data11.status != "-1")
                {
                    successemail = "1";
                    string msgid = data11.messageid;
                    string otherlookreturnid = data11.status;
                    string msgstatus = status;

                }
                else
                {
                    successemail = "-1";
                }
            }

        }

        catch (Exception ex)
        {
            successemail = "-1";
            ///Logger.FaceBookLogError($"error when sending the welcome email to client: {ex.Message}");
        }


    }
    protected static void SetWelcomeNotification_toclient(string rid = "", string regid = "", string TemplateID = "", string TemplateType = "", string TemplateMediaType = "", string uplaodfilename = "", string CustomerName = "", string AssignedTo = "", string AssignedToContactNumber = "", string CustomerMobileNumber = "")
    {
        ////SaveLogText("SetWelcomeNotification_toclient methid called");
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        string apiUrl = ConfigurationManager.AppSettings["gmailapi"].ToString() + "/api/WhatApp/sendWhatsAppToclient";
        var httpRequest1 = (HttpWebRequest)WebRequest.Create(apiUrl);
        ///mobileno = "8920951626,7534060586";
        httpRequest1.Method = "POST";
        httpRequest1.Accept = "application/json";
        httpRequest1.ContentType = "application/json";
        var data1 = @"{";
        data1 += " \"LeadID\":\"" + rid + "\",";
        data1 += " \"RegID\":\"" + regid + "\",";
        data1 += " \"TemplateID\":\"" + TemplateID + "\",";
        data1 += " \"mobileno\":\"" + CustomerMobileNumber + "\",";
        data1 += " \"TemplateType\":\"" + TemplateType + "\",";
        data1 += " \"TemplateMediaType\":\"" + TemplateMediaType + "\",";
        data1 += " \"uplaodfilename\":\"" + uplaodfilename + "\",";
        data1 += " \"CustomerName\":\"" + CustomerName + "\",";
        data1 += " \"AssignedTo\":\"" + AssignedTo + "\",";
        data1 += " \"AssignedToContactNumber\":\"" + AssignedToContactNumber + "\"";
        data1 += "}";

        ////SaveLogText("sending message to client: " + data1);
        using (var streamWriter = new StreamWriter(httpRequest1.GetRequestStream()))
        {
            streamWriter.Write(data1);
        }
        try
        {
            var httpResponse1 = (HttpWebResponse)httpRequest1.GetResponse();
            using (var streamReader = new StreamReader(httpResponse1.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd().Trim();
            }
        }
        catch (Exception ex)
        {
            SaveLogText("Error Find when sending the whatsapp message to client" + ex.Message);
        }

    }




    public void resendnotificationlead(int rid, int typeid, int ntfor, string douplicateid)
    {

        try
        {
            string strsql = "select * from tbl_notificationtemplates where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and  typeid='" + typeid + "' and ntfor='" + ntfor + "' and status=1 and archive=0";
            DataSet dsnot = da.GetDataSetInline(strsql);
            if (dsnot.Tables[0].Rows.Count > 0)
            {
                string isscreenpopup = dsnot.Tables[0].Rows[0]["isscreenpoppup"].ToString();
                string isscreenpopup_frz = dsnot.Tables[0].Rows[0]["isscreenfreez"].ToString();
                string notificationtext = dsnot.Tables[0].Rows[0]["temptext"].ToString();

                string isnotification = dsnot.Tables[0].Rows[0]["isnotification"].ToString();
                string isemail = dsnot.Tables[0].Rows[0]["isemail"].ToString();
                string issms = dsnot.Tables[0].Rows[0]["issms"].ToString();
                string isscreenpoppup = dsnot.Tables[0].Rows[0]["isscreenpoppup"].ToString();
                string isscreenfreez = dsnot.Tables[0].Rows[0]["isscreenfreez"].ToString();


                string whatsapptemplateid = dsnot.Tables[0].Rows[0]["Whatsapptemplateid"].ToString();
                string Whatsapptemplateprm = dsnot.Tables[0].Rows[0]["Whatsapptemplateprm"].ToString();
                string Whatsapptemplate = dsnot.Tables[0].Rows[0]["Whatsapptemplate"].ToString();
                string strtemplate_wt = "";
                string iswatsapp = dsnot.Tables[0].Rows[0]["iswhatsapp"].ToString();
                string istelegram = dsnot.Tables[0].Rows[0]["istelegram"].ToString();
                string uid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();

                string ownerid = "0";

                string telegramchatid = "";
                string wtsapp_no = "";
                string wtsapp_cno = "";
                string wtsapp_email = "";
                string wtsapp_company = "";
                string wtsapp_leadno = "";
                string wtsapp_from = "";
                string wtsapp_req = "";
                string countryid = "0";
                string LeadDate = "&nbsp;";
                string LeadNo = "&nbsp;";
                string LeadName = "&nbsp;";
                string FirstName = "&nbsp;";
                string LastName = "&nbsp;";
                string Orgname = "&nbsp;";
                string OrgUser = "&nbsp;";
                string OrgPhone = "&nbsp;";
                string OrgWebsite = "&nbsp;";
                string refno = "&nbsp;";






                string sqldetails = @"select (tbl_mainleads.FirstName+' '+tbl_mainleads.LastName)LeadName ,tbl_mainleads.FirstName,tbl_mainleads.lastName,Title,company,PhoneNo,EmailId,Website,DisplayName Employee,tbl_leadstatus.StatusName,tbl_mainleads.cdate,tbl_mainleads.id,tbl_mainleads.LeadStatus,tbl_mainleads.LeadNo,tbl_mainleads.Descriptions,ISNULL(tbl_employee.country_Id,0) countryid
,(case when tbl_employee.mobile_no is null then tbl_employee.MobileNumber when tbl_employee.mobile_no ='' then tbl_employee.MobileNumber else tbl_employee.mobile_no end) mobilenumber,isnull(tbl_mainleads.OwnerName,0) ownerid,TeleGramBoatId from tbl_mainleads  
                            inner join tbl_employee on tbl_mainleads.OwnerName = tbl_employee.id
                            left join tbl_leadstatus on tbl_mainleads.LeadStatus = tbl_leadstatus.id
                            where tbl_mainleads.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and tbl_mainleads.id='" + rid + "' and tbl_mainleads.status=1 and tbl_mainleads.archive=0";
                //SaveLogText("sqldetails: " + sqldetails);
                DataSet dsdetails = da.GetDataSetInline(sqldetails);
                if (dsdetails.Tables[0].Rows.Count > 0)
                {
                    ownerid = dsdetails.Tables[0].Rows[0]["ownerid"].ToString();
                    try
                    {
                        countryid = getcontry();
                    }
                    catch
                    { }
                    LeadDate = Cmf.dateConvert1(dsdetails.Tables[0].Rows[0]["cdate"].ToString()).ToString();
                    LeadNo = dsdetails.Tables[0].Rows[0]["LeadNo"].ToString();
                    LeadName = dsdetails.Tables[0].Rows[0]["LeadName"].ToString();
                    FirstName = dsdetails.Tables[0].Rows[0]["FirstName"].ToString();
                    LastName = dsdetails.Tables[0].Rows[0]["lastName"].ToString();
                    Orgname = dsdetails.Tables[0].Rows[0]["company"].ToString();
                    OrgUser = dsdetails.Tables[0].Rows[0]["company"].ToString();
                    OrgPhone = dsdetails.Tables[0].Rows[0]["PhoneNo"].ToString();
                    OrgWebsite = dsdetails.Tables[0].Rows[0]["LeadNo"].ToString();

                    wtsapp_cno = dsdetails.Tables[0].Rows[0]["PhoneNo"].ToString();
                    wtsapp_email = dsdetails.Tables[0].Rows[0]["EmailId"].ToString();
                    wtsapp_company = dsdetails.Tables[0].Rows[0]["company"].ToString();
                    wtsapp_no = dsdetails.Tables[0].Rows[0]["mobilenumber"].ToString();
                    wtsapp_leadno = LeadNo;
                    wtsapp_from = LeadName;
                    wtsapp_req = dsdetails.Tables[0].Rows[0]["Descriptions"].ToString();
                    telegramchatid = dsdetails.Tables[0].Rows[0]["TeleGramBoatId"].ToString();
                    string sqldetailsnew = @"select (tbl_mainleads.FirstName+' '+tbl_mainleads.LastName)LeadName ,tbl_mainleads.FirstName,tbl_mainleads.lastName,Title,company,PhoneNo,EmailId,Website,DisplayName Employee,tbl_leadstatus.StatusName,tbl_mainleads.cdate,tbl_mainleads.id,tbl_mainleads.LeadStatus,tbl_mainleads.LeadNo,tbl_mainleads.Descriptions,ISNULL(tbl_employee.country_Id,0) countryid
,(case when tbl_employee.mobile_no is null then tbl_employee.MobileNumber when tbl_employee.mobile_no ='' then tbl_employee.MobileNumber else tbl_employee.mobile_no end) mobilenumber,isnull(tbl_mainleads.OwnerName,0) ownerid,TeleGramBoatId from tbl_mainleads_douplicate tbl_mainleads  
                            inner join tbl_employee on tbl_mainleads.OwnerName = tbl_employee.id
                            left join tbl_leadstatus on tbl_mainleads.LeadStatus = tbl_leadstatus.id
                            where tbl_mainleads.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and tbl_mainleads.id='" + douplicateid + "' and tbl_mainleads.status=1 and tbl_mainleads.archive=0";

                    DataSet dsD = da.GetDataSetInline(sqldetailsnew);
                    if (dsD.Tables[0].Rows.Count > 0)
                    {
                        wtsapp_req = dsD.Tables[0].Rows[0]["Descriptions"].ToString();
                    }


                }

                string nuid = "0";
                string iscustomer = "0";
                string isview = "0";
                notificationtext = " Duplicate Lead <br/> " + notificationtext;
                notificationtext = notificationtext.Replace("{####LeadDate####}", LeadDate
                   ).Replace("{####LeadNumber####}", LeadNo
                   ).Replace("{####RefNumber####}", refno
                   ).Replace("{####CustomerName####}", LeadName
                   ).Replace("{####CustomerFirstName####}", FirstName
                   ).Replace("{####CustomerLastName####}", LastName
                   ).Replace("{####OrganizationName####}", Orgname
                   ).Replace("{####OrganizationUser####}", OrgUser
                   ).Replace("{####OrganizationPhone####}", OrgPhone
                  ).Replace("{####OrganizationWebsite####}", OrgWebsite
                  ).Replace("{####OrganizationEmail####}", wtsapp_email
                  ).Replace("{####Descriptions####}", wtsapp_req

                  );
                SaveLogText("notificationtext: " + notificationtext);
                if (Whatsapptemplateprm != "")
                {
                    string[] arprm = Whatsapptemplateprm.Split(',');
                    for (int i = 0; i < arprm.Length; i++)
                    {
                        //if (notificationtext.Contains(arprm[i]))
                        //{
                        if (arprm[i] == "LeadDate")
                        {
                            Whatsapptemplateprm = Whatsapptemplateprm.Replace("LeadDate", Cmf.dateConvert1(dsdetails.Tables[0].Rows[0]["cdate"].ToString()));


                        }
                        else if (arprm[i] == "LeadNumber")
                        {
                            Whatsapptemplateprm = Whatsapptemplateprm.Replace("LeadNumber", wtsapp_leadno);


                        }
                        else if (arprm[i] == "CustomerName")
                        {
                            Whatsapptemplateprm = Whatsapptemplateprm.Replace("CustomerName", LeadName);


                        }
                        else if (arprm[i] == "CustomerFirstName")
                        {
                            Whatsapptemplateprm = Whatsapptemplateprm.Replace("CustomerFirstName", FirstName);


                        }
                        else if (arprm[i] == "CustomerLastName")
                        {
                            Whatsapptemplateprm = Whatsapptemplateprm.Replace("CustomerLastName", LastName);


                        }
                        else if (arprm[i] == "OrganizationName")
                        {

                            if (wtsapp_company == "")
                            {
                                wtsapp_company = "No Company";
                            }
                            Whatsapptemplateprm = Whatsapptemplateprm.Replace("OrganizationName", wtsapp_company);

                        }
                        else if (arprm[i] == "OrganizationUser")
                        {
                            if (wtsapp_company == "")
                            {
                                wtsapp_company = "No Company";
                            }
                            Whatsapptemplateprm = Whatsapptemplateprm.Replace("OrganizationUser", wtsapp_company);

                        }
                        else if (arprm[i] == "OrganizationPhone")
                        {
                            if (OrgPhone == "")
                            {
                                OrgPhone = "No Contact No";
                            }
                            Whatsapptemplateprm = Whatsapptemplateprm.Replace("OrganizationPhone", OrgPhone);

                        }
                        else if (arprm[i] == "OrganizationEmail")
                        {
                            if (wtsapp_email == "")
                            {
                                wtsapp_email = "No Mail Id";
                            }
                            Whatsapptemplateprm = Whatsapptemplateprm.Replace("OrganizationEmail", wtsapp_email);

                        }
                        else if (arprm[i] == "OrganizationWebsite")
                        {
                            if (OrgWebsite == "")
                            {
                                OrgWebsite = "No";
                            }
                            Whatsapptemplateprm = Whatsapptemplateprm.Replace("OrganizationWebsite", OrgWebsite);

                        }
                        else if (arprm[i] == "Descriptions")
                        {
                            if (wtsapp_req == "")
                            {
                                wtsapp_req = "No Descriptions";
                            }
                            Whatsapptemplateprm = Whatsapptemplateprm.Replace("Descriptions", wtsapp_req);

                        }
                        //}
                    }

                }
                char[] cha = { ',' };
                char[] cha1 = { '~' };

                string strsqlitem = "select * from tbl_notificationsetupemp where type='" + typeid + "'  and ntfor='" + ntfor + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  order by aLevel ";
                DataSet dsadd = da.GetDataSetInline(strsqlitem);
                if (dsadd.Tables[0].Rows.Count > 0)
                {
                    functions fnsm = new functions();
                    string emailids = "";
                    string ewatsappno = "";
                    string etelegram = "";
                    nuid = ownerid;
                    if (nuid != "0")
                    {
                        string strtoemail = "";
                        if (isnotification == "1")
                        {

                            SqlParameter[] prmn = new SqlParameter[14];
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
                            if (isscreenpopup == "1")
                            {
                                prmn[12] = new SqlParameter("@isscreenpoppup", isscreenpopup);
                            }
                            else
                            {
                                prmn[12] = new SqlParameter("@isscreenpoppup", DBNull.Value);
                            }
                            if (isscreenpopup_frz == "1")
                            {
                                prmn[13] = new SqlParameter("@isscreenfreez", isscreenpopup_frz);
                            }
                            else
                            {
                                prmn[13] = new SqlParameter("@isscreenfreez", DBNull.Value);
                            }
                            da.executeDMLQuery("tbl_pnotificationdetailsinsert", prmn);
                        }
                        if (issms == "1")
                        {

                        }
                        if (iswatsapp == "1")
                        {
                            if (iscustomer == "1")
                            {
                                ewatsappno = wtsapp_cno;
                            }
                            else
                            {
                                ewatsappno = getwatsappId(nuid);
                            }
                            if (ewatsappno != "")
                            {

                                string WhatsAppConfiguredWithCompany = "";
                                string APIKey = "";
                                string APIURL = "";
                                string CampaignName = "";
                                string strsqlchk = "select * from tbl_WhatsAppconfiguration_crm where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and status=1 and archive=0";
                                DataSet Dschk = da.GetDataSetInline(strsqlchk);
                                if (Dschk.Tables[0].Rows.Count > 0)
                                {
                                    string RegistrationID = HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString();
                                    setnotification_whatsapp(rid.ToString(), RegistrationID, ewatsappno, whatsapptemplateid, Whatsapptemplateprm);
                                    //setnotification_whatsapp(ewatsappno, countryid, whatsapptemplateid, strtemplate_wt);

                                }




                            }
                        }
                        if (istelegram == "1")
                        {
                            if (iscustomer == "1")
                            {
                                etelegram = telegramchatid;
                            }
                            else
                            {
                                etelegram = gettelegramId(nuid);
                            }
                            if (etelegram != "")
                            {
                                sendTeleGramMessage(etelegram, notificationtext);
                            }

                        }
                        if (isemail == "1")
                        {

                            try
                            {
                                if (iscustomer == "1")
                                {
                                    strtoemail = wtsapp_email;
                                }
                                else
                                {
                                    strtoemail = fnsm.getEmpemailById(nuid);
                                }

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
                            sub = "Lead Notification by " + fnsm.getEmpNameById(uid);
                            body = notificationtext;
                            string stsignature = "";
                            //and tbl_gmailconfiguration_crm.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'
                            string strsqlchk = "select tbl_gmailconfiguration_crm.*,tbl_employee.Displayname from tbl_gmailconfiguration_crm inner join tbl_employee on tbl_gmailconfiguration_crm.uid=tbl_employee.id where tbl_gmailconfiguration_crm.uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"] + "'   and  tbl_gmailconfiguration_crm.status=1 and tbl_gmailconfiguration_crm.archive=0 and configtype='1'";
                            DataSet Dschk = da.GetDataSetInline(strsqlchk);
                            if (Dschk.Tables[0].Rows.Count > 0)
                            {
                                stsignature = Dschk.Tables[0].Rows[0]["txtSignature"].ToString();
                                if (stsignature != "")
                                {
                                    body += "<br>" + stsignature;
                                }
                                //SaveLogText("send from :" + Dschk.Tables[0].Rows[0]["cfgmailid"].ToString());
                                from = Dschk.Tables[0].Rows[0]["cfgmailid"].ToString();
                                if (to != string.Empty)
                                {
                                    sendmailnew_common(rid.ToString(), Dschk.Tables[0].Rows[0]["tokenid"].ToString(), Dschk.Tables[0].Rows[0]["refreshtokenid"].ToString(), to, from, cc, bcc, sub, body, filesdata, "1", "1", 1);
                                }
                            }
                            else
                            {
                                //tbl_gmailconfiguration_crm.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'
                                strsqlchk = "select tbl_gmailconfiguration_crm.*,tbl_employee.Displayname from tbl_gmailconfiguration_crm inner join tbl_employee on tbl_gmailconfiguration_crm.uid=tbl_employee.id where   tbl_gmailconfiguration_crm.uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"] + "' and  tbl_gmailconfiguration_crm.status=1 and tbl_gmailconfiguration_crm.archive=0 and configtype='2'";
                                Dschk = da.GetDataSetInline(strsqlchk);
                                if (Dschk.Tables[0].Rows.Count > 0)
                                {
                                    stsignature = Dschk.Tables[0].Rows[0]["txtSignature"].ToString();
                                    if (stsignature != "")
                                    {
                                        body += "<br>" + stsignature;
                                    }
                                    //SaveLogText("send from :" + Dschk.Tables[0].Rows[0]["cfgmailid"].ToString());
                                    from = Dschk.Tables[0].Rows[0]["cfgmailid"].ToString();
                                    if (to != string.Empty)
                                    {
                                        sendmailnew_common(rid.ToString(), Dschk.Tables[0].Rows[0]["tokenid"].ToString(), Dschk.Tables[0].Rows[0]["refreshtokenid"].ToString(), to, from, cc, bcc, sub, body, filesdata, "1", "1", 2);
                                    }
                                }
                                else
                                {
                                    if (to != string.Empty)
                                    {
                                        try
                                        {
                                            //sm.SendMailtoUser(to, from, cc, bcc, sub, body, filesdata);
                                        }
                                        catch
                                        { }
                                    }

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
        catch
        {

        }

    }

    protected static void sendNew_whatsappNotification(int LeadID, string RegID, string APIKey = "", string APIURL = "", string CampaignName = "")
    {
        SaveLogText("method called for new whatsapp");
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        string apiUrl = ConfigurationManager.AppSettings["gmailapi"].ToString() + "/api/values/sendWhatsAppLeadForASKRealState";
        var httpRequest1 = (HttpWebRequest)WebRequest.Create(apiUrl);

        httpRequest1.Method = "POST";
        httpRequest1.Accept = "application/json";
        httpRequest1.ContentType = "application/json";
        var data1 = @"{";
        data1 += " \"LeadID\":\"" + LeadID + "\",";
        data1 += " \"RegID\":\"" + RegID + "\",";
        data1 += " \"APIKey\":\"" + APIKey + "\",";
        data1 += " \"APIURL\":\"" + APIURL + "\",";
        data1 += " \"CampaignName\":\"" + CampaignName + "\"";
        data1 += "}";

        // SaveLogText(data1);
        using (var streamWriter = new StreamWriter(httpRequest1.GetRequestStream()))
        {
            streamWriter.Write(data1);
        }
        try
        {
            var httpResponse1 = (HttpWebResponse)httpRequest1.GetResponse();
            using (var streamReader = new StreamReader(httpResponse1.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd().Trim();
                SaveLogText("New WhatApp API Result " + result);

            }
        }
        catch (Exception ex)
        {
            SaveLogText("error when read the data" + ex.Message);
        }

    }


    protected static void sendTeleGramMessage(string chatid, string txttemp)
    {
        //SaveLogText("telegram api parametesssssr from lead");
        string apiUrl = "";
        string LeadID = "";

        if (chatid != "")
        {
            apiUrl = ConfigurationManager.AppSettings["gmailapi"].ToString() + "/api/values/sendTelegramMessageOpportunitiesowner";
            var httpRequest1 = (HttpWebRequest)WebRequest.Create(apiUrl);
            httpRequest1.Method = "POST";
            httpRequest1.Accept = "application/json";
            httpRequest1.ContentType = "application/json";
            var data1 = @"{";
            data1 += " \"ChatId\":\"" + chatid + "\",";
            data1 += " \"LeadID\":\"" + LeadID + "\",";
            data1 += " \"MessageDetail\":\"" + EscapeQuotes(txttemp) + "\",";
            data1 += " \"RegID\":\"" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "\"";
            data1 += "}";
            SaveLogText("data1:" + data1);
            using (var streamWriter = new StreamWriter(httpRequest1.GetRequestStream()))
            {
                streamWriter.Write(data1);
            }
            var httpResponse1 = (HttpWebResponse)httpRequest1.GetResponse();
            using (var streamReader = new StreamReader(httpResponse1.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd().Trim();
                //SaveLogText("Step 9:User Email Sent and Responce" + result + "");
            }

        }

    }
    public string getwatsappId(string id)
    {

        string sqlstr = "select COALESCE(NULLIF(mobile_no, ''),NULLIF(MobileNumber, '')) MobileNumber  from tbl_employee where id='" + id + "'";
        string retstr = "";
        DataSet ds = da.GetDataSetInline(sqlstr);

        int numrows = ds.Tables[0].Rows.Count;
        if (numrows > 0)
        {
            retstr = ds.Tables[0].Rows[0]["MobileNumber"].ToString();
        }
        return retstr;
    }
    public string gettelegramId(string id)
    {

        string sqlstr = "select TeleGramUserID  from tbl_employee where id='" + id + "'";
        string retstr = "";
        DataSet ds = da.GetDataSetInline(sqlstr);

        int numrows = ds.Tables[0].Rows.Count;
        if (numrows > 0)
        {
            string strCHATID = "select * from  tbl_TeleGramBotAddedUserList  where ChatId='" + ds.Tables[0].Rows[0]["TeleGramUserID"].ToString() + "' and status=1 and archive=0";
            DataSet ds_chatid = da.GetDataSetInline(strCHATID);
            if (ds_chatid.Tables[0].Rows.Count > 0)
            {
                retstr = ds_chatid.Tables[0].Rows[0]["ChatId"].ToString();
            }
            //retstr = ds.Tables[0].Rows[0]["TeleGramUserID"].ToString();
        }
        return retstr;
    }
    static string EscapeQuotes(string input)
    {
        return input.Replace("\"", "\\\"");
    }
    protected string getcontry()
    {
        string retval = "0";
        string strsql = "select isnull(countryid,2) countryid from tbl_registrationdetails where id='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            retval = ds.Tables[0].Rows[0]["countryid"].ToString();

        }
        return retval;
    }
    [WebMethod]
    public static string bindProbality(string stageid = "0")
    {
        //Response.Write(companyid);
        DataAccess da1 = new DataAccess();

        string strsqlprob = @"select id,LProbability from tbl_leadstatus where  status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and  id='" + stageid + "' ";
        DataSet dsProm = da1.GetDataSetInline(strsqlprob);

        return dsProm.GetXml();

        //Response.End();

    }
}