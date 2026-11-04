# Construction PMS — Requirements

**DRAFT FOR BUSINESS AND FINANCE VALIDATION — Version 1.0-draft — 2026-11-03**

**Status:** Requirements package covering business decisions from Batches 1–5 (all recorded 2026-11-03). Nothing in this document is approved for design, configuration, migration or go-live until the sign-off items in `signoff-checklist.md` are closed.

**Not decided:**
- No ERP has been selected.
- No implementation architecture has been designed.
- The existing PMS has not been modified.

**Changes in v1.0-draft:**
- Batch 5 rules added:
  - reporting (§6)
  - deployment, security and data (§13)
  - phased scope options (§12)
  - JV and accounting-entity rules (§2.1)
  - intercompany (§4.7)
  - at-risk work (§3.8)
- Hidden assumptions in Examples 11, 13, 15b and 16 made explicit (CR-6).
- Must-have to acceptance-criteria matrix added (§7.1).
- Status separation added (§0.2).

**Earlier versions:** v0.2 → v0.3 → v0.4 (2026-11-03). Their content is incorporated here.

**Changes in v0.4:**
- Batch 4 rules added for subcontracts, equipment, materials, and client billing and revenue.
- Certificate dates are defined explicitly.
- Six new worked examples (11–16).
- A plain-language reference guide (§0.1).
- The statement on uninstalled materials is corrected (Q13-C1).
- RT-02 status is detailed (§11).
**Nature:** Requirements and acceptance criteria only. No design, prototype or implementation.
**Sources:**
- `business-decisions.md` (decision IDs, e.g. Q6-P1)
- `research.md` (sections, e.g. §6.3)
- `vendor-demo-checklist.md` (VD-xx)
- RT-01 / RT-02 research notes (§10–§11 of this document)

**Changes in v0.3:**
- CR-1: three separate reconciliations replace the single bridge identity, and all worked examples were rerun.
- CR-2: ETC, pending-change exposure and forecast are now defined as distinct, non-overlapping amounts.
- Batch 3 rules added: budget, contingency, change orders, approvals, period close.

## 0. How to read this document

| Tag | Meaning |
|---|---|
| **[C]** | Based on a **Confirmed** business decision. |
| **[OP]** | Based on an **Owner-proposed** decision: the direction is intended but still under review. |
| **[FIN]** | The rule's shape is agreed, but its parameter or policy needs Finance/HR approval. |
| **[TBP]** | Values are **to be provided** by the business. None have been invented. |
| **[RT]** | Depends on statutory research RT-01 (India/UAE) and professional validation. |
| **[VD]** | Depends on vendor/ERP verification. |
| **[OPEN]** | No decision has been made yet. |

Research findings are evidence only. **No ERP has been selected.**

### 0.1 Reference guide (plain-language meaning of every code used here)

| Code | Plain-language meaning | Where defined |
|---|---|---|
| **Commitment reconciliation (REC-A)** | For each order, subcontract or hire line: original value + approved amendments − cancelled unfulfilled quantity = revised commitment. Revised commitment = value already fulfilled (net of returns) + value still to be delivered. | §3.2 |
| **Inventory reconciliation (REC-B)** | For each store and item: opening stock + receipts ± transfers − issues − vendor returns ± valuation adjustments − write-offs = closing stock. | §3.3 |
| **Incurred-cost reconciliation (REC-C)** | For each work package, cost code and cost type: management incurred cost = cost already posted in the ERP + approved operational cost not yet posted. PMS posted cost must equal the ERP's eligible ledger, apart from listed exceptions. | §3.4 |
| **Bridges BR-1 to BR-4** | Explanations, not equations, of how fulfilled order value turns into cost or stock. They make price, FX and stock-valuation differences visible. | §3.5 |
| **Commitment rules A-1 to A-5** | Cancellations only touch unfulfilled quantity. Returns either reopen the order (replacement expected) or cancel that quantity. Closing an order cancels only the remainder. Remaining commitment can never be negative. | §3.2 |
| **Posting rules C-1 to C-3** | An ERP posting that carries the PMS line ID replaces the PMS accrual. A late invoice replaces its accrual and adds only the price difference. Postings with missing dimensions stay visible. | §3.4 |
| **Forecast rules F-1 to F-4** | Estimate to complete never includes remaining commitments or earmarked stock. Pending-change cost already in the estimate is not counted again. Approving a change moves cost between columns without adding to it. Reserves are not in work-package budgets until released. | §3.1 |
| **Reserve rules RS-1 to RS-8** | Contingency and client allowances are held as separate reserves at one level only. Nothing can be posted to a reserve. Releases are approved transfers. | §3.6 |
| **Change rules CM-1 to CM-7** | Change classes; four independent statuses (internal authorisation, client approval, subcontract approval, budget posting); pending changes are never budget. | §3.8 |
| **Certificate dates (CD-1 to CD-8)** | The separate dates held on every subcontract certificate and client bill. | §4.3 |
| **Subcontract amounts (SA-1 to SA-9)** | The separate amounts on a subcontract certificate: gross, variations, retention, advance, back-charge, tax, withholding, net. | §4.3 |
| **Candidate statutory requirements ST-01 to ST-21** | Requirements suggested by the India/UAE research, for example separate dates, retention fields and tax identifiers. They are **not approved**. | §10 |
| **At-risk rules AR-01 to AR-05** | Authorised at-risk cost is recorded on the work package straight away and flagged as at-risk exposure. Budget authority and client recovery are tracked separately. | §3.8 |
| **Intercompany rules IC-01 to IC-09** | Controlled agreements between our own entities. Separate valuation dimensions. Reconciliation of both sides. Consolidation adjustments kept separate. | §4.7 |
| **Non-functional rules NF-01 to NF-14** | Hosting, isolation, security, data minimisation, recovery, export and exit, site connectivity. | §13 |

### 0.2 Status separation (what kind of statement each requirement is)

| Category | Meaning | Where to find it |
|---|---|---|
| **Confirmed scope** | Approved by the business owner. So far this is only **Q1-C1**: multinational construction operations, starting with India and UAE, with further countries added through configuration. | §1.1, tag [C] |
| **Proposed decisions** | The owner's intended direction from Batches 1–5. These still need business and Finance validation. | Tag [OP]; `business-decisions.md` |
| **Unresolved facts / values** | Values and facts the business has not yet supplied, such as volumes, thresholds, rates, calendars, JV structures and budget. | Tags [TBP] / [OPEN]; `signoff-checklist.md` §A |
| **External validations** | Items for Finance and HR policy, tax/legal/audit professionals, vendor demonstrations, or missing RT-02 inputs. | Tags [FIN] / [RT] / [VD]; `signoff-checklist.md` §B–D |

---

## 1. Scope and prioritised requirements

### 1.1 Agreed scope
- **[C]** Multi-country operation, starting with **India and UAE**. Further countries are added through configuration (Q1-C1).
- **[OP]** The PMS manages construction operations, budgets, commitments, materials, manpower allocation, subcontractors, equipment, progress and management cost reporting (Q3-P1).
- **[OP]** An accounting system/ERP (not yet selected) holds the statutory GL, AP, AR, tax and payroll postings. The PMS never maintains the statutory GL (Q3-P2, Q6a-P9).
- **[OP]** The PMS supports multiple companies, countries, projects and sites. Capacity figures are **[OPEN]** (Q2).
- **Out of scope for the PMS:**
  - statutory accounting
  - tax calculation logic (the PMS obtains tax treatment from the ERP or a tax service)
  - payroll calculation
  - ERP selection

### 1.2 Prioritised requirements (M = Must, S = Should, F = Future)

| ID | Requirement | Priority | Basis |
|---|---|---|---|
| REQ-001 | Legal entity → Project → Subproject (≤2 nested) → Phase → Work package → Task, with roll-up | M | Q4-P1, Q4-P3, Q4-P8 |
| REQ-002 | Optional group programme spanning projects and entities | S | Q4-P2, Q1b-P3 |
| REQ-003 | Group cost-code library with controlled extensions, versioning and BoQ mapping | M | Q5-P1…P7 |
| REQ-004 | Mandatory allocation dimensions on every project cost line | M | Q5a-P1 |
| REQ-005 | Split allocations that total exactly 100 % with no duplication | M | Q5-P6, Q5a-P3 |
| REQ-006 | Multi-currency amounts on every monetary record | M | Q1a-P2…P4, Q6b-P13 |
| REQ-007 | Management-actual recognition rules by cost type | M | Q6-P1…P9 |
| REQ-008 | Three reconciliations (commitment, inventory, incurred cost) | M | Q6a, Q6b, CR-1 |
| REQ-009 | Commitment relief, matching, reversals and duplicate prevention | M | Q6b-P1…P12 |
| REQ-010 | FX analysis that reconciles to the full functional-currency cost | M | Q6b-P13 |
| REQ-011 | Daily standard labour cost with a monthly payroll true-up | M | Q7-P1…P8 |
| REQ-012 | Exceptions queue for unmatched or invalid postings | M | Q6b-P6 |
| REQ-013 | Locked baseline, versioned revisions and zero-net transfers | M | Q8a-P1…P9 |
| REQ-014 | Controlled contingency reserves | M | Q8-P1…P9 |
| REQ-015 | Change management with classes and four independent statuses | M | Q9-P1…P12 |
| REQ-016 | Configurable role-based approvals and segregation of duties | M | Q14-P1…P13 |
| REQ-017 | Monthly immutable snapshots and controlled reopening | M | Q15a-P1…P11 |
| REQ-018 | Group consolidated reporting in a selected currency | S | Q1a-P5 **[FIN]** |
| REQ-019 | Country tax, e-invoicing, payroll and statutory integrations | M | Q1-P1 **[RT]** |
| REQ-020 | Subcontract measurement, cumulative certificates, retention, advances, back-charges, compliance holds | M | Q10-P1…P8 (§4.3) |
| REQ-021 | Equipment register, rate cards, logs, cost pools and recovery reconciliation | S | Q11-P1…P10 (§4.4) |
| REQ-022 | Client billing measures and revenue inputs to the ERP | S | Q13-P1…P9 (§4.6) |
| REQ-023 | Stores, inspection, transfers, wastage analysis, counts | M | Q12-P1…P13 (§4.1) |
| REQ-024 | Reporting cadence, required report set, dashboards with data freshness | M | Q15-P1…P5 (§6) |
| REQ-025 | Exports that preserve context and access; client extracts that exclude confidential data | M | Q15-P6, Q15-P7 (§6) |
| REQ-026 | Accounting-entity rule and JV models | M | Q1b-C2, Q1b-P5…P9 (§2.1) |
| REQ-027 | Controlled intercompany agreements, separate valuation fields, two-sided reconciliation | M | Q1b-P10…P16 (§4.7) |
| REQ-028 | At-risk work recorded immediately and flagged as exposure | M | Q9-P13…P16 (§3.8) |
| REQ-029 | Security, isolation, access control, audit, retention, personal-data minimisation | M | Q16-P3, Q16-P4 (§13) |
| REQ-030 | Recovery targets, backup and DR locations, export and exit | M | Q16-P3 (§13); values **[TBP]** |
| REQ-031 | Mobile use and unreliable site connectivity | S | Q16-P3 (§13) |

---

## 2. Project hierarchy and dimension rules

### 2.1 Hierarchy
- **HR-01 [OP]** The hierarchy is Legal entity → Project → Subproject → (Subproject) → Phase → Work package → Task (Q4-P1, Q4-P3). The limits of a two-level subproject structure are listed in business-decisions L-1.
- **HR-02 [OP]** A project belongs to exactly one **accounting entity**. Its subprojects stay in that accounting entity but may have a different site (Q1b-P2, Q1b-C2, Q4-P4).
  - An accounting entity is either a legal entity, or a Finance/legal-approved accounting arrangement such as a separately accounted non-company JV.
- **HR-03 [OP]** A multi-company development is modelled as separate linked projects under a programme (Q1b-P3).
- **HR-08 [OP]** JV models (Q1b-P5…P9):
  - **(a)** The JV has its own books: the JV is the project's accounting entity, or has its own ledger arrangement. This needs Finance/legal approval.
  - **(b)** The lead partner accounts for the project: the lead partner's entity owns it, and JV participation is recorded separately.
  - **(c)** The partners keep separate accounts: linked projects under a programme.
- **HR-09 [OP]** Partner shares are effective-dated, with agreement references and approved reporting rules. The PMS never consolidates a JV automatically because of its ownership percentage or a "JV" label. Finance sets the treatment (Q1b-P8, Q1b-P9). Actual JV structures are **[TBP]** (Q1b-O3).
- **HR-04 [OP]** Only a designated **posting work package** carries budget, commitment and cost lines. Reserves (§3.6) are the only budget type held above work-package level (Q4-P5, Q8-P3).
- **HR-05 [OP]** Task-level records point to exactly one posting work package, so their cost is counted once (Q4-P7).
- **HR-06 [OP]** All measures roll up through the hierarchy and the programme. Translation rules for cross-currency roll-up are **[FIN]** (Q4-P8, X-4).
- **HR-07 [OPEN]** Migration of existing records depends on the RT-02 findings (§11). No mapping is assumed (Q4-O1).

