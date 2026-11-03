using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Web;
/// <summary>
/// Summary description for sendsms
/// </summary>
public class sendsms
{
	public sendsms()
	{
		//
		// TODO: Add constructor logic here
		//
	}



    public string sendsmsquick(string senderid,string destination,string msg)
    {

        string strresult = "";
        WebRequest WebRequest = default(WebRequest);
        //object for WebRequest
        WebResponse WebResonse = default(WebResponse);
        //object for WebResponse
        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' ''''''''
        // DEFINE PARAMETERS USED IN URL
        ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' ''''''''
        //To what server you need to connect to for submission
        //i.e. Dim Server As String = "smpp1.spheredge.com"
        string Server = "sms6.routesms.com";
        //Port that is to be used like 8080 or 8000
        string Port = "8080";
        //Username that is to be used for submission
        //i.e. Dim UserName As String = "tester"
        string UserName = "starmediatra";
        // password that is to be used along with username
        //i.e. Dim Password As String = "password"
        string Password = "star123";
        //What type of the message that is to be sent.
        //0:means plain text
        //1:means flash
        //2:means Unicode (Message content should be in Hex)
        //6:means Unicode Flash(Message content should be in Hex)
        int type = 0;
        //Message content that is to be transmitted
        string Message = msg;
        //Url Encode message
        Message = HttpUtility.UrlEncode(Message);
        //if ((type == 2) | (type == 6)) {
        //    Message = ConvertToUnicode(Message);
        //}
        //Require DLR or not
        //0:means DLR is not Required
        //1:means DLR is Required
        int DLR = 1;
        //Sender Id to be used for submitting the message
        //i.e. Dim SenderName As String = "test"
        string Source = senderid;
        //Destinations to which message is to be sent For submitting more        than(one)
        //destination at once destinations should be comma separated Like
        //91999000123,91999000124
        string Destination = destination;
        ///''''CODE COMPLETE TO DEFINE PARAMETER''''''''''''''''
        string WebResponseString = "";
        string URL = "http://" + Server + ":" + Port + "/bulksms/bulksms?username=" + UserName + "&password=" + Password + "&type=" + type + "&dlr=" + DLR + "&destination=" + Destination + "&source=" + Source + "&message=" + Message + "";
        WebRequest = HttpWebRequest.Create(URL);

        // Response.Write(URL);
        // Response.End();
        //Hit URL Link
        WebRequest.Timeout = 25000;
        try {
        WebResonse = WebRequest.GetResponse();
        //Get Response
        StreamReader reader = new StreamReader(WebResonse.GetResponseStream());
        //Read Response and store in variable
        WebResponseString = reader.ReadToEnd();
        WebResonse.Close();
        strresult=WebResponseString;
        //Display Response.
        } catch (Exception ex) {
           WebResponseString = "Request Timeout";
           //If any exception            occur.
            strresult=WebResponseString;
        }
        return strresult;
    }


    public string sendsmsquicksawali(string senderid, string destination, string msg)
    {

        string strresult = "";


        if (HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() == "144")
        {


            WebRequest WebRequest = default(WebRequest);
            //object for WebRequest
            WebResponse WebResonse = default(WebResponse);
            //object for WebResponse
            ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' ''''''''
            // DEFINE PARAMETERS USED IN URL
            ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' ''''''''
            //To what server you need to connect to for submission
            //i.e. Dim Server As String = "smpp1.spheredge.com"
            string Server = "103.16.101.52";
            //Port that is to be used like 8080 or 8000
            string Port = "8080";
            //Username that is to be used for submission
            //i.e. Dim UserName As String = "tester"
            string UserName = "aweb-sawali";
            // password that is to be used along with username
            //i.e. Dim Password As String = "password"
            string Password = "sawali";
            //What type of the message that is to be sent.
            //0:means plain text
            //1:means flash
            //2:means Unicode (Message content should be in Hex)
            //6:means Unicode Flash(Message content should be in Hex)
            int type = 0;
            //Message content that is to be transmitted
            string Message = msg;
            //Url Encode message
            Message = HttpUtility.UrlEncode(Message);
            //if ((type == 2) | (type == 6)) {
            //    Message = ConvertToUnicode(Message);
            //}
            //Require DLR or not
            //0:means DLR is not Required
            //1:means DLR is Required
            int DLR = 1;
            //Sender Id to be used for submitting the message
            //i.e. Dim SenderName As String = "test"
            string Source = "sawali";
            //Destinations to which message is to be sent For submitting more        than(one)
            //destination at once destinations should be comma separated Like
            //91999000123,91999000124
            string Destination = destination;

            ///''''CODE COMPLETE TO DEFINE PARAMETER''''''''''''''''

            string WebResponseString = "";
            string URL = "http://" + Server + ":" + Port + "/sendsms/bulksms?username=" + UserName + "&password=" + Password + "&type=" + type + "&dlr=" + DLR + "&destination=" + Destination + "&source=" + Source + "&message=" + Message + "";



            WebRequest = HttpWebRequest.Create(URL);



            // http://103.16.101.52:8080/sendsms/bulksms?username=aweb-srvpathlab&password=srvlab12&type=0&dlr=1&destination=9810502506&source=SRVLAB&message=hello


            //HttpContext.Current.Response.Write(URL);
            //HttpContext.Current.Response.End();

            //Hit URL Link
            WebRequest.Timeout = 25000;
            try
            {
                WebResonse = WebRequest.GetResponse();
                //Get Response
                StreamReader reader = new StreamReader(WebResonse.GetResponseStream());
                //Read Response and store in variable
                WebResponseString = reader.ReadToEnd();
                WebResonse.Close();
                strresult = WebResponseString;
                //Display Response.
            }
            catch (Exception ex)
            {
                WebResponseString = "Request Timeout";
                //If any exception            occur.
                strresult = WebResponseString;
            }

        }
        return strresult;
    }



  



}