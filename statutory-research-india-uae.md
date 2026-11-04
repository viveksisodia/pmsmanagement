# RT-01 — Statutory Research: India and UAE (research only)

**Research date:** 2026-11-03
**Status:** Research findings only. These are **not** approved finance or tax policy, **not** tax advice, and **not** requirements until approved through `business-decisions.md`. India and UAE rules must **not** be assumed for any other country.

**Classification key**
- **L** — legal requirement
- **C** — configurable company policy or contract practice
- **P** — needs professional (tax, legal or audit) validation

**Source quality**
- Most official portals blocked automated fetching: incometaxindia.gov.in, tax.gov.ae and gpssa.gov.ae returned 403 or were rejected; cbic-gst.gov.in, mca.gov.in and uaelegislation.gov.ae were not fetched.
- Many findings therefore rest on search excerpts of official documents, or on secondary professional sources (Big-4 firms, law firms, tax publishers). These are marked **Unverified**.
- Every rate, threshold and date must be checked against the Gazette or official text before sign-off.

---

## Part A — India

### A.0 Laws in force on 2026-11-03
| Item | Status | Source |
|---|---|---|
| Income-tax Act, 2025 replaces the Income-tax Act, 1961 | In force from 1 Apr 2026. Income-tax Rules 2026 effective 1 Apr 2026. Non-salary TDS is consolidated in s.393 (table-driven). | https://www.incometaxindia.gov.in/documents/d/guest/press-release-income-tax-act-2025-comes-into-force-from-01-april-2026-pdf (official, located) |
| Four Labour Codes (Wages, Industrial Relations, Social Security, Occupational Safety & Health) | In force from 21 Nov 2025. Central rules reported notified May 2026. State rules vary and are still being notified. | https://labour.gov.in/en/faqs-labour-codes ; https://www.livelaw.in/amp/top-stories/centre-brings-four-labour-codes-into-operation-with-effect-from-november-21-310772 |
| DPDP Act 2023 and DPDP Rules 2025 | Phased commencement. Exact dates are in **Part F** (RT-05a): 13 Nov 2025 / 13 Nov 2026 / 13 May 2027, calculated from a Gazette date of 13 Nov 2025, which still needs verifying. *Corrected 2026-11-03: the earlier approximate wording is replaced.* | Part F |

### A.1 GST
| Rule | Class | Effective | PMS implication | Source |
|---|---|---|---|---|
| A works contract is a supply of services (composite supply, SAC 9954). | L | 1 Jul 2017 | Classify each contract (works contract, goods, other service). Store SAC/HSN on PO, subcontract and BoQ lines. | cbic-gst.gov.in (CGST Act) |
| GST 2.0 rate changes (Notification 15/2025-CT(Rate)), e.g. reported changes to works-contract and cement rates. | L/P | 22 Sep 2025 | The rate master must be effective-dated. Open commitments and unbilled work may need re-rating across a rate-change date. | busy.in (secondary); notification not read — **Unverified rates** |
| Place of supply for services related to immovable property is the location of the property (IGST s.12(3)). | L | 2017 | Capture site state per project/site and supplier and recipient GSTINs. Registration needs per state are **P**. | CGST/IGST Acts |
| Time of supply for services; invoicing within 30 days (Rule 47); continuous supply invoiced at payment due date or milestone (s.31(5)). | L | 2017 | Store work-done date, certification date, invoice date and payment date separately. | CGST Act |
| Advances for services are taxable on receipt. Advances for goods are exempt (Notification 66/2017). A receipt voucher is required. | L | 2017 | Track mobilisation advances and their recovery. Flag advances that relate to services. | CBIC |
| Retention money forms part of taxable value. Timing depends on how the invoice is drafted. | P | — | Store gross value, retention held and retention released separately. Do not net retention out of taxable value without tax sign-off. | — |
| Blocked input tax credit for works contracts and own-account construction of immovable property, except plant and machinery (s.17(5)(c)/(d)). The Finance Act 2025 amended this retrospectively. | L/P | Retro 1 Jul 2017 | Tag each cost line as eligible, blocked or capital credit. Distinguish client projects from own-asset construction. | taxguru; NLIU Law Review (secondary) |
| Reverse-charge items common in construction: renting by an unregistered person (entry 5AB), GTA, certain security services, advocates, government services. | L | 5AB: 10 Oct 2024 | Vendor registration-status flag. Reverse-charge flag on PO and service lines (self-invoice raised in ERP). | taxmann; taxguru (secondary) |
| GST TDS 2% when the recipient is a government or notified body (s.51). | L | 1 Oct 2018 | Customer-contract deductor type and expected GST TDS, so receipts reconcile. | CBIC |
| Distinct persons: inter-state stock transfers between own GSTINs are taxable supplies. Same-GSTIN movements need a delivery challan. | L | 2017 | Inter-state site-to-site transfers need a tax invoice and transfer value. The PMS must know the GSTIN of both sites. | CGST Act |
| Input Service Distributor mandatory for distributing common input services across GSTINs. | L | 1 Apr 2025 | Head-office allocation to projects in other states must align with ISD. | CBIC (not fetched) — **Unverified detail** |
| E-way bill for goods movements above ₹50,000 (state thresholds vary). 180-day document limit and 360-day extension cap. | L | 2018; limits 1 Jan 2025 | Transfer, issue and return documents carry value, HSN, vehicle, distance and e-way bill number. Per-state threshold table. | taxguru (GSTN advisory) |

### A.2 E-invoicing (IRN/IRP)
| Rule | Class | Effective | PMS implication | Source |
|---|---|---|---|---|
| E-invoice mandatory for B2B supplies where aggregate turnover in any year since FY 2017-18 exceeds ₹5 crore. | L | 1 Aug 2023 | Client running bills and inter-GSTIN transfer invoices need an IRN/QR from the ERP. Subcontractor invoices should carry a valid IRN, which the PMS captures or validates. | CBIC (not fetched) |
| AATO ≥ ₹10 crore: report invoices and credit/debit notes to the IRP within 30 days. | L | 1 Apr 2025 | Certification must flow to invoicing promptly. Credit notes for back-charges and retention adjustments also fall within 30 days. | GSTN advisory via mahagst.gov.in; taxscan |

### A.3 Income-tax withholding (TDS) under the Income-tax Act, 2025
Note: the CBDT FAQ is reported to say rates and thresholds carried over from the 1961 Act. Returns must quote s.393 table items.

