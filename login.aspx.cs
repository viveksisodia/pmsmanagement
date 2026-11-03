using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonClass;
using System.IO;
using System.Web.Security;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
public partial class login : System.Web.UI.Page
{
    CommonFunction commf;    
    UserAuthentication uauthentication;
    public string msg = "";
     string uid;
     string type;
     string idu;
    DataSet ds = new DataSet();
    dataaccesscomm da = new dataaccesscomm();
    CommonFunction cmf=new CommonFunction();
    public function fns = new function();
    string ddlfinancialyear = "";
    //public string msg = "";
    protected void Page_Load(object sender, EventArgs e)
    {
      // Response.Write(Convert.ToDateTime("21 September2026 12:33:00"));


        Session.Remove("leftmenudata");
        // Response.Write(cmf.Decrypt("Crssisw681OsPwkhE2S4ztTAlldlld"));
        if (Request.Url.ToString().Contains("ictbase.erpabsolute.net"))
        {
            lblplogo.Text = "<a href='https://ictbaselimited.com' target='_blank'><img src='/partner-logo/ICTBASE.png' height='120px' title='Partner Logo'/></a>";
            lblmark.Text = "<p style='color:#000 !important;background:#ccc;display: inline-block;padding: 2px 8px;'>Powered & Marketed by <a href='https://ictbaselimited.com/' target='_blank' style='color:#277fd9;'>ICTBASE</a></p>";


            // Response.Redirect(Request.Url.ToString().Replace("http://", "https://"));
        }
        if (Request.Url.ToString().Contains("erpabsolute.net"))
        {
            if (Request.Url.ToString().Contains("http://"))
            {
                 Response.Redirect(Request.Url.ToString().Replace("http://", "https://"));
            }
        }

       
        if (Request.QueryString["u_id"] != null)
        {
            commf = new CommonFunction();
            uauthentication = new UserAuthentication();
            SqlParameter[] sparam = new SqlParameter[2];
            sparam[0] = new SqlParameter("@id", cmf.Decrypt(Request.QueryString["u_id"].ToString()));
            sparam[1] = new SqlParameter("@regid", cmf.Decrypt(Request.QueryString["regid"].ToString()));

            //Response.Write(cmf.Decrypt(Request.QueryString["u_id"].ToString()));
           // Response.Write(cmf.Decrypt(Request.QueryString["regid"].ToString()));
            ds = da.GetDataSet("tbl_employee_login_direct", sparam);
            if (ds.Tables[0].Rows.Count > 0)
            {
                uid = ds.Tables[0].Rows[0]["id"].ToString();
                Session["Uid"] = uid;
                type = ds.Tables[0].Rows[0]["roleid"].ToString();


                string redurl = FormsAuthentication.GetRedirectUrl("1", true);
                FormsAuthentication.RedirectFromLoginPage("1", false);
                HttpCookie Employee = new HttpCookie("cadmin");
                Employee.Values.Add("id", ds.Tables[0].Rows[0]["regid"].ToString());
                HttpContext.Current.Response.Cookies.Add(Employee);

                bindfinancialyear(ds.Tables[0].Rows[0]["regid"].ToString());

                if (uauthentication.SetCookie(uid))
                {
                    
                    FormsAuthentication.RedirectFromLoginPage("1", false);
                    Response.Redirect("/");
                   // Response.Redirect("reportsmanagemanet.aspx");
                }

            }
        }

        if (Request.QueryString["prm1"] != null && Request.QueryString["prm2"] != null && Request.QueryString["regid"] != null)
        {
            if (authenticationweb(Request.QueryString["prm1"].ToString().Trim(), Request.QueryString["prm2"].ToString().Trim(), Request.QueryString["regid"].ToString().Trim()) == 1)
            {
                commf = new CommonFunction();
                uauthentication = new UserAuthentication();
                if (uauthentication.SetCookie(uid))
                {


                    HttpCookie usertype = new HttpCookie("usertype");
                    usertype.Values.Add("id", type);
                    HttpContext.Current.Response.Cookies.Add(usertype);

                    string redurl = FormsAuthentication.GetRedirectUrl("1", false);
                    FormsAuthentication.RedirectFromLoginPage("1", false);
                    HttpCookie iduser = new HttpCookie("iduser");
                    iduser.Values.Add("idu", uid);
                    HttpContext.Current.Response.Cookies.Add(iduser);
                    string branchid = "1";
                    HttpCookie leadusercid2 = new HttpCookie("branchid");
                    leadusercid2.Values.Add("bid", branchid);
                    Response.Cookies.Add(leadusercid2);
                    if (Request.Cookies["cadmin"] != null)
                    {
                        if (Request.Cookies["cadmin"]["id"] != null)
                        {

                             
                        }
                    }
                   
                  
                    if (Request.QueryString["returnurl"] != null)
                    {
                        Response.Redirect(Request.QueryString["returnurl"].ToString());
                    }
                    else
                    {
                        Response.Redirect("/userprofile.aspx");
                    }



                }
            }
            else
            {

                 
                    msg = "Invalid User Name or Password";
                    pwdtxt.Text = "";
                    lblmsg.Text = msg;

                
            }

        }



        if (Request.QueryString["id"] != null && Request.QueryString["regid"] != null && Request.QueryString["returnurl"] != null)
        {
            commf = new CommonFunction();
            uauthentication = new UserAuthentication();
            SqlParameter[] sparam = new SqlParameter[2];
            sparam[0] = new SqlParameter("@id", cmf.Decrypt(Request.QueryString["id"].ToString()));
            sparam[1] = new SqlParameter("@regid", cmf.Decrypt(Request.QueryString["regid"].ToString()));

            ds = da.GetDataSet("tbl_commonemployee_login_direct", sparam);
            if (ds.Tables[0].Rows.Count > 0)
            {
                
                uid = ds.Tables[0].Rows[0]["uid"].ToString();
                // Session["Uid"] = uid;
                // type = ds.Tables[0].Rows[0]["roleid"].ToString();
                if (Request.QueryString["cid"] != null)
                {
                    HttpCookie regcid = new HttpCookie("regcid");
                    regcid.Values.Add("regcidname", Request.QueryString["cid"]);
                    HttpContext.Current.Response.Cookies.Add(regcid);
                    //Response.Write(Request.QueryString["cid"]);
                    //Response.End();

                }

                string redurl = FormsAuthentication.GetRedirectUrl("1", true);
                FormsAuthentication.RedirectFromLoginPage("1", false);
                HttpCookie Employee = new HttpCookie("cadmin");
                Employee.Values.Add("id", ds.Tables[0].Rows[0]["regid"].ToString());
                HttpContext.Current.Response.Cookies.Add(Employee);

                HttpCookie database = new HttpCookie("database");
                database.Values.Add("databasename", ds.Tables[0].Rows[0]["databasename"].ToString());
                HttpContext.Current.Response.Cookies.Add(database);
          


                bindfinancialyear(ds.Tables[0].Rows[0]["regid"].ToString());

                if (uauthentication.SetCookie(uid))
                {

                    FormsAuthentication.RedirectFromLoginPage("1", false);




                    Response.Redirect(Request.QueryString["returnurl"].ToString());
                    // Response.Redirect("reportsmanagemanet.aspx");
                }

            }
        }


    }



