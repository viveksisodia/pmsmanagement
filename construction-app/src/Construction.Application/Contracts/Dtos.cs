namespace Construction.Application.Contracts;

// Responses are property records (filled by Dapper from stored procedure results).
// Requests are positional records (filled from JSON).
// RowVersion travels as hex text ("0x…") so the browser can send it back unchanged.

/* ---------- auth and admin ---------- */
public sealed record SignInRequest(string UserName, string Password);
public sealed record SessionUser(int UserId, int RegId, string UserName, string FullName, string Email, string TenantName, string ProfileName, IReadOnlyList<string> Permissions);
public sealed record SignInResult(string AccessToken, DateTime ExpiresAtUtc, SessionUser User);

/// <summary>A person from tbl_employee (read-only here; people are managed in the ERP).</summary>
public sealed record PersonItem
{
    public int Id { get; init; }
    public string FullName { get; init; } = "";
    public string UserName { get; init; } = "";
    public string Email { get; init; } = "";
    public int? ProfileId { get; init; }
    public string ProfileName { get; init; } = "";
    public DateTime? LastSignInUtc { get; init; }
    public DateTime? LockoutEndUtc { get; init; }
}
public sealed record UserLookup
{
    public int Id { get; init; }
    public string FullName { get; init; } = "";
    public string Email { get; init; } = "";
}

public sealed record PermissionItem
{
    public string Code { get; init; } = "";
    public string Module { get; init; } = "";
    public string Name { get; init; } = "";
}
/// <summary>A Manage_Profile row with the construction permissions it has in this company.</summary>
public sealed record ProfileItem
{
    public int Id { get; init; }
    public string Name { get; init; } = "";
    public bool IsAdmin { get; init; }
    public int People { get; init; }
    public string? Permissions { get; init; }
    public IReadOnlyList<string> PermissionList => string.IsNullOrEmpty(Permissions) ? [] : Permissions.Split(',');
}
public sealed record ProfilePermissionsRequest(IReadOnlyList<string> Permissions);

public sealed record AuditItem
{
    public long Id { get; init; }
    public string Action { get; init; } = "";
    public string? Reason { get; init; }
    public string? OldData { get; init; }
    public string? NewData { get; init; }
    public DateTime AtUtc { get; init; }
    public string? UserName { get; init; }
}

/* ---------- masters ---------- */
public sealed record CountryItem
{
    public int Id { get; init; }
    public string Name { get; init; } = "";
}
public sealed record StateItem
{
    public int Id { get; init; }
    public int? CountryId { get; init; }
    public string Name { get; init; } = "";
}
public sealed record CityItem
{
    public int Id { get; init; }
    public string Name { get; init; } = "";
}
public sealed record LocationLists(IReadOnlyList<CountryItem> Countries, IReadOnlyList<StateItem> States, IReadOnlyList<CityItem> Cities);

public sealed record UnitTypeItem
{
    public int Id { get; init; }
    public string Code { get; init; } = "";
    public string Name { get; init; } = "";
    public string Category { get; init; } = "";
    public int? Bedrooms { get; init; }
    public int? Bathrooms { get; init; }
    public decimal? DefaultCarpetSqm { get; init; }
    public decimal? DefaultBuiltUpSqm { get; init; }
    public decimal? DefaultSuperSqm { get; init; }
    public decimal? DefaultPlotSqm { get; init; }
    public bool IsSaleable { get; init; }
    public bool IsActive { get; init; }
    public string RowVersion { get; init; } = "";
    public int UnitsUsing { get; init; }
}
public sealed record UnitTypeSaveRequest(string Code, string Name, string Category, int? Bedrooms, int? Bathrooms, decimal? DefaultCarpetSqm,
    decimal? DefaultBuiltUpSqm, decimal? DefaultSuperSqm, decimal? DefaultPlotSqm, bool IsSaleable, bool IsActive, string? RowVersion);

