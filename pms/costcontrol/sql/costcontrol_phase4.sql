/* =====================================================================
   PMS Cost Control - Phase 4 database script: cost entry documents
   ---------------------------------------------------------------------
   Run AFTER costcontrol_phase1.sql, phase2.sql and phase3.sql
   (always in the order phase1 -> phase2 -> phase3 -> phase4). Safe to re-run.

   Adds supporting documents (bills, vouchers, POs, GRNs ...) to cost entries:
     - several files per entry;
     - the file itself is stored on the web server outside the public web folder
       (App_Data/CostControlDocs/<regid>/<entry id>/), under a generated name;
       this table keeps the original name, type and size;
     - files of an entry dated in a closed month cannot be added or removed;
     - removal is a soft delete (status 0 / archive 1) and is kept in the history.

   Creates only Cost Control objects (Pms_tbl_CostEntryDocument, usp_pms_cc_CostEntryDoc_*).
   No existing PMS / ERP table is changed.
   ===================================================================== */

IF OBJECT_ID('dbo.Pms_tbl_CostEntryDocument', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Pms_tbl_CostEntryDocument
    (
        id          INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Pms_tbl_CostEntryDocument PRIMARY KEY,
        regid       INT            NOT NULL,
        ProjectId   INT            NOT NULL,
        EntryId     INT            NOT NULL,     -- Pms_tbl_ProjectCostEntry.id
        FileName    NVARCHAR(260)  NOT NULL,     -- original file name, shown to users
        StoredName  NVARCHAR(100)  NOT NULL,     -- generated name on disk (GUID + extension)
        ContentType NVARCHAR(100)  NULL,
        FileSize    BIGINT         NOT NULL,
        status      INT            NOT NULL CONSTRAINT DF_Pms_tbl_CostEntryDocument_status  DEFAULT (1),
        archive     INT            NOT NULL CONSTRAINT DF_Pms_tbl_CostEntryDocument_archive DEFAULT (0),
        cby         INT            NULL,
        cdate       DATETIME       NOT NULL CONSTRAINT DF_Pms_tbl_CostEntryDocument_cdate DEFAULT (GETDATE()),
        dby         INT            NULL,
        ddate       DATETIME       NULL
    );
    CREATE INDEX IX_Pms_tbl_CostEntryDocument_entry ON dbo.Pms_tbl_CostEntryDocument (regid, EntryId, status, archive);
    CREATE INDEX IX_Pms_tbl_CostEntryDocument_project ON dbo.Pms_tbl_CostEntryDocument (regid, ProjectId, status, archive);
END
GO

-- Active documents of every cost entry of a project.
IF OBJECT_ID('dbo.usp_pms_cc_CostEntryDoc_List', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_CostEntryDoc_List AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_CostEntryDoc_List
    @regid INT, @ProjectId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT d.id, d.EntryId, d.FileName, d.ContentType, d.FileSize, d.cdate, e.DisplayName AS UploadedBy
      FROM dbo.Pms_tbl_CostEntryDocument d
      LEFT JOIN dbo.tbl_employee e ON e.id = d.cby AND e.regid = d.regid
     WHERE d.regid = @regid AND d.ProjectId = @ProjectId AND d.status = 1 AND d.archive = 0
     ORDER BY d.EntryId, d.id;
END
GO

-- One document, for viewing / downloading.
IF OBJECT_ID('dbo.usp_pms_cc_CostEntryDoc_Get', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_CostEntryDoc_Get AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_CostEntryDoc_Get
    @regid INT, @id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT d.id, d.ProjectId, d.EntryId, d.FileName, d.StoredName, d.ContentType, d.FileSize
      FROM dbo.Pms_tbl_CostEntryDocument d
     WHERE d.id = @id AND d.regid = @regid AND d.status = 1 AND d.archive = 0;
END
GO

-- Checks that a document may be attached (called before the file is written to disk)
-- and records it (called after). @StoredName NULL = check only.
IF OBJECT_ID('dbo.usp_pms_cc_CostEntryDoc_Add', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_CostEntryDoc_Add AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_CostEntryDoc_Add
    @regid INT, @EntryId INT, @FileName NVARCHAR(260), @StoredName NVARCHAR(100) = NULL,
    @ContentType NVARCHAR(100) = NULL, @FileSize BIGINT, @NewFiles INT = 1, @uid INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ProjectId INT, @EntryDate DATE, @SourceType NVARCHAR(30);
    SELECT @ProjectId = ProjectId, @EntryDate = EntryDate, @SourceType = SourceType
      FROM dbo.Pms_tbl_ProjectCostEntry WHERE id = @EntryId AND regid = @regid AND status = 1 AND archive = 0;
    IF @ProjectId IS NULL BEGIN SELECT 0 AS Result, N'The cost entry was not found.' AS Message; RETURN; END
    IF @SourceType <> N'Manual' BEGIN SELECT 0 AS Result, N'Documents can only be attached to manual cost entries.' AS Message; RETURN; END
    IF dbo.ufn_pms_cc_IsPeriodClosed(@regid, @ProjectId, @EntryDate) = 1
        BEGIN SELECT 0 AS Result, N'The month of this cost entry is closed; documents cannot be added.' AS Message; RETURN; END
    SET @FileName = LTRIM(RTRIM(ISNULL(@FileName, N'')));
    IF @FileName = N'' OR ISNULL(@FileSize, 0) <= 0 BEGIN SELECT 0 AS Result, N'The file is empty.' AS Message; RETURN; END
    IF (SELECT COUNT(*) FROM dbo.Pms_tbl_CostEntryDocument WHERE regid = @regid AND EntryId = @EntryId AND status = 1 AND archive = 0)
       + CASE WHEN @StoredName IS NULL THEN ISNULL(@NewFiles, 1) ELSE 1 END > 20
        BEGIN SELECT 0 AS Result, N'A cost entry can have at most 20 documents.' AS Message; RETURN; END

    IF @StoredName IS NULL BEGIN SELECT 1 AS Result, N'OK' AS Message, @ProjectId AS id; RETURN; END

    DECLARE @id INT, @new NVARCHAR(MAX);
    INSERT INTO dbo.Pms_tbl_CostEntryDocument (regid, ProjectId, EntryId, FileName, StoredName, ContentType, FileSize, cby, cdate)
    VALUES (@regid, @ProjectId, @EntryId, @FileName, @StoredName, @ContentType, @FileSize, @uid, GETDATE());
    SET @id = CAST(SCOPE_IDENTITY() AS INT);
    SET @new = (SELECT @EntryId AS EntryId, @FileName AS FileName, @FileSize AS FileSize FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
    EXEC dbo.usp_pms_cc_Audit_Add @regid, @ProjectId, N'CostEntryDocument', @id, N'Attach', NULL, NULL, @new, @uid;
    SELECT 1 AS Result, N'Document attached.' AS Message, @id AS id;
END
GO

-- Soft-deletes a document (kept in the history with the reason).
IF OBJECT_ID('dbo.usp_pms_cc_CostEntryDoc_Remove', 'P') IS NULL EXEC('CREATE PROCEDURE dbo.usp_pms_cc_CostEntryDoc_Remove AS RETURN 0');
GO
ALTER PROCEDURE dbo.usp_pms_cc_CostEntryDoc_Remove
    @regid INT, @id INT, @Reason NVARCHAR(1000) = NULL, @uid INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ProjectId INT, @EntryId INT, @EntryDate DATE, @old NVARCHAR(MAX);
    SELECT @ProjectId = d.ProjectId, @EntryId = d.EntryId, @EntryDate = c.EntryDate
      FROM dbo.Pms_tbl_CostEntryDocument d
      INNER JOIN dbo.Pms_tbl_ProjectCostEntry c ON c.id = d.EntryId AND c.regid = d.regid
     WHERE d.id = @id AND d.regid = @regid AND d.status = 1 AND d.archive = 0;
    IF @ProjectId IS NULL BEGIN SELECT 0 AS Result, N'The document was not found.' AS Message; RETURN; END
    IF dbo.ufn_pms_cc_IsPeriodClosed(@regid, @ProjectId, @EntryDate) = 1
        BEGIN SELECT 0 AS Result, N'The month of this cost entry is closed; documents cannot be removed.' AS Message; RETURN; END

    SET @old = (SELECT EntryId, FileName, FileSize FROM dbo.Pms_tbl_CostEntryDocument WHERE id = @id FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
    UPDATE dbo.Pms_tbl_CostEntryDocument SET status = 0, archive = 1, dby = @uid, ddate = GETDATE() WHERE id = @id;
    EXEC dbo.usp_pms_cc_Audit_Add @regid, @ProjectId, N'CostEntryDocument', @id, N'Remove', @Reason, @old, NULL, @uid;
    SELECT 1 AS Result, N'Document removed.' AS Message, @id AS id;
END
GO
