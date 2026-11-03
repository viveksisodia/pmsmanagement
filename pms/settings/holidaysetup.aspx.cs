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

public partial class holidaysetup : System.Web.UI.Page
{
    DataAccess da = new DataAccess();
    SqlParameter[] prm;
    DataSet ds = new DataSet();
    CommonFunction cmg = new CommonFunction();
    CommonFunction cmf = new CommonFunction();
    public function fns = new function();

    public int isspg = 0;

    protected void Page_Load(object sender, EventArgs e)
    {

        //if (Request.Cookies["cadmin"]["id"].ToString() == "2172" || Request.Cookies["cadmin"]["id"].ToString() == "2305")
        //{
        //    isspg = 1;
        //    Panel1.Visible = true;
        //}
        //else
        //{
        //    Panel1.Visible = false;
        //}

        if (!Page.IsPostBack)
        {
            bindleaveupdate();
            //binddata();
            //bindRestrictedleave();
            bindapproval();
            
        }
    }
    protected void chkapplybackdate_change(object sender, EventArgs e)
    {
        if (chkapplybackdate1.Checked)
        {
            txtapplybackdate.Enabled = true;
            RequiredFieldValidator8.Enabled = true;
        }
        else
        {
            txtapplybackdate.Enabled = false;
            RequiredFieldValidator8.Enabled = false;
            txtapplybackdate.Text = "";
        }
    }

