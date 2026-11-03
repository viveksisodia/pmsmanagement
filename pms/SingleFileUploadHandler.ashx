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
            if(HttpContext.Current.Request.QueryString["ftype"]!=null)
            {
                     SaveLogText("ftypeA-"+HttpContext.Current.Request.QueryString["ftype"]);
                if(HttpContext.Current.Request.QueryString["ftype"].ToString()=="milestone")
                    Fileurl = "/pms/MileImages/"+HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString()+"/"+HttpContext.Current.Request.QueryString["id"];
                else if(HttpContext.Current.Request.QueryString["ftype"].ToString()=="task")
                    Fileurl = "/pms/TaskImages/"+HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString()+"/"+HttpContext.Current.Request.QueryString["id"];
                else if(HttpContext.Current.Request.QueryString["ftype"].ToString()=="subtask")
                    Fileurl = "/pms/SubtaskImages/"+HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString()+"/"+HttpContext.Current.Request.QueryString["id"];

            }

            string savedbFileurl = "";
            string strexceptions = "";
            for (int i = 0; i < files.Count; i++)
            {
                try
                {
                    string fext = "";
                    HttpPostedFile file = files[i];
                    fext=Path.GetExtension(file.FileName).ToString();
                    
                    string UFileurl = Fileurl + "/" + file.FileName;
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
                    prm[4] = new SqlParameter("@pid", HttpContext.Current.Request.QueryString["pid"]);
                    prm[5] = new SqlParameter("@fentryid",HttpContext.Current.Request.QueryString["id"]);
                    prm[6] = new SqlParameter("@fext",fext);
                    prm[7] = new SqlParameter("@ftype", HttpContext.Current.Request.QueryString["ftype"]);
                    prm[8] = new SqlParameter("@fileurl",UFileurl);
                    prm[9] = new SqlParameter("@filedes",file.FileName);
                    prm[10] = new SqlParameter("ReturnValue", SqlDbType.Int);
                    prm[10].Direction = ParameterDirection.ReturnValue;
                    da.executeDMLQuery("Pms_tbl_projectfiles_DML", prm);
                }
                catch(Exception ex)
                {
                    strexceptions += ex.Message+",";
                }
            }


            context.Response.ContentType = "text/plain";
            if (strexceptions != string.Empty)
            {
                context.Response.Write(strexceptions);
            }
            else
            {
                context.Response.Write(HttpContext.Current.Request.QueryString["id"].ToString());
            }
        }

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

}