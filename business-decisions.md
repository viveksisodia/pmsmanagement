# Business Decisions Register — Construction PMS

**Source:** `research.md` §10.1 (17 open business questions) and §10.2 (vendor-demo conflicts, tracked separately in `vendor-demo-checklist.md`).
**Started:** 2026-11-03 · **Last updated:** 2026-11-03 (Batch 1 and Batch 2 answers recorded)
**Rule:** Research findings (research.md) are *evidence*, not decisions. Only items marked **Confirmed** are approved requirements. Owner-proposed items are the business owner's intended direction but not yet final. Analyst suggestions are recorded as **Analyst-proposed** and are never treated as approved.

## Status legend

| Status | Meaning |
|---|---|
| **Confirmed** | Approved requirement stated by the business owner. |
| **Owner-proposed** | Direction given by the business owner, explicitly to be treated as proposed (not final). |
| **Analyst-proposed** | Recommendation from the analyst; not approved. |
| **Open** | Not yet answered, or an unknown business fact. |
| **Asked** | Presented to the business owner; awaiting answer. |
| **Needs finance approval** | Requires sign-off by Finance before it can be confirmed. |
| **Needs research** | Requires current statutory/vendor research before rules can be defined. |
| **Deferred** | Owner chose to decide later; dependent items stay blocked. |

## Classification

**A. Business decisions the owner must answer:** Q1, Q2, Q3, Q4, Q5, Q8, Q10, Q11, Q12, Q14, Q15, Q16, Q17.
**B. Facts requiring vendor verification:** all §10.2 items (VD-01 … VD-17 in `vendor-demo-checklist.md`), plus vendor components of Q1 (localisation) and Q16 (deployment).
**C. Decisions that depend on earlier answers:**

| Question | Depends on | Why |
|---|---|---|
| Q5a Dimension set | Q1b, Q3, Q4 | Dimension ownership splits between PMS and ERP. |
| Q6 When cost becomes actual | Q3, Q1, Q13 | Ledger owner and accounting standard decide accounting recognition; management view can differ. |
| Q6a Accounting actual vs management cost | Q3, Q6 | Requires known ledger owner. |
| Q6b Matching, accruals, reversals, double counting | Q3, Q6, Q10, Q1a | Depends on where POs/invoices live, certification practice and FX rules. |
| Q7 Labour cost source | Q1, Q3 | Payroll country/system and integration direction. |
| Q8 / Q8a Contingency, revisions, transfers | Q9, Q14 | Uses CO classes and approval matrix. |
| Q9 Change order process | Q4, Q5, Q14 | COs attach to hierarchy and cost-code lines. |
| Q10 Subcontract practice | Q1, Q13 | Retention/advances partly statutory and mirror client terms. |
| Q13 Client billing & revenue recognition | Q1, Q3 | Accounting standard per country; revenue in system of record. |
| Q15 Reporting cadence | Q6, Q13 | Cut-off depends on accrual and revenue processes. |
| Q16 Deployment / data residency | Q1, Q3 | Country law and build/buy path. |
| Q17 Budget & implementation tolerance | Q2, Q3 | Size and path drive cost. |

## Batch plan

| Batch | Questions | Status |
|---|---|---|
| 1 | Q1 (+Q1a, Q1b), Q3, Q2, Q4 | **Answered** 2026-11-03 |
| 2 | Q5 (+Q5a), Q6, Q6a, Q6b, Q7 | **Answered** 2026-11-03 |
| 3 | Q8a, Q8, Q9, Q14, Q15a (period close, pulled forward from Q15) | **Answered** 2026-11-03 (values TO BE PROVIDED) |
| 4 | Q10, Q11, Q12, Q13 | **Answered** 2026-11-03 (values TO BE PROVIDED; Finance items open) |
| 5 | Q15, Q16, Q17, plus outstanding Q1b-O1 (JV), Q1b-O2 (intercompany), Q9-O1 (at-risk) | **Answered** 2026-11-03 (values TO BE PROVIDED) |

---

## Register

