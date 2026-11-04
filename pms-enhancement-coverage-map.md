# PMS Enhancement Coverage Map (Step 1 and Step 2, before implementation)

**Date:** 2026-11-04
**Branch:** avdesh
**Status:** For owner approval. **No PMS code, database or configuration has been changed.**

## Scope

This map covers the requirements in `requirements.md` v1.0-draft (REQ-001…REQ-031). These are the requirements produced by the construction PMS research.

- Each requirement is mapped to the existing PMS screen, backend method and table/column.
- Each is classified as **Covered**, **Partial** or **Missing**.
- Per your rules, nothing is implemented until you approve:
  - the schema changes;
  - the open decisions in §4.

## Evidence basis and limits

The evidence comes from read-only inspection of the source in this repo (see `existing-pms-inspection.md`). Three limits apply:

- **Column names are inferred.** Names such as `Budget` and `Actual` come from stored-procedure parameters in code, not from the database. Their data types are **unknown**; the C# model holds `Budget` as a `string` (pms\Addproject.aspx.cs:4330).
- **Stored procedures are not in the repo.** Their logic, for example `pmstbl_Addproject_usp` and `pms_Adtask_usp_ang`, is unknown.
- **`CommonClass.dll` source is missing.**

## 1. Summary

| Classification | Count | Requirements |
|---|---|---|
| Covered | 0 | — |
| Partial | 8 | REQ-001, 011, 013, 016, 022, 024, 025, 029 |
| Missing, needs columns only | 2 | REQ-006, 026 (project-level fields) |
| Missing, needs new tables | 18 | REQ-002, 003, 004, 005, 007, 008, 009, 010, 012, 014, 015, 017, 018, 020, 021, 023, 027, 028 |
| Outside PMS code (ERP or infrastructure) | 2 | REQ-019, 030 |
| Not assessed | 1 | REQ-031 |

**Main conflict.** Your rule is "use existing tables; do not create new tables". The cost-control requirements need records that have no existing table:
- cost codes;
- commitments (PO, subcontract, hire);
- postings;
- budget versions and transfers;
- change orders;
- certificates;
- snapshots.

No existing column has a matching meaning, and your rules forbid repurposing unrelated columns such as Issues, Extended_Timeline or Remarks. These 18 requirements therefore **cannot be implemented** until you decide D-1 in §4.

## 2. Existing structures that can be reused (meaning matches)

| Existing structure | Location | Reuse for |
|---|---|---|
| Project → Milestone → Task → Subtask (`Pms_tbl_AddProject`, `Pms_tbl_AddMilestone`, `Pms_tbl_Addtask`, `Pms_tbl_Addsubtask`) | Addproject / Addmilestone / Add_Task / AddSubtask .aspx.cs; assignprojects workspace | Operational hierarchy for REQ-001 (see D-2 for the cost-level mapping) |
| `Budget` / `Actual` on every level | Addproject.aspx.cs:622-623; Addmilestone.aspx.cs:121-134; Add_Task.aspx.cs:284-297; AddSubtask.aspx.cs:283-296 | Simple budget-vs-actual at each level (part of REQ-013). The Angular workspace saves these as NULL (assignprojects\default.aspx.cs:3845-3846, 4240-4241, 4524-4525). |
| Timers and worked hours (`Pms_tbl_Timerstatus`) | Add_Task.aspx.cs:711-767 | Hours quantity for labour cost (REQ-011). No rates exist. |
| Weighted progress `iscompletepercentage` roll-up | AddSubtask.aspx.cs:428-475 | Progress input for forecasts and dashboards |
| QC workflow engine (`WorkflowSetting`, `ActionMaster`) | pms\settings\critical\*.sql; pendingqc | Possible approval engine for REQ-016 (D-4) |
| Audit log (`pmstbl_Loginsert_usp`) | various | Audit for REQ-029 (procedure text needed) |
| Dashboards (admin, manager, user, team) | pms\*dashboard* | Display for REQ-024 |
| Excel export (Spire.XLS) | Downloadexcel.aspx.cs | Exports for REQ-025 |
| Customer / sales-order link | Addproject.aspx.cs:124, 4203 | Contract reference for REQ-022 (read-only) |
| `regid` tenant column | all tables | Tenant isolation (preserved) |

## 3. Requirement map

