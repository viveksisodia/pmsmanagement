/* =====================================================================
   PMS Cost Control - Phase 3 database script
   ---------------------------------------------------------------------
   Run AFTER costcontrol_phase1.sql and costcontrol_phase2.sql
   (always in the order phase1 -> phase2 -> phase3). Safe to re-run.

   Adds:
     1. Monthly snapshots on close (immutable, version n+1 on reopen) (PE-01, PE-03, PE-06, AC-20)
        + fixes: future-dated entries rejected; approval right checked
          inside the procedures; snapshot info for exports.
     2. Budget movements: zero-net transfers, contingency reserves,
        releases / returns / additions, corrections;
        RB = OB + revisions + corrections + approved changes + net transfers;
        RB + RSV constant on release/return                       (BV-1, BV-3, RS-1..RS-7, RP-04)
     3. Change register: 6 classes, 4 independent statuses, PCE / PPC / EACp,
        at-risk authorisation and exposure                        (CM-1..CM-7, AR-01..AR-05, RP-05)
     4. Split allocation of a cost entry totalling exactly the entry (DM-03, REQ-005)
     5. Optional per-project access + pending-approvals list      (NF-04)

   Safety: ADDITIVE ONLY. New tables / columns only on Cost Control tables.
   Existing PMS / ERP tables are READ only. Requires SQL Server 2016+
   (FOR JSON, OPENJSON, STRING_SPLIT; database compatibility level 130+).
   ===================================================================== */

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

/* =====================================================================
   1. TABLES
   ===================================================================== */
