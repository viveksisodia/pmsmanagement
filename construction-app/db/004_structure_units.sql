/* =====================================================================
   Construction app · 004 phases, buildings, floors, units, availability
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
   OR NOT EXISTS (SELECT 1 FROM dbo.cpm_SchemaVersion WHERE ScriptName = N'003_land_projects')
BEGIN
    RAISERROR(N'Stopped: run 001, 002 and 003 first.', 16, 1);
    SET NOEXEC ON;
END
GO

/* ---------- tables ---------- */
IF OBJECT_ID(N'dbo.cpm_Phase', N'U') IS NULL
CREATE TABLE dbo.cpm_Phase(
    PhaseId          int IDENTITY(1,1) NOT NULL CONSTRAINT PK_cpm_Phase PRIMARY KEY,
    RegId            int NOT NULL,
    ProjectId        int NOT NULL CONSTRAINT FK_cpm_Phase_Project REFERENCES dbo.cpm_Project(ProjectId),
    NodeId           bigint NOT NULL CONSTRAINT FK_cpm_Phase_Node REFERENCES dbo.cpm_PropertyNode(NodeId),
    Code             nvarchar(20)  NOT NULL,
    Name             nvarchar(100) NOT NULL,
    RegistrationNo   nvarchar(60)  NULL,
    PlannedStart     date NULL,
    PlannedFinish    date NULL,
    RegisteredCompletion date NULL,
    SortOrder        int NOT NULL CONSTRAINT DF_cpm_Phase_Sort DEFAULT 0,
    IsDeleted        bit NOT NULL CONSTRAINT DF_cpm_Phase_Del DEFAULT 0,
    CreatedBy int NULL, CreatedAtUtc datetime2(0) NOT NULL CONSTRAINT DF_cpm_Phase_At DEFAULT SYSUTCDATETIME(),
    UpdatedBy int NULL, UpdatedAtUtc datetime2(0) NULL,
    RowVersion rowversion NOT NULL,
    CONSTRAINT CK_cpm_Phase_Dates CHECK (PlannedStart IS NULL OR PlannedFinish IS NULL OR PlannedStart <= PlannedFinish));
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_cpm_Phase_Code')
    CREATE UNIQUE INDEX UX_cpm_Phase_Code ON dbo.cpm_Phase(ProjectId, Code) WHERE IsDeleted = 0;
GO
IF OBJECT_ID(N'dbo.cpm_Building', N'U') IS NULL
CREATE TABLE dbo.cpm_Building(
    BuildingId     int IDENTITY(1,1) NOT NULL CONSTRAINT PK_cpm_Building PRIMARY KEY,
    RegId          int NOT NULL,
    ProjectId      int NOT NULL CONSTRAINT FK_cpm_Building_Project REFERENCES dbo.cpm_Project(ProjectId),
    PhaseId        int NULL CONSTRAINT FK_cpm_Building_Phase REFERENCES dbo.cpm_Phase(PhaseId),
    NodeId         bigint NOT NULL CONSTRAINT FK_cpm_Building_Node REFERENCES dbo.cpm_PropertyNode(NodeId),
    Code           nvarchar(20)  NOT NULL,
    Name           nvarchar(100) NOT NULL,
    BuildingType   nvarchar(20)  NOT NULL CONSTRAINT CK_cpm_Building_Type CHECK (BuildingType IN (N'Tower', N'Block', N'Commercial', N'Villa cluster', N'Plot sector')),
    Lifts          tinyint NULL,
    Staircases     tinyint NULL,
    StructureType  nvarchar(60) NULL,
    PlannedStart   date NULL,
    PlannedFinish  date NULL,
    UnitNumberPattern nvarchar(40) NULL,
    SortOrder      int NOT NULL CONSTRAINT DF_cpm_Building_Sort DEFAULT 0,
    IsDeleted      bit NOT NULL CONSTRAINT DF_cpm_Building_Del DEFAULT 0,
    CreatedBy int NULL, CreatedAtUtc datetime2(0) NOT NULL CONSTRAINT DF_cpm_Building_At DEFAULT SYSUTCDATETIME(),
    UpdatedBy int NULL, UpdatedAtUtc datetime2(0) NULL,
    RowVersion rowversion NOT NULL,
    CONSTRAINT CK_cpm_Building_Dates CHECK (PlannedStart IS NULL OR PlannedFinish IS NULL OR PlannedStart <= PlannedFinish));
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_cpm_Building_Code')
    CREATE UNIQUE INDEX UX_cpm_Building_Code ON dbo.cpm_Building(ProjectId, Code) WHERE IsDeleted = 0;
GO
IF OBJECT_ID(N'dbo.cpm_Floor', N'U') IS NULL
CREATE TABLE dbo.cpm_Floor(
    FloorId     int IDENTITY(1,1) NOT NULL CONSTRAINT PK_cpm_Floor PRIMARY KEY,
    RegId       int NOT NULL,
    BuildingId  int NOT NULL CONSTRAINT FK_cpm_Floor_Building REFERENCES dbo.cpm_Building(BuildingId),
    NodeId      bigint NOT NULL CONSTRAINT FK_cpm_Floor_Node REFERENCES dbo.cpm_PropertyNode(NodeId),
    Code        nvarchar(10) NOT NULL,
    Name        nvarchar(60) NOT NULL,
    LevelTypeId int NOT NULL CONSTRAINT FK_cpm_Floor_Level REFERENCES dbo.cpm_LevelType(LevelTypeId),
    SortOrder   int NOT NULL,
    HeightM     decimal(5,2) NULL,
    IsDeleted   bit NOT NULL CONSTRAINT DF_cpm_Floor_Del DEFAULT 0,
    CreatedBy int NULL, CreatedAtUtc datetime2(0) NOT NULL CONSTRAINT DF_cpm_Floor_At DEFAULT SYSUTCDATETIME(),
    UpdatedBy int NULL, UpdatedAtUtc datetime2(0) NULL,
    RowVersion rowversion NOT NULL);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_cpm_Floor_Code')
    CREATE UNIQUE INDEX UX_cpm_Floor_Code ON dbo.cpm_Floor(BuildingId, Code) WHERE IsDeleted = 0;
GO
IF OBJECT_ID(N'dbo.cpm_Unit', N'U') IS NULL
CREATE TABLE dbo.cpm_Unit(
    UnitId       int IDENTITY(1,1) NOT NULL CONSTRAINT PK_cpm_Unit PRIMARY KEY,
    RegId        int NOT NULL,
    ProjectId    int NOT NULL CONSTRAINT FK_cpm_Unit_Project REFERENCES dbo.cpm_Project(ProjectId),
    PhaseId      int NULL CONSTRAINT FK_cpm_Unit_Phase REFERENCES dbo.cpm_Phase(PhaseId),
    BuildingId   int NULL CONSTRAINT FK_cpm_Unit_Building REFERENCES dbo.cpm_Building(BuildingId),
    FloorId      int NULL CONSTRAINT FK_cpm_Unit_Floor REFERENCES dbo.cpm_Floor(FloorId),
    NodeId       bigint NOT NULL CONSTRAINT FK_cpm_Unit_Node REFERENCES dbo.cpm_PropertyNode(NodeId),
    UnitNo       nvarchar(40) NOT NULL,
    Position     smallint NULL,
    UnitTypeId   int NOT NULL CONSTRAINT FK_cpm_Unit_Type REFERENCES dbo.cpm_UnitType(UnitTypeId),
    Facing       nvarchar(20) NULL,
    CarpetSqm    decimal(14,4) NULL,
    BuiltUpSqm   decimal(14,4) NULL,
    SuperSqm     decimal(14,4) NULL,
    BalconySqm   decimal(14,4) NULL,
    PlotSqm      decimal(14,4) NULL,
    Availability nvarchar(20) NOT NULL CONSTRAINT DF_cpm_Unit_Avail DEFAULT N'Available'
                 CONSTRAINT CK_cpm_Unit_Avail CHECK (Availability IN (N'Available', N'Blocked', N'Booked', N'Sold', N'Registered', N'Handed over')),
    IsDeleted    bit NOT NULL CONSTRAINT DF_cpm_Unit_Del DEFAULT 0,
    CreatedBy int NULL, CreatedAtUtc datetime2(0) NOT NULL CONSTRAINT DF_cpm_Unit_At DEFAULT SYSUTCDATETIME(),
    UpdatedBy int NULL, UpdatedAtUtc datetime2(0) NULL,
    RowVersion rowversion NOT NULL,
    CONSTRAINT CK_cpm_Unit_Areas CHECK ((CarpetSqm IS NULL OR BuiltUpSqm IS NULL OR CarpetSqm <= BuiltUpSqm)
                                    AND (BuiltUpSqm IS NULL OR SuperSqm IS NULL OR BuiltUpSqm <= SuperSqm)));
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_cpm_Unit_No')
    CREATE UNIQUE INDEX UX_cpm_Unit_No ON dbo.cpm_Unit(ProjectId, UnitNo) WHERE IsDeleted = 0;
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_cpm_Unit_Building')
    CREATE INDEX IX_cpm_Unit_Building ON dbo.cpm_Unit(RegId, BuildingId, FloorId) INCLUDE(UnitNo, Availability, UnitTypeId) WHERE IsDeleted = 0;