### 2.2 Cost codes
- **CC-01** Group library, structured Division → Section → Element [OP] (Q5-P1, Q5-P2).
- **CC-02** Entity extensions sit beneath group codes and never change them [OP] (Q5-P3).
- **CC-03** Owned by the group cost-control administrator [OP]. The person or role is **[TBP]** (Q5-P4, Q5-O1).
- **CC-04** Changes are versioned and closed reports keep the code version that was valid at the time [OP] (Q5-P5).
- **CC-05** Client BoQ items map many-to-one or by controlled split. Splits total 100 %, and a BoQ item never carries its own budget or cost on top of its mapped lines [OP] (Q5-P6).
- **CC-06** Client billing quantities are kept separate from cost-control quantities [OP] (Q5-P7).

### 2.3 Dimensions and master ownership
- **DM-01 [OP]** Every allocation carries:
  - legal entity
  - project and posting work package (parent levels are derived)
  - cost code
  - cost type (Materials, Labour, Subcontract, Equipment, Overhead)
  - site where applicable
  - source document and line
  - transaction currency and conversion details (rate, rate date, rate source, functional amount)

  Basis: Q5a-P1, Q4-P6, Q1a-P4.
- **DM-02 [OP]** Cost centres are kept separate from the hierarchy. A cost centre is optional on project cost and required on the overhead and non-project categories Finance defines (Q5a-P2). The category list is **[FIN]**.
- **DM-03 [OP]** Splits total 100 % of the source line and keep a link back to it (Q5a-P3).
- **DM-04 [OP] [VD]** Master-data ownership:
  - **ERP:** legal entities, GL accounts, tax configuration, vendors.
  - **HR/payroll:** employees.
  - **PMS:** hierarchy, cost-code library, crews, operational allocations.
  - **Equipment master:**
    - **PMS:** operational data and allocations.
    - **ERP:** fixed assets, depreciation and lease accounting.

    This split is resolved in direction (Q11-P9) and is subject to integration verification **[VD]**.

---

## 3. Budget and cost definitions, formulas and worked examples

### 3.1 Measures
All amounts are in functional currency unless stated otherwise.

| Symbol | Measure | Definition | Basis |
|---|---|---|---|
| **OB** | Original budget | Approved baseline per posting line. Never overwritten. | Q8a-P2 |
| **ADJ** | Baseline corrections | Approved adjustment documents (class: baseline correction) | Q8a-P2, Q9-P1 |
| **ACH** | Approved changes | Internal budget changes and client variations with budget posting status = posted | Q9-P2 |
| **NTR** | Net transfers | Approved transfers in minus transfers out, including contingency releases received | Q8a-P4, Q8-P5 |
| **RB** | Revised budget (work package line) | **RB = OB + ADJ + ACH + NTR** | Q8a-P9 |
| **RSV** | Remaining reserve | Reserve original + additions − releases + returns (§3.6) | Q8-P6 |
| **RCV** | Revised commitment value | Original + approved amendments − cancellations (commitment reconciliation, §3.2) | Q6b-P9 |
| **FUL** | Fulfilled value | Quantity received, certified or accepted, minus returns, × commitment price (§3.2) | Q6b-P7, Q6b-P8 |
| **REM** | Remaining commitment | RCV − FUL: value still to be delivered or certified (§3.2) | Q6b-P7 |
| **INV** | Project-held inventory | Closing stock held for the project (inventory reconciliation, §3.3). An asset, not project cost. | Q6-P3, Q6-P9 |
| **PC** | Posted project cost | Eligible ERP postings. Excludes payments, advances, inventory balances, recoverable taxes and other non-expense items. | Q6a-P3 **[FIN]** |
| **UA** | Unposted operational accrual | Approved operational cost that no ERP posting yet represents | Q6a-P4 |
| **IC** | Management incurred cost | **IC = PC + UA** (REC-C) | Q6a-P2 |
| PV / FXV / IVD / LV | Price, FX, inventory-valuation and labour differences | Shown explicitly. These are analysis amounts inside the measure they affect, never deducted from PC. | Q6b-P9, Q6b-P13, Q7-P6, CR-1 |
| **ETC** | Estimate to complete | Estimated cost of remaining **approved** scope that is **not** covered by REM or by INV earmarked for the project. **ETC excludes remaining commitments.** | Q8a-Q1, CR-2 |
| **CTC** | Cost to complete | **CTC = REM + INV(earmarked) + ETC** | CR-2 |
| **EAC** | Estimate at completion, approved scope | **EAC = IC + CTC = IC + REM + INV(earmarked) + ETC** | research.md §6.3 |
| **PCE** | Pending change exposure | Estimated cost of changes not yet approved (cost status ≠ posted). Shown for information; never part of RB. | Q9-P4 |
| **PPC** | Probable pending cost | For each pending change classed as probable under policy Q9-F1: its estimated cost minus any part already included in ETC | Q9-P5, Q9-P6 |
| **EACp** | EAC including probable changes | **EACp = EAC + PPC** | Q9-P5 |
| **VAC** | Variance at completion (line) | **VAC = RB − EAC** | Q8a-P9 |
| **VACp** | Project variance after reserve and probable changes | **VACp = ΣRB + RSV − ΣEACp** | Q8-P9, Q9-P12 |

**No-double-count rules for the forecast:**
- **F-1.** ETC never includes REM or earmarked INV. When a commitment is approved, the estimate it covers is removed from ETC in the same transaction (moved, not added).
- **F-2.** Each pending change records how much of its estimated cost is already inside ETC. PPC counts only the remainder.
- **F-3.** When a change is approved and budget-posted, its PPC becomes 0, and its cost goes to ETC or REM. EACp does not change as a result of the approval itself.
- **F-4.** A reserve is never part of any work-package RB until it is released (§3.6).

### 3.2 REC-A: Commitment reconciliation
Applies to each commitment line (PO, subcontract, hire agreement), measured at commitment unit price and PO baseline rate.

```
Approved original value (AOV)
+ Approved amendments (AMD)            each a new approved version; may be + or −
− Cancellations of unfulfilled qty (CAN)
= Revised commitment value (RCV)

RCV = Fulfilled value (FUL) + Remaining commitment (REM)
FUL = (qty received / certified / accepted − qty returned) × commitment unit price
```

Rules:
- **A-1.** CAN may only cover quantity that has not been fulfilled, so it never overlaps FUL.
- **A-2.** A return that **expects a replacement** reduces FUL and increases REM.
- **A-3.** A return with **no replacement expected** reduces FUL and records an equal CAN, so REM is unchanged.
- **A-4.** Closing a document records CAN for the remaining unfulfilled balance only. Receipts, FUL, cost, accruals and liabilities are unaffected.
- **A-5.** REM can never be negative. Over-delivery or over-certification above RCV needs an approved amendment first (Q14-P7).

### 3.3 REC-B: Inventory reconciliation
Applies to each project or site store and item, at inventory valuation.

```
Opening inventory
+ Receipts (at receipt valuation)
+ Transfers in − Transfers out
− Issues to work packages (at issue valuation)
− Returns to vendor
± Valuation adjustments (e.g. invoice price differences capitalised to stock, revaluation)
− Write-offs / wastage / count differences (per policy)
= Closing inventory (INV)
```

- The valuation method (e.g., moving average) is **[FIN]**.
- Wastage is analysis only (MW-11, MW-12). Its accounting classification is **[FIN]** (Q12-F2).
- Inter-store transfers net to zero across stores of the same entity. Movements between entities are inter-company and **[OPEN]** (Q1b-O2).

### 3.4 REC-C: Incurred-cost reconciliation
Applies to each entity × project × work package × cost code × cost type.

```
IC = PC + UA
PC = Σ eligible ERP postings (PO-matched, issue journals, payroll allocations, non-PO, journals), each with its source reference
UA = Σ approved operational events not represented by a matched ledger posting
ERP eligible ledger − PMS PC = 0 ± listed exceptions
```

- **C-1.** An ERP posting that carries a PMS source-line ID removes the matching UA. The PMS reads the ERP posting rather than re-entering it (Q6a-P4, Q6a-P6).
- **C-2.** A late posting that **replaces** an accrual removes the accrual. Only a difference in value is a new adjustment (Q15a-P10).
- **C-3.** Valid postings with incomplete dimensions stay in IC at the lowest known level and also appear in exceptions (Q6b-P6).

### 3.5 Bridges between the reconciliations
These bridges explain differences. They are not identities.

- **BR-1 Direct-to-work line:** FUL (at commitment price and rate) + PV + FXV = the IC contributed by that line.
- **BR-2 Line delivered to a store:**
  - FUL (at commitment price and rate) + receipt valuation difference = REC-B receipts for that line.
  - The receipt valuation difference covers invoice price differences capitalised to stock and FX.
- **BR-3 Issues from a store:** issue valuation follows REC-B. Any difference from a particular PO's price is an **inventory valuation difference (IVD)**. It stays in REC-B and is not attributed to that PO.
- **BR-4 Cost not linked to a commitment:** non-PO cost, labour and internal equipment appear only in REC-C and never in REC-A.

### 3.6 Contingency and reserves
- **RS-1 [OP]** A reserve is a separate budget type held at project or subproject level, and at one level only. Parent totals include it once (Q8-P1, Q8-P3).
- **RS-2 [OP]** Contractor contingency is kept separate from client allowances and provisional sums. Each has its own reserve type (Q8-P2).
- **RS-3 [OP]** A reserve cannot receive cost postings or commitments (Q8-P4).
- **RS-4 [OP]** A release is an approved transfer from the reserve to a posting work package and cost code. It records the reason, a risk reference where available, and the approval. A return moves budget back into the reserve (Q8-P5, Q8-P6).
- **RS-5 [OP]** A release to a work package in a different subproject follows the stricter of the contingency-release and cross-subproject-transfer approval rules (X-17, proposed).
- **RS-6 [OP]** The project's total available cost budget is ΣRB + RSV. A release moves value between the two and leaves the total unchanged (Q8-P9).
- **RS-7 [OP]** Contractor contingency is internal by default. Client-owned allowances are disclosed according to the contract (Q8-P7).
- **RS-8 [TBP]** Reserve amounts or percentages and the release approvers. No default percentage is applied (Q8-P8, Q8-O1).

### 3.7 Budget revisions and transfers
- **BV-1 [OP]** The original baseline is locked when approved and kept permanently. Corrections are made through approved adjustment documents (Q8a-P2).
- **BV-2 [OP]** Every budget version keeps its reason, supporting documents, approvals and history (Q8a-P3).
- **BV-3 [OP]** A transfer must net to zero within one project and one legal entity. Transfers between legal entities or unrelated projects are rejected; these need separately approved budget changes (Q8a-P4, Q8a-P6).
- **BV-4 [OP]** A transfer between subprojects needs higher-level approval **[TBP]** (Q8a-P5).
- **BV-5 [OP]** Cost and revenue budgets are kept separate (Q8a-P7).
- **BV-6 [OP]** Forecast versions (ETC) never change budgets (Q8a-P8).

### 3.8 Change management
- **CM-1 [OP]** Change classes:
  - baseline correction
  - internal budget change
  - client variation
  - budget transfer
  - subcontract variation
  - contingency release

  Basis: Q9-P1.
- **CM-2 [OP]** Each change has four independent statuses: internal cost authorisation, client commercial approval, subcontract approval and budget posting. Client approval never sets the other three statuses automatically (Q9-P2, Q9-P3).
- **CM-3 [OP]** Each change records its proposed, submitted, assessed, approved, rejected and withdrawn amounts and dates. Rejected and superseded records are kept (Q9-P7, Q9-P11).
- **CM-4 [OP]** One client change can link to several cost allocations and subcontract variations. Differences between client price, internal cost and subcontract value are shown and explained (Q9-P8, Q9-P9).
- **CM-5 [OP]** Pending changes feed PCE and PPC only, following rules F-2 and F-3 (Q9-P4…P6).
- **CM-6 [OP]** Forecast margin and unrecovered exposure are reported. Pending revenue is never treated as approved revenue (Q9-P12).
- **CM-7 [OP]** At-risk work goes through a separate exception workflow (Q9-P10), governed by AR-01 to AR-05.
- **AR-01 [OP]** Authorised at-risk cost is recorded **immediately** against the correct posting work package. It is never parked outside project cost while client approval is awaited (Q9-P13, Q9-P15).
- **AR-02 [OP]** Each at-risk cost line carries an at-risk flag and a link to its change record. Reports show it inside IC and also in a separate **at-risk exposure** measure (Q9-P14).
- **AR-03 [OP]** At-risk authorisation is a distinct approval. If the resulting commitment exceeds available RB, the over-budget amount is approved explicitly under AP-07 and reported as at-risk (X-28).
- **AR-04 [OP]** Budget authority (internal), expected recovery (client) and revenue treatment (Finance) are separate fields and statuses. None of them sets another automatically (Q9-P16, CM-2).
- **AR-05 [OP]** If recovery is later rejected, the cost stays where it is and the exposure is reclassified as unrecovered. No cost is moved out of the project (Q9-P15).

### 3.9 Worked examples (rerun under REC-A/B/C)
Common data, unless stated otherwise:
- INR entity IN-01, project P-100, work package A.01, cost code 03-30-10, RB = 100,000.
- Amounts exclude recoverable tax.
- All figures are illustrative.

#### Example 1: Direct-to-work material
PO-1: 10 units × 9,000.

| Step | Event | REC-A: RCV / FUL / REM | REC-C: UA / PC / IC | BR-1 check |
|---|---|---|---|---|
| 0 | PO approved | 90,000 / 0 / 90,000 | 0 / 0 / 0 | — |
| 1 | Receipt of 10 units approved | 90,000 / 90,000 / 0 | 90,000 / 0 / 90,000 | FUL 90,000 = IC 90,000 |
| 2 | Invoice of 90,000 posted with the PO-line ID | 90,000 / 90,000 / 0 | 0 / 90,000 / 90,000 | unchanged |

