/* =====================================================================
   PMS Cost Control - Phase 2 database script
   ---------------------------------------------------------------------
   Run AFTER costcontrol_phase1.sql (always in the order phase1 -> phase2).
   Both scripts are safe to re-run.

   Adds:
     1. Cost-code lock once used + change history        (CC-04)
     2. Cost-code hierarchy Division > Section > Element  (CC-01)
     3. Audit history, insert-only, with reasons          (AU-01, NF-06)
     4. Budget maker-checker: Draft > Submitted > Locked  (AP-03, AP-10)
     5. Cost Control access (view / enter / approve)      (5.1, NF-04)
     6. Estimate to complete (ETC) -> EAC, VAC            (RP-01)
     7. Report by cost type + multi-project summary       (RP-01, RP-16)
     9. Monthly period lock per project                   (PE-01, PE-04, PE-06)
    10. Site + source document type on cost entries,
        duplicate-reference warning                       (DM-01)
     (8. Excel export needs no database change.)

   Safety: ADDITIVE ONLY. New tables and new columns are created only on
   the cost-control tables created in Phase 1. Existing PMS / ERP tables
   (Pms_tbl_AddProject, Pms_tbl_AddMilestone, Pms_tbl_Addtask,
   tbl_employee, tbl_unitmaster) are only READ.
   Requires SQL Server 2016+ (FOR JSON).
   ===================================================================== */

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

/* ---------------------------------------------------------------------
   1. New tables
   --------------------------------------------------------------------- */