| Rule | Class | Effective | PMS implication | Source |
|---|---|---|---|---|
| Contractors and subcontractors (old 194C): s.393(1) Table Sl. 6(i). 1% for individuals/HUF, 2% for others. Thresholds ₹30,000 per payment and ₹1,00,000 per year (secondary). | L | 1 Apr 2026 | Vendor master needs PAN, entity type and TDS table item. Commitment type drives the default TDS code. | tdsman; incometaxindia FAQ (excerpt) |
| Professional/technical fees (old 194J): Sl. 6(iii). Classification of manpower supply is in question. | L/P | 1 Apr 2026 | Classify consultants, design, testing and manpower supply separately. | incometaxindia memo-2026 (located) |
| Rent (old 194I): 2% for plant and machinery, 10% for land/building (rates under the 2025 Act unconfirmed). | L/P | 1 Apr 2026 | Split equipment hire into bare rent and hire with operator (possibly a contract). Needs tax sign-off. | secondary |
| Purchase of goods (old 194Q): 0.1% above ₹50 lakh per seller per year. Carry-over of the turnover test is disputed. | L/P | 1 Apr 2026 | Track cumulative purchases per supplier PAN per tax year across projects and entities on the same PAN. | tdsman; taxheal |
| Timing: on credit or payment, whichever is earlier. | L | 1 Apr 2026 | Receipt or certificate approval that posts a liability may trigger TDS. Unbilled accruals and year-end provisions need a TDS rule. | taxguru |
| Lower/nil deduction certificate: s.395, Form 128. | L | 1 Apr 2026 | Vendor master holds certificate number, rate, value limit and validity, with usage tracked against the limit. | tdsman |
| TDS on value excluding GST where GST is shown separately (Circular 23/2017 under the 1961 Act). | P | — | Store GST and base value separately on every bill. Carry-over **Unverified**. | — |
| MSME payment within 45 days (MSMED Act s.15). | L | — | Vendor Udyam number and category; MSME ageing report. Income-tax disallowance equivalent under the 2025 Act **Unverified**. | — |

### A.4 Payroll and labour cost
| Rule | Class | Effective | PMS implication | Source |
|---|---|---|---|---|
| Uniform "wages" definition: allowances above 50% are added back for statutory calculations. | L | 21 Nov 2025 | Labour rate build-up must recompute the statutory base. Rates are supplied by payroll. | labour.gov.in FAQs |
| EPF 12% + 12%. Wage ceiling reported raised to ₹25,000 (S.O. 5109(E)). | L/P | 17 Sep 2026 (reported) | Statutory on-cost rates are effective-dated. **Unverified** (secondary). | dcpsol; elplaw |
| ESI 3.25% employer / 0.75% employee, for wages up to ₹21,000. | L | Jul 2019 | Effective-dated on-cost table. | secondary |
| Gratuity under the Social Security Code: fixed-term employees qualify after 1 year. Contractor bears gratuity for contract labour. | L | 21 Nov 2025 | Gratuity accrual in the cost rate. Accrual method C/P. | labour.gov.in FAQ |
| Bonus minimum 8.33%, maximum 20%. | L | 21 Nov 2025 | Bonus provision in the cost rate. | — |
| Minimum/floor wages by state, zone and skill. Overtime at twice the ordinary rate. Working-hours limits. | L | 21 Nov 2025 | Timesheets capture normal and OT hours, skill category and work state. | — |
| Professional tax and Labour Welfare Fund (state Acts). | L | Varies by state | Work-state per employee. | — |
| BOCW cess 1% of construction cost under the Social Security Code (S.O. 2322(E) reported 8 May 2026). | L/P | Code 21 Nov 2025; rate 8 May 2026 (reported) | Project budget cess line. Definition of "cost of construction" is **P**. Track per project and state. | taxmann; taxheal — **Unverified notification** |
| Single establishment registration under the OSH Code. | L | 21 Nov 2025 | Project/site registration number and status. | — |
| Contract labour: contractor licence threshold reported raised to 50 workers. Principal-employer welfare duties and verification of contractor wage/PF/ESI payments. | L/P | 21 Nov 2025 | Subcontract master: licence and validity, workforce count per site. Optional hold on certificate payment pending compliance evidence (C). | labour.gov.in FAQs; mondaq — **threshold Unverified** |

### A.5 Accounting and records
| Rule | Class | Effective | PMS implication | Source |
|---|---|---|---|---|
| Ind AS applies to listed companies and those with net worth ≥ ₹250 crore. Others use AS (AS 7 covers construction contracts). | L/P | 2016 phasing | Reporting basis per legal entity (Ind AS or AS). | MCA |
| Ind AS 115: over-time recognition (input method common). Some costs may need adjusting out of an input measure of progress, for example uninstalled materials, but only when conditions in the standard are met (see RT-03). Variable consideration is constrained. *Corrected 2026-11-03: the earlier wording implied uninstalled materials are always excluded.* | L | 1 Apr 2018 | Cost to date, ETC, uninstalled-material flag, variation status, contract assets and liabilities. | MCA |
| Ind AS 2: site inventory valued at lower of cost and NRV. Issued materials become contract cost. | L | 2016 | Received vs issued tracked separately (consistent with REC-B). | MCA |
| Ind AS 21: transaction-date rate. | L | — | Currency, rate and date on commitments (consistent with DM-01). | MCA |
| Ind AS 37 / AS 29: onerous contracts. | L | — | EAC must flag loss-making contracts. | MCA |
| Accounting software must keep a non-disableable audit trail (edit log) (Companies (Accounts) Rules 2014 r.3(1)). | L | 1 Apr 2023 | Immutable change log on financial records. Whether the PMS counts as accounting software is **P**. | MCA (G.S.R. 235(E)) |
| Books and audit trail kept for 8 years. Daily backup of electronic books on servers in India. | L | Backup rule 5 Aug 2022 | ≥ 8-year retention. India-hosted backup if PMS data forms part of the books (**P**). | MCA |

### A.6 Data protection
| Rule | Class | Effective | PMS implication | Source |
|---|---|---|---|---|
| DPDP Act 2023 / Rules 2025: notice, security safeguards, breach notification, retention and erasure. Employment purposes are a legitimate use. | L | Phased. See Part F for the dates per provision: core obligations 13 May 2027 (calculated; Gazette verification pending). | Role-based access, logging, breach-reporting support, retention and erasure rules for worker data. | Part F |
| Cross-border transfer allowed unless restricted by government. No general localisation rule. | L/P | — | Hosting region is a policy choice, subject to the Companies Act India-backup rule. | — |

### A.7 India — items requiring professional validation
1. s.393 table items, rates and thresholds (rent of plant and machinery, professional fees, 194Q test, no-PAN rate).
2. Equipment hire with operator, and manpower supply — which TDS category applies.
3. GST tax point for running bills, retention release and milestones under s.31(5).
4. Registration requirement in each site state.
5. Post-GST 2.0 works-contract and goods rates.
6. Blocked-credit classification of temporary works and own-use buildings.
7. BOCW cess base and self-assessment procedure.
8. Principal-employer liability for contract labour.
9. Whether the PMS is within the Companies Act audit-trail and India-backup scope.
10. Ind AS 115 method per contract and treatment of uninstalled materials.

---

## Part B — UAE

