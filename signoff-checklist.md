# Sign-off Checklist — Construction PMS Requirements

**Version:** 1.0-draft · **Date:** 2026-11-03 · **Relates to:** `requirements.md` v1.0-draft (Draft for business and finance validation)
**Purpose:** List every open item that must be resolved before the requirements can be used. Each item shows its priority, the responsible role, the requirements it affects, and which stage it blocks.
**Rule:** Nothing here has been approved. No values have been invented. Recommendations in the source files stay *proposed*.

## How to read

**Priority**
- **P1** blocks **design**.
- **P2** blocks **configuration**.
- **P3** blocks **data migration**.
- **P4** blocks **go-live**.

Items are listed in priority order within each section.

**Blocks columns:** **D** = design, **C** = configuration, **M** = migration, **G** = go-live. ● = blocks.

**Status values:** Open · In progress · Provided · Approved.

---

## A. Business facts and values to be provided

| # | Pri | Item | Responsible role | Affects | D | C | M | G | Source | Status |
|---|---|---|---|---|---|---|---|---|---|---|
| A1 | P1 | Final scope confirmation of the owner-proposed decisions (Batches 1–5) | Business owner / MD | All [OP] requirements | ● | ● | ● | ● | business-decisions.md | Open |
| A2 | P1 | Volumes: concurrent projects, subprojects, office and site users, monthly POs, timesheets and claims | Operations director | Q2; AC-52 and performance criteria; §12 | ● | ● |  | ● | Q2-O1…O6 | Open |
| A3 | P1 | Actual JV structures and agreements | Business owner + Legal + Finance controller | HR-02, HR-08, HR-09, REQ-026 | ● | ● |  | ● | Q1b-O3, X-1, X-27 | Open |
| A4 | P1 | Budget, delivery deadline, internal team size, implementation-partner tolerance | Business owner / CFO | §12 options; Q17 | ● |  |  |  | Q17-O1 | Open |
| A5 | P1 | Client and government hosting clauses; any on-premises requirement | Commercial / contracts manager + IT/security | NF-01…NF-03 | ● | ● |  | ● | Q16-O1, Q16-O4 | Open |
| A6 | P2 | Approval amounts per role, entity and document type; approval combinations; incompatible-duty matrix; purchase-splitting and amendment rules | CFO + Project director | AP-01…AP-10, AC-18, AC-19 |  | ● |  | ● | Q14-O1, O2, O4, O5 | Open |
| A7 | P2 | Matching tolerances (percentage and absolute) | Finance controller + Procurement manager | MW-04, AP-08, AC-19 |  | ● |  | ● | Q14-O2 | Open |
| A8 | P2 | Contingency amounts or percentages, and release approvers | Project director + CFO | RS-8, AC-15 |  | ● |  | ● | Q8-O1/O2 | Open |
| A9 | P2 | Retention %, caps, advance terms, release triggers and mandatory documents per contract and country (subcontract side) | Commercial manager (QS) | SW-08, SW-09, SW-11 |  | ● |  | ● | Q10-O1 | Open |
| A10 | P2 | Client retention, advance terms and billing cycles per contract | Commercial manager | CB-11 |  | ● |  | ● | Q13-O1 | Open |
| A11 | P2 | Equipment rates, idle-time policy and charging terms | Plant manager + Finance controller | EW-02, EW-03 |  | ● |  | ● | Q11-O1 | Open |
| A12 | P2 | Wastage norms per material and activity | Project engineering lead | MW-11 |  | ● |  | ● | Q12-O1 | Open |
| A13 | P2 | Close calendars, reporting deadlines and snapshot approvers per entity | Finance controller | PE-01, RP-10 |  | ● |  | ● | Q15-O1, Q15a-O1 | Open |
| A14 | P2 | Intercompany pricing policies and approvers | CFO + Tax adviser | IC-02, IC-03 |  | ● |  | ● | Q1b-O4 | Open |
| A15 | P2 | RPO/RTO per entity | IT/security + Business owner | NF-10, AC-52 |  | ● |  | ● | Q16-O2 | Open |
| A16 | P2 | UAE entity revenue bands (e-invoicing phase) | Finance controller (UAE) | ST-09, X-18, X-25 |  | ● |  | ● | Q16-O3 | Open |
| A17 | P2 | Group cost-control administrator (named role) and cost-code change process | Business owner | CC-03 |  | ● |  |  | Q5-O1 | Open |
| A18 | P2 | Integration-delay threshold for dashboards | IT + Finance controller | RP-15 |  | ● |  |  | RP-15 | Open |
| A19 | P3 | Meaning and quality of existing Budget/Actual values (business confirmation; no production access) | Project controls lead | HR-07 |  |  | ● |  | existing-pms-inspection §9.3 | Open |

