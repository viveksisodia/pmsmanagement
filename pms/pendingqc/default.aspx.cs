using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using CommonClass;

public partial class pms_qcaction_default : System.Web.UI.Page
{
    public DataAccess da = new DataAccess();

    // Requested behavior:
    // 0 = normal employee -> only QC assigned/notified to logged-in employee.
    // 1 = admin           -> all company QC runtimes are visible for action.
    // Keep default 0. Set this from your existing permission/role logic for admins.
    public int isadmin = 0;

    public int pageRegId = 0;
    public int pageEmployeeId = 0;
    public string qcApiBase = "/api/manageqcworkflows";

    protected void Page_Load(object sender, EventArgs e)
    {
        int.TryParse(GetCookieValue("cadmin", "id"), out pageRegId);
        int.TryParse(GetCookieValue("user", "user_id"), out pageEmployeeId);

        // Admin check uses your existing role logic. Default remains 0.
        isadmin = pageEmployeeId > 0
            ? checkadmin(pageEmployeeId.ToString(CultureInfo.InvariantCulture))
            : 0;

        // PageMethods are static, therefore keep the server-side admin result in Session.
        // The client-side isadmin variable is display-only; data filtering uses this Session value.
        Session["QCActionIsAdmin"] = isadmin;

        // Keep the existing relative URL as the UI default.
        // When the workflow API is hosted in a separate IIS application/port,
        // configure QCWorkflowApiBaseUrl in Web.config.
        string configuredApi = ConfigurationManager.AppSettings["QCWorkflowApiBaseUrl"];
        if (!String.IsNullOrWhiteSpace(configuredApi))
            qcApiBase = configuredApi.Trim().TrimEnd('/');
    }

    private static string GetCookieValue(string cookieName, string key)
    {
        HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
        if (cookie == null) return "";
        return Convert.ToString(cookie[key], CultureInfo.InvariantCulture) ?? "";
    }