- **Nothing omitted:** the cost appears at receipt.
- **Nothing counted twice:** at step 2 the UA is replaced by PC (C-1).
- EAC with ETC = 0 is 90,000 at every step.

#### Example 2: Site inventory, partial consumption and a valuation difference
- The site store holds 20 bags at 380, worth 7,600.
- PO-2: 100 bags × 400 = 40,000, delivered to the store.
- The invoice comes in at 405 per bag, so the 500 difference is capitalised to stock under a moving-average policy assumed **[FIN]**.
- 60 bags are issued to A.01.

| Step | Event | REC-A: RCV / FUL / REM | REC-B: INV (qty) | REC-C: UA / PC / IC |
|---|---|---|---|---|
| 0 | Opening | 40,000 / 0 / 40,000 | 7,600 (20) | 0 / 0 / 0 |
| 1 | 100 bags received into the store | 40,000 / 40,000 / 0 | 47,600 (120) | 0 / 0 / 0 |
| 2 | Invoice of 40,500 posted to inventory | 40,000 / 40,000 / 0 | 48,100 (120); valuation adjustment +500 | 0 / 0 / 0 (inventory is not project cost) |
| 3 | 60 bags issued at average 400.8333 | unchanged | 24,050 (60) | 24,050 / 0 / 24,050 |
| 4 | ERP posts the issue journal of 24,050 with the issue-line ID | unchanged | 24,050 (60) | 0 / 24,050 / 24,050 |

- **REC-A:** RCV 40,000 = FUL 40,000 + REM 0.
- **REC-B:** 7,600 + 40,000 + 500 − 24,050 = 24,050.
- **BR-2:** FUL 40,000 + receipt valuation difference 500 = receipts into REC-B of 40,500.
- **BR-3:** issuing 60 bags at the PO price of 400 would give 24,000. The extra 50 is IVD, explained in REC-B and not charged to PO-2.
- **EAC for A.01's cement** = IC 24,050 + INV earmarked 24,050 + ETC 0 = 48,100. This equals all stock available to the project. The 20 opening bags were already project stock, so nothing is omitted or duplicated.

#### Example 3: Partial receipts and partial invoices
PO-3: 10 units × 9,000, direct-to-work.

| Step | Event | RCV / FUL / REM | UA / PC / IC | Of which PV |
|---|---|---|---|---|
| 0 | PO approved | 90,000 / 0 / 90,000 | 0 / 0 / 0 | 0 |
| 1 | 6 units received | 90,000 / 54,000 / 36,000 | 54,000 / 0 / 54,000 | 0 |
| 2 | Invoice for 4 units at 9,000 (36,000) | unchanged | 18,000 / 36,000 / 54,000 | 0 |
| 3 | 4 more units received | 90,000 / 90,000 / 0 | 54,000 / 36,000 / 90,000 | 0 |
| 4 | Invoice for 6 units at 9,100 (54,600), covering two receipts | unchanged | 0 / 90,600 / 90,600 | 600 |

- **BR-1:** FUL 90,000 + PV 600 = IC 90,600.
- **EAC** (ETC = 0) moves through 90,000 → 90,000 → 90,000 → 90,000 → 90,600 (IC + REM at each step). It is never above 90,600.

#### Example 4: ERP receipt accrual followed by invoice
This is Example 3 step 1, but the ERP also posts a GRNI accrual of 54,000 (Dr project expense, Cr GRNI) carrying the PMS receipt-line ID.

| Step | Event | UA / PC / IC | If C-1 were ignored |
|---|---|---|---|
| 1 | Receipt approved in the PMS | 54,000 / 0 / 54,000 | 54,000 |
| 1a | ERP posts the GRNI accrual | 0 / 54,000 / 54,000 | 108,000 (double count) |
| 2 | Invoice posted (Dr GRNI, Cr AP); no new expense | 0 / 54,000 / 54,000 | 108,000 |

- If the ERP reverses its accrual at the start of the month, PC goes down and the UA comes back until the invoice is posted.
- This relies on the ERP exposing postings at line level **[VD]** (VD-18).

#### Example 5: Return followed by credit note
PO-5: 10 units × 9,000, direct-to-work, fully received and invoiced. Two units are defective.

**5a: no replacement expected**

| Step | Event | AOV / CAN / RCV / FUL / REM | UA / PC / IC |
|---|---|---|---|
| 0 | After receipt and invoice | 90,000 / 0 / 90,000 / 90,000 / 0 | 0 / 90,000 / 90,000 |
| 1 | Return of 2 units approved | 90,000 / 18,000 / 72,000 / 72,000 / 0 | −18,000 / 90,000 / 72,000 |
| 2 | Credit note of −18,000 posted, linked to the return | unchanged | 0 / 72,000 / 72,000 |

**5b: replacement expected, supplier replaces free of charge with no credit note**

| Step | Event | RCV / FUL / REM | UA / PC / IC | EAC |
|---|---|---|---|---|
| 1 | Return of 2 units approved | 90,000 / 72,000 / 18,000 | −18,000 / 90,000 / 72,000 | 90,000 |
| 2 | 2 replacement units received | 90,000 / 90,000 / 0 | 0 / 90,000 / 90,000 | 90,000 |

- **Economic cost is reversed once:**
  - In 5a, the return's negative accrual is replaced by the credit note.
  - In 5b, the negative accrual is cancelled by the replacement receipt.
- **The commitment reopens only in 5b** (rule A-2).

#### Example 6: Non-PO project expense
AED entity AE-01. A generator repair of AED 2,500 goes through the controlled non-PO workflow.

| Case | REC-C: UA / PC / IC | Exceptions |
|---|---|---|
| Posted with full dimensions | 0 / 2,500 / 2,500 at P-200 / B.03 | 0 |
| Posted to P-200 without a work package or cost code | 0 / 2,500 / 2,500 at project level | 1 item, AED 2,500 |

There is no REC-A entry (BR-4), and the cost is never dropped (C-3).

#### Example 7: Foreign-currency purchase with a rate difference
- AE-01 (functional currency AED) issues PO-7 for USD 50,000, direct-to-work.
- PO baseline rate 3.6700, giving AED 183,500.
- The accrual rate policy is **[FIN]**; the receipt-date rate of 3.6720 is assumed.
- Invoice rate 3.6800.

| Step | Event | REC-A (AED at PO rate): RCV / FUL / REM | REC-C (AED): UA / PC / IC | FXV vs baseline |
|---|---|---|---|---|
| 0 | PO approved | 183,500 / 0 / 183,500 | 0 / 0 / 0 | — |
| 1 | Receipt approved | 183,500 / 183,500 / 0 | 183,600 / 0 / 183,600 | 100 |
| 2 | Invoice posted | unchanged | 0 / 184,000 / 184,000 | 500 |

- **BR-1:** FUL 183,500 + FXV 500 = IC 184,000.
- Posted actual is reported at the full 184,000, never net of FX.
- **In transaction currency:** USD 50,000 committed, fulfilled and incurred.

#### Example 8: Labour standard cost with payroll true-up
- AE-01, crew of 10, P-200 / B.03, one month.
- Standard rate AED 25/h, with components **[FIN]**.
- 1,760 productive hours, 80 idle hours.
- Overtime premium estimated at 900.
- Payroll total AED 47,000, including actual OT premium of 1,000.

| Step | Event | UA / PC / IC on B.03 | Idle / unallocated |
|---|---|---|---|
| 1 | Approved timesheets: 1,760 h × 25 + OT estimate 900 | 44,900 / 0 / 44,900 | 80 h visible |
| 2 | Payroll true-up: OT 1,000 assigned directly; 46,000 allocated by hours (1,760/1,840 → 44,000; 80/1,840 → 2,000) | 0 / 45,000 / 45,000 | AED 2,000, shown separately **[FIN]** Q7-F2 |

- **LV** = 45,000 − 44,900 = 100.
- Payroll of 47,000 = 45,000 on B.03 + 2,000 idle.
- The standard cost is replaced by actual payroll, not added to it.
- No REC-A entry (BR-4).

#### Example 9: Partial delivery, amendment and cancellation
PO-9: 10 units × 9,000 = 90,000, direct-to-work.

| Step | Event | AOV / AMD / CAN / RCV | FUL / REM | UA / PC / IC |
|---|---|---|---|---|
| 0 | PO approved | 90,000 / 0 / 0 / 90,000 | 0 / 90,000 | 0 / 0 / 0 |
| 1 | Amendment +2 units approved (version 2) | 90,000 / 18,000 / 0 / 108,000 | 0 / 108,000 | 0 / 0 / 0 |
| 2 | 7 units received and invoiced (63,000) | unchanged | 63,000 / 45,000 | 0 / 63,000 / 63,000 |
| 3 | Supplier cannot deliver the remaining 5 units; balance cancelled and PO closed | 90,000 / 18,000 / 45,000 / 63,000 | 63,000 / 0 | 0 / 63,000 / 63,000 |

- **REC-A:** RCV 63,000 = FUL 63,000 + REM 0.
- **Closing the PO left FUL, PC and the unpaid liability unchanged** (rule A-4).
- **If the cancelled scope is still needed:** its estimate must be added back to ETC with a reason, or EAC will understate (rule F-1).

#### Example 10: Pending change, contingency release and approval
- INR project P-300, work package C.01.
- OB = 1,000,000. Contractor contingency RSV = 100,000, held at project level.
- Starting position: IC 600,000, REM 200,000, ETC 150,000.

| Step | Event | RB C.01 | RSV | REM | ETC | EAC | PCE | PPC | EACp | VAC | VACp |
|---|---|---|---|---|---|---|---|---|---|---|---|
| 0 | Start | 1,000,000 | 100,000 | 200,000 | 150,000 | 950,000 | 0 | 0 | 950,000 | 50,000 | 150,000 |
| 1 | PCO-1 raised: client extra works, estimated cost 80,000 (price 100,000), classed probable; PM had already put 30,000 of it into ETC | 1,000,000 | 100,000 | 200,000 | 150,000 | 950,000 | 80,000 | 50,000 | 1,000,000 | 50,000 | 100,000 |
| 2 | Ground condition raises ETC by 40,000; contingency release of 40,000 to C.01 approved | 1,040,000 | 60,000 | 200,000 | 190,000 | 990,000 | 80,000 | 50,000 | 1,040,000 | 50,000 | 60,000 |
| 3 | PCO-1 approved on all four statuses; budget +80,000 posted; the remaining 50,000 of its cost moves into ETC | 1,120,000 | 60,000 | 200,000 | 240,000 | 1,040,000 | 0 | 0 | 1,040,000 | 80,000 | 140,000 |
| 4 | Subcontract variation of 60,000 issued for PCO-1 scope | 1,120,000 | 60,000 | 260,000 | 180,000 | 1,040,000 | 0 | 0 | 1,040,000 | 80,000 | 140,000 |

(IC stays 600,000 throughout. VACp = RB + RSV − EACp.)

- **No double count:**
  - At step 1, only the 50,000 not already in ETC is added (rule F-2).
  - At step 3, EACp is unchanged by the approval (rule F-3).
  - At step 4, cost moves from ETC to REM and EAC is unchanged (rule F-1).
- **Pending change kept out of budget:** RB rises only at step 3, when the budget posting status is set (CM-2).
- **Contingency counted once:** at step 2, RB + RSV stays at 1,100,000 (RS-6).
- **Revenue:** the 100,000 price is reported as pending revenue until client approval and is never treated as approved revenue (CM-6). The revenue budget is kept separately (BV-5).
- **VACp check, step 3:** 1,120,000 + 60,000 − 1,040,000 = 140,000. This equals the step-2 VACp (60,000) plus the 80,000 approved budget increase. Recovering the change improves the margin position, and no amount is double counted.

#### Example 11: Cumulative subcontract certification with retention and advance recovery
Subcontract SC-11 in AED.
- Original value: 2,000,000.
- Retention: 10 %, capped at 5 % of the original value (100,000).
- Advance: 200,000 paid against a guarantee, recovered at 10 % of cumulative gross.
- These values are **illustrative**; the real ones are TO BE PROVIDED.
- A variation of +100,000 is approved before certificate 2.
- Recoverable VAT is excluded.

**Conditions this example relies on (CR-6):**
- The "cumulative gross" column contains **only** work performed: original scope (SA-1) plus approved variations (SA-2).
- Variation work is certified once, under SA-2. It is never also included in SA-1.
- Advances, recoverable tax, withholding and other non-cost items are **not** in the gross. If a certificate's gross does include them, they are separated out before cost is recognised (rule SC-1 in §4.3).
- No certificate in this example has been posted to the ERP before it is replaced.

| Cert | Cumulative gross (original + variation) | Current gross | Retention cum / current | Advance recovered cum / current | Current net payable | RCV / FUL / REM | IC cumulative |
|---|---|---|---|---|---|---|---|
| 1 | 300,000 (300,000 + 0) | 300,000 | 30,000 / 30,000 | 30,000 / 30,000 | 240,000 | 2,000,000 / 300,000 / 1,700,000 | 300,000 |
| 2 | 850,000 (800,000 + 50,000) | 550,000 | 85,000 / 55,000 | 85,000 / 55,000 | 440,000 | 2,100,000 / 850,000 / 1,250,000 | 850,000 |
| 3 | 1,300,000 (1,200,000 + 100,000) | 450,000 | 100,000 / 15,000 (cap reached) | 130,000 / 45,000 | 390,000 | 2,100,000 / 1,300,000 / 800,000 | 1,300,000 |
| 4 (final) | 2,100,000 | 800,000 | 100,000 / 0 | 200,000 / 70,000 (fully recovered) | 730,000 | 2,100,000 / 2,100,000 / 0 | 2,100,000 |

