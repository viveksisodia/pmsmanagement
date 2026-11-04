# Cost Control (Phase 1)

New module at `/pms/costcontrol/`. Only company administrators can use it, and the menu link appears only for them.

Existing PMS tables are **read only**. No existing table, column or stored procedure is changed.

## Install (non-production first)

1. Back up the PMS database.
2. Run `sql/costcontrol_phase1.sql`. It is safe to re-run:
   - tables are created only if missing;
   - procedures are created or altered, and only those named `usp_pms_cc_*`.
3. Deploy these files:
   - `pms/costcontrol/default.aspx`
   - `pms/costcontrol/default.aspx.cs`
   - `pms/costcontrol/app/costcontrol.main.js`
   - the edited `pms/angularmaster/angular_head.master` (4 added lines: the "Cost Control" link)

## What it does

| Requirement | Existing table/columns used | Implementation | Files | Verification |
|---|---|---|---|---|
| REQ-003 Cost-code library | none (new) | New table `Pms_tbl_CostCode`. Code is unique per regid. Cost types: Material, Labour, Subcontract, Equipment, Overhead, Other. A code that is in use cannot be deleted. | sql, default.aspx(.cs), costcontrol.main.js | C# compiles (csc 4.x); JS syntax checked. **Not run against a DB.** |
| REQ-013 Locked baseline and versioned revisions | Pms_tbl_AddProject (id, ProjectName) | `Pms_tbl_ProjectBudgetVersion` and `Pms_tbl_ProjectBudgetLine`. Version 1 is the Original budget. Locked versions are immutable. A revision copies the last locked version and requires a reason. Only one draft is allowed at a time. | same | Same as above |
| REQ-001 (partial) Budget/cost against existing hierarchy | Pms_tbl_AddMilestone (id, milestone, ProjectId, orderno); Pms_tbl_Addtask (id, taskname, Milestoneid, orderno) | Lines and entries may reference an existing milestone and task. The server checks that the task belongs to the milestone, the milestone to the project, and the project to the regid. | same | Same as above |
| REQ-007 (partial) Actual cost | none (new) | `Pms_tbl_ProjectCostEntry`: manual cost entries with a date-only `EntryDate`. Negative amounts are reversals. `SourceType` is reserved for future ERP feeds. | same | Same as above |
| REQ-024 (partial) Budget vs actual | — | Grouped by cost code or by milestone. Columns: Original, Current (latest locked), Actual, Variance, Consumed %. Drafts are excluded and flagged. | same | Same as above |

## Rules preserved

- **Tenant isolation:** every query is filtered by `regid` from the existing login cookie, and every row stores `regid`.
- **Soft delete:** the existing `status=1 / archive=0` convention is used.
- **Audit fields:** `cby`, `cdate`, `mby`, `mdate`. Locks record `LockedBy` / `LockedOn`.
- **Parameterised access:** all data access uses stored procedures with parameters, with no string-built SQL for user input.
- **Existing Budget/Actual fields:** these fields on Project, Milestone, Task and Subtask are **untouched**, and the module does not read or overwrite them.
- **Dates:** cost dates are sent as `yyyy-MM-dd` and stored as `DATE`, so there is no time-zone shift. Timestamps use the database server's `GETDATE()`, the same source as `DataAccess.getcdatetime()`.

## Not tested (no database or test login available)

- Running the SQL script, saving and reloading through the screens, and the menu link rendering.
- Regression check of existing screens. The only existing file changed is the master page, and only by 4 added lines.

## Open decisions

- Who may use the module beyond administrators (a role-permission module ID is needed).
- Whether actuals should come from the ERP (PO, GRN, bills, payroll) instead of manual entry.
- Currency per project, commitments, change orders, subcontracts and the other items listed in `pms-enhancement-coverage-map.md`.
