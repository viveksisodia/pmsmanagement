using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using CommonClass;

public partial class CriticalSetting : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataAccess da = new DataAccess();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack) { }
    }

    private static string Json(object value)
    {
        return new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(value);
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
        if (HttpContext.Current == null || HttpContext.Current.Request == null ||
            HttpContext.Current.Request.Cookies["cadmin"] == null ||
            HttpContext.Current.Request.Cookies["cadmin"]["id"] == null ||
            HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() == "")
            throw new Exception("Company login cookie not found.");
        return HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString();
    }

    private static string GetUid()
    {
        if (HttpContext.Current == null || HttpContext.Current.Request == null ||
            HttpContext.Current.Request.Cookies["user"] == null ||
            HttpContext.Current.Request.Cookies["user"]["user_id"] == null ||
            HttpContext.Current.Request.Cookies["user"]["user_id"].ToString() == "")
            throw new Exception("User login cookie not found.");
        return HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string GetMasters()
    {
        try
        {
            DataAccess da = new DataAccess();
            SqlParameter[] prm = new SqlParameter[2];
            prm[0] = new SqlParameter("@regid", GetRegId());
            prm[1] = new SqlParameter("@uid", GetUid());
            DataSet ds = da.GetDataSet("usp_Pms_Critical_GetMasters", prm);

            string regid = GetRegId().Replace("'", "''");
            DataSet empDs = da.GetDataSetInline(
                "select id,DisplayName from tbl_employee where status=1 and archive=0 and regid='" + regid + "' order by DisplayName"
            );

            Dictionary<string, object> result = new Dictionary<string, object>();
            result["labels"] = ds.Tables.Count > 0 ? TableToList(ds.Tables[0]) : new List<Dictionary<string, object>>();
            result["levels"] = ds.Tables.Count > 1 ? TableToList(ds.Tables[1]) : new List<Dictionary<string, object>>();
            result["fields"] = ds.Tables.Count > 2 ? TableToList(ds.Tables[2]) : new List<Dictionary<string, object>>();
            result["operators"] = ds.Tables.Count > 3 ? TableToList(ds.Tables[3]) : new List<Dictionary<string, object>>();
            result["logicalOperators"] = ds.Tables.Count > 4 ? TableToList(ds.Tables[4]) : new List<Dictionary<string, object>>();
            result["actions"] = ds.Tables.Count > 5 ? TableToList(ds.Tables[5]) : new List<Dictionary<string, object>>();
            result["statuses"] = ds.Tables.Count > 6 ? TableToList(ds.Tables[6]) : new List<Dictionary<string, object>>();
            result["priorities"] = ds.Tables.Count > 7 ? TableToList(ds.Tables[7]) : new List<Dictionary<string, object>>();
            result["employees"] = empDs.Tables.Count > 0 ? TableToList(empDs.Tables[0]) : new List<Dictionary<string, object>>();
            result["success"] = true;
            return Json(result);
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string GetRules(int levelId)
    {
        try
        {
            DataAccess da = new DataAccess();
            SqlParameter[] prm = new SqlParameter[2];
            prm[0] = new SqlParameter("@regid", GetRegId());
            prm[1] = new SqlParameter("@LevelId", levelId);
            DataSet ds = da.GetDataSet("usp_Pms_CriticalRule_List", prm);
            return Json(new { success = true, data = ds.Tables.Count > 0 ? TableToList(ds.Tables[0]) : new List<Dictionary<string, object>>() });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string GetRule(int ruleId)
    {
        try
        {
            DataAccess da = new DataAccess();
            SqlParameter[] prm = new SqlParameter[2];
            prm[0] = new SqlParameter("@RuleId", ruleId);
            prm[1] = new SqlParameter("@regid", GetRegId());
            DataSet ds = da.GetDataSet("usp_Pms_CriticalRule_GetById", prm);
            return Json(new
            {
                success = true,
                rule = ds.Tables.Count > 0 ? TableToList(ds.Tables[0]) : new List<Dictionary<string, object>>(),
                conditions = ds.Tables.Count > 1 ? TableToList(ds.Tables[1]) : new List<Dictionary<string, object>>(),
                actions = ds.Tables.Count > 2 ? TableToList(ds.Tables[2]) : new List<Dictionary<string, object>>()
            });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    private static string BuildConditionsXml(List<ConditionModel> conditions)
    {
        XElement root = new XElement("Conditions");
        if (conditions != null)
        {
            int seq = 1;
            foreach (ConditionModel c in conditions)
            {
                root.Add(new XElement("Condition",
                    new XAttribute("BranchNo", c.BranchNo > 0 ? c.BranchNo : 1),
                    new XAttribute("BranchType", String.IsNullOrWhiteSpace(c.BranchType) ? "IF" : c.BranchType.ToUpperInvariant()),
                    new XAttribute("SequenceNo", c.SequenceNo > 0 ? c.SequenceNo : seq),
                    new XAttribute("OpenBracket", c.OpenBracket ?? ""),
                    new XAttribute("ConditionSource", String.IsNullOrWhiteSpace(c.ConditionSource) ? "SELF" : c.ConditionSource.ToUpperInvariant()),
                    new XAttribute("FieldId", c.FieldId.HasValue ? c.FieldId.Value.ToString() : ""),
                    new XAttribute("ChildLevelId", c.ChildLevelId.HasValue ? c.ChildLevelId.Value.ToString() : ""),
                    new XAttribute("ChildCriticalLabelId", c.ChildCriticalLabelId.HasValue ? c.ChildCriticalLabelId.Value.ToString() : ""),
                    new XAttribute("ChildMeasure", c.ChildMeasure ?? ""),
                    new XAttribute("OperatorId", c.OperatorId),
                    new XAttribute("CompareValue", c.CompareValue ?? ""),
                    new XAttribute("CompareValue2", c.CompareValue2 ?? ""),
                    new XAttribute("LogicalOperatorId", c.LogicalOperatorId.HasValue ? c.LogicalOperatorId.Value.ToString() : ""),
                    new XAttribute("CloseBracket", c.CloseBracket ?? ""),
                    new XAttribute("GroupNo", c.GroupNo > 0 ? c.GroupNo : 1)
                ));
                seq++;
            }
        }
        return root.ToString(SaveOptions.DisableFormatting);
    }

    private static string BuildActionsXml(List<ActionModel> actions)
    {
        XElement root = new XElement("Actions");
        if (actions != null)
        {
            int seq = 1;
            foreach (ActionModel a in actions)
            {
                root.Add(new XElement("Action",
                    new XAttribute("BranchNo", a.BranchNo > 0 ? a.BranchNo : 1),
                    new XAttribute("BranchType", String.IsNullOrWhiteSpace(a.BranchType) ? "IF" : a.BranchType.ToUpperInvariant()),
                    new XAttribute("ActionId", a.ActionId),
                    new XAttribute("ActionValue", a.ActionValue ?? ""),
                    new XAttribute("RecipientType", a.RecipientType ?? ""),
                    new XAttribute("RecipientValue", a.RecipientValue ?? ""),
                    new XAttribute("SequenceNo", a.SequenceNo > 0 ? a.SequenceNo : seq)
                ));
                seq++;
            }
        }
        return root.ToString(SaveOptions.DisableFormatting);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string SaveRule(RuleSaveModel model)
    {
        try
        {
            if (model == null) throw new Exception("Rule data was not received.");
            if (model.Conditions == null || model.Conditions.Count == 0)
                throw new Exception("At least one IF/ELSE IF condition is required.");
            if (model.Actions == null || model.Actions.Count == 0)
                throw new Exception("At least one action is required.");

            DataAccess da = new DataAccess();
            string conditionsXml = BuildConditionsXml(model.Conditions);
            string actionsXml = BuildActionsXml(model.Actions);

            SqlParameter[] prm = new SqlParameter[12];
            prm[0] = new SqlParameter("@RuleId", model.RuleId);
            prm[1] = new SqlParameter("@RuleName", model.RuleName ?? "");
            prm[2] = new SqlParameter("@LevelId", model.LevelId);
            prm[3] = new SqlParameter("@CriticalLabelId", model.CriticalLabelId.HasValue ? (object)model.CriticalLabelId.Value : DBNull.Value);
            prm[4] = new SqlParameter("@PriorityId", model.PriorityId.HasValue ? (object)model.PriorityId.Value : DBNull.Value);
            prm[5] = new SqlParameter("@RulePriority", model.RulePriority <= 0 ? 100 : model.RulePriority);
            prm[6] = new SqlParameter("@StopProcessing", model.StopProcessing);
            prm[7] = new SqlParameter("@Description", String.IsNullOrWhiteSpace(model.Description) ? (object)DBNull.Value : model.Description);
            prm[8] = new SqlParameter("@regid", GetRegId());
            prm[9] = new SqlParameter("@uid", GetUid());
            prm[10] = new SqlParameter("@ConditionsXml", SqlDbType.Xml); prm[10].Value = conditionsXml;
            prm[11] = new SqlParameter("@ActionsXml", SqlDbType.Xml); prm[11].Value = actionsXml;

            DataSet ds = da.GetDataSet("usp_Pms_CriticalRule_Save", prm);
            return Json(new { success = true, data = ds.Tables.Count > 0 ? TableToList(ds.Tables[0]) : new List<Dictionary<string, object>>() });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string DeleteRule(int ruleId)
    {
        try
        {
            DataAccess da = new DataAccess();
            SqlParameter[] prm = new SqlParameter[2];
            prm[0] = new SqlParameter("@RuleId", ruleId);
            prm[1] = new SqlParameter("@regid", GetRegId());
            DataSet ds = da.GetDataSet("usp_Pms_CriticalRule_Delete", prm);
            return Json(new { success = true, data = ds.Tables.Count > 0 ? TableToList(ds.Tables[0]) : new List<Dictionary<string, object>>() });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string ChangeRuleStatus(int ruleId, bool status)
    {
        try
        {
            DataAccess da = new DataAccess();
            SqlParameter[] prm = new SqlParameter[3];
            prm[0] = new SqlParameter("@RuleId", ruleId);
            prm[1] = new SqlParameter("@regid", GetRegId());
            prm[2] = new SqlParameter("@status", status);
            DataSet ds = da.GetDataSet("usp_Pms_CriticalRule_ChangeStatus", prm);
            return Json(new { success = true, data = ds.Tables.Count > 0 ? TableToList(ds.Tables[0]) : new List<Dictionary<string, object>>() });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    public class ConditionModel
    {
        public int BranchNo { get; set; }
        public string BranchType { get; set; }
        public int SequenceNo { get; set; }
        public string ConditionSource { get; set; }
        public string OpenBracket { get; set; }
        public int? FieldId { get; set; }
        public int? ChildLevelId { get; set; }
        public int? ChildCriticalLabelId { get; set; }
        public string ChildMeasure { get; set; }
        public int OperatorId { get; set; }
        public string CompareValue { get; set; }
        public string CompareValue2 { get; set; }
        public int? LogicalOperatorId { get; set; }
        public string CloseBracket { get; set; }
        public int GroupNo { get; set; }
    }

    public class ActionModel
    {
        public int BranchNo { get; set; }
        public string BranchType { get; set; }
        public int ActionId { get; set; }
        public string ActionValue { get; set; }
        public string RecipientType { get; set; }
        public string RecipientValue { get; set; }
        public int SequenceNo { get; set; }
    }

    public class RuleSaveModel
    {
        public int RuleId { get; set; }
        public string RuleName { get; set; }
        public int LevelId { get; set; }
        public int? CriticalLabelId { get; set; }
        public int? PriorityId { get; set; }
        public int RulePriority { get; set; }
        public bool StopProcessing { get; set; }
        public string Description { get; set; }
        public List<ConditionModel> Conditions { get; set; }
        public List<ActionModel> Actions { get; set; }
    }
}
