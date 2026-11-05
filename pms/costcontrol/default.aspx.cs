using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;

/// <summary>
/// PMS Cost Control: cost codes, versioned budgets with maker-checker approval, budget movements
/// (transfers, reserves, corrections, approved changes), change register with at-risk work,
/// cost entries with split allocation, estimate to complete, budget-vs-actual (live and monthly
/// snapshots), project summary, monthly period lock, pending approvals, audit history and access.
/// All data access goes through the parameterised usp_pms_cc_* procedures
/// (pms/costcontrol/sql/costcontrol_phase1.sql, phase2.sql, phase3.sql).
/// Access rules (rights and per-project access) live in App_Code/PmsCostControl.cs.
/// </summary>
public partial class pms_costcontrol_default : Page
{
    public int pageRegId;
    public int pageEmployeeId;
    public int isadmin;
    public int canEnter;
    public int canApprove;

    protected void Page_Load(object sender, EventArgs e)
    {
        PmsCostControl.Context login = PmsCostControl.Current();
        pageRegId = login.RegId;
        pageEmployeeId = login.EmployeeId;
        isadmin = login.IsAdmin ? 1 : 0;
        canEnter = login.Allows(PmsCostControl.Need.Enter) ? 1 : 0;
        canApprove = login.Allows(PmsCostControl.Need.Approve) ? 1 : 0;
        if (!login.Allows(PmsCostControl.Need.View)) Response.Redirect("/pms/accessdenied.aspx");
    }

    #region Helpers

    private static PmsCostControl.Context Require(PmsCostControl.Need need) { return PmsCostControl.Require(need); }

    /// <summary>Right + access to the project.</summary>
    private static PmsCostControl.Context RequireOnProject(PmsCostControl.Need need, int projectId)
    {
        var login = Require(need);
        PmsCostControl.RequireProject(login, projectId);
        return login;
    }

    /// <summary>Right + access to the project that owns the record.</summary>
    private static PmsCostControl.Context RequireOnRecord(PmsCostControl.Need need, string entityType, int id)
    {
        var login = Require(need);
        PmsCostControl.RequireRecordProject(login, entityType, id);
        return login;
    }

    private static DataSet Run(string procedureName, params SqlParameter[] parameters) { return PmsCostControl.RunProcedure(procedureName, parameters); }
    private static List<Dictionary<string, object>> Rows(DataSet ds, int index) { return PmsCostControl.ToRows(ds, index); }

    /// <summary>
    /// Reads the Result / Message / id row returned by every save procedure.
    /// Result 2 means "needs confirmation" (for example a duplicate reference).
    /// </summary>
    private static object SaveResult(DataSet ds)
    {
        if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            return new { Success = false, Message = "No response from the database." };
        DataTable table = ds.Tables[0];
        DataRow row = table.Rows[0];
        int result = Convert.ToInt32(row["Result"], CultureInfo.InvariantCulture);
        int id = table.Columns.Contains("id") && row["id"] != DBNull.Value ? Convert.ToInt32(row["id"], CultureInfo.InvariantCulture) : 0;
        bool alreadyExists = table.Columns.Contains("AlreadyExists") && row["AlreadyExists"] != DBNull.Value && Convert.ToInt32(row["AlreadyExists"], CultureInfo.InvariantCulture) == 1;
        return new { Success = result == 1, NeedsConfirm = result == 2, Message = Convert.ToString(row["Message"], CultureInfo.InvariantCulture), Id = id, AlreadyExists = alreadyExists };
    }

    private static object Failure(Exception ex) { return new { Success = false, Message = ex.Message }; }

    private static SqlParameter Reg(PmsCostControl.Context login) { return new SqlParameter("@regid", login.RegId); }
    private static SqlParameter Uid(PmsCostControl.Context login) { return new SqlParameter("@uid", login.EmployeeId); }
    private static SqlParameter Project(int projectId) { return new SqlParameter("@ProjectId", projectId); }

    #endregion

