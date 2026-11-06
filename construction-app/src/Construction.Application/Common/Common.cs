namespace Construction.Application.Common;

/// <summary>The signed-in user. RegId always comes from the session, never from a request.</summary>
public interface ICurrentUser
{
    int UserId { get; }
    int RegId { get; }
    bool HasPermission(string code);
}

/// <summary>An error the user should see, with the HTTP status the API returns.</summary>
public sealed class AppException(int status, string message) : Exception(message)
{
    public int Status { get; } = status;
    public static AppException BadRequest(string message) => new(400, message);
    public static AppException NotFound(string message = "Not found.") => new(404, message);
}

public sealed record PagedResult<T>(int Total, IReadOnlyList<T> Items);

public sealed record IdResult(int Id);

/// <summary>Permission codes; the same list is seeded in table cpm_Permission.</summary>
public static class Permissions
{
    public const string AdminRoles = "admin.roles";
    public const string AuditView = "audit.view";
    public const string MastersEdit = "masters.edit";
    public const string LandView = "land.view";
    public const string LandEdit = "land.edit";
    public const string ProjectsView = "projects.view";
    public const string ProjectsAll = "projects.all";
    public const string ProjectsEdit = "projects.edit";
    public const string StructureEdit = "structure.edit";
    public const string UnitsGenerate = "units.generate";
    public const string UnitsEdit = "units.edit";
    public const string AvailabilityChange = "availability.change";

    public static readonly IReadOnlyList<string> All =
    [
        AdminRoles, AuditView, MastersEdit, LandView, LandEdit, ProjectsView, ProjectsAll, ProjectsEdit,
        StructureEdit, UnitsGenerate, UnitsEdit, AvailabilityChange
    ];
}
