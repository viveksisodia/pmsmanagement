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
/// PMS Cost Control: cost codes, versioned budgets with maker-checker approval,
/// cost entries, estimate to complete, budget-vs-actual, project summary,
/// monthly period lock, audit history and access management.
/// All data access goes through the parameterised usp_pms_cc_* procedures
/// (pms/costcontrol/sql/costcontrol_phase1.sql + costcontrol_phase2.sql).
/// Access rules live in App_Code/PmsCostControl.cs.
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
        DataRow row = ds.Tables[0].Rows[0];
        int result = Convert.ToInt32(row["Result"], CultureInfo.InvariantCulture);
        int id = ds.Tables[0].Columns.Contains("id") && row["id"] != DBNull.Value ? Convert.ToInt32(row["id"], CultureInfo.InvariantCulture) : 0;
        return new { Success = result == 1, NeedsConfirm = result == 2, Message = Convert.ToString(row["Message"], CultureInfo.InvariantCulture), Id = id };
    }

    private static object Failure(Exception ex) { return new { Success = false, Message = ex.Message }; }

    private static SqlParameter Reg(PmsCostControl.Context login) { return new SqlParameter("@regid", login.RegId); }
    private static SqlParameter Uid(PmsCostControl.Context login) { return new SqlParameter("@uid", login.EmployeeId); }

    #endregion

    #region Lookups

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetLookups()
    {
        try
        {
            var login = Require(PmsCostControl.Need.View);
            DataSet projects = Run("usp_pms_cc_Project_List", Reg(login));
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
            var login = Require(PmsCostControl.Need.View);
            DataSet ds = Run("usp_pms_cc_ProjectStructure", Reg(login), new SqlParameter("@ProjectId", projectId));
            return new { Success = true, Milestones = Rows(ds, 0), Tasks = Rows(ds, 1) };
        }
        catch (Exception ex) { return Failure(ex); }
    }

    /// <summary>Adds a unit to the existing unit master through the existing ERP procedure.</summary>
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

    #region Budget

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetBudget(int projectId, int versionId)
    {
        try
        {
            var login = Require(PmsCostControl.Need.View);
            DataSet ds = Run("usp_pms_cc_Budget_Get", Reg(login), new SqlParameter("@ProjectId", projectId),
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
            var login = Require(PmsCostControl.Need.Enter);
            return SaveResult(Run("usp_pms_cc_BudgetVersion_Create", Reg(login), new SqlParameter("@ProjectId", projectId),
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
            var login = Require(PmsCostControl.Need.Enter);
            return SaveResult(Run("usp_pms_cc_BudgetVersion_Submit", Reg(login), new SqlParameter("@VersionId", versionId), Uid(login)));
        }
        catch (Exception ex) { return Failure(ex); }
    }

    /// <summary>Approve and lock. The procedure refuses the person who submitted it (AP-03).</summary>
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object LockBudgetVersion(int versionId)
    {
        try
        {
            var login = Require(PmsCostControl.Need.Approve);
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
            var login = Require(PmsCostControl.Need.Approve);
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
            var login = Require(PmsCostControl.Need.Enter);
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
            var login = Require(PmsCostControl.Need.Enter);
            if (request == null) throw new ArgumentException("Nothing to save.");
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
            var login = Require(PmsCostControl.Need.Enter);
            return SaveResult(Run("usp_pms_cc_BudgetLine_Delete", Reg(login), new SqlParameter("@id", budgetLineId), Uid(login)));
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
            var login = Require(PmsCostControl.Need.View);
            DataSet ds = Run("usp_pms_cc_CostEntry_List", Reg(login), new SqlParameter("@ProjectId", projectId));
            return new { Success = true, Entries = Rows(ds, 0) };
        }
        catch (Exception ex) { return Failure(ex); }
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
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object SaveCostEntry(CostEntryRequest request)
    {
        try
        {
            var login = Require(PmsCostControl.Need.Enter);
            if (request == null) throw new ArgumentException("Nothing to save.");
            DateTime entryDate = PmsCostControl.ParseBusinessDate(request.EntryDate, "the cost date");
            return SaveResult(Run("usp_pms_cc_CostEntry_Save",
                Reg(login),
                new SqlParameter("@id", request.Id),
                new SqlParameter("@ProjectId", request.ProjectId),
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
                new SqlParameter("@ConfirmDuplicate", request.ConfirmDuplicate)));
        }
        catch (Exception ex) { return Failure(ex); }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object DeleteCostEntry(int costEntryId, string reason)
    {
        try
        {
            var login = Require(PmsCostControl.Need.Enter);
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
            var login = Require(PmsCostControl.Need.View);
            DataSet ds = Run("usp_pms_cc_Etc_List", Reg(login), new SqlParameter("@ProjectId", projectId));
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
            var login = Require(PmsCostControl.Need.Enter);
            if (request == null) throw new ArgumentException("Nothing to save.");
            DateTime asOf = PmsCostControl.ParseBusinessDate(request.AsOfDate, "the as-of date");
            return SaveResult(Run("usp_pms_cc_Etc_Save",
                Reg(login),
                new SqlParameter("@ProjectId", request.ProjectId),
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

    #region Reports

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetBudgetVsActual(int projectId, string groupBy)
    {
        try
        {
            var login = Require(PmsCostControl.Need.View);
            string grouping = PmsCostControl.NormalizeGroupBy(groupBy);
            DataSet ds = Run("usp_pms_cc_BudgetVsActual", Reg(login), new SqlParameter("@ProjectId", projectId), new SqlParameter("@GroupBy", grouping));
            return new { Success = true, Rows = Rows(ds, 0), Header = Rows(ds, 1).FirstOrDefault() };
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
            return new { Success = true, Rows = Rows(Run("usp_pms_cc_ProjectSummary", Reg(login)), 0) };
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
            var login = Require(PmsCostControl.Need.View);
            return new { Success = true, Rows = Rows(Run("usp_pms_cc_Period_List", Reg(login), new SqlParameter("@ProjectId", projectId)), 0) };
        }
        catch (Exception ex) { return Failure(ex); }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object SetPeriodStatus(int projectId, string periodStart, bool close, string reason)
    {
        try
        {
            var login = Require(PmsCostControl.Need.Approve);
            DateTime period = PmsCostControl.ParseBusinessDate(periodStart, "the month");
            return SaveResult(Run("usp_pms_cc_Period_SetStatus", Reg(login), new SqlParameter("@ProjectId", projectId),
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
            var login = Require(PmsCostControl.Need.View);
            if (projectId <= 0 && !login.IsAdmin) throw new HttpException(403, "Only administrators can view company-level history.");
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

    #endregion
}