IF OBJECT_ID('dbo.Pms_tbl_CostControlSnapshot', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Pms_tbl_CostControlSnapshot
    (
        id                INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Pms_tbl_CostControlSnapshot PRIMARY KEY,
        regid             INT           NOT NULL,
        ProjectId         INT           NOT NULL,
        PeriodStart       DATE          NOT NULL,
        SnapshotVersion   INT           NOT NULL,     -- 1, 2, ... (n+1 after each reopen + close)
        BaselineVersionNo INT           NULL,         -- latest approved budget version at the time
        TotalRB           DECIMAL(18,2) NOT NULL,
        TotalRSV          DECIMAL(18,2) NOT NULL,
        PCE               DECIMAL(18,2) NOT NULL,
        PPC               DECIMAL(18,2) NOT NULL,
        AtRiskExposure    DECIMAL(18,2) NOT NULL,
        Unrecovered       DECIMAL(18,2) NOT NULL,
        CreatedBy         INT           NULL,
        CreatedOn         DATETIME      NOT NULL CONSTRAINT DF_Pms_tbl_CostControlSnapshot_on DEFAULT (GETDATE())
    );
    CREATE UNIQUE INDEX UX_Pms_tbl_CostControlSnapshot ON dbo.Pms_tbl_CostControlSnapshot (regid, ProjectId, PeriodStart, SnapshotVersion);
END
GO

IF OBJECT_ID('dbo.Pms_tbl_CostControlSnapshotLine', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Pms_tbl_CostControlSnapshotLine
    (
        id            INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Pms_tbl_CostControlSnapshotLine PRIMARY KEY,
        SnapshotId    INT           NOT NULL,
        regid         INT           NOT NULL,
        CostCodeId    INT           NOT NULL,
        MilestoneId   INT           NOT NULL,          -- 0 = project level
        CostCode      NVARCHAR(30)  NULL,
        CostCodeName  NVARCHAR(200) NULL,
        CostType      NVARCHAR(30)  NULL,
        MilestoneName NVARCHAR(500) NULL,
        OB            DECIMAL(18,2) NOT NULL,
        RevisionAdj   DECIMAL(18,2) NOT NULL,
        Corrections   DECIMAL(18,2) NOT NULL,
        ApprovedChanges DECIMAL(18,2) NOT NULL,
        NetTransfers  DECIMAL(18,2) NOT NULL,
        RB            DECIMAL(18,2) NOT NULL,
        Actual        DECIMAL(18,2) NOT NULL,          -- cost dated up to the end of the month
        Etc           DECIMAL(18,2) NOT NULL
    );
    CREATE INDEX IX_Pms_tbl_CostControlSnapshotLine ON dbo.Pms_tbl_CostControlSnapshotLine (SnapshotId);
END
GO

-- Snapshots are immutable.
IF OBJECT_ID('dbo.trg_Pms_tbl_CostControlSnapshot_NoChange', 'TR') IS NULL
    EXEC('CREATE TRIGGER dbo.trg_Pms_tbl_CostControlSnapshot_NoChange ON dbo.Pms_tbl_CostControlSnapshot
          INSTEAD OF UPDATE, DELETE AS BEGIN RAISERROR(N''Cost Control snapshots cannot be changed or deleted.'', 16, 1); END');
GO
IF OBJECT_ID('dbo.trg_Pms_tbl_CostControlSnapshotLine_NoChange', 'TR') IS NULL
    EXEC('CREATE TRIGGER dbo.trg_Pms_tbl_CostControlSnapshotLine_NoChange ON dbo.Pms_tbl_CostControlSnapshotLine
          INSTEAD OF UPDATE, DELETE AS BEGIN RAISERROR(N''Cost Control snapshots cannot be changed or deleted.'', 16, 1); END');
GO

IF OBJECT_ID('dbo.Pms_tbl_ProjectReserve', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Pms_tbl_ProjectReserve
    (
        id          INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Pms_tbl_ProjectReserve PRIMARY KEY,
        regid       INT            NOT NULL,
        ProjectId   INT            NOT NULL,
        ReserveType NVARCHAR(30)   NOT NULL,   -- ContractorContingency / ClientAllowance / ProvisionalSum
        Name        NVARCHAR(200)  NOT NULL,
        IsInternal  BIT            NOT NULL,   -- contractor contingency is internal by default (RS-7)
        Remarks     NVARCHAR(1000) NULL,
        status      INT            NOT NULL CONSTRAINT DF_Pms_tbl_ProjectReserve_status  DEFAULT (1),
        archive     INT            NOT NULL CONSTRAINT DF_Pms_tbl_ProjectReserve_archive DEFAULT (0),
        cby         INT            NULL,
        cdate       DATETIME       NOT NULL CONSTRAINT DF_Pms_tbl_ProjectReserve_cdate DEFAULT (GETDATE())
    );
END
GO

IF OBJECT_ID('dbo.Pms_tbl_BudgetMovement', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Pms_tbl_BudgetMovement
    (
        id             INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Pms_tbl_BudgetMovement PRIMARY KEY,
        regid          INT            NOT NULL,
        ProjectId      INT            NOT NULL,
        MovementNo     INT            NOT NULL,
        MovementType   NVARCHAR(30)   NOT NULL,   -- Transfer / ReserveAddition / ReserveRelease / ReserveReturn / Correction / ApprovedChange
        MovementStatus NVARCHAR(20)   NOT NULL,   -- Draft / Submitted / Approved / Rejected
        Reason         NVARCHAR(1000) NOT NULL,
        RiskReference  NVARCHAR(100)  NULL,
        ChangeId       INT            NULL,       -- Pms_tbl_ChangeRecord.id for an approved change
        SubmittedBy    INT NULL, SubmittedOn DATETIME NULL,
        ApprovedBy     INT NULL, ApprovedOn  DATETIME NULL,
        RejectReason   NVARCHAR(1000) NULL,
        status         INT            NOT NULL CONSTRAINT DF_Pms_tbl_BudgetMovement_status  DEFAULT (1),
        archive        INT            NOT NULL CONSTRAINT DF_Pms_tbl_BudgetMovement_archive DEFAULT (0),
        cby            INT            NULL,
        cdate          DATETIME       NOT NULL CONSTRAINT DF_Pms_tbl_BudgetMovement_cdate DEFAULT (GETDATE())
    );
    CREATE INDEX IX_Pms_tbl_BudgetMovement_project ON dbo.Pms_tbl_BudgetMovement (regid, ProjectId, MovementStatus);
END
GO

IF OBJECT_ID('dbo.Pms_tbl_BudgetMovementLine', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Pms_tbl_BudgetMovementLine
    (
        id          INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Pms_tbl_BudgetMovementLine PRIMARY KEY,
        regid       INT            NOT NULL,
        MovementId  INT            NOT NULL,
        LineKind    NVARCHAR(10)   NOT NULL,      -- Budget / Reserve
        CostCodeId  INT            NULL,          -- Budget lines
        MilestoneId INT            NULL,          -- Budget lines (NULL = project level)
        ReserveId   INT            NULL,          -- Reserve lines
        Amount      DECIMAL(18,2)  NOT NULL,      -- + increases, - decreases
        Remarks     NVARCHAR(500)  NULL,
        status      INT            NOT NULL CONSTRAINT DF_Pms_tbl_BudgetMovementLine_status  DEFAULT (1),
        archive     INT            NOT NULL CONSTRAINT DF_Pms_tbl_BudgetMovementLine_archive DEFAULT (0)
    );
    CREATE INDEX IX_Pms_tbl_BudgetMovementLine_movement ON dbo.Pms_tbl_BudgetMovementLine (MovementId, status, archive);
END
GO

IF OBJECT_ID('dbo.Pms_tbl_ChangeRecord', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Pms_tbl_ChangeRecord
    (
        id                    INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Pms_tbl_ChangeRecord PRIMARY KEY,
        regid                 INT            NOT NULL,
        ProjectId             INT            NOT NULL,
        ChangeNo              INT            NOT NULL,
        Title                 NVARCHAR(200)  NOT NULL,
        Description           NVARCHAR(2000) NULL,
        ChangeClass           NVARCHAR(30)   NOT NULL,   -- CM-1 classes
        RecordStatus          NVARCHAR(20)   NOT NULL,   -- Open / Withdrawn / Closed
        InternalStatus        NVARCHAR(20)   NOT NULL,   -- Pending / Approved / Rejected            (internal cost authorisation)
        ClientStatus          NVARCHAR(20)   NOT NULL,   -- NotApplicable / Pending / Submitted / Approved / Rejected
        SubcontractStatus     NVARCHAR(20)   NOT NULL,   -- NotApplicable / Pending / Approved / Rejected
        BudgetPostingStatus   NVARCHAR(20)   NOT NULL,   -- NotPosted / Posted
        EstimatedCost         DECIMAL(18,2)  NOT NULL,
        AmountInEtc           DECIMAL(18,2)  NOT NULL,   -- part of the cost already inside ETC (F-2)
        IsProbable            BIT            NOT NULL,   -- counts in PPC (policy Q9-F1 is Finance's)
        ClientValueProposed   DECIMAL(18,2)  NULL,
        ClientValueApproved   DECIMAL(18,2)  NULL,
        SubcontractValue      DECIMAL(18,2)  NULL,
        IsAtRisk              BIT            NOT NULL,
        AtRiskAuthorisedBy    INT NULL, AtRiskAuthorisedOn DATETIME NULL,
        InternalDecisionBy    INT NULL, InternalDecisionOn DATETIME NULL,
        ClientDecisionOn      DATE NULL, SubcontractDecisionOn DATE NULL,
        PostedOn              DATETIME NULL,
        status                INT            NOT NULL CONSTRAINT DF_Pms_tbl_ChangeRecord_status  DEFAULT (1),
        archive               INT            NOT NULL CONSTRAINT DF_Pms_tbl_ChangeRecord_archive DEFAULT (0),
        cby                   INT            NULL,
        cdate                 DATETIME       NOT NULL CONSTRAINT DF_Pms_tbl_ChangeRecord_cdate DEFAULT (GETDATE()),
        mby                   INT            NULL,
        mdate                 DATETIME       NULL
    );
    CREATE INDEX IX_Pms_tbl_ChangeRecord_project ON dbo.Pms_tbl_ChangeRecord (regid, ProjectId, RecordStatus);
END
GO

IF OBJECT_ID('dbo.Pms_tbl_ChangeAllocation', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Pms_tbl_ChangeAllocation
    (
        id          INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Pms_tbl_ChangeAllocation PRIMARY KEY,
        regid       INT           NOT NULL,
        ChangeId    INT           NOT NULL,
        CostCodeId  INT           NOT NULL,
        MilestoneId INT           NULL,
        Amount      DECIMAL(18,2) NOT NULL,
        status      INT           NOT NULL CONSTRAINT DF_Pms_tbl_ChangeAllocation_status  DEFAULT (1),
        archive     INT           NOT NULL CONSTRAINT DF_Pms_tbl_ChangeAllocation_archive DEFAULT (0)
    );
    CREATE INDEX IX_Pms_tbl_ChangeAllocation_change ON dbo.Pms_tbl_ChangeAllocation (ChangeId, status, archive);
END
GO

IF OBJECT_ID('dbo.Pms_tbl_CostEntrySplit', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Pms_tbl_CostEntrySplit
    (
        id          INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Pms_tbl_CostEntrySplit PRIMARY KEY,
        regid       INT           NOT NULL,
        EntryId     INT           NOT NULL,      -- Pms_tbl_ProjectCostEntry.id (the source line)
        CostCodeId  INT           NOT NULL,
        MilestoneId INT           NULL,
        TaskId      INT           NULL,
        Amount      DECIMAL(18,2) NOT NULL,
        SplitPercent DECIMAL(9,4) NOT NULL,
        status      INT           NOT NULL CONSTRAINT DF_Pms_tbl_CostEntrySplit_status  DEFAULT (1),
        archive     INT           NOT NULL CONSTRAINT DF_Pms_tbl_CostEntrySplit_archive DEFAULT (0),
        cby         INT           NULL,
        cdate       DATETIME      NOT NULL CONSTRAINT DF_Pms_tbl_CostEntrySplit_cdate DEFAULT (GETDATE())
    );
    CREATE INDEX IX_Pms_tbl_CostEntrySplit_entry ON dbo.Pms_tbl_CostEntrySplit (EntryId, status, archive);
END
GO

IF OBJECT_ID('dbo.Pms_tbl_CostControlAccessProject', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Pms_tbl_CostControlAccessProject
    (
        id         INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Pms_tbl_CostControlAccessProject PRIMARY KEY,
        regid      INT      NOT NULL,
        EmployeeId INT      NOT NULL,
        ProjectId  INT      NOT NULL,
        cby        INT      NULL,
        cdate      DATETIME NOT NULL CONSTRAINT DF_Pms_tbl_CostControlAccessProject_cdate DEFAULT (GETDATE())
    );
    CREATE INDEX IX_Pms_tbl_CostControlAccessProject ON dbo.Pms_tbl_CostControlAccessProject (regid, EmployeeId);
END
GO

IF COL_LENGTH('dbo.Pms_tbl_ProjectCostEntry', 'ChangeId') IS NULL
    ALTER TABLE dbo.Pms_tbl_ProjectCostEntry ADD ChangeId INT NULL;
GO

/* =====================================================================
   2. VIEWS AND FUNCTIONS
   ===================================================================== */

-- Every reportable cost allocation: an entry itself, or its split rows when it is split (DM-03).
IF OBJECT_ID('dbo.vw_pms_cc_CostAllocation', 'V') IS NOT NULL DROP VIEW dbo.vw_pms_cc_CostAllocation;
GO
CREATE VIEW dbo.vw_pms_cc_CostAllocation
AS
    SELECT e.regid, e.ProjectId, e.id AS EntryId, e.EntryDate, e.CostCodeId, e.MilestoneId, e.TaskId, e.Amount, e.ChangeId
      FROM dbo.Pms_tbl_ProjectCostEntry e
     WHERE e.status = 1 AND e.archive = 0
       AND NOT EXISTS (SELECT 1 FROM dbo.Pms_tbl_CostEntrySplit s WHERE s.EntryId = e.id AND s.status = 1 AND s.archive = 0)
    UNION ALL
    SELECT e.regid, e.ProjectId, e.id, e.EntryDate, s.CostCodeId, s.MilestoneId, s.TaskId, s.Amount, e.ChangeId
      FROM dbo.Pms_tbl_ProjectCostEntry e
      INNER JOIN dbo.Pms_tbl_CostEntrySplit s ON s.EntryId = e.id AND s.status = 1 AND s.archive = 0
     WHERE e.status = 1 AND e.archive = 0;
GO

/* Revised budget per cost code x milestone (requirements 3.1):
   OB              = first approved version
   RevisionAdj     = latest approved version - OB      (re-baselines)
   Corrections     = approved Correction movements after the latest approval   (ADJ)
   ApprovedChanges = approved ApprovedChange movements after it                 (ACH)
   NetTransfers    = approved Transfer / ReserveRelease / ReserveReturn budget lines after it (NTR)
   RB              = latest approved version + Corrections + ApprovedChanges + NetTransfers
   A new revision copies RB, so movements before the latest approval are already inside it. */
IF OBJECT_ID('dbo.tvf_pms_cc_RevisedBudget', 'IF') IS NOT NULL DROP FUNCTION dbo.tvf_pms_cc_RevisedBudget;
GO
CREATE FUNCTION dbo.tvf_pms_cc_RevisedBudget (@regid INT, @ProjectId INT)
RETURNS TABLE
AS
RETURN
    WITH Locked AS
    (
        SELECT id, VersionNo, LockedOn FROM dbo.Pms_tbl_ProjectBudgetVersion
         WHERE regid = @regid AND ProjectId = @ProjectId AND VersionStatus = N'Locked' AND status = 1 AND archive = 0
    ),
    Orig AS (SELECT TOP 1 id FROM Locked ORDER BY VersionNo ASC),
    Cur  AS (SELECT TOP 1 id, LockedOn FROM Locked ORDER BY VersionNo DESC),
    Src AS
    (
        SELECT b.CostCodeId, ISNULL(b.MilestoneId, 0) AS MilestoneId, b.Amount AS OB, CAST(0 AS DECIMAL(18,2)) AS Baseline,
               CAST(0 AS DECIMAL(18,2)) AS Corr, CAST(0 AS DECIMAL(18,2)) AS Ach, CAST(0 AS DECIMAL(18,2)) AS Ntr
          FROM dbo.Pms_tbl_ProjectBudgetLine b INNER JOIN Orig o ON b.VersionId = o.id
         WHERE b.regid = @regid AND b.status = 1 AND b.archive = 0
        UNION ALL
        SELECT b.CostCodeId, ISNULL(b.MilestoneId, 0), 0, b.Amount, 0, 0, 0
          FROM dbo.Pms_tbl_ProjectBudgetLine b INNER JOIN Cur c ON b.VersionId = c.id
         WHERE b.regid = @regid AND b.status = 1 AND b.archive = 0
        UNION ALL
        SELECT l.CostCodeId, ISNULL(l.MilestoneId, 0), 0, 0,
               CASE WHEN m.MovementType = N'Correction' THEN l.Amount ELSE 0 END,
               CASE WHEN m.MovementType = N'ApprovedChange' THEN l.Amount ELSE 0 END,
               CASE WHEN m.MovementType IN (N'Transfer', N'ReserveRelease', N'ReserveReturn') THEN l.Amount ELSE 0 END
          FROM dbo.Pms_tbl_BudgetMovementLine l
          INNER JOIN dbo.Pms_tbl_BudgetMovement m ON m.id = l.MovementId AND m.regid = l.regid
          INNER JOIN Cur c ON m.ApprovedOn > c.LockedOn
         WHERE l.regid = @regid AND l.LineKind = N'Budget' AND l.status = 1 AND l.archive = 0
           AND m.ProjectId = @ProjectId AND m.MovementStatus = N'Approved' AND m.status = 1 AND m.archive = 0
    )
    SELECT CostCodeId, MilestoneId,
           SUM(OB) AS OB, SUM(Baseline) - SUM(OB) AS RevisionAdj, SUM(Corr) AS Corrections, SUM(Ach) AS ApprovedChanges,
           SUM(Ntr) AS NetTransfers, SUM(Baseline) + SUM(Corr) + SUM(Ach) + SUM(Ntr) AS RB
      FROM Src
     GROUP BY CostCodeId, MilestoneId;
GO

-- Reserve balances (RS-1..RS-6). Reserves are independent of budget versions.
IF OBJECT_ID('dbo.tvf_pms_cc_ReserveBalance', 'IF') IS NOT NULL DROP FUNCTION dbo.tvf_pms_cc_ReserveBalance;
GO
CREATE FUNCTION dbo.tvf_pms_cc_ReserveBalance (@regid INT, @ProjectId INT)
RETURNS TABLE
AS
RETURN
    SELECT r.id AS ReserveId, r.ReserveType, r.Name, r.IsInternal,
           ISNULL(SUM(CASE WHEN m.MovementType = N'ReserveAddition' THEN l.Amount END), 0) AS Additions,
           ISNULL(SUM(CASE WHEN m.MovementType = N'ReserveRelease'  THEN -l.Amount END), 0) AS Releases,
           ISNULL(SUM(CASE WHEN m.MovementType = N'ReserveReturn'   THEN l.Amount END), 0) AS Returns,
           ISNULL(SUM(l.Amount), 0) AS Remaining
      FROM dbo.Pms_tbl_ProjectReserve r
      LEFT JOIN dbo.Pms_tbl_BudgetMovementLine l ON l.ReserveId = r.id AND l.regid = r.regid AND l.LineKind = N'Reserve' AND l.status = 1 AND l.archive = 0
            AND EXISTS (SELECT 1 FROM dbo.Pms_tbl_BudgetMovement mm WHERE mm.id = l.MovementId AND mm.MovementStatus = N'Approved' AND mm.status = 1 AND mm.archive = 0)
      LEFT JOIN dbo.Pms_tbl_BudgetMovement m ON m.id = l.MovementId
     WHERE r.regid = @regid AND r.ProjectId = @ProjectId AND r.status = 1 AND r.archive = 0
     GROUP BY r.id, r.ReserveType, r.Name, r.IsInternal;
GO

-- Project-level measures: reserves, pending changes, at-risk exposure (RP-01, CM-5, AR-02, AR-05).
IF OBJECT_ID('dbo.tvf_pms_cc_ProjectMeasures', 'IF') IS NOT NULL DROP FUNCTION dbo.tvf_pms_cc_ProjectMeasures;
GO
CREATE FUNCTION dbo.tvf_pms_cc_ProjectMeasures (@regid INT, @ProjectId INT, @UpTo DATE)
RETURNS TABLE
AS
RETURN
    SELECT
        ISNULL((SELECT SUM(Remaining) FROM dbo.tvf_pms_cc_ReserveBalance(@regid, @ProjectId)), 0) AS RSV,
        ISNULL((SELECT SUM(EstimatedCost) FROM dbo.Pms_tbl_ChangeRecord c
                 WHERE c.regid = @regid AND c.ProjectId = @ProjectId AND c.status = 1 AND c.archive = 0
                   AND c.RecordStatus = N'Open' AND c.BudgetPostingStatus = N'NotPosted' AND c.InternalStatus <> N'Rejected'), 0) AS PCE,
        ISNULL((SELECT SUM(CASE WHEN c.EstimatedCost - c.AmountInEtc > 0 THEN c.EstimatedCost - c.AmountInEtc ELSE 0 END)
                  FROM dbo.Pms_tbl_ChangeRecord c
                 WHERE c.regid = @regid AND c.ProjectId = @ProjectId AND c.status = 1 AND c.archive = 0 AND c.IsProbable = 1
                   AND c.RecordStatus = N'Open' AND c.BudgetPostingStatus = N'NotPosted' AND c.InternalStatus <> N'Rejected'), 0) AS PPC,
        ISNULL((SELECT SUM(a.Amount) FROM dbo.vw_pms_cc_CostAllocation a
                 INNER JOIN dbo.Pms_tbl_ChangeRecord c ON c.id = a.ChangeId AND c.regid = a.regid
                 WHERE a.regid = @regid AND a.ProjectId = @ProjectId AND c.IsAtRisk = 1
                   AND c.ClientStatus NOT IN (N'Approved', N'Rejected') AND (@UpTo IS NULL OR a.EntryDate <= @UpTo)), 0) AS AtRiskExposure,
        ISNULL((SELECT SUM(a.Amount) FROM dbo.vw_pms_cc_CostAllocation a
                 INNER JOIN dbo.Pms_tbl_ChangeRecord c ON c.id = a.ChangeId AND c.regid = a.regid
                 WHERE a.regid = @regid AND a.ProjectId = @ProjectId AND c.IsAtRisk = 1
                   AND c.ClientStatus = N'Rejected' AND (@UpTo IS NULL OR a.EntryDate <= @UpTo)), 0) AS Unrecovered;
GO

-- Approval right inside the database (defence in depth, AP-03 / 5.1).
IF OBJECT_ID('dbo.ufn_pms_cc_CanApprove', 'FN') IS NOT NULL DROP FUNCTION dbo.ufn_pms_cc_CanApprove;
GO
CREATE FUNCTION dbo.ufn_pms_cc_CanApprove (@regid INT, @uid INT)
RETURNS BIT
AS
BEGIN
    RETURN CASE WHEN EXISTS (SELECT 1 FROM dbo.Pms_tbl_CostControlAccess
                              WHERE regid = @regid AND EmployeeId = @uid AND CanApprove = 1 AND status = 1 AND archive = 0)
                THEN 1 ELSE 0 END;
END
GO

-- Per-project access: no project rows = all company projects (keeps earlier behaviour).
IF OBJECT_ID('dbo.ufn_pms_cc_ProjectAllowed', 'FN') IS NOT NULL DROP FUNCTION dbo.ufn_pms_cc_ProjectAllowed;
GO
CREATE FUNCTION dbo.ufn_pms_cc_ProjectAllowed (@regid INT, @uid INT, @IsAdmin BIT, @ProjectId INT)
RETURNS BIT
AS
BEGIN
    IF @IsAdmin = 1 RETURN 1;
    IF NOT EXISTS (SELECT 1 FROM dbo.Pms_tbl_CostControlAccessProject WHERE regid = @regid AND EmployeeId = @uid) RETURN 1;
    RETURN CASE WHEN EXISTS (SELECT 1 FROM dbo.Pms_tbl_CostControlAccessProject WHERE regid = @regid AND EmployeeId = @uid AND ProjectId = @ProjectId)
                THEN 1 ELSE 0 END;
END
GO

/* =====================================================================
   3. ACCESS: per-project list, project filter, resolver, pending approvals
   ===================================================================== */
ALTER PROCEDURE dbo.usp_pms_cc_Project_List @regid INT, @uid INT = NULL, @IsAdmin BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.id, ISNULL(p.ProjectName, N'') AS ProjectName
      FROM dbo.Pms_tbl_AddProject p WITH (NOLOCK)
     WHERE p.regid = @regid AND p.status = 1 AND p.archive = 0
       AND (@uid IS NULL OR dbo.ufn_pms_cc_ProjectAllowed(@regid, @uid, @IsAdmin, p.id) = 1)
     ORDER BY p.ProjectName;
END
GO

IF OBJECT_ID('dbo.usp_pms_cc_ProjectAllowed', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_ProjectAllowed AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_ProjectAllowed @regid INT, @uid INT, @IsAdmin BIT, @ProjectId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CAST(CASE WHEN EXISTS (SELECT 1 FROM dbo.Pms_tbl_AddProject WHERE id = @ProjectId AND regid = @regid AND status = 1 AND archive = 0)
                      AND dbo.ufn_pms_cc_ProjectAllowed(@regid, @uid, @IsAdmin, @ProjectId) = 1 THEN 1 ELSE 0 END AS INT) AS Allowed;
END
GO

-- Finds the project of a record so the server can apply per-project access to id-based actions.
IF OBJECT_ID('dbo.usp_pms_cc_ResolveProject', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_ResolveProject AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_ResolveProject @regid INT, @EntityType NVARCHAR(30), @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CASE @EntityType
             WHEN N'BudgetVersion' THEN (SELECT ProjectId FROM dbo.Pms_tbl_ProjectBudgetVersion WHERE id = @Id AND regid = @regid)
             WHEN N'BudgetLine'    THEN (SELECT ProjectId FROM dbo.Pms_tbl_ProjectBudgetLine WHERE id = @Id AND regid = @regid)
             WHEN N'CostEntry'     THEN (SELECT ProjectId FROM dbo.Pms_tbl_ProjectCostEntry WHERE id = @Id AND regid = @regid)
             WHEN N'Movement'      THEN (SELECT ProjectId FROM dbo.Pms_tbl_BudgetMovement WHERE id = @Id AND regid = @regid)
             WHEN N'Change'        THEN (SELECT ProjectId FROM dbo.Pms_tbl_ChangeRecord WHERE id = @Id AND regid = @regid)
             WHEN N'Reserve'       THEN (SELECT ProjectId FROM dbo.Pms_tbl_ProjectReserve WHERE id = @Id AND regid = @regid)
             WHEN N'Snapshot'      THEN (SELECT ProjectId FROM dbo.Pms_tbl_CostControlSnapshot WHERE id = @Id AND regid = @regid)
           END AS ProjectId;
END
GO

IF OBJECT_ID('dbo.usp_pms_cc_Access_ProjectList', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_Access_ProjectList AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_Access_ProjectList @regid INT, @EmployeeId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ProjectId FROM dbo.Pms_tbl_CostControlAccessProject WHERE regid = @regid AND EmployeeId = @EmployeeId;
END
GO

IF OBJECT_ID('dbo.usp_pms_cc_Access_ProjectSave', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_Access_ProjectSave AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_Access_ProjectSave @regid INT, @EmployeeId INT, @ProjectIds NVARCHAR(MAX) = NULL, @uid INT
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM dbo.tbl_employee WHERE id = @EmployeeId AND regid = @regid AND status = 1 AND archive = 0)
        BEGIN SELECT 0 AS Result, N'Employee was not found.' AS Message; RETURN; END
    DECLARE @old NVARCHAR(MAX) = (SELECT ProjectId FROM dbo.Pms_tbl_CostControlAccessProject WHERE regid = @regid AND EmployeeId = @EmployeeId FOR JSON PATH);
    SET XACT_ABORT ON;
    BEGIN TRANSACTION;
        DELETE FROM dbo.Pms_tbl_CostControlAccessProject WHERE regid = @regid AND EmployeeId = @EmployeeId;
        INSERT INTO dbo.Pms_tbl_CostControlAccessProject (regid, EmployeeId, ProjectId, cby, cdate)
        SELECT DISTINCT @regid, @EmployeeId, p.id, @uid, GETDATE()
          FROM STRING_SPLIT(ISNULL(@ProjectIds, N''), N',') s
          INNER JOIN dbo.Pms_tbl_AddProject p ON p.id = TRY_CONVERT(INT, LTRIM(RTRIM(s.value))) AND p.regid = @regid AND p.status = 1 AND p.archive = 0;
        DECLARE @new NVARCHAR(MAX) = (SELECT ProjectId FROM dbo.Pms_tbl_CostControlAccessProject WHERE regid = @regid AND EmployeeId = @EmployeeId FOR JSON PATH);
        EXEC dbo.usp_pms_cc_Audit_Add @regid, NULL, N'AccessProjects', @EmployeeId, N'Update', NULL, @old, @new, @uid;
    COMMIT TRANSACTION;
    SELECT 1 AS Result, CASE WHEN @new IS NULL THEN N'Access to all projects.' ELSE N'Project access saved.' END AS Message;
END
GO

IF OBJECT_ID('dbo.usp_pms_cc_PendingApprovals', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_PendingApprovals AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_PendingApprovals @regid INT, @uid INT, @IsAdmin BIT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT x.ItemType, x.ItemId, x.ProjectId, ISNULL(p.ProjectName, N'') AS ProjectName, x.Title, x.Amount,
           x.RequestedBy, ISNULL(e.DisplayName, N'') AS RequestedByName, CONVERT(VARCHAR(16), x.RequestedOn, 120) AS RequestedOn,
           CAST(CASE WHEN x.RequestedBy = @uid THEN 1 ELSE 0 END AS INT) AS IsOwn
      FROM
      (
          SELECT N'Budget' AS ItemType, v.id AS ItemId, v.ProjectId, N'Budget V' + CAST(v.VersionNo AS NVARCHAR(10)) + N' - ' + v.VersionType AS Title,
                 (SELECT SUM(b.Amount) FROM dbo.Pms_tbl_ProjectBudgetLine b WHERE b.VersionId = v.id AND b.status = 1 AND b.archive = 0) AS Amount,
                 v.SubmittedBy AS RequestedBy, v.SubmittedOn AS RequestedOn
            FROM dbo.Pms_tbl_ProjectBudgetVersion v
           WHERE v.regid = @regid AND v.VersionStatus = N'Submitted' AND v.status = 1 AND v.archive = 0
          UNION ALL
          SELECT N'Movement', m.id, m.ProjectId, N'Movement #' + CAST(m.MovementNo AS NVARCHAR(10)) + N' - ' + m.MovementType,
                 (SELECT SUM(ABS(l.Amount)) FROM dbo.Pms_tbl_BudgetMovementLine l WHERE l.MovementId = m.id AND l.status = 1 AND l.archive = 0 AND l.Amount > 0),
                 m.SubmittedBy, m.SubmittedOn
            FROM dbo.Pms_tbl_BudgetMovement m
           WHERE m.regid = @regid AND m.MovementStatus = N'Submitted' AND m.status = 1 AND m.archive = 0
          UNION ALL
          SELECT N'Change', c.id, c.ProjectId, N'Change #' + CAST(c.ChangeNo AS NVARCHAR(10)) + N' - ' + c.Title + N' (internal authorisation)',
                 c.EstimatedCost, c.cby, c.cdate
            FROM dbo.Pms_tbl_ChangeRecord c
           WHERE c.regid = @regid AND c.RecordStatus = N'Open' AND c.InternalStatus = N'Pending' AND c.status = 1 AND c.archive = 0
          UNION ALL
          SELECT N'AtRisk', c.id, c.ProjectId, N'Change #' + CAST(c.ChangeNo AS NVARCHAR(10)) + N' - ' + c.Title + N' (at-risk authorisation)',
                 c.EstimatedCost, c.cby, c.cdate
            FROM dbo.Pms_tbl_ChangeRecord c
           WHERE c.regid = @regid AND c.RecordStatus = N'Open' AND c.IsAtRisk = 1 AND c.AtRiskAuthorisedOn IS NULL AND c.status = 1 AND c.archive = 0
      ) x
      LEFT JOIN dbo.Pms_tbl_AddProject p ON p.id = x.ProjectId AND p.regid = @regid
      LEFT JOIN dbo.tbl_employee e ON e.id = x.RequestedBy
     WHERE dbo.ufn_pms_cc_ProjectAllowed(@regid, @uid, @IsAdmin, x.ProjectId) = 1
     ORDER BY x.RequestedOn;
END
GO

/* =====================================================================
   4. BUDGET VERSIONS: revisions carry RB forward; approval right checked in the database
   ===================================================================== */
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
    IF EXISTS (SELECT 1 FROM dbo.Pms_tbl_BudgetMovement WHERE regid = @regid AND ProjectId = @ProjectId
               AND MovementStatus IN (N'Draft', N'Submitted') AND status = 1 AND archive = 0)
        BEGIN SELECT 0 AS Result, N'Finish or discard the open budget movements before starting a revision.' AS Message; RETURN; END

    DECLARE @LastLockedId INT, @LastLockedOn DATETIME, @NextNo INT, @NewId INT;
    SELECT TOP 1 @LastLockedId = id, @LastLockedOn = LockedOn FROM dbo.Pms_tbl_ProjectBudgetVersion
     WHERE regid = @regid AND ProjectId = @ProjectId AND VersionStatus = N'Locked' AND status = 1 AND archive = 0
     ORDER BY VersionNo DESC;
    SELECT @NextNo = ISNULL(MAX(VersionNo), 0) + 1 FROM dbo.Pms_tbl_ProjectBudgetVersion
     WHERE regid = @regid AND ProjectId = @ProjectId AND status = 1 AND archive = 0;
    IF @LastLockedId IS NOT NULL AND LTRIM(RTRIM(ISNULL(@Reason, N''))) = N''
        BEGIN SELECT 0 AS Result, N'A reason is required for a budget revision.' AS Message; RETURN; END

    SET XACT_ABORT ON;
    BEGIN TRANSACTION;
        INSERT INTO dbo.Pms_tbl_ProjectBudgetVersion (regid, ProjectId, VersionNo, VersionType, VersionStatus, Reason, cby, cdate)
        VALUES (@regid, @ProjectId, @NextNo, CASE WHEN @LastLockedId IS NULL THEN N'Original' ELSE N'Revision' END,
                N'Draft', NULLIF(LTRIM(RTRIM(@Reason)), N''), @uid, GETDATE());
        SET @NewId = CAST(SCOPE_IDENTITY() AS INT);

        IF @LastLockedId IS NOT NULL
        BEGIN
            -- Copy the last approved version ...
            INSERT INTO dbo.Pms_tbl_ProjectBudgetLine (regid, VersionId, ProjectId, MilestoneId, TaskId, CostCodeId, Quantity, UnitId, Unit, Rate, Amount, Remarks, cby, cdate)
            SELECT regid, @NewId, ProjectId, MilestoneId, TaskId, CostCodeId, Quantity, UnitId, Unit, Rate, Amount, Remarks, @uid, GETDATE()
              FROM dbo.Pms_tbl_ProjectBudgetLine
             WHERE VersionId = @LastLockedId AND regid = @regid AND status = 1 AND archive = 0;
            -- ... plus the net of movements approved since, so the revision starts from RB.
            INSERT INTO dbo.Pms_tbl_ProjectBudgetLine (regid, VersionId, ProjectId, MilestoneId, TaskId, CostCodeId, Amount, Remarks, cby, cdate)
            SELECT @regid, @NewId, @ProjectId, NULLIF(l.MilestoneId, 0), NULL, l.CostCodeId, SUM(l.Amount), N'Carried forward from approved budget movements', @uid, GETDATE()
              FROM dbo.Pms_tbl_BudgetMovementLine l
              INNER JOIN dbo.Pms_tbl_BudgetMovement m ON m.id = l.MovementId
             WHERE m.regid = @regid AND m.ProjectId = @ProjectId AND m.MovementStatus = N'Approved' AND m.ApprovedOn > @LastLockedOn
               AND m.status = 1 AND m.archive = 0 AND l.LineKind = N'Budget' AND l.status = 1 AND l.archive = 0
             GROUP BY l.CostCodeId, NULLIF(l.MilestoneId, 0)
            HAVING SUM(l.Amount) <> 0;
        END

        DECLARE @new NVARCHAR(MAX) = (SELECT VersionNo, VersionType, VersionStatus, Reason FROM dbo.Pms_tbl_ProjectBudgetVersion WHERE id = @NewId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
        EXEC dbo.usp_pms_cc_Audit_Add @regid, @ProjectId, N'BudgetVersion', @NewId, N'Create', @Reason, NULL, @new, @uid;
    COMMIT TRANSACTION;
    SELECT 1 AS Result, N'Draft budget created.' AS Message, @NewId AS id;
END
GO

ALTER PROCEDURE dbo.usp_pms_cc_BudgetVersion_Lock @regid INT, @VersionId INT, @uid INT
AS
BEGIN
    SET NOCOUNT ON;
    IF dbo.ufn_pms_cc_CanApprove(@regid, @uid) = 0 BEGIN SELECT 0 AS Result, N'You do not have Cost Control approval rights.' AS Message; RETURN; END
    DECLARE @ProjectId INT, @SubmittedBy INT;
    SELECT @ProjectId = ProjectId, @SubmittedBy = SubmittedBy FROM dbo.Pms_tbl_ProjectBudgetVersion
     WHERE id = @VersionId AND regid = @regid AND VersionStatus = N'Submitted' AND status = 1 AND archive = 0;
    IF @ProjectId IS NULL BEGIN SELECT 0 AS Result, N'Only a submitted budget can be approved.' AS Message; RETURN; END
    IF @SubmittedBy = @uid BEGIN SELECT 0 AS Result, N'You submitted this budget, so another approver must approve it.' AS Message; RETURN; END
    UPDATE dbo.Pms_tbl_ProjectBudgetVersion SET VersionStatus = N'Locked', LockedBy = @uid, LockedOn = GETDATE()
     WHERE id = @VersionId AND regid = @regid;
    EXEC dbo.usp_pms_cc_Audit_Add @regid, @ProjectId, N'BudgetVersion', @VersionId, N'Approve', NULL, NULL, NULL, @uid;
    SELECT 1 AS Result, N'Budget approved and locked.' AS Message;
END
GO

ALTER PROCEDURE dbo.usp_pms_cc_BudgetVersion_Reject @regid INT, @VersionId INT, @Reason NVARCHAR(1000), @uid INT
AS
BEGIN
    SET NOCOUNT ON;
    IF dbo.ufn_pms_cc_CanApprove(@regid, @uid) = 0 BEGIN SELECT 0 AS Result, N'You do not have Cost Control approval rights.' AS Message; RETURN; END
    SET @Reason = LTRIM(RTRIM(ISNULL(@Reason, N'')));
    IF @Reason = N'' BEGIN SELECT 0 AS Result, N'A reason is required to return the budget.' AS Message; RETURN; END
    DECLARE @ProjectId INT;
    SELECT @ProjectId = ProjectId FROM dbo.Pms_tbl_ProjectBudgetVersion
     WHERE id = @VersionId AND regid = @regid AND VersionStatus = N'Submitted' AND status = 1 AND archive = 0;
    IF @ProjectId IS NULL BEGIN SELECT 0 AS Result, N'Only a submitted budget can be returned.' AS Message; RETURN; END
    UPDATE dbo.Pms_tbl_ProjectBudgetVersion SET VersionStatus = N'Draft', RejectReason = @Reason, SubmittedBy = NULL, SubmittedOn = NULL
     WHERE id = @VersionId AND regid = @regid;
    EXEC dbo.usp_pms_cc_Audit_Add @regid, @ProjectId, N'BudgetVersion', @VersionId, N'Reject', @Reason, NULL, NULL, @uid;
    SELECT 1 AS Result, N'Budget returned to draft.' AS Message;
END
GO

/* =====================================================================
   5. RESERVES AND BUDGET MOVEMENTS
   ===================================================================== */
IF OBJECT_ID('dbo.usp_pms_cc_Reserve_Save', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_Reserve_Save AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_Reserve_Save
    @regid INT, @id INT, @ProjectId INT, @ReserveType NVARCHAR(30), @Name NVARCHAR(200), @Remarks NVARCHAR(1000) = NULL, @uid INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @IsValid BIT;
    EXEC dbo.usp_pms_cc_ValidateHierarchy @regid, @ProjectId, NULL, NULL, @IsValid OUTPUT;
    IF @IsValid = 0 BEGIN SELECT 0 AS Result, N'Project was not found.' AS Message; RETURN; END
    IF @ReserveType NOT IN (N'ContractorContingency', N'ClientAllowance', N'ProvisionalSum')
        BEGIN SELECT 0 AS Result, N'Select a valid reserve type.' AS Message; RETURN; END
    SET @Name = LTRIM(RTRIM(ISNULL(@Name, N'')));
    IF @Name = N'' BEGIN SELECT 0 AS Result, N'Enter the reserve name.' AS Message; RETURN; END
    DECLARE @IsInternal BIT = CASE WHEN @ReserveType = N'ContractorContingency' THEN 1 ELSE 0 END;
    IF ISNULL(@id, 0) = 0
    BEGIN
        INSERT INTO dbo.Pms_tbl_ProjectReserve (regid, ProjectId, ReserveType, Name, IsInternal, Remarks, cby, cdate)
        VALUES (@regid, @ProjectId, @ReserveType, @Name, @IsInternal, NULLIF(LTRIM(RTRIM(@Remarks)), N''), @uid, GETDATE());
        SET @id = CAST(SCOPE_IDENTITY() AS INT);
        EXEC dbo.usp_pms_cc_Audit_Add @regid, @ProjectId, N'Reserve', @id, N'Create', NULL, NULL, NULL, @uid;
    END
    ELSE
    BEGIN
        -- The type cannot change once the reserve has movements.
        IF EXISTS (SELECT 1 FROM dbo.Pms_tbl_BudgetMovementLine WHERE ReserveId = @id AND regid = @regid AND status = 1 AND archive = 0)
           AND NOT EXISTS (SELECT 1 FROM dbo.Pms_tbl_ProjectReserve WHERE id = @id AND ReserveType = @ReserveType)
            BEGIN SELECT 0 AS Result, N'The reserve type cannot change after movements exist.' AS Message; RETURN; END
        UPDATE dbo.Pms_tbl_ProjectReserve SET ReserveType = @ReserveType, Name = @Name, IsInternal = @IsInternal, Remarks = NULLIF(LTRIM(RTRIM(@Remarks)), N'')
         WHERE id = @id AND regid = @regid AND ProjectId = @ProjectId AND status = 1 AND archive = 0;
        EXEC dbo.usp_pms_cc_Audit_Add @regid, @ProjectId, N'Reserve', @id, N'Update', NULL, NULL, NULL, @uid;
    END
    SELECT 1 AS Result, N'Reserve saved.' AS Message, @id AS id;
END
GO

IF OBJECT_ID('dbo.usp_pms_cc_Reserve_Report', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_Reserve_Report AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_Reserve_Report @regid INT, @ProjectId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT r.ReserveId AS id, r.ReserveType, r.Name, CAST(r.IsInternal AS INT) AS IsInternal,
           r.Additions, r.Releases, r.Returns, r.Remaining, ISNULL(x.Remarks, N'') AS Remarks
      FROM dbo.tvf_pms_cc_ReserveBalance(@regid, @ProjectId) r
      LEFT JOIN dbo.Pms_tbl_ProjectReserve x ON x.id = r.ReserveId
     ORDER BY r.ReserveType, r.Name;
END
GO

IF OBJECT_ID('dbo.usp_pms_cc_Movement_List', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_Movement_List AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_Movement_List @regid INT, @ProjectId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT m.id, m.MovementNo, m.MovementType, m.MovementStatus, m.Reason, ISNULL(m.RiskReference, N'') AS RiskReference,
           m.ChangeId, c.ChangeNo, m.cby, m.SubmittedBy, ISNULL(es.DisplayName, N'') AS SubmittedByName, m.SubmittedOn,
           ISNULL(ea.DisplayName, N'') AS ApprovedByName, m.ApprovedOn, ISNULL(m.RejectReason, N'') AS RejectReason,
           ISNULL((SELECT SUM(l.Amount) FROM dbo.Pms_tbl_BudgetMovementLine l WHERE l.MovementId = m.id AND l.status = 1 AND l.archive = 0 AND l.LineKind = N'Budget'), 0) AS BudgetNet,
           ISNULL((SELECT SUM(l.Amount) FROM dbo.Pms_tbl_BudgetMovementLine l WHERE l.MovementId = m.id AND l.status = 1 AND l.archive = 0 AND l.LineKind = N'Reserve'), 0) AS ReserveNet
      FROM dbo.Pms_tbl_BudgetMovement m
      LEFT JOIN dbo.Pms_tbl_ChangeRecord c ON c.id = m.ChangeId
      LEFT JOIN dbo.tbl_employee es ON es.id = m.SubmittedBy
      LEFT JOIN dbo.tbl_employee ea ON ea.id = m.ApprovedBy
     WHERE m.regid = @regid AND m.ProjectId = @ProjectId AND m.status = 1 AND m.archive = 0
     ORDER BY m.MovementNo DESC;

    SELECT l.id, l.MovementId, l.LineKind, l.CostCodeId, l.MilestoneId, l.ReserveId, l.Amount, ISNULL(l.Remarks, N'') AS Remarks,
           ISNULL(cc.Code + N' - ' + cc.Name, N'') AS CostCodeLabel, ISNULL(ms.milestone, N'') AS MilestoneName, ISNULL(r.Name, N'') AS ReserveName
      FROM dbo.Pms_tbl_BudgetMovementLine l
      INNER JOIN dbo.Pms_tbl_BudgetMovement m ON m.id = l.MovementId AND m.regid = l.regid
      LEFT JOIN dbo.Pms_tbl_CostCode cc ON cc.id = l.CostCodeId
      LEFT JOIN dbo.Pms_tbl_AddMilestone ms ON ms.id = l.MilestoneId
      LEFT JOIN dbo.Pms_tbl_ProjectReserve r ON r.id = l.ReserveId
     WHERE m.regid = @regid AND m.ProjectId = @ProjectId AND m.status = 1 AND m.archive = 0 AND l.status = 1 AND l.archive = 0
     ORDER BY l.MovementId, l.id;
END
GO

/* Validates a movement's lines by type. @CheckBalances = 1 also checks that no budget
   line (RB) or reserve goes below zero after approval. Returns the problem in @Message. */
IF OBJECT_ID('dbo.usp_pms_cc_Movement_Validate', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_Movement_Validate AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_Movement_Validate
    @regid INT, @MovementId INT, @CheckBalances BIT, @Message NVARCHAR(400) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @Message = NULL;
    DECLARE @ProjectId INT, @Type NVARCHAR(30), @ChangeId INT;
    SELECT @ProjectId = ProjectId, @Type = MovementType, @ChangeId = ChangeId FROM dbo.Pms_tbl_BudgetMovement WHERE id = @MovementId AND regid = @regid;

    DECLARE @BudgetLines INT, @ReserveLines INT, @BudgetSum DECIMAL(18,2), @ReserveSum DECIMAL(18,2),
            @BudgetPos INT, @BudgetNeg INT, @ReservePos INT, @ReserveNeg INT, @Zero INT;
    SELECT @BudgetLines = SUM(CASE WHEN LineKind = N'Budget' THEN 1 ELSE 0 END),
           @ReserveLines = SUM(CASE WHEN LineKind = N'Reserve' THEN 1 ELSE 0 END),
           @BudgetSum = ISNULL(SUM(CASE WHEN LineKind = N'Budget' THEN Amount END), 0),
           @ReserveSum = ISNULL(SUM(CASE WHEN LineKind = N'Reserve' THEN Amount END), 0),
           @BudgetPos = SUM(CASE WHEN LineKind = N'Budget' AND Amount > 0 THEN 1 ELSE 0 END),
           @BudgetNeg = SUM(CASE WHEN LineKind = N'Budget' AND Amount < 0 THEN 1 ELSE 0 END),
           @ReservePos = SUM(CASE WHEN LineKind = N'Reserve' AND Amount > 0 THEN 1 ELSE 0 END),
           @ReserveNeg = SUM(CASE WHEN LineKind = N'Reserve' AND Amount < 0 THEN 1 ELSE 0 END),
           @Zero = SUM(CASE WHEN Amount = 0 THEN 1 ELSE 0 END)
      FROM dbo.Pms_tbl_BudgetMovementLine WHERE MovementId = @MovementId AND regid = @regid AND status = 1 AND archive = 0;
    SET @BudgetLines = ISNULL(@BudgetLines, 0); SET @ReserveLines = ISNULL(@ReserveLines, 0);

    IF @BudgetLines + @ReserveLines = 0 BEGIN SET @Message = N'Add at least one line.'; RETURN; END
    IF @Zero > 0 BEGIN SET @Message = N'Remove lines with a zero amount.'; RETURN; END

    IF @Type = N'Transfer' AND (@ReserveLines > 0 OR @BudgetLines < 2 OR @BudgetSum <> 0 OR @BudgetPos = 0 OR @BudgetNeg = 0)
        BEGIN SET @Message = N'A transfer moves budget between at least two budget lines and must net to zero (BV-3).'; RETURN; END
    IF @Type = N'ReserveAddition' AND (@BudgetLines > 0 OR @ReserveNeg > 0)
        BEGIN SET @Message = N'A reserve addition has only positive reserve lines.'; RETURN; END
    IF @Type = N'ReserveRelease' AND (@ReserveLines = 0 OR @BudgetLines = 0 OR @ReservePos > 0 OR @BudgetNeg > 0 OR @BudgetSum + @ReserveSum <> 0)
        BEGIN SET @Message = N'A release takes budget out of a reserve (negative) into budget lines (positive); the total must be zero (RS-6).'; RETURN; END
    IF @Type = N'ReserveReturn' AND (@ReserveLines = 0 OR @BudgetLines = 0 OR @ReserveNeg > 0 OR @BudgetPos > 0 OR @BudgetSum + @ReserveSum <> 0)
        BEGIN SET @Message = N'A return moves budget from budget lines (negative) back into a reserve (positive); the total must be zero.'; RETURN; END
    IF @Type IN (N'Correction', N'ApprovedChange') AND @ReserveLines > 0
        BEGIN SET @Message = N'Corrections and approved changes have budget lines only.'; RETURN; END
    IF @Type = N'ApprovedChange' AND @ChangeId IS NULL
        BEGIN SET @Message = N'An approved change must come from the change register.'; RETURN; END

    -- Every budget line uses a posting code and a milestone of this project; every reserve line a reserve of this project.
    IF EXISTS (SELECT 1 FROM dbo.Pms_tbl_BudgetMovementLine l WHERE l.MovementId = @MovementId AND l.regid = @regid AND l.status = 1 AND l.archive = 0 AND l.LineKind = N'Budget'
               AND (dbo.ufn_pms_cc_IsPostingCode(@regid, l.CostCodeId) = 0
                    OR (l.MilestoneId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.Pms_tbl_AddMilestone ms WHERE ms.id = l.MilestoneId AND ms.ProjectId = @ProjectId AND ms.regid = @regid AND ms.status = 1 AND ms.archive = 0)))))
        BEGIN SET @Message = N'A budget line has an invalid cost code or milestone.'; RETURN; END
    IF EXISTS (SELECT 1 FROM dbo.Pms_tbl_BudgetMovementLine l WHERE l.MovementId = @MovementId AND l.regid = @regid AND l.status = 1 AND l.archive = 0 AND l.LineKind = N'Reserve'
               AND NOT EXISTS (SELECT 1 FROM dbo.Pms_tbl_ProjectReserve r WHERE r.id = l.ReserveId AND r.ProjectId = @ProjectId AND r.regid = @regid AND r.status = 1 AND r.archive = 0))
        BEGIN SET @Message = N'A reserve line has an invalid reserve.'; RETURN; END

    IF @CheckBalances = 1
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM dbo.Pms_tbl_ProjectBudgetVersion WHERE regid = @regid AND ProjectId = @ProjectId AND VersionStatus = N'Locked' AND status = 1 AND archive = 0)
            BEGIN SET @Message = N'The project has no approved budget yet.'; RETURN; END
        IF EXISTS (SELECT 1 FROM (SELECT l.CostCodeId, ISNULL(l.MilestoneId, 0) AS MilestoneId, SUM(l.Amount) AS Delta
                                    FROM dbo.Pms_tbl_BudgetMovementLine l WHERE l.MovementId = @MovementId AND l.regid = @regid AND l.status = 1 AND l.archive = 0 AND l.LineKind = N'Budget'
                                   GROUP BY l.CostCodeId, ISNULL(l.MilestoneId, 0)) d
                   LEFT JOIN dbo.tvf_pms_cc_RevisedBudget(@regid, @ProjectId) rb ON rb.CostCodeId = d.CostCodeId AND rb.MilestoneId = d.MilestoneId
                   WHERE ISNULL(rb.RB, 0) + d.Delta < 0)
            BEGIN SET @Message = N'This movement would take a budget line below zero.'; RETURN; END
        IF EXISTS (SELECT 1 FROM (SELECT l.ReserveId, SUM(l.Amount) AS Delta FROM dbo.Pms_tbl_BudgetMovementLine l
                                   WHERE l.MovementId = @MovementId AND l.regid = @regid AND l.status = 1 AND l.archive = 0 AND l.LineKind = N'Reserve' GROUP BY l.ReserveId) d
                   LEFT JOIN dbo.tvf_pms_cc_ReserveBalance(@regid, @ProjectId) rs ON rs.ReserveId = d.ReserveId
                   WHERE ISNULL(rs.Remaining, 0) + d.Delta < 0)
            BEGIN SET @Message = N'The release is larger than the reserve balance.'; RETURN; END
    END
END
GO

IF OBJECT_ID('dbo.usp_pms_cc_Movement_Save', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_Movement_Save AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_Movement_Save
    @regid INT, @id INT, @ProjectId INT, @MovementType NVARCHAR(30), @Reason NVARCHAR(1000),
    @RiskReference NVARCHAR(100) = NULL, @LinesJson NVARCHAR(MAX), @uid INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @IsValid BIT, @Message NVARCHAR(400);
    EXEC dbo.usp_pms_cc_ValidateHierarchy @regid, @ProjectId, NULL, NULL, @IsValid OUTPUT;
    IF @IsValid = 0 BEGIN SELECT 0 AS Result, N'Project was not found.' AS Message; RETURN; END
    IF @MovementType NOT IN (N'Transfer', N'ReserveAddition', N'ReserveRelease', N'ReserveReturn', N'Correction', N'ApprovedChange')
        BEGIN SELECT 0 AS Result, N'Select a valid movement type.' AS Message; RETURN; END
    SET @Reason = LTRIM(RTRIM(ISNULL(@Reason, N'')));
    IF @Reason = N'' BEGIN SELECT 0 AS Result, N'A reason is required.' AS Message; RETURN; END
    IF NOT EXISTS (SELECT 1 FROM dbo.Pms_tbl_ProjectBudgetVersion WHERE regid = @regid AND ProjectId = @ProjectId AND VersionStatus = N'Locked' AND status = 1 AND archive = 0)
        BEGIN SELECT 0 AS Result, N'Approve the project budget before recording budget movements.' AS Message; RETURN; END
    IF ISJSON(ISNULL(@LinesJson, N'')) = 0 BEGIN SELECT 0 AS Result, N'The movement lines are not valid.' AS Message; RETURN; END

    DECLARE @old NVARCHAR(MAX), @Action NVARCHAR(30) = N'Update', @ChangeId INT;
    SET XACT_ABORT ON;
    BEGIN TRANSACTION;
        IF ISNULL(@id, 0) = 0
        BEGIN
            DECLARE @No INT = (SELECT ISNULL(MAX(MovementNo), 0) + 1 FROM dbo.Pms_tbl_BudgetMovement WHERE regid = @regid AND ProjectId = @ProjectId);
            INSERT INTO dbo.Pms_tbl_BudgetMovement (regid, ProjectId, MovementNo, MovementType, MovementStatus, Reason, RiskReference, cby, cdate)
            VALUES (@regid, @ProjectId, @No, @MovementType, N'Draft', @Reason, NULLIF(LTRIM(RTRIM(@RiskReference)), N''), @uid, GETDATE());
            SET @id = CAST(SCOPE_IDENTITY() AS INT); SET @Action = N'Create';
        END
        ELSE
        BEGIN
            SELECT @ChangeId = ChangeId FROM dbo.Pms_tbl_BudgetMovement WHERE id = @id AND regid = @regid AND ProjectId = @ProjectId AND MovementStatus = N'Draft' AND status = 1 AND archive = 0;
            IF @@ROWCOUNT = 0 BEGIN ROLLBACK TRANSACTION; SELECT 0 AS Result, N'Only a draft movement can be changed.' AS Message; RETURN; END
            IF @ChangeId IS NOT NULL AND @MovementType <> (SELECT MovementType FROM dbo.Pms_tbl_BudgetMovement WHERE id = @id)
                BEGIN ROLLBACK TRANSACTION; SELECT 0 AS Result, N'The type of a movement created from a change cannot change.' AS Message; RETURN; END
            SET @old = (SELECT m.MovementType, m.Reason, (SELECT LineKind, CostCodeId, MilestoneId, ReserveId, Amount FROM dbo.Pms_tbl_BudgetMovementLine
                          WHERE MovementId = @id AND status = 1 AND archive = 0 FOR JSON PATH) AS Lines FROM dbo.Pms_tbl_BudgetMovement m WHERE m.id = @id FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
            UPDATE dbo.Pms_tbl_BudgetMovement SET MovementType = @MovementType, Reason = @Reason, RiskReference = NULLIF(LTRIM(RTRIM(@RiskReference)), N''), RejectReason = NULL
             WHERE id = @id;
            UPDATE dbo.Pms_tbl_BudgetMovementLine SET status = 0, archive = 1 WHERE MovementId = @id AND status = 1 AND archive = 0;
        END

        INSERT INTO dbo.Pms_tbl_BudgetMovementLine (regid, MovementId, LineKind, CostCodeId, MilestoneId, ReserveId, Amount, Remarks)
        SELECT @regid, @id, j.LineKind,
               CASE WHEN j.LineKind = N'Budget' THEN j.CostCodeId END,
               CASE WHEN j.LineKind = N'Budget' THEN NULLIF(j.MilestoneId, 0) END,
               CASE WHEN j.LineKind = N'Reserve' THEN j.ReserveId END,
               ROUND(j.Amount, 2), NULLIF(LTRIM(RTRIM(j.Remarks)), N'')
          FROM OPENJSON(@LinesJson) WITH (LineKind NVARCHAR(10) '$.LineKind', CostCodeId INT '$.CostCodeId', MilestoneId INT '$.MilestoneId',
                                          ReserveId INT '$.ReserveId', Amount DECIMAL(18,2) '$.Amount', Remarks NVARCHAR(500) '$.Remarks') j
         WHERE j.LineKind IN (N'Budget', N'Reserve');

        EXEC dbo.usp_pms_cc_Movement_Validate @regid, @id, 0, @Message OUTPUT;
        IF @Message IS NOT NULL BEGIN ROLLBACK TRANSACTION; SELECT 0 AS Result, @Message AS Message; RETURN; END

        DECLARE @new NVARCHAR(MAX) = (SELECT m.MovementType, m.Reason, (SELECT LineKind, CostCodeId, MilestoneId, ReserveId, Amount FROM dbo.Pms_tbl_BudgetMovementLine
                                       WHERE MovementId = @id AND status = 1 AND archive = 0 FOR JSON PATH) AS Lines FROM dbo.Pms_tbl_BudgetMovement m WHERE m.id = @id FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
        EXEC dbo.usp_pms_cc_Audit_Add @regid, @ProjectId, N'Movement', @id, @Action, @Reason, @old, @new, @uid;
    COMMIT TRANSACTION;
    SELECT 1 AS Result, N'Movement saved as draft.' AS Message, @id AS id;
END
GO

IF OBJECT_ID('dbo.usp_pms_cc_Movement_SetStatus', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_Movement_SetStatus AS RETURN 0');
GO
/* @Action: Submit (Enter right), Approve / Reject (approval right, not the submitter), Discard (draft only). */
ALTER PROCEDURE dbo.usp_pms_cc_Movement_SetStatus
    @regid INT, @MovementId INT, @Action NVARCHAR(20), @Reason NVARCHAR(1000) = NULL, @uid INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ProjectId INT, @Status NVARCHAR(20), @SubmittedBy INT, @ChangeId INT, @Message NVARCHAR(400);
    SELECT @ProjectId = ProjectId, @Status = MovementStatus, @SubmittedBy = SubmittedBy, @ChangeId = ChangeId
      FROM dbo.Pms_tbl_BudgetMovement WHERE id = @MovementId AND regid = @regid AND status = 1 AND archive = 0;
    IF @ProjectId IS NULL BEGIN SELECT 0 AS Result, N'Movement was not found.' AS Message; RETURN; END
    SET @Reason = NULLIF(LTRIM(RTRIM(@Reason)), N'');

    IF @Action = N'Submit'
    BEGIN
        IF @Status <> N'Draft' BEGIN SELECT 0 AS Result, N'Only a draft movement can be submitted.' AS Message; RETURN; END
        EXEC dbo.usp_pms_cc_Movement_Validate @regid, @MovementId, 0, @Message OUTPUT;
        IF @Message IS NOT NULL BEGIN SELECT 0 AS Result, @Message AS Message; RETURN; END
        UPDATE dbo.Pms_tbl_BudgetMovement SET MovementStatus = N'Submitted', SubmittedBy = @uid, SubmittedOn = GETDATE(), RejectReason = NULL WHERE id = @MovementId;
    END
    ELSE IF @Action IN (N'Approve', N'Reject')
    BEGIN
        IF dbo.ufn_pms_cc_CanApprove(@regid, @uid) = 0 BEGIN SELECT 0 AS Result, N'You do not have Cost Control approval rights.' AS Message; RETURN; END
        IF @Status <> N'Submitted' BEGIN SELECT 0 AS Result, N'Only a submitted movement can be approved or returned.' AS Message; RETURN; END
        IF @SubmittedBy = @uid BEGIN SELECT 0 AS Result, N'You submitted this movement, so another approver must decide it.' AS Message; RETURN; END
        IF @Action = N'Reject'
        BEGIN
            IF @Reason IS NULL BEGIN SELECT 0 AS Result, N'A reason is required to return the movement.' AS Message; RETURN; END
            UPDATE dbo.Pms_tbl_BudgetMovement SET MovementStatus = N'Draft', RejectReason = @Reason, SubmittedBy = NULL, SubmittedOn = NULL WHERE id = @MovementId;
        END
        ELSE
        BEGIN
            IF EXISTS (SELECT 1 FROM dbo.Pms_tbl_ProjectBudgetVersion WHERE regid = @regid AND ProjectId = @ProjectId AND VersionStatus IN (N'Draft', N'Submitted') AND status = 1 AND archive = 0)
                BEGIN SELECT 0 AS Result, N'A budget revision is open. Approve or discard it before approving movements.' AS Message; RETURN; END
            EXEC dbo.usp_pms_cc_Movement_Validate @regid, @MovementId, 1, @Message OUTPUT;
            IF @Message IS NOT NULL BEGIN SELECT 0 AS Result, @Message AS Message; RETURN; END
            SET XACT_ABORT ON;
            BEGIN TRANSACTION;
                UPDATE dbo.Pms_tbl_BudgetMovement SET MovementStatus = N'Approved', ApprovedBy = @uid, ApprovedOn = GETDATE() WHERE id = @MovementId;
                -- An approved change is now budget-posted (CM-2). Its cost not yet in ETC moves into ETC, so EACp does not jump (F-3).
                IF @ChangeId IS NOT NULL
                BEGIN
                    DECLARE @Remainder DECIMAL(18,2), @AllocTotal DECIMAL(18,2);
                    SELECT @Remainder = CASE WHEN EstimatedCost - AmountInEtc > 0 THEN EstimatedCost - AmountInEtc ELSE 0 END
                      FROM dbo.Pms_tbl_ChangeRecord WHERE id = @ChangeId;
                    SELECT @AllocTotal = SUM(Amount) FROM dbo.Pms_tbl_ChangeAllocation WHERE ChangeId = @ChangeId AND status = 1 AND archive = 0;
                    IF @Remainder > 0 AND ISNULL(@AllocTotal, 0) > 0
                    BEGIN
                        ;WITH Share AS
                        (
                            SELECT a.CostCodeId, a.MilestoneId, ROUND(@Remainder * a.Amount / @AllocTotal, 2) AS Amount
                              FROM dbo.Pms_tbl_ChangeAllocation a WHERE a.ChangeId = @ChangeId AND a.status = 1 AND a.archive = 0
                        )
                        MERGE dbo.Pms_tbl_ProjectEtc AS t
                        USING (SELECT CostCodeId, MilestoneId, SUM(Amount) AS Amount FROM Share GROUP BY CostCodeId, MilestoneId) AS s
                           ON t.regid = @regid AND t.ProjectId = @ProjectId AND t.CostCodeId = s.CostCodeId
                          AND ISNULL(t.MilestoneId, 0) = ISNULL(s.MilestoneId, 0) AND t.status = 1 AND t.archive = 0
                        WHEN MATCHED THEN UPDATE SET EtcAmount = t.EtcAmount + s.Amount, AsOfDate = CAST(GETDATE() AS DATE), mby = @uid, mdate = GETDATE()
                        WHEN NOT MATCHED THEN INSERT (regid, ProjectId, CostCodeId, MilestoneId, EtcAmount, AsOfDate, Remarks, cby, cdate)
                             VALUES (@regid, @ProjectId, s.CostCodeId, s.MilestoneId, s.Amount, CAST(GETDATE() AS DATE), N'From approved change', @uid, GETDATE());
                        EXEC dbo.usp_pms_cc_Audit_Add @regid, @ProjectId, N'Etc', NULL, N'Update', N'Cost of approved change moved into ETC (F-3)', NULL, NULL, @uid;
                    END
                    UPDATE dbo.Pms_tbl_ChangeRecord SET BudgetPostingStatus = N'Posted', PostedOn = GETDATE(), AmountInEtc = EstimatedCost, mby = @uid, mdate = GETDATE()
                     WHERE id = @ChangeId;
                    EXEC dbo.usp_pms_cc_Audit_Add @regid, @ProjectId, N'Change', @ChangeId, N'BudgetPosted', NULL, NULL, NULL, @uid;
                END
            COMMIT TRANSACTION;
        END
    END
    ELSE IF @Action = N'Discard'
    BEGIN
        IF @Status <> N'Draft' BEGIN SELECT 0 AS Result, N'Only a draft movement can be discarded.' AS Message; RETURN; END
        UPDATE dbo.Pms_tbl_BudgetMovement SET status = 0, archive = 1 WHERE id = @MovementId;
    END
    ELSE BEGIN SELECT 0 AS Result, N'Unknown action.' AS Message; RETURN; END

    EXEC dbo.usp_pms_cc_Audit_Add @regid, @ProjectId, N'Movement', @MovementId, @Action, @Reason, NULL, NULL, @uid;
    SELECT 1 AS Result, CASE @Action WHEN N'Submit' THEN N'Movement submitted for approval.' WHEN N'Approve' THEN N'Movement approved.'
                                     WHEN N'Reject' THEN N'Movement returned to draft.' ELSE N'Draft movement discarded.' END AS Message;
END
GO

/* =====================================================================
   6. CHANGE REGISTER AND AT-RISK WORK
   ===================================================================== */
IF OBJECT_ID('dbo.usp_pms_cc_Change_List', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_Change_List AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_Change_List @regid INT, @ProjectId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT c.id, c.ChangeNo, c.Title, ISNULL(c.Description, N'') AS Description, c.ChangeClass, c.RecordStatus,
           c.InternalStatus, c.ClientStatus, c.SubcontractStatus, c.BudgetPostingStatus,
           c.EstimatedCost, c.AmountInEtc, CAST(c.IsProbable AS INT) AS IsProbable,
           c.ClientValueProposed, c.ClientValueApproved, c.SubcontractValue,
           CAST(c.IsAtRisk AS INT) AS IsAtRisk, c.AtRiskAuthorisedOn, ISNULL(ea.DisplayName, N'') AS AtRiskAuthorisedByName,
           c.InternalDecisionOn, ISNULL(ei.DisplayName, N'') AS InternalDecisionByName,
           CONVERT(VARCHAR(10), c.ClientDecisionOn, 23) AS ClientDecisionOn, CONVERT(VARCHAR(10), c.SubcontractDecisionOn, 23) AS SubcontractDecisionOn,
           c.PostedOn, c.cby, ISNULL(er.DisplayName, N'') AS RaisedByName, c.cdate AS RaisedOn,
           ISNULL((SELECT SUM(a.Amount) FROM dbo.vw_pms_cc_CostAllocation a WHERE a.ChangeId = c.id AND a.regid = c.regid), 0) AS CostIncurred,
           (SELECT TOP 1 m.id FROM dbo.Pms_tbl_BudgetMovement m WHERE m.ChangeId = c.id AND m.status = 1 AND m.archive = 0 ORDER BY m.id DESC) AS MovementId,
           (SELECT TOP 1 m.MovementStatus FROM dbo.Pms_tbl_BudgetMovement m WHERE m.ChangeId = c.id AND m.status = 1 AND m.archive = 0 ORDER BY m.id DESC) AS MovementStatus,
           -- Forecast margin and unrecovered exposure (CM-6). Pending revenue is never treated as approved.
           ISNULL(c.ClientValueApproved, 0) - c.EstimatedCost AS ApprovedMargin
      FROM dbo.Pms_tbl_ChangeRecord c
      LEFT JOIN dbo.tbl_employee ea ON ea.id = c.AtRiskAuthorisedBy
      LEFT JOIN dbo.tbl_employee ei ON ei.id = c.InternalDecisionBy
      LEFT JOIN dbo.tbl_employee er ON er.id = c.cby
     WHERE c.regid = @regid AND c.ProjectId = @ProjectId AND c.status = 1 AND c.archive = 0
     ORDER BY c.ChangeNo DESC;

    SELECT a.id, a.ChangeId, a.CostCodeId, a.MilestoneId, a.Amount, ISNULL(cc.Code + N' - ' + cc.Name, N'') AS CostCodeLabel, ISNULL(ms.milestone, N'') AS MilestoneName
      FROM dbo.Pms_tbl_ChangeAllocation a
      INNER JOIN dbo.Pms_tbl_ChangeRecord c ON c.id = a.ChangeId AND c.regid = a.regid
      LEFT JOIN dbo.Pms_tbl_CostCode cc ON cc.id = a.CostCodeId
      LEFT JOIN dbo.Pms_tbl_AddMilestone ms ON ms.id = a.MilestoneId
     WHERE c.regid = @regid AND c.ProjectId = @ProjectId AND c.status = 1 AND c.archive = 0 AND a.status = 1 AND a.archive = 0
     ORDER BY a.ChangeId, a.id;
END
GO

IF OBJECT_ID('dbo.usp_pms_cc_Change_Save', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_Change_Save AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_Change_Save
    @regid INT, @id INT, @ProjectId INT, @Title NVARCHAR(200), @Description NVARCHAR(2000) = NULL, @ChangeClass NVARCHAR(30),
    @EstimatedCost DECIMAL(18,2), @AmountInEtc DECIMAL(18,2) = 0, @IsProbable BIT = 0,
    @ClientValueProposed DECIMAL(18,2) = NULL, @SubcontractValue DECIMAL(18,2) = NULL, @IsAtRisk BIT = 0,
    @AllocationsJson NVARCHAR(MAX) = NULL, @uid INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @IsValid BIT;
    EXEC dbo.usp_pms_cc_ValidateHierarchy @regid, @ProjectId, NULL, NULL, @IsValid OUTPUT;
    IF @IsValid = 0 BEGIN SELECT 0 AS Result, N'Project was not found.' AS Message; RETURN; END
    SET @Title = LTRIM(RTRIM(ISNULL(@Title, N'')));
    IF @Title = N'' BEGIN SELECT 0 AS Result, N'Enter a title.' AS Message; RETURN; END
    IF @ChangeClass NOT IN (N'BaselineCorrection', N'InternalBudgetChange', N'ClientVariation', N'BudgetTransfer', N'SubcontractVariation', N'ContingencyRelease')
        BEGIN SELECT 0 AS Result, N'Select a valid change class.' AS Message; RETURN; END
    IF @EstimatedCost IS NULL BEGIN SELECT 0 AS Result, N'Enter the estimated cost (0 if none).' AS Message; RETURN; END
    SET @AmountInEtc = ISNULL(@AmountInEtc, 0);
    IF @AmountInEtc < 0 OR @AmountInEtc > ABS(@EstimatedCost)
        BEGIN SELECT 0 AS Result, N'The amount already in ETC must be between 0 and the estimated cost.' AS Message; RETURN; END
    IF @AllocationsJson IS NOT NULL AND ISJSON(@AllocationsJson) = 0 BEGIN SELECT 0 AS Result, N'The allocations are not valid.' AS Message; RETURN; END

    DECLARE @Alloc TABLE (CostCodeId INT, MilestoneId INT, Amount DECIMAL(18,2));
    INSERT INTO @Alloc SELECT j.CostCodeId, NULLIF(j.MilestoneId, 0), ROUND(j.Amount, 2)
      FROM OPENJSON(ISNULL(@AllocationsJson, N'[]')) WITH (CostCodeId INT '$.CostCodeId', MilestoneId INT '$.MilestoneId', Amount DECIMAL(18,2) '$.Amount') j;
    IF EXISTS (SELECT 1 FROM @Alloc a WHERE a.Amount IS NULL OR a.Amount = 0 OR dbo.ufn_pms_cc_IsPostingCode(@regid, a.CostCodeId) = 0
               OR (a.MilestoneId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.Pms_tbl_AddMilestone m WHERE m.id = a.MilestoneId AND m.ProjectId = @ProjectId AND m.regid = @regid AND m.status = 1 AND m.archive = 0)))
        BEGIN SELECT 0 AS Result, N'Each cost allocation needs an Element cost code, a valid milestone and a non-zero amount.' AS Message; RETURN; END

    DECLARE @old NVARCHAR(MAX), @new NVARCHAR(MAX), @Action NVARCHAR(30) = N'Update';
    SET XACT_ABORT ON;
    BEGIN TRANSACTION;
        IF ISNULL(@id, 0) = 0
        BEGIN
            DECLARE @No INT = (SELECT ISNULL(MAX(ChangeNo), 0) + 1 FROM dbo.Pms_tbl_ChangeRecord WHERE regid = @regid AND ProjectId = @ProjectId);
            INSERT INTO dbo.Pms_tbl_ChangeRecord (regid, ProjectId, ChangeNo, Title, Description, ChangeClass, RecordStatus, InternalStatus,
                        ClientStatus, SubcontractStatus, BudgetPostingStatus, EstimatedCost, AmountInEtc, IsProbable,
                        ClientValueProposed, SubcontractValue, IsAtRisk, cby, cdate)
            VALUES (@regid, @ProjectId, @No, @Title, NULLIF(LTRIM(RTRIM(@Description)), N''), @ChangeClass, N'Open', N'Pending',
                    CASE WHEN @ChangeClass = N'ClientVariation' THEN N'Pending' ELSE N'NotApplicable' END,
                    CASE WHEN @ChangeClass = N'SubcontractVariation' THEN N'Pending' ELSE N'NotApplicable' END,
                    N'NotPosted', @EstimatedCost, @AmountInEtc, ISNULL(@IsProbable, 0), @ClientValueProposed, @SubcontractValue, ISNULL(@IsAtRisk, 0), @uid, GETDATE());
            SET @id = CAST(SCOPE_IDENTITY() AS INT); SET @Action = N'Create';
        END
        ELSE
        BEGIN
            IF NOT EXISTS (SELECT 1 FROM dbo.Pms_tbl_ChangeRecord WHERE id = @id AND regid = @regid AND ProjectId = @ProjectId AND RecordStatus = N'Open'
                           AND BudgetPostingStatus = N'NotPosted' AND status = 1 AND archive = 0)
                BEGIN ROLLBACK TRANSACTION; SELECT 0 AS Result, N'Only an open change that is not yet budget-posted can be edited.' AS Message; RETURN; END
            IF EXISTS (SELECT 1 FROM dbo.Pms_tbl_BudgetMovement WHERE ChangeId = @id AND MovementStatus IN (N'Draft', N'Submitted') AND status = 1 AND archive = 0)
                BEGIN ROLLBACK TRANSACTION; SELECT 0 AS Result, N'A budget posting for this change is in progress. Discard it first.' AS Message; RETURN; END
            -- Removing the at-risk flag is not allowed once at-risk cost is booked against the change.
            IF ISNULL(@IsAtRisk, 0) = 0 AND EXISTS (SELECT 1 FROM dbo.Pms_tbl_ProjectCostEntry WHERE ChangeId = @id AND status = 1 AND archive = 0)
               AND EXISTS (SELECT 1 FROM dbo.Pms_tbl_ChangeRecord WHERE id = @id AND IsAtRisk = 1)
                BEGIN ROLLBACK TRANSACTION; SELECT 0 AS Result, N'At-risk cost is already booked against this change; it stays at risk.' AS Message; RETURN; END
            SET @old = (SELECT Title, ChangeClass, EstimatedCost, AmountInEtc, IsProbable, ClientValueProposed, SubcontractValue, IsAtRisk FROM dbo.Pms_tbl_ChangeRecord WHERE id = @id FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
            UPDATE dbo.Pms_tbl_ChangeRecord
               SET Title = @Title, Description = NULLIF(LTRIM(RTRIM(@Description)), N''), ChangeClass = @ChangeClass, EstimatedCost = @EstimatedCost,
                   AmountInEtc = @AmountInEtc, IsProbable = ISNULL(@IsProbable, 0), ClientValueProposed = @ClientValueProposed,
                   SubcontractValue = @SubcontractValue, IsAtRisk = ISNULL(@IsAtRisk, 0),
                   AtRiskAuthorisedBy = CASE WHEN ISNULL(@IsAtRisk, 0) = 1 THEN AtRiskAuthorisedBy END,
                   AtRiskAuthorisedOn = CASE WHEN ISNULL(@IsAtRisk, 0) = 1 THEN AtRiskAuthorisedOn END,
                   mby = @uid, mdate = GETDATE()
             WHERE id = @id;
            UPDATE dbo.Pms_tbl_ChangeAllocation SET status = 0, archive = 1 WHERE ChangeId = @id AND status = 1 AND archive = 0;
        END
        INSERT INTO dbo.Pms_tbl_ChangeAllocation (regid, ChangeId, CostCodeId, MilestoneId, Amount)
        SELECT @regid, @id, CostCodeId, MilestoneId, Amount FROM @Alloc;
        SET @new = (SELECT Title, ChangeClass, EstimatedCost, AmountInEtc, IsProbable, ClientValueProposed, SubcontractValue, IsAtRisk,
                           (SELECT CostCodeId, MilestoneId, Amount FROM dbo.Pms_tbl_ChangeAllocation WHERE ChangeId = @id AND status = 1 AND archive = 0 FOR JSON PATH) AS Allocations
                      FROM dbo.Pms_tbl_ChangeRecord WHERE id = @id FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
        EXEC dbo.usp_pms_cc_Audit_Add @regid, @ProjectId, N'Change', @id, @Action, NULL, @old, @new, @uid;
    COMMIT TRANSACTION;
    SELECT 1 AS Result, N'Change saved.' AS Message, @id AS id;
END
GO

IF OBJECT_ID('dbo.usp_pms_cc_Change_SetStatus', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_Change_SetStatus AS RETURN 0');
GO
/* Sets ONE of the independent statuses (CM-2). No status ever sets another.
   @Field: Internal (Approved / Rejected / Pending; approval right, not the person who raised it)
           Client (Pending / Submitted / Approved / Rejected) with @DecisionDate and @Amount (approved client value)
           Subcontract (Pending / Approved / Rejected) with @DecisionDate
           AtRisk (Authorise; approval right, not the person who raised it)
           Record (Open / Withdrawn / Closed) */
ALTER PROCEDURE dbo.usp_pms_cc_Change_SetStatus
    @regid INT, @ChangeId INT, @Field NVARCHAR(20), @Value NVARCHAR(20), @Reason NVARCHAR(1000) = NULL,
    @DecisionDate DATE = NULL, @Amount DECIMAL(18,2) = NULL, @uid INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ProjectId INT, @RaisedBy INT, @Record NVARCHAR(20), @IsAtRisk BIT, @Posted NVARCHAR(20), @old NVARCHAR(MAX);
    SELECT @ProjectId = ProjectId, @RaisedBy = cby, @Record = RecordStatus, @IsAtRisk = IsAtRisk, @Posted = BudgetPostingStatus
      FROM dbo.Pms_tbl_ChangeRecord WHERE id = @ChangeId AND regid = @regid AND status = 1 AND archive = 0;
    IF @ProjectId IS NULL BEGIN SELECT 0 AS Result, N'Change was not found.' AS Message; RETURN; END
    SET @Reason = NULLIF(LTRIM(RTRIM(@Reason)), N'');
    IF @Value IN (N'Rejected', N'Withdrawn') AND @Reason IS NULL BEGIN SELECT 0 AS Result, N'A reason is required.' AS Message; RETURN; END
    IF @Record <> N'Open' AND @Field <> N'Record' BEGIN SELECT 0 AS Result, N'The change is not open.' AS Message; RETURN; END
    SET @old = (SELECT InternalStatus, ClientStatus, SubcontractStatus, RecordStatus, ClientValueApproved, AtRiskAuthorisedOn FROM dbo.Pms_tbl_ChangeRecord WHERE id = @ChangeId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);

    IF @Field = N'Internal'
    BEGIN
        IF @Value NOT IN (N'Pending', N'Approved', N'Rejected') BEGIN SELECT 0 AS Result, N'Invalid internal status.' AS Message; RETURN; END
        IF dbo.ufn_pms_cc_CanApprove(@regid, @uid) = 0 BEGIN SELECT 0 AS Result, N'You do not have Cost Control approval rights.' AS Message; RETURN; END
        IF @RaisedBy = @uid BEGIN SELECT 0 AS Result, N'You raised this change, so another approver must authorise it.' AS Message; RETURN; END
        IF @Posted = N'Posted' BEGIN SELECT 0 AS Result, N'The change is already budget-posted.' AS Message; RETURN; END
        UPDATE dbo.Pms_tbl_ChangeRecord SET InternalStatus = @Value, InternalDecisionBy = @uid, InternalDecisionOn = GETDATE(), mby = @uid, mdate = GETDATE() WHERE id = @ChangeId;
    END
    ELSE IF @Field = N'Client'
    BEGIN
        IF @Value NOT IN (N'NotApplicable', N'Pending', N'Submitted', N'Approved', N'Rejected') BEGIN SELECT 0 AS Result, N'Invalid client status.' AS Message; RETURN; END
        IF @Value IN (N'Approved', N'Rejected') AND @DecisionDate IS NULL BEGIN SELECT 0 AS Result, N'Enter the client decision date.' AS Message; RETURN; END
        IF @Value = N'Approved' AND @Amount IS NULL BEGIN SELECT 0 AS Result, N'Enter the approved client value.' AS Message; RETURN; END
        UPDATE dbo.Pms_tbl_ChangeRecord SET ClientStatus = @Value, ClientDecisionOn = @DecisionDate,
               ClientValueApproved = CASE WHEN @Value = N'Approved' THEN @Amount ELSE NULL END, mby = @uid, mdate = GETDATE() WHERE id = @ChangeId;
    END
    ELSE IF @Field = N'Subcontract'
    BEGIN
        IF @Value NOT IN (N'NotApplicable', N'Pending', N'Approved', N'Rejected') BEGIN SELECT 0 AS Result, N'Invalid subcontract status.' AS Message; RETURN; END
        IF @Value IN (N'Approved', N'Rejected') AND @DecisionDate IS NULL BEGIN SELECT 0 AS Result, N'Enter the subcontract decision date.' AS Message; RETURN; END
        UPDATE dbo.Pms_tbl_ChangeRecord SET SubcontractStatus = @Value, SubcontractDecisionOn = @DecisionDate, mby = @uid, mdate = GETDATE() WHERE id = @ChangeId;
    END
    ELSE IF @Field = N'AtRisk'
    BEGIN
        IF @IsAtRisk = 0 BEGIN SELECT 0 AS Result, N'This change is not marked as at-risk work.' AS Message; RETURN; END
        IF dbo.ufn_pms_cc_CanApprove(@regid, @uid) = 0 BEGIN SELECT 0 AS Result, N'You do not have Cost Control approval rights.' AS Message; RETURN; END
        IF @RaisedBy = @uid BEGIN SELECT 0 AS Result, N'You raised this change, so another approver must authorise the at-risk work.' AS Message; RETURN; END
        UPDATE dbo.Pms_tbl_ChangeRecord SET AtRiskAuthorisedBy = @uid, AtRiskAuthorisedOn = GETDATE(), mby = @uid, mdate = GETDATE() WHERE id = @ChangeId AND AtRiskAuthorisedOn IS NULL;
        IF @@ROWCOUNT = 0 BEGIN SELECT 0 AS Result, N'The at-risk work is already authorised.' AS Message; RETURN; END
    END
    ELSE IF @Field = N'Record'
    BEGIN
        IF @Value NOT IN (N'Open', N'Withdrawn', N'Closed') BEGIN SELECT 0 AS Result, N'Invalid record status.' AS Message; RETURN; END
        IF @Value = N'Withdrawn' AND @Posted = N'Posted' BEGIN SELECT 0 AS Result, N'A budget-posted change cannot be withdrawn; record a correction instead.' AS Message; RETURN; END
        IF @Value <> N'Open' AND EXISTS (SELECT 1 FROM dbo.Pms_tbl_BudgetMovement WHERE ChangeId = @ChangeId AND MovementStatus IN (N'Draft', N'Submitted') AND status = 1 AND archive = 0)
            BEGIN SELECT 0 AS Result, N'A budget posting for this change is in progress.' AS Message; RETURN; END
        UPDATE dbo.Pms_tbl_ChangeRecord SET RecordStatus = @Value, mby = @uid, mdate = GETDATE() WHERE id = @ChangeId;
    END
    ELSE BEGIN SELECT 0 AS Result, N'Unknown status field.' AS Message; RETURN; END

    DECLARE @new NVARCHAR(MAX) = (SELECT InternalStatus, ClientStatus, SubcontractStatus, RecordStatus, ClientValueApproved, AtRiskAuthorisedOn FROM dbo.Pms_tbl_ChangeRecord WHERE id = @ChangeId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
    DECLARE @AuditAction NVARCHAR(30) = @Field + N':' + @Value;
    EXEC dbo.usp_pms_cc_Audit_Add @regid, @ProjectId, N'Change', @ChangeId, @AuditAction, @Reason, @old, @new, @uid;
    SELECT 1 AS Result, N'Status saved.' AS Message;
END
GO

IF OBJECT_ID('dbo.usp_pms_cc_Change_CreatePosting', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_Change_CreatePosting AS RETURN 0');
GO
/* Creates the draft budget movement that will post the change to the budget once approved.
   Requires the internal cost authorisation. Client approval alone is not enough (CM-2). */
ALTER PROCEDURE dbo.usp_pms_cc_Change_CreatePosting @regid INT, @ChangeId INT, @uid INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ProjectId INT, @Class NVARCHAR(30), @Internal NVARCHAR(20), @Posted NVARCHAR(20), @Record NVARCHAR(20), @Title NVARCHAR(200), @No INT;
    SELECT @ProjectId = ProjectId, @Class = ChangeClass, @Internal = InternalStatus, @Posted = BudgetPostingStatus, @Record = RecordStatus, @Title = Title, @No = ChangeNo
      FROM dbo.Pms_tbl_ChangeRecord WHERE id = @ChangeId AND regid = @regid AND status = 1 AND archive = 0;
    IF @ProjectId IS NULL BEGIN SELECT 0 AS Result, N'Change was not found.' AS Message; RETURN; END
    IF @Record <> N'Open' OR @Posted = N'Posted' BEGIN SELECT 0 AS Result, N'Only an open change that is not yet posted can be posted.' AS Message; RETURN; END
    IF @Internal <> N'Approved' BEGIN SELECT 0 AS Result, N'The internal cost authorisation must be approved first.' AS Message; RETURN; END
    IF EXISTS (SELECT 1 FROM dbo.Pms_tbl_BudgetMovement WHERE ChangeId = @ChangeId AND MovementStatus IN (N'Draft', N'Submitted') AND status = 1 AND archive = 0)
        BEGIN SELECT 0 AS Result, N'A budget posting for this change already exists.' AS Message; RETURN; END
    IF NOT EXISTS (SELECT 1 FROM dbo.Pms_tbl_ChangeAllocation WHERE ChangeId = @ChangeId AND status = 1 AND archive = 0)
        BEGIN SELECT 0 AS Result, N'Add cost allocations to the change first.' AS Message; RETURN; END
    IF NOT EXISTS (SELECT 1 FROM dbo.Pms_tbl_ProjectBudgetVersion WHERE regid = @regid AND ProjectId = @ProjectId AND VersionStatus = N'Locked' AND status = 1 AND archive = 0)
        BEGIN SELECT 0 AS Result, N'Approve the project budget first.' AS Message; RETURN; END

    DECLARE @Type NVARCHAR(30) = CASE @Class WHEN N'BaselineCorrection' THEN N'Correction' WHEN N'BudgetTransfer' THEN N'Transfer'
                                             WHEN N'ContingencyRelease' THEN N'ReserveRelease' ELSE N'ApprovedChange' END;
    DECLARE @MovementNo INT = (SELECT ISNULL(MAX(MovementNo), 0) + 1 FROM dbo.Pms_tbl_BudgetMovement WHERE regid = @regid AND ProjectId = @ProjectId), @MovementId INT;
    DECLARE @Reason NVARCHAR(1000) = N'Change #' + CAST(@No AS NVARCHAR(10)) + N': ' + @Title;
    SET XACT_ABORT ON;
    BEGIN TRANSACTION;
        INSERT INTO dbo.Pms_tbl_BudgetMovement (regid, ProjectId, MovementNo, MovementType, MovementStatus, Reason, ChangeId, cby, cdate)
        VALUES (@regid, @ProjectId, @MovementNo, @Type, N'Draft', @Reason, @ChangeId, @uid, GETDATE());
        SET @MovementId = CAST(SCOPE_IDENTITY() AS INT);
        INSERT INTO dbo.Pms_tbl_BudgetMovementLine (regid, MovementId, LineKind, CostCodeId, MilestoneId, Amount)
        SELECT @regid, @MovementId, N'Budget', CostCodeId, MilestoneId, Amount FROM dbo.Pms_tbl_ChangeAllocation WHERE ChangeId = @ChangeId AND status = 1 AND archive = 0;
        EXEC dbo.usp_pms_cc_Audit_Add @regid, @ProjectId, N'Movement', @MovementId, N'Create', @Reason, NULL, NULL, @uid;
    COMMIT TRANSACTION;
    SELECT 1 AS Result, CASE WHEN @Type IN (N'Transfer', N'ReserveRelease')
                             THEN N'Draft movement created. Add the offsetting lines (source budget lines or reserve) in Movements, then submit.'
                             ELSE N'Draft budget posting created in Movements. Submit it for approval.' END AS Message, @MovementId AS id;
END
GO

/* =====================================================================
   7. COST ENTRIES: future-date check, change / at-risk link, split allocation
   ===================================================================== */
ALTER PROCEDURE dbo.usp_pms_cc_CostEntry_List @regid INT, @ProjectId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT e.id, CONVERT(VARCHAR(10), e.EntryDate, 23) AS EntryDate,
           e.MilestoneId, ISNULL(m.milestone, N'') AS MilestoneName, e.TaskId, ISNULL(t.taskname, N'') AS TaskName,
           e.CostCodeId, ISNULL(c.Code, N'') AS CostCode, ISNULL(c.Name, N'') AS CostCodeName, ISNULL(c.CostType, N'') AS CostType,
           e.Amount, e.SourceType, ISNULL(e.SourceDocType, N'') AS SourceDocType, ISNULL(e.ReferenceNo, N'') AS ReferenceNo,
           ISNULL(e.SiteName, N'') AS SiteName, ISNULL(e.Remarks, N'') AS Remarks,
           CAST(dbo.ufn_pms_cc_IsPeriodClosed(e.regid, e.ProjectId, e.EntryDate) AS INT) AS IsPeriodClosed,
           e.ChangeId, ch.ChangeNo, CAST(ISNULL(ch.IsAtRisk, 0) AS INT) AS IsAtRisk,
           (SELECT COUNT(1) FROM dbo.Pms_tbl_CostEntrySplit s WHERE s.EntryId = e.id AND s.status = 1 AND s.archive = 0) AS SplitCount
    FROM dbo.Pms_tbl_ProjectCostEntry e WITH (NOLOCK)
    LEFT JOIN dbo.Pms_tbl_CostCode c WITH (NOLOCK) ON c.id = e.CostCodeId AND c.regid = e.regid
    LEFT JOIN dbo.Pms_tbl_AddMilestone m WITH (NOLOCK) ON m.id = e.MilestoneId AND m.regid = e.regid
    LEFT JOIN dbo.Pms_tbl_Addtask t WITH (NOLOCK) ON t.id = e.TaskId AND t.regid = e.regid
    LEFT JOIN dbo.Pms_tbl_ChangeRecord ch ON ch.id = e.ChangeId
    WHERE e.regid = @regid AND e.ProjectId = @ProjectId AND e.status = 1 AND e.archive = 0
    ORDER BY e.EntryDate DESC, e.id DESC;

    SELECT s.id, s.EntryId, s.CostCodeId, s.MilestoneId, s.TaskId, s.Amount, s.SplitPercent,
           ISNULL(c.Code + N' - ' + c.Name, N'') AS CostCodeLabel, ISNULL(m.milestone, N'') AS MilestoneName, ISNULL(t.taskname, N'') AS TaskName
      FROM dbo.Pms_tbl_CostEntrySplit s
      INNER JOIN dbo.Pms_tbl_ProjectCostEntry e ON e.id = s.EntryId AND e.regid = s.regid
      LEFT JOIN dbo.Pms_tbl_CostCode c ON c.id = s.CostCodeId
      LEFT JOIN dbo.Pms_tbl_AddMilestone m ON m.id = s.MilestoneId
      LEFT JOIN dbo.Pms_tbl_Addtask t ON t.id = s.TaskId
     WHERE e.regid = @regid AND e.ProjectId = @ProjectId AND e.status = 1 AND e.archive = 0 AND s.status = 1 AND s.archive = 0
     ORDER BY s.EntryId, s.id;
END
GO

ALTER PROCEDURE dbo.usp_pms_cc_CostEntry_Save
    @regid INT, @id INT, @ProjectId INT, @MilestoneId INT = NULL, @TaskId INT = NULL, @CostCodeId INT,
    @EntryDate DATE, @Amount DECIMAL(18,2), @ReferenceNo NVARCHAR(100) = NULL, @Remarks NVARCHAR(1000) = NULL, @uid INT,
    @SiteName NVARCHAR(100) = NULL, @SourceDocType NVARCHAR(30) = NULL, @Reason NVARCHAR(1000) = NULL, @ConfirmDuplicate BIT = 0,
    @ChangeId INT = NULL, @SplitsJson NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @IsValid BIT;
    IF @EntryDate IS NULL BEGIN SELECT 0 AS Result, N'Enter the cost date.' AS Message; RETURN; END
    IF @EntryDate > CAST(GETDATE() AS DATE) BEGIN SELECT 0 AS Result, N'The cost date cannot be in the future.' AS Message; RETURN; END
    IF @Amount IS NULL OR @Amount = 0 BEGIN SELECT 0 AS Result, N'Enter a non-zero amount (negative for a reversal).' AS Message; RETURN; END
    SET @SourceDocType = NULLIF(LTRIM(RTRIM(@SourceDocType)), N'');
    IF @SourceDocType IS NOT NULL AND @SourceDocType NOT IN (N'Bill / Invoice', N'Voucher', N'Purchase Order', N'GRN', N'Petty Cash', N'Timesheet', N'Other')
        BEGIN SELECT 0 AS Result, N'Select a valid source document type.' AS Message; RETURN; END
    IF dbo.ufn_pms_cc_IsPeriodClosed(@regid, @ProjectId, @EntryDate) = 1
        BEGIN SELECT 0 AS Result, N'The month of this date is closed. Use a date in an open month.' AS Message; RETURN; END

    -- Change / at-risk link (AR-01, AR-03): at-risk cost needs the at-risk authorisation first.
    IF @ChangeId IS NOT NULL
    BEGIN
        DECLARE @ChRecord NVARCHAR(20), @ChAtRisk BIT, @ChAuth DATETIME;
        SELECT @ChRecord = RecordStatus, @ChAtRisk = IsAtRisk, @ChAuth = AtRiskAuthorisedOn FROM dbo.Pms_tbl_ChangeRecord
         WHERE id = @ChangeId AND regid = @regid AND ProjectId = @ProjectId AND status = 1 AND archive = 0;
        IF @ChRecord IS NULL OR @ChRecord <> N'Open' BEGIN SELECT 0 AS Result, N'Select an open change of this project.' AS Message; RETURN; END
        IF @ChAtRisk = 1 AND @ChAuth IS NULL BEGIN SELECT 0 AS Result, N'The at-risk work of this change is not authorised yet.' AS Message; RETURN; END
    END

    -- Split allocation (DM-03): at least two rows totalling exactly the entry amount.
    DECLARE @Splits TABLE (CostCodeId INT, MilestoneId INT, TaskId INT, Amount DECIMAL(18,2));
    DECLARE @IsSplit BIT = 0;
    IF @SplitsJson IS NOT NULL AND LTRIM(RTRIM(@SplitsJson)) NOT IN (N'', N'[]')
    BEGIN
        IF ISJSON(@SplitsJson) = 0 BEGIN SELECT 0 AS Result, N'The split rows are not valid.' AS Message; RETURN; END
        INSERT INTO @Splits SELECT j.CostCodeId, NULLIF(j.MilestoneId, 0), NULLIF(j.TaskId, 0), ROUND(j.Amount, 2)
          FROM OPENJSON(@SplitsJson) WITH (CostCodeId INT '$.CostCodeId', MilestoneId INT '$.MilestoneId', TaskId INT '$.TaskId', Amount DECIMAL(18,2) '$.Amount') j;
        IF (SELECT COUNT(1) FROM @Splits) < 2 BEGIN SELECT 0 AS Result, N'A split needs at least two rows.' AS Message; RETURN; END
        IF (SELECT SUM(Amount) FROM @Splits) <> @Amount BEGIN SELECT 0 AS Result, N'The split rows must total exactly the entry amount (100 %).' AS Message; RETURN; END
        IF EXISTS (SELECT 1 FROM @Splits WHERE Amount IS NULL OR Amount = 0 OR SIGN(Amount) <> SIGN(@Amount))
            BEGIN SELECT 0 AS Result, N'Each split row needs a non-zero amount with the same sign as the entry.' AS Message; RETURN; END
        IF EXISTS (SELECT 1 FROM @Splits s WHERE dbo.ufn_pms_cc_IsPostingCode(@regid, s.CostCodeId) = 0)
            BEGIN SELECT 0 AS Result, N'Each split row needs an Element cost code.' AS Message; RETURN; END
        IF EXISTS (SELECT 1 FROM @Splits s WHERE
                   (s.MilestoneId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.Pms_tbl_AddMilestone m WHERE m.id = s.MilestoneId AND m.ProjectId = @ProjectId AND m.regid = @regid AND m.status = 1 AND m.archive = 0))
                OR (s.TaskId IS NOT NULL AND (s.MilestoneId IS NULL OR NOT EXISTS (SELECT 1 FROM dbo.Pms_tbl_Addtask t WHERE t.id = s.TaskId AND t.Milestoneid = s.MilestoneId AND t.ProjectId = @ProjectId AND t.regid = @regid AND t.status = 1 AND t.archive = 0))))
            BEGIN SELECT 0 AS Result, N'A split row has a milestone or task that does not belong to this project.' AS Message; RETURN; END
        SET @IsSplit = 1;
        -- The entry header keeps the largest split's cost code for display; reports use the split rows.
        SELECT TOP 1 @CostCodeId = CostCodeId FROM @Splits ORDER BY ABS(Amount) DESC;
        SET @MilestoneId = NULL; SET @TaskId = NULL;
    END
    ELSE
    BEGIN
        EXEC dbo.usp_pms_cc_ValidateHierarchy @regid, @ProjectId, @MilestoneId, @TaskId, @IsValid OUTPUT;
        IF @IsValid = 0 BEGIN SELECT 0 AS Result, N'Project, milestone or task is not valid.' AS Message; RETURN; END
        IF dbo.ufn_pms_cc_IsPostingCode(@regid, @CostCodeId) = 0 BEGIN SELECT 0 AS Result, N'Select an active Element-level cost code.' AS Message; RETURN; END
    END

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

    SET XACT_ABORT ON;
    BEGIN TRANSACTION;
        IF ISNULL(@id, 0) = 0
        BEGIN
            INSERT INTO dbo.Pms_tbl_ProjectCostEntry (regid, ProjectId, MilestoneId, TaskId, CostCodeId, EntryDate, Amount, SourceType, SourceDocType, ReferenceNo, SiteName, Remarks, ChangeId, cby, cdate)
            VALUES (@regid, @ProjectId, @MilestoneId, @TaskId, @CostCodeId, @EntryDate, @Amount, N'Manual', @SourceDocType,
                    @ReferenceNo, NULLIF(LTRIM(RTRIM(@SiteName)), N''), NULLIF(LTRIM(RTRIM(@Remarks)), N''), @ChangeId, @uid, GETDATE());
            SET @id = CAST(SCOPE_IDENTITY() AS INT);
        END
        ELSE
        BEGIN
            SET @old = (SELECT EntryDate, MilestoneId, TaskId, CostCodeId, Amount, SourceDocType, ReferenceNo, SiteName, Remarks, ChangeId,
                               (SELECT CostCodeId, MilestoneId, TaskId, Amount FROM dbo.Pms_tbl_CostEntrySplit WHERE EntryId = @id AND status = 1 AND archive = 0 FOR JSON PATH) AS Splits
                          FROM dbo.Pms_tbl_ProjectCostEntry WHERE id = @id FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
            UPDATE dbo.Pms_tbl_ProjectCostEntry
               SET MilestoneId = @MilestoneId, TaskId = @TaskId, CostCodeId = @CostCodeId, EntryDate = @EntryDate, Amount = @Amount,
                   SourceDocType = @SourceDocType, ReferenceNo = @ReferenceNo, SiteName = NULLIF(LTRIM(RTRIM(@SiteName)), N''),
                   Remarks = NULLIF(LTRIM(RTRIM(@Remarks)), N''), ChangeId = @ChangeId, mby = @uid, mdate = GETDATE()
             WHERE id = @id AND regid = @regid;
            UPDATE dbo.Pms_tbl_CostEntrySplit SET status = 0, archive = 1 WHERE EntryId = @id AND status = 1 AND archive = 0;
        END
        IF @IsSplit = 1
            INSERT INTO dbo.Pms_tbl_CostEntrySplit (regid, EntryId, CostCodeId, MilestoneId, TaskId, Amount, SplitPercent, cby, cdate)
            SELECT @regid, @id, CostCodeId, MilestoneId, TaskId, Amount, ROUND(Amount * 100.0 / @Amount, 4), @uid, GETDATE() FROM @Splits;
        SET @new = (SELECT EntryDate, MilestoneId, TaskId, CostCodeId, Amount, SourceDocType, ReferenceNo, SiteName, Remarks, ChangeId,
                           (SELECT CostCodeId, MilestoneId, TaskId, Amount FROM dbo.Pms_tbl_CostEntrySplit WHERE EntryId = @id AND status = 1 AND archive = 0 FOR JSON PATH) AS Splits
                      FROM dbo.Pms_tbl_ProjectCostEntry WHERE id = @id FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
        DECLARE @AuditAction NVARCHAR(30) = CASE WHEN @old IS NULL THEN N'Create' ELSE N'Update' END;
        EXEC dbo.usp_pms_cc_Audit_Add @regid, @ProjectId, N'CostEntry', @id, @AuditAction, @Reason, @old, @new, @uid;
    COMMIT TRANSACTION;
    SELECT 1 AS Result, N'Saved.' AS Message, @id AS id;
END
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
    SET @old = (SELECT EntryDate, MilestoneId, TaskId, CostCodeId, Amount, SourceDocType, ReferenceNo, SiteName, Remarks, ChangeId,
                       (SELECT CostCodeId, MilestoneId, TaskId, Amount FROM dbo.Pms_tbl_CostEntrySplit WHERE EntryId = @id AND status = 1 AND archive = 0 FOR JSON PATH) AS Splits
                  FROM dbo.Pms_tbl_ProjectCostEntry WHERE id = @id FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
    SET XACT_ABORT ON;
    BEGIN TRANSACTION;
        UPDATE dbo.Pms_tbl_ProjectCostEntry SET status = 0, archive = 1, mby = @uid, mdate = GETDATE() WHERE id = @id AND regid = @regid;
        UPDATE dbo.Pms_tbl_CostEntrySplit SET status = 0, archive = 1 WHERE EntryId = @id AND status = 1 AND archive = 0;
        EXEC dbo.usp_pms_cc_Audit_Add @regid, @ProjectId, N'CostEntry', @id, N'Delete', @Reason, @old, NULL, @uid;
    COMMIT TRANSACTION;
    SELECT 1 AS Result, N'Deleted.' AS Message;
END
GO

/* =====================================================================
   8. REPORTS: RB, allocations, ETC, measures; snapshots
   ===================================================================== */
ALTER PROCEDURE dbo.usp_pms_cc_Etc_List @regid INT, @ProjectId INT
AS
BEGIN
    SET NOCOUNT ON;
    ;WITH Keys AS
    (
        SELECT CostCodeId, MilestoneId FROM dbo.tvf_pms_cc_RevisedBudget(@regid, @ProjectId)
        UNION SELECT CostCodeId, ISNULL(MilestoneId, 0) FROM dbo.vw_pms_cc_CostAllocation WHERE regid = @regid AND ProjectId = @ProjectId
        UNION SELECT CostCodeId, ISNULL(MilestoneId, 0) FROM dbo.Pms_tbl_ProjectEtc WHERE regid = @regid AND ProjectId = @ProjectId AND status = 1 AND archive = 0
    )
    SELECT k.CostCodeId, NULLIF(k.MilestoneId, 0) AS MilestoneId, ISNULL(c.Code, N'') AS CostCode, ISNULL(c.Name, N'') AS CostCodeName,
           ISNULL(m.milestone, N'') AS MilestoneName,
           ISNULL(rb.RB, 0) AS CurrentBudget,
           ISNULL((SELECT SUM(a.Amount) FROM dbo.vw_pms_cc_CostAllocation a WHERE a.regid = @regid AND a.ProjectId = @ProjectId
                    AND a.CostCodeId = k.CostCodeId AND ISNULL(a.MilestoneId, 0) = k.MilestoneId), 0) AS Actual,
           x.id AS EtcId, x.EtcAmount, CONVERT(VARCHAR(10), x.AsOfDate, 23) AS AsOfDate, ISNULL(x.Remarks, N'') AS Remarks
      FROM Keys k
      LEFT JOIN dbo.tvf_pms_cc_RevisedBudget(@regid, @ProjectId) rb ON rb.CostCodeId = k.CostCodeId AND rb.MilestoneId = k.MilestoneId
      LEFT JOIN dbo.Pms_tbl_CostCode c ON c.id = k.CostCodeId AND c.regid = @regid
      LEFT JOIN dbo.Pms_tbl_AddMilestone m ON m.id = k.MilestoneId AND m.regid = @regid
      LEFT JOIN dbo.Pms_tbl_ProjectEtc x ON x.regid = @regid AND x.ProjectId = @ProjectId AND x.CostCodeId = k.CostCodeId
            AND ISNULL(x.MilestoneId, 0) = k.MilestoneId AND x.status = 1 AND x.archive = 0
     ORDER BY c.Code, m.milestone;
END
GO

/* Live budget vs actual. Columns follow requirements 3.1 with the breakdown of RB.
   Header (table 1) carries the project measures. */
ALTER PROCEDURE dbo.usp_pms_cc_BudgetVsActual @regid INT, @ProjectId INT, @GroupBy VARCHAR(20) = 'COSTCODE'
AS
BEGIN
    SET NOCOUNT ON;
    IF @GroupBy NOT IN ('COSTCODE', 'MILESTONE', 'COSTTYPE') SET @GroupBy = 'COSTCODE';

    ;WITH Src AS
    (
        SELECT CostCodeId, MilestoneId, OB, RevisionAdj, Corrections, ApprovedChanges, NetTransfers, RB,
               CAST(0 AS DECIMAL(18,2)) AS Actual, CAST(0 AS DECIMAL(18,2)) AS Etc
          FROM dbo.tvf_pms_cc_RevisedBudget(@regid, @ProjectId)
        UNION ALL
        SELECT CostCodeId, ISNULL(MilestoneId, 0), 0, 0, 0, 0, 0, 0, Amount, 0
          FROM dbo.vw_pms_cc_CostAllocation WHERE regid = @regid AND ProjectId = @ProjectId
        UNION ALL
        SELECT CostCodeId, ISNULL(MilestoneId, 0), 0, 0, 0, 0, 0, 0, 0, EtcAmount
          FROM dbo.Pms_tbl_ProjectEtc WHERE regid = @regid AND ProjectId = @ProjectId AND status = 1 AND archive = 0
    ),
    Grouped AS
    (
        SELECT CASE WHEN @GroupBy = 'MILESTONE' THEN CAST(s.MilestoneId AS NVARCHAR(40))
                    WHEN @GroupBy = 'COSTTYPE'  THEN ISNULL(c.CostType, N'(none)')
                    ELSE CAST(s.CostCodeId AS NVARCHAR(40)) END AS GroupKey, s.*
          FROM Src s LEFT JOIN dbo.Pms_tbl_CostCode c ON c.id = s.CostCodeId AND c.regid = @regid
    ),
    Agg AS
    (
        SELECT GroupKey, MIN(CostCodeId) AS AnyCostCodeId, MIN(MilestoneId) AS AnyMilestoneId,
               SUM(OB) AS OB, SUM(RevisionAdj) AS RevisionAdj, SUM(Corrections) AS Corrections, SUM(ApprovedChanges) AS ApprovedChanges,
               SUM(NetTransfers) AS NetTransfers, SUM(RB) AS RB, SUM(Actual) AS Actual, SUM(Etc) AS Etc
          FROM Grouped GROUP BY GroupKey
    )
    SELECT a.GroupKey AS GroupId,
           CASE WHEN @GroupBy = 'MILESTONE' THEN CASE WHEN a.AnyMilestoneId = 0 THEN N'Project level (no milestone)' ELSE ISNULL(m.milestone, N'(removed milestone)') END
                WHEN @GroupBy = 'COSTTYPE' THEN a.GroupKey
                ELSE ISNULL(c.Code + N' - ' + c.Name, N'(removed cost code)') END AS GroupName,
           CASE WHEN @GroupBy = 'COSTCODE' THEN ISNULL(c.CostType, N'') ELSE N'' END AS CostType,
           a.OB AS OriginalBudget, a.RevisionAdj, a.Corrections, a.ApprovedChanges, a.NetTransfers, a.RB AS CurrentBudget,
           a.Actual, a.RB - a.Actual AS Variance, a.Etc, a.Actual + a.Etc AS Eac, a.RB - (a.Actual + a.Etc) AS Vac,
           CASE WHEN a.RB = 0 THEN NULL ELSE CAST(ROUND(a.Actual * 100.0 / a.RB, 2) AS DECIMAL(18,2)) END AS ConsumedPercent
      FROM Agg a
      LEFT JOIN dbo.Pms_tbl_CostCode c ON @GroupBy = 'COSTCODE' AND c.id = a.AnyCostCodeId AND c.regid = @regid
      LEFT JOIN dbo.Pms_tbl_AddMilestone m ON @GroupBy = 'MILESTONE' AND m.id = a.AnyMilestoneId AND m.regid = @regid
     ORDER BY GroupName;

    SELECT (SELECT MAX(VersionNo) FROM dbo.Pms_tbl_ProjectBudgetVersion WHERE regid = @regid AND ProjectId = @ProjectId AND VersionStatus = N'Locked' AND status = 1 AND archive = 0) AS CurrentVersionNo,
           (SELECT COUNT(1) FROM dbo.Pms_tbl_ProjectBudgetVersion WHERE regid = @regid AND ProjectId = @ProjectId AND VersionStatus IN (N'Draft', N'Submitted') AND status = 1 AND archive = 0) AS DraftCount,
           (SELECT COUNT(1) FROM dbo.Pms_tbl_BudgetMovement WHERE regid = @regid AND ProjectId = @ProjectId AND MovementStatus IN (N'Draft', N'Submitted') AND status = 1 AND archive = 0) AS OpenMovements,
           pm.RSV, pm.PCE, pm.PPC, pm.AtRiskExposure, pm.Unrecovered,
           CAST(NULL AS INT) AS SnapshotId, CAST(NULL AS VARCHAR(10)) AS PeriodStart, CAST(NULL AS INT) AS SnapshotVersion
      FROM dbo.tvf_pms_cc_ProjectMeasures(@regid, @ProjectId, NULL) pm;
END
GO

IF OBJECT_ID('dbo.usp_pms_cc_Snapshot_Create', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_Snapshot_Create AS RETURN 0');
GO
-- Freezes the project figures as at the end of a month (PE-03). Called when the month is closed.
ALTER PROCEDURE dbo.usp_pms_cc_Snapshot_Create @regid INT, @ProjectId INT, @PeriodStart DATE, @uid INT, @SnapshotId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @PeriodEnd DATE = EOMONTH(@PeriodStart);
    DECLARE @Version INT = (SELECT ISNULL(MAX(SnapshotVersion), 0) + 1 FROM dbo.Pms_tbl_CostControlSnapshot
                             WHERE regid = @regid AND ProjectId = @ProjectId AND PeriodStart = @PeriodStart);
    DECLARE @Lines TABLE (CostCodeId INT, MilestoneId INT, OB DECIMAL(18,2), RevisionAdj DECIMAL(18,2), Corrections DECIMAL(18,2),
                          ApprovedChanges DECIMAL(18,2), NetTransfers DECIMAL(18,2), RB DECIMAL(18,2), Actual DECIMAL(18,2), Etc DECIMAL(18,2));
    ;WITH Src AS
    (
        SELECT CostCodeId, MilestoneId, OB, RevisionAdj, Corrections, ApprovedChanges, NetTransfers, RB, CAST(0 AS DECIMAL(18,2)) AS Actual, CAST(0 AS DECIMAL(18,2)) AS Etc
          FROM dbo.tvf_pms_cc_RevisedBudget(@regid, @ProjectId)
        UNION ALL
        SELECT CostCodeId, ISNULL(MilestoneId, 0), 0, 0, 0, 0, 0, 0, Amount, 0
          FROM dbo.vw_pms_cc_CostAllocation WHERE regid = @regid AND ProjectId = @ProjectId AND EntryDate <= @PeriodEnd
        UNION ALL
        SELECT CostCodeId, ISNULL(MilestoneId, 0), 0, 0, 0, 0, 0, 0, 0, EtcAmount
          FROM dbo.Pms_tbl_ProjectEtc WHERE regid = @regid AND ProjectId = @ProjectId AND status = 1 AND archive = 0
    )
    INSERT INTO @Lines SELECT CostCodeId, MilestoneId, SUM(OB), SUM(RevisionAdj), SUM(Corrections), SUM(ApprovedChanges), SUM(NetTransfers), SUM(RB), SUM(Actual), SUM(Etc)
      FROM Src GROUP BY CostCodeId, MilestoneId;

    INSERT INTO dbo.Pms_tbl_CostControlSnapshot (regid, ProjectId, PeriodStart, SnapshotVersion, BaselineVersionNo, TotalRB, TotalRSV, PCE, PPC, AtRiskExposure, Unrecovered, CreatedBy, CreatedOn)
    SELECT @regid, @ProjectId, @PeriodStart, @Version,
           (SELECT MAX(VersionNo) FROM dbo.Pms_tbl_ProjectBudgetVersion WHERE regid = @regid AND ProjectId = @ProjectId AND VersionStatus = N'Locked' AND status = 1 AND archive = 0),
           ISNULL((SELECT SUM(RB) FROM @Lines), 0), pm.RSV, pm.PCE, pm.PPC, pm.AtRiskExposure, pm.Unrecovered, @uid, GETDATE()
      FROM dbo.tvf_pms_cc_ProjectMeasures(@regid, @ProjectId, @PeriodEnd) pm;
    SET @SnapshotId = CAST(SCOPE_IDENTITY() AS INT);

    INSERT INTO dbo.Pms_tbl_CostControlSnapshotLine (SnapshotId, regid, CostCodeId, MilestoneId, CostCode, CostCodeName, CostType, MilestoneName,
                OB, RevisionAdj, Corrections, ApprovedChanges, NetTransfers, RB, Actual, Etc)
    SELECT @SnapshotId, @regid, l.CostCodeId, l.MilestoneId, c.Code, c.Name, c.CostType, m.milestone,
           l.OB, l.RevisionAdj, l.Corrections, l.ApprovedChanges, l.NetTransfers, l.RB, l.Actual, l.Etc
      FROM @Lines l
      LEFT JOIN dbo.Pms_tbl_CostCode c ON c.id = l.CostCodeId AND c.regid = @regid
      LEFT JOIN dbo.Pms_tbl_AddMilestone m ON m.id = l.MilestoneId AND m.regid = @regid;
END
GO

ALTER PROCEDURE dbo.usp_pms_cc_Period_SetStatus
    @regid INT, @ProjectId INT, @PeriodStart DATE, @Close BIT, @Reason NVARCHAR(1000) = NULL, @uid INT
AS
BEGIN
    SET NOCOUNT ON;
    IF dbo.ufn_pms_cc_CanApprove(@regid, @uid) = 0 BEGIN SELECT 0 AS Result, N'You do not have Cost Control approval rights.' AS Message; RETURN; END
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

    DECLARE @SnapshotId INT = NULL;
    SET XACT_ABORT ON;
    BEGIN TRANSACTION;
        IF @IsClosed IS NULL
            INSERT INTO dbo.Pms_tbl_CostControlPeriod (regid, ProjectId, PeriodStart, IsClosed, ClosedBy, ClosedOn)
            VALUES (@regid, @ProjectId, @PeriodStart, 1, @uid, GETDATE());
        ELSE IF @Close = 1
            UPDATE dbo.Pms_tbl_CostControlPeriod SET IsClosed = 1, ClosedBy = @uid, ClosedOn = GETDATE()
             WHERE regid = @regid AND ProjectId = @ProjectId AND PeriodStart = @PeriodStart;
        ELSE
            UPDATE dbo.Pms_tbl_CostControlPeriod SET IsClosed = 0, ReopenedBy = @uid, ReopenedOn = GETDATE(), ReopenReason = @Reason
             WHERE regid = @regid AND ProjectId = @ProjectId AND PeriodStart = @PeriodStart;

        -- Closing freezes the month's figures; reopening keeps every earlier snapshot (next close = version n+1).
        IF @Close = 1 EXEC dbo.usp_pms_cc_Snapshot_Create @regid, @ProjectId, @PeriodStart, @uid, @SnapshotId OUTPUT;

        DECLARE @Detail NVARCHAR(MAX) = (SELECT CONVERT(VARCHAR(10), @PeriodStart, 23) AS PeriodStart, @SnapshotId AS SnapshotId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
        DECLARE @AuditAction NVARCHAR(30) = CASE WHEN @Close = 1 THEN N'Close' ELSE N'Reopen' END;
        EXEC dbo.usp_pms_cc_Audit_Add @regid, @ProjectId, N'Period', NULL, @AuditAction, @Reason, NULL, @Detail, @uid;
    COMMIT TRANSACTION;
    SELECT 1 AS Result, CASE WHEN @Close = 1 THEN N'Month closed and snapshot saved.' ELSE N'Month reopened. The earlier snapshot is kept.' END AS Message;
END
GO

ALTER PROCEDURE dbo.usp_pms_cc_Period_List @regid INT, @ProjectId INT
AS
BEGIN
    SET NOCOUNT ON;
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
              AND e.EntryDate >= mo.PeriodStart AND e.EntryDate < DATEADD(MONTH, 1, mo.PeriodStart)), 0) AS EntryTotal,
           (SELECT COUNT(1) FROM dbo.Pms_tbl_CostControlSnapshot s WHERE s.regid = @regid AND s.ProjectId = @ProjectId AND s.PeriodStart = mo.PeriodStart) AS SnapshotCount,
           (SELECT TOP 1 s.id FROM dbo.Pms_tbl_CostControlSnapshot s WHERE s.regid = @regid AND s.ProjectId = @ProjectId AND s.PeriodStart = mo.PeriodStart ORDER BY s.SnapshotVersion DESC) AS LatestSnapshotId
      FROM Months mo
      LEFT JOIN dbo.Pms_tbl_CostControlPeriod p ON p.regid = @regid AND p.ProjectId = @ProjectId AND p.PeriodStart = mo.PeriodStart
      LEFT JOIN dbo.tbl_employee ec ON ec.id = p.ClosedBy
      LEFT JOIN dbo.tbl_employee er ON er.id = p.ReopenedBy
     ORDER BY mo.PeriodStart DESC
    OPTION (MAXRECURSION 1200);
END
GO

IF OBJECT_ID('dbo.usp_pms_cc_Snapshot_List', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_Snapshot_List AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_Snapshot_List @regid INT, @ProjectId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT s.id, CONVERT(VARCHAR(10), s.PeriodStart, 23) AS PeriodStart, s.SnapshotVersion, s.BaselineVersionNo,
           CONVERT(VARCHAR(16), s.CreatedOn, 120) AS CreatedOn, ISNULL(e.DisplayName, N'') AS CreatedByName
      FROM dbo.Pms_tbl_CostControlSnapshot s
      LEFT JOIN dbo.tbl_employee e ON e.id = s.CreatedBy
     WHERE s.regid = @regid AND s.ProjectId = @ProjectId
     ORDER BY s.PeriodStart DESC, s.SnapshotVersion DESC;
END
GO

IF OBJECT_ID('dbo.usp_pms_cc_Snapshot_Report', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_Snapshot_Report AS RETURN 0');
GO
-- Same columns as usp_pms_cc_BudgetVsActual, read only from the frozen snapshot (AC-20: identical on re-run).
ALTER PROCEDURE dbo.usp_pms_cc_Snapshot_Report @regid INT, @SnapshotId INT, @GroupBy VARCHAR(20) = 'COSTCODE'
AS
BEGIN
    SET NOCOUNT ON;
    IF @GroupBy NOT IN ('COSTCODE', 'MILESTONE', 'COSTTYPE') SET @GroupBy = 'COSTCODE';
    ;WITH Grouped AS
    (
        SELECT CASE WHEN @GroupBy = 'MILESTONE' THEN CAST(l.MilestoneId AS NVARCHAR(40))
                    WHEN @GroupBy = 'COSTTYPE' THEN ISNULL(l.CostType, N'(none)')
                    ELSE CAST(l.CostCodeId AS NVARCHAR(40)) END AS GroupKey,
               CASE WHEN @GroupBy = 'MILESTONE' THEN CASE WHEN l.MilestoneId = 0 THEN N'Project level (no milestone)' ELSE ISNULL(l.MilestoneName, N'(removed milestone)') END
                    WHEN @GroupBy = 'COSTTYPE' THEN ISNULL(l.CostType, N'(none)')
                    ELSE ISNULL(l.CostCode + N' - ' + l.CostCodeName, N'(removed cost code)') END AS GroupName,
               CASE WHEN @GroupBy = 'COSTCODE' THEN ISNULL(l.CostType, N'') ELSE N'' END AS DisplayCostType,
               l.CostCodeId, l.MilestoneId, l.OB, l.RevisionAdj, l.Corrections, l.ApprovedChanges, l.NetTransfers, l.RB, l.Actual, l.Etc
          FROM dbo.Pms_tbl_CostControlSnapshotLine l
         WHERE l.SnapshotId = @SnapshotId AND l.regid = @regid
    )
    SELECT GroupKey AS GroupId, MIN(GroupName) AS GroupName, MIN(DisplayCostType) AS CostType,
           SUM(OB) AS OriginalBudget, SUM(RevisionAdj) AS RevisionAdj, SUM(Corrections) AS Corrections, SUM(ApprovedChanges) AS ApprovedChanges,
           SUM(NetTransfers) AS NetTransfers, SUM(RB) AS CurrentBudget, SUM(Actual) AS Actual, SUM(RB) - SUM(Actual) AS Variance,
           SUM(Etc) AS Etc, SUM(Actual) + SUM(Etc) AS Eac, SUM(RB) - SUM(Actual) - SUM(Etc) AS Vac,
           CASE WHEN SUM(RB) = 0 THEN NULL ELSE CAST(ROUND(SUM(Actual) * 100.0 / SUM(RB), 2) AS DECIMAL(18,2)) END AS ConsumedPercent
      FROM Grouped
     GROUP BY GroupKey
     ORDER BY MIN(GroupName);

    SELECT s.BaselineVersionNo AS CurrentVersionNo, 0 AS DraftCount, 0 AS OpenMovements,
           s.TotalRSV AS RSV, s.PCE, s.PPC, s.AtRiskExposure, s.Unrecovered,
           s.id AS SnapshotId, CONVERT(VARCHAR(10), s.PeriodStart, 23) AS PeriodStart, s.SnapshotVersion, s.ProjectId
      FROM dbo.Pms_tbl_CostControlSnapshot s WHERE s.id = @SnapshotId AND s.regid = @regid;
END
GO

ALTER PROCEDURE dbo.usp_pms_cc_ProjectSummary @regid INT, @uid INT = NULL, @IsAdmin BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.id AS ProjectId, ISNULL(p.ProjectName, N'') AS ProjectName,
           (SELECT MAX(VersionNo) FROM dbo.Pms_tbl_ProjectBudgetVersion v WHERE v.regid = @regid AND v.ProjectId = p.id AND v.VersionStatus = N'Locked' AND v.status = 1 AND v.archive = 0) AS CurrentVersionNo,
           ISNULL((SELECT MAX(VersionStatus) FROM dbo.Pms_tbl_ProjectBudgetVersion v WHERE v.regid = @regid AND v.ProjectId = p.id AND v.VersionStatus IN (N'Draft', N'Submitted') AND v.status = 1 AND v.archive = 0), N'') AS OpenVersionStatus,
           ISNULL(b.RB, 0) AS CurrentBudget, ISNULL(a.Actual, 0) AS Actual, ISNULL(x.Etc, 0) AS Etc,
           ISNULL(a.Actual, 0) + ISNULL(x.Etc, 0) AS Eac,
           ISNULL(b.RB, 0) - (ISNULL(a.Actual, 0) + ISNULL(x.Etc, 0)) AS Vac,
           CASE WHEN ISNULL(b.RB, 0) = 0 THEN NULL ELSE CAST(ROUND(ISNULL(a.Actual, 0) * 100.0 / b.RB, 2) AS DECIMAL(18,2)) END AS ConsumedPercent,
           pm.RSV, pm.PCE, pm.PPC, ISNULL(a.Actual, 0) + ISNULL(x.Etc, 0) + pm.PPC AS EacP,
           ISNULL(b.RB, 0) + pm.RSV - (ISNULL(a.Actual, 0) + ISNULL(x.Etc, 0) + pm.PPC) AS VacP,
           pm.AtRiskExposure, pm.Unrecovered
      FROM dbo.Pms_tbl_AddProject p WITH (NOLOCK)
      OUTER APPLY (SELECT SUM(RB) AS RB FROM dbo.tvf_pms_cc_RevisedBudget(@regid, p.id)) b
      OUTER APPLY (SELECT SUM(Amount) AS Actual FROM dbo.vw_pms_cc_CostAllocation c WHERE c.regid = @regid AND c.ProjectId = p.id) a
      OUTER APPLY (SELECT SUM(EtcAmount) AS Etc FROM dbo.Pms_tbl_ProjectEtc e WHERE e.regid = @regid AND e.ProjectId = p.id AND e.status = 1 AND e.archive = 0) x
      CROSS APPLY dbo.tvf_pms_cc_ProjectMeasures(@regid, p.id, NULL) pm
     WHERE p.regid = @regid AND p.status = 1 AND p.archive = 0
       AND (@uid IS NULL OR dbo.ufn_pms_cc_ProjectAllowed(@regid, @uid, @IsAdmin, p.id) = 1)
       AND (EXISTS (SELECT 1 FROM dbo.Pms_tbl_ProjectBudgetVersion v WHERE v.regid = @regid AND v.ProjectId = p.id AND v.status = 1 AND v.archive = 0)
            OR a.Actual IS NOT NULL OR x.Etc IS NOT NULL)
     ORDER BY p.ProjectName;
END
GO

/* =====================================================================
   9. UNITS: duplicate check ignores case and extra spaces
   ===================================================================== */
ALTER PROCEDURE dbo.usp_pms_cc_Unit_Add @regid INT, @UnitName NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    -- Normalise: trim and collapse double spaces, so "Bags", " bags " and "BAGS" are the same unit.
    SET @UnitName = LTRIM(RTRIM(ISNULL(@UnitName, N'')));
    WHILE CHARINDEX(N'  ', @UnitName) > 0 SET @UnitName = REPLACE(@UnitName, N'  ', N' ');
    IF @UnitName = N'' BEGIN SELECT 0 AS Result, N'Enter a unit name.' AS Message; RETURN; END

    DECLARE @UnitId INT, @ExistingName NVARCHAR(50), @IsActive BIT;
    SELECT TOP 1 @UnitId = id, @ExistingName = unitname,
                 @IsActive = CASE WHEN ISNULL(status, 1) = 1 AND ISNULL(archive, 0) = 0 THEN 1 ELSE 0 END
      FROM dbo.tbl_unitmaster
     WHERE regid = @regid AND UPPER(LTRIM(RTRIM(REPLACE(unitname, N'  ', N' ')))) = UPPER(@UnitName)
     ORDER BY CASE WHEN ISNULL(status, 1) = 1 AND ISNULL(archive, 0) = 0 THEN 0 ELSE 1 END, id;

    IF @UnitId IS NOT NULL AND @IsActive = 1
        BEGIN SELECT 1 AS Result, N'Unit "' + @ExistingName + N'" already exists, so it has been selected instead of adding a duplicate.' AS Message, @UnitId AS id, 1 AS AlreadyExists; RETURN; END
    IF @UnitId IS NOT NULL
        BEGIN SELECT 0 AS Result, N'Unit "' + @ExistingName + N'" already exists but is inactive in the unit master. Reactivate it there.' AS Message; RETURN; END

    -- Reuse the existing ERP insert procedure so unit-master and Tally behaviour stay unchanged.
    EXEC dbo.tbl_unitmasterInsertNew @regid = @regid, @unitname = @UnitName;

    SELECT TOP 1 @UnitId = id FROM dbo.tbl_unitmaster
     WHERE regid = @regid AND unitname = @UnitName AND ISNULL(status, 1) = 1 AND ISNULL(archive, 0) = 0
     ORDER BY id DESC;
    IF @UnitId IS NULL SELECT 0 AS Result, N'The unit could not be added.' AS Message;
    ELSE SELECT 1 AS Result, N'Unit added.' AS Message, @UnitId AS id, 0 AS AlreadyExists;
END
GO
-- Unit drop-down: one row per unit name (the master already holds some duplicates such as PCS / CM / METER;
-- they are not changed here). The earliest active id of each name is offered.
ALTER PROCEDURE dbo.usp_pms_cc_Unit_List @regid INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT MIN(u.id) AS id, MIN(LTRIM(RTRIM(u.unitname))) AS UnitName
      FROM dbo.tbl_unitmaster u WITH (NOLOCK)
     WHERE u.regid = @regid AND ISNULL(u.status, 1) = 1 AND ISNULL(u.archive, 0) = 0 AND LTRIM(RTRIM(ISNULL(u.unitname, N''))) <> N''
     GROUP BY UPPER(LTRIM(RTRIM(u.unitname)))
     ORDER BY MIN(LTRIM(RTRIM(u.unitname)));
END
GO