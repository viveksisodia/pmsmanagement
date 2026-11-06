using System.Data;
using Construction.Application.Common;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Construction.Infrastructure.Data;

public sealed class DbOptions
{
    /// <summary>Connection string for absolutebook_2172. Supplied by environment or user secrets, never committed.</summary>
    public string ConnectionString { get; set; } = "";
    public int CommandTimeoutSeconds { get; set; } = 60;
}

/// <summary>Runs cpm_usp_* stored procedures and turns their THROW codes into AppException.</summary>
public sealed class Db(DbOptions options)
{
    static Db()
    {
        SqlMapper.AddTypeHandler(new DateOnlyHandler());
        SqlMapper.AddTypeHandler(new NullableDateOnlyHandler());
    }

    private SqlConnection Open()
    {
        if (string.IsNullOrWhiteSpace(options.ConnectionString))
            throw new InvalidOperationException("The database connection string is not set (ConstructionDb:ConnectionString).");
        return new SqlConnection(options.ConnectionString);
    }

    public Task<IEnumerable<T>> QueryAsync<T>(string proc, object? args = null) => Run(c => c.QueryAsync<T>(Cmd(proc, args)));

    public Task<T?> QuerySingleAsync<T>(string proc, object? args = null) => Run(c => c.QuerySingleOrDefaultAsync<T>(Cmd(proc, args)));

    public Task<int> ExecuteAsync(string proc, object? args = null) => Run(c => c.ExecuteAsync(Cmd(proc, args)));

    public Task<TResult> MultipleAsync<TResult>(string proc, object? args, Func<SqlMapper.GridReader, Task<TResult>> read)
        => Run(async c =>
        {
            await using var grid = await c.QueryMultipleAsync(Cmd(proc, args));
            return await read(grid);
        });

    private CommandDefinition Cmd(string proc, object? args)
        => new(proc, args, commandType: CommandType.StoredProcedure, commandTimeout: options.CommandTimeoutSeconds);

    private async Task<T> Run<T>(Func<SqlConnection, Task<T>> work)
    {
        await using var c = Open();
        try { return await work(c); }
        catch (SqlException e) when (e.Number is >= 50400 and <= 50499)
        {
            throw new AppException(e.Number - 50000, e.Message);
        }
        catch (SqlException e) when (e.Number is 2601 or 2627)
        {
            throw new AppException(409, "This record already exists. Reload and check the code or number.");
        }
        catch (SqlException e) when (e.Number == 547)
        {
            throw new AppException(400, "The record refers to something that doesn't exist or is still in use.");
        }
    }

    /* ---------- table-valued parameters ---------- */
    public static SqlMapper.ICustomQueryParameter IntList(IEnumerable<int> ids)
    {
        var t = new DataTable(); t.Columns.Add("Id", typeof(int));
        foreach (var id in ids.Distinct()) t.Rows.Add(id);
        return t.AsTableValuedParameter("dbo.cpm_tt_IntList");
    }

    public static SqlMapper.ICustomQueryParameter CodeList(IEnumerable<string> codes)
    {
        var t = new DataTable(); t.Columns.Add("Code", typeof(string));
        foreach (var c in codes.Where(c => !string.IsNullOrWhiteSpace(c)).Distinct(StringComparer.OrdinalIgnoreCase)) t.Rows.Add(c);
        return t.AsTableValuedParameter("dbo.cpm_tt_CodeList");
    }

    public static DataTable NewTable(params (string Name, Type Type)[] columns)
    {
        var t = new DataTable();
        foreach (var (name, type) in columns) t.Columns.Add(name, Nullable.GetUnderlyingType(type) ?? type);
        return t;
    }

    /// <summary>Row versions come back from the procedures as hex text ("0x00000000000007D1").</summary>
    public static byte[]? RowVersion(string? hex)
    {
        if (string.IsNullOrWhiteSpace(hex)) return null;
        var s = hex.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? hex[2..] : hex;
        try { return Convert.FromHexString(s); }
        catch (FormatException) { throw AppException.BadRequest("The record version is invalid. Reload and try again."); }
    }

    public static object Value(object? v) => v ?? DBNull.Value;

    private sealed class DateOnlyHandler : SqlMapper.TypeHandler<DateOnly>
    {
        public override void SetValue(IDbDataParameter p, DateOnly value) { p.DbType = DbType.Date; p.Value = value.ToDateTime(TimeOnly.MinValue); }
        public override DateOnly Parse(object value) => value is DateTime d ? DateOnly.FromDateTime(d) : (DateOnly)value;
    }

    private sealed class NullableDateOnlyHandler : SqlMapper.TypeHandler<DateOnly?>
    {
        public override void SetValue(IDbDataParameter p, DateOnly? value)
        {
            p.DbType = DbType.Date;
            p.Value = value is DateOnly d ? d.ToDateTime(TimeOnly.MinValue) : DBNull.Value;
        }
        public override DateOnly? Parse(object value) => value is null or DBNull ? null : value is DateTime d ? DateOnly.FromDateTime(d) : (DateOnly)value;
    }
}
