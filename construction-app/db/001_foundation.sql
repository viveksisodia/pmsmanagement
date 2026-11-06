/* =====================================================================
   Construction app · 001 foundation
   Target: absolutebook_2172 only (SQL Server 2016 SP1, compatibility 130)

   Uses these EXISTING tables read-only (nothing is added to or changed in them):
     tbl_registrationdetails  company (RegId = tbl_registrationdetails.id)
     tbl_employee             people and sign-in (id, regid, emp_name, Password, status, archive, roleid, isactivelogin)
     Manage_Profile           profiles (tbl_employee.roleid → Manage_Profile.id; id 1 = Admin)
   Creates NEW objects only, all prefixed cpm_. Safe to run again.
   ===================================================================== */
SET NOCOUNT ON;
IF DB_NAME() <> N'absolutebook_2172' AND DB_NAME() NOT LIKE N'cpmtest[_]%'
BEGIN
    RAISERROR(N'Stopped: this script may only run in absolutebook_2172.', 16, 1);
    SET NOEXEC ON;
END
GO
IF OBJECT_ID(N'dbo.tbl_employee', N'U') IS NULL OR OBJECT_ID(N'dbo.tbl_registrationdetails', N'U') IS NULL OR OBJECT_ID(N'dbo.Manage_Profile', N'U') IS NULL
BEGIN
    RAISERROR(N'Stopped: tbl_employee, tbl_registrationdetails and Manage_Profile must exist.', 16, 1);
    SET NOEXEC ON;
END
GO

/* ---------- script log ---------- */
IF OBJECT_ID(N'dbo.cpm_SchemaVersion', N'U') IS NULL
CREATE TABLE dbo.cpm_SchemaVersion(
    ScriptName   nvarchar(100) NOT NULL CONSTRAINT PK_cpm_SchemaVersion PRIMARY KEY,
    AppliedAtUtc datetime2(0)  NOT NULL CONSTRAINT DF_cpm_SchemaVersion_At DEFAULT SYSUTCDATETIME());
GO

/* ---------- permissions ---------- */
IF OBJECT_ID(N'dbo.cpm_Permission', N'U') IS NULL
CREATE TABLE dbo.cpm_Permission(
    Code      nvarchar(60)  NOT NULL CONSTRAINT PK_cpm_Permission PRIMARY KEY,
    Module    nvarchar(40)  NOT NULL,
    Name      nvarchar(150) NOT NULL,
    SortOrder int NOT NULL);
GO
/* Construction permissions per company and Manage_Profile. Profile 1 (Admin) always has every permission. */
IF OBJECT_ID(N'dbo.cpm_ProfilePermission', N'U') IS NULL
CREATE TABLE dbo.cpm_ProfilePermission(
    RegId          int NOT NULL,
    ProfileId      int NOT NULL,          -- Manage_Profile.id
    PermissionCode nvarchar(60) NOT NULL CONSTRAINT FK_cpm_ProfilePerm_Perm REFERENCES dbo.cpm_Permission(Code),
    CreatedBy      int NULL,
    CreatedAtUtc   datetime2(0) NOT NULL CONSTRAINT DF_cpm_ProfilePerm_At DEFAULT SYSUTCDATETIME(),
    CONSTRAINT PK_cpm_ProfilePermission PRIMARY KEY(RegId, ProfileId, PermissionCode));
GO

/* ---------- sign-in support (the app's own; tbl_employee is never written) ---------- */
IF OBJECT_ID(N'dbo.cpm_SignInState', N'U') IS NULL
CREATE TABLE dbo.cpm_SignInState(
    UserId        int NOT NULL CONSTRAINT PK_cpm_SignInState PRIMARY KEY,   -- tbl_employee.id
    FailedCount   int NOT NULL CONSTRAINT DF_cpm_SignInState_Failed DEFAULT 0,
    LockoutEndUtc datetime2(0) NULL,
    LastSignInUtc datetime2(0) NULL);