    protected void bindapproval()
    {
        string strslq = "select * from tbl_PreferencesGeneral where regid='" + Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0";
        DataSet ds = da.GetDataSetInline(strslq);
        if (ds.Tables[0].Rows.Count > 0)
        {

            if (ds.Tables[0].Rows[0]["isbackdate"].ToString() == "1")
            {
                chkapplybackdate1.Checked = true;
                txtapplybackdate.Text = ds.Tables[0].Rows[0]["isbackdateday"].ToString();
                chkapplybackdate2.Checked = false;
            }
            else
            {
                chkapplybackdate1.Checked = false;

                chkapplybackdate2.Checked = true;
            }
            txtnoofrholiday.Text= ds.Tables[0].Rows[0]["txtnoofrholiday"].ToString();
            txtsoonerday.Text = ds.Tables[0].Rows[0]["txtsoonerday"].ToString();

            if (ds.Tables[0].Rows[0]["isrholidayapprovalrequired"].ToString() != "1")
            {
                rdlleaveapproval1.Checked = false;
                rdlleaveapproval2.Checked = true;
                pnlleaveapprovelabel.Style.Add("display", "none");
            }
            else
            {
                rdlleaveapproval1.Checked = true;
                rdlleaveapproval2.Checked = false;
                pnlleaveapprovelabel.Style.Add("display", "block");
                string strsqlitem = "select * from tbl_masterapprovalsetup where regid='" + Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0 and type=37 order by aLevel ";
                DataSet dsadd = da.GetDataSetInline(strsqlitem);
                if (dsadd.Tables[0].Rows.Count > 0)
                {
                    hdngridviewrow.Value = dsadd.Tables[0].Rows.Count.ToString();
                    ViewState["CurrentTableapproval"] = dsadd.Tables[0];
                    gird1.DataSource = dsadd.Tables[0];
                    gird1.DataBind();

                    SetPreviousDataapproval();



                }
            }
        }
    }
    protected void btnsave_click(object sender, EventArgs e)
    {
        string isapprovalrequired = "0";

        if (rdlleaveapproval1.Checked)
        {
            isapprovalrequired = "1";
        }
        else
        {
            isapprovalrequired = "2";
        }

        string isbackdate = "0";
        string isbackdateday = "0";
        if (chkapplybackdate1.Checked)
        {
            isbackdate = "1";
            isbackdateday = txtapplybackdate.Text;

        }
        else
        {
            isbackdate = "2";
        }

        SqlParameter[] prm = new SqlParameter[5];
        prm[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"]);
        prm[1] = new SqlParameter("@txtnoofrholiday", txtnoofrholiday.Text);
        prm[2] = new SqlParameter("@txtsoonerday", txtsoonerday.Text);
        prm[3] = new SqlParameter("@isbackdate", isbackdate);
        prm[4] = new SqlParameter("@isbackdateday", isbackdateday);
        da.executeDMLQuery("tbl_PreferencesGeneral_txtnoofrholidayupdate", prm);

        if (checkisapprovallevel())
        {

            int approvalLevel = 0;

            string strsql = "delete from tbl_masterapprovalsetup where regid='" + Request.Cookies["cadmin"]["id"] + "' and type='37'";
            da.executeDMLQueryinline(strsql);


            for (int i = 0; i < gird1.Rows.Count; i++)
            {
                HiddenField hdnempids = (HiddenField)gird1.Rows[i].FindControl("hdnempids");
                if (hdnempids.Value != string.Empty)
                {
                    SqlParameter[] prmapprover = new SqlParameter[4];
                    prmapprover[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"]);
                    prmapprover[1] = new SqlParameter("@hdnempids", hdnempids.Value);
                    prmapprover[2] = new SqlParameter("@aLevel", (i + 1));
                    prmapprover[3] = new SqlParameter("@type", "37");
                    da.executeDMLQuery("tbl_masterapprovalsetupinsert", prmapprover);
                    approvalLevel++;
                }
            }
            prm = new SqlParameter[5];
            prm[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"]);
            prm[1] = new SqlParameter("@isapprovalrequired", isapprovalrequired);
            if (isapprovalrequired == "1")
            {

                prm[2] = new SqlParameter("@approvalLevel", approvalLevel);
            }
            else
            {
                prm[2] = new SqlParameter("@approvalLevel", "0");
            }
            if (rdlemailnotification1.Checked)
            {
                prm[4] = new SqlParameter("@isemailnotification", "1");
            }
            else
            {
                prm[4] = new SqlParameter("@isemailnotification", "0");

            }
            prm[3] = new SqlParameter("@type", "37");
            da.executeDMLQuery("intra_sp_approvalrequiredupdate", prm);
            Page.RegisterClientScriptBlock("js", "<script>alert('Data Saved Sucessfully!!!!!!');</script>");
        }


        }

    protected bool checkisapprovallevel()
    {
        if (rdlleaveapproval1.Checked)
        {
            if (gird1.Rows.Count > 0)
            {

                for (int i = 0; i < gird1.Rows.Count; i++)
                {
                    HiddenField hdnempids = (HiddenField)gird1.Rows[i].FindControl("hdnempids");
                    if (hdnempids.Value != string.Empty)
                    {
                        return true;
                    }
                }

            }

            return false;

        }


        return true;
    }
    protected void rdlleaveapproval_change(object sender, EventArgs e)
    {
        if (rdlleaveapproval1.Checked)
        {

            pnlleaveapprovelabel.Style.Add("display", "block");
            // pnlleaveapprovelabel.Visible = true;
        }
        else
        {

            pnlleaveapprovelabel.Style.Add("display", "none");


        }
    }

    protected void addnewrow_click(object sender, EventArgs e)
    {
        AddNewRowToGridapproval();
    }
    private void SetInitialRowapproval()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(new DataColumn("id", typeof(string)));
        dt.Columns.Add(new DataColumn("hdnempids", typeof(string)));
        dr = dt.NewRow();
        dr["id"] = 1;
        dr["hdnempids"] = string.Empty;
        dt.Rows.Add(dr);


        ViewState["CurrentTableapproval"] = dt;
        hdngridviewrow.Value = dt.Rows.Count.ToString();
        gird1.DataSource = dt;
        gird1.DataBind();
    }

    private void AddNewRowToGridapproval()
    {
        int rowIndex = 0;

        if (ViewState["CurrentTableapproval"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTableapproval"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {


                    HiddenField hdnempids = (HiddenField)gird1.Rows[rowIndex].FindControl("hdnempids");
                    TextBox txtapproval = (TextBox)gird1.Rows[rowIndex].FindControl("txtapproval");

                    // drCurrentRow["hdnempids"] = hdnempids.Value;

                    dtCurrentTable.Rows[i - 1]["id"] = i.ToString();
                    dtCurrentTable.Rows[i - 1]["hdnempids"] = hdnempids.Value;
                    rowIndex++;
                }
                drCurrentRow = dtCurrentTable.NewRow();
                drCurrentRow["id"] = dtCurrentTable.Rows.Count + 1;

                HiddenField hdnempids1 = (HiddenField)gird1.Rows[rowIndex - 1].FindControl("hdnempids");

                if (hdnempids1.Value != string.Empty)
                {

                    dtCurrentTable.Rows.Add(drCurrentRow);
                }

                ViewState["CurrentTableapproval"] = dtCurrentTable;
                hdngridviewrow.Value = dtCurrentTable.Rows.Count.ToString();
                gird1.DataSource = dtCurrentTable;
                gird1.DataBind();
            }
            else
            {
                SetInitialRowapproval();
            }
        }
        else
        {
            SetInitialRowapproval();
        }

        //Set Previous Data on Postbacks
        SetPreviousDataapproval();
    }
    private void SetPreviousDataapproval()
    {
        int rowIndex = 0;
        if (ViewState["CurrentTableapproval"] != null)
        {
            DataTable dt = (DataTable)ViewState["CurrentTableapproval"];
            hdngridviewrow.Value = dt.Rows.Count.ToString();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    HiddenField hdnempids = (HiddenField)gird1.Rows[i].FindControl("hdnempids");
                    TextBox txtapproval = (TextBox)gird1.Rows[i].FindControl("txtapproval");
                    HtmlGenericControl empdata111 = (HtmlGenericControl)gird1.Rows[i].FindControl("empdata");
                    hdnempids.Value = dt.Rows[i]["hdnempids"].ToString();

                    HtmlControl searchdata = (HtmlControl)gird1.Rows[i].FindControl("searchdata");




                    string displaempstyle = "";

                    ////ADO.Net
                    //SqlConnection cn = new SqlConnection();
                    DataSet ds = new DataSet();
                    DataTable dtemp = new DataTable();
                    char[] cha = { ',' };
                    char[] cha1 = { '~' };

                    string[] arr = hdnempids.Value.Split(cha, StringSplitOptions.None);

                    for (int j = 0; j < arr.Length; j++)
                    {
                        if (arr[j].ToString() != string.Empty)
                        {
                            string[] arrlist = arr[j].ToString().Split(cha1, StringSplitOptions.None);

                            if (arrlist.Length > 1)
                            {
                                if (arrlist[1].ToString() == "1")
                                {

                                    string strsql1 = "select tbl_ImplicitRoles.* from tbl_ImplicitRoles where id ='" + arrlist[0] + "'";
                                    ds = da.GetDataSetInline(strsql1);
                                    dtemp = ds.Tables[0];

                                    foreach (DataRow row in dtemp.Rows)
                                    {
                                        displaempstyle += "<div class='role-notify'><p><i class='fa fa-users'></i>&nbsp;" + row["roleshortname"].ToString() + "</p>";
                                        displaempstyle += "<a href='javascript://'  onclick='deleteemp(\"" + row["id"].ToString() + "~1\",\"" + hdnempids.ClientID + "\",\"" + empdata111.ClientID + "\",\"" + searchdata.ClientID + "\",\"" + txtapproval.ClientID + "\")'>X</a>";
                                        displaempstyle += "</div>";




                                    }


                                }
                                else
                                {



                                    string strsql = "select tbl_employee.id,(tbl_employee.DisplayName) as empname from tbl_employee where id in(" + arrlist[0] + ")";


                                    ds = da.GetDataSetInline(strsql);

                                    dtemp = ds.Tables[0];



                                    foreach (DataRow row in dtemp.Rows)
                                    {
                                        displaempstyle += "<div class='role-notify'><p><i class='fa fa-user'></i>&nbsp;" + row["empname"].ToString() + "</p>";
                                        displaempstyle += "<a href='javascript://'  onclick='deleteemp(\"" + row["id"].ToString() + "~2\",\"" + hdnempids.ClientID + "\",\"" + empdata111.ClientID + "\",\"" + searchdata.ClientID + "\",\"" + txtapproval.ClientID + "\")'>X</a>";
                                        displaempstyle += "</div>";




                                    }
                                }
                            }


                        }
                    }





                    empdata111.InnerHtml = displaempstyle;







                }
            }
        }

    }


    protected void gird1_rowdatabound(object sender, GridViewRowEventArgs e)
    {

        Label lblapproballavel = (Label)e.Row.FindControl("lblapproballavel");
        HiddenField hdnempids = (HiddenField)e.Row.FindControl("hdnempids");
        TextBox txtapproval = (TextBox)e.Row.FindControl("txtapproval");
        HtmlControl searchdata = (HtmlControl)e.Row.FindControl("searchdata");
        HtmlControl empdata = (HtmlControl)e.Row.FindControl("empdata");
        if (txtapproval != null)
        {

            string supstring = "";

            if ((e.Row.RowIndex + 1) == 1 || (e.Row.RowIndex + 1) == 21)
            {

                supstring = "<sup>st</sup>";
            }
            else if ((e.Row.RowIndex + 1) == 2 || (e.Row.RowIndex + 1) == 22)
            {

                supstring = "<sup>nd</sup>";
            }
            else if ((e.Row.RowIndex + 1) == 3 || (e.Row.RowIndex + 1) == 24)
            {

                supstring = "<sup>rd</sup>";
            }
            else
            {

                supstring = "<sup>th</sup>";
            }

            lblapproballavel.Text = (e.Row.RowIndex + 1).ToString() + " " + supstring + "  Approvers";
            txtapproval.Attributes.Add("onkeyup", "searchempdetails(this.value,'" + hdnempids.ClientID + "','" + searchdata.ClientID + "','" + empdata.ClientID + "','" + txtapproval.ClientID + "')");
            txtapproval.Attributes.Add("onfocus", "searchempdetails(this.value,'" + hdnempids.ClientID + "','" + searchdata.ClientID + "','" + empdata.ClientID + "','" + txtapproval.ClientID + "')");


            // txtapproval.Attributes.Add("onblur", "searchempend(this.value,'" + hdnempids.ClientID + "','" + searchdata.ClientID + "','" + empdata.ClientID + "','" + txtapproval.ClientID + "')");


        }
    }

    protected void btnreset_click(object sender, EventArgs e)
    {

        int rowIndex = 0;
        if (ViewState["CurrentTableapproval"] != null)
        {
            DataTable dt = (DataTable)ViewState["CurrentTableapproval"];
            hdngridviewrow.Value = dt.Rows.Count.ToString();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    HiddenField hdnempids = (HiddenField)gird1.Rows[i].FindControl("hdnempids");

                    if (hdnempids.Value == string.Empty)
                    {
                        dt.Rows.RemoveAt(i);
                    }
                }
            }
            ViewState["CurrentTableapproval"] = dt;
            hdngridviewrow.Value = dt.Rows.Count.ToString();
            gird1.DataSource = dt;
            gird1.DataBind();
            SetPreviousDataapproval();
        }

    }



    protected void bindRestrictedleave()
    {
        string strsql= "select * from restrictedholiday where status=1 and archive=0 and regid='" + Request.Cookies["cadmin"]["id"] + "'";
        DataSet dsr = da.GetDataSetInline(strsql);
        GridView1.DataSource = dsr;
        GridView1.DataBind();

    }

    protected void binddata()
    {
        string strsql = "select * from tbl_holidaytype where status=1 and archive=0 and regid='" + Request.Cookies["cadmin"]["id"] + "'";
        DataSet ds = da.GetDataSetInline(strsql);

        grduser.DataSource = ds;
        grduser.DataBind();

        if (ds.Tables[0].Rows.Count > 0)
        {

            hdnvalue1.Value = "";
            hdnvalue2.Value = "";
            hdnvalue3.Value = "";
            try
            {

               
              
                    hdnstatus.Value = "1";
                    hdnvalue1.Value = cmf.dateConvert(ds.Tables[0].Rows[0]["cdate"].ToString());
                    hdnvalue3.Value = "Completed";
                    hdnvalue2.Value = cmf.dateConvert(ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1]["cdate"].ToString());

            }
            catch (Exception ex)
            {
            }
        }
       
    }

    protected void lnkeditholiday1_Click(object sender, EventArgs e)
    {
        LinkButton link = (LinkButton)GridView1.FindControl("btnedit");
        link = (LinkButton)sender;
        GridViewRow gvr = (GridViewRow)link.NamingContainer;
        int id = Convert.ToInt32(GridView1.DataKeys[gvr.RowIndex].Value);
        ViewState["rhid"] = id;
        Button5.Text = "Update";
        useupdateholidayr(id.ToString());

    }

    protected void useupdateholidayr(string val1)
    {
        string strsql = "select * from restrictedholiday where id='" + val1 + "'";
        DataSet ds = da.GetDataSetInline(strsql);
        TextBox3.Text = ds.Tables[0].Rows[0]["title"].ToString();
        TextBox4.Text = ds.Tables[0].Rows[0]["leavedesc"].ToString();
        Date2.Text = ds.Tables[0].Rows[0]["eventdate"].ToString();

        
        //myModalform2.Style.Add("display", "block");
    }

    protected void lnkeditholiday_Click(object sender, EventArgs e)
    {
        LinkButton link = (LinkButton)grdholidaylist.FindControl("btnedit");
        link = (LinkButton)sender;
        GridViewRow gvr = (GridViewRow)link.NamingContainer;
        int id = Convert.ToInt32(grdholidaylist.DataKeys[gvr.RowIndex].Value);
        ViewState["hid"] = id;
        btnaddholiday.Text = "Update";
        useupdateholiday(id.ToString());

    }
    protected void useupdateholiday(string val1)
    {
        string strsql = "select * from holidays where id='" + val1 + "'";
        DataSet ds = da.GetDataSetInline(strsql);
        txtnameholiday.Text = ds.Tables[0].Rows[0]["title"].ToString();
        txtholidaydesc.Text = ds.Tables[0].Rows[0]["leavedesc"].ToString();
        eventdate.Text = ds.Tables[0].Rows[0]["eventdate"].ToString();

        if (ds.Tables[0].Rows[0]["isfloating"].ToString() == "1")
        {
            chkisfloating.Checked = true;
        }
        else
        {
            chkisfloating.Checked = false;
        }
        if (ds.Tables[0].Rows[0]["chkisnationalholiday"].ToString() == "1")
        {
            chkisnationalholiday.Checked = true;
        }
        else
        {
            chkisnationalholiday.Checked = false;
        }
        //myModalform1.Style.Add("display", "block");
    }

    protected void lnkdeleteholiday_Click(object sender, EventArgs e)
    {
        LinkButton link = (LinkButton)grdholidaylist.FindControl("btnedit");
        link = (LinkButton)sender;
        GridViewRow gvr = (GridViewRow)link.NamingContainer;
        int id = Convert.ToInt32(grdholidaylist.DataKeys[gvr.RowIndex].Value);
        string strsql = "update holidays set status=0,archive=1 where id='" + id + "'";
        da.executeDMLQueryinline(strsql);
        //bindleaveupdate(ViewState["id"].ToString());


    }
    protected void lnkdeleteholiday1_Click(object sender, EventArgs e)
    {
        LinkButton link = (LinkButton)GridView1.FindControl("btnedit");
        link = (LinkButton)sender;
        GridViewRow gvr = (GridViewRow)link.NamingContainer;
        int id = Convert.ToInt32(GridView1.DataKeys[gvr.RowIndex].Value);
        string strsql = "update restrictedholiday set status=0,archive=1 where id='" + id + "'";
        //Response.Write(strsql);
        //Response.End();
        da.executeDMLQueryinline(strsql);
        
        bindRestrictedleave();


    }

    protected void lnkedit_Click(object sender, EventArgs e)
    {
        LinkButton link = (LinkButton)grduser.FindControl("btnedit");
        link = (LinkButton)sender;
        GridViewRow gvr = (GridViewRow)link.NamingContainer;
       int id = Convert.ToInt32(grduser.DataKeys[gvr.RowIndex].Value);
        ViewState["id"] = id;
      //  btnadd.Text = "Update";
        useupdate(id.ToString());

    }
    protected void lnkmanage_Click(object sender, EventArgs e)
    {
        LinkButton link = (LinkButton)grduser.FindControl("btnedit");
        link = (LinkButton)sender;
        GridViewRow gvr = (GridViewRow)link.NamingContainer;
        int id = Convert.ToInt32(grduser.DataKeys[gvr.RowIndex].Value);
        ViewState["id"] = id;
       // btnadd.Text = "Update";
        //bindleaveupdate(id.ToString());
        pnlholidaylist.Visible = false;
        pnlholidaymanage.Visible = true;
        pnlcloase.Visible = true;
    }

    protected void useupdate(string id)
    {
        string strsql = "select * from tbl_holidaytype where id='" + id + "'";
        DataSet ds = da.GetDataSetInline(strsql);
        txtlistname.Text = ds.Tables[0].Rows[0]["listname"].ToString();
        txtlistdesc.Text = ds.Tables[0].Rows[0]["listdetails"].ToString();
        btnadd.Text = "Update";
        hdnmodel1.Value = "1";
        myModalform.Style.Add("display", "block");
       // Page.RegisterClientScriptBlock("js","alert('hi');document.getElementById('myModalform').style.display = 'block';");

    }


    protected string showfloating(string val)
    {
        string retstr = "";

        if (val == "1")
        {
            retstr = fns.getltext("- Floating Holiday", "2");
        }

        return retstr;

    }

    protected void bindleaveupdate()
    {

        string holydaytypeid = "0";
        string strsql = "select * from tbl_holidaytype where status=1 and archive=0 and regid='" + Request.Cookies["cadmin"]["id"] + "' and isnull(isdefault,0)=1 ";
        DataSet ds1 = da.GetDataSetInline(strsql);
        if (ds1.Tables[0].Rows.Count > 0)
        {
            holydaytypeid = ds1.Tables[0].Rows[0]["id"].ToString();
        }
        else
        {
            SqlParameter[] prm = new SqlParameter[4];
            prm[0] = new SqlParameter("@listname","Default");
            prm[1] = new SqlParameter("@listdetails", "Default");
            prm[2] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"].ToString());
            prm[3] = new SqlParameter("@cdate", da.getcdatetime());
            da.executeDMLQuery("tbl_holidaytypeinsertdate", prm);
            strsql = "select * from tbl_holidaytype where status=1 and archive=0 and regid='" + Request.Cookies["cadmin"]["id"] + "' and isnull(isdefault,0)=1 ";
            ds1 = da.GetDataSetInline(strsql);
            if (ds1.Tables[0].Rows.Count > 0)
            {
                holydaytypeid = ds1.Tables[0].Rows[0]["id"].ToString();
            }
        }
        hdndefaulttypeid.Value = holydaytypeid;
        string str = "select * from holidays where levelistid='" + holydaytypeid + "' and status=1 and archive=0 order by eventdate";
        DataSet ds = da.GetDataSetInline(str);
       // grdholidaylist.CssClass = "";
        grdholidaylist.DataSource = ds;
        grdholidaylist.DataBind();

    }
    protected void bindleaveupdateold(string id)
    {

        string strsql = "select * from tbl_holidaytype where id='" + ViewState["id"] + "'";
        DataSet ds1 = da.GetDataSetInline(strsql);
        if (ds1.Tables[0].Rows.Count > 0)
        {
            lblholidaylistname.Text = ds1.Tables[0].Rows[0]["listname"].ToString();
        }

        string str = "select * from holidays where levelistid='" + ViewState["id"] + "' and status=1 and archive=0 order by eventdate";
        DataSet ds = da.GetDataSetInline(str);
        grdholidaylist.DataSource = ds;
        grdholidaylist.DataBind();

    }
    protected string showapplies(string val1)
    {
        string strret = "0 " + fns.getltext("Employees", "2") + "";


        string strsql = "select count(id) as tcount from tbl_employee where HolidayList='" + val1 + "' and status=1 and archive=0";
        DataSet dsemp = da.GetDataSetInline(strsql);
        strret = dsemp.Tables[0].Rows[0]["tcount"].ToString()+ " " + fns.getltext("Employees", "2") + "";
        return strret;
    }

    protected string shownoofholiday(string val1)
    {
        string strret = "0";

        string strsql = "select count(id) as tcount from holidays where levelistid='" + val1 + "' and status=1 and archive=0";
      DataSet ds= da.GetDataSetInline(strsql);
      if (ds.Tables[0].Rows.Count > 0)
      {
          strret = ds.Tables[0].Rows[0]["tcount"].ToString();
      }

        return strret;
    }

    protected void grduser_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        HiddenField hdndefault = (HiddenField)e.Row.FindControl("hdndefault");
        LinkButton linksetdefault = (LinkButton)e.Row.FindControl("linksetdefault");
        LinkButton linksetdefault1 = (LinkButton)e.Row.FindControl("linksetdefault1");
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (hdndefault != null)
            {

                if (hdndefault.Value == "1")
                {
                    linksetdefault.Visible = true;
                    linksetdefault1.Visible = false;

                }
                else
                {
                    linksetdefault.Visible = false;
                    linksetdefault1.Visible = true;
                }

                e.Row.Attributes["onmouseover"] = "javascript:SetMouseOver(this)";
                e.Row.Attributes["onmouseout"] = "javascript:SetMouseOut(this)";
            }

        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            for (int i = 1; i < e.Row.Cells.Count; i++)
            {

                e.Row.Cells[i].Text = fns.getltext(e.Row.Cells[i].Text, "2");
            }
        }

    }
    protected void grdholidaylist_PreRender(object sender, System.EventArgs e)
    {

        try
        {

            if (grdholidaylist.Rows.Count > 0)
            {

                grdholidaylist.UseAccessibleHeader = true;
                grdholidaylist.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void linksetdefault_Click(object sender, EventArgs e)
    {

        LinkButton link = (LinkButton)grduser.FindControl("linksetdefault");
        link = (LinkButton)sender;
        GridViewRow gvr = (GridViewRow)link.NamingContainer;
        int id = Convert.ToInt32(grduser.DataKeys[gvr.RowIndex].Value);


        string strsql = "update tbl_holidaytype set isdefault='0' where id='" + id.ToString() + "' and regid='" + Request.Cookies["cadmin"]["id"] + "'";
        da.executeDMLQueryinline(strsql);

        //strsql = "update tbl_holidaytype set isdefault='1' where id='" + id.ToString() + "'";
        //da.executeDMLQueryinline(strsql);

        binddata();


    }

    protected void linksetdefault1_Click(object sender, EventArgs e)
    {
        LinkButton link = (LinkButton)grduser.FindControl("linksetdefault1");
        link = (LinkButton)sender;
        GridViewRow gvr = (GridViewRow)link.NamingContainer;
        int id = Convert.ToInt32(grduser.DataKeys[gvr.RowIndex].Value);
       


        string strsql = "update tbl_holidaytype set isdefault='0' where  regid='" + Request.Cookies["cadmin"]["id"] + "'";
        da.executeDMLQueryinline(strsql);
        strsql = "update tbl_holidaytype set isdefault='1' where id='" + id.ToString() + "'";
        da.executeDMLQueryinline(strsql);
        binddata();
    }

    protected void btnadd_click(object sender, EventArgs e)
    {
        if (btnadd.Text == "Save")
        {

            SqlParameter[] prm = new SqlParameter[4];
            prm[0] = new SqlParameter("@listname", txtlistname.Text);
            prm[1] = new SqlParameter("@listdetails", txtlistdesc.Text);
            prm[2] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"].ToString());
            prm[3] = new SqlParameter("@cdate", da.getcdatetime());
            da.executeDMLQuery("tbl_holidaytypeinsertdate", prm);
        }
        else
        {
            SqlParameter[] prm = new SqlParameter[3];
            prm[0] = new SqlParameter("@listname", txtlistname.Text);
            prm[1] = new SqlParameter("@listdetails", txtlistdesc.Text);
            prm[2] = new SqlParameter("@id", ViewState["id"].ToString());
            da.executeDMLQuery("tbl_holidaytypeupdate", prm);
        }
        txtlistname.Text = "";
        txtlistdesc.Text = "";
       
        btnadd.Text = "Save";
        hdnmodel1.Value = "0";
        myModalform.Style.Add("display", "none");

        binddata();
    }

    protected void btncancel_click(object sender, EventArgs e)
    {
        txtlistname.Text = "";
        txtlistdesc.Text = "";
        btnadd.Text = "Save";
        hdnmodel1.Value = "0";
        myModalform.Style.Add("display", "none");
    }

    protected void btnaddholiday_click(object sender, EventArgs e)
    {
        if (btnaddholiday.Text == "Save")
        {

            SqlParameter[] prm = new SqlParameter[7];
            prm[0] = new SqlParameter("@title", txtnameholiday.Text);
            prm[1] = new SqlParameter("@eventdate", eventdate.Text);
            if (chkisfloating.Checked)
            {
                prm[2] = new SqlParameter("@isfloating", "1");
            }
            else
            {
                prm[2] = new SqlParameter("@isfloating", "2");
            }
            prm[3] = new SqlParameter("@leavedesc", txtholidaydesc.Text);
            prm[4] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"].ToString());
            prm[5] = new SqlParameter("@levelistid",hdndefaulttypeid.Value);
            if (chkisnationalholiday.Checked)
            {
                prm[6] = new SqlParameter("@chkisnationalholiday", "1");
            }
            else
            {
                prm[6] = new SqlParameter("@chkisnationalholiday", "0");
            }
            
            da.executeDMLQuery("holidaysinsert_reg", prm);
        }
        else
        {
            SqlParameter[] prm = new SqlParameter[6];
            prm[0] = new SqlParameter("@title", txtnameholiday.Text);
            prm[1] = new SqlParameter("@eventdate", eventdate.Text);
            if (chkisfloating.Checked)
            {
                prm[2] = new SqlParameter("@isfloating", "1");
            }
            else
            {
                prm[2] = new SqlParameter("@isfloating", "2");
            }
            prm[3] = new SqlParameter("@leavedesc", txtholidaydesc.Text);
            prm[4] = new SqlParameter("@id", ViewState["hid"].ToString());
            if (chkisnationalholiday.Checked)
            {
                prm[5] = new SqlParameter("@chkisnationalholiday", "1");
            }
            else
            {
                prm[5] = new SqlParameter("@chkisnationalholiday", "0");
            }

            da.executeDMLQuery("holidaysupdate_reg", prm);
        }
        txtnameholiday.Text = "";
        eventdate.Text = "";
        txtholidaydesc.Text = "";
        chkisfloating.Checked = false;
        chkisnationalholiday.Checked = false;
        btnaddholiday.Text = "Save";
        myModalform1.Style.Add("display", "none");
        bindleaveupdate();
    }
    protected void btncancelholiday_click(object sender, EventArgs e)
    {
        txtnameholiday.Text = "";
        eventdate.Text = "";
        txtholidaydesc.Text = "";
        chkisfloating.Checked = false;
        btnaddholiday.Text = "Save";
        myModalform1.Style.Add("display", "none");
        
    }
    protected void btncancelholiday1_click(object sender, EventArgs e)
    {
        TextBox3.Text = "";
        Date2.Text = "";
        TextBox4.Text = ""; 
        Button5.Text = "Save";
        myModalform2.Style.Add("display", "none");

    }
    protected void btnaddholiday1_click(object sender, EventArgs e)
    {
        if (Button5.Text == "Save")
        {

            SqlParameter[] prm = new SqlParameter[4];
            prm[0] = new SqlParameter("@title", TextBox3.Text);
            prm[1] = new SqlParameter("@eventdate", Date2.Text); 
            prm[2] = new SqlParameter("@leavedesc", TextBox4.Text);
            prm[3] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"].ToString()); 
            da.executeDMLQuery("restrictedholidayinsert_reg", prm);
        }
        else
        {
            SqlParameter[] prm = new SqlParameter[4];
            prm[0] = new SqlParameter("@title", TextBox3.Text);
            prm[1] = new SqlParameter("@eventdate", Date2.Text); 
            prm[2] = new SqlParameter("@leavedesc", TextBox4.Text);
            prm[3] = new SqlParameter("@id", ViewState["rhid"].ToString()); 
            da.executeDMLQuery("restrictedholidayholidaysupdate_reg", prm);
        }
        TextBox3.Text = "";
        Date2.Text = "";
        TextBox4.Text = "";

        Button5.Text = "Save";
        myModalform2.Style.Add("display", "none");
        bindRestrictedleave();
    }


}