### B.0 Key changes 2025–2026
- VAT Law and Tax Procedures Law amendments: Decree-Laws 16/2025 and 17/2025, effective 1 Jan 2026.
- E-invoicing: pilot from 1 Jul 2026; Phase 1 go-live 1 Jan 2027.
- New Wage Protection System (WPS) resolution (Ministerial Resolution 340/2026): effective 1 Jun 2026.

### B.1 VAT
| Rule | Class | Effective | PMS implication | Source |
|---|---|---|---|---|
| Standard rate 5%. Rate, zero-rating and exemptions unchanged by the 2026 amendments. | L | 2018; confirmed 1 Jan 2026 | Tax codes are master data. No hard-coded rates. | https://www.wam.ae/en/article/bn10b09-ministry-finance-implement-vat-law-amendments |
| Decree-Law 16/2025: self-invoicing for reverse charge abolished (buyer keeps supporting documents); 5-year limit on recovering credit balances. | L | 1 Jan 2026 | Keep supplier invoice and contract on reverse-charge PO/receipt records. | WAM; DLA Piper |
| Periodic/progress payments: date of supply is the earliest of invoice, payment due or payment received, with a 12-month backstop (Art. 26). | L (detail P) | 2018 | Store certificate date, invoice date, payment due date and payment date separately. Certificate date ≠ tax point by default. | secondary — **Unverified** (FTA guide not fetched) |
| Advances: receipt of payment triggers date of supply for the amount received. | L | 2018 | Advance records need a VAT tax-point flag and recovery against certificates. | **Unverified wording** |
| Retentions: VAT at the earlier of release/payment or invoice. Permanently withheld retentions treated as not taxable (FTA guide). | P | FTA Real Estate guide (revised) | Separate fields for gross, retention held, released and invoiced. Release is a tax-point event. | BDO (secondary) — **Unverified** |
| Tax invoice contents, including supplier and recipient TRN, date of supply, VAT in AED. | L | 2018 | TRN is master data. Capture supplier invoice number, date and TRN on matched receipts and certificates. | **Verify current ER text** |
| Input tax recovery and blocked items (ER Art. 53: entertainment, personal-use vehicles, employee personal benefits unless legally required). | L/P | 2018; 2026 tightening | Recoverable/blocked attribute per cost code or expense type. Worker accommodation and welfare need tax review. | **Unverified** |
| Reverse charge on imported services and goods; domestic reverse charge for specified goods. | L | 2018 | Foreign-vendor PO reverse-charge indicator and vendor country. | **Verify** |
| Designated Zones: construction services inside a DZ are generally taxable as mainland. DIFC and ADGM are not DZs for VAT. | L/P | List amended periodically | Site attribute for emirate and free-zone/DZ. VAT code derived per line. | **Unverified** current list |
| Residential: zero-rating applies to the developer's first supply. Contractor services are understood to be standard-rated. | L/P | 2018 | Never default "residential project" to 0%. VAT code from tax configuration. | **Contractor treatment Unverified** |
| Record retention: at least 5 years; real-estate records 15 years. Limitation and audit periods changed from 1 Jan 2026. | L/P | 2023; 1 Jan 2026 | Configurable retention, at least 15 years for project/real-estate records. No hard delete of posted transactions. | WAM; KPMG |

### B.2 E-invoicing
| Rule | Class | Effective | PMS implication | Source |
|---|---|---|---|---|
| Legal basis: Ministerial Decisions 64/2025, 243/2025, 244/2025, 56/2026, 66/2026, 168/2026, 197/2026; Cabinet Decision 173/2026 (penalties). | L | MD 243/244 issued 29 Sep 2025 | Requirements must reference the current amended texts. | https://mof.gov.ae/einvoicing/ (official, read) |
| Peppol 5-corner model via Accredited Service Providers (ASPs). PINT AE (UBL XML) format. PDFs are not e-invoices. | L | — | ERP/ASP issues invoices. The PMS supplies structured line data (items, units, prices, VAT category, TRNs, PO/contract reference, progress-period dates). | MoF hub |
| Timeline: pilot/voluntary 1 Jul 2026. Phase 1 (revenue ≥ AED 50m): ASP by 30 Oct 2026, go-live 1 Jan 2027. Phase 2 (< AED 50m): ASP by 31 Mar 2027, go-live 1 Jul 2027. Government entities: go-live 1 Oct 2027. | L | as stated | Determine each UAE entity's revenue band. Large entities must be live by 1 Jan 2027, which may precede PMS go-live. | KPMG; lookuptax |
| Scope: B2B and B2G in; B2C out for now. 24-month exclusion for Tax Group members from 1 Jan 2027. | L | — | Flag inter-company counterparties. | lookuptax — **verify vs MD 243** |
| Mandatory fields (51 tax invoice / 49 commercial). Issue and transmit within 14 business days (MD 197/2026). | L | Oct 2026 | Map billing/certificate data to the PINT AE field list. Match incoming supplier e-invoices to PO/GRN/certificate. | **Unverified** |
| Penalties (Cabinet Decision 173/2026). | L | 2026 | Affects go-live sequencing. | **Unverified amounts** |

### B.3 Corporate Tax
| Rule | Class | Effective | PMS implication | Source |
|---|---|---|---|---|
| 0% up to AED 375,000, 9% above. Qualifying Free Zone Person regime. | L | FY from 1 Jun 2023 | Tax-residency and free-zone status per entity. | secondary |
| Domestic Minimum Top-up Tax of 15% for MNE groups ≥ EUR 750m. | L/P | FY from 1 Jan 2025 | Entity/jurisdiction tagging. | secondary |
| Withholding tax 0%. | L | 2023 | No UAE withholding on payments. | **Unverified** |
| Accounting basis IFRS (IFRS for SMEs below AED 50m). Audited statements above AED 50m. | L | 2023 | PMS POC/WIP must reconcile to IFRS 15 figures. | **Unverified** |
| Transfer pricing at arm's length; documentation thresholds. | L/P | 2023 | Inter-company project services, equipment and secondees need contracts and a recharge basis in the PMS. | **Unverified thresholds** |
| CT record retention 7 years. | L | 2023 | Retention ≥ 7 years (15 years for real estate under VAT). | **Unverified** |