GO
IF OBJECT_ID(N'dbo.cpm_RefreshToken', N'U') IS NULL
CREATE TABLE dbo.cpm_RefreshToken(
    TokenId        bigint IDENTITY(1,1) NOT NULL CONSTRAINT PK_cpm_RefreshToken PRIMARY KEY,
    UserId         int NOT NULL,                                             -- tbl_employee.id
    TokenHash      varbinary(32) NOT NULL,
    ExpiresAtUtc   datetime2(0) NOT NULL,
    CreatedAtUtc   datetime2(0) NOT NULL CONSTRAINT DF_cpm_RefreshToken_At DEFAULT SYSUTCDATETIME(),
    RevokedAtUtc   datetime2(0) NULL,
    ReplacedByHash varbinary(32) NULL,
    CONSTRAINT UQ_cpm_RefreshToken_Hash UNIQUE(TokenHash));
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_cpm_RefreshToken_User')
    CREATE INDEX IX_cpm_RefreshToken_User ON dbo.cpm_RefreshToken(UserId) WHERE RevokedAtUtc IS NULL;
GO
IF OBJECT_ID(N'dbo.cpm_SignInLog', N'U') IS NULL
CREATE TABLE dbo.cpm_SignInLog(
    SignInLogId bigint IDENTITY(1,1) NOT NULL CONSTRAINT PK_cpm_SignInLog PRIMARY KEY,
    RegId       int NULL, UserId int NULL,
    UserName    nvarchar(200) NOT NULL,
    Succeeded   bit NOT NULL,
    Reason      nvarchar(100) NULL,
    IpAddress   nvarchar(64) NULL,
    AtUtc       datetime2(0) NOT NULL CONSTRAINT DF_cpm_SignInLog_At DEFAULT SYSUTCDATETIME());
GO
IF OBJECT_ID(N'dbo.cpm_AuditLog', N'U') IS NULL
CREATE TABLE dbo.cpm_AuditLog(
    AuditId    bigint IDENTITY(1,1) NOT NULL CONSTRAINT PK_cpm_AuditLog PRIMARY KEY,
    RegId      int NOT NULL,
    EntityType nvarchar(40) NOT NULL,
    EntityId   bigint NOT NULL,
    Action     nvarchar(40) NOT NULL,
    Reason     nvarchar(500) NULL,
    OldData    nvarchar(max) NULL,
    NewData    nvarchar(max) NULL,
    UserId     int NULL,
    AtUtc      datetime2(0) NOT NULL CONSTRAINT DF_cpm_AuditLog_At DEFAULT SYSUTCDATETIME());
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_cpm_AuditLog_Entity')
    CREATE INDEX IX_cpm_AuditLog_Entity ON dbo.cpm_AuditLog(RegId, EntityType, EntityId, AuditId DESC);
GO

/* ---------- table types ---------- */
IF TYPE_ID(N'dbo.cpm_tt_IntList') IS NULL
    CREATE TYPE dbo.cpm_tt_IntList AS TABLE(Id int NOT NULL PRIMARY KEY);
GO
IF TYPE_ID(N'dbo.cpm_tt_CodeList') IS NULL
    CREATE TYPE dbo.cpm_tt_CodeList AS TABLE(Code nvarchar(60) NOT NULL PRIMARY KEY);
GO

/* ---------- permission list ---------- */
MERGE dbo.cpm_Permission AS t
USING (VALUES
 (N'admin.roles',        N'Administration', N'Set construction permissions for profiles', 20),
 (N'audit.view',         N'Administration', N'View audit history',                 30),
 (N'masters.edit',       N'Masters',        N'Edit construction masters',          40),
 (N'land.view',          N'Land',           N'View land parcels',                  50),
 (N'land.edit',          N'Land',           N'Add and edit land parcels',          60),
 (N'projects.view',      N'Projects',       N'View projects they are a member of', 70),
 (N'projects.all',       N'Projects',       N'See every project in the company',   80),
 (N'projects.edit',      N'Projects',       N'Create and edit projects and team',  90),
 (N'structure.edit',     N'Structure',      N'Edit phases, buildings and floors', 100),
 (N'units.generate',     N'Units',          N'Run the floor and unit generators', 110),
 (N'units.edit',         N'Units',          N'Edit units',                        120),
 (N'availability.change',N'Units',          N'Change unit availability',          130)
) AS s(Code, Module, Name, SortOrder)
ON t.Code = s.Code
WHEN MATCHED THEN UPDATE SET Module = s.Module, Name = s.Name, SortOrder = s.SortOrder
WHEN NOT MATCHED THEN INSERT(Code, Module, Name, SortOrder) VALUES(s.Code, s.Module, s.Name, s.SortOrder);
GO
/* admin.users existed in an earlier draft; people are now managed in the ERP. */
DELETE dbo.cpm_ProfilePermission WHERE PermissionCode = N'admin.users';
DELETE dbo.cpm_Permission WHERE Code = N'admin.users';
GO

