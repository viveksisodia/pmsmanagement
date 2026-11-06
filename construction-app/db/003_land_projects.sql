/* =====================================================================
   Construction app · 003 land parcels, projects, project team
   Target: absolutebook_2172 only. New cpm_ objects only. Safe to run again.
   Every table has regid, cdate, uid, status, archive (mid, mdate for changes).
   Reads use status = 1 AND archive = 0; deleting sets status = 0, archive = 1.
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
    regid            int NOT NULL,
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
    uid     int NULL,
    cdate   datetime NOT NULL CONSTRAINT DF_cpm_LandParcel_cdate DEFAULT GETDATE(),
    mid     int NULL, mdate datetime NULL,
    status  bit NOT NULL CONSTRAINT DF_cpm_LandParcel_status DEFAULT 1,
    archive bit NOT NULL CONSTRAINT DF_cpm_LandParcel_archive DEFAULT 0,
    RowVersion rowversion NOT NULL);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_cpm_LandParcel_Survey')
    CREATE UNIQUE INDEX UX_cpm_LandParcel_Survey ON dbo.cpm_LandParcel(regid, StateId, SurveyNo) WHERE status = 1 AND archive = 0;
GO
IF OBJECT_ID(N'dbo.cpm_LandParcelOwner', N'U') IS NULL
CREATE TABLE dbo.cpm_LandParcelOwner(
    OwnerId      int IDENTITY(1,1) NOT NULL CONSTRAINT PK_cpm_LandParcelOwner PRIMARY KEY,
    regid        int NOT NULL,
    LandParcelId int NOT NULL CONSTRAINT FK_cpm_LandOwner_Parcel REFERENCES dbo.cpm_LandParcel(LandParcelId),
    OwnerName    nvarchar(200) NOT NULL,
    SharePct     decimal(5,2) NOT NULL CONSTRAINT CK_cpm_LandOwner_Share CHECK (SharePct > 0 AND SharePct <= 100),
    uid     int NULL,
    cdate   datetime NOT NULL CONSTRAINT DF_cpm_LandOwner_cdate DEFAULT GETDATE(),
    mid     int NULL, mdate datetime NULL,
    status  bit NOT NULL CONSTRAINT DF_cpm_LandOwner_status DEFAULT 1,
    archive bit NOT NULL CONSTRAINT DF_cpm_LandOwner_archive DEFAULT 0);
GO
IF TYPE_ID(N'dbo.cpm_tt_Owner') IS NULL
    CREATE TYPE dbo.cpm_tt_Owner AS TABLE(OwnerName nvarchar(200) NOT NULL, SharePct decimal(5,2) NOT NULL);
GO

/* ---------- projects ---------- */
IF OBJECT_ID(N'dbo.cpm_PropertyNode', N'U') IS NULL
CREATE TABLE dbo.cpm_PropertyNode(
    NodeId       bigint IDENTITY(1,1) NOT NULL CONSTRAINT PK_cpm_PropertyNode PRIMARY KEY,
    regid        int NOT NULL,
    ProjectId    int NULL,
    NodeType     nvarchar(20) NOT NULL CONSTRAINT CK_cpm_Node_Type CHECK (NodeType IN (N'Project', N'Phase', N'Building', N'Floor', N'Unit')),
    ParentNodeId bigint NULL CONSTRAINT FK_cpm_Node_Parent REFERENCES dbo.cpm_PropertyNode(NodeId),
    uid     int NULL,
    cdate   datetime NOT NULL CONSTRAINT DF_cpm_Node_cdate DEFAULT GETDATE(),
    mid     int NULL, mdate datetime NULL,
    status  bit NOT NULL CONSTRAINT DF_cpm_Node_status DEFAULT 1,
    archive bit NOT NULL CONSTRAINT DF_cpm_Node_archive DEFAULT 0);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_cpm_PropertyNode_Parent')
    CREATE INDEX IX_cpm_PropertyNode_Parent ON dbo.cpm_PropertyNode(regid, ParentNodeId) WHERE status = 1 AND archive = 0;
