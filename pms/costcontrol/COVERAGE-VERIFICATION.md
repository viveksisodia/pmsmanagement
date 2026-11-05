# Cost Control: coverage verification against the construction PMS research

**Date:** 2026-11-05

**Branch:** avdesh. Code state: Phase 1 + Phase 2 (commit `2b10e59`).

**Method:** read-only.
- Every requirement and acceptance criterion in `requirements.md` v1.0-draft was checked against the built code (procedures, WebMethods, screens).
- Read-only queries on the dev database (`absolutebook_2172`) confirmed that all Phase 2 objects are installed. This includes the fixed `usp_pms_cc_Period_SetStatus`.
- Nothing was changed.

**Status key**

| Status | Meaning |
|---|---|
| **Covered** | Works as the requirement states. |
| **Partial** | Some of it is built; the gap is stated. |
| **Missing** | Not built, but could be built without outside input. |
| **Blocked** | Needs a decision or input first; the decision is named. |
| **Out of PMS scope** | Belongs to the ERP, HR/payroll or infrastructure (DM-04, §12). |

> **Decision status.** In `business-decisions.md`, only **Q1-C1** is Confirmed. Everything the module implements rests on **Owner-proposed** decisions, which are **not yet confirmed**:
> - locked baseline (Q8a-P2)
> - maker-checker (Q14-P4)
> - monthly close (Q15a)
> - cost-code hierarchy (Q5-P1/P2)
>
> They need sign-off (`signoff-checklist.md` §A) before go-live.

---

## A. Summary

| Level | Covered | Partial | Missing (buildable) | Blocked | Out of scope | Total |
|---|---|---|---|---|---|---|
| REQ-001…031 | 0 | 10 | 6 | 13 | 2 | 31 |
| AC-01…52 | 2 (AC-24, AC-49; Cost Control scope) | 8 | 10 | 30 | 2 (AC-40, AC-52) | 52 |

**Overall picture**

- **What the module covers:**
  - the **budget-control core**: cost codes, approved budget versions, manual actuals, forecast, budget vs actual;
  - the **controls** around it: approval, audit, period lock, access.
- **What is still open:**
  - the **commitment and ERP side** of construction cost control: POs, subcontracts, receipts, invoices, reconciliations REC-A/B/C, FX, labour, equipment, stores, billing;
  - **entity, JV and intercompany** handling.
- **Why it is open:** all of these are blocked by decisions D-5, D-6, D-7, D-8 and the currency rules.

---

## B. Requirement coverage

### B.1 Prioritised requirements (requirements §1.2)

