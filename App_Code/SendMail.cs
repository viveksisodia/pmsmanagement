using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net.Mail;
using System.Text;
using System.Collections;
using System.Threading;
using System.Text.RegularExpressions;
using System.Net;
using CommonClass;
/// <summary>
/// Summary description for SendMail
/// </summary>

public class SendMail
{
    private MailMessage mail_obj;
    private SmtpClient smtp_obj;
    //public void  SendMail()
    //{
    //    //
    //    // TODO: Add constructor logic here
    //    //
    //    SmtpClient smtp_obj = new SmtpClient(smtpAddress);
    //}

    /*****************************check valid email address by vivek sisodia*******************/

    public string checkvalidemailid(string EMail)
    {
        SmtpClient smtp_obj = new SmtpClient(ConfigurationManager.AppSettings["smtpAddress"].ToString());
        if (EMail != "")
        {
            Regex n = new Regex("(?<user>[^@]+)@(?<host>.+)");
            Match v = n.Match(EMail);
            if (!v.Success || EMail.Length != v.Length)
            {
                // this.errorProvider1.SetError(EMail, "invalid e-mail format");
                return "false";
            }
            else
            {
                return "true";
            }
        }
        else
        {
            return "false";
        }

    }

    /**************************** function for sending a mail without smtp by vivek sisodia *****************/


    public void sendmail(string to, string from, string sub, string body)
    {
        string smtpAddress = ConfigurationManager.AppSettings["smtpAddress"].ToString();
        string smtpPort = ConfigurationManager.AppSettings["smtpPort"].ToString();
        string smtpUser = ConfigurationManager.AppSettings["smtpUser"].ToString();
        string smtppwd = ConfigurationManager.AppSettings["smtppwd"].ToString();
        string issendmailfromusername = "0";
        string sendmailuserid = "";
        try
        {
            DataAccess da = new DataAccess();
            string strsql = "select * from tbl_smtpdetails where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and status=1 and archive=0";

            DataSet ds = da.GetDataSetInline(strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                smtpAddress = ds.Tables[0].Rows[0]["smtpAddress"].ToString();
                smtpPort = ds.Tables[0].Rows[0]["smtpPort"].ToString();
                smtpUser = ds.Tables[0].Rows[0]["smtpUser"].ToString();
                smtppwd = ds.Tables[0].Rows[0]["smtppwd"].ToString();
                issendmailfromusername = ds.Tables[0].Rows[0]["issendmailfromusername"].ToString();
                sendmailuserid = ds.Tables[0].Rows[0]["sendmailuserid"].ToString();
            }
        }
        catch
        {

        }
        if (sendmailuserid != string.Empty)
        {
            from = sendmailuserid;
        }
        SmtpClient smtp_obj = new SmtpClient(smtpAddress);
        System.Web.Mail.MailMessage message = new System.Web.Mail.MailMessage();
        //System.Web.Mail.MailMessage message = new System.Web.Mail.MailMessage();
        message.From = from;
        message.To = to;
        message.Subject = sub;
        message.BodyFormat = System.Web.Mail.MailFormat.Html;

        message.Body = body;
        try
        {
            System.Web.Mail.SmtpMail.Send(message);
            //System.Web.Mail.SmtpMail.Send(message);
        }
        catch (Exception e)
        {
            HttpContext.Current.Response.Write(e.Message);
        }

    }




    /***************************** function for sending a mail********************/
    public void SendMailtoUser(string to, string from, string sub, string body)
    {
        string smtpAddress = ConfigurationManager.AppSettings["smtpAddress"].ToString();
        string smtpPort = ConfigurationManager.AppSettings["smtpPort"].ToString();
        string smtpUser = ConfigurationManager.AppSettings["smtpUser"].ToString();
        string smtppwd = ConfigurationManager.AppSettings["smtppwd"].ToString();
        string issendmailfromusername = "0";
        string sendmailuserid = "";
        try
        {
            DataAccess da = new DataAccess();
            string strsql = "select * from tbl_smtpdetails where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and status=1 and archive=0";

            DataSet ds = da.GetDataSetInline(strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                smtpAddress = ds.Tables[0].Rows[0]["smtpAddress"].ToString();
                smtpPort = ds.Tables[0].Rows[0]["smtpPort"].ToString();
                smtpUser = ds.Tables[0].Rows[0]["smtpUser"].ToString();
                smtppwd = ds.Tables[0].Rows[0]["smtppwd"].ToString();
                issendmailfromusername = ds.Tables[0].Rows[0]["issendmailfromusername"].ToString();
                sendmailuserid = ds.Tables[0].Rows[0]["sendmailuserid"].ToString();
            }
        }
        catch
        {

        }
        if (sendmailuserid != string.Empty)
        {
            from = sendmailuserid;
        }

        // SmtpClient smtp_obj = new SmtpClient(smtpAddress);
        mail_obj = new MailMessage(from, to, sub, body);
        mail_obj.IsBodyHtml = true;

        try
        {
            //if (smtp_obj != null)
            //{
            SmtpClient smtp_obj = new SmtpClient(smtpAddress);
            smtp_obj.Port = Convert.ToInt32(smtpPort);
            NetworkCredential x = new NetworkCredential();
            x.UserName = smtpUser;
            x.Password = smtppwd;
            smtp_obj.UseDefaultCredentials = false;
            smtp_obj.Credentials = x;
            smtp_obj.Send(mail_obj);
            //}


        }
        catch (Exception e)
        {
            HttpContext.Current.Response.Write(e.Message);
        }

    }


