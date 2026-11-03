/*
===============================================================================
PMS QC WORKFLOW - SINGLE SERVER DEPLOYMENT SCRIPT
===============================================================================
Purpose:
    One SQL file containing the workflow database objects used by the current
    QC Workflow configuration page.

SAFE DEPLOYMENT:
    - DOES NOT DROP workflow tables.
    - DOES NOT DELETE existing workflow headers as part of installation.
    - Creates missing tables/columns/indexes.
    - Creates/updates workflow stored procedures.
    - Existing PMS master tables are reused.

CURRENT WORKFLOW TABLES
-----------------------
1. dbo.Pms_tbl_Workflow
2. dbo.Pms_tbl_WorkflowCondition
3. dbo.Pms_tbl_WorkflowAction
4. dbo.Pms_tbl_WorkflowUserAction
5. dbo.Pms_tbl_WorkflowThenAction
   (compatibility table for the separate THEN Action persistence patch)

EXISTING PMS MASTER TABLE DEPENDENCIES
--------------------------------------
1. dbo.Pms_tbl_SimpleMasterList    -- QC Action master
2. dbo.Pms_tbl_Projectstatus       -- Next Status / Status condition
3. dbo.Pms_tbl_Priority            -- Priority condition
4. Existing Department master
5. Existing Designation master

WORKFLOW STORED PROCEDURES
--------------------------
1. dbo.usp_Pms_Workflow_GetMasters
2. dbo.usp_Pms_Workflow_List
3. dbo.usp_Pms_Workflow_GetById
4. dbo.usp_Pms_Workflow_Save
5. dbo.usp_Pms_Workflow_Delete
6. dbo.usp_Pms_Workflow_ChangeStatus

ACTION CONDITIONS / USER ACTION
-------------------------------
7. dbo.usp_Pms_WorkflowUserAction_GetByWorkflow
8. dbo.usp_Pms_WorkflowUserAction_Save
9. dbo.usp_Pms_WorkflowUserAction_DeleteByWorkflow

THEN ACTION COMPATIBILITY
-------------------------
10. dbo.usp_Pms_WorkflowThenAction_Save
11. dbo.usp_Pms_WorkflowThenAction_GetByWorkflow
12. dbo.usp_Pms_WorkflowThenAction_DeleteByWorkflow

NOT INCLUDED YET
----------------
Runtime Workflow Instance / Runtime Workflow History is NOT implemented yet.
This script contains only the workflow configuration objects managed till now.

Run this script on the PMS database used by the website.
===============================================================================
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

/* ============================================================================
   0. MASTER DEPENDENCY INFORMATION
   ============================================================================ */
PRINT 'PMS QC Workflow deployment started...';
GO

IF OBJECT_ID('dbo.Pms_tbl_SimpleMasterList','U') IS NULL
    PRINT 'WARNING: dbo.Pms_tbl_SimpleMasterList not found. QC Action dropdown requires it.';
IF OBJECT_ID('dbo.Pms_tbl_Projectstatus','U') IS NULL
    PRINT 'WARNING: dbo.Pms_tbl_Projectstatus not found. Status/Next Status requires it.';
IF OBJECT_ID('dbo.Pms_tbl_Priority','U') IS NULL
    PRINT 'WARNING: dbo.Pms_tbl_Priority not found. Priority condition requires it.';
GO


/* ============================================================================
   1. WORKFLOW HEADER
   ============================================================================ */
IF OBJECT_ID('dbo.Pms_tbl_Workflow','U') IS NULL
BEGIN
    CREATE TABLE dbo.Pms_tbl_Workflow
    (
        id              INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Pms_tbl_Workflow PRIMARY KEY,
        WorkflowName    NVARCHAR(250) NOT NULL,
        ApplyOn         NVARCHAR(30) NOT NULL,
        OrderNo         INT NOT NULL CONSTRAINT DF_Pms_Workflow_OrderNo DEFAULT(1),
        Description     NVARCHAR(1000) NULL,
        IsActive        BIT NOT NULL CONSTRAINT DF_Pms_Workflow_IsActive DEFAULT(1),
        regid           INT NOT NULL,
        uid             INT NULL,
        status          BIT NOT NULL CONSTRAINT DF_Pms_Workflow_Status DEFAULT(1),
        archive         BIT NOT NULL CONSTRAINT DF_Pms_Workflow_Archive DEFAULT(0),
        cdate           DATETIME NULL CONSTRAINT DF_Pms_Workflow_CDate DEFAULT(GETDATE()),
        modifieddate    DATETIME NULL
    );
END
GO

IF COL_LENGTH('dbo.Pms_tbl_Workflow','WorkflowName') IS NULL
    ALTER TABLE dbo.Pms_tbl_Workflow ADD WorkflowName NVARCHAR(250) NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_Workflow','ApplyOn') IS NULL
    ALTER TABLE dbo.Pms_tbl_Workflow ADD ApplyOn NVARCHAR(30) NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_Workflow','OrderNo') IS NULL
    ALTER TABLE dbo.Pms_tbl_Workflow ADD OrderNo INT NULL DEFAULT(1);
GO
IF COL_LENGTH('dbo.Pms_tbl_Workflow','Description') IS NULL
    ALTER TABLE dbo.Pms_tbl_Workflow ADD Description NVARCHAR(1000) NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_Workflow','IsActive') IS NULL
    ALTER TABLE dbo.Pms_tbl_Workflow ADD IsActive BIT NULL DEFAULT(1);
GO
IF COL_LENGTH('dbo.Pms_tbl_Workflow','regid') IS NULL
    ALTER TABLE dbo.Pms_tbl_Workflow ADD regid INT NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_Workflow','uid') IS NULL
    ALTER TABLE dbo.Pms_tbl_Workflow ADD uid INT NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_Workflow','status') IS NULL
    ALTER TABLE dbo.Pms_tbl_Workflow ADD status BIT NULL DEFAULT(1);
GO
IF COL_LENGTH('dbo.Pms_tbl_Workflow','archive') IS NULL
    ALTER TABLE dbo.Pms_tbl_Workflow ADD archive BIT NULL DEFAULT(0);
GO
IF COL_LENGTH('dbo.Pms_tbl_Workflow','cdate') IS NULL
    ALTER TABLE dbo.Pms_tbl_Workflow ADD cdate DATETIME NULL DEFAULT(GETDATE());
GO
IF COL_LENGTH('dbo.Pms_tbl_Workflow','modifieddate') IS NULL
    ALTER TABLE dbo.Pms_tbl_Workflow ADD modifieddate DATETIME NULL;
GO


/* ============================================================================
   2. WORKFLOW CONDITIONS
   ============================================================================ */
IF OBJECT_ID('dbo.Pms_tbl_WorkflowCondition','U') IS NULL
BEGIN
    CREATE TABLE dbo.Pms_tbl_WorkflowCondition
    (
        id                  INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Pms_tbl_WorkflowCondition PRIMARY KEY,
        WorkflowId          INT NOT NULL,
        SrNo                INT NULL CONSTRAINT DF_Pms_WorkflowCondition_SrNo DEFAULT(1),
        BranchType          NVARCHAR(20) NULL,
        SequenceNo          INT NULL CONSTRAINT DF_Pms_WorkflowCondition_SequenceNo DEFAULT(1),
        FieldCode           NVARCHAR(100) NULL,
        OperatorCode        NVARCHAR(50) NULL,
        CompareValue        NVARCHAR(1000) NULL,
        CompareValue2       NVARCHAR(1000) NULL,
        LogicalOperator     NVARCHAR(10) NULL,
        regid               INT NULL,
        uid                 INT NULL,
        status              BIT NULL CONSTRAINT DF_Pms_WorkflowCondition_Status DEFAULT(1),
        archive             BIT NULL CONSTRAINT DF_Pms_WorkflowCondition_Archive DEFAULT(0),
        cdate               DATETIME NULL CONSTRAINT DF_Pms_WorkflowCondition_CDate DEFAULT(GETDATE()),
        modifieddate        DATETIME NULL
    );
END
GO

IF COL_LENGTH('dbo.Pms_tbl_WorkflowCondition','WorkflowId') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowCondition ADD WorkflowId INT NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowCondition','SrNo') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowCondition ADD SrNo INT NULL DEFAULT(1);
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowCondition','BranchType') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowCondition ADD BranchType NVARCHAR(20) NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowCondition','SequenceNo') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowCondition ADD SequenceNo INT NULL DEFAULT(1);
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowCondition','FieldCode') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowCondition ADD FieldCode NVARCHAR(100) NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowCondition','OperatorCode') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowCondition ADD OperatorCode NVARCHAR(50) NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowCondition','CompareValue') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowCondition ADD CompareValue NVARCHAR(1000) NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowCondition','CompareValue2') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowCondition ADD CompareValue2 NVARCHAR(1000) NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowCondition','LogicalOperator') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowCondition ADD LogicalOperator NVARCHAR(10) NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowCondition','regid') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowCondition ADD regid INT NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowCondition','uid') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowCondition ADD uid INT NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowCondition','status') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowCondition ADD status BIT NULL DEFAULT(1);
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowCondition','archive') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowCondition ADD archive BIT NULL DEFAULT(0);
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowCondition','cdate') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowCondition ADD cdate DATETIME NULL DEFAULT(GETDATE());
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowCondition','modifieddate') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowCondition ADD modifieddate DATETIME NULL;
GO


