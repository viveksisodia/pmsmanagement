using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CommonClass;

public partial class settings_adddesignation : System.Web.UI.Page
{
    DataAccess da = new DataAccess();
    DataSet ds = new DataSet();
    public CommonFunction Cmf = new CommonFunction();
    public function fns = new function();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            bindgroup();
        }
    }
    protected void bindgroup()
    {
        SqlParameter[] prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"]);
        prm[1] = new SqlParameter("@type", "Select");
        DataSet ds = da.GetDataSet("tbl_Standard_master_DML", prm);
        GrdvDepartment.DataSource = ds;
        GrdvDepartment.DataBind();
    }

    protected void btncreate_Click(object sender, EventArgs e)
    {
        if (btncreate.Text.Contains("Update"))
        {
            SqlParameter[] prm = new SqlParameter[6];
            prm[0] = new SqlParameter("@id", ViewState["id"]);
            prm[1] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"]);
            prm[2] = new SqlParameter("@Standard_Name", Standard_Name.Text.Trim());
            prm[3] = new SqlParameter("@Scheme_Name", Scheme_Name.Text.Trim());
            prm[4] = new SqlParameter("@C_Date", da.getcdate());
            prm[5] = new SqlParameter("@type", "Update");
            da.executeDMLQuery("tbl_Standard_master_DML", prm);

            Page.RegisterClientScriptBlock("js", "<script>alert('Data Updated Sucessfully!!!!!!'); </script>");
            Standard_Name.Text = "";
            Scheme_Name.Text = "";
            btncreate.Text = "Create";
            bindgroup();
        }
        else
        {
            if (checkitemname())
            {
                SqlParameter[] prm = new SqlParameter[5];
                prm[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"]);
                prm[1] = new SqlParameter("@Standard_Name", Standard_Name.Text.Trim());
                prm[2] = new SqlParameter("@Scheme_Name", Scheme_Name.Text.Trim());
                prm[3] = new SqlParameter("@C_Date", da.getcdate());
                prm[4] = new SqlParameter("@type", "Insert");
                da.executeDMLQuery("tbl_Standard_master_DML", prm);

                Page.RegisterClientScriptBlock("js", "<script>alert('Data Save Sucessfully!!!!!!');</script>");
                Standard_Name.Text = "";
                Scheme_Name.Text = "";
                bindgroup();
            }
            else
            {
                Page.RegisterClientScriptBlock("js", "<script>alert('This Name is Already Exist!!!!!!');</script>");
            }
        }
    }
    protected bool checkitemname()
    {
        SqlParameter[] prm = new SqlParameter[4];
        prm[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"].ToString());
        prm[1] = new SqlParameter("@Standard_Name", Standard_Name.Text.Trim());
        prm[2] = new SqlParameter("@Scheme_Name", Scheme_Name.Text.Trim());
        prm[3] = new SqlParameter("@type", "Check");
        DataSet dsc = da.GetDataSet("tbl_Standard_master_DML", prm);
        if (dsc.Tables[0].Rows.Count > 0)
        {
            return false;
        }

        return true;
    }
    protected void lnkedit_Click(object sender, EventArgs e)
    {
        LinkButton link = (LinkButton)GrdvDepartment.FindControl("btnedit");
        link = (LinkButton)sender;
        GridViewRow gvr = (GridViewRow)link.NamingContainer;
        int id = Convert.ToInt32(GrdvDepartment.DataKeys[gvr.RowIndex].Value);
        ViewState["id"] = id;

        Label name = (Label)gvr.FindControl("lblName");
        Label lblcodee = (Label)gvr.FindControl("lblcodee");
        Standard_Name.Text = name.Text;
        Scheme_Name.Text = lblcodee.Text;
        btncreate.Text = "Update";
    }

    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        LinkButton link = (LinkButton)GrdvDepartment.FindControl("lnkDelete");
        link = (LinkButton)sender;
        GridViewRow gvr = (GridViewRow)link.NamingContainer;
        string i = GrdvDepartment.DataKeys[gvr.RowIndex].Value.ToString();
        int id = Convert.ToInt32(i);
        SqlParameter[] prm = new SqlParameter[3];
        prm[0] = new SqlParameter("@id", id);
        prm[1] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"]);
        prm[2] = new SqlParameter("@type", "Delete");
        da.executeDMLQuery("tbl_Standard_master_DML", prm);

        Page.RegisterClientScriptBlock("js", "<script>alert('Data Deleted Sucessfully!!!!!!')</script>");
        bindgroup();
    }
    protected void GrdvDepartment_PreRender(object sender, EventArgs e)
    {
        try
        {
            if (GrdvDepartment.Rows.Count > 0)
            {
                GrdvDepartment.UseAccessibleHeader = true;
                GrdvDepartment.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
        catch (Exception)
        {
        }
    }
}