GO
IF OBJECT_ID(N'dbo.cpm_AvailabilityTransition', N'U') IS NULL
CREATE TABLE dbo.cpm_AvailabilityTransition(
    FromStatus nvarchar(20) NOT NULL,
    ToStatus   nvarchar(20) NOT NULL,
    CONSTRAINT PK_cpm_AvailabilityTransition PRIMARY KEY(FromStatus, ToStatus));
GO
MERGE dbo.cpm_AvailabilityTransition AS t
USING (VALUES (N'Available',N'Blocked'),(N'Available',N'Booked'),(N'Blocked',N'Available'),(N'Blocked',N'Booked'),
              (N'Booked',N'Available'),(N'Booked',N'Sold'),(N'Sold',N'Available'),(N'Sold',N'Registered'),(N'Registered',N'Handed over')) AS s(F, T)
ON t.FromStatus = s.F AND t.ToStatus = s.T
WHEN NOT MATCHED THEN INSERT(FromStatus, ToStatus) VALUES(s.F, s.T);
GO
IF OBJECT_ID(N'dbo.cpm_UnitAvailabilityHistory', N'U') IS NULL
CREATE TABLE dbo.cpm_UnitAvailabilityHistory(
    HistoryId   bigint IDENTITY(1,1) NOT NULL CONSTRAINT PK_cpm_UnitAvailHist PRIMARY KEY,
    RegId       int NOT NULL,
    UnitId      int NOT NULL CONSTRAINT FK_cpm_UnitAvailHist_Unit REFERENCES dbo.cpm_Unit(UnitId),
    FromStatus  nvarchar(20) NULL,
    ToStatus    nvarchar(20) NOT NULL,
    Reason      nvarchar(500) NOT NULL,
    ChangedBy   int NULL,
    ChangedAtUtc datetime2(0) NOT NULL CONSTRAINT DF_cpm_UnitAvailHist_At DEFAULT SYSUTCDATETIME());
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_cpm_UnitAvailHist_Unit')
    CREATE INDEX IX_cpm_UnitAvailHist_Unit ON dbo.cpm_UnitAvailabilityHistory(UnitId, HistoryId DESC);
GO

/* ---------- table types for generators ---------- */
IF TYPE_ID(N'dbo.cpm_tt_FloorGen') IS NULL
    CREATE TYPE dbo.cpm_tt_FloorGen AS TABLE(Code nvarchar(10) NOT NULL PRIMARY KEY, Name nvarchar(60) NOT NULL, LevelTypeId int NOT NULL,
                                             SortOrder int NOT NULL, HeightM decimal(5,2) NULL);
GO
IF TYPE_ID(N'dbo.cpm_tt_UnitGen') IS NULL
    CREATE TYPE dbo.cpm_tt_UnitGen AS TABLE(UnitNo nvarchar(40) NOT NULL PRIMARY KEY, FloorId int NULL, Position smallint NULL, UnitTypeId int NOT NULL,
                                            Facing nvarchar(20) NULL, CarpetSqm decimal(14,4) NULL, BuiltUpSqm decimal(14,4) NULL,
                                            SuperSqm decimal(14,4) NULL, BalconySqm decimal(14,4) NULL, PlotSqm decimal(14,4) NULL);
GO

/* ---------- structure tree ---------- */
CREATE OR ALTER PROCEDURE dbo.cpm_usp_Structure_Get
    @RegId int, @UserId int, @ProjectId int
AS
BEGIN
    SET NOCOUNT ON;
    IF dbo.cpm_fn_CanAccessProject(@RegId, @UserId, @ProjectId) = 0 THROW 50404, N'Project not found.', 1;
    SELECT p.PhaseId AS Id, p.Code, p.Name, p.RegistrationNo, p.PlannedStart, p.PlannedFinish, p.RegisteredCompletion, p.SortOrder, CONVERT(varchar(18), CAST(p.RowVersion AS binary(8)), 1) AS RowVersion,
           (SELECT COUNT(*) FROM dbo.cpm_Unit u WHERE u.PhaseId = p.PhaseId AND u.IsDeleted = 0) AS Units
    FROM dbo.cpm_Phase p WHERE p.ProjectId = @ProjectId AND p.IsDeleted = 0 ORDER BY p.SortOrder, p.Code;
    SELECT b.BuildingId AS Id, b.PhaseId, b.Code, b.Name, b.BuildingType, b.Lifts, b.Staircases, b.StructureType, b.PlannedStart, b.PlannedFinish,
           b.UnitNumberPattern, b.SortOrder, CONVERT(varchar(18), CAST(b.RowVersion AS binary(8)), 1) AS RowVersion,
           (SELECT COUNT(*) FROM dbo.cpm_Floor f WHERE f.BuildingId = b.BuildingId AND f.IsDeleted = 0) AS Floors,
           (SELECT COUNT(*) FROM dbo.cpm_Unit u WHERE u.BuildingId = b.BuildingId AND u.IsDeleted = 0) AS Units
    FROM dbo.cpm_Building b WHERE b.ProjectId = @ProjectId AND b.IsDeleted = 0 ORDER BY b.SortOrder, b.Code;
END
GO