| Requirement | Existing screen / method | Existing table / columns | Status | Minimum change needed |
|---|---|---|---|---|
| REQ-001 Hierarchy: entity → project → subproject → phase → work package → task | Addproject, Addmilestone, Add_Task, AddSubtask; assignprojects | Pms_tbl_AddProject (id, regid), Pms_tbl_AddMilestone (ProjectId), Pms_tbl_Addtask (Milestoneid), Pms_tbl_Addsubtask (task) | Partial | Subproject: one column `ParentProjectId` on Pms_tbl_AddProject. Phase / work package: decision D-2. Entity: see REQ-026. |
| REQ-002 Group programme | — | — | Missing | New table, or `ProgrammeName` column (D-1) |
| REQ-003 Cost-code library and BoQ mapping | — | — | Missing | New tables (library, versions, BoQ map) |
| REQ-004 Mandatory dimensions | — | — | Missing | Depends on the cost-line table (REQ-003/007) |
| REQ-005 Split allocations to 100 % | — | — | Missing | New allocation table |
| REQ-006 Multi-currency | Customer / sales order only (ERP tables) | Pms_tbl_AddProject has no currency | Missing | Column `CurrencyCode` on Pms_tbl_AddProject. Amounts on new cost tables. |
| REQ-007 Recognition rules by cost type | — | — | Missing | New cost-posting table |
| REQ-008 Three reconciliations | — | — | Missing | Needs commitment, inventory and posting data |
| REQ-009 Commitment relief and matching | PO/GRN exist only in ERP notification code, not PMS | — | Missing | Commitment tables, or read from ERP (D-5) |
| REQ-010 FX analysis | — | — | Missing | Depends on REQ-006/007 |
| REQ-011 Daily labour cost and payroll true-up | Add_Task timers | Pms_tbl_Timerstatus (hours) | Partial | Rate source (D-6); new labour-cost table |
| REQ-012 Exceptions queue | — | — | Missing | New table |
| REQ-013 Baseline, revisions, transfers | Budget field on each level | Budget, Actual (type unknown) | Partial | Versions and transfers: new table. Workspace NULL save: fix only with approval (D-3). |
| REQ-014 Contingency reserves | — | — | Missing | New table |
| REQ-015 Change management | Issues module is **not** change orders | — | Missing | New table (Issues not repurposed) |
| REQ-016 Approvals and SoD | QC workflow, pendingqc | QC workflow tables | Partial | D-4: extend QC workflow or build a separate approval flow |
| REQ-017 Monthly snapshots | — | — | Missing | New table |
| REQ-018 Consolidated reporting | — | — | Missing | Depends on REQ-006/026 |
| REQ-019 Statutory integrations (GST, VAT, e-invoice, payroll) | — | — | ERP scope | Not in PMS code (D-5) |
| REQ-020 Subcontract certificates and retention | — | — | Missing | New tables |
| REQ-021 Equipment costing | — | — | Missing | **Not assumed.** May belong to the Assets module (D-7). |
| REQ-022 Client billing measures | Sales-order link | salesorderid on Pms_tbl_AddProject | Partial reference only | New table; ERP revenue (D-5) |
| REQ-023 Stores and wastage | — | — | Missing | **Not assumed.** May belong to ERP inventory (D-7). |
| REQ-024 Reports and dashboards | Dashboards; Reports menu commented out (MasterPage.master:63-71) | — | Partial | New report pages once the data exists |
| REQ-025 Exports | Downloadexcel.aspx | — | Partial | Extend export to new data |
| REQ-026 Accounting entity and JV | — | No entity column on project | Missing | Column `AccountingEntityId` on Pms_tbl_AddProject (needs an entity master: D-8) |
| REQ-027 Intercompany | — | — | Missing | New tables |
| REQ-028 At-risk work | — | — | Missing | Depends on REQ-015 |
| REQ-029 Security and audit | Cookie tenant (login.aspx.cs), audit log | regid | Partial | Hardening is a separate change; known SQL-injection and cookie issues |
| REQ-030 Recovery and backup | — | — | Infrastructure | Not a code change |
| REQ-031 Mobile / poor connectivity | Bootstrap pages | — | Not assessed | Needs a test on devices |

## 4. Decisions needed before any code change

| ID | Decision | Options |
|---|---|---|
| D-1 | New tables for cost control | (a) Approve new tables (list follows). (b) Limit scope to what existing tables support: §5 only. (c) Keep cost control in the selected ERP and show results in the PMS. |
| D-2 | Phase and work package | (a) Milestone = Phase, Task = Work package (needs business confirmation per project). (b) Add new levels. |
| D-3 | Angular workspace saves Budget/Actual as NULL | Is this intended, or a bug to fix? Fixing it changes current behaviour. |
| D-4 | Approvals | Reuse the QC workflow engine, or build a separate approval flow? |
| D-5 | ERP boundary | Which ERP was selected, and which data (PO, GRN, invoices, payroll, revenue) is read from it rather than stored in the PMS? |
| D-6 | Labour rates | Source of rates (HR, payroll, manual master)? |
| D-7 | Equipment and stores | Do these belong to the PMS, or to the Assets / Inventory modules? |
| D-8 | Accounting-entity master | Is there an existing company or entity table to reference? |

## 5. What could be done now with existing tables plus approved columns only

These are candidates only. Each needs your approval.

1. **Budget-vs-actual variance display** at Project, Milestone, Task and Subtask levels:
   - uses the existing `Budget` and `Actual`;
   - read-only calculation;
   - shown in the existing views.

   This needs DDL to confirm the data type, because the C# model holds it as a string.
2. **Budget/Actual save in the Angular workspace:** only if D-3 says it is a bug.
3. **Subproject:** `ParentProjectId INT NULL` on Pms_tbl_AddProject.
4. **Currency:** `CurrencyCode` on Pms_tbl_AddProject.
5. **Accounting entity:** `AccountingEntityId` on Pms_tbl_AddProject (after D-8).

The proposed columns (items 3–5) would be added as nullable, with no defaults that change existing rows. The save procedure would get optional parameters that default to NULL, so existing callers keep working. No drops, no truncation.

## 6. Inputs needed from you

1. Answers to D-1…D-8.
2. One of the following, for the PMS database:
   - a schema export (DDL) and stored-procedure definitions;
   - confirmation that I may read the schema read-only from a **non-production** database through the connected SQL tool.
3. A local or development URL with a test login, to verify save and reload.
4. Which of the duplicate or backup pages are live.
