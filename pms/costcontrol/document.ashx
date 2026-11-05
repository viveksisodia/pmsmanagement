<%@ WebHandler Language="C#" Class="PmsCostControlDocumentHandler" %>

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Web;

/// <summary>
/// Cost entry documents (Phase 4).
///   GET  document.ashx?id=N[&amp;download=1]  view / download one document (View right + project access)
///   POST document.ashx?entryId=N            multipart upload of one or more files (Enter right + project access)
/// Files are stored outside the public web folder: ~/App_Data/CostControlDocs/&lt;regid&gt;/&lt;entry id&gt;/&lt;guid&gt;.&lt;ext&gt;.
/// Only the types below are accepted, up to 10 MB each; the first bytes must match the extension.
/// Removal is a WebMethod in default.aspx.cs (RemoveCostEntryDocument).
/// </summary>
public class PmsCostControlDocumentHandler : IHttpHandler
{
    private const int MaxFileBytes = 10 * 1024 * 1024;
    private const int MaxFilesPerUpload = 10;

    private sealed class FileType
    {
        public string ContentType; public bool Inline; public byte[][] Signatures;
        public FileType(string contentType, bool inline, params byte[][] signatures) { ContentType = contentType; Inline = inline; Signatures = signatures; }
    }

    private static readonly byte[] Zip = { 0x50, 0x4B, 0x03, 0x04 };
    private static readonly byte[] Ole = { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 };

    private static readonly Dictionary<string, FileType> Types = new Dictionary<string, FileType>(StringComparer.OrdinalIgnoreCase)
    {
        { ".pdf",  new FileType("application/pdf", true, new byte[] { 0x25, 0x50, 0x44, 0x46 }) },
        { ".jpg",  new FileType("image/jpeg", true, new byte[] { 0xFF, 0xD8, 0xFF }) },
        { ".jpeg", new FileType("image/jpeg", true, new byte[] { 0xFF, 0xD8, 0xFF }) },
        { ".png",  new FileType("image/png", true, new byte[] { 0x89, 0x50, 0x4E, 0x47 }) },
        { ".gif",  new FileType("image/gif", true, new byte[] { 0x47, 0x49, 0x46, 0x38 }) },
        { ".webp", new FileType("image/webp", true, new byte[] { 0x52, 0x49, 0x46, 0x46 }) },
        { ".doc",  new FileType("application/msword", false, Ole) },
        { ".docx", new FileType("application/vnd.openxmlformats-officedocument.wordprocessingml.document", false, Zip) },
        { ".xls",  new FileType("application/vnd.ms-excel", false, Ole) },
        { ".xlsx", new FileType("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", false, Zip) },
        { ".csv",  new FileType("text/csv", false) },
        { ".txt",  new FileType("text/plain", false) }
    };

    public bool IsReusable { get { return false; } }

    public void ProcessRequest(HttpContext context)
    {
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        context.Response.Headers["X-Content-Type-Options"] = "nosniff";
        try
        {
            if (context.Request.HttpMethod == "POST") Upload(context);
            else Download(context);
        }
        catch (HttpException ex)
        {
            Fail(context, ex.GetHttpCode() > 0 ? ex.GetHttpCode() : 400, ex.Message);
        }
        catch (Exception ex)
        {
            Fail(context, 400, ex.Message);
        }
    }

    private static void Fail(HttpContext context, int statusCode, string message)
    {
        context.Response.Clear();
        context.Response.StatusCode = statusCode;
        context.Response.TrySkipIisCustomErrors = true;
        context.Response.ContentType = "application/json";
        context.Response.Write(PmsCostControl.ToJson(new { Success = false, Message = message }));
    }

    private static string Folder(HttpContext context, int regId, int entryId)
    {
        return context.Server.MapPath("~/App_Data/CostControlDocs/" + regId.ToString(CultureInfo.InvariantCulture) + "/" + entryId.ToString(CultureInfo.InvariantCulture));
    }

    private static int QueryInt(HttpContext context, string name)
    {
        int value;
        return Int32.TryParse(context.Request.QueryString[name], NumberStyles.Integer, CultureInfo.InvariantCulture, out value) ? value : 0;
    }

    private static bool MatchesSignature(HttpPostedFile file, FileType type)
    {
        if (type.Signatures == null || type.Signatures.Length == 0) return true;
        byte[] head = new byte[8];
        Stream stream = file.InputStream;
        stream.Position = 0;
        int read = stream.Read(head, 0, head.Length);
        stream.Position = 0;
        foreach (byte[] signature in type.Signatures)
        {
            if (read < signature.Length) continue;
            bool same = true;
            for (int i = 0; i < signature.Length; i++) if (head[i] != signature[i]) { same = false; break; }
            if (same) return true;
        }
        return false;
    }

    private static string CleanFileName(string name)
    {
        name = Path.GetFileName(name ?? "").Trim();
        foreach (char c in Path.GetInvalidFileNameChars()) name = name.Replace(c, '_');
        if (name.Length > 200) name = name.Substring(name.Length - 200);
        return name;
    }

