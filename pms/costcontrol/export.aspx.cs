using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using Spire.Xls;

/// <summary>
/// Excel export for Cost Control (RP-17).
///   export.aspx?type=report&amp;projectId=1&amp;groupBy=COSTCODE|MILESTONE|COSTTYPE
///   export.aspx?type=summary
///   export.aspx?type=entries&amp;projectId=1
///   export.aspx?type=budget&amp;projectId=1&amp;versionId=5
///   export.aspx?type=etc&amp;projectId=1
/// Every file carries a header block: report, project, filter / version, generated time and user.
/// Uses the same access rules as the page: a user cannot export what they cannot view.
/// The workbook is streamed to the browser; nothing is written to the server disk.
/// </summary>
public partial class pms_costcontrol_export : Page
{
    private sealed class Column
    {
        public string Header; public string Field; public bool IsNumber;
        public Column(string header, string field, bool isNumber) { Header = header; Field = field; IsNumber = isNumber; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PmsCostControl.Context login;
        try { login = PmsCostControl.Require(PmsCostControl.Need.View); }
        catch (HttpException ex) { Response.StatusCode = ex.GetHttpCode(); Response.Write(HttpUtility.HtmlEncode(ex.Message)); return; }

        string type = (Request.QueryString["type"] ?? "").Trim().ToLowerInvariant();
        int projectId = ReadInt("projectId");
        string projectName = projectId > 0 ? FindProjectName(login, projectId) : "All projects";
        if (projectId > 0 && projectName == null) { Response.StatusCode = 404; Response.Write("Project was not found."); return; }

        try { Export(login, type, projectId, projectName); }
        catch (ArgumentException ex) { Response.StatusCode = 400; Response.Write(HttpUtility.HtmlEncode(ex.Message)); }
    }

