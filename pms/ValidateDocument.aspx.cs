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
using System.Web.Services;

public partial class employee_lettertemplate : System.Web.UI.Page
{
    DataAccess da = new DataAccess();
    SqlParameter[] prm;
    DataSet ds = new DataSet();
    CommonFunction cmg = new CommonFunction();
    CommonFunction cmf = new CommonFunction();
    public string empdata = "";
    public string cufieldvalue = "";
    public string docinformation = "";
    functions fns = new functions();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {


           // docview.Width = 600;
            docview.Height = 600;
            
            if (Request.QueryString["id"] != null)
            {
               
                        string strsql = "select * from Pms_tbl_projectfiles where id='" + Request.QueryString["id"] + "'  and status=1 and archive=0";
                        DataSet dsdoc = da.GetDataSetInline(strsql);
                        if (dsdoc.Tables[0].Rows.Count > 0)
                        {
                    //string foldername = "~/employee/IdentityDocs/" + Request.Cookies["cadmin"]["id"].ToString() + "/";
                    docview.Watermarks.Clear();
                    docview.Document = dsdoc.Tables[0].Rows[0]["fileurl"].ToString();
                    //if (Request.QueryString["typeid"].ToString() == "1")
                            //{
                                //docview.Document = foldername + dsdoc.Tables[0].Rows[0]["licenseproofdoc"].ToString();
                                //docinformation = "<h1><u>" + fns.getEmpNameById(dsdoc.Tables[0].Rows[0]["empid"].ToString()) + "</u></h1><h3>Driver's License</h3>";
                                //docinformation += "<table class='table'>";
                                //docinformation += "<tr><td><b>Driver's License</b></td>";
                                //docinformation += "<td>" + dsdoc.Tables[0].Rows[0]["txtlicenseno"].ToString() + "</td></tr>";
                                //docinformation += "<tr><td><b>Date of Birth</b></td>";
                                //docinformation += "<td>" + cmf.dateConvertexcl(dsdoc.Tables[0].Rows[0]["dateofbirthlicense"].ToString()) + "</td></tr>";
                                //docinformation += "<tr><td><b>Name</b></td>";
                                //docinformation += "<td>" + dsdoc.Tables[0].Rows[0]["txtlicensename"].ToString() + "</td></tr>";
                                //docinformation += "<tr><td><b> Father's Name </b></td>";
                                //docinformation += "<td>" + dsdoc.Tables[0].Rows[0]["txtlicensefathername"].ToString() + "</td></tr>";
                                //docinformation += "</table>";

                            //}
                           
                        }
                    
            }



            docdetails.Text = docinformation;
        }
       
