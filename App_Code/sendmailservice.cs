using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using CommonClass;
/// <summary>
/// Summary description for sendmailservice
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class sendmailservice : System.Web.Services.WebService {

    public sendmailservice () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string sendmailtouser(string to,string from,string sub,string msg) {

        SendMail sm = new SendMail();
        try
        {
            sm.SendMailtoUser(to, from, sub, msg);
            return "success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }



    }
    
}
