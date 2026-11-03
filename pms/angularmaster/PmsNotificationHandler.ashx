<%@ WebHandler Language="C#" Class="PmsNotificationHandler" %>

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.Script.Serialization;
using CommonClass;

public class PmsNotificationHandler : IHttpHandler
{
    private const int DefaultTake = 8;
    private const int MaxTake = 100;

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.ContentEncoding = System.Text.Encoding.UTF8;
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        context.Response.Cache.SetNoStore();

        try
        {
            int regid = GetCookieInt(context, "cadmin", "id");
            int uid = GetCookieInt(context, "user", "user_id");

            if (regid <= 0 || uid <= 0)
            {
                context.Response.StatusCode = 401;
                WriteJson(context, new { success = false, message = "Session expired." });
                return;
            }

            string action = (context.Request["action"] ?? "initial").Trim().ToLowerInvariant();

            if (action == "initial")
            {
                int unreadTake = ClampTake(ParseInt(context.Request["unreadTake"], DefaultTake));
                int readTake = ClampTake(ParseInt(context.Request["readTake"], DefaultTake));
                WriteInitial(context, regid, uid, unreadTake, readTake);
                return;
            }

            if (action == "check")
            {
                WriteBellCheck(context, regid, uid);
                return;
            }

            if (action == "load")
            {
                string section = (context.Request["section"] ?? "").Trim().ToLowerInvariant();
                int beforeId = ParseInt(context.Request["beforeId"], 0);
                int take = ClampTake(ParseInt(context.Request["take"], DefaultTake));

                if (section != "unread" && section != "read")
                {
                    WriteJson(context, new { success = false, message = "Invalid section." });
                    return;
                }

                WriteMore(context, regid, uid, section, beforeId, take);
                return;
            }

            if (action == "markread")
            {
                int id = ParseInt(context.Request["id"], 0);
                if (id <= 0)
                {
                    WriteJson(context, new { success = false, message = "Invalid notification id." });
                    return;
                }

                MarkRead(context, regid, uid, id);
                return;
            }

            WriteJson(context, new { success = false, message = "Invalid action." });
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            WriteJson(context, new { success = false, message = ex.Message });
        }
    }

    private static void WriteBellCheck(HttpContext context, int regid, int uid)
    {
        // Lightweight 1-minute polling query: counts + latest ids only.
        // No notification text/list payload is returned unless the Angular client detects a change.
        string sql = @"
SELECT COUNT(1) AS UnreadCount, ISNULL(MAX(id),0) AS LatestUnreadId
FROM tbl_pnotificationdetails WITH(NOLOCK)
WHERE regid=" + regid + @"
  AND nuid=" + uid + @"
  AND status=1 AND archive=0 AND typeid in(222,223,224)
  AND ISNULL(isread,0)=0;

SELECT COUNT(1) AS ReadCount, ISNULL(MAX(id),0) AS LatestReadId
FROM tbl_pnotificationdetails WITH(NOLOCK)
WHERE regid=" + regid + @"
  AND nuid=" + uid + @"
  AND status=1 AND archive=0 AND typeid in(222,223,224)
  AND ISNULL(isread,0)=1;";

        DataAccess da = new DataAccess();
        DataSet ds = da.GetDataSetInline(sql);

        DataTable unreadTable = GetTable(ds, 0);
        DataTable readTable = GetTable(ds, 1);

        WriteJson(context, new
        {
            success = true,
            unreadCount = ReadCount(unreadTable, "UnreadCount"),
            readCount = ReadCount(readTable, "ReadCount"),
            latestUnreadId = ReadCount(unreadTable, "LatestUnreadId"),
            latestReadId = ReadCount(readTable, "LatestReadId")
        });
    }

    private static void WriteInitial(HttpContext context, int regid, int uid, int unreadTake, int readTake)
    {
        // Values are ints parsed by server; no user text is concatenated into SQL.
        string sql = @"
SELECT TOP " + (unreadTake + 1) + @"
       id, notificationtext, ndatetime
FROM tbl_pnotificationdetails WITH(NOLOCK)
WHERE regid=" + regid + @"
  AND nuid=" + uid + @"
  AND status=1 AND archive=0 AND typeid in(222,223,224)
  AND ISNULL(isread,0)=0
ORDER BY id DESC;

SELECT COUNT(1) AS UnreadCount
FROM tbl_pnotificationdetails WITH(NOLOCK)
WHERE regid=" + regid + @"
  AND nuid=" + uid + @"
  AND status=1 AND archive=0 AND typeid in(222,223,224)
  AND ISNULL(isread,0)=0;

SELECT TOP " + (readTake + 1) + @"
       id, notificationtext, ndatetime
FROM tbl_pnotificationdetails WITH(NOLOCK)
WHERE regid=" + regid + @"
  AND nuid=" + uid + @"
  AND status=1 AND archive=0 AND typeid in(222,223,224)
  AND ISNULL(isread,0)=1
ORDER BY id DESC;

SELECT COUNT(1) AS ReadCount
FROM tbl_pnotificationdetails WITH(NOLOCK)
WHERE regid=" + regid + @"
  AND nuid=" + uid + @"
  AND status=1 AND archive=0 AND typeid in(222,223,224)
  AND ISNULL(isread,0)=1;";

        DataAccess da = new DataAccess();
        DataSet ds = da.GetDataSetInline(sql);

        DataTable unreadTable = GetTable(ds, 0);
        DataTable unreadCountTable = GetTable(ds, 1);
        DataTable readTable = GetTable(ds, 2);
        DataTable readCountTable = GetTable(ds, 3);

        int unreadCount = ReadCount(unreadCountTable, "UnreadCount");
        int readCount = ReadCount(readCountTable, "ReadCount");

        bool unreadHasMore = unreadTable != null && unreadTable.Rows.Count > unreadTake;
        bool readHasMore = readTable != null && readTable.Rows.Count > readTake;

        WriteJson(context, new
        {
            success = true,
            unreadCount = unreadCount,
            readCount = readCount,
            unread = ToNotificationList(unreadTable, unreadTake),
            read = ToNotificationList(readTable, readTake),
            unreadHasMore = unreadHasMore,
            readHasMore = readHasMore
        });
    }

    private static void WriteMore(HttpContext context, int regid, int uid, string section, int beforeId, int take)
    {
        int isRead = section == "read" ? 1 : 0;
        string beforeClause = beforeId > 0 ? " AND id<" + beforeId : "";

        string sql = @"
SELECT TOP " + (take + 1) + @"
       id, notificationtext, ndatetime
FROM tbl_pnotificationdetails WITH(NOLOCK)
WHERE regid=" + regid + @"
  AND nuid=" + uid + @"
  AND status=1 AND archive=0 AND typeid in(222,223,224)
  AND ISNULL(isread,0)=" + isRead + beforeClause + @"
ORDER BY id DESC;

SELECT COUNT(1) AS UnreadCount
FROM tbl_pnotificationdetails WITH(NOLOCK)
WHERE regid=" + regid + @"
  AND nuid=" + uid + @"
  AND status=1 AND archive=0 AND typeid in(222,223,224)
  AND ISNULL(isread,0)=0;

SELECT COUNT(1) AS ReadCount
FROM tbl_pnotificationdetails WITH(NOLOCK)
WHERE regid=" + regid + @"
  AND nuid=" + uid + @"
  AND status=1 AND archive=0 AND typeid in(222,223,224)
  AND ISNULL(isread,0)=1;";

        DataAccess da = new DataAccess();
        DataSet ds = da.GetDataSetInline(sql);
        DataTable items = GetTable(ds, 0);
        int unreadCount = ReadCount(GetTable(ds, 1), "UnreadCount");
        int readCount = ReadCount(GetTable(ds, 2), "ReadCount");
        bool hasMore = items != null && items.Rows.Count > take;

        WriteJson(context, new
        {
            success = true,
            items = ToNotificationList(items, take),
            hasMore = hasMore,
            unreadCount = unreadCount,
            readCount = readCount
        });
    }

    private static void MarkRead(HttpContext context, int regid, int uid, int id)
    {
        // Update only the logged-in user's own PMS critical notification.
        string updateSql = @"
UPDATE tbl_pnotificationdetails
SET isread=1
WHERE id=" + id + @"
  AND regid=" + regid + @"
  AND nuid=" + uid + @"
  AND typeid in(222,223,224)
  AND status=1
  AND archive=0
  AND ISNULL(isread,0)=0;";

        DataAccess da = new DataAccess();
        da.executeDMLQueryinline(updateSql);

        string countSql = @"
SELECT COUNT(1) AS UnreadCount
FROM tbl_pnotificationdetails WITH(NOLOCK)
WHERE regid=" + regid + @" AND nuid=" + uid + @"
  AND status=1 AND archive=0 AND typeid in(222,223,224) AND ISNULL(isread,0)=0;
SELECT COUNT(1) AS ReadCount
FROM tbl_pnotificationdetails WITH(NOLOCK)
WHERE regid=" + regid + @" AND nuid=" + uid + @"
  AND status=1 AND archive=0 AND typeid in(222,223,224) AND ISNULL(isread,0)=1;";

        DataSet ds = da.GetDataSetInline(countSql);
        WriteJson(context, new
        {
            success = true,
            unreadCount = ReadCount(GetTable(ds, 0), "UnreadCount"),
            readCount = ReadCount(GetTable(ds, 1), "ReadCount")
        });
    }

    private static List<object> ToNotificationList(DataTable table, int take)
    {
        List<object> result = new List<object>();
        if (table == null) return result;

        int count = Math.Min(take, table.Rows.Count);
        for (int i = 0; i < count; i++)
        {
            DataRow row = table.Rows[i];
            DateTime dateValue;
            string dateText = "";
            if (row["ndatetime"] != DBNull.Value && DateTime.TryParse(Convert.ToString(row["ndatetime"]), out dateValue))
                dateText = dateValue.ToString("dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);

            result.Add(new
            {
                id = Convert.ToInt32(row["id"]),
                notificationtext = Convert.ToString(row["notificationtext"]),
                ndatetime = dateText
            });
        }
        return result;
    }

    private static DataTable GetTable(DataSet ds, int index)
    {
        return ds != null && ds.Tables.Count > index ? ds.Tables[index] : null;
    }

    private static int ReadCount(DataTable table, string column)
    {
        int value = 0;
        if (table != null && table.Rows.Count > 0)
            Int32.TryParse(Convert.ToString(table.Rows[0][column]), out value);
        return value;
    }

    private static int GetCookieInt(HttpContext context, string cookieName, string key)
    {
        HttpCookie cookie = context.Request.Cookies[cookieName];
        if (cookie == null) return 0;
        return ParseInt(cookie[key], 0);
    }

    private static int ParseInt(string value, int defaultValue)
    {
        int parsed;
        return Int32.TryParse(value, out parsed) ? parsed : defaultValue;
    }

    private static int ClampTake(int value)
    {
        if (value < DefaultTake) return DefaultTake;
        if (value > MaxTake) return MaxTake;
        return value;
    }

    private static void WriteJson(HttpContext context, object value)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        context.Response.Write(serializer.Serialize(value));
    }

    public bool IsReusable { get { return false; } }
}
