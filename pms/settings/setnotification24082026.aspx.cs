using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using CommonClass;

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
      ('regid','uid','status','archive','cdate','modifieddate')
ORDER BY C.ORDINAL_POSITION;";

            DataSet dsColumns = da.GetDataSetInline(placeholderSql);

            if (dsColumns != null && dsColumns.Tables.Count > 0)
            {
                foreach (DataRow row in dsColumns.Tables[0].Rows)
                {
                    string columnName = Convert.ToString(row["COLUMN_NAME"]);
                    if (String.IsNullOrWhiteSpace(columnName))
                        continue;

                    response.Placeholders.Add(new PlaceholderItem
                    {
                        ColumnName = columnName,
                        DisplayName = ToDisplayName(columnName),
                        Token = "{####" + columnName + "####}"
                    });
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