/* ============================================================================
   3. MAIN THEN ACTION TABLE
   ============================================================================ */
IF OBJECT_ID('dbo.Pms_tbl_WorkflowAction','U') IS NULL
BEGIN
    CREATE TABLE dbo.Pms_tbl_WorkflowAction
    (
        id              INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Pms_tbl_WorkflowAction PRIMARY KEY,
        WorkflowId      INT NOT NULL,
        SrNo            INT NULL CONSTRAINT DF_Pms_WorkflowAction_SrNo DEFAULT(1),
        BranchType      NVARCHAR(20) NULL,
        SequenceNo      INT NULL CONSTRAINT DF_Pms_WorkflowAction_SequenceNo DEFAULT(1),
        TakenById       INT NULL,
        DesignationId   INT NULL,
        IsEmail         BIT NULL CONSTRAINT DF_Pms_WorkflowAction_IsEmail DEFAULT(0),
        IsWhatsApp      BIT NULL CONSTRAINT DF_Pms_WorkflowAction_IsWhatsApp DEFAULT(0),
        IsBell          BIT NULL CONSTRAINT DF_Pms_WorkflowAction_IsBell DEFAULT(0),
        regid           INT NULL,
        uid             INT NULL,
        status          BIT NULL CONSTRAINT DF_Pms_WorkflowAction_Status DEFAULT(1),
        archive         BIT NULL CONSTRAINT DF_Pms_WorkflowAction_Archive DEFAULT(0),
        cdate           DATETIME NULL CONSTRAINT DF_Pms_WorkflowAction_CDate DEFAULT(GETDATE()),
        modifieddate    DATETIME NULL
    );
END
GO

IF COL_LENGTH('dbo.Pms_tbl_WorkflowAction','WorkflowId') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowAction ADD WorkflowId INT NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowAction','SrNo') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowAction ADD SrNo INT NULL DEFAULT(1);
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowAction','BranchType') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowAction ADD BranchType NVARCHAR(20) NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowAction','SequenceNo') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowAction ADD SequenceNo INT NULL DEFAULT(1);
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowAction','TakenById') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowAction ADD TakenById INT NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowAction','DesignationId') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowAction ADD DesignationId INT NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowAction','IsEmail') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowAction ADD IsEmail BIT NULL DEFAULT(0);
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowAction','IsWhatsApp') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowAction ADD IsWhatsApp BIT NULL DEFAULT(0);
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowAction','IsBell') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowAction ADD IsBell BIT NULL DEFAULT(0);
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowAction','regid') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowAction ADD regid INT NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowAction','uid') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowAction ADD uid INT NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowAction','status') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowAction ADD status BIT NULL DEFAULT(1);
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowAction','archive') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowAction ADD archive BIT NULL DEFAULT(0);
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowAction','cdate') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowAction ADD cdate DATETIME NULL DEFAULT(GETDATE());
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowAction','modifieddate') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowAction ADD modifieddate DATETIME NULL;
GO


/* ============================================================================
   4. USER ACTION / ACTION CONDITIONS
   ============================================================================ */
IF OBJECT_ID('dbo.Pms_tbl_WorkflowUserAction','U') IS NULL
BEGIN
    CREATE TABLE dbo.Pms_tbl_WorkflowUserAction
    (
        id                  INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Pms_tbl_WorkflowUserAction PRIMARY KEY,
        WorkflowId          INT NOT NULL,
        SrNo                INT NULL CONSTRAINT DF_Pms_WorkflowUserAction_SrNo DEFAULT(1),
        BranchType          NVARCHAR(20) NULL,
        SequenceNo          INT NULL CONSTRAINT DF_Pms_WorkflowUserAction_SequenceNo DEFAULT(1),
        ActionBranchType    NVARCHAR(20) NULL,
        ActionId            INT NULL,
        NextStatusId        INT NULL,
        TakenById           INT NULL,
        DesignationId       INT NULL,
        IsEmail             BIT NULL CONSTRAINT DF_Pms_WorkflowUserAction_IsEmail DEFAULT(0),
        IsWhatsApp          BIT NULL CONSTRAINT DF_Pms_WorkflowUserAction_IsWhatsApp DEFAULT(0),
        IsBell              BIT NULL CONSTRAINT DF_Pms_WorkflowUserAction_IsBell DEFAULT(0),
        IsRemarkRequired    BIT NULL CONSTRAINT DF_Pms_WorkflowUserAction_IsRemarkRequired DEFAULT(0),
        regid               INT NULL,
        uid                 INT NULL,
        status              BIT NULL CONSTRAINT DF_Pms_WorkflowUserAction_Status DEFAULT(1),
        archive             BIT NULL CONSTRAINT DF_Pms_WorkflowUserAction_Archive DEFAULT(0),
        cdate               DATETIME NULL CONSTRAINT DF_Pms_WorkflowUserAction_CDate DEFAULT(GETDATE()),
        modifieddate        DATETIME NULL
    );
END
GO

IF COL_LENGTH('dbo.Pms_tbl_WorkflowUserAction','WorkflowId') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowUserAction ADD WorkflowId INT NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowUserAction','SrNo') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowUserAction ADD SrNo INT NULL DEFAULT(1);
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowUserAction','BranchType') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowUserAction ADD BranchType NVARCHAR(20) NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowUserAction','SequenceNo') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowUserAction ADD SequenceNo INT NULL DEFAULT(1);
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowUserAction','ActionBranchType') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowUserAction ADD ActionBranchType NVARCHAR(20) NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowUserAction','ActionId') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowUserAction ADD ActionId INT NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowUserAction','NextStatusId') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowUserAction ADD NextStatusId INT NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowUserAction','TakenById') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowUserAction ADD TakenById INT NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowUserAction','DesignationId') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowUserAction ADD DesignationId INT NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowUserAction','IsEmail') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowUserAction ADD IsEmail BIT NULL DEFAULT(0);
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowUserAction','IsWhatsApp') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowUserAction ADD IsWhatsApp BIT NULL DEFAULT(0);
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowUserAction','IsBell') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowUserAction ADD IsBell BIT NULL DEFAULT(0);
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowUserAction','IsRemarkRequired') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowUserAction ADD IsRemarkRequired BIT NULL DEFAULT(0);
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowUserAction','regid') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowUserAction ADD regid INT NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowUserAction','uid') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowUserAction ADD uid INT NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowUserAction','status') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowUserAction ADD status BIT NULL DEFAULT(1);
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowUserAction','archive') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowUserAction ADD archive BIT NULL DEFAULT(0);
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowUserAction','cdate') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowUserAction ADD cdate DATETIME NULL DEFAULT(GETDATE());
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowUserAction','modifieddate') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowUserAction ADD modifieddate DATETIME NULL;
GO

/* ELSE route is allowed to have NULL ActionId. */
IF COL_LENGTH('dbo.Pms_tbl_WorkflowUserAction','ActionId') IS NOT NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowUserAction ALTER COLUMN ActionId INT NULL;
GO


/* ============================================================================
   5. DEDICATED THEN ACTION TABLE - COMPATIBILITY WITH LATEST PATCH
   ============================================================================ */
IF OBJECT_ID('dbo.Pms_tbl_WorkflowThenAction','U') IS NULL
BEGIN
    CREATE TABLE dbo.Pms_tbl_WorkflowThenAction
    (
        id              INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Pms_tbl_WorkflowThenAction PRIMARY KEY,
        WorkflowId      INT NOT NULL,
        SrNo            INT NULL CONSTRAINT DF_Pms_WorkflowThenAction_SrNo DEFAULT(1),
        BranchType      NVARCHAR(20) NULL,
        SequenceNo      INT NULL CONSTRAINT DF_Pms_WorkflowThenAction_SequenceNo DEFAULT(1),
        TakenById       INT NULL,
        DesignationId   INT NULL,
        IsEmail         BIT NULL CONSTRAINT DF_Pms_WorkflowThenAction_IsEmail DEFAULT(0),
        IsWhatsApp      BIT NULL CONSTRAINT DF_Pms_WorkflowThenAction_IsWhatsApp DEFAULT(0),
        IsBell          BIT NULL CONSTRAINT DF_Pms_WorkflowThenAction_IsBell DEFAULT(0),
        regid           INT NULL,
        uid             INT NULL,
        status          BIT NULL CONSTRAINT DF_Pms_WorkflowThenAction_Status DEFAULT(1),
        archive         BIT NULL CONSTRAINT DF_Pms_WorkflowThenAction_Archive DEFAULT(0),
        cdate           DATETIME NULL CONSTRAINT DF_Pms_WorkflowThenAction_CDate DEFAULT(GETDATE()),
        modifieddate    DATETIME NULL
    );