    public void SendMailtoUser(string to, string from, string sub, string cc, string body)
    {
        string smtpAddress = ConfigurationManager.AppSettings["smtpAddress"].ToString();
        string smtpPort = ConfigurationManager.AppSettings["smtpPort"].ToString();
        string smtpUser = ConfigurationManager.AppSettings["smtpUser"].ToString();
        string smtppwd = ConfigurationManager.AppSettings["smtppwd"].ToString();
        string issendmailfromusername = "0";
        string sendmailuserid = "";
        try
        {
            DataAccess da = new DataAccess();
            string strsql = "select * from tbl_smtpdetails where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and status=1 and archive=0";

            DataSet ds = da.GetDataSetInline(strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                smtpAddress = ds.Tables[0].Rows[0]["smtpAddress"].ToString();
                smtpPort = ds.Tables[0].Rows[0]["smtpPort"].ToString();
                smtpUser = ds.Tables[0].Rows[0]["smtpUser"].ToString();
                smtppwd = ds.Tables[0].Rows[0]["smtppwd"].ToString();
                issendmailfromusername = ds.Tables[0].Rows[0]["issendmailfromusername"].ToString();
                sendmailuserid = ds.Tables[0].Rows[0]["sendmailuserid"].ToString();
            }
        }
        catch
        {

        }
        if (sendmailuserid != string.Empty)
        {
            from = sendmailuserid;
        }
        SmtpClient smtp_obj = new SmtpClient(smtpAddress);
        mail_obj = new MailMessage(from, to, sub, body);
        mail_obj.CC.Add(cc);
        try
        {
            smtp_obj.Port = Convert.ToInt32(smtpPort);
            NetworkCredential x = new NetworkCredential();
            x.UserName = smtpUser;
            x.Password = smtppwd;
            smtp_obj.UseDefaultCredentials = false;
            smtp_obj.Credentials = x;
            smtp_obj.Send(mail_obj);
        }
        catch (Exception e)
        {
            HttpContext.Current.Response.Write(e.Message);
        }

    }

    public void SendMailtoUser(string to, string from, string cc, string bcc, string sub, string body)
    {
        string smtpAddress = ConfigurationManager.AppSettings["smtpAddress"].ToString();
        string smtpPort = ConfigurationManager.AppSettings["smtpPort"].ToString();
        string smtpUser = ConfigurationManager.AppSettings["smtpUser"].ToString();
        string smtppwd = ConfigurationManager.AppSettings["smtppwd"].ToString();
        string issendmailfromusername = "0";
        string sendmailuserid = "";
        try
        {
            DataAccess da = new DataAccess();
            string strsql = "select * from tbl_smtpdetails where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and status=1 and archive=0";

            DataSet ds = da.GetDataSetInline(strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                smtpAddress = ds.Tables[0].Rows[0]["smtpAddress"].ToString();
                smtpPort = ds.Tables[0].Rows[0]["smtpPort"].ToString();
                smtpUser = ds.Tables[0].Rows[0]["smtpUser"].ToString();
                smtppwd = ds.Tables[0].Rows[0]["smtppwd"].ToString();
                issendmailfromusername = ds.Tables[0].Rows[0]["issendmailfromusername"].ToString();
                sendmailuserid = ds.Tables[0].Rows[0]["sendmailuserid"].ToString();
            }
        }
        catch
        {

        }
        if (sendmailuserid != string.Empty)
        {
            from = sendmailuserid;
        }


        SmtpClient smtp_obj = new SmtpClient(smtpAddress);
        mail_obj = new MailMessage(from, to, sub, body);
        mail_obj.CC.Add(cc);
        mail_obj.Bcc.Add(bcc);
        try
        {
            smtp_obj.Port = Convert.ToInt32(smtpPort);
            NetworkCredential x = new NetworkCredential();
            x.UserName = smtpUser;
            x.Password = smtppwd;
            smtp_obj.UseDefaultCredentials = false;
            smtp_obj.Credentials = x;
            smtp_obj.Send(mail_obj);
        }
        catch (Exception e)
        {
            HttpContext.Current.Response.Write(e.Message);
        }

    }


