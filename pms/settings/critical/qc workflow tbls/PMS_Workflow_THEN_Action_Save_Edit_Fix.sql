/*
=====================================================================
PMS QC WORKFLOW - THEN ACTION DEPARTMENT / DESIGNATION SAVE + EDIT FIX
=====================================================================

SCOPE
-----
Only the "THEN Action" row is persisted here:
  - SrNo / IF-ELSE branch
  - Take By (Department)
  - Designation
  - Email
  - WhatsApp
  - Bell

IMPORTANT
---------
This script DOES NOT alter:
  dbo.usp_Pms_Workflow_Save
  dbo.usp_Pms_Workflow_GetById
  Conditions
  Action Conditions / User Actions
  QC Action / Next Status
  Existing HTML view

A dedicated table is used so existing workflow working is not disturbed.
=====================================================================
*/

SET NOCOUNT ON;
GO

/* ---------------------------------------------------------------
   1. Dedicated THEN Action table
   --------------------------------------------------------------- */
IF OBJECT_ID('dbo.Pms_tbl_WorkflowThenAction','U') IS NULL
BEGIN
    CREATE TABLE dbo.Pms_tbl_WorkflowThenAction
    (
        id              INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        WorkflowId      INT NOT NULL,
        SrNo            INT NULL,
        BranchType      NVARCHAR(20) NULL,
        SequenceNo      INT NULL,
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

/* Safe column additions if an earlier test version of the table exists. */
IF COL_LENGTH('dbo.Pms_tbl_WorkflowThenAction','WorkflowId') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowThenAction ADD WorkflowId INT NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowThenAction','SrNo') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowThenAction ADD SrNo INT NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowThenAction','BranchType') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowThenAction ADD BranchType NVARCHAR(20) NULL;
GO
IF COL_LENGTH('dbo.Pms_tbl_WorkflowThenAction','SequenceNo') IS NULL
    ALTER TABLE dbo.Pms_tbl_WorkflowThenAction ADD SequenceNo INT NULL;
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

/* Useful index - created only once. */
IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE object_id = OBJECT_ID('dbo.Pms_tbl_WorkflowThenAction')
      AND name = 'IX_Pms_WorkflowThenAction_Workflow'
)
BEGIN
    CREATE INDEX IX_Pms_WorkflowThenAction_Workflow
        ON dbo.Pms_tbl_WorkflowThenAction
           (WorkflowId, regid, status, archive, SrNo, SequenceNo);
END
GO