### Q1 — Countries of operation and statutory requirements
- **Original (research.md §10.1 #1):** "Countries of operation and statutory requirements (GST/VAT, e-invoicing, payroll, labour law) — affects payroll and billing fit."
- **Type:** A + B. **Research refs:** §1 finding 6; §9.4; §4.6–4.8.

| ID | Item | Status |
|---|---|---|
| Q1-C1 | Support **India and UAE**, with the ability to add **additional countries** through configuration. | **Confirmed** (2026-11-03) |
| Q1-P1 | Country-specific tax, invoicing, payroll and statutory **integrations** are provided (not hard-coded in core logic). | Owner-proposed |
| Q1-R1 | Current Indian and UAE requirements (tax, e-invoicing, payroll, labour law, retention/withholding rules) must be researched and verified **before** compliance rules are defined. One country's rules must not be applied to another. | **Needs research** (open research task RT-01) |
| Q1-F1 | Accounting standards per entity/country (e.g., Ind AS / IFRS), exchange-rate sources and consolidation policies. | **Needs finance approval** |

### Q1a — Currencies
| ID | Item | Status |
|---|---|---|
| Q1a-P1 | INR and AED initially, plus additional transaction and reporting currencies. | Owner-proposed |
| Q1a-P2 | Each legal entity has its own functional currency. | Owner-proposed |
| Q1a-P3 | Purchasing, subcontracting and customer billing allowed in foreign currencies. | Owner-proposed |
| Q1a-P4 | Store original-currency amount, exchange rate (with rate date/source) and functional-currency amount on every monetary record. | Owner-proposed |
| Q1a-P5 | Consolidated reporting in a selected group reporting currency, with documented translation rules. | Owner-proposed; rules **Needs finance approval** |
| Q1a-O1 | Rate used for commitments (PO-date rate vs periodic revaluation) and how FX differences between commitment and actual are shown. | Open → asked under Q6b |

### Q1b — Legal entities
| ID | Item | Status |
|---|---|---|
| Q1b-P1 | Multiple legal entities per country. | Owner-proposed |
| Q1b-P2 | Each project belongs to exactly one legal entity; subprojects stay within that entity. | Owner-proposed |
| Q1b-P3 | Multi-company developments use separate linked projects under a group-level **programme**. | Owner-proposed |
| Q1b-P4 | Joint ventures supported; treatment follows the JV's actual legal and accounting structure (not assumed to be a separate company). | Owner-proposed |
| Q1b-O1 | How an unincorporated JV/consortium project is owned and reported (see contradiction X-1). | **Answered in direction 2026-11-03** → Q1b-P5…P9 |
| Q1b-O2 | Inter-company transactions (one entity buying, hiring or lending staff/equipment for another entity's project). | **Answered in direction 2026-11-03** → Q1b-P10…P17 |
| Q1b-P5 | JV arrangements supported without confusing legal entities with reporting structures. | Owner-proposed |
| Q1b-P6 | Where a JV has its own books: represent the appropriate **accounting entity** or ledger arrangement, subject to finance/legal approval. | Owner-proposed |
| Q1b-P7 | Where the lead partner accounts for the project: assign it to that accounting entity and record JV participation separately. Where partners keep separate project accounts: linked projects under a programme. | Owner-proposed |
| Q1b-P8 | Partner shares with effective dates, agreement references and approved reporting rules. | Owner-proposed |
| Q1b-P9 | No automatic consolidation from ownership percentage or the label "JV"; Finance determines accounting treatment. | Owner-proposed |
| Q1b-C2 | **Clarification of Q1b-P2:** "each project belongs to one legal entity" becomes "each project belongs to exactly one **accounting entity**", which is either a legal entity or a Finance/legal-approved accounting entity (for example a separately accounted non-company JV). | Owner-proposed clarification |
| Q1b-P10 | Controlled intercompany agreements for materials, equipment and staff. Each specifies supplying and receiving entities; projects and allocations; service or transfer period; pricing method and currency; tax and invoicing treatment; approvals; linked documents and settlement terms. | Owner-proposed |
| Q1b-P11 | Pricing approved by finance/tax; no universal cost-plus, market or book-value rule. | Owner-proposed |
| Q1b-P12 | Commercial pricing, accounting carrying values, VAT/GST valuation, transfer pricing and corporate-tax relief are distinguished and verified separately. | Owner-proposed → research RT-06 |
| Q1b-P13 | No automatic purchase commitment for every intercompany allocation; the required commitment, billing and journal documents are defined per transaction type and ERP capability. | Owner-proposed |
| Q1b-P14 | Both entities' records reconciled, with timing and FX differences identified. | Owner-proposed |
| Q1b-P15 | Entity-level results kept separate from consolidation adjustments; intercompany balances, transactions and unrealised profit eliminated where required by the approved consolidation policy. | Owner-proposed |
| Q1b-P16 | Cross-border movements require separate customs and regulatory review; a domestic workflow is not assumed sufficient. | Owner-proposed |
| Q1b-O3 | Actual JV structures and agreements. | Open — TO BE PROVIDED |
| Q1b-O4 | Intercompany pricing policies and approvers. | Open — TO BE PROVIDED |
| Q1b-C3 | **Correction:** the analyst statement that "UAE book-value pricing is available only under a qualifying-group election" is withdrawn. Each valuation dimension is to be verified separately (RT-06). | Correction recorded |

### Q2 — Company size
- **Original (#2):** "Company size: number of concurrent projects, subprojects per project, users (office vs site), transaction volumes."

| ID | Item | Status |
|---|---|---|
| Q2-P1 | Requirements must support multiple companies, countries, projects and sites. | Owner-proposed |
| Q2-O1 | Concurrent projects | Open (unknown) |
| Q2-O2 | Subprojects per project | Open (unknown) |
| Q2-O3 | Office users | Open (unknown) |
| Q2-O4 | Site users | Open (unknown) |
| Q2-O5 | Monthly POs, timesheet entries, subcontractor claims | Open (unknown) |
| Q2-O6 | Numerical capacity targets and performance acceptance criteria | Open — blocked until volumes provided. No estimates to be presented as facts. |

### Q3 — Build vs buy; financial system of record
- **Original (#3):** "Build vs buy: is the goal to extend our existing PMS (and integrate with an ERP for accounting), or to select one of these ERPs? Which ERP/accounting system is the financial system of record?"

| ID | Item | Status |
|---|---|---|
| Q3-P1 | Direction: option (a) — the PMS manages construction operations, project budgets, commitments, materials, manpower allocation, subcontractors, equipment, progress and management cost reporting. | Owner-proposed |
| Q3-P2 | An accounting system/ERP holds the official GL, payables, receivables, tax and statutory accounts. | Owner-proposed |
| Q3-P3 | Define ownership of every transaction and master record, integration timing, reconciliation and duplicate prevention. | Owner-proposed (ownership matrix to be drafted — see Q5a, Q6b) |
| Q3-P4 | Clearly distinguish operational cost estimates and accruals from posted accounting actuals. | Owner-proposed (detailed under Q6a) |
| Q3-O1 | ERP/accounting system to be used. | Open — **no ERP selected**; selection requires explicit instruction. |
| Q3-O2 | Current PMS capabilities and the existing accounting system. | Open — **unconfirmed; assume nothing exists.** |

### Q4 — Hierarchy depth; subprojects across companies/sites
- **Original (#4):** "Hierarchy depth needed (project → subproject → phase → package → activity → task?) and whether subprojects can belong to different companies/sites."

| ID | Item | Status |
|---|---|---|
| Q4-P1 | Hierarchy: **Legal entity → Project → Subproject → Phase → Work package → Task**. | Owner-proposed |
| Q4-P2 | Optional group-level **programme** for reporting across projects and legal entities. | Owner-proposed |
| Q4-P3 | Up to **two nested subproject levels** initially; limitations to be explained before finalising (see note L-1). | Owner-proposed (pending limitation review) |
| Q4-P4 | A subproject may have a different site from its parent but stays in the same legal entity. | Owner-proposed |
| Q4-P5 | Detailed budgets and costs recorded against a designated **posting work package × cost code × cost type**. | Owner-proposed |
| Q4-P6 | Cost types: materials, labour, subcontractors, equipment, overheads. | Owner-proposed |
| Q4-P7 | Task-level operational records roll up to their work package without posting cost twice. | Owner-proposed |
| Q4-P8 | Budgets, commitments, actuals and forecasts roll up through the hierarchy. | Owner-proposed |
| Q4-P9 | Cost centres and other financial dimensions are kept separate from the project hierarchy. | Owner-proposed |
| Q4-O1 | Migration of existing Milestone/Task/Subtask records — **no direct mapping assumed**; existing structure must be inspected before any migration proposal. | Open |

**L-1 Limitations of a two-level subproject limit (for review before finalising Q4-P3)** — Analyst note:
- Developments with deeper splits (e.g., Programme → Project → Phase-of-development → Tower → Podium/Tower zone) would need the extra level modelled as a Phase, or as separate projects under the programme.
- A fixed depth keeps roll-up, report columns and security rules predictable; raising it later affects reports and access rules but not the posting key (posting stays at work package).
- Vendor fit differs: CMiC master/sub-jobs; Business Central has no subproject object; Acumatica parent-child only since 2026 R2 (research.md §5). Integration to an ERP may need the subproject flattened into a dimension.

### Q5 — Cost code standard
- **Original (#5):** "Cost code standard (in-house, CSI MasterFormat, client-specific BoQ structure)?"
- **Type:** A. **Depends on:** Q4. **Answered:** 2026-11-03 (Batch 2).

| ID | Item | Status |
|---|---|---|
| Q5-P1 | Option (d): one central **group cost-code library**; client BoQ items maintained separately and mapped to it. | Owner-proposed |
| Q5-P2 | Library levels start as Division → Section → Element. | Owner-proposed |
| Q5-P3 | Controlled entity-specific extensions allowed without changing group codes. | Owner-proposed |
| Q5-P4 | A **group cost-control administrator** owns the library. | Owner-proposed (person/role not yet named → Q5-O1) |
| Q5-P5 | Library changes are versioned; historical reporting preserved. | Owner-proposed |
| Q5-P6 | Many-to-one BoQ mappings and controlled **split mappings** (one BoQ item → several cost codes); splits total 100 % and never duplicate budgets or costs. | Owner-proposed |
| Q5-P7 | Client billing quantities and internal cost-control quantities kept separate where necessary. | Owner-proposed |
| Q5-O1 | Name/role of the group cost-control administrator and the change-request process. | Open |

### Q5a — Dimension set and cost centres (derived)
- **Question:** Which dimensions are mandatory on every cost line, and who owns/maintains each master?
- **Type:** C. **Depends on:** Q1b, Q3, Q4. **Answered:** 2026-11-03.

| ID | Item | Status |
|---|---|---|
| Q5a-P1 | Every project cost allocation identifies: legal entity; project and posting work package (parent hierarchy derived); cost code; cost type; site/location where applicable; source document and source line; transaction currency and conversion details. | Owner-proposed |
| Q5a-P2 | Cost centres separate from the hierarchy; optional on project costs when Finance needs it; **required** for relevant overhead and non-project costs. | Owner-proposed |
| Q5a-P3 | A source document line may be split across several work packages/dimensions with controlled allocations (total 100 %, no duplication). | Owner-proposed |
| Q5a-P4 | Master ownership: **ERP** — legal entities, GL accounts, tax configuration, financial vendor records. **Payroll/HR** — employee records. **PMS** — project hierarchy, group cost-code library, crews, operational allocations. | Owner-proposed, subject to ERP integration research |
| Q5a-O1 | Equipment master ownership (ERP fixed assets vs PMS equipment register). | Open — confirm during integration research |
| Q5a-O2 | Which overhead/non-project cost categories require a cost centre. | Open — Finance |

### Q6 — When cost becomes actual
- **Original (#6):** "When cost becomes actual: on goods receipt / certification, or on vendor invoice? (Drives accrual design.)"
- **Type:** C. **Depends on:** Q3, Q1, Q13. **Answered:** 2026-11-03 (management view).

| ID | Cost type | Becomes management actual on | Status |
|---|---|---|---|
| Q6-P1 | Stock materials | Approved issue/consumption against a work package | Owner-proposed |
| Q6-P2 | Direct-to-work materials | Approved receipt, where accepted for immediate use and *not inventory under the agreed policy* | Owner-proposed; policy → Q6-F1 |
| Q6-P3 | Materials merely stored at site | Remain inventory until issued/consumed; delivery alone never creates project cost | Owner-proposed |
| Q6-P4 | Subcontracts | Approved certification of work performed, at the certified amount | Owner-proposed |
| Q6-P5 | Labour | Approved timesheets at standard rates, then payroll true-up (see Q7) | Owner-proposed |
| Q6-P6 | Internal equipment | Approved usage logs at approved rates | Owner-proposed |
| Q6-P7 | External equipment hire | Approved service-period confirmation under the contract | Owner-proposed |
| Q6-P8 | Overheads and other | Approved allocations, service accruals or eligible ERP postings per agreed policy | Owner-proposed; policy → Q6-F2 |
| Q6-P9 | Material receipt, inventory value, consumption and project cost tracked separately | — | Owner-proposed |
| Q6-F1 | Policy defining when received material is "direct-to-work" vs inventory, per entity/country | — | **Needs finance approval** |
| Q6-F2 | Overhead and other-cost recognition/allocation policy | — | **Needs finance approval** |

### Q6a — Accounting actuals vs management cost reporting (derived)
- **Type:** C. **Depends on:** Q3, Q6. **Answered:** 2026-11-03.

| ID | Item | Status |
|---|---|---|
| Q6a-P1 | Separate, clearly labelled accounting and management views. | Owner-proposed |
| Q6a-P2 | **Management incurred cost = Eligible posted project cost + Operational cost not already represented in those postings.** | Owner-proposed |
| Q6a-P3 | Eligible posted project cost excludes payments, advances, inventory balances, recoverable taxes and other non-expense items per agreed policy. | Owner-proposed; eligibility policy → Q6a-F1 |
| Q6a-P4 | Operational accruals exclude anything already represented by ERP receipt accruals, payroll postings or other ledger entries (not only vendor invoices). | Owner-proposed |
| Q6a-P5 | Matching and reconciliation at source-line/allocation level, including partial receipts, invoices and postings. | Owner-proposed |
| Q6a-P6 | When an accrual is replaced by a posting, the matched estimate is removed and the posted amount used; differences appear as explained variances. | Owner-proposed |
| Q6a-P7 | Report shows posted project cost, unposted operational accrual, total management incurred cost, reconciliation differences and exceptions. | Owner-proposed |
| Q6a-P8 | Monthly reconciliation by entity, project, work package and cost type; period snapshots preserved; later corrections handled transparently. | Owner-proposed; period-close rules → Batch 3 (Q15a) |
| Q6a-P9 | The PMS does not maintain the statutory GL. | Owner-proposed |
| Q6a-F1 | Eligibility policy (which GL postings count as project expense) per entity/country. | **Needs finance approval** |

### Q6b — Commitments, accruals, invoice matching, reversals, double counting (derived)
- **Type:** C. **Depends on:** Q3, Q6, Q10, Q1a. **Answered:** 2026-11-03.

| ID | Item | Status |
|---|---|---|
| Q6b-P1 | PMS originates and approves project POs, subcontracts and hire agreements. | Owner-proposed; **verify fit with selected ERP** (VD-19) |
| Q6b-P2 | One authoritative owner per document and its amendments. | Owner-proposed |
| Q6b-P3 | Tax treatment obtained from the configured ERP/tax service. | Owner-proposed; depends on RT-01 |
| Q6b-P4 | Receipts, certificates, invoices, returns and credits linked to source lines; one invoice may cover several receipts/allocations. | Owner-proposed |
| Q6b-P5 | Approved non-PO expenses and journals allowed through a controlled workflow. | Owner-proposed |
| Q6b-P6 | Unmatched ERP project postings stay visible in reconciliation/exceptions; valid posted costs are never silently excluded. | Owner-proposed |
| Q6b-P7 | A receipt relieves the undelivered purchase commitment even when material becomes inventory; remaining delivery commitment, inventory and incurred cost tracked separately with an explained bridge. | Owner-proposed |
| Q6b-P8 | Subcontract/service commitments relieved on approved certification or service acceptance. | Owner-proposed |
| Q6b-P9 | Price, quantity and approved scope changes shown separately. | Owner-proposed |
| Q6b-P10 | Matching: materials PO–receipt–invoice; subcontracts contract–certificate–invoice; tolerances and approvers defined in Batch 3. | Owner-proposed |
| Q6b-P11 | Reversals linked to originals; a return plus its credit note reverses economic cost once; commitment reopened only if replacement/remaining delivery expected; closing a PO closes only the unfulfilled balance; reasons, approvals and history preserved. | Owner-proposed |
| Q6b-P12 | Integration: stable document/line IDs, versioning, duplicate prevention, retry handling, reconciliation; resending never creates another posting. | Owner-proposed |
| Q6b-P13 | FX: retain transaction currency, functional currency, rate, rate date, rate source; PO-rate baseline for commitment analysis; finance-approved valuation for operational accruals; ledger values for posted actuals; FX effects shown separately but totals reconcile to full functional-currency cost; never label "posted actual" net of an FX variance. | Owner-proposed |
| Q6b-F1 | Accrual valuation rate (PO rate, receipt-date rate or other) for foreign-currency operational accruals. | **Needs finance approval** |
| Q6b-F2 | Open-commitment revaluation policy and group currency translation. | **Needs finance approval** (pending) |

### Q7 — Labour cost source
- **Original (#7):** "Labour cost source: timesheet × standard rate, actual payroll cost, or both with true-up?"
- **Type:** C. **Depends on:** Q1, Q3. **Answered:** 2026-11-03.

| ID | Item | Status |
|---|---|---|
| Q7-P1 | Option (c): daily standard cost with monthly payroll true-up. | Owner-proposed |
| Q7-P2 | Daily cost = approved hours × effective-dated standard rates by entity and trade, employee-specific where required. | Owner-proposed |
| Q7-P3 | Included cost components defined; no overlapping overtime or burden calculations. | Owner-proposed; components → Q7-F1 |
| Q7-P4 | Actual payroll allocated using approved project hours where appropriate; identifiable allowances/costs directly assigned; shared burdens allocated by documented rules. | Owner-proposed; rules → Q7-F2 |
| Q7-P5 | Leave, idle time and unallocated hours kept visible; payroll not automatically spread over productive project hours only. | Owner-proposed |
| Q7-P6 | Matched standard estimate replaced by actual payroll allocation; difference shown as labour variance; full payroll never added on top of the same standard cost. | Owner-proposed |
| Q7-P7 | Labour-contractor costs classified under the agreed subcontract or purchased-labour policy; never counted again as employee payroll. | Owner-proposed; policy → Q7-F3 |
| Q7-P8 | Individual pay details restricted; authorised project-cost reporting allowed. | Owner-proposed |
| Q7-F1 | Included payroll cost components per country (India, UAE). | **Needs finance & HR approval**; depends on RT-01 |
| Q7-F2 | Burden allocation rules and treatment of idle/leave cost. | **Needs finance approval** |
| Q7-F3 | Labour-contractor classification policy (subcontract vs purchased labour). | **Needs finance approval** |

### Q8 — Contingency policy
- **Original (#8):** "Contingency policy: who owns it, approval thresholds, reporting to client?"
- **Type:** C. **Depends on:** Q9, Q14. **Answered:** 2026-11-03 (Batch 3).

| ID | Item | Status |
|---|---|---|
| Q8-P1 | Option (c): controlled contingency at project and, where needed, subproject level. | Owner-proposed |
| Q8-P2 | Contractor contingency kept separate from client allowances and provisional sums. | Owner-proposed |
| Q8-P3 | A reserve is held at one level only; parent and subproject totals never duplicate it. | Owner-proposed |
| Q8-P4 | No direct cost postings or purchase commitments against a reserve. | Owner-proposed |
| Q8-P5 | Release only by approved transfer to a posting work package and cost code; each release has reason, risk reference where available, and approval. | Owner-proposed |
| Q8-P6 | Report original reserve, additions, releases, returns, remaining reserve and forecast need. | Owner-proposed |
| Q8-P7 | Contractor contingency internal by default; client-owned allowances disclosed per contract and authorised reporting rules. | Owner-proposed |
| Q8-P8 | No assumed contingency percentage; percentages/amounts to be provided. | Owner-proposed; values **Open** (Q8-O1) |
| Q8-P9 | Reserve never counted both inside work-package budgets and as additional available budget. | Owner-proposed |
| Q8-O1 | Contingency percentages/amounts and who may approve releases at which value. | Open — TO BE PROVIDED |
| Q8-O2 | Source of "forecast need" (risk register, PM judgement, quantified risk analysis). | Open |

### Q8a — Budget revisions and transfers (derived)
- **Type:** C. **Depends on:** Q9, Q14. **Answered:** 2026-11-03 (Batch 3).

| ID | Item | Status |
|---|---|---|
| Q8a-P1 | Option (c): locked original budget, controlled revisions, separate forecasts. | Owner-proposed |
| Q8a-P2 | Original approved baseline preserved permanently; corrections only through approved adjustment documents, never overwrite. | Owner-proposed |
| Q8a-P3 | Revised budget, version history, reasons, supporting documents and approvals maintained. | Owner-proposed |
| Q8a-P4 | Transfers net to zero within the same project and legal entity. | Owner-proposed |
| Q8a-P5 | Transfers across subprojects require the appropriate higher-level approval. | Owner-proposed; approver → Q14 |
| Q8a-P6 | No transfers between legal entities or unrelated projects — use separately approved budget changes instead. | Owner-proposed |
| Q8a-P7 | Cost and revenue budgets kept separate. | Owner-proposed |
| Q8a-P8 | Forecast revisions never change approved budgets. | Owner-proposed |
| Q8a-P9 | Show original budget, approved changes, net transfers, revised budget, incurred cost, open commitments, ETC, EAC and variance. | Owner-proposed |
| Q8a-Q1 | Clarification requested: does ETC include remaining commitments? | **Answered by analyst definition (proposed):** ETC *excludes* remaining commitments and project-held inventory; see requirements.md §3.4. Awaiting owner confirmation. |

### Q9 — Change order process
- **Original (#9):** "Change order process: classes, approval levels, link to client variations and subcontract variations."
- **Type:** C. **Depends on:** Q4, Q5, Q14. **Answered:** 2026-11-03 (Batch 3).

| ID | Item | Status |
|---|---|---|
| Q9-P1 | Option (c): potential changes followed by controlled approval, with classes: baseline correction, internal budget change, client variation, budget transfer, subcontract variation, contingency release. | Owner-proposed |
| Q9-P2 | Separate statuses for internal cost authorisation, client commercial approval, subcontract approval and budget posting. | Owner-proposed |
| Q9-P3 | Client approval never automatically authorises internal budget increases or subcontract amendments. | Owner-proposed |
| Q9-P4 | Pending changes appear as exposure, not approved budget. | Owner-proposed |
| Q9-P5 | Probable costs included in the forecast under a documented policy, even when client recovery is uncertain. | Owner-proposed; policy → Q9-F1 |
| Q9-P6 | No double counting where pending-change costs are already in ETC. | Owner-proposed |
| Q9-P7 | Track proposed, submitted, assessed, approved, rejected and withdrawn amounts and dates. | Owner-proposed |
| Q9-P8 | One client change links to multiple cost allocations and subcontract variations. | Owner-proposed |
| Q9-P9 | Differences between client price, internal cost and subcontract value explained (need not be equal). | Owner-proposed |
| Q9-P10 | Approved at-risk work through a separate exception workflow. | Owner-proposed; cost placement → Q9-O1 |
| Q9-P11 | Rejected and superseded records preserved. | Owner-proposed |
| Q9-P12 | Forecast margin and unrecovered exposure shown; pending revenue never treated as approved revenue. | Owner-proposed |
| Q9-F1 | Definition of "probable" for including pending-change cost in forecast (and any link to onerous-contract assessment). | **Needs finance approval** |
| Q9-O1 | Where at-risk cost is budgeted/posted before approval. | **Answered in direction 2026-11-03** → Q9-P13…P16 |
| Q9-P13 | Authorised at-risk costs are recorded immediately against the correct work package. | Owner-proposed |
| Q9-P14 | They are identified as at-risk exposure. | Owner-proposed |
| Q9-P15 | They are never hidden outside project cost while awaiting client approval. | Owner-proposed |
| Q9-P16 | Budget authority, expected recovery and revenue treatment are kept separate. | Owner-proposed |

### Q10 — Subcontract practice
- **Original (#10):** "Subcontract practice: measurement method, retention %, caps, release triggers, advance payments, back-charges, compliance documents required."
- **Type:** A/C. **Depends on:** Q1, Q13. **Answered:** 2026-11-03 (Batch 4).

| ID | Item | Status |
|---|---|---|
| Q10-P1 | Four measurement methods per subcontract: BoQ re-measurement; % completion against schedule of values; milestones; approved labour/daywork records. | Owner-proposed |
| Q10-P2 | Certificates show current-period and cumulative amounts, with controls against duplicate certification. | Owner-proposed |
| Q10-P3 | Separate amounts: gross work certified; approved variations; retention held/released; advances paid/recovered; back-charges; taxes and withholding; net payable. | Owner-proposed |
| Q10-P4 | Management cost recognised from approved work certification, subject to accrual and ledger reconciliation rules. | Owner-proposed (consistent with Q6-P4) |
| Q10-P5 | Retention and advance recovery never reduce the gross cost of work performed; advances tracked separately from project expense. | Owner-proposed |
| Q10-P6 | Back-charges do not become cost credits merely because proposed; record basis, evidence, approval, disputed/accepted status, accounting treatment and source-cost links; prevent duplicate recovery or duplicate cost reduction. | Owner-proposed; accounting treatment → Q10-F1 |
| Q10-P7 | Effective-dated retention %, caps and release conditions; advance guarantees with expiry alerts and recovery schedules; defects-liability tracking; contractual payment terms; compliance documents with expiry dates and controlled payment holds; purchased-labour classification without duplicate payroll cost. | Owner-proposed |
| Q10-P8 | Certificate dates defined explicitly (work period, submission, certification, accounting/tax dates, payment due, payment). | Owner-proposed; defined in requirements.md §4.3 |
| Q10-O1 | Retention %, caps, advance terms, release triggers and mandatory documents per contract and country. | Open — TO BE PROVIDED |
| Q10-F1 | Accounting treatment of accepted back-charges (cost reduction on the source cost line vs other income). | **Needs finance approval** |
| Q10-R1 | Country tax and compliance on certificates, retention, advances and withholding. | **Needs research / professional validation** (RT-01 §A.7, §B.8) |

### Q11 — Equipment
- **Original (#11):** "Equipment: owned vs rented share, charging basis (hour/day/month), operator costing."
- **Type:** A. **Answered:** 2026-11-03 (Batch 4).

| ID | Item | Status |
|---|---|---|
| Q11-P1 | Equipment register; owned, leased and externally hired equipment. | Owner-proposed |
| Q11-P2 | Hourly, daily, weekly and monthly rates; effective-dated rate cards by entity and equipment class. | Owner-proposed |
| Q11-P3 | Time categories: working, idle, breakdown, maintenance, transfer. | Owner-proposed |
| Q11-P4 | Minimum hire, standby, mobilisation, demobilisation and partial-period rules; bare hire and hire with operator. | Owner-proposed |
| Q11-P5 | Usage, availability, utilisation and cost-recovery reporting. | Owner-proposed |
| Q11-P6 | Operator and fuel treatment explicit in each rate (included or separate); underlying fuel and payroll still recorded; when included, allocated to the equipment cost pool and never charged again to the receiving work package. | Owner-proposed |
| Q11-P7 | Internal equipment charges kept separate from underlying accounting expenses; under/over-recovery reconciled; internal charges eliminated in consolidated reporting. | Owner-proposed |
| Q11-P8 | Market-rate internal charges are not assumed to represent accounting actual cost. | Owner-proposed |
| Q11-P9 | PMS owns operational equipment data and allocations; ERP owns fixed assets, depreciation and financial lease accounting. | Owner-proposed, subject to integration verification (closes Q5a-O1 in direction) |
| Q11-P10 | Cross-entity equipment use requires a controlled intercompany process. | Owner-proposed; process → Q1b-O2 |
| Q11-O1 | Rates, idle-time policies and charging terms. | Open — TO BE PROVIDED |
| Q11-F1 | Whether project management cost for owned equipment is the internal charge or the pool actual cost (and treatment of under/over-recovery in project reports). | **Needs finance approval** |

### Q12 — Material practice
- **Original (#12):** "Material practice: central store vs site stores, wastage norms, return handling."
- **Type:** A. **Answered:** 2026-11-03 (Batch 4).

| ID | Item | Status |
|---|---|---|
| Q12-P1 | Central stores, site stores and controlled transfers. | Owner-proposed |
| Q12-P2 | Material master, units of measure and approved conversions. | Owner-proposed |
| Q12-P3 | Receipts with inspection, accepted/rejected quantities and quarantine. | Owner-proposed |
| Q12-P4 | Issues to work packages; returns from work packages to stock; vendor returns; store/site transfers and goods in transit; adjustments, wastage, scrap and disposal; quantity and value reconciliation. | Owner-proposed |
| Q12-P5 | Physical location alone never determines inventory vs project expense. | Owner-proposed (refines Q6-P2/P3) |
| Q12-P6 | Same-entity transfers preserve inventory value under the approved valuation policy and never create project cost by movement alone. | Owner-proposed |
| Q12-P7 | Cross-entity transfers use approved intercompany documents, pricing, tax and accounting; book value is not assumed. | Owner-proposed |
| Q12-P8 | Cross-border and domestic transfer documentation researched separately. | **Needs research** (RT-04, in progress) |
| Q12-P9 | Theoretical consumption, actual consumption, allowed wastage and excess wastage tracked separately; norms configurable by material and activity with effective dates. | Owner-proposed |
| Q12-P10 | No wastage charge added on top of consumption that already includes the quantity. | Owner-proposed |
| Q12-P11 | Reasons and approval for excess wastage, loss and adjustments. | Owner-proposed; classification → Q12-F2 |
| Q12-P12 | Monthly counts as baseline, risk-based cycle counts, controlled inventory cut-offs; count variances and approvals recorded. | Owner-proposed |
| Q12-P13 | Usable leftovers, scrap and recoveries tracked separately. | Owner-proposed |
| Q12-O1 | Wastage norms per material and activity. | Open — TO BE PROVIDED |
| Q12-F1 | Inventory valuation method. | **Needs finance approval** (pending) |
| Q12-F2 | Accounting classification of excess wastage, losses and abnormal losses, and their treatment in revenue-progress calculations. | **Needs finance approval** (pending) |

### Q13 — Client billing and revenue recognition
- **Original (#13):** "Client billing: milestone, BoQ/valuation-based, cost-plus? Retention terms? Revenue recognition standard (Ind AS 115 / IFRS 15 / ASC 606)?"
- **Type:** C. **Depends on:** Q1, Q3. **Answered:** 2026-11-03 (Batch 4).

| ID | Item | Status |
|---|---|---|
| Q13-P1 | Billing methods per contract: BoQ re-measurement/interim valuation; milestones; cost-plus; lump sum with schedule of values; mixed methods where clearly allocated. | Owner-proposed |
| Q13-P2 | Separate measures: work performed; amount applied for; amount assessed/certified by client; statutory invoices and credit notes; recognised revenue; retention; advances and recoveries; receipts and outstanding balances. | Owner-proposed |
| Q13-P3 | Current-period and cumulative billing quantities and values; client BoQ quantities separate from internal cost quantities. | Owner-proposed |
| Q13-P4 | Approved and pending variations tracked separately; neither a client certificate nor an invoice is automatically recognised revenue. | Owner-proposed |
| Q13-P5 | PMS prepares and approves commercial billing data; the designated ERP or compliant invoicing service issues statutory invoices and returns invoice IDs, status and references. | Owner-proposed |
| Q13-P6 | Finance approves revenue policy per entity and contract; method never selected solely from billing method or country. | Owner-proposed |
| Q13-P7 | PMS provides validated inputs: eligible costs incurred; ETC and EAC; measured progress; approved contract value and separately identified uncertain changes; uninstalled materials and other progress adjustments; forecast margin and loss indicators. | Owner-proposed |
| Q13-P8 | ERP responsible for accounting revenue postings. | Owner-proposed |
| Q13-P9 | Contract assets, receivables, contract liabilities and commercial over/under billing defined separately; never interchangeable. | Owner-proposed |
| Q13-C1 | **Correction:** the analyst statement that uninstalled materials are always excluded from cost-to-cost calculations is withdrawn. Treatment depends on conditions in the applicable standard and is a Finance decision. | Correction recorded; RT-03 complete (statutory-research Part E) |
| Q13-R1 | Verify the applicable Indian reporting framework per entity (Ind AS / AS / non-company frameworks); do not assume one standard. | **Needs research** (RT-03, in progress) and Finance confirmation |
| Q13-O1 | Retention, advance terms and billing cycles per contract. | Open — TO BE PROVIDED |
| Q13-F1 | Revenue policies per entity and contract type (method, treatment of uninstalled materials, variable consideration/pending variations). | **Needs finance approval** |

### Q14 — Approval matrix
- **Original (#14):** "Approval matrix by document type and amount."
- **Type:** A. **Answered:** 2026-11-03 (Batch 3) — structure only; values TO BE PROVIDED.

| ID | Item | Status |
|---|---|---|
| Q14-P1 | Option (c): role-based approval tiers configurable by entity and project. | Owner-proposed |
| Q14-P2 | Roles: Requester/site engineer; Storekeeper/material controller; Project engineer/discipline lead; Quantity surveyor/cost controller; Procurement manager; Project manager; Project director; Finance controller; CFO/authorised executive; HR/payroll administrator; System administrator (no automatic financial approval rights). | Owner-proposed |
| Q14-P3 | Thresholds stored in each entity's functional currency. | Owner-proposed |
| Q14-P4 | No self-approval. | Owner-proposed |
| Q14-P5 | Incompatible duties defined per document and risk (no blanket rule that blocks legitimate site workflows); staffing exceptions documented with compensating review and authorisation. | Owner-proposed |
| Q14-P6 | Prevent purchase splitting to bypass thresholds. | Owner-proposed; detection rule → Q14-O4 |
| Q14-P7 | Material changes to value, scope, vendor, dimensions or terms require re-approval. | Owner-proposed |
| Q14-P8 | Explicit handling of over-budget commitments. | Owner-proposed |
| Q14-P9 | Percentage and absolute matching limits applied together under a documented rule. | Owner-proposed |
| Q14-P10 | Until limits are approved, discrepancies are routed for review — never auto-accepted. | Owner-proposed |
| Q14-P11 | Time-limited, logged delegation that cannot bypass segregation of duties. | Owner-proposed |
| Q14-P12 | Individual payroll information restricted to authorised HR/finance roles; aggregated cost for project teams (resolves X-11). | Owner-proposed |
| Q14-P13 | Complete approval and override histories. | Owner-proposed |
| Q14-O1 | Approval amounts per role, entity and document type. | Open — TO BE PROVIDED |
| Q14-O2 | Tolerance percentages and absolute limits; required approval combinations. | Open — TO BE PROVIDED |
| Q14-O3 | Conversion rule for foreign-currency approval values (e.g., document-date rate). | Open — **Needs finance approval** |
| Q14-O4 | Whether amendments are assessed against cumulative document value, change amount, or both; purchase-splitting detection window and criteria. | Open |
| Q14-O5 | Incompatible-duty matrix by document type. | Open |

### Q15 — Reporting cadence
- **Original (#15):** "Reporting cadence (monthly cost report cut-off, forecast review cycle) and required report formats."
- **Type:** C. **Answered:** 2026-11-03 (Batch 5).

| ID | Item | Status |
|---|---|---|
| Q15-P1 | Monthly cost reporting and immutable snapshots aligned with each entity's finance calendar. | Owner-proposed (consistent with Q15a) |
| Q15-P2 | Monthly work-package ETC review, signed off by PM and QS/cost controller. | Owner-proposed |
| Q15-P3 | Weekly flash: commitments, incurred cost, pending changes, forecast risks, exceptions. | Owner-proposed |
| Q15-P4 | Operational dashboards show last successful source-data update and any integration delays. | Owner-proposed |
| Q15-P5 | Required reports: original/revised budget vs incurred; remaining commitments, inventory held, ETC, EAC, forecast variance; materials consumption and wastage; labour hours, productivity, cost variances; equipment utilisation and recovery; subcontract certification, retention, advances, outstanding liabilities; change register and contingency movements; client billing, collections, recognised revenue and forecast margin as distinct measures; PMS-to-ERP reconciliation and unresolved exceptions; project, subproject, entity, country and group summaries. | Owner-proposed |
| Q15-P6 | Dashboard, Excel and PDF formats; exports preserve filters, currency basis, reporting period, snapshot version and access restrictions. | Owner-proposed |
| Q15-P7 | Client extracts exclude internal contingency, individual pay, internal margins and other confidential information unless explicitly authorised. | Owner-proposed |
| Q15-O1 | Deadlines, calendars and snapshot approvers. | Open — TO BE PROVIDED (same as Q15a-O1) |

### Q15a — Period closing and snapshots (derived from Q15; pulled forward because Q6a-P8 needs it)
- **Question:** How is a cost period closed, what is frozen in the snapshot, and how are late or back-dated transactions and corrections handled?
- **Type:** C. **Depends on:** Q6a, Q6b, ERP close calendar. **Answered:** 2026-11-03 (Batch 3).

| ID | Item | Status |
|---|---|---|
| Q15a-P1 | Monthly immutable snapshots; controlled reopening only as an exception. | Owner-proposed |
| Q15a-P2 | PMS close calendar aligned with each entity's finance calendar. | Owner-proposed |
| Q15a-P3 | Document date, service/receipt date, accounting posting period and PMS reporting period preserved separately. | Owner-proposed |
| Q15a-P4 | Snapshot freezes budgets, commitments, inventory, incurred cost, accruals, forecasts, FX assumptions and reconciliation status. | Owner-proposed |
| Q15a-P5 | Snapshot records version, preparation time, approvers and source-data cut-off. | Owner-proposed |
| Q15a-P6 | Late items affect the next open management period with a clear prior-period reference. | Owner-proposed |
| Q15a-P7 | PMS never changes an ERP posting date or accounting period to fit a snapshot. | Owner-proposed |
| Q15a-P8 | ERP postings into a previously closed accounting period are identified and reconciled explicitly. | Owner-proposed |
| Q15a-P9 | Reopening requires Finance approval and creates a new snapshot version; the original is preserved. | Owner-proposed |
| Q15a-P10 | Adjustments distinguished from replacement of an existing accrual, so a late invoice never duplicates cost. | Owner-proposed |
| Q15a-P11 | Entity-level close plus consolidated reporting cut-off that identifies entities still open. | Owner-proposed |
| Q15a-O1 | Close calendar dates per entity and snapshot approvers. | Open — TO BE PROVIDED |

### Q16 — Deployment and data residency
- **Original (#16):** "Deployment and data-residency constraints (cloud vs on-prem)."
- **Type:** A + B. **Answered:** 2026-11-03 (Batch 5).

| ID | Item | Status |
|---|---|---|
| Q16-P1 | Preference: managed cloud, subject to country, client-contract, security and integration requirements. | Owner-proposed |
| Q16-P2 | Research regional hosting and data separation; per-entity region selection is **not** assumed to be supported by every vendor or achievable by configuration. | Owner-proposed → research RT-05b |
| Q16-P3 | Evaluate: tenant and legal-entity isolation; hosting, backup, DR and support-access locations; cross-border transfers and group reporting; encryption, RBAC, SSO/MFA, audit logs, retention; RPO/RTO; export, portability and vendor exit; mobile use and unreliable site connectivity. | Owner-proposed |
| Q16-P4 | Minimise personal data; detailed payroll/HR records stay in authoritative systems; PMS receives only authorised costing data; no medical records in PMS scope. | Owner-proposed |
| Q16-P5 | Reverify legal claims against current authoritative sources with exact effective dates, applicability and transition periods; distinguish statutory, contractual and recommended-practice items; unverified claims pending professional validation. | Owner-proposed → research RT-05a |
| Q16-O1 | Client/government hosting clauses. | Open — TO BE PROVIDED |
| Q16-O2 | Recovery targets (RPO/RTO). | Open — TO BE PROVIDED |
| Q16-O3 | UAE entity revenue bands (e-invoicing phase). | Open — TO BE PROVIDED |
| Q16-O4 | Any on-premises requirement. | Open — TO BE PROVIDED |

### Q17 — Budget and implementation tolerance
- **Original (#17):** "Budget for licences and implementation; tolerance for partner-led implementation."
- **Type:** A. **Answered:** 2026-11-03 (Batch 5) — direction only.

| ID | Item | Status |
|---|---|---|
| Q17-P1 | Preferred direction: extend or develop our construction PMS and integrate with an accounting ERP, subject to the existing-system inspection and feasibility assessment. **Planning preference only — not implementation approval, not an ERP selection.** | Owner-proposed |
| Q17-P2 | Prepare alternative phased scope options with dependencies; no invented budgets, licence prices, delivery commitments or team capacity. | Owner-proposed → requirements.md §12 |
| Q17-P3 | Cost categories kept separate: software subscriptions; configuration and custom development; integration; data migration; hosting and security; training and change management; ongoing support; country-specific compliance services. Sourced public pricing where available, otherwise "Quote required"; any estimate states assumptions and confidence. | Owner-proposed |
| Q17-O1 | Budget, delivery deadline, internal team size, implementation-partner tolerance. | Open — TO BE PROVIDED |
| Q17-D1 | Feasibility of extending the existing PMS depends on RT-02 inputs (schema, stored procedures, CommonClass source). | Dependency (RT-02 partially blocked) |

---

## Contradictions and dependencies identified after Batch 1

| ID | Issue | Items | Resolution needed |
|---|---|---|---|
| X-1 | "Each project belongs to one legal entity" vs "JVs are not assumed to be separate legal companies". An unincorporated JV/consortium project has no single owning entity unless one is designated. | Q1b-P2, Q1b-P4 | **Resolved in direction 2026-11-03.** Q1b-C2 introduces the accounting-entity rule and Q1b-P6…P9 set the JV models. Actual JV structures are still TO BE PROVIDED, and Finance/legal must approve each one. |
| X-2 | PMS manages commitments (Q3-P1) but the ERP owns payables and tax (Q3-P2). POs and subcontracts carry tax and become AP invoices — the system that *issues* POs/subcontracts and calculates tax is undefined. | Q3-P1, Q3-P2, Q1-P1 | Ownership matrix: which system creates and numbers POs/subcontracts, who calculates tax, and when the record is sent to the ERP. Addressed in Q6b. |
| X-3 | Foreign-currency purchasing (Q1a-P3) plus budget-vs-actual: a commitment at PO-date rate and an actual at invoice-date rate differ, which can look like an overrun or a double count. | Q1a-P3, Q1a-P4, Q4-P8 | FX rule for commitments, actuals and budget (asked under Q6b). |
| X-4 | Group programme spans legal entities with different functional currencies; programme roll-up needs translation rules that Finance has not yet approved. | Q1a-P5, Q4-P2, Q1-F1 | Finance approval of translation rules before programme-level financial reports can be specified. |
| X-5 | Task-level records roll up "without posting twice" (Q4-P7): if tasks carry hours, materials or progress, each record must reference exactly one posting work package; tasks themselves must be non-posting. | Q4-P5, Q4-P7 | Rule proposed in Batch 2 (Q5a/Q6b). |
| X-6 | Capacity and performance criteria cannot be written (Q2-O6), and implementation budget (Q17) depends on size. | Q2, Q17 | Volumes required from the business. |
| X-7 | Compliance-related requirements (tax on POs/claims, e-invoicing, payroll inputs, retention/withholding) cannot be finalised until RT-01 research is done. | Q1-R1, Q6b, Q7, Q10, Q13 | Run RT-01 before requirements.md compliance sections. |
| X-8 | Direct-to-work vs inventory classification (Q6-P2) must agree with how the ERP books the receipt (inventory account vs expense). If the PMS says "project cost" while the ERP books inventory, the reconciliation will always show a difference. | Q6-P2, Q6-F1, Q6a-P3 | Finance policy Q6-F1 must be the single rule used by both systems; verify ERP supports it (VD-18). |
| X-9 | Q6a-P4 requires the PMS to know which ERP receipt accruals/payroll postings already represent an operational cost. This only works if the ERP exposes those postings at line level with the PMS source-line ID. | Q6a-P4, Q6a-P5, Q6b-P12 | Integration requirement; verify per ERP (VD-18). Until verified, treat as a vendor dependency. |
| X-10 | PMS originates POs (Q6b-P1) but the ERP owns vendor records and tax configuration (Q5a-P4, Q6b-P3). The PMS needs validated vendor data and tax determination *at PO approval time*, before the ERP has the document. | Q6b-P1, Q6b-P3, Q5a-P4 | Integration pattern (real-time lookup vs later tax recalculation) to be decided after ERP research (VD-19). |
| X-11 | Employee-specific standard rates (Q7-P2) can reveal individual pay, conflicting with restricted pay access (Q7-P8). | Q7-P2, Q7-P8 | Role rule: rate tables and per-employee cost visible only to authorised roles; project reports show aggregated cost by trade/crew. To confirm in Batch 3 (Q14 roles). |
| X-12 | Subcontract certification as management actual (Q6-P4) may differ from the statutory tax point and withholding (e.g., TDS in India, VAT in UAE) applied on the invoice. | Q6-P4, Q6b-P3, RT-01 | Keep management actual at certified amount excluding recoverable tax; statutory treatment from ERP/tax service after RT-01. |
| X-13 | Period snapshots with later transparent corrections (Q6a-P8) need a period-close rule — otherwise a back-dated ERP posting silently changes a closed month. | Q6a-P8, Q15a | **Resolved in direction** by Q15a-P1…P11; calendar values open (Q15a-O1). |
| X-14 | Including "probable" pending-change cost in the forecast (Q9-P5) needs a definition of probable and a rule linking it to ETC (Q9-P6); otherwise the same cost can sit in ETC and in pending exposure. | Q9-P5, Q9-P6, Q9-F1 | Requirements.md §3.4 proposes a per-change "already in ETC" amount; definition of probable needs Finance. |
| X-15 | At-risk work (Q9-P10) incurs cost before budget approval, but cost may only post to a budgeted posting work package and over-budget commitments need explicit handling (Q14-P8). | Q9-P10, Q14-P8, HR-04 | **Resolved in direction 2026-11-03** by Q9-P13…P16. Remaining detail is in X-28. |
| X-16 | Thresholds are in functional currency (Q14-P3) but documents can be in foreign currency (Q1a-P3); the approval value of a foreign-currency document depends on an unapproved rate rule. | Q14-P3, Q14-O3, Q1a-P3 | Finance to approve Q14-O3. |
| X-18 | UAE e-invoicing Phase 1 goes live 1 Jan 2027 for entities with revenue ≥ AED 50m (RT-01 B.2). If the PMS is to produce progress-billing data, the ERP/ASP flow may need to be live before PMS go-live. | Q3-P2, Q13, Q16, ST-09 | Revenue band per UAE entity and go-live sequencing — business/finance input in Batch 5. |
| X-19 | Record retention differs: India books at least 8 years (A.5); UAE up to 15 years for real-estate records (B.1). A single PMS retention rule is not possible without professional validation. | ST-14, AU-01 | Per-country configurable retention; unified policy needs professional validation. |
| X-20 | India's Companies Act audit-trail and India-hosted backup rules may apply if PMS records form part of the books of account (A.5). This conflicts with treating the PMS purely as a management system (Q6a-P9) if PMS documents feed the ERP. | Q6a-P9, Q16, ST-14 | Professional validation of whether the PMS is in scope; affects Q16 hosting. |
| X-21 | Equipment hire with operator and manpower supply may fall under different India TDS categories (A.3). The PMS commitment types (hire agreement vs subcontract vs purchased labour; Q6b-P1, Q7-P7) must align with the tax classification. | Q6b-P1, Q7-F3, ST-08 | Batch 4 (Q10, Q11) plus tax sign-off. |
| X-22 | The tax point for progress certificates and retention can differ from the management actual date in both countries (A.1, B.1). This confirms X-12. Certificates must carry several separate dates. | X-12, ST-03, ST-04 | Adopt ST-03/ST-04 when approved. |
| X-27 | The "accounting entity" concept (Q1b-C2) may not exist in every ERP. A separately accounted non-company JV may need its own ERP company or ledger. | Q1b-C2, Q1b-P6, Q3-O1 | Vendor check VD-31; Finance and legal to approve per JV. |
| X-28 | At-risk cost is recorded on the work package immediately (Q9-P13), but commitments over available budget need explicit approval (AP-07). At-risk work therefore needs budget authority that is separate from client recovery. | Q9-P13…P16, AP-07 | requirements.md AR-01…AR-05 (proposed): at-risk authorisation is a distinct approval, and the over-budget amount is flagged as at-risk exposure. |
| X-29 | Personal-data minimisation (Q16-P4) vs employee-level timesheets and employee-specific rates (Q7-P2). The PMS must hold some personal data. | Q16-P4, Q7-P2, Q7-P8 | Minimum dataset: employee ID, trade, entity, hours and approved cost rate. No salary components beyond the authorised cost rate. Restricted roles. |
| X-30 | The preferred direction to extend the existing PMS (Q17-P1) depends on a feasibility assessment. That assessment cannot be finished while RT-02 is partially blocked. | Q17-P1, Q17-D1, RT-02 | RT-02 inputs are required before feasibility can be concluded. |
| X-23 | Q6-P6 counts internal equipment at "usage log × approved rate" as management actual, while Q11-P8 says internal charges are not accounting actual cost. Project cost, the equipment pool's actual cost and the ERP ledger will differ by the under/over-recovery. | Q6-P6, Q11-P7, Q11-P8, Q6a | Finance decision Q11-F1; reconciliation rule in requirements.md §4.4 (Example 14). |
| X-24 | An accepted back-charge recorded as a cost credit on the source cost line (Q10-P6) must reconcile to the ERP. This works only if the ERP posts the recovery with the same project, work package and cost-code dimensions and a link back to the back-charge. | Q10-P6, Q10-F1, REC-C | Finance Q10-F1 plus integration verification (VD-25). |
| X-25 | The PMS approves commercial billing data (Q13-P5) and the ERP or e-invoicing service issues statutory invoices. Statutory issue deadlines apply: UAE e-invoices within 14 business days (Unverified) and India IRP reporting within 30 days for larger taxpayers. Client certification cycles must allow for these. | Q13-P5, ST-09, X-18 | Billing-cycle values (Q13-O1) and integration timing. |
| X-26 | Cross-entity equipment use (Q11-P10) and cross-entity material transfers (Q12-P7) both depend on the intercompany process, which is still open (Q1b-O2). | Q11-P10, Q12-P7, Q1b-O2 | Asked again in the consolidated list. |
| X-17 | Contingency held at subproject level (Q8-P1) and transfers across subprojects needing higher approval (Q8a-P5): a contingency release from a project-level reserve to a subproject work package is both a release and a cross-subproject transfer. | Q8-P5, Q8a-P5 | Proposed: treat as a contingency release with the stricter of the two approval rules; confirm in Q14 values. |

## Open research tasks

| ID | Task | Status |
|---|---|---|
| RT-01 | Research current Indian and UAE requirements relevant to construction PMS: indirect tax (GST / VAT) on purchases, subcontracts and client billing; e-invoicing; withholding/TDS; payroll and labour-law cost elements (e.g., statutory contributions, end-of-service benefits, WPS); retention and advance practices; relevant accounting requirements; data-residency/data-protection. Use official government sources; record effective dates; separate legal requirement / configurable policy / professional validation; never applied to other countries; never becomes finance policy without approval. | **Complete 2026-11-03** — findings in `statutory-research-india-uae.md` (research only; candidate requirements ST-01…ST-17 not approved; many items Unverified pending official-text check) |
| RT-03 | Research accounting treatment of uninstalled materials under the applicable standards (conditions, effect on progress and revenue) and the Indian reporting framework applicable to each type of entity. Research only; Finance decides. | **Complete 2026-11-03.** Findings in `statutory-research-india-uae.md` Part E. Finance open items E.5. Candidate requirements ST-19…ST-21 not approved. |
| RT-04 | Research domestic and cross-border documentation for transfers of materials and equipment (same entity, other GSTIN/emirate, other legal entity, India↔UAE). Research only. | **Complete 2026-11-03.** Findings are in `statutory-research-india-uae.md` Part D. Most items are Unverified (secondary sources) and need professional validation (Part D.4). Candidate ST-18 is not approved. |
| RT-05a | Reverify data-protection, records and hosting-related legal claims (India, UAE) with exact effective dates; separate statutory / contractual / recommended practice. | **Complete 2026-11-03.** Findings in statutory-research Part F. Several dates are calculated or unverified (e.g., DPDP 13 vs 14 Nov 2025). Open items in F.4. |
| RT-05b | Research documented hosting regions, isolation, backup/DR, support access, security and exit provisions for the eight researched ERPs and main public clouds. No vendor recommendation. | **Complete 2026-11-03.** Findings in `hosting-research.md`. Only Business Central and Zoho publicly document India and UAE regions. Odoo Online stores backups for its Middle East & S. Asia region offshore. Most vendors need written confirmation (VD-30). |
| RT-06 | Research intercompany valuation dimensions (commercial, accounting, VAT/GST, TP, CT relief, customs) for UAE and India. | **Complete 2026-11-03.** Findings are in statutory-research Part G. Correction Q1b-C3 is confirmed. Professional validation items are in G.3. Candidate ST-22 is not approved. |
| RT-02 | Read-only inspection of PMS source and documentation in the authorised workspace: hierarchy, modules, data model, integrations, gaps. No code/data/config changes, no production systems or credentials. Mark blocked if inputs missing. | **Complete (partially blocked) 2026-11-03** — findings in `existing-pms-inspection.md`. Blocked inputs: DB schema/DDL, stored-procedure definitions, CommonClass source, confirmation of live page versions. |

## Decision log

| Date | ID | Decision | Status | Notes |
|---|---|---|---|---|
| 2026-11-03 | Q1-C1 | Support India, UAE and additional countries via configuration | **Confirmed** | Only confirmed requirement so far. |
| 2026-11-03 | Q1a-P1…P5, Q1b-P1…P4 | Currency and legal-entity direction | Owner-proposed | Finance approval required for standards, FX sources, consolidation. |
| 2026-11-03 | Q3-P1…P4 | PMS for operations/cost management; ERP as official books | Owner-proposed | No ERP selected; existing systems unconfirmed. |
| 2026-11-03 | Q2-P1 | Multi-company, multi-country, multi-project, multi-site support | Owner-proposed | Volumes unknown. |
| 2026-11-03 | Q4-P1…P9 | Hierarchy and posting level | Owner-proposed | Two-level subproject limit pending L-1 review. |
| 2026-11-03 | Q5-P1…P7, Q5a-P1…P4 | Group cost-code library with BoQ mapping; dimension set and master ownership | Owner-proposed | Ownership subject to ERP integration research. |
| 2026-11-03 | Q6-P1…P9 | Management actual recognition by cost type | Owner-proposed | Inventory and overhead policies need Finance. |
| 2026-11-03 | Q6a-P1…P9 | Posted vs operational accrual vs incurred cost | Owner-proposed | Eligibility policy needs Finance. |
| 2026-11-03 | Q6b-P1…P13 | Commitment relief, matching, reversals, integration, FX | Owner-proposed | Q1a-O1 superseded by Q6b-P13 + Q6b-F1/F2. |
| 2026-11-03 | Q7-P1…P8 | Daily standard labour cost with payroll true-up | Owner-proposed | Components per country need Finance & HR. |
| 2026-11-03 | Q8a-P1…P9 | Locked baseline, controlled revisions, zero-net transfers within project/entity | Owner-proposed | ETC definition clarified (Q8a-Q1). |
| 2026-11-03 | Q8-P1…P9 | Controlled contingency, separate client allowances | Owner-proposed | Amounts TO BE PROVIDED. |
| 2026-11-03 | Q9-P1…P12 | Change classes and four independent approval statuses | Owner-proposed | "Probable" policy needs Finance. |
| 2026-11-03 | Q14-P1…P13 | Role-based configurable approvals and SoD principles | Owner-proposed | All amounts/tolerances TO BE PROVIDED. |
| 2026-11-03 | Q15a-P1…P11 | Monthly immutable snapshots, controlled reopen | Owner-proposed | Calendars TO BE PROVIDED. |
| 2026-11-03 | Q10-P1…P8 | Subcontract measurement, cumulative certificates, separate retention/advance/back-charge amounts | Owner-proposed | Contract values TO BE PROVIDED; back-charge accounting Finance. |
| 2026-11-03 | Q11-P1…P10 | Equipment register, charging rules, cost pools, recovery reconciliation | Owner-proposed | Rates TO BE PROVIDED; Q11-F1 Finance. |
| 2026-11-03 | Q12-P1…P13 | Stores, transfers, wastage norms, counts | Owner-proposed | Norms TO BE PROVIDED; valuation and abnormal-loss Finance. |
| 2026-11-03 | Q13-P1…P9, Q13-C1 | Billing measures kept separate; ERP posts revenue; correction on uninstalled materials | Owner-proposed | Revenue policy Finance; RT-03 research. |
| 2026-11-03 | Q15-P1…P7 | Monthly snapshots, weekly flash, required report set, export rules, client extract exclusions | Owner-proposed | Deadlines TO BE PROVIDED. |
| 2026-11-03 | Q16-P1…P5 | Managed-cloud preference; hosting/isolation evaluation; personal-data minimisation; legal reverification | Owner-proposed | Hosting clauses, RPO/RTO, revenue bands, on-prem need TO BE PROVIDED. |
| 2026-11-03 | Q17-P1…P3 | Extend/develop PMS + accounting ERP as planning preference; phased options; separated cost categories | Owner-proposed | Not implementation approval; not ERP selection. |
| 2026-11-03 | Q1b-P5…P16, Q1b-C2, Q1b-C3 | JV models, accounting-entity clarification, intercompany agreements, valuation dimensions, correction | Owner-proposed | JV structures and pricing policies TO BE PROVIDED. |
| 2026-11-03 | Q9-P13…P16 | At-risk cost on work package immediately, flagged as exposure | Owner-proposed | — |

## Corrections made after owner review (2026-11-03)

| ID | Correction | Where |
|---|---|---|
| CR-1 | The single "material bridge" identity (commitment = open + closed + stock + incurred) is withdrawn as a universal rule. It is replaced by three separate reconciliations (commitment, inventory, incurred cost) plus an explanatory bridge with explicit valuation differences. Worked examples rerun, and partial delivery, return and cancellation cases added. | requirements.md §3.2–3.5 |
| CR-2 | Pending changes are kept distinct from approved budget and forecast. ETC is defined to exclude remaining commitments, and pending-change cost already in ETC is excluded from probable pending cost. | requirements.md §3.4, Example 10 |
| CR-3 | Claims that "all vendors lack" a capability are restated as "not found in the public documentation reviewed", unless a vendor document states the limitation. | research.md §1 finding 5, §5 legend and matrix, §7.3 R-B5 |
| CR-4 | The statement that uninstalled materials are always excluded from cost-to-cost progress is withdrawn (Q13-C1). | requirements.md CB-08, Example 16; statutory-research E |
| CR-5 | The statement that UAE book-value pricing is available only under a qualifying-group election is withdrawn (Q1b-C3). Each valuation dimension is now researched separately. | statutory-research D.2/D.5 and Part G; requirements.md MW-10 |
| CR-6 | Worked-example hidden assumptions are made explicit: <br>• certified gross ≠ project cost when it contains advances, taxes, duplicated variation amounts or other non-cost items <br>• certificate resubmissions keep history, and posted certificates are adjusted or reversed under control <br>• back-charge treatment is a Finance decision with more than one option <br>• intercompany elimination and revenue examples are conditional | requirements.md §3.9 Examples 11, 13, 15b, 16; SW-05 |
| CR-7 | "One legal entity per project" is clarified as "one accounting entity per project" (Q1b-C2). | requirements.md HR-02 |

## Finance / HR validations required (consolidated)
Q1-F1 accounting standards, FX sources, consolidation · Q1a-P5 translation rules · Q6-F1 direct-to-work vs inventory policy · Q6-F2 overhead policy · Q6a-F1 eligible posted-cost policy · Q6b-F1 FX accrual valuation rate · Q6b-F2 commitment revaluation & group translation · Q7-F1 payroll components per country · Q7-F2 burden/idle allocation · Q7-F3 labour-contractor classification · Q9-F1 definition of "probable" pending change · Q14-O3 FX rule for approval values · Q10-F1 back-charge accounting · Q11-F1 owned-equipment cost basis and under/over-recovery · Q12-F1 inventory valuation method · Q12-F2 excess wastage / abnormal loss classification · Q13-F1 revenue policies per entity and contract.

## Values TO BE PROVIDED by the business
Q15-O1 reporting deadlines · Q16-O1 client/government hosting clauses · Q16-O2 RPO/RTO · Q16-O3 UAE entity revenue bands · Q16-O4 on-premises requirement · Q17-O1 budget, deadline, team, partner tolerance · Q1b-O3 JV structures and agreements · Q1b-O4 intercompany pricing policies and approvers · Q8-O1 contingency amounts/percentages · Q14-O1 approval amounts · Q14-O2 tolerances and approval combinations · Q14-O5 incompatible-duty matrix · Q15a-O1 close calendars and snapshot approvers · Q2 volumes · Q10-O1 retention, caps, advance terms, release triggers, mandatory documents · Q11-O1 equipment rates, idle-time and charging terms · Q12-O1 wastage norms · Q13-O1 retention, advance terms and billing cycles per contract.

## Unresolved items
- Q2 volumes (all), Q3-O1 ERP, Q3-O2 existing systems, Q4-O1 migration, Q1b-O1 JV ownership, Q1b-O2 inter-company, Q5-O1 cost-code administrator, Q5a-O1 equipment master, Q5a-O2 cost-centre categories.
- Q1a-O1 (FX on commitments) is now covered by Q6b-P13 with Finance items Q6b-F1/F2.
- Q8-O1/O2, Q9-O1, Q14-O1…O5, Q15a-O1.
- Contradictions X-1 … X-26 (X-13 resolved in direction).
- Q5a-O1 (equipment master) resolved in direction by Q11-P9, subject to integration verification.
- RT-03 and RT-04 complete (research only). Their professional-validation and Finance items remain open: statutory-research Part D.4 and E.5.
- Batch 5 (Q15, Q16, Q17) and the outstanding owner decisions Q1b-O1 (JV ownership) and Q1b-O2 (intercompany process) were asked on 2026-11-03.
- RT-01 complete; professional-validation items in `statutory-research-india-uae.md` §A.7 and §B.8 remain open. RT-02 complete but partially blocked (missing DB schema, SP definitions, CommonClass source).
- Finance/HR validations listed above.
- Questions Q10–Q13, Q15–Q17 (Batches 4–5).
- All vendor-demo checks Pending verification.
- ERP selection: **not started** — requires explicit instruction.
