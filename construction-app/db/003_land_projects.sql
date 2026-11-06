/* =====================================================================
   Construction app · 003 land parcels, projects, project team
   Target: absolutebook_2172 only. New cpm_ objects only. Safe to run again.
   ===================================================================== */
SET NOCOUNT ON;
IF DB_NAME() <> N'absolutebook_2172' AND DB_NAME() NOT LIKE N'cpmtest[_]%'
BEGIN
    RAISERROR(N'Stopped: this script may only run in absolutebook_2172.', 16, 1);
    SET NOEXEC ON;
END
GO
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'cpm_SchemaVersion')
   OR NOT EXISTS (SELECT 1 FROM dbo.cpm_SchemaVersion WHERE ScriptName = N'002_masters')
BEGIN
    RAISERROR(N'Stopped: run 001 and 002 first.', 16, 1);
    SET NOEXEC ON;
END
GO

/* ---------- land ---------- */
IF OBJECT_ID(N'dbo.cpm_LandParcel', N'U') IS NULL
CREATE TABLE dbo.cpm_LandParcel(
    LandParcelId     int IDENTITY(1,1) NOT NULL CONSTRAINT PK_cpm_LandParcel PRIMARY KEY,
    RegId            int NOT NULL,
    SurveyNo         nvarchar(60)  NOT NULL,
    Locality         nvarchar(150) NULL,
    CityId           int NULL,                 -- tbl_City_master.City_ID
    StateId          int NOT NULL,             -- tbl_statemaster.id
    AreaSqm          decimal(16,4) NOT NULL CONSTRAINT CK_cpm_LandParcel_Area CHECK (AreaSqm > 0),
    AreaEntered      decimal(16,4) NOT NULL,
    AreaUnitEntered  nvarchar(10)  NOT NULL CONSTRAINT CK_cpm_LandParcel_Unit CHECK (AreaUnitEntered IN (N'sqm', N'sqft', N'acre', N'hectare')),
    Tenure           nvarchar(30)  NOT NULL CONSTRAINT CK_cpm_LandParcel_Tenure CHECK (Tenure IN (N'Owned', N'Lease', N'Joint development')),
    RegistrationNo   nvarchar(80)  NULL,
    RegistrationDate date NULL,
    RegistryOffice   nvarchar(150) NULL,
    LandUse          nvarchar(100) NULL,
    Encumbrance      nvarchar(20)  NOT NULL CONSTRAINT CK_cpm_LandParcel_Enc CHECK (Encumbrance IN (N'Clear', N'Charged', N'Under check')),
    Latitude         decimal(9,6) NULL,
    Longitude        decimal(9,6) NULL,
    ResponsibleUserId int NULL,                -- tbl_employee.id
    Remarks          nvarchar(1000) NULL,
    IsDeleted        bit NOT NULL CONSTRAINT DF_cpm_LandParcel_Del DEFAULT 0,
    CreatedBy int NULL, CreatedAtUtc datetime2(0) NOT NULL CONSTRAINT DF_cpm_LandParcel_At DEFAULT SYSUTCDATETIME(),
    UpdatedBy int NULL, UpdatedAtUtc datetime2(0) NULL,
    RowVersion rowversion NOT NULL);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_cpm_LandParcel_Survey')
    CREATE UNIQUE INDEX UX_cpm_LandParcel_Survey ON dbo.cpm_LandParcel(RegId, StateId, SurveyNo) WHERE IsDeleted = 0;
GO
IF OBJECT_ID(N'dbo.cpm_LandParcelOwner', N'U') IS NULL
CREATE TABLE dbo.cpm_LandParcelOwner(
    OwnerId      int IDENTITY(1,1) NOT NULL CONSTRAINT PK_cpm_LandParcelOwner PRIMARY KEY,
    LandParcelId int NOT NULL CONSTRAINT FK_cpm_LandOwner_Parcel REFERENCES dbo.cpm_LandParcel(LandParcelId),
    OwnerName    nvarchar(200) NOT NULL,
    SharePct     decimal(5,2) NOT NULL CONSTRAINT CK_cpm_LandOwner_Share CHECK (SharePct > 0 AND SharePct <= 100));
GO
IF TYPE_ID(N'dbo.cpm_tt_Owner') IS NULL
    CREATE TYPE dbo.cpm_tt_Owner AS TABLE(OwnerName nvarchar(200) NOT NULL, SharePct decimal(5,2) NOT NULL);
GO

