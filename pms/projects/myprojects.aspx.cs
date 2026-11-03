﻿using System;
using System.Web;
using System.Collections.Generic;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Script.Services;
using CommonClass;
using System.Data;
using System.IO;
using System.Linq;
using System.Globalization;


public partial class myprojects : System.Web.UI.Page
{
    // PERFORMANCE: project list lookups are batch-loaded once per request; existing output fields are preserved.
    DataSet ds = new DataSet();
    DataAccess da = new DataAccess();
    public CommonFunction cmf = new CommonFunction();
    SqlParameter[] prm, prm1;
    public function fns = new function();
    public string projectdata = "";
    public string Multiteam = "";
    public string MultiOwner = "";
    public string Multichangeowner = "";
    public string Multiaddteam = "";
    public string checkstring = "";
    public int ischeckAllaccess = 0;

    public int ischeckCreate = 0;
    public int ischeckEdit = 0;
    public int ischeckDelete = 0;
    public int ischeckView = 0;
    public int ischeckAssign = 0;

    

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string strvalidationS = get_EditDelete_Validation("53");
            //string strvalidationS = get_EditDelete_Validation("1056");
            //Response.Write(strvalidationS);
            string[] arS = strvalidationS.Split(',');
            ischeckAllaccess = Convert.ToInt32(arS[0]);
            ischeckView = Convert.ToInt32(arS[1]);
            ischeckCreate = Convert.ToInt32(arS[2]);
            ischeckEdit = Convert.ToInt32(arS[3]);
            ischeckDelete = Convert.ToInt32(arS[4]);
            ischeckAssign = Convert.ToInt32(arS[5]);
            if(ischeckAllaccess==1)
            {
                ischeckView = 1;
                ischeckCreate = 1;
                ischeckEdit = 1;
                ischeckDelete = 1;
                ischeckAssign = 1;
            }
           