### B.4 Payroll and labour cost (Decree-Law 33/2021 as amended)
| Rule | Class | Effective | PMS implication | Source |
|---|---|---|---|---|
| 8 h/day, 48 h/week normal hours. OT max 2 h/day. 144 h per 3 weeks. Weekly rest day. | L | 2 Feb 2022 | Timesheets flag daily and weekly limits per worker. | secondary — **verify** |
| OT premium +25% of basic hourly wage; +50% at night (22:00–04:00); rest day and public holiday: substitute day or +50%. | L | 2022 | Timesheet hour types with configurable multipliers. Premium base is the basic wage. | Morgan Lewis |
| Annual leave: 2 days/month after 6 months; 30 days/year after 1 year. Public holiday calendar set annually. | L | 2022 | Leave accrual as on-cost. Configurable holiday calendar. | Morgan Lewis |
| End-of-service gratuity: 21 days' basic per year for the first 5 years, then 30 days per year, capped at 2 years' wage. Nationals under GPSSA excluded. | L | 2022 | Monthly EOSB accrual rate on basic wage. Exclude nationals with pension cover. | Morgan Lewis |
| WPS (MR 340/2026): wages due on the 1st of the following month; at least 85% paid via WPS; escalation from day 1. | L | 1 Jun 2026 | Timesheet approval cut-off must support payroll by the 1st. Monthly labour accruals. | Morgan Lewis; Clyde & Co |
| GPSSA pension for UAE nationals (~26% total; employer 12.5–15%, sources conflict); ADPF for Abu Dhabi; GCC schemes. | L/P | 31 Oct 2023 | On-cost by nationality/scheme, configurable. | **Unverified rates** |
| ILOE unemployment insurance is employee-paid. | L | 1 Jan 2023 | Not an employer project cost unless reimbursed (C). | **Unverified tiers** |
| Employer-paid health insurance mandatory in all emirates (Northern Emirates from 1 Jan 2025). | L | as stated | Per-employee on-cost by emirate of visa. | Clyde & Co |
| Midday work ban: 12:30–15:00, 15 Jun–15 Sep 2026; 8-hour daily limit in that period. | L (annual decision) | 2026 season | Timesheets flag outdoor hours in the banned window (date-range rule). | The National |
| Visa, work permit and recruitment costs are employer-borne; recruitment fees cannot be charged to workers. | L/C | 2022 | Mobilisation cost codes. Project allocation is policy. | **Unverified fee schedule** |

### B.5 Accounting
| Rule | Class | Effective | PMS implication | Source |
|---|---|---|---|---|
| IFRS 15: input or output methods. Adjustments to an input measure apply to costs that do not reflect progress, such as wasted resources, and to costs not proportionate to progress, such as uninstalled materials, but only when specific conditions are met (see RT-03). Variations are contract modifications. Variable consideration is constrained. *Corrected 2026-11-03.* | L/P | 2018 | Measured progress, ETC, uninstalled-material flag, approved vs pending variations. | ifrs.org |
| IAS 2 site inventory; IAS 21 FX (AED pegged at 3.6725/USD); IAS 37 onerous contracts. | L | — | Consistent with REC-B, DM-01 and the EAC loss flag. | ifrs.org |
| IFRS 18 replaces IAS 1. | L | 1 Jan 2027 | Management P&L mapping readiness. | ifrs.org |
| Books of account and audit-trail expectations under commercial and tax laws. | L/P | — | Immutable audit log, document attachments, translation on request. | **Unverified articles** |

### B.6 Data protection
| Rule | Class | Effective | PMS implication | Source |
|---|---|---|---|---|
| Federal PDPL (Decree-Law 45/2021) in force. Executive Regulations **not confirmed issued**. | L/P | 2 Jan 2022 | Role-based access, purpose records, data-subject requests, configurable hosting region. | Simmons & Simmons |
| DIFC DP Law 5/2020 and ADGM DP Regulations 2021 for entities in those zones. | L | 2020/2021 | Entity-level data-regime attribute; DPIA for payroll hosting. | **Unverified amendments** |
| Health data localisation (Federal Law 2/2019). | L/P | 2019 | Do not hold employee medical/insurance claim details in the PMS. | **Unverified scope** |

### B.7 Contract practice (not statutory)
| Practice | Class | PMS implication |
|---|---|---|
| FIDIC interim payment certificates, advance payment guarantees, retention (typically 5–10%, with caps; staged release), performance security. | C | Configurable retention %, cap and release events; advance recovery schedule; guarantee register with expiry alerts. |
| Back-to-back subcontracts, pay-when-paid (enforceability **P**), liquidated damages, variation orders. | C/P | Link subcontract certificates to main-contract certificates; LD and contra-charge deductions. |
| Decennial liability (Civil Code Art. 880). | L/P | Completion and handover dates per structure, for liability and warranty registers. |

### B.8 UAE — items requiring professional validation
1. VAT tax point for certificates vs invoice; 12-month backstop.
2. VAT on retentions, including permanently withheld retentions.
3. VAT on contractor services for residential, mixed-use, DZ and DIFC/ADGM sites.
4. Blocked input tax on worker accommodation, transport, meals and medical costs.
5. Unified record-retention period (5, 7 or 15 years).
6. E-invoicing revenue band per entity; 14-day rule; Tax Group exclusion; self-billing profile for subcontractors.
7. Corporate Tax treatment of long-term contracts; transfer-pricing method for inter-company services.
8. Pension contribution rates and the definition of contribution salary.
9. WPS grace period under MR 340/2026; free-zone WPS rules.
10. PDPL Executive Regulations status; payroll/HR hosting across onshore, DIFC and ADGM.
11. Enforceability of pay-when-paid; scope of decennial liability.

---

## Part D — RT-04: Documentation for transfers of materials and equipment (research only, 2026-11-03)
**Source quality.** Most official texts (CBIC, FTA, ICP) were not opened directly, and many items rest on secondary sources. Treat every item below as **Unverified** until checked against the official text and validated by a professional.

### D.1 India, domestic
| Case | Requirement (summary) | Class | PMS implication |
|---|---|---|---|
| Same GSTIN (same state, same entity) | Not a supply: no tax invoice. A delivery challan (Rule 55) carries consignor/consignee, HSN, quantity and declared value. An e-way bill is needed above ₹50,000, including non-supply movements; state intra-state thresholds vary. | L (field list Unverified) | Challan series per GSTIN; declared value field (not book value by default); reason code (returnable / non-returnable / job work); e-way bill number, validity and vehicle; table of state thresholds. |
| Own equipment sent to site and returned | Returnable challan, plus an e-way bill above threshold; a return challan when it comes back. | L/C | Outward and return challans linked; equipment location status. |
| Third-party job work (e.g., fabrication) | Inputs must return within 1 year, capital goods within 3 years, or they are deemed supplied. Inter-state job-work movement needs an e-way bill regardless of value. | L | Due-back dates and alerts. |
| Different GSTIN, same entity ("distinct persons") | Taxable supply even without consideration. Tax invoice with IGST. Valued under Rule 28 (open-market value / 90 % option / invoice value deemed OMV where the recipient has full credit). E-invoice above the turnover threshold. | L/P | Transfer document becomes a tax invoice. Valuation method and justification stored. Never default silently to book value. |
| Capital goods / equipment between states | Supply, with tax at the higher of reversed credit or transaction value (s.18(6)). A circular treats rigs, tools and conveyances as non-supply; whether it covers construction plant is uncertain. | L/P | Equipment master holds credit history and capitalisation date; flag for the circular's applicability. |
| Different legal entities (related) | Supply with tax invoice; Rule 28 valuation; e-invoice and e-way bill. Domestic transfer pricing applies only in limited cases (Income-tax Act 2025 section mapping Unverified). | L/P | Counterparty marked as related; valuation evidence; arm's-length support kept. |
| New state site | May need its own GST registration. | P | Site → state → GSTIN link. |

