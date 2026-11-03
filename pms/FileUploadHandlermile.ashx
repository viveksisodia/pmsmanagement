<%@ WebHandler Language="C#" Class="FileUploadHandler" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonClass;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using CustomDropDown;

public class FileUploadHandler : IHttpHandler {

    public void ProcessRequest (HttpContext context)
    {
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        if (context.Request.Files.Count > 0)
        {

            HttpFileCollection files = context.Request.Files;
            string fname = "";
            string Fileurl = "";
            string savedbFileurl = "";
            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFile file = files[i];
                Fileurl = "/pms/MileImages/"+HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString()+"/"+HttpContext.Current.Request.QueryString["id"]+"/"+file.FileName;

                //fname = context.Server.MapPath("/employee/emppic/" + HttpContext.Current.Request.QueryString["id"]+"_"+DateTime.Now.ToString("yyyyMMdd_hhmmss")+"_"+file.FileName);
                string path = context.Server.MapPath("/pms/MileImages/"+HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString()+"/"+HttpContext.Current.Request.QueryString["id"]);  
                if (!Directory.Exists(path))
                {
                    //Create the directory on the given path  
                    Directory.CreateDirectory(path);
                }
                fname = context.Server.MapPath(Fileurl);
                file.SaveAs(fname);
                savedbFileurl=HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString()+"/"+HttpContext.Current.Request.QueryString["id"]+"/"+file.FileName;
            }
            
            string strsql2 = "update Pms_tbl_AddMilestone set Uploaddrawing ='" + savedbFileurl + "' where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + HttpContext.Current.Request.QueryString["id"] + "'";
            da.executeDMLQueryinline(strsql2);
            context.Response.ContentType = "text/plain";
            context.Response.Write(HttpContext.Current.Request.QueryString["id"].ToString()+"_"+Fileurl);
        }

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}