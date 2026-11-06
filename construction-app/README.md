# Construction property application

A standalone ASP.NET Core Web API and Angular application built on the property structure
**Land → Project → Phase → Building (tower, block, villa cluster, plot sector) → Floor → Unit**.

The basic foundation comes from the **existing ERP tables, read-only**:

| Need | Existing table | How it is used |
|---|---|---|
| Company (RegId) | `tbl_registrationdetails` | `RegId = tbl_registrationdetails.id`, company name |
| People and sign-in | `tbl_employee` | same username (`emp_name`) and password as the ERP, checked the same way as `tbl_commonemployee_login`; `status = 1`, `archive = 0`; an ERP lock (`isactivelogin = 2`) is respected |
| Profiles | `Manage_Profile` | `tbl_employee.roleid` → profile; profile 1 (Admin) has every construction permission; other profiles get permissions in `cpm_ProfilePermission` |
| Countries, states, cities | `tbl_countrymaster`, `tbl_statemaster`, `tbl_City_master` | read for land and project addresses (the city master has no state column) |

Nothing is ever written to these tables. Everything construction-specific lives in **new `cpm_` tables and stored
procedures inside `absolutebook_2172`**. The PMS, CRM and Cost Control tables are not used. People, passwords and
profiles are managed in the ERP and apply here straight away.

The look follows the existing theme: the ERP login page (background, logo, box and button colours) and the PMS
colours (page `#f1f5f7`, white top bar, plum `#804574`, Helvetica Neue 13px).

## What is in this first delivery

| Area | Included |
|---|---|
| Sign-in | ERP username and password (case-sensitive, like the ERP), the app's own lockout after 5 failed attempts for 15 minutes (kept in `cpm_SignInState`, not in `tbl_employee`), 15-minute access tokens in memory, rotating refresh token in an httpOnly cookie (reuse revokes all sessions), rate limits. A username used by more than one active person is refused until it is made unique in the ERP |
| Administration | Construction permissions per ERP profile (12 permission codes), read-only people list with unlock, project membership, audit history |
| Masters | Unit types and floor level types (per company); countries, states and cities read from the ERP masters |
| Land | Parcels with owners (shares must total 100 percent), area entered in sq m, sq ft, acre or hectare and stored in sq m, encumbrance, registration |
| Projects | Project details, phases on or off, area unit, time zone, currency, unit number pattern, land links (area used can't exceed what is left on a parcel), team |
| Structure | Phases (with registration numbers), buildings by type, floor generator (basements, ground, podiums, typical floors, skips, terrace) with preview |
| Units | Unit generator with preview (towers by floor range; villas and plots without floors), stacking plan, list with filters, unit form, availability with allowed transitions, reason and history |

Not yet built (next stages of the proposal): compliance documents and checklists, reminders, work planning and Gantt,
unit finishing matrix, QC, budget and cost, dashboards and reports, email sending, password reset by email.

## Folders

```
construction-app/
├─ db/                 SQL scripts for absolutebook_2172 (run in order 001 → 005)
├─ src/
│  ├─ Construction.Domain/          rules: floor and unit generators, numbering, areas, availability
│  ├─ Construction.Application/     contracts, repository interfaces, services (auth, generators, land)
│  ├─ Construction.Infrastructure/  Dapper calls to the cpm_usp_* procedures, password hasher
│  └─ Construction.Api/             controllers, JWT, permissions, errors, create-tenant command
├─ tests/
│  ├─ Construction.Domain.Tests/    36 unit tests
│  └─ api-smoke.mjs                 20 end-to-end API checks (test database only)
├─ web/                Angular 22 app (standalone components, lazy feature routes, signals)
├─ .local.example/     example of the git-ignored .local/dev.env settings file
└─ run-api-dev.ps1     starts the API with settings from .local/dev.env
```

## Database

* Scripts: `db/001_foundation.sql`, `002_masters.sql`, `003_land_projects.sql`, `004_structure_units.sql`, `005_bootstrap.sql`.
* They check that the existing tables listed above are there, and only ever read them (no foreign keys onto them either).
* Every object is prefixed `cpm_` (tables `dbo.cpm_*`, procedures `dbo.cpm_usp_*`, table types `dbo.cpm_tt_*`).
* Each script stops unless it runs in `absolutebook_2172` (or a scratch database named `cpmtest_…` for testing),
  and records itself in `dbo.cpm_SchemaVersion`. All scripts are safe to run again.
* Written for SQL Server 2016 SP1, compatibility level 130 (no `STRING_AGG`, `TRIM`, `CONCAT_WS`).
* 21 tables, 43 procedures, 4 functions and 6 table types. Every business table has `RegId`; every procedure takes `@RegId` and `@UserId`
  first and checks permissions and project access itself. Saves use row versions, so a stale save is refused.

**Installing in absolutebook_2172 (your DBA runs these in SSMS):**

1. Back up the database.
2. Run `001` to `005` in order. Each prints nothing on success.
3. Add the starter construction masters for your company: `EXEC dbo.cpm_usp_Company_Setup @RegId = 2172;`
   (or `dotnet run --project src/Construction.Api -- setup-company --regid 2172`). It does nothing if they already exist.
4. Sign in with an ERP user whose profile is Admin, then give other profiles their construction permissions
   under Administration › Profile permissions.

## Running locally

Requirements: .NET SDK 10.0.403 or later, Node 22 LTS, SQL Server (LocalDB is enough for development).

1. Create a scratch database: `sqlcmd -S "(localdb)\MSSQLLocalDB" -E -Q "CREATE DATABASE cpmtest_scratch; ALTER DATABASE cpmtest_scratch SET COMPATIBILITY_LEVEL = 130;"`,
   run `tests/db/000_test_fixture.sql` (small stand-ins for the six ERP tables, with test passwords passed as sqlcmd variables
   `-v AdminPassword=… ViewerPassword=…`), then the five scripts, then `EXEC dbo.cpm_usp_Company_Setup @RegId = 2172`.
2. Copy `.local.example/dev.env.example` to `.local/dev.env` and fill it in.
3. Start the API: `powershell -ExecutionPolicy Bypass -File run-api-dev.ps1` → http://localhost:5160
4. Start the web app: `cd web; npm install; npm start` → http://localhost:4200 (calls to `/api` are proxied to the API).

Tests: `dotnet test`, `cd web; npx ng test --watch=false`, and with the API running `node tests/api-smoke.mjs`.

## Dates and areas

* All dates show as dd/mm/yyyy and date-times as dd/mm/yyyy HH:mm. Date fields accept dd/mm/yyyy typed text.
* The API sends dates as yyyy-mm-dd and timestamps in UTC.
* Areas are stored in square metres with 4 decimals and shown in each project's unit (sq m or sq ft).

## Before production

* **This folder sits inside the PMS web root.** If IIS serves `D:\pmsmanagement` as a website, block this folder
  (for example with `<hiddenSegments><add segment="construction-app" /></hiddenSegments>` in the site's Web.config),
  or move it to its own repository. It contains source code, not secrets.
* Put the signing key and connection string in environment variables or a secrets store on the server.
* Give the application's database login `EXECUTE` on the `cpm_usp_*` procedures only.
* Host the API and the built Angular app (`npm run build`) behind HTTPS on one site, so the refresh cookie stays same-site.
* SQL Server 2016 is past extended support (14/07/2026). Plan an upgrade; the scripts need no changes.