### D.2 UAE, domestic
| Case | Requirement (summary) | Class | PMS implication |
|---|---|---|---|
| Same legal person, any emirate | Not a supply (VAT registration is per legal person). A delivery note is commercial practice. | L/C | Delivery note series with quantity, value and locations; no VAT fields. |
| Designated zone / free zone ↔ mainland | May be a taxable supply or an import. Customs exit and entry declarations may be needed. | L/P | Location attribute for customs/VAT zone type; declaration number and VAT treatment when crossing a zone boundary. |
| Different legal entities | Taxable supply at 5 % with a tax invoice. Supplies between members of a VAT group are disregarded for VAT. Corporate Tax transfer pricing applies at arm's length. Valuation dimensions are separate; see Part G. | L/P | Both TRNs; a same-VAT-group flag per entity pair; separate valuation fields per Part G. *Corrected 2026-11-03: an earlier version implied that book value is acceptable only under a qualifying-group election. That is withdrawn; see Part G.* |

### D.3 Cross-border (India ↔ UAE)
| Case | Requirement (summary) | Class | PMS implication |
|---|---|---|---|
| Export from India | Shipping bill (ICEGATE); zero-rated under LUT or IGST refund; e-way bill to port. Temporary export and re-import of equipment uses an exemption with conditions (Unverified). | L/P | Shipping bill number and date, LUT/IGST reference; equipment serial numbers; expected return date linked to re-import. |
| Import into UAE | Customs declaration and importer code; GCC duty generally 5 % of CIF value (Unverified); import VAT by reverse charge for registered importers; temporary admission with guarantee and re-export deadline. **ATA carnets generally exclude construction and earth-moving equipment.** | L/P | Declaration number, duty basis (CIF, not book value), suspension flag, guarantee and re-export alerts. No carnet option for heavy equipment. |
| Import into India from UAE | Bill of entry; basic customs duty + Social Welfare Surcharge + IGST (creditable); India–UAE CEPA preferential duty with certificate of origin and origin-evidence duties; temporary/project imports may **restrict redeployment to another project**. | L/P | Customs status per equipment (duty-paid / temporary / project-exempt); linked project; re-export deadline; **block on transfer to another site** without customs approval. |
| Related-party valuation | Transfer-pricing value, customs value and accounting value may all differ, and related-party customs values may be examined. | L/P | Store transfer-pricing, customs and accounting values separately. |

### D.4 Items requiring professional validation (transfers)
1. Whether construction plant moved between states is a "supply" (circular scope), and the s.18(6) calculation.
2. Rule 28 valuation choice and documentation.
3. GST registration need per new state.
4. UAE designated/free-zone ↔ mainland VAT and customs treatment.
5. Whether to use a UAE Corporate Tax qualifying-group election, and VAT grouping.
6. India temporary export/re-import route for equipment going to UAE projects.
7. UAE temporary admission periods and guarantees for construction plant.
8. Indian temporary/project import notifications and redeployment restrictions.
9. Value for free-of-cost cross-border related-party transfers; reconciling customs and transfer-pricing values.

### D.5 Candidate requirement (not approved)
| ID | Candidate requirement |
|---|---|
| ST-18 | Every transfer record holds:<br>• transfer type: same registration / other registration of the same entity / other legal entity / cross-border<br>• tax IDs and the state, emirate or zone of both ends<br>• HSN<br>• **valuation fields kept separate** (Part G)<br>• document numbers: challan or tax invoice, IRN, e-way bill, shipping bill or bill of entry, UAE customs declaration<br>• equipment customs status<br>• linked project<br>• return or re-export deadline<br>• any redeployment restriction |

## Part G — RT-06: Intercompany valuation dimensions (research only, 2026-11-03)
**Source quality:**
- tax.gov.ae and incometaxindia.gov.in blocked fetching.
- UAE Corporate Tax detail relies on Big-4 summaries and MoF announcements.
- India 2025-Act section numbers come from search snippets of official pages.
- Standards references (IFRS 10 B86, IAS 2, IAS 16, Ind AS 103 App. C) are cited from knowledge and must be checked against the official text.
- Anything not confirmed from a primary source is **Unverified**.

### G.1 Six separate valuation dimensions
| Dimension | UAE | India | Book value's role |
|---|---|---|---|
| 1. Commercial price | Freely agreed by contract (C). | Freely agreed. Related-party transactions under Companies Act s.188 may need board or shareholder approval unless at arm's length and in the ordinary course (C/L). | Allowed as a commercial choice in both countries. |
| 2. Accounting carrying value | IFRS. The seller derecognises at carrying amount and books a gain or loss. The buyer records cost as the consideration paid. Consolidation eliminates intragroup unrealised profit (IFRS 10 B86). Transfers of a *business* under common control involve a policy choice (L/C). | Ind AS / AS mirror IFRS for asset sales. Ind AS 110 eliminates unrealised profit. Ind AS 103 Appendix C (pooling of interests at book value) applies **only to business combinations** under common control, not to simple asset sales (L). | Used for the seller's derecognition and for elimination. Not the buyer's cost unless the price equals book value. |
| 3. Indirect tax value | VAT Art. 36: open market value applies when related parties transact below market value **and** the recipient cannot fully recover input tax. Otherwise the consideration stands. VAT-group supplies are disregarded (L). | GST Rule 28: open market value; alternatively like-kind goods or the 90 % option. Second proviso: invoice value is deemed OMV where the recipient is eligible for full ITC. Rules 30/31 are fallbacks. Construction ITC blocks (s.17(5)) may defeat "full ITC" (L/P). | Not relevant as a tax base (only cost-based fallbacks). |
| 4. Transfer pricing | CT Art. 34 arm's length; Art. 35/36 related and connected persons; Art. 55 documentation. Thresholds in MD 97/2023 (AED 200m entity revenue / AED 3.15bn group revenue) (L; thresholds Unverified). | Income-tax Act 2025 (s.161–172, mapped from 1961 s.92–92F; **section numbers Unverified**). India↔UAE transfers are international transactions. Domestic transfers are in scope only in limited "specified domestic transaction" cases (L/P). | Not acceptable unless shown to be arm's length. |
| 5. Corporate-tax relief | Art. 26 Qualifying Group Relief: ≥75 % common ownership; both UAE-resident juridical persons (or a UAE PE); not exempt or QFZP; same year-end and accounting standards. The transferor elects, and the transfer is treated at **net book value for CT** with a 2-year clawback. Art. 27 Business Restructuring Relief applies to business transfers for shares. A Tax Group (Art. 40, 95 %) removes intragroup transactions (L/P). **Not available for UAE↔India transfers.** | No general relief for ordinary intercompany sales. Structural routes (amalgamation, demerger, specified holding–subsidiary capital-asset transfers) are noted only; 2025-Act mapping Unverified (P). | **Only here does an election matter:** in the UAE, NBV for CT computation when Art. 26/27 relief is validly elected, whatever the commercial price. |
| 6. Customs value (cross-border) | GCC transaction value; related-party price accepted if the relationship did not influence it (L/P). | Customs Valuation Rules 2007 r.3(3), same principle (L/P). | Not relevant unless it happens to be the transaction value and passes the test. |