    protected void bindfinancialyear(string cid)
    {

        DataAccess Da = new DataAccess();
        int yearid = Convert.ToDateTime(Da.getcdate()).Year;
        int monthid = Convert.ToDateTime(Da.getcdate()).Month;
        string ddlfinancialyeartype = "1";
        string ddlfrommonth = "";
        string ddltomonth = "";
        string fromdate = "01/01/" + yearid;
        string todate = "12/31/" + yearid;
        DataAccess da = new DataAccess();
        SqlParameter[] prm = new SqlParameter[1];
        prm[0] = new SqlParameter("@regid", cid);
        DataSet ds = da.GetDataSet("tbl_companydetails_selectbyregid", prm);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["txtnameofcompany"] = ds.Tables[0].Rows[0]["txtnameofcompany"].ToString();
            string dateofbooks = ds.Tables[0].Rows[0]["txtdateofbooks"].ToString();

            string curdate = da.getcdate();

            ddlfinancialyeartype = ds.Tables[0].Rows[0]["ddlfinancialyear"].ToString();
            ddlfrommonth = ds.Tables[0].Rows[0]["ddlfrommonth"].ToString();
            ddltomonth = ds.Tables[0].Rows[0]["ddltomonth"].ToString();

            string strsql = "select * from [dbo].[Fn_Interval]('" + dateofbooks + "','" + curdate + "') order by yearvalue desc";
            DataSet dsyear = da.GetDataSetInline(strsql);
            if (dsyear.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsyear.Tables[0].Rows.Count; i++)
                {
                    if (ddlfrommonth != string.Empty && ddlfrommonth != "0")
                    {
                        if(ddlfrommonth!="1")
                        {
                            int yearname = Convert.ToInt32(dsyear.Tables[0].Rows[i]["YearValue"].ToString());
                            if (yearname == yearid)
                            {

                                if (monthid > (Convert.ToInt32(ddltomonth)-1))
                                {
                                    ListItem li = new ListItem(dsyear.Tables[0].Rows[i]["YearValue"].ToString() + "-" + (yearname + 1).ToString(), dsyear.Tables[0].Rows[i]["YearValue"].ToString());

                                }
                            }
                            else
                            {
                                ListItem li = new ListItem(dsyear.Tables[0].Rows[i]["YearValue"].ToString() + "-" + (yearname + 1).ToString(), (yearname + 1).ToString());

                            }
                        }
                        else
                        {
                            int yearname = Convert.ToInt32(dsyear.Tables[0].Rows[i]["YearValue"].ToString());

                            ListItem li = new ListItem(dsyear.Tables[0].Rows[i]["YearValue"].ToString(), dsyear.Tables[0].Rows[i]["YearValue"].ToString());

                        }
                    }
                    else
                    {

                        if (ddlfinancialyeartype == "2")
                        {
                            int yearname = Convert.ToInt32(dsyear.Tables[0].Rows[i]["YearValue"].ToString());
                            if (yearname == yearid)
                            {

                                if (monthid > 3)
                                {
                                    ListItem li = new ListItem(dsyear.Tables[0].Rows[i]["YearValue"].ToString() + "-" + (yearname + 1).ToString(), dsyear.Tables[0].Rows[i]["YearValue"].ToString());

                                }
                            }
                            else
                            {
                                ListItem li = new ListItem(dsyear.Tables[0].Rows[i]["YearValue"].ToString() + "-" + (yearname + 1).ToString(), (yearname + 1).ToString());

                            }

                        }
                        else
                        {
                            int yearname = Convert.ToInt32(dsyear.Tables[0].Rows[i]["YearValue"].ToString());

                            ListItem li = new ListItem(dsyear.Tables[0].Rows[i]["YearValue"].ToString(), dsyear.Tables[0].Rows[i]["YearValue"].ToString());

                        }

                    }
                }
            }