END
GO

IF COL_LENGTH('dbo.Pms_tbl_WorkflowThenAction','WorkflowId') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowThenAction ADD WorkflowId INT NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowThenAction','SrNo') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowThenAction ADD SrNo INT NULL DEFAULT(1);
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowThenAction','BranchType') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowThenAction ADD BranchType NVARCHAR(20) NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowThenAction','SequenceNo') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowThenAction ADD SequenceNo INT NULL DEFAULT(1);
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowThenAction','TakenById') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowThenAction ADD TakenById INT NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowThenAction','DesignationId') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowThenAction ADD DesignationId INT NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowThenAction','IsEmail') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowThenAction ADD IsEmail BIT NULL DEFAULT(0);
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowThenAction','IsWhatsApp') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowThenAction ADD IsWhatsApp BIT NULL DEFAULT(0);
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowThenAction','IsBell') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowThenAction ADD IsBell BIT NULL DEFAULT(0);
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowThenAction','regid') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowThenAction ADD regid INT NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowThenAction','uid') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowThenAction ADD uid INT NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowThenAction','status') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowThenAction ADD status BIT NULL DEFAULT(1);
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowThenAction','archive') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowThenAction ADD archive BIT NULL DEFAULT(0);
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowThenAction','cdate') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowThenAction ADD cdate DATETIME NULL DEFAULT(GETDATE());
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowThenAction','modifieddate') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowThenAction ADD modifieddate DATETIME NULL;
GO


/* ============================================================================
   6. INDEXES
   ============================================================================ */
IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID('dbo.Pms_tbl_Workflow')
      AND name = 'IX_Pms_Workflow_RegId'
)
    CREATE INDEX IX_Pms_Workflow_RegId
        ON dbo.Pms_tbl_Workflow(regid, archive, IsActive, OrderNo);
GO

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID('dbo.Pms_tbl_WorkflowCondition')
      AND name = 'IX_Pms_WorkflowCondition_Workflow'
)
    CREATE INDEX IX_Pms_WorkflowCondition_Workflow
        ON dbo.Pms_tbl_WorkflowCondition(WorkflowId, regid, archive, SrNo, SequenceNo);
GO

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID('dbo.Pms_tbl_WorkflowAction')
      AND name = 'IX_Pms_WorkflowAction_Workflow'
)
    CREATE INDEX IX_Pms_WorkflowAction_Workflow
        ON dbo.Pms_tbl_WorkflowAction(WorkflowId, regid, archive, SrNo, SequenceNo);
GO

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID('dbo.Pms_tbl_WorkflowUserAction')
      AND name = 'IX_Pms_WorkflowUserAction_Workflow'
)
    CREATE INDEX IX_Pms_WorkflowUserAction_Workflow
        ON dbo.Pms_tbl_WorkflowUserAction(WorkflowId, regid, archive, SrNo, SequenceNo);
GO

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID('dbo.Pms_tbl_WorkflowThenAction')
      AND name = 'IX_Pms_WorkflowThenAction_Workflow'
)
    CREATE INDEX IX_Pms_WorkflowThenAction_Workflow
        ON dbo.Pms_tbl_WorkflowThenAction(WorkflowId, regid, archive, SrNo, SequenceNo);
GO


/* ============================================================================
   7. GET WORKFLOW MASTERS
      Returns five result sets expected by WorkflowSetting.aspx.cs:
      1 Actions, 2 Statuses, 3 Priorities, 4 Departments, 5 Designations

      Department/Designation are discovered from the existing PMS DB because
      they are existing main masters and their table names can differ between
      installations.
   ============================================================================ */
IF OBJECT_ID('dbo.usp_Pms_Workflow_GetMasters','P') IS NULL
    EXEC('CREATE PROCEDURE dbo.usp_Pms_Workflow_GetMasters AS BEGIN SET NOCOUNT ON; END');
GO

ALTER PROCEDURE dbo.usp_Pms_Workflow_GetMasters
(
    @regid INT
)
AS
BEGIN
    SET NOCOUNT ON;

    /* 1. QC ACTION MASTER */
    IF OBJECT_ID('dbo.Pms_tbl_SimpleMasterList','U') IS NOT NULL
    BEGIN
        SELECT
            id,
            MasterName AS ActionName
        FROM dbo.Pms_tbl_SimpleMasterList
        WHERE ISNULL(status,1)=1
          AND ISNULL(archive,0)=0
          AND regid=@regid
        ORDER BY MasterName,id;
    END
    ELSE
    BEGIN
        SELECT CAST(NULL AS INT) AS id,
               CAST(NULL AS NVARCHAR(250)) AS ActionName
        WHERE 1=0;
    END;

    /* 2. STATUS MASTER */
    IF OBJECT_ID('dbo.Pms_tbl_Projectstatus','U') IS NOT NULL
    BEGIN
        SELECT
            id,
            ProjectStatus AS ValueText
        FROM dbo.Pms_tbl_Projectstatus
        WHERE ISNULL(status,1)=1
          AND ISNULL(archive,0)=0
          AND regid=@regid
        ORDER BY ProjectStatus,id;
    END
    ELSE
    BEGIN
        SELECT CAST(NULL AS INT) AS id,
               CAST(NULL AS NVARCHAR(250)) AS ValueText
        WHERE 1=0;
    END;

    /* 3. PRIORITY MASTER */
    IF OBJECT_ID('dbo.Pms_tbl_Priority','U') IS NOT NULL
    BEGIN
        SELECT
            id,
            name AS ValueText
        FROM dbo.Pms_tbl_Priority
        WHERE ISNULL(status,1)=1
          AND ISNULL(archive,0)=0
          AND regid=@regid
        ORDER BY name,id;
    END
    ELSE
    BEGIN
        SELECT CAST(NULL AS INT) AS id,
               CAST(NULL AS NVARCHAR(250)) AS ValueText
        WHERE 1=0;
    END;

    /* 4. DEPARTMENT MASTER - discover existing PMS/HR master */
    DECLARE
        @DeptObjectId INT = NULL,
        @DeptSchema SYSNAME = NULL,
        @DeptTable SYSNAME = NULL,
        @DeptColumn SYSNAME = NULL,
        @DeptSql NVARCHAR(MAX);

    SELECT TOP(1)
        @DeptObjectId = T.object_id,
        @DeptSchema = S.name,
        @DeptTable = T.name,
        @DeptColumn = C.name
    FROM sys.tables T
    INNER JOIN sys.schemas S ON S.schema_id=T.schema_id
    INNER JOIN sys.columns IDCOL
        ON IDCOL.object_id=T.object_id AND LOWER(IDCOL.name)='id'
    INNER JOIN sys.columns C
        ON C.object_id=T.object_id
       AND LOWER(C.name) IN
           ('departmentname','department_name','department','deptname','dept_name','name')
    WHERE
        (
            LOWER(T.name) LIKE '%department%'
            OR LOWER(T.name) LIKE '%dept%'
        )
        AND T.name NOT LIKE 'Pms_tbl_Workflow%'
    ORDER BY
        CASE LOWER(C.name)
            WHEN 'departmentname' THEN 1
            WHEN 'department_name' THEN 2
            WHEN 'department' THEN 3
            WHEN 'deptname' THEN 4
            WHEN 'dept_name' THEN 5
            ELSE 6
        END,
        CASE WHEN LOWER(T.name) LIKE '%master%' THEN 0 ELSE 1 END,
        T.name;

    IF @DeptObjectId IS NOT NULL
    BEGIN
        SET @DeptSql =
            N'SELECT id, CONVERT(NVARCHAR(250),' + QUOTENAME(@DeptColumn) +
            N') AS DepartmentName FROM ' +
            QUOTENAME(@DeptSchema) + N'.' + QUOTENAME(@DeptTable) +
            N' WHERE 1=1 ';

        IF EXISTS(SELECT 1 FROM sys.columns WHERE object_id=@DeptObjectId AND LOWER(name)='regid')
            SET @DeptSql += N' AND regid=@regid ';

        IF EXISTS(SELECT 1 FROM sys.columns WHERE object_id=@DeptObjectId AND LOWER(name)='status')
            SET @DeptSql += N' AND ISNULL(status,1)=1 ';

        IF EXISTS(SELECT 1 FROM sys.columns WHERE object_id=@DeptObjectId AND LOWER(name)='archive')
            SET @DeptSql += N' AND ISNULL(archive,0)=0 ';

        SET @DeptSql += N' ORDER BY ' + QUOTENAME(@DeptColumn) + N',id;';

        EXEC sys.sp_executesql
            @DeptSql,
            N'@regid INT',
            @regid=@regid;
    END
    ELSE
    BEGIN
        SELECT CAST(NULL AS INT) AS id,
               CAST(NULL AS NVARCHAR(250)) AS DepartmentName
        WHERE 1=0;
    END;

    /* 5. DESIGNATION MASTER - discover existing PMS/HR master */
    DECLARE
        @DesigObjectId INT = NULL,
        @DesigSchema SYSNAME = NULL,
        @DesigTable SYSNAME = NULL,
        @DesigColumn SYSNAME = NULL,
        @DesigSql NVARCHAR(MAX);

    SELECT TOP(1)
        @DesigObjectId = T.object_id,
        @DesigSchema = S.name,
        @DesigTable = T.name,
        @DesigColumn = C.name
    FROM sys.tables T
    INNER JOIN sys.schemas S ON S.schema_id=T.schema_id
    INNER JOIN sys.columns IDCOL
        ON IDCOL.object_id=T.object_id AND LOWER(IDCOL.name)='id'
    INNER JOIN sys.columns C
        ON C.object_id=T.object_id
       AND LOWER(C.name) IN
           ('designationname','designation_name','designation','jobtitle','job_title','title','name')
    WHERE
        (
            LOWER(T.name) LIKE '%designation%'
            OR LOWER(T.name) LIKE '%jobtitle%'
            OR LOWER(T.name) LIKE '%job_title%'
        )
        AND T.name NOT LIKE 'Pms_tbl_Workflow%'
    ORDER BY
        CASE LOWER(C.name)
            WHEN 'designationname' THEN 1
            WHEN 'designation_name' THEN 2
            WHEN 'designation' THEN 3
            WHEN 'jobtitle' THEN 4
            WHEN 'job_title' THEN 5
            WHEN 'title' THEN 6
            ELSE 7
        END,
        CASE WHEN LOWER(T.name) LIKE '%master%' THEN 0 ELSE 1 END,
        T.name;

    IF @DesigObjectId IS NOT NULL
    BEGIN
        SET @DesigSql =
            N'SELECT id, CONVERT(NVARCHAR(250),' + QUOTENAME(@DesigColumn) +
            N') AS DesignationName FROM ' +
            QUOTENAME(@DesigSchema) + N'.' + QUOTENAME(@DesigTable) +
            N' WHERE 1=1 ';

        IF EXISTS(SELECT 1 FROM sys.columns WHERE object_id=@DesigObjectId AND LOWER(name)='regid')
            SET @DesigSql += N' AND regid=@regid ';

        IF EXISTS(SELECT 1 FROM sys.columns WHERE object_id=@DesigObjectId AND LOWER(name)='status')
            SET @DesigSql += N' AND ISNULL(status,1)=1 ';

        IF EXISTS(SELECT 1 FROM sys.columns WHERE object_id=@DesigObjectId AND LOWER(name)='archive')
            SET @DesigSql += N' AND ISNULL(archive,0)=0 ';

        SET @DesigSql += N' ORDER BY ' + QUOTENAME(@DesigColumn) + N',id;';

        EXEC sys.sp_executesql
            @DesigSql,
            N'@regid INT',
            @regid=@regid;
    END
    ELSE
    BEGIN
        SELECT CAST(NULL AS INT) AS id,
               CAST(NULL AS NVARCHAR(250)) AS DesignationName
        WHERE 1=0;
    END;
