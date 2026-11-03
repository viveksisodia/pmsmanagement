using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.UI;
using CommonClass;

public partial class pms_team_dashboard : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int regId, uid;
        if (!TryIdentity(out regId, out uid)) { Response.Redirect("/login.aspx", true); return; }
        DataSet access = new DataAccess().GetDataSet("dbo.pms_ManagerDashboardAllowedEmployees_team", new SqlParameter[] {
            new SqlParameter("@regid", regId), new SqlParameter("@isadmin", SqlDbType.Int) { Value = 0 }, new SqlParameter("@uid", uid)
        });
        if (access == null || access.Tables.Count < 2) throw new InvalidOperationException("Install the team dashboard employee procedure.");
        int admin = Convert.ToInt32(access.Tables[1].Rows[0]["IsAdmin"], CultureInfo.InvariantCulture);
        if (admin == 0 && access.Tables[0].Rows.Count <= 1) Response.Redirect("/pms/userdashboard/default.aspx", true);
    }
    private static bool TryIdentity(out int regId, out int uid)
    {
        regId = uid = 0;
        HttpCookie tenant = HttpContext.Current.Request.Cookies["cadmin"];
        HttpCookie user = HttpContext.Current.Request.Cookies["user"];
        return tenant != null && user != null && Int32.TryParse(tenant["id"], out regId) &&
            Int32.TryParse(user["user_id"], out uid) && regId > 0 && uid > 0;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetTeamDashboardData(string fromDate, string toDate, string today, int projectId)
    {
        int regId, uid;
        if (!TryIdentity(out regId, out uid)) throw new HttpException(401, "Login information is missing or invalid.");
        DateTime localToday = ParseDate(today);
        bool allDates = String.IsNullOrEmpty(fromDate) && String.IsNullOrEmpty(toDate);
        DateTime start = allDates ? localToday : ParseDate(fromDate);
        DateTime end = allDates ? localToday : ParseDate(toDate);
        if (end < start || (end-start).TotalDays > 366 || projectId < 0)
            throw new ArgumentException("Select a valid date range of up to one year.");
        DataAccess da = new DataAccess();
        DataSet allowed = da.GetDataSet("dbo.pms_ManagerDashboardAllowedEmployees_team", new SqlParameter[] {
            new SqlParameter("@regid", regId), new SqlParameter("@isadmin", SqlDbType.Int) { Value = 0 }, new SqlParameter("@uid", uid)
        });
        if (allowed == null || allowed.Tables.Count < 2) throw new InvalidOperationException("Install the team dashboard employee procedure.");
        var ids = allowed.Tables[0].Rows.Cast<DataRow>().Select(r => Convert.ToInt32(r["id"], CultureInfo.InvariantCulture)).Distinct().ToList();
        if (!ids.Contains(uid)) throw new HttpException(403, "Employee access is unavailable.");
        int isAdmin = Convert.ToInt32(allowed.Tables[1].Rows[0]["IsAdmin"], CultureInfo.InvariantCulture);
        DateTime weekStart = localToday.AddDays(-(((int)localToday.DayOfWeek+6)%7));
        DateTime calendarEnd = localToday.AddDays(120);
        DataSet data = da.GetDataSet("dbo.pms_ManagerDashboardProjects_team", new SqlParameter[] {
            new SqlParameter("@regid",regId), new SqlParameter("@AllowedEmployeeIds", SqlDbType.NVarChar,-1) { Value=String.Join(",",ids.Select(i=>i.ToString(CultureInfo.InvariantCulture)).ToArray()) },
            new SqlParameter("@projectId",projectId),new SqlParameter("@fromDate",SqlDbType.Date){Value=allDates ? (object)DBNull.Value : start},
            new SqlParameter("@toDate",SqlDbType.Date){Value=allDates ? (object)DBNull.Value : end},new SqlParameter("@calendarStart",SqlDbType.Date){Value=weekStart},
            new SqlParameter("@calendarEnd",SqlDbType.Date){Value=calendarEnd}
        });
        if (data==null || data.Tables.Count<8) throw new InvalidOperationException("Install the team dashboard data procedure.");
        DataRow policy=data.Tables[5].Rows.Count>0?data.Tables[5].Rows[0]:null;
        var holidays=new HashSet<string>(data.Tables[6].Rows.Cast<DataRow>().Select(r=>Key(Convert.ToDateTime(r["EventDate"],CultureInfo.InvariantCulture))));
        var offDays=new HashSet<string>();
        for(DateTime day=weekStart;day<=calendarEnd;day=day.AddDays(1))
            if(WeeklyIsWeekOff(day,policy)) offDays.Add(Key(day));
        var employees=new List<object>();
        foreach(DataRow employee in data.Tables[0].Rows)
        {
            int id=Convert.ToInt32(employee["EmployeeId"],CultureInfo.InvariantCulture);
            bool shiftConfigured=employee["starttime"]!=DBNull.Value && employee["endtime"]!=DBNull.Value;
            TimeSpan shiftStart,shiftEnd;
            shiftConfigured=shiftConfigured && TryShiftTime(Text(employee,"starttime"),out shiftStart) && TryShiftTime(Text(employee,"endtime"),out shiftEnd);
            double shiftHours=0;
            if(shiftConfigured) {
                TryShiftTime(Text(employee,"starttime"),out shiftStart); TryShiftTime(Text(employee,"endtime"),out shiftEnd);
                double minutes=(shiftEnd-shiftStart).TotalMinutes;if(minutes<=0)minutes+=1440;
                shiftHours=Math.Max(0,minutes-Math.Min(minutes,Math.Max(0,Number(employee,"LunchMinutes"))))/60.0;
            }
            var leaves=new Dictionary<string,int>();
            foreach(DataRow leave in data.Tables[4].Rows)
            {
                if(Convert.ToInt32(leave["EmployeeId"],CultureInfo.InvariantCulture)!=id)continue;
                DateTime leaveStart=Convert.ToDateTime(leave["fromdate"],CultureInfo.InvariantCulture).Date;
                DateTime leaveEnd=Convert.ToDateTime(leave["todate"],CultureInfo.InvariantCulture).Date;
                bool half=Flag(leave,"ishalfday"),first=Flag(leave,"firsthalfday"),second=Flag(leave,"secondhalhday");
                int mask=!half?3:first!=second?(first?1:2):4;
                for(DateTime day=leaveStart>weekStart?leaveStart:weekStart;day<=leaveEnd && day<=calendarEnd;day=day.AddDays(1)) {
                    string key=Key(day);int old;leaves.TryGetValue(key,out old);leaves[key]=old|mask;
                }
            }
            var spent=new Dictionary<string,double>();
            foreach(DataRow log in data.Tables[3].Rows)
                if(Convert.ToInt32(log["EmployeeId"],CultureInfo.InvariantCulture)==id) spent[Text(log,"WorkDate")]=Number(log,"Hours");
            var days=new List<object>();double expected=0,weekCapacity=0,nextCapacity=0;int missing=0,workDays=0;
            for(DateTime day=weekStart;day<=calendarEnd;day=day.AddDays(1)) {
                string key=Key(day);int mask;leaves.TryGetValue(key,out mask);
                double fraction=(mask&3)==3?1:mask==0?0:0.5;
                bool nonwork=holidays.Contains(key)||offDays.Contains(key);
                double capacity=nonwork?0:shiftHours*(1-fraction);
                double hours;spent.TryGetValue(key,out hours);
                if(day>=localToday && !nonwork && workDays<10) { nextCapacity+=capacity;workDays++; }
                if(day<weekStart.AddDays(7)) {
                    weekCapacity+=capacity;
                    if(day<=localToday) {expected+=capacity;if(capacity>0 && hours<capacity) missing++;}
                    days.Add(new {key=key, label=day.ToString("ddd",CultureInfo.InvariantCulture),spent=hours,capacity=capacity,
                        future=day>localToday,leave=fraction,leaveLabel=fraction==0?"":(fraction==1?"Full Day":(mask&3)==1?"First Half":(mask&3)==2?"Second Half":"Half Day")+" (Approved)",
                        holiday=holidays.Contains(key),weekOff=offDays.Contains(key)});
                }
            }
            employees.Add(new {id=id,name=Text(employee,"DisplayName"),days=days,shiftConfigured=shiftConfigured,shiftHours=shiftHours,
                expected=Math.Round(expected,4),weekCapacity=Math.Round(weekCapacity,4),nextCapacity=Math.Round(nextCapacity,4),missing=missing});
        }
        string currentName=Text(allowed.Tables[0].Rows.Cast<DataRow>().First(r=>Convert.ToInt32(r["id"])==uid),"DisplayName");
        return new { userName=currentName,userId=uid,isAdmin=isAdmin,hasTeam=ids.Count>1,
            today=Key(localToday),fromDate=allDates ? null : Key(start),toDate=allDates ? null : Key(end),employees=employees,tasks=Rows(data.Tables[1]),
            issues=Rows(data.Tables[2]),projects=Rows(data.Tables[7]) };
    }
    private static DateTime ParseDate(string text) {
        DateTime value;if(!DateTime.TryParseExact(text,"yyyy-MM-dd",CultureInfo.InvariantCulture,DateTimeStyles.None,out value))
            throw new ArgumentException("Dates must use yyyy-MM-dd.");return value.Date;
    }
    private static string Key(DateTime date){return date.ToString("yyyy-MM-dd",CultureInfo.InvariantCulture);}
    private static string Text(DataRow row,string column){return row.Table.Columns.Contains(column)&&row[column]!=DBNull.Value?Convert.ToString(row[column],CultureInfo.InvariantCulture):"";}
    private static double Number(DataRow row,string column){double value;return Double.TryParse(Text(row,column),NumberStyles.Any,CultureInfo.InvariantCulture,out value)?value:0;}
    private static bool Flag(DataRow row,string column){string value=Text(row,column);return value=="1"||value.Equals("true",StringComparison.OrdinalIgnoreCase);}
    private static bool TryShiftTime(string value,out TimeSpan result) {
        DateTime date;string[] formats={"hh:mm tt","h:mm tt","HH:mm","HH:mm:ss","H:mm","H:mm:ss"};
        if(DateTime.TryParseExact(value.Trim(),formats,CultureInfo.InvariantCulture,DateTimeStyles.AllowWhiteSpaces,out date)){result=date.TimeOfDay;return true;}
        return TimeSpan.TryParse(value,CultureInfo.InvariantCulture,out result);
    }
    private static List<Dictionary<string,object>> Rows(DataTable table){
        var rows=new List<Dictionary<string,object>>();foreach(DataRow row in table.Rows){
            var item=new Dictionary<string,object>();foreach(DataColumn column in table.Columns)
                item[column.ColumnName]=row[column]==DBNull.Value?null:row[column] is DateTime?(object)Key((DateTime)row[column]):row[column];
            rows.Add(item);
        }return rows;
    }
    private static int WeeklyGetWeekNumber(DateTime date)
    {
        try
        {
            DataAccess da = new DataAccess();
            SqlParameter[] prm = new SqlParameter[1];
            prm[0] = new SqlParameter("@DATE", date);
            DataSet ds = da.GetDataSet("selectweeknofromdate", prm);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int n;
                if (Int32.TryParse(ds.Tables[0].Rows[0]["WEEK_OF_MONTH"].ToString(), out n)) return n;
            }
        }
        catch { }
        return ((date.Day - 1) / 7) + 1;
    }

    private static int WeeklyReturnStatus(string type, int weekNo)
    {
        if (type == "1") return 1;
        if (type == "2" && weekNo == 1) return 1;
        if (type == "3" && weekNo == 2) return 1;
        if (type == "4" && weekNo == 3) return 1;
        if (type == "5" && weekNo == 4) return 1;
        if (type == "6" && weekNo == 5) return 1;
        if (type == "7" && (weekNo == 1 || weekNo == 3)) return 1;
        if (type == "8" && (weekNo == 1 || weekNo == 3 || weekNo == 5)) return 1;
        if (type == "9" && (weekNo == 2 || weekNo == 4)) return 1;
        return 0;
    }

    private static bool WeeklyIsWeekOff(DateTime date, DataRow row)
    {
        if (row == null) return false;
        try
        {
            int dayNo = ((int)date.DayOfWeek) + 1; // Sunday=1 ... Saturday=7, same as Assign Project.
            string enabledColumn = "isweekoff" + dayNo.ToString(CultureInfo.InvariantCulture);
            string typeColumn = enabledColumn + "type";
            if (!row.Table.Columns.Contains(enabledColumn) || !row.Table.Columns.Contains(typeColumn)) return false;
            if (row[enabledColumn].ToString() != "1") return false;
            string type = row[typeColumn].ToString();
            if (type == "1") return true;
            return WeeklyReturnStatus(type, WeeklyGetWeekNumber(date)) == 1;
        }
        catch { return false; }
    }


}
