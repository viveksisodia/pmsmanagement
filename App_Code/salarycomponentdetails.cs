using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using CommonClass;
using System.Web.Script.Services;
using System.IO;
using System.Web.UI.WebControls;
/// <summary>
/// Summary description for employee
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class salarycomponentdetails : System.Web.Services.WebService
{

    public salarycomponentdetails()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }

    /// <summary>
    /// Salary component Details
    /// </summary>
    /// <param name="empid"></param>
    /// <param name="mid"></param>
    /// <param name="yearid"></param>
    /// <returns></returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string getsalarycomponent(string empid,string mid,string yearid)
    {
        string strdata = "";

        decimal totalgross = 0;

        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        function fns = new function();
        ////ADO.Net
        //SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();

        SqlParameter[] prm = new SqlParameter[3];

        prm[0] = new SqlParameter("@uid", empid);
        prm[2] = new SqlParameter("@mid", mid);
        prm[1] = new SqlParameter("@yearid", yearid);

        ds = da.GetDataSet("tbl_employeesalarymonthcomponentdetailsselect", prm);
        if(ds.Tables[0].Rows.Count>0)
        {
            strdata += "<table cellpadding='0' cellspacing='0' border='0' class='table table-bordered datatables' id='editable'>";

            strdata += "<thead>";
            strdata += "<tr>";
            strdata += "<th>Salary Break up</th>";
            strdata += "   <th>";
            strdata += fns.getmonthname( Convert.ToInt32(mid)) + "-" + yearid;
            strdata += "   </th>";       

           
            strdata += " </tr>";
            strdata += "</thead>";
            strdata += "  <tbody>";
            for (int i=0;i<ds.Tables[0].Rows.Count;i++)
            {
                strdata += "<tr>";
                strdata += "<td>"+ds.Tables[0].Rows[i]["headname"].ToString()+ "</td>";
                strdata += "   <td>";
                strdata += ds.Tables[0].Rows[i]["comamount"].ToString();

                try
                {
                    totalgross += Convert.ToDecimal(ds.Tables[0].Rows[i]["comamount"].ToString());
                }
                catch(Exception ex)
                {

                }
                strdata += "   </td>";
                strdata += "   </tr>";
            }



            prm = new SqlParameter[3];

            prm[0] = new SqlParameter("@uid", empid);
            prm[2] = new SqlParameter("@mid", mid);
            prm[1] = new SqlParameter("@yearid", yearid);

            ds = da.GetDataSet("tbl_employeesalarymonthcomponentdetailsselectbasecomponents", prm);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strdata += "<tr>";
                    strdata += "<td>" + ds.Tables[0].Rows[i]["headname"].ToString() + "</td>";
                    strdata += "   <td>";
                    strdata += ds.Tables[0].Rows[i]["comamount"].ToString();

                    try
                    {
                        totalgross += Convert.ToDecimal(ds.Tables[0].Rows[i]["comamount"].ToString());
                    }
                    catch (Exception ex)
                    {

                    }
                    strdata += "   </td>";
                    strdata += "   </tr>";
                }

            }

            ////////bonus///////////
            prm = new SqlParameter[3];

            prm[0] = new SqlParameter("@uid", empid);
            prm[2] = new SqlParameter("@mid", mid);
            prm[1] = new SqlParameter("@yearid", yearid);

            ds = da.GetDataSet("tbl_employeesalarymonthcomponentdetailsbonusbonusselect", prm);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strdata += "<tr>";
                    strdata += "<td>" + ds.Tables[0].Rows[i]["headname"].ToString() + "</td>";
                    strdata += "   <td>";
                    strdata += ds.Tables[0].Rows[i]["comamount"].ToString();

                    try
                    {
                        totalgross += Convert.ToDecimal(ds.Tables[0].Rows[i]["comamount"].ToString());
                    }
                    catch (Exception ex)
                    {

                    }
                    strdata += "   </td>";
                    strdata += "   </tr>";
                }

            }

            ////////One Time Payment///////////
            prm = new SqlParameter[3];

            prm[0] = new SqlParameter("@uid", empid);
            prm[2] = new SqlParameter("@mid", mid);
            prm[1] = new SqlParameter("@yearid", yearid);

            ds = da.GetDataSet("tbl_employeesalarymonthcomponentdetailsonetimepaymentselect", prm);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strdata += "<tr>";
                    strdata += "<td>" + ds.Tables[0].Rows[i]["headname"].ToString() + "</td>";
                    strdata += "   <td>";
                    strdata += ds.Tables[0].Rows[i]["comamount"].ToString();

                    try
                    {
                        totalgross += Convert.ToDecimal(ds.Tables[0].Rows[i]["comamount"].ToString());
                    }
                    catch (Exception ex)
                    {

                    }
                    strdata += "   </td>";
                    strdata += "   </tr>";
                }

            }


            strdata += "  </tbody>";


            strdata += " <tfoot>";
            strdata += "<tr>";
            strdata += "<td></td>";
        
            strdata += "  <td>Total: " + totalgross + "</td>";
          
            strdata += " </tr>";
            strdata += "</tfoot>";


            strdata += "  </table>";

        }

        return strdata;
    }



}
