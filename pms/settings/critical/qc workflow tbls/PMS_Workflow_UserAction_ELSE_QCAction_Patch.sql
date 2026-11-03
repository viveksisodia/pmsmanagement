/* ============================================================
   PMS QC WORKFLOW - ELSE QC ACTION PATCH

   Scope:
     ONLY dbo.usp_Pms_WorkflowUserAction_Save

   Change:
     - IF, ELSE IF and ELSE all require/store ActionId.
     - ELSE remains the last/default branch type; only its QC Action
       persistence is changed.
     - AssignToType / TakenById / DesignationId / NextStatus /
       Notification / Remark behavior is preserved.
   ============================================================ */
SET NOCOUNT ON;
GO

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

            /* QC ACTION: IF / ELSE IF / ELSE all require a valid master value. */
            IF OBJECT_ID('dbo.Pms_tbl_SimpleMasterList','U') IS NULL
                RAISERROR('QC Action master dbo.Pms_tbl_SimpleMasterList is missing.',16,1);

            IF EXISTS
            (
                SELECT 1
                FROM @UserActionsXml.nodes('/UserActions/UserAction') T(C)
                WHERE TRY_CONVERT(INT,NULLIF(
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
                RAISERROR('Invalid QC Action selected in Action Conditions.',16,1);

            /* NEXT STATUS: existing validation unchanged. */
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

            /* ELSE must remain last. */
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
                /* IMPORTANT: do not force ELSE ActionId to NULL. */
                TRY_CONVERT(INT,NULLIF(
                    T.C.value('string((@ActionId)[1])','NVARCHAR(30)'),'')
                ),
                TRY_CONVERT(INT,NULLIF(
                    T.C.value('string((@NextStatusId)[1])','NVARCHAR(30)'),'')
                ),
                /* Existing separate Assign To Type behavior preserved. */
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

/* Verification after save:
SELECT WorkflowId,SrNo,SequenceNo,ActionBranchType,ActionId,NextStatusId,
       AssignToType,TakenById,DesignationId
FROM dbo.Pms_tbl_WorkflowUserAction
WHERE WorkflowId = YOUR_WORKFLOW_ID
  AND regid = YOUR_REGID
  AND ISNULL(status,1)=1
  AND ISNULL(archive,0)=0
ORDER BY SrNo,SequenceNo,id;
*/
