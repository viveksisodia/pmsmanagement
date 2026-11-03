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
public partial class weeklyoffsetup : System.Web.UI.Page
{
    DataAccess da = new DataAccess();
    SqlParameter[] prm;
    DataSet ds = new DataSet();
    CommonFunction cmg = new CommonFunction();
    CommonFunction cmf = new CommonFunction();
    public function fns = new function();
    protected void Page_Load(object sender, EventArgs e)
    {
        chk1.Attributes.Add("onclick", "changechk('chk1','div1')");
        chk2.Attributes.Add("onclick", "changechk('chk2','div2')");
        chk3.Attributes.Add("onclick", "changechk('chk3','div3')");
        chk4.Attributes.Add("onclick", "changechk('chk4','div4')");
        chk5.Attributes.Add("onclick", "changechk('chk5','div5')");
        chk6.Attributes.Add("onclick", "changechk('chk6','div6')");
        chk7.Attributes.Add("onclick", "changechk('chk7','div7')");
        if (!Page.IsPostBack)
        {


            binddata();
        }
        if (!Page.IsPostBack)
        {
            fns.changeddltext(ddl1);
            fns.changeddltext(ddl2);
            fns.changeddltext(ddl3);
            fns.changeddltext(ddl4);
            fns.changeddltext(ddl5);
            fns.changeddltext(ddl6);
            fns.changeddltext(ddl7);
        }
    }