            if (ischeckView == 0 && ischeckCreate == 0 && ischeckEdit == 0 && ischeckDelete == 0 && ischeckAssign == 0)
            {
                Response.Redirect("/pms/accessdenied.aspx");
            }

        }
        catch
        {

        }
    }
    public static string get_EditDelete_Validation(string moduleid)
    {
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
            
            string strsql = "select Manage_Profile.* from Manage_Profile inner join tbl_employee on Manage_Profile.id=cast(tbl_employee.roleid as int) where tbl_employee.id='" + HttpContext.Current.Request.Cookies["user"]["user_id"] + "'  and Manage_Profile.status=1 and Manage_Profile.archive=0";
            DataSet ds = da.GetDataSetInline(strsql);

            if (ds.Tables[0].Rows.Count > 0)
            {

                if (ds.Tables[0].Rows[0]["id"].ToString() == "1")
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
                    string strsqlmodule = @"select crm_tbl_roletaskmaster.formname,crm_tbl_roletaskmaster.formurl, isnull(ismanage,0) ismanage,isnull(isschedule,0) isschedule,  isnull(ischangeowner,0) ischangeowner,
                            isnull(formaccess,0) formaccess, isnull(isinsert,0) isinsert, isnull(isupdate,0) isupdate, isnull(isdelete,0) isdelete, isnull(isview,0) isview, isnull(isimport,0) isimport, isnull(isexport,0) isexport, isnull(issendmail,0) issendmail, isnull(isviewmail,0) isviewmail, isnull(isdelmail,0) isdelmail, isnull(isfull,0) isfull, isnull(moduleid,0) moduleid,isnull(isassign,0) as isassign  
                                    from crm_tbl_roleaccessdetails inner join crm_tbl_roletaskmaster on crm_tbl_roleaccessdetails.moduleid=crm_tbl_roletaskmaster.id 
                                    where crm_tbl_roleaccessdetails.status=1 and  crm_tbl_roleaccessdetails.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and crm_tbl_roleaccessdetails.roleid='" + ds.Tables[0].Rows[0]["id"].ToString() + "' and entrytype='form' and moduleid=" + moduleid + "";
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
        }
        catch { }
        retval = strFull + "," + strView + "," + strCreate + "," + strEdit + ","+ strDelete + strAssign+"," + strChangeOwner;
        return retval;
    }

    #region Customer details
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string checkduplicate(string projectid, string id)
    {
        string ret = "0";
        DataAccess da1 = new DataAccess();

        try
        {
            if (string.IsNullOrWhiteSpace(projectid))
            {
                return "0";
            }

            string safeProjectId = projectid.Trim().Replace("'", "''");
            int currentProjectId = 0;
            int.TryParse(id, out currentProjectId);

            string sqlq =
                "select id from pms_tbl_addproject with(nolock) " +
                "where status=1 and archive=0 " +
                "and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' " +
                "and LTRIM(RTRIM(ProjectID))='" + safeProjectId + "' ";

            // During update, do not treat the current project row as duplicate.
            if (currentProjectId > 0)
            {
                sqlq += "and id<>" + currentProjectId;
            }

            DataSet dsD = da1.GetDataSetInline(sqlq);

            if (dsD != null &&
                dsD.Tables.Count > 0 &&
                dsD.Tables[0].Rows.Count > 0)
            {
                ret = "1";
            }
        }
        catch (Exception ex)
        {
            SaveLogText("checkduplicateEx-" + ex.Message);
            ret = "-1";
        }

        return ret;
    }
    public static string getdefaultstatus()
    {
        DataAccess da1 = new DataAccess();
        string retval = "0";
        string sqldeflt = "select id from Pms_tbl_Projectstatus where  status=1 and archive=0 and isnull(isdefault,0)=1 and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
        DataSet dsdeflt = da1.GetDataSetInline(sqldeflt);
        if (dsdeflt.Tables[0].Rows.Count > 0)
        {
            retval = dsdeflt.Tables[0].Rows[0]["id"].ToString();
        }
        return retval;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string saveproject(newproject projectdetails, string ptype)
    {
        string retval = "0";
        DataAccess danew = new DataAccess();
        CommonFunction cmf1 = new CommonFunction();
        if (projectdetails != null)
        {
            string entrytype = ptype;            
            try
            {
                if (projectdetails.ProjectStatus == string.Empty || projectdetails.ProjectStatus == "0")
                {
                    projectdetails.ProjectStatus = getdefaultstatus();
                }
            }
            catch { }
            SqlParameter[] prm = new SqlParameter[38];
            prm[0] = new SqlParameter("@id", projectdetails.id);
            prm[1] = new SqlParameter("@ProjectID", projectdetails.ProjectID);
            prm[2] = new SqlParameter("@ProjectName", projectdetails.ProjectName);
            prm[3] = new SqlParameter("@Owner", projectdetails.Owner);
            prm[4] = new SqlParameter("@team", projectdetails.team);
            prm[5] = new SqlParameter("@ProjectStatus", projectdetails.ProjectStatus);
            prm[6] = new SqlParameter("@ProjectAddress1", projectdetails.ProjectAddress1);
            prm[7] = new SqlParameter("@ProjectAddress2", projectdetails.ProjectAddress2);
            prm[8] = new SqlParameter("@City", projectdetails.City);
            prm[9] = new SqlParameter("@PostalCode", projectdetails.PostalCode);
            prm[10] = new SqlParameter("@StateName", projectdetails.Stateid);
            prm[11] = new SqlParameter("@Countryid", projectdetails.Countryid);
            prm[12] = new SqlParameter("@ClientName", projectdetails.ClientName);

            prm[13] = new SqlParameter("@Priority", projectdetails.Priority);
            prm[14] = new SqlParameter("@Budget", projectdetails.Budget);
            prm[15] = new SqlParameter("@Actual", projectdetails.Actual);

            if (projectdetails.AwardDate == null)
            {
                prm[16] = new SqlParameter("@AwardDate", DBNull.Value);
            }
            else
            {
                prm[16] = new SqlParameter("@AwardDate", Convert.ToDateTime(projectdetails.AwardDate).Date);
            }

            if (projectdetails.StartDate == null)
            {
                prm[17] = new SqlParameter("@StartDate", DBNull.Value);
            }
            else
            {
                prm[17] = new SqlParameter("@StartDate", Convert.ToDateTime(projectdetails.StartDate).Date);
            }

            if (projectdetails.TargetDate == null)
            {
                prm[18] = new SqlParameter("@TargetDate", DBNull.Value);
            }
            else
            {
                prm[18] = new SqlParameter("@TargetDate", Convert.ToDateTime(projectdetails.TargetDate).Date);
            }

            if (projectdetails.ApprovalDrawing==null)
            {
                prm[19] = new SqlParameter("@ApprovalDrawing", DBNull.Value);
            }
            else
            {
                prm[19] = new SqlParameter("@ApprovalDrawing", projectdetails.ApprovalDrawing == true ? 1 : 0);
            }

            if (projectdetails.DrawingApprovalDate == null)
            {
                prm[20] = new SqlParameter("@DrawingApprovalDate", DBNull.Value);
            }
            else
            {
                prm[20] = new SqlParameter("@DrawingApprovalDate", projectdetails.DrawingApprovalDate);
            }

            
            prm[21] = new SqlParameter("@Uploaddrawing", DBNull.Value);           

            prm[22] = new SqlParameter("@Weightage", projectdetails.Weightage);
            prm[23] = new SqlParameter("@isdrawing", (projectdetails.RequiredDrawing==true?1:0));
            prm[24] = new SqlParameter("@checkprojectWeightage",(projectdetails.checkmilesameWeightage== true ? 1 : 0));
            prm[25] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[26] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prm[27] = new SqlParameter("@cdate", danew.getcdatetime());
            prm[28] = new SqlParameter("@type", entrytype);
            prm[29] = new SqlParameter("@customerid", projectdetails.customerid);
            prm[30] = new SqlParameter("@salesorderid", projectdetails.salesorderid);
            if (projectdetails.hastask != null)
            {
                
                prm[31] = new SqlParameter("@hastask", (projectdetails.hastask == true ? 1 : 0));
            }
            else
            {
                prm[31] = new SqlParameter("@hastask", DBNull.Value);
            }
            if (projectdetails.checktasksameWeightage!= null)
            {
                prm[32] = new SqlParameter("@checktaskWeightage", (projectdetails.checktasksameWeightage == true ? 1 : 0));
            }
            else
            {
                prm[32] = new SqlParameter("@checktaskWeightage", DBNull.Value);
            }
            if (projectdetails.hassubtask != null)
            {
                prm[33] = new SqlParameter("@hassubtask", (projectdetails.hassubtask == true ? 1 : 0));
            }
            else
            {
                prm[33] = new SqlParameter("@hassubtask", DBNull.Value);
            }
            if (projectdetails.checksubtasksameWeightage!= null)
            {
                prm[34] = new SqlParameter("@checksubtaskWeightage", (projectdetails.checksubtasksameWeightage == true ? 1 : 0));
            }
            else
            {
                prm[34] = new SqlParameter("@checksubtaskWeightage", DBNull.Value);
            }

            if (projectdetails.iscountholydays != null)
            {
                prm[35] = new SqlParameter("@iscountholydays", (projectdetails.iscountholydays == true ? 1 : 0));
            }
            else
            {
                prm[35] = new SqlParameter("@iscountholydays", DBNull.Value);
            }
            if (projectdetails.iscountweekoff != null)
            {
                prm[36] = new SqlParameter("@iscountweekoff", (projectdetails.iscountweekoff == true ? 1 : 0));
            }
            else
            {
                prm[36] = new SqlParameter("@iscountweekoff", DBNull.Value);
            }
            //da1.executeDMLQuery("pmstbl_Addproject_usp_old", prm);
            prm[37] = new SqlParameter("ReturnValue", SqlDbType.Int);
            prm[37].Direction = ParameterDirection.ReturnValue;
            try
            {
                int retval1 = danew.executeDMLQuery("pmstbl_Addproject_usp", prm);
                if (retval1 > 0)
                {
                  if(entrytype=="insert"&&(projectdetails.ProjectID==string.Empty|| projectdetails.ProjectID == null))
                    {
                        string sqlupdate = "update pms_tbl_addproject set ProjectID='P"+ retval1.ToString()+ "' where id="+ retval1.ToString()+ " and regid='"+ HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                        danew.executeDMLQueryinline(sqlupdate);
                    }
                    
                }
                retval = retval1.ToString();
            }
            catch(Exception ex)
            {
                SaveLogText("saveprojectex-"+ex.Message);
            }
        }

        return  retval;
        
    }
 [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<customerlist> bindcustomerlist()
    {
        string str2 = "select id,txtcompanyname from tbl_contactsdetails where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0 and ContactType=1 and isnull(txtcompanyname,'')!='' order by txtcompanyname";
        
        DataAccess danew = new DataAccess();
        DataSet drmile = danew.GetDataSetInline(str2);

        List<customerlist> list = new List<customerlist>();

        foreach (DataRow row in drmile.Tables[0].Rows)
        {
            customerlist obj = new customerlist();
            obj.customerid = row["id"].ToString();
            obj.ClientName = row["txtcompanyname"].ToString();
           
            list.Add(obj);
        }

        return list;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<solist> bindsolist(string clientid,string bindtype)
    {
        
        DataAccess da1 = new DataAccess();
        List<solist> list = new List<solist>();
        if (clientid==string.Empty&& clientid== "0")
        {
            return list;
        }            
        string strssql = "select id,Estimateno from tbl_SalesOrderdetails where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and cusid ='" + clientid + "' and id not in(select isnull(salesorderid,0) salesorderid from Pms_tbl_AddProject  where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "') order by id ";
        
            if (bindtype == "edit")
            {
                strssql = "select id,Estimateno from tbl_SalesOrderdetails where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and cusid ='" + clientid + "'  order by id ";
            }
        SaveLogText("solist-" + strssql);
        DataSet drSO = da1.GetDataSetInline(strssql);

        foreach (DataRow row in drSO.Tables[0].Rows)
        {
            solist obj = new solist();
            obj.salesorderid = row["id"].ToString();
            obj.Estimateno = row["Estimateno"].ToString();
            list.Add(obj);
        }

        return list;
    }
    #endregion

    #region update status
    [WebMethod]
    public static string UpdateProject_Status(string projectid = "", string statusid = "", string iscomplete = "")
    {
        string retval = "0";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (projectid != "" && projectid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[8];
        prm[0] = new SqlParameter("@id", projectid);
        prm[1] = new SqlParameter("@ProjectStatus", statusid);
        prm[2] = new SqlParameter("@iscomplete", iscomplete);
        prm[3] = new SqlParameter("@iscompletedate", da1.getcdatetime());
        prm[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[6] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[7] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pmstbl_Status_DeatilsUpdate_usp", prm);


        // percentage update start  // 
        if (iscomplete == "1")
        {
            decimal projecttotalpercent = 100;
            string strstatusupdate = "update Pms_tbl_AddProject set iscompletepercentage='" + projecttotalpercent.ToString() + "' where id='" + projectid + "' and status=1 and archive=0 and isnull(iscomplete,0)=1";
            DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

        }
        else
        {

            decimal projecttotalpercent = 0;
            string strstatusupdate = "update Pms_tbl_AddProject set iscompletepercentage='" + projecttotalpercent.ToString() + "' where id='" + projectid + "' and status=1 and archive=0 and isnull(iscomplete,0)=0";
            DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

        }
        // percentage update end  // 



        //project  log details insert start
        SqlParameter[] prmlog = new SqlParameter[8];
        prmlog[0] = new SqlParameter("@Transid", projectid);
        prmlog[1] = new SqlParameter("@Logstatus", statusid);
        prmlog[2] = new SqlParameter("@LogDate", da1.getcdatetime());
        prmlog[3] = new SqlParameter("@Logtype", "project");
        prmlog[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prmlog[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prmlog[6] = new SqlParameter("@cdate", da1.getcdatetime());
        prmlog[7] = new SqlParameter("@type", "Insert");
        da1.executeDMLQuery("pmstbl_Loginsert_usp", prmlog);
        retval = "1";
        // return dscode.GetXml();
        return retval;
    }



    #endregion
    #region bind country and state list
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<countrylist> bindcountrylist()
    {
        string str2 = "select conid id,country countryname  from Imt_Country order by conid";

        DataAccess danew = new DataAccess();
        DataSet drmile = danew.GetDataSetInline(str2);

        List<countrylist> list = new List<countrylist>();

        foreach (DataRow row in drmile.Tables[0].Rows)
        {
            countrylist obj = new countrylist();
            obj.Countryid = row["id"].ToString();
            obj.CountryName = row["countryname"].ToString();
           
            list.Add(obj);
        }

        return list;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<prioritylist> bindprioritylist()
    {
        string str2 = "select id, name, PrintColor as colorname, isnull(isdefault,0) isdefault from Pms_tbl_Priority where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";

        DataAccess danew = new DataAccess();
        DataSet drmile = danew.GetDataSetInline(str2);

        List<prioritylist> list = new List<prioritylist>();

        foreach (DataRow row in drmile.Tables[0].Rows)
        {
            prioritylist obj = new prioritylist();
            obj.id = row["id"].ToString();
            obj.name = row["name"].ToString();
            obj.colorname = row["colorname"].ToString();
            obj.isdefault = row["isdefault"].ToString();
            list.Add(obj);
        }

        return list;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<statelist> bindstatelist(string Countryid)
    {
        string str2 = "select  * from tbl_statemaster  where countryid='" + Countryid + "' and status=1 and archive=0 order by statename";

        DataAccess danew = new DataAccess();
        DataSet drmile = danew.GetDataSetInline(str2);

        List<statelist> list = new List<statelist>();

        foreach (DataRow row in drmile.Tables[0].Rows)
        {
            statelist obj = new statelist();
            obj.Stateid = row["id"].ToString();
            obj.StateName = row["statename"].ToString();

            list.Add(obj);
        }

        return list;
    }

    #endregion
    #region bind statuslist
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<statuslist> bindstatuslist()
    {
        string str2 = "select id, ProjectStatus as name, PrintColor as colorname,PrintIcon as statusicon,isnull(iscomplete,0) as iscomplete, isnull(isdefault,0) isdefault from Pms_tbl_Projectstatus where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";

        DataAccess danew = new DataAccess();
        DataSet drmile = danew.GetDataSetInline(str2);

        List<statuslist> list = new List<statuslist>();

        foreach (DataRow row in drmile.Tables[0].Rows)
        {
            statuslist obj = new statuslist();
            obj.id = row["id"].ToString();
            obj.name = row["name"].ToString();
            obj.colorname = row["colorname"].ToString();
            obj.statusicon = row["statusicon"].ToString();
            obj.iscomplete = row["iscomplete"].ToString();
            obj.isdefault = row["isdefault"].ToString();
            list.Add(obj);
        }

        return list;
    }

    #endregion
    #region update priority
    [WebMethod]
    public static string UpdatePriority_Status(string projectid = "", string priorityid = "")
    {
        string retval = "0";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (projectid != "" && projectid != "0")
        {
            entrytype = "Update";
        }
        try
        {
            SqlParameter[] prm = new SqlParameter[6];
            prm[0] = new SqlParameter("@id", projectid);
            prm[1] = new SqlParameter("@Priority", priorityid);
            prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
            prm[5] = new SqlParameter("@type", entrytype);
            da1.executeDMLQuery("pmstbl_Priority_DeatilsUpdate_usp", prm);
            retval = "1";
        }
        catch { }
        // return dscode.GetXml();
        return retval;

    }
    #endregion
    #region Optimized project list loaders
    private class ProjectLookupCacheItem
    {
        public string Name { get; set; }
        public string Color { get; set; }
    }

    private class ProjectPersonCacheItem
    {
        public string id { get; set; }
        public string name { get; set; }
        public string profileimage { get; set; }
        public string shortname { get; set; }
    }

    private static string SqlSafeValue(string value)
    {
        return (value ?? string.Empty).Replace("'", "''");
    }

    private static string BuildNumericIdList(IEnumerable<string> values)
    {
        HashSet<int> ids = new HashSet<int>();

        if (values != null)
        {
            foreach (string value in values)
            {
                if (string.IsNullOrWhiteSpace(value))
                    continue;

                string[] parts = value.Split(',');
                foreach (string part in parts)
                {
                    int id;
                    if (int.TryParse(part.Trim(), out id) && id > 0)
                        ids.Add(id);
                }
            }
        }

        return string.Join(",", ids.Select(x => x.ToString(CultureInfo.InvariantCulture)).ToArray());
    }

    private static Dictionary<string, string> LoadStringMap(DataAccess da, string sql, string keyColumn, string valueColumn)
    {
        Dictionary<string, string> map = new Dictionary<string, string>();
        DataSet data = da.GetDataSetInline(sql);

        if (data != null && data.Tables.Count > 0)
        {
            foreach (DataRow row in data.Tables[0].Rows)
            {
                string key = row[keyColumn].ToString();
                if (!map.ContainsKey(key))
                    map.Add(key, row[valueColumn].ToString());
            }
        }

        return map;
    }

    private static Dictionary<string, ProjectLookupCacheItem> LoadStatusMap(DataAccess da, string regid)
    {
        Dictionary<string, ProjectLookupCacheItem> map = new Dictionary<string, ProjectLookupCacheItem>();
        string sql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus with(nolock) " +
                     "where status=1 and archive=0 and regid='" + SqlSafeValue(regid) + "'";
        DataSet data = da.GetDataSetInline(sql);

        if (data != null && data.Tables.Count > 0)
        {
            foreach (DataRow row in data.Tables[0].Rows)
            {
                map[row["id"].ToString()] = new ProjectLookupCacheItem
                {
                    Name = row["ProjectStatus"].ToString(),
                    Color = row["PrintColor"].ToString()
                };
            }
        }

        return map;
    }

    private static Dictionary<string, ProjectLookupCacheItem> LoadPriorityMap(DataAccess da, string regid)
    {
        Dictionary<string, ProjectLookupCacheItem> map = new Dictionary<string, ProjectLookupCacheItem>();
        string sql = "select id,name,PrintColor from Pms_tbl_Priority with(nolock) " +
                     "where status=1 and archive=0 and regid='" + SqlSafeValue(regid) + "'";
        DataSet data = da.GetDataSetInline(sql);

        if (data != null && data.Tables.Count > 0)
        {
            foreach (DataRow row in data.Tables[0].Rows)
            {
                map[row["id"].ToString()] = new ProjectLookupCacheItem
                {
                    Name = row["name"].ToString(),
                    Color = row["PrintColor"].ToString()
                };
            }
        }

        return map;
    }

    private static project_summary CreateProjectSummary(int total, int complete)
    {
        int pending = complete > 0 ? Math.Abs(total - complete) : 0;
        double completeRate = total > 0
            ? Math.Round((Convert.ToDouble(complete) / total) * 100, 2)
            : 0;
        double pendingRate = completeRate > 0 ? 100 - completeRate : 100;

        return new project_summary
        {
            total = total,
            complete = complete,
            pending = pending,
            complete_rate = completeRate,
            pending_rate = pendingRate
        };
    }

    private static Dictionary<string, project_summary> LoadSummaryMap(
        DataAccess da,
        string regid,
        string projectIds,
        string tableName,
        string statusColumn,
        string extraWhere)
    {
        Dictionary<string, project_summary> map = new Dictionary<string, project_summary>();
        if (string.IsNullOrEmpty(projectIds))
            return map;

        string sql = "select cast(ProjectId as varchar(50)) ProjectId, count(*) Total, " +
                     "sum(case when isnull(" + statusColumn + ",0) in " +
                     "(select id from Pms_tbl_Projectstatus with(nolock) where regid='" + SqlSafeValue(regid) + "' " +
                     "and status=1 and archive=0 and isnull(iscomplete,0)=1) then 1 else 0 end) Complete " +
                     "from " + tableName + " with(nolock) where regid='" + SqlSafeValue(regid) + "' " +
                     "and status=1 and archive=0 and ProjectId in(" + projectIds + ") " + (extraWhere ?? string.Empty) +
                     " group by ProjectId";

        DataSet data = da.GetDataSetInline(sql);
        if (data != null && data.Tables.Count > 0)
        {
            foreach (DataRow row in data.Tables[0].Rows)
            {
                int total = Convert.ToInt32(row["Total"]);
                int complete = row["Complete"] == DBNull.Value ? 0 : Convert.ToInt32(row["Complete"]);
                map[row["ProjectId"].ToString()] = CreateProjectSummary(total, complete);
            }
        }

        return map;
    }

    private static Dictionary<string, double> LoadCompletionMap(DataAccess da, string projectIds)
    {
        Dictionary<string, double> map = new Dictionary<string, double>();
        if (string.IsNullOrEmpty(projectIds))
            return map;

        string sql = "select id,cast(round(isnull(iscompletepercentage,0),2) as decimal(10,2)) iscompletepercentage " +
                     "from Pms_tbl_AddProject with(nolock) where status=1 and archive=0 and id in(" + projectIds + ")";
        DataSet data = da.GetDataSetInline(sql);

        if (data != null && data.Tables.Count > 0)
        {
            foreach (DataRow row in data.Tables[0].Rows)
                map[row["id"].ToString()] = Convert.ToDouble(row["iscompletepercentage"]);
        }

        return map;
    }

    private static Dictionary<string, ProjectPersonCacheItem> LoadPersonMap(DataAccess da, string regid, string employeeIds)
    {
        Dictionary<string, ProjectPersonCacheItem> map = new Dictionary<string, ProjectPersonCacheItem>();
        if (string.IsNullOrEmpty(employeeIds))
            return map;

        string sql = "select id,DisplayName,fileupload1,SUBSTRING(DisplayName,1,2) splinameone " +
                     "from tbl_employee with(nolock) where status=1 and archive=0 and regid='" +
                     SqlSafeValue(regid) + "' and id in(" + employeeIds + ")";
        DataSet data = da.GetDataSetInline(sql);

        if (data != null && data.Tables.Count > 0)
        {
            foreach (DataRow row in data.Tables[0].Rows)
            {
                map[row["id"].ToString()] = new ProjectPersonCacheItem
                {
                    id = row["id"].ToString(),
                    name = row["DisplayName"].ToString(),
                    profileimage = row["fileupload1"].ToString(),
                    shortname = row["splinameone"].ToString()
                };
            }
        }

        return map;
    }

    private static List<T> CreatePersonList<T>(string ids, Dictionary<string, ProjectPersonCacheItem> personMap)
        where T : IPerson, new()
    {
        List<T> list = new List<T>();
        HashSet<string> added = new HashSet<string>();

        if (string.IsNullOrWhiteSpace(ids))
            return list;

        foreach (string rawId in ids.Split(','))
        {
            string id = rawId.Trim();
            ProjectPersonCacheItem person;
            if (id == string.Empty || added.Contains(id) || !personMap.TryGetValue(id, out person))
                continue;

            T item = new T();
            item.id = person.id;
            item.name = person.name;
            item.profileimage = person.profileimage;
            item.shortname = person.shortname;
            list.Add(item);
            added.Add(id);
        }

        return list;
    }

    private static Dictionary<string, List<taglist>> LoadTagMap(DataAccess da, string projectIds)
    {
        Dictionary<string, List<taglist>> map = new Dictionary<string, List<taglist>>();
        if (string.IsNullOrEmpty(projectIds))
            return map;

        string sql = "select cast(ProjectId as varchar(50)) ProjectId,id,tags,TagsColor " +
                     "from Pms_tbl_Addtags with(nolock) where ProjectId in(" + projectIds + ") order by ProjectId,id asc";
        DataSet data = da.GetDataSetInline(sql);

        if (data != null && data.Tables.Count > 0)
        {
            foreach (DataRow row in data.Tables[0].Rows)
            {
                string projectId = row["ProjectId"].ToString();
                List<taglist> list;
                if (!map.TryGetValue(projectId, out list))
                {
                    list = new List<taglist>();
                    map[projectId] = list;
                }

                list.Add(new taglist
                {
                    id = row["id"].ToString(),
                    tag_name = row["tags"].ToString(),
                    tag_color = row["TagsColor"].ToString()
                });
            }
        }

        return map;
    }

    private static Dictionary<string, List<projectfiles>> LoadProjectFilesMap(DataAccess da, string regid, string projectIds)
    {
        Dictionary<string, List<projectfiles>> map = new Dictionary<string, List<projectfiles>>();
        if (string.IsNullOrEmpty(projectIds))
            return map;

        string safeRegid = SqlSafeValue(regid);
        string sql = @"select emp.DisplayName empname,files.*,
                        case
                            when lower(isnull(files.ftype,''))='milestone' then mile.Milestone
                            when lower(isnull(files.ftype,''))='task' then task.taskname
                            when lower(isnull(files.ftype,''))='subtask' then subtask.subtaskname
                            else project.ProjectName
                        end ftypename
                        from Pms_tbl_projectfiles files with(nolock)
                        left join tbl_employee emp with(nolock) on files.uid=emp.id
                        left join Pms_tbl_AddProject project with(nolock)
                            on cast(project.id as varchar(50))=cast(files.pid as varchar(50)) and project.status=1 and project.archive=0 and project.regid='" + safeRegid + @"'
                        left join Pms_tbl_AddMilestone mile with(nolock)
                            on lower(isnull(files.ftype,''))='milestone' and cast(mile.ProjectId as varchar(50))=cast(files.pid as varchar(50)) and cast(mile.id as varchar(50))=cast(files.fentryid as varchar(50))
                            and mile.status=1 and mile.archive=0 and mile.regid='" + safeRegid + @"'
                        left join Pms_tbl_AddTask task with(nolock)
                            on lower(isnull(files.ftype,''))='task' and cast(task.ProjectId as varchar(50))=cast(files.pid as varchar(50)) and cast(task.id as varchar(50))=cast(files.fentryid as varchar(50))
                            and task.status=1 and task.archive=0 and task.regid='" + safeRegid + @"'
                        left join Pms_tbl_AddSubTask subtask with(nolock)
                            on lower(isnull(files.ftype,''))='subtask' and cast(subtask.ProjectId as varchar(50))=cast(files.pid as varchar(50)) and cast(subtask.id as varchar(50))=cast(files.fentryid as varchar(50))
                            and subtask.status=1 and subtask.archive=0 and subtask.regid='" + safeRegid + @"'
                        where files.regid='" + safeRegid + @"' and files.status=1 and files.archive=0
                        and files.pid in(" + projectIds + @") order by files.pid,files.id desc";

        DataSet data = da.GetDataSetInline(sql);
        if (data != null && data.Tables.Count > 0)
        {
            foreach (DataRow row in data.Tables[0].Rows)
            {
                string projectId = row["pid"].ToString();
                List<projectfiles> list;
                if (!map.TryGetValue(projectId, out list))
                {
                    list = new List<projectfiles>();
                    map[projectId] = list;
                }

                string fentryid = row["fentryid"].ToString();
                list.Add(new projectfiles
                {
                    id = row["id"].ToString(),
                    Uploadby = row["empname"].ToString(),
                    pid = projectId,
                    fentryid = fentryid,
                    ftype = row["ftype"].ToString(),
                    ftypename = row["ftypename"].ToString(),
                    fileurl = row["fileurl"].ToString(),
                    filedes = row["filedes"].ToString(),
                    fext = row["fext"].ToString(),
                    filepath = "/assets/images/" + regid + "/" + fentryid + "/",
                    isshow = true
                });
            }
        }

        return map;
    }

    private static extendedlist CreateExtendedItem(DataRow row)
    {
        DateTime startDate;
        DateTime targetDate;
        if (!DateTime.TryParse(row["StartDate"].ToString(), out startDate) ||
            !DateTime.TryParse(row["TargetDate"].ToString(), out targetDate))
        {
            return new extendedlist
            {
                id = row["id"].ToString(),
                fromdate = row["StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["StartDate"]),
                todate = row["TargetDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["TargetDate"]),
                extended_percent = "100"
            };
        }

        double extendedDays = (targetDate - startDate).TotalDays + 1;
        double leftDays = (targetDate - DateTime.Now).Days + 1;
        string extensionType = leftDays < 0 ? "days delayed" : "days left";

        return new extendedlist
        {
            id = row["id"].ToString(),
            fromdate = startDate,
            todate = targetDate,
            extended_from = startDate.Day + " " + startDate.ToString("MMM", CultureInfo.InvariantCulture),
            extended_to = targetDate.Day + " " + targetDate.ToString("MMM", CultureInfo.InvariantCulture),
            extended_days = extendedDays.ToString(),
            extended_percent = "100",
            extended_type = extensionType,
            delayed_days = leftDays.ToString().Replace("-", "")
        };
    }

    private static Dictionary<string, List<extendedlist>> LoadExtensionMap(DataAccess da, string regid, string projectIds)
    {
        Dictionary<string, List<extendedlist>> map = new Dictionary<string, List<extendedlist>>();
        if (string.IsNullOrEmpty(projectIds))
            return map;

        string sql = "select cast(ProjectID as varchar(50)) ProjectID,id,extendStartdate StartDate,extendEnddate TargetDate " +
                     "from Pms_Tbl_Extended_Timeline with(nolock) where status=1 and Type='Project' and archive=0 " +
                     "and regid='" + SqlSafeValue(regid) + "' and ProjectID in(" + projectIds + ") order by ProjectID,id asc";
        DataSet data = da.GetDataSetInline(sql);

        if (data != null && data.Tables.Count > 0)
        {
            foreach (DataRow row in data.Tables[0].Rows)
            {
                string projectId = row["ProjectID"].ToString();
                List<extendedlist> list;
                if (!map.TryGetValue(projectId, out list))
                {
                    list = new List<extendedlist>();
                    map[projectId] = list;
                }
                list.Add(CreateExtendedItem(row));
            }
        }

        return map;
    }

    private static lastextendeddetails CreateLastExtendedDetails(List<extendedlist> items)
    {
        if (items == null || items.Count == 0)
            return new lastextendeddetails();

        extendedlist item = items[items.Count - 1];
        return new lastextendeddetails
        {
            id = item.id,
            fromdate = item.fromdate,
            todate = item.todate,
            extended_from = item.extended_from,
            extended_to = item.extended_to,
            extended_days = item.extended_days,
            extended_percent = item.extended_percent,
            extended_type = item.extended_type,
            delayed_days = item.delayed_days,
            status = item.status
        };
    }
    #endregion

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<projectdata_details> bindProjectdata(string fromdate = "", string todate = "")
    {
        DataAccess da = new DataAccess();
        string regid = HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString();
        string userId = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();
        DataSet ds;
        List<projectdata_details> projectlist = new List<projectdata_details>();

        int isadmin = 0;
        string adminSql = "select Manage_Profile.id from Manage_Profile with(nolock) " +
                          "inner join tbl_employee with(nolock) on Manage_Profile.id=cast(tbl_employee.roleid as int) " +
                          "where tbl_employee.id='" + SqlSafeValue(userId) + "' and Manage_Profile.status=1 and Manage_Profile.archive=0";
        DataSet adminData = da.GetDataSetInline(adminSql);
        if (adminData != null && adminData.Tables.Count > 0 && adminData.Tables[0].Rows.Count > 0 &&
            adminData.Tables[0].Rows[0]["id"].ToString() == "1")
        {
            isadmin = 1;
        }

        if (isadmin == 1)
        {
            SqlParameter[] prm = new SqlParameter[3];
            prm[0] = new SqlParameter("@regid", regid);
            prm[1] = new SqlParameter("@startdate", fromdate);
            prm[2] = new SqlParameter("@enddate", todate);
            ds = da.GetDataSet("Pms_select_tblprojectall_dashbord_ang", prm);
        }
        else
        {
            SqlParameter[] prm = new SqlParameter[4];
            prm[0] = new SqlParameter("@regid", regid);
            prm[1] = new SqlParameter("@startdate", fromdate);
            prm[2] = new SqlParameter("@enddate", todate);
            prm[3] = new SqlParameter("@owner", userId);
            ds = da.GetDataSet("Pms_select_tblprojectall_dashbord_ownerwise_ang", prm);
        }

        if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            return projectlist;

        DataTable projects = ds.Tables[0];
        List<string> projectIdValues = new List<string>();
        List<string> employeeIdValues = new List<string>();
        List<string> stateIdValues = new List<string>();
        List<string> countryIdValues = new List<string>();
        List<string> clientIdValues = new List<string>();
        List<string> salesOrderIdValues = new List<string>();

        foreach (DataRow projectRow in projects.Rows)
        {
            projectIdValues.Add(projectRow["id"].ToString());
            employeeIdValues.Add(projectRow["Owner"].ToString());
            employeeIdValues.Add(projectRow["team"].ToString());
            stateIdValues.Add(projectRow["StateName"].ToString());
            countryIdValues.Add(projectRow["Countryid"].ToString());
            clientIdValues.Add(projectRow["customerid"].ToString());
            salesOrderIdValues.Add(projectRow["salesorderid"].ToString());
        }

        string projectIds = BuildNumericIdList(projectIdValues);
        string employeeIds = BuildNumericIdList(employeeIdValues);
        string stateIds = BuildNumericIdList(stateIdValues);
        string countryIds = BuildNumericIdList(countryIdValues);
        string clientIds = BuildNumericIdList(clientIdValues);
        string salesOrderIds = BuildNumericIdList(salesOrderIdValues);

        // Bulk-load request data once. Each loader is isolated so one schema/data difference
        // can NEVER break the whole project grid. If a bulk loader fails, the existing
        // per-project helper is used only for that part below.
        Dictionary<string, ProjectLookupCacheItem> statusMap = new Dictionary<string, ProjectLookupCacheItem>();
        Dictionary<string, ProjectLookupCacheItem> priorityMap = new Dictionary<string, ProjectLookupCacheItem>();
        Dictionary<string, ProjectPersonCacheItem> personMap = new Dictionary<string, ProjectPersonCacheItem>();
        Dictionary<string, List<taglist>> tagMap = new Dictionary<string, List<taglist>>();
        Dictionary<string, double> completionMap = new Dictionary<string, double>();
        Dictionary<string, project_summary> milestoneMap = new Dictionary<string, project_summary>();
        Dictionary<string, project_summary> taskMap = new Dictionary<string, project_summary>();
        Dictionary<string, project_summary> issueMap = new Dictionary<string, project_summary>();
        Dictionary<string, List<projectfiles>> fileMap = new Dictionary<string, List<projectfiles>>();
        Dictionary<string, List<extendedlist>> extensionMap = new Dictionary<string, List<extendedlist>>();
        Dictionary<string, string> stateMap = new Dictionary<string, string>();
        Dictionary<string, string> countryMap = new Dictionary<string, string>();
        Dictionary<string, string> clientMap = new Dictionary<string, string>();
        Dictionary<string, string> estimateMap = new Dictionary<string, string>();

        try { statusMap = LoadStatusMap(da, regid); }
        catch (Exception ex) { SaveLogText("bindProjectdata LoadStatusMap-" + ex.Message); }

        try { priorityMap = LoadPriorityMap(da, regid); }
        catch (Exception ex) { SaveLogText("bindProjectdata LoadPriorityMap-" + ex.Message); }

        try { personMap = LoadPersonMap(da, regid, employeeIds); }
        catch (Exception ex) { SaveLogText("bindProjectdata LoadPersonMap-" + ex.Message); }

        try { tagMap = LoadTagMap(da, projectIds); }
        catch (Exception ex) { SaveLogText("bindProjectdata LoadTagMap-" + ex.Message); }

        try { completionMap = LoadCompletionMap(da, projectIds); }
        catch (Exception ex) { SaveLogText("bindProjectdata LoadCompletionMap-" + ex.Message); }

        try { milestoneMap = LoadSummaryMap(da, regid, projectIds, "Pms_tbl_AddMilestone", "P_Status", string.Empty); }
        catch (Exception ex) { SaveLogText("bindProjectdata LoadMilestoneMap-" + ex.Message); }

        try { taskMap = LoadSummaryMap(da, regid, projectIds, "Pms_tbl_AddTask", "P_Status", string.Empty); }
        catch (Exception ex) { SaveLogText("bindProjectdata LoadTaskMap-" + ex.Message); }

        try { issueMap = LoadSummaryMap(da, regid, projectIds, "Pms_tbl_AddIssues", "IssueStatus", " and Issueon='Project' "); }
        catch (Exception ex) { SaveLogText("bindProjectdata LoadIssueMap-" + ex.Message); }

        try { fileMap = LoadProjectFilesMap(da, regid, projectIds); }
        catch (Exception ex) { SaveLogText("bindProjectdata LoadFileMap-" + ex.Message); }

        try { extensionMap = LoadExtensionMap(da, regid, projectIds); }
        catch (Exception ex) { SaveLogText("bindProjectdata LoadExtensionMap-" + ex.Message); }

        if (!string.IsNullOrEmpty(stateIds))
        {
            try
            {
                stateMap = LoadStringMap(da,
                    "select id,statename from tbl_statemaster with(nolock) where status=1 and archive=0 and id in(" + stateIds + ")",
                    "id", "statename");
            }
            catch (Exception ex) { SaveLogText("bindProjectdata LoadStateMap-" + ex.Message); }
        }

        if (!string.IsNullOrEmpty(countryIds))
        {
            try
            {
                countryMap = LoadStringMap(da,
                    "select conid,country from Imt_Country with(nolock) where status=1 and archive=0 and conid in(" + countryIds + ")",
                    "conid", "country");
            }
            catch (Exception ex) { SaveLogText("bindProjectdata LoadCountryMap-" + ex.Message); }
        }

        if (!string.IsNullOrEmpty(clientIds))
        {
            try
            {
                clientMap = LoadStringMap(da,
                    "select id,txtcompanyname from tbl_contactsdetails with(nolock) where regid='" + SqlSafeValue(regid) +
                    "' and status=1 and archive=0 and id in(" + clientIds + ")",
                    "id", "txtcompanyname");
            }
            catch (Exception ex) { SaveLogText("bindProjectdata LoadClientMap-" + ex.Message); }
        }

        if (!string.IsNullOrEmpty(salesOrderIds))
        {
            try
            {
                estimateMap = LoadStringMap(da,
                    "select id,Estimateno from tbl_SalesOrderdetails with(nolock) where regid='" + SqlSafeValue(regid) +
                    "' and status=1 and archive=0 and id in(" + salesOrderIds + ")",
                    "id", "Estimateno");
            }
            catch (Exception ex) { SaveLogText("bindProjectdata LoadEstimateMap-" + ex.Message); }
        }

        foreach (DataRow row in projects.Rows)
        {
            string projectId = row["id"].ToString();
            string statusId = row["ProjectStatus"].ToString();
            string priorityId = row["Priority"].ToString();
            string stateId = row["StateName"].ToString();
            string countryId = row["Countryid"].ToString();
            string clientId = row["customerid"].ToString();
            string salesOrderId = row["salesorderid"].ToString();

            ProjectLookupCacheItem statusInfo;
            ProjectLookupCacheItem priorityInfo;
            statusMap.TryGetValue(statusId, out statusInfo);
            priorityMap.TryGetValue(priorityId, out priorityInfo);

            string stateName = stateMap.ContainsKey(stateId) ? stateMap[stateId] : get_statename(stateId);
            string countryName = countryMap.ContainsKey(countryId) ? countryMap[countryId] : get_Countryname(countryId);
            string clientName = clientMap.ContainsKey(clientId) ? clientMap[clientId] : get_clientname(clientId);
            string estimateNo = estimateMap.ContainsKey(salesOrderId) ? estimateMap[salesOrderId] : get_estimateno(salesOrderId);

            project_summary milestoneSummary;
            project_summary taskSummary;
            project_summary issueSummary;
            if (!milestoneMap.TryGetValue(projectId, out milestoneSummary)) milestoneSummary = getproject_milestone_details(projectId);
            if (!taskMap.TryGetValue(projectId, out taskSummary)) taskSummary = getproject_tasks_details(projectId);
            if (!issueMap.TryGetValue(projectId, out issueSummary)) issueSummary = getproject_issues_details(projectId);

            double completeValue = completionMap.ContainsKey(projectId) ? completionMap[projectId] : get_completionstatus(projectId);
            double pendingValue = completeValue > 0 ? 100 - completeValue : 100;

            List<taglist> tags;
            List<projectfiles> files;
            List<extendedlist> extensions;
            if (!tagMap.TryGetValue(projectId, out tags)) tags = gete_taglist(projectId);
            if (!fileMap.TryGetValue(projectId, out files)) files = bindfiledata(projectId, "project");
            if (!extensionMap.TryGetValue(projectId, out extensions)) extensions = get_extended_data(projectId);
            lastextendeddetails latestExtension = CreateLastExtendedDetails(extensions);

            string completeAddress = row["ProjectAddress1"].ToString();
            string address2 = row["ProjectAddress2"].ToString();
            if (!string.IsNullOrEmpty(address2))
                completeAddress = string.IsNullOrEmpty(completeAddress) ? address2 : completeAddress + " " + address2;

            string location = row["City"].ToString() + " " + row["PostalCode"].ToString() + " " + stateName + " " + countryName;
            completeAddress = string.IsNullOrEmpty(completeAddress) ? location : completeAddress + "</br>" + location;

            List<ownerlist> ownerItems = CreatePersonList<ownerlist>(row["Owner"].ToString(), personMap);
            if (ownerItems.Count == 0 && !string.IsNullOrWhiteSpace(row["Owner"].ToString()) && row["Owner"].ToString() != "0")
                ownerItems = getemployeelist<ownerlist>(row["Owner"].ToString());

            List<teamlist> teamItems = CreatePersonList<teamlist>(row["team"].ToString(), personMap);
            if (teamItems.Count == 0 && !string.IsNullOrWhiteSpace(row["team"].ToString()) && row["team"].ToString() != "0")
                teamItems = getemployeelist<teamlist>(row["team"].ToString());

            projectlist.Add(new projectdata_details
            {
                id = projectId,
                ProjectID = row["ProjectID"].ToString(),
                ProjectName = row["ProjectName"].ToString(),
                Owner = row["Owner"].ToString(),
                team = row["team"].ToString(),
                ProjectStatus = statusId,
                status_name = string.IsNullOrEmpty(statusId) || statusId == "0" ? "-" : (statusInfo == null ? "-" : statusInfo.Name),
                status_color = string.IsNullOrEmpty(statusId) || statusId == "0" ? "-" : (statusInfo == null ? string.Empty : statusInfo.Color),
                Priority = priorityId,
                priority_name = string.IsNullOrEmpty(priorityId) || priorityId == "0" ? "-" : (priorityInfo == null ? "-" : priorityInfo.Name),
                priority_color = string.IsNullOrEmpty(priorityId) || priorityId == "0" ? "-" : (priorityInfo == null ? string.Empty : priorityInfo.Color),
                ProjectAddress1 = row["ProjectAddress1"].ToString(),
                ProjectAddress2 = address2,
                Completeaddress = completeAddress,
                City = row["City"].ToString(),
                CityName = row["City"].ToString(),
                PostalCode = row["PostalCode"].ToString(),
                StateName = stateId,
                StateName2 = stateName,
                Countryid = countryId,
                CountryName = countryName,
                Budget = row["Budget"].ToString(),
                Actual = row["Actual"].ToString(),
                AwardDate = row["AwardDate"] as DateTime?,
                StartDate = row["StartDate"] as DateTime?,
                TargetDate = row["TargetDate"] as DateTime?,
                ApprovalDrawing = row["ApprovalDrawing"].ToString(),
                DrawingApprovalDate = row["DrawingApprovalDate"] as DateTime?,
                Uploaddrawing = row["Uploaddrawing"].ToString(),
                Weightage = row["Weightage"].ToString(),
                isdrawing = row["isdrawing"].ToString(),
                checkprojectWeightage = row["checkprojectWeightage"].ToString(),
                customerid = clientId,
                ClientName = clientName,
                salesorderid = salesOrderId,
                Estimateno = estimateNo,
                hastask = row["hastask"].ToString(),
                checktaskWeightage = row["checktaskWeightage"].ToString(),
                hassubtask = row["hassubtask"].ToString(),
                checksubtaskWeightage = row["checksubtaskWeightage"].ToString(),
                leftdays = get_tdays("left", row["StartDate"].ToString(), row["TargetDate"].ToString()),
                txtdays = get_tdays("total", row["StartDate"].ToString(), row["TargetDate"].ToString()),
                iscountholydays = row["iscountholydays"].ToString() == "" ? "0" : row["iscountholydays"].ToString(),
                iscountweekoff = row["iscountweekoff"].ToString() == "" ? "0" : row["iscountweekoff"].ToString(),
                complete_value = Math.Round(completeValue, 2).ToString(),
                pending_value = Math.Round(pendingValue, 2).ToString(),
                totalmile = milestoneSummary.total.ToString(),
                completemile = milestoneSummary.complete.ToString(),
                completemile_rate = milestoneSummary.complete_rate.ToString(),
                pendingmile = milestoneSummary.pending.ToString(),
                pendingmile_rate = milestoneSummary.pending_rate.ToString(),
                totaltask = taskSummary.total.ToString(),
                completetask = taskSummary.complete.ToString(),
                completetask_rate = taskSummary.complete_rate.ToString(),
                pendingtask = taskSummary.pending.ToString(),
                pendingtask_rate = taskSummary.pending_rate.ToString(),
                totaldependency = "0",
                totalissue = issueSummary.total.ToString(),
                completeissue = issueSummary.complete.ToString(),
                completeissue_rate = issueSummary.complete_rate.ToString(),
                pendingissue = issueSummary.pending.ToString(),
                pendingissue_rate = issueSummary.pending_rate.ToString(),
                tags = tags,
                owners = ownerItems,
                teams = teamItems,
                // Preserve existing combined/inherited project team behavior.
                teams_all = getemployeelist_all<teamlist>(projectId),
                viewfiles = files,
                lastextendstartdate = latestExtension.fromdate,
                lastextendenddate = latestExtension.todate,
                extendedlists = extensions
            });
        }

        return projectlist;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static projectdata_details getorojectdetails(string id)
    {

        string projectdata = "";
        function fns = new function();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();

        DataSet ds = new DataSet();
        projectdata_details projectlist = new  projectdata_details();
        int isadmin = 0;
        

        
            SqlParameter[] prm = new SqlParameter[2];
            prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[1] = new SqlParameter("@pid", id);
            ds = da.GetDataSet("Pms_select_tblprojectall_byid_ang", prm);


        if (ds.Tables[0].Rows.Count > 0)
        {


            string streditP = "0";
            string strdeleteP = "0";
            string strownerP = "0";

            string strviewM = "0";
            string strviewT = "0";
            string strviewI = "0";
            string strviewD = "0";


            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int total_dependency = 0;
                try
                {

                }
                catch { }
                List<taglist> newtaglist_p = gete_taglist(ds.Tables[0].Rows[i]["id"].ToString());
                List<ownerlist> newownerlist_p = getemployeelist<ownerlist>(ds.Tables[0].Rows[i]["Owner"].ToString());
                List<teamlist> newteamlist_p = getemployeelist<teamlist>(ds.Tables[0].Rows[i]["team"].ToString());
                double mileTaskTotal = 0;
                mileTaskTotal = get_completionstatus(ds.Tables[0].Rows[i]["id"].ToString());
                //SaveLogText("chkadmintodayEX-" + mileTaskTotal);
                double mileWattagecomplite = mileTaskTotal;
                double mileWattagepending = 100;
                if (mileTaskTotal > 0)
                {
                    mileWattagepending = mileWattagepending - mileTaskTotal;
                }
                string strsddress = ds.Tables[0].Rows[i]["ProjectAddress1"].ToString();
                if (ds.Tables[0].Rows[i]["ProjectAddress2"].ToString() != string.Empty)
                {
                    if (strsddress != string.Empty)
                    {
                        strsddress += " " + ds.Tables[0].Rows[i]["ProjectAddress2"].ToString();
                    }
                    else
                    {
                        strsddress += ds.Tables[0].Rows[i]["ProjectAddress2"].ToString();
                    }
                }
                if (strsddress != string.Empty)
                {
                    strsddress += "</br>" + ds.Tables[0].Rows[i]["City"].ToString() + " " + ds.Tables[0].Rows[i]["PostalCode"].ToString() + " " + get_statename(ds.Tables[0].Rows[i]["StateName"].ToString()) + " " + get_Countryname(ds.Tables[0].Rows[i]["Countryid"].ToString());
                }
                else
                {
                    strsddress += ds.Tables[0].Rows[i]["City"].ToString() + " " + ds.Tables[0].Rows[i]["PostalCode"].ToString() + " " + get_statename(ds.Tables[0].Rows[i]["StateName"].ToString()) + " " + get_Countryname(ds.Tables[0].Rows[i]["Countryid"].ToString());
                }
                List<projectfiles> newfileslist = bindfiledata(ds.Tables[0].Rows[i]["id"].ToString(), "project");
                project_summary _sumarryM = getproject_milestone_details(ds.Tables[0].Rows[i]["id"].ToString());
                project_summary _sumarryT = getproject_tasks_details(ds.Tables[0].Rows[i]["id"].ToString());
                project_summary _sumarryI = getproject_issues_details(ds.Tables[0].Rows[i]["id"].ToString());
                List<extendedlist> extendlist = get_extended_data(ds.Tables[0].Rows[i]["id"].ToString());


                projectlist.id = ds.Tables[0].Rows[i]["id"].ToString();
                projectlist.ProjectID = ds.Tables[0].Rows[i]["ProjectID"].ToString();
                projectlist.ProjectName = ds.Tables[0].Rows[i]["ProjectName"].ToString();
                projectlist.Owner = ds.Tables[0].Rows[i]["Owner"].ToString();
                projectlist.team = ds.Tables[0].Rows[i]["team"].ToString();

                projectlist.ProjectStatus = ds.Tables[0].Rows[i]["ProjectStatus"].ToString();
                projectlist.status_name = get_Projectstatus(ds.Tables[0].Rows[i]["ProjectStatus"].ToString());
                projectlist.status_color = get_Status_Color(ds.Tables[0].Rows[i]["ProjectStatus"].ToString());
                projectlist.Priority = ds.Tables[0].Rows[i]["Priority"].ToString();
                    projectlist.priority_name = get_PriorityName(ds.Tables[0].Rows[i]["Priority"].ToString());
                projectlist.priority_color = get_PriorityColor(ds.Tables[0].Rows[i]["Priority"].ToString());
                projectlist.ProjectAddress1 = ds.Tables[0].Rows[i]["ProjectAddress1"].ToString();
                projectlist.ProjectAddress2 = ds.Tables[0].Rows[i]["ProjectAddress2"].ToString();
                projectlist.Completeaddress = strsddress;
                projectlist.City = ds.Tables[0].Rows[i]["City"].ToString();
                projectlist.CityName = ds.Tables[0].Rows[i]["City"].ToString();
                projectlist.PostalCode = ds.Tables[0].Rows[i]["PostalCode"].ToString();
                projectlist.StateName = ds.Tables[0].Rows[i]["StateName"].ToString();
                projectlist.StateName2 = get_statename(ds.Tables[0].Rows[i]["StateName"].ToString());
                projectlist.Countryid = ds.Tables[0].Rows[i]["Countryid"].ToString();
                projectlist.CountryName = get_Countryname(ds.Tables[0].Rows[i]["Countryid"].ToString());
                projectlist.Budget = ds.Tables[0].Rows[i]["Budget"].ToString();
                projectlist.Actual = ds.Tables[0].Rows[i]["Actual"].ToString();
                projectlist.AwardDate = ds.Tables[0].Rows[i]["AwardDate"] as DateTime?;
                projectlist.StartDate = ds.Tables[0].Rows[i]["StartDate"] as DateTime?;
                projectlist.TargetDate = ds.Tables[0].Rows[i]["TargetDate"] as DateTime?;
                projectlist.ApprovalDrawing = ds.Tables[0].Rows[i]["ApprovalDrawing"].ToString();
                projectlist.DrawingApprovalDate = ds.Tables[0].Rows[i]["DrawingApprovalDate"] as DateTime?;
                projectlist.Uploaddrawing = ds.Tables[0].Rows[i]["Uploaddrawing"].ToString();
                projectlist.Weightage = ds.Tables[0].Rows[i]["Weightage"].ToString();
                projectlist.isdrawing = ds.Tables[0].Rows[i]["isdrawing"].ToString();
                projectlist.checkprojectWeightage = ds.Tables[0].Rows[i]["checkprojectWeightage"].ToString();
                projectlist.customerid = ds.Tables[0].Rows[i]["customerid"].ToString();
                projectlist.ClientName = get_clientname(ds.Tables[0].Rows[i]["customerid"].ToString());
                projectlist.salesorderid = ds.Tables[0].Rows[i]["salesorderid"].ToString();
                projectlist.Estimateno = get_estimateno(ds.Tables[0].Rows[i]["salesorderid"].ToString());
                projectlist.hastask = ds.Tables[0].Rows[i]["hastask"].ToString();
                    projectlist.checktaskWeightage = ds.Tables[0].Rows[i]["checktaskWeightage"].ToString();
                projectlist.hassubtask = ds.Tables[0].Rows[i]["hassubtask"].ToString();
                projectlist.checksubtaskWeightage = ds.Tables[0].Rows[i]["checksubtaskWeightage"].ToString();
                projectlist.leftdays = get_tdays("left", ds.Tables[0].Rows[i]["StartDate"].ToString(), ds.Tables[0].Rows[i]["TargetDate"].ToString());
                projectlist.txtdays = get_tdays("total", ds.Tables[0].Rows[i]["StartDate"].ToString(), ds.Tables[0].Rows[i]["TargetDate"].ToString());

                projectlist.iscountholydays = (ds.Tables[0].Rows[i]["iscountholydays"].ToString() == "" ? "0" : ds.Tables[0].Rows[i]["iscountholydays"].ToString());
                projectlist.iscountweekoff = (ds.Tables[0].Rows[i]["iscountweekoff"].ToString() == "" ? "0" : ds.Tables[0].Rows[i]["iscountweekoff"].ToString());


                projectlist.complete_value = Math.Round(mileTaskTotal, 2).ToString();
                projectlist.pending_value = Math.Round(mileWattagepending, 2).ToString();
                projectlist.totalmile = _sumarryM.total.ToString();
                projectlist.completemile = _sumarryM.complete.ToString();
                projectlist.completemile_rate = _sumarryM.complete_rate.ToString();
                projectlist.pendingmile = _sumarryM.pending.ToString();
                projectlist.pendingmile_rate = _sumarryM.pending_rate.ToString();

                projectlist.totaltask = _sumarryT.total.ToString();
                projectlist.completetask = _sumarryT.complete.ToString();
                projectlist.completetask_rate = _sumarryT.complete_rate.ToString();
                projectlist.pendingtask = _sumarryT.pending.ToString();
                projectlist.pendingtask_rate = _sumarryT.pending_rate.ToString();

                projectlist.totaldependency = total_dependency.ToString();
                projectlist.totalissue = _sumarryI.total.ToString();
                projectlist.completeissue = _sumarryI.complete.ToString();
                projectlist.completeissue_rate = _sumarryI.complete_rate.ToString();
                projectlist.pendingissue = _sumarryI.pending.ToString();
                projectlist.pendingissue_rate = _sumarryI.pending_rate.ToString();
                projectlist.tags = newtaglist_p;
                projectlist.owners = newownerlist_p;
                    projectlist.teams = newteamlist_p;
                    projectlist.viewfiles = newfileslist;
                    projectlist.extendedlists = extendlist;

                
            }


        }

        return projectlist;
    }


    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<taglist> bindtaglist()
    {

        List<taglist> list = new List<taglist>();
        DataAccess da = new DataAccess();
        string strtags = "select id, tags,TagsColor from   Pms_tbl_Addtags where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  order by id asc";
        DataSet dstags = da.GetDataSetInline(strtags);

        if (dstags != null && dstags.Tables.Count > 0 && dstags.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow row in dstags.Tables[0].Rows)
            {
                list.Add(new taglist
                {
                    id = row["id"].ToString(),
                    tag_name = row["tags"].ToString(),
                    tag_color = row["TagsColor"].ToString()
                });
            }
        }

        return list;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<projectfiles> bindprojectfileslist(string id)
    {

        DataAccess da = new DataAccess();
        string retval = "";
        string strsqlfils = "select * from Pms_tbl_projectfiles where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and status=1 and archive=0 and pid='" + id + "'  order by id desc";
        //if(isall=="1")
        //{
        // strsqlfils = "select * from Pms_tbl_projectfiles where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and status=1 and archive=0 and pid='" + id + "' order by id desc";
        //}
        DataSet dsfils = da.GetDataSetInline(strsqlfils);
        string strdatafils = "";
        int countrow = 0;

        List<projectfiles> newprojectfiles = new List<projectfiles>();
        if (dsfils.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dsfils.Tables[0].Rows.Count; i++)
            {
                string filepath = "";
                string ftype = dsfils.Tables[0].Rows[i]["ftype"].ToString();
                string fentryid = dsfils.Tables[0].Rows[i]["fentryid"].ToString();
                string strtypename = "";
                int isshowf = 0;
                if (ftype != string.Empty)
                {
                    strtypename = get_filetypename(id, fentryid, ftype);
                    isshowf = 1;
                    if (ftype == "project")
                    {
                        filepath = "/assets/images/" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "/" + fentryid + "/";
                    }
                    else if (ftype == "milestone")
                    {
                        filepath = "/pms/MileImages/" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "/" + fentryid + "/";
                    }
                    else if (ftype == "task")
                    {
                        filepath = "/pms/TaskImages/" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "/" + fentryid + "/";
                    }
                    else if (ftype == "subtask")
                    {
                        filepath = "/pms/SubtaskImages/" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "/" + fentryid + "/";
                    }

                }
                newprojectfiles.Add(new projectfiles
                {

                    id = dsfils.Tables[0].Rows[i]["id"].ToString(),
                    pid = dsfils.Tables[0].Rows[i]["pid"].ToString(),
                    fentryid = dsfils.Tables[0].Rows[i]["fentryid"].ToString(),
                    ftype = dsfils.Tables[0].Rows[i]["ftype"].ToString(),
                    ftypename = strtypename,
                    fileurl = dsfils.Tables[0].Rows[i]["fileurl"].ToString(),
                    filedes = dsfils.Tables[0].Rows[i]["filedes"].ToString(),
                    fext = dsfils.Tables[0].Rows[i]["fext"].ToString(),
                    filepath = filepath,
                    isshow = (isshowf == 1 ? true : false)


                });

            }

        }


        return newprojectfiles;
    }
   

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string updateOwner(string id, string ownerid)
    {
        string retval = "0";
        DataAccess da1 = new DataAccess();
        if (id != string.Empty && id != "0")
        {


            try
            {
                string strownerupdate = "update Pms_tbl_AddProject set owner='" + ownerid + "' where id='" + id + "' and status=1 and archive=0 ";

                da1.executeDMLQueryinline(strownerupdate);
                retval = "1";
            }
            catch { }


        }
        return retval;
    }
    [WebMethod]
    public static string removeteambyid(projectteams projectteams)
    {
        string retval = "";
        DataAccess da = new DataAccess();
        if (projectteams.id != null)
        {

            try
            {
                string strsqlown = "select id,team from Pms_tbl_Addproject where status=1 and archive=0 and regid='"
                + HttpContext.Current.Request.Cookies["cadmin"]["id"] +
                "' and id='" + projectteams.id + "' ";

                DataSet dsown = da.GetDataSetInline(strsqlown);

                if (dsown.Tables[0].Rows.Count > 0)
                {
                    string teamStr = dsown.Tables[0].Rows[0]["team"].ToString();

                    if (!string.IsNullOrEmpty(teamStr))
                    {
                        // Split
                        List<string> teams = teamStr.Split(',').ToList();

                        // Remove matching ID
                        teams.Remove(projectteams.Team.ToString());

                        // Join back
                        string updatedteams = string.Join(",", teams);

                        // Update DB
                        string updateQuery = "update Pms_tbl_Addproject set team='" + updatedteams +
                                             "' where id='" + projectteams.id + "'";

                        da.executeDMLQueryinline(updateQuery);
                        retval = "1";

                    }
                }
            }
            catch { }
        }


        return retval;
    }
    [WebMethod]
    public static string removecumulativeteam(projectteams projectteams)
    {
        string retval = "";
        DataAccess da = new DataAccess();
        if (projectteams.id != null)
        {

            try
            {
                SqlParameter[] prm = new SqlParameter[4];
                prm[0] = new SqlParameter("@ProjectId", projectteams.id);
                prm[1] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prm[2] = new SqlParameter("@TeamId", projectteams.Team);
                prm[3] = new SqlParameter("ReturnValue", SqlDbType.Int);
                prm[3].Direction = ParameterDirection.ReturnValue;
                int a= da.executeDMLQuery("pms_removeproject_com_team", prm);
                if(a>0)
                {
                    retval = "1";
                }
            }
            catch { }
        }


        return retval;
    }
    [WebMethod]
    public static string UpdateP_Team(string id = "", string ownerids = "")
    {
        string retval = "0";
        try
        {
            DataAccess da1 = new DataAccess();
            string finsql4 = "update Pms_tbl_Addproject set Team='" + ownerids + "' where id='" + id + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da1.GetDataSetInline(finsql4);
            retval = "1";
        }
        catch { }
        return retval;
    }


    #region mange issues

    [WebMethod]
    public static project_summary bind_projectdetails(string id, string type)
    {
        DataAccess da = new DataAccess();
        project_summary _sumarry = new project_summary();

        try
        {
            if (string.IsNullOrEmpty(id))
                return _sumarry;

            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null)
                return _sumarry;


            string regid = regCookie["id"];
            int total = 0;
            int complete = 0;
            int pending = 0;
            double complete_rates = 0;
            double pending_rates = 100;


            if (type == "milestone")
            {
                _sumarry = getproject_milestone_details(id);

            }
            else if (type == "task")
            {
                _sumarry = getproject_tasks_details(id);

            }
            else if (type == "issue")
            {
                _sumarry = getproject_issues_details(id);

            }

            double mileTaskTotal = 0;
            mileTaskTotal = get_completionstatus(id);
            double mileWattagecomplite = mileTaskTotal;
            double mileWattagepending = 100;
            if (mileTaskTotal > 0)
            {
                mileWattagepending = mileWattagepending - mileTaskTotal;
            }
            _sumarry.pcopmelete_value = mileTaskTotal;
            _sumarry.ppending_value = mileWattagepending;

        }
        catch
        {

            return new project_summary();
        }


        return _sumarry;
    }


    #endregion
    #region manage dependency
    public static int Binddependencydata(string id, string utype)
    {
        int retval = 0;
        DataAccess da = new DataAccess();
        string strD = "";
        if (utype == "milestone")
        {
            strD = " and isnull(milestoneid,0)='" + id + "'";
        }
        else if (utype == "task")
        {
            strD = " and isnull(taskid,0)='" + id + "'";
        }
        else if (utype == "subtask")
        {
            strD = " and isnull(subtaskid,0)='" + id + "'";
        }
        string strsql = "select * from tbl_dependencyST where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        // SaveLogText("getdependencydetails-"+ strsql);
        if (strD != "")
        {
            strsql += strD;
            DataSet dsD = da.GetDataSetInline(strsql);
            if (dsD.Tables[0].Rows.Count > 0)
            {
                retval = getdependencydetails_count(dsD.Tables[0].Rows[0]["id"].ToString());
            }
        }
        return retval;
    }
    public static int getdependencydetails_count(string ids)
    {
        int retval = 0;
        DataAccess da = new DataAccess();
        string strdependency = "";

        try
        {

            string str_selecteddeatil = "";
            if (ids != "")
            {
                SqlParameter[] prm = new SqlParameter[2];
                prm[0] = new SqlParameter("@id", ids);
                prm[1] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                DataSet dsD = da.GetDataSet("sp_dependencydetails", prm);
                if (dsD.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < dsD.Tables[0].Rows.Count; i++)
                    {
                        int chkallcomplete = 0;

                        if (dsD.Tables[0].Rows[i]["mid"].ToString() != "" && dsD.Tables[0].Rows[i]["mid"].ToString() != "0")
                        {
                            try
                            {
                                string stsqlM = "select isnull(statuscomplete,0) statuscomplete from Pms_tbl_AddMilestone where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + dsD.Tables[0].Rows[i]["mid"].ToString() + "'";
                                DataSet dschkM = da.GetDataSetInline(stsqlM);
                                if (dschkM.Tables[0].Rows[0]["statuscomplete"].ToString() != "1")
                                {
                                    chkallcomplete = 1;
                                }

                            }
                            catch { }
                        }

                        if (dsD.Tables[0].Rows[i]["tid"].ToString() != "" && dsD.Tables[0].Rows[i]["tid"].ToString() != "0")
                        {
                            try
                            {
                                string stsqlM = "select isnull(statuscomplete,0) statuscomplete from Pms_tbl_AddTask where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + dsD.Tables[0].Rows[i]["tid"].ToString() + "'";
                                DataSet dschkM = da.GetDataSetInline(stsqlM);
                                if (dschkM.Tables[0].Rows[0]["statuscomplete"].ToString() != "1")
                                {
                                    chkallcomplete = 1;
                                }

                            }
                            catch { }
                        }


                        if (dsD.Tables[0].Rows[i]["stid"].ToString() != "" && dsD.Tables[0].Rows[i]["stid"].ToString() != "0")
                        {
                            try
                            {
                                string stsqlM = "select isnull(statuscomplete,0) statuscomplete from Pms_tbl_AddSubtask where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + dsD.Tables[0].Rows[i]["stid"].ToString() + "'";

                                DataSet dschkM = da.GetDataSetInline(stsqlM);
                                if (dschkM.Tables[0].Rows[0]["statuscomplete"].ToString() != "1")
                                {
                                    chkallcomplete = 1;
                                }


                            }
                            catch (Exception ex)
                            {
                                //SaveLogText("strbgc_s2-" + ex.Message);
                            }
                        }
                        if (chkallcomplete == 1)
                        {
                            retval++;
                        }
                    }
                }

            }


        }
        catch (Exception ex)
        {

        }
        return retval;
    }
    [WebMethod]
    public static string binddependency_details(string id, string type)
    {
        string retval = "0";
        DataAccess da = new DataAccess();


        try
        {
            if (string.IsNullOrEmpty(id))
                return retval;

            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];


            string regid = regCookie["id"];

            #region find dependency
            try
            {

                retval = Binddependencydata(id, type).ToString();

            }
            catch
            {

            }
            #endregion


        }
        catch
        {


        }


        return retval;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static project_summary projectsummarydetails(string id, string type)
    {

        DataAccess da = new DataAccess();
        project_summary _sumarry = new project_summary();
        //string strsql = "select * from Pms_tbl_projectfiles where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and status=1 and archive=0 and pid='" + id + "'  order by id desc";
        string strsql = "";
        try
        {
            if (type == "milestone")
            {
                _sumarry = getproject_milestone_details(id);
            }
            else if (type == "task")
            {
                _sumarry = getproject_tasks_details(id);
            }
            else if (type == "issue")
            {
                _sumarry = getproject_issues_details(id);
            }
            else if (type == "dependancy")
            {

            }
            if (strsql != string.Empty)
            {
                DataSet ds = da.GetDataSetInline(strsql);

            }
        }
        catch { }
        return _sumarry;
    }
    public static project_summary getproject_milestone_details(string pid)
    {
        DataAccess da = new DataAccess();
        int total = 0;
        int complete = 0;
        int pending = 0;
        double complete_rate = 0;
        double pending_rate = 0;

        if (string.IsNullOrEmpty(pid) || pid == "0")
            return new project_summary();


        #region total milestone
        string str1 = "select COUNT(*) as countmilstone from Pms_tbl_AddMilestone where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and ProjectId='" + pid + "' and status=1 and archive=0 ";
        DataSet ds1 = da.GetDataSetInline(str1);
        if (ds1.Tables[0].Rows.Count > 0)
        {
            try
            {
                total = Convert.ToInt32(ds1.Tables[0].Rows[0]["countmilstone"]);
            }
            catch (Exception)
            {
            }
        }
        #endregion
        #region complete milestone
        string str2 = "select COUNT(*) as ctotal  from Pms_tbl_AddMilestone where ProjectId='" + pid + "'  and P_Status in ( select id from Pms_tbl_Projectstatus where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and   regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and status=1 and archive=0 and isnull(iscomplete,0)=1)  and status=1 and archive=0";
        DataSet ds2 = da.GetDataSetInline(str2);
        try
        {
            complete = Convert.ToInt32(ds2.Tables[0].Rows[0]["ctotal"]);
        }
        catch
        {
        }
        #endregion
        #region pending
        if (complete > 0)
        {
            pending = Math.Abs(total - complete);
        }
        #endregion
        #region cal percentage
        complete_rate = Math.Round((Convert.ToDouble(complete) / total) * 100, 2);
        if (complete_rate > 0)
        {
            pending_rate = 100 - complete_rate;
        }
        else
        {
            pending_rate = 100;
        }
        #endregion
        project_summary list = new project_summary();
        list.total = total;
        list.complete = complete;
        list.pending = pending;
        list.complete_rate = complete_rate;
        list.pending_rate = pending_rate;
        return list;
    }
    public static project_summary getproject_tasks_details(string pid)
    {
        DataAccess da = new DataAccess();
        int total = 0;
        int complete = 0;
        int pending = 0;
        double complete_rate = 0;
        double pending_rate = 0;

        if (string.IsNullOrEmpty(pid) || pid == "0")
            return new project_summary();

        string str1 = "select COUNT(*) as counttsk from Pms_tbl_Addtask where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and  ProjectId='" + pid + "' and status=1 and archive=0 ";
        DataSet ds1 = da.GetDataSetInline(str1);
        if (ds1.Tables[0].Rows.Count > 0)
        {

            try
            {
                total = Convert.ToInt32(ds1.Tables[0].Rows[0]["counttsk"]);
            }
            catch (Exception)
            {
            }
        }

        string str2 = "select COUNT(*) as counttask  from Pms_tbl_Addtask where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and  ProjectId='" + pid + "'  and P_Status in ( select id from Pms_tbl_Projectstatus where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and   status=1 and archive=0 and isnull(iscomplete,0)=1)  and status=1 and archive=0";
        DataSet ds2 = da.GetDataSetInline(str2);
        if (ds2.Tables[0].Rows.Count > 0)
        {

            try
            {
                complete = Convert.ToInt32(ds2.Tables[0].Rows[0]["counttask"]);
            }
            catch
            {

            }

        }
        #region pending
        if (complete > 0)
        {
            pending = Math.Abs(total - complete);
        }
        #endregion
        #region cal percentage
        complete_rate = Math.Round((Convert.ToDouble(complete) / total) * 100, 2);
        if (complete_rate > 0)
        {
            pending_rate = 100 - complete_rate;
        }
        else
        {
            pending_rate = 100;
        }
        #endregion
        project_summary list = new project_summary();
        list.total = total;
        list.complete = complete;
        list.pending = pending;
        list.complete_rate = complete_rate;
        list.pending_rate = pending_rate;
        return list;
    }
    public static project_summary getproject_issues_details(string pid)
    {
        DataAccess da = new DataAccess();
        int total = 0;
        int complete = 0;
        int pending = 0;
        double complete_rate = 0;
        double pending_rate = 0;

        if (string.IsNullOrEmpty(pid) || pid == "0")
            return new project_summary();

        string str1 = "select COUNT(*) as countIssues from Pms_tbl_Addissues where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and  ProjectId='" + pid + "' and Issueon='Project' and status=1 and archive=0 ";
        DataSet ds1 = da.GetDataSetInline(str1);
        if (ds1.Tables[0].Rows.Count > 0)
        {
            try
            {
                total = Convert.ToInt32(ds1.Tables[0].Rows[0]["countIssues"]);
            }
            catch (Exception)
            {
            }
        }


        string str2 = "select COUNT(*) as count_Issues from Pms_tbl_Addissues where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and  ProjectId='" + pid + "' and Issueon='Project' and IssueStatus in ( select id from Pms_tbl_Projectstatus where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and   status=1 and archive=0 and isnull(iscomplete,0)=1)  and status=1 and archive=0";
        DataSet ds2 = da.GetDataSetInline(str2);
        if (ds2.Tables[0].Rows.Count > 0)
        {
            try
            {
                complete = Convert.ToInt32(ds2.Tables[0].Rows[0]["count_Issues"]);
            }
            catch
            {

            }
        }


        #region pending
        if (complete > 0)
        {
            pending = Math.Abs(total - complete);
        }
        #endregion
        #region cal percentage
        complete_rate = Math.Round((Convert.ToDouble(complete) / total) * 100, 2);
        if (complete_rate > 0)
        {
            pending_rate = 100 - complete_rate;
        }
        else
        {
            pending_rate = 100;
        }
        #endregion
        project_summary list = new project_summary();
        list.total = total;
        list.complete = complete;
        list.pending = pending;
        list.complete_rate = complete_rate;
        list.pending_rate = pending_rate;
        return list;
    }

    #endregion
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<userlist> binduserlist()
    {
        string str2 = "select id,DisplayName,fileupload1,SUBSTRING (DisplayName, 1, 2) as splinameone from tbl_employee where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
        SaveLogText("Checkuserlist-" + str2);
        DataAccess danew = new DataAccess();

        SaveLogText("strTeams=" + str2);
        DataSet drmile = danew.GetDataSetInline(str2);
        List<userlist> list = new List<userlist>();
        foreach (DataRow row in drmile.Tables[0].Rows)
        {
            string profile = "";
            string prosname = "";
            if (row["fileupload1"].ToString() != "")
            {
                profile = row["fileupload1"].ToString();
            }

            list.Add(new userlist
            {
                id = row["id"].ToString(),
                name = row["DisplayName"].ToString(),
                profileimage = profile,
                shortname = row["splinameone"].ToString()

            });
        }
        return list;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<userlist> binduserlist_byrefid(string id, string utype, string stype)
    {
        DataAccess danew = new DataAccess();
        string struser = "";
        string refids = "";
        if (id != "" && id != "0")
        {
            if (utype == "project")
            {
                struser = "select team,owner from pms_tbl_addproject where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                struser += " and id=" + id;
            }
            else if (utype == "milestone")
            {
                struser = "select team,owner from pms_tbl_addmilestone where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                struser += " and id=" + id;
            }
            else if (utype == "task")
            {
                struser = "select team,owner from pms_tbl_addtask where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                struser += " and id=" + id;
            }
        }
        List<userlist> list = new List<userlist>();
        if (struser != "")
        {
            DataSet druser = danew.GetDataSetInline(struser);
            if (druser.Tables[0].Rows.Count > 0)
            {
                refids = druser.Tables[0].Rows[0][stype].ToString();
            }
            string str2 = "select id,DisplayName,fileupload1,SUBSTRING (DisplayName, 1, 2) as splinameone from tbl_employee where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";

            if (refids != "")
            {
                str2 += " and id in(" + refids + ")";
                DataSet drmile = danew.GetDataSetInline(str2);
                foreach (DataRow row in drmile.Tables[0].Rows)
                {
                    string profile = "";
                    string prosname = "";
                    if (row["fileupload1"].ToString() != "")
                    {
                        profile = row["fileupload1"].ToString();
                    }

                    list.Add(new userlist
                    {
                        id = row["id"].ToString(),
                        name = row["DisplayName"].ToString(),
                        profileimage = profile,
                        shortname = row["splinameone"].ToString()

                    });
                }
            }
        }
        else
        {
            list = binduserlistnew();
        }
        return list;
    }
    public static List<userlist> binduserlistnew()
    {
        string str2 = "select id,DisplayName,fileupload1,SUBSTRING (DisplayName, 1, 2) as splinameone from tbl_employee where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
        DataAccess danew = new DataAccess();
        DataSet drmile = danew.GetDataSetInline(str2);
        List<userlist> list = new List<userlist>();
        foreach (DataRow row in drmile.Tables[0].Rows)
        {
            string profile = "";
            string prosname = "";
            if (row["fileupload1"].ToString() != "")
            {
                profile = row["fileupload1"].ToString();
            }

            list.Add(new userlist
            {
                id = row["id"].ToString(),
                name = row["DisplayName"].ToString(),
                profileimage = profile,
                shortname = row["splinameone"].ToString()

            });
        }
        return list;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static extendedlist UpdateExtendeddatedtls(string projctid = "", string extendStrtdate = "", string extendEnddate = "")
    {
        string retval = "";
        string[] mlabel = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        string estendleftdlydys = "";
        string estendstartmonthname = "";
        string estendendmonthname = "";
        double estendleftDays = 0;
        int estendststartdys = 0;
        int estendenddys = 0;
        double estendDays = 0;
        string extenddatebind = "";


        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (projctid != "" && projctid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[8];
        prm[0] = new SqlParameter("@id", projctid);
        prm[1] = new SqlParameter("@StartDate", extendStrtdate);
        prm[2] = new SqlParameter("@TargetDate", extendEnddate);
        prm[3] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[4] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[5] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[6] = new SqlParameter("@type", entrytype);
        prm[7] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[7].Direction = ParameterDirection.ReturnValue;
        int newid = da1.executeDMLQuery("pmstbl_Extnddate_DeatilsUpdate", prm);
        SaveLogText("strextendid-" + newid.ToString());

        extendedlist list = new extendedlist();
        // extendedtimeleine 


        string strextend = "select id,extendStartdate as StartDate,extendEnddate as TargetDate from Pms_Tbl_Extended_Timeline where  status=1 and Type='Project' and archive=0  and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and ProjectID = '" + projctid + "' and id=" + newid + " ";
        SaveLogText("strextend-" + strextend);
        DataSet dsstrextend = da1.GetDataSetInline(strextend);
        if (dsstrextend.Tables[0].Rows.Count > 0)
        {
            for (int k = 0; k < dsstrextend.Tables[0].Rows.Count; k++)
            {
                DateTime estendStartDate = Convert.ToDateTime(dsstrextend.Tables[0].Rows[k]["StartDate"].ToString() == "" ? "0" : dsstrextend.Tables[0].Rows[k]["StartDate"].ToString());
                DateTime estendTargetdate = Convert.ToDateTime(dsstrextend.Tables[0].Rows[k]["TargetDate"].ToString() == "" ? "0" : dsstrextend.Tables[0].Rows[k]["TargetDate"].ToString());
                TimeSpan estendobjTimeSpan = estendTargetdate - estendStartDate;
                estendDays = Convert.ToDouble(estendobjTimeSpan.TotalDays);
                estendDays = estendDays + 1;


                DateTime estendEnddate = Convert.ToDateTime(dsstrextend.Tables[0].Rows[k]["Targetdate"].ToString() == "" ? "0" : dsstrextend.Tables[0].Rows[k]["Targetdate"].ToString());
                DateTime estendCurrentdate = DateTime.Now;
                TimeSpan estendobjtimeleftspn = estendEnddate - estendCurrentdate;
                estendleftDays = Convert.ToDouble(estendobjtimeleftspn.Days);
                estendleftDays = estendleftDays + 1;


                if (estendleftDays < 0)
                {
                    estendleftdlydys = "days delayed";
                }
                else
                {
                    estendleftdlydys = "days left";
                }

                //start days months  

                estendststartdys = estendStartDate.Day;
                int estendstrtmonth = estendStartDate.Month;
                if (estendstrtmonth > 0)
                    estendstartmonthname = mlabel[estendstrtmonth - 1];
                //end days months 

                estendenddys = estendTargetdate.Day;
                int estendendmonth = estendTargetdate.Month;
                if (estendendmonth > 0)
                    estendendmonthname = mlabel[estendendmonth - 1];

                string complete_rate = "0";
                try
                {
                    if (estendleftDays > 0)
                    {
                        estendleftdlydys = "days left";
                        int completedays = Convert.ToInt32(estendDays - estendleftDays);
                        complete_rate = Math.Round((completedays / estendDays) * 100, 2).ToString();
                    }
                    else
                    {
                        estendleftdlydys = "days delayed";
                    }
                }
                catch { }

                try
                {
                    list.status = "1";
                    list.id = newid.ToString();
                    list.fromdate = GetNullableDate(dsstrextend.Tables[0].Rows[k]["StartDate"].ToString());
                    list.todate = GetNullableDate(dsstrextend.Tables[0].Rows[k]["TargetDate"].ToString());
                    list.extended_from = estendststartdys.ToString() + " " + estendstartmonthname;
                    list.extended_to = estendenddys + " " + estendendmonthname;
                    list.extended_days = estendDays.ToString();
                    list.extended_percent = "100";
                    list.extended_type = estendleftdlydys;
                    list.delayed_days = estendleftDays.ToString().Replace("-", "");
                }
                catch (Exception ex) { list.status = ex.Message; }



            }


        }


        return list;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static int deleteproject(string id)
    {
        int ret = 0;
        DataAccess da1 = new DataAccess();
        try
        {
            string sqlq = "update pms_tbl_addproject set status=0 , archive=1 where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + id + "'";

            da1.executeDMLQueryinline(sqlq);
            ret = 1;



        }
        catch { }
        return ret;
        // Save to DB and return new id
    }
    [WebMethod]
    public static projectdata_details bindProjectdata_byid(string id)
    {

        string projectdata = "";
        function fns = new function();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();

        DataSet ds = new DataSet();
        projectdata_details projectlist = new projectdata_details();

        SqlParameter[] prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[1] = new SqlParameter("@id", id);
        ds = da.GetDataSet("Pms_getproject_byid", prm);
        int istaskchk = 0;
        string sqlchkistsk = "select isnull(istask,0) istask from Pms_tbl_AddMilestone where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and isnull(istask,0)=1  ";
        DataSet dschkistsk = da.GetDataSetInline(sqlchkistsk);
        if (dschkistsk.Tables[0].Rows.Count > 0)
        {
            istaskchk = 1;

        }
        int isdrawingkchk = 0;
        string sqlchkisdrawing = "select isnull(isdrawing,0) isdrawing from Pms_tbl_AddProject where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and isnull(isdrawing,0)=1  ";
        DataSet dschkisdrawing = da.GetDataSetInline(sqlchkisdrawing);
        if (dschkisdrawing.Tables[0].Rows.Count > 0)
        {
            isdrawingkchk = 1;

        }

        if (ds.Tables[0].Rows.Count > 0)
        {

            string streditP = "0";
            string strdeleteP = "0";
            string strownerP = "0";

            string strviewM = "0";
            string strviewT = "0";
            string strviewI = "0";
            string strviewD = "0";


            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                List<taglist> newtaglist_p = gete_taglist(ds.Tables[0].Rows[i]["id"].ToString());
                List<ownerlist> newownerlist_p = getemployeelist<ownerlist>(ds.Tables[0].Rows[i]["Owner"].ToString());
                List<teamlist> newteamlist_p = getemployeelist<teamlist>(ds.Tables[0].Rows[i]["team"].ToString());
                double mileTaskTotal = 0;
                mileTaskTotal = get_completionstatus(ds.Tables[0].Rows[i]["id"].ToString());
                //SaveLogText("chkadmintodayEX-" + mileTaskTotal);
                double mileWattagecomplite = mileTaskTotal;
                double mileWattagepending = 100;
                if (mileTaskTotal > 0)
                {
                    mileWattagepending = mileWattagepending - mileTaskTotal;
                }
                string strsddress = ds.Tables[0].Rows[i]["ProjectAddress1"].ToString();
                if (ds.Tables[0].Rows[i]["ProjectAddress2"].ToString() != string.Empty)
                {
                    if (strsddress != string.Empty)
                    {
                        strsddress += " " + ds.Tables[0].Rows[i]["ProjectAddress2"].ToString();
                    }
                    else
                    {
                        strsddress += ds.Tables[0].Rows[i]["ProjectAddress2"].ToString();
                    }
                }
                if (strsddress != string.Empty)
                {
                    strsddress += "</br>" + ds.Tables[0].Rows[i]["City"].ToString() + " " + ds.Tables[0].Rows[i]["PostalCode"].ToString() + " " + get_statename(ds.Tables[0].Rows[i]["StateName"].ToString()) + " " + get_Countryname(ds.Tables[0].Rows[i]["Countryid"].ToString());
                }
                else
                {
                    strsddress += ds.Tables[0].Rows[i]["City"].ToString() + " " + ds.Tables[0].Rows[i]["PostalCode"].ToString() + " " + get_statename(ds.Tables[0].Rows[i]["StateName"].ToString()) + " " + get_Countryname(ds.Tables[0].Rows[i]["Countryid"].ToString());
                }
                List<projectfiles> newfileslist = bindfiledata(ds.Tables[0].Rows[i]["id"].ToString(), "project");
                project_summary _sumarryM = getproject_milestone_details(ds.Tables[0].Rows[i]["id"].ToString());
                project_summary _sumarryT = getproject_tasks_details(ds.Tables[0].Rows[i]["id"].ToString());
                project_summary _sumarryI = getproject_issues_details(ds.Tables[0].Rows[i]["id"].ToString());
                List<extendedlist> extendlist = get_extended_data(ds.Tables[0].Rows[i]["id"].ToString());


                projectlist.id = ds.Tables[0].Rows[i]["id"].ToString();
                projectlist.ProjectID = ds.Tables[0].Rows[i]["ProjectID"].ToString();
                projectlist.ProjectName = ds.Tables[0].Rows[i]["ProjectName"].ToString();
                projectlist.Owner = ds.Tables[0].Rows[i]["Owner"].ToString();
                projectlist.team = ds.Tables[0].Rows[i]["team"].ToString();
                projectlist.ClientName = ds.Tables[0].Rows[i]["ClientName"].ToString();
                projectlist.ProjectStatus = ds.Tables[0].Rows[i]["ProjectStatus"].ToString();
                projectlist.status_name = get_Projectstatus(ds.Tables[0].Rows[i]["ProjectStatus"].ToString());
                projectlist.status_color = get_Status_Color(ds.Tables[0].Rows[i]["ProjectStatus"].ToString());
                projectlist.Priority = ds.Tables[0].Rows[i]["Priority"].ToString();
                projectlist.priority_name = get_PriorityName(ds.Tables[0].Rows[i]["Priority"].ToString());
                projectlist.priority_color = get_PriorityColor(ds.Tables[0].Rows[i]["Priority"].ToString());
                projectlist.ProjectAddress1 = ds.Tables[0].Rows[i]["ProjectAddress1"].ToString();
                projectlist.ProjectAddress2 = ds.Tables[0].Rows[i]["ProjectAddress2"].ToString();
                projectlist.Completeaddress = strsddress;
                projectlist.City = ds.Tables[0].Rows[i]["City"].ToString();
                projectlist.CityName = ds.Tables[0].Rows[i]["City"].ToString();
                projectlist.PostalCode = ds.Tables[0].Rows[i]["PostalCode"].ToString();
                projectlist.StateName = ds.Tables[0].Rows[i]["StateName"].ToString();
                projectlist.Countryid = ds.Tables[0].Rows[i]["Countryid"].ToString();
                projectlist.Budget = ds.Tables[0].Rows[i]["Budget"].ToString();
                projectlist.Actual = ds.Tables[0].Rows[i]["Actual"].ToString();
                projectlist.AwardDate = ds.Tables[0].Rows[i]["AwardDate"] as DateTime?;
                projectlist.StartDate = ds.Tables[0].Rows[i]["StartDate"] as DateTime?;
                projectlist.TargetDate = ds.Tables[0].Rows[i]["TargetDate"] as DateTime?;
                projectlist.ApprovalDrawing = ds.Tables[0].Rows[i]["ApprovalDrawing"].ToString();
                projectlist.DrawingApprovalDate = ds.Tables[0].Rows[i]["DrawingApprovalDate"] as DateTime?;
                projectlist.Uploaddrawing = ds.Tables[0].Rows[i]["Uploaddrawing"].ToString();
                projectlist.Weightage = ds.Tables[0].Rows[i]["Weightage"].ToString();
                projectlist.isdrawing = ds.Tables[0].Rows[i]["isdrawing"].ToString();
                projectlist.checkprojectWeightage = ds.Tables[0].Rows[i]["checkprojectWeightage"].ToString();
                projectlist.customerid = ds.Tables[0].Rows[i]["customerid"].ToString();
                projectlist.salesorderid = ds.Tables[0].Rows[i]["salesorderid"].ToString();
                projectlist.hastask = ds.Tables[0].Rows[i]["hastask"].ToString();
                projectlist.checktaskWeightage = ds.Tables[0].Rows[i]["checktaskWeightage"].ToString();
                projectlist.hassubtask = ds.Tables[0].Rows[i]["hassubtask"].ToString();
                projectlist.checksubtaskWeightage = ds.Tables[0].Rows[i]["checksubtaskWeightage"].ToString();
                projectlist.leftdays = get_tdays("left", ds.Tables[0].Rows[i]["StartDate"].ToString(), ds.Tables[0].Rows[i]["TargetDate"].ToString());
                projectlist.txtdays = get_tdays("total", ds.Tables[0].Rows[i]["StartDate"].ToString(), ds.Tables[0].Rows[i]["TargetDate"].ToString());
                projectlist.complete_value = Math.Round(mileTaskTotal, 2).ToString();
                projectlist.pending_value = Math.Round(mileWattagepending, 2).ToString();
                projectlist.totalmile = _sumarryM.total.ToString();
                projectlist.completemile = _sumarryM.complete.ToString();
                projectlist.completemile_rate = _sumarryM.complete_rate.ToString();
                projectlist.pendingmile = _sumarryM.pending.ToString();
                projectlist.pendingmile_rate = _sumarryM.pending_rate.ToString();

                projectlist.totaltask = _sumarryT.total.ToString();
                projectlist.completetask = _sumarryT.complete.ToString();
                projectlist.completetask_rate = _sumarryT.complete_rate.ToString();
                projectlist.pendingtask = _sumarryT.pending.ToString();
                projectlist.pendingtask_rate = _sumarryT.pending_rate.ToString();

                projectlist.totalissue = _sumarryI.total.ToString();
                projectlist.completeissue = _sumarryI.complete.ToString();
                projectlist.completeissue_rate = _sumarryI.complete_rate.ToString();
                projectlist.pendingissue = _sumarryI.pending.ToString();
                projectlist.pendingissue_rate = _sumarryI.pending_rate.ToString();
                projectlist.tags = newtaglist_p;
                projectlist.owners = newownerlist_p;
                projectlist.teams = newteamlist_p;
                projectlist.viewfiles = newfileslist;
                projectlist.extendedlists = extendlist;


            }


        }

        return projectlist;
    }
    public class userlist : IPerson { public string id { get; set; } public string name { get; set; } public string profileimage { get; set; } public string shortname { get; set; } }

    public static lastextendeddetails get_lastextended_data(string pid)
    {
        string[] mlabel = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        DataAccess da = new DataAccess();
        if (string.IsNullOrEmpty(pid))
            return new lastextendeddetails();
        lastextendeddetails list = new lastextendeddetails();
        string estendleftdlydys = "";
        string estendstartmonthname = "";
        string estendendmonthname = "";
        double estendleftDays = 0;
        int estendststartdys = 0;
        int estendenddys = 0;
        double estendDays = 0;
        string extenddatebind = "";
        string strextend = "select top(1) id,extendStartdate as StartDate,extendEnddate as TargetDate from Pms_Tbl_Extended_Timeline where  status=1 and Type='Project' and archive=0  and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and ProjectID = '" + pid + "' order by id desc  ";

        DataSet dsstrextend = da.GetDataSetInline(strextend);
        if (dsstrextend.Tables[0].Rows.Count > 0)
        {

            for (int k = 0; k < dsstrextend.Tables[0].Rows.Count; k++)
            {
                DateTime estendStartDate = Convert.ToDateTime(dsstrextend.Tables[0].Rows[k]["StartDate"].ToString() == "" ? "0" : dsstrextend.Tables[0].Rows[k]["StartDate"].ToString());
                DateTime estendTargetdate = Convert.ToDateTime(dsstrextend.Tables[0].Rows[k]["TargetDate"].ToString() == "" ? "0" : dsstrextend.Tables[0].Rows[k]["TargetDate"].ToString());
                TimeSpan estendobjTimeSpan = estendTargetdate - estendStartDate;
                estendDays = Convert.ToDouble(estendobjTimeSpan.TotalDays);
                estendDays = estendDays + 1;


                DateTime estendEnddate = Convert.ToDateTime(dsstrextend.Tables[0].Rows[k]["Targetdate"].ToString() == "" ? "0" : dsstrextend.Tables[0].Rows[k]["Targetdate"].ToString());
                DateTime estendCurrentdate = DateTime.Now;
                TimeSpan estendobjtimeleftspn = estendEnddate - estendCurrentdate;
                estendleftDays = Convert.ToDouble(estendobjtimeleftspn.Days);
                estendleftDays = estendleftDays + 1;


                if (estendleftDays < 0)
                {
                    estendleftdlydys = "days delayed";
                }
                else
                {
                    estendleftdlydys = "days left";
                }

                //start days months  

                estendststartdys = estendStartDate.Day;
                int estendstrtmonth = estendStartDate.Month;
                if (estendstrtmonth > 0)
                    estendstartmonthname = mlabel[estendstrtmonth - 1];
                //end days months 

                estendenddys = estendTargetdate.Day;
                int estendendmonth = estendTargetdate.Month;
                if (estendendmonth > 0)
                    estendendmonthname = mlabel[estendendmonth - 1];

                string complete_rate = "0";
                try
                {
                    if (estendleftDays > 0)
                    {
                        estendleftdlydys = "days left";
                        int completedays = Convert.ToInt32(estendDays - estendleftDays);
                        complete_rate = Math.Round((completedays / estendDays) * 100, 2).ToString();
                    }
                    else
                    {
                        estendleftdlydys = "days delayed";
                    }
                }
                catch { }



                   list.id = dsstrextend.Tables[0].Rows[k]["id"].ToString();
                    list.fromdate = GetNullableDate(dsstrextend.Tables[0].Rows[k]["StartDate"].ToString());
                    list.todate = GetNullableDate(dsstrextend.Tables[0].Rows[k]["TargetDate"].ToString());
                    list.extended_from = estendststartdys.ToString() + " " + estendstartmonthname;
                    list.extended_to = estendenddys + " " + estendendmonthname;
                    list.extended_days = estendDays.ToString();
                    list.extended_percent = "100";
                    list.extended_type = estendleftdlydys;
                    list.delayed_days = estendleftDays.ToString().Replace("-", "");

                


            }


        }

        return list;
    }
    public static List<extendedlist> get_extended_data(string pid)
    {
        string[] mlabel = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        DataAccess da = new DataAccess();
        if (string.IsNullOrEmpty(pid))
            return new List<extendedlist>();
        List<extendedlist> list = new List<extendedlist>();
        string estendleftdlydys = "";
        string estendstartmonthname = "";
        string estendendmonthname = "";
        double estendleftDays = 0;
        int estendststartdys = 0;
        int estendenddys = 0;
        double estendDays = 0;
        string extenddatebind = "";
        string strextend = "select id,extendStartdate as StartDate,extendEnddate as TargetDate from Pms_Tbl_Extended_Timeline where  status=1 and Type='Project' and archive=0  and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and ProjectID = '" + pid + "'  ";

        DataSet dsstrextend = da.GetDataSetInline(strextend);
        if (dsstrextend.Tables[0].Rows.Count > 0)
        {

            for (int k = 0; k < dsstrextend.Tables[0].Rows.Count; k++)
            {
                DateTime estendStartDate = Convert.ToDateTime(dsstrextend.Tables[0].Rows[k]["StartDate"].ToString() == "" ? "0" : dsstrextend.Tables[0].Rows[k]["StartDate"].ToString());
                DateTime estendTargetdate = Convert.ToDateTime(dsstrextend.Tables[0].Rows[k]["TargetDate"].ToString() == "" ? "0" : dsstrextend.Tables[0].Rows[k]["TargetDate"].ToString());
                TimeSpan estendobjTimeSpan = estendTargetdate - estendStartDate;
                estendDays = Convert.ToDouble(estendobjTimeSpan.TotalDays);
                estendDays = estendDays + 1;


                DateTime estendEnddate = Convert.ToDateTime(dsstrextend.Tables[0].Rows[k]["Targetdate"].ToString() == "" ? "0" : dsstrextend.Tables[0].Rows[k]["Targetdate"].ToString());
                DateTime estendCurrentdate = DateTime.Now;
                TimeSpan estendobjtimeleftspn = estendEnddate - estendCurrentdate;
                estendleftDays = Convert.ToDouble(estendobjtimeleftspn.Days);
                estendleftDays = estendleftDays + 1;


                if (estendleftDays < 0)
                {
                    estendleftdlydys = "days delayed";
                }
                else
                {
                    estendleftdlydys = "days left";
                }

                //start days months  

                estendststartdys = estendStartDate.Day;
                int estendstrtmonth = estendStartDate.Month;
                if (estendstrtmonth > 0)
                    estendstartmonthname = mlabel[estendstrtmonth - 1];
                //end days months 

                estendenddys = estendTargetdate.Day;
                int estendendmonth = estendTargetdate.Month;
                if (estendendmonth > 0)
                    estendendmonthname = mlabel[estendendmonth - 1];

                string complete_rate = "0";
                try
                {
                    if (estendleftDays > 0)
                    {
                        estendleftdlydys = "days left";
                        int completedays = Convert.ToInt32(estendDays - estendleftDays);
                        complete_rate = Math.Round((completedays / estendDays) * 100, 2).ToString();
                    }
                    else
                    {
                        estendleftdlydys = "days delayed";
                    }
                }
                catch { }


                list.Add(new extendedlist
                {
                    id = dsstrextend.Tables[0].Rows[k]["id"].ToString(),
                    fromdate = GetNullableDate(dsstrextend.Tables[0].Rows[k]["StartDate"].ToString()),
                    todate = GetNullableDate(dsstrextend.Tables[0].Rows[k]["TargetDate"].ToString()),
                    extended_from = estendststartdys.ToString() + " " + estendstartmonthname,
                    extended_to = estendenddys + " " + estendendmonthname,
                    extended_days = estendDays.ToString(),
                    extended_percent = "100",
                    extended_type = estendleftdlydys,
                    delayed_days = estendleftDays.ToString().Replace("-", "")

                });


            }


        }

        return list;
    }
    public static DateTime? GetNullableDate(object value)
    {
        return value == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(value);
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
    protected static double get_completionstatus(string id)
    {
        double strret = 0;

        if (id == "" || id == "0")
        {
            return 0;
        }
        DataAccess da = new DataAccess();

        double mileTaskTotal = 0;
        string findmile = "select cast(ROUND(isnull(iscompletepercentage,0),2) AS DECIMAL(10,2)) as iscompletepercentage from Pms_tbl_AddProject  where id='" + id + "'  and status=1  and archive=0";

        DataSet dsssummile = da.GetDataSetInline(findmile);

        try
        {
            mileTaskTotal = Convert.ToDouble(dsssummile.Tables[0].Rows[0]["iscompletepercentage"]);
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
    public static string get_tdays(string type, string startdate, string enddate)
    {
        string retval = "";

        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();

        string tleftdays = "";
        #region bind timeline
        //Timeline
        if (startdate != string.Empty && enddate != string.Empty)
        {

            double miledayscomplite = 0;


            DateTime mileStartDate = Convert.ToDateTime(startdate == "" ? DateTime.Now.ToString() : startdate);
            DateTime mileTargetdate = Convert.ToDateTime(enddate == "" ? DateTime.Now.ToString() : enddate);
            TimeSpan mileobjTimeSpan = mileTargetdate - mileStartDate;
            double mileDays = Convert.ToDouble(mileobjTimeSpan.TotalDays);
            if (type == "total")
            {
                retval = mileDays.ToString();
                return retval;
            }
            mileDays = mileDays + 1;

            DateTime mileEnddate = Convert.ToDateTime(enddate == "" ? DateTime.Now.ToString() : enddate);
            DateTime mileCurrentdate = DateTime.Now;
            TimeSpan mileobjtimeleftspn = mileEnddate - mileCurrentdate;
            double mileleftDays = Convert.ToDouble(mileobjtimeleftspn.Days);


            string mileleftdlydys = "";
            if (mileleftDays < 0)
            {
                mileleftDays = mileleftDays + (-1);
                miledayscomplite = 100;
                mileleftdlydys = "days delayed";
            }
            else
            {
                mileleftDays = mileleftDays + 1;
                miledayscomplite = Math.Round(((mileDays - mileleftDays) * 100) / mileDays, 2);
                mileleftdlydys = "days left";
            }
            //start days months  
            string milestartmonthname = "";
            int mileststartdys = mileStartDate.Day;
            int milestrtmonth = mileStartDate.Month;
            int milestonestartyear = mileStartDate.Year;
            //end days months 
            string mileendmonthname = "";
            int mileenddys = mileTargetdate.Day;
            int mileendmonth = mileTargetdate.Month;
            int milestoneEndyear = mileTargetdate.Year;
            if (mileleftdlydys != "")
            {
                tleftdays = mileleftDays.ToString().Replace("-", "") + " " + mileleftdlydys;
            }

            retval = tleftdays;
        }

        #endregion
        return retval;
    }

    public static string get_tdays11(string id)
    {
        string retval = "";

        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        string strsql = "select id,Startdate,Targetdate,team,owner from Pms_tbl_Addproject where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + id + "'";

        string tleftdays = "";
        #region bind timeline
        //Timeline
        if (strsql != "")
        {
            DataSet dsD = da.GetDataSetInline(strsql);
            double miledayscomplite = 0;


            DateTime mileStartDate = Convert.ToDateTime(dsD.Tables[0].Rows[0]["StartDate"].ToString() == "" ? DateTime.Now.ToString() : dsD.Tables[0].Rows[0]["StartDate"].ToString());
            DateTime mileTargetdate = Convert.ToDateTime(dsD.Tables[0].Rows[0]["Targetdate"].ToString() == "" ? DateTime.Now.ToString() : dsD.Tables[0].Rows[0]["Targetdate"].ToString());
            TimeSpan mileobjTimeSpan = mileTargetdate - mileStartDate;
            double mileDays = Convert.ToDouble(mileobjTimeSpan.TotalDays);
            mileDays = mileDays + 1;


            DateTime mileEnddate = Convert.ToDateTime(dsD.Tables[0].Rows[0]["Targetdate"].ToString() == "" ? DateTime.Now.ToString() : dsD.Tables[0].Rows[0]["Targetdate"].ToString());
            DateTime mileCurrentdate = DateTime.Now;
            TimeSpan mileobjtimeleftspn = mileEnddate - mileCurrentdate;
            double mileleftDays = Convert.ToDouble(mileobjtimeleftspn.Days);


            string mileleftdlydys = "";
            if (mileleftDays < 0)
            {
                mileleftDays = mileleftDays + (-1);
                miledayscomplite = 100;
                mileleftdlydys = "days delayed";
            }
            else
            {
                mileleftDays = mileleftDays + 1;
                miledayscomplite = Math.Round(((mileDays - mileleftDays) * 100) / mileDays, 2);
                mileleftdlydys = "days left";
            }
            //start days months  
            string milestartmonthname = "";
            int mileststartdys = mileStartDate.Day;
            int milestrtmonth = mileStartDate.Month;
            int milestonestartyear = mileStartDate.Year;
            //end days months 
            string mileendmonthname = "";
            int mileenddys = mileTargetdate.Day;
            int mileendmonth = mileTargetdate.Month;
            int milestoneEndyear = mileTargetdate.Year;
            if (mileleftdlydys != "")
            {
                tleftdays = mileleftDays.ToString().Replace("-", "") + " " + mileleftdlydys;
            }
            retval = tleftdays;
        }

        #endregion
        return retval;
    }

    public static List<T> getemployeelist<T>(string ids) where T : IPerson, new()
    {
        if (string.IsNullOrEmpty(ids) || ids == "0")
            return new List<T>();

        string str2 = "select id, DisplayName, fileupload1, SUBSTRING(DisplayName,1,2) as splinameone " +
                      "from tbl_employee " +
                      "where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' " +
                      "and id in(" + ids + ")";

        DataAccess danew = new DataAccess();
        DataSet drmile = danew.GetDataSetInline(str2);

        List<T> list = new List<T>();

        foreach (DataRow row in drmile.Tables[0].Rows)
        {
            T obj = new T();
            obj.id = row["id"].ToString();
            obj.name = row["DisplayName"].ToString();
            obj.profileimage = row["fileupload1"].ToString();
            obj.shortname = row["splinameone"].ToString();
            list.Add(obj);
        }

        return list;
    }
   public static List<T> getemployeelist_all<T>(string pid) where T : IPerson, new()
    {
        if (string.IsNullOrEmpty(pid) || pid == "0")
            return new List<T>();

        SqlParameter[] prm_PT = new SqlParameter[2];
        prm_PT[0] = new SqlParameter("@ProjectId", pid);
        prm_PT[1] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());

        DataAccess danew = new DataAccess();
        DataSet drmile = danew.GetDataSet("pms_getproject_com_teamnew", prm_PT);

        List<T> list = new List<T>();

        foreach (DataRow row in drmile.Tables[0].Rows)
        {
            T obj = new T();
            obj.id = row["id"].ToString();
            obj.name = row["DisplayName"].ToString();
            obj.profileimage = row["fileupload1"].ToString();
            obj.shortname = row["splinameone"].ToString();
            list.Add(obj);
        }

        return list;
    }
    public static List<taglist> gete_taglist(string pid)
    {
        if (string.IsNullOrEmpty(pid) || pid == "0")
            return new List<taglist>();

        List<taglist> list = new List<taglist>();
        DataAccess da = new DataAccess();
        string strtags = "select id, tags,TagsColor from   Pms_tbl_Addtags where ProjectId='" + pid + "'  order by id asc";
        DataSet dstags = da.GetDataSetInline(strtags);

        if (dstags != null && dstags.Tables.Count > 0 && dstags.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow row in dstags.Tables[0].Rows)
            {
                list.Add(new taglist
                {
                    id = row["id"].ToString(),
                    tag_name = row["tags"].ToString(),
                    tag_color = row["TagsColor"].ToString()
                });
            }
        }

        return list;
    }
    public static string get_statename(string stateid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from tbl_statemaster where status=1 and archive=0 and id='" + stateid + "'  ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["statename"].ToString();

        }

        return strret;
    }
    public static string get_clientname(string clientid)
    {
        DataAccess da = new DataAccess();
        string ret = "";
        if(clientid==string.Empty|| clientid == "0")
        {
            return "";
        }
        string strsql = "select id,txtcompanyname from tbl_contactsdetails where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0 and id='"+clientid+"' ";

        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            ret = dscode.Tables[0].Rows[0]["txtcompanyname"].ToString();

        }

        return ret;
    }
    public static string get_estimateno(string salesorderid)
    {
        DataAccess da = new DataAccess();
        string ret = "";
        if(salesorderid == string.Empty|| salesorderid == "0")
        {
            return "";
        }
        string strsql = "select id,Estimateno from tbl_SalesOrderdetails where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id ='" + salesorderid + "' ";

        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            ret = dscode.Tables[0].Rows[0]["Estimateno"].ToString();

        }

        return ret;
    }
    public static string get_Countryname(string countryid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Imt_Country where status=1 and archive=0 and conid='" + countryid + "'  ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["country"].ToString();

        }

        return strret;
    }
    protected static string get_Projectstatus(string statusid)
    {
        if (statusid == "" || statusid == "0")
        {
            return "-";
        }
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_Projectstatus where status=1 and archive=0 and id='" + statusid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString();

        }
        else
        {
            strret = "-";
        }

        return strret;

    }
    protected static string get_Status_Color(string statusid)
    {
        if (statusid == "" || statusid == "0")
        {
            return "-";
        }
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_Projectstatus where status=1 and archive=0 and id='" + statusid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["PrintColor"].ToString();

        }

        return strret;

    }
    protected static string get_PriorityName(string priorityid)
    {
        if (priorityid == "" || priorityid == "0")
        {
            return "-";
        }
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_Priority where status=1 and archive=0 and Id='" + priorityid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["name"].ToString();

        }
        else
        {
            strret = "-";
        }

        return strret;

    }
    protected static string get_PriorityColor(string priorityid)
    {
        if (priorityid == "" || priorityid == "0")
        {
            return "-";
        }
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from Pms_tbl_Priority where status=1 and archive=0 and Id='" + priorityid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["PrintColor"].ToString();

        }

        return strret;

    }
    protected static string get_filetypename(string projectid, string fentryid, string ftype)
    {
        if (fentryid == "" || fentryid == "0")
        {
            return "";
        }
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = "";
        //if (ftype == "project")
       // {
          //  return ftype;
       // }
        strsql = " select id,ProjectName as ftypename from Pms_tbl_addproject where status=1 and archive=0 and id=" + projectid + " and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        if (ftype == "milestone")
        {
            strsql = " select id,Milestone as ftypename from Pms_tbl_addmilestone where status=1 and archive=0 and projectid=" + projectid + " and id='" + fentryid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        }
        else if (ftype == "task")
        {
            strsql = " select id,taskname as ftypename from Pms_tbl_addtask where status=1 and archive=0 and projectid=" + projectid + " and id='" + fentryid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        }
        else if (ftype == "subtask")
        {
            strsql = " select id,subtaskname as ftypename from Pms_tbl_addsubtask where status=1 and archive=0 and projectid=" + projectid + " and id='" + fentryid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        }
        try
        {
            DataSet ds = da.GetDataSetInline(strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                strret = ds.Tables[0].Rows[0]["ftypename"].ToString();

            }
            else
            {
                strret = "";
            }
        }
        catch { }

        return strret;

    }
    public static List<projectfiles> bindfiledata(string pid, string type)
    {

        DataAccess da = new DataAccess();
        string retval = "";


        string strsqlfils = @"select emp.DisplayName as empname,files.* from Pms_tbl_projectfiles files with(nolock)
                            left join tbl_employee emp with(nolock) on files.uid=emp.id where files.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and files.status=1 and files.archive=0 and files.pid='" + pid + "'   order by id desc";//and ftype='project' and fentryid='" + pid + "'
        SaveLogText("strsqlfils-"+ strsqlfils);
        DataSet dsfils = da.GetDataSetInline(strsqlfils);
        string strdatafils = "";
        int countrow = 0;

        //"<thead>" +
        //"<tr><th></th><th>File</th><th style='width:30px'></th></tr>" +
        //"</thead>";
        List<projectfiles> newprojectfiles = new List<projectfiles>();
        if (dsfils.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dsfils.Tables[0].Rows.Count; i++)
            {
                string filepath = "";
                string ftype = dsfils.Tables[0].Rows[i]["ftype"].ToString();
                string fentryid = dsfils.Tables[0].Rows[i]["fentryid"].ToString();
                string strtypename = "";
                if (type != string.Empty)
                {
                    strtypename = get_filetypename(pid, fentryid, ftype);
                    if (type == "project")
                        filepath = "/assets/images/" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "/" + fentryid + "/";
                    else if (type == "milestone")
                        filepath = "/pms/MileImages/" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "/" + fentryid + "/";
                    else if (type == "task")
                        filepath = "/pms/TaskImages/" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "/" + fentryid + "/";
                    else if (type == "subtask")
                        filepath = "/pms/SubtaskImages/" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "/" + fentryid + "/";

                }
                newprojectfiles.Add(new projectfiles
                {

                    id = dsfils.Tables[0].Rows[i]["id"].ToString(),
                    Uploadby = dsfils.Tables[0].Rows[i]["empname"].ToString(),
                    pid = dsfils.Tables[0].Rows[i]["pid"].ToString(),
                    fentryid = dsfils.Tables[0].Rows[i]["fentryid"].ToString(),
                    ftype = dsfils.Tables[0].Rows[i]["ftype"].ToString(),
                    ftypename = strtypename,
                    fileurl = dsfils.Tables[0].Rows[i]["fileurl"].ToString(),
                    filedes = dsfils.Tables[0].Rows[i]["filedes"].ToString(),
                    fext = dsfils.Tables[0].Rows[i]["fext"].ToString(),
                    filepath = filepath,
                    isshow = true,

                });

            }

        }


        return newprojectfiles;
    }
    public class countrylist
    {
        public string Countryid { get; set; }
        public string CountryName { get; set; }
    }
    public class prioritylist
    {
        public string id { get; set; }
        public string name { get; set; }
        public string colorname { get; set; }
        public string isdefault { get; set; }
    }
    public class statelist
    {
        public string Stateid { get; set; }
        public string StateName { get; set; }
    }
    public class statuslist
    {
        public string id { get; set; }
        public string name { get; set; }
        public string colorname { get; set; }
        public string statusicon { get; set; }
        public string iscomplete { get; set; }
        public string isdefault { get; set; }
    }
    public class lastextendeddetails
    {
        public string id { get; set; }
        public DateTime? fromdate { get; set; }
        public DateTime? todate { get; set; }
        public string extended_from { get; set; }
        public string extended_to { get; set; }
        public string extended_days { get; set; }
        public string extended_percent { get; set; }
        public string extended_type { get; set; }
        public string delayed_days { get; set; }
        public string status { get; set; }
    }
    public class extendedlist
    {
        public string id { get; set; }
        public DateTime? fromdate { get; set; }
        public DateTime? todate { get; set; }

        public string extended_from { get; set; }
        public string extended_to { get; set; }
        public string extended_days { get; set; }
        public string extended_percent { get; set; }
        public string extended_type { get; set; }
        public string delayed_days { get; set; }
        public string status { get; set; }

    }
    public class extendedlist_ret
    {

        public string id { get; set; }
        public DateTime? fromdate { get; set; }
        public DateTime? todate { get; set; }
        public string extended_from { get; set; }
        public string extended_to { get; set; }
        public string extended_days { get; set; }
        public string extended_percent { get; set; }
        public string extended_type { get; set; }
        public string delayed_days { get; set; }
        public string status { get; set; }

    }
    public class projectteams
    {
        public string id { get; set; }
        public string Team { get; set; }

    }
    public class projectdata_countdetails
    {
        public string ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string total { get; set; }
        public string complete { get; set; }
        public string complete_rate { get; set; }
        public string pending { get; set; }
        public string pending_rate { get; set; }

    }
    public class projectdata_details
    {
        public string id { get; set; }
        public string ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string Owner { get; set; }
        public string team { get; set; }
        public string ProjectStatus { get; set; }
        public string status_name { get; set; }
        public string status_color { get; set; }
        public string ProjectAddress1 { get; set; }
        public string ProjectAddress2 { get; set; }
        public string Completeaddress { get; set; }
        public string City { get; set; }
        public string CityName { get; set; }
        public string PostalCode { get; set; }
        public string StateName { get; set; }
        public string StateName2 { get; set; }
        public string Countryid { get; set; }
        public string CountryName { get; set; }
        public string Priority { get; set; }
        public string priority_name { get; set; }
        public string priority_color { get; set; }
        public string Budget { get; set; }
        public string Actual { get; set; }
        public DateTime? AwardDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? TargetDate { get; set; }
        public string ApprovalDrawing { get; set; }
        public DateTime? DrawingApprovalDate { get; set; }
        public string Uploaddrawing { get; set; }
        public string Weightage { get; set; }
        public string isdrawing { get; set; }
        public string checkprojectWeightage { get; set; }
        public string customerid { get; set; }
        public string ClientName { get; set; }
        public string salesorderid { get; set; }
        public string Estimateno { get; set; }        
        public string hastask { get; set; }
        public string checktaskWeightage { get; set; }
        public string hassubtask { get; set; }
        public string checksubtaskWeightage { get; set; }
        public string leftdays { get; set; }
        public string txtdays { get; set; }
        public string complete_value { get; set; }
        public string pending_value { get; set; }
        public string totaldependency { get; set; }
        public string totalmile { get; set; }
        public string completemile { get; set; }
        public string completemile_rate { get; set; }
        public string pendingmile { get; set; }
        public string pendingmile_rate { get; set; }
        public string totaltask { get; set; }
        public string completetask { get; set; }
        public string completetask_rate { get; set; }
        public string pendingtask { get; set; }
        public string pendingtask_rate { get; set; }
        public string totalissue { get; set; }
        public string completeissue { get; set; }
        public string completeissue_rate { get; set; }
        public string pendingissue { get; set; }
        public string pendingissue_rate { get; set; }
        public string iscountholydays { get; set; }
        public string iscountweekoff { get; set; }

        public DateTime? lastextendstartdate { get; set; }
        public DateTime? lastextendenddate { get; set; }
        public List<extendedlist> extendedlists { get; set; }
        public List<taglist> tags { get; set; }
        public List<ownerlist> owners { get; set; }
        public List<teamlist> teams { get; set; }
        public List<teamlist> teams_all { get; set; }
        public List<projectfiles> viewfiles { get; set; }

    }
    public class newproject
    {
        public string id { get; set; }
        public string ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string Owner { get; set; }
        public string OwnerName { get; set; }
        public string team { get; set; }
        public string ProjectStatus { get; set; }
        public string StatusName { get; set; }
        public string ProjectAddress1 { get; set; }
        public string ProjectAddress2 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Stateid { get; set; }
        public string StateName { get; set; }
        public string Countryid { get; set; }
        public string CountryName { get; set; }        
        public string Priority { get; set; }
        public string PriorityName { get; set; }
        public string Budget { get; set; }
        public string Actual { get; set; }
        public DateTime? AwardDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? TargetDate { get; set; }
        public bool? RequiredDrawing { get; set; }
        public bool ApprovalDrawing { get; set; }
        public DateTime? DrawingApprovalDate { get; set; }
        public string Weightage { get; set; }        
        public string iscomplete { get; set; }
        public DateTime? iscompletedate { get; set; }
        public string iscompletepercentage { get; set; }
        public string ckprojectWeightage { get; set; }
        public string customerid { get; set; }
        public string ClientName { get; set; }
        public string salesorderid { get; set; }
        public string Estimateno { get; set; }        
        public bool? hasmile { get; set; }
        public bool? checkmilesameWeightage { get; set; }
        public bool? hastask { get; set; }
        public bool? checktasksameWeightage { get; set; }
        public bool? hassubtask { get; set; }
        public bool? checksubtasksameWeightage { get; set; }
        public string tagids { get; set; }
        public bool? iscountholydays { get; set; }
        public bool? iscountweekoff { get; set; }
        public List<addprojectfiles> uploadprojectfiles { get; set; }
    }
    public class addprojectfiles
    {
        
        public string id { get; set; }
        public string Uploadby { get; set; }
        public string file { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string size { get; set; }        
        public string fext { get; set; }        
        public string data { get; set; }        


    }
    public interface IPerson
    {
        string id { get; set; }
        string name { get; set; }
        string profileimage { get; set; }
        string shortname { get; set; }
    }

    public class taglist
    {
        public string id { get; set; }
        public string tag_name { get; set; }
        public string tag_color { get; set; }
    }
    public class projectfiles
    {
        public string id { get; set; }
        public string pid { get; set; }
        public string Uploadby { get; set; }
        public string fentryid { get; set; }
        public string ftype { get; set; }
        public string ftypename { get; set; }
        public string fileurl { get; set; }
        public string filedes { get; set; }
        public string fext { get; set; }
        public string filepath { get; set; }
        public bool isshow { get; set; }


    }
    public class ownerlist : IPerson { public string id { get; set; } public string name { get; set; } public string profileimage { get; set; } public string shortname { get; set; } }
    public class teamlist : IPerson { public string id { get; set; } public string name { get; set; } public string profileimage { get; set; } public string shortname { get; set; } }
    public class issuesdetails
    {
        public int? totalissue { get; set; }
        public int? completeissue { get; set; }
        public int? pendingissue { get; set; }
        public double? completeissues_rate { get; set; }
        public double? pendingissues_rate { get; set; }

    }
    public class project_summary
    {
        public double? pcopmelete_value { get; set; }
        public double? ppending_value { get; set; }
        public int? total { get; set; }
        public int? complete { get; set; }
        public int? pending { get; set; }
        public double? complete_rate { get; set; }
        public double? pending_rate { get; set; }

    }
    public class customerlist
    {
        public string customerid { get; set; }
        public string ClientName { get; set; }
        
    }
    public class solist
    {
        public string salesorderid { get; set; }
        public string Estimateno { get; set; }

    }
}