            if (ddlfrommonth != string.Empty && ddlfrommonth != "0")
            {
                if (ddlfrommonth != "1")
                {
                    if (monthid < Convert.ToInt32(ddlfrommonth))
                    {
                        ddlfinancialyear = (yearid - 1).ToString();

                    }
                    else
                    {
                        ddlfinancialyear = yearid.ToString();
                    }
                }
                else
                {
                    ddlfinancialyear = yearid.ToString();
                }

            }
            else
            {
                if (ddlfinancialyeartype == "2")
                {
                    if (monthid < 4)
                    {
                        ddlfinancialyear = (yearid - 1).ToString();

                    }
                    else
                    {
                        ddlfinancialyear = yearid.ToString();
                    }
                }
                else
                {
                    ddlfinancialyear = yearid.ToString();
                }
            }


                HttpCookie Employee = new HttpCookie("fyear");
                Employee.Values.Add("id", ddlfinancialyear);
                HttpContext.Current.Response.Cookies.Add(Employee);
            if (ddlfrommonth != string.Empty && ddlfrommonth != "0")
            {

                if (ddlfrommonth != "1")
                {


                    if (monthid < Convert.ToInt32(ddlfrommonth))
                    {

                        fromdate = (Convert.ToInt32(ddlfrommonth)).ToString()+"/01/" + (yearid - 1);
                        int lsatday= DateTime.DaysInMonth(yearid, Convert.ToInt32(ddltomonth));
                        todate = ( Convert.ToInt32(ddltomonth)).ToString()+"/"+ lsatday +"/" + yearid;
                    }
                    else
                    {
                        fromdate = (Convert.ToInt32(ddlfrommonth)).ToString() + "/01/" + (yearid);
                        int lsatday = DateTime.DaysInMonth((yearid + 1), Convert.ToInt32(ddltomonth)); 
                        todate = (Convert.ToInt32(ddltomonth)).ToString() + "/" + lsatday + "/" + (yearid + 1);
                    }
                }
                else
                {
                    fromdate = (Convert.ToInt32(ddlfrommonth)).ToString() + "/01/" + (ddlfinancialyear);
                    int lsatday = DateTime.DaysInMonth(Convert.ToInt32(ddlfinancialyear), Convert.ToInt32(ddltomonth));
                    todate = (Convert.ToInt32(ddltomonth)).ToString() + "/" + lsatday + "/" + (ddlfinancialyear);
                   
                }

            }
            else
            {
                if (ddlfinancialyeartype == "2")
                {


                    if (monthid < 4)
                    {

                        fromdate = "04/01/" + (yearid - 1);
                        todate = "03/31/" + yearid;
                    }
                    else
                    {
                        fromdate = "04/01/" + (yearid);
                        todate = "03/31/" + (yearid + 1);
                    }
                }
                else
                {
                    fromdate = "01/01/" + ddlfinancialyear;
                    todate = "12/31/" + ddlfinancialyear;
                }
            }
                HttpCookie Employee1 = new HttpCookie("fromdate");
                Employee1.Values.Add("id", fromdate);
                HttpContext.Current.Response.Cookies.Add(Employee1);

