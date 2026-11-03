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
using System.Globalization;
using System.Text;
public partial class products_default : System.Web.UI.Page
{
    // Gantt shading uses the same default weekly-off rules as scheduling.
    // Only the requested visible dates are returned; holiday wins on overlap.
    [WebMethod]
    public static Dictionary<string, string> GetGanttNonWorkingDates(string startdate, string enddate)
    {
        Dictionary<string, string> result = new Dictionary<string, string>();
        DateTime start, end;
        int regid;
        HttpCookie admin = HttpContext.Current.Request.Cookies["cadmin"];
        if (admin == null || !Int32.TryParse(admin["id"], out regid) || regid <= 0 ||
            !DateTime.TryParseExact(startdate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out start) ||
            !DateTime.TryParseExact(enddate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out end) ||
            end < start || (end - start).TotalDays > 730) return result;

        DataAccess da = new DataAccess();
        string weekOffSql = "select top(1) * from tbl_weekoffmaster with(nolock) " +
            "where status=1 and archive=0 and isnull(isdefault,0)=1 and regid=" + regid;
        DataSet weekOffData = da.GetDataSetInline(weekOffSql);
        DataRow policy = weekOffData.Tables.Count > 0 && weekOffData.Tables[0].Rows.Count > 0
            ? weekOffData.Tables[0].Rows[0] : null;
        if (policy != null)
        {
            for (DateTime day = start; day <= end; day = day.AddDays(1))
            {
                int dayNumber = (int)day.DayOfWeek + 1;
                string column = dayNumber.ToString(CultureInfo.InvariantCulture);
                if (Convert.ToString(policy["isweekoff" + column]) != "1") continue;
                string rule = Convert.ToString(policy["isweekoff" + column + "type"]);
                if (getreturnstatus(rule, rule == "1" ? 0 : GetWeekNumber(day)) == 1)
                    result[day.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)] = "weekoff";
            }
        }

        string from = start.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
        string through = end.AddDays(1).ToString("yyyyMMdd", CultureInfo.InvariantCulture);
        string holidaySql = "select distinct cast(eventdate as date) as EventDate from holidays with(nolock) " +
            "where status=1 and archive=0 and regid=" + regid +
            " and cast(eventdate as date)>=convert(date,'" + from + "',112)" +
            " and cast(eventdate as date)<convert(date,'" + through + "',112)";
        DataSet holidayData = da.GetDataSetInline(holidaySql);
        if (holidayData.Tables.Count > 0)
            foreach (DataRow row in holidayData.Tables[0].Rows)
                result[Convert.ToDateTime(row["EventDate"]).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)] = "holiday";
        return result;
    }

    public function fns = new function();

    public int ischeckAllaccess = 0;
    public int ischeckCreate = 0;
    public int ischeckEdit = 0;
    public int ischeckDelete = 0;
    public int ischeckView = 0;
    public int ischeckAssign = 0;

    public int ischeckAllaccess_task = 0;
    public int ischeckCreate_task = 0;
    public int ischeckEdit_task = 0;
    public int ischeckDelete_task = 0;
    public int ischeckView_task = 0;
    public int ischeckAssign_task = 0;

    public int ischeckAllaccess_stask = 0;
    public int ischeckCreate_stask = 0;
    public int ischeckEdit_stask = 0;
    public int ischeckDelete_stask = 0;
    public int ischeckView_stask = 0;
    public int ischeckAssign_stask = 0;
    protected void Page_Load(object sender, EventArgs e)
    {

        //Response.Write(strvalidationS);
        string uid = "0";
        if (HttpContext.Current.Request.Cookies["user"]["user_id"] != null)
        {
            uid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();
        }
        #region milestone
        string strvalidationS = get_EditDelete_Validation("54",uid);
        //string strvalidationS = get_EditDelete_Validation("1056", uid);
        string[] arS = strvalidationS.Split(',');
        ischeckView = Convert.ToInt32(arS[0]);
        ischeckAllaccess = Convert.ToInt32(arS[1]);
        ischeckCreate = Convert.ToInt32(arS[2]);
        ischeckEdit = Convert.ToInt32(arS[3]);
        ischeckDelete = Convert.ToInt32(arS[4]);
        ischeckAssign = Convert.ToInt32(arS[5]);
        //Response.Write(ischeckDelete);
        //Response.End();
        #endregion
        #region task
        string strvalidationS_task = get_EditDelete_Validation("55", uid);
       // string strvalidationS_task = get_EditDelete_Validation("1057");
        string[] arS_task = strvalidationS_task.Split(',');

        ischeckView_task = Convert.ToInt32(arS_task[0]);
        ischeckAllaccess_task = Convert.ToInt32(arS_task[1]);
        ischeckCreate_task = Convert.ToInt32(arS_task[2]);
        ischeckEdit_task = Convert.ToInt32(arS_task[3]);
        ischeckDelete_task = Convert.ToInt32(arS_task[4]);
        ischeckAssign_task = Convert.ToInt32(arS_task[5]);
        #endregion
        #region sub task
        string strvalidationS_stask = get_EditDelete_Validation("56", uid);
        //string strvalidationS_stask = get_EditDelete_Validation("1058");
        string[] arS_stask = strvalidationS_stask.Split(',');

        ischeckView_stask = Convert.ToInt32(arS_stask[0]);
        ischeckAllaccess_stask = Convert.ToInt32(arS_stask[1]);
        ischeckCreate_stask = Convert.ToInt32(arS_stask[2]);
        ischeckEdit_stask = Convert.ToInt32(arS_stask[3]);
        ischeckDelete_stask = Convert.ToInt32(arS_stask[4]);
        ischeckAssign_stask = Convert.ToInt32(arS_stask[5]);
        #endregion
        if (ischeckView == 0 && ischeckCreate == 0 && ischeckEdit == 0 && ischeckDelete == 0 && ischeckAssign == 0 && ischeckCreate_task == 0 && ischeckEdit_task == 0 && ischeckDelete_task == 0 && ischeckAssign_task == 0 && ischeckCreate_stask == 0 && ischeckEdit_stask == 0 && ischeckDelete_stask == 0 && ischeckAssign_stask == 0)
        {
            Response.Redirect("/pms/accessdenied.aspx");
        }
        
        if (!IsPostBack)
        {
            //updateallstatus_subtask();
            //updateallstatus_task();
            //updateallstatus_mile();
            //ManageCriticalWorkflow("2172");
        }
    }
   
    // PMS calendar dates are date-only values. They must never depend on the
    // Windows culture/timezone of the web server. All browser requests use
    // yyyy-MM-dd and all SQL literals use invariant ISO/112 formats.
    private static readonly string[] DateOnlyInputFormats = new string[]
    {
        "yyyy-MM-dd",
        "yyyyMMdd",
        "MM/dd/yyyy", "M/d/yyyy",
        "dd/MM/yyyy", "d/M/yyyy",
        "dd-MM-yyyy", "d-M-yyyy",
        "yyyy-MM-ddTHH:mm:ss", "yyyy-MM-ddTHH:mm:ss.fff",
        "yyyy-MM-ddTHH:mm:ssK", "yyyy-MM-ddTHH:mm:ss.fffK", "yyyy-MM-ddTHH:mm:ss.FFFFFFFK",
        "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd HH:mm:ss.fff",
        "MM/dd/yyyy hh:mm tt", "M/d/yyyy h:mm tt",
        "MM/dd/yyyy HH:mm:ss", "M/d/yyyy H:mm:ss"
    };

    private static bool TryParseDateOnlyInvariant(object value, out DateTime result)
    {
        result = DateTime.MinValue;
        if (value == null || value == DBNull.Value) return false;

        if (value is DateTime)
        {
            result = DateTime.SpecifyKind(((DateTime)value).Date, DateTimeKind.Unspecified);
            return true;
        }

        string text = Convert.ToString(value, CultureInfo.InvariantCulture);
        if (String.IsNullOrWhiteSpace(text)) return false;
        text = text.Trim();

        DateTime parsed;
        if (DateTime.TryParseExact(
                text,
                DateOnlyInputFormats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AllowWhiteSpaces,
                out parsed))
        {
            result = DateTime.SpecifyKind(parsed.Date, DateTimeKind.Unspecified);
            return true;
        }

        return false;
    }

    private static object ParseDateOnly(object value)
    {
        DateTime parsed;
        return TryParseDateOnlyInvariant(value, out parsed)
            ? (object)parsed
            : DBNull.Value;
    }

    private static TimeSpan ParseTimeOfDayInvariant(string value, string fallback)
    {
        string text = String.IsNullOrWhiteSpace(value) ? fallback : value.Trim();
        DateTime parsed;
        string[] formats = new string[]
        {
            "hh:mm tt", "h:mm tt", "HH:mm", "H:mm", "HH:mm:ss", "H:mm:ss"
        };

        if (DateTime.TryParseExact(text, formats, CultureInfo.InvariantCulture,
            DateTimeStyles.AllowWhiteSpaces, out parsed))
            return parsed.TimeOfDay;

        if (!String.Equals(text, fallback, StringComparison.OrdinalIgnoreCase) &&
            DateTime.TryParseExact(fallback, formats, CultureInfo.InvariantCulture,
            DateTimeStyles.AllowWhiteSpaces, out parsed))
            return parsed.TimeOfDay;

        return TimeSpan.Zero;
    }

    private static string ToSqlDateOnly(DateTime value)
    {
        return value.Date.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
    }

    private static string ToSqlDateTime(DateTime value)
    {
        return value.ToString("yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
    }

    // SQL Server can return INT, BIGINT, SMALLINT or DECIMAL depending on the
    // production schema/stored procedure. The `as int?` operator works only
    // when the boxed value is exactly System.Int32 and silently returns null
    // for BIGINT/DECIMAL. Use an invariant conversion for scheduler IDs.
    private static int? ToNullableIntInvariant(object value)
    {
        if (value == null || value == DBNull.Value) return null;

        int result;
        if (Int32.TryParse(Convert.ToString(value, CultureInfo.InvariantCulture),
            NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
            return result;

        try
        {
            long longValue = Convert.ToInt64(value, CultureInfo.InvariantCulture);
            if (longValue >= Int32.MinValue && longValue <= Int32.MaxValue)
                return (int)longValue;
        }
        catch { }

        return null;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<projectlists> bindProjectdata(string currentid)
    {

        string projectdata = "";
        function fns = new function();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();

        DataSet ds = new DataSet();
        List<projectlists> projectlist = new List<projectlists>();
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
        SqlParameter[] prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[1] = new SqlParameter("@action", "GETBY_REGID");
        ds = da.GetDataSet("usp_pms_projecttemplates_all", prm);
        // if (isadmin == 1)
        //{
        //SqlParameter[] prm = new SqlParameter[1];
        //prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        //ds = da.GetDataSet("Pms_select_tblprojectall_dashbord", prm);

        //}
        //else
        //{
        //    SqlParameter[] prm = new SqlParameter[2];
        //    prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        //    prm[1] = new SqlParameter("@owner", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        //    ds = da.GetDataSet("Pms_select_tblprojectall_dashbord_ownerwise", prm);

        //}



        if (ds.Tables[0].Rows.Count > 0)
        {

            string streditP = "0";
            string strdeleteP = "0";
            string strownerP = "0";

            string strviewM = "0";
            string strviewT = "0";
            string strviewI = "0";
            string strviewD = "0";

            // SaveLogText("edtdel:" + strviewI+","+ strviewD);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (ds.Tables[0].Rows[i]["id"].ToString() != currentid)
                {
                    //ProjectName = ds.Tables[0].Rows[i]["ProjectName"].ToString(),
                    projectlist.Add(new projectlists
                    {
                        id = ds.Tables[0].Rows[i]["id"].ToString(),
                        ProjectName = ds.Tables[0].Rows[i]["templatename"].ToString(),

                    });
                }
            }
        }

        return projectlist;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<projectlists> bindProjectdata_old(string currentid)
    {

        string projectdata = "";
        function fns = new function();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();

        DataSet ds = new DataSet();
        List<projectlists> projectlist = new List<projectlists>();
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

            // SaveLogText("edtdel:" + strviewI+","+ strviewD);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (ds.Tables[0].Rows[i]["id"].ToString() != currentid)
                {
                    projectlist.Add(new projectlists
                    {
                        id = ds.Tables[0].Rows[i]["id"].ToString(),
                        ProjectName = ds.Tables[0].Rows[i]["ProjectName"].ToString(),

                    });
                }
            }
        }

        return projectlist;
    }
    public static int getcopyid(string templateid)
    {
        int retval = 0;
        DataAccess da = new DataAccess();
        string strsql = "select id,isnull(pid,0) projectid from pms_tbl_projecttemplates where status=1 and archive=0 and id="+ templateid + "";
        try
        {
            DataSet ds = da.GetDataSetInline(strsql);
            if(ds.Tables[0].Rows.Count>0)
            {
                retval = Convert.ToInt32(ds.Tables[0].Rows[0]["projectid"].ToString());
            }
        }
        catch { }

        return retval;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string copyproject(string currentid,string copyprojectid)
    {

        string projectdata = "0";
        function fns = new function();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
      
        if (currentid != string.Empty && currentid != "0" && copyprojectid != string.Empty && copyprojectid != "0")
        {
            var cookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (cookie == null || string.IsNullOrEmpty(cookie["id"]))
            {
                throw new Exception("User not authenticated");
            }

            int currentId = Convert.ToInt32(currentid);
            //int copyId = Convert.ToInt32(copyprojectid);
            int copyId = getcopyid(copyprojectid);

            string regid = cookie["id"];

            SqlParameter[] prm = new SqlParameter[6];
            prm[0] = new SqlParameter("@CurrentProjectId", currentid);
            prm[1] = new SqlParameter("@CopyProjectId", copyId);
            prm[2] = new SqlParameter("@RegId", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[3] = new SqlParameter("@lastmodify_byid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
            prm[4] = new SqlParameter("@lastmodify_date", da.getcdatetime());

            prm[5] = new SqlParameter("ReturnValue", SqlDbType.Int);
            prm[5].Direction = ParameterDirection.ReturnValue;
            int retval = da.executeDMLQuery("usp_CopyProject_Flag", prm);
           if(retval==1)
            {
                #region copy all details of copyprojectid in currentid milestone,task,subtasks
                
                SqlParameter[] prm2 = new SqlParameter[4];
                prm2[0] = new SqlParameter("@CurrentProjectId", currentid);
                prm2[1] = new SqlParameter("@CopyProjectId", copyId);
                prm2[2] = new SqlParameter("@RegId", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prm2[3] = new SqlParameter("ReturnValue", SqlDbType.Int);
                prm2[3].Direction = ParameterDirection.ReturnValue;
                int retval2 = da.executeDMLQuery("sp_CopyProjectData", prm2);
                if (retval2 == 1)
                {
                    projectdata = "1";
                }
                #endregion
                // end
            }



            // return details of current project as updated

        }
        return projectdata;
    }
    #region load milestone
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<userlist> binduserlist()
    {
        string str2 = "select id,empcode as empid,DisplayName,fileupload1,SUBSTRING (DisplayName, 1, 2) as splinameone from tbl_employee where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
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
                empid = row["empid"].ToString(),
                name = row["DisplayName"].ToString(),
                profileimage = profile,
                shortname = row["splinameone"].ToString()

        });
        }
        return list;
    }
    // Preserve the selected item's team and limit it to the login user's allowed employees.
    private static string FilterAssignmentTeamEmployees(string userIds)
    {
        if (String.IsNullOrWhiteSpace(userIds)) return String.Empty;
        HttpCookie regCookie = HttpContext.Current.Request.Cookies["cadmin"];
        HttpCookie userCookie = HttpContext.Current.Request.Cookies["user"];
        if (regCookie == null || userCookie == null) return String.Empty;

        string regid = regCookie["id"] ?? "0";
        string uid = userCookie["user_id"] ?? "0";
        int isadmin = String.IsNullOrWhiteSpace(uid) ? 0 : checkadmin(uid);
        DataAccess da = new DataAccess();
        SqlParameter[] prm = new SqlParameter[3];
        prm[0] = new SqlParameter("@regid", regid);
        prm[1] = new SqlParameter("@isadmin", isadmin);
        prm[2] = new SqlParameter("@uid", uid);
        DataSet ds = da.GetDataSet("bindemployee_forTransfer", prm);
        if (ds == null || ds.Tables.Count == 0) return String.Empty;

        DataTable employees = ds.Tables[0];
        // The procedure must return the tbl_employee primary key as id.
        if (!employees.Columns.Contains("id"))
            throw new InvalidOperationException("bindemployee_forTransfer must return employee ID as id.");

        HashSet<int> allowedIds = new HashSet<int>();
        foreach (DataRow employee in employees.Rows)
        {
            int employeeId;
            if (Int32.TryParse(Convert.ToString(employee["id"], CultureInfo.InvariantCulture), out employeeId) && employeeId > 0)
                allowedIds.Add(employeeId);
        }

        List<string> filteredIds = new List<string>();
        HashSet<int> seenIds = new HashSet<int>();
        foreach (string value in userIds.Split(','))
        {
            int employeeId;
            if (Int32.TryParse(value.Trim(), out employeeId) && employeeId > 0 &&
                allowedIds.Contains(employeeId) && seenIds.Add(employeeId))
                filteredIds.Add(employeeId.ToString(CultureInfo.InvariantCulture));
        }
        return String.Join(",", filteredIds.ToArray());
    }

    [WebMethod]
    public static List<userlist_withtask> getuserwithtasklist(string userIds,string checkdate)
    {
        userIds = FilterAssignmentTeamEmployees(userIds);
        CommonFunction cmf = new CommonFunction();
        DataAccess da = new DataAccess();
        if(userIds==string.Empty)
         return new List<userlist_withtask>();

        string str2 = "select id,empcode as empid,DisplayName,fileupload1,SUBSTRING (DisplayName, 1, 2) as splinameone from tbl_employee WITH (NOLOCK) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id in("+ userIds + ")";
        DataSet drmile = da.GetDataSetInline(str2);
        List<userlist_withtask> list = new List<userlist_withtask>();
        foreach (DataRow row in drmile.Tables[0].Rows)
        {
            string profile = "";
            string prosname = "";
            if (row["fileupload1"].ToString() != "")
            {
                profile = row["fileupload1"].ToString();
            }
            string strshiftstart = "09:30 AM";
            string strhrs = "9";
            string strshiftend = "06:00 AM";
            //string strshift = getemployeeshift(row["id"].ToString());
            string strshift = getemployeeshift2(row["id"].ToString());
            if(strshift!=string.Empty)
            {
                string[] shift = strshift.Split('-');
                strshiftstart = shift[0];
                strhrs = shift[1];
                strshiftend = shift[2];
            }
            if(row["id"].ToString()=="4")
            {
                SaveLogText("Segment1-strshiftstart="+ strshiftstart+ ",strshiftend="+ strshiftend);
            }
            //usertasks= _assigntasks
            List<assignsubtasklist> _assigntasks = getAssignmentRowsFromAssignTable(row["id"].ToString(), checkdate);
            if (row["id"].ToString() == "4")
            {
                SaveLogText("Segment2-assigntasks=" + _assigntasks.Count.ToString());
            }
            List<taskSegment> segments =GenerateTaskSegments(_assigntasks,strshiftstart,strshiftend);
            if (row["id"].ToString() == "4")
            {
                SaveLogText("Segment4-segmenttasks=" + segments.Count.ToString());
            }
            list.Add(new userlist_withtask
            {
                id = row["id"].ToString(),
                empid = row["empid"].ToString(),
                name = row["DisplayName"].ToString(),
                profileimage = profile,
                shortname = row["splinameone"].ToString(),
                ShiftStart = strshiftstart,
                ShiftHours = strhrs,
                ShiftEnd = strshiftend,               
                usertasks = _assigntasks,
                taskSegments = segments

            });
        }

        
       

        return list;
    }
    [WebMethod]
    public static userlist_withtask getuserwithtasklist_byempid(string userIds, string checkdate)
    {
        userIds = FilterAssignmentTeamEmployees(userIds);
        CommonFunction cmf = new CommonFunction();
        DataAccess da = new DataAccess();
        if (userIds == string.Empty)
            return new userlist_withtask();

        string str2 = "select id,empcode as empid,DisplayName,fileupload1,SUBSTRING (DisplayName, 1, 2) as splinameone from tbl_employee WITH (NOLOCK) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id in(" + userIds + ")";
        DataSet drmile = da.GetDataSetInline(str2);
        userlist_withtask list = new userlist_withtask();
        foreach (DataRow row in drmile.Tables[0].Rows)
        {
            string profile = "";
            string prosname = "";
            if (row["fileupload1"].ToString() != "")
            {
                profile = row["fileupload1"].ToString();
            }
            string strshiftstart = "09:30 AM";
            string strhrs = "9";
            string strshiftend = "06:00 AM";
            //string strshift = getemployeeshift(row["id"].ToString());
            string strshift = getemployeeshift2(row["id"].ToString());
            if (strshift != string.Empty)
            {
                string[] shift = strshift.Split('-');
                strshiftstart = shift[0];
                strhrs = shift[1];
                strshiftend = shift[2];
            }
            //usertasks= _assigntasks
            List<assignsubtasklist> _assigntasks = getAssignmentRowsFromAssignTable(row["id"].ToString(), checkdate);
            List<taskSegment> segments = GenerateTaskSegments(_assigntasks, strshiftstart, strshiftend);
            list.id = row["id"].ToString();
            list.empid = row["empid"].ToString();
            list.name = row["DisplayName"].ToString();
            list.profileimage = profile;
            list.shortname = row["splinameone"].ToString();
            list.ShiftStart = strshiftstart;
            list.ShiftHours = strhrs;
            list.ShiftEnd = strshiftend;
            list.usertasks = _assigntasks;
            list.taskSegments = segments;
        }


        




        return list;
    }

    public static string getemployeeshift(string empid)
    {
        DataAccess da = new DataAccess();
        string ret = "";
        string strstart = "8";
        string strend = "9";
        string shiftStart = "09:00 AM";
        string shiftEnd = "06:00 PM";
        try
        {
           string sqlsql= "select emp.id,emp.ddlShift,shft.starttime,shft.endtime from tbl_employee as emp WITH (NOLOCK) inner join tbl_shift shft WITH (NOLOCK) on emp.ddlShift = shft.id where emp.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id="+empid+"";
            DataSet dsE = da.GetDataSetInline(sqlsql);
            if(dsE.Tables[0].Rows.Count>0)
            {
              if(dsE.Tables[0].Rows[0]["starttime"].ToString()!=string.Empty)
                {
                    shiftStart = dsE.Tables[0].Rows[0]["starttime"].ToString();
                }
                if (dsE.Tables[0].Rows[0]["endtime"].ToString()!=string.Empty)
                {
                    shiftEnd = dsE.Tables[0].Rows[0]["endtime"].ToString();
                }               
            }
            DateTime start = DateTime.Today.Add(ParseTimeOfDayInvariant(shiftStart, "09:00 AM"));
            DateTime end = DateTime.Today.Add(ParseTimeOfDayInvariant(shiftEnd, "06:00 PM"));
            int startHour = start.Hour; // 8
            double totalHours = (end - start).TotalHours; // 10
            int shiftHours = Convert.ToInt32(totalHours);
            strstart = startHour.ToString();
            strend = shiftHours.ToString();
        }
        catch
        {

        }
        //ret = shiftStart + "-" + shiftEnd;
        ret = strstart + "-" + strend;

        return ret;
    }
    public static string getemployeeshift2(string empid)
    {
        DataAccess da = new DataAccess();
        string ret = "";
        string strstart = "8";
        string strend = "9";
        string shiftStart = "09:00 AM";
        string shiftEnd = "06:00 PM";
        string shiftHrs = "8";
        try
        {
            string sqlsql = "select emp.id,emp.ddlShift,shft.starttime,shft.endtime from tbl_employee as emp WITH (NOLOCK) inner join tbl_shift shft WITH (NOLOCK) on emp.ddlShift = shft.id where emp.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id=" + empid + "";
            DataSet dsE = da.GetDataSetInline(sqlsql);
            if (dsE.Tables[0].Rows.Count > 0)
            {
                if (dsE.Tables[0].Rows[0]["starttime"].ToString() != string.Empty)
                {
                    shiftStart = dsE.Tables[0].Rows[0]["starttime"].ToString();
                }
                if (dsE.Tables[0].Rows[0]["endtime"].ToString() != string.Empty)
                {
                    shiftEnd = dsE.Tables[0].Rows[0]["endtime"].ToString();
                }
            }
            DateTime start = DateTime.Today.Add(ParseTimeOfDayInvariant(shiftStart, "09:00 AM"));
            DateTime end = DateTime.Today.Add(ParseTimeOfDayInvariant(shiftEnd, "06:00 PM"));
            int startHour = start.Hour; // 8
            double totalHours = (end - start).TotalHours; // 10
            int shiftHours = Convert.ToInt32(totalHours);
            shiftHrs = shiftHours.ToString();

        }
        catch
        {

        }
        ret = shiftStart + "-"+ shiftHrs + "-" + shiftEnd;        

        return ret;
    }
    public static List<assignsubtasklist> getemployeetasklistlist(string empids, string fromdate)
    { 
        DataAccess da = new DataAccess();
        int monthid = Convert.ToDateTime(da.getcdate()).Month;
        int yearid = Convert.ToDateTime(da.getcdate()).Year;
        int lastday = DateTime.DaysInMonth(yearid, monthid);
        List<assignsubtasklist> list = new List<assignsubtasklist>();
        // Load a date window, not only selected date.
        // Reason: one saved task row can split into multiple visual segments.
        // Example: 5 hours starts on 25-Jun, 30 min gap on 25-Jun, remaining continues after last task / 26-Jun.
        // If we load only selected date, the 26-Jun carried segment cannot be generated.
        DateTime selectedWorkDate = ParseAssignDateOnly(fromdate);
        string rangeStartDate = selectedWorkDate.AddDays(-30).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        string rangeEndDate = selectedWorkDate.AddDays(30).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

        SqlParameter[] prm1 = new SqlParameter[8];
        prm1[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm1[1] = new SqlParameter("@projectid", '%');
        prm1[2] = new SqlParameter("@milestoneid", '%');
        prm1[3] = new SqlParameter("@taskid", '%');
        prm1[4] = new SqlParameter("@ondate", "");
        prm1[5] = new SqlParameter("@startdate", rangeStartDate);
        prm1[6] = new SqlParameter("@enddate", rangeEndDate);
        prm1[7] = new SqlParameter("@userid", empids);
        string strprm = "";
        DataSet dsL = da.GetDataSet("pms_GetSubtaskList_user_view_exist", prm1);
        
        foreach (DataRow row in dsL.Tables[0].Rows)
        {          

            list.Add(new assignsubtasklist
            {
                id = row["id"] as int?,
                tasktype = row["tasktype"].ToString(),
                ProjectId = row["ProjectId"] as int?,
                Powner = row["Powner"].ToString(),
                ProjectName = row["ProjectName"].ToString(),
                Milestoneid = row["Milestoneid"] as int?,
                Milestone = row["Milestone"].ToString(),
                Task = row["task"] as int?,
                Taskname = row["taskname"].ToString(),
                Subtaskname = row["Subtaskname"].ToString(),
                SubTaskDescription = row["SubTaskDescription"].ToString(),
                P_Status = row["statusid"] as int?,
                status_name = row["statusname"].ToString(),
                status_color = row["statuscolor"].ToString(),
                Priority = row["Priority"] as int?,
                priority_name = row["Priorityname"].ToString(),
                priority_color = row["Prioritycolor"].ToString(),
                Startdate = GetNullableDate(row["Startdate"]),
                Targetdate = GetNullableDate(row["TargetDate"]),
                StartDateKey = GetDateOnlyString(row["Startdate"]),
                TargetDateKey = GetDateOnlyString(row["TargetDate"]),
                TaskDuration = row["TaskDuration"].ToString(),
                starttime= row["StartTime"].ToString()

            });
        }
        return list;
    }

    public static List<assignsubtasklist> getAssignmentRowsFromAssignTable(string empids, string fromdate)
    {
        DataAccess da = new DataAccess();
        List<assignsubtasklist> list = new List<assignsubtasklist>();
        try
        {
            string regid = HttpContext.Current.Request.Cookies["cadmin"]["id"];
            DateTime selectedDate = ParseAssignDateOnly(fromdate);
            string rangeStart = ToSqlDateOnly(selectedDate.AddDays(-30));
            string rangeEnd = ToSqlDateOnly(selectedDate.AddDays(30));
            string sql = @"
select 
 a.id as assignid,
 a.assigndatetime,
 a.tasktype,
 a.projectid,
 a.milestoneid,
 a.taskid,
 a.subtaskid,
 a.ownerid,
 a.startdatetime,
 a.enddatetime,
 isnull(a.taskduration,'01:00') as taskduration,
 p.ProjectName,
 m.Milestone,
 t.taskname,
 st.Subtaskname,
 st.SubTaskDescription,
 isnull(t.P_Status,isnull(m.P_Status,0)) as statusid,
 isnull(t.Priority,isnull(m.Priority,0)) as Priority
from Pms_tbl_assigntasks a with(nolock)
left join Pms_tbl_AddProject p with(nolock) on p.id=a.projectid
left join Pms_tbl_AddMilestone m with(nolock) on m.id=a.milestoneid
left join Pms_tbl_Addtask t with(nolock) on t.id=a.taskid
left join Pms_tbl_Addsubtask st with(nolock) on st.id=a.subtaskid
where a.status=1 and a.archive=0 
  and a.regid='" + regid + @"'
  and a.ownerid in(" + empids + @")
  and a.startdatetime < dateadd(day,1,convert(date,'" + rangeEnd + @"',112))
  and isnull(a.enddatetime,a.startdatetime) >= convert(date,'" + rangeStart + @"',112)
order by a.startdatetime, a.assigndatetime";
            SaveLogText("Sagmentdatails1=" + sql);
            DataSet ds = da.GetDataSetInline(sql);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string tasktype = NormalizeAssignmentTaskType(row["tasktype"].ToString());
                int? itemId = null;
                if (tasktype == "milestone") itemId = ToNullableIntInvariant(row["milestoneid"]);
                else if (tasktype == "task") itemId = ToNullableIntInvariant(row["taskid"]);
                else itemId = ToNullableIntInvariant(row["subtaskid"]);

                DateTime? sdt = GetNullableDate(row["startdatetime"]);
                DateTime? edt = GetNullableDate(row["enddatetime"]);
                string stime = sdt.HasValue ? sdt.Value.ToString("hh:mm tt") : "09:00 AM";

                list.Add(new assignsubtasklist
                {
                    id = itemId,
                    assignid = ToNullableIntInvariant(row["assignid"]),
                    assigndatetime = GetNullableDate(row["assigndatetime"]),
                    tasktype = tasktype,
                    ProjectId = ToNullableIntInvariant(row["projectid"]),
                    ProjectName = row["ProjectName"].ToString(),
                    Milestoneid = ToNullableIntInvariant(row["milestoneid"]),
                    Milestone = row["Milestone"].ToString(),
                    Task = ToNullableIntInvariant(row["taskid"]),
                    Taskname = row["taskname"].ToString(),
                    Subtaskname = row["Subtaskname"].ToString(),
                    SubTaskDescription = row["SubTaskDescription"].ToString(),
                    owner = row["ownerid"].ToString(),
                    Startdate = sdt,
                    Targetdate = edt,
                    StartDateKey = sdt.HasValue ? sdt.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : null,
                    TargetDateKey = edt.HasValue ? edt.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : null,
                    AssignmentDateTimeKey = sdt.HasValue ? sdt.Value.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture) : null,
                    TaskDuration = row["taskduration"].ToString(),
                    starttime = stime,
                    StartTime = stime,
                    P_Status = ToNullableIntInvariant(row["statusid"]),
                    Priority = ToNullableIntInvariant(row["Priority"])
                });
            }
        }
        catch (Exception ex)
        {
            SaveLogText("getAssignmentRowsFromAssignTableEX-" + ex.Message);
        }
        return list;
    }

    private static int ParseDurationMinutesInvariant(string duration, int fallbackMinutes)
    {
        if (String.IsNullOrWhiteSpace(duration)) return fallbackMinutes;

        string text = duration.Trim();
        try
        {
            // Accept all existing values without changing old screens:
            // 2:, 2:00, 02:00, :30, 2/30, 2 and 2.5.
            if (text.Contains(":") || text.Contains("/"))
            {
                char separator = text.Contains(":") ? ':' : '/';
                string[] parts = text.Split(new char[] { separator }, 2);

                int hours = 0;
                int minutes = 0;

                string hourText = parts.Length > 0 ? parts[0].Trim() : String.Empty;
                string minuteText = parts.Length > 1 ? parts[1].Trim() : String.Empty;

                if (!String.IsNullOrEmpty(hourText) &&
                    !Int32.TryParse(hourText, NumberStyles.Integer, CultureInfo.InvariantCulture, out hours))
                    return fallbackMinutes;

                if (!String.IsNullOrEmpty(minuteText) &&
                    !Int32.TryParse(minuteText, NumberStyles.Integer, CultureInfo.InvariantCulture, out minutes))
                    return fallbackMinutes;

                if (hours < 0 || minutes < 0) return fallbackMinutes;
                return checked((hours * 60) + minutes);
            }

            decimal numericHours;
            if (Decimal.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out numericHours) ||
                Decimal.TryParse(text, NumberStyles.Number, CultureInfo.CurrentCulture, out numericHours))
            {
                if (numericHours < 0) return fallbackMinutes;
                return Convert.ToInt32(Math.Round(numericHours * 60M, 0, MidpointRounding.AwayFromZero));
            }
        }
        catch { }

        return fallbackMinutes;
    }

    public static int GetDurationMinutes(string duration)
    {
        return ParseDurationMinutesInvariant(duration, 0);
    }

    private static string NormalizeDurationTextInvariant(string duration, string separateMinutes)
    {
        string raw = (duration ?? String.Empty).Trim();
        string minutePart = (separateMinutes ?? String.Empty).Trim();
        if (String.IsNullOrEmpty(raw) && String.IsNullOrEmpty(minutePart)) return String.Empty;

        string combined = raw;
        if (!String.IsNullOrEmpty(minutePart))
        {
            int separatorIndex = raw.IndexOf(':');
            if (separatorIndex < 0) separatorIndex = raw.IndexOf('/');

            if (separatorIndex < 0)
            {
                combined = raw + ":" + minutePart;
            }
            else
            {
                string existingMinutePart = separatorIndex + 1 < raw.Length
                    ? raw.Substring(separatorIndex + 1).Trim()
                    : String.Empty;
                if (String.IsNullOrEmpty(existingMinutePart))
                    combined = raw.Substring(0, separatorIndex) + ":" + minutePart;
            }
        }

        int totalMinutes = ParseDurationMinutesInvariant(combined, -1);
        if (totalMinutes < 0) return raw;

        int hours = totalMinutes / 60;
        int minutes = totalMinutes % 60;
        return hours.ToString(CultureInfo.InvariantCulture) + ":" + minutes.ToString("00", CultureInfo.InvariantCulture);
    }

    private static string GetBaseTableDuration(DataAccess da, string tasktype, object itemId, string candidate)
    {
        string normalizedCandidate = NormalizeDurationTextInvariant(candidate, String.Empty);
        try
        {
            tasktype = NormalizeAssignmentTaskType(tasktype);
            string tableName = GetAssignTableName(tasktype);
            string idText = itemId == null || itemId == DBNull.Value ? String.Empty : itemId.ToString();
            int parsedId;
            if (String.IsNullOrEmpty(tableName) || !Int32.TryParse(idText, out parsedId) || parsedId <= 0)
                return normalizedCandidate;

            string sql = "select top(1) TaskDuration from " + tableName +
                         " with(nolock) where id=" + parsedId +
                         " and status=1 and archive=0";
            DataSet ds = da.GetDataSetInline(sql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string rawDbDuration = ds.Tables[0].Rows[0]["TaskDuration"].ToString();
                string normalizedDbDuration = NormalizeDurationTextInvariant(rawDbDuration, String.Empty);
                if (!String.IsNullOrEmpty(normalizedDbDuration)) return normalizedDbDuration;
            }
        }
        catch (Exception ex)
        {
            SaveLogText("GetBaseTableDurationEX-" + ex.Message);
        }
        return normalizedCandidate;
    }


    public static List<taskSegment> GenerateTaskSegments(
    List<assignsubtasklist> tasks,
    string shiftStart,
    string shiftEnd)
    {
        SaveLogText("Sagmentdatails="+ tasks.Count.ToString()+ ",shiftStart="+ shiftStart+ ",shiftEnd="+ shiftEnd);
        // Assignment-table based runtime scheduler.
        // Older/planned rows stay fixed. Newer rows are split into free gaps around older rows.
        // This prevents "Find free slot" from shifting existing tasks, and it allows remaining
        // duration to continue after the last task or on the next date.
        List<taskSegment> segments = new List<taskSegment>();
        if (tasks == null || tasks.Count == 0) return segments;

        DateTime shiftStartTime = DateTime.Today.Add(ParseTimeOfDayInvariant(shiftStart, "09:00 AM"));
        DateTime shiftEndTime = DateTime.Today.Add(ParseTimeOfDayInvariant(shiftEnd, "06:00 PM"));
        bool isNightShift = false;
        if (shiftEndTime <= shiftStartTime)
        {
            isNightShift = true;
            shiftEndTime = shiftEndTime.AddDays(1);
        }

        double hourWidth = 100;

        var orderedTasks = tasks
            .OrderBy(x => x.assigndatetime ?? x.cdate ?? (x.Startdate ?? DateTime.Now))
            .ThenBy(x => (x.Startdate ?? DateTime.Now).Date)
            .ThenBy(x => string.IsNullOrEmpty(x.starttime) ? shiftStartTime : DateTime.Today.Add(ParseTimeOfDayInvariant(x.starttime, shiftStart)))
            .ToList();

        Func<assignsubtasklist, DateTime> getStart = (x) =>
        {
            DateTime od = (x.Startdate ?? DateTime.Now).Date;
            DateTime ost = od.Add(!string.IsNullOrEmpty(x.starttime)
                ? ParseTimeOfDayInvariant(x.starttime, shiftStart)
                : shiftStartTime.TimeOfDay);
            if (isNightShift && ost < od.Add(shiftStartTime.TimeOfDay)) ost = ost.AddDays(1);
            return ost;
        };

        Func<assignsubtasklist, DateTime> getPlanOrder = (x) =>
            x.assigndatetime ?? x.cdate ?? getStart(x);

        foreach (var t in orderedTasks)
        {
            int remainingMinutes = GetDurationMinutes(t.TaskDuration);
            if (remainingMinutes <= 0) continue;

            DateTime workDate = (t.Startdate ?? DateTime.Now).Date;
            DateTime cursor = getStart(t);
            DateTime currentOrder = getPlanOrder(t);

            int safety = 0;
            while (remainingMinutes > 0 && safety < 120)
            {
                safety++;

                DateTime dayShiftStart = workDate.Date.Add(shiftStartTime.TimeOfDay);
                DateTime dayShiftEnd = workDate.Date.Add(shiftEndTime.TimeOfDay);
                if (dayShiftEnd <= dayShiftStart) dayShiftEnd = dayShiftEnd.AddDays(1);

                if (isNightShift && cursor < dayShiftStart)
                    cursor = cursor.AddDays(1);

                if (cursor < dayShiftStart) cursor = dayShiftStart;
                if (cursor >= dayShiftEnd)
                {
                    workDate = workDate.AddDays(1);
                    cursor = workDate.Date.Add(shiftStartTime.TimeOfDay);
                    continue;
                }

                // Only older assignments are obstacles. They stay fixed.
                // New task is allowed only in gaps between these older planned tasks.
                var obstacles = orderedTasks
                    .Where(x => !Object.ReferenceEquals(x, t))
                    .Where(x =>
                    {
                        DateTime xOrder = getPlanOrder(x);
                        if (xOrder < currentOrder) return true;
                        if (xOrder == currentOrder && getStart(x) < getStart(t)) return true;
                        return false;
                    })
                    .Select(x =>
                    {
                        DateTime ost = getStart(x);
                        int om = GetDurationMinutes(x.TaskDuration);
                        return new { Start = ost, End = ost.AddMinutes(om) };
                    })
                    .Where(o => o.End > dayShiftStart && o.Start < dayShiftEnd)
                    .OrderBy(o => o.Start)
                    .ToList();

                DateTime freeEnd = dayShiftEnd;
                bool jumped = false;

                foreach (var o in obstacles)
                {
                    if (cursor >= o.End) continue;

                    if (cursor >= o.Start && cursor < o.End)
                    {
                        cursor = o.End;
                        jumped = true;
                        break;
                    }

                    if (o.Start > cursor)
                    {
                        freeEnd = o.Start < freeEnd ? o.Start : freeEnd;
                        break;
                    }
                }

                if (jumped) continue;

                if (freeEnd <= cursor)
                {
                    workDate = workDate.AddDays(1);
                    cursor = workDate.Date.Add(shiftStartTime.TimeOfDay);
                    continue;
                }

                int allocateMinutes = Math.Min(remainingMinutes, (int)(freeEnd - cursor).TotalMinutes);
                if (allocateMinutes <= 0)
                {
                    workDate = workDate.AddDays(1);
                    cursor = workDate.Date.Add(shiftStartTime.TimeOfDay);
                    continue;
                }

                double leftMinutes = (cursor - dayShiftStart).TotalMinutes;
                double left = (leftMinutes / 60.0) * hourWidth;
                double width = (allocateMinutes / 60.0) * hourWidth;
                if (width > 8) width = width - 4;

                segments.Add(new taskSegment
                {
                    assignid = t.assignid,
                    taskid = t.id,
                    tasktype = t.tasktype,
                    taskname = gettaskname2(t.tasktype,t.id.ToString()),
                    workDate = DateTime.SpecifyKind(cursor.Date, DateTimeKind.Unspecified),
                    workDateKey = cursor.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    DurationMinutes = allocateMinutes,
                    TaskDuration = t.TaskDuration,
                    StartTime = cursor.ToString("hh:mm tt"),
                    EndTime = cursor.AddMinutes(allocateMinutes).ToString("hh:mm tt"),
                    left = left,
                    width = width,
                    mid=t.Milestoneid.ToString(),
                    tid=t.Task.ToString(),
                });

                remainingMinutes -= allocateMinutes;
                cursor = cursor.AddMinutes(allocateMinutes);

                if (remainingMinutes > 0 && cursor >= dayShiftEnd)
                {
                    workDate = workDate.AddDays(1);
                    cursor = workDate.Date.Add(shiftStartTime.TimeOfDay);
                }
            }
        }

        return segments;
    }

    public static List<taskSegment> GenerateTaskSegmentsold(
    List<assignsubtasklist> tasks,
    string shiftStart,
    string shiftEnd)
    {
        List<taskSegment> segments =
            new List<taskSegment>();

        DateTime shiftStartTime =
            DateTime.Today.Add(ParseTimeOfDayInvariant(shiftStart, "09:00 AM"));

        DateTime shiftEndTime =
            DateTime.Today.Add(ParseTimeOfDayInvariant(shiftEnd, "06:00 PM"));

        int shiftMinutes =
            (int)(shiftEndTime - shiftStartTime)
            .TotalMinutes;

        foreach (var t in tasks)
        {
            int totalMinutes =
                GetDurationMinutes(t.TaskDuration);

            DateTime workDate =
                t.Startdate ?? DateTime.Now;

            while (totalMinutes > 0)
            {
                int allocateMinutes =
                    Math.Min(
                        totalMinutes,
                        shiftMinutes
                    );

                segments.Add(new taskSegment
                {
                    taskid = t.id,
                    tasktype = t.tasktype,
                    taskname = t.Subtaskname,
                    workDate = DateTime.SpecifyKind(workDate.Date, DateTimeKind.Unspecified),
                    workDateKey = workDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),

                    DurationMinutes =
                        allocateMinutes,

                    TaskDuration = t.TaskDuration,
                    StartTime = shiftStart,

                    EndTime = shiftStartTime
                        .AddMinutes(
                            allocateMinutes
                        )
                        .ToString("hh:mm tt")
                });

                totalMinutes -= allocateMinutes;

                workDate =
                    workDate.AddDays(1);
            }
        }

        return segments;
    }
    

    public static List<userlist> binduserlistnew()
    {
        string str2 = "select id,empcode as empid,DisplayName,fileupload1,SUBSTRING (DisplayName, 1, 2) as splinameone from tbl_employee where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
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
                empid = row["empid"].ToString(),
                name = row["DisplayName"].ToString(),
                profileimage = profile,
                shortname = row["splinameone"].ToString()

            });
        }
        return list;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<userlist> binduserlist_byrefid(string id,string utype,string stype)
    {
        DataAccess danew = new DataAccess();
        string struser ="";
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
        if (struser!="")
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
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<statuslist> bindstatuslist()
    {
        string str2 = "select id, ProjectStatus as name, PrintColor as colorname,isnull(iscomplete,0) as iscomplete, isnull(isdefault,0) isdefault from Pms_tbl_Projectstatus where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";

        DataAccess danew = new DataAccess();
        DataSet drmile = danew.GetDataSetInline(str2);

        List<statuslist> list = new List<statuslist>();

        foreach (DataRow row in drmile.Tables[0].Rows)
        {
            statuslist obj = new statuslist();
            obj.id = row["id"].ToString();
            obj.name = row["name"].ToString();
            obj.colorname = row["colorname"].ToString();
            obj.iscomplete = row["iscomplete"].ToString();
            obj.isdefault = row["isdefault"].ToString();
            list.Add(obj);
        }

        return list;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<tasksituationlist> bindtasksituationlist()
    {
        string str2 = "select id, CriticalLabel as name, ColorCode as colorname from Pms_tbl_CriticalLabel with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by id";

        DataAccess da = new DataAccess();
        DataSet ds = da.GetDataSetInline(str2);
        List<tasksituationlist> list = new List<tasksituationlist>();

        foreach (DataRow row in ds.Tables[0].Rows)
        {
            tasksituationlist obj = new tasksituationlist();
            obj.id = row["id"].ToString();
            obj.name = row["name"].ToString();
            obj.colorname = row["colorname"].ToString();
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
    public static project_details bindtaskdata(string RecordId)
    {
        DataAccess da = new DataAccess();

        project_details obj = new project_details();
        try
        {
            if (string.IsNullOrEmpty(RecordId))
                RecordId = "0";

            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null)
                return obj;

            string regid = regCookie["id"];

            int chkadmin = 0;
            string uid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();
            if (uid != string.Empty)
            {
                chkadmin = checkadmin(uid);
            }
            else
            {
                chkadmin = 0;
            }

            string strsql = "select id,ProjectID,ProjectName,Owner,team,ProjectStatus,AwardDate,StartDate,TargetDate,salesorderid,customerid,checkprojectWeightage,hastask,checktaskWeightage,hassubtask,checksubtaskWeightage from Pms_tbl_AddProject with(nolock) where id=" + RecordId + " and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";


            SqlDataReader dr = da.getDataReaderInline(strsql);
            while (dr.Read())
            {
                if (dr["Owner"].ToString() == uid)
                {
                    chkadmin = 1;
                }
                List<ownerlist> newownerlist_p = getemployeelist<ownerlist>(dr["Owner"].ToString());
                List<teamlist> newteamlist_p = getemployeelist<teamlist>(dr["team"].ToString());

                string highlightcolor = geteditcolor();
                obj.id = dr["id"] != DBNull.Value ? Convert.ToInt32(dr["id"]) : (int?)null;
                obj.ProjectID = dr["ProjectID"].ToString();
                obj.ProjectName = dr["ProjectName"].ToString();
                obj.Owner = dr["Owner"].ToString();
                obj.team = dr["team"].ToString();
                obj.ProjectStatus = Convert.ToInt32(dr["ProjectStatus"].ToString() == "" ? "0" : dr["ProjectStatus"].ToString());
                obj.AwardDate = GetDateOnlyString(dr["AwardDate"]);
                obj.StartDate = GetDateOnlyString(dr["StartDate"]);
                obj.TargetDate = GetDateOnlyString(dr["TargetDate"]);
                obj.salesorderid = dr["salesorderid"] as int?;
                obj.customerid = dr["customerid"] as int?;
                obj.checkprojectWeightage = dr["checkprojectWeightage"] as int?;
                obj.hastask = dr["hastask"] as int?;
                obj.checktaskWeightage = dr["checktaskWeightage"] as int?;
                obj.hassubtask = dr["hassubtask"] as int?;
                obj.checksubtaskWeightage = dr["checksubtaskWeightage"] as int?;
                obj.editcolor = highlightcolor;
                obj.owners = newownerlist_p;
                obj.teams = newteamlist_p;
                obj.isadmin = chkadmin == 1 ? true : false;
                List<newmilestone> addmilestones = new List<newmilestone>();
                try
                {
                    DataAccess danew = new DataAccess();
                    DataSet drmile = new DataSet();
                    string str2 = "select * from Pms_tbl_AddMilestone with(nolock) where regid='" + regid + "' and ProjectId='" + obj.id + "'" +
                        " and status=1 and archive=0 order by isnull(orderno,0)";
                    if (chkadmin != 1)
                    {
                        string chkview = chk_AnyPmsPermission();
                        if (chkview == "1")
                        {
                            //  str2 = "select * from Pms_tbl_AddMilestone where ProjectId='" + obj.id + "'" +
                            //" and status=1 and archive=0" +
                            //" EXISTS(SELECT * FROM STRING_SPLIT(Pms_tbl_AddMilestone.Owner, ',') WHERE value IN (" + uid + ")) or EXISTS(SELECT* FROM STRING_SPLIT(Pms_tbl_AddMilestone.team, ',') WHERE value IN(" + uid + "))" +
                            //" order by isnull(orderno,0)";

                            SqlParameter[] prmMile = new SqlParameter[3];
                            prmMile[0] = new SqlParameter("@regid", regid);
                            prmMile[1] = new SqlParameter("@projectid", obj.id);
                            prmMile[2] = new SqlParameter("@uid", uid);

                            drmile = danew.GetDataSet("Pms_GetMilestone_ByUserOwnerTeam", prmMile);
                        }

                    }
                    else
                    {
                        drmile = danew.GetDataSetInline(str2);
                    }
                    SaveLogText("drmileA=" + drmile.Tables[0].Rows.Count);
                    if (drmile.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in drmile.Tables[0].Rows)
                        {


                            int totaltasks = 0;

                            List<ownerlist> newownerlist = getemployeelist<ownerlist>(row["Owner"].ToString());
                            List<teamlist> newteamlist = getemployeelist<teamlist>(row["team"].ToString());
                            List<projectfiles> newfileslist = bindfiledata(row["projectid"].ToString(), row["id"].ToString(), "milestone");
                            List<newtask> newtasklist = bindtasklist_bymileid(row["id"].ToString(), row["projectid"].ToString());
                            List<issuestdata> newissueslist = bindissueslist(row["id"].ToString(), "milestone");
                            int totaldependencys = 0;
                            totaldependencys = Binddependencydata(row["id"].ToString(), "milestone");
                            totaltasks = checktaskby_milestoneid(row["id"].ToString());

                            double mileTaskTotal = 0;
                            mileTaskTotal = get_completionstatus(row["id"].ToString(), "milestone");
                            //SaveLogText("chkadmintodayEX-" + mileTaskTotal);
                            double mileWattagecomplite = mileTaskTotal;
                            double mileWattagepending = 100;
                            if (mileTaskTotal > 0)
                            {
                                mileWattagepending = mileWattagepending - mileTaskTotal;
                            }
                            string tleftdays = getleftdays_load(row["id"].ToString(), "milestone");
                            #region find issues
                            int totalissues = 0;
                            int completeissues = 0;
                            int pendingissues = 0;
                            double completeissue_rates = 0;
                            double pendingissue_rates = 0;
                            try
                            {

                                totalissues = get_totalissue(row["id"].ToString(), "milestone");
                                completeissues = get_completeissue(row["id"].ToString(), "milestone");
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

                            int reassign = 0;
                            string chk_assignto = getassignid(row["id"].ToString(), "milestone");
                            if (chk_assignto != string.Empty && chk_assignto != "0")
                            {
                                reassign = 1;
                            }
                            addmilestones.Add(new newmilestone
                            {

                                id = row["id"] as int?,
                                ProjectId = row["ProjectId"] as int?,
                                Milestone = row["Milestone"].ToString(),
                                Weightage = row["Weightage"] as double?,
                                
                                //StartDate = row["StartDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["StartDate"]) : null,
                                StartDate = GetDateOnlyString(row["StartDate"]),
                                //EndDate = row["EndDate"] as DateTime?,
                                EndDate = GetDateOnlyString(row["EndDate"]),
                                Owner = row["Owner"].ToString(),
                                assignto = getassignid(row["id"].ToString(), "milestone"),
                                Access = row["Access"] as int?,
                                team = row["team"].ToString(),
                                P_Status = row["P_Status"] as int?,
                                status_name = get_Projectstatus(row["P_Status"].ToString()),
                                status_color = get_Status_Color(row["P_Status"].ToString()),
                                Priority = row["Priority"] as int?,
                                priority_name = get_PriorityName(row["Priority"].ToString()),
                                priority_color = get_PriorityColor(row["Priority"].ToString()),
                                Budget = row["Budget"] as double?,
                                Actual = row["Actual"] as double?,
                                uid = row["uid"] as int?,
                                cdate = row["cdate"] as DateTime?,
                                istask = row["istask"] as int?,
                                checkmileWeightage = row["checkmileWeightage"] as int?,
                                iscomplete = row["iscomplete"] as int?,
                                iscompletedate = GetDateOnlyString(row["iscompletedate"]),
                                iscompletepercentage = row["iscompletepercentage"] as int?,
                                Durationtype = row["Durationtype"] as int?,
                                TaskDuration = get_task_hrs(row["TaskDuration"].ToString(), "hrs"),
                                TaskDuration_min = get_task_hrs(row["TaskDuration"].ToString(), "min"),

                                StartTime = row.Table.Columns.Contains("StartTime") ? row["StartTime"].ToString() : "",

                                Durationhourcal = row["Durationhourcal"] as double?,
                                orderno = row["orderno"] as int?,
                                miledescription = row["miledescription"].ToString(),
                                Uploaddrawing = row["Uploaddrawing"].ToString(),
                                txtdays = row["txtdays"].ToString(),
                                complete_value = Math.Round(mileTaskTotal, 2).ToString(),
                                pending_value = Math.Round(mileWattagepending, 2).ToString(),
                                taskcount = totaltasks,
                                isExpanded = false,
                                leftdays = tleftdays,
                                totalissue = totalissues,
                                completeissue = completeissues,
                                pendingissue = pendingissues,
                                completeissue_rate = completeissue_rates,
                                pendingissue_rate = pendingissue_rates,
                                totaldependency = totaldependencys,
                                isreassign = reassign,
                                owners = newownerlist,
                                teams = newteamlist,
                                viewfiles = newfileslist,
                                tasks = newtasklist,
                                CriticalLabelId= row["CriticalLabelId"].ToString(),
                                CriticalLabel =getlabelname_color(row["CriticalLabelId"].ToString(),"label"),
                                CriticalLabelcolor = getlabelname_color(row["CriticalLabelId"].ToString(),"color")




                            });
                        }
                    }
                    obj.miles = addmilestones;

                    // projectlist.Add(obj);
                }
                catch (Exception ex)
                {
                    SaveLogText("drmileA1=" + ex.Message);
                }

            }


        }
        catch (Exception ex)
        {
            SaveLogText("chkadmintodayEX-" + ex.Message);
            return new project_details();
        }

        return obj;
    }

    private static string BuildAssignmentItemKey(string taskType, object milestoneId, object taskId, object subtaskId)
    {
        string type = NormalizeAssignmentTaskType(taskType);
        string itemId = string.Empty;

        if (type == "milestone") itemId = Convert.ToString(milestoneId, CultureInfo.InvariantCulture);
        else if (type == "task") itemId = Convert.ToString(taskId, CultureInfo.InvariantCulture);
        else if (type == "subtask") itemId = Convert.ToString(subtaskId, CultureInfo.InvariantCulture);

        return string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(itemId)
            ? string.Empty
            : type + ":" + itemId.Trim();
    }

    private static HashSet<string> GetActiveAssignmentItemKeys(DataAccess da, string regid, string projectId)
    {
        HashSet<string> keys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        try
        {
            int safeProjectId;
            int safeRegId;
            if (!Int32.TryParse(projectId, out safeProjectId) || !Int32.TryParse(regid, out safeRegId))
                return keys;

            string sql = @"
select tasktype, milestoneid, taskid, subtaskid
from Pms_tbl_assigntasks with(nolock)
where status=1 and archive=0
  and regid=" + safeRegId.ToString(CultureInfo.InvariantCulture) + @"
  and projectid=" + safeProjectId.ToString(CultureInfo.InvariantCulture);

            DataSet ds = da.GetDataSetInline(sql);
            if (ds.Tables.Count == 0) return keys;

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string key = BuildAssignmentItemKey(
                    row["tasktype"].ToString(),
                    row["milestoneid"],
                    row["taskid"],
                    row["subtaskid"]);
                if (!string.IsNullOrEmpty(key)) keys.Add(key);
            }
        }
        catch (Exception ex)
        {
            // The right panel must still load even if the assignment-state lookup fails.
            SaveLogText("GetActiveAssignmentItemKeys-" + ex.Message);
        }
        return keys;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<unassignsubtasklist> getunassigntasklist(string ProjectId,string MilestoneId,string RecordId,string Rtype)
    {
        CommonFunction cmf = new CommonFunction();
        DataAccess da = new DataAccess();
        List<unassignsubtasklist> obj = new List<unassignsubtasklist>();
        var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
        if (regCookie == null)
            return obj;
        string regid = regCookie["id"];
        int chkadmin = 0;
        HashSet<string> activeAssignmentKeys = GetActiveAssignmentItemKeys(da, regid, ProjectId);
        
       
        try
        {
            //string str2 = "select * from Pms_tbl_Addsubtask where regid='" + regid + "' and task='" + RecordId + "' and status=1 and archive=0 order by isnull(orderno,0)";
            SqlParameter[] prm1 = new SqlParameter[5];
            prm1[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm1[1] = new SqlParameter("@projectid", ProjectId);
            prm1[2] = new SqlParameter("@milestoneid", MilestoneId);
            prm1[3] = new SqlParameter("@recordid", RecordId);
            if (Rtype=="milestone")
            prm1[4] = new SqlParameter("@labelid", '3');
            else if (Rtype == "task")
                prm1[4] = new SqlParameter("@labelid", '2');
            else if (Rtype == "subtask")
                prm1[4] = new SqlParameter("@labelid", '1');
            

            string strprm = "";
            SaveLogText("pms_GetSubtaskList="+ prm1[0].Value + "," + prm1[1].Value + "," + prm1[2].Value + "," + prm1[3].Value + "," + prm1[4].Value );
            DataSet dsL = da.GetDataSet("pms_GetSubtaskList_unassign_labelview", prm1);
            
            foreach (DataRow row in dsL.Tables[0].Rows)
            {

              
                string tasktypes = "subtask";
                try
                {
                    tasktypes = NormalizeAssignmentTaskType(row["tasktype"].ToString());
                }
                catch { }

                string assignmentKey = BuildAssignmentItemKey(
                    tasktypes,
                    row.Table.Columns.Contains("Milestoneid") ? row["Milestoneid"] : null,
                    row.Table.Columns.Contains("task") ? row["task"] : null,
                    tasktypes.Equals("subtask", StringComparison.OrdinalIgnoreCase) ? row["id"] : null);

                if (tasktypes.Equals("milestone", StringComparison.OrdinalIgnoreCase))
                    assignmentKey = "milestone:" + row["id"].ToString();
                else if (tasktypes.Equals("task", StringComparison.OrdinalIgnoreCase))
                    assignmentKey = "task:" + row["id"].ToString();

                bool isAlreadyAssigned = !string.IsNullOrEmpty(assignmentKey)
                    && activeAssignmentKeys.Contains(assignmentKey);
                #endregion
                //tasktype= row["tasktype"].ToString(),
                obj.Add(new unassignsubtasklist
                {

                    id = ToNullableIntInvariant(row["id"]),
                    tasktype = tasktypes,
                    ProjectId = ToNullableIntInvariant(row["ProjectId"]),
                    Powner = row["Powner"].ToString(),
                    owner = row["owner"].ToString(),
                    ProjectName = row["ProjectName"].ToString(),
                    Milestoneid = ToNullableIntInvariant(row["Milestoneid"]),
                    Milestone = row["Milestone"].ToString(),
                    Task = ToNullableIntInvariant(row["task"]),
                    Taskname = row["taskname"].ToString(),
                    Subtaskname = row["Subtaskname"].ToString(),
                    SubTaskDescription = row["SubTaskDescription"].ToString(),
                    P_Status = row["statusid"] as int?,
                    status_name = row["statusname"].ToString(),
                    status_color = row["statuscolor"].ToString(),
                    Priority = row["Priority"] as int?,
                    priority_name = row["Priorityname"].ToString(),
                    priority_color = row["Prioritycolor"].ToString(),
                    Startdate = GetDateOnlyString(row["Startdate"]),
                    Targetdate = GetDateOnlyString(row["TargetDate"]),
                    TaskDuration = GetBaseTableDuration(da, tasktypes, row["id"], row["TaskDuration"].ToString()),
                    isassigned = isAlreadyAssigned

                });
            }




        }
        catch (Exception ex)
        {
            SaveLogText("chkadminsubtaskEX-" + ex.Message);
            return new List<unassignsubtasklist>();
        }


        return obj;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static project_details bindProjectdetail_pre(string RecordId)
    {
        DataAccess da = new DataAccess();

        project_details obj = new project_details();
        try
        {
            if (string.IsNullOrEmpty(RecordId))
                RecordId = "0";

            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null)
                return obj;

            string regid = regCookie["id"];

            int chkadmin = 0;
            if (HttpContext.Current.Request.Cookies["user"]["user_id"] != null)
            {
                chkadmin = checkadmin(HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            }
            else
            {
                chkadmin = 0;
            }

            string strsql = "select * from Pms_tbl_AddProject where id=" + RecordId + " and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";

            //SaveLogText("mcount=" + strsql);
            DataSet dsP= da.GetDataSetInline(strsql);
           
            if (dsP.Tables[0].Rows.Count>0)
            {
                List<ownerlist> newownerlist_p = getemployeelist<ownerlist>(dsP.Tables[0].Rows[0]["Owner"].ToString());
                List<teamlist> newteamlist_p = getemployeelist<teamlist>(dsP.Tables[0].Rows[0]["team"].ToString());

                string highlightcolor = geteditcolor();
                obj.id = dsP.Tables[0].Rows[0]["id"] != DBNull.Value ? Convert.ToInt32(dsP.Tables[0].Rows[0]["id"]) : (int?)null;
                obj.ProjectID = dsP.Tables[0].Rows[0]["ProjectID"].ToString();
                obj.ProjectName = dsP.Tables[0].Rows[0]["ProjectName"].ToString();
                obj.Owner = dsP.Tables[0].Rows[0]["Owner"].ToString();
                obj.team = dsP.Tables[0].Rows[0]["team"].ToString();
                obj.ProjectStatus = Convert.ToInt32(dsP.Tables[0].Rows[0]["ProjectStatus"].ToString() == "" ? "0" : dsP.Tables[0].Rows[0]["ProjectStatus"].ToString());
                obj.AwardDate = GetDateOnlyString(dsP.Tables[0].Rows[0]["AwardDate"]);
                obj.StartDate = GetDateOnlyString(dsP.Tables[0].Rows[0]["StartDate"]);
                obj.TargetDate = GetDateOnlyString(dsP.Tables[0].Rows[0]["TargetDate"]);
                obj.salesorderid = dsP.Tables[0].Rows[0]["salesorderid"] as int?;
                obj.customerid = dsP.Tables[0].Rows[0]["customerid"] as int?;
                obj.checkprojectWeightage = dsP.Tables[0].Rows[0]["checkprojectWeightage"] as int?;
                obj.hastask = dsP.Tables[0].Rows[0]["hastask"] as int?;
                obj.checktaskWeightage = dsP.Tables[0].Rows[0]["checktaskWeightage"] as int?;
                obj.hassubtask = dsP.Tables[0].Rows[0]["hassubtask"] as int?;
                obj.checksubtaskWeightage = dsP.Tables[0].Rows[0]["checksubtaskWeightage"] as int?;
                obj.editcolor = highlightcolor;
                obj.owners = newownerlist_p;
                obj.teams = newteamlist_p;
                obj.isadmin = chkadmin == 1 ? true : false;
                List<newmilestone> addmilestones = new List<newmilestone>();
                
                    //string str2 = "select * from Pms_tbl_AddMilestone where ProjectId='" + obj.id + "' and status=1 and archive=0 order by isnull(orderno,0)";
                    //DataAccess danew = new DataAccess();
                    //DataSet drmile = danew.GetDataSetInline(str2);
                    
                        SqlParameter[] prm = new SqlParameter[2];
                    prm[0] = new SqlParameter("@ProjectId", RecordId);
                    prm[1] = new SqlParameter("@RegId", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                    //SaveLogText("mcount=" + prm[0].Value+","+ prm[1].Value);
                    
                        DataSet drmile = da.GetDataSet("usp_GetMilestones_pre", prm);
                    if (drmile.Tables[0].Rows.Count > 0)
                    {

                    #region update copy data
                    string sqlupdt = "update pms_tbl_addmilestone set status=0 , archive=1 where projectid=" + RecordId + " and regid=" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + " and status=1 and archive=0";                    
                    sqlupdt+=";update pms_tbl_addtask set status=0 , archive=1 where projectid=" + RecordId + "  and regid=" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + " and status=1 and archive=0";                   
                    sqlupdt+=";update pms_tbl_addsubtask set status=0 , archive=1 where projectid=" + RecordId + "  and regid=" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + " and status=1 and archive=0";
                    da.executeDMLQueryinline(sqlupdt);
                    #endregion

                    foreach (DataRow row in drmile.Tables[0].Rows)
                        {
                            try
                            {
                           
                            sqlupdt = "update pms_tbl_addmilestone set status=1 , archive=0 where projectid=" + RecordId + " and regid=" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + " and id="+row["id"]+" and isnull(iscopy,0)=1";
                            da.executeDMLQueryinline(sqlupdt);

                            int totaltasks = 0;

                                List<ownerlist> newownerlist = getemployeelist<ownerlist>(row["Owner"].ToString());
                                List<teamlist> newteamlist = getemployeelist<teamlist>(row["team"].ToString());
                                List<projectfiles> newfileslist = bindfiledata(row["projectid"].ToString(), row["id"].ToString(), "milestone");
                                List<newtask> newtasklist = bindtasklist_pre_bymileid(row["id"].ToString());
                                List<issuestdata> newissueslist = bindissueslist(row["id"].ToString(), "milestone");


                                int totaldependencys = 0;
                                totaldependencys = Binddependencydata(row["id"].ToString(), "milestone");
                                totaltasks = checktaskby_milestoneid(row["id"].ToString());

                                double mileTaskTotal = 0;
                                mileTaskTotal = get_completionstatus(row["id"].ToString(), "milestone");

                                double mileWattagecomplite = mileTaskTotal;
                                double mileWattagepending = 100;
                                if (mileTaskTotal > 0)
                                {
                                    mileWattagepending = mileWattagepending - mileTaskTotal;
                                }
                                string tleftdays = getleftdays_load(row["id"].ToString(), "milestone");
                                #region find issues
                                int totalissues = 0;
                                int completeissues = 0;
                                int pendingissues = 0;
                                double completeissue_rates = 0;
                                double pendingissue_rates = 0;
                                try
                                {

                                    totalissues = get_totalissue(row["id"].ToString(), "milestone");
                                    completeissues = get_completeissue(row["id"].ToString(), "milestone");
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

                                SaveLogText("mcount111=" + row["projectid"].ToString());
                            addmilestones.Add(new newmilestone
                            {

                                id = row["id"] as int?,
                                ProjectId = row["ProjectId"] as int?,
                                Milestone = row["Milestone"].ToString(),
                                Weightage = row["Weightage"] as double?,
                                //StartDate =Convert.ToDateTime(row["StartDate"] as DateTime?),
                                StartDate = GetDateOnlyString(row["StartDate"]),
                                EndDate = GetDateOnlyString(row["EndDate"]),
                                Owner = row["Owner"].ToString(),
                                assignto = getassignid(row["id"].ToString(), "milestone"),
                            Access = row["Access"] as int?,
                                    team = row["team"].ToString(),
                                    P_Status = row["P_Status"] as int?,
                                    status_name = get_Projectstatus(row["P_Status"].ToString()),
                                    status_color = get_Status_Color(row["P_Status"].ToString()),
                                    Priority = row["Priority"] as int?,
                                    priority_name = get_PriorityName(row["Priority"].ToString()),
                                    priority_color = get_PriorityColor(row["Priority"].ToString()),
                                    Budget = row["Budget"] as double?,
                                    Actual = row["Actual"] as double?,
                                    uid = row["uid"] as int?,
                                    cdate = row["cdate"] as DateTime?,
                                    istask = row["istask"] as int?,
                                    checkmileWeightage = row["checkmileWeightage"] as int?,
                                    iscomplete = row["iscomplete"] as int?,
                                    iscompletedate = GetDateOnlyString(row["iscompletedate"]),
                                    iscompletepercentage = row["iscompletepercentage"] as int?,
                                    Durationtype = row["Durationtype"] as int?,
                                    TaskDuration = get_task_hrs(row["TaskDuration"].ToString(), "hrs"),
                                    TaskDuration_min = get_task_hrs(row["TaskDuration"].ToString(), "min"),

                                    StartTime = row.Table.Columns.Contains("StartTime") ? row["StartTime"].ToString() : "",

                                    Durationhourcal = row["Durationhourcal"] as double?,
                                    orderno = row["orderno"] as int?,
                                    miledescription = row["miledescription"].ToString(),
                                    Uploaddrawing = row["Uploaddrawing"].ToString(),
                                    txtdays = row["txtdays"].ToString(),
                                    complete_value = Math.Round(mileTaskTotal, 2).ToString(),
                                    pending_value = Math.Round(mileWattagepending, 2).ToString(),
                                    taskcount = totaltasks,
                                    isExpanded = false,
                                    leftdays = tleftdays,
                                    totalissue = totalissues,
                                    completeissue = completeissues,
                                    pendingissue = pendingissues,
                                    completeissue_rate = completeissue_rates,
                                    pendingissue_rate = pendingissue_rates,
                                    totaldependency = totaldependencys,
                                    owners = newownerlist,
                                    teams = newteamlist,
                                    viewfiles = newfileslist,
                                    tasks = newtasklist



                                });
                            }
                            catch (Exception ex)
                            {
                                SaveLogText("mcount=" + ex.Message);
                            }
                        }
                    }
                    obj.miles = addmilestones;

                    // projectlist.Add(obj);
                

            }


        }
        catch (Exception ex)
        {
            SaveLogText("chkadmintodayEX-" + ex.Message);
            return new project_details();
        }

        return obj;
    }

    public static string chk_AnyPmsPermission(string permisstiontype = "")
    {

        string chkMile = "0";
        string chkTask = "0";
        string chkSubTask = "0";
        if (permisstiontype == string.Empty)
        {
            chkMile = chk_EditDelete_Validation("54"); // milestone
            chkTask = chk_EditDelete_Validation("55"); // task
            chkSubTask = chk_EditDelete_Validation("56"); // subtask
            if (chkMile == "1" || chkTask == "1" || chkSubTask == "1")
            {
                return "1";
            }
        }
        else if (permisstiontype == "2")
        {
            chkTask = chk_EditDelete_Validation("55"); // task
            chkSubTask = chk_EditDelete_Validation("56"); // subtask
            if (chkTask == "1" || chkSubTask == "1")
            {
                return "1";
            }
        }
        else if (permisstiontype == "3")
        {
            chkSubTask = chk_EditDelete_Validation("56"); // subtask
            if (chkSubTask == "1")
            {
                return "1";
            }
        }

        return "0";
    }
    public static string chk_EditDelete_Validation(string moduleid)
    {
        string retval = "0";
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
                SaveLogText("strsqlmoduleA-" + ds.Tables[0].Rows[0]["id"].ToString());
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
                    SaveLogText("strsqlmoduleA1-" + strsqlmodule);
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
                            retval = "1";
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
                            if (strView == "0" && (strCreate == "1" || strEdit == "1" || strDelete == "1" || strAssign == "1"))
                            {
                                strView = "1";
                            }

                        }

                    }

                }

            }
        }
        catch { }
        if (strFull == "1" || strView == "1" || strCreate == "1" || strEdit == "1" || strDelete == "1" || strAssign == "1" || strChangeOwner == "1")
            retval = "1";
        return retval;
    }
    protected static int checkprojectowner(string projectid, string userid)
    {
        int retval = 0;
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        function fns = new function();
        string strsql = "select id,ProjectID,ProjectName,Owner,team,ProjectStatus,AwardDate,StartDate,TargetDate,salesorderid,customerid,checkprojectWeightage,hastask,checktaskWeightage,hassubtask,checksubtaskWeightage from Pms_tbl_AddProject with(nolock) where id=" + projectid + " and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";


        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["Owner"].ToString() == userid)
            {
                retval = 1;
            }
        }
        return retval;
    }
    #region load task list
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static milestone_details bindmile_tasklist(string RecordId)
    {
        DataAccess da = new DataAccess();
        milestone_details obj = new milestone_details();
        try
        {
            if (string.IsNullOrEmpty(RecordId))
                RecordId = "0";

            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null)
                return obj;

            string regid = regCookie["id"];



            string strsql = "select * from Pms_tbl_Addmilestone where id=" + RecordId + " and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";


            SqlDataReader dr = da.getDataReaderInline(strsql);
            while (dr.Read())
            {

                obj.id = dr["id"] != DBNull.Value ? Convert.ToInt32(dr["id"]) : (int?)null;
                obj.ProjectId = dr["ProjectId"] != DBNull.Value ? Convert.ToInt32(dr["ProjectId"]) : (int?)null;
                obj.Milestone = dr["Milestone"].ToString();
                obj.Owner = dr["Owner"].ToString();
                obj.assignto = getassignid(dr["id"].ToString(), "milestone");
                obj.team = dr["team"].ToString();
                obj.P_Status = Convert.ToInt32(dr["P_Status"].ToString() == "" ? "0" : dr["P_Status"].ToString());
                obj.StartDate = GetDateOnlyString(dr["StartDate"]);
                obj.EndDate = GetDateOnlyString(dr["EndDate"]);
                obj.istask = dr["istask"] as int?;
                List<newtask> addtasks = new List<newtask>();
                try
                {
                    string str2 = "select * from Pms_tbl_Addtask where ProjectId='" + obj.ProjectId + "' and Milestoneid='" + RecordId + "' and status=1 and archive=0 order by isnull(orderno,0)";
                    DataAccess danew = new DataAccess();
                    DataSet drtask = danew.GetDataSetInline(str2);

                    foreach (DataRow row in drtask.Tables[0].Rows)
                    {
                        List<ownerlist> newownerlist = getemployeelist<ownerlist>(row["Owner"].ToString());
                        List<teamlist> newteamlist = getemployeelist<teamlist>(row["team"].ToString());
                        List<projectfiles> newfileslist = bindfiledata(row["ProjectId"].ToString(), row["id"].ToString(), "task");

                        double mileTaskTotal = 0;
                        mileTaskTotal = get_completionstatus(row["id"].ToString(), "milestone");
                        //SaveLogText("chkadmintodayEX-" + mileTaskTotal);
                        double mileWattagecomplite = mileTaskTotal;
                        double mileWattagepending = 100;
                        if (mileTaskTotal > 0)
                        {
                            mileWattagepending = mileWattagepending - mileTaskTotal;
                        }

                        addtasks.Add(new newtask
                        {

                            id = row["id"] as int?,
                            ProjectId = row["ProjectId"] as int?,
                            Milestoneid = row["Milestoneid"] as int?,
                            taskname = row["taskname"].ToString(),
                            Weightage = row["Weightage"] as double?,
                            Startdate = GetDateOnlyString(row["StartDate"]),
                            Targetdate = GetDateOnlyString(row["Targetdate"]),
                            owner = row["Owner"].ToString(),
                            assignto = getassignid(row["id"].ToString(), "task"),
                        team = row["team"].ToString(),
                            P_Status = row["P_Status"] as int?,
                            status_name = get_Projectstatus(row["P_Status"].ToString()),
                            status_color = get_Status_Color(row["P_Status"].ToString()),
                            Priority = row["Priority"] as int?,
                            priority_name = get_PriorityName(row["Priority"].ToString()),
                            priority_color = get_PriorityColor(row["Priority"].ToString()),
                            Budget = row["Budget"] as double?,
                            Actual = row["Actual"] as double?,
                            uid = row["uid"] as int?,
                            cdate = row["cdate"] as DateTime?,
                            issubtask = row["issubtask"] as int?,
                            iscompletepercentage = row["iscompletepercentage"] as double?,
                            TaskDuration = get_task_hrs(row["TaskDuration"].ToString(), "hrs"),
                            TaskDuration_min = get_task_hrs(row["TaskDuration"].ToString(), "min"),

                            StartTime = row.Table.Columns.Contains("StartTime") ? row["StartTime"].ToString() : "",

                            orderno = row["orderno"] as int?,
                            taskdescripton = row["taskdescripton"].ToString(),
                            txtdays = row["txtdays"].ToString(),
                            complete_value = Math.Round(mileTaskTotal, 2).ToString(),
                            pending_value = Math.Round(mileWattagepending, 2).ToString(),
                            owners = newownerlist,
                            teams = newteamlist,
                            viewfiles = newfileslist



                        });
                    }

                    obj.tasks = addtasks;

                    // projectlist.Add(obj);
                }
                catch { }

            }


        }
        catch (Exception ex)
        {
            SaveLogText("chkadmintaskEX-" + ex.Message);
            return new milestone_details();
        }


        return obj;
    }

    public static List<newtask> bindtasklist_bymileid(string RecordId, string projectid)
    {
        DataAccess da = new DataAccess();
        List<newtask> obj = new List<newtask>();

        try
        {
            if (string.IsNullOrEmpty(RecordId))
                RecordId = "0";

            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null)
                return obj;

            string regid = regCookie["id"];
            int chkadmin = 0;
            string uid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();
            if (uid != string.Empty)
            {
                chkadmin = checkadmin(uid);
            }
            else
            {
                chkadmin = 0;
            }
            int isprojectowner = checkprojectowner(projectid, uid);
            if (isprojectowner == 1)
            {
                chkadmin = 1;
            }
            DataSet drtask = new DataSet();
            DataAccess danew = new DataAccess();
            try
            {
                string str2 = "select * from Pms_tbl_Addtask where regid='" + regid + "' and Milestoneid='" + RecordId + "' and status=1 and archive=0 order by isnull(orderno,0)";

                if (chkadmin != 1)
                {
                    string chkview = chk_AnyPmsPermission("2");
                    if (chkview == "1")
                    {

                        SqlParameter[] prmTask = new SqlParameter[4];
                        prmTask[0] = new SqlParameter("@regid", regid);
                        prmTask[1] = new SqlParameter("@projectid", projectid);
                        prmTask[2] = new SqlParameter("@milestoneid", RecordId);
                        prmTask[3] = new SqlParameter("@uid", uid);

                        drtask = danew.GetDataSet("Pms_GetTasks_ByUserOwnerTeam", prmTask);
                    }

                }
                else
                {
                    drtask = danew.GetDataSetInline(str2);
                }
                SaveLogText("gettasklist-" + str2);



                foreach (DataRow row in drtask.Tables[0].Rows)
                {
                    List<ownerlist> newownerlist = getemployeelist<ownerlist>(row["Owner"].ToString());
                    List<teamlist> newteamlist = getemployeelist<teamlist>(row["team"].ToString());
                    List<projectfiles> newfileslist = bindfiledata(row["ProjectId"].ToString(), row["id"].ToString(), "task");
                    List<newsubtask> newsubtasklist = bindsubtasklist_bytaskid(row["id"].ToString(), row["ProjectId"].ToString(), row["Milestoneid"].ToString());
                    SaveLogText("getsubtasklistcount-" + newsubtasklist.Count);
                    int totaldependencys = 0;
                    totaldependencys = Binddependencydata(row["id"].ToString(), "task");
                    double mileTaskTotal = 0;
                    mileTaskTotal = get_completionstatus(row["id"].ToString(), "task");
                    //SaveLogText("chkadmintodayEX-" + mileTaskTotal);
                    double mileWattagecomplite = mileTaskTotal;
                    double mileWattagepending = 100;
                    if (mileTaskTotal > 0)
                    {
                        mileWattagepending = mileWattagepending - mileTaskTotal;
                    }
                    string tleftdays = getleftdays_load(row["id"].ToString(), "task");
                    #region find issues
                    int totalissues = 0;
                    int completeissues = 0;
                    int pendingissues = 0;
                    double completeissue_rates = 0;
                    double pendingissue_rates = 0;
                    try
                    {

                        totalissues = get_totalissue(row["id"].ToString(), "task");
                        completeissues = get_completeissue(row["id"].ToString(), "task");
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
                    int reassign = 0;
                    string chk_assignto = getassignid(row["id"].ToString(), "task");
                    if (chk_assignto != string.Empty && chk_assignto != "0")
                    {
                        reassign = 1;
                    }
                    obj.Add(new newtask
                    {

                        id = row["id"] as int?,
                        ProjectId = row["ProjectId"] as int?,
                        Milestoneid = row["Milestoneid"] as int?,
                        taskname = row["taskname"].ToString(),
                        Weightage = row["Weightage"] as double?,
                        Startdate = GetDateOnlyString(row["StartDate"]),
                        Targetdate = GetDateOnlyString(row["Targetdate"]),
                        owner = row["Owner"].ToString(),
                        assignto = getassignid(row["id"].ToString(), "task"),
                        team = row["team"].ToString(),
                        P_Status = row["P_Status"] as int?,
                        status_name = get_Projectstatus(row["P_Status"].ToString()),
                        status_color = get_Status_Color(row["P_Status"].ToString()),
                        Priority = row["Priority"] as int?,
                        priority_name = get_PriorityName(row["Priority"].ToString()),
                        priority_color = get_PriorityColor(row["Priority"].ToString()),
                        Budget = row["Budget"] as double?,
                        Actual = row["Actual"] as double?,
                        uid = row["uid"] as int?,
                        cdate = row["cdate"] as DateTime?,
                        issubtask = row["issubtask"] as int?,
                        iscomplete = row["iscomplete"] as int?,
                    iscompletepercentage = row["iscompletepercentage"] as double?,
                        TaskDuration = get_task_hrs(row["TaskDuration"].ToString(), "hrs"),
                        TaskDuration_min = get_task_hrs(row["TaskDuration"].ToString(), "min"),

                        StartTime = row.Table.Columns.Contains("StartTime") ? row["StartTime"].ToString() : "",
                        orderno = row["orderno"] as int?,
                        taskdescripton = row["taskdescripton"].ToString(),
                        txtdays = row["txtdays"].ToString(),
                        complete_value = Math.Round(mileTaskTotal, 2).ToString(),
                        pending_value = Math.Round(mileWattagepending, 2).ToString(),
                        isExpanded = false,
                        leftdays = tleftdays,
                        totalissue = totalissues,
                        completeissue = completeissues,
                        pendingissue = pendingissues,
                        completeissue_rate = completeissue_rates,
                        pendingissue_rate = pendingissue_rates,
                        totaldependency = totaldependencys,
                        isreassign = reassign,
                        owners = newownerlist,
                        teams = newteamlist,
                        viewfiles = newfileslist,
                        subtasks = newsubtasklist,
                        CriticalLabelId = row["CriticalLabelId"].ToString(),
                        CriticalLabel = getlabelname_color(row["CriticalLabelId"].ToString(), "label"),
                        CriticalLabelcolor = getlabelname_color(row["CriticalLabelId"].ToString(), "color")



                    });
                }



                // projectlist.Add(obj);
            }
            catch { }




        }
        catch (Exception ex)
        {
            SaveLogText("chkadmintaskEX-" + ex.Message);
            return new List<newtask>();
        }


        return obj;
    }
    public static List<newtask> bindtasklist_pre_bymileid(string RecordId)
    {
        DataAccess da = new DataAccess();
        List<newtask> obj = new List<newtask>();

        try
        {
            if (string.IsNullOrEmpty(RecordId))
                RecordId = "0";

            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null)
                return obj;

            string regid = regCookie["id"];


            try
            {
                string str2 = "select * from Pms_tbl_Addtask where regid='" + regid + "' and Milestoneid='" + RecordId + "' and status=0 and archive=1 and isnull(iscopy,0)=1 order by isnull(orderno,0)";

                SaveLogText("gettasklist-" + str2);
                DataAccess danew = new DataAccess();
                DataSet drtask = danew.GetDataSetInline(str2);

                foreach (DataRow row in drtask.Tables[0].Rows)
                {
                    List<ownerlist> newownerlist = getemployeelist<ownerlist>(row["Owner"].ToString());
                    List<teamlist> newteamlist = getemployeelist<teamlist>(row["team"].ToString());
                    List<projectfiles> newfileslist = bindfiledata(row["ProjectId"].ToString(), row["id"].ToString(), "task");
                    List<newsubtask> newsubtasklist = bindsubtasklist_pre_bytaskid(row["id"].ToString());
                    SaveLogText("getsubtasklistcount-" + newsubtasklist.Count);
                    int totaldependencys = 0;
                    totaldependencys = Binddependencydata(row["id"].ToString(), "task");
                    double mileTaskTotal = 0;
                    mileTaskTotal = get_completionstatus(row["id"].ToString(), "task");
                    //SaveLogText("chkadmintodayEX-" + mileTaskTotal);
                    double mileWattagecomplite = mileTaskTotal;
                    double mileWattagepending = 100;
                    if (mileTaskTotal > 0)
                    {
                        mileWattagepending = mileWattagepending - mileTaskTotal;
                    }
                    string tleftdays = getleftdays_load(row["id"].ToString(), "task");
                    #region find issues
                    int totalissues = 0;
                    int completeissues = 0;
                    int pendingissues = 0;
                    double completeissue_rates = 0;
                    double pendingissue_rates = 0;
                    try
                    {

                        totalissues = get_totalissue(row["id"].ToString(), "task");
                        completeissues = get_completeissue(row["id"].ToString(), "task");
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
                    obj.Add(new newtask
                    {

                        id = row["id"] as int?,
                        ProjectId = row["ProjectId"] as int?,
                        Milestoneid = row["Milestoneid"] as int?,
                        taskname = row["taskname"].ToString(),
                        Weightage = row["Weightage"] as double?,
                        Startdate = GetDateOnlyString(row["StartDate"]),
                        Targetdate = GetDateOnlyString(row["Targetdate"]),
                        owner = row["Owner"].ToString(),
                        assignto = getassignid(row["id"].ToString(), "task"),
                    team = row["team"].ToString(),
                        P_Status = row["P_Status"] as int?,
                        status_name = get_Projectstatus(row["P_Status"].ToString()),
                        status_color = get_Status_Color(row["P_Status"].ToString()),
                        Priority = row["Priority"] as int?,
                        priority_name = get_PriorityName(row["Priority"].ToString()),
                        priority_color = get_PriorityColor(row["Priority"].ToString()),
                        Budget = row["Budget"] as double?,
                        Actual = row["Actual"] as double?,
                        uid = row["uid"] as int?,
                        cdate = row["cdate"] as DateTime?,
                        issubtask = row["issubtask"] as int?,
                        iscompletepercentage = row["iscompletepercentage"] as double?,
                        TaskDuration = get_task_hrs(row["TaskDuration"].ToString(), "hrs"),
                        TaskDuration_min = get_task_hrs(row["TaskDuration"].ToString(), "min"),

                        StartTime = row.Table.Columns.Contains("StartTime") ? row["StartTime"].ToString() : "",
                        orderno = row["orderno"] as int?,
                        taskdescripton = row["taskdescripton"].ToString(),
                        txtdays = row["txtdays"].ToString(),
                        complete_value = Math.Round(mileTaskTotal, 2).ToString(),
                        pending_value = Math.Round(mileWattagepending, 2).ToString(),
                        isExpanded = false,
                        leftdays = tleftdays,
                        totalissue = totalissues,
                        completeissue = completeissues,
                        pendingissue = pendingissues,
                        completeissue_rate = completeissue_rates,
                        pendingissue_rate = pendingissue_rates,
                        totaldependency = totaldependencys,
                        owners = newownerlist,
                        teams = newteamlist,
                        viewfiles = newfileslist,
                        subtasks = newsubtasklist



                    });
                }



                // projectlist.Add(obj);
            }
            catch { }




        }
        catch (Exception ex)
        {
            SaveLogText("chkadmintaskEX-" + ex.Message);
            return new List<newtask>();
        }


        return obj;
    }
    public static List<newtask_details> bindtaskdetails_bymileid(string RecordId)
    {
        DataAccess da = new DataAccess();
        List<newtask_details> obj = new List<newtask_details>();

        try
        {
            if (string.IsNullOrEmpty(RecordId))
                RecordId = "0";

            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null)
                return obj;

            string regid = regCookie["id"];


            try
            {
                string str2 = "select * from Pms_tbl_Addtask where regid='" + regid + "' and Milestoneid='" + RecordId + "' and status=1 and archive=0 order by isnull(orderno,0)";

                SaveLogText("gettasklist-" + str2);
                DataAccess danew = new DataAccess();
                DataSet drtask = danew.GetDataSetInline(str2);

                foreach (DataRow row in drtask.Tables[0].Rows)
                {
                    
                     List<newsubtask_details> newsubtasklist = bindsubtaskdetails_bytaskid(row["id"].ToString());
                    
                    double mileTaskTotal = 0;
                    mileTaskTotal = get_completionstatus(row["id"].ToString(), "task");
                    //SaveLogText("chkadmintodayEX-" + mileTaskTotal);
                    double mileWattagecomplite = mileTaskTotal;
                    double mileWattagepending = 100;
                    if (mileTaskTotal > 0)
                    {
                        mileWattagepending = mileWattagepending - mileTaskTotal;
                    }
                    obj.Add(new newtask_details
                    {
                        id = row["id"] as int?,
                        ProjectId = row["ProjectId"] as int?,
                        Milestoneid = row["Milestoneid"] as int?,                        
                        complete_value =Math.Round(mileTaskTotal, 2),
                        pending_value =Math.Round(mileWattagepending, 2),
                        subtasks_details= newsubtasklist

                    });
                }
                // projectlist.Add(obj);
            }
            catch { }
        }
        catch (Exception ex)
        {
            SaveLogText("chkadmintaskEX-" + ex.Message);
            return new List<newtask_details>();
        }


        return obj;
    }

    public static List<newsubtask> bindsubtasklist_bytaskid(string RecordId, string projectid, string milesetonid)
    {
        DataAccess da = new DataAccess();
        List<newsubtask> obj = new List<newsubtask>();
        try
        {
            if (string.IsNullOrEmpty(RecordId))
                RecordId = "0";

            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null)
                return obj;

            string regid = regCookie["id"];
            int chkadmin = 0;
            string uid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();
            if (uid != string.Empty)
            {
                chkadmin = checkadmin(uid);
            }
            else
            {
                chkadmin = 0;
            }
            int isprojectowner = checkprojectowner(projectid, uid);
            if (isprojectowner == 1)
            {
                chkadmin = 1;
            }
            DataAccess danew = new DataAccess();
            DataSet drsubtask = new DataSet();
            try
            {
                string str2 = "select * from Pms_tbl_Addsubtask where regid='" + regid + "' and task='" + RecordId + "' and projectid=" + projectid + " and status=1 and archive=0 order by isnull(orderno,0)";



                if (chkadmin != 1)
                {
                    string chkview = chk_AnyPmsPermission("3");
                    if (chkview == "1")
                    {

                        SqlParameter[] prmTask = new SqlParameter[5];
                        prmTask[0] = new SqlParameter("@regid", regid);
                        prmTask[1] = new SqlParameter("@projectid", projectid);
                        prmTask[2] = new SqlParameter("@milestoneid", milesetonid);
                        prmTask[3] = new SqlParameter("@taskid", RecordId);
                        prmTask[4] = new SqlParameter("@uid", uid);

                        drsubtask = danew.GetDataSet("Pms_GetSubTasks_ByUserOwnerTeam", prmTask);
                    }

                }
                else
                {
                    drsubtask = danew.GetDataSetInline(str2);
                }
                drsubtask = danew.GetDataSetInline(str2);
                foreach (DataRow row in drsubtask.Tables[0].Rows)
                {
                    SaveLogText("getsubtaskobjlist-" + row["id"]);
                    List<ownerlist> newownerlist = getemployeelist<ownerlist>(row["Owner"].ToString());
                    List<teamlist> newteamlist = getemployeelist<teamlist>(row["team"].ToString());
                    List<projectfiles> newfileslist = bindfiledata(row["ProjectId"].ToString(), row["id"].ToString(), "subtask");
                    int totaldependencys = 0;
                    totaldependencys = Binddependencydata(row["id"].ToString(), "subtask");
                    double mileTaskTotal = 0;
                    mileTaskTotal = get_completionstatus(row["id"].ToString(), "subtask");
                    //SaveLogText("chkadmintodayEX-" + mileTaskTotal);
                    double mileWattagecomplite = mileTaskTotal;
                    double mileWattagepending = 100;
                    if (mileTaskTotal > 0)
                    {
                        mileWattagepending = mileWattagepending - mileTaskTotal;
                    }
                    string tleftdays = getleftdays_load(row["id"].ToString(), "subtask");
                    #region find issues
                    int totalissues = 0;
                    int completeissues = 0;
                    int pendingissues = 0;
                    double completeissue_rates = 0;
                    double pendingissue_rates = 0;
                    try
                    {

                        totalissues = get_totalissue(row["id"].ToString(), "subtask");
                        completeissues = get_completeissue(row["id"].ToString(), "subtask");
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
                    int reassign = 0;
                    string chk_assignto = getassignid(row["id"].ToString(), "subtask");
                    if (chk_assignto != string.Empty && chk_assignto != "0")
                    {
                        reassign = 1;
                    }
                    obj.Add(new newsubtask
                    {

                        id = row["id"] as int?,
                        ProjectId = row["ProjectId"] as int?,
                        Milestoneid = row["Milestoneid"] as int?,
                        Task = row["task"] as int?,
                        Subtaskname = row["Subtaskname"].ToString(),
                        Weightage = row["Weightage"] as double?,
                        Startdate = GetDateOnlyString(row["StartDate"]),
                        Targetdate = GetDateOnlyString(row["Targetdate"]),
                        owner = row["Owner"].ToString(),
                        assignto = getassignid(row["id"].ToString(), "subtask"),
                        team = row["team"].ToString(),
                        P_Status = row["P_Status"] as int?,
                        status_name = get_Projectstatus(row["P_Status"].ToString()),
                        status_color = get_Status_Color(row["P_Status"].ToString()),
                        Priority = row["Priority"] as int?,
                        priority_name = get_PriorityName(row["Priority"].ToString()),
                        priority_color = get_PriorityColor(row["Priority"].ToString()),
                        Budget = row["Budget"] as double?,
                        Actual = row["Actual"] as double?,
                        uid = row["uid"] as int?,
                        cdate = row["cdate"] as DateTime?,
                        iscomplete = row["iscomplete"] as int?,
                        iscompletepercentage = row["iscompletepercentage"] as double?,
                        TaskDuration = get_task_hrs(row["TaskDuration"].ToString(), "hrs"),
                        TaskDuration_min = get_task_hrs(row["TaskDuration"].ToString(), "min"),

                        StartTime = row.Table.Columns.Contains("StartTime") ? row["StartTime"].ToString() : "",
                        orderno = row["orderno"] as int?,
                        SubTaskDescription = row["SubTaskDescription"].ToString(),
                        txtdays = row["txtdays"].ToString(),
                        complete_value = Math.Round(mileTaskTotal, 2).ToString(),
                        pending_value = Math.Round(mileWattagepending, 2).ToString(),
                        leftdays = tleftdays,
                        totalissue = totalissues,
                        completeissue = completeissues,
                        pendingissue = pendingissues,
                        completeissue_rate = completeissue_rates,
                        pendingissue_rate = pendingissue_rates,
                        totaldependency = totaldependencys,
                        isreassign = reassign,
                        owners = newownerlist,
                        teams = newteamlist,
                        viewfiles = newfileslist,
                        CriticalLabelId = row["CriticalLabelId"].ToString(),
                        CriticalLabel = getlabelname_color(row["CriticalLabelId"].ToString(), "label"),
                        CriticalLabelcolor = getlabelname_color(row["CriticalLabelId"].ToString(), "color")



                    });
                }




                // projectlist.Add(obj);
            }
            catch { }




        }
        catch (Exception ex)
        {
            SaveLogText("chkadminsubtaskEX-" + ex.Message);
            return new List<newsubtask>();
        }


        return obj;
    }
    public static List<newsubtask> bindsubtasklist_pre_bytaskid(string RecordId)
    {
        DataAccess da = new DataAccess();
        List<newsubtask> obj = new List<newsubtask>();
        try
        {
            if (string.IsNullOrEmpty(RecordId))
                RecordId = "0";

            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null)
                return obj;

            string regid = regCookie["id"];


            try
            {
                string str2 = "select * from Pms_tbl_Addsubtask where regid='" + regid + "' and task='" + RecordId + "' and status=0 and archive=1 and isnull(iscopy,0)=1 order by isnull(orderno,0)";


                DataAccess danew = new DataAccess();
                DataSet drsubtask = danew.GetDataSetInline(str2);
                foreach (DataRow row in drsubtask.Tables[0].Rows)
                {
                    SaveLogText("getsubtaskobjlist-" + row["id"]);
                    List<ownerlist> newownerlist = getemployeelist<ownerlist>(row["Owner"].ToString());
                    List<teamlist> newteamlist = getemployeelist<teamlist>(row["team"].ToString());
                    List<projectfiles> newfileslist = bindfiledata(row["ProjectId"].ToString(), row["id"].ToString(), "subtask");
                    int totaldependencys = 0;
                    totaldependencys = Binddependencydata(row["id"].ToString(), "subtask");
                    double mileTaskTotal = 0;
                    mileTaskTotal = get_completionstatus(row["id"].ToString(), "subtask");
                    //SaveLogText("chkadmintodayEX-" + mileTaskTotal);
                    double mileWattagecomplite = mileTaskTotal;
                    double mileWattagepending = 100;
                    if (mileTaskTotal > 0)
                    {
                        mileWattagepending = mileWattagepending - mileTaskTotal;
                    }
                    string tleftdays = getleftdays_load(row["id"].ToString(), "subtask");
                    #region find issues
                    int totalissues = 0;
                    int completeissues = 0;
                    int pendingissues = 0;
                    double completeissue_rates = 0;
                    double pendingissue_rates = 0;
                    try
                    {

                        totalissues = get_totalissue(row["id"].ToString(), "subtask");
                        completeissues = get_completeissue(row["id"].ToString(), "subtask");
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
                    obj.Add(new newsubtask
                    {

                        id = row["id"] as int?,
                        ProjectId = row["ProjectId"] as int?,
                        Milestoneid = row["Milestoneid"] as int?,
                        Task = row["task"] as int?,
                        Subtaskname = row["Subtaskname"].ToString(),
                        Weightage = row["Weightage"] as double?,
                        Startdate = GetDateOnlyString(row["StartDate"]),
                        Targetdate = GetDateOnlyString(row["Targetdate"]),
                        owner = row["Owner"].ToString(),
                        assignto = getassignid(row["id"].ToString(), "subtask"),
                        team = row["team"].ToString(),
                        P_Status = row["P_Status"] as int?,
                        status_name = get_Projectstatus(row["P_Status"].ToString()),
                        status_color = get_Status_Color(row["P_Status"].ToString()),
                        Priority = row["Priority"] as int?,
                        priority_name = get_PriorityName(row["Priority"].ToString()),
                        priority_color = get_PriorityColor(row["Priority"].ToString()),
                        Budget = row["Budget"] as double?,
                        Actual = row["Actual"] as double?,
                        uid = row["uid"] as int?,
                        cdate = row["cdate"] as DateTime?,
                        iscompletepercentage = row["iscompletepercentage"] as double?,
                        TaskDuration = get_task_hrs(row["TaskDuration"].ToString(), "hrs"),
                        TaskDuration_min = get_task_hrs(row["TaskDuration"].ToString(), "min"),

                        StartTime = row.Table.Columns.Contains("StartTime") ? row["StartTime"].ToString() : "",

                        orderno = row["orderno"] as int?,
                        SubTaskDescription = row["SubTaskDescription"].ToString(),
                        txtdays = row["txtdays"].ToString(),
                        complete_value = Math.Round(mileTaskTotal, 2).ToString(),
                        pending_value = Math.Round(mileWattagepending, 2).ToString(),
                        leftdays = tleftdays,
                        totalissue = totalissues,
                        completeissue = completeissues,
                        pendingissue = pendingissues,
                        completeissue_rate = completeissue_rates,
                        pendingissue_rate = pendingissue_rates,
                        totaldependency = totaldependencys,
                        owners = newownerlist,
                        teams = newteamlist,
                        viewfiles = newfileslist



                    });
                }




                // projectlist.Add(obj);
            }
            catch { }




        }
        catch (Exception ex)
        {
            SaveLogText("chkadminsubtaskEX-" + ex.Message);
            return new List<newsubtask>();
        }


        return obj;
    }
    public static List<newsubtask_details> bindsubtaskdetails_bytaskid(string RecordId)
    {
        DataAccess da = new DataAccess();
        List<newsubtask_details> obj = new List<newsubtask_details>();
        try
        {
            if (string.IsNullOrEmpty(RecordId))
                RecordId = "0";

            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null)
                return obj;

            string regid = regCookie["id"];


            try
            {
                string str2 = "select * from Pms_tbl_Addsubtask where regid='" + regid + "' and task='" + RecordId + "' and status=1 and archive=0 order by isnull(orderno,0)";


                DataAccess danew = new DataAccess();
                DataSet drsubtask = danew.GetDataSetInline(str2);
                foreach (DataRow row in drsubtask.Tables[0].Rows)
                {
                       double mileTaskTotal = 0;
                    mileTaskTotal = get_completionstatus(row["id"].ToString(), "subtask");
                    double mileWattagecomplite = mileTaskTotal;
                    double mileWattagepending = 100;
                    if (mileTaskTotal > 0)
                    {
                        mileWattagepending = mileWattagepending - mileTaskTotal;
                    }                   
                    obj.Add(new newsubtask_details
                    {

                        id = row["id"] as int?,
                        ProjectId = row["ProjectId"] as int?,
                        Milestoneid = row["Milestoneid"] as int?,
                        Task = row["task"] as int?,                        
                        complete_value = Math.Round(mileTaskTotal, 2),
                        pending_value = Math.Round(mileWattagepending, 2),
                        

                    });
                }




                // projectlist.Add(obj);
            }
            catch { }




        }
        catch (Exception ex)
        {
            SaveLogText("chkadminsubtaskEX-" + ex.Message);
            return new List<newsubtask_details>();
        }


        return obj;
    }


    #endregion

    protected static string Mileget_Priority_listbind()
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = "select * from Pms_tbl_Priority where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret += "<ul class='sttus-list' data-simplebar style='max-height: 160px'>";
            for (int i = 0; i < dscode.Tables[0].Rows.Count; i++)
            {

                strret += "<li>" +
                  "<div class='dropdown-item p-0'>" +
                    "<a class='' href='javascript:void(0);' onclick = 'MileUpdateDatapropertydeatils(" + dscode.Tables[0].Rows[i]["id"].ToString() + ")' > " +
                    "<i class='bi bi-square-fill' style='color:" + dscode.Tables[0].Rows[i]["PrintColor"].ToString() + "' ></i> " + dscode.Tables[0].Rows[i]["name"].ToString() + "" +
                    "</a>" +
                    "</div>" +
                    "</li>";


            }
            strret += "</ul>";
        }

        return strret;


    }
    protected static string Projget_Priority_listbind()
    {
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = "select * from Pms_tbl_Priority where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            strret += "<ul class='sttus-list' data-simplebar style='max-height: 160px'>";
            for (int i = 0; i < dscode.Tables[0].Rows.Count; i++)
            {

                strret += "<li>" +
                  "<div class='dropdown-item p-0'>" +
                    "<a class='' href='javascript:void(0);' onclick = 'ProjUpdateDatapropertydeatils(" + dscode.Tables[0].Rows[i]["id"].ToString() + ")' > " +
                    "<i class='bi bi-square-fill' style='color:" + dscode.Tables[0].Rows[i]["PrintColor"].ToString() + "' ></i> " + dscode.Tables[0].Rows[i]["name"].ToString() + "" +
                    "</a>" +
                    "</div>" +
                    "</li>";


            }
            strret += "</ul>";
        }

        return strret;


    }
    protected static double get_completionstatus(string id,string type)
    {
        double strret = 0;
        string tblname = "Pms_tbl_AddMilestone";
        if(type=="task")
        {
            tblname = "Pms_tbl_Addtask";
        }
        else if (type == "subtask")
        {
            tblname = "Pms_tbl_Addsubtask";
        }
        if (id == "" || id == "0")
        {
            return 0;
        }
        DataAccess da = new DataAccess();               
        
        double mileTaskTotal = 0;
        string findmile = "select cast(ROUND(isnull(iscompletepercentage,0),2) AS DECIMAL(10,2)) as iscompletepercentage from "+ tblname + "  where id='" +id + "'  and status=1  and archive=0";
        SaveLogText("chkadmintodayEX-" + findmile);
        DataSet dsssummile = da.GetDataSetInline(findmile);

        try
        {
            mileTaskTotal = Convert.ToDouble(dsssummile.Tables[0].Rows[0]["iscompletepercentage"]);
            if(mileTaskTotal==99.99)
            {
                mileTaskTotal = 100;
            }
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
    protected static string get_Projectstatus(string statusid)
    {
        if(statusid==""|| statusid=="0")
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
    protected static string get_task_hrs(string val,string gettype)
    {
        if (String.IsNullOrWhiteSpace(val)) return String.Empty;
        int totalMinutes = ParseDurationMinutesInvariant(val, -1);
        if (totalMinutes < 0) return String.Empty;

        if (String.Equals(gettype, "hrs", StringComparison.OrdinalIgnoreCase))
            return (totalMinutes / 60).ToString(CultureInfo.InvariantCulture);

        if (String.Equals(gettype, "min", StringComparison.OrdinalIgnoreCase))
            return (totalMinutes % 60).ToString("00", CultureInfo.InvariantCulture);

        return String.Empty;
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
     protected static string getlabelname_color(string labelid,string gtype)
    {
        if (labelid == "" || labelid == "0")
        {
            return "-";
        }
        DataAccess da = new DataAccess();
        string strret = "";
        string strsql = " select id,CriticalLabel as labaelname,ColorCode from Pms_tbl_CriticalLabel with(nolock) where status=1 and archive=0 and Id='" + labelid + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            if(gtype=="label")
            strret = dscode.Tables[0].Rows[0]["labaelname"].ToString();
            if (gtype == "color")
                strret = dscode.Tables[0].Rows[0]["ColorCode"].ToString();

        }

        return strret;

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
    #region check admin login
    protected static int checkadmin(string struserid = "0")
    {
        int retval = 0;
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        function fns = new function();
        string strsql = "select Manage_Profile.* from Manage_Profile inner join tbl_employee on Manage_Profile.id=cast(tbl_employee.roleid as int) where tbl_employee.id='" + struserid + "'  and Manage_Profile.status=1 and Manage_Profile.archive=0 ";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["id"].ToString() == "1")
            {
                retval = 1;
            }
        }
        return retval;
    }
    protected static int checkroleid(string struserid = "0")
    {
        int retval = 0;
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        function fns = new function();
        string strsql = "select Manage_Profile.* from Manage_Profile inner join tbl_employee on Manage_Profile.id=cast(tbl_employee.roleid as int) where tbl_employee.id='" + struserid + "'  and Manage_Profile.status=1 and Manage_Profile.archive=0 ";
        try
        {
            DataSet ds = da.GetDataSetInline(strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {


                retval = Convert.ToInt32(ds.Tables[0].Rows[0]["id"].ToString());


            }
        }
        catch { }
        return retval;
    }
    #endregion
    #region get highlight color
    public static string geteditcolor()
    {
        string retval = "";
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        function fns = new function();
        string strsql = "select id,PrintColor from Pms_tbl_editcolor  where Pms_tbl_editcolor.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["PrintColor"].ToString() == string.Empty)
            {
                retval = "#fde796";
            }
            else
            {
                retval = ds.Tables[0].Rows[0]["PrintColor"].ToString();
            }
        }
        return retval;
    }
    #endregion

    #region get user authentication
    public static string get_EditDelete_Validation(string moduleid, string userid)
    {
        int chkadmin = 0;
        int chkroleid = 0;
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

            chkadmin = checkadmin(userid);

            if (chkadmin == 1)
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
                chkroleid = checkroleid(userid);
                string strsqlmodule = @"select crm_tbl_roletaskmaster.formname,crm_tbl_roletaskmaster.formurl, isnull(ismanage,0) ismanage,isnull(isschedule,0) isschedule,  isnull(ischangeowner,0) ischangeowner,
                            isnull(formaccess,0) formaccess, isnull(isinsert,0) isinsert, isnull(isupdate,0) isupdate, isnull(isdelete,0) isdelete, isnull(isview,0) isview, isnull(isimport,0) isimport, isnull(isexport,0) isexport, isnull(issendmail,0) issendmail, isnull(isviewmail,0) isviewmail, isnull(isdelmail,0) isdelmail, isnull(isfull,0) isfull, isnull(moduleid,0) moduleid,isnull(isassign,0) as isassign  
                                    from crm_tbl_roleaccessdetails inner join crm_tbl_roletaskmaster on crm_tbl_roleaccessdetails.moduleid=crm_tbl_roletaskmaster.id 
                                    where crm_tbl_roleaccessdetails.status=1 and  crm_tbl_roleaccessdetails.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and crm_tbl_roleaccessdetails.roleid='" + chkroleid + "' and entrytype='form' and moduleid=" + moduleid + "";
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
        catch { }
        retval = strFull + "," + strView + "," + strCreate + "," + strEdit + "," + strDelete + "," + strAssign + "," + strChangeOwner;
        return retval;
    }
    //public static string get_EditDelete_Validation(string moduleid)
    //{
    //    string retval = "";
    //    string strFull = "0";
    //    string strView = "0";
    //    string strCreate = "0";
    //    string strEdit = "0";
    //    string strDelete = "0";
    //    string strChangeOwner = "0";
    //    string strAssign = "0";
    //    DataAccess da = new DataAccess();
    //    try
    //    {

    //        string strsql = "select Manage_Profile.* from Manage_Profile inner join tbl_employee on Manage_Profile.id=cast(tbl_employee.roleid as int) where tbl_employee.id='" + HttpContext.Current.Request.Cookies["user"]["user_id"] + "'  and Manage_Profile.status=1 and Manage_Profile.archive=0";
    //        DataSet ds = da.GetDataSetInline(strsql);

    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            SaveLogText("strsqlmoduleA-" + ds.Tables[0].Rows[0]["id"].ToString());
    //            if (ds.Tables[0].Rows[0]["id"].ToString() == "1")
    //            {

    //                strFull = "1";
    //                strView = "1";
    //                strCreate = "1";
    //                strEdit = "1";
    //                strDelete = "1";
    //                strChangeOwner = "1";
    //                strAssign = "1";
    //            }
    //            else
    //            {
    //                string strsqlmodule = @"select crm_tbl_roletaskmaster.formname,crm_tbl_roletaskmaster.formurl, isnull(ismanage,0) ismanage,isnull(isschedule,0) isschedule,  isnull(ischangeowner,0) ischangeowner,
    //                        isnull(formaccess,0) formaccess, isnull(isinsert,0) isinsert, isnull(isupdate,0) isupdate, isnull(isdelete,0) isdelete, isnull(isview,0) isview, isnull(isimport,0) isimport, isnull(isexport,0) isexport, isnull(issendmail,0) issendmail, isnull(isviewmail,0) isviewmail, isnull(isdelmail,0) isdelmail, isnull(isfull,0) isfull, isnull(moduleid,0) moduleid,isnull(isassign,0) as isassign  
    //                                from crm_tbl_roleaccessdetails inner join crm_tbl_roletaskmaster on crm_tbl_roleaccessdetails.moduleid=crm_tbl_roletaskmaster.id 
    //                                where crm_tbl_roleaccessdetails.status=1 and  crm_tbl_roleaccessdetails.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and crm_tbl_roleaccessdetails.roleid='" + ds.Tables[0].Rows[0]["id"].ToString() + "' and entrytype='form' and moduleid=" + moduleid + "";
    //                SaveLogText("strsqlmoduleA1-" + strsqlmodule);
    //                DataSet dsmodule = da.GetDataSetInline(strsqlmodule);
    //                if (dsmodule.Tables[0].Rows.Count > 0)
    //                {
    //                    if (dsmodule.Tables[0].Rows[0]["isfull"].ToString() == "1" || dsmodule.Tables[0].Rows[0]["isfull"].ToString() == "True")
    //                    {
    //                        strFull = "1";
    //                    }
    //                    if (strFull == "1")
    //                    {
    //                        strView = "1";
    //                        strCreate = "1";
    //                        strEdit = "1";
    //                        strDelete = "1";
    //                        strChangeOwner = "1";
    //                        strAssign = "1";
    //                    }
    //                    else
    //                    {
    //                        if (dsmodule.Tables[0].Rows[0]["isview"].ToString() == "1" || dsmodule.Tables[0].Rows[0]["isview"].ToString() == "True")
    //                        {
    //                            strView = "1";
    //                        }
    //                        if (dsmodule.Tables[0].Rows[0]["isinsert"].ToString() == "1" || dsmodule.Tables[0].Rows[0]["isinsert"].ToString() == "True")
    //                        {
    //                            strCreate = "1";
    //                        }
    //                        if (dsmodule.Tables[0].Rows[0]["isupdate"].ToString() == "1" || dsmodule.Tables[0].Rows[0]["isupdate"].ToString() == "True")
    //                        {
    //                            strEdit = "1";
    //                        }
    //                        if (dsmodule.Tables[0].Rows[0]["isdelete"].ToString() == "1" || dsmodule.Tables[0].Rows[0]["isdelete"].ToString() == "True")
    //                        {
    //                            strDelete = "1";
    //                        }
    //                        if (dsmodule.Tables[0].Rows[0]["ischangeowner"].ToString() == "1" || dsmodule.Tables[0].Rows[0]["ischangeowner"].ToString() == "True")
    //                        {
    //                            strChangeOwner = "1";
    //                        }
    //                        if (dsmodule.Tables[0].Rows[0]["isassign"].ToString() == "1" || dsmodule.Tables[0].Rows[0]["isassign"].ToString() == "True")
    //                        {
    //                            strAssign = "1";
    //                        }
    //                        if (strView == "0" && (strCreate == "1" || strEdit == "1" || strDelete == "1"))
    //                        {
    //                            strView = "1";
    //                        }

    //                    }

    //                }

    //            }

    //        }
    //    }
    //    catch { }
    //    retval = strFull + "," + strView + "," + strCreate + "," + strEdit + "," + strDelete + "," + strAssign + "," + strChangeOwner;
    //    return retval;
    //}
    #endregion


    #region savedata project
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string savedata_project(project_details model)
    {
        try
        {
            DataSet ds = new DataSet();
            DataAccess da = new DataAccess();
            CommonFunction cmf = new CommonFunction();
            function fns = new function();
            SqlParameter[] prm = new SqlParameter[68];
            //prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            //prm[1] = new SqlParameter("@cdate", da.getcdatetime());
            //prm[2] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
            //prm[3] = new SqlParameter("@ptype", (object)model.ptype ?? DBNull.Value);
            //prm[4] = new SqlParameter("@pname", (object)model.pname ?? DBNull.Value);
            //prm[5] = new SqlParameter("@Unitval", (object)model.Unitval ?? DBNull.Value);
            //prm[6] = new SqlParameter("@SKUval", (object)model.SKUval ?? DBNull.Value);
            //prm[7] = new SqlParameter("@exciseproduct", (object)model.exciseproduct ?? DBNull.Value);
            //prm[8] = new SqlParameter("@issale", (object)model.issale ?? DBNull.Value);
            //prm[9] = new SqlParameter("@salesRate", (object)model.salesRate ?? DBNull.Value);
            //prm[10] = new SqlParameter("@salesaccount", (object)model.salesaccount ?? DBNull.Value);
            //prm[11] = new SqlParameter("@salesDescription", (object)model.salesDescription ?? DBNull.Value);
            //prm[12] = new SqlParameter("@salesTaxtype", (object)model.salesTaxtype ?? DBNull.Value);
            //prm[13] = new SqlParameter("@salestaxvalue", (object)model.salestaxvalue ?? DBNull.Value);

            //prm[14] = new SqlParameter("@ispurchase", (object)model.ispurchase ?? "0");
            //prm[15] = new SqlParameter("@PurchaseRate", (object)model.PurchaseRate ?? DBNull.Value);
            //prm[16] = new SqlParameter("@Purchaseaccount", (object)model.Purchaseaccount ?? DBNull.Value);
            //prm[17] = new SqlParameter("@PurchaseDescription", (object)model.PurchaseDescription ?? DBNull.Value);
            //prm[18] = new SqlParameter("@PurchaseTaxtype", (object)model.PurchaseTaxtype ?? DBNull.Value);
            //prm[19] = new SqlParameter("@Purchasetaxvalue", (object)model.Purchasetaxvalue ?? DBNull.Value);

            //prm[20] = new SqlParameter("@isTrackInventory", ToDbInt(model.isTrackInventory));          
            //prm[21] = new SqlParameter("@trackAccount", (object)model.trackAccount ?? DBNull.Value);
            //prm[22] = new SqlParameter("@OpeningStock", (object)model.OpeningStock ?? DBNull.Value);
            //prm[23] = new SqlParameter("@OpeningStockrateperunit", (object)model.OpeningStockrateperunit ?? DBNull.Value);

            //prm[24] = new SqlParameter("ReturnValue", SqlDbType.Int);
            //prm[24].Direction = ParameterDirection.ReturnValue;

            //prm[25] = new SqlParameter("@pimg", (object)model.pimg ?? DBNull.Value);
            //prm[26] = new SqlParameter("@categoryid", ToDbInt(model.categoryid));
            //prm[27] = new SqlParameter("@subcategoryid", ToDbInt(model.subcategoryid));
            //prm[28] = new SqlParameter("@rsaleprice", (object)model.rsaleprice ?? DBNull.Value);
            //prm[29] = new SqlParameter("@rrentprice", (object)model.rrentprice ?? DBNull.Value);
            //prm[30] = new SqlParameter("@rtype", (object)model.rtype ?? DBNull.Value);
            //if ((object)model.issalepurchasesame == "1")
            //{
            //    prm[31] = new SqlParameter("@Unitval2", (object)model.Unitval ?? DBNull.Value);
            //    prm[32] = new SqlParameter("@unitrelationvalue", "1");
            //    prm[33] = new SqlParameter("@unitvalue", (object)model.unitvalue ?? "0");
            //    prm[34] = new SqlParameter("@unitvalue2", (object)model.unitvalue ?? "0");
            //}
            //else
            //{
            //    prm[31] = new SqlParameter("@Unitval2", (object)model.Unitval2 ?? DBNull.Value);
            //    prm[32] = new SqlParameter("@unitrelationvalue", (object)model.unitrelationvalue ?? "1");
            //    prm[33] = new SqlParameter("@unitvalue", (object)model.unitvalue ?? "0");
            //    prm[34] = new SqlParameter("@unitvalue2", (object)model.unitvalue2 ?? "0");
            //}
            //prm[35] = new SqlParameter("@barcode", (object)model.barcode ?? DBNull.Value);
            //prm[36] = new SqlParameter("@txtreorder", (object)model.txtreorder ?? DBNull.Value);
            //prm[37] = new SqlParameter("@istaxinclusive", (object)model.istaxinclusive ?? DBNull.Value);
            //prm[38] = new SqlParameter("@Wholesale_Price", (object)model.Wholesale_Price ?? DBNull.Value);
            //prm[39] = new SqlParameter("@Wholesale_istaxinclusive", (object)model.Wholesale_istaxinclusive ?? DBNull.Value);
            //prm[40] = new SqlParameter("@Franchise_Price", (object)model.Franchise_Price ?? DBNull.Value);
            //prm[41] = new SqlParameter("@Franchise_istaxinclusive", (object)model.Franchise_istaxinclusive ?? DBNull.Value);
            //prm[42] = new SqlParameter("@Discount", (object)model.Discount ?? DBNull.Value);
            //prm[43] = new SqlParameter("@txtfocprice", (object)model.txtfocprice ?? DBNull.Value);
            //prm[44] = new SqlParameter("@txtordercode", (object)model.txtordercode ?? DBNull.Value);
            //prm[45] = new SqlParameter("@CaseBag_Size", (object)model.CaseBag_Size ?? DBNull.Value);
            //prm[46] = new SqlParameter("@txtbrand", (object)model.txtbrand ?? DBNull.Value);
            //prm[47] = new SqlParameter("@PcsCase_Bag", (object)model.PcsCase_Bag ?? DBNull.Value);
            //prm[48] = new SqlParameter("@groupId", (object)model.groupId ?? DBNull.Value);
            //prm[49] = new SqlParameter("@txtspecificgravity", (object)model.txtspecificgravity ?? DBNull.Value);
            //prm[50] = new SqlParameter("@txtsize", (object)model.txtsize ?? DBNull.Value);
            //prm[51] = new SqlParameter("@txtopensize", (object)model.txtopensize ?? DBNull.Value);
            //prm[52] = new SqlParameter("@txtmicrion", (object)model.txtmicrion ?? DBNull.Value);
            //prm[53] = new SqlParameter("@txtcutlength", (object)model.txtcutlength ?? DBNull.Value);
            //prm[54] = new SqlParameter("@subcategory2id", ToDbInt(model.subcategory2id));
            //prm[55] = new SqlParameter("@subcategory3id", ToDbInt(model.subcategory3id));
            //prm[56] = new SqlParameter("@subcategory4id", ToDbInt(model.subcategory4id));
            //prm[57] = new SqlParameter("@MaxLimit", (object)model.MaxLimit ?? DBNull.Value);
            //prm[58] = new SqlParameter("@C_Cat", (object)model.C_Cat ?? DBNull.Value);
            //prm[59] = new SqlParameter("@C_Mod", (object)model.C_Mod ?? DBNull.Value);
            //prm[60] = new SqlParameter("@C_End", (object)model.C_End ?? DBNull.Value);
            //prm[61] = new SqlParameter("@C_Material", (object)model.C_Material ?? DBNull.Value);
            //prm[62] = new SqlParameter("@C_Pick", (object)model.C_Pick ?? DBNull.Value);
            //prm[63] = new SqlParameter("@C_Meter", (object)model.C_Meter ?? DBNull.Value);
            //prm[64] = new SqlParameter("@ddlinventorytype", (object)model.ddlinventorytype ?? "1");
            //prm[65] = new SqlParameter("@type", model.QType);
            //prm[66] = new SqlParameter("@issalepurchasesame", (object)model.issalepurchasesame ?? "0");
            //prm[67] = new SqlParameter("@id", (object)model.id ?? "0");

            //int cusid = 0;
            //cusid = da.executeDMLQuery("angular_usp_tbl_itemsdetails", prm);
            

            return "success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    #endregion
    #region savedata milestone
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static int addnewmilestonerrequest(newMilerequest request)
    {
        function fns = new function();
        DataAccess da1 = new DataAccess();
        SaveLogText("newMilerequest-"+ request);
        var obj = request.milestone;
        int newOrderNo = request.orderno;
        string strpid = obj.ProjectId.ToString();
        string strweight = "";
        string strdurationtype = "";
        string strduration = "";
        string stristask = "";

        string strstartdate = obj.StartDate != null ? obj.StartDate.ToString() : "";
        string strenddate = obj.EndDate != null ? obj.EndDate.ToString() : "";

        string txtdays= obj.txtdays != null ? obj.txtdays.ToString() :"";
        string strowner = obj.Owner != null ? obj.Owner.ToString() : "";
        string strteam = obj.team != null ? obj.team.ToString() : "";
        string strtimeline = "";
        string strstatus = obj.P_Status.ToString();
        string strpriority = obj.Priority.ToString();
        
        string istasks = "0";
        string checkmileWeightages = "0";
        try
        {
            string sqlM = "select id,isnull(hastask,0) as istask,isnull(checkprojectWeightage,0) as checkmileWeightage from pms_tbl_addproject where status=1 and archive=0 and id=" + obj.ProjectId + " and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
            DataSet dsM = da1.GetDataSetInline(sqlM);
            if (dsM.Tables[0].Rows.Count > 0)
            {
                istasks = dsM.Tables[0].Rows[0]["istask"].ToString();
                checkmileWeightages = dsM.Tables[0].Rows[0]["checkmileWeightage"].ToString();
            }
        }
        catch { }
        if (istasks == "0")
        {
            if (obj.istask != null)
            {
                istasks = obj.istask.ToString(); ;
            }
        }
        if (checkmileWeightages == "0")
        {
            if (obj.checkmileWeightage != null)
            {
                checkmileWeightages = obj.checkmileWeightage.ToString(); ;
            }
        }
        
        
        SqlParameter[] prm = new SqlParameter[24];
        prm[0] = new SqlParameter("@id", "0");
        prm[1] = new SqlParameter("@ProjectId", strpid);
        prm[2] = new SqlParameter("@Milestone", DBNull.Value);
        prm[3] = new SqlParameter("@Weightage", strweight);
        prm[4] = new SqlParameter("@StartDate", ParseDateOnly(strstartdate));
        prm[5] = new SqlParameter("@EndDate", ParseDateOnly(strenddate));
        if (strowner != "")
        {
            prm[6] = new SqlParameter("@Owner", strowner);
        }
        else
        {
            prm[6] = new SqlParameter("@Owner", DBNull.Value);
        }
        prm[7] = new SqlParameter("@Access", DBNull.Value);
        if (strteam != "")
        {
            prm[8] = new SqlParameter("@team", strteam);
        }
        else
        {
            prm[8] = new SqlParameter("@team", DBNull.Value);
        }
        if (strstatus != "")
        {
            prm[9] = new SqlParameter("@P_Status", strstatus);
        }
        else
        {
            prm[9] = new SqlParameter("@P_Status", DBNull.Value);
        }
        if (strpriority != "")
        {
            prm[10] = new SqlParameter("@Priority", strpriority);
        }
        else
        {
            prm[10] = new SqlParameter("@Priority", DBNull.Value);
        }
        prm[11] = new SqlParameter("@Budget", DBNull.Value);
        prm[12] = new SqlParameter("@Actual", DBNull.Value);

        prm[13] = new SqlParameter("@checkmileWeightage",(checkmileWeightages==string.Empty?"0": checkmileWeightages));
        if (strdurationtype != "")
        {
            prm[14] = new SqlParameter("@Durationtype", strdurationtype);
        }
        else
        {
            prm[14] = new SqlParameter("@Durationtype", DBNull.Value);
        }

        strduration = NormalizeDurationTextInvariant(strduration, obj.TaskDuration_min);
        if (strduration != "")
        {
            prm[15] = new SqlParameter("@TaskDuration", strduration);
        }
        else
        {
            prm[15] = new SqlParameter("@TaskDuration", DBNull.Value);
        }
            
        prm[16] = new SqlParameter("@istask",(istasks==""?"0": istasks));
            
        

        prm[17] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[18] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[19] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[20] = new SqlParameter("@type", "Insert");
        prm[21] = new SqlParameter("@orderno", newOrderNo);
        prm[22] = new SqlParameter("@txtdays", txtdays);
        prm[23] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[23].Direction = ParameterDirection.ReturnValue;
        SaveLogText("strstatus-" + prm[9].Value + "strpriority-" + prm[10].Value);
        int retval1 = da1.executeDMLQuery("pmstbl_Addmilestone_usp_ang", prm);

        try
        {
            // iscomplete update //

            string striscomplt = "select  isnull(iscomplete,0) as iscomplete from Pms_tbl_Projectstatus where status = 1 and archive = 0  and id='" + strstatus + "'";
            DataSet dsiscomplt = da1.GetDataSetInline(striscomplt);
            if (dsiscomplt.Tables[0].Rows.Count > 0)
            {
                int iscompletestatus = 0;
                iscompletestatus = Convert.ToInt32(dsiscomplt.Tables[0].Rows[0]["iscomplete"]);

                string strstatusupdateiscom = "update Pms_tbl_AddMilestone set iscomplete='" + iscompletestatus.ToString() + "' where id = " + retval1 + " and status=1 and archive=0";
                DataSet dshourminiscom = da1.GetDataSetInline(strstatusupdateiscom);

            }

            //   iscomplete update 



            int checkweitage = 0;
            decimal wetagecount = 0;
            decimal totalwetage = 0;
            string strweightage = "select isnull(checkprojectWeightage ,0) as checkprojectWeightage  from Pms_tbl_AddProject where id='" + strpid + "' and  status=1 and  isnull(checkprojectWeightage ,0)=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
            DataSet dsweitagecheck = da1.GetDataSetInline(strweightage);
            if (dsweitagecheck.Tables[0].Rows.Count > 0)
            {
                checkweitage = Convert.ToInt32(dsweitagecheck.Tables[0].Rows[0]["checkprojectWeightage"]);

            }
            if (checkweitage == 1)
            {
                string strweightagecount = "select count (*) as WeitageCount from Pms_tbl_AddMilestone where ProjectId='" + strpid + "' and  status=1   and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                DataSet dsweitagecount = da1.GetDataSetInline(strweightagecount);
                if (dsweitagecount.Tables[0].Rows.Count > 0)
                {

                    wetagecount = Convert.ToInt32(dsweitagecount.Tables[0].Rows[0]["WeitageCount"]);
                    totalwetage = 100 / wetagecount;
                }
                else
                {
                    totalwetage = 100;
                }

                
                string strstatusupdate = "update Pms_tbl_AddMilestone set Weightage='" + fns.ToFormattedCurrencyString(totalwetage.ToString()) + "' where ProjectId='" + strpid + "' and status=1  and archive=0";
                DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

            }


            int iscomplete = 0;
            string findsqlismile = "select isnull(iscomplete,0) as iscomplete, * from Pms_tbl_AddMilestone where id=" + retval1 + " and status=1 and archive=0";
            DataSet dsfindidismile = da1.GetDataSetInline(findsqlismile);
            if (dsfindidismile.Tables[0].Rows.Count > 0)
            {

                iscomplete = Convert.ToInt32(dsfindidismile.Tables[0].Rows[0]["iscomplete"]);

            }

            if (iscomplete == 1)
            {
                decimal milestonetotalpercent = 100;
                decimal totlweitagemile = 0;
                string finsql1 = "select * from Pms_tbl_AddMilestone where id=" + retval1 + " and status=1  and archive=0";
                DataSet dsfindid1 = da1.GetDataSetInline(finsql1);

                // Milstone percentage //
                string strstatusupdate = "update Pms_tbl_AddMilestone set iscompletepercentage='" + milestonetotalpercent.ToString() + "' where id='" + retval1 + "' and isnull(iscomplete,0)=1 and status=1 and archive=0";
                DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                // project percentage //
                string finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1  and archive=0";
                DataSet dsfindid2 = da1.GetDataSetInline(finsql2);
                for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
                {
                    decimal sumweitage = 0;
                    decimal subpercentageemile = 0;
                    sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
                    subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                    totlweitagemile += (sumweitage * subpercentageemile) / 100;

                }

                string finsql4 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitagemile.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
                DataSet dsfindid4 = da1.GetDataSetInline(finsql4);
            }
            else
            {
                string finsql2 = "";
                DataSet dsfindid2;
                decimal milestonetotalpercent = 0;
                decimal totlweitagemile = 0;
                decimal totlweitageproject = 0;
                string finsql1 = "select * from Pms_tbl_AddMilestone where id=" + retval1 + " and status=1 and archive=0";
                DataSet dsfindid1 = da1.GetDataSetInline(finsql1);
                if (dsfindid1.Tables[0].Rows.Count > 0)
                {
                    if (dsfindid1.Tables[0].Rows[0]["istask"].ToString() == "1")
                    {



                        finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where  Milestoneid='" + retval1 + "'  and status=1  and archive=0";
                        dsfindid2 = da1.GetDataSetInline(finsql2);
                        for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
                        {
                            decimal sumweitage = 0;
                            decimal subpercentageemile = 0;
                            sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
                            subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                            totlweitagemile += (sumweitage * subpercentageemile) / 100;

                        }
                    }

                }
                // Milestone percentage //
                string strstatusupdate = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile.ToString() + "' where id='" + retval1 + "' and status=1 and archive=0 and isnull(iscomplete,0)=0";
                DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                // project percentage //
                finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1  and archive=0";
                dsfindid2 = da1.GetDataSetInline(finsql2);
                for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
                {
                    decimal sumweitage = 0;
                    decimal subpercentageemile = 0;
                    sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
                    subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                    totlweitageproject += (sumweitage * subpercentageemile) / 100;

                }

                string finsql4 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
                DataSet dsfindid4 = da1.GetDataSetInline(finsql4);



            }
        }
        catch { }
        return retval1;
        // Save to DB and return new id
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static int deletemilestone(string id)
    {
        int ret = 0;
        DataAccess da1 = new DataAccess();
        try
        {
            
            string sqlq = "update pms_tbl_addmilestone set status=0 , archive=1 where regid='"+ HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + id + "'";
            SaveLogText("newMilerequest-" + sqlq);
            da1.executeDMLQueryinline(sqlq);
            string chkval= managecalculation_mile(id,"milestone");
            ret = 1;

            
           
        }
        catch { }
        return ret;
        // Save to DB and return new id
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static int updatemilestone_order(string id,string orderno)
    {
        int ret = 0;
        DataAccess da1 = new DataAccess();
        try
        {
            string sqlq = "update pms_tbl_addmilestone set orderno='"+ orderno + "' where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + id + "'";
            SaveLogText("newMilerequest-" + sqlq);
            da1.executeDMLQueryinline(sqlq);
            ret = 1;


        }
        catch { }
        return ret;
        // Save to DB and return new id
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string savedata_milestone(newmilestone model)
    {
        try
        {
            DataSet ds = new DataSet();
            DataAccess da = new DataAccess();
            CommonFunction cmf = new CommonFunction();
            function fns = new function();
            SqlParameter[] prm = new SqlParameter[68];
            //prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            //prm[1] = new SqlParameter("@cdate", da.getcdatetime());
            //prm[2] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"]);
            //prm[3] = new SqlParameter("@ptype", (object)model.ptype ?? DBNull.Value);
            //prm[4] = new SqlParameter("@pname", (object)model.pname ?? DBNull.Value);
            //prm[5] = new SqlParameter("@Unitval", (object)model.Unitval ?? DBNull.Value);
            //prm[6] = new SqlParameter("@SKUval", (object)model.SKUval ?? DBNull.Value);
            //prm[7] = new SqlParameter("@exciseproduct", (object)model.exciseproduct ?? DBNull.Value);
            //prm[8] = new SqlParameter("@issale", (object)model.issale ?? DBNull.Value);
            //prm[9] = new SqlParameter("@salesRate", (object)model.salesRate ?? DBNull.Value);
            //prm[10] = new SqlParameter("@salesaccount", (object)model.salesaccount ?? DBNull.Value);
            //prm[11] = new SqlParameter("@salesDescription", (object)model.salesDescription ?? DBNull.Value);
            //prm[12] = new SqlParameter("@salesTaxtype", (object)model.salesTaxtype ?? DBNull.Value);
            //prm[13] = new SqlParameter("@salestaxvalue", (object)model.salestaxvalue ?? DBNull.Value);

            //prm[14] = new SqlParameter("@ispurchase", (object)model.ispurchase ?? "0");
            //prm[15] = new SqlParameter("@PurchaseRate", (object)model.PurchaseRate ?? DBNull.Value);
            //prm[16] = new SqlParameter("@Purchaseaccount", (object)model.Purchaseaccount ?? DBNull.Value);
            //prm[17] = new SqlParameter("@PurchaseDescription", (object)model.PurchaseDescription ?? DBNull.Value);
            //prm[18] = new SqlParameter("@PurchaseTaxtype", (object)model.PurchaseTaxtype ?? DBNull.Value);
            //prm[19] = new SqlParameter("@Purchasetaxvalue", (object)model.Purchasetaxvalue ?? DBNull.Value);

            //prm[20] = new SqlParameter("@isTrackInventory", ToDbInt(model.isTrackInventory));          
            //prm[21] = new SqlParameter("@trackAccount", (object)model.trackAccount ?? DBNull.Value);
            //prm[22] = new SqlParameter("@OpeningStock", (object)model.OpeningStock ?? DBNull.Value);
            //prm[23] = new SqlParameter("@OpeningStockrateperunit", (object)model.OpeningStockrateperunit ?? DBNull.Value);

            //prm[24] = new SqlParameter("ReturnValue", SqlDbType.Int);
            //prm[24].Direction = ParameterDirection.ReturnValue;

            //prm[25] = new SqlParameter("@pimg", (object)model.pimg ?? DBNull.Value);
            //prm[26] = new SqlParameter("@categoryid", ToDbInt(model.categoryid));
            //prm[27] = new SqlParameter("@subcategoryid", ToDbInt(model.subcategoryid));
            //prm[28] = new SqlParameter("@rsaleprice", (object)model.rsaleprice ?? DBNull.Value);
            //prm[29] = new SqlParameter("@rrentprice", (object)model.rrentprice ?? DBNull.Value);
            //prm[30] = new SqlParameter("@rtype", (object)model.rtype ?? DBNull.Value);
            //if ((object)model.issalepurchasesame == "1")
            //{
            //    prm[31] = new SqlParameter("@Unitval2", (object)model.Unitval ?? DBNull.Value);
            //    prm[32] = new SqlParameter("@unitrelationvalue", "1");
            //    prm[33] = new SqlParameter("@unitvalue", (object)model.unitvalue ?? "0");
            //    prm[34] = new SqlParameter("@unitvalue2", (object)model.unitvalue ?? "0");
            //}
            //else
            //{
            //    prm[31] = new SqlParameter("@Unitval2", (object)model.Unitval2 ?? DBNull.Value);
            //    prm[32] = new SqlParameter("@unitrelationvalue", (object)model.unitrelationvalue ?? "1");
            //    prm[33] = new SqlParameter("@unitvalue", (object)model.unitvalue ?? "0");
            //    prm[34] = new SqlParameter("@unitvalue2", (object)model.unitvalue2 ?? "0");
            //}
            //prm[35] = new SqlParameter("@barcode", (object)model.barcode ?? DBNull.Value);
            //prm[36] = new SqlParameter("@txtreorder", (object)model.txtreorder ?? DBNull.Value);
            //prm[37] = new SqlParameter("@istaxinclusive", (object)model.istaxinclusive ?? DBNull.Value);
            //prm[38] = new SqlParameter("@Wholesale_Price", (object)model.Wholesale_Price ?? DBNull.Value);
            //prm[39] = new SqlParameter("@Wholesale_istaxinclusive", (object)model.Wholesale_istaxinclusive ?? DBNull.Value);
            //prm[40] = new SqlParameter("@Franchise_Price", (object)model.Franchise_Price ?? DBNull.Value);
            //prm[41] = new SqlParameter("@Franchise_istaxinclusive", (object)model.Franchise_istaxinclusive ?? DBNull.Value);
            //prm[42] = new SqlParameter("@Discount", (object)model.Discount ?? DBNull.Value);
            //prm[43] = new SqlParameter("@txtfocprice", (object)model.txtfocprice ?? DBNull.Value);
            //prm[44] = new SqlParameter("@txtordercode", (object)model.txtordercode ?? DBNull.Value);
            //prm[45] = new SqlParameter("@CaseBag_Size", (object)model.CaseBag_Size ?? DBNull.Value);
            //prm[46] = new SqlParameter("@txtbrand", (object)model.txtbrand ?? DBNull.Value);
            //prm[47] = new SqlParameter("@PcsCase_Bag", (object)model.PcsCase_Bag ?? DBNull.Value);
            //prm[48] = new SqlParameter("@groupId", (object)model.groupId ?? DBNull.Value);
            //prm[49] = new SqlParameter("@txtspecificgravity", (object)model.txtspecificgravity ?? DBNull.Value);
            //prm[50] = new SqlParameter("@txtsize", (object)model.txtsize ?? DBNull.Value);
            //prm[51] = new SqlParameter("@txtopensize", (object)model.txtopensize ?? DBNull.Value);
            //prm[52] = new SqlParameter("@txtmicrion", (object)model.txtmicrion ?? DBNull.Value);
            //prm[53] = new SqlParameter("@txtcutlength", (object)model.txtcutlength ?? DBNull.Value);
            //prm[54] = new SqlParameter("@subcategory2id", ToDbInt(model.subcategory2id));
            //prm[55] = new SqlParameter("@subcategory3id", ToDbInt(model.subcategory3id));
            //prm[56] = new SqlParameter("@subcategory4id", ToDbInt(model.subcategory4id));
            //prm[57] = new SqlParameter("@MaxLimit", (object)model.MaxLimit ?? DBNull.Value);
            //prm[58] = new SqlParameter("@C_Cat", (object)model.C_Cat ?? DBNull.Value);
            //prm[59] = new SqlParameter("@C_Mod", (object)model.C_Mod ?? DBNull.Value);
            //prm[60] = new SqlParameter("@C_End", (object)model.C_End ?? DBNull.Value);
            //prm[61] = new SqlParameter("@C_Material", (object)model.C_Material ?? DBNull.Value);
            //prm[62] = new SqlParameter("@C_Pick", (object)model.C_Pick ?? DBNull.Value);
            //prm[63] = new SqlParameter("@C_Meter", (object)model.C_Meter ?? DBNull.Value);
            //prm[64] = new SqlParameter("@ddlinventorytype", (object)model.ddlinventorytype ?? "1");
            //prm[65] = new SqlParameter("@type", model.QType);
            //prm[66] = new SqlParameter("@issalepurchasesame", (object)model.issalepurchasesame ?? "0");
            //prm[67] = new SqlParameter("@id", (object)model.id ?? "0");

            //int cusid = 0;
            //cusid = da.executeDMLQuery("angular_usp_tbl_itemsdetails", prm);


            return "success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    #endregion
    #region savedata task
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static int addnewtaskrequest(newTaskrequest request)
    {
        function fns = new function();
        DataAccess da1 = new DataAccess();
        var obj = request.task;
        int newOrderNo = request.orderno;
        string strpid = obj.ProjectId.ToString();       
        string strstartdate = obj.Startdate != null ? obj.Startdate.ToString() : "";
        string strenddate = obj.Targetdate != null ? obj.Targetdate.ToString() : "";
        string strowner = obj.owner != null ? obj.owner.ToString() : "";
        string strteam = obj.team != null ? obj.team.ToString() : "";
        string strstatus = obj.P_Status.ToString();
        string strpriority = obj.Priority.ToString();

        string issubtasks = "0";
        string checktaskWeightages = "0";

        try
        {
            string sqlM = "select id,isnull(hassubtask,0) as issubtask,isnull(checkTaskWeightage,0) as checkTaskWeightage from pms_tbl_addproject where status=1 and archive=0 and id=" + obj.ProjectId + " and regid='"+ HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
            DataSet dsM= da1.GetDataSetInline(sqlM);
            if(dsM.Tables[0].Rows.Count>0)
            {
                issubtasks = dsM.Tables[0].Rows[0]["issubtask"].ToString();
                checktaskWeightages = dsM.Tables[0].Rows[0]["checkTaskWeightage"].ToString();
            }
        }
        catch { }
        if (issubtasks == "0")
        {
            if (obj.issubtask != null)
            {
                issubtasks = obj.issubtask.ToString(); ;
            }
        }
        if (checktaskWeightages == "0")
        {
            if (obj.checkTaskWeightage != null)
            {
                checktaskWeightages = obj.checkTaskWeightage.ToString(); ;
            }
        }
        

        SqlParameter[] prm = new SqlParameter[35];
        prm[0] = new SqlParameter("@id", "0");
        prm[1] = new SqlParameter("@ProjectId",obj.ProjectId);
        prm[2] = new SqlParameter("@Milestoneid", obj.Milestoneid);
        prm[3] = new SqlParameter("@tasklist", DBNull.Value);
        prm[4] = new SqlParameter("@taskname", DBNull.Value);
        prm[5] = new SqlParameter("@taskdescripton", DBNull.Value);
        prm[6] = new SqlParameter("@owner", obj.owner);
        prm[7] = new SqlParameter("@Startdate", ParseDateOnly(strstartdate));
        prm[8] = new SqlParameter("@Targetdate", ParseDateOnly(strenddate));
        prm[9] = new SqlParameter("@StartTime", DBNull.Value);
        prm[10] = new SqlParameter("@Timeline", DBNull.Value);
        prm[11] = new SqlParameter("@TaskDuration", NormalizeDurationTextInvariant(obj.TaskDuration, obj.TaskDuration_min));
        prm[12] = new SqlParameter("@P_Status", obj.P_Status);
        prm[13] = new SqlParameter("@Priority", obj.Priority);
        prm[14] = new SqlParameter("@Budget", DBNull.Value);
        prm[15] = new SqlParameter("@Actual", DBNull.Value);
        prm[16] = new SqlParameter("@Reminder", DBNull.Value);
        prm[17] = new SqlParameter("@ReminderType", DBNull.Value);
        prm[18] = new SqlParameter("@Days", DBNull.Value);
        prm[19] = new SqlParameter("@Time", DBNull.Value);
        prm[20] = new SqlParameter("@Notifyusers", DBNull.Value);
        prm[21] = new SqlParameter("@ChooseDate", DBNull.Value);
        prm[22] = new SqlParameter("@Occurrence", DBNull.Value);
        prm[23] = new SqlParameter("@tagto", DBNull.Value);
        prm[24] = new SqlParameter("@team", obj.team);
        prm[25] = new SqlParameter("@Stage", DBNull.Value);
        prm[26] = new SqlParameter("@Weightage", obj.Weightage);
        prm[27] = new SqlParameter("@issubtask", issubtasks);
        prm[28] = new SqlParameter("@checkTaskWeightage", checktaskWeightages);
        prm[29] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[30] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[31] = new SqlParameter("@cdate", ParseDateOnly(da1.getcdatetime()));
        prm[32] = new SqlParameter("@type", "Insert");
        prm[33] = new SqlParameter("@Uploaddrawing", DBNull.Value);        
        prm[34] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[34].Direction = ParameterDirection.ReturnValue;
        int retval1 = da1.executeDMLQuery("pms_Adtask_usp_ang", prm);

        try
        {
            // iscomplete update //

            string striscomplt = "select  isnull(iscomplete,0) as iscomplete from Pms_tbl_Projectstatus where status = 1 and archive = 0  and id='" + obj.P_Status + "'";
            DataSet dsiscomplt = da1.GetDataSetInline(striscomplt);
            if (dsiscomplt.Tables[0].Rows.Count > 0)
            {
                int iscompletestatus = 0;
                iscompletestatus = Convert.ToInt32(dsiscomplt.Tables[0].Rows[0]["iscomplete"]);

                string strstatusupdateiscom = "update Pms_tbl_Addtask set iscomplete='" + iscompletestatus.ToString() + "' where id = " + retval1 + " and status=1 and archive=0";
                DataSet dshourminiscom = da1.GetDataSetInline(strstatusupdateiscom);

            }

            //   iscomplete update  



            int checkweitage = 0;
            decimal wetagecount = 0;
            decimal totalwetage = 0;
            string strweightage = "select isnull(checkmileWeightage ,0) as checkmileWeightage  from Pms_tbl_AddMilestone where id='" + obj.Milestoneid + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
            SaveLogText("strweightagecount1=" + strweightage);
            DataSet dsweitagecheck = da1.GetDataSetInline(strweightage);
            if (dsweitagecheck.Tables[0].Rows.Count > 0)
            {

                checkweitage = Convert.ToInt32(dsweitagecheck.Tables[0].Rows[0]["checkmileWeightage"]);

            }

            if (checkweitage == 1)
            {
                string strweightagecount = "select count (*) as WeitageCount from Pms_tbl_Addtask where Milestoneid='" + obj.Milestoneid + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                SaveLogText("strweightagecount2="+ strweightagecount);
                DataSet dsweitagecount = da1.GetDataSetInline(strweightagecount);
                if (dsweitagecount.Tables[0].Rows.Count > 0)
                {

                    wetagecount = Convert.ToInt32(dsweitagecount.Tables[0].Rows[0]["WeitageCount"]);
                    totalwetage = 100 / wetagecount;
                }
                else
                {
                    totalwetage = 100;
                }

                
                string strstatusupdate = "update Pms_tbl_Addtask set Weightage='" + fns.ToFormattedCurrencyString(totalwetage.ToString()) + "' where Milestoneid='" + obj.Milestoneid + "' and status=1 and archive=0";
                DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

            }

            int iscomplete = 0;
            string finsql1 = "select isnull(iscomplete,0) as iscomplete, * from Pms_tbl_Addtask where id=" + retval1 + " and status=1 and archive=0";
            DataSet dsfindid1 = da1.GetDataSetInline(finsql1);
            if (dsfindid1.Tables[0].Rows.Count > 0)
            {

                iscomplete = Convert.ToInt32(dsfindid1.Tables[0].Rows[0]["iscomplete"]);

            }

            if (iscomplete == 1)
            {
                decimal tsktotalpercent = 100;
                decimal totlweitagemile = 0;
                decimal totlweitageproject = 0;

                string finsql01 = "select * from Pms_tbl_Addtask where id=" + retval1 + " and status=1  and archive=0";
                DataSet dsfindid01 = da1.GetDataSetInline(finsql01);

                // task percentage //
                string strstatusupdate = "update Pms_tbl_Addtask set iscompletepercentage='" + tsktotalpercent.ToString() + "' where id='" + retval1 + "' and status=1 and archive=0 and isnull(iscomplete,0)=1";
                DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                // Milestone percentage //
                string finsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "'   and status=1  and archive=0";
                DataSet dsfindmile = da1.GetDataSetInline(finsqlmile);
                for (int j = 0; j < dsfindmile.Tables[0].Rows.Count; j++)
                {
                    decimal sumweitage = 0;
                    decimal subpercentageemile = 0;
                    sumweitage = Convert.ToDecimal(dsfindmile.Tables[0].Rows[j]["Weightage"].ToString());
                    subpercentageemile = Convert.ToDecimal(dsfindmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                    totlweitagemile += (sumweitage * subpercentageemile) / 100;

                }

                string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile.ToString() + "' where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and id='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1 and archive=0";
                DataSet dsfindid4 = da1.GetDataSetInline(finsql4);


                // project percentage //
                string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "'   and status=1  and archive=0";
                DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
                for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
                {
                    decimal sumweitageprojt = 0;
                    decimal subpercentageeproject = 0;
                    sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                    subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                    totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

                }

                string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
                DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);




            }
            else
            {
                decimal tsktotalpercent = 0;
                decimal totlweitagemile = 0;
                decimal totlweitageproject = 0;

                string finsql01 = "select * from Pms_tbl_Addtask where id=" + retval1 + " and status=1  and archive=0";
                DataSet dsfindid01 = da1.GetDataSetInline(finsql01);


                if (dsfindid01.Tables[0].Rows.Count > 0)
                {
                    if (dsfindid01.Tables[0].Rows[0]["issubtask"].ToString() == "1")
                    {

                        string finsql211 = "select isnull(sum(weightage),0)Wattagetasksum from Pms_tbl_Addsubtask where   task='" + retval1 + "'  and status=1 and isnull(iscomplete,0)=1 and archive=0";
                        DataSet dsfindid211 = da1.GetDataSetInline(finsql211);
                        if (dsfindid211.Tables[0].Rows.Count > 0)
                        {
                            string strstatusupdate = "update Pms_tbl_Addtask set iscompletepercentage='" + dsfindid211.Tables[0].Rows[0]["Wattagetasksum"] + "' where id='" + retval1 + "' and status=1 and archive=0";
                            DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);
                        }
                    }
                    else
                    {
                        string strstatusupdate = "update Pms_tbl_Addtask set iscompletepercentage='0' where id='" + retval1 + "' and status=1 and archive=0";
                        DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                    }

                }

                // Milestone percentage //
                string finsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "'   and status=1  and archive=0";
                DataSet dsfindmile = da1.GetDataSetInline(finsqlmile);
                for (int j = 0; j < dsfindmile.Tables[0].Rows.Count; j++)
                {
                    decimal sumweitage = 0;
                    decimal subpercentageemile = 0;
                    sumweitage = Convert.ToDecimal(dsfindmile.Tables[0].Rows[j]["Weightage"].ToString());
                    subpercentageemile = Convert.ToDecimal(dsfindmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                    totlweitagemile += (sumweitage * subpercentageemile) / 100;

                }

                string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile.ToString() + "' where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and id='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1 and archive=0";
                DataSet dsfindid4 = da1.GetDataSetInline(finsql4);

                // project percentage //
                string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "'   and status=1  and archive=0";
                DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
                for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
                {
                    decimal sumweitageprojt = 0;
                    decimal subpercentageeproject = 0;
                    sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                    subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                    totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

                }

                string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
                DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);



            }
        }
        catch { }
        return retval1;
        // Save to DB and return new id
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static int deletetask(string id)
    {
        int ret = 0;
        string retval = "0";
        DataAccess da1 = new DataAccess();
        try
        {            
            string sqlq = "update pms_tbl_addtask set status=0 , archive=1 where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + id + "'";
            da1.executeDMLQueryinline(sqlq);
            string chkval = managecalculation_task(id, "task");
            ret = 1;


        }
        catch { }
        return ret;
        // Save to DB and return new id
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static int updatetask_order(string id, string orderno)
    {
        int ret = 0;
        DataAccess da1 = new DataAccess();
        try
        {
            string sqlq = "update pms_tbl_addtask set orderno='" + orderno + "' where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + id + "'";
           da1.executeDMLQueryinline(sqlq);
            ret = 1;


        }
        catch { }
        return ret;
        // Save to DB and return new id
    }
    #endregion
    #region savedata sub task
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static int addnewsubtaskrequest(newSubTaskrequest request)
    {
        function fns = new function();
        DataAccess da1 = new DataAccess();
        var obj = request.subtask;
        int newOrderNo = request.orderno;
        string strpid = obj.ProjectId.ToString();
        
        string strstartdate = obj.Startdate != null ? obj.Startdate.ToString() : "";
        string strenddate = obj.Targetdate != null ? obj.Targetdate.ToString() : "";
        string strowner = obj.owner != null ? obj.owner.ToString() : "";
        string strteam = obj.team != null ? obj.team.ToString() : "";
        string strtimeline = "";
        string strstatus = obj.P_Status.ToString();
        string strpriority = obj.Priority.ToString();
        SqlParameter[] prm = new SqlParameter[34];
        prm[0] = new SqlParameter("@id", "0");
        prm[1] = new SqlParameter("@ProjectId", obj.ProjectId);
        prm[2] = new SqlParameter("@Milestoneid", obj.Milestoneid);
        prm[3] = new SqlParameter("@tasklist", DBNull.Value);
        prm[4] = new SqlParameter("@task", obj.Task);
        prm[5] = new SqlParameter("@Subtaskname", DBNull.Value);
        prm[6] = new SqlParameter("@SubTaskDescription", DBNull.Value);
        prm[7] = new SqlParameter("@owner", obj.owner);
        prm[8] = new SqlParameter("@Startdate", ParseDateOnly(strstartdate));
        prm[9] = new SqlParameter("@Targetdate", ParseDateOnly(strenddate));
        prm[10] = new SqlParameter("@StartTime", DBNull.Value);
        prm[11] = new SqlParameter("@Timeline", DBNull.Value);
        prm[12] = new SqlParameter("@TaskDuration", NormalizeDurationTextInvariant(obj.TaskDuration, obj.TaskDuration_min));
        prm[13] = new SqlParameter("@P_Status", obj.P_Status);
        prm[14] = new SqlParameter("@Priority", obj.Priority);
        prm[15] = new SqlParameter("@Budget", DBNull.Value);
        prm[16] = new SqlParameter("@Actual", DBNull.Value);
        prm[17] = new SqlParameter("@Reminder", DBNull.Value);
        prm[18] = new SqlParameter("@ReminderType", DBNull.Value);
        prm[19] = new SqlParameter("@Days", DBNull.Value);
        prm[20] = new SqlParameter("@Time", DBNull.Value);
        prm[21] = new SqlParameter("@Notifyusers", DBNull.Value);
        prm[22] = new SqlParameter("@ChooseDate", DBNull.Value);
        prm[23] = new SqlParameter("@Occurrence", DBNull.Value);
        prm[24] = new SqlParameter("@tagto", DBNull.Value);
        prm[25] = new SqlParameter("@team", obj.team);
        prm[26] = new SqlParameter("@Stage", DBNull.Value);
        prm[27] = new SqlParameter("@Weightage", obj.Weightage);
        prm[28] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[29] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[30] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[31] = new SqlParameter("@type", "Insert");
        prm[32] = new SqlParameter("@Uploaddrawing", DBNull.Value);
        prm[33] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[33].Direction = ParameterDirection.ReturnValue;
        int retval1 = da1.executeDMLQuery("pms_Add_Subtask_usp_ang", prm);
        try
        {
            // iscomplete update //

            string striscomplt = "select  isnull(iscomplete,0) as iscomplete from Pms_tbl_Projectstatus where status = 1 and archive = 0  and id='" + obj.P_Status + "'";
            DataSet dsiscomplt = da1.GetDataSetInline(striscomplt);
            if (dsiscomplt.Tables[0].Rows.Count > 0)
            {
                int iscompletestatus = 0;
                iscompletestatus = Convert.ToInt32(dsiscomplt.Tables[0].Rows[0]["iscomplete"]);

                string strstatusupdateiscom = "update Pms_tbl_Addsubtask set iscomplete='" + iscompletestatus.ToString() + "' where id = " + retval1 + " and status=1 and archive=0";
                DataSet dshourminiscom = da1.GetDataSetInline(strstatusupdateiscom);

            }

            //   iscomplete update  



            int checkweitage = 0;
            decimal wetagecount = 0;
            decimal totalwetage = 0;
            string strweightage = "select isnull(checkTaskWeightage ,0) as checkTaskWeightage  from pms_tbl_addtask where id='" + obj.Task + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
            SaveLogText("strweightageA1="+ strweightage);
            DataSet dsweitagecheck = da1.GetDataSetInline(strweightage);
            if (dsweitagecheck.Tables[0].Rows.Count > 0)
            {

                checkweitage = Convert.ToInt32(dsweitagecheck.Tables[0].Rows[0]["checkTaskWeightage"]);

            }
            SaveLogText("strweightageA2=" + checkweitage);
            if (checkweitage == 1)
            {
                string strweightagecount = "select count (*) as WeitageCount from Pms_tbl_Addsubtask where task='" + obj.Task + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                DataSet dsweitagecount = da1.GetDataSetInline(strweightagecount);
                if (dsweitagecount.Tables[0].Rows.Count > 0)
                {

                    wetagecount = Convert.ToInt32(dsweitagecount.Tables[0].Rows[0]["WeitageCount"]);
                    totalwetage = 100 / wetagecount;
                }
                else
                {
                    totalwetage = 100;
                }

                
                string strstatusupdate = "update Pms_tbl_Addsubtask set Weightage='" + fns.ToFormattedCurrencyString(totalwetage.ToString()) + "' where task='" + obj.Task + "' and status=1 and archive=0";
                DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

            }


            int iscomplete = 0;
            string finsql1 = "select  isnull(iscomplete,0) as iscomplete,* from Pms_tbl_Addsubtask where id=" + retval1 + " and status=1  and archive=0";
            DataSet dsfindid1 = da1.GetDataSetInline(finsql1);
            if (dsfindid1.Tables[0].Rows.Count > 0)
            {

                iscomplete = Convert.ToInt32(dsfindid1.Tables[0].Rows[0]["iscomplete"]);

            }

            if (iscomplete == 1)
            {
                decimal subtsktotalpercent = 100;
                decimal totlweitagemile = 0;
                decimal totlweitageproject = 0;

                string finsql01 = "select * from Pms_tbl_Addsubtask where id=" + retval1 + " and status=1  and archive=0";
                DataSet dsfindid01 = da1.GetDataSetInline(finsql01);

                string finsql2 = "select isnull(sum(weightage),0)Wattagetasksum from Pms_tbl_Addsubtask where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "' and task='" + dsfindid01.Tables[0].Rows[0]["task"] + "'  and status=1 and isnull(iscomplete,0)=1 and archive=0";
                DataSet dsfindid2 = da1.GetDataSetInline(finsql2);

                //sub task percentage //
                string strstatusupdate = "update Pms_tbl_Addsubtask set iscompletepercentage='" + subtsktotalpercent.ToString() + "' where id='" + retval1 + "' and status=1 and archive=0 and isnull(iscomplete,0)=1";
                DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                // task percentage //
                string finsql3 = "update Pms_tbl_Addtask set iscompletepercentage='" + dsfindid2.Tables[0].Rows[0]["Wattagetasksum"] + "' where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "' and id='" + dsfindid01.Tables[0].Rows[0]["task"] + "' and status=1 and archive=0";
                DataSet dsfindid3 = da1.GetDataSetInline(finsql3);


                // Milestone percentage //
                string findsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask  where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1  and archive=0";
                DataSet dsfindidmile = da1.GetDataSetInline(findsqlmile);
                for (int j = 0; j < dsfindidmile.Tables[0].Rows.Count; j++)
                {
                    decimal sumweitage = 0;
                    decimal subpercentageemile = 0;
                    sumweitage = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["Weightage"].ToString());
                    subpercentageemile = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                    totlweitagemile += (sumweitage * subpercentageemile) / 100;

                }

                string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile + "' where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and id='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1 and archive=0";
                DataSet dsfindid4 = da1.GetDataSetInline(finsql4);



                // project percentage //
                string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "'    and status=1  and archive=0";
                DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
                for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
                {
                    decimal sumweitageprojt = 0;
                    decimal subpercentageeproject = 0;
                    sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                    subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                    totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

                }

                string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
                DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);






            }

            else
            {
                decimal subtsktotalpercent = 0;
                decimal totlweitagemile = 0;
                decimal totlweitageproject = 0;

                string finsql01 = "select * from Pms_tbl_Addsubtask where id=" + retval1 + " and status=1 and archive=0";
                DataSet dsfindid01 = da1.GetDataSetInline(finsql1);

                string finsql2 = "select isnull(sum(weightage),0)Wattagetasksum from Pms_tbl_Addsubtask where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "' and task='" + dsfindid01.Tables[0].Rows[0]["task"] + "'  and status=1 and isnull(iscomplete,0)=1 and archive=0";
                DataSet dsfindid2 = da1.GetDataSetInline(finsql2);

                //Sub task percentage //
                string strstatusupdate = "update Pms_tbl_Addsubtask set iscompletepercentage='" + subtsktotalpercent.ToString() + "' where id='" + retval1 + "' and status=1 and archive=0 and isnull(iscomplete,0)=0";
                DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                // task percentage //
                string finsql3 = "update Pms_tbl_Addtask set iscompletepercentage='" + dsfindid2.Tables[0].Rows[0]["Wattagetasksum"] + "' where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "' and id='" + dsfindid01.Tables[0].Rows[0]["task"] + "' and status=1 and archive=0";
                DataSet dsfindid3 = da1.GetDataSetInline(finsql3);


                // Milestone percentage //
                string findsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask  where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "'    and status=1  and archive=0";
                DataSet dsfindidmile = da1.GetDataSetInline(findsqlmile);
                for (int j = 0; j < dsfindidmile.Tables[0].Rows.Count; j++)
                {
                    decimal sumweitage = 0;
                    decimal subpercentageemile = 0;
                    sumweitage = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["Weightage"].ToString());
                    subpercentageemile = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                    totlweitagemile += (sumweitage * subpercentageemile) / 100;

                }

                string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile + "' where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "' and id='" + dsfindid01.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1 and archive=0";
                DataSet dsfindid4 = da1.GetDataSetInline(finsql4);


                // project percentage //
                string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "'   and status=1  and archive=0";
                DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
                for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
                {
                    decimal sumweitageprojt = 0;
                    decimal subpercentageeproject = 0;
                    sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                    subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                    totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

                }

                string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + dsfindid01.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
                DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);


            }
        }
        catch { }
        return retval1;
        // Save to DB and return new id
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static int deletesubtask(string id)
    {
        int ret = 0;
        string retval = "0";
        DataAccess da1 = new DataAccess();
        try
        {
           
            string sqlq = "update pms_tbl_addsubtask set status=0 , archive=1 where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + id + "'";
            da1.executeDMLQueryinline(sqlq);
            string chkval = managecalculation_subtask(id, "subtask");
            ret = 1;


        }
        catch { }
        return ret;
        // Save to DB and return new id
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static int updatesubtask_order(string id, string orderno)
    {
        int ret = 0;
        DataAccess da1 = new DataAccess();
        try
        {
            string sqlq = "update pms_tbl_addsubtask set orderno='" + orderno + "' where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + id + "'";
            da1.executeDMLQueryinline(sqlq);
            ret = 1;


        }
        catch { }
        return ret;
        // Save to DB and return new id
    }
    #endregion
    #region get new weightage
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string get_newweightage(string id, string gtype)
    {
        function fns = new function();
        DataAccess da1 = new DataAccess();        
        string retval = "0";
        string rid = id;
        string pid = "0";
        string mid = "0";
        string tid = "0";
        string sid = "0";

        string sqlP = "select id ,isnull(Weightage ,0) as Weightage from pms_tbl_addmilestone where status=1 and archive=0 and id=" + rid + "";
        if (gtype == "task")
        {
            sqlP = "select id,isnull(Weightage ,0) as Weightage  from pms_tbl_addtask where status=1 and archive=0 and id=" + rid + "";
        }
        else if (gtype == "subtask")
        {
            sqlP = "select id ,isnull(Weightage ,0) as Weightage from pms_tbl_addsubtask where status=1 and archive=0 and id=" + rid + "";
        }

        try
        {
            DataSet dsD = da1.GetDataSetInline(sqlP);
            if (dsD.Tables[0].Rows.Count > 0)
            {
                retval = dsD.Tables[0].Rows[0]["Weightage"].ToString();
            }
        }
        catch
        {
        }
       
        return retval;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string get_newweightage2(string id, string gtype)
    {
        function fns = new function();
        DataAccess da1 = new DataAccess();
        string retval = "0";
        string rid = id;
        string pid = "0";
        string mid = "0";
        string tid = "0";
        string sid = "0";
        string sqlP = "";
        if (gtype == "milestone")
        {
            sqlP = "select top(1) id ,isnull(Weightage ,0) as Weightage from pms_tbl_addmilestone where status=1 and archive=0 and projectid=" + rid + " and isnull(Weightage ,0)>0 ";
        }
        else if (gtype == "task")
        {
            sqlP = "select id,isnull(Weightage ,0) as Weightage  from pms_tbl_addtask where status=1 and archive=0 and milestoneid=" + rid + "  and isnull(Weightage ,0)>0 ";
        }
        else if (gtype == "subtask")
        {
            sqlP = "select id ,isnull(Weightage ,0) as Weightage from pms_tbl_addsubtask where status=1 and archive=0 and Task=" + rid + "  and isnull(Weightage ,0)>0 ";
        }

        try
        {
            DataSet dsD = da1.GetDataSetInline(sqlP);
            if (dsD.Tables[0].Rows.Count > 0)
            {
                retval = dsD.Tables[0].Rows[0]["Weightage"].ToString();
            }
            else
            {
                retval = "100";
            }
        }
        catch
        {
        }

        return retval;
    }
    #endregion
    #region update details

    private static bool HasPmsEditPermission(string moduleid)
    {
        try
        {
            string uid = "0";
            if (HttpContext.Current.Request.Cookies["user"]["user_id"] != null)
            {
                uid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();
            }
            string[] permission = get_EditDelete_Validation(moduleid,uid).Split(',');
            // Permission order: View, AllAccess, Create, Edit, Delete, Assign.
            return permission.Length > 3 && (permission[1] == "1" || permission[3] == "1");
        }
        catch
        {
            return false;
        }
    }


    #region update status
    [WebMethod]
    public static string UpdateMilestone_Status(string milstoneid = "", string statusid = "", string iscomplete = "")
    {
        if (!HasPmsEditPermission("54")) return "You do not have edit permission.";
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (milstoneid != "" && milstoneid != "0")
        {
            entrytype = "Update";
        }

        // percentage update start  // 
        if (iscomplete == "1")
        {
            string independency = checkGanttPredecessorCompletion("milestone", milstoneid);
            if (independency != "0")
            {
                retval = independency == "1" ? "Please complete first dependency of this? " : independency;
            }
            else
            {
                SqlParameter[] prm = new SqlParameter[8];
                prm[0] = new SqlParameter("@id", milstoneid);
                prm[1] = new SqlParameter("@milstoneStatus", statusid);
                prm[2] = new SqlParameter("@iscomplete", iscomplete);
                prm[3] = new SqlParameter("@iscompletedate", da1.getcdatetime());
                prm[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prm[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                prm[6] = new SqlParameter("@cdate", da1.getcdatetime());
                prm[7] = new SqlParameter("@type", entrytype);
                da1.executeDMLQuery("pmstbl_Status_milstone_Update_usp", prm);
                decimal milestonetotalpercent = 100;
                decimal totlweitagemile = 0;
                string finsql1 = "select * from Pms_tbl_AddMilestone where id=" + milstoneid + " and status=1  and archive=0";
                DataSet dsfindid1 = da1.GetDataSetInline(finsql1);

                // Milestone percentage //
                string strstatusupdate = "update Pms_tbl_AddMilestone set iscompletepercentage='" + milestonetotalpercent.ToString() + "' where id='" + milstoneid + "' and status=1 and archive=0 and isnull(iscomplete,0)=1";
                DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                // project percentage //
                string finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1  and archive=0";
                DataSet dsfindid2 = da1.GetDataSetInline(finsql2);
                for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
                {
                    decimal sumweitage = 0;
                    decimal subpercentageemile = 0;
                    sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
                    subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                    totlweitagemile += (sumweitage * subpercentageemile) / 100;

                }

                string finsql4 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitagemile.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
                DataSet dsfindid4 = da1.GetDataSetInline(finsql4);

                //project  log details insert start
                SqlParameter[] prmlog = new SqlParameter[8];
                prmlog[0] = new SqlParameter("@Transid", milstoneid);
                prmlog[1] = new SqlParameter("@Logstatus", statusid);
                prmlog[2] = new SqlParameter("@LogDate", da1.getcdatetime());
                prmlog[3] = new SqlParameter("@Logtype", "milestone");
                prmlog[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prmlog[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                prmlog[6] = new SqlParameter("@cdate", da1.getcdatetime());
                prmlog[7] = new SqlParameter("@type", "Insert");
                da1.executeDMLQuery("pmstbl_Loginsert_usp", prmlog);
                //project  log details insert end
                retval = "1";
                //DataAccess da = new DataAccess();
                //string strsql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus where  status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + statusid + "' ";
                //DataSet dscode = da.GetDataSetInline(strsql);
                //if (dscode.Tables[0].Rows.Count > 0)
                //{
                //    retval = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
                //}
            }


            //string chkval = managecalculation(milstoneid, "milestone");

        }
        else
        {
            SqlParameter[] prm = new SqlParameter[8];
            prm[0] = new SqlParameter("@id", milstoneid);
            prm[1] = new SqlParameter("@milstoneStatus", statusid);
            prm[2] = new SqlParameter("@iscomplete", iscomplete);
            prm[3] = new SqlParameter("@iscompletedate", da1.getcdatetime());
            prm[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prm[6] = new SqlParameter("@cdate", da1.getcdatetime());
            prm[7] = new SqlParameter("@type", entrytype);
            da1.executeDMLQuery("pmstbl_Status_milstone_Update_usp", prm);
            decimal milestonetotalpercent = 0;
            decimal totlweitagemile = 0;
            string finsql1 = "select * from Pms_tbl_AddMilestone where id=" + milstoneid + " and status=1  and archive=0";
            DataSet dsfindid1 = da1.GetDataSetInline(finsql1);

            // Milestone percentage //
            string strstatusupdate = "update Pms_tbl_AddMilestone set iscompletepercentage='" + milestonetotalpercent.ToString() + "' where id='" + milstoneid + "' and status=1 and archive=0 and isnull(iscomplete,0)=0";
            DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

            if (dsfindid1.Tables[0].Rows[0]["istask"].ToString() == "1")
            {

                string sqlCW = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where  Milestoneid='" + milstoneid + "'  and status=1  and archive=0 ";
                DataSet dsCW = da1.GetDataSetInline(sqlCW);
                for (int k = 0; k < dsCW.Tables[0].Rows.Count; k++)
                {
                    decimal sumweitage = 0;
                    decimal subpercentageemile = 0;
                    sumweitage = Convert.ToDecimal(dsCW.Tables[0].Rows[k]["Weightage"].ToString());
                    subpercentageemile = Convert.ToDecimal(dsCW.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                    totlweitagemile += (sumweitage * subpercentageemile) / 100;

                }
                string strupdate = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile.ToString() + "' where id='" + milstoneid + "' and status=1 and archive=0 ";
                SaveLogText("updatestatusA="+ strupdate);
                da1.executeDMLQueryinline(strupdate);

            }
            // project percentage //
            string finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1  and archive=0";
            DataSet dsfindid2 = da1.GetDataSetInline(finsql2);
            for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
            {
                decimal sumweitage = 0;
                decimal subpercentageemile = 0;
                sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
                subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                totlweitagemile += (sumweitage * subpercentageemile) / 100;

            }

            string finsql4 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitagemile.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da1.GetDataSetInline(finsql4);
            //project  log details insert start
            SqlParameter[] prmlog = new SqlParameter[8];
            prmlog[0] = new SqlParameter("@Transid", milstoneid);
            prmlog[1] = new SqlParameter("@Logstatus", statusid);
            prmlog[2] = new SqlParameter("@LogDate", da1.getcdatetime());
            prmlog[3] = new SqlParameter("@Logtype", "milestone");
            prmlog[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prmlog[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prmlog[6] = new SqlParameter("@cdate", da1.getcdatetime());
            prmlog[7] = new SqlParameter("@type", "Insert");
            da1.executeDMLQuery("pmstbl_Loginsert_usp", prmlog);
            //project  log details insert end
            retval = "1";
            


        }
        // percentage update end  // 





        // return dscode.GetXml();
        return retval;
    }
    [WebMethod]
    public static item_summaryN UpdateMilestone_StatusN(string milstoneid = "", string statusid = "", string iscomplete = "")
    {
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (milstoneid != "" && milstoneid != "0")
        {
            entrytype = "Update";
        }

        // percentage update start  // 
        if (iscomplete == "1")
        {
            string independency = checkGanttPredecessorCompletion("milestone", milstoneid);
            if (independency != "0")
            {
                retval = independency == "1" ? "Please complete first dependency of this? " : independency;
            }
            else
            {
                SqlParameter[] prm = new SqlParameter[8];
                prm[0] = new SqlParameter("@id", milstoneid);
                prm[1] = new SqlParameter("@milstoneStatus", statusid);
                prm[2] = new SqlParameter("@iscomplete", iscomplete);
                prm[3] = new SqlParameter("@iscompletedate", da1.getcdatetime());
                prm[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prm[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                prm[6] = new SqlParameter("@cdate", da1.getcdatetime());
                prm[7] = new SqlParameter("@type", entrytype);
                da1.executeDMLQuery("pmstbl_Status_milstone_Update_usp", prm);
                decimal milestonetotalpercent = 100;
                decimal totlweitagemile = 0;
                string finsql1 = "select * from Pms_tbl_AddMilestone where id=" + milstoneid + " and status=1  and archive=0";
                DataSet dsfindid1 = da1.GetDataSetInline(finsql1);

                // Milestone percentage //
                string strstatusupdate = "update Pms_tbl_AddMilestone set iscompletepercentage='" + milestonetotalpercent.ToString() + "' where id='" + milstoneid + "' and status=1 and archive=0 and isnull(iscomplete,0)=1";
                DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                // project percentage //
                string finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1  and archive=0";
                DataSet dsfindid2 = da1.GetDataSetInline(finsql2);
                for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
                {
                    decimal sumweitage = 0;
                    decimal subpercentageemile = 0;
                    sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
                    subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                    totlweitagemile += (sumweitage * subpercentageemile) / 100;

                }

                string finsql4 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitagemile.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
                DataSet dsfindid4 = da1.GetDataSetInline(finsql4);

                //project  log details insert start
                SqlParameter[] prmlog = new SqlParameter[8];
                prmlog[0] = new SqlParameter("@Transid", milstoneid);
                prmlog[1] = new SqlParameter("@Logstatus", statusid);
                prmlog[2] = new SqlParameter("@LogDate", da1.getcdatetime());
                prmlog[3] = new SqlParameter("@Logtype", "milestone");
                prmlog[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prmlog[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                prmlog[6] = new SqlParameter("@cdate", da1.getcdatetime());
                prmlog[7] = new SqlParameter("@type", "Insert");
                da1.executeDMLQuery("pmstbl_Loginsert_usp", prmlog);
                //project  log details insert end
                retval = "1";
                //DataAccess da = new DataAccess();
                //string strsql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus where  status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + statusid + "' ";
                //DataSet dscode = da.GetDataSetInline(strsql);
                //if (dscode.Tables[0].Rows.Count > 0)
                //{
                //    retval = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
                //}
            }


            //string chkval = managecalculation(milstoneid, "milestone");

        }
        else
        {
            SqlParameter[] prm = new SqlParameter[8];
            prm[0] = new SqlParameter("@id", milstoneid);
            prm[1] = new SqlParameter("@milstoneStatus", statusid);
            prm[2] = new SqlParameter("@iscomplete", iscomplete);
            prm[3] = new SqlParameter("@iscompletedate", da1.getcdatetime());
            prm[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prm[6] = new SqlParameter("@cdate", da1.getcdatetime());
            prm[7] = new SqlParameter("@type", entrytype);
            da1.executeDMLQuery("pmstbl_Status_milstone_Update_usp", prm);
            decimal milestonetotalpercent = 0;
            decimal totlweitagemile = 0;
            string finsql1 = "select * from Pms_tbl_AddMilestone where id=" + milstoneid + " and status=1  and archive=0";
            DataSet dsfindid1 = da1.GetDataSetInline(finsql1);

            // Milestone percentage //
            string strstatusupdate = "update Pms_tbl_AddMilestone set iscompletepercentage='" + milestonetotalpercent.ToString() + "' where id='" + milstoneid + "' and status=1 and archive=0 and isnull(iscomplete,0)=0";
            DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

            // project percentage //
            string finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1  and archive=0";
            DataSet dsfindid2 = da1.GetDataSetInline(finsql2);
            for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
            {
                decimal sumweitage = 0;
                decimal subpercentageemile = 0;
                sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
                subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                totlweitagemile += (sumweitage * subpercentageemile) / 100;

            }

            string finsql4 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitagemile.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da1.GetDataSetInline(finsql4);
            //project  log details insert start
            SqlParameter[] prmlog = new SqlParameter[8];
            prmlog[0] = new SqlParameter("@Transid", milstoneid);
            prmlog[1] = new SqlParameter("@Logstatus", statusid);
            prmlog[2] = new SqlParameter("@LogDate", da1.getcdatetime());
            prmlog[3] = new SqlParameter("@Logtype", "milestone");
            prmlog[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prmlog[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prmlog[6] = new SqlParameter("@cdate", da1.getcdatetime());
            prmlog[7] = new SqlParameter("@type", "Insert");
            da1.executeDMLQuery("pmstbl_Loginsert_usp", prmlog);
            //project  log details insert end
            retval = "1";
            //DataAccess da = new DataAccess();
            //string strsql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus where  status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + statusid + "' ";
            //DataSet dscode = da.GetDataSetInline(strsql);
            //if (dscode.Tables[0].Rows.Count > 0)
            //{
            //    retval = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
            //}


        }
        // percentage update end  // 

        item_summaryN _summary = new item_summaryN();
        _summary.status = retval;
        double mileTaskTotal = 0;
        double mileWattagepending = 100;
        try
        {

            mileTaskTotal = get_completionstatus(milstoneid, "milestone");
            if (mileTaskTotal > 0)
            {
                mileWattagepending = mileWattagepending - mileTaskTotal;
            }
        }
        catch { }
        _summary.status = retval;
        _summary.copmelete_value = mileTaskTotal;
        _summary.pending_value = mileWattagepending;


        // return dscode.GetXml();
        return _summary;
    }
    [WebMethod]
    public static string UpdateTask_Status(string taskid = "", string statusid = "", string iscomplete = "")
    {
        if (!HasPmsEditPermission("55")) return "You do not have edit permission.";
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (taskid != "" && taskid != "0")
        {
            entrytype = "Update";
        }




        // percentage update start  // 
        if (iscomplete == "1")
        {
            string independency = checkGanttPredecessorCompletion("task", taskid);
            if (independency != "0")
            {
                retval = independency == "1" ? "Please complete first dependency of this? " : independency;
            }
            else
            {
                SqlParameter[] prm = new SqlParameter[8];
                prm[0] = new SqlParameter("@id", taskid);
                prm[1] = new SqlParameter("@ProjectStatus", statusid);
                prm[2] = new SqlParameter("@iscomplete", iscomplete);
                prm[3] = new SqlParameter("@iscompletedate", da1.getcdatetime());
                prm[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prm[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                prm[6] = new SqlParameter("@cdate", da1.getcdatetime());
                prm[7] = new SqlParameter("@type", entrytype);
                da1.executeDMLQuery("pmstbl_task_Status_DeatilsUpdate", prm);
                decimal tsktotalpercent = 100;
                decimal totlweitagemile = 0;
                decimal totlweitageproject = 0;
                string finsql1 = "select * from Pms_tbl_Addtask where id=" + taskid + " and status=1 and archive=0";
                DataSet dsfindid1 = da1.GetDataSetInline(finsql1);

                // task percentage //
                string strstatusupdate = "update Pms_tbl_Addtask set iscompletepercentage='" + tsktotalpercent.ToString() + "' where id='" + taskid + "' and status=1 and archive=0 and isnull(iscomplete,0)=1";
                DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                // milestone percentage //
                string finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'   and status=1  and archive=0";
                DataSet dsfindid2 = da1.GetDataSetInline(finsql2);
                for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
                {
                    decimal sumweitage = 0;
                    decimal subpercentageemile = 0;
                    sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
                    subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                    totlweitagemile += (sumweitage * subpercentageemile) / 100;

                }

                string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile.ToString() + "' where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and id='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1 and archive=0";
                DataSet dsfindid4 = da1.GetDataSetInline(finsql4);

                // project percentage //

                string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1  and archive=0";
                DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
                for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
                {
                    decimal sumweitageprojt = 0;
                    decimal subpercentageeproject = 0;
                    sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                    subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                    totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

                }

                string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
                DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);



                //project  log details insert start
                SqlParameter[] prmlog = new SqlParameter[8];
                prmlog[0] = new SqlParameter("@Transid", taskid);
                prmlog[1] = new SqlParameter("@Logstatus", statusid);
                prmlog[2] = new SqlParameter("@LogDate", da1.getcdatetime());
                prmlog[3] = new SqlParameter("@Logtype", "task");
                prmlog[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prmlog[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                prmlog[6] = new SqlParameter("@cdate", da1.getcdatetime());
                prmlog[7] = new SqlParameter("@type", "Insert");
                da1.executeDMLQuery("pmstbl_Loginsert_usp", prmlog);
                //project  log details insert end
                retval = "1";
                //string strsql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + statusid + "' ";
                //DataSet dscode = da1.GetDataSetInline(strsql);
                //if (dscode.Tables[0].Rows.Count > 0)
                //{
                //    retval = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
                //}
            }

            
        }
        else
        {
            SqlParameter[] prm = new SqlParameter[8];
            prm[0] = new SqlParameter("@id", taskid);
            prm[1] = new SqlParameter("@ProjectStatus", statusid);
            prm[2] = new SqlParameter("@iscomplete", iscomplete);
            prm[3] = new SqlParameter("@iscompletedate", da1.getcdatetime());
            prm[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prm[6] = new SqlParameter("@cdate", da1.getcdatetime());
            prm[7] = new SqlParameter("@type", entrytype);
            da1.executeDMLQuery("pmstbl_task_Status_DeatilsUpdate", prm);
            decimal tsktotalpercent = 0;
            decimal totlweitagemile = 0;
            decimal totlweitageproject = 0;

            string finsql1 = "select * from Pms_tbl_Addtask where id=" + taskid + " and status=1  and archive=0";
            DataSet dsfindid1 = da1.GetDataSetInline(finsql1);

            // task percentage //
            if (dsfindid1.Tables[0].Rows.Count > 0)
            {
                if (dsfindid1.Tables[0].Rows[0]["issubtask"].ToString() == "1")
                {

                    string finsql211 = "select isnull(sum(weightage),0)Wattagetasksum from Pms_tbl_Addsubtask where   task='" + taskid + "'  and status=1 and isnull(iscomplete,0)=1 and archive=0";
                    DataSet dsfindid211 = da1.GetDataSetInline(finsql211);
                    if (dsfindid211.Tables[0].Rows.Count > 0)
                    {
                        string strstatusupdate = "update Pms_tbl_Addtask set iscompletepercentage='" + dsfindid211.Tables[0].Rows[0]["Wattagetasksum"] + "' where id='" + taskid + "' and status=1 and archive=0";
                        DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);
                    }
                }
                else
                {
                    string strstatusupdate = "update Pms_tbl_Addtask set iscompletepercentage='0' where id='" + taskid + "' and status=1 and archive=0";
                    DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                }

            }


            // milestone percentage //

            string finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1  and archive=0";
            DataSet dsfindid2 = da1.GetDataSetInline(finsql2);
            for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
            {
                decimal sumweitage = 0;
                decimal subpercentageemile = 0;
                sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
                subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                totlweitagemile += (sumweitage * subpercentageemile) / 100;

            }

            string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile + "' where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and id='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da1.GetDataSetInline(finsql4);

            // project percentage //

            string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'   and status=1  and archive=0";
            DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
            for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
            {
                decimal sumweitageprojt = 0;
                decimal subpercentageeproject = 0;
                sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

            }

            string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
            DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);



            //project  log details insert start
            SqlParameter[] prmlog = new SqlParameter[8];
            prmlog[0] = new SqlParameter("@Transid", taskid);
            prmlog[1] = new SqlParameter("@Logstatus", statusid);
            prmlog[2] = new SqlParameter("@LogDate", da1.getcdatetime());
            prmlog[3] = new SqlParameter("@Logtype", "task");
            prmlog[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prmlog[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prmlog[6] = new SqlParameter("@cdate", da1.getcdatetime());
            prmlog[7] = new SqlParameter("@type", "Insert");
            da1.executeDMLQuery("pmstbl_Loginsert_usp", prmlog);
            //project  log details insert end
            retval = "1";
            //string strsql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + statusid + "' ";
            //DataSet dscode = da1.GetDataSetInline(strsql);
            //if (dscode.Tables[0].Rows.Count > 0)
            //{
            //    retval = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
            //}

        }

        // percentage update end  // 




        return retval;
    }

    [WebMethod]
    public static string UpdateSubtask_Status(string subtaskid = "", string statusid = "", string iscomplete = "")
    {
        if (!HasPmsEditPermission("56")) return "You do not have edit permission.";
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";

        if (iscomplete == "1")
        {
            string independency = checkGanttPredecessorCompletion("subtask", subtaskid);
            if (independency != "0")
            {
                retval = independency == "1" ? "Please complete first dependency of this? " : independency;
            }
            else
            {

                SqlParameter[] prm = new SqlParameter[8];
                prm[0] = new SqlParameter("@id", subtaskid);
                prm[1] = new SqlParameter("@ProjectStatus", statusid);
                prm[2] = new SqlParameter("@iscomplete", iscomplete);
                prm[3] = new SqlParameter("@iscompletedate", da1.getcdatetime());
                prm[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prm[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                prm[6] = new SqlParameter("@cdate", da1.getcdatetime());
                prm[7] = new SqlParameter("@type", entrytype);
                da1.executeDMLQuery("pmstbl_subtask_Status_DeatilsUpdate", prm);


                decimal subtsktotalpercent = 100;
                decimal totlweitagemile = 0;
                decimal totlweitageproject = 0;

                string finsql1 = "select * from Pms_tbl_Addsubtask where id=" + subtaskid + " and status=1  and archive=0";
                DataSet dsfindid1 = da1.GetDataSetInline(finsql1);
                SaveLogText("finsql2-" + finsql1);
                string finsql2 = "select isnull(sum(weightage),0)Wattagetasksum from Pms_tbl_Addsubtask where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "' and task='" + dsfindid1.Tables[0].Rows[0]["task"] + "'  and status=1 and  archive=0 and isnull(iscomplete,0)=1";

                DataSet dsfindid2 = da1.GetDataSetInline(finsql2);

                //sub task percentage //
                string strstatusupdate = "update Pms_tbl_Addsubtask set iscompletepercentage='" + subtsktotalpercent.ToString() + "' where id='" + subtaskid + "' and status=1 and archive=0 and isnull(iscomplete,0)=1";
                DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                // task percentage //
                string finsql3 = "update Pms_tbl_Addtask set iscompletepercentage='" + dsfindid2.Tables[0].Rows[0]["Wattagetasksum"] + "' where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "' and id='" + dsfindid1.Tables[0].Rows[0]["task"] + "' and status=1 and archive=0";
                DataSet dsfindid3 = da1.GetDataSetInline(finsql3);


                // Milestone percentage //
                string findsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1  and archive=0";
                DataSet dsfindidmile = da1.GetDataSetInline(findsqlmile);
                for (int j = 0; j < dsfindidmile.Tables[0].Rows.Count; j++)
                {
                    decimal sumweitage = 0;
                    decimal subpercentageemile = 0;
                    sumweitage = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["Weightage"].ToString());
                    subpercentageemile = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                    totlweitagemile += (sumweitage * subpercentageemile) / 100;

                }

                string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile + "' where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and id='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1 and archive=0";
                DataSet dsfindid4 = da1.GetDataSetInline(finsql4);

                // project percentage //
                string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'   and status=1  and archive=0";
                DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
                for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
                {
                    decimal sumweitageprojt = 0;
                    decimal subpercentageeproject = 0;
                    sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                    subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                    totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

                }

                string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'   and status=1 and archive=0";
                DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);
                //project  log details insert start
                SqlParameter[] prmlog = new SqlParameter[8];
                prmlog[0] = new SqlParameter("@Transid", subtaskid);
                prmlog[1] = new SqlParameter("@Logstatus", statusid);
                prmlog[2] = new SqlParameter("@LogDate", da1.getcdatetime());
                prmlog[3] = new SqlParameter("@Logtype", "subtask");
                prmlog[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                prmlog[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
                prmlog[6] = new SqlParameter("@cdate", da1.getcdatetime());
                prmlog[7] = new SqlParameter("@type", "Insert");
                da1.executeDMLQuery("pmstbl_Loginsert_usp", prmlog);
                //project  log details insert end


                retval = "1";

                //string strsql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + statusid + "' ";
                //DataSet dscode = da1.GetDataSetInline(strsql);
                //if (dscode.Tables[0].Rows.Count > 0)
                //{
                //    retval = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
                //}
            }


        }
        else
        {
            decimal subtsktotalpercent = 0;
            decimal totlweitagemile = 0;
            decimal totlweitageproject = 0;
            SqlParameter[] prm = new SqlParameter[8];
            prm[0] = new SqlParameter("@id", subtaskid);
            prm[1] = new SqlParameter("@ProjectStatus", statusid);
            prm[2] = new SqlParameter("@iscomplete", iscomplete);
            prm[3] = new SqlParameter("@iscompletedate", da1.getcdatetime());
            prm[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prm[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prm[6] = new SqlParameter("@cdate", da1.getcdatetime());
            prm[7] = new SqlParameter("@type", entrytype);
            da1.executeDMLQuery("pmstbl_subtask_Status_DeatilsUpdate", prm);
            string finsql1 = "select * from Pms_tbl_Addsubtask where id=" + subtaskid + " and status=1  and archive=0";
            DataSet dsfindid1 = da1.GetDataSetInline(finsql1);

            string finsql2 = "select isnull(sum(weightage),0)Wattagetasksum from Pms_tbl_Addsubtask where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "' and task='" + dsfindid1.Tables[0].Rows[0]["task"] + "'  and status=1 and isnull(iscomplete,0)=1 and archive=0";
            DataSet dsfindid2 = da1.GetDataSetInline(finsql2);

            //sub task percentage //
            string strstatusupdate = "update Pms_tbl_Addsubtask set iscompletepercentage='" + subtsktotalpercent.ToString() + "' where id='" + subtaskid + "' and status=1 and archive=0";
            DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

            //task percentage //
            string finsql3 = "update Pms_tbl_Addtask set iscompletepercentage='" + dsfindid2.Tables[0].Rows[0]["Wattagetasksum"] + "' where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "' and id='" + dsfindid1.Tables[0].Rows[0]["task"] + "' and status=1 and archive=0";
            DataSet dsfindid3 = da1.GetDataSetInline(finsql3);

            //milestone percentage //
            string findsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'    and status=1  and archive=0";
            DataSet dsfindidmile = da1.GetDataSetInline(findsqlmile);
            for (int j = 0; j < dsfindidmile.Tables[0].Rows.Count; j++)
            {
                decimal sumweitage = 0;
                decimal subpercentageemile = 0;
                sumweitage = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["Weightage"].ToString());
                subpercentageemile = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                totlweitagemile += (sumweitage * subpercentageemile) / 100;

            }

            string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile + "' where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and id='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da1.GetDataSetInline(finsql4);


            // project percentage //
            string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'   and status=1  and archive=0";
            DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
            for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
            {
                decimal sumweitageprojt = 0;
                decimal subpercentageeproject = 0;
                sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

            }

            string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'   and status=1 and archive=0";
            DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);
            //project  log details insert start
            SqlParameter[] prmlog = new SqlParameter[8];
            prmlog[0] = new SqlParameter("@Transid", subtaskid);
            prmlog[1] = new SqlParameter("@Logstatus", statusid);
            prmlog[2] = new SqlParameter("@LogDate", da1.getcdatetime());
            prmlog[3] = new SqlParameter("@Logtype", "subtask");
            prmlog[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            prmlog[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
            prmlog[6] = new SqlParameter("@cdate", da1.getcdatetime());
            prmlog[7] = new SqlParameter("@type", "Insert");
            da1.executeDMLQuery("pmstbl_Loginsert_usp", prmlog);
            //project  log details insert end
            retval = "1";



            //string strsql = "select id,ProjectStatus,PrintColor from Pms_tbl_Projectstatus where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + statusid + "' ";
            //DataSet dscode = da1.GetDataSetInline(strsql);
            //if (dscode.Tables[0].Rows.Count > 0)
            //{
            //    retval = dscode.Tables[0].Rows[0]["ProjectStatus"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
            //}


        }
        // percentage update end  // 




        return retval;
    }

   
    #endregion
    #region update priority
    [WebMethod]
    public static string MileUpdatePriority_Status(string milstoneid = "", string priorityid = "")
    {
        if (!HasPmsEditPermission("54")) return "You do not have edit permission.";
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (milstoneid != "" && milstoneid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", milstoneid);
        prm[1] = new SqlParameter("@Priority", priorityid);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pmstbl_Priority_milstone_DtlsUpdate", prm);

        DataAccess da = new DataAccess();
        string strsql = "select Id,name,PrintColor from Pms_tbl_Priority where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + priorityid + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            retval = dscode.Tables[0].Rows[0]["name"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
        }
        // return dscode.GetXml();
        return retval;
    }
    [WebMethod]
    public static string TaskUpdatePriority_Status(string taskid = "", string priorityid = "")
    {
        if (!HasPmsEditPermission("55")) return "You do not have edit permission.";
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (taskid != "" && taskid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", taskid);
        prm[1] = new SqlParameter("@Priority", priorityid);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pms_tbl_task_Priority_DeatilsUpdate", prm);

        DataAccess da = new DataAccess();
        string strsql = "select Id,name,PrintColor from Pms_tbl_Priority  where status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + priorityid + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            retval = dscode.Tables[0].Rows[0]["name"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
        }

        return retval;
    }


    [WebMethod]
    public static string SubtaskUpdatePriority_Status(string subtaskid = "", string priorityid = "")
    {
        if (!HasPmsEditPermission("56")) return "You do not have edit permission.";
        string retval = "";
        DataAccess da1 = new DataAccess();
        string entrytype = "Update";
        if (subtaskid != "" && subtaskid != "0")
        {
            entrytype = "Update";
        }

        SqlParameter[] prm = new SqlParameter[6];
        prm[0] = new SqlParameter("@id", subtaskid);
        prm[1] = new SqlParameter("@Priority", priorityid);
        prm[2] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[3] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[4] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[5] = new SqlParameter("@type", entrytype);
        da1.executeDMLQuery("pms_tbl_Subtask_Priority_DeatilsUpdate", prm);

        DataAccess da = new DataAccess();
        string strsql = "select Id,name,PrintColor from Pms_tbl_Priority  where  status=1 and archive=0   and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + priorityid + "' ";
        DataSet dscode = da.GetDataSetInline(strsql);
        if (dscode.Tables[0].Rows.Count > 0)
        {
            retval = dscode.Tables[0].Rows[0]["name"].ToString() + "~~" + dscode.Tables[0].Rows[0]["PrintColor"].ToString();
        }

        return retval;

    }

    #endregion
    private static string NormalizeAssignmentTaskType(string value)
    {
        string type = (value ?? string.Empty).Trim().ToLowerInvariant();
        if (type == "milestones") return "milestone";
        if (type == "tasks") return "task";
        if (type == "subtasks") return "subtask";
        return type;
    }

    private static DateTime ParseAssignDateOnly(string value)
    {
        if (String.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Assignment date is required.");

        DateTime parsed;
        if (TryParseDateOnlyInvariant(value, out parsed))
            return parsed;

        throw new FormatException("Invalid assignment date: " + value);
    }

    // Server-side safety guard for direct assignment calls.
    // The Angular client validates the full milestone before scheduling any child;
    // this method also prevents a task/milestone without subtasks from being saved directly.
    private static Dictionary<string, object> ReadAssignmentProjectSettings(DataAccess da, string id, string utype)
    {
        int itemId, regId;
        HttpCookie cookie = HttpContext.Current.Request.Cookies["cadmin"];
        string table = GetAssignTableName(NormalizeAssignmentTaskType(utype));
        if (cookie == null || !int.TryParse(cookie["id"], out regId)
            || !int.TryParse(id, out itemId) || itemId <= 0 || regId <= 0 || table == "")
            throw new InvalidOperationException("Invalid assignment item.");
        string sql = "select p.id,isnull(p.hastask,0) hastask,isnull(p.hassubtask,0) hassubtask"
            + " from " + table + " i inner join Pms_tbl_AddProject p on p.id=i.ProjectId and p.regid=i.regid"
            + " where i.id=" + itemId + " and i.regid=" + regId
            + " and i.status=1 and i.archive=0 and p.status=1 and p.archive=0";
        DataSet ds = da.GetDataSetInline(sql);
        if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            throw new InvalidOperationException("Assignment item or project not found.");
        DataRow row = ds.Tables[0].Rows[0];
        string level = Convert.ToInt32(row["hassubtask"]) == 1 ? "subtask"
            : Convert.ToInt32(row["hastask"]) == 1 ? "task" : "milestone";
        return new Dictionary<string, object> { { "ProjectId", Convert.ToInt32(row["id"]) }, { "Level", level } };
    }

    [WebMethod]
    public static Dictionary<string, object> GetAssignmentProjectSettings(string id, string utype)
    {
        return ReadAssignmentProjectSettings(new DataAccess(), id, utype);
    }

    private static string ValidateAssignHierarchyHasSubtask(DataAccess da, string id, string utype)
    {
        utype = NormalizeAssignmentTaskType(utype);
        try
        {
            string type = (utype ?? string.Empty).Trim().ToLowerInvariant();
            string level = Convert.ToString(ReadAssignmentProjectSettings(da, id, type)["Level"]);
            if (type == "subtask")
                return level == "subtask" ? string.Empty : "Subtask assignment is not enabled for this project.";
            if (level == "milestone")
                return type == "milestone" ? string.Empty : "This project allows milestone assignment only.";
            if (level == "task")
            {
                if (type == "task") return string.Empty;
                return "This project allows task assignment. Please create and assign the milestone's tasks instead of assigning the milestone directly.";
            }
            if (type != "task" && type != "milestone") return "Invalid assignment type.";

            string regid = HttpContext.Current.Request.Cookies["cadmin"]["id"];
            string safeId = id.Replace("'", "''");
            string safeRegId = (regid ?? string.Empty).Replace("'", "''");

            if (type == "task")
            {
                string taskSql = @"
select top(1)
    isnull(t.taskname,'Selected task') as taskname,
    case when exists
    (
        select 1
        from Pms_tbl_Addsubtask st with(nolock)
        where st.Task=t.id
          and st.regid=t.regid
          and st.status=1
          and st.archive=0
    ) then 1 else 0 end as hassubtask
from Pms_tbl_Addtask t with(nolock)
where t.id='" + safeId + @"'
  and t.regid='" + safeRegId + @"'
  and t.status=1
  and t.archive=0";

                DataSet taskDs = da.GetDataSetInline(taskSql);
                if (taskDs.Tables.Count > 0 && taskDs.Tables[0].Rows.Count > 0 &&
                    Convert.ToInt32(taskDs.Tables[0].Rows[0]["hassubtask"]) == 0)
                {
                    return "Task \"" + taskDs.Tables[0].Rows[0]["taskname"].ToString() +
                           "\" cannot be assigned without subtask. Kindly create a subtask first.";
                }

                return "This project allows subtask assignment. Please assign this task's subtasks.";
            }

            string milestoneSql = @"
select top(1) isnull(Milestone,'Selected milestone') as Milestone
from Pms_tbl_AddMilestone with(nolock)
where id='" + safeId + @"'
  and regid='" + safeRegId + @"'
  and status=1
  and archive=0;

select top(1) isnull(t.taskname,'Selected task') as taskname
from Pms_tbl_Addtask t with(nolock)
where t.Milestoneid='" + safeId + @"'
  and t.regid='" + safeRegId + @"'
  and t.status=1
  and t.archive=0
  and not exists
  (
      select 1
      from Pms_tbl_Addsubtask st with(nolock)
      where st.Task=t.id
        and st.regid=t.regid
        and st.status=1
        and st.archive=0
  );

select count(1) as taskcount
from Pms_tbl_Addtask with(nolock)
where Milestoneid='" + safeId + @"'
  and regid='" + safeRegId + @"'
  and status=1
  and archive=0;";

            DataSet milestoneDs = da.GetDataSetInline(milestoneSql);
            string milestoneName = "Selected milestone";
            if (milestoneDs.Tables.Count > 0 && milestoneDs.Tables[0].Rows.Count > 0)
                milestoneName = milestoneDs.Tables[0].Rows[0]["Milestone"].ToString();

            if (milestoneDs.Tables.Count > 2 && milestoneDs.Tables[2].Rows.Count > 0 &&
                Convert.ToInt32(milestoneDs.Tables[2].Rows[0]["taskcount"]) == 0)
            {
                return "Milestone \"" + milestoneName +
                       "\" cannot be assigned without task/subtask. Kindly create a task and subtask first.";
            }

            if (milestoneDs.Tables.Count > 1 && milestoneDs.Tables[1].Rows.Count > 0)
            {
                return "Milestone \"" + milestoneName + "\" cannot be assigned because task \"" +
                       milestoneDs.Tables[1].Rows[0]["taskname"].ToString() +
                       "\" has no subtask. Kindly create a subtask first.";
            }
        }
        catch (Exception ex)
        {
            SaveLogText("ValidateAssignHierarchyHasSubtaskEX-" + ex.Message);
            return "Unable to verify the project's assignment settings. Please try again.";
        }

        return "This project allows subtask assignment. Please assign the milestone's subtasks.";
    }

    #region Assign to
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string updateOwner_assign(string id, string empid, string utype,string changedate,string starttime)
    {
        utype = NormalizeAssignmentTaskType(utype);
        string retval = "0";
        string pid = "0";
        DataAccess da1 = new DataAccess();
        if (id != string.Empty && id != "0")
        {
            string hierarchyValidation = ValidateAssignHierarchyHasSubtask(da1, id, utype);
            if (!string.IsNullOrEmpty(hierarchyValidation))
                return hierarchyValidation;

            SaveLogText("strAssignto1-" + id+","+ empid + "," + utype + "," + changedate + "," + starttime);
            string tblname = "";
            if (utype == "milestone")
            {
                tblname = "Pms_tbl_AddMilestone";
            }
            else if (utype == "task")
            {
                tblname = "Pms_tbl_Addtask";
            }
            else if (utype == "subtask")
            {
                tblname = "Pms_tbl_Addsubtask";
            }
            if (tblname != string.Empty)
            {
                try
                {
                    string strsql = "select id,StartDate,EndDate,isnull(txtdays,0) as txtdays,isnull(TaskDuration,'01:00') TaskDuration,projectid from " + tblname + " where id='" + id + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0 ";
                    if (utype == "task" || utype == "subtask")
                        strsql = "select id,StartDate,Targetdate as EndDate,isnull(txtdays,0) as txtdays,isnull(TaskDuration,'01:00') TaskDuration,projectid from " + tblname + " where id='" + id + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0 ";
                    SaveLogText("strAssignto2-" + strsql);
                    DataSet dsS = da1.GetDataSetInline(strsql);
                    string txtday = "0";
                    try
                    {
                        pid = dsS.Tables[0].Rows[0]["projectid"].ToString();
                        txtday = dsS.Tables[0].Rows[0]["txtdays"].ToString();
                    }
                    catch { }
                    
                    string durationText = "01:00";
                    try { durationText = dsS.Tables[0].Rows[0]["TaskDuration"].ToString(); } catch { }

                    DateTime dropStartDateTime = ParseAssignDateOnly(changedate).Add(ParseTimeOfDayInvariant(starttime, "09:00 AM"));
                    int dropDurationMinutes = AssignDurationToMinutes(durationText);
                    if (HasSchedulerOverlapOnShiftSegments(da1, empid, dropStartDateTime, dropDurationMinutes, "0", utype, id))
                    {
                        return "Drop only in available area";
                    }

                    DateTime _enddate = ParseAssignDateOnly(changedate);
                    try
                    {
                        _enddate = GetEstimatedAssignEndDate(empid, ParseAssignDateOnly(changedate), starttime, durationText);
                    }
                    catch
                    {
                        string chkdate = manage_enddatenew(txtday, changedate, pid);
                        try { _enddate = ParseAssignDateOnly(chkdate); } catch { }
                    }

                    // Do not update original Owner from scheduler assignment.
                    // Assigned employee is saved only in Pms_tbl_assigntasks.ownerid.
                    string assignDateSql = ParseAssignDateOnly(changedate).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    string assignEndSql = _enddate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    string strownerupdate = "update " + tblname + " set startdate='" + assignDateSql + "',EndDate='" + assignEndSql + "',StartTime='" + starttime + "'  where id='" + id + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0 ";
                    if (utype == "task"||utype == "subtask")
                        strownerupdate = "update " + tblname + " set startdate='" + assignDateSql + "',Targetdate='" + assignEndSql + "',StartTime='" + starttime + "'  where id='" + id + "' and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and status=1 and archive=0 ";

                    SaveLogText("strAssignto4-" + strownerupdate);

                    // da1.executeDMLQueryinline(strownerupdate);

                    // Also save scheduler row in the single assignment table.
                    // Center panel display should be generated from this row at runtime.
                    if (utype == "subtask" || utype == "task" || utype == "milestone")
                    {
                        UpsertSchedulerAssignment(da1, id, empid, utype, changedate, starttime, durationText, _enddate);
                    }
                    retval = "1";
                }
                catch(Exception ex) {
                    SaveLogText("strAssignto5-" + ex.Message);
                }
            }

        }
        return retval;
    }




    public static void UpsertSchedulerAssignment(DataAccess da, string id, string empid, string utype, string changedate, string starttime, string durationText, DateTime endDate)
    {
        utype = NormalizeAssignmentTaskType(utype);
        try
        {
            string regid = HttpContext.Current.Request.Cookies["cadmin"]["id"];
            string uid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();
            string tblname = GetAssignTableName(utype);
            if (tblname == "") throw new InvalidOperationException("Invalid assignment type.");

            string projectid = "0", milestoneid = "0", taskid = "0", subtaskid = "0";
            string sel = "";
            if (utype == "milestone") sel = "select ProjectId,id as MilestoneId,0 as TaskId,0 as SubtaskId from Pms_tbl_AddMilestone where id='" + id + "' and regid='" + regid + "' and status=1 and archive=0";
            else if (utype == "task") sel = "select ProjectId,Milestoneid,id as TaskId,0 as SubtaskId from Pms_tbl_Addtask where id='" + id + "' and regid='" + regid + "' and status=1 and archive=0";
            else if (utype == "subtask") sel = "select ProjectId,Milestoneid,Task as TaskId,id as SubtaskId from Pms_tbl_Addsubtask where id='" + id + "' and regid='" + regid + "' and status=1 and archive=0";

            DataSet ds = da.GetDataSetInline(sel);
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) throw new InvalidOperationException("Assignment item not found.");
            projectid = ds.Tables[0].Rows[0]["ProjectId"].ToString();
            milestoneid = ds.Tables[0].Rows[0]["MilestoneId"].ToString();
            taskid = ds.Tables[0].Rows[0]["TaskId"].ToString();
            subtaskid = ds.Tables[0].Rows[0]["SubtaskId"].ToString();

            DateTime startdt = ParseAssignDateOnly(changedate).Add(ParseTimeOfDayInvariant(starttime, "09:00 AM"));
            string sdt = ToSqlDateTime(startdt);

            // IMPORTANT: enddatetime must not be same as startdatetime.
            // One DB assignment row is saved, but enddatetime is used for date-range visibility.
            // Timeline split is still calculated at runtime from taskduration + employee shift.
            int assignMinutes = AssignDurationToMinutes(durationText);
            DateTime enddt = CalculateAssignmentEndDateTime(empid, startdt, assignMinutes);
            string edt = ToSqlDateTime(enddt);
            string dur = AssignMinutesToDuration(assignMinutes).Replace("'", "''");

            string closeOld = "update Pms_tbl_assigntasks set status=0, archive=1, removedatetime=getdate() " +
                              "where regid='" + regid + "' and status=1 and archive=0 and tasktype='" + utype + "' ";
            if (utype == "milestone") closeOld += " and milestoneid='" + id + "' and regid='" + regid + "' and status=1 and archive=0";
            else if (utype == "task") closeOld += " and taskid='" + id + "' and regid='" + regid + "' and status=1 and archive=0";
            else closeOld += " and subtaskid='" + id + "' and regid='" + regid + "' and status=1 and archive=0";
            da.executeDMLQueryinline(closeOld);

            string ins = "insert into Pms_tbl_assigntasks(uid,regid,cdate,status,archive,projectid,milestoneid,taskid,tasktype,subtaskid,ownerid,startdatetime,enddatetime,taskduration,assigndatetime) values(" +
                         "'" + uid + "','" + regid + "',getdate(),1,0,'" + projectid + "','" + milestoneid + "','" + taskid + "','" + utype + "','" + subtaskid + "','" + empid + "','" + sdt + "','" + edt + "','" + dur + "',getdate())";
            da.executeDMLQueryinline(ins);
        }
        catch (Exception ex)
        {
            SaveLogText("UpsertSchedulerAssignmentEX-" + ex.Message);
            throw;
        }
    }

    public static int AssignDurationToMinutes(string duration)
    {
        return ParseDurationMinutesInvariant(duration, 60);
    }

    public static string AssignMinutesToDuration(int minutes)
    {
        minutes = Math.Max(0, minutes);
        int h = minutes / 60;
        int m = minutes % 60;
        return h.ToString("00") + ":" + m.ToString("00");
    }

    public static string GetAssignTableName(string utype)
    {
        utype = NormalizeAssignmentTaskType(utype);
        if (utype == "milestone") return "Pms_tbl_AddMilestone";
        if (utype == "task") return "Pms_tbl_Addtask";
        if (utype == "subtask") return "Pms_tbl_Addsubtask";
        return "";
    }



    public static string GetAssignDurationText(DataAccess da, string id, string utype)
    {
        utype = NormalizeAssignmentTaskType(utype);
        string durationText = "01:00";
        try
        {
            string tblname = GetAssignTableName(utype);
            if (tblname == "") return durationText;
            string safeId = (id ?? "0").Replace("'", "''");
            string sql = "select top(1) isnull(TaskDuration,'01:00') TaskDuration from " + tblname + " with(nolock) where id='" + safeId + "' and status=1 and archive=0";
            DataSet ds = da.GetDataSetInline(sql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                durationText = ds.Tables[0].Rows[0]["TaskDuration"].ToString();
        }
        catch { }
        return NormalizeDurationTextInvariant(durationText, String.Empty);
    }

    public static DateTime NormalizeToShiftCursor(string empid, DateTime cursor)
    {
        DateTime shiftStart = DateTime.Today.Add(ParseTimeOfDayInvariant("09:00 AM", "09:00 AM"));
        DateTime shiftEnd = DateTime.Today.Add(ParseTimeOfDayInvariant("06:00 PM", "06:00 PM"));
        try
        {
            string shift = getemployeeshift2(empid);
            if (!string.IsNullOrEmpty(shift))
            {
                string[] a = shift.Split('-');
                if (a.Length >= 3)
                {
                    shiftStart = DateTime.Today.Add(ParseTimeOfDayInvariant(a[0], "09:00 AM"));
                    shiftEnd = DateTime.Today.Add(ParseTimeOfDayInvariant(a[2], "06:00 PM"));
                }
            }
        }
        catch { }

        DateTime dayShiftStart = cursor.Date.Add(shiftStart.TimeOfDay);
        DateTime dayShiftEnd = cursor.Date.Add(shiftEnd.TimeOfDay);
        if (dayShiftEnd <= dayShiftStart) dayShiftEnd = dayShiftEnd.AddDays(1);

        if (cursor < dayShiftStart) return dayShiftStart;
        if (cursor >= dayShiftEnd) return cursor.Date.AddDays(1).Add(shiftStart.TimeOfDay);
        return cursor;
    }

    public static bool TryBuildNonOverlapSchedule(DataAccess da, string empid, DateTime startdt, int durationMinutes, string excludeTaskType, string excludeItemId, out DateTime enddt, out DateTime conflictEnd)
    {
        enddt = startdt;
        conflictEnd = DateTime.MinValue;
        try
        {
            string regid = HttpContext.Current.Request.Cookies["cadmin"]["id"];
            DateTime shiftStart = DateTime.Today.Add(ParseTimeOfDayInvariant("09:00 AM", "09:00 AM"));
            DateTime shiftEnd = DateTime.Today.Add(ParseTimeOfDayInvariant("06:00 PM", "06:00 PM"));
            try
            {
                string shift = getemployeeshift2(empid);
                if (!string.IsNullOrEmpty(shift))
                {
                    string[] a = shift.Split('-');
                    if (a.Length >= 3)
                    {
                        shiftStart = DateTime.Today.Add(ParseTimeOfDayInvariant(a[0], "09:00 AM"));
                        shiftEnd = DateTime.Today.Add(ParseTimeOfDayInvariant(a[2], "06:00 PM"));
                    }
                }
            }
            catch { }

            DateTime cursor = startdt;
            int remaining = durationMinutes <= 0 ? 60 : durationMinutes;
            int safety = 0;

            while (remaining > 0 && safety < 180)
            {
                safety++;
                DateTime dayShiftStart = cursor.Date.Add(shiftStart.TimeOfDay);
                DateTime dayShiftEnd = cursor.Date.Add(shiftEnd.TimeOfDay);
                if (dayShiftEnd <= dayShiftStart) dayShiftEnd = dayShiftEnd.AddDays(1);

                if (cursor < dayShiftStart) cursor = dayShiftStart;
                if (cursor >= dayShiftEnd)
                {
                    cursor = cursor.Date.AddDays(1).Add(shiftStart.TimeOfDay);
                    continue;
                }

                int available = (int)(dayShiftEnd - cursor).TotalMinutes;
                int take = Math.Min(remaining, available);
                DateTime segStart = cursor;
                DateTime segEnd = cursor.AddMinutes(take);

                string sql = "select top(1) id,startdatetime,isnull(enddatetime,startdatetime) enddatetime from Pms_tbl_assigntasks with(nolock) " +
                             "where regid='" + regid + "' and status=1 and archive=0 " +
                             "and ownerid='" + (empid ?? "").Replace("'", "''") + "' " +
                             "and startdatetime < '" + ToSqlDateTime(segEnd) + "' " +
                             "and isnull(enddatetime,startdatetime) > '" + ToSqlDateTime(segStart) + "' ";

                if (!string.IsNullOrEmpty(excludeTaskType) && !string.IsNullOrEmpty(excludeItemId) && excludeItemId != "0")
                {
                    string safeType = excludeTaskType.Replace("'", "''").ToLower();
                    string safeItem = excludeItemId.Replace("'", "''");
                    if (safeType == "milestone") sql += " and not (tasktype='milestone' and milestoneid='" + safeItem + "') ";
                    else if (safeType == "task") sql += " and not (tasktype='task' and taskid='" + safeItem + "') ";
                    else if (safeType == "subtask") sql += " and not (tasktype='subtask' and subtaskid='" + safeItem + "') ";
                }
                sql += " order by startdatetime";

                DataSet ds = da.GetDataSetInline(sql);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DateTime busyEnd = Convert.ToDateTime(ds.Tables[0].Rows[0]["enddatetime"]);
                    conflictEnd = busyEnd > segStart ? busyEnd : segStart.AddMinutes(15);
                    return false;
                }

                cursor = segEnd;
                remaining -= take;
                if (remaining > 0) cursor = cursor.Date.AddDays(1).Add(shiftStart.TimeOfDay);
            }

            enddt = cursor;
            return remaining <= 0;
        }
        catch (Exception ex)
        {
            SaveLogText("TryBuildNonOverlapScheduleEX-" + ex.Message);
            return false;
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string findNextAvailableAssignStart(string id, string empid, string utype, string changedate, string starttime)
    {
        utype = NormalizeAssignmentTaskType(utype);
        try
        {
            DataAccess da = new DataAccess();
            string durationText = GetAssignDurationText(da, id, utype);
            int durationMinutes = AssignDurationToMinutes(durationText);
            DateTime cursor = ParseAssignDateOnly(changedate).Add(ParseTimeOfDayInvariant(starttime, "09:00 AM"));
            cursor = NormalizeToShiftCursor(empid, cursor);
            int safety = 0;
            while (safety < 500)
            {
                safety++;
                DateTime enddt, conflictEnd;
                if (TryBuildNonOverlapSchedule(da, empid, cursor, durationMinutes, utype, id, out enddt, out conflictEnd))
                {
                    return "1~~" + cursor.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "~~" + cursor.ToString("hh:mm tt", CultureInfo.InvariantCulture) + "~~" + enddt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "~~" + enddt.ToString("hh:mm tt", CultureInfo.InvariantCulture);
                }
                if (conflictEnd == DateTime.MinValue)
                    cursor = cursor.AddMinutes(15);
                else
                    cursor = conflictEnd;

                cursor = NormalizeToShiftCursor(empid, cursor);
            }
        }
        catch (Exception ex)
        {
            SaveLogText("findNextAvailableAssignStartEX-" + ex.Message);
        }
        return "Drop only in available area";
    }


    public static DateTime CalculateAssignmentEndDateTime(string empid, DateTime startdt, int durationMinutes)
    {
        DateTime shiftStart = DateTime.Today.Add(ParseTimeOfDayInvariant("09:00 AM", "09:00 AM"));
        DateTime shiftEnd = DateTime.Today.Add(ParseTimeOfDayInvariant("06:00 PM", "06:00 PM"));
        try
        {
            string shift = getemployeeshift2(empid);
            if (!string.IsNullOrEmpty(shift))
            {
                string[] a = shift.Split('-');
                if (a.Length >= 3)
                {
                    shiftStart = DateTime.Today.Add(ParseTimeOfDayInvariant(a[0], "09:00 AM"));
                    shiftEnd = DateTime.Today.Add(ParseTimeOfDayInvariant(a[2], "06:00 PM"));
                }
            }
        }
        catch { }

        DateTime cursor = startdt;
        int remaining = durationMinutes <= 0 ? 60 : durationMinutes;
        int safety = 0;

        while (remaining > 0 && safety < 90)
        {
            safety++;
            DateTime dayShiftStart = cursor.Date.Add(shiftStart.TimeOfDay);
            DateTime dayShiftEnd = cursor.Date.Add(shiftEnd.TimeOfDay);
            if (dayShiftEnd <= dayShiftStart) dayShiftEnd = dayShiftEnd.AddDays(1);

            if (cursor < dayShiftStart) cursor = dayShiftStart;
            if (cursor >= dayShiftEnd)
            {
                cursor = cursor.Date.AddDays(1).Add(shiftStart.TimeOfDay);
                continue;
            }

            int available = (int)(dayShiftEnd - cursor).TotalMinutes;
            int take = Math.Min(remaining, available);
            cursor = cursor.AddMinutes(take);
            remaining -= take;

            if (remaining > 0)
                cursor = cursor.Date.AddDays(1).Add(shiftStart.TimeOfDay);
        }

        return cursor;
    }

    public static bool HasSchedulerOverlapOnShiftSegments(DataAccess da, string empid, DateTime startdt, int durationMinutes, string excludeAssignId, string excludeTaskType, string excludeItemId)
    {
        try
        {
            string regid = HttpContext.Current.Request.Cookies["cadmin"]["id"];
            DateTime shiftStart = DateTime.Today.Add(ParseTimeOfDayInvariant("09:00 AM", "09:00 AM"));
            DateTime shiftEnd = DateTime.Today.Add(ParseTimeOfDayInvariant("06:00 PM", "06:00 PM"));

            try
            {
                string shift = getemployeeshift2(empid);
                if (!string.IsNullOrEmpty(shift))
                {
                    string[] a = shift.Split('-');
                    if (a.Length >= 3)
                    {
                        shiftStart = DateTime.Today.Add(ParseTimeOfDayInvariant(a[0], "09:00 AM"));
                        shiftEnd = DateTime.Today.Add(ParseTimeOfDayInvariant(a[2], "06:00 PM"));
                    }
                }
            }
            catch { }

            DateTime cursor = startdt;
            int remaining = durationMinutes <= 0 ? 60 : durationMinutes;
            int safety = 0;

            while (remaining > 0 && safety < 120)
            {
                safety++;

                DateTime dayShiftStart = cursor.Date.Add(shiftStart.TimeOfDay);
                DateTime dayShiftEnd = cursor.Date.Add(shiftEnd.TimeOfDay);
                if (dayShiftEnd <= dayShiftStart) dayShiftEnd = dayShiftEnd.AddDays(1);

                if (cursor < dayShiftStart) cursor = dayShiftStart;
                if (cursor >= dayShiftEnd)
                {
                    cursor = cursor.Date.AddDays(1).Add(shiftStart.TimeOfDay);
                    continue;
                }

                int available = (int)(dayShiftEnd - cursor).TotalMinutes;
                int take = Math.Min(remaining, available);
                DateTime segStart = cursor;
                DateTime segEnd = cursor.AddMinutes(take);

                string sql = "select top(1) id from Pms_tbl_assigntasks with(nolock) " +
                             "where regid='" + regid + "' and status=1 and archive=0 " +
                             "and ownerid='" + (empid ?? "").Replace("'", "''") + "' " +
                             "and startdatetime < '" + ToSqlDateTime(segEnd) + "' " +
                             "and isnull(enddatetime,startdatetime) > '" + ToSqlDateTime(segStart) + "' ";

                if (!string.IsNullOrEmpty(excludeAssignId) && excludeAssignId != "0")
                    sql += " and id<>'" + excludeAssignId.Replace("'", "''") + "' ";

                if (!string.IsNullOrEmpty(excludeTaskType) && !string.IsNullOrEmpty(excludeItemId) && excludeItemId != "0")
                {
                    string safeType = excludeTaskType.Replace("'", "''").ToLower();
                    string safeItem = excludeItemId.Replace("'", "''");
                    if (safeType == "milestone") sql += " and not (tasktype='milestone' and milestoneid='" + safeItem + "') ";
                    else if (safeType == "task") sql += " and not (tasktype='task' and taskid='" + safeItem + "') ";
                    else if (safeType == "subtask") sql += " and not (tasktype='subtask' and subtaskid='" + safeItem + "') ";
                }

                DataSet ds = da.GetDataSetInline(sql);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    return true;

                cursor = segEnd;
                remaining -= take;

                if (remaining > 0)
                    cursor = cursor.Date.AddDays(1).Add(shiftStart.TimeOfDay);
            }
        }
        catch (Exception ex)
        {
            SaveLogText("HasSchedulerOverlapOnShiftSegmentsEX-" + ex.Message);
        }

        return false;
    }

    public static DateTime GetEstimatedAssignEndDate(string empid, DateTime startDate, string starttime, string duration)
    {
        // Used only for date range visibility. Actual timeline split is done by GenerateTaskSegments().
        int durationMinutes = AssignDurationToMinutes(duration);
        DateTime shiftStart = DateTime.Today.Add(ParseTimeOfDayInvariant("09:00 AM", "09:00 AM"));
        DateTime shiftEnd = DateTime.Today.Add(ParseTimeOfDayInvariant("06:00 PM", "06:00 PM"));
        try
        {
            string shift = getemployeeshift2(empid);
            if (!string.IsNullOrEmpty(shift))
            {
                string[] a = shift.Split('-');
                if (a.Length >= 3)
                {
                    shiftStart = DateTime.Today.Add(ParseTimeOfDayInvariant(a[0], "09:00 AM"));
                    shiftEnd = DateTime.Today.Add(ParseTimeOfDayInvariant(a[2], "06:00 PM"));
                }
            }
        }
        catch { }

        DateTime start = startDate.Date.Add(ParseTimeOfDayInvariant(starttime, "09:00 AM"));
        DateTime dayShiftStart = startDate.Date.Add(shiftStart.TimeOfDay);
        DateTime dayShiftEnd = startDate.Date.Add(shiftEnd.TimeOfDay);
        if (dayShiftEnd <= dayShiftStart) dayShiftEnd = dayShiftEnd.AddDays(1);
        if (start < dayShiftStart) start = dayShiftStart;

        int shiftMinutes = Math.Max(60, (int)(dayShiftEnd - dayShiftStart).TotalMinutes);
        int todayRemaining = Math.Max(0, (int)(dayShiftEnd - start).TotalMinutes);
        int extraMinutes = Math.Max(0, durationMinutes - todayRemaining);
        int extraDays = extraMinutes <= 0 ? 0 : (int)Math.Ceiling(extraMinutes / (double)shiftMinutes);

        // Add one buffer day because existing planned tasks can reduce today's free time.
        if (durationMinutes > todayRemaining) extraDays += 1;

        return startDate.Date.AddDays(extraDays);
    }
    public static string gettaskname(string type,string milestoneid = "", string taskid = "", string subtaskid="")
    {
        string retname = "";
        DataAccess da = new DataAccess();
        string strsql = "";
        if (type == "milestone") strsql = "select id,Milestone as taskname from Pms_tbl_AddMilestone where id='" + milestoneid + "'";
        else if (type == "task") strsql = "select id,taskname from Pms_tbl_Addtask where id='" + taskid + "'";
        else if (type == "subtask") strsql = "select id,Subtaskname as taskname from Pms_tbl_Addsubtask where id='" + subtaskid + "'";
               
        try
        {
            DataSet dstsk = da.GetDataSetInline(strsql);
            retname = dstsk.Tables[0].Rows[0]["taskname"].ToString();
        }
        catch
        {

        }
        return retname;
    }
    public static string gettaskname2(string type, string id)
    {
        string retname = "";
        DataAccess da = new DataAccess();
        string strsql = "";
        if (type == "milestone") strsql = "select id,Milestone as taskname from Pms_tbl_AddMilestone where id='" + id + "'";
        else if (type == "task") strsql = "select id,taskname from Pms_tbl_Addtask where id='" + id + "'";
        else if (type == "subtask") strsql = "select id,Subtaskname as taskname from Pms_tbl_Addsubtask where id='" + id + "'";

        try
        {
            DataSet dstsk = da.GetDataSetInline(strsql);
            retname = dstsk.Tables[0].Rows[0]["taskname"].ToString();
        }
        catch
        {

        }
        return retname;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string shiftAssignTaskWithExisting(string id, string empid, string utype, string changedate, string starttime)
    {
        utype = NormalizeAssignmentTaskType(utype);
        string retval = "0";
        DataAccess da = new DataAccess();

        if (string.IsNullOrEmpty(id) || id == "0" || string.IsNullOrEmpty(empid))
            return retval;

        string hierarchyValidation = ValidateAssignHierarchyHasSubtask(da, id, utype);
        if (!string.IsNullOrEmpty(hierarchyValidation)) return hierarchyValidation;

        string tblname = GetAssignTableName(utype);
        if (tblname == "") return retval;

        try
        {
            string regid = HttpContext.Current.Request.Cookies["cadmin"]["id"];
            string uid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();
            string safeId = (id ?? "").Replace("'", "''");
            string safeEmp = (empid ?? "").Replace("'", "''");
            string safeType = (utype ?? "").Replace("'", "''").ToLower();
            string safeStart = (starttime ?? "09:00 AM").Replace("'", "''");

            DateTime dropDate = ParseAssignDateOnly(changedate);
            DateTime droppedStart = dropDate.Add(ParseTimeOfDayInvariant(starttime, "09:00 AM"));

            string projectid = "0", milestoneid = "0", taskid = "0", subtaskid = "0", durationText = "01:00";
            string getDropSql = "";
            if (utype == "milestone")
                getDropSql = "select ProjectId,id as MilestoneId,0 as TaskId,0 as SubtaskId,isnull(TaskDuration,'01:00') TaskDuration from Pms_tbl_AddMilestone where id='" + safeId + "' and status=1 and archive=0";
            else if (utype == "task")
                getDropSql = "select ProjectId,Milestoneid,id as TaskId,0 as SubtaskId,isnull(TaskDuration,'01:00') TaskDuration from Pms_tbl_Addtask where id='" + safeId + "' and status=1 and archive=0";
            else
                getDropSql = "select ProjectId,Milestoneid,Task as TaskId,id as SubtaskId,isnull(TaskDuration,'01:00') TaskDuration from Pms_tbl_Addsubtask where id='" + safeId + "' and status=1 and archive=0";

            DataSet dsDrop = da.GetDataSetInline(getDropSql);
            if (dsDrop.Tables[0].Rows.Count == 0) return "Dropped item not found.";

            projectid = dsDrop.Tables[0].Rows[0]["ProjectId"].ToString();
            milestoneid = dsDrop.Tables[0].Rows[0]["MilestoneId"].ToString();
            taskid = dsDrop.Tables[0].Rows[0]["TaskId"].ToString();
            subtaskid = dsDrop.Tables[0].Rows[0]["SubtaskId"].ToString();
            durationText = dsDrop.Tables[0].Rows[0]["TaskDuration"].ToString();

            int droppedMinutes = AssignDurationToMinutes(durationText);
            DateTime droppedEnd = CalculateAssignmentEndDateTime(empid, droppedStart, droppedMinutes);
            DateTime nextStart = droppedEnd;

            // Read current assignment rows before closing old row of dropped item.
            // Center scheduler works from Pms_tbl_assigntasks, so shift must update this table also.
            string exclude = "";
            if (utype == "milestone") exclude = " and not(tasktype='milestone' and milestoneid='" + safeId + "')";
            else if (utype == "task") exclude = " and not(tasktype='task' and taskid='" + safeId + "')";
            else exclude = " and not(tasktype='subtask' and subtaskid='" + safeId + "')";

            string sqlExisting = @"
select id as assignid,tasktype,milestoneid,taskid,subtaskid,ownerid,startdatetime,enddatetime,isnull(taskduration,'01:00') taskduration
,'' as taskname from Pms_tbl_assigntasks with(nolock)
where regid='" + regid + @"' and status=1 and archive=0 and ownerid='" + safeEmp + @"'
  and startdatetime >= convert(datetime2,'" + ToSqlDateTime(droppedStart) + @"',126)
  and startdatetime < dateadd(day,1,convert(date,'" + ToSqlDateOnly(dropDate) + @"',112))" + exclude + @"
order by startdatetime,id";

            DataSet dsExisting = da.GetDataSetInline(sqlExisting);

            // Close old active assignment row for dropped item and insert the dropped item at selected time.
            string closeOld = "update Pms_tbl_assigntasks set status=0, archive=1, removedatetime=getdate() " +
                              "where regid='" + regid + "' and status=1 and archive=0 and tasktype='" + safeType + "' ";
            if (utype == "milestone") closeOld += " and milestoneid='" + safeId + "'";
            else if (utype == "task") closeOld += " and taskid='" + safeId + "'";
            else closeOld += " and subtaskid='" + safeId + "'";
            da.executeDMLQueryinline(closeOld);

            string dur = AssignMinutesToDuration(droppedMinutes).Replace("'", "''");
            string ins = "insert into Pms_tbl_assigntasks(uid,regid,cdate,status,archive,projectid,milestoneid,taskid,tasktype,subtaskid,ownerid,startdatetime,enddatetime,taskduration,assigndatetime) values(" +
                         "'" + uid + "','" + regid + "',getdate(),1,0,'" + projectid + "','" + milestoneid + "','" + taskid + "','" + safeType + "','" + subtaskid + "','" + safeEmp + "','" + ToSqlDateTime(droppedStart) + "','" + ToSqlDateTime(droppedEnd) + "','" + dur + "',getdate())";
            da.executeDMLQueryinline(ins);

            // Keep old detail date/time in sync, but do NOT overwrite original Owner.
            string updDroppedBase = "update " + tblname + " set StartDate='" + ToSqlDateOnly(dropDate) + "', StartTime='" + safeStart + "' ";
            if (utype == "milestone") updDroppedBase += ", EndDate='" + ToSqlDateOnly(droppedEnd) + "' ";
            else updDroppedBase += ", Targetdate='" + ToSqlDateOnly(droppedEnd) + "' ";
            updDroppedBase += " where id='" + safeId + "' and status=1 and archive=0";
            da.executeDMLQueryinline(updDroppedBase);

            foreach (DataRow r in dsExisting.Tables[0].Rows)
            {
                string assignid = r["assignid"].ToString();
                string rt = r["tasktype"].ToString();
                string rdur = r["taskduration"].ToString();
                int minutes = AssignDurationToMinutes(rdur);
                string tskname = gettaskname(rt, milestoneid, taskid, subtaskid);

                DateTime oldStart = Convert.ToDateTime(r["startdatetime"]);
                if (oldStart > nextStart) nextStart = oldStart;

                DateTime newStart = nextStart;
                DateTime newEnd = CalculateAssignmentEndDateTime(empid, newStart, minutes);
                //tname="+ tskname + ",
                string updAssign = "update Pms_tbl_assigntasks set  startdatetime='" + ToSqlDateTime(newStart) + "', enddatetime='" + ToSqlDateTime(newEnd) + "' where id='" + assignid + "' and regid='" + regid + "'";
                da.executeDMLQueryinline(updAssign);

                string baseTable = GetAssignTableName(rt);
                string baseId = rt == "milestone" ? r["milestoneid"].ToString() : (rt == "task" ? r["taskid"].ToString() : r["subtaskid"].ToString());
                if (baseTable != "" && baseId != "" && baseId != "0")
                {
                    string updBase = "update " + baseTable + " set StartDate='" + ToSqlDateOnly(newStart) + "', StartTime='" + newStart.ToString("hh:mm tt") + "' ";
                    if (rt == "milestone") updBase += ", EndDate='" + ToSqlDateOnly(newEnd) + "' ";
                    else updBase += ", Targetdate='" + ToSqlDateOnly(newEnd) + "' ";
                    updBase += " where id='" + baseId.Replace("'", "''") + "' and status=1 and archive=0";
                    da.executeDMLQueryinline(updBase);
                }

                nextStart = newEnd;
            }

            retval = "1";
        }
        catch (Exception ex)
        {
            SaveLogText("shiftAssignTaskWithExistingEX-" + ex.Message);
            retval = ex.Message;
        }

        return retval;
    }


    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string updateAssignOwnerTime(string assignid, string empid, string changedate, string starttime)
    {
        string retval = "0";
        DataAccess da1 = new DataAccess();
        try
        {
            if (string.IsNullOrEmpty(assignid) || assignid == "0" || string.IsNullOrEmpty(empid) || empid == "0")
                return retval;

            string regid = HttpContext.Current.Request.Cookies["cadmin"]["id"];
            string safeAssignId = (assignid ?? "").Replace("'", "''");
            string safeEmpId = (empid ?? "").Replace("'", "''");
            string safeStartTime = (starttime ?? "").Replace("'", "''");
            if (string.IsNullOrEmpty(safeStartTime)) return retval;

            string sel = "select top(1) id, tasktype, milestoneid, taskid, subtaskid, taskduration, startdatetime " +
                         "from Pms_tbl_assigntasks where id='" + safeAssignId + "' and regid='" + regid + "' and status=1 and archive=0";
            DataSet ds = da1.GetDataSetInline(sel);
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return retval;

            DataRow r = ds.Tables[0].Rows[0];
            string duration = r["taskduration"].ToString();
            if (string.IsNullOrEmpty(duration)) duration = "01:00";

            DateTime datePart;
            try { datePart = ParseAssignDateOnly(changedate); }
            catch { datePart = Convert.ToDateTime(r["startdatetime"]).Date; }

            DateTime newStart = datePart.Add(ParseTimeOfDayInvariant(safeStartTime, "09:00 AM"));
            int minutes = AssignDurationToMinutes(duration);
            if (HasSchedulerOverlapOnShiftSegments(da1, safeEmpId, newStart, minutes, safeAssignId, "", ""))
            {
                return "Drop only in available area";
            }
            DateTime newEnd = CalculateAssignmentEndDateTime(safeEmpId, newStart, minutes);

            string upd = "update Pms_tbl_assigntasks set ownerid='" + safeEmpId + "', " +
                         "startdatetime='" + ToSqlDateTime(newStart) + "', " +
                         "enddatetime='" + ToSqlDateTime(newEnd) + "' " +
                         "where id='" + safeAssignId + "' and regid='" + regid + "' and status=1 and archive=0";
            SaveLogText("updateAssignOwnerTime_ASSIGN-" + upd);
            da1.executeDMLQueryinline(upd);

            // Keep old hierarchy date/time fields in sync, but do NOT overwrite original Owner.
            string realType = r["tasktype"].ToString().ToLower();
            string tblname = GetAssignTableName(realType);
            string realId = "0";
            if (realType == "milestone") realId = r["milestoneid"].ToString();
            else if (realType == "task") realId = r["taskid"].ToString();
            else if (realType == "subtask") realId = r["subtaskid"].ToString();

            if (!string.IsNullOrEmpty(tblname) && realId != "0")
            {
                string updOld = "update " + tblname + " set StartDate='" + ToSqlDateOnly(newStart) + "', StartTime='" + newStart.ToString("hh:mm tt") + "' ";
                if (realType == "milestone") updOld += ", EndDate='" + ToSqlDateOnly(newEnd) + "' ";
                else updOld += ", Targetdate='" + ToSqlDateOnly(newEnd) + "' ";
                updOld += "where id='" + realId.Replace("'", "''") + "' and status=1 and archive=0";
                SaveLogText("updateAssignOwnerTime_OLD-" + updOld);
                da1.executeDMLQueryinline(updOld);
            }

            retval = "1";
        }
        catch (Exception ex)
        {
            SaveLogText("updateAssignOwnerTimeEX-" + ex.Message);
            retval = ex.Message;
        }
        return retval;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string updateAssignTaskTime(string id, string utype, string starttime, string taskduration, string changedate, string oldstarttime, string oldchangedate, string editmode)
    {
        utype = NormalizeAssignmentTaskType(utype);
        string retval = "0";
        DataAccess da1 = new DataAccess();

        try
        {
            if (string.IsNullOrEmpty(id) || id == "0")
                return retval;

            string regid = HttpContext.Current.Request.Cookies["cadmin"]["id"];
            string safeId = (id ?? "").Replace("'", "''");
            string safeType = (utype ?? "").Replace("'", "''").ToLower();
            string safeStartTime = (starttime ?? "").Replace("'", "''");
            int requestedDurationMinutes = AssignDurationToMinutes(taskduration);
            string safeDuration = AssignMinutesToDuration(requestedDurationMinutes).Replace("'", "''");
            string safeEditMode = (editmode ?? "").Replace("'", "''").ToLower();
            bool isResizeEdit = (safeEditMode == "resize-left" || safeEditMode == "resize-right");

            if (string.IsNullOrEmpty(safeStartTime))
                return retval;

            // First update scheduler assignment table. Center panel is managed from Pms_tbl_assigntasks.
            string chkAssign = "select top(1) id, uid, regid, cdate, projectid, milestoneid, taskid, tasktype, subtaskid, ownerid, startdatetime, enddatetime, taskduration, assigndatetime " +
                               "from Pms_tbl_assigntasks where id='" + safeId + "' and regid='" + regid + "' and status=1 and archive=0";
            DataSet dsAssign = da1.GetDataSetInline(chkAssign);

            if (dsAssign.Tables.Count > 0 && dsAssign.Tables[0].Rows.Count > 0)
            {
                DataRow ar = dsAssign.Tables[0].Rows[0];
                DateTime oldStart = Convert.ToDateTime(ar["startdatetime"]);
                DateTime datePart = oldStart.Date;

                // Resize/move may happen on a split segment visible on next date.
                // Therefore JS sends changedate from segment.workDate/selected date.
                if (!string.IsNullOrEmpty(changedate))
                {
                    try { datePart = ParseAssignDateOnly(changedate); }
                    catch { datePart = oldStart.Date; }
                }

                DateTime newStart = isResizeEdit ? oldStart : datePart.Add(ParseTimeOfDayInvariant(safeStartTime, "09:00 AM"));
                int minutes = AssignDurationToMinutes(safeDuration);

                // If one DB assignment is displayed as multiple day-wise segments,
                // resizing any visible day must update that day's segment only.
                // Example: Day-1 = 02:00, Day-2 = 03:00. Resize Day-1 should not save 05:00/total.
                if (!isResizeEdit)
                try
                {
                    string empidForSplit = ar["ownerid"].ToString();
                    DateTime oldSegDate = datePart;
                    if (!string.IsNullOrEmpty(oldchangedate))
                    {
                        try { oldSegDate = ParseAssignDateOnly(oldchangedate); } catch { oldSegDate = datePart; }
                    }
                    string oldSegStart = !string.IsNullOrEmpty(oldstarttime) ? oldstarttime : safeStartTime;

                    string empShift = getemployeeshift2(empidForSplit);
                    string splitShiftStart = "09:00 AM";
                    string splitShiftEnd = "06:00 PM";
                    if (!string.IsNullOrEmpty(empShift))
                    {
                        string[] sp = empShift.Split('-');
                        if (sp.Length >= 3)
                        {
                            splitShiftStart = sp[0];
                            splitShiftEnd = sp[2];
                        }
                    }

                    List<assignsubtasklist> allEmpRows = getAssignmentRowsFromAssignTable(empidForSplit, oldSegDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
                    List<taskSegment> allSegments = GenerateTaskSegments(allEmpRows, splitShiftStart, splitShiftEnd);
                    List<taskSegment> sameAssignSegments = allSegments
                        .Where(x => x.assignid.HasValue && x.assignid.Value.ToString() == safeId)
                        .OrderBy(x => x.workDate)
                        .ThenBy(x => ParseTimeOfDayInvariant(x.StartTime, "09:00 AM"))
                        .ToList();

                    if (sameAssignSegments.Count > 1)
                    {
                        string archiveSql = "update Pms_tbl_assigntasks set status=0, archive=1, removedatetime=getdate() where id='" + safeId + "' and regid='" + regid + "' and status=1 and archive=0";
                        da1.executeDMLQueryinline(archiveSql);

                        foreach (taskSegment sg in sameAssignSegments)
                        {
                            bool isEditedSegment = false;
                            try
                            {
                                isEditedSegment = sg.workDate.HasValue
                                    && sg.workDate.Value.Date == oldSegDate.Date
                                    && string.Equals((sg.StartTime ?? "").Trim(), (oldSegStart ?? "").Trim(), StringComparison.OrdinalIgnoreCase);
                            }
                            catch { isEditedSegment = false; }

                            DateTime segDate = isEditedSegment ? datePart : (sg.workDate.HasValue ? sg.workDate.Value.Date : oldSegDate.Date);
                            string segStartText = isEditedSegment ? safeStartTime : sg.StartTime;
                            string segDurText = isEditedSegment ? safeDuration : AssignMinutesToDuration(sg.DurationMinutes);
                            int segMinutes = AssignDurationToMinutes(segDurText);
                            DateTime segStartDt = segDate.Add(ParseTimeOfDayInvariant(segStartText, "09:00 AM"));
                            DateTime segEndDt = segStartDt.AddMinutes(segMinutes);

                            string insAssign = "insert into Pms_tbl_assigntasks(uid,regid,cdate,status,archive,projectid,milestoneid,taskid,tasktype,subtaskid,ownerid,startdatetime,enddatetime,taskduration,assigndatetime) values(" +
                                "'" + HttpContext.Current.Request.Cookies["user"]["user_id"] + "'," +
                                "'" + regid + "',getdate(),1,0," +
                                "'" + ar["projectid"].ToString().Replace("'", "''") + "'," +
                                "'" + ar["milestoneid"].ToString().Replace("'", "''") + "'," +
                                "'" + ar["taskid"].ToString().Replace("'", "''") + "'," +
                                "'" + ar["tasktype"].ToString().Replace("'", "''") + "'," +
                                "'" + ar["subtaskid"].ToString().Replace("'", "''") + "'," +
                                "'" + empidForSplit.Replace("'", "''") + "'," +
                                "'" + ToSqlDateTime(segStartDt) + "'," +
                                "'" + ToSqlDateTime(segEndDt) + "'," +
                                "'" + segDurText.Replace("'", "''") + "'," +
                                "'" + ToSqlDateTime(ar["assigndatetime"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(ar["assigndatetime"])) + "')";
                            da1.executeDMLQueryinline(insAssign);
                        }

                        SaveLogText("updateAssignTaskTime_SPLIT_DAILY-assignid=" + safeId);
                        return "1";
                    }
                }
                catch (Exception splitEx)
                {
                    SaveLogText("updateAssignTaskTime_SPLIT_DAILY_EX-" + splitEx.Message);
                }

                DateTime newEnd = CalculateAssignmentEndDateTime(ar["ownerid"].ToString(), newStart, minutes);

                string updAssign = "update Pms_tbl_assigntasks set " +
                                   "startdatetime='" + ToSqlDateTime(newStart) + "', " +
                                   "enddatetime='" + ToSqlDateTime(newEnd) + "', " +
                                   "taskduration='" + safeDuration + "' " +
                                   "where id='" + safeId + "' and regid='" + regid + "' and status=1 and archive=0";
                SaveLogText("updateAssignTaskTime_ASSIGN-" + updAssign);
                da1.executeDMLQueryinline(updAssign);

                // Optional sync old hierarchy table using real task/milestone/subtask id from assignment row.
                string realType = ar["tasktype"].ToString().ToLower();
                string tblname = GetAssignTableName(realType);
                string realId = "0";
                if (realType == "milestone") realId = ar["milestoneid"].ToString();
                else if (realType == "task") realId = ar["taskid"].ToString();
                else if (realType == "subtask") realId = ar["subtaskid"].ToString();

                if (!string.IsNullOrEmpty(tblname) && realId != "0")
                {
                    string hierarchyStartTime = isResizeEdit ? Convert.ToDateTime(ar["startdatetime"]).ToString("hh:mm tt") : safeStartTime;
                    string updOld = "update " + tblname + " set StartTime='" + hierarchyStartTime.Replace("'", "''") + "', TaskDuration='" + safeDuration + "' " +
                                    "where id='" + realId.Replace("'", "''") + "' and status=1 and archive=0";
                    SaveLogText("updateAssignTaskTime_OLD-" + updOld);
                    da1.executeDMLQueryinline(updOld);
                }

                return "1";
            }

            // Fallback: old flow if caller still sends actual milestone/task/subtask id.
            string fallbackTable = GetAssignTableName(safeType);
            if (string.IsNullOrEmpty(fallbackTable))
                return retval;

            string strsql = "update " + fallbackTable + " set StartTime='" + safeStartTime + "', TaskDuration='" + safeDuration + "' where id='" + safeId + "' and status=1 and archive=0";
            SaveLogText("updateAssignTaskTime_FALLBACK-" + strsql);
            da1.executeDMLQueryinline(strsql);
            retval = "1";
        }
        catch (Exception ex)
        {
            SaveLogText("updateAssignTaskTimeEX-" + ex.Message);
        }

        return retval;
    }


    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string removeOwner_assign(string id, string utype)
    {
        utype = NormalizeAssignmentTaskType(utype);
        string retval = "0";
        DataAccess da1 = new DataAccess();

        try
        {
            if (string.IsNullOrEmpty(id) || id == "0")
                return retval;

            string regid = HttpContext.Current.Request.Cookies["cadmin"]["id"];

            // IMPORTANT:
            // Scheduler center panel is now managed from Pms_tbl_assigntasks.
            // Remove should archive the active assignment row, not only clear owner in old PMS tables.
            string safeId = id.Replace("'", "''");
            string safeType = (utype ?? "").Replace("'", "''");

            // First try direct assignment-row remove. The center bar carries assignid.
            string directCheckSql = "select top(1) id from Pms_tbl_assigntasks where regid='" + regid + "' and status=1 and archive=0 and id='" + safeId + "'";
            DataSet dsDirect = da1.GetDataSetInline(directCheckSql);
            if (dsDirect.Tables[0].Rows.Count > 0)
            {
                string directSql = "update Pms_tbl_assigntasks " +
                                   "set status=0, archive=1, removedatetime=getdate() " +
                                   "where regid='" + regid + "' and status=1 and archive=0 and id='" + safeId + "'";
                da1.executeDMLQueryinline(directSql);
                return "1";
            }

            string strsql = "update Pms_tbl_assigntasks " +
                            "set status=0, archive=1, removedatetime=getdate() " +
                            "where regid='" + regid + "' and status=1 and archive=0 and tasktype='" + safeType + "' ";

            if (utype == "milestone")
            {
                strsql += " and milestoneid='" + safeId + "'";
            }
            else if (utype == "task")
            {
                strsql += " and taskid='" + safeId + "'";
            }
            else if (utype == "subtask")
            {
                strsql += " and subtaskid='" + safeId + "'";
            }
            else
            {
                return retval;
            }

            da1.executeDMLQueryinline(strsql);

            // Do not clear original owner/start time from old hierarchy tables on scheduler remove.
            // Removing from center only archives Pms_tbl_assigntasks.

            retval = "1";
        }
        catch (Exception ex)
        {
            SaveLogText("removeOwner_assign-" + ex.Message);
        }

        return retval;
    }
    
    private static bool IsProjectExcludedCalendarDate(
        DateTime checkDate,
        string empid,
        string isweekoff,
        string isholyday)
    {
        bool excluded = false;

        if (isweekoff == "1" && getweeklyoff(checkDate, empid))
            excluded = true;

        if (isholyday == "1" && getholiday(checkDate, empid))
            excluded = true;

        return excluded;
    }

    public static string manage_enddatenew(string ndays, string startdate,string pid)
    {
        string retval = "";

        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        //startdate = cmf.dateConvert(startdate.ToString());
        string isweekoff = "0";
        string isholyday = "0";
        string projectid = pid;
        if (!string.IsNullOrWhiteSpace(projectid) && projectid != "0")
        {
            string weekoffholyday = check_weekoffholyday(projectid);
            SaveLogText("weekoffholyday5-" + weekoffholyday);
            if (weekoffholyday!=string.Empty)
            {
                string[] aroff = weekoffholyday.Split(',');
                try
                {
                    isholyday = aroff[0];
                    isweekoff = aroff[1];
                }
                catch { }
            }
        }
        //leave leaveweek = new leave();
        string empid = "0";
        string strsqltm = "";
        try
        {
            DateTime startDateValue;
            if (!TryParseDateOnlyInvariant(startdate, out startDateValue))
                return retval;

            int newday = 0;
            int addon = 0;
            if (ndays != string.Empty && ndays != "0")
            {
                if (!string.IsNullOrWhiteSpace(ndays))
                {
                    double dayValue;
                    if (!Double.TryParse(ndays, NumberStyles.Any, CultureInfo.InvariantCulture, out dayValue) &&
                        !Double.TryParse(ndays, NumberStyles.Any, CultureInfo.CurrentCulture, out dayValue))
                        dayValue = 0;
                    newday = Convert.ToInt32(Math.Ceiling(dayValue));
                }

                DateTime checkDate = startDateValue;
                for (int i = 0; i < newday; i++)
                {
                    addon = 0;
                    if (i > 0) checkDate = checkDate.AddDays(1);

                    if (IsProjectExcludedCalendarDate(
                        checkDate, empid, isweekoff, isholyday))
                    {
                        addon = 1;
                    }
                    if (addon == 1)
                    {
                        newday += 1;
                    }
                }
                retval = startDateValue.AddDays(Math.Max(0, newday - 1))
                    .ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            }
            else
            {
                retval = startDateValue.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
        }
        catch(Exception ex)
        {
            SaveLogText("strAssignto3-" + ex.Message);
        }
        return retval;
    }
    #endregion
    #region update owner

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string updateOwner(string id, string empid, string utype)
    {
        string retval = "0";
        DataAccess da1 = new DataAccess();
        if(id!=string.Empty&&id!="0")
        {
            string tblname = "";
            if (utype == "milestone")
            {
            tblname = "Pms_tbl_AddMilestone";
            }
            else if (utype=="task")
            {
                tblname = "Pms_tbl_Addtask";
            }
           else if (utype == "subtask")
            {
                tblname = "Pms_tbl_Addsubtask";
            }
            if (tblname != string.Empty)
            {
                try
                {
                    string strownerupdate = "update " + tblname + " set owner='" + empid + "' where id='" + id + "' and status=1 and archive=0 ";
                    if (utype == "subtask"&& empid!=string.Empty && empid !="0")
                    {
                        string updatedetails = "update Pms_tbl_Timerstatus set uid="+ empid + " where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and subtaskid="+ id + " and type='subtask'";
                    }
                    else if (utype == "task" && empid != string.Empty && empid != "0")
                    {
                        string updatedetails = "update Pms_tbl_Timerstatus set uid=" + empid + " where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and taskid=" + id + " and type='task'";
                    }
                    else if (utype == "milestone" && empid != string.Empty && empid != "0")
                    {
                        string updatedetails = "update Pms_tbl_Timerstatus set uid=" + empid + " where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and milestonid=" + id + " and type='milestone'";
                    }
                    SaveLogText("strownerupdate-"+ strownerupdate);
                    da1.executeDMLQueryinline(strownerupdate);
                    retval = "1";
                }
                catch { }
            }
          
        }
        return retval;
    }


    // Kanban popup owner save. This method intentionally accepts one employee only.
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string UpdateKanbanOwner(string id, string empid, string utype)
    {
        try
        {
            int recordId;
            int ownerId;
            int regId;
            if (!Int32.TryParse(id, out recordId) || recordId <= 0)
                return "Invalid record.";
            if (!Int32.TryParse(empid, out ownerId) || ownerId <= 0)
                return "Please select one owner.";

            HttpCookie regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null || !Int32.TryParse(regCookie["id"], out regId))
                return "User is not authenticated.";

            string type = (utype ?? "").Trim().ToLowerInvariant();
            string tableName = "";
            string moduleId = "";
            string timerWhere = "";
            if (type == "milestone")
            {
                tableName = "Pms_tbl_AddMilestone";
                moduleId = "54";
                timerWhere = "milestonid=" + recordId + " and type='milestone'";
            }
            else if (type == "task")
            {
                tableName = "Pms_tbl_AddTask";
                moduleId = "55";
                timerWhere = "taskid=" + recordId + " and type='task'";
            }
            else if (type == "subtask")
            {
                tableName = "Pms_tbl_AddSubTask";
                moduleId = "56";
                timerWhere = "subtaskid=" + recordId + " and type='subtask'";
            }
            else
            {
                return "Invalid item type.";
            }

            if (!HasPmsEditPermission(moduleId))
                return "You do not have edit permission.";

            DataAccess da = new DataAccess();
            string employeeSql = "select id from tbl_employee with(nolock) where id=" + ownerId +
                                 " and regid=" + regId + " and status=1 and archive=0";
            DataSet employeeDs = da.GetDataSetInline(employeeSql);
            if (employeeDs.Tables.Count == 0 || employeeDs.Tables[0].Rows.Count == 0)
                return "Selected owner is not active.";

            string itemSql = "select id from " + tableName + " with(nolock) where id=" + recordId +
                             " and regid=" + regId + " and status=1 and archive=0";
            DataSet itemDs = da.GetDataSetInline(itemSql);
            if (itemDs.Tables.Count == 0 || itemDs.Tables[0].Rows.Count == 0)
                return "Record not found.";

            string updateSql = "update " + tableName + " set Owner='" + ownerId + "' where id=" + recordId +
                               " and regid=" + regId + " and status=1 and archive=0;" +
                               " update Pms_tbl_Timerstatus set uid=" + ownerId +
                               " where regid=" + regId + " and status=1 and archive=0 and " + timerWhere + ";";
            da.executeDMLQueryinline(updateSql);
            return "1";
        }
        catch (Exception ex)
        {
            SaveLogText("UpdateKanbanOwner-" + ex.Message);
            return "Owner update failed.";
        }
    }

    private static decimal ParsePerDayHoursInvariant(string value)
    {
        decimal result;
        if (!Decimal.TryParse(
                Convert.ToString(value, CultureInfo.InvariantCulture),
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out result) || result <= 0)
        {
            result = 8M;
        }
        return result;
    }

    private static int GetPerDayCapacityMinutes(decimal validationDays, string perdayhrs)
    {
        decimal perDayHours = ParsePerDayHoursInvariant(perdayhrs);
        return Convert.ToInt32(
            Math.Round(
                (validationDays <= 0 ? 1M : validationDays) * perDayHours * 60M,
                0,
                MidpointRounding.AwayFromZero
            )
        );
    }

    private static string FormatPerDayHoursInvariant(string perdayhrs)
    {
        return ParsePerDayHoursInvariant(perdayhrs)
            .ToString("0.##", CultureInfo.InvariantCulture);
    }

    private static string FormatDurationMinutesInvariant(int totalMinutes)
    {
        totalMinutes = Math.Max(0, totalMinutes);
        return (totalMinutes / 60).ToString(CultureInfo.InvariantCulture) + ":" +
               (totalMinutes % 60).ToString("00", CultureInfo.InvariantCulture);
    }

    // Kanban popup duration save. Uses the same hours/minutes and parent-duration rules as the page.
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string UpdateKanbanDuration(string id, string hours, string minutes, string utype, string perdayhrs)
    {
        try
        {
            int recordId;
            int durationHours;
            int durationMinutes;
            int regId;
            if (!Int32.TryParse(id, out recordId) || recordId <= 0)
                return "Invalid record.";
            if (!Int32.TryParse(hours, out durationHours) || durationHours < 0)
                return "Invalid hours.";
            if (!Int32.TryParse(minutes, out durationMinutes) || durationMinutes < 0 || durationMinutes >= 60)
                return "Minutes must be less than 60.";

            int totalMinutes = (durationHours * 60) + durationMinutes;
            if (totalMinutes <= 0)
                return "Duration must be greater than zero.";

            HttpCookie regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null || !Int32.TryParse(regCookie["id"], out regId))
                return "User is not authenticated.";

            string type = (utype ?? "").Trim().ToLowerInvariant();
            string tableName = "";
            string moduleId = "";
            string endColumn = "";
            string parentColumn = "";
            if (type == "milestone")
            {
                tableName = "Pms_tbl_AddMilestone";
                moduleId = "54";
                endColumn = "EndDate";
                parentColumn = "ProjectId";
            }
            else if (type == "task")
            {
                tableName = "Pms_tbl_AddTask";
                moduleId = "55";
                endColumn = "TargetDate";
                parentColumn = "Milestoneid";
            }
            else if (type == "subtask")
            {
                tableName = "Pms_tbl_AddSubTask";
                moduleId = "56";
                endColumn = "TargetDate";
                parentColumn = "task";
            }
            else
            {
                return "Invalid item type.";
            }

            if (!HasPmsEditPermission(moduleId))
                return "You do not have edit permission.";

            DataAccess da = new DataAccess();
            string itemSql = "select id,StartDate," + endColumn + " as EndDate," + parentColumn +
                             " as ParentId,txtdays from " + tableName + " with(nolock) where id=" + recordId +
                             " and regid=" + regId + " and status=1 and archive=0";
            DataSet itemDs = da.GetDataSetInline(itemSql);
            if (itemDs.Tables.Count == 0 || itemDs.Tables[0].Rows.Count == 0)
                return "Record not found.";

            string durationValue = durationHours + ":" + durationMinutes.ToString("00");
            string durationValidation = ValidateTaskDurationDateCapacity(
                da,
                tableName,
                endColumn,
                recordId.ToString(CultureInfo.InvariantCulture),
                regId.ToString(CultureInfo.InvariantCulture),
                durationValue,
                perdayhrs
            );
            if (durationValidation != "1")
                return durationValidation;

            string updateSql = "update " + tableName + " set TaskDuration='" + durationValue + "' where id=" +
                               recordId + " and regid=" + regId + " and status=1 and archive=0";
            da.executeDMLQueryinline(updateSql);
            return "1";
        }
        catch (Exception ex)
        {
            SaveLogText("UpdateKanbanDuration-" + ex.Message);
            return "Duration update failed.";
        }
    }

    #endregion
    #region update custom_col
    private static bool IsSafeSqlColumnName(string columnName)
    {
        if (String.IsNullOrWhiteSpace(columnName)) return false;
        return columnName.All(ch => Char.IsLetterOrDigit(ch) || ch == '_');
    }

    private static bool IsDateOnlyColumn(string columnName)
    {
        string name = (columnName ?? String.Empty).Trim().ToLowerInvariant();
        return name == "startdate" || name == "enddate" || name == "targetdate" ||
               name == "awarddate" || name == "iscompletedate";
    }

    private static string GetCustomColumnSqlValue(string columnName, string columnValue)
    {
        if (IsDateOnlyColumn(columnName))
        {
            if (String.IsNullOrWhiteSpace(columnValue)) return "NULL";
            DateTime parsedDate;
            if (!TryParseDateOnlyInvariant(columnValue, out parsedDate))
                throw new FormatException("Invalid date value for " + columnName + ".");
            return "CONVERT(date,'" + ToSqlDateOnly(parsedDate) + "',112)";
        }

        if (String.Equals((columnName ?? String.Empty).Trim(), "TaskDuration", StringComparison.OrdinalIgnoreCase))
        {
            string normalizedDuration = NormalizeDurationTextInvariant(columnValue, String.Empty);
            return "N'" + normalizedDuration.Replace("'", "''") + "'";
        }

        return "N'" + (columnValue ?? String.Empty).Replace("'", "''") + "'";
    }

    private static string ValidateDurationHierarchyTotals(
        DataAccess da,
        string itemType,
        int recordId,
        int regId,
        int candidateMinutes)
    {
        string type = (itemType ?? String.Empty).Trim().ToLowerInvariant();

        if (type == "milestone")
        {
            DataSet taskDs = da.GetDataSetInline(
                "select TaskDuration from Pms_tbl_AddTask with(nolock) where Milestoneid=" + recordId +
                " and regid=" + regId + " and status=1 and archive=0"
            );
            int totalTaskMinutes = 0;
            if (taskDs.Tables.Count > 0)
            {
                foreach (DataRow row in taskDs.Tables[0].Rows)
                    totalTaskMinutes += GetDurationMinutes(row["TaskDuration"].ToString());
            }
            if (totalTaskMinutes > candidateMinutes)
                return "Total task duration cannot be greater than milestone duration.";

            return "1";
        }

        if (type == "task")
        {
            // A task cannot be reduced below the total duration of its own subtasks.
            DataSet ownSubtaskDs = da.GetDataSetInline(
                "select TaskDuration from Pms_tbl_AddSubTask with(nolock) where task=" + recordId +
                " and regid=" + regId + " and status=1 and archive=0"
            );
            int ownSubtaskMinutes = 0;
            if (ownSubtaskDs.Tables.Count > 0)
            {
                foreach (DataRow row in ownSubtaskDs.Tables[0].Rows)
                    ownSubtaskMinutes += GetDurationMinutes(row["TaskDuration"].ToString());
            }
            if (ownSubtaskMinutes > candidateMinutes)
                return "Total subtask duration cannot be greater than task duration.";

            DataSet itemDs = da.GetDataSetInline(
                "select Milestoneid from Pms_tbl_AddTask with(nolock) where id=" + recordId +
                " and regid=" + regId + " and status=1 and archive=0"
            );
            if (itemDs.Tables.Count == 0 || itemDs.Tables[0].Rows.Count == 0)
                return "Record not found.";

            int milestoneId = ToNullableIntInvariant(itemDs.Tables[0].Rows[0]["Milestoneid"]) ?? 0;
            if (milestoneId <= 0) return "1";

            DataSet parentDs = da.GetDataSetInline(
                "select TaskDuration from Pms_tbl_AddMilestone with(nolock) where id=" + milestoneId +
                " and regid=" + regId + " and status=1 and archive=0"
            );
            if (parentDs.Tables.Count == 0 || parentDs.Tables[0].Rows.Count == 0)
                return "1";

            int parentMinutes = GetDurationMinutes(parentDs.Tables[0].Rows[0]["TaskDuration"].ToString());
            if (parentMinutes <= 0) return "1";

            DataSet siblingDs = da.GetDataSetInline(
                "select id,TaskDuration from Pms_tbl_AddTask with(nolock) where Milestoneid=" + milestoneId +
                " and regid=" + regId + " and status=1 and archive=0"
            );
            int totalTaskMinutes = 0;
            if (siblingDs.Tables.Count > 0)
            {
                foreach (DataRow row in siblingDs.Tables[0].Rows)
                {
                    int childId = ToNullableIntInvariant(row["id"]) ?? 0;
                    totalTaskMinutes += childId == recordId
                        ? candidateMinutes
                        : GetDurationMinutes(row["TaskDuration"].ToString());
                }
            }
            if (totalTaskMinutes > parentMinutes)
                return "Total task duration cannot be greater than milestone duration.";

            return "1";
        }

        if (type == "subtask")
        {
            DataSet itemDs = da.GetDataSetInline(
                "select task from Pms_tbl_AddSubTask with(nolock) where id=" + recordId +
                " and regid=" + regId + " and status=1 and archive=0"
            );
            if (itemDs.Tables.Count == 0 || itemDs.Tables[0].Rows.Count == 0)
                return "Record not found.";

            int taskId = ToNullableIntInvariant(itemDs.Tables[0].Rows[0]["task"]) ?? 0;
            if (taskId <= 0) return "1";

            DataSet parentDs = da.GetDataSetInline(
                "select TaskDuration from Pms_tbl_AddTask with(nolock) where id=" + taskId +
                " and regid=" + regId + " and status=1 and archive=0"
            );
            if (parentDs.Tables.Count == 0 || parentDs.Tables[0].Rows.Count == 0)
                return "1";

            int parentMinutes = GetDurationMinutes(parentDs.Tables[0].Rows[0]["TaskDuration"].ToString());
            if (parentMinutes <= 0) return "1";

            DataSet siblingDs = da.GetDataSetInline(
                "select id,TaskDuration from Pms_tbl_AddSubTask with(nolock) where task=" + taskId +
                " and regid=" + regId + " and status=1 and archive=0"
            );
            int totalSubtaskMinutes = 0;
            if (siblingDs.Tables.Count > 0)
            {
                foreach (DataRow row in siblingDs.Tables[0].Rows)
                {
                    int childId = ToNullableIntInvariant(row["id"]) ?? 0;
                    totalSubtaskMinutes += childId == recordId
                        ? candidateMinutes
                        : GetDurationMinutes(row["TaskDuration"].ToString());
                }
            }
            if (totalSubtaskMinutes > parentMinutes)
                return "Total subtask duration cannot be greater than task duration.";

            return "1";
        }

        return "1";
    }

    private static string ValidateTaskDurationDateCapacity(
        DataAccess da,
        string tableName,
        string endColumn,
        string id,
        string regid,
        string durationValue,
        string perdayhrs)
    {
        int recordId;
        int safeRegId;
        if (!Int32.TryParse(id, out recordId) || recordId <= 0 ||
            !Int32.TryParse(regid, out safeRegId) || safeRegId <= 0)
            return "Invalid record.";

        // A normal field edit may intentionally clear Working Hrs. Store the
        // blank value without date-capacity or hierarchy validation. Reset Start
        // Date still performs the complete validation before any cascade save.
        if (String.IsNullOrWhiteSpace(durationValue))
            return "1";

        string type = "";
        string nameColumn = "";
        if (String.Equals(tableName, "Pms_tbl_AddMilestone", StringComparison.OrdinalIgnoreCase))
        {
            type = "milestone";
            nameColumn = "Milestone";
        }
        else if (String.Equals(tableName, "Pms_tbl_AddTask", StringComparison.OrdinalIgnoreCase))
        {
            type = "task";
            nameColumn = "taskname";
        }
        else if (String.Equals(tableName, "Pms_tbl_AddSubTask", StringComparison.OrdinalIgnoreCase))
        {
            type = "subtask";
            nameColumn = "Subtaskname";
        }

        if (String.IsNullOrEmpty(type) || String.IsNullOrEmpty(nameColumn))
            return "Invalid item type.";

        string itemSql = "select txtdays," + nameColumn + " as ItemName from " + tableName +
                         " with(nolock) where id=" + recordId + " and regid=" + safeRegId +
                         " and status=1 and archive=0";
        DataSet itemDs = da.GetDataSetInline(itemSql);
        if (itemDs.Tables.Count == 0 || itemDs.Tables[0].Rows.Count == 0)
            return "Record not found.";

        DataRow itemRow = itemDs.Tables[0].Rows[0];
        string itemName = itemRow["ItemName"].ToString().Trim();
        string itemLabel = String.IsNullOrEmpty(itemName)
            ? CultureInfo.InvariantCulture.TextInfo.ToTitleCase(type)
            : itemName;
        string rawDays = itemRow["txtdays"] == DBNull.Value
            ? String.Empty
            : itemRow["txtdays"].ToString().Trim();

        if (String.IsNullOrWhiteSpace(rawDays))
            return itemLabel + " duration days cannot be blank.";

        decimal durationDays;
        if (!Decimal.TryParse(rawDays, NumberStyles.Number, CultureInfo.InvariantCulture, out durationDays) ||
            durationDays < 0)
            return itemLabel + " duration days is invalid.";

        int candidateMinutes = GetDurationMinutes(durationValue);
        int allowedMinutes = GetPerDayCapacityMinutes(durationDays, perdayhrs);
        if (candidateMinutes > allowedMinutes)
        {
            return itemLabel + " Working Hrs " + FormatDurationMinutesInvariant(candidateMinutes) +
                   " cannot be greater than " + FormatDurationMinutesInvariant(allowedMinutes) +
                   " for " + rawDays + " duration days.";
        }

        return ValidateDurationHierarchyTotals(
            da,
            type,
            recordId,
            safeRegId,
            candidateMinutes
        );
    }

    private static string ValidateStoredDurationAgainstDays(
        DataAccess da,
        string tableName,
        string nameColumn,
        string id,
        string regid,
        string durationDaysValue,
        string perdayhrs)
    {
        int recordId;
        int safeRegId;
        if (!Int32.TryParse(id, out recordId) || recordId <= 0 ||
            !Int32.TryParse(regid, out safeRegId) || safeRegId <= 0)
            return "Invalid record.";

        string rawDays = (durationDaysValue ?? String.Empty).Trim();
        if (String.IsNullOrWhiteSpace(rawDays))
            return "Duration days cannot be blank.";

        decimal durationDays;
        if (!Decimal.TryParse(rawDays, NumberStyles.Number, CultureInfo.InvariantCulture, out durationDays) ||
            durationDays < 0)
            return "Duration days is invalid.";

        string itemSql = "select TaskDuration," + nameColumn + " as ItemName from " + tableName +
                         " with(nolock) where id=" + recordId + " and regid=" + safeRegId +
                         " and status=1 and archive=0";
        DataSet itemDs = da.GetDataSetInline(itemSql);
        if (itemDs.Tables.Count == 0 || itemDs.Tables[0].Rows.Count == 0)
            return "Record not found.";

        DataRow itemRow = itemDs.Tables[0].Rows[0];
        string itemName = itemRow["ItemName"].ToString().Trim();
        string itemLabel = String.IsNullOrEmpty(itemName) ? "Item" : itemName;
        int workingMinutes = GetDurationMinutes(itemRow["TaskDuration"].ToString());
        int allowedMinutes = GetPerDayCapacityMinutes(durationDays, perdayhrs);

        if (workingMinutes > allowedMinutes)
        {
            return itemLabel + " Working Hrs " + FormatDurationMinutesInvariant(workingMinutes) +
                   " cannot be greater than " + FormatDurationMinutesInvariant(allowedMinutes) +
                   " for " + rawDays + " duration days.";
        }

        return "1";
    }

    [WebMethod]
    public static string MileUpdateDetails(string id, string col_name, string col_value, string perdayhrs)
    {
        string retval = "0";
        DataAccess da1 = new DataAccess();
        try
        {
            if (!IsSafeSqlColumnName(col_name)) return retval;
            string regid = HttpContext.Current.Request.Cookies["cadmin"]["id"];
            if (String.Equals(col_name, "TaskDuration", StringComparison.OrdinalIgnoreCase))
            {
                string validation = ValidateTaskDurationDateCapacity(
                    da1, "Pms_tbl_AddMilestone", "EndDate", id, regid, col_value, perdayhrs);
                if (validation != "1") return validation;
            }
            string sqlValue = GetCustomColumnSqlValue(col_name, col_value);
            string strsql = " update Pms_tbl_AddMilestone set " + col_name + "=" + sqlValue + "  where status=1 and archive=0 and id='" + id + "' and regid='" + regid + "' ";
            da1.executeDMLQueryinline(strsql);
            retval = "1";
            if (col_name.ToLower() == "weightage")
            {
                string chkval = managecalculation(id, "milestone");
            }
        }
        catch (Exception ex)
        {
            retval = "0";
        }
        // return dscode.GetXml();
        
        return retval;
    }
    [WebMethod]
    public static string TaskUpdateDetails(string id, string col_name, string col_value, string perdayhrs)
    {
        string retval = "0";
        DataAccess da1 = new DataAccess();
        try
        {
            if (!IsSafeSqlColumnName(col_name)) return retval;
            string regid = HttpContext.Current.Request.Cookies["cadmin"]["id"];
            if (String.Equals(col_name, "TaskDuration", StringComparison.OrdinalIgnoreCase))
            {
                string validation = ValidateTaskDurationDateCapacity(
                    da1, "Pms_tbl_AddTask", "TargetDate", id, regid, col_value, perdayhrs);
                if (validation != "1") return validation;
            }
            string sqlValue = GetCustomColumnSqlValue(col_name, col_value);
            string strsql = " update Pms_tbl_AddTask set " + col_name + "=" + sqlValue + "  where status=1 and archive=0 and id='" + id + "' and regid='" + regid + "' ";
            da1.executeDMLQueryinline(strsql);
            retval = "1";
            if (col_name.ToLower() == "weightage")
            {
                string chkval = managecalculation(id, "task");
            }
        }
        catch (Exception ex)
        {
            retval = "0";
        }
        // return dscode.GetXml();
        return retval;
    }
    [WebMethod]
    public static string SubTaskUpdateDetails(string id, string col_name, string col_value, string perdayhrs)
    {
        string retval = "0";
        DataAccess da1 = new DataAccess();
        try
        {
            if (!IsSafeSqlColumnName(col_name)) return retval;
            string regid = HttpContext.Current.Request.Cookies["cadmin"]["id"];
            if (String.Equals(col_name, "TaskDuration", StringComparison.OrdinalIgnoreCase))
            {
                string validation = ValidateTaskDurationDateCapacity(
                    da1, "Pms_tbl_AddSubTask", "TargetDate", id, regid, col_value, perdayhrs);
                if (validation != "1") return validation;
            }
            string sqlValue = GetCustomColumnSqlValue(col_name, col_value);
            string strsql = " update Pms_tbl_AddSubTask set " + col_name + "=" + sqlValue + "  where status=1 and archive=0 and id='" + id + "' and regid='" + regid + "' ";
            da1.executeDMLQueryinline(strsql);
            retval = "1";
            if (col_name.ToLower() == "weightage")
            {
                string chkval = managecalculation(id, "subtask");
            }
        }
        catch (Exception ex)
        {
            retval = "0";
        }
        // return dscode.GetXml();
        return retval;
    }

    #endregion
    #region manage weightage

    public void updateallstatus_subtask()
    {
        DataAccess da = new DataAccess();
        string retval = "";
        string currentstatusid = "0";
        string chkstatusid = "0";
        try
        {
            string str2 = "select id, ProjectStatus as name, PrintColor as colorname,isnull(iscomplete,0) as iscomplete, isnull(isdefault,0) isdefault from Pms_tbl_Projectstatus where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and isnull(iscomplete,0)=1";
            DataSet dsS = da.GetDataSetInline(str2);
            chkstatusid = dsS.Tables[0].Rows[0]["id"].ToString();

        }
        catch { }

       
        //if (HttpContext.Current.Request.QueryString["id"] != null)
        //{
            string strsql = "select id,isnull(p_status,0) as p_status from pms_tbl_addsubtask where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  and status=1 and archive=0 ";//and projectid='" + HttpContext.Current.Request.QueryString["id"] + "'
            DataSet ds = da.GetDataSetInline(strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string updateids = "";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        //string str2 = "select id  from Pms_tbl_Addsubtask where ProjectId='" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "' and id=" + ds.Tables[0].Rows[i]["id"].ToString() + "  and P_Status in ( select id from Pms_tbl_Projectstatus where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "'  and status=1 and archive=0 and isnull(iscomplete,0)=1)  and status=1 and archive=0";
                        //DataSet ds2 = da.GetDataSetInline(str2);
                        //if (ds2.Tables[0].Rows.Count > 0)
                        //{
                        //if (ds.Tables[0].Rows[i]["id"].ToString() == statusid)
                        //{
                       
                        currentstatusid = ds.Tables[0].Rows[i]["p_status"].ToString();
                        if (currentstatusid == chkstatusid)
                        {
                            
                            string sts = UpdateSubtask_wt(ds.Tables[0].Rows[i]["id"].ToString(), currentstatusid, "1");
                        }
                            //}
                        //}
                    }
                    catch (Exception ex)
                    {
                        SaveLogText("updateidsAEXE=" + ex.Message);
                    }
                    //updateids = updateids + ","+ ds.Tables[0].Rows[i]["id"].ToString();
                }
                
            }
        //}
    }
    public void updateallstatus_task()
    {
        string retval = "";
        DataAccess da = new DataAccess();
        string currentstatusid = "0";
        string chkstatusid = "0";
        try
        {
            string str2 = "select id, ProjectStatus as name, PrintColor as colorname,isnull(iscomplete,0) as iscomplete, isnull(isdefault,0) isdefault from Pms_tbl_Projectstatus where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and isnull(iscomplete,0)=1";
            DataSet dsS = da.GetDataSetInline(str2);
            chkstatusid = dsS.Tables[0].Rows[0]["id"].ToString();

        }
        catch { }
        //if (HttpContext.Current.Request.QueryString["id"] != null)
        //{
            string strsql = "select id,isnull(p_status,0) as p_status from pms_tbl_addtask where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  ";//and projectid='" + HttpContext.Current.Request.QueryString["id"] + "'
        DataSet ds = da.GetDataSetInline(strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string updateids = "";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        //string str2 = "select id  from Pms_tbl_Addtask where ProjectId='" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "' and id=" + ds.Tables[0].Rows[i]["id"].ToString() + "  and P_Status in ( select id from Pms_tbl_Projectstatus where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and   regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and status=1 and archive=0 and isnull(iscomplete,0)=1)  and status=1 and archive=0";
                        //DataSet ds2 = da.GetDataSetInline(str2);
                        //if (ds2.Tables[0].Rows.Count > 0)
                        //{
                        currentstatusid = ds.Tables[0].Rows[i]["p_status"].ToString();
                        if (currentstatusid == chkstatusid)
                        {
                            string sts = UpdateTask_wt(ds.Tables[0].Rows[i]["id"].ToString(), currentstatusid,"1");
                            updateids = updateids + ",";
                        }
                        //}
                    }
                    catch(Exception ex) {
                        SaveLogText("updateidsAEXE=" + ex.Message);
                    }
                }
                SaveLogText("updateidsA="+ updateids);
            }
        //}
    }
    public void updateallstatus_mile()
    {
        string retval = "";
        DataAccess da = new DataAccess();
        string currentstatusid = "0";
        string chkstatusid = "0";
        try
        {
            string str2 = "select id, ProjectStatus as name, PrintColor as colorname,isnull(iscomplete,0) as iscomplete, isnull(isdefault,0) isdefault from Pms_tbl_Projectstatus where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and isnull(iscomplete,0)=1";
            DataSet dsS = da.GetDataSetInline(str2);
            chkstatusid = dsS.Tables[0].Rows[0]["id"].ToString();

        }
        catch { }
       // if (HttpContext.Current.Request.QueryString["id"] != null)
        //{
            string strsql = "select id,isnull(p_status,0) as p_status from pms_tbl_addmilestone where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  ";//and projectid='" + HttpContext.Current.Request.QueryString["id"] + "'
        DataSet ds = da.GetDataSetInline(strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    try
                    {

                        //string str2 = "select id  from Pms_tbl_Addmilestone where ProjectId='" + ds.Tables[0].Rows[i]["ProjectId"].ToString() + "' and id=" + ds.Tables[0].Rows[i]["id"].ToString() + "  and P_Status in ( select id from Pms_tbl_Projectstatus where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and   regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and status=1 and archive=0 and isnull(iscomplete,0)=1)  and status=1 and archive=0";
                        // DataSet ds2 = da.GetDataSetInline(str2);
                        //if (ds2.Tables[0].Rows.Count > 0)
                        //{
                        currentstatusid = ds.Tables[0].Rows[i]["p_status"].ToString();
                        if (currentstatusid == chkstatusid)
                        {
                            string sts = UpdateMilestone_wt(ds.Tables[0].Rows[i]["id"].ToString(), currentstatusid,"1");
                        }
                            //}
                    }
                    catch { }
                }
            }
        //}
    }
    protected string UpdateSubtask_wt(string subtaskid, string statusid,string iscomplete)
    {
        string retval = "";
        DataAccess da1 = new DataAccess();

        SqlParameter[] prm = new SqlParameter[8];
        prm[0] = new SqlParameter("@id", subtaskid);
        prm[1] = new SqlParameter("@ProjectStatus", statusid);
        prm[2] = new SqlParameter("@iscomplete", iscomplete);
        prm[3] = new SqlParameter("@iscompletedate", da1.getcdatetime());
        prm[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[6] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[7] = new SqlParameter("@type", "Update");
        da1.executeDMLQuery("pmstbl_subtask_Status_DeatilsUpdate", prm);
        decimal subtsktotalpercent = 100;
        decimal totlweitagemile = 0;
        decimal totlweitageproject = 0;

        string finsql1 = "select * from Pms_tbl_Addsubtask where id=" + subtaskid + " and status=1  and archive=0";
        DataSet dsfindid1 = da1.GetDataSetInline(finsql1);
        SaveLogText("finsql2-" + finsql1);
        string finsql2 = "select isnull(sum(weightage),0) as Wattagetasksum from Pms_tbl_Addsubtask where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "' and task='" + dsfindid1.Tables[0].Rows[0]["task"] + "'  and status=1 and  archive=0 and isnull(iscomplete,0)=1";

        DataSet dsfindid2 = da1.GetDataSetInline(finsql2);

        //sub task percentage //
        string strstatusupdate = "update Pms_tbl_Addsubtask set iscompletepercentage='" + subtsktotalpercent.ToString() + "' where id='" + subtaskid + "' and status=1 and archive=0 and isnull(iscomplete,0)=1";
        DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

        // task percentage //
        string finsql3 = "update Pms_tbl_Addtask set iscompletepercentage='" + dsfindid2.Tables[0].Rows[0]["Wattagetasksum"] + "' where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "' and id='" + dsfindid1.Tables[0].Rows[0]["task"] + "' and status=1 and archive=0";
        DataSet dsfindid3 = da1.GetDataSetInline(finsql3);


        // Milestone percentage //
        string findsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1  and archive=0";
        DataSet dsfindidmile = da1.GetDataSetInline(findsqlmile);
        for (int j = 0; j < dsfindidmile.Tables[0].Rows.Count; j++)
        {
            decimal sumweitage = 0;
            decimal subpercentageemile = 0;
            sumweitage = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["Weightage"].ToString());
            subpercentageemile = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
            totlweitagemile += (sumweitage * subpercentageemile) / 100;

        }

        string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile + "' where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and id='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1 and archive=0";
        DataSet dsfindid4 = da1.GetDataSetInline(finsql4);

        // project percentage //
        string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'   and status=1  and archive=0";
        DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
        for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
        {
            decimal sumweitageprojt = 0;
            decimal subpercentageeproject = 0;
            sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
            subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
            totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

        }

        string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'   and status=1 and archive=0";
        DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);
        //project  log details insert start
        SqlParameter[] prmlog = new SqlParameter[8];
        prmlog[0] = new SqlParameter("@Transid", subtaskid);
        prmlog[1] = new SqlParameter("@Logstatus", statusid);
        prmlog[2] = new SqlParameter("@LogDate", da1.getcdatetime());
        prmlog[3] = new SqlParameter("@Logtype", "subtask");
        prmlog[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prmlog[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prmlog[6] = new SqlParameter("@cdate", da1.getcdatetime());
        prmlog[7] = new SqlParameter("@type", "Insert");
        da1.executeDMLQuery("pmstbl_Loginsert_usp", prmlog);
        //project  log details insert end


        retval = "1";






        return retval;
    }
    protected string UpdateTask_wt(string taskid, string statusid, string iscomplete)
    {
        string retval = "";
        DataAccess da1 = new DataAccess();

        SqlParameter[] prm = new SqlParameter[8];
        prm[0] = new SqlParameter("@id", taskid);
        prm[1] = new SqlParameter("@ProjectStatus", statusid);
        prm[2] = new SqlParameter("@iscomplete", iscomplete);
        prm[3] = new SqlParameter("@iscompletedate", da1.getcdatetime());
        prm[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[6] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[7] = new SqlParameter("@type", "Update");
        da1.executeDMLQuery("pmstbl_task_Status_DeatilsUpdate", prm);
        decimal tsktotalpercent = 100;
        decimal totlweitagemile = 0;
        decimal totlweitageproject = 0;
        string finsql1 = "select * from Pms_tbl_Addtask where id=" + taskid + " and status=1 and archive=0";
        DataSet dsfindid1 = da1.GetDataSetInline(finsql1);

        // task percentage //
        string strstatusupdate = "update Pms_tbl_Addtask set iscompletepercentage='" + tsktotalpercent.ToString() + "' where id='" + taskid + "' and status=1 and archive=0 and isnull(iscomplete,0)=1";
        DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

        // milestone percentage //
        string finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and Milestoneid='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'   and status=1  and archive=0";
        DataSet dsfindid2 = da1.GetDataSetInline(finsql2);
        for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
        {
            decimal sumweitage = 0;
            decimal subpercentageemile = 0;
            sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
            subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
            totlweitagemile += (sumweitage * subpercentageemile) / 100;

        }

        string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile.ToString() + "' where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "' and id='" + dsfindid1.Tables[0].Rows[0]["Milestoneid"] + "'  and status=1 and archive=0";
        DataSet dsfindid4 = da1.GetDataSetInline(finsql4);

        // project percentage //

        string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1  and archive=0";
        DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
        for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
        {
            decimal sumweitageprojt = 0;
            decimal subpercentageeproject = 0;
            sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
            subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
            totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

        }

        string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
        DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);


        //project  log details insert start
        SqlParameter[] prmlog = new SqlParameter[8];
        prmlog[0] = new SqlParameter("@Transid", taskid);
        prmlog[1] = new SqlParameter("@Logstatus", statusid);
        prmlog[2] = new SqlParameter("@LogDate", da1.getcdatetime());
        prmlog[3] = new SqlParameter("@Logtype", "task");
        prmlog[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prmlog[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prmlog[6] = new SqlParameter("@cdate", da1.getcdatetime());
        prmlog[7] = new SqlParameter("@type", "Insert");
        da1.executeDMLQuery("pmstbl_Loginsert_usp", prmlog);
        //project  log details insert end
        retval = "1";


        return retval;
    }
    protected string UpdateMilestone_wt(string milstoneid,string statusid, string iscomplete)
    {
        string retval = "";
        DataAccess da1 = new DataAccess();

        SqlParameter[] prm = new SqlParameter[8];
        prm[0] = new SqlParameter("@id", milstoneid);
        prm[1] = new SqlParameter("@milstoneStatus", statusid);
        prm[2] = new SqlParameter("@iscomplete", iscomplete);
        prm[3] = new SqlParameter("@iscompletedate", da1.getcdatetime());
        prm[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[6] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[7] = new SqlParameter("@type", "Update");
        da1.executeDMLQuery("pmstbl_Status_milstone_Update_usp", prm);
        decimal milestonetotalpercent = 100;
        decimal totlweitagemile = 0;
        string finsql1 = "select * from Pms_tbl_AddMilestone where id=" + milstoneid + " and status=1  and archive=0";
        DataSet dsfindid1 = da1.GetDataSetInline(finsql1);

        // Milestone percentage //
        string strstatusupdate = "update Pms_tbl_AddMilestone set iscompletepercentage='" + milestonetotalpercent.ToString() + "' where id='" + milstoneid + "' and status=1 and archive=0 and isnull(iscomplete,0)=1";
        DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

        // project percentage //
        string finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1  and archive=0";
        DataSet dsfindid2 = da1.GetDataSetInline(finsql2);
        for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
        {
            decimal sumweitage = 0;
            decimal subpercentageemile = 0;
            sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
            subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
            totlweitagemile += (sumweitage * subpercentageemile) / 100;

        }

        string finsql4 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitagemile.ToString() + "' where id='" + dsfindid1.Tables[0].Rows[0]["ProjectId"] + "'  and status=1 and archive=0";
        DataSet dsfindid4 = da1.GetDataSetInline(finsql4);

        //project  log details insert start
        SqlParameter[] prmlog = new SqlParameter[8];
        prmlog[0] = new SqlParameter("@Transid", milstoneid);
        prmlog[1] = new SqlParameter("@Logstatus", statusid);
        prmlog[2] = new SqlParameter("@LogDate", da1.getcdatetime());
        prmlog[3] = new SqlParameter("@Logtype", "milestone");
        prmlog[4] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prmlog[5] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prmlog[6] = new SqlParameter("@cdate", da1.getcdatetime());
        prmlog[7] = new SqlParameter("@type", "Insert");
        da1.executeDMLQuery("pmstbl_Loginsert_usp", prmlog);
        //project  log details insert end
        retval = "1";

        return retval;
    }
   
    //public static string getweightage(string id,string wtype)
    //{
    //    DataAccess da = new DataAccess();
    //    string strdependency = "";
    //    string retval = "0";
    //    int checkweitage = 0;
    //    decimal wetagecount = 0;
    //    decimal totalwetage = 0;
    //    string strweightage = "select isnull(checkprojectWeightage ,0) as checkprojectWeightage  from Pms_tbl_AddProject where id='" + milestonedetails.ProjectId + "' and  status=1 and  isnull(checkprojectWeightage ,0)=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
    //    DataSet dsweitagecheck = da.GetDataSetInline(strweightage);
    //    if (dsweitagecheck.Tables[0].Rows.Count > 0)
    //    {
    //        checkweitage = Convert.ToInt32(dsweitagecheck.Tables[0].Rows[0]["checkprojectWeightage"]);

    //    }
    //    if (checkweitage == 1)
    //    {
    //        string strweightagecount = "select count (*) as WeitageCount from Pms_tbl_AddMilestone where ProjectId='" + milestonedetails.ProjectId + "' and  status=1   and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
    //        DataSet dsweitagecount = da.GetDataSetInline(strweightagecount);
    //        if (dsweitagecount.Tables[0].Rows.Count > 0)
    //        {


    //            wetagecount = Convert.ToInt32(dsweitagecount.Tables[0].Rows[0]["WeitageCount"]);

    //        }

    //        totalwetage = 100 / wetagecount;
    //        string strstatusupdate = "update Pms_tbl_AddMilestone set Weightage='" + fns.ToFormattedCurrencyString(totalwetage.ToString()) + "' where ProjectId='" + milestonedetails.ProjectId + "' and status=1  and archive=0";
    //        DataSet dshourmin = da.GetDataSetInline(strstatusupdate);

    //    }
    //    return retval;
    //}
    #endregion
    #region check dependency
    // Validate Gantt predecessor links when a task, subtask, or milestone is
    // being completed. Gantt links are stored separately from tbl_dependencyST.
    // FS and FF links require the predecessor to be complete before the
    // successor can be completed. SS and SF links do not require the
    // predecessor to be complete at that point.
    public static string checkGanttPredecessorCompletion(string strtype, string strid)
    {
        int regid;
        int itemId;
        HttpCookie accountCookie = HttpContext.Current.Request.Cookies["cadmin"];
        if (accountCookie == null || !Int32.TryParse(accountCookie["id"], out regid) || regid <= 0 ||
            !Int32.TryParse(strid, out itemId) || itemId <= 0)
            return "Could not validate dependencies. Please refresh and try again.";

        string itemType = (strtype ?? "").Trim().ToLowerInvariant();
        string entityTable = "";
        if (itemType == "milestone") entityTable = "Pms_tbl_AddMilestone";
        else if (itemType == "task") entityTable = "Pms_tbl_AddTask";
        else if (itemType == "subtask") entityTable = "Pms_tbl_AddSubtask";
        else return "Could not validate dependencies for this item.";

        try
        {
            DataAccess da = new DataAccess();
            string projectSql = "select top 1 ProjectId from " + entityTable +
                " where regid=" + regid + " and id=" + itemId + " and status=1 and archive=0";
            DataSet projectData = da.GetDataSetInline(projectSql);
            if (projectData == null || projectData.Tables.Count == 0 || projectData.Tables[0].Rows.Count == 0)
                return "Could not find this item to validate its dependencies.";

            int projectId;
            if (!Int32.TryParse(Convert.ToString(projectData.Tables[0].Rows[0]["ProjectId"]), out projectId) || projectId <= 0)
                return "Could not validate dependencies for this project item.";

            // Resolve all predecessor rows in one query. The previous implementation
            // queried each linked predecessor separately, making completion slow for
            // items with several dependencies and returning only a numeric ID to List.
            string linksSql = "select top 1 d.PredecessorType,d.PredecessorId,d.LinkType," +
                "case when lower(d.PredecessorType)='milestone' then isnull(convert(nvarchar(100),pm.orderno),'') " +
                "when lower(d.PredecessorType)='task' then isnull(convert(nvarchar(100),ptm.orderno),'')+'.'+isnull(convert(nvarchar(100),pt.orderno),'') " +
                "when lower(d.PredecessorType)='subtask' then isnull(convert(nvarchar(100),psm.orderno),'')+'.'+isnull(convert(nvarchar(100),pst.orderno),'')+'.'+isnull(convert(nvarchar(100),ps.orderno),'') else '' end as PredecessorSerial," +
                "case when lower(d.PredecessorType)='milestone' then isnull(convert(nvarchar(500),pm.Milestone),'') " +
                "when lower(d.PredecessorType)='task' then isnull(convert(nvarchar(500),pt.taskname),'') " +
                "when lower(d.PredecessorType)='subtask' then isnull(convert(nvarchar(500),ps.Subtaskname),'') else '' end as PredecessorName " +
                "from dbo.Pms_tbl_GanttDependency d " +
                "left join dbo.Pms_tbl_AddMilestone pm on lower(d.PredecessorType)='milestone' and pm.regid=" + regid + " and pm.ProjectId=" + projectId + " and pm.id=d.PredecessorId and pm.status=1 and pm.archive=0 " +
                "left join dbo.Pms_tbl_AddTask pt on lower(d.PredecessorType)='task' and pt.regid=" + regid + " and pt.ProjectId=" + projectId + " and pt.id=d.PredecessorId and pt.status=1 and pt.archive=0 " +
                "left join dbo.Pms_tbl_AddMilestone ptm on lower(d.PredecessorType)='task' and ptm.regid=" + regid + " and ptm.ProjectId=" + projectId + " and ptm.id=pt.Milestoneid and ptm.status=1 and ptm.archive=0 " +
                "left join dbo.Pms_tbl_AddSubtask ps on lower(d.PredecessorType)='subtask' and ps.regid=" + regid + " and ps.ProjectId=" + projectId + " and ps.id=d.PredecessorId and ps.status=1 and ps.archive=0 " +
                "left join dbo.Pms_tbl_AddTask pst on lower(d.PredecessorType)='subtask' and pst.regid=" + regid + " and pst.ProjectId=" + projectId + " and pst.id=ps.Task and pst.status=1 and pst.archive=0 " +
                "left join dbo.Pms_tbl_AddMilestone psm on lower(d.PredecessorType)='subtask' and psm.regid=" + regid + " and psm.ProjectId=" + projectId + " and psm.id=ps.Milestoneid and psm.status=1 and psm.archive=0 " +
                "where d.RegId=" + regid + " and d.ProjectId=" + projectId +
                " and lower(d.SuccessorType)='" + itemType + "' and d.SuccessorId=" + itemId +
                " and upper(isnull(d.LinkType,'')) in ('FS','FF') " +
                "and (lower(d.PredecessorType) not in ('milestone','task','subtask') " +
                "or (lower(d.PredecessorType)='milestone' and isnull(pm.statuscomplete,0)<>1) " +
                "or (lower(d.PredecessorType)='task' and isnull(pt.statuscomplete,0)<>1) " +
                "or (lower(d.PredecessorType)='subtask' and isnull(ps.statuscomplete,0)<>1)) order by d.Id";
            DataSet linksData = da.GetDataSetInline(linksSql);
            if (linksData == null || linksData.Tables.Count == 0 || linksData.Tables[0].Rows.Count == 0)
                return "0";

            DataRow link = linksData.Tables[0].Rows[0];
            string predecessorType = Convert.ToString(link["PredecessorType"]).Trim().ToLowerInvariant();
            int predecessorId;
            if (!Int32.TryParse(Convert.ToString(link["PredecessorId"]), out predecessorId) || predecessorId <= 0)
                return "Could not validate a linked predecessor. Please refresh and try again.";
            if (predecessorType != "milestone" && predecessorType != "task" && predecessorType != "subtask")
                return "Could not validate a linked predecessor of type '" + predecessorType + "'.";

            string predecessorSerial = HttpUtility.UrlEncode(Convert.ToString(link["PredecessorSerial"]), Encoding.UTF8);
            string predecessorName = HttpUtility.UrlEncode(Convert.ToString(link["PredecessorName"]), Encoding.UTF8);
            return "GANTT_DEPENDENCY_BLOCKED|" + predecessorType + "|" + predecessorId + "|" +
                Convert.ToString(link["LinkType"]) + "|" + predecessorSerial + "|" + predecessorName;
        }
        catch (Exception ex)
        {
            SaveLogText("Gantt predecessor completion validation: " + ex.Message);
            return "Could not validate dependencies. Please refresh and try again.";
        }

        return "0";
    }

    public static string checkdependency(string strtype, string strid)
    {
        DataAccess da1 = new DataAccess();
        string ret = "0";
        string strsqlM = "select id, isnull(Isdone,0) Isdone from tbl_dependencyST where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and milestoneid>0 and  milestoneid='" + strid + "' and status=1 and archive=0 and isnull(Isdone,0)=0";
        if (strtype == "task")
        {
            strsqlM = "select id, isnull(Isdone,0)  from tbl_dependencyST where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and taskid>0  and taskid='" + strid + "'  and status=1 and archive=0  and isnull(Isdone,0)=0";
        }
        else if (strtype == "subtask")
        {
            strsqlM = "select id,  isnull(Isdone,0)  from tbl_dependencyST where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and subtaskid>0  and subtaskid='" + strid + "'  and status=1 and archive=0  and isnull(Isdone,0)=0";
        }

        DataSet dschkD = da1.GetDataSetInline(strsqlM);
        if (dschkD.Tables[0].Rows.Count > 0)
        {
            string chkD = checkdependencynotcomplete(dschkD.Tables[0].Rows[0]["id"].ToString());
            ret = chkD;

        }

        return ret;

    }
    public static string checkdependencynotcomplete(string ids)
    {
        DataAccess da = new DataAccess();
        string strdependency = "";
        string retval = "0";
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
                        if (dsD.Tables[0].Rows[i]["stid"].ToString() != "" && dsD.Tables[0].Rows[i]["stid"].ToString() != "0")
                        {
                            try
                            {
                                string stsqlM = "select isnull(statuscomplete,0) statuscomplete from Pms_tbl_AddSubtask where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + dsD.Tables[0].Rows[i]["stid"].ToString() + "' and isnull(statuscomplete,0)=0";
                                SaveLogText("checkcomplete123-" + stsqlM);
                                DataSet dschkM = da.GetDataSetInline(stsqlM);
                                //if (dschkM.Tables[0].Rows[0]["statuscomplete"].ToString() != "1")
                                if (dschkM.Tables[0].Rows.Count > 0)
                                {
                                    retval = "1";
                                }

                            }
                            catch (Exception ex)
                            {
                                //SaveLogText("strbgc_s2-" + ex.Message);
                            }
                        }
                        else if (dsD.Tables[0].Rows[i]["tid"].ToString() != "" && dsD.Tables[0].Rows[i]["tid"].ToString() != "0")
                        {
                            try
                            {
                                string stsqlM = "select isnull(statuscomplete,0) statuscomplete from Pms_tbl_AddTask where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + dsD.Tables[0].Rows[i]["tid"].ToString() + "' and isnull(statuscomplete,0)=0";


                                DataSet dschkM = da.GetDataSetInline(stsqlM);
                                //if (dschkM.Tables[0].Rows[0]["statuscomplete"].ToString() != "1")
                                if (dschkM.Tables[0].Rows.Count > 0)
                                {
                                    retval = "1";
                                }
                            }
                            catch { }
                        }

                        else if (dsD.Tables[0].Rows[i]["mid"].ToString() != "" && dsD.Tables[0].Rows[i]["mid"].ToString() != "0")
                        {
                            try
                            {
                                string stsqlM = "select isnull(statuscomplete,0) statuscomplete from Pms_tbl_AddMilestone where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + dsD.Tables[0].Rows[i]["mid"].ToString() + "' and isnull(statuscomplete,0)=0";
                                DataSet dschkM = da.GetDataSetInline(stsqlM);
                                //if (dschkM.Tables[0].Rows[0]["statuscomplete"].ToString() != "1")
                                if (dschkM.Tables[0].Rows.Count > 0)
                                {
                                    retval = "1";
                                }
                            }
                            catch { }
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
    public static string checkdependencyUpdate(string strtype, string strid)
    {
        DataAccess da1 = new DataAccess();
        string ret = "0";
        string strsqlM = "select isnull(Isdone,0) Isdone from tbl_dependencyST where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and milestoneid>0 and  milestoneid='" + strid + "' and status=1 and archive=0 and isnull(Isdone,0)=0";
        if (strtype == "task")
        {
            strsqlM = "select  isnull(Isdone,0)  from tbl_dependencyST where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and taskid>0 and taskid='" + strid + "'  and status=1 and archive=0  and isnull(Isdone,0)=0";
        }
        else if (strtype == "subtask")
        {
            strsqlM = "select  isnull(Isdone,0)  from tbl_dependencyST where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and subtaskid>0 and subtaskid='" + strid + "'  and status=1 and archive=0  and isnull(Isdone,0)=0";
        }
        //SaveLogText("strp123=" + strsqlM);

        DataSet dschkD = da1.GetDataSetInline(strsqlM);
        if (dschkD.Tables[0].Rows.Count > 0)
        {
            string strupdt = "update  tbl_dependencyST set Isdone=1    where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and milestoneid>0 and  milestoneid='" + strid + "'  and status=1 and archive=0  and isnull(Isdone,0)=0";
            if (strtype == "task")
            {
                strupdt = "update  tbl_dependencyST set Isdone=1    where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and taskid>0 and taskid='" + strid + "'  and status=1 and archive=0  and isnull(Isdone,0)=0";
            }
            if (strtype == "subtask")
            {
                strupdt = "update  tbl_dependencyST set Isdone=1    where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and subtaskid>0 and subtaskid='" + strid + "'  and status=1 and archive=0  and isnull(Isdone,0)=0";
            }
            ret = "1";

        }

        return ret;

    }
    #endregion

    #region bindfiles
    
    public static List<projectfiles> bindfiledata(string pid, string id, string type)
    {

        DataAccess da = new DataAccess();
        string retval = "";


        //string strsqlfils = "select * from Pms_tbl_projectfiles where regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and status=1 and archive=0 and pid='" + pid + "' and fentryid='" + id + "' and ftype='" + type + "' order by id desc";
        string strsqlfils = @"select emp.DisplayName as empname,files.* from Pms_tbl_projectfiles files with(nolock)
                            left join tbl_employee emp with(nolock) on files.uid=emp.id where files.regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString() + "' and files.status=1 and files.archive=0 and files.pid='" + pid + "'  and fentryid='" + id + "' and ftype='" + type + "'   order by id desc";//and ftype='project' and fentryid='" + pid + "'
        SaveLogText("strsqlfils-" + strsqlfils);
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
                if (type != string.Empty)
                {
                    if (type == "milestone")
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
                    Uploadby = dsfils.Tables[0].Rows[i]["empname"].ToString(),
                    fentryid = dsfils.Tables[0].Rows[i]["fentryid"].ToString(),
                    ftype = dsfils.Tables[0].Rows[i]["ftype"].ToString(),
                    fileurl = dsfils.Tables[0].Rows[i]["fileurl"].ToString(),
                    filedes = dsfils.Tables[0].Rows[i]["filedes"].ToString(),
                    fext = dsfils.Tables[0].Rows[i]["fext"].ToString(),
                    filepath = filepath,
                    

                });

            }
           
        }
        

        return newprojectfiles;
    }
    #endregion
    #region hierarchy timeline end-date popup
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string UpdateHierarchyEndDate(string id = "", string utype = "", string enddate = "")
    {
        try
        {
            int recordId;
            int regId;
            if (!Int32.TryParse(id, out recordId) || recordId <= 0)
                return "0";

            HttpCookie companyCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (companyCookie == null ||
                !Int32.TryParse(companyCookie["id"], out regId) ||
                regId <= 0)
            {
                return "0";
            }

            string normalizedType = (utype ?? "").Trim().ToLowerInvariant();
            string tableName;
            string startColumn;
            string endColumn;

            if (normalizedType == "milestone")
            {
                tableName = "Pms_tbl_AddMilestone";
                startColumn = "StartDate";
                endColumn = "EndDate";
            }
            else if (normalizedType == "task")
            {
                tableName = "Pms_tbl_Addtask";
                startColumn = "Startdate";
                endColumn = "Targetdate";
            }
            else if (normalizedType == "subtask")
            {
                tableName = "Pms_tbl_Addsubtask";
                startColumn = "Startdate";
                endColumn = "Targetdate";
            }
            else
            {
                return "0";
            }

            DateTime newEndDate = ParseAssignDateOnly(enddate).Date;
            DataAccess da = new DataAccess();

            string selectSql;
            if (normalizedType == "milestone")
            {
                selectSql =
                    "select m.StartDate,p.TargetDate as ParentEndDate " +
                    "from Pms_tbl_AddMilestone m with(nolock) " +
                    "inner join Pms_tbl_AddProject p with(nolock) on p.id=m.ProjectId " +
                    "where m.status=1 and m.archive=0 and p.status=1 and p.archive=0 " +
                    "and m.regid=" + regId + " and m.id=" + recordId;
            }
            else if (normalizedType == "task")
            {
                selectSql =
                    "select t.Startdate,m.EndDate as ParentEndDate " +
                    "from Pms_tbl_Addtask t with(nolock) " +
                    "inner join Pms_tbl_AddMilestone m with(nolock) on m.id=t.Milestoneid " +
                    "where t.status=1 and t.archive=0 and m.status=1 and m.archive=0 " +
                    "and t.regid=" + regId + " and t.id=" + recordId;
            }
            else
            {
                selectSql =
                    "select st.Startdate,t.Targetdate as ParentEndDate " +
                    "from Pms_tbl_Addsubtask st with(nolock) " +
                    "inner join Pms_tbl_Addtask t with(nolock) on t.id=st.task " +
                    "where st.status=1 and st.archive=0 and t.status=1 and t.archive=0 " +
                    "and st.regid=" + regId + " and st.id=" + recordId;
            }

            DataSet dateData = da.GetDataSetInline(selectSql);
            if (dateData.Tables.Count == 0 || dateData.Tables[0].Rows.Count == 0)
                return "0";

            DateTime startDate = Convert.ToDateTime(
                dateData.Tables[0].Rows[0]["StartDate"],
                CultureInfo.InvariantCulture
            ).Date;

            if (newEndDate < startDate)
                return "0";

            object parentEndValue = dateData.Tables[0].Rows[0]["ParentEndDate"];
            if (parentEndValue == DBNull.Value)
                return "0";

            DateTime parentEndDate = Convert.ToDateTime(
                parentEndValue,
                CultureInfo.InvariantCulture
            ).Date;

            // The UI extends the parent first. This server check ensures that a
            // direct or partial request can never leave a child beyond its parent.
            if (newEndDate > parentEndDate)
                return "0";

            string childEndSql = "";
            if (normalizedType == "task")
            {
                childEndSql =
                    "select max(Targetdate) as MaxChildEnd from Pms_tbl_Addsubtask with(nolock) " +
                    "where status=1 and archive=0 and regid=" + regId +
                    " and task=" + recordId;
            }
            else if (normalizedType == "milestone")
            {
                childEndSql =
                    "select max(ChildEnd) as MaxChildEnd from (" +
                    "select Targetdate as ChildEnd from Pms_tbl_Addtask with(nolock) " +
                    "where status=1 and archive=0 and regid=" + regId + " and Milestoneid=" + recordId +
                    " union all " +
                    "select Targetdate as ChildEnd from Pms_tbl_Addsubtask with(nolock) " +
                    "where status=1 and archive=0 and regid=" + regId + " and Milestoneid=" + recordId +
                    ") hierarchychildren";
            }

            if (!String.IsNullOrEmpty(childEndSql))
            {
                DataSet childData = da.GetDataSetInline(childEndSql);
                if (childData.Tables.Count > 0 &&
                    childData.Tables[0].Rows.Count > 0 &&
                    childData.Tables[0].Rows[0]["MaxChildEnd"] != DBNull.Value)
                {
                    DateTime maxChildEnd = Convert.ToDateTime(
                        childData.Tables[0].Rows[0]["MaxChildEnd"],
                        CultureInfo.InvariantCulture
                    ).Date;

                    if (newEndDate < maxChildEnd)
                        return "0";
                }
            }

            int totalWorkingDays = 0;
            const string employeeId = "0";
            for (DateTime checkDate = startDate;
                 checkDate <= newEndDate;
                 checkDate = checkDate.AddDays(1))
            {
                if (!getweeklyoff(checkDate, employeeId) &&
                    !getholiday(checkDate, employeeId))
                {
                    totalWorkingDays++;
                }
            }

            // Existing hierarchy validation expects a positive duration value.
            if (totalWorkingDays <= 0)
                totalWorkingDays = 1;

            string endText = newEndDate.ToString(
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture
            );

            string updateSql =
                "update " + tableName +
                " set " + endColumn + "=convert(date,'" + ToSqlDateOnly(newEndDate) + "',112)," +
                " txtdays='" + totalWorkingDays.ToString(CultureInfo.InvariantCulture) + "'" +
                " where status=1 and archive=0 and regid=" + regId +
                " and id=" + recordId;

            da.executeDMLQueryinline(updateSql);

            // Date-only response prevents browser timezone shifting.
            return endText + "|" +
                totalWorkingDays.ToString(CultureInfo.InvariantCulture);
        }
        catch (Exception ex)
        {
            SaveLogText("UpdateHierarchyEndDateEX-" + ex.Message);
            return "0";
        }
    }
    #endregion

    #region update project timeline
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string ExtendProjectTimeline(string projctid = "", string extendStrtdate = "", string extendEnddate = "", string oldEnddate = "")
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
        prm[1] = new SqlParameter("@StartDate", ParseDateOnly(oldEnddate));
        prm[2] = new SqlParameter("@TargetDate", ParseDateOnly(extendEnddate));
        prm[3] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[4] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[5] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[6] = new SqlParameter("@type", entrytype);
        prm[7] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[7].Direction = ParameterDirection.ReturnValue;
        int newid = da1.executeDMLQuery("pmstbl_Extnddate_DeatilsUpdate", prm);        


        return newid.ToString();
    }
    private class HierarchyExtensionLevel
    {
        public string Type { get; set; }
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime OldEndDate { get; set; }
        public string Days { get; set; }
    }

    private static int SaveHierarchyExtensionHistory(
        DataAccess da,
        int recordId,
        DateTime oldEndDate,
        DateTime newEndDate,
        int regId,
        int userId,
        string extendType)
    {
        string normalizedType = (extendType ?? "").Trim().ToLowerInvariant();

        // First try the hierarchy-aware stored procedure signature. This works
        // when @extendtype has already been added to the procedure.
        try
        {
            SqlParameter[] hierarchyPrm = new SqlParameter[9];
            hierarchyPrm[0] = new SqlParameter("@id", recordId);
            hierarchyPrm[1] = new SqlParameter("@StartDate", oldEndDate.Date);
            hierarchyPrm[2] = new SqlParameter("@TargetDate", newEndDate.Date);
            hierarchyPrm[3] = new SqlParameter("@regid", regId);
            hierarchyPrm[4] = new SqlParameter("@uid", userId);
            hierarchyPrm[5] = new SqlParameter("@cdate", da.getcdatetime());
            hierarchyPrm[6] = new SqlParameter("@type", "Update");
            hierarchyPrm[7] = new SqlParameter("@extendtype", normalizedType);
            hierarchyPrm[8] = new SqlParameter("ReturnValue", SqlDbType.Int);
            hierarchyPrm[8].Direction = ParameterDirection.ReturnValue;

            int hierarchyHistoryId = da.executeDMLQuery(
                "pmstbl_Extnddate_DeatilsUpdate",
                hierarchyPrm
            );

            if (hierarchyHistoryId > 0)
                return hierarchyHistoryId;
        }
        catch (Exception ex)
        {
            SaveLogText(
                "SaveHierarchyExtensionHistory-9Param-" +
                normalizedType + "-" + ex.Message
            );
        }

        // Backward compatibility: the old procedure supports only the project
        // parameter list. Continue using it for project history.
        if (normalizedType == "project")
        {
            try
            {
                SqlParameter[] projectPrm = new SqlParameter[8];
                projectPrm[0] = new SqlParameter("@id", recordId);
                projectPrm[1] = new SqlParameter("@StartDate", oldEndDate.Date);
                projectPrm[2] = new SqlParameter("@TargetDate", newEndDate.Date);
                projectPrm[3] = new SqlParameter("@regid", regId);
                projectPrm[4] = new SqlParameter("@uid", userId);
                projectPrm[5] = new SqlParameter("@cdate", da.getcdatetime());
                projectPrm[6] = new SqlParameter("@type", "Update");
                projectPrm[7] = new SqlParameter("ReturnValue", SqlDbType.Int);
                projectPrm[7].Direction = ParameterDirection.ReturnValue;

                int projectHistoryId = da.executeDMLQuery(
                    "pmstbl_Extnddate_DeatilsUpdate",
                    projectPrm
                );

                if (projectHistoryId > 0)
                    return projectHistoryId;
            }
            catch (Exception ex)
            {
                SaveLogText(
                    "SaveHierarchyExtensionHistory-8Param-project-" + ex.Message
                );
            }
        }

        // When the existing procedure has not yet been changed for hierarchy
        // types, retain extension history directly in the same history table.
        // ProjectID stores the extended hierarchy record ID and Type identifies
        // whether it is a Subtask, Task, Milestone or Project.
        try
        {
            string historyType = normalizedType == "subtask" ? "Subtask" :
                                 normalizedType == "task" ? "Task" :
                                 normalizedType == "milestone" ? "Milestone" :
                                 "Project";

            DateTime historyDate;
            try
            {
                historyDate = Convert.ToDateTime(
                    da.getcdatetime(),
                    CultureInfo.InvariantCulture
                );
            }
            catch
            {
                historyDate = DateTime.Now;
            }

            string insertHistorySql =
                "insert into Pms_Tbl_Extended_Timeline " +
                "(ProjectID,extendStartdate,extendEnddate,regid,uid,cdate,Type,status,archive) values (" +
                recordId + ",convert(date,'" + ToSqlDateOnly(oldEndDate) + "',112)," +
                "convert(date,'" + ToSqlDateOnly(newEndDate) + "',112)," +
                regId + "," + userId + ",convert(datetime2,'" +
                ToSqlDateTime(historyDate) +
                "',126),'" + historyType + "',1,0)";

            da.executeDMLQueryinline(insertHistorySql);
            return 1;
        }
        catch (Exception ex)
        {
            SaveLogText(
                "SaveHierarchyExtensionHistory-Direct-" +
                normalizedType + "-" + ex.Message
            );
            return 0;
        }
    }

    private class HierarchyCascadeSavedItem
    {
        public string Type { get; set; }
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime OldEndDate { get; set; }
        public string Days { get; set; }
        public string StartTime { get; set; }
    }

    private class HierarchyCascadeRow
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Days { get; set; }
        public string StartTime { get; set; }
        public string TaskDuration { get; set; }
    }

    private class HierarchyCascadeRangeResult
    {
        public DateTime CompletionDateTime { get; set; }
        public DateTime MaxEndDate { get; set; }
    }

    private static string GetHierarchyCascadeKey(string type, int id)
    {
        return (type ?? String.Empty).Trim().ToLowerInvariant() + ":" +
            id.ToString(CultureInfo.InvariantCulture);
    }

    private static DateTime GetHierarchyDateOrMinValue(
        DataRow row,
        string columnName)
    {
        if (row == null || String.IsNullOrWhiteSpace(columnName) ||
            !row.Table.Columns.Contains(columnName) ||
            row[columnName] == null || row[columnName] == DBNull.Value)
        {
            return DateTime.MinValue;
        }

        DateTime parsed;
        return DateTime.TryParse(
            Convert.ToString(row[columnName], CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture,
            DateTimeStyles.AllowWhiteSpaces,
            out parsed)
            ? parsed.Date
            : DateTime.MinValue;
    }

    private static void AddOrUpdateHierarchyCascadeResult(
        List<HierarchyCascadeSavedItem> savedItems,
        Dictionary<string, int> savedIndexes,
        HierarchyCascadeSavedItem item)
    {
        if (item == null) return;

        string key = GetHierarchyCascadeKey(item.Type, item.Id);
        int existingIndex;
        if (savedIndexes.TryGetValue(key, out existingIndex))
        {
            HierarchyCascadeSavedItem existing = savedItems[existingIndex];
            existing.StartDate = item.StartDate;
            existing.EndDate = item.EndDate;
            existing.Days = item.Days;
            existing.StartTime = item.StartTime;
            return;
        }

        savedIndexes[key] = savedItems.Count;
        savedItems.Add(item);
    }

    private static HierarchyCascadeRow LoadHierarchyCascadeRow(
        DataAccess da,
        string type,
        int id,
        int regId,
        bool allowBlankDates = false)
    {
        string normalizedType = (type ?? String.Empty).Trim().ToLowerInvariant();
        string sql;

        if (normalizedType == "milestone")
        {
            sql =
                "select id,StartDate as StartDateValue,EndDate as EndDateValue," +
                "isnull(txtdays,'0') as DaysValue,isnull(StartTime,'') as StartTimeValue," +
                "isnull(TaskDuration,'00:00') as TaskDurationValue " +
                "from Pms_tbl_AddMilestone with(nolock) where status=1 and archive=0 " +
                "and regid=" + regId + " and id=" + id;
        }
        else if (normalizedType == "task")
        {
            sql =
                "select id,Startdate as StartDateValue,Targetdate as EndDateValue," +
                "isnull(txtdays,'0') as DaysValue,isnull(StartTime,'') as StartTimeValue," +
                "isnull(TaskDuration,'00:00') as TaskDurationValue " +
                "from Pms_tbl_Addtask with(nolock) where status=1 and archive=0 " +
                "and regid=" + regId + " and id=" + id;
        }
        else if (normalizedType == "subtask")
        {
            sql =
                "select id,Startdate as StartDateValue,Targetdate as EndDateValue," +
                "isnull(txtdays,'0') as DaysValue,isnull(StartTime,'') as StartTimeValue," +
                "isnull(TaskDuration,'00:00') as TaskDurationValue " +
                "from Pms_tbl_Addsubtask with(nolock) where status=1 and archive=0 " +
                "and regid=" + regId + " and id=" + id;
        }
        else
        {
            throw new Exception("Invalid hierarchy type.");
        }

        DataSet data = da.GetDataSetInline(sql);
        if (data.Tables.Count == 0 || data.Tables[0].Rows.Count == 0)
            throw new Exception("Hierarchy record was not found.");

        DataRow row = data.Tables[0].Rows[0];
        bool hasStartDate = row["StartDateValue"] != DBNull.Value;
        bool hasEndDate = row["EndDateValue"] != DBNull.Value;

        if ((!hasStartDate || !hasEndDate) && !allowBlankDates)
            throw new Exception("Hierarchy timeline date is invalid.");

        return new HierarchyCascadeRow
        {
            Id = Convert.ToInt32(row["id"], CultureInfo.InvariantCulture),
            StartDate = hasStartDate
                ? Convert.ToDateTime(row["StartDateValue"], CultureInfo.InvariantCulture).Date
                : DateTime.MinValue,
            EndDate = hasEndDate
                ? Convert.ToDateTime(row["EndDateValue"], CultureInfo.InvariantCulture).Date
                : DateTime.MinValue,
            Days = row["DaysValue"].ToString(),
            StartTime = row["StartTimeValue"].ToString(),
            TaskDuration = row["TaskDurationValue"].ToString()
        };
    }

    private static List<HierarchyCascadeRow> LoadHierarchyCascadeSiblings(
        DataAccess da,
        string type,
        int parentId,
        int regId,
        bool allowBlankDates = false)
    {
        string normalizedType = (type ?? String.Empty).Trim().ToLowerInvariant();
        string sql;

        if (normalizedType == "milestone")
        {
            sql =
                "select id,StartDate as StartDateValue,EndDate as EndDateValue," +
                "isnull(txtdays,'0') as DaysValue,isnull(StartTime,'') as StartTimeValue," +
                "isnull(TaskDuration,'00:00') as TaskDurationValue " +
                "from Pms_tbl_AddMilestone with(nolock) where status=1 and archive=0 " +
                "and regid=" + regId + " and ProjectId=" + parentId +
                " order by isnull(orderno,0),id";
        }
        else if (normalizedType == "task")
        {
            sql =
                "select id,Startdate as StartDateValue,Targetdate as EndDateValue," +
                "isnull(txtdays,'0') as DaysValue,isnull(StartTime,'') as StartTimeValue," +
                "isnull(TaskDuration,'00:00') as TaskDurationValue " +
                "from Pms_tbl_Addtask with(nolock) where status=1 and archive=0 " +
                "and regid=" + regId + " and Milestoneid=" + parentId +
                " order by isnull(orderno,0),id";
        }
        else if (normalizedType == "subtask")
        {
            sql =
                "select id,Startdate as StartDateValue,Targetdate as EndDateValue," +
                "isnull(txtdays,'0') as DaysValue,isnull(StartTime,'') as StartTimeValue," +
                "isnull(TaskDuration,'00:00') as TaskDurationValue " +
                "from Pms_tbl_Addsubtask with(nolock) where status=1 and archive=0 " +
                "and regid=" + regId + " and task=" + parentId +
                " order by isnull(orderno,0),id";
        }
        else
        {
            throw new Exception("Invalid hierarchy type.");
        }

        DataSet data = da.GetDataSetInline(sql);
        List<HierarchyCascadeRow> rows = new List<HierarchyCascadeRow>();
        if (data.Tables.Count == 0) return rows;

        foreach (DataRow row in data.Tables[0].Rows)
        {
            bool hasStartDate = row["StartDateValue"] != DBNull.Value;
            bool hasEndDate = row["EndDateValue"] != DBNull.Value;

            // Normal timeline operations keep the old behaviour and ignore
            // incomplete rows. Reset Start Date must include them so the reset
            // can populate their StartDate and EndDate from the parent cursor.
            if ((!hasStartDate || !hasEndDate) && !allowBlankDates)
                continue;

            rows.Add(new HierarchyCascadeRow
            {
                Id = Convert.ToInt32(row["id"], CultureInfo.InvariantCulture),
                StartDate = hasStartDate
                    ? Convert.ToDateTime(row["StartDateValue"], CultureInfo.InvariantCulture).Date
                    : DateTime.MinValue,
                EndDate = hasEndDate
                    ? Convert.ToDateTime(row["EndDateValue"], CultureInfo.InvariantCulture).Date
                    : DateTime.MinValue,
                Days = row["DaysValue"].ToString(),
                StartTime = row["StartTimeValue"].ToString(),
                TaskDuration = row["TaskDurationValue"].ToString()
            });
        }

        return rows;
    }

    private static string FormatHierarchyCascadeStartTime(DateTime value)
    {
        return value.ToString("hh:mm tt", CultureInfo.InvariantCulture);
    }

    private static DateTime GetHierarchyCascadeStartDateTime(HierarchyCascadeRow row)
    {
        TimeSpan startTime = ParseTimeOfDayInvariant(row == null ? String.Empty : row.StartTime, "09:00 AM");
        DateTime startDate = row == null ? DateTime.Today : row.StartDate.Date;
        return DateTime.SpecifyKind(startDate.Add(startTime), DateTimeKind.Unspecified);
    }

    private static DateTime CalculateHierarchyWorkCompletionDateTime(HierarchyCascadeRow row,string perDayHours,string pid)
    {
        DateTime startDateTime = GetHierarchyCascadeStartDateTime(row);
        int durationMinutes = GetDurationMinutes(row == null ? String.Empty : row.TaskDuration);
        if (durationMinutes <= 0) return startDateTime;

        decimal capacityHours = ParsePerDayHoursInvariant(perDayHours);
        int capacityMinutes = Convert.ToInt32(
            Math.Round(capacityHours * 60M, 0, MidpointRounding.AwayFromZero)
        );
        if (capacityMinutes <= 0) capacityMinutes = 8 * 60;

        // Keep this calculation identical to the Reset Start Date cursor in JS:
        // - partial final day: remain on that working date at the remaining time;
        // - exact full-day completion: move to the next working date at StartTime;
        // - use the existing manage_enddate rule so weekly offs/holidays are
        //   handled exactly as they are on Reset Start Date click.
        int workDays = Math.Max(
            1,
            Convert.ToInt32(Math.Ceiling(
                Convert.ToDecimal(durationMinutes) / capacityMinutes
            ))
        );
        int remainingMinutes = durationMinutes % capacityMinutes;
        int dateDays = remainingMinutes > 0 ? workDays : workDays + 1;

        string completionDateText = manage_enddate(dateDays.ToString(CultureInfo.InvariantCulture),
            startDateTime.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
        ,pid);
        DateTime completionDate = String.IsNullOrWhiteSpace(completionDateText)
            ? startDateTime.Date.AddDays(dateDays - 1)
            : ParseAssignDateOnly(completionDateText).Date;

        DateTime completionDateTime = completionDate.Add(startDateTime.TimeOfDay);
        if (remainingMinutes > 0)
            completionDateTime = completionDateTime.AddMinutes(remainingMinutes);

        return DateTime.SpecifyKind(completionDateTime, DateTimeKind.Unspecified);
    }

    private static DateTime GetHierarchyEffectiveCompletionDateTime(
        DataAccess da,
        string type,
        int id,
        string perDayHours,
        int regId,
        Dictionary<string, DateTime> completionOverrides,
        HashSet<string> visiting)
    {
        string projectid = getprojectid(id.ToString(),type);
        string normalizedType = (type ?? String.Empty).Trim().ToLowerInvariant();
        string key = GetHierarchyCascadeKey(normalizedType, id);
        if (visiting.Contains(key))
            return DateTime.MinValue;

        visiting.Add(key);
        HierarchyCascadeRow row = LoadHierarchyCascadeRow(da, normalizedType, id, regId);
        DateTime completion = CalculateHierarchyWorkCompletionDateTime(row, perDayHours,projectid);

        DateTime explicitCompletion;
        if (completionOverrides != null &&
            completionOverrides.TryGetValue(key, out explicitCompletion) &&
            explicitCompletion > completion)
        {
            completion = explicitCompletion;
        }

        /*
         * The sequence cursor must match Reset Start Date exactly: the next
         * sibling starts from this row's own StartDate + StartTime + Working Hrs.
         * A child can extend the parent's displayed EndDate, but that child end
         * must not become the start cursor of the next Task or Milestone.
         *
         * Example: Milestone 2 starts 30-Jul-2026 and has 20 working hours at
         * 8 hours/day. Its own completion cursor is 01-Aug-2026 (2.5 days), even
         * when a child Task extends Milestone 2's displayed EndDate to 10-Aug.
         */
        visiting.Remove(key);
        return completion;
    }

    private static DateTime GetHierarchyEffectiveCompletionDateTime(
        DataAccess da,
        string type,
        int id,
        string perDayHours,
        int regId,
        Dictionary<string, DateTime> completionOverrides)
    {
        return GetHierarchyEffectiveCompletionDateTime(
            da,
            type,
            id,
            perDayHours,
            regId,
            completionOverrides,
            new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        );
    }

    private static HierarchyCascadeSavedItem SaveHierarchyCascadeStartDate(
        DataAccess da,
        string type,
        int id,
        DateTime newStartDate,
        string days,
        string startTime,
        string perDayHours,
        int regId,
        bool allowBlankExistingDates = false)
    {
        string normalizedType = (type ?? String.Empty).Trim().ToLowerInvariant();
        HierarchyCascadeRow oldRow = LoadHierarchyCascadeRow(
            da, normalizedType, id, regId, allowBlankExistingDates
        );

        if (String.IsNullOrWhiteSpace(days))
            throw new Exception("Duration days cannot be blank.");

        string normalizedStartTime = String.IsNullOrWhiteSpace(startTime)
            ? oldRow.StartTime
            : startTime.Trim();
        if (String.IsNullOrWhiteSpace(normalizedStartTime))
            normalizedStartTime = "09:00 AM";

        string result;
        string idText = id.ToString(CultureInfo.InvariantCulture);
        string startText = newStartDate.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

        if (normalizedType == "milestone")
        {
            result = updatetimeline_mile(
                idText, days, startText, normalizedStartTime, perDayHours, String.Empty
            );
        }
        else if (normalizedType == "task")
        {
            result = updatetimeline_task(
                idText, days, startText, normalizedStartTime, perDayHours, String.Empty
            );
        }
        else if (normalizedType == "subtask")
        {
            result = updatetimeline_subtask(
                idText, days, startText, normalizedStartTime, perDayHours, String.Empty
            );
        }
        else
        {
            throw new Exception("Invalid hierarchy type.");
        }

        if (String.IsNullOrWhiteSpace(result))
            throw new Exception("Hierarchy start date was not updated.");

        if (result.StartsWith("ERROR|", StringComparison.OrdinalIgnoreCase))
            throw new Exception(result.Substring(6));

        DateTime savedEndDate = ParseAssignDateOnly(result).Date;
        return new HierarchyCascadeSavedItem
        {
            Type = normalizedType,
            Id = id,
            StartDate = newStartDate.Date,
            EndDate = savedEndDate,
            OldEndDate = oldRow.EndDate,
            Days = days,
            StartTime = normalizedStartTime
        };
    }

    private static HierarchyCascadeSavedItem SaveHierarchyCascadeEndDate(
        DataAccess da,
        string type,
        int id,
        DateTime newEndDate,
        int regId)
    {
        string normalizedType = (type ?? String.Empty).Trim().ToLowerInvariant();
        HierarchyCascadeRow oldRow = LoadHierarchyCascadeRow(
            da, normalizedType, id, regId
        );

        if (newEndDate.Date < oldRow.StartDate)
            throw new Exception("End date cannot be before start date.");

        string tableName;
        string endColumn;
        if (normalizedType == "milestone")
        {
            tableName = "Pms_tbl_AddMilestone";
            endColumn = "EndDate";
        }
        else if (normalizedType == "task")
        {
            tableName = "Pms_tbl_Addtask";
            endColumn = "Targetdate";
        }
        else if (normalizedType == "subtask")
        {
            tableName = "Pms_tbl_Addsubtask";
            endColumn = "Targetdate";
        }
        else
        {
            throw new Exception("Invalid hierarchy type.");
        }

        string updateSql =
            "update " + tableName + " set " + endColumn + "=convert(date,'" +
            ToSqlDateOnly(newEndDate.Date) + "',112) " +
            "where status=1 and archive=0 and regid=" + regId + " and id=" + id;

        da.executeDMLQueryinline(updateSql);

        return new HierarchyCascadeSavedItem
        {
            Type = normalizedType,
            Id = id,
            StartDate = oldRow.StartDate,
            EndDate = newEndDate.Date,
            OldEndDate = oldRow.EndDate,
            Days = oldRow.Days,
            StartTime = oldRow.StartTime
        };
    }

    private static HierarchyCascadeSavedItem SaveHierarchyCascadeProjectEndDate(
        DataAccess da,
        int projectId,
        DateTime newEndDate,
        int regId)
    {
        string sql =
            "select StartDate,TargetDate from Pms_tbl_AddProject with(nolock) " +
            "where status=1 and archive=0 and regid=" + regId +
            " and id=" + projectId;
        DataSet data = da.GetDataSetInline(sql);
        if (data.Tables.Count == 0 || data.Tables[0].Rows.Count == 0 ||
            data.Tables[0].Rows[0]["StartDate"] == DBNull.Value)
        {
            throw new Exception("Project timeline was not found.");
        }

        DateTime startDate = Convert.ToDateTime(
            data.Tables[0].Rows[0]["StartDate"], CultureInfo.InvariantCulture
        ).Date;
        DateTime oldEndDate = data.Tables[0].Rows[0]["TargetDate"] == DBNull.Value
            ? DateTime.MinValue
            : Convert.ToDateTime(
                data.Tables[0].Rows[0]["TargetDate"], CultureInfo.InvariantCulture
            ).Date;

        if (newEndDate.Date < startDate)
            throw new Exception("Project end date cannot be before project start date.");

        string updateSql =
            "update Pms_tbl_AddProject set TargetDate=convert(date,'" +
            ToSqlDateOnly(newEndDate.Date) + "',112) where status=1 and archive=0 " +
            "and regid=" + regId + " and id=" + projectId;
        da.executeDMLQueryinline(updateSql);

        return new HierarchyCascadeSavedItem
        {
            Type = "project",
            Id = projectId,
            StartDate = startDate,
            EndDate = newEndDate.Date,
            OldEndDate = oldEndDate,
            Days = String.Empty,
            StartTime = String.Empty
        };
    }

    private static DateTime ShiftHierarchyTaskSubtasks(
        DataAccess da,
        int taskId,
        TimeSpan delta,
        string perDayHours,
        int regId,
        List<HierarchyCascadeSavedItem> savedItems,
        Dictionary<string, int> savedIndexes)
    {
        DateTime maxEndDate = DateTime.MinValue;
        List<HierarchyCascadeRow> subtasks = LoadHierarchyCascadeSiblings(
            da, "subtask", taskId, regId
        );

        foreach (HierarchyCascadeRow subtask in subtasks)
        {
            DateTime oldStartDateTime = GetHierarchyCascadeStartDateTime(subtask);
            DateTime shiftedStartDateTime = oldStartDateTime.Add(delta);
            HierarchyCascadeSavedItem saved = SaveHierarchyCascadeStartDate(
                da,
                "subtask",
                subtask.Id,
                shiftedStartDateTime.Date,
                subtask.Days,
                FormatHierarchyCascadeStartTime(shiftedStartDateTime),
                perDayHours,
                regId
            );
            AddOrUpdateHierarchyCascadeResult(savedItems, savedIndexes, saved);
            if (saved.EndDate > maxEndDate) maxEndDate = saved.EndDate;
        }

        return maxEndDate;
    }

    private static DateTime ShiftHierarchyMilestoneChildren(
        DataAccess da,
        int milestoneId,
        TimeSpan delta,
        string perDayHours,
        int regId,
        List<HierarchyCascadeSavedItem> savedItems,
        Dictionary<string, int> savedIndexes)
    {
        DateTime maxTaskEnd = DateTime.MinValue;
        List<HierarchyCascadeRow> tasks = LoadHierarchyCascadeSiblings(
            da, "task", milestoneId, regId
        );

        foreach (HierarchyCascadeRow task in tasks)
        {
            DateTime oldTaskStartDateTime = GetHierarchyCascadeStartDateTime(task);
            DateTime shiftedTaskStartDateTime = oldTaskStartDateTime.Add(delta);
            HierarchyCascadeSavedItem savedTask = SaveHierarchyCascadeStartDate(
                da,
                "task",
                task.Id,
                shiftedTaskStartDateTime.Date,
                task.Days,
                FormatHierarchyCascadeStartTime(shiftedTaskStartDateTime),
                perDayHours,
                regId
            );
            AddOrUpdateHierarchyCascadeResult(savedItems, savedIndexes, savedTask);

            DateTime maxSubtaskEnd = ShiftHierarchyTaskSubtasks(
                da,
                task.Id,
                delta,
                perDayHours,
                regId,
                savedItems,
                savedIndexes
            );

            if (maxSubtaskEnd != DateTime.MinValue && maxSubtaskEnd > savedTask.EndDate)
            {
                HierarchyCascadeSavedItem extendedTask = SaveHierarchyCascadeEndDate(
                    da, "task", task.Id, maxSubtaskEnd, regId
                );
                AddOrUpdateHierarchyCascadeResult(savedItems, savedIndexes, extendedTask);
                savedTask.EndDate = maxSubtaskEnd;
            }

            if (savedTask.EndDate > maxTaskEnd) maxTaskEnd = savedTask.EndDate;
        }

        return maxTaskEnd;
    }

    private static HierarchyCascadeRangeResult CascadeHierarchySubtasksAfter(
        DataAccess da,
        int taskId,
        int selectedSubtaskId,
        DateTime selectedCompletionDateTime,
        DateTime selectedBoundaryEndDate,
        string perDayHours,
        int regId,
        Dictionary<string, DateTime> completionOverrides,
        List<HierarchyCascadeSavedItem> savedItems,
        Dictionary<string, int> savedIndexes)
    {
        List<HierarchyCascadeRow> subtasks = LoadHierarchyCascadeSiblings(
            da, "subtask", taskId, regId
        );
        bool selectedFound = false;
        DateTime cursor = selectedCompletionDateTime;
        DateTime maxEndDate = selectedBoundaryEndDate.Date;

        foreach (HierarchyCascadeRow subtask in subtasks)
        {
            if (!selectedFound)
            {
                if (subtask.Id == selectedSubtaskId) selectedFound = true;
                continue;
            }

            DateTime rowStartDateTime = GetHierarchyCascadeStartDateTime(subtask);
            DateTime rowEndDate = subtask.EndDate;

            // Re-sequence every later sibling to the exact previous
            // completion date/time. This removes both overlaps and gaps.
            if (rowStartDateTime != cursor)
            {
                HierarchyCascadeSavedItem saved = SaveHierarchyCascadeStartDate(
                    da,
                    "subtask",
                    subtask.Id,
                    cursor.Date,
                    subtask.Days,
                    FormatHierarchyCascadeStartTime(cursor),
                    perDayHours,
                    regId
                );
                AddOrUpdateHierarchyCascadeResult(savedItems, savedIndexes, saved);
                rowEndDate = saved.EndDate;
            }

            if (rowEndDate > maxEndDate) maxEndDate = rowEndDate;

            DateTime completion = GetHierarchyEffectiveCompletionDateTime(
                da,
                "subtask",
                subtask.Id,
                perDayHours,
                regId,
                completionOverrides
            );
            if (completion > cursor) cursor = completion;
        }

        return new HierarchyCascadeRangeResult
        {
            CompletionDateTime = cursor,
            MaxEndDate = maxEndDate
        };
    }

    private static HierarchyCascadeRangeResult CascadeHierarchyTasksAfter(
        DataAccess da,
        int milestoneId,
        int selectedTaskId,
        DateTime selectedTaskCompletionDateTime,
        DateTime selectedTaskBoundaryEndDate,
        string perDayHours,
        int regId,
        Dictionary<string, DateTime> completionOverrides,
        List<HierarchyCascadeSavedItem> savedItems,
        Dictionary<string, int> savedIndexes)
    {
        List<HierarchyCascadeRow> tasks = LoadHierarchyCascadeSiblings(
            da, "task", milestoneId, regId
        );
        bool selectedFound = false;
        DateTime cursor = selectedTaskCompletionDateTime;
        DateTime maxEndDate = selectedTaskBoundaryEndDate.Date;

        foreach (HierarchyCascadeRow task in tasks)
        {
            if (!selectedFound)
            {
                if (task.Id == selectedTaskId) selectedFound = true;
                continue;
            }

            DateTime oldStartDateTime = GetHierarchyCascadeStartDateTime(task);
            DateTime taskEndDate = task.EndDate;

            // Re-sequence every later sibling to the exact previous
            // completion date/time. This removes both overlaps and gaps.
            if (oldStartDateTime != cursor)
            {
                DateTime newStartDateTime = cursor;
                TimeSpan delta = newStartDateTime - oldStartDateTime;
                HierarchyCascadeSavedItem savedTask = SaveHierarchyCascadeStartDate(
                    da,
                    "task",
                    task.Id,
                    newStartDateTime.Date,
                    task.Days,
                    FormatHierarchyCascadeStartTime(newStartDateTime),
                    perDayHours,
                    regId
                );
                AddOrUpdateHierarchyCascadeResult(savedItems, savedIndexes, savedTask);

                DateTime maxSubtaskEnd = ShiftHierarchyTaskSubtasks(
                    da,
                    task.Id,
                    delta,
                    perDayHours,
                    regId,
                    savedItems,
                    savedIndexes
                );

                if (maxSubtaskEnd != DateTime.MinValue && maxSubtaskEnd > savedTask.EndDate)
                {
                    HierarchyCascadeSavedItem extendedTask = SaveHierarchyCascadeEndDate(
                        da, "task", task.Id, maxSubtaskEnd, regId
                    );
                    AddOrUpdateHierarchyCascadeResult(savedItems, savedIndexes, extendedTask);
                    savedTask.EndDate = maxSubtaskEnd;
                }

                taskEndDate = savedTask.EndDate;
            }

            if (taskEndDate > maxEndDate) maxEndDate = taskEndDate;

            DateTime completion = GetHierarchyEffectiveCompletionDateTime(
                da,
                "task",
                task.Id,
                perDayHours,
                regId,
                completionOverrides
            );
            if (completion > cursor) cursor = completion;
        }

        return new HierarchyCascadeRangeResult
        {
            CompletionDateTime = cursor,
            MaxEndDate = maxEndDate
        };
    }

    private static HierarchyCascadeRangeResult CascadeHierarchyMilestonesAfter(
        DataAccess da,
        int projectId,
        int selectedMilestoneId,
        DateTime selectedMilestoneCompletionDateTime,
        DateTime selectedMilestoneBoundaryEndDate,
        string perDayHours,
        int regId,
        Dictionary<string, DateTime> completionOverrides,
        List<HierarchyCascadeSavedItem> savedItems,
        Dictionary<string, int> savedIndexes)
    {
        List<HierarchyCascadeRow> milestones = LoadHierarchyCascadeSiblings(
            da, "milestone", projectId, regId
        );
        bool selectedFound = false;
        DateTime cursor = selectedMilestoneCompletionDateTime;
        DateTime maxEndDate = selectedMilestoneBoundaryEndDate.Date;

        foreach (HierarchyCascadeRow milestone in milestones)
        {
            if (!selectedFound)
            {
                if (milestone.Id == selectedMilestoneId) selectedFound = true;
                continue;
            }

            DateTime oldStartDateTime = GetHierarchyCascadeStartDateTime(milestone);
            DateTime milestoneEndDate = milestone.EndDate;

            // Re-sequence every later sibling to the exact previous
            // completion date/time. This removes both overlaps and gaps.
            if (oldStartDateTime != cursor)
            {
                DateTime newStartDateTime = cursor;
                TimeSpan delta = newStartDateTime - oldStartDateTime;
                HierarchyCascadeSavedItem savedMilestone = SaveHierarchyCascadeStartDate(
                    da,
                    "milestone",
                    milestone.Id,
                    newStartDateTime.Date,
                    milestone.Days,
                    FormatHierarchyCascadeStartTime(newStartDateTime),
                    perDayHours,
                    regId
                );
                AddOrUpdateHierarchyCascadeResult(savedItems, savedIndexes, savedMilestone);

                DateTime maxTaskEnd = ShiftHierarchyMilestoneChildren(
                    da,
                    milestone.Id,
                    delta,
                    perDayHours,
                    regId,
                    savedItems,
                    savedIndexes
                );

                if (maxTaskEnd != DateTime.MinValue && maxTaskEnd > savedMilestone.EndDate)
                {
                    HierarchyCascadeSavedItem extendedMilestone = SaveHierarchyCascadeEndDate(
                        da, "milestone", milestone.Id, maxTaskEnd, regId
                    );
                    AddOrUpdateHierarchyCascadeResult(savedItems, savedIndexes, extendedMilestone);
                    savedMilestone.EndDate = maxTaskEnd;
                }

                milestoneEndDate = savedMilestone.EndDate;
            }

            if (milestoneEndDate > maxEndDate) maxEndDate = milestoneEndDate;

            DateTime completion = GetHierarchyEffectiveCompletionDateTime(
                da,
                "milestone",
                milestone.Id,
                perDayHours,
                regId,
                completionOverrides
            );
            if (completion > cursor) cursor = completion;
        }

        return new HierarchyCascadeRangeResult
        {
            CompletionDateTime = cursor,
            MaxEndDate = maxEndDate
        };
    }


    private static HierarchyCascadeRangeResult ResetHierarchyTaskSubtasks(
        DataAccess da,
        int taskId,
        DateTime startCursor,
        string perDayHours,
        int regId,
        Dictionary<string, DateTime> completionOverrides,
        List<HierarchyCascadeSavedItem> savedItems,
        Dictionary<string, int> savedIndexes)
    {
        DateTime cursor = DateTime.SpecifyKind(startCursor, DateTimeKind.Unspecified);
        DateTime maxEndDate = DateTime.MinValue;
        List<HierarchyCascadeRow> subtasks = LoadHierarchyCascadeSiblings(
            da, "subtask", taskId, regId, true
        );

        foreach (HierarchyCascadeRow subtask in subtasks)
        {
            string projectid = getprojectid(subtask.Id.ToString(), "subtask");
            HierarchyCascadeSavedItem saved = SaveHierarchyCascadeStartDate(
                da,
                "subtask",
                subtask.Id,
                cursor.Date,
                subtask.Days,
                FormatHierarchyCascadeStartTime(cursor),
                perDayHours,
                regId,
                true
            );
            
            AddOrUpdateHierarchyCascadeResult(savedItems, savedIndexes, saved);
            if (saved.EndDate > maxEndDate) maxEndDate = saved.EndDate;

            HierarchyCascadeRow savedRow = LoadHierarchyCascadeRow(
                da, "subtask", subtask.Id, regId
            );
            cursor = CalculateHierarchyWorkCompletionDateTime(savedRow, perDayHours,projectid);
            completionOverrides[GetHierarchyCascadeKey("subtask", subtask.Id)] = cursor;
        }

        return new HierarchyCascadeRangeResult
        {
            CompletionDateTime = cursor,
            MaxEndDate = maxEndDate
        };
    }

    private static HierarchyCascadeRangeResult ResetHierarchyMilestoneTasks(
        DataAccess da,
        int milestoneId,
        DateTime startCursor,
        string perDayHours,
        int regId,
        Dictionary<string, DateTime> completionOverrides,
        List<HierarchyCascadeSavedItem> savedItems,
        Dictionary<string, int> savedIndexes)
    {
        DateTime cursor = DateTime.SpecifyKind(startCursor, DateTimeKind.Unspecified);
        DateTime maxEndDate = DateTime.MinValue;
        List<HierarchyCascadeRow> tasks = LoadHierarchyCascadeSiblings(
            da, "task", milestoneId, regId, true
        );

        foreach (HierarchyCascadeRow task in tasks)
        {
            string projectid = getprojectid(task.Id.ToString(), "task");
            HierarchyCascadeSavedItem savedTask = SaveHierarchyCascadeStartDate(
                da,
                "task",
                task.Id,
                cursor.Date,
                task.Days,
                FormatHierarchyCascadeStartTime(cursor),
                perDayHours,
                regId,
                true
            );
            
            AddOrUpdateHierarchyCascadeResult(savedItems, savedIndexes, savedTask);

            HierarchyCascadeRow savedTaskRow = LoadHierarchyCascadeRow(
                da, "task", task.Id, regId
            );
            DateTime taskOwnCompletion = CalculateHierarchyWorkCompletionDateTime(
                savedTaskRow, perDayHours,projectid
            );
            completionOverrides[GetHierarchyCascadeKey("task", task.Id)] = taskOwnCompletion;

            HierarchyCascadeRangeResult subtaskReset = ResetHierarchyTaskSubtasks(
                da,
                task.Id,
                GetHierarchyCascadeStartDateTime(savedTaskRow),
                perDayHours,
                regId,
                completionOverrides,
                savedItems,
                savedIndexes
            );

            DateTime taskRequiredEnd = savedTask.EndDate;
            if (subtaskReset.MaxEndDate != DateTime.MinValue &&
                subtaskReset.MaxEndDate > taskRequiredEnd)
            {
                HierarchyCascadeSavedItem extendedTask = SaveHierarchyCascadeEndDate(
                    da, "task", task.Id, subtaskReset.MaxEndDate, regId
                );
                AddOrUpdateHierarchyCascadeResult(savedItems, savedIndexes, extendedTask);
                taskRequiredEnd = extendedTask.EndDate;
            }

            if (taskRequiredEnd > maxEndDate) maxEndDate = taskRequiredEnd;
            cursor = taskOwnCompletion;
        }

        return new HierarchyCascadeRangeResult
        {
            CompletionDateTime = cursor,
            MaxEndDate = maxEndDate
        };
    }

    private static HierarchyCascadeRangeResult ResetHierarchyTasksAfter(
        DataAccess da,
        int milestoneId,
        int selectedTaskId,
        DateTime selectedCompletionDateTime,
        DateTime selectedBoundaryEndDate,
        string perDayHours,
        int regId,
        Dictionary<string, DateTime> completionOverrides,
        List<HierarchyCascadeSavedItem> savedItems,
        Dictionary<string, int> savedIndexes)
    {
        List<HierarchyCascadeRow> tasks = LoadHierarchyCascadeSiblings(
            da, "task", milestoneId, regId, true
        );
        bool selectedFound = false;
        DateTime cursor = selectedCompletionDateTime;
        DateTime maxEndDate = selectedBoundaryEndDate.Date;

        foreach (HierarchyCascadeRow task in tasks)
        {
            if (!selectedFound)
            {
                if (task.Id == selectedTaskId) selectedFound = true;
                continue;
            }
            string projectid = getprojectid(task.Id.ToString(), "task");
            HierarchyCascadeSavedItem savedTask = SaveHierarchyCascadeStartDate(
                da,
                "task",
                task.Id,
                cursor.Date,
                task.Days,
                FormatHierarchyCascadeStartTime(cursor),
                perDayHours,
                regId,
                true
            );
            
            AddOrUpdateHierarchyCascadeResult(savedItems, savedIndexes, savedTask);

            HierarchyCascadeRow savedTaskRow = LoadHierarchyCascadeRow(
                da, "task", task.Id, regId
            );
            DateTime taskOwnCompletion = CalculateHierarchyWorkCompletionDateTime(
                savedTaskRow, perDayHours, projectid
            );
            completionOverrides[GetHierarchyCascadeKey("task", task.Id)] = taskOwnCompletion;

            HierarchyCascadeRangeResult subtaskReset = ResetHierarchyTaskSubtasks(
                da,
                task.Id,
                GetHierarchyCascadeStartDateTime(savedTaskRow),
                perDayHours,
                regId,
                completionOverrides,
                savedItems,
                savedIndexes
            );

            DateTime taskRequiredEnd = savedTask.EndDate;
            if (subtaskReset.MaxEndDate != DateTime.MinValue &&
                subtaskReset.MaxEndDate > taskRequiredEnd)
            {
                HierarchyCascadeSavedItem extendedTask = SaveHierarchyCascadeEndDate(
                    da, "task", task.Id, subtaskReset.MaxEndDate, regId
                );
                AddOrUpdateHierarchyCascadeResult(savedItems, savedIndexes, extendedTask);
                taskRequiredEnd = extendedTask.EndDate;
            }

            if (taskRequiredEnd > maxEndDate) maxEndDate = taskRequiredEnd;
            cursor = taskOwnCompletion;
        }

        return new HierarchyCascadeRangeResult
        {
            CompletionDateTime = cursor,
            MaxEndDate = maxEndDate
        };
    }

    private static HierarchyCascadeRangeResult ResetHierarchyMilestonesAfter(
        DataAccess da,
        int projectId,
        int selectedMilestoneId,
        DateTime selectedCompletionDateTime,
        DateTime selectedBoundaryEndDate,
        string perDayHours,
        int regId,
        Dictionary<string, DateTime> completionOverrides,
        List<HierarchyCascadeSavedItem> savedItems,
        Dictionary<string, int> savedIndexes)
    {
        List<HierarchyCascadeRow> milestones = LoadHierarchyCascadeSiblings(
            da, "milestone", projectId, regId, true
        );
        bool selectedFound = false;
        DateTime cursor = selectedCompletionDateTime;
        DateTime maxEndDate = selectedBoundaryEndDate.Date;

        foreach (HierarchyCascadeRow milestone in milestones)
        {
            if (!selectedFound)
            {
                if (milestone.Id == selectedMilestoneId) selectedFound = true;
                continue;
            }
            string projectid = getprojectid(milestone.Id.ToString(), "milestone");
            HierarchyCascadeSavedItem savedMilestone = SaveHierarchyCascadeStartDate(
                da,
                "milestone",
                milestone.Id,
                cursor.Date,
                milestone.Days,
                FormatHierarchyCascadeStartTime(cursor),
                perDayHours,
                regId,
                true
            );
            
            AddOrUpdateHierarchyCascadeResult(savedItems, savedIndexes, savedMilestone);

            HierarchyCascadeRow savedMilestoneRow = LoadHierarchyCascadeRow(
                da, "milestone", milestone.Id, regId
            );
            DateTime milestoneOwnCompletion = CalculateHierarchyWorkCompletionDateTime(
                savedMilestoneRow, perDayHours,projectid
            );
            completionOverrides[GetHierarchyCascadeKey("milestone", milestone.Id)] =
                milestoneOwnCompletion;

            HierarchyCascadeRangeResult taskReset = ResetHierarchyMilestoneTasks(
                da,
                milestone.Id,
                GetHierarchyCascadeStartDateTime(savedMilestoneRow),
                perDayHours,
                regId,
                completionOverrides,
                savedItems,
                savedIndexes
            );

            DateTime milestoneRequiredEnd = savedMilestone.EndDate;
            if (taskReset.MaxEndDate != DateTime.MinValue &&
                taskReset.MaxEndDate > milestoneRequiredEnd)
            {
                HierarchyCascadeSavedItem extendedMilestone = SaveHierarchyCascadeEndDate(
                    da, "milestone", milestone.Id, taskReset.MaxEndDate, regId
                );
                AddOrUpdateHierarchyCascadeResult(
                    savedItems, savedIndexes, extendedMilestone
                );
                milestoneRequiredEnd = extendedMilestone.EndDate;
            }

            if (milestoneRequiredEnd > maxEndDate) maxEndDate = milestoneRequiredEnd;
            cursor = milestoneOwnCompletion;
        }

        return new HierarchyCascadeRangeResult
        {
            CompletionDateTime = cursor,
            MaxEndDate = maxEndDate
        };
    }

    private static void SaveHierarchyCascadeHistory(
        DataAccess da,
        List<HierarchyCascadeSavedItem> savedItems,
        int regId,
        int userId)
    {
        foreach (HierarchyCascadeSavedItem item in savedItems)
        {
            if (item == null) continue;

            /*
             * A row whose original EndDate was NULL is being scheduled for the
             * first time by Reset Start Date; it is not an extension. The blank
             * database value is represented by DateTime.MinValue in the cascade
             * model. Trying to write that value to the extension-history table
             * can fail when its columns/procedure use SQL datetime, even though
             * the hierarchy dates have already been saved successfully.
             *
             * Skip history only for this initial blank-date case. Real extensions
             * of rows that already had a valid EndDate continue to be recorded.
             */
            if (item.OldEndDate == DateTime.MinValue ||
                item.OldEndDate < new DateTime(1753, 1, 1))
            {
                continue;
            }

            if (item.EndDate <= item.OldEndDate) continue;

            int historyId = SaveHierarchyExtensionHistory(
                da,
                item.Id,
                item.OldEndDate,
                item.EndDate,
                regId,
                userId,
                item.Type
            );

            if (historyId <= 0)
            {
                throw new Exception(
                    "Extension history was not saved for " + item.Type + "."
                );
            }
        }
    }

    private static string BuildHierarchyCascadeResponse(
        List<HierarchyCascadeSavedItem> savedItems)
    {
        List<string> details = new List<string>();
        foreach (HierarchyCascadeSavedItem item in savedItems)
        {
            details.Add(
                item.Type + "," +
                item.Id.ToString(CultureInfo.InvariantCulture) + "," +
                item.EndDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "," +
                (item.Days ?? String.Empty) + "," +
                item.StartDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "," +
                (item.StartTime ?? String.Empty)
            );
        }
        return "1|" + String.Join(";", details.ToArray());
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string ExtendProjectTimeline_new(
        string id = "",
        string extendStrtdate = "",
        string extendEnddate = "",
        string oldEnddate = "",
        string extendtype = "",
        string durationDays = "",
        string starttime = "",
        string perdayhrs = "",
        string changeSource = "")
    {
        try
        {
            int selectedId;
            int regId;
            int userId;

            if (!Int32.TryParse(id, out selectedId) || selectedId <= 0)
                return "0";

            HttpCookie companyCookie = HttpContext.Current.Request.Cookies["cadmin"];
            HttpCookie userCookie = HttpContext.Current.Request.Cookies["user"];

            if (companyCookie == null ||
                !Int32.TryParse(companyCookie["id"], out regId) ||
                regId <= 0)
            {
                return "0";
            }

            if (userCookie == null ||
                !Int32.TryParse(userCookie["user_id"], out userId))
            {
                userId = 0;
            }

            string selectedType = (extendtype ?? String.Empty)
                .Trim()
                .ToLowerInvariant();

            if (selectedType != "subtask" &&
                selectedType != "task" &&
                selectedType != "milestone" &&
                selectedType != "project")
            {
                return "0";
            }

            DateTime requestedStartDate = ParseAssignDateOnly(extendStrtdate).Date;
            DateTime requestedEndDate = ParseAssignDateOnly(extendEnddate).Date;
            string normalizedChangeSource = (changeSource ?? String.Empty)
                .Trim()
                .ToLowerInvariant();
            bool isResetChange = normalizedChangeSource == "reset";
            bool isStartDateChange = normalizedChangeSource == "startdate";
            bool isDurationChange = normalizedChangeSource == "duration";
            bool recalculateSelectedFromDuration =
                isStartDateChange || isDurationChange || isResetChange;

            if (requestedEndDate < requestedStartDate)
                return "0";

            if (recalculateSelectedFromDuration && selectedType != "project" &&
                String.IsNullOrWhiteSpace(durationDays))
            {
                return "0|Duration days cannot be blank.";
            }

            DataAccess da = new DataAccess();
            string hierarchySql;

            if (selectedType == "subtask")
            {
                hierarchySql =
                    "select st.id SubtaskId,st.Startdate SubtaskStartDate,st.Targetdate SubtaskEndDate," +
                    "t.id TaskId,t.Startdate TaskStartDate,t.Targetdate TaskEndDate," +
                    "m.id MilestoneId,m.StartDate MilestoneStartDate,m.EndDate MilestoneEndDate," +
                    "p.id ProjectId,p.StartDate ProjectStartDate,p.TargetDate ProjectEndDate " +
                    "from Pms_tbl_Addsubtask st with(nolock) " +
                    "inner join Pms_tbl_Addtask t with(nolock) on t.id=st.task and t.regid=st.regid " +
                    "inner join Pms_tbl_AddMilestone m with(nolock) on m.id=st.Milestoneid and m.regid=st.regid " +
                    "inner join Pms_tbl_AddProject p with(nolock) on p.id=st.ProjectId and p.regid=st.regid " +
                    "where st.status=1 and st.archive=0 and t.status=1 and t.archive=0 " +
                    "and m.status=1 and m.archive=0 and p.status=1 and p.archive=0 " +
                    "and st.regid=" + regId + " and st.id=" + selectedId;
            }
            else if (selectedType == "task")
            {
                hierarchySql =
                    "select t.id TaskId,t.Startdate TaskStartDate,t.Targetdate TaskEndDate," +
                    "m.id MilestoneId,m.StartDate MilestoneStartDate,m.EndDate MilestoneEndDate," +
                    "p.id ProjectId,p.StartDate ProjectStartDate,p.TargetDate ProjectEndDate " +
                    "from Pms_tbl_Addtask t with(nolock) " +
                    "inner join Pms_tbl_AddMilestone m with(nolock) on m.id=t.Milestoneid and m.regid=t.regid " +
                    "inner join Pms_tbl_AddProject p with(nolock) on p.id=t.ProjectId and p.regid=t.regid " +
                    "where t.status=1 and t.archive=0 and m.status=1 and m.archive=0 " +
                    "and p.status=1 and p.archive=0 and t.regid=" + regId +
                    " and t.id=" + selectedId;
            }
            else if (selectedType == "milestone")
            {
                hierarchySql =
                    "select m.id MilestoneId,m.StartDate MilestoneStartDate,m.EndDate MilestoneEndDate," +
                    "p.id ProjectId,p.StartDate ProjectStartDate,p.TargetDate ProjectEndDate " +
                    "from Pms_tbl_AddMilestone m with(nolock) " +
                    "inner join Pms_tbl_AddProject p with(nolock) on p.id=m.ProjectId and p.regid=m.regid " +
                    "where m.status=1 and m.archive=0 and p.status=1 and p.archive=0 " +
                    "and m.regid=" + regId + " and m.id=" + selectedId;
            }
            else
            {
                hierarchySql =
                    "select p.id ProjectId,p.StartDate ProjectStartDate,p.TargetDate ProjectEndDate " +
                    "from Pms_tbl_AddProject p with(nolock) " +
                    "where p.status=1 and p.archive=0 and p.regid=" + regId +
                    " and p.id=" + selectedId;
            }

            DataSet hierarchyData = da.GetDataSetInline(hierarchySql);
            if (hierarchyData.Tables.Count == 0 ||
                hierarchyData.Tables[0].Rows.Count == 0)
            {
                return "0";
            }

            DataRow hierarchyRow = hierarchyData.Tables[0].Rows[0];
            int projectId = hierarchyRow["ProjectId"] == DBNull.Value
                ? 0
                : Convert.ToInt32(hierarchyRow["ProjectId"], CultureInfo.InvariantCulture);

            // Reset Start Date can be used while later hierarchy dates are still
            // NULL. Do not cast a database NULL directly to DateTime; represent a
            // missing old date as DateTime.MinValue so the cascade can populate it.
            DateTime projectOldEnd = GetHierarchyDateOrMinValue(
                hierarchyRow, "ProjectEndDate"
            );

            int milestoneId = hierarchyRow.Table.Columns.Contains("MilestoneId") &&
                hierarchyRow["MilestoneId"] != DBNull.Value
                ? Convert.ToInt32(hierarchyRow["MilestoneId"], CultureInfo.InvariantCulture)
                : 0;
            DateTime milestoneOldEnd = milestoneId > 0
                ? GetHierarchyDateOrMinValue(hierarchyRow, "MilestoneEndDate")
                : DateTime.MinValue;

            int taskId = hierarchyRow.Table.Columns.Contains("TaskId") &&
                hierarchyRow["TaskId"] != DBNull.Value
                ? Convert.ToInt32(hierarchyRow["TaskId"], CultureInfo.InvariantCulture)
                : 0;
            DateTime taskOldEnd = taskId > 0
                ? GetHierarchyDateOrMinValue(hierarchyRow, "TaskEndDate")
                : DateTime.MinValue;

            List<HierarchyCascadeSavedItem> savedItems =
                new List<HierarchyCascadeSavedItem>();
            Dictionary<string, int> savedIndexes =
                new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            if (selectedType == "project")
            {
                if (requestedEndDate <= projectOldEnd)
                {
                    return "1|project," + projectId.ToString(CultureInfo.InvariantCulture) + "," +
                        projectOldEnd.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + ",," +
                        Convert.ToDateTime(hierarchyRow["ProjectStartDate"], CultureInfo.InvariantCulture)
                            .Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + ",";
                }

                HierarchyCascadeSavedItem savedProject = SaveHierarchyCascadeProjectEndDate(
                    da, projectId, requestedEndDate, regId
                );
                AddOrUpdateHierarchyCascadeResult(savedItems, savedIndexes, savedProject);
                SaveHierarchyCascadeHistory(da, savedItems, regId, userId);
                return BuildHierarchyCascadeResponse(savedItems);
            }

            HierarchyCascadeRow selectedOldRow = LoadHierarchyCascadeRow(
                da, selectedType, selectedId, regId, isResetChange
            );
            HierarchyCascadeSavedItem selectedSaved;

            if (recalculateSelectedFromDuration)
            {
                selectedSaved = SaveHierarchyCascadeStartDate(
                    da,
                    selectedType,
                    selectedId,
                    requestedStartDate,
                    durationDays,
                    starttime,
                    perdayhrs,
                    regId,
                    isResetChange
                );
            }
            else
            {
                selectedSaved = SaveHierarchyCascadeEndDate(
                    da,
                    selectedType,
                    selectedId,
                    requestedEndDate,
                    regId
                );
            }
            AddOrUpdateHierarchyCascadeResult(savedItems, savedIndexes, selectedSaved);

            Dictionary<string, DateTime> completionOverrides =
                new Dictionary<string, DateTime>(StringComparer.OrdinalIgnoreCase);

            // Forward sequencing must use the same hour-based completion cursor
            // as Reset Start Date. An edited/extended display EndDate controls the
            // selected row and parent timeline boundary only; it must not become
            // the start cursor for the next subtask, task or milestone.
            //
            // Example: a milestone starting 30-Jul with 20 working hours and an
            // 8-hour day completes on 01-Aug. Even when its display EndDate is
            // extended to 10-Aug by a child task, the next milestone starts from
            // the actual 01-Aug working completion date/time.

            DateTime currentTaskEnd = taskOldEnd;
            DateTime currentMilestoneEnd = milestoneOldEnd;
            DateTime finalHierarchyEnd = selectedSaved.EndDate;

            if (selectedType == "subtask")
            {
                DateTime selectedSubtaskCompletion = GetHierarchyEffectiveCompletionDateTime(
                    da,
                    "subtask",
                    selectedId,
                    perdayhrs,
                    regId,
                    completionOverrides
                );

                HierarchyCascadeRangeResult subtaskCascade = CascadeHierarchySubtasksAfter(
                    da,
                    taskId,
                    selectedId,
                    selectedSubtaskCompletion,
                    selectedSaved.EndDate,
                    perdayhrs,
                    regId,
                    completionOverrides,
                    savedItems,
                    savedIndexes
                );

                DateTime requiredTaskEnd = selectedSaved.EndDate;
                if (subtaskCascade.MaxEndDate > requiredTaskEnd)
                    requiredTaskEnd = subtaskCascade.MaxEndDate;

                if (requiredTaskEnd > taskOldEnd)
                {
                    HierarchyCascadeSavedItem savedTask = SaveHierarchyCascadeEndDate(
                        da, "task", taskId, requiredTaskEnd, regId
                    );
                    AddOrUpdateHierarchyCascadeResult(savedItems, savedIndexes, savedTask);
                    currentTaskEnd = savedTask.EndDate;
                }
                else
                {
                    currentTaskEnd = taskOldEnd;
                }

                DateTime parentTaskCompletion = GetHierarchyEffectiveCompletionDateTime(
                    da,
                    "task",
                    taskId,
                    perdayhrs,
                    regId,
                    completionOverrides
                );

                HierarchyCascadeRangeResult taskCascade = isResetChange
                    ? ResetHierarchyTasksAfter(
                        da,
                        milestoneId,
                        taskId,
                        parentTaskCompletion,
                        currentTaskEnd,
                        perdayhrs,
                        regId,
                        completionOverrides,
                        savedItems,
                        savedIndexes
                    )
                    : CascadeHierarchyTasksAfter(
                        da,
                        milestoneId,
                        taskId,
                        parentTaskCompletion,
                        currentTaskEnd,
                        perdayhrs,
                        regId,
                        completionOverrides,
                        savedItems,
                        savedIndexes
                    );

                finalHierarchyEnd = currentTaskEnd;
                if (taskCascade.MaxEndDate > finalHierarchyEnd)
                    finalHierarchyEnd = taskCascade.MaxEndDate;
            }
            else if (selectedType == "task")
            {
                currentTaskEnd = selectedSaved.EndDate;

                if (isResetChange)
                {
                    HierarchyCascadeRow selectedCurrentRow = LoadHierarchyCascadeRow(
                        da, "task", selectedId, regId
                    );
                    HierarchyCascadeRangeResult selectedTaskSubtasks =
                        ResetHierarchyTaskSubtasks(
                            da,
                            selectedId,
                            GetHierarchyCascadeStartDateTime(selectedCurrentRow),
                            perdayhrs,
                            regId,
                            completionOverrides,
                            savedItems,
                            savedIndexes
                        );

                    if (selectedTaskSubtasks.MaxEndDate != DateTime.MinValue &&
                        selectedTaskSubtasks.MaxEndDate > currentTaskEnd)
                    {
                        HierarchyCascadeSavedItem extendedSelectedTask =
                            SaveHierarchyCascadeEndDate(
                                da,
                                "task",
                                selectedId,
                                selectedTaskSubtasks.MaxEndDate,
                                regId
                            );
                        AddOrUpdateHierarchyCascadeResult(
                            savedItems, savedIndexes, extendedSelectedTask
                        );
                        currentTaskEnd = extendedSelectedTask.EndDate;
                    }
                }
                else if (isStartDateChange)
                {
                    HierarchyCascadeRow selectedCurrentRow = LoadHierarchyCascadeRow(
                        da, "task", selectedId, regId
                    );
                    TimeSpan selectedTaskDelta =
                        GetHierarchyCascadeStartDateTime(selectedCurrentRow) -
                        GetHierarchyCascadeStartDateTime(selectedOldRow);

                    if (selectedTaskDelta != TimeSpan.Zero)
                    {
                        DateTime maxSelectedTaskSubtaskEnd = ShiftHierarchyTaskSubtasks(
                            da,
                            selectedId,
                            selectedTaskDelta,
                            perdayhrs,
                            regId,
                            savedItems,
                            savedIndexes
                        );

                        if (maxSelectedTaskSubtaskEnd != DateTime.MinValue &&
                            maxSelectedTaskSubtaskEnd > currentTaskEnd)
                        {
                            HierarchyCascadeSavedItem extendedSelectedTask =
                                SaveHierarchyCascadeEndDate(
                                    da,
                                    "task",
                                    selectedId,
                                    maxSelectedTaskSubtaskEnd,
                                    regId
                                );
                            AddOrUpdateHierarchyCascadeResult(
                                savedItems, savedIndexes, extendedSelectedTask
                            );
                            currentTaskEnd = extendedSelectedTask.EndDate;
                        }
                    }
                }

                DateTime selectedTaskCompletion = GetHierarchyEffectiveCompletionDateTime(
                    da,
                    "task",
                    selectedId,
                    perdayhrs,
                    regId,
                    completionOverrides
                );

                HierarchyCascadeRangeResult taskCascade = isResetChange
                    ? ResetHierarchyTasksAfter(
                        da,
                        milestoneId,
                        selectedId,
                        selectedTaskCompletion,
                        currentTaskEnd,
                        perdayhrs,
                        regId,
                        completionOverrides,
                        savedItems,
                        savedIndexes
                    )
                    : CascadeHierarchyTasksAfter(
                        da,
                        milestoneId,
                        selectedId,
                        selectedTaskCompletion,
                        currentTaskEnd,
                        perdayhrs,
                        regId,
                        completionOverrides,
                        savedItems,
                        savedIndexes
                    );

                finalHierarchyEnd = currentTaskEnd;
                if (taskCascade.MaxEndDate > finalHierarchyEnd)
                    finalHierarchyEnd = taskCascade.MaxEndDate;
            }
            else if (selectedType == "milestone")
            {
                currentMilestoneEnd = selectedSaved.EndDate;

                if (isResetChange)
                {
                    HierarchyCascadeRow selectedCurrentRow = LoadHierarchyCascadeRow(
                        da, "milestone", selectedId, regId
                    );
                    HierarchyCascadeRangeResult selectedMilestoneTasks =
                        ResetHierarchyMilestoneTasks(
                            da,
                            selectedId,
                            GetHierarchyCascadeStartDateTime(selectedCurrentRow),
                            perdayhrs,
                            regId,
                            completionOverrides,
                            savedItems,
                            savedIndexes
                        );

                    if (selectedMilestoneTasks.MaxEndDate != DateTime.MinValue &&
                        selectedMilestoneTasks.MaxEndDate > currentMilestoneEnd)
                    {
                        HierarchyCascadeSavedItem extendedSelectedMilestone =
                            SaveHierarchyCascadeEndDate(
                                da,
                                "milestone",
                                selectedId,
                                selectedMilestoneTasks.MaxEndDate,
                                regId
                            );
                        AddOrUpdateHierarchyCascadeResult(
                            savedItems, savedIndexes, extendedSelectedMilestone
                        );
                        currentMilestoneEnd = extendedSelectedMilestone.EndDate;
                    }
                }
                else if (isStartDateChange)
                {
                    HierarchyCascadeRow selectedCurrentRow = LoadHierarchyCascadeRow(
                        da, "milestone", selectedId, regId
                    );
                    TimeSpan selectedMilestoneDelta =
                        GetHierarchyCascadeStartDateTime(selectedCurrentRow) -
                        GetHierarchyCascadeStartDateTime(selectedOldRow);

                    if (selectedMilestoneDelta != TimeSpan.Zero)
                    {
                        DateTime maxSelectedMilestoneTaskEnd = ShiftHierarchyMilestoneChildren(
                            da,
                            selectedId,
                            selectedMilestoneDelta,
                            perdayhrs,
                            regId,
                            savedItems,
                            savedIndexes
                        );

                        if (maxSelectedMilestoneTaskEnd != DateTime.MinValue &&
                            maxSelectedMilestoneTaskEnd > currentMilestoneEnd)
                        {
                            HierarchyCascadeSavedItem extendedSelectedMilestone =
                                SaveHierarchyCascadeEndDate(
                                    da,
                                    "milestone",
                                    selectedId,
                                    maxSelectedMilestoneTaskEnd,
                                    regId
                                );
                            AddOrUpdateHierarchyCascadeResult(
                                savedItems, savedIndexes, extendedSelectedMilestone
                            );
                            currentMilestoneEnd = extendedSelectedMilestone.EndDate;
                        }
                    }
                }

                finalHierarchyEnd = currentMilestoneEnd;
            }

            if (selectedType == "subtask" || selectedType == "task")
            {
                DateTime requiredMilestoneEnd = milestoneOldEnd;
                if (finalHierarchyEnd > requiredMilestoneEnd)
                    requiredMilestoneEnd = finalHierarchyEnd;

                if (requiredMilestoneEnd > milestoneOldEnd)
                {
                    HierarchyCascadeSavedItem savedMilestone =
                        SaveHierarchyCascadeEndDate(
                            da,
                            "milestone",
                            milestoneId,
                            requiredMilestoneEnd,
                            regId
                        );
                    AddOrUpdateHierarchyCascadeResult(
                        savedItems, savedIndexes, savedMilestone
                    );
                    currentMilestoneEnd = savedMilestone.EndDate;
                }
                else
                {
                    currentMilestoneEnd = milestoneOldEnd;
                }
            }

            int cascadeMilestoneId = selectedType == "milestone"
                ? selectedId
                : milestoneId;

            if (currentMilestoneEnd == DateTime.MinValue)
                currentMilestoneEnd = selectedSaved.EndDate;

            DateTime selectedMilestoneCompletion = GetHierarchyEffectiveCompletionDateTime(
                da,
                "milestone",
                cascadeMilestoneId,
                perdayhrs,
                regId,
                completionOverrides
            );

            /*
             * Confirmed Start Date cascade (date-only rule):
             * When a Subtask/Task start-date change extends its Milestone end,
             * the next Milestone must begin on the calendar date immediately
             * after that new Milestone end. Keep every existing duration-day and
             * working-hour value unchanged; only start/end dates are moved.
             *
             * Example: T2/ST4 finishes on 13-Aug, therefore M2 starts 14-Aug.
             * This is intentionally limited to the Yes-confirmed Start Date path
             * and does not change normal edits or End Date/Duration behaviour.
             */
            bool useConfirmedDateOnlyMilestoneCursor =
                isStartDateChange &&
                (selectedType == "subtask" || selectedType == "task") &&
                currentMilestoneEnd > milestoneOldEnd;

            if (useConfirmedDateOnlyMilestoneCursor)
            {
                HierarchyCascadeRow cascadeMilestoneRow = LoadHierarchyCascadeRow(
                    da, "milestone", cascadeMilestoneId, regId
                );
                selectedMilestoneCompletion = DateTime.SpecifyKind(
                    currentMilestoneEnd.Date.AddDays(1).Add(
                        GetHierarchyCascadeStartDateTime(cascadeMilestoneRow).TimeOfDay
                    ),
                    DateTimeKind.Unspecified
                );
            }

            HierarchyCascadeRangeResult milestoneCascade = isResetChange
                ? ResetHierarchyMilestonesAfter(
                    da,
                    projectId,
                    cascadeMilestoneId,
                    selectedMilestoneCompletion,
                    currentMilestoneEnd,
                    perdayhrs,
                    regId,
                    completionOverrides,
                    savedItems,
                    savedIndexes
                )
                : CascadeHierarchyMilestonesAfter(
                    da,
                    projectId,
                    cascadeMilestoneId,
                    selectedMilestoneCompletion,
                    currentMilestoneEnd,
                    perdayhrs,
                    regId,
                    completionOverrides,
                    savedItems,
                    savedIndexes
                );

            DateTime requiredProjectEnd = projectOldEnd;
            if (currentMilestoneEnd > requiredProjectEnd)
                requiredProjectEnd = currentMilestoneEnd;
            if (milestoneCascade.MaxEndDate > requiredProjectEnd)
                requiredProjectEnd = milestoneCascade.MaxEndDate;

            if (requiredProjectEnd > projectOldEnd)
            {
                HierarchyCascadeSavedItem savedProject =
                    SaveHierarchyCascadeProjectEndDate(
                        da,
                        projectId,
                        requiredProjectEnd,
                        regId
                    );
                AddOrUpdateHierarchyCascadeResult(
                    savedItems, savedIndexes, savedProject
                );
            }

            SaveHierarchyCascadeHistory(da, savedItems, regId, userId);
            return BuildHierarchyCascadeResponse(savedItems);
        }
        catch (Exception ex)
        {
            SaveLogText("ExtendProjectTimeline_newEX-" + ex.Message);
            return "0|" + ex.Message;
        }
    }



    #endregion
    #region update timeline
    public static string getprojectid(string id,string utype)
    {
        string ret = "0";
        DataAccess da = new DataAccess();
        string tblname = "";
        if (utype == "milestone")
        {
            tblname = "Pms_tbl_AddMilestone";
        }
        else if (utype == "task")
        {
            tblname = "Pms_tbl_Addtask";
        }
        else if (utype == "subtask")
        {
            tblname = "Pms_tbl_Addsubtask";
        }
        if (tblname != string.Empty)
        {
            try
            {
                string strsql = "select id,projectid from " + tblname + " where id='" + id + "' and status=1 and archive=0 ";
                DataSet dsS = da.GetDataSetInline(strsql);
                string txtday = "0";
                try
                {
                    ret = dsS.Tables[0].Rows[0]["projectid"].ToString();
                    
                }
                catch { }
            }
            catch { }
        }
                return ret;
    }
    [WebMethod]
    public static string updatetimeline_mile(string id, string ndays, string startdate, string starttime, string perdayhrs, string calculateddays)
    {
        string retval = null;
        DataAccess da = new DataAccess();
        string empid = "0";
        string projectid = getprojectid(id,"milestone");

        try
        {
            string regid = HttpContext.Current.Request.Cookies["cadmin"]["id"];
            string capacityValidation = ValidateStoredDurationAgainstDays(
                da, "Pms_tbl_AddMilestone", "Milestone", id, regid, ndays, perdayhrs);
            if (capacityValidation != "1")
                return "ERROR|" + capacityValidation;

            DateTime startDateValue = ParseAssignDateOnly(startdate);
            DateTime endDateValue = startDateValue;
            int newday = 0;
            string isweekoff = "0";
            string isholyday = "0";
            if (!string.IsNullOrWhiteSpace(projectid) && projectid != "0")
            {
                string weekoffholyday = check_weekoffholyday(projectid);
                SaveLogText("weekoffholyday2-" + weekoffholyday);
                if (weekoffholyday != string.Empty)
                {
                    string[] aroff = weekoffholyday.Split(',');
                    try
                    {
                        isholyday = aroff[0];
                        isweekoff = aroff[1];
                    }
                    catch { }
                }
            }

            // EndDate is always controlled by the saved duration-days value.
            // calculateddays is retained in the WebMethod signature only for
            // backward compatibility; hour-based reset sequencing must never
            // override the item's declared txtdays.
            string dateDaysText = ndays;

            if (!string.IsNullOrWhiteSpace(dateDaysText))
            {
                newday = Math.Max(
                    0,
                    Convert.ToInt32(Math.Ceiling(Convert.ToDouble(dateDaysText, CultureInfo.InvariantCulture)))
                );

                if (newday > 0)
                {
                    DateTime checkDate = startDateValue;

                    for (int i = 0; i < newday; i++)
                    {
                        if (i > 0)
                            checkDate = checkDate.AddDays(1);

                        if (IsProjectExcludedCalendarDate(
                            checkDate, empid, isweekoff, isholyday))
                        {
                            newday += 1;
                        }
                    }

                    endDateValue = startDateValue.AddDays(newday - 1);
                }
                else
                {
                    // Zero duration days is valid; keep EndDate on StartDate.
                    endDateValue = startDateValue;
                }
                //endDateValue = startDateValue.AddDays(newday);

                string startText = startDateValue.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                string endText = endDateValue.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);


                string startTimeSql = "";
                if (!String.IsNullOrWhiteSpace(starttime))
                {
                    DateTime parsedStartTime = DateTime.Today.Add(
                        ParseTimeOfDayInvariant(starttime, "09:00 AM")
                    );
                    startTimeSql = ",StartTime=N'" +
                        parsedStartTime.ToString("hh:mm tt", CultureInfo.InvariantCulture)
                            .Replace("'", "''") + "'";
                }

                string strsqltm = "update Pms_tbl_AddMilestone set startdate=convert(date,'" + ToSqlDateOnly(startDateValue) + "',112),EndDate=convert(date,'" + ToSqlDateOnly(endDateValue) + "',112),txtdays='" + (ndays == "" ? "0" : ndays) + "'" + startTimeSql + " where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + id + "'";
                SaveLogText("strsqltm-" + strsqltm);
                da.executeDMLQueryinline(strsqltm);
                retval = endText;
            }
        }
        catch (Exception ex)
        {
            SaveLogText("updatetimeline_mileEX-" + ex.Message);
        }

        return retval;
    }
    [WebMethod]
    public static string updatetimeline_task(string id, string ndays, string startdate, string starttime, string perdayhrs, string calculateddays)
    {
        string retval = null;
        DataAccess da = new DataAccess();
        string empid = "0";
        string projectid = getprojectid(id, "task");

        try
        {
            string regid = HttpContext.Current.Request.Cookies["cadmin"]["id"];
            string capacityValidation = ValidateStoredDurationAgainstDays(
                da, "Pms_tbl_AddTask", "taskname", id, regid, ndays, perdayhrs);
            if (capacityValidation != "1")
                return "ERROR|" + capacityValidation;

            DateTime startDateValue = ParseAssignDateOnly(startdate);
            DateTime endDateValue = startDateValue;
            int newday = 0;
            string isweekoff = "0";
            string isholyday = "0";
            if (!string.IsNullOrWhiteSpace(projectid) && projectid != "0")
            {
                string weekoffholyday = check_weekoffholyday(projectid);
                SaveLogText("weekoffholyday3-" + weekoffholyday);
                if (weekoffholyday != string.Empty)
                {
                    string[] aroff = weekoffholyday.Split(',');
                    try
                    {
                        isholyday = aroff[0];
                        isweekoff = aroff[1];
                    }
                    catch { }
                }
            }
            // EndDate is always controlled by the saved duration-days value.
            // calculateddays is retained in the WebMethod signature only for
            // backward compatibility; hour-based reset sequencing must never
            // override the item's declared txtdays.
            string dateDaysText = ndays;

            if (!string.IsNullOrWhiteSpace(dateDaysText))
            {
                newday = Math.Max(
                    0,
                    Convert.ToInt32(Math.Ceiling(Convert.ToDouble(dateDaysText, CultureInfo.InvariantCulture)))
                );

                if (newday > 0)
                {
                    DateTime checkDate = startDateValue;

                    for (int i = 0; i < newday; i++)
                    {
                        if (i > 0)
                            checkDate = checkDate.AddDays(1);

                        if (IsProjectExcludedCalendarDate(
                            checkDate, empid, isweekoff, isholyday))
                        {
                            newday += 1;
                        }
                    }

                    endDateValue = startDateValue.AddDays(newday - 1);
                }
                else
                {
                    // Zero duration days is valid; keep EndDate on StartDate.
                    endDateValue = startDateValue;
                }
                //endDateValue = startDateValue.AddDays(newday);

                string startText = startDateValue.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                string endText = endDateValue.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);


                string startTimeSql = "";
                if (!String.IsNullOrWhiteSpace(starttime))
                {
                    DateTime parsedStartTime = DateTime.Today.Add(
                        ParseTimeOfDayInvariant(starttime, "09:00 AM")
                    );
                    startTimeSql = ",StartTime=N'" +
                        parsedStartTime.ToString("hh:mm tt", CultureInfo.InvariantCulture)
                            .Replace("'", "''") + "'";
                }

                string strsqltm = "update Pms_tbl_Addtask set startdate=convert(date,'" + ToSqlDateOnly(startDateValue) + "',112),targetdate=convert(date,'" + ToSqlDateOnly(endDateValue) + "',112),txtdays='" + (ndays == "" ? "0" : ndays) + "'" + startTimeSql + " where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + id + "'";
                SaveLogText("strsqltm-" + strsqltm);
                da.executeDMLQueryinline(strsqltm);
                retval = endText;
            }
        }
        catch (Exception ex)
        {
            SaveLogText("updatetimeline_taskEX-" + ex.Message);
        }

        return retval;
    }
    [WebMethod]
    public static string updatetimeline_subtask(string id, string ndays, string startdate, string starttime, string perdayhrs, string calculateddays)
    {
        string retval = null;
        DataAccess da = new DataAccess();
        string empid = "0";
        string projectid = getprojectid(id, "subtask");
        try
        {
            string regid = HttpContext.Current.Request.Cookies["cadmin"]["id"];
            string capacityValidation = ValidateStoredDurationAgainstDays(
                da, "Pms_tbl_AddSubTask", "Subtaskname", id, regid, ndays, perdayhrs);
            if (capacityValidation != "1")
                return "ERROR|" + capacityValidation;

            DateTime startDateValue = ParseAssignDateOnly(startdate);
            DateTime endDateValue = startDateValue;
            int newday = 0;
            string isweekoff = "0";
            string isholyday = "0";
            
            if (!string.IsNullOrWhiteSpace(projectid) && projectid != "0")
            {
                string weekoffholyday = check_weekoffholyday(projectid);
                SaveLogText("weekoffholyday4-" + weekoffholyday);
                if (weekoffholyday != string.Empty)
                {
                    string[] aroff = weekoffholyday.Split(',');
                    try
                    {
                        isholyday = aroff[0];
                        isweekoff = aroff[1];
                    }
                    catch { }
                }
            }
            // EndDate is always controlled by the saved duration-days value.
            // calculateddays is retained in the WebMethod signature only for
            // backward compatibility; hour-based reset sequencing must never
            // override the item's declared txtdays.
            string dateDaysText = ndays;

            if (!string.IsNullOrWhiteSpace(dateDaysText))
            {
                newday = Math.Max(
                    0,
                    Convert.ToInt32(Math.Ceiling(Convert.ToDouble(dateDaysText, CultureInfo.InvariantCulture)))
                );

                if (newday > 0)
                {
                    DateTime checkDate = startDateValue;

                    for (int i = 0; i < newday; i++)
                    {
                        if (i > 0)
                            checkDate = checkDate.AddDays(1);

                        if (IsProjectExcludedCalendarDate(
                            checkDate, empid, isweekoff, isholyday))
                        {
                            newday += 1;
                        }
                    }

                    endDateValue = startDateValue.AddDays(newday - 1);
                }
                else
                {
                    // Zero duration days is valid; keep EndDate on StartDate.
                    endDateValue = startDateValue;
                }
                //endDateValue = startDateValue.AddDays(newday);

                string startText = startDateValue.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                string endText = endDateValue.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);


                string startTimeSql = "";
                if (!String.IsNullOrWhiteSpace(starttime))
                {
                    DateTime parsedStartTime = DateTime.Today.Add(
                        ParseTimeOfDayInvariant(starttime, "09:00 AM")
                    );
                    startTimeSql = ",StartTime=N'" +
                        parsedStartTime.ToString("hh:mm tt", CultureInfo.InvariantCulture)
                            .Replace("'", "''") + "'";
                }

                string strsqltm = "update Pms_tbl_Addsubtask set startdate=convert(date,'" + ToSqlDateOnly(startDateValue) + "',112),targetdate=convert(date,'" + ToSqlDateOnly(endDateValue) + "',112),txtdays='" + (ndays == "" ? "0" : ndays) + "'" + startTimeSql + " where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + id + "'";
                SaveLogText("strsqltm-" + strsqltm);
                da.executeDMLQueryinline(strsqltm);
                retval = endText;
            }
        }
        catch (Exception ex)
        {
            SaveLogText("updatetimeline_subtaskEX-" + ex.Message);
        }

        return retval;
    }

    [WebMethod]
    public static string getleftdays(string id,string gtype)
    {
        string retval = "";

        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        string strsql = "";
        if (gtype == "subtask")
        {
            strsql = "select id,Startdate,Targetdate,team,owner from Pms_tbl_Addsubtask where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + id + "'";
        }
        else if (gtype == "task")
        {
            strsql = "select id,Startdate,Targetdate,team,owner from pms_tbl_addtask where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'   and id='" +id + "'";
        }
        else if (gtype=="milestone")
        {
            strsql = "select id,StartDate,EndDate as Targetdate,team,owner from Pms_tbl_AddMilestone where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  and id='" + id + "'";
        }
        string tleftdays = "";
        #region bind timeline
        //Timeline
        if (strsql != "")
        {
            DataSet dsD = da.GetDataSetInline(strsql);
            double miledayscomplite = 0;


            DateTime mileStartDate = GetNullableDate(dsD.Tables[0].Rows[0]["StartDate"]) ?? DateTime.Today;
            DateTime mileTargetdate = GetNullableDate(dsD.Tables[0].Rows[0]["Targetdate"]) ?? DateTime.Today;
            TimeSpan mileobjTimeSpan = mileTargetdate - mileStartDate;
            double mileDays = Convert.ToDouble(mileobjTimeSpan.TotalDays);
            mileDays = mileDays + 1;


            DateTime mileEnddate = mileTargetdate;
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
                mileleftDays = mileleftDays +1;
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
    public static string getleftdays_load(string id,string gtype)
    {
        string retval = "";

        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        string strsql = "";
        try
        {
            if (gtype == "subtask")
            {
                strsql = "select id,Startdate,Targetdate,team,owner from Pms_tbl_Addsubtask where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id='" + id + "'";
            }
            else if (gtype == "task")
            {
                strsql = "select id,Startdate,Targetdate,team,owner from pms_tbl_addtask where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'   and id='" + id + "'";
            }
            else if (gtype == "milestone")
            {
                strsql = "select id,StartDate,EndDate as Targetdate,team,owner from Pms_tbl_AddMilestone where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  and id='" + id + "'";
            }
            string tleftdays = "";
            #region bind timeline
            //Timeline
            if (strsql != "")
            {
                DataSet dsD = da.GetDataSetInline(strsql);
                double miledayscomplite = 0;


                DateTime mileStartDate = GetNullableDate(dsD.Tables[0].Rows[0]["StartDate"]) ?? DateTime.Today;
                DateTime mileTargetdate = GetNullableDate(dsD.Tables[0].Rows[0]["Targetdate"]) ?? DateTime.Today;
                TimeSpan mileobjTimeSpan = mileTargetdate - mileStartDate;
                double mileDays = Convert.ToDouble(mileobjTimeSpan.TotalDays);
                mileDays = mileDays + 1;


                DateTime mileEnddate = mileTargetdate;
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
        }
        catch { }
        return retval;
    }
    [WebMethod]
    public static string manage_enddate(string ndays, string startdate,string pid)
    {
        try
        {
            DateTime startDateValue = ParseAssignDateOnly(startdate);
            DateTime endDateValue = startDateValue;
            string empid = "0";
            string projectid = pid;
            if (!string.IsNullOrWhiteSpace(ndays) && ndays != "0")
            {
                int newday = Convert.ToInt32(
                    Math.Ceiling(Convert.ToDouble(ndays, CultureInfo.InvariantCulture))
                );
                string isweekoff = "0";
                string isholyday = "0";
               
                if (!string.IsNullOrWhiteSpace(projectid) && projectid != "0")
                {
                    //SaveLogText("weekoffholyday0-ok-"+ HttpContext.Current.Request.QueryString["id"].ToString());
                    string weekoffholyday = check_weekoffholyday(projectid);
                    SaveLogText("weekoffholyday1-"+ weekoffholyday);
                    if (weekoffholyday != string.Empty)
                    {
                        string[] aroff = weekoffholyday.Split(',');
                        try
                        {
                            isholyday = aroff[0];
                            isweekoff = aroff[1];
                        }
                        catch { }
                    }
                }
                DateTime checkDate = startDateValue;
                for (int i = 0; i < newday; i++)
                {
                    if (i > 0)
                        checkDate = checkDate.AddDays(1);

                    if (IsProjectExcludedCalendarDate(
                        checkDate, empid, isweekoff, isholyday))
                    {
                        newday += 1;
                    }
                }

                endDateValue = startDateValue.AddDays(newday - 1);
                //endDateValue = startDateValue.AddDays(newday);
            }

            // Return a date-only string. Returning DateTime makes ASP.NET serialize
            // /Date(milliseconds)/ and browsers can display the previous/next day.
            return endDateValue.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
        catch (Exception ex)
        {
            SaveLogText("manage_enddateEX-" + ex.Message);
            return null;
        }
    }

    [WebMethod]
    public static string manage_durationdays(string startdate, string enddate)
    {
        try
        {
            DateTime startDateValue = ParseAssignDateOnly(startdate).Date;
            DateTime endDateValue = ParseAssignDateOnly(enddate).Date;

            if (endDateValue < startDateValue)
                return "0";

            int totalWorkingDays = 0;
            string empid = "0";

            for (DateTime checkDate = startDateValue;
                 checkDate <= endDateValue;
                 checkDate = checkDate.AddDays(1))
            {
                if (!getweeklyoff(checkDate, empid) &&
                    !getholiday(checkDate, empid))
                {
                    totalWorkingDays++;
                }
            }

            return totalWorkingDays.ToString(CultureInfo.InvariantCulture);
        }
        catch (Exception ex)
        {
            SaveLogText("manage_durationdaysEX-" + ex.Message);
            return "0";
        }
    }

    [WebMethod]
    public static string UpdateMilestone_Owner(string id = "", string empids = "")
    {
        string retval = "0";
        try
        {
            DataAccess da1 = new DataAccess();
            string finsql4 = "update Pms_tbl_AddMilestone set Owner='" + empids + "' where id='" + id + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da1.GetDataSetInline(finsql4);
            retval = "1";
        }
        catch { }
        return retval;
    }
    [WebMethod]
    public static string UpdateTask_Owner(string id = "", string empids = "")
    {
        string retval = "0";
        try
        {
            DataAccess da1 = new DataAccess();
            string finsql4 = "update Pms_tbl_Addtask set Owner='" + empids + "' where id='" + id + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da1.GetDataSetInline(finsql4);
            retval = "1";
        }
        catch { }
        return retval;
    }
    [WebMethod]
    public static string UpdateSubtask_Owner(string id = "", string empids = "")
    {
        string retval = "0";
        try
        {
            DataAccess da1 = new DataAccess();
            string finsql4 = "update Pms_tbl_Addsubtask set Owner='" + empids + "' where id='" +id + "'  and status=1 and archive=0";
            DataSet dsfindid4 = da1.GetDataSetInline(finsql4);
            retval = "1";
        }
        catch { }
        return retval;
    }



    [WebMethod]
    public static string removeownerbyid(projectowneres projectowneres)
    {
        string retval = "0";
        DataAccess da = new DataAccess();
        if (projectowneres.id != null)
        {

            try
            {
                string strsqlown = "select id,Owner from Pms_tbl_AddMilestone where status=1 and archive=0 and regid='"
                    + HttpContext.Current.Request.Cookies["cadmin"]["id"] +
                    "' and id='" + projectowneres.id + "' ";
                if (projectowneres.Ownertype == "task")
                {
                    strsqlown = "select id,Owner from Pms_tbl_AddTask where status=1 and archive=0 and regid='"
                                    + HttpContext.Current.Request.Cookies["cadmin"]["id"] +
                                    "' and id='" + projectowneres.id + "' ";
                }
                else if (projectowneres.Ownertype == "subtask")
                {
                    strsqlown = "select id,Owner from Pms_tbl_AddsubTask where status=1 and archive=0 and regid='"
                                    + HttpContext.Current.Request.Cookies["cadmin"]["id"] +
                                    "' and id='" + projectowneres.id + "' ";
                }
                DataSet dsown = da.GetDataSetInline(strsqlown);

                if (dsown.Tables[0].Rows.Count > 0)
                {
                    string ownerStr = dsown.Tables[0].Rows[0]["Owner"].ToString();
                    SaveLogText("updateQuery1-" + ownerStr);
                    if (!string.IsNullOrEmpty(ownerStr))
                    {
                        // Split
                        List<string> owners = ownerStr.Split(',').ToList();

                        // Remove matching ID
                        owners.Remove(projectowneres.Owner.ToString());

                        // Join back
                        string updatedOwners = string.Join(",", owners);

                        // Update DB
                        string updateQuery = "update Pms_tbl_AddMilestone set Owner='" + updatedOwners +
                                             "' where id='" + projectowneres.id + "'";
                        if (projectowneres.Ownertype == "task")
                        {
                            updateQuery = "update Pms_tbl_AddTask set Owner='" + updatedOwners +
                                             "' where id='" + projectowneres.id + "'";
                        }
                        else if (projectowneres.Ownertype == "subtask")
                        {
                            updateQuery = "update Pms_tbl_Addsubtask set Owner='" + updatedOwners +
                                              "' where id='" + projectowneres.id + "'";
                        }
                        SaveLogText("updateQuery2-" + updateQuery);
                        da.executeDMLQueryinline(updateQuery);
                        retval = "1";

                    }
                }
            }
            catch { }

        }

        
        return retval;
    }


    #endregion
    #region update team details

    // Team is intentionally kept in the existing comma-separated Team columns.
    // No new team table is required.  On add, only the newly-added employee IDs
    // are cascaded to the related hierarchy. Removal cascades downward only,
    // while all parent levels remain unchanged.
    private static List<int> NormalizeTeamIds(string value)
    {
        List<int> result = new List<int>();

        if (string.IsNullOrWhiteSpace(value))
            return result;

        foreach (string part in value.Split(','))
        {
            int employeeId;
            if (int.TryParse((part ?? "").Trim(), out employeeId)
                && employeeId > 0
                && !result.Contains(employeeId))
            {
                result.Add(employeeId);
            }
        }

        return result;
    }

    private static string TeamIdsToCsv(IEnumerable<int> employeeIds)
    {
        if (employeeIds == null)
            return "";

        return string.Join(",", employeeIds.Distinct());
    }

    private static string GetTeamValue(
        DataAccess da,
        string tableName,
        int recordId,
        int regId)
    {
        string sql = "select isnull(Team,'') Team from " + tableName
                   + " where id=" + recordId
                   + " and regid=" + regId
                   + " and status=1 and archive=0";

        DataSet ds = da.GetDataSetInline(sql);
        if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            return null;

        return ds.Tables[0].Rows[0]["Team"].ToString();
    }

    private static void SetExactTeamValue(
        DataAccess da,
        string tableName,
        int recordId,
        int regId,
        IEnumerable<int> employeeIds)
    {
        string csv = TeamIdsToCsv(employeeIds);
        string sql = "update " + tableName
                   + " set Team='" + csv + "'"
                   + " where id=" + recordId
                   + " and regid=" + regId
                   + " and status=1 and archive=0";

        da.executeDMLQueryinline(sql);
    }

    private static void MergeTeamIntoRows(
        DataAccess da,
        string tableName,
        string whereCondition,
        int regId,
        IEnumerable<int> employeeIds)
    {
        List<int> idsToAdd = employeeIds == null
            ? new List<int>()
            : employeeIds.Distinct().Where(x => x > 0).ToList();

        if (idsToAdd.Count == 0)
            return;

        string selectSql = "select id,isnull(Team,'') Team from " + tableName
                         + " where regid=" + regId
                         + " and status=1 and archive=0 and (" + whereCondition + ")";

        DataSet ds = da.GetDataSetInline(selectSql);
        if (ds.Tables.Count == 0)
            return;

        foreach (DataRow row in ds.Tables[0].Rows)
        {
            int rowId;
            if (!int.TryParse(row["id"].ToString(), out rowId))
                continue;

            List<int> merged = NormalizeTeamIds(row["Team"].ToString());
            foreach (int employeeId in idsToAdd)
            {
                if (!merged.Contains(employeeId))
                    merged.Add(employeeId);
            }

            SetExactTeamValue(da, tableName, rowId, regId, merged);
        }
    }

    private static void RemoveTeamFromRows(
        DataAccess da,
        string tableName,
        string whereCondition,
        int regId,
        IEnumerable<int> employeeIds)
    {
        List<int> idsToRemove = employeeIds == null
            ? new List<int>()
            : employeeIds.Distinct().Where(x => x > 0).ToList();

        if (idsToRemove.Count == 0)
            return;

        string selectSql = "select id,isnull(Team,'') Team from " + tableName
                         + " where regid=" + regId
                         + " and status=1 and archive=0 and (" + whereCondition + ")";

        DataSet ds = da.GetDataSetInline(selectSql);
        if (ds.Tables.Count == 0)
            return;

        foreach (DataRow row in ds.Tables[0].Rows)
        {
            int rowId;
            if (!int.TryParse(row["id"].ToString(), out rowId))
                continue;

            List<int> remaining = NormalizeTeamIds(row["Team"].ToString())
                .Where(x => !idsToRemove.Contains(x))
                .ToList();

            SetExactTeamValue(da, tableName, rowId, regId, remaining);
        }
    }

    private static bool TryGetTeamRequest(
        string id,
        string empids,
        string tableName,
        out DataAccess da,
        out int recordId,
        out int regId,
        out List<int> incomingIds,
        out List<int> addedIds,
        out List<int> removedIds)
    {
        da = new DataAccess();
        recordId = 0;
        regId = 0;
        incomingIds = NormalizeTeamIds(empids);
        addedIds = new List<int>();
        removedIds = new List<int>();

        HttpCookie regCookie = HttpContext.Current.Request.Cookies["cadmin"];
        if (regCookie == null
            || !int.TryParse(regCookie["id"], out regId)
            || !int.TryParse(id, out recordId)
            || recordId <= 0
            || regId <= 0)
        {
            return false;
        }

        string existingValue = GetTeamValue(da, tableName, recordId, regId);
        if (existingValue == null)
            return false;

        List<int> existingIds = NormalizeTeamIds(existingValue);
        // C# 5 does not allow an out parameter to be captured inside a lambda.
        // Use a normal local variable for the LINQ comparisons.
        List<int> requestedIds = incomingIds;
        addedIds = requestedIds.Where(x => !existingIds.Contains(x)).ToList();
        removedIds = existingIds.Where(x => !requestedIds.Contains(x)).ToList();
        return true;
    }

    // Check the scheduler before changing any team membership in this hierarchy.
    private static string CheckTeamRemovalAssignments(DataAccess da, string teamType,
        int recordId, int regId, IEnumerable<int> employeeIds)
    {
        List<int> ids = employeeIds.Distinct().Where(x => x > 0).ToList();
        if (ids.Count == 0) return null;
        string scope;
        switch (teamType)
        {
            case "project": scope = "projectid=" + recordId; break;
            case "milestone": scope = "milestoneid=" + recordId; break;
            case "task": scope = "taskid=" + recordId; break;
            case "subtask": scope = "tasktype='subtask' and subtaskid=" + recordId; break;
            default: return "0";
        }
        string sql = "select top(1) id from Pms_tbl_assigntasks where regid=" + regId
            + " and status=1 and archive=0 and ownerid in ("
            + string.Join(",", ids.Select(x => x.ToString()).ToArray()) + ") and (" + scope + ")";
        DataSet ds = da.GetDataSetInline(sql);
        if (ds.Tables.Count == 0) throw new InvalidOperationException("Assignment check failed.");
        return ds.Tables[0].Rows.Count > 0
            ? "TEAM_ASSIGNMENT_BLOCKED:This employee has assigned milestones/tasks/subtasks. Please remove those assignments before removing the employee from the team."
            : null;
    }

    [WebMethod]
    public static string UpdateProject_Team(string id = "", string empids = "")
    {
        string retval = "0";
        try
        {
            DataAccess da1;
            int projectId;
            int regId;
            List<int> incomingIds;
            List<int> addedIds;
            List<int> removedIds;

            if (!TryGetTeamRequest(id, empids, "Pms_tbl_AddProject",
                out da1, out projectId, out regId, out incomingIds, out addedIds, out removedIds))
                return retval;

            string assignmentBlock = CheckTeamRemovalAssignments(da1, "project", projectId, regId, removedIds);
            if (assignmentBlock != null) return assignmentBlock;

            // Preserve the exact current Project selection (add as well as remove).
            SetExactTeamValue(da1, "Pms_tbl_AddProject", projectId, regId, incomingIds);

            if (addedIds.Count > 0)
            {
                // Downward: all Milestones, Tasks and Subtasks of this Project.
                MergeTeamIntoRows(da1, "Pms_tbl_AddMilestone",
                    "ProjectId=" + projectId, regId, addedIds);
                MergeTeamIntoRows(da1, "Pms_tbl_AddTask",
                    "ProjectId=" + projectId, regId, addedIds);
                MergeTeamIntoRows(da1, "Pms_tbl_AddSubtask",
                    "ProjectId=" + projectId, regId, addedIds);
            }

            if (removedIds.Count > 0)
            {
                RemoveTeamFromRows(da1, "Pms_tbl_AddMilestone",
                    "ProjectId=" + projectId, regId, removedIds);
                RemoveTeamFromRows(da1, "Pms_tbl_AddTask",
                    "ProjectId=" + projectId, regId, removedIds);
                RemoveTeamFromRows(da1, "Pms_tbl_AddSubtask",
                    "ProjectId=" + projectId, regId, removedIds);
            }

            retval = "1";
        }
        catch (Exception ex)
        {
            SaveLogText("UpdateProject_TeamEX-" + ex.Message);
        }
        return retval;
    }

    [WebMethod]
    public static string UpdateMilestone_Team(string id = "", string empids = "")
    {
        string retval = "0";
        try
        {
            DataAccess da1;
            int milestoneId;
            int regId;
            List<int> incomingIds;
            List<int> addedIds;
            List<int> removedIds;

            if (!TryGetTeamRequest(id, empids, "Pms_tbl_AddMilestone",
                out da1, out milestoneId, out regId, out incomingIds, out addedIds, out removedIds))
                return retval;

            string assignmentBlock = CheckTeamRemovalAssignments(da1, "milestone", milestoneId, regId, removedIds);
            if (assignmentBlock != null) return assignmentBlock;

            // Preserve the exact current Milestone selection (add as well as remove).
            SetExactTeamValue(da1, "Pms_tbl_AddMilestone", milestoneId, regId, incomingIds);

            if (addedIds.Count > 0)
            {
                string hierarchySql = "select ProjectId from Pms_tbl_AddMilestone"
                                    + " where id=" + milestoneId
                                    + " and regid=" + regId
                                    + " and status=1 and archive=0";
                DataSet hierarchy = da1.GetDataSetInline(hierarchySql);

                if (hierarchy.Tables.Count > 0 && hierarchy.Tables[0].Rows.Count > 0)
                {
                    int projectId = Convert.ToInt32(hierarchy.Tables[0].Rows[0]["ProjectId"]);

                    // Upward: Project
                    MergeTeamIntoRows(da1, "Pms_tbl_AddProject",
                        "id=" + projectId, regId, addedIds);

                    // Downward: every Task and every Subtask of this Milestone
                    MergeTeamIntoRows(da1, "Pms_tbl_AddTask",
                        "Milestoneid=" + milestoneId, regId, addedIds);
                    MergeTeamIntoRows(da1, "Pms_tbl_AddSubtask",
                        "Milestoneid=" + milestoneId, regId, addedIds);
                }
            }

            if (removedIds.Count > 0)
            {
                // Removal cascades downward only; Project is intentionally unchanged.
                RemoveTeamFromRows(da1, "Pms_tbl_AddTask",
                    "Milestoneid=" + milestoneId, regId, removedIds);
                RemoveTeamFromRows(da1, "Pms_tbl_AddSubtask",
                    "Milestoneid=" + milestoneId, regId, removedIds);
            }

            retval = "1";
        }
        catch (Exception ex)
        {
            SaveLogText("UpdateMilestone_TeamEX-" + ex.Message);
        }
        return retval;
    }

    [WebMethod]
    public static string UpdateTask_Team(string id = "", string empids = "")
    {
        string retval = "0";
        try
        {
            DataAccess da1;
            int taskId;
            int regId;
            List<int> incomingIds;
            List<int> addedIds;
            List<int> removedIds;

            if (!TryGetTeamRequest(id, empids, "Pms_tbl_AddTask",
                out da1, out taskId, out regId, out incomingIds, out addedIds, out removedIds))
                return retval;

            string assignmentBlock = CheckTeamRemovalAssignments(da1, "task", taskId, regId, removedIds);
            if (assignmentBlock != null) return assignmentBlock;

            // Preserve the exact current Task selection (add as well as remove).
            SetExactTeamValue(da1, "Pms_tbl_AddTask", taskId, regId, incomingIds);

            if (addedIds.Count > 0)
            {
                string hierarchySql = "select ProjectId,Milestoneid from Pms_tbl_AddTask"
                                    + " where id=" + taskId
                                    + " and regid=" + regId
                                    + " and status=1 and archive=0";
                DataSet hierarchy = da1.GetDataSetInline(hierarchySql);

                if (hierarchy.Tables.Count > 0 && hierarchy.Tables[0].Rows.Count > 0)
                {
                    int projectId = Convert.ToInt32(hierarchy.Tables[0].Rows[0]["ProjectId"]);
                    int milestoneId = Convert.ToInt32(hierarchy.Tables[0].Rows[0]["Milestoneid"]);

                    // Upward: Milestone and Project
                    MergeTeamIntoRows(da1, "Pms_tbl_AddMilestone",
                        "id=" + milestoneId, regId, addedIds);
                    MergeTeamIntoRows(da1, "Pms_tbl_AddProject",
                        "id=" + projectId, regId, addedIds);

                    // Downward: every Subtask of this Task
                    MergeTeamIntoRows(da1, "Pms_tbl_AddSubtask",
                        "task=" + taskId, regId, addedIds);
                }
            }

            if (removedIds.Count > 0)
            {
                // Removal cascades to this Task's Subtasks only.
                RemoveTeamFromRows(da1, "Pms_tbl_AddSubtask",
                    "task=" + taskId, regId, removedIds);
            }

            retval = "1";
        }
        catch (Exception ex)
        {
            SaveLogText("UpdateTask_TeamEX-" + ex.Message);
        }
        return retval;
    }

    [WebMethod]
    public static string UpdateSubtask_Team(string id = "", string empids = "")
    {
        string retval = "0";
        try
        {
            DataAccess da1;
            int subtaskId;
            int regId;
            List<int> incomingIds;
            List<int> addedIds;
            List<int> removedIds;

            if (!TryGetTeamRequest(id, empids, "Pms_tbl_AddSubtask",
                out da1, out subtaskId, out regId, out incomingIds, out addedIds, out removedIds))
                return retval;

            string assignmentBlock = CheckTeamRemovalAssignments(da1, "subtask", subtaskId, regId, removedIds);
            if (assignmentBlock != null) return assignmentBlock;

            // Preserve the exact current Subtask selection (add as well as remove).
            SetExactTeamValue(da1, "Pms_tbl_AddSubtask", subtaskId, regId, incomingIds);

            if (addedIds.Count > 0)
            {
                string hierarchySql = "select ProjectId,Milestoneid,task from Pms_tbl_AddSubtask"
                                    + " where id=" + subtaskId
                                    + " and regid=" + regId
                                    + " and status=1 and archive=0";
                DataSet hierarchy = da1.GetDataSetInline(hierarchySql);

                if (hierarchy.Tables.Count > 0 && hierarchy.Tables[0].Rows.Count > 0)
                {
                    int projectId = Convert.ToInt32(hierarchy.Tables[0].Rows[0]["ProjectId"]);
                    int milestoneId = Convert.ToInt32(hierarchy.Tables[0].Rows[0]["Milestoneid"]);
                    int taskId = Convert.ToInt32(hierarchy.Tables[0].Rows[0]["task"]);

                    // Upward only: Task, Milestone and Project.
                    // Sibling Subtasks are intentionally not changed.
                    MergeTeamIntoRows(da1, "Pms_tbl_AddTask",
                        "id=" + taskId, regId, addedIds);
                    MergeTeamIntoRows(da1, "Pms_tbl_AddMilestone",
                        "id=" + milestoneId, regId, addedIds);
                    MergeTeamIntoRows(da1, "Pms_tbl_AddProject",
                        "id=" + projectId, regId, addedIds);
                }
            }

            retval = "1";
        }
        catch (Exception ex)
        {
            SaveLogText("UpdateSubtask_TeamEX-" + ex.Message);
        }
        return retval;
    }

    [WebMethod]
    public static string removeteambyid(projectteams projectteams)
    {
        string retval = "0";

        try
        {
            int recordId;
            int employeeId;
            int regId;
            HttpCookie regCookie = HttpContext.Current.Request.Cookies["cadmin"];

            if (projectteams == null
                || regCookie == null
                || !int.TryParse(regCookie["id"], out regId)
                || !int.TryParse(Convert.ToString(projectteams.id), out recordId)
                || !int.TryParse(Convert.ToString(projectteams.Team), out employeeId)
                || recordId <= 0
                || employeeId <= 0
                || regId <= 0)
            {
                return retval;
            }

            string teamType = (projectteams.Teamtype ?? "").Trim().ToLowerInvariant();
            DataAccess da = new DataAccess();
            List<int> employeeIds = new List<int> { employeeId };
            string assignmentBlock = CheckTeamRemovalAssignments(da, teamType, recordId, regId, employeeIds);
            if (assignmentBlock != null) return assignmentBlock;

            if (teamType == "project")
            {
                RemoveTeamFromRows(da, "Pms_tbl_AddProject",
                    "id=" + recordId, regId, employeeIds);
                RemoveTeamFromRows(da, "Pms_tbl_AddMilestone",
                    "ProjectId=" + recordId, regId, employeeIds);
                RemoveTeamFromRows(da, "Pms_tbl_AddTask",
                    "ProjectId=" + recordId, regId, employeeIds);
                RemoveTeamFromRows(da, "Pms_tbl_AddSubtask",
                    "ProjectId=" + recordId, regId, employeeIds);
            }
            else if (teamType == "milestone")
            {
                RemoveTeamFromRows(da, "Pms_tbl_AddMilestone",
                    "id=" + recordId, regId, employeeIds);
                RemoveTeamFromRows(da, "Pms_tbl_AddTask",
                    "Milestoneid=" + recordId, regId, employeeIds);
                RemoveTeamFromRows(da, "Pms_tbl_AddSubtask",
                    "Milestoneid=" + recordId, regId, employeeIds);
            }
            else if (teamType == "task")
            {
                RemoveTeamFromRows(da, "Pms_tbl_AddTask",
                    "id=" + recordId, regId, employeeIds);
                RemoveTeamFromRows(da, "Pms_tbl_AddSubtask",
                    "task=" + recordId, regId, employeeIds);
            }
            else if (teamType == "subtask")
            {
                RemoveTeamFromRows(da, "Pms_tbl_AddSubtask",
                    "id=" + recordId, regId, employeeIds);
            }
            else
            {
                return retval;
            }

            retval = "1";
        }
        catch (Exception ex)
        {
            SaveLogText("removeteambyidEX-" + ex.Message);
        }

        return retval;
    }
    #endregion
    [WebMethod]
    public static newmilestone binditemdetails_mile(string id, string type)
    {
        DataAccess da = new DataAccess();
        newmilestone obj = new newmilestone();
        if (string.IsNullOrEmpty(id))
            return obj;

        var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
        if (regCookie == null)
            return obj;
        string str2 = "select * from Pms_tbl_AddMilestone where id='" + id + "' and status=1 and archive=0";
        DataAccess danew = new DataAccess();
        DataSet drmile = danew.GetDataSetInline(str2);

        foreach (DataRow row in drmile.Tables[0].Rows)
        {


            int totaltasks = 0;
            List<ownerlist> newownerlist = getemployeelist<ownerlist>(row["Owner"].ToString());
            List<teamlist> newteamlist = getemployeelist<teamlist>(row["team"].ToString());
            List<projectfiles> newfileslist = bindfiledata(row["projectid"].ToString(), row["id"].ToString(), "milestone");
            List<newtask> newtasklist = bindtasklist_bymileid(row["id"].ToString(), row["projectid"].ToString());
            List<issuestdata> newissueslist = bindissueslist(row["id"].ToString(), "milestone");
            totaltasks = checktaskby_milestoneid(row["id"].ToString());
            int totaldependencys = 0;
            totaldependencys = Binddependencydata(row["id"].ToString(), "milestone");
            double mileTaskTotal = 0;
            mileTaskTotal = get_completionstatus(row["id"].ToString(), "milestone");
            //SaveLogText("chkadmintodayEX-" + mileTaskTotal);
            double mileWattagecomplite = mileTaskTotal;
            double mileWattagepending = 100;
            if (mileTaskTotal > 0)
            {
                mileWattagepending = mileWattagepending - mileTaskTotal;
            }
            string tleftdays = getleftdays_load(row["id"].ToString(), "milestone");
            #region find issues
            int totalissues = 0;
            int completeissues = 0;
            int pendingissues = 0;
            double completeissue_rates = 0;
            double pendingissue_rates = 0;
            try
            {

                totalissues = get_totalissue(row["id"].ToString(), "milestone");
                completeissues = get_completeissue(row["id"].ToString(), "milestone");
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


            obj.id = row["id"] as int?;
            obj.ProjectId = row["ProjectId"] as int?;
            obj.Milestone = row["Milestone"].ToString();
            obj.Weightage = row["Weightage"] as double?;
            //StartDate =Convert.ToDateTime(row["StartDate"] as DateTime?),
            obj.StartDate = GetDateOnlyString(row["StartDate"]);
            obj.EndDate = GetDateOnlyString(row["EndDate"]);
            obj.Owner = row["Owner"].ToString();
            obj.assignto = getassignid(row["id"].ToString(), "milestone");
            obj.Access = row["Access"] as int?;
            obj.team = row["team"].ToString();
            obj.P_Status = row["P_Status"] as int?;
            obj.status_name = get_Projectstatus(row["P_Status"].ToString());
            obj.status_color = get_Status_Color(row["P_Status"].ToString());
            obj.Priority = row["Priority"] as int?;
            obj.priority_name = get_PriorityName(row["Priority"].ToString());
            obj.priority_color = get_PriorityColor(row["Priority"].ToString());
            obj.Budget = row["Budget"] as double?;
            obj.Actual = row["Actual"] as double?;
            obj.uid = row["uid"] as int?;
            obj.cdate = row["cdate"] as DateTime?;
            obj.istask = row["istask"] as int?;
            obj.checkmileWeightage = row["checkmileWeightage"] as int?;
            obj.iscomplete = row["iscomplete"] as int?;
            obj.iscompletedate = GetDateOnlyString(row["iscompletedate"]);
            obj.iscompletepercentage = row["iscompletepercentage"] as int?;
            obj.Durationtype = row["Durationtype"] as int?;
            obj.TaskDuration = row["TaskDuration"].ToString();
            obj.Durationhourcal = row["Durationhourcal"] as double?;
            obj.orderno = row["orderno"] as int?;
            obj.miledescription = row["miledescription"].ToString();
            obj.Uploaddrawing = row["Uploaddrawing"].ToString();
            obj.txtdays = row["txtdays"].ToString();
            obj.complete_value = Math.Round(mileTaskTotal, 2).ToString();
            obj.pending_value = Math.Round(mileWattagepending, 2).ToString();
            obj.taskcount = totaltasks;
            obj.isExpanded = false;
            obj.leftdays = tleftdays;
            obj.totalissue = totalissues;
            obj.completeissue = completeissues;
            obj.pendingissue = pendingissues;
            obj.completeissue_rate = completeissue_rates;
            obj.pendingissue_rate = pendingissue_rates;
            obj.totaldependency = totaldependencys;
            obj.owners = newownerlist;
            obj.teams = newteamlist;
            obj.viewfiles = newfileslist;
            obj.tasks = newtasklist;

        }


        return obj;
    }
    [WebMethod]
    public static newtask binditemdetails_task(string id, string type)
    {
        DataAccess da = new DataAccess();
        newtask obj = new newtask();
        try
        {
            if (string.IsNullOrEmpty(id))
                return obj;

            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null)
                return obj;

            string regid = regCookie["id"];


            try
            {
                string str2 = "select * from Pms_tbl_Addtask where regid='" + regid + "' and id='" + id + "' and status=1 and archive=0 order by isnull(orderno,0)";

                SaveLogText("gettasklist-" + str2);
                DataAccess danew = new DataAccess();
                DataSet drtask = danew.GetDataSetInline(str2);

                foreach (DataRow row in drtask.Tables[0].Rows)
                {
                    List<ownerlist> newownerlist = getemployeelist<ownerlist>(row["Owner"].ToString());
                    List<teamlist> newteamlist = getemployeelist<teamlist>(row["team"].ToString());
                    List<projectfiles> newfileslist = bindfiledata(row["ProjectId"].ToString(), row["id"].ToString(), "task");
                    List<newsubtask> newsubtasklist = bindsubtasklist_bytaskid(row["id"].ToString(), row["ProjectId"].ToString(), row["Milestoneid"].ToString());
                    SaveLogText("getsubtasklistcount-" + newsubtasklist.Count);
                    double mileTaskTotal = 0;
                    mileTaskTotal = get_completionstatus(row["id"].ToString(), "task");
                    int totaldependencys = 0;
                    totaldependencys = Binddependencydata(row["id"].ToString(), "task");
                    double mileWattagecomplite = mileTaskTotal;
                    double mileWattagepending = 100;
                    if (mileTaskTotal > 0)
                    {
                        mileWattagepending = mileWattagepending - mileTaskTotal;
                    }
                    string tleftdays = getleftdays_load(row["id"].ToString(), "task");
                    #region find issues
                    int totalissues = 0;
                    int completeissues = 0;
                    int pendingissues = 0;
                    double completeissue_rates = 0;
                    double pendingissue_rates = 0;
                    try
                    {

                        totalissues = get_totalissue(row["id"].ToString(), "task");
                        completeissues = get_completeissue(row["id"].ToString(), "task");
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


                    obj.id = row["id"] as int?;
                    obj.ProjectId = row["ProjectId"] as int?;
                    obj.Milestoneid = row["Milestoneid"] as int?;
                    obj.taskname = row["taskname"].ToString();
                    obj.Weightage = row["Weightage"] as double?;
                    obj.Startdate = GetDateOnlyString(row["StartDate"]);
                    obj.Targetdate = GetDateOnlyString(row["Targetdate"]);
                    obj.owner = row["Owner"].ToString();
                    obj.assignto = getassignid(row["id"].ToString(), "task");
                    obj.team = row["team"].ToString();
                    obj.P_Status = row["P_Status"] as int?;
                    obj.status_name = get_Projectstatus(row["P_Status"].ToString());
                    obj.status_color = get_Status_Color(row["P_Status"].ToString());
                    obj.Priority = row["Priority"] as int?;
                    obj.priority_name = get_PriorityName(row["Priority"].ToString());
                    obj.priority_color = get_PriorityColor(row["Priority"].ToString());
                    obj.Budget = row["Budget"] as double?;
                    obj.Actual = row["Actual"] as double?;
                    obj.uid = row["uid"] as int?;
                    obj.cdate = row["cdate"] as DateTime?;
                    obj.issubtask = row["issubtask"] as int?;
                    obj.iscompletepercentage = row["iscompletepercentage"] as double?;
                    obj.TaskDuration = row["TaskDuration"].ToString();
                    obj.orderno = row["orderno"] as int?;
                    obj.taskdescripton = row["taskdescripton"].ToString();
                    obj.txtdays = row["txtdays"].ToString();
                    obj.complete_value = Math.Round(mileTaskTotal, 2).ToString();
                    obj.pending_value = Math.Round(mileWattagepending, 2).ToString();
                    obj.isExpanded = false;
                    obj.leftdays = tleftdays;
                    obj.totalissue = totalissues;
                    obj.completeissue = completeissues;
                    obj.pendingissue = pendingissues;
                    obj.completeissue_rate = completeissue_rates;
                    obj.pendingissue_rate = pendingissue_rates;
                    obj.totaldependency = totaldependencys;
                    obj.owners = newownerlist;
                    obj.teams = newteamlist;
                    obj.viewfiles = newfileslist;
                    obj.subtasks = newsubtasklist;

                }


                // projectlist.Add(obj);
            }
            catch { }




        }
        catch (Exception ex)
        {
            SaveLogText("chkadmintaskEX-" + ex.Message);
            return new newtask();
        }


        return obj;
    }
    [WebMethod]
    public static newsubtask binditemdetails_subtask(string id, string type)
    {
        DataAccess da = new DataAccess();
        newsubtask obj = new newsubtask();
        try
        {
            if (string.IsNullOrEmpty(id))
                return obj;

            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null)
                return obj;
            string regid = regCookie["id"];
            string str2 = "select * from Pms_tbl_Addsubtask where regid='" + regid + "' and id='" + id + "' and status=1 and archive=0 order by isnull(orderno,0)";


            DataAccess danew = new DataAccess();
            DataSet drsubtask = danew.GetDataSetInline(str2);
            foreach (DataRow row in drsubtask.Tables[0].Rows)
            {
                SaveLogText("getsubtaskobjlist-" + row["id"]);
                List<ownerlist> newownerlist = getemployeelist<ownerlist>(row["Owner"].ToString());
                List<teamlist> newteamlist = getemployeelist<teamlist>(row["team"].ToString());
                List<projectfiles> newfileslist = bindfiledata(row["ProjectId"].ToString(), row["id"].ToString(), "subtask");
                double mileTaskTotal = 0;
                mileTaskTotal = get_completionstatus(row["id"].ToString(), "subtask");
                int totaldependencys = 0;
                totaldependencys = Binddependencydata(row["id"].ToString(), "subtask");
                //SaveLogText("chkadmintodayEX-" + mileTaskTotal);
                double mileWattagecomplite = mileTaskTotal;
                double mileWattagepending = 100;
                if (mileTaskTotal > 0)
                {
                    mileWattagepending = mileWattagepending - mileTaskTotal;
                }
                string tleftdays = getleftdays_load(row["id"].ToString(), "subtask");
                #region find issues
                int totalissues = 0;
                int completeissues = 0;
                int pendingissues = 0;
                double completeissue_rates = 0;
                double pendingissue_rates = 0;
                try
                {

                    totalissues = get_totalissue(row["id"].ToString(), "subtask");
                    completeissues = get_completeissue(row["id"].ToString(), "subtask");
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


                obj.id = row["id"] as int?;
                obj.ProjectId = row["ProjectId"] as int?;
                obj.Milestoneid = row["Milestoneid"] as int?;
                obj.Task = row["task"] as int?;
                obj.Subtaskname = row["Subtaskname"].ToString();
                obj.Weightage = row["Weightage"] as double?;
                obj.Startdate = GetDateOnlyString(row["StartDate"]);
                obj.Targetdate = GetDateOnlyString(row["Targetdate"]);
                obj.owner = row["Owner"].ToString();
                obj.assignto = getassignid(row["id"].ToString(), "subtask");
                obj.team = row["team"].ToString();
                obj.P_Status = row["P_Status"] as int?;
                obj.status_name = get_Projectstatus(row["P_Status"].ToString());
                obj.status_color = get_Status_Color(row["P_Status"].ToString());
                obj.Priority = row["Priority"] as int?;
                obj.priority_name = get_PriorityName(row["Priority"].ToString());
                obj.priority_color = get_PriorityColor(row["Priority"].ToString());
                obj.Budget = row["Budget"] as double?;
                obj.Actual = row["Actual"] as double?;
                obj.uid = row["uid"] as int?;
                obj.cdate = row["cdate"] as DateTime?;
                obj.iscompletepercentage = row["iscompletepercentage"] as double?;
                obj.TaskDuration = row["TaskDuration"].ToString();
                obj.orderno = row["orderno"] as int?;
                obj.SubTaskDescription = row["SubTaskDescription"].ToString();
                obj.txtdays = row["txtdays"].ToString();
                obj.complete_value = Math.Round(mileTaskTotal, 2).ToString();
                obj.pending_value = Math.Round(mileWattagepending, 2).ToString();
                obj.leftdays = tleftdays;
                obj.totalissue = totalissues;
                obj.completeissue = completeissues;
                obj.pendingissue = pendingissues;
                obj.completeissue_rate = completeissue_rates;
                obj.pendingissue_rate = pendingissue_rates;
                obj.totaldependency = totaldependencys;
                obj.owners = newownerlist;
                obj.teams = newteamlist;
                obj.viewfiles = newfileslist;




            }

        }
        catch (Exception ex)
        {
            return new newsubtask();
        }
        return obj;
    }

    #endregion
    // Issue status options use the dedicated issue master only.
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<Dictionary<string, string>> bindIssueStatusOptions()
    {
        var result = new List<Dictionary<string, string>>();
        int regid;
        var cookie = HttpContext.Current.Request.Cookies["cadmin"];
        if (cookie == null || !Int32.TryParse(cookie["id"], out regid) || regid <= 0)
            throw new HttpException(401, "Invalid login session.");

        DataSet ds = new DataAccess().GetDataSet("Pms_SP_SelectIssuestatus_reg",
            new SqlParameter[] { new SqlParameter("@regid", regid) });
        foreach (DataRow row in ds.Tables[0].Rows)
        {
            result.Add(new Dictionary<string, string> {
                { "id", Convert.ToString(row["id"], CultureInfo.InvariantCulture) },
                { "name", Convert.ToString(row["issuename"], CultureInfo.InvariantCulture) }
            });
        }
        return result;
    }
    #region mange issues
    [WebMethod]
    public static string saveIssue(issuestdata issuesdetails)
    {
        string retval = "0";
        DataAccess da1 = new DataAccess();
        string editid = issuesdetails.id.ToString();
        string entrytype = "Insert";
        if (editid != "" && editid != "0")
        {
            entrytype = "Update";
        }

        if (issuesdetails.subtaskid > 0)
        {
            issuesdetails.Issueon = "subtask";
        }
        else if (issuesdetails.taskid > 0)
        {
            issuesdetails.Issueon = "task";
        }
        else if (issuesdetails.Milestoneid > 0)
        {
            issuesdetails.Issueon = "milstone";
        }
        else
        {
            issuesdetails.Issueon = "project";
        }
        SqlParameter[] prm = new SqlParameter[30];
        prm[0] = new SqlParameter("@id", editid);
        prm[1] = new SqlParameter("@ProjectId", issuesdetails.ProjectId);
        prm[2] = new SqlParameter("@Milestoneid", issuesdetails.Milestoneid);
        prm[3] = new SqlParameter("@tasklistid", issuesdetails.tasklistid);
        prm[4] = new SqlParameter("@taskid", issuesdetails.taskid);
        prm[5] = new SqlParameter("@subtaskid", issuesdetails.subtaskid);
        prm[6] = new SqlParameter("@Issues", issuesdetails.Issues);

        if (issuesdetails.StartDate != null)
        {
            prm[7] = new SqlParameter("@StartDate", DBNull.Value);
        }
        else
        {
            prm[7] = new SqlParameter("@StartDate", issuesdetails.StartDate);
        }
        if (issuesdetails.EndDate != null)
        {
            prm[8] = new SqlParameter("@EndDate", DBNull.Value);
        }
        else
        {
            prm[8] = new SqlParameter("@EndDate", issuesdetails.EndDate);
        }

        prm[9] = new SqlParameter("@Owner", issuesdetails.Owner);
        prm[10] = new SqlParameter("@team", issuesdetails.team);
        prm[11] = new SqlParameter("@Issueon", issuesdetails.Issueon);
        prm[12] = new SqlParameter("@Description", issuesdetails.Description);
        prm[13] = new SqlParameter("@Reminder", issuesdetails.Reminder);
        if (issuesdetails.ReminderType == string.Empty)
        {
            prm[14] = new SqlParameter("@ReminderType", DBNull.Value);
        }
        else
        {
            prm[14] = new SqlParameter("@ReminderType", issuesdetails.ReminderType);
        }
        if (issuesdetails.Days == string.Empty)
        {
            prm[15] = new SqlParameter("@Days", DBNull.Value);
        }
        else
        {
            prm[15] = new SqlParameter("@Days", issuesdetails.Days);
        }
        if (issuesdetails.Time == string.Empty)
        {
            prm[16] = new SqlParameter("@Time", DBNull.Value);

        }
        else
        {
            prm[16] = new SqlParameter("@Time", issuesdetails.Time);
        }

        if (issuesdetails.Notifyusers == string.Empty)
        {
            prm[17] = new SqlParameter("@Notifyusers", DBNull.Value);
        }
        else
        {
            prm[17] = new SqlParameter("@Notifyusers", issuesdetails.Notifyusers);
        }

        if (issuesdetails.ChooseDate == string.Empty)
        {
            prm[18] = new SqlParameter("@ChooseDate", DBNull.Value);
        }
        else
        {
            prm[18] = new SqlParameter("@ChooseDate", issuesdetails.ChooseDate);
        }
        if (issuesdetails.AssignTo == string.Empty)
        {
            prm[19] = new SqlParameter("@AssignTo", DBNull.Value);
        }
        else
        {
            prm[19] = new SqlParameter("@AssignTo", issuesdetails.AssignTo);

        }
        prm[20] = new SqlParameter("@IssueStatus", issuesdetails.IssueStatus);
        prm[21] = new SqlParameter("@Priority", issuesdetails.Priority);
        prm[22] = new SqlParameter("@IssueType", issuesdetails.IssueType);
        prm[23] = new SqlParameter("@Replicability", issuesdetails.Replicability);
        prm[24] = new SqlParameter("@UploadImage", issuesdetails.UploadImage);
        prm[25] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        prm[26] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[27] = new SqlParameter("@cdate", da1.getcdatetime());
        prm[28] = new SqlParameter("@type", entrytype);
        // da1.executeDMLQuery("pms_AddiSSUES_usp_bak", prm);
        prm[29] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[29].Direction = ParameterDirection.ReturnValue;
        int retval1 = da1.executeDMLQuery("pms_AddiSSUES_usp", prm);
        return retval = retval1.ToString();
    }
    public static List<issuestdata> bindissueslist(string id, string issuetype)
    {
        DataAccess da = new DataAccess();
        List<issuestdata> obj = new List<issuestdata>();

        try
        {
            if (string.IsNullOrEmpty(id))
                id = "0";

            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null)
                return obj;

            string regid = regCookie["id"];


            try
            {
                string milestr111 = "";

                if (issuetype == "milestone")
                {
                    milestr111 = "select COUNT(*) as countIssues from Pms_tbl_Addissues where  Milestoneid='" + id + "' and Issueon='milstone' and status=1 and archive=0 ";
                }
                else if (issuetype == "task")
                {
                    milestr111 = "select COUNT(*) as countIssues from Pms_tbl_Addissues where  taskid='" + id + "' and Issueon='task' and status=1 and archive=0 ";
                }
                else if (issuetype == "subtask")
                {
                    milestr111 = "select COUNT(*) as countIssues from Pms_tbl_Addissues where  subtaskid='" + id + "' and Issueon='subtask' and status=1 and archive=0 ";
                }
                if (id == "" || id == "0")
                {
                    return obj;
                }


                DataSet dsI = da.GetDataSetInline(milestr111);

                foreach (DataRow row in dsI.Tables[0].Rows)
                {

                    obj.Add(new issuestdata
                    {

                        id = row["id"] as int?,
                        ProjectId = row["ProjectId"] as int?,
                        Milestoneid = row["Milestoneid"] as int?,
                        tasklistid = row["tasklistid"].ToString(),
                        taskid = row["taskid"] as int?,
                        subtaskid = row["subtaskid"] as int?,
                        Issues = row["Issues"].ToString(),
                        StartDate = row["StartDate"] as DateTime?,
                        EndDate = row["EndDate"] as DateTime?,
                        Owner = row["Owner"].ToString(),
                        team = row["team"].ToString(),
                        Issueon = row["Issueon"].ToString(),
                        project_id = row["project_id"].ToString(),
                        Description = row["Description"].ToString(),
                        Reminder = row["Reminder"].ToString(),
                        ReminderType = row["ReminderType"].ToString(),
                        Days = row["Days"].ToString(),
                        Time = row["Time"].ToString(),
                        Notifyusers = row["Notifyusers"].ToString(),
                        ChooseDate = row["ChooseDate"].ToString(),
                        AssignTo = row["AssignTo"].ToString(),
                        IssueStatus = row["IssueStatus"] as int?,
                        Priority = row["Priority"] as int?,
                        IssueType = row["IssueType"].ToString(),
                        IssueTypeName = get_issuetype(row["IssueType"].ToString()),
                        Replicability = row["Replicability"].ToString(),

                    });
                }



                // projectlist.Add(obj);
            }
            catch { }




        }
        catch (Exception ex)
        {
            SaveLogText("chkadmintaskEX-" + ex.Message);
            return new List<issuestdata>();
        }


        return obj;
    }
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
                SaveLogText("Issuetxt=" + totalissues.ToString() + "," + completeissues.ToString());
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
        catch (Exception ex)
        {
            SaveLogText("chkadmintaskEX-" + ex.Message);
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
    protected static string get_issuetype(string id)
    {
        string strret = "";
        string milestr111 = " select id ,name  from Pms_tbl_IssueType where status=1 and archive=0 and id="+id+" ";
        if (id == "" || id == "0")
        {
            return "";
        }
        DataAccess da = new DataAccess();

        DataSet miledss111 = da.GetDataSetInline(milestr111);
        if (miledss111.Tables[0].Rows.Count > 0)
        {

            try
            {
                strret = miledss111.Tables[0].Rows[0]["name"].ToString();
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
            catch (Exception ex)
            {
                SaveLogText("Issuesql=" + milestr111+","+ ex.Message);
            }
        }

        return strret;

    }
    #endregion
    #region manage dependency
    // Gantt scheduling links are separate from tbl_dependencyST's completion rules.
    [WebMethod]
    public static Dictionary<string, object> GanttDependency(string action, int projectId,
        string itemType, int itemId, string otherType, int otherId,
        string relation, string linkType, int lagHours, int linkId)
    {
        var answer = new Dictionary<string, object>();
        int regid;
        HttpCookie cookie = HttpContext.Current.Request.Cookies["cadmin"];
        if (cookie == null || !Int32.TryParse(cookie["id"], out regid) || regid <= 0)
        {
            answer["error"] = "Please sign in again.";
            return answer;
        }
        try
        {
            SqlParameter[] parameters = {
                new SqlParameter("@Action", action ?? ""),
                new SqlParameter("@RegId", regid),
                new SqlParameter("@ProjectId", projectId),
                new SqlParameter("@ItemType", itemType ?? ""),
                new SqlParameter("@ItemId", itemId),
                new SqlParameter("@OtherType", otherType ?? ""),
                new SqlParameter("@OtherId", otherId),
                new SqlParameter("@Relation", relation ?? ""),
                new SqlParameter("@LinkType", linkType ?? "FS"),
                new SqlParameter("@LagHours", lagHours),
                new SqlParameter("@LinkId", linkId)
            };
            DataSet data = new DataAccess().GetDataSet("dbo.usp_Pms_GanttDependency", parameters);
            var rows = new List<Dictionary<string, object>>();
            if (data != null && data.Tables.Count > 0)
                foreach (DataRow row in data.Tables[0].Rows)
                {
                    var item = new Dictionary<string, object>();
                    foreach (DataColumn col in data.Tables[0].Columns)
                        item[col.ColumnName] = row.IsNull(col) ? null : row[col];
                    rows.Add(item);
                }
            answer["rows"] = rows;
        }
        catch (Exception ex)
        {
            answer["error"] = ex.Message;
        }
        return answer;
    }

    [WebMethod]
    public static Dictionary<string, object> UpdateGanttDependency(int projectId, int linkId,
        string linkType, int lagHours)
    {
        var answer = new Dictionary<string, object>();
        int regid;
        HttpCookie cookie = HttpContext.Current.Request.Cookies["cadmin"];
        if (cookie == null || !Int32.TryParse(cookie["id"], out regid) || regid <= 0)
        {
            answer["error"] = "Please sign in again.";
            return answer;
        }
        linkType = (linkType ?? "").Trim().ToUpperInvariant();
        if (projectId <= 0 || linkId <= 0 ||
            (linkType != "FS" && linkType != "SS" && linkType != "FF" && linkType != "SF") ||
            lagHours < -8760 || lagHours > 8760)
        {
            answer["error"] = "Invalid dependency details.";
            return answer;
        }
        try
        {
            // Values are constrained to integers or the four supported link types.
            string sql = "update dbo.Pms_tbl_GanttDependency set LinkType='" + linkType +
                "', LagHours=" + lagHours + " where Id=" + linkId + " and RegId=" + regid +
                " and ProjectId=" + projectId + "; select @@ROWCOUNT as Affected";
            DataSet data = new DataAccess().GetDataSetInline(sql);
            int affected = 0;
            if (data != null && data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
                Int32.TryParse(Convert.ToString(data.Tables[0].Rows[0]["Affected"]), out affected);
            if (affected == 0)
                answer["error"] = "Dependency was not found. Refresh the list and try again.";
            else
                answer["rows"] = new List<Dictionary<string, object>>();
        }
        catch (Exception ex)
        {
            answer["error"] = ex.Message;
        }
        return answer;
    }

    public static int Binddependencydata(string id,string utype)
    {
        int retval = 0;
        DataAccess da = new DataAccess();
        string strD = "";
        if(utype=="milestone")
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
                                if (dschkM.Tables[0].Rows[0]["statuscomplete"].ToString()!="1")
                                {
                                    chkallcomplete =1;
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
                                if (dschkM.Tables[0].Rows[0]["statuscomplete"].ToString()!= "1")
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
                        if(chkallcomplete==1)
                        {
                            retval++;
                        }
                    }
                }

            }
            if (ids == "7")
            {
                SaveLogText("ddetails:" + retval);

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
        catch (Exception ex)
        {
            SaveLogText("chkadmintaskEX-" + ex.Message);
            
        }


        return retval;
    }

    #endregion





    #region manage complete weightage
    public static string managecalculation(string id, string type)
    {
        function fns = new function();
        DataAccess da1 = new DataAccess();
        string retval = "1";
        string rid = id;
        string pid = "0";
        string mid = "0";
        string tid = "0";
        string sid = "0";

        string sqlP = "select id as mid,isnull(ProjectId,0) as pid,0 as tid,0 as sid,isnull(checkmileWeightage ,0) as checkmileWeightage from pms_tbl_addmilestone where status=1 and archive=0 and id=" + rid + "";
        if (type == "task")
        {
            sqlP = "select id as tid, isnull(Milestoneid,0) as mid,isnull(ProjectId,0) as pid,0 as sid from pms_tbl_addtask where status=1 and archive=0 and id=" + rid + "";
        }
        else if (type == "subtask")
        {
            sqlP = "select id as sid,isnull(task,0) as tid, isnull(Milestoneid,0) as mid,isnull(ProjectId,0) as pid from pms_tbl_addsubtask where status=1 and archive=0 and id=" + rid + "";
        }

        try
        {
            DataSet dsD = da1.GetDataSetInline(sqlP);
            if (dsD.Tables[0].Rows.Count > 0)
            {
                pid = dsD.Tables[0].Rows[0]["pid"].ToString();
                mid = dsD.Tables[0].Rows[0]["mid"].ToString();
                tid = dsD.Tables[0].Rows[0]["tid"].ToString();
                sid = dsD.Tables[0].Rows[0]["sid"].ToString();
            }
        }
        catch
        {
        }
        int checkweitage = 0;
        decimal wetagecount = 0;
        decimal totalwetage = 0;

        if (type == "milestone")
        {
            try
            {
                string strweightage = "select isnull(checkprojectWeightage ,0) as checkprojectWeightage  from Pms_tbl_AddProject where id='" + pid + "' and  status=1 and  isnull(checkprojectWeightage ,0)=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                DataSet dsweitagecheck = da1.GetDataSetInline(strweightage);
                if (dsweitagecheck.Tables[0].Rows.Count > 0)
                {

                    checkweitage = Convert.ToInt32(dsweitagecheck.Tables[0].Rows[0]["checkprojectWeightage"]);

                }
                //   iscomplete

                if (checkweitage == 1)
                {
                    string strweightagecount = "select count (*) as WeitageCount from Pms_tbl_AddMilestone where ProjectId='" + pid + "' and  status=1   and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                    DataSet dsweitagecount = da1.GetDataSetInline(strweightagecount);
                    if (dsweitagecount.Tables[0].Rows.Count > 0)
                    {


                        wetagecount = Convert.ToInt32(dsweitagecount.Tables[0].Rows[0]["WeitageCount"]);

                    }

                    totalwetage = 100 / wetagecount;
                    string strstatusupdate = "update Pms_tbl_AddMilestone set Weightage='" + fns.ToFormattedCurrencyString(totalwetage.ToString()) + "' where ProjectId='" + pid + "' and status=1  and archive=0";
                    DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                }


                int iscomplete = 0;
                string findsqlismile = "select isnull(iscomplete,0) as iscomplete, * from Pms_tbl_AddMilestone where id=" + mid + " and status=1 and archive=0";
                DataSet dsfindidismile = da1.GetDataSetInline(findsqlismile);
                if (dsfindidismile.Tables[0].Rows.Count > 0)
                {

                    iscomplete = Convert.ToInt32(dsfindidismile.Tables[0].Rows[0]["iscomplete"]);

                }



                if (iscomplete == 1)
                {
                    decimal milestonetotalpercent = 100;
                    decimal totlweitagemile = 0;
                    //string finsql1 = "select * from Pms_tbl_AddMilestone where id=" + mid + " and status=1  and archive=0";
                    //DataSet dsfindid1 = da1.GetDataSetInline(finsql1);

                    // Milstone percentage //
                    string strstatusupdate = "update Pms_tbl_AddMilestone set iscompletepercentage='" + milestonetotalpercent.ToString() + "' where id='" + mid + "' and isnull(iscomplete,0)=1 and status=1 and archive=0";
                    DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                    // project percentage //
                    string finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + pid + "'  and status=1  and archive=0";
                    DataSet dsfindid2 = da1.GetDataSetInline(finsql2);
                    for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
                    {
                        decimal sumweitage = 0;
                        decimal subpercentageemile = 0;
                        sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
                        subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                        totlweitagemile += (sumweitage * subpercentageemile) / 100;

                    }

                    string finsql4 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitagemile.ToString() + "' where id='" + pid + "'  and status=1 and archive=0";
                    DataSet dsfindid4 = da1.GetDataSetInline(finsql4);

                }
                else
                {
                    string finsql2 = "";
                    DataSet dsfindid2;
                    decimal milestonetotalpercent = 0;
                    decimal totlweitagemile = 0;
                    decimal totlweitageproject = 0;
                    string finsql1 = "select * from Pms_tbl_AddMilestone where id=" + mid + " and status=1 and archive=0";
                    DataSet dsfindid1 = da1.GetDataSetInline(finsql1);
                    if (dsfindid1.Tables[0].Rows.Count > 0)
                    {
                        if (dsfindid1.Tables[0].Rows[0]["istask"].ToString() == "1")
                        {
                            finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where  Milestoneid='" + mid + "'  and status=1  and archive=0";
                            dsfindid2 = da1.GetDataSetInline(finsql2);
                            for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
                            {
                                decimal sumweitage = 0;
                                decimal subpercentageemile = 0;
                                sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
                                subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                                totlweitagemile += (sumweitage * subpercentageemile) / 100;

                            }
                        }

                    }






                    // Milestone percentage //
                    string strstatusupdate = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile.ToString() + "' where id='" + mid + "' and status=1 and archive=0 and isnull(iscomplete,0)=0";
                    DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                    // project percentage //
                    finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + pid + "'  and status=1  and archive=0";
                    dsfindid2 = da1.GetDataSetInline(finsql2);
                    for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
                    {
                        decimal sumweitage = 0;
                        decimal subpercentageemile = 0;
                        sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
                        subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                        totlweitageproject += (sumweitage * subpercentageemile) / 100;

                    }
                    string finsql4 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + pid + "'  and status=1 and archive=0";
                    DataSet dsfindid4 = da1.GetDataSetInline(finsql4);



                }
            }
            catch { retval = "0"; }
        }
        else if (type == "task")
        {
            try
            {
                //string strweightage = "select isnull(checkprojectWeightage ,0) as checkprojectWeightage  from Pms_tbl_AddProject where id='" + pid + "' and  status=1 and  isnull(checkprojectWeightage ,0)=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                //DataSet dsweitagecheck = da1.GetDataSetInline(strweightage);
                //if (dsweitagecheck.Tables[0].Rows.Count > 0)
                //{

                //    checkweitage = Convert.ToInt32(dsweitagecheck.Tables[0].Rows[0]["checkprojectWeightage"]);

                //}
                string strweightage = "select isnull(checkmileWeightage ,0) as checkmileWeightage  from Pms_tbl_AddMilestone where id='" + mid + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                DataSet dsweitagecheck = da1.GetDataSetInline(strweightage);
                if (dsweitagecheck.Tables[0].Rows.Count > 0)
                {

                    checkweitage = Convert.ToInt32(dsweitagecheck.Tables[0].Rows[0]["checkmileWeightage"]);

                }
                if (checkweitage == 1)
                {
                    string strweightagecount = "select count (*) as WeitageCount from Pms_tbl_Addtask where Milestoneid='" + mid + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                    DataSet dsweitagecount = da1.GetDataSetInline(strweightagecount);
                    if (dsweitagecount.Tables[0].Rows.Count > 0)
                    {

                        wetagecount = Convert.ToInt32(dsweitagecount.Tables[0].Rows[0]["WeitageCount"]);

                    }

                    totalwetage = 100 / wetagecount;
                    string strstatusupdate = "update Pms_tbl_Addtask set Weightage='" + fns.ToFormattedCurrencyString(totalwetage.ToString()) + "' where Milestoneid='" + mid + "' and status=1 and archive=0";
                    DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                }

                int iscomplete = 0;
                string finsql1 = "select isnull(iscomplete,0) as iscomplete, * from Pms_tbl_Addtask where id=" + tid + " and status=1 and archive=0";
                DataSet dsfindid1 = da1.GetDataSetInline(finsql1);
                if (dsfindid1.Tables[0].Rows.Count > 0)
                {

                    iscomplete = Convert.ToInt32(dsfindid1.Tables[0].Rows[0]["iscomplete"]);

                }
                if (iscomplete == 1)
                {
                    decimal tsktotalpercent = 100;
                    decimal totlweitagemile = 0;
                    decimal totlweitageproject = 0;

                    string finsql01 = "select * from Pms_tbl_Addtask where id=" + tid + " and status=1  and archive=0";
                    DataSet dsfindid01 = da1.GetDataSetInline(finsql01);

                    // task percentage //
                    string strstatusupdate = "update Pms_tbl_Addtask set iscompletepercentage='" + tsktotalpercent.ToString() + "' where id='" + tid + "' and status=1 and archive=0 and isnull(iscomplete,0)=1";
                    DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                    // Milestone percentage //
                    string finsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where ProjectId='" + pid + "' and Milestoneid='" + mid + "'   and status=1  and archive=0";
                    DataSet dsfindmile = da1.GetDataSetInline(finsqlmile);
                    for (int j = 0; j < dsfindmile.Tables[0].Rows.Count; j++)
                    {
                        decimal sumweitage = 0;
                        decimal subpercentageemile = 0;
                        sumweitage = Convert.ToDecimal(dsfindmile.Tables[0].Rows[j]["Weightage"].ToString());
                        subpercentageemile = Convert.ToDecimal(dsfindmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                        totlweitagemile += (sumweitage * subpercentageemile) / 100;

                    }

                    string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile.ToString() + "' where ProjectId='" + pid + "' and id='" + mid + "'  and status=1 and archive=0";
                    DataSet dsfindid4 = da1.GetDataSetInline(finsql4);


                    // project percentage //
                    string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + pid + "'   and status=1  and archive=0";
                    DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
                    for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
                    {
                        decimal sumweitageprojt = 0;
                        decimal subpercentageeproject = 0;
                        sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                        subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                        totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

                    }

                    string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + pid + "'  and status=1 and archive=0";
                    DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);




                }
                else
                {
                    decimal tsktotalpercent = 0;
                    decimal totlweitagemile = 0;
                    decimal totlweitageproject = 0;

                    string finsql01 = "select * from Pms_tbl_Addtask where id=" + tid + " and status=1  and archive=0";
                    DataSet dsfindid01 = da1.GetDataSetInline(finsql01);


                    if (dsfindid01.Tables[0].Rows.Count > 0)
                    {
                        if (dsfindid01.Tables[0].Rows[0]["issubtask"].ToString() == "1")
                        {

                            string finsql211 = "select isnull(sum(weightage),0)Wattagetasksum from Pms_tbl_Addsubtask where   task='" + tid + "'  and status=1 and isnull(iscomplete,0)=1 and archive=0";
                            DataSet dsfindid211 = da1.GetDataSetInline(finsql211);
                            if (dsfindid211.Tables[0].Rows.Count > 0)
                            {
                                string strstatusupdate = "update Pms_tbl_Addtask set iscompletepercentage='" + dsfindid211.Tables[0].Rows[0]["Wattagetasksum"] + "' where id='" + tid + "' and status=1 and archive=0";
                                DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);
                            }
                        }
                        else
                        {
                            string strstatusupdate = "update Pms_tbl_Addtask set iscompletepercentage='0' where id='" + tid + "' and status=1 and archive=0";
                            DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                        }

                    }


                    // Milestone percentage //
                    string finsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where ProjectId='" + pid + "' and Milestoneid='" + mid + "'   and status=1  and archive=0";
                    DataSet dsfindmile = da1.GetDataSetInline(finsqlmile);
                    for (int j = 0; j < dsfindmile.Tables[0].Rows.Count; j++)
                    {
                        decimal sumweitage = 0;
                        decimal subpercentageemile = 0;
                        sumweitage = Convert.ToDecimal(dsfindmile.Tables[0].Rows[j]["Weightage"].ToString());
                        subpercentageemile = Convert.ToDecimal(dsfindmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                        totlweitagemile += (sumweitage * subpercentageemile) / 100;

                    }

                    string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile.ToString() + "' where ProjectId='" + pid + "' and id='" + mid + "'  and status=1 and archive=0";
                    DataSet dsfindid4 = da1.GetDataSetInline(finsql4);

                    // project percentage //
                    string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + pid + "'   and status=1  and archive=0";
                    DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
                    for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
                    {
                        decimal sumweitageprojt = 0;
                        decimal subpercentageeproject = 0;
                        sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                        subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                        totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

                    }

                    string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + pid + "'  and status=1 and archive=0";
                    DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);



                }
            }
            catch { retval = "0"; }
        }
        else if (type == "subtask")
        {
            try
            {
                string strweightage = "select isnull(checkTaskWeightage ,0) as checkTaskWeightage  from pms_tbl_addtask where id='" + tid + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                DataSet dsweitagecheck = da1.GetDataSetInline(strweightage);
                if (dsweitagecheck.Tables[0].Rows.Count > 0)
                {

                    checkweitage = Convert.ToInt32(dsweitagecheck.Tables[0].Rows[0]["checkTaskWeightage"]);

                }
                if (checkweitage == 1)
                {
                    string strweightagecount = "select count (*) as WeitageCount from Pms_tbl_Addsubtask where task='" + tid + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                    DataSet dsweitagecount = da1.GetDataSetInline(strweightagecount);
                    if (dsweitagecount.Tables[0].Rows.Count > 0)
                    {

                        wetagecount = Convert.ToInt32(dsweitagecount.Tables[0].Rows[0]["WeitageCount"]);

                    }

                    totalwetage = 100 / wetagecount;
                    string strstatusupdate = "update Pms_tbl_Addsubtask set Weightage='" + fns.ToFormattedCurrencyString(totalwetage.ToString()) + "' where task='" + tid + "' and status=1 and archive=0";
                    DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                }


                int iscomplete = 0;
                string finsql1 = "select  isnull(iscomplete,0) as iscomplete,* from Pms_tbl_Addsubtask where id=" + retval + " and status=1  and archive=0";
                DataSet dsfindid1 = da1.GetDataSetInline(finsql1);
                if (dsfindid1.Tables[0].Rows.Count > 0)
                {

                    iscomplete = Convert.ToInt32(dsfindid1.Tables[0].Rows[0]["iscomplete"]);

                }

                if (iscomplete == 1)
                {
                    decimal subtsktotalpercent = 100;
                    decimal totlweitagemile = 0;
                    decimal totlweitageproject = 0;

                    string finsql2 = "select isnull(sum(weightage),0)Wattagetasksum from Pms_tbl_Addsubtask where ProjectId='" + pid + "' and Milestoneid='" + mid + "' and task='" + tid + "'  and status=1 and isnull(iscomplete,0)=1 and archive=0";
                    DataSet dsfindid2 = da1.GetDataSetInline(finsql2);

                    //sub task percentage //
                    string strstatusupdate = "update Pms_tbl_Addsubtask set iscompletepercentage='" + subtsktotalpercent.ToString() + "' where id='" + sid + "' and status=1 and archive=0 and isnull(iscomplete,0)=1";
                    DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                    // task percentage //
                    string finsql3 = "update Pms_tbl_Addtask set iscompletepercentage='" + dsfindid2.Tables[0].Rows[0]["Wattagetasksum"] + "' where ProjectId='" + pid + "' and Milestoneid='" + mid + "' and id='" + tid + "' and status=1 and archive=0";
                    DataSet dsfindid3 = da1.GetDataSetInline(finsql3);


                    // Milestone percentage //
                    string findsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask  where ProjectId='" + pid + "' and Milestoneid='" + mid + "'  and status=1  and archive=0";
                    DataSet dsfindidmile = da1.GetDataSetInline(findsqlmile);
                    for (int j = 0; j < dsfindidmile.Tables[0].Rows.Count; j++)
                    {
                        decimal sumweitage = 0;
                        decimal subpercentageemile = 0;
                        sumweitage = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["Weightage"].ToString());
                        subpercentageemile = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                        totlweitagemile += (sumweitage * subpercentageemile) / 100;

                    }

                    string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile + "' where ProjectId='" + pid + "' and id='" + mid + "'  and status=1 and archive=0";
                    DataSet dsfindid4 = da1.GetDataSetInline(finsql4);



                    // project percentage //
                    string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + pid + "'    and status=1  and archive=0";
                    DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
                    for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
                    {
                        decimal sumweitageprojt = 0;
                        decimal subpercentageeproject = 0;
                        sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                        subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                        totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

                    }

                    string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + pid + "'  and status=1 and archive=0";
                    DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);

                }

                else
                {
                    decimal subtsktotalpercent = 0;
                    decimal totlweitagemile = 0;
                    decimal totlweitageproject = 0;

                    string finsql2 = "select isnull(sum(weightage),0)Wattagetasksum from Pms_tbl_Addsubtask where ProjectId='" + pid + "' and Milestoneid='" + mid + "' and task='" + tid + "'  and status=1 and isnull(iscomplete,0)=1 and archive=0";
                    DataSet dsfindid2 = da1.GetDataSetInline(finsql2);

                    //Sub task percentage //
                    string strstatusupdate = "update Pms_tbl_Addsubtask set iscompletepercentage='" + subtsktotalpercent.ToString() + "' where id='" + sid + "' and status=1 and archive=0 and isnull(iscomplete,0)=0";
                    DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                    // task percentage //
                    string finsql3 = "update Pms_tbl_Addtask set iscompletepercentage='" + dsfindid2.Tables[0].Rows[0]["Wattagetasksum"] + "' where ProjectId='" + pid + "' and Milestoneid='" + mid + "' and id='" + tid + "' and status=1 and archive=0";
                    DataSet dsfindid3 = da1.GetDataSetInline(finsql3);


                    // Milestone percentage //
                    string findsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask  where ProjectId='" + pid + "' and Milestoneid='" + mid + "'    and status=1  and archive=0";
                    DataSet dsfindidmile = da1.GetDataSetInline(findsqlmile);
                    for (int j = 0; j < dsfindidmile.Tables[0].Rows.Count; j++)
                    {
                        decimal sumweitage = 0;
                        decimal subpercentageemile = 0;
                        sumweitage = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["Weightage"].ToString());
                        subpercentageemile = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                        totlweitagemile += (sumweitage * subpercentageemile) / 100;

                    }

                    string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile + "' where ProjectId='" + pid + "' and id='" + mid + "'  and status=1 and archive=0";
                    DataSet dsfindid4 = da1.GetDataSetInline(finsql4);


                    // project percentage //
                    string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + pid + "'   and status=1  and archive=0";
                    DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
                    for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
                    {
                        decimal sumweitageprojt = 0;
                        decimal subpercentageeproject = 0;
                        sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                        subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                        totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

                    }

                    string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + pid + "'  and status=1 and archive=0";
                    DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);


                }

            }
            catch
            {
                retval = "0";
            }
        }
        return retval;
    }
    public static string managecalculation_mile(string id,string type)
    {
        function fns = new function();
        DataAccess da1 = new DataAccess();
        string retval = "1";
        string rid = id;
        string pid = "0";
        string mid = "0";
        string tid = "0";
        string sid = "0";
        
        string sqlP = "select id as mid,isnull(ProjectId,0) as pid,0 as tid,0 as sid,isnull(checkmileWeightage ,0) as checkmileWeightage from pms_tbl_addmilestone where  id=" + rid + "";
        SaveLogText("strstatusupdateAP=" + sqlP);
        try
        {
            DataSet dsD = da1.GetDataSetInline(sqlP);
            if (dsD.Tables[0].Rows.Count > 0)
            {
                pid = dsD.Tables[0].Rows[0]["pid"].ToString();
                mid = dsD.Tables[0].Rows[0]["mid"].ToString();
                tid = dsD.Tables[0].Rows[0]["tid"].ToString();
                sid = dsD.Tables[0].Rows[0]["sid"].ToString();
            }
         }
        catch {
        }
        int checkweitage = 0;
        decimal wetagecount = 0;
        decimal totalwetage = 0;
        

        try
            {
                string strweightage = "select isnull(checkprojectWeightage ,0) as checkprojectWeightage  from Pms_tbl_AddProject where id='" + pid + "'    and  isnull(checkprojectWeightage ,0)=1  and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
            
            DataSet dsweitagecheck = da1.GetDataSetInline(strweightage);
                if (dsweitagecheck.Tables[0].Rows.Count > 0)
                {

                    checkweitage = Convert.ToInt32(dsweitagecheck.Tables[0].Rows[0]["checkprojectWeightage"]);

                }
            //   iscomplete
            SaveLogText("strstatusupdateAE=" + checkweitage);
            if (checkweitage == 1)
                {
                    string strweightagecount = "select count (*) as WeitageCount from Pms_tbl_AddMilestone where ProjectId='" + pid + "' and  status=1   and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                    DataSet dsweitagecount = da1.GetDataSetInline(strweightagecount);
                    if (dsweitagecount.Tables[0].Rows.Count > 0)
                    {


                        wetagecount = Convert.ToInt32(dsweitagecount.Tables[0].Rows[0]["WeitageCount"]);

                    }

                    totalwetage = 100 / wetagecount;
                SaveLogText("strstatusupdateA1="+ totalwetage+","+ wetagecount);
                    string strstatusupdate1 = "update Pms_tbl_AddMilestone set Weightage='" + fns.ToFormattedCurrencyString(totalwetage.ToString()) + "' where ProjectId='" + pid + "' and status=1  and archive=0";
                SaveLogText("strstatusupdateA2=" + strstatusupdate1);
                da1.executeDMLQueryinline(strstatusupdate1);
                //da1.GetDataSetInline(strstatusupdate1);

                }

                    string finsql2 = "";
                    DataSet dsfindid2;
                    decimal milestonetotalpercent = 0;
                    decimal totlweitagemile = 0;
                    decimal totlweitageproject = 0;
                    string finsql1 = "select * from Pms_tbl_AddMilestone where id=" + mid + " and status=1 and archive=0";
                    DataSet dsfindid1 = da1.GetDataSetInline(finsql1);
                    if (dsfindid1.Tables[0].Rows.Count > 0)
                    {
                        if (dsfindid1.Tables[0].Rows[0]["istask"].ToString() == "1")
                        {
                            finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where  Milestoneid='" + mid + "'  and status=1  and archive=0";
                            dsfindid2 = da1.GetDataSetInline(finsql2);
                            for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
                            {
                                decimal sumweitage = 0;
                                decimal subpercentageemile = 0;
                                sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
                                subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                                totlweitagemile += (sumweitage * subpercentageemile) / 100;

                            }
                        }

                    }






                    // Milestone percentage //
                    string strstatusupdate = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile.ToString() + "' where id='" + mid + "' and status=1 and archive=0 and isnull(iscomplete,0)=0";
                    DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                    // project percentage //
                    finsql2 = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + pid + "'  and status=1  and archive=0";
                    dsfindid2 = da1.GetDataSetInline(finsql2);
                    for (int j = 0; j < dsfindid2.Tables[0].Rows.Count; j++)
                    {
                        decimal sumweitage = 0;
                        decimal subpercentageemile = 0;
                        sumweitage = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["Weightage"].ToString());
                        subpercentageemile = Convert.ToDecimal(dsfindid2.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                        totlweitageproject += (sumweitage * subpercentageemile) / 100;

                    }
                    string finsql4 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + pid + "'  and status=1 and archive=0";
                    DataSet dsfindid4 = da1.GetDataSetInline(finsql4);



                
            }
            catch(Exception ex) {
            SaveLogText("strstatusupdateAE="+ex.Message);
            retval = "0"; }
       
            return retval;
    }
    public static string managecalculation_task(string id, string type)
    {
        function fns = new function();
        DataAccess da1 = new DataAccess();
        string retval = "1";
        string rid = id;
        string pid = "0";
        string mid = "0";
        string tid = "0";
        string sid = "0";

        string sqlP = "select id as tid, isnull(Milestoneid,0) as mid,isnull(ProjectId,0) as pid,0 as sid from pms_tbl_addtask where  id=" + rid + "";
        

        try
        {
            DataSet dsD = da1.GetDataSetInline(sqlP);
            if (dsD.Tables[0].Rows.Count > 0)
            {
                pid = dsD.Tables[0].Rows[0]["pid"].ToString();
                mid = dsD.Tables[0].Rows[0]["mid"].ToString();
                tid = dsD.Tables[0].Rows[0]["tid"].ToString();
                sid = dsD.Tables[0].Rows[0]["sid"].ToString();
            }
        }
        catch
        {
        }
        int checkweitage = 0;
        decimal wetagecount = 0;
        decimal totalwetage = 0;

            try
            {
            //string strweightage = "select isnull(checkprojectWeightage ,0) as checkprojectWeightage  from Pms_tbl_AddProject where id='" + pid + "' and  status=1 and  isnull(checkprojectWeightage ,0)=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
            //DataSet dsweitagecheck = da1.GetDataSetInline(strweightage);
            //if (dsweitagecheck.Tables[0].Rows.Count > 0)
            //{

            //    checkweitage = Convert.ToInt32(dsweitagecheck.Tables[0].Rows[0]["checkprojectWeightage"]);

            //}
            string strweightage = "select isnull(checkmileWeightage ,0) as checkmileWeightage  from Pms_tbl_AddMilestone where id='" + mid + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
            DataSet dsweitagecheck = da1.GetDataSetInline(strweightage);
            if (dsweitagecheck.Tables[0].Rows.Count > 0)
            {

                checkweitage = Convert.ToInt32(dsweitagecheck.Tables[0].Rows[0]["checkmileWeightage"]);

            }

            if (checkweitage == 1)
                {
                    string strweightagecount = "select count (*) as WeitageCount from Pms_tbl_Addtask where Milestoneid='" + mid + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                    DataSet dsweitagecount = da1.GetDataSetInline(strweightagecount);
                    if (dsweitagecount.Tables[0].Rows.Count > 0)
                    {

                        wetagecount = Convert.ToInt32(dsweitagecount.Tables[0].Rows[0]["WeitageCount"]);

                    }

                    totalwetage = 100 / wetagecount;
                    string strstatusupdate = "update Pms_tbl_Addtask set Weightage='" + fns.ToFormattedCurrencyString(totalwetage.ToString()) + "' where Milestoneid='" + mid + "' and status=1 and archive=0";
                    DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                }

                    decimal tsktotalpercent = 0;
                    decimal totlweitagemile = 0;
                    decimal totlweitageproject = 0;

                    string finsql01 = "select * from Pms_tbl_Addtask where id=" + tid + " and status=1  and archive=0";
                    DataSet dsfindid01 = da1.GetDataSetInline(finsql01);


                    if (dsfindid01.Tables[0].Rows.Count > 0)
                    {
                        if (dsfindid01.Tables[0].Rows[0]["issubtask"].ToString() == "1")
                        {

                            string finsql211 = "select isnull(sum(weightage),0)Wattagetasksum from Pms_tbl_Addsubtask where   task='" + tid + "'  and status=1 and isnull(iscomplete,0)=1 and archive=0";
                            DataSet dsfindid211 = da1.GetDataSetInline(finsql211);
                            if (dsfindid211.Tables[0].Rows.Count > 0)
                            {
                                string strstatusupdate = "update Pms_tbl_Addtask set iscompletepercentage='" + dsfindid211.Tables[0].Rows[0]["Wattagetasksum"] + "' where id='" + tid + "' and status=1 and archive=0";
                                DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);
                            }
                        }
                        else
                        {
                            string strstatusupdate = "update Pms_tbl_Addtask set iscompletepercentage='0' where id='" + tid + "' and status=1 and archive=0";
                            DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                        }

                    }


                    // Milestone percentage //
                    string finsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask where ProjectId='" + pid + "' and Milestoneid='" + mid + "'   and status=1  and archive=0";
                    DataSet dsfindmile = da1.GetDataSetInline(finsqlmile);
                    for (int j = 0; j < dsfindmile.Tables[0].Rows.Count; j++)
                    {
                        decimal sumweitage = 0;
                        decimal subpercentageemile = 0;
                        sumweitage = Convert.ToDecimal(dsfindmile.Tables[0].Rows[j]["Weightage"].ToString());
                        subpercentageemile = Convert.ToDecimal(dsfindmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                        totlweitagemile += (sumweitage * subpercentageemile) / 100;

                    }

                    string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile.ToString() + "' where ProjectId='" + pid + "' and id='" + mid + "'  and status=1 and archive=0";
                    DataSet dsfindid4 = da1.GetDataSetInline(finsql4);

                    // project percentage //
                    string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + pid + "'   and status=1  and archive=0";
                    DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
                    for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
                    {
                        decimal sumweitageprojt = 0;
                        decimal subpercentageeproject = 0;
                        sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                        subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                        totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

                    }

                    string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + pid + "'  and status=1 and archive=0";
                    DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);



                
            }
            catch { retval = "0"; }
       
        return retval;
    }
    public static string managecalculation_subtask(string id, string type)
    {
        function fns = new function();
        DataAccess da1 = new DataAccess();
        string retval = "1";
        string rid = id;
        string pid = "0";
        string mid = "0";
        string tid = "0";
        string sid = "0";

        string sqlP = "select id as sid,isnull(task,0) as tid, isnull(Milestoneid,0) as mid,isnull(ProjectId,0) as pid from pms_tbl_addsubtask where  id=" + rid + "";
        

        try
        {
            DataSet dsD = da1.GetDataSetInline(sqlP);
            if (dsD.Tables[0].Rows.Count > 0)
            {
                pid = dsD.Tables[0].Rows[0]["pid"].ToString();
                mid = dsD.Tables[0].Rows[0]["mid"].ToString();
                tid = dsD.Tables[0].Rows[0]["tid"].ToString();
                sid = dsD.Tables[0].Rows[0]["sid"].ToString();
            }
        }
        catch
        {
        }
        int checkweitage = 0;
        decimal wetagecount = 0;
        decimal totalwetage = 0;

            try
            {
                string strweightage = "select isnull(checkTaskWeightage ,0) as checkTaskWeightage  from pms_tbl_addtask where id='" + tid + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                DataSet dsweitagecheck = da1.GetDataSetInline(strweightage);
                if (dsweitagecheck.Tables[0].Rows.Count > 0)
                {

                    checkweitage = Convert.ToInt32(dsweitagecheck.Tables[0].Rows[0]["checkTaskWeightage"]);

                }
                if (checkweitage == 1)
                {
                    string strweightagecount = "select count (*) as WeitageCount from Pms_tbl_Addsubtask where task='" + tid + "' and  status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
                    DataSet dsweitagecount = da1.GetDataSetInline(strweightagecount);
                    if (dsweitagecount.Tables[0].Rows.Count > 0)
                    {

                        wetagecount = Convert.ToInt32(dsweitagecount.Tables[0].Rows[0]["WeitageCount"]);

                    }

                    totalwetage = 100 / wetagecount;
                    string strstatusupdate1 = "update Pms_tbl_Addsubtask set Weightage='" + fns.ToFormattedCurrencyString(totalwetage.ToString()) + "' where task='" + tid + "' and status=1 and archive=0";
                    DataSet dshourmin1 = da1.GetDataSetInline(strstatusupdate1);

                }

                    decimal subtsktotalpercent = 0;
                    decimal totlweitagemile = 0;
                    decimal totlweitageproject = 0;

                    string finsql2 = "select isnull(sum(weightage),0) Wattagetasksum from Pms_tbl_Addsubtask where ProjectId='" + pid + "' and Milestoneid='" + mid + "' and task='" + tid + "'  and status=1 and isnull(iscomplete,0)=1 and archive=0";
                    DataSet dsfindid2 = da1.GetDataSetInline(finsql2);

                    //Sub task percentage //
                    string strstatusupdate = "update Pms_tbl_Addsubtask set iscompletepercentage='" + subtsktotalpercent.ToString() + "' where id='" + sid + "' and status=1 and archive=0 and isnull(iscomplete,0)=0";
                    DataSet dshourmin = da1.GetDataSetInline(strstatusupdate);

                    // task percentage //
                    string finsql3 = "update Pms_tbl_Addtask set iscompletepercentage='" + dsfindid2.Tables[0].Rows[0]["Wattagetasksum"] + "' where ProjectId='" + pid + "' and Milestoneid='" + mid + "' and id='" + tid + "' and status=1 and archive=0";
                    DataSet dsfindid3 = da1.GetDataSetInline(finsql3);


                    // Milestone percentage //
                    string findsqlmile = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_Addtask  where ProjectId='" + pid + "' and Milestoneid='" + mid + "'    and status=1  and archive=0";
                    DataSet dsfindidmile = da1.GetDataSetInline(findsqlmile);
                    for (int j = 0; j < dsfindidmile.Tables[0].Rows.Count; j++)
                    {
                        decimal sumweitage = 0;
                        decimal subpercentageemile = 0;
                        sumweitage = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["Weightage"].ToString());
                        subpercentageemile = Convert.ToDecimal(dsfindidmile.Tables[0].Rows[j]["iscompletepercentage"].ToString());
                        totlweitagemile += (sumweitage * subpercentageemile) / 100;

                    }

                    string finsql4 = "update Pms_tbl_AddMilestone set iscompletepercentage='" + totlweitagemile + "' where ProjectId='" + pid + "' and id='" + mid + "'  and status=1 and archive=0";
                    DataSet dsfindid4 = da1.GetDataSetInline(finsql4);


                    // project percentage //
                    string finsqlproject = "select isnull(Weightage,0) as Weightage,isnull(iscompletepercentage,0) as iscompletepercentage from Pms_tbl_AddMilestone where ProjectId='" + pid + "'   and status=1  and archive=0";
                    DataSet dsfindidproject = da1.GetDataSetInline(finsqlproject);
                    for (int k = 0; k < dsfindidproject.Tables[0].Rows.Count; k++)
                    {
                        decimal sumweitageprojt = 0;
                        decimal subpercentageeproject = 0;
                        sumweitageprojt = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["Weightage"].ToString());
                        subpercentageeproject = Convert.ToDecimal(dsfindidproject.Tables[0].Rows[k]["iscompletepercentage"].ToString());
                        totlweitageproject += (sumweitageprojt * subpercentageeproject) / 100;

                    }

                    string finsqlproject1 = "update Pms_tbl_AddProject set iscompletepercentage='" + totlweitageproject.ToString() + "' where id='" + pid + "'  and status=1 and archive=0";
                    DataSet dsfindidproject1 = da1.GetDataSetInline(finsqlproject1);


                

            }
            catch
            {
                retval = "0";
            }
      
        return retval;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static completiondetails_mile bindcompletiondetails_mile(string RecordId)
    {
        DataAccess da = new DataAccess();
        completiondetails_mile obj = new completiondetails_mile();
        try
        {
            if (string.IsNullOrEmpty(RecordId))
                RecordId = "0";
            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null)
                return obj;
            string regid = regCookie["id"];
            string strsql = "select * from Pms_tbl_Addmilestone where id=" + RecordId + " and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
            SqlDataReader dr = da.getDataReaderInline(strsql);
            while (dr.Read())
            {
                obj.id = dr["id"] != DBNull.Value ? Convert.ToInt32(dr["id"]) : (int?)null;
                obj.ProjectId = dr["ProjectId"] != DBNull.Value ? Convert.ToInt32(dr["ProjectId"]) : (int?)null;
                double mileTaskTotal = 0;
                double mileWattagepending = 100;
                try
                {

                    mileTaskTotal = get_completionstatus(RecordId, "milestone");
                    if (mileTaskTotal > 0)
                    {
                        mileWattagepending = mileWattagepending - mileTaskTotal;
                    }
                }
                catch { }
                obj.complete_value = mileTaskTotal;
                obj.pending_value = mileWattagepending;
                List<newtask_details> newtasklist = bindtaskdetails_bymileid(RecordId);
                obj.tasks_details = newtasklist;
                

            }


        }
        catch (Exception ex)
        {
            SaveLogText("chkadmintaskEX-" + ex.Message);
            return new completiondetails_mile();
        }


        return obj;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static newtask_details bindcompletiondetails_task(string RecordId)
    {
        DataAccess da = new DataAccess();
        newtask_details obj = new newtask_details();
        try
        {
            if (string.IsNullOrEmpty(RecordId))
                RecordId = "0";
            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null)
                return obj;
            string regid = regCookie["id"];
            string strsql = "select * from Pms_tbl_Addtask where id=" + RecordId + " and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
            SqlDataReader dr = da.getDataReaderInline(strsql);
            while (dr.Read())
            {
                obj.id = dr["id"] != DBNull.Value ? Convert.ToInt32(dr["id"]) : (int?)null;
                obj.ProjectId = dr["ProjectId"] != DBNull.Value ? Convert.ToInt32(dr["ProjectId"]) : (int?)null;
                obj.Milestoneid = dr["Milestoneid"] != DBNull.Value ? Convert.ToInt32(dr["Milestoneid"]) : (int?)null;
                double mileTaskTotal = 0;
                double mileWattagepending = 100;
                try
                {

                    mileTaskTotal = get_completionstatus(RecordId, "task");
                    if (mileTaskTotal > 0)
                    {
                        mileWattagepending = mileWattagepending - mileTaskTotal;
                    }
                }
                catch { }
                obj.complete_value = mileTaskTotal;
                obj.pending_value = mileWattagepending;
                List<newsubtask_details> newsubtasklist = bindsubtaskdetails_bytaskid(RecordId);
                obj.subtasks_details = newsubtasklist;


            }


        }
        catch (Exception ex)
        {
            SaveLogText("chkadmintaskEX-" + ex.Message);
            return new newtask_details();
        }


        return obj;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static newsubtask_details bindcompletiondetails_subtask(string RecordId)
    {
        DataAccess da = new DataAccess();
        newsubtask_details obj = new newsubtask_details();
        try
        {
            if (string.IsNullOrEmpty(RecordId))
                RecordId = "0";
            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null)
                return obj;
            string regid = regCookie["id"];
            string strsql = "select * from Pms_tbl_Addsubtask where id=" + RecordId + " and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'";
            SqlDataReader dr = da.getDataReaderInline(strsql);
            while (dr.Read())
            {
                obj.id = dr["id"] != DBNull.Value ? Convert.ToInt32(dr["id"]) : (int?)null;
                obj.ProjectId = dr["ProjectId"] != DBNull.Value ? Convert.ToInt32(dr["ProjectId"]) : (int?)null;
                obj.Milestoneid = dr["Milestoneid"] != DBNull.Value ? Convert.ToInt32(dr["Milestoneid"]) : (int?)null;
                obj.Task = dr["Task"] != DBNull.Value ? Convert.ToInt32(dr["Task"]) : (int?)null;
                double mileTaskTotal = 0;
                double mileWattagepending = 100;
                try
                {

                    mileTaskTotal = get_completionstatus(RecordId, "subtask");
                    if (mileTaskTotal > 0)
                    {
                        mileWattagepending = mileWattagepending - mileTaskTotal;
                    }
                }
                catch { }
                obj.complete_value = mileTaskTotal;
                obj.pending_value = mileWattagepending;
               
            }


        }
        catch (Exception ex)
        {
            SaveLogText("chkadmintaskEX-" + ex.Message);
            return new newsubtask_details();
        }


        return obj;
    }

    [WebMethod]  
    public static string gettimespam(string id, string tasktype,string owner)
    {
        CommonFunction cmf = new CommonFunction();
        string retval = "";
        DataAccess da = new DataAccess();

        string hour = "00";
        string min = "00";
        string sec = "00";
        string mili = "00";
        string transactionid = "0";
        try
        {

           
            string strsqlselect = "select id,task,Milestoneid,Projectid from pms_tbl_addsubtask with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id=" + id + "";
            if (tasktype == "task")
                strsqlselect = "select id,id as Task,Milestoneid,Projectid from pms_tbl_addtask with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id=" + id + "";
            if (tasktype == "milestone")
                strsqlselect = "select id,'' Task,id Milestoneid,Projectid from pms_tbl_addmilestone with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and id=" + id + "";




            DataSet dsT = da.GetDataSetInline(strsqlselect);
            if (dsT.Tables[0].Rows.Count > 0)
            {

                transactionid = "0";
                string pausetime = "";
                int ispause = 0;
                int isrun = 0;
                int isend = 0;

                //isnull(isend,0)=0 and
                string strhourminuts = "select *  from Pms_tbl_Timerstatus with(nolock) where  type='" + tasktype + "' and subtaskid='" + id + "' and taskid='" + dsT.Tables[0].Rows[0]["task"].ToString() + "' and ProjectId='" + dsT.Tables[0].Rows[0]["ProjectId"].ToString() + "' and Milestoneid='" + dsT.Tables[0].Rows[0]["Milestoneid"].ToString() + "' and uid='" + owner + "'  and status=1 and archive=0 order by id desc";
                if (tasktype == "task")
                    strhourminuts = "select *  from Pms_tbl_Timerstatus with(nolock) where  type='" + tasktype + "' and taskid='" + id + "' and ProjectId='" + dsT.Tables[0].Rows[0]["ProjectId"].ToString() + "' and Milestoneid='" + dsT.Tables[0].Rows[0]["Milestoneid"].ToString() + "' and uid='" + owner + "'  and status=1 and archive=0 order by id desc";
                else if (tasktype == "milestone")
                    strhourminuts = "select *  from Pms_tbl_Timerstatus with(nolock) where  type='" + tasktype + "' and Milestoneid='" + id + "'  and ProjectId='" + dsT.Tables[0].Rows[0]["ProjectId"].ToString() + "'  and uid='" + HttpContext.Current.Request.Cookies["user"]["user_id"].ToString() + "'  and status=1 and archive=0 order by id desc";


                if (id == "2364")
                    SaveLogText("strpauseddetailsCHKA-" + strhourminuts);
                DataSet dshourmin = da.GetDataSetInline(strhourminuts);
                if (dshourmin.Tables[0].Rows.Count > 0)
                {
                    transactionid = dshourmin.Tables[0].Rows[0]["id"].ToString();
                    if (dshourmin.Tables[0].Rows[0]["Isend"].ToString() == "1")
                    {
                        hour = "00";
                        min = "00";
                        sec = "00";
                        mili = "00";
                    }
                    else if (dshourmin.Tables[0].Rows[0]["IsPaused"].ToString() == "1")
                    {
                        hour = dshourmin.Tables[0].Rows[0]["hour"].ToString();
                        min = dshourmin.Tables[0].Rows[0]["minute"].ToString();
                        sec = dshourmin.Tables[0].Rows[0]["second"].ToString();
                        mili = dshourmin.Tables[0].Rows[0]["millisecond"].ToString();

                    }
                    else
                    {
                        string chkpausemin = "0";
                        string strpauseddetailsCHK = "select top(1) id,tenddate  from Pms_Tbl_PauseStatus with(nolock) where Transactionid='" + transactionid + "'  and status=1 and archive=0 and type='" + tasktype + "' and isnumeric(isnull(PausedTime,0))=1 and tenddate is not null order by id desc";

                        if (transactionid == "2")
                            SaveLogText("strpauseddetailsCHK1-" + strpauseddetailsCHK);
                        DataSet dspausedCHK = da.GetDataSetInline(strpauseddetailsCHK);
                        if (dspausedCHK.Tables[0].Rows.Count > 0)
                        {
                            string endatetime = dspausedCHK.Tables[0].Rows[0]["tenddate"].ToString();
                            if (transactionid == "2")
                                SaveLogText("strpauseddetailsCHK4E-" + endatetime);
                            if (endatetime == string.Empty)
                            {
                                endatetime = dshourmin.Tables[0].Rows[0]["StartTime"].ToString();
                            }
                            string strpauseddetails2 = "select Transactionid, sum(cast(isnull(PausedTime,0) as numeric)) tpause  from Pms_Tbl_PauseStatus with(nolock) where Transactionid='" + transactionid + "'  and status=1 and archive=0 and type='" + tasktype + "' and isnumeric(isnull(PausedTime,0))=1 group by Transactionid";
                            if (transactionid == "2")
                                SaveLogText("strpauseddetailsCHK2-" + strpauseddetails2);
                            DataSet dspaused2 = da.GetDataSetInline(strpauseddetails2);
                            if (dspaused2.Tables[0].Rows.Count > 0)
                            {
                                chkpausemin = (dspaused2.Tables[0].Rows[0]["tpause"].ToString() == "" ? "0" : dspaused2.Tables[0].Rows[0]["tpause"].ToString());
                            }

                            TimeSpan toatlhr = (DateTime.Now - Convert.ToDateTime(endatetime));
                            TimeSpan t1 = TimeSpan.FromMinutes(Convert.ToInt32(chkpausemin));
                            if (transactionid == "")
                                SaveLogText("strpauseddetailsCHK4-" + toatlhr);
                            int diffdays = (toatlhr.Days) * 24;
                            toatlhr = toatlhr.Add(t1);
                            hour = (toatlhr.Hours + diffdays).ToString();
                            min = toatlhr.Minutes.ToString();
                            sec = toatlhr.Seconds.ToString();
                            // mili = toatlhr.Milliseconds.ToString();

                        }
                        else
                        {
                            TimeSpan toatlhr = (DateTime.Now - Convert.ToDateTime(dshourmin.Tables[0].Rows[0]["StartTime"].ToString()));
                            TimeSpan t1 = TimeSpan.FromMinutes(Convert.ToInt32(chkpausemin));
                            //TimeSpan t1 = new TimeSpan(0,Convert.ToInt32(chkpausemin) ,0,0);
                            int diffdays = (toatlhr.Days) * 24;
                            toatlhr = toatlhr.Subtract(t1);
                            hour = (toatlhr.Hours + diffdays).ToString();
                            min = toatlhr.Minutes.ToString();
                            sec = toatlhr.Seconds.ToString();
                            mili = toatlhr.Milliseconds.ToString();
                        }



                    }


                }

            }
        }
        catch (Exception ex) { SaveLogText("strhourminutsA1EX-" + ex.Message); }
        retval = hour + ":" + min + ":" + sec + ":" + mili;
        if (transactionid == "1030")
            SaveLogText("strpauseddetailsCHK3-" + retval);
        //SaveLogText("strhourminutsA1-"+retval);
        return retval;
    }

    public static List<newsubtask_details>bindsubtasklist_bytaskid_details(string RecordId)
    {
        DataAccess da = new DataAccess();
        List<newsubtask_details> obj = new List<newsubtask_details>();
        try
        {
            if (string.IsNullOrEmpty(RecordId))
                RecordId = "0";

            var regCookie = HttpContext.Current.Request.Cookies["cadmin"];
            if (regCookie == null)
                return obj;

            string regid = regCookie["id"];
            try
            {
                string str2 = "select * from Pms_tbl_Addsubtask where regid='" + regid + "' and task='" + RecordId + "' and status=1 and archive=0 order by isnull(orderno,0)";


                DataAccess danew = new DataAccess();
                DataSet drsubtask = danew.GetDataSetInline(str2);
                foreach (DataRow row in drsubtask.Tables[0].Rows)
                {
                    double mileTaskTotal = 0;
                    mileTaskTotal = get_completionstatus(row["id"].ToString(), "subtask");
                    double mileWattagecomplite = mileTaskTotal;
                    double mileWattagepending = 100;
                    if (mileTaskTotal > 0)
                    {
                        mileWattagepending = mileWattagepending - mileTaskTotal;
                    }
                    obj.Add(new newsubtask_details
                    {

                        id = row["id"] as int?,
                        ProjectId = row["ProjectId"] as int?,
                        Milestoneid = row["Milestoneid"] as int?,
                        Task = row["task"] as int?,
                        complete_value = Math.Round(mileTaskTotal, 2),
                        pending_value = Math.Round(mileWattagepending, 2),

                    });
                }

            }
            catch { }




        }
        catch (Exception ex)
        {
            SaveLogText("chkadminsubtaskEX-" + ex.Message);
            return new List<newsubtask_details>();
        }


        return obj;
    }
    #endregion
    public static int GetWeekNumber(DateTime dtime)
    {
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        int weekno = 0;
        SqlParameter[] prmw = new SqlParameter[1];
        prmw[0] = new SqlParameter("@DATE", dtime);

        DataSet dsw = da.GetDataSet("selectweeknofromdate", prmw);
        if (dsw.Tables[0].Rows.Count > 0)
        {
            weekno = Convert.ToInt32(dsw.Tables[0].Rows[0]["WEEK_OF_MONTH"].ToString());
        }
        return weekno;
        //int count = 0;
        //if (dtime.Day <= 7)
        //{
        //    count = 1;
        //}
        //else if (dtime.Day > 7 && dtime.Day <= 14)
        //{
        //    count = 2;
        //}
        //else if (dtime.Day > 14 && dtime.Day <= 21)
        //{
        //    count = 3;
        //}
        //else if (dtime.Day > 21 && dtime.Day <= 28)
        //{
        //    count = 4;
        //}
        //else if (dtime.Day > 28)
        //{
        //    count = 5;
        //}

        //return count;
    }

    public static int GetdayNumber(DateTime dtime)
    {
        //    CultureInfo ciCurr = CultureInfo.CurrentCulture;
        //    int weekNum = ciCurr.Calendar.GetWeekOfYear(dtPassed, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        //    return weekNum;
        //}

        int count = 0;
        int day = (int)dtime.DayOfWeek;
        count = day + 1;
        return count;
    }
    public static string check_weekoffholyday(string pid)
    {
        string ret = "";
        if(pid!="0")
        {
            try
            {
                string strsql = "select id,isnull(iscountholydays,0) as iscountholydays,isnull(iscountweekoff,0) as iscountweekoff from pms_tbl_addproject with(nolock) where id=" + pid + "";
                SaveLogText("weekoffholyday11-" + strsql);
                DataAccess da = new DataAccess();
                DataSet dsh = da.GetDataSetInline(strsql);
                if (dsh.Tables[0].Rows.Count > 0)
                {
                    ret = dsh.Tables[0].Rows[0]["iscountholydays"].ToString() + "," + dsh.Tables[0].Rows[0]["iscountweekoff"].ToString();
                }
            }
            catch { ret = ""; }
        }
        return ret;
    }
    public static bool getweeklyoff(DateTime edate, string empid = "")
    {
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();

        int wcount = 0;
        int dno = 0;
        wcount = GetWeekNumber(Convert.ToDateTime(edate));
        dno = GetdayNumber(Convert.ToDateTime(edate));
        SaveLogText("dayno-" + dno + "-" + edate);
        string w1, wt1, w2, wt2, w3, wt3, w4, wt4, w5, wt5, w6, wt6, w7, wt7;
        string strret = "";

        string strsql = "select * from tbl_weekoffmaster where  status = 1 and archive = 0 and isnull(isdefault,0)=1 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'  ";
        SaveLogText("strsqlweekoff-" + strsql);
        DataSet dsh = da.GetDataSetInline(strsql);

        //SqlParameter[] prmh = new SqlParameter[1];
        //prmh[0] = new SqlParameter("@empid", empid);
        //DataSet dsh = da.GetDataSet("WeeklyOffPolicybyempid", prmh);

        if (dsh.Tables[0].Rows.Count > 0)
        {

            w1 = dsh.Tables[0].Rows[0]["isweekoff1"].ToString();
            wt1 = dsh.Tables[0].Rows[0]["isweekoff1type"].ToString();
            w2 = dsh.Tables[0].Rows[0]["isweekoff2"].ToString();
            wt2 = dsh.Tables[0].Rows[0]["isweekoff2type"].ToString();
            w3 = dsh.Tables[0].Rows[0]["isweekoff3"].ToString();
            wt3 = dsh.Tables[0].Rows[0]["isweekoff3type"].ToString();
            w4 = dsh.Tables[0].Rows[0]["isweekoff4"].ToString();
            wt4 = dsh.Tables[0].Rows[0]["isweekoff4type"].ToString();
            w5 = dsh.Tables[0].Rows[0]["isweekoff5"].ToString();
            wt5 = dsh.Tables[0].Rows[0]["isweekoff5type"].ToString();
            w6 = dsh.Tables[0].Rows[0]["isweekoff6"].ToString();
            wt6 = dsh.Tables[0].Rows[0]["isweekoff6type"].ToString();
            w7 = dsh.Tables[0].Rows[0]["isweekoff7"].ToString();
            wt7 = dsh.Tables[0].Rows[0]["isweekoff7type"].ToString();
            int returntype = 0;
            if (w1 == "1" && dno.ToString() == "1")
            {
                returntype = getreturnstatus(wt1, wcount);

            }
            if (w2 == "1" && dno.ToString() == "2")
            {
                returntype = getreturnstatus(wt2, wcount);

            }

            if (w3 == "1" && dno.ToString() == "3")
            {
                returntype = getreturnstatus(wt3, wcount);

            }

            if (w4 == "1" && dno.ToString() == "4")
            {
                returntype = getreturnstatus(wt4, wcount);

            }

            if (w5 == "1" && dno.ToString() == "5")
            {
                returntype = getreturnstatus(wt5, wcount);

            }

            if (w6 == "1" && dno.ToString() == "6")
            {
                returntype = getreturnstatus(wt6, wcount);

            }
            if (w7 == "1" && dno.ToString() == "7")
            {
                returntype = getreturnstatus(wt7, wcount);

            }
            SaveLogText("returntype-" + returntype);
            if (returntype == 1)
            {
                return true;
            }
        }


        return false;

        //}
        //catch (Exception ex)
        //{
        //    return false;
        //}
    }
    public static bool getholiday(DateTime edate, string empid = "")
    {
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        try
        {

            // Response.Write(cmf.Decrypt(empid) + "," + edate.ToShortDateString());
            //SqlParameter[] prmh = new SqlParameter[2];
            //prmh[0] = new SqlParameter("@empid", empid);
            //prmh[1] = new SqlParameter("@cdate", edate);
            //DataSet dsh = da.GetDataSet("holidaysselectbydate", prmh);
            string strsql = "select * from holidays where cast(eventdate as date)=convert(date,'" + ToSqlDateOnly(edate) + "',112) and status = 1 and archive = 0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "'    ";
            DataSet dsh = da.GetDataSetInline(strsql);
            if (dsh.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                //SqlParameter[] prmrh = new SqlParameter[2];
                //prmrh[0] = new SqlParameter("@empid", empid);
                //prmrh[1] = new SqlParameter("@cdate", edate);
                //DataSet dshr = da.GetDataSet("resholidaysselectbydate", prmrh);
                //if (dshr.Tables[0].Rows.Count > 0)
                //{
                //    return true;
                //}
                //else
                //{

                //    return false;
                //}

                return false;
            }
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    public static int getreturnstatus(string wt1chk, int wcountchk)
    {
        if (wt1chk == "1")
        {
            return 1;
        }
        if (wt1chk == "2" && wcountchk == 1)
        {
            return 1;
        }
        if (wt1chk == "3" && wcountchk == 2)
        {
            return 1;
        }
        if (wt1chk == "4" && wcountchk == 3)
        {
            return 1;
        }
        if (wt1chk == "5" && wcountchk == 4)
        {
            return 1;
        }
        if (wt1chk == "6" && wcountchk == 5)
        {
            return 1;
        }
        if (wt1chk == "7" && (wcountchk == 1 || wcountchk == 3))
        {
            return 1;
        }
        if (wt1chk == "8" && (wcountchk == 1 || wcountchk == 3 || wcountchk == 5))
        {
            return 1;
        }
        if (wt1chk == "9" && (wcountchk == 2 || wcountchk == 4))
        {
            return 1;
        }

        return 0;
    }



    

    private static object ToDbInt(object value, bool defaultZero = false)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            return defaultZero ? (object)0 : DBNull.Value;

        int result;
        return int.TryParse(value.ToString(), out result)
            ? (object)result
            : (defaultZero ? (object)0 : DBNull.Value);
    }
    


    public static string SafeGetString(IDataReader dr, string col)
    {
        try
        {
            int ordinal = dr.GetOrdinal(col);
            return dr.IsDBNull(ordinal) ? "" : dr.GetValue(ordinal).ToString();
        }
        catch
        {
            return "";
        }
    }

    public static DateTime? SafeGetDate(IDataReader dr, string col)
    {
        int ordinal = dr.GetOrdinal(col);
        return dr.IsDBNull(ordinal) ? (DateTime?)null : GetNullableDate(dr.GetValue(ordinal));
    }
   
    public static DateTime? GetNullableDate(object value)
    {
        if (value == null || value == DBNull.Value) return null;
        if (value is DateTime) return (DateTime)value;

        string text = Convert.ToString(value, CultureInfo.InvariantCulture);
        if (String.IsNullOrWhiteSpace(text)) return null;

        DateTime parsed;
        string[] dateTimeFormats = new string[]
        {
            "yyyy-MM-ddTHH:mm:ss", "yyyy-MM-ddTHH:mm:ss.fff", "yyyy-MM-ddTHH:mm:ss.FFFFFFF",
            "yyyy-MM-ddTHH:mm:ssK", "yyyy-MM-ddTHH:mm:ss.fffK", "yyyy-MM-ddTHH:mm:ss.FFFFFFFK",
            "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd HH:mm:ss.fff",
            "yyyy-MM-dd", "yyyyMMdd",
            "MM/dd/yyyy hh:mm tt", "M/d/yyyy h:mm tt",
            "dd/MM/yyyy hh:mm tt", "d/M/yyyy h:mm tt",
            "MM/dd/yyyy", "M/d/yyyy", "dd/MM/yyyy", "d/M/yyyy",
            "dd-MM-yyyy", "d-M-yyyy"
        };

        if (DateTime.TryParseExact(text.Trim(), dateTimeFormats, CultureInfo.InvariantCulture,
            DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.RoundtripKind, out parsed))
            return parsed;

        return null;
    }
    private static string GetDateOnlyString(object value)
    {
        if (value == null || value == DBNull.Value)
            return null;

        DateTime dateValue;
        if (!TryParseDateOnlyInvariant(value, out dateValue))
            return null;

        // Universal date-only response contract:
        // return yyyy-MM-dd text, never an ASP.NET /Date(milliseconds)/
        // timestamp. Browsers in India, Dubai, USA, etc. receive the same
        // calendar date and perform no timezone conversion.
        return dateValue.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
    }
    public static bool? SafeGetBool(IDataReader dr, string col)
    {
        int ordinal = dr.GetOrdinal(col);
        return dr.IsDBNull(ordinal) ? (bool?)null : Convert.ToBoolean(dr.GetValue(ordinal));
    }

    public static decimal? SafeGetDecimal(IDataReader dr, string col)
    {
        int ordinal = dr.GetOrdinal(col);
        return dr.IsDBNull(ordinal) ? (decimal?)null : Convert.ToDecimal(dr.GetValue(ordinal));
    }


    public static List<T>getemployeelist<T>(string ids) where T : IPerson, new()
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
    public static int checktaskby_milestoneid(string mid)
     {
        int ret = 0;
        try
        {
            string str2 = "select id from Pms_tbl_Addtask where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and Milestoneid ='" + mid + "'";
            SaveLogText("gettasklist="+ str2);
            DataAccess danew = new DataAccess();
            DataSet drtsk = danew.GetDataSetInline(str2);
            if (drtsk.Tables[0].Rows.Count > 0)
            {
                ret = 1;
            }
        }
        catch { }
        return ret;
     }
    public static string getassignid(string id,string type)
    {
        type = NormalizeAssignmentTaskType(type);
        string ret = "0";
        try
        {
            string str2 = "select top(1) id,isnull(ownerid,0) as assignid from Pms_tbl_assigntasks with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and subtaskid ='" + id + "' and tasktype='"+type+ "' order by id desc";
            if(type=="task")
            {
                str2 = "select top(1)  id,isnull(ownerid,0) as assignid from Pms_tbl_assigntasks with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and taskid ='" + id + "' and tasktype='" + type + "' order by id desc";
            }
            else if (type == "milestone")
            {
                str2 = "select top(1) id,isnull(ownerid,0) as assignid from Pms_tbl_assigntasks with(nolock) where status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' and milestoneid ='" + id + "' and tasktype='" + type + "' order by id desc";
            }
            DataAccess danew = new DataAccess();
            DataSet drtsk = danew.GetDataSetInline(str2);
            if (drtsk.Tables[0].Rows.Count > 0)
            {
                ret = drtsk.Tables[0].Rows[0]["assignid"].ToString();
            }
        }
        catch { ret = "0"; }
        return ret;
    }
    #region manage criticalworkflow
    public static void ManageCriticalWorkflow(string regid)
    {
        try
        {
            int companyId;
            if (!Int32.TryParse(regid, out companyId) || companyId <= 0)
            {
                SaveLogText("ManageCriticalWorkflow Invalid RegId => RegId=" + (regid ?? ""));
                return;
            }

            regid = companyId.ToString(CultureInfo.InvariantCulture);
            DataAccess da = new DataAccess();

            //SaveLogText("ManageCriticalWorkflow START => RegId=" + regid);

            // 1. Masters once for company. No uid parameter.
            SqlParameter[] prmMaster = new SqlParameter[1];
            prmMaster[0] = new SqlParameter("@regid", companyId);
            DataSet dsMaster = da.GetDataSet("usp_Pms_CriticalWorkflow_GetMasters", prmMaster);

            if (dsMaster == null || dsMaster.Tables.Count < 6)
            {
                SaveLogText("ManageCriticalWorkflow Masters Not Found => RegId=" + regid);
                return;
            }

            // 2. ALL active rule headers + conditions + actions in ONE company call.
            SqlParameter[] prmRules = new SqlParameter[1];
            prmRules[0] = new SqlParameter("@regid", companyId);
            DataSet dsRules = da.GetDataSet("usp_Pms_CriticalWorkflow_GetRules", prmRules);

            Dictionary<string, List<CriticalRuleRuntime>> ruleCache =
                BuildCriticalRuleCache(dsRules);

            // 3. ALL active entities + current workflow context in ONE company call.
            SqlParameter[] prmEntities = new SqlParameter[1];
            prmEntities[0] = new SqlParameter("@regid", companyId);
            DataSet dsEntities = da.GetDataSet("usp_Pms_CriticalWorkflow_GetAllEntities", prmEntities);

            if (dsEntities == null || dsEntities.Tables.Count == 0 || dsEntities.Tables[0].Rows.Count == 0)
            {
                SaveLogText("ManageCriticalWorkflow No Active Entity => RegId=" + regid);
                return;
            }

            int processed = 0;
            int failed = 0;

            foreach (DataRow entityRow in dsEntities.Tables[0].Rows)
            {
                CriticalWorkflowContext context = BuildCriticalWorkflowContext(entityRow);
                if (context == null)
                    continue;

                try
                {
                    ManageCriticalWorkflowEntity(context, regid, dsMaster, ruleCache);
                    processed++;
                }
                catch (Exception entityEx)
                {
                    failed++;
                    SaveLogText(
                        "ManageCriticalWorkflow Entity Error => Type=" + context.TaskType +
                        ", EntityId=" + context.EntityId +
                        ", RegId=" + regid +
                        ", Error=" + entityEx.Message
                    );
                }
            }

           
        }
        catch (Exception ex)
        {
            SaveLogText(
                "ManageCriticalWorkflow ERROR => RegId=" + (regid ?? "") +
                ", Error=" + ex.Message
            );
        }
    }

    private static void ManageCriticalWorkflowEntity(
        CriticalWorkflowContext context,
        string regid,
        DataSet dsMaster,
        Dictionary<string, List<CriticalRuleRuntime>> ruleCache)
    {
        if (context == null)
            return;

        string type = NormalizeCriticalKey(context.TaskType);
        List<CriticalRuleRuntime> rules;
        if (!ruleCache.TryGetValue(type, out rules) || rules == null || rules.Count == 0)
        {
            // Preserve current runtime behavior: if no active rule exists for the level,
            // do not automatically clear an old label.
            //SaveLogText(
            //    "ManageCriticalWorkflow No Active Rules => Type=" + context.TaskType +
            //    ", EntityId=" + context.EntityId +
            //    ", RegId=" + regid
            //);
            return;
        }

       
        bool anyCriticalLabelApplied = false;
        bool criticalEvaluationHadError = false;

        foreach (CriticalRuleRuntime rule in rules.OrderBy(x => x.RulePriority).ThenBy(x => x.RuleId))
        {
            try
            {
                CriticalWorkflowEvaluationResult evaluationResult =
                    EvaluateCriticalRuleAndExecuteActions(
                        rule.RuleId,
                        rule.RuleName,
                        rule.Conditions,
                        rule.Actions,
                        dsMaster,
                        context,
                        regid
                    );

                if (evaluationResult != null)
                {
                    if (evaluationResult.LabelApplied)
                        anyCriticalLabelApplied = true;

                    if (evaluationResult.HadError)
                        criticalEvaluationHadError = true;
                }
            }
            catch (Exception ex)
            {
                criticalEvaluationHadError = true;
                SaveLogText(
                    "ManageCriticalWorkflow Rule Error => RuleId=" + rule.RuleId +
                    ", RuleName=" + (rule.RuleName ?? "") +
                    ", Type=" + context.TaskType +
                    ", EntityId=" + context.EntityId +
                    ", RegId=" + regid +
                    ", Error=" + ex.Message
                );
            }
        }

        // Same stale-label rule as your tested runtime:
        // clear only after all active workflows evaluated successfully and none set a label.
        if (!criticalEvaluationHadError && !anyCriticalLabelApplied)
        {
            ClearCriticalEntityLabel(context, regid);
        }
        else if (criticalEvaluationHadError)
        {
            SaveLogText(
                "ManageCriticalWorkflow Label Clear Skipped => Type=" + context.TaskType +
                ", EntityId=" + context.EntityId +
                ", RegId=" + regid +
                ", Reason=Evaluation error detected"
            );
        }
    }

    private static Dictionary<string, List<CriticalRuleRuntime>> BuildCriticalRuleCache(DataSet dsRules)
    {
        Dictionary<string, List<CriticalRuleRuntime>> result =
            new Dictionary<string, List<CriticalRuleRuntime>>(StringComparer.OrdinalIgnoreCase);

        if (dsRules == null || dsRules.Tables.Count < 3)
            return result;

        DataTable headers = dsRules.Tables[0];
        DataTable allConditions = dsRules.Tables[1];
        DataTable allActions = dsRules.Tables[2];

        foreach (DataRow ruleRow in headers.Rows)
        {
            int ruleId;
            if (!Int32.TryParse(GetCriticalRowValue(ruleRow, "RuleId", "id"), out ruleId) || ruleId <= 0)
                continue;

            string levelCode = NormalizeCriticalKey(GetCriticalRowValue(ruleRow, "LevelCode"));
            if (String.IsNullOrWhiteSpace(levelCode))
                continue;

            int priority;
            if (!Int32.TryParse(GetCriticalRowValue(ruleRow, "RulePriority"), out priority))
                priority = 100;

            CriticalRuleRuntime rule = new CriticalRuleRuntime();
            rule.RuleId = ruleId;
            rule.RuleName = GetCriticalRowValue(ruleRow, "RuleName");
            rule.LevelCode = levelCode;
            rule.RulePriority = priority;
            rule.Conditions = FilterRowsByRuleId(allConditions, ruleId);
            rule.Actions = FilterRowsByRuleId(allActions, ruleId);

            List<CriticalRuleRuntime> list;
            if (!result.TryGetValue(levelCode, out list))
            {
                list = new List<CriticalRuleRuntime>();
                result[levelCode] = list;
            }

            list.Add(rule);
        }

        return result;
    }

    private static DataTable FilterRowsByRuleId(DataTable source, int ruleId)
    {
        if (source == null)
            return null;

        DataTable result = source.Clone();
        foreach (DataRow row in source.Rows)
        {
            int rowRuleId;
            if (Int32.TryParse(GetCriticalRowValue(row, "RuleId"), out rowRuleId) && rowRuleId == ruleId)
                result.ImportRow(row);
        }
        return result;
    }

    private static CriticalWorkflowContext BuildCriticalWorkflowContext(DataRow row)
    {
        if (row == null)
            return null;

        string entityType = NormalizeCriticalKey(GetCriticalRowValue(row, "EntityType"));
        string entityId = GetCriticalRowValue(row, "EntityId");
        if (String.IsNullOrWhiteSpace(entityType) || String.IsNullOrWhiteSpace(entityId))
            return null;

        CriticalWorkflowContext context = new CriticalWorkflowContext();
        context.TaskType = entityType.ToLowerInvariant();
        context.EntityId = entityId;
        context.StatusId = GetCriticalRowValue(row, "StatusId");
        context.PriorityId = GetCriticalRowValue(row, "PriorityId");
        context.CompletionPercent = GetCriticalDecimal(row, "CompletionPercent");
        context.TaskDuration = GetCriticalRowValue(row, "TaskDuration");
        context.OwnerIds = GetCriticalRowValue(row, "OwnerIds");
        context.TeamIds = GetCriticalRowValue(row, "TeamIds");
        context.ProjectManagerOwnerIds = GetCriticalRowValue(row, "ProjectManagerOwnerIds");
        context.TimeConsumedPercent = CalculateCriticalTimeConsumedPercent(row, context.TaskDuration, entityType);
        return context;
    }

    private static decimal CalculateCriticalTimeConsumedPercent(DataRow row, string taskDuration, string entityType)
    {
        if (String.Equals(entityType, "PROJECT", StringComparison.OrdinalIgnoreCase))
            return 0;

        decimal allocatedMinutes = ParseCriticalDurationMinutes(taskDuration);
        if (allocatedMinutes <= 0)
            return 0;

        decimal h = GetCriticalDecimal(row, "TimerHour");
        decimal m = GetCriticalDecimal(row, "TimerMinute");
        decimal s = GetCriticalDecimal(row, "TimerSecond");
        decimal ms = GetCriticalDecimal(row, "TimerMillisecond");

        decimal consumedMinutes = (h * 60M) + m + (s / 60M) + (ms / 60000M);
        if (consumedMinutes <= 0)
            return 0;

        return Decimal.Round((consumedMinutes / allocatedMinutes) * 100M, 2);
    }

    private static decimal ParseCriticalDurationMinutes(string duration)
    {
        if (String.IsNullOrWhiteSpace(duration))
            return 0;

        string[] parts = duration.Trim().Split(':');
        decimal hours = 0;
        decimal minutes = 0;

        if (parts.Length > 0)
            Decimal.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out hours);
        if (parts.Length > 1)
            Decimal.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out minutes);

        return (hours * 60M) + minutes;
    }


    private sealed class CriticalWorkflowContext
    {
        public string TaskType { get; set; }
        public string EntityId { get; set; }
        public string StatusId { get; set; }
        public string PriorityId { get; set; }
        public decimal CompletionPercent { get; set; }
        public decimal TimeConsumedPercent { get; set; }
        public string TaskDuration { get; set; }
        public string OwnerIds { get; set; }
        public string TeamIds { get; set; }
        public string ProjectManagerOwnerIds { get; set; }
    }

    private sealed class CriticalRuleRuntime
    {
        public int RuleId { get; set; }
        public string RuleName { get; set; }
        public string LevelCode { get; set; }
        public int RulePriority { get; set; }
        public DataTable Conditions { get; set; }
        public DataTable Actions { get; set; }
    }

    private sealed class CriticalWorkflowEvaluationResult
    {
        public bool BranchMatched { get; set; }
        public bool LabelApplied { get; set; }
        public bool HadError { get; set; }
    }

    private sealed class CriticalActionExecutionResult
    {
        public bool LabelApplied { get; set; }
        public bool HadError { get; set; }
    }

    private sealed class CriticalNotificationRecipient
    {
        public int EmployeeId { get; set; }
        public string DisplayName { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
    }

    private static decimal GetCriticalDecimal(DataRow row, params string[] columnNames)
    {
        string value = GetCriticalRowValue(row, columnNames);
        decimal result = 0;

        if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            return result;

        if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out result))
            return result;

        return 0;
    }

    private static DataRow FindCriticalMasterRow(DataTable table, string id, string alternateIdColumn)
    {
        if (table == null || string.IsNullOrWhiteSpace(id))
            return null;

        foreach (DataRow row in table.Rows)
        {
            string rowId = GetCriticalRowValue(row, "id", alternateIdColumn);
            if (string.Equals(rowId, id, StringComparison.OrdinalIgnoreCase))
                return row;
        }

        return null;
    }

    private static string GetCriticalActualValue(DataRow fieldMasterRow, CriticalWorkflowContext context)
    {
        if (fieldMasterRow == null || context == null)
            return "";

        string sourceType = NormalizeCriticalKey(GetCriticalRowValue(fieldMasterRow, "SourceType"));
        string fieldCode = NormalizeCriticalKey(GetCriticalRowValue(fieldMasterRow, "FieldCode", "FieldKey"));
        string fieldName = NormalizeCriticalKey(GetCriticalRowValue(fieldMasterRow, "FieldName"));
        string key = sourceType + "|" + fieldCode + "|" + fieldName;

        // Current CriticalSetting stores Time Consumed % with FieldKey DURATION_PERCENT.
        if (key.Contains("DURATIONPERCENT") ||
            key.Contains("TIMECONSUMED") ||
            key.Contains("CONSUMEDTIME") ||
            key.Contains("TIMESPENT") ||
            key.Contains("SPENTTIME") ||
            key.Contains("TIMEUSED"))
        {
            return context.TimeConsumedPercent.ToString(CultureInfo.InvariantCulture);
        }

        if (key.Contains("STATUS"))
            return context.StatusId ?? "";

        if (key.Contains("PRIORITY"))
            return context.PriorityId ?? "";

        if (key.Contains("PROGRESSDIFFERENCEPERCENT"))
        {
            decimal difference = context.CompletionPercent - context.TimeConsumedPercent;
            return difference.ToString(CultureInfo.InvariantCulture);
        }

        if (key.Contains("COMPLETION") ||
            key.Contains("COMPLETEPERCENT") ||
            key.Contains("PROGRESS"))
        {
            return context.CompletionPercent.ToString(CultureInfo.InvariantCulture);
        }

        return "";
    }

    private static CriticalWorkflowEvaluationResult EvaluateCriticalRuleAndExecuteActions(
        int ruleId,
        string ruleName,
        DataTable dtConditions,
        DataTable dtActions,
        DataSet dsMaster,
        CriticalWorkflowContext context,
        string regid)
    {
        CriticalWorkflowEvaluationResult evaluationResult = new CriticalWorkflowEvaluationResult();

        if (dtConditions == null || dtConditions.Rows.Count == 0)
        {
            SaveLogText(
                "CriticalWorkflow No Conditions => RuleId=" + ruleId +
                ", RuleName=" + (ruleName ?? "")
            );
            // Do not clear an existing label when the rule itself could not be evaluated.
            evaluationResult.HadError = true;
            return evaluationResult;
        }

        List<int> branchNos = new List<int>();

        foreach (DataRow row in dtConditions.Rows)
        {
            int branchNo;
            if (Int32.TryParse(GetCriticalRowValue(row, "BranchNo"), out branchNo) &&
                branchNo > 0 && !branchNos.Contains(branchNo))
            {
                branchNos.Add(branchNo);
            }
        }

        // Include an actions-only branch too, so a classic empty ELSE remains possible later.
        if (dtActions != null)
        {
            foreach (DataRow row in dtActions.Rows)
            {
                int branchNo;
                if (Int32.TryParse(GetCriticalRowValue(row, "BranchNo"), out branchNo) &&
                    branchNo > 0 && !branchNos.Contains(branchNo))
                {
                    branchNos.Add(branchNo);
                }
            }
        }

        branchNos.Sort();
        bool matchedAnyBranch = false;

        foreach (int branchNo in branchNos)
        {
            List<DataRow> branchConditions = dtConditions.AsEnumerable()
                .Where(delegate (DataRow r)
                {
                    int n;
                    return Int32.TryParse(GetCriticalRowValue(r, "BranchNo"), out n) && n == branchNo;
                })
                .OrderBy(delegate (DataRow r)
                {
                    int n;
                    return Int32.TryParse(GetCriticalRowValue(r, "SequenceNo"), out n) ? n : Int32.MaxValue;
                })
                .ToList();

            string branchType = "";
            if (branchConditions.Count > 0)
                branchType = GetCriticalRowValue(branchConditions[0], "BranchType");

            if (String.IsNullOrWhiteSpace(branchType) && dtActions != null)
            {
                DataRow actionBranchRow = dtActions.AsEnumerable().FirstOrDefault(delegate (DataRow r)
                {
                    int n;
                    return Int32.TryParse(GetCriticalRowValue(r, "BranchNo"), out n) && n == branchNo;
                });
                branchType = GetCriticalRowValue(actionBranchRow, "BranchType");
            }

            branchType = (branchType ?? "").Trim().ToUpperInvariant();

            string expressionText;
            string branchError;
            bool branchResult;

            // Current UI saves conditions for ELSE too. This fallback also supports
            // a classic empty ELSE safely if one exists in old data.
            if (branchConditions.Count == 0)
            {
                branchResult = branchType == "ELSE";
                expressionText = branchResult ? "TRUE (unconditional ELSE)" : "";
                branchError = branchResult ? "" : "No conditions found.";
            }
            else
            {
                branchResult = EvaluateCriticalBranch(
                    ruleId,
                    branchNo,
                    branchType,
                    branchConditions,
                    dsMaster,
                    context,
                    out expressionText,
                    out branchError
                );
            }

           

            if (!String.IsNullOrWhiteSpace(branchError))
                evaluationResult.HadError = true;

            if (!branchResult)
                continue;

            matchedAnyBranch = true;
            evaluationResult.BranchMatched = true;

            

            // Execute every action of the matched branch in SequenceNo order.
            // Each action is isolated so a new/unsupported action cannot break existing status/timer working.
            if (dtActions != null)
            {
                List<DataRow> matchedActions = dtActions.AsEnumerable()
                    .Where(delegate (DataRow r)
                    {
                        int n;
                        return Int32.TryParse(GetCriticalRowValue(r, "BranchNo"), out n) && n == branchNo;
                    })
                    .OrderBy(delegate (DataRow r)
                    {
                        int n;
                        return Int32.TryParse(GetCriticalRowValue(r, "SequenceNo"), out n) ? n : Int32.MaxValue;
                    })
                    .ToList();

                foreach (DataRow actionRow in matchedActions)
                {
                    

                    try
                    {
                        CriticalActionExecutionResult actionResult = ExecuteCriticalMatchedAction(
                            ruleId,
                            branchNo,
                            actionRow,
                            dsMaster,
                            context,
                            regid
                        );

                        if (actionResult != null)
                        {
                            if (actionResult.LabelApplied)
                                evaluationResult.LabelApplied = true;

                            if (actionResult.HadError)
                                evaluationResult.HadError = true;
                        }
                    }
                    catch (Exception actionEx)
                    {
                        evaluationResult.HadError = true;
                        SaveLogText(
                            "CriticalWorkflow Action Execute Error => " +
                            "RuleId=" + ruleId +
                            ", BranchNo=" + branchNo +
                            ", ActionId=" + GetCriticalRowValue(actionRow, "ActionId") +
                            ", Error=" + actionEx.Message
                        );
                    }
                }
            }

            // FIRST matching IF / ELSE IF / ELSE branch wins inside this workflow.
            // The outer workflow loop continues to the next active workflow.
            break;
        }

        if (!matchedAnyBranch)
        {
            //SaveLogText(
            //    "CriticalWorkflow No Matched Branch => RuleId=" + ruleId +
            //    ", RuleName=" + (ruleName ?? "")
            //);
        }

        return evaluationResult;
    }

    private static CriticalActionExecutionResult ExecuteCriticalMatchedAction(
        int ruleId,
        int branchNo,
        DataRow actionRow,
        DataSet dsMaster,
        CriticalWorkflowContext context,
        string regid)
    {
        CriticalActionExecutionResult executionResult = new CriticalActionExecutionResult();

        if (actionRow == null || context == null)
        {
            executionResult.HadError = true;
            return executionResult;
        }

        string actionId = GetCriticalRowValue(actionRow, "ActionId");
        string actionValue = GetCriticalRowValue(actionRow, "ActionValue");
        string sequenceNo = GetCriticalRowValue(actionRow, "SequenceNo");

        DataRow actionMasterRow = null;
        if (dsMaster != null && dsMaster.Tables.Count > 5)
            actionMasterRow = FindCriticalMasterRow(dsMaster.Tables[5], actionId, "ActionId");

        if (actionMasterRow == null)
        {
            SaveLogText(
                "CriticalWorkflow Action Master Not Found => " +
                "RuleId=" + ruleId +
                ", BranchNo=" + branchNo +
                ", ActionId=" + actionId
            );
            executionResult.HadError = true;
            return executionResult;
        }

        string actionCodeRaw = GetCriticalRowValue(actionMasterRow, "ActionCode", "ActionKey", "Code");
        string actionName = GetCriticalRowValue(actionMasterRow, "ActionName", "Name");
        string actionCode = NormalizeCriticalKey(actionCodeRaw);

        

        if (actionCode == "SETCRITICALLABEL")
        {
            executionResult.LabelApplied = UpdateCriticalEntityLabel(
                ruleId,
                branchNo,
                context,
                actionValue,
                dsMaster,
                regid
            );

            if (!executionResult.LabelApplied)
                executionResult.HadError = true;

            return executionResult;
        }

        // ===== Critical Workflow Notification Verify Step =====
        // Safe extension after Step 7: only resolve recipients + configured channels and LOG them.
        // No Email / WhatsApp / Bell is actually sent in this step, so existing working is untouched.
        if (actionCode == "SENDNOTIFICATION")
        {
            VerifyCriticalNotificationAction(
                ruleId,
                branchNo,
                actionRow,
                context,
                regid
            );
            return executionResult;
        }

        

        return executionResult;
    }

    private static void VerifyCriticalNotificationAction(
        int ruleId,
        int branchNo,
        DataRow actionRow,
        CriticalWorkflowContext context,
        string regid)
    {
        if (actionRow == null || context == null)
            return;

        string recipientTypeRaw = GetCriticalRowValue(actionRow, "RecipientType");
        string recipientType = NormalizeCriticalKey(recipientTypeRaw);
        string recipientValue = GetCriticalRowValue(actionRow, "RecipientValue");
        string notificationText = GetCriticalRowValue(actionRow, "ActionValue");

        bool isEmail = GetCriticalBoolean(actionRow, "IsEmail");
        bool isWhatsApp = GetCriticalBoolean(actionRow, "IsWhatsApp");
        bool isBell = GetCriticalBoolean(actionRow, "IsBell");

        

        // Setting validation already requires at least one channel. Runtime still protects old rows.
        if (!isEmail && !isWhatsApp && !isBell)
        {
            SaveLogText(
                "CriticalWorkflow Notification Skip => " +
                "RuleId=" + ruleId +
                ", BranchNo=" + branchNo +
                ", Reason=No notification channel selected"
            );
            return;
        }

        List<int> configuredRecipients = ResolveCriticalNotificationRecipients(
            context,
            recipientType,
            recipientValue,
            regid
        );

        List<CriticalNotificationRecipient> activeRecipients =
            LoadCriticalActiveNotificationRecipients(configuredRecipients, regid);

        string recipientIds = String.Join(",", activeRecipients.Select(x => x.EmployeeId.ToString()).ToArray());
        string recipientNames = String.Join("|", activeRecipients.Select(x => x.DisplayName ?? "").ToArray());
        string recipientEmails = String.Join("|", activeRecipients
            .Where(x => !String.IsNullOrWhiteSpace(x.EmailId))
            .Select(x => x.EmailId)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray());
        string recipientMobiles = String.Join("|", activeRecipients
            .Where(x => !String.IsNullOrWhiteSpace(x.MobileNo))
            .Select(x => x.MobileNo)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray());

        int emailRecipientCount = activeRecipients.Count(x => !String.IsNullOrWhiteSpace(x.EmailId));
        int mobileRecipientCount = activeRecipients.Count(x => !String.IsNullOrWhiteSpace(x.MobileNo));

       
    }

    private static string GetCriticalFirstNonEmptyRowValue(DataRow row, params string[] columnNames)
    {
        if (row == null || row.Table == null || columnNames == null)
            return "";

        foreach (string columnName in columnNames)
        {
            if (String.IsNullOrWhiteSpace(columnName))
                continue;

            foreach (DataColumn col in row.Table.Columns)
            {
                if (!String.Equals(col.ColumnName, columnName, StringComparison.OrdinalIgnoreCase))
                    continue;

                if (row[col] == null || row[col] == DBNull.Value)
                    break;

                string value = row[col].ToString();
                if (!String.IsNullOrWhiteSpace(value))
                    return value.Trim();

                break;
            }
        }

        return "";
    }

    private static bool GetCriticalBoolean(DataRow row, params string[] columnNames)
    {
        string value = GetCriticalRowValue(row, columnNames);
        if (String.IsNullOrWhiteSpace(value))
            return false;

        value = value.Trim();

        bool boolValue;
        if (Boolean.TryParse(value, out boolValue))
            return boolValue;

        int intValue;
        if (Int32.TryParse(value, out intValue))
            return intValue == 1;

        return String.Equals(value, "yes", StringComparison.OrdinalIgnoreCase) ||
               String.Equals(value, "y", StringComparison.OrdinalIgnoreCase);
    }

    private static List<int> ResolveCriticalNotificationRecipients(
        CriticalWorkflowContext context,
        string recipientType,
        string recipientValue,
        string regid)
    {
        List<int> recipients = new List<int>();
        if (context == null)
            return recipients;

        recipientType = NormalizeCriticalKey(recipientType);

        if (recipientType == "CUSTOM")
            return ParseCriticalEmployeeIds(recipientValue);

        if (recipientType == "OWNER")
            return ParseCriticalEmployeeIds(context.OwnerIds);

        if (recipientType == "TEAM")
            return ParseCriticalEmployeeIds(context.TeamIds);

        if (recipientType == "PROJECTMANAGER")
            return ParseCriticalEmployeeIds(context.ProjectManagerOwnerIds);

        return recipients;
    }

    private static List<int> ParseCriticalEmployeeIds(string ids)
    {
        List<int> result = new List<int>();
        if (String.IsNullOrWhiteSpace(ids))
            return result;

        string[] parts = ids.Split(new char[] { ',', ';', '|' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string part in parts)
        {
            int employeeId;
            if (Int32.TryParse((part ?? "").Trim(), out employeeId) && employeeId > 0 && !result.Contains(employeeId))
                result.Add(employeeId);
        }

        return result;
    }

    private static List<CriticalNotificationRecipient> LoadCriticalActiveNotificationRecipients(
        List<int> employeeIds,
        string regid)
    {
        List<CriticalNotificationRecipient> result = new List<CriticalNotificationRecipient>();
        if (employeeIds == null || employeeIds.Count == 0)
            return result;

        List<int> safeIds = employeeIds.Where(x => x > 0).Distinct().ToList();
        if (safeIds.Count == 0)
            return result;

        int companyId;
        if (!Int32.TryParse(regid, out companyId) || companyId <= 0)
            return result;

        StringBuilder xml = new StringBuilder();
        xml.Append("<Ids>");
        foreach (int employeeId in safeIds)
            xml.Append("<Id Value=\"").Append(employeeId.ToString(CultureInfo.InvariantCulture)).Append("\" />");
        xml.Append("</Ids>");

        SqlParameter[] prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@regid", companyId);
        prm[1] = new SqlParameter("@EmployeeIdsXml", xml.ToString());

        DataAccess da = new DataAccess();
        DataSet ds = da.GetDataSet("usp_Pms_CriticalWorkflow_GetEmployees", prm);
        if (ds == null || ds.Tables.Count == 0)
            return result;

        foreach (DataRow row in ds.Tables[0].Rows)
        {
            int employeeId;
            if (!Int32.TryParse(GetCriticalRowValue(row, "id"), out employeeId))
                continue;

            result.Add(new CriticalNotificationRecipient
            {
                EmployeeId = employeeId,
                DisplayName = GetCriticalRowValue(row, "DisplayName"),
                EmailId = GetCriticalFirstNonEmptyRowValue(row, "email_Id", "Email", "EmailId", "EmailID"),
                MobileNo = GetCriticalFirstNonEmptyRowValue(row, "mobile_no", "MobileNumber", "MobileNo", "Mobile")
            });
        }

        return result;
    }

    private static bool UpdateCriticalEntityLabel(
        int ruleId,
        int branchNo,
        CriticalWorkflowContext context,
        string actionValue,
        DataSet dsMaster,
        string regid)
    {
        int entityId;
        int criticalLabelId;
        int companyId;

        if (context == null ||
            !Int32.TryParse(context.EntityId, out entityId) || entityId <= 0 ||
            !Int32.TryParse(actionValue, out criticalLabelId) || criticalLabelId <= 0 ||
            !Int32.TryParse(regid, out companyId) || companyId <= 0)
        {
            SaveLogText(
                "CriticalWorkflow Set Label Error => RuleId=" + ruleId +
                ", BranchNo=" + branchNo +
                ", EntityId=" + (context == null ? "" : context.EntityId) +
                ", CriticalLabelId=" + (actionValue ?? "")
            );
            return false;
        }

        DataRow labelMasterRow = null;
        if (dsMaster != null && dsMaster.Tables.Count > 0)
            labelMasterRow = FindCriticalMasterRow(dsMaster.Tables[0], criticalLabelId.ToString(CultureInfo.InvariantCulture), "CriticalLabelId");

        if (labelMasterRow == null)
        {
            SaveLogText(
                "CriticalWorkflow Set Label Error => RuleId=" + ruleId +
                ", BranchNo=" + branchNo +
                ", CriticalLabelId=" + criticalLabelId +
                ", Error=Critical label not found in active company master"
            );
            return false;
        }

        SqlParameter[] prm = new SqlParameter[4];
        prm[0] = new SqlParameter("@regid", companyId);
        prm[1] = new SqlParameter("@EntityType", context.TaskType);
        prm[2] = new SqlParameter("@EntityId", entityId);
        prm[3] = new SqlParameter("@CriticalLabelId", criticalLabelId);

        DataAccess da = new DataAccess();
        DataSet ds = da.GetDataSet("usp_Pms_CriticalWorkflow_SetLabel", prm);

        bool saved = false;
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            saved = GetCriticalBoolean(ds.Tables[0].Rows[0], "Saved", "Result");

       
        return saved;
    }

    private static bool ClearCriticalEntityLabel(CriticalWorkflowContext context, string regid)
    {
        try
        {
            int entityId;
            int companyId;
            if (context == null ||
                !Int32.TryParse(context.EntityId, out entityId) || entityId <= 0 ||
                !Int32.TryParse(regid, out companyId) || companyId <= 0)
                return false;

            SqlParameter[] prm = new SqlParameter[3];
            prm[0] = new SqlParameter("@regid", companyId);
            prm[1] = new SqlParameter("@EntityType", context.TaskType);
            prm[2] = new SqlParameter("@EntityId", entityId);

            DataAccess da = new DataAccess();
            DataSet ds = da.GetDataSet("usp_Pms_CriticalWorkflow_ClearLabel", prm);

            bool cleared = false;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                cleared = GetCriticalBoolean(ds.Tables[0].Rows[0], "Cleared", "Result");

            
            return cleared;
        }
        catch (Exception ex)
        {
            SaveLogText(
                "CriticalWorkflow Label Clear Error => Type=" +
                (context == null ? "" : context.TaskType) +
                ", EntityId=" + (context == null ? "" : context.EntityId) +
                ", Error=" + ex.Message
            );
            return false;
        }
    }

    private static bool EvaluateCriticalBranch(
        int ruleId,
        int branchNo,
        string branchType,
        List<DataRow> conditions,
        DataSet dsMaster,
        CriticalWorkflowContext context,
        out string expressionText,
        out string error)
    {
        expressionText = "";
        error = "";
        List<string> tokens = new List<string>();

        if (conditions == null || conditions.Count == 0)
        {
            error = "No conditions found.";
            return false;
        }

        for (int i = 0; i < conditions.Count; i++)
        {
            DataRow conditionRow = conditions[i];
            string source = (GetCriticalRowValue(conditionRow, "ConditionSource") ?? "SELF").Trim().ToUpperInvariant();
            string fieldId = GetCriticalRowValue(conditionRow, "FieldId");
            string operatorId = GetCriticalRowValue(conditionRow, "OperatorId");
            string compareValue = GetCriticalRowValue(conditionRow, "CompareValue");
            string compareValue2 = GetCriticalRowValue(conditionRow, "CompareValue2");
            string sequenceNo = GetCriticalRowValue(conditionRow, "SequenceNo");

            DataRow fieldMasterRow = null;
            if (dsMaster != null && dsMaster.Tables.Count > 2)
                fieldMasterRow = FindCriticalMasterRow(dsMaster.Tables[2], fieldId, "FieldId");

            DataRow operatorMasterRow = null;
            if (dsMaster != null && dsMaster.Tables.Count > 3)
                operatorMasterRow = FindCriticalMasterRow(dsMaster.Tables[3], operatorId, "OperatorId");

            string fieldName = GetCriticalRowValue(fieldMasterRow, "FieldName");
            string fieldCode = GetCriticalRowValue(fieldMasterRow, "FieldCode", "FieldKey");
            string dataType = GetCriticalRowValue(fieldMasterRow, "DataType");
            string operatorName = GetCriticalRowValue(operatorMasterRow, "OperatorName");
            string operatorSymbol = ResolveCriticalOperatorSymbol(operatorMasterRow);
            string actualValue = "";
            bool conditionResult = false;
            string conditionError = "";

            if (source != "SELF")
            {
                conditionError = "ConditionSource " + source + " is not evaluated in Step 6.";
                conditionResult = false;
            }
            else if (fieldMasterRow == null)
            {
                conditionError = "Field master not found for FieldId=" + fieldId;
                conditionResult = false;
            }
            else if (operatorMasterRow == null)
            {
                conditionError = "Operator master not found for OperatorId=" + operatorId;
                conditionResult = false;
            }
            else
            {
                actualValue = GetCriticalActualValue(fieldMasterRow, context);
                conditionResult = EvaluateCriticalComparison(
                    actualValue,
                    compareValue,
                    compareValue2,
                    dataType,
                    operatorSymbol,
                    out conditionError
                );
            }

            

            AddCriticalBracketTokens(tokens, GetCriticalRowValue(conditionRow, "OpenBracket"), true);
            tokens.Add(conditionResult ? "TRUE" : "FALSE");
            AddCriticalBracketTokens(tokens, GetCriticalRowValue(conditionRow, "CloseBracket"), false);

            if (i < conditions.Count - 1)
            {
                string logicalOperatorId = GetCriticalRowValue(conditionRow, "LogicalOperatorId");
                string logicalOperator = ResolveCriticalLogicalOperator(dsMaster, logicalOperatorId);

                if (String.IsNullOrWhiteSpace(logicalOperator))
                {
                    error = "Logical operator not found after SequenceNo=" + sequenceNo +
                            " (LogicalOperatorId=" + logicalOperatorId + ").";
                    expressionText = String.Join(" ", tokens.ToArray());
                    return false;
                }

                tokens.Add(logicalOperator);
            }
        }

        expressionText = String.Join(" ", tokens.ToArray());

        bool result;
        string expressionError;
        if (!EvaluateCriticalBooleanExpression(tokens, out result, out expressionError))
        {
            error = expressionError;
            return false;
        }

        return result;
    }

    private static bool EvaluateCriticalComparison(
        string actualValue,
        string compareValue,
        string compareValue2,
        string dataType,
        string operatorSymbol,
        out string error)
    {
        error = "";
        string op = (operatorSymbol ?? "").Trim().ToUpperInvariant();
        if (op == "==") op = "=";
        if (op == "<>") op = "!=";

        if (String.IsNullOrWhiteSpace(op))
        {
            error = "Operator symbol is blank.";
            return false;
        }

        if (op == "IN" || op == "NOT IN")
        {
            string[] values = (compareValue ?? "").Split(',');
            bool found = values.Any(delegate (string x)
            {
                return String.Equals(
                    (x ?? "").Trim(),
                    (actualValue ?? "").Trim(),
                    StringComparison.OrdinalIgnoreCase
                );
            });
            return op == "IN" ? found : !found;
        }

        string normalizedDataType = NormalizeCriticalKey(dataType);
        bool numericOperator = op == ">" || op == "<" || op == ">=" || op == "<=" || op == "BETWEEN";
        bool shouldCompareAsNumber = normalizedDataType == "NUMBER" || numericOperator;

        if (shouldCompareAsNumber)
        {
            decimal actualNumber;
            decimal expectedNumber;

            if (!TryParseCriticalDecimal(actualValue, out actualNumber))
            {
                error = "Actual value is not numeric: " + (actualValue ?? "");
                return false;
            }

            if (!TryParseCriticalDecimal(compareValue, out expectedNumber))
            {
                error = "Compare value is not numeric: " + (compareValue ?? "");
                return false;
            }

            if (op == "=") return actualNumber == expectedNumber;
            if (op == "!=") return actualNumber != expectedNumber;
            if (op == ">") return actualNumber > expectedNumber;
            if (op == "<") return actualNumber < expectedNumber;
            if (op == ">=") return actualNumber >= expectedNumber;
            if (op == "<=") return actualNumber <= expectedNumber;

            if (op == "BETWEEN")
            {
                decimal secondNumber;
                if (!TryParseCriticalDecimal(compareValue2, out secondNumber))
                {
                    error = "Second compare value is not numeric: " + (compareValue2 ?? "");
                    return false;
                }

                // BETWEEN is inclusive: >= first value AND <= second value.
                return actualNumber >= expectedNumber && actualNumber <= secondNumber;
            }

            error = "Unsupported numeric operator: " + op;
            return false;
        }

        int stringCompare = String.Compare(
            (actualValue ?? "").Trim(),
            (compareValue ?? "").Trim(),
            StringComparison.OrdinalIgnoreCase
        );

        if (op == "=") return stringCompare == 0;
        if (op == "!=") return stringCompare != 0;

        error = "Unsupported operator for DataType=" + dataType + ": " + op;
        return false;
    }

    private static bool TryParseCriticalDecimal(string value, out decimal result)
    {
        if (Decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            return true;

        if (Decimal.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out result))
            return true;

        result = 0;
        return false;
    }

    private static string ResolveCriticalOperatorSymbol(DataRow operatorMasterRow)
    {
        string symbol = GetCriticalRowValue(operatorMasterRow, "OperatorSymbol", "OperatorCode");
        if (!String.IsNullOrWhiteSpace(symbol))
            return symbol.Trim().ToUpperInvariant();

        string name = NormalizeCriticalKey(GetCriticalRowValue(operatorMasterRow, "OperatorName"));
        if (name == "EQUALTO" || name == "EQUAL") return "=";
        if (name == "NOTEQUALTO" || name == "NOTEQUAL") return "!=";
        if (name == "GREATERTHAN") return ">";
        if (name == "LESSTHAN") return "<";
        if (name == "GREATERTHANOREQUAL" || name == "GREATERTHANOREQUALTO") return ">=";
        if (name == "LESSTHANOREQUAL" || name == "LESSTHANOREQUALTO") return "<=";
        if (name == "BETWEEN") return "BETWEEN";
        if (name == "IN") return "IN";
        if (name == "NOTIN") return "NOT IN";
        return "";
    }

    private static string ResolveCriticalLogicalOperator(DataSet dsMaster, string logicalOperatorId)
    {
        if (dsMaster == null || dsMaster.Tables.Count <= 4 || String.IsNullOrWhiteSpace(logicalOperatorId))
            return "";

        DataRow logicalRow = FindCriticalMasterRow(dsMaster.Tables[4], logicalOperatorId, "LogicalOperatorId");
        if (logicalRow == null)
            return "";

        string value = GetCriticalRowValue(
            logicalRow,
            "OperatorName",
            "LogicalOperator",
            "LogicalOperatorName",
            "OperatorCode",
            "OperatorSymbol"
        );

        string key = NormalizeCriticalKey(value);
        if (key.Contains("AND")) return "AND";
        if (key.Contains("OR")) return "OR";
        return "";
    }

    private static void AddCriticalBracketTokens(List<string> tokens, string bracketValue, bool opening)
    {
        if (tokens == null || String.IsNullOrWhiteSpace(bracketValue))
            return;

        char wanted = opening ? '(' : ')';
        foreach (char ch in bracketValue)
        {
            if (ch == wanted)
                tokens.Add(wanted.ToString());
        }
    }

    private static bool EvaluateCriticalBooleanExpression(
        List<string> tokens,
        out bool result,
        out string error)
    {
        result = false;
        error = "";

        if (tokens == null || tokens.Count == 0)
        {
            error = "Boolean expression is empty.";
            return false;
        }

        List<string> postfix = new List<string>();
        Stack<string> operators = new Stack<string>();

        foreach (string rawToken in tokens)
        {
            string token = (rawToken ?? "").Trim().ToUpperInvariant();

            if (token == "TRUE" || token == "FALSE")
            {
                postfix.Add(token);
            }
            else if (token == "AND" || token == "OR")
            {
                while (operators.Count > 0 &&
                       operators.Peek() != "(" &&
                       GetCriticalBooleanPrecedence(operators.Peek()) >= GetCriticalBooleanPrecedence(token))
                {
                    postfix.Add(operators.Pop());
                }
                operators.Push(token);
            }
            else if (token == "(")
            {
                operators.Push(token);
            }
            else if (token == ")")
            {
                bool foundOpen = false;
                while (operators.Count > 0)
                {
                    string op = operators.Pop();
                    if (op == "(")
                    {
                        foundOpen = true;
                        break;
                    }
                    postfix.Add(op);
                }

                if (!foundOpen)
                {
                    error = "Closing bracket has no matching opening bracket.";
                    return false;
                }
            }
            else
            {
                error = "Invalid boolean token: " + token;
                return false;
            }
        }

        while (operators.Count > 0)
        {
            string op = operators.Pop();
            if (op == "(" || op == ")")
            {
                error = "Opening bracket has no matching closing bracket.";
                return false;
            }
            postfix.Add(op);
        }

        Stack<bool> values = new Stack<bool>();
        foreach (string token in postfix)
        {
            if (token == "TRUE")
            {
                values.Push(true);
            }
            else if (token == "FALSE")
            {
                values.Push(false);
            }
            else if (token == "AND" || token == "OR")
            {
                if (values.Count < 2)
                {
                    error = "Invalid AND/OR expression.";
                    return false;
                }

                bool right = values.Pop();
                bool left = values.Pop();
                values.Push(token == "AND" ? (left && right) : (left || right));
            }
        }

        if (values.Count != 1)
        {
            error = "Boolean expression did not reduce to one result.";
            return false;
        }

        result = values.Pop();
        return true;
    }

    private static int GetCriticalBooleanPrecedence(string logicalOperator)
    {
        if (String.Equals(logicalOperator, "AND", StringComparison.OrdinalIgnoreCase)) return 2;
        if (String.Equals(logicalOperator, "OR", StringComparison.OrdinalIgnoreCase)) return 1;
        return 0;
    }

    private static string NormalizeCriticalKey(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return "";

        char[] chars = value
            .ToUpperInvariant()
            .Where(delegate (char ch) { return Char.IsLetterOrDigit(ch); })
            .ToArray();

        return new string(chars);
    }

    private static string GetCriticalLevelCode(string tasktype)
    {
        string type = (tasktype ?? "").Trim().Replace(" ", "").ToUpperInvariant();

        if (type == "PROJECT") return "PROJECT";
        if (type == "MILESTONE") return "MILESTONE";
        if (type == "TASK") return "TASK";

        // Current end-user screen normally sends subtask.
        return "SUBTASK";
    }

    private static string GetCriticalRowValue(DataRow row, params string[] columnNames)
    {
        if (row == null || row.Table == null || columnNames == null)
            return "";

        foreach (string columnName in columnNames)
        {
            if (string.IsNullOrWhiteSpace(columnName))
                continue;

            foreach (DataColumn col in row.Table.Columns)
            {
                if (string.Equals(col.ColumnName, columnName, StringComparison.OrdinalIgnoreCase))
                {
                    if (row[col] == null || row[col] == DBNull.Value)
                        return "";

                    return row[col].ToString();
                }
            }
        }

        return "";
    }

    private static bool IsCriticalRuleActive(DataRow row)
    {
        // status must be 1 / true when the column is returned by the list SP.
        string statusValue = GetCriticalRowValue(row, "status", "Status");
        if (!string.IsNullOrWhiteSpace(statusValue))
        {
            bool statusBool;
            int statusInt;

            if (bool.TryParse(statusValue, out statusBool))
            {
                if (!statusBool) return false;
            }
            else if (int.TryParse(statusValue, out statusInt))
            {
                if (statusInt != 1) return false;
            }
            else
            {
                return false;
            }
        }

        // archive must be 0 / false when the column is returned by the list SP.
        // If archive is not returned, the existing list SP remains the source of the non-archived list.
        string archiveValue = GetCriticalRowValue(row, "archive", "Archive");
        if (!string.IsNullOrWhiteSpace(archiveValue))
        {
            bool archiveBool;
            int archiveInt;

            if (bool.TryParse(archiveValue, out archiveBool))
            {
                if (archiveBool) return false;
            }
            else if (int.TryParse(archiveValue, out archiveInt))
            {
                if (archiveInt != 0) return false;
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    #endregion



}
public class unassignsubtasklist
{
    public int? id { get; set; }
    public int? assignid { get; set; }
    public DateTime? assigndatetime { get; set; }
    public string tasktype { get; set; }
    // True when an active row exists in Pms_tbl_assigntasks. This state is
    // independent of the currently selected scheduler date.
    public bool isassigned { get; set; }
    public int? ProjectId { get; set; }    
    public string ProjectName { get; set; }
    public string Powner { get; set; }
    public int? Milestoneid { get; set; }
    public string Milestone { get; set; }
    public int? Task { get; set; }
    public string Taskname { get; set; }
    public string Subtaskname { get; set; }
    public string SubTaskDescription { get; set; }
    public string owner { get; set; }
    public string team { get; set; }
    //public DateTime? Startdate { get; set; }
    //public DateTime? Targetdate { get; set; }
    public string Startdate { get; set; }
    public string Targetdate { get; set; }
    public double? Weightage { get; set; }
    public string StartTime { get; set; }
    public string Timeline { get; set; }
    public string TaskDuration { get; set; }
    public string TaskDuration_min { get; set; }
    public int? P_Status { get; set; }
    public string status_name { get; set; }
    public string status_color { get; set; }
    public int? Priority { get; set; }
    public string priority_name { get; set; }
    public string priority_color { get; set; }
    public double? Budget { get; set; }
    public double? Actual { get; set; }
    public string Reminder { get; set; }
    public int? ReminderType { get; set; }
    public string Days { get; set; }
    public string Time { get; set; }
    public int? uid { get; set; }
    public DateTime? cdate { get; set; }
    public string tagto { get; set; }
    public string dependancy { get; set; }
    public string iscomplete { get; set; }
    public double? iscompletepercentage { get; set; }
    public int? orderno { get; set; }
    public string txtdays { get; set; }
    public string starttime { get; set; }
    //public List<taskSegment> taskSegments { get; set; }

}
public class assignsubtasklist
{
    public int? id { get; set; }
    public int? assignid { get; set; }
    public DateTime? assigndatetime { get; set; }
    public string tasktype { get; set; }
    public int? ProjectId { get; set; }
    public string ProjectName { get; set; }
    public string Powner { get; set; }
    public int? Milestoneid { get; set; }
    public string Milestone { get; set; }
    public int? Task { get; set; }
    public string Taskname { get; set; }
    public string Subtaskname { get; set; }
    public string SubTaskDescription { get; set; }
    public string owner { get; set; }
    public string team { get; set; }
    public DateTime? Startdate { get; set; }
    public DateTime? Targetdate { get; set; }
    // Stable date-only keys for JavaScript. Keep the existing DateTime properties
    // so no old binding/code is broken.
    public string StartDateKey { get; set; }
    public string TargetDateKey { get; set; }
    public string AssignmentDateTimeKey { get; set; }
    public double? Weightage { get; set; }
    public string StartTime { get; set; }
    public string Timeline { get; set; }
    public string TaskDuration { get; set; }
    public string TaskDuration_min { get; set; }
    public int? P_Status { get; set; }
    public string status_name { get; set; }
    public string status_color { get; set; }
    public int? Priority { get; set; }
    public string priority_name { get; set; }
    public string priority_color { get; set; }
    public double? Budget { get; set; }
    public double? Actual { get; set; }
    public string Reminder { get; set; }
    public int? ReminderType { get; set; }
    public string Days { get; set; }
    public string Time { get; set; }
    public int? uid { get; set; }
    public DateTime? cdate { get; set; }
    public string tagto { get; set; }
    public string dependancy { get; set; }
    public string iscomplete { get; set; }
    public double? iscompletepercentage { get; set; }
    public int? orderno { get; set; }
    public string txtdays { get; set; }
    public string starttime { get; set; }
    //public List<taskSegment> taskSegments { get; set; }

}
public class item_summaryN
{
    public string status{ get; set; }
    public double? copmelete_value { get; set; }
    public double? pending_value { get; set; }
   
}
public class project_details
{
    public int? id { get; set; }
    public string ProjectID { get; set; }
    public string ProjectName { get; set; }
    public string Owner { get; set; }
    public string team { get; set; }
    public int? ProjectStatus { get; set; }
    //public DateTime? AwardDate { get; set; }
    //public DateTime? StartDate { get; set; }
    //public DateTime? TargetDate { get; set; }
    public string AwardDate { get; set; }
    public string StartDate { get; set; }
    public string TargetDate { get; set; }
    public int? ApprovalDrawing { get; set; }
    public int? salesorderid { get; set; }
    public int? customerid { get; set; }
    public int? checkprojectWeightage { get; set; }
    public int? hastask { get; set; }
    public int? checktaskWeightage { get; set; }
    public int? hassubtask { get; set; }
    public int? checksubtaskWeightage { get; set; }
    public string editcolor { get; set; }
    public List<ownerlist> owners { get; set; }
    public List<teamlist> teams { get; set; }
    public List<newmilestone> miles { get; set; }
    public bool isadmin { get; set; }




}
public class milestone_details
{
    public int? id { get; set; }
    public int?ProjectId { get; set; }
    public string Milestone { get; set; }
    public double? Weightage { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string Owner { get; set; }
    public string assignto { get; set; }
    public string team { get; set; }
    public int? P_Status { get; set; }
    public int? Priority { get; set; }
    public int? istask { get; set; }
    public List<newtask> tasks { get; set; }



}
public class newmilestone
{
    public int? id { get; set; }
    public int? ProjectId { get; set; }
    public string Milestone { get; set; }
    public double? Weightage { get; set; }
    //public DateTime? StartDate { get; set; }
    //public DateTime? EndDate { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string Owner { get; set; }
    public string assignto { get; set; }
    public int? Access { get; set; }
    public string team { get; set; }
    public int? P_Status { get; set; }
    public string status_name { get; set; }
    public string status_color { get; set; }
    public int? Priority {get; set;}
    public string priority_name { get; set; }
    public string priority_color { get; set; }
    public double? Budget { get; set; }
    public double? Actual { get; set; }
    public int? uid { get; set; }
    public DateTime? cdate { get; set; }
    public int? statuscomplete { get; set; }
    public int? istask { get; set; }
    public int? checkmileWeightage { get; set; }
    public int? iscomplete { get; set; }
    //public DateTime? iscompletedate { get; set; }
    public string iscompletedate { get; set; }
    public double? iscompletepercentage { get; set; }
    public int? Durationtype { get; set; }
    public string StartTime { get; set; }
    public string TaskDuration { get; set; }
    public string TaskDuration_min { get; set; }
    public double? Durationhourcal { get; set; }
    public int? orderno { get; set; }
    public string miledescription { get; set; }
    public string Uploaddrawing { get; set; }
    public string txtdays { get; set; }
    public string complete_value { get; set; }
    public string pending_value { get; set; }
    public int? taskcount { get; set; }
    public bool isExpanded { get; set; }    
    public string leftdays { get; set; }
    public int? totalissue { get; set; }
    public int? pendingissue { get; set; }
    public int? completeissue { get; set; }
    public double? pendingissue_rate { get; set; }
    public double? completeissue_rate { get; set; }

    public int? totaldependency { get; set; }
    public int? isreassign { get; set; }
    
    public List<ownerlist> owners { get; set; }
    public List<teamlist> teams { get; set; }
    public List<projectfiles>viewfiles { get; set; }
    public List<newtask> tasks { get; set; }
    //public List<issuestdata> mileissues { get; set; }
    public string CriticalLabelId { get; set; }
    public string CriticalLabel { get; set; }
    public string CriticalLabelcolor { get; set; }



}
public class newtask
{
    public int? id { get; set; }
    public int? ProjectId { get; set; }
    public int? Milestoneid { get; set; }
    public string taskname { get; set; }
    public string taskdescripton { get; set; }
    public string owner { get; set; }
    public string assignto { get; set; }
    public string team { get; set; }
    //public DateTime? Startdate { get; set; }
    //public DateTime? Targetdate { get; set; }
    public string Startdate { get; set; }
    public string Targetdate { get; set; }
    public double? Weightage { get; set; }
    public string StartTime { get; set; }
    public string Timeline { get; set; }
    public string TaskDuration { get; set; }
    public string TaskDuration_min { get; set; }
    public int? P_Status { get; set; }
    public string status_name { get; set; }
    public string status_color { get; set; }
    public int? Priority { get; set; }
    public string priority_name { get; set; }
    public string priority_color { get; set; }
    public double? Budget { get; set; }
    public double? Actual { get; set; }
    public string Reminder { get; set; }
    public int? ReminderType { get; set; }
    public string Days { get; set; }
    public string Time { get; set; }
    public int? uid { get; set; }
    public DateTime? cdate { get; set; }
    public string tagto { get; set; }
    public string dependancy { get; set; }
    public int? issubtask { get; set; }
    public string checkTaskWeightage { get; set; }
    public int? iscomplete { get; set; }
    public double? iscompletepercentage { get; set; }
    public int? orderno { get; set; }
    public string txtdays { get; set; }
    public string complete_value { get; set; }
    public string pending_value { get; set; }
    public bool isExpanded { get; set; }
    
    public string leftdays { get; set; }
    public int? totalissue { get; set; }
    public int? pendingissue { get; set; }
    public int? completeissue { get; set; }
    public double? pendingissue_rate { get; set; }
    public double? completeissue_rate { get; set; }
    public int? totaldependency { get; set; }
    public int? isreassign { get; set; }
    public List<ownerlist> owners { get; set; }
    public List<teamlist> teams { get; set; }
    public List<projectfiles> viewfiles { get; set; }
    public List<newsubtask> subtasks { get; set; }
    //public List<issuestdata> taskissues { get; set; }
    public string CriticalLabelId { get; set; }
    public string CriticalLabel { get; set; }
    public string CriticalLabelcolor { get; set; }

}
public class newsubtask
{
    public int? id { get; set; }
    public int? ProjectId { get; set; }
    public int? Milestoneid { get; set; }
    public int? Task { get; set; }
    public string Subtaskname { get; set; }
    public string SubTaskDescription { get; set; }
    public string owner { get; set; }
    public string assignto { get; set; }
    public string team { get; set; }
    //public DateTime? Startdate { get; set; }
    //public DateTime? Targetdate { get; set; }
    public string Startdate { get; set; }
    public string Targetdate { get; set; }
    public double? Weightage { get; set; }
    public string StartTime { get; set; }
    public string Timeline { get; set; }
    public string TaskDuration { get; set; }
    public string TaskDuration_min { get; set; }
    public int? P_Status { get; set; }
    public string status_name { get; set; }
    public string status_color { get; set; }
    public int? Priority { get; set; }
    public string priority_name { get; set; }
    public string priority_color { get; set; }
    public double? Budget { get; set; }
    public double? Actual { get; set; }
    public string Reminder { get; set; }
    public int? ReminderType { get; set; }
    public string Days { get; set; }
    public string Time { get; set; }
    public int? uid { get; set; }
    public DateTime? cdate { get; set; }
    public string tagto { get; set; }
    public string dependancy { get; set; }
    public int? iscomplete { get; set; }
    public double? iscompletepercentage { get; set; }
    public int? orderno { get; set; }
    public string txtdays { get; set; }
    public string complete_value { get; set; }
    public string pending_value { get; set; }
    
    public string leftdays { get; set; }
    public int? totalissue { get; set; }
    public int? pendingissue { get; set; }
    public int? completeissue { get; set; }
    public double? pendingissue_rate { get; set; }
    public double? completeissue_rate { get; set; }

    public int? totaldependency { get; set; }
    public int? isreassign { get; set; }
    public List<ownerlist> owners { get; set; }
    public List<teamlist> teams { get; set; }
    public List<projectfiles> viewfiles { get; set; }
    //public List<issuestdata> subtaskissues { get; set; }
    public string CriticalLabelId { get; set; }
    public string CriticalLabel { get; set; }
    public string CriticalLabelcolor { get; set; }
    public string isrework { get; set; }
    public string lastactionname { get; set; }
    public string lastactiondate { get; set; }
    public string lastactionremark { get; set; }

}
public class projectfiles
{
    public string id { get; set; }
    public string pid { get; set; }
    public string Uploadby { get; set; }
    public string fentryid { get; set; }
    public string ftype { get; set; }
    public string fileurl { get; set; }
    public string filedes { get; set; }
    public string fext { get; set; }
    public string filepath { get; set; }


}

public class ownerlist : IPerson { public string id { get; set; } public string name { get; set; } public string profileimage { get; set; } public string shortname { get; set; } }
public class teamlist : IPerson { public string id { get; set; } public string name { get; set; } public string profileimage { get; set; } public string shortname { get; set; } }
public class userlist : IPerson { public string id { get; set; } public string empid { get; set; } public string name { get; set; } public string profileimage { get; set; } public string shortname { get; set; } }
public class taskSegment
{
    public int? assignid { get; set; }
    public int? taskid { get; set; }
    public string tasktype { get; set; }
    public string taskname { get; set; }
    public DateTime? workDate { get; set; }
    // Date-only companion used by the browser instead of /Date(milliseconds)/.
    public string workDateKey { get; set; }
    public int DurationMinutes { get; set; }
    public string TaskDuration { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public double? left { get; set; }
    public double? width { get; set; }

    public string mid { get; set; }
    public string tid { get; set; }

}
public class userlist_withtask
{
    public string id { get; set; }
    public string empid { get; set; }
    public string name { get; set; }
    public string profileimage { get; set; }
    public string shortname { get; set; }
    
    public string ShiftStart { get; set; }
    public string ShiftHours { get; set; }
    public string ShiftEnd { get; set; }
    public string tskleft { get; set; }
    public string tskwidth { get; set; }
    public string timelineWidth { get; set; }

    public List<userhrs> hours { get; set; }
    public List<assignsubtasklist> usertasks { get; set; }
    public List<taskSegment> taskSegments { get; set; }

}
public class userhrs
{
    public string hour { get; set; }
    public string period { get; set; }
}
public class statuslist
{  public string id { get; set; }
    public string name { get; set; }
    public string colorname { get; set; }
    public string iscomplete { get; set; }
    public string isdefault { get; set; }
}
public class tasksituationlist
{
    public string id { get; set; }
    public string name { get; set; }
    public string colorname { get; set; }
}
public class prioritylist
{
    public string id { get; set; }
    public string name { get; set; }
    public string colorname { get; set; }
    public string isdefault { get; set; }
}
public interface IPerson
{
    string id { get; set; }
    string name { get; set; }
    string profileimage { get; set; }
    string shortname { get; set; }
}
public class newMilerequest
{
    public newmilestone milestone { get; set; }
    public int orderno { get; set; }
}
public class newTaskrequest
{
    public newtask task { get; set; }
    public int orderno { get; set; }
}
public class newSubTaskrequest
{
    public newsubtask subtask { get; set; }
    public int orderno { get; set; }
}
public class projectowneres
{
    public string id { get; set; }
    public string Owner { get; set; }
    public string Ownertype { get; set; }
}
public class projectteams
{
    public string id { get; set; }
    public string Team { get; set; }
    public string Teamtype { get; set; }
}
public class newuserlist {
    public string id { get; set; }
    public string name { get; set; }
    
}
public class issuesdetails
{
    public int? totalissue { get; set; }
    public int? completeissue { get; set; }
    public int? pendingissue { get; set; }
    public double? completeissues_rate { get; set; }
    public double? pendingissues_rate { get; set; }

}
public class issuestdata
{
    public int? id { get; set; }
    public int? ProjectId { get; set; }
    public int? Milestoneid { get; set; }
    public string tasklistid { get; set; }
    public int? taskid { get; set; }
    public int? subtaskid { get; set; }
    public string Issues { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Owner { get; set; }
    public string team { get; set; }
    public string Issueon { get; set; }
    public string project_id { get; set; }
    public string Description { get; set; }
    public string Reminder { get; set; }
    public string ReminderType { get; set; }
    public string Days { get; set; }
    public string Time { get; set; }
    public string Notifyusers { get; set; }
    public string ChooseDate { get; set; }
    public string AssignTo { get; set; }
    public int? IssueStatus { get; set; }
    public int? Priority { get; set; }
    public string IssueType { get; set; }
    public string IssueTypeName { get; set; }
    public string Replicability { get; set; }
    public string UploadImage { get; set; }

}
public class completiondetails_mile
{
    public int? id { get; set; }
    public int? ProjectId { get; set; }
    public double? complete_value { get; set; }
    public double? pending_value { get; set; }
    public List<newtask_details> tasks_details { get; set; }

}
public class newtask_details
{
    public int? id { get; set; }
    public int? ProjectId { get; set; }
    public int? Milestoneid { get; set; }
    public double? complete_value { get; set; }
    public double? pending_value { get; set; }
    public List<newsubtask_details> subtasks_details { get; set; } 


}
public class newsubtask_details
{
    public int? id { get; set; }
    public int? ProjectId { get; set; }
    public int? Milestoneid { get; set; }
    public int? Task { get; set; }
    public double? complete_value { get; set; }
    public double? pending_value { get; set; }
}
public class projectlists
{
    public string id { get; set; }
    public string ProjectName { get; set; }
}
