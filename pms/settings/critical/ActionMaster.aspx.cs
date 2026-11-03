using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using CommonClass;

public partial class ActionMaster : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataAccess da = new DataAccess();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
        }
    }

    private static string Json(object value)
    {
        return new JavaScriptSerializer
        {
            MaxJsonLength = Int32.MaxValue
        }.Serialize(value);
    }

    private static List<Dictionary<string, object>> TableToList(DataTable dt)
    {
        List<Dictionary<string, object>> list =
            new List<Dictionary<string, object>>();

        if (dt == null)
            return list;

        foreach (DataRow row in dt.Rows)
        {
            Dictionary<string, object> item =
                new Dictionary<string, object>(
                    StringComparer.OrdinalIgnoreCase
                );

            foreach (DataColumn col in dt.Columns)
            {
                item[col.ColumnName] =
                    row[col] == DBNull.Value
                        ? null
                        : row[col];
            }

            list.Add(item);
        }

        return list;
    }

    private static string GetRegId()
    {
        if (
            HttpContext.Current == null ||
            HttpContext.Current.Request == null ||
            HttpContext.Current.Request.Cookies["cadmin"] == null ||
            HttpContext.Current.Request.Cookies["cadmin"]["id"] == null ||
            HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() == ""
        )
        {
            throw new Exception("Company login cookie not found.");
        }

        return HttpContext.Current
            .Request
            .Cookies["cadmin"]["id"]
            .ToString();
    }

    private static string GetUid()
    {
        if (
            HttpContext.Current == null ||
            HttpContext.Current.Request == null ||
            HttpContext.Current.Request.Cookies["user"] == null ||
            HttpContext.Current.Request.Cookies["user"]["user_id"] == null ||
            HttpContext.Current.Request.Cookies["user"]["user_id"].ToString() == ""
        )
        {
            return "0";
        }

        return HttpContext.Current
            .Request
            .Cookies["user"]["user_id"]
            .ToString();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string GetMasters()
    {
        try
        {
            DataSet ds = new DataSet();
            DataAccess da = new DataAccess();

            SqlParameter[] prm = new SqlParameter[2];
            prm[0] = new SqlParameter("@regid", GetRegId());
            prm[1] = new SqlParameter("@uid", GetUid());

            ds = da.GetDataSet(
                "usp_Pms_SimpleMaster_List",
                prm
            );

            return Json(new
            {
                success = true,
                data = ds.Tables.Count > 0
                    ? TableToList(ds.Tables[0])
                    : new List<Dictionary<string, object>>()
            });
        }
        catch (Exception ex)
        {
            return Json(new
            {
                success = false,
                message = ex.Message
            });
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string SaveMaster(int editId, string masterName)
    {
        try
        {
            if (String.IsNullOrWhiteSpace(masterName))
            {
                throw new Exception("Master Name is required.");
            }

            DataSet ds = new DataSet();
            DataAccess da = new DataAccess();

            SqlParameter[] prm = new SqlParameter[4];
            prm[0] = new SqlParameter("@id", editId);
            prm[1] = new SqlParameter("@MasterName", masterName.Trim());
            prm[2] = new SqlParameter("@regid", GetRegId());
            prm[3] = new SqlParameter("@uid", GetUid());

            ds = da.GetDataSet(
                "usp_Pms_SimpleMaster_Create",
                prm
            );

            return Json(new
            {
                success = true,
                data = ds.Tables.Count > 0
                    ? TableToList(ds.Tables[0])
                    : new List<Dictionary<string, object>>()
            });
        }
        catch (Exception ex)
        {
            return Json(new
            {
                success = false,
                message = ex.Message
            });
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string DeleteMaster(int id)
    {
        try
        {
            DataSet ds = new DataSet();
            DataAccess da = new DataAccess();

            SqlParameter[] prm = new SqlParameter[3];
            prm[0] = new SqlParameter("@id", id);
            prm[1] = new SqlParameter("@regid", GetRegId());
            prm[2] = new SqlParameter("@uid", GetUid());

            ds = da.GetDataSet(
                "usp_Pms_SimpleMaster_Delete",
                prm
            );

            return Json(new
            {
                success = true,
                data = ds.Tables.Count > 0
                    ? TableToList(ds.Tables[0])
                    : new List<Dictionary<string, object>>()
            });
        }
        catch (Exception ex)
        {
            return Json(new
            {
                success = false,
                message = ex.Message
            });
        }
    }
}