    protected void binddata()
    {
        string strsql = "select * from tbl_weekoffmaster where status=1 and archive=0 and regid='" + Request.Cookies["cadmin"]["id"] + "'";
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
    protected string showapplies(string val1)
    {
        string strret = "0 " + fns.getltext("Employees", "2") + "";
        string strsql = "select count(id) as tcount from tbl_employee where WeeklyOffPolicy='" + val1 + "' and status=1 and archive=0";
        DataSet dsemp = da.GetDataSetInline(strsql);
        strret = dsemp.Tables[0].Rows[0]["tcount"].ToString() + " " + fns.getltext("Employees", "2") + "";
        return strret;
    }
    protected void linksetdefault_Click(object sender, EventArgs e)
    {

        LinkButton link = (LinkButton)grduser.FindControl("linksetdefault");
        link = (LinkButton)sender;
        GridViewRow gvr = (GridViewRow)link.NamingContainer;
        int id = Convert.ToInt32(grduser.DataKeys[gvr.RowIndex].Value);


        string strsql = "update tbl_weekoffmaster set isdefault='0' where id='" + id.ToString() + "' and regid='" + Request.Cookies["cadmin"]["id"] + "'";
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



        string strsql = "update tbl_weekoffmaster set isdefault='0' where  regid='" + Request.Cookies["cadmin"]["id"] + "'";
        da.executeDMLQueryinline(strsql);
        strsql = "update tbl_weekoffmaster set isdefault='1' where id='" + id.ToString() + "'";
        da.executeDMLQueryinline(strsql);
        binddata();
    }
    protected void grduser_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        HiddenField hdndefault = (HiddenField)e.Row.FindControl("hdndefault");
        LinkButton linksetdefault = (LinkButton)e.Row.FindControl("linksetdefault");
        LinkButton linksetdefault1 = (LinkButton)e.Row.FindControl("linksetdefault1");
        if (e.Row.RowType == DataControlRowType.DataRow)
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

            //e.Row.Attributes["onmouseover"] = "javascript:SetMouseOver(this)";
            //e.Row.Attributes["onmouseout"] = "javascript:SetMouseOut(this)";


        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].Text = fns.getltext(e.Row.Cells[i].Text, "2");
            }
        }

    }

    protected void btnadd_click(object sender, EventArgs e)
    {

        string isweekoff1 = "0";
        string isweekoff1type = "0";
        string isweekoff2 = "0";
        string isweekoff2type = "0";
        string isweekoff3 = "0";
        string isweekoff3type = "0";
        string isweekoff4 = "0";
        string isweekoff4type = "0";
        string isweekoff5 = "0";
        string isweekoff5type = "0";
        string isweekoff6 = "0";
        string isweekoff6type = "0";
        string isweekoff7 = "0";
        string isweekoff7type = "0";

        if (chk1.Checked)
        {
            isweekoff1 = "1";
            isweekoff1type = ddl1.SelectedValue;

        }
        if (chk2.Checked)
        {
            isweekoff2 = "1";
            isweekoff2type = ddl2.SelectedValue;

        }
        if (chk3.Checked)
        {
            isweekoff3= "1";
            isweekoff3type = ddl3.SelectedValue;

        }
        if (chk4.Checked)
        {
            isweekoff4 = "1";
            isweekoff4type = ddl4.SelectedValue;

        }
        if (chk5.Checked)
        {
            isweekoff5 = "1";
            isweekoff5type = ddl5.SelectedValue;

        }
        if (chk6.Checked)
        {
            isweekoff6 = "1";
            isweekoff6type = ddl6.SelectedValue;

        }
        if (chk7.Checked)
        {
            isweekoff7 = "1";
            isweekoff7type = ddl7.SelectedValue;

        }


        if (btnadd.Text == "Save")
        {

            SqlParameter[] prm = new SqlParameter[17];
            prm[0] = new SqlParameter("@weekoffname", txtlistname.Text);
            prm[1] = new SqlParameter("@isweekoff1", isweekoff1);
            prm[2] = new SqlParameter("@isweekoff1type", isweekoff1type);
            prm[3] = new SqlParameter("@isweekoff2", isweekoff2);
            prm[4] = new SqlParameter("@isweekoff2type", isweekoff2type);
            prm[5] = new SqlParameter("@isweekoff3", isweekoff3);
            prm[6] = new SqlParameter("@isweekoff3type", isweekoff3type);
            prm[7] = new SqlParameter("@isweekoff4", isweekoff4);
            prm[8] = new SqlParameter("@isweekoff4type", isweekoff4type);
            prm[9] = new SqlParameter("@isweekoff5", isweekoff5);
            prm[10] = new SqlParameter("@isweekoff5type", isweekoff5type);
            prm[11] = new SqlParameter("@isweekoff6", isweekoff6);
            prm[12] = new SqlParameter("@isweekoff6type", isweekoff6type);
            prm[13] = new SqlParameter("@isweekoff7", isweekoff7);
            prm[14] = new SqlParameter("@isweekoff7type",isweekoff7type);
            prm[15] = new SqlParameter("@cdate", da.getcdatetime());
            prm[16] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"].ToString());
            da.executeDMLQuery("tbl_weekoffnameinsert", prm);
        }
        else
        {
            SqlParameter[] prm = new SqlParameter[16];
            prm[0] = new SqlParameter("@weekoffname", txtlistname.Text);
            prm[1] = new SqlParameter("@isweekoff1", isweekoff1);
            prm[2] = new SqlParameter("@isweekoff1type", isweekoff1type);
            prm[3] = new SqlParameter("@isweekoff2", isweekoff2);
            prm[4] = new SqlParameter("@isweekoff2type", isweekoff2type);
            prm[5] = new SqlParameter("@isweekoff3", isweekoff3);
            prm[6] = new SqlParameter("@isweekoff3type", isweekoff3type);
            prm[7] = new SqlParameter("@isweekoff4", isweekoff4);
            prm[8] = new SqlParameter("@isweekoff4type", isweekoff4type);
            prm[9] = new SqlParameter("@isweekoff5", isweekoff5);
            prm[10] = new SqlParameter("@isweekoff5type", isweekoff5type);
            prm[11] = new SqlParameter("@isweekoff6", isweekoff6);
            prm[12] = new SqlParameter("@isweekoff6type", isweekoff6type);
            prm[13] = new SqlParameter("@isweekoff7", isweekoff7);
            prm[14] = new SqlParameter("@isweekoff7type", isweekoff7type);
            prm[15] = new SqlParameter("@id", ViewState["id"].ToString());
            da.executeDMLQuery("tbl_weekoffnameupdate", prm);
        }
        txtlistname.Text = "";
        chk1.Checked = false;
        chk2.Checked = false;
        chk3.Checked = false;
        chk4.Checked = false;
        chk5.Checked = false;
        chk6.Checked = false;
        chk7.Checked = false;
        ddl1.SelectedValue = "1";
        ddl2.SelectedValue = "1";
        ddl3.SelectedValue = "1";
        ddl4.SelectedValue = "1";
        ddl5.SelectedValue = "1";
        ddl6.SelectedValue = "1";
        ddl7.SelectedValue = "1";

        div1.Style.Add("display", "none");
        div2.Style.Add("display", "none");
        div3.Style.Add("display", "none");
        div4.Style.Add("display", "none");
        div5.Style.Add("display", "none");
        div6.Style.Add("display", "none");
        div7.Style.Add("display", "none");
       // txtlistdesc.Text = "";

        btnadd.Text = "Save";
       // hdnmodel1.Value = "0";
        myModalform4.Style.Add("display", "none");

        binddata();
    }

    protected string getddlvalueweek(string val)
    {
        string strret="";
        switch(val)
        {
            case "1":
                strret= fns.getltext("All", "2");
                break;
                case "2":
                strret="1st";
                break;
                case "3":
                strret="2nd";
                break;
                case "4":
                strret="3rd";
                break;
                case "5":
                strret="4th";
                break;
                case "6":
                strret="5th";
                break;
                case "7":
                strret= fns.getltext("Odd(1,3)", "2");
                break;
                case "8":
                strret= fns.getltext("Odd(1,3,5)", "2");
                break;
                case "9":
                strret= fns.getltext("Even(2,4)", "2");
                break;

        }


        return strret;
    }


    protected string showweekdays(string w1, string wt1, string w2, string wt2, string w3, string wt3, string w4, string wt4, string w5, string wt5, string w6, string wt6, string w7, string wt7)
    {


        string strret = "";

        if (w1 == "1")
        {
            if (strret == string.Empty)
            {
                strret = getddlvalueweek(wt1) + " " + fns.getltext("Sunday", "2") + " ";
            }
            else
            {
                strret +=" + "+ getddlvalueweek(wt1) + " " + fns.getltext("Sunday", "2") + " ";
            }
        }
        if (w2 == "1")
        {
            if (strret == string.Empty)
            {
                strret = getddlvalueweek(wt2) + " " + fns.getltext("Monday", "2") + " ";
            }
            else
            {
                strret += " + " + getddlvalueweek(wt2) + " " + fns.getltext("Monday", "2") + " ";
            }
        }
        if (w3 == "1")
        {
            if (strret == string.Empty)
            {
                strret = getddlvalueweek(wt3) + " " + fns.getltext("Tuesday", "2") + " ";
            }
            else
            {
                strret += " + " + getddlvalueweek(wt3) + " " + fns.getltext("Tuesday", "2") + " ";
            }
        }

        if (w4 == "1")
        {
            if (strret == string.Empty)
            {
                strret = getddlvalueweek(wt4) + " " + fns.getltext("Wednesday", "2") + " ";
            }
            else
            {
                strret += " + " + getddlvalueweek(wt4) + " " + fns.getltext("Wednesday", "2") + " ";
            }
        }

        if (w5 == "1")
        {
            if (strret == string.Empty)
            {
                strret = getddlvalueweek(wt5) + " " + fns.getltext("Thrusday", "2") + " ";
            }
            else
            {
                strret += " + " + getddlvalueweek(wt5) + " " + fns.getltext("Thrusday", "2") + " ";
            }
        }

        if (w6 == "1")
        {
            if (strret == string.Empty)
            {
                strret = getddlvalueweek(wt6) + " " + fns.getltext("Friday", "2") + " ";
            }
            else
            {
                strret += " + " + getddlvalueweek(wt6) + " " + fns.getltext("Friday", "2") + " ";
            }
        }

        if (w7 == "1")
        {
            if (strret == string.Empty)
            {
                strret = getddlvalueweek(wt7) + " " + fns.getltext("Saturday", "2") + " ";
            }
            else
            {
                strret += " + " + getddlvalueweek(wt7) + " " + fns.getltext("Saturday", "2") + " ";
            }
        }



        return strret;
    }


    protected void btncancel_click(object sender, EventArgs e)
    {
        txtlistname.Text = "";
        chk1.Checked = false;
        chk2.Checked = false;
        chk3.Checked = false;
        chk4.Checked = false;
        chk5.Checked = false;
        chk6.Checked = false;
        chk7.Checked = false;
        ddl1.SelectedValue = "1";
        ddl2.SelectedValue = "1";
        ddl3.SelectedValue = "1";
        ddl4.SelectedValue = "1";
        ddl5.SelectedValue = "1";
        ddl6.SelectedValue = "1";
        ddl7.SelectedValue = "1";

        div1.Style.Add("display", "none");
        div2.Style.Add("display", "none");
        div3.Style.Add("display", "none");
        div4.Style.Add("display", "none");
        div5.Style.Add("display", "none");
        div6.Style.Add("display", "none");
        div7.Style.Add("display", "none");
        //txtlistdesc.Text = "";
        btnadd.Text = "Save";
       // hdnmodel1.Value = "0";
        myModalform4.Style.Add("display", "none");
    }


   
    protected void useupdate(string id)
    {
        string strsql = "select * from tbl_weekoffmaster where id='" + id + "'";
        DataSet ds = da.GetDataSetInline(strsql);
        txtlistname.Text = ds.Tables[0].Rows[0]["weekoffname"].ToString();
        if (ds.Tables[0].Rows[0]["isweekoff1"].ToString() == "1")
        {
            chk1.Checked = true;
            div1.Style.Add("display", "block");
            ddl1.SelectedValue = ds.Tables[0].Rows[0]["isweekoff1type"].ToString();
        }
        else
        {
            chk1.Checked = false;
            div1.Style.Add("display", "none");
            ddl1.SelectedValue = "1";
        }

        if (ds.Tables[0].Rows[0]["isweekoff2"].ToString() == "1")
        {
            chk2.Checked = true;
            div2.Style.Add("display", "block");
            ddl2.SelectedValue = ds.Tables[0].Rows[0]["isweekoff2type"].ToString();
        }
        else
        {
            chk2.Checked = false;
            div2.Style.Add("display", "none");
            ddl2.SelectedValue = "1";
        }
        if (ds.Tables[0].Rows[0]["isweekoff3"].ToString() == "1")
        {
            chk3.Checked = true;
            div3.Style.Add("display", "block");
            ddl3.SelectedValue = ds.Tables[0].Rows[0]["isweekoff3type"].ToString();
        }
        else
        {
            chk3.Checked = false;
            div3.Style.Add("display", "none");
            ddl3.SelectedValue = "1";
        }
        if (ds.Tables[0].Rows[0]["isweekoff4"].ToString() == "1")
        {
            chk4.Checked = true;
            div4.Style.Add("display", "block");
            ddl4.SelectedValue = ds.Tables[0].Rows[0]["isweekoff4type"].ToString();
        }
        else
        {
            chk4.Checked = false;
            div4.Style.Add("display", "none");
            ddl4.SelectedValue = "1";
        }
        if (ds.Tables[0].Rows[0]["isweekoff5"].ToString() == "1")
        {
            chk5.Checked = true;
            div5.Style.Add("display", "block");
            ddl5.SelectedValue = ds.Tables[0].Rows[0]["isweekoff5type"].ToString();
        }
        else
        {
            chk5.Checked = false;
            div5.Style.Add("display", "none");
            ddl5.SelectedValue = "1";
        }
        if (ds.Tables[0].Rows[0]["isweekoff6"].ToString() == "1")
        {
            chk6.Checked = true;
            div6.Style.Add("display", "block");
            ddl6.SelectedValue = ds.Tables[0].Rows[0]["isweekoff6type"].ToString();
        }
        else
        {
            chk6.Checked = false;
            div6.Style.Add("display", "none");
            ddl6.SelectedValue = "1";
        }
        if (ds.Tables[0].Rows[0]["isweekoff7"].ToString() == "1")
        {
            chk7.Checked = true;
            div7.Style.Add("display", "block");
            ddl7.SelectedValue = ds.Tables[0].Rows[0]["isweekoff7type"].ToString();
        }
        else
        {
            chk7.Checked = false;
            div7.Style.Add("display", "none");
            ddl7.SelectedValue = "1";
        }



        btnadd.Text = "Update";
       // hdnmodel1.Value = "1";
        myModalform4.Style.Add("display", "block");
        // Page.RegisterClientScriptBlock("js","alert('hi');document.getElementById('myModalform').style.display = 'block';");

    }

}