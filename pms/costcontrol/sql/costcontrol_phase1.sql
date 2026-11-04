/* =====================================================================
   PMS Cost Control - Phase 1 database script
   ---------------------------------------------------------------------
   Purpose : Cost-code library, versioned project budgets (locked
             baseline + revisions), cost entries (actuals) and a
             budget-vs-actual report.
   Safety  : ADDITIVE ONLY.
             - Creates 4 new tables (only if they do not exist).
             - Creates/alters only procedures named usp_pms_cc_*.
             - Does NOT alter, drop or truncate any existing table,
               column or stored procedure.
             - Every row carries regid (tenant isolation) and uses the
               existing soft-delete convention (status=1 / archive=0).
   Reads   : Pms_tbl_AddProject (id, ProjectName, regid, status, archive)
             Pms_tbl_AddMilestone (id, milestone, ProjectId, regid, ...)
             Pms_tbl_Addtask (id, taskname, ProjectId, Milestoneid, ...)
   Run on  : a NON-PRODUCTION copy first. Take a backup before running.
   ===================================================================== */

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

/* ---------------------------------------------------------------------
   1. Tables
   --------------------------------------------------------------------- */
IF OBJECT_ID('dbo.Pms_tbl_CostCode', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Pms_tbl_CostCode
    (
        id           INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Pms_tbl_CostCode PRIMARY KEY,
        regid        INT            NOT NULL,
        Code         NVARCHAR(30)   NOT NULL,
        Name         NVARCHAR(200)  NOT NULL,
        CostType     NVARCHAR(30)   NOT NULL,   -- Material / Labour / Subcontract / Equipment / Overhead / Other
        Description  NVARCHAR(1000) NULL,
        status       INT            NOT NULL CONSTRAINT DF_Pms_tbl_CostCode_status  DEFAULT (1),
        archive      INT            NOT NULL CONSTRAINT DF_Pms_tbl_CostCode_archive DEFAULT (0),
        cby          INT            NULL,
        cdate        DATETIME       NOT NULL CONSTRAINT DF_Pms_tbl_CostCode_cdate DEFAULT (GETDATE()),
        mby          INT            NULL,
        mdate        DATETIME       NULL
    );
    CREATE INDEX IX_Pms_tbl_CostCode_regid ON dbo.Pms_tbl_CostCode (regid, status, archive, Code);
END
GO

IF OBJECT_ID('dbo.Pms_tbl_ProjectBudgetVersion', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Pms_tbl_ProjectBudgetVersion
    (
        id            INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Pms_tbl_ProjectBudgetVersion PRIMARY KEY,
        regid         INT            NOT NULL,
        ProjectId     INT            NOT NULL,   -- Pms_tbl_AddProject.id
        VersionNo     INT            NOT NULL,
        VersionType   NVARCHAR(20)   NOT NULL,   -- Original / Revision
        VersionStatus NVARCHAR(20)   NOT NULL,   -- Draft / Locked
        Reason        NVARCHAR(1000) NULL,
        LockedBy      INT            NULL,
        LockedOn      DATETIME       NULL,
        status        INT            NOT NULL CONSTRAINT DF_Pms_tbl_ProjectBudgetVersion_status  DEFAULT (1),
        archive       INT            NOT NULL CONSTRAINT DF_Pms_tbl_ProjectBudgetVersion_archive DEFAULT (0),
        cby           INT            NULL,
        cdate         DATETIME       NOT NULL CONSTRAINT DF_Pms_tbl_ProjectBudgetVersion_cdate DEFAULT (GETDATE())
    );
    CREATE INDEX IX_Pms_tbl_ProjectBudgetVersion_project ON dbo.Pms_tbl_ProjectBudgetVersion (regid, ProjectId, status, archive, VersionNo);
END
GO

IF OBJECT_ID('dbo.Pms_tbl_ProjectBudgetLine', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Pms_tbl_ProjectBudgetLine
    (
        id           INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Pms_tbl_ProjectBudgetLine PRIMARY KEY,
        regid        INT            NOT NULL,
        VersionId    INT            NOT NULL,   -- Pms_tbl_ProjectBudgetVersion.id
        ProjectId    INT            NOT NULL,   -- Pms_tbl_AddProject.id
        MilestoneId  INT            NULL,       -- Pms_tbl_AddMilestone.id (optional)
        TaskId       INT            NULL,       -- Pms_tbl_Addtask.id (optional)
        CostCodeId   INT            NOT NULL,   -- Pms_tbl_CostCode.id
        Quantity     DECIMAL(18,4)  NULL,
        Unit         NVARCHAR(50)   NULL,
        Rate         DECIMAL(18,4)  NULL,
        Amount       DECIMAL(18,2)  NOT NULL,
        Remarks      NVARCHAR(1000) NULL,
        status       INT            NOT NULL CONSTRAINT DF_Pms_tbl_ProjectBudgetLine_status  DEFAULT (1),
        archive      INT            NOT NULL CONSTRAINT DF_Pms_tbl_ProjectBudgetLine_archive DEFAULT (0),
        cby          INT            NULL,
        cdate        DATETIME       NOT NULL CONSTRAINT DF_Pms_tbl_ProjectBudgetLine_cdate DEFAULT (GETDATE()),
        mby          INT            NULL,
        mdate        DATETIME       NULL
    );
    CREATE INDEX IX_Pms_tbl_ProjectBudgetLine_version ON dbo.Pms_tbl_ProjectBudgetLine (regid, VersionId, status, archive);
END
GO

-- UnitId links a budget line to the existing unit master (tbl_unitmaster.id, read only).
-- Unit keeps the unit name as saved, so history stays readable if the master changes.
-- Added to the NEW cost-control table only; tbl_unitmaster itself is not changed.
IF COL_LENGTH('dbo.Pms_tbl_ProjectBudgetLine', 'UnitId') IS NULL
    ALTER TABLE dbo.Pms_tbl_ProjectBudgetLine ADD UnitId INT NULL;
GO
-- Unit holds tbl_unitmaster.unitname (up to 50 characters).
IF COL_LENGTH('dbo.Pms_tbl_ProjectBudgetLine', 'Unit') < 100
    ALTER TABLE dbo.Pms_tbl_ProjectBudgetLine ALTER COLUMN Unit NVARCHAR(50) NULL;
GO

IF OBJECT_ID('dbo.Pms_tbl_ProjectCostEntry', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Pms_tbl_ProjectCostEntry
    (
        id           INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Pms_tbl_ProjectCostEntry PRIMARY KEY,
        regid        INT            NOT NULL,
        ProjectId    INT            NOT NULL,
        MilestoneId  INT            NULL,
        TaskId       INT            NULL,
        CostCodeId   INT            NOT NULL,
        EntryDate    DATE           NOT NULL,   -- business date (date-only)
        Amount       DECIMAL(18,2)  NOT NULL,
        SourceType   NVARCHAR(30)   NOT NULL CONSTRAINT DF_Pms_tbl_ProjectCostEntry_source DEFAULT (N'Manual'),
        ReferenceNo  NVARCHAR(100)  NULL,
        Remarks      NVARCHAR(1000) NULL,
        status       INT            NOT NULL CONSTRAINT DF_Pms_tbl_ProjectCostEntry_status  DEFAULT (1),
        archive      INT            NOT NULL CONSTRAINT DF_Pms_tbl_ProjectCostEntry_archive DEFAULT (0),
        cby          INT            NULL,
        cdate        DATETIME       NOT NULL CONSTRAINT DF_Pms_tbl_ProjectCostEntry_cdate DEFAULT (GETDATE()),
        mby          INT            NULL,
        mdate        DATETIME       NULL
    );
    CREATE INDEX IX_Pms_tbl_ProjectCostEntry_project ON dbo.Pms_tbl_ProjectCostEntry (regid, ProjectId, status, archive, EntryDate);
END
GO

/* ---------------------------------------------------------------------
   2. Helper: validate project / milestone / task belong together
   Returns 1 when valid, else 0. Read-only on existing tables.
   --------------------------------------------------------------------- */
IF OBJECT_ID('dbo.usp_pms_cc_ValidateHierarchy', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_ValidateHierarchy AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_ValidateHierarchy
    @regid INT, @ProjectId INT, @MilestoneId INT = NULL, @TaskId INT = NULL, @IsValid BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @IsValid = 0;
    IF NOT EXISTS (SELECT 1 FROM dbo.Pms_tbl_AddProject WITH (NOLOCK)
                   WHERE id = @ProjectId AND regid = @regid AND status = 1 AND archive = 0) RETURN;
    IF @MilestoneId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.Pms_tbl_AddMilestone WITH (NOLOCK)
                   WHERE id = @MilestoneId AND ProjectId = @ProjectId AND regid = @regid AND status = 1 AND archive = 0) RETURN;
    IF @TaskId IS NOT NULL AND (@MilestoneId IS NULL OR NOT EXISTS (SELECT 1 FROM dbo.Pms_tbl_Addtask WITH (NOLOCK)
                   WHERE id = @TaskId AND ProjectId = @ProjectId AND Milestoneid = @MilestoneId AND regid = @regid AND status = 1 AND archive = 0)) RETURN;
    SET @IsValid = 1;
END
GO

/* ---------------------------------------------------------------------
   3. Cost codes
   --------------------------------------------------------------------- */
IF OBJECT_ID('dbo.usp_pms_cc_CostCode_List', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_CostCode_List AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_CostCode_List @regid INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT c.id, c.Code, c.Name, c.CostType, ISNULL(c.Description, N'') AS Description,
           CASE WHEN EXISTS (SELECT 1 FROM dbo.Pms_tbl_ProjectBudgetLine b WHERE b.CostCodeId = c.id AND b.regid = c.regid AND b.status = 1 AND b.archive = 0)
                  OR EXISTS (SELECT 1 FROM dbo.Pms_tbl_ProjectCostEntry e WHERE e.CostCodeId = c.id AND e.regid = c.regid AND e.status = 1 AND e.archive = 0)
                THEN 1 ELSE 0 END AS IsUsed
    FROM dbo.Pms_tbl_CostCode c WITH (NOLOCK)
    WHERE c.regid = @regid AND c.status = 1 AND c.archive = 0
    ORDER BY c.Code;
END
GO

IF OBJECT_ID('dbo.usp_pms_cc_CostCode_Save', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_CostCode_Save AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_CostCode_Save
    @regid INT, @id INT, @Code NVARCHAR(30), @Name NVARCHAR(200), @CostType NVARCHAR(30),
    @Description NVARCHAR(1000) = NULL, @uid INT
AS
BEGIN
    SET NOCOUNT ON;
    SET @Code = LTRIM(RTRIM(ISNULL(@Code, N''))); SET @Name = LTRIM(RTRIM(ISNULL(@Name, N'')));
    IF @Code = N'' OR @Name = N''
        BEGIN SELECT 0 AS Result, N'Code and name are required.' AS Message; RETURN; END
    IF @CostType NOT IN (N'Material', N'Labour', N'Subcontract', N'Equipment', N'Overhead', N'Other')
        BEGIN SELECT 0 AS Result, N'Select a valid cost type.' AS Message; RETURN; END
    IF EXISTS (SELECT 1 FROM dbo.Pms_tbl_CostCode WHERE regid = @regid AND status = 1 AND archive = 0
               AND Code = @Code AND id <> ISNULL(@id, 0))
        BEGIN SELECT 0 AS Result, N'This cost code already exists.' AS Message; RETURN; END

    IF ISNULL(@id, 0) = 0
    BEGIN
        INSERT INTO dbo.Pms_tbl_CostCode (regid, Code, Name, CostType, Description, cby, cdate)
        VALUES (@regid, @Code, @Name, @CostType, @Description, @uid, GETDATE());
        SELECT 1 AS Result, N'Saved.' AS Message, CAST(SCOPE_IDENTITY() AS INT) AS id;
    END
    ELSE
    BEGIN
        UPDATE dbo.Pms_tbl_CostCode
           SET Code = @Code, Name = @Name, CostType = @CostType, Description = @Description, mby = @uid, mdate = GETDATE()
         WHERE id = @id AND regid = @regid AND status = 1 AND archive = 0;
        IF @@ROWCOUNT = 1 SELECT 1 AS Result, N'Saved.' AS Message, @id AS id;
        ELSE SELECT 0 AS Result, N'Cost code was not found.' AS Message;
    END
END
GO

IF OBJECT_ID('dbo.usp_pms_cc_CostCode_Delete', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_CostCode_Delete AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_CostCode_Delete @regid INT, @id INT, @uid INT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM dbo.Pms_tbl_ProjectBudgetLine WHERE CostCodeId = @id AND regid = @regid AND status = 1 AND archive = 0)
       OR EXISTS (SELECT 1 FROM dbo.Pms_tbl_ProjectCostEntry WHERE CostCodeId = @id AND regid = @regid AND status = 1 AND archive = 0)
        BEGIN SELECT 0 AS Result, N'This cost code is used in budgets or cost entries and cannot be deleted.' AS Message; RETURN; END
    UPDATE dbo.Pms_tbl_CostCode SET status = 0, archive = 1, mby = @uid, mdate = GETDATE()
     WHERE id = @id AND regid = @regid AND status = 1 AND archive = 0;
    IF @@ROWCOUNT = 1 SELECT 1 AS Result, N'Deleted.' AS Message;
    ELSE SELECT 0 AS Result, N'Cost code was not found.' AS Message;
END
GO

/* ---------------------------------------------------------------------
   4. Project lookups (read-only on existing tables)
   --------------------------------------------------------------------- */
IF OBJECT_ID('dbo.usp_pms_cc_Project_List', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_Project_List AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_Project_List @regid INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.id, ISNULL(p.ProjectName, N'') AS ProjectName
    FROM dbo.Pms_tbl_AddProject p WITH (NOLOCK)
    WHERE p.regid = @regid AND p.status = 1 AND p.archive = 0
    ORDER BY p.ProjectName;
END
GO

IF OBJECT_ID('dbo.usp_pms_cc_ProjectStructure', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_ProjectStructure AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_ProjectStructure @regid INT, @ProjectId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT m.id, ISNULL(m.milestone, N'') AS MilestoneName
    FROM dbo.Pms_tbl_AddMilestone m WITH (NOLOCK)
    WHERE m.regid = @regid AND m.ProjectId = @ProjectId AND m.status = 1 AND m.archive = 0
    ORDER BY m.orderno, m.id;

    SELECT t.id, t.Milestoneid AS MilestoneId, ISNULL(t.taskname, N'') AS TaskName
    FROM dbo.Pms_tbl_Addtask t WITH (NOLOCK)
    WHERE t.regid = @regid AND t.ProjectId = @ProjectId AND t.status = 1 AND t.archive = 0
    ORDER BY t.orderno, t.id;
END
GO

IF OBJECT_ID('dbo.usp_pms_cc_Unit_List', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_Unit_List AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_Unit_List @regid INT
AS
BEGIN
    SET NOCOUNT ON;
    -- Existing unit master (also used by accounting/Tally); read only.
    SELECT u.id, u.unitname AS UnitName
    FROM dbo.tbl_unitmaster u WITH (NOLOCK)
    WHERE u.regid = @regid AND ISNULL(u.status, 1) = 1 AND ISNULL(u.archive, 0) = 0
    ORDER BY u.unitname;
END
GO

IF OBJECT_ID('dbo.usp_pms_cc_Unit_Add', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_Unit_Add AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_Unit_Add @regid INT, @UnitName NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SET @UnitName = LTRIM(RTRIM(ISNULL(@UnitName, N'')));
    IF @UnitName = N'' BEGIN SELECT 0 AS Result, N'Enter a unit name.' AS Message; RETURN; END

    DECLARE @UnitId INT;
    SELECT TOP 1 @UnitId = id FROM dbo.tbl_unitmaster
     WHERE regid = @regid AND unitname = @UnitName AND ISNULL(status, 1) = 1 AND ISNULL(archive, 0) = 0;
    IF @UnitId IS NOT NULL BEGIN SELECT 1 AS Result, N'This unit already exists and has been selected.' AS Message, @UnitId AS id; RETURN; END

    IF EXISTS (SELECT 1 FROM dbo.tbl_unitmaster WHERE regid = @regid AND unitname = @UnitName)
        BEGIN SELECT 0 AS Result, N'This unit exists but is inactive in the unit master. Reactivate it there.' AS Message; RETURN; END

    -- Reuse the existing ERP insert procedure so unit-master and Tally behaviour stay unchanged.
    EXEC dbo.tbl_unitmasterInsertNew @regid = @regid, @unitname = @UnitName;

    SELECT TOP 1 @UnitId = id FROM dbo.tbl_unitmaster
     WHERE regid = @regid AND unitname = @UnitName AND ISNULL(status, 1) = 1 AND ISNULL(archive, 0) = 0
     ORDER BY id DESC;
    IF @UnitId IS NULL SELECT 0 AS Result, N'The unit could not be added.' AS Message;
    ELSE SELECT 1 AS Result, N'Unit added.' AS Message, @UnitId AS id;
END
GO

/* ---------------------------------------------------------------------
   5. Budget versions and lines
   --------------------------------------------------------------------- */
IF OBJECT_ID('dbo.usp_pms_cc_Budget_Get', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_Budget_Get AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_Budget_Get @regid INT, @ProjectId INT, @VersionId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    -- Table 0: all versions of the project
    SELECT v.id, v.VersionNo, v.VersionType, v.VersionStatus, ISNULL(v.Reason, N'') AS Reason,
           v.LockedOn, v.cdate,
           (SELECT ISNULL(SUM(b.Amount), 0) FROM dbo.Pms_tbl_ProjectBudgetLine b
             WHERE b.VersionId = v.id AND b.regid = v.regid AND b.status = 1 AND b.archive = 0) AS TotalAmount
    FROM dbo.Pms_tbl_ProjectBudgetVersion v WITH (NOLOCK)
    WHERE v.regid = @regid AND v.ProjectId = @ProjectId AND v.status = 1 AND v.archive = 0
    ORDER BY v.VersionNo DESC;

    IF @VersionId IS NULL
        SELECT TOP 1 @VersionId = id FROM dbo.Pms_tbl_ProjectBudgetVersion
         WHERE regid = @regid AND ProjectId = @ProjectId AND status = 1 AND archive = 0
         ORDER BY VersionNo DESC;

    -- Table 1: lines of the selected version
    SELECT b.id, b.VersionId, b.MilestoneId, ISNULL(m.milestone, N'') AS MilestoneName,
           b.TaskId, ISNULL(t.taskname, N'') AS TaskName,
           b.CostCodeId, ISNULL(c.Code, N'') AS CostCode, ISNULL(c.Name, N'') AS CostCodeName, ISNULL(c.CostType, N'') AS CostType,
           b.Quantity, b.UnitId, ISNULL(u.unitname, ISNULL(b.Unit, N'')) AS Unit, b.Rate, b.Amount, ISNULL(b.Remarks, N'') AS Remarks
    FROM dbo.Pms_tbl_ProjectBudgetLine b WITH (NOLOCK)
    INNER JOIN dbo.Pms_tbl_ProjectBudgetVersion v WITH (NOLOCK) ON v.id = b.VersionId AND v.regid = b.regid
    LEFT JOIN dbo.Pms_tbl_CostCode c WITH (NOLOCK) ON c.id = b.CostCodeId AND c.regid = b.regid
    LEFT JOIN dbo.Pms_tbl_AddMilestone m WITH (NOLOCK) ON m.id = b.MilestoneId AND m.regid = b.regid
    LEFT JOIN dbo.Pms_tbl_Addtask t WITH (NOLOCK) ON t.id = b.TaskId AND t.regid = b.regid
    LEFT JOIN dbo.tbl_unitmaster u WITH (NOLOCK) ON u.id = b.UnitId AND u.regid = b.regid
    WHERE b.regid = @regid AND b.ProjectId = @ProjectId AND b.VersionId = @VersionId
      AND b.status = 1 AND b.archive = 0
    ORDER BY c.Code, m.milestone, t.taskname, b.id;
END
GO

IF OBJECT_ID('dbo.usp_pms_cc_BudgetVersion_Create', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_BudgetVersion_Create AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_BudgetVersion_Create
    @regid INT, @ProjectId INT, @Reason NVARCHAR(1000) = NULL, @uid INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @IsValid BIT;
    EXEC dbo.usp_pms_cc_ValidateHierarchy @regid, @ProjectId, NULL, NULL, @IsValid OUTPUT;
    IF @IsValid = 0 BEGIN SELECT 0 AS Result, N'Project was not found.' AS Message; RETURN; END

    IF EXISTS (SELECT 1 FROM dbo.Pms_tbl_ProjectBudgetVersion WHERE regid = @regid AND ProjectId = @ProjectId
               AND VersionStatus = N'Draft' AND status = 1 AND archive = 0)
        BEGIN SELECT 0 AS Result, N'A draft budget already exists. Lock or finish it first.' AS Message; RETURN; END

    DECLARE @LastLockedId INT, @NextNo INT, @NewId INT;
    SELECT TOP 1 @LastLockedId = id FROM dbo.Pms_tbl_ProjectBudgetVersion
     WHERE regid = @regid AND ProjectId = @ProjectId AND VersionStatus = N'Locked' AND status = 1 AND archive = 0
     ORDER BY VersionNo DESC;
    SELECT @NextNo = ISNULL(MAX(VersionNo), 0) + 1 FROM dbo.Pms_tbl_ProjectBudgetVersion
     WHERE regid = @regid AND ProjectId = @ProjectId AND status = 1 AND archive = 0;

    IF @LastLockedId IS NOT NULL AND LTRIM(RTRIM(ISNULL(@Reason, N''))) = N''
        BEGIN SELECT 0 AS Result, N'A reason is required for a budget revision.' AS Message; RETURN; END

    SET XACT_ABORT ON;
    BEGIN TRANSACTION;
        INSERT INTO dbo.Pms_tbl_ProjectBudgetVersion (regid, ProjectId, VersionNo, VersionType, VersionStatus, Reason, cby, cdate)
        VALUES (@regid, @ProjectId, @NextNo,
                CASE WHEN @LastLockedId IS NULL THEN N'Original' ELSE N'Revision' END,
                N'Draft', NULLIF(LTRIM(RTRIM(@Reason)), N''), @uid, GETDATE());
        SET @NewId = CAST(SCOPE_IDENTITY() AS INT);

        -- A revision starts as a copy of the last locked version.
        IF @LastLockedId IS NOT NULL
            INSERT INTO dbo.Pms_tbl_ProjectBudgetLine (regid, VersionId, ProjectId, MilestoneId, TaskId, CostCodeId, Quantity, UnitId, Unit, Rate, Amount, Remarks, cby, cdate)
            SELECT regid, @NewId, ProjectId, MilestoneId, TaskId, CostCodeId, Quantity, UnitId, Unit, Rate, Amount, Remarks, @uid, GETDATE()
              FROM dbo.Pms_tbl_ProjectBudgetLine
             WHERE VersionId = @LastLockedId AND regid = @regid AND status = 1 AND archive = 0;
    COMMIT TRANSACTION;

    SELECT 1 AS Result, N'Draft budget created.' AS Message, @NewId AS id;
END
GO

IF OBJECT_ID('dbo.usp_pms_cc_BudgetLine_Save', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_BudgetLine_Save AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_BudgetLine_Save
    @regid INT, @id INT, @VersionId INT, @MilestoneId INT = NULL, @TaskId INT = NULL, @CostCodeId INT,
    @Quantity DECIMAL(18,4) = NULL, @UnitId INT = NULL, @Rate DECIMAL(18,4) = NULL,
    @Amount DECIMAL(18,2) = NULL, @Remarks NVARCHAR(1000) = NULL, @uid INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ProjectId INT, @VersionStatus NVARCHAR(20), @IsValid BIT;
    SELECT @ProjectId = ProjectId, @VersionStatus = VersionStatus FROM dbo.Pms_tbl_ProjectBudgetVersion
     WHERE id = @VersionId AND regid = @regid AND status = 1 AND archive = 0;
    IF @ProjectId IS NULL BEGIN SELECT 0 AS Result, N'Budget version was not found.' AS Message; RETURN; END
    IF @VersionStatus <> N'Draft' BEGIN SELECT 0 AS Result, N'A locked budget cannot be changed. Create a revision.' AS Message; RETURN; END

    EXEC dbo.usp_pms_cc_ValidateHierarchy @regid, @ProjectId, @MilestoneId, @TaskId, @IsValid OUTPUT;
    IF @IsValid = 0 BEGIN SELECT 0 AS Result, N'Milestone or task does not belong to this project.' AS Message; RETURN; END
    IF NOT EXISTS (SELECT 1 FROM dbo.Pms_tbl_CostCode WHERE id = @CostCodeId AND regid = @regid AND status = 1 AND archive = 0)
        BEGIN SELECT 0 AS Result, N'Select an active cost code.' AS Message; RETURN; END

    -- Unit comes from the existing unit master (tbl_unitmaster); its name is kept on the line as saved.
    DECLARE @UnitName NVARCHAR(50) = NULL;
    IF @UnitId IS NOT NULL
    BEGIN
        SELECT @UnitName = unitname FROM dbo.tbl_unitmaster WITH (NOLOCK)
         WHERE id = @UnitId AND regid = @regid AND ISNULL(status, 1) = 1 AND ISNULL(archive, 0) = 0;
        IF @UnitName IS NULL BEGIN SELECT 0 AS Result, N'Select an active unit.' AS Message; RETURN; END
    END

    -- When quantity and rate are both given, the amount is always quantity x rate.
    IF @Quantity IS NOT NULL AND @Rate IS NOT NULL SET @Amount = ROUND(@Quantity * @Rate, 2);
    IF @Amount IS NULL OR @Amount < 0 BEGIN SELECT 0 AS Result, N'Enter a budget amount of zero or more.' AS Message; RETURN; END

    IF ISNULL(@id, 0) = 0
    BEGIN
        INSERT INTO dbo.Pms_tbl_ProjectBudgetLine (regid, VersionId, ProjectId, MilestoneId, TaskId, CostCodeId, Quantity, UnitId, Unit, Rate, Amount, Remarks, cby, cdate)
        VALUES (@regid, @VersionId, @ProjectId, @MilestoneId, @TaskId, @CostCodeId, @Quantity, @UnitId, @UnitName, @Rate, @Amount, NULLIF(LTRIM(RTRIM(@Remarks)), N''), @uid, GETDATE());
        SELECT 1 AS Result, N'Saved.' AS Message, CAST(SCOPE_IDENTITY() AS INT) AS id;
    END
    ELSE
    BEGIN
        UPDATE dbo.Pms_tbl_ProjectBudgetLine
           SET MilestoneId = @MilestoneId, TaskId = @TaskId, CostCodeId = @CostCodeId, Quantity = @Quantity,
               UnitId = @UnitId, Unit = @UnitName, Rate = @Rate, Amount = @Amount,
               Remarks = NULLIF(LTRIM(RTRIM(@Remarks)), N''), mby = @uid, mdate = GETDATE()
         WHERE id = @id AND VersionId = @VersionId AND regid = @regid AND status = 1 AND archive = 0;
        IF @@ROWCOUNT = 1 SELECT 1 AS Result, N'Saved.' AS Message, @id AS id;
        ELSE SELECT 0 AS Result, N'Budget line was not found.' AS Message;
    END
END
GO

IF OBJECT_ID('dbo.usp_pms_cc_BudgetLine_Delete', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_BudgetLine_Delete AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_BudgetLine_Delete @regid INT, @id INT, @uid INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE b SET b.status = 0, b.archive = 1, b.mby = @uid, b.mdate = GETDATE()
      FROM dbo.Pms_tbl_ProjectBudgetLine b
      INNER JOIN dbo.Pms_tbl_ProjectBudgetVersion v ON v.id = b.VersionId AND v.regid = b.regid
     WHERE b.id = @id AND b.regid = @regid AND b.status = 1 AND b.archive = 0 AND v.VersionStatus = N'Draft';
    IF @@ROWCOUNT = 1 SELECT 1 AS Result, N'Deleted.' AS Message;
    ELSE SELECT 0 AS Result, N'Only lines of a draft budget can be deleted.' AS Message;
END
GO

IF OBJECT_ID('dbo.usp_pms_cc_BudgetVersion_Lock', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_BudgetVersion_Lock AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_BudgetVersion_Lock @regid INT, @VersionId INT, @uid INT
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM dbo.Pms_tbl_ProjectBudgetLine WHERE VersionId = @VersionId AND regid = @regid AND status = 1 AND archive = 0)
        BEGIN SELECT 0 AS Result, N'Add at least one budget line before locking.' AS Message; RETURN; END
    UPDATE dbo.Pms_tbl_ProjectBudgetVersion
       SET VersionStatus = N'Locked', LockedBy = @uid, LockedOn = GETDATE()
     WHERE id = @VersionId AND regid = @regid AND VersionStatus = N'Draft' AND status = 1 AND archive = 0;
    IF @@ROWCOUNT = 1 SELECT 1 AS Result, N'Budget locked.' AS Message;
    ELSE SELECT 0 AS Result, N'Only a draft budget can be locked.' AS Message;
END
GO

IF OBJECT_ID('dbo.usp_pms_cc_BudgetVersion_DeleteDraft', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_BudgetVersion_DeleteDraft AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_BudgetVersion_DeleteDraft @regid INT, @VersionId INT, @uid INT
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM dbo.Pms_tbl_ProjectBudgetVersion WHERE id = @VersionId AND regid = @regid
                   AND VersionStatus = N'Draft' AND status = 1 AND archive = 0)
        BEGIN SELECT 0 AS Result, N'Only a draft budget can be discarded.' AS Message; RETURN; END
    SET XACT_ABORT ON;
    BEGIN TRANSACTION;
        UPDATE dbo.Pms_tbl_ProjectBudgetLine SET status = 0, archive = 1, mby = @uid, mdate = GETDATE()
         WHERE VersionId = @VersionId AND regid = @regid AND status = 1 AND archive = 0;
        UPDATE dbo.Pms_tbl_ProjectBudgetVersion SET status = 0, archive = 1
         WHERE id = @VersionId AND regid = @regid;
    COMMIT TRANSACTION;
    SELECT 1 AS Result, N'Draft discarded.' AS Message;
END
GO

/* ---------------------------------------------------------------------
   6. Cost entries (actuals)
   --------------------------------------------------------------------- */
IF OBJECT_ID('dbo.usp_pms_cc_CostEntry_List', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_CostEntry_List AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_CostEntry_List @regid INT, @ProjectId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT e.id, CONVERT(VARCHAR(10), e.EntryDate, 23) AS EntryDate,
           e.MilestoneId, ISNULL(m.milestone, N'') AS MilestoneName,
           e.TaskId, ISNULL(t.taskname, N'') AS TaskName,
           e.CostCodeId, ISNULL(c.Code, N'') AS CostCode, ISNULL(c.Name, N'') AS CostCodeName,
           e.Amount, e.SourceType, ISNULL(e.ReferenceNo, N'') AS ReferenceNo, ISNULL(e.Remarks, N'') AS Remarks
    FROM dbo.Pms_tbl_ProjectCostEntry e WITH (NOLOCK)
    LEFT JOIN dbo.Pms_tbl_CostCode c WITH (NOLOCK) ON c.id = e.CostCodeId AND c.regid = e.regid
    LEFT JOIN dbo.Pms_tbl_AddMilestone m WITH (NOLOCK) ON m.id = e.MilestoneId AND m.regid = e.regid
    LEFT JOIN dbo.Pms_tbl_Addtask t WITH (NOLOCK) ON t.id = e.TaskId AND t.regid = e.regid
    WHERE e.regid = @regid AND e.ProjectId = @ProjectId AND e.status = 1 AND e.archive = 0
    ORDER BY e.EntryDate DESC, e.id DESC;
END
GO

IF OBJECT_ID('dbo.usp_pms_cc_CostEntry_Save', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_CostEntry_Save AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_CostEntry_Save
    @regid INT, @id INT, @ProjectId INT, @MilestoneId INT = NULL, @TaskId INT = NULL, @CostCodeId INT,
    @EntryDate DATE, @Amount DECIMAL(18,2), @ReferenceNo NVARCHAR(100) = NULL, @Remarks NVARCHAR(1000) = NULL, @uid INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @IsValid BIT;
    EXEC dbo.usp_pms_cc_ValidateHierarchy @regid, @ProjectId, @MilestoneId, @TaskId, @IsValid OUTPUT;
    IF @IsValid = 0 BEGIN SELECT 0 AS Result, N'Project, milestone or task is not valid.' AS Message; RETURN; END
    IF NOT EXISTS (SELECT 1 FROM dbo.Pms_tbl_CostCode WHERE id = @CostCodeId AND regid = @regid AND status = 1 AND archive = 0)
        BEGIN SELECT 0 AS Result, N'Select an active cost code.' AS Message; RETURN; END
    IF @EntryDate IS NULL BEGIN SELECT 0 AS Result, N'Enter the cost date.' AS Message; RETURN; END
    IF @Amount IS NULL OR @Amount = 0 BEGIN SELECT 0 AS Result, N'Enter a non-zero amount (negative for a reversal).' AS Message; RETURN; END

    IF ISNULL(@id, 0) = 0
    BEGIN
        INSERT INTO dbo.Pms_tbl_ProjectCostEntry (regid, ProjectId, MilestoneId, TaskId, CostCodeId, EntryDate, Amount, SourceType, ReferenceNo, Remarks, cby, cdate)
        VALUES (@regid, @ProjectId, @MilestoneId, @TaskId, @CostCodeId, @EntryDate, @Amount, N'Manual',
                NULLIF(LTRIM(RTRIM(@ReferenceNo)), N''), NULLIF(LTRIM(RTRIM(@Remarks)), N''), @uid, GETDATE());
        SELECT 1 AS Result, N'Saved.' AS Message, CAST(SCOPE_IDENTITY() AS INT) AS id;
    END
    ELSE
    BEGIN
        UPDATE dbo.Pms_tbl_ProjectCostEntry
           SET MilestoneId = @MilestoneId, TaskId = @TaskId, CostCodeId = @CostCodeId, EntryDate = @EntryDate, Amount = @Amount,
               ReferenceNo = NULLIF(LTRIM(RTRIM(@ReferenceNo)), N''), Remarks = NULLIF(LTRIM(RTRIM(@Remarks)), N''),
               mby = @uid, mdate = GETDATE()
         WHERE id = @id AND ProjectId = @ProjectId AND regid = @regid AND SourceType = N'Manual' AND status = 1 AND archive = 0;
        IF @@ROWCOUNT = 1 SELECT 1 AS Result, N'Saved.' AS Message, @id AS id;
        ELSE SELECT 0 AS Result, N'Cost entry was not found.' AS Message;
    END
END
GO

IF OBJECT_ID('dbo.usp_pms_cc_CostEntry_Delete', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_CostEntry_Delete AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_CostEntry_Delete @regid INT, @id INT, @uid INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Pms_tbl_ProjectCostEntry SET status = 0, archive = 1, mby = @uid, mdate = GETDATE()
     WHERE id = @id AND regid = @regid AND SourceType = N'Manual' AND status = 1 AND archive = 0;
    IF @@ROWCOUNT = 1 SELECT 1 AS Result, N'Deleted.' AS Message;
    ELSE SELECT 0 AS Result, N'Cost entry was not found.' AS Message;
END
GO

/* ---------------------------------------------------------------------
   7. Budget vs actual
   Original budget = version 1 (when locked).
   Current budget  = latest LOCKED version (drafts are excluded).
   Actual          = sum of active cost entries.
   Variance        = Current budget - Actual (positive = under budget).
   @GroupBy: 'COSTCODE' or 'MILESTONE'.
   --------------------------------------------------------------------- */
IF OBJECT_ID('dbo.usp_pms_cc_BudgetVsActual', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_BudgetVsActual AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_BudgetVsActual @regid INT, @ProjectId INT, @GroupBy VARCHAR(20) = 'COSTCODE'
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @OriginalId INT, @CurrentId INT;
    SELECT TOP 1 @OriginalId = id FROM dbo.Pms_tbl_ProjectBudgetVersion
     WHERE regid = @regid AND ProjectId = @ProjectId AND VersionStatus = N'Locked' AND status = 1 AND archive = 0
     ORDER BY VersionNo ASC;
    SELECT TOP 1 @CurrentId = id FROM dbo.Pms_tbl_ProjectBudgetVersion
     WHERE regid = @regid AND ProjectId = @ProjectId AND VersionStatus = N'Locked' AND status = 1 AND archive = 0
     ORDER BY VersionNo DESC;

    ;WITH Src AS
    (
        SELECT CASE WHEN @GroupBy = 'MILESTONE' THEN ISNULL(b.MilestoneId, 0) ELSE b.CostCodeId END AS GroupId,
               CASE WHEN b.VersionId = @OriginalId THEN b.Amount ELSE 0 END AS OriginalBudget,
               CASE WHEN b.VersionId = @CurrentId  THEN b.Amount ELSE 0 END AS CurrentBudget,
               CAST(0 AS DECIMAL(18,2)) AS Actual
          FROM dbo.Pms_tbl_ProjectBudgetLine b
         WHERE b.regid = @regid AND b.ProjectId = @ProjectId AND b.status = 1 AND b.archive = 0
           AND b.VersionId IN (ISNULL(@OriginalId, -1), ISNULL(@CurrentId, -1))
        UNION ALL
        SELECT CASE WHEN @GroupBy = 'MILESTONE' THEN ISNULL(e.MilestoneId, 0) ELSE e.CostCodeId END,
               0, 0, e.Amount
          FROM dbo.Pms_tbl_ProjectCostEntry e
         WHERE e.regid = @regid AND e.ProjectId = @ProjectId AND e.status = 1 AND e.archive = 0
    ),
    Agg AS
    (
        SELECT GroupId, SUM(OriginalBudget) AS OriginalBudget, SUM(CurrentBudget) AS CurrentBudget, SUM(Actual) AS Actual
          FROM Src GROUP BY GroupId
    )
    SELECT a.GroupId,
           CASE WHEN @GroupBy = 'MILESTONE'
                THEN CASE WHEN a.GroupId = 0 THEN N'Project level (no milestone)' ELSE ISNULL(m.milestone, N'(removed milestone)') END
                ELSE ISNULL(c.Code + N' - ' + c.Name, N'(removed cost code)') END AS GroupName,
           CASE WHEN @GroupBy = 'MILESTONE' THEN N'' ELSE ISNULL(c.CostType, N'') END AS CostType,
           a.OriginalBudget, a.CurrentBudget, a.Actual,
           a.CurrentBudget - a.Actual AS Variance,
           CASE WHEN a.CurrentBudget = 0 THEN NULL ELSE CAST(ROUND(a.Actual * 100.0 / a.CurrentBudget, 2) AS DECIMAL(18,2)) END AS ConsumedPercent
      FROM Agg a
      LEFT JOIN dbo.Pms_tbl_CostCode c ON @GroupBy <> 'MILESTONE' AND c.id = a.GroupId AND c.regid = @regid
      LEFT JOIN dbo.Pms_tbl_AddMilestone m ON @GroupBy = 'MILESTONE' AND m.id = a.GroupId AND m.regid = @regid
     ORDER BY GroupName;

    -- Table 1: header information
    SELECT (SELECT VersionNo FROM dbo.Pms_tbl_ProjectBudgetVersion WHERE id = @CurrentId) AS CurrentVersionNo,
           (SELECT COUNT(1) FROM dbo.Pms_tbl_ProjectBudgetVersion WHERE regid = @regid AND ProjectId = @ProjectId
             AND VersionStatus = N'Draft' AND status = 1 AND archive = 0) AS DraftCount;
END
GO