- **Cost equals the certified value of work performed.** Under the conditions above, IC equals the cumulative gross certified. Retention and advance recovery never reduce it (SW-06, Q10-P5). If those conditions do not hold, IC is gross certified minus any non-cost items it contains (SC-1).
- **Cash reconciles.** Cumulative net payable = 2,100,000 − 100,000 − 200,000 = **1,800,000**, which equals 240,000 + 440,000 + 390,000 + 730,000. The advance balance falls from 200,000 to 0 and is never project cost.
- **Commitment reconciliation (§3.2) holds at every certificate:** RCV = FUL + REM.
- **Duplicate control, certificate not yet posted.** If certificate 3 is resubmitted as version 2 at cumulative 1,320,000, version 2 becomes the effective version. The current gross becomes 470,000, not a further 1,320,000. Version 1 is **kept in history** with its status, approver and reason for supersession (SW-05).
- **Duplicate control, certificate already posted to the ERP.** Version 1 cannot be replaced silently. The 20,000 difference is handled through a controlled adjustment certificate, or through a reversal of version 1 and re-issue. Either way it is approved and linked to the original. This creates exactly one additional cost and liability of 20,000 in the period of the adjustment (SC-2, PE-07).

#### Example 12: Retention release
Continuing SC-11 after certificate 4: retention held is 100,000.

| Step | Event | Retention held | Payable (SA-4 released) | IC |
|---|---|---|---|---|
| 1 | Taking-over: 50 % released | 50,000 | 50,000 | 2,100,000 |
| 2 | End of defects period: balance released | 0 | 50,000 | 2,100,000 |

- **Project cost is unchanged.** Releasing retention settles a liability; it is not a new cost (SW-06).
- **Cash total.** Total paid to the subcontractor = 1,800,000 net + 100,000 retention = **2,100,000**. This equals the cost incurred, net of the advance that was paid and then fully recovered.
- **Defect rectification by others.** If others had to rectify defects, that cost and any recovery from the subcontractor follow Example 13. They are not netted silently against the release.

#### Example 13: Disputed back-charge
- Our own labour clears SC-11's debris at a cost of AED 8,000.
- That cost is on work package G.01 (site general), already posted after the payroll true-up: PC 8,000.
- Back-charge BC-1 is raised to SC-11.
- Finance treatment Q10-F1 is pending. Treatment **A** is shown: an accepted recovery reduces cost on the source line.

| Step | BC-1 status | IC on G.01 | SC-11 gross cost | SC-11 deductions (SA-7) | Note |
|---|---|---|---|---|---|
| 1 | Proposed (8,000) | 8,000 | 2,100,000 | 0 | A proposed back-charge has no cost effect (SW-10) |
| 2 | Notified → Disputed | 8,000 | 2,100,000 | 0 | Disputed amount shown in the back-charge register only |
| 3 | Accepted at 5,000 | 3,000 (UA −5,000 until the ERP posts the recovery) | 2,100,000 | 5,000 on the next certificate | Recovery linked to the source cost line |
| 4 | ERP posts recovery journal with the BC-1 link | 3,000 (PC 3,000, UA 0) | 2,100,000 | 5,000 | Incurred-cost reconciliation (§3.4) holds |
| 5 | Attempt to deduct BC-1 again on a later certificate | 3,000 | 2,100,000 | rejected | Each back-charge can be deducted only once (SW-10) |

- **No double reduction.** The subcontract's gross cost stays 2,100,000. The recovery reduces only the source cost (G.01), and only once.
- **Recovery cap in this example.** Recoveries are capped at the accepted back-charge (5,000). The 8,000 source cost is the upper limit only because this example uses treatment A.
- **Treatment B (other income).** G.01 stays at 8,000, and the 5,000 is shown as a recovery row outside project cost.
- **Treatment C (work not performed).** If the back-charge relates to subcontract work that was certified but not actually performed, Finance may instead reduce the subcontract's certified value. In that case G.01 is unaffected.
- **Finance decides.** Treatments A, B and C are illustrations. Q10-F1 decides which apply. Integration is checked in VD-25. Under every treatment the 5,000 has an effect exactly once (CR-6).

#### Example 14: Equipment cost recovery (owned excavator, rate including operator and fuel)
- Entity IN-01. Excavator EQ-1 has an internal rate of INR 2,500/h. The rate **includes** operator and fuel.
- Month: 150 working h on A.01, 30 working h on B.02, 20 h idle (not charged under the assumed policy, **[TBP]**), 10 h breakdown (not charged).

| Line | Amount (INR) | Goes to |
|---|---|---|
| Internal charge A.01: 150 × 2,500 | 375,000 | Work package A.01 |
| Internal charge B.02: 30 × 2,500 | 75,000 | Work package B.02 |
| **Total internal charges (pool recovery)** | **450,000** | Equipment pool EQ-1 (credit) |
| Depreciation (ERP) | 200,000 | Pool |
| Operator payroll (timesheets booked to EQ-1, not to A.01/B.02) | 90,000 | Pool |
| Fuel issued from store to EQ-1 | 100,000 | Pool |
| Maintenance | 40,000 | Pool |
| **Pool actual cost** | **430,000** | — |
| **Over-recovery** (450,000 − 430,000) | **20,000** | Cost-recovery report (EW-07) |

- **No double charging.** Operator hours and fuel go to the pool only. An attempt to book the operator's timesheet or a fuel issue to A.01 would be flagged and rejected (EW-06).
- **Entity view.** The 450,000 of internal charges is eliminated against the pool recovery, so entity cost = **430,000** (EW-08).
- **Project view pending Finance (Q11-F1):**
  - **Option 1 (internal charge):** A.01 375,000 and B.02 75,000, with the 20,000 over-recovery held in the pool.
  - **Option 2 (pool actual allocated by working hours):** A.01 = 430,000 × 150/180 = **358,333**; B.02 = 430,000 × 30/180 = **71,667**; total 430,000.
  - Under either option, projects + pool remainder = 430,000.

#### Example 15: Material transfers
**15a: Same legal entity (IN-01), central store to Tower B site store.** 50 t rebar at moving-average book value 60,000/t.

| Step | Event | Central store | Goods in transit | Tower B store | Entity total stock | Project IC |
|---|---|---|---|---|---|---|
| 0 | Opening | 3,000,000 | 0 | 0 | 3,000,000 | 0 |
| 1 | Dispatch (delivery note or, where legally required, a tax document; see RT-04) | 0 | 3,000,000 | 0 | 3,000,000 | 0 |
| 2 | Received at site | 0 | 0 | 3,000,000 | 3,000,000 | 0 |
| 3 | 20 t issued to work package A.01 | 0 | 0 | 1,800,000 | 1,800,000 | 1,200,000 |

- The movement creates no cost. Cost arises only at issue (MW-09).
- **Inter-state case (India).** If the two stores are different GST registrations of IN-01, RT-04 indicates the transfer is a taxable supply. It needs a tax invoice with a value basis under the GST valuation rules, and an e-way bill. That tax value is held in its own field. Management stock value stays 3,000,000, and no project cost is created (professional validation required).

**15b: Different legal entities (IN-01 to IN-02).** 5 t at an approved intercompany price of 62,000/t (illustrative; policy **[OPEN]**, Q1b-O2). IN-01 book value is 60,000/t.

| View | IN-01 | IN-02 | Group (after elimination) |
|---|---|---|---|
| Stock movement | −300,000 stock; intercompany sale 310,000 (ERP) | +310,000 stock via intercompany PO (commitment relieved on receipt) | Stock 300,000 (10,000 unrealised margin eliminated while held) |
| After IN-02 issues 5 t to its project | — | Project IC 310,000 | Project cost 300,000 (margin eliminated) |

- Book value is not assumed. Each entity sees its own cost.
- **Conditions for the group column (CR-6):**
  - IN-01 and IN-02 are both consolidated subsidiaries under the approved consolidation policy.
  - The 62,000/t price is the approved commercial price.
  - Intercompany profit on goods still held, or consumed into project cost, is eliminated under that policy.
- **These conditions do not always hold.** If IN-02 is, for example, a JV, an equity-accounted investee or an entity outside the group, or Finance's policy differs, the group column changes. It is not universally applicable (Q1b-P15).
- **Valuation dimensions are separate.** Commercial price, accounting carrying value, GST valuation, transfer-pricing value and any tax relief are separate fields, each set by its own rule (Q1b-P12; statutory-research Part G).
- **Professional validation** is needed for tax, transfer pricing and documentation (RT-04, RT-06).

#### Example 16: Billing vs recognised revenue
Client contract in INR. These inputs are illustrative:
- Contract value: 10,000,000.
- EAC: 8,400,000, including 400,000 of materials on site that are not yet installed.
- Cost incurred: 2,800,000, of which 400,000 is those uninstalled materials.
- Revenue method: cost-to-cost, **assumed** to be approved by Finance (Q13-F1).

| Measure | Amount |
|---|---|
| BM-1 Work performed (internal) | 3,300,000 |
| BM-2 Applied for (cumulative) | 3,500,000 |
| BM-3 Certified by client (cumulative) | 3,200,000 |
| BM-4 Statutory invoices issued (ERP) | 3,200,000 |
| BM-6 Retention held by client (5 %, illustrative) | 160,000 |
| BM-7 Advance recovered by client (10 %, illustrative) | 320,000 |
| BM-8 Receipts | 2,000,000 |
| Pending client variation (not approved) | 500,000, excluded from contract value |

**Revenue under two Finance outcomes (subject to RT-03):**

| Case | Progress | Recognised revenue (BM-5) |
|---|---|---|
| A: conditions for adjusting uninstalled materials **met** | (2,800,000 − 400,000) / (8,400,000 − 400,000) = 30 % | 30 % × (10,000,000 − 400,000) + 400,000 = **3,280,000** |
| B: conditions **not met**, and Finance confirms the materials depict performance | 2,800,000 / 8,400,000 = 33.33 % | **3,333,333** |
| C: AS 7 entity; materials not made specially for the contract | 2,400,000 / 8,400,000 = 28.57 % (future-activity cost left out of costs to date) | **2,857,143**. The 400,000 is held as a future-activity asset. AS 7 has no zero-margin rule. |

**Conditions (CR-6):**
- All three cases assume a single performance obligation, the cost-to-cost method, an EAC that is reliable, and contract value fixed at 10,000,000.
- They also assume the 400,000 of materials is already treated as cost incurred, meaning control of the materials has passed in the way the framework requires.
- If any of these conditions differs, so does the result.

These three cases come from RT-03 (statutory-research Part E). They are illustrations, not policy. Which case applies to an entity depends on its reporting framework (ST-19), the facts recorded on each material line (ST-20) and Finance decision Q13-F1. Under IFRS 15 and Ind AS 115, a material whose control has not passed to the client may be inventory rather than cost incurred (statutory-research E.1). In that situation it would not be in IC at all.

Points this example shows:
- **Billing is not revenue.** BM-2, BM-3 and BM-4 are separate measures, and none of them equals BM-5. Revenue depends on Finance's policy and inputs, not on the client certificate (CB-04).
- **Commercial over/under billing** (Case A) = 3,280,000 − 3,200,000 = **80,000 under-billed**. This is a management indicator only. Contract asset, receivable and liability classification is done by the ERP and Finance (CB-10).
- **No double counting.** The 400,000 of uninstalled materials is in IC once. In Case A it earns revenue equal to its cost (zero margin), and it is excluded from the progress ratio.
- **The pending variation (500,000)** stays out of contract value until it is approved or Finance includes it under its variable-consideration policy.
- **Forecast margin** = 10,000,000 − 8,400,000 = 1,600,000. No loss indicator.

---

## 4. Workflows

### 4.1 Materials
- **MW-01 [OP]** Requisition → PO (commitment) → receipt (direct-to-work or to a store) → transfers → issue → returns → invoice matching (Q6, Q6b).
- **MW-02 [OP]** Materials are tracked in three places, linked by bridges BR-1 to BR-3 (§3.5) (Q6-P9, CR-1):
  - the commitment reconciliation (§3.2), for what was ordered and delivered;
  - the inventory reconciliation (§3.3), for what is in stock;
  - the incurred-cost reconciliation (§3.4), for what became project cost.
- **MW-03 [OP]** A receipt relieves REM even when the material goes into inventory (Q6b-P7).
- **MW-04 [OP]** Matching is PO → receipt → invoice. Tolerances are **[TBP]**. Until they are approved, every discrepancy is routed for review (Q6b-P10, Q14-P10).
- **MW-05 [OP]** Returns, credits and cancellations follow rules A-2 to A-4 and Examples 5 and 9 (Q6b-P11).
- **MW-06 [OP]** Central stores, site stores and controlled transfers. Material master with units of measure and approved conversions (Q12-P1, Q12-P2).
- **MW-07 [OP]** Receiving flow: receipt → inspection → accepted, rejected or quarantined quantities.
  - Only accepted quantity relieves the commitment and enters stock or cost.
  - Rejected quantity becomes a vendor return.
  - Quarantined stock is not issuable.
  - Basis: Q12-P3.
