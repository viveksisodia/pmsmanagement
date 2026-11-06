/* TEST ONLY. Creates minimal stand-ins for the existing ERP tables the app reads
   (tbl_registrationdetails, tbl_employee, Manage_Profile, tbl_countrymaster, tbl_statemaster, tbl_City_master)
   in a scratch database named cpmtest_*. Never run this in absolutebook_2172.
   sqlcmd variables: AdminPassword, ViewerPassword (test values from .local/dev.env). */
SET NOCOUNT ON;
IF DB_NAME() NOT LIKE N'cpmtest[_]%'
BEGIN
    RAISERROR(N'Stopped: the test fixture only runs in a cpmtest_ database.', 16, 1);
    SET NOEXEC ON;
END
GO
CREATE TABLE dbo.tbl_registrationdetails(id int IDENTITY(1,1) PRIMARY KEY, company_name nvarchar(500), countryid int, status bit, archive bit);
CREATE TABLE dbo.Manage_Profile(id int IDENTITY(1,1) PRIMARY KEY, profile nvarchar(200), regid int NULL, status bit, archive bit);
CREATE TABLE dbo.tbl_employee(id int IDENTITY(1,1) PRIMARY KEY, regid int, emp_name nvarchar(100), DisplayName nvarchar(100), f_name nvarchar(100),
    l_name nvarchar(100), Password nvarchar(100), email_Id nvarchar(100), roleid nvarchar(max), status bit, archive bit, isactivelogin int NULL);
CREATE TABLE dbo.tbl_countrymaster(id int IDENTITY(1,1) PRIMARY KEY, countryname nvarchar(100), status bit, archive bit, regid int NULL);
CREATE TABLE dbo.tbl_statemaster(id int IDENTITY(1,1) PRIMARY KEY, statename nvarchar(100), countryid int, status bit, archive bit, regid int NULL);
CREATE TABLE dbo.tbl_City_master(City_ID int IDENTITY(1,1) PRIMARY KEY, City_name nvarchar(100), Status bit, Archive bit, regid int NULL);
GO
SET IDENTITY_INSERT dbo.tbl_registrationdetails ON;
INSERT dbo.tbl_registrationdetails(id, company_name, countryid, status, archive) VALUES (2172, N'Demo Developers', 1, 1, 0), (9001, N'Other Company', 1, 1, 0);
SET IDENTITY_INSERT dbo.tbl_registrationdetails OFF;
INSERT dbo.Manage_Profile(profile, regid, status, archive) VALUES (N'Admin', NULL, 1, 0), (N'Site engineer', 2172, 1, 0), (N'Viewer', 2172, 1, 0), (N'Accounts', 9001, 1, 0);
INSERT dbo.tbl_employee(regid, emp_name, DisplayName, f_name, l_name, Password, email_Id, roleid, status, archive, isactivelogin) VALUES
 (2172, N'demoadmin', N'Demo Admin', N'Demo', N'Admin', N'$(AdminPassword)', N'admin@demo.test', N'1', 1, 0, NULL),
 (2172, N'viewer1', NULL, N'View', N'Only', N'$(ViewerPassword)', N'viewer@demo.test', N'3', 1, 0, NULL),
 (2172, N'engineer1', N'Site Engineer', N'Site', N'Engineer', N'$(ViewerPassword)', NULL, N'2', 1, 0, NULL),
 (9001, N'otheradmin', N'Other Admin', NULL, NULL, N'$(AdminPassword)', NULL, N'1', 1, 0, NULL),
 (2172, N'shared', N'Shared One', NULL, NULL, N'$(ViewerPassword)', NULL, N'2', 1, 0, NULL),
 (9001, N'shared', N'Shared Two', NULL, NULL, N'$(ViewerPassword)', NULL, N'4', 1, 0, NULL),
 (2172, N'erplocked', N'ERP Locked', NULL, NULL, N'$(ViewerPassword)', NULL, N'2', 1, 0, 2),
 (2172, N'leftco', N'Left Company', NULL, NULL, N'$(ViewerPassword)', NULL, N'2', 1, 1, NULL);
INSERT dbo.tbl_countrymaster(countryname, status, archive, regid) VALUES (N'India', 1, 0, NULL), (N'UAE', 1, 0, NULL);
INSERT dbo.tbl_statemaster(statename, countryid, status, archive, regid) VALUES (N'Uttar Pradesh', 1, 1, 0, NULL), (N'Haryana', 1, 1, 0, NULL), (N'Dubai', 2, 1, 0, 2172);
INSERT dbo.tbl_City_master(City_name, Status, Archive, regid) VALUES (N'Noida', 1, 0, 0), (N'Gurugram', 1, 0, 0), (N'Dubai', 1, 0, 0);
GO
SET NOEXEC OFF;
GO