public sealed record LevelTypeItem
{
    public int Id { get; init; }
    public string Code { get; init; } = "";
    public string Name { get; init; } = "";
    public bool IsHabitable { get; init; }
    public int SortOrder { get; init; }
    public bool IsActive { get; init; }
    public string RowVersion { get; init; } = "";
    public int FloorsUsing { get; init; }
}
public sealed record LevelTypeSaveRequest(string Code, string Name, bool IsHabitable, int SortOrder, bool IsActive, string? RowVersion);

/* ---------- land ---------- */
public sealed record LandOwner
{
    public string OwnerName { get; init; } = "";
    public decimal SharePct { get; init; }
}
public sealed record LandParcelListItem
{
    public int Id { get; init; }
    public string SurveyNo { get; init; } = "";
    public string? Locality { get; init; }
    public string? City { get; init; }
    public string State { get; init; } = "";
    public decimal AreaSqm { get; init; }
    public decimal AreaEntered { get; init; }
    public string AreaUnitEntered { get; init; } = "";
    public string Tenure { get; init; } = "";
    public string? RegistrationNo { get; init; }
    public DateOnly? RegistrationDate { get; init; }
    public string Encumbrance { get; init; } = "";
    public string? Responsible { get; init; }
    public decimal AreaUsedSqm { get; init; }
    public string? Projects { get; init; }
}
public sealed record LandParcelDetail
{
    public int Id { get; init; }
    public string SurveyNo { get; init; } = "";
    public string? Locality { get; init; }
    public int? CityId { get; init; }
    public int StateId { get; init; }
    public int CountryId { get; init; }
    public decimal AreaSqm { get; init; }
    public decimal AreaEntered { get; init; }
    public string AreaUnitEntered { get; init; } = "";
    public string Tenure { get; init; } = "";
    public string? RegistrationNo { get; init; }
    public DateOnly? RegistrationDate { get; init; }
    public string? RegistryOffice { get; init; }
    public string? LandUse { get; init; }
    public string Encumbrance { get; init; } = "";
    public decimal? Latitude { get; init; }
    public decimal? Longitude { get; init; }
    public int? ResponsibleUserId { get; init; }
    public string? Remarks { get; init; }
    public string RowVersion { get; init; } = "";
    public IReadOnlyList<LandOwner> Owners { get; init; } = [];
}
public sealed record LandParcelSaveRequest(string SurveyNo, string? Locality, int? CityId, int StateId, decimal AreaEntered, string AreaUnitEntered,
    string Tenure, string? RegistrationNo, DateOnly? RegistrationDate, string? RegistryOffice, string? LandUse, string Encumbrance,
    decimal? Latitude, decimal? Longitude, int? ResponsibleUserId, string? Remarks, IReadOnlyList<LandOwner> Owners, string? RowVersion);
public sealed record DeleteRequest(string Reason);

/* ---------- projects ---------- */
public sealed record ProjectListItem
{
    public int Id { get; init; }
    public string Code { get; init; } = "";
    public string Name { get; init; } = "";
    public string DeveloperEntity { get; init; } = "";
    public string ProjectType { get; init; } = "";
    public string Status { get; init; } = "";
    public string? City { get; init; }
    public DateOnly? StartDate { get; init; }
    public DateOnly? TargetDate { get; init; }
    public int Buildings { get; init; }
    public int Units { get; init; }
    public decimal LandSqm { get; init; }
}
public sealed record ProjectLand
{
    public int LandParcelId { get; init; }
    public string SurveyNo { get; init; } = "";
    public decimal ParcelAreaSqm { get; init; }
    public decimal AreaUsedSqm { get; init; }
    public decimal AvailableSqm { get; init; }
}
public sealed record ProjectLandInput(int LandParcelId, decimal AreaUsedSqm);
public sealed record ProjectDetail
{
    public int Id { get; init; }
    public string Code { get; init; } = "";
    public string Name { get; init; } = "";
    public string DeveloperEntity { get; init; } = "";
    public string ProjectType { get; init; } = "";
    public bool UsesPhases { get; init; }
    public string AreaUnit { get; init; } = "";
    public string TimeZoneId { get; init; } = "";
    public string CurrencyCode { get; init; } = "";
    public string? Address { get; init; }
    public int? CityId { get; init; }
    public int? StateId { get; init; }
    public int? CountryId { get; init; }
    public DateOnly? StartDate { get; init; }
    public DateOnly? TargetDate { get; init; }
    public string UnitNumberPattern { get; init; } = "";
    public string? SkipFloorNumbers { get; init; }
    public string Status { get; init; } = "";
    public string RowVersion { get; init; } = "";
    public IReadOnlyList<ProjectLand> Land { get; init; } = [];
    public IReadOnlyList<UserLookup> Members { get; init; } = [];
}
public sealed record ProjectSaveRequest(string Code, string Name, string DeveloperEntity, string ProjectType, bool UsesPhases, string AreaUnit,
    string TimeZoneId, string CurrencyCode, string? Address, int? CityId, int? StateId, DateOnly? StartDate, DateOnly? TargetDate,
    string UnitNumberPattern, string? SkipFloorNumbers, string Status, IReadOnlyList<ProjectLandInput> Land, IReadOnlyList<int> MemberIds, string? RowVersion);

