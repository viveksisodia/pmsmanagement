using System.Data;
using Construction.Application.Abstractions;
using Construction.Application.Common;
using Construction.Application.Contracts;
using Construction.Domain.Structure;
using Dapper;

namespace Construction.Infrastructure.Data;

public sealed class AuthRepository(Db db) : IAuthRepository
{
    public async Task<SignInCheck> SignInAsync(string userName, string password, string? ip, int maxFailed, int lockoutMinutes) =>
        await db.QuerySingleAsync<SignInCheck>("dbo.cpm_usp_Auth_SignIn", new
        {
            UserName = userName, Password = password, IpAddress = ip, MaxFailed = maxFailed, LockoutMinutes = lockoutMinutes
        }) ?? new SignInCheck { Result = "Unknown" };

    public Task<SessionInfo?> GetSessionAsync(int userId) =>
        db.MultipleAsync<SessionInfo?>("dbo.cpm_usp_Auth_GetSession", new { UserId = userId }, async g =>
        {
            var row = await g.ReadSingleOrDefaultAsync<SessionRow>();
            var perms = (await g.ReadAsync<string>()).ToList();
            return row is null ? null : new SessionInfo(row.UserId, row.RegId ?? 0, row.UserName ?? "", row.FullName ?? row.UserName ?? "", row.Email ?? "",
                                                        row.TenantName ?? "", row.ProfileId, row.ProfileName ?? "", row.IsActive && row.RegId is not null, perms);
        });

    public Task CreateRefreshTokenAsync(int userId, byte[] tokenHash, DateTime expiresAtUtc) =>
        db.ExecuteAsync("dbo.cpm_usp_RefreshToken_Create", new { UserId = userId, TokenHash = tokenHash, ExpiresAtUtc = expiresAtUtc });

    public Task<int?> RotateRefreshTokenAsync(byte[] oldHash, byte[] newHash, DateTime newExpiresAtUtc) =>
        db.QuerySingleAsync<int?>("dbo.cpm_usp_RefreshToken_Rotate", new { OldHash = oldHash, NewHash = newHash, NewExpiresAtUtc = newExpiresAtUtc });

    public Task RevokeRefreshTokenAsync(byte[] tokenHash) => db.ExecuteAsync("dbo.cpm_usp_RefreshToken_Revoke", new { TokenHash = tokenHash });

    public async Task<(int LevelTypes, int UnitTypes)> SetupCompanyAsync(int regId)
    {
        var r = await db.QuerySingleAsync<SetupRow>("dbo.cpm_usp_Company_Setup", new { RegId = regId }) ?? new SetupRow();
        return (r.LevelTypesAdded, r.UnitTypesAdded);
    }

    private sealed class SessionRow
    {
        public int UserId { get; set; }
        public int? RegId { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? TenantName { get; set; }
        public int? ProfileId { get; set; }
        public string? ProfileName { get; set; }
        public bool IsActive { get; set; }
    }
    private sealed class SetupRow { public int LevelTypesAdded { get; set; } public int UnitTypesAdded { get; set; } }
}

public sealed class PeopleRepository(Db db) : IPeopleRepository
{
    public async Task<IReadOnlyList<PersonItem>> ListAsync(int regId, int userId, string? search) =>
        (await db.QueryAsync<PersonItem>("dbo.cpm_usp_People_List", new { RegId = regId, UserId = userId, Search = search })).ToList();

    public Task UnlockAsync(int regId, int userId, int id) => db.ExecuteAsync("dbo.cpm_usp_People_Unlock", new { RegId = regId, UserId = userId, Id = id });

    public async Task<IReadOnlyList<UserLookup>> LookupAsync(int regId) => (await db.QueryAsync<UserLookup>("dbo.cpm_usp_User_Lookup", new { RegId = regId })).ToList();
}

public sealed class ProfileRepository(Db db) : IProfileRepository
{
    public async Task<IReadOnlyList<PermissionItem>> PermissionsAsync() => (await db.QueryAsync<PermissionItem>("dbo.cpm_usp_Permission_List")).ToList();

