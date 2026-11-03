/* ============================================================
   PMS ACTION MASTER - CRUD PROCEDURES
   Table expected: dbo.Pms_tbl_ActionMaster
   ============================================================ */

/* If the table does not exist yet, use this block. */
IF OBJECT_ID('dbo.Pms_tbl_ActionMaster','U') IS NULL
BEGIN
    CREATE TABLE dbo.Pms_tbl_ActionMaster
    (
        id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        ActionName NVARCHAR(100) NOT NULL,
        ActionCode NVARCHAR(100) NOT NULL,
        RequiresRemark BIT NULL CONSTRAINT DF_PmsAction_RequiresRemark DEFAULT(0),
        RequiresRecipient BIT NULL CONSTRAINT DF_PmsAction_RequiresRecipient DEFAULT(0),
        RequiresDepartment BIT NULL CONSTRAINT DF_PmsAction_RequiresDepartment DEFAULT(0),
        IsFinalAction BIT NULL CONSTRAINT DF_PmsAction_IsFinal DEFAULT(0),
        SortOrder INT NULL CONSTRAINT DF_PmsAction_Sort DEFAULT(0),
        regid INT NULL,
        uid INT NULL,
        status BIT NULL CONSTRAINT DF_PmsAction_Status DEFAULT(1),
        archive BIT NULL CONSTRAINT DF_PmsAction_Archive DEFAULT(0),
        cdate DATETIME NULL CONSTRAINT DF_PmsAction_CDate DEFAULT(GETDATE()),
        modifieddate DATETIME NULL
    );
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Pms_ActionMaster_List
(
    @regid INT,
    @uid INT = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        id,
        ActionName,
        ActionCode,
        ISNULL(RequiresRemark,0) AS RequiresRemark,
        ISNULL(RequiresRecipient,0) AS RequiresRecipient,
        ISNULL(RequiresDepartment,0) AS RequiresDepartment,
        ISNULL(IsFinalAction,0) AS IsFinalAction,
        ISNULL(SortOrder,0) AS SortOrder,
        ISNULL(status,1) AS status
    FROM dbo.Pms_tbl_ActionMaster
    WHERE archive = 0
      AND regid = @regid
    ORDER BY ISNULL(SortOrder,0), ActionName, id;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Pms_ActionMaster_Save
(
    @id INT = 0,
    @ActionName NVARCHAR(100),
    @ActionCode NVARCHAR(100),
    @RequiresRemark BIT = 0,
    @RequiresRecipient BIT = 0,
    @RequiresDepartment BIT = 0,
    @IsFinalAction BIT = 0,
    @SortOrder INT = 0,
    @IsActive BIT = 1,
    @regid INT,
    @uid INT = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        SET @ActionName = LTRIM(RTRIM(ISNULL(@ActionName,'')));
        SET @ActionCode = UPPER(LTRIM(RTRIM(ISNULL(@ActionCode,''))));

        IF @ActionName = ''
            RAISERROR('Action Name is required.',16,1);

        IF @ActionCode = ''
            RAISERROR('Action Code is required.',16,1);

        IF EXISTS
        (
            SELECT 1
            FROM dbo.Pms_tbl_ActionMaster
            WHERE regid = @regid
              AND archive = 0
              AND id <> ISNULL(@id,0)
              AND UPPER(LTRIM(RTRIM(ActionName))) = UPPER(@ActionName)
        )
            RAISERROR('Action Name already exists.',16,1);

        IF EXISTS
        (
            SELECT 1
            FROM dbo.Pms_tbl_ActionMaster
            WHERE regid = @regid
              AND archive = 0
              AND id <> ISNULL(@id,0)
              AND UPPER(LTRIM(RTRIM(ActionCode))) = @ActionCode
        )
            RAISERROR('Action Code already exists.',16,1);

        IF ISNULL(@id,0) = 0
        BEGIN
            INSERT INTO dbo.Pms_tbl_ActionMaster
            (
                ActionName,
                ActionCode,
                RequiresRemark,
                RequiresRecipient,
                RequiresDepartment,
                IsFinalAction,
                SortOrder,
                regid,
                uid,
                status,
                archive,
                cdate
            )
            VALUES
            (
                @ActionName,
                @ActionCode,
                ISNULL(@RequiresRemark,0),
                ISNULL(@RequiresRecipient,0),
                ISNULL(@RequiresDepartment,0),
                ISNULL(@IsFinalAction,0),
                ISNULL(@SortOrder,0),
                @regid,
                @uid,
                ISNULL(@IsActive,1),
                0,
                GETDATE()
            );

            SET @id = CAST(SCOPE_IDENTITY() AS INT);

            COMMIT TRANSACTION;
            SELECT 1 AS Result, @id AS id, 'Action saved successfully.' AS Message;
            RETURN;
        END

        UPDATE dbo.Pms_tbl_ActionMaster
        SET
            ActionName = @ActionName,
            ActionCode = @ActionCode,
            RequiresRemark = ISNULL(@RequiresRemark,0),
            RequiresRecipient = ISNULL(@RequiresRecipient,0),
            RequiresDepartment = ISNULL(@RequiresDepartment,0),
            IsFinalAction = ISNULL(@IsFinalAction,0),
            SortOrder = ISNULL(@SortOrder,0),
            status = ISNULL(@IsActive,1),
            uid = @uid,
            modifieddate = GETDATE()
        WHERE id = @id
          AND regid = @regid
          AND archive = 0;

        IF @@ROWCOUNT = 0
            RAISERROR('Action not found.',16,1);

        COMMIT TRANSACTION;
        SELECT 1 AS Result, @id AS id, 'Action updated successfully.' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        DECLARE @Err NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@Err,16,1);
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Pms_ActionMaster_Delete
(
    @id INT,
    @regid INT,
    @uid INT = NULL
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
            FROM dbo.Pms_tbl_ActionMaster
            WHERE id = @id
              AND regid = @regid
              AND archive = 0
        )
        BEGIN
            COMMIT TRANSACTION;
            SELECT 0 AS Result, 'Action not found.' AS Message;
            RETURN;
        END

        /* When workflow action table is introduced, block deletion if action is in use. */
        IF OBJECT_ID('dbo.Pms_tbl_WorkflowAction','U') IS NOT NULL
        BEGIN
            DECLARE @Used INT = 0;
            DECLARE @Sql NVARCHAR(MAX) = N'
                SELECT @x = COUNT(1)
                FROM dbo.Pms_tbl_WorkflowAction
                WHERE ActionId = @ActionId
                  AND ISNULL(archive,0) = 0;';

            EXEC sp_executesql
                @Sql,
                N'@ActionId INT, @x INT OUTPUT',
                @ActionId = @id,
                @x = @Used OUTPUT;

            IF @Used > 0
            BEGIN
                COMMIT TRANSACTION;
                SELECT 0 AS Result, 'Action is used in workflow and cannot be deleted.' AS Message;
                RETURN;
            END
        END

        UPDATE dbo.Pms_tbl_ActionMaster
        SET
            status = 0,
            archive = 1,
            uid = @uid,
            modifieddate = GETDATE()
        WHERE id = @id
          AND regid = @regid
          AND archive = 0;

        COMMIT TRANSACTION;
        SELECT 1 AS Result, 'Action deleted successfully.' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        DECLARE @Err NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@Err,16,1);
    END CATCH
END
GO
