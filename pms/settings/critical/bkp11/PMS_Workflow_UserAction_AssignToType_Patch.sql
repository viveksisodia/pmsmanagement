/* ============================================================
   PMS QC WORKFLOW - USER ACTION ASSIGN TO TYPE PATCH

   Scope: ONLY dbo.Pms_tbl_WorkflowUserAction and its Save/Get procs.
   Existing Workflow Header / Conditions / Automatic (THEN) Action /
   Delete / Status logic is not changed.

   New separate field:
       AssignToType = DEPARTMENT | OWNER | NO_ASSIGNMENT

   Important:
       TakenById     continues to store Department Id ONLY.
       DesignationId continues to store Designation Id ONLY.
   ============================================================ */

SET NOCOUNT ON;
GO

/* ------------------------------------------------------------
   1. Add separate AssignToType column.
   ------------------------------------------------------------ */
IF OBJECT_ID('dbo.Pms_tbl_WorkflowUserAction','U') IS NULL
BEGIN
    RAISERROR('Table dbo.Pms_tbl_WorkflowUserAction does not exist.',16,1);
    RETURN;
END
GO

IF COL_LENGTH('dbo.Pms_tbl_WorkflowUserAction','AssignToType') IS NULL
BEGIN
    ALTER TABLE dbo.Pms_tbl_WorkflowUserAction
    ADD AssignToType NVARCHAR(30) NULL;
END
GO

/* Existing rows did not have AssignToType.
   If a Department / Designation was already saved, preserve it as DEPARTMENT.
   Otherwise preserve the old no-assignment behavior as NO_ASSIGNMENT.
   OWNER cannot be inferred from old rows, so it is never guessed. */
UPDATE UA
SET AssignToType = CASE
                       WHEN UA.TakenById IS NOT NULL OR UA.DesignationId IS NOT NULL
                           THEN 'DEPARTMENT'
                       ELSE 'NO_ASSIGNMENT'
                   END
FROM dbo.Pms_tbl_WorkflowUserAction UA
WHERE NULLIF(LTRIM(RTRIM(UA.AssignToType)),'') IS NULL;
GO

/* ------------------------------------------------------------
   2. Read Action Conditions / User Actions on Edit.
      Existing output columns remain the same; only AssignToType is added.
   ------------------------------------------------------------ */
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
        CASE
            WHEN UPPER(ISNULL(NULLIF(LTRIM(RTRIM(UA.AssignToType)),''),''))
                 IN ('DEPARTMENT','OWNER','NO_ASSIGNMENT')
                THEN UPPER(UA.AssignToType)
            WHEN UA.TakenById IS NOT NULL OR UA.DesignationId IS NOT NULL
                THEN 'DEPARTMENT'
            ELSE 'NO_ASSIGNMENT'
        END AS AssignToType,
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