### G.2 Correction to prior claim
The statement "UAE book-value pricing is available only under a qualifying-group election" is **wrong as stated**:
- A book-value **commercial price** can be agreed with or without any election.
- **Carrying value** is required for accounting derecognition and elimination, regardless of any election.
- The Art. 26/27 election affects **only the Corporate-Tax computation**: the transfer is treated at NBV for CT.
- The election does not set the VAT value or the transfer-pricing value.
- The election never applies to UAE↔India transfers.

### G.3 Open items for professional validation
1. How Art. 26 relief interacts with Art. 34 transfer pricing and the TP disclosure form.
2. Whether Art. 26 covers inventory/materials, and the election mechanics (per transaction or per period; revocable or not).
3. Exact clawback wording and anything added by MD 132/2023.
4. Current TP thresholds and penalties.
5. VAT Art. 36 recovery position on construction inputs; whether to form a VAT group.
6. GST second-proviso full-ITC test per line, given the s.17(5) blocks.
7. India 2025-Act section numbers, commencement and thresholds for TP.
8. India relief equivalents under the 2025 Act.
9. Whether moving equipment between entities is a **sale or a hire**. All six dimensions differ between the two.

### G.4 Candidate requirement (not approved)
| ID | Candidate requirement |
|---|---|
| ST-22 | Per intercompany line, store these separately:<br>• commercial price and basis, with agreement reference and related-party approval reference<br>• seller carrying amount, buyer cost and unrealised profit for elimination<br>• indirect-tax value, valuation rule, recipient full-recovery flag and VAT group ID<br>• transfer-pricing value, method and transaction class<br>• CT relief type, election flag, CT value and clawback expiry date<br>• customs value and procedure<br>• classification as sale or hire |

## Part E — RT-03: Uninstalled materials and reporting frameworks (research only, 2026-11-03)
**Source quality:**
- **IFRS 15:** read from the IFRS Foundation HTML.
- **Indian texts:** taken from ICAI pages and reproduced copies; the MCA Gazette PDFs could not be read.
- Items marked **Unverified** must be checked against mca.gov.in.

**This is not Finance policy.**

### E.1 IFRS 15 and Ind AS 115 (same wording for B18–B19 and ¶105–109)
| Ref | Finding | Class | PMS data implication |
|---|---|---|---|
| ¶40–41 | One progress method per performance obligation, applied consistently. Input or output methods are allowed. | L / C | Progress method stored per contract or performance obligation. |
| ¶44–45 | If progress cannot be measured reliably but costs are expected to be recovered, revenue is limited to cost incurred. | L | A "progress not reasonably measurable" flag per contract. |
| B19(a) | Costs that do not contribute to progress (e.g., significant inefficiencies such as wasted materials or labour not reflected in the price) are excluded from the input measure. | L / P | Cost lines are tagged waste / rework / abnormal inefficiency. This links to excess wastage (MW-11, MW-12). |
| B19(b) | A cost not proportionate to progress (e.g., an uninstalled good) gets revenue equal to its cost **only if all four conditions** are expected at contract inception: (i) the good is not distinct; (ii) the customer obtains control significantly before the related services; (iii) its cost is significant relative to total expected costs; (iv) it is procured from a third party and the entity is not significantly involved in its design or manufacture. | L (conditions); P ("significant") | For each material line, capture the four condition flags, the assessment date, control-transfer date, delivery date, install date and approver. Exclude the line with zero margin only if all four are true. |
| Example 19 (IE95–IE100) | Transaction price 5.0m. Expected costs 4.0m, of which elevators 1.5m. Costs to date 2.0m, of which elevators 1.5m. Progress = 0.5 / 2.5 = 20 %. Revenue = 20 % × (5.0 − 1.5) + 1.5 = 2.2m. | L (illustrative) | The ERP needs an adjusted price and cost pool, plus a zero-margin revenue amount. |
| Conditions not met | There is no zero-margin carve-out. The cost remains in the measure only if it depicts performance. If control has not passed to the customer, the material is typically inventory, not cost incurred. | P | Status per material line: *in progress measure* / *excluded, zero margin* / *held as inventory*. |
| ¶105–109 | Contract asset (right conditioned on something other than passage of time), contract liability (consideration received or due before performance), and receivable (unconditional right) are presented separately. | L | The PMS supplies cumulative billing, certified amounts, retention and revenue to date. The ERP derives the classification. Retention may be a contract asset where it is conditional (P). |
| Ind AS 115 specifics | Effective for periods from 1 Apr 2018. The reported carve-out on penalties (¶51AA) is **Unverified** in the current notified text. Whether the notified text includes the illustrative examples is **Unverified**. | P | Capture liquidated-damages exposure per contract. Finance decides its treatment. |

### E.2 AS 7 (Companies (Accounting Standards) Rules 2021; non-company entities by ICAI level)
| Para | Finding | Class | PMS data implication |
|---|---|---|---|
| 21, 29 | Revenue and costs are recognised by stage of completion. Allowed methods: cost ratio, survey of work performed, or physical proportion. | L / C | Method stored per contract. |
| 26 | Costs for future activity are an asset if recovery is probable. | L | Excluded costs are reported as an asset input, not as cost-to-date. |
| **30** | Costs for future activity are excluded from stage-of-completion costs. Examples: materials delivered to site but not yet installed, **unless made specially for the contract**, and advances to subcontractors. | L | Per material line: an *uninstalled* flag and a *made specially for contract* flag. Specially made materials are **included**. **No zero-margin rule.** |
| 31, 35 | If the outcome is not reliable, revenue is limited to recoverable cost. An expected loss is expensed immediately. | L | Loss flag when EAC exceeds contract revenue. |
| 38–43 | Disclosures: advances, retentions, and gross amounts due from or to customers. | L | Supply advances, retentions and progress billings. |

### E.3 Which framework applies (India)
| Rule | Finding | Class |
|---|---|---|
| Ind AS Rules 2015 r.4 | **Mandatory** for listed companies, and for unlisted companies with net worth ≥ ₹250 crore, phased from 2016 and 2017. Also applies to the holding, subsidiary, JV and associate companies of those. **Voluntary** adoption is allowed and is irrevocable. A company stays on Ind AS even if it later falls below the thresholds. Companies listed on an SME exchange are exempt. Overseas subsidiaries may use local GAAP for their standalone accounts. | L |
| Companies (AS) Rules 2021 | Companies not on Ind AS use AS. A small and medium company (SMC) is unlisted, has turnover ≤ ₹250 crore and borrowings ≤ ₹50 crore, and is not a holding or subsidiary of a non-SMC. SMCs get some relaxations. AS 7 applies in full. | L |
| ICAI non-company entities (from 1 Apr 2020) | LLPs, partnerships and sole proprietors are classified into Levels I–IV by turnover and borrowings. AS 7 applies. | L (ICAI) |
| Unincorporated JV / consortium / AOP | Treated as a non-company entity. If it is a jointly controlled operation, each venturer accounts for its share. | P |
| Indian branch of a foreign company | Which framework applies is **Unverified**. | P |
| NBFCs, banks, insurers | Follow separate phasing or regulator rules. Noted only. | P |

