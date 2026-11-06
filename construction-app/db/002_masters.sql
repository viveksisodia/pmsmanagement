/* =====================================================================
   Construction app · 002 masters (unit types, level types; locations read from the ERP masters)
   Target: absolutebook_2172 only. New cpm_ objects only. Safe to run again.
   ===================================================================== */
SET NOCOUNT ON;
IF DB_NAME() <> N'absolutebook_2172' AND DB_NAME() NOT LIKE N'cpmtest[_]%'
BEGIN
    RAISERROR(N'Stopped: this script may only run in absolutebook_2172.', 16, 1);
    SET NOEXEC ON;
END
GO
IF OBJECT_ID(N'dbo.cpm_SchemaVersion', N'U') IS NULL OR NOT EXISTS (SELECT 1 FROM dbo.cpm_SchemaVersion WHERE ScriptName = N'001_foundation')
BEGIN
    RAISERROR(N'Stopped: run 001_foundation.sql first.', 16, 1);
    SET NOEXEC ON;
END
GO

IF OBJECT_ID(N'dbo.tbl_countrymaster', N'U') IS NULL OR OBJECT_ID(N'dbo.tbl_statemaster', N'U') IS NULL OR OBJECT_ID(N'dbo.tbl_City_master', N'U') IS NULL
BEGIN
    RAISERROR(N'Stopped: tbl_countrymaster, tbl_statemaster and tbl_City_master must exist.', 16, 1);
    SET NOEXEC ON;
END
GO

/* ---------- unit types ---------- */
IF OBJECT_ID(N'dbo.cpm_UnitType', N'U') IS NULL
CREATE TABLE dbo.cpm_UnitType(
    UnitTypeId        int IDENTITY(1,1) NOT NULL CONSTRAINT PK_cpm_UnitType PRIMARY KEY,
    regid             int NOT NULL,
    Code              nvarchar(30)  NOT NULL,
    Name              nvarchar(100) NOT NULL,
    Category          nvarchar(20)  NOT NULL CONSTRAINT CK_cpm_UnitType_Cat CHECK (Category IN (N'Residential', N'Villa', N'Plot', N'Commercial', N'Office', N'Parking', N'Other')),
    Bedrooms          tinyint NULL,
    Bathrooms         tinyint NULL,
    DefaultCarpetSqm  decimal(14,4) NULL,
    DefaultBuiltUpSqm decimal(14,4) NULL,
    DefaultSuperSqm   decimal(14,4) NULL,
    DefaultPlotSqm    decimal(14,4) NULL,
    IsSaleable        bit NOT NULL CONSTRAINT DF_cpm_UnitType_Sale DEFAULT 1,
    IsActive          bit NOT NULL CONSTRAINT DF_cpm_UnitType_Active DEFAULT 1,
    uid int NULL, cdate datetime NOT NULL CONSTRAINT DF_cpm_UnitType_cdate DEFAULT GETDATE(),
    mid int NULL, mdate datetime NULL,
    status  bit NOT NULL CONSTRAINT DF_cpm_UnitType_status DEFAULT 1,
    archive bit NOT NULL CONSTRAINT DF_cpm_UnitType_archive DEFAULT 0,
    RowVersion rowversion NOT NULL,
    CONSTRAINT CK_cpm_UnitType_Areas CHECK ((DefaultCarpetSqm IS NULL OR DefaultBuiltUpSqm IS NULL OR DefaultCarpetSqm <= DefaultBuiltUpSqm)
                                       AND (DefaultBuiltUpSqm IS NULL OR DefaultSuperSqm IS NULL OR DefaultBuiltUpSqm <= DefaultSuperSqm)));