/* ---------------------------------------------------------------
   2. SAVE / UPDATE THEN ACTION
   --------------------------------------------------------------- */
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
            WHERE id = @WorkflowId
              AND regid = @regid
              AND ISNULL(archive,0) = 0
        )
        BEGIN
            RAISERROR('Workflow not found for THEN Action.',16,1);
        END;

        /* Replace only dedicated THEN Action rows. */
        DELETE FROM dbo.Pms_tbl_WorkflowThenAction
        WHERE WorkflowId = @WorkflowId
          AND regid = @regid;

        IF @ActionsXml IS NOT NULL
           AND @ActionsXml.exist('/Actions/Action') = 1
        BEGIN
            INSERT INTO dbo.Pms_tbl_WorkflowThenAction
            (
                WorkflowId,
                SrNo,
                BranchType,
                SequenceNo,
                TakenById,
                DesignationId,
                IsEmail,
                IsWhatsApp,
                IsBell,
                regid,
                uid,
                status,
                archive,
                cdate,
                modifieddate
            )
            SELECT
                @WorkflowId,
                ISNULL(
                    TRY_CONVERT(
                        INT,
                        NULLIF(T.C.value('string((@SrNo)[1])','NVARCHAR(20)'), '')
                    ),
                    1
                ),
                UPPER(
                    ISNULL(
                        NULLIF(T.C.value('string((@Type)[1])','NVARCHAR(20)'), ''),
                        'IF'
                    )
                ),
                ISNULL(
                    TRY_CONVERT(
                        INT,
                        NULLIF(T.C.value('string((@SequenceNo)[1])','NVARCHAR(20)'), '')
                    ),
                    1
                ),
                TRY_CONVERT(
                    INT,
                    NULLIF(T.C.value('string((@TakenById)[1])','NVARCHAR(30)'), '')
                ),
                TRY_CONVERT(
                    INT,
                    NULLIF(T.C.value('string((@DesignationId)[1])','NVARCHAR(30)'), '')
                ),
                CASE
                    WHEN LOWER(
                        ISNULL(
                            NULLIF(T.C.value('string((@IsEmail)[1])','NVARCHAR(10)'), ''),
                            '0'
                        )
                    ) IN ('1','true') THEN 1 ELSE 0
                END,
                CASE
                    WHEN LOWER(
                        ISNULL(
                            NULLIF(T.C.value('string((@IsWhatsApp)[1])','NVARCHAR(10)'), ''),
                            '0'
                        )
                    ) IN ('1','true') THEN 1 ELSE 0
                END,
                CASE
                    WHEN LOWER(
                        ISNULL(
                            NULLIF(T.C.value('string((@IsBell)[1])','NVARCHAR(10)'), ''),
                            '0'
                        )
                    ) IN ('1','true') THEN 1 ELSE 0
                END,
                @regid,
                @uid,
                1,
                0,
                GETDATE(),
                NULL
            FROM @ActionsXml.nodes('/Actions/Action') T(C);
        END;

        DECLARE @ExpectedRows INT = 0;
        DECLARE @SavedRows INT = 0;

        IF @ActionsXml IS NOT NULL
            SET @ExpectedRows = @ActionsXml.value('count(/Actions/Action)','INT');

        SELECT @SavedRows = COUNT(1)
        FROM dbo.Pms_tbl_WorkflowThenAction
        WHERE WorkflowId = @WorkflowId
          AND regid = @regid
          AND ISNULL(status,1) = 1
          AND ISNULL(archive,0) = 0;

        IF @SavedRows <> @ExpectedRows
        BEGIN
            RAISERROR(
                'THEN Action count mismatch. Expected %d row(s), saved %d row(s).',
                16,
                1,
                @ExpectedRows,
                @SavedRows
            );
        END;

        COMMIT TRANSACTION;

        SELECT
            1 AS Result,
            @WorkflowId AS WorkflowId,
            @SavedRows AS SavedRows,
            'THEN Action saved successfully.' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SELECT
            0 AS Result,
            @WorkflowId AS WorkflowId,
            0 AS SavedRows,
            ERROR_MESSAGE() AS Message;
    END CATCH
END
GO


/* ---------------------------------------------------------------
   3. LOAD THEN ACTION ON EDIT
   --------------------------------------------------------------- */
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
    WHERE TA.WorkflowId = @WorkflowId
      AND TA.regid = @regid
      AND ISNULL(TA.status,1) = 1
      AND ISNULL(TA.archive,0) = 0
    ORDER BY ISNULL(TA.SrNo,1),
             ISNULL(TA.SequenceNo,1),
             TA.id;
END
GO


/* ---------------------------------------------------------------
   4. DELETE WHEN WORKFLOW IS DELETED
   --------------------------------------------------------------- */
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
    SET status = 0,
        archive = 1,
        uid = @uid,
        modifieddate = GETDATE()
    WHERE WorkflowId = @WorkflowId
      AND regid = @regid
      AND ISNULL(archive,0) = 0;

    SELECT 1 AS Result, 'THEN Action removed.' AS Message;
END
GO


/* ---------------------------------------------------------------
   TEST AFTER SAVE:
   Replace IDs with actual values.
   ---------------------------------------------------------------

SELECT
    WorkflowId,
    SrNo,
    BranchType,
    SequenceNo,
    TakenById,
    DesignationId,
    IsEmail,
    IsWhatsApp,
    IsBell
FROM dbo.Pms_tbl_WorkflowThenAction
WHERE WorkflowId = YOUR_WORKFLOW_ID
  AND regid = YOUR_REGID
ORDER BY SrNo, SequenceNo, id;

*/
