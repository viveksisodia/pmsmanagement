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
                string fext =Path.GetExtension(file.FileName).ToString();
                Fileurl = "/assets/images/" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "/" + HttpContext.Current.Request.QueryString["id"] + "/" + file.FileName;
                string UFileurl = Fileurl + "/" + file.FileName;
                //fname = context.Server.MapPath("/employee/emppic/" + HttpContext.Current.Request.QueryString["id"]+"_"+DateTime.Now.ToString("yyyyMMdd_hhmmss")+"_"+file.FileName);
                string path = context.Server.MapPath("/assets/images/" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "/" + HttpContext.Current.Request.QueryString["id"]);
                if (!Directory.Exists(path))
                {
                    //Create the directory on the given path  
                    Directory.CreateDirectory(path);
                }
                fname = context.Server.MapPath(Fileurl);
                file.SaveAs(fname);
                try
                {
                    SqlParameter[] prm = new SqlParameter[11];
                    prm[0] = new SqlParameter("@action", "insert");
                    prm[1] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                    prm[2] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                    prm[3] = new SqlParameter("@cdate", da.getcdate());
                    prm[4] = new SqlParameter("@pid", context.Request.Form["id"]);
                    prm[5] = new SqlParameter("@fentryid", context.Request.Form["id"]);
                    prm[6] = new SqlParameter("@fext", fext);
                    prm[7] = new SqlParameter("@ftype", "project");
                    prm[8] = new SqlParameter("@fileurl", UFileurl);
                    prm[9] = new SqlParameter("@filedes", file.FileName);
                    prm[10] = new SqlParameter("ReturnValue", SqlDbType.Int);
                    prm[10].Direction = ParameterDirection.ReturnValue;
                    int newid = da.executeDMLQuery("Pms_tbl_projectfiles_DML", prm);
                }
                catch { }
                if (savedbFileurl!="")
                {
                    savedbFileurl+="^"+HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "/" + HttpContext.Current.Request.QueryString["id"] + "/" + file.FileName;
                }
                else
                {
                    savedbFileurl = HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "/" + HttpContext.Current.Request.QueryString["id"] + "/" + file.FileName;
                }
            }
            //SqlParameter[] prmitem = new SqlParameter[5];
            //prmitem[0] = new SqlParameter("@estmid", HttpContext.Current.Request.QueryString["id"]);
            //prmitem[1] = new SqlParameter("@fileurl", Fileurl);
            //prmitem[2] = new SqlParameter("@cdate", da.getcdatetime());
            //prmitem[3] = new SqlParameter("@RegId", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            //prmitem[4] = new SqlParameter("ReturnValue", SqlDbType.Int);
            //prmitem[4].Direction = ParameterDirection.ReturnValue;
            //int retid = da.executeDMLQuery("tbl_Leaddetailsattachedfilesinsert", prmitem);
            string strsql2 = "update Pms_tbl_AddProject set Uploaddrawing ='" + savedbFileurl + "' where status=1 and archive=0 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + HttpContext.Current.Request.QueryString["id"] + "'";
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