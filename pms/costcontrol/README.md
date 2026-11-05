# Cost Control (Phase 1 + Phase 2)

Module at `/pms/costcontrol/`.

Existing PMS and ERP tables are **read only**:
- `Pms_tbl_AddProject`
- `Pms_tbl_AddMilestone`
- `Pms_tbl_Addtask`
- `tbl_employee`
- `tbl_unitmaster`

There is one exception: "+ Add new unit" inserts a unit through the existing ERP procedure `tbl_unitmasterInsertNew`.

## Install (non-production first)

1. Back up the PMS database.
2. Run `sql/costcontrol_phase1.sql`, then `sql/costcontrol_phase2.sql`.
   - **Always run them in this order.** Phase 2 replaces several Phase 1 procedures, so running Phase 1 alone later would undo Phase 2 behaviour.
   - Both are safe to re-run. Tables and columns are created only if missing, and only procedures named `usp_pms_cc_*` (plus two `ufn_pms_cc_*` functions) are created or altered.
   - Phase 2 needs SQL Server 2016 or later (`FOR JSON`).
3. Deploy these files:
   - `App_Code/PmsCostControl.cs` (new, shared access rules)
   - `pms/costcontrol/default.aspx`
   - `pms/costcontrol/default.aspx.cs`
   - `pms/costcontrol/app/costcontrol.main.js`
   - `pms/costcontrol/export.aspx`
   - `pms/costcontrol/export.aspx.cs`
   - `pms/angularmaster/angular_head.master` (the "Cost Control" link)
   - `pms/angularmaster/angular_head.master.cs` (one guarded call that decides whether to show the link)
4. Grant access in **Cost Control > Access**. Tick **Approve** for at least one person who does not prepare budgets.

## Who can do what

| Right | Allows | Granted by |
|---|---|---|
| View | All tabs read-only, Excel export | Access tab |
| Enter | Budgets (create, edit draft, submit), cost entries, forecasts (ETC), add unit | Access tab; administrators always have it |
| Approve | Approve and lock or return a submitted budget; close and reopen months | Access tab only. Administrators do **not** get it automatically (requirements 5.1). |
| Administrator | Cost Codes and Access tabs, company-level history | Existing PMS admin role |

A person can never approve a budget they submitted themselves (AP-03). This is enforced in the database procedure, not only on screen.

## What it does

| Requirement | Existing table/columns used | Implementation | Verification |
|---|---|---|---|
| REQ-003 / CC-01 Cost-code library and hierarchy | none (new `Pms_tbl_CostCode`) | Division > Section > Element. Only Element codes are posting codes: budget, cost entry and ETC refuse other levels. Codes are unique per company. | UI tested on a mock server |
| CC-04 Lock once used | — | A used code can only have its description changed; code, name, type, level and parent are locked in the procedure. Every change is in the history. | UI tested on a mock server |
| REQ-013 / AP-03 / AP-10 Baseline, revisions, maker-checker | Pms_tbl_AddProject | Draft > Submitted > Approved/Locked. "Return to Draft" needs a reason, which the preparer sees. A revision copies the last approved version and needs a reason. Only one open (draft or submitted) version at a time. | UI tested on a mock server |
| REQ-001 (partial) Hierarchy link | Pms_tbl_AddMilestone (id, milestone, ProjectId, orderno); Pms_tbl_Addtask (id, taskname, Milestoneid, orderno) | Lines, entries and ETC may reference a milestone and task. The server checks that they belong to the project and the company. | UI tested on a mock server |
| REQ-007 (partial) / DM-01 Actual cost | none (new `Pms_tbl_ProjectCostEntry`) | Manual entries with date-only `EntryDate`, site, source document type and reference. A duplicate reference with the same document type triggers a warning that must be confirmed. Negative amounts are reversals. | UI tested on a mock server |
| AU-01 / NF-06 Audit history | none (new `Pms_tbl_CostControlAudit`) | Every create, change, delete, submit, approve, return, close, reopen and access change is recorded, with a before/after JSON snapshot. A reason is mandatory to change or delete a saved cost entry, return a budget or reopen a month. A trigger refuses any update or delete of history. | UI tested on a mock server |
| RP-01 (partial) Forecast | none (new `Pms_tbl_ProjectEtc`) | One current ETC per project × cost code × milestone, with an as-of date and basis. EAC = Actual + ETC; VAC = Current budget − EAC. | UI tested on a mock server |
| RP-01 / RP-16 (partial) Reports | — | Budget vs Actual grouped by cost code, milestone or **cost type**. Columns: Original, Current, Actual, Remaining, ETC, EAC, VAC, Consumed %. The **Project Summary** tab covers all projects. | UI tested on a mock server |
| RP-17 Excel export | — | `export.aspx` (Spire.XLS, already in the PMS) for the report, summary, budget, cost entries and forecast. Each file carries title, project, filter/version, generation time and user. It is streamed to the browser, never saved on the server, and uses the same access check as the screen. | Compiles; **not run** |
| PE-01 / PE-04 / PE-06 (simplified) Period lock | none (new `Pms_tbl_CostControlPeriod`) | Monthly close per project. Entries dated in a closed month cannot be added, changed or deleted. Reopening needs a reason and is recorded. | UI tested on a mock server |
| 5.1 / NF-04 Access | tbl_employee (id, DisplayName, regid, status, archive), read only | New `Pms_tbl_CostControlAccess`: View / Enter / Approve per employee. The menu link shows for administrators and granted employees. | UI tested on a mock server |

## Rules preserved

- **Tenant isolation:** every query is filtered by `regid` from the existing login cookie.
- **Soft delete:** the `status=1 / archive=0` convention is used.
- **Parameterised data access:** stored procedures with parameters, and no string-built SQL with user input.
- **Existing Budget/Actual fields:** these fields on Project, Milestone, Task and Subtask are untouched.
- **Dates:** business dates are sent as `yyyy-MM-dd` and stored as `DATE`. Timestamps use the database server's `GETDATE()`.

## Verification done and not done

- **Done:**
  - all C# compiles (csc 4.x, with CommonClass and Spire.XLS references);
  - JavaScript syntax is checked;
  - the full page (real markup and controller) was driven in a browser against a mock server that applies the same rules as the procedures: budget revision, add line, submit, own-approval block, return with reason, duplicate-reference warning, closed-month block, edit/delete reason, ETC/EAC/VAC, cost-type grouping, month close/reopen, history, cost-code lock and hierarchy, access, and the view-only user's screens.
- **Not done:**
  - the Phase 2 SQL script has **not** been run on a database;
  - saving and reloading against the real database;
  - the Excel download;
  - the menu link on the real site;
  - regression of existing screens. The only existing files changed are `angular_head.master` (link condition) and `angular_head.master.cs` (one added field and one guarded call).

## Open decisions

See `GAP-REVIEW.md` §5:
- accounting entity;
- currency;
- the ERP source for commitments and actuals;
- whether the "Other" cost type is allowed;
- whether amounts are net of recoverable tax;
- the period-close calendar;
- whether to reuse the existing PMS audit log.

Sites are free text until a site master is chosen. `tbl_locationdetails` is a branch/office master, so it was not assumed to be a site list.

Spire.XLS adds an "Evaluation Warning" sheet if it is not licensed on the server. The PMS already uses it in the same way.