/* ------------------------------------------------------------
   3. Save/Update Action Conditions / User Actions.

   Existing behavior preserved:
       ActionId      -> dbo.Pms_tbl_SimpleMasterList.id
       NextStatusId  -> dbo.Pms_tbl_Projectstatus.id
       IF / ELSE IF / ELSE validations unchanged.

   New behavior only:
       AssignToType is saved separately.
       Department/Designation IDs are kept only for DEPARTMENT.
   ------------------------------------------------------------ */
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

            /* New Assign To Type validation. Missing value is allowed only for
               backward compatibility and is inferred during INSERT. */
            IF EXISTS
            (
                SELECT 1
                FROM @UserActionsXml.nodes('/UserActions/UserAction') T(C)
                WHERE NULLIF(LTRIM(RTRIM(
                          T.C.value('string((@AssignToType)[1])','NVARCHAR(30)')
                      )),'') IS NOT NULL
                  AND UPPER(LTRIM(RTRIM(
                          T.C.value('string((@AssignToType)[1])','NVARCHAR(30)')
                      ))) NOT IN ('DEPARTMENT','OWNER','NO_ASSIGNMENT')
            )
                RAISERROR('Invalid Assign To Type in Action Conditions.',16,1);

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
                ActionId,NextStatusId,AssignToType,TakenById,DesignationId,
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
                /* Separate assignment type. Old XML without this attribute is inferred. */
                CASE
                    WHEN NULLIF(LTRIM(RTRIM(
                        T.C.value('string((@AssignToType)[1])','NVARCHAR(30)')
                    )),'') IS NOT NULL
                        THEN UPPER(LTRIM(RTRIM(
                            T.C.value('string((@AssignToType)[1])','NVARCHAR(30)')
                        )))
                    WHEN TRY_CONVERT(INT,NULLIF(
                             T.C.value('string((@TakenById)[1])','NVARCHAR(30)'),'')
                         ) IS NOT NULL
                      OR TRY_CONVERT(INT,NULLIF(
                             T.C.value('string((@DesignationId)[1])','NVARCHAR(30)'),'')
                         ) IS NOT NULL
                        THEN 'DEPARTMENT'
                    ELSE 'NO_ASSIGNMENT'
                END,
                /* TakenById remains DepartmentId only. */
                CASE
                    WHEN
                        CASE
                            WHEN NULLIF(LTRIM(RTRIM(
                                T.C.value('string((@AssignToType)[1])','NVARCHAR(30)')
                            )),'') IS NOT NULL
                                THEN UPPER(LTRIM(RTRIM(
                                    T.C.value('string((@AssignToType)[1])','NVARCHAR(30)')
                                )))
                            WHEN TRY_CONVERT(INT,NULLIF(
                                     T.C.value('string((@TakenById)[1])','NVARCHAR(30)'),'')
                                 ) IS NOT NULL
                              OR TRY_CONVERT(INT,NULLIF(
                                     T.C.value('string((@DesignationId)[1])','NVARCHAR(30)'),'')
                                 ) IS NOT NULL
                                THEN 'DEPARTMENT'
                            ELSE 'NO_ASSIGNMENT'
                        END = 'DEPARTMENT'
                    THEN TRY_CONVERT(INT,NULLIF(
                        T.C.value('string((@TakenById)[1])','NVARCHAR(30)'),'')
                    )
                    ELSE NULL
                END,
                CASE
                    WHEN
                        CASE
                            WHEN NULLIF(LTRIM(RTRIM(
                                T.C.value('string((@AssignToType)[1])','NVARCHAR(30)')
                            )),'') IS NOT NULL
                                THEN UPPER(LTRIM(RTRIM(
                                    T.C.value('string((@AssignToType)[1])','NVARCHAR(30)')
                                )))
                            WHEN TRY_CONVERT(INT,NULLIF(
                                     T.C.value('string((@TakenById)[1])','NVARCHAR(30)'),'')
                                 ) IS NOT NULL
                              OR TRY_CONVERT(INT,NULLIF(
                                     T.C.value('string((@DesignationId)[1])','NVARCHAR(30)'),'')
                                 ) IS NOT NULL
                                THEN 'DEPARTMENT'
                            ELSE 'NO_ASSIGNMENT'
                        END = 'DEPARTMENT'
                    THEN TRY_CONVERT(INT,NULLIF(
                        T.C.value('string((@DesignationId)[1])','NVARCHAR(30)'),'')
                    )
                    ELSE NULL
                END,
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

/* ------------------------------------------------------------
   4. No change required in:
      dbo.usp_Pms_WorkflowUserAction_DeleteByWorkflow
      dbo.Pms_tbl_WorkflowThenAction
      dbo.usp_Pms_WorkflowThenAction_Save
      dbo.usp_Pms_WorkflowThenAction_GetByWorkflow
      dbo.usp_Pms_WorkflowThenAction_DeleteByWorkflow
      or any of the main Workflow procedures.
   ------------------------------------------------------------ */

/* Verification after running patch:
SELECT id, WorkflowId, SrNo, ActionBranchType, ActionId, NextStatusId,
       AssignToType, TakenById, DesignationId,
       IsEmail, IsWhatsApp, IsBell, IsRemarkRequired,
       regid, uid, status, archive
FROM dbo.Pms_tbl_WorkflowUserAction
WHERE regid = 2172
  AND ISNULL(status,1)=1
  AND ISNULL(archive,0)=0
ORDER BY WorkflowId, SrNo, SequenceNo, id;
*/
