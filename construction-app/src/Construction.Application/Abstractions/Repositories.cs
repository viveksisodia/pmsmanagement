using Construction.Application.Common;
using Construction.Application.Contracts;
using Construction.Domain.Structure;

namespace Construction.Application.Abstractions;

/* Every repository calls the cpm_usp_* stored procedures. Methods take regId and userId first,
   matching the procedures, which check permissions and project access themselves. */

/// <summary>Result of checking a username and password against tbl_employee (Ok, Unknown, WrongPassword, Ambiguous, Locked, ErpLocked).</summary>
public sealed record SignInCheck
{
    public string Result { get; init; } = "";
    public int? UserId { get; init; }
    public DateTime? LockoutEndUtc { get; init; }
}
public sealed record SessionInfo(int UserId, int RegId, string UserName, string FullName, string Email, string TenantName, int? ProfileId,
                                 string ProfileName, bool IsActive, IReadOnlyList<string> Permissions);

public interface IAuthRepository
{
    Task<SignInCheck> SignInAsync(string userName, string password, string? ip, int maxFailed, int lockoutMinutes);
    Task<SessionInfo?> GetSessionAsync(int userId);
    Task CreateRefreshTokenAsync(int userId, byte[] tokenHash, DateTime expiresAtUtc);
    Task<int?> RotateRefreshTokenAsync(byte[] oldHash, byte[] newHash, DateTime newExpiresAtUtc);
    Task RevokeRefreshTokenAsync(byte[] tokenHash);
    Task<(int LevelTypes, int UnitTypes)> SetupCompanyAsync(int regId);
}

/// <summary>People are tbl_employee rows; this app only reads them.</summary>
public interface IPeopleRepository
{
    Task<IReadOnlyList<PersonItem>> ListAsync(int regId, int userId, string? search);
    Task UnlockAsync(int regId, int userId, int id);
    Task<IReadOnlyList<UserLookup>> LookupAsync(int regId);
}

/// <summary>Construction permissions per Manage_Profile.</summary>
public interface IProfileRepository
{
    Task<IReadOnlyList<PermissionItem>> PermissionsAsync();
    Task<IReadOnlyList<ProfileItem>> ListAsync(int regId, int userId);
    Task SavePermissionsAsync(int regId, int userId, int profileId, IReadOnlyList<string> codes);
}

public interface IAuditRepository
{
    Task<IReadOnlyList<AuditItem>> ListAsync(int regId, int userId, string entityType, long entityId);
}

public interface IMasterRepository
{
    Task<LocationLists> LocationsAsync(int regId);
    Task<IReadOnlyList<UnitTypeItem>> UnitTypesAsync(int regId, bool includeInactive);
    Task<int> SaveUnitTypeAsync(int regId, int userId, int? id, UnitTypeSaveRequest r);
    Task<IReadOnlyList<LevelTypeItem>> LevelTypesAsync(int regId);
    Task<int> SaveLevelTypeAsync(int regId, int userId, int? id, LevelTypeSaveRequest r);
}

public interface ILandRepository
{
    Task<IReadOnlyList<LandParcelListItem>> ListAsync(int regId, int userId, string? search);
    Task<LandParcelDetail?> GetAsync(int regId, int userId, int id);
    Task<int> SaveAsync(int regId, int userId, int? id, LandParcelSaveRequest r, decimal areaSqm);
    Task DeleteAsync(int regId, int userId, int id, string reason);
}

public interface IProjectRepository
{
    Task<IReadOnlyList<ProjectListItem>> ListAsync(int regId, int userId);
    Task<ProjectDetail?> GetAsync(int regId, int userId, int id);
    Task<int> SaveAsync(int regId, int userId, int? id, ProjectSaveRequest r);
    Task DeleteAsync(int regId, int userId, int id, string reason);
}

public interface IStructureRepository
{
    Task<StructureTree> GetTreeAsync(int regId, int userId, int projectId);
    Task<int> SavePhaseAsync(int regId, int userId, int projectId, int? id, PhaseSaveRequest r);
    Task DeletePhaseAsync(int regId, int userId, int projectId, int id, string reason);
    Task<int> SaveBuildingAsync(int regId, int userId, int projectId, int? id, BuildingSaveRequest r);
    Task DeleteBuildingAsync(int regId, int userId, int projectId, int id, string reason);
    Task<IReadOnlyList<FloorItem>> FloorsAsync(int regId, int userId, int projectId, int buildingId);
    Task<GenerateResult> GenerateFloorsAsync(int regId, int userId, int projectId, int buildingId, IReadOnlyList<PlannedFloor> floors);
    Task<int> SaveFloorAsync(int regId, int userId, int projectId, int id, FloorSaveRequest r);
    Task DeleteFloorAsync(int regId, int userId, int projectId, int id, string reason);
}

public interface IUnitRepository
{
    Task<PagedResult<UnitListItem>> ListAsync(int regId, int userId, int projectId, UnitQuery q);
    Task<StackingPlan> StackAsync(int regId, int userId, int projectId, int buildingId);
    Task<IReadOnlySet<string>> ExistingNumbersAsync(int regId, int userId, int projectId);
    Task<GenerateResult> GenerateAsync(int regId, int userId, int projectId, int buildingId, IReadOnlyList<PlannedUnit> units);
    Task<UnitDetail?> GetAsync(int regId, int userId, int projectId, int id);
    Task<int> SaveAsync(int regId, int userId, int projectId, int id, UnitSaveRequest r);
    Task ChangeAvailabilityAsync(int regId, int userId, int projectId, int id, AvailabilityChangeRequest r);
    Task<int> DeleteAsync(int regId, int userId, int projectId, UnitDeleteRequest r);
}