END
GO


/* ============================================================================
   8. WORKFLOW LIST
   ============================================================================ */
IF OBJECT_ID('dbo.usp_Pms_Workflow_List','P') IS NULL
    EXEC('CREATE PROCEDURE dbo.usp_Pms_Workflow_List AS BEGIN SET NOCOUNT ON; END');
GO

ALTER PROCEDURE dbo.usp_Pms_Workflow_List
(
    @regid INT
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        W.id,
        W.WorkflowName,
        W.ApplyOn,
        ISNULL(W.OrderNo,1) AS OrderNo,
        W.Description,
        CAST(ISNULL(W.IsActive,1) AS BIT) AS status,
        CAST(ISNULL(W.IsActive,1) AS BIT) AS IsActive,
        W.cdate,
        (
            SELECT COUNT(1)
            FROM dbo.Pms_tbl_WorkflowCondition C
            WHERE C.WorkflowId=W.id
              AND C.regid=W.regid
              AND ISNULL(C.status,1)=1
              AND ISNULL(C.archive,0)=0
        ) AS ConditionCount,
        (
            SELECT COUNT(1)
            FROM dbo.Pms_tbl_WorkflowAction A
            WHERE A.WorkflowId=W.id
              AND A.regid=W.regid
              AND ISNULL(A.status,1)=1
              AND ISNULL(A.archive,0)=0
        ) AS ActionCount
    FROM dbo.Pms_tbl_Workflow W
    WHERE W.regid=@regid
      AND ISNULL(W.status,1)=1
      AND ISNULL(W.archive,0)=0
    ORDER BY ISNULL(W.OrderNo,1), W.id;
END
GO


/* ============================================================================
   9. GET WORKFLOW FOR EDIT
      Result 1: Header
      Result 2: Conditions
      Result 3: THEN Actions
   ============================================================================ */
IF OBJECT_ID('dbo.usp_Pms_Workflow_GetById','P') IS NULL
    EXEC('CREATE PROCEDURE dbo.usp_Pms_Workflow_GetById AS BEGIN SET NOCOUNT ON; END');
GO

ALTER PROCEDURE dbo.usp_Pms_Workflow_GetById
(
    @WorkflowId INT,
    @regid INT
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        W.id,
        W.WorkflowName,
        W.ApplyOn,
        ISNULL(W.OrderNo,1) AS OrderNo,
        W.Description,
        CAST(ISNULL(W.IsActive,1) AS BIT) AS status,
        CAST(ISNULL(W.IsActive,1) AS BIT) AS IsActive
    FROM dbo.Pms_tbl_Workflow W
    WHERE W.id=@WorkflowId
      AND W.regid=@regid
      AND ISNULL(W.status,1)=1
      AND ISNULL(W.archive,0)=0;

    SELECT
        C.id,
        ISNULL(C.SrNo,1) AS SrNo,
        ISNULL(NULLIF(C.BranchType,''),'IF') AS Type,
        ISNULL(C.SequenceNo,1) AS SequenceNo,
        ISNULL(C.FieldCode,'') AS FieldCode,
        ISNULL(C.OperatorCode,'') AS OperatorCode,
        ISNULL(C.CompareValue,'') AS CompareValue,
        ISNULL(C.CompareValue2,'') AS CompareValue2,
        ISNULL(C.LogicalOperator,'') AS LogicalOperator
    FROM dbo.Pms_tbl_WorkflowCondition C
    WHERE C.WorkflowId=@WorkflowId
      AND C.regid=@regid
      AND ISNULL(C.status,1)=1
      AND ISNULL(C.archive,0)=0
    ORDER BY ISNULL(C.SrNo,1),ISNULL(C.SequenceNo,1),C.id;

    SELECT
        A.id,
        ISNULL(A.SrNo,1) AS SrNo,
        ISNULL(NULLIF(A.BranchType,''),'IF') AS Type,
        ISNULL(A.SequenceNo,1) AS SequenceNo,
        A.TakenById,
        A.DesignationId,
        ISNULL(A.IsEmail,0) AS IsEmail,
        ISNULL(A.IsWhatsApp,0) AS IsWhatsApp,
        ISNULL(A.IsBell,0) AS IsBell
    FROM dbo.Pms_tbl_WorkflowAction A
    WHERE A.WorkflowId=@WorkflowId
      AND A.regid=@regid
      AND ISNULL(A.status,1)=1
      AND ISNULL(A.archive,0)=0
    ORDER BY ISNULL(A.SrNo,1),ISNULL(A.SequenceNo,1),A.id;
END
GO


/* ============================================================================
   10. SAVE / UPDATE WORKFLOW
       Exact parameter contract used by current WorkflowSetting.aspx.cs
   ============================================================================ */
IF OBJECT_ID('dbo.usp_Pms_Workflow_Save','P') IS NULL
    EXEC('CREATE PROCEDURE dbo.usp_Pms_Workflow_Save AS BEGIN SET NOCOUNT ON; END');
GO

ALTER PROCEDURE dbo.usp_Pms_Workflow_Save
(
    @WorkflowId INT = 0,
    @WorkflowName NVARCHAR(250),
    @ApplyOn NVARCHAR(30),
    @OrderNo INT = 1,
    @Description NVARCHAR(1000) = NULL,
    @IsActive BIT = 1,
    @regid INT,
    @uid INT = NULL,
    @ConditionsXml XML = NULL,
    @ActionsXml XML = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        SET @WorkflowName=LTRIM(RTRIM(ISNULL(@WorkflowName,'')));
        SET @ApplyOn=UPPER(LTRIM(RTRIM(ISNULL(@ApplyOn,''))));
        IF ISNULL(@OrderNo,0)<=0 SET @OrderNo=1;

        IF @WorkflowName=''
            RAISERROR('Workflow Name is required.',16,1);

        IF @ApplyOn NOT IN ('PROJECT','MILESTONE','TASK','SUBTASK')
            RAISERROR('Invalid Apply On value.',16,1);

        IF ISNULL(@WorkflowId,0)<=0
        BEGIN
            INSERT INTO dbo.Pms_tbl_Workflow
            (
                WorkflowName,ApplyOn,OrderNo,Description,IsActive,
                regid,uid,status,archive,cdate,modifieddate
            )
            VALUES
            (
                @WorkflowName,@ApplyOn,@OrderNo,NULLIF(@Description,''),
                @IsActive,@regid,@uid,1,0,GETDATE(),NULL
            );

            SET @WorkflowId=SCOPE_IDENTITY();
        END
        ELSE
        BEGIN
            IF NOT EXISTS
            (
                SELECT 1
                FROM dbo.Pms_tbl_Workflow
                WHERE id=@WorkflowId
                  AND regid=@regid
                  AND ISNULL(archive,0)=0
            )
                RAISERROR('Workflow not found.',16,1);

            UPDATE dbo.Pms_tbl_Workflow
            SET WorkflowName=@WorkflowName,
                ApplyOn=@ApplyOn,
                OrderNo=@OrderNo,
                Description=NULLIF(@Description,''),
                IsActive=@IsActive,
                uid=@uid,
                status=1,
                archive=0,
                modifieddate=GETDATE()
            WHERE id=@WorkflowId
              AND regid=@regid;
        END;

        /* Replace configuration children only for this workflow. */
        DELETE FROM dbo.Pms_tbl_WorkflowCondition
        WHERE WorkflowId=@WorkflowId AND regid=@regid;

        DELETE FROM dbo.Pms_tbl_WorkflowAction
        WHERE WorkflowId=@WorkflowId AND regid=@regid;

        /* CONDITIONS */
        IF @ConditionsXml IS NOT NULL
           AND @ConditionsXml.exist('/Conditions/Condition')=1
        BEGIN
            IF EXISTS
            (
                SELECT 1
                FROM @ConditionsXml.nodes('/Conditions/Condition') T(C)
                WHERE UPPER(ISNULL(NULLIF(
                        T.C.value('string((@Type)[1])','NVARCHAR(20)'),'')
                        ,'IF')) NOT IN ('IF','ELSEIF','ELSE')
            )
                RAISERROR('Invalid condition branch type.',16,1);

            INSERT INTO dbo.Pms_tbl_WorkflowCondition
            (
                WorkflowId,SrNo,BranchType,SequenceNo,
                FieldCode,OperatorCode,CompareValue,CompareValue2,
                LogicalOperator,regid,uid,status,archive,cdate,modifieddate
            )
            SELECT
                @WorkflowId,
                ISNULL(TRY_CONVERT(INT,NULLIF(
                    T.C.value('string((@SrNo)[1])','NVARCHAR(20)'),'')
                ),1),
                UPPER(ISNULL(NULLIF(
                    T.C.value('string((@Type)[1])','NVARCHAR(20)'),'')
                ,'IF')),
                ISNULL(TRY_CONVERT(INT,NULLIF(
                    T.C.value('string((@SequenceNo)[1])','NVARCHAR(20)'),'')
                ),1),
                NULLIF(T.C.value('string((@FieldCode)[1])','NVARCHAR(100)'),''),
                NULLIF(T.C.value('string((@OperatorCode)[1])','NVARCHAR(50)'),''),
                NULLIF(T.C.value('string((@CompareValue)[1])','NVARCHAR(1000)'),''),
                NULLIF(T.C.value('string((@CompareValue2)[1])','NVARCHAR(1000)'),''),
                NULLIF(T.C.value('string((@LogicalOperator)[1])','NVARCHAR(10)'),''),
                @regid,@uid,1,0,GETDATE(),NULL
            FROM @ConditionsXml.nodes('/Conditions/Condition') T(C);
        END;

        /* THEN ACTIONS
           TakenById + DesignationId are stored here directly. */
        IF @ActionsXml IS NOT NULL
           AND @ActionsXml.exist('/Actions/Action')=1
        BEGIN
            INSERT INTO dbo.Pms_tbl_WorkflowAction
            (
                WorkflowId,SrNo,BranchType,SequenceNo,
                TakenById,DesignationId,
                IsEmail,IsWhatsApp,IsBell,
                regid,uid,status,archive,cdate,modifieddate
            )
            SELECT
                @WorkflowId,
                ISNULL(TRY_CONVERT(INT,NULLIF(
                    T.C.value('string((@SrNo)[1])','NVARCHAR(20)'),'')
                ),1),
                UPPER(ISNULL(NULLIF(
                    T.C.value('string((@Type)[1])','NVARCHAR(20)'),'')
                ,'IF')),
                ISNULL(TRY_CONVERT(INT,NULLIF(
                    T.C.value('string((@SequenceNo)[1])','NVARCHAR(20)'),'')
                ),1),
                TRY_CONVERT(INT,NULLIF(
                    T.C.value('string((@TakenById)[1])','NVARCHAR(30)'),'')
                ),
                TRY_CONVERT(INT,NULLIF(
                    T.C.value('string((@DesignationId)[1])','NVARCHAR(30)'),'')
                ),
                CASE WHEN LOWER(ISNULL(NULLIF(
                    T.C.value('string((@IsEmail)[1])','NVARCHAR(10)'),'')
                ,'0')) IN ('1','true') THEN 1 ELSE 0 END,
                CASE WHEN LOWER(ISNULL(NULLIF(
                    T.C.value('string((@IsWhatsApp)[1])','NVARCHAR(10)'),'')
                ,'0')) IN ('1','true') THEN 1 ELSE 0 END,
                CASE WHEN LOWER(ISNULL(NULLIF(
                    T.C.value('string((@IsBell)[1])','NVARCHAR(10)'),'')
                ,'0')) IN ('1','true') THEN 1 ELSE 0 END,
                @regid,@uid,1,0,GETDATE(),NULL
            FROM @ActionsXml.nodes('/Actions/Action') T(C);
        END;

        COMMIT TRANSACTION;

        SELECT
            1 AS Result,
            @WorkflowId AS WorkflowId,
            'Workflow saved successfully.' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT>0 ROLLBACK TRANSACTION;

        DECLARE @Err NVARCHAR(4000)=ERROR_MESSAGE();
        RAISERROR(@Err,16,1);
    END CATCH
END
GO


/* ============================================================================
   11. DELETE WORKFLOW
   ============================================================================ */
IF OBJECT_ID('dbo.usp_Pms_Workflow_Delete','P') IS NULL
    EXEC('CREATE PROCEDURE dbo.usp_Pms_Workflow_Delete AS BEGIN SET NOCOUNT ON; END');
GO

ALTER PROCEDURE dbo.usp_Pms_Workflow_Delete
(
    @WorkflowId INT,
    @regid INT,
    @uid INT = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE dbo.Pms_tbl_Workflow
        SET status=0,
            archive=1,
            IsActive=0,
            uid=@uid,
            modifieddate=GETDATE()
        WHERE id=@WorkflowId
          AND regid=@regid
          AND ISNULL(archive,0)=0;

        UPDATE dbo.Pms_tbl_WorkflowCondition
        SET status=0,archive=1,uid=@uid,modifieddate=GETDATE()
        WHERE WorkflowId=@WorkflowId AND regid=@regid;

        UPDATE dbo.Pms_tbl_WorkflowAction
        SET status=0,archive=1,uid=@uid,modifieddate=GETDATE()
        WHERE WorkflowId=@WorkflowId AND regid=@regid;

        /* Also clean current optional child configuration if available. */
        IF OBJECT_ID('dbo.Pms_tbl_WorkflowUserAction','U') IS NOT NULL
            UPDATE dbo.Pms_tbl_WorkflowUserAction
            SET status=0,archive=1,uid=@uid,modifieddate=GETDATE()
            WHERE WorkflowId=@WorkflowId AND regid=@regid;

        IF OBJECT_ID('dbo.Pms_tbl_WorkflowThenAction','U') IS NOT NULL
            UPDATE dbo.Pms_tbl_WorkflowThenAction
            SET status=0,archive=1,uid=@uid,modifieddate=GETDATE()
            WHERE WorkflowId=@WorkflowId AND regid=@regid;

        COMMIT TRANSACTION;

        SELECT 1 AS Result,@WorkflowId AS WorkflowId,
               'Workflow deleted successfully.' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT>0 ROLLBACK TRANSACTION;
        DECLARE @Err NVARCHAR(4000)=ERROR_MESSAGE();
        RAISERROR(@Err,16,1);
    END CATCH
END
GO


/* ============================================================================
   12. ACTIVE / INACTIVE
   ============================================================================ */
IF OBJECT_ID('dbo.usp_Pms_Workflow_ChangeStatus','P') IS NULL
    EXEC('CREATE PROCEDURE dbo.usp_Pms_Workflow_ChangeStatus AS BEGIN SET NOCOUNT ON; END');
GO

ALTER PROCEDURE dbo.usp_Pms_Workflow_ChangeStatus
(
    @WorkflowId INT,
    @IsActive BIT,
    @regid INT,
    @uid INT = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Pms_tbl_Workflow
    SET IsActive=@IsActive,
        uid=@uid,
        modifieddate=GETDATE()
    WHERE id=@WorkflowId
      AND regid=@regid
      AND ISNULL(status,1)=1
      AND ISNULL(archive,0)=0;

    IF @@ROWCOUNT=0
        RAISERROR('Workflow not found.',16,1);

    SELECT 1 AS Result,@WorkflowId AS WorkflowId,
           @IsActive AS IsActive,'Workflow status updated.' AS Message;
END
GO


/* ============================================================================
   13. READ ACTION CONDITIONS / USER ACTIONS
   ============================================================================ */
IF OBJECT_ID('dbo.usp_Pms_WorkflowUserAction_GetByWorkflow','P') IS NULL
    EXEC('CREATE PROCEDURE dbo.usp_Pms_WorkflowUserAction_GetByWorkflow AS BEGIN SET NOCOUNT ON; END');
GO

ALTER PROCEDURE dbo.usp_Pms_WorkflowUserAction_GetByWorkflow
(
    @WorkflowId INT,
    @regid INT
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        UA.id,
        UA.WorkflowId,
        ISNULL(UA.SrNo,1) AS SrNo,
        ISNULL(NULLIF(UA.BranchType,''),'IF') AS Type,
        ISNULL(UA.SequenceNo,1) AS ActionSrNo,
        ISNULL(
            NULLIF(UA.ActionBranchType,''),
            CASE WHEN ISNULL(UA.SequenceNo,1)=1 THEN 'IF' ELSE 'ELSEIF' END
        ) AS ActionType,
        UA.ActionId,
        UA.NextStatusId,
        UA.TakenById,
        UA.DesignationId,
        ISNULL(UA.IsEmail,0) AS IsEmail,
        ISNULL(UA.IsWhatsApp,0) AS IsWhatsApp,
        ISNULL(UA.IsBell,0) AS IsBell,
        ISNULL(UA.IsRemarkRequired,0) AS IsRemarkRequired
    FROM dbo.Pms_tbl_WorkflowUserAction UA
    WHERE UA.WorkflowId=@WorkflowId
      AND UA.regid=@regid
      AND ISNULL(UA.status,1)=1
      AND ISNULL(UA.archive,0)=0
    ORDER BY ISNULL(UA.SrNo,1),ISNULL(UA.SequenceNo,1),UA.id;
END
GO


/* ============================================================================
   14. SAVE ACTION CONDITIONS / USER ACTIONS

   IMPORTANT:
       ActionId      -> dbo.Pms_tbl_SimpleMasterList.id
       NextStatusId  -> dbo.Pms_tbl_Projectstatus.id
   ============================================================================ */
IF OBJECT_ID('dbo.usp_Pms_WorkflowUserAction_Save','P') IS NULL
    EXEC('CREATE PROCEDURE dbo.usp_Pms_WorkflowUserAction_Save AS BEGIN SET NOCOUNT ON; END');
GO

ALTER PROCEDURE dbo.usp_Pms_WorkflowUserAction_Save
(
    @WorkflowId INT,
    @regid INT,
    @uid INT = NULL,
    @UserActionsXml XML = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Pms_tbl_Workflow
            WHERE id=@WorkflowId
              AND regid=@regid
              AND ISNULL(archive,0)=0
        )
            RAISERROR('Workflow not found for Action Conditions.',16,1);

        DELETE FROM dbo.Pms_tbl_WorkflowUserAction
        WHERE WorkflowId=@WorkflowId
          AND regid=@regid;

        IF @UserActionsXml IS NOT NULL
           AND @UserActionsXml.exist('/UserActions/UserAction')=1
        BEGIN
            /* Parent branch and nested Action branch must be valid. */
            IF EXISTS
            (
                SELECT 1
                FROM @UserActionsXml.nodes('/UserActions/UserAction') T(C)
                WHERE UPPER(ISNULL(NULLIF(
                          T.C.value('string((@Type)[1])','NVARCHAR(20)'),'')
                      ,'IF')) NOT IN ('IF','ELSEIF','ELSE')
                   OR UPPER(ISNULL(NULLIF(
                          T.C.value('string((@ActionType)[1])','NVARCHAR(20)'),'')
                      ,'IF')) NOT IN ('IF','ELSEIF','ELSE')
            )
                RAISERROR('Invalid IF / ELSE IF / ELSE type in Action Conditions.',16,1);

            /* First nested Action of every outer branch must be IF. */
            IF EXISTS
            (
                SELECT 1
                FROM
                (
                    SELECT
                        ISNULL(TRY_CONVERT(INT,NULLIF(
                            T.C.value('string((@SrNo)[1])','NVARCHAR(20)'),'')
                        ),1) AS SrNo,
                        ISNULL(TRY_CONVERT(INT,NULLIF(
                            T.C.value('string((@SequenceNo)[1])','NVARCHAR(20)'),'')
                        ),1) AS SequenceNo,
                        UPPER(ISNULL(NULLIF(
                            T.C.value('string((@ActionType)[1])','NVARCHAR(20)'),'')
                        ,'IF')) AS ActionType
                    FROM @UserActionsXml.nodes('/UserActions/UserAction') T(C)
                ) X
                WHERE X.SequenceNo=1
                  AND X.ActionType<>'IF'
            )
                RAISERROR('First Action Condition must be IF.',16,1);

            /* QC ACTION: correct source is Pms_tbl_SimpleMasterList. */
            IF OBJECT_ID('dbo.Pms_tbl_SimpleMasterList','U') IS NULL
                RAISERROR('QC Action master dbo.Pms_tbl_SimpleMasterList is missing.',16,1);

            IF EXISTS
            (
                SELECT 1
                FROM @UserActionsXml.nodes('/UserActions/UserAction') T(C)
                WHERE UPPER(ISNULL(NULLIF(
                          T.C.value('string((@ActionType)[1])','NVARCHAR(20)'),'')
                      ,'IF')) <> 'ELSE'
                  AND
                  (
                      TRY_CONVERT(INT,NULLIF(
                          T.C.value('string((@ActionId)[1])','NVARCHAR(30)'),'')
                      ) IS NULL
                      OR NOT EXISTS
                      (
                          SELECT 1
                          FROM dbo.Pms_tbl_SimpleMasterList SM
                          WHERE SM.id=TRY_CONVERT(INT,NULLIF(
                                    T.C.value('string((@ActionId)[1])','NVARCHAR(30)'),'')
                                )
                            AND SM.regid=@regid
                            AND ISNULL(SM.status,1)=1
                            AND ISNULL(SM.archive,0)=0
                      )
                  )
            )
                RAISERROR('Invalid QC Action selected in Action Conditions.',16,1);

            /* NEXT STATUS: correct source is Pms_tbl_Projectstatus. */
            IF OBJECT_ID('dbo.Pms_tbl_Projectstatus','U') IS NULL
                RAISERROR('PMS Status master dbo.Pms_tbl_Projectstatus is missing.',16,1);

            IF EXISTS
            (
                SELECT 1
                FROM @UserActionsXml.nodes('/UserActions/UserAction') T(C)
                WHERE TRY_CONVERT(INT,NULLIF(
                          T.C.value('string((@NextStatusId)[1])','NVARCHAR(30)'),'')
                      ) IS NOT NULL
                  AND NOT EXISTS
                  (
                      SELECT 1
                      FROM dbo.Pms_tbl_Projectstatus PS
                      WHERE PS.id=TRY_CONVERT(INT,NULLIF(
                                T.C.value('string((@NextStatusId)[1])','NVARCHAR(30)'),'')
                            )
                        AND PS.regid=@regid
                        AND ISNULL(PS.status,1)=1
                        AND ISNULL(PS.archive,0)=0
                  )
            )
                RAISERROR('Invalid Next Status selected in Action Conditions.',16,1);

            /* Only one ELSE for each outer workflow branch. */
            IF EXISTS
            (
                SELECT X.SrNo
                FROM
                (
                    SELECT
                        ISNULL(TRY_CONVERT(INT,NULLIF(
                            T.C.value('string((@SrNo)[1])','NVARCHAR(20)'),'')
                        ),1) AS SrNo,
                        UPPER(ISNULL(NULLIF(
                            T.C.value('string((@ActionType)[1])','NVARCHAR(20)'),'')
                        ,'IF')) AS ActionType
                    FROM @UserActionsXml.nodes('/UserActions/UserAction') T(C)
                ) X
                WHERE X.ActionType='ELSE'
                GROUP BY X.SrNo
                HAVING COUNT(*)>1
            )
                RAISERROR('Only one ELSE is allowed in Action Conditions for each workflow branch.',16,1);

            /* ELSE must be last. */
            IF EXISTS
            (
                SELECT 1
                FROM
                (
                    SELECT
                        ISNULL(TRY_CONVERT(INT,NULLIF(
                            T.C.value('string((@SrNo)[1])','NVARCHAR(20)'),'')
                        ),1) AS SrNo,
                        ISNULL(TRY_CONVERT(INT,NULLIF(
                            T.C.value('string((@SequenceNo)[1])','NVARCHAR(20)'),'')
                        ),1) AS SequenceNo,
                        UPPER(ISNULL(NULLIF(
                            T.C.value('string((@ActionType)[1])','NVARCHAR(20)'),'')
                        ,'IF')) AS ActionType
                    FROM @UserActionsXml.nodes('/UserActions/UserAction') T(C)
                ) E
                WHERE E.ActionType='ELSE'
                  AND EXISTS
                  (
                      SELECT 1
                      FROM
                      (
                          SELECT
                              ISNULL(TRY_CONVERT(INT,NULLIF(
                                  T2.C.value('string((@SrNo)[1])','NVARCHAR(20)'),'')
                              ),1) AS SrNo,
                              ISNULL(TRY_CONVERT(INT,NULLIF(
                                  T2.C.value('string((@SequenceNo)[1])','NVARCHAR(20)'),'')
                              ),1) AS SequenceNo
                          FROM @UserActionsXml.nodes('/UserActions/UserAction') T2(C)
                      ) N
                      WHERE N.SrNo=E.SrNo
                        AND N.SequenceNo>E.SequenceNo
                  )
            )
                RAISERROR('Action Conditions ELSE must be the last item.',16,1);

            INSERT INTO dbo.Pms_tbl_WorkflowUserAction
            (
                WorkflowId,SrNo,BranchType,SequenceNo,ActionBranchType,
                ActionId,NextStatusId,TakenById,DesignationId,
                IsEmail,IsWhatsApp,IsBell,IsRemarkRequired,
                regid,uid,status,archive,cdate,modifieddate
            )
            SELECT
                @WorkflowId,
                ISNULL(TRY_CONVERT(INT,NULLIF(
                    T.C.value('string((@SrNo)[1])','NVARCHAR(20)'),'')
                ),1),
                UPPER(ISNULL(NULLIF(
                    T.C.value('string((@Type)[1])','NVARCHAR(20)'),'')
                ,'IF')),
                ISNULL(TRY_CONVERT(INT,NULLIF(
                    T.C.value('string((@SequenceNo)[1])','NVARCHAR(20)'),'')
                ),1),
                UPPER(ISNULL(NULLIF(
                    T.C.value('string((@ActionType)[1])','NVARCHAR(20)'),'')
                ,'IF')),
                CASE
                    WHEN UPPER(ISNULL(NULLIF(
                        T.C.value('string((@ActionType)[1])','NVARCHAR(20)'),'')
                    ,'IF'))='ELSE'
                    THEN NULL
                    ELSE TRY_CONVERT(INT,NULLIF(
                        T.C.value('string((@ActionId)[1])','NVARCHAR(30)'),'')
                    )
                END,
                TRY_CONVERT(INT,NULLIF(
                    T.C.value('string((@NextStatusId)[1])','NVARCHAR(30)'),'')
                ),
                TRY_CONVERT(INT,NULLIF(
                    T.C.value('string((@TakenById)[1])','NVARCHAR(30)'),'')
                ),
                TRY_CONVERT(INT,NULLIF(
                    T.C.value('string((@DesignationId)[1])','NVARCHAR(30)'),'')
                ),
                CASE WHEN LOWER(ISNULL(NULLIF(
                    T.C.value('string((@IsEmail)[1])','NVARCHAR(10)'),'')
                ,'0')) IN ('1','true') THEN 1 ELSE 0 END,
                CASE WHEN LOWER(ISNULL(NULLIF(
                    T.C.value('string((@IsWhatsApp)[1])','NVARCHAR(10)'),'')
                ,'0')) IN ('1','true') THEN 1 ELSE 0 END,
                CASE WHEN LOWER(ISNULL(NULLIF(
                    T.C.value('string((@IsBell)[1])','NVARCHAR(10)'),'')
                ,'0')) IN ('1','true') THEN 1 ELSE 0 END,
                CASE WHEN LOWER(ISNULL(NULLIF(
                    T.C.value('string((@IsRemarkRequired)[1])','NVARCHAR(10)'),'')
                ,'0')) IN ('1','true') THEN 1 ELSE 0 END,
                @regid,@uid,1,0,GETDATE(),NULL
            FROM @UserActionsXml.nodes('/UserActions/UserAction') T(C);
        END;

        COMMIT TRANSACTION;

        SELECT
            1 AS Result,
            @WorkflowId AS WorkflowId,
            (
                SELECT COUNT(1)
                FROM dbo.Pms_tbl_WorkflowUserAction
                WHERE WorkflowId=@WorkflowId
                  AND regid=@regid
                  AND ISNULL(status,1)=1
                  AND ISNULL(archive,0)=0
            ) AS SavedRows,
            'Action Conditions saved successfully.' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT>0 ROLLBACK TRANSACTION;
        DECLARE @Err NVARCHAR(4000)=ERROR_MESSAGE();
        RAISERROR(@Err,16,1);
    END CATCH
END
GO


/* ============================================================================
   15. DELETE USER ACTION CONFIGURATION
   ============================================================================ */
IF OBJECT_ID('dbo.usp_Pms_WorkflowUserAction_DeleteByWorkflow','P') IS NULL
    EXEC('CREATE PROCEDURE dbo.usp_Pms_WorkflowUserAction_DeleteByWorkflow AS BEGIN SET NOCOUNT ON; END');
GO

ALTER PROCEDURE dbo.usp_Pms_WorkflowUserAction_DeleteByWorkflow
(
    @WorkflowId INT,
    @regid INT,
    @uid INT = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Pms_tbl_WorkflowUserAction
    SET status=0,
        archive=1,
        uid=@uid,
        modifieddate=GETDATE()
    WHERE WorkflowId=@WorkflowId
      AND regid=@regid
      AND ISNULL(archive,0)=0;

    SELECT 1 AS Result,'Workflow Action Conditions removed.' AS Message;
END
GO


/* ============================================================================
   16. DEDICATED THEN ACTION SAVE - FOR COMPATIBILITY PATCH
   ============================================================================ */
IF OBJECT_ID('dbo.usp_Pms_WorkflowThenAction_Save','P') IS NULL
    EXEC('CREATE PROCEDURE dbo.usp_Pms_WorkflowThenAction_Save AS BEGIN SET NOCOUNT ON; END');
GO

ALTER PROCEDURE dbo.usp_Pms_WorkflowThenAction_Save
(
    @WorkflowId INT,
    @regid INT,
    @uid INT = NULL,
    @ActionsXml XML = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Pms_tbl_Workflow
            WHERE id=@WorkflowId
              AND regid=@regid
              AND ISNULL(archive,0)=0
        )
            RAISERROR('Workflow not found for THEN Action.',16,1);

        DELETE FROM dbo.Pms_tbl_WorkflowThenAction
        WHERE WorkflowId=@WorkflowId
          AND regid=@regid;

        IF @ActionsXml IS NOT NULL
           AND @ActionsXml.exist('/Actions/Action')=1
        BEGIN
            INSERT INTO dbo.Pms_tbl_WorkflowThenAction
            (
                WorkflowId,SrNo,BranchType,SequenceNo,
                TakenById,DesignationId,IsEmail,IsWhatsApp,IsBell,
                regid,uid,status,archive,cdate,modifieddate
            )
            SELECT
                @WorkflowId,
                ISNULL(TRY_CONVERT(INT,NULLIF(
                    T.C.value('string((@SrNo)[1])','NVARCHAR(20)'),'')
                ),1),
                UPPER(ISNULL(NULLIF(
                    T.C.value('string((@Type)[1])','NVARCHAR(20)'),'')
                ,'IF')),
                ISNULL(TRY_CONVERT(INT,NULLIF(
                    T.C.value('string((@SequenceNo)[1])','NVARCHAR(20)'),'')
                ),1),
                TRY_CONVERT(INT,NULLIF(
                    T.C.value('string((@TakenById)[1])','NVARCHAR(30)'),'')
                ),
                TRY_CONVERT(INT,NULLIF(
                    T.C.value('string((@DesignationId)[1])','NVARCHAR(30)'),'')
                ),
                CASE WHEN LOWER(ISNULL(NULLIF(
                    T.C.value('string((@IsEmail)[1])','NVARCHAR(10)'),'')
                ,'0')) IN ('1','true') THEN 1 ELSE 0 END,
                CASE WHEN LOWER(ISNULL(NULLIF(
                    T.C.value('string((@IsWhatsApp)[1])','NVARCHAR(10)'),'')
                ,'0')) IN ('1','true') THEN 1 ELSE 0 END,
                CASE WHEN LOWER(ISNULL(NULLIF(
                    T.C.value('string((@IsBell)[1])','NVARCHAR(10)'),'')
                ,'0')) IN ('1','true') THEN 1 ELSE 0 END,
                @regid,@uid,1,0,GETDATE(),NULL
            FROM @ActionsXml.nodes('/Actions/Action') T(C);
        END;

        COMMIT TRANSACTION;

        SELECT
            1 AS Result,
            @WorkflowId AS WorkflowId,
            (
                SELECT COUNT(1)
                FROM dbo.Pms_tbl_WorkflowThenAction
                WHERE WorkflowId=@WorkflowId
                  AND regid=@regid
                  AND ISNULL(status,1)=1
                  AND ISNULL(archive,0)=0
            ) AS SavedRows,
            'THEN Action saved successfully.' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT>0 ROLLBACK TRANSACTION;

        SELECT 0 AS Result,@WorkflowId AS WorkflowId,
               0 AS SavedRows,ERROR_MESSAGE() AS Message;
    END CATCH
END
GO


/* ============================================================================
   17. READ DEDICATED THEN ACTION
   ============================================================================ */
IF OBJECT_ID('dbo.usp_Pms_WorkflowThenAction_GetByWorkflow','P') IS NULL
    EXEC('CREATE PROCEDURE dbo.usp_Pms_WorkflowThenAction_GetByWorkflow AS BEGIN SET NOCOUNT ON; END');
GO

ALTER PROCEDURE dbo.usp_Pms_WorkflowThenAction_GetByWorkflow
(
    @WorkflowId INT,
    @regid INT
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        TA.id,
        TA.WorkflowId,
        ISNULL(TA.SrNo,1) AS SrNo,
        ISNULL(NULLIF(TA.BranchType,''),'IF') AS Type,
        ISNULL(TA.SequenceNo,1) AS SequenceNo,
        TA.TakenById,
        TA.DesignationId,
        ISNULL(TA.IsEmail,0) AS IsEmail,
        ISNULL(TA.IsWhatsApp,0) AS IsWhatsApp,
        ISNULL(TA.IsBell,0) AS IsBell
    FROM dbo.Pms_tbl_WorkflowThenAction TA
    WHERE TA.WorkflowId=@WorkflowId
      AND TA.regid=@regid
      AND ISNULL(TA.status,1)=1
      AND ISNULL(TA.archive,0)=0
    ORDER BY ISNULL(TA.SrNo,1),ISNULL(TA.SequenceNo,1),TA.id;
END
GO


/* ============================================================================
   18. DELETE DEDICATED THEN ACTION
   ============================================================================ */
IF OBJECT_ID('dbo.usp_Pms_WorkflowThenAction_DeleteByWorkflow','P') IS NULL
    EXEC('CREATE PROCEDURE dbo.usp_Pms_WorkflowThenAction_DeleteByWorkflow AS BEGIN SET NOCOUNT ON; END');
GO

ALTER PROCEDURE dbo.usp_Pms_WorkflowThenAction_DeleteByWorkflow
(
    @WorkflowId INT,
    @regid INT,
    @uid INT = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Pms_tbl_WorkflowThenAction
    SET status=0,
        archive=1,
        uid=@uid,
        modifieddate=GETDATE()
    WHERE WorkflowId=@WorkflowId
      AND regid=@regid
      AND ISNULL(archive,0)=0;

    SELECT 1 AS Result,'THEN Action removed.' AS Message;
END
GO


/* ============================================================================
   19. FINAL SERVER VERIFICATION
   ============================================================================ */
PRINT '------------------------------------------------------------';
PRINT 'PMS QC Workflow deployment finished.';
PRINT 'Check the result set below. MissingObjectCount should be 0.';
PRINT '------------------------------------------------------------';

DECLARE @Missing INT=0;

;WITH RequiredObjects AS
(
    SELECT 'TABLE' AS ObjectType,'dbo.Pms_tbl_Workflow' AS ObjectName,'U' AS ObjType UNION ALL
    SELECT 'TABLE','dbo.Pms_tbl_WorkflowCondition','U' UNION ALL
    SELECT 'TABLE','dbo.Pms_tbl_WorkflowAction','U' UNION ALL
    SELECT 'TABLE','dbo.Pms_tbl_WorkflowUserAction','U' UNION ALL
    SELECT 'TABLE','dbo.Pms_tbl_WorkflowThenAction','U' UNION ALL

    SELECT 'PROC','dbo.usp_Pms_Workflow_GetMasters','P' UNION ALL
    SELECT 'PROC','dbo.usp_Pms_Workflow_List','P' UNION ALL
    SELECT 'PROC','dbo.usp_Pms_Workflow_GetById','P' UNION ALL
    SELECT 'PROC','dbo.usp_Pms_Workflow_Save','P' UNION ALL
    SELECT 'PROC','dbo.usp_Pms_Workflow_Delete','P' UNION ALL
    SELECT 'PROC','dbo.usp_Pms_Workflow_ChangeStatus','P' UNION ALL
    SELECT 'PROC','dbo.usp_Pms_WorkflowUserAction_GetByWorkflow','P' UNION ALL
    SELECT 'PROC','dbo.usp_Pms_WorkflowUserAction_Save','P' UNION ALL
    SELECT 'PROC','dbo.usp_Pms_WorkflowUserAction_DeleteByWorkflow','P' UNION ALL
    SELECT 'PROC','dbo.usp_Pms_WorkflowThenAction_Save','P' UNION ALL
    SELECT 'PROC','dbo.usp_Pms_WorkflowThenAction_GetByWorkflow','P' UNION ALL
    SELECT 'PROC','dbo.usp_Pms_WorkflowThenAction_DeleteByWorkflow','P'
)
SELECT
    ObjectType,
    ObjectName,
    CASE WHEN OBJECT_ID(ObjectName,ObjType) IS NULL THEN 'MISSING' ELSE 'OK' END AS [Status]
FROM RequiredObjects
ORDER BY ObjectType,ObjectName;

SELECT @Missing=COUNT(1)
FROM
(
    SELECT 'dbo.Pms_tbl_Workflow' N,'U' T UNION ALL
    SELECT 'dbo.Pms_tbl_WorkflowCondition','U' UNION ALL
    SELECT 'dbo.Pms_tbl_WorkflowAction','U' UNION ALL
    SELECT 'dbo.Pms_tbl_WorkflowUserAction','U' UNION ALL
    SELECT 'dbo.Pms_tbl_WorkflowThenAction','U' UNION ALL
    SELECT 'dbo.usp_Pms_Workflow_GetMasters','P' UNION ALL
    SELECT 'dbo.usp_Pms_Workflow_List','P' UNION ALL
    SELECT 'dbo.usp_Pms_Workflow_GetById','P' UNION ALL
    SELECT 'dbo.usp_Pms_Workflow_Save','P' UNION ALL
    SELECT 'dbo.usp_Pms_Workflow_Delete','P' UNION ALL
    SELECT 'dbo.usp_Pms_Workflow_ChangeStatus','P' UNION ALL
    SELECT 'dbo.usp_Pms_WorkflowUserAction_GetByWorkflow','P' UNION ALL
    SELECT 'dbo.usp_Pms_WorkflowUserAction_Save','P' UNION ALL
    SELECT 'dbo.usp_Pms_WorkflowUserAction_DeleteByWorkflow','P' UNION ALL
    SELECT 'dbo.usp_Pms_WorkflowThenAction_Save','P' UNION ALL
    SELECT 'dbo.usp_Pms_WorkflowThenAction_GetByWorkflow','P' UNION ALL
    SELECT 'dbo.usp_Pms_WorkflowThenAction_DeleteByWorkflow','P'
) Q
WHERE OBJECT_ID(Q.N,Q.T) IS NULL;

SELECT @Missing AS MissingObjectCount;
GO

/*
===============================================================================
SERVER TEST AFTER DEPLOYMENT
===============================================================================

-- 1. Check workflow list for one company:
-- EXEC dbo.usp_Pms_Workflow_List @regid = YOUR_REGID;

-- 2. Check masters:
-- EXEC dbo.usp_Pms_Workflow_GetMasters @regid = YOUR_REGID;

-- 3. Check one workflow:
-- EXEC dbo.usp_Pms_Workflow_GetById
--      @WorkflowId = YOUR_WORKFLOW_ID,
--      @regid = YOUR_REGID;

-- 4. Check User Action rows:
-- SELECT *
-- FROM dbo.Pms_tbl_WorkflowUserAction
-- WHERE WorkflowId=YOUR_WORKFLOW_ID AND regid=YOUR_REGID
-- ORDER BY SrNo,SequenceNo,id;

-- 5. Confirm correct QC Action master:
-- SELECT id,MasterName,regid,status,archive
-- FROM dbo.Pms_tbl_SimpleMasterList
-- WHERE regid=YOUR_REGID;

-- 6. Confirm PMS Status master:
-- SELECT id,ProjectStatus,regid,status,archive
-- FROM dbo.Pms_tbl_Projectstatus
-- WHERE regid=YOUR_REGID;

===============================================================================
*/