- **MW-08 [OP]** Inventory or expense is decided by the Finance-approved policy (Q6-F1), never by physical location alone (Q12-P5).
- **MW-09 [OP]** Transfers within the same legal entity:
  - Inventory value is preserved under the approved valuation method (Q12-F1).
  - Goods in transit are tracked between dispatch and receipt.
  - No project cost is created by the movement (Q12-P6).
  - Where a tax document is legally required, its tax value is a separate field. It never changes the management inventory value. This applies, for example, to transfers between two Indian GST registrations of the same company; see RT-04.
- **MW-10 [OP]** Transfers between legal entities follow the intercompany rules in §4.7 (Q12-P7, Q1b-P10…P16).
  - Book value is not assumed.
  - The receiving entity's cost is what it was charged under the approved pricing.
  - Group-level elimination happens only under the approved consolidation policy and under the conditions stated in Example 15b.
- **MW-11 [OP]** Consumption tracking, per material and work package:
  - **Theoretical consumption:** quantity of work × approved norm.
  - **Actual consumption:** issues minus returns from the work package.
  - **Allowed wastage:** norm % × theoretical.
  - **Excess wastage:** actual − theoretical − allowed, if positive.

  Wastage norms are effective-dated by material and activity, with values **[TBP]** (Q12-P9, Q12-O1).
- **MW-12 [OP]** Wastage is an *analysis* of actual consumption, never an extra charge on top of it (Q12-P10). Excess wastage, losses and adjustments need a reason and approval. Their accounting classification and effect on revenue progress are **[FIN]** (Q12-P11, Q12-F2).
- **MW-13 [OP]** Physical counts:
  - Monthly as the baseline, with risk-based cycle counts.
  - Controlled inventory cut-off at period end.
  - Count variances are recorded and approved.
  - Basis: Q12-P12.
- **MW-14 [OP]** Usable leftovers, scrap and recoveries are kept separate. Scrap sale proceeds are not a reduction of consumption unless Finance approves that treatment (Q12-P13).

### 4.2 Manpower
- **LW-01 [OP]** Approved timesheets per work package and cost code, at effective-dated standard rates (Q7-P2).
- **LW-02 [OP] [FIN] [RT]** Cost components are defined once, with no overlapping overtime or burden (Q7-P3, Q7-F1).
- **LW-03 [OP]** Monthly true-up as in Example 8 (Q7-P4…P6).
- **LW-04 [OP]** Idle, leave and unallocated hours stay visible (Q7-P5).
- **LW-05 [OP]** Individual pay is visible only to authorised HR and finance roles. Project roles see aggregated cost (Q7-P8, Q14-P12).
- **LW-06 [OP] [FIN]** Labour-contractor cost is classified under the agreed policy (Q7-P7).

### 4.3 Subcontractors
- **SW-01 [OP] [VD]** The PMS originates subcontracts and their amendments. Each has one owner (Q6b-P1, Q6b-P2; VD-19).
- **SW-02 [OP]** Management actual is the certified amount. REM is relieved on certification (Q6-P4, Q6b-P8).
- **SW-03 [OP]** Matching runs contract → certificate → invoice (Q6b-P10).
- **SW-04 [OP]** Measurement method is set per subcontract (Q10-P1):
  - **BoQ re-measurement:** quantity × rate.
  - **% complete against a schedule of values:** % × line value.
  - **Milestones:** milestone value when achieved.
  - **Approved labour/daywork records:** hours × rate, from approved daywork sheets only.
- **SW-05 [OP]** Certificates are **cumulative**. Current-period amount = cumulative certified − previously certified cumulative (Q10-P2).
  - Duplicate controls:
    - one effective certificate per subcontract per period;
    - certificate numbers in sequence;
    - a resubmitted certificate supersedes its earlier version, never adds to it, and every version is kept in history with status, approver and reason;
    - the cumulative quantity on any line cannot exceed its revised commitment quantity without an approved variation.
- **SC-1 [OP] Cost basis of a certificate.** Management cost = certified value of work performed (SA-1 + SA-2).
  - Advances, recoverable tax, withholding, deposits, and any amount already certified under another line (for example a variation also included in original scope) are excluded.
  - If a certificate's gross contains any of these, they are separated out before cost is recognised (CR-6).
- **SC-2 [OP] Posted certificates.** Once a certificate has been posted to the ERP, it is changed only by an approved adjustment certificate, or by a reversal and re-issue, linked to the original. Silent replacement is never allowed (CR-6, PE-07).
- **SW-06 [OP] Separate amounts on every certificate**, each shown for the current period and cumulatively (Q10-P3):

| Code | Amount | Effect on management cost |
|---|---|---|
| SA-1 | Gross work certified, original scope | Cost (incurred-cost reconciliation) |
| SA-2 | Gross work certified, approved variations | Cost |
| SA-3 | Retention held | None. It is a liability. |
| SA-4 | Retention released | None |
| SA-5 | Advance paid | None. It is an advance asset (Q10-P5). |
| SA-6 | Advance recovered | None. It reduces the advance asset. |
| SA-7 | Back-charges deducted (accepted only) | Per Finance decision Q10-F1. Never on the subcontract's gross cost. |
| SA-8 | Taxes and withholding | None for recoverable tax. Withholding is a payment deduction. **[RT]** |
| SA-9 | Net payable | SA-1 + SA-2 − SA-3 + SA-4 − SA-6 − SA-7 ± SA-8 tax lines. Cash only. |

- **SW-07 [OP] Certificate dates.** Each is a separate field (Q10-P8, Q15a-P3, RT-01):

| Code | Date | Meaning |
|---|---|---|
| CD-1 | Work period start | First day of the work covered by the certificate |
| CD-2 | Work period end | Last day of the work covered. This sets the **management reporting period** of the cost. |
| CD-3 | Submission date | Date the subcontractor's application was received |
| CD-4 | Certification date | Date we approved the assessed amount. This creates the management actual (Q6-P4). |
| CD-5 | Supplier invoice date | Date on the subcontractor's tax invoice |
| CD-6 | Accounting posting date / period | Period in which the ERP posts the liability. Owned by the ERP. |
| CD-7 | Tax point date | Determined by the ERP or tax service under country rules **[RT]**. Never assumed equal to CD-4. |
| CD-8 | Payment due date / payment date | From contractual payment terms / actual payment |

- **SW-08 [OP]** Retention:
  - percentage, cap and release conditions are effective-dated per subcontract;
  - releases are staged, for example at completion and at the end of the defects period;
  - defects-liability end dates are tracked.

  Values are **[TBP]** (Q10-P7, Q10-O1).
- **SW-09 [OP]** Advances:
  - guarantee reference, amount and expiry, with alerts before expiry;
  - a recovery schedule (% of each certificate or fixed instalments);
  - the outstanding advance balance is never project cost (Q10-P5, Q10-P7).
- **SW-10 [OP]** Back-charges have their own lifecycle: Proposed → Notified → Disputed / Accepted / Withdrawn → Deducted or otherwise settled. Each records its basis, evidence, approval, disputed or accepted status, accounting treatment and source-cost links (Q10-P6). Rules:
  - A back-charge affects any measure only once it is **Accepted**.
  - Its accounting treatment is a **Finance decision (Q10-F1)**, and more than one treatment may be allowed. Options include:
    - reduction of the source cost line(s);
    - reduction of the subcontract's certified value where the work was not performed;
    - other income / recovery;
    - an amount recovered from a different party.
  - Recovery is **not** universally limited to reducing a single source cost (CR-6).
  - Whatever treatment is chosen, the same economic amount can reduce cost, or be recognised as recovery, only once.
  - Total recoveries cannot exceed the amount approved in the back-charge.
  - A back-charge can be settled only once.
- **SW-11 [OP]** Compliance documents (insurance, licences, guarantees, and country-specific evidence **[RT]**):
  - each document has an expiry date;
  - a missing or expired mandatory document creates a **controlled payment hold** with an override approval;
  - the mandatory list per contract and country is **[TBP]** (Q10-P7).
- **SW-12 [OP]** Purchased labour (labour contractors) uses this subcontract structure under its own class. It is never also counted as employee payroll (Q10-P7, Q7-P7).

### 4.4 Equipment
- **EW-01 [OP]** Equipment register covering owned, leased and externally hired equipment (Q11-P1).
  - **PMS owns:** operational data (allocation, logs, rates).
  - **ERP owns:** fixed assets, depreciation and lease accounting.
  - This split is subject to integration verification (Q11-P9).
- **EW-02 [OP]** Rate cards by entity and equipment class, effective-dated. A rate can be hourly, daily, weekly or monthly. Each rate states explicitly whether **operator** and **fuel** are *included* or *separate*. Values are **[TBP]** (Q11-P2, Q11-P6, Q11-O1).
- **EW-03 [OP]** Daily logs record time as working, idle, breakdown, maintenance or transfer. Charging rules (minimum hire, standby, mobilisation, demobilisation, part periods, idle treatment) are configurable and **[TBP]** (Q11-P3, Q11-P4).
- **EW-04 [OP]** External hire:
  - the hire agreement is a commitment, split into bare hire or hire with operator;
  - cost is recognised on approved service-period confirmation (Q6-P7, Q11-P4).
- **EW-05 [OP]** Owned equipment uses an **equipment cost pool** per item or class. The pool collects depreciation (from the ERP), maintenance, insurance, and, where the rate includes them, operator payroll and fuel. Internal charges credit the pool and debit the work package (Q11-P6, Q11-P7).
- **EW-06 [OP]** No double charging:
  - Where the rate includes operator or fuel, operator time and fuel issues go to the pool, never to the receiving work package.
  - The system rejects or flags an operator timesheet or fuel issue to a work package for equipment whose rate includes them (Q11-P6).
- **EW-07 [OP]** Recovery reconciliation per period: internal charges − pool actual cost = over- or under-recovery. This is reported, not hidden (Q11-P7).
  - Whether **project management cost** for owned equipment is the internal charge or the pool's actual cost is **[FIN]** (Q11-F1, X-23).
  - Market-rate internal charges are not treated as accounting actual cost (Q11-P8).
- **EW-08 [OP]** In consolidated and entity reports, internal charges are eliminated against pool recoveries, so entity cost = pool actual cost (Q11-P7).
- **EW-09 [OP]** Cross-entity equipment use goes through the intercompany process (Q11-P10, Q1b-O2), not an internal charge.
- **EW-10 [OP]** Reports: usage, availability, utilisation (working ÷ available hours) and cost recovery (Q11-P5).

### 4.5 Non-PO costs and integration
- **IW-01 [OP]** Non-PO expenses and journals go through a controlled workflow (Q6b-P5).
- **IW-02 [OP]** Exceptions are always visible (Q6b-P6).
- **IW-03 [OP]** Integration uses stable IDs and versions, prevents duplicates, retries safely and reconciles (Q6b-P12).
- **IW-04 [OP] [RT] [VD]** Tax is determined by the ERP or a tax service. The integration pattern is open (Q6b-P3, X-10).

### 4.6 Client billing and revenue inputs
- **CB-01 [OP]** Billing method is set per contract (Q13-P1):
  - BoQ re-measurement / interim valuation
  - milestones
  - cost-plus
  - lump sum with schedule of values
  - mixed, where each part of the contract is allocated to one method.
- **CB-02 [OP]** Each of these measures is kept separately, current period and cumulative (Q13-P2, Q13-P3):

| Code | Measure | Owner |
|---|---|---|
| BM-1 | Work performed (internal measurement) | PMS |
| BM-2 | Amount applied for (our application to the client) | PMS |
| BM-3 | Amount assessed / certified by the client | PMS (recorded from the client certificate) |
| BM-4 | Statutory invoices and credit notes (IDs, status, amounts) | ERP / invoicing service, returned to PMS (Q13-P5) |
| BM-5 | Recognised revenue | ERP posts it, using PMS inputs (Q13-P8) |
| BM-6 | Retention held by client / released | PMS commercial and ERP accounting |
| BM-7 | Advances received / recovered by client | PMS commercial and ERP accounting |
| BM-8 | Receipts and outstanding balance | ERP, shown in PMS |

- **CB-03 [OP]** Client BoQ quantities are kept separate from internal cost quantities (Q5-P7, Q13-P3).
- **CB-04 [OP]** Approved and pending variations are tracked separately. Neither a client certificate (BM-3) nor an invoice (BM-4) automatically counts as recognised revenue (BM-5) (Q13-P4).
- **CB-05 [OP]** The PMS prepares and approves commercial billing data. The ERP or a compliant e-invoicing service issues the statutory invoice and returns its ID, status and references. The PMS never issues statutory invoices (Q13-P5; research §10 candidate ST-09).
- **CB-06 [OP] [FIN]** The revenue method and policy are approved by Finance for each entity and contract. They are never derived from the billing method or the country alone (Q13-P6, Q13-F1).
- **CB-07 [OP]** Validated revenue inputs the PMS provides to the ERP (Q13-P7):
  - eligible costs incurred
  - estimate to complete (ETC) and estimate at completion (EAC)
  - measured progress
  - approved contract value, with uncertain changes listed separately
  - **uninstalled materials and other potential progress adjustments**, each with the attributes Finance needs to apply its policy (RT-03)
  - forecast margin and potential-loss indicator