    public async Task<IReadOnlyList<ProfileItem>> ListAsync(int regId, int userId) =>
        (await db.QueryAsync<ProfileItem>("dbo.cpm_usp_Profile_List", new { RegId = regId, UserId = userId })).ToList();

    public Task SavePermissionsAsync(int regId, int userId, int profileId, IReadOnlyList<string> codes) =>
        db.ExecuteAsync("dbo.cpm_usp_Profile_SavePermissions", new { RegId = regId, UserId = userId, ProfileId = profileId, Permissions = Db.CodeList(codes) });
}

public sealed class AuditRepository(Db db) : IAuditRepository
{
    public async Task<IReadOnlyList<AuditItem>> ListAsync(int regId, int userId, string entityType, long entityId) =>
        (await db.QueryAsync<AuditItem>("dbo.cpm_usp_Audit_List", new { RegId = regId, UserId = userId, EntityType = entityType, EntityId = entityId })).ToList();
}

public sealed class MasterRepository(Db db) : IMasterRepository
{
    public Task<LocationLists> LocationsAsync(int regId) =>
        db.MultipleAsync("dbo.cpm_usp_Location_List", new { RegId = regId }, async g => new LocationLists(
            (await g.ReadAsync<CountryItem>()).ToList(), (await g.ReadAsync<StateItem>()).ToList(), (await g.ReadAsync<CityItem>()).ToList()));

    public async Task<IReadOnlyList<UnitTypeItem>> UnitTypesAsync(int regId, bool includeInactive) =>
        (await db.QueryAsync<UnitTypeItem>("dbo.cpm_usp_UnitType_List", new { RegId = regId, IncludeInactive = includeInactive })).ToList();

    public Task<int> SaveUnitTypeAsync(int regId, int userId, int? id, UnitTypeSaveRequest r) =>
        db.QuerySingleAsync<int>("dbo.cpm_usp_UnitType_Save", new
        {
            RegId = regId, UserId = userId, Id = id, r.Code, r.Name, r.Category,
            Bedrooms = (byte?)r.Bedrooms, Bathrooms = (byte?)r.Bathrooms, r.DefaultCarpetSqm, r.DefaultBuiltUpSqm, r.DefaultSuperSqm, r.DefaultPlotSqm,
            r.IsSaleable, r.IsActive, RowVersion = Db.RowVersion(r.RowVersion)
        });

    public async Task<IReadOnlyList<LevelTypeItem>> LevelTypesAsync(int regId) =>
        (await db.QueryAsync<LevelTypeItem>("dbo.cpm_usp_LevelType_List", new { RegId = regId })).ToList();

    public Task<int> SaveLevelTypeAsync(int regId, int userId, int? id, LevelTypeSaveRequest r) =>
        db.QuerySingleAsync<int>("dbo.cpm_usp_LevelType_Save", new
        {
            RegId = regId, UserId = userId, Id = id, r.Code, r.Name, r.IsHabitable, r.SortOrder, r.IsActive, RowVersion = Db.RowVersion(r.RowVersion)
        });
}

public sealed class LandRepository(Db db) : ILandRepository
{
    public async Task<IReadOnlyList<LandParcelListItem>> ListAsync(int regId, int userId, string? search) =>
        (await db.QueryAsync<LandParcelListItem>("dbo.cpm_usp_LandParcel_List", new { RegId = regId, UserId = userId, Search = search })).ToList();

    public Task<LandParcelDetail?> GetAsync(int regId, int userId, int id) =>
        db.MultipleAsync("dbo.cpm_usp_LandParcel_Get", new { RegId = regId, UserId = userId, Id = id }, async g =>
        {
            var p = await g.ReadSingleOrDefaultAsync<LandParcelDetail>();
            var owners = (await g.ReadAsync<LandOwner>()).ToList();
            return p is null ? null : p with { Owners = owners };
        });