        if(Request.QueryString["isview"]!=null)
        {
            btnsave.Visible = false;
            btnreject.Visible = false;


        }

    }



    protected void btnaddfield_click(object sender, EventArgs e)
    {
       
    }

    protected void bindcusfield()
    {
       
    }

    protected void btnsave_click(object sender, EventArgs e)
    {



        string strsql = "";
        if (Request.QueryString["id"] != null)
        {
            if (Request.QueryString["type"] != null)
            {
                if (Request.QueryString["type"].ToString() == "1")
                {
                    if (Request.QueryString["typeid"].ToString() == "1")
                    {

                        strsql = "update tbl_empIdentitydocumentdetails set isvarify='1',isvarifydate='" + da.getcdatetime() + "',isvarifyuid='" + Request.Cookies["user"]["user_id"] + "' where id='" + Request.QueryString["id"] + "'";
                    }
                    if (Request.QueryString["typeid"].ToString() == "2")
                    {

                        strsql = "update tbl_empIdentitydocumentdetails set isvarify1='1',isvarifydate1='" + da.getcdatetime() + "',isvarifyuid1='" + Request.Cookies["user"]["user_id"] + "' where id='" + Request.QueryString["id"] + "'";
                    }
                    if (Request.QueryString["typeid"].ToString() == "3")
                    {

                        strsql = "update tbl_empIdentitydocumentdetails set isvarify2='1',isvarifydate2='" + da.getcdatetime() + "',isvarifyuid2='" + Request.Cookies["user"]["user_id"] + "' where id='" + Request.QueryString["id"] + "'";
                    }
                    if (Request.QueryString["typeid"].ToString() == "4")
                    {

                        strsql = "update tbl_empIdentitydocumentdetails set isvarify3='1',isvarifydate3='" + da.getcdatetime() + "',isvarifyuid3='" + Request.Cookies["user"]["user_id"] + "' where id='" + Request.QueryString["id"] + "'";
                    }
                    if (Request.QueryString["typeid"].ToString() == "5")
                    {

                        strsql = "update tbl_empIdentitydocumentdetails set isvarify4='1',isvarifydate4='" + da.getcdatetime() + "',isvarifyuid4='" + Request.Cookies["user"]["user_id"] + "' where id='" + Request.QueryString["id"] + "'";
                    }
                    if (strsql != string.Empty)
                    {
                        da.executeDMLQueryinline(strsql);
                    }
                }

                if (Request.QueryString["type"].ToString() == "2")
                {
                    strsql = "update tbl_employeeEducation set isvarify='1',isvarifydate='" + da.getcdatetime() + "',isvarifyuid='" + Request.Cookies["user"]["user_id"] + "' where id='" + Request.QueryString["id"] + "'";

                    if (strsql != string.Empty)
                    {
                        da.executeDMLQueryinline(strsql);
                    }
                }

                if (Request.QueryString["type"].ToString() == "3")
                {
                    strsql = "update tbl_employeeExperience set isvarify='1',isvarifydate='" + da.getcdatetime() + "',isvarifyuid='" + Request.Cookies["user"]["user_id"] + "' where id='" + Request.QueryString["id"] + "'";

                    if (strsql != string.Empty)
                    {
                        da.executeDMLQueryinline(strsql);
                    }
                }
            }
            }

        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", "alert('Document Approved Successfully!!');window.parent.location.href = window.parent.location.href;", true);


    }

    protected void btnreject_click(object sender, EventArgs e)
    {

        string strsql = "";
        if (Request.QueryString["id"] != null)
        {
            if (Request.QueryString["type"] != null)
            {
                if (Request.QueryString["type"].ToString() == "1")
                {
                    if (Request.QueryString["typeid"].ToString() == "1")
                    {

                        strsql = "update tbl_empIdentitydocumentdetails set isvarify='2',isvarifydate='" + da.getcdatetime() + "',isvarifyuid='" + Request.Cookies["user"]["user_id"] + "' where id='" + Request.QueryString["id"] + "'";
                    }
                    if (Request.QueryString["typeid"].ToString() == "2")
                    {

                        strsql = "update tbl_empIdentitydocumentdetails set isvarify1='2',isvarifydate1='" + da.getcdatetime() + "',isvarifyuid1='" + Request.Cookies["user"]["user_id"] + "' where id='" + Request.QueryString["id"] + "'";
                    }
                    if (Request.QueryString["typeid"].ToString() == "3")
                    {

                        strsql = "update tbl_empIdentitydocumentdetails set isvarify2='2',isvarifydate2='" + da.getcdatetime() + "',isvarifyuid2='" + Request.Cookies["user"]["user_id"] + "' where id='" + Request.QueryString["id"] + "'";
                    }
                    if (Request.QueryString["typeid"].ToString() == "4")
                    {

                        strsql = "update tbl_empIdentitydocumentdetails set isvarify3='2',isvarifydate3='" + da.getcdatetime() + "',isvarifyuid3='" + Request.Cookies["user"]["user_id"] + "' where id='" + Request.QueryString["id"] + "'";
                    }
                    if (Request.QueryString["typeid"].ToString() == "5")
                    {

                        strsql = "update tbl_empIdentitydocumentdetails set isvarify4='2',isvarifydate4='" + da.getcdatetime() + "',isvarifyuid4='" + Request.Cookies["user"]["user_id"] + "' where id='" + Request.QueryString["id"] + "'";
                    }
                    if (strsql != string.Empty)
                    {
                        da.executeDMLQueryinline(strsql);
                    }
                }

                if (Request.QueryString["type"].ToString() == "2")
                {
                    strsql = "update tbl_employeeEducation set isvarify='2',isvarifydate='" + da.getcdatetime() + "',isvarifyuid='" + Request.Cookies["user"]["user_id"] + "' where id='" + Request.QueryString["id"] + "'";

                    if (strsql != string.Empty)
                    {
                        da.executeDMLQueryinline(strsql);
                    }
                }
                if (Request.QueryString["type"].ToString() == "3")
                {
                    strsql = "update tbl_employeeExperience set isvarify='2',isvarifydate='" + da.getcdatetime() + "',isvarifyuid='" + Request.Cookies["user"]["user_id"] + "' where id='" + Request.QueryString["id"] + "'";

                    if (strsql != string.Empty)
                    {
                        da.executeDMLQueryinline(strsql);
                    }
                }
            }
        }

        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", "alert('Document Rejected Successfully!!');window.parent.location.href = window.parent.location.href;", true);



    }
}