                HttpCookie Employee2 = new HttpCookie("todate");
                Employee2.Values.Add("id", todate);
                HttpContext.Current.Response.Cookies.Add(Employee2);


           



        }
           
      


        // ddlfinancialyear
    }



    protected int authentication()
    {
        string uname = uidtxt.Text.Trim();
        string paswd = pwdtxt.Text.Trim();
        SqlParameter[] sparam = new SqlParameter[2];
        sparam[0] = new SqlParameter("@name", uname);
        sparam[1] = new SqlParameter("@password", paswd);
        ds = da.GetDataSet("tbl_commonemployee_login", sparam);
        if (ds.Tables[0].Rows.Count > 0)
        {
            uid = ds.Tables[0].Rows[0]["uid"].ToString();

            //Response.Write(ds.Tables[0].Rows[0]["databasename"].ToString());
            //Response.End();

           string userip = Request.UserHostAddress;

            dataaccessruntime darun = new dataaccessruntime();
            SqlParameter[] prmip = new SqlParameter[4]; 
            prmip[0] = new SqlParameter("@utype","1");
            prmip[1] = new SqlParameter("@uid", ds.Tables[0].Rows[0]["uid"].ToString());
            prmip[2] = new SqlParameter("@intime",da.getcdatetime());
            prmip[3] = new SqlParameter("@ipaddress", userip);
            darun.executeDMLQuery("Login_logdetails_usp", prmip, ds.Tables[0].Rows[0]["databasename"].ToString());


            Session["Uid"] = uid;
            

            string strdatabase = "";

            string redurl = FormsAuthentication.GetRedirectUrl("1", true);
            FormsAuthentication.RedirectFromLoginPage("1", false);
            HttpCookie Employee = new HttpCookie("cadmin");
            Employee.Values.Add("id", ds.Tables[0].Rows[0]["regid"].ToString());
            HttpContext.Current.Response.Cookies.Add(Employee);
            string opnamestr = "Operation";
          
            string strsql = "select * from tbl_crmPreferencesGeneral where regid='" + ds.Tables[0].Rows[0]["regid"].ToString() + "' and status=1 and archive=0";
            //Response.Write(strsql);
            //Response.End();
            DataSet dsv = darun.GetDataSetInline(strsql, ds.Tables[0].Rows[0]["databasename"].ToString());
            if (dsv.Tables[0].Rows.Count > 0)
            {
                if(dsv.Tables[0].Rows[0]["txtviewname"].ToString()!=string.Empty)
                {
                    opnamestr = dsv.Tables[0].Rows[0]["txtviewname"].ToString();
                }
               
            }
            
            HttpCookie opname = new HttpCookie("opname");
            opname.Values.Add("name", opnamestr);
            HttpContext.Current.Response.Cookies.Add(opname);

            HttpCookie database = new HttpCookie("database");
            database.Values.Add("databasename", ds.Tables[0].Rows[0]["databasename"].ToString());
            HttpContext.Current.Response.Cookies.Add(database); 

           bindfinancialyear(ds.Tables[0].Rows[0]["regid"].ToString()); 
            return 1;
        }
        else
        {
            return 0;
        }

    }

    protected int authenticationweb(string uname, string paswd,string regid)
    {
        
        SqlParameter[] sparam = new SqlParameter[3];
        sparam[0] = new SqlParameter("@name", uname);
        sparam[1] = new SqlParameter("@password", paswd);
        sparam[2] = new SqlParameter("@regid", regid);
        ds = da.GetDataSet("tbl_commonemployee_loginweb", sparam);
        if (ds.Tables[0].Rows.Count > 0)
        {
            uid = ds.Tables[0].Rows[0]["uid"].ToString();

            //Response.Write(ds.Tables[0].Rows[0]["databasename"].ToString());
            //Response.End();
            Session["Uid"] = uid;


            string strdatabase = "";

            string redurl = FormsAuthentication.GetRedirectUrl("1", true);
            FormsAuthentication.RedirectFromLoginPage("1", false);
            HttpCookie Employee = new HttpCookie("cadmin");
            Employee.Values.Add("id", ds.Tables[0].Rows[0]["regid"].ToString());
            HttpContext.Current.Response.Cookies.Add(Employee);

            HttpCookie opname = new HttpCookie("opname");
            string strsql = "select * from tbl_crmPreferencesGeneral where regid='" + Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0";
            //Response.Write(strsql);
            //Response.End();
            DataSet dsv = da.GetDataSetInline(strsql);
            if (dsv.Tables[0].Rows.Count > 0)
            {
                opname.Values.Add("name", dsv.Tables[0].Rows[0]["txtviewname"].ToString());
                HttpContext.Current.Response.Cookies.Add(opname);
            }
            else
            {

                opname.Values.Add("name", "Operation");
                HttpContext.Current.Response.Cookies.Add(opname);
            }


            HttpCookie database = new HttpCookie("database");
            database.Values.Add("databasename", ds.Tables[0].Rows[0]["databasename"].ToString());
            HttpContext.Current.Response.Cookies.Add(database);

            bindfinancialyear(ds.Tables[0].Rows[0]["regid"].ToString());
            return 1;
        }
        else
        {
            return 0;
        }

    }


    protected void resetpwd(object sender, EventArgs e)
    {
        string to = "";
        string from = "";
        string cc = "";
        string bcc = "";
        string sub = "";
        string body = "";
        string files = "";
        string[] chkdd = { "####" };
        string[] filesdata = files.Split(chkdd, StringSplitOptions.None);
      
        string strsql = "select * from tbl_commonemployee where emp_name='" + txtemailid.Text.Trim() + "' and status=1 and archive=0";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            try
            {

                string emailid = ds.Tables[0].Rows[0]["email_Id"].ToString();
                string changepwdmsg = "   Hello " + ds.Tables[0].Rows[0]["emp_name"].ToString() + "<br/>";
                changepwdmsg += "<br/>We have received a request to reset your Absolute ERP account password associated with this email address. If you have not placed this request, you can safely ignore this email and we assure you that your account is completely secure.";
                changepwdmsg += "<br/>If you do need to change your Absolute ERP Password, you can use the link given below.<br/><br/>";

                to = emailid;
                from = "noreply@absolutewebtech.com";
                sub = "Reset your Absolute ERP Account Password";
                ViewState["JoinAccount"] = "<a href='" + ConfigurationManager.AppSettings["domainname"].ToString() + "/changepwd.aspx?email=" + cmf.Encrypt(txtemailid.Text.Trim()) + "'>Change password</a>";

                changepwdmsg += ViewState["JoinAccount"];

                changepwdmsg += "<br/><br/>Thank you <br/>Absolute Teams";
                body = changepwdmsg;
                SendMail sm = new SendMail();
                sm.SendMailtoUser(to, from, cc, bcc, sub, body, filesdata);
                lblmsgchangepwdreques.Text = "An email with password reset link sent to your email address. Follow the instructions in the email to reset your account password and get back into your account.";
                pnlforgotpwd.Visible = false;
                pnlsendreques.Visible = true;
            }
            catch
            {

            }
        }
        else
        {
            lblmsgchangepwd.Text = "Email-Id Not Match";
       }  
    }


    protected void forgotpwdlink_click(object sender, EventArgs e)
    {
        pnlforgotpwd.Visible = true;
        pnllogin.Visible = false;
    }

    protected void submit_click(object sender, EventArgs e)
    {

        try
        {
            commf = new CommonFunction();
            uauthentication = new UserAuthentication();
            if (Page.IsValid)
            {
                //Response.Write(authentication());
                //Response.End();
                if (authentication() == 1)
                {
                    if (uauthentication.SetCookie(uid))
                    {
                       

                        HttpCookie usertype = new HttpCookie("usertype");
                        usertype.Values.Add("id", type);
                        HttpContext.Current.Response.Cookies.Add(usertype);

                        string redurl = FormsAuthentication.GetRedirectUrl("1", false);
                        FormsAuthentication.RedirectFromLoginPage("1", false);
                        HttpCookie iduser = new HttpCookie("iduser");
                        iduser.Values.Add("idu", uid);
                        HttpContext.Current.Response.Cookies.Add(iduser);
                        string branchid = "1";
                        HttpCookie leadusercid2 = new HttpCookie("branchid");
                        leadusercid2.Values.Add("bid", branchid);
                        Response.Cookies.Add(leadusercid2);
                        if (Request.Cookies["cadmin"] != null)
                        {
                            if (Request.Cookies["cadmin"]["id"] != null)
                            {

                                //if (Request.Cookies["cadmin"]["id"].ToString() == "255")
                                //{
                                //    Response.Redirect("BookingCalendar.aspx");
                                //}
                            }
                        }
                        if (Request.Url.ToString().Contains("crm.erpabsolute.net"))
                        {
                            Response.Redirect("/crm/");
                        }
                        if (Request.Url.ToString().Contains("erp.erpabsolute.net"))
                        {
                            if (Request.Cookies["cadmin"]["id"].ToString() == "2410")
                            {
                                Response.Redirect("/democomlink.aspx");
                            }
                        }


                        Response.Cookies["v_user"].Expires = DateTime.Now.AddDays(-1);

                        if (Request.QueryString["returnurl"] != null)
                        {
                            Response.Redirect(Request.QueryString["returnurl"].ToString());
                        }
                        else
                        {
                            Response.Redirect("/userprofile.aspx");
                        }

                      

                        }
                }
                else
                {
                    if (Request.Url.ToString().Contains("gardex.erpabsolute.net"))
                    {

                        string uname = uidtxt.Text.Trim();
                        SqlParameter[] sparam = new SqlParameter[1];
                        sparam[0] = new SqlParameter("@name", uname);
                        ds = da.GetDataSet("tbl_commonemployee_login_Check", sparam);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                                DataAccess daA = new DataAccess();
                                uid = ds.Tables[0].Rows[0]["uid"].ToString();
                                string strsqlp = "select Manage_Profile.* from Manage_Profile inner join tbl_employee on Manage_Profile.id=cast(tbl_employee.roleid as int) where tbl_employee.id='" + ds.Tables[0].Rows[0]["uid"].ToString() + "'  and Manage_Profile.status=1 and Manage_Profile.archive=0";
                                DataSet dsp = daA.GetDataSetInline(strsqlp);
                                if (dsp.Tables[0].Rows.Count > 0)
                                {
                                    if (dsp.Tables[0].Rows[0]["id"].ToString() != "1")
                                    {
                                        string stremp = "select isnull(totalattampt,0) as attampt from tbl_employee where id='" + ds.Tables[0].Rows[0]["uid"].ToString() + "' and status=1 and archive=0";
                                        DataSet dst = daA.GetDataSetInline(stremp);
                                        if (Convert.ToInt32(dst.Tables[0].Rows[0]["attampt"].ToString()) >= 2)
                                        {
                                            string strsql = "update tbl_employee set isactivelogin='2' where id='" + ds.Tables[0].Rows[0]["uid"].ToString() + "'";
                                            daA.executeDMLQueryinline(strsql);
                                            msg = "You login is blocked please contact to Admin!!";
                                            pwdtxt.Text = "";
                                            lblmsg.Text = msg;
                                        }
                                        else
                                        {
                                            int countattmpt = Convert.ToInt32(dst.Tables[0].Rows[0]["attampt"].ToString()) + 1;
                                            string strsql = "update tbl_employee set totalattampt='" + countattmpt + "' where id='" + ds.Tables[0].Rows[0]["uid"].ToString() + "'";
                                            daA.executeDMLQueryinline(strsql);

                                            msg = "Invalid User Name or Password ! Your remaining Attempts is " + (3 - countattmpt) + "";
                                            pwdtxt.Text = "";
                                            lblmsg.Text = msg;
                                        }
                                    }
                                    else
                                    {
                                        msg = "Invalid User Name or Password";
                                        pwdtxt.Text = "";
                                        lblmsg.Text = msg;
                                    }
                                }
                                else
                                {
                                    msg = "Invalid User Name or Password";
                                    pwdtxt.Text = "";
                                    lblmsg.Text = msg;
                                }
                           
                        }
                        else
                        {
                            msg = "Invalid User Name or Password";
                            pwdtxt.Text = "";
                            lblmsg.Text = msg;
                        }
                    }
                    else
                    {
                        msg = "Invalid User Name or Password";
                        pwdtxt.Text = "";
                        lblmsg.Text = msg;

                    }
                }               

            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }
}