| Requirement | Short description | Status | Evidence | Gap / reason | Depends on |
|---|---|---|---|---|---|
| REQ-001 | Entity → Project → Subproject → Phase → Work package → Task | Partial | Lines, entries and ETC link to the existing milestone/task: `usp_pms_cc_ValidateHierarchy` (phase1.sql), called at phase2.sql:482, 648, 784 | No entity, subproject or phase. No designated posting work package (HR-04): cost may sit at project, milestone or task level. | D-2 (milestone = phase?), D-8 (entity) |
| REQ-002 | Group programme | Missing | — | — | — |
| REQ-003 | Cost-code library, extensions, versioning, BoQ | Partial | Hierarchy and lock: `usp_pms_cc_CostCode_Save` phase2.sql:294 | No entity extensions (CC-02). No version history usable by reports (CC-04 is met by blocking changes instead). No BoQ mapping (CC-05). | D-8 |
| REQ-004 | Mandatory dimensions | Partial | Cost code mandatory and Element-only (`ufn_pms_cc_IsPostingCode`); site (text) and source document (phase2.sql:640) | Missing: entity, transaction currency/rate, cost centre, source document **line**, site master | D-8, currency, DM-02 [FIN] |
| REQ-005 | Splits total 100 % | Missing | — | One entry goes to one code/milestone only | — |
| REQ-006 | Multi-currency | Blocked | — | Single amount, no currency | Currency rules (Q1a, FIN) |
| REQ-007 | Recognition rules by cost type | Partial | Manual cost entries: `usp_pms_cc_CostEntry_Save` | No PC/UA split and no rule per cost type. "Actual" = whatever is typed. | D-5 ERP source, Q6-F1 |
| REQ-008 | REC-A / REC-B / REC-C reconciliations | Blocked | — | Needs commitments, inventory and ERP postings | D-5 |
| REQ-009 | Commitment relief and matching | Blocked | — | No commitments | D-5, VD-19 |
| REQ-010 | FX analysis | Blocked | — | — | Currency |
| REQ-011 | Labour daily cost and true-up | Blocked | Existing PMS timers record hours only (not linked) | No rates | D-6 |
| REQ-012 | Exceptions queue | Missing / Blocked | — | Meaningful only with ERP feeds | D-5 |
| REQ-013 | Locked baseline, revisions, **zero-net transfers** | Partial | Draft → Submitted → Locked: phase2.sql:541, 560, 579. Revision copies last locked version and needs a reason (phase2.sql:424). | No **transfer** document (BV-3/BV-4). No RB breakdown OB + ADJ + ACH + NTR. A revision may change the total freely. | — |
| REQ-014 | Contingency reserves | Missing | — | — | RS-8 values TBP (build possible) |
| REQ-015 | Change management (classes, 4 statuses) | Missing | — | — | — |
| REQ-016 | Approvals and segregation of duties | Partial | AP-03 enforced in the procedure (phase2.sql:568). Admin has no automatic approval (PmsCostControl.cs:36). | No value tiers (AP-01, TBP), no SoD matrix (AP-04), no delegation (AP-09), no project-level roles | AP-01/AP-04 values TBP |
| REQ-017 | Monthly immutable snapshots, controlled reopen | Partial | Month lock on cost entries (`ufn_pms_cc_IsPeriodClosed`, phase2.sql:657, 723); reopen with reason | **No snapshot.** Budget approvals and ETC changes after a close still change that month's figures (PE-03, AC-20). | Finance calendar TBP (build possible) |
| REQ-018 | Group consolidated reporting | Blocked | Project Summary (phase2.sql:886) is company-wide only | — | Q1a-P5 [FIN], D-8 |
| REQ-019 | Country tax, e-invoicing, payroll, statutory | Out of PMS scope | — | ERP / tax service (IW-04, CB-05) | — |
| REQ-020 | Subcontract certificates, retention, advances, back-charges, compliance holds | Missing / Blocked | — | Large module; whether the PMS originates subcontracts is VD-19 | VD-19, D-5, Q10-F1 |
| REQ-021 | Equipment costing | Blocked | — | Could belong to the Assets module | D-7 |
| REQ-022 | Client billing measures | Blocked | Existing sales-order link (read-only) | — | Q13 [FIN], ERP |
| REQ-023 | Stores, wastage, counts | Blocked | — | Could belong to ERP inventory | D-7 |
| REQ-024 | Reporting cadence and report set | Partial | Budget vs Actual by code / milestone / type (phase2.sql:817); Project Summary; ETC, EAC, VAC | Missing: weekly flash, monthly pack, data freshness (RP-15), commitments, reserves, changes | — |
| REQ-025 | Exports with context; client extracts | Partial | `export.aspx.cs:166-171`: title, project, filter, version, user | No currency basis or snapshot version. No PDF. No client-extract mode (RP-18). | — |
| REQ-026 | Accounting entity and JV models | Blocked | — | — | D-8, Q1b-O3 |
| REQ-027 | Intercompany | Blocked | — | — | D-8, Q1b-O4 |
| REQ-028 | At-risk work | Missing | — | Depends on change management | — |
| REQ-029 | Security, isolation, audit, retention, data minimisation | Partial | regid isolation; insert-only audit (trigger phase2.sql:79); no personal data stored | Login is still the existing cookie (no SSO/MFA). Access is not per project. No retention configuration. | NF-07 values TBP |
| REQ-030 | RPO/RTO, backup, exit | Out of PMS scope | — | Infrastructure | Q16-O2 |
| REQ-031 | Mobile and offline | Missing | Pages are responsive (CSS media queries) | No offline capture | — |

