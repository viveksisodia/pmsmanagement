<%@ WebHandler Language="C#" Class="itemimageuploadHandler" %>

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

public class itemimageuploadHandler : IHttpHandler { 
 

    public void ProcessRequest(HttpContext context)
    {
        try
        {
            if (context.Request.Files.Count == 0)
            {
                context.Response.Write("No File");
                return ;
            }

            HttpPostedFile file = context.Request.Files[0];

            string extension = Path.GetExtension(file.FileName);

            string fileName = Guid.NewGuid().ToString() + extension;

            string folder = context.Server.MapPath("~/products/itemimg/"+HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString()+"/");

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string path = Path.Combine(folder, fileName);

            file.SaveAs(path);

            string fileUrl = "/products/itemimg/"+HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString()+"/" + fileName;

            context.Response.ContentType = "text/plain";
                context.Response.Write(fileUrl);
            return ;
        }
        catch (Exception ex)
        {
              context.Response.Write("Error: " + ex.Message);
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



 