/* =====================================================================
   Construction app · 005 company setup
   Target: absolutebook_2172 only. New cpm_ objects only. Safe to run again.

   Companies and people already exist (tbl_registrationdetails, tbl_employee).
   cpm_usp_Company_Setup adds the starter construction masters for one company
   (floor level types and unit types, without areas) if it has none yet.
   Run it once per company, for example:  EXEC dbo.cpm_usp_Company_Setup @RegId = 2172;
   ===================================================================== */
SET NOCOUNT ON;
IF DB_NAME() <> N'absolutebook_2172' AND DB_NAME() NOT LIKE N'cpmtest[_]%'
BEGIN
    RAISERROR(N'Stopped: this script may only run in absolutebook_2172.', 16, 1);
    SET NOEXEC ON;
END
GO
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'cpm_SchemaVersion')
   OR NOT EXISTS (SELECT 1 FROM dbo.cpm_SchemaVersion WHERE ScriptName = N'004_structure_units')
BEGIN
    RAISERROR(N'Stopped: run 001 to 004 first.', 16, 1);
    SET NOEXEC ON;
END
GO
/* The company-creation procedure of an earlier draft is no longer used. */
IF OBJECT_ID(N'dbo.cpm_usp_Tenant_Bootstrap', N'P') IS NOT NULL DROP PROCEDURE dbo.cpm_usp_Tenant_Bootstrap;
GO
CREATE OR ALTER PROCEDURE dbo.cpm_usp_Company_Setup
    @RegId int
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    IF NOT EXISTS (SELECT 1 FROM dbo.tbl_registrationdetails WHERE id = @RegId) THROW 50404, N'No company with this RegId in tbl_registrationdetails.', 1;
    DECLARE @Levels int = 0, @Types int = 0;
    BEGIN TRY
    BEGIN TRAN;
    IF NOT EXISTS (SELECT 1 FROM dbo.cpm_LevelType WHERE regid = @RegId AND status = 1 AND archive = 0)
    BEGIN
        INSERT dbo.cpm_LevelType(regid, Code, Name, IsHabitable, SortOrder) VALUES
            (@RegId, N'BAS', N'Basement', 0, 10), (@RegId, N'GRD', N'Ground', 1, 20), (@RegId, N'STL', N'Stilt', 0, 30),
            (@RegId, N'POD', N'Podium', 0, 40), (@RegId, N'TYP', N'Typical', 1, 50), (@RegId, N'REF', N'Refuge', 0, 60),
            (@RegId, N'SRV', N'Service', 0, 70), (@RegId, N'TER', N'Terrace', 0, 80);
        SET @Levels = @@ROWCOUNT;
    END
    IF NOT EXISTS (SELECT 1 FROM dbo.cpm_UnitType WHERE regid = @RegId AND status = 1 AND archive = 0)
    BEGIN
        INSERT dbo.cpm_UnitType(regid, Code, Name, Category, Bedrooms) VALUES
            (@RegId, N'1BHK', N'1 BHK', N'Residential', 1), (@RegId, N'2BHK', N'2 BHK', N'Residential', 2),
            (@RegId, N'3BHK', N'3 BHK', N'Residential', 3), (@RegId, N'4BHK', N'4 BHK', N'Residential', 4),
            (@RegId, N'PH', N'Penthouse', N'Residential', NULL), (@RegId, N'VILLA', N'Villa', N'Villa', NULL),
            (@RegId, N'PLOT', N'Plot', N'Plot', NULL), (@RegId, N'SHOP', N'Shop', N'Commercial', NULL),
            (@RegId, N'OFFICE', N'Office', N'Office', NULL), (@RegId, N'PARK', N'Parking', N'Parking', NULL);
        SET @Types = @@ROWCOUNT;
    END
    IF @Levels + @Types > 0
        INSERT dbo.cpm_AuditLog(regid, EntityType, EntityId, Action, NewData)
        VALUES(@RegId, N'Company', @RegId, N'Setup', (SELECT @Levels AS LevelTypes, @Types AS UnitTypes FOR JSON PATH, WITHOUT_ARRAY_WRAPPER));
    COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
    SELECT @Levels AS LevelTypesAdded, @Types AS UnitTypesAdded;
END
GO
IF NOT EXISTS (SELECT 1 FROM dbo.cpm_SchemaVersion WHERE ScriptName = N'005_bootstrap')
    INSERT dbo.cpm_SchemaVersion(ScriptName) VALUES(N'005_bootstrap');
GO
SET NOEXEC OFF;
GO
