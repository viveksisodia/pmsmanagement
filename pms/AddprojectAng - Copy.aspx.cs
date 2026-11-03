using System;
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

public partial class AddprojectAng : System.Web.UI.Page
{
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

     
    protected void Page_Load(object sender, EventArgs e)
    {
      

    }


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
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<projectdata_details> bindProjectdata()
    {
        
        string projectdata = "";
        function fns = new function();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();

        DataSet ds = new DataSet();   
        List<projectdata_details> projectlist = new List<projectdata_details>();
        int isadmin = 0;
        string strsql = "select Manage_Profile.* from Manage_Profile inner join tbl_employee on Manage_Profile.id=cast(tbl_employee.roleid as int) where tbl_employee.id='" + HttpContext.Current.Request.Cookies["user"]["user_id"] + "'  and Manage_Profile.status=1 and Manage_Profile.archive=0";
        DataSet dsadminchk = da.GetDataSetInline(strsql);
        if (dsadminchk.Tables[0].Rows.Count > 0)
        {
            if (dsadminchk.Tables[0].Rows[0]["id"].ToString() == "1")
            {
                isadmin = 1;

            }

        }

        if (isadmin == 1)
        {
            SqlParameter[] prm = new SqlParameter[1];
            prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            ds = da.GetDataSet("Pms_select_tblprojectall_dashbord", prm);

        }
        else
        {
            SqlParameter[] prm = new SqlParameter[2];
            prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[1] = new SqlParameter("@owner", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            ds = da.GetDataSet("Pms_select_tblprojectall_dashbord_ownerwise", prm);

        }


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

                    projectlist.Add(new projectdata_details
                    {
                        id = ds.Tables[0].Rows[i]["id"].ToString(),
                        ProjectID = ds.Tables[0].Rows[i]["ProjectID"].ToString(),
                        ProjectName = ds.Tables[0].Rows[i]["ProjectName"].ToString(),
                        Owner = ds.Tables[0].Rows[i]["Owner"].ToString(),
                        team = ds.Tables[0].Rows[i]["team"].ToString(),
                        ClientName = ds.Tables[0].Rows[i]["ClientName"].ToString(),
                        ProjectStatus = ds.Tables[0].Rows[i]["ProjectStatus"].ToString(),
                        status_name = get_Projectstatus(ds.Tables[0].Rows[i]["ProjectStatus"].ToString()),
                        status_color = get_Status_Color(ds.Tables[0].Rows[i]["ProjectStatus"].ToString()),
                        Priority = ds.Tables[0].Rows[i]["Priority"].ToString(),
                        priority_name = get_PriorityName(ds.Tables[0].Rows[i]["Priority"].ToString()),
                        priority_color = get_PriorityColor(ds.Tables[0].Rows[i]["Priority"].ToString()),
                        ProjectAddress1 = ds.Tables[0].Rows[i]["ProjectAddress1"].ToString(),
                        ProjectAddress2 = ds.Tables[0].Rows[i]["ProjectAddress2"].ToString(),
                        Completeaddress = strsddress,
                        City = ds.Tables[0].Rows[i]["City"].ToString(),
                        CityName = ds.Tables[0].Rows[i]["City"].ToString(),
                        PostalCode = ds.Tables[0].Rows[i]["PostalCode"].ToString(),
                        StateName = ds.Tables[0].Rows[i]["StateName"].ToString(),
                        Countryid = ds.Tables[0].Rows[i]["Countryid"].ToString(),
                        //CountryName = ds.Tables[0].Rows[i]["CountryName"].ToString(),
                        Budget = ds.Tables[0].Rows[i]["Budget"].ToString(),
                        Actual = ds.Tables[0].Rows[i]["Actual"].ToString(),
                        AwardDate = ds.Tables[0].Rows[i]["AwardDate"] as DateTime?,
                        StartDate = ds.Tables[0].Rows[i]["StartDate"] as DateTime?,
                        TargetDate = ds.Tables[0].Rows[i]["TargetDate"] as DateTime?,
                        ApprovalDrawing = ds.Tables[0].Rows[i]["ApprovalDrawing"].ToString(),
                        DrawingApprovalDate = ds.Tables[0].Rows[i]["DrawingApprovalDate"] as DateTime?,
                        Uploaddrawing = ds.Tables[0].Rows[i]["Uploaddrawing"].ToString(),
                        Weightage = ds.Tables[0].Rows[i]["Weightage"].ToString(),
                        isdrawing = ds.Tables[0].Rows[i]["isdrawing"].ToString(),
                        checkprojectWeightage = ds.Tables[0].Rows[i]["checkprojectWeightage"].ToString(),
                        customerid = ds.Tables[0].Rows[i]["customerid"].ToString(),
                        salesorderid = ds.Tables[0].Rows[i]["salesorderid"].ToString(),
                        hastask = ds.Tables[0].Rows[i]["hastask"].ToString(),
                        checktaskWeightage = ds.Tables[0].Rows[i]["checktaskWeightage"].ToString(),
                        hassubtask = ds.Tables[0].Rows[i]["hassubtask"].ToString(),
                        checksubtaskWeightage = ds.Tables[0].Rows[i]["checksubtaskWeightage"].ToString(),
                        leftdays = get_tdays("left", ds.Tables[0].Rows[i]["StartDate"].ToString(), ds.Tables[0].Rows[i]["TargetDate"].ToString()),
                        txtdays = get_tdays("total", ds.Tables[0].Rows[i]["StartDate"].ToString(), ds.Tables[0].Rows[i]["TargetDate"].ToString()),
                        complete_value = Math.Round(mileTaskTotal, 2).ToString(),
                        pending_value = Math.Round(mileWattagepending, 2).ToString(),
                        totalmile = _sumarryM.total.ToString(),
                        completemile = _sumarryM.complete.ToString(),
                        completemile_rate = _sumarryM.complete_rate.ToString(),
                        pendingmile = _sumarryM.pending.ToString(),
                        pendingmile_rate = _sumarryM.pending_rate.ToString(),

                        totaltask = _sumarryT.total.ToString(),
                        completetask = _sumarryT.complete.ToString(),
                        completetask_rate = _sumarryT.complete_rate.ToString(),
                        pendingtask = _sumarryT.pending.ToString(),
                        pendingtask_rate = _sumarryT.pending_rate.ToString(),

                        totalissue = _sumarryI.total.ToString(),
                        completeissue = _sumarryI.complete.ToString(),
                        completeissue_rate = _sumarryI.complete_rate.ToString(),
                        pendingissue = _sumarryI.pending.ToString(),
                        pendingissue_rate = _sumarryI.pending_rate.ToString(),
                        tags = newtaglist_p,
                        owners = newownerlist_p,
                        teams = newteamlist_p,
                        viewfiles = newfileslist,
                        extendedlists = extendlist

                    });
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
                    if (ftype == "project")
                    {
                        isshowf = 1;
                        filepath = "/assets/images/" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "/" + fentryid+"/";                        
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
                    isshow=(isshowf==1?true:false)


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
    public static issuesdetails bindissues_details(string id, string issuetype)
    {
        DataAccess da = new DataAccess();
        issuesdetails obj = new issuesdetails();

        try
        {
            if (string.IsNullOrEmpty(id))
                return obj;

            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null)
                return obj;


            string regid = regCookie["id"];
            int totalissues = 0;
            int completeissues = 0;
            int pendingissues = 0;
            double completeissue_rates = 0;
            double pendingissue_rates = 100;
            #region find issues
            try
            {

                totalissues = get_totalissue(id, issuetype);
                completeissues = get_completeissue(id, issuetype);
               if (completeissues > 0)
                {
                    pendingissues = totalissues - completeissues;
                    completeissue_rates = Math.Round((Convert.ToDouble(completeissues) / totalissues) * 100, 2);
                    pendingissue_rates = 100 - completeissue_rates;
                }
                else
                {
                    pendingissues = totalissues;
                    completeissue_rates = 0;
                    pendingissue_rates = 100;
                }

            }
            catch
            {

            }
            #endregion
            obj.totalissue = totalissues;
            obj.completeissue = completeissues;
            obj.pendingissue = pendingissues;
            obj.completeissues_rate = completeissue_rates;
            obj.pendingissues_rate = pendingissue_rates;

        }
        catch 
        {
           
            return new issuesdetails();
        }


        return obj;
    }

    protected static int get_totalissue(string id, string type)
    {
        int strret = 0;
        string milestr111 = "";

        if (type == "milestone")
        {
            milestr111 = "select COUNT(*) as countIssues from Pms_tbl_Addissues where  Milestoneid='" + id + "' and Issueon='milstone' and status=1 and archive=0 ";
        }
        else if (type == "task")
        {
            milestr111 = "select COUNT(*) as countIssues from Pms_tbl_Addissues where  taskid='" + id + "' and Issueon='task' and status=1 and archive=0 ";
        }
        else if (type == "subtask")
        {
            milestr111 = "select COUNT(*) as countIssues from Pms_tbl_Addissues where  subtaskid='" + id + "' and Issueon='subtask' and status=1 and archive=0 ";
        }
        if (id == "" || id == "0")
        {
            return 0;
        }
        DataAccess da = new DataAccess();

        DataSet miledss111 = da.GetDataSetInline(milestr111);
        if (miledss111.Tables[0].Rows.Count > 0)
        {

            try
            {
                strret = Convert.ToInt32(miledss111.Tables[0].Rows[0]["countIssues"]);
            }
            catch (Exception)
            {
            }
        }

        return strret;

    }
    protected static int get_completeissue(string id, string type)
    {
        int strret = 0;
        string milestr111 = "";

        if (type == "milestone")
        {

            milestr111 = "select COUNT(*) as countIssues from Pms_tbl_Addissues where Milestoneid='" + id + "' and Issueon='milstone' and IssueStatus in ( select id from Pms_tbl_Projectstatus where  status=1 and archive=0 and isnull(iscomplete,0)=1)  and status=1 and archive=0";
        }
        else if (type == "task")
        {
            milestr111 = "select COUNT(*) as countIssues from Pms_tbl_Addissues where taskid='" + id + "' and Issueon='task' and IssueStatus in ( select id from Pms_tbl_Projectstatus where  status=1 and archive=0 and isnull(iscomplete,0)=1)  and status=1 and archive=0";
        }
        else if (type == "subtask")
        {
            milestr111 = "select COUNT(*) as countIssues from Pms_tbl_Addissues where subtaskid='" + id + "' and Issueon='subtask' and IssueStatus in ( select id from Pms_tbl_Projectstatus where  status=1 and archive=0 and isnull(iscomplete,0)=1)  and status=1 and archive=0";
        }
        if (id == "" || id == "0")
        {
            return 0;
        }

        DataAccess da = new DataAccess();

        DataSet miledss111 = da.GetDataSetInline(milestr111);
        if (miledss111.Tables[0].Rows.Count > 0)
        {

            try
            {
                strret = Convert.ToInt32(miledss111.Tables[0].Rows[0]["countIssues"]);
            }
            catch
            {
               
            }
        }

        return strret;

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
        int total=0;
        int complete=0;
        int pending=0;
        double complete_rate=0;
        double pending_rate=0;

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
        list.total=total;
        list.complete=complete;
        list.pending=pending;
        list.complete_rate=complete_rate;
        list.pending_rate=pending_rate;
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


        string str2 = "select COUNT(*) as count_Issues from Pms_tbl_Addissues where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and  ProjectId='" +pid+ "' and Issueon='Project' and IssueStatus in ( select id from Pms_tbl_Projectstatus where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and   status=1 and archive=0 and isnull(iscomplete,0)=1)  and status=1 and archive=0";
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
    public static extendedlist_ret UpdateExtendeddatedtls(string projctid = "", string extendStrtdate = "", string extendEnddate = "")
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
        prm[7] = new SqlParameter("@RETURN_VALUE", SqlDbType.Int);
        prm[7].Direction = ParameterDirection.ReturnValue;

        int newid = da1.executeDMLQuery("pmstbl_Extnddate_DeatilsUpdate", prm);
        SaveLogText("strextendid-" + newid.ToString());

        extendedlist_ret list = new extendedlist_ret();
        // extendedtimeleine 


        string strextend = "select id,extendStartdate as StartDate,extendEnddate as TargetDate from Pms_Tbl_Extended_Timeline where  status=1 and Type='Project' and archive=0  and regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and ProjectID = '" + projctid + "' and id="+newid+" ";
        SaveLogText("strextend-"+ strextend);
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
                catch(Exception ex) { list.status = ex.Message; }
                


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
                        extended_days= estendDays.ToString(),
                        extended_percent ="100",
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
    public static string get_tdays(string type,string startdate, string enddate)
    {
        string retval = "";

        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        
        string tleftdays = "";
        #region bind timeline
        //Timeline
        if (startdate != string.Empty && enddate!=string.Empty)
        {
           
            double miledayscomplite = 0;


            DateTime mileStartDate = Convert.ToDateTime(startdate == "" ? DateTime.Now.ToString() : startdate);
            DateTime mileTargetdate = Convert.ToDateTime(enddate == "" ? DateTime.Now.ToString() :enddate);
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
        string  strsql = "select id,Startdate,Targetdate,team,owner from Pms_tbl_Addproject where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + id + "'";
        
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
        string strsql = " select * from Imt_state where status=1 and archive=0 and stateid='" + stateid + "'  ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["state"].ToString();

        }

        return strret;
    }
    public static string get_Countryname(string countryid)
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select * from tbl_countrymaster where status=1 and archive=0 and id='" + countryid + "'  ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret = dscode.Tables[0].Rows[0]["countryname"].ToString();

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
        if(ftype=="project")
        {
            return ftype;
        }
        string strsql = " select id,Milestone as ftypename from Pms_tbl_addmilestone where status=1 and archive=0 and projectid=" + projectid + " and id='" + fentryid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        if (ftype == "task")
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
    public static List<projectfiles> bindfiledata(string pid,string type)
    {

        DataAccess da = new DataAccess();
        string retval = "";


        string strsqlfils = "select * from Pms_tbl_projectfiles where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and status=1 and archive=0 and pid='" + pid + "' and fentryid='" + pid + "' and ftype='project' order by id desc";
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
                        filepath = "/assets/images/" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "/" + fentryid+"/";
                    else if(type == "milestone")
                        filepath = "/pms/MileImages/" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "/" + fentryid + "/";
                    else if (type == "task")
                        filepath = "/pms/TaskImages/" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "/" + fentryid + "/";
                    else if (type == "subtask")
                        filepath = "/pms/SubtaskImages/" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "/" + fentryid + "/";

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

                });

            }

        }