    protected static int checkadmin(string struserid = "0")
    {
        int retval = 0;
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        function fns = new function();
        string strsql = "select Manage_Profile.* from Manage_Profile inner join tbl_employee on Manage_Profile.id=cast(tbl_employee.roleid as int) where tbl_employee.id='" + struserid + "'  and Manage_Profile.status=1 and Manage_Profile.archive=0 ";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["id"].ToString() == "1")
            {
                retval = 1;
            }
        }
        return retval;
    }

    private static int GetCurrentRegId()
    {
        int value;
        Int32.TryParse(GetCookieValue("cadmin", "id"), out value);
        return value;
    }

    private static int GetCurrentEmployeeId()
    {
        int value;
        Int32.TryParse(GetCookieValue("user", "user_id"), out value);
        return value;
    }

    private static bool GetCurrentIsAdmin()
    {
        object value = HttpContext.Current.Session == null
            ? null
            : HttpContext.Current.Session["QCActionIsAdmin"];

        int admin;
        return Int32.TryParse(Convert.ToString(value, CultureInfo.InvariantCulture), out admin)
               && admin == 1;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static QCGridResponse GetQCActionGrid(QCGridFilterRequest model)
    {
        QCGridResponse response = new QCGridResponse();

        try
        {
            int regid = GetCurrentRegId();
            int employeeId = GetCurrentEmployeeId();
            bool isAdmin = GetCurrentIsAdmin();

            if (regid <= 0)
                throw new Exception("Company login was not found.");

            if (!isAdmin && employeeId <= 0)
                throw new Exception("Employee login was not found.");

            if (model == null) model = new QCGridFilterRequest();

            SqlParameter[] prm = new SqlParameter[9];
            prm[0] = new SqlParameter("@regid", SqlDbType.Int);
            prm[0].Value = regid;
            prm[1] = new SqlParameter("@EmployeeId", SqlDbType.Int);
            prm[1].Value = employeeId;
            prm[2] = new SqlParameter("@IsAdmin", SqlDbType.Bit);
            prm[2].Value = isAdmin;
            prm[3] = new SqlParameter("@EntityType", SqlDbType.NVarChar, 20);
            prm[3].Value = DbText(model.EntityType);
            prm[4] = new SqlParameter("@SearchText", SqlDbType.NVarChar, 250);
            prm[4].Value = DbText(model.SearchText);
            prm[5] = new SqlParameter("@DateFilter", SqlDbType.NVarChar, 30);
            prm[5].Value = DbText(DefaultIfBlank(model.DateFilter, "ALL"));
            prm[6] = new SqlParameter("@FromDate", SqlDbType.Date);
            prm[6].Value = DbDate(model.FromDate);
            prm[7] = new SqlParameter("@ToDate", SqlDbType.Date);
            prm[7].Value = DbDate(model.ToDate);
            prm[8] = new SqlParameter("@QCStatus", SqlDbType.NVarChar, 30);
            prm[8].Value = DbText(DefaultIfBlank(model.QCStatus, "ALL"));

            DataAccess da = new DataAccess();
            DataSet ds = da.GetDataSet("usp_Pms_QCActionGrid_Get", prm);

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    response.Rows.Add(new QCGridRow
                    {
                        RuntimeId = ReadInt(row, "RuntimeId"),
                        WorkflowId = ReadInt(row, "WorkflowId"),
                        WorkflowName = ReadString(row, "WorkflowName"),
                        BranchNo = ReadInt(row, "BranchNo"),
                        BranchType = ReadString(row, "BranchType"),
                        EntityType = ReadString(row, "EntityType"),
                        EntityId = ReadInt(row, "EntityId"),
                        EntityName = ReadString(row, "EntityName"),
                        HierarchySrNo = GetHierarchySrNo(
                            regid,
                            ReadString(row, "EntityType"),
                            ReadInt(row, "EntityId")
                        ),
                        ProjectName = ReadString(row, "ProjectName"),
                        CycleNo = ReadInt(row, "CycleNo"),
                        RuntimeStatus = ReadString(row, "RuntimeStatus"),
                        QCStatus = ReadString(row, "QCStatus"),
                        QCDate = ReadDateText(row, "QCDate"),
                        NotifiedTo = ReadString(row, "NotifiedTo"),
                        ActionTakenBy = ReadNullableInt(row, "ActionTakenBy"),
                        ActionTakenByName = ReadString(row, "ActionTakenByName"),
                        TakenActionId = ReadNullableInt(row, "TakenActionId"),
                        TakenActionName = ReadString(row, "TakenActionName"),
                        ActionTakenDate = ReadActionDateText(row, "ActionTakenDate"),
                        ActionRemark = ReadString(row, "ActionRemark")
                    });
                }
            }

            Dictionary<int, QCGridRow> rowMap = new Dictionary<int, QCGridRow>();
            foreach (QCGridRow item in response.Rows)
                rowMap[item.RuntimeId] = item;

            if (ds != null && ds.Tables.Count > 1)
            {
                foreach (DataRow row in ds.Tables[1].Rows)
                {
                    int runtimeId = ReadInt(row, "RuntimeId");
                    QCGridRow parent;
                    if (!rowMap.TryGetValue(runtimeId, out parent))
                        continue;

                    parent.Actions.Add(new QCActionOption
                    {
                        WorkflowUserActionId = ReadInt(row, "WorkflowUserActionId"),
                        ActionId = ReadInt(row, "ActionId"),
                        ActionName = ReadString(row, "ActionName"),
                        NextStatusId = ReadNullableInt(row, "NextStatusId"),
                        IsRemarkRequired = ReadBool(row, "IsRemarkRequired"),
                        SequenceNo = ReadInt(row, "SequenceNo")
                    });
                }
            }

            var history = new List<QCGridRow>();
            if (ds != null && ds.Tables.Count > 2)
                foreach (DataRow row in ds.Tables[2].Rows)
                    history.Add(new QCGridRow {
                        EntityType = ReadString(row, "EntityType"), EntityId = ReadInt(row, "EntityId"),
                        RuntimeId = ReadInt(row, "RuntimeId"), TakenActionId = ReadNullableInt(row, "TakenActionId"),
                        TakenActionName = ReadString(row, "TakenActionName"),
                        ActionTakenBy = ReadNullableInt(row, "ActionTakenBy"),
                        ActionTakenByName = ReadString(row, "ActionTakenByName"),
                        ActionTakenDate = ReadActionDateText(row, "ActionTakenDate"),
                        ActionRemark = ReadString(row, "ActionRemark")
                    });

            var grouped = new List<QCGridRow>();
            foreach (var group in response.Rows.GroupBy(x => x.EntityType + ":" + x.EntityId))
            {
                var pending = group.Where(x => x.RuntimeStatus == "OPEN").ToList();
                var entries = history.Where(x => x.EntityType == group.First().EntityType &&
                                                x.EntityId == group.First().EntityId).ToList();
                var latest = entries.FirstOrDefault(); // SQL orders by actual action datetime.
                var representative = pending.FirstOrDefault() ??
                    group.FirstOrDefault(x => latest != null && x.RuntimeId == latest.RuntimeId) ?? group.First();
                // Copy before attaching children, avoiding circular JSON references.
                var serializer = new JavaScriptSerializer();
                var summary = serializer.Deserialize<QCGridRow>(serializer.Serialize(representative));
                summary.PendingRuntimes = pending;
                summary.History = entries;
                summary.QCStatus = pending.Count > 0 ? "PENDING" : latest != null ? "ACTION TAKEN" : "CLOSED";
                if (latest != null) {
                    summary.TakenActionId = latest.TakenActionId;
                    summary.TakenActionName = latest.TakenActionName;
                    summary.ActionTakenBy = latest.ActionTakenBy;
                    summary.ActionTakenByName = latest.ActionTakenByName;
                    summary.ActionTakenDate = latest.ActionTakenDate;
                    summary.ActionRemark = latest.ActionRemark;
                }
                string selectedStatus = DefaultIfBlank(model.QCStatus, "ALL").ToUpperInvariant().Replace(" ", "");
                if (selectedStatus == "ALL" || summary.QCStatus.Replace(" ", "") == selectedStatus)
                    grouped.Add(summary);
            }
            response.Rows = grouped;

            // Execute the existing DB procedure on every request, but return only
            // the requested page to the browser along with the complete row count.
            int totalRows = response.Rows.Count;
            int pageSize = model.PageSize > 0 ? model.PageSize : 10;
            int pageNumber = model.PageNumber > 0 ? model.PageNumber : 1;
            int totalPages = Math.Max(1,
                (int)Math.Ceiling(totalRows / (double)pageSize));

            if (pageNumber > totalPages)
                pageNumber = totalPages;

            response.Rows = response.Rows
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            response.Success = true;
            response.IsAdmin = isAdmin ? 1 : 0;
            response.TotalRows = totalRows;
            return response;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
            return response;
        }
    }

    // Returns the same hierarchy number displayed in Assign Project:
    // Milestone = 1, Task = 1.1, Subtask = 1.1.1.
    // This is a read-only lookup and does not change the existing QC procedure/actions.
    private static string GetHierarchySrNo(int regid, string entityType, int entityId)
    {
        if (regid <= 0 || entityId <= 0) return "";

        string type = (entityType ?? "").Trim().ToUpperInvariant();
        string reg = regid.ToString(CultureInfo.InvariantCulture);
        string id = entityId.ToString(CultureInfo.InvariantCulture);
        string sql;

        if (type == "MILESTONE")
        {
            sql =
                "SELECT CAST(M.orderno AS VARCHAR(20)) AS HierarchySrNo " +
                "FROM Pms_tbl_AddMilestone M WHERE M.id=" + id + " AND M.regid=" + reg +
                " AND M.status=1 AND M.archive=0";
        }
        else if (type == "TASK")
        {
            sql =
                "SELECT CAST(M.orderno AS VARCHAR(20)) + '.' + " +
                "CAST(T.orderno AS VARCHAR(20)) AS HierarchySrNo " +
                "FROM Pms_tbl_AddTask T INNER JOIN Pms_tbl_AddMilestone M ON M.id=T.milestoneid " +
                "WHERE T.id=" + id + " AND T.regid=" + reg +
                " AND T.status=1 AND T.archive=0 AND M.status=1 AND M.archive=0";
        }
        else if (type == "SUBTASK")
        {
            sql =
                "SELECT CAST(M.orderno AS VARCHAR(20)) + '.' + " +
                "CAST(T.orderno AS VARCHAR(20)) + '.' + " +
                "CAST(S.orderno AS VARCHAR(20)) AS HierarchySrNo " +
                "FROM Pms_tbl_AddSubTask S " +
                "INNER JOIN Pms_tbl_AddTask T ON T.id=S.task " +
                "INNER JOIN Pms_tbl_AddMilestone M ON M.id=T.milestoneid " +
                "WHERE S.id=" + id + " AND S.regid=" + reg +
                " AND S.status=1 AND S.archive=0 " +
                "AND T.status=1 AND T.archive=0 AND M.status=1 AND M.archive=0";
        }
        else
        {
            return "";
        }

        try
        {
            DataAccess da = new DataAccess();
            DataSet ds = da.GetDataSetInline(sql);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return "";
            return ReadString(ds.Tables[0].Rows[0], "HierarchySrNo");
        }
        catch
        {
            // A serial-number lookup must never stop the existing QC grid.
            return "";
        }
    }

    // =============================================================
    // QC USER ACTION WEBMETHOD
    // =============================================================
    // Executes the QC action directly from this WebForms application.
    // RegId and UserId are always read from the authenticated cookies and are
    // never accepted from JavaScript.

    protected static int checkstatus_complete(int statusid)
    {
        int ret = 0;
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        function fns = new function();
        try
        {
            string striscomplt = "select  isnull(iscomplete,0) as iscomplete from Pms_tbl_Projectstatus where status = 1 and archive = 0  and id='" + statusid + "'";
            DataSet dsiscomplt = da.GetDataSetInline(striscomplt);
            if (dsiscomplt.Tables[0].Rows.Count > 0)
            {

                ret = Convert.ToInt32(dsiscomplt.Tables[0].Rows[0]["iscomplete"]);

            }
        }
        catch { }
        return ret;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static QCActionProxyResponse ExecuteQCUserAction(
        string runtimeId,
        string actionId,
        string remark)
    {
        try
        {
            int regid = GetCurrentRegId();
            int employeeId = GetCurrentEmployeeId();
            int runtimeKey;
            int actionKey;

            if (regid <= 0)
                return QCActionProxyResponse.Fail("Company login was not found.");

            if (employeeId <= 0)
                return QCActionProxyResponse.Fail("Employee login was not found.");

            if (!Int32.TryParse(runtimeId, out runtimeKey) || runtimeKey <= 0)
                return QCActionProxyResponse.Fail("Invalid Runtime Id.");

            if (!Int32.TryParse(actionId, out actionKey) || actionKey <= 0)
                return QCActionProxyResponse.Fail("Please select QC Action.");

            // IMPORTANT: execute through Service 2 in manageqcworkflowsController.
            // That service executes the action, updates history and processes the
            // Stage-2 Bell/WhatsApp/Email notification queue.
            QCUserWorkflowServiceResponse serviceResult = CallQCUserWorkflowService(
                regid,
                employeeId,
                runtimeKey,
                actionKey,
                remark
            );

            if (serviceResult == null)
                return QCActionProxyResponse.Fail("QC user workflow service returned no result.");

            QCActionProxyResponse result = new QCActionProxyResponse();
            result.Result = serviceResult.Result;
            result.Message = serviceResult.Message;

            if (result.Result != 1)
                return result;

            string entityType = serviceResult.EntityType;
            int entityId = serviceResult.EntityId;
            string actionName = serviceResult.ActionName;
            string actionRemark = String.IsNullOrWhiteSpace(remark)?"": remark.Trim();
            int laststatusid = serviceResult.PreviousStatusId;
            int nextstatusid = serviceResult.NextStatusId;
            int nextstatusid_complete = 0;
            try
            {
                if (nextstatusid > 0)
                {
                    nextstatusid_complete = checkstatus_complete(nextstatusid);
                }
            }
            catch { }

            // Store the latest successfully completed QC action on the actual
            // Project/Milestone/Task/Subtask record. Failure here must not cause
            // the already-completed workflow action to be submitted again.
            try
            {
                UpdateEntityLastQCAction(
                    regid,
                    entityType,
                    entityId,
                    actionKey,
                    actionName,
                    actionRemark,
                    employeeId,
                    laststatusid,
                    nextstatusid

                );

                // The service-success guard above has already passed.
                // Use the existing status routines only after last QC action update succeeds.
                if (nextstatusid > 0)
                {
                    string statusResult = "1";
                    string itemId = entityId.ToString(CultureInfo.InvariantCulture);
                    string statusId = nextstatusid.ToString(CultureInfo.InvariantCulture);
                    string isComplete = nextstatusid_complete.ToString(CultureInfo.InvariantCulture);
                    string itemType = (entityType ?? "").Trim().ToUpperInvariant();

                    if (itemType == "MILESTONE")
                        statusResult = UpdateMilestone_Status(itemId, statusId, isComplete);
                    else if (itemType == "TASK")
                        statusResult = UpdateTask_Status(itemId, statusId, isComplete);
                    else if (itemType == "SUBTASK")
                        statusResult = UpdateSubtask_Status(itemId, statusId, isComplete);

                    if (statusResult != "1")
                        throw new InvalidOperationException("Status update failed: " + statusResult);
                }
            }
            catch (Exception lastActionEx)
            {
                result.Message = (result.Message ?? "QC action completed.") +
                    " QC action succeeded, but the local last-action/status update failed: " + lastActionEx.Message;
                //SaveQCUserActionLog(
                SaveLogText(
                    "Last QC action/status update failed. RegId=" + regid +
                    ", EntityType=" + entityType +
                    ", EntityId=" + entityId +
                    ", ActionId=" + actionKey +
                    ", Error=" + lastActionEx
                );
            }
            SaveLogText(
                "Action and Stage2 notification service completed. RegId=" + regid +
                ", RuntimeId=" + runtimeKey +
                ", UserId=" + employeeId +
                ", ActionId=" + actionKey +
                ", NotificationStatus=" + (serviceResult.NotificationStatus ?? "")
            );
            //SaveQCUserActionLog(
            //    "Action and Stage2 notification service completed. RegId=" + regid +
            //    ", RuntimeId=" + runtimeKey +
            //    ", UserId=" + employeeId +
            //    ", ActionId=" + actionKey +
            //    ", NotificationStatus=" + (serviceResult.NotificationStatus ?? "")
            //);

            return result;
        }
        catch (Exception ex)
        {
            SaveQCUserActionLog("ExecuteQCUserAction ERROR => " + ex);
            return QCActionProxyResponse.Fail(ex.Message);
        }
    }

    private static QCUserWorkflowServiceResponse CallQCUserWorkflowService(
        int regid,
        int employeeId,
        int runtimeId,
        int actionId,
        string remark)
    {
        
        //string configuredApi = ConfigurationManager.AppSettings["QCWorkflowApiBaseUrl"];
        string configuredApi = ConfigurationManager.AppSettings["gmailapi"].ToString();
        SaveLogText("AVchk_configuredApi=" + configuredApi);
        string apiBase;

        if (!String.IsNullOrWhiteSpace(configuredApi))
        {
            apiBase = configuredApi.Trim().TrimEnd('/');
            apiBase = apiBase+ "/api/manageqcworkflows";
        }
        else
        {
            HttpRequest currentRequest = HttpContext.Current.Request;
            apiBase = currentRequest.Url.GetLeftPart(UriPartial.Authority) +
                      "/api/manageqcworkflows";
        }
        SaveLogText("AVchk_apiBase=" + apiBase);
        string serviceUrl = apiBase + "/manageuserworkflows";
        SaveLogText("AVchk_serviceUrl=" + serviceUrl);
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        string json = serializer.Serialize(new
        {
            regid = regid.ToString(CultureInfo.InvariantCulture),
            uid = employeeId.ToString(CultureInfo.InvariantCulture),
            runtimeId = runtimeId.ToString(CultureInfo.InvariantCulture),
            actionId = actionId.ToString(CultureInfo.InvariantCulture),
            remark = String.IsNullOrWhiteSpace(remark) ? "" : remark.Trim()
        });
        SaveLogText("jsonbody="+ json);
        byte[] body = Encoding.UTF8.GetBytes(json);
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceUrl);
        request.Method = "POST";
        request.ContentType = "application/json; charset=utf-8";
        request.Accept = "application/json";
        request.Timeout = 120000;
        request.ReadWriteTimeout = 120000;
        request.ContentLength = body.Length;

        using (Stream requestStream = request.GetRequestStream())
        {
            requestStream.Write(body, 0, body.Length);
        }

        try
        {
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string responseJson = reader.ReadToEnd();
                return serializer.Deserialize<QCUserWorkflowServiceResponse>(responseJson);
            }
        }
        catch (WebException ex)
        {
            string errorText = "";
            if (ex.Response != null)
            {
                using (StreamReader reader = new StreamReader(ex.Response.GetResponseStream()))
                    errorText = reader.ReadToEnd();
            }

            throw new InvalidOperationException(
                "QC user workflow service call failed." +
                (String.IsNullOrWhiteSpace(errorText) ? "" : " " + errorText),
                ex
            );
        }
    }

    private static void UpdateEntityLastQCAction(
        int regid,
        string entityType,
        int entityId,
        int actionId,
        string actionName,
        string actionRemark,
        int takenBy,
        int oldstatusid,
        int newstatusd
        )
    {
        if (regid <= 0 || entityId <= 0 || actionId <= 0 || takenBy <= 0)
            throw new ArgumentException("Invalid last QC action data.");
        DataAccess da = new DataAccess();
        string tableName;
        string col_status= "ProjectStatus";
        string normalizedType = (entityType ?? "").Trim().ToUpperInvariant();

        if (normalizedType == "PROJECT")
        {
            tableName = "Pms_tbl_AddProject";
        }
        else if (normalizedType == "MILESTONE")
        {
            tableName = "Pms_tbl_AddMilestone";
            col_status = "P_Status";
        }
        else if (normalizedType == "TASK")
        {
            tableName = "Pms_tbl_AddTask";
            col_status = "P_Status";
        }
        else if (normalizedType == "SUBTASK")
        {
            tableName = "Pms_tbl_AddSubTask";
            col_status = "P_Status";
        }
        else
            throw new ArgumentException("Invalid QC EntityType: " + entityType);

        string safeActionName = String.IsNullOrWhiteSpace(actionName)
            ? null
            : actionName.Trim();

        if (safeActionName != null && safeActionName.Length > 50)
            safeActionName = safeActionName.Substring(0, 50);
    string actionNameSql = safeActionName == null
            ? "NULL"
            : "N'" + safeActionName.Replace("'", "''") + "'";
        string safeActionRemark = String.IsNullOrWhiteSpace(actionRemark)
    ? null
    : actionRemark.Trim();

        string actionRemarkSql = safeActionRemark == null
            ? "NULL"
            : "N'" + safeActionRemark.Replace("'", "''") + "'";

        string sql =
            "UPDATE dbo." + tableName + " " +
            "SET " + (normalizedType == "PROJECT" ? "" : "lastqcactionid=" + actionId.ToString(CultureInfo.InvariantCulture) + ", ") +
            "takenlastaction=" + actionId.ToString(CultureInfo.InvariantCulture) + ", " +
            "lastactionname=" + actionNameSql + ", " +
            "lastactiondate='" + Convert.ToString(da.getcdatetime(), CultureInfo.InvariantCulture).Replace("'", "''") + "', " +
            "lastactionremark=" + actionRemarkSql + ", " +
            "takenby=" + takenBy.ToString(CultureInfo.InvariantCulture) + " ";

           //sql = sql + @" ," + col_status + "=" + newstatusd.ToString(CultureInfo.InvariantCulture) + " ";

           sql = sql + @" WHERE id=" + entityId.ToString(CultureInfo.InvariantCulture) + " " +
            "AND regid=" + regid.ToString(CultureInfo.InvariantCulture) + "; " +
            "SELECT @@ROWCOUNT AS AffectedRows;";
        SaveLogText("lastsql="+ sql);
       
        DataSet ds = da.GetDataSetInline(sql);

        if (ds == null ||
            ds.Tables.Count == 0 ||
            ds.Tables[0].Rows.Count == 0)
        {
            throw new InvalidOperationException(
                "Last QC action update returned no result."
            );
        }

        int affectedRows = ReadInt(ds.Tables[0].Rows[0], "AffectedRows");
        if (affectedRows != 1)
        {
            throw new InvalidOperationException(
                "Last QC action entity was not updated. Affected rows: " +
                affectedRows
            );
        }
    }

    private static void MarkQCWorkflowHistoryActionTaken(
        int regid,
        int runtimeId,
        int actionTakenBy,
        int takenActionId,
        string takenActionName,
        string remark,
        int? previousStatusId,
        int? nextStatusId,
        string runtimeStatus)
    {
        SqlParameter[] prm = new SqlParameter[9];
        prm[0] = new SqlParameter("@regid", regid);
        prm[1] = new SqlParameter("@RuntimeId", runtimeId);
        prm[2] = new SqlParameter("@ActionTakenBy", actionTakenBy);
        prm[3] = new SqlParameter("@TakenActionId", takenActionId);
        prm[4] = new SqlParameter("@TakenActionName",
            String.IsNullOrWhiteSpace(takenActionName)
                ? (object)DBNull.Value
                : takenActionName.Trim());
        prm[5] = new SqlParameter("@Remark",
            String.IsNullOrWhiteSpace(remark)
                ? (object)DBNull.Value
                : remark.Trim());
        prm[6] = new SqlParameter("@PreviousStatusId",
            previousStatusId.HasValue
                ? (object)previousStatusId.Value
                : DBNull.Value);
        prm[7] = new SqlParameter("@NextStatusId",
            nextStatusId.HasValue
                ? (object)nextStatusId.Value
                : DBNull.Value);
        prm[8] = new SqlParameter("@RuntimeStatus",
            String.IsNullOrWhiteSpace(runtimeStatus)
                ? (object)DBNull.Value
                : runtimeStatus.Trim());

        DataAccess da = new DataAccess();
        da.GetDataSet("usp_Pms_QCWorkflowHistory_MarkActionTaken", prm);
    }
    private static void SaveLogText(string message)
    {
        string retval = "";
        DataAccess da = new DataAccess();
        SqlParameter[] prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@message", message);
        prm[1] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[1].Direction = ParameterDirection.ReturnValue;
        DataSet ds = da.GetDataSet("dbo.crm_entryLogTable", prm);
        retval = "1";
        //return retval;
    }
    private static void SaveQCUserActionLog(string message)
    {
        try
        {
            string directory = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Logs"
            );
            Directory.CreateDirectory(directory);

            string file = Path.Combine(
                directory,
                "QCUserWorkflow_" + DateTime.Now.ToString("yyyyMMdd") + ".log"
            );

            File.AppendAllText(
                file,
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                " | " + (message ?? "") + Environment.NewLine
            );
        }
        catch
        {
            // Logging must not stop workflow processing.
        }
    }
    #region update status
    
    public static string UpdateMilestone_Status(string milstoneid = "", string statusid = "", string iscomplete = "")
    {
        //if (!HasPmsEditPermission("54")) return "You do not have edit permission.";
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (milstoneid != "" && milstoneid != "0")
        {
            entrytype = "Update";
        }

        // percentage update start  // 
        if (iscomplete == "1")
        {
            string independency = "0";// checkGanttPredecessorCompletion("milestone", milstoneid);
            if (independency != "0")
            {
                retval = independency == "1" ? "Please complete first dependency of this? " : independency;
            }
            else
            {
                SqlParameter[] prm = new SqlParameter[8];
                prm[0] = new SqlParameter("@id", milstoneid);
                prm[1] = new SqlParameter("@milstoneStatus", statusid);
                prm[2] = new SqlParameter("@iscomplete", iscomplete);
                prm[3] = new SqlParameter("@iscompletedate", da1.getcdatetime());
                prm[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prm[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                prm[6] = new SqlParameter("@cdate", da1.getcdatetime());
                prm[7] = new SqlParameter("@type", entrytype);
                da1.executeDMLQuery("pmstbl_Status_milstone_Update_usp", prm);
                decimal milestonetotalpercent = 100;
                decimal totlweitagemile = 0;
                string finsql1 = "select * from Pms_tbl_AddMilestone where id=" + milstoneid + " and status=1  and archive=0";
                DataSet dsfindid1 = da1.GetDataSetInline(finsql1);

                // Milestone percentage //
                string strstatusupdate = "update Pms_tbl_AddMilestone set iscompletepercentage='" + milestonetotalpercent.ToString() + "' where id='" + milstoneid + "' and status=1 and archive=0 and isnull(iscomplete,0)=1";
                DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                // project percentage //
                string finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1  and archive=0";
                DataSet dsfindid2 = da1.GetDataSetInline(finsql2);
                for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
                {
                    decimal sumweitage = 0;
                    decimal subpercentageemile = 0;
                    sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
                    subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                    totlweitagemile += (sumweitage * subpercentageemile) / 100;

                }

                string finsql4 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitagemile.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
                DataSet dsfindid4 = da1.GetDataSetInline(finsql4);

                //project  log details insert start
                SqlParameter[] prmlog = new SqlParameter[8];
                prmlog[0] = new SqlParameter("@Transid", milstoneid);
                prmlog[1] = new SqlParameter("@Logstatus", statusid);
                prmlog[2] = new SqlParameter("@LogDate", da1.getcdatetime());
                prmlog[3] = new SqlParameter("@Logtype", "milestone");
                prmlog[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prmlog[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                prmlog[6] = new SqlParameter("@cdate", da1.getcdatetime());
                prmlog[7] = new SqlParameter("@type", "Insert");
                da1.executeDMLQuery("pmstbl_Loginsert_usp", prmlog);
                //project  log details insert end
                retval = "1";
                //DataAccess da = new DataAccess();
                //string strsql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus where  status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + statusid + "' ";
                //DataSet dscode = da.GetDataSetInline(strsql);
                //if (dscode.Tables[0].Rows.Count > 0)
                //{
                //    retval = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
                //}
            }


            //string chkval = managecalculation(milstoneid, "milestone");

        }
        else
        {
            SqlParameter[] prm = new SqlParameter[8];
            prm[0] = new SqlParameter("@id", milstoneid);
            prm[1] = new SqlParameter("@milstoneStatus", statusid);
            prm[2] = new SqlParameter("@iscomplete", iscomplete);
            prm[3] = new SqlParameter("@iscompletedate", da1.getcdatetime());
            prm[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prm[6] = new SqlParameter("@cdate", da1.getcdatetime());
            prm[7] = new SqlParameter("@type", entrytype);
            da1.executeDMLQuery("pmstbl_Status_milstone_Update_usp", prm);
            decimal milestonetotalpercent = 0;
            decimal totlweitagemile = 0;
            string finsql1 = "select * from Pms_tbl_AddMilestone where id=" + milstoneid + " and status=1  and archive=0";
            DataSet dsfindid1 = da1.GetDataSetInline(finsql1);

            // Milestone percentage //
            string strstatusupdate = "update Pms_tbl_AddMilestone set iscompletepercentage='" + milestonetotalpercent.ToString() + "' where id='" + milstoneid + "' and status=1 and archive=0 and isnull(iscomplete,0)=0";
            DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

            if (dsfindid1.Tables[0].Rows[0]["istask"].ToString() == "1")
            {

                string sqlCW = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where  Milestoneid='" + milstoneid + "'  and status=1  and archive=0 ";
                DataSet dsCW = da1.GetDataSetInline(sqlCW);
                for (int k = 0; k < dsCW.Tables[0].Rows.Count; k++)
                {
                    decimal sumweitage = 0;
                    decimal subpercentageemile = 0;
                    sumweitage = Convert.ToDecimal(dsCW.Tables[0].Rows[k]["Weightage"].ToString());
                    subpercentageemile = Convert.ToDecimal(dsCW.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                    totlweitagemile += (sumweitage * subpercentageemile) / 100;

                }
                string strupdate = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile.ToString() + "' where id='" + milstoneid + "' and status=1 and archive=0 ";
                SaveLogText("updatestatusA=" + strupdate);
                da1.executeDMLQueryinline(strupdate);

            }
            // project percentage //
            string finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1  and archive=0";
            DataSet dsfindid2 = da1.GetDataSetInline(finsql2);
            for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
            {
                decimal sumweitage = 0;
                decimal subpercentageemile = 0;
                sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
                subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                totlweitagemile += (sumweitage * subpercentageemile) / 100;

            }

            string finsql4 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitagemile.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da1.GetDataSetInline(finsql4);
            //project  log details insert start
            SqlParameter[] prmlog = new SqlParameter[8];
            prmlog[0] = new SqlParameter("@Transid", milstoneid);
            prmlog[1] = new SqlParameter("@Logstatus", statusid);
            prmlog[2] = new SqlParameter("@LogDate", da1.getcdatetime());
            prmlog[3] = new SqlParameter("@Logtype", "milestone");
            prmlog[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prmlog[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prmlog[6] = new SqlParameter("@cdate", da1.getcdatetime());
            prmlog[7] = new SqlParameter("@type", "Insert");
            da1.executeDMLQuery("pmstbl_Loginsert_usp", prmlog);
            //project  log details insert end
            retval = "1";



        }
        // percentage update end  // 





        // return dscode.GetXml();
        return retval;
    }
    
    public static string UpdateTask_Status(string taskid = "", string statusid = "", string iscomplete = "")
    {
        //if (!HasPmsEditPermission("55")) return "You do not have edit permission.";
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (taskid != "" && taskid != "0")
        {
            entrytype = "Update";
        }




        // percentage update start  // 
        if (iscomplete == "1")
        {
            string independency = "0";// checkGanttPredecessorCompletion("task", taskid);
            if (independency != "0")
            {
                retval = independency == "1" ? "Please complete first dependency of this? " : independency;
            }
            else
            {
                SqlParameter[] prm = new SqlParameter[8];
                prm[0] = new SqlParameter("@id", taskid);
                prm[1] = new SqlParameter("@ProjectStatus", statusid);
                prm[2] = new SqlParameter("@iscomplete", iscomplete);
                prm[3] = new SqlParameter("@iscompletedate", da1.getcdatetime());
                prm[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prm[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                prm[6] = new SqlParameter("@cdate", da1.getcdatetime());
                prm[7] = new SqlParameter("@type", entrytype);
                da1.executeDMLQuery("pmstbl_task_Status_DeatilsUpdate", prm);
                decimal tsktotalpercent = 100;
                decimal totlweitagemile = 0;
                decimal totlweitageproject = 0;
                string finsql1 = "select * from Pms_tbl_Addtask where id=" + taskid + " and status=1 and archive=0";
                DataSet dsfindid1 = da1.GetDataSetInline(finsql1);

                // task percentage //
                string strstatusupdate = "update Pms_tbl_Addtask set iscompletepercentage='" + tsktotalpercent.ToString() + "' where id='" + taskid + "' and status=1 and archive=0 and isnull(iscomplete,0)=1";
                DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                // milestone percentage //
                string finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'   and status=1  and archive=0";
                DataSet dsfindid2 = da1.GetDataSetInline(finsql2);
                for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
                {
                    decimal sumweitage = 0;
                    decimal subpercentageemile = 0;
                    sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
                    subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                    totlweitagemile += (sumweitage * subpercentageemile) / 100;

                }

                string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile.ToString() + "' where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and id='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1 and archive=0";
                DataSet dsfindid4 = da1.GetDataSetInline(finsql4);

                // project percentage //

                string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1  and archive=0";
                DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
                for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
                {
                    decimal sumweitageprojt = 0;
                    decimal subpercentageeproject = 0;
                    sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                    subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                    totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

                }

                string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
                DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);



                //project  log details insert start
                SqlParameter[] prmlog = new SqlParameter[8];
                prmlog[0] = new SqlParameter("@Transid", taskid);
                prmlog[1] = new SqlParameter("@Logstatus", statusid);
                prmlog[2] = new SqlParameter("@LogDate", da1.getcdatetime());
                prmlog[3] = new SqlParameter("@Logtype", "task");
                prmlog[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prmlog[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                prmlog[6] = new SqlParameter("@cdate", da1.getcdatetime());
                prmlog[7] = new SqlParameter("@type", "Insert");
                da1.executeDMLQuery("pmstbl_Loginsert_usp", prmlog);
                //project  log details insert end
                retval = "1";
                //string strsql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + statusid + "' ";
                //DataSet dscode = da1.GetDataSetInline(strsql);
                //if (dscode.Tables[0].Rows.Count > 0)
                //{
                //    retval = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
                //}
            }


        }
        else
        {
            SqlParameter[] prm = new SqlParameter[8];
            prm[0] = new SqlParameter("@id", taskid);
            prm[1] = new SqlParameter("@ProjectStatus", statusid);
            prm[2] = new SqlParameter("@iscomplete", iscomplete);
            prm[3] = new SqlParameter("@iscompletedate", da1.getcdatetime());
            prm[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prm[6] = new SqlParameter("@cdate", da1.getcdatetime());
            prm[7] = new SqlParameter("@type", entrytype);
            da1.executeDMLQuery("pmstbl_task_Status_DeatilsUpdate", prm);
            decimal tsktotalpercent = 0;
            decimal totlweitagemile = 0;
            decimal totlweitageproject = 0;

            string finsql1 = "select * from Pms_tbl_Addtask where id=" + taskid + " and status=1  and archive=0";
            DataSet dsfindid1 = da1.GetDataSetInline(finsql1);

            // task percentage //
            if (dsfindid1.Tables[0].Rows.Count > 0)
            {
                if (dsfindid1.Tables[0].Rows[0]["issubtask"].ToString() == "1")
                {

                    string finsql211 = "select isnull(sum(weightage),0)Wattagetasksum from Pms_tbl_Addsubtask where   task='" + taskid + "'  and status=1 and isnull(iscomplete,0)=1 and archive=0";
                    DataSet dsfindid211 = da1.GetDataSetInline(finsql211);
                    if (dsfindid211.Tables[0].Rows.Count > 0)
                    {
                        string strstatusupdate = "update Pms_tbl_Addtask set iscompletepercentage='" + dsfindid211.Tables[0].Rows[0]["Wattagetasksum"] + "' where id='" + taskid + "' and status=1 and archive=0";
                        DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);
                    }
                }
                else
                {
                    string strstatusupdate = "update Pms_tbl_Addtask set iscompletepercentage='0' where id='" + taskid + "' and status=1 and archive=0";
                    DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                }

            }


            // milestone percentage //

            string finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1  and archive=0";
            DataSet dsfindid2 = da1.GetDataSetInline(finsql2);
            for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
            {
                decimal sumweitage = 0;
                decimal subpercentageemile = 0;
                sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
                subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                totlweitagemile += (sumweitage * subpercentageemile) / 100;

            }

            string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile + "' where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and id='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da1.GetDataSetInline(finsql4);

            // project percentage //

            string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'   and status=1  and archive=0";
            DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
            for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
            {
                decimal sumweitageprojt = 0;
                decimal subpercentageeproject = 0;
                sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

            }

            string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
            DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);



            //project  log details insert start
            SqlParameter[] prmlog = new SqlParameter[8];
            prmlog[0] = new SqlParameter("@Transid", taskid);
            prmlog[1] = new SqlParameter("@Logstatus", statusid);
            prmlog[2] = new SqlParameter("@LogDate", da1.getcdatetime());
            prmlog[3] = new SqlParameter("@Logtype", "task");
            prmlog[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prmlog[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prmlog[6] = new SqlParameter("@cdate", da1.getcdatetime());
            prmlog[7] = new SqlParameter("@type", "Insert");
            da1.executeDMLQuery("pmstbl_Loginsert_usp", prmlog);
            //project  log details insert end
            retval = "1";
            //string strsql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + statusid + "' ";
            //DataSet dscode = da1.GetDataSetInline(strsql);
            //if (dscode.Tables[0].Rows.Count > 0)
            //{
            //    retval = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
            //}

        }

        // percentage update end  // 




        return retval;
    }

    
    public static string UpdateSubtask_Status(string subtaskid = "", string statusid = "", string iscomplete = "")
    {
       
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";

        if (iscomplete == "1")
        {
            string independency = "0";// checkGanttPredecessorCompletion("subtask", subtaskid);
            if (independency != "0")
            {
                retval = independency == "1" ? "Please complete first dependency of this? " : independency;
            }
            else
            {

                SqlParameter[] prm = new SqlParameter[8];
                prm[0] = new SqlParameter("@id", subtaskid);
                prm[1] = new SqlParameter("@ProjectStatus", statusid);
                prm[2] = new SqlParameter("@iscomplete", iscomplete);
                prm[3] = new SqlParameter("@iscompletedate", da1.getcdatetime());
                prm[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prm[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                prm[6] = new SqlParameter("@cdate", da1.getcdatetime());
                prm[7] = new SqlParameter("@type", entrytype);
                da1.executeDMLQuery("pmstbl_subtask_Status_DeatilsUpdate", prm);


                decimal subtsktotalpercent = 100;
                decimal totlweitagemile = 0;
                decimal totlweitageproject = 0;

                string finsql1 = "select * from Pms_tbl_Addsubtask where id=" + subtaskid + " and status=1  and archive=0";
                DataSet dsfindid1 = da1.GetDataSetInline(finsql1);
                SaveLogText("finsql2-" + finsql1);
                string finsql2 = "select isnull(sum(weightage),0)Wattagetasksum from Pms_tbl_Addsubtask where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "' and task='" + dsfindid1.Tables[0].Rows[0]["task"] + "'  and status=1 and  archive=0 and isnull(iscomplete,0)=1";

                DataSet dsfindid2 = da1.GetDataSetInline(finsql2);

                //sub task percentage //
                string strstatusupdate = "update Pms_tbl_Addsubtask set iscompletepercentage='" + subtsktotalpercent.ToString() + "' where id='" + subtaskid + "' and status=1 and archive=0 and isnull(iscomplete,0)=1";
                DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                // task percentage //
                string finsql3 = "update Pms_tbl_Addtask set iscompletepercentage='" + dsfindid2.Tables[0].Rows[0]["Wattagetasksum"] + "' where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "' and id='" + dsfindid1.Tables[0].Rows[0]["task"] + "' and status=1 and archive=0";
                DataSet dsfindid3 = da1.GetDataSetInline(finsql3);


                // Milestone percentage //
                string findsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1  and archive=0";
                DataSet dsfindidmile = da1.GetDataSetInline(findsqlmile);
                for (int j = 0; j < dsfindidmile.Tables[0].Rows.Count; j++)
                {
                    decimal sumweitage = 0;
                    decimal subpercentageemile = 0;
                    sumweitage = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["Weightage"].ToString());
                    subpercentageemile = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                    totlweitagemile += (sumweitage * subpercentageemile) / 100;

                }

                string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile + "' where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and id='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1 and archive=0";
                DataSet dsfindid4 = da1.GetDataSetInline(finsql4);

                // project percentage //
                string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'   and status=1  and archive=0";
                DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
                for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
                {
                    decimal sumweitageprojt = 0;
                    decimal subpercentageeproject = 0;
                    sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                    subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                    totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

                }

                string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'   and status=1 and archive=0";
                DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);
                //project  log details insert start
                SqlParameter[] prmlog = new SqlParameter[8];
                prmlog[0] = new SqlParameter("@Transid", subtaskid);
                prmlog[1] = new SqlParameter("@Logstatus", statusid);
                prmlog[2] = new SqlParameter("@LogDate", da1.getcdatetime());
                prmlog[3] = new SqlParameter("@Logtype", "subtask");
                prmlog[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prmlog[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                prmlog[6] = new SqlParameter("@cdate", da1.getcdatetime());
                prmlog[7] = new SqlParameter("@type", "Insert");
                da1.executeDMLQuery("pmstbl_Loginsert_usp", prmlog);
                //project  log details insert end


                retval = "1";

                
            }


        }
        else
        {
            decimal subtsktotalpercent = 0;
            decimal totlweitagemile = 0;
            decimal totlweitageproject = 0;
            SqlParameter[] prm = new SqlParameter[8];
            prm[0] = new SqlParameter("@id", subtaskid);
            prm[1] = new SqlParameter("@ProjectStatus", statusid);
            prm[2] = new SqlParameter("@iscomplete", iscomplete);
            prm[3] = new SqlParameter("@iscompletedate", da1.getcdatetime());
            prm[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prm[6] = new SqlParameter("@cdate", da1.getcdatetime());
            prm[7] = new SqlParameter("@type", entrytype);
            da1.executeDMLQuery("pmstbl_subtask_Status_DeatilsUpdate", prm);
            string finsql1 = "select * from Pms_tbl_Addsubtask where id=" + subtaskid + " and status=1  and archive=0";
            DataSet dsfindid1 = da1.GetDataSetInline(finsql1);

            string finsql2 = "select isnull(sum(weightage),0)Wattagetasksum from Pms_tbl_Addsubtask where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "' and task='" + dsfindid1.Tables[0].Rows[0]["task"] + "'  and status=1 and isnull(iscomplete,0)=1 and archive=0";
            DataSet dsfindid2 = da1.GetDataSetInline(finsql2);

            //sub task percentage //
            string strstatusupdate = "update Pms_tbl_Addsubtask set iscompletepercentage='" + subtsktotalpercent.ToString() + "' where id='" + subtaskid + "' and status=1 and archive=0";
            DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

            //task percentage //
            string finsql3 = "update Pms_tbl_Addtask set iscompletepercentage='" + dsfindid2.Tables[0].Rows[0]["Wattagetasksum"] + "' where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "' and id='" + dsfindid1.Tables[0].Rows[0]["task"] + "' and status=1 and archive=0";
            DataSet dsfindid3 = da1.GetDataSetInline(finsql3);

            //milestone percentage //
            string findsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'    and status=1  and archive=0";
            DataSet dsfindidmile = da1.GetDataSetInline(findsqlmile);
            for (int j = 0; j < dsfindidmile.Tables[0].Rows.Count; j++)
            {
                decimal sumweitage = 0;
                decimal subpercentageemile = 0;
                sumweitage = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["Weightage"].ToString());
                subpercentageemile = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                totlweitagemile += (sumweitage * subpercentageemile) / 100;

            }

            string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile + "' where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and id='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da1.GetDataSetInline(finsql4);


            // project percentage //
            string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'   and status=1  and archive=0";
            DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
            for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
            {
                decimal sumweitageprojt = 0;
                decimal subpercentageeproject = 0;
                sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

            }

            string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'   and status=1 and archive=0";
            DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);
            //project  log details insert start
            SqlParameter[] prmlog = new SqlParameter[8];
            prmlog[0] = new SqlParameter("@Transid", subtaskid);
            prmlog[1] = new SqlParameter("@Logstatus", statusid);
            prmlog[2] = new SqlParameter("@LogDate", da1.getcdatetime());
            prmlog[3] = new SqlParameter("@Logtype", "subtask");
            prmlog[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prmlog[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prmlog[6] = new SqlParameter("@cdate", da1.getcdatetime());
            prmlog[7] = new SqlParameter("@type", "Insert");
            da1.executeDMLQuery("pmstbl_Loginsert_usp", prmlog);
            //project  log details insert end
            retval = "1";



            //string strsql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + statusid + "' ";
            //DataSet dscode = da1.GetDataSetInline(strsql);
            //if (dscode.Tables[0].Rows.Count > 0)
            //{
            //    retval = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
            //}


        }
        // percentage update end  // 




        return retval;
    }
    private static bool HasPmsEditPermission(string moduleid)
    {
        try
        {
            string uid = "0";
            if (HttpContext.Current.Request.Cookies["user"]["user_id"] != null)
            {
                uid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();
            }
            string[] permission = get_EditDelete_Validation(moduleid, uid).Split(',');
            // Permission order: View, AllAccess, Create, Edit, Delete, Assign.
            return permission.Length > 3 && (permission[1] == "1" || permission[3] == "1");
        }
        catch
        {
            return false;
        }
    }
    public static string get_EditDelete_Validation(string moduleid, string userid)
    {
        int chkadmin = 0;
        int chkroleid = 0;
        string retval = "";
        string strFull = "0";
        string strView = "0";
        string strCreate = "0";
        string strEdit = "0";
        string strDelete = "0";
        string strChangeOwner = "0";
        string strAssign = "0";
        DataAccess da = new DataAccess();
        try
        {

            chkadmin = checkadmin(userid);

            if (chkadmin == 1)
            {

                strFull = "1";
                strView = "1";
                strCreate = "1";
                strEdit = "1";
                strDelete = "1";
                strChangeOwner = "1";
                strAssign = "1";
            }
            else
            {
                chkroleid = checkroleid(userid);
                string strsqlmodule = @"select crm_tbl_roletaskmaster.formname,crm_tbl_roletaskmaster.formurl, isnull(ismanage,0) ismanage,isnull(isschedule,0) isschedule,  isnull(ischangeowner,0) ischangeowner,
                            isnull(formaccess,0) formaccess, isnull(isinsert,0) isinsert, isnull(isupdate,0) isupdate, isnull(isdelete,0) isdelete, isnull(isview,0) isview, isnull(isimport,0) isimport, isnull(isexport,0) isexport, isnull(issendmail,0) issendmail, isnull(isviewmail,0) isviewmail, isnull(isdelmail,0) isdelmail, isnull(isfull,0) isfull, isnull(moduleid,0) moduleid,isnull(isassign,0) as isassign  
                                    from crm_tbl_roleaccessdetails inner join crm_tbl_roletaskmaster on crm_tbl_roleaccessdetails.moduleid=crm_tbl_roletaskmaster.id 
                                    where crm_tbl_roleaccessdetails.status=1 and  crm_tbl_roleaccessdetails.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and crm_tbl_roleaccessdetails.roleid='" + chkroleid + "' and entrytype='form' and moduleid=" + moduleid + "";
                //SaveLogText("strsqlmoduleA1-" + strsqlmodule);
                DataSet dsmodule = da.GetDataSetInline(strsqlmodule);
                if (dsmodule.Tables[0].Rows.Count > 0)
                {
                    if (dsmodule.Tables[0].Rows[0]["isfull"].ToString() == "1" || dsmodule.Tables[0].Rows[0]["isfull"].ToString() == "True")
                    {
                        strFull = "1";
                    }
                    if (strFull == "1")
                    {
                        strView = "1";
                        strCreate = "1";
                        strEdit = "1";
                        strDelete = "1";
                        strChangeOwner = "1";
                        strAssign = "1";
                    }
                    else
                    {
                        if (dsmodule.Tables[0].Rows[0]["isview"].ToString() == "1" || dsmodule.Tables[0].Rows[0]["isview"].ToString() == "True")
                        {
                            strView = "1";
                        }
                        if (dsmodule.Tables[0].Rows[0]["isinsert"].ToString() == "1" || dsmodule.Tables[0].Rows[0]["isinsert"].ToString() == "True")
                        {
                            strCreate = "1";
                        }
                        if (dsmodule.Tables[0].Rows[0]["isupdate"].ToString() == "1" || dsmodule.Tables[0].Rows[0]["isupdate"].ToString() == "True")
                        {
                            strEdit = "1";
                        }
                        if (dsmodule.Tables[0].Rows[0]["isdelete"].ToString() == "1" || dsmodule.Tables[0].Rows[0]["isdelete"].ToString() == "True")
                        {
                            strDelete = "1";
                        }
                        if (dsmodule.Tables[0].Rows[0]["ischangeowner"].ToString() == "1" || dsmodule.Tables[0].Rows[0]["ischangeowner"].ToString() == "True")
                        {
                            strChangeOwner = "1";
                        }
                        if (dsmodule.Tables[0].Rows[0]["isassign"].ToString() == "1" || dsmodule.Tables[0].Rows[0]["isassign"].ToString() == "True")
                        {
                            strAssign = "1";
                        }
                        if (strView == "0" && (strCreate == "1" || strEdit == "1" || strDelete == "1"))
                        {
                            strView = "1";
                        }

                    }

                }

            }


        }
        catch { }
        retval = strFull + "," + strView + "," + strCreate + "," + strEdit + "," + strDelete + strAssign + "," + strChangeOwner;
        return retval;
    }
    protected static int checkroleid(string struserid = "0")
    {
        int retval = 0;
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        function fns = new function();
        string strsql = "select Manage_Profile.* from Manage_Profile inner join tbl_employee on Manage_Profile.id=cast(tbl_employee.roleid as int) where tbl_employee.id='" + struserid + "'  and Manage_Profile.status=1 and Manage_Profile.archive=0 ";
        try
        {
            DataSet ds = da.GetDataSetInline(strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {


                retval = Convert.ToInt32(ds.Tables[0].Rows[0]["id"].ToString());


            }
        }
        catch { }
        return retval;
    }
    protected static double get_completionstatus(string id, string type)
    {
        double strret = 0;
        string tblname = "Pms_tbl_AddMilestone";
        if (type == "task")
        {
            tblname = "Pms_tbl_Addtask";
        }
        else if (type == "subtask")
        {
            tblname = "Pms_tbl_Addsubtask";
        }
        if (id == "" || id == "0")
        {
            return 0;
        }
        DataAccess da = new DataAccess();

        double mileTaskTotal = 0;
        string findmile = "select cast(ROUND(isnull(iscompletepercentage,0),2) AS DECIMAL(10,2)) as iscompletepercentage from " + tblname + "  where id='" + id + "'  and status=1  and archive=0";
        SaveLogText("chkadmintodayEX-" + findmile);
        DataSet dsssummile = da.GetDataSetInline(findmile);

        try
        {
            mileTaskTotal = Convert.ToDouble(dsssummile.Tables[0].Rows[0]["iscompletepercentage"]);
            if (mileTaskTotal == 99.99)
            {
                mileTaskTotal = 100;
            }
        }
        catch (Exception)
        {
        }
        if (mileTaskTotal > 0)
        {
            strret = mileTaskTotal;

        }
        return strret;

    }
    public static string checkGanttPredecessorCompletion(string strtype, string strid)
    {
        int regid;
        int itemId;
        HttpCookie accountCookie = HttpContext.Current.Request.Cookies["cadmin"];
        if (accountCookie == null || !Int32.TryParse(accountCookie["id"], out regid) || regid <= 0 ||
            !Int32.TryParse(strid, out itemId) || itemId <= 0)
            return "Could not validate dependencies. Please refresh and try again.";

        string itemType = (strtype ?? "").Trim().ToLowerInvariant();
        string entityTable = "";
        if (itemType == "milestone") entityTable = "Pms_tbl_AddMilestone";
        else if (itemType == "task") entityTable = "Pms_tbl_AddTask";
        else if (itemType == "subtask") entityTable = "Pms_tbl_AddSubtask";
        else return "Could not validate dependencies for this item.";

        try
        {
            DataAccess da = new DataAccess();
            string projectSql = "select top 1 ProjectId from " + entityTable +
                " where regid=" + regid + " and id=" + itemId + " and status=1 and archive=0";
            DataSet projectData = da.GetDataSetInline(projectSql);
            if (projectData == null || projectData.Tables.Count == 0 || projectData.Tables[0].Rows.Count == 0)
                return "Could not find this item to validate its dependencies.";

            int projectId;
            if (!Int32.TryParse(Convert.ToString(projectData.Tables[0].Rows[0]["ProjectId"]), out projectId) || projectId <= 0)
                return "Could not validate dependencies for this project item.";

            // Resolve all predecessor rows in one query. The previous implementation
            // queried each linked predecessor separately, making completion slow for
            // items with several dependencies and returning only a numeric ID to List.
            string linksSql = "select top 1 d.PredecessorType,d.PredecessorId,d.LinkType," +
                "case when lower(d.PredecessorType)='milestone' then isnull(convert(nvarchar(100),pm.orderno),'') " +
                "when lower(d.PredecessorType)='task' then isnull(convert(nvarchar(100),ptm.orderno),'')+'.'+isnull(convert(nvarchar(100),pt.orderno),'') " +
                "when lower(d.PredecessorType)='subtask' then isnull(convert(nvarchar(100),psm.orderno),'')+'.'+isnull(convert(nvarchar(100),pst.orderno),'')+'.'+isnull(convert(nvarchar(100),ps.orderno),'') else '' end as PredecessorSerial," +
                "case when lower(d.PredecessorType)='milestone' then isnull(convert(nvarchar(500),pm.Milestone),'') " +
                "when lower(d.PredecessorType)='task' then isnull(convert(nvarchar(500),pt.taskname),'') " +
                "when lower(d.PredecessorType)='subtask' then isnull(convert(nvarchar(500),ps.Subtaskname),'') else '' end as PredecessorName " +
                "from dbo.Pms_tbl_GanttDependency d " +
                "left join dbo.Pms_tbl_AddMilestone pm on lower(d.PredecessorType)='milestone' and pm.regid=" + regid + " and pm.ProjectId=" + projectId + " and pm.id=d.PredecessorId and pm.status=1 and pm.archive=0 " +
                "left join dbo.Pms_tbl_AddTask pt on lower(d.PredecessorType)='task' and pt.regid=" + regid + " and pt.ProjectId=" + projectId + " and pt.id=d.PredecessorId and pt.status=1 and pt.archive=0 " +
                "left join dbo.Pms_tbl_AddMilestone ptm on lower(d.PredecessorType)='task' and ptm.regid=" + regid + " and ptm.ProjectId=" + projectId + " and ptm.id=pt.Milestoneid and ptm.status=1 and ptm.archive=0 " +
                "left join dbo.Pms_tbl_AddSubtask ps on lower(d.PredecessorType)='subtask' and ps.regid=" + regid + " and ps.ProjectId=" + projectId + " and ps.id=d.PredecessorId and ps.status=1 and ps.archive=0 " +
                "left join dbo.Pms_tbl_AddTask pst on lower(d.PredecessorType)='subtask' and pst.regid=" + regid + " and pst.ProjectId=" + projectId + " and pst.id=ps.Task and pst.status=1 and pst.archive=0 " +
                "left join dbo.Pms_tbl_AddMilestone psm on lower(d.PredecessorType)='subtask' and psm.regid=" + regid + " and psm.ProjectId=" + projectId + " and psm.id=ps.Milestoneid and psm.status=1 and psm.archive=0 " +
                "where d.RegId=" + regid + " and d.ProjectId=" + projectId +
                " and lower(d.SuccessorType)='" + itemType + "' and d.SuccessorId=" + itemId +
                " and upper(isnull(d.LinkType,'')) in ('FS','FF') " +
                "and (lower(d.PredecessorType) not in ('milestone','task','subtask') " +
                "or (lower(d.PredecessorType)='milestone' and isnull(pm.statuscomplete,0)<>1) " +
                "or (lower(d.PredecessorType)='task' and isnull(pt.statuscomplete,0)<>1) " +
                "or (lower(d.PredecessorType)='subtask' and isnull(ps.statuscomplete,0)<>1)) order by d.Id";
            DataSet linksData = da.GetDataSetInline(linksSql);
            if (linksData == null || linksData.Tables.Count == 0 || linksData.Tables[0].Rows.Count == 0)
                return "0";

            DataRow link = linksData.Tables[0].Rows[0];
            string predecessorType = Convert.ToString(link["PredecessorType"]).Trim().ToLowerInvariant();
            int predecessorId;
            if (!Int32.TryParse(Convert.ToString(link["PredecessorId"]), out predecessorId) || predecessorId <= 0)
                return "Could not validate a linked predecessor. Please refresh and try again.";
            if (predecessorType != "milestone" && predecessorType != "task" && predecessorType != "subtask")
                return "Could not validate a linked predecessor of type '" + predecessorType + "'.";

            string predecessorSerial = HttpUtility.UrlEncode(Convert.ToString(link["PredecessorSerial"]), Encoding.UTF8);
            string predecessorName = HttpUtility.UrlEncode(Convert.ToString(link["PredecessorName"]), Encoding.UTF8);
            return "GANTT_DEPENDENCY_BLOCKED|" + predecessorType + "|" + predecessorId + "|" +
                Convert.ToString(link["LinkType"]) + "|" + predecessorSerial + "|" + predecessorName;
        }
        catch (Exception ex)
        {
            SaveLogText("Gantt predecessor completion validation: " + ex.Message);
            return "Could not validate dependencies. Please refresh and try again.";
        }

        return "0";
    }

    public class item_summaryN
    {
        public string status { get; set; }
        public double? copmelete_value { get; set; }
        public double? pending_value { get; set; }

    }
    #endregion
    public class QCActionProxyResponse
    {
        public int Result { get; set; }
        public string Message { get; set; }

        public QCActionProxyResponse()
        {
            Message = "";
        }

        public static QCActionProxyResponse Fail(string message)
        {
            QCActionProxyResponse response = new QCActionProxyResponse();
            response.Result = 0;
            response.Message = message ?? "QC action could not be completed.";
            return response;
        }
    }

    // Response returned by api/manageqcworkflows/manageuserworkflows.
    private class QCUserWorkflowServiceResponse
    {
        public int Result { get; set; }
        public string Message { get; set; }
        public int RuntimeId { get; set; }
        public string EntityType { get; set; }
        public int EntityId { get; set; }
        public int ActionId { get; set; }
        public string ActionName { get; set; }
        public string NotificationStatus { get; set; }
        public string RuntimeStatus { get; set; }
        public int PreviousStatusId { get; set; }
        public int NextStatusId { get; set; }
    }

    private static object DbText(string value)
    {
        return String.IsNullOrWhiteSpace(value) ? (object)DBNull.Value : value.Trim();
    }

    private static string DefaultIfBlank(string value, string defaultValue)
    {
        return String.IsNullOrWhiteSpace(value) ? defaultValue : value.Trim();
    }

    private static object DbDate(string value)
    {
        if (String.IsNullOrWhiteSpace(value)) return DBNull.Value;

        DateTime date;
        if (DateTime.TryParseExact(value.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture,
            DateTimeStyles.None, out date))
            return date.Date;

        return DBNull.Value;
    }

    private static string ReadString(DataRow row, string column)
    {
        if (!row.Table.Columns.Contains(column) || row[column] == DBNull.Value) return "";
        return Convert.ToString(row[column], CultureInfo.InvariantCulture) ?? "";
    }

    private static int ReadInt(DataRow row, string column)
    {
        int value;
        return Int32.TryParse(ReadString(row, column), out value) ? value : 0;
    }

    private static int? ReadNullableInt(DataRow row, string column)
    {
        if (!row.Table.Columns.Contains(column) || row[column] == DBNull.Value) return null;
        int value;
        return Int32.TryParse(Convert.ToString(row[column], CultureInfo.InvariantCulture), out value)
            ? (int?)value
            : null;
    }

    private static bool ReadBool(DataRow row, string column)
    {
        if (!row.Table.Columns.Contains(column) || row[column] == DBNull.Value) return false;
        bool b;
        if (Boolean.TryParse(Convert.ToString(row[column], CultureInfo.InvariantCulture), out b)) return b;
        return ReadInt(row, column) == 1;
    }

    private static string ReadDateText(DataRow row, string column)
    {
        if (!row.Table.Columns.Contains(column) || row[column] == DBNull.Value) return "";
        DateTime value;
        if (!DateTime.TryParse(Convert.ToString(row[column], CultureInfo.InvariantCulture), out value)) return "";
        return value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
    }

    private static string ReadActionDateText(DataRow row, string column)
    {
        if (!row.Table.Columns.Contains(column) || row[column] == DBNull.Value) return "";
        DateTime value;
        return DateTime.TryParse(Convert.ToString(row[column], CultureInfo.InvariantCulture), out value)
            ? value.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture) : "";
    }

    public class QCGridFilterRequest
    {
        public string EntityType { get; set; }
        public string SearchText { get; set; }
        public string DateFilter { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string QCStatus { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class QCGridResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int IsAdmin { get; set; }
        public int TotalRows { get; set; }
        public List<QCGridRow> Rows { get; set; }

        public QCGridResponse()
        {
            Message = "";
            Rows = new List<QCGridRow>();
        }
    }

    public class QCGridRow
    {
        public int RuntimeId { get; set; }
        public int WorkflowId { get; set; }
        public string WorkflowName { get; set; }
        public int BranchNo { get; set; }
        public string BranchType { get; set; }
        public string EntityType { get; set; }
        public int EntityId { get; set; }
        public string EntityName { get; set; }
        public string HierarchySrNo { get; set; }
        public string ProjectName { get; set; }
        public int CycleNo { get; set; }
        public string RuntimeStatus { get; set; }
        public string QCStatus { get; set; }
        public string QCDate { get; set; }
        public string NotifiedTo { get; set; }
        public int? ActionTakenBy { get; set; }
        public string ActionTakenByName { get; set; }
        public int? TakenActionId { get; set; }
        public string TakenActionName { get; set; }
        public string ActionTakenDate { get; set; }
        public string ActionRemark { get; set; }
        public List<QCGridRow> PendingRuntimes { get; set; }
        public List<QCGridRow> History { get; set; }
        public List<QCActionOption> Actions { get; set; }

        public QCGridRow()
        {
            Actions = new List<QCActionOption>();
        }
    }

    public class QCActionOption
    {
        public int WorkflowUserActionId { get; set; }
        public int ActionId { get; set; }
        public string ActionName { get; set; }
        public int? NextStatusId { get; set; }
        public bool IsRemarkRequired { get; set; }
        public int SequenceNo { get; set; }
    }
}