GO
IF OBJECT_ID(N'dbo.cpm_Project', N'U') IS NULL
CREATE TABLE dbo.cpm_Project(
    ProjectId         int IDENTITY(1,1) NOT NULL CONSTRAINT PK_cpm_Project PRIMARY KEY,
    regid             int NOT NULL,
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
    ProjectStatus     nvarchar(20)  NOT NULL CONSTRAINT CK_cpm_Project_ProjectStatus CHECK (ProjectStatus IN (N'Planning', N'Active', N'On hold', N'Completed')),
    uid     int NULL,
    cdate   datetime NOT NULL CONSTRAINT DF_cpm_Project_cdate DEFAULT GETDATE(),
    mid     int NULL, mdate datetime NULL,
    status  bit NOT NULL CONSTRAINT DF_cpm_Project_status DEFAULT 1,
    archive bit NOT NULL CONSTRAINT DF_cpm_Project_archive DEFAULT 0,
    RowVersion rowversion NOT NULL,
    CONSTRAINT CK_cpm_Project_Dates CHECK (StartDate IS NULL OR TargetDate IS NULL OR StartDate <= TargetDate));
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_cpm_Project_Code')
    CREATE UNIQUE INDEX UX_cpm_Project_Code ON dbo.cpm_Project(regid, Code) WHERE status = 1 AND archive = 0;
GO
IF OBJECT_ID(N'dbo.cpm_ProjectLandParcel', N'U') IS NULL
CREATE TABLE dbo.cpm_ProjectLandParcel(
    id           int IDENTITY(1,1) NOT NULL CONSTRAINT PK_cpm_ProjectLandParcel PRIMARY KEY,
    regid        int NOT NULL,
    ProjectId    int NOT NULL CONSTRAINT FK_cpm_ProjLand_Project REFERENCES dbo.cpm_Project(ProjectId),
    LandParcelId int NOT NULL CONSTRAINT FK_cpm_ProjLand_Parcel REFERENCES dbo.cpm_LandParcel(LandParcelId),
    AreaUsedSqm  decimal(16,4) NOT NULL CONSTRAINT CK_cpm_ProjLand_Area CHECK (AreaUsedSqm > 0),
    uid     int NULL,
    cdate   datetime NOT NULL CONSTRAINT DF_cpm_ProjLand_cdate DEFAULT GETDATE(),
    mid     int NULL, mdate datetime NULL,
    status  bit NOT NULL CONSTRAINT DF_cpm_ProjLand_status DEFAULT 1,
    archive bit NOT NULL CONSTRAINT DF_cpm_ProjLand_archive DEFAULT 0);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_cpm_ProjectLandParcel')
    CREATE UNIQUE INDEX UX_cpm_ProjectLandParcel ON dbo.cpm_ProjectLandParcel(ProjectId, LandParcelId) WHERE status = 1 AND archive = 0;
GO
IF OBJECT_ID(N'dbo.cpm_ProjectMember', N'U') IS NULL
CREATE TABLE dbo.cpm_ProjectMember(
    id           int IDENTITY(1,1) NOT NULL CONSTRAINT PK_cpm_ProjectMember PRIMARY KEY,
    regid        int NOT NULL,
    ProjectId    int NOT NULL CONSTRAINT FK_cpm_ProjMember_Project REFERENCES dbo.cpm_Project(ProjectId),
    UserId       int NOT NULL,                 -- tbl_employee.id
    uid     int NULL,
    cdate   datetime NOT NULL CONSTRAINT DF_cpm_ProjMember_cdate DEFAULT GETDATE(),
    mid     int NULL, mdate datetime NULL,
    status  bit NOT NULL CONSTRAINT DF_cpm_ProjMember_status DEFAULT 1,
    archive bit NOT NULL CONSTRAINT DF_cpm_ProjMember_archive DEFAULT 0);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_cpm_ProjectMember')
    CREATE UNIQUE INDEX UX_cpm_ProjectMember ON dbo.cpm_ProjectMember(ProjectId, UserId) WHERE status = 1 AND archive = 0;
