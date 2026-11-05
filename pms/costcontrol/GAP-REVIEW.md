# Cost Control: gap review against the research requirements

**Date:** 2026-11-05
**Compared:** what is built in `/pms/costcontrol/` (Phase 1) against `requirements.md` v1.0-draft.
**Areas:** dimensions (§2), compliance (§5, §10, §13, SW-11) and reports (§6).

## Status key

| Status | Meaning |
|---|---|
| **Built** | Implemented and working as the requirement describes. |
| **Partial** | Some of it exists, or a simpler version exists. |
| **Missing** | Not built. |
| **Blocked** | Needs a business, Finance or ERP decision or input first. |

## 1. Dimensions

| Req | Requirement (short) | Phase 1 status | Gap |
|---|---|---|---|
| DM-01 | Every cost line carries legal/accounting entity | **Missing** | No entity on project or cost line. Projects have no entity field (RT-02). **Blocked:** entity master (D-8). |
| DM-01 | Project + **posting work package** | **Partial** | Lines link to project / milestone / task. There is no "posting work package" flag (HR-04), so cost can sit at any level. |
| DM-01 | Cost code | **Built** | — |
| DM-01 | Cost type (Materials, Labour, Subcontract, Equipment, Overhead) | **Partial** | Held on the cost code, not on the line. An extra type "Other" exists that is not in DM-01 (decision needed). |
| DM-01 | Site | **Missing** | No site on lines or entries. |
| DM-01 | Source document and line | **Partial** | Only a free-text Reference No. There is no document type or line reference, and no duplicate check. |
| DM-01 | Transaction currency, rate, rate date, rate source, functional amount | **Missing** | Single amount, no currency. **Blocked:** currency rules (Q1a, FIN). |
| DM-02 | Cost centre (optional on project cost) | **Missing** | **Blocked:** Finance category list. |
| DM-03 / REQ-005 | Split allocation totalling 100 % with link to source | **Missing** | Each entry goes to one code / milestone / task only. |
| CC-01 | Cost-code hierarchy Division → Section → Element | **Missing** | Codes are a flat list. |
| CC-02 | Entity extensions under group codes | **Missing** | Depends on entity. |
| CC-04 | Code changes versioned; old reports keep old version | **Missing, and a risk in Phase 1** | A code that is already used can still be renamed or retyped, which silently changes past reports. **Fix recommended.** |
| CC-05 | Client BoQ mapping | **Missing** | — |
| — | Unit (`tbl_unitmaster`) | **Built** | — |

## 2. Compliance, control and audit

| Req | Requirement (short) | Phase 1 status | Gap |
|---|---|---|---|
| AU-01 / NF-06 / ST-14 | Immutable history of changes, approvals, reversals, with reasons | **Partial** | Only created-by/modified-by stamps, and they are overwritten. Edits and deletes of cost entries and lines keep no history and need no reason. |
| AP-03 | No one approves their own document | **Missing** | The same admin can create and lock a budget. |
| §5.1 | System administrator has **no automatic financial approval rights** | **Conflict** | Phase 1 is admin-only, so the admin does everything. Role-based access is needed. |
| AP-01 / AP-10 | Configurable approval tiers for baseline, revision, transfers | **Partial** | Lock is one step with no value tiers. Revision reason is mandatory (built). |
| AP-07 | Cost/commitment above available budget needs approval | **Missing** | Over-budget entries are only visible in the report. |
| PE-01…08 / REQ-017 | Monthly period close, immutable snapshots, controlled reopen | **Missing** | Entries can be back-dated or edited in any month. |
| PE-02 | Four dates per record | **Partial** | One business date only. |
| SW-11 / ST-13 | Subcontractor compliance documents, expiry, payment hold | **Missing** | Needs the subcontract module first. |
| ST-05 | Tax attributes per line; recoverable tax excluded from cost | **Missing** | Amount is entered as-is. **Blocked:** Finance rule on what amount is entered. |
| ST-12 | Statutory levies as budget lines (e.g. India BOCW cess) | **Possible now** | Can be set up as a cost code; no code change needed. |
| ST-03 | Separate dates on certificates and bills | **Missing** | Subcontract / billing not built. |
| ST-14 / NF-07 | Retention period per country | **Missing** | Values TBP after professional validation. |
| NF-02 | Entity isolation | **Partial** | Company (`regid`) isolation is built; entity-level isolation needs entities. |
| NF-04 | SSO/MFA, RBAC | **Partial** | Uses the existing cookie login (known weakness from the earlier review). |
| NF-08 | Personal-data minimisation | **Built** | No personal data is stored in Cost Control. |

## 3. Reports

