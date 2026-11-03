/* =========================================================
   PMS SIMPLE MASTER CREATOR
   User enters only Master Name.
   Example:
      Department -> Pms_tbl_Department
      Department -> Pms_tbl_Department2
      Department -> Pms_tbl_Department3
   ========================================================= */

IF OBJECT_ID('dbo.Pms_tbl_SimpleMasterList','U') IS NULL
BEGIN
    CREATE TABLE dbo.Pms_tbl_SimpleMasterList
    (
        id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,

        MasterName NVARCHAR(150) NOT NULL,
        TableName NVARCHAR(200) NOT NULL,

        regid INT NULL,
        uid INT NULL,

        status BIT NULL DEFAULT 1,
        archive BIT NULL DEFAULT 0,

        cdate DATETIME NULL DEFAULT GETDATE(),
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
        TableName,
        cdate
    FROM dbo.Pms_tbl_SimpleMasterList
    WHERE regid = @regid
      AND status = 1
      AND archive = 0
    ORDER BY id DESC;
END
GO


CREATE OR ALTER PROCEDURE dbo.usp_Pms_SimpleMaster_Create
(
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

        SET @MasterName =
            LTRIM(RTRIM(ISNULL(@MasterName,'')));

        IF @MasterName = ''
        BEGIN
            RAISERROR(
                'Master Name is required.',
                16,
                1
            );
        END;


        /* -------------------------------------------------
           Clean Master Name for SQL table name.
           Keep only A-Z, a-z and 0-9.
           ------------------------------------------------- */
        DECLARE @CleanName NVARCHAR(150) = '';
        DECLARE @i INT = 1;
        DECLARE @ch NCHAR(1);

        WHILE @i <= LEN(@MasterName)
        BEGIN
            SET @ch = SUBSTRING(@MasterName,@i,1);

            IF
            (
                @ch LIKE '[A-Za-z0-9]'
            )
            BEGIN
                SET @CleanName = @CleanName + @ch;
            END

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


        /* Avoid table-name limit issue */
        IF LEN(@CleanName) > 100
        BEGIN
            SET @CleanName =
                LEFT(@CleanName,100);
        END;


        DECLARE @BaseTableName NVARCHAR(200);
        DECLARE @FinalTableName NVARCHAR(200);
        DECLARE @Counter INT = 1;

        SET @BaseTableName =
            'Pms_tbl_' + @CleanName;

        SET @FinalTableName =
            @BaseTableName;


        /* -------------------------------------------------
           If same table already exists:
           Pms_tbl_Department
           Pms_tbl_Department2
           Pms_tbl_Department3
           ...
           ------------------------------------------------- */
        WHILE OBJECT_ID(
            'dbo.' + @FinalTableName,
            'U'
        ) IS NOT NULL
        BEGIN

            SET @Counter =
                @Counter + 1;

            SET @FinalTableName =
                @BaseTableName
                + CAST(@Counter AS NVARCHAR(20));

        END;


        /* -------------------------------------------------
           Create actual simple master table.

           Only one business input field = Name.
           Other columns are system columns.
           ------------------------------------------------- */
        DECLARE @Sql NVARCHAR(MAX);

        SET @Sql =
        N'
        CREATE TABLE dbo.'
        + QUOTENAME(@FinalTableName)
        + N'
        (
            id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,

            Name NVARCHAR(250) NULL,

            regid INT NULL,
            uid INT NULL,

            status BIT NULL DEFAULT 1,
            archive BIT NULL DEFAULT 0,

            cdate DATETIME NULL DEFAULT GETDATE(),
            modifieddate DATETIME NULL
        );';

        EXEC sp_executesql @Sql;


        /* -------------------------------------------------
           Save created master information.
           ------------------------------------------------- */
        INSERT INTO dbo.Pms_tbl_SimpleMasterList
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
            GETDATE()
        );


        DECLARE @NewId INT =
            CAST(SCOPE_IDENTITY() AS INT);


        COMMIT TRANSACTION;


        SELECT
            1 AS Result,
            @NewId AS id,
            @MasterName AS MasterName,
            @FinalTableName AS TableName,
            'Master created successfully. Table: '
                + @FinalTableName AS Message;

    END TRY

    BEGIN CATCH

        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END;

        DECLARE @Err NVARCHAR(4000) =
            ERROR_MESSAGE();

        RAISERROR(
            @Err,
            16,
            1
        );

    END CATCH
END
GO