    private static void Upload(HttpContext context)
    {
        // A custom header cannot be sent by a cross-site form, so this blocks forged uploads.
        if (context.Request.Headers["X-CC-Upload"] != "1") throw new HttpException(400, "Invalid upload request.");
        var login = PmsCostControl.Require(PmsCostControl.Need.Enter);
        int entryId = QueryInt(context, "entryId");
        if (entryId <= 0) throw new HttpException(400, "Save the cost entry before attaching documents.");
        PmsCostControl.RequireRecordProject(login, "CostEntry", entryId);

        HttpFileCollection files = context.Request.Files;
        if (files.Count == 0) throw new HttpException(400, "Choose at least one file.");
        if (files.Count > MaxFilesPerUpload) throw new HttpException(400, "Upload at most " + MaxFilesPerUpload + " files at a time.");

        // Validate every file before anything is written.
        var checkedFiles = new List<KeyValuePair<HttpPostedFile, FileType>>();
        for (int i = 0; i < files.Count; i++)
        {
            HttpPostedFile file = files[i];
            string name = CleanFileName(file.FileName);
            FileType type;
            if (name == "" || !Types.TryGetValue(Path.GetExtension(name), out type))
                throw new HttpException(400, "\"" + name + "\": only PDF, image (JPG, PNG, GIF, WEBP), Word, Excel, CSV and text files can be attached.");
            if (file.ContentLength <= 0) throw new HttpException(400, "\"" + name + "\" is empty.");
            if (file.ContentLength > MaxFileBytes) throw new HttpException(400, "\"" + name + "\" is larger than 10 MB.");
            if (!MatchesSignature(file, type)) throw new HttpException(400, "\"" + name + "\" does not look like a valid " + Path.GetExtension(name).TrimStart('.').ToUpperInvariant() + " file.");
            checkedFiles.Add(new KeyValuePair<HttpPostedFile, FileType>(file, type));
        }

        DataSet check = PmsCostControl.RunProcedure("usp_pms_cc_CostEntryDoc_Add",
            new SqlParameter("@regid", login.RegId), new SqlParameter("@EntryId", entryId),
            new SqlParameter("@FileName", "check"), new SqlParameter("@StoredName", DBNull.Value),
            new SqlParameter("@ContentType", DBNull.Value), new SqlParameter("@FileSize", 1L),
            new SqlParameter("@NewFiles", checkedFiles.Count), new SqlParameter("@uid", login.EmployeeId));
        EnsureOk(check);

        string folder = Folder(context, login.RegId, entryId);
        Directory.CreateDirectory(folder);
        var saved = new List<object>();
        foreach (var item in checkedFiles)
        {
            HttpPostedFile file = item.Key;
            string name = CleanFileName(file.FileName);
            string storedName = Guid.NewGuid().ToString("N") + Path.GetExtension(name).ToLowerInvariant();
            string fullPath = Path.Combine(folder, storedName);
            file.SaveAs(fullPath);
            DataSet ds;
            try
            {
                ds = PmsCostControl.RunProcedure("usp_pms_cc_CostEntryDoc_Add",
                    new SqlParameter("@regid", login.RegId), new SqlParameter("@EntryId", entryId),
                    new SqlParameter("@FileName", name), new SqlParameter("@StoredName", storedName),
                    new SqlParameter("@ContentType", item.Value.ContentType), new SqlParameter("@FileSize", (long)file.ContentLength),
                    new SqlParameter("@NewFiles", 1), new SqlParameter("@uid", login.EmployeeId));
                EnsureOk(ds);
            }
            catch
            {
                try { File.Delete(fullPath); } catch { }
                throw;
            }
            saved.Add(new { id = Convert.ToInt32(ds.Tables[0].Rows[0]["id"], CultureInfo.InvariantCulture), EntryId = entryId, FileName = name, FileSize = file.ContentLength });
        }

        context.Response.ContentType = "application/json";
        context.Response.Write(PmsCostControl.ToJson(new { Success = true, Message = saved.Count + " document(s) attached.", Documents = saved }));
    }

    private static void EnsureOk(DataSet ds)
    {
        if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) throw new HttpException(500, "No response from the database.");
        DataRow row = ds.Tables[0].Rows[0];
        if (Convert.ToInt32(row["Result"], CultureInfo.InvariantCulture) != 1)
            throw new HttpException(400, Convert.ToString(row["Message"], CultureInfo.InvariantCulture));
    }

    private static void Download(HttpContext context)
    {
        var login = PmsCostControl.Require(PmsCostControl.Need.View);
        int id = QueryInt(context, "id");
        DataSet ds = PmsCostControl.RunProcedure("usp_pms_cc_CostEntryDoc_Get", new SqlParameter("@regid", login.RegId), new SqlParameter("@id", id));
        if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) throw new HttpException(404, "The document was not found.");
        DataRow row = ds.Tables[0].Rows[0];
        PmsCostControl.RequireProject(login, Convert.ToInt32(row["ProjectId"], CultureInfo.InvariantCulture));

        string storedName = Path.GetFileName(Convert.ToString(row["StoredName"], CultureInfo.InvariantCulture));
        int entryId = Convert.ToInt32(row["EntryId"], CultureInfo.InvariantCulture);
        string fullPath = Path.Combine(Folder(context, login.RegId, entryId), storedName);
        if (!File.Exists(fullPath)) throw new HttpException(404, "The file is missing on the server.");

        string fileName = Convert.ToString(row["FileName"], CultureInfo.InvariantCulture);
        FileType type;
        if (!Types.TryGetValue(Path.GetExtension(storedName), out type)) type = new FileType("application/octet-stream", false);
        bool inline = type.Inline && context.Request.QueryString["download"] != "1";

        string asciiName = new string(Array.ConvertAll(fileName.ToCharArray(), c => c < 32 || c > 126 || c == '"' || c == '\\' ? '_' : c));
        context.Response.ContentType = type.ContentType;
        context.Response.AddHeader("Content-Disposition", (inline ? "inline" : "attachment") +
            "; filename=\"" + asciiName + "\"; filename*=UTF-8''" + Uri.EscapeDataString(fileName));
        context.Response.TransmitFile(fullPath);
    }
}