| Req | Requirement (short) | Phase 1 status | Gap |
|---|---|---|---|
| RP-01 | OB | **Built** | Original = first locked version. |
| RP-01 | ADJ, ACH, NTR → RB | **Partial** | "Current budget" = latest locked version. There is no breakdown into corrections, changes and transfers. |
| RP-01 | RCV, REM (commitments) | **Missing** | No commitments. **Blocked:** ERP PO/subcontract source (D-5). |
| RP-01 | INV earmarked | **Missing** | Blocked on ERP inventory. |
| RP-01 | PC, UA → IC | **Partial** | "Actual" = manual entries; no PC/UA split. |
| RP-01 | ETC, CTC, EAC, VAC | **Missing** | No forecast. Variance shown is RB − Actual, **not** VAC = RB − EAC. |
| RP-01 | PCE, PPC, EACp, RSV, VACp | **Missing** | Needs change management and reserves. |
| RP-01 | Roll-up by work package × cost code × **cost type** | **Partial** | Group by cost code or milestone only. No cost-type grouping and no multi-level roll-up. |
| RP-04 / RP-05 | Reserve report, change register | **Missing** | — |
| RP-06 / RP-19 | Transaction / functional / group currency views | **Missing** | Blocked on currency. |
| RP-07 | PMS vs ERP ledger reconciliation | **Missing** | Blocked on ERP integration. |
| RP-08 | Snapshot comparison between periods | **Missing** | Needs period close. |
| RP-15 | Dashboard data-freshness | **Missing** | Only relevant once integrations exist. |
| RP-16 | Summaries at project / subproject / entity / country / group | **Missing** | Only one project at a time. No multi-project summary. |
| RP-17 | Excel and PDF export with filters, period, version | **Missing** | No export. (Spire.XLS already exists in the PMS.) |
| RP-18 | Client extracts without confidential fields | **Missing** | — |
| RP-10 | Monthly report, weekly flash, monthly ETC sign-off | **Missing** | Needs ETC and period close. |

## 4. What can be built next with no outside dependency (proposed Phase 2)

> **Status 2026-11-05:** all 10 items are **built**. The code is in place but the database script has not been run yet. Details and verification are in `README.md`. The status tables in §1–§3 describe Phase 1. After Phase 2, these items are covered: CC-01, CC-04, AU-01/NF-06 (Cost Control scope), AP-03, AP-10 (simplified), §5.1 admin-approval conflict, PE-01/04/06 (simplified monthly lock), RP-01 (ETC/EAC/VAC, cost type), RP-16 (multi-project summary), RP-17 (Excel), and DM-01 (site as text, source document type).

None of these needs an ERP, Finance or statutory decision first.

| # | Item | Requirements | Database change |
|---|---|---|---|
| 1 | Block rename/retype of a cost code once used; keep code-change history | CC-04 | New history table |
| 2 | Cost-code hierarchy (Division → Section → Element) | CC-01 | `ParentId`, `Level` on `Pms_tbl_CostCode` (new table, ours) |
| 3 | Audit history of every create/edit/delete/lock, with mandatory reason on edit/delete of a saved entry | AU-01, NF-06 | New `Pms_tbl_CostControlAudit` (or reuse the existing PMS audit log, once its procedure is reviewed) |
| 4 | Maker-checker: budget submitted by one user, locked/approved by another | AP-03, AP-10 | `SubmittedBy/On` on the version table |
| 5 | Role-based access instead of admin-only (view / enter cost / approve) | §5.1, NF-04 | Uses the existing role-permission tables; needs a module ID |
| 6 | ETC per budget line; report adds EAC = Actual + ETC and VAC = Budget − EAC | RP-01 (partial), RP-10 | `ETC` columns (new table or on budget line) |
| 7 | Report grouping by cost type; multi-project summary page | RP-01, RP-16 | None |
| 8 | Excel export of each report, with project, filters, version and date in the header | RP-17 | None |
| 9 | Monthly period lock per project: no entry, edit or delete dated in a closed month; reopen with reason | PE-01, PE-04, PE-06 (simplified) | New period table |
| 10 | Site and source-document type on cost entries; duplicate-reference warning | DM-01 | New columns on our cost-entry table |

## 5. Decisions needed

| # | Decision | Needed for |
|---|---|---|
| 1 | Accounting entity master: is there an existing company/entity table to link projects to? (D-8) | DM-01, NF-02, entity reports |
| 2 | Currency: one currency per project, or per transaction? Who maintains rates? | DM-01, RP-06 |
| 3 | Which ERP data feeds commitments and actuals (PO, GRN, bills, payroll)? (D-5) | RCV/REM, PC, RP-07 |
| 4 | Is "Other" allowed as a cost type, or only the 5 types in DM-01? | DM-01 |
| 5 | Which roles may view, enter cost, and approve budgets? | AP-03, §5.1 |
| 6 | Are amounts entered net of recoverable GST/VAT? | ST-05, PC |
| 7 | Which month-end calendar and who may close and reopen a period? | PE-01…06 |
| 8 | Is the existing PMS audit log (`pmstbl_Loginsert_usp`) acceptable for cost-control history, or should cost control have its own? | AU-01 |
