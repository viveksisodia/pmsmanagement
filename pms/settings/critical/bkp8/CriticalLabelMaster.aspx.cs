using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using CommonClass;

public partial class CriticalLabelMaster : System.Web.UI.Page
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
    public static string GetLabels()
    {
        try
        {
            DataSet ds = new DataSet();
            DataAccess da = new DataAccess();

            SqlParameter[] prm = new SqlParameter[2];
            prm[0] = new SqlParameter("@regid", GetRegId());
            prm[1] = new SqlParameter("@uid", GetUid());

            ds = da.GetDataSet("usp_Pms_Critical_GetMasters", prm);

            return Json(new
            {
                success = true,
                data = ds.Tables.Count > 0 ? TableToList(ds.Tables[0]) : new List<Dictionary<string, object>>()
            });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string SaveLabel(int editId, LabelModel model)
    {
        try
        {
            if (model == null)
                throw new Exception("Critical label data was not received.");

            DataSet ds = new DataSet();
            DataAccess da = new DataAccess();

            SqlParameter[] prm = new SqlParameter[7];
            prm[0] = new SqlParameter("@id", editId);
            prm[1] = new SqlParameter("@CriticalLabel", model.CriticalLabel == null ? "" : model.CriticalLabel.Trim());
            prm[2] = new SqlParameter("@ColorCode", String.IsNullOrWhiteSpace(model.ColorCode) ? "#28a745" : model.ColorCode);
            prm[3] = new SqlParameter("@Severity", model.Severity <= 0 ? 1 : model.Severity);
            prm[4] = new SqlParameter("@DisplayOrder", model.DisplayOrder < 0 ? 0 : model.DisplayOrder);
            prm[5] = new SqlParameter("@regid", GetRegId());
            prm[6] = new SqlParameter("@uid", GetUid());

            ds = da.GetDataSet("usp_Pms_CriticalLabel_Save", prm);

            return Json(new
            {
                success = true,
                data = ds.Tables.Count > 0 ? TableToList(ds.Tables[0]) : new List<Dictionary<string, object>>()
            });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string DeleteLabel(int id)
    {
        try
        {
            DataSet ds = new DataSet();
            DataAccess da = new DataAccess();

            SqlParameter[] prm = new SqlParameter[3];
            prm[0] = new SqlParameter("@id", id);
            prm[1] = new SqlParameter("@regid", GetRegId());
            prm[2] = new SqlParameter("@uid", GetUid());

            ds = da.GetDataSet("usp_Pms_CriticalLabel_Delete", prm);

            return Json(new
            {
                success = true,
                data = ds.Tables.Count > 0 ? TableToList(ds.Tables[0]) : new List<Dictionary<string, object>>()
            });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    public class LabelModel
    {
        public int id { get; set; }
        public string CriticalLabel { get; set; }
        public string ColorCode { get; set; }
        public int Severity { get; set; }
        public int DisplayOrder { get; set; }
    }
}