    #region Lookups

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetLookups()
    {
        try
        {
            var login = Require(PmsCostControl.Need.View);
            DataSet projects = Run("usp_pms_cc_Project_List", Reg(login), Uid(login), new SqlParameter("@IsAdmin", login.IsAdmin));
            DataSet costCodes = Run("usp_pms_cc_CostCode_List", Reg(login));
            DataSet units = Run("usp_pms_cc_Unit_List", Reg(login));
            return new
            {
                Success = true,
                Projects = Rows(projects, 0),
                CostCodes = Rows(costCodes, 0),
                Units = Rows(units, 0),
                Access = new { IsAdmin = login.IsAdmin, CanEnter = login.Allows(PmsCostControl.Need.Enter), CanApprove = login.CanApprove, EmployeeId = login.EmployeeId }
            };
        }
        catch (Exception ex) { return Failure(ex); }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetProjectStructure(int projectId)
    {
        try
        {
            var login = RequireOnProject(PmsCostControl.Need.View, projectId);
            DataSet ds = Run("usp_pms_cc_ProjectStructure", Reg(login), Project(projectId));
            return new { Success = true, Milestones = Rows(ds, 0), Tasks = Rows(ds, 1) };
        }
        catch (Exception ex) { return Failure(ex); }
    }

    /// <summary>Adds a unit to the existing unit master through the existing ERP procedure (duplicates by name are refused).</summary>
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object AddUnit(string unitName)
    {
        try
        {
            var login = Require(PmsCostControl.Need.Enter);
            return SaveResult(Run("usp_pms_cc_Unit_Add", Reg(login), new SqlParameter("@UnitName", unitName ?? "")));
        }
        catch (Exception ex) { return Failure(ex); }
    }

    #endregion

    #region Cost codes (administrators)

    public class CostCodeRequest
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string CostType { get; set; }
        public string Description { get; set; }
        public string CodeLevel { get; set; }
        public int? ParentId { get; set; }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object SaveCostCode(CostCodeRequest request)
    {
        try
        {
            var login = Require(PmsCostControl.Need.Admin);
            if (request == null) throw new ArgumentException("Nothing to save.");
            return SaveResult(Run("usp_pms_cc_CostCode_Save",
                Reg(login),
                new SqlParameter("@id", request.Id),
                new SqlParameter("@Code", request.Code ?? ""),
                new SqlParameter("@Name", request.Name ?? ""),
                new SqlParameter("@CostType", request.CostType ?? ""),
                new SqlParameter("@Description", PmsCostControl.DbText(request.Description)),
                Uid(login),
                new SqlParameter("@CodeLevel", String.IsNullOrWhiteSpace(request.CodeLevel) ? "Element" : request.CodeLevel),
                new SqlParameter("@ParentId", PmsCostControl.DbValue(request.ParentId))));
        }
        catch (Exception ex) { return Failure(ex); }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object DeleteCostCode(int costCodeId)
    {
        try
        {
            var login = Require(PmsCostControl.Need.Admin);
            return SaveResult(Run("usp_pms_cc_CostCode_Delete", Reg(login), new SqlParameter("@id", costCodeId), Uid(login)));
        }
        catch (Exception ex) { return Failure(ex); }
    }

    #endregion

    #region Budget versions

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetBudget(int projectId, int versionId)
    {
        try
        {
            var login = RequireOnProject(PmsCostControl.Need.View, projectId);
            DataSet ds = Run("usp_pms_cc_Budget_Get", Reg(login), Project(projectId),
                new SqlParameter("@VersionId", PmsCostControl.DbValue((int?)versionId)));
            return new { Success = true, Versions = Rows(ds, 0), Lines = Rows(ds, 1) };
        }
        catch (Exception ex) { return Failure(ex); }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object CreateBudgetVersion(int projectId, string reason)
    {
        try
        {
            var login = RequireOnProject(PmsCostControl.Need.Enter, projectId);
            return SaveResult(Run("usp_pms_cc_BudgetVersion_Create", Reg(login), Project(projectId),
                new SqlParameter("@Reason", PmsCostControl.DbText(reason)), Uid(login)));
        }
        catch (Exception ex) { return Failure(ex); }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object SubmitBudgetVersion(int versionId)
    {
        try
        {
            var login = RequireOnRecord(PmsCostControl.Need.Enter, "BudgetVersion", versionId);
            return SaveResult(Run("usp_pms_cc_BudgetVersion_Submit", Reg(login), new SqlParameter("@VersionId", versionId), Uid(login)));
        }
        catch (Exception ex) { return Failure(ex); }
    }

    /// <summary>Approve and lock. The procedure also checks the approval right and refuses the submitter (AP-03).</summary>
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object LockBudgetVersion(int versionId)
    {
        try
        {
            var login = RequireOnRecord(PmsCostControl.Need.Approve, "BudgetVersion", versionId);
            return SaveResult(Run("usp_pms_cc_BudgetVersion_Lock", Reg(login), new SqlParameter("@VersionId", versionId), Uid(login)));
        }
        catch (Exception ex) { return Failure(ex); }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object RejectBudgetVersion(int versionId, string reason)
    {
        try
        {
            var login = RequireOnRecord(PmsCostControl.Need.Approve, "BudgetVersion", versionId);
            return SaveResult(Run("usp_pms_cc_BudgetVersion_Reject", Reg(login), new SqlParameter("@VersionId", versionId),
                new SqlParameter("@Reason", reason ?? ""), Uid(login)));
        }
        catch (Exception ex) { return Failure(ex); }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object DiscardDraftVersion(int versionId)
    {
        try
        {
            var login = RequireOnRecord(PmsCostControl.Need.Enter, "BudgetVersion", versionId);
            return SaveResult(Run("usp_pms_cc_BudgetVersion_DeleteDraft", Reg(login), new SqlParameter("@VersionId", versionId), Uid(login)));
        }
        catch (Exception ex) { return Failure(ex); }
    }

    public class BudgetLineRequest
    {
        public int Id { get; set; }
        public int VersionId { get; set; }
        public int? MilestoneId { get; set; }
        public int? TaskId { get; set; }
        public int CostCodeId { get; set; }
        public decimal? Quantity { get; set; }
        public int? UnitId { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Amount { get; set; }
        public string Remarks { get; set; }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object SaveBudgetLine(BudgetLineRequest request)
    {
        try
        {
            if (request == null) throw new ArgumentException("Nothing to save.");
            var login = RequireOnRecord(PmsCostControl.Need.Enter, "BudgetVersion", request.VersionId);
            return SaveResult(Run("usp_pms_cc_BudgetLine_Save",
                Reg(login),
                new SqlParameter("@id", request.Id),
                new SqlParameter("@VersionId", request.VersionId),
                new SqlParameter("@MilestoneId", PmsCostControl.DbValue(request.MilestoneId)),
                new SqlParameter("@TaskId", PmsCostControl.DbValue(request.TaskId)),
                new SqlParameter("@CostCodeId", request.CostCodeId),
                new SqlParameter("@Quantity", PmsCostControl.DbValue(request.Quantity)),
                new SqlParameter("@UnitId", PmsCostControl.DbValue(request.UnitId)),
                new SqlParameter("@Rate", PmsCostControl.DbValue(request.Rate)),
                new SqlParameter("@Amount", PmsCostControl.DbValue(request.Amount)),
                new SqlParameter("@Remarks", PmsCostControl.DbText(request.Remarks)),
                Uid(login)));
        }
        catch (Exception ex) { return Failure(ex); }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object DeleteBudgetLine(int budgetLineId)
    {
        try
        {
            var login = RequireOnRecord(PmsCostControl.Need.Enter, "BudgetLine", budgetLineId);
            return SaveResult(Run("usp_pms_cc_BudgetLine_Delete", Reg(login), new SqlParameter("@id", budgetLineId), Uid(login)));
        }
        catch (Exception ex) { return Failure(ex); }
    }

    #endregion

    #region Reserves and budget movements

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetMovements(int projectId)
    {
        try
        {
            var login = RequireOnProject(PmsCostControl.Need.View, projectId);
            DataSet movements = Run("usp_pms_cc_Movement_List", Reg(login), Project(projectId));
            DataSet reserves = Run("usp_pms_cc_Reserve_Report", Reg(login), Project(projectId));
            return new { Success = true, Movements = Rows(movements, 0), Lines = Rows(movements, 1), Reserves = Rows(reserves, 0) };
        }
        catch (Exception ex) { return Failure(ex); }
    }

    public class ReserveRequest
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string ReserveType { get; set; }
        public string Name { get; set; }
        public string Remarks { get; set; }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object SaveReserve(ReserveRequest request)
    {
        try
        {
            if (request == null) throw new ArgumentException("Nothing to save.");
            var login = RequireOnProject(PmsCostControl.Need.Enter, request.ProjectId);
            return SaveResult(Run("usp_pms_cc_Reserve_Save", Reg(login), new SqlParameter("@id", request.Id), Project(request.ProjectId),
                new SqlParameter("@ReserveType", request.ReserveType ?? ""), new SqlParameter("@Name", request.Name ?? ""),
                new SqlParameter("@Remarks", PmsCostControl.DbText(request.Remarks)), Uid(login)));
        }
        catch (Exception ex) { return Failure(ex); }
    }

    public class MovementLineRequest
    {
        public string LineKind { get; set; }    // Budget / Reserve
        public int? CostCodeId { get; set; }
        public int? MilestoneId { get; set; }
        public int? ReserveId { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
    }

    public class MovementRequest
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string MovementType { get; set; }
        public string Reason { get; set; }
        public string RiskReference { get; set; }
        public List<MovementLineRequest> Lines { get; set; }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object SaveMovement(MovementRequest request)
    {
        try
        {
            if (request == null) throw new ArgumentException("Nothing to save.");
            var login = RequireOnProject(PmsCostControl.Need.Enter, request.ProjectId);
            if (request.Id > 0 && PmsCostControl.RequireRecordProject(login, "Movement", request.Id) != request.ProjectId)
                throw new HttpException(400, "The movement belongs to another project.");
            return SaveResult(Run("usp_pms_cc_Movement_Save", Reg(login), new SqlParameter("@id", request.Id), Project(request.ProjectId),
                new SqlParameter("@MovementType", request.MovementType ?? ""), new SqlParameter("@Reason", request.Reason ?? ""),
                new SqlParameter("@RiskReference", PmsCostControl.DbText(request.RiskReference)),
                new SqlParameter("@LinesJson", PmsCostControl.ToJson(request.Lines ?? new List<MovementLineRequest>())), Uid(login)));
        }
        catch (Exception ex) { return Failure(ex); }
    }

    /// <summary>action: Submit / Discard (enter right) or Approve / Reject (approval right; never the submitter).</summary>
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object SetMovementStatus(int movementId, string action, string reason)
    {
        try
        {
            string value = (action ?? "").Trim();
            if (value != "Submit" && value != "Discard" && value != "Approve" && value != "Reject") throw new ArgumentException("Unknown action.");
            var need = value == "Approve" || value == "Reject" ? PmsCostControl.Need.Approve : PmsCostControl.Need.Enter;
            var login = RequireOnRecord(need, "Movement", movementId);
            return SaveResult(Run("usp_pms_cc_Movement_SetStatus", Reg(login), new SqlParameter("@MovementId", movementId),
                new SqlParameter("@Action", value), new SqlParameter("@Reason", PmsCostControl.DbText(reason)), Uid(login)));
        }
        catch (Exception ex) { return Failure(ex); }
    }

    #endregion

    #region Change register and at-risk work

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetChanges(int projectId)
    {
        try
        {
            var login = RequireOnProject(PmsCostControl.Need.View, projectId);
            DataSet ds = Run("usp_pms_cc_Change_List", Reg(login), Project(projectId));
            return new { Success = true, Changes = Rows(ds, 0), Allocations = Rows(ds, 1) };
        }
        catch (Exception ex) { return Failure(ex); }
    }

    public class ChangeAllocationRequest
    {
        public int CostCodeId { get; set; }
        public int? MilestoneId { get; set; }
        public decimal Amount { get; set; }
    }

    public class ChangeRequest
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ChangeClass { get; set; }
        public decimal? EstimatedCost { get; set; }
        public decimal? AmountInEtc { get; set; }
        public bool IsProbable { get; set; }
        public decimal? ClientValueProposed { get; set; }
        public decimal? SubcontractValue { get; set; }
        public bool IsAtRisk { get; set; }
        public List<ChangeAllocationRequest> Allocations { get; set; }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object SaveChange(ChangeRequest request)
    {
        try
        {
            if (request == null) throw new ArgumentException("Nothing to save.");
            var login = RequireOnProject(PmsCostControl.Need.Enter, request.ProjectId);
            if (request.Id > 0 && PmsCostControl.RequireRecordProject(login, "Change", request.Id) != request.ProjectId)
                throw new HttpException(400, "The change belongs to another project.");
            return SaveResult(Run("usp_pms_cc_Change_Save", Reg(login), new SqlParameter("@id", request.Id), Project(request.ProjectId),
                new SqlParameter("@Title", request.Title ?? ""), new SqlParameter("@Description", PmsCostControl.DbText(request.Description)),
                new SqlParameter("@ChangeClass", request.ChangeClass ?? ""),
                new SqlParameter("@EstimatedCost", PmsCostControl.DbValue(request.EstimatedCost)),
                new SqlParameter("@AmountInEtc", request.AmountInEtc ?? 0m), new SqlParameter("@IsProbable", request.IsProbable),
                new SqlParameter("@ClientValueProposed", PmsCostControl.DbValue(request.ClientValueProposed)),
                new SqlParameter("@SubcontractValue", PmsCostControl.DbValue(request.SubcontractValue)),
                new SqlParameter("@IsAtRisk", request.IsAtRisk),
                new SqlParameter("@AllocationsJson", PmsCostControl.ToJson(request.Allocations ?? new List<ChangeAllocationRequest>())),
                Uid(login)));
        }
        catch (Exception ex) { return Failure(ex); }
    }

    /// <summary>
    /// Sets one independent status (CM-2). field: Internal / AtRisk need the approval right (never the person who raised it);
    /// Client / Subcontract / Record need the enter right.
    /// </summary>
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object SetChangeStatus(int changeId, string field, string value, string reason, string decisionDate, decimal? amount)
    {
        try
        {
            string f = (field ?? "").Trim();
            var need = f == "Internal" || f == "AtRisk" ? PmsCostControl.Need.Approve : PmsCostControl.Need.Enter;
            var login = RequireOnRecord(need, "Change", changeId);
            object date = String.IsNullOrWhiteSpace(decisionDate) ? (object)DBNull.Value : PmsCostControl.ParseBusinessDate(decisionDate, "the decision date");
            return SaveResult(Run("usp_pms_cc_Change_SetStatus", Reg(login), new SqlParameter("@ChangeId", changeId),
                new SqlParameter("@Field", f), new SqlParameter("@Value", value ?? ""), new SqlParameter("@Reason", PmsCostControl.DbText(reason)),
                new SqlParameter("@DecisionDate", SqlDbType.Date) { Value = date }, new SqlParameter("@Amount", PmsCostControl.DbValue(amount)), Uid(login)));
        }
        catch (Exception ex) { return Failure(ex); }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object CreateChangePosting(int changeId)
    {
        try
        {
            var login = RequireOnRecord(PmsCostControl.Need.Enter, "Change", changeId);
            return SaveResult(Run("usp_pms_cc_Change_CreatePosting", Reg(login), new SqlParameter("@ChangeId", changeId), Uid(login)));
        }
        catch (Exception ex) { return Failure(ex); }
    }

    #endregion

    #region Cost entries (actuals)

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetCostEntries(int projectId)
    {
        try
        {
            var login = RequireOnProject(PmsCostControl.Need.View, projectId);
            DataSet ds = Run("usp_pms_cc_CostEntry_List", Reg(login), Project(projectId));
            List<Dictionary<string, object>> documents;
            try { documents = Rows(Run("usp_pms_cc_CostEntryDoc_List", Reg(login), Project(projectId)), 0); }
            catch (SqlException) { documents = new List<Dictionary<string, object>>(); }   // phase 4 script not installed yet
            return new { Success = true, Entries = Rows(ds, 0), Splits = Rows(ds, 1), Documents = documents };
        }
        catch (Exception ex) { return Failure(ex); }
    }

    /// <summary>Removes (soft-deletes) a supporting document. Upload and viewing are in document.ashx.</summary>
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object RemoveCostEntryDocument(int documentId, string reason)
    {
        try
        {
            var login = Require(PmsCostControl.Need.Enter);
            DataSet doc = Run("usp_pms_cc_CostEntryDoc_Get", Reg(login), new SqlParameter("@id", documentId));
            if (doc == null || doc.Tables.Count == 0 || doc.Tables[0].Rows.Count == 0) throw new HttpException(404, "The document was not found.");
            PmsCostControl.RequireProject(login, Convert.ToInt32(doc.Tables[0].Rows[0]["ProjectId"], CultureInfo.InvariantCulture));
            return SaveResult(Run("usp_pms_cc_CostEntryDoc_Remove", Reg(login), new SqlParameter("@id", documentId),
                new SqlParameter("@Reason", PmsCostControl.DbText(reason)), Uid(login)));
        }
        catch (Exception ex) { return Failure(ex); }
    }

    public class CostSplitRequest
    {
        public int CostCodeId { get; set; }
        public int? MilestoneId { get; set; }
        public int? TaskId { get; set; }
        public decimal Amount { get; set; }
    }

    public class CostEntryRequest
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int? MilestoneId { get; set; }
        public int? TaskId { get; set; }
        public int CostCodeId { get; set; }
        public string EntryDate { get; set; }   // yyyy-MM-dd, business date only
        public decimal? Amount { get; set; }
        public string SourceDocType { get; set; }
        public string ReferenceNo { get; set; }
        public string SiteName { get; set; }
        public string Remarks { get; set; }
        public string Reason { get; set; }      // required when changing a saved entry
        public bool ConfirmDuplicate { get; set; }
        public int? ChangeId { get; set; }      // change / at-risk link
        public List<CostSplitRequest> Splits { get; set; }   // 2+ rows totalling the amount, or empty
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object SaveCostEntry(CostEntryRequest request)
    {
        try
        {
            if (request == null) throw new ArgumentException("Nothing to save.");
            var login = RequireOnProject(PmsCostControl.Need.Enter, request.ProjectId);
            if (request.Id > 0 && PmsCostControl.RequireRecordProject(login, "CostEntry", request.Id) != request.ProjectId)
                throw new HttpException(400, "The cost entry belongs to another project.");
            DateTime entryDate = PmsCostControl.ParseBusinessDate(request.EntryDate, "the cost date");
            bool isSplit = request.Splits != null && request.Splits.Count > 0;
            return SaveResult(Run("usp_pms_cc_CostEntry_Save",
                Reg(login),
                new SqlParameter("@id", request.Id),
                Project(request.ProjectId),
                new SqlParameter("@MilestoneId", PmsCostControl.DbValue(request.MilestoneId)),
                new SqlParameter("@TaskId", PmsCostControl.DbValue(request.TaskId)),
                new SqlParameter("@CostCodeId", request.CostCodeId),
                PmsCostControl.DateParameter("@EntryDate", entryDate),
                new SqlParameter("@Amount", PmsCostControl.DbValue(request.Amount)),
                new SqlParameter("@ReferenceNo", PmsCostControl.DbText(request.ReferenceNo)),
                new SqlParameter("@Remarks", PmsCostControl.DbText(request.Remarks)),
                Uid(login),
                new SqlParameter("@SiteName", PmsCostControl.DbText(request.SiteName)),
                new SqlParameter("@SourceDocType", PmsCostControl.DbText(request.SourceDocType)),
                new SqlParameter("@Reason", PmsCostControl.DbText(request.Reason)),
                new SqlParameter("@ConfirmDuplicate", request.ConfirmDuplicate),
                new SqlParameter("@ChangeId", PmsCostControl.DbValue(request.ChangeId)),
                new SqlParameter("@SplitsJson", isSplit ? (object)PmsCostControl.ToJson(request.Splits) : DBNull.Value)));
        }
        catch (Exception ex) { return Failure(ex); }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object DeleteCostEntry(int costEntryId, string reason)
    {
        try
        {
            var login = RequireOnRecord(PmsCostControl.Need.Enter, "CostEntry", costEntryId);
            return SaveResult(Run("usp_pms_cc_CostEntry_Delete", Reg(login), new SqlParameter("@id", costEntryId), Uid(login),
                new SqlParameter("@Reason", PmsCostControl.DbText(reason))));
        }
        catch (Exception ex) { return Failure(ex); }
    }

    #endregion

    #region Forecast (ETC)

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetEtc(int projectId)
    {
        try
        {
            var login = RequireOnProject(PmsCostControl.Need.View, projectId);
            DataSet ds = Run("usp_pms_cc_Etc_List", Reg(login), Project(projectId));
            return new { Success = true, Rows = Rows(ds, 0) };
        }
        catch (Exception ex) { return Failure(ex); }
    }

    public class EtcRequest
    {
        public int ProjectId { get; set; }
        public int CostCodeId { get; set; }
        public int? MilestoneId { get; set; }
        public decimal? EtcAmount { get; set; }
        public string AsOfDate { get; set; }
        public string Remarks { get; set; }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object SaveEtc(EtcRequest request)
    {
        try
        {
            if (request == null) throw new ArgumentException("Nothing to save.");
            var login = RequireOnProject(PmsCostControl.Need.Enter, request.ProjectId);
            DateTime asOf = PmsCostControl.ParseBusinessDate(request.AsOfDate, "the as-of date");
            return SaveResult(Run("usp_pms_cc_Etc_Save",
                Reg(login),
                Project(request.ProjectId),
                new SqlParameter("@CostCodeId", request.CostCodeId),
                new SqlParameter("@MilestoneId", PmsCostControl.DbValue(request.MilestoneId)),
                new SqlParameter("@EtcAmount", PmsCostControl.DbValue(request.EtcAmount)),
                PmsCostControl.DateParameter("@AsOfDate", asOf),
                new SqlParameter("@Remarks", PmsCostControl.DbText(request.Remarks)),
                Uid(login)));
        }
        catch (Exception ex) { return Failure(ex); }
    }

    #endregion

    #region Reports and snapshots

    /// <summary>Live figures when snapshotId is 0; otherwise the frozen month-end snapshot.</summary>
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetBudgetVsActual(int projectId, string groupBy, int snapshotId)
    {
        try
        {
            var login = RequireOnProject(PmsCostControl.Need.View, projectId);
            string grouping = PmsCostControl.NormalizeGroupBy(groupBy);
            DataSet ds;
            if (snapshotId > 0)
            {
                if (PmsCostControl.RequireRecordProject(login, "Snapshot", snapshotId) != projectId)
                    throw new HttpException(400, "The snapshot belongs to another project.");
                ds = Run("usp_pms_cc_Snapshot_Report", Reg(login), new SqlParameter("@SnapshotId", snapshotId), new SqlParameter("@GroupBy", grouping));
            }
            else
                ds = Run("usp_pms_cc_BudgetVsActual", Reg(login), Project(projectId), new SqlParameter("@GroupBy", grouping));
            return new { Success = true, Rows = Rows(ds, 0), Header = Rows(ds, 1).FirstOrDefault() };
        }
        catch (Exception ex) { return Failure(ex); }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetSnapshots(int projectId)
    {
        try
        {
            var login = RequireOnProject(PmsCostControl.Need.View, projectId);
            return new { Success = true, Rows = Rows(Run("usp_pms_cc_Snapshot_List", Reg(login), Project(projectId)), 0) };
        }
        catch (Exception ex) { return Failure(ex); }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetProjectSummary()
    {
        try
        {
            var login = Require(PmsCostControl.Need.View);
            return new { Success = true, Rows = Rows(Run("usp_pms_cc_ProjectSummary", Reg(login), Uid(login), new SqlParameter("@IsAdmin", login.IsAdmin)), 0) };
        }
        catch (Exception ex) { return Failure(ex); }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetPendingApprovals()
    {
        try
        {
            var login = Require(PmsCostControl.Need.View);
            return new { Success = true, Rows = Rows(Run("usp_pms_cc_PendingApprovals", Reg(login), Uid(login), new SqlParameter("@IsAdmin", login.IsAdmin)), 0) };
        }
        catch (Exception ex) { return Failure(ex); }
    }

    #endregion

    #region Periods

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetPeriods(int projectId)
    {
        try
        {
            var login = RequireOnProject(PmsCostControl.Need.View, projectId);
            return new { Success = true, Rows = Rows(Run("usp_pms_cc_Period_List", Reg(login), Project(projectId)), 0) };
        }
        catch (Exception ex) { return Failure(ex); }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object SetPeriodStatus(int projectId, string periodStart, bool close, string reason)
    {
        try
        {
            var login = RequireOnProject(PmsCostControl.Need.Approve, projectId);
            DateTime period = PmsCostControl.ParseBusinessDate(periodStart, "the month");
            return SaveResult(Run("usp_pms_cc_Period_SetStatus", Reg(login), Project(projectId),
                PmsCostControl.DateParameter("@PeriodStart", period), new SqlParameter("@Close", close),
                new SqlParameter("@Reason", PmsCostControl.DbText(reason)), Uid(login)));
        }
        catch (Exception ex) { return Failure(ex); }
    }

    #endregion

    #region History and access

    /// <summary>projectId = 0 returns company-level history (cost codes, access).</summary>
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetHistory(int projectId)
    {
        try
        {
            PmsCostControl.Context login;
            if (projectId <= 0)
            {
                login = Require(PmsCostControl.Need.Admin);
            }
            else login = RequireOnProject(PmsCostControl.Need.View, projectId);
            DataSet ds = Run("usp_pms_cc_Audit_List", Reg(login), new SqlParameter("@ProjectId", PmsCostControl.DbValue((int?)projectId)));
            return new { Success = true, Rows = Rows(ds, 0) };
        }
        catch (Exception ex) { return Failure(ex); }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetAccessList()
    {
        try
        {
            var login = Require(PmsCostControl.Need.Admin);
            return new { Success = true, Rows = Rows(Run("usp_pms_cc_Access_List", Reg(login)), 0) };
        }
        catch (Exception ex) { return Failure(ex); }
    }

    public class AccessRequest
    {
        public int EmployeeId { get; set; }
        public bool CanView { get; set; }
        public bool CanEnter { get; set; }
        public bool CanApprove { get; set; }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object SaveAccess(AccessRequest request)
    {
        try
        {
            var login = Require(PmsCostControl.Need.Admin);
            if (request == null || request.EmployeeId <= 0) throw new ArgumentException("Select an employee.");
            return SaveResult(Run("usp_pms_cc_Access_Save", Reg(login), new SqlParameter("@EmployeeId", request.EmployeeId),
                new SqlParameter("@CanView", request.CanView), new SqlParameter("@CanEnter", request.CanEnter),
                new SqlParameter("@CanApprove", request.CanApprove), Uid(login)));
        }
        catch (Exception ex) { return Failure(ex); }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetAccessProjects(int employeeId)
    {
        try
        {
            var login = Require(PmsCostControl.Need.Admin);
            DataSet ds = Run("usp_pms_cc_Access_ProjectList", Reg(login), new SqlParameter("@EmployeeId", employeeId));
            return new { Success = true, ProjectIds = Rows(ds, 0).Select(r => Convert.ToInt32(r["ProjectId"], CultureInfo.InvariantCulture)).ToList() };
        }
        catch (Exception ex) { return Failure(ex); }
    }

    /// <summary>An empty list means "all projects".</summary>
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object SaveAccessProjects(int employeeId, List<int> projectIds)
    {
        try
        {
            var login = Require(PmsCostControl.Need.Admin);
            string csv = String.Join(",", (projectIds ?? new List<int>()).Where(id => id > 0).Distinct().Select(id => id.ToString(CultureInfo.InvariantCulture)).ToArray());
            return SaveResult(Run("usp_pms_cc_Access_ProjectSave", Reg(login), new SqlParameter("@EmployeeId", employeeId),
                new SqlParameter("@ProjectIds", csv), Uid(login)));
        }
        catch (Exception ex) { return Failure(ex); }
    }

    #endregion
}