## B. Finance and HR policy validations

| # | Pri | Policy | Responsible role | Affects | D | C | M | G | Source | Status |
|---|---|---|---|---|---|---|---|---|---|---|
| B1 | P1 | Reporting framework per entity (IFRS / IFRS for SMEs / Ind AS / AS / non-company level) | CFO + Auditor | CB-09, ST-19 | ● | ● |  | ● | Q1-F1, Q13-R1 | Open |
| B2 | P1 | Eligible posted project cost (which ledger postings count) | Finance controller | §3.4, PC definition | ● | ● |  | ● | Q6a-F1 | Open |
| B3 | P1 | Direct-to-work vs inventory policy | Finance controller | RR-02, MW-08, X-8 | ● | ● |  | ● | Q6-F1 | Open |
| B4 | P1 | Revenue policies per entity and contract type: method; uninstalled materials (B19 / AS 7); variable consideration; liquidated damages | CFO + Auditor | CB-06…CB-08, Ex 16 | ● | ● |  | ● | Q13-F1, statutory-research E.5 | Open |
| B5 | P1 | Back-charge accounting treatment(s) | Finance controller | SW-10, Ex 13 | ● | ● |  | ● | Q10-F1 | Open |
| B6 | P1 | Owned-equipment project cost basis (internal charge vs pool actual) and under/over-recovery treatment | Finance controller | EW-07, Ex 14, X-23 | ● | ● |  | ● | Q11-F1 | Open |
| B7 | P1 | Consolidation policy, including JV treatment and intercompany elimination | CFO | IC-06, HR-09, Ex 15b | ● | ● |  | ● | Q1b-P9, Q1b-P15 | Open |
| B8 | P2 | FX: rate sources, accrual valuation rate, commitment revaluation, group translation, FX rule for approval values | Finance controller | DM-01, RP-19, AP-02 |  | ● |  | ● | Q1-F1, Q1a-P5, Q6b-F1, Q6b-F2, Q14-O3 | Open |
| B9 | P2 | Inventory valuation method; abnormal loss and excess wastage classification | Finance controller | §3.3, MW-12 | ● | ● |  | ● | Q12-F1, Q12-F2 | Open |
| B10 | P2 | Overhead recognition and allocation; cost-centre categories | Finance controller | RR-08, DM-02 |  | ● |  | ● | Q6-F2, Q5a-O2 | Open |
| B11 | P2 | Definition of a "probable" pending change | CFO | PPC, CM-5 |  | ● |  | ● | Q9-F1 | Open |
| B12 | P2 | Payroll cost components per country; burden and idle allocation; labour-contractor classification | Finance controller + HR/payroll | LW-02, LW-06, Ex 8 |  | ● |  | ● | Q7-F1, F2, F3 | Open |

## C. Professional (tax / legal / audit) validations