IF OBJECT_ID('dbo.Pms_tbl_CostControlAccess', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Pms_tbl_CostControlAccess
    (
        id          INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Pms_tbl_CostControlAccess PRIMARY KEY,
        regid       INT      NOT NULL,
        EmployeeId  INT      NOT NULL,     -- tbl_employee.id
        CanView     BIT      NOT NULL CONSTRAINT DF_Pms_tbl_CostControlAccess_view    DEFAULT (0),
        CanEnter    BIT      NOT NULL CONSTRAINT DF_Pms_tbl_CostControlAccess_enter   DEFAULT (0),
        CanApprove  BIT      NOT NULL CONSTRAINT DF_Pms_tbl_CostControlAccess_approve DEFAULT (0),
        status      INT      NOT NULL CONSTRAINT DF_Pms_tbl_CostControlAccess_status  DEFAULT (1),
        archive     INT      NOT NULL CONSTRAINT DF_Pms_tbl_CostControlAccess_archive DEFAULT (0),
        cby         INT      NULL,
        cdate       DATETIME NOT NULL CONSTRAINT DF_Pms_tbl_CostControlAccess_cdate DEFAULT (GETDATE()),
        mby         INT      NULL,
        mdate       DATETIME NULL
    );
    CREATE INDEX IX_Pms_tbl_CostControlAccess_emp ON dbo.Pms_tbl_CostControlAccess (regid, EmployeeId, status, archive);
END
GO

IF OBJECT_ID('dbo.Pms_tbl_CostControlAudit', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Pms_tbl_CostControlAudit
    (
        id          BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Pms_tbl_CostControlAudit PRIMARY KEY,
        regid       INT            NOT NULL,
        ProjectId   INT            NULL,
        EntityType  NVARCHAR(30)   NOT NULL,   -- CostCode / BudgetVersion / BudgetLine / CostEntry / Etc / Period / Access
        EntityId    INT            NULL,
        Action      NVARCHAR(30)   NOT NULL,   -- Create / Update / Delete / Submit / Approve / Reject / Close / Reopen ...
        Reason      NVARCHAR(1000) NULL,
        OldData     NVARCHAR(MAX)  NULL,       -- JSON snapshot before
        NewData     NVARCHAR(MAX)  NULL,       -- JSON snapshot after
        uid         INT            NULL,
        cdate       DATETIME       NOT NULL CONSTRAINT DF_Pms_tbl_CostControlAudit_cdate DEFAULT (GETDATE())
    );
    CREATE INDEX IX_Pms_tbl_CostControlAudit_project ON dbo.Pms_tbl_CostControlAudit (regid, ProjectId, cdate);
    CREATE INDEX IX_Pms_tbl_CostControlAudit_entity  ON dbo.Pms_tbl_CostControlAudit (regid, EntityType, EntityId);
END
GO

-- The audit history is insert-only: updates and deletes are refused.
IF OBJECT_ID('dbo.trg_Pms_tbl_CostControlAudit_NoChange', 'TR') IS NULL
    EXEC('CREATE TRIGGER dbo.trg_Pms_tbl_CostControlAudit_NoChange ON dbo.Pms_tbl_CostControlAudit
          INSTEAD OF UPDATE, DELETE
          AS
          BEGIN
              RAISERROR(N''Cost Control audit history cannot be changed or deleted.'', 16, 1);
          END');
GO

IF OBJECT_ID('dbo.Pms_tbl_ProjectEtc', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Pms_tbl_ProjectEtc
    (
        id          INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Pms_tbl_ProjectEtc PRIMARY KEY,
        regid       INT            NOT NULL,
        ProjectId   INT            NOT NULL,
        CostCodeId  INT            NOT NULL,
        MilestoneId INT            NULL,
        EtcAmount   DECIMAL(18,2)  NOT NULL,
        AsOfDate    DATE           NOT NULL,
        Remarks     NVARCHAR(1000) NULL,
        status      INT            NOT NULL CONSTRAINT DF_Pms_tbl_ProjectEtc_status  DEFAULT (1),
        archive     INT            NOT NULL CONSTRAINT DF_Pms_tbl_ProjectEtc_archive DEFAULT (0),
        cby         INT            NULL,
        cdate       DATETIME       NOT NULL CONSTRAINT DF_Pms_tbl_ProjectEtc_cdate DEFAULT (GETDATE()),
        mby         INT            NULL,
        mdate       DATETIME       NULL
    );
    CREATE INDEX IX_Pms_tbl_ProjectEtc_project ON dbo.Pms_tbl_ProjectEtc (regid, ProjectId, status, archive);
END
GO

IF OBJECT_ID('dbo.Pms_tbl_CostControlPeriod', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Pms_tbl_CostControlPeriod
    (
        id           INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Pms_tbl_CostControlPeriod PRIMARY KEY,
        regid        INT            NOT NULL,
        ProjectId    INT            NOT NULL,
        PeriodStart  DATE           NOT NULL,   -- first day of the month
        IsClosed     BIT            NOT NULL CONSTRAINT DF_Pms_tbl_CostControlPeriod_closed DEFAULT (0),
        ClosedBy     INT            NULL,
        ClosedOn     DATETIME       NULL,
        ReopenedBy   INT            NULL,
        ReopenedOn   DATETIME       NULL,
        ReopenReason NVARCHAR(1000) NULL
    );
    CREATE UNIQUE INDEX UX_Pms_tbl_CostControlPeriod ON dbo.Pms_tbl_CostControlPeriod (regid, ProjectId, PeriodStart);
END
GO

/* ---------------------------------------------------------------------
   2. New columns on the Phase 1 cost-control tables
   --------------------------------------------------------------------- */
IF COL_LENGTH('dbo.Pms_tbl_CostCode', 'ParentId') IS NULL
    ALTER TABLE dbo.Pms_tbl_CostCode ADD ParentId INT NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_CostCode', 'CodeLevel') IS NULL
    ALTER TABLE dbo.Pms_tbl_CostCode ADD CodeLevel NVARCHAR(20) NULL;
GO
-- Codes created in Phase 1 are posting codes.
UPDATE dbo.Pms_tbl_CostCode SET CodeLevel = N'Element' WHERE CodeLevel IS NULL;
GO

IF COL_LENGTH('dbo.Pms_tbl_ProjectBudgetVersion', 'SubmittedBy') IS NULL
    ALTER TABLE dbo.Pms_tbl_ProjectBudgetVersion ADD SubmittedBy INT NULL, SubmittedOn DATETIME NULL, RejectReason NVARCHAR(1000) NULL;
GO

IF COL_LENGTH('dbo.Pms_tbl_ProjectCostEntry', 'SiteName') IS NULL
    ALTER TABLE dbo.Pms_tbl_ProjectCostEntry ADD SiteName NVARCHAR(100) NULL, SourceDocType NVARCHAR(30) NULL;
GO

/* ---------------------------------------------------------------------
   3. Helpers
   --------------------------------------------------------------------- */
IF OBJECT_ID('dbo.usp_pms_cc_Audit_Add', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_Audit_Add AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_Audit_Add
    @regid INT, @ProjectId INT = NULL, @EntityType NVARCHAR(30), @EntityId INT = NULL, @Action NVARCHAR(30),
    @Reason NVARCHAR(1000) = NULL, @OldData NVARCHAR(MAX) = NULL, @NewData NVARCHAR(MAX) = NULL, @uid INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Pms_tbl_CostControlAudit (regid, ProjectId, EntityType, EntityId, Action, Reason, OldData, NewData, uid, cdate)
    VALUES (@regid, @ProjectId, @EntityType, @EntityId, @Action, NULLIF(LTRIM(RTRIM(@Reason)), N''), @OldData, @NewData, @uid, GETDATE());
END
GO

-- Returns 1 when the month of @AnyDate is closed for the project.
IF OBJECT_ID('dbo.ufn_pms_cc_IsPeriodClosed', 'FN') IS NOT NULL DROP FUNCTION dbo.ufn_pms_cc_IsPeriodClosed;
GO
CREATE FUNCTION dbo.ufn_pms_cc_IsPeriodClosed (@regid INT, @ProjectId INT, @AnyDate DATE)
RETURNS BIT
AS
BEGIN
    RETURN CASE WHEN EXISTS (SELECT 1 FROM dbo.Pms_tbl_CostControlPeriod
                              WHERE regid = @regid AND ProjectId = @ProjectId AND IsClosed = 1
                                AND PeriodStart = DATEFROMPARTS(YEAR(@AnyDate), MONTH(@AnyDate), 1))
                THEN 1 ELSE 0 END;
END
GO

-- A budget line, cost entry or ETC may only use an active Element-level (posting) code.
IF OBJECT_ID('dbo.ufn_pms_cc_IsPostingCode', 'FN') IS NOT NULL DROP FUNCTION dbo.ufn_pms_cc_IsPostingCode;
GO
CREATE FUNCTION dbo.ufn_pms_cc_IsPostingCode (@regid INT, @CostCodeId INT)
RETURNS BIT
AS
BEGIN
    RETURN CASE WHEN EXISTS (SELECT 1 FROM dbo.Pms_tbl_CostCode
                              WHERE id = @CostCodeId AND regid = @regid AND status = 1 AND archive = 0
                                AND ISNULL(CodeLevel, N'Element') = N'Element')
                THEN 1 ELSE 0 END;
END
GO

/* ---------------------------------------------------------------------
   4. Access (view / enter / approve)
   --------------------------------------------------------------------- */
IF OBJECT_ID('dbo.usp_pms_cc_Access_Get', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_Access_Get AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_Access_Get @regid INT, @EmployeeId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP 1 CAST(CanView AS INT) AS CanView, CAST(CanEnter AS INT) AS CanEnter, CAST(CanApprove AS INT) AS CanApprove
      FROM dbo.Pms_tbl_CostControlAccess
     WHERE regid = @regid AND EmployeeId = @EmployeeId AND status = 1 AND archive = 0;
END
GO

IF OBJECT_ID('dbo.usp_pms_cc_Access_List', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_Access_List AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_Access_List @regid INT
AS
BEGIN
    SET NOCOUNT ON;
    -- All active employees of the company, with their Cost Control rights (0 when none).
    SELECT e.id AS EmployeeId, ISNULL(e.DisplayName, N'') AS EmployeeName,
           CAST(ISNULL(a.CanView, 0) AS INT) AS CanView, CAST(ISNULL(a.CanEnter, 0) AS INT) AS CanEnter,
           CAST(ISNULL(a.CanApprove, 0) AS INT) AS CanApprove
      FROM dbo.tbl_employee e WITH (NOLOCK)
      LEFT JOIN dbo.Pms_tbl_CostControlAccess a ON a.EmployeeId = e.id AND a.regid = e.regid AND a.status = 1 AND a.archive = 0
     WHERE e.regid = @regid AND e.status = 1 AND e.archive = 0
     ORDER BY e.DisplayName;
END
GO

IF OBJECT_ID('dbo.usp_pms_cc_Access_Save', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_Access_Save AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_Access_Save
    @regid INT, @EmployeeId INT, @CanView BIT, @CanEnter BIT, @CanApprove BIT, @uid INT
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM dbo.tbl_employee WHERE id = @EmployeeId AND regid = @regid AND status = 1 AND archive = 0)
        BEGIN SELECT 0 AS Result, N'Employee was not found.' AS Message; RETURN; END
    -- Entering or approving implies viewing.
    IF @CanEnter = 1 OR @CanApprove = 1 SET @CanView = 1;

    DECLARE @id INT, @old NVARCHAR(MAX), @new NVARCHAR(MAX);
    SELECT @id = id FROM dbo.Pms_tbl_CostControlAccess WHERE regid = @regid AND EmployeeId = @EmployeeId AND status = 1 AND archive = 0;
    SET @old = (SELECT CanView, CanEnter, CanApprove FROM dbo.Pms_tbl_CostControlAccess WHERE id = @id FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);

    IF @id IS NULL
    BEGIN
        INSERT INTO dbo.Pms_tbl_CostControlAccess (regid, EmployeeId, CanView, CanEnter, CanApprove, cby, cdate)
        VALUES (@regid, @EmployeeId, @CanView, @CanEnter, @CanApprove, @uid, GETDATE());
        SET @id = CAST(SCOPE_IDENTITY() AS INT);
    END
    ELSE
        UPDATE dbo.Pms_tbl_CostControlAccess SET CanView = @CanView, CanEnter = @CanEnter, CanApprove = @CanApprove, mby = @uid, mdate = GETDATE()
         WHERE id = @id;

    SET @new = (SELECT EmployeeId, CanView, CanEnter, CanApprove FROM dbo.Pms_tbl_CostControlAccess WHERE id = @id FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
    EXEC dbo.usp_pms_cc_Audit_Add @regid, NULL, N'Access', @EmployeeId, N'Update', NULL, @old, @new, @uid;
    SELECT 1 AS Result, N'Access saved.' AS Message, @id AS id;
END
GO

IF OBJECT_ID('dbo.usp_pms_cc_Employee_Names', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_Employee_Names AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_Employee_Names @regid INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT e.id, ISNULL(e.DisplayName, N'') AS EmployeeName FROM dbo.tbl_employee e WITH (NOLOCK) WHERE e.regid = @regid;
END
GO

/* ---------------------------------------------------------------------
   5. Cost codes: hierarchy, lock once used, history
   --------------------------------------------------------------------- */
ALTER PROCEDURE dbo.usp_pms_cc_CostCode_List @regid INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT c.id, c.Code, c.Name, c.CostType, ISNULL(c.Description, N'') AS Description,
           ISNULL(c.CodeLevel, N'Element') AS CodeLevel, c.ParentId,
           ISNULL(p.Code, N'') AS ParentCode, ISNULL(p.Name, N'') AS ParentName,
           ISNULL(g.Code + N' / ', N'') + ISNULL(p.Code + N' / ', N'') + c.Code AS CodePath,
           CASE WHEN EXISTS (SELECT 1 FROM dbo.Pms_tbl_ProjectBudgetLine b WHERE b.CostCodeId = c.id AND b.regid = c.regid AND b.status = 1 AND b.archive = 0)
                  OR EXISTS (SELECT 1 FROM dbo.Pms_tbl_ProjectCostEntry e WHERE e.CostCodeId = c.id AND e.regid = c.regid AND e.status = 1 AND e.archive = 0)
                  OR EXISTS (SELECT 1 FROM dbo.Pms_tbl_ProjectEtc x WHERE x.CostCodeId = c.id AND x.regid = c.regid AND x.status = 1 AND x.archive = 0)
                THEN 1 ELSE 0 END AS IsUsed,
           CASE WHEN EXISTS (SELECT 1 FROM dbo.Pms_tbl_CostCode k WHERE k.ParentId = c.id AND k.regid = c.regid AND k.status = 1 AND k.archive = 0)
                THEN 1 ELSE 0 END AS HasChildren
    FROM dbo.Pms_tbl_CostCode c WITH (NOLOCK)
    LEFT JOIN dbo.Pms_tbl_CostCode p WITH (NOLOCK) ON p.id = c.ParentId AND p.regid = c.regid
    LEFT JOIN dbo.Pms_tbl_CostCode g WITH (NOLOCK) ON g.id = p.ParentId AND g.regid = p.regid
    WHERE c.regid = @regid AND c.status = 1 AND c.archive = 0
    ORDER BY CodePath;
END
GO

IF OBJECT_ID('dbo.usp_pms_cc_CostCode_Save', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_CostCode_Save AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_CostCode_Save
    @regid INT, @id INT, @Code NVARCHAR(30), @Name NVARCHAR(200), @CostType NVARCHAR(30),
    @Description NVARCHAR(1000) = NULL, @uid INT,
    @CodeLevel NVARCHAR(20) = N'Element', @ParentId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET @Code = LTRIM(RTRIM(ISNULL(@Code, N''))); SET @Name = LTRIM(RTRIM(ISNULL(@Name, N'')));
    SET @CodeLevel = ISNULL(NULLIF(@CodeLevel, N''), N'Element');
    IF @Code = N'' OR @Name = N''
        BEGIN SELECT 0 AS Result, N'Code and name are required.' AS Message; RETURN; END
    IF @CostType NOT IN (N'Material', N'Labour', N'Subcontract', N'Equipment', N'Overhead', N'Other')
        BEGIN SELECT 0 AS Result, N'Select a valid cost type.' AS Message; RETURN; END
    IF @CodeLevel NOT IN (N'Division', N'Section', N'Element')
        BEGIN SELECT 0 AS Result, N'Select a valid level.' AS Message; RETURN; END
    IF EXISTS (SELECT 1 FROM dbo.Pms_tbl_CostCode WHERE regid = @regid AND status = 1 AND archive = 0
               AND Code = @Code AND id <> ISNULL(@id, 0))
        BEGIN SELECT 0 AS Result, N'This cost code already exists.' AS Message; RETURN; END

    -- Hierarchy: Division has no parent; Section sits under a Division; Element under a Section (or none for older codes).
    DECLARE @ParentLevel NVARCHAR(20) = NULL;
    IF @ParentId IS NOT NULL
        SELECT @ParentLevel = ISNULL(CodeLevel, N'Element') FROM dbo.Pms_tbl_CostCode
         WHERE id = @ParentId AND regid = @regid AND status = 1 AND archive = 0;
    IF @CodeLevel = N'Division' AND @ParentId IS NOT NULL
        BEGIN SELECT 0 AS Result, N'A Division has no parent.' AS Message; RETURN; END
    IF @CodeLevel = N'Section' AND ISNULL(@ParentLevel, N'') <> N'Division'
        BEGIN SELECT 0 AS Result, N'A Section must sit under a Division.' AS Message; RETURN; END
    IF @CodeLevel = N'Element' AND @ParentId IS NOT NULL AND ISNULL(@ParentLevel, N'') <> N'Section'
        BEGIN SELECT 0 AS Result, N'An Element must sit under a Section.' AS Message; RETURN; END

    DECLARE @old NVARCHAR(MAX), @new NVARCHAR(MAX);
    IF ISNULL(@id, 0) = 0
    BEGIN
        INSERT INTO dbo.Pms_tbl_CostCode (regid, Code, Name, CostType, Description, CodeLevel, ParentId, cby, cdate)
        VALUES (@regid, @Code, @Name, @CostType, @Description, @CodeLevel, @ParentId, @uid, GETDATE());
        SET @id = CAST(SCOPE_IDENTITY() AS INT);
        SET @new = (SELECT Code, Name, CostType, CodeLevel, ParentId, Description FROM dbo.Pms_tbl_CostCode WHERE id = @id FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
        EXEC dbo.usp_pms_cc_Audit_Add @regid, NULL, N'CostCode', @id, N'Create', NULL, NULL, @new, @uid;
        SELECT 1 AS Result, N'Saved.' AS Message, @id AS id;
        RETURN;
    END

    DECLARE @OldCode NVARCHAR(30), @OldName NVARCHAR(200), @OldType NVARCHAR(30), @OldLevel NVARCHAR(20), @OldParent INT;
    SELECT @OldCode = Code, @OldName = Name, @OldType = CostType, @OldLevel = ISNULL(CodeLevel, N'Element'), @OldParent = ParentId
      FROM dbo.Pms_tbl_CostCode WHERE id = @id AND regid = @regid AND status = 1 AND archive = 0;
    IF @OldCode IS NULL BEGIN SELECT 0 AS Result, N'Cost code was not found.' AS Message; RETURN; END

    -- CC-04: once used, a code's identity cannot change (only the description may), so past reports stay true.
    DECLARE @IsUsed BIT = CASE WHEN EXISTS (SELECT 1 FROM dbo.Pms_tbl_ProjectBudgetLine WHERE CostCodeId = @id AND regid = @regid AND status = 1 AND archive = 0)
                                 OR EXISTS (SELECT 1 FROM dbo.Pms_tbl_ProjectCostEntry WHERE CostCodeId = @id AND regid = @regid AND status = 1 AND archive = 0)
                                 OR EXISTS (SELECT 1 FROM dbo.Pms_tbl_ProjectEtc WHERE CostCodeId = @id AND regid = @regid AND status = 1 AND archive = 0)
                               THEN 1 ELSE 0 END;
    IF @IsUsed = 1 AND (@Code <> @OldCode OR @Name <> @OldName OR @CostType <> @OldType OR @CodeLevel <> @OldLevel OR ISNULL(@ParentId, 0) <> ISNULL(@OldParent, 0))
        BEGIN SELECT 0 AS Result, N'This cost code is already used. Only its description can be changed; create a new code instead.' AS Message; RETURN; END
    IF @CodeLevel <> @OldLevel AND EXISTS (SELECT 1 FROM dbo.Pms_tbl_CostCode WHERE ParentId = @id AND regid = @regid AND status = 1 AND archive = 0)
        BEGIN SELECT 0 AS Result, N'This code has child codes, so its level cannot change.' AS Message; RETURN; END
    IF @ParentId = @id BEGIN SELECT 0 AS Result, N'A code cannot be its own parent.' AS Message; RETURN; END

    SET @old = (SELECT Code, Name, CostType, CodeLevel, ParentId, Description FROM dbo.Pms_tbl_CostCode WHERE id = @id FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
    UPDATE dbo.Pms_tbl_CostCode
       SET Code = @Code, Name = @Name, CostType = @CostType, Description = @Description,
           CodeLevel = @CodeLevel, ParentId = @ParentId, mby = @uid, mdate = GETDATE()
     WHERE id = @id AND regid = @regid AND status = 1 AND archive = 0;
    SET @new = (SELECT Code, Name, CostType, CodeLevel, ParentId, Description FROM dbo.Pms_tbl_CostCode WHERE id = @id FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
    EXEC dbo.usp_pms_cc_Audit_Add @regid, NULL, N'CostCode', @id, N'Update', NULL, @old, @new, @uid;
    SELECT 1 AS Result, N'Saved.' AS Message, @id AS id;
END
GO

ALTER PROCEDURE dbo.usp_pms_cc_CostCode_Delete @regid INT, @id INT, @uid INT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM dbo.Pms_tbl_ProjectBudgetLine WHERE CostCodeId = @id AND regid = @regid AND status = 1 AND archive = 0)
       OR EXISTS (SELECT 1 FROM dbo.Pms_tbl_ProjectCostEntry WHERE CostCodeId = @id AND regid = @regid AND status = 1 AND archive = 0)
       OR EXISTS (SELECT 1 FROM dbo.Pms_tbl_ProjectEtc WHERE CostCodeId = @id AND regid = @regid AND status = 1 AND archive = 0)
        BEGIN SELECT 0 AS Result, N'This cost code is used in budgets, cost entries or forecasts and cannot be deleted.' AS Message; RETURN; END
    IF EXISTS (SELECT 1 FROM dbo.Pms_tbl_CostCode WHERE ParentId = @id AND regid = @regid AND status = 1 AND archive = 0)
        BEGIN SELECT 0 AS Result, N'Delete or move its child codes first.' AS Message; RETURN; END
    DECLARE @old NVARCHAR(MAX) = (SELECT Code, Name, CostType, CodeLevel, ParentId FROM dbo.Pms_tbl_CostCode WHERE id = @id AND regid = @regid FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
    UPDATE dbo.Pms_tbl_CostCode SET status = 0, archive = 1, mby = @uid, mdate = GETDATE()
     WHERE id = @id AND regid = @regid AND status = 1 AND archive = 0;
    IF @@ROWCOUNT = 1
    BEGIN
        EXEC dbo.usp_pms_cc_Audit_Add @regid, NULL, N'CostCode', @id, N'Delete', NULL, @old, NULL, @uid;
        SELECT 1 AS Result, N'Deleted.' AS Message;
    END
    ELSE SELECT 0 AS Result, N'Cost code was not found.' AS Message;
END
GO

/* ---------------------------------------------------------------------
   6. Budget: maker-checker (Draft > Submitted > Locked), audit
   --------------------------------------------------------------------- */
ALTER PROCEDURE dbo.usp_pms_cc_Budget_Get @regid INT, @ProjectId INT, @VersionId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT v.id, v.VersionNo, v.VersionType, v.VersionStatus, ISNULL(v.Reason, N'') AS Reason,
           v.LockedOn, v.cdate, v.cby, v.SubmittedBy, v.SubmittedOn, v.LockedBy, ISNULL(v.RejectReason, N'') AS RejectReason,
           ISNULL(es.DisplayName, N'') AS SubmittedByName, ISNULL(el.DisplayName, N'') AS LockedByName,
           (SELECT ISNULL(SUM(b.Amount), 0) FROM dbo.Pms_tbl_ProjectBudgetLine b
             WHERE b.VersionId = v.id AND b.regid = v.regid AND b.status = 1 AND b.archive = 0) AS TotalAmount
    FROM dbo.Pms_tbl_ProjectBudgetVersion v WITH (NOLOCK)
    LEFT JOIN dbo.tbl_employee es WITH (NOLOCK) ON es.id = v.SubmittedBy
    LEFT JOIN dbo.tbl_employee el WITH (NOLOCK) ON el.id = v.LockedBy
    WHERE v.regid = @regid AND v.ProjectId = @ProjectId AND v.status = 1 AND v.archive = 0
    ORDER BY v.VersionNo DESC;

    IF @VersionId IS NULL
        SELECT TOP 1 @VersionId = id FROM dbo.Pms_tbl_ProjectBudgetVersion
         WHERE regid = @regid AND ProjectId = @ProjectId AND status = 1 AND archive = 0
         ORDER BY VersionNo DESC;

    SELECT b.id, b.VersionId, b.MilestoneId, ISNULL(m.milestone, N'') AS MilestoneName,
           b.TaskId, ISNULL(t.taskname, N'') AS TaskName,
           b.CostCodeId, ISNULL(c.Code, N'') AS CostCode, ISNULL(c.Name, N'') AS CostCodeName, ISNULL(c.CostType, N'') AS CostType,
           b.Quantity, b.UnitId, ISNULL(u.unitname, ISNULL(b.Unit, N'')) AS Unit, b.Rate, b.Amount, ISNULL(b.Remarks, N'') AS Remarks
    FROM dbo.Pms_tbl_ProjectBudgetLine b WITH (NOLOCK)
    LEFT JOIN dbo.Pms_tbl_CostCode c WITH (NOLOCK) ON c.id = b.CostCodeId AND c.regid = b.regid
    LEFT JOIN dbo.Pms_tbl_AddMilestone m WITH (NOLOCK) ON m.id = b.MilestoneId AND m.regid = b.regid
    LEFT JOIN dbo.Pms_tbl_Addtask t WITH (NOLOCK) ON t.id = b.TaskId AND t.regid = b.regid
    LEFT JOIN dbo.tbl_unitmaster u WITH (NOLOCK) ON u.id = b.UnitId AND u.regid = b.regid
    WHERE b.regid = @regid AND b.ProjectId = @ProjectId AND b.VersionId = @VersionId
      AND b.status = 1 AND b.archive = 0
    ORDER BY c.Code, m.milestone, t.taskname, b.id;
END
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
               AND VersionStatus IN (N'Draft', N'Submitted') AND status = 1 AND archive = 0)
        BEGIN SELECT 0 AS Result, N'A draft or submitted budget already exists. Finish it first.' AS Message; RETURN; END

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

        IF @LastLockedId IS NOT NULL
            INSERT INTO dbo.Pms_tbl_ProjectBudgetLine (regid, VersionId, ProjectId, MilestoneId, TaskId, CostCodeId, Quantity, UnitId, Unit, Rate, Amount, Remarks, cby, cdate)
            SELECT regid, @NewId, ProjectId, MilestoneId, TaskId, CostCodeId, Quantity, UnitId, Unit, Rate, Amount, Remarks, @uid, GETDATE()
              FROM dbo.Pms_tbl_ProjectBudgetLine
             WHERE VersionId = @LastLockedId AND regid = @regid AND status = 1 AND archive = 0;

        DECLARE @new NVARCHAR(MAX) = (SELECT VersionNo, VersionType, VersionStatus, Reason FROM dbo.Pms_tbl_ProjectBudgetVersion WHERE id = @NewId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
        EXEC dbo.usp_pms_cc_Audit_Add @regid, @ProjectId, N'BudgetVersion', @NewId, N'Create', @Reason, NULL, @new, @uid;
    COMMIT TRANSACTION;

    SELECT 1 AS Result, N'Draft budget created.' AS Message, @NewId AS id;
END
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
    IF @VersionStatus <> N'Draft' BEGIN SELECT 0 AS Result, N'Only a draft budget can be changed.' AS Message; RETURN; END

    EXEC dbo.usp_pms_cc_ValidateHierarchy @regid, @ProjectId, @MilestoneId, @TaskId, @IsValid OUTPUT;
    IF @IsValid = 0 BEGIN SELECT 0 AS Result, N'Milestone or task does not belong to this project.' AS Message; RETURN; END
    IF dbo.ufn_pms_cc_IsPostingCode(@regid, @CostCodeId) = 0
        BEGIN SELECT 0 AS Result, N'Select an active Element-level cost code.' AS Message; RETURN; END

    DECLARE @UnitName NVARCHAR(50) = NULL;
    IF @UnitId IS NOT NULL
    BEGIN
        SELECT @UnitName = unitname FROM dbo.tbl_unitmaster WITH (NOLOCK)
         WHERE id = @UnitId AND regid = @regid AND ISNULL(status, 1) = 1 AND ISNULL(archive, 0) = 0;
        IF @UnitName IS NULL BEGIN SELECT 0 AS Result, N'Select an active unit.' AS Message; RETURN; END
    END

    IF @Quantity IS NOT NULL AND @Rate IS NOT NULL SET @Amount = ROUND(@Quantity * @Rate, 2);
    IF @Amount IS NULL OR @Amount < 0 BEGIN SELECT 0 AS Result, N'Enter a budget amount of zero or more.' AS Message; RETURN; END

    DECLARE @old NVARCHAR(MAX), @new NVARCHAR(MAX), @Action NVARCHAR(30) = N'Update';
    IF ISNULL(@id, 0) = 0
    BEGIN
        INSERT INTO dbo.Pms_tbl_ProjectBudgetLine (regid, VersionId, ProjectId, MilestoneId, TaskId, CostCodeId, Quantity, UnitId, Unit, Rate, Amount, Remarks, cby, cdate)
        VALUES (@regid, @VersionId, @ProjectId, @MilestoneId, @TaskId, @CostCodeId, @Quantity, @UnitId, @UnitName, @Rate, @Amount, NULLIF(LTRIM(RTRIM(@Remarks)), N''), @uid, GETDATE());
        SET @id = CAST(SCOPE_IDENTITY() AS INT); SET @Action = N'Create';
    END
    ELSE
    BEGIN
        SET @old = (SELECT VersionId, MilestoneId, TaskId, CostCodeId, Quantity, UnitId, Rate, Amount, Remarks FROM dbo.Pms_tbl_ProjectBudgetLine
                     WHERE id = @id AND regid = @regid FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
        UPDATE dbo.Pms_tbl_ProjectBudgetLine
           SET MilestoneId = @MilestoneId, TaskId = @TaskId, CostCodeId = @CostCodeId, Quantity = @Quantity,
               UnitId = @UnitId, Unit = @UnitName, Rate = @Rate, Amount = @Amount,
               Remarks = NULLIF(LTRIM(RTRIM(@Remarks)), N''), mby = @uid, mdate = GETDATE()
         WHERE id = @id AND VersionId = @VersionId AND regid = @regid AND status = 1 AND archive = 0;
        IF @@ROWCOUNT <> 1 BEGIN SELECT 0 AS Result, N'Budget line was not found.' AS Message; RETURN; END
    END
    SET @new = (SELECT VersionId, MilestoneId, TaskId, CostCodeId, Quantity, UnitId, Rate, Amount, Remarks FROM dbo.Pms_tbl_ProjectBudgetLine
                 WHERE id = @id FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
    EXEC dbo.usp_pms_cc_Audit_Add @regid, @ProjectId, N'BudgetLine', @id, @Action, NULL, @old, @new, @uid;
    SELECT 1 AS Result, N'Saved.' AS Message, @id AS id;
END
GO

ALTER PROCEDURE dbo.usp_pms_cc_BudgetLine_Delete @regid INT, @id INT, @uid INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ProjectId INT, @old NVARCHAR(MAX);
    SELECT @ProjectId = b.ProjectId FROM dbo.Pms_tbl_ProjectBudgetLine b
      INNER JOIN dbo.Pms_tbl_ProjectBudgetVersion v ON v.id = b.VersionId AND v.regid = b.regid
     WHERE b.id = @id AND b.regid = @regid AND b.status = 1 AND b.archive = 0 AND v.VersionStatus = N'Draft';
    IF @ProjectId IS NULL BEGIN SELECT 0 AS Result, N'Only lines of a draft budget can be deleted.' AS Message; RETURN; END
    SET @old = (SELECT VersionId, MilestoneId, TaskId, CostCodeId, Quantity, UnitId, Rate, Amount, Remarks FROM dbo.Pms_tbl_ProjectBudgetLine WHERE id = @id FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
    UPDATE dbo.Pms_tbl_ProjectBudgetLine SET status = 0, archive = 1, mby = @uid, mdate = GETDATE() WHERE id = @id AND regid = @regid;
    EXEC dbo.usp_pms_cc_Audit_Add @regid, @ProjectId, N'BudgetLine', @id, N'Delete', NULL, @old, NULL, @uid;
    SELECT 1 AS Result, N'Deleted.' AS Message;
END
GO

IF OBJECT_ID('dbo.usp_pms_cc_BudgetVersion_Submit', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_BudgetVersion_Submit AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_BudgetVersion_Submit @regid INT, @VersionId INT, @uid INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ProjectId INT;
    SELECT @ProjectId = ProjectId FROM dbo.Pms_tbl_ProjectBudgetVersion
     WHERE id = @VersionId AND regid = @regid AND VersionStatus = N'Draft' AND status = 1 AND archive = 0;
    IF @ProjectId IS NULL BEGIN SELECT 0 AS Result, N'Only a draft budget can be submitted.' AS Message; RETURN; END
    IF NOT EXISTS (SELECT 1 FROM dbo.Pms_tbl_ProjectBudgetLine WHERE VersionId = @VersionId AND regid = @regid AND status = 1 AND archive = 0)
        BEGIN SELECT 0 AS Result, N'Add at least one budget line before submitting.' AS Message; RETURN; END
    UPDATE dbo.Pms_tbl_ProjectBudgetVersion
       SET VersionStatus = N'Submitted', SubmittedBy = @uid, SubmittedOn = GETDATE(), RejectReason = NULL
     WHERE id = @VersionId AND regid = @regid;
    EXEC dbo.usp_pms_cc_Audit_Add @regid, @ProjectId, N'BudgetVersion', @VersionId, N'Submit', NULL, NULL, NULL, @uid;
    SELECT 1 AS Result, N'Budget submitted for approval.' AS Message;
END
GO

-- Approve = lock. AP-03: the approver cannot be the person who submitted the budget.
ALTER PROCEDURE dbo.usp_pms_cc_BudgetVersion_Lock @regid INT, @VersionId INT, @uid INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ProjectId INT, @SubmittedBy INT;
    SELECT @ProjectId = ProjectId, @SubmittedBy = SubmittedBy FROM dbo.Pms_tbl_ProjectBudgetVersion
     WHERE id = @VersionId AND regid = @regid AND VersionStatus = N'Submitted' AND status = 1 AND archive = 0;
    IF @ProjectId IS NULL BEGIN SELECT 0 AS Result, N'Only a submitted budget can be approved.' AS Message; RETURN; END
    IF @SubmittedBy = @uid BEGIN SELECT 0 AS Result, N'You submitted this budget, so another approver must approve it.' AS Message; RETURN; END
    UPDATE dbo.Pms_tbl_ProjectBudgetVersion
       SET VersionStatus = N'Locked', LockedBy = @uid, LockedOn = GETDATE()
     WHERE id = @VersionId AND regid = @regid;
    EXEC dbo.usp_pms_cc_Audit_Add @regid, @ProjectId, N'BudgetVersion', @VersionId, N'Approve', NULL, NULL, NULL, @uid;
    SELECT 1 AS Result, N'Budget approved and locked.' AS Message;
END
GO

IF OBJECT_ID('dbo.usp_pms_cc_BudgetVersion_Reject', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_BudgetVersion_Reject AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_BudgetVersion_Reject @regid INT, @VersionId INT, @Reason NVARCHAR(1000), @uid INT
AS
BEGIN
    SET NOCOUNT ON;
    SET @Reason = LTRIM(RTRIM(ISNULL(@Reason, N'')));
    IF @Reason = N'' BEGIN SELECT 0 AS Result, N'A reason is required to return the budget.' AS Message; RETURN; END
    DECLARE @ProjectId INT;
    SELECT @ProjectId = ProjectId FROM dbo.Pms_tbl_ProjectBudgetVersion
     WHERE id = @VersionId AND regid = @regid AND VersionStatus = N'Submitted' AND status = 1 AND archive = 0;
    IF @ProjectId IS NULL BEGIN SELECT 0 AS Result, N'Only a submitted budget can be returned.' AS Message; RETURN; END
    UPDATE dbo.Pms_tbl_ProjectBudgetVersion
       SET VersionStatus = N'Draft', RejectReason = @Reason, SubmittedBy = NULL, SubmittedOn = NULL
     WHERE id = @VersionId AND regid = @regid;
    EXEC dbo.usp_pms_cc_Audit_Add @regid, @ProjectId, N'BudgetVersion', @VersionId, N'Reject', @Reason, NULL, NULL, @uid;
    SELECT 1 AS Result, N'Budget returned to draft.' AS Message;
END
GO

ALTER PROCEDURE dbo.usp_pms_cc_BudgetVersion_DeleteDraft @regid INT, @VersionId INT, @uid INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ProjectId INT;
    SELECT @ProjectId = ProjectId FROM dbo.Pms_tbl_ProjectBudgetVersion WHERE id = @VersionId AND regid = @regid
       AND VersionStatus = N'Draft' AND status = 1 AND archive = 0;
    IF @ProjectId IS NULL BEGIN SELECT 0 AS Result, N'Only a draft budget can be discarded.' AS Message; RETURN; END
    SET XACT_ABORT ON;
    BEGIN TRANSACTION;
        UPDATE dbo.Pms_tbl_ProjectBudgetLine SET status = 0, archive = 1, mby = @uid, mdate = GETDATE()
         WHERE VersionId = @VersionId AND regid = @regid AND status = 1 AND archive = 0;
        UPDATE dbo.Pms_tbl_ProjectBudgetVersion SET status = 0, archive = 1 WHERE id = @VersionId AND regid = @regid;
        EXEC dbo.usp_pms_cc_Audit_Add @regid, @ProjectId, N'BudgetVersion', @VersionId, N'Discard', NULL, NULL, NULL, @uid;
    COMMIT TRANSACTION;
    SELECT 1 AS Result, N'Draft discarded.' AS Message;
END
GO

/* ---------------------------------------------------------------------
   7. Cost entries: site, source document, duplicate warning,
      period lock, mandatory reason on edit/delete, audit
   --------------------------------------------------------------------- */
ALTER PROCEDURE dbo.usp_pms_cc_CostEntry_List @regid INT, @ProjectId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT e.id, CONVERT(VARCHAR(10), e.EntryDate, 23) AS EntryDate,
           e.MilestoneId, ISNULL(m.milestone, N'') AS MilestoneName,
           e.TaskId, ISNULL(t.taskname, N'') AS TaskName,
           e.CostCodeId, ISNULL(c.Code, N'') AS CostCode, ISNULL(c.Name, N'') AS CostCodeName, ISNULL(c.CostType, N'') AS CostType,
           e.Amount, e.SourceType, ISNULL(e.SourceDocType, N'') AS SourceDocType, ISNULL(e.ReferenceNo, N'') AS ReferenceNo,
           ISNULL(e.SiteName, N'') AS SiteName, ISNULL(e.Remarks, N'') AS Remarks,
           CAST(dbo.ufn_pms_cc_IsPeriodClosed(e.regid, e.ProjectId, e.EntryDate) AS INT) AS IsPeriodClosed
    FROM dbo.Pms_tbl_ProjectCostEntry e WITH (NOLOCK)
    LEFT JOIN dbo.Pms_tbl_CostCode c WITH (NOLOCK) ON c.id = e.CostCodeId AND c.regid = e.regid
    LEFT JOIN dbo.Pms_tbl_AddMilestone m WITH (NOLOCK) ON m.id = e.MilestoneId AND m.regid = e.regid
    LEFT JOIN dbo.Pms_tbl_Addtask t WITH (NOLOCK) ON t.id = e.TaskId AND t.regid = e.regid
    WHERE e.regid = @regid AND e.ProjectId = @ProjectId AND e.status = 1 AND e.archive = 0
    ORDER BY e.EntryDate DESC, e.id DESC;
END
GO

ALTER PROCEDURE dbo.usp_pms_cc_CostEntry_Save
    @regid INT, @id INT, @ProjectId INT, @MilestoneId INT = NULL, @TaskId INT = NULL, @CostCodeId INT,
    @EntryDate DATE, @Amount DECIMAL(18,2), @ReferenceNo NVARCHAR(100) = NULL, @Remarks NVARCHAR(1000) = NULL, @uid INT,
    @SiteName NVARCHAR(100) = NULL, @SourceDocType NVARCHAR(30) = NULL, @Reason NVARCHAR(1000) = NULL, @ConfirmDuplicate BIT = 0
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @IsValid BIT;
    EXEC dbo.usp_pms_cc_ValidateHierarchy @regid, @ProjectId, @MilestoneId, @TaskId, @IsValid OUTPUT;
    IF @IsValid = 0 BEGIN SELECT 0 AS Result, N'Project, milestone or task is not valid.' AS Message; RETURN; END
    IF dbo.ufn_pms_cc_IsPostingCode(@regid, @CostCodeId) = 0
        BEGIN SELECT 0 AS Result, N'Select an active Element-level cost code.' AS Message; RETURN; END
    IF @EntryDate IS NULL BEGIN SELECT 0 AS Result, N'Enter the cost date.' AS Message; RETURN; END
    IF @Amount IS NULL OR @Amount = 0 BEGIN SELECT 0 AS Result, N'Enter a non-zero amount (negative for a reversal).' AS Message; RETURN; END
    SET @SourceDocType = NULLIF(LTRIM(RTRIM(@SourceDocType)), N'');
    IF @SourceDocType IS NOT NULL AND @SourceDocType NOT IN (N'Bill / Invoice', N'Voucher', N'Purchase Order', N'GRN', N'Petty Cash', N'Timesheet', N'Other')
        BEGIN SELECT 0 AS Result, N'Select a valid source document type.' AS Message; RETURN; END
    IF dbo.ufn_pms_cc_IsPeriodClosed(@regid, @ProjectId, @EntryDate) = 1
        BEGIN SELECT 0 AS Result, N'The month of this date is closed. Use a date in an open month.' AS Message; RETURN; END

    SET @ReferenceNo = NULLIF(LTRIM(RTRIM(@ReferenceNo)), N'');
    DECLARE @old NVARCHAR(MAX), @new NVARCHAR(MAX), @OldDate DATE;

    IF ISNULL(@id, 0) <> 0
    BEGIN
        SELECT @OldDate = EntryDate FROM dbo.Pms_tbl_ProjectCostEntry
         WHERE id = @id AND ProjectId = @ProjectId AND regid = @regid AND SourceType = N'Manual' AND status = 1 AND archive = 0;
        IF @OldDate IS NULL BEGIN SELECT 0 AS Result, N'Cost entry was not found.' AS Message; RETURN; END
        IF dbo.ufn_pms_cc_IsPeriodClosed(@regid, @ProjectId, @OldDate) = 1
            BEGIN SELECT 0 AS Result, N'This entry is in a closed month and cannot be changed.' AS Message; RETURN; END
        IF LTRIM(RTRIM(ISNULL(@Reason, N''))) = N''
            BEGIN SELECT 0 AS Result, N'Enter a reason for changing a saved cost entry.' AS Message; RETURN; END
    END

    -- Duplicate warning: the same document type and reference already used on another active entry.
    IF @ReferenceNo IS NOT NULL AND ISNULL(@ConfirmDuplicate, 0) = 0
    BEGIN
        DECLARE @DupText NVARCHAR(400);
        SELECT TOP 1 @DupText = N'Reference "' + @ReferenceNo + N'" is already used on ' + CONVERT(VARCHAR(10), e.EntryDate, 23)
                     + N' (' + ISNULL(p.ProjectName, N'') + N', amount ' + CONVERT(NVARCHAR(30), e.Amount) + N').'
          FROM dbo.Pms_tbl_ProjectCostEntry e
          LEFT JOIN dbo.Pms_tbl_AddProject p ON p.id = e.ProjectId AND p.regid = e.regid
         WHERE e.regid = @regid AND e.status = 1 AND e.archive = 0 AND e.id <> ISNULL(@id, 0)
           AND e.ReferenceNo = @ReferenceNo AND ISNULL(e.SourceDocType, N'') = ISNULL(@SourceDocType, N'');
        IF @DupText IS NOT NULL BEGIN SELECT 2 AS Result, @DupText + N' Save anyway?' AS Message; RETURN; END
    END

    IF ISNULL(@id, 0) = 0
    BEGIN
        INSERT INTO dbo.Pms_tbl_ProjectCostEntry (regid, ProjectId, MilestoneId, TaskId, CostCodeId, EntryDate, Amount, SourceType, SourceDocType, ReferenceNo, SiteName, Remarks, cby, cdate)
        VALUES (@regid, @ProjectId, @MilestoneId, @TaskId, @CostCodeId, @EntryDate, @Amount, N'Manual', @SourceDocType,
                @ReferenceNo, NULLIF(LTRIM(RTRIM(@SiteName)), N''), NULLIF(LTRIM(RTRIM(@Remarks)), N''), @uid, GETDATE());
        SET @id = CAST(SCOPE_IDENTITY() AS INT);
        SET @new = (SELECT EntryDate, MilestoneId, TaskId, CostCodeId, Amount, SourceDocType, ReferenceNo, SiteName, Remarks FROM dbo.Pms_tbl_ProjectCostEntry WHERE id = @id FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
        EXEC dbo.usp_pms_cc_Audit_Add @regid, @ProjectId, N'CostEntry', @id, N'Create', NULL, NULL, @new, @uid;
        SELECT 1 AS Result, N'Saved.' AS Message, @id AS id;
        RETURN;
    END

    SET @old = (SELECT EntryDate, MilestoneId, TaskId, CostCodeId, Amount, SourceDocType, ReferenceNo, SiteName, Remarks FROM dbo.Pms_tbl_ProjectCostEntry WHERE id = @id FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
    UPDATE dbo.Pms_tbl_ProjectCostEntry
       SET MilestoneId = @MilestoneId, TaskId = @TaskId, CostCodeId = @CostCodeId, EntryDate = @EntryDate, Amount = @Amount,
           SourceDocType = @SourceDocType, ReferenceNo = @ReferenceNo, SiteName = NULLIF(LTRIM(RTRIM(@SiteName)), N''),
           Remarks = NULLIF(LTRIM(RTRIM(@Remarks)), N''), mby = @uid, mdate = GETDATE()
     WHERE id = @id AND regid = @regid;
    SET @new = (SELECT EntryDate, MilestoneId, TaskId, CostCodeId, Amount, SourceDocType, ReferenceNo, SiteName, Remarks FROM dbo.Pms_tbl_ProjectCostEntry WHERE id = @id FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
    EXEC dbo.usp_pms_cc_Audit_Add @regid, @ProjectId, N'CostEntry', @id, N'Update', @Reason, @old, @new, @uid;
    SELECT 1 AS Result, N'Saved.' AS Message, @id AS id;
END
GO

IF OBJECT_ID('dbo.usp_pms_cc_CostEntry_Delete', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_CostEntry_Delete AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_CostEntry_Delete @regid INT, @id INT, @uid INT, @Reason NVARCHAR(1000) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF LTRIM(RTRIM(ISNULL(@Reason, N''))) = N''
        BEGIN SELECT 0 AS Result, N'Enter a reason for deleting this cost entry.' AS Message; RETURN; END
    DECLARE @ProjectId INT, @EntryDate DATE, @old NVARCHAR(MAX);
    SELECT @ProjectId = ProjectId, @EntryDate = EntryDate FROM dbo.Pms_tbl_ProjectCostEntry
     WHERE id = @id AND regid = @regid AND SourceType = N'Manual' AND status = 1 AND archive = 0;
    IF @ProjectId IS NULL BEGIN SELECT 0 AS Result, N'Cost entry was not found.' AS Message; RETURN; END
    IF dbo.ufn_pms_cc_IsPeriodClosed(@regid, @ProjectId, @EntryDate) = 1
        BEGIN SELECT 0 AS Result, N'This entry is in a closed month and cannot be deleted.' AS Message; RETURN; END
    SET @old = (SELECT EntryDate, MilestoneId, TaskId, CostCodeId, Amount, SourceDocType, ReferenceNo, SiteName, Remarks FROM dbo.Pms_tbl_ProjectCostEntry WHERE id = @id FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
    UPDATE dbo.Pms_tbl_ProjectCostEntry SET status = 0, archive = 1, mby = @uid, mdate = GETDATE() WHERE id = @id AND regid = @regid;
    EXEC dbo.usp_pms_cc_Audit_Add @regid, @ProjectId, N'CostEntry', @id, N'Delete', @Reason, @old, NULL, @uid;
    SELECT 1 AS Result, N'Deleted.' AS Message;
END
GO

/* ---------------------------------------------------------------------
   8. Estimate to complete (ETC)
   One current ETC per project x cost code x milestone; every change is in the audit history.
   --------------------------------------------------------------------- */
IF OBJECT_ID('dbo.usp_pms_cc_Etc_List', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_Etc_List AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_Etc_List @regid INT, @ProjectId INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @CurrentId INT;
    SELECT TOP 1 @CurrentId = id FROM dbo.Pms_tbl_ProjectBudgetVersion
     WHERE regid = @regid AND ProjectId = @ProjectId AND VersionStatus = N'Locked' AND status = 1 AND archive = 0
     ORDER BY VersionNo DESC;

    -- One row per cost code x milestone that has a budget, actual or ETC.
    ;WITH Keys AS
    (
        SELECT CostCodeId, MilestoneId FROM dbo.Pms_tbl_ProjectBudgetLine
         WHERE regid = @regid AND ProjectId = @ProjectId AND VersionId = ISNULL(@CurrentId, -1) AND status = 1 AND archive = 0
        UNION SELECT CostCodeId, MilestoneId FROM dbo.Pms_tbl_ProjectCostEntry
         WHERE regid = @regid AND ProjectId = @ProjectId AND status = 1 AND archive = 0
        UNION SELECT CostCodeId, MilestoneId FROM dbo.Pms_tbl_ProjectEtc
         WHERE regid = @regid AND ProjectId = @ProjectId AND status = 1 AND archive = 0
    )
    SELECT k.CostCodeId, k.MilestoneId, ISNULL(c.Code, N'') AS CostCode, ISNULL(c.Name, N'') AS CostCodeName,
           ISNULL(m.milestone, N'') AS MilestoneName,
           ISNULL((SELECT SUM(b.Amount) FROM dbo.Pms_tbl_ProjectBudgetLine b WHERE b.regid = @regid AND b.ProjectId = @ProjectId
                    AND b.VersionId = ISNULL(@CurrentId, -1) AND b.CostCodeId = k.CostCodeId AND ISNULL(b.MilestoneId, 0) = ISNULL(k.MilestoneId, 0)
                    AND b.status = 1 AND b.archive = 0), 0) AS CurrentBudget,
           ISNULL((SELECT SUM(e.Amount) FROM dbo.Pms_tbl_ProjectCostEntry e WHERE e.regid = @regid AND e.ProjectId = @ProjectId
                    AND e.CostCodeId = k.CostCodeId AND ISNULL(e.MilestoneId, 0) = ISNULL(k.MilestoneId, 0)
                    AND e.status = 1 AND e.archive = 0), 0) AS Actual,
           x.id AS EtcId, x.EtcAmount, CONVERT(VARCHAR(10), x.AsOfDate, 23) AS AsOfDate, ISNULL(x.Remarks, N'') AS Remarks
      FROM Keys k
      LEFT JOIN dbo.Pms_tbl_CostCode c ON c.id = k.CostCodeId AND c.regid = @regid
      LEFT JOIN dbo.Pms_tbl_AddMilestone m ON m.id = k.MilestoneId AND m.regid = @regid
      LEFT JOIN dbo.Pms_tbl_ProjectEtc x ON x.regid = @regid AND x.ProjectId = @ProjectId AND x.CostCodeId = k.CostCodeId
            AND ISNULL(x.MilestoneId, 0) = ISNULL(k.MilestoneId, 0) AND x.status = 1 AND x.archive = 0
     ORDER BY c.Code, m.milestone;
END
GO

IF OBJECT_ID('dbo.usp_pms_cc_Etc_Save', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_Etc_Save AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_Etc_Save
    @regid INT, @ProjectId INT, @CostCodeId INT, @MilestoneId INT = NULL, @EtcAmount DECIMAL(18,2),
    @AsOfDate DATE, @Remarks NVARCHAR(1000) = NULL, @uid INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @IsValid BIT;
    EXEC dbo.usp_pms_cc_ValidateHierarchy @regid, @ProjectId, @MilestoneId, NULL, @IsValid OUTPUT;
    IF @IsValid = 0 BEGIN SELECT 0 AS Result, N'Project or milestone is not valid.' AS Message; RETURN; END
    IF dbo.ufn_pms_cc_IsPostingCode(@regid, @CostCodeId) = 0
        BEGIN SELECT 0 AS Result, N'Select an active Element-level cost code.' AS Message; RETURN; END
    IF @EtcAmount IS NULL OR @EtcAmount < 0 BEGIN SELECT 0 AS Result, N'Enter an estimate to complete of zero or more.' AS Message; RETURN; END
    IF @AsOfDate IS NULL BEGIN SELECT 0 AS Result, N'Enter the as-of date.' AS Message; RETURN; END

    DECLARE @id INT, @old NVARCHAR(MAX), @new NVARCHAR(MAX), @Action NVARCHAR(30) = N'Update';
    SELECT @id = id FROM dbo.Pms_tbl_ProjectEtc WHERE regid = @regid AND ProjectId = @ProjectId AND CostCodeId = @CostCodeId
       AND ISNULL(MilestoneId, 0) = ISNULL(@MilestoneId, 0) AND status = 1 AND archive = 0;
    IF @id IS NULL
    BEGIN
        INSERT INTO dbo.Pms_tbl_ProjectEtc (regid, ProjectId, CostCodeId, MilestoneId, EtcAmount, AsOfDate, Remarks, cby, cdate)
        VALUES (@regid, @ProjectId, @CostCodeId, @MilestoneId, @EtcAmount, @AsOfDate, NULLIF(LTRIM(RTRIM(@Remarks)), N''), @uid, GETDATE());
        SET @id = CAST(SCOPE_IDENTITY() AS INT); SET @Action = N'Create';
    END
    ELSE
    BEGIN
        SET @old = (SELECT CostCodeId, MilestoneId, EtcAmount, AsOfDate, Remarks FROM dbo.Pms_tbl_ProjectEtc WHERE id = @id FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
        UPDATE dbo.Pms_tbl_ProjectEtc SET EtcAmount = @EtcAmount, AsOfDate = @AsOfDate, Remarks = NULLIF(LTRIM(RTRIM(@Remarks)), N''),
               mby = @uid, mdate = GETDATE()
         WHERE id = @id;
    END
    SET @new = (SELECT CostCodeId, MilestoneId, EtcAmount, AsOfDate, Remarks FROM dbo.Pms_tbl_ProjectEtc WHERE id = @id FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
    EXEC dbo.usp_pms_cc_Audit_Add @regid, @ProjectId, N'Etc', @id, @Action, NULL, @old, @new, @uid;
    SELECT 1 AS Result, N'Estimate saved.' AS Message, @id AS id;
END
GO

/* ---------------------------------------------------------------------
   9. Budget vs actual with forecast; grouping by cost type; project summary
   Current budget = latest LOCKED version. EAC = Actual + ETC. VAC = Current budget - EAC.
   --------------------------------------------------------------------- */
ALTER PROCEDURE dbo.usp_pms_cc_BudgetVsActual @regid INT, @ProjectId INT, @GroupBy VARCHAR(20) = 'COSTCODE'
AS
BEGIN
    SET NOCOUNT ON;
    IF @GroupBy NOT IN ('COSTCODE', 'MILESTONE', 'COSTTYPE') SET @GroupBy = 'COSTCODE';
    DECLARE @OriginalId INT, @CurrentId INT;
    SELECT TOP 1 @OriginalId = id FROM dbo.Pms_tbl_ProjectBudgetVersion
     WHERE regid = @regid AND ProjectId = @ProjectId AND VersionStatus = N'Locked' AND status = 1 AND archive = 0
     ORDER BY VersionNo ASC;
    SELECT TOP 1 @CurrentId = id FROM dbo.Pms_tbl_ProjectBudgetVersion
     WHERE regid = @regid AND ProjectId = @ProjectId AND VersionStatus = N'Locked' AND status = 1 AND archive = 0
     ORDER BY VersionNo DESC;

    ;WITH Src AS
    (
        SELECT b.CostCodeId, ISNULL(b.MilestoneId, 0) AS MilestoneId,
               CASE WHEN b.VersionId = @OriginalId THEN b.Amount ELSE 0 END AS OriginalBudget,
               CASE WHEN b.VersionId = @CurrentId  THEN b.Amount ELSE 0 END AS CurrentBudget,
               CAST(0 AS DECIMAL(18,2)) AS Actual, CAST(0 AS DECIMAL(18,2)) AS Etc
          FROM dbo.Pms_tbl_ProjectBudgetLine b
         WHERE b.regid = @regid AND b.ProjectId = @ProjectId AND b.status = 1 AND b.archive = 0
           AND b.VersionId IN (ISNULL(@OriginalId, -1), ISNULL(@CurrentId, -1))
        UNION ALL
        SELECT e.CostCodeId, ISNULL(e.MilestoneId, 0), 0, 0, e.Amount, 0
          FROM dbo.Pms_tbl_ProjectCostEntry e
         WHERE e.regid = @regid AND e.ProjectId = @ProjectId AND e.status = 1 AND e.archive = 0
        UNION ALL
        SELECT x.CostCodeId, ISNULL(x.MilestoneId, 0), 0, 0, 0, x.EtcAmount
          FROM dbo.Pms_tbl_ProjectEtc x
         WHERE x.regid = @regid AND x.ProjectId = @ProjectId AND x.status = 1 AND x.archive = 0
    ),
    Grouped AS
    (
        SELECT CASE WHEN @GroupBy = 'MILESTONE' THEN CAST(s.MilestoneId AS NVARCHAR(40))
                    WHEN @GroupBy = 'COSTTYPE'  THEN ISNULL(c.CostType, N'(none)')
                    ELSE CAST(s.CostCodeId AS NVARCHAR(40)) END AS GroupKey,
               s.CostCodeId, s.MilestoneId, s.OriginalBudget, s.CurrentBudget, s.Actual, s.Etc, c.CostType
          FROM Src s
          LEFT JOIN dbo.Pms_tbl_CostCode c ON c.id = s.CostCodeId AND c.regid = @regid
    ),
    Agg AS
    (
        SELECT GroupKey, MIN(CostCodeId) AS AnyCostCodeId, MIN(MilestoneId) AS AnyMilestoneId,
               SUM(OriginalBudget) AS OriginalBudget, SUM(CurrentBudget) AS CurrentBudget, SUM(Actual) AS Actual, SUM(Etc) AS Etc
          FROM Grouped GROUP BY GroupKey
    )
    SELECT a.GroupKey AS GroupId,
           CASE WHEN @GroupBy = 'MILESTONE'
                THEN CASE WHEN a.AnyMilestoneId = 0 THEN N'Project level (no milestone)' ELSE ISNULL(m.milestone, N'(removed milestone)') END
                WHEN @GroupBy = 'COSTTYPE' THEN a.GroupKey
                ELSE ISNULL(c.Code + N' - ' + c.Name, N'(removed cost code)') END AS GroupName,
           CASE WHEN @GroupBy = 'COSTCODE' THEN ISNULL(c.CostType, N'') ELSE N'' END AS CostType,
           a.OriginalBudget, a.CurrentBudget, a.Actual,
           a.CurrentBudget - a.Actual AS Variance,
           a.Etc, a.Actual + a.Etc AS Eac, a.CurrentBudget - (a.Actual + a.Etc) AS Vac,
           CASE WHEN a.CurrentBudget = 0 THEN NULL ELSE CAST(ROUND(a.Actual * 100.0 / a.CurrentBudget, 2) AS DECIMAL(18,2)) END AS ConsumedPercent
      FROM Agg a
      LEFT JOIN dbo.Pms_tbl_CostCode c ON @GroupBy = 'COSTCODE' AND c.id = a.AnyCostCodeId AND c.regid = @regid
      LEFT JOIN dbo.Pms_tbl_AddMilestone m ON @GroupBy = 'MILESTONE' AND m.id = a.AnyMilestoneId AND m.regid = @regid
     ORDER BY GroupName;

    SELECT (SELECT VersionNo FROM dbo.Pms_tbl_ProjectBudgetVersion WHERE id = @CurrentId) AS CurrentVersionNo,
           (SELECT COUNT(1) FROM dbo.Pms_tbl_ProjectBudgetVersion WHERE regid = @regid AND ProjectId = @ProjectId
             AND VersionStatus IN (N'Draft', N'Submitted') AND status = 1 AND archive = 0) AS DraftCount;
END
GO

IF OBJECT_ID('dbo.usp_pms_cc_ProjectSummary', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_ProjectSummary AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_ProjectSummary @regid INT
AS
BEGIN
    SET NOCOUNT ON;
    ;WITH CurrentVersion AS
    (
        SELECT ProjectId, MAX(VersionNo) AS VersionNo FROM dbo.Pms_tbl_ProjectBudgetVersion
         WHERE regid = @regid AND VersionStatus = N'Locked' AND status = 1 AND archive = 0 GROUP BY ProjectId
    ),
    Budget AS
    (
        SELECT v.ProjectId, cv.VersionNo, SUM(b.Amount) AS CurrentBudget
          FROM CurrentVersion cv
          INNER JOIN dbo.Pms_tbl_ProjectBudgetVersion v ON v.ProjectId = cv.ProjectId AND v.VersionNo = cv.VersionNo AND v.regid = @regid AND v.status = 1 AND v.archive = 0
          INNER JOIN dbo.Pms_tbl_ProjectBudgetLine b ON b.VersionId = v.id AND b.regid = @regid AND b.status = 1 AND b.archive = 0
         GROUP BY v.ProjectId, cv.VersionNo
    ),
    Actual AS
    (
        SELECT ProjectId, SUM(Amount) AS Actual FROM dbo.Pms_tbl_ProjectCostEntry
         WHERE regid = @regid AND status = 1 AND archive = 0 GROUP BY ProjectId
    ),
    Etc AS
    (
        SELECT ProjectId, SUM(EtcAmount) AS Etc FROM dbo.Pms_tbl_ProjectEtc
         WHERE regid = @regid AND status = 1 AND archive = 0 GROUP BY ProjectId
    ),
    OpenVersion AS
    (
        SELECT ProjectId, MAX(VersionStatus) AS OpenStatus FROM dbo.Pms_tbl_ProjectBudgetVersion
         WHERE regid = @regid AND VersionStatus IN (N'Draft', N'Submitted') AND status = 1 AND archive = 0 GROUP BY ProjectId
    )
    SELECT p.id AS ProjectId, ISNULL(p.ProjectName, N'') AS ProjectName,
           b.VersionNo AS CurrentVersionNo, ISNULL(o.OpenStatus, N'') AS OpenVersionStatus,
           ISNULL(b.CurrentBudget, 0) AS CurrentBudget, ISNULL(a.Actual, 0) AS Actual, ISNULL(x.Etc, 0) AS Etc,
           ISNULL(a.Actual, 0) + ISNULL(x.Etc, 0) AS Eac,
           ISNULL(b.CurrentBudget, 0) - (ISNULL(a.Actual, 0) + ISNULL(x.Etc, 0)) AS Vac,
           CASE WHEN ISNULL(b.CurrentBudget, 0) = 0 THEN NULL ELSE CAST(ROUND(ISNULL(a.Actual, 0) * 100.0 / b.CurrentBudget, 2) AS DECIMAL(18,2)) END AS ConsumedPercent
      FROM dbo.Pms_tbl_AddProject p WITH (NOLOCK)
      LEFT JOIN Budget b ON b.ProjectId = p.id
      LEFT JOIN Actual a ON a.ProjectId = p.id
      LEFT JOIN Etc x ON x.ProjectId = p.id
      LEFT JOIN OpenVersion o ON o.ProjectId = p.id
     WHERE p.regid = @regid AND p.status = 1 AND p.archive = 0
       AND (b.ProjectId IS NOT NULL OR a.ProjectId IS NOT NULL OR x.ProjectId IS NOT NULL OR o.ProjectId IS NOT NULL)
     ORDER BY p.ProjectName;
END
GO

/* ---------------------------------------------------------------------
   10. Monthly period lock
   --------------------------------------------------------------------- */
IF OBJECT_ID('dbo.usp_pms_cc_Period_List', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_Period_List AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_Period_List @regid INT, @ProjectId INT
AS
BEGIN
    SET NOCOUNT ON;
    -- Months from the earliest cost entry (or the current month) up to the current month.
    DECLARE @First DATE, @Last DATE = DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1);
    SELECT @First = MIN(DATEFROMPARTS(YEAR(EntryDate), MONTH(EntryDate), 1)) FROM dbo.Pms_tbl_ProjectCostEntry
     WHERE regid = @regid AND ProjectId = @ProjectId AND status = 1 AND archive = 0;
    SELECT @First = CASE WHEN @First IS NULL OR @First > @Last THEN @Last ELSE @First END;
    SELECT @First = CASE WHEN MIN(PeriodStart) < @First THEN MIN(PeriodStart) ELSE @First END
      FROM dbo.Pms_tbl_CostControlPeriod WHERE regid = @regid AND ProjectId = @ProjectId;

    ;WITH Months AS
    (
        SELECT @First AS PeriodStart
        UNION ALL SELECT DATEADD(MONTH, 1, PeriodStart) FROM Months WHERE PeriodStart < @Last
    )
    SELECT CONVERT(VARCHAR(10), mo.PeriodStart, 23) AS PeriodStart,
           CAST(ISNULL(p.IsClosed, 0) AS INT) AS IsClosed,
           p.ClosedOn, ISNULL(ec.DisplayName, N'') AS ClosedByName,
           p.ReopenedOn, ISNULL(er.DisplayName, N'') AS ReopenedByName, ISNULL(p.ReopenReason, N'') AS ReopenReason,
           (SELECT COUNT(1) FROM dbo.Pms_tbl_ProjectCostEntry e WHERE e.regid = @regid AND e.ProjectId = @ProjectId AND e.status = 1 AND e.archive = 0
              AND e.EntryDate >= mo.PeriodStart AND e.EntryDate < DATEADD(MONTH, 1, mo.PeriodStart)) AS EntryCount,
           ISNULL((SELECT SUM(e.Amount) FROM dbo.Pms_tbl_ProjectCostEntry e WHERE e.regid = @regid AND e.ProjectId = @ProjectId AND e.status = 1 AND e.archive = 0
              AND e.EntryDate >= mo.PeriodStart AND e.EntryDate < DATEADD(MONTH, 1, mo.PeriodStart)), 0) AS EntryTotal
      FROM Months mo
      LEFT JOIN dbo.Pms_tbl_CostControlPeriod p ON p.regid = @regid AND p.ProjectId = @ProjectId AND p.PeriodStart = mo.PeriodStart
      LEFT JOIN dbo.tbl_employee ec ON ec.id = p.ClosedBy
      LEFT JOIN dbo.tbl_employee er ON er.id = p.ReopenedBy
     ORDER BY mo.PeriodStart DESC
    OPTION (MAXRECURSION 1200);
END
GO

IF OBJECT_ID('dbo.usp_pms_cc_Period_SetStatus', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_Period_SetStatus AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_Period_SetStatus
    @regid INT, @ProjectId INT, @PeriodStart DATE, @Close BIT, @Reason NVARCHAR(1000) = NULL, @uid INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @IsValid BIT;
    EXEC dbo.usp_pms_cc_ValidateHierarchy @regid, @ProjectId, NULL, NULL, @IsValid OUTPUT;
    IF @IsValid = 0 BEGIN SELECT 0 AS Result, N'Project was not found.' AS Message; RETURN; END
    SET @PeriodStart = DATEFROMPARTS(YEAR(@PeriodStart), MONTH(@PeriodStart), 1);
    IF @PeriodStart > DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1)
        BEGIN SELECT 0 AS Result, N'A future month cannot be closed.' AS Message; RETURN; END
    IF @Close = 0 AND LTRIM(RTRIM(ISNULL(@Reason, N''))) = N''
        BEGIN SELECT 0 AS Result, N'A reason is required to reopen a month.' AS Message; RETURN; END

    DECLARE @IsClosed BIT;
    SELECT @IsClosed = IsClosed FROM dbo.Pms_tbl_CostControlPeriod WHERE regid = @regid AND ProjectId = @ProjectId AND PeriodStart = @PeriodStart;
    IF @Close = 1 AND ISNULL(@IsClosed, 0) = 1 BEGIN SELECT 0 AS Result, N'This month is already closed.' AS Message; RETURN; END
    IF @Close = 0 AND ISNULL(@IsClosed, 0) = 0 BEGIN SELECT 0 AS Result, N'This month is not closed.' AS Message; RETURN; END

    IF @IsClosed IS NULL
        INSERT INTO dbo.Pms_tbl_CostControlPeriod (regid, ProjectId, PeriodStart, IsClosed, ClosedBy, ClosedOn)
        VALUES (@regid, @ProjectId, @PeriodStart, 1, @uid, GETDATE());
    ELSE IF @Close = 1
        UPDATE dbo.Pms_tbl_CostControlPeriod SET IsClosed = 1, ClosedBy = @uid, ClosedOn = GETDATE()
         WHERE regid = @regid AND ProjectId = @ProjectId AND PeriodStart = @PeriodStart;
    ELSE
        UPDATE dbo.Pms_tbl_CostControlPeriod SET IsClosed = 0, ReopenedBy = @uid, ReopenedOn = GETDATE(), ReopenReason = @Reason
         WHERE regid = @regid AND ProjectId = @ProjectId AND PeriodStart = @PeriodStart;

    DECLARE @Detail NVARCHAR(MAX) = (SELECT CONVERT(VARCHAR(10), @PeriodStart, 23) AS PeriodStart FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
    EXEC dbo.usp_pms_cc_Audit_Add @regid, @ProjectId, N'Period', NULL, CASE WHEN @Close = 1 THEN N'Close' ELSE N'Reopen' END, @Reason, NULL, @Detail, @uid;
    SELECT 1 AS Result, CASE WHEN @Close = 1 THEN N'Month closed.' ELSE N'Month reopened.' END AS Message;
END
GO

/* ---------------------------------------------------------------------
   11. Audit history (read)
   --------------------------------------------------------------------- */
IF OBJECT_ID('dbo.usp_pms_cc_Audit_List', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_Audit_List AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_Audit_List @regid INT, @ProjectId INT = NULL, @Top INT = 500
AS
BEGIN
    SET NOCOUNT ON;
    -- Project history plus company-level history (cost codes, access) when no project is given.
    SELECT TOP (@Top) a.id, CONVERT(VARCHAR(16), a.cdate, 120) AS ActionTime, a.EntityType, a.EntityId, a.Action,
           ISNULL(a.Reason, N'') AS Reason, ISNULL(a.OldData, N'') AS OldData, ISNULL(a.NewData, N'') AS NewData,
           ISNULL(e.DisplayName, N'') AS UserName, ISNULL(p.ProjectName, N'') AS ProjectName
      FROM dbo.Pms_tbl_CostControlAudit a WITH (NOLOCK)
      LEFT JOIN dbo.tbl_employee e WITH (NOLOCK) ON e.id = a.uid
      LEFT JOIN dbo.Pms_tbl_AddProject p WITH (NOLOCK) ON p.id = a.ProjectId AND p.regid = a.regid
     WHERE a.regid = @regid
       AND ((@ProjectId IS NULL AND a.ProjectId IS NULL) OR a.ProjectId = @ProjectId)
     ORDER BY a.id DESC;
END
GO
