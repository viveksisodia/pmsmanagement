/* ============================================================
   PMS QC WORKFLOW - USER ACTION FINAL FIX

   IMPORTANT MASTER MAPPING
   ------------------------
   QC Action     -> dbo.Pms_tbl_SimpleMasterList
   Next Status   -> dbo.Pms_tbl_Projectstatus

   This script changes ONLY the User Action / Action Conditions
   save + edit procedures. Existing Workflow, Conditions and
   THEN Action logic is not changed.
   ============================================================ */

SET NOCOUNT ON;
GO

/* ------------------------------------------------------------
   Ensure NextStatusId exists (safe for existing installation).
   ------------------------------------------------------------ */
IF COL_LENGTH('dbo.Pms_tbl_WorkflowUserAction','NextStatusId') IS NULL
BEGIN
    ALTER TABLE dbo.Pms_tbl_WorkflowUserAction
    ADD NextStatusId INT NULL;
END
GO

/* ELSE is a default route, therefore ActionId may be NULL. */
IF COL_LENGTH('dbo.Pms_tbl_WorkflowUserAction','ActionId') IS NOT NULL
BEGIN
    ALTER TABLE dbo.Pms_tbl_WorkflowUserAction
    ALTER COLUMN ActionId INT NULL;
END
GO

/* ============================================================
   READ USER ACTIONS ON EDIT

   No master join is required here. The page already loads the
   Action and Status master lists separately and selects by ID.
   This avoids any dependency on display-column names.
   ============================================================ */
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
            CASE WHEN ISNULL(UA.SequenceNo,1) = 1 THEN 'IF' ELSE 'ELSEIF' END
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
    WHERE UA.WorkflowId = @WorkflowId
      AND UA.regid = @regid
      AND ISNULL(UA.status,1) = 1
      AND ISNULL(UA.archive,0) = 0
    ORDER BY ISNULL(UA.SrNo,1), ISNULL(UA.SequenceNo,1), UA.id;
END
GO