/* ---------- structure ---------- */
public sealed record PhaseItem
{
    public int Id { get; init; }
    public string Code { get; init; } = "";
    public string Name { get; init; } = "";
    public string? RegistrationNo { get; init; }
    public DateOnly? PlannedStart { get; init; }
    public DateOnly? PlannedFinish { get; init; }
    public DateOnly? RegisteredCompletion { get; init; }
    public int SortOrder { get; init; }
    public string RowVersion { get; init; } = "";
    public int Units { get; init; }
}
public sealed record PhaseSaveRequest(string Code, string Name, string? RegistrationNo, DateOnly? PlannedStart, DateOnly? PlannedFinish,
    DateOnly? RegisteredCompletion, int SortOrder, string? RowVersion);
public sealed record BuildingItem
{
    public int Id { get; init; }
    public int? PhaseId { get; init; }
    public string Code { get; init; } = "";
    public string Name { get; init; } = "";
    public string BuildingType { get; init; } = "";
    public int? Lifts { get; init; }
    public int? Staircases { get; init; }
    public string? StructureType { get; init; }
    public DateOnly? PlannedStart { get; init; }
    public DateOnly? PlannedFinish { get; init; }
    public string? UnitNumberPattern { get; init; }
    public int SortOrder { get; init; }
    public string RowVersion { get; init; } = "";
    public int Floors { get; init; }
    public int Units { get; init; }
}
public sealed record BuildingSaveRequest(int? PhaseId, string Code, string Name, string BuildingType, int? Lifts, int? Staircases, string? StructureType,
    DateOnly? PlannedStart, DateOnly? PlannedFinish, string? UnitNumberPattern, int SortOrder, string? RowVersion);
public sealed record StructureTree(IReadOnlyList<PhaseItem> Phases, IReadOnlyList<BuildingItem> Buildings);

public sealed record FloorItem
{
    public int Id { get; init; }
    public string Code { get; init; } = "";
    public string Name { get; init; } = "";
    public int LevelTypeId { get; init; }
    public string LevelType { get; init; } = "";
    public bool IsHabitable { get; init; }
    public int SortOrder { get; init; }
    public decimal? HeightM { get; init; }
    public string RowVersion { get; init; } = "";
    public int Units { get; init; }
}
public sealed record FloorSaveRequest(string Code, string Name, int LevelTypeId, int SortOrder, decimal? HeightM, string RowVersion);
public sealed record FloorGenerateRequest(int Basements, int BasementLevelTypeId, int? GroundLevelTypeId, int Podiums, int PodiumLevelTypeId,
    int TypicalFrom, int TypicalTo, int TypicalLevelTypeId, string? SkipNumbers, int? TerraceLevelTypeId, decimal? HeightM);
public sealed record FloorPreviewItem(string Code, string Name, int LevelTypeId, int SortOrder, decimal? HeightM, bool AlreadyExists);
public sealed record GenerateResult
{
    public int Created { get; init; }
    public int Skipped { get; init; }
}