    public Task<int> SaveAsync(int regId, int userId, int? id, LandParcelSaveRequest r, decimal areaSqm)
    {
        var owners = Db.NewTable(("OwnerName", typeof(string)), ("SharePct", typeof(decimal)));
        foreach (var o in r.Owners ?? []) owners.Rows.Add(o.OwnerName.Trim(), o.SharePct);
        return db.QuerySingleAsync<int>("dbo.cpm_usp_LandParcel_Save", new
        {
            RegId = regId, UserId = userId, Id = id, r.SurveyNo, r.Locality, r.CityId, r.StateId, AreaSqm = areaSqm, r.AreaEntered, r.AreaUnitEntered,
            r.Tenure, r.RegistrationNo, r.RegistrationDate, r.RegistryOffice, r.LandUse, r.Encumbrance, r.Latitude, r.Longitude, r.ResponsibleUserId,
            r.Remarks, Owners = owners.AsTableValuedParameter("dbo.cpm_tt_Owner"), RowVersion = Db.RowVersion(r.RowVersion)
        });
    }

    public Task DeleteAsync(int regId, int userId, int id, string reason) =>
        db.ExecuteAsync("dbo.cpm_usp_LandParcel_Delete", new { RegId = regId, UserId = userId, Id = id, Reason = reason });
}

public sealed class ProjectRepository(Db db) : IProjectRepository
{
    public async Task<IReadOnlyList<ProjectListItem>> ListAsync(int regId, int userId) =>
        (await db.QueryAsync<ProjectListItem>("dbo.cpm_usp_Project_List", new { RegId = regId, UserId = userId })).ToList();

    public Task<ProjectDetail?> GetAsync(int regId, int userId, int id) =>
        db.MultipleAsync("dbo.cpm_usp_Project_Get", new { RegId = regId, UserId = userId, Id = id }, async g =>
        {
            var p = await g.ReadSingleOrDefaultAsync<ProjectDetail>();
            var land = (await g.ReadAsync<ProjectLand>()).ToList();
            var members = (await g.ReadAsync<MemberRow>()).Select(m => new UserLookup { Id = m.UserId, FullName = m.FullName, Email = m.Email }).ToList();
            return p is null ? null : p with { Land = land, Members = members };
        });

    public Task<int> SaveAsync(int regId, int userId, int? id, ProjectSaveRequest r)
    {
        var land = Db.NewTable(("LandParcelId", typeof(int)), ("AreaUsedSqm", typeof(decimal)));
        foreach (var l in (r.Land ?? []).GroupBy(x => x.LandParcelId)) land.Rows.Add(l.Key, l.Sum(x => x.AreaUsedSqm));
        return db.QuerySingleAsync<int>("dbo.cpm_usp_Project_Save", new
        {
            RegId = regId, UserId = userId, Id = id, r.Code, r.Name, r.DeveloperEntity, r.ProjectType, r.UsesPhases, r.AreaUnit, r.TimeZoneId,
            r.CurrencyCode, r.Address, r.CityId, r.StateId, r.StartDate, r.TargetDate, r.UnitNumberPattern, r.SkipFloorNumbers, r.Status,
            Land = land.AsTableValuedParameter("dbo.cpm_tt_ProjectLand"), MemberIds = Db.IntList(r.MemberIds ?? []), RowVersion = Db.RowVersion(r.RowVersion)
        });
    }

    public Task DeleteAsync(int regId, int userId, int id, string reason) =>
        db.ExecuteAsync("dbo.cpm_usp_Project_Delete", new { RegId = regId, UserId = userId, Id = id, Reason = reason });

    private sealed class MemberRow { public int UserId { get; set; } public string FullName { get; set; } = ""; public string Email { get; set; } = ""; }
}

public sealed class StructureRepository(Db db) : IStructureRepository
{
    public Task<StructureTree> GetTreeAsync(int regId, int userId, int projectId) =>
        db.MultipleAsync("dbo.cpm_usp_Structure_Get", new { RegId = regId, UserId = userId, ProjectId = projectId }, async g =>
            new StructureTree((await g.ReadAsync<PhaseItem>()).ToList(), (await g.ReadAsync<BuildingItem>()).ToList()));

    public Task<int> SavePhaseAsync(int regId, int userId, int projectId, int? id, PhaseSaveRequest r) =>
        db.QuerySingleAsync<int>("dbo.cpm_usp_Phase_Save", new
        {
            RegId = regId, UserId = userId, ProjectId = projectId, Id = id, r.Code, r.Name, r.RegistrationNo, r.PlannedStart, r.PlannedFinish,
            r.RegisteredCompletion, r.SortOrder, RowVersion = Db.RowVersion(r.RowVersion)
        });

