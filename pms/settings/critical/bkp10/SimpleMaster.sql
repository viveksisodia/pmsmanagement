/* =========================================================
   PMS SIMPLE MASTER
   ---------------------------------------------------------
   UI input: Master Name only.

   Grid shows only:
     Sr. No. | Name | Date | Action

   Physical table name remains internal and is NOT displayed.

   Same-name table logic:
     Department -> Pms_tbl_Department
     Department -> Pms_tbl_Department2
     Department -> Pms_tbl_Department3

   GLOBAL TIME CONTRACT:
   Same business timezone used by the PMS assignment date fix:
   India / IST = UTC + 05:30.
   New timestamps are generated from GETUTCDATE() + 330 minutes,
   so IIS/SQL server physical timezone does not change stored business time.

   The list returns DisplayDate as text so JavaScript/browser timezone
   cannot convert or shift ASP.NET /Date(...)/ values.
   ========================================================= */

IF OBJECT_ID('dbo.Pms_tbl_ActionMasterListMasterList','U') IS NULL
BEGIN
    CREATE TABLE dbo.Pms_tbl_ActionMasterListMasterList
    (
        id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        MasterName NVARCHAR(150) NOT NULL,
        TableName NVARCHAR(200) NOT NULL,
        regid INT NULL,
        uid INT NULL,
        status BIT NULL DEFAULT 1,
        archive BIT NULL DEFAULT 0,
        cdate DATETIME NULL,
        modifieddate DATETIME NULL
    );
END
GO


CREATE OR ALTER PROCEDURE dbo.usp_Pms_SimpleMaster_List
(
    @regid INT,
    @uid INT = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        id,
        MasterName,

        /* Return display date as plain text.
           Do not expose DateTime to Angular, because JavaScriptSerializer
           would return /Date(...)/ and browser timezone can shift it. */
        CASE
            WHEN cdate IS NULL THEN ''
            ELSE CONVERT(VARCHAR(10), cdate, 105)
                 + ' '
                 + CONVERT(VARCHAR(5), cdate, 108)
        END AS DisplayDate

    FROM dbo.Pms_tbl_ActionMasterListMasterList
    WHERE regid = @regid
      AND ISNULL(status,1) = 1
      AND ISNULL(archive,0) = 0
    ORDER BY id DESC;
END
GO


CREATE OR ALTER PROCEDURE dbo.usp_Pms_SimpleMaster_Create
(
    @id INT = 0,
    @MasterName NVARCHAR(150),
    @regid INT,
    @uid INT = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        SET @MasterName = LTRIM(RTRIM(ISNULL(@MasterName,'')));

        IF @MasterName = ''
        BEGIN
            RAISERROR('Master Name is required.',16,1);
        END;

        /* PMS business time: UTC + 05:30.
           Independent from server/browser physical timezone. */
        DECLARE @BusinessNow DATETIME;
        SET @BusinessNow = DATEADD(MINUTE,330,GETUTCDATE());


        /* =====================================================
           UPDATE
           -----------------------------------------------------
           Only MasterName changes.
           Physical TableName is intentionally NOT renamed.
           This avoids breaking references/data already using it.
           ===================================================== */
        IF ISNULL(@id,0) > 0
        BEGIN
            UPDATE dbo.Pms_tbl_ActionMasterListMasterList
            SET
                MasterName = @MasterName,
                uid = @uid,
                modifieddate = @BusinessNow
            WHERE id = @id
              AND regid = @regid
              AND ISNULL(archive,0) = 0;

            IF @@ROWCOUNT = 0
            BEGIN
                RAISERROR('Master not found.',16,1);
            END;

            COMMIT TRANSACTION;

            SELECT
                1 AS Result,
                @id AS id,
                'Master updated successfully.' AS Message;

            RETURN;
        END;


        /* =====================================================
           INSERT - clean master name for safe physical table name
           ===================================================== */
        DECLARE @CleanName NVARCHAR(150) = '';
        DECLARE @i INT = 1;
        DECLARE @ch NCHAR(1);

        WHILE @i <= LEN(@MasterName)
        BEGIN
            SET @ch = SUBSTRING(@MasterName,@i,1);

            IF @ch LIKE '[A-Za-z0-9]'
            BEGIN
                SET @CleanName = @CleanName + @ch;
            END;

            SET @i = @i + 1;
        END;

        IF ISNULL(@CleanName,'') = ''
        BEGIN
            RAISERROR(
                'Master Name must contain letters or numbers.',
                16,
                1
            );
        END;

        IF LEN(@CleanName) > 100
        BEGIN
            SET @CleanName = LEFT(@CleanName,100);
        END;


        DECLARE @BaseTableName NVARCHAR(200);
        DECLARE @FinalTableName NVARCHAR(200);
        DECLARE @Counter INT = 1;

        SET @BaseTableName = 'Pms_tbl_' + @CleanName;
        SET @FinalTableName = @BaseTableName;

        /* Same physical name -> add 2,3,4... */
        WHILE OBJECT_ID('dbo.' + @FinalTableName,'U') IS NOT NULL
        BEGIN
            SET @Counter = @Counter + 1;
            SET @FinalTableName =
                @BaseTableName + CAST(@Counter AS NVARCHAR(20));
        END;


        /* =====================================================
           Create actual master table.
           Only one business column: Name.
           Remaining columns are PMS system columns.
           ===================================================== */
        DECLARE @Sql NVARCHAR(MAX);

        SET @Sql =
        N'CREATE TABLE dbo.' + QUOTENAME(@FinalTableName) + N'
        (
            id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
            Name NVARCHAR(250) NULL,
            regid INT NULL,
            uid INT NULL,
            status BIT NULL DEFAULT 1,
            archive BIT NULL DEFAULT 0,
            cdate DATETIME NULL DEFAULT (DATEADD(MINUTE,330,GETUTCDATE())),
            modifieddate DATETIME NULL
        );';

        EXEC sp_executesql @Sql;


        INSERT INTO dbo.Pms_tbl_ActionMasterListMasterList
        (
            MasterName,
            TableName,
            regid,
            uid,
            status,
            archive,
            cdate
        )
        VALUES
        (
            @MasterName,
            @FinalTableName,
            @regid,
            @uid,
            1,
            0,
            @BusinessNow
        );

        DECLARE @NewId INT;
        SET @NewId = CAST(SCOPE_IDENTITY() AS INT);

        COMMIT TRANSACTION;

        /* TableName is intentionally not returned to UI. */
        SELECT
            1 AS Result,
            @NewId AS id,
            'Master saved successfully.' AS Message;

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


CREATE OR ALTER PROCEDURE dbo.usp_Pms_SimpleMaster_Delete
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

        DECLARE @BusinessNow DATETIME;
        SET @BusinessNow = DATEADD(MINUTE,330,GETUTCDATE());

        UPDATE dbo.Pms_tbl_ActionMasterListMasterList
        SET
            status = 0,
            archive = 1,
            uid = @uid,
            modifieddate = @BusinessNow
        WHERE id = @id
          AND regid = @regid
          AND ISNULL(archive,0) = 0;

        IF @@ROWCOUNT = 0
        BEGIN
            RAISERROR('Master not found.',16,1);
        END;

        /* IMPORTANT:
           Physical generated table is NOT dropped here.
           This prevents accidental loss of master data/references. */

        COMMIT TRANSACTION;

        SELECT
            1 AS Result,
            'Master deleted successfully.' AS Message;

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
