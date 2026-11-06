using Construction.Infrastructure.Data;

namespace Construction.Api;

/// <summary>
/// dotnet run --project src/Construction.Api -- setup-company --regid 2172
/// Adds the starter floor level types and unit types for a company that exists in tbl_registrationdetails.
/// Does nothing for a company that already has them. (The same as EXEC dbo.cpm_usp_Company_Setup @RegId = 2172.)
/// </summary>
public static class Cli
{
    public static async Task<int> SetupCompanyAsync(string[] args, DbOptions dbOptions)
    {
        var i = Array.IndexOf(args, "--regid");
        if (i < 0 || i + 1 >= args.Length || !int.TryParse(args[i + 1], out var regId))
        {
            Console.Error.WriteLine("Usage: setup-company --regid <tbl_registrationdetails.id>");
            return 2;
        }
        var repo = new AuthRepository(new Db(dbOptions));
        var (levels, types) = await repo.SetupCompanyAsync(regId);
        Console.WriteLine(levels + types == 0
            ? $"Company {regId} already has its construction masters. Nothing changed."
            : $"Company {regId}: added {levels} floor level types and {types} unit types.");
        return 0;
    }
}