| # | Pri | Item | Responsible role | Affects | D | C | M | G | Source | Status |
|---|---|---|---|---|---|---|---|---|---|---|
| C1 | P1 | Whether PMS records fall within India's audit-trail and India-backup rules | Statutory auditor + Legal (India) | NF-03, NF-06, X-20 | ● |  |  | ● | statutory-research A.5, F | Open |
| C2 | P1 | Data-protection obligations and cross-border transfer for HR and costing data (India DPDP; UAE PDPL, DIFC, ADGM) | Legal / DPO | NF-08, NF-09 | ● |  |  | ● | statutory-research A.6, B.6, F | Open |
| C3 | P2 | Tax point for certificates, retention and advances (GST / VAT) | Tax adviser | CD-7, ST-03, ST-04 |  | ● |  | ● | A.7 (3), B.8 (1–2) | Open |
| C4 | P2 | India TDS classification: s.393 items; equipment hire with operator; manpower supply | Tax adviser (India) | ST-08, X-21 |  | ● |  | ● | A.7 (1–2) | Open |
| C5 | P2 | Input-tax recoverability and blocked credits (India works contracts; UAE accommodation and welfare) | Tax adviser | ST-05, Q6a-P3 |  | ● |  | ● | A.7 (6), B.8 (4) | Open |
| C6 | P2 | Transfer documentation and valuation: distinct persons, related parties, cross-border, temporary import and redeployment | Tax / customs adviser | MW-09, MW-10, IC-07, ST-18 |  | ● |  | ● | D.4 | Open |
| C7 | P2 | Intercompany valuation: UAE Art. 26/27 interaction with TP; VAT Art. 36; GST Rule 28; India TP (2025 Act) | Tax adviser | IC-03, ST-22 |  | ● |  | ● | G.3 | Open |
| C8 | P2 | UAE e-invoicing scope: 14-day rule, Tax Group exclusion, subcontractor self-billing | Tax adviser (UAE) | CB-05, ST-09, X-25 |  | ● |  | ● | B.8 (6) | Open |
| C9 | P2 | Record-retention period per country, unified into one policy | Legal + Auditor | NF-07, ST-14, X-19 |  | ● |  | ● | B.8 (5) | Open |
| C10 | P2 | Labour-law items: UAE WPS, pension and overtime; India Labour Codes, BOCW cess and contract labour | HR + Legal | ST-10…ST-13 |  | ● |  | ● | A.7 (7–8), B.8 (8–9) | Open |
| C11 | P2 | Verify every "Unverified" statutory item against official text | Tax / legal advisers | All [RT] items |  | ● |  | ● | statutory-research Unverified lists | Open |
| C12 | P1 | Exact DPDP dates (Gazette 13 vs 14 Nov 2025; computation of 1-year and 18-month periods); SDF exposure; CERT-In offshore-log position; UAE PDPL Executive Regulations status; DIFC/ADGM presence of any entity | Legal / DPO + IT/security | NF-03, NF-08, NF-09 | ● |  |  | ● | statutory-research F.4 | Open |

## D. Vendor checks (all Pending verification)

| # | Pri | Checks | Responsible role | Affects | D | C | M | G | Status |
|---|---|---|---|---|---|---|---|---|---|
| D1 | P1 | VD-17, VD-18, VD-19: double-count test; ERP line-level postings; PMS-originated commitments | IT / integration lead + Finance controller | REC-C, IW-03, IW-04, SW-01 | ● | ● |  | ● | Pending verification |
| D2 | P1 | VD-30: hosting, isolation, DR, support access, exit | IT/security | NF-01…NF-12 | ● |  |  | ● | Pending verification |
| D3 | P1 | VD-31, VD-32: accounting entity for JV; intercompany documents | Finance controller | HR-08, IC-04 | ● | ● |  |  | Pending verification |
| D4 | P2 | VD-01…VD-16: vendor-specific conflicts from research.md §10.2 | Project controls lead | as listed per check |  | ● |  |  | Pending verification |
| D5 | P2 | VD-20…VD-29: Batch 3–4 scenarios (budget, changes, contingency, approvals, close, subcontracts, equipment, materials, billing, revenue) | Project controls lead + Finance controller | per check |  | ● |  | ● | Pending verification |

Vendor checks are run only on your instruction and only for candidates still under consideration. Running them does not select an ERP.

## E. Missing inputs (RT-02, partially blocked)

