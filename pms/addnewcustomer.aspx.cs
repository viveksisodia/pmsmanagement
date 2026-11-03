using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonClass;
using System.Data;
using System.Data.SqlClient;

public partial class addnewcustomer : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataAccess da = new DataAccess();
    CommonFunction cmf = new CommonFunction();
    SqlParameter[] prm, prm1;
    public function fns = new function();
    CheckBox Chkbox;
    TextBox txtText;     // text box will work for text box also
    FileUpload fleupload;
    DropDownList dropdown;
    RequiredFieldValidator requiredfieldvalidator;
    public int module1Create;
    public int module2Create;
    public string showtrcomponent = "";
    public string TCSdisplay = "display:none;";
    public string issalesperson = "display:none;";
    public string codetext = "";
    public string isturk = "";
    public string Egypt = "0";
    public string turkeydispaly = "";
    public string ispromax = "display:none;";
    public string issborg = "display:none;";
    public int isRevelex = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        Label17.Text = fns.getltext("Billing Address", "2");
        if (Request.Cookies["cadmin"]["id"].ToString() == "2369")
        {
            ispromax = "";
        }
        if (fns.getcountryid() != 1)
        {
            string strsql4 = "select * from tbl_PreferencesGeneral where regid='" + Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0";
            DataSet dsper4 = da.GetDataSetInline(strsql4);
            if (dsper4.Tables[0].Rows.Count > 0)
            {
                if (dsper4.Tables[0].Rows[0]["rdplaceofsupply"].ToString() == "1")
                {
                    Egypt = "0";
                    isturk = "";
                }
                else
                {
                    Egypt = "1";
                    isturk = "display:none;";
                    rddd.Enabled = false;
                }
            }
        }
        if (Request.Cookies["cadmin"]["id"].ToString() == "2413")//SB ORG
        {
            issborg = "";
            lblcredit.Text = fns.getltext("Credit Period Allowed (in days)", "2");
            if (Request.QueryString["type"].ToString() == "MPO")
            {

                if (ContactType.SelectedValue == "2")
                {
                    Label17.Text = fns.getltext("Supplier Address", "2");
                    pnlShipping.Visible = false;
                }
                else
                {
                    pnlShipping.Visible = true;
                }
            }
        }
        //if (Request.Cookies["cadmin"]["id"].ToString() == "2364")
        //{
        //    txtvcode.Disabled = true;
        //    if (Request.QueryString["id"] == null)
        //    {
        //        txtvcode.Value = "Auto Genarate...";
        //    }
        //}
        if (Request.Cookies["user"] == null || Request.Cookies["cadmin"] == null)
        {
            Response.Redirect("login.aspx");
        }
        this.Title = fns.getltext("Add New Party", "2");
        // lbltitle.Text = fns.getltext("Add New Party", "2");
        lblvattrnnumber.Text = fns.getltext("VAT TRN Number", "2");
        lbldeliveryplace.Text = fns.getltext("Delivery Place", "2");
        lblVATTreatment.Text = fns.getltext("VAT Treatment", "2");
        if (Request.Cookies["cadmin"]["id"].ToString() == "2182")
        {

            RequiredFieldValidator5.Enabled = true;

        }
        if (fns.getcountryid() == 1)
        {
            showtrcomponent = "display:none;";
            lblvattrnnumber.Text = fns.getltext("GSTN Number", "2");
            RegExp1.Enabled = false;
            lbldeliveryplace.Text = fns.getltext("State", "2");
            lblVATTreatment.Text = fns.getltext("GST Treatment", "2");
            //    rddd.Enabled = false;


            //lblvatclassificationname.Text = "HSN Code";

        }
        else
        {
            trbillgst.Visible = false;
            trShipgst.Visible = false;
        }
        this.Title = fns.getltext("Add New Contact", "2");
        string strsqlper = "select * from tbl_PreferencesGeneral where regid='" + Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0";
        DataSet dsper = da.GetDataSetInline(strsqlper);
        if (dsper.Tables[0].Rows.Count > 0)
        {

            if (dsper.Tables[0].Rows[0]["rdltcs"].ToString() == "1")
            {
                TCSdisplay = "";

            }
        }
        addCustomfield();

        DataSet dsfreference = fns.getgpreference();
        if (dsfreference.Tables[0].Rows.Count > 0)
        {
            if (dsfreference.Tables[0].Rows[0]["islenghcal"].ToString() == "1")
            {
                issalesperson = "";
            }
            if (dsfreference.Tables[0].Rows[0]["comname"].ToString() == "Revelex")//Revelex 2422
            {
                isRevelex = 1;
            }
        }

        if(Request.Cookies["cadmin"]["id"].ToString()=="2172")
        {
            isRevelex = 1;
        }


        if(isRevelex==1)
        {
            txtvcode.Disabled = true;
            txtvcode.Attributes.Add("placeholder", "Auto Generate");
            //txtvcode.Disabled = true; // no need autogen
            //string st = "select count(id) as autogen from tbl_contactsdetails";
            //DataSet dsauto = da.GetDataSetInline(st);
            //if (dsauto.Tables[0].Rows.Count > 0)
            //{
            //    decimal autogen = 0;
            //    try
            //    {
            //        autogen = Convert.ToDecimal(dsauto.Tables[0].Rows[0]["autogen"].ToString());
            //    }
            //    catch { }
            //    string autogenCode = "Rev-" + (autogen + 1).ToString("000");
            //    txtvcode.Value = autogenCode;
            //}
        }
        string strsql = "select * from tbl_Preferencescontact where regid='" + Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0";

        DataSet dsrun = da.GetDataSetInline(strsql);
        if (dsrun.Tables[0].Rows.Count > 0)
        {
            if (dsrun.Tables[0].Rows[0]["ddlissalespersone"].ToString() == "2")
            {
                issalesperson = "";
            }
        }

        if (!Page.IsPostBack)
        {
            bindemp();

            bindsectionname();
            ViewState["trcat"] = "0";
            ViewState["trsubcat"] = "0";
            // pnlShipping.Visible = true;
            if (Request.QueryString["type"] != null)
            {
                if (Request.QueryString["type"].ToString() == "3")
                {
                    ContactType.SelectedValue = "3";
                    binddatatab();
                }
                else
                {
                    ContactType.SelectedValue = "1";
                }
            }
            else
            {
                ContactType.SelectedValue = "2";
                binddatatab();
            }
            if (ContactType.SelectedValue == "3")
            {
                codetext = fns.getltext("Contractor Code", "2");
                //creditlimit.Visible = false;
            }
            else if (ContactType.SelectedValue == "2")
            {
                codetext = fns.getltext("Vendor Code", "2");
                //creditlimit.Visible = false;
            }
            else
            {
                codetext = fns.getltext("Customer Code", "2");
                // creditlimit.Visible = true;
            }
            if (Request.Cookies["cadmin"]["id"].ToString() == "2292" || Request.Cookies["cadmin"]["id"].ToString() == "2318")
            {
                txtvcode.Disabled = true;
            }
            grdviewpricebind();
            binddata();
            bindgroup();

            bindmasterdata();
            bindddlplaceofsupply();
            SetInitialRow();
            //  bindaitemsdetails();
            if (ViewState["trcat"].ToString() == "0")
            {
                GrdvDepartment.Columns[1].Visible = false;
            }
            if (ViewState["trsubcat"].ToString() == "0")
            {
                GrdvDepartment.Columns[2].Visible = false;
            }

            if (Request.QueryString["id"] != null)
            {
                ContactType.Enabled = false;
                //  binddetails(); 
                binddatatab();
            }
            if (fns.getcountryid() == 3)
            {
                //txtdeductxt.Text = fns.getltext("Tax deducted?(Withholding Tax)", "2");
                turkeydispaly = "display:none;";
                trcustomertype.Visible = false;
                RegExp1.Enabled = false;
                RegExpturkey.Enabled = true;
            }
            else
            {
                RegExpturkey.Enabled = false;
            }

        }
        if (Request.Cookies["cadmin"]["id"].ToString() == "2239" || Request.Cookies["cadmin"]["id"].ToString() == "2264" || Request.Cookies["cadmin"]["id"].ToString() == "2286")
        {
            placeofsupplytr.Visible = false;
        }
        if (!Page.IsPostBack)
        {
            if (Request.Cookies["cadmin"]["id"].ToString() == "2322")
            {
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
            }
            fns.changerdltext(ContactType);
            fns.changerdltext(rdltcs);
            fns.changerdltext(istds);
            fns.changeddltext(txtunder);

            fns.changeddltext(VATTreatment);
            fns.changeddltext(ddlcustomertype);
            fns.changeddltext(ddlemployee);
            ddlcustomertype.DataPlaceHolder = fns.getltext("--Select Price Category--", "2");
            ddlemployee.DataPlaceHolder = fns.getltext("--Select Employee--", "2");

        }

        if (Request.Cookies["cadmin"]["id"].ToString() == "2351")
        {
            if (ContactType.SelectedValue == "1")
            {
                txtvcode.Disabled = true;
            }
        }

    }
    protected void ddlsectionname_change(object sender, EventArgs e)
    {
        if (ddlsectionname.SelectedValue == "-1")
        {
            txtsectionname.Visible = true;
            RequiredFieldValidator11.Enabled = true;
            testper.Visible = true;
            RequiredFieldValidator12.Enabled = true;
        }
        else
        {
            txtsectionname.Visible = false;
            RequiredFieldValidator11.Enabled = false;
            testper.Visible = false;
            RequiredFieldValidator12.Enabled = false;
        }
    }
    protected void istds_change(object sender, EventArgs e)
    {
        if (istds.SelectedValue == "1")
        {
            pnltdssection.Visible = true;
        }
        else
        {
            pnltdssection.Visible = false;

        }
    }
    protected void bindsectionname()
    {
        string strsql = "select * from tbl_tdssectionname where regid='" + Request.Cookies["cadmin"]["id"].ToString() + "' and status=1 and archive=0";
        DataSet dss = da.GetDataSetInline(strsql);
        cmf.BindDropDownList(ddlsectionname, dss, "sectionname", "id");
        ListItem li = new ListItem(" ", "0");
        ddlsectionname.Items.Insert(0, li);
        ListItem li1 = new ListItem("+ Add New", "-1");
        ddlsectionname.Items.Add(li1);
        txtsectionname.Visible = false;
        RequiredFieldValidator11.Enabled = false;
        testper.Visible = false;
        RequiredFieldValidator12.Enabled = false;

    }
    protected void grdviewpricebind()
    {
        if (ContactType.SelectedValue == "1")
        {
            trcustomertype.Visible = true;
        }
        else
        {
            trcustomertype.Visible = false;
        }
        string strdata = "select * from tbl_pricescategory where status=1 and archive=0 and regid='" + Request.Cookies["cadmin"]["id"] + "' order by id";
        DataSet ds = da.GetDataSetInline(strdata);
        cmf.BindDropDownList(ddlcustomertype, ds, "catname", "id");
        ListItem lip = new ListItem("--Select Price Category--", "0");
        ddlcustomertype.Items.Insert(0, lip);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ContactType.SelectedValue == "1")
            {
                trcustomertype.Visible = true;
            }
            else
            {
                trcustomertype.Visible = false;
            }
        }

    }

    protected void bindgroup()
    {

        SqlParameter[] prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
        if (ContactType.SelectedValue == "1")
        {
            if (fns.getcountryid() == 3)
            {
                prm[1] = new SqlParameter("@pid", "268");
            }
            else
            {
                prm[1] = new SqlParameter("@pid", "26");
            }
        }
        else
        {
            if (fns.getcountryid() == 3)
            {
                prm[1] = new SqlParameter("@pid", "22");
            }
            else
            {
                prm[1] = new SqlParameter("@pid", "22");
            }
        }
        DataSet ds = da.GetDataSet("selectDebtorsgroup", prm);
        cmf.BindDropDownList(txtunder, ds, "groupname", "id");



    }
    protected void VATTreatment_change(object sender, EventArgs e)
    {
        if (fns.getcountryid() != 1)
        {
            bindddlplaceofsupply();
        }
        else
        {

            if (VATTreatment.SelectedValue == "10")
            {
                lbldeliveryplace.Text = "Country";
                bindddlplaceofsupplycountry();
            }
            else
            {
                lbldeliveryplace.Text = "State";
                //if(Request.QueryString["id"]==null)
                // {
                bindddlplaceofsupply();

                //}


            }

        }
    }
    protected void bindddlplaceofsupplycountry()
    {


        string strsql = "select * from Imt_Country where status=1 and archive=0";
        DataSet ds = da.GetDataSetInline(strsql);
        cmf.BindDropDownList(ddlplaceofsupply, ds, "country", "conid");
        ListItem li = new ListItem("", "0");
        ddlplaceofsupply.Items.Insert(0, li);



    }
    protected void bindddlplaceofsupply()
    {
        if (fns.getcountryid() == 1)
        {

            string strsql = "select * from tbl_statemaster where conid='" + fns.getcountryid() + "' and status=1 and archive=0";
            DataSet ds = da.GetDataSetInline(strsql);
            cmf.BindDropDownList(ddlplaceofsupply, ds, "statename", "id");
            ListItem li = new ListItem("", "0");
            ddlplaceofsupply.Items.Insert(0, li);


        }
        else
        {
            if (fns.getcountryid() == 3)
            {
                string strsqltrt = "select * from tbl_VATTreatmentmaster where id='" + VATTreatment.SelectedValue + "'";
                DataSet dstt = da.GetDataSetInline(strsqltrt);
                if (dstt.Tables[0].Rows.Count > 0)
                {
                    string strsql = "select * from tbl_PlaceOfSupply where status=1 and archive=0 and typeid='3'";
                    DataSet ds = da.GetDataSetInline(strsql);
                    cmf.BindDropDownList(ddlplaceofsupply, ds, "statename", "id");
                    ListItem li = new ListItem("", "0");
                    ddlplaceofsupply.Items.Insert(0, li);
                    if (dstt.Tables[0].Rows[0]["isgcc"].ToString() == "0")
                    {
                        ddlplaceofsupply.SelectedValue = "0";
                        VATTRNNumbertr.Visible = false;
                        placeofsupplytr.Visible = false;
                    }
                    else
                    {
                        VATTRNNumbertr.Visible = true;
                        if (Egypt != "1")
                        {
                            placeofsupplytr.Visible = true;
                        }
                        else
                        {
                            placeofsupplytr.Visible = false;

                        }
                    }
                }
            }
            else
            {





                string strsqltrt = "select * from tbl_VATTreatmentmaster where id='" + VATTreatment.SelectedValue + "'";
                DataSet dstt = da.GetDataSetInline(strsqltrt);
                if (dstt.Tables[0].Rows.Count > 0)
                {


                    string strsql = "select * from tbl_PlaceOfSupply where status=1 and archive=0 and typeid='1'";

                    if (dstt.Tables[0].Rows[0]["isgcc"].ToString() == "2")
                    {
                        strsql = "select * from tbl_PlaceOfSupply where status=1 and archive=0 and typeid='2'";

                    }

                    DataSet ds = da.GetDataSetInline(strsql);
                    cmf.BindDropDownList(ddlplaceofsupply, ds, "statename", "id");
                    ListItem li = new ListItem("", "0");
                    ddlplaceofsupply.Items.Insert(0, li);

                    if (dstt.Tables[0].Rows[0]["isgcc"].ToString() == "0")
                    {
                        ddlplaceofsupply.SelectedValue = "0";
                        VATTRNNumbertr.Visible = false;
                        placeofsupplytr.Visible = false;
                    }
                    else
                    {
                        VATTRNNumbertr.Visible = true;
                        if (Egypt != "1")
                        {
                            placeofsupplytr.Visible = true;
                        }
                        else
                        {
                            placeofsupplytr.Visible = false;

                        }
                    }

                }

            }
        }
        if (Request.Cookies["cadmin"]["id"].ToString() == "2239" || Request.Cookies["cadmin"]["id"].ToString() == "2264" || Request.Cookies["cadmin"]["id"].ToString() == "2286")
        {
            placeofsupplytr.Visible = false;
        }
    }


    protected void ContactType_change(object sender, EventArgs e)
    {
        binddatatab();


    }
    protected void binddatatab()
    {
        if (ContactType.SelectedValue == "3")
        {
            codetext = fns.getltext("Contractor Code", "2");
            //creditlimit.Visible = false;
        }
        else if (ContactType.SelectedValue == "2")
        {
            codetext = fns.getltext("Vendor Code", "2");
            //creditlimit.Visible = false;
        }
        else
        {
            codetext = fns.getltext("Customer Code", "2");
            // creditlimit.Visible = true;
        }

    }
    protected void bindcat()
    {
        string strsql = "select * from tbl_itemcategory where regid='" + Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0";
        DataSet dscat = da.GetDataSetInline(strsql);
        cmf.BindDropDownList(ddlcat, dscat, "catname", "id");
        ListItem li = new ListItem("All", "%");
        ddlcat.Items.Insert(0, li);

    }
    protected void bindsubcat()
    {
        string catid = "0";

        if (ddlcat.SelectedValue != "%")
        {
            catid = ddlcat.SelectedValue;
        }
        string strsql = "select * from tbl_itemsubcatgory where regid='" + Request.Cookies["cadmin"]["id"] + "' and catid='" + catid + "' and status=1 and archive=0";
        DataSet dscat = da.GetDataSetInline(strsql);
        cmf.BindDropDownList(ddlsubcat, dscat, "subcatname", "id");
        ListItem li = new ListItem("All", "%");
        ddlsubcat.Items.Insert(0, li);


    }
    protected void GrdvDepartment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:SetMouseOver(this)";
            e.Row.Attributes["onmouseout"] = "javascript:SetMouseOut(this)";

            HiddenField hdnitemid = (HiddenField)e.Row.FindControl("hdnitemid");
            TextBox txtprice = (TextBox)e.Row.FindControl("txtprice");

            if (Request.QueryString["id"] != null)
            {
                string strsql = "select * from tbl_contactpriceslist where sid='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "' and itemid='" + hdnitemid.Value + "'";
                DataSet ds = da.GetDataSetInline(strsql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtprice.Text = ds.Tables[0].Rows[0]["price"].ToString();
                }
            }

        }

        if (e.Row.RowType == DataControlRowType.Header)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].Text = fns.getltext(e.Row.Cells[i].Text, "2");
            }
        }

    }


    protected void ddlcat_change(object sender, EventArgs e)
    {

        bindsubcat();
        binddata();
    }
    protected void ddlsubcat_change(object sender, EventArgs e)
    {
        bindaitemsdetails();
    }


    protected void bindaitemsdetails()
    {
        string strfromdate = da.getcdate();
        string strtodate = da.getcdate();

        string catid = "%";
        string subcatid = "%";
        if (ViewState["trcat"].ToString() == "1")
        {
            catid = ddlcat.SelectedValue;
        }
        if (ViewState["trsubcat"].ToString() == "1")
        {
            subcatid = ddlsubcat.SelectedValue;
        }

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"]);
        prm[1] = new SqlParameter("@searchtxt", "%");
        prm[2] = new SqlParameter("@cdate", strfromdate);
        prm[3] = new SqlParameter("@todate", strtodate);
        prm[4] = new SqlParameter("@categoryid", catid);
        prm[5] = new SqlParameter("@subcategoryid", subcatid);
        ds = da.GetDataSet("tbl_itemsdetailsmanagenew_catsup", prm);
        GrdvDepartment.DataSource = ds;
        GrdvDepartment.DataBind();





    }


    protected void binddata()
    {
        if (Request.QueryString["id"] == null)
        {
            string strsql = "select Manage_Profile.* from Manage_Profile inner join tbl_employee on Manage_Profile.id=cast(tbl_employee.roleid as int) where tbl_employee.id='" + Request.Cookies["user"]["user_id"] + "'  and Manage_Profile.status=1 and Manage_Profile.archive=0";
            DataSet ds = da.GetDataSetInline(strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {

                if (ds.Tables[0].Rows[0]["id"].ToString() == "1")
                {
                    module1Create = 1;
                    module2Create = 1;
                }
                else
                {
                    string strsqlmodule = "select * from tbl_rolepermisionsdetails where regid='" + Request.Cookies["cadmin"]["id"].ToString() + "' and profileid='" + ds.Tables[0].Rows[0]["id"].ToString() + "' and moduleid in(1,2)";
                    DataSet dsmodule = da.GetDataSetInline(strsqlmodule);
                    if (dsmodule.Tables[0].Rows.Count > 0)
                    {
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
                            if (dsmodule.Tables[0].Rows[i]["moduleid"].ToString() == "1")
                            {
                                if (CheckBox2Customersvalue == "1")
                                {

                                    module1Create = 1;
                                }
                            }
                            if (dsmodule.Tables[0].Rows[i]["moduleid"].ToString() == "2")
                            {
                                if (CheckBox2Customersvalue == "1")
                                {

                                    module2Create = 1;
                                }
                            }
                        }
                    }
                }
            }


            if (module1Create == 1 && module2Create == 1)
            {

            }
            else
            {
                if (module1Create == 1)
                {
                    ContactType.SelectedValue = "1";
                    ContactType.Enabled = false;
                }
                if (module2Create == 1)
                {
                    ContactType.SelectedValue = "2";
                    ContactType.Enabled = false;
                }
            }
        }
    }


    protected void grduser_PreRender(object sender, System.EventArgs e)
    {

        try
        {
            if (Gridview1.Rows.Count > 0)
            {
                Gridview1.UseAccessibleHeader = true;
                Gridview1.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void binddetails()
    {
        string strsql = "select * from tbl_contactsdetails where id='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "' and status=1 and archive=0 and regid='" + Request.Cookies["cadmin"]["id"].ToString() + "'";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ContactType.SelectedValue = ds.Tables[0].Rows[0]["ContactType"].ToString();
            //  prm[2] = new SqlParameter("@cdate", da.getcdatetime());
            //  prm[3] = new SqlParameter("@uid", Request.Cookies["user"]["user_id"].ToString());
            txtsalutation.Value = ds.Tables[0].Rows[0]["txtsalutation"].ToString();
            txtfirstname.Value = ds.Tables[0].Rows[0]["txtfirstname"].ToString();
            txtlastname.Value = ds.Tables[0].Rows[0]["txtlastname"].ToString();
            txtvcode.Value = ds.Tables[0].Rows[0]["vcode"].ToString();
            txtcompanyname.Value = ds.Tables[0].Rows[0]["txtcompanyname"].ToString();
            txtcontactdisplayname.Value = ds.Tables[0].Rows[0]["txtcompanyname"].ToString();
            txtContactEmail.Text = ds.Tables[0].Rows[0]["txtContactEmail"].ToString();
            txtworkphonePhone.Value = ds.Tables[0].Rows[0]["txtworkphonePhone"].ToString();
            txtmobile.Value = ds.Tables[0].Rows[0]["txtmobile"].ToString();
            txtSkypeName.Value = ds.Tables[0].Rows[0]["txtSkypeName"].ToString();
            txtDesignation.Value = ds.Tables[0].Rows[0]["txtDesignation"].ToString();
            txtDepartment.Value = ds.Tables[0].Rows[0]["txtDepartment"].ToString();
            Website.Value = ds.Tables[0].Rows[0]["Website"].ToString();
            VATTreatment.SelectedValue = ds.Tables[0].Rows[0]["VATTreatment"].ToString();
            TaxRegistrationNumber.Value = ds.Tables[0].Rows[0]["TaxRegistrationNumber"].ToString();
            ddlplaceofsupply.SelectedValue = ds.Tables[0].Rows[0]["PlaceOfSupply"].ToString();

            try
            {
                txtCurrency.SelectedValue = ds.Tables[0].Rows[0]["txtCurrency"].ToString();
            }
            catch (Exception ex)
            {
            }


            PaymentTerms.Value = ds.Tables[0].Rows[0]["PaymentTerms"].ToString();
            PaymentTermsdisplay.Value = ds.Tables[0].Rows[0]["PaymentTermsdisplay"].ToString();
            Facebook.Value = ds.Tables[0].Rows[0]["Facebook"].ToString();
            Twitter.Value = ds.Tables[0].Rows[0]["Twitter"].ToString();
            billAttention.Value = ds.Tables[0].Rows[0]["billAttention"].ToString();
            billAddress1.Value = ds.Tables[0].Rows[0]["billAddress1"].ToString();
            billAddress2.Value = ds.Tables[0].Rows[0]["billAddress2"].ToString();
            billCity.Value = ds.Tables[0].Rows[0]["billCity"].ToString();
            billState.Value = ds.Tables[0].Rows[0]["billState"].ToString();
            billZipCode.Value = ds.Tables[0].Rows[0]["billZipCode"].ToString();
            billCountry.Value = ds.Tables[0].Rows[0]["billCountry"].ToString();
            billPhone.Value = ds.Tables[0].Rows[0]["billPhone"].ToString();
            billFax.Value = ds.Tables[0].Rows[0]["billFax"].ToString();
            billgst.Value = ds.Tables[0].Rows[0]["billgst"].ToString();
            Shipgst.Value = ds.Tables[0].Rows[0]["Shipgst"].ToString();
            ShipAttention.Value = ds.Tables[0].Rows[0]["ShipAttention"].ToString();
            ShipAddress1.Value = ds.Tables[0].Rows[0]["ShipAddress1"].ToString();
            ShipAddress2.Value = ds.Tables[0].Rows[0]["ShipAddress2"].ToString();
            ShipCity.Value = ds.Tables[0].Rows[0]["ShipCity"].ToString();
            ShipState.Value = ds.Tables[0].Rows[0]["ShipState"].ToString();
            ShipZipCode.Value = ds.Tables[0].Rows[0]["ShipZipCode"].ToString();
            ShipCountry.Value = ds.Tables[0].Rows[0]["ShipCountry"].ToString();
            ShipPhone.Value = ds.Tables[0].Rows[0]["ShipPhone"].ToString();
            ShipFax.Value = ds.Tables[0].Rows[0]["ShipFax"].ToString();
            remarks.Value = ds.Tables[0].Rows[0]["remarks"].ToString();
            txtcreditlimit.Text = ds.Tables[0].Rows[0]["txtcreditlimit"].ToString();
            txtcreditperiod.Text = ds.Tables[0].Rows[0]["txtcreditperiod"].ToString();
            btnsave.Text = "Update";

            SqlParameter[] prmadd = new SqlParameter[1];
            prmadd[0] = new SqlParameter("@contactid", cmf.Decrypt(Request.QueryString["id"].ToString()));
            DataSet dsadd = da.GetDataSet("tbl_contactcontactpersondetailsselect", prmadd);
            if (dsadd.Tables[0].Rows.Count > 0)
            {
                ViewState["CurrentTable"] = dsadd.Tables[0];
                Gridview1.DataSource = dsadd.Tables[0];
                Gridview1.DataBind();
                SetPreviousData();


            }

            //bindpricelist();

            bindcusdata();
        }
    }
    //protected void bindpricelist()
    //{

    //}

    protected void bindcusdata()
    {

        string strsql = "select tbl_customefielddetails.*,tbl_cusdatatypemaster.cusdatatype from tbl_customefielddetails inner join tbl_cusdatatypemaster  on  tbl_customefielddetails.labeldatatype=tbl_cusdatatypemaster.id  where tbl_customefielddetails.regid='" + Request.Cookies["cadmin"]["id"] + "' and tbl_customefielddetails.status=1 and tbl_customefielddetails.archive=0 and tbl_customefielddetails.typeid=2";
        DataSet ds = da.GetDataSetInline(strsql);

        int counter = 1; // this will be used to control naming  
        // create a table to store 
        for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
        {
            int y = Convert.ToInt32(ds.Tables[0].Rows[x]["labeldatatype"].ToString());
            string controlid = ds.Tables[0].Rows[x]["id"].ToString();
            string controlvalue = "";


            string strsqldata = "select * from tbl_customercustomefieldvalue where uid='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "' and controlid='" + controlid + "' and status=1 and archive=0";
            DataSet dsc = da.GetDataSetInline(strsqldata);
            if (dsc.Tables[0].Rows.Count > 0)
            {
                controlvalue = dsc.Tables[0].Rows[0]["controlvalue"].ToString();

                switch (y)
                {
                    case 1:
                        txtText = (TextBox)pnlcustomControl.FindControl("ctlcust" + Convert.ToString(counter));
                        txtText.Text = controlvalue;
                        break;
                    case 2:
                        Chkbox = (CheckBox)pnlcustomControl.FindControl("ctlcust" + Convert.ToString(counter));
                        if (controlvalue == "1")
                        {

                            Chkbox.Checked = true;
                        }
                        break;
                    case 3:
                        dropdown = (DropDownList)pnlcustomControl.FindControl("ctlcust" + Convert.ToString(counter));
                        try
                        {
                            dropdown.SelectedValue = controlvalue;
                        }
                        catch (Exception ex)
                        {
                        }
                        break;
                    default:
                        break;
                }
            }
            counter++;


        }
    }

    private void addCustomfield()
    {
        // add the custom field label and their name from
        //according to database
        /* Process 
         *  First take the book id and access all the 
         *  custom field inoformation and create dynamic field 
         *  and add the the panel
         */
        // now access the control  record from the database 
        // To Access book structure use proc "intra_sp_fetchtblstruct" and take one argument "@folder_id"
        // and it will give columns book_id,label,field_id(Control id),sort_order


        string strsql = "select tbl_customefielddetails.*,tbl_cusdatatypemaster.cusdatatype from tbl_customefielddetails inner join tbl_cusdatatypemaster  on  tbl_customefielddetails.labeldatatype=tbl_cusdatatypemaster.id  where tbl_customefielddetails.regid='" + Request.Cookies["cadmin"]["id"] + "' and tbl_customefielddetails.status=1 and tbl_customefielddetails.archive=0 and tbl_customefielddetails.typeid=2";
        DataSet ds = da.GetDataSetInline(strsql);

        int i = 1;  // this will be used to control naming  
        // create a table to store 
        for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
        {
            addControls(ds.Tables[0].Rows[x]["labeldatatype"].ToString(), ds.Tables[0].Rows[x]["labelname"].ToString(), i, ds.Tables[0].Rows[x]["defaultvalue"].ToString(), ds.Tables[0].Rows[x]["txtvalues"].ToString(), ds.Tables[0].Rows[x]["ismandatry"].ToString(), ds.Tables[0].Rows[x]["ismultiselection"].ToString());
            i++;
        }

    }
    void addControls(string controlid, string lablename, int counter, string defaultvalue, string txtvalues, string ismandatry, string ismultiselection)
    {

        // control id description 1.fileupload, 2. checkbox 3. textbox 4. textArea
        int x = Convert.ToInt32(controlid);


        switch (x)
        {
            case 1:
                txtText = new TextBox();
                txtText.ID = "ctlcust" + Convert.ToString(counter);
                txtText.Text = defaultvalue;

                pnlcustomControl.Controls.Add(new LiteralControl("<tr> <td class='leftcol'>" + lablename.Replace("_", " ") + "</td><td>"));
                pnlcustomControl.Controls.Add(txtText);
                if (ismandatry == "1")
                {
                    requiredfieldvalidator = new RequiredFieldValidator();
                    requiredfieldvalidator.ID = "ctlrequired" + Convert.ToString(counter);
                    requiredfieldvalidator.ValidationGroup = "gi";
                    requiredfieldvalidator.ControlToValidate = txtText.ID;
                    requiredfieldvalidator.SetFocusOnError = true;
                    requiredfieldvalidator.ErrorMessage = "Please select " + lablename + " in Custom Field Detail";
                    pnlcustomControl.Controls.Add(requiredfieldvalidator);
                }
                pnlcustomControl.Controls.Add(new LiteralControl("</td></tr>"));
                break;
            case 2:
                //Response.Write("you are in the 2");  
                Chkbox = new CheckBox();
                Chkbox.ID = "ctlcust" + Convert.ToString(counter);
                if (defaultvalue == "1")
                {
                    Chkbox.Checked = true;
                }
                pnlcustomControl.Controls.Add(new LiteralControl("<tr> <td class='leftcol'>" + lablename.Replace("_", " ") + "</td><td>"));
                pnlcustomControl.Controls.Add(Chkbox);
                if (ismandatry == "1")
                {
                    requiredfieldvalidator = new RequiredFieldValidator();
                    requiredfieldvalidator.ID = "ctlrequired" + Convert.ToString(counter);
                    requiredfieldvalidator.ValidationGroup = "gi";
                    requiredfieldvalidator.ControlToValidate = Chkbox.ID;
                    requiredfieldvalidator.SetFocusOnError = true;
                    requiredfieldvalidator.ErrorMessage = "Please select " + lablename + " in Custom Field Detail";
                    pnlcustomControl.Controls.Add(requiredfieldvalidator);
                }

                pnlcustomControl.Controls.Add(new LiteralControl("</td></tr>"));
                break;
            case 3:
                //Response.Write("you are in the 3");  
                if (ismultiselection == "1")
                {
                    ListBox ListBoxmulti;
                    ListBoxmulti = new ListBox();
                    ListBoxmulti.ID = "ctlcust" + Convert.ToString(counter);

                    if (hdnmultiselectionid.Value != string.Empty)
                    {
                        hdnmultiselectionid.Value = hdnmultiselectionid.Value + "," + "ctlcust" + Convert.ToString(counter);
                    }
                    else
                    {
                        hdnmultiselectionid.Value = "ctlcust" + Convert.ToString(counter);
                    }

                    ListBoxmulti.SelectionMode = ListSelectionMode.Multiple;
                    string[] ch = { "###" };
                    string[] arr = txtvalues.Split(ch, StringSplitOptions.None);
                    ListBoxmulti.DataSource = arr;
                    // dropdown.DataTextField=
                    ListBoxmulti.DataBind();
                    //ListItem li = new ListItem("", "0");
                    //ListBoxmulti.Items.Insert(0, li);
                    pnlcustomControl.Controls.Add(new LiteralControl("<tr> <td class='leftcol'>" + lablename.Replace("_", " ") + "</td><td>"));
                    pnlcustomControl.Controls.Add(ListBoxmulti);
                    if (ismandatry == "1")
                    {
                        //requiredfieldvalidator = new RequiredFieldValidator();
                        //requiredfieldvalidator.ID = "ctlrequired" + Convert.ToString(counter);
                        //requiredfieldvalidator.ValidationGroup = "savebtn";
                        //requiredfieldvalidator.ControlToValidate = dropdown.ID;
                        //requiredfieldvalidator.SetFocusOnError = true;
                        //requiredfieldvalidator.ErrorMessage = "*";
                        //requiredfieldvalidator.InitialValue = "0";
                        //pnlcustomControl.Controls.Add(requiredfieldvalidator);
                    }
                    pnlcustomControl.Controls.Add(new LiteralControl("</td></tr>"));
                }
                else
                {
                    dropdown = new DropDownList();
                    dropdown.ID = "ctlcust" + Convert.ToString(counter);
                    string[] ch = { "###" };
                    string[] arr = txtvalues.Split(ch, StringSplitOptions.None);
                    dropdown.DataSource = arr;
                    // dropdown.DataTextField=
                    dropdown.DataBind();
                    ListItem li = new ListItem("", "0");
                    dropdown.Items.Insert(0, li);
                    pnlcustomControl.Controls.Add(new LiteralControl("<tr> <td class='leftcol'>" + lablename.Replace("_", " ") + "</td><td>"));
                    pnlcustomControl.Controls.Add(dropdown);
                    if (ismandatry == "1")
                    {
                        requiredfieldvalidator = new RequiredFieldValidator();
                        requiredfieldvalidator.ID = "ctlrequired" + Convert.ToString(counter);
                        requiredfieldvalidator.ValidationGroup = "gi";
                        requiredfieldvalidator.ControlToValidate = dropdown.ID;
                        requiredfieldvalidator.SetFocusOnError = true;
                        requiredfieldvalidator.ErrorMessage = "Please select " + lablename + " in Custom Field Detail";
                        requiredfieldvalidator.InitialValue = "0";
                        pnlcustomControl.Controls.Add(requiredfieldvalidator);
                    }
                    pnlcustomControl.Controls.Add(new LiteralControl("</td></tr>"));
                }
                break;


            default:
                break;
        }
    }

    private void SetInitialRow()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(new DataColumn("id", typeof(string)));
        dt.Columns.Add(new DataColumn("Salutation", typeof(string)));
        dt.Columns.Add(new DataColumn("FirstName", typeof(string)));
        dt.Columns.Add(new DataColumn("lastname", typeof(string)));
        dt.Columns.Add(new DataColumn("EmailAddress", typeof(string)));
        dt.Columns.Add(new DataColumn("WorkPhone", typeof(string)));
        dt.Columns.Add(new DataColumn("Mobile", typeof(string)));
        dt.Columns.Add(new DataColumn("SkypeName", typeof(string)));
        dt.Columns.Add(new DataColumn("Designation", typeof(string)));
        dt.Columns.Add(new DataColumn("Department", typeof(string)));
        dr = dt.NewRow();
        dr["id"] = 1;
        dr["Salutation"] = string.Empty;
        dr["FirstName"] = string.Empty;
        dr["lastname"] = string.Empty;
        dr["EmailAddress"] = string.Empty;
        dr["WorkPhone"] = string.Empty;
        dr["Mobile"] = string.Empty;
        dr["SkypeName"] = string.Empty;
        dr["Designation"] = string.Empty;
        dr["Department"] = string.Empty;


        dt.Rows.Add(dr);
        //DataRow dr1 = null;
        //dr1 = dt.NewRow();
        //dr1["id"] = 1;
        //dr1["Salutation"] = string.Empty;
        //dr1["FirstName"] = string.Empty;
        //dr1["lastname"] = string.Empty;
        //dr1["EmailAddress"] = string.Empty;
        //dr1["WorkPhone"] = string.Empty;
        //dr1["Mobile"] = string.Empty;
        //dr1["SkypeName"] = string.Empty;
        //dr1["Designation"] = string.Empty;
        //dr1["Department"] = string.Empty;

        //dt.Rows.Add(dr1);

        ViewState["CurrentTable"] = dt;
        hdngridviewrow.Value = dt.Rows.Count.ToString();
        Gridview1.DataSource = dt;
        Gridview1.DataBind();
    }



    private void AddNewRowToGrid()
    {
        int rowIndex = 0;

        if (ViewState["CurrentTable"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {


                    TextBox Salutation = (TextBox)Gridview1.Rows[rowIndex].FindControl("Salutation");
                    TextBox FirstName = (TextBox)Gridview1.Rows[rowIndex].FindControl("FirstName");
                    TextBox lastname = (TextBox)Gridview1.Rows[rowIndex].FindControl("lastname");
                    TextBox EmailAddress = (TextBox)Gridview1.Rows[rowIndex].FindControl("EmailAddress");
                    TextBox WorkPhone = (TextBox)Gridview1.Rows[rowIndex].FindControl("WorkPhone");
                    TextBox Mobile = (TextBox)Gridview1.Rows[rowIndex].FindControl("Mobile");
                    TextBox SkypeName = (TextBox)Gridview1.Rows[rowIndex].FindControl("SkypeName");
                    TextBox Designation = (TextBox)Gridview1.Rows[rowIndex].FindControl("Designation");
                    TextBox Department = (TextBox)Gridview1.Rows[rowIndex].FindControl("Department");


                    drCurrentRow = dtCurrentTable.NewRow();
                    drCurrentRow["id"] = i + 1;
                    dtCurrentTable.Rows[i - 1]["id"] = i.ToString();
                    dtCurrentTable.Rows[i - 1]["Salutation"] = Salutation.Text;
                    dtCurrentTable.Rows[i - 1]["FirstName"] = FirstName.Text;
                    dtCurrentTable.Rows[i - 1]["lastname"] = lastname.Text;
                    dtCurrentTable.Rows[i - 1]["EmailAddress"] = EmailAddress.Text;
                    dtCurrentTable.Rows[i - 1]["WorkPhone"] = WorkPhone.Text;
                    dtCurrentTable.Rows[i - 1]["Mobile"] = Mobile.Text;
                    dtCurrentTable.Rows[i - 1]["SkypeName"] = SkypeName.Text;
                    dtCurrentTable.Rows[i - 1]["Designation"] = Designation.Text;
                    dtCurrentTable.Rows[i - 1]["Department"] = Department.Text;


                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["CurrentTable"] = dtCurrentTable;
                hdngridviewrow.Value = dtCurrentTable.Rows.Count.ToString();
                Gridview1.DataSource = dtCurrentTable;
                Gridview1.DataBind();
            }
        }
        else
        {
            Response.Write("ViewState is null");
        }

        //Set Previous Data on Postbacks
        SetPreviousData();
    }
    private void SetPreviousData()
    {
        int rowIndex = 0;
        if (ViewState["CurrentTable"] != null)
        {
            DataTable dt = (DataTable)ViewState["CurrentTable"];
            hdnGridview1.Value = dt.Rows.Count.ToString();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    TextBox Salutation = (TextBox)Gridview1.Rows[i].FindControl("Salutation");
                    TextBox FirstName = (TextBox)Gridview1.Rows[i].FindControl("FirstName");
                    TextBox lastname = (TextBox)Gridview1.Rows[i].FindControl("lastname");
                    TextBox EmailAddress = (TextBox)Gridview1.Rows[i].FindControl("EmailAddress");
                    TextBox WorkPhone = (TextBox)Gridview1.Rows[i].FindControl("WorkPhone");
                    TextBox Mobile = (TextBox)Gridview1.Rows[i].FindControl("Mobile");
                    TextBox SkypeName = (TextBox)Gridview1.Rows[i].FindControl("SkypeName");
                    TextBox Designation = (TextBox)Gridview1.Rows[i].FindControl("Designation");
                    TextBox Department = (TextBox)Gridview1.Rows[i].FindControl("Department");




                    Salutation.Text = dt.Rows[i]["Salutation"].ToString();
                    FirstName.Text = dt.Rows[i]["FirstName"].ToString();
                    lastname.Text = dt.Rows[i]["lastname"].ToString();
                    EmailAddress.Text = dt.Rows[i]["EmailAddress"].ToString();
                    WorkPhone.Text = dt.Rows[i]["WorkPhone"].ToString();
                    Mobile.Text = dt.Rows[i]["Mobile"].ToString();
                    SkypeName.Text = dt.Rows[i]["SkypeName"].ToString();
                    Designation.Text = dt.Rows[i]["Designation"].ToString();
                    Department.Text = dt.Rows[i]["Department"].ToString();



                }
            }
        }
    }


    protected void btnadd_Click(object sender, EventArgs e)
    {

        AddNewRowToGrid();

    }
    protected void btndelete_Click(object sender, EventArgs e)
    {

        LinkButton link = (LinkButton)Gridview1.FindControl("btndelete");
        link = (LinkButton)sender;
        GridViewRow gvr = (GridViewRow)link.NamingContainer;
        int rowIndex = 0;
        if (ViewState["CurrentTable"] != null)
        {

            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
            //   DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                {
                    //extract the TextBox values
                    TextBox Salutation = (TextBox)Gridview1.Rows[i].FindControl("Salutation");
                    TextBox FirstName = (TextBox)Gridview1.Rows[i].FindControl("FirstName");
                    TextBox lastname = (TextBox)Gridview1.Rows[i].FindControl("lastname");
                    TextBox EmailAddress = (TextBox)Gridview1.Rows[i].FindControl("EmailAddress");
                    TextBox WorkPhone = (TextBox)Gridview1.Rows[i].FindControl("WorkPhone");
                    TextBox Mobile = (TextBox)Gridview1.Rows[i].FindControl("Mobile");
                    TextBox SkypeName = (TextBox)Gridview1.Rows[i].FindControl("SkypeName");
                    TextBox Designation = (TextBox)Gridview1.Rows[i].FindControl("Designation");
                    TextBox Department = (TextBox)Gridview1.Rows[i].FindControl("Department");





                    dtCurrentTable.Rows[i]["id"] = i.ToString();

                    dtCurrentTable.Rows[i]["Salutation"] = Salutation.Text;

                    dtCurrentTable.Rows[i]["FirstName"] = FirstName.Text;

                    dtCurrentTable.Rows[i]["lastname"] = lastname.Text;


                    dtCurrentTable.Rows[i]["EmailAddress"] = EmailAddress.Text;

                    dtCurrentTable.Rows[i]["WorkPhone"] = WorkPhone.Text;

                    dtCurrentTable.Rows[i]["Mobile"] = Mobile.Text;

                    dtCurrentTable.Rows[i]["SkypeName"] = SkypeName.Text;

                    dtCurrentTable.Rows[i]["Designation"] = Designation.Text;
                    dtCurrentTable.Rows[i]["Department"] = Department.Text;

                }


                dtCurrentTable.Rows.RemoveAt(gvr.RowIndex);

                ViewState["CurrentTable"] = dtCurrentTable;
                hdngridviewrow.Value = dtCurrentTable.Rows.Count.ToString();
                Gridview1.DataSource = dtCurrentTable;
                Gridview1.DataBind();
                SetPreviousData();

            }
        }

    }


    protected void rowdatabind(object sender, GridViewRowEventArgs e)
    {

        LinkButton btddel = (LinkButton)e.Row.FindControl("btddel");
        Panel pnlshowlinkbutton = (Panel)e.Row.FindControl("pnlshowlinkbutton");





        if (btddel != null)
        {
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                if (dt.Rows.Count == 1)
                {
                    btddel.Visible = false;

                }
            }
            e.Row.Attributes.Add("onmouseover", "shoqlink('" + pnlshowlinkbutton.ClientID + "')");
            e.Row.Attributes.Add("onmouseout", "hidelink('" + pnlshowlinkbutton.ClientID + "')");






        }

        if (e.Row.RowType == DataControlRowType.Header)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].Text = fns.getltext(e.Row.Cells[i].Text, "2");
            }
        }
    }





    protected void bindmasterdata()
    {
        //string strdata = "select * from tbl_Currenciesmaster where status=1 and archive=0 order by Currency";
        //ds = da.GetDataSetInline(strdata);
        ////cmf.BindDropDownList(ddlcompid, ds, "comid", "id");
        ////ListItem li = new ListItem("", "0");
        ////ddlcompid.Items.Insert(0, li);
        //string
        //    strdatavalue = "";
        //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //{
        //    if (strdatavalue != string.Empty)
        //    {
        //        strdatavalue += "##" + ds.Tables[0].Rows[i]["Currency"].ToString() + " - " + ds.Tables[0].Rows[i]["curName"].ToString();
        //    }
        //    else
        //    {
        //        strdatavalue += ds.Tables[0].Rows[i]["Currency"].ToString() + " - " + ds.Tables[0].Rows[i]["curName"].ToString();
        //    }
        //}
        ////Response.Write(strdatavalue);
        //hdnCurrencyid.Value = strdatavalue;



        string strsql = "select tbl_Currenciesdetails.*,(tbl_Currenciesmaster.Currency+' - '+tbl_Currenciesmaster.curName) as curnamedetails from tbl_Currenciesdetails inner join tbl_Currenciesmaster on tbl_Currenciesdetails.curid=tbl_Currenciesmaster.id where tbl_Currenciesdetails.regid='" + Request.Cookies["cadmin"]["id"].ToString() + "' and tbl_Currenciesdetails.status=1 and tbl_Currenciesdetails.archive=0";
        DataSet ds = da.GetDataSetInline(strsql);
        cmf.BindDropDownList(txtCurrency, ds, "curnamedetails", "id");
        //ListItem li = new ListItem(" ", "0");
        //txtCurrency.Items.Insert(0, li);

        strsql = "select tbl_Currenciesdetails.*,(tbl_Currenciesmaster.Currency+' - '+tbl_Currenciesmaster.curName) as curnamedetails from tbl_Currenciesdetails inner join tbl_Currenciesmaster on tbl_Currenciesdetails.curid=tbl_Currenciesmaster.id where tbl_Currenciesdetails.regid='" + Request.Cookies["cadmin"]["id"].ToString() + "' and tbl_Currenciesdetails.status=1 and tbl_Currenciesdetails.archive=0 and isdefault=1";
        ds = da.GetDataSetInline(strsql);

        if (ds.Tables[0].Rows.Count > 0)
        {
            try
            {
                txtCurrency.SelectedValue = ds.Tables[0].Rows[0]["id"].ToString();
            }
            catch (Exception ex)
            {
            }
        }

        string
          strdatavalue = "";

        string strdata = "select * from tbl_Salutationnamemaster where status=1 and archive=0 and (regid='" + Request.Cookies["cadmin"]["id"] + "' or regid is null) order by id ";
        ds = da.GetDataSetInline(strdata);
        //cmf.BindDropDownList(ddlcompid, ds, "comid", "id");
        //ListItem li = new ListItem("", "0");
        //ddlcompid.Items.Insert(0, li);

        strdatavalue = "";
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            if (strdatavalue != string.Empty)
            {
                strdatavalue += "##" + ds.Tables[0].Rows[i]["Salutationname"].ToString();
            }
            else
            {
                strdatavalue += ds.Tables[0].Rows[i]["Salutationname"].ToString();
            }
        }
        //Response.Write(strdatavalue);
        hdnSalutation.Value = strdatavalue;







        if (fns.getcountryid() == 1)
        {
            strdata = "select * from tbl_VATTreatmentmaster where status=1 and archive=0 and isindia=1";
            ds = da.GetDataSetInline(strdata);
            cmf.BindDropDownList(VATTreatment, ds, "vname", "id");
            // VATTreatment.AutoPostBack = false;

        }
        else
        {
            if (fns.getcountryid() == 3)
            {
                strdata = "select * from tbl_VATTreatmentmaster where status=1 and archive=0 and istr=1";
                ds = da.GetDataSetInline(strdata);
                cmf.BindDropDownList(VATTreatment, ds, "vname", "id");
                // VATTreatment.AutoPostBack = false;

            }
            else
            {
                strdata = "select * from tbl_VATTreatmentmaster where status=1 and archive=0 and isoutside=1";
                ds = da.GetDataSetInline(strdata);
                cmf.BindDropDownList(VATTreatment, ds, "vname", "id");
            }

        }








        string strsqlpr = "select * from tbl_Preferencesitems where regid='" + Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0";
        DataSet dspr = da.GetDataSetInline(strsqlpr);
        if (dspr.Tables[0].Rows.Count > 0)
        {
            if (dspr.Tables[0].Rows[0]["inventrylevels"].ToString() == "3")
            {
                trcat.Style.Add("display", "block");
                trsubcat.Style.Add("display", "block");

                ViewState["trcat"] = "1";
                ViewState["trsubcat"] = "1";
                lblcatname.Text = dspr.Tables[0].Rows[0]["firstlevelname"].ToString();
                lblsubcatname.Text = dspr.Tables[0].Rows[0]["secondlevelname"].ToString();
                //  catname
                bindcat();
                bindsubcat();
            }
            else if (dspr.Tables[0].Rows[0]["inventrylevels"].ToString() == "2")
            {
                trcat.Style.Add("display", "block");
                ViewState["trcat"] = "1";
                ViewState["trsubcat"] = "0";
                lblcatname.Text = dspr.Tables[0].Rows[0]["firstlevelname"].ToString();
                bindcat();
            }
        }





    }



    protected bool checkvalidataion()
    {
        string strsql = "select * from tbl_contactsdetails where txtcompanyname='" + txtcompanyname.Value.Trim() + "' and status=1 and archive=0 and regid='" + Request.Cookies["cadmin"]["id"].ToString() + "' and ContactType='" + ContactType.SelectedValue + "'";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    protected bool checkvalidataionafter()
    {
        string strsql = "select * from tbl_contactsdetails where txtcompanyname='" + txtcompanyname.Value.Trim() + "' and id!='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "' and status=1 and archive=0 and regid='" + Request.Cookies["cadmin"]["id"].ToString() + "' and ContactType='" + ContactType.SelectedValue + "'";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    protected void bindemp()
    {
        string strsql = "select * from tbl_employee where status=1 and archive=0 and regid='" + Request.Cookies["cadmin"]["id"].ToString() + "'";
        DataSet ds = da.GetDataSetInline(strsql);
        cmf.BindDropDownList(ddlemployee, ds, "DisplayName", "id");
        ListItem li = new ListItem("--select Employee--", "0");
        ddlemployee.Items.Insert(0, li);
    }

    protected void btnsave_click(object sender, EventArgs e)
    {

        if (btnsave.Text == "Update")
        {


            if (checkvalidataionafter())
            {
                SqlParameter[] prm = new SqlParameter[50];
                prm[0] = new SqlParameter("@id", cmf.Decrypt(Request.QueryString["id"].ToString()));
                prm[1] = new SqlParameter("@ContactType", ContactType.SelectedValue);
                prm[2] = new SqlParameter("@cdate", da.getcdatetime());
                prm[3] = new SqlParameter("@uid", Request.Cookies["user"]["user_id"].ToString());
                prm[4] = new SqlParameter("@txtsalutation", txtsalutation.Value.Trim());
                prm[5] = new SqlParameter("@txtfirstname", txtfirstname.Value.Trim());
                prm[6] = new SqlParameter("@txtlastname", txtlastname.Value.Trim());
                prm[7] = new SqlParameter("@txtcompanyname", txtcompanyname.Value.Trim());
                prm[8] = new SqlParameter("@txtcontactdisplayname", txtcompanyname.Value.Trim());
                prm[9] = new SqlParameter("@txtContactEmail", txtContactEmail.Text.Trim());
                prm[10] = new SqlParameter("@txtworkphonePhone", txtworkphonePhone.Value.Trim());
                prm[11] = new SqlParameter("@txtmobile", txtmobile.Value.Trim());
                prm[12] = new SqlParameter("@txtSkypeName", txtSkypeName.Value.Trim());
                prm[13] = new SqlParameter("@txtDesignation", txtDesignation.Value.Trim());
                prm[14] = new SqlParameter("@txtDepartment", txtDepartment.Value.Trim());
                prm[15] = new SqlParameter("@Website", Website.Value.Trim());
                prm[16] = new SqlParameter("@VATTreatment", VATTreatment.SelectedValue);
                prm[17] = new SqlParameter("@TaxRegistrationNumber", TaxRegistrationNumber.Value.Trim());
                prm[18] = new SqlParameter("@PlaceOfSupply", ddlplaceofsupply.SelectedValue.Trim());
                prm[19] = new SqlParameter("@txtCurrency", txtCurrency.SelectedValue);
                prm[20] = new SqlParameter("@PaymentTerms", PaymentTerms.Value.Trim());
                prm[21] = new SqlParameter("@isEnablePortal", "0");
                prm[22] = new SqlParameter("@PortalLanguage", "0");
                prm[23] = new SqlParameter("@Facebook", Facebook.Value.Trim());
                prm[24] = new SqlParameter("@Twitter", Twitter.Value.Trim());
                prm[25] = new SqlParameter("@billAttention", billAttention.Value.Trim());
                prm[26] = new SqlParameter("@billAddress1", billAddress1.Value.Trim());
                prm[27] = new SqlParameter("@billAddress2", billAddress2.Value.Trim());
                prm[28] = new SqlParameter("@billCity", billCity.Value.Trim());
                prm[29] = new SqlParameter("@billState", billState.Value.Trim());
                prm[30] = new SqlParameter("@billZipCode", billZipCode.Value.Trim());
                prm[31] = new SqlParameter("@billCountry", billCountry.Value.Trim());
                prm[32] = new SqlParameter("@billPhone", billPhone.Value.Trim());
                prm[33] = new SqlParameter("@billFax", billFax.Value.Trim());
                prm[34] = new SqlParameter("@ShipAttention", ShipAttention.Value.Trim());
                prm[35] = new SqlParameter("@ShipAddress1", ShipAddress1.Value.Trim());
                prm[36] = new SqlParameter("@ShipAddress2", ShipAddress2.Value.Trim());
                prm[37] = new SqlParameter("@ShipCity", ShipCity.Value.Trim());
                prm[38] = new SqlParameter("@ShipState", ShipState.Value.Trim());
                prm[39] = new SqlParameter("@ShipZipCode", ShipZipCode.Value.Trim());
                prm[40] = new SqlParameter("@ShipCountry", ShipCountry.Value.Trim());
                prm[41] = new SqlParameter("@ShipPhone", ShipPhone.Value.Trim());
                prm[42] = new SqlParameter("@ShipFax", ShipFax.Value.Trim());
                prm[43] = new SqlParameter("@remarks", remarks.Value.Trim());
                prm[44] = new SqlParameter("ReturnValue", SqlDbType.Int);
                prm[44].Direction = ParameterDirection.ReturnValue;
                if (txtcreditlimit.Text != string.Empty)
                {
                    prm[45] = new SqlParameter("@txtcreditlimit", txtcreditlimit.Text.Trim());
                }
                else
                {
                    prm[45] = new SqlParameter("@txtcreditlimit", DBNull.Value);
                }
                if (txtcreditperiod.Text != string.Empty)
                {
                    prm[46] = new SqlParameter("@txtcreditperiod", txtcreditperiod.Text.Trim());
                }
                else
                {
                    prm[46] = new SqlParameter("@txtcreditperiod", DBNull.Value);
                }

                prm[47] = new SqlParameter("@vcode", txtvcode.Value.Trim());

                prm[48] = new SqlParameter("@billgst", billgst.Value.Trim());
                prm[49] = new SqlParameter("@Shipgst", Shipgst.Value.Trim());

                int cusid = da.executeDMLQuery("tbl_contactsdetailsupdate_creditlimit", prm);
                try
                {
                    string strs = "update tbl_contactsdetails set txtDLN='" + txtDLN.Value + "' where id='" + cusid + "'";
                    da.executeDMLQueryinline(strs);
                }
                catch { }


                prm = new SqlParameter[2];
                prm[0] = new SqlParameter("@groupname", txtcompanyname.Value.Trim());
                prm[1] = new SqlParameter("@refid", cusid);
                da.executeDMLQuery("tbl_ledgersupdatecontact", prm);



                string strsql = "delete from tbl_contactcontactpersondetails where contactid='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "'";
                da.executeDMLQueryinline(strsql);

                cusid = Convert.ToInt32(cmf.Decrypt(Request.QueryString["id"].ToString()));
                for (int i = 0; i < Gridview1.Rows.Count; i++)
                {
                    int rowIndex = i;
                    TextBox Salutation = (TextBox)Gridview1.Rows[rowIndex].FindControl("Salutation");
                    TextBox FirstName = (TextBox)Gridview1.Rows[rowIndex].FindControl("FirstName");
                    TextBox lastname = (TextBox)Gridview1.Rows[rowIndex].FindControl("lastname");
                    TextBox EmailAddress = (TextBox)Gridview1.Rows[rowIndex].FindControl("EmailAddress");
                    TextBox WorkPhone = (TextBox)Gridview1.Rows[rowIndex].FindControl("WorkPhone");
                    TextBox Mobile = (TextBox)Gridview1.Rows[rowIndex].FindControl("Mobile");
                    TextBox SkypeName = (TextBox)Gridview1.Rows[rowIndex].FindControl("SkypeName");
                    TextBox Designation = (TextBox)Gridview1.Rows[rowIndex].FindControl("Designation");
                    TextBox Department = (TextBox)Gridview1.Rows[rowIndex].FindControl("Department");

                    if (Salutation.Text != string.Empty && FirstName.Text != string.Empty)
                    {
                        SqlParameter[] prmadd = new SqlParameter[10];


                        prmadd[0] = new SqlParameter("@contactid", cusid);
                        prmadd[1] = new SqlParameter("@Salutation", Salutation.Text.Trim());
                        prmadd[2] = new SqlParameter("@FirstName", FirstName.Text.Trim());
                        prmadd[3] = new SqlParameter("@lastname", lastname.Text.Trim());
                        prmadd[4] = new SqlParameter("@EmailAddress", EmailAddress.Text.Trim());
                        prmadd[5] = new SqlParameter("@WorkPhone", WorkPhone.Text.Trim());
                        prmadd[6] = new SqlParameter("@Mobile", Mobile.Text.Trim());
                        prmadd[7] = new SqlParameter("@SkypeName", SkypeName.Text.Trim());
                        prmadd[8] = new SqlParameter("@Designation", Designation.Text.Trim());
                        prmadd[9] = new SqlParameter("@Department", Department.Text.Trim());
                        da.executeDMLQuery("tbl_contactcontactpersondetailsinsert", prmadd);
                    }

                }

                //string strsql111 = "delete from tbl_contactpriceslist where sid='" + cmf.Decrypt(Request.QueryString["id"].ToString()) + "'";
                //da.executeDMLQueryinline(strsql111);

                for (int i = 0; i < GrdvDepartment.Rows.Count; i++)
                {
                    int rowIndex = i;
                    TextBox txtprice = (TextBox)GrdvDepartment.Rows[rowIndex].FindControl("txtprice");
                    HiddenField hdnitemid = (HiddenField)GrdvDepartment.Rows[rowIndex].FindControl("hdnitemid");





                    if (txtprice.Text != string.Empty)
                    {
                        SqlParameter[] prmadd = new SqlParameter[3];
                        prmadd[0] = new SqlParameter("@sid", cmf.Decrypt(Request.QueryString["id"].ToString()));
                        prmadd[1] = new SqlParameter("@itemid", hdnitemid.Value);
                        prmadd[2] = new SqlParameter("@price", txtprice.Text.Trim());
                        da.executeDMLQuery("tbl_contactpriceslistinsert", prmadd);
                    }

                }


                strsql = "select tbl_customefielddetails.*,tbl_cusdatatypemaster.cusdatatype from tbl_customefielddetails inner join tbl_cusdatatypemaster  on  tbl_customefielddetails.labeldatatype=tbl_cusdatatypemaster.id  where tbl_customefielddetails.regid='" + Request.Cookies["cadmin"]["id"] + "' and tbl_customefielddetails.status=1 and tbl_customefielddetails.archive=0 and tbl_customefielddetails.typeid=2";
                DataSet ds = da.GetDataSetInline(strsql);

                int counter = 1; // this will be used to control naming  
                // create a table to store 
                for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
                {
                    int y = Convert.ToInt32(ds.Tables[0].Rows[x]["labeldatatype"].ToString());
                    string controlid = ds.Tables[0].Rows[x]["id"].ToString();
                    string controlvalue = "";


                    switch (y)
                    {
                        case 1:
                            txtText = (TextBox)pnlcustomControl.FindControl("ctlcust" + Convert.ToString(counter));
                            controlvalue = txtText.Text;
                            break;
                        case 2:
                            Chkbox = (CheckBox)pnlcustomControl.FindControl("ctlcust" + Convert.ToString(counter));
                            if (Chkbox.Checked)
                            {

                                controlvalue = "1";
                            }
                            break;
                        case 3:
                            dropdown = (DropDownList)pnlcustomControl.FindControl("ctlcust" + Convert.ToString(counter));
                            controlvalue = dropdown.SelectedValue;
                            break;
                        default:
                            break;
                    }
                    counter++;
                    prm = new SqlParameter[3];
                    prm[0] = new SqlParameter("@uid", cusid);
                    prm[1] = new SqlParameter("@controlid", controlid);
                    prm[2] = new SqlParameter("@controlvalue", controlvalue);
                    da.executeDMLQuery("tbl_customercustomefieldvalueinsertupdate", prm);

                }



                Page.RegisterClientScriptBlock("js", "<script>alert('Data Updated Sucessfully!!!!!!');document.location.href = 'allcontact.aspx';</script>");



            }
            else
            {
                Page.RegisterClientScriptBlock("js", "<script>alert('This Company Name Is Already Exits');</script>");
            }




        }
        else
        {
            if (checkvalidataion())
            {
                if (checkvalidataiongst())
                {
                    int ismultycompany = 0;
                    string ismaincid = Request.Cookies["cadmin"]["id"].ToString();
                    int issamesup = 0;
                    dataaccesscomm dacomm = new dataaccesscomm();
                    dataaccessruntime dacommruntime = new dataaccessruntime();
                    string databasename = "";
                    string strsqladmin = "select * from tbl_registrationdetails where id='" + Request.Cookies["cadmin"]["id"].ToString() + "' and status=1 and archive=0";
                    DataSet dsismulti = dacomm.GetDataSetInline(strsqladmin);
                    
                    //if (dsismulti.Tables[0].Rows.Count > 0)
                    //{
                       
                    //    if (dsismulti.Tables[0].Rows[0]["ismulticompany"].ToString() == "1")
                    //    {
                    //        //Response.Write(dsismulti.Tables[0].Rows[0]["databasename"].ToString());
                    //        //Response.End();
                    //        ismultycompany = 1;
                    //        ismaincid = dsismulti.Tables[0].Rows[0]["id"].ToString();
                    //        databasename = "0";

                    //    }
                    //    else
                    //    {
                    //        if (dsismulti.Tables[0].Rows[0]["pid"].ToString() != string.Empty && dsismulti.Tables[0].Rows[0]["pid"].ToString() != "0")
                    //        {

                    //            ismultycompany = 1;
                    //            ismaincid = dsismulti.Tables[0].Rows[0]["pid"].ToString();
                    //            strsqladmin = "select * from tbl_registrationdetails where id='" + ismaincid + "' and status=1 and archive=0";
                    //            DataSet dsismulti1 = dacomm.GetDataSetInline(strsqladmin);
                    //            // Response.Write(strsqladmin);
                    //            if (dsismulti1.Tables[0].Rows.Count > 0)
                    //            {

                    //                databasename = dsismulti1.Tables[0].Rows[0]["databasename"].ToString();
                    //            }

                    //        }
                    //    }
                    //}

                    //if (ismultycompany == 1)
                    //{

                    //    if (databasename != string.Empty)
                    //    {
                    //        string strsqlbb = "select * from tbl_Preferencescontact where regid='" + ismaincid + "' and status=1 and archive=0";

                    //        DataSet dsrun = dacommruntime.GetDataSetInline(strsqlbb, databasename);
                    //        if (dsrun.Tables[0].Rows.Count > 0)
                    //        {
                    //            if (dsrun.Tables[0].Rows[0]["ddlsameSupplier"].ToString() == "2")
                    //            {
                    //                issamesup = 1;
                    //            }
                    //        }
                    //    }


                    //}

                    string ismainregid = Request.Cookies["cadmin"]["id"].ToString();

                    string databasenamemain = "";


                    if (issamesup == 1)
                    {

                        ismainregid = ismaincid;
                        string strsqladmins = "select * from tbl_registrationdetails where id='" + ismainregid + "' and status=1 and archive=0";
                        DataSet dsismultim = dacomm.GetDataSetInline(strsqladmins);
                        // Response.Write(strsqladmin);
                        if (dsismultim.Tables[0].Rows.Count > 0)
                        {
                            databasenamemain = dsismultim.Tables[0].Rows[0]["databasename"].ToString();
                        }


                    }
                    if (ContactType.SelectedValue == "1")
                    {
                        ismainregid = Request.Cookies["cadmin"]["id"].ToString();
                    }


                    SqlParameter[] prm = new SqlParameter[70];
                    prm[0] = new SqlParameter("@regid", ismainregid);
                    prm[1] = new SqlParameter("@ContactType", ContactType.SelectedValue);
                    prm[2] = new SqlParameter("@cdate", da.getcdatetime());
                    prm[3] = new SqlParameter("@uid", Request.Cookies["user"]["user_id"].ToString());
                    prm[4] = new SqlParameter("@txtsalutation", txtsalutation.Value.Trim());
                    prm[5] = new SqlParameter("@txtfirstname", txtfirstname.Value.Trim());
                    prm[6] = new SqlParameter("@txtlastname", txtlastname.Value.Trim());
                    prm[7] = new SqlParameter("@txtcompanyname", txtcompanyname.Value.Trim());
                    prm[8] = new SqlParameter("@txtcontactdisplayname", txtcompanyname.Value.Trim());
                    prm[9] = new SqlParameter("@txtContactEmail", txtContactEmail.Text.Trim());
                    prm[10] = new SqlParameter("@txtworkphonePhone", txtworkphonePhone.Value.Trim());
                    prm[11] = new SqlParameter("@txtmobile", txtmobile.Value.Trim());
                    prm[12] = new SqlParameter("@txtSkypeName", txtSkypeName.Value.Trim());
                    prm[13] = new SqlParameter("@txtDesignation", txtDesignation.Value.Trim());
                    prm[14] = new SqlParameter("@txtDepartment", txtDepartment.Value.Trim());
                    prm[15] = new SqlParameter("@Website", Website.Value.Trim());
                    prm[16] = new SqlParameter("@VATTreatment", VATTreatment.SelectedValue);
                    prm[17] = new SqlParameter("@TaxRegistrationNumber", TaxRegistrationNumber.Value.Trim());
                    prm[18] = new SqlParameter("@PlaceOfSupply", ddlplaceofsupply.SelectedValue);
                    prm[19] = new SqlParameter("@txtCurrency", txtCurrency.SelectedValue);
                    prm[20] = new SqlParameter("@PaymentTerms", PaymentTerms.Value.Trim());
                    prm[21] = new SqlParameter("@isEnablePortal", "0");
                    prm[22] = new SqlParameter("@PortalLanguage", "0");
                    prm[23] = new SqlParameter("@Facebook", Facebook.Value.Trim());
                    prm[24] = new SqlParameter("@Twitter", Twitter.Value.Trim());
                    prm[25] = new SqlParameter("@billAttention", billAttention.Value.Trim());
                    prm[26] = new SqlParameter("@billAddress1", billAddress1.Value.Trim());
                    prm[27] = new SqlParameter("@billAddress2", billAddress2.Value.Trim());
                    prm[28] = new SqlParameter("@billCity", billCity.Value.Trim());
                    prm[29] = new SqlParameter("@billState", billState.Value.Trim());
                    prm[30] = new SqlParameter("@billZipCode", billZipCode.Value.Trim());
                    prm[31] = new SqlParameter("@billCountry", billCountry.Value.Trim());
                    prm[32] = new SqlParameter("@billPhone", billPhone.Value.Trim());
                    prm[33] = new SqlParameter("@billFax", billFax.Value.Trim());
                    prm[34] = new SqlParameter("@ShipAttention", ShipAttention.Value.Trim());
                    prm[35] = new SqlParameter("@ShipAddress1", ShipAddress1.Value.Trim());
                    prm[36] = new SqlParameter("@ShipAddress2", ShipAddress2.Value.Trim());
                    prm[37] = new SqlParameter("@ShipCity", ShipCity.Value.Trim());
                    prm[38] = new SqlParameter("@ShipState", ShipState.Value.Trim());
                    prm[39] = new SqlParameter("@ShipZipCode", ShipZipCode.Value.Trim());
                    prm[40] = new SqlParameter("@ShipCountry", ShipCountry.Value.Trim());
                    prm[41] = new SqlParameter("@ShipPhone", ShipPhone.Value.Trim());
                    prm[42] = new SqlParameter("@ShipFax", ShipFax.Value.Trim());
                    prm[43] = new SqlParameter("@remarks", remarks.Value.Trim());
                    prm[44] = new SqlParameter("ReturnValue", SqlDbType.Int);
                    prm[44].Direction = ParameterDirection.ReturnValue;
                    if (txtcreditlimit.Text != string.Empty)
                    {
                        prm[45] = new SqlParameter("@txtcreditlimit", txtcreditlimit.Text.Trim());
                    }
                    else
                    {
                        prm[45] = new SqlParameter("@txtcreditlimit", DBNull.Value);
                    }
                    if (txtcreditperiod.Text != string.Empty)
                    {
                        prm[46] = new SqlParameter("@txtcreditperiod", txtcreditperiod.Text.Trim());
                    }
                    else
                    {
                        prm[46] = new SqlParameter("@txtcreditperiod", DBNull.Value);
                    }
                    prm[47] = new SqlParameter("@PaymentTermsdisplay", PaymentTermsdisplay.Value.Trim());
                    prm[48] = new SqlParameter("@txtbankname", txtbankname.Text);
                    prm[49] = new SqlParameter("@txtaccountno", txtaccountno.Text);
                    prm[50] = new SqlParameter("@txtifccode", txtifccode.Text);
                    prm[51] = new SqlParameter("@txtcorporateid", txtcorporateid.Text);
                    prm[52] = new SqlParameter("@txtbranch", txtbranch.Text);
                    prm[53] = new SqlParameter("@txtbranchaddress", txtbranchaddress.Text);
                    prm[54] = new SqlParameter("@txtbenificiaryemail", txtbenificiaryemail.Text);
                    prm[55] = new SqlParameter("@txtcinno", txtcinno.Value);
                    prm[56] = new SqlParameter("@txtmodeofpayment", txtmodeofpayment.Value);
                    prm[57] = new SqlParameter("@txtfreightterms", txtfreightterms.Value);
                    prm[58] = new SqlParameter("@txtdeliverytype", txtdeliverytype.Value);
                    prm[59] = new SqlParameter("@Txttransport", Txttransport.Value);
                    prm[60] = new SqlParameter("@txtSplInstructoin", txtSplInstructoin.Value);
                    prm[61] = new SqlParameter("@txtsalevalueprvyear", txtsalevalueprvyear.Value);
                    prm[62] = new SqlParameter("@ceoname", ceoname.Value);
                    prm[63] = new SqlParameter("@ceoContact", ceoContact.Value);
                    prm[64] = new SqlParameter("@ceoEmail", ceoEmail.Value);

                    prm[65] = new SqlParameter("@vcode", txtvcode.Value.Trim());
                    prm[66] = new SqlParameter("@billgst", billgst.Value.Trim());
                    prm[67] = new SqlParameter("@Shipgst", Shipgst.Value.Trim());
                    prm[68] = new SqlParameter("@txtcity", txtcity.Text.Trim());
                    prm[69] = new SqlParameter("@txtRegion", txtRegion.Text.Trim());
                    int cusid = 0;
                    if (issamesup == 1)
                    {
                        if (ContactType.SelectedValue == "2" || ContactType.SelectedValue == "1")
                        {
                            cusid = dacommruntime.executeDMLQuery("tbl_contactsdetailsinsert_creditlimit", prm, databasenamemain);
                        }
                        else
                        {
                            cusid = da.executeDMLQuery("tbl_contactsdetailsinsert_creditlimit", prm);
                        }
                    }
                    else
                    {
                        cusid = da.executeDMLQuery("tbl_contactsdetailsinsert_creditlimit", prm);
                    }

                    try
                    {
                        string strs = "update tbl_contactsdetails set txtDLN='" + txtDLN.Value + "' where id='" + cusid + "'";
                        da.executeDMLQueryinline(strs);
                    }
                    catch { }




                    if (Request.Cookies["cadmin"]["id"].ToString() == "2318")
                    {
                        string isCusType = "";
                        if (ContactType.SelectedValue == "2")
                        {
                            isCusType = "SUPP";
                        }
                        else
                        {
                            isCusType = "CUST";
                        }
                        string itemcodeValue = "";
                        string currentYear = "";

                        if (txtcompanyname.Value != string.Empty)
                        {
                            String x = txtcompanyname.Value.Replace(" - ", " ").Replace("-", " ").Replace(".", " ").Replace("/", " ").Replace("&", " ").Replace("And", " ").Trim();
                            String[] nameparts = x.Split(' ');
                            string a = "";
                            for (int i = 0; i <= nameparts.Length; i++)
                            {
                                if (i <= 1)
                                {
                                    try
                                    {
                                        if (a != string.Empty)
                                        {
                                            a += "" + nameparts[i].ToString().Substring(0, 1).ToUpper();
                                        }
                                        else
                                        {
                                            a = nameparts[i].ToString().Substring(0, 1).ToUpper();
                                        }
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                            itemcodeValue += a;
                        }
                        currentYear = DateTime.Now.Year.ToString();
                        string strseries = "select * from tbl_series where prefix='" + isCusType.Trim() + "'";
                        DataSet dsseries = da.GetDataSetInline(strseries);
                        if (dsseries.Tables[0].Rows.Count > 0)
                        {
                            string vendorCode = isCusType + "" + currentYear.ToString().Replace("20", "") + "" + itemcodeValue.Trim() + "" + dsseries.Tables[0].Rows[0]["NextNumber"].ToString();
                            string strupdatevcode = "update tbl_contactsdetails set vcode='" + vendorCode + "' where id='" + cusid + "'";
                            da.executeDMLQueryinline(strupdatevcode);
                            SqlParameter[] prmSe = new SqlParameter[1];
                            prmSe[0] = new SqlParameter("@prefix", isCusType.Trim());
                            da.executeDMLQuery("tbl_seriesUpdate", prmSe);
                        }
                    }


                    if (isRevelex == 1) //Revelex
                    {
                        string isCusType = "";
                        //if (ContactType.SelectedValue == "2")
                        //{
                        //    isCusType = "SUPP";
                        //}
                        //else 
                        //{
                        //    isCusType = "CUST";
                        //}
                        if (ContactType.SelectedValue == "2")
                        {
                            isCusType = "S";
                        }
                        else
                        {
                            isCusType = "C";
                        }
                        string firsCharacter = txtfirstname.Value.ToString().Substring(0, 4).ToUpper();
                        string strseries = "select * from tbl_series where prefix='" + isCusType.Trim() + "'";
                        DataSet dsseries = da.GetDataSetInline(strseries);
                        if (dsseries.Tables[0].Rows.Count > 0)
                        {
                            string vendorCode = firsCharacter.Trim() + "_" + dsseries.Tables[0].Rows[0]["NextNumber"].ToString();
                            string strupdatevcode = "update tbl_contactsdetails set vcode='" + vendorCode + "' where id='" + cusid + "'";
                            da.executeDMLQueryinline(strupdatevcode);
                            SqlParameter[] prmSe = new SqlParameter[1];
                            prmSe[0] = new SqlParameter("@prefix", isCusType.Trim());
                            da.executeDMLQuery("tbl_seriesUpdate", prmSe);
                        }
                    }

                    //if (Request.Cookies["cadmin"]["id"].ToString() == "2364")
                    //{
                    //    string firsCharacter = "";
                    //    if (ContactType.SelectedValue == "2")
                    //    {
                    //        firsCharacter = "AAAV";
                    //        string strseries = "select * from tbl_series where prefix='AAAV'";
                    //        DataSet dsseries = da.GetDataSetInline(strseries);
                    //        if (dsseries.Tables[0].Rows.Count > 0)
                    //        {
                    //            string vendorCode = firsCharacter.Trim() + "" + dsseries.Tables[0].Rows[0]["NextNumber"].ToString();
                    //            string strupdatevcode = "update tbl_contactsdetails set vcode='" + vendorCode + "' where id='" + cusid + "'";
                    //            da.executeDMLQueryinline(strupdatevcode);
                    //            SqlParameter[] prmSe = new SqlParameter[1];
                    //            prmSe[0] = new SqlParameter("@prefix", "AAAV");
                    //            da.executeDMLQuery("tbl_seriesUpdate", prmSe);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        firsCharacter = "AAA/C";
                    //        string strseries = "select * from tbl_series where prefix='AAA'";
                    //        DataSet dsseries = da.GetDataSetInline(strseries);
                    //        if (dsseries.Tables[0].Rows.Count > 0)
                    //        {
                    //            string vendorCode = firsCharacter.Trim() + "" + dsseries.Tables[0].Rows[0]["NextNumber"].ToString();
                    //            string strupdatevcode = "update tbl_contactsdetails set vcode='" + vendorCode + "' where id='" + cusid + "'";
                    //            da.executeDMLQueryinline(strupdatevcode);
                    //            SqlParameter[] prmSe = new SqlParameter[1];
                    //            prmSe[0] = new SqlParameter("@prefix", "AAA");
                    //            da.executeDMLQuery("tbl_seriesUpdate", prmSe);
                    //        }

                    //    }
                    //}
                    if (Request.Cookies["cadmin"]["id"].ToString() == "2292")
                    {
                        string firsCharacter = txtcompanyname.Value.ToString().Substring(0, 1).ToUpper();
                        string strseries = "select * from tbl_series where prefix='" + firsCharacter.Trim() + "'";
                        DataSet dsseries = da.GetDataSetInline(strseries);
                        if (dsseries.Tables[0].Rows.Count > 0)
                        {
                            string vendorCode = firsCharacter.Trim() + "" + dsseries.Tables[0].Rows[0]["NextNumber"].ToString();
                            string strupdatevcode = "update tbl_contactsdetails set vcode='" + vendorCode + "' where id='" + cusid + "'";
                            da.executeDMLQueryinline(strupdatevcode);
                            SqlParameter[] prmSe = new SqlParameter[1];
                            prmSe[0] = new SqlParameter("@prefix", firsCharacter.Trim());
                            da.executeDMLQuery("tbl_seriesUpdate", prmSe);
                        }
                    }

                    if (Request.Cookies["cadmin"]["id"].ToString() == "2351")
                    {
                        if (ContactType.SelectedValue == "1")
                        {
                            string itemcodeValue = "";
                            if (txtcompanyname.Value != string.Empty)
                            {
                                String x = txtcompanyname.Value.Trim();
                                String[] nameparts = x.Split(' ');
                                string a = "";
                                for (int i = 0; i < nameparts.Length; i++)
                                {
                                    try
                                    {
                                        if (a != string.Empty)
                                        {
                                            a += "" + nameparts[i].ToString().Substring(0, 1).ToUpper();
                                        }
                                        else
                                        {
                                            a = nameparts[i].ToString().Substring(0, 1).ToUpper();
                                        }
                                    }
                                    catch
                                    {
                                    }
                                }
                                itemcodeValue += a;
                            }
                            string strseries = "select * from tbl_series where prefix='Vendor'";
                            DataSet dsseries = da.GetDataSetInline(strseries);
                            if (dsseries.Tables[0].Rows.Count > 0)
                            {
                                string vendorCode = itemcodeValue.Trim() + "" + dsseries.Tables[0].Rows[0]["NextNumber"].ToString();
                                string strupdatevcode = "update tbl_contactsdetails set vcode='" + vendorCode + "' where id='" + cusid + "'";
                                da.executeDMLQueryinline(strupdatevcode);
                                SqlParameter[] prmSe = new SqlParameter[1];
                                prmSe[0] = new SqlParameter("@prefix", "Vendor");
                                da.executeDMLQuery("tbl_seriesUpdate", prmSe);
                            }
                        }
                    }
                    try
                    {

                        string cuspricetype = "0";
                        try
                        {
                            cuspricetype = ddlcustomertype.SelectedValue;
                        }
                        catch
                        {


                        }
                        string sectionid = "0";
                        try
                        {
                            if (istds.SelectedValue == "1")
                            {
                                if (ddlsectionname.SelectedValue == "-1")
                                {
                                    SqlParameter[] prmsec = new SqlParameter[4];
                                    prmsec[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"].ToString());
                                    prmsec[1] = new SqlParameter("@sectionname", txtsectionname.Text.Trim());
                                    prmsec[2] = new SqlParameter("ReturnValue", SqlDbType.Int);
                                    prmsec[2].Direction = ParameterDirection.ReturnValue;
                                    prmsec[3] = new SqlParameter("@tdsper", testper.Text.Trim());
                                    sectionid = da.executeDMLQuery("tbl_tdssectionnameinsert", prmsec).ToString();
                                }
                                else
                                    sectionid = ddlsectionname.SelectedValue;
                            }
                        }
                        catch
                        {

                        }
                        string strsqluptcs = "update tbl_contactsdetails set txtpterms='" + txtpterms.Value + "',rdltcs='" + rdltcs.SelectedValue + "',cuspricetype='" + cuspricetype + "',panno='" + panno.Value.Trim() + "',Tanno='" + Tanno.Value.Trim() + "',istds='" + istds.SelectedValue + "',sectionid='" + sectionid + "',salepersonid='" + ddlemployee.SelectedValue + "' where id='" + cusid + "'";
                        if (issamesup == 1)
                        {
                            if (ContactType.SelectedValue == "2" || ContactType.SelectedValue == "1")
                            {
                                dacommruntime.executeDMLQueryinline(strsqluptcs, databasenamemain);
                            }
                            else
                            {
                                da.executeDMLQueryinline(strsqluptcs);
                            }
                        }
                        else
                        {
                            da.executeDMLQueryinline(strsqluptcs);
                        }



                    }
                    catch
                    {

                    }


                    if (Request.QueryString["isledgernot"] != null)
                    {
                    }
                    else
                    {

                        prm = new SqlParameter[8];
                        prm[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"]);
                        prm[1] = new SqlParameter("@cdate", da.getcdatetimenew());
                        prm[2] = new SqlParameter("@groupname", txtcompanyname.Value.Trim());
                        prm[3] = new SqlParameter("@pid", txtunder.SelectedValue);

                        //if (ContactType.SelectedValue == "1")
                        //{

                        //    prm[3] = new SqlParameter("@pid", "26");
                        //}
                        //else
                        //{
                        //    prm[3] = new SqlParameter("@pid", "22");
                        //}
                        prm[4] = new SqlParameter("@openingval", "0");
                        prm[5] = new SqlParameter("@typeid", "1");
                        prm[6] = new SqlParameter("@refid", cusid);
                        prm[7] = new SqlParameter("@entrytype", "1");
                        if (issamesup == 1)
                        {
                            if (ContactType.SelectedValue == "2" || ContactType.SelectedValue == "1")
                            {
                                dacommruntime.executeDMLQuery("tbl_ledgersinsertcontact_entertype", prm, databasenamemain);
                            }
                            else
                            {
                                da.executeDMLQuery("tbl_ledgersinsertcontact_entertype", prm);
                            }
                        }
                        else
                        {
                            //if (Request.QueryString["rtype"] != null)
                            //{

                            da.executeDMLQuery("tbl_ledgersinsertcontact_entertype", prm);
                            // }

                        }
                    }


                    for (int i = 0; i < Gridview1.Rows.Count; i++)
                    {
                        int rowIndex = i;
                        TextBox Salutation = (TextBox)Gridview1.Rows[rowIndex].FindControl("Salutation");
                        TextBox FirstName = (TextBox)Gridview1.Rows[rowIndex].FindControl("FirstName");
                        TextBox lastname = (TextBox)Gridview1.Rows[rowIndex].FindControl("lastname");
                        TextBox EmailAddress = (TextBox)Gridview1.Rows[rowIndex].FindControl("EmailAddress");
                        TextBox WorkPhone = (TextBox)Gridview1.Rows[rowIndex].FindControl("WorkPhone");
                        TextBox Mobile = (TextBox)Gridview1.Rows[rowIndex].FindControl("Mobile");
                        TextBox SkypeName = (TextBox)Gridview1.Rows[rowIndex].FindControl("SkypeName");
                        TextBox Designation = (TextBox)Gridview1.Rows[rowIndex].FindControl("Designation");
                        TextBox Department = (TextBox)Gridview1.Rows[rowIndex].FindControl("Department");

                        if (Salutation.Text != string.Empty && FirstName.Text != string.Empty)
                        {
                            SqlParameter[] prmadd = new SqlParameter[10];


                            prmadd[0] = new SqlParameter("@contactid", cusid);
                            prmadd[1] = new SqlParameter("@Salutation", Salutation.Text.Trim());
                            prmadd[2] = new SqlParameter("@FirstName", FirstName.Text.Trim());
                            prmadd[3] = new SqlParameter("@lastname", lastname.Text.Trim());
                            prmadd[4] = new SqlParameter("@EmailAddress", EmailAddress.Text.Trim());
                            prmadd[5] = new SqlParameter("@WorkPhone", WorkPhone.Text.Trim());
                            prmadd[6] = new SqlParameter("@Mobile", Mobile.Text.Trim());
                            prmadd[7] = new SqlParameter("@SkypeName", SkypeName.Text.Trim());
                            prmadd[8] = new SqlParameter("@Designation", Designation.Text.Trim());
                            prmadd[9] = new SqlParameter("@Department", Department.Text.Trim());
                            if (issamesup == 1)
                            {
                                if (ContactType.SelectedValue == "2" || ContactType.SelectedValue == "1")
                                {
                                    dacommruntime.executeDMLQuery("tbl_contactcontactpersondetailsinsert", prmadd, databasenamemain);

                                }
                                else
                                {
                                    da.executeDMLQuery("tbl_contactcontactpersondetailsinsert", prmadd);
                                }
                            }
                            else
                            {
                                da.executeDMLQuery("tbl_contactcontactpersondetailsinsert", prmadd);
                            }
                        }

                    }


                    for (int i = 0; i < GrdvDepartment.Rows.Count; i++)
                    {
                        int rowIndex = i;
                        TextBox txtprice = (TextBox)GrdvDepartment.Rows[rowIndex].FindControl("txtprice");
                        HiddenField hdnitemid = (HiddenField)GrdvDepartment.Rows[rowIndex].FindControl("hdnitemid");





                        if (txtprice.Text != string.Empty)
                        {
                            SqlParameter[] prmadd = new SqlParameter[3];
                            prmadd[0] = new SqlParameter("@sid", cusid);
                            prmadd[1] = new SqlParameter("@itemid", hdnitemid.Value);
                            prmadd[2] = new SqlParameter("@price", txtprice.Text.Trim());
                            if (issamesup == 1)
                            {
                                if (ContactType.SelectedValue == "2" || ContactType.SelectedValue == "1")
                                {
                                    dacommruntime.executeDMLQuery("tbl_contactpriceslistinsert", prmadd, databasenamemain);
                                }
                                else
                                {
                                    da.executeDMLQuery("tbl_contactpriceslistinsert", prmadd);
                                }
                            }
                            else
                            {
                                da.executeDMLQuery("tbl_contactpriceslistinsert", prmadd);
                            }
                        }

                    }



                    if (issamesup == 1)
                    {
                        if (ContactType.SelectedValue == "2" || ContactType.SelectedValue == "1")
                        {
                            insertinextracompanydata(ismainregid, cusid.ToString());
                        }
                    }




                    string strsql = "select tbl_customefielddetails.*,tbl_cusdatatypemaster.cusdatatype from tbl_customefielddetails inner join tbl_cusdatatypemaster  on  tbl_customefielddetails.labeldatatype=tbl_cusdatatypemaster.id  where tbl_customefielddetails.regid='" + Request.Cookies["cadmin"]["id"] + "' and tbl_customefielddetails.status=1 and tbl_customefielddetails.archive=0 and tbl_customefielddetails.typeid=2";
                    DataSet ds = da.GetDataSetInline(strsql);

                    int counter = 1; // this will be used to control naming  
                                     // create a table to store 
                    for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
                    {
                        int y = Convert.ToInt32(ds.Tables[0].Rows[x]["labeldatatype"].ToString());
                        string controlid = ds.Tables[0].Rows[x]["id"].ToString();
                        string controlvalue = "";
                        string ismultiselection = ds.Tables[0].Rows[x]["ismultiselection"].ToString();

                        switch (y)
                        {
                            case 1:
                                txtText = (TextBox)pnlcustomControl.FindControl("ctlcust" + Convert.ToString(counter));
                                controlvalue = txtText.Text;
                                break;
                            case 2:
                                Chkbox = (CheckBox)pnlcustomControl.FindControl("ctlcust" + Convert.ToString(counter));
                                if (Chkbox.Checked)
                                {

                                    controlvalue = "1";
                                }
                                break;
                            case 3:
                                if (ismultiselection == "1")
                                {
                                    ListBox ListBoxmulti = (ListBox)pnlcustomControl.FindControl("ctlcust" + Convert.ToString(counter));

                                    //  string roomnos = "";
                                    foreach (ListItem item in ListBoxmulti.Items)
                                    {
                                        if (item.Selected)
                                        {

                                            if (controlvalue != string.Empty)
                                            {
                                                controlvalue += "###" + item.Value;
                                            }
                                            else
                                            {
                                                controlvalue = item.Value;
                                            }
                                        }
                                    }
                                    //   controlvalue = dropdown.SelectedValue;
                                }
                                else
                                {
                                    dropdown = (DropDownList)pnlcustomControl.FindControl("ctlcust" + Convert.ToString(counter));
                                    controlvalue = dropdown.SelectedValue;
                                }
                                break;
                            default:
                                break;
                        }
                        counter++;
                        prm = new SqlParameter[3];
                        prm[0] = new SqlParameter("@uid", cusid);
                        prm[1] = new SqlParameter("@controlid", controlid);
                        prm[2] = new SqlParameter("@controlvalue", controlvalue);
                        if (issamesup == 1)
                        {
                            if (ContactType.SelectedValue == "2" || ContactType.SelectedValue == "1")
                            {
                                dacommruntime.executeDMLQuery("tbl_customercustomefieldvalueinsertupdate", prm, databasenamemain);
                            }
                            else
                            {
                                da.executeDMLQuery("tbl_customercustomefieldvalueinsertupdate", prm);
                            }
                        }
                        else
                        {
                            da.executeDMLQuery("tbl_customercustomefieldvalueinsertupdate", prm);
                        }

                    }

                    if (issamesup == 1)
                    {
                        if (ismainregid != Request.Cookies["cadmin"]["id"].ToString())
                        {
                            string strsqkk = "select * from tbl_contactsamedetailes where regidextra='" + Request.Cookies["cadmin"]["id"].ToString() + "' and cid='" + cusid.ToString() + "' and status=1 and archive=0";
                            DataSet dsmmcom = dacommruntime.GetDataSetInline(strsqkk, databasenamemain);
                            if (dsmmcom.Tables[0].Rows.Count > 0)
                            {
                                cusid = Convert.ToInt32(dsmmcom.Tables[0].Rows[0]["cidextra"].ToString());
                            }
                        }

                    }



                    if (Request.QueryString["type"] != null)
                    {

                        if (Request.QueryString["type"].ToString() == "2")
                        {
                            
                            if (Request.QueryString["rtype"] != null && Request.QueryString["calltype"] != null && Request.QueryString["form"] != null && Request.QueryString["calltype"].ToString() == "pms")
                            {
                                if (Request.QueryString["form"].ToString() == "sales")
                                {
                                    if (Request.QueryString["id"] != null)
                                    {
                                        Page.RegisterClientScriptBlock("js", "<script>alert('Data Saved Sucessfully!!!!!!');window.parent.location.href ='/crm/addnewsalesordersinglecal.aspx?id=" + Request.QueryString["id"].ToString() + "&sid=" + cmf.Encrypt(cusid.ToString()) + "';</script>");
                                    }
                                    else
                                    {
                                        Page.RegisterClientScriptBlock("js", "<script>alert('Data Saved Sucessfully!!!!!!');window.parent.location.href ='/crm/addnewsalesordersinglecal.aspx?sid=" + cmf.Encrypt(cusid.ToString()) + "';</script>");
                                    }
                                }
                                else if (Request.QueryString["form"].ToString() == "quotation")
                                {
                                    if (Request.QueryString["id"] != null)
                                    {
                                        Page.RegisterClientScriptBlock("js", "<script>alert('Data Saved Sucessfully!!!!!!');window.parent.location.href ='/crm/addnewQuotationsinglecal.aspx?id=" + Request.QueryString["id"].ToString() + "&sid=" + cmf.Encrypt(cusid.ToString()) + "';</script>");
                                    }
                                    else
                                    {
                                        Page.RegisterClientScriptBlock("js", "<script>alert('Data Saved Sucessfully!!!!!!');window.parent.location.href ='/crm/addnewQuotationsinglecal.aspx?sid=" + cmf.Encrypt(cusid.ToString()) + "';</script>");
                                    }
                                   
                                }
                                else if (Request.QueryString["form"].ToString() == "project")
                                {
                                    
                                    //Response.Write(Request.QueryString["calltype"].ToString());
                                    //Response.End();
                                    if (Request.QueryString["calltype"] != null)
                                    {
                                        SaveLogText("customeraadd2-" + Request.QueryString["calltype"].ToString());
                                        if (Request.QueryString["calltype"].ToString() == "pms")
                                        {
                                            Page.RegisterClientScriptBlock("js", "<script>alert('Data Saved Sucessfully!!!!!!');window.parent.document.getElementById('ContentPlaceHolder1_hdnnewclient').value ='" + cusid.ToString() + "';window.parent.document.getElementById('btnclsoesalesN').click();</script>");
                                        }
                                    }

                                }
                            }
                        }
                        else
                        {
                            if (Request.QueryString["rtype"] != null)
                            {


                                if (Request.QueryString["rtype"].ToString() == "openlink")
                                {

                                    Page.RegisterClientScriptBlock("js", "<script>alert('Data Saved Sucessfully!!!!!!');window.parent.location.href ='/ramakrishna/createopenlink.aspx?sid=" + cmf.Encrypt(cusid.ToString()) + "';</script>");

                                }

                                if (Request.QueryString["rtype"].ToString() == "sales")
                                {

                                    Page.RegisterClientScriptBlock("js", "<script>alert('Data Saved Sucessfully!!!!!!');window.parent.location.href ='addnewsalesorder.aspx?sid=" + cmf.Encrypt(cusid.ToString()) + "';</script>");

                                }
                                if (Request.QueryString["rtype"].ToString() == "estink")
                                {

                                    Page.RegisterClientScriptBlock("js", "<script>alert('Data Saved Sucessfully!!!!!!');window.parent.location.href ='addnewestimationinsukink.aspx?sid=" + cmf.Encrypt(cusid.ToString()) + "';</script>");

                                }
                                else if (Request.QueryString["rtype"].ToString() == "jobs")
                                {
                                    Page.RegisterClientScriptBlock("js", "<script>alert('Data Saved !!!!!!');window.parent.location.href ='Add_jobScheduler.aspx?sid=" + cmf.Encrypt(cusid.ToString()) + "';</script>");
                                }
                                else if (Request.QueryString["rtype"].ToString() == "invoice")
                                {

                                    if (Request.QueryString["invoicetype"] != null)
                                    {
                                        Page.RegisterClientScriptBlock("js", "<script>alert('Data Saved Sucessfully !!!!!!');window.parent.location.href ='addnewinvoiceorder.aspx?sid=" + cmf.Encrypt(cusid.ToString()) + "&type=" + Request.QueryString["invoicetype"] + "';</script>");

                                    }
                                    else
                                    {

                                        Page.RegisterClientScriptBlock("js", "<script>alert('Data Saved Sucessfully !!!!!!');window.parent.location.href ='addnewinvoiceorder.aspx?sid=" + cmf.Encrypt(cusid.ToString()) + "';</script>");
                                    }
                                }
                                else if (Request.QueryString["rtype"].ToString() == "pinvoice")
                                {

                                    if (Request.QueryString["rtype"] != null)
                                    {
                                        Page.RegisterClientScriptBlock("js", "<script>alert('Data Saved Sucessfully !!!!!!');window.parent.location.href ='addnewProformaInvoice.aspx?sid=" + cmf.Encrypt(cusid.ToString()) + "&type=" + Request.QueryString["invoicetype"] + "';</script>");

                                    }
                                    else
                                    {

                                        Page.RegisterClientScriptBlock("js", "<script>alert('Data Saved Sucessfully !!!!!!');window.parent.location.href ='addnewinvoiceorder.aspx?sid=" + cmf.Encrypt(cusid.ToString()) + "';</script>");
                                    }
                                }
                                else if (Request.QueryString["rtype"].ToString() == "cloneinvoice")
                                {
                                    Page.RegisterClientScriptBlock("js", "<script>alert('Data Saved Sucessfully !!!!!!');window.parent.location.href ='addnewinvoiceorderclone.aspx?sid=" + cmf.Encrypt(cusid.ToString()) + "&id=" + Request.QueryString["invoiceid"].ToString() + "';</script>");

                                }
                                else if (Request.QueryString["rtype"].ToString() == "rentalinvoice")
                                {
                                    Page.RegisterClientScriptBlock("js", "<script>alert('Data Saved Sucessfully!!!!!!');window.parent.location.href ='addnewinvoiceorderrental.aspx?sid=" + cmf.Encrypt(cusid.ToString()) + "';</script>");

                                }
                                else if (Request.QueryString["rtype"].ToString() == "projects")
                                {
                                    Page.RegisterClientScriptBlock("js", "<script>alert('Data Saved Sucessfully!!!!!!');window.parent.location.href ='addnewProject.aspx?sid=" + cmf.Encrypt(cusid.ToString()) + "';</script>");

                                }
                                else if (Request.QueryString["rtype"].ToString() == "amountreceived")
                                {
                                    Page.RegisterClientScriptBlock("js", "<script>alert('Data Saved Sucessfully!!!!!!');window.parent.location.href ='addnewpaymentsreceived.aspx?sid=" + cmf.Encrypt(cusid.ToString()) + "';</script>");

                                }
                                else if (Request.QueryString["rtype"].ToString() == "estimation")
                                {
                                    Page.RegisterClientScriptBlock("js", "<script>alert('Data Saved Sucessfully!!!!!!');window.parent.location.href ='addnewQuotationclone.aspx?sid=" + cmf.Encrypt(cusid.ToString()) + "&id=" + Request.QueryString["esid"] + "&type=estimation';</script>");
                                }
                                else if (Request.QueryString["rtype"].ToString() == "site")
                                {
                                    Page.RegisterClientScriptBlock("js", "<script>alert('Data Saved Sucessfully!!!!!!');window.parent.location.href ='/SiteManagement/addSite.aspx?sid=" + cmf.Encrypt(cusid.ToString()) + "&count=" + Request.QueryString["count"] + "';</script>");
                                }
                                else if (Request.QueryString["rtype"].ToString() == "contr")
                                {
                                    Page.RegisterClientScriptBlock("js", "<script>alert('Data Saved Sucessfully!!!!!!');window.parent.location.href ='/SiteManagement/AddWorkOrder.aspx?soid=" + Request.QueryString["soid"].ToString() + "&sid=" + cmf.Encrypt(cusid.ToString()) + "&count=" + Request.QueryString["count"] + "';</script>");
                                }
                                else
                                {
                                    Page.RegisterClientScriptBlock("js", "<script>alert('Data Saved Sucessfully!!!!!!');window.parent.location.href ='addnewQuotation.aspx?sid=" + cmf.Encrypt(cusid.ToString()) + "';</script>");
                                }
                            }
                            else
                            {
                                if (Request.QueryString["soid"] != null)
                                {
                                    Page.RegisterClientScriptBlock("js", "<script>alert('Data Saved Sucessfully!!!!!!');window.parent.location.href ='AddWorkOrder.aspx?soid=" + Request.QueryString["soid"].ToString() + "&sid=" + cmf.Encrypt(cusid.ToString()) + "';</script>");
                                }
                                else if (Request.QueryString["type"].ToString() == "MPO")
                                {
                                    string str = "update tbl_contactsdetails set ContactType=2 where id='" + cusid + "'";
                                    da.executeDMLQueryinline(str);

                                    Page.RegisterClientScriptBlock("js", "<script>alert('Data Saved Sucessfully!!!!!!');window.parent.location.href ='esbee/AddNewProvisionalPo.aspx';</script>");
                                    hdnreload11.Value = "11";
                                }
                                else if (Request.QueryString["type"].ToString() == "Sborg")
                                {
                                    string str = "update tbl_contactsdetails set ContactType=2 where id='" + cusid + "'";
                                    da.executeDMLQueryinline(str);

                                    Page.RegisterClientScriptBlock("js", "<script>alert('Data Saved Sucessfully!!!!!!');window.parent.location.href ='SbOrganic/AddNewProvisionalPo.aspx';</script>");
                                    hdnreload11.Value = "11";
                                }
                                else
                                {

                                    Page.RegisterClientScriptBlock("js", "<script>alert('Data Saved Sucessfully!!!!!!');window.parent.location.href ='addnewQuotation.aspx?sid=" + cmf.Encrypt(cusid.ToString()) + "';</script>");

                                }
                            }
                        }
                    }
                    else
                    {
                        if (Request.QueryString["rtype"] != null)
                        {

                            if (Request.QueryString["rtype"].ToString() == "vdc")
                            {
                                Page.RegisterClientScriptBlock("js", "<script>alert('Data Saved Sucessfully!!!!!!');window.parent.location.href ='addnewdcreturneornonpovendor.aspx?sid=" + cmf.Encrypt(cusid.ToString()) + "&typeid=" + Request.QueryString["typeid"] + "';</script>");

                            }

                            if (Request.QueryString["rtype"].ToString() == "billorder")
                            {
                                Page.RegisterClientScriptBlock("js", "<script>alert('Data Saved Sucessfully!!!!!!');window.parent.location.href ='/crm/addnewbillorder.aspx?sid=" + cmf.Encrypt(cusid.ToString()) + "';</script>");

                            }
                            else if (Request.QueryString["rtype"].ToString() == "amountpay")
                            {
                                Page.RegisterClientScriptBlock("js", "<script>alert('Data Saved Sucessfully!!!!!!');window.parent.location.href ='/crm/addnewpaymentsmade.aspx?sid=" + cmf.Encrypt(cusid.ToString()) + "';</script>");

                            }
                            else if (Request.QueryString["rtype"].ToString() == "expenceorder")
                            {
                                Page.RegisterClientScriptBlock("js", "<script>alert('Data Saved Sucessfully!!!!!!');window.parent.location.href ='/crm/addnewbillorderexpense.aspx?sid=" + cmf.Encrypt(cusid.ToString()) + "';</script>");

                            }
                            else
                            {
                                Page.RegisterClientScriptBlock("js", "<script>alert('Data Saved Sucessfully!!!!!!');window.parent.location.href ='/crm/addnewpurchaseordersinglecal.aspx?sid=" + cmf.Encrypt(cusid.ToString()) + "';</script>");
                            }
                        }
                        else
                        {
                            Page.RegisterClientScriptBlock("js", "<script>alert('Data Saved Sucessfully!!!!!!');window.parent.location.href ='/crm/addnewpurchaseordersinglecal.aspx?sid=" + cmf.Encrypt(cusid.ToString()) + "';</script>");
                        }
                    }
                }
                else
                {
                    Page.RegisterClientScriptBlock("js", "<script>alert('This GSTN Number Is Already Exits');</script>");
                }
            }

            else
            {
                Page.RegisterClientScriptBlock("js", "<script>alert('This Company Name Is Already Exits');</script>");
            }
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

    protected bool checkvalidataiongst()
    {
        //if (TaxRegistrationNumber.Value != string.Empty)
        //{
        //    string strsql = "select * from tbl_contactsdetails where TaxRegistrationNumber='" + TaxRegistrationNumber.Value.Trim() + "' and status=1 and archive=0 and regid='" + Request.Cookies["cadmin"]["id"].ToString() + "' and ContactType='" + ContactType.SelectedValue + "'";
        //    DataSet ds = da.GetDataSetInline(strsql);
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}
        //else
        //{
        return true;

        // }
    }

    protected void insertinextracompanydata(string regid, string cid)
    {
        dataaccessruntime dacommruntime = new dataaccessruntime();
        dataaccesscomm dacomm = new dataaccesscomm();
        string databasenamemain = "";
        string strsqladmins = "select * from tbl_registrationdetails where id='" + regid + "' and status=1 and archive=0";
        DataSet dsismultim = dacomm.GetDataSetInline(strsqladmins);
        // Response.Write(strsqladmin);
        if (dsismultim.Tables[0].Rows.Count > 0)
        {
            databasenamemain = dsismultim.Tables[0].Rows[0]["databasename"].ToString();
        }


        string databasename = "";
        string regidextra = "0";
        string strsqladmin = "select * from tbl_registrationdetails where pid='" + regid + "' and status=1 and archive=0";
        DataSet dsismulti = dacomm.GetDataSetInline(strsqladmin);
        //Response.Write(strsqladmin);
        if (dsismulti.Tables[0].Rows.Count > 0)
        {
            for (int jk = 0; jk < dsismulti.Tables[0].Rows.Count; jk++)
            {
                databasename = dsismulti.Tables[0].Rows[jk]["databasename"].ToString();



                SqlParameter[] prm = new SqlParameter[51];
                prm[0] = new SqlParameter("@regid", dsismulti.Tables[0].Rows[jk]["id"].ToString());
                prm[1] = new SqlParameter("@ContactType", ContactType.SelectedValue);
                prm[2] = new SqlParameter("@cdate", da.getcdatetime());
                prm[3] = new SqlParameter("@uid", Request.Cookies["user"]["user_id"].ToString());
                prm[4] = new SqlParameter("@txtsalutation", txtsalutation.Value.Trim());
                prm[5] = new SqlParameter("@txtfirstname", txtfirstname.Value.Trim());
                prm[6] = new SqlParameter("@txtlastname", txtlastname.Value.Trim());
                prm[7] = new SqlParameter("@txtcompanyname", txtcompanyname.Value.Trim());
                prm[8] = new SqlParameter("@txtcontactdisplayname", txtcompanyname.Value.Trim());
                prm[9] = new SqlParameter("@txtContactEmail", txtContactEmail.Text.Trim());
                prm[10] = new SqlParameter("@txtworkphonePhone", txtworkphonePhone.Value.Trim());
                prm[11] = new SqlParameter("@txtmobile", txtmobile.Value.Trim());
                prm[12] = new SqlParameter("@txtSkypeName", txtSkypeName.Value.Trim());
                prm[13] = new SqlParameter("@txtDesignation", txtDesignation.Value.Trim());
                prm[14] = new SqlParameter("@txtDepartment", txtDepartment.Value.Trim());
                prm[15] = new SqlParameter("@Website", Website.Value.Trim());
                prm[16] = new SqlParameter("@VATTreatment", VATTreatment.SelectedValue);
                prm[17] = new SqlParameter("@TaxRegistrationNumber", TaxRegistrationNumber.Value.Trim());
                prm[18] = new SqlParameter("@PlaceOfSupply", ddlplaceofsupply.SelectedValue);
                prm[19] = new SqlParameter("@txtCurrency", txtCurrency.SelectedValue);
                prm[20] = new SqlParameter("@PaymentTerms", PaymentTerms.Value.Trim());
                prm[21] = new SqlParameter("@isEnablePortal", "0");
                prm[22] = new SqlParameter("@PortalLanguage", "0");
                prm[23] = new SqlParameter("@Facebook", Facebook.Value.Trim());
                prm[24] = new SqlParameter("@Twitter", Twitter.Value.Trim());
                prm[25] = new SqlParameter("@billAttention", billAttention.Value.Trim());
                prm[26] = new SqlParameter("@billAddress1", billAddress1.Value.Trim());
                prm[27] = new SqlParameter("@billAddress2", billAddress2.Value.Trim());
                prm[28] = new SqlParameter("@billCity", billCity.Value.Trim());
                prm[29] = new SqlParameter("@billState", billState.Value.Trim());
                prm[30] = new SqlParameter("@billZipCode", billZipCode.Value.Trim());
                prm[31] = new SqlParameter("@billCountry", billCountry.Value.Trim());
                prm[32] = new SqlParameter("@billPhone", billPhone.Value.Trim());
                prm[33] = new SqlParameter("@billFax", billFax.Value.Trim());
                prm[34] = new SqlParameter("@ShipAttention", ShipAttention.Value.Trim());
                prm[35] = new SqlParameter("@ShipAddress1", ShipAddress1.Value.Trim());
                prm[36] = new SqlParameter("@ShipAddress2", ShipAddress2.Value.Trim());
                prm[37] = new SqlParameter("@ShipCity", ShipCity.Value.Trim());
                prm[38] = new SqlParameter("@ShipState", ShipState.Value.Trim());
                prm[39] = new SqlParameter("@ShipZipCode", ShipZipCode.Value.Trim());
                prm[40] = new SqlParameter("@ShipCountry", ShipCountry.Value.Trim());
                prm[41] = new SqlParameter("@ShipPhone", ShipPhone.Value.Trim());
                prm[42] = new SqlParameter("@ShipFax", ShipFax.Value.Trim());
                prm[43] = new SqlParameter("@remarks", remarks.Value.Trim());
                prm[44] = new SqlParameter("ReturnValue", SqlDbType.Int);
                prm[44].Direction = ParameterDirection.ReturnValue;
                if (txtcreditlimit.Text != string.Empty)
                {
                    prm[45] = new SqlParameter("@txtcreditlimit", txtcreditlimit.Text.Trim());
                }
                else
                {
                    prm[45] = new SqlParameter("@txtcreditlimit", DBNull.Value);
                }
                if (txtcreditperiod.Text != string.Empty)
                {
                    prm[46] = new SqlParameter("@txtcreditperiod", txtcreditperiod.Text.Trim());
                }
                else
                {
                    prm[46] = new SqlParameter("@txtcreditperiod", DBNull.Value);
                }
                prm[47] = new SqlParameter("@PaymentTermsdisplay", PaymentTermsdisplay.Value.Trim());
                prm[48] = new SqlParameter("@vcode", txtvcode.Value.Trim());
                prm[49] = new SqlParameter("@billgst", billgst.Value.Trim());
                prm[50] = new SqlParameter("@Shipgst", Shipgst.Value.Trim());
                int cusid = dacommruntime.executeDMLQuery("tbl_contactsdetailsinsert_creditlimit", prm, databasename);

                try
                {
                    SqlParameter[] prmext = new SqlParameter[4];
                    prmext[0] = new SqlParameter("@regid", regid);
                    prmext[1] = new SqlParameter("@cid", cid);
                    prmext[2] = new SqlParameter("@regidextra", dsismulti.Tables[0].Rows[jk]["id"].ToString());
                    prmext[3] = new SqlParameter("@cidextra", cusid);
                    dacommruntime.executeDMLQuery("tbl_contactsamedetailes_usp", prmext, databasenamemain);


                }
                catch
                {

                }



                try
                {

                    string cuspricetype = "0";
                    try
                    {
                        cuspricetype = ddlcustomertype.SelectedValue;
                    }
                    catch
                    {


                    }

                    string sectionid = "0";
                    try
                    {
                        if (istds.SelectedValue == "1")
                        {
                            if (ddlsectionname.SelectedValue == "-1")
                            {
                                SqlParameter[] prmsec = new SqlParameter[4];
                                prmsec[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"].ToString());
                                prmsec[1] = new SqlParameter("@sectionname", txtsectionname.Text.Trim());
                                prmsec[2] = new SqlParameter("ReturnValue", SqlDbType.Int);
                                prmsec[2].Direction = ParameterDirection.ReturnValue;
                                prmsec[3] = new SqlParameter("@tdsper", testper.Text.Trim());
                                sectionid = da.executeDMLQuery("tbl_tdssectionnameinsert", prmsec).ToString();
                            }
                            else
                                sectionid = ddlsectionname.SelectedValue;
                        }
                    }
                    catch
                    {

                    }

                    string strsqluptcs = "update tbl_contactsdetails set txtpterms='" + txtpterms.Value + "',rdltcs='" + rdltcs.SelectedValue + "',cuspricetype='" + cuspricetype + "',panno='" + panno.Value.Trim() + "',Tanno='" + Tanno.Value.Trim() + "',istds='" + istds.SelectedValue + "',sectionid='" + sectionid + "' where id='" + cusid + "'";
                    dacommruntime.executeDMLQueryinline(strsqluptcs, databasename);
                }
                catch
                {

                }

                prm = new SqlParameter[8];
                prm[0] = new SqlParameter("@regid", dsismulti.Tables[0].Rows[jk]["id"].ToString());
                prm[1] = new SqlParameter("@cdate", da.getcdatetimenew());
                prm[2] = new SqlParameter("@groupname", txtcompanyname.Value.Trim());
                prm[3] = new SqlParameter("@pid", txtunder.SelectedValue);
                prm[4] = new SqlParameter("@openingval", "0");
                prm[5] = new SqlParameter("@typeid", "1");
                prm[6] = new SqlParameter("@refid", cusid);
                prm[7] = new SqlParameter("@entrytype", "1");
                dacommruntime.executeDMLQuery("tbl_ledgersinsertcontact_entertype", prm, databasename);



                for (int i = 0; i < Gridview1.Rows.Count; i++)
                {
                    int rowIndex = i;
                    TextBox Salutation = (TextBox)Gridview1.Rows[rowIndex].FindControl("Salutation");
                    TextBox FirstName = (TextBox)Gridview1.Rows[rowIndex].FindControl("FirstName");
                    TextBox lastname = (TextBox)Gridview1.Rows[rowIndex].FindControl("lastname");
                    TextBox EmailAddress = (TextBox)Gridview1.Rows[rowIndex].FindControl("EmailAddress");
                    TextBox WorkPhone = (TextBox)Gridview1.Rows[rowIndex].FindControl("WorkPhone");
                    TextBox Mobile = (TextBox)Gridview1.Rows[rowIndex].FindControl("Mobile");
                    TextBox SkypeName = (TextBox)Gridview1.Rows[rowIndex].FindControl("SkypeName");
                    TextBox Designation = (TextBox)Gridview1.Rows[rowIndex].FindControl("Designation");
                    TextBox Department = (TextBox)Gridview1.Rows[rowIndex].FindControl("Department");

                    if (Salutation.Text != string.Empty && FirstName.Text != string.Empty)
                    {
                        SqlParameter[] prmadd = new SqlParameter[10];


                        prmadd[0] = new SqlParameter("@contactid", cusid);
                        prmadd[1] = new SqlParameter("@Salutation", Salutation.Text.Trim());
                        prmadd[2] = new SqlParameter("@FirstName", FirstName.Text.Trim());
                        prmadd[3] = new SqlParameter("@lastname", lastname.Text.Trim());
                        prmadd[4] = new SqlParameter("@EmailAddress", EmailAddress.Text.Trim());
                        prmadd[5] = new SqlParameter("@WorkPhone", WorkPhone.Text.Trim());
                        prmadd[6] = new SqlParameter("@Mobile", Mobile.Text.Trim());
                        prmadd[7] = new SqlParameter("@SkypeName", SkypeName.Text.Trim());
                        prmadd[8] = new SqlParameter("@Designation", Designation.Text.Trim());
                        prmadd[9] = new SqlParameter("@Department", Department.Text.Trim());
                        dacommruntime.executeDMLQuery("tbl_contactcontactpersondetailsinsert", prmadd, databasename);
                    }

                }





                for (int i = 0; i < GrdvDepartment.Rows.Count; i++)
                {
                    int rowIndex = i;
                    TextBox txtprice = (TextBox)GrdvDepartment.Rows[rowIndex].FindControl("txtprice");
                    HiddenField hdnitemid = (HiddenField)GrdvDepartment.Rows[rowIndex].FindControl("hdnitemid");
                    if (txtprice.Text != string.Empty)
                    {
                        SqlParameter[] prmadd = new SqlParameter[3];
                        prmadd[0] = new SqlParameter("@sid", cusid);
                        prmadd[1] = new SqlParameter("@itemid", hdnitemid.Value);
                        prmadd[2] = new SqlParameter("@price", txtprice.Text.Trim());
                        dacommruntime.executeDMLQuery("tbl_contactpriceslistinsert", prmadd, databasename);
                    }

                }



            }
        }
    }








}