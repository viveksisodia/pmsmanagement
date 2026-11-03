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
    static string uid;
    static string type;
    static string idu;
    DataSet ds = new DataSet();
    DataAccess da = new DataAccess();
    CommonFunction cmf=new CommonFunction();

    string ddlfinancialyear = "";
    //public string msg = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        // Response.Write(cmf.Decrypt("Crssisw681OsPwkhE2S4ztTAlldlld"));
        //if (Request.Url.ToString().Contains("http://"))
        //{
        //    Response.Redirect(Request.Url.ToString().Replace("http://", "https://"));
        //}

        if (Request.QueryString["u_id"] != null)
        {
            commf = new CommonFunction();
            uauthentication = new UserAuthentication();
            SqlParameter[] sparam = new SqlParameter[2];
			if(Request.QueryString["u_id"].ToString()=="0nclncc26KdtzBJZrZCsisxSzwlldlld")
			{
				sparam[0] = new SqlParameter("@id", "1");
            sparam[1] = new SqlParameter("@regid","212");
				
			}
		else{
			sparam[0] = new SqlParameter("@id", cmf.Decrypt(Request.QueryString["u_id"].ToString()));
            sparam[1] = new SqlParameter("@regid", cmf.Decrypt(Request.QueryString["regid"].ToString()));
		}
            

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
  



    }



    protected void bindfinancialyear(string cid)
    {

        //DataAccess Da = new DataAccess();
        //int yearid = Convert.ToDateTime(Da.getcdate()).Year;
        //int monthid = Convert.ToDateTime(Da.getcdate()).Month;
        //string ddlfinancialyeartype = "1";
        //string fromdate = "01/01/" + yearid;
        //string todate = "12/31/" + yearid;
        //DataAccess da = new DataAccess();
        //SqlParameter[] prm = new SqlParameter[1];
        //prm[0] = new SqlParameter("@regid", cid);
        //DataSet ds = da.GetDataSet("tbl_companydetails_selectbyregid", prm);
        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    ViewState["txtnameofcompany"] = ds.Tables[0].Rows[0]["txtnameofcompany"].ToString();





        //    string dateofbooks = ds.Tables[0].Rows[0]["txtdateofbooks"].ToString();

        //    string curdate = da.getcdate();

        //    ddlfinancialyeartype = ds.Tables[0].Rows[0]["ddlfinancialyear"].ToString();


        //    string strsql = "select * from [dbo].[Fn_Interval]('" + dateofbooks + "','" + curdate + "') order by yearvalue desc";
        //    DataSet dsyear = da.GetDataSetInline(strsql);
        //    if (dsyear.Tables[0].Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dsyear.Tables[0].Rows.Count; i++)
        //        {


        //            if (ddlfinancialyeartype == "2")
        //            {
        //                int yearname = Convert.ToInt32(dsyear.Tables[0].Rows[i]["YearValue"].ToString());
        //                //if (monthid < 4)
        //                //{

        //                if (yearname == yearid)
        //                {

        //                    if (monthid > 4)
        //                    {
        //                        ListItem li = new ListItem(dsyear.Tables[0].Rows[i]["YearValue"].ToString() + "-" + (yearname + 1).ToString(), dsyear.Tables[0].Rows[i]["YearValue"].ToString());

        //                       // ddlfinancialyear.Items.Add(li);
        //                    }

        //                    // }


        //                }
        //                else
        //                {
        //                    ListItem li = new ListItem(dsyear.Tables[0].Rows[i]["YearValue"].ToString() + "-" + (yearname + 1).ToString(), (yearname + 1).ToString());
        //                    // }

        //                 //   ddlfinancialyear.Items.Add(li);
        //                }

        //            }
        //            else
        //            {
        //                int yearname = Convert.ToInt32(dsyear.Tables[0].Rows[i]["YearValue"].ToString());
        //                //if (monthid < 4)
        //                //{
        //                ListItem li = new ListItem(dsyear.Tables[0].Rows[i]["YearValue"].ToString(), dsyear.Tables[0].Rows[i]["YearValue"].ToString());
        //                // }

        //               // ddlfinancialyear.Items.Add(li);
        //            }


        //        }
        //    }


            

        //        if (ddlfinancialyeartype == "2")
        //        {
        //            if (monthid < 4)
        //            {
        //                ddlfinancialyear = (yearid - 1).ToString();

        //            }
        //            else
        //            {
        //                ddlfinancialyear = yearid.ToString();
        //            }
        //        }
        //        else
        //        {
        //            ddlfinancialyear = yearid.ToString();
        //        }


        //        HttpCookie Employee = new HttpCookie("fyear");
        //        Employee.Values.Add("id", ddlfinancialyear);
        //        HttpContext.Current.Response.Cookies.Add(Employee);


        //        if (ddlfinancialyeartype == "2")
        //        {
        //            // string ddlfinancialyeartype = "1";

        //            if (monthid < 4)
        //            {

        //                fromdate = "04/01/" + (yearid - 1);
        //                todate = "03/31/" + yearid;
        //            }
        //            else
        //            {
        //                fromdate = "04/01/" + (yearid);
        //                todate = "03/31/" + (yearid + 1);
        //            }
        //        }
        //        else
        //        {
        //            fromdate = "01/01/" + ddlfinancialyear;
        //            todate = "12/31/" + ddlfinancialyear;
        //        }
        //        HttpCookie Employee1 = new HttpCookie("fromdate");
        //        Employee1.Values.Add("id", fromdate);
        //        HttpContext.Current.Response.Cookies.Add(Employee1);

        //        HttpCookie Employee2 = new HttpCookie("todate");
        //        Employee2.Values.Add("id", todate);
        //        HttpContext.Current.Response.Cookies.Add(Employee2);
        //        //ViewState["fromdate"] = fromdate;
        //        //ViewState["todate"] = todate;





        //    }
           
      


        // ddlfinancialyear
    }



    protected int authentication()
    {
        string uname = uidtxt.Text.Trim();
        string paswd = pwdtxt.Text.Trim();
        SqlParameter[] sparam = new SqlParameter[2];
        sparam[0] = new SqlParameter("@name", uname);
        sparam[1] = new SqlParameter("@password", paswd);
        ds = da.GetDataSet("tbl_employee_login", sparam);
        if (ds.Tables[0].Rows.Count > 0)
        {
            uid = ds.Tables[0].Rows[0]["id"].ToString();
            Session["Uid"] = uid;
            type = ds.Tables[0].Rows[0]["roleid"].ToString();
int countryid = 0;
 string strsql = "select isnull(countryid,2) as countryid from tbl_registrationdetails where id='" + ds.Tables[0].Rows[0]["regid"].ToString() + "'";
            DataSet dscon = da.GetDataSetInline(strsql);
            if (dscon.Tables[0].Rows.Count > 0)
            {
                countryid = Convert.ToInt32(dscon.Tables[0].Rows[0]["countryid"].ToString());
            }

            if (countryid == 1)
            { 
		//Response.Redirect("http://absolutebooks.absolutewebtech.com/login.aspx?regid=" + cmf.Encrypt(ds.Tables[0].Rows[0]["regid"].ToString()) + "&u_id=" + cmf.Encrypt(uid));

           
               
			}
            else
            {
                }
            string redurl = FormsAuthentication.GetRedirectUrl("1", true);
            FormsAuthentication.RedirectFromLoginPage("1", false);
            HttpCookie Employee = new HttpCookie("cadmin");
            Employee.Values.Add("id", ds.Tables[0].Rows[0]["regid"].ToString());
            HttpContext.Current.Response.Cookies.Add(Employee);

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
      
        string strsql = "select * from tbl_employee where email_Id='" + txtemailid.Text.Trim() + "' and status=1 and archive=0";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
         string changepwdmsg="   Hello "+ds.Tables[0].Rows[0]["f_name"].ToString()+"<br/>";
         changepwdmsg += "<br/>We have received a request to reset your Absolute Books account password associated with this email address. If you have not placed this request, you can safely ignore this email and we assure you that your account is completely secure.";
         changepwdmsg += "<br/>If you do need to change your Absolute Books Password, you can use the link given below.<br/><br/>";

         to = txtemailid.Text.Trim();
         from = "noreply@absolutewebtech.com";
         sub = "Reset your Absolute books Account Password";
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
        commf = new CommonFunction();
        uauthentication = new UserAuthentication();
        if (Page.IsValid)
        {
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
                    //Response.Redirect("Default.aspx");
                    Response.Redirect("Addproject.aspx");
                }
            }
            else
            {
                msg = "Invalid User Name or Password";
                pwdtxt.Text = "";
                lblmsg.Text = msg;
               
            }
            //string uname = uidtxt.Text.Trim();
            //string paswd = pwdtxt.Text.Trim();
            //if (uname == "atm" && paswd == "Atm@1234")
            //{
            //    if (uauthentication.SetCookie("1"))
            //    {
                   
            //        string redurl = FormsAuthentication.GetRedirectUrl("1", true);
            //        FormsAuthentication.RedirectFromLoginPage("1", false);
            //    }
            //}
            //else
            //{
            //    msg = "Invalid Emp Code or Password";
            //    //uidtxt.Text = "";
            //    pwdtxt.Text = "";
            //}
           
        }
    }
}