    public void SendMailtoUser(string to, string from, string cc, string bcc, string sub, string body, string[] files)
    {


        string smtpAddress = ConfigurationManager.AppSettings["smtpAddress"].ToString();
        string smtpPort = ConfigurationManager.AppSettings["smtpPort"].ToString();
        string smtpUser = ConfigurationManager.AppSettings["smtpUser"].ToString();
        string smtppwd = ConfigurationManager.AppSettings["smtppwd"].ToString();
        string issendmailfromusername = "0";
        string sendmailuserid = "";
        try
        {
            DataAccess da = new DataAccess();
            string strsql = "select * from tbl_smtpdetails where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and status=1 and archive=0";

            DataSet ds = da.GetDataSetInline(strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                smtpAddress = ds.Tables[0].Rows[0]["smtpAddress"].ToString();
                smtpPort = ds.Tables[0].Rows[0]["smtpPort"].ToString();
                smtpUser = ds.Tables[0].Rows[0]["smtpUser"].ToString();
                smtppwd = ds.Tables[0].Rows[0]["smtppwd"].ToString();
                issendmailfromusername = ds.Tables[0].Rows[0]["issendmailfromusername"].ToString();
                sendmailuserid = ds.Tables[0].Rows[0]["sendmailuserid"].ToString();
            }
        }
        catch
        {

        }
        if (sendmailuserid != string.Empty)
        {
            from = sendmailuserid;
        }
        SmtpClient smtp_obj = new SmtpClient(smtpAddress);

        mail_obj = new MailMessage(from, to, sub, body);
        mail_obj.IsBodyHtml = true;
        if (cc != string.Empty)
        {
            mail_obj.CC.Add(cc);
        }
        if (bcc != string.Empty)
        {
            mail_obj.Bcc.Add(bcc);
        }

        try
        {

            for (int i = 0; i < files.Length; i++)
            {
                string filename = files[i].ToString();

                Attachment att = new Attachment(filename);
                mail_obj.Attachments.Add(att);
            }
        }
        catch (Exception ex)
        {
        }

        try
        {
            smtp_obj.Port = Convert.ToInt32(smtpPort);
            NetworkCredential x = new NetworkCredential();
            x.UserName = smtpUser;
            x.Password = smtppwd;
            smtp_obj.UseDefaultCredentials = false;
            smtp_obj.Credentials = x;
            smtp_obj.Send(mail_obj);
        }
        catch (Exception e)
        {
            HttpContext.Current.Response.Write(e.Message);
        }

    }

    private MailAddressCollection Address(string name)
    {
        SmtpClient smtp_obj = new SmtpClient(ConfigurationManager.AppSettings["smtpAddress"].ToString());
        MailAddressCollection add_collection = new MailAddressCollection();
        char[] ch = { ',' };
        string[] arr = name.Split(ch);
        foreach (string s_name in arr)
        {
            add_collection.Add(s_name);
        }
        return add_collection;
    }



    public void SendMailtoUserwithatt(string to, string from, string sub, string body, string file_name)
    {
        string smtpAddress = ConfigurationManager.AppSettings["smtpAddress"].ToString();
        string smtpPort = ConfigurationManager.AppSettings["smtpPort"].ToString();
        string smtpUser = ConfigurationManager.AppSettings["smtpUser"].ToString();
        string smtppwd = ConfigurationManager.AppSettings["smtppwd"].ToString();
        string issendmailfromusername = "0";
        string sendmailuserid = "";
        try
        {
            DataAccess da = new DataAccess();
            string strsql = "select * from tbl_smtpdetails where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and status=1 and archive=0";

            DataSet ds = da.GetDataSetInline(strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                smtpAddress = ds.Tables[0].Rows[0]["smtpAddress"].ToString();
                smtpPort = ds.Tables[0].Rows[0]["smtpPort"].ToString();
                smtpUser = ds.Tables[0].Rows[0]["smtpUser"].ToString();
                smtppwd = ds.Tables[0].Rows[0]["smtppwd"].ToString();
                issendmailfromusername = ds.Tables[0].Rows[0]["issendmailfromusername"].ToString();
                sendmailuserid = ds.Tables[0].Rows[0]["sendmailuserid"].ToString();
            }
        }
        catch
        {

        }
        if (sendmailuserid != string.Empty)
        {
            from = sendmailuserid;
        }
        //SmtpClient smtp_obj = new SmtpClient(smtpAddress);
        mail_obj = new MailMessage(from, to, sub, body);
        mail_obj.IsBodyHtml = true;

        //for (int i = 0; i < file_name.Length; i++)
        //{
        //    string filename=file_name[0].ToString;

        Attachment att = new Attachment(file_name);
        mail_obj.Attachments.Add(att);
        //}


        try
        {
            //if (smtp_obj != null)
            //{
            SmtpClient smtp_obj = new SmtpClient(smtpAddress);
            smtp_obj.Port = Convert.ToInt32(smtpPort);
            NetworkCredential x = new NetworkCredential();
            x.UserName = smtpUser;
            x.Password = smtppwd;
            smtp_obj.UseDefaultCredentials = false;
            smtp_obj.Credentials = x;
            smtp_obj.Send(mail_obj);
            //}


        }
        catch (Exception e)
        {
            HttpContext.Current.Response.Write(e.Message);
        }

    }
    /******************************End send mail********************************/

}