### B.2 Detailed rules (requirements §2–§5, §13)

| Rules | Status | Evidence / gap |
|---|---|---|
| HR-01, HR-03, HR-06, HR-07 | Partial / Missing | Project-level roll-up only. No subproject, programme or migration. |
| HR-02, HR-08, HR-09 | Blocked | Accounting entity and JV (D-8). |
| HR-04, HR-05 (posting work package only) | Partial | Only the Element **cost code** is enforced. Any hierarchy level can carry cost (D-2). |
| CC-01 (hierarchy) | Covered | Division > Section > Element validated in phase2.sql:294. |
| CC-02 (entity extensions) | Blocked | Needs D-8. |
| CC-03 (group owner) | Covered | Only administrators manage codes (`Need.Admin`). The named person is still TBP. |
| CC-04 (versioning) | Partial | A used code cannot change (phase2.sql:294). Description edits still apply to past reports. No versions. |
| CC-05, CC-06 (BoQ) | Missing | — |
| DM-01 | Partial | See REQ-004. |
| DM-02 (cost centre) | Blocked | FIN category list. |
| DM-03 (splits) | Missing | — |
| DM-04 (master ownership) | Partial | Units are added to the ERP's `tbl_unitmaster` through the existing ERP procedure. Confirm the PMS may do this. |
| §3.1 measures OB, RB | Partial | OB = first locked version. "Current budget" = latest locked version, with no ADJ/ACH/NTR breakdown. |
| §3.1 measures RCV, FUL, REM, INV, PC, UA | Blocked | D-5. |
| §3.1 measures IC, ETC, CTC, EAC, VAC | Partial | EAC = Actual + ETC; VAC = Current − EAC (phase2.sql:871). Correct only while there are no commitments or inventory. See conflict D-2. |
| §3.1 measures PCE, PPC, EACp, VACp, RSV | Missing | Needs change management and reserves. |
| F-1 … F-4 | Partial | F-4 holds because there are no reserves. F-1 is not yet relevant (no REM). F-2 and F-3 need change management. |
| REC-A, REC-B, REC-C, BR-1…4 | Blocked | D-5. |
| RS-1 … RS-8 | Missing | Buildable. Values for RS-8 are TBP. |
| BV-1 | Partial | Baseline locked and kept. Corrections are full revisions, not adjustment documents. |
| BV-2 | Covered | Reason, approver, dates and history on every version (phase2.sql:389, 541-597). Attachments not supported. |
| BV-3, BV-4 | Missing | No transfer document. |
| BV-5 | Covered | Cost budget only; no revenue budget mixed in. |
| BV-6 | Covered | ETC is a separate table (`Pms_tbl_ProjectEtc`) and never touches budgets. |
| CM-1 … CM-7, AR-01 … AR-05 | Missing | Buildable. |
| MW-01 … MW-14 (materials) | Blocked | D-5, D-7. |
| LW-01 … LW-06 (labour) | Blocked | D-6. LW-05 is met within Cost Control, which holds no pay data. |
| SW-01 … SW-12, SC-1, SC-2 (subcontracts) | Missing / Blocked | VD-19, D-5, Q10-F1. |
| EW-01 … EW-10 (equipment) | Blocked | D-7. |
| IW-01 (non-PO workflow) | Partial | Manual cost entry has a reason-controlled edit, but no approval step. |
| IW-02 (exceptions) | Missing | — |
| IW-03 (idempotent integration) | Blocked | No integration yet. |
| IW-04 (tax from ERP) | Out of PMS scope | Tax treatment of the typed amount is undecided (ST-05). |
| CB-01 … CB-11 (billing) | Blocked | Q13 [FIN]. CB-07 inputs (cost, ETC, EAC) exist and could be fed later. |
| IC-01 … IC-09 (intercompany) | Blocked | D-8. |
| AP-03 | Covered | Enforced in the procedure (phase2.sql:568). |
| AP-01, AP-02, AP-04, AP-05, AP-06, AP-08, AP-09 | Missing / Blocked | Values TBP. |
| AP-07 | Missing | Cost over budget is only reported, not approved. |
| AP-10 | Partial | Baseline approval only. No transfers, releases or change statuses. |
| AU-01 | Partial | Insert-only history with before/after snapshots and reasons for Cost Control actions. Override and delegation are not applicable yet. |
| AU-02 | Partial | Returned budgets and discarded drafts are recorded in history. No change records yet. |
| AU-03 | Blocked | RT / professional validation (X-20). |
| PE-01, PE-03, PE-06 | Partial | Lock only. No snapshot or version n+1. |
| PE-02 | Partial | One date per entry; the research needs four. |
| PE-04 | Covered for entries | Closed month blocks entries. A late item must use an open month. No reference to the prior period. |
| PE-05, PE-07, PE-08 | Blocked / Missing | ERP periods; entity close. |
| RP-01 | Partial | Original, Current, Actual, ETC, EAC, VAC by code, milestone or type. No commitment columns. |
| RP-02, RP-03, RP-06, RP-07, RP-11 … RP-14, RP-19 | Blocked | See D-5, D-7, currency. |
| RP-04, RP-05, RP-08, RP-09 | Missing | — |
| RP-10 | Missing | No flash report or monthly pack. ETC sign-off is not tracked. |
| RP-15 | Missing | No integrations yet. |
| RP-16 | Partial | Project summary and budget/actual/ETC/EAC only. |
| RP-17 | Partial | See REQ-025. |
| RP-18 | Missing | — |
| NF-02 | Partial | Company isolation. No entity. |
| NF-04 | Partial | View/Enter/Approve per employee. Not per project or entity. No SSO/MFA. |
| NF-06 | Covered (Cost Control) | Audit table plus trigger. |
| NF-07 | Missing | Retention values TBP. |
| NF-08 | Covered (Cost Control) | Stores only employee ID and display name. |
| NF-12 | Partial | Excel exports. No full-entity export. |
| NF-13 | Missing | — |
| NF-01, NF-03, NF-05, NF-09, NF-10, NF-11, NF-14 | Out of scope / Blocked | Hosting and infrastructure. |
| ST-01 … ST-22 (statutory candidates, not approved) | Mostly Out of scope / Blocked | ST-12 (a levy as a budget line) can be done now as a cost code. ST-03 and ST-05 are blocked. |

