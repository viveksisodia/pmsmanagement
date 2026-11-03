using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CommonClass;
using Spire.Xls;

public partial class Downloadexcel : System.Web.UI.Page
{
    DataAccess da = new DataAccess();
    public CommonFunction cmf = new CommonFunction();
    protected void Page_Load(object sender, EventArgs e)
    {
       
        Workbook wb = new Workbook();
        Worksheet sheet = wb.Worksheets[0];
       
        string[] result1;
        string[] result2;
        string[] result3;
       
      
      
            sheet.Range["A1"].Text = "ProjectID";
            sheet.Range["B1"].Text = "ProjectName";
            sheet.Range["C1"].Text = "Owner";
            sheet.Range["D1"].Text = "team";
            sheet.Range["E1"].Text = "ProjectStatus";
            sheet.Range["F1"].Text = "ProjectAddress1";
            sheet.Range["G1"].Text = "ProjectAddress2";
            sheet.Range["H1"].Text = "City";
            sheet.Range["I1"].Text = "PostalCode";
            sheet.Range["J1"].Text = "StateName";
            sheet.Range["K1"].Text = "Countryid";
            sheet.Range["L1"].Text = "ClientName";
            sheet.Range["M1"].Text = "Priority";
            sheet.Range["N1"].Text = "Budget";
            sheet.Range["O1"].Text = "Actual";
            sheet.Range["P1"].Text = "AwardDate";
            sheet.Range["Q1"].Text = "StartDate";
            sheet.Range["R1"].Text = "TargetDate";
            sheet.Range["S1"].Text = "ApprovalDrawing";
            sheet.Range["T1"].Text = "DrawingApprovalDate";
            sheet.Range["U1"].Text = "Uploaddrawing";

            sheet.Range["A1"].Style.Font.IsBold = true;
            sheet.Range["B1"].Style.Font.IsBold = true;
            sheet.Range["C1"].Style.Font.IsBold = true;
            sheet.Range["D1"].Style.Font.IsBold = true;
            sheet.Range["E1"].Style.Font.IsBold = true;
            sheet.Range["F1"].Style.Font.IsBold = true;
            sheet.Range["G1"].Style.Font.IsBold = true;
            sheet.Range["H1"].Style.Font.IsBold = true;
            sheet.Range["I1"].Style.Font.IsBold = true;
            sheet.Range["J1"].Style.Font.IsBold = true;
            sheet.Range["K1"].Style.Font.IsBold = true;
            sheet.Range["L1"].Style.Font.IsBold = true;
            sheet.Range["M1"].Style.Font.IsBold = true;
            sheet.Range["N1"].Style.Font.IsBold = true;
            sheet.Range["O1"].Style.Font.IsBold = true;
            sheet.Range["P1"].Style.Font.IsBold = true;
            sheet.Range["Q1"].Style.Font.IsBold = true;
            sheet.Range["R1"].Style.Font.IsBold = true;
            sheet.Range["S1"].Style.Font.IsBold = true;
            sheet.Range["T1"].Style.Font.IsBold = true;
            sheet.Range["U1"].Style.Font.IsBold = true;




        
            #region ProjectStatus
            string strsqlProjectStatus = "select id,ProjectStatus  from Pms_tbl_Projectstatus where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0 order by id";
            DataSet dsddlstaus = da.GetDataSetInline(strsqlProjectStatus);
            result1 = new string[dsddlstaus.Tables[0].Rows.Count];
            for (int i = 0; i < dsddlstaus.Tables[0].Rows.Count; i++)
            {
                result1[i] = dsddlstaus.Tables[0].Rows[i]["ProjectStatus"].ToString();
            }
        #endregion

        Response.Write(result1.Length);
        Response.End();

        #region OwnerName
        int isadmin = 0;
            if (Request.Cookies["cadmin"]["id"].ToString() == "1") 
            {
                
                isadmin = 1;
            }
         

            SqlParameter[] prm = new SqlParameter[3];
            prm[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"]);
            prm[1] = new SqlParameter("@isadmin", isadmin);
            prm[2] = new SqlParameter("@uid", Request.Cookies["user"]["user_id"].ToString());
            DataSet dsddlOwnerName = da.GetDataSet("Pms_bindemployee_forTransfer", prm);
            result2 = new string[dsddlOwnerName.Tables[0].Rows.Count];
            for (int i = 0; i < dsddlOwnerName.Tables[0].Rows.Count; i++)
            {
                result2[i] = dsddlOwnerName.Tables[0].Rows[i]["displayname"].ToString();
            }
            //Response.Write(result2.Length);
            // Response.End();
            #endregion
            #region CountryName
            string strsqlCountryName = "select id,countryname  from tbl_countrymaster  where  status=1 and archive=0 order by id";
            DataSet dsddlCountryName = da.GetDataSetInline(strsqlCountryName);
            result3 = new string[dsddlCountryName.Tables[0].Rows.Count];
            for (int i = 0; i < dsddlCountryName.Tables[0].Rows.Count; i++)
            {
                result3[i] = dsddlCountryName.Tables[0].Rows[i]["countryname"].ToString();
            }
        #endregion
        




        for (int i = 2; i < 200; i++)
            {

                


                try
                {

                    sheet.Range["H" + i.ToString()].DataValidation.Values = result1;
                    sheet.Range["H" + i.ToString()].DataValidation.IsSuppressDropDownArrow = false;
                    sheet.Range["H" + i.ToString()].DataValidation.AlertStyle = AlertStyleType.Warning;
                    sheet.Range["H" + i.ToString()].DataValidation.ShowError = true;
                    sheet.Range["H" + i.ToString()].DataValidation.ErrorTitle = "Error001";
                    sheet.Range["H" + i.ToString()].DataValidation.ErrorMessage = "Please select an item from list!";
                }
                catch
                {

                }
                try
                {

                    sheet.Range["I" + i.ToString()].DataValidation.Values = result2;
                    sheet.Range["I" + i.ToString()].DataValidation.IsSuppressDropDownArrow = false;
                    sheet.Range["I" + i.ToString()].DataValidation.AlertStyle = AlertStyleType.Warning;
                    sheet.Range["I" + i.ToString()].DataValidation.ShowError = true;
                    sheet.Range["I" + i.ToString()].DataValidation.ErrorTitle = "Error001";
                    sheet.Range["I" + i.ToString()].DataValidation.ErrorMessage = "Please select an item from list!";
                    //Response.Write(result2.Length);
                    //Response.End();
                }
                catch
                {

                }
                try
                {

                    sheet.Range["K" + i.ToString()].DataValidation.Values = result3;
                    sheet.Range["K" + i.ToString()].DataValidation.IsSuppressDropDownArrow = false;
                    sheet.Range["K" + i.ToString()].DataValidation.AlertStyle = AlertStyleType.Warning;
                    sheet.Range["K" + i.ToString()].DataValidation.ShowError = true;
                    sheet.Range["K" + i.ToString()].DataValidation.ErrorTitle = "Error001";
                    sheet.Range["K" + i.ToString()].DataValidation.ErrorMessage = "Please select an item from list!";
              
            }
            catch
                {

                }
            

            }



            for (int j = 1; j < 25; j++)
            {
                sheet.SetColumnWidth(j, 20);
            }



            sheet.AutoFilters.Range = sheet.Range["A1:X1"];
            sheet.FreezePanes(2, 1);
            //sheet.fr
            string filename = "Project Details Import ";

            //if (dsc.Tables[0].Rows.Count > 0)
            //{
            //    filename += "- " + dsc.Tables[0].Rows[0]["compname"].ToString();
            //}
            filename += ".xlsx";
            wb.SaveToFile(Server.MapPath(filename), ExcelVersion.Version2013);

            Response.Redirect(filename);
    
       
    }
}