- **CB-08 [OP] Correction (Q13-C1).** Uninstalled materials are **not** automatically excluded from cost-to-cost progress. The applicable standard sets conditions, and Finance decides the treatment per contract. The PMS records the facts the decision depends on; RT-03 lists which.
- **CB-09 [OP] [RT]** The applicable reporting framework is recorded per legal entity (e.g., IFRS, Ind AS, AS or a non-company framework). No single standard is assumed for all Indian entities (Q13-R1; RT-03).
- **CB-10 [OP]** Contract assets, receivables, contract liabilities and *commercial* over/under billing are separate measures (Q13-P9):

| Measure | Plain meaning | Who determines |
|---|---|---|
| Commercial over/under billing | Recognised revenue (BM-5) − amount certified by the client (BM-3). A management indicator only. | PMS report |
| Receivable | An amount the client must pay, where only the passage of time remains | ERP / Finance (accounting standard) |
| Contract asset | A right to consideration for work done that is conditional on something other than time | ERP / Finance |
| Contract liability | Consideration received or due before the work is performed | ERP / Finance |

  The PMS supplies the inputs and does not classify these.
- **CB-11 [TBP]** Retention, advance terms and billing cycles per contract (Q13-O1).

### 4.7 Intercompany (materials, equipment, staff)
- **IC-01 [OP]** Any use by one entity of another entity's materials, equipment or staff needs a controlled **intercompany agreement** (Q1b-P10). It records:
  - supplying and receiving entities
  - projects and allocations
  - service or transfer period
  - pricing method and currency
  - tax and invoicing treatment
  - approvals
  - linked documents and settlement terms
- **IC-02 [OP] [FIN]** Pricing method is approved by finance/tax for each agreement type. The PMS applies no universal cost-plus, market or book-value rule (Q1b-P11). Policies and approvers are **[TBP]** (Q1b-O4).
- **IC-03 [OP]** Each intercompany line keeps these valuation fields separately, each set by its own approved rule (Q1b-P12, RT-06):
  - commercial price
  - supplying entity's accounting carrying value
  - indirect-tax (GST/VAT) value
  - transfer-pricing value and method
  - corporate-tax relief or election flag
  - customs value (cross-border only)
- **IC-04 [OP]** Purchase commitments are **not** created automatically for every allocation. The required documents (commitment, intercompany billing, journal) are defined per transaction type and ERP capability **[VD]** (Q1b-P13, VD-32).
- **IC-05 [OP]** Both entities' records are reconciled per agreement and period. Timing and FX differences are identified, not netted (Q1b-P14).
- **IC-06 [OP]** Entity-level results are kept separate from consolidation adjustments. Intercompany balances, transactions and unrealised profit are eliminated only where the approved consolidation policy requires it (Q1b-P15).
- **IC-07 [OP]** A cross-border movement cannot be completed until its customs and regulatory review is recorded as complete. The domestic workflow is not sufficient (Q1b-P16, ST-18).
- **IC-08 [OP]** Staff secondments use the agreement's pricing. They never put employee pay details into the receiving entity's project. Only the authorised charge is visible (Q16-P4, LW-05).
- **IC-09 [OP]** Cross-entity equipment use (EW-09) follows IC-01 to IC-07.

---

## 5. Approval rules, roles and audit

### 5.1 Roles (Q14-P2) [OP]
- Requester / site engineer
- Storekeeper / material controller
- Project engineer / discipline lead
- Quantity surveyor / cost controller
- Procurement manager
- Project manager
- Project director
- Finance controller
- CFO / authorised executive
- HR / payroll administrator
- System administrator. This role has **no automatic financial approval rights**.

### 5.2 Approval rules
- **AP-01 [OP]** Approval tiers can be configured by entity, project, document type and value. **All amounts are [TBP]** (Q14-P1, Q14-O1).
- **AP-02 [OP]** Thresholds are held in the entity's functional currency. The FX conversion rule for foreign-currency documents is **[FIN]** (Q14-P3, Q14-O3).
- **AP-03 [OP]** No one may approve their own document (Q14-P4).
- **AP-04 [OP]** Incompatible duties are defined per document and risk in a matrix that is **[TBP]**. Staffing exceptions need documented compensating review and authorisation (Q14-P5, Q14-O5).
- **AP-05 [OP]** Purchase splitting is detected and prevented. The detection rule is **[OPEN]** (Q14-P6, Q14-O4).
- **AP-06 [OP]** A material change to value, scope, vendor, dimensions or terms triggers re-approval. Whether amendments are assessed on cumulative value, change amount or both is **[OPEN]** (Q14-P7, Q14-O4).
- **AP-07 [OP]** A commitment that would exceed the available RB on its line needs explicit approval of the overrun, or a prior budget change (Q14-P8).
- **AP-08 [OP]** Percentage and absolute matching limits apply together under a documented rule. Values are **[TBP]**. Until they are approved, discrepancies go to review (Q14-P9, Q14-P10).
- **AP-09 [OP]** Delegation is time-limited and logged, and cannot breach segregation of duties (Q14-P11).
- **AP-10 [OP]** Budget documents follow these rules:
  - baseline approval
  - corrections (BV-1)
  - transfers, with higher approval across subprojects (BV-4)
  - contingency releases (RS-4, RS-5)
  - the four change statuses (CM-2)

### 5.3 Audit
- **AU-01 [OP]** A full history of approvals, overrides, reversals, closures and corrections, with reasons (Q14-P13, Q6b-P11).
- **AU-02 [OP]** Rejected and superseded change records are kept (Q9-P11).
- **AU-03 [RT]** Statutory audit-trail obligations of the ERP or the PMS (see §10).

### 5.4 Period close (Q15a) [OP]
- **PE-01** Monthly immutable snapshots on each entity's finance calendar. Calendars are **[TBP]** (Q15a-P1, Q15a-P2, Q15a-O1).
- **PE-02** Each record keeps four dates: document date, service/receipt date, accounting posting period and PMS reporting period (Q15a-P3).
- **PE-03** A snapshot freezes budgets, commitments, inventory, IC, UA, forecasts, FX assumptions and reconciliation status. It records the version, preparation time, approvers and source cut-off (Q15a-P4, Q15a-P5).
- **PE-04** Late items go to the next open period with a reference to the prior period. ERP dates and periods are never changed (Q15a-P6, Q15a-P7).
- **PE-05** An ERP posting into a closed accounting period is flagged and reconciled (Q15a-P8).
- **PE-06** Reopening a period needs Finance approval and creates a new snapshot version. The original is kept (Q15a-P9).
- **PE-07** An adjustment is distinguished from a replacement of an accrual (rule C-2) (Q15a-P10).
- **PE-08** Each entity closes separately. The consolidated cut-off lists any entities still open (Q15a-P11).

---

## 6. Budget-versus-actual report requirements
- **RP-01 [OP]** Columns per work package × cost code × cost type, rolled up the hierarchy:
  - OB, ADJ, ACH, NTR, RB
  - RCV, REM
  - INV (earmarked)
  - PC, UA, IC
  - ETC, CTC, EAC
  - PCE, PPC, EACp
  - VAC

  At project level, the report also shows RSV and VACp (Q8a-P9, Q8-P6, Q9-P12).
- **RP-02 [OP]** PV, FXV, IVD and LV appear as "of which" columns (Q6b-P9, Q6b-P13, Q7-P6, CR-1).
- **RP-03 [OP]** Commitment, inventory and incurred-cost reconciliation reports (§3.2–3.4), each with drill-down to source lines (CR-1).
- **RP-11 [OP]** Subcontract ledger per subcontract: amounts SA-1 to SA-9, current and cumulative; retention and advance balances; guarantee expiries; compliance holds; back-charge register (§4.3).
- **RP-12 [OP]** Equipment usage, utilisation and cost-recovery report, with the over/under-recovery of each pool (§4.4).
- **RP-13 [OP]** Material reconciliation by work package: theoretical, actual, allowed wastage, excess wastage, leftovers and scrap (§4.1).
- **RP-14 [OP]** Billing vs revenue report: BM-1 to BM-8, approved and pending variations, commercial over/under billing (§4.6).
- **RP-04 [OP]** A reserve report shows original, additions, releases, returns, remaining and forecast need. Contractor contingency is hidden from client-facing outputs by default (Q8-P6, Q8-P7).
- **RP-05 [OP]** A change register shows class, the four statuses, amounts by stage, links to cost lines and subcontracts, and margin and unrecovered exposure (Q9).
- **RP-06 [OP]** Transaction, functional and (once approved) group currency views (Q1a-P4, Q1a-P5 **[FIN]**).
- **RP-07 [OP]** Monthly reconciliation of PMS PC against the ERP's eligible ledger, with exceptions (Q6a-P8).
- **RP-08 [OP]** Snapshot comparison between periods, with prior-period adjustments listed separately (Q15a).
- **RP-09 [OP]** Idle and unallocated labour, and posted cost that has no allocation, each get their own rows (Q7-P5, Q6b-P6).
- **RP-10 [OP]** Cadence (Q15-P1…P3):
  - monthly cost report and immutable snapshot per entity, following PE-01 to PE-08;
  - monthly ETC review for each work package, signed off by the PM and the QS / cost controller;
  - weekly flash report covering commitments, incurred cost, pending changes, forecast risks and exceptions.

  Deadlines and calendars are **[TBP]** (Q15-O1).
- **RP-15 [OP]** Operational dashboards show when each data source was last successfully updated, and flag integration delays beyond a configurable threshold. The threshold is **[TBP]** (Q15-P4).
- **RP-16 [OP]** Required report set (Q15-P5):
  - original and revised budget vs incurred
  - remaining commitments, inventory held, ETC, EAC, forecast variance
  - materials consumption and wastage
  - labour hours, productivity and cost variances
  - equipment utilisation and recovery
  - subcontract certification, retention, advances, outstanding liabilities
  - change register and contingency movements
  - client billing, collections, recognised revenue and forecast margin, **as distinct measures**
  - PMS-to-ERP reconciliation and unresolved exceptions
  - summaries at project, subproject, accounting entity, country and group level
  - at-risk exposure (AR-02)
- **RP-17 [OP]** Formats: dashboard, Excel and PDF. Every export carries its filters, currency basis, reporting period and snapshot version, and keeps the user's access restrictions. A user cannot export data they cannot view (Q15-P6).
- **RP-18 [OP]** Client extracts exclude internal contingency, individual pay, internal margins and other confidential fields unless explicitly authorised. Each authorisation is logged (Q15-P7).
- **RP-19 [OP]** Group and country summaries in a group reporting currency are available only once the translation rules have Finance approval (Q1a-P5 **[FIN]**). Until then, summaries are shown per functional currency, with no translated totals.

---

## 7. Acceptance criteria (testable)
"Pass" means every stated value matches exactly, using the data from the example named.