### E.4 UAE (note only)
- IFRS is required. For Corporate Tax, IFRS for SMEs may be used where revenue ≤ AED 50m (MD 114/2023).
- IFRS for SMEs has no B19(b)-equivalent wording. Its revenue section was revised in 2025, effective 2027 (**Unverified** detail).

### E.5 Open items for Finance
1. Whether the Ind AS 115 penalties carve-out exists, and the policy on liquidated damages.
2. Thresholds for "significant" under B19(a) and B19(b)(iii).
3. Whether an exclusion persists after the goods are installed.
4. Evidence required to show that materials were "made specially for the contract" under AS 7.
5. Whether retention is a contract asset or a receivable, decided by contract clause.
6. Framework for any Indian branch of a foreign company, and for consortium/AOP entities.
7. Uninstalled-materials policy for UAE entities on IFRS for SMEs.
8. NBFC phase dates and Schedule III line mapping (note only).

### E.6 Candidate requirements (not approved)
| ID | Candidate requirement |
|---|---|
| ST-19 | Per legal entity, store:<br>• reporting framework: IFRS, IFRS for SMEs, Ind AS, AS (company) or AS (non-company, Level I–IV)<br>• effective-from date<br>• whether adopted voluntarily<br>• an irreversibility lock<br>• yearly SMC/level status<br>• the parent group framework |
| ST-20 | Per material line, store:<br>• uninstalled flag, delivery date, install date and control-transfer date<br>• the four IFRS/Ind AS B19(b) condition flags, with assessment date and approver<br>• the AS 7 "made specially for the contract" flag<br>• resulting status: *in progress measure* / *excluded, zero margin* / *held as inventory* (Finance-approved) |
| ST-21 | Per contract, store:<br>• progress method<br>• "progress not reasonably measurable" flag<br>• liquidated-damages exposure<br>• loss flag<br>• cost lines tagged as waste / rework / inefficiency, so they can be excluded under B19(a) |

## Part F — RT-05a: Data protection, records and hosting (re-verification, 2026-11-03)
**Tags**
- **S** statutory
- **K** contractual
- **R** recommended practice

**Verified column**
- **Yes:** primary text was read.
- **Partial:** an official listing or press release confirms the fact, but the Gazette text was not read.
- **Unverified:** secondary sources only, or from memory.

**Limits of this research**
- Several official portals returned HTTP 403.
- DPDP calendar dates below are **calculated** from the "one year" and "eighteen months" wording, counted from a Gazette publication date of 13 Nov 2025. The notifications do not print these calendar dates. PIB states 14 Nov 2025, so there is a **one-day conflict to verify**.

### F.1 India
| Item | Tag | Exact effective date / transition | PMS hosting implication | Verified? |
|---|---|---|---|---|
| DPDP Act 2023 commencement (G.S.R. 843(E), 13 Nov 2025) | S | **13 Nov 2025:** definitions, Board and some other provisions.<br>**13 Nov 2026:** consent-manager provisions (s.6(9), s.27(1)(d)).<br>**13 May 2027:** core obligations (ss.3–17 and others). | Design now for notice, consent, security safeguards, breach notice and erasure, which are binding from 13 May 2027. | Section list **Unverified** (sources conflict). Phasing Partial. |
| DPDP Rules 2025 (G.S.R. 846(E); corrigendum G.S.R. 892(E), 10 Dec 2025) | S | **13 Nov 2025:** rules 1, 2, 17–21.<br>**13 Nov 2026:** rule 4.<br>**13 May 2027:** rules 3, 5–16, 22, 23 (security incl. log retention ≥ 1 year; 72-hour breach notice; retention; SDF duties). | Security, log-retention and breach-notice capability needed by 13 May 2027. | Grouping **Unverified**. Phase Partial. |
| Proposed compression of DPDP timelines (consultation Jan–Feb 2026) | — | **No notified amendment found.** | Watch item. | **Unverified** |
| Cross-border transfer (s.16; rule 15; rule 13(4) for significant data fiduciaries, SDFs) | S | From 13 May 2027. No restriction list found. | Transfers allowed by default. Keep the ability to pin data to an India region in case of SDF notification or government restriction (R). | Partial / **Unverified** |
| Companies (Accounts) Rules r.3(5) (G.S.R. 624(E)) | S | In force on Gazette publication (5 Aug 2022; one source shows 11 Aug 2022). | **Daily backup of books kept electronically on servers physically in India**, if PMS records are books of account (validation C1). Report service-provider details to the Registrar of Companies (RoC) where applicable. | **Unverified** (Gazette not read) |
| Companies (Accounts) Rules r.3(1) audit trail | S | Financial years beginning on or after **1 Apr 2023**. | Always-on, non-disableable edit log if the PMS holds books of account. | **Unverified** (notification numbers) |
| Companies Act s.128(5) | S | Since 1 Apr 2014 | Keep books for 8 financial years. | **Unverified** (text not re-read) |
| CERT-In Directions 20(3)/2022 (28 Apr 2022) | S | Effective **27 Jun 2022** (60 days after issue). An MSME extension to 25 Sep 2022 is reported. | Report incidents **within 6 hours**. Synchronise clocks to NIC/NPL NTP. Keep logs for a **rolling 180 days within Indian jurisdiction**. Designate a point of contact. | Yes (directions text). Extension and offshore-log FAQ **Unverified**. |
| GST record retention (CGST s.36) | S | 72 months from the annual-return due date | No storage-location rule found. | **Unverified** (memory) |