| # | Pri | Input | Responsible role | Affects | D | C | M | G | Status |
|---|---|---|---|---|---|---|---|---|---|
| E1 | P1 | Database schema export (DDL) of the existing PMS | IT | Q17-P1 feasibility (X-30); HR-07 | ● |  | ● |  | Open |
| E2 | P1 | Stored-procedure definitions | IT | Reuse of roll-up, QC and audit log; migration | ● |  | ● |  | Open |
| E3 | P1 | `CommonClass` source code | IT | Tenant model, security remediation | ● |  | ● |  | Open |
| E4 | P3 | Confirmation of which page versions are live | IT | Migration scope |  |  | ● |  | Open |

## F. Remaining contradictions (from business-decisions.md)

| ID | Short description | Status / resolution path |
|---|---|---|
| X-1 | JV ownership vs one entity per project | Resolved in direction (Q1b-C2). Values in A3. |
| X-2 | Who originates POs and calculates tax | Direction Q6b-P1/P3. Verified through VD-19. |
| X-3 | FX between commitment and actual | Direction Q6b-P13. Values in B8. |
| X-4 | Programme roll-up across currencies | Blocked by B8. |
| X-5 | Task records must not post cost | Resolved by HR-04 and HR-05. |
| X-6 | Capacity criteria need volumes | A2. |
| X-7 | Compliance needs research and validation | RT-01 done. C1–C11 open. |
| X-8 | Inventory policy must match ERP booking | B3 + VD-18. |
| X-9 | ERP must expose line-level postings | VD-18. |
| X-10 | Tax and vendor validation at PO approval | VD-19. |
| X-11 | Employee rates vs pay privacy | Resolved in direction (Q14-P12, NF-08). |
| X-12, X-22 | Tax point vs management actual date | Separate dates (CD-1…CD-8). C3. |
| X-13 | Late postings and closed months | Resolved in direction (Q15a). Values in A13. |
| X-14 | Probable pending cost vs ETC | Rule F-2. Definition in B11. |
| X-15, X-28 | At-risk cost placement and budget authority | Resolved in direction (AR-01…AR-05). |
| X-16 | FX for approval values | B8. |
| X-17 | Contingency release across subprojects | Proposed: stricter rule applies (RS-5). Values in A6. |
| X-18, X-25 | UAE e-invoicing timing vs PMS billing | A16, C8. |
| X-19 | Retention periods differ by country | C9. |
| X-20 | India audit-trail / backup scope | C1. |
| X-21 | India TDS classification of equipment hire and manpower | C4. |
| X-23 | Internal equipment charge vs accounting cost | B6. |
| X-24 | Back-charge reconciliation to ERP | B5 + VD-25. |
| X-26 | Intercompany process dependency | Resolved in direction (Q1b-P10…P16). Values in A14. |
| X-27 | Accounting entity may not exist in the ERP | VD-31. |
| X-29 | Personal-data minimisation vs timesheets | Proposed minimum dataset NF-08. C2. |
| X-30 | Extending the PMS depends on RT-02 | E1–E3. |

## G. Unsupported or unverified claims (summary)
- **Research quality:** most statutory findings come from secondary sources, because official portals blocked automated access. The detailed lists are in statutory-research A.7, B.8, D.4, E.5, F and G.3, plus each part's "Unverified" items.
- **Vendor capability:** cells marked "Unverified" or "Not found in public docs" in research.md §5 are claims about the documentation reviewed, not proof that the capability is missing (CR-3).
- **Vendor marketing:** marketing claims (e.g., CMiC NEXUS AI, Acumatica AI anomaly detection) are not treated as capabilities.
- **Prices:** prices recorded in research.md are list prices fetched on 2026-11-03. Regional prices differ, and they are not quotes.
- **Worked examples:** all example numbers are illustrative. Their conditions are stated in requirements.md §3.9 (CR-6).

## H. Sign-off sequence (proposed)
1. **P1 business and Finance items:** A1–A5, B1–B7, C1–C2, D1–D3, E1–E3. Their outcome unblocks **design**.
2. **P2 items:** unblock **configuration**.
3. **P3 items:** unblock **migration**.
4. All acceptance criteria AC-01…AC-52 are tested, and all P4 / go-live items are closed. Then **go-live** can be considered.

Each sign-off is recorded in the business-decisions.md decision log with date and approver.