/* ---------- helpers ---------- */
/* The profile of an employee: tbl_employee.roleid holds a Manage_Profile id as text. */
CREATE OR ALTER FUNCTION dbo.cpm_fn_ProfileOf(@UserId int)
RETURNS int
AS
BEGIN
    RETURN (SELECT TOP 1 TRY_CAST(e.roleid AS int) FROM dbo.tbl_employee e WHERE e.id = @UserId);
END
GO
CREATE OR ALTER FUNCTION dbo.cpm_fn_HasPermission(@UserId int, @Code nvarchar(60))
RETURNS bit
AS
BEGIN
    DECLARE @RegId int, @Profile int;
    SELECT @RegId = e.regid, @Profile = TRY_CAST(e.roleid AS int)
    FROM dbo.tbl_employee e WHERE e.id = @UserId AND e.status = 1 AND ISNULL(e.archive, 0) = 0;
    IF @Profile IS NULL RETURN 0;
    IF @Profile = 1 RETURN 1;   -- ERP Admin profile
    RETURN CASE WHEN EXISTS (SELECT 1 FROM dbo.cpm_ProfilePermission
                             WHERE RegId = @RegId AND ProfileId = @Profile AND PermissionCode = @Code) THEN 1 ELSE 0 END;
END
GO
/* Display name of an employee. */
CREATE OR ALTER FUNCTION dbo.cpm_fn_PersonName(@UserId int)
RETURNS nvarchar(200)
AS
BEGIN
    RETURN (SELECT TOP 1 COALESCE(NULLIF(LTRIM(RTRIM(e.DisplayName)), N''), NULLIF(LTRIM(RTRIM(ISNULL(e.f_name, N'') + N' ' + ISNULL(e.l_name, N''))), N''), e.emp_name)
            FROM dbo.tbl_employee e WHERE e.id = @UserId);
END
GO

/* ---------- sign-in (same username and password as the ERP) ----------
   Mirrors dbo.tbl_commonemployee_login: emp_name match, case-sensitive password match,
   status = 1 and archive = 0. The password is compared inside SQL and never returned.
   Also refuses an employee the ERP has locked (isactivelogin = 2) and the app's own lockout. */