### F.2 UAE
| Item | Tag | Exact effective date / transition | PMS hosting implication | Verified? |
|---|---|---|---|---|
| PDPL (Federal Decree-Law 45/2021) | S | Issued 20 Sep 2021; Gazette No. 712, 26 Sep 2021; **in force 2 Jan 2022**. Organisations have 6 months to comply **after** the Executive Regulations are issued. | Data-subject rights and security apply now. | Partial |
| PDPL Executive Regulations | S | **Not found issued** as of Sep–Oct 2026 sources. | No compliance-grace clock has started. Monitor. | **Unverified** |
| PDPL Arts. 22–23 (cross-border) | S | In force; no adequacy list or federal standard clauses found. | Contractual safeguards (intra-group and processor data-processing agreements) are the practical route. | **Unverified** |
| UAE Data Office (Decree-Law 44/2021); new Federal Authority for AI and Data announced 14 Jun 2026 | S | The regulator is changing. The founding instrument has not been confirmed. | Monitor. | **Unverified** |
| DIFC DP Law 5/2020 | S | In force 1 Jul 2020; enforceable 1 Oct 2020. Amended 2022, and by Law No. 1 of 2025 (effective 15 Jul 2025; private right of action). | Applies only to DIFC entities. Transfers need adequacy, standard contractual clauses (SCCs) or binding corporate rules (BCRs). | **Unverified** |
| ADGM DP Regulations 2021 | S | Enacted 14 Feb 2021. Transition: 12 months for existing entities, 6 months for new entities. No transition for transfer rules. | Applies only to ADGM entities. | **Unverified** |
| Health data (Federal Law 2/2019, Art. 13) | S | In force May 2019 (day unverified). Exceptions under Ministerial Decision 51/2021 (Gazette 16 May 2021). | Only relevant if health data is stored. **Out of PMS scope (NF-08)**. | **Unverified** |
| Dubai ISR (Resolution 13/2012; DESC) | S for Dubai government / K for suppliers | Version and date unverified | Expect contract clauses with Dubai government clients. | Partial |
| DESC Cloud Service Provider standard | K | Not listed on the DESC page | Relevant only for Dubai government clients. | **Unverified** |
| UAE IA Regulation v1.1 (TDRA, Mar 2020) | S for government and critical entities / K | Mar 2020 | Control set used in government tenders. | Partial |
| UAE e-invoicing storage | S | Oct 2026 amendments reportedly **removed the in-UAE storage requirement** (retention per Tax Procedures Law; produce to FTA on request). Phase 1 ASP appointment by 30 Oct 2026; go-live 1 Jan 2027. | Archive must be retrievable for the statutory period. Location is reportedly flexible. | **Unverified** (decision numbers) |

### F.3 Hosting implications (statutory items only; all subject to validation)
- **India (statutory, if applicable):**
  - Daily backup of books on servers in India (C1).
  - CERT-In logs kept 180 days in India, with clock sync and 6-hour incident reporting.
  - Audit trail and 8-year retention for books.
  - DPDP security, retention and breach obligations from 13 May 2027.
- **UAE:**
  - No general onshore localisation rule was found.
  - Use contractual transfer safeguards.
  - Localise only where health law applies (out of scope) or where a government or critical-entity contract requires it.
- **Recommended practice (R), not a legal requirement:** capability to keep India-entity and UAE-entity data in separate regions, with residency pinning.
  - Whether a candidate product supports this is a **vendor fact** (`hosting-research.md`, VD-30).

### F.4 Open items for professional validation
1. DPDP Gazette date (13 vs 14 Nov 2025) and how the 1-year and 18-month periods are computed.
2. Exact section and rule lists, including the corrigendum.
3. Whether the proposed timeline compression has been notified, and whether the company could be designated an SDF.
4. Wording and date of rule 3(5) (G.S.R. 624(E)), and whether an Indian cloud-region backup satisfies "servers physically located in India".
5. The CERT-In FAQ position on offshore logs.
6. PDPL Executive Regulations status, and the powers of the new Authority.
7. Whether any group entity is in DIFC or ADGM.
8. UAE e-invoicing decision numbers and archive retention (5 years, or 15 for real-estate records).
9. DESC ISR version and the need for CSP certification.

## Part C — Cross-cutting implications for PMS requirements (analysis; not approved)
These are **candidate requirements** that follow from the research. Each needs approval before it enters requirements.md as anything other than **[RT]**.

| ID | Candidate requirement | India basis | UAE basis |
|---|---|---|---|
| ST-01 | Effective-dated tax codes, rates and statutory on-cost tables held as master data (ERP/tax service owned); no hard-coded rates. | A.1 GST 2.0, A.4 EPF ceiling | B.1, B.4 |
| ST-02 | Party tax identifiers on vendors, customers and sites (GSTIN/PAN/Udyam; TRN), owned by ERP and referenced by PMS. | A.1, A.3 | B.1 |
| ST-03 | Separate dates on certificates and bills: work-done/service period, certification date, invoice date, payment due date, payment date. | A.1 time of supply | B.1 Art. 26 |
| ST-04 | Separate fields for gross certified value, retention held, retention released, retention invoiced, advance paid/received and advance recovered. | A.1 retention, advances | B.1 retentions, advances; B.7 |
| ST-05 | Per-line tax attributes: SAC/HSN, reverse-charge flag, credit eligibility (eligible/blocked/capital), recoverable vs non-recoverable portion — recoverable tax excluded from project cost (aligns with Q6a-P3). | A.1 | B.1 |
| ST-06 | Site attributes: state/emirate, GSTIN of site registration, free-zone/DZ status, data regime. | A.1 place of supply, distinct persons | B.1 DZ, B.6 |
| ST-07 | Inter-site and inter-entity transfers carry transfer documents (tax invoice or delivery challan; e-way bill data) and transfer value. | A.1 distinct persons, e-way bill | B.2/B.3 inter-company |
| ST-08 | Withholding attributes on commitments and vendors (TDS table item, lower-deduction certificate and usage) — India only; UAE WHT 0% (Unverified). | A.3 | B.3 |
| ST-09 | Structured billing and certificate data handed to ERP/e-invoicing provider (IRN for India; PINT AE fields for UAE); PMS never issues statutory e-invoices itself. | A.2 | B.2 |
| ST-10 | Timesheet hour types and limits configurable per country (normal, OT, night OT, rest day, public holiday; daily/weekly caps; seasonal midday-ban window), with country-specific multipliers supplied by HR/payroll. | A.4 | B.4 |
| ST-11 | Labour cost-rate components configurable per country and effective-dated (statutory and policy on-costs), approved under Q7-F1. | A.4 | B.4 |
| ST-12 | Project statutory registrations and levies (e.g., India OSH establishment registration; BOCW cess line per project). | A.4 | — |
| ST-13 | Subcontractor compliance records (India contractor licence, workforce count, wage/PF/ESI evidence; UAE guarantees/insurances per contract). | A.4 | B.7 |
| ST-14 | Immutable audit trail and retention configurable per country (India ≥ 8 years; UAE up to 15 years for real-estate records — to be unified by professional validation). | A.5 | B.1, B.3, B.5 |
| ST-15 | Revenue-recognition data: cost to date, ETC, uninstalled-material flag, approved vs pending variations, contract assets/liabilities, onerous-contract flag; reporting basis per entity (Ind AS / AS / IFRS). | A.5 | B.5 |
| ST-16 | Personal-data controls: role-based access, purpose/retention rules, breach-reporting support, no health data in PMS, configurable hosting region per entity. | A.6 | B.6 |
| ST-17 | Country rules are configuration per country; adding a country requires its own statutory research (never inherits India/UAE rules). | Q1-R1 | Q1-R1 |