    public Task DeletePhaseAsync(int regId, int userId, int projectId, int id, string reason) =>
        db.ExecuteAsync("dbo.cpm_usp_Phase_Delete", new { RegId = regId, UserId = userId, ProjectId = projectId, Id = id, Reason = reason });

    public Task<int> SaveBuildingAsync(int regId, int userId, int projectId, int? id, BuildingSaveRequest r) =>
        db.QuerySingleAsync<int>("dbo.cpm_usp_Building_Save", new
        {
            RegId = regId, UserId = userId, ProjectId = projectId, Id = id, r.PhaseId, r.Code, r.Name, r.BuildingType,
            Lifts = (byte?)r.Lifts, Staircases = (byte?)r.Staircases, r.StructureType, r.PlannedStart, r.PlannedFinish, r.UnitNumberPattern, r.SortOrder,
            RowVersion = Db.RowVersion(r.RowVersion)
        });

    public Task DeleteBuildingAsync(int regId, int userId, int projectId, int id, string reason) =>
        db.ExecuteAsync("dbo.cpm_usp_Building_Delete", new { RegId = regId, UserId = userId, ProjectId = projectId, Id = id, Reason = reason });

    public async Task<IReadOnlyList<FloorItem>> FloorsAsync(int regId, int userId, int projectId, int buildingId) =>
        (await db.QueryAsync<FloorItem>("dbo.cpm_usp_Floor_List", new { RegId = regId, UserId = userId, ProjectId = projectId, BuildingId = buildingId })).ToList();

    public async Task<GenerateResult> GenerateFloorsAsync(int regId, int userId, int projectId, int buildingId, IReadOnlyList<PlannedFloor> floors)
    {
        var t = Db.NewTable(("Code", typeof(string)), ("Name", typeof(string)), ("LevelTypeId", typeof(int)), ("SortOrder", typeof(int)), ("HeightM", typeof(decimal)));
        foreach (var f in floors) t.Rows.Add(f.Code, f.Name, f.LevelTypeId, f.SortOrder, Db.Value(f.HeightM));
        return await db.QuerySingleAsync<GenerateResult>("dbo.cpm_usp_Floor_Generate", new
        {
            RegId = regId, UserId = userId, ProjectId = projectId, BuildingId = buildingId, Floors = t.AsTableValuedParameter("dbo.cpm_tt_FloorGen")
        }) ?? new GenerateResult();
    }

    public Task<int> SaveFloorAsync(int regId, int userId, int projectId, int id, FloorSaveRequest r) =>
        db.QuerySingleAsync<int>("dbo.cpm_usp_Floor_Save", new
        {
            RegId = regId, UserId = userId, ProjectId = projectId, Id = id, r.Code, r.Name, r.LevelTypeId, r.SortOrder, r.HeightM,
            RowVersion = Db.RowVersion(r.RowVersion)
        });

    public Task DeleteFloorAsync(int regId, int userId, int projectId, int id, string reason) =>
        db.ExecuteAsync("dbo.cpm_usp_Floor_Delete", new { RegId = regId, UserId = userId, ProjectId = projectId, Id = id, Reason = reason });
}

public sealed class UnitRepository(Db db) : IUnitRepository
{
    public Task<PagedResult<UnitListItem>> ListAsync(int regId, int userId, int projectId, UnitQuery q) =>
        db.MultipleAsync("dbo.cpm_usp_Unit_List", new
        {
            RegId = regId, UserId = userId, ProjectId = projectId, q.BuildingId, q.FloorId, q.UnitTypeId, q.Availability, q.Search,
            Page = Math.Max(1, q.Page), PageSize = Math.Clamp(q.PageSize, 1, 500)
        }, async g => new PagedResult<UnitListItem>(await g.ReadSingleAsync<int>(), (await g.ReadAsync<UnitListItem>()).ToList()));