        return newprojectfiles;
    }
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

}
public class extendedlist_ret
{
    public string status { get; set; }
    public string id { get; set; }
    public DateTime? fromdate { get; set; }
    public DateTime? todate { get; set; }
    public string extended_from { get; set; }
    public string extended_to { get; set; }
    public string extended_days { get; set; }
    public string extended_percent { get; set; }
    public string extended_type { get; set; }
    public string delayed_days { get; set; }

}
public class projectteams
{
    public string id { get; set; }
    public string Team { get; set; }
    
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
    public string Countryid { get; set; }
    public string CountryName { get; set; }
    public string ClientName { get; set; }
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
    public string salesorderid { get; set; }
    public string hastask { get; set; }
    public string checktaskWeightage { get; set; }
    public string hassubtask { get; set; }
    public string checksubtaskWeightage { get; set; }
    public string leftdays { get; set; }
    public string txtdays { get; set; }
    public string complete_value { get; set; }
    public string pending_value { get; set; }

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



    public List<extendedlist> extendedlists { get; set; }
    public List<taglist> tags { get; set; }
    public List<ownerlist> owners { get; set; }
    public List<teamlist> teams { get; set; }
    public List<projectfiles> viewfiles { get; set; }

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
    public string fentryid { get; set; }
    public string ftype { get; set; }
    public string ftypename { get; set; }
    public string fileurl { get; set; }
    public string filedes { get; set; }
    public string fext { get; set; }
    public string filepath { get; set; }
    public bool  isshow { get; set; }


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
    public int? total { get; set; }
    public int? complete{ get; set; }
    public int? pending { get; set; }
    public double? complete_rate { get; set; }
    public double? pending_rate { get; set; }

}






