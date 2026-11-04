# RT-02 — Existing PMS Inspection (read-only)

**Date:** 2026-11-03
**Scope:** Source code and documentation in `D:\pmsmanagement` only.
**Method:** Files were read with search and read tools only.
- No database, build, network, production system or credentials were accessed.
- No files were modified.

**Status:** **PARTIALLY BLOCKED** (confirmed by the business owner on 2026-11-03). The full inspection is not complete. The data model cannot be confirmed from source; see "Evidence limits" and §9.

## Evidence limits
- **Data access layer is not in the repo.** `DataAccess` / `CommonFunction` come from the compiled `bin\CommonClass.dll`. Its source is absent.
- **Core tables have no DDL.** These tables have no `CREATE TABLE` in the repo: `Pms_tbl_AddProject`, `Pms_tbl_AddMilestone`, `Pms_tbl_Addtasklist`, `Pms_tbl_Addtask`, `Pms_tbl_Addsubtask`, `Pms_tbl_Addissues`, `Pms_tbl_Timerstatus`, `Pms_tbl_assigntasks`.
- **Save procedures are not in the repo.** `pmstbl_Addproject_usp`, `pmstbl_Addmilestone_usp(_ang)`, `pms_Adtask_usp(_ang)`, `pms_Add_Subtask_usp(_ang)`, `pms_AddiSSUES_usp`, the timer and extend-date procedures, and `sp_CopyProjectData` have no `CREATE PROC`. Columns are inferred from procedure parameters and inline SQL.
- **Only QC workflow and critical-label DDL exist.** They are under `pms\settings\critical\` (`qc workflow tbls`, `critical workflow tbls`, `ActionMaster.sql`, `SimpleMaster.sql`), with backup copies in `bkp6`–`bkp13`.
- **Inputs needed to complete RT-02:**
  - A schema export (DDL) of the PMS database.
  - Definitions of the stored procedures listed above.
  - Source code of `CommonClass`.
  - Confirmation of which page versions are live.

## 1. Hierarchy (confirmed in source)

| Level | Table | Link keys | Evidence |
|---|---|---|---|
| Project | Pms_tbl_AddProject | id, regid, customerid, salesorderid | pms\Addproject.aspx.cs:688-689, 726 |
| Milestone | Pms_tbl_AddMilestone | ProjectId; istask flag | pms\Addmilestone.aspx.cs:94, 141, 148 |
| Tasklist | Pms_tbl_Addtasklist | ProjectId, Milestone | pms\Addtasklist.aspx.cs:57-58, 103 |
| Task | Pms_tbl_Addtask | ProjectId, Milestoneid, tasklist; issubtask flag | pms\Add_Task.aspx.cs:232-234, 352, 369 |
| Subtask | Pms_tbl_Addsubtask | ProjectId, Milestoneid, tasklist, task | pms\AddSubtask.aspx.cs:232-235, 359 |
| Issue | Pms_tbl_Addissues | ProjectId + Issueon ('Project', 'milstone', 'task', 'subtask') | pms\assignprojects\default.aspx.cs:12316-12324 |

- **Optional levels:** the project flags `hastask` and `hassubtask` (pms\Addproject.aspx.cs:690-713).
- **Tasklist appears retired.** Its menu entry is commented out (pms\MasterPage.master:45-47), and the progress roll-up skips it.
- **Assignments** are stored in `Pms_tbl_assigntasks` (pms\assignprojects\default.aspx.cs:6025).
- **Templates and project copy:** `usp_pms_projecttemplates_all` and `sp_CopyProjectData`.
- **Not found:** subproject, phase, work package, cost code, WBS code, and legal entity per project.

## 2. Cost and progress fields

| Item | Finding | Evidence |
|---|---|---|
| Budget / Actual | One free-entry value per row at every level | Addproject.aspx.cs:622-623; Addmilestone.aspx.cs:119-135; Add_Task.aspx.cs:282-298; AddSubtask.aspx.cs:281-297 |
| Budget / Actual in the Angular workspace | Saved as NULL for milestones, tasks and subtasks | assignprojects\default.aspx.cs:3845-3846, 4240-4241, 4524-4525 |
| Budget-vs-actual calculation | **Not found** (no variance, overrun or sum logic) | — |
| Progress | Weighted `iscompletepercentage` rolls up subtask → task → milestone → project | AddSubtask.aspx.cs:428-475 |
| Hours | Start/pause timers with worked-hours sum. No rates, and no timesheet entity. | Add_Task.aspx.cs:711-767, 3879 |
| Dates | Duration, timeline, extended dates with history (`Pms_Tbl_Extended_Timeline`) | Add_Task.aspx.cs:2084; assignprojects:8667 |
| Sales-order link | A project can reference a customer and a sales order from ERP tables | Addproject.aspx.cs:124, 4203 |

## 3. Tenancy and entity model
- **Tenant key:** `regid` comes from a cookie, and the tenant database is chosen by a `database` cookie (login.aspx.cs:71-72, 180-181; App_Code\dataaccessruntime.cs:68-72).
- **Branch and financial year** exist only as login cookies.
- **Country:** the project has address and country fields. The company is per regid.
- **Currency:** exists only on customer and sales-order records. Projects have no currency and no legal-entity field.

## 4. Modules present
- **Projects:** current version is `pms\projects`; a legacy version exists.
- **Project workspace:** `pms\assignprojects`, about 16k lines.
- **Milestone, task and subtask pages**, and inline grid editing in `makemilestone`.
- **Issues**, with masters for status, type and replicability.
- **Dependencies:** `tbl_dependencyP/M/T/ST`.
- **Gantt:** dhtmlx.
- **QC workflow:** `pendingqc`, `WorkflowSetting`.
- **Critical labels.**
- **Notification templates.**
- **Dashboards:** admin, manager, user and team.
- **Team scheduling:** `myteamtask`.
- **Settings masters**, including holidays and weekly off.
- **HR employee page:** hidden from the menu.
- **Audit log** via `pmstbl_Loginsert_usp`.
- **Commented-out menu items** with no pages behind them: Expenses, Purchase, Reports (MasterPage.master:63-71).

## 5. Integrations
| Integration | Status | Evidence |
|---|---|---|
| External QC workflow API (`{gmailapi}/api/manageqcworkflows/...`) | Active | pendingqc\default.aspx.cs:514-543 |
| SMTP email | Code present; no call found from PMS task pages | App_Code\SendMail.cs |
| Gmail/WhatsApp/Telegram relay | In CRM conversion and sales-order pages, not in PMS task flows | convertoproject.aspx.cs; viewsalesorder.aspx.cs:7964 |
| SMS | Present, with hard-coded gateway credentials (security issue) | App_Code\sendsms.cs:161 |
| ERP/CRM | Read-only customer and sales-order lookup; CRM code not in repo | Addproject.aspx.cs:4203 |
| Crystal Reports | Configured only; no reports exist (dead) | Web.config |
| Excel | Export template (Spire.XLS); import commented out | Downloadexcel.aspx.cs; Addnewproject.aspx.cs:3317 |

## 6. Documentation
- **README.md** contains only the project name.
- **Small component READMEs** exist for `myteamtask`, `teamdashboard` and `newuserdashboard`, plus `pms\projects\VERIFY.txt`.
- **No architecture, schema or API documentation** exists.

## 7. Gap summary against the requirements draft

| Area | Evidence in existing PMS |
|---|---|
| Hierarchy with posting work package; subproject/phase | Not found. Fixed Project/Milestone/(Tasklist)/Task/Subtask keys. |
| Cost codes, cost types, dimensions | Not found |
| Budget versions, transfers, contingency, change orders | Not found. One Budget field per row. |
| Commitments (PO/subcontract/hire), receipts, inventory | Not found in PMS. PO/GRN references exist only in shared ERP notification code. |
| Subcontract claims, retention | Not found |
| Labour rates / payroll costing | Not found. Timers record hours only. |
| Equipment costing | Not found |
| Multi-currency, legal entity per project | Not found |
| Accruals, reconciliation, period close | Not found |
| ERP posting integration | Not found. Read-only sales-order/customer lookup only. |
| Progress roll-up, dependencies, Gantt, QC workflow, critical labels, notifications | **Present.** These could inform the operational side of the new design. Reuse is not assumed. |

## 8. Migration considerations (analysis; no migration proposed)
- **No direct mapping from Milestone/Task/Subtask to Phase/Work package/Task can be assumed (Q4-O1).** Existing levels carry no cost-code or posting semantics, so any mapping would be a business decision made per project.
- **Existing Budget/Actual values are single numbers.** Their meaning is unknown, and the Angular workspace saves them as NULL. Migrating them as an "original budget" would need business validation.
- **The data model can't be fully confirmed.** Inline SQL (about 5,566 calls), stored procedures that aren't in the repo, and soft deletes with no visible foreign keys all prevent it. RT-02 is **partially blocked** until the inputs listed above are provided.
- **Security findings from the earlier review remain relevant to any reuse:** cookie-based tenant identity, SQL concatenation, and plaintext secrets.

## 9. RT-02 status: inspected, verified, unknown, dependent

### 9.1 What was inspected
- All `.aspx`, `.aspx.cs`, `.ascx`, `.master`, `.ashx`, `.js`, `.sql` and `.txt` files under `D:\pmsmanagement`, including `pms\`, `App_Code\`, the root login/logout pages and `Web.config`.
- Method: search and file reading only.
- **Not inspected:**
  - any database
  - `bin\CommonClass.dll` internals
  - production systems
  - credentials
  - the CRM/ERP code referenced by the PMS but not in the workspace

### 9.2 What was verified from source
- The hierarchy tables named in §1, and the link keys passed in code.
- Budget/Actual fields on each level, and the absence of any budget-vs-actual calculation in the page code.
- The weighted progress roll-up logic in page code.
- Timers (hours only), dependencies, Gantt, QC workflow (including its DDL), critical labels (including their DDL), notification templates.
- The tenant cookie model.
- The read-only sales-order link.
- Integrations listed in §5.
- Absence in the **page code** of cost codes, commitments, inventory, rates, currency per project, accruals and period close.

### 9.3 What remains unknown (blocked by missing inputs)
| Unknown | Missing input |
|---|---|
| Actual table structures, data types, constraints and indexes of the core tables | Database schema export (DDL) |
| Logic inside save, copy, dependency, timer and QC procedures (e.g., `pmstbl_Addproject_usp`, `sp_CopyProjectData`, `usp_Pms_QCActionGrid_Get`) | Stored-procedure definitions |
| Data-access behaviour: connection handling, tenant database switching, encryption/decryption, cookie handling inside `UserAuthentication.SetCookie` | `CommonClass` source |
| Whether any cost, budget or financial logic exists in the database layer that is not visible in page code | DDL + stored procedures |
| Which duplicate or backup pages are live | Confirmation of deployed page set |
| Meaning and quality of existing Budget/Actual data | Business confirmation (not production access) |

### 9.4 Requirements that depend on the missing inputs
| Requirement | Dependency |
|---|---|
| HR-07 / Q4-O1: migration of existing hierarchy records | DDL, stored procedures, live-page confirmation |
| Any proposal to reuse existing progress roll-up, dependencies, QC workflow or critical labels | Stored procedures (behaviour), DDL |
| Migration of existing Budget/Actual values as baselines | DDL plus business confirmation of data meaning |
| Integration with the existing tenant model (regid / database selection) | `CommonClass` source |
| Reuse of the existing audit log (`pmstbl_Loginsert_usp`) against AU-01…03 and candidate ST-14 | Stored-procedure definition |
| Security remediation scope if any existing component is reused | `CommonClass` source, DDL |

| Feasibility of the preferred direction "extend or develop the PMS" (Q17-P1, X-30) | DDL, stored procedures, `CommonClass` source |

The requirements for the new cost-control capabilities (§2–§13 of requirements.md v1.0-draft) do **not** depend on these inputs. Research that does not depend on them continues.

**What this document does not claim:**
- It does not claim the full inspection is complete.
- It does not describe database behaviour beyond what page code shows.
- It does not assume any existing functionality that was not found in source.

The missing inputs are tracked in `signoff-checklist.md` §E.
