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

/// <summary>
/// Summary description for mailsend
/// </summary>
public class mailsend
{
    private MailMessage mail_obj;
    private SmtpClient smtp_obj;
    
	public mailsend()
	{
		//
		// TODO: Add constructor logic here
		//
	}


    public void SendMailtoUserwithatt(string to, string from, string sub, string body, string[] file_name)
    {

        //SmtpClient smtp_obj = new SmtpClient(ConfigurationManager.AppSettings["smtpAddress"]);
        mail_obj = new MailMessage(from, to, sub, body);
        mail_obj.IsBodyHtml = true;

        //for (int i = 0; i < file_name.Length; i++)
        //{
        //    string filename=file_name[0].ToString;

        if (file_name != null)
        {
            // HttpContext.Current.Response.Write(count);

            for (int i = 0; i < file_name.Length; i++)
            {
                //HttpContext.Current.Response.Write(file_name[i]);
                //HttpContext.Current.Response.End();
                Attachment att = new Attachment(file_name[i]);
                mail_obj.Attachments.Add(att);
            }
        }
        //}


        try
        {
            //if (smtp_obj != null)
            //{
            SmtpClient smtp_obj = new SmtpClient(ConfigurationManager.AppSettings["smtpAddress"]);
            smtp_obj.Port = Convert.ToInt32(ConfigurationManager.AppSettings["smtpPort"]);
            NetworkCredential x = new NetworkCredential();
            x.UserName = ConfigurationManager.AppSettings["smtpUser"];
            x.Password = ConfigurationManager.AppSettings["smtppwd"];
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

    public void SendMailtoUserwithatt(string to, string from, string sub, string body, string[] file_name,string bcc)
    {

        //SmtpClient smtp_obj = new SmtpClient(ConfigurationManager.AppSettings["smtpAddress"]);
        mail_obj = new MailMessage(from, to, sub, body);
        if (bcc != string.Empty)
        {
            mail_obj.Bcc.Add(bcc);
        }
        mail_obj.IsBodyHtml = true;

        //for (int i = 0; i < file_name.Length; i++)
        //{
        //    string filename=file_name[0].ToString;
        int count = file_name.Length;
        // HttpContext.Current.Response.Write(count);
        for (int i = 0; i < count; i++)
        {
            //HttpContext.Current.Response.Write(file_name[i]);
            //HttpContext.Current.Response.End();
            Attachment att = new Attachment(file_name[i]);
            mail_obj.Attachments.Add(att);
        }
        //}


        try
        {
            //if (smtp_obj != null)
            //{
            SmtpClient smtp_obj = new SmtpClient(ConfigurationManager.AppSettings["smtpAddress"]);
            smtp_obj.Port = Convert.ToInt32(ConfigurationManager.AppSettings["smtpPort"]);
            NetworkCredential x = new NetworkCredential();
            x.UserName = ConfigurationManager.AppSettings["smtpUser"];
            x.Password = ConfigurationManager.AppSettings["smtppwd"];
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
    public void SendMailtoUserwithatt(string to, string from, string sub, string body, string[] file_name,string bcc,string cc)
    {

        //SmtpClient smtp_obj = new SmtpClient(ConfigurationManager.AppSettings["smtpAddress"]);
        mail_obj = new MailMessage(from, to, sub, body);
        mail_obj.IsBodyHtml = true;
        if (bcc != string.Empty)
        {
            mail_obj.Bcc.Add(bcc);
        }
        if (cc != string.Empty)
        {
            mail_obj.CC.Add(cc);
        }

        //for (int i = 0; i < file_name.Length; i++)
        //{
        //    string filename=file_name[0].ToString;
        int count = file_name.Length;
        // HttpContext.Current.Response.Write(count);
        for (int i = 0; i < count; i++)
        {
            //HttpContext.Current.Response.Write(file_name[i]);
            //HttpContext.Current.Response.End();
            Attachment att = new Attachment(file_name[i]);
            mail_obj.Attachments.Add(att);
        }
        //}


        try
        {
            //if (smtp_obj != null)
            //{
            SmtpClient smtp_obj = new SmtpClient(ConfigurationManager.AppSettings["smtpAddress"]);
            smtp_obj.Port = Convert.ToInt32(ConfigurationManager.AppSettings["smtpPort"]);
            NetworkCredential x = new NetworkCredential();
            x.UserName = ConfigurationManager.AppSettings["smtpUser"];
            x.Password = ConfigurationManager.AppSettings["smtppwd"];
            smtp_obj.UseDefaultCredentials = false;
            smtp_obj.Credentials = x;
            smtp_obj.Send(mail_obj);

        }
        catch (Exception e)
        {
            HttpContext.Current.Response.Write(e.Message);
        }

    }


    public void SendMailtoUserwithattcc(string to, string from, string sub, string body, string[] file_name, string cc)
    {

        //SmtpClient smtp_obj = new SmtpClient(ConfigurationManager.AppSettings["smtpAddress"]);
        mail_obj = new MailMessage(from, to, sub, body);
        mail_obj.IsBodyHtml = true;
        mail_obj.CC.Add(cc);

        //for (int i = 0; i < file_name.Length; i++)
        //{
        //    string filename=file_name[0].ToString;
        int count = file_name.Length;
        // HttpContext.Current.Response.Write(count);
        for (int i = 0; i < count; i++)
        {
            //HttpContext.Current.Response.Write(file_name[i]);
            //HttpContext.Current.Response.End();
            Attachment att = new Attachment(file_name[i]);
            mail_obj.Attachments.Add(att);
        }
        //}


        try
        {
            //if (smtp_obj != null)
            //{
            SmtpClient smtp_obj = new SmtpClient(ConfigurationManager.AppSettings["smtpAddress"]);
            smtp_obj.Port = Convert.ToInt32(ConfigurationManager.AppSettings["smtpPort"]);
            NetworkCredential x = new NetworkCredential();
            x.UserName = ConfigurationManager.AppSettings["smtpUser"];
            x.Password = ConfigurationManager.AppSettings["smtppwd"];
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


}