---

## C. Acceptance criteria: could the module pass today?

| AC | Example | Can pass now? | Evidence / reason |
|---|---|---|---|
| AC-01 … AC-09, AC-11 | Ex 1–5, 7, 9 | No (Blocked) | Need commitments, receipts, invoices, inventory and FX (D-5, currency). |
| AC-10 | Ex 6 | No (Blocked) | Exceptions from ERP postings. |
| AC-12 | Ex 8 | No (Blocked) | Labour rates and true-up (D-6). |
| AC-13 | Ex 10 step 4 | No | No commitments. EAC = Actual + ETC cannot double-count REM today because REM does not exist. |
| AC-14 | Ex 10 steps 1, 3 | No (Missing) | Change management. |
| AC-15 | Ex 10 step 2 | No (Missing) | Reserves. |
| AC-16 | — | No (Missing) | Transfers. |
| AC-17 | Ex 10 | No (Missing) | Change management. |
| AC-18 | — | Partial | Own approval rejected: **pass** (phase2.sql:568). Delegation: not built. |
| AC-19 | — | No (Blocked) | Matching tolerances need commitments. |
| AC-20 | Ex 4 + late posting | No (Missing) | No snapshots. Approving a budget or changing ETC after a month close changes that month's report. |
| AC-21 | — | No (Blocked) | No integration. |
| AC-22 | — | No (Missing) | Splits. |
| AC-23 | Ex 7 | No (Blocked) | Currency. |
| AC-24 | Ex 8 | Pass (Cost Control scope) | No pay data in Cost Control. Labour costing is not built. |
| AC-25 | — | Partial | A non-Element cost code is rejected (`ufn_pms_cc_IsPostingCode`). A non-posting hierarchy level is not rejected. |
| AC-26 … AC-30 | Ex 11–13 | No (Missing / Blocked) | Subcontracts. |
| AC-31 | Ex 14 | No (Blocked) | Equipment (D-7). |
| AC-32, AC-33 | Ex 15 | No (Blocked) | Stores, intercompany. |
| AC-34 | Ex 16 | No (Blocked) | Billing / revenue. |
| AC-35, AC-36 | §4.1 | No (Blocked) | Materials. |
| AC-37 | Ex 10 | Partial | Project total = sum of lines at project level. No subproject, programme or reserve levels. |
| AC-38 | — | Partial | Used codes cannot change, so reports keep their figures. There is no snapshot to prove the closed-period re-run. No entity extensions. No BoQ. |
| AC-39 | Ex 6 | Partial | Cost code mandatory. Entity and currency dimensions do not exist. No ERP exceptions path. |
| AC-40 | — | Pass in effect / Blocked | Cost Control holds no tax rates. Commitments and certificates do not exist. |
| AC-41 | — | No (Missing) | Flash report, monthly pack, data freshness. |
| AC-42 | — | Partial | Filters, version and user are shown. Currency basis and snapshot version are not. Access is respected (View right). |
| AC-43 | — | No (Missing) | Client extracts. |
| AC-44 … AC-46 | — | No (Blocked) | Entity / JV / intercompany (D-8). |
| AC-47 | — | No (Missing) | At-risk. |
| AC-48 | — | Partial | Immutable audit and role checks are in place. SSO/MFA and per-project RBAC are not. |
| AC-49 | Ex 8 | Pass (Cost Control scope) | Minimal personal data only. |
| AC-50 | — | Partial | Excel per report. No full export. |
| AC-51 | — | No (Missing) | Offline. |
| AC-52 | — | Out of scope | RPO/RTO TBP. |