CREATE OR ALTER PROCEDURE dbo.cpm_usp_Auth_SignIn
    @UserName nvarchar(200), @Password nvarchar(200), @IpAddress nvarchar(64) = NULL,
    @MaxFailed int = 5, @LockoutMinutes int = 15
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    DECLARE @Name nvarchar(200) = LTRIM(RTRIM(ISNULL(@UserName, N'')));
    DECLARE @Candidates TABLE(UserId int, RegId int, PwOk bit, ErpLocked bit);
    INSERT @Candidates(UserId, RegId, PwOk, ErpLocked)
    SELECT e.id, e.regid,
           CASE WHEN CAST(e.Password AS varbinary(400)) = CAST(@Password AS varbinary(400)) THEN 1 ELSE 0 END,
           CASE WHEN ISNULL(e.isactivelogin, 0) = 2 THEN 1 ELSE 0 END
    FROM dbo.tbl_employee e
    WHERE e.emp_name = @Name AND e.status = 1 AND ISNULL(e.archive, 0) = 0;

    DECLARE @Result nvarchar(20), @UserId int, @RegId int;
    IF NOT EXISTS (SELECT 1 FROM @Candidates) SET @Result = N'Unknown';
    ELSE IF (SELECT COUNT(*) FROM @Candidates) > 1 SET @Result = N'Ambiguous';
    ELSE
    BEGIN
        SELECT @UserId = UserId, @RegId = RegId FROM @Candidates;
        IF EXISTS (SELECT 1 FROM dbo.cpm_SignInState WHERE UserId = @UserId AND LockoutEndUtc > SYSUTCDATETIME()) SET @Result = N'Locked';
        ELSE IF EXISTS (SELECT 1 FROM @Candidates WHERE ErpLocked = 1) SET @Result = N'ErpLocked';
        ELSE IF EXISTS (SELECT 1 FROM @Candidates WHERE PwOk = 1) SET @Result = N'Ok';
        ELSE SET @Result = N'WrongPassword';
    END

    BEGIN TRY
        BEGIN TRAN;
        IF @UserId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.cpm_SignInState WHERE UserId = @UserId)
            INSERT dbo.cpm_SignInState(UserId) VALUES(@UserId);
        IF @Result = N'Ok'
            UPDATE dbo.cpm_SignInState SET FailedCount = 0, LockoutEndUtc = NULL, LastSignInUtc = SYSUTCDATETIME() WHERE UserId = @UserId;
        ELSE IF @Result = N'WrongPassword'
            UPDATE dbo.cpm_SignInState
               SET FailedCount = FailedCount + 1,
                   LockoutEndUtc = CASE WHEN FailedCount + 1 >= @MaxFailed THEN DATEADD(MINUTE, @LockoutMinutes, SYSUTCDATETIME()) ELSE LockoutEndUtc END
             WHERE UserId = @UserId;
        INSERT dbo.cpm_SignInLog(RegId, UserId, UserName, Succeeded, Reason, IpAddress)
        VALUES(@RegId, @UserId, LEFT(@Name, 200), CASE WHEN @Result = N'Ok' THEN 1 ELSE 0 END, CASE WHEN @Result = N'Ok' THEN NULL ELSE @Result END, @IpAddress);
        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH

    SELECT @Result AS Result, CASE WHEN @Result = N'Ok' THEN @UserId END AS UserId,
           (SELECT LockoutEndUtc FROM dbo.cpm_SignInState WHERE UserId = @UserId) AS LockoutEndUtc;
END
GO
/* Session of a signed-in employee: who they are, their company and construction permissions. */
CREATE OR ALTER PROCEDURE dbo.cpm_usp_Auth_GetSession
    @UserId int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT e.id AS UserId, e.regid AS RegId, ISNULL(e.email_Id, N'') AS Email, dbo.cpm_fn_PersonName(e.id) AS FullName,
           e.emp_name AS UserName, ISNULL(r.company_name, N'') AS TenantName, TRY_CAST(e.roleid AS int) AS ProfileId,
           ISNULL(p.profile, N'') AS ProfileName,
           CAST(CASE WHEN e.status = 1 AND ISNULL(e.archive, 0) = 0 AND ISNULL(e.isactivelogin, 0) <> 2 THEN 1 ELSE 0 END AS bit) AS IsActive
    FROM dbo.tbl_employee e
    LEFT JOIN dbo.tbl_registrationdetails r ON r.id = e.regid
    LEFT JOIN dbo.Manage_Profile p ON p.id = TRY_CAST(e.roleid AS int)
    WHERE e.id = @UserId;
    SELECT p.Code FROM dbo.cpm_Permission p WHERE dbo.cpm_fn_HasPermission(@UserId, p.Code) = 1 ORDER BY p.SortOrder;
END
GO
CREATE OR ALTER PROCEDURE dbo.cpm_usp_RefreshToken_Create
    @UserId int, @TokenHash varbinary(32), @ExpiresAtUtc datetime2(0)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT dbo.cpm_RefreshToken(UserId, TokenHash, ExpiresAtUtc) VALUES(@UserId, @TokenHash, @ExpiresAtUtc);
