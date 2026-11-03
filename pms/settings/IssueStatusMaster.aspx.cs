using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using CommonClass;

public partial class settings_IssueStatusMaster : System.Web.UI.Page
{
    public function fns = new function();
    protected void Page_Load(object sender, EventArgs e) { }
    protected void Button1_Click(object sender, EventArgs e) { }

    private static int TenantId()
    {
        HttpCookie cookie = HttpContext.Current.Request.Cookies["cadmin"];
        int id;
        if (cookie == null || !Int32.TryParse(cookie["id"], out id) || id <= 0)
            throw new HttpException(401, "Login information is missing.");
        return id;
    }
    private static int RecordIdValue(string value)
    {
        int id;
        if (String.IsNullOrWhiteSpace(value)) return 0;
        if (!Int32.TryParse(value, out id) || id < 0) throw new ArgumentException("Invalid record ID.");
        return id;
    }
    private static string ColorValue(string value)
    {
        string color = (value ?? String.Empty).Trim().TrimStart('#');
        if (!Regex.IsMatch(color, "^[0-9a-fA-F]{6}$|^[0-9a-fA-F]{3}$"))
            throw new ArgumentException("Select a valid printing color.");
        return "#" + color;
    }
    private static DataSet GetStatus(int id, int tenant)
    {
        return new DataAccess().GetDataSetInline("SELECT id AS Id,name AS name,PrintColor FROM dbo.Pms_tbl_addIssueStatus WHERE status=1 AND archive=0 AND regid=" + tenant + " AND id=" + id);
    }
    [WebMethod]
    public static void SavePrintingMaster(IssueStatusMasterDetails printingdetails)
    {
        if (printingdetails == null) throw new ArgumentException("Issue status details are required.");
        int tenant = TenantId(), id = RecordIdValue(printingdetails.id);
        string name = (printingdetails.Name ?? String.Empty).Trim();
        if (name.Length == 0 || name.Length > 200) throw new ArgumentException("Enter an issue status name of up to 200 characters.");
        if (id > 0)
        {
            DataSet existing = GetStatus(id, tenant);
            if (existing.Tables.Count == 0 || existing.Tables[0].Rows.Count == 0)
                throw new ArgumentException("Issue status was not found.");
        }
        DataAccess da = new DataAccess();
        da.executeDMLQuery("dbo.pmstbl_Issuestatus_usp", new[] {
            new SqlParameter("@id",id), new SqlParameter("@name",name),
            new SqlParameter("@PrintColor",ColorValue(printingdetails.PrintColor)),
            new SqlParameter("@regid",tenant),new SqlParameter("@cdate",da.getcdatetime()),
            new SqlParameter("@type",id > 0 ? "Update" : "Insert")
        });
    }
    private static string ColorMarkup(int id, string color)
    {
        return "<div class='gap-1 flex-shrink-0 align-self-center'><a href='javascript:void(0);' id='btncolor_" + id +
            "' onclick='Binddetails_Addcolor(" + id + ",\"" + color + "\",this.id)' data-bs-toggle='modal' data-bs-target='#addcolor'><span style='width:20px;height:20px;display:inline-block;background:" + color + ";margin-left:5px;'></span></a></div>";
    }
    [WebMethod]
    public static string bindPrintingMasterData()
    {
        int tenant = TenantId();
        DataSet data = new DataAccess().GetDataSetInline("SELECT id,name,PrintColor FROM dbo.Pms_tbl_addIssueStatus WHERE status=1 AND archive=0 AND regid=" + tenant + " ORDER BY id");
        StringBuilder html = new StringBuilder("<table cellpadding='0' cellspacing='0' border='0' width='100%' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm datatable'><thead><tr><th>S.No</th><th>Issue Status Name</th><th>Printing Color</th><th>Action</th></tr></thead><tbody>");
        if (data != null && data.Tables.Count > 0)
        {
            int serial = 0;
            foreach (DataRow row in data.Tables[0].Rows)
            {
                int id = Convert.ToInt32(row["id"],CultureInfo.InvariantCulture);
                string color;
                try { color = ColorValue(Convert.ToString(row["PrintColor"])); } catch (ArgumentException) { color = "#8a94a6"; }
                html.Append("<tr><td>").Append(++serial).Append("</td><td>").Append(HttpUtility.HtmlEncode(Convert.ToString(row["name"])))
                    .Append("</td><td><div id='lblAddcolor_").Append(id).Append("'>").Append(ColorMarkup(id,color)).Append("</div></td><td>")
                    .Append("<a href='javascript:void(0);' class='btn btn-info btn-sm' onclick='Binddetails(").Append(id).Append(")' data-bs-toggle='offcanvas' data-bs-target='#addproject'><i class='ri-edit-line'></i></a> ")
                    .Append("<a href='javascript:void(0);' class='btn btn-danger btn-sm' onclick='deletedata(").Append(id).Append(")'><i class='ri-delete-bin-5-line'></i></a></td></tr>");
            }
        }
        return html.Append("</tbody></table>").ToString();
    }
    [WebMethod]
    public static string BindPrintingdetail(int RecordId = 0)
    {
        if (RecordId <= 0) throw new ArgumentException("Invalid record ID.");
        return GetStatus(RecordId,TenantId()).GetXml();
    }
    [WebMethod]
    public static string UpdateColor(string update_id = "", string Color = "")
    {
        int id = RecordIdValue(update_id), tenant = TenantId();
        if (id <= 0) throw new ArgumentException("Invalid record ID.");
        DataSet current = GetStatus(id,tenant);
        if (current.Tables.Count == 0 || current.Tables[0].Rows.Count == 0) throw new ArgumentException("Issue status was not found.");
        string color = ColorValue(Color);
        SavePrintingMaster(new IssueStatusMasterDetails { id=id.ToString(CultureInfo.InvariantCulture),Name=Convert.ToString(current.Tables[0].Rows[0]["name"]),PrintColor=color });
        return ColorMarkup(id,color);
    }
    [WebMethod]
    public static string Deleteprojectdata(int RecordId = 0)
    {
        if (RecordId <= 0) throw new ArgumentException("Invalid record ID.");
        int tenant = TenantId();
        new DataAccess().GetDataSetInline("UPDATE dbo.Pms_tbl_addIssueStatus SET status=0,archive=1 WHERE id=" + RecordId + " AND regid=" + tenant + " AND status=1 AND archive=0");
        return "1";
    }
}
public class IssueStatusMasterDetails
{
    public string id { get; set; }
    public string Name { get; set; }
    public string PrintColor { get; set; }
}