How these rows are counted in §A:

| §A status | Count | AC rows |
|---|---|---|
| Covered (Cost Control scope) | 2 | AC-24, AC-49 |
| Partial | 8 | AC-18, 25, 37, 38, 39, 42, 48, 50 |
| Missing (buildable) | 10 | AC-14, 15, 16, 17, 20, 22, 41, 43, 47, 51 |
| Out of scope | 2 | AC-40, AC-52 |
| Blocked | 30 | All other rows, including AC-13 (needs commitments) |

---

## D. Rule conflicts and label mismatches found in the built module

These are description-only corrections. Nothing has been applied.

| # | Severity | Finding | Where | Suggested correction |
|---|---|---|---|---|
| D-1 | High | **Closed months are not reproducible.** Closing a month locks cost entries only. A later budget approval, revision or ETC change still changes that month's Budget vs Actual (PE-03, AC-20). | `usp_pms_cc_Period_SetStatus` phase2.sql:976; `usp_pms_cc_BudgetVsActual` phase2.sql:817 | On close, store an immutable snapshot of the report figures. On reopen, create snapshot version n+1. Report "as at month X" from the snapshot. |
| D-2 | Medium | **EAC formula is a simplification.** Research: EAC = IC + REM + INV(earmarked) + ETC, and ETC **excludes** remaining commitments. The module uses EAC = Actual + ETC, so users will put ordered-but-undelivered value into ETC. Once commitments exist, that would double count (F-1). | phase2.sql:871, 921; Forecast tab hint | Until commitments exist, label the ETC help text as "includes committed but not yet incurred cost". When commitments are built, move that value out of ETC (F-1). |
| D-3 | Medium | **"Actual" is not IC.** There is no PC/UA split, and amounts may or may not include recoverable tax (Q6a-P3 / ST-05 undecided). | Cost Entries; report column "Actual" | Rename it "Actual (manual entries)" and add a policy note on tax once Finance decides. |
| D-4 | Medium | **"Current budget" is not RB.** A revision replaces the whole budget, so there are no ADJ/ACH/NTR components, and the total can change without a zero-net transfer (BV-1, BV-3). | `usp_pms_cc_BudgetVersion_Create` phase2.sql:424 | Phase 3 transfers and reserves: classify each revision line change as correction, change or transfer, and enforce zero-net for transfers. |
| D-5 | Medium | **No posting-level rule (HR-04).** Cost can be booked at project, milestone or task level, so the same cost scope can be spread across levels. | `usp_pms_cc_ValidateHierarchy` | Decide D-2, then enforce a single posting level per project. |
| D-6 | Medium | **Two "budget" figures per project.** The legacy free-text Budget/Actual fields on Project, Milestone, Task and Subtask still exist, and differ from Cost Control (this duplicates data, against your rule). | Addproject / Addmilestone / Add_Task / AddSubtask (unchanged) | Decide (D-3 in the coverage map) whether to hide those fields, or show the Cost Control totals there read-only. |
| D-7 | Medium | **Access is company-wide.** "Enter" or "Approve" applies to every project in the company (NF-04 asks for per project and entity). | `Pms_tbl_CostControlAccess` | Add an optional project list per access row. |
| D-8 | Low | **Future-dated cost entries are accepted.** There is no check that EntryDate ≤ today. | `usp_pms_cc_CostEntry_Save` phase2.sql:640 | Reject or warn on future dates. |
| D-9 | Low | **Approval right is checked only in C#.** The lock procedure checks "not the submitter" but not "has approve right". | phase2.sql:560; PmsCostControl.cs:36 | Also check `Pms_tbl_CostControlAccess.CanApprove` inside the procedure (defence in depth). |
| D-10 | Low | **Export header is incomplete.** It lacks currency basis and period/snapshot version (RP-17). | export.aspx.cs:166-171 | Add a "Currency: single, not converted" line, and the snapshot version once D-1 is built. |
| D-11 | Low | **Months can be closed in any order.** A later month can be closed while earlier months stay open. | phase2.sql:976 | Optionally require earlier months to be closed first (Finance calendar decision). |
| D-12 | Info | **Cost type "Other" is not in DM-01** (five types). | phase2.sql:294 | Decision #4 in GAP-REVIEW. |
| D-13 | Info | **"+ Add new unit" writes to the ERP's `tbl_unitmaster`.** It uses the existing ERP procedure; DM-04 ownership is not confirmed. | `usp_pms_cc_Unit_Add` | Confirm the PMS may add units, or restrict this to the ERP. |