/* ---------- phases ---------- */
CREATE OR ALTER PROCEDURE dbo.cpm_usp_Phase_Save
    @RegId int, @UserId int, @ProjectId int, @Id int = NULL, @Code nvarchar(20), @Name nvarchar(100), @RegistrationNo nvarchar(60) = NULL,
    @PlannedStart date = NULL, @PlannedFinish date = NULL, @RegisteredCompletion date = NULL, @SortOrder int = 0, @RowVersion binary(8) = NULL
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    IF dbo.cpm_fn_HasPermission(@UserId, N'structure.edit') = 0 THROW 50403, N'You don''t have the "Edit phases, buildings and floors" permission.', 1;
    IF dbo.cpm_fn_CanAccessProject(@RegId, @UserId, @ProjectId) = 0 THROW 50404, N'Project not found.', 1;
    IF EXISTS (SELECT 1 FROM dbo.cpm_Project WHERE ProjectId = @ProjectId AND UsesPhases = 0) THROW 50400, N'This project doesn''t use phases. Switch phases on in the project first.', 1;
    SET @Code = LTRIM(RTRIM(ISNULL(@Code, N''))); SET @Name = LTRIM(RTRIM(ISNULL(@Name, N'')));
    IF @Code = N'' OR @Name = N'' THROW 50400, N'Enter the phase code and name.', 1;
    IF @PlannedStart IS NOT NULL AND @PlannedFinish IS NOT NULL AND @PlannedStart > @PlannedFinish THROW 50400, N'Planned finish must be after planned start.', 1;
    IF EXISTS (SELECT 1 FROM dbo.cpm_Phase WHERE ProjectId = @ProjectId AND Code = @Code AND IsDeleted = 0 AND PhaseId <> ISNULL(@Id, 0))
        THROW 50400, N'A phase with this code already exists in the project.', 1;
    DECLARE @Old nvarchar(max), @Action nvarchar(20), @NodeId bigint;
    BEGIN TRY
    BEGIN TRAN;
    IF @Id IS NULL
    BEGIN
        INSERT dbo.cpm_PropertyNode(RegId, ProjectId, NodeType, ParentNodeId) SELECT @RegId, @ProjectId, N'Phase', NodeId FROM dbo.cpm_Project WHERE ProjectId = @ProjectId;
        SET @NodeId = SCOPE_IDENTITY();
        INSERT dbo.cpm_Phase(RegId, ProjectId, NodeId, Code, Name, RegistrationNo, PlannedStart, PlannedFinish, RegisteredCompletion, SortOrder, CreatedBy)
        VALUES(@RegId, @ProjectId, @NodeId, @Code, @Name, @RegistrationNo, @PlannedStart, @PlannedFinish, @RegisteredCompletion, @SortOrder, @UserId);
        SET @Id = SCOPE_IDENTITY(); SET @Action = N'Create';
    END
    ELSE
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM dbo.cpm_Phase WHERE PhaseId = @Id AND ProjectId = @ProjectId AND IsDeleted = 0) THROW 50404, N'Phase not found.', 1;
        IF NOT EXISTS (SELECT 1 FROM dbo.cpm_Phase WHERE PhaseId = @Id AND RowVersion = @RowVersion) THROW 50409, N'Someone else changed this phase. Reload and try again.', 1;
        SET @Old = (SELECT Code, Name, RegistrationNo, PlannedStart, PlannedFinish, RegisteredCompletion, SortOrder FROM dbo.cpm_Phase WHERE PhaseId = @Id FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
        UPDATE dbo.cpm_Phase SET Code = @Code, Name = @Name, RegistrationNo = @RegistrationNo, PlannedStart = @PlannedStart, PlannedFinish = @PlannedFinish,
               RegisteredCompletion = @RegisteredCompletion, SortOrder = @SortOrder, UpdatedBy = @UserId, UpdatedAtUtc = SYSUTCDATETIME() WHERE PhaseId = @Id;
        SET @Action = N'Update';
    END
    INSERT dbo.cpm_AuditLog(RegId, EntityType, EntityId, Action, OldData, NewData, UserId)
    SELECT @RegId, N'Phase', @Id, @Action, @Old,
           (SELECT Code, Name, RegistrationNo, PlannedStart, PlannedFinish, RegisteredCompletion, SortOrder FROM dbo.cpm_Phase WHERE PhaseId = @Id FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), @UserId;
    COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
    SELECT @Id AS Id;
END
GO
CREATE OR ALTER PROCEDURE dbo.cpm_usp_Phase_Delete
    @RegId int, @UserId int, @ProjectId int, @Id int, @Reason nvarchar(500)
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    IF dbo.cpm_fn_HasPermission(@UserId, N'structure.edit') = 0 THROW 50403, N'You don''t have the "Edit phases, buildings and floors" permission.', 1;
    IF dbo.cpm_fn_CanAccessProject(@RegId, @UserId, @ProjectId) = 0 THROW 50404, N'Project not found.', 1;
    IF LTRIM(RTRIM(ISNULL(@Reason, N''))) = N'' THROW 50400, N'Enter the reason for deleting.', 1;
    IF NOT EXISTS (SELECT 1 FROM dbo.cpm_Phase WHERE PhaseId = @Id AND ProjectId = @ProjectId AND IsDeleted = 0) THROW 50404, N'Phase not found.', 1;
    IF EXISTS (SELECT 1 FROM dbo.cpm_Building WHERE PhaseId = @Id AND IsDeleted = 0) OR EXISTS (SELECT 1 FROM dbo.cpm_Unit WHERE PhaseId = @Id AND IsDeleted = 0)
        THROW 50400, N'The phase still has buildings or units. Delete or move them first.', 1;
    BEGIN TRY
    BEGIN TRAN;
    UPDATE dbo.cpm_Phase SET IsDeleted = 1, UpdatedBy = @UserId, UpdatedAtUtc = SYSUTCDATETIME() WHERE PhaseId = @Id;
    UPDATE dbo.cpm_PropertyNode SET IsDeleted = 1 WHERE NodeId = (SELECT NodeId FROM dbo.cpm_Phase WHERE PhaseId = @Id);
    INSERT dbo.cpm_AuditLog(RegId, EntityType, EntityId, Action, Reason, UserId) VALUES(@RegId, N'Phase', @Id, N'Delete', @Reason, @UserId);
    COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
END
GO

/* ---------- buildings ---------- */
CREATE OR ALTER PROCEDURE dbo.cpm_usp_Building_Save
    @RegId int, @UserId int, @ProjectId int, @Id int = NULL, @PhaseId int = NULL, @Code nvarchar(20), @Name nvarchar(100), @BuildingType nvarchar(20),
    @Lifts tinyint = NULL, @Staircases tinyint = NULL, @StructureType nvarchar(60) = NULL, @PlannedStart date = NULL, @PlannedFinish date = NULL,
    @UnitNumberPattern nvarchar(40) = NULL, @SortOrder int = 0, @RowVersion binary(8) = NULL
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    IF dbo.cpm_fn_HasPermission(@UserId, N'structure.edit') = 0 THROW 50403, N'You don''t have the "Edit phases, buildings and floors" permission.', 1;
    IF dbo.cpm_fn_CanAccessProject(@RegId, @UserId, @ProjectId) = 0 THROW 50404, N'Project not found.', 1;
    SET @Code = LTRIM(RTRIM(ISNULL(@Code, N''))); SET @Name = LTRIM(RTRIM(ISNULL(@Name, N'')));
    IF @Code = N'' OR @Name = N'' THROW 50400, N'Enter the building code and name.', 1;
    IF @BuildingType NOT IN (N'Tower', N'Block', N'Commercial', N'Villa cluster', N'Plot sector') THROW 50400, N'Choose a valid building type.', 1;
    IF EXISTS (SELECT 1 FROM dbo.cpm_Project WHERE ProjectId = @ProjectId AND UsesPhases = 1) AND @PhaseId IS NULL THROW 50400, N'Choose the phase this building belongs to.', 1;
    IF EXISTS (SELECT 1 FROM dbo.cpm_Project WHERE ProjectId = @ProjectId AND UsesPhases = 0) SET @PhaseId = NULL;
    IF @PhaseId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.cpm_Phase WHERE PhaseId = @PhaseId AND ProjectId = @ProjectId AND IsDeleted = 0) THROW 50400, N'The phase doesn''t belong to this project.', 1;
    IF @PlannedStart IS NOT NULL AND @PlannedFinish IS NOT NULL AND @PlannedStart > @PlannedFinish THROW 50400, N'Planned finish must be after planned start.', 1;
    IF @UnitNumberPattern IS NOT NULL AND LTRIM(RTRIM(@UnitNumberPattern)) = N'' SET @UnitNumberPattern = NULL;
    IF @UnitNumberPattern IS NOT NULL AND CHARINDEX(N'{NN}', @UnitNumberPattern) = 0 THROW 50400, N'The unit number pattern must contain {NN}.', 1;
    IF EXISTS (SELECT 1 FROM dbo.cpm_Building WHERE ProjectId = @ProjectId AND Code = @Code AND IsDeleted = 0 AND BuildingId <> ISNULL(@Id, 0))
        THROW 50400, N'A building with this code already exists in the project.', 1;
    IF @Id IS NOT NULL AND @BuildingType IN (N'Villa cluster', N'Plot sector') AND EXISTS (SELECT 1 FROM dbo.cpm_Floor WHERE BuildingId = @Id AND IsDeleted = 0)
        THROW 50400, N'This building has floors. Villa clusters and plot sectors can''t have floors.', 1;
    DECLARE @Old nvarchar(max), @Action nvarchar(20), @NodeId bigint, @ParentNode bigint;
    SET @ParentNode = ISNULL((SELECT NodeId FROM dbo.cpm_Phase WHERE PhaseId = @PhaseId), (SELECT NodeId FROM dbo.cpm_Project WHERE ProjectId = @ProjectId));
    BEGIN TRY
    BEGIN TRAN;
    IF @Id IS NULL
    BEGIN
        INSERT dbo.cpm_PropertyNode(RegId, ProjectId, NodeType, ParentNodeId) VALUES(@RegId, @ProjectId, N'Building', @ParentNode);
        SET @NodeId = SCOPE_IDENTITY();
        INSERT dbo.cpm_Building(RegId, ProjectId, PhaseId, NodeId, Code, Name, BuildingType, Lifts, Staircases, StructureType, PlannedStart, PlannedFinish, UnitNumberPattern, SortOrder, CreatedBy)
        VALUES(@RegId, @ProjectId, @PhaseId, @NodeId, @Code, @Name, @BuildingType, @Lifts, @Staircases, @StructureType, @PlannedStart, @PlannedFinish, @UnitNumberPattern, @SortOrder, @UserId);
        SET @Id = SCOPE_IDENTITY(); SET @Action = N'Create';
    END
    ELSE
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM dbo.cpm_Building WHERE BuildingId = @Id AND ProjectId = @ProjectId AND IsDeleted = 0) THROW 50404, N'Building not found.', 1;
        IF NOT EXISTS (SELECT 1 FROM dbo.cpm_Building WHERE BuildingId = @Id AND RowVersion = @RowVersion) THROW 50409, N'Someone else changed this building. Reload and try again.', 1;
        SET @Old = (SELECT PhaseId, Code, Name, BuildingType, Lifts, Staircases, StructureType, PlannedStart, PlannedFinish, UnitNumberPattern, SortOrder
                    FROM dbo.cpm_Building WHERE BuildingId = @Id FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
        UPDATE dbo.cpm_Building SET PhaseId = @PhaseId, Code = @Code, Name = @Name, BuildingType = @BuildingType, Lifts = @Lifts, Staircases = @Staircases,
               StructureType = @StructureType, PlannedStart = @PlannedStart, PlannedFinish = @PlannedFinish, UnitNumberPattern = @UnitNumberPattern,
               SortOrder = @SortOrder, UpdatedBy = @UserId, UpdatedAtUtc = SYSUTCDATETIME() WHERE BuildingId = @Id;
        UPDATE dbo.cpm_PropertyNode SET ParentNodeId = @ParentNode WHERE NodeId = (SELECT NodeId FROM dbo.cpm_Building WHERE BuildingId = @Id);
        UPDATE dbo.cpm_Unit SET PhaseId = @PhaseId WHERE BuildingId = @Id AND ISNULL(PhaseId, 0) <> ISNULL(@PhaseId, 0);
        SET @Action = N'Update';
    END
    INSERT dbo.cpm_AuditLog(RegId, EntityType, EntityId, Action, OldData, NewData, UserId)
    SELECT @RegId, N'Building', @Id, @Action, @Old,
           (SELECT PhaseId, Code, Name, BuildingType, Lifts, Staircases, StructureType, PlannedStart, PlannedFinish, UnitNumberPattern, SortOrder
            FROM dbo.cpm_Building WHERE BuildingId = @Id FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), @UserId;
    COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
    SELECT @Id AS Id;
END
GO
CREATE OR ALTER PROCEDURE dbo.cpm_usp_Building_Delete
    @RegId int, @UserId int, @ProjectId int, @Id int, @Reason nvarchar(500)
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    IF dbo.cpm_fn_HasPermission(@UserId, N'structure.edit') = 0 THROW 50403, N'You don''t have the "Edit phases, buildings and floors" permission.', 1;
    IF dbo.cpm_fn_CanAccessProject(@RegId, @UserId, @ProjectId) = 0 THROW 50404, N'Project not found.', 1;
    IF LTRIM(RTRIM(ISNULL(@Reason, N''))) = N'' THROW 50400, N'Enter the reason for deleting.', 1;
    IF NOT EXISTS (SELECT 1 FROM dbo.cpm_Building WHERE BuildingId = @Id AND ProjectId = @ProjectId AND IsDeleted = 0) THROW 50404, N'Building not found.', 1;
    IF EXISTS (SELECT 1 FROM dbo.cpm_Unit WHERE BuildingId = @Id AND IsDeleted = 0) THROW 50400, N'The building still has units. Delete them first.', 1;
    BEGIN TRY
    BEGIN TRAN;
    UPDATE dbo.cpm_PropertyNode SET IsDeleted = 1 WHERE NodeId IN (SELECT NodeId FROM dbo.cpm_Floor WHERE BuildingId = @Id AND IsDeleted = 0);
    UPDATE dbo.cpm_Floor SET IsDeleted = 1, UpdatedBy = @UserId, UpdatedAtUtc = SYSUTCDATETIME() WHERE BuildingId = @Id AND IsDeleted = 0;
    UPDATE dbo.cpm_Building SET IsDeleted = 1, UpdatedBy = @UserId, UpdatedAtUtc = SYSUTCDATETIME() WHERE BuildingId = @Id;
    UPDATE dbo.cpm_PropertyNode SET IsDeleted = 1 WHERE NodeId = (SELECT NodeId FROM dbo.cpm_Building WHERE BuildingId = @Id);
    INSERT dbo.cpm_AuditLog(RegId, EntityType, EntityId, Action, Reason, UserId) VALUES(@RegId, N'Building', @Id, N'Delete', @Reason, @UserId);
    COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
END
GO

/* ---------- floors ---------- */
CREATE OR ALTER PROCEDURE dbo.cpm_usp_Floor_List
    @RegId int, @UserId int, @ProjectId int, @BuildingId int
AS
BEGIN
    SET NOCOUNT ON;
    IF dbo.cpm_fn_CanAccessProject(@RegId, @UserId, @ProjectId) = 0 THROW 50404, N'Project not found.', 1;
    SELECT f.FloorId AS Id, f.Code, f.Name, f.LevelTypeId, l.Name AS LevelType, l.IsHabitable, f.SortOrder, f.HeightM, CONVERT(varchar(18), CAST(f.RowVersion AS binary(8)), 1) AS RowVersion,
           (SELECT COUNT(*) FROM dbo.cpm_Unit u WHERE u.FloorId = f.FloorId AND u.IsDeleted = 0) AS Units
    FROM dbo.cpm_Floor f JOIN dbo.cpm_LevelType l ON l.LevelTypeId = f.LevelTypeId
    JOIN dbo.cpm_Building b ON b.BuildingId = f.BuildingId
    WHERE f.BuildingId = @BuildingId AND b.ProjectId = @ProjectId AND f.IsDeleted = 0
    ORDER BY f.SortOrder;
END
GO
/* Inserts generated floors; existing codes are skipped. Returns created and skipped counts. */
CREATE OR ALTER PROCEDURE dbo.cpm_usp_Floor_Generate
    @RegId int, @UserId int, @ProjectId int, @BuildingId int, @Floors dbo.cpm_tt_FloorGen READONLY
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    IF dbo.cpm_fn_HasPermission(@UserId, N'units.generate') = 0 THROW 50403, N'You don''t have the "Run the floor and unit generators" permission.', 1;
    IF dbo.cpm_fn_CanAccessProject(@RegId, @UserId, @ProjectId) = 0 THROW 50404, N'Project not found.', 1;
    IF NOT EXISTS (SELECT 1 FROM dbo.cpm_Building WHERE BuildingId = @BuildingId AND ProjectId = @ProjectId AND IsDeleted = 0) THROW 50404, N'Building not found.', 1;
    IF EXISTS (SELECT 1 FROM dbo.cpm_Building WHERE BuildingId = @BuildingId AND BuildingType IN (N'Villa cluster', N'Plot sector'))
        THROW 50400, N'Villa clusters and plot sectors don''t have floors.', 1;
    IF EXISTS (SELECT 1 FROM @Floors g WHERE NOT EXISTS (SELECT 1 FROM dbo.cpm_LevelType l WHERE l.LevelTypeId = g.LevelTypeId AND l.RegId = @RegId))
        THROW 50400, N'A floor uses a level type that doesn''t exist.', 1;
    IF NOT EXISTS (SELECT 1 FROM @Floors) THROW 50400, N'There are no floors to create.', 1;
    DECLARE @New TABLE(Code nvarchar(10) PRIMARY KEY, Name nvarchar(60), LevelTypeId int, SortOrder int, HeightM decimal(5,2), NodeId bigint NULL);
    DECLARE @BuildingNode bigint = (SELECT NodeId FROM dbo.cpm_Building WHERE BuildingId = @BuildingId);
    DECLARE @Total int = (SELECT COUNT(*) FROM @Floors), @Created int;
    BEGIN TRY
    BEGIN TRAN;
    INSERT @New(Code, Name, LevelTypeId, SortOrder, HeightM)
    SELECT g.Code, g.Name, g.LevelTypeId, g.SortOrder, g.HeightM FROM @Floors g
    WHERE NOT EXISTS (SELECT 1 FROM dbo.cpm_Floor f WITH (UPDLOCK, HOLDLOCK) WHERE f.BuildingId = @BuildingId AND f.Code = g.Code AND f.IsDeleted = 0);
    SET @Created = @@ROWCOUNT;
    /* one property node per new floor, matched back by order */
    DECLARE @Nodes TABLE(Seq int IDENTITY(1,1), NodeId bigint);
    INSERT dbo.cpm_PropertyNode(RegId, ProjectId, NodeType, ParentNodeId)
    OUTPUT inserted.NodeId INTO @Nodes(NodeId)
    SELECT @RegId, @ProjectId, N'Floor', @BuildingNode FROM @New ORDER BY Code;
    /* identity values follow the ORDER BY of the insert, so match by rank of NodeId, not by OUTPUT order */
    ;WITH n AS (SELECT Code, ROW_NUMBER() OVER (ORDER BY Code) AS Seq FROM @New),
          nd AS (SELECT NodeId, ROW_NUMBER() OVER (ORDER BY NodeId) AS Seq FROM @Nodes)
    UPDATE x SET NodeId = nd.NodeId FROM @New x JOIN n ON n.Code = x.Code JOIN nd ON nd.Seq = n.Seq;
    INSERT dbo.cpm_Floor(RegId, BuildingId, NodeId, Code, Name, LevelTypeId, SortOrder, HeightM, CreatedBy)
    SELECT @RegId, @BuildingId, NodeId, Code, Name, LevelTypeId, SortOrder, HeightM, @UserId FROM @New;
    /* keep sort order of existing floors in line with the generated plan */
    UPDATE f SET SortOrder = g.SortOrder FROM dbo.cpm_Floor f JOIN @Floors g ON g.Code = f.Code WHERE f.BuildingId = @BuildingId AND f.IsDeleted = 0;
    INSERT dbo.cpm_AuditLog(RegId, EntityType, EntityId, Action, NewData, UserId)
    VALUES(@RegId, N'Building', @BuildingId, N'GenerateFloors',
           (SELECT @Created AS Created, @Total - @Created AS Skipped, (SELECT Code FROM @New FOR JSON PATH) AS Codes FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), @UserId);
    COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
    SELECT @Created AS Created, @Total - @Created AS Skipped;
END
GO
CREATE OR ALTER PROCEDURE dbo.cpm_usp_Floor_Save
    @RegId int, @UserId int, @ProjectId int, @Id int, @Code nvarchar(10), @Name nvarchar(60), @LevelTypeId int, @SortOrder int,
    @HeightM decimal(5,2) = NULL, @RowVersion binary(8)
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    IF dbo.cpm_fn_HasPermission(@UserId, N'structure.edit') = 0 THROW 50403, N'You don''t have the "Edit phases, buildings and floors" permission.', 1;
    IF dbo.cpm_fn_CanAccessProject(@RegId, @UserId, @ProjectId) = 0 THROW 50404, N'Project not found.', 1;
    DECLARE @BuildingId int = (SELECT f.BuildingId FROM dbo.cpm_Floor f JOIN dbo.cpm_Building b ON b.BuildingId = f.BuildingId
                               WHERE f.FloorId = @Id AND b.ProjectId = @ProjectId AND f.IsDeleted = 0);
    IF @BuildingId IS NULL THROW 50404, N'Floor not found.', 1;
    SET @Code = LTRIM(RTRIM(ISNULL(@Code, N''))); SET @Name = LTRIM(RTRIM(ISNULL(@Name, N'')));
    IF @Code = N'' OR @Name = N'' THROW 50400, N'Enter the floor code and name.', 1;
    IF NOT EXISTS (SELECT 1 FROM dbo.cpm_LevelType WHERE LevelTypeId = @LevelTypeId AND RegId = @RegId) THROW 50400, N'Choose a level type.', 1;
    IF EXISTS (SELECT 1 FROM dbo.cpm_Floor WHERE BuildingId = @BuildingId AND Code = @Code AND IsDeleted = 0 AND FloorId <> @Id)
        THROW 50400, N'Another floor in this building already uses this code.', 1;
    IF EXISTS (SELECT 1 FROM dbo.cpm_LevelType WHERE LevelTypeId = @LevelTypeId AND IsHabitable = 0) AND EXISTS (SELECT 1 FROM dbo.cpm_Unit WHERE FloorId = @Id AND IsDeleted = 0)
        THROW 50400, N'This floor has units, so it must stay a habitable level type.', 1;
    IF NOT EXISTS (SELECT 1 FROM dbo.cpm_Floor WHERE FloorId = @Id AND RowVersion = @RowVersion) THROW 50409, N'Someone else changed this floor. Reload and try again.', 1;
    DECLARE @Old nvarchar(max) = (SELECT Code, Name, LevelTypeId, SortOrder, HeightM FROM dbo.cpm_Floor WHERE FloorId = @Id FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
    BEGIN TRY
    BEGIN TRAN;
    UPDATE dbo.cpm_Floor SET Code = @Code, Name = @Name, LevelTypeId = @LevelTypeId, SortOrder = @SortOrder, HeightM = @HeightM,
           UpdatedBy = @UserId, UpdatedAtUtc = SYSUTCDATETIME() WHERE FloorId = @Id;
    INSERT dbo.cpm_AuditLog(RegId, EntityType, EntityId, Action, OldData, NewData, UserId)
    SELECT @RegId, N'Floor', @Id, N'Update', @Old,
           (SELECT Code, Name, LevelTypeId, SortOrder, HeightM FROM dbo.cpm_Floor WHERE FloorId = @Id FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), @UserId;
    COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
    SELECT @Id AS Id;
END
GO
CREATE OR ALTER PROCEDURE dbo.cpm_usp_Floor_Delete
    @RegId int, @UserId int, @ProjectId int, @Id int, @Reason nvarchar(500)
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    IF dbo.cpm_fn_HasPermission(@UserId, N'structure.edit') = 0 THROW 50403, N'You don''t have the "Edit phases, buildings and floors" permission.', 1;
    IF dbo.cpm_fn_CanAccessProject(@RegId, @UserId, @ProjectId) = 0 THROW 50404, N'Project not found.', 1;
    IF LTRIM(RTRIM(ISNULL(@Reason, N''))) = N'' THROW 50400, N'Enter the reason for deleting.', 1;
    IF NOT EXISTS (SELECT 1 FROM dbo.cpm_Floor f JOIN dbo.cpm_Building b ON b.BuildingId = f.BuildingId WHERE f.FloorId = @Id AND b.ProjectId = @ProjectId AND f.IsDeleted = 0)
        THROW 50404, N'Floor not found.', 1;
    IF EXISTS (SELECT 1 FROM dbo.cpm_Unit WHERE FloorId = @Id AND IsDeleted = 0) THROW 50400, N'The floor still has units. Delete them first.', 1;
    BEGIN TRY
    BEGIN TRAN;
    UPDATE dbo.cpm_Floor SET IsDeleted = 1, UpdatedBy = @UserId, UpdatedAtUtc = SYSUTCDATETIME() WHERE FloorId = @Id;
    UPDATE dbo.cpm_PropertyNode SET IsDeleted = 1 WHERE NodeId = (SELECT NodeId FROM dbo.cpm_Floor WHERE FloorId = @Id);
    INSERT dbo.cpm_AuditLog(RegId, EntityType, EntityId, Action, Reason, UserId) VALUES(@RegId, N'Floor', @Id, N'Delete', @Reason, @UserId);
    COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
END
GO

/* ---------- units ---------- */
CREATE OR ALTER PROCEDURE dbo.cpm_usp_Unit_List
    @RegId int, @UserId int, @ProjectId int, @BuildingId int = NULL, @FloorId int = NULL, @UnitTypeId int = NULL,
    @Availability nvarchar(20) = NULL, @Search nvarchar(40) = NULL, @Page int = 1, @PageSize int = 100
AS
BEGIN
    SET NOCOUNT ON;
    IF dbo.cpm_fn_CanAccessProject(@RegId, @UserId, @ProjectId) = 0 THROW 50404, N'Project not found.', 1;
    DECLARE @s nvarchar(42) = N'%' + NULLIF(LTRIM(RTRIM(@Search)), N'') + N'%';
    SELECT COUNT(*) AS Total FROM dbo.cpm_Unit u
    WHERE u.ProjectId = @ProjectId AND u.IsDeleted = 0 AND (@BuildingId IS NULL OR u.BuildingId = @BuildingId) AND (@FloorId IS NULL OR u.FloorId = @FloorId)
      AND (@UnitTypeId IS NULL OR u.UnitTypeId = @UnitTypeId) AND (@Availability IS NULL OR u.Availability = @Availability) AND (@s IS NULL OR u.UnitNo LIKE @s);
    SELECT u.UnitId AS Id, u.UnitNo, u.PhaseId, ph.Name AS Phase, u.BuildingId, b.Name AS Building, u.FloorId, f.Name AS Floor, u.Position,
           u.UnitTypeId, t.Name AS UnitType, u.Facing, u.CarpetSqm, u.BuiltUpSqm, u.SuperSqm, u.BalconySqm, u.PlotSqm, u.Availability
    FROM dbo.cpm_Unit u
    JOIN dbo.cpm_UnitType t ON t.UnitTypeId = u.UnitTypeId
    LEFT JOIN dbo.cpm_Phase ph ON ph.PhaseId = u.PhaseId
    LEFT JOIN dbo.cpm_Building b ON b.BuildingId = u.BuildingId
    LEFT JOIN dbo.cpm_Floor f ON f.FloorId = u.FloorId
    WHERE u.ProjectId = @ProjectId AND u.IsDeleted = 0 AND (@BuildingId IS NULL OR u.BuildingId = @BuildingId) AND (@FloorId IS NULL OR u.FloorId = @FloorId)
      AND (@UnitTypeId IS NULL OR u.UnitTypeId = @UnitTypeId) AND (@Availability IS NULL OR u.Availability = @Availability) AND (@s IS NULL OR u.UnitNo LIKE @s)
    ORDER BY b.SortOrder, b.Code, f.SortOrder, u.Position, u.UnitNo
    OFFSET (CASE WHEN @Page < 1 THEN 0 ELSE @Page - 1 END) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;
END
GO
/* Stacking plan: floors of one building (top first) and their units. */
CREATE OR ALTER PROCEDURE dbo.cpm_usp_Unit_Stack
    @RegId int, @UserId int, @ProjectId int, @BuildingId int
AS
BEGIN
    SET NOCOUNT ON;
    IF dbo.cpm_fn_CanAccessProject(@RegId, @UserId, @ProjectId) = 0 THROW 50404, N'Project not found.', 1;
    IF NOT EXISTS (SELECT 1 FROM dbo.cpm_Building WHERE BuildingId = @BuildingId AND ProjectId = @ProjectId AND IsDeleted = 0) THROW 50404, N'Building not found.', 1;
    SELECT f.FloorId AS Id, f.Code, f.Name, l.IsHabitable, f.SortOrder
    FROM dbo.cpm_Floor f JOIN dbo.cpm_LevelType l ON l.LevelTypeId = f.LevelTypeId
    WHERE f.BuildingId = @BuildingId AND f.IsDeleted = 0 ORDER BY f.SortOrder DESC;
    SELECT u.UnitId AS Id, u.UnitNo, u.FloorId, u.Position, t.Name AS UnitType, u.CarpetSqm, u.Availability
    FROM dbo.cpm_Unit u JOIN dbo.cpm_UnitType t ON t.UnitTypeId = u.UnitTypeId
    WHERE u.BuildingId = @BuildingId AND u.IsDeleted = 0 ORDER BY u.Position, u.UnitNo;
END
GO
/* Existing unit numbers in a project, used by the generator preview. */
CREATE OR ALTER PROCEDURE dbo.cpm_usp_Unit_ExistingNumbers
    @RegId int, @UserId int, @ProjectId int
AS
BEGIN
    SET NOCOUNT ON;
    IF dbo.cpm_fn_CanAccessProject(@RegId, @UserId, @ProjectId) = 0 THROW 50404, N'Project not found.', 1;
    SELECT UnitNo FROM dbo.cpm_Unit WHERE ProjectId = @ProjectId AND IsDeleted = 0;
END
GO
CREATE OR ALTER PROCEDURE dbo.cpm_usp_Unit_Generate
    @RegId int, @UserId int, @ProjectId int, @BuildingId int, @Units dbo.cpm_tt_UnitGen READONLY
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    IF dbo.cpm_fn_HasPermission(@UserId, N'units.generate') = 0 THROW 50403, N'You don''t have the "Run the floor and unit generators" permission.', 1;
    IF dbo.cpm_fn_CanAccessProject(@RegId, @UserId, @ProjectId) = 0 THROW 50404, N'Project not found.', 1;
    DECLARE @Type nvarchar(20), @PhaseId int, @BuildingNode bigint;
    SELECT @Type = BuildingType, @PhaseId = PhaseId, @BuildingNode = NodeId FROM dbo.cpm_Building WHERE BuildingId = @BuildingId AND ProjectId = @ProjectId AND IsDeleted = 0;
    IF @Type IS NULL THROW 50404, N'Building not found.', 1;
    IF NOT EXISTS (SELECT 1 FROM @Units) THROW 50400, N'There are no units to create.', 1;
    IF @Type IN (N'Tower', N'Block', N'Commercial') AND EXISTS (SELECT 1 FROM @Units WHERE FloorId IS NULL)
        THROW 50400, N'Every unit in a tower, block or commercial building needs a floor.', 1;
    IF @Type IN (N'Villa cluster', N'Plot sector') AND EXISTS (SELECT 1 FROM @Units WHERE FloorId IS NOT NULL)
        THROW 50400, N'Villas and plots sit directly under their cluster or sector, without floors.', 1;
    IF EXISTS (SELECT 1 FROM @Units g WHERE g.FloorId IS NOT NULL AND NOT EXISTS (
                SELECT 1 FROM dbo.cpm_Floor f JOIN dbo.cpm_LevelType l ON l.LevelTypeId = f.LevelTypeId
                WHERE f.FloorId = g.FloorId AND f.BuildingId = @BuildingId AND f.IsDeleted = 0 AND l.IsHabitable = 1))
        THROW 50400, N'Units can only go on habitable floors of this building.', 1;
    IF EXISTS (SELECT 1 FROM @Units g WHERE NOT EXISTS (SELECT 1 FROM dbo.cpm_UnitType t WHERE t.UnitTypeId = g.UnitTypeId AND t.RegId = @RegId AND t.IsActive = 1))
        THROW 50400, N'A unit uses a unit type that doesn''t exist or is inactive.', 1;
    IF EXISTS (SELECT 1 FROM @Units WHERE (CarpetSqm IS NOT NULL AND BuiltUpSqm IS NOT NULL AND CarpetSqm > BuiltUpSqm)
                                       OR (BuiltUpSqm IS NOT NULL AND SuperSqm IS NOT NULL AND BuiltUpSqm > SuperSqm))
        THROW 50400, N'Areas must follow carpet ≤ built-up ≤ super.', 1;
    DECLARE @New TABLE(Seq int IDENTITY(1,1), UnitNo nvarchar(40), FloorId int, Position smallint, UnitTypeId int, Facing nvarchar(20),
                       CarpetSqm decimal(14,4), BuiltUpSqm decimal(14,4), SuperSqm decimal(14,4), BalconySqm decimal(14,4), PlotSqm decimal(14,4), NodeId bigint);
    DECLARE @Nodes TABLE(Seq int IDENTITY(1,1), NodeId bigint);
    DECLARE @Total int = (SELECT COUNT(*) FROM @Units), @Created int;
    BEGIN TRY
    BEGIN TRAN;
    INSERT @New(UnitNo, FloorId, Position, UnitTypeId, Facing, CarpetSqm, BuiltUpSqm, SuperSqm, BalconySqm, PlotSqm)
    SELECT g.UnitNo, g.FloorId, g.Position, g.UnitTypeId, g.Facing, g.CarpetSqm, g.BuiltUpSqm, g.SuperSqm, g.BalconySqm, g.PlotSqm
    FROM @Units g
    WHERE NOT EXISTS (SELECT 1 FROM dbo.cpm_Unit u WITH (UPDLOCK, HOLDLOCK) WHERE u.ProjectId = @ProjectId AND u.UnitNo = g.UnitNo AND u.IsDeleted = 0)
    ORDER BY g.UnitNo;
    SET @Created = @@ROWCOUNT;
    INSERT dbo.cpm_PropertyNode(RegId, ProjectId, NodeType, ParentNodeId)
    OUTPUT inserted.NodeId INTO @Nodes(NodeId)
    SELECT @RegId, @ProjectId, N'Unit', ISNULL(f.NodeId, @BuildingNode)
    FROM @New n LEFT JOIN dbo.cpm_Floor f ON f.FloorId = n.FloorId ORDER BY n.Seq;
    ;WITH nd AS (SELECT NodeId, ROW_NUMBER() OVER (ORDER BY NodeId) AS Seq FROM @Nodes)
    UPDATE n SET NodeId = nd.NodeId FROM @New n JOIN nd ON nd.Seq = n.Seq;
    INSERT dbo.cpm_Unit(RegId, ProjectId, PhaseId, BuildingId, FloorId, NodeId, UnitNo, Position, UnitTypeId, Facing, CarpetSqm, BuiltUpSqm, SuperSqm, BalconySqm, PlotSqm, CreatedBy)
    SELECT @RegId, @ProjectId, @PhaseId, @BuildingId, FloorId, NodeId, UnitNo, Position, UnitTypeId, Facing, CarpetSqm, BuiltUpSqm, SuperSqm, BalconySqm, PlotSqm, @UserId
    FROM @New;
    INSERT dbo.cpm_UnitAvailabilityHistory(RegId, UnitId, FromStatus, ToStatus, Reason, ChangedBy)
    SELECT @RegId, u.UnitId, NULL, N'Available', N'Created by unit generator', @UserId
    FROM dbo.cpm_Unit u JOIN @New n ON n.NodeId = u.NodeId;
    INSERT dbo.cpm_AuditLog(RegId, EntityType, EntityId, Action, NewData, UserId)
    VALUES(@RegId, N'Building', @BuildingId, N'GenerateUnits',
           (SELECT @Created AS Created, @Total - @Created AS Skipped,
                   (SELECT MIN(UnitNo) FROM @New) AS FirstUnit, (SELECT MAX(UnitNo) FROM @New) AS LastUnit FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), @UserId);
    COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
    SELECT @Created AS Created, @Total - @Created AS Skipped;
END
GO
CREATE OR ALTER PROCEDURE dbo.cpm_usp_Unit_Get
    @RegId int, @UserId int, @ProjectId int, @Id int
AS
BEGIN
    SET NOCOUNT ON;
    IF dbo.cpm_fn_CanAccessProject(@RegId, @UserId, @ProjectId) = 0 THROW 50404, N'Project not found.', 1;
    SELECT u.UnitId AS Id, u.UnitNo, u.PhaseId, u.BuildingId, b.Name AS Building, u.FloorId, f.Name AS Floor, u.Position, u.UnitTypeId,
           u.Facing, u.CarpetSqm, u.BuiltUpSqm, u.SuperSqm, u.BalconySqm, u.PlotSqm, u.Availability, CONVERT(varchar(18), CAST(u.RowVersion AS binary(8)), 1) AS RowVersion
    FROM dbo.cpm_Unit u LEFT JOIN dbo.cpm_Building b ON b.BuildingId = u.BuildingId LEFT JOIN dbo.cpm_Floor f ON f.FloorId = u.FloorId
    WHERE u.UnitId = @Id AND u.ProjectId = @ProjectId AND u.IsDeleted = 0;
    SELECT h.HistoryId AS Id, h.FromStatus, h.ToStatus, h.Reason, h.ChangedAtUtc, dbo.cpm_fn_PersonName(h.ChangedBy) AS ChangedBy
    FROM dbo.cpm_UnitAvailabilityHistory h
    JOIN dbo.cpm_Unit u ON u.UnitId = h.UnitId
    WHERE h.UnitId = @Id AND u.ProjectId = @ProjectId ORDER BY h.HistoryId DESC;
    SELECT FromStatus, ToStatus FROM dbo.cpm_AvailabilityTransition;
END
GO
CREATE OR ALTER PROCEDURE dbo.cpm_usp_Unit_Save
    @RegId int, @UserId int, @ProjectId int, @Id int, @UnitNo nvarchar(40), @UnitTypeId int, @Facing nvarchar(20) = NULL,
    @CarpetSqm decimal(14,4) = NULL, @BuiltUpSqm decimal(14,4) = NULL, @SuperSqm decimal(14,4) = NULL, @BalconySqm decimal(14,4) = NULL,
    @PlotSqm decimal(14,4) = NULL, @Reason nvarchar(500) = NULL, @RowVersion binary(8)
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    IF dbo.cpm_fn_HasPermission(@UserId, N'units.edit') = 0 THROW 50403, N'You don''t have the "Edit units" permission.', 1;
    IF dbo.cpm_fn_CanAccessProject(@RegId, @UserId, @ProjectId) = 0 THROW 50404, N'Project not found.', 1;
    IF NOT EXISTS (SELECT 1 FROM dbo.cpm_Unit WHERE UnitId = @Id AND ProjectId = @ProjectId AND IsDeleted = 0) THROW 50404, N'Unit not found.', 1;
    SET @UnitNo = LTRIM(RTRIM(ISNULL(@UnitNo, N'')));
    IF @UnitNo = N'' THROW 50400, N'Enter the unit number.', 1;
    IF EXISTS (SELECT 1 FROM dbo.cpm_Unit WHERE UnitId = @Id AND UnitNo <> @UnitNo) AND LTRIM(RTRIM(ISNULL(@Reason, N''))) = N''
        THROW 50400, N'Enter the reason for changing the unit number.', 1;
    IF EXISTS (SELECT 1 FROM dbo.cpm_Unit WHERE ProjectId = @ProjectId AND UnitNo = @UnitNo AND IsDeleted = 0 AND UnitId <> @Id)
        THROW 50400, N'Another unit in this project already has this number.', 1;
    IF NOT EXISTS (SELECT 1 FROM dbo.cpm_UnitType WHERE UnitTypeId = @UnitTypeId AND RegId = @RegId) THROW 50400, N'Choose a unit type.', 1;
    IF (@CarpetSqm IS NOT NULL AND @BuiltUpSqm IS NOT NULL AND @CarpetSqm > @BuiltUpSqm) OR (@BuiltUpSqm IS NOT NULL AND @SuperSqm IS NOT NULL AND @BuiltUpSqm > @SuperSqm)
        THROW 50400, N'Areas must follow carpet ≤ built-up ≤ super.', 1;
    IF NOT EXISTS (SELECT 1 FROM dbo.cpm_Unit WHERE UnitId = @Id AND RowVersion = @RowVersion) THROW 50409, N'Someone else changed this unit. Reload and try again.', 1;
    DECLARE @Old nvarchar(max) = (SELECT UnitNo, UnitTypeId, Facing, CarpetSqm, BuiltUpSqm, SuperSqm, BalconySqm, PlotSqm FROM dbo.cpm_Unit WHERE UnitId = @Id FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
    BEGIN TRY
    BEGIN TRAN;
    UPDATE dbo.cpm_Unit SET UnitNo = @UnitNo, UnitTypeId = @UnitTypeId, Facing = @Facing, CarpetSqm = @CarpetSqm, BuiltUpSqm = @BuiltUpSqm, SuperSqm = @SuperSqm,
           BalconySqm = @BalconySqm, PlotSqm = @PlotSqm, UpdatedBy = @UserId, UpdatedAtUtc = SYSUTCDATETIME() WHERE UnitId = @Id;
    INSERT dbo.cpm_AuditLog(RegId, EntityType, EntityId, Action, Reason, OldData, NewData, UserId)
    SELECT @RegId, N'Unit', @Id, N'Update', NULLIF(LTRIM(RTRIM(ISNULL(@Reason, N''))), N''), @Old,
           (SELECT UnitNo, UnitTypeId, Facing, CarpetSqm, BuiltUpSqm, SuperSqm, BalconySqm, PlotSqm FROM dbo.cpm_Unit WHERE UnitId = @Id FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), @UserId;
    COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
    SELECT @Id AS Id;
END
GO
CREATE OR ALTER PROCEDURE dbo.cpm_usp_Unit_ChangeAvailability
    @RegId int, @UserId int, @ProjectId int, @Id int, @ToStatus nvarchar(20), @Reason nvarchar(500), @RowVersion binary(8)
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    IF dbo.cpm_fn_HasPermission(@UserId, N'availability.change') = 0 THROW 50403, N'You don''t have the "Change unit availability" permission.', 1;
    IF dbo.cpm_fn_CanAccessProject(@RegId, @UserId, @ProjectId) = 0 THROW 50404, N'Project not found.', 1;
    IF LTRIM(RTRIM(ISNULL(@Reason, N''))) = N'' THROW 50400, N'Enter the reason for the change.', 1;
    DECLARE @From nvarchar(20) = (SELECT Availability FROM dbo.cpm_Unit WHERE UnitId = @Id AND ProjectId = @ProjectId AND IsDeleted = 0);
    IF @From IS NULL THROW 50404, N'Unit not found.', 1;
    IF NOT EXISTS (SELECT 1 FROM dbo.cpm_AvailabilityTransition WHERE FromStatus = @From AND ToStatus = @ToStatus)
    BEGIN
        DECLARE @Msg nvarchar(200) = N'A unit can''t move from ' + @From + N' to ' + ISNULL(@ToStatus, N'(none)') + N'.';
        THROW 50400, @Msg, 1;
    END
    IF NOT EXISTS (SELECT 1 FROM dbo.cpm_Unit WHERE UnitId = @Id AND RowVersion = @RowVersion) THROW 50409, N'Someone else changed this unit. Reload and try again.', 1;
    BEGIN TRY
    BEGIN TRAN;
    UPDATE dbo.cpm_Unit SET Availability = @ToStatus, UpdatedBy = @UserId, UpdatedAtUtc = SYSUTCDATETIME() WHERE UnitId = @Id;
    INSERT dbo.cpm_UnitAvailabilityHistory(RegId, UnitId, FromStatus, ToStatus, Reason, ChangedBy) VALUES(@RegId, @Id, @From, @ToStatus, LTRIM(RTRIM(@Reason)), @UserId);
    INSERT dbo.cpm_AuditLog(RegId, EntityType, EntityId, Action, Reason, OldData, NewData, UserId)
    VALUES(@RegId, N'Unit', @Id, N'Availability', LTRIM(RTRIM(@Reason)),
           (SELECT @From AS Availability FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), (SELECT @ToStatus AS Availability FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), @UserId);
    COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
END
GO
CREATE OR ALTER PROCEDURE dbo.cpm_usp_Unit_Delete
    @RegId int, @UserId int, @ProjectId int, @Ids dbo.cpm_tt_IntList READONLY, @Reason nvarchar(500)
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    IF dbo.cpm_fn_HasPermission(@UserId, N'units.edit') = 0 THROW 50403, N'You don''t have the "Edit units" permission.', 1;
    IF dbo.cpm_fn_CanAccessProject(@RegId, @UserId, @ProjectId) = 0 THROW 50404, N'Project not found.', 1;
    IF LTRIM(RTRIM(ISNULL(@Reason, N''))) = N'' THROW 50400, N'Enter the reason for deleting.', 1;
    IF EXISTS (SELECT 1 FROM @Ids i WHERE NOT EXISTS (SELECT 1 FROM dbo.cpm_Unit u WHERE u.UnitId = i.Id AND u.ProjectId = @ProjectId AND u.IsDeleted = 0))
        THROW 50404, N'A selected unit wasn''t found.', 1;
    IF EXISTS (SELECT 1 FROM dbo.cpm_Unit u JOIN @Ids i ON i.Id = u.UnitId WHERE u.Availability <> N'Available')
        THROW 50400, N'Only Available units can be deleted. Booked, sold or handed-over units must stay.', 1;
    DECLARE @Deleted int;
    BEGIN TRY
    BEGIN TRAN;
    UPDATE u SET IsDeleted = 1, UpdatedBy = @UserId, UpdatedAtUtc = SYSUTCDATETIME() FROM dbo.cpm_Unit u JOIN @Ids i ON i.Id = u.UnitId;
    SET @Deleted = @@ROWCOUNT;
    UPDATE n SET IsDeleted = 1 FROM dbo.cpm_PropertyNode n JOIN dbo.cpm_Unit u ON u.NodeId = n.NodeId JOIN @Ids i ON i.Id = u.UnitId;
    INSERT dbo.cpm_AuditLog(RegId, EntityType, EntityId, Action, Reason, UserId) SELECT @RegId, N'Unit', Id, N'Delete', @Reason, @UserId FROM @Ids;
    COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
    SELECT @Deleted AS Deleted;
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.cpm_SchemaVersion WHERE ScriptName = N'004_structure_units')
    INSERT dbo.cpm_SchemaVersion(ScriptName) VALUES(N'004_structure_units');
GO
SET NOEXEC OFF;
GO
