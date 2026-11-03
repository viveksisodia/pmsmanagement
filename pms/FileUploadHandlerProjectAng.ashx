<%@ WebHandler Language="C#" Class="FileUploadHandlerAng" %>

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

public class FileUploadHandlerAng : IHttpHandler {


    public void ProcessRequest (HttpContext context)
    {
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        if (context.Request.Files.Count > 0)
        {
            string pid = context.Request.Form["id"];
            string regid = HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString();
            string uid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();

            HttpFileCollection files = context.Request.Files;
            string fname = "";
            string Fileurl = "";
            string savedbFileurl = "";
            for (int i = 0; i < files.Count; i++)
            {

                HttpPostedFile file = files[i];
                string fext =Path.GetExtension(file.FileName).ToString();
                string ufilename = CleanFileName(file.FileName);
                //Fileurl = "/assets/images/" + regid + "/" + pid + "/" + file.FileName;
                Fileurl = "/assets/images/" + regid + "/" + pid + "/" + ufilename;
                //string UFileurl = Fileurl + "/" + file.FileName;
                string UFileurl = Fileurl;
                string path = context.Server.MapPath("/assets/images/" + regid + "/" + pid);
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
                    prm[1] = new SqlParameter("@regid", regid);
                    prm[2] = new SqlParameter("@uid", uid);
                    prm[3] = new SqlParameter("@cdate", da.getcdate());
                    prm[4] = new SqlParameter("@pid", context.Request.Form["id"]);
                    prm[5] = new SqlParameter("@fentryid",pid);
                    prm[6] = new SqlParameter("@fext", fext);
                    prm[7] = new SqlParameter("@ftype", "project");
                    prm[8] = new SqlParameter("@fileurl", UFileurl);
                    prm[9] = new SqlParameter("@filedes", file.FileName);
                    prm[10] = new SqlParameter("ReturnValue", SqlDbType.Int);
                    prm[10].Direction = ParameterDirection.ReturnValue;
                    int newid = da.executeDMLQuery("Pms_tbl_projectfiles_DML", prm);
                }
                catch { }

            }

            context.Response.ContentType = "text/plain";
            context.Response.Write(pid);
        }

    }
    public static string CleanFileName(string fileName)
    {
        string nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
        string ext = Path.GetExtension(fileName);

        // replace everything except letters, numbers, underscore, dash
        nameWithoutExt = System.Text.RegularExpressions.Regex.Replace(
            nameWithoutExt,
            @"[^a-zA-Z0-9_-]",
            "_"
        );

        // optional: remove multiple underscores
        nameWithoutExt = System.Text.RegularExpressions.Regex.Replace(
            nameWithoutExt,
            @"_+",
            "_"
        );

        return nameWithoutExt + ext;
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}