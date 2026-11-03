using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using CommonClass;

public partial class ActionMasterold : System.Web.UI.Page
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
        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
        if (dt == null) return list;

        foreach (DataRow row in dt.Rows)
        {
            Dictionary<string, object> item = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            foreach (DataColumn col in dt.Columns)
                item[col.ColumnName] = row[col] == DBNull.Value ? null : row[col];

            list.Add(item);
        }
        return list;
    }

    private static string GetRegId()
    {
        if (HttpContext.Current == null ||
            HttpContext.Current.Request == null ||
            HttpContext.Current.Request.Cookies["cadmin"] == null ||
            HttpContext.Current.Request.Cookies["cadmin"]["id"] == null ||
            HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() == "")
        {
            throw new Exception("Company login cookie not found.");
        }

        return HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString();
    }

    private static string GetUid()
    {
        if (HttpContext.Current == null ||
            HttpContext.Current.Request == null ||
            HttpContext.Current.Request.Cookies["user"] == null ||
            HttpContext.Current.Request.Cookies["user"]["user_id"] == null ||
            HttpContext.Current.Request.Cookies["user"]["user_id"].ToString() == "")
        {
            throw new Exception("User login cookie not found.");
        }

        return HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string GetActions()
    {
        try
        {
            DataSet ds = new DataSet();
            DataAccess da = new DataAccess();

            SqlParameter[] prm = new SqlParameter[2];
            prm[0] = new SqlParameter("@regid", GetRegId());
            prm[1] = new SqlParameter("@uid", GetUid());

            ds = da.GetDataSet("usp_Pms_ActionMaster_List", prm);

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
            return Json(new { success = false, message = ex.Message });
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string SaveAction(int editId, ActionModel model)
    {
        try
        {
            if (model == null)
                throw new Exception("Action data was not received.");

            if (String.IsNullOrWhiteSpace(model.ActionName))
                throw new Exception("Action Name is required.");

            if (String.IsNullOrWhiteSpace(model.ActionCode))
                throw new Exception("Action Code is required.");

            DataSet ds = new DataSet();
            DataAccess da = new DataAccess();

            SqlParameter[] prm = new SqlParameter[11];
            prm[0] = new SqlParameter("@id", editId);
            prm[1] = new SqlParameter("@ActionName", model.ActionName.Trim());
            prm[2] = new SqlParameter("@ActionCode", model.ActionCode.Trim().ToUpper());
            prm[3] = new SqlParameter("@RequiresRemark", model.RequiresRemark);
            prm[4] = new SqlParameter("@RequiresRecipient", model.RequiresRecipient);
            prm[5] = new SqlParameter("@RequiresDepartment", model.RequiresDepartment);
            prm[6] = new SqlParameter("@IsFinalAction", model.IsFinalAction);
            prm[7] = new SqlParameter("@SortOrder", model.SortOrder < 0 ? 0 : model.SortOrder);
            prm[8] = new SqlParameter("@IsActive", model.IsActive);
            prm[9] = new SqlParameter("@regid", GetRegId());
            prm[10] = new SqlParameter("@uid", GetUid());

            ds = da.GetDataSet("usp_Pms_ActionMaster_Save", prm);

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
            return Json(new { success = false, message = ex.Message });
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string DeleteAction(int id)
    {
        try
        {
            DataSet ds = new DataSet();
            DataAccess da = new DataAccess();

            SqlParameter[] prm = new SqlParameter[3];
            prm[0] = new SqlParameter("@id", id);
            prm[1] = new SqlParameter("@regid", GetRegId());
            prm[2] = new SqlParameter("@uid", GetUid());

            ds = da.GetDataSet("usp_Pms_ActionMaster_Delete", prm);

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
            return Json(new { success = false, message = ex.Message });
        }
    }

    public class ActionModel
    {
        public int id { get; set; }
        public string ActionName { get; set; }
        public string ActionCode { get; set; }
        public bool RequiresRemark { get; set; }
        public bool RequiresRecipient { get; set; }
        public bool RequiresDepartment { get; set; }
        public bool IsFinalAction { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