/* ============================================================
   SAVE / UPDATE USER ACTIONS
   ============================================================ */
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

        /* 1. Workflow must belong to this company. */
        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Pms_tbl_Workflow
            WHERE id = @WorkflowId
              AND regid = @regid
              AND ISNULL(archive,0) = 0
        )
        BEGIN
            RAISERROR('Workflow not found for Action Conditions.',16,1);
        END;

        /* 2. Replace only this workflow's User Action rows. */
        DELETE FROM dbo.Pms_tbl_WorkflowUserAction
        WHERE WorkflowId = @WorkflowId
          AND regid = @regid;

        IF @UserActionsXml IS NOT NULL
           AND @UserActionsXml.exist('/UserActions/UserAction') = 1
        BEGIN
            /* 3. Parent branch + User Action branch types. */
            IF EXISTS
            (
                SELECT 1
                FROM @UserActionsXml.nodes('/UserActions/UserAction') T(C)
                WHERE UPPER(ISNULL(NULLIF(T.C.value('@Type','NVARCHAR(20)'),''),'IF'))
                      NOT IN ('IF','ELSEIF','ELSE')
                   OR UPPER(ISNULL(NULLIF(T.C.value('@ActionType','NVARCHAR(20)'),''),'IF'))
                      NOT IN ('IF','ELSEIF','ELSE')
            )
            BEGIN
                RAISERROR('Invalid IF / ELSE IF / ELSE type in Action Conditions.',16,1);
            END;

            /* 4. First User Action in every outer branch must be IF. */
            IF EXISTS
            (
                SELECT 1
                FROM
                (
                    SELECT
                        ISNULL(TRY_CONVERT(INT,NULLIF(T.C.value('@SrNo','NVARCHAR(20)'),'')),1) AS SrNo,
                        ISNULL(TRY_CONVERT(INT,NULLIF(T.C.value('@SequenceNo','NVARCHAR(20)'),'')),1) AS SequenceNo,
                        UPPER(ISNULL(NULLIF(T.C.value('@ActionType','NVARCHAR(20)'),''),'IF')) AS ActionType
                    FROM @UserActionsXml.nodes('/UserActions/UserAction') T(C)
                ) X
                WHERE X.SequenceNo = 1
                  AND X.ActionType <> 'IF'
            )
            BEGIN
                RAISERROR('First Action Condition must be IF.',16,1);
            END;

            /* ====================================================
               5. QC ACTION VALIDATION

               Correct source: dbo.Pms_tbl_SimpleMasterList
               ELSE does not require ActionId.
               ==================================================== */
            IF EXISTS
            (
                SELECT 1
                FROM @UserActionsXml.nodes('/UserActions/UserAction') T(C)
                WHERE UPPER(ISNULL(NULLIF(T.C.value('@ActionType','NVARCHAR(20)'),''),'IF')) <> 'ELSE'
                  AND
                  (
                      TRY_CONVERT(INT,NULLIF(T.C.value('@ActionId','NVARCHAR(20)'),'')) IS NULL
                      OR NOT EXISTS
                      (
                          SELECT 1
                          FROM dbo.Pms_tbl_SimpleMasterList SM
                          WHERE SM.id = TRY_CONVERT(INT,NULLIF(T.C.value('@ActionId','NVARCHAR(20)'),''))
                            AND SM.regid = @regid
                            AND ISNULL(SM.status,1) = 1
                            AND ISNULL(SM.archive,0) = 0
                      )
                  )
            )
            BEGIN
                RAISERROR('Invalid QC Action selected in Action Conditions.',16,1);
            END;

            /* ====================================================
               6. NEXT STATUS VALIDATION

               Correct source: dbo.Pms_tbl_Projectstatus
               Next Status is optional. If selected, it must be a
               valid active PMS Task/Subtask status for this regid.
               ==================================================== */
            IF EXISTS
            (
                SELECT 1
                FROM @UserActionsXml.nodes('/UserActions/UserAction') T(C)
                WHERE TRY_CONVERT(INT,NULLIF(T.C.value('@NextStatusId','NVARCHAR(20)'),'')) IS NOT NULL
                  AND NOT EXISTS
                  (
                      SELECT 1
                      FROM dbo.Pms_tbl_Projectstatus PS
                      WHERE PS.id = TRY_CONVERT(INT,NULLIF(T.C.value('@NextStatusId','NVARCHAR(20)'),''))
                        AND PS.regid = @regid
                        AND ISNULL(PS.status,1) = 1
                        AND ISNULL(PS.archive,0) = 0
                  )
            )
            BEGIN
                RAISERROR('Invalid Next Status selected in Action Conditions.',16,1);
            END;

            /* 7. Only one ELSE in each outer workflow branch. */
            IF EXISTS
            (
                SELECT X.SrNo
                FROM
                (
                    SELECT
                        ISNULL(TRY_CONVERT(INT,NULLIF(T.C.value('@SrNo','NVARCHAR(20)'),'')),1) AS SrNo,
                        UPPER(ISNULL(NULLIF(T.C.value('@ActionType','NVARCHAR(20)'),''),'IF')) AS ActionType
                    FROM @UserActionsXml.nodes('/UserActions/UserAction') T(C)
                ) X
                WHERE X.ActionType = 'ELSE'
                GROUP BY X.SrNo
                HAVING COUNT(*) > 1
            )
            BEGIN
                RAISERROR('Only one ELSE is allowed in Action Conditions for each workflow branch.',16,1);
            END;

            /* 8. ELSE must be the final User Action of that branch. */
            IF EXISTS
            (
                SELECT 1
                FROM
                (
                    SELECT
                        ISNULL(TRY_CONVERT(INT,NULLIF(T.C.value('@SrNo','NVARCHAR(20)'),'')),1) AS SrNo,
                        ISNULL(TRY_CONVERT(INT,NULLIF(T.C.value('@SequenceNo','NVARCHAR(20)'),'')),1) AS SequenceNo,
                        UPPER(ISNULL(NULLIF(T.C.value('@ActionType','NVARCHAR(20)'),''),'IF')) AS ActionType
                    FROM @UserActionsXml.nodes('/UserActions/UserAction') T(C)
                ) E
                WHERE E.ActionType = 'ELSE'
                  AND EXISTS
                  (
                      SELECT 1
                      FROM
                      (
                          SELECT
                              ISNULL(TRY_CONVERT(INT,NULLIF(T2.C.value('@SrNo','NVARCHAR(20)'),'')),1) AS SrNo,
                              ISNULL(TRY_CONVERT(INT,NULLIF(T2.C.value('@SequenceNo','NVARCHAR(20)'),'')),1) AS SequenceNo
                          FROM @UserActionsXml.nodes('/UserActions/UserAction') T2(C)
                      ) N
                      WHERE N.SrNo = E.SrNo
                        AND N.SequenceNo > E.SequenceNo
                  )
            )
            BEGIN
                RAISERROR('Action Conditions ELSE must be the last item.',16,1);
            END;

            /* 9. Insert User Action configuration. */
            INSERT INTO dbo.Pms_tbl_WorkflowUserAction
            (
                WorkflowId,
                SrNo,
                BranchType,
                SequenceNo,
                ActionBranchType,
                ActionId,
                NextStatusId,
                TakenById,
                DesignationId,
                IsEmail,
                IsWhatsApp,
                IsBell,
                IsRemarkRequired,
                regid,
                uid,
                status,
                archive,
                cdate,
                modifieddate
            )
            SELECT
                @WorkflowId,
                ISNULL(TRY_CONVERT(INT,NULLIF(T.C.value('@SrNo','NVARCHAR(20)'),'')),1),
                UPPER(ISNULL(NULLIF(T.C.value('@Type','NVARCHAR(20)'),''),'IF')),
                ISNULL(TRY_CONVERT(INT,NULLIF(T.C.value('@SequenceNo','NVARCHAR(20)'),'')),1),
                UPPER(ISNULL(NULLIF(T.C.value('@ActionType','NVARCHAR(20)'),''),'IF')),
                CASE
                    WHEN UPPER(ISNULL(NULLIF(T.C.value('@ActionType','NVARCHAR(20)'),''),'IF')) = 'ELSE'
                        THEN NULL
                    ELSE TRY_CONVERT(INT,NULLIF(T.C.value('@ActionId','NVARCHAR(20)'),'') )
                END,
                TRY_CONVERT(INT,NULLIF(T.C.value('@NextStatusId','NVARCHAR(20)'),'')),
                TRY_CONVERT(INT,NULLIF(T.C.value('@TakenById','NVARCHAR(20)'),'')),
                TRY_CONVERT(INT,NULLIF(T.C.value('@DesignationId','NVARCHAR(20)'),'')),
                ISNULL(TRY_CONVERT(BIT,NULLIF(T.C.value('@IsEmail','NVARCHAR(5)'),'')),0),
                ISNULL(TRY_CONVERT(BIT,NULLIF(T.C.value('@IsWhatsApp','NVARCHAR(5)'),'')),0),
                ISNULL(TRY_CONVERT(BIT,NULLIF(T.C.value('@IsBell','NVARCHAR(5)'),'')),0),
                ISNULL(TRY_CONVERT(BIT,NULLIF(T.C.value('@IsRemarkRequired','NVARCHAR(5)'),'')),0),
                @regid,
                @uid,
                1,
                0,
                GETDATE(),
                NULL
            FROM @UserActionsXml.nodes('/UserActions/UserAction') T(C);
        END;

        COMMIT TRANSACTION;

        SELECT
            1 AS Result,
            @WorkflowId AS WorkflowId,
            (
                SELECT COUNT(*)
                FROM dbo.Pms_tbl_WorkflowUserAction
                WHERE WorkflowId = @WorkflowId
                  AND regid = @regid
                  AND ISNULL(status,1) = 1
                  AND ISNULL(archive,0) = 0
            ) AS SavedRows,
            'Action Conditions saved successfully.' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @Err NVARCHAR(4000);
        SET @Err = ERROR_MESSAGE();
        RAISERROR(@Err,16,1);
    END CATCH
END
GO

/* ============================================================
   OPTIONAL QUICK CHECKS AFTER RUNNING THIS SCRIPT
   ============================================================

-- QC Action selected by the UI, e.g. ActionId = 6
SELECT id, MasterName, regid, status, archive
FROM dbo.Pms_tbl_SimpleMasterList
WHERE id = 6;

-- Next Status selected by the UI, e.g. NextStatusId = 3
SELECT id, ProjectStatus, regid, status, archive
FROM dbo.Pms_tbl_Projectstatus
WHERE id = 3;

-- Verify saved User Action rows
SELECT *
FROM dbo.Pms_tbl_WorkflowUserAction
ORDER BY id DESC;

*/