| ID | Criterion | Example | Traces to |
|---|---|---|---|
| AC-01 | After a direct-to-work receipt: IC 90,000, UA 90,000, PC 0, REM 0. After the invoice: IC 90,000, UA 0, PC 90,000. | Ex 1 | RR-02, C-1 |
| AC-02 | Receipt into a store adds 40,000 to INV and 0 to IC. An invoice posted to inventory never increases PC. Issuing 60 bags gives IC 24,050 and INV 24,050. | Ex 2 | RR-01, RR-03, Q6a-P3 |
| AC-03 | REC-A holds at every step: RCV = AOV + AMD − CAN = FUL + REM. | Ex 1, 3, 5, 7, 9 | §3.2 |
| AC-04 | REC-B holds: opening + receipts ± transfers − issues − returns ± valuation adjustments − write-offs = closing INV. The IVD of 50 is shown and is not attributed to PO-2. | Ex 2 | §3.3, BR-3 |
| AC-05 | REC-C holds: IC = PC + UA. PMS PC equals the eligible ERP ledger plus or minus the listed exceptions. | All | §3.4 |
| AC-06 | One invoice clears two receipts, matched by quantity. Final IC 90,600, of which PV 600. | Ex 3 | Q6a-P5, Q6b-P4 |
| AC-07 | A line-level ERP receipt accrual sets UA to 0. IC never exceeds 54,000. | Ex 4 | C-1 |
| AC-08 | Return without replacement: CAN 18,000, REM 0, final IC 72,000. Return with replacement: REM 18,000 after the return, final IC 90,000. IC is never reduced twice. | Ex 5a/5b | A-2, A-3 |
| AC-09 | Closing a PO changes only CAN and REM. FUL, PC, UA and the liability are unchanged. | Ex 9 | A-4 |
| AC-10 | A posted cost with missing dimensions is in IC at project level and in exceptions. | Ex 6 | C-3 |
| AC-11 | Posted actual 184,000 is shown in full, with FXV 500 shown separately. FUL + FXV = IC. | Ex 7 | Q6b-P13 |
| AC-12 | After the true-up, B.03 = 45,000 (not 44,900, and not their sum). LV 100. Idle 2,000 is separate. | Ex 8 | Q7 |
| AC-13 | EAC never includes REM twice. Approving a commitment moves its amount from ETC to REM and EAC is unchanged. | Ex 10 step 4 | F-1 |
| AC-14 | A pending change adds only its cost that is not already in ETC to PPC. Approval leaves EACp unchanged and RB rises only when the budget posting status is set. | Ex 10 steps 1, 3 | F-2, F-3, CM-2 |
| AC-15 | A contingency release keeps RB + RSV constant. Cost postings or commitments against a reserve are rejected. | Ex 10 step 2 | RS-3, RS-6 |
| AC-16 | A transfer that does not net to zero, or that crosses legal entities or unrelated projects, is rejected. A transfer between subprojects requires the configured higher approval. | — | BV-3, BV-4 |
| AC-17 | Client approval alone does not change internal authorisation, subcontract approval or budget posting status. | Ex 10 | CM-2 |
| AC-18 | A document approved by its own requester is rejected. A delegation that would breach a configured incompatible duty is rejected. | — | AP-03, AP-09 |
| AC-19 | While tolerances are unapproved, any price or quantity discrepancy creates a review task and is not auto-accepted. | — | AP-08 |
| AC-20 | A closed snapshot reproduces identical figures when re-run. A late invoice that replaces an accrual creates no additional cost in the next period. Reopening creates version n+1 and keeps version n. | Ex 4 + late posting | PE-03, PE-06, PE-07 |
| AC-21 | Resending an integration message with the same ID and version leaves all measures unchanged. | Any | IW-03 |
| AC-22 | A split that does not total 100 % is rejected. | Any | DM-03 |
| AC-23 | A monetary record missing currency, rate, rate date, rate source or functional amount is rejected. | Ex 7 | DM-01 |
| AC-24 | Users without the pay-data role cannot see individual rates or pay, but can see aggregated labour cost. | Ex 8 | LW-05 |
| AC-25 | Cost cannot be posted to a non-posting level. | — | HR-04 |
| AC-26 | Subcontract certificates are cumulative. Current amounts equal cumulative minus previous. Values match the Example 11 table at every certificate, and cumulative net payable equals 1,800,000. A certificate whose gross contains an advance or tax line recognises cost only on SA-1 + SA-2. | Ex 11 | SW-05, SW-06, SC-1 |
| AC-27 | Retention and advance recovery never change IC. IC equals cumulative gross certified. Once the cap is reached, retention stops accruing. | Ex 11, 12 | SW-06, SW-08 |
| AC-28 | A resubmitted unposted certificate supersedes the earlier version: current 470,000, and version 1 is kept in history. For a posted certificate, a silent replacement is rejected, and only an approved adjustment of 20,000 or a reversal and re-issue is accepted. Cumulative quantity above RCV is rejected without an approved variation. | Ex 11 | SW-05, SC-2 |
| AC-29 | Releasing retention leaves IC unchanged and creates a payable only. | Ex 12 | SW-06 |
| AC-30 | A Proposed or Disputed back-charge has no effect. An Accepted one is applied exactly once, under the configured Finance treatment: G.01 8,000 → 3,000 under A; G.01 unchanged plus a 5,000 recovery under B; certified value reduced under C. Settling the same back-charge a second time is rejected, and recoveries never exceed the accepted amount. | Ex 13 | SW-10 |
| AC-31 | For equipment whose rate includes operator and fuel, an operator timesheet or fuel issue booked to a work package is rejected or flagged. The pool recovery report shows charges 450,000, pool actual 430,000 and over-recovery 20,000. After elimination, entity cost equals 430,000. | Ex 14 | EW-05…EW-08 |
| AC-32 | A transfer within the same entity changes store balances and goods in transit only. Entity stock is unchanged until issue, and project IC is 0 until issue. | Ex 15a | MW-09 |
| AC-33 | A cross-entity transfer uses an intercompany document and price. The group view eliminates intercompany margin (300,000 group cost vs 310,000 in IN-02). | Ex 15b | MW-10 |
| AC-34 | The billing vs revenue report shows BM-1 to BM-8 separately. Neither the certified amount nor the invoiced amount is used as recognised revenue. The uninstalled-material adjustment applies only when Finance has recorded that its conditions are met (Case A vs Case B). | Ex 16 | CB-02, CB-04, CB-08 |
| AC-35 | Wastage reporting never increases consumption cost: actual consumption = issues − returns, and allowed and excess wastage are analysis of that figure. | §4.1 | MW-11, MW-12 |
| AC-36 | Only accepted receipt quantity relieves commitment and enters stock or cost. Rejected quantity creates a vendor return. Quarantined stock cannot be issued. | §4.1 | MW-07 |
| AC-37 | RB, RCV, REM, INV, IC, ETC and EAC at subproject, project and programme level equal the sum of their posting work packages. A reserve is added once, at its holding level. | Ex 10 | HR-06, RS-1 |
| AC-38 | After a cost-code library change, closed-period reports reproduce their original figures. An entity extension cannot change a group code. A BoQ split that totals other than 100 % is rejected. | — | CC-02, CC-04, CC-05 |
| AC-39 | A PMS-originated allocation that is missing any mandatory dimension (DM-01) is rejected. An ERP posting with missing dimensions is accepted into exceptions instead (C-3). | Ex 6 | DM-01, C-3 |
| AC-40 | PMS configuration holds no tax rates. Tax codes and amounts on commitments come from the ERP or tax service. A country with no approved configuration cannot create commitments or certificates. | — | IW-04, ST-01, ST-17 |
| AC-41 | The weekly flash and monthly pack contain every measure listed in RP-10 and RP-16. A dashboard shows the last successful update for each source and raises a delay flag when the configured threshold is exceeded. | — | RP-10, RP-15, RP-16 |
| AC-42 | An Excel or PDF export shows the applied filters, currency basis, period and snapshot version. A user cannot export rows or fields they are not authorised to view. | — | RP-17 |
| AC-43 | A client extract omits contractor contingency, individual pay and internal margin unless an authorisation is recorded, and every such authorisation is logged. | — | RP-18 |
| AC-44 | Every project has exactly one accounting entity. Partner shares are effective-dated with an agreement reference. Changing a partner's share % never changes consolidation treatment by itself. | — | HR-02, HR-08, HR-09 |
| AC-45 | A cross-entity allocation without an approved intercompany agreement is rejected. Each intercompany line holds the separate valuation fields (IC-03). No purchase commitment is created unless the transaction type requires one. The two-sided reconciliation lists timing and FX differences. Consolidation adjustments appear separately from entity results. | Ex 15b | IC-01…IC-06 |
| AC-46 | A cross-border movement cannot reach "completed" status until its customs/regulatory review is recorded as complete. | — | IC-07, ST-18 |
| AC-47 | An authorised at-risk cost appears in IC on its work package in the period incurred, and also in at-risk exposure. Rejecting the client recovery does not remove it from IC. An over-budget at-risk commitment requires the AP-07 approval. | — | AR-01…AR-05 |
| AC-48 | Access requires SSO with MFA for all interactive users. Permissions follow RBAC by entity, project and role. Every create, approve, change and reverse action writes an immutable audit record. Retention periods are configurable per country, with values **[TBP]**. | — | NF-04…NF-07 |
| AC-49 | The PMS stores only the approved minimum personal dataset (NF-08). No medical fields exist. Payroll components beyond the authorised cost rate cannot be imported. | Ex 8 | NF-08, NF-09 |
| AC-50 | A complete export of project, commitment, cost, billing and audit data in the documented format can be produced for an entity. | — | NF-12 |
| AC-51 | (Should) A site entry recorded offline syncs exactly once when connectivity returns. A conflicting edit raises a review task instead of overwriting. | — | NF-13 |
| AC-52 | Backup and recovery tests meet the approved RPO/RTO. The criterion is **[TBP]** until targets are provided (Q16-O2). | — | NF-10, NF-11 |

### 7.1 Must-have requirements → acceptance criteria → decisions

| Must-have | Acceptance criteria | Decisions | Research |
|---|---|---|---|
| REQ-001 Hierarchy | AC-15, AC-25, AC-37 | Q4-P1…P9 | research.md §5, §7.1 |
| REQ-003 Cost codes | AC-22, AC-38 | Q5-P1…P7 | §2.2, §7.2 |
| REQ-004 Dimensions | AC-23, AC-39 | Q5a-P1…P4 | §7.2 |
| REQ-005 Splits | AC-22, AC-38 | Q5-P6, Q5a-P3 | — |
| REQ-006 Multi-currency | AC-11, AC-23 | Q1a-P2…P4, Q6b-P13 | §4.6, §4.8 |
| REQ-007 Recognition rules | AC-01, AC-02 | Q6-P1…P9 | §6.3 |
| REQ-008 Reconciliations | AC-03, AC-04, AC-05 | Q6a, Q6b, CR-1 | §6.2–6.3 |
| REQ-009 Relief, matching, reversals | AC-06…AC-09, AC-21 | Q6b-P1…P12 | §6.3 |
| REQ-010 FX analysis | AC-11 | Q6b-P13 | — |
| REQ-011 Labour | AC-12, AC-24 | Q7-P1…P8 | §6.3 (4) |
| REQ-012 Exceptions | AC-10, AC-39 | Q6b-P6 | — |
| REQ-013 Budget versions/transfers | AC-16 | Q8a-P1…P9 | §7.3 |
| REQ-014 Contingency | AC-15, AC-37 | Q8-P1…P9 | §1 finding 5 |
| REQ-015 Change management | AC-13, AC-14, AC-17 | Q9-P1…P12 | §7.3 |
| REQ-016 Approvals/SoD | AC-18, AC-19 | Q14-P1…P13 | §7.10 |
| REQ-017 Snapshots | AC-20 | Q15a-P1…P11 | §7.8 |
| REQ-019 Statutory integrations | AC-40 | Q1-P1, Q1-R1 | statutory-research A–G |
| REQ-020 Subcontracts | AC-26…AC-30 | Q10-P1…P8 | §4.6–4.8 (vendor docs) |
| REQ-023 Materials | AC-02, AC-32, AC-35, AC-36 | Q12-P1…P13 | — |
| REQ-024 Reporting | AC-41 | Q15-P1…P5 | §7.11 |
| REQ-025 Exports/extracts | AC-42, AC-43 | Q15-P6, Q15-P7 | — |
| REQ-026 Accounting entity / JV | AC-44 | Q1b-C2, Q1b-P5…P9 | statutory-research E.3 |
| REQ-027 Intercompany | AC-33, AC-45, AC-46 | Q1b-P10…P16 | statutory-research D, G |
| REQ-028 At-risk | AC-47 | Q9-P13…P16 | — |
| REQ-029 Security/data | AC-48, AC-49 | Q16-P3, Q16-P4 | statutory-research F (RT-05a) |
| REQ-030 Recovery/exit | AC-50, AC-52 | Q16-P3 | RT-05b |

Every Must-have now has at least one acceptance criterion. AC-52 cannot be tested until the RPO/RTO values are provided.

Capacity and performance criteria are **[OPEN]** until Q2 volumes are provided.

---

## 8. Assumptions, vendor dependencies and open decisions

### 8.1 Assumptions in this draft (not business facts)
- Example rates, prices and quantities are illustrative only.
- Moving-average inventory valuation (Example 2) is an assumption pending **[FIN]** approval.
- Recoverable indirect tax is excluded from project cost. The applicable rules are **[RT]**.

### 8.2 Vendor and ERP dependencies
- **VD-18:** line-level posting visibility.
- **VD-19:** commitments originated in the PMS.
- **VD-17:** cross-vendor double-count test.
- **VD-20 to VD-32:** Batch 3–5 scenarios: budget, changes, contingency, approvals, close, subcontracts, equipment, materials, billing, revenue, hosting, JV accounting entity, intercompany (see `vendor-demo-checklist.md`). All are Pending verification.
- **Hosting facts:** see `hosting-research.md`. Only two of the eight ERPs publicly document both India and UAE regions.

### 8.3 Open decisions, contradictions, unsupported claims, vendor checks, missing inputs
- The consolidated, prioritised list is in **`signoff-checklist.md`**.
  - Each item names its responsible role and the requirements it affects.
  - Each item says whether it blocks design, configuration, migration or go-live.
- Source registers:
  - `business-decisions.md`: contradictions X-1 to X-30, Finance list, TO BE PROVIDED list.
  - `vendor-demo-checklist.md`: VD-01 to VD-32.
  - `statutory-research-india-uae.md`: professional validation lists A.7, B.8, D.4, E.5, F, G.3.
  - `existing-pms-inspection.md` §9.

---

## 9. Traceability

| Requirement group | Business decisions | Research evidence |
|---|---|---|
| HR-01…07 | Q1b-P2/P3, Q4-P1…P9 | research.md §5, §7.1 |
| CC-01…06 | Q5-P1…P7 | §2.2, §7.2 |
| DM-01…04 | Q5a-P1…P4 | §7.2 |
| §3.1 measures; F-1…F-4 | Q6a, Q8a-P9, Q8a-Q1, Q9-P4…P6, CR-2 | §2.2 (ETC/EAC/VAC), §6.3 conclusion 5 |
| REC-A/B/C, BR-1…4 | Q6-P9, Q6a-P2…P7, Q6b-P7…P13, CR-1 | §6.2, §6.3 conclusions 1–2 |
| RS-1…8 | Q8-P1…P9 | §1 finding 5 (corrected), §7.3 R-B5 |
| BV-1…6 | Q8a-P1…P9 | §7.3 R-B2–B4 |
| CM-1…7 | Q9-P1…P12 | §7.3 R-B6; CMiC PCI classes (§4.6) |
| AP-01…10, AU-01…03 | Q14-P1…P13 | §7.10 |
| PE-01…08 | Q15a-P1…P11 | §7.8 R-C4 |
| RP-01…10 | Q6a, Q8, Q9, Q15a | §7.11 |
| AC-01…52 | per row; Must-have matrix §7.1 | §6; vendor-demo-checklist |
| SW-04…12, SC-1, SC-2 | Q10-P1…P8, CR-6 | statutory-research A.1, B.1, B.7 |
| EW-01…10 | Q11-P1…P10 | research.md §4.6 (CMiC equipment), §4.8 (IFS rental) |
| MW-06…14 | Q12-P1…P13 | statutory-research D; Ind AS/IFRS (E.1) |
| CB-01…11 | Q13-P1…P9, Q13-C1 | statutory-research B.2, E |
| HR-08, HR-09 | Q1b-C2, Q1b-P5…P9 | statutory-research E.3 |
| IC-01…09 | Q1b-P10…P16, Q1b-C3 | statutory-research D, G |
| AR-01…05 | Q9-P13…P16 | — |
| RP-10…19 | Q15-P1…P7 | research.md §7.11 |
| NF-01…14 | Q16-P1…P5 | statutory-research F; hosting-research.md |
| §12 options | Q17-P1…P3 | research.md §4 pricing |

