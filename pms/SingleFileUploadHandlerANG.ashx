<%@ WebHandler Language="C#" Class="SingleFileUploadHandler" %>

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

public class SingleFileUploadHandler : IHttpHandler {

    public void ProcessRequest (HttpContext context)
    {
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        if (context.Request.Files.Count > 0)
        {

            HttpFileCollection files = context.Request.Files;
            string fname = "";
            string Fileurl = "";
            if(context.Request.Form["ftype"]!=null)
            {

                if(context.Request.Form["ftype"].ToString()=="project")
                    Fileurl = "/assets/images/" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "/" + HttpContext.Current.Request.QueryString["id"];
                else if(context.Request.Form["ftype"].ToString()=="milestone")
                    Fileurl = "/pms/MileImages/"+HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString()+"/"+context.Request.Form["id"];
                else if(context.Request.Form["ftype"].ToString()=="task")
                    Fileurl = "/pms/TaskImages/"+HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString()+"/"+context.Request.Form["id"];
                else if(context.Request.Form["ftype"].ToString()=="subtask")
                    Fileurl = "/pms/SubtaskImages/"+HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString()+"/"+context.Request.Form["id"];

            }

            string savedbFileurl = "";
            string strexceptions = "";
            List<projectfiles> uploadedFiles = new List<projectfiles>();
            for (int i = 0; i < files.Count; i++)
            {
                try
                {
                    
                    string fext = "";
                    HttpPostedFile file = files[i];
                    fext=Path.GetExtension(file.FileName).ToString();
                    string ffname = CleanFileName(file.FileName);
                    string UFileurl = Fileurl + "/" + ffname;
                    if(fext==".docx")
                    {
                        SaveLogText("UFileurl-" +UFileurl);
                    }
                    //fname = context.Server.MapPath("/employee/emppic/" + HttpContext.Current.Request.QueryString["id"]+"_"+DateTime.Now.ToString("yyyyMMdd_hhmmss")+"_"+file.FileName);
                    string path = context.Server.MapPath(Fileurl);
                    if (!Directory.Exists(path))
                    {
                        //Create the directory on the given path  
                        Directory.CreateDirectory(path);
                    }
                    fname = context.Server.MapPath(UFileurl);
                    if (File.Exists(fname))
                    {
                        try
                        {
                            File.SetAttributes(fname, FileAttributes.Normal); // remove readonly
                            File.Delete(fname);
                        }
                        catch (Exception ex)
                        {
                            strexceptions += ex.Message+",";
                        }
                    }
                    file.SaveAs(fname);

                    SqlParameter[] prm = new SqlParameter[11];
                    prm[0] = new SqlParameter("@action", "insert");
                    prm[1] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());
                    prm[2] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                    prm[3] = new SqlParameter("@cdate",da.getcdate());
                    prm[4] = new SqlParameter("@pid", context.Request.Form["pid"]);
                    prm[5] = new SqlParameter("@fentryid",context.Request.Form["id"]);
                    prm[6] = new SqlParameter("@fext",fext);
                    prm[7] = new SqlParameter("@ftype", context.Request.Form["ftype"]);
                    prm[8] = new SqlParameter("@fileurl",UFileurl);
                    prm[9] = new SqlParameter("@filedes",file.FileName);
                    prm[10] = new SqlParameter("ReturnValue", SqlDbType.Int);
                    prm[10].Direction = ParameterDirection.ReturnValue;
                    int newid=da.executeDMLQuery("Pms_tbl_projectfiles_DML", prm);
                    uploadedFiles.Add(new projectfiles{
                        id = newid.ToString(),
                        Uploadby=getemployee(HttpContext.Current.Request.Cookies["user"]["user_id"].ToString()),
                        pid =context.Request.Form["pid"],
                        fentryid =context.Request.Form["id"],
                        ftype =context.Request.Form["ftype"],
                        fileurl = UFileurl,
                        filedes = file.FileName,
                        fext = fext,
                        filepath = Fileurl
                    });
                }
                catch(Exception ex)
                {
                    strexceptions += ex.Message+",";
                }
            }


            context.Response.ContentType = "application/json";
            if (strexceptions != string.Empty)
            {
                context.Response.Write(strexceptions);
            }
            else
            {
                //context.Response.Write(context.Request.Form["id"].ToString());
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(uploadedFiles));
            }
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
    public static string getemployee(string ids)
    {
        string retname = "";
        if (string.IsNullOrEmpty(ids) || ids == "0")
            return retname;

        string str2 = "select id, DisplayName, fileupload1,UPPER(SUBSTRING(DisplayName,1,1)) as splinameone " +
                      "from tbl_employee " +
                      "where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' " +
                      "and id in(" + ids + ")";

        DataAccess danew = new DataAccess();
        try
        {
            DataSet drmile = danew.GetDataSetInline(str2);
            retname = drmile.Tables[0].Rows[0]["DisplayName"].ToString();
        }
        catch { }
        return retname;

    }
    public static string SaveLogText(string Message)
    {
        string retval = "";
        CommonFunction cmf1 = new CommonFunction();
        DataAccess da1 = new DataAccess();
        DataSet dschk = new DataSet();

        SqlParameter[] prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@message", Message);
        prm[1] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[1].Direction = ParameterDirection.ReturnValue;
        retval = Convert.ToString(da1.executeDMLQuery("crm_entryLogTable", prm));
        return retval;
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
    public class projectfiles
    {
        public string id { get; set; }
        public string pid { get; set; }
        public string Uploadby { get; set; }
        public string fentryid { get; set; }
        public string ftype { get; set; }
        public string fileurl { get; set; }
        public string filedes { get; set; }
        public string fext { get; set; }
        public string filepath { get; set; }


    }

}