END
GO
/* Rotates a refresh token. A token used twice (already replaced) revokes every token of that user. */
CREATE OR ALTER PROCEDURE dbo.cpm_usp_RefreshToken_Rotate
    @OldHash varbinary(32), @NewHash varbinary(32), @NewExpiresAtUtc datetime2(0)
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    DECLARE @UserId int, @Revoked datetime2(0), @Expires datetime2(0);
    BEGIN TRY
    BEGIN TRAN;
    SELECT @UserId = UserId, @Revoked = RevokedAtUtc, @Expires = ExpiresAtUtc
    FROM dbo.cpm_RefreshToken WITH (UPDLOCK, HOLDLOCK) WHERE TokenHash = @OldHash;
    IF @UserId IS NULL BEGIN COMMIT; SELECT CAST(NULL AS int) AS UserId; RETURN; END
    IF @Revoked IS NOT NULL
    BEGIN
        UPDATE dbo.cpm_RefreshToken SET RevokedAtUtc = SYSUTCDATETIME() WHERE UserId = @UserId AND RevokedAtUtc IS NULL;
        COMMIT; SELECT CAST(NULL AS int) AS UserId; RETURN;
    END
    IF @Expires <= SYSUTCDATETIME() BEGIN COMMIT; SELECT CAST(NULL AS int) AS UserId; RETURN; END
    UPDATE dbo.cpm_RefreshToken SET RevokedAtUtc = SYSUTCDATETIME(), ReplacedByHash = @NewHash WHERE TokenHash = @OldHash;
    INSERT dbo.cpm_RefreshToken(UserId, TokenHash, ExpiresAtUtc) VALUES(@UserId, @NewHash, @NewExpiresAtUtc);
    COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
    SELECT @UserId AS UserId;
END
GO
CREATE OR ALTER PROCEDURE dbo.cpm_usp_RefreshToken_Revoke
    @TokenHash varbinary(32)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.cpm_RefreshToken SET RevokedAtUtc = SYSUTCDATETIME() WHERE TokenHash = @TokenHash AND RevokedAtUtc IS NULL;
END
GO