/* ---------- projects ---------- */
IF OBJECT_ID(N'dbo.cpm_PropertyNode', N'U') IS NULL
CREATE TABLE dbo.cpm_PropertyNode(
    NodeId       bigint IDENTITY(1,1) NOT NULL CONSTRAINT PK_cpm_PropertyNode PRIMARY KEY,
    RegId        int NOT NULL,
    ProjectId    int NULL,
    NodeType     nvarchar(20) NOT NULL CONSTRAINT CK_cpm_Node_Type CHECK (NodeType IN (N'Project', N'Phase', N'Building', N'Floor', N'Unit')),
    ParentNodeId bigint NULL CONSTRAINT FK_cpm_Node_Parent REFERENCES dbo.cpm_PropertyNode(NodeId),
    IsDeleted    bit NOT NULL CONSTRAINT DF_cpm_Node_Del DEFAULT 0);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_cpm_PropertyNode_Parent')
    CREATE INDEX IX_cpm_PropertyNode_Parent ON dbo.cpm_PropertyNode(RegId, ParentNodeId);
GO
IF OBJECT_ID(N'dbo.cpm_Project', N'U') IS NULL
CREATE TABLE dbo.cpm_Project(
    ProjectId         int IDENTITY(1,1) NOT NULL CONSTRAINT PK_cpm_Project PRIMARY KEY,
    RegId             int NOT NULL,
    NodeId            bigint NOT NULL CONSTRAINT FK_cpm_Project_Node REFERENCES dbo.cpm_PropertyNode(NodeId),
    Code              nvarchar(30)  NOT NULL,
    Name              nvarchar(200) NOT NULL,
    DeveloperEntity   nvarchar(200) NOT NULL,
    ProjectType       nvarchar(20)  NOT NULL CONSTRAINT CK_cpm_Project_Type CHECK (ProjectType IN (N'Residential', N'Commercial', N'Mixed', N'Villas', N'Plots')),
    UsesPhases        bit NOT NULL CONSTRAINT DF_cpm_Project_Phases DEFAULT 1,
    AreaUnit          nvarchar(10)  NOT NULL CONSTRAINT CK_cpm_Project_AreaUnit CHECK (AreaUnit IN (N'sqm', N'sqft')),
    TimeZoneId        nvarchar(64)  NOT NULL,
    CurrencyCode      char(3)       NOT NULL,
    Address           nvarchar(300) NULL,
    CityId            int NULL,                -- tbl_City_master.City_ID
    StateId           int NULL,                -- tbl_statemaster.id
    StartDate         date NULL,
    TargetDate        date NULL,
    UnitNumberPattern nvarchar(40)  NOT NULL CONSTRAINT DF_cpm_Project_Pattern DEFAULT N'{T}-{FF}{NN}',
    SkipFloorNumbers  nvarchar(100) NULL,
    Status            nvarchar(20)  NOT NULL CONSTRAINT CK_cpm_Project_Status CHECK (Status IN (N'Planning', N'Active', N'On hold', N'Completed')),
    IsDeleted         bit NOT NULL CONSTRAINT DF_cpm_Project_Del DEFAULT 0,
    CreatedBy int NULL, CreatedAtUtc datetime2(0) NOT NULL CONSTRAINT DF_cpm_Project_At DEFAULT SYSUTCDATETIME(),
    UpdatedBy int NULL, UpdatedAtUtc datetime2(0) NULL,
    RowVersion rowversion NOT NULL,
    CONSTRAINT CK_cpm_Project_Dates CHECK (StartDate IS NULL OR TargetDate IS NULL OR StartDate <= TargetDate));
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_cpm_Project_Code')
    CREATE UNIQUE INDEX UX_cpm_Project_Code ON dbo.cpm_Project(RegId, Code) WHERE IsDeleted = 0;
GO
IF OBJECT_ID(N'dbo.cpm_ProjectLandParcel', N'U') IS NULL
CREATE TABLE dbo.cpm_ProjectLandParcel(
    ProjectId    int NOT NULL CONSTRAINT FK_cpm_ProjLand_Project REFERENCES dbo.cpm_Project(ProjectId),
    LandParcelId int NOT NULL CONSTRAINT FK_cpm_ProjLand_Parcel REFERENCES dbo.cpm_LandParcel(LandParcelId),
    AreaUsedSqm  decimal(16,4) NOT NULL CONSTRAINT CK_cpm_ProjLand_Area CHECK (AreaUsedSqm > 0),
    CONSTRAINT PK_cpm_ProjectLandParcel PRIMARY KEY(ProjectId, LandParcelId));
GO
IF OBJECT_ID(N'dbo.cpm_ProjectMember', N'U') IS NULL
CREATE TABLE dbo.cpm_ProjectMember(
    ProjectId    int NOT NULL CONSTRAINT FK_cpm_ProjMember_Project REFERENCES dbo.cpm_Project(ProjectId),
    UserId       int NOT NULL,                 -- tbl_employee.id
    CreatedBy    int NULL, CreatedAtUtc datetime2(0) NOT NULL CONSTRAINT DF_cpm_ProjMember_At DEFAULT SYSUTCDATETIME(),
    CONSTRAINT PK_cpm_ProjectMember PRIMARY KEY(ProjectId, UserId));
