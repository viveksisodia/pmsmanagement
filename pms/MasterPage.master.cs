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
using System.Collections.Generic;
using System.IO;
using CustomDropDown;
using System.Web.Services;


public partial class MasterPage : System.Web.UI.MasterPage
{
    DataSet ds = new DataSet();
    DataAccess da = new DataAccess();
    public CommonFunction cmf = new CommonFunction();
    SqlParameter[] prm, prm1;
    public function fns = new function();
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (Request.Cookies["user"] != null && Request.Cookies["cadmin"] != null)
        {

        }
        else
        {
            Response.Redirect("/login.aspx");
        }
        if (!IsPostBack)
        {
            userlogininsert();
            getname();
        }

        bindmenudata();
        String activepage = Request.RawUrl;
        if (activepage.Contains("/default.aspx"))
        {
           // page1.Attributes.Add("class", "nav-item active");
        }
        else if (activepage.Contains("/project.aspx"))
        {
            page2.Attributes.Add("class", "nav-item active");
        }
        else if (activepage.Contains("/team.aspx"))
        {
           // page3.Attributes.Add("class", "nav-item active");
        }
        else if (activepage.Contains("/milestone.aspx"))
        {
            page4.Attributes.Add("class", "nav-item active");
        }
        else if (activepage.Contains("/tasklist.aspx"))
        {
            //page5.Attributes.Add("class", "nav-item active");
        }
        else if (activepage.Contains("/task.aspx"))
        {
            page6.Attributes.Add("class", "nav-item active");
        }
        else if (activepage.Contains("/issues.aspx"))
        {
            page7.Attributes.Add("class", "nav-item active");
        }
        else if (activepage.Contains("/documents.aspx"))
        {
           // page8.Attributes.Add("class", "nav-item active");
        }
        else if (activepage.Contains("/reports.aspx"))
        {
           // page9.Attributes.Add("class", "nav-item active");
        }
        else if (activepage.Contains("/expenses.aspx"))
        {
            //page10.Attributes.Add("class", "nav-item active");
        }
        else if (activepage.Contains("/purchase.aspx"))
        {
           // page11.Attributes.Add("class", "nav-item active");
        }
        else if (activepage.Contains("/hr-management.aspx"))
        {
            page12.Attributes.Add("class", "nav-item active");
        }
    }
    protected void getname()
    {
        string str = "select emp_name,roleid,f_name,l_name,DisplayName from tbl_employee where id='" + Request.Cookies["user"]["user_id"].ToString() + "' and status=1 and archive=0 and isnull(isactivelogin,0)!='2' ";
        DataSet ds = da.GetDataSetInline(str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            lblusername.Text = "Welcome " + ds.Tables[0].Rows[0]["DisplayName"].ToString() + "!";
        }
        else
        {
            Response.Redirect("/login.aspx");
        }

    }
    protected void bindmenudata()
    {
        ViewState["isadmin"] = 0;
       
        string strsql = "select Manage_Profile.* from Manage_Profile inner join tbl_employee on Manage_Profile.id=cast(tbl_employee.roleid as int) where tbl_employee.id='" + Request.Cookies["user"]["user_id"] + "'  and Manage_Profile.status=1 and Manage_Profile.archive=0";
        //Response.Write(strsql);

        DataSet ds = da.GetDataSetInline(strsql);

        if (ds.Tables[0].Rows.Count > 0)
        {

            if (ds.Tables[0].Rows[0]["id"].ToString() == "1")
            {
                ViewState["isadmin"] = 1;
                page2.Visible = true;
                page4.Visible = true;
                page6.Visible = true;
                page13.Visible = true;
                page7.Visible = true;                

            }
            else
            {
                ViewState["isadmin"] = 0;
                string strsqlmodule = "select * from tbl_rolepermisionsdetails where regid='" + Request.Cookies["cadmin"]["id"].ToString() + "' and profileid='" + ds.Tables[0].Rows[0]["id"].ToString() + "'";
                //Response.Write(strsqlmodule);
                //Response.End();
                //SaveLogText("strsqlmoduleA-"+ strsqlmodule);
                DataSet dsmodule = da.GetDataSetInline(strsqlmodule);
                if (dsmodule.Tables[0].Rows.Count > 0)
                {
                   
                    for (int i = 0; i < dsmodule.Tables[0].Rows.Count; i++)
                    {
                        string chkfullcontrolCustomersvalue = "0";
                        string CheckBox1Customersvalue = "0";
                        string CheckBox2Customersvalue = "0";
                        string CheckBox3Customersvalue = "0";
                        string CheckBox4Customersvalue = "0";
                        string CheckBox5Customersvalue = "0";

                        chkfullcontrolCustomersvalue = dsmodule.Tables[0].Rows[i]["fullaccesspermision"].ToString();
                        CheckBox1Customersvalue = dsmodule.Tables[0].Rows[i]["viewpermision"].ToString();
                        CheckBox2Customersvalue = dsmodule.Tables[0].Rows[i]["Createpermision"].ToString();
                        CheckBox3Customersvalue = dsmodule.Tables[0].Rows[i]["Editpermision"].ToString();
                        CheckBox4Customersvalue = dsmodule.Tables[0].Rows[i]["Deletepermision"].ToString();
                        //CheckBox5Customersvalue = dsmodule.Tables[0].Rows[i]["Assignownerpermision"].ToString();
                      if (dsmodule.Tables[0].Rows[i]["moduleid"].ToString() == "53")
                        {
                            if (chkfullcontrolCustomersvalue == "1" || CheckBox1Customersvalue == "1" || CheckBox2Customersvalue == "1" || CheckBox3Customersvalue == "1" || CheckBox4Customersvalue == "1")
                            {
                                page2.Visible = true;
                            }
                            else
                            {
                                page2.Visible = false;
                            }
                            
                        }
                     else if (dsmodule.Tables[0].Rows[i]["moduleid"].ToString() == "54")
                        {
                            if (chkfullcontrolCustomersvalue == "1" || CheckBox1Customersvalue == "1" || CheckBox2Customersvalue == "1" || CheckBox3Customersvalue == "1" || CheckBox4Customersvalue == "1")
                            {
                                page4.Visible = true;
                            }
                            else
                            {
                                page4.Visible = false;
                            }
                           
                           
                        }
                        else if (dsmodule.Tables[0].Rows[i]["moduleid"].ToString() == "55")
                        {
                            if (chkfullcontrolCustomersvalue == "1" || CheckBox1Customersvalue == "1" || CheckBox2Customersvalue == "1" || CheckBox3Customersvalue == "1" || CheckBox4Customersvalue == "1")
                            {
                                page6.Visible = true;
                            }
                            else
                            {
                                page6.Visible = false;
                            }
                           

                        }
                        else if (dsmodule.Tables[0].Rows[i]["moduleid"].ToString() == "56")
                        {
                            if (chkfullcontrolCustomersvalue == "1" || CheckBox1Customersvalue == "1" || CheckBox2Customersvalue == "1" || CheckBox3Customersvalue == "1" || CheckBox4Customersvalue == "1")
                            {
                                page13.Visible = true;
                            }
                            else
                            {
                                page13.Visible = false;
                            }
                            

                        }
                        else if (dsmodule.Tables[0].Rows[i]["moduleid"].ToString() == "57")
                        {
                            if (chkfullcontrolCustomersvalue == "1" || CheckBox1Customersvalue == "1" || CheckBox2Customersvalue == "1" || CheckBox3Customersvalue == "1" || CheckBox4Customersvalue == "1")
                            {
                                page7.Visible = true;
                            }
                            else
                            {
                                page7.Visible = false;
                            }
                            

                        }
                        else if (dsmodule.Tables[0].Rows[i]["moduleid"].ToString() == "459")
                        {

                           // if (chkfullcontrolCustomersvalue == "0")
                           // {
                              //  page2.Visible = false;
                            //}

                        }

                    }
                }
                
            }

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
    protected void bindmenudataold()
    {
        ViewState["isadmin"] = 0;

        string strsql = "select Manage_Profile.* from Manage_Profile inner join tbl_employee on Manage_Profile.id=cast(tbl_employee.roleid as int) where tbl_employee.id='" + Request.Cookies["user"]["user_id"] + "'  and Manage_Profile.status=1 and Manage_Profile.archive=0";
        //Response.Write(strsql);

        DataSet ds = da.GetDataSetInline(strsql);

        if (ds.Tables[0].Rows.Count > 0)
        {

            if (ds.Tables[0].Rows[0]["id"].ToString() == "1")
            {
                ViewState["isadmin"] = 1;
                page2.Visible = true;
                page4.Visible = true;
                page6.Visible = true;
                page13.Visible = true;
                page7.Visible = true;

            }
            else
            {
                ViewState["isadmin"] = 0;
                string strsqlmodule = "select * from tbl_rolepermisionsdetails where regid='" + Request.Cookies["cadmin"]["id"].ToString() + "' and profileid='" + ds.Tables[0].Rows[0]["id"].ToString() + "'";
                DataSet dsmodule = da.GetDataSetInline(strsqlmodule);
                if (dsmodule.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < dsmodule.Tables[0].Rows.Count; i++)
                    {
                        string chkfullcontrolCustomersvalue = "0";
                        string CheckBox1Customersvalue = "0";
                        string CheckBox2Customersvalue = "0";
                        string CheckBox3Customersvalue = "0";
                        string CheckBox4Customersvalue = "0";
                        string CheckBox5Customersvalue = "0";

                        chkfullcontrolCustomersvalue = dsmodule.Tables[0].Rows[i]["fullaccesspermision"].ToString();
                        CheckBox1Customersvalue = dsmodule.Tables[0].Rows[i]["viewpermision"].ToString();
                        CheckBox2Customersvalue = dsmodule.Tables[0].Rows[i]["Createpermision"].ToString();
                        CheckBox3Customersvalue = dsmodule.Tables[0].Rows[i]["Editpermision"].ToString();
                        CheckBox4Customersvalue = dsmodule.Tables[0].Rows[i]["Deletepermision"].ToString();
                        //CheckBox5Customersvalue = dsmodule.Tables[0].Rows[i]["Assignownerpermision"].ToString();
                        if (dsmodule.Tables[0].Rows[i]["moduleid"].ToString() == "19")
                        {
                            if (chkfullcontrolCustomersvalue == "1" || CheckBox1Customersvalue == "1" || CheckBox2Customersvalue == "1" || CheckBox3Customersvalue == "1" || CheckBox4Customersvalue == "1")
                            {
                                page2.Visible = true;
                            }
                            else
                            {
                                page2.Visible = false;
                            }

                        }
                        else if (dsmodule.Tables[0].Rows[i]["moduleid"].ToString() == "455")
                        {
                            if (chkfullcontrolCustomersvalue == "1" || CheckBox1Customersvalue == "1" || CheckBox2Customersvalue == "1" || CheckBox3Customersvalue == "1" || CheckBox4Customersvalue == "1")
                            {
                                page4.Visible = true;
                            }
                            else
                            {
                                page4.Visible = false;
                            }


                        }
                        else if (dsmodule.Tables[0].Rows[i]["moduleid"].ToString() == "456")
                        {
                            if (chkfullcontrolCustomersvalue == "1" || CheckBox1Customersvalue == "1" || CheckBox2Customersvalue == "1" || CheckBox3Customersvalue == "1" || CheckBox4Customersvalue == "1")
                            {
                                page6.Visible = true;
                            }
                            else
                            {
                                page6.Visible = false;
                            }


                        }
                        else if (dsmodule.Tables[0].Rows[i]["moduleid"].ToString() == "457")
                        {
                            if (chkfullcontrolCustomersvalue == "1" || CheckBox1Customersvalue == "1" || CheckBox2Customersvalue == "1" || CheckBox3Customersvalue == "1" || CheckBox4Customersvalue == "1")
                            {
                                page13.Visible = true;
                            }
                            else
                            {
                                page13.Visible = false;
                            }


                        }
                        else if (dsmodule.Tables[0].Rows[i]["moduleid"].ToString() == "458")
                        {
                            if (chkfullcontrolCustomersvalue == "1" || CheckBox1Customersvalue == "1" || CheckBox2Customersvalue == "1" || CheckBox3Customersvalue == "1" || CheckBox4Customersvalue == "1")
                            {
                                page7.Visible = true;
                            }
                            else
                            {
                                page7.Visible = false;
                            }


                        }
                        else if (dsmodule.Tables[0].Rows[i]["moduleid"].ToString() == "459")
                        {

                            // if (chkfullcontrolCustomersvalue == "0")
                            // {
                            //  page2.Visible = false;
                            //}

                        }

                    }
                }

            }

        }



    }
    protected void userlogininsert()
    {
        string strsqladm = @"select *  from crm_tbl_roletaskmaster where status=1 and archive=0 and entrytype='form' ";
        DataSet dsadm = da.GetDataSetInline(strsqladm);
        if (dsadm.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dsadm.Tables[0].Rows.Count; i++)
            {
                string strsqladmchk = @"select *  from crm_tbl_moduleshow where status=1 and archive=0  
                                        and moduleid='" + dsadm.Tables[0].Rows[i]["id"].ToString() + "'   and regid= '" + Request.Cookies["cadmin"]["id"].ToString() + "'";
                DataSet dsadmchk = da.GetDataSetInline(strsqladmchk);
                if (dsadmchk.Tables[0].Rows.Count == 0)
                {
                    SqlParameter[] prmm = new SqlParameter[3];
                    prmm[0] = new SqlParameter("@regid", Request.Cookies["cadmin"]["id"]);
                    prmm[1] = new SqlParameter("@moduleid", dsadm.Tables[0].Rows[i]["id"].ToString());
                    prmm[2] = new SqlParameter("@isshow", "1");
                    da.executeDMLQuery("crm_tbl_moduleshowinsertupdateHome", prmm);
                    //if (dsadm.Tables[0].Rows[i]["id"].ToString() == "13")
                    //{
                    //    Response.Write("ok");
                    //    Response.End();
                    //}
                }
            }
        }

    }
}
