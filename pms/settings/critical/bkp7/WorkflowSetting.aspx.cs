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

public partial class WorkflowSetting : System.Web.UI.Page
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
            return "0";

        return HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string GetMasters()
    {
        try
        {
            DataAccess da = new DataAccess();
            SqlParameter[] prm = new SqlParameter[1];
            prm[0] = new SqlParameter("@regid", GetRegId());
            DataSet ds = da.GetDataSet("usp_Pms_Workflow_GetMasters", prm);

            return Json(new
            {
                success = true,
                actions = ds.Tables.Count > 0 ? TableToList(ds.Tables[0]) : new List<Dictionary<string, object>>(),
                statuses = ds.Tables.Count > 1 ? TableToList(ds.Tables[1]) : new List<Dictionary<string, object>>(),
                priorities = ds.Tables.Count > 2 ? TableToList(ds.Tables[2]) : new List<Dictionary<string, object>>(),
                departments = ds.Tables.Count > 3 ? TableToList(ds.Tables[3]) : new List<Dictionary<string, object>>(),
                designations = ds.Tables.Count > 4 ? TableToList(ds.Tables[4]) : new List<Dictionary<string, object>>()
            });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string GetWorkflows()
    {
        try
        {
            DataAccess da = new DataAccess();
            SqlParameter[] prm = new SqlParameter[1];
            prm[0] = new SqlParameter("@regid", GetRegId());
            DataSet ds = da.GetDataSet("usp_Pms_Workflow_List", prm);

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
    public static string GetWorkflow(int workflowId)
    {
        try
        {
            DataAccess da = new DataAccess();
            SqlParameter[] prm = new SqlParameter[2];
            prm[0] = new SqlParameter("@WorkflowId", workflowId);
            prm[1] = new SqlParameter("@regid", GetRegId());
            DataSet ds = da.GetDataSet("usp_Pms_Workflow_GetById", prm);

            SqlParameter[] uprm = new SqlParameter[2];
            uprm[0] = new SqlParameter("@WorkflowId", workflowId);
            uprm[1] = new SqlParameter("@regid", GetRegId());

            DataSet uds = da.GetDataSet(
                "usp_Pms_WorkflowUserAction_GetByWorkflow",
                uprm
            );

            return Json(new
            {
                success = true,
                workflow = ds.Tables.Count > 0 ? TableToList(ds.Tables[0]) : new List<Dictionary<string, object>>(),
                conditions = ds.Tables.Count > 1 ? TableToList(ds.Tables[1]) : new List<Dictionary<string, object>>(),
                actions = ds.Tables.Count > 2 ? TableToList(ds.Tables[2]) : new List<Dictionary<string, object>>(),
                userActions = uds.Tables.Count > 0 ? TableToList(uds.Tables[0]) : new List<Dictionary<string, object>>()
            });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string SaveWorkflow(WorkflowModel model, string userActionsJson)
    {
        try
        {
            if (model == null) throw new Exception("Workflow data was not received.");
            if (String.IsNullOrWhiteSpace(model.WorkflowName)) throw new Exception("Workflow Name is required.");
            if (String.IsNullOrWhiteSpace(model.ApplyOn)) throw new Exception("Apply On is required.");
            if (model.OrderNo <= 0) model.OrderNo = 1;
            if (model.Actions == null || model.Actions.Count == 0) throw new Exception("At least one action is required.");

            // UserActions are sent separately as JSON text so WebForms does not
            // bind nested nullable values before this PageMethod executes.
            List<UserActionModel> userActions = new List<UserActionModel>();

            if (!String.IsNullOrWhiteSpace(userActionsJson))
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = Int32.MaxValue;
                List<UserActionModel> parsed = serializer.Deserialize<List<UserActionModel>>(userActionsJson);
                if (parsed != null) userActions = parsed;
            }

            for (int i = 0; i < userActions.Count; i++)
            {
                UserActionModel ua = userActions[i];
                if (ua == null) continue;

                string actionType = String.IsNullOrWhiteSpace(ua.ActionType)
                    ? "IF"
                    : ua.ActionType.Trim().ToUpperInvariant();

                if (actionType != "ELSE" && (!ua.ActionId.HasValue || ua.ActionId.Value <= 0))
                    throw new Exception("Action Condition " + (i + 1) + ": QC Action is required.");
            }

            string conditionsXml = BuildConditionsXml(model.Conditions);
            string actionsXml = BuildActionsXml(model.Actions);
            string userActionsXml = BuildUserActionsXml(userActions);

            DataAccess da = new DataAccess();
            SqlParameter[] prm = new SqlParameter[10];
            prm[0] = new SqlParameter("@WorkflowId", model.id);
            prm[1] = new SqlParameter("@WorkflowName", model.WorkflowName.Trim());
            prm[2] = new SqlParameter("@ApplyOn", model.ApplyOn);
            prm[3] = new SqlParameter("@OrderNo", model.OrderNo);
            prm[4] = new SqlParameter("@Description", String.IsNullOrWhiteSpace(model.Description) ? (object)DBNull.Value : model.Description.Trim());
            prm[5] = new SqlParameter("@IsActive", model.IsActive);
            prm[6] = new SqlParameter("@regid", GetRegId());
            prm[7] = new SqlParameter("@uid", GetUid());
            prm[8] = new SqlParameter("@ConditionsXml", SqlDbType.Xml);
            prm[8].Value = String.IsNullOrWhiteSpace(conditionsXml) ? (object)DBNull.Value : conditionsXml;
            prm[9] = new SqlParameter("@ActionsXml", SqlDbType.Xml);
            prm[9].Value = actionsXml;

            DataSet ds = da.GetDataSet("usp_Pms_Workflow_Save", prm);

            int savedWorkflowId = model.id;

            if (savedWorkflowId <= 0 &&
                ds != null &&
                ds.Tables.Count > 0 &&
                ds.Tables[0].Rows.Count > 0)
            {
                DataTable t = ds.Tables[0];

                if (t.Columns.Contains("WorkflowId"))
                    savedWorkflowId = Convert.ToInt32(t.Rows[0]["WorkflowId"]);
                else if (t.Columns.Contains("id"))
                    savedWorkflowId = Convert.ToInt32(t.Rows[0]["id"]);
            }

            if (savedWorkflowId <= 0)
                throw new Exception("Workflow saved but WorkflowId was not returned.");

            SqlParameter[] uprm = new SqlParameter[4];
            uprm[0] = new SqlParameter("@WorkflowId", savedWorkflowId);
            uprm[1] = new SqlParameter("@regid", GetRegId());
            uprm[2] = new SqlParameter("@uid", GetUid());
            uprm[3] = new SqlParameter("@UserActionsXml", SqlDbType.Xml);
            uprm[3].Value = userActions.Count == 0
                ? (object)DBNull.Value
                : userActionsXml;

            DataSet userActionSaveDs = da.GetDataSet(
                "usp_Pms_WorkflowUserAction_Save",
                uprm
            );

            // Never return success if Action Conditions were present but DB save failed.
            if (userActions.Count > 0)
            {
                if (userActionSaveDs == null ||
                    userActionSaveDs.Tables.Count == 0 ||
                    userActionSaveDs.Tables[0].Rows.Count == 0)
                {
                    throw new Exception("Action Conditions save procedure returned no result.");
                }

                DataTable userActionResult = userActionSaveDs.Tables[0];
                if (userActionResult.Columns.Contains("Result") &&
                    Convert.ToInt32(userActionResult.Rows[0]["Result"]) != 1)
                {
                    string userActionMessage = userActionResult.Columns.Contains("Message")
                        ? Convert.ToString(userActionResult.Rows[0]["Message"])
                        : "Action Conditions were not saved.";

                    throw new Exception(userActionMessage);
                }

                // Final DB verification. This catches the old silent-success problem.
                SqlParameter[] checkPrm = new SqlParameter[2];
                checkPrm[0] = new SqlParameter("@WorkflowId", savedWorkflowId);
                checkPrm[1] = new SqlParameter("@regid", GetRegId());

                DataSet checkDs = da.GetDataSet(
                    "usp_Pms_WorkflowUserAction_GetByWorkflow",
                    checkPrm
                );

                int savedUserActionCount =
                    (checkDs != null && checkDs.Tables.Count > 0)
                        ? checkDs.Tables[0].Rows.Count
                        : 0;

                if (savedUserActionCount != userActions.Count)
                {
                    throw new Exception(
                        "Action Conditions were received (" + userActions.Count +
                        ") but DB saved " + savedUserActionCount +
                        ". Run the supplied UserAction SQL fix."
                    );
                }
            }

            return Json(new
            {
                success = true,
                workflowId = savedWorkflowId,
                userActionCount = userActions.Count,
                data = ds != null && ds.Tables.Count > 0
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
    public static string DeleteWorkflow(int workflowId)
    {
        try
        {
            DataAccess da = new DataAccess();
            SqlParameter[] prm = new SqlParameter[3];
            prm[0] = new SqlParameter("@WorkflowId", workflowId);
            prm[1] = new SqlParameter("@regid", GetRegId());
            prm[2] = new SqlParameter("@uid", GetUid());
            DataSet ds = da.GetDataSet("usp_Pms_Workflow_Delete", prm);

            SqlParameter[] uprm = new SqlParameter[3];
            uprm[0] = new SqlParameter("@WorkflowId", workflowId);
            uprm[1] = new SqlParameter("@regid", GetRegId());
            uprm[2] = new SqlParameter("@uid", GetUid());

            da.GetDataSet(
                "usp_Pms_WorkflowUserAction_DeleteByWorkflow",
                uprm
            );

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
    public static string ChangeStatus(int workflowId, bool isActive)
    {
        try
        {
            DataAccess da = new DataAccess();
            SqlParameter[] prm = new SqlParameter[4];
            prm[0] = new SqlParameter("@WorkflowId", workflowId);
            prm[1] = new SqlParameter("@IsActive", isActive);
            prm[2] = new SqlParameter("@regid", GetRegId());
            prm[3] = new SqlParameter("@uid", GetUid());
            DataSet ds = da.GetDataSet("usp_Pms_Workflow_ChangeStatus", prm);

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

    private static string BuildConditionsXml(List<ConditionModel> conditions)
    {
        XElement root = new XElement("Conditions");
        Dictionary<int, int> seq = new Dictionary<int, int>();

        if (conditions != null)
        {
            foreach (ConditionModel c in conditions)
            {
                if (c == null) continue;
                int srNo = c.SrNo <= 0 ? 1 : c.SrNo;
                if (!seq.ContainsKey(srNo)) seq[srNo] = 0;
                seq[srNo]++;

                root.Add(new XElement("Condition",
                    new XAttribute("SrNo", srNo),
                    new XAttribute("Type", String.IsNullOrWhiteSpace(c.Type) ? "IF" : c.Type),
                    new XAttribute("SequenceNo", seq[srNo]),
                    new XAttribute("FieldCode", c.FieldCode ?? ""),
                    new XAttribute("OperatorCode", c.OperatorCode ?? ""),
                    new XAttribute("CompareValue", c.CompareValue ?? ""),
                    new XAttribute("CompareValue2", c.CompareValue2 ?? ""),
                    new XAttribute("LogicalOperator", c.LogicalOperator ?? "")
                ));
            }
        }

        return root.ToString(SaveOptions.DisableFormatting);
    }

    private static string BuildActionsXml(List<ActionModel> actions)
    {
        XElement root = new XElement("Actions");
        Dictionary<int, int> seq = new Dictionary<int, int>();

        if (actions != null)
        {
            foreach (ActionModel a in actions)
            {
                if (a == null) continue;
                int srNo = a.SrNo <= 0 ? 1 : a.SrNo;
                if (!seq.ContainsKey(srNo)) seq[srNo] = 0;
                seq[srNo]++;

                XElement node = new XElement("Action",
                    new XAttribute("SrNo", srNo),
                    new XAttribute("Type", String.IsNullOrWhiteSpace(a.Type) ? "IF" : a.Type),
                    new XAttribute("SequenceNo", seq[srNo])
                );

                if (a.TakenById.HasValue)
                    node.Add(new XAttribute("TakenById", a.TakenById.Value));

                if (a.DesignationId.HasValue)
                    node.Add(new XAttribute("DesignationId", a.DesignationId.Value));

                node.Add(new XAttribute("IsEmail", a.IsEmail ? 1 : 0));
                node.Add(new XAttribute("IsWhatsApp", a.IsWhatsApp ? 1 : 0));
                node.Add(new XAttribute("IsBell", a.IsBell ? 1 : 0));

                root.Add(node);
            }
        }

        return root.ToString(SaveOptions.DisableFormatting);
    }

    private static string BuildUserActionsXml(List<UserActionModel> userActions)
    {
        XElement root = new XElement("UserActions");
        Dictionary<int, int> seq = new Dictionary<int, int>();

        if (userActions != null)
        {
            foreach (UserActionModel a in userActions)
            {
                if (a == null) continue;

                int srNo = a.SrNo <= 0 ? 1 : a.SrNo;

                if (!seq.ContainsKey(srNo))
                    seq[srNo] = 0;

                seq[srNo]++;

                XElement node = new XElement(
                    "UserAction",
                    new XAttribute("SrNo", srNo),
                    new XAttribute(
                        "Type",
                        String.IsNullOrWhiteSpace(a.Type)
                            ? "IF"
                            : a.Type
                    ),
                    new XAttribute("SequenceNo", seq[srNo]),
                    new XAttribute(
                        "ActionType",
                        String.IsNullOrWhiteSpace(a.ActionType)
                            ? "IF"
                            : a.ActionType
                    ),
                    new XAttribute(
                        "IsEmail",
                        a.IsEmail ? 1 : 0
                    ),
                    new XAttribute(
                        "IsWhatsApp",
                        a.IsWhatsApp ? 1 : 0
                    ),
                    new XAttribute(
                        "IsBell",
                        a.IsBell ? 1 : 0
                    ),
                    new XAttribute(
                        "IsRemarkRequired",
                        a.IsRemarkRequired ? 1 : 0
                    )
                );

                if (a.ActionId.HasValue)
                    node.Add(
                        new XAttribute(
                            "ActionId",
                            a.ActionId.Value
                        )
                    );

                if (a.NextStatusId.HasValue)
                    node.Add(
                        new XAttribute(
                            "NextStatusId",
                            a.NextStatusId.Value
                        )
                    );

                if (a.TakenById.HasValue)
                    node.Add(
                        new XAttribute(
                            "TakenById",
                            a.TakenById.Value
                        )
                    );

                if (a.DesignationId.HasValue)
                    node.Add(
                        new XAttribute(
                            "DesignationId",
                            a.DesignationId.Value
                        )
                    );

                root.Add(node);
            }
        }

        return root.ToString(
            SaveOptions.DisableFormatting
        );
    }


    public class WorkflowModel
    {
        public int id { get; set; }
        public string WorkflowName { get; set; }
        public string ApplyOn { get; set; }
        public int OrderNo { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public List<ConditionModel> Conditions { get; set; }
        public List<ActionModel> Actions { get; set; }
        public List<UserActionModel> UserActions { get; set; }
    }

    public class ConditionModel
    {
        public int SrNo { get; set; }
        public string Type { get; set; }
        public string FieldCode { get; set; }
        public string OperatorCode { get; set; }
        public string CompareValue { get; set; }
        public string CompareValue2 { get; set; }
        public string LogicalOperator { get; set; }
    }

    public class ActionModel
    {
        public int SrNo { get; set; }
        public string Type { get; set; }
        public int? TakenById { get; set; }
        public int? DesignationId { get; set; }
        public bool IsEmail { get; set; }
        public bool IsWhatsApp { get; set; }
        public bool IsBell { get; set; }
    }


    public class UserActionModel
    {
        public int SrNo { get; set; }
        public string Type { get; set; }
        public string ActionType { get; set; }
        public int? ActionId { get; set; }
        public int? NextStatusId { get; set; }
        public int? TakenById { get; set; }
        public int? DesignationId { get; set; }
        public bool IsEmail { get; set; }
        public bool IsWhatsApp { get; set; }
        public bool IsBell { get; set; }
        public bool IsRemarkRequired { get; set; }
    }

}