GO
IF TYPE_ID(N'dbo.cpm_tt_ProjectLand') IS NULL
    CREATE TYPE dbo.cpm_tt_ProjectLand AS TABLE(LandParcelId int NOT NULL PRIMARY KEY, AreaUsedSqm decimal(16,4) NOT NULL);
GO

/* Project access: "See every project" permission, or membership of the project. */
CREATE OR ALTER FUNCTION dbo.cpm_fn_CanAccessProject(@RegId int, @UserId int, @ProjectId int)
RETURNS bit
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM dbo.cpm_Project WHERE ProjectId = @ProjectId AND RegId = @RegId AND IsDeleted = 0) RETURN 0;
    IF dbo.cpm_fn_HasPermission(@UserId, N'projects.all') = 1 RETURN 1;
    IF dbo.cpm_fn_HasPermission(@UserId, N'projects.view') = 1
       AND EXISTS (SELECT 1 FROM dbo.cpm_ProjectMember WHERE ProjectId = @ProjectId AND UserId = @UserId) RETURN 1;
    RETURN 0;
END
GO

/* ---------- land procedures ---------- */
CREATE OR ALTER PROCEDURE dbo.cpm_usp_LandParcel_List
    @RegId int, @UserId int, @Search nvarchar(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF dbo.cpm_fn_HasPermission(@UserId, N'land.view') = 0 THROW 50403, N'You don''t have the "View land parcels" permission.', 1;
    DECLARE @s nvarchar(102) = N'%' + NULLIF(LTRIM(RTRIM(@Search)), N'') + N'%';
    SELECT p.LandParcelId AS Id, p.SurveyNo, p.Locality, c.City_name AS City, ISNULL(s.statename, N'') AS State, p.AreaSqm, p.AreaEntered, p.AreaUnitEntered,
           p.Tenure, p.RegistrationNo, p.RegistrationDate, p.Encumbrance, dbo.cpm_fn_PersonName(p.ResponsibleUserId) AS Responsible,
           ISNULL((SELECT SUM(pl.AreaUsedSqm) FROM dbo.cpm_ProjectLandParcel pl JOIN dbo.cpm_Project pr ON pr.ProjectId = pl.ProjectId
                   WHERE pl.LandParcelId = p.LandParcelId AND pr.IsDeleted = 0), 0) AS AreaUsedSqm,
           STUFF((SELECT N', ' + pr.Name FROM dbo.cpm_ProjectLandParcel pl JOIN dbo.cpm_Project pr ON pr.ProjectId = pl.ProjectId
                  WHERE pl.LandParcelId = p.LandParcelId AND pr.IsDeleted = 0 FOR XML PATH(''), TYPE).value('.', 'nvarchar(max)'), 1, 2, N'') AS Projects
    FROM dbo.cpm_LandParcel p
    LEFT JOIN dbo.tbl_statemaster s ON s.id = p.StateId
    LEFT JOIN dbo.tbl_City_master c ON c.City_ID = p.CityId
    WHERE p.RegId = @RegId AND p.IsDeleted = 0
      AND (@s IS NULL OR p.SurveyNo LIKE @s OR p.Locality LIKE @s OR c.City_name LIKE @s)
    ORDER BY p.SurveyNo;
END
GO
CREATE OR ALTER PROCEDURE dbo.cpm_usp_LandParcel_Get
    @RegId int, @UserId int, @Id int
AS
BEGIN
    SET NOCOUNT ON;
    IF dbo.cpm_fn_HasPermission(@UserId, N'land.view') = 0 THROW 50403, N'You don''t have the "View land parcels" permission.', 1;
    SELECT p.LandParcelId AS Id, p.SurveyNo, p.Locality, p.CityId, p.StateId, ISNULL(st.countryid, 0) AS CountryId, p.AreaSqm, p.AreaEntered, p.AreaUnitEntered, p.Tenure,
           p.RegistrationNo, p.RegistrationDate, p.RegistryOffice, p.LandUse, p.Encumbrance, p.Latitude, p.Longitude,
           p.ResponsibleUserId, p.Remarks, CONVERT(varchar(18), CAST(p.RowVersion AS binary(8)), 1) AS RowVersion
    FROM dbo.cpm_LandParcel p LEFT JOIN dbo.tbl_statemaster st ON st.id = p.StateId
    WHERE p.RegId = @RegId AND p.LandParcelId = @Id AND p.IsDeleted = 0;
    SELECT o.OwnerName, o.SharePct FROM dbo.cpm_LandParcelOwner o JOIN dbo.cpm_LandParcel p ON p.LandParcelId = o.LandParcelId
    WHERE p.RegId = @RegId AND o.LandParcelId = @Id ORDER BY o.OwnerId;
END
GO
CREATE OR ALTER PROCEDURE dbo.cpm_usp_LandParcel_Save
    @RegId int, @UserId int, @Id int = NULL, @SurveyNo nvarchar(60), @Locality nvarchar(150) = NULL, @CityId int = NULL, @StateId int,
    @AreaSqm decimal(16,4), @AreaEntered decimal(16,4), @AreaUnitEntered nvarchar(10), @Tenure nvarchar(30),
    @RegistrationNo nvarchar(80) = NULL, @RegistrationDate date = NULL, @RegistryOffice nvarchar(150) = NULL, @LandUse nvarchar(100) = NULL,
    @Encumbrance nvarchar(20), @Latitude decimal(9,6) = NULL, @Longitude decimal(9,6) = NULL, @ResponsibleUserId int = NULL,
    @Remarks nvarchar(1000) = NULL, @Owners dbo.cpm_tt_Owner READONLY, @RowVersion binary(8) = NULL
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    IF dbo.cpm_fn_HasPermission(@UserId, N'land.edit') = 0 THROW 50403, N'You don''t have the "Add and edit land parcels" permission.', 1;
    SET @SurveyNo = LTRIM(RTRIM(ISNULL(@SurveyNo, N'')));
    IF @SurveyNo = N'' THROW 50400, N'Enter the survey or plot number.', 1;
    IF @AreaSqm IS NULL OR @AreaSqm <= 0 THROW 50400, N'Enter an area greater than zero.', 1;
    IF NOT EXISTS (SELECT 1 FROM dbo.tbl_statemaster WHERE id = @StateId AND ISNULL(archive, 0) = 0 AND (ISNULL(regid, 0) = 0 OR regid = @RegId)) THROW 50400, N'Choose a state.', 1;
    IF @CityId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.tbl_City_master WHERE City_ID = @CityId AND ISNULL(Archive, 0) = 0 AND (ISNULL(regid, 0) = 0 OR regid = @RegId))
        THROW 50400, N'Choose a city from the list.', 1;
    IF @ResponsibleUserId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.tbl_employee WHERE id = @ResponsibleUserId AND regid = @RegId AND status = 1 AND ISNULL(archive, 0) = 0)
        THROW 50400, N'Choose a valid responsible person.', 1;
    IF EXISTS (SELECT 1 FROM @Owners) AND (SELECT SUM(SharePct) FROM @Owners) <> 100
        THROW 50400, N'Owner shares must total 100 percent.', 1;
    IF EXISTS (SELECT 1 FROM dbo.cpm_LandParcel WHERE RegId = @RegId AND StateId = @StateId AND SurveyNo = @SurveyNo AND IsDeleted = 0 AND LandParcelId <> ISNULL(@Id, 0))
        THROW 50400, N'This survey number already exists in this state.', 1;
    IF @Id IS NOT NULL AND @AreaSqm < ISNULL((SELECT SUM(pl.AreaUsedSqm) FROM dbo.cpm_ProjectLandParcel pl JOIN dbo.cpm_Project pr ON pr.ProjectId = pl.ProjectId
                                              WHERE pl.LandParcelId = @Id AND pr.IsDeleted = 0), 0)
        THROW 50400, N'The area can''t be less than the area already used by projects.', 1;
    DECLARE @Old nvarchar(max), @Action nvarchar(20);
    BEGIN TRY
    BEGIN TRAN;
    IF @Id IS NULL
    BEGIN
        INSERT dbo.cpm_LandParcel(RegId, SurveyNo, Locality, CityId, StateId, AreaSqm, AreaEntered, AreaUnitEntered, Tenure, RegistrationNo, RegistrationDate,
                                  RegistryOffice, LandUse, Encumbrance, Latitude, Longitude, ResponsibleUserId, Remarks, CreatedBy)
        VALUES(@RegId, @SurveyNo, @Locality, @CityId, @StateId, @AreaSqm, @AreaEntered, @AreaUnitEntered, @Tenure, @RegistrationNo, @RegistrationDate,
               @RegistryOffice, @LandUse, @Encumbrance, @Latitude, @Longitude, @ResponsibleUserId, @Remarks, @UserId);
        SET @Id = SCOPE_IDENTITY(); SET @Action = N'Create';
    END
    ELSE
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM dbo.cpm_LandParcel WHERE LandParcelId = @Id AND RegId = @RegId AND IsDeleted = 0) THROW 50404, N'Land parcel not found.', 1;
        IF NOT EXISTS (SELECT 1 FROM dbo.cpm_LandParcel WHERE LandParcelId = @Id AND RowVersion = @RowVersion) THROW 50409, N'Someone else changed this parcel. Reload and try again.', 1;
        SET @Old = (SELECT p.*, (SELECT OwnerName, SharePct FROM dbo.cpm_LandParcelOwner WHERE LandParcelId = p.LandParcelId FOR JSON PATH) AS Owners
                    FROM (SELECT SurveyNo, Locality, CityId, StateId, AreaSqm, Tenure, RegistrationNo, RegistrationDate, RegistryOffice, LandUse, Encumbrance,
                                 Latitude, Longitude, ResponsibleUserId, Remarks, LandParcelId FROM dbo.cpm_LandParcel WHERE LandParcelId = @Id) p
                    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
        UPDATE dbo.cpm_LandParcel SET SurveyNo = @SurveyNo, Locality = @Locality, CityId = @CityId, StateId = @StateId, AreaSqm = @AreaSqm,
               AreaEntered = @AreaEntered, AreaUnitEntered = @AreaUnitEntered, Tenure = @Tenure, RegistrationNo = @RegistrationNo,
               RegistrationDate = @RegistrationDate, RegistryOffice = @RegistryOffice, LandUse = @LandUse, Encumbrance = @Encumbrance,
               Latitude = @Latitude, Longitude = @Longitude, ResponsibleUserId = @ResponsibleUserId, Remarks = @Remarks,
               UpdatedBy = @UserId, UpdatedAtUtc = SYSUTCDATETIME()
        WHERE LandParcelId = @Id;
        DELETE dbo.cpm_LandParcelOwner WHERE LandParcelId = @Id;
        SET @Action = N'Update';
    END
    INSERT dbo.cpm_LandParcelOwner(LandParcelId, OwnerName, SharePct) SELECT @Id, LTRIM(RTRIM(OwnerName)), SharePct FROM @Owners WHERE LTRIM(RTRIM(OwnerName)) <> N'';
    INSERT dbo.cpm_AuditLog(RegId, EntityType, EntityId, Action, OldData, NewData, UserId)
    SELECT @RegId, N'LandParcel', @Id, @Action, @Old,
           (SELECT p.*, (SELECT OwnerName, SharePct FROM dbo.cpm_LandParcelOwner WHERE LandParcelId = p.LandParcelId FOR JSON PATH) AS Owners
            FROM (SELECT SurveyNo, Locality, CityId, StateId, AreaSqm, Tenure, RegistrationNo, RegistrationDate, RegistryOffice, LandUse, Encumbrance,
                         Latitude, Longitude, ResponsibleUserId, Remarks, LandParcelId FROM dbo.cpm_LandParcel WHERE LandParcelId = @Id) p
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), @UserId;
    COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
    SELECT @Id AS Id;
END
GO
CREATE OR ALTER PROCEDURE dbo.cpm_usp_LandParcel_Delete
    @RegId int, @UserId int, @Id int, @Reason nvarchar(500)
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    IF dbo.cpm_fn_HasPermission(@UserId, N'land.edit') = 0 THROW 50403, N'You don''t have the "Add and edit land parcels" permission.', 1;
    IF LTRIM(RTRIM(ISNULL(@Reason, N''))) = N'' THROW 50400, N'Enter the reason for deleting.', 1;
    IF NOT EXISTS (SELECT 1 FROM dbo.cpm_LandParcel WHERE LandParcelId = @Id AND RegId = @RegId AND IsDeleted = 0) THROW 50404, N'Land parcel not found.', 1;
    IF EXISTS (SELECT 1 FROM dbo.cpm_ProjectLandParcel pl JOIN dbo.cpm_Project p ON p.ProjectId = pl.ProjectId WHERE pl.LandParcelId = @Id AND p.IsDeleted = 0)
        THROW 50400, N'This parcel is linked to a project. Unlink it from the project first.', 1;
    BEGIN TRY
    BEGIN TRAN;
    UPDATE dbo.cpm_LandParcel SET IsDeleted = 1, UpdatedBy = @UserId, UpdatedAtUtc = SYSUTCDATETIME() WHERE LandParcelId = @Id;
    INSERT dbo.cpm_AuditLog(RegId, EntityType, EntityId, Action, Reason, UserId) VALUES(@RegId, N'LandParcel', @Id, N'Delete', @Reason, @UserId);
    COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
END
GO

/* ---------- project procedures ---------- */
CREATE OR ALTER PROCEDURE dbo.cpm_usp_Project_List
    @RegId int, @UserId int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.ProjectId AS Id, p.Code, p.Name, p.DeveloperEntity, p.ProjectType, p.Status, c.City_name AS City, p.StartDate, p.TargetDate,
           (SELECT COUNT(*) FROM dbo.cpm_Building b WHERE b.ProjectId = p.ProjectId AND b.IsDeleted = 0) AS Buildings,
           (SELECT COUNT(*) FROM dbo.cpm_Unit u WHERE u.ProjectId = p.ProjectId AND u.IsDeleted = 0) AS Units,
           ISNULL((SELECT SUM(pl.AreaUsedSqm) FROM dbo.cpm_ProjectLandParcel pl WHERE pl.ProjectId = p.ProjectId), 0) AS LandSqm
    FROM dbo.cpm_Project p LEFT JOIN dbo.tbl_City_master c ON c.City_ID = p.CityId
    WHERE p.RegId = @RegId AND p.IsDeleted = 0 AND dbo.cpm_fn_CanAccessProject(@RegId, @UserId, p.ProjectId) = 1
    ORDER BY p.Name;
END
GO
CREATE OR ALTER PROCEDURE dbo.cpm_usp_Project_Get
    @RegId int, @UserId int, @Id int
AS
BEGIN
    SET NOCOUNT ON;
    IF dbo.cpm_fn_CanAccessProject(@RegId, @UserId, @Id) = 0 THROW 50404, N'Project not found.', 1;
    SELECT p.ProjectId AS Id, p.Code, p.Name, p.DeveloperEntity, p.ProjectType, p.UsesPhases, p.AreaUnit, p.TimeZoneId, p.CurrencyCode,
           p.Address, p.CityId, p.StateId, st.countryid AS CountryId, p.StartDate, p.TargetDate, p.UnitNumberPattern, p.SkipFloorNumbers, p.Status, CONVERT(varchar(18), CAST(p.RowVersion AS binary(8)), 1) AS RowVersion
    FROM dbo.cpm_Project p LEFT JOIN dbo.tbl_statemaster st ON st.id = p.StateId WHERE p.ProjectId = @Id;
    SELECT pl.LandParcelId, lp.SurveyNo, lp.AreaSqm AS ParcelAreaSqm, pl.AreaUsedSqm,
           lp.AreaSqm - ISNULL((SELECT SUM(x.AreaUsedSqm) FROM dbo.cpm_ProjectLandParcel x JOIN dbo.cpm_Project px ON px.ProjectId = x.ProjectId
                                WHERE x.LandParcelId = pl.LandParcelId AND x.ProjectId <> @Id AND px.IsDeleted = 0), 0) AS AvailableSqm
    FROM dbo.cpm_ProjectLandParcel pl JOIN dbo.cpm_LandParcel lp ON lp.LandParcelId = pl.LandParcelId
    WHERE pl.ProjectId = @Id ORDER BY lp.SurveyNo;
    SELECT m.UserId, dbo.cpm_fn_PersonName(m.UserId) AS FullName, ISNULL(e.email_Id, N'') AS Email FROM dbo.cpm_ProjectMember m JOIN dbo.tbl_employee e ON e.id = m.UserId
    WHERE m.ProjectId = @Id ORDER BY FullName;
END
GO
CREATE OR ALTER PROCEDURE dbo.cpm_usp_Project_Save
    @RegId int, @UserId int, @Id int = NULL, @Code nvarchar(30), @Name nvarchar(200), @DeveloperEntity nvarchar(200), @ProjectType nvarchar(20),
    @UsesPhases bit, @AreaUnit nvarchar(10), @TimeZoneId nvarchar(64), @CurrencyCode char(3), @Address nvarchar(300) = NULL,
    @CityId int = NULL, @StateId int = NULL, @StartDate date = NULL, @TargetDate date = NULL,
    @UnitNumberPattern nvarchar(40), @SkipFloorNumbers nvarchar(100) = NULL, @Status nvarchar(20),
    @Land dbo.cpm_tt_ProjectLand READONLY, @MemberIds dbo.cpm_tt_IntList READONLY, @RowVersion binary(8) = NULL
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    IF dbo.cpm_fn_HasPermission(@UserId, N'projects.edit') = 0 THROW 50403, N'You don''t have the "Create and edit projects" permission.', 1;
    SET @Code = LTRIM(RTRIM(ISNULL(@Code, N''))); SET @Name = LTRIM(RTRIM(ISNULL(@Name, N''))); SET @DeveloperEntity = LTRIM(RTRIM(ISNULL(@DeveloperEntity, N'')));
    IF @Code = N'' OR @Name = N'' OR @DeveloperEntity = N'' THROW 50400, N'Enter the project code, name and developer entity.', 1;
    IF @StartDate IS NOT NULL AND @TargetDate IS NOT NULL AND @StartDate > @TargetDate THROW 50400, N'The target date must be after the start date.', 1;
    IF CHARINDEX(N'{NN}', ISNULL(@UnitNumberPattern, N'')) = 0 THROW 50400, N'The unit number pattern must contain {NN} (position on the floor).', 1;
    IF @CityId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.tbl_City_master WHERE City_ID = @CityId AND ISNULL(Archive, 0) = 0 AND (ISNULL(regid, 0) = 0 OR regid = @RegId))
        THROW 50400, N'Choose a city from the list.', 1;
    IF EXISTS (SELECT 1 FROM dbo.cpm_Project WHERE RegId = @RegId AND Code = @Code AND IsDeleted = 0 AND ProjectId <> ISNULL(@Id, 0))
        THROW 50400, N'A project with this code already exists.', 1;
    IF EXISTS (SELECT 1 FROM @Land l WHERE NOT EXISTS (SELECT 1 FROM dbo.cpm_LandParcel p WHERE p.LandParcelId = l.LandParcelId AND p.RegId = @RegId AND p.IsDeleted = 0))
        THROW 50400, N'A linked land parcel doesn''t exist.', 1;
    IF EXISTS (SELECT 1 FROM @Land WHERE AreaUsedSqm <= 0) THROW 50400, N'Area used must be greater than zero.', 1;
    IF EXISTS (SELECT 1 FROM @MemberIds m WHERE NOT EXISTS (SELECT 1 FROM dbo.tbl_employee u WHERE u.id = m.Id AND u.regid = @RegId AND u.status = 1 AND ISNULL(u.archive, 0) = 0))
        THROW 50400, N'A selected team member doesn''t exist.', 1;
    IF @Id IS NOT NULL AND @UsesPhases = 0 AND EXISTS (SELECT 1 FROM dbo.cpm_Phase WHERE ProjectId = @Id AND IsDeleted = 0)
        THROW 50400, N'This project already has phases. Delete them before switching phases off.', 1;
    IF @Id IS NOT NULL AND @UsesPhases = 1 AND EXISTS (SELECT 1 FROM dbo.cpm_Building WHERE ProjectId = @Id AND PhaseId IS NULL AND IsDeleted = 0)
        THROW 50400, N'Some buildings have no phase. Assign them to a phase before switching phases on.', 1;
    DECLARE @Old nvarchar(max), @Action nvarchar(20), @NodeId bigint, @Over nvarchar(200);
    BEGIN TRY
    BEGIN TRAN;
    /* land over-use: other projects' use + this project's use must not exceed the parcel */
    SELECT TOP 1 @Over = lp.SurveyNo
    FROM @Land l JOIN dbo.cpm_LandParcel lp WITH (UPDLOCK) ON lp.LandParcelId = l.LandParcelId
    WHERE l.AreaUsedSqm + ISNULL((SELECT SUM(x.AreaUsedSqm) FROM dbo.cpm_ProjectLandParcel x JOIN dbo.cpm_Project px ON px.ProjectId = x.ProjectId
                                  WHERE x.LandParcelId = l.LandParcelId AND x.ProjectId <> ISNULL(@Id, 0) AND px.IsDeleted = 0), 0) > lp.AreaSqm;
    IF @Over IS NOT NULL
    BEGIN
        SET @Over = N'Area used on survey ' + @Over + N' is more than what is left on that parcel.';
        THROW 50400, @Over, 1;
    END
    IF @Id IS NULL
    BEGIN
        INSERT dbo.cpm_PropertyNode(RegId, NodeType) VALUES(@RegId, N'Project');
        SET @NodeId = SCOPE_IDENTITY();
        INSERT dbo.cpm_Project(RegId, NodeId, Code, Name, DeveloperEntity, ProjectType, UsesPhases, AreaUnit, TimeZoneId, CurrencyCode, Address, CityId, StateId,
                               StartDate, TargetDate, UnitNumberPattern, SkipFloorNumbers, Status, CreatedBy)
        VALUES(@RegId, @NodeId, @Code, @Name, @DeveloperEntity, @ProjectType, @UsesPhases, @AreaUnit, @TimeZoneId, @CurrencyCode, @Address, @CityId, @StateId,
               @StartDate, @TargetDate, @UnitNumberPattern, @SkipFloorNumbers, @Status, @UserId);
        SET @Id = SCOPE_IDENTITY(); SET @Action = N'Create';
        UPDATE dbo.cpm_PropertyNode SET ProjectId = @Id WHERE NodeId = @NodeId;
        IF NOT EXISTS (SELECT 1 FROM @MemberIds WHERE Id = @UserId) INSERT dbo.cpm_ProjectMember(ProjectId, UserId, CreatedBy) VALUES(@Id, @UserId, @UserId);
    END
    ELSE
    BEGIN
        IF dbo.cpm_fn_CanAccessProject(@RegId, @UserId, @Id) = 0 THROW 50404, N'Project not found.', 1;
        IF NOT EXISTS (SELECT 1 FROM dbo.cpm_Project WHERE ProjectId = @Id AND RowVersion = @RowVersion) THROW 50409, N'Someone else changed this project. Reload and try again.', 1;
        SET @Old = (SELECT p.Code, p.Name, p.DeveloperEntity, p.ProjectType, p.UsesPhases, p.AreaUnit, p.TimeZoneId, p.CurrencyCode, p.Address, p.CityId, p.StateId,
                           p.StartDate, p.TargetDate, p.UnitNumberPattern, p.SkipFloorNumbers, p.Status,
                           (SELECT LandParcelId, AreaUsedSqm FROM dbo.cpm_ProjectLandParcel WHERE ProjectId = p.ProjectId FOR JSON PATH) AS Land,
                           (SELECT UserId FROM dbo.cpm_ProjectMember WHERE ProjectId = p.ProjectId FOR JSON PATH) AS Members
                    FROM dbo.cpm_Project p WHERE p.ProjectId = @Id FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
        UPDATE dbo.cpm_Project SET Code = @Code, Name = @Name, DeveloperEntity = @DeveloperEntity, ProjectType = @ProjectType, UsesPhases = @UsesPhases,
               AreaUnit = @AreaUnit, TimeZoneId = @TimeZoneId, CurrencyCode = @CurrencyCode, Address = @Address, CityId = @CityId, StateId = @StateId,
               StartDate = @StartDate, TargetDate = @TargetDate, UnitNumberPattern = @UnitNumberPattern, SkipFloorNumbers = @SkipFloorNumbers,
               Status = @Status, UpdatedBy = @UserId, UpdatedAtUtc = SYSUTCDATETIME()
        WHERE ProjectId = @Id;
        DELETE dbo.cpm_ProjectLandParcel WHERE ProjectId = @Id;
        DELETE dbo.cpm_ProjectMember WHERE ProjectId = @Id;
        SET @Action = N'Update';
    END
    INSERT dbo.cpm_ProjectLandParcel(ProjectId, LandParcelId, AreaUsedSqm) SELECT @Id, LandParcelId, AreaUsedSqm FROM @Land;
    INSERT dbo.cpm_ProjectMember(ProjectId, UserId, CreatedBy) SELECT @Id, Id, @UserId FROM @MemberIds;
    INSERT dbo.cpm_AuditLog(RegId, EntityType, EntityId, Action, OldData, NewData, UserId)
    SELECT @RegId, N'Project', @Id, @Action, @Old,
           (SELECT p.Code, p.Name, p.DeveloperEntity, p.ProjectType, p.UsesPhases, p.AreaUnit, p.TimeZoneId, p.CurrencyCode, p.Address, p.CityId, p.StateId,
                   p.StartDate, p.TargetDate, p.UnitNumberPattern, p.SkipFloorNumbers, p.Status,
                   (SELECT LandParcelId, AreaUsedSqm FROM dbo.cpm_ProjectLandParcel WHERE ProjectId = p.ProjectId FOR JSON PATH) AS Land,
                   (SELECT UserId FROM dbo.cpm_ProjectMember WHERE ProjectId = p.ProjectId FOR JSON PATH) AS Members
            FROM dbo.cpm_Project p WHERE p.ProjectId = @Id FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), @UserId;
    COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
    SELECT @Id AS Id;
END
GO
CREATE OR ALTER PROCEDURE dbo.cpm_usp_Project_Delete
    @RegId int, @UserId int, @Id int, @Reason nvarchar(500)
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    IF dbo.cpm_fn_HasPermission(@UserId, N'projects.edit') = 0 THROW 50403, N'You don''t have the "Create and edit projects" permission.', 1;
    IF dbo.cpm_fn_CanAccessProject(@RegId, @UserId, @Id) = 0 THROW 50404, N'Project not found.', 1;
    IF LTRIM(RTRIM(ISNULL(@Reason, N''))) = N'' THROW 50400, N'Enter the reason for deleting.', 1;
    IF EXISTS (SELECT 1 FROM dbo.cpm_Phase WHERE ProjectId = @Id AND IsDeleted = 0) OR EXISTS (SELECT 1 FROM dbo.cpm_Building WHERE ProjectId = @Id AND IsDeleted = 0)
        THROW 50400, N'The project still has phases or buildings. Delete them first.', 1;
    BEGIN TRY
    BEGIN TRAN;
    UPDATE dbo.cpm_Project SET IsDeleted = 1, UpdatedBy = @UserId, UpdatedAtUtc = SYSUTCDATETIME() WHERE ProjectId = @Id;
    UPDATE dbo.cpm_PropertyNode SET IsDeleted = 1 WHERE NodeId = (SELECT NodeId FROM dbo.cpm_Project WHERE ProjectId = @Id);
    DELETE dbo.cpm_ProjectLandParcel WHERE ProjectId = @Id;
    INSERT dbo.cpm_AuditLog(RegId, EntityType, EntityId, Action, Reason, UserId) VALUES(@RegId, N'Project', @Id, N'Delete', @Reason, @UserId);
    COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
END
GO
/* People who can be put on a project team or made responsible. */
CREATE OR ALTER PROCEDURE dbo.cpm_usp_User_Lookup
    @RegId int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT e.id AS Id, dbo.cpm_fn_PersonName(e.id) AS FullName, ISNULL(e.email_Id, N'') AS Email
    FROM dbo.tbl_employee e WHERE e.regid = @RegId AND e.status = 1 AND ISNULL(e.archive, 0) = 0 ORDER BY FullName;
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.cpm_SchemaVersion WHERE ScriptName = N'003_land_projects')
    INSERT dbo.cpm_SchemaVersion(ScriptName) VALUES(N'003_land_projects');
GO
SET NOEXEC OFF;
GO