GO
IF TYPE_ID(N'dbo.cpm_tt_ProjectLand') IS NULL
    CREATE TYPE dbo.cpm_tt_ProjectLand AS TABLE(LandParcelId int NOT NULL PRIMARY KEY, AreaUsedSqm decimal(16,4) NOT NULL);
GO

/* Project access: "See every project" permission, or membership of the project. */
CREATE OR ALTER FUNCTION dbo.cpm_fn_CanAccessProject(@RegId int, @UserId int, @ProjectId int)
RETURNS bit
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM dbo.cpm_Project WHERE ProjectId = @ProjectId AND regid = @RegId AND status = 1 AND archive = 0) RETURN 0;
    IF dbo.cpm_fn_HasPermission(@UserId, N'projects.all') = 1 RETURN 1;
    IF dbo.cpm_fn_HasPermission(@UserId, N'projects.view') = 1
       AND EXISTS (SELECT 1 FROM dbo.cpm_ProjectMember WHERE ProjectId = @ProjectId AND UserId = @UserId AND status = 1 AND archive = 0) RETURN 1;
    RETURN 0;
END
GO
/* Land used by current projects, per parcel (optionally leaving one project out). */
CREATE OR ALTER FUNCTION dbo.cpm_fn_LandUsedSqm(@LandParcelId int, @ExceptProjectId int)
RETURNS decimal(16,4)
AS
BEGIN
    RETURN ISNULL((SELECT SUM(pl.AreaUsedSqm)
                   FROM dbo.cpm_ProjectLandParcel pl JOIN dbo.cpm_Project pr ON pr.ProjectId = pl.ProjectId
                   WHERE pl.LandParcelId = @LandParcelId AND pl.ProjectId <> ISNULL(@ExceptProjectId, 0)
                     AND pl.status = 1 AND pl.archive = 0 AND pr.status = 1 AND pr.archive = 0), 0);
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
           dbo.cpm_fn_LandUsedSqm(p.LandParcelId, NULL) AS AreaUsedSqm,
           STUFF((SELECT N', ' + pr.Name FROM dbo.cpm_ProjectLandParcel pl JOIN dbo.cpm_Project pr ON pr.ProjectId = pl.ProjectId
                  WHERE pl.LandParcelId = p.LandParcelId AND pl.status = 1 AND pl.archive = 0 AND pr.status = 1 AND pr.archive = 0
                  FOR XML PATH(''), TYPE).value('.', 'nvarchar(max)'), 1, 2, N'') AS Projects
    FROM dbo.cpm_LandParcel p
    LEFT JOIN dbo.tbl_statemaster s ON s.id = p.StateId AND s.status = 1 AND s.archive = 0
    LEFT JOIN dbo.tbl_City_master c ON c.City_ID = p.CityId AND c.Status = 1 AND c.Archive = 0
    WHERE p.regid = @RegId AND p.status = 1 AND p.archive = 0
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
    FROM dbo.cpm_LandParcel p LEFT JOIN dbo.tbl_statemaster st ON st.id = p.StateId AND st.status = 1 AND st.archive = 0
    WHERE p.regid = @RegId AND p.LandParcelId = @Id AND p.status = 1 AND p.archive = 0;
    SELECT o.OwnerName, o.SharePct FROM dbo.cpm_LandParcelOwner o
    WHERE o.regid = @RegId AND o.LandParcelId = @Id AND o.status = 1 AND o.archive = 0 ORDER BY o.OwnerId;
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
    IF NOT EXISTS (SELECT 1 FROM dbo.tbl_statemaster WHERE id = @StateId AND status = 1 AND archive = 0 AND (ISNULL(regid, 0) = 0 OR regid = @RegId))
        THROW 50400, N'Choose a state.', 1;
    IF @CityId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.tbl_City_master WHERE City_ID = @CityId AND Status = 1 AND Archive = 0 AND (ISNULL(regid, 0) = 0 OR regid = @RegId))
        THROW 50400, N'Choose a city from the list.', 1;
    IF @ResponsibleUserId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.tbl_employee WHERE id = @ResponsibleUserId AND regid = @RegId AND status = 1 AND archive = 0)
        THROW 50400, N'Choose a valid responsible person.', 1;
    IF EXISTS (SELECT 1 FROM @Owners) AND (SELECT SUM(SharePct) FROM @Owners) <> 100
        THROW 50400, N'Owner shares must total 100 percent.', 1;
    IF EXISTS (SELECT 1 FROM dbo.cpm_LandParcel WHERE regid = @RegId AND StateId = @StateId AND SurveyNo = @SurveyNo AND status = 1 AND archive = 0 AND LandParcelId <> ISNULL(@Id, 0))
        THROW 50400, N'This survey number already exists in this state.', 1;
    IF @Id IS NOT NULL AND @AreaSqm < dbo.cpm_fn_LandUsedSqm(@Id, NULL)
        THROW 50400, N'The area can''t be less than the area already used by projects.', 1;
    DECLARE @Old nvarchar(max), @Action nvarchar(20);
    BEGIN TRY
    BEGIN TRAN;
    IF @Id IS NULL
    BEGIN
        INSERT dbo.cpm_LandParcel(regid, SurveyNo, Locality, CityId, StateId, AreaSqm, AreaEntered, AreaUnitEntered, Tenure, RegistrationNo, RegistrationDate,
                                  RegistryOffice, LandUse, Encumbrance, Latitude, Longitude, ResponsibleUserId, Remarks, uid)
        VALUES(@RegId, @SurveyNo, @Locality, @CityId, @StateId, @AreaSqm, @AreaEntered, @AreaUnitEntered, @Tenure, @RegistrationNo, @RegistrationDate,
               @RegistryOffice, @LandUse, @Encumbrance, @Latitude, @Longitude, @ResponsibleUserId, @Remarks, @UserId);
        SET @Id = SCOPE_IDENTITY(); SET @Action = N'Create';
    END
    ELSE
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM dbo.cpm_LandParcel WHERE LandParcelId = @Id AND regid = @RegId AND status = 1 AND archive = 0) THROW 50404, N'Land parcel not found.', 1;
        IF NOT EXISTS (SELECT 1 FROM dbo.cpm_LandParcel WHERE LandParcelId = @Id AND RowVersion = @RowVersion AND status = 1 AND archive = 0) THROW 50409, N'Someone else changed this parcel. Reload and try again.', 1;
        SET @Old = (SELECT p.*, (SELECT OwnerName, SharePct FROM dbo.cpm_LandParcelOwner WHERE LandParcelId = p.LandParcelId AND status = 1 AND archive = 0 FOR JSON PATH) AS Owners
                    FROM (SELECT SurveyNo, Locality, CityId, StateId, AreaSqm, Tenure, RegistrationNo, RegistrationDate, RegistryOffice, LandUse, Encumbrance,
                                 Latitude, Longitude, ResponsibleUserId, Remarks, LandParcelId FROM dbo.cpm_LandParcel WHERE LandParcelId = @Id AND status = 1 AND archive = 0) p
                    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
        UPDATE dbo.cpm_LandParcel SET SurveyNo = @SurveyNo, Locality = @Locality, CityId = @CityId, StateId = @StateId, AreaSqm = @AreaSqm,
               AreaEntered = @AreaEntered, AreaUnitEntered = @AreaUnitEntered, Tenure = @Tenure, RegistrationNo = @RegistrationNo,
               RegistrationDate = @RegistrationDate, RegistryOffice = @RegistryOffice, LandUse = @LandUse, Encumbrance = @Encumbrance,
               Latitude = @Latitude, Longitude = @Longitude, ResponsibleUserId = @ResponsibleUserId, Remarks = @Remarks,
               mid = @UserId, mdate = GETDATE()
        WHERE LandParcelId = @Id AND status = 1 AND archive = 0;
        /* the owner list is replaced: the old rows are archived, not deleted */
        UPDATE dbo.cpm_LandParcelOwner SET status = 0, archive = 1, mid = @UserId, mdate = GETDATE()
        WHERE LandParcelId = @Id AND status = 1 AND archive = 0;
        SET @Action = N'Update';
    END
    INSERT dbo.cpm_LandParcelOwner(regid, LandParcelId, OwnerName, SharePct, uid)
    SELECT @RegId, @Id, LTRIM(RTRIM(OwnerName)), SharePct, @UserId FROM @Owners WHERE LTRIM(RTRIM(OwnerName)) <> N'';
    INSERT dbo.cpm_AuditLog(regid, EntityType, EntityId, Action, OldData, NewData, uid)
    SELECT @RegId, N'LandParcel', @Id, @Action, @Old,
           (SELECT p.*, (SELECT OwnerName, SharePct FROM dbo.cpm_LandParcelOwner WHERE LandParcelId = p.LandParcelId AND status = 1 AND archive = 0 FOR JSON PATH) AS Owners
            FROM (SELECT SurveyNo, Locality, CityId, StateId, AreaSqm, Tenure, RegistrationNo, RegistrationDate, RegistryOffice, LandUse, Encumbrance,
                         Latitude, Longitude, ResponsibleUserId, Remarks, LandParcelId FROM dbo.cpm_LandParcel WHERE LandParcelId = @Id AND status = 1 AND archive = 0) p
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
    IF NOT EXISTS (SELECT 1 FROM dbo.cpm_LandParcel WHERE LandParcelId = @Id AND regid = @RegId AND status = 1 AND archive = 0) THROW 50404, N'Land parcel not found.', 1;
    IF dbo.cpm_fn_LandUsedSqm(@Id, NULL) > 0
        THROW 50400, N'This parcel is linked to a project. Unlink it from the project first.', 1;
    BEGIN TRY
    BEGIN TRAN;
    UPDATE dbo.cpm_LandParcel SET status = 0, archive = 1, mid = @UserId, mdate = GETDATE() WHERE LandParcelId = @Id AND status = 1 AND archive = 0;
    UPDATE dbo.cpm_LandParcelOwner SET status = 0, archive = 1, mid = @UserId, mdate = GETDATE() WHERE LandParcelId = @Id AND status = 1 AND archive = 0;
    INSERT dbo.cpm_AuditLog(regid, EntityType, EntityId, Action, Reason, uid) VALUES(@RegId, N'LandParcel', @Id, N'Delete', @Reason, @UserId);
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
    SELECT p.ProjectId AS Id, p.Code, p.Name, p.DeveloperEntity, p.ProjectType, p.ProjectStatus AS Status, c.City_name AS City, p.StartDate, p.TargetDate,
           (SELECT COUNT(*) FROM dbo.cpm_Building b WHERE b.ProjectId = p.ProjectId AND b.status = 1 AND b.archive = 0) AS Buildings,
           (SELECT COUNT(*) FROM dbo.cpm_Unit u WHERE u.ProjectId = p.ProjectId AND u.status = 1 AND u.archive = 0) AS Units,
           ISNULL((SELECT SUM(pl.AreaUsedSqm) FROM dbo.cpm_ProjectLandParcel pl WHERE pl.ProjectId = p.ProjectId AND pl.status = 1 AND pl.archive = 0), 0) AS LandSqm
    FROM dbo.cpm_Project p LEFT JOIN dbo.tbl_City_master c ON c.City_ID = p.CityId AND c.Status = 1 AND c.Archive = 0
    WHERE p.regid = @RegId AND p.status = 1 AND p.archive = 0 AND dbo.cpm_fn_CanAccessProject(@RegId, @UserId, p.ProjectId) = 1
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
           p.Address, p.CityId, p.StateId, st.countryid AS CountryId, p.StartDate, p.TargetDate, p.UnitNumberPattern, p.SkipFloorNumbers,
           p.ProjectStatus AS Status, CONVERT(varchar(18), CAST(p.RowVersion AS binary(8)), 1) AS RowVersion
    FROM dbo.cpm_Project p LEFT JOIN dbo.tbl_statemaster st ON st.id = p.StateId AND st.status = 1 AND st.archive = 0
    WHERE p.ProjectId = @Id AND p.regid = @RegId AND p.status = 1 AND p.archive = 0;
    SELECT pl.LandParcelId, lp.SurveyNo, lp.AreaSqm AS ParcelAreaSqm, pl.AreaUsedSqm,
           lp.AreaSqm - dbo.cpm_fn_LandUsedSqm(pl.LandParcelId, @Id) AS AvailableSqm
    FROM dbo.cpm_ProjectLandParcel pl JOIN dbo.cpm_LandParcel lp ON lp.LandParcelId = pl.LandParcelId AND lp.status = 1 AND lp.archive = 0
    WHERE pl.ProjectId = @Id AND pl.status = 1 AND pl.archive = 0 ORDER BY lp.SurveyNo;
    SELECT m.UserId, dbo.cpm_fn_PersonName(m.UserId) AS FullName, ISNULL(e.email_Id, N'') AS Email
    FROM dbo.cpm_ProjectMember m JOIN dbo.tbl_employee e ON e.id = m.UserId AND e.status = 1 AND e.archive = 0
    WHERE m.ProjectId = @Id AND m.status = 1 AND m.archive = 0 ORDER BY FullName;
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
    IF @StateId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.tbl_statemaster WHERE id = @StateId AND status = 1 AND archive = 0 AND (ISNULL(regid, 0) = 0 OR regid = @RegId))
        THROW 50400, N'Choose a state from the list.', 1;
    IF @CityId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.tbl_City_master WHERE City_ID = @CityId AND Status = 1 AND Archive = 0 AND (ISNULL(regid, 0) = 0 OR regid = @RegId))
        THROW 50400, N'Choose a city from the list.', 1;
    IF EXISTS (SELECT 1 FROM dbo.cpm_Project WHERE regid = @RegId AND Code = @Code AND status = 1 AND archive = 0 AND ProjectId <> ISNULL(@Id, 0))
        THROW 50400, N'A project with this code already exists.', 1;
    IF EXISTS (SELECT 1 FROM @Land l WHERE NOT EXISTS (SELECT 1 FROM dbo.cpm_LandParcel p WHERE p.LandParcelId = l.LandParcelId AND p.regid = @RegId AND p.status = 1 AND p.archive = 0))
        THROW 50400, N'A linked land parcel doesn''t exist.', 1;
    IF EXISTS (SELECT 1 FROM @Land WHERE AreaUsedSqm <= 0) THROW 50400, N'Area used must be greater than zero.', 1;
    IF EXISTS (SELECT 1 FROM @MemberIds m WHERE NOT EXISTS (SELECT 1 FROM dbo.tbl_employee u WHERE u.id = m.Id AND u.regid = @RegId AND u.status = 1 AND u.archive = 0))
        THROW 50400, N'A selected team member doesn''t exist.', 1;
    IF @Id IS NOT NULL AND @UsesPhases = 0 AND EXISTS (SELECT 1 FROM dbo.cpm_Phase WHERE ProjectId = @Id AND status = 1 AND archive = 0)
        THROW 50400, N'This project already has phases. Delete them before switching phases off.', 1;
    IF @Id IS NOT NULL AND @UsesPhases = 1 AND EXISTS (SELECT 1 FROM dbo.cpm_Building WHERE ProjectId = @Id AND PhaseId IS NULL AND status = 1 AND archive = 0)
        THROW 50400, N'Some buildings have no phase. Assign them to a phase before switching phases on.', 1;
    DECLARE @Old nvarchar(max), @Action nvarchar(20), @NodeId bigint, @Over nvarchar(200);
    BEGIN TRY
    BEGIN TRAN;
    /* land over-use: other projects' use + this project's use must not exceed the parcel */
    SELECT TOP 1 @Over = lp.SurveyNo
    FROM @Land l JOIN dbo.cpm_LandParcel lp WITH (UPDLOCK) ON lp.LandParcelId = l.LandParcelId AND lp.status = 1 AND lp.archive = 0
    WHERE l.AreaUsedSqm + dbo.cpm_fn_LandUsedSqm(l.LandParcelId, @Id) > lp.AreaSqm;
    IF @Over IS NOT NULL
    BEGIN
        SET @Over = N'Area used on survey ' + @Over + N' is more than what is left on that parcel.';
        THROW 50400, @Over, 1;
    END
    IF @Id IS NULL
    BEGIN
        INSERT dbo.cpm_PropertyNode(regid, NodeType, uid) VALUES(@RegId, N'Project', @UserId);
        SET @NodeId = SCOPE_IDENTITY();
        INSERT dbo.cpm_Project(regid, NodeId, Code, Name, DeveloperEntity, ProjectType, UsesPhases, AreaUnit, TimeZoneId, CurrencyCode, Address, CityId, StateId,
                               StartDate, TargetDate, UnitNumberPattern, SkipFloorNumbers, ProjectStatus, uid)
        VALUES(@RegId, @NodeId, @Code, @Name, @DeveloperEntity, @ProjectType, @UsesPhases, @AreaUnit, @TimeZoneId, @CurrencyCode, @Address, @CityId, @StateId,
               @StartDate, @TargetDate, @UnitNumberPattern, @SkipFloorNumbers, @Status, @UserId);
        SET @Id = SCOPE_IDENTITY(); SET @Action = N'Create';
        UPDATE dbo.cpm_PropertyNode SET ProjectId = @Id WHERE NodeId = @NodeId;
        IF NOT EXISTS (SELECT 1 FROM @MemberIds WHERE Id = @UserId)
            INSERT dbo.cpm_ProjectMember(regid, ProjectId, UserId, uid) VALUES(@RegId, @Id, @UserId, @UserId);
    END
    ELSE
    BEGIN
        IF dbo.cpm_fn_CanAccessProject(@RegId, @UserId, @Id) = 0 THROW 50404, N'Project not found.', 1;
        IF NOT EXISTS (SELECT 1 FROM dbo.cpm_Project WHERE ProjectId = @Id AND RowVersion = @RowVersion AND status = 1 AND archive = 0) THROW 50409, N'Someone else changed this project. Reload and try again.', 1;
        SET @Old = (SELECT p.Code, p.Name, p.DeveloperEntity, p.ProjectType, p.UsesPhases, p.AreaUnit, p.TimeZoneId, p.CurrencyCode, p.Address, p.CityId, p.StateId,
                           p.StartDate, p.TargetDate, p.UnitNumberPattern, p.SkipFloorNumbers, p.ProjectStatus,
                           (SELECT LandParcelId, AreaUsedSqm FROM dbo.cpm_ProjectLandParcel WHERE ProjectId = p.ProjectId AND status = 1 AND archive = 0 FOR JSON PATH) AS Land,
                           (SELECT UserId FROM dbo.cpm_ProjectMember WHERE ProjectId = p.ProjectId AND status = 1 AND archive = 0 FOR JSON PATH) AS Members
                    FROM dbo.cpm_Project p WHERE p.ProjectId = @Id AND p.status = 1 AND p.archive = 0 FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
        UPDATE dbo.cpm_Project SET Code = @Code, Name = @Name, DeveloperEntity = @DeveloperEntity, ProjectType = @ProjectType, UsesPhases = @UsesPhases,
               AreaUnit = @AreaUnit, TimeZoneId = @TimeZoneId, CurrencyCode = @CurrencyCode, Address = @Address, CityId = @CityId, StateId = @StateId,
               StartDate = @StartDate, TargetDate = @TargetDate, UnitNumberPattern = @UnitNumberPattern, SkipFloorNumbers = @SkipFloorNumbers,
               ProjectStatus = @Status, mid = @UserId, mdate = GETDATE()
        WHERE ProjectId = @Id AND status = 1 AND archive = 0;
        /* land links and team: archive what was removed or changed, keep what is unchanged */
        UPDATE pl SET status = 0, archive = 1, mid = @UserId, mdate = GETDATE()
        FROM dbo.cpm_ProjectLandParcel pl
        WHERE pl.ProjectId = @Id AND pl.status = 1 AND pl.archive = 0
          AND NOT EXISTS (SELECT 1 FROM @Land l WHERE l.LandParcelId = pl.LandParcelId AND l.AreaUsedSqm = pl.AreaUsedSqm);
        UPDATE m SET status = 0, archive = 1, mid = @UserId, mdate = GETDATE()
        FROM dbo.cpm_ProjectMember m
        WHERE m.ProjectId = @Id AND m.status = 1 AND m.archive = 0 AND NOT EXISTS (SELECT 1 FROM @MemberIds x WHERE x.Id = m.UserId);
        SET @Action = N'Update';
    END
    INSERT dbo.cpm_ProjectLandParcel(regid, ProjectId, LandParcelId, AreaUsedSqm, uid)
    SELECT @RegId, @Id, l.LandParcelId, l.AreaUsedSqm, @UserId FROM @Land l
    WHERE NOT EXISTS (SELECT 1 FROM dbo.cpm_ProjectLandParcel pl WHERE pl.ProjectId = @Id AND pl.LandParcelId = l.LandParcelId AND pl.status = 1 AND pl.archive = 0);
    INSERT dbo.cpm_ProjectMember(regid, ProjectId, UserId, uid)
    SELECT @RegId, @Id, x.Id, @UserId FROM @MemberIds x
    WHERE NOT EXISTS (SELECT 1 FROM dbo.cpm_ProjectMember m WHERE m.ProjectId = @Id AND m.UserId = x.Id AND m.status = 1 AND m.archive = 0);
    INSERT dbo.cpm_AuditLog(regid, EntityType, EntityId, Action, OldData, NewData, uid)
    SELECT @RegId, N'Project', @Id, @Action, @Old,
           (SELECT p.Code, p.Name, p.DeveloperEntity, p.ProjectType, p.UsesPhases, p.AreaUnit, p.TimeZoneId, p.CurrencyCode, p.Address, p.CityId, p.StateId,
                   p.StartDate, p.TargetDate, p.UnitNumberPattern, p.SkipFloorNumbers, p.ProjectStatus,
                   (SELECT LandParcelId, AreaUsedSqm FROM dbo.cpm_ProjectLandParcel WHERE ProjectId = p.ProjectId AND status = 1 AND archive = 0 FOR JSON PATH) AS Land,
                   (SELECT UserId FROM dbo.cpm_ProjectMember WHERE ProjectId = p.ProjectId AND status = 1 AND archive = 0 FOR JSON PATH) AS Members
            FROM dbo.cpm_Project p WHERE p.ProjectId = @Id AND p.status = 1 AND p.archive = 0 FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), @UserId;
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
    IF EXISTS (SELECT 1 FROM dbo.cpm_Phase WHERE ProjectId = @Id AND status = 1 AND archive = 0) OR EXISTS (SELECT 1 FROM dbo.cpm_Building WHERE ProjectId = @Id AND status = 1 AND archive = 0)
        THROW 50400, N'The project still has phases or buildings. Delete them first.', 1;
    BEGIN TRY
    BEGIN TRAN;
    UPDATE dbo.cpm_PropertyNode SET status = 0, archive = 1, mid = @UserId, mdate = GETDATE() WHERE NodeId = (SELECT NodeId FROM dbo.cpm_Project WHERE ProjectId = @Id AND status = 1 AND archive = 0);
    UPDATE dbo.cpm_Project SET status = 0, archive = 1, mid = @UserId, mdate = GETDATE() WHERE ProjectId = @Id AND status = 1 AND archive = 0;
    UPDATE dbo.cpm_ProjectLandParcel SET status = 0, archive = 1, mid = @UserId, mdate = GETDATE() WHERE ProjectId = @Id AND status = 1 AND archive = 0;
    UPDATE dbo.cpm_ProjectMember SET status = 0, archive = 1, mid = @UserId, mdate = GETDATE() WHERE ProjectId = @Id AND status = 1 AND archive = 0;
    INSERT dbo.cpm_AuditLog(regid, EntityType, EntityId, Action, Reason, uid) VALUES(@RegId, N'Project', @Id, N'Delete', @Reason, @UserId);
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
    FROM dbo.tbl_employee e WHERE e.regid = @RegId AND e.status = 1 AND e.archive = 0 ORDER BY FullName;
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.cpm_SchemaVersion WHERE ScriptName = N'003_land_projects')
    INSERT dbo.cpm_SchemaVersion(ScriptName) VALUES(N'003_land_projects');
GO
SET NOEXEC OFF;
GO