    private void Export(PmsCostControl.Context login, string type, int projectId, string projectName)
    {
        string title, filter = "", fileStem;
        DataTable data;
        List<Column> columns;

        switch (type)
        {
            case "summary":
                title = "Cost Control - Project summary"; fileStem = "CostControl_Summary";
                data = Table(PmsCostControl.RunProcedure("usp_pms_cc_ProjectSummary", Reg(login)), 0);
                columns = new List<Column> {
                    new Column("Project", "ProjectName", false), new Column("Current version", "CurrentVersionNo", false),
                    new Column("Open version", "OpenVersionStatus", false), new Column("Current budget", "CurrentBudget", true),
                    new Column("Actual", "Actual", true), new Column("ETC", "Etc", true), new Column("EAC", "Eac", true),
                    new Column("VAC", "Vac", true), new Column("Consumed %", "ConsumedPercent", true) };
                break;

            case "entries":
                RequireProject(projectId);
                title = "Cost Control - Cost entries"; fileStem = "CostControl_CostEntries";
                data = Table(PmsCostControl.RunProcedure("usp_pms_cc_CostEntry_List", Reg(login), new SqlParameter("@ProjectId", projectId)), 0);
                columns = new List<Column> {
                    new Column("Date", "EntryDate", false), new Column("Cost code", "CostCode", false), new Column("Cost code name", "CostCodeName", false),
                    new Column("Cost type", "CostType", false), new Column("Milestone", "MilestoneName", false), new Column("Task", "TaskName", false),
                    new Column("Site", "SiteName", false), new Column("Source document", "SourceDocType", false), new Column("Reference", "ReferenceNo", false),
                    new Column("Amount", "Amount", true), new Column("Remarks", "Remarks", false), new Column("Month closed", "IsPeriodClosed", false) };
                break;

            case "budget":
                RequireProject(projectId);
                title = "Cost Control - Budget"; fileStem = "CostControl_Budget";
                DataSet budget = PmsCostControl.RunProcedure("usp_pms_cc_Budget_Get", Reg(login), new SqlParameter("@ProjectId", projectId),
                    new SqlParameter("@VersionId", PmsCostControl.DbValue((int?)ReadInt("versionId"))));
                data = Table(budget, 1);
                filter = DescribeVersion(Table(budget, 0), data);
                columns = new List<Column> {
                    new Column("Cost code", "CostCode", false), new Column("Cost code name", "CostCodeName", false), new Column("Cost type", "CostType", false),
                    new Column("Milestone", "MilestoneName", false), new Column("Task", "TaskName", false), new Column("Quantity", "Quantity", true),
                    new Column("Unit", "Unit", false), new Column("Rate", "Rate", true), new Column("Amount", "Amount", true), new Column("Remarks", "Remarks", false) };
                break;

            case "etc":
                RequireProject(projectId);
                title = "Cost Control - Forecast (ETC)"; fileStem = "CostControl_Forecast";
                data = Table(PmsCostControl.RunProcedure("usp_pms_cc_Etc_List", Reg(login), new SqlParameter("@ProjectId", projectId)), 0);
                columns = new List<Column> {
                    new Column("Cost code", "CostCode", false), new Column("Cost code name", "CostCodeName", false), new Column("Milestone", "MilestoneName", false),
                    new Column("Current budget", "CurrentBudget", true), new Column("Actual", "Actual", true), new Column("ETC", "EtcAmount", true),
                    new Column("As of", "AsOfDate", false), new Column("Remarks", "Remarks", false) };
                break;

            default: // report
                RequireProject(projectId);
                string groupBy = PmsCostControl.NormalizeGroupBy(Request.QueryString["groupBy"]);
                title = "Cost Control - Budget vs Actual"; fileStem = "CostControl_BudgetVsActual";
                DataSet report = PmsCostControl.RunProcedure("usp_pms_cc_BudgetVsActual", Reg(login), new SqlParameter("@ProjectId", projectId), new SqlParameter("@GroupBy", groupBy));
                data = Table(report, 0);
                DataTable header = Table(report, 1);
                string version = header.Rows.Count > 0 && header.Rows[0]["CurrentVersionNo"] != DBNull.Value ? "locked version " + header.Rows[0]["CurrentVersionNo"] : "no locked budget";
                filter = "Grouped by " + (groupBy == "MILESTONE" ? "milestone" : groupBy == "COSTTYPE" ? "cost type" : "cost code") + "; current budget = " + version;
                columns = new List<Column> { new Column(groupBy == "MILESTONE" ? "Milestone" : groupBy == "COSTTYPE" ? "Cost type" : "Cost code", "GroupName", false) };
                if (groupBy == "COSTCODE") columns.Add(new Column("Cost type", "CostType", false));
                columns.AddRange(new[] {
                    new Column("Original budget", "OriginalBudget", true), new Column("Current budget", "CurrentBudget", true),
                    new Column("Actual", "Actual", true), new Column("Remaining budget", "Variance", true), new Column("ETC", "Etc", true),
                    new Column("EAC", "Eac", true), new Column("VAC", "Vac", true), new Column("Consumed %", "ConsumedPercent", true) });
                break;
        }

        WriteWorkbook(title, projectName, filter, UserName(login), columns, data, fileStem);
    }

    private static void RequireProject(int projectId)
    {
        if (projectId <= 0) throw new ArgumentException("Select a project.");
    }

    private int ReadInt(string key)
    {
        int value;
        return Int32.TryParse(Request.QueryString[key], NumberStyles.Integer, CultureInfo.InvariantCulture, out value) ? value : 0;
    }

    private static SqlParameter Reg(PmsCostControl.Context login) { return new SqlParameter("@regid", login.RegId); }

    private static DataTable Table(DataSet ds, int index)
    {
        return ds != null && ds.Tables.Count > index ? ds.Tables[index] : new DataTable();
    }

    private static string FindProjectName(PmsCostControl.Context login, int projectId)
    {
        DataTable projects = Table(PmsCostControl.RunProcedure("usp_pms_cc_Project_List", Reg(login)), 0);
        DataRow match = projects.Rows.Cast<DataRow>().FirstOrDefault(r => Convert.ToInt32(r["id"], CultureInfo.InvariantCulture) == projectId);
        return match == null ? null : Convert.ToString(match["ProjectName"], CultureInfo.InvariantCulture);
    }

    private static string UserName(PmsCostControl.Context login)
    {
        DataTable names = Table(PmsCostControl.RunProcedure("usp_pms_cc_Employee_Names", Reg(login)), 0);
        DataRow match = names.Rows.Cast<DataRow>().FirstOrDefault(r => Convert.ToInt32(r["id"], CultureInfo.InvariantCulture) == login.EmployeeId);
        return match == null ? "Employee " + login.EmployeeId : Convert.ToString(match["EmployeeName"], CultureInfo.InvariantCulture);
    }