---

## E. Statements in README / GAP-REVIEW that overstate coverage

| File | Statement | Correction |
|---|---|---|
| GAP-REVIEW.md §4 status note | "PE-01/04/06 (simplified monthly lock)" covered | PE-01 and PE-06 require immutable snapshots and version n+1. Only the entry lock exists. These are **Partial**. |
| GAP-REVIEW.md §4 status note | "AP-10 (simplified)" covered | Only baseline approval exists. Transfers, contingency releases and change statuses are not built. This is **Partial**. |
| README.md, table row for AU-01 | "Every … change … is recorded" | True for Cost Control actions only. Existing PMS screens and the legacy Budget field are not covered. |

---

## F. Recommended Phase 3 scope, in priority order

### (i) Buildable now (no outside decision)

1. **Fix D-1, D-8, D-9, D-10:**
   - monthly **snapshots** on close, with version n+1 on reopen (REQ-017, AC-20);
   - future-date check;
   - approval check inside the procedure;
   - export header.
2. **Budget transfers and contingency reserves:**
   - zero-net transfers;
   - reserve type held once;
   - releases and returns with reasons and approval;
   - RB = OB + ADJ + ACH + NTR;
   - RB + RSV constant;
   - reserve report.

   Covers REQ-013, REQ-014, BV-3, RS-1…7, RP-04, AC-15, AC-16. RS-8 values are left blank, with no default percentage.