/* ---------- people (read-only view of tbl_employee for this company) ---------- */
CREATE OR ALTER PROCEDURE dbo.cpm_usp_People_List
    @RegId int, @UserId int, @Search nvarchar(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @s nvarchar(102) = N'%' + NULLIF(LTRIM(RTRIM(@Search)), N'') + N'%';
    SELECT e.id AS Id, dbo.cpm_fn_PersonName(e.id) AS FullName, e.emp_name AS UserName, ISNULL(e.email_Id, N'') AS Email,
           TRY_CAST(e.roleid AS int) AS ProfileId, ISNULL(p.profile, N'') AS ProfileName, s.LastSignInUtc, s.LockoutEndUtc
    FROM dbo.tbl_employee e
    LEFT JOIN dbo.Manage_Profile p ON p.id = TRY_CAST(e.roleid AS int)
    LEFT JOIN dbo.cpm_SignInState s ON s.UserId = e.id
    WHERE e.regid = @RegId AND e.status = 1 AND ISNULL(e.archive, 0) = 0
      AND (@s IS NULL OR e.emp_name LIKE @s OR e.DisplayName LIKE @s OR e.email_Id LIKE @s)
    ORDER BY FullName;
END
GO
/* Clears the app's own lockout for an employee (an ERP lock stays with the ERP). */
CREATE OR ALTER PROCEDURE dbo.cpm_usp_People_Unlock
    @RegId int, @UserId int, @Id int
AS
BEGIN
    SET NOCOUNT ON;
    IF dbo.cpm_fn_HasPermission(@UserId, N'admin.roles') = 0 THROW 50403, N'You don''t have permission to unlock sign-in.', 1;
    IF NOT EXISTS (SELECT 1 FROM dbo.tbl_employee WHERE id = @Id AND regid = @RegId) THROW 50404, N'Person not found.', 1;
    UPDATE dbo.cpm_SignInState SET FailedCount = 0, LockoutEndUtc = NULL WHERE UserId = @Id;
    INSERT dbo.cpm_AuditLog(RegId, EntityType, EntityId, Action, UserId) VALUES(@RegId, N'Person', @Id, N'Unlock', @UserId);
END
GO

/* ---------- profile permissions ---------- */
CREATE OR ALTER PROCEDURE dbo.cpm_usp_Permission_List
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Code, Module, Name FROM dbo.cpm_Permission ORDER BY SortOrder;
END
GO
CREATE OR ALTER PROCEDURE dbo.cpm_usp_Profile_List
    @RegId int, @UserId int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.id AS Id, ISNULL(NULLIF(p.profile, N''), N'(no name)') AS Name, CAST(CASE WHEN p.id = 1 THEN 1 ELSE 0 END AS bit) AS IsAdmin,
           (SELECT COUNT(*) FROM dbo.tbl_employee e WHERE e.regid = @RegId AND TRY_CAST(e.roleid AS int) = p.id AND e.status = 1 AND ISNULL(e.archive, 0) = 0) AS People,
           CASE WHEN p.id = 1 THEN (SELECT STUFF((SELECT N',' + Code FROM dbo.cpm_Permission ORDER BY SortOrder FOR XML PATH(''), TYPE).value('.', 'nvarchar(max)'), 1, 1, N''))
                ELSE STUFF((SELECT N',' + pp.PermissionCode FROM dbo.cpm_ProfilePermission pp WHERE pp.RegId = @RegId AND pp.ProfileId = p.id
                            FOR XML PATH(''), TYPE).value('.', 'nvarchar(max)'), 1, 1, N'') END AS Permissions
    FROM dbo.Manage_Profile p
    WHERE (p.regid IS NULL OR p.regid = @RegId) AND p.status = 1 AND ISNULL(p.archive, 0) = 0
    ORDER BY CASE WHEN p.id = 1 THEN 0 ELSE 1 END, p.profile;
END
GO
CREATE OR ALTER PROCEDURE dbo.cpm_usp_Profile_SavePermissions
    @RegId int, @UserId int, @ProfileId int, @Permissions dbo.cpm_tt_CodeList READONLY
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    IF dbo.cpm_fn_HasPermission(@UserId, N'admin.roles') = 0 THROW 50403, N'You don''t have the "Set construction permissions" permission.', 1;
    IF @ProfileId = 1 THROW 50400, N'The Admin profile always has every permission.', 1;
    IF NOT EXISTS (SELECT 1 FROM dbo.Manage_Profile WHERE id = @ProfileId AND (regid IS NULL OR regid = @RegId) AND status = 1 AND ISNULL(archive, 0) = 0)
        THROW 50404, N'Profile not found.', 1;
    IF EXISTS (SELECT 1 FROM @Permissions p WHERE NOT EXISTS (SELECT 1 FROM dbo.cpm_Permission x WHERE x.Code = p.Code))
        THROW 50400, N'A selected permission doesn''t exist.', 1;
    IF @ProfileId = dbo.cpm_fn_ProfileOf(@UserId) AND NOT EXISTS (SELECT 1 FROM @Permissions WHERE Code = N'admin.roles')
        THROW 50400, N'You can''t remove "Set construction permissions" from your own profile.', 1;
    DECLARE @Old nvarchar(max) = (SELECT PermissionCode FROM dbo.cpm_ProfilePermission WHERE RegId = @RegId AND ProfileId = @ProfileId FOR JSON PATH);
    BEGIN TRY
    BEGIN TRAN;
    DELETE dbo.cpm_ProfilePermission WHERE RegId = @RegId AND ProfileId = @ProfileId;
    INSERT dbo.cpm_ProfilePermission(RegId, ProfileId, PermissionCode, CreatedBy) SELECT @RegId, @ProfileId, Code, @UserId FROM @Permissions;
    INSERT dbo.cpm_AuditLog(RegId, EntityType, EntityId, Action, OldData, NewData, UserId)
    VALUES(@RegId, N'Profile', @ProfileId, N'Permissions', @Old,
           (SELECT PermissionCode FROM dbo.cpm_ProfilePermission WHERE RegId = @RegId AND ProfileId = @ProfileId FOR JSON PATH), @UserId);
    COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
END
GO

/* ---------- audit ---------- */
CREATE OR ALTER PROCEDURE dbo.cpm_usp_Audit_List
    @RegId int, @UserId int, @EntityType nvarchar(40), @EntityId bigint
AS
BEGIN
    SET NOCOUNT ON;
    IF dbo.cpm_fn_HasPermission(@UserId, N'audit.view') = 0 THROW 50403, N'You don''t have the "View audit history" permission.', 1;
    SELECT a.AuditId AS Id, a.Action, a.Reason, a.OldData, a.NewData, a.AtUtc, dbo.cpm_fn_PersonName(a.UserId) AS UserName
    FROM dbo.cpm_AuditLog a
    WHERE a.RegId = @RegId AND a.EntityType = @EntityType AND a.EntityId = @EntityId
    ORDER BY a.AuditId DESC;
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.cpm_SchemaVersion WHERE ScriptName = N'001_foundation')
    INSERT dbo.cpm_SchemaVersion(ScriptName) VALUES(N'001_foundation');
GO
SET NOEXEC OFF;
GO