    private static string DescribeVersion(DataTable versions, DataTable lines)
    {
        if (versions.Rows.Count == 0) return "No budget version";
        int versionId = lines.Rows.Count > 0 ? Convert.ToInt32(lines.Rows[0]["VersionId"], CultureInfo.InvariantCulture) : Convert.ToInt32(versions.Rows[0]["id"], CultureInfo.InvariantCulture);
        DataRow v = versions.Rows.Cast<DataRow>().FirstOrDefault(r => Convert.ToInt32(r["id"], CultureInfo.InvariantCulture) == versionId) ?? versions.Rows[0];
        return "Version V" + v["VersionNo"] + " - " + v["VersionType"] + " (" + v["VersionStatus"] + ")";
    }

    private void WriteWorkbook(string title, string projectName, string filter, string userName, List<Column> columns, DataTable data, string fileStem)
    {
        Workbook workbook = new Workbook();
        Worksheet sheet = workbook.Worksheets[0];
        sheet.Name = "Report";

        // Header block (RP-17: the export carries its context).
        sheet.Range[1, 1].Text = title;
        sheet.Range[1, 1].Style.Font.IsBold = true;
        sheet.Range[1, 1].Style.Font.Size = 13;
        sheet.Range[2, 1].Text = "Project: " + projectName;
        sheet.Range[3, 1].Text = String.IsNullOrEmpty(filter) ? "Filter: none" : filter;
        sheet.Range[4, 1].Text = "Generated: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture) + " by " + userName;

        const int headerRow = 6;
        for (int c = 0; c < columns.Count; c++)
        {
            CellRange cell = sheet.Range[headerRow, c + 1];
            cell.Text = columns[c].Header;
            cell.Style.Font.IsBold = true;
        }

        int row = headerRow + 1;
        foreach (DataRow dataRow in data.Rows)
        {
            for (int c = 0; c < columns.Count; c++)
            {
                Column column = columns[c];
                object value = data.Columns.Contains(column.Field) ? dataRow[column.Field] : null;
                CellRange cell = sheet.Range[row, c + 1];
                if (value == null || value == DBNull.Value) continue;
                if (column.IsNumber)
                {
                    cell.NumberValue = Convert.ToDouble(value, CultureInfo.InvariantCulture);
                    cell.NumberFormat = "#,##0.00";
                }
                else if (column.Field == "IsPeriodClosed")
                    cell.Text = Convert.ToInt32(value, CultureInfo.InvariantCulture) == 1 ? "Yes" : "No";
                else
                    cell.Text = Convert.ToString(value, CultureInfo.InvariantCulture);
            }
            row++;
        }

        // Totals for amount columns (not for percentages or quantities/rates).
        if (data.Rows.Count > 0)
        {
            sheet.Range[row, 1].Text = "Total";
            sheet.Range[row, 1].Style.Font.IsBold = true;
            for (int c = 0; c < columns.Count; c++)
            {
                Column column = columns[c];
                if (!column.IsNumber || column.Field == "ConsumedPercent" || column.Field == "Quantity" || column.Field == "Rate") continue;
                double total = data.Rows.Cast<DataRow>().Where(r => r[column.Field] != DBNull.Value)
                                   .Sum(r => Convert.ToDouble(r[column.Field], CultureInfo.InvariantCulture));
                CellRange cell = sheet.Range[row, c + 1];
                cell.NumberValue = Math.Round(total, 2);
                cell.NumberFormat = "#,##0.00";
                cell.Style.Font.IsBold = true;
            }
        }

        for (int c = 1; c <= columns.Count; c++) sheet.SetColumnWidth(c, 18);
        sheet.FreezePanes(headerRow + 1, 1);

        string fileName = fileStem + "_" + DateTime.Now.ToString("yyyyMMdd_HHmm", CultureInfo.InvariantCulture) + ".xlsx";
        using (MemoryStream stream = new MemoryStream())
        {
            workbook.SaveToStream(stream, FileFormat.Version2013);
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
            Response.BinaryWrite(stream.ToArray());
            Response.Flush();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
}