GO
/* ---------- level types ---------- */
IF OBJECT_ID(N'dbo.cpm_LevelType', N'U') IS NULL
CREATE TABLE dbo.cpm_LevelType(
    LevelTypeId int IDENTITY(1,1) NOT NULL CONSTRAINT PK_cpm_LevelType PRIMARY KEY,
    regid       int NOT NULL,
    Code        nvarchar(20) NOT NULL,
    Name        nvarchar(60) NOT NULL,
    IsHabitable bit NOT NULL,
    SortOrder   int NOT NULL,
    IsActive    bit NOT NULL CONSTRAINT DF_cpm_LevelType_Active DEFAULT 1,
    uid int NULL, cdate datetime NOT NULL CONSTRAINT DF_cpm_LevelType_cdate DEFAULT GETDATE(),
    mid int NULL, mdate datetime NULL,
    status  bit NOT NULL CONSTRAINT DF_cpm_LevelType_status DEFAULT 1,
    archive bit NOT NULL CONSTRAINT DF_cpm_LevelType_archive DEFAULT 0,
    RowVersion rowversion NOT NULL);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_cpm_UnitType_Code')
    CREATE UNIQUE INDEX UX_cpm_UnitType_Code ON dbo.cpm_UnitType(regid, Code) WHERE status = 1 AND archive = 0;
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_cpm_LevelType_Code')
    CREATE UNIQUE INDEX UX_cpm_LevelType_Code ON dbo.cpm_LevelType(regid, Code) WHERE status = 1 AND archive = 0;
GO

/* ---------- locations: read from the existing ERP masters ----------
   tbl_countrymaster(id, countryname), tbl_statemaster(id, statename, countryid), tbl_City_master(City_ID, City_name).
   Shared rows have regid NULL or 0; company rows have the company's regid. tbl_City_master has no state column. */
CREATE OR ALTER PROCEDURE dbo.cpm_usp_Location_List
    @RegId int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT c.id AS Id, LTRIM(RTRIM(c.countryname)) AS Name
    FROM dbo.tbl_countrymaster c
    WHERE c.status = 1 AND c.archive = 0 AND (ISNULL(c.regid, 0) = 0 OR c.regid = @RegId)
    ORDER BY c.countryname;
    SELECT s.id AS Id, s.countryid AS CountryId, LTRIM(RTRIM(s.statename)) AS Name
    FROM dbo.tbl_statemaster s
    WHERE s.status = 1 AND s.archive = 0 AND (ISNULL(s.regid, 0) = 0 OR s.regid = @RegId)
    ORDER BY s.statename;
    SELECT ci.City_ID AS Id, LTRIM(RTRIM(ci.City_name)) AS Name
    FROM dbo.tbl_City_master ci
    WHERE ci.Status = 1 AND ci.Archive = 0 AND (ISNULL(ci.regid, 0) = 0 OR ci.regid = @RegId)
    ORDER BY ci.City_name;
END
GO
IF OBJECT_ID(N'dbo.cpm_usp_City_Add', N'P') IS NOT NULL DROP PROCEDURE dbo.cpm_usp_City_Add;
GO

/* ---------- unit type procedures ---------- */
CREATE OR ALTER PROCEDURE dbo.cpm_usp_UnitType_List
    @RegId int, @IncludeInactive bit = 0
AS
BEGIN
    SET NOCOUNT ON;
    SELECT t.UnitTypeId AS Id, t.Code, t.Name, t.Category, t.Bedrooms, t.Bathrooms, t.DefaultCarpetSqm, t.DefaultBuiltUpSqm,
           t.DefaultSuperSqm, t.DefaultPlotSqm, t.IsSaleable, t.IsActive, CONVERT(varchar(18), CAST(t.RowVersion AS binary(8)), 1) AS RowVersion,
           (SELECT COUNT(*) FROM dbo.cpm_Unit u WHERE u.UnitTypeId = t.UnitTypeId AND u.status = 1 AND u.archive = 0) AS UnitsUsing
    FROM dbo.cpm_UnitType t
    WHERE t.regid = @RegId AND t.status = 1 AND t.archive = 0 AND (@IncludeInactive = 1 OR t.IsActive = 1)
    ORDER BY t.Category, t.Name;
