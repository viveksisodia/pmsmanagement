# Cost Control (Phases 1–3)

> **Phase 3 (2026-11-05)** adds:
> - monthly **snapshots**;
> - **budget movements**: transfers, reserves, corrections;
> - a **change register** with at-risk work;
> - **split** cost entries;
> - **per-project access** and **Pending Approvals**;
> - the label fixes and the unit duplicate check.
>
> **Phase 4 (2026-11-05)** adds supporting **documents on cost entries** (several files per entry, view / remove on hover).
>
> **Install order: `costcontrol_phase1.sql` → `costcontrol_phase2.sql` → `costcontrol_phase3.sql` → `costcontrol_phase4.sql`.** Each later script replaces some procedures of the earlier ones, so always re-run the whole chain in this order.
>
> See [Phase 3 details](#phase-3-details) and [Phase 4 details](#phase-4-details) at the end of this file.

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

## Phase 3 details

| Item | Requirements | What it does | New tables (Cost Control only) |
|---|---|---|---|
| Monthly snapshots | PE-01, PE-03, PE-06, AC-20 | **Close Month** freezes, per cost code × milestone: OB, revision/correction/change/transfer breakdown, RB, actual to month end and ETC. It also freezes RSV, PCE, PPC and at-risk exposure. The snapshot is immutable (a trigger refuses changes). Reopening keeps it, and the next close saves version n+1. **Budget vs Actual > As at** shows any snapshot, and Excel export marks it as a snapshot. | `Pms_tbl_CostControlSnapshot`, `Pms_tbl_CostControlSnapshotLine` |
| Budget movements | BV-1, BV-3, RS-1…RS-7, RP-04, AC-15, AC-16 | Types: **Transfer** (must net to zero), **Reserve addition**, **Reserve release** and **Reserve return** (total zero, so ΣRB + RSV is unchanged), **Baseline correction**, and **Approved change** (from the change register). Every type follows Draft → Submitted → Approved by another person, or is returned with a reason. On approval, no budget line or reserve may go below zero. RB = latest approved version + corrections + approved changes + net transfers. A new revision starts from RB. | `Pms_tbl_ProjectReserve`, `Pms_tbl_BudgetMovement`, `Pms_tbl_BudgetMovementLine` |
| Change register and at-risk | CM-1…CM-7, AR-01…AR-05, RP-05, AC-14, AC-17, AC-47 | There are six classes and four **independent** statuses: internal cost authorisation (approver ≠ person who raised it), client, subcontract, and budget posting. PCE = open, unposted changes. PPC = probable ones minus the part already in ETC. **Post to budget** creates an Approved-change movement. When that movement is approved, the change becomes Posted, and its remaining cost moves into ETC, so EACp does not jump (F-3). At-risk work needs its own authorisation before cost can be linked. Its cost stays in actuals and appears as at-risk exposure, or as unrecovered if the client rejects. | `Pms_tbl_ChangeRecord`, `Pms_tbl_ChangeAllocation` |
| Split cost entries | DM-03, REQ-005, AC-22 | One entry split across at least two cost codes and milestones. Rows must total exactly the entry amount (100 %), and each keeps its link to the source entry. Every report uses the split rows (view `vw_pms_cc_CostAllocation`). | `Pms_tbl_CostEntrySplit` |
| Per-project access | NF-04 | **Access > Choose projects**. No projects ticked means all projects, which keeps the old behaviour. Every server method and export checks it. | `Pms_tbl_CostControlAccessProject` |
| Pending approvals | — | One list of submitted budgets, submitted movements, change cost authorisations and at-risk authorisations, across the projects you can see. | — |
| Fixes | D-2, D-3, D-8, D-9, D-10 | Labels now read "Revised Budget (RB)" and "Actual (manual entries)", and the ETC help text says to include ordered-not-received value until commitments exist. Future-dated cost entries are rejected. Approval rights are checked inside the procedures. The Excel header shows currency and live-or-snapshot. | — |
| Unit duplicates | — | Adding a unit ignores case and extra spaces. An existing name is selected, with a message, instead of being added again. The Unit list shows each name once. Your unit master already contains duplicate names such as PCS ×3, CM ×2 and METER ×2; they were **not** changed. | — |

### Verification (Phase 3)

- **Compiles:** all C# (csc 4.x with CommonClass and Spire.XLS).
- **Syntax:** JavaScript checked.
- **Browser test against a mock server.** The real markup and controller were run with the Phase 3 rules mirrored in the mock. Every check passed:
  - **Reserves:** reserve addition; own-approval block.
  - **Transfers:** a transfer that doesn't net to zero is rejected.
  - **Reserve release:** RB CO-001 40,000 → 44,000 and RSV 10,000 → 6,000; ΣRB + RSV stays 50,000.
  - **Change register:** a change gives PCE 5,000 and PPC 4,000; internal and at-risk authorisation work; at-risk cost 1,500 shows as exposure; posting gives an approved movement, the change becomes Posted, and PCE/PPC drop to 0.
  - **Splits:** an unbalanced split is rejected; a balanced 60/40 split saves.
  - **Future dates:** rejected.
  - **Snapshots:** after Close Month, the snapshot stays at 14,500 while the live figure changes to 15,499.
  - **Other screens:** Pending Approvals, the unit duplicate check, project access, and the view-only user.
- **Not done:**
  - the Phase 3 SQL script has not been run;
  - saving and reloading on the real database;
  - Excel downloads;
  - the "approved change moves its cost into ETC" step, which happens only in SQL and not in the mock.

## Phase 4 details

**Cost entry documents.** In **Add / Edit Cost Entry**, the upload area appears once a **Source Document** type is selected.

- **Choosing files:** several files can be chosen at once. Each file shows as a tile; hovering over a tile shows **View** and **Remove** icons (on touch screens the icons are always shown).
- **When files are saved:** new files are uploaded when you click **Save**, after the entry itself is saved, one file per request. A saved file you remove is only marked; it is removed on Save, using the entry's "Reason for change", and **Undo** puts it back. If an upload fails after the entry is saved, the dialog closes and the message says which file failed, so the entry is never saved twice.
- **Cost Entries list:** the Source Doc column shows a paperclip with the number of files. Clicking it opens a read-only list with **View** and **Download**, available to every user with View rights.
- **Rules:**
  - PDF, JPG, PNG, GIF, WEBP, DOC(X), XLS(X), CSV or TXT only;
  - up to 10 MB per file and 20 per entry;
  - the first bytes of each file must match its extension;
  - if documents are attached, a Source Document type is required;
  - documents of an entry in a closed month cannot be added or removed;
  - only manual entries can have documents.
- **Storage:** files go under `~/App_Data/CostControlDocs/<regid>/<entry id>/`, with a generated name (GUID). IIS never serves `App_Data` directly; files are only read through `document.ashx`, which checks the login, rights and project access and sends the file with a fixed content type. The table keeps the original name, type, size, uploader and date. Removal is a soft delete recorded in the History tab. The file itself stays on disk.
- **New objects:**
  - table `Pms_tbl_CostEntryDocument`;
  - procedures `usp_pms_cc_CostEntryDoc_List`, `_Get`, `_Add` and `_Remove`;
  - handler `pms/costcontrol/document.ashx`;
  - WebMethod `RemoveCostEntryDocument`.
  - `GetCostEntries` also returns the documents. It returns an empty list until the Phase 4 script is installed.
- **Deploy:**
  - run `costcontrol_phase4.sql`;
  - copy `document.ashx`, `default.aspx`, `default.aspx.cs` and `app/costcontrol.main.js`;
  - give the IIS application pool identity **modify** rights on the site's `App_Data` folder (it is created if missing);
  - include `App_Data/CostControlDocs` in the server backup.

The existing PMS upload handlers (`FileUploadHandler*.ashx`) were **not** reused. They save files with their original names in public folders, build SQL by joining strings, and do not check the user's rights.

### Verification (Phase 4)

- **Checks run:**
  - the C# for the handler, page and shared class compiles (csc 4.x);
  - JavaScript syntax is checked;
  - SQL bracket and BEGIN/END balance is checked.
- **Browser test against a mock server (all passed):**
  - **Upload area:** it is hidden until a source document type is chosen. An .exe and an 11 MB file are refused, and choosing the same file twice is ignored.
  - **New entry:** two files upload after Save, and the list shows a paperclip with 2.
  - **Edit:** removing a saved file and adding another needs the change reason, and the reason is passed to the removal.
  - **Source type:** clearing it while files are attached is refused.
  - **Failed upload:** the message says the entry was saved and which file failed.
  - **Closed month:** nothing changes.
  - **Paperclip list:** it is read-only, and View and Download open `document.ashx?id=…` (plus `&download=1` for Download).
  - **View-only user:** they can view documents but get no upload or remove options.
  - **Multiple files:** three files go up as three requests.
- **Not done:**
  - the Phase 4 SQL script has not been run;
  - a real upload to IIS (folder permissions, file signature check, `TransmitFile`) has not been tried.