/* ---------- units ---------- */
public sealed record UnitListItem
{
    public int Id { get; init; }
    public string UnitNo { get; init; } = "";
    public int? PhaseId { get; init; }
    public string? Phase { get; init; }
    public int? BuildingId { get; init; }
    public string? Building { get; init; }
    public int? FloorId { get; init; }
    public string? Floor { get; init; }
    public int? Position { get; init; }
    public int UnitTypeId { get; init; }
    public string UnitType { get; init; } = "";
    public string? Facing { get; init; }
    public decimal? CarpetSqm { get; init; }
    public decimal? BuiltUpSqm { get; init; }
    public decimal? SuperSqm { get; init; }
    public decimal? BalconySqm { get; init; }
    public decimal? PlotSqm { get; init; }
    public string Availability { get; init; } = "";
}
public sealed record UnitQuery(int? BuildingId, int? FloorId, int? UnitTypeId, string? Availability, string? Search, int Page = 1, int PageSize = 100);
public sealed record AvailabilityHistoryItem
{
    public long Id { get; init; }
    public string? FromStatus { get; init; }
    public string ToStatus { get; init; } = "";
    public string Reason { get; init; } = "";
    public DateTime ChangedAtUtc { get; init; }
    public string? ChangedBy { get; init; }
}
public sealed record UnitDetail
{
    public int Id { get; init; }
    public string UnitNo { get; init; } = "";
    public int? PhaseId { get; init; }
    public int? BuildingId { get; init; }
    public string? Building { get; init; }
    public int? FloorId { get; init; }
    public string? Floor { get; init; }
    public int? Position { get; init; }
    public int UnitTypeId { get; init; }
    public string? Facing { get; init; }
    public decimal? CarpetSqm { get; init; }
    public decimal? BuiltUpSqm { get; init; }
    public decimal? SuperSqm { get; init; }
    public decimal? BalconySqm { get; init; }
    public decimal? PlotSqm { get; init; }
    public string Availability { get; init; } = "";
    public string RowVersion { get; init; } = "";
    public IReadOnlyList<AvailabilityHistoryItem> History { get; init; } = [];
    public IReadOnlyList<string> AllowedNext { get; init; } = [];
}
public sealed record UnitSaveRequest(string UnitNo, int UnitTypeId, string? Facing, decimal? CarpetSqm, decimal? BuiltUpSqm, decimal? SuperSqm,
    decimal? BalconySqm, decimal? PlotSqm, string? Reason, string RowVersion);
public sealed record AvailabilityChangeRequest(string ToStatus, string Reason, string RowVersion);
public sealed record UnitDeleteRequest(IReadOnlyList<int> Ids, string Reason);

public sealed record UnitGenPosition(int Position, int UnitTypeId, string? Facing, decimal? CarpetSqm, decimal? BuiltUpSqm, decimal? SuperSqm,
    decimal? BalconySqm, decimal? PlotSqm);
public sealed record UnitGenerateRequest(int? FromFloorId, int? ToFloorId, IReadOnlyList<UnitGenPosition> Positions);
public sealed record UnitPreviewItem(string UnitNo, int? FloorId, string? FloorCode, int Position, int UnitTypeId, decimal? CarpetSqm, bool AlreadyExists);
public sealed record UnitPreview(string Pattern, int NewCount, int ExistingCount, IReadOnlyList<UnitPreviewItem> Units);

public sealed record StackFloor
{
    public int Id { get; init; }
    public string Code { get; init; } = "";
    public string Name { get; init; } = "";
    public bool IsHabitable { get; init; }
    public int SortOrder { get; init; }
}
public sealed record StackUnit
{
    public int Id { get; init; }
    public string UnitNo { get; init; } = "";
    public int? FloorId { get; init; }
    public int? Position { get; init; }
    public string UnitType { get; init; } = "";
    public decimal? CarpetSqm { get; init; }
    public string Availability { get; init; } = "";
}
public sealed record StackingPlan(IReadOnlyList<StackFloor> Floors, IReadOnlyList<StackUnit> Units);