3. **Change management and at-risk:**
   - change register with the six classes and four independent statuses;
   - PCE / PPC / EACp per F-2 and F-3;
   - at-risk flag and exposure.

   Covers REQ-015, REQ-028, CM-1…7, AR-01…05, RP-05, AC-14, AC-17, AC-47.
4. **Split allocation** of a cost entry, totalling exactly 100 % with a source link (REQ-005, DM-03, AC-22).
5. **Per-project access** (D-7) and a **"Pending approvals" list** across projects.
6. **Label corrections** D-2 and D-3. Text only.

### (ii) Blocked: decision or input needed

| Decision / input | Unblocks | Owner (per signoff-checklist) |
|---|---|---|
| D-5: ERP source for PO, GRN, invoices, payroll, revenue; whether the PMS originates subcontracts (VD-19) | REQ-007…010, 012, 020; REC-A/B/C; AC-01…11, 19, 21, 26…30 | Business owner + ERP vendor |
| Currency rules (Q1a-P2…P5) | REQ-006, 010, 018; AC-11, 23 | Finance |
| D-8: accounting-entity master; JV structures (Q1b-O3) | REQ-001 (entity), 026, 027; AC-44…46 | Finance / legal |
| D-2: milestone = phase? posting work-package level | HR-04, AC-25 | Business owner |
| D-6: labour rate source | REQ-011, AC-12 | HR / Finance |
| D-7: equipment and stores in the PMS or the Assets/Inventory modules? | REQ-021, 023; AC-31, 32, 35, 36 | Business owner |
| Tax treatment of entered amounts (ST-05) | REQ-007, D-3 | Finance |
| D-3: legacy Budget/Actual fields | D-6 | Business owner |
| Approval tiers, SoD matrix, delegation values (AP-01, AP-04, AP-09) | REQ-016 | Finance / management |
| Billing and revenue policy (Q13-F1) | REQ-022, AC-34 | Finance |

---

## G. Questions for you

1. **Phase 3 scope:** should it be items F(i) 1–6 in that order, or a subset?
2. **Legacy budget fields (D-6):** should they be hidden or replaced by read-only Cost Control totals? Or should both be left as they are?
3. **Units (D-13):** may the PMS add units to `tbl_unitmaster`, or should units be added only in the ERP?
4. **ETC (D-2):** until commitments exist, may users include ordered-but-not-received value in ETC (with the label changed)? Or should it be excluded now?
5. **Access (D-7):** should Enter/Approve rights be limited to selected projects?

---

## H. Regression risk: existing PMS files changed

| File | Change | Possible effect |
|---|---|---|
| `pms/angularmaster/angular_head.master` | The "Cost Control" link condition changed from `chkisadmin == 1` to `iscostcontrol == 1` (4 lines in total) | Only the visibility of the new link. |
| `pms/angularmaster/angular_head.master.cs` | +1 field. +1 call, `PmsCostControl.ShowMenuLink()`, wrapped so it never throws. | Adds 2 small database queries (admin check and access lookup) to **every page load** that uses this master. Low performance cost; worth watching. |
| `App_Code/PmsCostControl.cs` (new) | New shared class | Any App_Code change recompiles the whole site on deploy. This means a one-time application restart. |
| Database | New tables, columns and procedures only. Units are inserted through the existing `tbl_unitmasterInsertNew` procedure. | No existing table, column or procedure was altered. |

Not regression-tested on the live site: the existing screens (Projects, Tasks, QC, Issues, dashboards). Recommended: open each main screen once after deployment to confirm the header renders and the timings are normal.