END
GO
CREATE OR ALTER PROCEDURE dbo.cpm_usp_UnitType_Save
    @RegId int, @UserId int, @Id int = NULL, @Code nvarchar(30), @Name nvarchar(100), @Category nvarchar(20),
    @Bedrooms tinyint = NULL, @Bathrooms tinyint = NULL, @DefaultCarpetSqm decimal(14,4) = NULL, @DefaultBuiltUpSqm decimal(14,4) = NULL,
    @DefaultSuperSqm decimal(14,4) = NULL, @DefaultPlotSqm decimal(14,4) = NULL, @IsSaleable bit = 1, @IsActive bit = 1,
    @RowVersion binary(8) = NULL
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    IF dbo.cpm_fn_HasPermission(@UserId, N'masters.edit') = 0 THROW 50403, N'You don''t have the "Edit masters" permission.', 1;
    SET @Code = LTRIM(RTRIM(ISNULL(@Code, N''))); SET @Name = LTRIM(RTRIM(ISNULL(@Name, N'')));
    IF @Code = N'' OR @Name = N'' THROW 50400, N'Enter the code and name.', 1;
    IF @Category NOT IN (N'Residential', N'Villa', N'Plot', N'Commercial', N'Office', N'Parking', N'Other') THROW 50400, N'Choose a valid category.', 1;
    IF (@DefaultCarpetSqm IS NOT NULL AND @DefaultBuiltUpSqm IS NOT NULL AND @DefaultCarpetSqm > @DefaultBuiltUpSqm)
       OR (@DefaultBuiltUpSqm IS NOT NULL AND @DefaultSuperSqm IS NOT NULL AND @DefaultBuiltUpSqm > @DefaultSuperSqm)
        THROW 50400, N'Areas must follow carpet ≤ built-up ≤ super.', 1;
    IF EXISTS (SELECT 1 FROM dbo.cpm_UnitType WHERE regid = @RegId AND Code = @Code AND status = 1 AND archive = 0 AND UnitTypeId <> ISNULL(@Id, 0))
        THROW 50400, N'A unit type with this code already exists.', 1;
    DECLARE @Old nvarchar(max), @Action nvarchar(20);
    BEGIN TRY
    BEGIN TRAN;
    IF @Id IS NULL
    BEGIN
        INSERT dbo.cpm_UnitType(regid, Code, Name, Category, Bedrooms, Bathrooms, DefaultCarpetSqm, DefaultBuiltUpSqm, DefaultSuperSqm, DefaultPlotSqm, IsSaleable, IsActive, uid)
        VALUES(@RegId, @Code, @Name, @Category, @Bedrooms, @Bathrooms, @DefaultCarpetSqm, @DefaultBuiltUpSqm, @DefaultSuperSqm, @DefaultPlotSqm, @IsSaleable, @IsActive, @UserId);
        SET @Id = SCOPE_IDENTITY(); SET @Action = N'Create';
    END
    ELSE
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM dbo.cpm_UnitType WHERE UnitTypeId = @Id AND regid = @RegId AND status = 1 AND archive = 0) THROW 50404, N'Unit type not found.', 1;
        IF NOT EXISTS (SELECT 1 FROM dbo.cpm_UnitType WHERE UnitTypeId = @Id AND RowVersion = @RowVersion AND status = 1 AND archive = 0) THROW 50409, N'Someone else changed this unit type. Reload and try again.', 1;
        SET @Old = (SELECT Code, Name, Category, Bedrooms, Bathrooms, DefaultCarpetSqm, DefaultBuiltUpSqm, DefaultSuperSqm, DefaultPlotSqm, IsSaleable, IsActive
                    FROM dbo.cpm_UnitType WHERE UnitTypeId = @Id AND status = 1 AND archive = 0 FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
        UPDATE dbo.cpm_UnitType SET Code = @Code, Name = @Name, Category = @Category, Bedrooms = @Bedrooms, Bathrooms = @Bathrooms,
               DefaultCarpetSqm = @DefaultCarpetSqm, DefaultBuiltUpSqm = @DefaultBuiltUpSqm, DefaultSuperSqm = @DefaultSuperSqm,
               DefaultPlotSqm = @DefaultPlotSqm, IsSaleable = @IsSaleable, IsActive = @IsActive, mid = @UserId, mdate = GETDATE()
        WHERE UnitTypeId = @Id AND status = 1 AND archive = 0;
        SET @Action = N'Update';
    END
    INSERT dbo.cpm_AuditLog(regid, EntityType, EntityId, Action, OldData, NewData, uid)
    SELECT @RegId, N'UnitType', @Id, @Action, @Old,
           (SELECT Code, Name, Category, Bedrooms, Bathrooms, DefaultCarpetSqm, DefaultBuiltUpSqm, DefaultSuperSqm, DefaultPlotSqm, IsSaleable, IsActive
            FROM dbo.cpm_UnitType WHERE UnitTypeId = @Id AND status = 1 AND archive = 0 FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), @UserId;
    COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
    SELECT @Id AS Id;
END
GO

/* ---------- level type procedures ---------- */
CREATE OR ALTER PROCEDURE dbo.cpm_usp_LevelType_List
    @RegId int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT l.LevelTypeId AS Id, l.Code, l.Name, l.IsHabitable, l.SortOrder, l.IsActive, CONVERT(varchar(18), CAST(l.RowVersion AS binary(8)), 1) AS RowVersion,
           (SELECT COUNT(*) FROM dbo.cpm_Floor f WHERE f.LevelTypeId = l.LevelTypeId AND f.status = 1 AND f.archive = 0) AS FloorsUsing
    FROM dbo.cpm_LevelType l WHERE l.regid = @RegId AND l.status = 1 AND l.archive = 0 ORDER BY l.SortOrder, l.Name;
END
GO
CREATE OR ALTER PROCEDURE dbo.cpm_usp_LevelType_Save
    @RegId int, @UserId int, @Id int = NULL, @Code nvarchar(20), @Name nvarchar(60), @IsHabitable bit, @SortOrder int,
    @IsActive bit = 1, @RowVersion binary(8) = NULL
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    IF dbo.cpm_fn_HasPermission(@UserId, N'masters.edit') = 0 THROW 50403, N'You don''t have the "Edit masters" permission.', 1;
    SET @Code = LTRIM(RTRIM(ISNULL(@Code, N''))); SET @Name = LTRIM(RTRIM(ISNULL(@Name, N'')));
    IF @Code = N'' OR @Name = N'' THROW 50400, N'Enter the code and name.', 1;
    IF EXISTS (SELECT 1 FROM dbo.cpm_LevelType WHERE regid = @RegId AND Code = @Code AND status = 1 AND archive = 0 AND LevelTypeId <> ISNULL(@Id, 0))
        THROW 50400, N'A level type with this code already exists.', 1;
    DECLARE @Old nvarchar(max), @Action nvarchar(20);
    BEGIN TRY
    BEGIN TRAN;
    IF @Id IS NULL
    BEGIN
        INSERT dbo.cpm_LevelType(regid, Code, Name, IsHabitable, SortOrder, IsActive, uid) VALUES(@RegId, @Code, @Name, @IsHabitable, @SortOrder, @IsActive, @UserId);
        SET @Id = SCOPE_IDENTITY(); SET @Action = N'Create';
    END
    ELSE
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM dbo.cpm_LevelType WHERE LevelTypeId = @Id AND regid = @RegId AND status = 1 AND archive = 0) THROW 50404, N'Level type not found.', 1;
        IF NOT EXISTS (SELECT 1 FROM dbo.cpm_LevelType WHERE LevelTypeId = @Id AND RowVersion = @RowVersion AND status = 1 AND archive = 0) THROW 50409, N'Someone else changed this level type. Reload and try again.', 1;
        SET @Old = (SELECT Code, Name, IsHabitable, SortOrder, IsActive FROM dbo.cpm_LevelType WHERE LevelTypeId = @Id AND status = 1 AND archive = 0 FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
        UPDATE dbo.cpm_LevelType SET Code = @Code, Name = @Name, IsHabitable = @IsHabitable, SortOrder = @SortOrder, IsActive = @IsActive,
               mid = @UserId, mdate = GETDATE() WHERE LevelTypeId = @Id AND status = 1 AND archive = 0;
        SET @Action = N'Update';
    END
    INSERT dbo.cpm_AuditLog(regid, EntityType, EntityId, Action, OldData, NewData, uid)
    SELECT @RegId, N'LevelType', @Id, @Action, @Old,
           (SELECT Code, Name, IsHabitable, SortOrder, IsActive FROM dbo.cpm_LevelType WHERE LevelTypeId = @Id AND status = 1 AND archive = 0 FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), @UserId;
    COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
    SELECT @Id AS Id;
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.cpm_SchemaVersion WHERE ScriptName = N'002_masters')
    INSERT dbo.cpm_SchemaVersion(ScriptName) VALUES(N'002_masters');
GO
SET NOEXEC OFF;
GO
