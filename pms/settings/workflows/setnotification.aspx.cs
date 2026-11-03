using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using CommonClass;
using System.Data.SqlClient;
using System.Linq;

public partial class setnotification : System.Web.UI.Page
{
    public string NotificationType = "222";

    protected void Page_Load(object sender, EventArgs e)
    {
        string type = Convert.ToString(Request.QueryString["type"]);

        if (String.IsNullOrWhiteSpace(type))
        {
            Response.Redirect("setnotification.aspx?type=222", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        type = type.Trim();
        NotificationType = type == "223" ? "223" : "222";
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static TemplateMasterResponse GetTemplateMasters(string type)
    {
        TemplateMasterResponse response = new TemplateMasterResponse();

        try
        {
            int regid = GetRegId();
            if (regid <= 0)
            {
                response.Success = false;
                response.Message = "Invalid company/regid.";
                return response;
            }

            type = (type ?? "222").Trim();
            if (type != "223")
                type = "222";

            DataAccess da = new DataAccess();

            if (type == "222")
            {
                string labelSql = @"
SELECT id, CriticalLabel AS Name
FROM dbo.Pms_tbl_CriticalLabel WITH(NOLOCK)
WHERE ISNULL(status,1)=1
  AND ISNULL(archive,0)=0
  AND regid=" + regid + @"
ORDER BY ISNULL(DisplayOrder,0), ISNULL(Severity,0), CriticalLabel, id;";

                DataSet dsLabels = da.GetDataSetInline(labelSql);
                AddLookupRows(response.Items, dsLabels, "id", "Name");
            }
            else
            {
                string workflowSql = @"
SELECT id, WorkflowName AS Name
FROM dbo.Pms_tbl_Workflow WITH(NOLOCK)
WHERE regid=" + regid + @"
  AND ISNULL(status,1)=1
  AND ISNULL(archive,0)=0
ORDER BY ISNULL(OrderNo,1), WorkflowName, id;";

                DataSet dsWorkflows = da.GetDataSetInline(workflowSql);
                AddLookupRows(response.Items, dsWorkflows, "id", "Name");
            }

            response.Success = true;
            response.Message = "";
            return response;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
            return response;
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
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static PlaceholderResponse GetTemplatePlaceholders(string entityType)
    {
        PlaceholderResponse response = new PlaceholderResponse();

        try
        {
            int regid = GetRegId();
            if (regid <= 0)
            {
                response.Success = false;
                response.Message = "Invalid company/regid.";
                return response;
            }
            string hidcol = ",'iscompletedate','modifieddate','StartTime','Timeline','Reminder','ReminderType','Days','Time','Notifyusers','ChooseDate','Occurrence','tagto','Stage','dependancy','statuscomplete','iscomplete','iscompletepercentage','orderno','Uploaddrawing','lastmodify_byid','lastmodify_date','iscopy','iscopyfrom','tagids','CriticalLabelId','TaskDuration_min','owner'";
            if(entityType=="TASK")
            {
                hidcol+= ",'iscompletedate','issubtask','checkTaskWeightage','owner'";
            }
            else if (entityType == "MILESTONE")
            {
                hidcol += ",'iscompletedate','istask','checkTaskWeightage','Access','Durationtype','Durationhourcal','checkmileWeightage','owner'";
            }
            else if (entityType == "PROJECT")
            {
                hidcol += ",'issubtask','iscompletedate','isdrawing','ApprovalDrawing','DrawingApprovalDate','Uploaddrawing','checktaskWeightage','checksubtaskWeightage','iscountholydays','iscountweekoff','templateid','ishold','hassubtask','hastask','checkprojectWeightage'";
                
            }
            
            string normalizedType = NormalizeEntityType(entityType);
            string tableName = GetEntityTableName(normalizedType);

            if (String.IsNullOrWhiteSpace(tableName))
            {
                response.Success = false;
                response.Message = "Please select Project, Milestone, Task or Sub Task.";
                return response;
            }

            DataAccess da = new DataAccess();

            // Table name comes only from the fixed server-side mapping above.
            // Technical/audit columns are hidden from the template picker.
            string placeholderSql = @"
SELECT C.COLUMN_NAME
FROM INFORMATION_SCHEMA.COLUMNS C
WHERE C.TABLE_SCHEMA='dbo'
  AND LOWER(C.TABLE_NAME)=LOWER('" + tableName.Replace("'", "''") + @"')
  AND LOWER(C.COLUMN_NAME) NOT IN
      ('regid','uid','status','archive','cdate'"+hidcol+") ORDER BY C.ORDINAL_POSITION;";

            SaveLogText("placeholderSql="+ placeholderSql);
            DataSet dsColumns = da.GetDataSetInline(placeholderSql);
           
            if (dsColumns != null && dsColumns.Tables.Count > 0)
            {
                foreach (DataRow row in dsColumns.Tables[0].Rows)
                {
                    string columnName = Convert.ToString(row["COLUMN_NAME"]);
                    if (String.IsNullOrWhiteSpace(columnName))
                        continue;

                    PlaceholderItem placeholder = BuildPlaceholderItem(normalizedType, columnName);
                    if (placeholder == null)
                        continue;

                    // Avoid duplicate user-facing placeholders such as task/tasklist.
                    bool tokenAlreadyAdded = false;
                    foreach (PlaceholderItem existing in response.Placeholders)
                    {
                        if (String.Equals(existing.Token, placeholder.Token, StringComparison.OrdinalIgnoreCase))
                        {
                            tokenAlreadyAdded = true;
                            break;
                        }
                    }

                    if (!tokenAlreadyAdded)
                        response.Placeholders.Add(placeholder);
                }
            }
            
            response.Success = true;
            response.Message = "";
            response.EntityType = normalizedType;
            response.TableName = tableName;
            return response;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
            return response;
        }
    }

    private static readonly Dictionary<string, int> ar_applyon =
        new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            { "PROJECT", 1 },
            { "MILESTONE", 2 },
            { "TASK", 3 },
            { "SUBTASK", 4 }
        };

    private static int GetApplyOnValue(string entityType)
    {
        string normalized = NormalizeEntityType(entityType);

        int value;
        return ar_applyon.TryGetValue(normalized, out value)
            ? value
            : 0;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static NotificationTemplateResponse GetSavedNotificationTemplate(
        string type,
        string entityType,
        int ntypeid)
    {
        NotificationTemplateResponse response = new NotificationTemplateResponse();

        try
        {
            int regid = GetRegId();
            if (regid <= 0)
            {
                response.Success = false;
                response.Message = "Invalid company/regid.";
                return response;
            }

            int typeid = (type ?? "").Trim() == "223" ? 223 : 222;
            string ntfor = NormalizeEntityType(entityType);

            if (String.IsNullOrWhiteSpace(ntfor) || ntypeid <= 0)
            {
                response.Success = false;
                response.Message = "Please select Template For and Critical Label/QC Workflow.";
                return response;
            }

            int applyOn = GetApplyOnValue(ntfor);
            if (applyOn <= 0)
            {
                response.Success = false;
                response.Message = "Invalid Template For (Type).";
                return response;
            }

            string sql = @"
SELECT TOP 1
       id, regid, typeid, ntfor, ntypeid, temptext,
       ISNULL(isActive,1) AS isActive,
       ISNULL(isscreenpoppup,0) AS isscreenpoppup,
       ISNULL(isscreenfreez,0) AS isscreenfreez,
       Whatsapptemplate,
       Whatsapptemplateid,
       Whatsapptemplateprm
FROM dbo.pms_tbl_notificationtemplates WITH(NOLOCK)
WHERE regid=" + regid + @"
  AND typeid=" + typeid + @"
  AND LTRIM(RTRIM(ISNULL(ntfor,'')))='" + applyOn.ToString() + @"'
  AND ntypeid=" + ntypeid + @"
  AND ISNULL(status,1)=1
  AND ISNULL(archive,0)=0
ORDER BY id DESC;";

            DataAccess da = new DataAccess();
            DataSet ds = da.GetDataSetInline(sql);

            response.Success = true;
            response.Message = "";

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                response.Found = false;
                return response;
            }

            DataRow row = ds.Tables[0].Rows[0];

            response.Found = true;
            response.Id = Convert.ToInt32(row["id"]);
            response.TypeId = Convert.ToInt32(row["typeid"]);
            response.NtFor = Convert.ToString(row["ntfor"]);
            response.NTypeId = Convert.ToInt32(row["ntypeid"]);
            response.TempText = Convert.ToString(row["temptext"]);
            response.IsActive = ToIntFlag(row["isActive"]);
            response.IsScreenPopup = ToIntFlag(row["isscreenpoppup"]);
            response.IsScreenFreeze = response.IsScreenPopup == 1
                ? ToIntFlag(row["isscreenfreez"])
                : 0;
            response.WhatsappTemplate = Convert.ToString(row["Whatsapptemplate"]);
            response.WhatsappTemplateId = Convert.ToString(row["Whatsapptemplateid"]);
            response.WhatsappTemplatePrm = Convert.ToString(row["Whatsapptemplateprm"]);

            return response;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
            return response;
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static NotificationTemplateResponse SaveNotificationTemplate(
        NotificationTemplateSaveRequest model)
    {
        NotificationTemplateResponse response = new NotificationTemplateResponse();

        try
        {
            int regid = GetRegId();
            if (regid <= 0)
            {
                response.Success = false;
                response.Message = "Invalid company/regid.";
                return response;
            }

            if (model == null)
            {
                response.Success = false;
                response.Message = "Invalid template data.";
                return response;
            }

            int typeid = model.TypeId == 223 ? 223 : 222;
            string ntfor = NormalizeEntityType(model.NtFor);
            int ntypeid = model.NTypeId;
            string temptext = model.TempText ?? "";

            if (String.IsNullOrWhiteSpace(ntfor))
            {
                response.Success = false;
                response.Message = "Please select Template For (Type).";
                return response;
            }

            int applyOn = GetApplyOnValue(ntfor);
            if (applyOn <= 0)
            {
                response.Success = false;
                response.Message = "Invalid Template For (Type).";
                return response;
            }

            if (ntypeid <= 0)
            {
                response.Success = false;
                response.Message = "Please select Critical Label/QC Workflow.";
                return response;
            }

            if (String.IsNullOrWhiteSpace(StripHtmlToPlainText(temptext)))
            {
                response.Success = false;
                response.Message = "Please enter notification template text.";
                return response;
            }

            int templateIsActive = model.IsActive == 0 ? 0 : 1;
            int isScreenPopup = model.IsScreenPopup == 1 ? 1 : 0;
            int isScreenFreeze = (isScreenPopup == 1 && model.IsScreenFreeze == 1) ? 1 : 0;

            // Whatsapptemplate is the same notification text, but cleaned to plain text:
            // no HTML/span/font/etc. tags are stored in this column.
            string whatsappTemplate = StripHtmlToPlainText(temptext);

            // Parameters are kept separately in the same order in which they occur.
            // Example: TaskName,SubTaskName,ProjectName
            string whatsappTemplatePrm = ExtractTemplateParameterNames(whatsappTemplate);

            DataAccess da = new DataAccess();

            SqlParameter[] prm = new SqlParameter[13];

            prm[0] = new SqlParameter("@id", SqlDbType.Int);
            prm[0].Value = model.Id > 0 ? model.Id : 0;

            prm[1] = new SqlParameter("@regid", SqlDbType.Int);
            prm[1].Value = regid;

            prm[2] = new SqlParameter("@typeid", SqlDbType.Int);
            prm[2].Value = typeid;

            prm[3] = new SqlParameter("@ntfor", SqlDbType.NVarChar, 50);
            prm[3].Value = applyOn.ToString();

            prm[4] = new SqlParameter("@ntypeid", SqlDbType.Int);
            prm[4].Value = ntypeid;

            prm[5] = new SqlParameter("@temptext", SqlDbType.NVarChar, -1);
            prm[5].Value = temptext;

            prm[6] = new SqlParameter("@isscreenpoppup", SqlDbType.Int);
            prm[6].Value = isScreenPopup;

            prm[7] = new SqlParameter("@isscreenfreez", SqlDbType.Int);
            prm[7].Value = isScreenFreeze;

            prm[8] = new SqlParameter("@Whatsapptemplate", SqlDbType.NVarChar, -1);
            prm[8].Value = String.IsNullOrWhiteSpace(whatsappTemplate)
                ? (object)DBNull.Value
                : whatsappTemplate;

            prm[9] = new SqlParameter("@Whatsapptemplateid", SqlDbType.NVarChar, -1);
            prm[9].Value = String.IsNullOrWhiteSpace(model.WhatsappTemplateId)
                ? (object)DBNull.Value
                : model.WhatsappTemplateId.Trim();

            prm[10] = new SqlParameter("@Whatsapptemplateprm", SqlDbType.NVarChar, -1);
            prm[10].Value = String.IsNullOrWhiteSpace(whatsappTemplatePrm)
                ? (object)DBNull.Value
                : whatsappTemplatePrm;

            prm[11] = new SqlParameter("@isActive", SqlDbType.Int);
            prm[11].Value = templateIsActive;

            prm[12] = new SqlParameter("ReturnValue", SqlDbType.Int);
            prm[12].Direction = ParameterDirection.ReturnValue;

            int executeResult = da.executeDMLQuery("usp_Pms_NotificationTemplate_Save", prm);

            int savedId = 0;
            Int32.TryParse(Convert.ToString(prm[12].Value), out savedId);

            if (savedId <= 0)
                savedId = executeResult;

            if (savedId <= 0)
            {
                response.Success = false;
                response.Message = "Template could not be saved.";
                return response;
            }

            response.Success = true;
            response.Found = true;
            response.Id = savedId;
            response.TypeId = typeid;
            response.NtFor = ntfor;
            response.NTypeId = ntypeid;
            response.TempText = temptext;
            response.IsActive = templateIsActive;
            response.IsScreenPopup = isScreenPopup;
            response.IsScreenFreeze = isScreenFreeze;
            response.WhatsappTemplate = whatsappTemplate;
            response.WhatsappTemplateId = model.WhatsappTemplateId ?? "";
            response.WhatsappTemplatePrm = whatsappTemplatePrm;
            response.Message = model.Id > 0
                ? "Template updated successfully."
                : "Template saved successfully.";

            return response;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
            return response;
        }
    }

    private static int ToIntFlag(object value)
    {
        string text = Convert.ToString(value).Trim().ToLowerInvariant();
        return (text == "1" || text == "true") ? 1 : 0;
    }

    private static string StripHtmlToPlainText(string html)
    {
        if (String.IsNullOrWhiteSpace(html))
            return "";

        string text = html;

        text = Regex.Replace(text, @"<br\s*/?>", "\r\n", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"</p\s*>", "\r\n", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"</div\s*>", "\r\n", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"<[^>]+>", String.Empty);

        text = HttpUtility.HtmlDecode(text);
        text = text.Replace('\u00A0', ' ');

        // Remove only formatting noise; keep the actual message and line breaks.
        text = Regex.Replace(text, @"[ \t]+\r?\n", "\r\n");
        text = Regex.Replace(text, @"\r?\n[ \t]+", "\r\n");

        return text.Trim();
    }

    private static string ExtractTemplateParameterNames(string templateText)
    {
        if (String.IsNullOrWhiteSpace(templateText))
            return "";

        MatchCollection matches = Regex.Matches(
            templateText,
            @"\{####(.*?)####\}",
            RegexOptions.IgnoreCase);

        List<string> parameters = new List<string>();

        foreach (Match match in matches)
        {
            string parameterName = match.Groups[1].Value.Trim();

            if (!String.IsNullOrWhiteSpace(parameterName))
                parameters.Add(parameterName);
        }

        return String.Join(",", parameters.ToArray());
    }

    private static string NormalizeEntityType(string value)
    {
        string key = (value ?? "")
            .Trim()
            .Replace(" ", "")
            .Replace("_", "")
            .ToUpperInvariant();

        if (key == "PROJECT") return "PROJECT";
        if (key == "MILESTONE") return "MILESTONE";
        if (key == "TASK") return "TASK";
        if (key == "SUBTASK") return "SUBTASK";

        return "";
    }

    private static string GetEntityTableName(string entityType)
    {
        switch (NormalizeEntityType(entityType))
        {
            case "PROJECT":
                return "Pms_tbl_AddProject";
            case "MILESTONE":
                return "Pms_tbl_AddMilestone";
            case "TASK":
                return "Pms_tbl_AddTask";
            case "SUBTASK":
                return "Pms_tbl_AddSubTask";
            default:
                return "";
        }
    }

    private static PlaceholderItem BuildPlaceholderItem(string entityType, string columnName)
    {
        if (String.IsNullOrWhiteSpace(columnName))
            return null;

        string key = columnName
            .Replace("_", "")
            .Replace(" ", "")
            .Trim()
            .ToLowerInvariant();

        // Never show database/audit columns in the template picker.
        string[] technicalColumns = new string[]
        {
            "id", "regid", "uid", "status", "archive", "cdate",
            "createdate", "createdon", "createdby",
            "modifieddate", "modifiedon", "modifiedby",
            "updatedate", "updatedon", "updatedby",
            "deletedate", "deletedon", "deletedby",
            "rowguid"
        };

        for (int i = 0; i < technicalColumns.Length; i++)
        {
            if (key == technicalColumns[i])
                return null;
        }

        string displayName = "";
        string tokenName = "";

        // Child tables store ProjectId as an id, but notification text
        // should expose the Project Name instead of the numeric id.
        if (key == "projectid")
        {
            displayName = "Project Name";
            tokenName = "ProjectName";
        }
        else if (key == "projectname")
        {
            displayName = "Project Name";
            tokenName = "ProjectName";
        }
        else if (key == "milestoneid")
        {
            displayName = "Milestone";
            tokenName = "Milestone";
        }
        else if (key == "milestone")
        {
            displayName = "Milestone";
            tokenName = "Milestone";
        }
        else if (key == "taskid" || key == "task" || key == "tasklist")
        {
            displayName = "Task Name";
            tokenName = "TaskName";
        }
        else if (key == "taskname")
        {
            displayName = "Task Name";
            tokenName = "TaskName";
        }
        else if (key == "subtaskname")
        {
            displayName = "Sub Task Name";
            tokenName = "SubTaskName";
        }
        else if (key == "Owner" || key == "owner" || key == "ownerid")
        {
            displayName = "Owner Name";
            tokenName = "OwnerName";
        }
        else if (key == "priority" || key == "priorityid")
        {
            displayName = "Priority Name";
            tokenName = "PriorityName";
        }
        else if (key == "pstatus" || key == "projectstatus" || key == "statusid")
        {
            displayName = "Status Name";
            tokenName = "StatusName";
        }
        else if (key == "startdate")
        {
            displayName = "Start Date";
            tokenName = "StartDate";
        }
        else if (key == "targetdate" || key == "enddate")
        {
            displayName = "End Date";
            tokenName = "EndDate";
        }
        else if (key == "txtdays")
        {
            displayName = "Duration Days";
            tokenName = "DurationDays";
        }
        else if (key == "team" || key == "teamid")
        {
            displayName = "Team Name";
            tokenName = "TeamName";
        }
        else if (key == "stage" || key == "stageid")
        {
            displayName = "Stage Name";
            tokenName = "StageName";
        }
        else if (key == "tagto" || key == "tagtoid")
        {
            displayName = "Tagged To";
            tokenName = "TaggedToName";
        }
        else if (key == "notifyusers")
        {
            displayName = "Notify Users";
            tokenName = "NotifyUsersName";
        }
        else
        {
            // Do not expose any unknown raw "*Id" field to the user.
            // Add an explicit friendly mapping above when a new master field is introduced.
            if (key.EndsWith("id"))
                return null;

            displayName = ToDisplayName(columnName);
            tokenName = columnName.Replace(" ", "");
        }

        return new PlaceholderItem
        {
            ColumnName = columnName,
            DisplayName = displayName,
            Token = "{####" + tokenName + "####}"
        };
    }

    private static void AddLookupRows(
        List<LookupItem> target,
        DataSet ds,
        string idColumn,
        string nameColumn)
    {
        if (target == null || ds == null || ds.Tables.Count == 0)
            return;

        HashSet<int> usedIds = new HashSet<int>();

        foreach (DataRow row in ds.Tables[0].Rows)
        {
            int id;
            if (!Int32.TryParse(Convert.ToString(row[idColumn]), out id) || id <= 0)
                continue;

            if (usedIds.Contains(id))
                continue;

            usedIds.Add(id);

            target.Add(new LookupItem
            {
                Id = id,
                Name = Convert.ToString(row[nameColumn])
            });
        }
    }

    private static int GetRegId()
    {
        try
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (cookie == null)
                return 0;

            int regid;
            return Int32.TryParse(Convert.ToString(cookie["id"]), out regid)
                ? regid
                : 0;
        }
        catch
        {
            return 0;
        }
    }

    private static string ToDisplayName(string value)
    {
        if (String.IsNullOrWhiteSpace(value))
            return "";

        string result = value.Replace("_", " ");
        result = Regex.Replace(result, "([a-z0-9])([A-Z])", "$1 $2");
        return result.Trim();
    }

    public class NotificationTemplateSaveRequest
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string NtFor { get; set; }
        public int NTypeId { get; set; }
        public string TempText { get; set; }
        public int IsActive { get; set; }
        public int IsScreenPopup { get; set; }
        public int IsScreenFreeze { get; set; }
        public string WhatsappTemplateId { get; set; }
    }

    public class NotificationTemplateResponse
    {
        public bool Success { get; set; }
        public bool Found { get; set; }
        public string Message { get; set; }
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string NtFor { get; set; }
        public int NTypeId { get; set; }
        public string TempText { get; set; }
        public int IsActive { get; set; }
        public int IsScreenPopup { get; set; }
        public int IsScreenFreeze { get; set; }
        public string WhatsappTemplate { get; set; }
        public string WhatsappTemplateId { get; set; }
        public string WhatsappTemplatePrm { get; set; }

        public NotificationTemplateResponse()
        {
            Message = "";
            NtFor = "";
            TempText = "";
            WhatsappTemplate = "";
            WhatsappTemplateId = "";
            WhatsappTemplatePrm = "";
        }
    }

    public class TemplateMasterResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<LookupItem> Items { get; set; }

        public TemplateMasterResponse()
        {
            Message = "";
            Items = new List<LookupItem>();
        }
    }

    public class PlaceholderResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string EntityType { get; set; }
        public string TableName { get; set; }
        public List<PlaceholderItem> Placeholders { get; set; }

        public PlaceholderResponse()
        {
            Message = "";
            EntityType = "";
            TableName = "";
            Placeholders = new List<PlaceholderItem>();
        }
    }

    public class LookupItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class PlaceholderItem
    {
        public string ColumnName { get; set; }
        public string DisplayName { get; set; }
        public string Token { get; set; }
    }
}