---

## 10. Statutory research notes: India and UAE (RT-01)
- **Status:** Research complete, dated 2026-11-03. The detail is in `statutory-research-india-uae.md`.
- **Nature of the findings:** they are research only. They are not tax advice and not finance policy.
- **Source quality:** many items rest on secondary sources because the official portals blocked automated access. Those items are marked *Unverified* in the research file.

These candidate requirements (ST-01 … ST-17) follow from the research. All are tagged **[RT]** and are **not approved**:

| ID | Candidate requirement (summary) | Relates to |
|---|---|---|
| ST-01 | Effective-dated tax codes, rates and statutory on-cost tables as master data. No hard-coded rates. | DM-04, LW-02 |
| ST-02 | Party tax identifiers: GSTIN, PAN, Udyam (India); TRN (UAE). Owned by the ERP and referenced by the PMS. | DM-04, IW-04 |
| ST-03 | Certificates and bills keep five separate dates: work done or service period, certification, invoice, payment due, payment. | PE-02, SW-02 |
| ST-04 | Separate fields for gross value, retention held, released and invoiced, and advances paid or received and recovered. | Batch 4 (Q10, Q13) |
| ST-05 | Tax attributes per line: SAC/HSN, reverse charge, credit eligibility, recoverable portion. Recoverable tax is excluded from project cost. | Q6a-P3 |
| ST-06 | Site attributes: state or emirate, GST registration, free-zone or designated-zone status, data regime. | DM-01 |
| ST-07 | Transfers between sites or entities carry transfer documents and a transfer value: India tax invoice or delivery challan, plus e-way bill data. | REC-B, Q1b-O2 |
| ST-08 | Withholding attributes (India TDS table item, lower-deduction certificate). UAE withholding is 0 % (*Unverified*). | SW, vendors |
| ST-09 | The PMS hands structured billing and certificate data to the ERP or e-invoicing provider (India IRN; UAE PINT AE). The PMS never issues statutory e-invoices itself. | Q13, IW-04 |
| ST-10 | Timesheet hour types and limits configurable per country, including the UAE seasonal midday-work window. | LW-01 |
| ST-11 | Labour cost-rate components configurable per country and effective-dated. | Q7-F1 |
| ST-12 | Project statutory registrations and levies, for example the India BOCW cess line. | §3.1 budget lines |
| ST-13 | Subcontractor compliance records per country. | Batch 4 (Q10) |
| ST-14 | Immutable audit trail. Retention period configurable per country, to be unified by professional validation (India at least 8 years; UAE up to 15 years for real-estate records). | AU-01…03 |
| ST-15 | Revenue-recognition data: cost to date, ETC, uninstalled-material flag, approved vs pending variations, onerous-contract flag. Reporting basis set per entity. | Q13, §3.1 |
| ST-16 | Personal-data controls, no health data held in the PMS, hosting region configurable per entity. | LW-05, Q16 |
| ST-17 | Each new country needs its own statutory research. It never inherits India or UAE rules. | Q1-R1 |
| ST-19 | Per legal entity: reporting framework (IFRS, IFRS for SMEs, Ind AS, AS company, or AS non-company Level I–IV), effective date, voluntary/irreversible lock, yearly SMC or level status. | CB-09; RT-03 (statutory-research Part E) |
| ST-20 | Per material line: uninstalled flag; delivery, install and control-transfer dates; the four IFRS/Ind AS B19(b) condition flags with assessment date and approver; AS 7 "made specially for the contract" flag; resulting Finance-approved status. | CB-07, CB-08, Example 16 |
| ST-21 | Per contract: progress method; "progress not reasonably measurable" flag; liquidated-damages exposure; loss flag; cost lines tagged as waste, rework or inefficiency. | CB-07, MW-12 |
| ST-22 | For each intercompany line, these are held as separate fields:<br>• commercial price<br>• seller carrying amount and buyer cost<br>• unrealised profit for elimination<br>• indirect-tax value and the valuation rule used<br>• transfer-pricing value and method<br>• CT relief type and election, with clawback date<br>• customs value<br>• whether the line is a sale or a hire<br><br>Basis: RT-06 (statutory-research Part G). Book value matters for CT only under a valid UAE Art. 26/27 election. | IC-03, Example 15b |
| ST-18 | Every transfer record holds:<br>• transfer type<br>• tax IDs and the state, emirate or zone of both ends<br>• HSN<br>• a **value basis kept separate from book value**<br>• statutory document numbers (challan or tax invoice, IRN, e-way bill, shipping bill or bill of entry, customs declaration)<br>• equipment customs status<br>• linked project<br>• return or re-export deadline<br>• any redeployment restriction | MW-09, MW-10, EW-09; RT-04 (statutory-research Part D) |

## 11. Existing PMS inspection (RT-02)
- **Status: PARTIALLY BLOCKED.** The full inspection is **not** complete. See `existing-pms-inspection.md` §9 for what was inspected, what was verified, what remains unknown, and which requirements depend on the missing inputs.
- **Detail:** `existing-pms-inspection.md`.
- **What exists:**
  - Project → Milestone → (Tasklist, apparently retired) → Task → Subtask, plus Issues at any level.
  - Fixed foreign keys between levels.
  - One free-entry Budget/Actual pair per row. The Angular workspace saves these as NULL.
  - Weighted progress roll-up, timers recording hours only, dependencies, Gantt, QC workflow, critical labels, notifications, dashboards.
  - A read-only link from a project to a sales order.
- **What was not found:** any of the cost-control areas in this document. That covers cost codes, posting work packages, commitments, inventory, subcontract claims, labour rates, equipment costing, currency or legal entity per project, accruals, period close and ERP posting.
- **Inputs needed to complete RT-02:**
  - Database schema (DDL)
  - Stored-procedure definitions
  - `CommonClass` source
  - Confirmation of which page versions are live
- **Consequences for this document:**
  - HR-07 and Q4-O1 stay **[OPEN]**. No migration mapping is assumed.
  - The feasibility of extending the existing PMS (Q17-P1) cannot be concluded (X-30).

---

## 12. Phased scope options (planning only, Q17)
This section is **not** an implementation plan and **not** an ERP selection. Budget, deadline, team size and partner tolerance are **[TBP]** (Q17-O1). No estimates of cost, effort or duration are given here, because there is no basis for them yet.

### 12.1 Options and dependencies
| Option | Phase content (in order) | Key dependencies | Main trade-off |
|---|---|---|---|
| **A. Cost-control core first** | **P1:** hierarchy, cost codes, budgets/changes/contingency, commitments, incurred-cost reconciliation, monthly reporting. **P2:** subcontracts and materials. **P3:** labour true-up and equipment. **P4:** billing/revenue inputs, intercompany, group reporting. | ERP integration for posted cost (VD-17, 18, 19); Finance policies Q6-F1, Q6a-F1; approval values Q14-O1. | Earliest reliable budget-vs-actual. Site operations stay on the existing tools longer. |
| **B. Country-by-country** | **P1:** all must-haves for one country's entities (India or UAE). **P2:** the second country. **P3:** group reporting and intercompany. | Professional validation of the first country's statutory items; UAE e-invoicing timing (X-18); per-entity hosting (RT-05). | Statutory risk is contained, but cross-country items (intercompany, JV, group reporting) come late. |
| **C. Pilot project then scale** | **P1:** one or two pilot projects with the full cost cycle. **P2:** rollout across the remaining projects of the entity. **P3:** other entities and countries. | Pilot selection; migration rules (blocked by RT-02); volumes (Q2). | Lower risk, real test data for the acceptance criteria. Longer until there is portfolio-wide reporting. |
| **D. Operations first** | **P1:** materials, labour, equipment, subcontract records with operational cost. **P2:** budgets/changes, ERP reconciliation. **P3:** billing and group reporting. | Store and site processes; timesheet capture; mobile/offline (NF-13). | Better site data early, but budget-vs-actual is not reliable until P2. |

**Common prerequisites for every option:**
- RT-02 inputs, if the existing PMS is to be extended (X-30).
- The Finance validation items.
- Approval values.
- An ERP decision, which needs your explicit instruction.

### 12.2 Cost categories (kept separate, per Q17-P3)
| Category | What it covers | Public pricing available? |
|---|---|---|
| Software subscriptions | ERP licences; any PMS platform licences | Partly. Public list prices recorded 2026-11-03 in research.md §4:<br>• **Odoo:** Standard ₹580 and Custom ₹1,080 per user/month, yearly, geo-localised.<br>• **Zoho Books US:** $15–$240 per organisation/month by plan. Zoho One $37 (all-employee) or $90 (flexible) per user/month.<br>• **Business Central:** Essentials $80, Premium $110, Team Members $8 per user/month, US list.<br>• **SAP, Infor, CMiC, Acumatica, IFS:** **Quote required.**<br>List prices are not quotes, and regional prices differ. |
| Configuration and custom development | Configuring a product, or developing or extending our PMS | **Quote required** / internal estimate needed. No basis yet: depends on Q2 volumes, RT-02 and option choice. |
| Integration | PMS ↔ ERP, payroll/HR, tax/e-invoicing services, banks | **Quote required.** Depends on ERP choice and VD-18, VD-19, VD-28. |
| Data migration | Existing PMS and ERP data | **Quote required.** Blocked by RT-02. |
| Hosting and security | Cloud hosting, backup/DR, security tooling, penetration testing | **Quote required.** Depends on RT-05b regions and Q16-O1…O4. |
| Training and change management | Users at office and site, in each country | Internal or **quote required**. Depends on Q2 user counts. |
| Ongoing support | Vendor support, internal support team, release management | **Quote required.** |
| Country-specific compliance services | Tax/e-invoicing service providers (e.g., UAE accredited service provider), professional validation (tax, legal, audit) | **Quote required.** |

**Estimate policy:** any later planning estimate must state its assumptions, data sources and a confidence level (low / medium / high). It must not be presented as a budget or commitment.

---

## 13. Deployment, security and data requirements (Q16)
Legal citations for this section are in statutory-research Part F (RT-05a). Vendor hosting facts are in `hosting-research.md` (RT-05b). Each item below is tagged **(S)** statutory, **(K)** contractual or **(R)** recommended practice, according to those sources.

| ID | Requirement | Type | Basis |
|---|---|---|---|
| NF-01 [OP] | Preferred deployment is managed cloud, subject to country, client-contract, security and integration requirements. Per-entity region selection is **not assumed**. It must be verified for each candidate (VD-30). | — | Q16-P1, Q16-P2 |
| NF-02 [OP] | Tenant and accounting-entity isolation: users and data of one accounting entity are not visible to another without an explicit group role. Group reporting uses authorised, minimised data. | R | Q16-P3 |
| NF-03 [OP] | Hosting, backup, DR and support-access locations are documented per entity, and must meet any applicable statutory or contractual location rule (Part F). | S/K | Q16-P3, RT-05a |
| NF-04 [OP] | SSO with MFA for interactive users. RBAC by entity, project and role. Least privilege. | R (K where a contract requires it) | Q16-P3 |
| NF-05 [OP] | Encryption in transit and at rest. | R (K where a contract requires it) | Q16-P3 |
| NF-06 [OP] | Immutable audit log of create, approve, change and reverse actions, and of exports. | S (where statutory audit-trail rules apply; India professional validation X-20) / R | AU-01, ST-14 |
| NF-07 [OP] | Retention periods configurable per country and record type. Values **[TBP]** after professional validation (X-19). | S | ST-14 |
| NF-08 [OP] | Personal-data minimisation. The PMS holds only: employee ID, name, trade/role, entity, site, approved hours and authorised cost rate. Payroll/HR detail stays in the authoritative systems. **No medical records.** | S/R | Q16-P4, X-29 |
| NF-09 [OP] | Cross-border transfer of personal data for group reporting follows the applicable rules (Part F). Group reports use aggregated cost wherever possible. | S | Q16-P3 |
| NF-10 [TBP] | Recovery point objective (RPO) and recovery time objective (RTO) per entity (Q16-O2). | K/R | Q16-P3 |
| NF-11 [OP] | Backups and DR are tested at an agreed frequency, with results recorded. | R | Q16-P3 |
| NF-12 [OP] | Data export, portability and vendor exit: full export in a documented format, with contractual exit provisions. | R/K | Q16-P3 |
| NF-13 [OP] (Should) | Mobile entry with offline capture at sites with unreliable connectivity, and duplicate-safe synchronisation. | R | Q16-P3 |
| NF-14 [OP] | Integration monitoring: last successful update and delay alerts (RP-15). | R | Q15-P4 |

Values still to be provided: client and government hosting clauses (Q16-O1), RPO/RTO (Q16-O2), UAE revenue bands (Q16-O3) and any on-premises requirement (Q16-O4).