    public Task<StackingPlan> StackAsync(int regId, int userId, int projectId, int buildingId) =>
        db.MultipleAsync("dbo.cpm_usp_Unit_Stack", new { RegId = regId, UserId = userId, ProjectId = projectId, BuildingId = buildingId }, async g =>
            new StackingPlan((await g.ReadAsync<StackFloor>()).ToList(), (await g.ReadAsync<StackUnit>()).ToList()));

    public async Task<IReadOnlySet<string>> ExistingNumbersAsync(int regId, int userId, int projectId) =>
        (await db.QueryAsync<string>("dbo.cpm_usp_Unit_ExistingNumbers", new { RegId = regId, UserId = userId, ProjectId = projectId }))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

    public async Task<GenerateResult> GenerateAsync(int regId, int userId, int projectId, int buildingId, IReadOnlyList<PlannedUnit> units)
    {
        var t = Db.NewTable(("UnitNo", typeof(string)), ("FloorId", typeof(int)), ("Position", typeof(short)), ("UnitTypeId", typeof(int)),
            ("Facing", typeof(string)), ("CarpetSqm", typeof(decimal)), ("BuiltUpSqm", typeof(decimal)), ("SuperSqm", typeof(decimal)),
            ("BalconySqm", typeof(decimal)), ("PlotSqm", typeof(decimal)));
        foreach (var u in units)
            t.Rows.Add(u.UnitNo, Db.Value(u.FloorId), (short)u.Position, u.UnitTypeId, Db.Value(u.Facing), Db.Value(u.CarpetSqm), Db.Value(u.BuiltUpSqm),
                       Db.Value(u.SuperSqm), Db.Value(u.BalconySqm), Db.Value(u.PlotSqm));
        return await db.QuerySingleAsync<GenerateResult>("dbo.cpm_usp_Unit_Generate", new
        {
            RegId = regId, UserId = userId, ProjectId = projectId, BuildingId = buildingId, Units = t.AsTableValuedParameter("dbo.cpm_tt_UnitGen")
        }) ?? new GenerateResult();
    }

    public Task<UnitDetail?> GetAsync(int regId, int userId, int projectId, int id) =>
        db.MultipleAsync("dbo.cpm_usp_Unit_Get", new { RegId = regId, UserId = userId, ProjectId = projectId, Id = id }, async g =>
        {
            var u = await g.ReadSingleOrDefaultAsync<UnitDetail>();
            var history = (await g.ReadAsync<AvailabilityHistoryItem>()).ToList();
            var transitions = (await g.ReadAsync<TransitionRow>()).ToList();
            return u is null ? null : u with
            {
                History = history,
                AllowedNext = transitions.Where(t => t.FromStatus == u.Availability).Select(t => t.ToStatus).ToList()
            };
        });

    public Task<int> SaveAsync(int regId, int userId, int projectId, int id, UnitSaveRequest r) =>
        db.QuerySingleAsync<int>("dbo.cpm_usp_Unit_Save", new
        {
            RegId = regId, UserId = userId, ProjectId = projectId, Id = id, r.UnitNo, r.UnitTypeId, r.Facing, r.CarpetSqm, r.BuiltUpSqm, r.SuperSqm,
            r.BalconySqm, r.PlotSqm, r.Reason, RowVersion = Db.RowVersion(r.RowVersion)
        });

    public Task ChangeAvailabilityAsync(int regId, int userId, int projectId, int id, AvailabilityChangeRequest r) =>
        db.ExecuteAsync("dbo.cpm_usp_Unit_ChangeAvailability", new
        {
            RegId = regId, UserId = userId, ProjectId = projectId, Id = id, r.ToStatus, r.Reason, RowVersion = Db.RowVersion(r.RowVersion)
        });

    public Task<int> DeleteAsync(int regId, int userId, int projectId, UnitDeleteRequest r) =>
        db.QuerySingleAsync<int>("dbo.cpm_usp_Unit_Delete", new { RegId = regId, UserId = userId, ProjectId = projectId, Ids = Db.IntList(r.Ids ?? []), r.Reason });

    private sealed class TransitionRow { public string FromStatus { get; set; } = ""; public string ToStatus { get; set; } = ""; }
}
